using System;
using System.Collections.Generic;
using System.Linq;
using SolarWorldERP.Models;

namespace SolarWorldERP.Services
{
    /// <summary>The four groups shown in the Action Inbox, in display order.</summary>
    public enum InboxGroup
    {
        OwnerApprovals,      // RFs and purchase orders waiting for the owner's decision
        PendingDeliveries,   // pickups that are not finished (driver, then receiving report)
        ReadyForIssuance,    // approved RFs whose stock is fully reserved
        LowStock             // items at or below their reorder point
    }

    /// <summary>One row of the Action Inbox. The screen only needs these fields to draw a row.</summary>
    public class InboxItem
    {
        public InboxGroup Group { get; set; }
        /// <summary>What the Id refers to: "RF", "PO", "Delivery" or "Item".</summary>
        public string Kind { get; set; }
        /// <summary>RfId, PurchaseOrderId, DeliveryId or ItemId, depending on Kind.</summary>
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        /// <summary>When the task started waiting. Null when it does not apply (low stock).</summary>
        public DateTime? Since { get; set; }

        /// <summary>A short age for the right edge of the row: now, 5m, 3h, 2d.</summary>
        public string Age
        {
            get
            {
                if (!Since.HasValue) return "now";
                var span = DateTime.Now - Since.Value;
                if (span.TotalMinutes < 1) return "now";
                if (span.TotalHours < 1) return (int)span.TotalMinutes + "m";
                if (span.TotalDays < 1) return (int)span.TotalHours + "h";
                return (int)span.TotalDays + "d";
            }
        }
    }

    /// <summary>The inbox rows, plus a message for any source that could not be read.</summary>
    public class InboxResult
    {
        public List<InboxItem> Items { get; private set; }
        public List<string> Warnings { get; private set; }
        public InboxResult() { Items = new List<InboxItem>(); Warnings = new List<string>(); }
    }

    /// <summary>
    /// Collects the tasks for the Action Inbox from the existing services. It only READS data;
    /// it never changes anything and it has no screen code, so it can be tested on its own.
    /// If one source fails (for example a missing database column), the other groups still load
    /// and the failure is reported in Warnings.
    /// </summary>
    public class InboxService
    {
        public InboxResult GetInbox()
        {
            var result = new InboxResult();
            result.Items.AddRange(Safe(OwnerApprovals, result.Warnings, "Owner approvals"));
            result.Items.AddRange(Safe(PendingDeliveries, result.Warnings, "Pending deliveries"));
            result.Items.AddRange(Safe(ReadyForIssuance, result.Warnings, "Ready for issuance"));
            result.Items.AddRange(Safe(LowStock, result.Warnings, "Low stock"));
            return result;
        }

        // RFs that were submitted, and purchase orders that were submitted for approval.
        private static List<InboxItem> OwnerApprovals()
        {
            var items = new List<InboxItem>();

            foreach (var rf in new RequisitionService().GetRequisitions("Submitted"))
                items.Add(new InboxItem
                {
                    Group = InboxGroup.OwnerApprovals,
                    Kind = "RF",
                    Id = rf.RfId,
                    Title = rf.RfNumber + " \u00b7 " + rf.ProjectName,
                    Subtitle = "Requisition waiting for the owner \u00b7 by " + rf.RequestedBy,
                    Since = rf.DateCreated
                });

            var purchasing = new PurchasingService();
            var suppliers = purchasing.GetSuppliers(false).ToDictionary(s => s.SupplierId, s => s.SupplierName);
            foreach (var po in purchasing.GetOrders("PendingApproval"))
            {
                string supplier;
                if (!suppliers.TryGetValue(po.SupplierId, out supplier)) supplier = "Supplier";
                items.Add(new InboxItem
                {
                    Group = InboxGroup.OwnerApprovals,
                    Kind = "PO",
                    Id = po.PurchaseOrderId,
                    Title = po.PoNumber + " \u00b7 " + supplier,
                    Subtitle = "Purchase order waiting for the owner",
                    Since = po.DateCreated
                });
            }
            return Oldest(items);
        }

        // Pickups that are still in progress: not yet picked up, on the way, or waiting for the receiving report.
        private static List<InboxItem> PendingDeliveries()
        {
            var items = new List<InboxItem>();
            var service = new DeliveryService();
            var poNumbers = service.GetOrderNumbers();
            foreach (var d in service.GetDeliveries(null))
            {
                if (d.Status != "Pending" && d.Status != "PickedUp" && d.Status != "Delivered") continue;
                string po;
                if (!poNumbers.TryGetValue(d.PurchaseOrderId, out po)) po = "PO";

                string text; DateTime? since;
                if (d.Status == "Delivered") { text = "At the office \u00b7 receiving report due"; since = d.DeliveredDate; }
                else if (d.Status == "PickedUp") { text = "Picked up \u00b7 on the way to the office"; since = d.PickedUpDate; }
                else { text = "Waiting for the driver"; since = d.ExpectedDate; }

                items.Add(new InboxItem
                {
                    Group = InboxGroup.PendingDeliveries,
                    Kind = "Delivery",
                    Id = d.DeliveryId,
                    Title = d.DeliveryNumber + " \u00b7 " + po,
                    Subtitle = text,
                    Since = since
                });
            }
            return Oldest(items);
        }

        // Approved or pending-fulfillment RFs that no longer have any shortage.
        private static List<InboxItem> ReadyForIssuance()
        {
            var withShortage = new HashSet<int>(new PurchasingService().GetRequisitionsWithShortage().Select(r => r.RfId));
            var items = new List<InboxItem>();
            foreach (var rf in new InventoryService().GetIssuableRequisitions())
            {
                if (withShortage.Contains(rf.RfId)) continue;
                items.Add(new InboxItem
                {
                    Group = InboxGroup.ReadyForIssuance,
                    Kind = "RF",
                    Id = rf.RfId,
                    Title = rf.RfNumber + " \u00b7 " + rf.ProjectName,
                    Subtitle = "Stock reserved \u00b7 ready to release",
                    Since = rf.DateCreated
                });
            }
            return Oldest(items);
        }

        // Items at or below their reorder point.
        private static List<InboxItem> LowStock()
        {
            var items = new List<InboxItem>();
            foreach (var i in new InventoryService().GetLowStockItems())
                items.Add(new InboxItem
                {
                    Group = InboxGroup.LowStock,
                    Kind = "Item",
                    Id = i.ItemId,
                    Title = i.ItemName,
                    Subtitle = "Available " + i.QtyAvailable + " \u00b7 reorder point " + i.ReorderPoint + (i.IsScarce ? " \u00b7 scarce" : ""),
                    Since = null
                });
            return items;
        }

        // The task that has waited longest goes first.
        private static List<InboxItem> Oldest(List<InboxItem> items)
        {
            return items.OrderBy(i => i.Since ?? DateTime.MaxValue).ToList();
        }

        // Runs one source; if it throws, remember the message and carry on with an empty list.
        private static List<InboxItem> Safe(Func<List<InboxItem>> build, List<string> warnings, string what)
        {
            try { return build(); }
            catch (Exception ex)
            {
                warnings.Add(what + ": " + ex.Message);
                return new List<InboxItem>();
            }
        }
    }
}