using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    internal class MaterialToolStripRender : ToolStripProfessionalRenderer, IMaterialControl
    {
        public int Depth
        {
            get;
            set;
        }

        public MouseState MouseState
        {
            get;
            set;
        }

        public MaterialSkinManager SkinManager
        {
            get
            {
                return MaterialSkinManager.Instance;
            }
        }

        public MaterialToolStripRender()
        {
        }

        private Rectangle GetItemRect(ToolStripItem item)
        {
            int y = item.ContentRectangle.Y;
            Rectangle contentRectangle = item.ContentRectangle;
            int width = contentRectangle.Width + 4;
            contentRectangle = item.ContentRectangle;
            return new Rectangle(0, y, width, contentRectangle.Height);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            int x = e.ArrowRectangle.X;
            Rectangle arrowRectangle = e.ArrowRectangle;
            int width = x + arrowRectangle.Width / 2;
            int y = e.ArrowRectangle.Y;
            arrowRectangle = e.ArrowRectangle;
            Point point = new Point(width, y + arrowRectangle.Height / 2);
            Brush brush = (e.Item.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush());
            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddLines(new Point[] { new Point(point.X - 4, point.Y - 4), new Point(point.X, point.Y), new Point(point.X - 4, point.Y + 4) });
                graphicsPath.CloseFigure();
                graphics.FillPath(brush, graphicsPath);
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            Rectangle itemRect = this.GetItemRect(e.Item);
            Rectangle rectangle = new Rectangle(24, itemRect.Y, itemRect.Width - 40, itemRect.Height);
            graphics.DrawString(e.Text, this.SkinManager.ROBOTO_MEDIUM_10, (e.Item.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush()), rectangle, new StringFormat()
            {
                LineAlignment = StringAlignment.Center
            });
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Brush solidBrush;
            Graphics graphics = e.Graphics;
            graphics.Clear(this.SkinManager.GetApplicationBackgroundColor());
            Rectangle itemRect = this.GetItemRect(e.Item);
            Graphics graphic = graphics;
            if (!e.Item.Selected || !e.Item.Enabled)
            {
                solidBrush = new SolidBrush(this.SkinManager.GetApplicationBackgroundColor());
            }
            else
            {
                solidBrush = this.SkinManager.GetCmsSelectedItemBrush();
            }
            graphic.FillRectangle(solidBrush, itemRect);
            MaterialContextMenuStrip toolStrip = e.ToolStrip as MaterialContextMenuStrip;
            if (toolStrip != null)
            {
                AnimationManager animationManager = toolStrip.AnimationManager;
                Point animationSource = toolStrip.AnimationSource;
                if ((!toolStrip.AnimationManager.IsAnimating() ? false : e.Item.Bounds.Contains(animationSource)))
                {
                    for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                    {
                        double progress = animationManager.GetProgress(i);
                        SolidBrush solidBrush1 = new SolidBrush(Color.FromArgb((int)(51 - progress * 50), Color.Black));
                        int width = (int)(progress * (double)itemRect.Width * 2.5);
                        graphics.FillEllipse(solidBrush1, new Rectangle(animationSource.X - width / 2, itemRect.Y - itemRect.Height, width, itemRect.Height * 3));
                    }
                }
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.FillRectangle(new SolidBrush(this.SkinManager.GetApplicationBackgroundColor()), e.Item.Bounds);
            Pen pen = new Pen(this.SkinManager.GetDividersColor());
            Rectangle bounds = e.Item.Bounds;
            int left = bounds.Left;
            bounds = e.Item.Bounds;
            Point point = new Point(left, bounds.Height / 2);
            bounds = e.Item.Bounds;
            int right = bounds.Right;
            bounds = e.Item.Bounds;
            graphics.DrawLine(pen, point, new Point(right, bounds.Height / 2));
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen pen = new Pen(this.SkinManager.GetDividersColor());
            int x = e.AffectedBounds.X;
            int y = e.AffectedBounds.Y;
            Rectangle affectedBounds = e.AffectedBounds;
            int width = affectedBounds.Width - 1;
            affectedBounds = e.AffectedBounds;
            graphics.DrawRectangle(pen, new Rectangle(x, y, width, affectedBounds.Height - 1));
        }
    }
}
