namespace SolarWorldERP.Forms
{
    partial class frmPurchasing
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
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.dgvOrderLines = new System.Windows.Forms.DataGridView();
            this.btnSubmitApproval = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnCancelOrder = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnFollowUp = new System.Windows.Forms.Button();
            this.grpNew = new System.Windows.Forms.GroupBox();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cboSupplier = new System.Windows.Forms.ComboBox();
            this.lblExpected = new System.Windows.Forms.Label();
            this.dtpExpected = new System.Windows.Forms.DateTimePicker();
            this.lblRf = new System.Windows.Forms.Label();
            this.cboRf = new System.Windows.Forms.ComboBox();
            this.btnLoadRf = new System.Windows.Forms.Button();
            this.lblNewSupplier = new System.Windows.Forms.Label();
            this.txtNewSupplier = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtSupplierAddress = new System.Windows.Forms.TextBox();
            this.btnAddSupplier = new System.Windows.Forms.Button();
            this.btnSaveAddress = new System.Windows.Forms.Button();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblItem = new System.Windows.Forms.Label();
            this.cboItem = new System.Windows.Forms.ComboBox();
            this.lblQty = new System.Windows.Forms.Label();
            this.numQty = new System.Windows.Forms.NumericUpDown();
            this.lblCost = new System.Windows.Forms.Label();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.dgvNewLines = new System.Windows.Forms.DataGridView();
            this.colNewItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRemoveLine = new System.Windows.Forms.Button();
            this.btnClearNew = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderLines)).BeginInit();
            this.grpNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewLines)).BeginInit();
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
            this.cboStatus.Items.AddRange(new object[] { "All", "Draft", "PendingApproval", "Ordered", "PartiallyReceived", "Completed", "Cancelled" });
            this.cboStatus.Location = new System.Drawing.Point(64, 12);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(160, 23);
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(234, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 27);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // dgvOrders
            //
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.AllowUserToDeleteRows = false;
            this.dgvOrders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.Location = new System.Drawing.Point(12, 44);
            this.dgvOrders.MultiSelect = false;
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.RowHeadersVisible = false;
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new System.Drawing.Size(960, 150);
            this.dgvOrders.SelectionChanged += new System.EventHandler(this.dgvOrders_SelectionChanged);
            //
            // dgvOrderLines
            //
            this.dgvOrderLines.AllowUserToAddRows = false;
            this.dgvOrderLines.AllowUserToDeleteRows = false;
            this.dgvOrderLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrderLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrderLines.Location = new System.Drawing.Point(12, 202);
            this.dgvOrderLines.Name = "dgvOrderLines";
            this.dgvOrderLines.ReadOnly = true;
            this.dgvOrderLines.RowHeadersVisible = false;
            this.dgvOrderLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderLines.Size = new System.Drawing.Size(640, 100);
            //
            // btnSubmitApproval
            //
            this.btnSubmitApproval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmitApproval.Location = new System.Drawing.Point(665, 202);
            this.btnSubmitApproval.Name = "btnSubmitApproval";
            this.btnSubmitApproval.Size = new System.Drawing.Size(145, 28);
            this.btnSubmitApproval.Text = "Submit for approval";
            this.btnSubmitApproval.UseVisualStyleBackColor = true;
            this.btnSubmitApproval.Click += new System.EventHandler(this.btnSubmitApproval_Click);
            //
            // btnApprove
            //
            this.btnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApprove.Location = new System.Drawing.Point(820, 202);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(152, 28);
            this.btnApprove.Text = "Approve and send (owner)";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            //
            // btnCancelOrder
            //
            this.btnCancelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelOrder.Location = new System.Drawing.Point(665, 236);
            this.btnCancelOrder.Name = "btnCancelOrder";
            this.btnCancelOrder.Size = new System.Drawing.Size(145, 28);
            this.btnCancelOrder.Text = "Cancel order";
            this.btnCancelOrder.UseVisualStyleBackColor = true;
            this.btnCancelOrder.Click += new System.EventHandler(this.btnCancelOrder_Click);
            //
            // btnReject
            //
            this.btnReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReject.Location = new System.Drawing.Point(820, 236);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(152, 28);
            this.btnReject.Text = "Reject (owner)";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            //
            // btnFollowUp
            //
            this.btnFollowUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFollowUp.Location = new System.Drawing.Point(665, 270);
            this.btnFollowUp.Name = "btnFollowUp";
            this.btnFollowUp.Size = new System.Drawing.Size(307, 28);
            this.btnFollowUp.Text = "Follow up supplier (new pickup for the balance)";
            this.btnFollowUp.UseVisualStyleBackColor = true;
            this.btnFollowUp.Click += new System.EventHandler(this.btnFollowUp_Click);
            //
            // grpNew
            //
            this.grpNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNew.Controls.Add(this.lblSupplier);
            this.grpNew.Controls.Add(this.cboSupplier);
            this.grpNew.Controls.Add(this.lblExpected);
            this.grpNew.Controls.Add(this.dtpExpected);
            this.grpNew.Controls.Add(this.lblRf);
            this.grpNew.Controls.Add(this.cboRf);
            this.grpNew.Controls.Add(this.btnLoadRf);
            this.grpNew.Controls.Add(this.lblNewSupplier);
            this.grpNew.Controls.Add(this.txtNewSupplier);
            this.grpNew.Controls.Add(this.lblAddress);
            this.grpNew.Controls.Add(this.txtSupplierAddress);
            this.grpNew.Controls.Add(this.btnAddSupplier);
            this.grpNew.Controls.Add(this.btnSaveAddress);
            this.grpNew.Controls.Add(this.lblRemarks);
            this.grpNew.Controls.Add(this.txtRemarks);
            this.grpNew.Controls.Add(this.lblItem);
            this.grpNew.Controls.Add(this.cboItem);
            this.grpNew.Controls.Add(this.lblQty);
            this.grpNew.Controls.Add(this.numQty);
            this.grpNew.Controls.Add(this.lblCost);
            this.grpNew.Controls.Add(this.txtCost);
            this.grpNew.Controls.Add(this.btnAddLine);
            this.grpNew.Controls.Add(this.dgvNewLines);
            this.grpNew.Controls.Add(this.btnRemoveLine);
            this.grpNew.Controls.Add(this.btnClearNew);
            this.grpNew.Controls.Add(this.btnCreate);
            this.grpNew.Location = new System.Drawing.Point(12, 312);
            this.grpNew.Name = "grpNew";
            this.grpNew.Size = new System.Drawing.Size(960, 345);
            this.grpNew.TabStop = false;
            this.grpNew.Text = "New purchase order";
            //
            // lblSupplier
            //
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(10, 29);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Text = "Supplier:";
            //
            // cboSupplier
            //
            this.cboSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSupplier.Location = new System.Drawing.Point(70, 25);
            this.cboSupplier.Name = "cboSupplier";
            this.cboSupplier.Size = new System.Drawing.Size(200, 23);
            this.cboSupplier.SelectedIndexChanged += new System.EventHandler(this.cboSupplier_SelectedIndexChanged);
            //
            // lblExpected
            //
            this.lblExpected.AutoSize = true;
            this.lblExpected.Location = new System.Drawing.Point(285, 29);
            this.lblExpected.Name = "lblExpected";
            this.lblExpected.Text = "Expected:";
            //
            // dtpExpected
            //
            this.dtpExpected.Checked = false;
            this.dtpExpected.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpected.Location = new System.Drawing.Point(350, 25);
            this.dtpExpected.Name = "dtpExpected";
            this.dtpExpected.ShowCheckBox = true;
            this.dtpExpected.Size = new System.Drawing.Size(125, 23);
            //
            // lblRf
            //
            this.lblRf.AutoSize = true;
            this.lblRf.Location = new System.Drawing.Point(495, 29);
            this.lblRf.Name = "lblRf";
            this.lblRf.Text = "Short requisition:";
            //
            // cboRf
            //
            this.cboRf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRf.Location = new System.Drawing.Point(600, 25);
            this.cboRf.Name = "cboRf";
            this.cboRf.Size = new System.Drawing.Size(210, 23);
            //
            // btnLoadRf
            //
            this.btnLoadRf.Location = new System.Drawing.Point(820, 23);
            this.btnLoadRf.Name = "btnLoadRf";
            this.btnLoadRf.Size = new System.Drawing.Size(130, 27);
            this.btnLoadRf.Text = "Load shortage";
            this.btnLoadRf.UseVisualStyleBackColor = true;
            this.btnLoadRf.Click += new System.EventHandler(this.btnLoadRf_Click);
            //
            // lblNewSupplier
            //
            this.lblNewSupplier.AutoSize = true;
            this.lblNewSupplier.Location = new System.Drawing.Point(10, 63);
            this.lblNewSupplier.Name = "lblNewSupplier";
            this.lblNewSupplier.Text = "New supplier:";
            //
            // txtNewSupplier
            //
            this.txtNewSupplier.Location = new System.Drawing.Point(95, 59);
            this.txtNewSupplier.MaxLength = 150;
            this.txtNewSupplier.Name = "txtNewSupplier";
            this.txtNewSupplier.Size = new System.Drawing.Size(160, 23);
            //
            // lblAddress
            //
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(268, 63);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Text = "Pickup address:";
            //
            // txtSupplierAddress
            //
            this.txtSupplierAddress.Location = new System.Drawing.Point(365, 59);
            this.txtSupplierAddress.MaxLength = 255;
            this.txtSupplierAddress.Name = "txtSupplierAddress";
            this.txtSupplierAddress.Size = new System.Drawing.Size(290, 23);
            //
            // btnAddSupplier
            //
            this.btnAddSupplier.Location = new System.Drawing.Point(665, 57);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(120, 27);
            this.btnAddSupplier.Text = "Add supplier";
            this.btnAddSupplier.UseVisualStyleBackColor = true;
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);
            //
            // btnSaveAddress
            //
            this.btnSaveAddress.Location = new System.Drawing.Point(795, 57);
            this.btnSaveAddress.Name = "btnSaveAddress";
            this.btnSaveAddress.Size = new System.Drawing.Size(155, 27);
            this.btnSaveAddress.Text = "Save address of supplier";
            this.btnSaveAddress.UseVisualStyleBackColor = true;
            this.btnSaveAddress.Click += new System.EventHandler(this.btnSaveAddress_Click);
            //
            // lblRemarks
            //
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(10, 97);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Text = "Remarks:";
            //
            // txtRemarks
            //
            this.txtRemarks.Location = new System.Drawing.Point(70, 93);
            this.txtRemarks.MaxLength = 255;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(880, 23);
            //
            // lblItem
            //
            this.lblItem.AutoSize = true;
            this.lblItem.Location = new System.Drawing.Point(10, 131);
            this.lblItem.Name = "lblItem";
            this.lblItem.Text = "Item:";
            //
            // cboItem
            //
            this.cboItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboItem.Location = new System.Drawing.Point(50, 127);
            this.cboItem.Name = "cboItem";
            this.cboItem.Size = new System.Drawing.Size(300, 23);
            //
            // lblQty
            //
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new System.Drawing.Point(365, 131);
            this.lblQty.Name = "lblQty";
            this.lblQty.Text = "Qty:";
            //
            // numQty
            //
            this.numQty.Location = new System.Drawing.Point(400, 127);
            this.numQty.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(70, 23);
            this.numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblCost
            //
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(485, 131);
            this.lblCost.Name = "lblCost";
            this.lblCost.Text = "Unit cost:";
            //
            // txtCost
            //
            this.txtCost.Location = new System.Drawing.Point(550, 127);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(80, 23);
            //
            // btnAddLine
            //
            this.btnAddLine.Location = new System.Drawing.Point(645, 125);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(130, 27);
            this.btnAddLine.Text = "Add to order";
            this.btnAddLine.UseVisualStyleBackColor = true;
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            //
            // dgvNewLines
            //
            this.dgvNewLines.AllowUserToAddRows = false;
            this.dgvNewLines.AllowUserToDeleteRows = false;
            this.dgvNewLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvNewLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNewLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colNewItemId, this.colNewItem, this.colNewQty, this.colNewCost });
            this.dgvNewLines.Location = new System.Drawing.Point(10, 162);
            this.dgvNewLines.MultiSelect = false;
            this.dgvNewLines.Name = "dgvNewLines";
            this.dgvNewLines.RowHeadersVisible = false;
            this.dgvNewLines.Size = new System.Drawing.Size(790, 173);
            //
            // columns of dgvNewLines (quantity and unit cost can be edited in the grid)
            //
            this.colNewItemId.HeaderText = "ItemId";
            this.colNewItemId.Name = "colNewItemId";
            this.colNewItemId.ReadOnly = true;
            this.colNewItemId.Visible = false;
            this.colNewItem.FillWeight = 250F;
            this.colNewItem.HeaderText = "Item";
            this.colNewItem.Name = "colNewItem";
            this.colNewItem.ReadOnly = true;
            this.colNewQty.HeaderText = "Qty";
            this.colNewQty.Name = "colNewQty";
            this.colNewCost.HeaderText = "Unit cost";
            this.colNewCost.Name = "colNewCost";
            //
            // btnRemoveLine
            //
            this.btnRemoveLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveLine.Location = new System.Drawing.Point(810, 162);
            this.btnRemoveLine.Name = "btnRemoveLine";
            this.btnRemoveLine.Size = new System.Drawing.Size(140, 30);
            this.btnRemoveLine.Text = "Remove line";
            this.btnRemoveLine.UseVisualStyleBackColor = true;
            this.btnRemoveLine.Click += new System.EventHandler(this.btnRemoveLine_Click);
            //
            // btnClearNew
            //
            this.btnClearNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearNew.Location = new System.Drawing.Point(810, 198);
            this.btnClearNew.Name = "btnClearNew";
            this.btnClearNew.Size = new System.Drawing.Size(140, 30);
            this.btnClearNew.Text = "Clear order";
            this.btnClearNew.UseVisualStyleBackColor = true;
            this.btnClearNew.Click += new System.EventHandler(this.btnClearNew_Click);
            //
            // btnCreate
            //
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(810, 300);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(140, 35);
            this.btnCreate.Text = "Create draft order";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            //
            // frmPurchasing
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 669);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(this.dgvOrderLines);
            this.Controls.Add(this.btnSubmitApproval);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.btnCancelOrder);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnFollowUp);
            this.Controls.Add(this.grpNew);
            this.MinimumSize = new System.Drawing.Size(1000, 708);
            this.Name = "frmPurchasing";
            this.Text = "Purchasing";
            this.Load += new System.EventHandler(this.frmPurchasing_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderLines)).EndInit();
            this.grpNew.ResumeLayout(false);
            this.grpNew.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.DataGridView dgvOrderLines;
        private System.Windows.Forms.Button btnSubmitApproval;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnCancelOrder;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnFollowUp;
        private System.Windows.Forms.GroupBox grpNew;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox cboSupplier;
        private System.Windows.Forms.Label lblExpected;
        private System.Windows.Forms.DateTimePicker dtpExpected;
        private System.Windows.Forms.Label lblRf;
        private System.Windows.Forms.ComboBox cboRf;
        private System.Windows.Forms.Button btnLoadRf;
        private System.Windows.Forms.Label lblNewSupplier;
        private System.Windows.Forms.TextBox txtNewSupplier;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtSupplierAddress;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.Button btnSaveAddress;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.ComboBox cboItem;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.DataGridView dgvNewLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewCost;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Button btnClearNew;
        private System.Windows.Forms.Button btnCreate;
    }
}