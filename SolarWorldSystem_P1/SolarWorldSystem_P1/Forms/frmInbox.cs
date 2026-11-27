using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SolarWorldERP.Helpers;
using SolarWorldERP.Services;

namespace SolarWorldERP.Forms
{
    /// <summary>
    /// The Action Inbox: a list of tasks on the left (grouped by status) and a workspace on the right.
    /// It is built in code (no designer file). The tasks come from InboxService.
    /// The right side only shows a short summary for now; the real document view is built in Part 4.
    /// </summary>
    public class frmInbox : Form
    {
        private readonly InboxService _service = new InboxService();
        private readonly FlowLayoutPanel _list = new FlowLayoutPanel();
        private readonly DocumentService _documents = new DocumentService();
        private readonly DocumentView _detail = new DocumentView();
        private readonly Label _count = new Label();
        private readonly Label _warning = new Label();
        private string _selectedKey;   // "RF:12", so the same task stays selected after a refresh

        public frmInbox()
        {
            Text = "Inbox";
            BackColor = Theme.Canvas;
            Font = Theme.Body;
            BuildLayout();
            Load += (s, e) => Reload();
        }

        // ----- Layout -----

        private void BuildLayout()
        {
            var left = new Panel { Dock = DockStyle.Left, Width = 400, BackColor = Theme.LeftPane };
            var line = new Panel { Dock = DockStyle.Left, Width = 1, BackColor = Theme.Hairline };
            _detail.Dock = DockStyle.Fill;
            _detail.BackColor = Theme.Canvas;

            // Fill goes in first, then the edge controls
            Controls.Add(_detail);
            Controls.Add(line);
            Controls.Add(left);

            // The scrolling list of groups and rows
            _list.Dock = DockStyle.Fill;
            _list.FlowDirection = FlowDirection.TopDown;
            _list.WrapContents = false;
            _list.AutoScroll = true;
            _list.BackColor = Theme.LeftPane;
            _list.Padding = new Padding(0);
            _list.Resize += (s, e) => ResizeRows();

            // A strip that appears only if some source could not be read
            _warning.Dock = DockStyle.Top;
            _warning.Height = 56;
            _warning.AutoSize = false;
            _warning.Visible = false;
            _warning.BackColor = Theme.AmberTint;
            _warning.ForeColor = Theme.AmberText;
            _warning.Font = Theme.Small;
            _warning.Padding = new Padding(24, 8, 24, 8);

            // Header: title, "+ New request" (staff), count, Refresh
            var header = new Panel { Dock = DockStyle.Top, Size = new Size(400, 84), BackColor = Theme.LeftPane };   // width = the left panel, so right-anchored controls land correctly
            header.Controls.Add(new Label { Text = "Inbox", Font = new Font("Segoe UI", 13f, FontStyle.Bold), ForeColor = Theme.Ink, AutoSize = true, Location = new Point(24, 16) });

            _count.Font = Theme.Small;
            _count.ForeColor = Theme.Muted;
            _count.AutoSize = true;
            _count.Location = new Point(24, 56);
            header.Controls.Add(_count);

            var refresh = new Label { Text = "Refresh", Font = Theme.Small, ForeColor = Theme.Navy, AutoSize = true, Cursor = Cursors.Hand, Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(400 - 24 - 44, 56) };
            refresh.Click += (s, e) => Reload();
            header.Controls.Add(refresh);

            if (Permissions.CanCreateRequisition)
            {
                var btn = new Button { Text = "+ New request", Size = new Size(130, 30), Location = new Point(400 - 24 - 130, 14), Anchor = AnchorStyles.Top | AnchorStyles.Right };
                Theme.StylePrimaryButton(btn);
                btn.Click += (s, e) => NewRequest();
                header.Controls.Add(btn);
            }

            // Fill first, then the top controls (the last one added ends up highest)
            left.Controls.Add(_list);
            left.Controls.Add(_warning);
            left.Controls.Add(header);
        }

        // ----- Loading and showing the tasks -----

        private void Reload()
        {
            InboxResult result;
            Cursor = Cursors.WaitCursor;
            try { result = _service.GetInbox(); }
            catch (Exception ex)
            {
                result = new InboxResult();
                result.Warnings.Add(ex.Message);
            }
            finally { Cursor = Cursors.Default; }
            Populate(result);
        }

        private void Populate(InboxResult result)
        {
            _list.SuspendLayout();
            foreach (Control c in _list.Controls.Cast<Control>().ToList()) c.Dispose();

            _count.Text = result.Items.Count + " open";
            _warning.Visible = result.Warnings.Count > 0;
            if (_warning.Visible) _warning.Text = "Some tasks could not be loaded: " + string.Join("; ", result.Warnings);

            InboxItem reselect = null;
            foreach (InboxGroup group in Enum.GetValues(typeof(InboxGroup)))
            {
                var items = result.Items.Where(i => i.Group == group).ToList();
                if (items.Count == 0) continue;

                _list.Controls.Add(new GroupHeader(GroupName(group), items.Count));
                foreach (var item in items)
                {
                    var row = new InboxRow(item);
                    row.RowClicked += OnRowClicked;
                    if (Key(item) == _selectedKey) { row.IsSelected = true; reselect = item; }
                    _list.Controls.Add(row);
                }
            }

            if (result.Items.Count == 0)
                _list.Controls.Add(new Label { Text = "You're all caught up.", ForeColor = Theme.Muted, Font = Theme.Body, Height = 60, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(0) });

            ResizeRows();
            _list.ResumeLayout();

            if (reselect == null) _selectedKey = null;
            ShowDetail(reselect);
        }

