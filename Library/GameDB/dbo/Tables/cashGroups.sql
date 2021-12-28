CREATE TABLE [dbo].[cashGroups] (
    [ID]      UNIQUEIDENTIFIER NOT NULL,
    [GroupID] INT              NOT NULL,
    [Locked]  TINYINT          NOT NULL,
    CONSTRAINT [PK_cashGroups] PRIMARY KEY CLUSTERED ([ID] ASC, [GroupID] ASC)
);

