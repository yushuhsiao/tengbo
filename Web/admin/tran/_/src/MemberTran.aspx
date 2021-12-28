<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/admin.master" AutoEventWireup="true" Culture="auto" UICulture="auto" Inherits="page"%>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>


<script runat="server">

    public LogType? logType;
    public bool hist;
    public bool dep;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.logType = null;
        this.hist = false;
        switch ((Request.QueryString["t"] ?? "").ToLower())
        {
            case "h": hist = true; break;
            case "d": logType = LogType.Deposit; this.dep = true; break;
            case "w": logType = LogType.Withdrawal; break;
        }
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        
        <% if (this.hist) {%>var hist = true;<% } else {%>var hist = false;<%}%>;

        $(document).ready(function (ind, rowid) {

            $table = $('#table1').jqGrid_init({
                useDefValues: true,
                pager: true, loadonce: !hist, _subGrid: true, nav1: '#nav1', nav2: '#nav2',
                postData: { LogType: '<%=logType%>', IsHist: hist },
                SelectCommand: function (postData) { return { MemberTranSelect: postData } },
                UpdateCommand: function (postData) { postData.LogType = postData.LogType || '<%=logType%>'; return { MemberTranUpdate: postData } },
                InsertCommand: function (postData) { postData.LogType = postData.LogType || '<%=logType%>'; return { MemberTranInsert: postData } },
                DeleteCommand: function (postData) { postData.LogType = postData.LogType || '<%=logType%>'; return { MemberTranDelete: postData } },
                editParams: { delayDeleteRow: 3000 },
                //editParams: { RowResponse: function (res, rowid, data) { if (data._RowDeleted == 1) { $table.delRowData(rowid); } } },

                colModel: [
<%if (!hist) { %>       { name: 'Action         ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
<%if (!dep) {%>         { name: 'Accept         ', label: '<%=lang["colAccept       "]%>', width: 060, sorttype: 'text    ', editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Accept"]%>' } } }, <%}%>
                        { name: 'Finish         ', label: '<%=lang["colFinish       "]%>', width: 060, sorttype: 'text    ', editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Finish"]%>' } } },<%}%>
                        { name: 'ID             ', label: '<%=lang["colID           "]%>', width: 280, sorttype: 'text    ', editable: false, fixed: true, hidden: true, key: true },
                        { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, sorttype: 'text    ', editable: false, fixed: true },
                        {
                            name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 080, sorttype: 'text    '
                            <% if (this.logType == BU.LogType.Deposit) { %>
                            , editable: true, editonce: true
                            <% } else { %>
                            , editable: false
                            <% } %>
                            , edittype: 'select', editoptions: { <%=enumlist<LogType>("value", true, BU.LogType.Deposit, BU.LogType.Alipay)%> }
                            , formatter: 'select', formatoptions: { <%=enumlist<LogType>("value")%> }
                        },
                        { name: 'State          ', label: '<%=lang["colState        "]%>', width: 075, sorttype: 'text    ', editable: false, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<TranState>("value")%> } },
<% if (hist) { %>       { name: 'FinishTime     ', label: '<%=lang["colFinishTime   "]%>', colType: 'DateTime2', nowrap: true },<%}%>
                        { name: 'CreateTime     ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', nowrap: true },
                    <%--{ name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 050, sorttype: 'int     ', editable: false, editonce: false },--%>
                        { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID', editoptions: {<%=corplist("value")%> } },
                        { name: 'AgentACNT      ', label: '<%=lang["colAgentACNT    "]%>', width: 080, sorttype: 'text    ', editable: false },
                        { name: 'MemberACNT     ', label: '<%=lang["colMemberACNT   "]%>', width: 080, sorttype: 'text    ', editable: true, editonce: true },
                        { name: 'Amount1        ', label: '<%=lang["colAmount1      "]%>', colType: 'Money', editable: true, editonce: true },
                        { name: 'Amount2        ', colType: 'Money'<%
        string amount2a = lang["colAmount2a"], amount2b=lang["colAmount2b"];
        if (hist) {       %>, label: '<%=amount2a+"/"+amount2b%>', editable: false, editonce: true<%
        } else if (dep) { %>, label: '<%=amount2b%>', editable: true, editonce: false<%
        } else {          %>, label: '<%=amount2a%>', editable: false, editonce: false<%}%>},
                        { name: 'Currency       ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency', editable: false, editonce: false, editoptions: { <%=enumlist<CurrencyCode>("value")%> } },
                        { name: 'Memo1          ', label: '<%=lang["colMemo1        "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist },
                        { name: 'Memo2          ', label: '<%=lang["colMemo2        "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist },
                        { name: 'a_BankName     ', label: '<%=lang["colBankNameA    "]%>', width: 080, sorttype: 'text    ', editable: true },
                        { name: 'a_CardID       ', label: '<%=lang["colCardIDA      "]%>', width: 080, sorttype: 'text    ', editable: true },
                        { name: 'a_Name         ', label: '<%=lang["colNameA        "]%>', width: 080, sorttype: 'text    ', editable: true },
                        { name: 'a_TranTime     ', label: '<%=lang["colTranTimeA    "]%>', width: 080, sorttype: 'date    ', editable: true, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss', formatNaN: '' } },
                        { name: 'a_TranSerial   ', label: '<%=lang["colTranSerialA  "]%>', width: 080, sorttype: 'text    ', editable: true },
                        { name: 'a_TranMemo     ', label: '<%=lang["colTranMemoA    "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist },
                        { name: 'b_BankName     ', label: '<%=lang["colBankNameB    "]%>', width: 080, sorttype: 'text    ', editable: true },
                        { name: 'b_CardID       ', label: '<%=lang["colCardIDB      "]%>', width: 080, sorttype: 'text    ', editable: true },
                        { name: 'b_Name         ', label: '<%=lang["colNameB        "]%>', width: 080, sorttype: 'text    ', editable: true },
                        { name: 'b_TranTime     ', label: '<%=lang["colTranTimeB    "]%>', width: 080, sorttype: 'date    ', editable: true, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss', formatNaN: '' } },
                        { name: 'b_TranSerial   ', label: '<%=lang["colTranSerialB  "]%>', width: 080, sorttype: 'text    ', editable: true },
                        { name: 'b_TranMemo     ', label: '<%=lang["colTranMemoB    "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist },
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
        });



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div id="nav1">
        <div>
            <% if (hist)
               { %>
            <a id="btnSwitch1" href="MemberTran.aspx?t=d"><%=lang["Menu_MemberTran_Deposit"]%></a>
            <a id="btnSwitch2" href="MemberTran.aspx?t=w"><%=lang["Menu_MemberTran_Withdrawal"]%></a>
            <% }
               else
               { %>
            <button id="btnAdd"><%=lang["btnAdd"]%></button>
            <a id="btnSwitch0" href="MemberTran.aspx?t=h"><%=lang["btnSwitch0"]%></a>
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

