using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace SK.Data.Pipeline.Core
{
    public class ParseXMLProcessNode : ProcessNode
    {
        public string TargetColumn { set; get; }

        public XMLEntityModel Model { set; get; }

        public ParseXMLProcessNode(DataNode parent, string targetColumn, XMLEntityModel model)
            : base(parent)
        {
            TargetColumn = targetColumn;
            Model = model;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            foreach (Entity entity in Parent.Entities)
            {
                string content = null;
                if (entity.TryGetValue(TargetColumn, out content))
                {
                    foreach (var newEntity in Parse(content))
                    {
                        entity.Extend(newEntity);
                        entity.RemoveColumn(TargetColumn);
                        yield return entity;
                    }
                }
                else
                {
                    yield return entity;
                }
            }
        }

        private IEnumerable<Entity> Parse(string content)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            MemoryStream stream = new MemoryStream(byteArray);
            XPathDocument document = new XPathDocument(stream);
            XPathNavigator navigator = document.CreateNavigator();

            XPathNodeIterator itemItr = navigator.Select(Model.ItemXPath);
            while (itemItr.MoveNext())
            {
                var entity = new Entity();

                XPathNavigator current = itemItr.Current;
                foreach (string column in Model.Columns)
                {
                    if (!Model.ColumnXPath.ContainsKey(column)) continue;

                    string xPath = Model.ColumnXPath[column];
                    var columnValue = current.SelectSingleNode(xPath);
                    if (columnValue != null)
                    {
                        entity.SetValue(column, columnValue.Value);
                    }
                }

                yield return entity;
            }
        }
    }
}
