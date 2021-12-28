<%@ Page Title="" Language="C#" MasterPageFile="~/logs/logs.master" AutoEventWireup="true" Inherits="web.page" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function (ind, rowid) {

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: false, sortname: 'BetStartTime',                
                cmTemplate: { editable: false, editonce: false },
                SelectCommand: function (postData) { return { GameLogSelect_001: postData } },

                colModel: [
                        { name: 'ACTime         ', label: '<%=lang["colACTime       "]%>', width: 080, sorttype: 'date', formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd', formatNaN: 'N/A' } },
                        { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                        { name: 'ParentID       ', label: '<%=lang["colParentID     "]%>', width: 080, sorttype: 'int', hidden: true },
                        { name: 'ParentACNT     ', label: '<%=lang["colParentACNT   "]%>', width: 080, sorttype: 'text', search: true },
                        { name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 080, sorttype: 'int', hidden: true },
                        { name: 'ACNT           ', label: '<%=lang["colACNT         "]%>', width: 080, sorttype: 'text', search: true },
                        { name: 'AccountId      ', label: '<%=lang["colAccountId    "]%>', width: 100, sorttype: 'text', search: true },
                        { name: 'UserID         ', label: '<%=lang["colUserID       "]%>', width: 080, sorttype: 'text', hidden: true },
                        { name: 'GameType       ', label: '<%=lang["colGameType     "]%>', width: 080, sorttype: 'text' },
                        { name: 'TableId        ', label: '<%=lang["colTableId      "]%>', width: 120, sorttype: 'text', hidden: true },
                        { name: 'TableName      ', label: '<%=lang["colTableName    "]%>', width: 120, sorttype: 'text' },
                        { name: 'GameId         ', label: '<%=lang["colGameId       "]%>', width: 180, sorttype: 'text', hidden: true },
                        { name: 'BetId          ', label: '<%=lang["colBetId        "]%>', width: 250, sorttype: 'text', hidden: true },
                        { name: 'TableId        ', label: '<%=lang["colTableId      "]%>', width: 120, sorttype: 'text', hidden: true },
                        { name: 'BetStartTime   ', label: '<%=lang["colBetStartTime "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'BetEndTime     ', label: '<%=lang["colBetEndTime   "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'BetAmount      ', label: '<%=lang["colBetAmount    "]%>', colType: 'Money' },
                        { name: 'BetAmountAct   ', label: '<%=lang["colBetAmountAct "]%>', colType: 'Money' },
                        { name: 'Payout         ', label: '<%=lang["colPayout       "]%>', colType: 'Money' },
                        { name: 'BetSpot        ', label: '<%=lang["colBetSpot      "]%>', width: 080, sorttype: 'text' },
                        { name: 'BetNo          ', label: '<%=lang["colBetNo        "]%>', width: 250, sorttype: 'text', hidden: true },
                        { name: 'Currency       ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency' },
                ]
            });

            $table[0].grid.$toolbar.css('height', 'auto');
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });

    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="Server">
    <table id="table1">
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

