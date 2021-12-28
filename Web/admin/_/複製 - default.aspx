<%@ Page Language="C#" MasterPageFile="~/default.Master" AutoEventWireup="true" Inherits="page" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="web" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Tools.Protocol" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Resources" %>

<script runat="server">
    public string MenuString;

    public Menu menu;

    protected void Page_Load(object sender, EventArgs e)
    {
        User user = Context.User as User;
        Codes.Node menu0 = Codes.Instance.Menu;
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

        public Menu(page page, Codes.Node node, User user, /*UrlListSection urllist, */int lcid)
        {
            this.Code = node.Code;
            //this.Text = page.get  this.GetLocalResourceObject();// node.GetText(lcid);
            string resid = node.resid ?? node.Code.ToString();
            this.Text = page.lang[resid] ?? resid;
            string url = node.Path;
            if (!string.IsNullOrEmpty(node.Path))
            {
                if (url.StartsWith("~/"))
                    url = url.Substring(2);
                this.Url = url;
            }
            for (int i = 0; i < node.Childs.Count; i++)
            {
                //if (user.Permissions[node.Childs[i].Code])
                {
                    if (this.Childs == null)
                        this.Childs = new List<Menu>();
                    this.Childs.Add(new Menu(page, node.Childs[i], user, /*urllist, */lcid));
                }
            }
        }
    }

