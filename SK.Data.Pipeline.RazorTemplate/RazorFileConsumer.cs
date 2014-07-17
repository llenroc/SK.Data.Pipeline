using RazorEngine;
using SK.Data.Pipeline.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.RazorTemplate
{
    public class RazorFileConsumer : ConsumerBase
    {
        public string Path { get; set; }
        public string Template { get; set; }
        public string HeadTemplate { get; set; }
        public string FootTemplate { get; set; }
        public Entity GlobalModel { get; set; }

        private StreamWriter _Writer = null;

        public RazorFileConsumer(string path, string template, string headTemplate = null, string footTemplate = null, Entity globalModel = null)
        {
            Path = path;
            Template = template;
            HeadTemplate = headTemplate;
            FootTemplate = footTemplate;
            GlobalModel = globalModel ?? new Entity();
        }

        public override void Start(object sender, StartEventArgs args)
        {
            _Writer = File.CreateText(Path);
            if (HeadTemplate == null) return;

            string content = Razor.Parse(HeadTemplate, GlobalModel.ToDynamicObject());
            _Writer.WriteLine(content);
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            string content = Razor.Parse(Template, args.CurrentEntity.ToDynamicObject());
            _Writer.WriteLine(content);
        }

        public override void Finish(object sender, FinishEventArgs args)
        {
            if (FootTemplate != null)
            {
                string content = Razor.Parse(FootTemplate, GlobalModel.ToDynamicObject());
                _Writer.WriteLine(content);
            }

            _Writer.Close();
        }
    }
}
