using _09.FunctionsPrj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_WinformGUI
{
    public static class GUIHelper
    {
        public static Matrix CalculateJacobiMatrix(SizeF viewSize, SizeF imageSize)
        {
            if ((PubFun.CheckIsEqual(imageSize.Width, 0f) || PubFun.CheckIsEqual(imageSize.Height, 0f)) || (PubFun.CheckIsEqual(viewSize.Width, 0f) || PubFun.CheckIsEqual(viewSize.Height, 0f)))
            {
                return null;
            }
            float num = viewSize.Width / imageSize.Width;
            float num2 = viewSize.Height / imageSize.Height;
            if (num < num2)
            {
                float num3 = 0f;
                return new Matrix(num, 0f, 0f, num, num3, (viewSize.Height - (imageSize.Height * num)) / 2f);
            }
            float dx = (viewSize.Width - (imageSize.Width * num2)) / 2f;
            return new Matrix(num2, 0f, 0f, num2, dx, 0f);
        }

    }
}
