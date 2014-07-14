using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core.Common
{
    public class Logger
    {
        private static ILog _Log;

        public static ILog Instance
        {
            get
            {
                if (_Log == null)
                {

                    _Log = LogManager.GetLogger("SK.Data.Pipeline.Logger");
                }

                return _Log;
            }
        }
    }
}
