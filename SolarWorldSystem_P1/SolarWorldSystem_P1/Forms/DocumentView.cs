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
    /// The right side of the inbox: shows one requisition as a clean, printable-style document
    /// (header band, items with Project / For Sale tags, shortage calculation, linked purchase orders, activity log).
    /// Call ShowDocument(...) to draw a requisition, or ShowMessage(...) when there is nothing to show.
    /// It only DISPLAYS data; the action buttons (approve, reject, ...) come in a later part.
    /// </summary>
    public class DocumentView : Panel
    {
        private readonly PaperControl _paper = new PaperControl();
        private readonly Label _message = new Label();

        public DocumentView()
        {
            AutoScroll = true;
            BackColor = Theme.Canvas;
            Padding = new Padding(0, 0, 0, 28);
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;

            _message.Dock = DockStyle.Fill;
            _message.TextAlign = ContentAlignment.TopLeft;
            _message.Padding = new Padding(42, 40, 42, 40);
            _message.ForeColor = Theme.Muted;
            _message.Font = Theme.Body;

            _paper.Visible = false;
            _paper.MouseEnter += (s, e) => Focus();   // so the mouse wheel scrolls the document

            Controls.Add(_paper);
            Controls.Add(_message);
            Resize += (s, e) => Arrange();
        }

        /// <summary>Shows plain text instead of a document (for example "Select a task").</summary>
        public void ShowMessage(string text)
        {
            _paper.Visible = false;
            _message.Text = text;
            _message.Visible = true;
            AutoScrollPosition = new Point(0, 0);
        }

        /// <summary>Draws a requisition document.</summary>
        public void ShowDocument(RfDocument doc)
        {
            _message.Visible = false;
            _paper.SetDocument(doc);
            _paper.Visible = true;
            Arrange();
            AutoScrollPosition = new Point(0, 0);
        }

        // Centers the paper and gives it a comfortable width.
        private void Arrange()
        {
            if (!_paper.Visible) return;
            int w = Math.Min(820, Math.Max(520, ClientSize.Width - 64));
            int left = Math.Max(24, (ClientSize.Width - w) / 2);
            _paper.SetBounds(left, 28, w, _paper.Height);
            _paper.Recalculate();
        }
    }

    /// <summary>The white "sheet of paper". It draws everything itself and works out its own height.</summary>
    internal class PaperControl : Control
    {
        private const int Pad = 40;
        private static readonly Font BigFont = new Font("Segoe UI", 20f, FontStyle.Bold);
        private static readonly Font SmallBold = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        private static readonly Color OldDot = ColorTranslator.FromHtml("#BDB8AC");
        private RfDocument _doc;

        public PaperControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        public void SetDocument(RfDocument doc) { _doc = doc; }

        /// <summary>Measures the content to set the height, then redraws.</summary>
        public void Recalculate()
        {
            if (_doc == null) return;
            using (var bmp = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(bmp))
                Height = Render(g, false);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_doc != null) Render(e.Graphics, true);
        }

        // One method both measures (draw = false) and paints (draw = true), so the two always agree.
        private int Render(Graphics g, bool draw)
        {
            int W = Width, iw = W - Pad * 2, y;
            if (draw)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Theme.Canvas);
                using (var path = UiDraw.Round(0, 0, W - 1, Height - 1, 8))
                using (var white = new SolidBrush(Color.White))
                using (var border = new Pen(Theme.Hairline))
                using (var band = new SolidBrush(Theme.NavyTint))
                {
                    g.FillPath(white, path);
                    g.SetClip(path);
                    g.FillRectangle(band, 0, 0, W, 92);
                    g.ResetClip();
                    g.DrawPath(border, path);
                }
            }

            // ----- Header -----
            Txt(g, draw, "REQUISITION FORM", Theme.Small, Theme.Muted, Pad, 20, iw / 2);
            Txt(g, draw, _doc.RfNumber, BigFont, Theme.Ink, Pad, 38, iw);
            string statusText, statusKind;
            RfStatus(_doc.Status, out statusText, out statusKind);
            Chip(g, draw, statusText, W - Pad, 24, statusKind, true);

            // ----- Details -----
            y = 112;
            string[] labels = { "Requested by", "Project", "Created", "Stock rule" };
            string[] values =
            {
                _doc.RequestedBy, _doc.ProjectName, _doc.DateCreated.ToString("d MMM yyyy"),
                _doc.StockTag == "ForSale" ? "For-sale stock only" : "Project stock only"
            };
            for (int i = 0; i < 4; i++)
            {
                int x = Pad + i * (iw / 4);
                Txt(g, draw, labels[i], Theme.Small, Theme.Muted, x, y, iw / 4 - 8);
                Txt(g, draw, values[i], Theme.Body, Theme.Ink, x, y + 18, iw / 4 - 8);
            }
            y += 58;
            Line(g, draw, y);
            y += 12;

            // ----- Items table -----
            int xTag = Pad + (int)(iw * 0.46), rReq = Pad + (int)(iw * 0.68), rRes = Pad + (int)(iw * 0.84), rShort = Pad + iw;
            Txt(g, draw, "ITEM", Theme.Small, Theme.Muted, Pad, y, 200);
            Txt(g, draw, "STOCK TAG", Theme.Small, Theme.Muted, xTag, y, 100);
            RightText(g, draw, "REQUESTED", Theme.Small, Theme.Muted, rReq, y);
            RightText(g, draw, "RESERVED", Theme.Small, Theme.Muted, rRes, y);
            RightText(g, draw, "SHORTAGE", Theme.Small, Theme.Muted, rShort, y);
            y += 24;

            if (_doc.Lines.Count == 0)
            {
                Line(g, draw, y);
                Txt(g, draw, "This requisition has no items.", Theme.Body, Theme.Muted, Pad, y + 10, iw);
                y += 40;
            }
            foreach (var l in _doc.Lines)
            {
                Line(g, draw, y);
                Txt(g, draw, l.ItemName, Theme.Body, Theme.Ink, Pad, y + 10, xTag - Pad - 12);
                Chip(g, draw, l.StockTag == "ForSale" ? "For Sale" : "Project", xTag, y + 8, l.StockTag == "ForSale" ? "sale" : "proj", false);
                RightText(g, draw, l.Requested.ToString(), Theme.Body, Theme.Ink, rReq, y + 10);
                RightText(g, draw, l.Reserved.ToString(), Theme.Body, Theme.Ink, rRes, y + 10);
                RightText(g, draw, l.Shortage.ToString(), l.Shortage > 0 ? Theme.Bold : Theme.Body, l.Shortage > 0 ? Theme.Red : Theme.Ink, rShort, y + 10);
                y += 38;
            }
            Line(g, draw, y);
            y += 18;

            // ----- Shortage calculation -----
            var short2 = _doc.Lines.Where(l => l.Shortage > 0).ToList();
            if (short2.Count > 0)
            {
                int shown = Math.Min(short2.Count, 4);
                int boxH = 16 + 24 + shown * 24 + (short2.Count > shown ? 22 : 0) + 8;
                Box(g, draw, Pad, y, iw, boxH, Theme.RedTint);
                Txt(g, draw, "Shortage calculation", SmallBold, Theme.Red, Pad + 16, y + 14, iw - 32);
                for (int i = 0; i < shown; i++)
                {
                    var l = short2[i];
                    Txt(g, draw, l.ItemName + ":  " + l.Requested + " requested \u2212 " + l.Reserved + " reserved = " + l.Shortage + " short",
                         Theme.Body, Theme.Ink, Pad + 16, y + 38 + i * 24, iw - 32);
                }
                if (short2.Count > shown)
                    Txt(g, draw, "+ " + (short2.Count - shown) + " more item(s) short", Theme.Small, Theme.Muted, Pad + 16, y + 38 + shown * 24, iw - 32);
                y += boxH;
            }
            else
            {
                Box(g, draw, Pad, y, iw, 48, Theme.GreenTint);
                Txt(g, draw, "All items are reserved. No shortage.", Theme.Bold, Theme.Green, Pad + 16, y + 15, iw - 32);
                y += 48;
            }
            y += 26;

            // ----- Linked purchase orders -----
            Txt(g, draw, "Linked purchase orders", Theme.Bold, Theme.Ink, Pad, y, iw);
            y += 26;
            if (_doc.Orders.Count == 0)
            {
                Txt(g, draw, "No purchase order has been created for this requisition.", Theme.Body, Theme.Muted, Pad, y, iw);
                y += 30;
            }
            foreach (var o in _doc.Orders)
            {
                Box(g, draw, Pad, y, iw, 66, Color.White, true);
                Txt(g, draw, o.PoNumber + "  \u00b7  " + o.SupplierName, Theme.Bold, Theme.Ink, Pad + 16, y + 12, iw - 260);
                string detail = "Ordered " + o.Ordered + "  \u00b7  received " + o.Received + "  \u00b7  outstanding " + o.Outstanding;
                if (o.TotalCost.HasValue) detail += "  \u00b7  PHP " + o.TotalCost.Value.ToString("N2");
                Txt(g, draw, detail, Theme.Small, Theme.Muted, Pad + 16, y + 38, iw - 260);
                string poText, poKind;
                PoStatus(o, out poText, out poKind);
                Chip(g, draw, poText, Pad + iw - 14, y + 10, poKind, true);
                Chip(g, draw, o.PaymentText, Pad + iw - 14, y + 36, "neu", true);
                y += 76;
            }
            y += 12;

            // ----- Activity -----
            Txt(g, draw, "Activity", Theme.Bold, Theme.Ink, Pad, y, iw);
            y += 28;
            for (int i = 0; i < _doc.Events.Count; i++)
            {
                var ev = _doc.Events[i];
                bool last = i == _doc.Events.Count - 1;
                if (draw)
                {
                    if (!last) using (var p = new Pen(Theme.Hairline)) g.DrawLine(p, Pad + 5, y + 14, Pad + 5, y + 36);
                    using (var b = new SolidBrush(last ? Theme.Navy : OldDot)) g.FillEllipse(b, Pad + 1.5f, y + 5.5f, 7, 7);
                }
                Txt(g, draw, ev.When.ToString("dd MMM HH:mm"), Theme.Small, Theme.Muted, Pad + 20, y + 2, 100);
                Txt(g, draw, ev.Actor, SmallBold, Theme.Ink, Pad + 124, y + 2, 120);
                Txt(g, draw, ev.Text, Theme.Body, Theme.Ink, Pad + 250, y, iw - 250);
                y += 28;
            }
            return y + Pad;
        }

        // ----- Small drawing helpers (they only draw when draw = true) -----

        private static void Txt(Graphics g, bool draw, string text, Font font, Color color, int x, int y, int w)
        {
            if (draw)
                TextRenderer.DrawText(g, text ?? "", font, new Rectangle(x, y, Math.Max(10, w), font.Height + 6), color,
                                      TextFormatFlags.Left | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        private static void RightText(Graphics g, bool draw, string text, Font font, Color color, int right, int y)
        {
            if (draw)
                TextRenderer.DrawText(g, text, font, new Rectangle(right - 90, y, 90, font.Height + 6), color,
                                      TextFormatFlags.Right | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
        }

        private void Line(Graphics g, bool draw, int y)
        {
            if (draw) using (var p = new Pen(Theme.Hairline)) g.DrawLine(p, Pad, y, Width - Pad, y);
        }

        private static void Box(Graphics g, bool draw, int x, int y, int w, int h, Color fill, bool border = false)
        {
            if (!draw) return;
            using (var path = UiDraw.Round(x, y, w, h, 8))
            using (var b = new SolidBrush(fill))
            {
                g.FillPath(b, path);
                if (border) using (var p = new Pen(Theme.Hairline)) g.DrawPath(p, path);
            }
        }

        // A small rounded label. Returns its width.
        private static int Chip(Graphics g, bool draw, string text, int x, int y, string kind, bool rightAlign)
        {
            Color fg, bg;
            switch (kind)
            {
                case "proj": fg = Theme.Navy; bg = Theme.NavyTint; break;
                case "sale": fg = Theme.AmberText; bg = Theme.AmberTint; break;
                case "ok": fg = Theme.Green; bg = Theme.GreenTint; break;
                case "warn": fg = Theme.AmberText; bg = Theme.AmberTint; break;
                case "bad": fg = Theme.Red; bg = Theme.RedTint; break;
                default: fg = Theme.Muted; bg = Theme.Hairline; break;
            }
            int w = TextRenderer.MeasureText(text, SmallBold, new Size(1000, 0), TextFormatFlags.NoPadding).Width + 20;
            int left = rightAlign ? x - w : x;
            if (draw)
            {
                using (var path = UiDraw.Round(left, y, w, 22, 11))
                using (var b = new SolidBrush(bg))
                    g.FillPath(b, path);
                TextRenderer.DrawText(g, text, SmallBold, new Rectangle(left, y, w, 22), fg,
                                      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
            }
            return w;
        }

        private static void RfStatus(string status, out string text, out string kind)
        {
            switch (status)
            {
                case "Submitted": text = "Awaiting owner approval"; kind = "warn"; break;
                case "Approved": text = "Approved"; kind = "ok"; break;
                case "PendingFulfillment": text = "Pending fulfillment"; kind = "neu"; break;
                case "Fulfilled": text = "Fulfilled"; kind = "ok"; break;
                case "ReturnedForRevision": text = "Returned for revision"; kind = "warn"; break;
                case "Cancelled": text = "Cancelled"; kind = "bad"; break;
                default: text = status; kind = "neu"; break;
            }
        }

        private static void PoStatus(DocOrder o, out string text, out string kind)
        {
            switch (o.Status)
            {
                case "Completed": text = "Closed \u00b7 fully received"; kind = "ok"; break;
                case "PartiallyReceived": text = "Partly received \u00b7 " + o.Outstanding + " outstanding"; kind = "warn"; break;
                case "Ordered": text = "Ordered \u00b7 " + o.Outstanding + " outstanding"; kind = "warn"; break;
                case "PendingApproval": text = "Awaiting owner approval"; kind = "warn"; break;
                case "Cancelled": text = "Cancelled"; kind = "bad"; break;
                default: text = "Draft"; kind = "neu"; break;
            }
        }
    }
}