<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" culture="auto" uiculture="auto" CodeBehind="CodeEdit.aspx.cs" Inherits="page" meta:resourcekey="PageResource1" %>

<script runat="server">

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //}
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function () {
            $table = $('#table01').jqGrid_init({
                pager: false, autowidth: false, shrinkToFit: false, loadonce: true, subGrid: false,
                nav1: '#nav1', nav2: '#nav2', msglist: '.msglist',
                SelectCommand: function (postData) { return { CodeSelect: postData } },
                UpdateCommand: function (postData) { return { CodeUpdate: postData } },
                InsertCommand: function (postData) { return { CodeInsert: postData } },
                DeleteCommand: function (postData) { return { CodeDelete: postData } },
                colModel: [
                    { name: 'Action     ', label: '<%=lang["colAction     "]%>', colType: 'Buttons' },
                    { name: 'ID         ', label: '<%=lang["colID         "]%>', colType: 'ID', hidden: false },
                    { name: 'Code       ', label: '<%=lang["colCode       "]%>', width: 200, sorttype: 'text', editable: true },
                    { name: 'Description', label: '<%=lang["colDescription"]%>', width: 200, sorttype: 'text', editable: true }
                ]
            });
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div id="nav1">
        <button id="btnAdd"><%=lang["btnAdd"]%></button>
    </div>
    <table id="table01">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide"   action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="edithide"   action="delRow"     icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="saveRow"    icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
    </table>
    <div id="nav2" class="ui-widget-content" style=""><table class="msglist"></table></div>
</asp:Content>