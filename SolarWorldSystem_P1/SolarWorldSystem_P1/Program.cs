using System;
using System.Windows.Forms;
using SolarWorldERP.Forms;

namespace SolarWorldERP
{
    static class Program
    {
        /// <summary>
        /// Where the program starts. Shows the login; if it succeeds, opens the main window.
        /// When the user chooses "Log out" the loop shows the login again; any other close ends the program.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                using (var login = new frmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK) return;
                }

                var main = new frmMain();
                Application.Run(main);

                if (!main.LoggedOut) return;   // closed the window or chose Exit
            }
        }
    }
}
