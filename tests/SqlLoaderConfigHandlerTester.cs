
using System.Collections.Generic;
using Aih.DataLoader.Interfaces;
using Aih.DataLoader.ConfigHandlers;

using Xunit;

namespace tests
{
    public class SqlLoaderConfigHandlerTester
    {

        private readonly string _connection;
        private ILoaderConfigHandler _configHandler;

        public SqlLoaderConfigHandlerTester()
        {
            string dbName = "LoaderTestDB";
            LocalDb.CreateLocalDb(dbName, DBCreatorScripts.GetSqlLoaderConfigHandlerTestSqlSetup(), true);

            _connection = LocalDb.GetConnectionString(dbName);  //@"Data Source=localhost\SQLEXPRESS;Database=LoaderDB;Integrated Security=True;";
            _configHandler = new SQLServerLoaderConfigHandler(_connection);

        }

        [Fact]
        public void GetOnlyLoaderConfigTest()
        {
            Dictionary<string, string> config = _configHandler.GetLoaderConfig("SimpleTestConfig", "SimpleTestLoader");

            //Two system config
            //Two for the simpletestconfig

            Assert.True(config.Count == 4);
        }


        [Fact]
        public void GetJointConfigTest()
        {
            //Get config for TestLoader in CmplicatedTestConfig -> special attention to the key jointkey1 -> should get specific value

            Dictionary<string, string> config = _configHandler.GetLoaderConfig("tests", "TestDataLoader");

            bool gotSpecificVal = config["jointkey1"] == "specific value";
            bool correctCount = config.Count == 7;

            Assert.True(gotSpecificVal && correctCount);
        }


    }
}
