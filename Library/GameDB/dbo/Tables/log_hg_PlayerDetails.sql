CREATE TABLE [dbo].[log_hg_PlayerDetails] (
    [SN]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [PlayerName]   VARCHAR (100)   NULL,
    [StartDate]    DATETIME        NULL,
    [EndDate]      DATETIME        NULL,
    [Bet_Amount]   DECIMAL (18, 2) NULL,
    [Bet_Payoff]   DECIMAL (18, 2) NULL,
    [TotalWin]     DECIMAL (18, 2) NULL,
    [DeductAmount] DECIMAL (18, 2) NULL,
    [EvenAmount]   DECIMAL (18, 2) NULL,
    [TotalAmount]  DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_log_hg_PlayerDetails] PRIMARY KEY CLUSTERED ([SN] ASC),
    CONSTRAINT [IX_log_hg_PlayerDetails] UNIQUE NONCLUSTERED ([PlayerName] ASC, [StartDate] ASC, [EndDate] ASC, [Bet_Amount] ASC, [Bet_Payoff] ASC, [TotalWin] ASC, [DeductAmount] ASC, [EvenAmount] ASC, [TotalAmount] ASC)
);

