<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="default_aspx" %>

<%@ Import Namespace="Resources" %>
<%@ Register Src="~/default_menu.ascx" TagPrefix="uc1" TagName="default_menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jquery-ui.js"></script>
    <link href="css/themes/start/jquery-ui.css" rel="stylesheet" />
    <link <%= "id=\"css_jquery_ui_theme_d\""%> href="css/themes/<%=res.ui_theme%>/jquery-ui.css" rel="stylesheet" />

    <script type="text/javascript" src="js/util.js"></script>
    <script type="text/javascript">

        function logout() { $.invoke_api({ AdminLogout: {} }, { success: function (data) { if (data.Status == 1) window.location.reload(); else if (data.LoginResult.t1 == 1) window.location.reload(); } }); }

        var _msgbox;

        var recvMessage = {
            dbgmsg: function (data) { _msgbox.message(data); }
        }

        $(document).ready(function () {
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
                $('#css_jquery_ui_theme_d').attr('href', 'css/themes/{0}/jquery-ui.css'.format($(this).text()));
                if ($.isFunction(main.contentWindow.theme_change))
                    main.contentWindow.theme_change();
                //$(main.contentDocument).find('#css_jquery_ui_theme').attr('href', '../' + href);
            });

            _msgbox = new function () {
                var _box = $('#msg_box');
                var _state = 'highlight';
                var _h;
                function onTimeout() {
                    _box.fadeOut(500);
                    clearTimeout(_h);
                }
                function onMessage(data) {
                    clearTimeout(_h);
                    _box.hide();
                    _box.removeClass('ui-state-' + _state);
                    _state = data.state;
                    if (_state == 'normal') _state = 'highlight';
                    _box.addClass('ui-state-' + _state);
                    if (data.msg)  $('._msg', _box).text(data.msg);
                    if (data.args) $('._args', _box).text(data.args);
                    if (data.code) $('._code', _box).text(data.code);
                    _box.fadeIn(100);
                    _h = setTimeout(onTimeout, 5000);
                }

                var div2_visible = true;
                function onSwitch() {
                    div2_visible = !div2_visible;
                    if (div2_visible)
                        $('#div2').show();
                    else
                        $('#div2').hide();
                }
                $('#msg_sw').button(/*{ icons: { primary: 'ui-icon-clipboard' } }*/).removeClass('ui-state-default').addClass('ui-widget-content').css({'bottom': _box.outerHeight()}).click(onSwitch);
                onSwitch();

                _box.position({
                    my: "left top",
                    at: "right top",
                    of: "#msg_sw",
                    collision: "flip flip",
                }).click(onTimeout);
                onMessage({ msg: 'Ready.', state : 'normal' });
                return { message: onMessage };
            }

            setTimeout(function () { $('#main').prop('src', $('a[target="mainframe"][href="admin/UserList.aspx"]').trigger('click').attr('href')); }, 500);

            $(window).resize(function () {
                $(div1).height(window.innerHeight - $(div3).outerHeight()/* - $(div2).outerHeight()*/);
            }).trigger('resize');

            // long pooling, comet
            var comet = new function () {
                var _super = this;
                var _watchdog;

                this.begin = function () {
                    if ($('#comet_01').hasClass('busy')) return;
                    $('#comet_01').addClass('busy');
                    //console.log('comet begin...');
                    $.invoke_api({ LoginID: '<%=User.LoginID%>' }, {
                        url: '<%=web.comet.GetUrl(this)%>',
                        crossDomain: true,
                        //xhrFields: { 'withCredentials': true },
                        success: function (data, textStatus, jqXHR) {
                            //console.log('comet success', arguments);
                            if (_super != null)
                                setTimeout(_super.begin, 0<%=web.comet.Sleep%>);
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            //console.log('comet error', arguments);
                        },
                        complete: function (jqXHR, textStatus) {
                            $('#comet_01').removeClass('busy');
                            //_super.begin();
                        }
                    });
                }

                function start() {
                    <% if (web.comet.WatchDog > 0)
                       { %>setInterval(_super.begin, 0<%=web.comet.WatchDog%>);<% }%>
                    _watchdog = setTimeout(_super.begin, 0<%=web.comet.Sleep%>);
                }
                start();
            };
            //$('#div4').draggable();
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
        /*.menu li { background-color: #eeeeee; border: 1px solid #555555; padding: 3px 1em 3px 1em; margin-top: 1px; margin-left: 2px; }*/
        /*.menu li:hover { background-color:#aaaaaa; }*/
        /*.menu a { text-decoration: none; color:#cccccc; background-color: #eeeeee; border: 1px solid #555555; margin: 3px 1em 3px 1em; }*/
        /*.menu a:link, .menu a:visited { color:#000000; background-color:#ffffff; }*/
        /*.menu a:active .menu a:hover { background-color:#aaaaaa; }*/
        .comet-icon span { float: right; margin-top: 10px; }
        .comet-icon .ui-icon-radio-off { display: block; }
        .comet-icon .ui-icon-bullet { display: none; }
        .comet-icon.busy .ui-icon-radio-off { display: none; }
        .comet-icon.busy .ui-icon-bullet { display: block; }
        .theme_select { cursor: pointer; }
        .msg { position: absolute; font-size: .6em; }
        #msg_sw .ui-button-text { padding: .1em .3em .1em .3em; }
        #msg_box { position: absolute; font-size: .6em; left: 6em; bottom: 0; padding: .3em; border-collapse: collapse; border-spacing: 1px; }
        #msg_box td { padding: .1em .3em .1em .3em }
    </style>
