using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public abstract class DataNode
    {
        public event EventHandler<GetEntityEventArgs> AfterGetEntity;

        public abstract DataNode Parent { get; }

        protected abstract IEnumerable<Entity> GetEntities();

        public IEnumerable<Entity> Entities
        {
            get
            {
                int index = 0;
                foreach (Entity entity in GetEntities())
                {
                    AfterGetEntity(this, new GetEntityEventArgs(entity, index));
                    yield return entity;
                }
            }
        }
    }

    public class GetEntityEventArgs : EventArgs
    {
        public GetEntityEventArgs(Entity entity, int index)
        {
            Index = index;
            NewEntity = entity;
        }

        public int Index { get; set; }

        public Entity NewEntity { get; set; }
    }
}
