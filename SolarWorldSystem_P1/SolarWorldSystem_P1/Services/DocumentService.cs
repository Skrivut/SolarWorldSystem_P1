using System;
using System.Collections.Generic;
using System.Linq;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
    /// <summary>One item line of a requisition, with the stock tag the design shows (Project or For Sale).</summary>
    public class DocLine
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string StockTag { get; set; }
        public int Requested { get; set; }
        public int Reserved { get; set; }
        public int Shortage { get; set; }
    }

    /// <summary>A purchase order linked to a requisition, with how much has arrived.</summary>
    public class DocOrder
    {
        public int PurchaseOrderId { get; set; }
        public string PoNumber { get; set; }
        public string SupplierName { get; set; }
        public string Status { get; set; }
        public int Ordered { get; set; }
        public int Received { get; set; }
        public int Outstanding { get { return Ordered - Received; } }
        /// <summary>Sum of quantity x unit cost for the lines that have a cost. Null when no line has one.</summary>
        public decimal? TotalCost { get; set; }
        /// <summary>The database has no supplier-payment column yet, so this stays a placeholder until Part 4D.</summary>
        public string PaymentText { get { return "Payment: not tracked yet"; } }
    }

    /// <summary>One step in the activity timeline of a requisition.</summary>
    public class DocEvent
    {
        public DateTime When { get; set; }
        public string Actor { get; set; }
        public string Text { get; set; }
    }

    /// <summary>Everything the document view needs to draw one requisition.</summary>
    public class RfDocument
    {
        public int RfId { get; set; }
        public string RfNumber { get; set; }
        public string ProjectName { get; set; }
        public string RequestedBy { get; set; }
        public string Status { get; set; }
        public string StockTag { get; set; }
        public DateTime DateCreated { get; set; }
        public string Remarks { get; set; }
        public List<DocLine> Lines { get; set; }
        public List<DocOrder> Orders { get; set; }
        public List<DocEvent> Events { get; set; }

        public int TotalReserved { get { return Lines.Sum(l => l.Reserved); } }
        public int TotalShortage { get { return Lines.Sum(l => l.Shortage); } }
    }

    /// <summary>
    /// Reads one requisition with everything linked to it, for the Document Workspace.
    /// It only READS data. The activity timeline is built from the dates and names already saved on the
    /// requisition, its purchase orders and their deliveries. It is not a separate audit table.
    /// </summary>
    public class DocumentService
    {
        /// <summary>Works out which requisition a task belongs to: an RF itself, a PO's RF, or a delivery's PO's RF.</summary>
        public int? FindRfId(string kind, int id)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                switch (kind)
                {
                    case "RF":
                        return id;
                    case "PO":
                        {
                            var po = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == id);
                            return po == null ? null : po.RfId;
                        }
                    case "Delivery":
                        {
                            var d = db.Deliveries.SingleOrDefault(x => x.DeliveryId == id);
                            if (d == null) return null;
                            var po = db.PurchaseOrders.SingleOrDefault(o => o.PurchaseOrderId == d.PurchaseOrderId);
                            return po == null ? null : po.RfId;
                        }
                    default:
                        return null;
                }
            }
        }

        /// <summary>The requisition with its lines, linked purchase orders and activity timeline.</summary>
        public RfDocument GetRequisitionDocument(int rfId)
        {
            RequireRole("Owner", "Staff");
            using (var db = new ErpDataContext())
            {
                var rf = db.Requisitions.SingleOrDefault(r => r.RfId == rfId);
                if (rf == null) throw new InvalidOperationException("Requisition not found.");

                var lines = (from l in db.RequisitionItems
                             where l.RfId == rfId
                             join i in db.Items on l.ItemId equals i.ItemId
                             orderby i.ItemName
                             select new DocLine
                             {
                                 ItemCode = i.ItemCode,
                                 ItemName = i.ItemName,
                                 Unit = i.Unit,
                                 StockTag = i.StockTag,
                                 Requested = l.QtyRequested,
                                 Reserved = l.QtyReserved,
                                 Shortage = l.QtyShortage
                             }).ToList();

                var suppliers = db.Suppliers.ToDictionary(s => s.SupplierId, s => s.SupplierName);
                var pos = db.PurchaseOrders.Where(o => o.RfId == rfId).OrderBy(o => o.DateCreated).ToList();
                var poIds = pos.Select(o => o.PurchaseOrderId).ToList();
                var poLines = poIds.Count == 0
                    ? new List<PurchaseOrderLine>()
                    : db.PurchaseOrderLines.Where(l => poIds.Contains(l.PurchaseOrderId)).ToList();
                var deliveries = poIds.Count == 0
                    ? new List<Delivery>()
                    : db.Deliveries.Where(d => poIds.Contains(d.PurchaseOrderId)).ToList();

                var orders = new List<DocOrder>();
                foreach (var po in pos)
                {
                    var mine = poLines.Where(l => l.PurchaseOrderId == po.PurchaseOrderId).ToList();
                    string supplier;
                    if (!suppliers.TryGetValue(po.SupplierId, out supplier)) supplier = "Supplier";
                    var costed = mine.Where(l => l.UnitCost.HasValue).ToList();
                    orders.Add(new DocOrder
                    {
                        PurchaseOrderId = po.PurchaseOrderId,
                        PoNumber = po.PoNumber,
                        SupplierName = supplier,
                        Status = po.Status,
                        Ordered = mine.Sum(l => l.QtyOrdered),
                        Received = mine.Sum(l => l.QtyReceived),
                        TotalCost = costed.Count == 0 ? (decimal?)null : costed.Sum(l => l.QtyOrdered * l.UnitCost.Value)
                    });
                }

                return new RfDocument
                {
                    RfId = rf.RfId,
                    RfNumber = rf.RfNumber,
                    ProjectName = rf.ProjectName,
                    RequestedBy = rf.RequestedBy,
                    Status = rf.Status,
                    StockTag = rf.StockTag,
                    DateCreated = rf.DateCreated,
                    Remarks = rf.Remarks,
                    Lines = lines,
                    Orders = orders,
                    Events = BuildTimeline(rf, pos, deliveries)
                };
            }
        }

        // Builds the timeline from the dates and names that are already saved.
        private static List<DocEvent> BuildTimeline(Requisition rf, List<PurchaseOrder> pos, List<Delivery> deliveries)
        {
            var events = new List<DocEvent>();
            events.Add(new DocEvent { When = rf.DateCreated, Actor = Who(rf.CreatedBy ?? rf.RequestedBy), Text = "Submitted requisition" });

            if (rf.DateApproved.HasValue)
            {
                string text = "Approved requisition";
                if (rf.Status == "ReturnedForRevision") text = "Returned for revision";
                else if (rf.Status == "Cancelled") text = "Rejected requisition";
                if ((rf.Status == "ReturnedForRevision" || rf.Status == "Cancelled") && !string.IsNullOrWhiteSpace(rf.Remarks))
                    text += ": " + rf.Remarks;
                events.Add(new DocEvent { When = rf.DateApproved.Value, Actor = Who(rf.ApprovedBy), Text = text });
            }

            foreach (var po in pos)
            {
                events.Add(new DocEvent { When = po.DateCreated, Actor = Who(po.CreatedBy), Text = "Created " + po.PoNumber });
                if (po.DateApproved.HasValue)
                    events.Add(new DocEvent
                    {
                        When = po.DateApproved.Value,
                        Actor = Who(po.ApprovedBy),
                        Text = (po.Status == "Cancelled" ? "Rejected " : "Approved ") + po.PoNumber
                    });
                if (po.DateClosed.HasValue)
                    events.Add(new DocEvent { When = po.DateClosed.Value, Actor = "System", Text = "Closed " + po.PoNumber + " (fully received)" });

                foreach (var d in deliveries.Where(x => x.PurchaseOrderId == po.PurchaseOrderId))
                {
                    if (d.PickedUpDate.HasValue)
                        events.Add(new DocEvent
                        {
                            When = d.PickedUpDate.Value,
                            Actor = Who(d.DriverName),
                            Text = "Picked up goods for " + po.PoNumber + (string.IsNullOrWhiteSpace(d.SupplierDrNumber) ? "" : " (DR " + d.SupplierDrNumber + ")")
                        });
                    if (d.DeliveredDate.HasValue)
                        events.Add(new DocEvent { When = d.DeliveredDate.Value, Actor = Who(d.DriverName), Text = "Delivered to the office" });
                    if (d.ReceivedDate.HasValue)
                        events.Add(new DocEvent
                        {
                            When = d.ReceivedDate.Value,
                            Actor = Who(d.ReceivedBy),
                            Text = "Receiving report " + (d.ReportNumber ?? "")
                        });
                }
            }
            return events.OrderBy(e => e.When).ToList();
        }

        private static string Who(string name) { return string.IsNullOrWhiteSpace(name) ? "System" : name; }

        private static void RequireRole(params string[] roles)
        {
            if (!roles.Contains(Session.Role)) throw new UnauthorizedAccessException("Your role is not allowed to perform this action.");
        }
    }
}