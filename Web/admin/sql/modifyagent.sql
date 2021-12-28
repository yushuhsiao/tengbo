declare @id int, @ai int, @aa varchar(20)
select @id=13, @ai=1033, @aa='test'
update			Member				set AgentID=@ai					where ID=@id
select * from	Member nolock										where ID=@id
update			GameLog_000			set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
select * from	GameLog_000 nolock									where MemberID=@id
update			GameLog_001			set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
select * from	GameLog_001 nolock									where MemberID=@id
update			GameLog_BetAmtDG	set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
select * from	GameLog_BetAmtDG nolock								where MemberID=@id
update			GameTran1			set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
update			GameTran2			set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
select * from	GameTran1 nolock									where MemberID=@id
select * from	GameTran2 nolock									where MemberID=@id
update			MemberTran1			set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
update			MemberTran2			set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
select * from	MemberTran1 nolock									where MemberID=@id
select * from	MemberTran2 nolock									where MemberID=@id
update			PromoTran1			set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
update			PromoTran2			set AgentID=@ai, AgentACNT=@aa	where MemberID=@id
select * from	PromoTran1 nolock									where MemberID=@id
select * from	PromoTran2 nolock									where MemberID=@id
