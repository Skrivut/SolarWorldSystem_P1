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
            this.grpSelected = new System.Windows.Forms.GroupBox();
            this.lblDr = new System.Windows.Forms.Label();
            this.txtDr = new System.Windows.Forms.TextBox();
            this.btnPickedUp = new System.Windows.Forms.Button();
            this.btnDelivered = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.colLineId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdered = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccepted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRejected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnReport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveries)).BeginInit();
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
            this.cboStatus.Items.AddRange(new object[] { "All", "Pending", "PickedUp", "Delivered", "Received", "Cancelled" });
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
            this.dgvDeliveries.Size = new System.Drawing.Size(960, 170);
            this.dgvDeliveries.SelectionChanged += new System.EventHandler(this.dgvDeliveries_SelectionChanged);
            //
            // grpSelected
            //
            this.grpSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSelected.Controls.Add(this.lblDr);
            this.grpSelected.Controls.Add(this.txtDr);
            this.grpSelected.Controls.Add(this.btnPickedUp);
            this.grpSelected.Controls.Add(this.btnDelivered);
            this.grpSelected.Controls.Add(this.btnCancel);
            this.grpSelected.Controls.Add(this.dgvLines);
            this.grpSelected.Controls.Add(this.lblNotes);
            this.grpSelected.Controls.Add(this.txtNotes);
            this.grpSelected.Controls.Add(this.btnReport);
            this.grpSelected.Location = new System.Drawing.Point(12, 224);
            this.grpSelected.Name = "grpSelected";
            this.grpSelected.Size = new System.Drawing.Size(960, 360);
            this.grpSelected.TabStop = false;
            this.grpSelected.Text = "Selected pickup: the driver collects and delivers; staff then check the goods and complete the receiving report";
            //
            // lblDr
            //
            this.lblDr.AutoSize = true;
            this.lblDr.Location = new System.Drawing.Point(10, 28);
            this.lblDr.Name = "lblDr";
            this.lblDr.Text = "Supplier DR no.:";
            //
            // txtDr
            //
            this.txtDr.Location = new System.Drawing.Point(115, 24);
            this.txtDr.MaxLength = 50;
            this.txtDr.Name = "txtDr";
            this.txtDr.Size = new System.Drawing.Size(150, 23);
            //
            // btnPickedUp
            //
            this.btnPickedUp.Location = new System.Drawing.Point(280, 22);
            this.btnPickedUp.Name = "btnPickedUp";
            this.btnPickedUp.Size = new System.Drawing.Size(140, 27);
            this.btnPickedUp.Text = "Driver picked up";
            this.btnPickedUp.UseVisualStyleBackColor = true;
            this.btnPickedUp.Click += new System.EventHandler(this.btnPickedUp_Click);
            //
            // btnDelivered
            //
            this.btnDelivered.Location = new System.Drawing.Point(430, 22);
            this.btnDelivered.Name = "btnDelivered";
            this.btnDelivered.Size = new System.Drawing.Size(150, 27);
            this.btnDelivered.Text = "Delivered to office";
            this.btnDelivered.UseVisualStyleBackColor = true;
            this.btnDelivered.Click += new System.EventHandler(this.btnDelivered_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(590, 22);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 27);
            this.btnCancel.Text = "Cancel pickup";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // dgvLines
            //
            this.dgvLines.AllowUserToAddRows = false;
            this.dgvLines.AllowUserToDeleteRows = false;
            this.dgvLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colLineId, this.colItem, this.colOrdered, this.colReceived, this.colRemaining,
                this.colAccepted, this.colRejected, this.colReason, this.colAction });
            this.dgvLines.Location = new System.Drawing.Point(10, 60);
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.RowHeadersVisible = false;
            this.dgvLines.Size = new System.Drawing.Size(940, 215);
            //
            // columns of dgvLines
            //
            this.colLineId.HeaderText = "Line";
            this.colLineId.Name = "colLineId";
            this.colLineId.ReadOnly = true;
            this.colLineId.Visible = false;
            this.colItem.FillWeight = 220F;
            this.colItem.HeaderText = "Item";
            this.colItem.Name = "colItem";
            this.colItem.ReadOnly = true;
            this.colOrdered.HeaderText = "Ordered";
            this.colOrdered.Name = "colOrdered";
            this.colOrdered.ReadOnly = true;
            this.colReceived.HeaderText = "Accepted so far";
            this.colReceived.Name = "colReceived";
            this.colReceived.ReadOnly = true;
            this.colRemaining.HeaderText = "Outstanding";
            this.colRemaining.Name = "colRemaining";
            this.colRemaining.ReadOnly = true;
            this.colAccepted.HeaderText = "Accepted now";
            this.colAccepted.Name = "colAccepted";
            this.colRejected.HeaderText = "Rejected now";
            this.colRejected.Name = "colRejected";
            this.colReason.FillWeight = 180F;
            this.colReason.HeaderText = "Reason (rejected, damaged, wrong item)";
            this.colReason.Name = "colReason";
            this.colAction.FillWeight = 110F;
            this.colAction.HeaderText = "Request";
            this.colAction.Items.AddRange(new object[] { "Return", "Replacement" });
            this.colAction.Name = "colAction";
            //
            // lblNotes
            //
            this.lblNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(10, 292);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Text = "Notes:";
            //
            // txtNotes
            //
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(60, 288);
            this.txtNotes.MaxLength = 255;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(890, 23);
            //
            // btnReport
            //
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReport.Location = new System.Drawing.Point(10, 320);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(230, 32);
            this.btnReport.Text = "Submit receiving report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            //
            // frmDelivery
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 596);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvDeliveries);
            this.Controls.Add(this.grpSelected);
            this.MinimumSize = new System.Drawing.Size(1000, 635);
            this.Name = "frmDelivery";
            this.Text = "Deliveries";
            this.Load += new System.EventHandler(this.frmDelivery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveries)).EndInit();
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
        private System.Windows.Forms.GroupBox grpSelected;
        private System.Windows.Forms.Label lblDr;
        private System.Windows.Forms.TextBox txtDr;
        private System.Windows.Forms.Button btnPickedUp;
        private System.Windows.Forms.Button btnDelivered;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdered;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccepted;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRejected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReason;
        private System.Windows.Forms.DataGridViewComboBoxColumn colAction;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnReport;
    }
}