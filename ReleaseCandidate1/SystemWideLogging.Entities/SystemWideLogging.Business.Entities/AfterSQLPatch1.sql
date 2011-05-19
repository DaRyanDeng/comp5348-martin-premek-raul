USE [SystemWideLog];


ALTER TABLE [dbo].[SystemLog] 
    DROP COLUMN [EventTime];

GO	



ALTER TABLE [dbo].[SystemLog] 
ADD [EventTime] datetime NOT NULL DEFAULT GetDate();


GO


