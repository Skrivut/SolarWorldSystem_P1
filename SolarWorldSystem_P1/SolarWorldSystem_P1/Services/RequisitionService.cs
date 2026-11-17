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
    /// <summary>One item and quantity the user typed on the RF form.</summary>
    public class SubmitLine
    {
        public int ItemId { get; set; }
        public int Qty { get; set; }
    }

    /// <summary>What happened to one line when the RF was checked against stock.</summary>
    public class SubmitResultLine
    {
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int Requested { get; set; }
        public int Reserved { get; set; }
        public int Shortage { get; set; }
    }

    /// <summary>The outcome of submitting (or resubmitting) an RF: its number and the per-line result.</summary>
    public class SubmitResult
    {
        public string RfNumber { get; set; }
        public List<SubmitResultLine> Lines { get; set; }

        /// <summary>True if at least one line could not be fully reserved.</summary>
        public bool HasShortage { get { return Lines.Any(l => l.Shortage > 0); } }
    }

    /// <summary>
    /// All the business rules for requisitions: creating, approving, returning, cancelling and listing RFs.
    /// The forms only call these methods. Every change that touches several rows runs in one
    /// database transaction, so stock and RF status can never end up out of step.
    /// </summary>
    public class RequisitionService
    {
        /// <summary>A requisition is for project use, so it only counts stock tagged "Project".</summary>
        public const string ProjectTag = "Project";

        // ------------------------------------------------------------------
        //  Reading data
        // ------------------------------------------------------------------

        /// <summary>Items that can be requested on an RF (project-tagged only), sorted by name. Fills the item drop-down.</summary>
        public List<Item> GetRequestableItems()
        {
            using (var db = new ErpDataContext())
            {
                return db.Items.Where(i => i.StockTag == ProjectTag)
                               .OrderBy(i => i.ItemName).ToList();
            }
        }

        /// <summary>
        /// The RFs for the list screen, newest first.
        /// Pass a status name (like "Submitted") to filter, or null/empty for all.
        /// </summary>
        public List<RfListRow> GetRequisitions(string status)
        {
            using (var db = new ErpDataContext())
            {
                IQueryable<Requisition> query = db.Requisitions;
                if (!string.IsNullOrEmpty(status))
                    query = query.Where(r => r.Status == status);

                return query.OrderByDescending(r => r.DateCreated)
                            .Select(r => new RfListRow
                            {
                                RfId = r.RfId,
                                RfNumber = r.RfNumber,
                                ProjectName = r.ProjectName,
                                RequestedBy = r.RequestedBy,
                                DateCreated = r.DateCreated,
                                Status = r.Status
                            }).ToList();
            }
        }

        /// <summary>One RF with all its lines (item names included). Used by the approval and revise screens.</summary>
        public RfDetail GetRequisition(int rfId)
        {
            using (var db = new ErpDataContext())
            {
                var rf = db.Requisitions.SingleOrDefault(r => r.RfId == rfId);
                if (rf == null) throw new InvalidOperationException("Requisition not found.");

                var lines = (from l in db.RequisitionItems
                             where l.RfId == rfId
                             join i in db.Items on l.ItemId equals i.ItemId
                             select new RfLineView
                             {
                                 ItemId = i.ItemId,
                                 ItemCode = i.ItemCode,
                                 ItemName = i.ItemName,
                                 Unit = i.Unit,
                                 QtyRequested = l.QtyRequested,
                                 QtyReserved = l.QtyReserved,
                                 QtyShortage = l.QtyShortage
                             }).ToList();

                return new RfDetail
                {
                    RfId = rf.RfId,
                    RfNumber = rf.RfNumber,
                    RequestedBy = rf.RequestedBy,
                    ProjectName = rf.ProjectName,
                    Status = rf.Status,
                    DateCreated = rf.DateCreated,
                    CreatedBy = rf.CreatedBy,
                    ApprovedBy = rf.ApprovedBy,
                    DateApproved = rf.DateApproved,
                    Remarks = rf.Remarks,
                    Lines = lines
                };
            }
        }

        // ------------------------------------------------------------------
        //  Staff actions
        // ------------------------------------------------------------------

        /// <summary>
        /// Creates a new RF, then checks stock (usable = on hand minus reserved, matching tag)
        /// and reserves what is available. Anything not available is saved as a shortage on the line.
        /// </summary>
        public SubmitResult Submit(string requestedBy, string projectName, List<SubmitLine> lines)
        {
            RequireRole("Staff", "Owner");
            ValidateInput(requestedBy, projectName, lines);

            return InTransaction(db =>
            {
                // RF number looks like RF-2026-0001: year plus a running count for that year.
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
                db.SubmitChanges();   // saves now so the database gives us rf.RfId

                return ReserveLines(db, rf, lines);
            });
        }

        /// <summary>
        /// Staff fixes an RF the owner returned and sends it again. The old lines are replaced,
        /// stock is checked and reserved again, and the status goes back to Submitted.
        /// </summary>
        public SubmitResult Resubmit(int rfId, string requestedBy, string projectName, List<SubmitLine> lines)
        {
            RequireRole("Staff", "Owner");
            ValidateInput(requestedBy, projectName, lines);

            return InTransaction(db =>
            {
                var rf = GetRfForChange(db, rfId, RfStatus.ReturnedForRevision);

                // The reservation was already released when the RF was returned,
                // so the old lines hold no stock and can simply be replaced.
                db.RequisitionItems.DeleteAllOnSubmit(db.RequisitionItems.Where(l => l.RfId == rfId).ToList());

                rf.RequestedBy = requestedBy.Trim();
                rf.ProjectName = projectName.Trim();
                rf.Status = RfStatus.Submitted.ToString();
                rf.Remarks = null;

                return ReserveLines(db, rf, lines);
            });
        }

        /// <summary>Staff (or owner) cancels an RF that is still Submitted or ReturnedForRevision. Reserved stock is released.</summary>
        public void Cancel(int rfId)
        {
            RequireRole("Staff", "Owner");

            InTransactionVoid(db =>
            {
                var rf = GetRfForChange(db, rfId, RfStatus.Submitted, RfStatus.ReturnedForRevision);
                ReleaseReservation(db, rfId);
                rf.Status = RfStatus.Cancelled.ToString();
                rf.Remarks = "Cancelled by " + Session.UserName;
            });
        }

        // ------------------------------------------------------------------
        //  Owner actions
        // ------------------------------------------------------------------

        /// <summary>Owner approves a Submitted RF. Records who approved and when. Reserved stock stays reserved.</summary>
        public void Approve(int rfId)
        {
            RequireRole("Owner");

            InTransactionVoid(db =>
            {
                var rf = GetRfForChange(db, rfId, RfStatus.Submitted);
                rf.Status = RfStatus.Approved.ToString();
                rf.ApprovedBy = Session.UserName;
                rf.DateApproved = DateTime.Now;
            });
        }

        /// <summary>Owner sends a Submitted RF back to staff with a reason. Reserved stock is released.</summary>
        public void ReturnForRevision(int rfId, string remarks)
        {
            RequireRole("Owner");
            RequireRemarks(remarks);

            InTransactionVoid(db =>
            {
                var rf = GetRfForChange(db, rfId, RfStatus.Submitted);
                ReleaseReservation(db, rfId);
                rf.Status = RfStatus.ReturnedForRevision.ToString();
                rf.Remarks = remarks.Trim();
            });
        }

        /// <summary>Owner rejects a Submitted RF with a reason. It becomes Cancelled and reserved stock is released.</summary>
        public void Reject(int rfId, string remarks)
        {
            RequireRole("Owner");
            RequireRemarks(remarks);

            InTransactionVoid(db =>
            {
                var rf = GetRfForChange(db, rfId, RfStatus.Submitted);
                ReleaseReservation(db, rfId);
                rf.Status = RfStatus.Cancelled.ToString();
                rf.Remarks = "Rejected by owner: " + remarks.Trim();
            });
        }

        // ------------------------------------------------------------------
        //  Hand-off for the purchasing module
        // ------------------------------------------------------------------

        /// <summary>
        /// TEAM NOTE (purchasing): call this once the PO for an approved RF's shortage has been SENT.
        /// It moves the RF from Approved to PendingFulfillment.
        /// Usage: new RequisitionService().MarkPendingFulfillment(rfId);
        /// It throws an error if the RF is not currently Approved.
        /// To find what to order, read QtyShortage on that RF's lines (RequisitionItems table).
        /// </summary>
        
        public void MarkPendingFulfillment(int rfId)
        {
            InTransactionVoid(db =>
            {
                var rf = GetRfForChange(db, rfId, RfStatus.Approved);
                rf.Status = RfStatus.PendingFulfillment.ToString();
            });
        }

        // ------------------------------------------------------------------
        //  Private helpers
        // ------------------------------------------------------------------

        /// <summary>
        /// Runs some work and saves it in one serializable database transaction and returns the work's result.
        /// If someone else changed the same rows at the same moment, nothing is saved and a friendly error is thrown.
        /// </summary>
        private static T InTransaction<T>(Func<ErpDataContext, T> work)
        {
            var options = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable };
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                using (var db = new ErpDataContext())
                {
                    T result = work(db);
                    db.SubmitChanges();
                    scope.Complete();
                    return result;
                }
            }
            catch (ChangeConflictException)
            {
                throw new InvalidOperationException(
                    "The data was changed by another user at the same time. Nothing was saved. Please try again.");
            }
        }

        /// <summary>Same as InTransaction, for work that returns nothing.</summary>
        private static void InTransactionVoid(Action<ErpDataContext> work)
        {
            InTransaction<object>(db => { work(db); return null; });
        }

        /// <summary>Stops the action unless the logged-in user has one of the allowed roles.</summary>
        private static void RequireRole(params string[] allowedRoles)
        {
            if (!allowedRoles.Contains(Session.Role))
                throw new UnauthorizedAccessException("Your role (" + Session.Role + ") is not allowed to do this.");
        }

        /// <summary>Makes sure the owner typed a reason before returning or rejecting an RF.</summary>
        private static void RequireRemarks(string remarks)
        {
            if (string.IsNullOrWhiteSpace(remarks))
                throw new ArgumentException("Please type a remark explaining why.");
        }

        /// <summary>Checks the RF form input: names filled in, at least one line, quantities above zero, no repeated item.</summary>
        private static void ValidateInput(string requestedBy, string projectName, List<SubmitLine> lines)
        {
            if (string.IsNullOrWhiteSpace(requestedBy)) throw new ArgumentException("Requested by is required.");
            if (string.IsNullOrWhiteSpace(projectName)) throw new ArgumentException("Project name is required.");
            if (lines == null || lines.Count == 0) throw new ArgumentException("Add at least one item.");
            if (lines.Any(l => l.Qty <= 0)) throw new ArgumentException("Quantities must be greater than zero.");
            if (lines.GroupBy(l => l.ItemId).Any(g => g.Count() > 1)) throw new ArgumentException("An item appears more than once.");
        }

        /// <summary>
        /// Loads an RF and checks it is in one of the allowed statuses before it is changed.
        /// This is what stops, for example, an already-approved RF from being approved twice.
        /// </summary>
        private static Requisition GetRfForChange(ErpDataContext db, int rfId, params RfStatus[] allowedStatuses)
        {
            var rf = db.Requisitions.SingleOrDefault(r => r.RfId == rfId);
            if (rf == null) throw new InvalidOperationException("Requisition not found.");

            if (!allowedStatuses.Any(s => s.ToString() == rf.Status))
                throw new InvalidOperationException(
                    rf.RfNumber + " is " + rf.Status + ", so this action is not allowed.");
            return rf;
        }

        /// <summary>
        /// The stock check. For each line: usable stock = on hand minus reserved. Reserve as much as is
        /// usable (up to the quantity asked), save the line, and record the rest as a shortage.
        /// </summary>
        private static SubmitResult ReserveLines(ErpDataContext db, Requisition rf, List<SubmitLine> lines)
        {
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
            return result;
        }

        /// <summary>
        /// Gives an RF's reserved stock back to the items (QtyReserved goes down) and zeroes the reserved
        /// amount on its lines. Safe to call twice: lines already at zero are skipped.
        /// </summary>
        private static void ReleaseReservation(ErpDataContext db, int rfId)
        {
            var reservedLines = db.RequisitionItems.Where(l => l.RfId == rfId && l.QtyReserved > 0).ToList();
            foreach (var line in reservedLines)
            {
                var item = db.Items.Single(i => i.ItemId == line.ItemId);
                item.QtyReserved = Math.Max(0, item.QtyReserved - line.QtyReserved);
                line.QtyReserved = 0;
            }
        }
    }
}
