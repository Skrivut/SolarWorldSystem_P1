using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    public partial class frmInventory : Form
    {
        private readonly InventoryService _service = new InventoryService();
        private bool _isLoading;
        private int _editingItemId = 0;

        public frmInventory()
        {
            InitializeComponent();
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            _isLoading = true;

            cboStockTag.Items.Clear();
            cboStockTag.Items.AddRange(new object[] { "All", "Project", "ForSale" });
            cboStockTag.SelectedIndex = 0;

            cboEditTag.Items.Clear();
            cboEditTag.Items.AddRange(new object[] { "Project", "ForSale" });
            cboEditTag.SelectedIndex = 0;

            _isLoading = false;

            LoadInventory();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (!_isLoading)
                LoadInventory();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void LoadInventory()
        {
            try
            {
                string tag = cboStockTag.SelectedItem != null ? cboStockTag.SelectedItem.ToString() : "All";
                var items = _service.GetItems(tag, txtSearch.Text, chkLowStockOnly.Checked);

                gridInventory.DataSource = items;

                int lowCount = items.Count(i => i.IsLowStock);
                lblSummary.Text = string.Format("Showing {0} item(s) | Low-stock alerts: {1}", items.Count, lowCount);

                if (items.Count == 0)
                    PrepareNewItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load inventory: " + ex.Message, "SolarWorld ERP",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridInventory_SelectionChanged(object sender, EventArgs e)
        {
            if (gridInventory.CurrentRow == null) return;

            var row = gridInventory.CurrentRow.DataBoundItem as InventoryItemRow;
            if (row == null) return;

            _editingItemId = row.ItemId;
            lblMode.Text = "Mode: Editing " + row.ItemCode;
            txtItemCode.Text = row.ItemCode;
            txtItemName.Text = row.ItemName;
            txtUnit.Text = row.Unit;
            cboEditTag.SelectedItem = row.StockTag;
            numInitialQty.Value = row.QtyOnHand;
            numInitialQty.Enabled = false; // Existing stock changes only through Receiving or Issuance
            numReorderPoint.Value = row.ReorderPoint;
            numMaxStock.Value = row.MaxStockLevel;
            chkIsScarce.Checked = row.IsScarce;
        }

        private void btnNewItem_Click(object sender, EventArgs e)
        {
            gridInventory.ClearSelection();
            PrepareNewItem();
        }

        private void PrepareNewItem()
        {
            _editingItemId = 0;
            lblMode.Text = "Mode: Creating New Item";
            txtItemCode.Text = "";
            txtItemName.Text = "";
            txtUnit.Text = "pcs";
            cboEditTag.SelectedIndex = 0;
            numInitialQty.Value = 0;
            numInitialQty.Enabled = true;
            numReorderPoint.Value = 0;
            numMaxStock.Value = 0;
            chkIsScarce.Checked = false;
            txtItemCode.Focus();
        }

        private void btnSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                string tag = cboEditTag.SelectedItem != null ? cboEditTag.SelectedItem.ToString() : "Project";

                _service.SaveItem(
                    _editingItemId,
                    txtItemCode.Text,
                    txtItemName.Text,
                    txtUnit.Text,
                    tag,
                    (int)numInitialQty.Value,
                    chkIsScarce.Checked,
                    (int)numReorderPoint.Value,
                    (int)numMaxStock.Value
                );

                MessageBox.Show("Item saved successfully.", "SolarWorld ERP",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SolarWorld ERP",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gridInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = gridInventory.Rows[e.RowIndex].DataBoundItem as InventoryItemRow;
            if (row != null && row.IsLowStock)
            {
                e.CellStyle.BackColor = Color.MistyRose;
                e.CellStyle.ForeColor = Color.DarkRed;
            }
        }
    }
}