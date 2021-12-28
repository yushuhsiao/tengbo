CREATE procedure [dbo].[BetAmtDG_ag] (@ACTime smalldatetime)
as begin
	exec BetAmtDG_bak @ACTime,11
	exec BetAmtDG_bak @ACTime,12
	exec BetAmtDG_bak @ACTime,13

    insert into GameLog_BetAmtDG   (ACTime,  GameID,GameType              ,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount     ,BetAmountAct       ,Payout        ,CreateTime,CreateUser)
	select							ACTime,a.GameID,isnull(b.dst,a.GameID),CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(betAmount),sum(validBetAmount),sum(netAmount),getdate() ,0
	from GameLog_011 a with(nolock)
	left join GameTypes b with(nolock)
	on b.GameID=11 and convert(varchar,a.GameID)=b.src
	where a.ACTime=@ACTime and a.GameID in (11,12,13)
	group by						ACTime,a.GameID,CorpID,MemberID,ACNT,AgentID,AgentACNT,b.dst
--	select							ACTime,a.GameID,isnull(b.dst,gameType),CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(betAmount),sum(validBetAmount),sum(netAmount),getdate() ,0
--	from GameLog_011 a with(nolock)
--	left join GameTypes b with(nolock)
--	on b.GameID=11 and a.gameType=b.src
--	where a.ACTime=@ACTime and a.GameID in (11,12,13)
--	group by						ACTime,a.GameID,gameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,b.dst

	exec BetAmtDG_diff @ACTime,11
	exec BetAmtDG_diff @ACTime,12
	exec BetAmtDG_diff @ACTime,13
end
