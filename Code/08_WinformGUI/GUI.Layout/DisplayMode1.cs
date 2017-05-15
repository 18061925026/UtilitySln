using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class DisplayMode1 : DisplayModeBase
    {
        // Methods
        public override int P_Count() =>
            1;

        public override void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            Size size = viewcontainer.Size;
            base.mPositions = new Rectangle[] { new Rectangle(0, 0, size.Width, size.Height) };
            base.SetControlPosition(ctrls, viewcontainer);
        }
    }
}
