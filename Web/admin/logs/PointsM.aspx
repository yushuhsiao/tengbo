<%@ Page Title="" Language="C#" MasterPageFile="~/logs/logs.master" AutoEventWireup="true" Inherits="web.page" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function (ind, rowid) {

            var enum2 = {
                <%=enumlist<BU.LogType>("logTypes")%> 
                <%=serializeEnum(",gameid", web.game.Names)%>
                <%=serializeEnum(",gameid_s", web.game.Names_Active)%>
            };
            enum2.gameid[0] = enum2.gameid_s[0] = '<%=lang["MainAccount"]%>';

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: false, sortname: 'ACMonth',
                cmTemplate: { editable: false, editonce: false },
                SelectCommand: function (postData) { return { GameLog_PointsMSelect: postData } },

                colModel: [
                    { name: 'ACMonth        ', label: '<%=lang["colACMonth      "]%>', width: 080, sorttype: 'date', formatter: 'datejs', formatoptions: { format: 'yyyy-MM', formatNaN: 'N/A' } },
                    { name: 'GameID         ', label: '<%=lang["colGameID       "]%>', width: 080, sorttype: 'int', formatter: 'select', edittype: 'select', editoptions: { value: enum2.gameid }, search: true, stype: 'select', searchoptions: { value: enum2.gameid_s, defaultValue: 255, nullKey: 255, nullValue: '--', } },
                    { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 120, sorttype: 'text', formatter: 'select', editoptions: { value: enum2.logTypes }, search: true, stype: 'select', searchoptions: { value: enum2.logTypes, defaultValue: 255, nullKey: 255, nullValue: '--', } },
                  //{ name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 050, sorttype: 'int' },
                    { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                    { name: 'ParentACNT     ', label: '<%=lang["colParentACNT   "]%>', width: 080, search: true },
                    { name: 'ACNT           ', label: '<%=lang["colACNT         "]%>', width: 080, search: true },
                    { name: 'Amount         ', label: '<%=lang["colAmount       "]%>', colType: 'Money' },
                    { name: 'CurrencyA      ', label: '<%=lang["colCurrencyA    "]%>', colType: 'Currency', hidden: true },
                    { name: 'CurrencyB      ', label: '<%=lang["colCurrencyB    "]%>', colType: 'Currency', hidden: true },
                    { name: 'CurrencyX      ', label: '<%=lang["colCurrencyX    "]%>', colType: 'Money', hidden: true },
                ],
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

