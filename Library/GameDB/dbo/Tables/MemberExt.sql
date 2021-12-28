CREATE TABLE [dbo].[MemberExt] (
    [MemberID] INT          NOT NULL,
    [Field]    VARCHAR (20) NOT NULL,
    [Value]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_MemberExt] PRIMARY KEY CLUSTERED ([MemberID] ASC, [Field] ASC)
);

