using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GeoPoint
    {
        public float x;
        public float y;
        public GeoPoint(float _x, float _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public override bool Equals(object obj) =>
            base.Equals(obj);

        public override int GetHashCode() =>
            base.GetHashCode();

        public static bool operator ==(GeoPoint g1, GeoPoint g2) =>
            ((g1.x == g2.x) && (g1.y == g2.y));

        public static bool operator !=(GeoPoint g1, GeoPoint g2)
        {
            if (g1.x == g2.x)
            {
                return !(g1.y == g2.y);
            }
            return true;
        }

        public bool Equals(GeoPoint g) =>
            ((this.x == g.x) && (this.y == g.y));
    }
}
