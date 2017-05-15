using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImagePrj
{
    public class BoundaryTrack : IDisposable
    {
        // Fields
        private float[] _data = null;
        private int _height;
        private bool _IsDisposed;
        private int _lines;
        private int _points;
        private int _width;

        // Methods
        public BoundaryTrack()
        {
            this._width = this._height = 0;
            this._lines = 0;
            this._IsDisposed = false;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (!this._IsDisposed)
            {
                this._data = null;
                this._IsDisposed = true;
            }
        }

        ~BoundaryTrack()
        {
            this.Dispose(false);
        }

        public bool[,] GetBoundary(Color color) =>
            this.GetBoundary((float)color.ToArgb(), 0f);

        public unsafe bool[,] GetBoundary(float value, float delta)
        {
            delta = Math.Abs(delta);
            bool* boolBoundary = GetBoundary(this._data, this._width, this._height, value, delta);
            if (boolBoundary == null)
            {
                return null;
            }
            bool[,] boolMatrix = new bool[this._width, this._height];
            fixed (bool* pBoolMatrixHead = boolMatrix)
            {
                bool* pBoolMatrix = pBoolMatrixHead;
                for (int i = 0; i < this._height; i++)
                {
                    for (int j = 0; j < this._width; j++)
                    {
                        *pBoolMatrix = *boolBoundary;
                        pBoolMatrix++;
                        boolBoundary++;
                    }
                }
                pBoolMatrix = null;
            }
            boolBoundary = null;
            return boolMatrix;
        }

        [DllImport("Isoline.dll")]
        public static extern unsafe bool* GetBoundary(float[] data, int cols, int rows, float value, float delta);
        public bool[,] GetRegion(Color color) =>
            this.GetRegion((float)color.ToArgb(), 0f);

        public unsafe bool[,] GetRegion(float value, float delta)
        {
            delta = Math.Abs(delta);
            bool* boolRegion = GetRegion(this._data, this._width, this._height, value, delta);
            if (boolRegion == null)
            {
                return null;
            }
            bool[,] boolMatrix = new bool[this._width, this._height];
            fixed (bool* pBoolMatrixHead = boolMatrix)
            {
                bool* pBoolMatrix = pBoolMatrixHead;
                for (int i = 0; i < this._height; i++)
                {
                    for (int j = 0; j < this._width; j++)
                    {
                        *pBoolMatrix = *boolRegion;
                        pBoolMatrix++;
                        boolRegion++;
                    }
                }
                pBoolMatrix = null;
            }
            boolRegion = null;
            return boolMatrix;
        }

        [DllImport("Isoline.dll")]
        public static extern unsafe bool* GetRegion(float[] data, int cols, int rows, float value, float delta);
        public bool GetSerializedBoundary(out AutoTps_BoundaryList BoundaryList, Color color, int skip) =>
            this.GetSerializedBoundary(out BoundaryList, (float)color.ToArgb(), 0f, skip);

        public bool GetSerializedBoundary(out AutoTps_Point[] Boundary, Color color, int skip) =>
            this.GetSerializedBoundary(out Boundary, (float)color.ToArgb(), 0f, skip);

        public bool GetSerializedBoundary(out ArrayList Boundary, Color color, int skip) =>
            this.GetSerializedBoundary(out Boundary, (float)color.ToArgb(), 0f, skip);

        public unsafe bool GetSerializedBoundary(out AutoTps_BoundaryList BoundaryList, float value, float delta, int skip)
        {
            delta = Math.Abs(delta);
            if (skip < 0)
            {
                throw new Exception("Invalid parameter! The 'skip' should be greater or equal to '0'.");
            }
            BoundaryList = new AutoTps_BoundaryList();
            AutoTps_Point* pointPtr = GetSerializedBoundary(this._data, this._width, this._height, value, delta, skip);
            if (pointPtr == null)
            {
                return false;
            }
            this._lines = pointPtr->x;
            pointPtr++;
            BoundaryList.BoundaryCount = this._lines;
            BoundaryList.Lines = new AutoTps_Line[this._lines];
            int index = 0;
            for (int i = 0; i < this._lines; i++)
            {
                int x = pointPtr->x;
                BoundaryList.Lines[index].Count = x;
                if (pointPtr->tag == 2)
                {
                    BoundaryList.Lines[index].LineType = AutoTps_LineType.CLOSED;
                }
                else
                {
                    BoundaryList.Lines[index].LineType = AutoTps_LineType.OPEN;
                }
                BoundaryList.Lines[index].points = new Point[x];
                pointPtr++;
                for (int j = 0; j < x; j++)
                {
                    BoundaryList.Lines[index].points[j].X = pointPtr->x;
                    BoundaryList.Lines[index].points[j].Y = pointPtr->y;
                    pointPtr++;
                }
                index++;
                this._points += x;
            }
            BoundaryList.PointCount = this._points;
            pointPtr = null;
            return true;
        }

        public bool GetSerializedBoundary(out ArrayList Boundary, Color color, float delta, int skip) =>
            this.GetSerializedBoundary(out Boundary, (float)color.ToArgb(), delta, skip);

        public unsafe bool GetSerializedBoundary(out AutoTps_Point[] Boundary, float value, float delta, int skip)
        {
            delta = Math.Abs(delta);
            if (skip < 0)
            {
                throw new Exception("Invalid parameter! The 'skip' should be greater or equal to '0'.");
            }
            Boundary = new AutoTps_Point[0];
            AutoTps_Point* pointPtr = GetSerializedBoundary(this._data, this._width, this._height, value, delta, skip);
            if (pointPtr == null)
            {
                return false;
            }
            ArrayList list = new ArrayList {
            pointPtr[0]
        };
            this._points = 1;
            this._lines = pointPtr->x;
            for (int i = 0; i < this._lines; i++)
            {
                int x = pointPtr->x;
                list.Add(pointPtr[0]);
                pointPtr++;
                for (int j = 0; j < x; j++)
                {
                    list.Add(pointPtr[0]);
                    pointPtr++;
                }
                this._points += x + 1;
            }
            Boundary = new AutoTps_Point[this._points];
            list.CopyTo(Boundary);
            list.Clear();
            list = null;
            pointPtr = null;
            return true;
        }

        public unsafe bool GetSerializedBoundary(out ArrayList Boundary, float value, float delta, int skip)
        {
            delta = Math.Abs(delta);
            if (skip < 0)
            {
                throw new Exception("Invalid parameter! The 'skip' should be greater or equal to '0'.");
            }
            Boundary = new ArrayList();
            AutoTps_Point* pointPtr = null;
            try
            {
                pointPtr = GetSerializedBoundary(this._data, this._width, this._height, value, delta, skip);
            }
            catch
            {
                return false;
            }
            if (pointPtr == null)
            {
                return false;
            }
            this._lines = pointPtr->x;
            pointPtr++;
            this._points = 0;
            for (int i = 0; i < this._lines; i++)
            {
                int x = pointPtr->x;
                Point[] pointArray = new Point[x];
                pointPtr++;
                for (int j = 0; j < x; j++)
                {
                    pointArray[j].X = pointPtr->x;
                    pointArray[j].Y = pointPtr->y;
                    pointPtr++;
                }
                this._points += x;
                Boundary.Add(pointArray);
            }
            pointPtr = null;
            return true;
        }

        [DllImport("IsolinePrj.dll")]
        public static extern unsafe AutoTps_Point* GetSerializedBoundary(float[] data, int cols, int rows, float value, float delta, int skip);
        public unsafe bool SetData(Bitmap source)
        {
            byte* numPtr;
            byte* numPtr2;
            byte* numPtr3;
            if (((source == null) || (source.Width < 2)) || (source.Height < 2))
            {
                return false;
            }
            BitmapData bitmapdata = new BitmapData();
            this._height = source.Height;
            this._width = source.Width;
            try
            {
                Rectangle rect = new Rectangle(0, 0, this._width, this._height);
                bitmapdata = source.LockBits(rect, ImageLockMode.ReadWrite, source.PixelFormat);
            }
            catch
            {
                source.UnlockBits(bitmapdata);
                throw new Exception("Load image failed! The image can not be resolved!");
            }
            int num = bitmapdata.Stride / this._width;
            switch (num)
            {
                case 4:
                case 3:
                    numPtr3 = (byte*)bitmapdata.Scan0;
                    numPtr2 = numPtr3 + 1;
                    numPtr = numPtr2 + 1;
                    break;

                default:
                    if (num == 1)
                    {
                        numPtr2 = numPtr3 = numPtr = (byte*)bitmapdata.Scan0;
                    }
                    else
                    {
                        source.UnlockBits(bitmapdata);
                        return false;
                    }
                    break;
            }
            int num2 = bitmapdata.Stride - (num * this._width);
            try
            {
                this._data = new float[this._width * this._height];
            }
            catch (Exception)
            {
                source.UnlockBits(bitmapdata);
                return false;
            }
            fixed (float* numRef = this._data)
            {
                float* numPtr4 = numRef;
                for (int i = 0; i < this._height; i++)
                {
                    for (int j = 0; j < this._width; j++)
                    {
                        numPtr4[0] = ((-16777216 + (numPtr[0] << 0x10)) + (numPtr2[0] << 8)) + numPtr3[0];
                        numPtr += num;
                        numPtr2 += num;
                        numPtr3 += num;
                        numPtr4 += 4;
                    }
                    numPtr += num2;
                    numPtr2 += num2;
                    numPtr3 += num2;
                }
                numPtr4 = null;
            }
            numPtr = numPtr2 = (byte*)(numPtr3 = null);
            bitmapdata = null;
            return true;
        }

        public bool SetData(string FileName)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(FileName);
            }
            catch
            {
                throw new Exception("Load image failed! The file: " + FileName.ToString() + " does not exist or can not be resolved!");
            }
            bool flag = this.SetData(bitmap);
            bitmap.Dispose();
            bitmap = null;
            return flag;
        }

        public bool SetData(float[] SourceData, int width, int height)
        {
            if ((width < 1) || (height < 1))
            {
                throw new Exception("Load image failed! The parameter 'width' and 'height' is not properly set!");
            }
            if ((width * height) > this._data.Length)
            {
                throw new Exception("Load image failed! The parameter  'width * height' is over data's upper bound !");
            }
            this._width = width;
            this._height = height;
            this._data = (float[])SourceData.Clone();
            return true;
        }

        // Properties
        public int Height =>
            this._height;

        public bool IsDisposed =>
            this._IsDisposed;

        public int lines =>
            this._lines;

        public int points =>
            this._points;

        public int Width =>
            this._width;

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        public struct AutoTps_BoundaryList
        {
            public int BoundaryCount;
            public int PointCount;
            public BoundaryTrack.AutoTps_Line[] Lines;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AutoTps_Line
        {
            public int Count;
            public BoundaryTrack.AutoTps_LineType LineType;
            public Point[] points;
        }

        public enum AutoTps_LineType
        {
            CLOSED,
            OPEN
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AutoTps_Point
        {
            public int x;
            public int y;
            public int tag;
        }
    }
}
