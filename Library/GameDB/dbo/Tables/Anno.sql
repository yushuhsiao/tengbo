CREATE TABLE [dbo].[Anno] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [CorpID]     INT            NOT NULL,
    [Name]       NVARCHAR (20)  NOT NULL,
    [Locked]     TINYINT        NOT NULL,
    [Sort]       INT            NOT NULL,
    [Text]       NVARCHAR (200) NOT NULL,
    [CreateTime] DATETIME       CONSTRAINT [DF_Anno_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT            NOT NULL,
    [ModifyTime] DATETIME       CONSTRAINT [DF_Anno_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT            NOT NULL,
    CONSTRAINT [PK_Anno] PRIMARY KEY CLUSTERED ([ID] ASC)
);