</head>
<body>
    <%
        System.Collections.Generic.List<web.MenuRow> nav = new System.Collections.Generic.List<web.MenuRow>();
        foreach (web.MenuRow item in web.MenuRow.Cache.Instance.GetItem(web.MenuRow.menu_root).Childs)
            if (User.Permissions[item.Code, BU.Permissions.Flag.Read])
                nav.Add(item);
        this.menu1.DataSource = this.menu2.DataSource = nav;
        this.menu1.DataBind();
        this.menu2.DataBind();
 %>
    <div id="div1" style="position: absolute; left: 0; top: 55px; width: 100%; height: 100px">
        <iframe id="main" name="mainframe" frameborder="0" style="width: 100%; height: 100%"></iframe>
    </div>
    <button id="msg_sw" style="position: absolute; font-size: .6em; left:0; bottom: 0; ">Message</button>
    <table id="msg_box" border="1"><tr><td class="_msg"></td><td class="_args"></td><td class="_code"></td></tr></table>
    <%--<div id="div2" style="position: absolute; left: 0; bottom: 0; width: auto; height: 1em; border: solid 1px red; "></div>--%>
    <div id="div2" style="position: absolute; left: 0; bottom: 2em; width: 100%; height: 30%; border: 2px;">
        <iframe frameborder="1" src="admin/MessagePanel.aspx" style="width: 100%; height: 100%; border: 0;"></iframe>
    </div>
    <div id="div3" style="position: absolute; left: 0; top: 0; width: 100%; height: 55px;">
        <table id="header" style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
            <tr>
                <td class="ui-state-default" style="text-align: center; font-size: 1.5em; width: 200px; border: 0;">系統管理</td>
                <td class="nav-tabs" style="vertical-align: top; padding: 0; border: 0;">
                    <ul>
                        <li style="float: right;"><a href="#nav_right"><span class="ui-icon ui-icon-gear" style="float: left;"></span>&nbsp;</a></li>
                        <li style="float: right;"><a><%=User.Name%></a></li>
                        <asp:Repeater ID="menu1" runat="server">
                            <ItemTemplate>
                                <li><a href="#nav_<%# Container.ItemIndex%>"><%# GetMenuText(Page.GetDataItem())%></a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <li id="comet_01" style="float: right;" class="comet-icon"><span class="ui-icon ui-icon-radio-off"></span><span class="ui-icon ui-icon-bullet"></span></li>
                        <%--<li style="float: right;"><a class="ui-icon-arrowstop-1-e"></a></li>--%>
                    </ul>
                    <div class="menu" id="nav_right" style="float: right; padding-bottom: 0;">
                        <ul>
                            <li><a href="sys/AccountInfo.aspx" target="mainframe"><%=res.AccountInfo%></a></li>
                            <li><a style="cursor: pointer;">theme</a>
                                <ul><% foreach (string s in default_aspx.themes)
                                       { %>
                                    <li><a class="theme_select"><%=s%></a></li><% } %>
                                </ul>
                            </li>
                            <li onclick="logout()" style="cursor: pointer;"><a><%=res.logout_text%></a></li>
                        </ul>
                    </div>
                    <asp:Repeater ID="menu2" runat="server">
                        <ItemTemplate>
                            <div class="menu" id="nav_<%# Container.ItemIndex%>">
                                <uc1:default_menu runat="server" ID="default_menu" DataItem="<%# Page.GetDataItem()%>" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </div>
    <%--<div id="div4" style="position: absolute; right: 0; top: 0; width: 100px; height: 100px;">
        <iframe style="width:100%; height: 100%;"></iframe>
    </div>--%>
</body>
</html>
