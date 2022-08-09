using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    public class MaterialCheckBox : CheckBox, IMaterialControl
    {
        private bool _ripple;

        private readonly AnimationManager _animationManager;

        private readonly AnimationManager _rippleAnimationManager;

        private const int CHECKBOX_SIZE = 18;

        private const int CHECKBOX_SIZE_HALF = 9;

        private const int CHECKBOX_INNER_BOX_SIZE = 14;

        private int _boxOffset;

        private Rectangle _boxRectangle;

        private readonly static Point[] CheckmarkLine;

        private const int TEXT_OFFSET = 22;

        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                if (value)
                {
                    base.Size = new Size(10, 10);
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

        [Category("Behavior")]
        public bool Ripple
        {
            get
            {
                return this._ripple;
            }
            set
            {
                this._ripple = value;
                this.AutoSize = this.AutoSize;
                if (value)
                {
                    base.Margin = new Padding(0);
                }
                base.Invalidate();
            }
        }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get
            {
                return MaterialSkinManager.Instance;
            }
        }

        static MaterialCheckBox()
        {
            MaterialCheckBox.CheckmarkLine = new Point[] { new Point(3, 8), new Point(7, 12), new Point(14, 5) };
        }

        public MaterialCheckBox()
        {
            this._animationManager = new AnimationManager(true)
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.05
            };
            this._rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.1,
                SecondaryIncrement = 0.08
            };
            this._animationManager.OnAnimationProgress += new AnimationManager.AnimationProgress((object sender) => base.Invalidate());
            this._rippleAnimationManager.OnAnimationProgress += new AnimationManager.AnimationProgress((object sender) => base.Invalidate());
            base.CheckedChanged += new EventHandler((object sender, EventArgs args) => this._animationManager.StartNewAnimation((base.Checked ? AnimationDirection.In : AnimationDirection.Out), null));
            this.Ripple = true;
            this.MouseLocation = new Point(-1, -1);
        }

        private Bitmap DrawCheckMarkBitmap()
        {
            Bitmap bitmap = new Bitmap(18, 18);
            Graphics graphic = Graphics.FromImage(bitmap);
            graphic.Clear(Color.Transparent);
            using (Pen pen = new Pen(base.Parent.BackColor, 2f))
            {
                graphic.DrawLines(pen, MaterialCheckBox.CheckmarkLine);
            }
            return bitmap;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            SizeF sizeF = base.CreateGraphics().MeasureString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10);
            int width = this._boxOffset + 18 + 2 + (int)sizeF.Width;
            return (this.Ripple ? new Size(width, 30) : new Size(width, 20));
        }

        private bool IsMouseInCheckArea()
        {
            return this._boxRectangle.Contains(this.MouseLocation);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.Font = this.SkinManager.ROBOTO_MEDIUM_10;
            if (!base.DesignMode)
            {
                this.MouseState = MouseState.OUT;
                base.MouseEnter += new EventHandler((object sender, EventArgs args) => this.MouseState = MouseState.HOVER);
                base.MouseLeave += new EventHandler((object sender, EventArgs args) => {
                    this.MouseLocation = new Point(-1, -1);
                    this.MouseState = MouseState.OUT;
                });
                base.MouseDown += new MouseEventHandler((object sender, MouseEventArgs args) => {
                    this.MouseState = MouseState.DOWN;
                    if ((!this.Ripple || args.Button != MouseButtons.Left ? false : this.IsMouseInCheckArea()))
                    {
                        this._rippleAnimationManager.SecondaryIncrement = 0;
                        this._rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { base.Checked });
                    }
                });
                base.MouseUp += new MouseEventHandler((object sender, MouseEventArgs args) => {
                    this.MouseState = MouseState.HOVER;
                    this._rippleAnimationManager.SecondaryIncrement = 0.08;
                });
                base.MouseMove += new MouseEventHandler((object sender, MouseEventArgs args) => {
                    this.MouseLocation = args.Location;
                    this.Cursor = (this.IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default);
                });
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Color checkBoxOffDisabledColor;
            int a;
            int num;
            Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Clear(base.Parent.BackColor);
            int num1 = this._boxOffset + 9 - 1;
            double progress = this._animationManager.GetProgress();
            if (base.Enabled)
            {
                a = (int)(progress * 255);
            }
            else
            {
                checkBoxOffDisabledColor = this.SkinManager.GetCheckBoxOffDisabledColor();
                a = (int)checkBoxOffDisabledColor.A;
            }
            int num2 = a;
            if (base.Enabled)
            {
                checkBoxOffDisabledColor = this.SkinManager.GetCheckboxOffColor();
                num = (int)((double)checkBoxOffDisabledColor.A * (1 - progress));
            }
            else
            {
                checkBoxOffDisabledColor = this.SkinManager.GetCheckBoxOffDisabledColor();
                num = (int)checkBoxOffDisabledColor.A;
            }
            int num3 = num;
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(num2, (base.Enabled ? this.SkinManager.ColorScheme.AccentColor : this.SkinManager.GetCheckBoxOffDisabledColor())));
            SolidBrush solidBrush1 = new SolidBrush((base.Enabled ? this.SkinManager.ColorScheme.AccentColor : this.SkinManager.GetCheckBoxOffDisabledColor()));
            Pen pen = new Pen(solidBrush.Color);
            if ((!this.Ripple ? false : this._rippleAnimationManager.IsAnimating()))
            {
                for (int i = 0; i < this._rippleAnimationManager.GetAnimationCount(); i++)
                {
                    double progress1 = this._rippleAnimationManager.GetProgress(i);
                    Point point = new Point(num1, num1);
                    SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb((int)(progress1 * 40), ((bool)this._rippleAnimationManager.GetData(i)[0] ? Color.Black : solidBrush.Color)));
                    int num4 = (base.Height % 2 == 0 ? base.Height - 3 : base.Height - 2);
                    int num5 = (this._rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn ? (int)((double)num4 * (0.8 + 0.2 * progress1)) : num4);
                    using (GraphicsPath graphicsPath = DrawHelper.CreateRoundRect((float)(point.X - num5 / 2), (float)(point.Y - num5 / 2), (float)num5, (float)num5, (float)(num5 / 2)))
                    {
                        graphics.FillPath(solidBrush2, graphicsPath);
                    }
                    solidBrush2.Dispose();
                }
            }
            solidBrush1.Dispose();
            Rectangle rectangle = new Rectangle(this._boxOffset, this._boxOffset, (int)(17 * progress), 17);
            using (GraphicsPath graphicsPath1 = DrawHelper.CreateRoundRect((float)this._boxOffset, (float)this._boxOffset, 17f, 17f, 1f))
            {
                SolidBrush solidBrush3 = new SolidBrush(DrawHelper.BlendColor(base.Parent.BackColor, (base.Enabled ? this.SkinManager.GetCheckboxOffColor() : this.SkinManager.GetCheckBoxOffDisabledColor()), (double)num3));
                Pen pen1 = new Pen(solidBrush3.Color);
                graphics.FillPath(solidBrush3, graphicsPath1);
                graphics.DrawPath(pen1, graphicsPath1);
                graphics.FillRectangle(new SolidBrush(base.Parent.BackColor), this._boxOffset + 2, this._boxOffset + 2, 13, 13);
                graphics.DrawRectangle(new Pen(base.Parent.BackColor), this._boxOffset + 2, this._boxOffset + 2, 13, 13);
                solidBrush3.Dispose();
                pen1.Dispose();
                if (base.Enabled)
                {
                    graphics.FillPath(solidBrush, graphicsPath1);
                    graphics.DrawPath(pen, graphicsPath1);
                }
                else if (base.Checked)
                {
                    graphics.SmoothingMode = SmoothingMode.None;
                    graphics.FillRectangle(solidBrush, this._boxOffset + 2, this._boxOffset + 2, 14, 14);
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                }
                graphics.DrawImageUnscaledAndClipped(this.DrawCheckMarkBitmap(), rectangle);
            }
            SizeF sizeF = graphics.MeasureString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10);
            graphics.DrawString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10, (base.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush()), (float)(this._boxOffset + 22), (float)(base.Height / 2) - sizeF.Height / 2f);
            pen.Dispose();
            solidBrush.Dispose();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this._boxOffset = base.Height / 2 - 9;
            this._boxRectangle = new Rectangle(this._boxOffset, this._boxOffset, 17, 17);
        }
    }
}
