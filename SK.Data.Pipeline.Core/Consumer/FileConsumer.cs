using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core.Common;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SK.Data.Pipeline.Core
{
    public class FileConsumer : ConsumerBase
    {
        public string Path { get; set; }

        private Stream _Stream;

        public FileConsumer(string path)
        {
            Path = path;
        }

        public override void Start(object sender, StartEventArgs args)
        {
            _Stream = File.Open(Path, FileMode.OpenOrCreate);
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(_Stream, args.CurrentEntity);
        }

        public override void Finish(object sender, FinishEventArgs args)
        {
            _Stream.Close();

            string.Format("{0} lines data wrote to {1}", args.Count, Path).Info();
        }
    }
}
