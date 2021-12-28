<%@ Page Language="C#" MasterPageFile="UserDetail.master" AutoEventWireup="true" CodeBehind="~/admin/UserList.aspx.cs" Inherits="web.UserList2_aspx" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<script runat="server">
    public override UserType UserType { get { return BU.UserType.Member; } }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var recvMessage = {
            MemberGameRowData: function (data) {
                $table01.restoreRow(data.GameID);
                $table02.restoreRow(data.GameID);
                $table01.setRowData(data.GameID, data);
                $table02.setRowData(data.GameID, data);
            }
        }

        var $table01, $table02;
        $(document).ready(function () {
            function onSelectRow(rowid, status, e) {
                //$($(this).getInd(rowid, true)).removeClass('ui-state-highlight');
            };

            function UpdateCommand(postData) {
                postData.MemberID = '<%=userID%>';
                return { MemberGameUpdate: postData };
            };

            function getBalance(rowid, iCol, cellcontent, e) {
                var $t = $(this);
                var cm = this.p.colModel[iCol];
                var $cell = $($t.getCellObj(rowid, cm.name));
                if ($cell.hasClass('gamebalance-load')) {
                    $cell.removeClass('ui-state-error');
                    if (!$cell.hasClass('gamebalance-loading')) {
                        $cell.addClass('gamebalance-loading').empty();
                        var $img = $('.template .load_balance').clone().appendTo($cell);
                        $.invoke_api({ MemberGameSelect: { GameID: rowid, MemberID: '<%=userID%>' } }, {
                            success: function (data, textStatus, jqXHR) {
                                if (data.Status == 1) {
                                    $t.restoreRow(rowid).setRowData(rowid, data.row);
                                    //$t.setCell(rowid, cm.name, data.row[cm.name]);
                                }
                                else {
                                    //$cell.addClass('ui-state-error');
                                    $t.setCell(rowid, cm.name, '{0} : {1}'.format(data.Status));
                                    sendMsg('error', null, data.Message, data.Status);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                $t.setCell(rowid, cm.name, 'err');
                                sendMsg('error', null, xhr.statusText, xhr.status);
                                console.log(xhr);
                            },
                            complete: function (jqXHR, textStatus) {
                                $cell.removeClass('gamebalance-loading');
                            }
                        });
                    }
                }
            };

            function gameTran(cell, cm, cellIndex) {
                var $cell = $(cell);
                var $ind = $cell.closest('tr.jqgrow');
                var rowid = $ind.attr('id');
                //console.log('cell_init', { arguments: arguments, $ind: $ind, rowid: rowid, data: $(this).getRowData(rowid), });
                $(cm._dom).children().clone(true, true).appendTo($cell.empty());
                $('.d, .w', cell).button().removeClass('ui-state-default').click(function () {
                    var $txt = $('.a', cell);
                    var postData = {
                        LogType: $(this).hasClass('d') ? '<%=LogType.GameDeposit%>' : '<%=LogType.GameWithdrawal%>',
                        GameID: rowid,
                        MemberID: '<%=userID%>',
                        Amount1: parseFloat($.trim($txt.val()))
                    };

                    //console.log(postData);

                    $txt.val('');
                    if ($txt.prop('disabled') == false) {
                        $txt.prop('disabled', true);
                        $.invoke_api({ GameTranInsert: postData }, {
                            success: function (data, textStatus, jqXHR) {
                                if (data.row) {
                                    sendMessage('MemberRowData', { ID: data.row.MemberID, Balance: data.row.Balance });
                                    if (data.row.MemberGameRow)
                                        sendMessage('MemberGameRowData', data.row.MemberGameRow);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                sendMsg('error', null, JSON.stringify(arguments), arguments);
                            },
                            complete: function (jqXHR, textStatus) {
                                $txt.prop('disabled', false);
                            }
                        });
                    }
                });
                $cell.addClass('gametran').css('white-space', 'nowrap');
            }

            <% if ((this.rows1 != null) && (this.rows1.Count > 0)) { %>
            $table01 = $('#table01').show().jqGrid_init({
                data: [<%=api.SerializeObject(rows1)%>][0],
                cmTemplate: { sortable: false }, editParams: { url: '.api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false,
                onSelectRow: onSelectRow, UpdateCommand: UpdateCommand, onCellSelect: getBalance,
                colModel: [
                    { name: 'Action             ', label: '<%=lang["colAction           "]%>', colType: 'Buttons' },
                    { name: 'GameID             ', label: '<%=lang["colGameID           "]%>', width: 080, colType: 'ID', hidden: false, formatter: 'select', editoptions: { <%=serializeEnum("value", web.game.Names_Active)%> } },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, editable: false },
                  //{ name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, editable: false },
                    { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked' },
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, formatter: 'currency', editable: false, classes: 'gamebalance-load' },
                    { name: 'GetBalanceTime     ', label: '<%=lang["colGetBalanceTime   "]%>', colType: 'DateTime2' },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: false, editonce: false },
                    { name: 'GameTran           ', label: '<%=lang["colGameTran         "]%>', width: 250, search: false, editable: false, sortable: false, fixed: true, align: 'left', hidden: false, cell_init: gameTran },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, editable: true, hidden: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, editable: true, hidden: true },
                ]
            });
            <% } if ((this.rows2 != null) && (this.rows2.Count > 0)) { %>
            $table02 = $('#table02').show().jqGrid_init({
                data: [<%=api.SerializeObject(rows2)%>][0],
                cmTemplate: { sortable: false }, editParams: { url: '.api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false,
                onSelectRow: onSelectRow, UpdateCommand: UpdateCommand,
                colModel: [
                   { name: 'Action             ', label: '<%=lang["colAction           "]%>', colType: 'Buttons' },
                   { name: 'GameID             ', label: '<%=lang["colGameID           "]%>', width: 080, colType: 'ID', hidden: false, formatter: 'select', editoptions: { <%=serializeEnum("value", web.game.Names_Active)%> } },
                   { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, editable: true },
                   { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, editable: true, hidden: true },
                   { name: 'Locked             ', label: '<%=lang["colLocked           "]%>', colType: 'Locked' },
                   { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, formatter: 'currency', editable: true },
                   { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: true, editonce: false },
                   { name: 'GameTran           ', label: '<%=lang["colGameTran         "]%>', width: 250, search: false, editable: false, sortable: false, fixed: true, align: 'left', hidden: false, cell_init: gameTran },
                   { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, editable: true, hidden: true },
                   { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, editable: true, hidden: true }
                ]
            });
            <% } %>
            $('.ui-jqgrid').removeClass('ui-corner-all').css({ 'border-bottom-width': 0, 'border-right-width': 0, 'margin-top': 1 });
            $('.ui-th-column').removeClass('ui-state-default').addClass('ui-widget-content');
            iframe_auto_height();
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <table id="table01" style="display: none">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow" icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                </span>
            </td>
            <td name="GameTran">
                <span class="m"></span>
                <input class="a" type="text" />
                <span class="d"><%=lang["colDeposit     "]%></span>
                <span class="w"><%=lang["colWithdrawal  "]%></span>
            </td>
        </tr>
    </table>
    <table id="table02" style="display: none;">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow" icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                </span>
            </td>
            <td name="GameTran">
                <span class="m"></span>
                <input class="a" type="text" />
                <span class="d"><%=lang["colDeposit     "]%></span>
                <span class="w"><%=lang["colWithdrawal  "]%></span>
            </td>
        </tr>
    </table>
    <div class="template" style="display: none;">
        <img class="load_balance" src="../images/loading1_000000.gif">
    </div>
</asp:Content>
