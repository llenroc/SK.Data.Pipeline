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
    public struct AzureTableInfo
    {
        public string ConnectionString;
        public string TableName;
        public string PartitionKey;
        public string RowKey;
    }

    public class AzureTableConsumer : ConsumerBase
    {
        public AzureTableInfo TableInfo { get; set; }

        private CloudTable _Table;

        public AzureTableConsumer(AzureTableInfo azureTableInfo)
        {
            TableInfo = azureTableInfo;
        }

        public override void Start(object sender, StartEventArgs args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(TableInfo.ConnectionString);
            _Table = storageAccount.CreateCloudTableClient().GetTableReference(TableInfo.TableName);
            _Table.CreateIfNotExists();
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            TableOperation insertOperation = TableOperation.Insert(new TableEntity(args.CurrentEntity, TableInfo.PartitionKey, TableInfo.RowKey));
            _Table.Execute(insertOperation);
        }
    }
}
