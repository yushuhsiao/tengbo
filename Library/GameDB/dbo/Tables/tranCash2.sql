﻿CREATE TABLE [dbo].[tranCash2] (
    [ID]                 UNIQUEIDENTIFIER NOT NULL,
    [LogType]            INT              NOT NULL,
    [CorpID]             INT              NOT NULL,
    [ProviderParentID]   INT              NOT NULL,
    [ProviderParentACNT] VARCHAR (20)     NOT NULL,
    [ProviderID]         INT              NOT NULL,
    [ProviderACNT]       VARCHAR (20)     NOT NULL,
    [ParentID]           INT              NOT NULL,
    [ParentACNT]         VARCHAR (20)     NOT NULL,
    [UserType]           TINYINT          NOT NULL,
    [UserID]             INT              NOT NULL,
    [UserACNT]           VARCHAR (20)     NOT NULL,
    [UserName]           NVARCHAR (20)    NOT NULL,
    [AcceptTime]         DATETIME         NULL,
    [FinishTime]         DATETIME         NOT NULL,
    [State]              INT              NOT NULL,
    [Amount]             DECIMAL (19, 6)  NOT NULL,
    [CashAmount]         DECIMAL (19, 6)  NOT NULL,
    [Fees1]              DECIMAL (19, 6)  NOT NULL,
    [Fees1x]             DECIMAL (19, 6)  NULL,
    [Fees2]              DECIMAL (19, 6)  NOT NULL,
    [Fees2x]             DECIMAL (19, 6)  NULL,
    [PCT]                DECIMAL (9, 6)   NOT NULL,
    [CurrencyA]          VARCHAR (3)      NOT NULL,
    [CurrencyB]          VARCHAR (3)      NOT NULL,
    [CurrencyX]          DECIMAL (19, 6)  NOT NULL,
    [SerialNumber]       VARCHAR (16)     NOT NULL,
    [RequestIP]          VARCHAR (20)     NOT NULL,
    [CreateTime]         DATETIME         CONSTRAINT [DF_tranCash2_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]         INT              NOT NULL,
    [ModifyTime]         DATETIME         CONSTRAINT [DF_tranCash2_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]         INT              NOT NULL,
    [Memo1]              NVARCHAR (100)   NULL,
    [Memo2]              NVARCHAR (100)   NULL,
    [a_BankName]         NVARCHAR (20)    NULL,
    [a_CardID]           VARCHAR (30)     NULL,
    [a_Name]             NVARCHAR (20)    NULL,
    [a_TranTime]         DATETIME         NULL,
    [a_TranSerial]       VARCHAR (30)     NULL,
    [a_TranMemo]         NVARCHAR (100)   NULL,
    [b_BankName]         NVARCHAR (20)    NULL,
    [b_CardID]           VARCHAR (30)     NULL,
    [b_Name]             NVARCHAR (20)    NULL,
    [b_TranTime]         DATETIME         NULL,
    [b_TranSerial]       VARCHAR (30)     NULL,
    [b_TranMemo]         NVARCHAR (100)   NULL,
    CONSTRAINT [PK_tranCash2] PRIMARY KEY CLUSTERED ([ID] ASC)
);

