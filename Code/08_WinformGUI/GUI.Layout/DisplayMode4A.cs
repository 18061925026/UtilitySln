using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class DisplayMode4A : DisplayModeBase
    {
        // Methods
        public override Bitmap GetShowImage(int width, int height, List<string> strs)
        {
            int num = (int)((((float)width) / 2f) + 0.5);
            int num2 = (int)((((float)height) / 2f) + 0.5);
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black);
            Pen pen = new Pen(Color.White, 3f);
            graphics.DrawLine(pen, new Point(0, height / 2), new Point(width, height / 2));
            graphics.DrawLine(pen, new Point(width / 2, 0), new Point(width / 2, height));
            graphics.DrawString(strs[0], base.mFont, base.mBrush, (PointF)base.GetLocationOfString(strs[0], new Rectangle(0, 0, num, num2)));
            graphics.DrawString(strs[1], base.mFont, base.mBrush, (PointF)base.GetLocationOfString(strs[1], new Rectangle(num, 0, num, num2)));
            graphics.DrawString(strs[2], base.mFont, base.mBrush, (PointF)base.GetLocationOfString(strs[2], new Rectangle(0, num2, num, num2)));
            graphics.DrawString(strs[3], base.mFont, base.mBrush, (PointF)base.GetLocationOfString(strs[3], new Rectangle(num, num2, num, num2)));
            return image;
        }

        public override int P_Count() =>
            4;

        public override void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            Size size = viewcontainer.Size;
            int width = (int)((((float)size.Width) / 2f) + 0.5);
            int height = (int)((((float)size.Height) / 2f) + 0.5);
            base.mPositions = new Rectangle[] { new Rectangle(0, 0, width, height), new Rectangle(width, 0, width, height), new Rectangle(0, height, width, height), new Rectangle(width, height, width, height) };
            base.SetControlPosition(ctrls, viewcontainer);
        }
    }


}
