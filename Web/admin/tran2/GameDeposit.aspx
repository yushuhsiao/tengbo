<%@ Page Title="" Language="C#" MasterPageFile="GameTran.master" AutoEventWireup="true" Inherits="GameTran_page" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.IsDeposit = true;
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
                    cm.GameID,
                    cm.CreateTime,
                    cm.CorpID,
                    cm.AgentACNT,
                    cm.MemberACNT,
                    cm.Amount1,
                    cm.Amount2,
                    cm.Currency,
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

        $(document).ready(function (ind, rowid) {

            $table = $('#table1').jqGrid_init({
                pager: true, nav1 : '#nav1', nav2: '#nav2', postData: { IsDeposit: true, IsHist: false },
                editParams: { delayDeleteRow: 1000 },
                SelectCommand: function (postData) { return { GameTranSelect: postData } },
                UpdateCommand: function (postData) { return { GameTranUpdate: postData } },
                InsertCommand: function (postData) { return { GameTranInsert: postData } },
                DeleteCommand: function (postData) { return { GameTranDelete: postData } },
                editParams: { delayDeleteRow: 1000 },

                colModel: [
                        { name: 'Action         ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
                        { name: 'Accept         ', label: '<%=lang["colAccept       "]%>', width: 060, editable: true, align: 'left', formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Accept"]%>' } } },
                        { name: 'Finish         ', label: '<%=lang["colFinish       "]%>', width: 060, editable: true, align: 'left', formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Finish"]%>' } } },
                        { name: 'ID             ', label: '<%=lang["colID           "]%>', width: 280, editable: false, fixed: true, hidden: true, key: true },
                        { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, editable: false, fixed: true },
                        { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 060, editable: false, formatter: 'select', editoptions: { <%=enumlist<LogType>("value")%> } },
                        { name: 'State          ', label: '<%=lang["colState        "]%>', width: 075, editable: false, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<TranState>("value")%> } },
                        { name: 'GameID         ', label: '<%=lang["colGameID       "]%>', width: 080, editable: true, editonce: true, formatter: 'select', edittype: 'select', editoptions: { <%=serializeEnum("value", web.GameRow.Cache.Instance.Rows2)%> } },
                      //{ name: 'FinishTime     ', label: '<%=lang["colFinishTime   "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'CreateTime     ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', nowrap: true },
                      //{ name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 050, sorttype: 'int     ', editable: false, editonce: false },
                        { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                        { name: 'AgentACNT      ', label: '<%=lang["colAgentACNT    "]%>', width: 080, sorttype: 'text    ', editable: false },
                        { name: 'MemberACNT     ', label: '<%=lang["colMemberACNT   "]%>', width: 080, sorttype: 'text    ', editable: true, editonce: true },
                        { name: 'Amount1        ', label: '<%=lang["colAmount1      "]%>', colType: 'Money', editable: true, editonce: true },
                        { name: 'Amount2        ', label: '<%=lang["colAmount2      "]%>', colType: 'Money', editable: false, editonce: false },
                        { name: 'Currency       ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency', editable: false, editonce: false },
                        { name: 'RequestIP      ', label: '<%=lang["colRequestIP    "]%>', width: 080, sorttype: 'text    ', editable: false },
                        { name: 'CreateUser     ', label: '<%=lang["colCreateUser   "]%>', colType: 'ACNT2' },
                        { name: 'ModifyTime     ', label: '<%=lang["colModifyTime   "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'ModifyUser     ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' },
                ],
            });
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });

    </script>--%>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="Server"></asp:Content>
