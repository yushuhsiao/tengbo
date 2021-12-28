<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="CashChannel.aspx.cs" Inherits="CashChannel_aspx" %>

<script runat="server">
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
    </style>
    <script type="text/javascript">
        var $table;

        $(document).ready(function () {
            var enum2 = {
                <%=enumlist<BU.LogType>(" logTypes")%>
                <%=enumlist<BU.LogType>(",logTypes1", true, CashChannel_aspx.LogTypes)%>
            };

            $table = $('#table01').jqGrid_init({
                pager: true, shrinkToFit: false, subGrid: true, sortname: 'CreateTime', sortorder: "desc",
                SelectCommand: function (postData) { return { CashChannelSelect: postData }; },
                UpdateCommand: function (postData) { postData.LogType = $table.getRowData(postData.ID).LogType; return { CashChannelUpdate: postData }; },
                InsertCommand: function (postData) { return { CashChannelInsert: postData }; },
                subGridBeforeExpand: function (pID, id, ind) { $table.restoreRow(id); },
                subGridRowCreated: function (pID, id, ind, tablediv) {
                    tablediv.parentElement.colSpan--;
                    $('<td class="ui-widget-content subgrid-cell" colspan="1"></td>').insertBefore(tablediv.parentElement);
                    var rowdata = $table.getRowData(id);
                    var data;
                    if (rowdata.LogType == 0) return; <% for (int i = 0; i < RowCommands.Length; i++) { %>
                    else if (rowdata.LogType == '<%=(int)RowCommands[i].AcceptLogType%>') {
                        data = [
                            { k: 'MerhantID', d: '<%=lang["colMerhantID  "]%>', v: rowdata['MerhantID'] }, <% foreach (string field in Schemas[i].Keys) { if (!ShareFields.Contains(field)) { %>
                            { k: '<%=field%>', d: '<%=lang["field_" + field] ?? field%>', v: rowdata['<%=field%>'] },<% } } %>
                        ];
                    } <% } %>
                    else return;

                    var $table2 = $('.table02').clone().removeClass('table02').prop('id', 'table02' + id).appendTo(tablediv).jqGrid_init({
                            data: data,
                            headervisible: false, cmTemplate: { sortable: false, align: 'left' }, editParams: { url: 'api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: true, shrinkToFit: true,
                            onSelectRow: function (rowid, status, e) { $($(this).getInd(rowid, true)).removeClass('ui-state-highlight'); },
                            UpdateCommand: function (postData) {
                                var postData2 = { ID: id, LogType: rowdata.LogType };
                                postData2[postData.k] = postData.v;
                                return { CashChannelUpdate: postData2 };
                            },
                            RowResponse: function (res, rowid, row) {
                                $table.setRowData(id, row);
                                row['k'] = rowid;
                                row['v'] = row[rowid];
                            },
                            colModel: [
                                { name: 'Action', label: '<%=lang["colAction    "]%>', colType: 'Buttons' },
                                { name: 'k     ', width: 150, fixed: true, key: true, hidden: true },
                                { name: 'd     ', width: 150, fixed: true },
                                { name: 'v     ', width: 500, editable: true },
                            ]
                        });
                    $table2.gridContainer().removeClass('ui-corner-all').css('border', 0);
                },
                subGridRowExpanded: function (pID, id, ind, tablediv) { },
                subGridBeforeColapsed: function (pID, id, ind, tablediv) { },
                subGridRowRemoved: function (pID, id, ind, tablediv) { },
                subGridRowColapsed: function (pID, id, ind, tablediv) { },

                colModel: [
                    { name: 'Action     ', label: '<%=lang["colAction     "]%>', colType: 'Buttons' },
                    { name: 'CorpID     ', label: '<%=lang["colCorpID     "]%>', colType: 'CorpID', editonce: false },
                    { name: 'Locked     ', label: '<%=lang["colLocked     "]%>', colType: 'Locked' },
                    { name: 'LogType    ', label: '<%=lang["colLogType    "]%>', width: 080, editable: true, editonce: true, formatter: 'select', formatoptions: { value: enum2.logTypes }, edittype: 'select', editoptions: { value: enum2.logTypes1 }, search: true, stype: 'select', searchoptions: { value: enum2.logTypes1, defaultValue: '-1', nullKey: '-1', nullValue: '--' } },
                    { name: 'DisplayName', label: '<%=lang["colDisplayName"]%>', width: 250, editable: true },
                    { name: 'MerhantID  ', label: '<%=lang["colMerhantID  "]%>', width: 200, editable: true, align: 'left' },
                    { name: 'FeesRate   ', label: '<%=lang["colFeesRate   "]%>', width: 080, editable: true, formatter: 'integer' },
                    { name: 'CreateTime ', label: '<%=lang["colCreateTime "]%>', colType: 'DateTime2' },
                    { name: 'CreateUser ', label: '<%=lang["colCreateUser "]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime ', label: '<%=lang["colModifyTime "]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser ', label: '<%=lang["colModifyUser "]%>', colType: 'ACNT2' },
                    <% foreach (string field in AllFields) { if (!ShareFields.Contains(field)) { %>
                    { name: '<%=field%>', editable: false, hidden: true },<% } } %>
                    { name: 'ID         ', label: '<%=lang["colID         "]%>', width: 300, colType: 'ID'<% if (showID) { %>, hidden: false<% } %> },
                    { hidden: true }
                ]
            });
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <table id="table01">
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
                    <button action="toggleSearch" icon="ui-icon-search" ><%=lang["btnSearch"]%></button>
                    <button action="reloadGrid"   icon="ui-icon-refresh"><%=lang["btnRefresh"]%></button>
                    <button action="addRow"       icon="ui-icon-plus"   ><%=lang["btnAdd"]%></button>
                </div>
                <div name="nav2" class="ui-widget-content" style=""></div>
            </td>
        </tr>
    </table>
    <table class="table02">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
    </table>
</asp:Content>

