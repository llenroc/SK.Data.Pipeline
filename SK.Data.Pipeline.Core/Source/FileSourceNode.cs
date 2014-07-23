using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class FileSourceNode : SourceNode
    {
        public string FilePath { set; get; }

        public FileSourceNode(string filePath)
            : base()
        {
            FilePath = filePath;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            try
            {
                while (stream.Position < stream.Length)
                {
                    yield return (Entity)formatter.Deserialize(stream);
                }
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
