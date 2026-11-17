using System;
using System.Windows.Forms;
using SolarWorldERP.Helpers;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var user = AuthService.Login(txtUser.Text.Trim(), txtPass.Text);
                if (user == null)
                {
                    MessageBox.Show("Wrong username or password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPass.Clear();
                    txtPass.Focus();
                    return;
                }
                Session.UserName = user.UserName;
                Session.FullName = user.FullName;
                Session.Role = user.Role;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect to the database.\n\n" + ex.Message, "Login",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
