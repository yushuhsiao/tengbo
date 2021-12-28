<%@ Page Title="" Language="C#" MasterPageFile="PrompTran.master" AutoEventWireup="true" Inherits="PrompTran_page" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.IsHist = false;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var recvMessage = {
            dbgmsg: function (data) {
                if (data.code == '<%=(int)BU.RowErrorCode.RefreshRequired%>')
                    $table.reloadGrid();
            }
        }

        var $table;
        $(document).ready(function () {
            $table = $('#table1').tranGrid_init('<%=this.IsHist%>', {
                colModel: [
                    cm.Action,
                    cm.Finish,
                    cm.ID,
                    cm.SerialNumber,
                    cm.LogType,
                    cm.State,
                    cm.CreateTime,
                    cm.CorpID,
                    cm.AgentACNT,
                    cm.MemberACNT,
                    cm.Amount1,
                    cm.Currency,
                    cm.RequestIP,
                    cm.Memo1,
                    cm.Memo2,
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
                pager: true, nav1 : '#nav1', nav2: '#nav2', postData: { IsHist: false },
                editParams: { delayDeleteRow: 1000 },
                SelectCommand: function (postData) { return { PromoTranSelect: postData } },
                UpdateCommand: function (postData) { return { PromoTranUpdate: postData } },
                InsertCommand: function (postData) { return { PromoTranInsert: postData } },
                DeleteCommand: function (postData) { return { PromoTranDelete: postData } },

                colModel: [
                        { name: 'Action         ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
                        { name: 'Finish         ', label: '<%=lang["colFinish       "]%>', width: 060, editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Finish"]%>' } } },
                        { name: 'ID             ', label: '<%=lang["colID           "]%>', width: 280, editable: false, fixed: true, hidden: true, key: true },
                        { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, editable: false, fixed: true },
                        { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 120, editable: true, editonce: true, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist("value", true, LogType.全勤優惠, LogType.存款優惠, LogType.洗碼優惠, LogType.首存優惠, LogType.彩金贈送, LogType.VIP直通车, LogType.好友推荐, LogType.复活礼金, LogType.生日礼金, LogType.晋级奖金,LogType.周周红利, LogType.绿色通道入款/*, LogType.全勤優惠_前置單, LogType.存款優惠_前置單, LogType.洗碼優惠_前置單, LogType.首存優惠_前置單*/)%> } },
                        { name: 'State          ', label: '<%=lang["colState        "]%>', width: 075, editable: false, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<TranState>("value")%> } },
                      //{ name: 'FinishTime     ', label: '<%=lang["colFinishTime   "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'CreateTime     ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', nowrap: true },
                      //{ name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 050, sorttype: 'int     ', editable: false, editonce: false },
                        { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                        { name: 'AgentACNT      ', label: '<%=lang["colAgentACNT    "]%>', width: 080, editable: false },
                        { name: 'MemberACNT     ', label: '<%=lang["colMemberACNT   "]%>', width: 080, editable: true, editonce: true },
                        { name: 'Amount1        ', label: '<%=lang["colAmount1      "]%>', colType: 'Money', editable: true, editonce: true },
                      //{ name: 'Amount2        ', label: '<%=lang["colAmount2      "]%>', colType: 'Money', editable: false, editonce: false },
                        { name: 'Currency       ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency', editable: false, editonce: false },
                        { name: 'RequestIP      ', label: '<%=lang["colRequestIP    "]%>', width: 080, editable: false },
                        { name: 'Memo1          ', label: '<%=lang["colMemo1        "]%>', width: 080, editable: true, edittype: 'textarea' },
                        { name: 'Memo2          ', label: '<%=lang["colMemo2        "]%>', width: 080, editable: true, edittype: 'textarea' },
                        { name: 'CreateUser     ', label: '<%=lang["colCreateUser   "]%>', colType: 'ACNT2' },
                        { name: 'ModifyTime     ', label: '<%=lang["colModifyTime   "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'ModifyUser     ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' },
                ],
            });
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });

    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server"></asp:Content>

