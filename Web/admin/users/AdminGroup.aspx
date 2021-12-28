<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="web.page" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var $table;
        $(document).ready(function () {
            $table = $('#table01').jqGrid_init({
                pager: true, toppager: false, pginput: false, rowList: [], shrinkToFit: true, loadonce: false, subGrid: true,
                sortname: 'CorpID', sortorder: "asc",

                SelectCommand: function (postData) { return { AdminGroupSelect: postData } },
                UpdateCommand: function (postData) { return { AdminGroupRowCommand: postData } },
                InsertCommand: function (postData) { postData.op_Insert = true; return { AdminGroupRowCommand: postData } },
                colModel: [
                    { name: 'Action    ', label: '<%=lang["colAction    "]%>', colType: 'Buttons' },
                    { name: 'ID        ', label: '<%=lang["colID        "]%>', colType: 'ID', width: 200, editable: false, hidden: true },
                    { name: 'CorpID    ', label: '<%=lang["colCorpID    "]%>', colType: 'CorpID', editonce: false },
                    { name: 'Sort      ', label: '<%=lang["colSort      "]%>', width: 50, sorttype: 'int', editable: true },
                    { name: 'Name      ', label: '<%=lang["colName      "]%>', sorttype: 'text', editable: true },
                    { name: 'Locked    ', label: '<%=lang["colLocked    "]%>', colType: 'Locked', search: false },
                    { name: 'CreateTime', label: '<%=lang["colCreateTime"]%>', colType: 'DateTime2' },
                    { name: 'CreateUser', label: '<%=lang["colCreateUser"]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime', label: '<%=lang["colModifyTime"]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser', label: '<%=lang["colModifyUser"]%>', colType: 'ACNT2' },
                ],
                subGridBeforeExpand: function (pID, id, ind) { },
                subGridRowCreated: function (pID, id, ind, tablediv) { },
                subGridRowExpanded: function (pID, id, ind, tablediv) { },
                subGridBeforeColapsed: function (pID, id, ind, tablediv) { },
                subGridRowRemoved: function (pID, id, ind, tablediv) { },
                subGridRowColapsed: function (pID, id, ind, tablediv) { }
            });

            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <table id="table01">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide"   action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow"   action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
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
</asp:Content>