</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link href="css/themes/start/jquery-ui.css" rel="stylesheet" />
    <link <%= "id=\"css_jquery_ui_theme\""%> href="css/themes/<%=res.ui_theme%>/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <%--<link href="default.aspx.css" rel="stylesheet" />--%>
    <%--<link href="css/style.css" rel="stylesheet" />--%>
    <script type="text/javascript">

        function auto_size(logo, menu, main) {
            body = $(document.body);
            logo = $(logo);
            menu = $(menu);
            main = $(main);
            (resize = function (eventObject) {
                var h1 = menu.height();
                main.css({
                    top: h1 + 'px',
                    height: (body.height() - h1 - 2) + 'px'
                });
            })();
            $(window).resize(resize);
        }

        function init_submenu(ul, menu) {
            if (ul == null) return;
            if (menu.sub == null) return;
            for (var i = 0; i < menu.sub.length; i++) {
                var node = menu.sub[i];
                var li = $(document.createElement('li')).appendTo(ul);
                var a = $(document.createElement('a')).appendTo(li);
                a.text(node.txt);
                if (node.url != null) {
                    a.attr('target', 'mainframe');
                    a.attr('href', node.url);
                }
                if (node.sub != null)
                    if (node.sub.length > 0)
                        init_submenu($(document.createElement('ul')).appendTo(li), node);
            }
        }

        $.fn.set_hover = function () {
            $('.hover').removeClass('hover');
            this.addClass('hover').find('>a').addClass('hover');
            //$('#dbg').text(++cnt);
            return this;
        }

        //var cnt = 0;
        function init_menu(menu) {
            if (menu != null) {
                var ul = $('#menu > ul');
                var li = ul.find('li.fill');
                var tmp = ul.find('li.template').removeClass('template').remove();
                for (var i = 0; i < menu.sub.length; i++) {
                    var node = menu.sub[i];
                    li = tmp.clone().removeClass('fill').insertAfter(li);
                    li.addClass('item' + i);
                    if (i == 0)
                        li.set_hover();
                    li.find('a').text(node.txt);
                    init_submenu(li.find('ul'), node);
                    li.hover(function () {
                        $(this).set_hover();
                    }, function () {
                    });
                }
                //menu_item = $('#menu .item');
                //console.log($('#menu > ul > li'));
                //ul.superfish();
            }
        }

        function logout() {
            $.invoke_api({ AdminLogout: {} }, {
                success: function (obj) {
                    if (obj.LoginResult.t1 == 1) {
                        window.location.reload();
                    }
                }
            });
            //util.execute({
            //    data: {
            //        AdminLogout: {
            //        }
            //    },
            //    success: function (obj) {
            //        if (obj.LoginResult.t1 == 1) {
            //            window.location.reload();
            //        }
            //    }
            //});
        }


        $(document).ready(function () {

            function submenu(menu) {
                var $ul = $('<ul></ul>');
                if (menu.sub) {
                    for (var i = 0; i < menu.sub.length; i++) {
                        var item = menu.sub[i];
                        var $li = $('<li></li>').appendTo($ul);
                        var $a = $('<a target="mainframe">{0}</a>'.format(item.txt)).appendTo($li);
                        if (item.url)
                            $a.prop('href', item.url);
                        submenu(item).appendTo($li);
                    }
                }
                return $ul;
            }

            (function (menu) {
                if (menu == null)
                    return;
                var $nav = $('.header .nav');
                var $menu = $('.header .menu');
                for (var i = 0; i < menu.sub.length; i++) {
                    var item = menu.sub[i];
                    var name = 'nav_' + i;
                    var $input = $('<input type="radio" name="nav" id="' + name + '" />').appendTo($nav);
                    var $label = $('<label for="' + name + '">' + item.txt + '</label>').appendTo($nav);
                    var $sub = submenu(item).appendTo($menu).hide();
                    $sub.attr('id', 'sub' + name);
                }
                $nav.buttonset().find('input[name="nav"]').click(function () {
                    $menu.children().hide();
                    $menu.find('#sub' + this.id).show();
                });
            })(<%=JsonProtocol.SerializeObject(this.menu)%>)

            $(window).resize(function () {
                $('#main').closest('div').height($(window).innerHeight() - $('.header').outerHeight());
            }).trigger('resize');

            //new auto_size('#logo', '#menu', '#main');
            <%--init_menu(<%=this.MenuString %>);--%>
            //$('#main').load(function () { $(this).contents().find('#css_jquery_ui_theme').attr('href', 'Scripts/jquery-ui/themes/dark-hive/jquery-ui.css') });

        });
    </script>
    <style type="text/css">
        .ui-widget {
            font-size:1.0em;
        }
        .ui-button-text-only .ui-button-text {
            padding: .1em 1em;
        }
        html, body {
           margin: 0;
           padding: 0;
           overflow: hidden;
           width:100%;
           height:100%;
        }
        .header {
            /*position: absolute;*/
            /*left: 0;*/
            /*top: 0;*/
            /*width: 100%;*/
            /*height: 60px;*/
            /*border-collapse: collapse;*/
            /*border-spacing: 0px;*/
            /*background-image:none;*/
        }
        .header > td {
            white-space: nowrap;
        }
        .logo {
            /*width: 200px;*/
            /*text-align: center;*/
            /*font-size: 1.5em;*/
        }
        .info {
            width:1%;
            padding-left: 10px;
            padding-right: 10px;
        }
        .setting {
            width: 1%;
            padding-right: 10px;
        }
        .nav {
            height: 1%;
            padding: 0;
            vertical-align: bottom;
        }
        .nav label {
            /*border-top-width: 0;*/
            border-bottom-width: 0;
            padding-bottom:0;
            /*border-left-width: 0;*/
            /*border-right-width: 0;*/
        }
        /*.menu a { color:black }*/
        .menu ul, .menu li { list-style:none; margin:0; padding:0; white-space:nowrap; }
        .menu > ul li { position: relative; white-space: nowrap; }
        .menu > ul ul { position: absolute; visibility:hidden; top: 100%; left: 0; }
        .menu > ul > li { float: left; }
        .menu > ul li:hover > ul { visibility:visible; }
        .menu > ul ul ul { top: 0; left: 100%; }
        .menu {
            padding: 0;
            height: 22px;
            font-size: 0.8em;
        }
        .menu li {
            background-color: #eeeeee;
		    /*border: 1px solid #555555;*/
		    padding: 3px 1em 3px 1em;
		    margin-top: 1px;
		    margin-left: 1px;
        }
        .menu a {
		    color:#cccccc;
        }
        .menu a:link,
        .menu a:visited {
            color:#000000;
            background-color:#ffffff;
        }
        .menu a:active
        .menu a:hover {
            background-color:#aaaaaa;
        }

    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <div style="position:absolute; left:0; bottom:0; width:100%; height:100px">
        <iframe id="main" name="mainframe" frameborder="0" style="width:100%; height:100%"></iframe>
    </div>
    <table class="header ui-state-default" style="position: absolute; top:0; left:0; width:100%; height:60px;" cellspacing="0" cellpadding="0">
        <tr>
            <td rowspan="3" class="logo" style="text-align:center; font-size:1.5em; width:200px;">系統管理</td>
            <td colspan="3"></td>
        </tr>
        <tr class="ui-widget-header">
            <td class="nav"></td>
            <td class="info"><%=User.ACNT%></td>
            <td class="setting"><img alt="" src="images/icon_settings.png" width="20px" height="20px" /></td>
        </tr>
        <tr class="ui-widget-content">
            <td colspan="3" class="menu">
                <%--<ul class="menu">
                    <li>1
                        <ul>
                            <li>1-1
                                <ul>
                                    <li>1-1</li>
                                    <li>1-2</li>
                                    <li>1-3</li>
                                    <li>1-4</li>
                                    <li>1-5</li>
                                </ul>
                            </li>
                            <li>1-2
                                <ul>
                                    <li>1-1</li>
                                    <li>1-2</li>
                                    <li>1-3</li>
                                    <li>1-4</li>
                                    <li>1-5</li>
                                </ul>
                            </li>
                            <li>1-3
                                <ul>
                                    <li>1-1</li>
                                    <li>1-2</li>
                                    <li>1-3</li>
                                    <li>1-4</li>
                                    <li>1-5</li>
                                </ul>
                            </li>
                            <li>1-4
                                <ul>
                                    <li>1-1</li>
                                    <li>1-2</li>
                                    <li>1-3</li>
                                    <li>1-4</li>
                                    <li>1-5</li>
                                </ul>
                            </li>
                            <li>1-5
                                <ul>
                                    <li>1-1</li>
                                    <li>1-2</li>
                                    <li>1-3</li>
                                    <li>1-4</li>
                                    <li>1-5</li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                    <li>2
                        <ul>
                            <li>2-1</li>
                            <li>2-2</li>
                            <li>2-3</li>
                            <li>2-4</li>
                            <li>2-5</li>
                        </ul>
                    </li>
                    <li>3
                        <ul>
                            <li>3-1</li>
                            <li>3-2</li>
                            <li>3-3</li>
                            <li>3-4</li>
                            <li>3-5</li>
                        </ul>
                    </li>
                    <li>4
                        <ul>
                            <li>4-1</li>
                            <li>4-2</li>
                            <li>4-3</li>
                            <li>4-4</li>
                            <li>4-5</li>
                        </ul>
                    </li>
                    <li>5
                        <ul>
                            <li>5-1</li>
                            <li>5-2</li>
                            <li>5-3</li>
                            <li>5-4</li>
                            <li>5-5</li>
                        </ul>
                    </li>
                </ul>--%>
            </td>
        </tr>
    </table>
