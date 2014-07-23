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
        public AzureTableInfo TableInfo { get; set; }
        public string PartitionKeyTemplate { get; set; }
        public string RowKeyTemplate { get; set; }

        private CloudTable _Table;

        public AzureTableConsumer(AzureTableInfo azureTableInfo, string partitionKeyTemplate, string rowKeyTemplate)
        {
            TableInfo = azureTableInfo;
            PartitionKeyTemplate = partitionKeyTemplate;
            RowKeyTemplate = rowKeyTemplate;
        }

        public override void Start(object sender, StartEventArgs args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(TableInfo.ConnectionString);
            _Table = storageAccount.CreateCloudTableClient().GetTableReference(TableInfo.TableName);
            _Table.CreateIfNotExists();
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(new TableEntity(args.CurrentEntity, PartitionKeyTemplate, RowKeyTemplate));
            _Table.Execute(insertOperation);
        }
    }
}
