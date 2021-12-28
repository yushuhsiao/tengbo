﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/tran.master" AutoEventWireup="true" Culture="auto" UICulture="auto" Inherits="page" meta:resourcekey="PageResource1" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>

<script runat="server">

    public bool hist;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        this.hist = Request.QueryString["h"] == "1";
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        <% if (hist) {%>var hist = true;<%} else {%>var hist = false;<%}%>;
        var logType = '<%=(int)LogType.GameWithdrawal%>';

        $(document).ready(function (ind, rowid) {


            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: !hist, nav1: '#nav1', nav2: '#nav2', postData: { IsDeposit: false, IsHist: hist },
                SelectCommand: function (postData) { postData.LogType = logType; return { GameTranSelect: postData } },
                UpdateCommand: function (postData) { postData.LogType = logType; return { GameTranUpdate: postData } },
                InsertCommand: function (postData) { postData.LogType = logType; return { GameTranInsert: postData } },
                DeleteCommand: function (postData) { postData.LogType = logType; return { GameTranDelete: postData } },
                editParams: { delayDeleteRow: 1000 },

                colModel: [
<%if(!hist){%>          { name: 'Action         ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' },
                        { name: 'Finish         ', label: '<%=lang["colFinish       "]%>', width: 060, sorttype: 'text    ', editable: true, align: 'left', formatter: $.empty, edittype: 'checkboxs', editoptions: { multiple: false, value: { 1: '<%=lang["op_Finish"]%>' } } }, <%}%>
                        { name: 'ID             ', label: '<%=lang["colID           "]%>', width: 280, sorttype: 'text    ', editable: false, fixed: true, hidden: true, key: true },
                        { name: 'SerialNumber   ', label: '<%=lang["colSerialNumber "]%>', width: 090, sorttype: 'text    ', editable: false, fixed: true },
                        { name: 'LogType        ', label: '<%=lang["colLogType      "]%>', width: 060, sorttype: 'text    ', editable: false, formatter: 'select', editoptions: { <%=enumlist<LogType>("value")%> } },
                        { name: 'State          ', label: '<%=lang["colState        "]%>', width: 075, sorttype: 'text    ', editable: false, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<TranState>("value")%> } },
                        { name: 'GameID         ', label: '<%=lang["colGameID       "]%>', width: 080, sorttype: 'int     ', editable: true, editonce: true, formatter: 'select', edittype: 'select', editoptions: { <%=serializeEnum("value", web.GameRow.Cache.Instance.Rows2)%> } },
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
                        { name: 'CreateUser     ', label: '<%=lang["colCreateUser   "]%>', colType: 'ACNT2' },
                        { name: 'ModifyTime     ', label: '<%=lang["colModifyTime   "]%>', colType: 'DateTime2', nowrap: true },
                        { name: 'ModifyUser     ', label: '<%=lang["colModifyUser   "]%>', colType: 'ACNT2' },
                ]
            });

            // 工具列
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $table[0].grid.$toolbar.css('height', 'auto');
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
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

