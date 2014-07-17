using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public partial class PipelineTask
    {
        private DataNode _LastNode;

        private PipelineTask(DataNode source)
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


    }
}
