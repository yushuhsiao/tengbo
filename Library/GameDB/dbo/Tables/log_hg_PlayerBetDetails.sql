CREATE TABLE [dbo].[log_hg_PlayerBetDetails] (
    [SN]                              BIGINT          IDENTITY (1, 1) NOT NULL,
    [AccountID]                       VARCHAR (100)   NULL,
    [DateVal]                         DATETIME        NULL,
    [TimeRange]                       INT             NULL,
    [StakedAmount]                    DECIMAL (16, 4) NULL,
    [LiveGameTotalAmount]             DECIMAL (16, 4) NULL,
    [LiveGameExcludeEvenandTieAmount] DECIMAL (16, 4) NULL,
    CONSTRAINT [PK_log_hg_PlayerBetDetails] PRIMARY KEY CLUSTERED ([SN] ASC),
    CONSTRAINT [IX_log_hg_PlayerBetDetails] UNIQUE NONCLUSTERED ([AccountID] ASC, [DateVal] ASC, [TimeRange] ASC, [StakedAmount] ASC, [LiveGameTotalAmount] ASC, [LiveGameExcludeEvenandTieAmount] ASC)
);

