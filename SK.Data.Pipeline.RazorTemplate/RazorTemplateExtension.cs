using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core;

namespace SK.Data.Pipeline.RazorTemplate
{
    using Pipeline = SK.Data.Pipeline.Core.PipelineTask;

    public static class RazorTemplateExtension
    {
        public static Pipeline AddRazorTemplateColumn(this Pipeline pipeline, string column, string template)
        {
            pipeline.AddProcessNode((node) => new AddRazorTemplateColumn(node, column, template));
            return pipeline;
        }

        public static Pipeline ToRazorFile(this Pipeline pipeline, string path, string template, string headTemplate = null, string footTemplate = null, Entity globalModel = null)
        {
            pipeline.To(new RazorFileConsumer(path, template, headTemplate, footTemplate, globalModel));
            return pipeline;
        }
    }
}
