<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" %>
<%@ Import Namespace="Resources" %>
<%--<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .row_act {
            white-space:nowrap;
        }
        .row_act > div, .row_act > div > span { float: left; cursor: pointer; }
        .row_act          > div.ui-inline-edit { display: block; }
        .row_act          > div.ui-inline-cancel ,
        .row_act          > div.ui-inline-save { display: none; }
        .row_act.editable > div.ui-inline-edit { display: none; }
        .row_act.editable > div.ui-inline-cancel,
        .row_act.editable > div.ui-inline-save { display: block; }
        
    </style>
    <script type="text/javascript">

        var usertypes = { 0: '試玩', 1: '普通', 2: 'VIP', 3: '鑽石VIP' }

        $(document).ready(function () {

            var grid = init_grid(
            '#table',
            '#pager',
            '#templates', {
                serializeGridData: function (postData) {
                    return { Member_Select: postData };
                },
                beforesave: function (postData) {
                    return { Member_Update: postData };
                },
                update_success: function (rowid, data) {
                    writelog("update_success", data);
                    writelog("update_success", grid.$table.getRowData(data.rowid));
                    return $.extend(true, {
                        ID: '',
                        ACNT: '',
                        UserType: '',
                        Password: '',
                        Locked: '',
                        Name: '',
                        Tel1: '',
                        Mail1: '',
                        CorpACNT: '',
                        AgentACNT: '',
                        Currency: '',
                        Bank: '',
                        Memo: '',
                    }, data);
                },
            });

            //$.fn.fmatter.checkbox1 = function (cval, opts) {
            //    var op = $.extend({}, opts.checkbox), ds;
            //    if (opts.colModel !== undefined && opts.colModel.formatoptions !== undefined) {
            //        op = $.extend({}, op, opts.colModel.formatoptions);
            //    }
            //    if (op.disabled === true) { ds = "disabled=\"disabled\""; } else { ds = ""; }
            //    if ($.fmatter.isEmpty(cval) || cval === undefined) { cval = $.fn.fmatter.defaultFormat(cval, op); }
            //    cval = String(cval);
            //    cval = cval.toLowerCase();
            //    var bchk = cval.search(/(false|f|0|no|n|off|undefined)/i) < 0 ? " checked='checked' " : "";
            //    return "<input type=\"checkbox\" " + bchk + " value=\"" + cval + "\" offval=\"no\" " + ds + "/>";
            //};

            //{
            //    label: '<%= lang.d[this, "acnt"]%>', width: 100, name: 'acnt', index: 'acnt', sorttype: "text", frozen: true, editable: true, edittype: 'custom', editoptions: {
            //        custom_element: function (value, options) {
            //            //writelog("acnt.custom_element", arguments);
            //            if (value == consts.new_acnt) {
            //                var elem = document.createElement("input");
            //                elem.type = "text";
            //                elem.value = "";
            //                return elem
            //            }
            //            else {
            //                var elem = document.createElement("label");
            //                elem.innerText = value;
            //                $(elem).val(value);
            //                return elem;
            //            }
            //        },
            //        custom_value: function (elem, operation, value) {
            //            //writelog("acnt.custom_value", arguments);
            //            if (operation === 'get') {
            //                return $(elem).val();
            //            } else if (operation === 'set') {
            //                $('input', elem).val(value);
            //            }
            //        }
            //    }
            //},

            var colModel =  [
                    { name: 'rowid', sorttype: "int", frozen: true, hidden: true, key: true },
                    { label: '<%=lang.d[this, "colID"]          %>', width: 050, frozen: true, name: 'ID', sorttype: "int", editable: true, hidden: true },
                    { label: '<%=lang.d[this, "colAction"]      %>', width: 090, frozen: true, name: 'Action', search: false, editable: false, sortable: false, fixed: true, formatter: function (cellvalue, options, rowObject) { return '<div class="row_act_new"></div>'; }, },
                    { label: '<%=lang.d[this, "colACNT"]        %>', width: 080, name: 'ACNT', sorttype: "text", editable: true, },
                    { label: '<%=lang.d[this, "colUserType"]    %>', width: 080, name: 'UserType', sorttype: "int", editable: true, edittype: 'select', editoptions: { value: usertypes }, formatter: 'select', /*formatoptions: { value: usertypes },*/ },
                    { label: '<%=lang.d[this, "colPassword"]    %>', width: 080, name: 'Password', sorttype: "text", editable: true, search: false, hidden: true },
                    { label: '<%=lang.d[this, "colLocked"]      %>', width: 100, name: 'Locked', editable: true, edittype: 'checkbox', editoptions: { value: "true:false" }, formatter: 'checkbox' },
                    { label: '<%=lang.d[this, "colName"]        %>', width: 100, name: 'Name', sorttype: "text", editable: true },
                    { label: '<%=lang.d[this, "colTel1"]        %>', width: 100, name: 'Tel1', editable: true },
                    { label: '<%=lang.d[this, "colMail1"]       %>', width: 100, name: 'Mail1', sorttype: "text", editable: true },
                    { label: '<%=lang.d[this, "colCorpACNT"]    %>', width: 100, name: 'CorpACNT', sorttype: "text", editable: true },
                    { label: '<%=lang.d[this, "colAgentACNT"]   %>', width: 100, name: 'AgentACNT', sorttype: "text", editable: true },
                    { label: '<%=lang.d[this, "colBalance"]     %>', width: 100, name: 'Balance', sorttype: "int", editable: false },
                    { label: '<%=lang.d[this, "colCurrency"]    %>', width: 100, name: 'Currency', sorttype: "text", editable: true },
                    { label: '<%=lang.d[this, "colCreateTime"]  %>', width: 100, name: 'CreateTime', sorttype: "text" },
                    { label: '<%=lang.d[this, "colLoginTime"]   %>', width: 100, name: 'LoginTime', sorttype: "date", formatter: "date" },
                    { label: '<%=lang.d[this, "colLoginIP"]     %>', width: 100, name: 'LoginIP', sorttype: "text" },
                    { label: '<%=lang.d[this, "colDepositCount"]%>', width: 100, name: 'DepositCount', sorttype: "int" },
                    { label: '<%=lang.d[this, "colBank"]        %>', width: 100, name: 'Bank', sorttype: "text", editable: true },
                    { label: '<%=lang.d[this, "colMemo"]        %>', width: 100, name: 'Memo', sorttype: "text", editable: true },
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
                                row.reg_t = Date.fromISO(row.reg_t);
                                row.login_t = Date.fromISO(row.login_t);
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
            //$table.setFrozenColumns();
            // {"MemberList":}

            //$table[0].addJSONData({
            //    records: 55,
            //    total:40,
            //    rows: [
            //        { "id": 1, "acnt": "aaa" },
            //        { "id": 2, "acnt": "bbb" }]
            //});


            // 工具列
            var $topPager = $('#' + $.jgrid.jqID(grid.$table[0].id) + "_toppager");
            //writelog($topPager.selector);

            grid.$table.navGrid($topPager.selector, { search: false, edit: false, add: false, del: false, refresh: false });

            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this, "btnSearch0"]%>', title: 'search', buttonicon: 'ui-icon-search', onClickButton: function () { grid.$table[0].triggerToolbar() } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this, "btnSearch1"]%>', title: 'toggle', buttonicon: 'ui-icon-pin-s', onClickButton: function () { grid.$table[0].toggleToolbar() } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this, "btnSearch2"]%>', title: 'clear', buttonicon: 'ui-icon-close', onClickButton: function () { grid.$table[0].clearToolbar() } });
            grid.$table.navSeparatorAdd($topPager.selector);
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%=lang.d[this, "btnRefresh"]%>', title: 'refresh', buttonicon: 'ui-icon-refresh', onClickButton: function () { grid.$table.trigger('reloadGrid'); } });
            grid.$table.navSeparatorAdd($topPager.selector);
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%= lang.d[this, "btnAdd"]%>', title: 'add', buttonicon: 'ui-icon-plus', onClickButton: grid.inline.addRow });
            grid.$table.navSeparatorAdd($topPager.selector);
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%= lang.d[this, "btnDetail"]%>', title: 'detail', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%= lang.d[this, "btnSubAcc"]%>', title: 'subacc', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%= lang.d[this, "btnAddPoint"]%>', title: 'addpoint', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%= lang.d[this, "btnReport"]%>', title: 'report', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%= lang.d[this, "btnBonus"]%>', title: 'bonus', buttonicon: 'ui-icon-comment', onClickButton: function () { } });
            grid.$table.navButtonAdd($topPager.selector, { caption: '<%= lang.d[this, "btnLog"]%>', title: 'log', buttonicon: 'ui-icon-comment', onClickButton: function () { } });

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
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <table id="table"></table>
    <div id="pager"></div>

    <div id="templates" style="display:none;">
        <div class="row_act">
            <div class="ui-pg-div ui-inline-edit"><span class="ui-icon ui-icon-pencil"></span><%= lang.d[this, "btnEdit"]%></div>
            <div class="ui-pg-div ui-inline-cancel"><span class="ui-icon ui-icon-cancel"></span><%= lang.d[this, "btnCancel"]%></div>
            <div class="ui-pg-div ui-inline-save"><span class="ui-icon ui-icon-disk"></span><%= lang.d[this, "btnSave"]%></div>
        </div>
    </div>

    <div id="detail-view" class="ui-jqgrid ui-widget ui-widget-content ui-corner-all" style="display:none;">
        <div class="ui-jqgrid-view">
            <div class="ui-state-default ui-jqgrid-toppager">
                <div class="ui-pager-control">
                    <table cellspacing="0" cellpadding="0" border="0" class="ui-pg-table navtable" style="float: left; table-layout: auto;">
                        <tbody>
                            <tr>
                                <td class="ui-pg-button ui-corner-all" title="add">
                                    <div class="ui-pg-div"><span class="ui-icon ui-icon-plus"></span>新增</div>
                                </td>
                                <td class="ui-pg-button ui-state-disabled" style="width: 4px;"><span class="ui-separator"></span></td>
                                <td class="ui-pg-button ui-corner-all" title="detail">
                                    <div class="ui-pg-div"><span class="ui-icon ui-icon-comment"></span>詳細</div>
                                </td>
                                <td class="ui-pg-button ui-corner-all" title="subacc">
                                    <div class="ui-pg-div"><span class="ui-icon ui-icon-comment"></span>子帳戶</div>
                                </td>
                                <td class="ui-pg-button ui-corner-all" title="addpoint">
                                    <div class="ui-pg-div"><span class="ui-icon ui-icon-comment"></span>加扣點</div>
                                </td>
                                <td class="ui-pg-button ui-corner-all" title="report">
                                    <div class="ui-pg-div"><span class="ui-icon ui-icon-comment"></span>報表</div>
                                </td>
                                <td class="ui-pg-button ui-corner-all" title="bonus">
                                    <div class="ui-pg-div"><span class="ui-icon ui-icon-comment"></span>優惠</div>
                                </td>
                                <td class="ui-pg-button ui-corner-all" title="log">
                                    <div class="ui-pg-div"><span class="ui-icon ui-icon-comment"></span>紀錄</div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            123
        </div>
        456
    </div>

</asp:Content>
