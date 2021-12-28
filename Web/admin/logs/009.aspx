<%@ Page Title="" Language="C#" MasterPageFile="~/logs/logs.master" AutoEventWireup="true" Inherits="web.page" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function (ind, rowid) {
            <%
                Dictionary<int, string> dict = new Dictionary<int, string>();
                foreach (object o in Enum.GetValues(typeof(extAPI.bbin.gametype)))
                {
                    int gameType_i = (int)o;
                    string key;
                    if (gameType_i < 0x10000)
                        key = gameType_i.ToString();
                    else
                        key = string.Format("0x{0:X8}", gameType_i);
                    dict[gameType_i] = web.Lang.GetLang("bbin_GameType", key) ?? key;// lang["bbin_GameType", "_" + key] ?? key;
                }
            %>

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: false, sortname: 'WagersDate',
                cmTemplate: { editable: false, editonce: false },
                SelectCommand: function (postData) { return { GameLogSelect_009: postData } },
                colModel: [
                        { name: 'ACTime         ', label: '<%=lang["colACTime           "]%>', width: 080, sorttype: 'date', formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd', formatNaN: 'N/A' } },
                        { name: 'CorpID         ', label: '<%=lang["colCorpID           "]%>', colType: 'CorpID' },
                        { name: 'ParentID       ', label: '<%=lang["colParentID         "]%>', width: 080, sorttype: 'int', hidden: true },
                        { name: 'ParentACNT     ', label: '<%=lang["colParentACNT       "]%>', width: 080, sorttype: 'text', search: true },
                        { name: 'MemberID       ', label: '<%=lang["colMemberID         "]%>', width: 080, sorttype: 'int', hidden: true },
                        { name: 'ACNT           ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text', search: true },
                        { name: 'UserName       ', label: '<%=lang["colUserName         "]%>', width: 100, sorttype: 'text', search: true },
                        { name: 'WagersDate     ', label: '<%=lang["colWagersDate       "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'WagersID       ', label: '<%=lang["colWagersID         "]%>', width: 150, sorttype: 'int' },
                        { name: 'SerialID       ', label: '<%=lang["colSerialID         "]%>', width: 080, sorttype: 'text' },
                        { name: 'RoundNo        ', label: '<%=lang["colRoundNo          "]%>', width: 080, sorttype: 'text' },
                        { name: 'GameTypei      ', label: '<%=lang["colGameType         "]%>', width: 080, sorttype: 'text', formatter: 'select', editoptions: { <%=serializeEnum ("value", dict)%> } },
                        { name: 'GameType       ', label: '<%=lang["colGameType         "]%>', width: 080, sorttype: 'text', hidden: true },
                        { name: 'GameCode       ', label: '<%=lang["colGameCode         "]%>', width: 080, sorttype: 'text' },
                        { name: 'Result         ', label: '<%=lang["colResult           "]%>', width: 120, sorttype: 'text' },
                        { name: 'BetAmount      ', label: '<%=lang["colBetAmount        "]%>', colType: 'Money' },
                        { name: 'Commissionable ', label: '<%=lang["colCommissionable   "]%>', colType: 'Money' },
                        { name: 'Payoff         ', label: '<%=lang["colPayoff           "]%>', colType: 'Money' },
                        { name: 'Commission     ', label: '<%=lang["colCommission       "]%>', colType: 'Money' },
                        { name: 'IsPaid         ', label: '<%=lang["colIsPaid           "]%>', width: 080, sorttype: 'text' },
                        { name: 'gamekind       ', label: '<%=lang["colgamekind         "]%>', width: 080, sorttype: 'int', formatter: 'select', editoptions: { <%=enumlist<extAPI.bbin.gamekind>("value")%> } },
                        { name: 'ResultType     ', label: '<%=lang["colResultType       "]%>', width: 080, sorttype: 'text' },
                        { name: 'Card           ', label: '<%=lang["colCard             "]%>', width: 200, sorttype: 'text' },
                        { name: 'Currency       ', label: '<%=lang["colCurrency         "]%>', width: 60, sorttype: 'text' },
                        { name: 'ExchangeRate   ', label: '<%=lang["colExchangeRate     "]%>', colType: 'Money' },
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

