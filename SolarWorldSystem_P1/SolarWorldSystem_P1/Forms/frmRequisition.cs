using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SolarWorldERP.Data;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    /// <summary>
    /// The requisition form. Opened normally it creates a new RF.
    /// Opened with an RF id (frmRequisition(rfId)) it revises a returned RF and resubmits it.
    /// </summary>
    public partial class frmRequisition : Form
    {
        /// <summary>One row of the lines grid on this form.</summary>
        public class LineRow
        {
            public int ItemId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public int Qty { get; set; }
        }

        /// <summary>One entry of the item drop-down (an id and the text shown).</summary>
        public class ItemChoice
        {
            public int ItemId { get; set; }
            public string Text { get; set; }
        }

        private readonly RequisitionService _service = new RequisitionService();
        private readonly BindingList<LineRow> _lines = new BindingList<LineRow>();
        private List<Item> _items = new List<Item>();

        // When this has a value, the form is revising that existing RF instead of creating a new one.
        private int? _editRfId;

        /// <summary>Normal start: a blank form for a new RF.</summary>
        public frmRequisition()
        {
            InitializeComponent();
            grid.DataSource = _lines;
        }

        /// <summary>Revise mode: the form loads the returned RF with this id so staff can fix it and resubmit.</summary>
        public frmRequisition(int rfId) : this()
        {
            _editRfId = rfId;
        }

        /// <summary>When the form opens: fills the item drop-down, and in revise mode loads the existing RF.</summary>
        private void frmRequisition_Load(object sender, EventArgs e)
        {
            LoadItems();
            if (_editRfId.HasValue) LoadExistingRequisition(_editRfId.Value);
        }

        /// <summary>Revise mode only: fills the boxes and grid from the saved RF and shows the owner's remarks.</summary>
        private void LoadExistingRequisition(int rfId)
        {
            try
            {
                var rf = _service.GetRequisition(rfId);
                Text = "Revise Requisition " + rf.RfNumber;
                lblRfNumberNote.Text = rf.RfNumber;
                btnSubmit.Text = "Resubmit RF";
                txtRequestedBy.Text = rf.RequestedBy;
                txtProject.Text = rf.ProjectName;

                _lines.Clear();
                foreach (var l in rf.Lines)
                    _lines.Add(new LineRow { ItemId = l.ItemId, Code = l.ItemCode, Name = l.ItemName, Unit = l.Unit, Qty = l.QtyRequested });

                if (!string.IsNullOrWhiteSpace(rf.Remarks))
                    MessageBox.Show("Owner's remarks:\n\n" + rf.Remarks, "Returned for revision",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Requisition", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Loads the requestable items from the database into the drop-down (also refreshes available quantities).</summary>
        private void LoadItems()
        {
            try
            {
                _items = _service.GetRequestableItems();
                cboItem.DataSource = _items.Select(i => new ItemChoice
                {
                    ItemId = i.ItemId,
                    Text = i.ItemCode + " - " + i.ItemName
                }).ToList();
                cboItem.DisplayMember = "Text";
                cboItem.ValueMember = "ItemId";
                ShowAvailability();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load items.\n\n" + ex.Message, "Requisition",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Returns the item currently chosen in the drop-down, or null if none.</summary>
        private Item SelectedItem()
        {
            var choice = cboItem.SelectedItem as ItemChoice;
            return choice == null ? null : _items.FirstOrDefault(i => i.ItemId == choice.ItemId);
        }

        /// <summary>Shows "Available: N unit" next to the drop-down for the chosen item.</summary>
        private void ShowAvailability()
        {
            var item = SelectedItem();
            lblAvail.Text = item == null ? "" :
                "Available: " + Math.Max(0, item.QtyOnHand - item.QtyReserved) + " " + item.Unit;
        }

        /// <summary>When another item is picked, updates the available-quantity label.</summary>
        private void cboItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowAvailability();
        }

        /// <summary>"Add line": adds the chosen item and quantity to the grid (an item can only be added once).</summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var item = SelectedItem();
            if (item == null) { MessageBox.Show("Select an item."); return; }
            if (_lines.Any(l => l.ItemId == item.ItemId))
            {
                MessageBox.Show("That item is already in the list. Remove it first to change the quantity.");
                return;
            }
            _lines.Add(new LineRow
            {
                ItemId = item.ItemId, Code = item.ItemCode, Name = item.ItemName,
                Unit = item.Unit, Qty = (int)nudQty.Value
            });
            nudQty.Value = 1;
        }

        /// <summary>"Remove selected": takes the highlighted line out of the grid.</summary>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;
            _lines.RemoveAt(grid.CurrentRow.Index);
        }

        /// <summary>"Clear" button: empties the form.</summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        /// <summary>Empties the lines grid and the text boxes and resets the quantity.</summary>
        private void ClearForm()
        {
            _lines.Clear();
            txtRequestedBy.Clear();
            txtProject.Clear();
            nudQty.Value = 1;
        }

        /// <summary>
        /// "Submit RF" / "Resubmit RF": sends the form to RequisitionService, which saves it and reserves stock.
        /// Shows what was reserved and what is short. In revise mode the form closes afterwards.
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var lines = _lines.Select(l => new SubmitLine { ItemId = l.ItemId, Qty = l.Qty }).ToList();
                bool revising = _editRfId.HasValue;

                var result = revising
                    ? _service.Resubmit(_editRfId.Value, txtRequestedBy.Text, txtProject.Text, lines)
                    : _service.Submit(txtRequestedBy.Text, txtProject.Text, lines);

                var sb = new StringBuilder();
                sb.AppendLine("Requisition " + result.RfNumber + (revising ? " resubmitted." : " submitted."));
                sb.AppendLine();
                foreach (var l in result.Lines)
                    sb.AppendLine(l.ItemName + ": requested " + l.Requested + ", reserved " + l.Reserved +
                                  (l.Shortage > 0 ? ", SHORT " + l.Shortage : "") + " " + l.Unit);
                if (result.HasShortage)
                {
                    sb.AppendLine();
                    sb.AppendLine("Some items are short. They will go to purchasing once that feature is connected.");
                }
                MessageBox.Show(sb.ToString(), "Requisition", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (revising)
                {
                    Close();
                }
                else
                {
                    ClearForm();
                    LoadItems();   // refresh available quantities
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Check your input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not submit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
