using System;
using System.Windows.Forms;
using SolarWorldERP.Forms;

namespace SolarWorldERP
{
    static class Program
    {
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
