CREATE TABLE [dbo].[MemberGroup] (
    [CorpID]     INT            NOT NULL,
    [GroupID]    TINYINT        NOT NULL,
    [Name]       NVARCHAR (50)  NOT NULL,
    [Locked]     TINYINT        NOT NULL,
    [BonusW]     DECIMAL (9, 6) NOT NULL,
    [BonusL]     DECIMAL (9, 6) NOT NULL,
    [CreateTime] DATETIME       CONSTRAINT [DF_MemberGroup_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT            NOT NULL,
    [ModifyTime] DATETIME       CONSTRAINT [DF_MemberGroup_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT            NOT NULL,
    CONSTRAINT [PK_MemberGroup] PRIMARY KEY CLUSTERED ([CorpID] ASC, [GroupID] ASC)
);

