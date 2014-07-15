using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public interface IConsumer
    {
        void Consume(object sender, GetEntityEventArgs args);

        void Finish(object sender, FinishEventArgs args);
    }
}
