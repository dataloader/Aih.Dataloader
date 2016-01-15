using System;
using System.Data.SqlClient;
using Aih.DataLoader.Tools.Models;

namespace Aih.DataLoader.Tools.StatusHandlers
{
    public class SQLServerStatusHandler : IStatusHandler
    {

        private string _connectionString;

        public SQLServerStatusHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CreateBatchStatusRecord(BatchStatus status)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(GetInsertCommand()))
                    {
                        cmd.Connection = conn;
                        cmd.Parameters.Clear();
                        SetParameters(cmd, status);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException exc)
                        {
                            Console.WriteLine("Error writing " + status.ToString() + " to database.   Exception: " + exc.Message);
                            throw exc;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
                return false;
            }

            return true;
        }

        public bool BatchExists(string batchid)
        {

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("select count(*) from [dbo].[BatchStatus] where [batchrefrence] = @batchid AND status = 'Finished'"))
                {
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@batchid", System.Data.SqlDbType.NChar).Value = batchid;

                    try
                    {
                        int result = (int)cmd.ExecuteScalar();
                        if (result == 0)
                            return false;
                        else
                            return true;
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine("Error checking if batch exists.   Exception: " + exc.Message);
                        throw exc;
                    }
                }
            }

        }

        public bool UpdateBatchStatusRecord(BatchStatus status)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(GetUpdateCommand()))
                    {
                        cmd.Connection = conn;
                        cmd.Parameters.Clear();
                        SetParameters(cmd, status);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException exc)
                        {
                            Console.WriteLine("Error writing " + status.ToString() + " to database.   Exception: " + exc.Message);
                            throw exc;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
                return false;
            }


            return true;
        }


        #region Very ugly private helper functions

        private string GetInsertCommand()
        {
            string command = @"INSERT INTO [dbo].[BatchStatus]
           ([batchname]
           ,[batchid]
           ,[batchrefrence]
           ,[start_time]
           ,[start_load_time]
           ,[start_transform_time]
           ,[start_save_time]
           ,[start_cleanup_time]
           ,[finish_time]
           ,[comment]
           ,[status])
     VALUES
           (@batchname
           ,@batchid
           ,@batchrefrence
           ,@start_time
           ,@start_load_time
           ,@start_transform_time
           ,@start_save_time
           ,@start_cleanup_time
           ,@finish_time
           ,@comment
           ,@status)";

            return command;
        }

        private void SetParameters(SqlCommand cmd, BatchStatus status)
        {
            cmd.Parameters.Add("@batchname", System.Data.SqlDbType.NChar).Value = status.BatchName;
            cmd.Parameters.Add("@batchid", System.Data.SqlDbType.NChar).Value = status.BatchId;
            cmd.Parameters.Add("@start_time", System.Data.SqlDbType.DateTime).Value = status.StartTime;

            cmd.Parameters.Add("@batchrefrence", System.Data.SqlDbType.NChar).Value = status.BatchRefrence;


            if (status.StartLoadTime != null)
                cmd.Parameters.Add("@start_load_time", System.Data.SqlDbType.DateTime).Value = status.StartLoadTime;
            else
                cmd.Parameters.Add("@start_load_time", System.Data.SqlDbType.DateTime).Value = DBNull.Value;

            if (status.StartTransformTime != null)
                cmd.Parameters.Add("@start_transform_time", System.Data.SqlDbType.DateTime).Value = status.StartTransformTime;
            else
                cmd.Parameters.Add("@start_transform_time", System.Data.SqlDbType.DateTime).Value = DBNull.Value;

            if (status.StartSaveTime != null)
                cmd.Parameters.Add("@start_save_time", System.Data.SqlDbType.DateTime).Value = status.StartSaveTime;
            else
                cmd.Parameters.Add("@start_save_time", System.Data.SqlDbType.DateTime).Value = DBNull.Value;

            if (status.StartCleanupTime != null)
                cmd.Parameters.Add("@start_cleanup_time", System.Data.SqlDbType.DateTime).Value = status.StartCleanupTime;
            else
                cmd.Parameters.Add("@start_cleanup_time", System.Data.SqlDbType.DateTime).Value = DBNull.Value;

            if (status.FinishTime != null)
                cmd.Parameters.Add("@finish_time", System.Data.SqlDbType.DateTime).Value = status.FinishTime;
            else
                cmd.Parameters.Add("@finish_time", System.Data.SqlDbType.DateTime).Value = DBNull.Value;

            cmd.Parameters.Add("@comment", System.Data.SqlDbType.Text).Value = status.Comment;

            cmd.Parameters.Add("@status", System.Data.SqlDbType.NChar).Value = status.Status;

        }



        private string GetUpdateCommand()
        {
            string update = @"UPDATE [dbo].[BatchStatus]
                SET [start_time] = @start_time
              ,[start_load_time] = @start_load_time
              ,[start_transform_time] = @start_transform_time
              ,[start_save_time] = @start_save_time
              ,[start_cleanup_time] = @start_cleanup_time
              ,[finish_time] = @finish_time
              ,[comment] = @comment
              ,[status] = @status
                WHERE [batchname] = @batchname AND [batchid] = @batchid";
            return update;
        }

        #endregion
    }
}
