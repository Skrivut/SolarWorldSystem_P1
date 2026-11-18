using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SolarWorldERP.Data;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    public class frmDelivery : Form
    {
        private readonly DeliveryService _service = new DeliveryService();
        private readonly DataGridView _deliveries = new DataGridView();
        private readonly TextBox _address = new TextBox();
        private readonly TextBox _driver = new TextBox();
        private readonly DateTimePicker _expected = new DateTimePicker();
        private readonly Button _schedule = new Button();
        private readonly Button _transit = new Button();
        private readonly Button _receive = new Button();

        public frmDelivery()
        {
            Text = "Delivery";
            Width = 900;
            Height = 500;
            StartPosition = FormStartPosition.CenterParent;
            BuildControls();
            LoadData();
        }

        private void BuildControls()
        {
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 70, Padding = new Padding(6) };
            top.Controls.Add(new Label { Text = "PO ID:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
            var poId = new NumericUpDown { Name = "poId", Minimum = 1, Maximum = 1000000, Width = 70 };
            top.Controls.Add(poId);
            top.Controls.Add(new Label { Text = "Address:", AutoSize = true, Padding = new Padding(8, 6, 0, 0) });
            _address.Width = 150;
            top.Controls.Add(_address);
            top.Controls.Add(new Label { Text = "Driver:", AutoSize = true, Padding = new Padding(8, 6, 0, 0) });
            _driver.Width = 110;
            top.Controls.Add(_driver);
            _expected.Format = DateTimePickerFormat.Short;
            top.Controls.Add(_expected);
            _schedule.Text = "Schedule";
            _schedule.Click += delegate { Schedule((int)poId.Value); };
            top.Controls.Add(_schedule);
            _transit.Text = "Mark In Transit";
            _transit.Click += MarkInTransit;
            top.Controls.Add(_transit);
            _receive.Text = "Receive Selected";
            _receive.Click += Receive;
            top.Controls.Add(_receive);
            Controls.Add(top);

            _deliveries.Dock = DockStyle.Fill;
            _deliveries.ReadOnly = true;
            _deliveries.AllowUserToAddRows = false;
            _deliveries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _deliveries.AutoGenerateColumns = true;
            Controls.Add(_deliveries);
        }

        private void LoadData()
        {
            try { _deliveries.DataSource = _service.GetDeliveries(null); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Delivery", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Schedule(int purchaseOrderId)
        {
            try { _service.Schedule(purchaseOrderId, _address.Text, _expected.Value, _driver.Text); LoadData(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Delivery", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void MarkInTransit(object sender, EventArgs e)
        {
            try { _service.MarkInTransit(GetSelectedDeliveryId()); LoadData(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Delivery", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void Receive(object sender, EventArgs e)
        {
            try
            {
                var delivery = GetSelectedDeliveryId();
                var message = "Enter the delivery line ID and quantity in code or a future detail screen.";
                MessageBox.Show(message, "Delivery", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Delivery", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private int GetSelectedDeliveryId()
        {
            if (_deliveries.CurrentRow == null) throw new InvalidOperationException("Select a delivery first.");
            return ((Delivery)_deliveries.CurrentRow.DataBoundItem).DeliveryId;
        }
    }
}
