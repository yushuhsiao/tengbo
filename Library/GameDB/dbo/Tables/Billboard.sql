CREATE TABLE [dbo].[Billboard] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [CorpID]     INT             NOT NULL,
    [RecordType] INT             NOT NULL,
    [MemberID]   INT             NULL,
    [MemberACNT] VARCHAR (20)    NULL,
    [Amount]     DECIMAL (19, 6) NOT NULL,
    [Locked]     TINYINT         NOT NULL,
    [CreateTime] DATETIME        CONSTRAINT [DF_Billboard_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT             NOT NULL,
    [ModifyTime] DATETIME        CONSTRAINT [DF_Billboard_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT             NOT NULL,
    CONSTRAINT [PK_Billboard] PRIMARY KEY CLUSTERED ([ID] ASC)
);

