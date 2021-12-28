CREATE TABLE [dbo].[WorkingNote] (
    [ID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [NoteType]   TINYINT        CONSTRAINT [DF_WorkingNote_NoteType] DEFAULT ((0)) NOT NULL,
    [NoteState]  TINYINT        CONSTRAINT [DF_WorkingNote_NoteState] DEFAULT ((0)) NOT NULL,
    [Note]       NVARCHAR (MAX) NOT NULL,
    [CreateTime] DATETIME       CONSTRAINT [DF_WorkingNote_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT            NOT NULL,
    [ModifyTime] DATETIME       CONSTRAINT [DF_WorkingNote_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT            NOT NULL,
    CONSTRAINT [PK_WorkingNote] PRIMARY KEY CLUSTERED ([ID] ASC)
);

