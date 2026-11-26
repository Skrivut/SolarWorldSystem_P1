using System;
using System.Drawing;
using System.Windows.Forms;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Forms
{
    /// <summary>
    /// A throwaway test window that shows the Theme working. It builds its controls in code,
    /// so there is no designer file and it cannot clash with anyone else's work.
    /// Delete this file once the real screens use the Theme.
    /// </summary>
    public class frmThemePreview : Form
    {
        public frmThemePreview()
        {
            Text = "Theme preview";
            Size = new Size(760, 520);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Theme.Canvas;
            Font = Theme.Body;

            var title = new Label { Text = "Theme preview", Font = Theme.Title, ForeColor = Theme.Ink, AutoSize = true, Location = new Point(24, 16) };
            Controls.Add(title);

            // Color swatches
            var swatches = new FlowLayoutPanel { Location = new Point(24, 70), Size = new Size(700, 60), WrapContents = false };
            AddSwatch(swatches, "Navy", Theme.Navy, Color.White);
            AddSwatch(swatches, "Amber", Theme.Amber, Theme.Ink);
            AddSwatch(swatches, "Green", Theme.Green, Color.White);
            AddSwatch(swatches, "Red", Theme.Red, Color.White);
            AddSwatch(swatches, "Navy tint", Theme.NavyTint, Theme.Ink);
            AddSwatch(swatches, "Left pane", Theme.LeftPane, Theme.Ink);
            Controls.Add(swatches);

            // Buttons
            var approve = new Button { Text = "Approve RF + PO", Size = new Size(140, 34), Location = new Point(24, 150) };
            var print = new Button { Text = "Print", Size = new Size(90, 34), Location = new Point(176, 150) };
            var reject = new Button { Text = "Reject", Size = new Size(90, 34), Location = new Point(278, 150) };
            Theme.StylePrimaryButton(approve);
            Theme.StyleGhostButton(print);
            Theme.StyleDangerButton(reject);
            Controls.Add(approve); Controls.Add(print); Controls.Add(reject);

            // Grid with sample rows
            var grid = new DataGridView { Location = new Point(24, 210), Size = new Size(700, 220) };
            Controls.Add(grid);
            Theme.StyleGrid(grid);
            grid.Columns.Add("item", "ITEM");
            grid.Columns.Add("tag", "STOCK TAG");
            grid.Columns.Add("req", "REQUESTED");
            grid.Columns.Add("res", "RESERVED");
            grid.Columns.Add("short", "SHORTAGE");
            grid.Rows.Add("Solar panel 550W", "Project", 20, 14, 6);
            grid.Rows.Add("Mounting rail 4.2 m", "Project", 40, 40, 0);
            grid.Rows.Add("MC4 connector pair", "Project", 100, 100, 0);
            grid.Columns[0].Width = 260;
            grid.Rows[0].Cells[4].Style.ForeColor = Theme.Red;
        }

        private static void AddSwatch(FlowLayoutPanel parent, string name, Color back, Color fore)
        {
            parent.Controls.Add(new Label { Text = name, BackColor = back, ForeColor = fore, Size = new Size(104, 50), TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(0, 0, 8, 0) });
        }
    }
}