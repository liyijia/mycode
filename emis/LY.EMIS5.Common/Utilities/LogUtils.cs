using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Utilities
{
    public class LogUtils
    {
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog Logger
        {
            get
            {
                return logger;
            }
        }
    }
}
