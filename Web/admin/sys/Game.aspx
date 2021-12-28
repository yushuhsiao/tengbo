<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="Game.aspx.cs" Inherits="web.page" %>

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
                pager: true, toppager: false, pginput: false,
                rowList: [], shrinkToFit: true, loadonce: true, subGrid: false,
                SelectCommand: function (postData) { return { GameSelect: postData } },
                UpdateCommand: function (postData) { return { GameUpdate: postData } },
                //InsertCommand: function (postData) { return { GameInsert: postData } },

                colModel: [
                    { name: 'Action     ', label: '<%=lang["colAction     "]%>', colType: 'Buttons' },
                    { name: 'ID         ', label: '<%=lang["colID         "]%>', width: 100, colType: 'ID', hidden: false },
                    { name: 'Name       ', label: '<%=lang["colName       "]%>', width: 100, sorttype: 'text', editable: true },
                    { name: 'Locked     ', label: '<%=lang["colLocked     "]%>', width: 100, colType: 'Locked', editoptions: {<%=enumlist<BU.GameLocked>("value")%> } },
                  //{ name: 'BonusW     ', label: '<%=lang["colBonusW     "]%>', colType: 'Bonus' },
                  //{ name: 'BonusL     ', label: '<%=lang["colBonusL     "]%>', colType: 'Bonus' },
                    { name: 'CreateTime ', label: '<%=lang["colCreateTime "]%>', colType: 'DateTime2' },
                    { name: 'CreateUser ', label: '<%=lang["colCreateUser "]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime ', label: '<%=lang["colModifyTime "]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser ', label: '<%=lang["colModifyUser "]%>', colType: 'ACNT2' },
                ],
                subGridBeforeExpand: function (pID, id, ind) { },
                subGridRowCreated: function (pID, id, ind, tablediv) {
                    $(tablediv.parentElement).before('<td class="ui-widget-content subgrid-cell" style="vertical-align:top;"></td>');
                    tablediv.parentElement.colSpan--;
                    //var row = $table.getRowData(id);
                    var p = {
                        pager: false, autowidth: false, shrinkToFit: false, loadonce: true, subGrid: false, height: "100%", rownumbers: false,
                        //nav1 : '<label></label>', nav2 : $('<label><%=lang["btnAdd"]%></label>'),
                        addRowParams: { position: 'last' },
                        SelectCommand: function (postData) { postData.GameID = id; return { GameTypeSelect: postData } },
                        InsertCommand: function (postData) { postData.GameID = id; return { GameTypeInsert: postData } },
                        UpdateCommand: function (postData) { postData.GameID = id; return { GameTypeUpdate: postData } },
                        colModel: [
                            { name: 'Action     ', label: '<%=lang["colAction  "]%>', colType: 'Buttons' },
                            { name: 'newTypeCode', label: '<%=lang["colTypeCode"]%>', width: 150, editable: true, sortable: false, formatter: 'alias', formatoptions: 'TypeCode' },
                            { name: 'GameType   ', label: '<%=lang["colGameType"]%>', width: 150, editable: true, sortable: false },
                            { name: 'Name       ', label: '<%=lang["colName    "]%>', width: 150, editable: true, sortable: false },
                            { name: 'TypeCode   ', label: '<%=lang["colTypeCode"]%>', width: 150, editable: true, colType: 'ID' },
                        ]
                    };
                    var $table02 = $('.table02').clone(true, true).removeClass('table02').prop('id', pID).appendTo(tablediv).show().jqGrid_init(p).each(function () {
                        this.grid.$toolbar.hide();
                        this.grid.$bottomtoolbar.removeClass('ui-state-default');
                    });
                    p.nav2.button({ icons: { primary: 'ui-icon-plus' } }).click($table02[0].addRow).css('border', 0).removeClass('ui-state-default');
                    $table02.gridContainer().removeClass('ui-corner-all').css({ 'border-top-width': 0 });
                    $table02.gridContainer().find('.ui-th-column').removeClass('ui-state-default').addClass('ui-widget-content');
                },
                subGridRowExpanded: function (pID, id, ind, tablediv) { },
                subGridBeforeColapsed: function (pID, id, ind, tablediv) { },
                subGridRowRemoved: function (pID, id, ind, tablediv) { },
                subGridRowColapsed: function (pID, id, ind, tablediv) { }
            });

            // 工具列
            //$('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);

            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
   </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" Runat="Server">
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
        <%--<tr class="grid-option">
            <td>
                <div name="nav1"></div>
                <div name="nav2" class="ui-widget-content" style=""></div>
            </td>
        </tr>--%>
    </table>
    <table class="table02"">
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
                <label name="nav1"></label>
                <label name="nav2"><%=lang["btnAdd"]%></label>
            </td>
        </tr>
    </table>
</asp:Content>