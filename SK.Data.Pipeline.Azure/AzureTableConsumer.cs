using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SK.Data.Pipeline.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Azure
{
    public class AzureTableConsumer : ConsumerBase
    {
        public override void Consume(object sender, GetEntityEventArgs args)
        {
            OperationContext d = new OperationContext();
        }


    }
}
