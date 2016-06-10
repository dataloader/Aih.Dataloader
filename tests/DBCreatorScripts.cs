using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tests
{
    public class DBCreatorScripts
    {

        public static string GetSqlLoaderConfigHandlerTestSqlSetup()
        {
            string sql = @"
                CREATE TABLE [dbo].[LoaderConfig](
	                [loaderContainerName] [nchar](100) NOT NULL,
	                [loadername] [nchar](100) NOT NULL,
	                [key] [nchar](100) NOT NULL,
	                [value] [nchar](250) NULL,
	                CONSTRAINT [PK_LoaderConfig] PRIMARY KEY CLUSTERED 
	                (
		                [loaderContainerName] ASC,
		                [loadername] ASC,
		                [key] ASC
	                )
                )


                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('SimpleTestConfig', 'SimpleTestLoader', 'key1', 'value1')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('SimpleTestConfig', 'SimpleTestLoader', 'key2', 'value2')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('tests', 'TestDataLoader', 'key1', 'value1')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('tests', 'TestDataLoader', 'key2', 'value2')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('tests', 'TestDataLoader', 'jointkey1', 'specific value')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('tests', '*', 'jointkey1', 'general value')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('tests', '*', 'containerkey1', 'containerval1')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('tests', '*', 'containerkey2', 'containerval2')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('*', '*', 'systemkey1', 'systemval1')
                INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('*', '*', 'systemkey2', 'systemval2')
                ";
            return sql;
        }



        public static string GetSqlStatusHandlerTestSqlSetup()
        {
            string sql = @"
CREATE TABLE [dbo].[BatchStatus](
	[batchname] [nchar](100) NOT NULL,
	[batchid] [nchar](50) NOT NULL,
	[batchrefrence] [nchar](50) NULL,
	[start_time] [datetime] NULL,
	[start_load_time] [datetime] NULL,
	[start_transform_time] [datetime] NULL,
	[start_save_time] [datetime] NULL,
	[start_cleanup_time] [datetime] NULL,
	[finish_time] [datetime] NULL,
	[comment] [text] NULL,
	[status] [nchar](20) NULL,
	[handled] [smallint] NULL,
	
	CONSTRAINT [PK_BatchStatus] PRIMARY KEY CLUSTERED 
	(
		[batchname] ASC,
		[batchid] ASC
	)
)

";

            return sql;
        }


    }



}
