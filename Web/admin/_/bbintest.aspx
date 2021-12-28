<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="js/jquery.js"></script>
</head>
<body>    
    <script type="text/javascript">
        $.ajax({
            type: 'get',
            url: 'http://888.ddt518.com/app/WebService/JSON/display.php/CheckUserBalance/',
            dataType: 'json',
            crossDomain: true,
            beforeSend: function (jqXHR, settings) {
                console.log('beforeSend', arguments);
            },
            //cache: false,
            //async: true,
            //data: { },
            success: function (data, textStatus, jqXHR) {
                console.log('success', arguments);
            },
            error: function (jqXHR, ajaxOptions, thrownError) {
                console.log('error', arguments);
            },
            complete: function (jqXHR, textStatus) {
                console.log('complete', arguments);
            }
        });
    </script>
</body>
</html>
