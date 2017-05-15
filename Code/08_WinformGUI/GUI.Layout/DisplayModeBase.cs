using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_WinformGUI
{
    public class DisplayModeBase
    {
        // Fields
        public bool Fullscreen;
        private object lockobj = new object();
        protected Brush mBrush = Brushes.LimeGreen;
        protected ControlBase mCurrentFullScreenControl;
        protected Font mFont = new Font("Arial", 10f);
        protected Rectangle[] mPositions;
        protected Rectangle P_Save = Rectangle.Empty;
        protected Rectangle P1_Shift = Rectangle.Empty;
        protected Rectangle P2_Shift = Rectangle.Empty;

        // Methods
        public bool CheckIsMainPosAndNoLastPos(ControlBase ctrl) =>
            (this.Fullscreen || ((ctrl.Location == this.mPositions[0].Location) && this.P1_Shift.IsEmpty));

        protected Point GetLocationOfString(string s, Rectangle rect)
        {
            SizeF stringSize = this.GetStringSize(s);
            float num = (rect.Width - stringSize.Width) / 2f;
            float num2 = (rect.Height - stringSize.Height) / 2f;
            return new Point(rect.X + ((int)num), rect.Y + ((int)num2));
        }

        public virtual Bitmap GetShowImage(int width, int height, List<string> strs) =>
            null;

        protected SizeF GetStringSize(string s) =>
            Graphics.FromImage(new Bitmap(1, 1)).MeasureString(s, this.mFont);

        public virtual int P_Count() =>
            0;

        public Rectangle ResetP1Shift(Rectangle rec, bool reset)
        {
            if (reset)
            {
                this.P1_Shift = rec;
            }
            return this.P1_Shift;
        }

        public virtual void SetControlPosition(List<ControlBase> ctrls, Control viewcontainer)
        {
            for (int i = 0; i < ctrls.Count; i++)
            {
                if (this.mPositions.Length > i)
                {
                    ctrls[i].SetLocation(this.mPositions[i]);
                }
            }
        }

        public void SetFullScreen(ControlBase ctrl, Control viewcontainer)
        {
            lock (this.lockobj)
            {
                if (viewcontainer == null)
                {
                    return;
                }
                if (this.Fullscreen)
                {
                    if (this.mCurrentFullScreenControl == ctrl)
                    {
                        if (this.P_Save != Rectangle.Empty)
                        {
                            ctrl.SetLocation(this.P_Save);
                            this.P_Save = Rectangle.Empty;
                        }
                        this.Fullscreen = !this.Fullscreen;
                        this.mCurrentFullScreenControl = null;
                    }
                    else
                    {
                        if (ctrl.Size == viewcontainer.Size)
                        {
                            return;
                        }
                        if (this.P_Save != Rectangle.Empty)
                        {
                            this.mCurrentFullScreenControl.SetLocation(this.P_Save);
                        }
                        this.P_Save = new Rectangle(ctrl.Location, ctrl.Size);
                        Rectangle rec = new Rectangle(new Point(0, 0), viewcontainer.Size);
                        ctrl.BringToFront();
                        ctrl.SetLocation(rec);
                        this.mCurrentFullScreenControl = ctrl;
                    }
                }
                else
                {
                    if (ctrl.Size == viewcontainer.Size)
                    {
                        return;
                    }
                    this.P_Save = new Rectangle(ctrl.Location, ctrl.Size);
                    Rectangle rectangle2 = new Rectangle(new Point(0, 0), viewcontainer.Size);
                    ctrl.BringToFront();
                    ctrl.SetLocation(rectangle2);
                    this.Fullscreen = !this.Fullscreen;
                    this.mCurrentFullScreenControl = ctrl;
                }
            }
            ctrl.IsFullScreen = this.Fullscreen;
        }

        public virtual void ShiftToP1(ControlBase ctrl, Control viewcontainer)
        {
            if ((viewcontainer != null) && !this.Fullscreen)
            {
                Size size = viewcontainer.Size;
                Point pt = new Point(this.mPositions[0].X + (this.mPositions[0].Width / 2), this.mPositions[0].Y + (this.mPositions[0].Height / 2));
                Control childAtPoint = viewcontainer.GetChildAtPoint(pt);
                if (childAtPoint is ControlBase)
                {
                    ControlBase ctl = childAtPoint as ControlBase;
                    if ((ctrl.Location == this.mPositions[0].Location) && !this.P1_Shift.IsEmpty)
                    {
                        Point point2 = new Point(this.P1_Shift.X + (this.P1_Shift.Width / 2), this.P1_Shift.Y + (this.P1_Shift.Height / 2));
                        Control control2 = viewcontainer.GetChildAtPoint(point2);
                        if (control2 is ControlBase)
                        {
                            ctrl.SetLocation(this.P1_Shift);
                            (control2 as ControlBase).SetLocation(this.mPositions[0]);
                        }
                    }
                    else if ((ctrl.Location != this.mPositions[0].Location) || !this.P1_Shift.IsEmpty)
                    {
                        this.P1_Shift = new Rectangle(ctrl.Location, ctrl.Size);
                        ctrl.SetLocation(this.mPositions[0]);
                        if (viewcontainer.Contains(ctl))
                        {
                            ctl.SetLocation(this.P1_Shift);
                        }
                    }
                }
            }
        }
    }
}
