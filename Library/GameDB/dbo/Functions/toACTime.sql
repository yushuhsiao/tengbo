
CREATE FUNCTION [toACTime](@d datetime)
RETURNS datetime
AS
BEGIN return dateadd(dd,datediff(dd,0,dateadd(hh,-12,@d)),0) END
