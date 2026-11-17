namespace SolarWorldERP.Forms
{
    partial class frmApproval
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
            this.lblRfCaption = new System.Windows.Forms.Label();
            this.lblRfValue = new System.Windows.Forms.Label();
            this.lblProjCaption = new System.Windows.Forms.Label();
            this.lblProjValue = new System.Windows.Forms.Label();
            this.lblByCaption = new System.Windows.Forms.Label();
            this.lblByValue = new System.Windows.Forms.Label();
            this.lblDateCaption = new System.Windows.Forms.Label();
            this.lblDateValue = new System.Windows.Forms.Label();
            this.lblStatusCaption = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.gridLines = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRequested = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReserved = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShortage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblRemarksCaption = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridLines)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRfCaption
            // 
            this.lblRfCaption.Location = new System.Drawing.Point(16, 16);
            this.lblRfCaption.Text = "RF Number:";
            this.lblRfCaption.AutoSize = true;
            this.lblRfCaption.Size = new System.Drawing.Size(0, 13);
            this.lblRfCaption.TabIndex = 0;
            this.lblRfCaption.Name = "lblRfCaption";
            // 
            // lblRfValue
            // 
            this.lblRfValue.Location = new System.Drawing.Point(110, 16);
            this.lblRfValue.Text = "";
            this.lblRfValue.AutoSize = true;
            this.lblRfValue.Size = new System.Drawing.Size(0, 13);
            this.lblRfValue.TabIndex = 1;
            this.lblRfValue.Name = "lblRfValue";
            // 
            // lblProjCaption
            // 
            this.lblProjCaption.Location = new System.Drawing.Point(16, 40);
            this.lblProjCaption.Text = "Project:";
            this.lblProjCaption.AutoSize = true;
            this.lblProjCaption.Size = new System.Drawing.Size(0, 13);
            this.lblProjCaption.TabIndex = 2;
            this.lblProjCaption.Name = "lblProjCaption";
            // 
            // lblProjValue
            // 
            this.lblProjValue.Location = new System.Drawing.Point(110, 40);
            this.lblProjValue.Text = "";
            this.lblProjValue.AutoSize = true;
            this.lblProjValue.Size = new System.Drawing.Size(0, 13);
            this.lblProjValue.TabIndex = 3;
            this.lblProjValue.Name = "lblProjValue";
            // 
            // lblByCaption
            // 
            this.lblByCaption.Location = new System.Drawing.Point(16, 64);
            this.lblByCaption.Text = "Requested by:";
            this.lblByCaption.AutoSize = true;
            this.lblByCaption.Size = new System.Drawing.Size(0, 13);
            this.lblByCaption.TabIndex = 4;
            this.lblByCaption.Name = "lblByCaption";
            // 
            // lblByValue
            // 
            this.lblByValue.Location = new System.Drawing.Point(110, 64);
            this.lblByValue.Text = "";
            this.lblByValue.AutoSize = true;
            this.lblByValue.Size = new System.Drawing.Size(0, 13);
            this.lblByValue.TabIndex = 5;
            this.lblByValue.Name = "lblByValue";
            // 
            // lblDateCaption
            // 
            this.lblDateCaption.Location = new System.Drawing.Point(16, 88);
            this.lblDateCaption.Text = "Date:";
            this.lblDateCaption.AutoSize = true;
            this.lblDateCaption.Size = new System.Drawing.Size(0, 13);
            this.lblDateCaption.TabIndex = 6;
            this.lblDateCaption.Name = "lblDateCaption";
            // 
            // lblDateValue
            // 
            this.lblDateValue.Location = new System.Drawing.Point(110, 88);
            this.lblDateValue.Text = "";
            this.lblDateValue.AutoSize = true;
            this.lblDateValue.Size = new System.Drawing.Size(0, 13);
            this.lblDateValue.TabIndex = 7;
            this.lblDateValue.Name = "lblDateValue";
            // 
            // lblStatusCaption
            // 
            this.lblStatusCaption.Location = new System.Drawing.Point(16, 112);
            this.lblStatusCaption.Text = "Status:";
            this.lblStatusCaption.AutoSize = true;
            this.lblStatusCaption.Size = new System.Drawing.Size(0, 13);
            this.lblStatusCaption.TabIndex = 8;
            this.lblStatusCaption.Name = "lblStatusCaption";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.Location = new System.Drawing.Point(110, 112);
            this.lblStatusValue.Text = "";
            this.lblStatusValue.AutoSize = true;
            this.lblStatusValue.Size = new System.Drawing.Size(0, 13);
            this.lblStatusValue.TabIndex = 9;
            this.lblStatusValue.Name = "lblStatusValue";
            // 
            // gridLines
            // 
            this.gridLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colName,
            this.colUnit,
            this.colRequested,
            this.colReserved,
            this.colShortage});
            this.gridLines.AllowUserToAddRows = false;
            this.gridLines.AllowUserToDeleteRows = false;
            this.gridLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLines.AutoGenerateColumns = false;
            this.gridLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLines.Location = new System.Drawing.Point(16, 140);
            this.gridLines.MultiSelect = false;
            this.gridLines.ReadOnly = true;
            this.gridLines.RowHeadersVisible = false;
            this.gridLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLines.Size = new System.Drawing.Size(608, 220);
            this.gridLines.TabIndex = 10;
            this.gridLines.Name = "gridLines";
            // 
            // colCode
            // 
            this.colCode.DataPropertyName = "ItemCode";
            this.colCode.HeaderText = "Code";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "ItemName";
            this.colName.HeaderText = "Item";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colUnit
            // 
            this.colUnit.DataPropertyName = "Unit";
            this.colUnit.HeaderText = "Unit";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            // 
            // colRequested
            // 
            this.colRequested.DataPropertyName = "QtyRequested";
            this.colRequested.HeaderText = "Requested";
            this.colRequested.Name = "colRequested";
            this.colRequested.ReadOnly = true;
            // 
            // colReserved
            // 
            this.colReserved.DataPropertyName = "QtyReserved";
            this.colReserved.HeaderText = "Reserved";
            this.colReserved.Name = "colReserved";
            this.colReserved.ReadOnly = true;
            // 
            // colShortage
            // 
            this.colShortage.DataPropertyName = "QtyShortage";
            this.colShortage.HeaderText = "Shortage";
            this.colShortage.Name = "colShortage";
            this.colShortage.ReadOnly = true;
            // 
            // lblNote
            // 
            this.lblNote.Location = new System.Drawing.Point(16, 368);
            this.lblNote.Text = "";
            this.lblNote.Size = new System.Drawing.Size(608, 32);
            this.lblNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNote.TabIndex = 11;
            this.lblNote.Name = "lblNote";
            // 
            // lblRemarksCaption
            // 
            this.lblRemarksCaption.Location = new System.Drawing.Point(16, 404);
            this.lblRemarksCaption.Text = "Remarks (required to return or reject):";
            this.lblRemarksCaption.AutoSize = true;
            this.lblRemarksCaption.Size = new System.Drawing.Size(0, 13);
            this.lblRemarksCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRemarksCaption.TabIndex = 12;
            this.lblRemarksCaption.Name = "lblRemarksCaption";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(16, 424);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Size = new System.Drawing.Size(608, 40);
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.TabIndex = 13;
            this.txtRemarks.Name = "txtRemarks";
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(16, 476);
            this.btnApprove.Size = new System.Drawing.Size(100, 23);
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApprove.TabIndex = 14;
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(126, 476);
            this.btnReturn.Size = new System.Drawing.Size(130, 23);
            this.btnReturn.Text = "Return for revision";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReturn.TabIndex = 15;
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnReject
            // 
            this.btnReject.Location = new System.Drawing.Point(266, 476);
            this.btnReject.Size = new System.Drawing.Size(100, 23);
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReject.TabIndex = 16;
            this.btnReject.Name = "btnReject";
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(524, 476);
            this.btnClose.Size = new System.Drawing.Size(100, 23);
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.TabIndex = 17;
            this.btnClose.Name = "btnClose";
            // 
            // frmApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(640, 516);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmApproval";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Review Requisition";
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.lblRemarksCaption);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.gridLines);
            this.Controls.Add(this.lblStatusValue);
            this.Controls.Add(this.lblStatusCaption);
            this.Controls.Add(this.lblDateValue);
            this.Controls.Add(this.lblDateCaption);
            this.Controls.Add(this.lblByValue);
            this.Controls.Add(this.lblByCaption);
            this.Controls.Add(this.lblProjValue);
            this.Controls.Add(this.lblProjCaption);
            this.Controls.Add(this.lblRfValue);
            this.Controls.Add(this.lblRfCaption);
            this.Load += new System.EventHandler(this.frmApproval_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblRfCaption;
        private System.Windows.Forms.Label lblRfValue;
        private System.Windows.Forms.Label lblProjCaption;
        private System.Windows.Forms.Label lblProjValue;
        private System.Windows.Forms.Label lblByCaption;
        private System.Windows.Forms.Label lblByValue;
        private System.Windows.Forms.Label lblDateCaption;
        private System.Windows.Forms.Label lblDateValue;
        private System.Windows.Forms.Label lblStatusCaption;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.DataGridView gridLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRequested;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReserved;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShortage;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.Label lblRemarksCaption;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnClose;
    }
}
