using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class MonitorConsumer : ConsumerBase
    {
        public Action<object, GetEntityEventArgs> MonitorFunc { get; set; }

        public MonitorConsumer(Action<object, GetEntityEventArgs> monitorFunc)
        {
            MonitorFunc = monitorFunc;
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            MonitorFunc(sender, args);
        }
    }
}
