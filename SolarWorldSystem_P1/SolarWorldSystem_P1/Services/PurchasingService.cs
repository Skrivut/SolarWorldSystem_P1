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

        public Supplier AddSupplier(string name, string contactInfo)
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
                    IsActive = true
                };
                db.Suppliers.InsertOnSubmit(supplier);
                db.SubmitChanges();
                return supplier;
            }
        }

        // ---- Purchase orders ----
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

        // Creates a Draft order. rfId is optional (set it when ordering to cover a requisition shortage).
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
                if (rfId.HasValue && !db.Requisitions.Any(r => r.RfId == rfId.Value))
                    throw new InvalidOperationException("The requisition was not found.");
                if (db.Items.Count(i => itemIds.Contains(i.ItemId)) != itemIds.Count)
                    throw new InvalidOperationException("One or more items were not found.");

                var order = new PurchaseOrder
                {
                    PoNumber = NextPoNumber(db),
                    SupplierId = supplierId,
                    RfId = rfId,
                    Status = "Draft",
                    DateCreated = DateTime.Now,
                    ExpectedDate = expectedDate,
                    Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks.Trim()
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

        public void Place(int purchaseOrderId)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == purchaseOrderId);
                if (order == null || order.Status != "Draft")
                    throw new InvalidOperationException("Only a draft order can be placed.");
                order.Status = "Ordered";
                db.SubmitChanges();
            }
        }

        public void Cancel(int purchaseOrderId)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == purchaseOrderId);
                if (order == null || (order.Status != "Draft" && order.Status != "Ordered"))
                    throw new InvalidOperationException("This order can no longer be cancelled.");
                if (db.PurchaseOrderLines.Any(l => l.PurchaseOrderId == purchaseOrderId && l.QtyReceived > 0))
                    throw new InvalidOperationException("Items were already received for this order.");
                if (db.Deliveries.Any(d => d.PurchaseOrderId == purchaseOrderId && (d.Status == "Scheduled" || d.Status == "InTransit")))
                    throw new InvalidOperationException("Cancel its scheduled deliveries first.");
                order.Status = "Cancelled";
                db.SubmitChanges();
            }
        }

        // Stock items to pick from when building an order.
        public List<Item> GetItems()
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
                return db.Items.OrderBy(i => i.ItemCode).ToList();
        }

        // Highest existing number + 1, so deleted rows can't cause a duplicate.
        private static string NextPoNumber(ErpDataContext db)
        {
            var prefix = "PO-" + DateTime.Now.Year + "-";
            var last = db.PurchaseOrders.Where(o => o.PoNumber.StartsWith(prefix))
                                        .Select(o => o.PoNumber).OrderByDescending(n => n).FirstOrDefault();
            var seq = last == null ? 0 : int.Parse(last.Substring(prefix.Length));
            return prefix + (seq + 1).ToString("D4");
        }

        private static void RequireRole(params string[] roles)
        {
            if (!roles.Contains(Session.Role)) throw new UnauthorizedAccessException("Your role is not allowed to perform this action.");
        }
    }
}