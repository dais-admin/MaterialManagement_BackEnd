USE [master]; -- Switch to master database to create login
GO

IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'IIS APPPOOL\MyAspNetCoreApi')
BEGIN
    CREATE LOGIN [IIS APPPOOL\MyAspNetCoreApi] 
      FROM WINDOWS WITH DEFAULT_DATABASE=[DIAS.MaterialDB_Dev], 
      DEFAULT_LANGUAGE=[us_english];
END
GO

USE [DIAS.MaterialDB_Dev]; -- Switch to the target database
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'Disha1')
BEGIN
    CREATE USER [Disha1] 
      FOR LOGIN [IIS APPPOOL\MyAspNetCoreApi];
END
GO

EXEC sp_addrolemember 'db_owner', 'Disha1';
GO
