using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Aih.DataLoader.Tools.PropertyHandlers
{
    public class SQLServerPropertyHandler : IPropertyHandler
    {

        private string _connectionString;


        public SQLServerPropertyHandler(string connectionString)
        {
            _connectionString = connectionString;
        }


        public Dictionary<string, string> GetProperties(string batchname)
        {

            Dictionary<string, string> properties = new Dictionary<string, string>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(GetPropertyQuery()))
                {
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();
                    SetParameters(cmd, batchname);

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




        private string GetPropertyQuery()
        {
            return "select [key], [value] from [dbo].[BatchProperties] where [batchname] = @batchname";
        }

        private void SetParameters(SqlCommand cmd, string batchname)
        {
            cmd.Parameters.Add("@batchname", System.Data.SqlDbType.NChar).Value = batchname;
        }

    }
}
