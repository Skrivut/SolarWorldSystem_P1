using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
    // One purchase order line with its item name, as shown on the receive screen.
    public class ReceiveLineView
    {
        public int PurchaseOrderLineId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyReceived { get; set; }
        public int QtyRemaining { get { return QtyOrdered - QtyReceived; } }
    }

    public class DeliveryService
    {
        public List<Delivery> GetDeliveries(string status)
        {
            RequireRole("Owner", "Staff", "Driver");
            using (var db = new ErpDataContext())
            {
                var query = db.Deliveries.AsQueryable();
                if (!string.IsNullOrWhiteSpace(status)) query = query.Where(d => d.Status == status);
                return query.OrderByDescending(d => d.ExpectedDate).ToList();
            }
        }

        public List<DeliveryLine> GetLines(int deliveryId)
        {
            RequireRole("Owner", "Staff", "Driver");
            using (var db = new ErpDataContext())
                return db.DeliveryLines.Where(l => l.DeliveryId == deliveryId).ToList();
        }

        public Delivery Schedule(int purchaseOrderId, string address, DateTime expectedDate, string driverName)
        {
            RequireRole("Owner", "Staff");
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("Delivery address is required.");
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == purchaseOrderId);
                if (order == null || (order.Status != "Ordered" && order.Status != "PartiallyReceived"))
                    throw new InvalidOperationException("Only an ordered purchase can be delivered.");
                var delivery = new Delivery
                {
                    DeliveryNumber = NextDeliveryNumber(db),
                    PurchaseOrderId = purchaseOrderId,
                    DeliveryAddress = address.Trim(),
                    DriverName = string.IsNullOrWhiteSpace(driverName) ? null : driverName.Trim(),
                    Status = "Scheduled",
                    ExpectedDate = expectedDate
                };
                db.Deliveries.InsertOnSubmit(delivery);
                db.SubmitChanges();
                return delivery;
            }
        }

        public void MarkInTransit(int deliveryId)
        {
            RequireRole("Owner", "Staff", "Driver");
            SetStatus(deliveryId, new[] { "Scheduled" }, "InTransit");
        }

        public void Cancel(int deliveryId)
        {
            RequireRole("Owner", "Staff");
            SetStatus(deliveryId, new[] { "Scheduled", "InTransit" }, "Cancelled");
        }

        // quantities: PurchaseOrderLineId -> quantity actually received on this delivery.
        public void Receive(int deliveryId, Dictionary<int, int> quantities, string notes)
        {
            RequireRole("Owner", "Staff", "Driver");
            if (quantities == null || quantities.Count == 0) throw new ArgumentException("Received quantities are required.");
            var options = new TransactionOptions { IsolationLevel = IsolationLevel.Serializable };
            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            using (var db = new ErpDataContext())
            {
                var delivery = db.Deliveries.SingleOrDefault(d => d.DeliveryId == deliveryId);
                if (delivery == null || (delivery.Status != "Scheduled" && delivery.Status != "InTransit"))
                    throw new InvalidOperationException("This delivery cannot be received.");
                var order = db.PurchaseOrders.Single(o => o.PurchaseOrderId == delivery.PurchaseOrderId);
                if (order.Status != "Ordered" && order.Status != "PartiallyReceived")
                    throw new InvalidOperationException("The purchase order is no longer open for receiving.");
                var orderLines = db.PurchaseOrderLines.Where(l => l.PurchaseOrderId == order.PurchaseOrderId).ToList();
                foreach (var pair in quantities)
                {
                    var line = orderLines.SingleOrDefault(l => l.PurchaseOrderLineId == pair.Key);
                    if (line == null || pair.Value <= 0 || pair.Value > line.QtyOrdered - line.QtyReceived)
                        throw new ArgumentException("The delivery quantity is invalid.");
                    line.QtyReceived += pair.Value;
                    var item = db.Items.Single(i => i.ItemId == line.ItemId);
                    item.QtyOnHand += pair.Value;
                    db.DeliveryLines.InsertOnSubmit(new DeliveryLine
                    {
                        DeliveryId = delivery.DeliveryId,
                        PurchaseOrderLineId = line.PurchaseOrderLineId,
                        QtyDelivered = pair.Value
                    });
                }
                delivery.Status = "Delivered";
                delivery.DeliveredDate = DateTime.Now;
                delivery.Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
                order.Status = orderLines.All(l => l.QtyReceived == l.QtyOrdered) ? "Completed" : "PartiallyReceived";
                db.SubmitChanges();
                scope.Complete();
            }
        }

        // Purchase order numbers by id, so the screen can show "PO-2026-0001" instead of an id.
        public Dictionary<int, string> GetOrderNumbers()
        {
            RequireRole("Owner", "Staff", "Driver");
            using (var db = new ErpDataContext())
                return db.PurchaseOrders.ToDictionary(o => o.PurchaseOrderId, o => o.PoNumber);
        }

        // The purchase order lines behind a delivery, with item names, for the receive screen.
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

        // Highest existing number + 1, so deleted rows can't cause a duplicate.
        private static string NextDeliveryNumber(ErpDataContext db)
        {
            var prefix = "DEL-" + DateTime.Now.Year + "-";
            var last = db.Deliveries.Where(d => d.DeliveryNumber.StartsWith(prefix))
                                    .Select(d => d.DeliveryNumber).OrderByDescending(n => n).FirstOrDefault();
            var seq = last == null ? 0 : int.Parse(last.Substring(prefix.Length));
            return prefix + (seq + 1).ToString("D4");
        }

        private static void SetStatus(int id, string[] allowedFrom, string next)
        {
            using (var db = new ErpDataContext())
            {
                var delivery = db.Deliveries.SingleOrDefault(d => d.DeliveryId == id);
                if (delivery == null || !allowedFrom.Contains(delivery.Status))
                    throw new InvalidOperationException("The delivery is not in the required status.");
                delivery.Status = next;
                db.SubmitChanges();
            }
        }

        private static void RequireRole(params string[] roles)
        {
            if (!roles.Contains(Session.Role)) throw new UnauthorizedAccessException("Your role is not allowed to perform this action.");
        }
    }
}