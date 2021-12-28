CREATE procedure [dbo].[BetAmtDG_bbin](@ACTime smalldatetime)
as begin
	exec BetAmtDG_bak @ACTime,9

    insert into GameLog_BetAmtDG        (ACTime,GameID,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser)
	select                               ACTime,9     ,b.dst,CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(BetAmount * f_sum),sum(Commissionable * f_sum),sum(Payoff * f_sum),getdate(),0
	from GameLog_009 a with(nolock)
	left join GameTypes b with(nolock)
	on b.GameID=9 and b.src='BBIN'
	where a.ACTime=@ACTime
	group by                             ACTime,CorpID,MemberID,ACNT,AgentID,AgentACNT,b.dst
--	select                               ACTime,9     ,isnull(b.dst,gamekind),CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(BetAmount * f_sum),sum(Commissionable * f_sum),sum(Payoff * f_sum),getdate(),0
--	from GameLog_009 a with(nolock)
--	left join GameTypes b with(nolock)
--	on b.GameID=9 and convert(varchar(100),gamekind)=b.src
--	where a.ACTime=@ACTime
--	group by                             ACTime,gamekind,CorpID,MemberID,ACNT,AgentID,AgentACNT,b.dst

	exec BetAmtDG_diff @ACTime,9,0
end
