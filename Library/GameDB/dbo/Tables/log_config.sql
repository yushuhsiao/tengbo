CREATE TABLE [dbo].[log_config] (
    [_Key]      VARCHAR (50) NOT NULL,
    [_Active]   TINYINT      NOT NULL,
    [_Time]     DATETIME     NOT NULL,
    [_Len]      FLOAT (53)   NOT NULL,
    [_Reserved] FLOAT (53)   NOT NULL,
    CONSTRAINT [PK_log_config] PRIMARY KEY CLUSTERED ([_Key] ASC)
);

