CREATE TABLE [dbo].[log_hg_Betinfo1] (
    [ID]           UNIQUEIDENTIFIER CONSTRAINT [DF_log_hg_Betinfo1_ID] DEFAULT (newid()) NOT NULL,
    [CreateTime]   DATETIME         CONSTRAINT [DF_log_hg_Betinfo1_createtime] DEFAULT (getdate()) NOT NULL,
    [BetStartDate] DATETIME         NULL,
    [BetEndDate]   DATETIME         NULL,
    [AccountId]    VARCHAR (100)    NULL,
    [TableId]      CHAR (16)        NULL,
    [TableName]    VARCHAR (100)    NULL,
    [GameId]       CHAR (24)        NULL,
    [BetId]        CHAR (32)        NULL,
    [BetAmount]    DECIMAL (16, 4)  NULL,
    [Payout]       DECIMAL (16, 4)  NULL,
    [Currency]     CHAR (3)         NULL,
    [GameType]     VARCHAR (100)    NULL,
    [BetSpot]      VARCHAR (80)     NULL,
    [BetNo]        CHAR (32)        NULL,
    CONSTRAINT [PK_log_hg_Betinfo1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_log_hg_Betinfo1] UNIQUE NONCLUSTERED ([BetStartDate] ASC, [BetEndDate] ASC, [AccountId] ASC, [TableId] ASC, [TableName] ASC, [GameId] ASC, [BetId] ASC, [BetAmount] ASC, [Payout] ASC, [Currency] ASC, [GameType] ASC, [BetSpot] ASC, [BetNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bet place time for a game by a 
player. Format MM/dd/yyyy 
hh:mm:ss Ex: 01/12/2011 03:59:59', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'BetStartDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Game end time for the bet placed 
by a player. Format MM/dd/yyyy 
hh:mm:ss Ex: 01/12/2011 
04:00:23', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'BetEndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Player Login name of a bet 
placed ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'AccountId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Table id for a Game type of a 
game where the bet was 
placed by a player. ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'TableId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Game id for which a bet was 
placed by a player', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'GameId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bet id for a game placed by a 
player. BetNo & BetId value 
will be same. ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'BetId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bet amount placed by a 
player for a game 
Ex:10.0000', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'BetAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Win or loss for the bet placed 
by a player for a game. Value 
positive means win. Value 
negative means loss 
Ex:10.0000, -10.0000', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'Payout';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Player currency code 
Ex: CNY, USD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'Currency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Game type name of a game 
for a bet by a player 
Ex: Baccarat, Roulette ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'GameType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bet placed spot for a game 
by a player. Ex: Banker, Split 
bet: 20 and 23', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'BetSpot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bet id for a game placed by a 
player. BetNo & BetId value 
will be same.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'log_hg_Betinfo1', @level2type = N'COLUMN', @level2name = N'BetNo';

