using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GeoRect
    {
        public float left;
        public float top;
        public float right;
        public float bottom;
        public GeoRect(float l, float b, float r, float t)
        {
            this.left = l;
            this.bottom = b;
            this.right = r;
            this.top = t;
        }

        public GeoRect(GeoRect g)
        {
            this.left = g.left;
            this.right = g.right;
            this.bottom = g.bottom;
            this.top = g.top;
        }

        public float Height() =>
            (this.top - this.bottom);

        public float Width() =>
            (this.right - this.left);

        public float Area() =>
            (this.Height() * this.Width());

        public GeoPoint CenterPonit() =>
            new GeoPoint((this.right + this.left) / 2f, (this.top + this.bottom) / 2f);

        public void Scale(float ratio)
        {
            this.left /= ratio;
            this.right /= ratio;
            this.top /= ratio;
            this.bottom /= ratio;
        }

        public void Translate(float x, float y)
        {
            this.left += x;
            this.right += x;
            this.top += y;
            this.bottom += y;
        }

        public GeoPoint TopLeft() =>
            new GeoPoint(this.left, this.top);

        public GeoPoint BottomRight() =>
            new GeoPoint(this.right, this.bottom);
    }
}
