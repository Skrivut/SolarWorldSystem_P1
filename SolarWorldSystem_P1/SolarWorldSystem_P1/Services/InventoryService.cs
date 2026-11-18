using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Transactions;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
    /// <summary>
    /// Display row for the Inventory grid, combining database columns with calculated availability and alert flags.
    /// </summary>
    public class InventoryItemRow
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string StockTag { get; set; }
        public int QtyOnHand { get; set; }
        public int QtyReserved { get; set; }
        public int QtyAvailable { get; set; }
        public bool IsScarce { get; set; }
        public int ReorderPoint { get; set; }
        public int MaxStockLevel { get; set; }
        public bool IsLowStock { get; set; }
    }

    /// <summary>
    /// Business logic for viewing, searching, and managing inventory stock levels.
    /// </summary>
    public class InventoryService
    {
        /// <summary>
        /// Returns all inventory items, optionally filtered by StockTag ("Project", "ForSale", or null/empty for all),
        /// search text (matches ItemCode or ItemName), and low-stock-only flag.
        /// </summary>
        public List<InventoryItemRow> GetItems(string stockTag = null, string searchText = null, bool lowStockOnly = false)
        {
            using (var db = new ErpDataContext())
            {
                IQueryable<Item> query = db.Items;

                if (!string.IsNullOrWhiteSpace(stockTag) && stockTag != "All")
                {
                    query = query.Where(i => i.StockTag == stockTag);
                }

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string term = searchText.Trim().ToLower();
                    query = query.Where(i => i.ItemCode.ToLower().Contains(term) ||
                                             i.ItemName.ToLower().Contains(term));
                }

                var rows = query
                    .OrderBy(i => i.ItemName)
                    .Select(i => new InventoryItemRow
                    {
                        ItemId = i.ItemId,
                        ItemCode = i.ItemCode,
                        ItemName = i.ItemName,
                        Unit = i.Unit,
                        StockTag = i.StockTag,
                        QtyOnHand = i.QtyOnHand,
                        QtyReserved = i.QtyReserved,
                        QtyAvailable = Math.Max(0, i.QtyOnHand - i.QtyReserved),
                        IsScarce = i.IsScarce,
                        ReorderPoint = i.ReorderPoint,
                        MaxStockLevel = i.MaxStockLevel,
                        IsLowStock = (i.QtyOnHand - i.QtyReserved) <= i.ReorderPoint
                    })
                    .ToList();

                if (lowStockOnly)
                {
                    rows = rows.Where(r => r.IsLowStock).ToList();
                }

                return rows;
            }
        }

        /// <summary>
        /// Returns a single item by ID, or throws if not found.
        /// </summary>
        public Item GetItemById(int itemId)
        {
            using (var db = new ErpDataContext())
            {
                var item = db.Items.SingleOrDefault(i => i.ItemId == itemId);
                if (item == null)
                    throw new InvalidOperationException("Inventory item not found.");
                return item;
            }
        }

        /// <summary>
        /// Returns all items currently at or below their reorder point (used for low-stock alerts).
        /// </summary>
        public List<InventoryItemRow> GetLowStockItems()
        {
            return GetItems(lowStockOnly: true);
        }

        /// <summary>
        /// Creates a new inventory item (itemId == 0) or updates an existing item's catalog settings (itemId > 0).
        /// For existing items, QtyOnHand and QtyReserved are preserved so stock movements stay controlled.
        /// </summary>
        public void SaveItem(
            int itemId,
            string itemCode,
            string itemName,
            string unit,
            string stockTag,
            int initialQtyOnHand,
            bool isScarce,
            int reorderPoint,
            int maxStockLevel)
        {
            RequireRole("Staff", "Owner");

            if (string.IsNullOrWhiteSpace(itemCode))
                throw new ArgumentException("Item code is required.");
            if (string.IsNullOrWhiteSpace(itemName))
                throw new ArgumentException("Item name is required.");
            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit is required.");
            if (stockTag != "Project" && stockTag != "ForSale")
                throw new ArgumentException("Stock tag must be Project or ForSale.");
            if (initialQtyOnHand < 0)
                throw new ArgumentException("Initial quantity cannot be negative.");
            if (reorderPoint < 0 || maxStockLevel < 0)
                throw new ArgumentException("Reorder point and max stock level cannot be negative.");
            if (isScarce && maxStockLevel <= reorderPoint)
                throw new ArgumentException("For scarce items, Max Stock Level must be greater than Reorder Point.");

            string cleanCode = itemCode.Trim().ToUpper();
            string cleanName = itemName.Trim();
            string cleanUnit = unit.Trim();

            var options = new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable };
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
                using (var db = new ErpDataContext())
                {
                    bool duplicateCode = db.Items.Any(i => i.ItemCode.ToLower() == cleanCode.ToLower() && i.ItemId != itemId);
                    if (duplicateCode)
                        throw new InvalidOperationException("Item code '" + cleanCode + "' already exists.");

                    if (itemId == 0)
                    {
                        var newItem = new Item
                        {
                            ItemCode = cleanCode,
                            ItemName = cleanName,
                            Unit = cleanUnit,
                            StockTag = stockTag,
                            QtyOnHand = initialQtyOnHand,
                            QtyReserved = 0,
                            IsScarce = isScarce,
                            ReorderPoint = reorderPoint,
                            MaxStockLevel = maxStockLevel
                        };
                        db.Items.InsertOnSubmit(newItem);
                    }
                    else
                    {
                        var existing = db.Items.SingleOrDefault(i => i.ItemId == itemId);
                        if (existing == null)
                            throw new InvalidOperationException("Item not found.");

                        if (existing.QtyReserved > 0 && existing.StockTag != stockTag)
                            throw new InvalidOperationException("Cannot change Stock Tag while units are currently reserved.");

                        existing.ItemCode = cleanCode;
                        existing.ItemName = cleanName;
                        existing.Unit = cleanUnit;
                        existing.StockTag = stockTag;
                        existing.IsScarce = isScarce;
                        existing.ReorderPoint = reorderPoint;
                        existing.MaxStockLevel = maxStockLevel;
                    }

                    db.SubmitChanges();
                    scope.Complete();
                }
            }
            catch (ChangeConflictException)
            {
                throw new InvalidOperationException("Another user modified this item at the same time. Please try again.");
            }
        }

        private static void RequireRole(params string[] allowedRoles)
        {
            if (!allowedRoles.Contains(Session.Role))
                throw new UnauthorizedAccessException("Your role (" + Session.Role + ") is not allowed to perform this action.");
        }
    }
}