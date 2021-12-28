<%@ Page Title="" Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" Inherits="web.page" %>

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
                pager: false, toppager: false, pginput: false, rowList: [], shrinkToFit: false, loadonce: true,
                sortname: '_Key', sortorder: "asc",
                cmTemplate: { align: 'left' },
                SelectCommand: function (postData) { return { ConfigSelect: postData } },
                UpdateCommand: function (postData) { return { ConfigUpdate: postData } },
                InsertCommand: function (postData) { return { ConfigInsert: postData } },

                colModel: [
                    { name: 'Action  ', label: '.      ', colType: 'Buttons' },
                    { name: '_Key    ', label: 'Key    ', width: 100, colType: 'ID', hidden: false },
                    { name: '_Active ', label: 'Active ', width: 080, colType: 'Locked' },
                    { name: '_Time   ', label: 'Time   ', colType: 'DateTime', editable: true },
                    { name: '_Start  ', label: 'Start  ', width: 100, sorttype: 'text', editable: true },
                    { name: '_End    ', label: 'End    ', width: 100, sorttype: 'text', editable: true },
                ],
            });
            // 工具列
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
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
                    <button id="btnAdd"><%=lang["btnAdd"]%></button>
                </div>
                <div name="nav2" class="ui-widget-content" style=""></div>
            </td>
        </tr>
    </table>
</asp:Content>