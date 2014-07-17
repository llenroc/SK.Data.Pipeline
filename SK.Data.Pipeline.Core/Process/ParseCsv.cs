using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class ParseCsv : Spilt
    {
        public string Separator { get; set; }

        public string Column { get; set; }

        public string[] SpiltColumns { get; set; }

        private bool _SkipFirstLine = false;

        public ParseCsv(DataNode parent, string column)
            : base(parent, column, "\\s*,\\s*")
        { }
    }
}
