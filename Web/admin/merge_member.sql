--EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'tengfa2'
--GO
--USE [master]
--GO
--ALTER DATABASE [tengfa2] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
--GO
--ALTER DATABASE [tengfa2] SET  SINGLE_USER 
--GO
--USE [master]
--GO
--/****** 物件:  Database [tengfa2]    指令碼日期: 02/12/2014 13:28:20 ******/
--DROP DATABASE [tengfa2]
--GO
--RESTORE DATABASE [tengfa2] FROM  DISK = N'C:\tengfa\bak\tengfa_backup_201402121653.bak' WITH  FILE = 1,  MOVE N'tengfa' TO N'c:\Sql\tengfa2.mdf',  MOVE N'tengfa_log' TO N'c:\Sql\tengfa2.ldf',  NOUNLOAD,  STATS = 10
--GO

use tengfa2
go
declare @ID1 int, @ID2 int

select @ID1=315, @ID2=566
--update Member			set Locked=1 where ID=@ID1
--update Member_005		set MemberID=@ID2 where MemberID=@ID1
--update GameLog_BetAmtDG	set CorpID=2, AgentID=307, AgentACNT='tengbo', MemberID=@ID2 where MemberID=@ID1 and GameID=5

select @ID1=328, @ID2=674
--update Member			set Locked=1 where ID=@ID1
--update Member_005		set MemberID=@ID2 where MemberID=@ID1
--update Member_006		set MemberID=@ID2 where MemberID=@ID1
--update GameLog_BetAmtDG	set CorpID=2, AgentID=307, AgentACNT='tengbo', MemberID=@ID2 where MemberID=@ID1 and GameID in (5,6)

select @ID1=376, @ID2=561
--update Member			set Locked=1 where ID=@ID1
--update Member_001		set MemberID=@ID2 where MemberID=@ID1
--update GameLog_001		set CorpID=2, AgentID=307, AgentACNT='tengbo', MemberID=@ID2 where MemberID=@ID1
--update GameLog_BetAmtDG	set CorpID=2, AgentID=307, AgentACNT='tengbo', MemberID=@ID2 where MemberID=@ID1 and GameID=1


go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[xx_log_replace]') AND type in (N'P', N'PC')) DROP PROCEDURE [dbo].[xx_log_replace]
go
create PROCEDURE [dbo].[xx_log_replace] (@ID int)
AS
BEGIN
	update GameLog_000			set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
    update GameLog_001			set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
	update GameLog_BetAmtDG		set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
	update GameTran1			set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
	update GameTran2			set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
	update MemberTran1			set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
	update MemberTran2			set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
	update PromoTran1			set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
	update PromoTran2			set CorpID=2, AgentID=307, AgentACNT='tengbo' where MemberID=@ID
END
go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[xx_move1]') AND type in (N'P', N'PC')) DROP PROCEDURE dbo.xx_move1
go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tmp1]') AND type in (N'U')) DROP TABLE [dbo].[tmp1]
go
CREATE TABLE [dbo].[tmp1]([ID] [int] NOT NULL,[ACNT] [varchar](20) NOT NULL,CONSTRAINT [PK_tmp1] PRIMARY KEY CLUSTERED ([ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]
go
insert into tmp1 (ID,ACNT) select ID,ACNT from Member nolock where CorpID=1 and AgentID=4 and Locked=0
go
declare @ID int, @ACNT varchar(20), @ID2 int
while 1=1
begin
	select @ID=null, @ACNT=null
	select top(1) @ID=ID, @ACNT=ACNT from tmp1 nolock
	if @ID is null break
	if (select count(*) from Member nolock where CorpID in (1,2) and ACNT=@ACNT) = 1
	begin
		--select @ID, @ACNT
		exec xx_log_replace @ID
		update LoginLog set CorpID=2 where ID=@ID
		update LoginState set CorpID=2 where ID=@ID
		update Member set CorpID=2, AgentID=307 where ID=@ID
	end
	else
	begin
		select @ID2=ID from Member nolock where ACNT=@ACNT and CorpID=2
		select * from Member nolock where ID in (@ID,@ID2)		
	end
	delete tmp1 where ID=@ID
end
