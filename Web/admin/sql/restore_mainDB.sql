EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'tengfa'
GO
USE [master]
GO
ALTER DATABASE [tengfa] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
ALTER DATABASE [tengfa] SET  SINGLE_USER 
GO
USE [master]
GO
/****** 物件:  Database [gamelog]    指令碼日期: 02/25/2014 23:48:05 ******/
DROP DATABASE [tengfa]
GO
RESTORE DATABASE [tengfa] FROM  DISK = N'C:\tengfa\bak\tengfa_backup_201408071200.bak' WITH  FILE = 1,  MOVE N'tengfa' TO N'C:\Data\Sql\tengfa.mdf',  MOVE N'tengfa_log' TO N'C:\Data\Sql\tengfa.ldf',  NOUNLOAD,  REPLACE,  STATS = 10
GO
USE [master]
GO
ALTER DATABASE [tengfa] SET RECOVERY SIMPLE WITH NO_WAIT
GO
ALTER DATABASE [tengfa] SET RECOVERY SIMPLE 
GO
USE [tengfa]
GO
DBCC SHRINKDATABASE(N'tengfa' )
GO
USE [tengfa]
GO
DBCC SHRINKFILE (N'tengfa' , 0, TRUNCATEONLY)
GO
