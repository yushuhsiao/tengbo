﻿CREATE TABLE [dbo].[tranPromo2] (
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
    [CurrencyA]          VARCHAR (3)      NOT NULL,
    [CurrencyB]          VARCHAR (3)      NOT NULL,
    [CurrencyX]          DECIMAL (19, 6)  NOT NULL,
    [SerialNumber]       VARCHAR (16)     NOT NULL,
    [RequestIP]          VARCHAR (20)     NOT NULL,
    [CreateTime]         DATETIME         CONSTRAINT [DF_tranPromo2_CreateTime_1] DEFAULT (getdate()) NOT NULL,
    [CreateUser]         INT              NOT NULL,
    [ModifyTime]         DATETIME         CONSTRAINT [DF_tranPromo2_ModifyTime_1] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]         INT              NOT NULL,
    [Memo1]              NVARCHAR (100)   NULL,
    [Memo2]              NVARCHAR (100)   NULL,
    CONSTRAINT [PK_tranPromo2] PRIMARY KEY CLUSTERED ([ID] ASC)
);

