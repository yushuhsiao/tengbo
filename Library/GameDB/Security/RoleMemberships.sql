EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'tfweb';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'tfweb';

