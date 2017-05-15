using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GridDataInfo
    {
        public int rows;
        public int cols;
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;
        public float zMin;
        public float zMax;
        public IsoFlip flip;
        public CloseupMethod closeupMethod;
        public GridDataInfo(int Rows, int Cols, float XMin, float XMax, float YMin, float YMax, float ZMin, float ZMax, IsoFlip Flip, CloseupMethod C)
        {
            this.rows = Rows;
            this.cols = Cols;
            this.xMin = XMin;
            this.xMax = XMax;
            this.yMin = YMin;
            this.yMax = YMax;
            this.zMin = ZMin;
            this.zMax = ZMax;
            this.flip = Flip;
            this.closeupMethod = C;
        }
    }
}
