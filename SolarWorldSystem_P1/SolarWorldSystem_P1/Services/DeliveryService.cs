using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
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
                    DeliveryNumber = "DEL-" + DateTime.Now.Year + "-" + (db.Deliveries.Count(d => d.DeliveryNumber.StartsWith("DEL-" + DateTime.Now.Year + "-")) + 1).ToString("D4"),
                    PurchaseOrderId = purchaseOrderId,
                    DeliveryAddress = address.Trim(),
                    DriverName = driverName,
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
            SetStatus(deliveryId, "Scheduled", "InTransit");
        }

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
                var orderLines = db.PurchaseOrderLines.Where(l => l.PurchaseOrderId == delivery.PurchaseOrderId).ToList();
                foreach (var pair in quantities)
                {
                    var line = orderLines.SingleOrDefault(l => l.PurchaseOrderLineId == pair.Key);
                    if (line == null || pair.Value <= 0 || pair.Value > line.QtyOrdered - line.QtyReceived)
                        throw new ArgumentException("The delivery quantity is invalid.");
                    line.QtyReceived += pair.Value;
                    var item = db.Items.Single(i => i.ItemId == line.ItemId);
                    item.QtyOnHand += pair.Value;
                    db.DeliveryLines.InsertOnSubmit(new DeliveryLine { DeliveryId = delivery.DeliveryId, PurchaseOrderLineId = line.PurchaseOrderLineId, QtyDelivered = pair.Value });
                }
                delivery.Status = "Delivered";
                delivery.DeliveredDate = DateTime.Now;
                delivery.Notes = notes;
                var order = db.PurchaseOrders.Single(o => o.PurchaseOrderId == delivery.PurchaseOrderId);
                order.Status = orderLines.All(l => l.QtyReceived == l.QtyOrdered) ? "Completed" : "PartiallyReceived";
                db.SubmitChanges();
                scope.Complete();
            }
        }

        private static void SetStatus(int id, string expected, string next)
        {
            using (var db = new ErpDataContext())
            {
                var delivery = db.Deliveries.SingleOrDefault(d => d.DeliveryId == id);
                if (delivery == null || delivery.Status != expected) throw new InvalidOperationException("The delivery is not in the required status.");
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
