﻿CREATE TABLE [dbo].[PromoTran1] (
    [ID]           UNIQUEIDENTIFIER NOT NULL,
    [LogType]      INT              NOT NULL,
    [GameID]       INT              NOT NULL,
    [CorpID]       INT              NOT NULL,
    [MemberID]     INT              NOT NULL,
    [MemberACNT]   VARCHAR (20)     NOT NULL,
    [AgentID]      INT              NOT NULL,
    [AgentACNT]    VARCHAR (20)     NOT NULL,
    [Amount1]      DECIMAL (19, 6)  NOT NULL,
    [Amount2]      DECIMAL (19, 6)  NOT NULL,
    [State]        INT              NOT NULL,
    [CurrencyA]    VARCHAR (3)      NOT NULL,
    [CurrencyB]    VARCHAR (3)      NOT NULL,
    [CurrencyX]    DECIMAL (19, 6)  NOT NULL,
    [SerialNumber] VARCHAR (16)     NOT NULL,
    [RequestIP]    VARCHAR (20)     NOT NULL,
    [FinishTime]   DATETIME         NULL,
    [CreateTime]   DATETIME         CONSTRAINT [DF_PromoTran1_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]   INT              NOT NULL,
    [ModifyTime]   DATETIME         CONSTRAINT [DF_PromoTran1_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]   INT              NOT NULL,
    [Memo1]        NVARCHAR (100)   NULL,
    [Memo2]        NVARCHAR (100)   NULL,
    CONSTRAINT [PK_PromoTran1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

