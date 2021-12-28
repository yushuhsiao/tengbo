<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="default_aspx" %>
<%@ Import Namespace="web" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
        var logo_width = 200;
        $.Message.themes.init('<%=ResolveClientUrl("~/js/jqwidgets/styles/")%>', 'ui-start');

        $.Message.SetMenu = function (items) {
            var $tabs = $('#tabs')

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
                $tabs.jqxTabs('addLast', item.label, '<div menu_index="' + item.id + '"></div>');
                $('div[menu_index=' + item.id + ']')
                    .jqxMenu2({ source: item.items, autoOpen: true, autoCloseOnClick: false, showTopLevelArrows: true })
                    .on('itemclick', function (event) {
                        //'jqx-fill-state-pressed jqx-fill-state-pressed-' + $.jqx.theme;
                        var href = $(event.args).attr('item-value');
                        $('#mainframe').prop('src', $(event.args).attr('item-value'));
                    });
            }

            if ($.isArray(items) && items.length > 0) {
                while ($tabs.jqxTabs('length') > 0)
                    $tabs.jqxTabs('removeAt', 0);

                for (var i = 0; i < items.length; i++)
                    add_tab(items[i]);
            }
        };

        $.fn.jqxMenu2 = function (o) {
            this.jqxMenu(o);
            this.removeClass_jqx('jqx-widget-header');//.addClass_jqx('jqx-widget-content');
            return this;
        }
    </script>
    <style type="text/css">
        html, body  { margin: 0; padding: 0; overflow: hidden; width: 100%; height: 100%; }
        table       { border-collapse: collapse; border-spacing: 0; }
        td          { white-space: normal; }

        #header     { position: absolute; left: 0; top: 0; width: 100%; }
        #logo       { position: absolute; top: 0; left: 0; width: 200px; height: 100%; }
        #logo td    { text-align: center; vertical-align: central; }
        #nav0       { position: relative; top: 0; right: 0; float: right; }
        #opts       { position: absolute; top: 0; right: 0; }
        #opts td    { border-radius: 0; }
        #opt0       { width: 50px; }
        #opt1       { padding-left: 1.1em; padding-right: 1.1em; }
        #opt2       { padding-left: 1.1em; padding-right: 1.1em; width: 1em; }

        #tabs .jqx-tabs-header { margin: 0; }
        #tabs,
        #tabs .jqx-tabs-header,
        #nav0 .jqx-tabs-content,
        #nav0 .jqx-tabs-content-element,
        #nav0 .jqx-tabs-content .jqx-menu { border-radius: 0; }
        #nav0 .jqx-tabs-content .jqx-menu,
        #nav0 .jqx-tabs-content .jqx-menu ul { padding-top: 0; padding-bottom: 0; }

        #main       { position: absolute; width: 100%; left: 0; bottom: 0; height: 50%; }
        #mainframe  { width:100%; height: 100%; border: 0; }
    </style>
</head>
<body>

    <div id="main">
        <iframe id="mainframe" name="mainframe"></iframe>
        <div id="theme_list" style="position: absolute; top: 0; left: 0;"></div>
    </div>
    <div id="header">
        <table id="logo"><tr><td>logo</td></tr></table>
        <div id="nav0">
            <table id="opts"><tr><td id="opt0"></td><td id="opt1"><%= User.Name %></td><td id="opt2"></td></tr></table>
            <div id="tabs">
                <ul><li></li></ul><div></div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function resize() {
            var t = { theme: $.jqx.theme };
            var nav0_w = $('#header').innerWidth() - $('#logo').outerWidth();
            $('#nav0').css({ width: nav0_w });
            $('#opt0, #opt1, #opt2').jqxButton(t);
            $('#tabs').jqxTabs({ theme: $.jqx.theme, width: Math.max(20, nav0_w - $('#opt1').outerWidth() - $('#opt2').outerWidth() - 20) });
            $('#opts').css({ height: $('#tabs').outerHeight() });
            $('#nav0 .jqx-tabs-content .jqx-menu').jqxMenu2(t);
            $('#logo').css({ height: $('#nav0').outerHeight() });
            $('#main').css({ height: window.innerHeight - $('#header').outerHeight() });
        }

        $.Message.themes.change = function () {
            console.log("theme change");
            resize();
        };

        function resize2() {
            var w1 = $('#options').outerWidth();
            setInterval(function () {
                var w2 = $('#opts').outerWidth();
                if (w1 == w2) return;
                w1 = w2;
                resize();
            });
        }

        $(document).ready(function () {

            $('#tabs').jqxTabs({ selectionTracker: true, position: 'top', scrollPosition: 'both', });
            $('#tabs .jqx-tabs-content').appendTo($('#nav0'));
            $('#opt0, #opt1, #opt2').jqxButton();

            $('#theme_list')
                .jqxListBox({ source: [<%=api.SerializeObject(themes_jqx) %>][0], width: 250, autoHeight: false })
                .on('select', function (event) { $.Message.themes.set(event.args.item.value); })
                .jqxListBox('selectItem', $.jqx.theme);

            $.Message.SetMenu(<%= api.SerializeObject(menu.items)%>);
            $(window).resize(resize).trigger('resize');
            setInterval(function () {
                console.log($('#opts').outerWidth());
            }, 100);
        });
    </script>
</body>
</html>
