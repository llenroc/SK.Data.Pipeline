using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core;
using SK.Data.Pipeline.Web;

namespace SK.Data.Pipeline
{
    public static class WebPipelineExtension
    {
        public static PipelineTask ParseHtml(this PipelineTask pipeline, string column, XMLEntityModel model)
        {
            pipeline.AddProcessNode((node) => new ParseHtmlProcessNode(node, column, model));
            return pipeline;
        }

        public static PipelineTask ParseHtml(this PipelineTask pipeline, XMLEntityModel model)
        {
            return ParseHtml(pipeline, model);
        }
    }
}
