using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            foreach (string line in File.ReadLines(FilePath))
            {
                var entity = new Entity();
                entity.SetValue(Entity.DefaultColumn, line, false);
                yield return entity;
            }
        }
    }
}
