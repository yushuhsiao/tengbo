<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="BankCard.aspx.cs" Inherits="web.page" %>

<script runat="server">
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //}
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
    </style>
    <script type="text/javascript">
        var $table;

        $(document).ready(function () {
            //var banklist = {};

            $table = $('#table01').jqGrid_init({
                pager: true, toppager: false, shrinkToFit: false, loadonce: false, sortname: 'BankName', sortorder: "desc",
                SelectCommand: function (postData) { return { BankCardSelect: postData } },
                UpdateCommand: function (postData) { return { BankCardUpdate: postData } },
                InsertCommand: function (postData) { return { BankCardInsert: postData } },

                colModel: [
                    { name: 'Action    ', label: '<%=lang["colAction    "]%>', colType: 'Buttons' },
                    { name: 'ID        ', label: '<%=lang["colID        "]%>', colType: 'ID' },
                    { name: 'CorpID    ', label: '<%=lang["colCorpID    "]%>', colType: 'CorpID', editonce: false },
                    { name: 'GroupID   ', label: '<%=lang["colGroupID   "]%>', width: 080, sorttype: 'int', editable: true, editonce: false, formatter: 'select', formatoptions: {<%=serializeEnum<long,string>("value", web.MemberGroupRow.Cache.Instance.Value2)%> }, edittype: 'select', editoptions: { value_func: function (rowdata) { return <%=web.api.SerializeObject(web.MemberGroupRow.Cache.Instance.Value1)%>[rowdata.CorpID] || {}; } } },
                    { name: 'LogType   ', label: '<%=lang["colLogType   "]%>', width: 060, sorttype: 'text', editable: true, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<BU.LogType>("value", true, BU.LogType.Deposit, BU.LogType.Withdrawal)%> } },
                    { name: 'Locked    ', label: '<%=lang["colLocked    "]%>', colType: 'Locked' },
                    { name: 'CardID    ', label: '<%=lang["colCardID    "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'BankName  ', label: '<%=lang["colBankName  "]%>', width: 080, sorttype: 'text', editable: true },//sorttype: 'int     ', editable: true, formatter: 'select', edittype: 'select', editoptions: { value_func: function () { return banklist; } } },
                    { name: 'AccName   ', label: '<%=lang["colName      "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Loc1      ', label: '<%=lang["colLoc1      "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Loc2      ', label: '<%=lang["colLoc2      "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Loc3      ', label: '<%=lang["colLoc3      "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Password  ', label: '<%=lang["colPassword  "]%>', width: 080, sorttype: 'text', editable: true, sortable: false },
                    { name: 'ExpireTime', label: '<%=lang["colExpireTime"]%>', width: 080, sorttype: 'date', editable: true, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd', formatNaN: 'N/A' } },
                    { name: 'CreateTime', label: '<%=lang["colCreateTime"]%>', colType: 'DateTime2' },
                    { name: 'CreateUser', label: '<%=lang["colCreateUser"]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime', label: '<%=lang["colModifyTime"]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser', label: '<%=lang["colModifyUser"]%>', colType: 'ACNT2' },
                ],
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
</asp:Content>

