using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace SK.Data.Pipeline.Core
{
    public class ParseXML : ProcessNode
    {
        private XMLParser _Parser;

        public string Column { set; get; }

        public ParseXML(DataNode parent, string column, string itemXPath, params string[] columnXPaths)
            : base(parent)
        {
            Column = column;
            _Parser = new XMLParser(itemXPath, columnXPaths);
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            foreach (Entity entity in Parent.Entities)
            {
                string content = null;
                if (entity.TryGetValue(Column, out content))
                {
                    foreach (Entity xmlEntity in _Parser.Parse(content))
                    {
                        yield return entity.Clone().Extend(xmlEntity);
                    }
                }
                else
                {
                    yield return entity;
                }
            }
        }
    }

    internal class XMLParser
    {
        public string ItemXPath { set; get; }

        private string[] ColumnXPaths { set; get; }

        public XMLParser(string itemXPath, params string[] columnXPaths)
        {
            ItemXPath = itemXPath;
            ColumnXPaths = columnXPaths;
        }

        public IEnumerable<Entity> Parse(string content)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            MemoryStream stream = new MemoryStream(byteArray);
            XPathDocument document = new XPathDocument(stream);
            XPathNavigator navigator = document.CreateNavigator();

            XPathNodeIterator itemItr = navigator.Select(ItemXPath);
            while (itemItr.MoveNext())
            {
                var entity = new Entity();

                XPathNavigator current = itemItr.Current;
                foreach (string xPath in ColumnXPaths)
                {
                    var columnValue = current.SelectSingleNode(xPath);
                    if (columnValue != null)
                    {
                        entity.SetValue(columnValue.Name, columnValue.Value);
                    }
                }

                yield return entity;
            }
        }
    }
}
