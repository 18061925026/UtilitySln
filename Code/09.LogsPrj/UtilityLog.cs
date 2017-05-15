using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogsPrj
{
    public sealed class UtilityLog : LogBase
    {
        // Methods
        public UtilityLog()
        {
            base.mLog = log4net.LogManager.GetLogger("UtilityLog");
        }
    }
}
