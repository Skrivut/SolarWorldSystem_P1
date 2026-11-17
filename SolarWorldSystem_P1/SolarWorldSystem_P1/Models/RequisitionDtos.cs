using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarWorldERP.Models
{
    // Small "display" classes returned by RequisitionService so the forms never touch the database directly.

    /// <summary>One row in the requisition list grid.</summary>
    public class RfListRow
    {
        public int RfId { get; set; }
        public string RfNumber { get; set; }
        public string ProjectName { get; set; }
        public string RequestedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
    }

    /// <summary>One item line of an RF, with the item's name and unit filled in for display.</summary>
    public class RfLineView
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public int QtyRequested { get; set; }
        public int QtyReserved { get; set; }
        public int QtyShortage { get; set; }
    }

    /// <summary>A whole RF: header fields plus all its lines.</summary>
    public class RfDetail
    {
        public int RfId { get; set; }
        public string RfNumber { get; set; }
        public string RequestedBy { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? DateApproved { get; set; }
        public string Remarks { get; set; }
        public List<RfLineView> Lines { get; set; } = new List<RfLineView>();

        /// <summary>True if any line could not be fully reserved.</summary>
        public bool HasShortage { get { return Lines.Any(l => l.QtyShortage > 0); } }
    }
}
