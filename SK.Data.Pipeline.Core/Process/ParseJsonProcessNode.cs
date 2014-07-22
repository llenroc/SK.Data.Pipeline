using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace SK.Data.Pipeline.Core
{
    public class ParseJsonProcessNode : ParseXMLProcessNode
    {
        public ParseJsonProcessNode(DataNode parent, string targetColumn, XMLEntityModel model)
            : base(parent, targetColumn, model)
        {
        }

        protected override IEnumerable<Entity> Parse(string content)
        {
            content = ((XmlDocument)JsonConvert.DeserializeXmlNode(content, "Root")).OuterXml;
            return base.Parse(content);
        }
    }
}
