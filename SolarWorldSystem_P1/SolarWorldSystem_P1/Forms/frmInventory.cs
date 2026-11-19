using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SolarWorldERP.Models;
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
            LoadIssuableRequisitions();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (!_isLoading)
                LoadInventory();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadInventory();
            LoadIssuableRequisitions();
        }

        private void LoadInventory()
        {
            try
            {
                string tag = cboStockTag.SelectedItem != null ? cboStockTag.SelectedItem.ToString() : "All";
                var items = _service.GetItems(tag, txtSearch.Text, chkLowStockOnly.Checked);
                Dictionary<int, int> incomingMap = _service.GetIncomingQtyByItem();

                foreach (var item in items)
                {
                    int incoming;
                    item.QtyIncoming = incomingMap.TryGetValue(item.ItemId, out incoming) ? incoming : 0;
                }

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

        private void LoadIssuableRequisitions()
        {
            try
            {
                List<RfListRow> list = _service.GetIssuableRequisitions();
                cboIssuableRf.DisplayMember = "RfNumber";
                cboIssuableRf.ValueMember = "RfId";
                cboIssuableRf.DataSource = list;
                btnIssueRf.Enabled = list.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load requisitions for issuance: " + ex.Message, "SolarWorld ERP",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIssueRf_Click(object sender, EventArgs e)
        {
            if (cboIssuableRf.SelectedValue == null) return;

            int rfId = Convert.ToInt32(cboIssuableRf.SelectedValue);
            string rfNumber = cboIssuableRf.Text;

            var confirm = MessageBox.Show(
                "Issue reserved stock for " + rfNumber + " and mark it as Fulfilled?",
                "Confirm Issuance",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _service.IssueRequisition(rfId);
                MessageBox.Show("Reserved stock issued for " + rfNumber + ".", "SolarWorld ERP",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
                LoadIssuableRequisitions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SolarWorld ERP",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            numInitialQty.Enabled = false;
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