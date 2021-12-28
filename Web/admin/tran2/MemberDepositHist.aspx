<%@ Page Title="" Language="C#" MasterPageFile="MemberTran.master" AutoEventWireup="true" Inherits="MemberTran_page" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.IsDeposit = true;
        this.IsHist = true;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function () {
            $table = $('#table1').tranGrid_init('<%=this.IsDeposit%>', '<%=this.IsHist%>', {
                sortname: 'FinishTime',
                colModel: [
                    cm.ID,
                    cm.SerialNumber,
                    cm.LogType,
                    cm.State,
                    cm.FinishTime,
                    cm.CreateTime,
                    cm.CorpID,
                    cm.AgentACNT,
                    cm.MemberACNT,
                    cm.MemberName,
                    cm.Amount1,
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
                useDefValues: true, pager: true, nav1 : '#nav1', nav2: '#nav2', postData: { IsDeposit: true, IsHist: true },
                cmTemplate: { editable: false },
                SelectCommand: function (postData) { return { MemberTranSelect: postData } },
              //UpdateCommand: function (postData) { return { MemberTranUpdate: postData } },
              //InsertCommand: function (postData) { return { MemberTranInsert: postData } },
              //DeleteCommand: function (postData) { return { MemberTranDelete: postData } },
                colModel: [
                  //{ name: 'Action         ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
                  //{ name: 'Accept         ', label: '<%=lang["colAccept       "]%>', width: 060, editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Accept"]%>' } } },
                  //{ name: 'Finish         ', label: '<%=lang["colFinish       "]%>', width: 060, editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Finish"]%>' } } },
                    { name: 'ID             ', label: '<%=lang["colID           "]%>', width: 280, fixed: true, hidden: true, key: true },
                    { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, fixed: true },
                    { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 080, formatter: 'select', editoptions: { <%=enumlist<LogType>("value")%> } },
                    { name: 'State          ', label: '<%=lang["colState        "]%>', width: 075, formatter: 'select', editoptions: { <%=enumlist<TranState>("value")%> } },
                    { name: 'FinishTime     ', label: '<%=lang["colFinishTime   "]%>', colType: 'DateTime2', nowrap: true },
                    { name: 'CreateTime     ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', nowrap: true },
                  //{ name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 050, sorttype: 'int     ', editable: false, editonce: false },
                    { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                    { name: 'AgentACNT      ', label: '<%=lang["colAgentACNT    "]%>', width: 080 },
                    { name: 'MemberACNT     ', label: '<%=lang["colMemberACNT   "]%>', width: 080 },
                    { name: 'Amount1        ', label: '<%=lang["colAmount1      "]%>', colType: 'Money' },
                    { name: 'Amount2        ', label: '<%=lang["colAmount2      "]%>', colType: 'Money' },
                    { name: 'Currency       ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency' },
                    { name: 'Memo1          ', label: '<%=lang["colMemo1        "]%>', width: 080 },
                    { name: 'Memo2          ', label: '<%=lang["colMemo2        "]%>', width: 080 },
                    { name: 'a_BankName     ', label: '<%=lang["colBankNameA    "]%>', width: 080 },
                    { name: 'a_CardID       ', label: '<%=lang["colCardIDA      "]%>', width: 080 },
                    { name: 'a_Name         ', label: '<%=lang["colNameA        "]%>', width: 080 },
                    { name: 'a_TranTime     ', label: '<%=lang["colTranTimeA    "]%>', width: 080, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss', formatNaN: '' } },
                    { name: 'a_TranSerial   ', label: '<%=lang["colTranSerialA  "]%>', width: 080 },
                    { name: 'a_TranMemo     ', label: '<%=lang["colTranMemoA    "]%>', width: 080 },
                    { name: 'b_BankName     ', label: '<%=lang["colBankNameB    "]%>', width: 080 },
                    { name: 'b_CardID       ', label: '<%=lang["colCardIDB      "]%>', width: 080 },
                    { name: 'b_Name         ', label: '<%=lang["colNameB        "]%>', width: 080 },
                    { name: 'b_TranTime     ', label: '<%=lang["colTranTimeB    "]%>', width: 080, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss', formatNaN: '' } },
                    { name: 'b_TranSerial   ', label: '<%=lang["colTranSerialB  "]%>', width: 080 },
                    { name: 'b_TranMemo     ', label: '<%=lang["colTranMemoB    "]%>', width: 080 },
                    { name: 'RequestIP      ', label: '<%=lang["colRequestIP    "]%>', width: 080 },
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
