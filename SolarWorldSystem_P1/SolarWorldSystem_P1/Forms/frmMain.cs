using System;
using System.Windows.Forms;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Forms
{
    /// <summary>
    /// Main window: an MDI container. Each module opens its forms inside it.
    /// Teammates: add your menu items in the designer, then call OpenChild&lt;YourForm&gt;() from the click handler.
    /// </summary>
    public partial class frmMain : Form
    {
        /// <summary>True when the user chose "Log out" (Program.cs then shows the login again).</summary>
        public bool LoggedOut { get; private set; }

        /// <summary>Builds the controls drawn in the designer.</summary>
        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>When the window opens: shows who is logged in and hides menus the role should not see.</summary>
        private void frmMain_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Logged in as " + Session.FullName + " (" + Session.Role + ")";
            ApplyRoleAccess();
        }

        /// <summary>Decides which top-level menus each role can see. Adjust once the client confirms who does what.</summary>
        private void ApplyRoleAccess()
        {
            bool owner = Session.Role == "Owner";
            bool staff = Session.Role == "Staff";

            mnuRequisition.Visible = owner || staff;
            mnuPurchasing.Visible = owner;
            mnuInventory.Visible = owner || staff;
            mnuDelivery.Visible = true;   // owner, staff and driver
        }

        /// <summary>Opens a form inside the main window, or brings it to the front if it is already open.</summary>
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

        /// <summary>Requisition > New Requisition: opens the create-RF form.</summary>
        private void mnuNewRequisition_Click(object sender, EventArgs e)
        {
            OpenChild<frmRequisition>();
        }

        /// <summary>Requisition > Requisition List: opens the list where staff track RFs and the owner approves them.</summary>
        private void mnuRequisitionList_Click(object sender, EventArgs e)
        {
            OpenChild<frmRequisitionList>();
        }

        /// <summary>Opens the purchasing module.</summary>
        private void mnuPurchasing_Click(object sender, EventArgs e)
        {
            OpenChild<frmPurchasing>();
        }

        /// <summary>Placeholder until the Inventory module is connected.</summary>
        private void mnuInventory_Click(object sender, EventArgs e) { NotBuilt("Inventory"); }

        /// <summary>Opens the delivery module.</summary>
        private void mnuDelivery_Click(object sender, EventArgs e)
        {
            OpenChild<frmDelivery>();
        }

        /// <summary>Shows a "not connected yet" message for a module that has no screens yet.</summary>
        private void NotBuilt(string module)
        {
            MessageBox.Show(module + " is not connected yet.", "SolarWorld ERP",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>File > Log out: closes this window and tells Program.cs to show the login again.</summary>
        private void mnuLogout_Click(object sender, EventArgs e)
        {
            LoggedOut = true;
            Close();
        }

        /// <summary>File > Exit: closes the window and the whole program.</summary>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
