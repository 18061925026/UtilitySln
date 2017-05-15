using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _08_StripPrj
{
    public abstract class MenuBase : IDisposable
    {
        // Fields
        protected MenuStrip mMenuStrip;

        // Events
        public event EventHandler ClickMenuItem;

        // Methods
        public MenuBase(MenuStrip menu)
        {
            this.mMenuStrip = menu;
            if (this.mMenuStrip == null)
            {
                this.mMenuStrip = new MenuStrip();
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

        protected virtual void CreateHelpMenu()
        {
            ToolStripMenuItem item = (ToolStripMenuItem)this.mMenuStrip.Items.Add("&Help");
            item.Alignment = ToolStripItemAlignment.Left;
            item.DropDownItems.AddRange(new ToolStripItem[] { this.CreateToolStripMenuItem("&About ITC", "&About ITC", new EventHandler(this.OnClickMenuItem)) });
        }

        protected virtual void CreateToolMenu()
        {
            ToolStripMenuItem item = (ToolStripMenuItem)this.mMenuStrip.Items.Add("&Tool");
            item.Alignment = ToolStripItemAlignment.Left;
            item.DropDownItems.AddRange(new ToolStripItem[] { this.CreateToolStripMenuItem("calc", "&Calculate", new EventHandler(this.SystemProcessClick)), this.CreateToolStripMenuItem("mspaint", "&Paint", new EventHandler(this.SystemProcessClick)), this.CreateToolStripMenuItem("notepad", "&Notepad", new EventHandler(this.SystemProcessClick)), this.CreateToolStripMenuItem("Wordpad", "T&ablet", new EventHandler(this.SystemProcessClick)) });
        }

        protected ToolStripMenuItem CreateToolStripMenuItem(string name, string text, EventHandler onClick) =>
            new ToolStripMenuItem(text, null, onClick, name);

        public virtual void Dispose()
        {
            if (this.mMenuStrip != null)
            {
                this.mMenuStrip.Dispose();
                this.mMenuStrip = null;
            }
        }

        protected virtual void OnClickMenuItem(object sender, EventArgs e)
        {
            if (this.ClickMenuItem != null)
            {
                this.ChangeCheckedStatus(sender as ToolStripMenuItem);
                this.ClickMenuItem(sender, e);
            }
        }

        private void SystemProcessClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start(((ToolStripMenuItem)sender).Name + ".exe");
            }
            catch (Exception)
            {
            }
        }
    }
}
