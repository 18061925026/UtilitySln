using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    public class SameHightLine
    {
        // Fields
        private List<List<List<GeoPoint>>> allCurveList;
        private GridDataInfo dataInfo;
        private float[,] gridDataMatrix;
        private List<float> valuesToFind;

        // Methods
        private Contour2DPoint[] FormatCurveValue()
        {
            List<List<GeoPoint>> list = null;
            List<GeoPoint> list2 = null;
            int num;
            int num2;
            int num3;
            int num5;
            int num6 = 0;
            int index = 0;
            Contour2DPoint[] pointArray = null;
            int count = this.valuesToFind.Count;
            for (num3 = 0; num3 < count; num3++)
            {
                list = this.allCurveList[num3];
                if (list != null)
                {
                    list2 = null;
                    num2 = 0;
                    num5 = 0;
                    while (num5 < list.Count)
                    {
                        list2 = list[num5];
                        num = list2.Count;
                        if (num >= 2)
                        {
                            num6 += num;
                            num6++;
                        }
                        num5++;
                    }
                    num6++;
                }
            }
            num6++;
            pointArray = new Contour2DPoint[num6];
            index = num6 - 1;
            for (num3 = 0; num3 < count; num3++)
            {
                list = this.allCurveList[num3];
                if (list != null)
                {
                    list2 = null;
                    num2 = 0;
                    for (num5 = 0; num5 < list.Count; num5++)
                    {
                        list2 = list[num5];
                        num = list2.Count;
                        if (num >= 2)
                        {
                            for (int i = 0; i < num; i++)
                            {
                                pointArray[index].x = list2[i].x;
                                pointArray[index].y = list2[i].y;
                                pointArray[index].tag = 0;
                                index--;
                            }
                            pointArray[index].x = num;
                            pointArray[index].y = num;
                            pointArray[index].tag = 1;
                            index--;
                            num2++;
                        }
                    }
                    pointArray[index].x = num2;
                    pointArray[index].y = num2;
                    pointArray[index].tag = 2;
                    index--;
                }
            }
            pointArray[index].x = count;
            pointArray[index].y = count;
            pointArray[index].tag = 3;
            return pointArray;
        }

        private void FreeAllCurve()
        {
            if (this.allCurveList != null)
            {
                int count = this.allCurveList.Count;
                List<List<GeoPoint>>[] listArray = this.allCurveList.ToArray();
                List<GeoPoint>[] listArray2 = null;
                for (int i = 0; i < count; i++)
                {
                    listArray2 = listArray[i].ToArray();
                    int num2 = listArray[i].Count;
                    for (int j = 0; j < num2; j++)
                    {
                        listArray2[j].Clear();
                        listArray2[j] = null;
                    }
                    listArray2[i].Clear();
                    listArray2[i] = null;
                }
                this.allCurveList.Clear();
                this.allCurveList = null;
            }
        }

        public bool FreeData()
        {
            if (this.gridDataMatrix != null)
            {
                C2DMemAlloctor.FreeMemory2D(ref this.gridDataMatrix);
                this.gridDataMatrix = null;
            }
            return true;
        }

        public void GenerateContours()
        {
            int count = this.valuesToFind.Count;
            if (count > 0)
            {
                this.FreeAllCurve();
                this.allCurveList = new List<List<List<GeoPoint>>>();
                Lint_trace _trace = new Lint_trace();
                _trace.SetGridDataInfo(ref this.dataInfo);
                _trace.SetInput(this.gridDataMatrix);
                for (int i = 0; i < count; i++)
                {
                    List<List<GeoPoint>> curveList = new List<List<GeoPoint>>();
                    _trace.SetOutput(curveList);
                    float num3 = this.valuesToFind.ToArray()[i];
                    _trace.ExecuteTracing(num3);
                    this.allCurveList.Add(curveList);
                    curveList = null;
                }
            }
        }

        public Contour2DPoint[] HeightLineFind(float[] value, int valueNum)
        {
            if (valueNum < 1)
            {
                return null;
            }
            if (value == null)
            {
                return null;
            }
            this.valuesToFind = new List<float>();
            this.valuesToFind.Clear();
            for (int i = 0; i < valueNum; i++)
            {
                this.valuesToFind.Add(value[i]);
            }
            this.GenerateContours();
            Contour2DPoint[] pointArray = this.FormatCurveValue();
            this.valuesToFind.Clear();
            if (this.gridDataMatrix != null)
            {
                C2DMemAlloctor.FreeMemory2D(ref this.gridDataMatrix);
                this.gridDataMatrix = null;
            }
            return pointArray;
        }

        public bool SetData(float xmin, float xmax, int cols, float ymin, float ymax, int rows, out float zmin, out float zmax, float[,] data, IsoFlip flip, CloseupMethod closeupMethod)
        {
            int num;
            int num2;
            int num3 = 0;
            int num4 = 0;
            this.dataInfo.cols = cols;
            this.dataInfo.rows = rows;
            if (closeupMethod != CloseupMethod.NOTCLOSEUP)
            {
                float num5 = (xmax - xmin) / ((float)(cols - 1));
                float num6 = (ymax - ymin) / ((float)(rows - 1));
                xmin -= num5;
                xmax += num5;
                ymin -= num6;
                ymax += num6;
                this.dataInfo.cols += 2;
                this.dataInfo.rows += 2;
                num3 = num4 = 1;
            }
            this.dataInfo.xMin = xmin;
            this.dataInfo.xMax = xmax;
            this.dataInfo.yMin = ymin;
            this.dataInfo.yMax = ymax;
            this.dataInfo.closeupMethod = closeupMethod;
            this.dataInfo.flip = flip;
            if (this.gridDataMatrix != null)
            {
                C2DMemAlloctor.FreeMemory2D(ref this.gridDataMatrix);
                this.gridDataMatrix = null;
            }
            if (!C2DMemAlloctor.AllocMemory2D(out this.gridDataMatrix, this.dataInfo.rows, this.dataInfo.cols))
            {
                this.gridDataMatrix = null;
                zmin = 0f;
                zmax = 0f;
                return false;
            }
            zmax = zmin = data[0, 0];
            for (num = num3; num < rows; num++)
            {
                for (num2 = num4; num2 < cols; num2++)
                {
                    if (zmax < data[num, num2])
                    {
                        zmax = data[num, num2];
                    }
                    if (zmin > data[num, num2])
                    {
                        zmin = data[num, num2];
                    }
                    this.gridDataMatrix[num, num2] = data[num, num2];
                }
            }
            if (closeupMethod != CloseupMethod.NOTCLOSEUP)
            {
                float num7;
                if (closeupMethod == CloseupMethod.GREATER)
                {
                    num7 = zmin - 100f;
                }
                else
                {
                    num7 = zmax + 100f;
                }
                for (num2 = 0; num2 < this.dataInfo.cols; num2++)
                {
                    this.gridDataMatrix[0, num2] = num7;
                    this.gridDataMatrix[this.dataInfo.rows - 1, num2] = num7;
                }
                for (num = 0; num < (this.dataInfo.rows - 1); num++)
                {
                    this.gridDataMatrix[num, 0] = num7;
                    this.gridDataMatrix[num, this.dataInfo.cols - 1] = num7;
                }
            }
            this.dataInfo.zMax = zmax;
            this.dataInfo.zMin = zmin;
            return true;
        }
    }
}