        private void ResizeRows()
        {
            int w = _list.ClientSize.Width - _list.Padding.Horizontal;
            foreach (Control c in _list.Controls) c.Width = w;
        }

        private void OnRowClicked(object sender, EventArgs e)
        {
            var clicked = (InboxRow)sender;
            foreach (Control c in _list.Controls)
            {
                var row = c as InboxRow;
                if (row != null) row.IsSelected = (row == clicked);
            }
            _selectedKey = Key(clicked.Item);
            ShowDetail(clicked.Item);
        }

        private static string Key(InboxItem item) { return item.Kind + ":" + item.Id; }

        private static string GroupName(InboxGroup g)
        {
            switch (g)
            {
                case InboxGroup.OwnerApprovals: return "Owner Approvals";
                case InboxGroup.PendingDeliveries: return "Pending Deliveries";
                case InboxGroup.ReadyForIssuance: return "Ready for Issuance";
                default: return "Low-Stock Replenishment";
            }
        }

        // ----- The right side: the document for the selected task -----

        private void ShowDetail(InboxItem item)
        {
            if (item == null) { _detail.ShowMessage("Select a task from the inbox."); return; }
            try
            {
                if (item.Kind == "Item")
                {
                    _detail.ShowMessage(item.Title + "\r\n" + item.Subtitle + "\r\n\r\nUse Inventory to check this stock, or Purchasing to prepare an order.");
                    return;
                }
                int? rfId = _documents.FindRfId(item.Kind, item.Id);
                if (!rfId.HasValue)
                {
                    _detail.ShowMessage("This purchase order is not linked to a requisition.");
                    return;
                }
                _detail.ShowDocument(_documents.GetRequisitionDocument(rfId.Value));
            }
            catch (Exception ex)
            {
                _detail.ShowMessage("Could not open this task.\r\n" + ex.Message);
            }
        }

        // ----- Actions -----

        /// <summary>Opens the existing requisition form as a dialog, then refreshes the inbox.</summary>
        private void NewRequest()
        {
            using (var form = new frmRequisition())
                form.ShowDialog(this);
            Reload();
        }
    }

    /// <summary>One task in the inbox: a status dot, a title, a subtitle and its age.</summary>
    internal class InboxRow : Control
    {
        private bool _selected, _hover;

        public InboxItem Item { get; private set; }
        public event EventHandler RowClicked;

        public InboxRow(InboxItem item)
        {
            Item = item;
            Height = 58;
            Margin = new Padding(0);
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        public bool IsSelected { get { return _selected; } set { _selected = value; Invalidate(); } }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnClick(EventArgs e) { base.OnClick(e); if (RowClicked != null) RowClicked(this, e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Theme.LeftPane);

            if (_selected || _hover)
            {
                using (var path = UiDraw.Round(8, 2, Width - 16, Height - 4, 6))
                using (var fill = new SolidBrush(_selected ? Theme.NavyTint : Theme.Hairline))
                    g.FillPath(fill, path);
            }
            if (_selected)
                using (var bar = new SolidBrush(Theme.Navy)) g.FillRectangle(bar, 8, 12, 3, Height - 24);

            // Status dot: amber = needs attention, green = ready, hollow = in progress
            var rect = new RectangleF(24, 17, 8, 8);
            switch (Item.Group)
            {
                case InboxGroup.ReadyForIssuance:
                    using (var b = new SolidBrush(Theme.Green)) g.FillEllipse(b, rect);
                    break;
                case InboxGroup.PendingDeliveries:
                    using (var p = new Pen(Theme.Muted, 1.5f)) g.DrawEllipse(p, 25, 18, 6, 6);
                    break;
                default:
                    using (var b = new SolidBrush(Theme.Amber)) g.FillEllipse(b, rect);
                    break;
            }

            var flags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.Left;
            TextRenderer.DrawText(g, Item.Title, Theme.Bold, new Rectangle(44, 8, Width - 44 - 64, 20), Theme.Ink, flags);
            TextRenderer.DrawText(g, Item.Subtitle, Theme.Small, new Rectangle(44, 30, Width - 44 - 24, 18), Theme.Muted, flags);
            TextRenderer.DrawText(g, Item.Age, Theme.Small, new Rectangle(Width - 64, 8, 48, 20), Theme.Muted,
                                  TextFormatFlags.Right | TextFormatFlags.NoPrefix);
        }
    }

    /// <summary>A group title in the inbox, with the number of tasks in it.</summary>
    internal class GroupHeader : Control
    {
        private readonly string _title;
        private readonly int _count;

        public GroupHeader(string title, int count)
        {
            _title = title;
            _count = count;
            Height = 36;
            Margin = new Padding(0);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Theme.LeftPane);
            TextRenderer.DrawText(g, _title, Theme.Small, new Rectangle(24, 12, Width - 80, 18), Theme.Muted, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

            // Count pill on the right
            using (var path = UiDraw.Round(Width - 24 - 24, 9, 24, 18, 9))
            using (var fill = new SolidBrush(Theme.Hairline))
                g.FillPath(fill, path);
            TextRenderer.DrawText(g, _count.ToString(), Theme.Small, new Rectangle(Width - 48, 9, 24, 18), Theme.Muted,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }

    /// <summary>A tiny drawing helper shared by the inbox controls.</summary>
    internal static class UiDraw
    {
        public static GraphicsPath Round(float x, float y, float w, float h, float r)
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
}