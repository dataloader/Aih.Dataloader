using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Aih.DataLoader;
using Aih.DataLoader.Interfaces;
using Aih.DataLoader.StatusHandlers;
using Aih.DataLoader.ConfigHandlers;

namespace tests
{
    public class TestDataLoaderTester
    {
        private readonly string _connection;

        public TestDataLoaderTester()
        {
            string sqlCommand = DBCreatorScripts.GetSqlLoaderConfigHandlerTestSqlSetup() + Environment.NewLine + DBCreatorScripts.GetSqlStatusHandlerTestSqlSetup();
            string dbName = "DataLoaderTestDB";
            LocalDb.CreateLocalDb(dbName, sqlCommand, true);

            _connection = LocalDb.GetConnectionString(dbName);  
        }


        [Fact]
        public void RunTestLoaderTest()
        {
            //string connection = @"Data Source=localhost\SQLEXPRESS;Database=LoaderDB;Integrated Security=True;";
            
            ILoaderConfigHandler configHandler= new SQLServerLoaderConfigHandler(_connection);
            IStatusHandler statusHandler = new SQLServerStatusHandler(_connection);

            TestDataLoader loader = new TestDataLoader();
            loader.InitializeHandlers(configHandler, statusHandler);
            loader.RunDataLoader();

            bool callCountOK = loader.CallCount == 5;
            bool configCountOK = loader.ConfigCount == 8;

            Assert.True(callCountOK && configCountOK);
        }


    }
}
