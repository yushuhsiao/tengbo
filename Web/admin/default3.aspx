<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="default_aspx" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="web" %>
<%@ Import Namespace="Resources" %>

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

        var _menu = new function () {
            function create_menu(item, $ul) {
                for (var i = 0; i < item.items.length; i++) {
                    var child = item.items[i];
                    var $li = $('<li></li>').appendTo($ul);
                    var $a = $('<a id="item_{0}" target="mainframe">{1}</a>'.format(child.id, child.label)).appendTo($li);
                    $a.attr('href', child.href);
                    if (child.items)
                        create_menu(child, $('<ul></ul>').appendTo($li))
                }
            }
            var nav_node_count = 0;
            function nav_setactive(nav_index) {
                if (nav_index) {
                    $('div.menu').hide();
                    $('div.menu[nav_index={0}]'.format(nav_index)).show();
                }
            }
            function nav_node(item) {
                var $button, $label, $menu_div;
                if (item == null) {
                    $button = $('#nav_0').button({ icons: { primary: "ui-icon-gear" }, text: false });
                    $label = $('label[for=nav_0]');
                    $menu_div = $('div.menu[nav_index=0]');
                }
                else {
                    nav_node_count++;
                    $button = $('<input id="nav_{0}" type="radio" name="nav" nav_index={0} />'.format(nav_node_count)).appendTo($('#nav-container'))
                    $label = $('<label for="nav_{0}" class="menu-nav">{1}</label>'.format(nav_node_count, item.label)).appendTo($('#nav-container'))
                    $button.button();
                    $menu_div = $('<div class="menu" nav_index="{0}"></div>'.format(nav_node_count)).appendTo($('#nav_menu'));
                    var $ul = $('<ul class="ui-menu"></ul>').appendTo($menu_div);
                    if (item.items) create_menu(item, $ul);
                }
                $label.removeClass('ui-corner-all').addClass('ui-corner-top');
                $('li', $menu_div).addClass('ui-menu-item').addClass('ui-widget-content')
                    .hover(function () { $(this).addClass('ui-state-hover'); }, function () { $(this).removeClass('ui-state-hover'); })
                    .click(function () { $('.menu li').removeClass('ui-state-active'); $(this).addClass('ui-state-active'); });
                $menu_div.hide();
                //$menu_div.css({ width: '10000' });
                $button.change(function () { nav_setactive($(this).attr('nav_index')); });
            }
            return {
                init: function (data) {
                    if (nav_node_count == 0)
                        new nav_node(null);
                    for (var i = 0; i < data.length; i++) {
                        new nav_node(data[i]);
                    }
                },
                setactive: nav_setactive
            }
        }

        var recvMessage = {
            rebuild_menu: _menu.init
        }

        $(document).ready(function () {
            //var $tabs = $('.nav-tabs');
            //var $ul = $tabs.find(' > ul')
            //$tabs.tabs({active:2}).removeClass('ui-corner-all');
            //$ul.removeClass('ui-corner-all');
            //$ul.find('.menu').appendTo($tabs);
            
            $('.theme_select').click(function () {
                $('#css_jquery_ui_theme_d').attr('href', 'css/themes/{0}/jquery-ui.css'.format($(this).text()));
                if ($.isFunction(main.contentWindow.theme_change))
                    main.contentWindow.theme_change();
                //$(main.contentDocument).find('#css_jquery_ui_theme').attr('href', '../' + href);
            });

            

            //setTimeout(function () { $('#main').prop('src', $('a[target="mainframe"][href="admin/UserList.aspx"]').trigger('click').attr('href')); }, 500);


            _menu.init(<%= api.SerializeObject(menu.items) %>);
            <% MenuRow row = MenuRow.Cache.Instance.GetItem("user_list"); if (row != null) { %>
            setTimeout(function () {
                var menu_item = $('#item_<%=row.ID%>');
                var nav_index = menu_item.closest('div.menu').attr('nav_index');
                if (nav_index) {
                    $('input[nav_index=' + nav_index + ']').trigger('click');
                    setTimeout(function () {
                        menu_item.trigger('click');
                        $('#main').prop('src', menu_item.attr('href'));
                    }, 200);
                }
            }, 500);
            <% } %>


            //$('#btnOptions').click();

            $(window).resize(function () {
                $(div1).height(window.innerHeight - $(div3).outerHeight()/* - $(div2).outerHeight()*/);
            }).trigger('resize');
        });
    </script>
    <style type="text/css">
        .ui-widget { font-size: 1em; }
        html, body { margin: 0; padding: 0; overflow: hidden; width: 100%; height: 100%; } 
        ul, li { list-style: none; margin: 0; padding: 0; white-space: nowrap; }
        .ui-menu { padding: 0; }
        .ui-menu .ui-menu-item { width: auto; padding-left: 1em; padding-right: 1em; }
        td { white-space: nowrap; padding: 0; }
        #nav-container .menu-nav { font-size: 14px; border-bottom-width: 0; margin: 0 2px 0 2px; padding: 0 1px 0 1px; }
        #nav-container .menu-nav .ui-button-text { padding: .3em .8em .4em .8em; }
        .nav-right .menu-nav { font-size: 12px; border-bottom-width: 0; margin: 0 15px 0 15px; padding: 0 10px 0 10px; }
        .menu > ul li { position: relative; white-space: nowrap; }
        .menu > ul ul { position: absolute; visibility: hidden; top: 100%; left: -1px; }
        .menu > ul > li { float: left; }
        .menu > ul li:hover > ul { visibility: visible; }
        .menu > ul ul ul { top: 0; left: 100%; }
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
        #msg_sw .ui-button-text { padding: .1em .3em .1em .3em; }
        #msg_box { position: absolute; font-size: .6em; left: 6em; bottom: 0; padding: .3em; border-collapse: collapse; border-spacing: 1px; }
        #msg_box td { padding: .1em .3em .1em .3em }
    </style>
