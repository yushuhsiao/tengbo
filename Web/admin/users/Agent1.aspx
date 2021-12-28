<%@ Page Language="C#" MasterPageFile="UserDetail.master" AutoEventWireup="true" Inherits="web.page" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<script runat="server">

    int agentID;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.agentID = Request.QueryString["id"].ToInt32() ?? 0;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { <%
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            AgentRow row = AgentRow.GetAgent(sqlcmd, this.agentID, null, null, "*");
            string rowdata = api.SerializeObject(row); %>

            var col1 = { name: 'Action      ', width: 100, colType: 'Buttons' };
            var col2 = { name: 'ID          ', width: 100, colType: 'ID' };
            var col3 = { name: '_fill', label: '.', fixed: false };
            function onSelectRow(rowid, status, e) { $($(this).getInd(rowid, true)).removeClass('ui-state-highlight'); };
            $.fn._init = function (o) {
                var css = o.css; delete o.css;
                var $t = this.jqGrid_init($.extend({
                    datatype: 'local', height: 'auto', sortable: false, rownumbers: false, shrinkToFit: true, headervisible: false, data: [<%=rowdata%>],
                    onSelectRow: onSelectRow,
                    UpdateCommand: function (postData) { return { AgentUpdate: postData } },
                    RowResponse: function (res, rowid, row) { sendMessage('AgentRowData', { ID: rowid, Balance: row.Balance }); },
                    cmTemplate: { sortable: false, fixed: true }, editParams: { url: 'api', },                    
                }, o));
                var c = $t.gridContainer();
                c.removeClass('ui-corner-all');
                if (css) c.css(css); else c.css({ 'border-left': 0, 'border-bottom': 0, 'border-right': 0 });
                //if (css == 1)
                //    c.css({ 'border-left': 0, 'border-bottom': 0, 'border-right': 0 });
                //else
                //    c.css({ 'border': 0 });
                return $t;
            }

            var _postdata = {
                UserType: '<%=(int)BU.UserType.Agent%>',
                UserID: '<%=agentID%>'
            };
            function RowResponse(res, rowid, row) {
                sendMessage('AgentRowData', { ID: row.ProviderID, Balance: row.ProviderBalance });
                sendMessage('AgentRowData', { ID: row.UserID, Balance: row.UserBalance });
                sendMessage('MemberRowData', { ID: row.UserID, Balance: row.UserBalance });
            }
            var $parent_d = $('#parent_d')._init({
                RowResponse: RowResponse, UpdateCommand: function (postData) { postData.LogType = '<%=(int)BU.LogType.LoadingBalance%>'; return { UserLoadBalance: $.extend(true, postData, _postdata) } },
                autowidth: false, css: { border: 0 }, colModel: [col1, col2, { name: 'Amount', label: '<%=lang["colAmount"]%>', width: 080, editable: true, formatter: function () { return ''; } }, col3]
            });
            var $parent_w = $('#parent_w')._init({
                RowResponse: RowResponse, UpdateCommand: function (postData) { postData.LogType = '<%=(int)BU.LogType.UnloadingBalance%>'; return { UserLoadBalance: $.extend(true, postData, _postdata) } },
                autowidth: false, css: { border: 0 }, colModel: [col1, col2, { name: 'Amount', label: '<%=lang["colAmount"]%>', width: 080, editable: true, formatter: function () { return ''; } }, col3]
            });
            var $table1 = $('#table1')._init({ colModel: [col1, col2, { name: 'Password    ', width: 100, editable: true }, col3] });
            var $table2 = $('#table2')._init({ colModel: [col1, col2, { name: 'SecPassword ', width: 100, editable: true }, col3], css: { 'border-bottom': 0, 'border-right': 0 } });
            var $table6 = $('#table6')._init({
                headervisible: true, headerclass: 'ui-widget-content',
                colModel: [col1, col2,
                    { name: 'PCT        ', label: '<%=lang["colPCT         "]%>', colType: 'Percent', editable: true, width: 080 },
                    col3]
            });
            var $table7 = $('#table7')._init({
                headervisible: true, headerclass: 'ui-widget-content',
                colModel: [col2,
                    { name: 'Action     ', label: '<%=lang["colChildMember "]%>', colType: 'Buttons', width: 100 },
                    { name: 'M_BonusL   ', label: '<%=lang["colBonusL      "]%>', colType: 'Percent', editable: true, width: 080 },
                    { name: 'M_BonusW   ', label: '<%=lang["colBonusW      "]%>', colType: 'Percent', editable: true, width: 080 },
                    { name: 'M_ShareL   ', label: '<%=lang["colShareL      "]%>', colType: 'Percent', editable: true, width: 080 },
                    { name: 'M_ShareW   ', label: '<%=lang["colShareW      "]%>', colType: 'Percent', editable: true, width: 080 },
                    { name: 'M_TranFlag1', label: '<%=lang["colTranFlag1   "]%>', formatter: 'checkbox', editable: true, edittype: 'checkbox', width: 080 },
                    { name: 'M_TranFlag2', label: '<%=lang["colTranFlag2   "]%>', formatter: 'checkbox', editable: true, edittype: 'checkbox', width: 080 },
                    { name: 'MaxMember  ', label: '<%=lang["colMaxMember   "]%>', editable: true, width: 080 },
                    col3]
            }, 1);
            var $table8 = $('#table8')._init({
                headervisible: true, headerclass: 'ui-widget-content',
                colModel: [col2,
                    { name: 'Action     ', label: '<%=lang["colChildAgent  "]%>', colType: 'Buttons', width: 100 },
                    { name: 'A_BonusL   ', label: '<%=lang["colBonusL      "]%>', colType: 'Percent', editable: true, width: 080 },
                    { name: 'A_BonusW   ', label: '<%=lang["colBonusW      "]%>', colType: 'Percent', editable: true, width: 080 },
                    { name: 'A_ShareL   ', label: '<%=lang["colShareL      "]%>', colType: 'Percent', editable: true, width: 080 },
                    { name: 'A_ShareW   ', label: '<%=lang["colShareW      "]%>', colType: 'Percent', editable: true, width: 080 },
                    { name: 'A_TranFlag1', label: '<%=lang["colTranFlag1   "]%>', formatter: 'checkbox', editable: true, edittype: 'checkbox', width: 080 },
                    { name: 'A_TranFlag2', label: '<%=lang["colTranFlag2   "]%>', formatter: 'checkbox', editable: true, edittype: 'checkbox', width: 080 },
                    { name: 'MaxAgent   ', label: '<%=lang["colMaxAgent    "]%>', editable: true, width: 080 },
                    { name: 'MaxDepth   ', label: '<%=lang["colMaxDepth    "]%>', editable: true, width: 080 },
                    col3]
            }, 1);

            var $table_bk = $('#table_bk').jqGrid_init({
                data: [<%=api.SerializeObject(sqlcmd.ToObjectList<AgentBankCardRow>("select * from AgentBank nolock where AgentID={0}", this.agentID) ?? Tools._null<List<AgentBankCardRow>>.value)%>][0],
                cmTemplate: { sortable: false }, editParams: { url: 'api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false, headerclass: 'ui-widget-content',
                editParams: { delayDeleteRow: 1000 }, addRowParams: { position: 'last' },
                onSelectRow: onSelectRow,
                InsertCommand: function (postData) { postData.MemberID = '<%=agentID%>'; return { MemberBankCardInsert: postData } },
                UpdateCommand: function (postData) { postData.MemberID = '<%=agentID%>'; return { MemberBankCardUpdate: postData } },
                DeleteCommand: function (postData) { postData.MemberID = '<%=agentID%>'; return { MemberBankCardDelete: postData } },
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
            $table_bk.gridContainer().removeClass('ui-corner-all').css('border-bottom', 0);
            $('.btnAdd_bk').button({ icons: { primary: "ui-icon-plus" } }).removeClass('ui-state-default').css({ "border-width": 1 }).click(function () { $table_bk.addRow({ position: 'last' }); });

            <% } %>
            iframe_auto_height();
        });
    </script>
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <div class="ui-jqgrid ui-widget ui-widget-content" style="width: 900px;">
        <% if (User.Permissions[Permissions.Code.load_agent_balance]) { %>
        <table id="parent_d">
            <tr class="colModel">
                <td name="Action">
                    <span property="action">
                        <div class="edithide" action="editRow"    icon="ui-icon-plus"  ><%=lang["Actions_ParentD"]%></div>
                        <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                        <div class="editshow" action="saveRow"    icon="ui-icon-check" ><%=lang["Actions_OK"]%></div>
                    </span>
                </td>
            </tr>
        </table>
        <table id="parent_w">
            <tr class="colModel">
                <td name="Action">
                    <span property="action">
                        <div class="edithide" action="editRow"    icon="ui-icon-minus" ><%=lang["Actions_ParentW"]%></div>
                        <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                        <div class="editshow" action="saveRow"    icon="ui-icon-check" ><%=lang["Actions_OK"]%></div>
                    </span>
                </td>
            </tr>
        </table>
        <br />
        <% } %>
        <div style="width: 220px; display: inline-block;">
            <table id="table1">
                <tr class="colModel">
                    <td name="Action">
                        <span property="action">
                            <div class="edithide" action="editRow" icon="ui-icon-key"><%=lang["ModifyPassword"]%></div>
                            <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                            <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 220px; display: inline-block;">
            <table id="table2">
                <tr class="colModel">
                    <td name="Action">
                        <span property="action">
                            <div class="edithide" action="editRow" icon="ui-icon-key"><%=lang["ModifySecPassword"]%></div>
                            <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                            <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <table id="table_bk">
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
        <div class="ui-jqgrid-view btnAdd_bk"><%=lang["btnAddBankCard"]%></div>
        <br />
        <br />
        <table id="table6">
            <tr class="colModel">
                <td name="Action">
                    <span property="action">
                        <div class="edithide" action="editRow" icon="ui-icon-key"><%=lang["Actions_Edit"]%></div>
                        <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                        <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                    </span>
                </td>
            </tr>
        </table>
        <br />
        <table id="table7">
            <tr class="colModel">
                <td name="Action">
                    <span property="action">
                        <div class="edithide" action="editRow" icon="ui-icon-key"><%=lang["Actions_Edit"]%></div>
                        <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                        <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                    </span>
                </td>
            </tr>
        </table>
        <br />
        <table id="table8">
            <tr class="colModel">
                <td name="Action">
                    <span property="action">
                        <div class="edithide" action="editRow" icon="ui-icon-key"><%=lang["Actions_Edit"]%></div>
                        <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                        <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                    </span>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
