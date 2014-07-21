using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core.Common
{
    public static class HtmlAgilityHelper
    {
        private static Regex AttrRegex = new Regex(@"##(.*?)##", RegexOptions.Compiled);

        private static void GetAttributeNameFromXPath(ref string xPath, out string attributeName)
        {
            attributeName = null;
            if (AttrRegex.IsMatch(xPath))
            {
                attributeName = AttrRegex.Match(xPath).Groups[1].Value;
                xPath = AttrRegex.Replace(xPath, "");
            }
        }

        public static HtmlNode ParseHtmlDocument(string content)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);

            return document.DocumentNode;
        }

        public static Entity GetFirstEntityFromContent(string content, XMLEntityModel Model)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);
            HtmlNode rootNode = document.DocumentNode;

            var nodes = rootNode.SelectNodes(Model.ItemXPath);
            if (nodes == null) return null;

            var firstNode = nodes.First();
            var entity = new Entity();
            foreach (string column in Model.ColumnXPath.Keys)
            {
                string xpath = Model.ColumnXPath[column];

                // For htmlAgilityPack do not support select attribute
                string attributeName;
                GetAttributeNameFromXPath(ref xpath, out attributeName);

                var columnNode = firstNode.SelectSingleNode(xpath);
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

            return entity;
        }

        public static IEnumerable<Entity> GetEntitiesFromContent(string content, XMLEntityModel Model)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(content);
            HtmlNode rootNode = document.DocumentNode;

            var nodes = rootNode.SelectNodes(Model.ItemXPath);
            

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var entity = new Entity();

                    foreach (string column in Model.ColumnXPath.Keys)
                    {
                        string xpath = Model.ColumnXPath[column];

                        // For htmlAgilityPack do not support select attribute
                        string attributeName;
                        GetAttributeNameFromXPath(ref xpath, out attributeName);

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

                    if (!entity.IsEmpty())
                        yield return entity;
                }
            }
        } 

    }
}
