using GraphsPrj;
using LogsPrj;
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
    public class ImageViewBase : ControlBase, IDisposable
    {
        // Fields
        protected SortedDictionary<LayerType, OneCanvas> mCanvases = new SortedDictionary<LayerType, OneCanvas>();
        protected bool mCanZoom;
        private GraphBase mCurrentGraph = null;
        private Size mImageViewSize = new Size(512, 512);
        protected Matrix mJacobiMatrix = new Matrix();
        protected PictureBox mPictureBox = new PictureBox();
        private float mZoomMaxScale = 10f;
        private float mZoomMinScale = 0.1f;
        protected Panel panelMain = new Panel();

        // Methods
        public ImageViewBase()
        {
            this.Initialize();
        }

        public void AddGraph(GraphBase graph)
        {
            if (graph != null)
            {
                if (this.mCanvases.ContainsKey(graph.LayerType))
                {
                    this.mCanvases[graph.LayerType].AddGraph(graph);
                }
                else
                {
                    OneCanvas canvas = new OneCanvas(graph, this.mPictureBox.Size);
                    this.mCanvases.Add(graph.LayerType, canvas);
                }
                this.mCanvases[graph.LayerType].Sort();
            }
        }

        protected void AddPictureBoxEvent()
        {
            this.mPictureBox.MouseDown += new MouseEventHandler(this.mPictureBox_MouseDown);
            this.mPictureBox.MouseDoubleClick += new MouseEventHandler(this.mPictureBox_MouseDoubleClick);
            this.mPictureBox.MouseUp += new MouseEventHandler(this.mPictureBox_MouseUp);
            this.mPictureBox.MouseMove += new MouseEventHandler(this.mPictureBox_MouseMove);
            this.mPictureBox.MouseWheel += new MouseEventHandler(this.mPictureBox_MouseWheel);
            this.mPictureBox.MouseEnter += new EventHandler(this.mPictureBox_MouseEnter);
            this.mPictureBox.MouseLeave += new EventHandler(this.mPictureBox_MouseLeave);
            this.mPictureBox.Paint += new PaintEventHandler(this.mPictureBox_Paint);
            this.mPictureBox.SizeChanged += new EventHandler(this.mPictureBox_SizeChanged);
            this.mPictureBox.PreviewKeyDown += new PreviewKeyDownEventHandler(this.mPictureBox_PreviewKeyDown);
            this.mPictureBox.LostFocus += new EventHandler(this.mPictureBox_LostFocus);
        }

        protected void ChangeGraphVisibleStatus<T>(LayerType layerType, bool visible) where T : GraphBase
        {
            T graph = this.GetGraph<T>(layerType);
            if (graph != null)
            {
                graph.Visible = visible;
                this.RefreshGraph(layerType);
            }
        }

        public void DeleteGraph(GraphBase graph)
        {
            if (this.mCanvases.ContainsKey(graph.LayerType))
            {
                this.mCanvases[graph.LayerType].DeleteGraph(graph);
            }
        }

        public void DeleteGraphs(LayerType layerType)
        {
            if (this.mCanvases.ContainsKey(layerType))
            {
                this.mCanvases[layerType].Dispose();
                this.mCanvases.Remove(layerType);
            }
        }

        public virtual void Dispose()
        {
            this.RemovePictureBoxEvent();
            this.mPictureBox.Dispose();
            this.mPictureBox = null;
            foreach (LayerType type in this.mCanvases.Keys)
            {
                this.mCanvases[type].Dispose();
            }
            this.mCanvases.Clear();
        }

        public void DrawGraph(LayerType layerType)
        {
            if (this.mCanvases.ContainsKey(layerType))
            {
                this.mCanvases[layerType].DrawGraphs(this.mJacobiMatrix);
            }
        }

        protected void DrawGraphs()
        {
            List<LayerType> list = new List<LayerType>();
            list.AddRange(this.mCanvases.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                this.mCanvases[list[i]].DrawGraphs(this.mJacobiMatrix);
            }
        }

        public void DrawToPictureBox()
        {
            if ((this.mPictureBox.Image == null) || (this.mPictureBox.Image.Size != this.mPictureBox.Size))
            {
                this.mPictureBox.Image = null;
                this.mPictureBox.Image = new Bitmap(this.mPictureBox.Width, this.mPictureBox.Height);
            }
            Graphics graphics = Graphics.FromImage(this.mPictureBox.Image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.Transparent);
            List<LayerType> list = new List<LayerType>();
            list.AddRange(this.mCanvases.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                graphics.DrawImage(this.mCanvases[list[i]].Bitmap, new Point(0, 0));
            }
            this.mPictureBox.Refresh();
            graphics.Dispose();
        }

        public T GetGraph<T>() where T : GraphBase =>
            this.GetGraph<T>(LayerType.None);

        public GraphBase GetGraph(string graphName)
        {
            foreach (LayerType type in this.mCanvases.Keys)
            {
                GraphBase graph = this.mCanvases[type].GetGraph(graphName);
                if (graph != null)
                {
                    return graph;
                }
            }
            return null;
        }

        public T GetGraph<T>(LayerType arg) where T : GraphBase
        {
            if (arg == LayerType.None)
            {
                foreach (LayerType type in this.mCanvases.Keys)
                {
                    foreach (GraphBase base2 in this.mCanvases[type].Graphs)
                    {
                        if (base2 is T)
                        {
                            return (base2 as T);
                        }
                    }
                }
            }
            else
            {
                if (!this.mCanvases.ContainsKey(arg))
                {
                    return default(T);
                }
                foreach (GraphBase base3 in this.mCanvases[arg].Graphs)
                {
                    if (base3 is T)
                    {
                        return (base3 as T);
                    }
                }
            }
            return default(T);
        }

        public List<GraphBase> GetGraphs(LayerType layerType)
        {
            if (this.mCanvases.ContainsKey(layerType))
            {
                return this.mCanvases[layerType].Graphs;
            }
            return null;
        }

        public Cursor GetPictureBoxCursor() =>
            this.mPictureBox.Cursor;

        private void Initialize()
        {
            this.InitView();
            this.RemovePictureBoxEvent();
            this.AddPictureBoxEvent();
            this.ResetJacobiMatrix();
        }

        private void InitView()
        {
            this.panelMain.Dock = DockStyle.Fill;
            this.mPictureBox.Dock = DockStyle.Fill;
            this.panelMain.Controls.Add(this.mPictureBox);
            base.Controls.Add(this.panelMain);
        }

        public void InsertGraph(int index, GraphBase graph)
        {
            if (graph != null)
            {
                graph.Priority = index;
                if (this.mCanvases.ContainsKey(graph.LayerType))
                {
                    this.mCanvases[graph.LayerType].InsertGraph(index, graph);
                }
                else
                {
                    OneCanvas canvas = new OneCanvas(graph, this.mPictureBox.Size);
                    this.mCanvases.Add(graph.LayerType, canvas);
                }
                this.mCanvases[graph.LayerType].Sort();
            }
        }

        protected virtual void mPictureBox_LostFocus(object sender, EventArgs e)
        {
        }

        protected virtual void mPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        protected virtual void mPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.mCurrentGraph != null)
            {
                this.mCurrentGraph.MouseDown(sender, e);
            }
        }

        protected virtual void mPictureBox_MouseEnter(object sender, EventArgs e)
        {
            this.mPictureBox.Focus();
            if (this.mCurrentGraph != null)
            {
                this.mCurrentGraph.MouseEnter(sender, e);
            }
        }

        protected virtual void mPictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (this.mCurrentGraph != null)
            {
                this.mCurrentGraph.MouseLeave(sender, e);
            }
        }

        protected virtual void mPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            base.mImgPictureBox_MouseMove(sender, e);
            if (this.mCurrentGraph == null)
            {
                this.mPictureBox.Cursor = Cursors.Default;
            }
            else
            {
                this.mPictureBox.Cursor = this.mCurrentGraph.MouseDetect(sender, e);
                this.mCurrentGraph.MouseMove(sender, e);
            }
        }

        protected virtual void mPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.mCurrentGraph != null)
            {
                this.mCurrentGraph.MouseUp(sender, e);
            }
        }

        protected virtual void mPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.None) && this.mCanZoom)
            {
                new PointF((float)e.X, (float)e.Y);
                if (((e.X >= 0) && (e.X <= base.Width)) && ((e.Y >= 0) && (e.Y <= base.Height)))
                {
                    this.ZoomImage(new PointF((float)e.X, (float)e.Y), e.Delta > 0);
                }
            }
        }

        protected virtual void mPictureBox_Paint(object sender, PaintEventArgs e)
        {
        }

        protected virtual void mPictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        protected virtual void mPictureBox_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (((this.mPictureBox.Size != Size.Empty) && (this.mPictureBox.Size.Width != 0)) && (this.mPictureBox.Size.Height != 0))
                {
                    this.ResetJacobiMatrix();
                    this.ResetCanvasSize();
                    this.RefreshGraphs();
                    this.mPictureBox.Refresh();
                }
            }
            catch (Exception)
            {
            }
        }

        public void RefreshGraph(LayerType layerType)
        {
            this.DrawGraph(layerType);
            this.DrawToPictureBox();
        }

        public void RefreshGraphs()
        {
            this.DrawGraphs();
            this.DrawToPictureBox();
        }

        protected void RemovePictureBoxEvent()
        {
            this.mPictureBox.MouseDown -= new MouseEventHandler(this.mPictureBox_MouseDown);
            this.mPictureBox.MouseDoubleClick -= new MouseEventHandler(this.mPictureBox_MouseDoubleClick);
            this.mPictureBox.MouseUp -= new MouseEventHandler(this.mPictureBox_MouseUp);
            this.mPictureBox.MouseMove -= new MouseEventHandler(this.mPictureBox_MouseMove);
            this.mPictureBox.MouseWheel -= new MouseEventHandler(this.mPictureBox_MouseWheel);
            this.mPictureBox.MouseEnter -= new EventHandler(this.mPictureBox_MouseEnter);
            this.mPictureBox.MouseLeave -= new EventHandler(this.mPictureBox_MouseLeave);
            this.mPictureBox.Paint -= new PaintEventHandler(this.mPictureBox_Paint);
            this.mPictureBox.SizeChanged -= new EventHandler(this.mPictureBox_SizeChanged);
            this.mPictureBox.PreviewKeyDown -= new PreviewKeyDownEventHandler(this.mPictureBox_PreviewKeyDown);
            this.mPictureBox.LostFocus -= new EventHandler(this.mPictureBox_LostFocus);
        }

        private void ResetCanvasSize()
        {
            List<LayerType> list = new List<LayerType>();
            list.AddRange(this.mCanvases.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                this.mCanvases[list[i]].InitBitmap(this.mPictureBox.Size);
            }
        }

        protected void ResetJacobiMatrix()
        {
            this.mJacobiMatrix = GUIHelper.CalculateJacobiMatrix((SizeF)this.mPictureBox.Size, (SizeF)this.mImageViewSize);
        }

        public void ResetOriginalSize()
        {
            this.mPictureBox_SizeChanged(null, null);
        }

        public void SetGraphVisible<T>(bool visible) where T : GraphBase
        {
            T graph = this.GetGraph<T>();
            if (graph != null)
            {
                graph.Visible = visible;
            }
        }

        public void SetGraphVisible(LayerType layerType, bool visible)
        {
            List<GraphBase> graphs = this.GetGraphs(layerType);
            if (graphs != null)
            {
                foreach (GraphBase base2 in graphs)
                {
                    base2.Visible = visible;
                }
            }
        }

        public void SetGraphVisible<T>(LayerType layerType, bool visible) where T : GraphBase
        {
            T graph = this.GetGraph<T>(layerType);
            if (graph != null)
            {
                graph.Visible = visible;
            }
        }

        public void SetPictureBoxCursor(Cursor cursor)
        {
            this.mPictureBox.Cursor = cursor;
        }

        protected void ZoomImage(PointF point, bool isZoom)
        {
            try
            {
                if (this.ZoomMouseWheel(point, isZoom))
                {
                    this.DrawGraphs();
                    this.DrawToPictureBox();
                    this.mPictureBox.Refresh();
                }
            }
            catch (Exception exception)
            {
                LogsManager.Instance.GetLog<UtilityLog>().Error(this.ToString() + " ViewBase Wheel Error Info: " + exception.Message);
            }
        }

        private bool ZoomMouseWheel(PointF curPoint, bool toBig)
        {
            float num = this.mJacobiMatrix.Elements[0];
            if ((toBig && (num >= this.mZoomMaxScale)) || (!toBig && (num <= this.mZoomMinScale)))
            {
                return false;
            }
            float scaleX = toBig ? 1.1f : 0.9f;
            float mZoomMaxScale = num * scaleX;
            if (mZoomMaxScale > this.mZoomMaxScale)
            {
                mZoomMaxScale = this.mZoomMaxScale;
            }
            else if (mZoomMaxScale < this.mZoomMinScale)
            {
                mZoomMaxScale = this.mZoomMinScale;
            }
            PointF tf = curPoint;
            Matrix matrix = this.mJacobiMatrix.Clone();
            matrix.Invert();
            PointF[] pts = new PointF[] { curPoint };
            matrix.TransformPoints(pts);
            PointF tf2 = pts[0];
            this.mJacobiMatrix.Scale(scaleX, scaleX);
            pts[0] = tf2;
            this.mJacobiMatrix.TransformPoints(pts);
            PointF tf3 = pts[0];
            float offsetX = tf.X - tf3.X;
            float offsetY = tf.Y - tf3.Y;
            this.mJacobiMatrix.Translate(offsetX, offsetY, MatrixOrder.Append);
            return true;
        }

        // Properties
        public bool CanZoom
        {
            get =>
                this.mCanZoom;
            set
            {
                this.mCanZoom = value;
            }
        }

        public GraphBase CurrentGraph
        {
            get =>
                this.mCurrentGraph;
            set
            {
                this.mCurrentGraph = value;
            }
        }

        public Size ImageViewSize
        {
            get =>
                this.mImageViewSize;
            set
            {
                this.mImageViewSize = value;
                this.ResetJacobiMatrix();
            }
        }

        public Matrix JacobiMatrix
        {
            get =>
                this.mJacobiMatrix;
            set
            {
                this.mJacobiMatrix = value;
            }
        }

        public float ZoomMaxScale
        {
            get =>
                this.mZoomMaxScale;
            set
            {
                if ((this.mCanZoom && (value > 0f)) && (value > this.mZoomMinScale))
                {
                    this.mZoomMaxScale = value;
                }
            }
        }

        public float ZoomMinScale
        {
            get =>
                this.mZoomMinScale;
            set
            {
                if ((this.mCanZoom && (value > 0f)) && (value < this.mZoomMaxScale))
                {
                    this.mZoomMinScale = value;
                }
            }
        }
    }
}
