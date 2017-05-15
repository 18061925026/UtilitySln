using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace _08_StripPrj
{
    public abstract class StatusBase : IDisposable
    {
        // Fields
        protected System.Timers.Timer mShowTimer;
        protected StatusStrip mStatusStrip;

        // Events
        public event EventHandler ClickStatusItem;

        // Methods
        public StatusBase(StatusStrip statusStrip)
        {
            this.mStatusStrip = statusStrip;
            if (this.mStatusStrip == null)
            {
                this.mStatusStrip = new StatusStrip();
            }
        }

        protected abstract void AssembleStatus();
        public void Clear()
        {
            foreach (ToolStripItem item in this.mStatusStrip.Items)
            {
                if (item is ToolStripStatusLabel)
                {
                    ToolStripStatusLabel label = item as ToolStripStatusLabel;
                    if (((label.Name != "User") && (label.Name != "PatientInfo")) && (label.Name != "Time"))
                    {
                        label.Text = "";
                    }
                }
            }
        }

        protected ToolStripStatusLabel CreateLabelItem(string name) =>
            new ToolStripStatusLabel(string.Empty, null, null, name)
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Alignment = ToolStripItemAlignment.Right,
                AutoSize = false,
                Width = 220,
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Flat
            };

        protected ToolStripProgressBar CreateProgressBarItem(string name) =>
            new ToolStripProgressBar(name)
            {
                TextAlign = ContentAlignment.MiddleCenter,
                Alignment = ToolStripItemAlignment.Right,
                AutoSize = false,
                Width = 220
            };

        protected void CreateTimeItem()
        {
            this.mStatusStrip.Items.Add(this.CreateLabelItem("Time"));
            this.mShowTimer = new System.Timers.Timer();
            this.mShowTimer.Interval = 1000;
            this.StartShowTime();
        }

        protected void CreateUserItem()
        {
            this.mStatusStrip.Items.Add(this.CreateLabelItem("User"));
        }

        public virtual void Dispose()
        {
            if (this.mShowTimer != null)
            {
                this.EndShowTime();
                this.mShowTimer = null;
            }
        }

        private void EndShowTime()
        {
            this.mShowTimer.Elapsed -= new ElapsedEventHandler(this.mShowTimer_Elapsed);
            this.mShowTimer.Stop();
            if (this.mStatusStrip.Items.ContainsKey("Time"))
            {
                this.mStatusStrip.Items["Time"].Text = "";
            }
        }

        private void mShowTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.mStatusStrip.InvokeRequired)
            {
                while (!this.mStatusStrip.IsHandleCreated)
                {
                    if (this.mStatusStrip.Disposing || this.mStatusStrip.IsDisposed)
                    {
                        return;
                    }
                }
                try
                {
                    this.mStatusStrip.Invoke(new ElapsedEventHandler(this.mShowTimer_Elapsed), new object[] { sender, e });
                }
                catch (Exception)
                {
                }
            }
            else if (this.mStatusStrip.Items.ContainsKey("Time"))
            {
                this.mStatusStrip.Items["Time"].Text = DateTime.Now.ToString();
                this.mStatusStrip.Refresh();
            }
        }

        private void OnClickStatusItem(object sender, EventArgs e)
        {
            if (this.ClickStatusItem != null)
            {
                this.ClickStatusItem(sender, e);
            }
        }

        protected void ResetItemWidth(string itemName, string text)
        {
            if (this.mStatusStrip.Items.ContainsKey(itemName))
            {
                int num = ((int)(this.mStatusStrip.CreateGraphics().MeasureString(text, this.mStatusStrip.Font).Width + 0.5f)) + 15;
                num = (num < 220) ? 220 : num;
                this.SetStatus(itemName, text);
                this.mStatusStrip.Items[itemName].Width = num;
            }
        }

        protected void SetShowOrHide(string itemName, bool visible)
        {
            if (this.mStatusStrip.Items.ContainsKey(itemName))
            {
                this.mStatusStrip.Items[itemName].Visible = visible;
            }
        }

        protected void SetStatus(string itemName, string value)
        {
            if ((value != null) && this.mStatusStrip.Items.ContainsKey(itemName))
            {
                this.mStatusStrip.Items[itemName].Text = value;
            }
        }

        private void StartShowTime()
        {
            this.mShowTimer.Elapsed -= new ElapsedEventHandler(this.mShowTimer_Elapsed);
            this.mShowTimer.Elapsed += new ElapsedEventHandler(this.mShowTimer_Elapsed);
            this.mShowTimer.Start();
            if (this.mStatusStrip.Items.ContainsKey("Time"))
            {
                this.mStatusStrip.Items["Time"].Text = DateTime.Now.ToString();
            }
        }

        // Properties
        public string User
        {
            set
            {
                this.SetStatus("User", value);
            }
        }
    }
}
