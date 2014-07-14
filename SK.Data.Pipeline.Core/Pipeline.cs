using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class Pipeline
    {
        private DataNode _LastNode;

        private Pipeline(DataNode source)
        {
            _LastNode = source;
        }

        public Pipeline To(IConsumer consumer)
        {
            _LastNode.AfterGetEntity += consumer.GetEntity;
            return this;
        }

        public int StartPull()
        {
            return _LastNode.Entities.Count();
        }

        public async Task<int> StartPullAsync()
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
