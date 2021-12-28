CREATE TABLE [dbo].[Permission1] (
    [ID]     INT           IDENTITY (1, 1) NOT NULL,
    [Parent] INT           NOT NULL,
    [Sort]   INT           NOT NULL,
    [Code]   VARCHAR (50)  NOT NULL,
    [Name]   VARCHAR (50)  NULL,
    [Url]    VARCHAR (100) NULL,
    CONSTRAINT [PK_Permission1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Permission1] UNIQUE NONCLUSTERED ([Code] ASC)
);

