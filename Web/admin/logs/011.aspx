<%@ Page Title="" Language="C#" MasterPageFile="~/logs/logs.master" AutoEventWireup="true" Inherits="web.page" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function (ind, rowid) {

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: false, sortname: 'betTime',
                cmTemplate: { editable: false, editonce: false },
                SelectCommand: function (postData) { return { GameLogSelect_011: postData } },

                colModel: [
                        { name: 'ACTime         ', label: '<%=lang["colACTime           "]%>', width: 080, sorttype: 'date', formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd', formatNaN: 'N/A' } },
                        { name: 'GameID         ', label: '<%=lang["colGameID           "]%>', width: 080, sorttype: 'int', formatter: 'select', edittype: 'select', editoptions: { <%=serializeEnum("value", web.game.Names)%> }},
                        { name: 'CorpID         ', label: '<%=lang["colCorpID           "]%>', colType: 'CorpID' },
                        { name: 'MemberID       ', label: '<%=lang["colMemberID         "]%>', width: 080, sorttype: 'int', hidden: true },
                        { name: 'ACNT           ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text', search: true },
                        { name: 'ParentID       ', label: '<%=lang["colParentID         "]%>', width: 080, sorttype: 'int', hidden: true },
                        { name: 'ParentACNT     ', label: '<%=lang["colParentACNT       "]%>', width: 080, sorttype: 'text', search: true },
                        { name: 'billNo         ', label: '<%=lang["colbillNo           "]%>', width: 100, sorttype: 'text', search: true },
                        { name: 'playerName     ', label: '<%=lang["colplayerName       "]%>', width: 120, sorttype: 'text' },
                        { name: 'agentCode      ', label: '<%=lang["colagentCode        "]%>', width: 150, sorttype: 'text' },
                        { name: 'gameCode       ', label: '<%=lang["colgameCode         "]%>', width: 080, sorttype: 'text' },
                        { name: 'netAmount      ', label: '<%=lang["colnetAmount        "]%>', width: 080, sorttype: 'text' },
                        { name: 'betTime        ', label: '<%=lang["colbetTime          "]%>', width: 080, sorttype: 'text' },
                        { name: 'gameType       ', label: '<%=lang["colgameType         "]%>', width: 080, sorttype: 'text' },
                        { name: 'betAmount      ', label: '<%=lang["colbetAmount        "]%>', width: 120, sorttype: 'text' },
                        { name: 'validBetAmount ', label: '<%=lang["colvalidBetAmount   "]%>', colType: 'Money' },
                        { name: 'flag           ', label: '<%=lang["colflag             "]%>', sorttype: 'text' },
                        { name: 'playType       ', label: '<%=lang["colplayType         "]%>', sorttype: 'text' },
                        { name: 'currency       ', label: '<%=lang["colcurrency         "]%>', sorttype: 'text' },
                        { name: 'tableCode      ', label: '<%=lang["coltableCode        "]%>', width: 080, sorttype: 'text' },
                        { name: 'loginIP        ', label: '<%=lang["colloginIP          "]%>', width: 080, sorttype: 'text' },
                        { name: 'recalcuTime    ', label: '<%=lang["colrecalcuTime      "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'platformId     ', label: '<%=lang["colplatformId       "]%>', width: 200, sorttype: 'text' },
                        { name: 'platformType   ', label: '<%=lang["colplatform         "]%>', width: 60, sorttype: 'text' },
                        { name: 'stringex       ', label: '<%=lang["colstringex         "]%>', colType: 'text' },
                        { name: 'remark         ', label: '<%=lang["colremark           "]%>', colType: 'text' },
                        { name: 'round          ', label: '<%=lang["colround            "]%>', colType: 'text' },
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

