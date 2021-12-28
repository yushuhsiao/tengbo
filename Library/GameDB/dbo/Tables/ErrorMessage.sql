CREATE TABLE [dbo].[ErrorMessage] (
    [code] INT           NOT NULL,
    [msg]  VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ErrorMessage] PRIMARY KEY CLUSTERED ([code] ASC)
);

