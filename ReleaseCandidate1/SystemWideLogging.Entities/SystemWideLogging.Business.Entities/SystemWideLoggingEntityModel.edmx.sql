
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/13/2011 19:45:26
-- Generated from EDMX file: E:\Usyd\COMP5348\Assignment2\Ver36\SystemWideLogging.Entities\SystemWideLogging.Business.Entities\SystemWideLoggingEntityModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SystemWideLog];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SystemLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemLogs];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SystemLog'
CREATE TABLE [dbo].[SystemLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Source] nvarchar(max)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [EventTime] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SystemLog'
ALTER TABLE [dbo].[SystemLog]
ADD CONSTRAINT [PK_SystemLog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------