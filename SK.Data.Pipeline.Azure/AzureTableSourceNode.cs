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
    public class AzureTableSourceNode : SourceNode
    {
        public AzureTableInfo TableInfo { set; get; }

        public AzureTableSourceNode(AzureTableInfo azureTableInfo)
        {
            TableInfo = azureTableInfo;
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(TableInfo.ConnectionString);
            CloudTable table = storageAccount.CreateCloudTableClient().GetTableReference(TableInfo.TableName);
            if (table.Exists())
            {
                foreach (TableEntity tentity in table.ExecuteQuery(new TableQuery<TableEntity>()))
                {
                    yield return tentity.ToEntity();
                }
            }
        }
    }
}
