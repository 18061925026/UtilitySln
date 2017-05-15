using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsoLinePrj
{
    public class Lint_trace
    {
        // Fields
        private List<GeoPoint> currentCurveLine = null;
        private IsoPoint CurrentPoint;
        private List<List<GeoPoint>> curveList = null;
        private float deltX = 0f;
        private float deltY = 0f;
        private float[,] gridData = null;
        private GridDataInfo gridDataInfo;
        private const bool HORIZON = true;
        private IsoPoint NextPoint;
        private float Ox = 0f;
        private float Oy = 0f;
        private IsoPoint PreviousPoint;
        private float valueTracing = 4f;
        private const bool VERTIC = false;
        private float[,] xSide = null;
        private float[,] ySide = null;

        // Methods
        public void AllocateMemory()
        {
            int cols = this.gridDataInfo.cols;
            int rows = this.gridDataInfo.rows;
            this.xSide = null;
            C2DMemAlloctor.AllocMemory2D(out this.xSide, rows, cols - 1);
            this.ySide = null;
            C2DMemAlloctor.AllocMemory2D(out this.ySide, rows - 1, cols);
        }

        private void CalcAndSaveOnePointCoord(int i, int j, bool bHorizon, out float x, out float y)
        {
            if (bHorizon)
            {
                x = this.Ox + ((j + this.xSide[i, j]) * this.deltX);
                y = this.Oy + (i * this.deltY);
            }
            else
            {
                x = this.Ox + (j * this.deltX);
                y = this.Oy + ((i + this.ySide[i, j]) * this.deltY);
            }
            GeoPoint item = new GeoPoint(x, y);
            this.currentCurveLine.Add(item);
        }

        public bool ExecuteTracing(float value)
        {
            if ((value < this.gridDataInfo.zMin) || (value > this.gridDataInfo.zMax))
            {
                return false;
            }
            this.valueTracing = value;
            this.AllocateMemory();
            this.InterpolateTracingValue();
            if (this.gridDataInfo.closeupMethod == CloseupMethod.NOTCLOSEUP)
            {
                this.TracingNonClosedContour();
            }
            if (!this.TracingClosedContour())
            {
                return false;
            }
            this.FreeMemory();
            return true;
        }

        private bool FloatEQ(float r1, float r2)
        {
            float num = r1 - r2;
            return ((num > -1E-06) && (num < 1E-06));
        }

        private void FreeMemory()
        {
            if (this.xSide != null)
            {
                C2DMemAlloctor.FreeMemory2D(ref this.xSide);
            }
            if (this.ySide != null)
            {
                C2DMemAlloctor.FreeMemory2D(ref this.ySide);
            }
        }

        private void FromBottom2TopTracing()
        {
            int i = this.CurrentPoint.i;
            int j = this.CurrentPoint.j;
            if (this.ySide[i, j] < this.ySide[i, j + 1])
            {
                if (this.ySide[i, j] > 0f)
                {
                    this.HandlingAfterNextPointFound(i, j, false);
                }
                else
                {
                    this.HandlingAfterNextPointFound(i, j + 1, false);
                }
            }
            else if (this.ySide[i, j] == this.ySide[i, j + 1])
            {
                if (this.ySide[i, j] < 0f)
                {
                    this.HandlingAfterNextPointFound(i + 1, j, true);
                }
                else
                {
                    float num3 = (this.valueTracing - this.gridData[i, j]) / (this.gridData[i, j + 1] - this.gridData[i, j]);
                    if (num3 <= 0.5f)
                    {
                        this.HandlingAfterNextPointFound(i, j, false);
                    }
                    else
                    {
                        this.HandlingAfterNextPointFound(i, j + 1, false);
                    }
                }
            }
            else if (this.ySide[i, j] > this.ySide[i, j + 1])
            {
                if (this.ySide[i, j + 1] > 0f)
                {
                    this.HandlingAfterNextPointFound(i, j + 1, false);
                }
                else
                {
                    this.HandlingAfterNextPointFound(i, j, false);
                }
            }
        }

        private void FromLeft2RightTracing()
        {
            int i = this.CurrentPoint.i;
            int j = this.CurrentPoint.j;
            if (this.xSide[i, j] < this.xSide[i + 1, j])
            {
                if (this.xSide[i, j] > 0f)
                {
                    this.HandlingAfterNextPointFound(i, j, true);
                }
                else
                {
                    this.HandlingAfterNextPointFound(i + 1, j, true);
                }
            }
            else if (this.xSide[i, j] == this.xSide[i + 1, j])
            {
                if (this.xSide[i, j] < 0f)
                {
                    this.HandlingAfterNextPointFound(i, j + 1, false);
                }
                else
                {
                    float num3 = (this.valueTracing - this.gridData[i, j]) / (this.gridData[i + 1, j] - this.gridData[i, j]);
                    if (num3 <= 0.5f)
                    {
                        this.HandlingAfterNextPointFound(i, j, true);
                    }
                    else
                    {
                        this.HandlingAfterNextPointFound(i + 1, j, true);
                    }
                }
            }
            else if (this.xSide[i, j] > this.xSide[i + 1, j])
            {
                if (this.xSide[i + 1, j] > 0f)
                {
                    this.HandlingAfterNextPointFound(i + 1, j, true);
                }
                else
                {
                    this.HandlingAfterNextPointFound(i, j, true);
                }
            }
        }

        private void FromRight2LeftTracing()
        {
            int i = this.CurrentPoint.i;
            int j = this.CurrentPoint.j;
            if (this.xSide[i, j - 1] < this.xSide[i + 1, j - 1])
            {
                if (this.xSide[i, j - 1] > 0f)
                {
                    this.HandlingAfterNextPointFound(i, j - 1, true);
                }
                else
                {
                    this.HandlingAfterNextPointFound(i + 1, j - 1, true);
                }
            }
            else if (this.xSide[i, j - 1] == this.xSide[i + 1, j - 1])
            {
                if (this.xSide[i, j - 1] < 0f)
                {
                    this.HandlingAfterNextPointFound(i, j - 1, false);
                }
                else
                {
                    float num3 = (this.valueTracing - this.gridData[i, j]) / (this.gridData[i + 1, j] - this.gridData[i, j]);
                    if (num3 <= 0.5f)
                    {
                        this.HandlingAfterNextPointFound(i, j - 1, true);
                    }
                    else
                    {
                        this.HandlingAfterNextPointFound(i + 1, j - 1, true);
                    }
                }
            }
            else if (this.xSide[i, j - 1] > this.xSide[i + 1, j - 1])
            {
                if (this.xSide[i + 1, j - 1] > 0f)
                {
                    this.HandlingAfterNextPointFound(i + 1, j - 1, true);
                }
                else
                {
                    this.HandlingAfterNextPointFound(i, j - 1, true);
                }
            }
        }

        private void FromTop2BottomTracing()
        {
            int i = this.CurrentPoint.i;
            int j = this.CurrentPoint.j;
            if (this.ySide[i - 1, j] < this.ySide[i - 1, j + 1])
            {
                if (this.ySide[i - 1, j] > 0f)
                {
                    this.HandlingAfterNextPointFound(i - 1, j, false);
                }
                else
                {
                    this.HandlingAfterNextPointFound(i - 1, j + 1, false);
                }
            }
            else if (this.ySide[i - 1, j] == this.ySide[i - 1, j + 1])
            {
                if (this.ySide[i - 1, j] < 0f)
                {
                    this.HandlingAfterNextPointFound(i - 1, j, true);
                }
                else
                {
                    float num3 = (this.valueTracing - this.gridData[i, j]) / (this.gridData[i, j + 1] - this.gridData[i, j]);
                    if (num3 <= 0.5f)
                    {
                        this.HandlingAfterNextPointFound(i - 1, j, false);
                    }
                    else
                    {
                        this.HandlingAfterNextPointFound(i - 1, j + 1, false);
                    }
                }
            }
            else if (this.ySide[i - 1, j] > this.ySide[i - 1, j + 1])
            {
                if (this.ySide[i - 1, j + 1] > 0f)
                {
                    this.HandlingAfterNextPointFound(i - 1, j + 1, false);
                }
                else
                {
                    this.HandlingAfterNextPointFound(i - 1, j, false);
                }
            }
        }

        private bool HandlingAfterNextPointFound(int i, int j, bool bHorizon)
        {
            if (((i < 0) || (i > (this.gridDataInfo.rows - 1))) || ((j < 0) || (j > (this.gridDataInfo.cols - 1))))
            {
                return false;
            }
            this.NextPoint.i = i;
            this.NextPoint.j = j;
            this.NextPoint.bHorV = bHorizon;
            this.CalcAndSaveOnePointCoord(i, j, bHorizon, out this.NextPoint.x, out this.NextPoint.y);
            if (this.NextPoint.bHorV)
            {
                this.xSide[i, j] = -2f;
            }
            else
            {
                this.ySide[i, j] = -2f;
            }
            return true;
        }

        public void InterpolateTracingValue()
        {
            int num;
            int num2;
            float num6;
            float num7;
            float num8;
            float num9;
            float num10;
            int rows = this.gridDataInfo.rows;
            int cols = this.gridDataInfo.cols;
            float valueTracing = this.valueTracing;
            for (num = 0; num < (rows - 1); num++)
            {
                num2 = 0;
                while (num2 < (cols - 1))
                {
                    num6 = this.gridData[num, num2];
                    num7 = this.gridData[num, num2 + 1];
                    num8 = this.gridData[num, num2];
                    num9 = this.gridData[num + 1, num2];
                    if (num6 == num7)
                    {
                        this.xSide[num, num2] = -2f;
                    }
                    else
                    {
                        if ((num6 - valueTracing) == 0f)
                        {
                            num6 += 0.001f;
                        }
                        else if ((num7 - valueTracing) == 0f)
                        {
                            num7 += 0.001f;
                        }
                        num10 = (valueTracing - num6) * (valueTracing - num7);
                        if (num10 > 0f)
                        {
                            this.xSide[num, num2] = -2f;
                        }
                        else
                        {
                            this.xSide[num, num2] = (valueTracing - num6) / (num7 - num6);
                        }
                    }
                    if (num8 == num9)
                    {
                        this.ySide[num, num2] = -2f;
                    }
                    else
                    {
                        if ((num8 - valueTracing) == 0f)
                        {
                            num8 += 0.001f;
                        }
                        else if ((num9 - valueTracing) == 0f)
                        {
                            num9 += 0.001f;
                        }
                        num10 = (valueTracing - num8) * (valueTracing - num9);
                        if (num10 > 0f)
                        {
                            this.ySide[num, num2] = -2f;
                        }
                        else
                        {
                            this.ySide[num, num2] = (valueTracing - num8) / (num9 - num8);
                        }
                    }
                    num2++;
                }
            }
            for (num2 = 0; num2 < (cols - 1); num2++)
            {
                num6 = this.gridData[rows - 1, num2];
                num7 = this.gridData[rows - 1, num2 + 1];
                if (num6 == num7)
                {
                    this.xSide[rows - 1, num2] = -2f;
                }
                else
                {
                    if ((num6 - valueTracing) == 0f)
                    {
                        num6 += 0.001f;
                    }
                    else if ((num7 - valueTracing) == 0f)
                    {
                        num7 += 0.001f;
                    }
                    num10 = (valueTracing - num6) * (valueTracing - num7);
                    if (num10 > 0f)
                    {
                        this.xSide[rows - 1, num2] = -2f;
                    }
                    else
                    {
                        this.xSide[rows - 1, num2] = (valueTracing - num6) / (num7 - num6);
                    }
                }
            }
            for (num = 0; num < (rows - 1); num++)
            {
                num8 = this.gridData[num, cols - 1];
                num9 = this.gridData[num + 1, cols - 1];
                if (num8 == num9)
                {
                    this.ySide[num, cols - 1] = -2f;
                }
                else
                {
                    if ((num8 - valueTracing) == 0f)
                    {
                        num8 += 0.001f;
                    }
                    else if ((num9 - valueTracing) == 0f)
                    {
                        num9 += 0.001f;
                    }
                    num10 = (valueTracing - num8) * (valueTracing - num9);
                    if (num10 > 0f)
                    {
                        this.ySide[num, cols - 1] = -2f;
                    }
                    else
                    {
                        this.ySide[num, cols - 1] = (valueTracing - num8) / (num9 - num8);
                    }
                }
            }
        }

        private bool IsHavingPoint(float r) =>
            ((r <= 1f) && (r >= 0f));

        public void SetGridDataInfo(ref GridDataInfo dataInfo)
        {
            this.gridDataInfo = dataInfo;
            this.deltX = (this.gridDataInfo.xMax - this.gridDataInfo.xMin) / ((float)(this.gridDataInfo.cols - 1));
            this.deltY = (this.gridDataInfo.yMax - this.gridDataInfo.yMin) / ((float)(this.gridDataInfo.rows - 1));
            if (dataInfo.flip == IsoFlip.NOTFLIP)
            {
                this.Ox = dataInfo.xMin;
                this.Oy = dataInfo.yMin;
            }
            else if (dataInfo.flip == IsoFlip.VERTICAL)
            {
                this.Ox = dataInfo.xMin;
                this.Oy = dataInfo.yMax;
                this.deltY = -this.deltY;
            }
            else if (dataInfo.flip == IsoFlip.HORIZONTAL)
            {
                this.Ox = dataInfo.xMax;
                this.Oy = dataInfo.yMin;
                this.deltX = -this.deltX;
            }
            else if (dataInfo.flip == IsoFlip.HORIZONTANANDVERTICAL)
            {
                this.Ox = dataInfo.xMax;
                this.Oy = dataInfo.yMax;
                this.deltX = -this.deltX;
                this.deltY = -this.deltY;
            }
        }

        public void SetInput(float[,] gridData)
        {
            this.gridData = gridData;
        }

        public void SetOutput(List<List<GeoPoint>> curveList)
        {
            this.curveList = curveList;
        }

        private bool TracingClosedContour()
        {
            int cols = this.gridDataInfo.cols;
            int rows = this.gridDataInfo.rows;
            for (int i = 0; i < (cols - 1); i++)
            {
                for (int j = 0; j < (rows - 1); j++)
                {
                    if (this.IsHavingPoint(this.ySide[j, i]))
                    {
                        this.TracingOneClosedContour(j, i);
                    }
                }
            }
            return true;
        }

        private void TracingNextPoint()
        {
            if (this.CurrentPoint.i > this.PreviousPoint.i)
            {
                this.FromBottom2TopTracing();
            }
            else if (this.CurrentPoint.j > this.PreviousPoint.j)
            {
                this.FromLeft2RightTracing();
            }
            else if (this.CurrentPoint.bHorV)
            {
                this.FromTop2BottomTracing();
            }
            else
            {
                this.FromRight2LeftTracing();
            }
        }

        private void TracingNonClosedContour()
        {
            int num;
            int num2;
            int cols = this.gridDataInfo.cols;
            int rows = this.gridDataInfo.rows;
            for (num2 = 0; num2 < (cols - 1); num2++)
            {
                if (this.IsHavingPoint(this.xSide[0, num2]))
                {
                    this.PreviousPoint.i = -1;
                    this.PreviousPoint.j = num2;
                    this.PreviousPoint.bHorV = true;
                    this.CurrentPoint.i = 0;
                    this.CurrentPoint.j = num2;
                    this.CurrentPoint.bHorV = true;
                    this.TracingOneNonClosedContour();
                }
            }
            for (num = 0; num < (rows - 1); num++)
            {
                if (this.IsHavingPoint(this.ySide[num, 0]))
                {
                    this.PreviousPoint.i = num;
                    this.PreviousPoint.j = -1;
                    this.PreviousPoint.bHorV = false;
                    this.CurrentPoint.i = num;
                    this.CurrentPoint.j = 0;
                    this.CurrentPoint.bHorV = false;
                    this.TracingOneNonClosedContour();
                }
            }
            for (num2 = 0; num2 < (cols - 1); num2++)
            {
                if (this.IsHavingPoint(this.xSide[rows - 1, num2]))
                {
                    this.PreviousPoint.i = rows - 1;
                    this.PreviousPoint.j = num2;
                    this.PreviousPoint.bHorV = true;
                    this.CurrentPoint.i = rows - 1;
                    this.CurrentPoint.j = num2;
                    this.CurrentPoint.bHorV = true;
                    this.TracingOneNonClosedContour();
                }
            }
            for (num = 0; num < (rows - 1); num++)
            {
                if (this.IsHavingPoint(this.ySide[num, cols - 1]))
                {
                    this.PreviousPoint.i = num;
                    this.PreviousPoint.j = cols - 1;
                    this.PreviousPoint.bHorV = false;
                    this.CurrentPoint.i = num;
                    this.CurrentPoint.j = cols - 1;
                    this.CurrentPoint.bHorV = false;
                    this.TracingOneNonClosedContour();
                }
            }
        }

        private bool TracingOneClosedContour(int startI, int startJ)
        {
            List<GeoPoint> item = new List<GeoPoint>();
            item.Clear();
            this.curveList.Add(item);
            this.currentCurveLine = item;
            this.PreviousPoint.i = startI;
            this.PreviousPoint.j = 0;
            this.PreviousPoint.bHorV = false;
            this.CurrentPoint.i = startI;
            this.CurrentPoint.j = startJ;
            this.CurrentPoint.bHorV = false;
            this.CalcAndSaveOnePointCoord(startI, startJ, false, out this.CurrentPoint.x, out this.CurrentPoint.y);
            this.TracingNextPoint();
            this.PreviousPoint = this.CurrentPoint;
            this.CurrentPoint = this.NextPoint;
            for (bool flag = false; !flag; flag = ((this.CurrentPoint.i == startI) && (this.CurrentPoint.j == startJ)) && !this.CurrentPoint.bHorV)
            {
                this.TracingNextPoint();
                this.PreviousPoint = this.CurrentPoint;
                this.CurrentPoint = this.NextPoint;
            }
            this.ySide[startI, startJ] = -2f;
            return true;
        }

        private void TracingOneNonClosedContour()
        {
            List<GeoPoint> item = new List<GeoPoint>();
            item.Clear();
            this.curveList.Add(item);
            this.currentCurveLine = item;
            int i = this.CurrentPoint.i;
            int j = this.CurrentPoint.j;
            bool bHorV = this.CurrentPoint.bHorV;
            this.CalcAndSaveOnePointCoord(i, j, bHorV, out this.CurrentPoint.x, out this.CurrentPoint.y);
            if (bHorV)
            {
                this.xSide[i, j] = -2f;
            }
            else
            {
                this.ySide[i, j] = -2f;
            }
            this.TracingNextPoint();
            this.PreviousPoint = this.CurrentPoint;
            this.CurrentPoint = this.NextPoint;
            int cols = this.gridDataInfo.cols;
            int rows = this.gridDataInfo.rows;
            for (bool flag2 = ((((this.CurrentPoint.i == 0) && this.CurrentPoint.bHorV) || (this.CurrentPoint.i == (rows - 1))) || ((this.CurrentPoint.j == 0) && !this.CurrentPoint.bHorV)) || (this.CurrentPoint.j == (cols - 1)); !flag2; flag2 = ((((this.CurrentPoint.i == 0) && this.CurrentPoint.bHorV) || (this.CurrentPoint.i == (rows - 1))) || ((this.CurrentPoint.j == 0) && !this.CurrentPoint.bHorV)) || (this.CurrentPoint.j == (cols - 1)))
            {
                this.TracingNextPoint();
                this.PreviousPoint = this.CurrentPoint;
                this.CurrentPoint = this.NextPoint;
            }
        }
    }
}
