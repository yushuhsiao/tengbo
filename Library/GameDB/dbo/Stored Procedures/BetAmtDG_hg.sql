CREATE procedure [dbo].[BetAmtDG_hg] (@ACTime smalldatetime)
as begin
	exec BetAmtDG_bak @ACTime,1

    insert into GameLog_BetAmtDG   (ACTime,GameID,GameType              ,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount               ,BetAmountAct               ,Payout               ,CreateTime,CreateUser)
	select							ACTime,1     ,b.dst,CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(isnull(BetAmount,0)),sum(isnull(BetAmountAct,0)),sum(isnull(Payout,0)),getdate() ,0
	from GameLog_001 a with(nolock)
	left join GameTypes b with(nolock)
	on b.GameID=1 and b.src='HG'
	where a.ACTime=@ACTime
	group by						ACTime,CorpID,MemberID,ACNT,AgentID,AgentACNT,b.dst
--	select							ACTime,1     ,isnull(b.dst,GameType),CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(isnull(BetAmount,0)),sum(isnull(BetAmountAct,0)),sum(isnull(Payout,0)),getdate() ,0
--	from GameLog_001 a with(nolock)
--	left join GameTypes b with(nolock)
--	on b.GameID=1 and a.GameType=b.src
--	where a.ACTime=@ACTime
--	group by						ACTime,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,b.dst

	exec BetAmtDG_diff @ACTime,1	
end
