CREATE TABLE [dbo].[log_hg] (
    [Interval]          INT      CONSTRAINT [DF_log_hg_Reload] DEFAULT ((10000)) NOT NULL,
    [GameLog_Time]      DATETIME CONSTRAINT [DF_log_hg_GameLog_Time] DEFAULT ('2013-01-1') NOT NULL,
    [GameLog_Interval]  INT      CONSTRAINT [DF_log_hg_GameLog_Delay] DEFAULT ((15)) NOT NULL,
    [Betinfo1_Time]     DATETIME CONSTRAINT [DF_log_hg_Betinfo1_Time] DEFAULT ('2013-09-30') NOT NULL,
    [Betinfo1_Start]    INT      CONSTRAINT [DF_log_hg_Betinfo1_Start] DEFAULT ((-30)) NOT NULL,
    [Betinfo1_End]      INT      CONSTRAINT [DF_log_hg_Betinfo1_End] DEFAULT ((60)) NOT NULL,
    [Betinfo2_Time]     DATETIME CONSTRAINT [DF_log_hg_Betinfo2_Time] DEFAULT ('2013-09-30') NOT NULL,
    [Betinfo2_Interval] INT      CONSTRAINT [DF_log_hg_Betinfo2_Interval] DEFAULT ((14400)) NOT NULL,
    [GameResult_Time]   DATETIME CONSTRAINT [DF_log_hg_GameResult_Time] DEFAULT ('2013-09-30') NOT NULL,
    [GameResult_Start]  INT      CONSTRAINT [DF_log_hg_GameResult_Start] DEFAULT ((-30)) NOT NULL,
    [GameResult_End]    INT      CONSTRAINT [DF_log_hg_GameResult_End] DEFAULT ((60)) NOT NULL,
    [Transfer_Time]     DATETIME CONSTRAINT [DF_log_hg_Transfer_Time] DEFAULT ('2013-1-1') NOT NULL,
    [Transfer_Start]    INT      CONSTRAINT [DF_log_hg_Transfer_Start] DEFAULT ((-30)) NOT NULL,
    [Transfer_End]      INT      CONSTRAINT [DF_log_hg_Transfer_End] DEFAULT ((60)) NOT NULL,
    CONSTRAINT [PK_log_hg] PRIMARY KEY CLUSTERED ([Interval] ASC)
);

