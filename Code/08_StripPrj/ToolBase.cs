using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_StripPrj
{
    public abstract class ToolBase : IDisposable
    {
        // Fields
        protected ToolStrip mToolStrip;

        // Events
        public event EventHandler ClickToolItem;

        // Methods
        public ToolBase(ToolStrip toolStrip)
        {
            this.mToolStrip = toolStrip;
            if (this.mToolStrip == null)
            {
                this.mToolStrip = new ToolStrip();
            }
        }

        protected abstract void AssembleTool();
        private void ChangeCheckedStatus(ToolStripButton item)
        {
            if (((item != null) && item.CheckOnClick) && item.Checked)
            {
                foreach (ToolStripButton button in this.mToolStrip.Items)
                {
                    if (button.Name != item.Name)
                    {
                        button.Checked = false;
                    }
                }
            }
        }

        protected ToolStripButton CreateToolStripButton(string name, string text, Image image, bool checkOnClick)
        {
            ToolStripButton item = new ToolStripButton(text, image, new EventHandler(this.OnClickToolItem), name);
            this.SetItemParam(item, checkOnClick);
            return item;
        }

        public virtual void Dispose()
        {
            if (this.mToolStrip != null)
            {
                this.mToolStrip.Dispose();
                this.mToolStrip = null;
            }
        }

        protected virtual void OnClickToolItem(object sender, EventArgs e)
        {
            if (this.ClickToolItem != null)
            {
                this.ChangeCheckedStatus(sender as ToolStripButton);
                this.ClickToolItem(sender, e);
            }
        }

        protected virtual void SetItemParam(ToolStripButton item, bool checkOnClick)
        {
            item.CheckOnClick = checkOnClick;
            item.BackColor = Color.Transparent;
            item.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            item.DisplayStyle = ToolStripItemDisplayStyle.Image;
            item.ImageTransparentColor = Color.Magenta;
            item.AutoSize = false;
            item.Width = item.Height = 0x24;
            item.Margin = new Padding(2);
        }
    }
}
