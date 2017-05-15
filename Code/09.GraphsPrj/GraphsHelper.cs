using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphsPrj
{
    public static class GraphsHelper
    {
        public static PointF TransformPoint(PointF p, Matrix matrix)
        {
            PointF[] pts = new PointF[] { p };
            matrix.TransformPoints(pts);
            return pts[0];
        }
    }
}
