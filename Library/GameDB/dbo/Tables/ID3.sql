CREATE TABLE [dbo].[ID3] (
    [I] INT              IDENTITY (1, 1) NOT NULL,
    [G] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ID3] PRIMARY KEY CLUSTERED ([I] ASC),
    CONSTRAINT [IX_ID3] UNIQUE NONCLUSTERED ([G] ASC)
);

