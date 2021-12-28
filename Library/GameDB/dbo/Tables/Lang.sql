CREATE TABLE [dbo].[Lang] (
    [cls]  VARCHAR (50)  NOT NULL,
    [lcid] INT           NOT NULL,
    [txt1] NVARCHAR (50) NOT NULL,
    [txt2] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Lang] PRIMARY KEY CLUSTERED ([cls] ASC, [lcid] ASC, [txt1] ASC)
);

