using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj.Interface
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AutoTps_GridInfo
    {
        public float Ox;
        public float Oy;
        public float Spacex;
        public float Spacey;
        private int m_Nx;
        private int m_Ny;
        public float[,] data;
        public int Nx
        {
            get =>
                this.m_Nx;
            set
            {
                if (value < 2)
                {
                    throw new Exception("the parameter 'Nx' must be greater than '1'!");
                }
                this.m_Nx = value;
            }
        }
        public int Ny
        {
            get =>
                this.m_Ny;
            set
            {
                if (value < 2)
                {
                    throw new Exception("the parameter 'Ny' must be greater than '1'!");
                }
                this.m_Ny = value;
            }
        }
    }

}
