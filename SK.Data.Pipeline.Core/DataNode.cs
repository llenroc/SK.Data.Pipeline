using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public abstract class DataNode
    {
        public event EventHandler<GetEntityEventArgs> GetEntity;
        public event EventHandler<FinishEventArgs> Finish;

        public abstract DataNode Parent { get; }

        protected abstract IEnumerable<Entity> GetEntities();

        public IEnumerable<Entity> Entities
        {
            get
            {
                int index = 0;
                foreach (Entity entity in GetEntities())
                {
                    if (GetEntity != null)
                        GetEntity(this, new GetEntityEventArgs(entity, index++));

                    yield return entity;
                }

                if (Finish != null)
                    Finish(this, new FinishEventArgs(index));
            }
        }
    }

    public class GetEntityEventArgs : EventArgs
    {
        public GetEntityEventArgs(Entity entity, int index)
        {
            Index = index;
            CurrentEntity = entity;
        }

        public int Index { get; set; }

        public Entity CurrentEntity { get; set; }
    }

    public class FinishEventArgs : EventArgs
    {
        public FinishEventArgs(int count)
        {
            Count = count;
        }

        public int Count { get; set; }
    }
}
