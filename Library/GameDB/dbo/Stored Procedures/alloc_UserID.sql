CREATE procedure [dbo].[alloc_UserID] (@ID int=null output,@retry int=100, @verify bit=1, @type varchar(10), @corpid int, @acnt varchar(20))
as
	declare @GUID uniqueidentifier--, @check varchar(100)
_begin:
	if @ID is not null goto _end
	set @GUID=newid()
	insert into ID1(G,T,C,A) values (@GUID,@type,@corpid,@acnt)
	select @ID=I from ID1 nolock where G=@GUID
	--set @check='check 0,1,2'
	if @ID in (0,1,2) goto _clear
	if not @verify=1 goto _next
	--set @check='check Admin'
	if exists (select ID from [Admin] nolock where ID=@ID) goto _clear
	--set @check='check Agent'
	if exists (select ID from Agent   nolock where ID=@ID) goto _clear
	--set @check='check Member'
	if exists (select ID from Member  nolock where ID=@ID) goto _clear
	--if exists (select ID from AdminGroup  nolock where ID=@ID) goto _clear
	--if exists (select ID from AgentGroup  nolock where ID=@ID) goto _clear
	--if exists (select ID from MemberGroup  nolock where ID=@ID) goto _clear
	goto _next
_clear:
	--select @ID, @check
	set @ID=null
_next:
	set @retry=@retry-1
	if @retry > 0 goto _begin
_end:
