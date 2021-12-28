<%@ Page Title="" Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" CodeBehind="UserList_.aspx.cs" Inherits="web.page" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css"> .ui-resizable-e { cursor: col-resize; } </style>
    <script type="text/javascript">
        var $table_t, $table_a, $table_m, init_complete = false;
        var $active;

        var recvMessage = {
            MemberRowData: function (data) {
                $table_m.restoreRow(data.ID);
                $table_m.setRowData(data.ID, data);
            },
            AgentRowData: function (data) {
                $table_a.restoreRow(data.ID);
                $table_a.setRowData(data.ID, data);
            }
        }

        function setToolbar1(disabled) {
            $('button[action=addRow]').button("option", "disabled", disabled);
            $('button[action=resetSelection]').button("option", "disabled", disabled);
            if (disabled) {
                $table_a.showCol('ParentACNT');
                $table_m.showCol('AgentACNT');
                <% if (showCorpID){ %>
                $table_a.showCol('CorpID');
                $table_m.showCol('CorpID');
                <% } %>
            }
            else {
                $table_a.hideCol('ParentACNT');
                $table_m.hideCol('AgentACNT');
                <% if (showCorpID){ %>
                $table_a.hideCol('CorpID');
                $table_m.hideCol('CorpID');
                <% } %>
            }
        };

        function onSelectRow(rowid, status, e, force) {
            var selrow = $table_t.getGridParam('selrow');
            setToolbar1(selrow == null);
            if (($active[0].grid.select_t != selrow) || (force == true)) {
                $active[0].grid.select_t = selrow;
                $active.clearGridData();
                $active.reloadGrid();
            }
        };

        function grid_split(width) {
            var hh = $(window).innerHeight();
            var ww = $(window).innerWidth();
            var s = 2;
            $table_t.gridWidth(width);
            $table_a.gridContainer().css({ left: width + s });
            $table_m.gridContainer().css({ left: width + s });
            ww = ww - width - s - s;
            $table_a.gridWidth(ww);
            $table_m.gridWidth(ww);
            $table_t.gridHeight(hh);
            $table_a.gridHeight(hh);
            $table_m.gridHeight(hh);
        };

        var navButton_action = {
            toggleSearch: function () { $(this)[0].toggleToolbar(); $(window).trigger("resize"); },
            switch_a: function () { $table_a.setactive(); },
            switch_m: function () { $table_m.setactive(); },
            resetSelection: function () { $table_t.resetSelection(); onSelectRow(); }
        };

        $.fn.setactive = function () {
            var $t1 = $active = this;
            var $t2 = this == $table_a ? $table_m : $table_a;
            $t1.gridContainer().show();
            $t2.gridContainer().hide();
            onSelectRow(null, null, null, true);
            return this;
        };

        $(function () {
            $table_t = $('#table_tree').jqGrid_init({
                pager: false, rownumbers: false, viewrecords: false, cmTemplate: { sortable: false },
                colModel: [
                    { name: 'DisplayName', label: '<%=lang["colACNT      "]%>', width: 200, editable: false, editonce: false, align: 'left', formatter: function (cellval, opts, rwd, act) { if (rwd.ACNT == rwd.Name) return rwd.ACNT; else return '{0} ({1})'.format(rwd.ACNT, rwd.Name); } },
                    { name: 'CorpID     ', label: '<%=lang["colCorpID    "]%>', colType: 'CorpID'<% if (showCorpID) { %>, hidden: false <% } %> },
                    { name: 'ACNT       ', label: '<%=lang["colACNT      "]%>', width: 080, editable: false, editonce: false, align: 'left' },
                    { name: 'Name       ', label: '<%=lang["colName      "]%>', width: 080, editable: false, hidden: true },
                    { name: 'ID         ', label: '<%=lang["colID        "]%>', width: 100, colType: 'ID'<%if (showID) { %>, hidden: false<% }%> }
                ],
                treeGrid: true, treeGridModel: 'adjacency', tree_root_level: 1, ExpandColumn: 'DisplayName', treeReader: { level_field: "level", parent_id_field: "ParentID", leaf_field: "isLeaf", expanded_field: "expanded", loaded: "loaded", icon_field: "icon" },
                SelectCommand: function (postData) { return { AgentTreeSelect: postData } },
                onSelectRow: onSelectRow,
                navButton_action: {
                    resetSelection: function () { $table_t.resetSelection(); onSelectRow(); }
                }
            }).setGridWidth(200);
            $table_a = $('#table_agent').jqGrid_init({
                pager: true, subGrid: true, navButton_action: navButton_action, detail_root: '.details-a-root',
                colModel: [
                    { name: 'Action       ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
                    { name: 'ID           ', label: '<%=lang["colID           "]%>', colType: 'ID'<%if (showID) { %>, hidden: false<% }%>, search: false },
                    { name: 'CorpID       ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                    { name: 'ParentACNT   ', label: '<%=lang["colParentACNT   "]%>', colType: 'ACNT', search: true },
                    { name: 'ACNT         ', label: '<%=lang["colACNT         "]%>', colType: 'ACNT', width: 120, search: true },
                    { name: 'Name         ', label: '<%=lang["colName         "]%>', width: 080, search: true, editable: true },
                    { name: 'GroupID      ', label: '<%=lang["colGroupID      "]%>', width: 080, editable: true, editable_func: function (rwd, new_row) { return !new_row; }, formatter: 'select', formatoptions: {<%=serializeEnum<long,string>("value", web.AgentGroupRow.Cache.Instance.Value2)%> }, edittype: 'select', editoptions: { value_func: function (rowdata) { return <%=web.api.SerializeObject(web.AgentGroupRow.Cache.Instance.Value1)%>[rowdata.CorpID] || {}; } } },
                    { name: 'Locked       ', label: '<%=lang["colLocked       "]%>', colType: 'Locked' },
                    { name: 'Balance      ', label: '<%=lang["colBalance      "]%>', width: 080, sorttype: 'currency', editable: false, formatter: 'currency' },
                    { name: 'Currency     ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency', hidden: true },
                    { name: 'Memo         ', label: '<%=lang["colMemo         "]%>', width: 080, editable: true },
                    { name: 'CreateTime   ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', sortable: true },
                    { name: 'RegisterIP   ', label: '<%=lang["colRegisterIP   "]%>', width: 100, editable: false },
                    { name: 'LoginTime    ', label: '<%=lang["colLoginTime    "]%>', colType: 'DateTime2' },
                    { name: 'LoginIP      ', label: '<%=lang["colLoginIP      "]%>', width: 100, editable: false },
                    { name: 'LoginCount   ', label: '<%=lang["colLoginCount   "]%>', width: 060, editable: false, editonce: false, hidden: true },
                    { name: 'CreateUser   ', label: '<%=lang["colCreateUser   "]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime   ', label: '<%=lang["colModifyTime   "]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser   ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' }
                ],
                SelectCommand: function (postData) { postData.ParentID = $table_t.getGridParam('selrow'); return { AgentSelect: postData } },
                UpdateCommand: function (postData) { postData.ParentID = $table_t.getGridParam('selrow'); return { AgentUpdate: postData } },
                InsertCommand: function (postData) { postData.ParentID = $table_t.getGridParam('selrow'); return { AgentInsert: postData } },
                subGridBeforeExpand: function (pID, id, ind) { },
                subGridRowCreated: function (pID, id, ind, tablediv) {
                    $('.tmp').clone().children().appendTo(tablediv);
                    $('iframe', tablediv).load(function () {
                        $(this).show();
                        $('.detail-content-loading', tablediv).hide();
                    }).prop('src', 'AgentDetails.aspx?id=' + id);
                },
                subGridRowExpanded: function (pID, id, ind, tablediv) { },
                subGridBeforeColapsed: function (pID, id, ind, tablediv) { },
                subGridRowRemoved: function (pID, id, ind, tablediv) { },
                subGridRowColapsed: function (pID, id, ind, tablediv) { },
                loadBeforeSend: function (xhr, settings) { return init_complete; },
                RowResponse: function (res, rowid, o) {
                    try {
                        var trow = $table_t.getRowData(o.ID);
                        if (trow.ID == null) {
                            o.isLeaf = false;
                            o.expanded = false;
                            o.loaded = false;
                            $table_t.addChildNode(o.ID, o.ParentID, o);
                        }
                    } catch (e) { }
                }
            });
            $table_m = $('#table_member').jqGrid_init({
                pager: true, subGrid: true, navButton_action: navButton_action, detail_root: '.details-m-root',
                colModel: [
                    { name: 'Action       ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
                    { name: 'ID           ', label: '<%=lang["colID           "]%>', colType: 'ID'<%if (showID) { %>, hidden: false<% }%>, search: false },
                    { name: 'CorpID       ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                    { name: 'AgentACNT    ', label: '<%=lang["colAgentACNT    "]%>', colType: 'ACNT', search: true, editable: true, editonce: true },
                    { name: 'ACNT         ', label: '<%=lang["colACNT         "]%>', colType: 'ACNT', width: 120, search: true },
                    { name: 'Name         ', label: '<%=lang["colName         "]%>', width: 080, search: true, editable: true },
                    { name: 'GroupID      ', label: '<%=lang["colGroupID      "]%>', width: 080, editable: true, editable_func: function (rwd, new_row) { return !new_row; }, formatter: 'select', formatoptions: {<%=serializeEnum<long,string>("value", web.MemberGroupRow.Cache.Instance.Value2)%> }, edittype: 'select', editoptions: { value_func: function (rowdata) { return <%=web.api.SerializeObject(web.MemberGroupRow.Cache.Instance.Value1)%>[rowdata.CorpID] || {}; } } },
                    { name: 'Locked       ', label: '<%=lang["colLocked       "]%>', colType: 'Locked' },
                    { name: 'Balance      ', label: '<%=lang["colBalance      "]%>', width: 080, editable: false, formatter: 'currency' },
                    { name: 'Currency     ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency', hidden: true },
                    { name: 'Memo         ', label: '<%=lang["colMemo         "]%>', width: 080, editable: true },
                    { name: 'CreateTime   ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', sortable: true },
                    { name: 'RegisterIP   ', label: '<%=lang["colRegisterIP   "]%>', width: 100, editable: false },
                    { name: 'LoginTime    ', label: '<%=lang["colLoginTime    "]%>', colType: 'DateTime2' },
                    { name: 'LoginIP      ', label: '<%=lang["colLoginIP      "]%>', width: 100, editable: false },
                    { name: 'LoginCount   ', label: '<%=lang["colLoginCount   "]%>', width: 050, editable: false, editonce: false },
                    { name: 'CreateUser   ', label: '<%=lang["colCreateUser   "]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime   ', label: '<%=lang["colModifyTime   "]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser   ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' }
                ],
                SelectCommand: function (postData) { postData.AgentID = $table_t.getGridParam('selrow'); return { MemberSelect: postData } },
                UpdateCommand: function (postData) { postData.AgentID = $table_t.getGridParam('selrow'); return { MemberUpdate: postData } },
                InsertCommand: function (postData) { postData.AgentID = $table_t.getGridParam('selrow'); return { MemberInsert: postData } },
                subGridBeforeExpand: function (pID, id, ind) { },
                subGridRowCreated: function (pID, id, ind, tablediv) {
                    $('.tmp').clone().children().appendTo(tablediv);
                    $('iframe', tablediv).load(function () {
                        $(this).show();
                        $('.detail-content-loading', tablediv).hide();
                    }).prop('src', 'MemberDetails.aspx?id=' + id);
                },
                subGridRowExpanded: function (pID, id, ind, tablediv) { },
                subGridBeforeColapsed: function (pID, id, ind, tablediv) { },
                subGridRowRemoved: function (pID, id, ind, tablediv) { },
                subGridRowColapsed: function (pID, id, ind, tablediv) { },
                loadBeforeSend: function (xhr, settings) { return init_complete; }
            });
            $table_t.gridContainer().resizable({ minWidth: 0, maxWidth: 800, autoHide: true, handles: 'e', resize: function (event, ui) { grid_split(ui.size.width); } });
            $table_a.gridContainer().css({ 'position': 'absolute', 'top': 0, });
            $table_m.gridContainer().css({ 'position': 'absolute', 'top': 0, });
            $(window).resize(function () { grid_split($table_t.gridWidth()); }).trigger("resize");
            $table_a[0].grid.select_t = $table_m[0].grid.select_t = init_complete = true;
            setTimeout(function () { $table_m.setactive(); }, 100);
        });
    </script>
    <style type="text/css">
        /*.detail-content-loading { position: absolute; left: 8px; top: 5px; }*/
        .detail-content-loading div { background: url(../images/loading3_000000.gif) #fff no-repeat center center; width: 32px; height: 32px; margin: 1px; border-width: 1px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="tmp" style="display:none">
        <div class="ui-widget-content ui-state-active detail-content-loading" style="display: inline-block;"><div></div></div>
        <iframe frameBorder="0" style="display: none; width:98%; height:1px; border: 0;"></iframe>
    </div>
    <table id="table_tree">
        <tr class="grid-option">
            <td>
                <div name="nav1">
                    <button action="resetSelection" icon="ui-icon-comment" disabled="true"><%=lang["btnResetSelT"]%></button>
                </div>
            </td>
        </tr>
    </table>
    <table id="table_agent">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
        <tr class="grid-option">
            <td>
                <div name="nav1">
                    <button action="addRow"         icon="ui-icon-plus"    disabled="true"><%=lang["btnAdd"]+lang["btnAgent"]%></button>
                    <button action="switch_a"       icon="ui-icon-comment" disabled="true"><%=lang["btnAgent"]%></button>
                    <button action="switch_m"       icon="ui-icon-comment"                ><%=lang["btnMember"]%></button>
                    <button action="toggleSearch"   icon="ui-icon-search"                 ><%=lang["btnSearch"]%></button>
                    <button action="reloadGrid"     icon="ui-icon-refresh"                ><%=lang["btnRefresh"]%></button>
                </div>
            </td>
        </tr>
    </table>
    <table id="table_member">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
        <tr class="grid-option">
            <td>
                <div name="nav1">
                    <button action="addRow"         icon="ui-icon-plus"    disabled="true"><%=lang["btnAdd"]+lang["btnMember"]%></button>
                    <button action="switch_a"       icon="ui-icon-comment"                ><%=lang["btnAgent"]%></button>
                    <button action="switch_m"       icon="ui-icon-comment" disabled="true"><%=lang["btnMember"]%></button>
                    <button action="toggleSearch"   icon="ui-icon-search"                 ><%=lang["btnSearch"]%></button>
                    <button action="reloadGrid"     icon="ui-icon-refresh"                ><%=lang["btnRefresh"]%></button>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
