using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
    // One purchase order line with its item name, as shown on the delivery screen.
    public class ReceiveLineView
    {
        public int PurchaseOrderLineId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyReceived { get; set; }
        public int QtyRemaining { get { return QtyOrdered - QtyReceived; } }
    }

    // What staff typed for one item on the receiving report (RR).
    public class ReceiveInput
    {
        public int PurchaseOrderLineId { get; set; }
        public int QtyAccepted { get; set; }
        public int QtyRejected { get; set; }
        public string RejectReason { get; set; }
        public string DiscrepancyAction { get; set; }   // "Return" or "Replacement", needed when something is rejected
    }

    // A Delivery row is the driver's pickup task for a purchase order:
    // Pending -> PickedUp (driver has the goods and the supplier's DR) -> Delivered (at the office)
    // -> Received (staff finished the receiving report). Pending tasks can be Cancelled.
    public class DeliveryService
    {
        public const string DropOff = "Office / Warehouse";

        // ---- Reading ----
        public List<Delivery> GetDeliveries(string status)
        {
            RequireRole("Owner", "Staff", "Driver");
            using (var db = new ErpDataContext())
            {
                var query = db.Deliveries.AsQueryable();
                if (!string.IsNullOrWhiteSpace(status)) query = query.Where(d => d.Status == status);
                return query.OrderByDescending(d => d.DeliveryId).ToList();
            }
        }

        // The saved receiving report lines of a delivery.
        public List<DeliveryLine> GetLines(int deliveryId)
        {
            RequireRole("Owner", "Staff", "Driver");
            using (var db = new ErpDataContext())
                return db.DeliveryLines.Where(l => l.DeliveryId == deliveryId).ToList();
        }

        // Purchase order numbers by id, so the screen can show "PO-2026-0001" instead of an id.
        public Dictionary<int, string> GetOrderNumbers()
        {
            RequireRole("Owner", "Staff", "Driver");
            using (var db = new ErpDataContext())
                return db.PurchaseOrders.ToDictionary(o => o.PurchaseOrderId, o => o.PoNumber);
        }

        // The purchase order lines behind a delivery, with item names and what is still outstanding.
        public List<ReceiveLineView> GetReceivableLines(int deliveryId)
        {
            RequireRole("Owner", "Staff", "Driver");
            using (var db = new ErpDataContext())
            {
                var delivery = db.Deliveries.SingleOrDefault(d => d.DeliveryId == deliveryId);
                if (delivery == null) return new List<ReceiveLineView>();
                return (from l in db.PurchaseOrderLines
                        join i in db.Items on l.ItemId equals i.ItemId
                        where l.PurchaseOrderId == delivery.PurchaseOrderId
                        select new ReceiveLineView
                        {
                            PurchaseOrderLineId = l.PurchaseOrderLineId,
                            ItemCode = i.ItemCode,
                            ItemName = i.ItemName,
                            QtyOrdered = l.QtyOrdered,
                            QtyReceived = l.QtyReceived
                        }).ToList();
            }
        }

        // ---- Driver steps ----
        // The driver collected the goods from the supplier and got the supplier's delivery receipt (DR).
        public void PickUp(int deliveryId, string drNumber)
        {
            RequireRole("Driver", "Staff", "Owner");
            if (string.IsNullOrWhiteSpace(drNumber))
                throw new ArgumentException("Enter the supplier's delivery receipt (DR) number.");
            using (var db = new ErpDataContext())
            {
                var delivery = db.Deliveries.SingleOrDefault(d => d.DeliveryId == deliveryId);
                if (delivery == null || delivery.Status != "Pending")
                    throw new InvalidOperationException("This pickup is not waiting for the driver.");
                delivery.Status = "PickedUp";
                delivery.SupplierDrNumber = Clip(drNumber, 50);
                delivery.PickedUpDate = DateTime.Now;
                delivery.DriverName = Session.UserName;
                db.SubmitChanges();
            }
        }

        // The driver brought the goods to the office or warehouse.
        public void DeliverToOffice(int deliveryId)
        {
            RequireRole("Driver", "Staff", "Owner");
            using (var db = new ErpDataContext())
            {
                var delivery = db.Deliveries.SingleOrDefault(d => d.DeliveryId == deliveryId);
                if (delivery == null || delivery.Status != "PickedUp")
                    throw new InvalidOperationException("The goods have not been picked up yet.");
                delivery.Status = "Delivered";
                delivery.DeliveredDate = DateTime.Now;
                db.SubmitChanges();
            }
        }

        // Cancels a pickup the driver has not started.
        public void Cancel(int deliveryId)
        {
            RequireRole("Staff", "Owner");
            using (var db = new ErpDataContext())
            {
                var delivery = db.Deliveries.SingleOrDefault(d => d.DeliveryId == deliveryId);
                if (delivery == null || delivery.Status != "Pending")
                    throw new InvalidOperationException("Only a pickup the driver has not started can be cancelled.");
                delivery.Status = "Cancelled";
                db.SubmitChanges();
            }
        }

        // ---- Staff step: the receiving report (RR) ----
        // Staff checked the goods against the PO and the DR. Accepted quantities go into stock; rejected
        // quantities are recorded as a discrepancy and never counted. When every line of the PO is fully
        // received the PO is closed and the items are allocated to the requisition that was short.
        // Returns the new RR number.
        public string Receive(int deliveryId, List<ReceiveInput> lines, string notes)
        {
            RequireRole("Staff", "Owner");
            lines = lines ?? new List<ReceiveInput>();
            if (lines.Any(l => l.QtyAccepted < 0 || l.QtyRejected < 0))
                throw new ArgumentException("Quantities cannot be negative.");
            var entered = lines.Where(l => l.QtyAccepted > 0 || l.QtyRejected > 0).ToList();
            if (entered.Count == 0)
                throw new ArgumentException("Enter the accepted or rejected quantity for at least one item.");
            foreach (var l in entered.Where(l => l.QtyRejected > 0))
            {
                if (string.IsNullOrWhiteSpace(l.RejectReason))
                    throw new ArgumentException("Write the reason (rejected, damaged or wrong item) for every rejected quantity.");
                if (l.DiscrepancyAction != "Return" && l.DiscrepancyAction != "Replacement")
                    throw new ArgumentException("Choose Return or Replacement for every rejected quantity.");
            }

            var options = new TransactionOptions { IsolationLevel = IsolationLevel.Serializable };
            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            using (var db = new ErpDataContext())
            {
                var delivery = db.Deliveries.SingleOrDefault(d => d.DeliveryId == deliveryId);
                if (delivery == null || delivery.Status != "Delivered")
                    throw new InvalidOperationException("Only goods the driver has delivered to the office can be received.");
                var order = db.PurchaseOrders.Single(o => o.PurchaseOrderId == delivery.PurchaseOrderId);
                if (order.Status != "Ordered" && order.Status != "PartiallyReceived")
                    throw new InvalidOperationException("The purchase order is no longer open for receiving.");
                var orderLines = db.PurchaseOrderLines.Where(l => l.PurchaseOrderId == order.PurchaseOrderId).ToList();

                foreach (var input in entered)
                {
                    var line = orderLines.SingleOrDefault(l => l.PurchaseOrderLineId == input.PurchaseOrderLineId);
                    if (line == null || input.QtyAccepted + input.QtyRejected > line.QtyOrdered - line.QtyReceived)
                        throw new ArgumentException("A quantity is more than what is still outstanding on the purchase order.");

                    line.QtyReceived += input.QtyAccepted;   // rejected goods are never counted
                    if (input.QtyAccepted > 0)
                        db.Items.Single(i => i.ItemId == line.ItemId).QtyOnHand += input.QtyAccepted;

                    db.DeliveryLines.InsertOnSubmit(new DeliveryLine
                    {
                        DeliveryId = delivery.DeliveryId,
                        PurchaseOrderLineId = line.PurchaseOrderLineId,
                        QtyDelivered = input.QtyAccepted + input.QtyRejected,
                        QtyAccepted = input.QtyAccepted,
                        QtyRejected = input.QtyRejected,
                        RejectReason = input.QtyRejected > 0 ? Clip(input.RejectReason, 255) : null,
                        DiscrepancyAction = input.QtyRejected > 0 ? input.DiscrepancyAction : null
                    });
                }

                var reportNumber = NextReportNumber(db);
                delivery.Status = "Received";
                delivery.ReportNumber = reportNumber;
                delivery.ReceivedBy = Session.UserName;
                delivery.ReceivedDate = DateTime.Now;
                delivery.Notes = string.IsNullOrWhiteSpace(notes) ? null : Clip(notes, 255);

                var complete = orderLines.All(l => l.QtyReceived >= l.QtyOrdered);
                if (complete)
                {
                    order.Status = "Completed";
                    order.DateClosed = DateTime.Now;
                    AllocateToRequisition(db, order, orderLines);
                }
                else
                {
                    order.Status = orderLines.Any(l => l.QtyReceived > 0) ? "PartiallyReceived" : "Ordered";
                }

                db.SubmitChanges();
                scope.Complete();
                return reportNumber;
            }
        }

        // ---- Used by PurchasingService ----
        // Adds a pickup task for a purchase order to the context. The caller saves it.
        internal static Delivery NewPickupTask(ErpDataContext db, PurchaseOrder order, string pickupAddress, string notes)
        {
            var task = new Delivery
            {
                DeliveryNumber = NextDeliveryNumber(db),
                PurchaseOrderId = order.PurchaseOrderId,
                PickupAddress = pickupAddress,
                DeliveryAddress = DropOff,
                Status = "Pending",
                ExpectedDate = order.ExpectedDate,
                Notes = string.IsNullOrWhiteSpace(notes) ? null : Clip(notes, 255)
            };
            db.Deliveries.InsertOnSubmit(task);
            return task;
        }

        // ---- Helpers ----
        // When the PO closes, the stock that arrived for it covers the requisition's shortage:
        // the shortage turns into reserved stock. Only for requisitions that are still open.
        private static void AllocateToRequisition(ErpDataContext db, PurchaseOrder order, List<PurchaseOrderLine> orderLines)
        {
            if (!order.RfId.HasValue) return;
            var rf = db.Requisitions.SingleOrDefault(r => r.RfId == order.RfId.Value);
            if (rf == null || (rf.Status != "Approved" && rf.Status != "PendingFulfillment")) return;

            var rfLines = db.RequisitionItems.Where(l => l.RfId == rf.RfId && l.QtyShortage > 0).ToList();
            foreach (var rfLine in rfLines)
            {
                var received = orderLines.Where(l => l.ItemId == rfLine.ItemId).Sum(l => l.QtyReceived);
                var item = db.Items.Single(i => i.ItemId == rfLine.ItemId);
                var free = Math.Max(0, item.QtyOnHand - item.QtyReserved);
                var allocate = Math.Min(rfLine.QtyShortage, Math.Min(received, free));
                if (allocate <= 0) continue;
                rfLine.QtyReserved += allocate;
                rfLine.QtyShortage -= allocate;
                item.QtyReserved += allocate;
            }
        }

        // Highest existing number + 1, so deleted rows can't cause a duplicate.
        private static string NextDeliveryNumber(ErpDataContext db)
        {
            var prefix = "DEL-" + DateTime.Now.Year + "-";
            var last = db.Deliveries.Where(d => d.DeliveryNumber.StartsWith(prefix))
                                    .Select(d => d.DeliveryNumber).OrderByDescending(n => n).FirstOrDefault();
            var seq = last == null ? 0 : int.Parse(last.Substring(prefix.Length));
            return prefix + (seq + 1).ToString("D4");
        }

        private static string NextReportNumber(ErpDataContext db)
        {
            var prefix = "RR-" + DateTime.Now.Year + "-";
            var last = db.Deliveries.Where(d => d.ReportNumber != null && d.ReportNumber.StartsWith(prefix))
                                    .Select(d => d.ReportNumber).OrderByDescending(n => n).FirstOrDefault();
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