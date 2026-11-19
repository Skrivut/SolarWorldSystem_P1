namespace SolarWorldERP.Forms
{
    partial class frmDelivery
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblStatus = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvDeliveries = new System.Windows.Forms.DataGridView();
            this.grpSchedule = new System.Windows.Forms.GroupBox();
            this.lblOrder = new System.Windows.Forms.Label();
            this.cboOrder = new System.Windows.Forms.ComboBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblExpected = new System.Windows.Forms.Label();
            this.dtpExpected = new System.Windows.Forms.DateTimePicker();
            this.lblDriver = new System.Windows.Forms.Label();
            this.txtDriver = new System.Windows.Forms.TextBox();
            this.btnSchedule = new System.Windows.Forms.Button();
            this.grpSelected = new System.Windows.Forms.GroupBox();
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.colLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdered = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReceiveNow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnInTransit = new System.Windows.Forms.Button();
            this.btnReceive = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveries)).BeginInit();
            this.grpSchedule.SuspendLayout();
            this.grpSelected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).BeginInit();
            this.SuspendLayout();
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "Status:";
            //
            // cboStatus
            //
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Items.AddRange(new object[] { "All", "Scheduled", "InTransit", "Delivered", "Cancelled" });
            this.cboStatus.Location = new System.Drawing.Point(64, 12);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(130, 23);
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(204, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 27);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // dgvDeliveries
            //
            this.dgvDeliveries.AllowUserToAddRows = false;
            this.dgvDeliveries.AllowUserToDeleteRows = false;
            this.dgvDeliveries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDeliveries.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDeliveries.Location = new System.Drawing.Point(12, 44);
            this.dgvDeliveries.MultiSelect = false;
            this.dgvDeliveries.Name = "dgvDeliveries";
            this.dgvDeliveries.ReadOnly = true;
            this.dgvDeliveries.RowHeadersVisible = false;
            this.dgvDeliveries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeliveries.Size = new System.Drawing.Size(960, 190);
            this.dgvDeliveries.SelectionChanged += new System.EventHandler(this.dgvDeliveries_SelectionChanged);
            //
            // grpSchedule
            //
            this.grpSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSchedule.Controls.Add(this.lblOrder);
            this.grpSchedule.Controls.Add(this.cboOrder);
            this.grpSchedule.Controls.Add(this.lblAddress);
            this.grpSchedule.Controls.Add(this.txtAddress);
            this.grpSchedule.Controls.Add(this.lblExpected);
            this.grpSchedule.Controls.Add(this.dtpExpected);
            this.grpSchedule.Controls.Add(this.lblDriver);
            this.grpSchedule.Controls.Add(this.txtDriver);
            this.grpSchedule.Controls.Add(this.btnSchedule);
            this.grpSchedule.Location = new System.Drawing.Point(12, 244);
            this.grpSchedule.Name = "grpSchedule";
            this.grpSchedule.Size = new System.Drawing.Size(960, 85);
            this.grpSchedule.TabStop = false;
            this.grpSchedule.Text = "Schedule a delivery";
            //
            // lblOrder
            //
            this.lblOrder.AutoSize = true;
            this.lblOrder.Location = new System.Drawing.Point(10, 25);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Text = "Purchase order:";
            //
            // cboOrder
            //
            this.cboOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrder.Location = new System.Drawing.Point(105, 21);
            this.cboOrder.Name = "cboOrder";
            this.cboOrder.Size = new System.Drawing.Size(140, 23);
            //
            // lblAddress
            //
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(265, 25);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Text = "Address:";
            //
            // txtAddress
            //
            this.txtAddress.Location = new System.Drawing.Point(320, 21);
            this.txtAddress.MaxLength = 255;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(280, 23);
            //
            // lblExpected
            //
            this.lblExpected.AutoSize = true;
            this.lblExpected.Location = new System.Drawing.Point(620, 25);
            this.lblExpected.Name = "lblExpected";
            this.lblExpected.Text = "Expected:";
            //
            // dtpExpected
            //
            this.dtpExpected.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpected.Location = new System.Drawing.Point(685, 21);
            this.dtpExpected.Name = "dtpExpected";
            this.dtpExpected.Size = new System.Drawing.Size(110, 23);
            //
            // lblDriver
            //
            this.lblDriver.AutoSize = true;
            this.lblDriver.Location = new System.Drawing.Point(10, 55);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Text = "Driver:";
            //
            // txtDriver
            //
            this.txtDriver.Location = new System.Drawing.Point(105, 51);
            this.txtDriver.MaxLength = 100;
            this.txtDriver.Name = "txtDriver";
            this.txtDriver.Size = new System.Drawing.Size(140, 23);
            //
            // btnSchedule
            //
            this.btnSchedule.Location = new System.Drawing.Point(265, 49);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(110, 27);
            this.btnSchedule.Text = "Schedule";
            this.btnSchedule.UseVisualStyleBackColor = true;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            //
            // grpSelected
            //
            this.grpSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSelected.Controls.Add(this.dgvLines);
            this.grpSelected.Controls.Add(this.lblNotes);
            this.grpSelected.Controls.Add(this.txtNotes);
            this.grpSelected.Controls.Add(this.btnInTransit);
            this.grpSelected.Controls.Add(this.btnReceive);
            this.grpSelected.Controls.Add(this.btnCancel);
            this.grpSelected.Location = new System.Drawing.Point(12, 335);
            this.grpSelected.Name = "grpSelected";
            this.grpSelected.Size = new System.Drawing.Size(960, 255);
            this.grpSelected.TabStop = false;
            this.grpSelected.Text = "Selected delivery: type the quantity received for each item, then press Receive items";
            //
            // dgvLines
            //
            this.dgvLines.AllowUserToAddRows = false;
            this.dgvLines.AllowUserToDeleteRows = false;
            this.dgvLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colLineId, this.colItem, this.colOrdered, this.colReceived, this.colRemaining, this.colReceiveNow });
            this.dgvLines.Location = new System.Drawing.Point(10, 22);
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.RowHeadersVisible = false;
            this.dgvLines.Size = new System.Drawing.Size(940, 135);
            //
            // columns of dgvLines
            //
            this.colLineId.HeaderText = "Line";
            this.colLineId.Name = "colLineId";
            this.colLineId.ReadOnly = true;
            this.colLineId.Visible = false;
            this.colItem.HeaderText = "Item";
            this.colItem.Name = "colItem";
            this.colItem.ReadOnly = true;
            this.colItem.FillWeight = 250F;
            this.colOrdered.HeaderText = "Ordered";
            this.colOrdered.Name = "colOrdered";
            this.colOrdered.ReadOnly = true;
            this.colReceived.HeaderText = "Already received";
            this.colReceived.Name = "colReceived";
            this.colReceived.ReadOnly = true;
            this.colRemaining.HeaderText = "Remaining";
            this.colRemaining.Name = "colRemaining";
            this.colRemaining.ReadOnly = true;
            this.colReceiveNow.HeaderText = "Receive now";
            this.colReceiveNow.Name = "colReceiveNow";
            //
            // lblNotes
            //
            this.lblNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(10, 170);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Text = "Notes:";
            //
            // txtNotes
            //
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(60, 166);
            this.txtNotes.MaxLength = 255;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(890, 23);
            //
            // btnInTransit
            //
            this.btnInTransit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInTransit.Location = new System.Drawing.Point(10, 205);
            this.btnInTransit.Name = "btnInTransit";
            this.btnInTransit.Size = new System.Drawing.Size(120, 30);
            this.btnInTransit.Text = "Mark in transit";
            this.btnInTransit.UseVisualStyleBackColor = true;
            this.btnInTransit.Click += new System.EventHandler(this.btnInTransit_Click);
            //
            // btnReceive
            //
            this.btnReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReceive.Location = new System.Drawing.Point(140, 205);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(120, 30);
            this.btnReceive.Text = "Receive items";
            this.btnReceive.UseVisualStyleBackColor = true;
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(270, 205);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 30);
            this.btnCancel.Text = "Cancel delivery";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // frmDelivery
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 601);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvDeliveries);
            this.Controls.Add(this.grpSchedule);
            this.Controls.Add(this.grpSelected);
            this.MinimumSize = new System.Drawing.Size(1000, 640);
            this.Name = "frmDelivery";
            this.Text = "Deliveries";
            this.Load += new System.EventHandler(this.frmDelivery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveries)).EndInit();
            this.grpSchedule.ResumeLayout(false);
            this.grpSchedule.PerformLayout();
            this.grpSelected.ResumeLayout(false);
            this.grpSelected.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvDeliveries;
        private System.Windows.Forms.GroupBox grpSchedule;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.ComboBox cboOrder;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblExpected;
        private System.Windows.Forms.DateTimePicker dtpExpected;
        private System.Windows.Forms.Label lblDriver;
        private System.Windows.Forms.TextBox txtDriver;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.GroupBox grpSelected;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdered;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReceiveNow;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnInTransit;
        private System.Windows.Forms.Button btnReceive;
        private System.Windows.Forms.Button btnCancel;
    }
}