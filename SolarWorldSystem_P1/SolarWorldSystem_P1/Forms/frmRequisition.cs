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
    public partial class frmRequisition : Form
    {
        public class LineRow
        {
            public int ItemId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public int Qty { get; set; }
        }

        public class ItemChoice
        {
            public int ItemId { get; set; }
            public string Text { get; set; }
        }

        private readonly RequisitionService _service = new RequisitionService();
        private readonly BindingList<LineRow> _lines = new BindingList<LineRow>();
        private List<Item> _items = new List<Item>();

        public frmRequisition()
        {
            InitializeComponent();
            grid.DataSource = _lines;
        }

        private void frmRequisition_Load(object sender, EventArgs e)
        {
            LoadItems();
        }

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

        private Item SelectedItem()
        {
            var choice = cboItem.SelectedItem as ItemChoice;
            return choice == null ? null : _items.FirstOrDefault(i => i.ItemId == choice.ItemId);
        }

        private void ShowAvailability()
        {
            var item = SelectedItem();
            lblAvail.Text = item == null ? "" :
                "Available: " + Math.Max(0, item.QtyOnHand - item.QtyReserved) + " " + item.Unit;
        }

        private void cboItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowAvailability();
        }

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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;
            _lines.RemoveAt(grid.CurrentRow.Index);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _lines.Clear();
            txtRequestedBy.Clear();
            txtProject.Clear();
            nudQty.Value = 1;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var lines = _lines.Select(l => new SubmitLine { ItemId = l.ItemId, Qty = l.Qty }).ToList();
                var result = _service.Submit(txtRequestedBy.Text, txtProject.Text, lines);

                var sb = new StringBuilder();
                sb.AppendLine("Requisition " + result.RfNumber + " submitted.");
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

                ClearForm();
                LoadItems();   // refresh available quantities
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
