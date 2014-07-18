using HtmlAgilityPack;
using SK.Data.Pipeline.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Web
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
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);
            HtmlNode rootNode = document.DocumentNode;

            var nodes = rootNode.SelectNodes(Model.ItemXPath);
            Regex attrRegex = new Regex(AttrRegex, RegexOptions.Compiled);

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var entity = new Entity();

                    foreach (string column in Model.ColumnXPath.Keys)
                    {
                        string xpath = Model.ColumnXPath[column];

                        // For htmlAgilityPack do not support select attribute
                        string attributeName = null;
                        if (attrRegex.IsMatch(xpath))
                        {
                            attributeName = attrRegex.Match(xpath).Groups[1].Value;
                            xpath = attrRegex.Replace(xpath, "");
                        }

                        var columnNode = node.SelectSingleNode(xpath);
                        if (columnNode != null)
                        {
                            if (attributeName == null)
                            {
                                string resultValue = columnNode.InnerText;
                                entity.SetValue(column, resultValue);
                            }
                            else
                            {
                                entity.SetValue(column, columnNode.Attributes[attributeName].Value);
                            }
                        }
                    }

                    yield return entity;
                }
            }
        }
    }
}

