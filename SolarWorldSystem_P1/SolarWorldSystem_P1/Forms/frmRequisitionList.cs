using System;
using System.Windows.Forms;
using SolarWorldERP.Helpers;
using SolarWorldERP.Models;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    /// <summary>
    /// Lists all requisition forms (RFs) with a status filter. Selecting one shows its items.
    /// Staff can cancel or revise RFs; the owner can open the approval screen.
    /// </summary>
    public partial class frmRequisitionList : Form
    {
        private readonly RequisitionService _service = new RequisitionService();
        private RfDetail _selected;   // the RF currently highlighted in the list (null if none)

        /// <summary>Builds the controls drawn in the designer.</summary>
        public frmRequisitionList()
        {
            InitializeComponent();
        }

        /// <summary>When the form opens: fills the status filter, shows only the Review button to the owner, loads the list.</summary>
        private void frmRequisitionList_Load(object sender, EventArgs e)
        {
            cboStatus.Items.Add("All");
            foreach (RfStatus status in Enum.GetValues(typeof(RfStatus)))
                cboStatus.Items.Add(status.ToString());

            btnReview.Visible = Session.Role == "Owner";
            cboStatus.SelectedIndex = 0;   // this triggers the first LoadList()
        }

        /// <summary>Reloads the list when a different status filter is picked.</summary>
        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>"Refresh" button: reloads the list from the database.</summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadList();
        }

        /// <summary>Loads the RFs matching the status filter into the top grid.</summary>
        private void LoadList()
        {
            try
            {
                string filter = (cboStatus.SelectedItem == null || cboStatus.Text == "All") ? null : cboStatus.Text;
                gridRf.DataSource = _service.GetRequisitions(filter);
                ShowSelected();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Requisition List", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>When another row is highlighted, shows that RF's items and remarks.</summary>
        private void gridRf_SelectionChanged(object sender, EventArgs e)
        {
            ShowSelected();
        }

        /// <summary>Loads the highlighted RF (items, remarks) into the bottom grid and updates the buttons.</summary>
        private void ShowSelected()
        {
            _selected = null;
            gridLines.DataSource = null;
            lblRemarks.Text = "";

            var row = gridRf.CurrentRow == null ? null : gridRf.CurrentRow.DataBoundItem as RfListRow;
            if (row != null)
            {
                try
                {
                    _selected = _service.GetRequisition(row.RfId);
                    gridLines.DataSource = _selected.Lines;
                    if (!string.IsNullOrWhiteSpace(_selected.Remarks))
                        lblRemarks.Text = "Remarks: " + _selected.Remarks;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Requisition List", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            UpdateButtons();
        }

        /// <summary>Turns each button on or off depending on the RF's status and the user's role.</summary>
        private void UpdateButtons()
        {
            string status = _selected == null ? "" : _selected.Status;
            bool canEdit = Session.Role == "Staff" || Session.Role == "Owner";

            btnReview.Enabled = status == RfStatus.Submitted.ToString();
            btnRevise.Enabled = canEdit && status == RfStatus.ReturnedForRevision.ToString();
            btnCancelRf.Enabled = canEdit &&
                (status == RfStatus.Submitted.ToString() || status == RfStatus.ReturnedForRevision.ToString());
        }

        /// <summary>"Review / Approve" (owner): opens the approval window for the selected RF, then refreshes the list.</summary>
        private void btnReview_Click(object sender, EventArgs e)
        {
            if (_selected == null) return;
            using (var f = new frmApproval(_selected.RfId))
            {
                f.ShowDialog(this);
            }
            LoadList();
        }

        /// <summary>"Revise &amp; Resubmit": opens the requisition form pre-filled with the returned RF; refreshes the list when it closes.</summary>
        private void btnRevise_Click(object sender, EventArgs e)
        {
            if (_selected == null) return;
            var f = new frmRequisition(_selected.RfId);
            f.MdiParent = this.MdiParent;
            f.FormClosed += (s, args) => LoadList();
            f.Show();
        }

        /// <summary>"Cancel RF": asks to confirm, then cancels the selected RF and releases its reserved stock.</summary>
        private void btnCancelRf_Click(object sender, EventArgs e)
        {
            if (_selected == null) return;
            var answer = MessageBox.Show("Cancel " + _selected.RfNumber + "? Reserved stock will be released.",
                                         "Cancel requisition", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes) return;

            try
            {
                _service.Cancel(_selected.RfId);
                LoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not cancel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
