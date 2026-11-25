using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    // The logic of the purchasing screen. The layout lives in frmPurchasing.Designer.cs.
    // Staff create an order (optionally for a requisition's shortage) and submit it for approval.
    // The owner approves (the PO goes to the supplier and the driver gets a pickup task) or rejects it.
    // After goods arrive, "Follow up supplier" sends the driver again for any balance or replacement.
    public partial class frmPurchasing : Form
    {
        private readonly PurchasingService _purchasing = new PurchasingService();
        private Dictionary<int, string> _supplierNames = new Dictionary<int, string>();
        private Dictionary<int, string> _supplierAddresses = new Dictionary<int, string>();
        private Dictionary<int, string> _itemNames = new Dictionary<int, string>();

        // Set when the lines on the new-order grid were loaded from a requisition's shortage.
        private int? _rfId;

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
            btnApprove.Visible = Session.Role == "Owner";   // only the owner approves or rejects
            btnReject.Visible = Session.Role == "Owner";
            colNewCost.DefaultCellStyle.Format = "N2";
            LoadLookups();
            cboStatus.SelectedIndex = 0;   // this triggers the first load of the order list
        }

        // ---- Loading ----
        private void LoadLookups()
        {
            Run(() => { LoadSuppliers(); LoadItems(); LoadRequisitions(); });
        }

        private void LoadSuppliers()
        {
            var all = _purchasing.GetSuppliers(false);
            _supplierNames = all.ToDictionary(s => s.SupplierId, s => s.SupplierName);
            _supplierAddresses = all.ToDictionary(s => s.SupplierId, s => s.Address ?? "");
            cboSupplier.DataSource = all.Where(s => s.IsActive)
                .Select(s => new { Id = s.SupplierId, Name = s.SupplierName }).ToList();
            cboSupplier.DisplayMember = "Name";
            cboSupplier.ValueMember = "Id";
            ShowSupplierAddress();
        }

        private void LoadItems()
        {
            var items = _purchasing.GetItems();
            _itemNames = items.ToDictionary(i => i.ItemId, i => i.ItemCode + " - " + i.ItemName);
            cboItem.DataSource = items.Select(i => new { Id = i.ItemId, Name = i.ItemCode + " - " + i.ItemName }).ToList();
            cboItem.DisplayMember = "Name";
            cboItem.ValueMember = "Id";
        }

        // Approved requisitions that are short on stock and have no open order yet.
        private void LoadRequisitions()
        {
            var list = _purchasing.GetRequisitionsWithShortage()
                .Select(r => new { Id = r.RfId, Text = r.RfNumber + " - " + r.ProjectName + " (short " + r.TotalShort + ")" })
                .ToList();
            cboRf.DataSource = list;
            cboRf.DisplayMember = "Text";
            cboRf.ValueMember = "Id";
            if (list.Count == 0) cboRf.SelectedIndex = -1;
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
                    ApprovedBy = o.ApprovedBy,
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
            btnSubmitApproval.Enabled = status == "Draft";
            btnApprove.Enabled = status == "PendingApproval";
            btnReject.Enabled = status == "PendingApproval";
            btnCancelOrder.Enabled = status == "Draft" || status == "PendingApproval" || status == "Ordered";
            btnFollowUp.Enabled = status == "Ordered" || status == "PartiallyReceived";
            dgvOrderLines.DataSource = null;
            if (id == 0) return;

            Run(() =>
            {
                dgvOrderLines.DataSource = _purchasing.GetLines(id).Select(l => new
                {
                    Item = _itemNames.ContainsKey(l.ItemId) ? _itemNames[l.ItemId] : "Item " + l.ItemId,
                    Ordered = l.QtyOrdered,
                    Accepted = l.QtyReceived,
                    Outstanding = l.QtyOrdered - l.QtyReceived,
                    UnitCost = l.UnitCost
                }).ToList();
                dgvOrderLines.Columns["UnitCost"].DefaultCellStyle.Format = "N2";
            });
        }

        private void ShowSupplierAddress()
        {
            if (cboSupplier.SelectedValue == null) { txtSupplierAddress.Clear(); return; }
            string address;
            txtSupplierAddress.Text = _supplierAddresses.TryGetValue(Convert.ToInt32(cboSupplier.SelectedValue), out address) ? address : "";
        }

        // ---- Order list events ----
        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e) { LoadOrders(); }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLookups();
            LoadOrders();
        }

        private void dgvOrders_SelectionChanged(object sender, EventArgs e) { ShowOrder(); }

        private void btnSubmitApproval_Click(object sender, EventArgs e)
        {
            var id = SelectedOrderId();
            if (id == 0) return;
            if (MessageBox.Show("Send this order to the owner for approval?", "Purchasing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            Run(() => { _purchasing.SubmitForApproval(id); LoadOrders(); });
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            var id = SelectedOrderId();
            if (id == 0) return;
            if (MessageBox.Show("Approve this order? It is sent to the supplier and the driver gets a pickup task.",
                "Purchasing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            Run(() =>
            {
                _purchasing.Approve(id);
                LoadRequisitions();
                LoadOrders();
                MessageBox.Show("The order was approved and a pickup task was created for the driver.", "Purchasing",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            var id = SelectedOrderId();
            if (id == 0) return;
            var reason = Ask("Reject order", "Reason for rejecting this purchase order:");
            if (reason == null) return;
            Run(() => { _purchasing.Reject(id, reason); LoadRequisitions(); LoadOrders(); });
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            var id = SelectedOrderId();
            if (id == 0) return;
            if (MessageBox.Show("Cancel this order?", "Purchasing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            Run(() => { _purchasing.Cancel(id); LoadRequisitions(); LoadOrders(); });
        }

        // The balance or a replacement is still owed: the driver gets another pickup task.
        private void btnFollowUp_Click(object sender, EventArgs e)
        {
            var id = SelectedOrderId();
            if (id == 0) return;
            var note = Ask("Follow up supplier", "Note for the driver (optional, for example what the supplier agreed to send):");
            if (note == null) return;
            Run(() =>
            {
                _purchasing.FollowUp(id, note);
                MessageBox.Show("A new pickup task was created for the driver.", "Purchasing",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        // ---- Supplier events ----
        private void cboSupplier_SelectedIndexChanged(object sender, EventArgs e) { ShowSupplierAddress(); }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            Run(() =>
            {
                var supplier = _purchasing.AddSupplier(txtNewSupplier.Text, null, txtSupplierAddress.Text);
                txtNewSupplier.Clear();
                LoadSuppliers();
                cboSupplier.SelectedValue = supplier.SupplierId;
            });
        }

        private void btnSaveAddress_Click(object sender, EventArgs e)
        {
            if (cboSupplier.SelectedValue == null) { Warn("Choose a supplier."); return; }
            var supplierId = Convert.ToInt32(cboSupplier.SelectedValue);
            Run(() =>
            {
                _purchasing.SetSupplierAddress(supplierId, txtSupplierAddress.Text);
                LoadSuppliers();
                cboSupplier.SelectedValue = supplierId;
            });
        }

        // ---- New order events ----
        // Fills the grid with what a requisition is short of, so it can be ordered in one go.
        private void btnLoadRf_Click(object sender, EventArgs e)
        {
            if (cboRf.SelectedValue == null) { Warn("There are no approved requisitions with a shortage."); return; }
            var rfId = Convert.ToInt32(cboRf.SelectedValue);
            Run(() =>
            {
                var shortage = _purchasing.GetShortageLines(rfId);
                dgvNewLines.Rows.Clear();
                foreach (var l in shortage)
                    dgvNewLines.Rows.Add(l.ItemId, l.ItemCode + " - " + l.ItemName, l.QtyShort, null);
                _rfId = rfId;
                grpNew.Text = "New purchase order for " + cboRf.Text;
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

        private void btnClearNew_Click(object sender, EventArgs e) { ClearNewOrder(); }

        private void ClearNewOrder()
        {
            dgvNewLines.Rows.Clear();
            txtRemarks.Clear();
            _rfId = null;
            grpNew.Text = "New purchase order";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            dgvNewLines.EndEdit();
            if (cboSupplier.SelectedValue == null) { Warn("Choose a supplier."); return; }
            if (dgvNewLines.Rows.Count == 0) { Warn("Add at least one item to the order."); return; }

            var lines = new List<PurchaseLineInput>();
            foreach (DataGridViewRow row in dgvNewLines.Rows)
            {
                int qty;
                if (!int.TryParse(Convert.ToString(row.Cells[colNewQty.Index].Value), out qty) || qty <= 0)
                {
                    Warn("Every quantity must be a whole number above zero.");
                    return;
                }
                decimal? cost = null;
                var rawCost = Convert.ToString(row.Cells[colNewCost.Index].Value);
                if (!string.IsNullOrWhiteSpace(rawCost))
                {
                    decimal parsed;
                    if (!decimal.TryParse(rawCost, out parsed) || parsed < 0)
                    {
                        Warn("Unit costs must be numbers, or blank.");
                        return;
                    }
                    cost = parsed;
                }
                lines.Add(new PurchaseLineInput
                {
                    ItemId = Convert.ToInt32(row.Cells[colNewItemId.Index].Value),
                    Qty = qty,
                    UnitCost = cost
                });
            }
            DateTime? expected = dtpExpected.Checked ? (DateTime?)dtpExpected.Value.Date : null;
            var supplierId = Convert.ToInt32(cboSupplier.SelectedValue);

            Run(() =>
            {
                var order = _purchasing.CreateOrder(supplierId, _rfId, expected, txtRemarks.Text, lines);
                ClearNewOrder();
                LoadRequisitions();
                LoadOrders();
                MessageBox.Show("Purchase order " + order.PoNumber + " was created as a draft. Submit it for approval when it is ready.",
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

        // A small box that asks for one line of text. Returns null if the person cancels.
        private static string Ask(string title, string prompt)
        {
            using (var f = new Form())
            {
                f.Text = title;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MinimizeBox = false;
                f.MaximizeBox = false;
                f.ClientSize = new Size(440, 120);
                var label = new Label { Text = prompt, Left = 12, Top = 12, Width = 416, Height = 32 };
                var box = new TextBox { Left = 12, Top = 48, Width = 416 };
                var ok = new Button { Text = "OK", Left = 272, Top = 82, Width = 75, DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "Cancel", Left = 353, Top = 82, Width = 75, DialogResult = DialogResult.Cancel };
                f.Controls.AddRange(new Control[] { label, box, ok, cancel });
                f.AcceptButton = ok;
                f.CancelButton = cancel;
                return f.ShowDialog() == DialogResult.OK ? box.Text : null;
            }
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