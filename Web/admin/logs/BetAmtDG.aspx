<%@ Page Title="" Language="C#" MasterPageFile="~/logs/logs.master" AutoEventWireup="true" Inherits="web.page" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="web" %>

<script runat="server">
    bool editable;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.editable = this.User.Permissions[Permissions.Code.log_betamtdg, Permissions.Flag.Write];
    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        var $table;
        $(document).ready(function (ind, rowid) {

            var enum2 = {
                <%=enumlist<BU.LogType>("logTypes")%> 
                <%=serializeEnum(",gameid", web.game.Names)%>
                <%=serializeEnum(",gameid_s", web.game.Names_Active)%>
            };
            enum2.gameid_s[0] = '<%=lang["MainAccount"]%>';

            <% if (editable) { %> var editable = true; <% } else {%> var editable = false; <% } %>

            $table = $('#table1').jqGrid_init({
                pager: true, loadonce: false, sortname: 'ACTime',
                cmTemplate: { editable: false, editonce: false },
                SelectCommand: function (postData) { return { GameLog_BetAmtDG_Select: postData } },
                InsertCommand: function (postData) { return { GameLog_BetAmtDG_Insert: postData } },
                UpdateCommand: function (postData) { return { GameLog_BetAmtDG_Update: postData } },
                DeleteCommand: function (postData) { return { GameLog_BetAmtDG_Delete: postData } },

                colModel: [
                    { name: 'Action      ', label: '<%=lang["colAction      "]%>', colType: 'Buttons', hidden: false },
                    { name: 'sn          ', label: '<%=lang["colID          "]%>', colType: 'ID', hidden: true },
                    { name: 'ACTime      ', label: '<%=lang["colACTime      "]%>', width: 080, editable: editable, sorttype: 'date', formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd', formatNaN: 'N/A' }, editoptions: { defaultValue: '<%=DateTime.Now.AddDays(-1).ToACTime().ToString(sqltool.DateFormat)%>' } },
                    { name: 'CorpID      ', label: '<%=lang["colCorpID      "]%>', colType: 'CorpID' },
                    { name: 'ParentID    ', label: '<%=lang["colParentID    "]%>', width: 080, sorttype: 'int ', hidden: true },
                    { name: 'ParentACNT  ', label: '<%=lang["colParentACNT  "]%>', width: 080, sorttype: 'text', editable: false, search: true },
                    { name: 'MemberID    ', label: '<%=lang["colMemberID    "]%>', width: 080, sorttype: 'int ', hidden: true },
                    { name: 'ACNT        ', label: '<%=lang["colACNT        "]%>', width: 080, sorttype: 'text', editable: editable, search: true },
                    { name: 'Name        ', label: '<%=lang["colName        "]%>', width: 080, sorttype: 'text', search: true },
                    { name: 'GameID      ', label: '<%=lang["colGameID      "]%>', width: 080, sorttype: 'int ', editable: editable, formatter: 'select', edittype: 'select', editoptions: { value: enum2.gameid_s }, search: true, stype: 'select', searchoptions: { value: enum2.gameid_s, defaultValue: 255, nullKey: 255, nullValue: '--', } },
                    { name: 'GameType    ', label: '<%=lang["colGameType    "]%>', width: 080, sorttype: 'text', editable: editable, search: true },
                    { name: 'BetAmount   ', label: '<%=lang["colBetAmount   "]%>', width: 100, colType: 'Money', editable: editable },
                    { name: 'BetAmountAct', label: '<%=lang["colBetAmountAct"]%>', width: 100, colType: 'Money', editable: editable },
                    { name: 'Payout      ', label: '<%=lang["colPayout      "]%>', width: 100, colType: 'Money', editable: editable },
                    { name: 'CreateTime  ', label: '<%=lang["colCreateTime  "]%>', colType: 'DateTime2' },
                    { name: 'CreateUser  ', label: '<%=lang["colCreateUser  "]%>', colType: 'ACNT2' },
                ]
            });

            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <table id="table1">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide"   action="editRow"    icon="ui-icon-pencil" disabled="disabled"><%=lang["Actions_Edit"]%></div>
                    <div class="edithide"   action="delRow"     icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="saveRow"    icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
        <tr class="grid-option">
            <td>
                <div name="nav1">
                    <button action="toggleSearch" icon="ui-icon-search" ><%=lang["btnSearch"]%></button>
                    <button action="reloadGrid"   icon="ui-icon-refresh"><%=lang["btnRefresh"]%></button>
                    <button action="addRow"       icon="ui-icon-plus"   ><%=lang["btnAdd"]%></button>
                </div>
                <div name="nav2" class="ui-widget-content" style=""></div>
            </td>
        </tr>
    </table>
</asp:Content>

