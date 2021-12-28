CREATE TABLE [dbo].[cashDinapy] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [CorpID]     INT              NOT NULL,
    [LogType]    INT              NOT NULL,
    [Locked]     TINYINT          NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_cashDinapy_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_cashDinapy_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    [FeesRate]   DECIMAL (9, 6)   NOT NULL,
    [M_ID]       VARCHAR (20)     NOT NULL,
    [M_URL]      VARCHAR (100)    NOT NULL,
    [action_Url] VARCHAR (100)    NOT NULL,
    [sec_key]    VARCHAR (50)     NOT NULL,
    CONSTRAINT [PK_cashDinapy] PRIMARY KEY CLUSTERED ([ID] ASC)
);

