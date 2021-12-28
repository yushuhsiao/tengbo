CREATE TABLE [dbo].[LoginState] (
    [ID]         INT              NOT NULL,
    [UserType]   TINYINT          CONSTRAINT [DF_LoginState_UserType] DEFAULT ((0)) NOT NULL,
    [LoginID]    UNIQUEIDENTIFIER NOT NULL,
    [IP]         VARCHAR (20)     NOT NULL,
    [SessionID]  VARCHAR (50)     NULL,
    [CorpID]     INT              NOT NULL,
    [GroupID]    TINYINT          NOT NULL,
    [ACNT]       VARCHAR (20)     NOT NULL,
    [ClassName]  VARCHAR (100)    NULL,
    [ExpireTime] DATETIME         NOT NULL,
    [LoginTime]  DATETIME         NOT NULL,
    [KickTime]   DATETIME         NOT NULL,
    [Count]      INT              CONSTRAINT [DF_LoginState_Count] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LoginState] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_LoginState]
    ON [dbo].[LoginState]([SessionID] ASC);

