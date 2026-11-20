using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
    public class PurchaseLineInput
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }
        public decimal? UnitCost { get; set; }
    }

    // An approved requisition that is short on stock and has no open purchase order yet.
    public class RfShortageView
    {
        public int RfId { get; set; }
        public string RfNumber { get; set; }
        public string ProjectName { get; set; }
        public int TotalShort { get; set; }
    }

    // One short item of a requisition: what still has to be bought.
    public class ShortageLineView
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int QtyShort { get; set; }
    }

    // Purchase order flow: Draft -> PendingApproval -> (owner approves) Ordered -> PartiallyReceived -> Completed.
    // Approving sends the PO to the supplier and creates the driver's pickup task.
    public class PurchasingService
    {
        // ---- Suppliers ----
        public List<Supplier> GetSuppliers(bool activeOnly)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var query = db.Suppliers.AsQueryable();
                if (activeOnly) query = query.Where(s => s.IsActive);
                return query.OrderBy(s => s.SupplierName).ToList();
            }
        }

        public Supplier AddSupplier(string name, string contactInfo, string address)
        {
            RequireRole("Owner", "Staff");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Supplier name is required.");
            name = name.Trim();
            using (var db = new ErpDataContext())
            {
                if (db.Suppliers.Any(s => s.SupplierName == name))
                    throw new InvalidOperationException("That supplier already exists.");
                var supplier = new Supplier
                {
                    SupplierName = name,
                    ContactInfo = string.IsNullOrWhiteSpace(contactInfo) ? null : contactInfo.Trim(),
                    Address = string.IsNullOrWhiteSpace(address) ? null : Clip(address, 255),
                    IsActive = true
                };
                db.Suppliers.InsertOnSubmit(supplier);
                db.SubmitChanges();
                return supplier;
            }
        }

        // The pickup address tells the driver where to collect the goods.
        public void SetSupplierAddress(int supplierId, string address)
        {
            RequireRole("Owner", "Staff");
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Type the supplier's pickup address.");
            using (var db = new ErpDataContext())
            {
                var supplier = db.Suppliers.SingleOrDefault(s => s.SupplierId == supplierId);
                if (supplier == null) throw new InvalidOperationException("The supplier was not found.");
                supplier.Address = Clip(address, 255);
                db.SubmitChanges();
            }
        }

        // ---- Reading orders ----
        public List<PurchaseOrder> GetOrders(string status)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var query = db.PurchaseOrders.AsQueryable();
                if (!string.IsNullOrWhiteSpace(status)) query = query.Where(o => o.Status == status);
                return query.OrderByDescending(o => o.DateCreated).ToList();
            }
        }

        public List<PurchaseOrderLine> GetLines(int purchaseOrderId)
        {
            RequireRole("Owner", "Staff", "Driver");
            using (var db = new ErpDataContext())
                return db.PurchaseOrderLines.Where(l => l.PurchaseOrderId == purchaseOrderId).ToList();
        }

        // Stock items to pick from when building an order.
        public List<Item> GetItems()
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
                return db.Items.OrderBy(i => i.ItemCode).ToList();
        }

        // ---- Buying for a requisition's shortage (the hand-off from the requisition module) ----
        // Approved requisitions that are short on stock and have no open purchase order yet.
        public List<RfShortageView> GetRequisitionsWithShortage()
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var coveredRfIds = db.PurchaseOrders
                    .Where(o => o.RfId != null && o.Status != "Cancelled" && o.Status != "Completed")
                    .Select(o => o.RfId.Value).ToList();
                return (from l in db.RequisitionItems
                        join r in db.Requisitions on l.RfId equals r.RfId
                        where l.QtyShortage > 0
                              && (r.Status == "Approved" || r.Status == "PendingFulfillment")
                              && !coveredRfIds.Contains(r.RfId)
                        group l by new { r.RfId, r.RfNumber, r.ProjectName } into g
                        orderby g.Key.RfNumber
                        select new RfShortageView
                        {
                            RfId = g.Key.RfId,
                            RfNumber = g.Key.RfNumber,
                            ProjectName = g.Key.ProjectName,
                            TotalShort = g.Sum(x => x.QtyShortage)
                        }).ToList();
            }
        }

        public List<ShortageLineView> GetShortageLines(int rfId)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                return (from l in db.RequisitionItems
                        join i in db.Items on l.ItemId equals i.ItemId
                        where l.RfId == rfId && l.QtyShortage > 0
                        select new ShortageLineView
                        {
                            ItemId = i.ItemId,
                            ItemCode = i.ItemCode,
                            ItemName = i.ItemName,
                            QtyShort = l.QtyShortage
                        }).ToList();
            }
        }

        // ---- Creating and moving an order along ----
        // Creates a Draft order. rfId is optional: set it when the order covers a requisition's shortage.
        public PurchaseOrder CreateOrder(int supplierId, int? rfId, DateTime? expectedDate, string remarks, List<PurchaseLineInput> lines)
        {
            RequireRole("Owner", "Staff");
            if (lines == null || lines.Count == 0) throw new ArgumentException("Add at least one item.");
            if (lines.Any(l => l.Qty <= 0)) throw new ArgumentException("Quantities must be greater than zero.");
            if (lines.Any(l => l.UnitCost.HasValue && l.UnitCost.Value < 0)) throw new ArgumentException("Unit cost cannot be negative.");
            var itemIds = lines.Select(l => l.ItemId).ToList();
            if (itemIds.Distinct().Count() != itemIds.Count) throw new ArgumentException("An item can only appear once per order.");

            using (var scope = new TransactionScope())
            using (var db = new ErpDataContext())
            {
                if (!db.Suppliers.Any(s => s.SupplierId == supplierId && s.IsActive))
                    throw new InvalidOperationException("Choose an active supplier.");
                if (db.Items.Count(i => itemIds.Contains(i.ItemId)) != itemIds.Count)
                    throw new InvalidOperationException("One or more items were not found.");

                if (rfId.HasValue)
                {
                    var rf = db.Requisitions.SingleOrDefault(r => r.RfId == rfId.Value);
                    if (rf == null || (rf.Status != "Approved" && rf.Status != "PendingFulfillment"))
                        throw new InvalidOperationException("Only an approved requisition can be ordered for.");
                    if (db.PurchaseOrders.Any(o => o.RfId == rfId.Value && o.Status != "Cancelled" && o.Status != "Completed"))
                        throw new InvalidOperationException("That requisition already has an open purchase order.");
                    var shortage = db.RequisitionItems.Where(l => l.RfId == rfId.Value && l.QtyShortage > 0).ToList();
                    foreach (var l in lines)
                    {
                        var s = shortage.SingleOrDefault(x => x.ItemId == l.ItemId);
                        if (s == null || l.Qty > s.QtyShortage)
                            throw new InvalidOperationException("An item is not short on that requisition, or the quantity is more than the shortage.");
                    }
                }

                var order = new PurchaseOrder
                {
                    PoNumber = NextPoNumber(db),
                    SupplierId = supplierId,
                    RfId = rfId,
                    Status = "Draft",
                    DateCreated = DateTime.Now,
                    CreatedBy = Session.UserName,
                    ExpectedDate = expectedDate,
                    Remarks = string.IsNullOrWhiteSpace(remarks) ? null : Clip(remarks, 255)
                };
                db.PurchaseOrders.InsertOnSubmit(order);
                db.SubmitChanges();

                foreach (var l in lines)
                    db.PurchaseOrderLines.InsertOnSubmit(new PurchaseOrderLine
                    {
                        PurchaseOrderId = order.PurchaseOrderId,
                        ItemId = l.ItemId,
                        QtyOrdered = l.Qty,
                        QtyReceived = 0,
                        UnitCost = l.UnitCost
                    });
                db.SubmitChanges();
                scope.Complete();
                return order;
            }
        }

        // Staff sends a draft to the owner for approval. The supplier needs a pickup address first.
        public void SubmitForApproval(int purchaseOrderId)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == purchaseOrderId);
                if (order == null || order.Status != "Draft")
                    throw new InvalidOperationException("Only a draft order can be submitted for approval.");
                var supplier = db.Suppliers.Single(s => s.SupplierId == order.SupplierId);
                if (string.IsNullOrWhiteSpace(supplier.Address))
                    throw new InvalidOperationException("Add the supplier's pickup address first (type it and press Save address), so the driver knows where to collect the goods.");
                order.Status = "PendingApproval";
                db.SubmitChanges();
            }
        }

        // The owner approves: the PO is sent to the supplier (status Ordered) and the driver's pickup task is created.
        // If the order covers a requisition, that requisition moves to PendingFulfillment.
        public void Approve(int purchaseOrderId)
        {
            RequireRole("Owner");
            int? rfToMark = null;
            var options = new TransactionOptions { IsolationLevel = IsolationLevel.Serializable };
            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == purchaseOrderId);
                if (order == null || order.Status != "PendingApproval")
                    throw new InvalidOperationException("Only an order waiting for approval can be approved.");
                var supplier = db.Suppliers.Single(s => s.SupplierId == order.SupplierId);
                if (string.IsNullOrWhiteSpace(supplier.Address))
                    throw new InvalidOperationException("The supplier has no pickup address. Ask staff to add it.");

                order.Status = "Ordered";
                order.ApprovedBy = Session.UserName;
                order.DateApproved = DateTime.Now;
                DeliveryService.NewPickupTask(db, order, supplier.Address, null);

                if (order.RfId.HasValue)
                {
                    var rf = db.Requisitions.SingleOrDefault(r => r.RfId == order.RfId.Value);
                    if (rf != null && rf.Status == "Approved") rfToMark = rf.RfId;
                }
                db.SubmitChanges();
                scope.Complete();
            }

            // Done after the order is saved, using the requisition module's own method.
            if (rfToMark.HasValue)
            {
                try { new RequisitionService().MarkPendingFulfillment(rfToMark.Value); }
                catch (InvalidOperationException) { /* the requisition changed in the meantime; the order itself is fine */ }
            }
        }

        public void Reject(int purchaseOrderId, string remarks)
        {
            RequireRole("Owner");
            if (string.IsNullOrWhiteSpace(remarks)) throw new ArgumentException("Type the reason for rejecting this order.");
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == purchaseOrderId);
                if (order == null || order.Status != "PendingApproval")
                    throw new InvalidOperationException("Only an order waiting for approval can be rejected.");
                order.Status = "Cancelled";
                order.Remarks = Clip("Rejected by owner: " + remarks.Trim(), 255);
                db.SubmitChanges();
            }
        }

        // Cancels an order nothing has been received for. Pickups the driver has not started are cancelled with it.
        public void Cancel(int purchaseOrderId)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == purchaseOrderId);
                if (order == null || (order.Status != "Draft" && order.Status != "PendingApproval" && order.Status != "Ordered"))
                    throw new InvalidOperationException("This order can no longer be cancelled.");
                if (db.PurchaseOrderLines.Any(l => l.PurchaseOrderId == purchaseOrderId && l.QtyReceived > 0))
                    throw new InvalidOperationException("Items were already received for this order.");
                var tasks = db.Deliveries.Where(d => d.PurchaseOrderId == purchaseOrderId).ToList();
                if (tasks.Any(t => t.Status == "PickedUp" || t.Status == "Delivered"))
                    throw new InvalidOperationException("The goods are already with the driver or at the office. Receive them first.");
                foreach (var t in tasks.Where(t => t.Status == "Pending")) t.Status = "Cancelled";
                order.Status = "Cancelled";
                db.SubmitChanges();
            }
        }

        // Staff followed up the supplier about the balance or a replacement: a new pickup task goes to the driver.
        // The PO stays open until everything on it has been accepted.
        public void FollowUp(int purchaseOrderId, string notes)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == purchaseOrderId);
                if (order == null || (order.Status != "Ordered" && order.Status != "PartiallyReceived"))
                    throw new InvalidOperationException("Only an open purchase order can be followed up.");
                if (!db.PurchaseOrderLines.Any(l => l.PurchaseOrderId == purchaseOrderId && l.QtyReceived < l.QtyOrdered))
                    throw new InvalidOperationException("Nothing is outstanding on this order.");
                if (db.Deliveries.Any(d => d.PurchaseOrderId == purchaseOrderId
                        && (d.Status == "Pending" || d.Status == "PickedUp" || d.Status == "Delivered")))
                    throw new InvalidOperationException("There is already an active pickup for this order.");
                var supplier = db.Suppliers.Single(s => s.SupplierId == order.SupplierId);
                if (string.IsNullOrWhiteSpace(supplier.Address))
                    throw new InvalidOperationException("The supplier has no pickup address. Add it first.");

                DeliveryService.NewPickupTask(db, order, supplier.Address,
                    string.IsNullOrWhiteSpace(notes) ? "Follow-up pickup for the balance or replacement." : notes);
                db.SubmitChanges();
            }
        }

        // ---- Helpers ----
        // Highest existing number + 1, so deleted rows can't cause a duplicate.
        private static string NextPoNumber(ErpDataContext db)
        {
            var prefix = "PO-" + DateTime.Now.Year + "-";
            var last = db.PurchaseOrders.Where(o => o.PoNumber.StartsWith(prefix))
                                        .Select(o => o.PoNumber).OrderByDescending(n => n).FirstOrDefault();
            var seq = last == null ? 0 : int.Parse(last.Substring(prefix.Length));
            return prefix + (seq + 1).ToString("D4");
        }

        private static string Clip(string text, int max)
        {
            text = text.Trim();
            return text.Length <= max ? text : text.Substring(0, max);
        }

        private static void RequireRole(params string[] roles)
        {
            if (!roles.Contains(Session.Role)) throw new UnauthorizedAccessException("Your role is not allowed to perform this action.");
        }
    }
}