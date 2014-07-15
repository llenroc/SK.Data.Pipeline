using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core.Common;
using System.Text.RegularExpressions;

namespace SK.Data.Pipeline.Core
{
    public class TemplateFileConsumer : IConsumer
    {
        private static Regex TemplateRegex = new Regex("##(.*?)##", RegexOptions.Compiled);

        public string Path { get; set; }
        public string Template { get; set; }

        private TextWriter _Writer = null;

        public TemplateFileConsumer(string path, string template)
        {
            Path = path;
            Template = template;

            _Writer = File.CreateText(Path);
        }

        public virtual void Consume(object sender, GetEntityEventArgs args)
        {
            Entity entity = args.CurrentEntity;
            string content = TemplateRegex.Replace(Template, (match) =>
            {
                object value = null;
                if (entity.TryGetValue(match.Groups[1].Value, out value))
                {
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }

                return match.Value;
            });

            _Writer.WriteLine(content);
        }

        public virtual void Finish(object sender, FinishEventArgs args)
        {
            _Writer.Dispose();

            string.Format("{0} lines data wrote to {1}", args.Count, Path).Info();
        }
    }
}
