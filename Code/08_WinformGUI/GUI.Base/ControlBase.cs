using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace _08_WinformGUI
{
    [ComVisible(false)]
    public partial class ControlBase : UserControl
    {
        // Fields
        public EventHandler FullScreenEvent;
        public bool IsFullScreen;
        protected Font mFont = new Font("Tahoma", 9f);
        protected Button mFullScreenButton;
        private bool mIsControllable;
        private bool mIsFront;
        private bool mIsShowLayout;
        private bool mShowFullScreenButton;
        private string mUnitID = "";
        protected string mUnitName;

        // Events
        public event EventHandler ShiftToP1;

        // Methods
        public ControlBase()
        {
            this.InitializeComponent();
            this.mUnitID = Guid.NewGuid().ToString();
            base.AutoScaleMode = AutoScaleMode.None;
            this.InitButton();
        }

        private void Control_Load(object sender, EventArgs e)
        {
            this.SetPropertyForAR(this);
        }

        private void gbFullScreen_MouseLeave(object sender, EventArgs e)
        {
            this.mFullScreenButton.Visible = false;
        }

        private void gbFullScreenA_Click(object sender, EventArgs e)
        {
            this.SetFullScreen();
        }

        private void InitButton()
        {
            base.Controls.Remove(this.mFullScreenButton);
            this.mFullScreenButton = new Button();
            this.mFullScreenButton.Location = new Point(5, 5);
            this.mFullScreenButton.BackColor = Color.Transparent;
            this.mFullScreenButton.Size = new Size(60, 50);
            this.mFullScreenButton.Size = new Size(50, 50);
            this.mFullScreenButton.Text = "Full";
            //this.mFullScreenButton.Image = Resources.FullScreen;
            this.mFullScreenButton.ImageAlign = ContentAlignment.MiddleCenter;
            this.mFullScreenButton.Visible = false;
            this.mFullScreenButton.Click += new EventHandler(this.gbFullScreenA_Click);
            this.mFullScreenButton.LostFocus += new EventHandler(this.gbFullScreen_MouseLeave);
            base.Controls.Add(this.mFullScreenButton);
        }

        protected void mImgPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if ((((this.Cursor == Cursors.Default) && (e.X >= 0)) && ((e.X < 0x41) && (e.Y >= 0))) && ((e.Y < 0x37) && this.mShowFullScreenButton))
            {
                this.mFullScreenButton.Visible = true;
                this.mFullScreenButton.BringToFront();
                this.mFullScreenButton.Focus();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ((this.Font.Name != this.mFont.Name) || (this.Font.Size != this.mFont.Size))
            {
                this.Font = this.mFont;
                this.SetControlFont(this.mFont, this);
                base.OnPaint(e);
            }
        }

        protected void SetControlFont(Font tempFont, Control parentControl)
        {
            parentControl.Font = tempFont;
            foreach (Control control in parentControl.Controls)
            {
                this.SetControlFont(tempFont, control);
            }
        }

        public virtual void SetControllableState(bool isControllable)
        {
            this.mIsControllable = isControllable;
        }

        public void SetFullScreen()
        {
            if (this.FullScreenEvent != null)
            {
                this.FullScreenEvent(this, null);
            }
        }

        public void SetLocation(Rectangle rec)
        {
            base.Visible = false;
            base.Location = rec.Location;
            base.Size = rec.Size;
            base.Visible = true;
        }

        public void SetLocation(int x, int y, Size size)
        {
            base.Location = new Point(x, y);
            base.Size = size;
        }

        private void SetPropertyForAR(Control parentControl)
        {
            if (parentControl != null)
            {
                if ((parentControl is TextBox) || (parentControl is ComboBox))
                {
                    parentControl.AccessibleDescription = "AcDescription" + parentControl.Name;
                }
                foreach (Control control in parentControl.Controls)
                {
                    if (control != null)
                    {
                        this.SetPropertyForAR(control);
                    }
                }
            }
        }

        public void ShiftToMainPanel()
        {
            if (this.ShiftToP1 != null)
            {
                this.ShiftToP1(this, null);
            }
        }

        // Properties
        public bool IsControllabe =>
            this.mIsControllable;

        public bool IsFrontDisplay
        {
            get =>
                this.mIsFront;
            set
            {
                this.mIsFront = value;
            }
        }

        public bool IsLayoutShow
        {
            get =>
                this.mIsShowLayout;
            set
            {
                this.mIsShowLayout = value;
            }
        }

        public bool ShowFullScreenButton
        {
            get =>
                this.mShowFullScreenButton;
            set
            {
                this.mShowFullScreenButton = value;
            }
        }

        public string UnitID =>
            this.mUnitID;

        public string UnitName
        {
            get =>
                this.mUnitName;
            set
            {
                this.mUnitName = value;
            }
        }
    }
}
