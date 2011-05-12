USE [Videos];



ALTER TABLE [dbo].[Users] 
    DROP COLUMN Revision;

GO	
	
ALTER TABLE [dbo].[Users] 
ADD [Revision] timestamp  NOT NULL;


GO




ALTER TABLE [dbo].[SystemLogs] 
    DROP COLUMN [EventTime];

GO	



ALTER TABLE [dbo].[SystemLogs] 
ADD [EventTime] datetime NOT NULL DEFAULT GetDate();


GO


