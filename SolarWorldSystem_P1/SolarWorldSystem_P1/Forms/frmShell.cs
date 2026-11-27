using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Forms
{
    /// <summary>
    /// The new main window: a menu rail with icons on the left and a content area on the right.
    /// It does the same job as frmMain (shows the modules, hides what a role should not see, logs out)
    /// but with the new layout. Which items a role sees comes from Permissions.
    /// The existing screens are shown INSIDE the content area, so nothing stops working while we redesign.
    /// </summary>
    public class frmShell : Form
    {
        /// <summary>True when the user chose "Log out" (Program.cs then shows the login again).</summary>
        public bool LoggedOut { get; private set; }

        private readonly Panel _rail = new Panel();
        private readonly FlowLayoutPanel _menu = new FlowLayoutPanel();
        private readonly Panel _content = new Panel();
        private readonly Dictionary<string, RailButton> _buttons = new Dictionary<string, RailButton>();
        private readonly Dictionary<string, Func<Form>> _screens = new Dictionary<string, Func<Form>>();
        private readonly ToolTip _tip = new ToolTip();
        private Form _current;

        public frmShell()
        {
            Text = "SolarWorld ERP";
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1000, 640);
            BackColor = Theme.Canvas;
            Font = Theme.Body;
            BuildLayout();
            Load += frmShell_Load;
        }

        /// <summary>Lays out the three big areas: rail on the left, a thin line, content filling the rest.</summary>
        private void BuildLayout()
        {
            _rail.Dock = DockStyle.Left;
            _rail.Width = 72;
            _rail.BackColor = Theme.LeftPane;

            var line = new Panel { Dock = DockStyle.Left, Width = 1, BackColor = Theme.Hairline };

            _content.Dock = DockStyle.Fill;
            _content.BackColor = Theme.Canvas;

            // Order matters for docking: the Fill control goes in first, the edge controls after it.
            Controls.Add(_content);
            Controls.Add(line);
            Controls.Add(_rail);

            BuildRail();
        }

        /// <summary>Fills the rail: logo on top, module buttons in the middle, role and log out at the bottom.</summary>
        private void BuildRail()
        {
            _menu.Dock = DockStyle.Fill;
            _menu.FlowDirection = FlowDirection.TopDown;
            _menu.WrapContents = false;
            _menu.BackColor = Theme.LeftPane;
            _menu.Padding = new Padding(0);

            // Logo: a rounded navy square with an amber dot (placeholder until the real logo is exported from Figma)
            var logoArea = new Panel { Dock = DockStyle.Top, Height = 64, BackColor = Theme.LeftPane };
            var logo = new LogoMark { Location = new Point(22, 18) };
            logoArea.Controls.Add(logo);

            // Bottom: who is logged in, and Log out
            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 96, BackColor = Theme.LeftPane };
            var role = new Label { Text = Session.Role, Dock = DockStyle.Top, Height = 28, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Theme.Muted, Font = Theme.Small };
            var logout = new RailButton("Log out", "logout") { Dock = DockStyle.Bottom, Height = 64 };
            logout.Click += (s, e) => { LoggedOut = true; Close(); };
            _tip.SetToolTip(logout, "Logged in as " + Session.FullName + " (" + Session.Role + ")");
            bottom.Controls.Add(logout);
            bottom.Controls.Add(role);

            // Fill goes in first, then the edges
            _rail.Controls.Add(_menu);
            _rail.Controls.Add(logoArea);
            _rail.Controls.Add(bottom);
        }

        /// <summary>Runs when the window opens: adds the menu items this role may see, then opens the first screen.</summary>
        private void frmShell_Load(object sender, EventArgs e)
        {
            // All role rules live in Permissions.cs. Nothing is decided here.
            AddItem("Inbox", "inbox", Permissions.CanOpenInbox, null, () => new frmInbox());
            AddItem("Requests", "requests", Permissions.CanOpenRequests, null, () => new frmRequisitionList());
            AddItem("Purchasing", "purchasing", Permissions.CanOpenPurchasing,
                Permissions.PurchasingIsViewOnly ? "View only" : null, () => new frmPurchasing());
            AddItem("Delivery", "delivery", Permissions.CanOpenDelivery, null, () => new frmDelivery());
            AddItem("Inventory", "inventory", Permissions.CanOpenInventory, null, () => new frmInventory());

            Text = "SolarWorld ERP - " + Session.FullName + " (" + Session.Role + ")";

            // First screen: the Inbox for owner and staff, Delivery for the driver
            string start = Permissions.CanOpenInbox ? "Inbox" : "Delivery";
            if (_screens.ContainsKey(start)) ShowModule(start);
        }

        /// <summary>Adds one rail button, but only if this role may see it. The hint shows as a tooltip.</summary>
        private void AddItem(string name, string icon, bool visible, string hint, Func<Form> create)
        {
            if (!visible) return;
            var b = new RailButton(name, icon);
            b.Click += (s, e) => ShowModule(name);
            if (hint != null) _tip.SetToolTip(b, hint);
            _buttons[name] = b;
            _screens[name] = create;
            _menu.Controls.Add(b);
        }

        /// <summary>Highlights the chosen rail button and shows its screen inside the content area.</summary>
        private void ShowModule(string name)
        {
            foreach (var kv in _buttons) kv.Value.Active = (kv.Key == name);

            if (_current != null) { _current.Close(); _current = null; }   // closes the previous screen
            _content.Controls.Clear();

            Form screen = _screens[name]();
            screen.TopLevel = false;                        // lets a form live inside a panel
            screen.FormBorderStyle = FormBorderStyle.None;  // no title bar of its own
            screen.Dock = DockStyle.Fill;
            _content.Controls.Add(screen);
            screen.Show();
            _current = screen;
        }

        /// <summary>A simple "coming soon" screen so every rail button works from day one.</summary>
        private static Form Placeholder(string title, string text)
        {
            var f = new Form { BackColor = Theme.Canvas };
            f.Controls.Add(new Label { Text = title, Font = Theme.Title, ForeColor = Theme.Ink, AutoSize = true, Location = new Point(32, 28) });
            f.Controls.Add(new Label { Text = text, Font = Theme.Body, ForeColor = Theme.Muted, AutoSize = true, Location = new Point(34, 84) });
            return f;
        }
    }

    /// <summary>
    /// One button on the menu rail: a drawn icon with its label underneath, like the design.
    /// Active = navy tint, hover = light grey. Icons are drawn with simple lines, so no image files are needed.
    /// </summary>
    internal class RailButton : Control
    {
        private static readonly Font NormalFont = new Font("Segoe UI", 8.5f);
        private static readonly Font BoldFont = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        private readonly string _icon;
        private bool _active, _hover;

        public RailButton(string text, string icon)
        {
            Text = text;
            _icon = icon;
            Size = new Size(72, 64);
            Margin = new Padding(0);
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        /// <summary>True for the module that is currently open.</summary>
        public bool Active { get { return _active; } set { _active = value; Invalidate(); } }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Theme.LeftPane);

            if (_active || _hover)
            {
                using (var path = RoundedRect(10, 4, 52, 56, 10))
                using (var fill = new SolidBrush(_active ? Theme.NavyTint : Theme.Hairline))
                    g.FillPath(fill, path);
            }

            Color c = _active ? Theme.Navy : Theme.Muted;
            using (var pen = new Pen(c, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                g.TranslateTransform(36, 24);   // icons are drawn around (0,0)
                DrawIcon(g, pen);
                g.ResetTransform();
            }

            TextRenderer.DrawText(g, Text, _active ? BoldFont : NormalFont, new Rectangle(0, 40, 72, 20), c,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
        }

        private void DrawIcon(Graphics g, Pen p)
        {
            switch (_icon)
            {
                case "inbox":
                    Round(g, p, -10, -8, 20, 16, 3);
                    g.DrawLines(p, new[] { new PointF(-10, 1), new PointF(-4, 1), new PointF(-2.5f, 4), new PointF(2.5f, 4), new PointF(4, 1), new PointF(10, 1) });
                    break;
                case "requests":
                    Round(g, p, -8, -10, 16, 20, 3);
                    g.DrawLine(p, -4, -4, 4, -4); g.DrawLine(p, -4, 0, 4, 0); g.DrawLine(p, -4, 4, 1, 4);
                    break;
                case "purchasing":
                    Round(g, p, -9, -4, 18, 14, 3);
                    g.DrawArc(p, -4, -10, 8, 8, 180, 180);
                    g.DrawLine(p, -4, -6, -4, -4); g.DrawLine(p, 4, -6, 4, -4);
                    break;
                case "delivery":
                    Round(g, p, -11, -7, 13, 12, 2);
                    g.DrawLines(p, new[] { new PointF(2, -3), new PointF(7, -3), new PointF(11, 1), new PointF(11, 5), new PointF(2, 5) });
                    g.DrawEllipse(p, -8, 6, 4, 4); g.DrawEllipse(p, 4, 6, 4, 4);
                    break;
                case "inventory":
                    Round(g, p, -9, -9, 8, 8, 2); Round(g, p, 1, -9, 8, 8, 2);
                    Round(g, p, -9, 1, 8, 8, 2); Round(g, p, 1, 1, 8, 8, 2);
                    break;
                case "logout":
                    g.DrawLines(p, new[] { new PointF(-2, -9), new PointF(-9, -9), new PointF(-9, 9), new PointF(-2, 9) });
                    g.DrawLine(p, -3, 0, 9, 0);
                    g.DrawLines(p, new[] { new PointF(5, -4), new PointF(9, 0), new PointF(5, 4) });
                    break;
            }
        }

        private static void Round(Graphics g, Pen p, float x, float y, float w, float h, float r)
        {
            using (var path = RoundedRect(x, y, w, h, r)) g.DrawPath(p, path);
        }

        private static GraphicsPath RoundedRect(float x, float y, float w, float h, float r)
        {
            float d = r * 2;
            var path = new GraphicsPath();
            path.AddArc(x, y, d, d, 180, 90);
            path.AddArc(x + w - d, y, d, d, 270, 90);
            path.AddArc(x + w - d, y + h - d, d, d, 0, 90);
            path.AddArc(x, y + h - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>The small logo at the top of the rail: a rounded navy square with an amber dot.</summary>
    internal class LogoMark : Control
    {
        public LogoMark()
        {
            Size = new Size(28, 28);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Theme.LeftPane);
            float d = 16;   // corner size: a bigger number makes rounder corners
            using (var path = new GraphicsPath())
            {
                path.AddArc(0, 0, d, d, 180, 90);
                path.AddArc(Width - 1 - d, 0, d, d, 270, 90);
                path.AddArc(Width - 1 - d, Height - 1 - d, d, d, 0, 90);
                path.AddArc(0, Height - 1 - d, d, d, 90, 90);
                path.CloseFigure();
                using (var navy = new SolidBrush(Theme.Navy)) g.FillPath(navy, path);
            }
            using (var amber = new SolidBrush(Theme.Amber)) g.FillEllipse(amber, 9, 9, 10, 10);
        }
    }
}