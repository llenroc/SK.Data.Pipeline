using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core.Common;

namespace SK.Data.Pipeline.Core
{
    public class TextFileConsumer : ConsumerBase
    {
        public string Path { get; set; }

        public string Separator { get; set; }

        public EntityModel Model { get; private set; }

        private TextWriter _Writer = null;

        public TextFileConsumer(string path, string separator = Entity.DefaultSeparator, string[] columns = null)
        {
            Path = path;
            Separator = separator;

            if (columns != null)
                Model = new EntityModel(columns);
        }

        public TextFileConsumer(string path, EntityModel model, string separator = Entity.DefaultSeparator)
        {
            Path = path;
            Separator = separator;
            Model = model;
        }

        private void InitColumns(Entity entity)
        {
            Model = new EntityModel(entity.Columns);
        }

        private void WirteTitle()
        {
            _Writer.WriteLine(string.Join(Separator, Model.Columns));
        }

        public override void Start(object sender, StartEventArgs args)
        {
            _Writer = File.CreateText(Path);
        }

        public override void GetFirstEntity(object sender, FirstEntityEventArgs args)
        {
            if (Model == null)
            {
                InitColumns(args.FirstEntity);
            }

            WirteTitle();
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            var currentEntity = args.CurrentEntity;
            currentEntity.ToStandradEntity(Model);

            object[] values = new object[Model.Columns.Length];
            for (int i = 0; i < Model.Columns.Length; ++i)
            {
                currentEntity.TryGetValue(Model.Columns[i], out values[i]);
            }

            _Writer.WriteLine(string.Join(Separator, values).Replace('\r', ' ').Replace('\n', ' '));
        }

        public override void Finish(object sender, FinishEventArgs args)
        {
            _Writer.Dispose();

            string.Format("{0} lines data wrote to {1}", args.Count, Path).Info();
        }
    }
}
