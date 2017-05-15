using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09.FunctionsPrj
{
    public class PubFun
    {
        public static bool CheckIsEqual(double a, double b) =>
    CheckIsEqual(a, b, 1E-06);

        public static bool CheckIsEqual(float a, float b) =>
    CheckIsEqual(a, b, 1E-06F);
        
        public static bool CheckIsEqual(float a, float b, float tolerance) =>
    (Math.Abs((float)(a - b)) <= tolerance);

        public static bool CheckIsEqual(double a, double b, double tolerance) =>
    (Math.Abs((double)(a - b)) <= tolerance);



    }
}
