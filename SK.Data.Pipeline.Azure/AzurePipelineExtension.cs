using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core;

namespace SK.Data.Pipeline.Azure
{
    public static class AzurePipelineExtension
    {
        public static PipelineTask ToAzureTable(this PipelineTask pipelineTask, AzureTableInfo azureTableInfo, string partitionKeyTemplate, string rowKeyTemplate)
        {
            return pipelineTask.To(new AzureTableConsumer(azureTableInfo, partitionKeyTemplate, rowKeyTemplate));
        }
    }
}
