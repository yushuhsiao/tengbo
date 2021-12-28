<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="web.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #Text1 {
            width: 82px;
        }
    </style>
</head>
<body>
    <table>
        <tr>
            <td>
    <input type="radio" />轉帳到遊戲<input type="checkbox" />預扣<br />
    <input type="radio" />轉回主帳戶
            </td>
            <td>
    <input type="text" /><br />
            </td>
            <td>
    <input type="button" value="submit" />
            </td>
        </tr>
    </table>
</body>
</html>
