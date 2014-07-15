using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public partial class Pipeline
    {
        private DataNode _LastNode;

        private Pipeline(DataNode source)
        {
            _LastNode = source;
        }

        public int Start()
        {
            return _LastNode.Entities.Count();
        }

        public async Task<int> StartAsync()
        {
            return await Task.Run(() => {
                return _LastNode.Entities.Count();
            });
        }

        public static Pipeline Create(SourceNode source)
        {
            return new Pipeline(source);
        }
    }
}
