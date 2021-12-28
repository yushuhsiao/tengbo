<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="Corp.aspx.cs" Inherits="web.page" %>

<script runat="server">
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var $table;

        $(document).ready(function () {
            $table = $('#table01').jqGrid_init({
                pager: true, loadonce: true, subGrid: false,
                SelectCommand: function (postData) { return { CorpSelect: postData } },
                UpdateCommand: function (postData) { return { CorpUpdate: postData } },
                InsertCommand: function (postData) { return { CorpInsert: postData } },

                colModel: [
                    { name: 'Action             ', label: '<%=lang["colAction           "]%>', colType: 'Buttons' },
                    { name: 'ID                 ', label: '<%=lang["colID               "]%>', colType: 'ID', width: 080, hidden: false, editable: true, editonce: true },
                  //{ name: 'newID              ', label: '<%=lang["colID               "]%>', width: 080, sorttype: 'int     ', editable: 'false ', formatter: { 'alias': 'ID' } },
                    { name: 'ACNT               ', label: '<%=lang["colCorpACNT         "]%>', colType: 'ACNT' },
                    { name: 'Name               ', label: '<%=lang["colName             "]%>', width: 080, sorttype: 'text', editable: true, editonce: false },
                    { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked' },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'AdminACNT          ', label: '<%=lang["colAdminACNT        "]%>', colType: 'ACNT' },
                    { name: 'AgentACNT          ', label: '<%=lang["colAgentACNT        "]%>', colType: 'ACNT' },
                    { name: 'CreateTime         ', label: '<%=lang["colCreateTime       "]%>', colType: 'DateTime2' },
                    { name: 'CreateUser         ', label: '<%=lang["colCreateUser       "]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime         ', label: '<%=lang["colModifyTime       "]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser         ', label: '<%=lang["colModifyUser       "]%>', colType: 'ACNT2' },
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