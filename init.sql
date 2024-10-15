USE master;
GO

CREATE LOGIN GolinksAppUser WITH PASSWORD = '673Uj3nsEFlzGCiXlB';
GO

CREATE DATABASE Golinks;
GO

USE Golinks;
GO

CREATE USER GolinksAppUser FOR LOGIN GolinksAppUser;
GO

ALTER ROLE db_owner ADD MEMBER GolinksAppUser;
GO
