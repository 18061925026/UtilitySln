using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphsPrj
{
    public abstract class GraphPoint : GraphGeometry
    {
        // Fields
        protected bool mCanDrawName;
        protected PointF mPointCoord;

        // Methods
        public GraphPoint(PointF point)
        {
            this.mPointCoord = point;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override bool Draw(Graphics gp, Matrix jacobiMatrix)
        {
            if (!base.Draw(gp, jacobiMatrix) || this.mPointCoord.IsEmpty)
            {
                return false;
            }
            base.mJacobiMatrix = null;
            base.mJacobiMatrix = jacobiMatrix;
            PointF point = GraphsHelper.TransformPoint(this.mPointCoord, base.mJacobiMatrix);
            return this.Draw(gp, point);
        }

        protected abstract bool Draw(Graphics gp, PointF point);
        protected void DrawName(Graphics gp, PointF pt)
        {
            if (this.mCanDrawName && (base.mName != null))
            {
                float num = (base.mJacobiMatrix.Elements[0] < 0f) ? 1f : base.mJacobiMatrix.Elements[0];
                PointF point = new PointF(pt.X - (10f * num), pt.Y + (10f * num));
                Font font = new Font(new FontFamily("Tahoma"), 9f * num);
                gp.DrawString(base.mName, font, base.mDrawBrush, point);
            }
        }

        ~GraphPoint()
        {
            this.Dispose();
        }

        // Properties
        public bool CanDrawName
        {
            get =>
                this.mCanDrawName;
            set
            {
                this.mCanDrawName = value;
            }
        }

        public PointF PointCoord =>
            this.mPointCoord;
    }
}
