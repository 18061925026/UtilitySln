using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogsPrj
{
    public abstract class LogBase : IDisposable
    {
        // Fields
        protected ILog mLog;

        // Methods
        public bool Debug(object msg)
        {
            if ((this.mLog == null) || !this.mLog.IsDebugEnabled)
            {
                return false;
            }
            this.mLog.Debug(msg);
            return true;
        }

        public bool Debug(object msg, Exception ex)
        {
            if ((this.mLog == null) || !this.mLog.IsDebugEnabled)
            {
                return false;
            }
            this.mLog.Debug(msg, ex);
            return true;
        }

        public virtual void Dispose()
        {
            this.mLog = null;
        }

        public bool Error(object msg)
        {
            if ((this.mLog == null) || !this.mLog.IsErrorEnabled)
            {
                return false;
            }
            this.mLog.Error(msg);
            return true;
        }

        public bool Error(object msg, Exception ex)
        {
            if ((this.mLog == null) || !this.mLog.IsErrorEnabled)
            {
                return false;
            }
            this.mLog.Error(msg, ex);
            return true;
        }

        public bool Fatal(object msg)
        {
            if ((this.mLog == null) || !this.mLog.IsFatalEnabled)
            {
                return false;
            }
            this.mLog.Fatal(msg);
            return true;
        }

        public bool Fatal(object msg, Exception ex)
        {
            if ((this.mLog == null) || !this.mLog.IsFatalEnabled)
            {
                return false;
            }
            this.mLog.Fatal(msg, ex);
            return true;
        }

        public bool Info(object msg)
        {
            if ((this.mLog == null) || !this.mLog.IsInfoEnabled)
            {
                return false;
            }
            this.mLog.Info(msg);
            return true;
        }

        public bool Info(object msg, Exception ex)
        {
            if ((this.mLog == null) || !this.mLog.IsInfoEnabled)
            {
                return false;
            }
            this.mLog.Info(msg, ex);
            return true;
        }

        public bool Warn(object msg)
        {
            if ((this.mLog == null) || !this.mLog.IsWarnEnabled)
            {
                return false;
            }
            this.mLog.Warn(msg);
            return true;
        }

        public bool Warn(object msg, Exception ex)
        {
            if ((this.mLog == null) || !this.mLog.IsWarnEnabled)
            {
                return false;
            }
            this.mLog.Warn(msg, ex);
            return true;
        }
    }
}
