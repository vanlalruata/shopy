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
    public class MaterialContextMenuStrip : ContextMenuStrip, IMaterialControl
    {
        internal AnimationManager AnimationManager;

        internal Point AnimationSource;

        private ToolStripItemClickedEventArgs _delayesArgs;

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

        public MaterialContextMenuStrip()
        {
            base.Renderer = new MaterialToolStripRender();
            this.AnimationManager = new AnimationManager(false)
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };
            this.AnimationManager.OnAnimationProgress += new AnimationManager.AnimationProgress((object sender) => base.Invalidate());
            this.AnimationManager.OnAnimationFinished += new AnimationManager.AnimationFinished((object sender) => this.OnItemClicked(this._delayesArgs));
            base.BackColor = this.SkinManager.GetApplicationBackgroundColor();
        }

        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            if ((e.ClickedItem == null ? false : !(e.ClickedItem is ToolStripSeparator)))
            {
                if (e != this._delayesArgs)
                {
                    this._delayesArgs = e;
                    MaterialContextMenuStrip.ItemClickStart itemClickStart = this.OnItemClickStart;
                    if (itemClickStart != null)
                    {
                        itemClickStart(this, e);
                    }
                    else
                    {
                    }
                    this.AnimationManager.StartNewAnimation(AnimationDirection.In, null);
                }
                else
                {
                    base.OnItemClicked(e);
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);
            this.AnimationSource = mea.Location;
        }

        public event MaterialContextMenuStrip.ItemClickStart OnItemClickStart;

        public delegate void ItemClickStart(object sender, ToolStripItemClickedEventArgs e);
    }
}
