EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'logDB'
GO
USE [master]
GO
ALTER DATABASE logDB SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
ALTER DATABASE logDB SET  SINGLE_USER 
GO
USE [master]
GO
/****** 物件:  Database [logDB]    指令碼日期: 02/25/2014 23:48:05 ******/
DROP DATABASE logDB
GO
RESTORE DATABASE logDB FROM  DISK = N'C:\tengfa\bak\logDB_backup_201408071200.bak' WITH  FILE = 1,  MOVE N'logDB' TO N'C:\Data\Sql\logDB.mdf',  MOVE N'logDB_log' TO N'C:\Data\Sql\logDB.ldf',  NOUNLOAD,  REPLACE,  STATS = 10
GO

