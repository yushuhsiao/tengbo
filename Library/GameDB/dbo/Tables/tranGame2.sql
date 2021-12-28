﻿CREATE TABLE [dbo].[tranGame2] (
    [ID]           UNIQUEIDENTIFIER NOT NULL,
    [LogType]      INT              NOT NULL,
    [GameID]       INT              NOT NULL,
    [CorpID]       INT              NOT NULL,
    [ParentID]     INT              NOT NULL,
    [ParentACNT]   VARCHAR (20)     NOT NULL,
    [UserType]     TINYINT          NOT NULL,
    [UserID]       INT              NOT NULL,
    [UserACNT]     VARCHAR (20)     NOT NULL,
    [UserName]     NVARCHAR (20)    NOT NULL,
    [GameACNT]     VARCHAR (50)     NULL,
    [AcceptTime]   DATETIME         NULL,
    [FinishTime]   DATETIME         NOT NULL,
    [State]        INT              NOT NULL,
    [Amount]       DECIMAL (19, 6)  NOT NULL,
    [CurrencyA]    VARCHAR (3)      NOT NULL,
    [CurrencyB]    VARCHAR (3)      NOT NULL,
    [CurrencyX]    DECIMAL (19, 6)  NOT NULL,
    [SerialNumber] VARCHAR (16)     NOT NULL,
    [RequestIP]    VARCHAR (20)     NOT NULL,
    [CreateTime]   DATETIME         CONSTRAINT [DF_tranGame2_CreateTime_1] DEFAULT (getdate()) NOT NULL,
    [CreateUser]   INT              NOT NULL,
    [ModifyTime]   DATETIME         CONSTRAINT [DF_tranGame2_ModifyTime_1] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]   INT              NOT NULL,
    [Memo1]        VARCHAR (200)    NULL,
    [Memo2]        VARCHAR (200)    NULL,
    CONSTRAINT [PK_tranGame2] PRIMARY KEY CLUSTERED ([ID] ASC)
);

