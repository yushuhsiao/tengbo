<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="web.page" %>

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
                pager: true, toppager: false, rowNum: 10000, pginput: false, rowList: [], shrinkToFit: true, loadonce: true, sortname: 'Category', sortorder: "asc",
                cmTemplate: { align: 'left' },
                SelectCommand: function (postData) { return { ConfigSelect: postData } },
                UpdateCommand: function (postData) { return { ConfigUpdate: postData } },
                InsertCommand: function (postData) { return { ConfigInsert: postData } },
                DeleteCommand: function (postData) { return { ConfigDelete: postData } },

                colModel: [
                    { name: 'Action     ', label: '<%=lang["colAction     "]%>', colType: 'Buttons' },
                    { name: 'CorpID     ', label: '<%=lang["colCorpID     "]%>', colType: 'CorpID', editable: true, editonce: false, edittype: 'select' },
                    { name: 'ID         ', label: '<%=lang["colID         "]%>', colType: 'ID' },
                    { name: 'Category   ', label: '<%=lang["colCategory   "]%>', width: 100, sorttype: 'text    ', editable: true },
                    { name: 'Key        ', label: '<%=lang["colKey        "]%>', width: 150, sorttype: 'text    ', editable: true },
                    { name: 'Value      ', label: '<%=lang["colValue      "]%>', width: 350, sorttype: 'text    ', editable: true },
                    { name: 'Description', label: '<%=lang["colDescription"]%>', width: 200, sorttype: 'text    ', editable: true },
                ]
            });
            // 工具列
            //$('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
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
        <tr class="grid-option">
            <td>
                <div name="nav1">
                    <button action="addRow"       icon="ui-icon-plus"   ><%=lang["btnAdd"]%></button>
                </div>
                <div name="nav2" class="ui-widget-content" style=""></div>
            </td>
        </tr>
    </table>
</asp:Content>