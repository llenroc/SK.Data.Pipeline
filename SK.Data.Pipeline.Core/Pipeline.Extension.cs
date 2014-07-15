using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core.Process;

namespace SK.Data.Pipeline.Core
{
    public partial class Pipeline
    {
        public Pipeline SpiltParse(string separator = Entity.DefaultSeparator, string column = Entity.DefaultColumn, string[] spiltColumns = null)
        {
            var SpiltNode = new SpiltParseProcessNode(_LastNode, separator, column, spiltColumns);
            _LastNode = SpiltNode;

            return this;
        }

        public Pipeline To(IConsumer consumer)
        {
            _LastNode.GetEntity += consumer.Consume;
            _LastNode.Finish += consumer.Finish;
            return this;
        }

        public Pipeline ToFile(string path, string separator = Entity.DefaultSeparator, string[] columns = null)
        {
            To(new FileConsumer(path, separator, columns));
            return this;
        }
    }
}
