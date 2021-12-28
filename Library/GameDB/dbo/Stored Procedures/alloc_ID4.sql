CREATE procedure alloc_ID4 (@ID int=null output,@retry int=100, @verify bit=1)
as
	declare @GUID uniqueidentifier, @check varchar(100)
_begin:
	if @ID is not null goto _end
	set @GUID=newid()
	insert into ID4(G) values (@GUID)
	select @ID=I from ID4 nolock where G=@GUID
	set @check='check 0,1,2'
	if @ID in (0,1,2) goto _clear
	if not @verify=1 goto _next
	set @check='check Admin'
	if exists (select ID from [Admin] nolock where ID=@ID) goto _clear
	set @check='check Agent'
	if exists (select ID from Agent   nolock where ID=@ID) goto _clear
	set @check='check Member'
	if exists (select ID from Member  nolock where ID=@ID) goto _clear
	goto _next
_clear:
	select @ID, @check
	set @ID=null
_next:
	set @retry=@retry-1
	if @retry > 0 goto _begin
_end:
