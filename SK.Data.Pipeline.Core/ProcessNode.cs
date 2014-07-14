using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public abstract class ProcessNode : DataNode
    {
        private DataNode _Parent = null;
        public override DataNode Parent
        {
            get
            {
                return _Parent;
            }
        }

        public ProcessNode(DataNode parent)
        {
            _Parent = parent;
        }
    }
}
