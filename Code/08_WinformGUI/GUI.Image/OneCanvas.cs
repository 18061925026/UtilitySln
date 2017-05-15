using GraphsPrj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class OneCanvas : IDisposable
    {
        // Fields
        private Bitmap mBitmap;
        private List<GraphBase> mGraphs;

        // Methods
        public OneCanvas(List<GraphBase> graphs, Size size)
        {
            this.mGraphs = new List<GraphBase>();
            this.mGraphs = graphs;
            this.InitBitmap(size);
        }

        public OneCanvas(GraphBase graph, Size size)
        {
            this.mGraphs = new List<GraphBase>();
            this.AddGraph(graph);
            this.InitBitmap(size);
        }

        public void AddGraph(GraphBase graph)
        {
            if ((graph != null) && !this.mGraphs.Contains(graph))
            {
                if (this.mBitmap != null)
                {
                    graph.Range = new RectangleF(new PointF(0, 0), this.mBitmap.Size);
                }
                this.mGraphs.Add(graph);
            }
        }

        public void AddGraphs(List<GraphBase> graphs)
        {
            if (graphs != null)
            {
                foreach (GraphBase base2 in graphs)
                {
                    this.AddGraph(base2);
                }
            }
        }

        public void DeleteGraph(GraphBase graph)
        {
            if ((graph != null) && this.mGraphs.Contains(graph))
            {
                this.mGraphs.Remove(graph);
            }
        }

        public void DeleteGraphs(List<GraphBase> graphs)
        {
            if (graphs != null)
            {
                foreach (GraphBase base2 in graphs)
                {
                    this.DeleteGraph(base2);
                }
            }
        }

        public void Dispose()
        {
            if (this.mBitmap != null)
            {
                this.mBitmap.Dispose();
                this.mBitmap = null;
            }
            for (int i = 0; i < this.mGraphs.Count; i++)
            {
                this.mGraphs[i].Dispose();
                this.mGraphs[i] = null;
            }
            this.mGraphs.Clear();
        }

        public void DrawGraphs(Matrix jacobiMatrix)
        {
            Graphics gp = Graphics.FromImage(this.mBitmap);
            gp.Clear(Color.Transparent);
            foreach (GraphBase base2 in this.mGraphs)
            {
                base2.Draw(gp, jacobiMatrix);
            }
            gp.Dispose();
            gp = null;
        }

        public GraphBase GetCurrentGraph(object sender, MouseEventArgs e)
        {
            Cursor cursor = Cursors.Default;
            foreach (GraphBase base2 in this.mGraphs)
            {
                cursor = base2.MouseDetect(sender, e);
                if (Cursors.Default != cursor)
                {
                    return base2;
                }
            }
            return null;
        }

        public GraphBase GetGraph(string graphName)
        {
            foreach (GraphBase base2 in this.mGraphs)
            {
                if (base2.Name == graphName)
                {
                    return base2;
                }
            }
            return null;
        }

        public void InitBitmap(Size size)
        {
            if (((Size.Empty != size) && (size.Width != 0)) && (size.Height != 0))
            {
                if (this.mBitmap != null)
                {
                    this.mBitmap.Dispose();
                    this.mBitmap = null;
                }
                this.mBitmap = new Bitmap(size.Width, size.Height);
                this.ResetGraphRange(size);
            }
        }

        public void InsertGraph(int index, GraphBase graph)
        {
            if ((graph != null) && !this.mGraphs.Contains(graph))
            {
                if (this.mBitmap != null)
                {
                    graph.Range = new RectangleF(new PointF(0, 0), this.mBitmap.Size);
                }
                if ((index >= 0) && (index <= (this.mGraphs.Count - 1)))
                {
                    this.mGraphs.Insert(index, graph);
                }
                else
                {
                    this.mGraphs.Add(graph);
                }
            }
        }

        public void ResetGraphRange(Size size)
        {
            for (int i = 0; i < this.mGraphs.Count; i++)
            {
                this.mGraphs[i].Range = new RectangleF(new PointF(0, 0), size);
            }
        }

        public void ShowOrHideGraphs(bool isShow)
        {
            foreach (GraphBase base2 in this.mGraphs)
            {
                base2.Visible = isShow;
            }
        }

        public void Sort()
        {
            if (this.mGraphs.Count > 0)
            {
                this.mGraphs.Sort();
            }
        }

        // Properties
        public Bitmap Bitmap =>
            this.mBitmap;

        public int Count =>
            this.mGraphs.Count;

        public List<GraphBase> Graphs =>
            this.mGraphs;
    }
}