<%--    <div id="logo" class="ui-state-default" style="border-right:0; display:none;">系統管理</div>
    <div id="menu2" class="ui-state-default" style="display:none;">
        <div class="menu1"></div>
        <div class="menu2 ui-widget-content"></div>
        <div class="settings"><%= Context.User.Identity.Name %> <img alt="" src="images/icon_settings.png" width="20px" height="20px" /></div>
    </div>
    <div id="menu" class="ui-state-default" style="display:none;">
        <ul>
            <li class="fill"></li>
            <li class="template"><a></a>
                <table>
                    <tr>
                        <td class="fill"></td>
                        <td><ul></ul></td>
                    </tr>
                </table>
            </li>
            <li class="userinfo"><%= Context.User.Identity.Name %></li>
            <li class="settings"><img alt="" src="images/icon_settings.png" width="20px" height="20px" />
                <ul>
                    <li></li>
                    <li><a><%=res.change_password %></a></li>
                    <li><a><%=res.login_hist %></a></li>
<%--                    <li><a>themes</a>
                        <ul>
                            <li><a>base</a></li>
                            <li><a>black-tie</a></li>
                            <li><a>blitzer</a></li>
                            <li><a>cupertino</a></li>
                            <li><a>dark-hive</a></li>
                            <li><a>dot-luv</a></li>
                            <li><a>eggplant</a></li>
                            <li><a>excite-bike</a></li>
                            <li><a>flick</a></li>
                            <li><a>hot-sneaks</a></li>
                            <li><a>humanity</a></li>
                            <li><a>le-frog</a></li>
                            <li><a>mint-choc</a></li>
                            <li><a>overcast</a></li>
                            <li><a>pepper-grinder</a></li>
                            <li><a>redmond</a></li>
                            <li><a>smoothness</a></li>
                            <li><a>south-street</a></li>
                            <li><a>start</a></li>
                            <li><a>sunny</a></li>
                            <li><a>swanky-purse</a></li>
                            <li><a>trontastic</a></li>
                            <li><a>ui-darkness</a></li>
                            <li><a>ui-lightness</a></li>
                            <li><a>vader</a></li>
                        </ul>
                    </li>
                    <li onclick="logout()"><a><%=res.logout_text %></a></li>
                    <li></li>
                </ul>
            </li>
        </ul>
    </div>
    
    <div id="dbg" style="position:fixed; bottom:0; right:0"></div>--%>
</asp:Content>
