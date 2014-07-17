using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class FuncSourceNode : SourceNode
    {
        public Func<IEnumerable<Entity>> GetEntitiesFunc { get; set; }

        public FuncSourceNode(Func<IEnumerable<Entity>> getEntitiesFunc)
        {
            GetEntitiesFunc = getEntitiesFunc;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            return GetEntitiesFunc();
        }
    }
}
