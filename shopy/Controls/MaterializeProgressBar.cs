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
    public class MaterialProgressBar : ProgressBar, IMaterialControl
    {
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

        public MaterialProgressBar()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clipRectangle = e.ClipRectangle;
            int width = (int)((double)clipRectangle.Width * ((double)base.Value / (double)base.Maximum));
            Graphics graphics = e.Graphics;
            Brush primaryBrush = this.SkinManager.ColorScheme.PrimaryBrush;
            clipRectangle = e.ClipRectangle;
            graphics.FillRectangle(primaryBrush, 0, 0, width, clipRectangle.Height);
            Graphics graphic = e.Graphics;
            Brush disabledOrHintBrush = this.SkinManager.GetDisabledOrHintBrush();
            int num = e.ClipRectangle.Width;
            clipRectangle = e.ClipRectangle;
            graphic.FillRectangle(disabledOrHintBrush, width, 0, num, clipRectangle.Height);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, 5, specified);
        }
    }
}
