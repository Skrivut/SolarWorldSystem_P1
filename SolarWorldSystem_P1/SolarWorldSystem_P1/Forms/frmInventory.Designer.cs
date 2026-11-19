namespace SolarWorldERP.Forms
{
    partial class frmInventory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTagFilter = new System.Windows.Forms.Label();
            this.cboStockTag = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.chkLowStockOnly = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.gridInventory = new System.Windows.Forms.DataGridView();
            this.colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockTag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtyOnHand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtyReserved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtyAvailable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtyIncoming = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReorderPoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxStockLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsScarce = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIsLowStock = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lblSummary = new System.Windows.Forms.Label();
            this.lblIssueRf = new System.Windows.Forms.Label();
            this.cboIssuableRf = new System.Windows.Forms.ComboBox();
            this.btnIssueRf = new System.Windows.Forms.Button();
            this.grpEditor = new System.Windows.Forms.GroupBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblEditTag = new System.Windows.Forms.Label();
            this.cboEditTag = new System.Windows.Forms.ComboBox();
            this.lblInitialQty = new System.Windows.Forms.Label();
            this.numInitialQty = new System.Windows.Forms.NumericUpDown();
            this.lblReorder = new System.Windows.Forms.Label();
            this.numReorderPoint = new System.Windows.Forms.NumericUpDown();
            this.lblMaxStock = new System.Windows.Forms.Label();
            this.numMaxStock = new System.Windows.Forms.NumericUpDown();
            this.chkIsScarce = new System.Windows.Forms.CheckBox();
            this.btnNewItem = new System.Windows.Forms.Button();
            this.btnSaveItem = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridInventory)).BeginInit();
            this.grpEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInitialQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReorderPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxStock)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTagFilter
            // 
            this.lblTagFilter.AutoSize = true;
            this.lblTagFilter.Location = new System.Drawing.Point(16, 16);
            this.lblTagFilter.Name = "lblTagFilter";
            this.lblTagFilter.Size = new System.Drawing.Size(58, 13);
            this.lblTagFilter.TabIndex = 0;
            this.lblTagFilter.Text = "Stock Tag:";
            // 
            // cboStockTag
            // 
            this.cboStockTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStockTag.FormattingEnabled = true;
            this.cboStockTag.Location = new System.Drawing.Point(80, 12);
            this.cboStockTag.Name = "cboStockTag";
            this.cboStockTag.Size = new System.Drawing.Size(120, 21);
            this.cboStockTag.TabIndex = 1;
            this.cboStockTag.SelectedIndexChanged += new System.EventHandler(this.FilterChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(216, 16);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(44, 13);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(266, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 20);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.FilterChanged);
            // 
            // chkLowStockOnly
            // 
            this.chkLowStockOnly.AutoSize = true;
            this.chkLowStockOnly.Location = new System.Drawing.Point(482, 15);
            this.chkLowStockOnly.Name = "chkLowStockOnly";
            this.chkLowStockOnly.Size = new System.Drawing.Size(134, 17);
            this.chkLowStockOnly.TabIndex = 4;
            this.chkLowStockOnly.Text = "Low-Stock Alerts Only";
            this.chkLowStockOnly.UseVisualStyleBackColor = true;
            this.chkLowStockOnly.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(630, 11);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // gridInventory
            // 
            this.gridInventory.AllowUserToAddRows = false;
            this.gridInventory.AllowUserToDeleteRows = false;
            this.gridInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridInventory.AutoGenerateColumns = false;
            this.gridInventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridInventory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemId,
            this.colItemCode,
            this.colItemName,
            this.colUnit,
            this.colStockTag,
            this.colQtyOnHand,
            this.colQtyReserved,
            this.colQtyAvailable,
            this.colQtyIncoming,
            this.colReorderPoint,
            this.colMaxStockLevel,
            this.colIsScarce,
            this.colIsLowStock});
            this.gridInventory.Location = new System.Drawing.Point(16, 44);
            this.gridInventory.MultiSelect = false;
            this.gridInventory.Name = "gridInventory";
            this.gridInventory.ReadOnly = true;
            this.gridInventory.RowHeadersVisible = false;
            this.gridInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridInventory.Size = new System.Drawing.Size(868, 312);
            this.gridInventory.TabIndex = 6;
            this.gridInventory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridInventory_CellFormatting);
            this.gridInventory.SelectionChanged += new System.EventHandler(this.gridInventory_SelectionChanged);
            // 
            // colItemId
            // 
            this.colItemId.DataPropertyName = "ItemId";
            this.colItemId.HeaderText = "ID";
            this.colItemId.Name = "colItemId";
            this.colItemId.ReadOnly = true;
            this.colItemId.Visible = false;
            // 
            // colItemCode
            // 
            this.colItemCode.DataPropertyName = "ItemCode";
            this.colItemCode.FillWeight = 80F;
            this.colItemCode.HeaderText = "Code";
            this.colItemCode.Name = "colItemCode";
            this.colItemCode.ReadOnly = true;
            // 
            // colItemName
            // 
            this.colItemName.DataPropertyName = "ItemName";
            this.colItemName.FillWeight = 155F;
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // colUnit
            // 
            this.colUnit.DataPropertyName = "Unit";
            this.colUnit.FillWeight = 55F;
            this.colUnit.HeaderText = "Unit";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            // 
            // colStockTag
            // 
            this.colStockTag.DataPropertyName = "StockTag";
            this.colStockTag.FillWeight = 70F;
            this.colStockTag.HeaderText = "Tag";
            this.colStockTag.Name = "colStockTag";
            this.colStockTag.ReadOnly = true;
            // 
            // colQtyOnHand
            // 
            this.colQtyOnHand.DataPropertyName = "QtyOnHand";
            this.colQtyOnHand.FillWeight = 68F;
            this.colQtyOnHand.HeaderText = "On Hand";
            this.colQtyOnHand.Name = "colQtyOnHand";
            this.colQtyOnHand.ReadOnly = true;
            // 
            // colQtyReserved
            // 
            this.colQtyReserved.DataPropertyName = "QtyReserved";
            this.colQtyReserved.FillWeight = 68F;
            this.colQtyReserved.HeaderText = "Reserved";
            this.colQtyReserved.Name = "colQtyReserved";
            this.colQtyReserved.ReadOnly = true;
            // 
            // colQtyAvailable
            // 
            this.colQtyAvailable.DataPropertyName = "QtyAvailable";
            this.colQtyAvailable.FillWeight = 68F;
            this.colQtyAvailable.HeaderText = "Available";
            this.colQtyAvailable.Name = "colQtyAvailable";
            this.colQtyAvailable.ReadOnly = true;
            // 
            // colQtyIncoming
            // 
            this.colQtyIncoming.DataPropertyName = "QtyIncoming";
            this.colQtyIncoming.FillWeight = 68F;
            this.colQtyIncoming.HeaderText = "Incoming";
            this.colQtyIncoming.Name = "colQtyIncoming";
            this.colQtyIncoming.ReadOnly = true;
            // 
            // colReorderPoint
            // 
            this.colReorderPoint.DataPropertyName = "ReorderPoint";
            this.colReorderPoint.FillWeight = 68F;
            this.colReorderPoint.HeaderText = "Reorder Pt";
            this.colReorderPoint.Name = "colReorderPoint";
            this.colReorderPoint.ReadOnly = true;
            // 
            // colMaxStockLevel
            // 
            this.colMaxStockLevel.DataPropertyName = "MaxStockLevel";
            this.colMaxStockLevel.FillWeight = 68F;
            this.colMaxStockLevel.HeaderText = "Max Level";
            this.colMaxStockLevel.Name = "colMaxStockLevel";
            this.colMaxStockLevel.ReadOnly = true;
            // 
            // colIsScarce
            // 
            this.colIsScarce.DataPropertyName = "IsScarce";
            this.colIsScarce.FillWeight = 52F;
            this.colIsScarce.HeaderText = "Scarce";
            this.colIsScarce.Name = "colIsScarce";
            this.colIsScarce.ReadOnly = true;
            // 
            // colIsLowStock
            // 
            this.colIsLowStock.DataPropertyName = "IsLowStock";
            this.colIsLowStock.FillWeight = 62F;
            this.colIsLowStock.HeaderText = "Low Alert";
            this.colIsLowStock.Name = "colIsLowStock";
            this.colIsLowStock.ReadOnly = true;
            // 
            // lblSummary
            // 
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSummary.AutoSize = true;
            this.lblSummary.Location = new System.Drawing.Point(16, 366);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(75, 13);
            this.lblSummary.TabIndex = 7;
            this.lblSummary.Text = "Items loaded: 0";
            // 
            // lblIssueRf
            // 
            this.lblIssueRf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblIssueRf.AutoSize = true;
            this.lblIssueRf.Location = new System.Drawing.Point(430, 366);
            this.lblIssueRf.Name = "lblIssueRf";
            this.lblIssueRf.Size = new System.Drawing.Size(102, 13);
            this.lblIssueRf.TabIndex = 8;
            this.lblIssueRf.Text = "Issue Requisition RF:";
            // 
            // cboIssuableRf
            // 
            this.cboIssuableRf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboIssuableRf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIssuableRf.FormattingEnabled = true;
            this.cboIssuableRf.Location = new System.Drawing.Point(538, 362);
            this.cboIssuableRf.Name = "cboIssuableRf";
            this.cboIssuableRf.Size = new System.Drawing.Size(220, 21);
            this.cboIssuableRf.TabIndex = 9;
            // 
            // btnIssueRf
            // 
            this.btnIssueRf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIssueRf.Location = new System.Drawing.Point(764, 361);
            this.btnIssueRf.Name = "btnIssueRf";
            this.btnIssueRf.Size = new System.Drawing.Size(120, 23);
            this.btnIssueRf.TabIndex = 10;
            this.btnIssueRf.Text = "Issue Reserved Stock";
            this.btnIssueRf.UseVisualStyleBackColor = true;
            this.btnIssueRf.Click += new System.EventHandler(this.btnIssueRf_Click);
            // 
            // grpEditor
            // 
            this.grpEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpEditor.Controls.Add(this.btnSaveItem);
            this.grpEditor.Controls.Add(this.btnNewItem);
            this.grpEditor.Controls.Add(this.chkIsScarce);
            this.grpEditor.Controls.Add(this.numMaxStock);
            this.grpEditor.Controls.Add(this.lblMaxStock);
            this.grpEditor.Controls.Add(this.numReorderPoint);
            this.grpEditor.Controls.Add(this.lblReorder);
            this.grpEditor.Controls.Add(this.numInitialQty);
            this.grpEditor.Controls.Add(this.lblInitialQty);
            this.grpEditor.Controls.Add(this.cboEditTag);
            this.grpEditor.Controls.Add(this.lblEditTag);
            this.grpEditor.Controls.Add(this.txtUnit);
            this.grpEditor.Controls.Add(this.lblUnit);
            this.grpEditor.Controls.Add(this.txtItemName);
            this.grpEditor.Controls.Add(this.lblName);
            this.grpEditor.Controls.Add(this.txtItemCode);
            this.grpEditor.Controls.Add(this.lblCode);
            this.grpEditor.Controls.Add(this.lblMode);
            this.grpEditor.Location = new System.Drawing.Point(16, 392);
            this.grpEditor.Name = "grpEditor";
            this.grpEditor.Size = new System.Drawing.Size(868, 122);
            this.grpEditor.TabIndex = 11;
            this.grpEditor.TabStop = false;
            this.grpEditor.Text = "Catalog Item Details";
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(14, 22);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(112, 13);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "Mode: Editing Selected";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(14, 50);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(35, 13);
            this.lblCode.TabIndex = 1;
            this.lblCode.Text = "Code:";
            // 
            // txtItemCode
            // 
            this.txtItemCode.Location = new System.Drawing.Point(56, 47);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(100, 20);
            this.txtItemCode.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(170, 50);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(214, 47);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(240, 20);
            this.txtItemName.TabIndex = 4;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(468, 50);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(29, 13);
            this.lblUnit.TabIndex = 5;
            this.lblUnit.Text = "Unit:";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(503, 47);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(70, 20);
            this.txtUnit.TabIndex = 6;
            // 
            // lblEditTag
            // 
            this.lblEditTag.AutoSize = true;
            this.lblEditTag.Location = new System.Drawing.Point(588, 50);
            this.lblEditTag.Name = "lblEditTag";
            this.lblEditTag.Size = new System.Drawing.Size(29, 13);
            this.lblEditTag.TabIndex = 7;
            this.lblEditTag.Text = "Tag:";
            // 
            // cboEditTag
            // 
            this.cboEditTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEditTag.FormattingEnabled = true;
            this.cboEditTag.Location = new System.Drawing.Point(623, 46);
            this.cboEditTag.Name = "cboEditTag";
            this.cboEditTag.Size = new System.Drawing.Size(100, 21);
            this.cboEditTag.TabIndex = 8;
            // 
            // lblInitialQty
            // 
            this.lblInitialQty.AutoSize = true;
            this.lblInitialQty.Location = new System.Drawing.Point(14, 84);
            this.lblInitialQty.Name = "lblInitialQty";
            this.lblInitialQty.Size = new System.Drawing.Size(55, 13);
            this.lblInitialQty.TabIndex = 9;
            this.lblInitialQty.Text = "Initial Qty:";
            // 
            // numInitialQty
            // 
            this.numInitialQty.Location = new System.Drawing.Point(75, 82);
            this.numInitialQty.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numInitialQty.Name = "numInitialQty";
            this.numInitialQty.Size = new System.Drawing.Size(80, 20);
            this.numInitialQty.TabIndex = 10;
            // 
            // lblReorder
            // 
            this.lblReorder.AutoSize = true;
            this.lblReorder.Location = new System.Drawing.Point(170, 84);
            this.lblReorder.Name = "lblReorder";
            this.lblReorder.Size = new System.Drawing.Size(60, 13);
            this.lblReorder.TabIndex = 11;
            this.lblReorder.Text = "Reorder Pt:";
            // 
            // numReorderPoint
            // 
            this.numReorderPoint.Location = new System.Drawing.Point(236, 82);
            this.numReorderPoint.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numReorderPoint.Name = "numReorderPoint";
            this.numReorderPoint.Size = new System.Drawing.Size(80, 20);
            this.numReorderPoint.TabIndex = 12;
            // 
            // lblMaxStock
            // 
            this.lblMaxStock.AutoSize = true;
            this.lblMaxStock.Location = new System.Drawing.Point(332, 84);
            this.lblMaxStock.Name = "lblMaxStock";
            this.lblMaxStock.Size = new System.Drawing.Size(60, 13);
            this.lblMaxStock.TabIndex = 13;
            this.lblMaxStock.Text = "Max Level:";
            // 
            // numMaxStock
            // 
            this.numMaxStock.Location = new System.Drawing.Point(398, 82);
            this.numMaxStock.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numMaxStock.Name = "numMaxStock";
            this.numMaxStock.Size = new System.Drawing.Size(80, 20);
            this.numMaxStock.TabIndex = 14;
            // 
            // chkIsScarce
            // 
            this.chkIsScarce.AutoSize = true;
            this.chkIsScarce.Location = new System.Drawing.Point(496, 83);
            this.chkIsScarce.Name = "chkIsScarce";
            this.chkIsScarce.Size = new System.Drawing.Size(119, 17);
            this.chkIsScarce.TabIndex = 15;
            this.chkIsScarce.Text = "Scarce (Bulk Order)";
            this.chkIsScarce.UseVisualStyleBackColor = true;
            // 
            // btnNewItem
            // 
            this.btnNewItem.Location = new System.Drawing.Point(650, 80);
            this.btnNewItem.Name = "btnNewItem";
            this.btnNewItem.Size = new System.Drawing.Size(95, 23);
            this.btnNewItem.TabIndex = 16;
            this.btnNewItem.Text = "New Item";
            this.btnNewItem.UseVisualStyleBackColor = true;
            this.btnNewItem.Click += new System.EventHandler(this.btnNewItem_Click);
            // 
            // btnSaveItem
            // 
            this.btnSaveItem.Location = new System.Drawing.Point(755, 80);
            this.btnSaveItem.Name = "btnSaveItem";
            this.btnSaveItem.Size = new System.Drawing.Size(95, 23);
            this.btnSaveItem.TabIndex = 17;
            this.btnSaveItem.Text = "Save Item";
            this.btnSaveItem.UseVisualStyleBackColor = true;
            this.btnSaveItem.Click += new System.EventHandler(this.btnSaveItem_Click);
            // 
            // frmInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 530);
            this.MinimumSize = new System.Drawing.Size(916, 569);
            this.Name = "frmInventory";
            this.Text = "Inventory";
            this.Controls.Add(this.grpEditor);
            this.Controls.Add(this.btnIssueRf);
            this.Controls.Add(this.cboIssuableRf);
            this.Controls.Add(this.lblIssueRf);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.gridInventory);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.chkLowStockOnly);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.cboStockTag);
            this.Controls.Add(this.lblTagFilter);
            this.Load += new System.EventHandler(this.frmInventory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridInventory)).EndInit();
            this.grpEditor.ResumeLayout(false);
            this.grpEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInitialQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReorderPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTagFilter;
        private System.Windows.Forms.ComboBox cboStockTag;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkLowStockOnly;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView gridInventory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockTag;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyOnHand;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyReserved;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyAvailable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyIncoming;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReorderPoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxStockLevel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsScarce;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsLowStock;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblIssueRf;
        private System.Windows.Forms.ComboBox cboIssuableRf;
        private System.Windows.Forms.Button btnIssueRf;
        private System.Windows.Forms.GroupBox grpEditor;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblEditTag;
        private System.Windows.Forms.ComboBox cboEditTag;
        private System.Windows.Forms.Label lblInitialQty;
        private System.Windows.Forms.NumericUpDown numInitialQty;
        private System.Windows.Forms.Label lblReorder;
        private System.Windows.Forms.NumericUpDown numReorderPoint;
        private System.Windows.Forms.Label lblMaxStock;
        private System.Windows.Forms.NumericUpDown numMaxStock;
        private System.Windows.Forms.CheckBox chkIsScarce;
        private System.Windows.Forms.Button btnNewItem;
        private System.Windows.Forms.Button btnSaveItem;
    }
}