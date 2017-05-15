using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    public class C2DMemAlloctor
    {
        // Methods
        public static bool AllocMemory2D(out float[,] matrix, int rows, int cols)
        {
            matrix = new float[rows, cols];
            if (matrix == null)
            {
                return false;
            }
            return true;
        }

        public static void FreeMemory2D(ref float[,] matrix)
        {
            matrix = null;
        }
    }


}
