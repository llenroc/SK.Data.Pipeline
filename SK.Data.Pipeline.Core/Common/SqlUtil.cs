using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core.Common
{
    public static class SqlUtil
    {
        public static void ExecuteNonQuery(string query, string connString)
        {
            int retry = 0;

            do
            {
                try
                {
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    break;
                }
                catch (SqlException sqlex)
                {
                    WaitFor(1);

                    if (retry >= 3)
                    {
                        Trace.TraceInformation("ExecuteNonQuery failed with exception {0}", sqlex);
                        throw;
                    }
                }
            } while (retry++ < 3);
        }

        public static void ExecuteNonQuery(string query, string connString, params SqlParameter[] parameters)
        {
            int retry = 0;

            do
            {
                try
                {
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }

                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }

                    break;
                }
                catch (SqlException sqlex)
                {
                    WaitFor(1);

                    if (retry >= 3)
                    {
                        Trace.TraceInformation("ExecuteReader failed with exception {0}", sqlex);
                        throw;
                    }
                }
            } while (retry++ < 3);
        }

        public static int ExecuteScalar(string query, string connString)
        {
            int retry = 0;

            do
            {
                try
                {
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            return (int)cmd.ExecuteScalar();
                        }
                    }
                }
                catch (SqlException sqlex)
                {
                    WaitFor(1);

                    if (retry >= 3)
                    {
                        Trace.TraceInformation("ExecuteScalar failed with exception {0}", sqlex);
                        throw;
                    }
                }
            } while (retry++ < 3);

            return 0;
        }

        public static SqlDataReader ExecuteReader_Reader(string query, SqlConnection conn, params SqlParameter[] parameters)
        {
            SqlDataReader reader = null;

            int retry = 0;

            do
            {
                try
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }

                        reader = cmd.ExecuteReader();
                    }

                    break;
                }
                catch (SqlException sqlex)
                {
                    WaitFor(1);

                    if (retry >= 3)
                    {
                        Trace.TraceInformation("ExecuteReader failed with exception {0}", sqlex);
                        throw;
                    }
                }
            } while (retry++ < 3);

            return reader;
        }

        public static DataTable ExecuteReader(string query, string connString, params SqlParameter[] parameters)
        {
            var table = new DataTable();

            int retry = 0;

            do
            {
                try
                {
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                table.Load(reader);
                            }
                        }
                    }

                    break;
                }
                catch (SqlException sqlex)
                {
                    WaitFor(1);

                    if (retry >= 3)
                    {
                        Trace.TraceInformation("ExecuteReader failed with exception {0}", sqlex);
                        throw;
                    }
                }
            } while (retry++ < 3);

            return table;
        }

        public static DataTable ExecuteOracleReader(string query, string connString, params DbParameter[] parameters)
        {
            var table = new DataTable();

            int retry = 0;

            do
            {
                try
                {
                    const string providerName = "Oracle.DataAccess.Client";
                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                    using (var conn = factory.CreateConnection())
                    {
                        conn.ConnectionString = connString;
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = query;
                            cmd.CommandType = CommandType.Text;
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }

                            using (DbDataReader reader = cmd.ExecuteReader())
                            {
                                table.Load(reader);
                            }
                        }
                    }
                    break;
                }
                catch (DbException sqlex)
                {
                    WaitFor(1);

                    if (retry >= 3)
                    {
                        Trace.TraceInformation("ExecuteReader failed with exception {0}", sqlex);
                        throw;
                    }
                }
            } while (retry++ < 3);

            return table;
        }

        private static void WaitFor(int seconds)
        {
            Thread.Sleep(1000 * seconds);
        }

        public static string GetMasterDbConnectionString(string connString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connString) { InitialCatalog = "master" };
            return connectionStringBuilder.ConnectionString;
        }
    }
}
