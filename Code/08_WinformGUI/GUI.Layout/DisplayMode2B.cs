using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class DisplayMode2B : DisplayModeBase
    {
        // Methods
        public override int P_Count() =>
            2;

        public override void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            Size size = viewcontainer.Size;
            int width = size.Width;
            int height = (int)(((size.Height * 2f) / 3f) + 0.5);
            base.mPositions = new Rectangle[] { new Rectangle(0, 0, width, height), new Rectangle(0, height, width, size.Height - height) };
            base.SetControlPosition(ctrls, viewcontainer);
        }
    }
}
