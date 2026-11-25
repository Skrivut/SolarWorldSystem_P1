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
            this.lblRemarks = new System.Windows.Forms.Label();
            this.lblLines = new System.Windows.Forms.Label();
            this.gridLines = new System.Windows.Forms.DataGridView();
            this.btnReview = new System.Windows.Forms.Button();
            this.btnRevise = new System.Windows.Forms.Button();
            this.btnCancelRf = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnReserveShortage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridRf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLines)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.AutoSize = true;
            this.lblStatusFilter.Location = new System.Drawing.Point(16, 16);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(40, 13);
            this.lblStatusFilter.TabIndex = 0;
            this.lblStatusFilter.Text = "Status:";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(70, 12);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(180, 21);
            this.cboStatus.TabIndex = 1;
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.cboStatus_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(260, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // gridRf
            // 
            this.gridRf.AllowUserToAddRows = false;
            this.gridRf.AllowUserToDeleteRows = false;
            this.gridRf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridRf.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridRf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRf.Location = new System.Drawing.Point(16, 44);
            this.gridRf.MultiSelect = false;
            this.gridRf.Name = "gridRf";
            this.gridRf.ReadOnly = true;
            this.gridRf.RowHeadersVisible = false;
            this.gridRf.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRf.Size = new System.Drawing.Size(868, 230);
            this.gridRf.TabIndex = 3;
            this.gridRf.SelectionChanged += new System.EventHandler(this.gridRf_SelectionChanged);
            // 
            // lblRemarks
            // 
            this.lblRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRemarks.Location = new System.Drawing.Point(16, 282);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(868, 32);
            this.lblRemarks.TabIndex = 4;
            // 
            // lblLines
            // 
            this.lblLines.AutoSize = true;
            this.lblLines.Location = new System.Drawing.Point(16, 320);
            this.lblLines.Name = "lblLines";
            this.lblLines.Size = new System.Drawing.Size(157, 13);
            this.lblLines.TabIndex = 5;
            this.lblLines.Text = "Items in the selected requisition:";
            // 
            // gridLines
            // 
            this.gridLines.AllowUserToAddRows = false;
            this.gridLines.AllowUserToDeleteRows = false;
            this.gridLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLines.Location = new System.Drawing.Point(16, 340);
            this.gridLines.MultiSelect = false;
            this.gridLines.Name = "gridLines";
            this.gridLines.ReadOnly = true;
            this.gridLines.RowHeadersVisible = false;
            this.gridLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLines.Size = new System.Drawing.Size(868, 200);
            this.gridLines.TabIndex = 6;
            // 
            // btnReview
            // 
            this.btnReview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReview.Location = new System.Drawing.Point(16, 552);
            this.btnReview.Name = "btnReview";
            this.btnReview.Size = new System.Drawing.Size(130, 23);
            this.btnReview.TabIndex = 7;
            this.btnReview.Text = "Review / Approve";
            this.btnReview.UseVisualStyleBackColor = true;
            this.btnReview.Click += new System.EventHandler(this.btnReview_Click);
            // 
            // btnRevise
            // 
            this.btnRevise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRevise.Location = new System.Drawing.Point(156, 552);
            this.btnRevise.Name = "btnRevise";
            this.btnRevise.Size = new System.Drawing.Size(130, 23);
            this.btnRevise.TabIndex = 8;
            this.btnRevise.Text = "Revise && Resubmit";
            this.btnRevise.UseVisualStyleBackColor = true;
            this.btnRevise.Click += new System.EventHandler(this.btnRevise_Click);
            // 
            // btnCancelRf
            // 
            this.btnCancelRf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelRf.Location = new System.Drawing.Point(296, 552);
            this.btnCancelRf.Name = "btnCancelRf";
            this.btnCancelRf.Size = new System.Drawing.Size(100, 23);
            this.btnCancelRf.TabIndex = 9;
            this.btnCancelRf.Text = "Cancel RF";
            this.btnCancelRf.UseVisualStyleBackColor = true;
            this.btnCancelRf.Click += new System.EventHandler(this.btnCancelRf_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 290);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnReserveShortage
            // 
            this.btnReserveShortage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReserveShortage.Location = new System.Drawing.Point(411, 552);
            this.btnReserveShortage.Name = "btnReserveShortage";
            this.btnReserveShortage.Size = new System.Drawing.Size(150, 23);
            this.btnReserveShortage.TabIndex = 11;
            this.btnReserveShortage.Text = "Reserve available stock";
            this.btnReserveShortage.UseVisualStyleBackColor = true;
            this.btnReserveShortage.Click += new System.EventHandler(this.btnReserveShortage_Click);
            // 
            // frmRequisitionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 590);
            this.Controls.Add(this.btnReserveShortage);
            this.Controls.Add(this.button1);
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
            this.MinimumSize = new System.Drawing.Size(916, 629);
            this.Name = "frmRequisitionList";
            this.Text = "Requisition List";
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnReserveShortage;
    }
}
