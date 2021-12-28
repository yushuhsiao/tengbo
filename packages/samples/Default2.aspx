<%@ Page Language="C#" MasterPageFile="~/samples/main.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<asp:Content ContentPlaceHolderID="head" Runat="Server">
    <script src="Scripts/jquery.floatinglayer.js"></script>
    <style type="text/css">
        html {
            height: 100%;
        }
        body {
            height: 100%;
            width: 100%;
            /*overflow: hidden;*/
            /*color: white;*/
            /*background-color: black;*/
            font-family: Arial, Helvetica, sans-serif;
        }
        .layer {
            position: fixed;
            left: 0px;
            top: 0px;
        }
        .fill, .fill_x {
            width: 100%;
        }
        .fill, .fill_y {
            height: 100%;
        }
        .nav_x {
            width: 200px;
        }
        .nav_y {
            height: 50px;
            vertical-align: bottom;
        }
        td {
            white-space: nowrap;
        }
        #logo {
        }
        #nav {
            top:20px;
            left:200px;
        }
        #info {
            left: auto;
            right: 0px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#logout').click(function () {
                call_api1({ AdminLogout: {} }, function (obj) {
                    if (obj.LoginResult.t1 == 1)
                        window.location.reload();
                });
            });
            $('#a').draggable();
            //$('.sidebar').resizable({ grid: [20, 10], handles: "e", distance: 30 })
            ;//.makeFloating({ position: { x: 0, y: 0 }, duration: 0 });
            //$('#nav_area').resizable({ grid: [20, 10], handles:"e", distance:30 });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" Runat="Server">
    <table class="layer fill" cellpadding="0" cellspacing="0">
        <tr><td class="nav_y" colspan="2"></td></tr>
        <tr><td class="nav_x"></td><td><iframe class="fill" src="frame.aspx"></iframe></td></tr>
    </table>
<%--    <div id="logo" class="layer">
        logo
    </div>
    <div id="nav" class="layer">
        nav
        <big>111</big>
    </div>
    <div id="info" class="layer">
        <div class="nav_y"></div>
        <div>2</div>
        <div>3</div>
        <div>4</div>
        
    </div>--%>
    <div class="layer fill_x">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr class="nav_y">
                <td style="width:200px; font-size:30px;">logo</td>
                <td>1 2 3 4 5</td>
                <td style="width:1px;">info</td>
            </tr>
        </table>
        <div style="float:left">
            <div>aaa</div>
            <div>aaa</div>
            <div>aaa</div>
            <div>aaa</div>
            <div>aaa</div>
        </div>
        <div style="float:right">
            <div>aa</div>
            <div>bbb</div>
        </div>
    </div>
<%--    <table id="info" class="layer" cellpadding="0" cellspacing="0">
        <tr class="nav_y" style="text-align:right"><th><%= UserInfo.Current.Name %></th></tr>
        <tr>
            <td>
                <button id="logout">logout</button>
            </td>
        </tr>
    </table>--%>
</asp:Content>
