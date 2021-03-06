CREATE TABLE [dbo].[Agent] (
    [ID]         INT             NOT NULL,
    [CorpID]     INT             NOT NULL,
    [ACNT]       VARCHAR (20)    NOT NULL,
    [GroupID]    TINYINT         NOT NULL,
    [ParentID]   INT             CONSTRAINT [DF_Agent_ParentID] DEFAULT ((0)) NOT NULL,
    [Name]       NVARCHAR (20)   NOT NULL,
    [pwd]        VARCHAR (50)    NOT NULL,
    [sec_pwd]    VARCHAR (50)    NULL,
    [Locked]     TINYINT         NOT NULL,
    [Currency]   VARCHAR (3)     NOT NULL,
    [Balance]    DECIMAL (19, 6) CONSTRAINT [DF_Agent_Balance] DEFAULT ((0)) NOT NULL,
    [TranFlag]   TINYINT         CONSTRAINT [DF_Agent_TranFlag] DEFAULT ((17)) NOT NULL,
    [PCT]        DECIMAL (9, 6)  CONSTRAINT [DF_Agent_PCT] DEFAULT ((0)) NOT NULL,
    [A_BonusW]   DECIMAL (9, 6)  CONSTRAINT [DF_Agent_A_BonusW] DEFAULT ((0)) NOT NULL,
    [A_BonusL]   DECIMAL (9, 6)  CONSTRAINT [DF_Agent_A_BonusL] DEFAULT ((0)) NOT NULL,
    [A_ShareW]   DECIMAL (9, 6)  CONSTRAINT [DF_Agent_A_ShareW] DEFAULT ((0)) NOT NULL,
    [A_ShareL]   DECIMAL (9, 6)  CONSTRAINT [DF_Agent_A_ShareL] DEFAULT ((0)) NOT NULL,
    [M_BonusW]   DECIMAL (9, 6)  CONSTRAINT [DF_Agent_M_BonusW] DEFAULT ((0)) NOT NULL,
    [M_BonusL]   DECIMAL (9, 6)  CONSTRAINT [DF_Agent_M_BonusL] DEFAULT ((0)) NOT NULL,
    [M_ShareW]   DECIMAL (9, 6)  CONSTRAINT [DF_Agent_M_ShareW] DEFAULT ((0)) NOT NULL,
    [M_ShareL]   DECIMAL (9, 6)  CONSTRAINT [DF_Agent_M_ShareL] DEFAULT ((0)) NOT NULL,
    [MaxMember]  INT             NULL,
    [MaxAgent]   INT             NULL,
    [MaxDepth]   INT             NULL,
    [CreateTime] DATETIME        CONSTRAINT [DF_Agent_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT             NOT NULL,
    [ModifyTime] DATETIME        CONSTRAINT [DF_Agent_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT             NOT NULL,
    CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Agent] UNIQUE NONCLUSTERED ([CorpID] ASC, [ACNT] ASC)
);

