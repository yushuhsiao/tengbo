CREATE TABLE [dbo].[MemberPayment] (
    [MemberID]   INT            NOT NULL,
    [GameID]     INT            NOT NULL,
    [GameSubID]  INT            NOT NULL,
    [BounsW]     DECIMAL (9, 6) NOT NULL,
    [BounsL]     DECIMAL (9, 6) NOT NULL,
    [ModifyTime] DATETIME       NOT NULL,
    CONSTRAINT [PK_MemberP] PRIMARY KEY CLUSTERED ([MemberID] ASC, [GameID] ASC, [GameSubID] ASC)
);

