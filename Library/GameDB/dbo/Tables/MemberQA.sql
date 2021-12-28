CREATE TABLE [dbo].[MemberQA] (
    [SN]       INT            IDENTITY (1, 1) NOT NULL,
    [MemberID] INT            NOT NULL,
    [Q]        NVARCHAR (100) NOT NULL,
    [A]        NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_MemberQA] PRIMARY KEY CLUSTERED ([SN] ASC)
);

