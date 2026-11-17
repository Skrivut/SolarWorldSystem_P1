namespace SolarWorldERP.Models
{
    /// <summary>
    /// The life of a requisition form (RF). The database stores the name as text.
    /// Flow: Submitted > Approved > PendingFulfillment (only if short) > Fulfilled.
    /// Exits: ReturnedForRevision (staff fixes and resubmits) and Cancelled.
    /// </summary>
    public enum RfStatus
    {
        /// <summary>Staff sent it in; waiting for the owner. Stock is reserved.</summary>
        Submitted,
        /// <summary>Owner approved it.</summary>
        Approved,
        /// <summary>Approved, but waiting for purchased stock to arrive (set by purchasing).</summary>
        PendingFulfillment,
        /// <summary>All items issued and the issuance slip signed.</summary>
        Fulfilled,
        /// <summary>Owner sent it back; reservation released; staff must revise.</summary>
        ReturnedForRevision,
        /// <summary>Rejected or cancelled; reservation released.</summary>
        Cancelled
    }
}
