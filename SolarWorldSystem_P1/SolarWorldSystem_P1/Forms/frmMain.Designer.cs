namespace SolarWorldERP.Forms
{
    partial class frmMain
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
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRequisition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewRequisition = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRequisitionList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPurchasing = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInventory = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelivery = new System.Windows.Forms.ToolStripMenuItem();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuMain.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuRequisition,
            this.mnuPurchasing,
            this.mnuInventory,
            this.mnuDelivery});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(900, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuMain";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLogout,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuLogout
            // 
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(112, 22);
            this.mnuLogout.Text = "Log out";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(112, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuRequisition
            // 
            this.mnuRequisition.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewRequisition,
            this.mnuRequisitionList});
            this.mnuRequisition.Name = "mnuRequisition";
            this.mnuRequisition.Size = new System.Drawing.Size(81, 20);
            this.mnuRequisition.Text = "Requisition";
            // 
            // mnuNewRequisition
            // 
            this.mnuNewRequisition.Name = "mnuNewRequisition";
            this.mnuNewRequisition.Size = new System.Drawing.Size(163, 22);
            this.mnuNewRequisition.Text = "New Requisition";
            this.mnuNewRequisition.Click += new System.EventHandler(this.mnuNewRequisition_Click);
            // 
            // mnuRequisitionList
            // 
            this.mnuRequisitionList.Name = "mnuRequisitionList";
            this.mnuRequisitionList.Size = new System.Drawing.Size(163, 22);
            this.mnuRequisitionList.Text = "Requisition List";
            this.mnuRequisitionList.Click += new System.EventHandler(this.mnuRequisitionList_Click);
            // 
            // mnuPurchasing
            // 
            this.mnuPurchasing.Name = "mnuPurchasing";
            this.mnuPurchasing.Size = new System.Drawing.Size(75, 20);
            this.mnuPurchasing.Text = "Purchasing";
            this.mnuPurchasing.Click += new System.EventHandler(this.mnuPurchasing_Click);
            // 
            // mnuInventory
            // 
            this.mnuInventory.Name = "mnuInventory";
            this.mnuInventory.Size = new System.Drawing.Size(69, 20);
            this.mnuInventory.Text = "Inventory";
            this.mnuInventory.Click += new System.EventHandler(this.mnuInventory_Click);
            // 
            // mnuDelivery
            // 
            this.mnuDelivery.Name = "mnuDelivery";
            this.mnuDelivery.Size = new System.Drawing.Size(61, 20);
            this.mnuDelivery.Text = "Delivery";
            this.mnuDelivery.Click += new System.EventHandler(this.mnuDelivery_Click);
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusMain.Location = new System.Drawing.Point(0, 578);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(900, 22);
            this.statusMain.TabIndex = 1;
            this.statusMain.Text = "statusMain";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.menuMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuMain;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SolarWorld ERP";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuRequisition;
        private System.Windows.Forms.ToolStripMenuItem mnuNewRequisition;
        private System.Windows.Forms.ToolStripMenuItem mnuRequisitionList;
        private System.Windows.Forms.ToolStripMenuItem mnuPurchasing;
        private System.Windows.Forms.ToolStripMenuItem mnuInventory;
        private System.Windows.Forms.ToolStripMenuItem mnuDelivery;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}
