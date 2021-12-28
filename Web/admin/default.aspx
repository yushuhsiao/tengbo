<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="default_aspx" %>
<%@ Import Namespace="web" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <base target="mainframe" />
    <link rel="stylesheet" href="js/jqwidgets/styles/jqx.base.css" />
    <link rel="stylesheet" href="" id="css_theme" path="<%=ResolveClientUrl("~/js/jqwidgets/styles/")%>" />
    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript" src="js/jqwidgets/jqx-all.js"></script>
    <script type="text/javascript" src="js/jqwidgets/globalization/globalize.js"></script>
    <script type="text/javascript" src="js/admin2.js"></script>
    <script type="text/javascript" src="js/util.js"></script>
    <script type="text/javascript" src="js/jqx.js"></script>
    <style type="text/css">
        html, body  { margin: 0; padding: 0; overflow: hidden; width: 100%; height: 100%; }
        table       { border-collapse: collapse; border-spacing: 0; }
        td          { white-space: nowrap; }

        #main       { position: absolute; width: 100%; top: 0; }
        #mainframe  { width: 100%; height: 100%; border: 0; }
        #header     { position: absolute; width: 100%; }
        #footer     { position: absolute; width: 100%; bottom: 0; }
        #nav1       { position: relative; }
        #nav2 .jqx-tabs-content-element,
        #nav2 .jqx-menu
        { border: 0; border-radius: 0;  }
        #tabs       { border: 0; border-radius: 0; }
        #tabs .jqx-tabs-header { margin: 0; border: 0; border-radius: 0; padding-bottom: 2px; }
        #opts       { position: absolute; top: 0; right: 0; overflow: hidden; white-space: nowrap; }
        #opts td    { padding-left: 1.1em; padding-right: 1.1em; }
        #opts1, #opts2, #opts3 { border-radius: 0; border: 0; }
        #opts1      { width: 2em; }
        #opts2      { }
        #opts3      { }
        #opts2_menu { position: absolute; top: 50px; right: 1em; }
        #opts3_menu { position: absolute; top: 50px; right: 0; text-align: center; }
        #logo       { position: absolute; top: 0; left: 0; width: 200px; height: 100%; }
    </style>
    <script type="text/javascript">
        $.Message.SetMenu = function (items) {
            var $tabs = $('#tabs')

            function process_data(items) {
                for (var i = 0; i < items.length; i++) {
                    items[i].value = items[i].href;
                    items[i].target = 'mainframe';
                    items[i].id = 'menu_' + items[i].id;
                    //items[i].html = '<a href="{1}">{0}</a>'.format(items[i].label, items[i].href);
                    //delete items[i].label;
                    if ($.isArray(items[i].items))
                        process_data(items[i].items);
                }
            }

            function build_menu(item, node) {
                if ($.isArray(item.items)) {
                    var ul = $('<ul></ul>').appendTo(node);
                    for (var i = 0; i < item.items.length; i++) {
                        var item2 = item.items[i];
                        var li;
                        if (item.items[i].href)
                            li = $('<li><a href="' + item.items[i].href + '" target="mainframe">' + item.items[i].label + '</a></li>');
                        else
                            li = $('<li>' + item.items[i].label + '</li>')
                        li.appendTo(ul);
                        build_menu(item.items[i], li);
                    }
                }
            }

            function add_tab(item) {
                //process_data(item.items);
                $tabs.jqxTabs('addLast', item.label, '<div menu_index="' + item.id + '"></div>');
                var node = $('div[menu_index=' + item.id + ']');
                build_menu(item, node);
                node.jqxMenu2({ autoCloseOnClick: false, showTopLevelArrows: true });
                //$('div[menu_index=' + item.id + ']')
                //    .jqxMenu2({ source: item.items, autoOpen: true, autoCloseOnClick: false, showTopLevelArrows: true })
                //    .on('itemclick', function (event) {
                //        //'jqx-fill-state-pressed jqx-fill-state-pressed-' + $.jqx.theme;
                //        var href = $(event.args).attr('item-value');
                //        $('#mainframe').prop('src', $(event.args).attr('item-value'));
                //    });
            }

            if ($.isArray(items) && items.length > 0) {
                while ($tabs.jqxTabs('length') > 0)
                    $tabs.jqxTabs('removeAt', 0);

                for (var i = 0; i < items.length; i++)
                    add_tab(items[i]);
            }
        };

        $.fn.css2 = function (p, v, cb) {
            this.v1[p] = v;
            if (this.v1[p] == this.v2[p]) return;
            this.v2[p] = v;
            this.css(p, v);
            if ($.isFunction(cb))
                cb(v);
        }
        $.fn.css3 = function (p, v, cb) {
            this.v1[p] = v;
            if (this.v1[p] == this.v2[p]) return;
            this.v2[p] = v;
            if ($.isFunction(cb))
                cb(v);
        }
    </script>
