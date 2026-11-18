using System;
using System.Collections.Generic;
using System.Linq;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
    public class PurchasingService
    {
        public List<PurchaseOrder> GetOrders(string status)
        {
            RequireRole("Owner");
            using (var db = new ErpDataContext())
            {
                var query = db.PurchaseOrders.AsQueryable();
                if (!string.IsNullOrWhiteSpace(status)) query = query.Where(o => o.Status == status);
                return query.OrderByDescending(o => o.DateCreated).ToList();
            }
        }

        public List<Item> GetProducts()
        {
            RequireRole("Owner");
            using (var db = new ErpDataContext())
                return db.Items.OrderBy(i => i.ItemName).ToList();
        }

        public PurchaseOrder CreateOrder(string supplier, Dictionary<int, int> quantities, Dictionary<int, decimal> costs)
        {
            RequireRole("Owner");
            if (string.IsNullOrWhiteSpace(supplier)) throw new ArgumentException("Supplier is required.");
            if (quantities == null || quantities.Count == 0) throw new ArgumentException("Add at least one product.");

            using (var db = new ErpDataContext())
            {
                var order = new PurchaseOrder
                {
                    PONumber = "PO-" + DateTime.Now.Year + "-" + (db.PurchaseOrders.Count(o => o.PONumber.StartsWith("PO-" + DateTime.Now.Year + "-")) + 1).ToString("D4"),
                    SupplierName = supplier.Trim(),
                    RequestedBy = Session.UserName,
                    Status = "Draft",
                    DateCreated = DateTime.Now,
                    TotalAmount = 0
                };
                db.PurchaseOrders.InsertOnSubmit(order);
                db.SubmitChanges();

                decimal total = 0;
                foreach (var requested in quantities)
                {
                    if (requested.Value <= 0) throw new ArgumentException("Quantities must be greater than zero.");
                    var item = db.Items.SingleOrDefault(i => i.ItemId == requested.Key);
                    if (item == null) throw new KeyNotFoundException("Product was not found.");
                    decimal cost = costs != null && costs.ContainsKey(item.ItemId) ? costs[item.ItemId] : 0;
                    if (cost < 0) throw new ArgumentException("Unit cost cannot be negative.");
                    db.PurchaseOrderLines.InsertOnSubmit(new PurchaseOrderLine { PurchaseOrderId = order.PurchaseOrderId, ItemId = item.ItemId, QtyOrdered = requested.Value, UnitCost = cost });
                    total += requested.Value * cost;
                }
                order.TotalAmount = total;
                db.SubmitChanges();
                return order;
            }
        }

        public void Approve(int purchaseOrderId)
        {
            RequireRole("Owner");
            ChangeStatus(purchaseOrderId, "Draft", "Approved", true);
        }

        public void SendToSupplier(int purchaseOrderId)
        {
            RequireRole("Owner");
            ChangeStatus(purchaseOrderId, "Approved", "Ordered", false);
        }

        private static void ChangeStatus(int id, string expected, string next, bool approved)
        {
            using (var db = new ErpDataContext())
            {
                var order = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == id);
                if (order == null) throw new KeyNotFoundException("Purchase order was not found.");
                if (order.Status != expected) throw new InvalidOperationException("The purchase order is not in the required status.");
                order.Status = next;
                if (approved) order.DateApproved = DateTime.Now;
                else order.DateOrdered = DateTime.Now;
                db.SubmitChanges();
            }
        }

        private static void RequireRole(params string[] roles)
        {
            if (!roles.Contains(Session.Role)) throw new UnauthorizedAccessException("Your role is not allowed to perform this action.");
        }
    }
}
