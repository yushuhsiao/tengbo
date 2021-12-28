CREATE TABLE [dbo].[Config] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [CorpID]      INT            NOT NULL,
    [Category]    VARCHAR (20)   NOT NULL,
    [Key]         VARCHAR (20)   NOT NULL,
    [Value]       VARCHAR (1000) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Config] UNIQUE NONCLUSTERED ([CorpID] ASC, [Category] ASC, [Key] ASC)
);

