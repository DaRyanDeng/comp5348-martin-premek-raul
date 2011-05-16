USE [Videos];



ALTER TABLE [dbo].[Users] 
    DROP COLUMN Revision;

GO	
	
ALTER TABLE [dbo].[Users] 
ADD [Revision] timestamp  NOT NULL;


GO


