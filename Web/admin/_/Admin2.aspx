<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Resources" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

        $(document).ready(function () {

            var grid = init_grid(
                '#table',
                '#pager',
                '#templates', {
                serializeGridData: function (postData) {
                    return { Admin_Select: postData };
                },
                serializeRowData: function (postData) {
                    return { Admin_Update: postData };
                },
                update_success: function (rowid, data) {
                    return $.extend(true, data, {
                        Password: '',
                        AdminACNT: '',
                    });
                },
            });

            var colModel = [
                    { name: 'rowid', sorttype: "int", frozen: true, hidden: true, key: true },
                    { label: '<%=lang.d[this, "colID"]           %>', width: 050, frozen: true, name: 'ID', sorttype: "int", editable: true, hidden: true },
                    { label: '<%=lang.d[this, "colAction"]       %>', width: 090, name: 'Action', search: false, editable: false, sortable: false, fixed: true, formatter: function (cellvalue, options, rowObject) { return '<div class="row_act_new"></div>'; }, },
                    { label: '<%=lang.d[this, "colACNT"]         %>', width: 100, name: 'ACNT', sorttype: "text", editable: true, },
                    { label: '<%=lang.d[this, "colCorpACNT"]     %>', width: 100, name: 'CorpACNT', sorttype: "text", editable: false },
                    { label: '<%=lang.d[this, "colParentACNT"]    %>', width: 100, name: 'ParentACNT', sorttype: "text", editable: false },
                    { label: '<%=lang.d[this, "colName"]         %>', width: 100, name: 'Name', sorttype: "text", editable: true },
                    { label: '<%=lang.d[this, "colPassword"]     %>', width: 080, name: 'Password', sorttype: "text", editable: true, search: false, hidden: true },
                    { label: '<%=lang.d[this, "colLocked"]       %>', width: 100, name: 'Locked', editable: true, edittype: 'checkbox', editoptions: { value: "true:false" }, formatter: 'checkbox' },
                    { label: '<%=lang.d[this, "colCreateTime"]   %>', width: 100, name: 'CreateTime', sorttype: "date", editable: false },
                    { label: '<%=lang.d[this, "colModifyTime"]   %>', width: 100, name: 'ModifyTime', sorttype: "date", editable: false },
                    { label: '<%=lang.d[this, "colModifyAdminID"]%>', width: 100, name: 'ModifyAdminID', sorttype: "text", editable: false },
            ]

            grid.$table.jqGrid({
                colModel: colModel,

                url: 'api',
                editurl: 'api',
                mtype: 'POST',
                datatype: 'json',

                pager: grid.$pager.selector,
                //pagerpos: 'right',
                rowNum: 10,
                rowList: [10, 20, 30, 50, 100],
                viewrecords: true,
                records: true,
                toppager: true,

                width: 800,
                height: 250,
                shrinkToFit: false,
                //width:'100%',
                forceFit: true,
                autowidth: true,

                //treeGrid: false,
                //treeGridModel: 'adjacency',
                //ExpandColumn: 'ACNT',
                //treeReader: {
                //    level_field: "level",
                //    parent_id_field: "AdminID", // then why does your table use "parent_id"?
                //    leaf_field: "isLeaf",
                //    expanded_field: "expanded"
                //},

                //toolbar: [true, 'top'],
                rownumbers: true,
                //footerrow: true,
                sortname: 'CreateTime',
                sortorder: "desc",
                emptyrecords: "Nothing to display",
                //headertitles: false,
                multiselect: false,
                jsonReader: { repeatitems: false },
                //caption: "Manipulating Array Data"

                //beforeRequest: function () {
                //    writelog("beforeRequest", arguments);
                //},
                serializeGridData: grid.params.serializeGridData,
                //loadBeforeSend: function (xhr, settings) { writelog("loadBeforeSend", arguments); },
                beforeProcessing: function (data, status, xhr) {
                    writelog("beforeProcessing", arguments);
                    if (data.jqRowList) {
                        for (var a in data.jqRowList)
                            data[a] = data.jqRowList[a];
                        delete data.jqRowList;
                        if (data.rows) {
                            for (var i = 0; i < data.rows.length; i++) {
                                var row = data.rows[i];
                                //row.reg_t = Date.fromISO(row.reg_t);
                                //row.login_t = Date.fromISO(row.login_t);
                            }
                        }
                    }
                    return true;
                },
                //gridComplete: function () { writelog('gridComplete', arguments); },
                //loadComplete: function (data) { writelog("loadComplete", arguments); },
                afterInsertRow: function (rowid, rowdata, rowelem) {
                    //writelog("afterInsertRow", arguments);
                    grid.row_act.setcell(rowid);
                },
                // onSelectRow: function (id) { writelog("onSelectRow", arguments); },
                // beforeSelectRow: function (rowid, e) { writelog("beforeSelectRow", arguments); },
                loadError: function (xhr, status, error) {
                    writelog("loadError", arguments);
                },
                //onCellSelect: function (rowid, iCol, cellcontent, e) { writelog("onCellSelect", arguments); },
                //ondblClickRow: function (rowid, iRow, iCol, e) { writelog("ondblClickRow", arguments); },
                //onHeaderClick: function (gridstate) { writelog("onHeaderClick", arguments); },
                //onPaging: function (pgButton) { writelog("onPaging", arguments); },
                //onRightClickRow: function (rowid, iRow, iCol, e) { writelog("onRightClickRow", arguments); },
                //onSelectAll: function (aRowids, status) { writelog("onSelectAll", arguments); },
                //onSelectRow: function (rowid, status, e) { writelog("onSelectRow", arguments); },
                //onSortCol: function (index, iCol, sortorder) { writelog("onSortCol", arguments); },
                //resizeStart: function (event, index) { writelog("resizeStart", arguments); },
                //resizeStop: function (newwidth, index) { writelog("resizeStop", arguments); },
                serializeRowData: grid.params.beforesavefunc,
            });

            var $filter = grid.$table.filterToolbar({
                autosearch: false,
                searchOnEnter: true,
                beforeSearch: function () { writelog("$filter.beforeSearch", arguments); },
                afterSearch: function () { writelog("$filter.afterSearch", arguments); },
                beforeClear: function () { writelog("$filter.beforeClear", arguments); },
                afterClear: function () { writelog("$filter.afterClear", arguments); },
                searchurl: '',
                stringResult: false,
                groupOp: 'AND',
                defaultSearch: "bw",
                searchOperators: false,
                operandTitle: "Click to select search operation.",
                operands: { "eq": "==", "ne": "!", "lt": "<", "le": "<=", "gt": ">", "ge": ">=", "bw": "^", "bn": "!^", "in": "=", "ni": "!=", "ew": "|", "en": "!@", "cn": "~", "nc": "!~", "nu": "#", "nn": "!#" }
            });

            // 工具列
            var $topPager = $('#' + $.jgrid.jqID(grid.$table[0].id) + "_toppager");


            grid.$table.navGrid($topPager.selector, { search: false, edit: false, add: false, del: false, refresh: false });

            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnSearch0"] %>', title: 'search', buttonicon: 'ui-icon-search', onClickButton: function () { grid.$table[0].triggerToolbar() } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnSearch1"] %>', title: 'toggle', buttonicon: 'ui-icon-pin-s', onClickButton: function () { grid.$table[0].toggleToolbar() } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnSearch2"] %>', title: 'clear', buttonicon: 'ui-icon-close', onClickButton: function () { grid.$table[0].clearToolbar() } });
            grid.$table.navSeparatorAdd($topPager.selector);
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnRefresh"] %>', title: 'refresh', buttonicon: 'ui-icon-refresh', onClickButton: function () { grid.$table.trigger('reloadGrid'); } });
            grid.$table.navSeparatorAdd($topPager.selector);
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnAdd"]     %>', title: 'add', buttonicon: 'ui-icon-plus', onClickButton: grid.inline.addRow });
            grid.$table.navSeparatorAdd($topPager.selector);
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnDetail"]  %>', title: 'detail', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnSubAcc"]  %>', title: 'subacc', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnAddPoint"]%>', title: 'addpoint', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnReport"]  %>', title: 'report', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnBonus"]   %>', title: 'bonus', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this,"btnLog"]     %>', title: 'log', buttonicon: 'ui-icon-comment', onClickButton: function () { } });

            $($topPager.selector + '_left').css('width', '100%');
            $($topPager.selector + '_center').hide();
            $($topPager.selector + '_right').hide();

            var $detailview = $('#detail-view');

            var resize;
            (resize = function () {
                var w = window.innerWidth - 25;
                grid.$table.setGridWidth(w, false);
                $detailview.width(w);
            })();
            resize();

            $(window).resize(resize);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <table id="table"></table>
    <div id="pager"></div>

    <div id="templates" style="display:none;">
        <div class="row_act">
            <div class="ui-pg-div ui-inline-edit"><span class="ui-icon ui-icon-pencil"></span><%= lang.d[this,"btnEdit"] %></div>
            <div class="ui-pg-div ui-inline-cancel"><span class="ui-icon ui-icon-cancel"></span><%= lang.d[this,"btnCancel"] %></div>
            <div class="ui-pg-div ui-inline-save"><span class="ui-icon ui-icon-disk"></span><%= lang.d[this,"btnSave"] %></div>
        </div>
    </div>
</asp:Content>

