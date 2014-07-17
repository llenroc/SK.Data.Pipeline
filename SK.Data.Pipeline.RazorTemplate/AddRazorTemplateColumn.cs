using RazorEngine;
using SK.Data.Pipeline.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.RazorTemplate
{
    public class AddRazorTemplateColumn : ProcessNode
    {
        public string Template { get; set; }

        public string Column { get; set; }

        public AddRazorTemplateColumn(DataNode parent, string column, string template)
            : base(parent)
        {
            Template = template;
            Column = column;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            foreach (Entity entity in Parent.Entities)
            {
                string content = Razor.Parse(Template, entity.ToDynamicObject());
                entity.SetValue(Column, content);
                yield return entity;
            }
        }
    }
}
