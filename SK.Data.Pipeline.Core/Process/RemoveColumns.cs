using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class RemoveColumns : ProcessNode
    {
        public string[] ColumnsShouldRemove;

        public RemoveColumns(DataNode parent, params string[] columnsShouldRemove)
            :base(parent)
        {
            ColumnsShouldRemove = columnsShouldRemove ?? new string[0];
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            foreach (Entity entity in Parent.Entities)
            {
                foreach (string column in ColumnsShouldRemove)
                {
                    entity.RemoveField(column);
                }

                yield return entity;
            }
        }
    }
}
