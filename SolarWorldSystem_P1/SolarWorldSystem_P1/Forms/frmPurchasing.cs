using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SolarWorldERP.Data;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    public class frmPurchasing : Form
    {
        private readonly PurchasingService _service = new PurchasingService();
        private readonly DataGridView _products = new DataGridView();
        private readonly DataGridView _orders = new DataGridView();
        private readonly TextBox _supplier = new TextBox();
        private readonly NumericUpDown _quantity = new NumericUpDown();
        private readonly Button _create = new Button();
        private readonly Button _approve = new Button();
        private readonly Button _send = new Button();

        public frmPurchasing()
        {
            Text = "Purchasing";
            Width = 900;
            Height = 600;
            StartPosition = FormStartPosition.CenterParent;
            BuildControls();
            LoadData();
        }

        private void BuildControls()
        {
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42, Padding = new Padding(6) };
            top.Controls.Add(new Label { Text = "Supplier:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
            _supplier.Width = 150;
            top.Controls.Add(_supplier);
            top.Controls.Add(new Label { Text = "Quantity:", AutoSize = true, Padding = new Padding(10, 6, 0, 0) });
            _quantity.Minimum = 1;
            _quantity.Maximum = 100000;
            _quantity.Value = 1;
            top.Controls.Add(_quantity);
            _create.Text = "Create Order";
            _create.Click += CreateOrder;
            top.Controls.Add(_create);
            _approve.Text = "Approve Selected";
            _approve.Click += ApproveOrder;
            top.Controls.Add(_approve);
            _send.Text = "Send to Supplier";
            _send.Click += SendOrder;
            top.Controls.Add(_send);
            Controls.Add(top);

            var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 250 };
            ConfigureGrid(_products);
            ConfigureGrid(_orders);
            split.Panel1.Controls.Add(_products);
            split.Panel2.Controls.Add(_orders);
            Controls.Add(split);
        }

        private static void ConfigureGrid(DataGridView grid)
        {
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoGenerateColumns = true;
        }

        private void LoadData()
        {
            try
            {
                _products.DataSource = _service.GetProducts();
                _orders.DataSource = _service.GetOrders(null);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Purchasing", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void CreateOrder(object sender, EventArgs e)
        {
            try
            {
                if (_products.CurrentRow == null) throw new InvalidOperationException("Select a product first.");
                var item = (Item)_products.CurrentRow.DataBoundItem;
                var quantities = new Dictionary<int, int> { { item.ItemId, (int)_quantity.Value } };
                var costs = new Dictionary<int, decimal> { { item.ItemId, 0 } };
                _service.CreateOrder(_supplier.Text, quantities, costs);
                LoadData();
                MessageBox.Show("Purchase order created.", "Purchasing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Purchasing", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ApproveOrder(object sender, EventArgs e)
        {
            try { _service.Approve(GetSelectedOrderId()); LoadData(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Purchasing", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void SendOrder(object sender, EventArgs e)
        {
            try { _service.SendToSupplier(GetSelectedOrderId()); LoadData(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Purchasing", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private int GetSelectedOrderId()
        {
            if (_orders.CurrentRow == null) throw new InvalidOperationException("Select a purchase order first.");
            return ((PurchaseOrder)_orders.CurrentRow.DataBoundItem).PurchaseOrderId;
        }
    }
}
