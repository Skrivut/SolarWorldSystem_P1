using System;
using System.Data.Linq.Mapping;

namespace SolarWorldERP.Data
{
    // Hand-mapped LINQ to SQL entities (no .dbml designer file needed).

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
