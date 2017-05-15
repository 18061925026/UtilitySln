using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphsPrj
{
    public abstract class GraphGeometry : GraphBase
    {
        // Fields
        protected float mDetectRadius = 5f;
        protected Brush mDrawBrush = new SolidBrush(Color.Red);//Default:SolidBrush
        protected Pen mDrawPen = new Pen(Color.Red);
        protected bool mMovable = false;
        protected bool mDetectable = false;
        protected bool mIsSelected = false;
        protected float mPixelSize = 1.0F;

        // Properties
        public bool Movable
        {
            get =>
                this.mMovable;
            set
            {
                this.mMovable = value;
            }
        }
        public bool Detectable
        {
            get { return mDetectable; }
            set { mDetectable = value; }
        }
        public bool IsSelected
        {
            get { return mIsSelected; }
            set { mIsSelected = value; }
        }
        public float DetectRadius
        {
            get =>
                this.mDetectRadius;
            set
            {
                this.mDetectRadius = value;
            }
        }

        public Brush DrawBrush
        {
            get =>
                this.mDrawBrush;
            set
            {
                this.mDrawBrush = value;
            }
        }

        public Pen DrawPen
        {
            get =>
                this.mDrawPen;
            set
            {
                this.mDrawPen = value;
            }
        }
        public float PixelSize
        {
            get { return mPixelSize; }
            set { mPixelSize = value; }
        }

        public virtual bool IsEmpty =>
            false;
        // Methods
        public GraphGeometry() : base() { }
        public GraphGeometry(GraphGeometry graph) : base(graph)
        {
            this.mDrawPen = graph.mDrawPen;
            this.mDrawBrush = graph.mDrawBrush;
            this.mDetectable = graph.mDetectable;
            this.mDetectRadius = graph.mDetectRadius;
            this.mMovable = graph.mMovable;
            this.mIsSelected = graph.mIsSelected;
            this.mPixelSize = graph.mPixelSize;
        }
        ~GraphGeometry()
        {
            this.Dispose();
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        public override bool Draw(Graphics gp, Matrix jacobiMatrix)
        {
            if ((!base.mVisible || (gp == null)) || (jacobiMatrix == null))
            {
                return false;
            }
            base.mJacobiMatrix = jacobiMatrix;
            return true;
        }
    }
}
