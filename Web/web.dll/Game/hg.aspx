<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hg.aspx.cs" Inherits="hg_aspx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>HOGaming</title>
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript">
        $(document).ready(function () { $(window).resize(function () { $('#gameframe').height($(window).innerHeight() - $('#msg').outerHeight()); }).trigger('resize'); });
    </script>
    <style type="text/css">
        html, body { margin: 0; padding: 0; width: 100%; height: 100%; background-color: black; color: white; overflow: hidden; } 
        .gameframe { position: absolute; bottom: 0; left: 0; width: 100%; } 
    </style>
</head>
<body><iframe id="gameframe" class="gameframe" frameborder="0" runat="server"></iframe></body>
</html>
