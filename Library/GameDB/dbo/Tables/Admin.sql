CREATE TABLE [dbo].[Admin] (
    [ID]         INT           NOT NULL,
    [CorpID]     INT           NOT NULL,
    [ACNT]       VARCHAR (20)  NOT NULL,
    [GroupID]    TINYINT       NOT NULL,
    [Name]       NVARCHAR (20) NOT NULL,
    [pwd]        VARCHAR (50)  NOT NULL,
    [Locked]     TINYINT       NOT NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_Admin_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      CONSTRAINT [DF_Admin_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Admin] UNIQUE NONCLUSTERED ([CorpID] ASC, [ACNT] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所屬公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'CorpID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'ACNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'密碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Admin', @level2type = N'COLUMN', @level2name = N'pwd';

