CREATE TABLE [dbo].[Currency] (
    [A]          VARCHAR (3)     NOT NULL,
    [B]          VARCHAR (3)     NOT NULL,
    [X]          DECIMAL (19, 6) NOT NULL,
    [ModifyTime] DATETIME        CONSTRAINT [DF_Currency_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] VARCHAR (50)    NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([A] ASC, [B] ASC)
);

