using System;
using System.Data.Linq.Mapping;

namespace SolarWorldERP.Data
{
    // Hand-mapped LINQ to SQL entities (no .dbml designer file needed).
    // Each class = one table. Each [Column] property = one column in that table.
    // To add a new table: copy one of these classes, change the table name and columns,
    // then add a Table<...> property for it in ErpDataContext.

    /// <summary>A person who can log in (table dbo.Users). Role is Staff, Owner or Driver.</summary>
    [Table(Name = "dbo.Users")]
    public class User
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int UserId { get; set; }
        [Column] public string UserName { get; set; }
        [Column] public string FullName { get; set; }
        [Column] public string PasswordHash { get; set; }
        [Column] public string Role { get; set; }
        [Column] public bool IsActive { get; set; }
    }

    /// <summary>A stock item (table dbo.Items). QtyOnHand minus QtyReserved is what is still free to request.</summary>
    [Table(Name = "dbo.Items")]
    public class Item
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int ItemId { get; set; }
        [Column] public string ItemCode { get; set; }
        [Column] public string ItemName { get; set; }
        [Column] public string Unit { get; set; }
        [Column] public string StockTag { get; set; }
        [Column] public int QtyOnHand { get; set; }
        [Column] public int QtyReserved { get; set; }
        [Column] public bool IsScarce { get; set; }
        [Column] public int ReorderPoint { get; set; }
        [Column] public int MaxStockLevel { get; set; }
    }

    /// <summary>The header of a requisition form, RF (table dbo.Requisitions). Status holds an RfStatus name.</summary>
    [Table(Name = "dbo.Requisitions")]
    public class Requisition
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int RfId { get; set; }
        [Column] public string RfNumber { get; set; }
        [Column] public string RequestedBy { get; set; }
        [Column] public string ProjectName { get; set; }
        [Column] public string StockTag { get; set; }
        [Column] public string Status { get; set; }
        [Column] public DateTime DateCreated { get; set; }
        [Column(CanBeNull = true)] public string CreatedBy { get; set; }
        [Column(CanBeNull = true)] public string ApprovedBy { get; set; }
        [Column(CanBeNull = true)] public DateTime? DateApproved { get; set; }
        [Column(CanBeNull = true)] public string Remarks { get; set; }
    }

    /// <summary>One item line inside an RF (table dbo.RequisitionItems), with how much was reserved and how much is short.</summary>
    [Table(Name = "dbo.RequisitionItems")]
    public class RequisitionItem
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)] public int RfItemId { get; set; }
        [Column] public int RfId { get; set; }
        [Column] public int ItemId { get; set; }
        [Column] public int QtyRequested { get; set; }
        [Column] public int QtyReserved { get; set; }
        [Column] public int QtyShortage { get; set; }
    }
}
