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
            this.btnPlace = new System.Windows.Forms.Button();
            this.btnCancelOrder = new System.Windows.Forms.Button();
            this.grpNew = new System.Windows.Forms.GroupBox();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cboSupplier = new System.Windows.Forms.ComboBox();
            this.txtNewSupplier = new System.Windows.Forms.TextBox();
            this.btnAddSupplier = new System.Windows.Forms.Button();
            this.lblExpected = new System.Windows.Forms.Label();
            this.dtpExpected = new System.Windows.Forms.DateTimePicker();
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
            this.cboStatus.Items.AddRange(new object[] { "All", "Draft", "Ordered", "PartiallyReceived", "Completed", "Cancelled" });
            this.cboStatus.Location = new System.Drawing.Point(64, 12);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(150, 23);
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            //
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(224, 10);
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
            this.dgvOrderLines.Size = new System.Drawing.Size(790, 100);
            //
            // btnPlace
            //
            this.btnPlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlace.Location = new System.Drawing.Point(822, 202);
            this.btnPlace.Name = "btnPlace";
            this.btnPlace.Size = new System.Drawing.Size(150, 30);
            this.btnPlace.Text = "Place order";
            this.btnPlace.UseVisualStyleBackColor = true;
            this.btnPlace.Click += new System.EventHandler(this.btnPlace_Click);
            //
            // btnCancelOrder
            //
            this.btnCancelOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelOrder.Location = new System.Drawing.Point(822, 240);
            this.btnCancelOrder.Name = "btnCancelOrder";
            this.btnCancelOrder.Size = new System.Drawing.Size(150, 30);
            this.btnCancelOrder.Text = "Cancel order";
            this.btnCancelOrder.UseVisualStyleBackColor = true;
            this.btnCancelOrder.Click += new System.EventHandler(this.btnCancelOrder_Click);
            //
            // grpNew
            //
            this.grpNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNew.Controls.Add(this.lblSupplier);
            this.grpNew.Controls.Add(this.cboSupplier);
            this.grpNew.Controls.Add(this.txtNewSupplier);
            this.grpNew.Controls.Add(this.btnAddSupplier);
            this.grpNew.Controls.Add(this.lblExpected);
            this.grpNew.Controls.Add(this.dtpExpected);
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
            this.grpNew.Controls.Add(this.btnCreate);
            this.grpNew.Location = new System.Drawing.Point(12, 312);
            this.grpNew.Name = "grpNew";
            this.grpNew.Size = new System.Drawing.Size(960, 300);
            this.grpNew.TabStop = false;
            this.grpNew.Text = "New purchase order";
            //
            // lblSupplier
            //
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(10, 28);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Text = "Supplier:";
            //
            // cboSupplier
            //
            this.cboSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSupplier.Location = new System.Drawing.Point(70, 24);
            this.cboSupplier.Name = "cboSupplier";
            this.cboSupplier.Size = new System.Drawing.Size(200, 23);
            //
            // txtNewSupplier
            //
            this.txtNewSupplier.Location = new System.Drawing.Point(285, 24);
            this.txtNewSupplier.MaxLength = 150;
            this.txtNewSupplier.Name = "txtNewSupplier";
            this.txtNewSupplier.Size = new System.Drawing.Size(170, 23);
            //
            // btnAddSupplier
            //
            this.btnAddSupplier.Location = new System.Drawing.Point(465, 22);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(110, 27);
            this.btnAddSupplier.Text = "Add supplier";
            this.btnAddSupplier.UseVisualStyleBackColor = true;
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);
            //
            // lblExpected
            //
            this.lblExpected.AutoSize = true;
            this.lblExpected.Location = new System.Drawing.Point(600, 28);
            this.lblExpected.Name = "lblExpected";
            this.lblExpected.Text = "Expected:";
            //
            // dtpExpected
            //
            this.dtpExpected.Checked = false;
            this.dtpExpected.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpected.Location = new System.Drawing.Point(665, 24);
            this.dtpExpected.Name = "dtpExpected";
            this.dtpExpected.ShowCheckBox = true;
            this.dtpExpected.Size = new System.Drawing.Size(130, 23);
            //
            // lblRemarks
            //
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(10, 62);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Text = "Remarks:";
            //
            // txtRemarks
            //
            this.txtRemarks.Location = new System.Drawing.Point(70, 58);
            this.txtRemarks.MaxLength = 255;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(505, 23);
            //
            // lblItem
            //
            this.lblItem.AutoSize = true;
            this.lblItem.Location = new System.Drawing.Point(10, 98);
            this.lblItem.Name = "lblItem";
            this.lblItem.Text = "Item:";
            //
            // cboItem
            //
            this.cboItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboItem.Location = new System.Drawing.Point(70, 94);
            this.cboItem.Name = "cboItem";
            this.cboItem.Size = new System.Drawing.Size(300, 23);
            //
            // lblQty
            //
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new System.Drawing.Point(385, 98);
            this.lblQty.Name = "lblQty";
            this.lblQty.Text = "Qty:";
            //
            // numQty
            //
            this.numQty.Location = new System.Drawing.Point(420, 94);
            this.numQty.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numQty.Name = "numQty";
            this.numQty.Size = new System.Drawing.Size(70, 23);
            this.numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblCost
            //
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(505, 98);
            this.lblCost.Name = "lblCost";
            this.lblCost.Text = "Unit cost:";
            //
            // txtCost
            //
            this.txtCost.Location = new System.Drawing.Point(570, 94);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(80, 23);
            //
            // btnAddLine
            //
            this.btnAddLine.Location = new System.Drawing.Point(665, 92);
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
            this.dgvNewLines.Location = new System.Drawing.Point(10, 130);
            this.dgvNewLines.MultiSelect = false;
            this.dgvNewLines.Name = "dgvNewLines";
            this.dgvNewLines.ReadOnly = true;
            this.dgvNewLines.RowHeadersVisible = false;
            this.dgvNewLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNewLines.Size = new System.Drawing.Size(790, 160);
            //
            // columns of dgvNewLines
            //
            this.colNewItemId.HeaderText = "ItemId";
            this.colNewItemId.Name = "colNewItemId";
            this.colNewItemId.Visible = false;
            this.colNewItem.FillWeight = 250F;
            this.colNewItem.HeaderText = "Item";
            this.colNewItem.Name = "colNewItem";
            this.colNewQty.HeaderText = "Qty";
            this.colNewQty.Name = "colNewQty";
            this.colNewCost.HeaderText = "Unit cost";
            this.colNewCost.Name = "colNewCost";
            //
            // btnRemoveLine
            //
            this.btnRemoveLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveLine.Location = new System.Drawing.Point(810, 130);
            this.btnRemoveLine.Name = "btnRemoveLine";
            this.btnRemoveLine.Size = new System.Drawing.Size(140, 30);
            this.btnRemoveLine.Text = "Remove line";
            this.btnRemoveLine.UseVisualStyleBackColor = true;
            this.btnRemoveLine.Click += new System.EventHandler(this.btnRemoveLine_Click);
            //
            // btnCreate
            //
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(810, 255);
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
            this.ClientSize = new System.Drawing.Size(984, 625);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(this.dgvOrderLines);
            this.Controls.Add(this.btnPlace);
            this.Controls.Add(this.btnCancelOrder);
            this.Controls.Add(this.grpNew);
            this.MinimumSize = new System.Drawing.Size(1000, 664);
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
        private System.Windows.Forms.Button btnPlace;
        private System.Windows.Forms.Button btnCancelOrder;
        private System.Windows.Forms.GroupBox grpNew;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox cboSupplier;
        private System.Windows.Forms.TextBox txtNewSupplier;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.Label lblExpected;
        private System.Windows.Forms.DateTimePicker dtpExpected;
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
        private System.Windows.Forms.Button btnCreate;
    }
}