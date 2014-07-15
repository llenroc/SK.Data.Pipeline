using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class SpiltParseProcessNode : ProcessNode
    {
        public string Separator { get; set; }

        public string Column { get; set; }

        public string[] SpiltColumns { get; set; }

        private bool _SkipFirstLine = false;

        public SpiltParseProcessNode(DataNode parent, string column, string separator = Entity.DefaultSeparator, string[] spiltColumns = null)
            : base(parent)
        {
            Separator = separator;
            Column = column;
            SpiltColumns = spiltColumns;

            if (spiltColumns == null)
            {
                _SkipFirstLine = true;
                Parent.GetEntity += GetNewEntity;
            }
        }

        protected void GetNewEntity(object sender, GetEntityEventArgs args)
        {
            if (SpiltColumns == null)
            {
                string content = args.CurrentEntity.GetValue<string>(Column);
                SpiltColumns = Regex.Split(content, Separator);
            }
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            bool isFirst = true;
            foreach (Entity entity in Parent.Entities)
            {
                if (isFirst && _SkipFirstLine)
                {
                    isFirst = false;
                    continue;
                }

                string content = null;
                if (entity.TryGetValue(Column, out content))
                {
                    string[] values = Regex.Split(content, Separator);
                    for (int i = 0; i < SpiltColumns.Length; ++i)
                    {
                        entity.SetValue(SpiltColumns[i], values[i]);
                    }
                }

                entity.RemoveField(Column);
                yield return entity;
            }
        }
    }
}
