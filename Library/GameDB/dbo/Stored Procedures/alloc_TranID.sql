
CREATE procedure [dbo].[alloc_TranID] (@ID uniqueidentifier output, @sn varchar(16) output,@prefix varchar(3), @retry int=100, @size int=8)
as
	declare @I int
	while @retry>0
	begin
		set @ID=newid()
		insert into ID2(G,S) values (@ID,@prefix)
		select @I=I from ID2 nolock where G=@ID
		if @I is not null
		begin
			set @sn=@prefix+right('0000000000000000' + convert(varchar, @I), @size)
			break
		end
		set @retry=@retry-1
		set @I=null
	end
