using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class DisplayMode2A : DisplayModeBase
    {
        // Methods
        public override int P_Count() =>
            2;

        public override void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            Size size = viewcontainer.Size;
            int width = (int)(((size.Width * 2f) / 4f) + 0.5);
            int height = size.Height;
            base.mPositions = new Rectangle[] { new Rectangle(0, 0, width, height), new Rectangle(width, 0, size.Width - width, height) };
            base.SetControlPosition(ctrls, viewcontainer);
        }
    }
}
