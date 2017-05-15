using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class DisplayMode4B : DisplayModeBase
    {
        // Methods
        public override int P_Count() =>
            4;

        public override void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            Size size = viewcontainer.Size;
            int width = (int)((((float)size.Width) / 3f) + 0.5);
            int num2 = size.Width - width;
            int height = (int)((((float)size.Height) / 3f) + 0.5);
            int num4 = size.Height - height;
            base.mPositions = new Rectangle[] { new Rectangle(0, 0, num2, num4), new Rectangle(num2, 0, width, height), new Rectangle(num2, height, width, size.Height - (height * 2)), new Rectangle(0, num4, size.Width, height) };
            base.SetControlPosition(ctrls, viewcontainer);
        }
    }

}
