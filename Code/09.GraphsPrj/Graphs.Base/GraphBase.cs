using LogsPrj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphsPrj
{
    public abstract class GraphBase : IDisposable, IComparable<GraphBase>
    {
        // Nested Types
        public delegate void MouseHandler(object sender);
        // Events
        public event MouseHandler GraphMouseDetectEvent;
        public event MouseHandler GraphMouseDownEvent;
        public event MouseHandler GraphMouseMoveEvent;
        public event MouseHandler GraphMouseUpEvent;

        // Fields
        protected static long mCount = 0L;
        protected Matrix mJacobiMatrix = new Matrix();
        protected LayerType mLayerType;
        protected bool mShowName = false;
        protected string mName = "";
        protected int mPriority;
        protected RectangleF mRange = new RectangleF();
        protected bool mVisible = true;
        private long mUID = 0;
        protected Font mFont = new Font("Tahoma", 9);


        // Properties
        public long ID
        {
            get { return mUID; }
        }

        public Matrix JacobiMatrix =>
            this.mJacobiMatrix;

        public LayerType LayerType =>
            this.mLayerType;

        public string Name
        {
            get =>
                this.mName;
            set
            {
                this.mName = value;
            }
        }

        public bool ShowName
        {
            get { return mShowName; }
            set { mShowName = value; }
        }

        public int Priority
        {
            get =>
                this.mPriority;
            set
            {
                this.mPriority = value;
            }
        }
        public RectangleF Range
        {
            get =>
                this.mRange;
            set
            {
                this.mRange = value;
            }
        }

        public bool Visible
        {
            get =>
                this.mVisible;
            set
            {
                this.mVisible = value;
            }
        }
        public Font Font
        {
            get { return mFont; }
            set { mFont = value; }
        }
        // Methods
        public GraphBase()
        {
            mCount++;
            mUID = mCount;
        }
        public GraphBase(GraphBase graph) : this()
        {
            this.GraphMouseDetectEvent = graph.GraphMouseDetectEvent;
            this.GraphMouseDownEvent = graph.GraphMouseDownEvent;
            this.GraphMouseMoveEvent = graph.GraphMouseMoveEvent;
            this.GraphMouseUpEvent = graph.GraphMouseUpEvent;
            this.mName = graph.mName;
            this.ShowName = graph.ShowName;
            this.Visible = graph.Visible;
            this.mJacobiMatrix = graph.mJacobiMatrix;
            this.mLayerType = graph.mLayerType;
            this.mRange = graph.mRange;
            this.mPriority = graph.mPriority;
            this.mFont = graph.mFont;

        }
        ~GraphBase()
        {
            this.Dispose();
            mCount -= 1;
        }
        public virtual GraphBase Clone() =>
            null;

        public int CompareTo(GraphBase target)
        {
            int num = 0;
            try
            {
                num = this.mPriority.CompareTo(target.mPriority);
            }
            catch (Exception exception)
            {
                LogsManager.Instance.GetLog<UtilityLog>().Error(this.ToString() + ": ComareTo Error!");
                throw new Exception("GraphBase Compare Exception: ", exception.InnerException);
            }
            return num;
        }

        public virtual void Dispose()
        {
        }

        public abstract bool Draw(Graphics gp, Matrix jacobiMatrix);

        public virtual Cursor MouseDetect(object sender, MouseEventArgs e) =>
            Cursors.Default;

        public virtual void MouseDown(object sender, MouseEventArgs e)
        {
        }

        public virtual void MouseEnter(object sender, EventArgs e)
        {
        }

        public virtual void MouseLeave(object sender, EventArgs e)
        {
        }

        public virtual void MouseMove(object sender, MouseEventArgs e)
        {
        }

        public virtual void MouseUp(object sender, MouseEventArgs e)
        {
        }
        protected void OnGraphMouseDetectEvent(object sender)
        {
            if (null == GraphMouseDetectEvent)
            {
                GraphMouseDetectEvent(sender);
            }
        }
        protected void OnGraphMouseDownEvent(object sender)
        {
            if (GraphMouseDownEvent != null)
            {
                GraphMouseDownEvent(sender);
            }
        }

        protected void OnGraphMouseMoveEvent(object sender)
        {
            if (GraphMouseMoveEvent != null)
            {
                GraphMouseMoveEvent(sender);
            }
        }

        protected void OnGraphMouseUpEvent(object sender)
        {
            if (GraphMouseUpEvent != null)
            {
                GraphMouseUpEvent(sender);
            }
        }

    }
}
