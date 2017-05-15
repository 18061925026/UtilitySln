using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj.Interface
{
    // Nested Types
    [StructLayout(LayoutKind.Sequential)]
    public struct AutoTps_Contour
    {
        public int LineCount;
        public float value;
        public ArrayList line;
    }

}
