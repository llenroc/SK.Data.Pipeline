using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public abstract class DataNode
    {
        public event EventHandler<StartEventArgs> Start;
        public event EventHandler<FirstEntityEventArgs> GetFirstEntity;
        public event EventHandler<GetEntityEventArgs> GetEntity;
        public event EventHandler<FinishEventArgs> Finish;

        public abstract DataNode Parent { get; }

        protected abstract IEnumerable<Entity> GetEntities();

        public IEnumerable<Entity> Entities
        {
            get
            {
                int index = 0;
                try
                {
                    // Call Start before pull entity
                    if (Start != null) Start(this, new StartEventArgs(this));

                    foreach (Entity entity in GetEntities())
                    {
                        // trigger first entity event
                        if (index == 0 && GetFirstEntity != null) GetFirstEntity(this, new FirstEntityEventArgs(entity));
                        if (GetEntity != null) GetEntity(this, new GetEntityEventArgs(entity, index++));

                        yield return entity;
                    }
                }
                finally
                {
                    if (Finish != null) Finish(this, new FinishEventArgs(index));
                }
            }
        }
    }

    public class StartEventArgs : EventArgs
    {
        public StartEventArgs(DataNode dataNode)
        {
            DataNode = dataNode;
        }

        public DataNode DataNode { get; set; }
    }

    public class FirstEntityEventArgs : EventArgs
    {
        public FirstEntityEventArgs(Entity entity)
        {
            FirstEntity = entity;
        }

        public Entity FirstEntity { get; set; }
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
