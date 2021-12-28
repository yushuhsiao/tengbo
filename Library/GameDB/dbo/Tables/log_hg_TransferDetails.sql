CREATE TABLE [dbo].[log_hg_TransferDetails] (
    [SN]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [Accountid]         VARCHAR (100)   NULL,
    [transact_id]       CHAR (16)       NULL,
    [transacttype_code] VARCHAR (100)   NULL,
    [TransactionType]   VARCHAR (100)   NULL,
    [transact_time]     DATETIME        NULL,
    [amount]            DECIMAL (16, 2) NULL,
    [currency_code]     CHAR (3)        NULL,
    [reference_no]      CHAR (16)       NULL,
    [notes]             VARCHAR (100)   NULL,
    CONSTRAINT [PK_log_hg_TransferDetails] PRIMARY KEY CLUSTERED ([SN] ASC),
    CONSTRAINT [IX_log_hg_TransferDetails] UNIQUE NONCLUSTERED ([Accountid] ASC, [transact_id] ASC, [transacttype_code] ASC, [TransactionType] ASC, [transact_time] ASC, [amount] ASC, [currency_code] ASC, [reference_no] ASC, [notes] ASC)
);

