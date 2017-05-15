using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI.GUI.Layout
{
    public class DisplayMode5A : DisplayModeBase
    {
        // Methods
        public override int P_Count() =>
            5;

        public override void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            Size size = viewcontainer.Size;
            int width = size.Width / 3;
            int num2 = (size.Width * 2) / 3;
            int y = 0x21;
            int num1 = num2 / 3;
            int height = (size.Height - y) / 3;
            base.mPositions = new Rectangle[] { new Rectangle(0, y, num2, size.Height - y), new Rectangle(num2, y, width, height), new Rectangle(num2, y + height, width, height), new Rectangle(num2, y + (height * 2), width, height), new Rectangle(0, 0, size.Width, y) };
            base.SetControlPosition(ctrls, viewcontainer);
        }

        public override void ShiftToP1(ControlBase ctrl, Control viewcontainer)
        {
        }
    }
}
