using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SolarWorldERP.Helpers;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    // The logic of the delivery screen. The layout lives in frmDelivery.Designer.cs.
    public partial class frmDelivery : Form
    {
        private readonly DeliveryService _deliveries = new DeliveryService();
        private readonly PurchasingService _purchasing = new PurchasingService();

        public frmDelivery()
        {
            InitializeComponent();
        }

        private bool CanManage
        {
            get { return Session.Role == "Owner" || Session.Role == "Staff"; }
        }

        private void frmDelivery_Load(object sender, EventArgs e)
        {
            grpSchedule.Visible = CanManage;   // drivers can only move and receive deliveries
            btnCancel.Visible = CanManage;
            dtpExpected.Value = DateTime.Today.AddDays(1);
            if (CanManage) LoadOpenOrders();
            cboStatus.SelectedIndex = 0;       // this triggers the first load of the list
        }

        // ---- Loading ----
        private void LoadDeliveries()
        {
            Run(() =>
            {
                var status = cboStatus.Text == "All" ? null : cboStatus.Text;
                var numbers = _deliveries.GetOrderNumbers();
                dgvDeliveries.DataSource = _deliveries.GetDeliveries(status).Select(d => new
                {
                    DeliveryId = d.DeliveryId,
                    Number = d.DeliveryNumber,
                    PurchaseOrder = numbers.ContainsKey(d.PurchaseOrderId) ? numbers[d.PurchaseOrderId] : "",
                    Status = d.Status,
                    Expected = d.ExpectedDate,
                    Driver = d.DriverName,
                    Address = d.DeliveryAddress,
                    Delivered = d.DeliveredDate
                }).ToList();
                dgvDeliveries.Columns["DeliveryId"].Visible = false;
                dgvDeliveries.Columns["Expected"].DefaultCellStyle.Format = "yyyy-MM-dd";
                dgvDeliveries.Columns["Delivered"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            });
            ShowSelected();
        }

        private void LoadOpenOrders()
        {
            Run(() =>
            {
                var orders = _purchasing.GetOrders("Ordered")
                    .Concat(_purchasing.GetOrders("PartiallyReceived"))
                    .Select(o => new { Id = o.PurchaseOrderId, Number = o.PoNumber })
                    .ToList();
                cboOrder.DataSource = orders;
                cboOrder.DisplayMember = "Number";
                cboOrder.ValueMember = "Id";
            });
        }

        // Shows the items of the selected delivery and enables only the buttons that make sense for its status.
        private void ShowSelected()
        {
            dgvLines.Rows.Clear();
            var id = SelectedDeliveryId();
            var status = SelectedStatus();
            var open = status == "Scheduled" || status == "InTransit";
            btnInTransit.Enabled = status == "Scheduled";
            btnReceive.Enabled = open;
            btnCancel.Enabled = open;
            colReceiveNow.ReadOnly = !open;
            if (id == 0) return;

            Run(() =>
            {
                foreach (var l in _deliveries.GetReceivableLines(id))
                    dgvLines.Rows.Add(l.PurchaseOrderLineId, l.ItemCode + " - " + l.ItemName,
                        l.QtyOrdered, l.QtyReceived, l.QtyRemaining, open ? l.QtyRemaining : 0);
            });
        }

        // ---- Events ----
        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e) { LoadDeliveries(); }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (CanManage) LoadOpenOrders();
            LoadDeliveries();
        }

        private void dgvDeliveries_SelectionChanged(object sender, EventArgs e) { ShowSelected(); }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            Run(() =>
            {
                if (cboOrder.SelectedValue == null) throw new InvalidOperationException("Choose a purchase order.");
                _deliveries.Schedule(Convert.ToInt32(cboOrder.SelectedValue), txtAddress.Text, dtpExpected.Value, txtDriver.Text);
                txtAddress.Clear();
                txtDriver.Clear();
                LoadDeliveries();
            });
        }

        private void btnInTransit_Click(object sender, EventArgs e)
        {
            var id = SelectedDeliveryId();
            if (id == 0) return;
            Run(() => { _deliveries.MarkInTransit(id); LoadDeliveries(); });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var id = SelectedDeliveryId();
            if (id == 0) return;
            if (MessageBox.Show("Cancel this delivery?", "Deliveries", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            Run(() => { _deliveries.Cancel(id); LoadDeliveries(); });
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            var id = SelectedDeliveryId();
            if (id == 0) return;
            dgvLines.EndEdit();

            // Collect the "Receive now" column; blank or 0 means nothing arrived for that item.
            var quantities = new Dictionary<int, int>();
            foreach (DataGridViewRow row in dgvLines.Rows)
            {
                var text = Convert.ToString(row.Cells[colReceiveNow.Index].Value);
                if (string.IsNullOrWhiteSpace(text)) continue;
                int qty;
                if (!int.TryParse(text, out qty) || qty < 0)
                {
                    Warn("Quantities must be whole numbers.");
                    return;
                }
                if (qty > 0) quantities[Convert.ToInt32(row.Cells[colLineId.Index].Value)] = qty;
            }
            if (quantities.Count == 0)
            {
                Warn("Enter a quantity for at least one item.");
                return;
            }
            if (MessageBox.Show("Add these items to stock and mark the delivery as received?", "Deliveries",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Run(() =>
            {
                _deliveries.Receive(id, quantities, txtNotes.Text);
                txtNotes.Clear();
                if (CanManage) LoadOpenOrders();
                LoadDeliveries();
            });
        }

        // ---- Helpers ----
        private int SelectedDeliveryId()
        {
            if (dgvDeliveries.CurrentRow == null || !dgvDeliveries.Columns.Contains("DeliveryId")) return 0;
            return Convert.ToInt32(dgvDeliveries.CurrentRow.Cells["DeliveryId"].Value);
        }

        private string SelectedStatus()
        {
            if (dgvDeliveries.CurrentRow == null || !dgvDeliveries.Columns.Contains("Status")) return "";
            return Convert.ToString(dgvDeliveries.CurrentRow.Cells["Status"].Value);
        }

        private void Warn(string message)
        {
            MessageBox.Show(message, "Deliveries", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Runs an action and shows any rule violation from the service in a message box.
        private void Run(Action action)
        {
            try { action(); }
            catch (Exception ex) { Warn(ex.Message); }
        }
    }
}