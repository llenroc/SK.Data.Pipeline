using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public interface IConsumer
    {
        void GetEntity(object sender, GetEntityEventArgs args);
    }
}
