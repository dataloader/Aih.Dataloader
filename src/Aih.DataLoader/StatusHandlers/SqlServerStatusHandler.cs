using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Aih.DataLoader.Interfaces;
using Aih.DataLoader.Models;

namespace Aih.DataLoader.StatusHandlers
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



        public IList<BatchStatus> GetUnhandledFailedBatches()
        {

            List<BatchStatus> batches = new List<BatchStatus>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(GetUnhandledFailedBatchesQuery()))
                {
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();

                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            BatchStatus status = new BatchStatus()
                            {
                                BatchName = reader.GetString(0).Trim(),
                                BatchId = reader.GetString(1).Trim(),
                                BatchRefrence = reader.IsDBNull(2) ? null : reader.GetString(2).Trim(),
                                StartTime = reader.IsDBNull(3) ? null : reader.GetDateTime(3) as DateTime?,
                                StartLoadTime = reader.IsDBNull(4) ? null : reader.GetDateTime(4) as DateTime?,
                                StartTransformTime = reader.IsDBNull(5) ? null : reader.GetDateTime(5) as DateTime?,
                                StartSaveTime = reader.IsDBNull(6) ? null : reader.GetDateTime(6) as DateTime?,
                                StartCleanupTime = reader.IsDBNull(7) ? null : reader.GetDateTime(7) as DateTime?,
                                FinishTime = reader.IsDBNull(8) ? null : reader.GetDateTime(8) as DateTime?,
                                Comment = reader.IsDBNull(9) ? "" : reader.GetString(9).Trim(),
                                Status = reader.IsDBNull(10) ? "" : reader.GetString(10).Trim()

                            };
                            batches.Add(status);
                        }
                        return batches;
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine("Error checking if batch exists.   Exception: " + exc.Message);
                        throw exc;
                    }
                }
            }
        }

        public bool SetBatchHandled(BatchStatus status)
        {
            if ((status.IsHandled == null) || (status.IsHandled == false))
            {
                status.IsHandled = true;
            }
            return UpdateBatchStatusRecord(status);
        }


        public BatchStatus GetStatusRecord(string batchname, string batchid)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(GetStatusRecordCommand()))
                {
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();

                    SetStatusRecordCommandParams(cmd, batchname, batchid);

                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            BatchStatus status = new BatchStatus()
                            {
                                BatchName = reader.GetString(0).Trim(),
                                BatchId = reader.GetString(1).Trim(),
                                BatchRefrence = reader.IsDBNull(2) ? null : reader.GetString(2).Trim(),
                                StartTime = reader.IsDBNull(3) ? null : reader.GetDateTime(3) as DateTime?,
                                StartLoadTime = reader.IsDBNull(4) ? null : reader.GetDateTime(4) as DateTime?,
                                StartTransformTime = reader.IsDBNull(5) ? null : reader.GetDateTime(5) as DateTime?,
                                StartSaveTime = reader.IsDBNull(6) ? null : reader.GetDateTime(6) as DateTime?,
                                StartCleanupTime = reader.IsDBNull(7) ? null : reader.GetDateTime(7) as DateTime?,
                                FinishTime = reader.IsDBNull(8) ? null : reader.GetDateTime(8) as DateTime?,
                                Comment = reader.IsDBNull(9) ? "" : reader.GetString(9).Trim(),
                                Status = reader.IsDBNull(10) ? "" : reader.GetString(10).Trim()

                            };
                            return status;
                        }

                        //If nothing is found
                        return null;
                    }
                    catch (SqlException exc)
                    {
                        Console.WriteLine("Error checking if batch exists.   Exception: " + exc.Message);
                        throw exc;
                    }
                }
            }
        }




        #region Very ugly private helper functions

        private string GetStatusRecordCommand()
        {
            return @"SELECT[batchname],[batchid],[batchrefrence],[start_time],[start_load_time],[start_transform_time],[start_save_time],[start_cleanup_time],[finish_time],[comment],[status],[handled] FROM [dbo].[BatchStatus] where [batchname] = @batchname and [batchid] = @batchid";
        }

        private void SetStatusRecordCommandParams(SqlCommand cmd, string batchname, string batchid)
        {
            cmd.Parameters.Add("@batchname", System.Data.SqlDbType.NChar).Value = batchname;
            cmd.Parameters.Add("@batchid", System.Data.SqlDbType.NChar).Value = batchid;
        }



        private string GetUnhandledFailedBatchesQuery()
        {
            return @"SELECT[batchname],[batchid],[batchrefrence],[start_time],[start_load_time],[start_transform_time],[start_save_time],[start_cleanup_time],[finish_time],[comment],[status],[handled] FROM [dbo].[BatchStatus] where handled is null AND   [status] = 'Failed'";
        }

        private string GetSetBatchHandledCommand()
        {
            return "UPDATE [dbo].[BatchStatus] SET [handled] = 1 WHERE [batchname] = @batchname AND [batchid] = @batchid";
        }

        private void SetBatchHandledCommandParams(SqlCommand cmd, BatchStatus status)
        {
            cmd.Parameters.Add("@batchname", System.Data.SqlDbType.NChar).Value = status.BatchName;
            cmd.Parameters.Add("@batchid", System.Data.SqlDbType.NChar).Value = status.BatchId;
        }


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
           ,[status]
           ,[handled]
            )
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
           ,@status
           ,@is_handled
            )";

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

            if (status.IsHandled != null)
                cmd.Parameters.Add("@is_handled", System.Data.SqlDbType.SmallInt).Value = status.IsHandled;
            else
                cmd.Parameters.Add("@is_handled", System.Data.SqlDbType.SmallInt).Value = DBNull.Value;

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
              ,[handled] = @is_handled
               WHERE [batchname] = @batchname AND [batchid] = @batchid";
            return update;
        }

        

        #endregion
    }
}
