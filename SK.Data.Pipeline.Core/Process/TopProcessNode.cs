using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class TopProcessNode : ProcessNode
    {
        public int Top { get; set; }

        public TopProcessNode(DataNode parent, int top)
            : base(parent)
        {
            Top = top;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            int index = 0;
            foreach (var entity in Parent.Entities)
            {
                if (index++ >= Top) break;

                yield return entity;
            }
        }
    }
}
