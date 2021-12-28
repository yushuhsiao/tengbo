CREATE TABLE [dbo].[cashYeepay] (
    [ID]               UNIQUEIDENTIFIER NOT NULL,
    [CorpID]           INT              NOT NULL,
    [LogType]          INT              NOT NULL,
    [Locked]           TINYINT          NOT NULL,
    [CreateTime]       DATETIME         CONSTRAINT [DF_cashYeepay_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]       INT              NOT NULL,
    [ModifyTime]       DATETIME         CONSTRAINT [DF_cashYeepay_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]       INT              NOT NULL,
    [FeesRate]         DECIMAL (9, 6)   NOT NULL,
    [alias_domain]     VARCHAR (100)    NULL,
    [authorizationURL] VARCHAR (100)    NULL,
    [merhantId]        VARCHAR (100)    NULL,
    [p5_Pid]           VARCHAR (100)    NULL,
    [p6_Pcat]          VARCHAR (100)    NULL,
    [p7_Pdesc]         VARCHAR (100)    NULL,
    [pa_MP]            VARCHAR (100)    NULL,
    [pd_FrpId]         VARCHAR (100)    NULL,
    [keyValue]         VARCHAR (100)    NULL,
    CONSTRAINT [PK_cashYeepay] PRIMARY KEY CLUSTERED ([ID] ASC)
);

