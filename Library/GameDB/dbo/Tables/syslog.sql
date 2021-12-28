CREATE TABLE [dbo].[syslog] (
    [sn]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [ct]       DATETIME      CONSTRAINT [DF_syslog_ct] DEFAULT (getdate()) NOT NULL,
    [msgid]    BIGINT        NOT NULL,
    [time]     DATETIME      NOT NULL,
    [grpid]    INT           NOT NULL,
    [category] VARCHAR (50)  NOT NULL,
    [message]  VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_syslog] PRIMARY KEY CLUSTERED ([sn] ASC)
);

