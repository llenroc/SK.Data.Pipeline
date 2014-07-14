using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public abstract class DataNode
    {
        

        protected abstract IEnumerable<Entity> GetEntities();
    }

    public class NewEntityArgs : EventArgs
    {
        public int Index { get; set; }

        public Entity NewEntity { get; set; }
    }
}
