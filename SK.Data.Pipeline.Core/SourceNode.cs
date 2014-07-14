using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public abstract class SourceNode : DataNode
    {
        public override DataNode Parent
        {
            get
            {
                return null;
            }
        }

        public SourceNode()
        {
        }
    }
}
