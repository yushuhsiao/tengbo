CREATE TABLE [dbo].[UserProfile] (
    [ID]  INT            NOT NULL,
    [txt] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED ([ID] ASC)
);

