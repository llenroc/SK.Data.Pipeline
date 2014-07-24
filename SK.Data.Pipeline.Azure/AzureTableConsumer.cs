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

        public int MaxParallelCount { get; set; }

        private CloudTable _Table;
        private List<TableEntity> _EntitiesWaitForInsert = new List<TableEntity>();

        public AzureTableConsumer(AzureTableInfo azureTableInfo, string partitionKeyTemplate, string rowKeyTemplate, int maxParallelCount = 10)
        {
            TableInfo = azureTableInfo;
            PartitionKeyTemplate = partitionKeyTemplate;
            RowKeyTemplate = rowKeyTemplate;
            MaxParallelCount = maxParallelCount;
        }

        public override void Start(object sender, StartEventArgs args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(TableInfo.ConnectionString);
            _Table = storageAccount.CreateCloudTableClient().GetTableReference(TableInfo.TableName);
            _Table.CreateIfNotExists();
        }

        public override void Consume(object sender, GetEntityEventArgs args)
        {
            // Insert entity to list and wait for inserting in the parallel way
            _EntitiesWaitForInsert.Add(new TableEntity(args.CurrentEntity, PartitionKeyTemplate, RowKeyTemplate));
            if (_EntitiesWaitForInsert.Count < MaxParallelCount) return;

            InsertAllTableEntity();
        }

        public override void Finish(object sender, FinishEventArgs args)
        {
            // Make sure all Entities have been inserted.
            InsertAllTableEntity();
        }

        /// <summary>
        /// Insert and clean all TableEntitys in wait list
        /// </summary>
        private void InsertAllTableEntity()
        {
            Parallel.ForEach(_EntitiesWaitForInsert,
                new ParallelOptions()
                {
                     MaxDegreeOfParallelism = MaxParallelCount
                }
                ,(tableEntity) =>
                {
                    TableOperation insertOperation = TableOperation.InsertOrReplace(tableEntity);
                    _Table.Execute(insertOperation);
                });

            _EntitiesWaitForInsert.Clear();
        }
    }
}
