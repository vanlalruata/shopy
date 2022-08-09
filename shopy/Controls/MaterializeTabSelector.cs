using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    public class MaterialTabSelector : Control, IMaterialControl
    {
        private MaterialTabControl _baseTabControl;

        private int _previousSelectedTabIndex;

        private Point _animationSource;

        private readonly AnimationManager _animationManager;

        private List<Rectangle> _tabRects;

        private const int TAB_HEADER_PADDING = 24;

        private const int TAB_INDICATOR_HEIGHT = 2;

        public MaterialTabControl BaseTabControl
        {
            get
            {
                return this._baseTabControl;
            }
            set
            {
                this._baseTabControl = value;
                if (this._baseTabControl != null)
                {
                    this._previousSelectedTabIndex = this._baseTabControl.SelectedIndex;
                    this._baseTabControl.Deselected += new TabControlEventHandler((object sender, TabControlEventArgs args) => this._previousSelectedTabIndex = this._baseTabControl.SelectedIndex);
                    this._baseTabControl.SelectedIndexChanged += new EventHandler((object sender, EventArgs args) => {
                        this._animationManager.SetProgress(0);
                        this._animationManager.StartNewAnimation(AnimationDirection.In, null);
                    });
                    this._baseTabControl.ControlAdded += new ControlEventHandler((object argument0, ControlEventArgs argument1) => base.Invalidate());
                    this._baseTabControl.ControlRemoved += new ControlEventHandler((object argument2, ControlEventArgs argument3) => base.Invalidate());
                }
            }
        }

        [Browsable(false)]
        public int Depth
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

        public MaterialTabSelector()
        {
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            base.Height = 48;
            this._animationManager = new AnimationManager(true)
            {
                AnimationType = AnimationType.EaseOut,
                Increment = 0.04
            };
            this._animationManager.OnAnimationProgress += new AnimationManager.AnimationProgress((object sender) => base.Invalidate());
        }

        private int CalculateTextAlpha(int tabIndex, double animationProgress)
        {
            int num;
            int a = this.SkinManager.ACTION_BAR_TEXT.A;
            int a1 = this.SkinManager.ACTION_BAR_TEXT_SECONDARY.A;
            if ((tabIndex != this._baseTabControl.SelectedIndex ? false : !this._animationManager.IsAnimating()))
            {
                num = a;
            }
            else if ((tabIndex == this._previousSelectedTabIndex ? true : tabIndex == this._baseTabControl.SelectedIndex))
            {
                num = (tabIndex != this._previousSelectedTabIndex ? a1 + (int)((double)(a - a1) * animationProgress) : a - (int)((double)(a - a1) * animationProgress));
            }
            else
            {
                num = a1;
            }
            return num;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this._tabRects == null)
            {
                this.UpdateTabRects();
            }
            for (int i = 0; i < this._tabRects.Count; i++)
            {
                if (this._tabRects[i].Contains(e.Location))
                {
                    this._baseTabControl.SelectedIndex = i;
                }
            }
            this._animationSource = e.Location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int num;
            Graphics graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Clear(this.SkinManager.ColorScheme.PrimaryColor);
            if (this._baseTabControl != null)
            {
                if ((!this._animationManager.IsAnimating() || this._tabRects == null ? true : this._tabRects.Count != this._baseTabControl.TabCount))
                {
                    this.UpdateTabRects();
                }
                double progress = this._animationManager.GetProgress();
                if (this._animationManager.IsAnimating())
                {
                    SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)(51 - progress * 50), Color.White));
                    Rectangle item = this._tabRects[this._baseTabControl.SelectedIndex];
                    int width = (int)(progress * (double)item.Width * 1.75);
                    graphics.SetClip(this._tabRects[this._baseTabControl.SelectedIndex]);
                    graphics.FillEllipse(solidBrush, new Rectangle(this._animationSource.X - width / 2, this._animationSource.Y - width / 2, width, width));
                    graphics.ResetClip();
                    solidBrush.Dispose();
                }
                foreach (TabPage tabPage in this._baseTabControl.TabPages)
                {
                    int num1 = this._baseTabControl.TabPages.IndexOf(tabPage);
                    Brush brush = new SolidBrush(Color.FromArgb(this.CalculateTextAlpha(num1, progress), this.SkinManager.ColorScheme.TextColor));
                    graphics.DrawString(tabPage.Text.ToUpper(), this.SkinManager.ROBOTO_MEDIUM_10, brush, this._tabRects[num1], new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
                    brush.Dispose();
                }
                num = (this._previousSelectedTabIndex == -1 ? this._baseTabControl.SelectedIndex : this._previousSelectedTabIndex);
                Rectangle rectangle = this._tabRects[num];
                Rectangle item1 = this._tabRects[this._baseTabControl.SelectedIndex];
                int bottom = item1.Bottom - 2;
                int x = rectangle.X + (int)((double)(item1.X - rectangle.X) * progress);
                int width1 = rectangle.Width + (int)((double)(item1.Width - rectangle.Width) * progress);
                graphics.FillRectangle(this.SkinManager.ColorScheme.AccentBrush, x, bottom, width1, 2);
            }
        }

        private void UpdateTabRects()
        {
            this._tabRects = new List<Rectangle>();
            if ((this._baseTabControl == null ? false : this._baseTabControl.TabCount != 0))
            {
                using (Bitmap bitmap = new Bitmap(1, 1))
                {
                    using (Graphics graphic = Graphics.FromImage(bitmap))
                    {
                        List<Rectangle> rectangles = this._tabRects;
                        int fORMPADDING = this.SkinManager.FORM_PADDING;
                        SizeF sizeF = graphic.MeasureString(this._baseTabControl.TabPages[0].Text, this.SkinManager.ROBOTO_MEDIUM_10);
                        rectangles.Add(new Rectangle(fORMPADDING, 0, 48 + (int)sizeF.Width, base.Height));
                        for (int i = 1; i < this._baseTabControl.TabPages.Count; i++)
                        {
                            List<Rectangle> rectangles1 = this._tabRects;
                            int right = this._tabRects[i - 1].Right;
                            sizeF = graphic.MeasureString(this._baseTabControl.TabPages[i].Text, this.SkinManager.ROBOTO_MEDIUM_10);
                            rectangles1.Add(new Rectangle(right, 0, 48 + (int)sizeF.Width, base.Height));
                        }
                    }
                }
            }
        }
    }
}
