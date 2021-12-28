<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="Billboard.aspx.cs" Inherits="web.page" %>

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

            $table = $('#table1').jqGrid_init({
                pager: true, toppager: false,
                SelectCommand: function (postData) { return { BillboardSelect: postData } },
                UpdateCommand: function (postData) { return { BillboardUpdate: postData } },
                InsertCommand: function (postData) { return { BillboardInsert: postData } },

                colModel: [
                    { name: 'Action    ', label: '<%=lang["colAction    "]%>', colType: 'Buttons' },
                    { name: 'ID        ', label: '<%=lang["colID        "]%>', colType: 'ID' },
                    { name: 'CorpID    ', label: '<%=lang["colCorpID    "]%>', colType: 'CorpID', editable: true },
                    { name: 'Locked    ', label: '<%=lang["colLocked    "]%>', colType: 'Locked' },
                    { name: 'RecordType', label: '<%=lang["colRecordType"]%>', width: 120, sorttype: 'int     ', editable: true, formatter: 'select', edittype: 'select', editoptions: {<%=enumlist<BU.BillboardRecordType>("value")%> } },
                    { name: 'Place     ', label: '<%=lang["colPlace     "]%>', width: 050, sorttype: 'int     ', editable: true, formatter: 'select', edittype: 'select', editoptions: { value: { 1: "1", 2: "2", 3: "3", 4: "4", 5: "5", 6: "6", 7: "7", 8: "8", 9: "9", 10: "10" } } },
                    { name: 'MemberACNT', label: '<%=lang["colMemberACNT"]%>', width: 100, sorttype: 'text    ', editable: true },
                    { name: 'Amount    ', label: '<%=lang["colAmount    "]%>', width: 080, sorttype: 'currency', editable: true, formatter: 'currency' },
                    { name: 'CreateTime', label: '<%=lang["colCreateTime"]%>', colType: 'DateTime2' },
                    { name: 'CreateUser', label: '<%=lang["colCreateUser"]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime', label: '<%=lang["colModifyTime"]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser', label: '<%=lang["colModifyUser"]%>', colType: 'ACNT2' },
                ],
            });
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });

        //var resize = function () {
        //    $table[0].grid.totalWidth($(window).innerWidth());
        //    $table[0].grid.totalHeight($(window).innerHeight());
        //};
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <table id="table1">
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