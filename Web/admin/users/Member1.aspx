<%@ Page Language="C#" MasterPageFile="UserDetail.master" AutoEventWireup="true" Inherits="web.page" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<script runat="server">

    int memberID;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.memberID = Request.QueryString["id"].ToInt32() ?? 0;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () { <%
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            MemberRow row = MemberRow.GetMember(sqlcmd, this.memberID, null, null, "*");
            string rowdata = api.SerializeObject(row); %>

            var col1 = { name: 'Action      ', width: 100, colType: 'Buttons' };
            var col2 = { name: 'ID          ', width: 100, colType: 'ID' };
            var col3 = { fixed: false };
            function onSelectRow(rowid, status, e) { $($(this).getInd(rowid, true)).removeClass('ui-state-highlight'); };
            $.fn._init = function (o) {
                var css = o.css; delete o.css;
                var $t = this.jqGrid_init($.extend({
                    datatype: 'local', height: 'auto', sortable: false, rownumbers: false, shrinkToFit: true, headervisible: false, data: [<%=rowdata%>],
                    onSelectRow: onSelectRow,
                    UpdateCommand: function (postData) { return { MemberUpdate: postData } },
                    RowResponse: function (res, rowid, row) { sendMessage('MemberRowData', { ID: rowid, Balance: row.Balance }); },
                    cmTemplate: { sortable: false, fixed: true }, editParams: { url: 'api', }
                }, o));
                var c = $t.gridContainer();
                c.removeClass('ui-corner-all');
                if (css) c.css(css); else c.css({ 'border-left': 0, 'border-bottom': 0, 'border-right': 0 });
                return $t;
            }

            <% if (User.Permissions[Permissions.Code.load_member_balance]) { %>
            var _postdata = {
                UserType: '<%=(int)BU.UserType.Member%>',
                UserID: '<%=memberID%>'
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
            <% } %>
            $('#table1')._init({ colModel: [col1, col2, { name: 'Password    ', width: 100, editable: true }, col3] });
            $('#table2')._init({ colModel: [col1, col2, { name: 'SecPassword ', width: 100, editable: true }, col3], css: { 'border-bottom': 0, 'border-right': 0 } });
            $('#table3')._init({
                colModel: [col1, col2,
                    { name: 'Sex        ', label: '<%=lang["colSex          "]%>', width: 080, colType: 'Field' }, { name: 'Sex       ', width: 080, editable: true, formatter: 'select', edittype: 'select', editoptions: { <%=enumlist<BU.UserSex>("value")%> } },
                    { name: 'Introducer ', label: '<%=lang["colIntroducer   "]%>', width: 080, colType: 'Field' }, { name: 'Introducer', width: 150, editable: true },
                col3]
            });
            $('#table4')._init({
                colModel: [col1, col2,
                    { name: 'Tel        ', label: '<%=lang["colTel          "]%>', width: 080, colType: 'Field' }, { name: 'Tel       ', width: 150, editable: true },
                    { name: 'Mail       ', label: '<%=lang["colMail         "]%>', width: 080, colType: 'Field' }, { name: 'Mail      ', width: 150, editable: true },
                    { name: 'QQ         ', label: '<%=lang["colQQ           "]%>', width: 080, colType: 'Field' }, { name: 'QQ        ', width: 150, editable: true },
                col3]
            });
            $('#table5')._init({
                colModel: [col1, col2,
                    { name: 'Birthday   ', label: '<%=lang["colBirthday     "]%>', width: 080, colType: 'Field' }, { name: 'Birthday  ', width: 150, editable: true, formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd', formatNaN: 'N/A' } },
                    { name: 'Addr       ', label: '<%=lang["colAddr         "]%>', width: 080, colType: 'Field' }, { name: 'Addr      ', width: 390, editable: true },
                    col3]
            });
            $('#table6')._init({
                colModel: [col1, col2,
                    { name: 'UserMemo   ', label: '<%=lang["colUserMemo     "]%>', width: 080, colType: 'Field' }, { name: 'UserMemo  ', width: 390, editable: true, edittype: 'textarea', fixed: false },
                col3]
            });
            var $table_bk = $('#table_bk').jqGrid_init({
                data: [<%=api.SerializeObject(sqlcmd.ToObjectList<MemberBankCardRow>("select * from MemberBank nolock where MemberID={0}", this.memberID) ?? Tools._null<List<MemberBankCardRow>>.value)%>][0],
                cmTemplate: { sortable: false }, editParams: { url: 'api' }, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false, headerclass: 'ui-widget-content',
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
            $table_bk.gridContainer().removeClass('ui-corner-all').css('border-bottom', 0);
            $('.btnAdd_bk').button({ icons: { primary: "ui-icon-plus" } }).removeClass('ui-state-default').css({ "border-width": 1 }).click(function () { $table_bk.addRow({ position: 'last' }); });
            <% } %>
            iframe_auto_height();
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <div class="ui-jqgrid ui-widget ui-widget-content" style="width: 900px;">
        <% if (User.Permissions[Permissions.Code.load_member_balance]) { %>
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
        <table id="table3">
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
        <table id="table4">
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
        <table id="table5">
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
    </div>
</asp:Content>
