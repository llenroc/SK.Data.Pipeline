using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public partial class Pipeline
    {
        public Pipeline SpiltParse(string column, string separator = Entity.DefaultSeparator, string[] spiltColumns = null)
        {
            var SpiltNode = new SpiltParseProcessNode(_LastNode, column, separator, spiltColumns);
            _LastNode = SpiltNode;

            return this;
        }

        public Pipeline AddTemplateColumn(string template, string column)
        {
            var AddTemplateColumnNode = new AddTemplateColumn(_LastNode, template, column);
            _LastNode = AddTemplateColumnNode;

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

        public Pipeline ToTemplateFile(string path, string template)
        {
            To(new TemplateFileConsumer(path, template));
            return this;
        }
    }
}
