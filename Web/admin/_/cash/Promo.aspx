<%@ Page Title="" Language="C#" MasterPageFile="~/admin/page.master" AutoEventWireup="true" Culture="auto" UICulture="auto" Inherits="page" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="web" %>
<%@ Import Namespace="web.data" %>
<%@ Import Namespace="BU" %>


<script runat="server">

    public bool IsHistory;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        this.IsHistory = string.Compare(Request.QueryString["t"], "h", true) == 0;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;

        $(document).ready(function (ind, rowid) {

            var hist = false;
            <% if (this.IsHistory) { %>
            hist = true;
            <% } %>

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: !hist, _subGrid: true, nav1: '#nav1', nav2: '#nav2', msg: '.msg_area',
                sortname: hist ? 'FinishTime' : 'CreateTime',
                postData: { hist: hist },
                SelectCommand: function (postData) { return { PromoSelect: postData } },
                UpdateCommand: function (postData) { return { PromoUpdate: postData } },
                InsertCommand: function (postData) { return { PromoInsert: postData } },

                // 收款卡號(公司), 交易資訊(客戶輸入), 交易資訊(網銀查帳)

                colModel: [
                    { name: 'Action       ', label: '<%=lang["colAction    "]%>', colType: 'Buttons', hidden: hist, },
                    { name: 'ID           ', label: '<%=lang["colID        "]%>', colType: 'ID',<%=showID%> },
                    { name: 'PromoType    ', label: '<%=lang["colPromoType "]%>', width: 080, sorttype: 'int     ', editable: true, editonce: true, formatter: 'select', edittype: 'select', editoptions: {  <%=enumlist<PromoType>("value")%> }, },
                    { name: 'FinishTime   ', label: '<%=lang["colFinishTime"]%>', colType: 'DateTime2', hidden: !hist },
                    { name: 'CreateTime   ', label: '<%=lang["colCreateTime"]%>', colType: 'DateTime2', },
                    { name: 'State        ', label: '<%=lang["colState     "]%>', width: 080, sorttype: 'text    ', editable: false, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<PromoState>("value")%> } },
                    { name: 'Accept       ', label: '<%=lang["colAccept    "]%>', width: 080, sorttype: 'text    ', editable: !hist, align: 'left', hidden: hist, formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false,<%=enumlist<AcceptOrReject>("value")%> } },
                    { name: 'CorpID       ', label: '<%=lang["colCorpID    "]%>', colType: 'CorpID',<%=showCorpID%> editoptions: {<%=corplist("value")%> }, },
                    { name: 'MemberACNT   ', label: '<%=lang["colMemberACNT"]%>', width: 080, sorttype: 'text    ', editable: !hist, editonce: true, },
                    { name: 'Amount       ', label: '<%=lang["colAmount    "]%>', colType: 'Money', editable: !hist, editonce: true },
                    { name: 'Currency     ', label: '<%=lang["colCurrency  "]%>', colType: 'Currency', editable: false, editonce: false, editoptions: { <%=enumlist<CurrencyCode>("value")%> } },
                    { name: 'Memo1        ', label: '<%=lang["colMemo1     "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist, },
                    { name: 'Memo2        ', label: '<%=lang["colMemo2     "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist, hidden: true, },
                    { name: 'Memo3        ', label: '<%=lang["colMemo3     "]%>', width: 080, sorttype: 'text    ', edittype: 'textarea', editable: !hist, hidden: true, },
                    { name: 'CreateUser   ', label: '<%=lang["colCreateUser"]%>', colType: 'ACNT2', },
                    { name: 'ModifyTime   ', label: '<%=lang["colModifyTime"]%>', colType: 'DateTime2', },
                    { name: 'ModifyUser   ', label: '<%=lang["colModifyUser"]%>', colType: 'ACNT2', },
                ],
                subGridOptions: {
                    //expandOnLoad: true,
                    //delayOnLoad: 50,
                    selectOnExpand: true,
                    reloadOnExpand: false,
                },
            });

            // 工具列
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' }, disabled: hist }).click($table[0].addRow).css('border', 0);
            $('#btnSwitch').button({ icons: { primary: 'ui-icon-comment' } }).css('border', 0);
            $table[0].grid.$toolbar.css('height', 'auto');
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div id="nav1">
        <div class="">
            <% if (!this.IsHistory) { %>
            <button id="btnAdd"><%=lang["btnAdd"]%></button>
            <a id="btnSwitch" href="Promo.aspx?t=h"><%=lang["btnSwitch2"]%></a>
            <% } else { %>
            <a id="btnSwitch" href="Promo.aspx"><%=lang["btnSwitch1"]%></a>
            <% } %>
        </div>
    </div>
    <table id="table1">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="inline-button edithide" action="editRow"><span class="ui-icon ui-icon-pencil"></span><%=lang["actionEdit"]%></div>
                    <div class="inline-button editshow" action="restoreRow"><span class="ui-icon ui-icon-cancel"></span><%=lang["actionCancel"]%></div>
                    <div class="inline-button editshow" action="saveRow"><span class="ui-icon ui-icon-disk"></span><%=lang["actionSave"]%></div>
                </span>
            </td>
        </tr>
    </table>
    <div id="nav2" class="ui-widget-content" style=""><div class="msg_area"></div></div>
</asp:Content>

