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
    public class MaterialRaisedButton : Button, IMaterialControl
    {
        private readonly AnimationManager _animationManager;

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

        public MaterialRaisedButton()
        {
            this.Primary = true;
            this._animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            this._animationManager.OnAnimationProgress += new AnimationManager.AnimationProgress((object sender) => base.Invalidate());
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.AutoSize = true;
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

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            this._animationManager.StartNewAnimation(AnimationDirection.In, mevent.Location, null);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.Clear(base.Parent.BackColor);
            float x = (float)base.ClientRectangle.X;
            float y = (float)base.ClientRectangle.Y;
            Rectangle clientRectangle = base.ClientRectangle;
            float width = (float)(clientRectangle.Width - 1);
            clientRectangle = base.ClientRectangle;
            using (GraphicsPath graphicsPath = DrawHelper.CreateRoundRect(x, y, width, (float)(clientRectangle.Height - 1), 1f))
            {
                graphics.FillPath((this.Primary ? this.SkinManager.ColorScheme.PrimaryBrush : this.SkinManager.GetRaisedButtonBackgroundBrush()), graphicsPath);
            }
            if (this._animationManager.IsAnimating())
            {
                for (int i = 0; i < this._animationManager.GetAnimationCount(); i++)
                {
                    double progress = this._animationManager.GetProgress(i);
                    Point source = this._animationManager.GetSource(i);
                    SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)(51 - progress * 50), Color.White));
                    int num = (int)(progress * (double)base.Width * 2);
                    graphics.FillEllipse(solidBrush, new Rectangle(source.X - num / 2, source.Y - num / 2, num, num));
                }
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
            Rectangle clientRectangle1 = base.ClientRectangle;
            if (this.Icon != null)
            {
                clientRectangle1.Width = clientRectangle1.Width - 44;
                clientRectangle1.X = clientRectangle1.X + 36;
            }
            graphics.DrawString(this.Text.ToUpper(), this.SkinManager.ROBOTO_MEDIUM_10, this.SkinManager.GetRaisedButtonTextBrush(this.Primary), clientRectangle1, new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
        }
    }
}
