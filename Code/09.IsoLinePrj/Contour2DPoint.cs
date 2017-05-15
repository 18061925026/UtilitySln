using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Contour2DPoint
    {
        public float x;
        public float y;
        public float value;
        public int tag;
    }
}
