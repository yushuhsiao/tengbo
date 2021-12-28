CREATE TABLE [dbo].[Bank] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (20) NOT NULL,
    [Locked]     TINYINT       NOT NULL,
    [WebATM]     VARCHAR (200) NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_Bank_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      CONSTRAINT [DF_Bank_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_Bank] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Bank] UNIQUE NONCLUSTERED ([Name] ASC)
);

