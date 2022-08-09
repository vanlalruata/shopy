using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    public class MaterialListView : ListView, IMaterialControl
    {
        private const int ITEM_PADDING = 12;

        [Browsable(false)]
        public int Depth
        {
            get;
            set;
        }

        [Browsable(false)]
        private ListViewItem HoveredItem
        {
            get;
            set;
        }

        [Browsable(false)]
        public Point MouseLocation
        {
            get;
            set;
        }

        [Browsable(false)]
        public MouseState MouseState
        {
            get;
            set;
        }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get
            {
                return MaterialSkinManager.Instance;
            }
        }

        public MaterialListView()
        {
            base.GridLines = false;
            base.FullRowSelect = true;
            base.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            base.View = View.Details;
            base.OwnerDraw = true;
            base.ResizeRedraw = true;
            base.BorderStyle = BorderStyle.None;
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            this.MouseLocation = new Point(-1, -1);
            this.MouseState = MouseState.OUT;
            base.MouseEnter += new EventHandler((object argument0, EventArgs argument1) => this.MouseState = MouseState.HOVER);
            base.MouseLeave += new EventHandler((object argument2, EventArgs argument3) => {
                this.MouseState = MouseState.OUT;
                this.MouseLocation = new Point(-1, -1);
                this.HoveredItem = null;
                base.Invalidate();
            });
            base.MouseDown += new MouseEventHandler((object argument4, MouseEventArgs argument5) => this.MouseState = MouseState.DOWN);
            base.MouseUp += new MouseEventHandler((object argument6, MouseEventArgs argument7) => this.MouseState = MouseState.HOVER);
            base.MouseMove += new MouseEventHandler((object sender, MouseEventArgs args) => {
                this.MouseLocation = args.Location;
                ListViewItem itemAt = base.GetItemAt(this.MouseLocation.X, this.MouseLocation.Y);
                if (this.HoveredItem != itemAt)
                {
                    this.HoveredItem = itemAt;
                    base.Invalidate();
                }
            });
        }

        private StringFormat getStringFormat()
        {
            return new StringFormat()
            {
                FormatFlags = StringFormatFlags.LineLimit,
                Trimming = StringTrimming.EllipsisCharacter,
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Font font = new Font(this.SkinManager.ROBOTO_MEDIUM_12.FontFamily, 24f);
            MaterialListView.LogFont logFont = new MaterialListView.LogFont();
            font.ToLogFont(logFont);
            try
            {
                this.Font = Font.FromLogFont(logFont);
            }
            catch (ArgumentException)
            {
                this.Font = new Font(FontFamily.GenericSansSerif, 24f);
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            Graphics graphics = e.Graphics;
            SolidBrush solidBrush = new SolidBrush(this.SkinManager.GetApplicationBackgroundColor());
            int x = e.Bounds.X;
            int y = e.Bounds.Y;
            int width = base.Width;
            Rectangle bounds = e.Bounds;
            graphics.FillRectangle(solidBrush, new Rectangle(x, y, width, bounds.Height));
            Graphics graphic = e.Graphics;
            string text = e.Header.Text;
            Font rOBOTOMEDIUM10 = this.SkinManager.ROBOTO_MEDIUM_10;
            Brush secondaryTextBrush = this.SkinManager.GetSecondaryTextBrush();
            bounds = e.Bounds;
            int num = bounds.X + 12;
            bounds = e.Bounds;
            int y1 = bounds.Y + 12;
            bounds = e.Bounds;
            int width1 = bounds.Width - 24;
            bounds = e.Bounds;
            graphic.DrawString(text, rOBOTOMEDIUM10, secondaryTextBrush, new Rectangle(num, y1, width1, bounds.Height - 24), this.getStringFormat());
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            Rectangle bounds = e.Item.Bounds;
            int width = bounds.Width;
            bounds = e.Item.Bounds;
            Bitmap bitmap = new Bitmap(width, bounds.Height);
            Graphics graphic = Graphics.FromImage(bitmap);
            SolidBrush solidBrush = new SolidBrush(this.SkinManager.GetApplicationBackgroundColor());
            bounds = e.Bounds;
            Point point = new Point(bounds.X, 0);
            bounds = e.Bounds;
            graphic.FillRectangle(solidBrush, new Rectangle(point, bounds.Size));
            if (e.State.HasFlag(ListViewItemStates.Selected))
            {
                Brush flatButtonPressedBackgroundBrush = this.SkinManager.GetFlatButtonPressedBackgroundBrush();
                bounds = e.Bounds;
                Point point1 = new Point(bounds.X, 0);
                bounds = e.Bounds;
                graphic.FillRectangle(flatButtonPressedBackgroundBrush, new Rectangle(point1, bounds.Size));
            }
            else if ((!e.Bounds.Contains(this.MouseLocation) ? false : this.MouseState == MouseState.HOVER))
            {
                Brush flatButtonHoverBackgroundBrush = this.SkinManager.GetFlatButtonHoverBackgroundBrush();
                bounds = e.Bounds;
                Point point2 = new Point(bounds.X, 0);
                bounds = e.Bounds;
                graphic.FillRectangle(flatButtonHoverBackgroundBrush, new Rectangle(point2, bounds.Size));
            }
            Pen pen = new Pen(this.SkinManager.GetDividersColor());
            int left = e.Bounds.Left;
            bounds = e.Bounds;
            graphic.DrawLine(pen, left, 0, bounds.Right, 0);
            foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
            {
                string text = subItem.Text;
                Font rOBOTOMEDIUM10 = this.SkinManager.ROBOTO_MEDIUM_10;
                Brush primaryTextBrush = this.SkinManager.GetPrimaryTextBrush();
                bounds = subItem.Bounds;
                int x = bounds.X + 12;
                bounds = subItem.Bounds;
                int num = bounds.Width - 24;
                bounds = subItem.Bounds;
                graphic.DrawString(text, rOBOTOMEDIUM10, primaryTextBrush, new Rectangle(x, 12, num, bounds.Height - 24), this.getStringFormat());
            }
            Graphics graphics = e.Graphics;
            Image image = (Image)bitmap.Clone();
            bounds = e.Item.Bounds;
            Point location = bounds.Location;
            graphics.DrawImage(image, new Point(0, location.Y));
            graphic.Dispose();
            bitmap.Dispose();
        }

        public class LogFont
        {
            public int lfHeight = 0;

            public int lfWidth = 0;

            public int lfEscapement = 0;

            public int lfOrientation = 0;

            public int lfWeight = 0;

            public byte lfItalic = 0;

            public byte lfUnderline = 0;

            public byte lfStrikeOut = 0;

            public byte lfCharSet = 0;

            public byte lfOutPrecision = 0;

            public byte lfClipPrecision = 0;

            public byte lfQuality = 0;

            public byte lfPitchAndFamily = 0;

            public string lfFaceName = string.Empty;

            public LogFont()
            {
            }
        }
    }
}
