CREATE procedure [dbo].[BetAmtDG_diff] (@ACTime smalldatetime, @GameID int,@digit int=2)
AS
BEGIN
	insert into GameLog_BetAmtDG (ACTime,GameID,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,CreateTime,CreateUser,BetAmount,BetAmountAct,Payout)
	select a.ACTime,a.GameID,'誤差',a.CorpID,a.MemberID,a.ACNT,a.AgentID,a.AgentACNT,getdate(),0
,a.BetAmount   -isnull(b.BetAmount    ,0)
,a.BetAmountAct-isnull(b.BetAmountAct ,0)
,a.Payout      -isnull(b.Payout       ,0)
	from       (select ACTime,GameID,CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(BetAmount) BetAmount,sum(BetAmountAct) BetAmountAct,sum(Payout) Payout
				from GameLog_BetAmtDG_bak with(nolock)
				where ACTime=@ACTime and GameID=@GameID
				group by ACTime,GameID,CorpID,MemberID,ACNT,AgentID,AgentACNT) a
	left join  (select ACTime,GameID,CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(BetAmount) BetAmount,sum(BetAmountAct) BetAmountAct,sum(Payout) Payout
				from GameLog_BetAmtDG     with(nolock)
				where ACTime=@ACTime and GameID=@GameID
				group by ACTime,GameID,CorpID,MemberID,ACNT,AgentID,AgentACNT) b
	on a.ACTime=b.ACTime and a.GameID=b.GameID and a.MemberID=b.MemberID
	where  round(a.BetAmount    ,@digit) <> round(isnull(b.BetAmount    ,0),@digit)
		or round(a.BetAmountAct ,@digit) <> round(isnull(b.BetAmountAct ,0),@digit)
		or round(a.Payout       ,@digit) <> round(isnull(b.Payout       ,0),@digit)
END