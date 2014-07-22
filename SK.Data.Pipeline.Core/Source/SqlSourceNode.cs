using SK.Data.Pipeline.Core.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public struct ConnectInfo
    {
        public string Server;
        public string Db;
        public string User;
        public string Password;
        public bool IsTrust;
    }

    public class SqlSourceNode : SourceNode
    {
        public string Query { get; set; }
        public string ConnString { get; set; }
        public SqlParameter[] Parameters { get; set; }

        public SqlSourceNode(string query, string connString, params SqlParameter[] parameters)
        {
            Query = query;
            ConnString = connString;
            Parameters = parameters;
        }

        public SqlSourceNode(string query, ConnectInfo connectInfo, params SqlParameter[] parameters)
        {
            Query = query;
            ConnString = BuildConnectionString(connectInfo);
            Parameters = parameters;
        }

        private static string BuildConnectionString(ConnectInfo info)
        {
            if (!info.IsTrust)
            {
                const string connectionStringTemplate = @"Server={0};Database={1};User Id={2};Password={3};";
                return string.Format(connectionStringTemplate, info.Server, info.Db, info.User, info.Password);
            }
            else
            {
                const string connectionStringTemplate = @"Server={0};Database={1};Trusted_Connection=True;";
                return string.Format(connectionStringTemplate, info.Server, info.Db);
            }
        }

        protected override IEnumerable<Entity> GetEntities()
        {
            using (var conn = new SqlConnection(ConnString))
            {
                SqlDataReader reader = SqlUtil.ExecuteReader_Reader(Query, conn, Parameters);

                while (reader.Read())
                {
                    var entity = new Entity();
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        string columnName = reader.GetName(i);
                        entity.SetValue(columnName, reader.GetValue(i));
                    }

                    yield return entity;
                }
            }
        }
    }
}
