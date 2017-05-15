using IsoLinePrj.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    public class InterploatedContourNewEdition
    {
        // Methods
        public static ArrayList GenerateContour(AutoTps_GridInfo doseGrid, float[] valuelist, IsoFlip flipType)
        {
            float num16;
            float num17;
            SameHightLine line = new SameHightLine();
            float ox = doseGrid.Ox;
            float xmax = doseGrid.Ox + ((doseGrid.Nx - 1) * doseGrid.Spacex);
            int nx = doseGrid.Nx;
            float oy = doseGrid.Oy;
            float ymax = doseGrid.Oy + ((doseGrid.Ny - 1) * doseGrid.Spacey);
            int ny = doseGrid.Ny;
            Contour2DPoint[] pointArray2 = null;
            line.SetData(ox, xmax, nx, oy, ymax, ny, out num17, out num16, doseGrid.data, flipType, CloseupMethod.NOTCLOSEUP);
            pointArray2 = line.HeightLineFind(valuelist, valuelist.Length);
            int index = 1;
            ArrayList list2 = new ArrayList();
            int x = (int)pointArray2[0].x;
            for (int i = 0; i < x; i++)
            {
                AutoTps_Contour contour2;
                contour2.line = new ArrayList();
                contour2.value = pointArray2[index].value;
                int num19 = (int)pointArray2[index].x;
                contour2.LineCount = num19;
                index++;
                for (int num23 = 0; num23 < num19; num23++)
                {
                    int num20 = (int)pointArray2[index].x;
                    PointF[] tfArray2 = new PointF[num20];
                    index++;
                    for (int num24 = 0; num24 < num20; num24++)
                    {
                        tfArray2[num24].X = pointArray2[index].x;
                        tfArray2[num24].Y = pointArray2[index].y;
                        index++;
                    }
                    contour2.line.Add(tfArray2);
                }
                list2.Add(contour2);
            }
            return list2;
        } 
    }
}
