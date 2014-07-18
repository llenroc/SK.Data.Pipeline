using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class AddTemplateColumnProcessNode : ProcessNode
    {
        private static Regex TemplateRegex = new Regex("##(.*?)##", RegexOptions.Compiled);

        public string Template { get; set; }
        public string Column { get; set; }

        public AddTemplateColumnProcessNode(DataNode parent, string column, string template)
            : base(parent)
        {
            Template = template;
            Column = column;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            string content = Template;
            foreach (Entity entity in Parent.Entities)
            {
                content = TemplateRegex.Replace(content, (match) =>
                {
                    object value = null;
                    if (entity.TryGetValue(match.Groups[1].Value, out value)){
                        if (value != null)
                        {
                            return value.ToString();
                        }
                    }

                    return match.Value;
                });

                entity.SetValue(Column, content);
                yield return entity;
            }
        }
    }
}
