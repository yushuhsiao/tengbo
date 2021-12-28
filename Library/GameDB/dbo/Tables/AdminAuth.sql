CREATE TABLE [dbo].[AdminAuth] (
    [AdminID]    INT          NOT NULL,
    [header]     VARCHAR (20) NULL,
    [idstr]      VARCHAR (50) NOT NULL,
    [rsakey]     XML          NOT NULL,
    [Locked]     TINYINT      NOT NULL,
    [ExpireTime] DATETIME     NOT NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_AdminAuth_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT          NOT NULL,
    [ModifyTime] DATETIME     CONSTRAINT [DF_AdminAuth_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT          NOT NULL,
    CONSTRAINT [PK_AdminAuth] PRIMARY KEY CLUSTERED ([AdminID] ASC)
);

