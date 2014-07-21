using HtmlAgilityPack;
using SK.Data.Pipeline.Core;
using SK.Data.Pipeline.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK.Data.Pipeline
{
    public class ParseHtmlProcessNode : ProcessNode
    {
        const string AttrRegex = @"##(.*?)##";

        public string TargetColumn { get; set; }

        public XMLEntityModel Model { get; set; }

        public ParseHtmlProcessNode(DataNode parent, string targetColumn, XMLEntityModel model)
            : base(parent)
        {
            Model = model;
            TargetColumn = targetColumn;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            foreach (Entity entity in Parent.Entities)
            {
                string content = null;

                if (entity.TryGetValue(TargetColumn, out content))
                {
                    foreach (Entity newEntity in Parse(content))
                    {
                        newEntity.Extend(entity);
                        newEntity.RemoveColumn(TargetColumn);
                        yield return newEntity;
                    }
                }
            }
        }

        private IEnumerable<Entity> Parse(string content)
        {
            return HtmlAgilityHelper.GetEntitiesFromContent(content, Model);
        }
    }
}

