CREATE procedure [dbo].[BetAmtDG_bak](@ACTime smalldatetime, @GameID int)
as begin
	declare @MemberID int
_del:
	set @MemberID=null
	select top(1) @MemberID=MemberID from GameLog_BetAmtDG nolock where ACTime=@ACTime and GameID=@GameID and CreateUser<>0
	group by ACTime,GameID,MemberID
--	select @MemberID
	if @MemberID is null goto _end
	delete GameLog_BetAmtDG_bak where ACTime=@ACTime and GameID=@GameID and MemberID=@MemberID
    insert into GameLog_BetAmtDG_bak (sn,ACTime,GameID,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser)
	select                            sn,ACTime,GameID,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser
	from GameLog_BetAmtDG nolock   where ACTime=@ACTime and GameID=@GameID and MemberID=@MemberID and CreateUser<>0
	delete GameLog_BetAmtDG        where ACTime=@ACTime and GameID=@GameID and MemberID=@MemberID
	goto _del
_end:
	delete GameLog_BetAmtDG        where ACTime=@ACTime and GameID=@GameID
	
--	delete from GameLog_BetAmtDG_bak 
--		   from GameLog_BetAmtDG
--	where     GameLog_BetAmtDG.ACTime    =GameLog_BetAmtDG_bak.ACTime
--		  and GameLog_BetAmtDG.GameID    =GameLog_BetAmtDG_bak.GameID
--		  and GameLog_BetAmtDG.GameType  =GameLog_BetAmtDG_bak.GameType
--		  and GameLog_BetAmtDG_bak.ACTime=@ACTime
--		  and GameLog_BetAmtDG_bak.GameID=@GameID
--
--    insert into GameLog_BetAmtDG_bak (sn,ACTime,GameID,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser)
--	select                            sn,ACTime,GameID,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser
--	from GameLog_BetAmtDG nolock   where ACTime=@ACTime and GameID=@GameID and CreateUser<>0
--
end
