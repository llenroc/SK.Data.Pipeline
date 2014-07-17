using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core.Common;

namespace SK.Data.Pipeline.Core
{
    public class FileConsumer : ConsumerBase
    {
        public string Path { get; set; }

        public string Separator { get; set; }

        public string[] Columns { get; set; }

        private TextWriter _Writer = null;

        public FileConsumer(string path, string separator = Entity.DefaultSeparator, string[] columns = null)
        {
            Path = path;
            Separator = separator;
            Columns = columns;
        }

        private void InitColumns(Entity entity)
        {
            Columns = entity.Values.Keys.ToArray();
        }

        private void WirteTitle()
        {
            _Writer.WriteLine(string.Join(Separator, Columns));
        }

        public override void Start(object sender, StartEventArgs args)
        {
            _Writer = File.CreateText(Path);
        }

        public override void GetFirstEntity(object sender, FirstEntityEventArgs args)
        {
            if (Columns == null)
            {
                InitColumns(args.FirstEntity);
            }

            WirteTitle();
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            object[] values = new object[Columns.Length];
                
            for (int i = 0; i < Columns.Length; ++i)
            {
                args.CurrentEntity.TryGetValue(Columns[i], out values[i]);
            }

            _Writer.WriteLine(string.Join(Separator, values));
        }

        public override void Finish(object sender, FinishEventArgs args)
        {
            _Writer.Dispose();

            string.Format("{0} lines data wrote to {1}", args.Count, Path).Info();
        }
    }
}