</head>
<body>
    <div id="main"><iframe id="mainframe" name="mainframe"></iframe></div>
    <div id="header">
        <div id="nav0" style="position: relative; float: right;">
            <div id="nav1">
                <table id="opts">
                    <tr>
                        <td id="opts1"></td>
                        <td id="opts2"><%= User.Name %></td>
                        <td id="opts3">*</td>
                    </tr>
                </table>
                <div id="tabs"><ul><li></li></ul><div></div></div>
            </div>
            <div id="nav2" style="clear: both;"></div>
        </div>
        <div id="logo">
            <table style="width: 100%; height:100%;">
                <tr>
                    <td style="text-align: center; vertical-align: central; height: 100%;"></td>
                </tr>
            </table>
        </div>
        <div id="opts2_menu" style="display: none;">
            <div>
                <ul>
                    <li op="accinfo"><a href="sys/AccountInfo.aspx" target="mainframe"><%=lang["AccountInfo"]%></a></li>
                    <li type='separator'></li>
                    <li op="logout"><%=lang["logout_text"]%></li>
                </ul>
            </div>
        </div>
        <div id="opts3_menu" style="display: none;">
            <div id="theme_list"></div>
        </div>
    </div>
    <div id="footer"><iframe src="admin/MessagePanel.aspx" style="width:100%; height: 20px; border: 0;"></iframe></div>
    <script type="text/javascript">
        var resize;

        $(document).ready(function () {
            function $$(selector) { var jq = $(selector); jq.v1 = {}; jq.v2 = {}; return jq; }
            var $window = $$(window),
                $logo = $$('#logo'), $header = $$('#header'), $main = $$('#main'), $footer = $$('#footer'),
                $menu = $$('#menu'), $opts = $$('#opts'), $tabs = $$('#tabs'),
                $nav0 = $$('#nav0'), $nav1 = $$('#nav1'), $nav2 = $$('#nav2');


            resize = function () {
                $tabs.css3('width', Math.max(100, $nav1.innerWidth() - $('#opts').outerWidth()), function (value) {
                    $tabs.jqxTabs({ width: value + 10 });
                });
                $opts.css2('height', $nav1.innerHeight(), function (value) {
                    //$('#opts1, #opts2, #opts3').jqxButton({ height: value });
                });

                $nav0.css('width', $header.innerWidth() - $logo.outerWidth());

                $nav0.w0 = $header.innerWidth() - $logo.outerWidth();
                var h0 = $window.innerHeight(),
                    h1 = $header.outerHeight(),
                    h2 = $footer.outerHeight();
                $main.css2('top', h1);
                $main.css2('height', h0 - h1 - h2);
            };

            $.Message.themes.change = function () {
                var t = { theme: $.jqx.theme };
                $('#theme_list').jqxListBox(t);
                $('#opts1, #opts2, #opts3').jqxButton(t);
                //$('#opts2_menu').jqxMenu(t);
                $('#opts2_menu > div').jqxMenu2(t);
                $tabs.jqxTabs(t);
                $('#nav2 .jqx-tabs-content .jqx-menu').jqxMenu2(t);
            };

            $('#opts1, #opts2, #opts3').jqxButton();

            $('#opts2_menu > div').jqxMenu2({ mode: 'vertical' }).on('itemclick', function (event) {
                switch ($(event.args).attr('op')) {
                    case 'logout':
                        $.invoke_api({ AdminLogout: {} }, {
                            success: function (data) {
                                if (data.Status == 1) window.location.reload(); else if (data.LoginResult.t1 == 1) window.location.reload();
                            }
                        });
                        break;
                    case 'accinfo':
                        break;
                }
            })

            function optmenus(selector1, selector2, hide_delay) {
                var $dst = $(selector2);
                var v = false;
                var t = null;
                function t_clear() {
                    if (t == null) return;
                    clearTimeout(t);
                    t = null;
                }
                function t_set() {
                    t_clear();
                    t = setTimeout(function () { setVisible(false); }, hide_delay);
                }
                function setVisible(visible) {
                    var vv = v;
                    if ((visible == true) || (visible == false)) v = visible;
                    else v = !v;
                    t_clear();
                    if (v == true) {
                        $dst.css({ top: $opts.outerHeight() });
                        $dst.show();
                    }
                    else {
                        $dst.hide();
                    }
                }

                $(selector1).on('click', setVisible);
                $(selector1 + ',' + selector2).hover(t_clear, t_set);

                return {
                    setVisible: setVisible,
                    show: function () { setVisible(true); },
                    hide: function () { setVisible(false); },
                    delayhide: function (n) {
                        t_clear();
                        t = setTimeout(function () { setVisible(false); }, n);
                    },
                    clearhide: t_clear,
                }
            }

            var _menu2 = new optmenus('#opts2', '#opts2_menu', 500);
            var _menu3 = new optmenus('#opts3', '#opts3_menu', 500);

            $('#tabs').jqxTabs({ selectionTracker: true, position: 'top', width: window.innerWidth, scrollPosition: 'both', });
            $('#tabs .jqx-tabs-content').appendTo($('#nav2'));
            $('#theme_list')
                .jqxListBox({ source: [<%=api.SerializeObject(themes_jqx) %>][0], width: 250, autoHeight: false })
                .on('select', function (event) { $.Message.themes.notify(event.args.item.value); })
                .jqxListBox('selectItem', $.jqx.theme);

            $.Message.SetMenu(<%= api.SerializeObject(menu.items)%>);
            setInterval(resize, 300);
            resize();

            var tabs_init = setInterval(function () { if ($tabs.jqxTabs('selectedItem') == 1) clearInterval(tabs_init); else $tabs.jqxTabs('select', 1); }, 100);
        });
    </script>
</body>
</html>
