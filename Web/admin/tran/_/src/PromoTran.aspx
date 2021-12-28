<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/tran.master" AutoEventWireup="true" Culture="auto" UICulture="auto" Inherits="page" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>


<script runat="server">
    public bool hist; protected void Page_Load(object sender, EventArgs e) { this.hist = Request.QueryString["h"] == "1"; }
</script>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;

        <% if (hist) {%>var hist = true;<%} else {%>var hist = false;<%}%>;

        $(document).ready(function (ind, rowid) {

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: !hist, _subGrid: true, nav1: '#nav1', nav2: '#nav2', postData: { IsHist: hist },
                SelectCommand: function (postData) { return { PromoTranSelect: postData } },
                UpdateCommand: function (postData) { return { PromoTranUpdate: postData } },
                InsertCommand: function (postData) { return { PromoTranInsert: postData } },
                DeleteCommand: function (postData) { return { PromoTranDelete: postData } },
                editParams: { delayDeleteRow: 3000 },
                //editParams: { RowResponse: function (res, rowid, data) { if (data._RowDeleted == 1) { setTimeout(function () { $table.delRowData(rowid); }, 3000); } } },

                colModel: [
<%if(!hist){%>          { name: 'Action         ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' }, 
                        { name: 'Finish         ', label: '<%=lang["colFinish       "]%>', width: 060, sorttype: 'text    ', editable: true, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Finish"]%>' } } },<%}%>
                        { name: 'ID             ', label: '<%=lang["colID           "]%>', width: 280, sorttype: 'text    ', editable: false, fixed: true, hidden: true, key: true },
                        { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, sorttype: 'text    ', editable: false, fixed: true },
                        { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 120, sorttype: 'text    ', editable: true, editonce: true, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist("value", true, LogType.全勤優惠, LogType.存款優惠, LogType.洗碼優惠, LogType.首存優惠, LogType.彩金贈送, LogType.VIP直通车, LogType.好友推荐, LogType.复活礼金, LogType.生日礼金, LogType.晋级奖金,LogType.周周红利, LogType.绿色通道入款/*, LogType.全勤優惠_前置單, LogType.存款優惠_前置單, LogType.洗碼優惠_前置單, LogType.首存優惠_前置單*/)%> } },
                        { name: 'State          ', label: '<%=lang["colState        "]%>', width: 075, sorttype: 'text    ', editable: false, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<TranState>("value")%> } },
<%if(hist){%>           { name: 'FinishTime     ', label: '<%=lang["colFinishTime   "]%>', colType: 'DateTime2', nowrap: true }, <%}%>
                        { name: 'CreateTime     ', label: '<%=lang["colCreateTime   "]%>', colType: 'DateTime2', nowrap: true },
                    <%--{ name: 'MemberID       ', label: '<%=lang["colMemberID     "]%>', width: 050, sorttype: 'int     ', editable: false, editonce: false },--%>
                        { name: 'CorpID         ', label: '<%=lang["colCorpID       "]%>', colType: 'CorpID' },
                        { name: 'AgentACNT      ', label: '<%=lang["colAgentACNT    "]%>', width: 080, sorttype: 'text    ', editable: false },
                        { name: 'MemberACNT     ', label: '<%=lang["colMemberACNT   "]%>', width: 080, sorttype: 'text    ', editable: true, editonce: true },
                        { name: 'Amount1        ', label: '<%=lang["colAmount1      "]%>', colType: 'Money', editable: true, editonce: true },
                      //{ name: 'Amount2        ', label: '<%=lang["colAmount2      "]%>', colType: 'Money', editable: false, editonce: false },
                        { name: 'Currency       ', label: '<%=lang["colCurrency     "]%>', colType: 'Currency', editable: false, editonce: false, editoptions: { <%=enumlist<CurrencyCode>("value")%> } },
                        { name: 'RequestIP      ', label: '<%=lang["colRequestIP    "]%>', width: 080, sorttype: 'text    ', editable: false },
                        { name: 'Memo1          ', label: '<%=lang["colMemo1        "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist },
                        { name: 'Memo2          ', label: '<%=lang["colMemo2        "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist },
                        { name: 'CreateUser     ', label: '<%=lang["colCreateUser   "]%>', colType: 'ACNT2' },
                        { name: 'ModifyTime     ', label: '<%=lang["colModifyTime   "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'ModifyUser     ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' },
                ],
            });

            // 工具列
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $table[0].grid.$toolbar.css('height', 'auto');
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
            $('#ttt').draggable({ containment: $table.gridContainer(), handle: '.ui-widget-header' }).resizable({});
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div id="nav1">
        <% if (!hist) { %><div><button id="btnAdd"><%=lang["btnAdd"]%></button></div><% } %>
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
        </tr>
    </table>
    <div id="nav2" class="ui-widget-content" style=""></div>
</asp:Content>

