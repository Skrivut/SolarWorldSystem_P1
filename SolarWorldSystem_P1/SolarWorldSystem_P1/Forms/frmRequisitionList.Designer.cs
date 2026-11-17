namespace SolarWorldERP.Forms
{
    partial class frmRequisitionList
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
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.gridRf = new System.Windows.Forms.DataGridView();
            this.colRfId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRfNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRequestedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.lblLines = new System.Windows.Forms.Label();
            this.gridLines = new System.Windows.Forms.DataGridView();
            this.colLineCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineRequested = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineReserved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineShortage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReview = new System.Windows.Forms.Button();
            this.btnRevise = new System.Windows.Forms.Button();
            this.btnCancelRf = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridRf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLines)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.Location = new System.Drawing.Point(16, 16);
            this.lblStatusFilter.Text = "Status:";
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Size = new System.Drawing.Size(0, 13);
            this.lblStatusFilter.TabIndex = 0;
            this.lblStatusFilter.Name = "lblStatusFilter";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(70, 12);
            this.cboStatus.Size = new System.Drawing.Size(180, 21);
            this.cboStatus.TabIndex = 1;
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(260, 10);
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // gridRf
            // 
            this.gridRf.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRfId,
            this.colRfNumber,
            this.colProject,
            this.colRequestedBy,
            this.colDate,
            this.colStatus});
            this.gridRf.AllowUserToAddRows = false;
            this.gridRf.AllowUserToDeleteRows = false;
            this.gridRf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.gridRf.AutoGenerateColumns = false;
            this.gridRf.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridRf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRf.Location = new System.Drawing.Point(16, 44);
            this.gridRf.MultiSelect = false;
            this.gridRf.ReadOnly = true;
            this.gridRf.RowHeadersVisible = false;
            this.gridRf.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRf.Size = new System.Drawing.Size(868, 230);
            this.gridRf.TabIndex = 3;
            this.gridRf.Name = "gridRf";
            this.gridRf.SelectionChanged += new System.EventHandler(this.gridRf_SelectionChanged);
            // 
            // colRfId
            // 
            this.colRfId.DataPropertyName = "RfId";
            this.colRfId.HeaderText = "RfId";
            this.colRfId.Name = "colRfId";
            this.colRfId.ReadOnly = true;
            this.colRfId.Visible = false;
            // 
            // colRfNumber
            // 
            this.colRfNumber.DataPropertyName = "RfNumber";
            this.colRfNumber.HeaderText = "RF Number";
            this.colRfNumber.Name = "colRfNumber";
            this.colRfNumber.ReadOnly = true;
            // 
            // colProject
            // 
            this.colProject.DataPropertyName = "ProjectName";
            this.colProject.HeaderText = "Project";
            this.colProject.Name = "colProject";
            this.colProject.ReadOnly = true;
            // 
            // colRequestedBy
            // 
            this.colRequestedBy.DataPropertyName = "RequestedBy";
            this.colRequestedBy.HeaderText = "Requested by";
            this.colRequestedBy.Name = "colRequestedBy";
            this.colRequestedBy.ReadOnly = true;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "DateCreated";
            this.colDate.HeaderText = "Date created";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // lblRemarks
            // 
            this.lblRemarks.Location = new System.Drawing.Point(16, 282);
            this.lblRemarks.Text = "";
            this.lblRemarks.Size = new System.Drawing.Size(868, 32);
            this.lblRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemarks.TabIndex = 4;
            this.lblRemarks.Name = "lblRemarks";
            // 
            // lblLines
            // 
            this.lblLines.Location = new System.Drawing.Point(16, 320);
            this.lblLines.Text = "Items in the selected requisition:";
            this.lblLines.AutoSize = true;
            this.lblLines.Size = new System.Drawing.Size(0, 13);
            this.lblLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.lblLines.TabIndex = 5;
            this.lblLines.Name = "lblLines";
            // 
            // gridLines
            // 
            this.gridLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLineCode,
            this.colLineName,
            this.colLineUnit,
            this.colLineRequested,
            this.colLineReserved,
            this.colLineShortage});
            this.gridLines.AllowUserToAddRows = false;
            this.gridLines.AllowUserToDeleteRows = false;
            this.gridLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLines.AutoGenerateColumns = false;
            this.gridLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLines.Location = new System.Drawing.Point(16, 340);
            this.gridLines.MultiSelect = false;
            this.gridLines.ReadOnly = true;
            this.gridLines.RowHeadersVisible = false;
            this.gridLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLines.Size = new System.Drawing.Size(868, 200);
            this.gridLines.TabIndex = 6;
            this.gridLines.Name = "gridLines";
            // 
            // colLineCode
            // 
            this.colLineCode.DataPropertyName = "ItemCode";
            this.colLineCode.HeaderText = "Code";
            this.colLineCode.Name = "colLineCode";
            this.colLineCode.ReadOnly = true;
            // 
            // colLineName
            // 
            this.colLineName.DataPropertyName = "ItemName";
            this.colLineName.HeaderText = "Item";
            this.colLineName.Name = "colLineName";
            this.colLineName.ReadOnly = true;
            // 
            // colLineUnit
            // 
            this.colLineUnit.DataPropertyName = "Unit";
            this.colLineUnit.HeaderText = "Unit";
            this.colLineUnit.Name = "colLineUnit";
            this.colLineUnit.ReadOnly = true;
            // 
            // colLineRequested
            // 
            this.colLineRequested.DataPropertyName = "QtyRequested";
            this.colLineRequested.HeaderText = "Requested";
            this.colLineRequested.Name = "colLineRequested";
            this.colLineRequested.ReadOnly = true;
            // 
            // colLineReserved
            // 
            this.colLineReserved.DataPropertyName = "QtyReserved";
            this.colLineReserved.HeaderText = "Reserved";
            this.colLineReserved.Name = "colLineReserved";
            this.colLineReserved.ReadOnly = true;
            // 
            // colLineShortage
            // 
            this.colLineShortage.DataPropertyName = "QtyShortage";
            this.colLineShortage.HeaderText = "Shortage";
            this.colLineShortage.Name = "colLineShortage";
            this.colLineShortage.ReadOnly = true;
            // 
            // btnReview
            // 
            this.btnReview.Location = new System.Drawing.Point(16, 552);
            this.btnReview.Size = new System.Drawing.Size(130, 23);
            this.btnReview.Text = "Review / Approve";
            this.btnReview.UseVisualStyleBackColor = true;
            this.btnReview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReview.TabIndex = 7;
            this.btnReview.Name = "btnReview";
            this.btnReview.Click += new System.EventHandler(this.btnReview_Click);
            // 
            // btnRevise
            // 
            this.btnRevise.Location = new System.Drawing.Point(156, 552);
            this.btnRevise.Size = new System.Drawing.Size(130, 23);
            this.btnRevise.Text = "Revise && Resubmit";
            this.btnRevise.UseVisualStyleBackColor = true;
            this.btnRevise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRevise.TabIndex = 8;
            this.btnRevise.Name = "btnRevise";
            this.btnRevise.Click += new System.EventHandler(this.btnRevise_Click);
            // 
            // btnCancelRf
            // 
            this.btnCancelRf.Location = new System.Drawing.Point(296, 552);
            this.btnCancelRf.Size = new System.Drawing.Size(100, 23);
            this.btnCancelRf.Text = "Cancel RF";
            this.btnCancelRf.UseVisualStyleBackColor = true;
            this.btnCancelRf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelRf.TabIndex = 9;
            this.btnCancelRf.Name = "btnCancelRf";
            this.btnCancelRf.Click += new System.EventHandler(this.btnCancelRf_Click);
            // 
            // frmRequisitionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 590);
            this.MinimumSize = new System.Drawing.Size(916, 629);
            this.Name = "frmRequisitionList";
            this.Text = "Requisition List";
            this.Controls.Add(this.btnCancelRf);
            this.Controls.Add(this.btnRevise);
            this.Controls.Add(this.btnReview);
            this.Controls.Add(this.gridLines);
            this.Controls.Add(this.lblLines);
            this.Controls.Add(this.lblRemarks);
            this.Controls.Add(this.gridRf);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.lblStatusFilter);
            this.Load += new System.EventHandler(this.frmRequisitionList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridRf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblStatusFilter;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView gridRf;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRfId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRfNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRequestedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.Label lblLines;
        private System.Windows.Forms.DataGridView gridLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineRequested;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineReserved;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineShortage;
        private System.Windows.Forms.Button btnReview;
        private System.Windows.Forms.Button btnRevise;
        private System.Windows.Forms.Button btnCancelRf;
    }
}
