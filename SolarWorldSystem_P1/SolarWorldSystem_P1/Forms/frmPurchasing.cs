using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SolarWorldERP.Helpers;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    // The logic of the purchasing screen. The layout lives in frmPurchasing.Designer.cs.
    public partial class frmPurchasing : Form
    {
        private readonly PurchasingService _purchasing = new PurchasingService();
        private Dictionary<int, string> _supplierNames = new Dictionary<int, string>();
        private Dictionary<int, string> _itemNames = new Dictionary<int, string>();

        public frmPurchasing()
        {
            InitializeComponent();
        }

        private void frmPurchasing_Load(object sender, EventArgs e)
        {
            if (Session.Role != "Owner" && Session.Role != "Staff")
            {
                Warn("Only the owner and office staff can use purchasing.");
                Close();
                return;
            }
            colNewCost.DefaultCellStyle.Format = "N2";
            Run(() => { LoadSuppliers(); LoadItems(); });
            cboStatus.SelectedIndex = 0;   // this triggers the first load of the order list
        }

        // ---- Loading ----
        private void LoadSuppliers()
        {
            var all = _purchasing.GetSuppliers(false);
            _supplierNames = all.ToDictionary(s => s.SupplierId, s => s.SupplierName);
            cboSupplier.DataSource = all.Where(s => s.IsActive)
                .Select(s => new { Id = s.SupplierId, Name = s.SupplierName }).ToList();
            cboSupplier.DisplayMember = "Name";
            cboSupplier.ValueMember = "Id";
        }

        private void LoadItems()
        {
            var items = _purchasing.GetItems();
            _itemNames = items.ToDictionary(i => i.ItemId, i => i.ItemCode + " - " + i.ItemName);
            cboItem.DataSource = items.Select(i => new { Id = i.ItemId, Name = i.ItemCode + " - " + i.ItemName }).ToList();
            cboItem.DisplayMember = "Name";
            cboItem.ValueMember = "Id";
        }

        private void LoadOrders()
        {
            Run(() =>
            {
                var status = cboStatus.Text == "All" ? null : cboStatus.Text;
                dgvOrders.DataSource = _purchasing.GetOrders(status).Select(o => new
                {
                    PurchaseOrderId = o.PurchaseOrderId,
                    Number = o.PoNumber,
                    Supplier = _supplierNames.ContainsKey(o.SupplierId) ? _supplierNames[o.SupplierId] : "",
                    Status = o.Status,
                    Created = o.DateCreated,
                    Expected = o.ExpectedDate,
                    Remarks = o.Remarks
                }).ToList();
                dgvOrders.Columns["PurchaseOrderId"].Visible = false;
                dgvOrders.Columns["Created"].DefaultCellStyle.Format = "yyyy-MM-dd";
                dgvOrders.Columns["Expected"].DefaultCellStyle.Format = "yyyy-MM-dd";
            });
            ShowOrder();
        }

        // Shows the items of the selected order and enables only the buttons that make sense for its status.
        private void ShowOrder()
        {
            var id = SelectedOrderId();
            var status = SelectedOrderStatus();
            btnPlace.Enabled = status == "Draft";
            btnCancelOrder.Enabled = status == "Draft" || status == "Ordered";
            dgvOrderLines.DataSource = null;
            if (id == 0) return;

            Run(() =>
            {
                dgvOrderLines.DataSource = _purchasing.GetLines(id).Select(l => new
                {
                    Item = _itemNames.ContainsKey(l.ItemId) ? _itemNames[l.ItemId] : "Item " + l.ItemId,
                    Ordered = l.QtyOrdered,
                    Received = l.QtyReceived,
                    UnitCost = l.UnitCost
                }).ToList();
                dgvOrderLines.Columns["UnitCost"].DefaultCellStyle.Format = "N2";
            });
        }

        // ---- Order list events ----
        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e) { LoadOrders(); }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Run(() => { LoadSuppliers(); LoadItems(); });
            LoadOrders();
        }

        private void dgvOrders_SelectionChanged(object sender, EventArgs e) { ShowOrder(); }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            var id = SelectedOrderId();
            if (id == 0) return;
            if (MessageBox.Show("Place this order with the supplier?", "Purchasing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            Run(() => { _purchasing.Place(id); LoadOrders(); });
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            var id = SelectedOrderId();
            if (id == 0) return;
            if (MessageBox.Show("Cancel this order?", "Purchasing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            Run(() => { _purchasing.Cancel(id); LoadOrders(); });
        }

        // ---- New order events ----
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            Run(() =>
            {
                var supplier = _purchasing.AddSupplier(txtNewSupplier.Text, null);
                txtNewSupplier.Clear();
                LoadSuppliers();
                cboSupplier.SelectedValue = supplier.SupplierId;
            });
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            if (cboItem.SelectedValue == null) { Warn("Choose an item."); return; }
            var itemId = Convert.ToInt32(cboItem.SelectedValue);

            // Unit cost is optional: leave it blank if the price is not known yet.
            decimal? cost = null;
            if (!string.IsNullOrWhiteSpace(txtCost.Text))
            {
                decimal parsed;
                if (!decimal.TryParse(txtCost.Text, out parsed) || parsed < 0)
                {
                    Warn("Unit cost must be a number, or leave it blank.");
                    return;
                }
                cost = parsed;
            }
            foreach (DataGridViewRow row in dgvNewLines.Rows)
                if (Convert.ToInt32(row.Cells[colNewItemId.Index].Value) == itemId)
                {
                    Warn("That item is already on the order.");
                    return;
                }

            dgvNewLines.Rows.Add(itemId, cboItem.Text, (int)numQty.Value, cost.HasValue ? (object)cost.Value : null);
            numQty.Value = 1;
            txtCost.Clear();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvNewLines.CurrentRow != null) dgvNewLines.Rows.Remove(dgvNewLines.CurrentRow);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (cboSupplier.SelectedValue == null) { Warn("Choose a supplier."); return; }
            if (dgvNewLines.Rows.Count == 0) { Warn("Add at least one item to the order."); return; }

            var lines = new List<PurchaseLineInput>();
            foreach (DataGridViewRow row in dgvNewLines.Rows)
            {
                var rawCost = row.Cells[colNewCost.Index].Value;
                lines.Add(new PurchaseLineInput
                {
                    ItemId = Convert.ToInt32(row.Cells[colNewItemId.Index].Value),
                    Qty = Convert.ToInt32(row.Cells[colNewQty.Index].Value),
                    UnitCost = rawCost == null ? (decimal?)null : Convert.ToDecimal(rawCost)
                });
            }
            DateTime? expected = dtpExpected.Checked ? (DateTime?)dtpExpected.Value.Date : null;

            Run(() =>
            {
                // The second argument (null) is the requisition this order covers; this screen does not link one yet.
                var order = _purchasing.CreateOrder(Convert.ToInt32(cboSupplier.SelectedValue), null, expected, txtRemarks.Text, lines);
                dgvNewLines.Rows.Clear();
                txtRemarks.Clear();
                LoadOrders();
                MessageBox.Show("Purchase order " + order.PoNumber + " was created as a draft. Place it when you are ready to order.",
                    "Purchasing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        // ---- Helpers ----
        private int SelectedOrderId()
        {
            if (dgvOrders.CurrentRow == null || !dgvOrders.Columns.Contains("PurchaseOrderId")) return 0;
            return Convert.ToInt32(dgvOrders.CurrentRow.Cells["PurchaseOrderId"].Value);
        }

        private string SelectedOrderStatus()
        {
            if (dgvOrders.CurrentRow == null || !dgvOrders.Columns.Contains("Status")) return "";
            return Convert.ToString(dgvOrders.CurrentRow.Cells["Status"].Value);
        }

        private void Warn(string message)
        {
            MessageBox.Show(message, "Purchasing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Runs an action and shows any rule violation from the service in a message box.
        private void Run(Action action)
        {
            try { action(); }
            catch (Exception ex) { Warn(ex.Message); }
        }
    }
}