<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="web.page" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

        var $table;

        $(document).ready(function () {
            $table = $('#table01').jqGrid_init({
                pager: true, subGrid: true, detail_root: '.details-m-root',
                width: '50%',
                SelectCommand: function (postData) { return { AdminSelect: postData } },
                UpdateCommand: function (postData) { return { AdminUpdate: postData } },
                InsertCommand: function (postData) { return { AdminInsert: postData } },
                subGridBeforeExpand: function (pID, id, ind) { },
                subGridRowCreated: function (pID, id, ind, tablediv) {
                    $('.tmp').clone().children().appendTo(tablediv);
                    $('iframe', tablediv).load(function () {
                        $(this).show();
                        $('.detail-content-loading', tablediv).hide();
                    }).prop('src', 'AdminDetails.aspx?id=' + id);
                },
                subGridRowExpanded: function (pID, id, ind, tablediv) { },
                subGridBeforeColapsed: function (pID, id, ind, tablediv) { },
                subGridRowRemoved: function (pID, id, ind, tablediv) { },
                subGridRowColapsed: function (pID, id, ind, tablediv) { },

                colModel: [
                    { name: 'Action    ', label: '<%=lang["colAction    "]%>', colType: 'Buttons', action: { editcode: function (ind, rowid) { $table.expandSubGridRow(rowid); } } },
                    { name: 'ID        ', label: '<%=lang["colID        "]%>', colType: 'ID'<%if (showID) { %>, hidden: false<% }%> },
                    { name: 'CorpID    ', label: '<%=lang["colCorpID    "]%>', colType: 'CorpID' },
                    { name: 'ACNT      ', label: '<%=lang["colACNT      "]%>', colType: 'ACNT', search: true },
                    { name: 'Name      ', label: '<%=lang["colName      "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'UserLevel ', label: '<%=lang["colUserLevel "]%>', width: 080, sorttype: 'int', editable: true, search: true },
                    { name: 'GroupID   ', label: '<%=lang["colGroupID   "]%>', width: 080, sorttype: 'int', editable: true, editonce: false, formatter: 'select', formatoptions: {<%=serializeEnum<Guid,string>("value", web.AdminGroupRow.Cache.Instance.Value2)%> }, edittype: 'select', editoptions: { value_func: function (rowdata) { return <%=web.api.SerializeObject(web.AdminGroupRow.Cache.Instance.Value1)%>[rowdata.CorpID] || {}; } } },
                    { name: 'Locked    ', label: '<%=lang["colLocked    "]%>', colType: 'Locked' },
                    { name: 'CreateTime', label: '<%=lang["colCreateTime"]%>', colType: 'DateTime2' },
                    { name: 'CreateUser', label: '<%=lang["colCreateUser"]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime', label: '<%=lang["colModifyTime"]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser', label: '<%=lang["colModifyUser"]%>', colType: 'ACNT2' },
                    { label: '.', width: 1500 }
                ]
            });

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

