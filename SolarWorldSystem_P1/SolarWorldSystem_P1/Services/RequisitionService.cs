using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Transactions;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;
using SolarWorldERP.Models;

namespace SolarWorldERP.Services
{
    public class SubmitLine
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }

    public class SubmitResultLine
    {
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int Requested { get; set; }
        public int Reserved { get; set; }
        public int Shortage { get; set; }
    }

    public class SubmitResult
    {
        public string RfNumber { get; set; }
        public List<SubmitResultLine> Lines { get; set; }
        public bool HasShortage { get { return Lines.Any(l => l.Shortage > 0); } }
    }

    public class RequisitionService
    {
        // A requisition is for project use, so it only counts stock tagged "Project".
        public const string ProjectTag = "Project";

        public List<Item> GetRequestableItems()
        {
            using (var db = new ErpDataContext())
            {
                return db.Items.Where(i => i.StockTag == ProjectTag)
                               .OrderBy(i => i.ItemName).ToList();
            }
        }

        // Saves the RF, checks stock (usable = on hand - reserved, matching tag),
        // and reserves what is available. All in one transaction.
        public SubmitResult Submit(string requestedBy, string projectName, List<SubmitLine> lines)
        {
            if (string.IsNullOrWhiteSpace(requestedBy)) throw new ArgumentException("Requested by is required.");
            if (string.IsNullOrWhiteSpace(projectName)) throw new ArgumentException("Project name is required.");
            if (lines == null || lines.Count == 0) throw new ArgumentException("Add at least one item.");
            if (lines.Any(l => l.Qty <= 0)) throw new ArgumentException("Quantities must be greater than zero.");
            if (lines.GroupBy(l => l.ItemId).Any(g => g.Count() > 1)) throw new ArgumentException("An item appears more than once.");

            var options = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable };
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                using (var db = new ErpDataContext())
                {
                    string prefix = "RF-" + DateTime.Now.Year + "-";
                    int next = db.Requisitions.Count(r => r.RfNumber.StartsWith(prefix)) + 1;

                    var rf = new Requisition
                    {
                        RfNumber = prefix + next.ToString("D4"),
                        RequestedBy = requestedBy.Trim(),
                        ProjectName = projectName.Trim(),
                        StockTag = ProjectTag,
                        Status = RfStatus.Submitted.ToString(),
                        DateCreated = DateTime.Now,
                        CreatedBy = Session.UserName
                    };
                    db.Requisitions.InsertOnSubmit(rf);
                    db.SubmitChanges();   // gets RfId

                    var result = new SubmitResult { RfNumber = rf.RfNumber, Lines = new List<SubmitResultLine>() };

                    foreach (var line in lines)
                    {
                        var item = db.Items.Single(i => i.ItemId == line.ItemId);
                        if (item.StockTag != ProjectTag)
                            throw new InvalidOperationException(item.ItemName + " is not tagged for project use.");

                        int available = Math.Max(0, item.QtyOnHand - item.QtyReserved);
                        int reserve = Math.Min(available, line.Qty);
                        item.QtyReserved += reserve;

                        db.RequisitionItems.InsertOnSubmit(new RequisitionItem
                        {
                            RfId = rf.RfId,
                            ItemId = item.ItemId,
                            QtyRequested = line.Qty,
                            QtyReserved = reserve,
                            QtyShortage = line.Qty - reserve
                        });

                        result.Lines.Add(new SubmitResultLine
                        {
                            ItemName = item.ItemName,
                            Unit = item.Unit,
                            Requested = line.Qty,
                            Reserved = reserve,
                            Shortage = line.Qty - reserve
                        });
                    }

                    db.SubmitChanges();
                    scope.Complete();
                    return result;
                }
            }
            catch (ChangeConflictException)
            {
                // Someone else changed the same stock at the same moment. Nothing was saved.
                throw new InvalidOperationException("Stock was updated by another user. Please submit again.");
            }
        }
    }
}
