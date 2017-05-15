using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IsoPoint
    {
        public int i;
        public int j;
        public bool bHorV;
        public float x;
        public float y;
    }
}
