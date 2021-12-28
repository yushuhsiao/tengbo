<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="default_aspx" %>
<%@ Import Namespace="web" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <base target="mainframe" />
    <link rel="stylesheet" href="js/jqwidgets/styles/jqx.base.css" />
    <link rel="stylesheet" href="" id="css_theme"/>
    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jqwidgets/jqx-all.js"></script>
    <script src="js/jqwidgets/jqxmenu.js"></script>
    <script type="text/javascript" src="js/jqwidgets/globalization/globalize.js"></script>
    <script type="text/javascript" src="js/admin2.js"></script>
    <script type="text/javascript" src="js/util.js"></script>
    <script type="text/javascript">

        $.Message.themes.init('<%=ResolveClientUrl("~/js/jqwidgets/styles/")%>', 'ui-start');

        function logout() { $.invoke_api({ AdminLogout: {} }, { success: function (data) { if (data.Status == 1) window.location.reload(); else if (data.LoginResult.t1 == 1) window.location.reload(); } }); }

        $(document).ready(function () {

            var _top = $(p_top), _main = $(p_main), _logo = $(p_logo), _opts = $(p_opts), _nav = $(p_nav), _menu;

            _opts
                .jqxMenu({ clickToOpen: true, autoOpen: false, autoCloseOnClick: false, showTopLevelArrows: true, rtl: true })
                .on('itemclick', function (event) {
                    if (event.args.id == 'btn_logout') logout();
                    else if (event.args.id == 'btn_accinfo') $('#main').prop('src', 'sys/AccountInfo.aspx');
                });
            $(theme_list)
                .jqxListBox({ source: [<%=api.SerializeObject(themes_jqx) %>][0], width: '100%' })
                .jqxListBox('selectItem', $.jqx.theme);
            $(theme_list).on('select', function (event) {
                    console.log(event.args.item);
                    $.Message.themes.set(event.args.item.value);
                });

            _nav.jqxTabs({ position: 'top', scrollPosition: 'both' });//.jqxTabs('removeLast');
            _menu = $('.jqx-tabs-content', _nav).appendTo(p_top);

            ($.Message.SetMenu = function (items) {

                function process_data(items) {
                    for (var i = 0; i < items.length; i++) {
                        items[i].value = items[i].href;
                        items[i].id = 'menu_' + items[i].id;
                        //items[i].html = '<a href="{1}">{0}</a>'.format(items[i].label, items[i].href);
                        //delete items[i].label;
                        if ($.isArray(items[i].items))
                            process_data(items[i].items);
                    }
                }

                function add_tab(item) {
                    process_data(item.items);
                    _nav.jqxTabs('addLast', item.label, '<div menu_index="' + item.id + '"></div>');
                    $('div[menu_index=' + item.id + ']', _menu)
                        .jqxMenu({ source: item.items, autoOpen: true, autoCloseOnClick: false, showTopLevelArrows: true })
                        .on('itemclick', function (event) {
                            //'jqx-fill-state-pressed jqx-fill-state-pressed-' + $.jqx.theme;
                            var href = $(event.args).attr('item-value');
                            $('#main').prop('src', $(event.args).attr('item-value'));
                        });
                }

                if ($.isArray(items) && items.length > 0) {
                    while (_nav.jqxTabs('length') > 0)
                        _nav.jqxTabs('removeAt', 0);

                    for (var i = 0; i < items.length; i++)
                        add_tab(items[i]);
                }
            })(<%= api.SerializeObject(menu.items)%>);

            $(window).resize(function () {
                _nav.jqxTabs({ theme:$.jqx.theme, width: _top.innerWidth() - _logo.outerWidth() - _opts.outerWidth() })
                    .css({ left: _logo.outerWidth() });
                _opts.css({ height: _nav.height() });
                _menu.css({ left: _logo.outerWidth(), top: _nav.outerHeight(), width: _top.innerWidth() - _logo.outerWidth() });
                _top.css({ height: _nav.outerHeight() + _menu.outerHeight() });
                _main.css({ height: window.innerHeight - _top.outerHeight() });

                var t = { theme: $.jqx.theme };
                $('.jqx-widget.jqx-menu[role=menubar]', _menu)
                    .jqxMenu(t).removeClass_jqx('jqx-widget-header').addClass_jqx('jqx-widget-content');
                _opts.jqxMenu(t);
                $(theme_list).jqxListBox(t);
            }).trigger('resize');

            $.Message.themes.change = function (theme) { $(window).trigger('resize'); }

            setTimeout(function () {
                _nav.jqxTabs('select', 0);
                _nav.jqxTabs('select', 1);
            }, 1);

        });
    </script>
    <style type="text/css">
        html, body  { margin: 0; padding: 0; overflow: hidden; width: 100%; height: 100%; }
        #p_top      { position: absolute; left: 0; top: 0; width: 100%; }
        #p_logo     { position: absolute; width: 200px; height: 100%; text-align: center; }
        #p_nav      { position: absolute; top: 0; border: 0; }
        #p_nav .jqx-tabs-header { border: 0; margin: 0; }
        #p_top .jqx-tabs-content { position: absolute; }
        #p_nav, #p_nav .jqx-tabs-header,
        #p_top .jqx-tabs-content,
        #p_top .jqx-tabs-content-element,
        #p_opts, #p_opts .jqx-tabs-header { border-radius: 0; }

        #p_top .jqx-menu { border-radius: 0; border-top: 0; }
        #p_top .jqx-menu > ul { padding-top: 0; padding-bottom: 0; }

        #p_opts     { position: absolute; right: 0; top: 0; width: auto; height: 30px; padding-left: 20px; padding-right: 20px; vertical-align: bottom; line-height: 26px; }
        #p_main     { position: absolute; left: 0; bottom: 0; width: 100%; }

        #main       { width: 100%; height: 100%; }
    </style>
</head>
<body>
    <div id="p_main"><iframe id="main" name="mainframe" frameborder="0"></iframe></div>
    <div id="p_top">
        <div id="p_logo" class="jqx-fill-state-"></div>
        <div id="p_nav"><ul><li>.</li></ul><div></div></div>
        <div id="p_opts">
            <ul>
                <li>
                    <%=User.Name%>
                    <ul>
                        <li id="btn_accinfo"><%=lang["AccountInfo"]%></li>
                        <li>Themes<div id="theme_list"></div></li>
                        <li type='separator'></li>
                        <li id="btn_logout"><%=lang["logout_text"]%></li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
</body>
</html>
