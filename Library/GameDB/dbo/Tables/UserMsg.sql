CREATE TABLE [dbo].[UserMsg] (
    [ID]      UNIQUEIDENTIFIER NOT NULL,
    [UserID]  INT              NOT NULL,
    [CorpID]  INT              NOT NULL,
    [ACNT]    VARCHAR (20)     NOT NULL,
    [State]   INT              NOT NULL,
    [MsgTime] DATETIME         NOT NULL,
    [Title]   NVARCHAR (50)    NOT NULL,
    [Text]    NVARCHAR (200)   NOT NULL,
    CONSTRAINT [PK_UserMsg] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserMsg_ID]
    ON [dbo].[UserMsg]([UserID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserMsg_Time]
    ON [dbo].[UserMsg]([MsgTime] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserMsg_State]
    ON [dbo].[UserMsg]([State] ASC);

