<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="Anno.aspx.cs" Inherits="web.page" %>

<script runat="server">

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //}
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        /*.ui-jqgrid .ui-userdata { height : auto;	}*/
    </style>
    <script type="text/javascript">
        var $table;

        $(document).ready(function () {
            var colBankID;

            $table = $('#table01').jqGrid_init({
                pager: true, toppager: false, sortname: 'ModifyTime', sortorder: "desc",
                SelectCommand: function (postData) { return { AnnoSelect: postData } },
                UpdateCommand: function (postData) { return { AnnoUpdate: postData } },
                InsertCommand: function (postData) { return { AnnoInsert: postData } },

                colModel: [
                    { name: 'Action    ', label: '<%=lang["colAction    "]%>', colType: 'Buttons' },
                  //{ name: 'Action    ', label: '<%=lang["colAction    "]%>', colType: { 'Action   ': { edittext: '<%=lang["Actions_Edit"]%>', canceltext: '<%=lang["Actions_Cancel"]%>', savetext: '<%=lang["Actions_Save"]%>' } } },
                    { name: 'ID        ', label: '<%=lang["colID        "]%>', colType: 'ID' },
                    { name: 'CorpID    ', label: '<%=lang["colCorpID    "]%>', colType: 'CorpID', editable: true, editonce: false },
                    { name: 'Name      ', label: '<%=lang["colName      "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Locked    ', label: '<%=lang["colLocked    "]%>', colType: 'Locked' },
                    { name: 'Sort      ', label: '<%=lang["colSort      "]%>', width: 050, sorttype: 'int', editable: true },
                    { name: 'Text      ', label: '<%=lang["colText      "]%>', width: 300, sorttype: 'text', editable: true, edittype: 'textarea', editoptions: { rows: "3" } },
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
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
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

