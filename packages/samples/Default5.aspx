<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default5.cs" Inherits="_Default" %>

<%@ Import Namespace="web" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="Scripts/jquery.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui/ui/jquery-ui.js"></script>
    <script type="text/javascript" src="Scripts/json2.js"></script>
    <script type="text/javascript" src="Scripts/util.js"></script>
    <link rel="stylesheet" href="Scripts/jquery-ui/themes/flick/jquery-ui.css" />
    <script type="text/javascript" src="Scripts/superfish/js/superfish.js"></script><%--
    <link href="Scripts/superfish/css/superfish.css" rel="stylesheet" />
    <link href="Scripts/superfish/css/superfish-navbar.css" rel="stylesheet" />--%>
    <link href="default.aspx.css" rel="stylesheet" />
    <script type="text/javascript">
        $.fn._autodock = function (obj) {
            obj.logo = $(obj.logo);
            obj.menu = $(obj.menu);
            obj.root = $(document);
            obj.main = this;
            var resize = (function () {
                obj.main.css({
                    top: obj.menu.height(),
                    height: (obj.root.height() - obj.menu.height() - 5) + 'px'
                });
            })();
            $(window).resize(resize);
            $(document).resize(resize);
            return this;
        };

        $.fn.init_sub_menu = function (menu) {
            for (var i = 0; i < menu.sub.length; i++) {
                var node = menu.sub[i];
                var li = $(document.createElement('li')).appendTo(this);
                //li.attr('op', node.op);
                var a = $(document.createElement('a')).appendTo(li);
                a.attr('target', $('#mainframe').attr('name'))
                a.text(node.txt);
                if (node.url != null)
                    a.attr('href', node.url);
                if (node.sub != null)
                    $(document.createElement('ul')).appendTo(li).init_sub_menu(node);
            }
            return this;
        };

        $.fn.init_menu = function (menu) {
            if (menu != null) {
                if (menu.Menu) return this.init_menu(menu.Menu);
                var menu0 = this.find('.top-nav');
                var tmp1 = this.find('label.navbar').removeClass('template');
                var tmp2 = this.find('table.navbar').removeClass('template');
                for (var i = 0; i < menu.sub.length; i++) {
                    var _class_id = 'nav' + i;
                    var node = menu.sub[i];
                    var label = tmp1.clone().appendTo(tmp1.parent());
                    label.text(node.txt);
                    label.attr('for', _class_id);
                    var table = tmp2.clone().insertBefore(this.find('.userinfo'));
                    table.addClass(_class_id);
                    table.find('ul.menu').init_sub_menu(node);
                }
                tmp1.remove();
                tmp2.remove();
                this.find('ul.menu').superfish();
                this.find('table.navbar').hide();
                this.find('label.navbar').hover(function () {
                    var _for = $(this).attr('for');
                    $('#menu table.navbar').each(function () {
                        var table = $(this);
                        if (table.hasClass(_for))
                            table.fadeIn('fast', 'linear');
                        else
                            table.fadeOut(0, 'linear');
                    });
                }, function () {
                });
            }
            return this;
        };

        var doc = (function () {
            function doc() { }
            doc.logout = function logout() {
                util.execute({
                    data: {
                        AdminLogout: {
                        }
                    },
                    success: function (obj) {
                        if (obj.LoginResult.t1 == 1) {
                            window.location.reload();
                        }
                    }
                });
            };
            return doc;
        })();
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#main')._autodock({ logo: '#logo', menu: '#menu' });
            //$('#menu')._navmenu();
            $('#menu').init_menu(<%= this.MenuString %>);
        });
    </script>
</head>
<body>
    <div id="logo">系統管理</div>
    <div id="menu">
        <table class="top-nav">
            <tr>
                <td class="fill"></td>
                <td>
                    <label class="navbar"></label>
                </td>
            </tr>
        </table>
        <table class="navbar">
            <tr>
                <td class="fill"></td>
                <td>
                    <ul class="menu">
                    </ul>
                </td>
            </tr>
        </table>
        <%--<table id="menu0"><tr><td class="fill"></td><td><%=Menu0%></td></tr></table><%=Menu1%>--%>
        <div class="userinfo"><%= Context.User.Identity.Name %></div>
        <ul class="menu settings">
            <li style="background-color:transparent;"><img alt="" src="images/icon_settings.png" width="20px" height="20px" />
                <ul>
                    <li></li>
                    <li class="btn"><%=Resources.res.change_password %></li>
                    <li class="btn"><%=Resources.res.login_hist %></li>
                    <li></li>
                    <li class="btn" onclick="doc.logout()"><%=Resources.res.logout_text %></li>
                    <li></li>
                </ul>
            </li>
        </ul>
    </div>
    <div id="main"><iframe id="mainframe" name="mainframe" frameborder="0" width="100%" height="100%"></iframe></div>
</body>
</html>