</head>
<body>
    <div id="div1" style="position: absolute; left: 0; top: 55px; width: 100%; height: 100px">
        <iframe id="main" name="mainframe" frameborder="0" style="width: 100%; height: 100%"></iframe>
    </div>
    <button id="msg_sw" style="position: absolute; font-size: .6em; left:0; bottom: 0; ">Message</button>
    <table id="msg_box" border="1"><tr><td class="_msg"></td><td class="_args"></td><td class="_code"></td></tr></table>
    <script type="text/javascript">
        $(document).ready(function () {
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
                $('._msg, ._args, ._code', _box).text('');
                if (data.msg) $('._msg', _box).text(data.msg);
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
            $('#msg_sw').button(/*{ icons: { primary: 'ui-icon-clipboard' } }*/).removeClass('ui-state-default').addClass('ui-widget-content').css({ 'bottom': _box.outerHeight() }).click(onSwitch);
            onSwitch();

            function _box_pos() {
                _box.position({
                    my: "left top",
                    at: "right top",
                    of: "#msg_sw",
                    collision: "flip flip",
                })
            }
            _box.click(onTimeout);
            _box_pos();
            $(window).resize(_box_pos);
            onMessage({ msg: 'Ready.', state: 'normal' });
            recvMessage.dbgmsg = onMessage;
        });
    </script>
    <div id="div2" style="position: absolute; left: 0; bottom: 2em; width: 100%; height: 30%; border: 2px;">
        <iframe frameborder="1" src="admin/MessagePanel.aspx" style="width: 100%; height: 100%; border: 0;"></iframe>
    </div>
    <div id="div3" style="position: absolute; left: 0; top: 0; width: 100%; height: 55px;">
        <table class="ui-widget-content" style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0; border: 0;">
            <tr class="ui-widget-header">
                <td class="ui-state-default" style="text-align: center; font-size: 1.5em; width: 200px; border: 0;" rowspan="2">系統管理</td>
                <td id="nav-container" style="vertical-align: bottom; width: 800px;">
                </td>
                <td class="nav-right" style="width: 1%; text-align: right; vertical-align: bottom; ">
                    <%=User.Name%>
                    <input id="nav_0" type="radio" name="nav" nav_index="0" />
                    <label for="nav_0" class="menu-nav">Options</label>
                </td>
            </tr>
            <tr style="height: 24px;">
                <td id="nav_menu" class="ui-widget-content" style="height: 24px; border-bottom-width: 0;" colspan="2">
                    <div class="menu" nav_index="0" style="float: right;">
                        <ul class="ui-menu">
                            <li><a href="sys/AccountInfo.aspx" target="mainframe"><%=res.AccountInfo%></a></li>
                            <li><a style="cursor: pointer;">theme</a>
                                <ul><% foreach (string s in default_aspx.themes) { %>
                                    <li><a class="theme_select"><%=s%></a></li>
                                    <% } %>
                                </ul>
                            </li>
                            <li onclick="logout()" style="cursor: pointer;"><a><%=res.logout_text%></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        // long pooling, comet
        $(document).ready(function () {
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
                //start();
            };
        });
   </script>
</body>
</html>
