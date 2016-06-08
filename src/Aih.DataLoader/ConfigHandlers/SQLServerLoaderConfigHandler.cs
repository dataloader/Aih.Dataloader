using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Aih.DataLoader.Interfaces;


namespace Aih.DataLoader.ConfigHandlers
{
    public class SQLServerLoaderConfigHandler : ILoaderConfigHandler
    {
        private string _connectionString;

        public SQLServerLoaderConfigHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Dictionary<string, string> GetLoaderConfig(string loaderContainerName, string loaderName)
        {
            var config = GetConfig(loaderContainerName, loaderName);
            var containerConfig = GetConfig(loaderContainerName, "*");
            var systemConfig = GetConfig("*", "*");

            foreach(var dict in containerConfig)
            {
                if(!config.ContainsKey(dict.Key))
                {
                    config.Add(dict.Key, dict.Value);
                }
            }

            foreach (var sysDict in systemConfig)
            {
                if (!config.ContainsKey(sysDict.Key))
                {
                    config.Add(sysDict.Key, sysDict.Value);
                }
            }

            return config;
        }

        private Dictionary<string, string> GetConfig(string loaderContainerName, string loaderName)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(GetLoaderConfigQuery()))
                {
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();
                    SetParameters(cmd, loaderContainerName, loaderName);

                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string batchkey = reader.GetString(0).Trim();
                            string batchval = reader.GetString(1).Trim();
                            properties.Add(batchkey, batchval);
                        }
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine(exc.Message);
                        throw exc;
                    }
                }
            }

            return properties;
        }


        private string GetLoaderConfigQuery()
        {
            return "select [key], [value] from [dbo].[LoaderConfig] where [loaderContainerName] = @loaderContainerName and [loadername] = @loaderName";
        }

        private void SetParameters(SqlCommand cmd, string loaderContainerName, string loaderName)
        {
            cmd.Parameters.Add("@loaderContainerName", System.Data.SqlDbType.NChar).Value = loaderContainerName;
            cmd.Parameters.Add("@loaderName", System.Data.SqlDbType.NChar).Value = loaderName;
        }


    }
}
