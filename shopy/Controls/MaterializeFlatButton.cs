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
    public class MaterialFlatButton : Button, IMaterialControl
    {
        private readonly AnimationManager _animationManager;

        private readonly AnimationManager _hoverAnimationManager;

        private SizeF _textSize;

        private Image _icon;

        [Browsable(false)]
        public int Depth
        {
            get;
            set;
        }

        public Image Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
                if (this.AutoSize)
                {
                    base.Size = this.GetPreferredSize();
                }
                base.Invalidate();
            }
        }

        [Browsable(false)]
        public MouseState MouseState
        {
            get;
            set;
        }

        public bool Primary
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

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this._textSize = base.CreateGraphics().MeasureString(value.ToUpper(), this.SkinManager.ROBOTO_MEDIUM_10);
                if (this.AutoSize)
                {
                    base.Size = this.GetPreferredSize();
                }
                base.Invalidate();
            }
        }

        public MaterialFlatButton()
        {
            this.Primary = false;
            this._animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            this._hoverAnimationManager = new AnimationManager(true)
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };
            this._hoverAnimationManager.OnAnimationProgress += new AnimationManager.AnimationProgress((object sender) => base.Invalidate());
            this._animationManager.OnAnimationProgress += new AnimationManager.AnimationProgress((object sender) => base.Invalidate());
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.AutoSize = true;
            base.Margin = new Padding(4, 6, 4, 6);
            base.Padding = new Padding(0);
        }

        private Size GetPreferredSize()
        {
            return this.GetPreferredSize(new Size(0, 0));
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            int num = 16;
            if (this.Icon != null)
            {
                num += 28;
            }
            Size size = new Size((int)Math.Ceiling((double)this._textSize.Width) + num, 36);
            return size;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!base.DesignMode)
            {
                this.MouseState = MouseState.OUT;
                base.MouseEnter += new EventHandler((object sender, EventArgs args) => {
                    this.MouseState = MouseState.HOVER;
                    this._hoverAnimationManager.StartNewAnimation(AnimationDirection.In, null);
                    base.Invalidate();
                });
                base.MouseLeave += new EventHandler((object sender, EventArgs args) => {
                    this.MouseState = MouseState.OUT;
                    this._hoverAnimationManager.StartNewAnimation(AnimationDirection.Out, null);
                    base.Invalidate();
                });
                base.MouseDown += new MouseEventHandler((object sender, MouseEventArgs args) => {
                    if (args.Button == MouseButtons.Left)
                    {
                        this.MouseState = MouseState.DOWN;
                        this._animationManager.StartNewAnimation(AnimationDirection.In, args.Location, null);
                        base.Invalidate();
                    }
                });
                base.MouseUp += new MouseEventHandler((object sender, MouseEventArgs args) => {
                    this.MouseState = MouseState.HOVER;
                    base.Invalidate();
                });
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Brush flatButtonDisabledTextBrush;
            Graphics graphics = pevent.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Clear(base.Parent.BackColor);
            Color flatButtonHoverBackgroundColor = this.SkinManager.GetFlatButtonHoverBackgroundColor();
            using (Brush solidBrush = new SolidBrush(Color.FromArgb((int)(this._hoverAnimationManager.GetProgress() * (double)flatButtonHoverBackgroundColor.A), flatButtonHoverBackgroundColor.RemoveAlpha())))
            {
                graphics.FillRectangle(solidBrush, base.ClientRectangle);
            }
            if (this._animationManager.IsAnimating())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                for (int i = 0; i < this._animationManager.GetAnimationCount(); i++)
                {
                    double progress = this._animationManager.GetProgress(i);
                    Point source = this._animationManager.GetSource(i);
                    using (Brush brush = new SolidBrush(Color.FromArgb((int)(101 - progress * 100), Color.Black)))
                    {
                        int width = (int)(progress * (double)base.Width * 2);
                        graphics.FillEllipse(brush, new Rectangle(source.X - width / 2, source.Y - width / 2, width, width));
                    }
                }
                graphics.SmoothingMode = SmoothingMode.None;
            }
            Rectangle rectangle = new Rectangle(8, 6, 24, 24);
            if (string.IsNullOrEmpty(this.Text))
            {
                rectangle.X = rectangle.X + 2;
            }
            if (this.Icon != null)
            {
                graphics.DrawImage(this.Icon, rectangle);
            }
            Rectangle clientRectangle = base.ClientRectangle;
            if (this.Icon != null)
            {
                clientRectangle.Width = clientRectangle.Width - 44;
                clientRectangle.X = clientRectangle.X + 36;
            }
            Graphics graphic = graphics;
            string upper = this.Text.ToUpper();
            Font rOBOTOMEDIUM10 = this.SkinManager.ROBOTO_MEDIUM_10;
            if (base.Enabled)
            {
                flatButtonDisabledTextBrush = (this.Primary ? this.SkinManager.ColorScheme.PrimaryBrush : this.SkinManager.GetPrimaryTextBrush());
            }
            else
            {
                flatButtonDisabledTextBrush = this.SkinManager.GetFlatButtonDisabledTextBrush();
            }
            graphic.DrawString(upper, rOBOTOMEDIUM10, flatButtonDisabledTextBrush, clientRectangle, new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
        }
    }
}
