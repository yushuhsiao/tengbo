<%@ Page Language="C#" MasterPageFile="UserDetail.master" AutoEventWireup="true" Inherits="web.page" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<script runat="server">

    int memberID;
    string data_bankcard;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        this.memberID = Request.QueryString["id"].ToInt32() ?? 0;
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            List<MemberBankCardRow> cards = new List<MemberBankCardRow>();
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from MemberBank nolock where MemberID={0}", this.memberID))
                cards.Add(r.ToObject<MemberBankCardRow>());

            data_bankcard = api.SerializeObject(cards);
        }
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            function onSelectRow(rowid, status, e) { $($(this).getInd(rowid, true)).removeClass('ui-state-highlight'); };
            function RowResponse(res, rowid, data) {
                if (data.src) sendMessage('AgentRowData', { ID: data.src.AgentID, Balance: data.src.Balance });
                if (data.dst) sendMessage('MemberRowData', { ID: data.dst.MemberID, Balance: data.dst.Balance });
                //if (data.Member) sendMessage('MemberUpdate', data.Member);
                //if (data.Agent) sendMessage('AgentUpdate', data.Agent);
            }
            $table1 = $('#table1').jqGrid_init({
                data: [<%=data_bankcard%>][0],
                cmTemplate: { sortable: false }, editParams: { url: 'api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false,
                editParams: { delayDeleteRow: 1000 }, addRowParams: { position: 'last' },
                onSelectRow: onSelectRow,
                InsertCommand: function (postData) { postData.MemberID = '<%=memberID%>'; return { MemberBankCardInsert: postData } },
                UpdateCommand: function (postData) { postData.MemberID = '<%=memberID%>'; return { MemberBankCardUpdate: postData } },
                DeleteCommand: function (postData) { postData.MemberID = '<%=memberID%>'; return { MemberBankCardDelete: postData } },
                colModel: [
                    { name: 'Action     ', label: '<%=lang["Title_Banks   "]%>', colType: 'Buttons' },
                    { name: 'Index      ', label: '<%=lang["colIndex      "]%>', width: 60, colType: 'ID', editable: false, hidden: true },
                    { name: 'newIndex   ', label: '<%=lang["colIndex      "]%>', width: 60, colType: 'ID', editable: true, hidden: false, key: false, formatter: function (cellval, opts, rwd, act) { return rwd.Index || cellval || ''; } },
                    { name: 'BankName   ', label: '<%=lang["colBankName   "]%>', width: 080, editable: true },
                    { name: 'AccountName', label: '<%=lang["colAccountName"]%>', width: 080, editable: true },
                    { name: 'CardID     ', label: '<%=lang["colCardID     "]%>', width: 080, editable: true },
                    { name: 'Loc1       ', label: '<%=lang["colLoc1       "]%>', width: 080, editable: true },
                    { name: 'Loc2       ', label: '<%=lang["colLoc2       "]%>', width: 080, editable: true },
                    { name: 'Loc3       ', label: '<%=lang["colLoc3       "]%>', width: 080, editable: true }
                ]
            });
            $table2 = $('#table2').jqGrid_init({
                data: [{}],
                cmTemplate: { sortable: false }, editParams: { url: 'api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false, headervisible: false,
                onSelectRow: onSelectRow, RowResponse: RowResponse, UpdateCommand: function (postData) { postData.MemberID = '<%=memberID%>'; return { MemberTranInsert: postData } },
                colModel: [
                    { name: 'Action     ', label: '.', colType: 'Buttons' },
                    { name: 'LogType    ', width: 080, editable: true, formatter: $.empty, edittype: 'select', editoptions: { <%=enumlist<LogType>("value", true, BU.LogType.Deposit, BU.LogType.Alipay)%> } },
                    { name: 'Amount1    ', width: 080, editable: true, formatter: $.empty },
                ]
            });
            $table3 = $('#table3').jqGrid_init({
                data: [{}],
                cmTemplate: { sortable: false }, editParams: { url: 'api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false, headervisible: false,
                onSelectRow: onSelectRow, RowResponse: RowResponse, UpdateCommand: function (postData) { postData.MemberID = '<%=memberID%>'; return { MemberTranInsert: postData } },
                colModel: [
                    { name: 'Action     ', label: '.', colType: 'Buttons' },
                    { name: 'LogType    ', width: 080, editable: true, formatter: $.empty, edittype: 'select', editoptions: { <%=enumlist<LogType>("value", true, BU.LogType.Withdrawal)%> } },
                    { name: 'Amount1    ', width: 080, editable: true, formatter: $.empty },
                    { name: 'Accept     ', width: 060, editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Accept"]%>' } } },
                ]
            });
            var data_balance = [
                { LogType: 0<%=(int)BU.LogType.Deposit   %>, Action: JSON.stringify({ editRow: { icons: { primary: 'ui-icon-arrowreturn-1-e' }, label: '<%=lang["btnAgentDeposit"]%>' } }) },
                { LogType: 0<%=(int)BU.LogType.Withdrawal%>, Action: JSON.stringify({ editRow: { icons: { primary: 'ui-icon-arrowreturn-1-w' }, label: '<%=lang["btnAgentWithdrawal"]%>' } }) }
            ];
            var colLogType = { name: 'LogType', width: 080, key: true, hidden: true, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<LogType>("value", true, BU.LogType.Deposit, BU.LogType.Withdrawal)%> } };
            $table4 = $('#table4').jqGrid_init({
                data: data_balance, cmTemplate: { sortable: false }, editParams: { url: 'api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false,
                onSelectRow: onSelectRow, RowResponse: RowResponse, UpdateCommand: function (postData) { postData.MemberID = '<%=memberID%>'; return { ParentTranM: postData } },
                colModel: [
                    { name: 'Action', colType: 'Buttons', label: '<%=lang["Title_Balance"]%>' },
                    colLogType,
                    { name: 'Amount', label: '<%=lang["colAmount"]%>', width: 080, editable: true, formatter: $.empty },
                ]
            });

            $('.ui-jqgrid').removeClass('ui-corner-all').css({ 'border-bottom-width': 0, 'border-right-width': 0, 'margin-top': 1 });
            $('.ui-th-column').removeClass('ui-state-default').addClass('ui-widget-content');
            $('.btnAdd')
                .button({ icons: { primary: "ui-icon-plus" } }).removeClass('ui-state-default').css({ "border-width": 1 })
                .click(function () { $table1.addRow({ position: 'last' }); });
            iframe_auto_height();
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
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
        </tr>
    </table>
    <div class="ui-jqgrid ui-widget ui-widget-content" style="float: left; border-width:0; padding:0;"><div class="ui-jqgrid-view btnAdd"><%=lang["btnAddBankCard"]%></div></div>
    <div style="clear: both;"></div>
    <hr />
    <table id="table2">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow" icon="ui-icon-plus"><%=lang["btnAdd"]%><%=lang["menu", "MemberTran Deposit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow" icon="ui-icon-check"><%=lang["Actions_OK"]%></div>
                </span>
            </td>
        </tr>
    </table>
    <table id="table3">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow" icon="ui-icon-plus"><%=lang["btnAdd"]%><%=lang["menu", "MemberTran Withdrawal"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow" icon="ui-icon-check"><%=lang["Actions_OK"]%></div>
                </span>
            </td>
        </tr>
    </table>
    <% if (User.Permissions[Permissions.Code.load_member_balance]) { %>
    <hr />
    <table id="table4">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow" icon="ui-icon-plus">??</div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow" icon="ui-icon-check"><%=lang["Actions_OK"]%></div>
                </span>
            </td>
        </tr>
    </table>
    <% } %>
</asp:Content>
