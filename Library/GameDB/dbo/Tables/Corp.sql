CREATE TABLE [dbo].[Corp] (
    [ID]         INT           NOT NULL,
    [ACNT]       VARCHAR (20)  NOT NULL,
    [Name]       NVARCHAR (20) NOT NULL,
    [Locked]     TINYINT       NOT NULL,
    [Currency]   VARCHAR (3)   NOT NULL,
    [AdminACNT]  VARCHAR (20)  NOT NULL,
    [AgentACNT]  VARCHAR (20)  NOT NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_Corp_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      CONSTRAINT [DF_Corp_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_Corp] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Corp] UNIQUE NONCLUSTERED ([ACNT] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Corp', @level2type = N'COLUMN', @level2name = N'Name';

