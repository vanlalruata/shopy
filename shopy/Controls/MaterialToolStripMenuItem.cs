using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shopy.Controls
{
    public class MaterialToolStripMenuItem : ToolStripMenuItem
    {
        public MaterialToolStripMenuItem()
        {
            base.AutoSize = false;
            this.Size = new Size(120, 30);
        }

        protected override ToolStripDropDown CreateDefaultDropDown()
        {
            ToolStripDropDown toolStripDropDown;
            ToolStripDropDown toolStripDropDown1 = base.CreateDefaultDropDown();
            if (!base.DesignMode)
            {
                MaterialContextMenuStrip materialContextMenuStrip = new MaterialContextMenuStrip();
                materialContextMenuStrip.Items.AddRange(toolStripDropDown1.Items);
                toolStripDropDown = materialContextMenuStrip;
            }
            else
            {
                toolStripDropDown = toolStripDropDown1;
            }
            return toolStripDropDown;
        }
    }
}
