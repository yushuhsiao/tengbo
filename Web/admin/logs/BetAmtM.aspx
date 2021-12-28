<%@ Page Title="" Language="C#" MasterPageFile="~/logs/logs.master" AutoEventWireup="true" Inherits="web.page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function (ind, rowid) {

            $table = $('#table1').jqGrid_init({                
                pager: true, loadonce: false, sortname: 'ACMonth',                
                cmTemplate: { editable: false, editonce: false },
                SelectCommand: function (postData) { return { GameLog_BetAmtM_Select: postData } },

                colModel: [
                    { name: 'ACMonth        ', label: '<%=lang["colACMonth      "]%>', width: 080, sorttype: 'date', formatter: 'datejs', formatoptions: { format: 'yyyy-MM', formatNaN: 'N/A' } },
                    { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                    { name: 'GroupID        ', label: '<%=lang["colGroupID      "]%>', width: 080, formatter: 'select', formatoptions: {<%=serializeEnum<long,string>("value", web.MemberGroupRow.Cache.Instance.Value2)%> }, edittype: 'select', editoptions: { value_func: function (rowdata) { return <%=web.api.SerializeObject(web.MemberGroupRow.Cache.Instance.Value1)%>[rowdata.CorpID] || {}; } } },
                    { name: 'ParentID       ', label: '<%=lang["colParentID     "]%>', width: 080, sorttype: 'int ', hidden: true },
                    { name: 'ParentACNT     ', label: '<%=lang["colParentACNT   "]%>', width: 080, sorttype: 'text', search: true },
                    { name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 080, sorttype: 'int ', hidden: true },
                    { name: 'ACNT           ', label: '<%=lang["colACNT         "]%>', width: 080, sorttype: 'text', search: true },
                    { name: 'Name           ', label: '<%=lang["colName         "]%>', width: 080, sorttype: 'text', search: true },
                    { name: 'BetAmount      ', label: '<%=lang["colBetAmount    "]%>', width: 120, colType: 'Money' },
                    { name: 'BetAmountAct   ', label: '<%=lang["colBetAmountAct "]%>', width: 120, colType: 'Money' },
                    { name: 'Payout         ', label: '<%=lang["colPayout       "]%>', width: 120, colType: 'Money' }
                ]
            });
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <table id="table1">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide"   action="editRow"    icon="ui-icon-pencil" disabled="disabled"><%=lang["Actions_Edit"]%></div>
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
                    <button action="toggleSearch" icon="ui-icon-search" ><%=lang["btnSearch"]%></button>
                    <button action="reloadGrid"   icon="ui-icon-refresh"><%=lang["btnRefresh"]%></button>
                </div>
                <div name="nav2" class="ui-widget-content" style=""></div>
            </td>
        </tr>
    </table>
</asp:Content>

