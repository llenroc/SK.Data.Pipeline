using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class ParseCsvProcessNode : SpiltProcessNode
    {
        public ParseCsvProcessNode(DataNode parent, string column)
            : base(parent, column, "\\s*,\\s*")
        { }
    }
}
