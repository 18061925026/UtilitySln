using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class DisplayMode3A : DisplayModeBase
    {
        // Methods
        public override Bitmap GetShowImage(int width, int height, List<string> strs) =>
            new Bitmap(0x18, 0x18);

        public override int P_Count() =>
            3;

        public override void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            Size size = viewcontainer.Size;
            int width = (int)(((size.Width * 2f) / 3f) + 0.5);
            int height = (int)((((float)size.Height) / 2f) + 0.5);
            base.mPositions = new Rectangle[] { new Rectangle(0, 0, width, size.Height), new Rectangle(width, 0, size.Width - width, height), new Rectangle(width, height, size.Width - width, size.Height - height) };
            base.SetControlPosition(ctrls, viewcontainer);
        }
    }
}
