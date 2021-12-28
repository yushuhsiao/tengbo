﻿CREATE TABLE [dbo].[AgentBank] (
    [AgentID]     INT           NOT NULL,
    [Index]       INT           NOT NULL,
    [CardID]      VARCHAR (30)  NOT NULL,
    [BankName]    NVARCHAR (20) NOT NULL,
    [AccountName] NVARCHAR (20) NOT NULL,
    [Loc1]        NVARCHAR (20) NULL,
    [Loc2]        NVARCHAR (20) NULL,
    [Loc3]        NVARCHAR (20) NULL,
    CONSTRAINT [PK_AgentBank] PRIMARY KEY CLUSTERED ([AgentID] ASC, [Index] ASC)
);

