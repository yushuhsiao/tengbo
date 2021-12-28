-- 不同樹系將會回傳 null
CREATE function getAgentLevel (@AgentID int, @ParentID int=0)
returns int
AS
begin
	declare @r1 int, @r2 int, @cnt int
	select @r1=0, @cnt=100
	while @cnt>0
	begin
		select @AgentID=ParentID from Agent nolock where ID=@AgentID
		set @r1=@r1+1
		if @AgentID=@ParentID
		begin set @r2=@r1 break end
		if @AgentID is null or @AgentID=0 break
		set @cnt=@cnt-1
	end
	return @r2
end
