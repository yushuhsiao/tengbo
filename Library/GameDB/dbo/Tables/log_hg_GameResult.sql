CREATE TABLE [dbo].[log_hg_GameResult] (
    [SN]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Game_Id]     CHAR (24)     NULL,
    [StartTime]   DATETIME      NULL,
    [EndTime]     DATETIME      NULL,
    [GameType_Id] CHAR (16)     NULL,
    [GameType]    VARCHAR (50)  NULL,
    [AccountID]   VARCHAR (100) NULL,
    [Dealer]      VARCHAR (100) NULL,
    [BankerPoint] INT           NULL,
    [PlayerPoint] INT           NULL,
    [Tie]         INT           NULL,
    [BankerCard]  VARCHAR (100) NULL,
    [PlayerCard]  VARCHAR (100) NULL,
    [DragonPoint] INT           NULL,
    [TigerPoint]  INT           NULL,
    [DragonCard]  VARCHAR (100) NULL,
    [TigerCard]   VARCHAR (100) NULL,
    [Result]      VARCHAR (100) NULL,
    CONSTRAINT [PK_log_hg_GameResult] PRIMARY KEY CLUSTERED ([SN] ASC),
    CONSTRAINT [IX_log_hg_GameResult] UNIQUE NONCLUSTERED ([Game_Id] ASC, [StartTime] ASC, [EndTime] ASC, [GameType_Id] ASC, [GameType] ASC, [AccountID] ASC, [Dealer] ASC, [BankerPoint] ASC, [PlayerPoint] ASC, [Tie] ASC, [BankerCard] ASC, [PlayerCard] ASC, [DragonPoint] ASC, [TigerPoint] ASC, [DragonCard] ASC, [TigerCard] ASC)
);

