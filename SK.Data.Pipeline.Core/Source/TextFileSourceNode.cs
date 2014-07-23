using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class TextFileSourceNode : SourceNode
    {
        public string FilePath { set; get; }

        public TextFileSourceNode(string filePath)
            : base()
        {
            FilePath = filePath;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            var entity = new Entity();
            entity.SetValue(Entity.DefaultColumn, File.ReadAllText(FilePath), false);
            
            yield return entity;
        }
    }
}
