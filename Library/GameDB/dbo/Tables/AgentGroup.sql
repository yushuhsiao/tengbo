CREATE TABLE [dbo].[AgentGroup] (
    [CorpID]     INT           NOT NULL,
    [GroupID]    TINYINT       NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Locked]     TINYINT       NOT NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_AgentGroup_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      CONSTRAINT [DF_AgentGroup_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_AgentGroup] PRIMARY KEY CLUSTERED ([CorpID] ASC, [GroupID] ASC)
);

