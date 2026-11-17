using System;
using System.Windows.Forms;
using SolarWorldERP.Helpers;
using SolarWorldERP.Models;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    /// <summary>
    /// The owner's approval window for one RF. Shows the details and items, and lets the owner
    /// approve it, return it for revision, or reject it (a remark is required for the last two).
    /// </summary>
    public partial class frmApproval : Form
    {
        private readonly RequisitionService _service = new RequisitionService();
        private int _rfId;

        /// <summary>Parameterless constructor so the Visual Studio designer can open this form.</summary>
        public frmApproval()
        {
            InitializeComponent();
        }

        /// <summary>Normal use: opens the approval window for the RF with this id.</summary>
        public frmApproval(int rfId) : this()
        {
            _rfId = rfId;
        }

        /// <summary>When the window opens: loads the RF, fills the labels and grid, and enables the buttons only if the owner can act.</summary>
        private void frmApproval_Load(object sender, EventArgs e)
        {
            try
            {
                var rf = _service.GetRequisition(_rfId);
                lblRfValue.Text = rf.RfNumber;
                lblProjValue.Text = rf.ProjectName;
                lblByValue.Text = rf.RequestedBy;
                lblDateValue.Text = rf.DateCreated.ToString("g");
                lblStatusValue.Text = rf.Status;
                gridLines.DataSource = rf.Lines;

                if (rf.HasShortage)
                    lblNote.Text = "Some items are short. Purchasing will draft a PO for the shortage (connected later).";

                // Only a Submitted RF can be decided, and only by the owner.
                bool canDecide = Session.Role == "Owner" && rf.Status == RfStatus.Submitted.ToString();
                btnApprove.Enabled = canDecide;
                btnReturn.Enabled = canDecide;
                btnReject.Enabled = canDecide;
                txtRemarks.Enabled = canDecide;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Review Requisition", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        /// <summary>"Approve": marks the RF Approved (records who and when) and closes the window.</summary>
        private void btnApprove_Click(object sender, EventArgs e)
        {
            Decide(() => _service.Approve(_rfId), "Requisition approved.");
        }

        /// <summary>"Return for revision": sends the RF back to staff with the typed remark and releases its stock.</summary>
        private void btnReturn_Click(object sender, EventArgs e)
        {
            Decide(() => _service.ReturnForRevision(_rfId, txtRemarks.Text), "Requisition returned to staff.");
        }

        /// <summary>"Reject": after confirmation, cancels the RF with the typed remark and releases its stock.</summary>
        private void btnReject_Click(object sender, EventArgs e)
        {
            var answer = MessageBox.Show("Reject this requisition? It will be cancelled.", "Reject",
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes) return;
            Decide(() => _service.Reject(_rfId, txtRemarks.Text), "Requisition rejected.");
        }

        /// <summary>Runs one decision, shows a success message and closes; shows the problem instead if it fails (e.g. remark missing).</summary>
        private void Decide(Action action, string successMessage)
        {
            try
            {
                action();
                MessageBox.Show(successMessage, "Review Requisition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Check your input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
