CREATE PROCEDURE [dbo].[init_root]
AS
	if exists (select ID from Corp nolock where ID=0) return
	begin try
		begin tran
		exec corp_insert @ID=0, @ACNT='root',@Locked=0,@Currency='USD',@CreateUser=0
		--update [Admin] set pwd='xdujF8NRvcnZTgoElotWKQ==',Locked=0,ModifyTime=getdate() where ID=0
		commit tran
	end try
	begin catch
		rollback tran
	end catch
