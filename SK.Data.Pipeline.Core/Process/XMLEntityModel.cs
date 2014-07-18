using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class XMLEntityModel : EntityModel
    {
        public string ItemXPath { set; get; }
        public Dictionary<string, string> ColumnXPath { get; set; }

        public XMLEntityModel(string itemXPath, params string[] columns)
            : base(columns)
        {
            ItemXPath = itemXPath;

            ColumnXPath = new Dictionary<string, string>();
        }

        public void AddXMLColumn(string columnName, string xPath, object defaultValue = null)
        {
            AddColumn(columnName, defaultValue);
            ColumnXPath[columnName] = xPath;
        }
    }
}
