using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class DisplayMode4C : DisplayModeBase
    {
        // Methods
        public override int P_Count() =>
            4;

        public override void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            Size size = viewcontainer.Size;
            int width = size.Width / 3;
            int num2 = (size.Width * 2) / 3;
            int num1 = num2 / 3;
            int height = size.Height / 3;
            base.mPositions = new Rectangle[] { new Rectangle(0, 0, num2, size.Height), new Rectangle(num2, 0, width, height), new Rectangle(num2, height, width, height), new Rectangle(num2, height * 2, width, height) };
            base.SetControlPosition(ctrls, viewcontainer);
        }

        public override void ShiftToP1(ControlBase ctrl, Control viewcontainer)
        {
        }
    }
}
