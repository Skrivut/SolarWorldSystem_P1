using System.Drawing;
using System.Windows.Forms;

namespace SolarWorldERP.Helpers
{
    /// <summary>
    /// The ONE place that knows what the app looks like (colors, fonts, how a button or grid is styled).
    /// Every screen reads from here, so changing the look later means editing this file only.
    /// Values come from the "Sunlit Panel" theme on page 7 of the design PDF.
    /// </summary>
    public static class Theme
    {
        // ----- Colors -----
        public static readonly Color Canvas = ColorTranslator.FromHtml("#FAF9F6");  // page background
        public static readonly Color LeftPane = ColorTranslator.FromHtml("#F4F2EC");  // menu rail and inbox background
        public static readonly Color Hairline = ColorTranslator.FromHtml("#E8E5DD");  // thin dividers and borders
        public static readonly Color Ink = ColorTranslator.FromHtml("#2A2925");  // main text
        public static readonly Color Muted = ColorTranslator.FromHtml("#8B877D");  // secondary text
        public static readonly Color Navy = ColorTranslator.FromHtml("#1F3A5F");  // primary buttons, active menu item
        public static readonly Color NavyTint = ColorTranslator.FromHtml("#E3EAF3");  // selected row, document header band
        public static readonly Color Amber = ColorTranslator.FromHtml("#E8A317");  // needs attention
        public static readonly Color AmberTint = ColorTranslator.FromHtml("#FCF0D2");
        public static readonly Color AmberText = ColorTranslator.FromHtml("#946600");
        public static readonly Color Green = ColorTranslator.FromHtml("#3F8F5B");  // success
        public static readonly Color GreenTint = ColorTranslator.FromHtml("#E1F0E6");
        public static readonly Color Red = ColorTranslator.FromHtml("#B94A2C");  // shortage, rejected
        public static readonly Color RedTint = ColorTranslator.FromHtml("#F8E4DD");

        // ----- Fonts (Segoe UI ships with Windows; the PDF uses Inter, which looks very similar) -----
        public static readonly Font Body = new Font("Segoe UI", 9.5f);
        public static readonly Font Small = new Font("Segoe UI", 8.5f);
        public static readonly Font Bold = new Font("Segoe UI", 9.5f, FontStyle.Bold);
        public static readonly Font Title = new Font("Segoe UI", 20f, FontStyle.Bold);

        // ----- Style helpers: call these from a form instead of setting properties by hand -----

        /// <summary>Navy filled button (Approve, Confirm, Submit).</summary>
        public static void StylePrimaryButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Navy;
            b.ForeColor = Color.White;
            b.Font = Bold;
            b.Cursor = Cursors.Hand;
        }

        /// <summary>White button with a thin border (Print, Cancel, Save draft).</summary>
        public static void StyleGhostButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Hairline;
            b.BackColor = Color.White;
            b.ForeColor = Ink;
            b.Font = Bold;
            b.Cursor = Cursors.Hand;
        }

        /// <summary>White button with a red outline (Reject).</summary>
        public static void StyleDangerButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Red;
            b.BackColor = Color.White;
            b.ForeColor = Red;
            b.Font = Bold;
            b.Cursor = Cursors.Hand;
        }

        /// <summary>Turns a default Windows grid into a clean one: no borders, light header, thin row lines.</summary>
        public static void StyleGrid(DataGridView g)
        {
            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Color.White;
            g.EnableHeadersVisualStyles = false;               // lets our header colors apply
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 32;
            g.ColumnHeadersDefaultCellStyle.BackColor = LeftPane;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Muted;
            g.ColumnHeadersDefaultCellStyle.Font = Small;
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = LeftPane;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.GridColor = Hairline;
            g.RowHeadersVisible = false;
            g.RowTemplate.Height = 32;
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = Ink;
            g.DefaultCellStyle.Font = Body;
            g.DefaultCellStyle.SelectionBackColor = NavyTint;
            g.DefaultCellStyle.SelectionForeColor = Ink;
            g.AllowUserToAddRows = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}