using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    public class MaterialTabControl : TabControl, IMaterialControl
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

        public MaterialTabControl()
        {
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg != 4904 ? true : base.DesignMode))
            {
                base.WndProc(ref m);
            }
            else
            {
                m.Result = (IntPtr)1;
            }
        }
    }
}
