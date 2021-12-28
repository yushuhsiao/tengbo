<%@ Page Language="C#" MasterPageFile="~/MasterPage/admin.master" AutoEventWireup="true" Inherits="page" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>
<script runat="server">

    int memberID;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.memberID = Request.QueryString["id"].ToInt32() ?? 0;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            function init_table(gid1, pin) {
                var $table = $('.template table')
                    .clone()
                    .appendTo(document.body)
                    .prop('id', gid1)
                    .jqGrid_init($.extend(true, {
                        cmTemplate: { sortable: false }, editParams: { url: '.api' },
                        cellEdit: false, cellEditOnClick: false, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false,
                        onSelectRow: function (rowid, status, e) {
                            var ind = $(this).getInd(rowid, true);
                            if (ind) {
                                $(ind).removeClass('ui-state-highlight');
                            }
                        },
                        UpdateCommand: function (postData) {
                            postData.MemberID = '<%=memberID%>';
                            postData.GameID = gid1;
                            return { MemberGameRow_Update: postData };
                        },
                        onCellSelect: function (rowid, iCol, cellcontent, e) {
                            var $t = $(this);
                            var cm = this.p.colModel[iCol];
                            var $cell = $($t.getCellObj(rowid, cm.name));
                            if ($cell.hasClass('gamebalance-load')) {
                                $cell.removeClass('ui-state-error');
                                if (!$cell.hasClass('gamebalance-loading')) {
                                    $cell.addClass('gamebalance-loading').empty();
                                    var $img = $('.template .load_balance').clone().appendTo($cell);
                                    $.invoke_api({ MemberGameSelect: { GameID: gid1, MemberID: '<%=memberID%>' } }, {
                                        success: function (data, textStatus, jqXHR) {
                                            if (data.Status == 1) {
                                                $t.setCell(rowid, cm.name, data.row[cm.name]);
                                            }
                                            else {
                                                $cell.addClass('ui-state-error');
                                                $t.setCell(rowid, cm.name, '{0} : {1}'.format(data.Status));
                                                addMsg('error', null, data.Message, data.Status);
                                            }
                                        },
                                        error: function (xhr, ajaxOptions, thrownError) {
                                            $t.setCell(rowid, cm.name, 'err');
                                            addMsg('error', null, xhr.statusText, xhr.status);
                                            console.log(xhr);
                                        },
                                        complete: function (jqXHR, textStatus) {
                                            $cell.removeClass('gamebalance-loading');
                                        }
                                    });
                                }
                            }
                        }
                    }, pin))
                    .editCell(1, 0, false)
                    .closest('tr').removeClass('hide');
                return $table;
            }
            //function fmt_balance(cellval, opts, rwd, act) {
            //    console.log(arguments);
            //    return cellval;
            //    return '<span>{0}</span>'.format(cellval);
            //}
            <%
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            if (memberID == 0) return;
            foreach (GameRow game in GameRow.Cache.GetInstance(null, sqlcmd).Rows)
            {
                MemberGame rowcmd = MemberGame.GetInstance(game.ID);
                if (rowcmd == null) continue;
                MemberGameRow row = rowcmd.SelectRow(sqlcmd, memberID, false);
                string data = api.SerializeObject(row ?? Tools._null<object>.value);

                if (game.ID == GameID.HG)
                { %>
            init_table('<%=(int?)game.ID%>', {
                data: [<%=data%>],
                colModel: [
                    { name: 'GameID', colType: 'ID' },
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked' },
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'currency', formatter: 'currency', editable: false, classes: 'gamebalance-load' },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: false, editonce: false, editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'mode               ', label: '<%=lang["colHGmode           "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'firstname          ', label: '<%=lang["colHGfirstname      "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'lastname           ', label: '<%=lang["colHGlastname       "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'agentid            ', label: '<%=lang["colHGagentid        "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'affiliateid        ', label: '<%=lang["colHGaffiliateid    "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'playerlevel        ', label: '<%=lang["colHGplayerlevel    "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'testusr            ', label: '<%=lang["colHGtestusr        "]%>', width: 080, sorttype: 'text', editable: false }
                ]
            });
        <% } else if (game.ID == GameID.EA) { %>
            init_table('<%=(int?)game.ID%>', {
                data: [<%=data%>],
                colModel: [
                    { name: 'GameID', colType: 'ID' },
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked' },
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'currency', formatter: 'currency', editable: false, classes: 'gamebalance-load' },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: false, editonce: false, editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'mode               ', label: '<%=lang["colGameMode         "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'agentid            ', label: '<%=lang["colAgentACNT        "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'username           ', label: '<%=lang["colNickName         "]%>', width: 080, sorttype: 'text', editable: false }
                ]
            });
        <% } else if (game.ID.In(GameID.WFT, GameID.WFT_SPORTS)) { %>
            init_table('<%=(int?)game.ID%>', {
                data: [<%=data%>],
                colModel: [
                    { name: 'GameID', colType: 'ID' },
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked', editoptions: {<%=enumlist<Locked>("value")%> } },
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'currency', formatter: 'currency', editable: true, classes: 'gamebalance-load' },
                    { name: 'currencyid         ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: true, editonce: false, editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'gametype           ', label: '<%=lang["colGameType         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'username           ', label: '<%=lang["colNickName         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'gametype           ', label: '<%=lang["colGameType         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'language           ', label: '<%=lang["colLanguage         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'stake              ', label: '<%=lang["colStake            "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            });
        <% } else if (game.ID.In(GameID.KENO, GameID.KENO_SSC)) { %>
            init_table('<%=(int?)game.ID%>', {
                data: [<%=data%>],
                colModel: [
                    { name: 'GameID', colType: 'ID' },
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked' },
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'currency', formatter: 'currency', editable: true, classes: 'gamebalance-load' },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: true, editonce: false, editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            });
        <% } else if ( game.ID.In(GameID.SUNBET, GameID.AG, GameID.CROWN_SPORTS)) { %>
            init_table('<%=(int?)game.ID%>', {
                data: [<%=data%>],
                colModel: [
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked' },
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'currency', formatter: 'currency', editable: true },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: true, editonce: false, editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            });
        <% } else if (game.ID == GameID.BBIN) { %>
            init_table('<%=(int?)game.ID%>', {
                data: [<%=data%>],
                colModel: [
                    { name: 'GameID', colType: 'ID' },
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked' },
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'currency', formatter: 'currency', editable: true, classes: 'gamebalance-load' },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: true, editonce: false, editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            });
            <% }
            }
        } %>

            $('.ui-jqgrid').removeClass('ui-corner-all');//.css({ 'border-top-width': 0, 'border-bottom-width': 1, 'border-right-width': 0 });
            $('.ui-th-column').removeClass('ui-state-default').addClass('ui-widget-content');
        });
    </script>
<style type="text/css">
    .gamebalance-load { cursor: pointer; }
</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <%--<table id="subacc">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow" icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
    </table>--%>
    <div class="template" style="display: none;">
        <img class="load_balance" src="../images/loading1_000000.gif">
        <table>
            <tr class="colModel">
                <td name="Action">
                    <span property="action">
                        <div class="edithide" action="editRow" icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                        <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                        <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                    </span>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
