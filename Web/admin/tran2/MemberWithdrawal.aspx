<%@ Page Title="" Language="C#" MasterPageFile="MemberTran.master" AutoEventWireup="true" Inherits="MemberTran_page" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.IsDeposit = false;
        this.IsHist = false;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function () {
            $table = $('#table1').tranGrid_init('<%=this.IsDeposit%>', '<%=this.IsHist%>', {
                colModel: [
                    cm.Action,
                    cm.Accept,
                    cm.Finish,
                    cm.ID,
                    cm.SerialNumber,
                    cm.LogType,
                    cm.State,
                    cm.CreateTime,
                    cm.CorpID,
                    cm.AgentACNT,
                    cm.MemberACNT,
                    cm.MemberName,
                    cm.Amount1,
                    cm.Amount2,
                    cm.Currency,
                    cm.Memo1,
                    cm.Memo2,
                    cm.a_BankName,
                    cm.a_CardID,
                    cm.a_Name,
                    cm.a_TranTime,
                    cm.a_TranSerial,
                    cm.a_TranMemo,
                    cm.b_BankName,
                    cm.b_CardID,
                    cm.b_Name,
                    cm.b_TranTime,
                    cm.b_TranSerial,
                    cm.b_TranMemo,
                    cm.RequestIP,
                    cm.CreateUser,
                    cm.ModifyTime,
                    cm.ModifyUser
                ]
            });
          });
    </script>
    <%--<script type="text/javascript">

        var $table;

        $(document).ready(function () {

            $table = $('#table1').jqGrid_init({
                useDefValues: true, pager: true, nav1 : '#nav1', nav2: '#nav2', postData: { IsDeposit: false, IsHist: false },
                editParams: { delayDeleteRow: 1000 },
                SelectCommand: function (postData) { return { MemberTranSelect: postData } },
                UpdateCommand: function (postData) { return { MemberTranUpdate: postData } },
                InsertCommand: function (postData) { return { MemberTranInsert: postData } },
                DeleteCommand: function (postData) { return { MemberTranDelete: postData } },
                colModel: [
                    { name: 'Action         ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
                    { name: 'Accept         ', label: '<%=lang["colAccept       "]%>', width: 060, editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Accept"]%>' } } },
                    { name: 'Finish         ', label: '<%=lang["colFinish       "]%>', width: 060, editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Finish"]%>' } } },
                    { name: 'ID             ', label: '<%=lang["colID           "]%>', width: 280, editable: false, fixed: true, hidden: true, key: true },
                    { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, editable: false, fixed: true },
                    { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 080, editable: true, editonce: true, edittype: 'select', editoptions: { <%=enumlist<LogType>("value", true, BU.LogType.Withdrawal)%> }, formatter: 'select', formatoptions: { <%=enumlist<LogType>("value")%> } },
                    { name: 'State          ', label: '<%=lang["colState        "]%>', width: 075, editable: false, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<TranState>("value")%> } },
                  //{ name: 'FinishTime     ', label: '<%=lang["colFinishTime   "]%>', colType: 'DateTime2', nowrap: true },
                    { name: 'CreateTime     ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', nowrap: true },
                  //{ name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 050, sorttype: 'int     ', editable: false, editonce: false },
                    { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                    { name: 'AgentACNT      ', label: '<%=lang["colAgentACNT    "]%>', width: 080, editable: false },
                    { name: 'MemberACNT     ', label: '<%=lang["colMemberACNT   "]%>', width: 080, editable: true, editonce: true },
                    { name: 'Amount1        ', label: '<%=lang["colAmount1      "]%>', colType: 'Money', editable: true, editonce: true },
                    { name: 'Amount2        ', label: '<%=lang["colAmount2      "]%>', colType: 'Money', editable: false, editonce: false },
                    { name: 'Currency       ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency', editable: false, editonce: false },
                    { name: 'Memo1          ', label: '<%=lang["colMemo1        "]%>', width: 080, editable: true, edittype: 'textarea' },
                    { name: 'Memo2          ', label: '<%=lang["colMemo2        "]%>', width: 080, editable: true, edittype: 'textarea' },
                    { name: 'a_BankName     ', label: '<%=lang["colBankNameA    "]%>', width: 080, editable: true },
                    { name: 'a_CardID       ', label: '<%=lang["colCardIDA      "]%>', width: 080, editable: true },
                    { name: 'a_Name         ', label: '<%=lang["colNameA        "]%>', width: 080, editable: true },
                    { name: 'a_TranTime     ', label: '<%=lang["colTranTimeA    "]%>', width: 080, editable: true, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss', formatNaN: '' } },
                    { name: 'a_TranSerial   ', label: '<%=lang["colTranSerialA  "]%>', width: 080, editable: true },
                    { name: 'a_TranMemo     ', label: '<%=lang["colTranMemoA    "]%>', width: 080, editable: true, edittype: 'textarea' },
                    { name: 'b_BankName     ', label: '<%=lang["colBankNameB    "]%>', width: 080, editable: true },
                    { name: 'b_CardID       ', label: '<%=lang["colCardIDB      "]%>', width: 080, editable: true },
                    { name: 'b_Name         ', label: '<%=lang["colNameB        "]%>', width: 080, editable: true },
                    { name: 'b_TranTime     ', label: '<%=lang["colTranTimeB    "]%>', width: 080, editable: true, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss', formatNaN: '' } },
                    { name: 'b_TranSerial   ', label: '<%=lang["colTranSerialB  "]%>', width: 080, editable: true },
                    { name: 'b_TranMemo     ', label: '<%=lang["colTranMemoB    "]%>', width: 080, editable: true, edittype: 'textarea' },
                    { name: 'RequestIP      ', label: '<%=lang["colRequestIP    "]%>', width: 080, editable: false },
                    { name: 'CreateUser     ', label: '<%=lang["colCreateUser   "]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime     ', label: '<%=lang["colModifyTime   "]%>', colType: 'DateTime2', nowrap: true },
                    { name: 'ModifyUser     ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' },
                ]
            });
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });



    </script>--%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="Server"></asp:Content>
