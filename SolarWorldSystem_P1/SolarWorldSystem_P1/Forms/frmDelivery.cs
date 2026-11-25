using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    // The logic of the delivery screen. The layout lives in frmDelivery.Designer.cs.
    // Driver: sees the pickup tasks, marks "picked up" (with the supplier's DR number) and "delivered to office".
    // Staff/Owner: also cancel a pickup and complete the receiving report once the goods are at the office.
    public partial class frmDelivery : Form
    {
        private readonly DeliveryService _deliveries = new DeliveryService();

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
            // Drivers only move the goods; they do not see the receiving-report controls.
            btnCancel.Visible = CanManage;
            btnReport.Visible = CanManage;
            lblNotes.Visible = CanManage;
            txtNotes.Visible = CanManage;
            colAccepted.Visible = CanManage;
            colRejected.Visible = CanManage;
            colReason.Visible = CanManage;
            colAction.Visible = CanManage;
            dgvLines.DataError += (s, args) => { args.ThrowException = false; };
            cboStatus.SelectedIndex = 0;   // this triggers the first load of the list
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
                    PickupFrom = d.PickupAddress,
                    SupplierDR = d.SupplierDrNumber,
                    Driver = d.DriverName,
                    Expected = d.ExpectedDate,
                    PickedUp = d.PickedUpDate,
                    Delivered = d.DeliveredDate,
                    Report = d.ReportNumber
                }).ToList();
                dgvDeliveries.Columns["DeliveryId"].Visible = false;
                dgvDeliveries.Columns["Expected"].DefaultCellStyle.Format = "yyyy-MM-dd";
                dgvDeliveries.Columns["PickedUp"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                dgvDeliveries.Columns["Delivered"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            });
            ShowSelected();
        }

        // Shows the items of the selected pickup and enables only the controls that make sense for its status.
        private void ShowSelected()
        {
            dgvLines.Rows.Clear();
            txtNotes.Clear();
            var id = SelectedDeliveryId();
            var status = SelectedStatus();
            txtDr.Text = id == 0 ? "" : SelectedCell("SupplierDR");
            txtDr.ReadOnly = status != "Pending";
            btnPickedUp.Enabled = status == "Pending";
            btnDelivered.Enabled = status == "PickedUp";
            btnCancel.Enabled = status == "Pending";

            var canReceive = CanManage && status == "Delivered";
            btnReport.Enabled = canReceive;
            txtNotes.Enabled = canReceive;
            colAccepted.ReadOnly = !canReceive;
            colRejected.ReadOnly = !canReceive;
            colReason.ReadOnly = !canReceive;
            colAction.ReadOnly = !canReceive;
            if (id == 0) return;

            Run(() =>
            {
                // Once received, show what the report recorded for each item.
                var report = status == "Received" ? _deliveries.GetLines(id) : new List<DeliveryLine>();
                foreach (var l in _deliveries.GetReceivableLines(id))
                {
                    var r = report.FirstOrDefault(x => x.PurchaseOrderLineId == l.PurchaseOrderLineId);
                    dgvLines.Rows.Add(
                        l.PurchaseOrderLineId,
                        l.ItemCode + " - " + l.ItemName,
                        l.QtyOrdered,
                        l.QtyReceived,
                        l.QtyRemaining,
                        r == null ? null : (object)r.QtyAccepted,
                        r == null ? null : (object)r.QtyRejected,
                        r == null ? null : r.RejectReason,
                        r == null ? null : r.DiscrepancyAction);
                }
            });
        }

        // ---- Events ----
        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e) { LoadDeliveries(); }

        private void btnRefresh_Click(object sender, EventArgs e) { LoadDeliveries(); }

        private void dgvDeliveries_SelectionChanged(object sender, EventArgs e) { ShowSelected(); }

        private void btnPickedUp_Click(object sender, EventArgs e)
        {
            var id = SelectedDeliveryId();
            if (id == 0) return;
            Run(() => { _deliveries.PickUp(id, txtDr.Text); LoadDeliveries(); });
        }

        private void btnDelivered_Click(object sender, EventArgs e)
        {
            var id = SelectedDeliveryId();
            if (id == 0) return;
            Run(() => { _deliveries.DeliverToOffice(id); LoadDeliveries(); });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var id = SelectedDeliveryId();
            if (id == 0) return;
            if (MessageBox.Show("Cancel this pickup?", "Deliveries", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            Run(() => { _deliveries.Cancel(id); LoadDeliveries(); });
        }

        // Staff checked the goods against the PO and the DR: accepted goods go to stock, rejected ones are recorded.
        private void btnReport_Click(object sender, EventArgs e)
        {
            var id = SelectedDeliveryId();
            if (id == 0) return;
            dgvLines.EndEdit();

            var inputs = new List<ReceiveInput>();
            foreach (DataGridViewRow row in dgvLines.Rows)
            {
                int accepted, rejected;
                if (!TryWholeNumber(row.Cells[colAccepted.Index].Value, out accepted) ||
                    !TryWholeNumber(row.Cells[colRejected.Index].Value, out rejected))
                {
                    Warn("Accepted and rejected quantities must be whole numbers (leave blank for none).");
                    return;
                }
                inputs.Add(new ReceiveInput
                {
                    PurchaseOrderLineId = Convert.ToInt32(row.Cells[colLineId.Index].Value),
                    QtyAccepted = accepted,
                    QtyRejected = rejected,
                    RejectReason = Convert.ToString(row.Cells[colReason.Index].Value),
                    DiscrepancyAction = Convert.ToString(row.Cells[colAction.Index].Value)
                });
            }
            if (MessageBox.Show("Add the accepted items to stock and record the receiving report?", "Deliveries",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            Run(() =>
            {
                var number = _deliveries.Receive(id, inputs, txtNotes.Text);
                LoadDeliveries();
                MessageBox.Show("Receiving report " + number + " was saved.", "Deliveries",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        }

        // ---- Helpers ----
        // A blank cell counts as 0.
        private static bool TryWholeNumber(object value, out int number)
        {
            var text = Convert.ToString(value);
            number = 0;
            if (string.IsNullOrWhiteSpace(text)) return true;
            return int.TryParse(text, out number) && number >= 0;
        }

        private int SelectedDeliveryId()
        {
            if (dgvDeliveries.CurrentRow == null || !dgvDeliveries.Columns.Contains("DeliveryId")) return 0;
            return Convert.ToInt32(dgvDeliveries.CurrentRow.Cells["DeliveryId"].Value);
        }

        private string SelectedStatus() { return SelectedCell("Status"); }

        private string SelectedCell(string column)
        {
            if (dgvDeliveries.CurrentRow == null || !dgvDeliveries.Columns.Contains(column)) return "";
            return Convert.ToString(dgvDeliveries.CurrentRow.Cells[column].Value);
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