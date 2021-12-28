<%@ Page Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" Inherits="page" Culture="auto" UICulture="auto" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="web" %>
<%@ Import Namespace="web.data" %>
<%@ Import Namespace="webAPI" %>
<%@ Import Namespace="Tools.Protocol" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Resources" %>

<script runat="server">
    public string MenuString;

    public Menu menu;

    protected void Page_Load(object sender, EventArgs e)
    {
        User user = Context.User as User;
        Code2Row menu0 = Codes.Instance.Menu;
        //UrlList urllist = UrlList.GetInstance(this.Context);
        //UrlListSection urllist = UrlListSection.GetSection();
        Menu menu1 = this.menu = new Menu(this, menu0, user, /*urllist, */CultureInfo.CurrentUICulture.LCID);
        menu1.Childs = menu1.Childs ?? new List<Menu>();
        this.MenuString = JsonProtocol.SerializeObject(menu1);
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Menu
    {
        //[JsonProperty("op")]
        public int Code;
        [JsonProperty("txt")]
        public string Text;
        [JsonProperty("url")]
        public string Url;
        [JsonProperty("sub")]
        public List<Menu> Childs;

        public Menu(page page, Code2Row node, User user, /*UrlListSection urllist, */int lcid)
        {
            this.Code = node.Code.Value;
            //this.Text = page.get  this.GetLocalResourceObject();// node.GetText(lcid);
            string resid = node.resid ?? node.Code.ToString();
            this.Text = page.lang["Menu_" + resid] ?? resid;
            string url = node.Path;
            if (!string.IsNullOrEmpty(node.Path))
            {
                if (url.StartsWith("~/"))
                    url = url.Substring(2);
                this.Url = url;
            }
            for (int i = 0; i < node.Childs.Count; i++)
            {
                if (user.Permissions2[node.Childs[i].Code.Value])
                {
                    if (this.Childs == null)
                        this.Childs = new List<Menu>();
                    this.Childs.Add(new Menu(page, node.Childs[i], user, /*urllist, */lcid));
                }
            }
        }
    }

</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link href="css/themes/start/jquery-ui.css" rel="stylesheet" />
    <link <%= "id=\"css_jquery_ui_theme\""%> href="css/themes/<%=res.ui_theme%>/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <%--<link href="default.aspx.css" rel="stylesheet" />--%>
    <%--<link href="css/style.css" rel="stylesheet" />--%>
    <script type="text/javascript">

        function logout() {
            $.invoke_api({ AdminLogout: {} }, {
                success: function (obj) {
                    if (obj.LoginResult.t1 == 1) {
                        window.location.reload();
                    }
                }
            });
        }

        $(document).ready(function () {

            //function submenu(menu) {
            //    var $ul = $('<ul></ul>');
            //    if (menu.sub) {
            //        for (var i = 0; i < menu.sub.length; i++) {
            //            var item = menu.sub[i];
            //            var $li = $('<li></li>').appendTo($ul);
            //            $li[0].url = item.url;
            //            $li.click(function () {
            //                $(this).find('a').each(function () {
            //                    $('#main').attr('src', this.href);
            //                });
            //                //console.log(this.url);
            //            });
            //            var $a = $('<a target="mainframe">{0}</a>'.format(item.txt)).appendTo($li);
            //            if (item.url) $a.prop('href', item.url);
            //            submenu(item).appendTo($li);
            //        }
            //    }
            //    return $ul;
            //}
            //
            //(function (menu) {
            //    if (menu == null)
            //        return;
            //    var $tabs = $('#tabs');
            //    var $ul = $tabs.find(' > ul')
            //    for (var i = 0; i < menu.sub.length; i++) {
            //        var item = menu.sub[i];
            //        var name = 'nav_' + i;
            //        var $li = $('<li><a href="#{0}">{1}</a></li>'.format(name, item.txt)).appendTo($ul);
            //        var $div = $('<div class="menu" id="{0}">'.format(name)).appendTo($tabs);
            //        var $sub = submenu(item).appendTo($div);
            //    }
            //    $tabs.tabs({ active: 2 }).removeClass('ui-corner-all');
            //    $ul.removeClass('ui-corner-all');
            //    $('.menu > ul').addClass('ui-menu');
            //    $('.menu li').addClass('ui-menu-item').addClass('ui-widget-content')
            //        .hover(function () { $(this).addClass('ui-state-hover'); }, function () { $(this).removeClass('ui-state-hover'); })
            //        .click(function () {
            //            $('.menu li').removeClass('ui-state-active');
            //            $(this).addClass('ui-state-active');
            //        });
            //
            //});//(< %=JsonProtocol.SerializeObject(this.menu)%>);

            var $tabs = $('.nav-tabs');
            var $ul = $tabs.find(' > ul')
            $tabs.tabs({ active: 2 }).removeClass('ui-corner-all');
            $ul.removeClass('ui-corner-all');
            $ul.find('.menu').appendTo($tabs);
            $('.menu > ul').addClass('ui-menu');
            $('.menu li').addClass('ui-menu-item').addClass('ui-widget-content')
                .hover(function () { $(this).addClass('ui-state-hover'); }, function () { $(this).removeClass('ui-state-hover'); })
                .click(function () { $('.menu li').removeClass('ui-state-active'); $(this).addClass('ui-state-active'); });

            $('.theme_select').click(function () {
                window._theme = $(this).text();
                $('#css_jquery_ui_theme').attr('href', 'css/themes/{0}/jquery-ui.css'.format(window._theme));
                raise_theme_change();
                //$(main.contentDocument).find('#css_jquery_ui_theme').attr('href', '../' + href);
            });

            $(window).resize(function () {
                $('#main').closest('div').height($(window).innerHeight() - $('#header').closest('div').outerHeight());
            }).trigger('resize');
            //$('#main').load(function () { $(this).contents().find('#css_jquery_ui_theme').attr('href', 'Scripts/jquery-ui/themes/dark-hive/jquery-ui.css') });
        });
    </script>
    <style type="text/css">
        .ui-widget { font-size: 1em; } 
        html, body { margin: 0; padding: 0; overflow: hidden; width: 100%; height: 100%; } 
        ul, li { list-style: none; margin: 0; padding: 0; white-space: nowrap; } 
        .ui-menu { padding: 0; } 
        .ui-menu .ui-menu-item { width: auto; padding-left: 1em; padding-right: 1em; } 
        td { white-space: nowrap; } 
        .ui-tabs .ui-tabs-nav { padding-top: 1px; }
        .ui-tabs .ui-tabs-nav li a { padding-top: .2em; padding-bottom: .1em; } 
        .ui-tabs .ui-tabs-panel { padding-top: -0px; padding-left: .2em; padding-right: 6px; } 
        .ui-tabs-nav > li { line-height: none; } 
        .menu > ul li { position: relative; white-space: nowrap; }
        .menu > ul ul { position: absolute; visibility: hidden; top: 100%; left: -1px; }
        .menu > ul > li { float: left; }
        .menu > ul li:hover > ul { visibility: visible; }
        .menu > ul ul ul { top: 0px; left: 100%; }
        .menu { /*padding: 0;*/ /*height: 22px;*/ font-size: 0.8em; }
        .menu li { /*background-color: #eeeeee;*/ /*border: 1px solid #555555;*/ /*padding: 3px 1em 3px 1em;*/ /*margin-top: 1px;*/ /*margin-left: 2px;*/ }
        .menu li:hover { /*background-color:#aaaaaa;*/ }
        .menu a { /*text-decoration: none;*/ /*color:#cccccc;*/ /*background-color: #eeeeee;*/ /*border: 1px solid #555555;*/ /*margin: 3px 1em 3px 1em;*/ }
        .menu a:link, .menu a:visited { /*color:#000000;*/ /*background-color:#ffffff;*/ }
        .menu a:active .menu a:hover { /*background-color:#aaaaaa;*/ }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div style="position:absolute; left:0; bottom:0; width:100%; height:100px">
        <iframe id="main" name="mainframe" frameborder="0" style="width:100%; height:100%"></iframe>
    </div>
    <div style="position:absolute; left:0; top:0; width:100%; height:55px; ">
        <table id="header" style="width:100%; height:100%;" cellspacing="0" cellpadding="0">
            <tr>
                <td class="ui-state-default" style="text-align:center; font-size:1.5em; width:200px; border:0;">系統管理</td>
                <td class="nav-tabs" style="vertical-align:top; padding:0; border:0;" id="tabs" runat="server">
                    <% MenuRow menu = MenuRow.GetMenu();
                       User user = HttpContext.Current.User as User; %>
                    <ul id="tabs_ul" runat="server" clientidmode="Inherit">
                        <li style="float:right;"><a href="#nav_right"><span class="ui-icon ui-icon-gear" style="float:left;"></span>&nbsp;</a></li>
                        <li style="float:right;"><a><%=User.ACNT%></a></li>                               
                        <% int n;
                           foreach (MenuRow m0 in menu.Childs)
                           {
                               n = menu.Childs.IndexOf(m0);
                               %><li><a href="#nav_<%=n%>"><%=m0.Name%></a><div class="menu" id="nav_<%=n%>"><%
                               if (m0.Childs != null)
                               {
                                   %><ul class="ui-menu"><%
                                   foreach (MenuRow m1 in m0.Childs)
                                   {
                                       n = menu.Childs.IndexOf(m1);
                                       %><li class="ui-menu-item ui-widget-content"><a <%if (!string.IsNullOrEmpty(m1.Url)) {%>href=<%=ResolveUrl(m1.Url)%> target="mainframe" <%}%>><%=m1.Name%></a><%
                                       if (m1.Childs != null)
                                       {
                                           %><ul><%
                                           foreach (MenuRow m2 in m1.Childs)
                                           {
                                               n = menu.Childs.IndexOf(m2);
                                               %><li class="ui-menu-item ui-widget-content"><a <%if (!string.IsNullOrEmpty(m2.Url)) {%>href=<%=ResolveUrl(m2.Url)%> target="mainframe" <%}%>><%=m2.Name%></a><%
                                               if (m2.Childs != null)
                                               {
                                                   %><ul><%
                                                   foreach (MenuRow m3 in m2.Childs)
                                                   {
                                                       n = menu.Childs.IndexOf(m3);
                                                       %><li class="ui-menu-item ui-widget-content"><a <%if (!string.IsNullOrEmpty(m3.Url)) {%>href=<%=ResolveUrl(m3.Url)%> target="mainframe" <%}%>><%=m3.Name%></a><%
                                                       if (m3.Childs != null)
                                                       {
                                                           %><ul><%
                                                           foreach (MenuRow m4 in m3.Childs)
                                                           {
                                                               n = menu.Childs.IndexOf(m4);
                                                               %><li class="ui-menu-item ui-widget-content"><a <%if (!string.IsNullOrEmpty(m4.Url)) {%>href=<%=ResolveUrl(m4.Url)%> target="mainframe" <%}%>><%=m4.Name%></a><%
                                                               if (m4.Childs != null)
                                                               {
                                                                   %><ul><%
                                                                   foreach (MenuRow m5 in m4.Childs)
                                                                   {
                                                                       n = menu.Childs.IndexOf(m5);
                                                                       %><li class="ui-menu-item ui-widget-content"><a <%if (!string.IsNullOrEmpty(m5.Url)) {%>href=<%=ResolveUrl(m5.Url)%> target="mainframe" <%}%>><%=m5.Name%></a></li><%
                                                                   } %></ul><%
                                                               } %></li><%
                                                           } %></ul><%
                                                       } %></li><%
                                                   } %></ul><%
                                               } %></li><%
                                           } %></ul><%
                                       } %></li><%
                                   } %></ul><%
                               } %></div></li><%
                           } %>
                    </ul>
                    <div class="menu" id="nav_right" style="float:right; padding-bottom:0;">
                        <ul>
                            <li><a><%=res.change_password %></a></li>
                            <li><a href="admin/AccountInfo.aspx" target="mainframe"><%=res.AccountInfo%></a></li>
                            <li><a>theme</a>
                                <ul><% foreach (string s in new string[] { "base", "black-tie", "blitzer", "cupertino", "dark-hive", "dot-luv", "eggplant", "excite-bike", "flick", "hot-sneaks", "humanity", "le-frog", "mint-choc", "overcast", "pepper-grinder", "redmond", "smoothness", "south-street", "start", "sunny", "swanky-purse", "trontastic", "ui-darkness", "ui-lightness" })
                                       { %>
                                    <li><a class="theme_select"><%=s%></a></li><% } %>
                                </ul>
                            </li>
                            <li onclick="logout()"><a><%=res.logout_text %></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
