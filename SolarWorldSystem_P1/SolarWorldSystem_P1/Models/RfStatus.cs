namespace SolarWorldERP.Models
{
    // Stored in the database as the enum name (string).
    public enum RfStatus
    {
        Submitted,
        Approved,
        PendingFulfillment,
        Fulfilled,
        ReturnedForRevision,
        Cancelled
    }
}
