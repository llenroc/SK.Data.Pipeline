using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class ExtendProcessNode : ProcessNode
    {
        public Action<Entity> ExtendAction { get; set; }

        public ExtendProcessNode(DataNode parent, Action<Entity> extendAction)
            : base(parent)
        {
            ExtendAction = extendAction;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            foreach (var entity in Parent.Entities)
            {
                ExtendAction(entity);

                yield return entity;
            }
        }
    }
}
