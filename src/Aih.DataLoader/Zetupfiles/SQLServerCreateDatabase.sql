USE [master]
GO

CREATE DATABASE LoaderDB
GO

USE LoaderDB
GO

--Being depriciated
--CREATE TABLE [dbo].[BatchProperties](
--	[batchname] [nchar](250) NOT NULL,
--	[key] [nchar](100) NOT NULL,
--	[value] [nchar](250) NULL,
--	CONSTRAINT [PK_BatchProperties] PRIMARY KEY CLUSTERED 
--	(
--		[batchname] ASC,
--		[key] ASC
--	)
--)
--GO



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
GO


USE LoaderDB
GO

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

GO
