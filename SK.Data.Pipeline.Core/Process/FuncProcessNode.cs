using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class FuncProcessNode : ProcessNode
    {
        public Func<IEnumerable<Entity>, IEnumerable<Entity>> ProcessFunc { get; set; }

        public FuncProcessNode(DataNode parent, Func<IEnumerable<Entity>, IEnumerable<Entity>> processFunc)
            : base(parent)
        {
            ProcessFunc = processFunc;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            return ProcessFunc(Parent.Entities);
        }
    }
}
