using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using Aih.DataLoader.Interfaces;
using Aih.DataLoader.StatusHandlers;
using Aih.DataLoader.Models;

namespace tests
{
    public class SqlServerStatusHandlerTester
    {

        //private readonly string _connection;
        private SQLServerStatusHandler _handler;

        public SqlServerStatusHandlerTester()
        {
            string dbName = "LoaderStatusHandlerTestDB";
            LocalDb.CreateLocalDb(dbName, DBCreatorScripts.GetSqlStatusHandlerTestSqlSetup(), true);

           // _connection = LocalDb.GetConnectionString(dbName);  

            _handler = new SQLServerStatusHandler(LocalDb.GetConnectionString(dbName));
            //string test = "";
        }



        [Fact]
        public void GetUnhandledFailedBatchesTest()
        {
            BatchStatus status = new BatchStatus()
            {
                BatchName = "TESTBatch",
                BatchId = Guid.NewGuid().ToString(),
                Status = "Failed",
                StartTime = DateTime.Now,
                BatchRefrence = "Refrence",
                Comment = "Starting"
            };

            _handler.CreateBatchStatusRecord(status);

            IList<BatchStatus> _statusList = _handler.GetUnhandledFailedBatches();
            Assert.True(_statusList.Count > 0);

            _handler.SetBatchHandled(status);
        }


        [Fact]
        public void UpdateBatchStatusRecordTest()
        {
            BatchStatus status = new BatchStatus()
            {
                BatchName = "TESTBatch",
                BatchId = Guid.NewGuid().ToString(),
                Status = "Failed",
                StartTime = DateTime.Now,
                BatchRefrence = "Refrence",
                Comment = "Starting"
            };

            _handler.CreateBatchStatusRecord(status);

            IList<BatchStatus> statusList = _handler.GetUnhandledFailedBatches();

            foreach(var stat in statusList)
            {
                _handler.SetBatchHandled(stat);
            }

            IList<BatchStatus> statusList2 = _handler.GetUnhandledFailedBatches();

            Assert.True(statusList2.Count == 0);
        }



        [Fact]
        public void GetBatchStatusRecordTest()
        {
            BatchStatus status = new BatchStatus()
            {
                BatchName = "TESTBatch",
                BatchId = Guid.NewGuid().ToString(),
                Status = "Failed",
                StartTime = DateTime.Now,
                BatchRefrence = "Refrence",
                Comment = "Starting"
            };

            _handler.CreateBatchStatusRecord(status);


            BatchStatus status2 = _handler.GetStatusRecord(status.BatchName, status.BatchId);


            Assert.True(status2.Status == "Failed");
        }


    }
}
