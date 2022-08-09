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
    public class MaterialRadioButton : RadioButton, IMaterialControl
    {
        private bool ripple;

        private readonly AnimationManager _animationManager;

        private readonly AnimationManager _rippleAnimationManager;

        private Rectangle _radioButtonBounds;

        private int _boxOffset;

        private const int RADIOBUTTON_SIZE = 19;

        private const int RADIOBUTTON_SIZE_HALF = 9;

        private const int RADIOBUTTON_OUTER_CIRCLE_WIDTH = 2;

        private const int RADIOBUTTON_INNER_CIRCLE_SIZE = 15;

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
                return this.ripple;
            }
            set
            {
                this.ripple = value;
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

        public MaterialRadioButton()
        {
            base.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            this._animationManager = new AnimationManager(true)
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.06
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
            base.SizeChanged += new EventHandler(this.OnSizeChanged);
            this.Ripple = true;
            this.MouseLocation = new Point(-1, -1);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            SizeF sizeF = base.CreateGraphics().MeasureString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10);
            int width = this._boxOffset + 20 + (int)sizeF.Width;
            return (this.Ripple ? new Size(width, 30) : new Size(width, 20));
        }

        private bool IsMouseInCheckArea()
        {
            return this._radioButtonBounds.Contains(this.MouseLocation);
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
            int num1 = this._boxOffset + 9;
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
            float single = (float)(progress * 8);
            float single1 = single / 2f;
            single = (float)(progress * 9);
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(num2, (base.Enabled ? this.SkinManager.ColorScheme.AccentColor : this.SkinManager.GetCheckBoxOffDisabledColor())));
            Pen pen = new Pen(solidBrush.Color);
            if ((!this.Ripple ? false : this._rippleAnimationManager.IsAnimating()))
            {
                for (int i = 0; i < this._rippleAnimationManager.GetAnimationCount(); i++)
                {
                    double progress1 = this._rippleAnimationManager.GetProgress(i);
                    Point point = new Point(num1, num1);
                    SolidBrush solidBrush1 = new SolidBrush(Color.FromArgb((int)(progress1 * 40), ((bool)this._rippleAnimationManager.GetData(i)[0] ? Color.Black : solidBrush.Color)));
                    int num4 = (base.Height % 2 == 0 ? base.Height - 3 : base.Height - 2);
                    int num5 = (this._rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn ? (int)((double)num4 * (0.8 + 0.2 * progress1)) : num4);
                    using (GraphicsPath graphicsPath = DrawHelper.CreateRoundRect((float)(point.X - num5 / 2), (float)(point.Y - num5 / 2), (float)num5, (float)num5, (float)(num5 / 2)))
                    {
                        graphics.FillPath(solidBrush1, graphicsPath);
                    }
                    solidBrush1.Dispose();
                }
            }
            Color color = DrawHelper.BlendColor(base.Parent.BackColor, (base.Enabled ? this.SkinManager.GetCheckboxOffColor() : this.SkinManager.GetCheckBoxOffDisabledColor()), (double)num3);
            using (GraphicsPath graphicsPath1 = DrawHelper.CreateRoundRect((float)this._boxOffset, (float)this._boxOffset, 19f, 19f, 9f))
            {
                graphics.FillPath(new SolidBrush(color), graphicsPath1);
                if (base.Enabled)
                {
                    graphics.FillPath(solidBrush, graphicsPath1);
                }
            }
            graphics.FillEllipse(new SolidBrush(base.Parent.BackColor), 2 + this._boxOffset, 2 + this._boxOffset, 15, 15);
            if (base.Checked)
            {
                using (GraphicsPath graphicsPath2 = DrawHelper.CreateRoundRect((float)num1 - single1, (float)num1 - single1, single, single, 4f))
                {
                    graphics.FillPath(solidBrush, graphicsPath2);
                }
            }
            SizeF sizeF = graphics.MeasureString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10);
            graphics.DrawString(this.Text, this.SkinManager.ROBOTO_MEDIUM_10, (base.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush()), (float)(this._boxOffset + 22), (float)(base.Height / 2) - sizeF.Height / 2f);
            solidBrush.Dispose();
            pen.Dispose();
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            this._boxOffset = base.Height / 2 - (int)Math.Ceiling(9.5);
            this._radioButtonBounds = new Rectangle(this._boxOffset, this._boxOffset, 19, 19);
        }
    }
}
