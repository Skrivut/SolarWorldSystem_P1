using System;
using System.Windows.Forms;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Forms
{
    // Main window: an MDI container. Each module opens its forms inside it.
    // Teammates: add your menu items in the designer, then call OpenChild<YourForm>() from the click handler.
    public partial class frmMain : Form
    {
        // True when the user chose "Log out" (Program.cs then shows the login again).
        public bool LoggedOut { get; private set; }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Logged in as " + Session.FullName + " (" + Session.Role + ")";
            ApplyRoleAccess();
        }

        // Which menus each role can see. Adjust once the client confirms who does what.
        private void ApplyRoleAccess()
        {
            bool owner = Session.Role == "Owner";
            bool staff = Session.Role == "Staff";

            mnuRequisition.Visible = owner || staff;
            mnuPurchasing.Visible = owner;
            mnuInventory.Visible = owner || staff;
            mnuDelivery.Visible = true;   // owner, staff and driver
        }

        // Opens a form inside the main window, or brings it to the front if already open.
        private void OpenChild<T>() where T : Form, new()
        {
            foreach (Form f in MdiChildren)
            {
                if (f is T) { f.Activate(); return; }
            }
            var child = new T();
            child.MdiParent = this;
            child.Show();
        }

        private void mnuNewRequisition_Click(object sender, EventArgs e)
        {
            OpenChild<frmRequisition>();
        }

        // Placeholders until those modules are connected.
        private void mnuPurchasing_Click(object sender, EventArgs e) { NotBuilt("Purchasing"); }
        private void mnuInventory_Click(object sender, EventArgs e) { NotBuilt("Inventory"); }
        private void mnuDelivery_Click(object sender, EventArgs e) { NotBuilt("Delivery"); }

        private void NotBuilt(string module)
        {
            MessageBox.Show(module + " is not connected yet.", "SolarWorld ERP",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            LoggedOut = true;
            Close();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
