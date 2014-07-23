using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Azure
{
    public class AzureTableInfo : AzureStorageInfo
    {
        public AzureTableInfo(string tableName)
        {
            TableName = tableName;
        }

        public AzureTableInfo(string accountName, string accountKey, string tableName)
            : base(accountName, accountKey)
        {
            TableName = tableName;
        }

        public string TableName { get; set; }
    }

    public class AzureStorageInfo
    {
        public AzureStorageInfo()
        {
            ConnectionString = @"UseDevelopmentStorage=true;";
        }

        public AzureStorageInfo(string accountName, string accountKey)
        {
            ConnectionString = string.Format(@"DefaultEndpointsProtocol=http;AccountName={0};AccountKey={1};", accountName, accountKey);
        }

        public string ConnectionString { get; set; }
    }
}
