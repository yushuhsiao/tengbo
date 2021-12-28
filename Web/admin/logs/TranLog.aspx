<%@ Page Title="" Language="C#" MasterPageFile="~/logs/logs.master" AutoEventWireup="true" Inherits="web.page" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function (ind, rowid) {

            var enum2 = {
                <%=enumlist<BU.LogType>("logTypes")%> 
                <%=serializeEnum(",gameid", web.game.Names)%>
                <%=serializeEnum(",gameid_s", web.game.Names_Active)%>
                <%=enumlist<BU.UserType>(",userTypes", true, BU.UserType.Agent, BU.UserType.Member)%>
            };
            enum2.gameid[0] = enum2.gameid_s[0] = '<%=lang["MainAccount"]%>';

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: false,
                cmTemplate: { editable: false, editonce: false },
                SelectCommand: function (postData) { return { TranLogSelect: postData } },
                
                colModel: [
                    { name: 'sn             ', label: '<%=lang["colSN           "]%>', colType: 'ID'<%if (showID) { %>, hidden: false<% }%> },
                    { name: 'ParentID       ', label: '<%=lang["colParentID     "]%>', width: 050, search: true<%if (!showID) { %>, hidden: true<% }%> },
                    { name: 'UserID         ', label: '<%=lang["colUserID       "]%>', width: 050, search: true<%if (!showID) { %>, hidden: true<% }%> },
                    { name: 'ACTime         ', label: '<%=lang["colACTime       "]%>', colType: 'ACTime' },
                    { name: 'CreateTime     ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', nowrap: true },
                    { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                    { name: 'GameID         ', label: '<%=lang["colGameID       "]%>', width: 080, formatter: 'select', edittype: 'select', editoptions: { value: enum2.gameid }, search: true, stype: 'select', searchoptions: { value: enum2.gameid_s, defaultValue: 255, nullKey: 255, nullValue: '--', } },
                    { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 120, formatter: 'select', editoptions: { value: enum2.logTypes }, search: true, stype: 'select', searchoptions: { value: enum2.logTypes, defaultValue: 255, nullKey: 255, nullValue: '--' } },
                    { name: 'LogTypeText    ', label: '<%=lang["colLogTypeText  "]%>', width: 100 },
                    { name: 'ParentACNT     ', label: '<%=lang["colParentACNT   "]%>', width: 080, search: true },
                    { name: 'UserType       ', label: '<%=lang["colUserType     "]%>', width: 075, editable: true, editonce: true, hidden: false, formatter: 'select', edittype: 'select', editoptions: { value: enum2.userTypes }, search: true, stype: 'select', searchoptions: { value: enum2.userTypes, defaultValue: 255, nullKey: 255, nullValue: '--' } },
                    { name: 'IsProvider     ', label: '<%=lang["colIsProvider   "]%>', width: 015, hidden: true, formatter: 'select', editoptions: { value: { 0: ' ', 1: '*' } }, search: true, stype: 'select', searchoptions: { value: { 0: ' ', 1: '*' }, defaultValue: 255, nullKey: 255, nullValue: '--' } },
                    { name: 'UserACNT       ', label: '<%=lang["colUserACNT     "]%>', width: 080, formatter: function (cellval, opts, rwd, act) { return (rwd.IsProvider == 1 ? "* " : '') + rwd.UserACNT; }, search: true },
                  //{ name: 'srcParentID    ', label: '<%=lang["colsrcParentID  "]%>', hidden: true, width: 050, search: true },
                  //{ name: 'srcUserID      ', label: '<%=lang["colsrcUserID    "]%>', width: 050, search: true<%if (!showID) { %>, hidden: true<% }%> },
                  //{ name: 'srcParentACNT  ', label: '<%=lang["colsrcParentACNT"]%>', hidden: true, width: 080, search: true },
                  //{ name: 'srcUserACNT    ', label: '<%=lang["colsrcUserACNT  "]%>', width: 080, search: true },
                  //{ name: 'srcUserType    ', label: '<%=lang["colsrcUserType  "]%>', width: 075, editable: true, editonce: true, hidden: false, formatter: 'select', edittype: 'select', editoptions: { value: enum2.userTypes }, search: true, stype: 'select', searchoptions: { value: enum2.userTypes, defaultValue: 255, nullKey: 255, nullValue: '--' } },
                    { name: 'PrevBalance    ', label: '<%=lang["colPrevBalance  "]%>', colType: 'Money' },
                    { name: 'Amount         ', label: '<%=lang["colAmount       "]%>', colType: 'Money' },
                    { name: 'Balance        ', label: '<%=lang["colBalance      "]%>', colType: 'Money' },
                    { name: 'CashAmount     ', label: '<%=lang["colCashAmount   "]%>', width: 70, colType: 'Money2' },
                    { name: 'CashPCT        ', label: '<%=lang["colCashPCT      "]%>', width: 60, colType: 'Percent' },
                    { name: 'CashFees       ', label: '<%=lang["colCashFees     "]%>', width: 70, colType: 'Money2' },
                    { name: 'BetAmount      ', label: '<%=lang["colBetAmount    "]%>', width: 70, colType: 'Money2' },
                    { name: 'BetBonus       ', label: '<%=lang["colBetBonus     "]%>', width: 60, colType: 'Percent' },
                    { name: 'BetPayout      ', label: '<%=lang["colBetPayout    "]%>', width: 70, colType: 'Money2' },
                    { name: 'BetShare       ', label: '<%=lang["colBetShare     "]%>', width: 60, colType: 'Percent' },
                    { name: 'CurrencyA      ', label: '<%=lang["colCurrencyA    "]%>', colType: 'Currency', hidden: true },
                    { name: 'CurrencyB      ', label: '<%=lang["colCurrencyB    "]%>', colType: 'Currency', hidden: true },
                    { name: 'CurrencyX      ', label: '<%=lang["colCurrencyX    "]%>', colType: 'Money', hidden: true },
                    { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, fixed: true, search: true },
                    { name: 'RequestTime    ', label: '<%=lang["colRequestTime  "]%>', colType: 'DateTime2', nowrap: true },
                    { name: 'FinishTime     ', label: '<%=lang["colFinishTime   "]%>', colType: 'DateTime2', nowrap: true },
                    { name: 'RequestIP      ', label: '<%=lang["colRequestIP    "]%>', width: 120 },
                    { name: 'TranID         ', label: '<%=lang["colTranID       "]%>', width: 280, fixed: true },
                    { name: 'CashChannelID  ', label: '<%=lang["colCashChannelID"]%>', width: 280, fixed: true },
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

