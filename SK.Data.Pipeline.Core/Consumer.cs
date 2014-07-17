using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public abstract class ConsumerBase
    {
        public virtual void Start(object sender, StartEventArgs args) { }

        public virtual void GetFirstEntity(object sender, FirstEntityEventArgs args) { }

        public abstract void Consume(object sender, GetEntityEventArgs args);

        public virtual void Finish(object sender, FinishEventArgs args) { }
    }
}
