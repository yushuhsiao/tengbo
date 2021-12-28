CREATE TABLE [dbo].[Game] (
    [ID]         INT            NOT NULL,
    [Name]       NVARCHAR (50)  NOT NULL,
    [Locked]     TINYINT        NOT NULL,
    [BonusW]     DECIMAL (9, 6) NULL,
    [BonusL]     DECIMAL (9, 6) NULL,
    [CreateTime] DATETIME       CONSTRAINT [DF_Game_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT            NOT NULL,
    [ModifyTime] DATETIME       CONSTRAINT [DF_Game_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT            NOT NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED ([ID] ASC)
);

