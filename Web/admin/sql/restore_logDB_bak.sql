EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'logDB_bak'
GO
USE [master]
GO
ALTER DATABASE logDB_bak SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
ALTER DATABASE logDB_bak SET  SINGLE_USER 
GO
USE [master]
GO
/****** 物件:  Database [logDB_bak]    指令碼日期: 02/25/2014 23:48:05 ******/
DROP DATABASE logDB_bak
GO
RESTORE DATABASE logDB_bak FROM  DISK = N'C:\tengfa\bak\logDB_bak_backup_201408041816.bak' WITH  FILE = 1,  MOVE N'logDB_bak' TO N'C:\Data\Sql\logDB_bak.mdf',  MOVE N'logDB_bak_log' TO N'C:\Data\Sql\logDB_bak.ldf',  NOUNLOAD,  REPLACE,  STATS = 10
GO

