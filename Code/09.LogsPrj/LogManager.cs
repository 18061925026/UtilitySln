using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogsPrj
{
    public class LogsManager
    {
        public static readonly LogsManager Instance = new LogsManager();
        private List<LogBase> mLogs = new List<LogBase>();
        private LogsManager()
        {
        }
        public LogBase GetLog<T>()
            where T : LogBase
        {
            for (int i = 0; i < mLogs.Count; i++)
            {
                if (mLogs[i] is T)
                {
                    return mLogs[i];
                }
            }
            T log = System.Activator.CreateInstance<T>();
            mLogs.Add(log);
            return log;
        }

    }
}
