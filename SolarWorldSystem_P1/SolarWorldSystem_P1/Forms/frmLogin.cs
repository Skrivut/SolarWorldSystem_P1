using System;
using System.Windows.Forms;
using SolarWorldERP.Helpers;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    /// <summary>The login window. Shown first; the program only continues if login succeeds.</summary>
    public partial class frmLogin : Form
    {
        /// <summary>Builds the controls drawn in the designer.</summary>
        public frmLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Login button: checks the username and password with AuthService.
        /// On success it stores the user in Session and closes with OK. On failure it shows a message.
        /// </summary>
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
