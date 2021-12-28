<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="web.page" %>
<%@ Import Namespace="web" %>

<script runat="server">
    MenuRow menu_a;
    MenuRow menu_m;
    protected void Page_Load(object sender, EventArgs e)
    {
        MenuRow.Cache m = MenuRow.Cache.Instance;
        menu_a = m.GetItem(this.User, BU.Permissions.Code.agents_list);
        menu_m = m.GetItem(this.User, BU.Permissions.Code.members_list);
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

        var $table = null;
        var parentID = null;
        var recvMessage = {
            AgentTree_Select: function (data) {
                if (parentID != data) {
                    parentID = data;
                    if ($table) {
                        $table.clearGridData();
                        $table.reloadGrid();
                    }
                }
            },
            AgentRowData: function (data) { $table.restoreRow(data.ID); $table.setRowData(data.ID, data); }
        }
        function onPostData(command, postData) {
            postData.ParentID = parentID;
            var cmd = {};
            cmd[command] = postData;
            return cmd;
        }

        $(document).ready(function () {
            sendMessage('AgentTree_GetSelect');
            $table = $('#table01').jqGrid_init({
                pager: true, subGrid: true, detail_root: '.details-a-root',
                colModel: [
                    { name: 'Action       ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
                    { name: 'ID           ', label: '<%=lang["colID           "]%>', colType: 'ID'<%if (showID) { %>, hidden: false<% }%>, search: false },
                    { name: 'CorpID       ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                    { name: 'ParentACNT   ', label: '<%=lang["colParentACNT   "]%>', colType: 'ACNT', search: true },
                    { name: 'ACNT         ', label: '<%=lang["colACNT         "]%>', colType: 'ACNT', width: 120, search: true },
                    { name: 'Name         ', label: '<%=lang["colName         "]%>', width: 080, search: true, editable: true },
                    { name: 'GroupID      ', label: '<%=lang["colGroupID      "]%>', width: 080, editable: true, editable_func: function (rwd, new_row) { return !new_row; }, formatter: 'select', formatoptions: {<%=serializeEnum<Guid,string>("value", web.AgentGroupRow.Cache.Instance.Value2)%> }, edittype: 'select', editoptions: { value_func: function (rowdata) { return <%=web.api.SerializeObject(web.AgentGroupRow.Cache.Instance.Value1)%>[rowdata.CorpID] || {}; } } },
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
                    { name: 'ModifyUser   ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' },
                    { label: '.', width: 1500 }
                ],
                SelectCommand: function (postData) { return onPostData('AgentSelect', postData); },
                UpdateCommand: function (postData) { return onPostData('AgentUpdate', postData); },
                InsertCommand: function (postData) { return onPostData('AgentInsert', postData); },
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
                //loadBeforeSend: function (xhr, settings) {
                //    if ($table == null) sendMessage('AgentTree_GetSelect');
                //    return true;
                //}
                //RowResponse: function (res, rowid, o) {
                //    try {
                //        var trow = $table_t.getRowData(o.ID);
                //        if (trow.ID == null) {
                //            o.isLeaf = false;
                //            o.expanded = false;
                //            o.loaded = false;
                //            $table_t.addChildNode(o.ID, o.ParentID, o);
                //        }
                //    } catch (e) { }
                //}
            });

            $table.gridContainer().removeClass('ui-corner-all');

            // 工具列
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
   </script>
    <style type="text/css">
        .detail-content-loading div { background: url(../images/loading3_000000.gif) #fff no-repeat center center; width: 32px; height: 32px; margin: 1px; border-width: 1px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="tmp" style="display:none">
        <div class="ui-widget-content ui-state-active detail-content-loading" style="display: inline-block;"><div></div></div>
        <iframe frameBorder="0" style="display: none; width:98%; height:1px; border: 0;"></iframe>
    </div>

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
                <div name="nav1" style="padding-left: 10px;">
                    <button action="addRow"         icon="ui-icon-plus"    disabled="true"><%=lang["btnAdd"]+lang["btnAgent"]%></button>
                    <% if (menu_a != null) { %><a action="link" icon="ui-icon-comment" disabled="true"                         ><%=menu_a.GetLabel()%></a><% } %>
                    <% if (menu_m != null) { %><a action="link" icon="ui-icon-comment" href="<%=ResolveClientUrl(menu_m.Url)%>"><%=menu_m.GetLabel()%></a><% } %>
                    <button action="toggleSearch"   icon="ui-icon-search"                 ><%=lang["btnSearch"]%></button>
                    <button action="reloadGrid"     icon="ui-icon-refresh"                ><%=lang["btnRefresh"]%></button>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

