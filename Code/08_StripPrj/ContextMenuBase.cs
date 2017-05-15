using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_StripPrj
{
    public abstract class ContextMenuBase : IDisposable
    {
        // Fields
        protected ContextMenuStrip mContextMenu;

        // Events
        public event EventHandler ClickMenuItem;

        // Methods
        public ContextMenuBase(ContextMenuStrip arg)
        {
            this.mContextMenu = arg;
            if (this.mContextMenu == null)
            {
                this.mContextMenu = new ContextMenuStrip();
            }
        }

        protected abstract void AssembleMenu();
        private void ChangeCheckedStatus(ToolStripMenuItem item)
        {
            if ((item != null) && item.CheckOnClick)
            {
                item.Checked = item.Checked;
            }
        }

        protected ToolStripMenuItem CreateItem(string name, bool isChecked)
        {
            ToolStripMenuItem item = this.CreateToolStripMenuItem(name, name, new EventHandler(this.OnClickMenuItem));
            item.CheckOnClick = isChecked;
            if (!this.mContextMenu.Items.ContainsKey(name))
            {
                this.mContextMenu.Items.Add(item);
            }
            return item;
        }

        protected ToolStripMenuItem CreateToolStripMenuItem(string name, string text, EventHandler onClick) =>
            new ToolStripMenuItem(text, null, onClick, name);

        public virtual void Dispose()
        {
            if (this.mContextMenu != null)
            {
                this.mContextMenu.Dispose();
                this.mContextMenu = null;
            }
        }

        protected virtual void OnClickMenuItem(object sender, EventArgs e)
        {
            this.ChangeCheckedStatus(sender as ToolStripMenuItem);
            if (this.ClickMenuItem != null)
            {
                this.ClickMenuItem(sender, e);
            }
        }
    }
}
