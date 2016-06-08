INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('SimpleTestConfig', 'SimpleTestLoader', 'key1', 'value1')
INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('SimpleTestConfig', 'SimpleTestLoader', 'key2', 'value2')



INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('CmplicatedTestConfig', 'TestLoader', 'key1', 'value1')
INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('CmplicatedTestConfig', 'TestLoader', 'key2', 'value2')

INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('CmplicatedTestConfig', 'TestLoader', 'jointkey1', 'specific value')
INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('CmplicatedTestConfig', '*', 'jointkey1', 'general value')

INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('CmplicatedTestConfig', '*', 'containerkey1', 'containerval1')
INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('CmplicatedTestConfig', '*', 'containerkey2', 'containerval2')



INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('*', '*', 'systemkey1', 'systemval1')
INSERT INTO [dbo].[LoaderConfig] ([loaderContainerName],[loadername],[key],[value]) VALUES ('*', '*', 'systemkey2', 'systemval2')