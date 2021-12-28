<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/admin.master" AutoEventWireup="true" Culture="auto" UICulture="auto" Inherits="page" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>

<script runat="server">

    public LogType? logType;
    bool flag0 = false;
    bool flag1;
    bool flag2;

    protected void Page_Load(object sender, EventArgs e)
    {
        switch ((Request.QueryString["t"] ?? "").ToLower())
        {
            case "d": logType = LogType.GameDeposit; break;
            case "w": logType = LogType.GameWithdrawal; break;
            default:
            case "h": logType = null; break;
        }
        flag0 = logType == null;
        flag1 = logType == LogType.GameDeposit;
        flag2 = (logType == LogType.GameDeposit) || (logType == LogType.GameWithdrawal);
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;

        var hist = false;
        <% if (flag0) { %>
        hist = true;
        <% } %>


        $(document).ready(function (ind, rowid) {

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: !hist, _subGrid: true, nav1: '#nav1', nav2: '#nav2',
                postData: { LogType: '<%=logType%>' },
                SelectCommand: function (postData) { return { GameTranSelect: postData } },
                UpdateCommand: function (postData) { postData.LogType = '<%=logType%>'; return { GameTranUpdate: postData } },
                InsertCommand: function (postData) { postData.LogType = '<%=logType%>'; return { GameTranInsert: postData } },
                DeleteCommand: function (postData) { postData.LogType = '<%=logType%>'; return { GameTranDelete: postData } },
                editParams: { delayDeleteRow: 3000 },
                //editParams: { RowResponse: function (res, rowid, data) { if (data._RowDeleted == 1) { setTimeout(function () { $table.delRowData(rowid); }, 3000); } } },

                // 1.寫入戶內轉帳需求單(state:inital)
                // 2.異動點數並調用extAPI (state:accept)
                // 3a.完成 (state:success), 移到歷史紀錄
                // 3b.失敗 (state:failed), 還原點數並移到歷史紀錄

                colModel: [
<% if (!flag0) { %>     { name: 'Action         ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' }, <%}%>
<% if (flag1) { %>      { name: 'Accept         ', label: '<%=lang["colAccept       "]%>', width: 060, sorttype: 'text    ', editable: true, align: 'left', formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Accept"]%>' } } }, <%}%>
<% if (flag2) { %>      { name: 'Finish         ', label: '<%=lang["colFinish       "]%>', width: 060, sorttype: 'text    ', editable: true, align: 'left', formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Finish"]%>' } } }, <%}%>
                        { name: 'ID             ', label: '<%=lang["colID           "]%>', width: 280, sorttype: 'text    ', editable: false, fixed: true, hidden: true, key: true },
                        { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, sorttype: 'text    ', editable: false, fixed: true },
<% if (flag0) { %>      { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 060, sorttype: 'text    ', editable: false, formatter: 'select', editoptions: { <%=enumlist<LogType>("value")%> } }, <%}%>
                        { name: 'State          ', label: '<%=lang["colState        "]%>', width: 075, sorttype: 'text    ', editable: false, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<TranState>("value")%> } },
                        { name: 'GameID         ', label: '<%=lang["colGameID       "]%>', width: 080, sorttype: 'int     ', editable: true, editonce: true, formatter: 'select', edittype: 'select', editoptions: { <%=serializeEnum("value", web.data.GameRow.Cache.Instance.Rows2)%> } },
<% if (flag0) { %>      { name: 'FinishTime     ', label: '<%=lang["colFinishTime   "]%>', colType: 'DateTime2', nowrap: true }, <%}%>
                        { name: 'CreateTime     ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', nowrap: true },
                    <%--{ name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 050, sorttype: 'int     ', editable: false, editonce: false },--%>
                        { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID', editoptions: {<%=corplist("value")%> } },
                        { name: 'AgentACNT      ', label: '<%=lang["colAgentACNT    "]%>', width: 080, sorttype: 'text    ', editable: false },
                        { name: 'MemberACNT     ', label: '<%=lang["colMemberACNT   "]%>', width: 080, sorttype: 'text    ', editable: true, editonce: true },
                        { name: 'Amount1        ', label: '<%=lang["colAmount1      "]%>', colType: 'Money', editable: true, editonce: true },
<% if (flag1) { %>      { name: 'Amount2        ', label: '<%=lang["colAmount2      "]%>', colType: 'Money', editable: false, editonce: false }, <%}%>
                        { name: 'Currency       ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency', editable: false, editonce: false, editoptions: { <%=enumlist<CurrencyCode>("value")%> } },
                        { name: 'RequestIP      ', label: '<%=lang["colRequestIP    "]%>', width: 080, sorttype: 'text    ', editable: false },
                        { name: 'CreateUser     ', label: '<%=lang["colCreateUser   "]%>', colType: 'ACNT2' },
                        { name: 'ModifyTime     ', label: '<%=lang["colModifyTime   "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'ModifyUser     ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' },
                ],
            });

            // 工具列
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $('#btnSwitch0, #btnSwitch1, #btnSwitch2').button({ icons: { primary: 'ui-icon-comment' } }).css('border', 0);
            $table[0].grid.$toolbar.css('height', 'auto');
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
            $('#ttt').draggable({ containment: $table.gridContainer(), handle: '.ui-widget-header' }).resizable({});
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div id="nav1">
        <div>
            <% if (flag2) { %>
            <button id="btnAdd"><%=lang["btnAdd"]%></button>
            <a id="btnSwitch0" href="GameTran.aspx?t=h"><%=lang["btnSwitch0"]%></a>
            <% } else {%>
            <a id="btnSwitch1" href="GameTran.aspx?t=d"><%=lang["Menu_GameTran_Deposit"]%></a>
            <a id="btnSwitch2" href="GameTran.aspx?t=w"><%=lang["Menu_GameTran_Withdrawal"]%></a>
            <% } %>

        </div>
    </div>
    <table id="table1">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide"   action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="edithide"   action="delRow"     icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="saveRow"    icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
            <td name="Confirm">
                <span property="editoptions">
                    <input class="edit" type="radio" checked="checked" /><label><%=lang["ConfirmOptions_Ignore"]%></label><br />
                    <input class="edit" type="radio" /><label><%=lang["ConfirmOptions_Accept"]%></label><br />
                    <input class="edit" type="radio" /><label><%=lang["ConfirmOptions_Reject"]%></label>
                </span>
            </td>
        </tr>
    </table>
    <div id="nav2" class="ui-widget-content" style=""></div>
</asp:Content>

