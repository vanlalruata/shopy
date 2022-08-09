using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    class MaterializeLabel : Label, IMaterialControl
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

        public MaterializeLabel()
        {
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.ForeColor = this.SkinManager.GetPrimaryTextColor();
            this.Font = this.SkinManager.ROBOTO_REGULAR_11;
            base.BackColorChanged += new EventHandler((object sender, EventArgs args) => this.ForeColor = this.SkinManager.GetPrimaryTextColor());
        }
    }


}
