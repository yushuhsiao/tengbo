<%@ Page Language="C#" MasterPageFile="UserDetail.master" AutoEventWireup="true" Inherits="web.page" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<script runat="server">

    int adminID;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.adminID = Request.QueryString["id"].ToInt32() ?? 0;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(document).ready(function () { <%
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            AdminRow row = AdminRow.GetAdmin(sqlcmd, this.adminID, null, null, "*");
            string rowdata = api.SerializeObject(row); %>

            var col1 = { name: 'Action      ', width: 100, colType: 'Buttons' };
            var col2 = { name: 'ID          ', width: 100, colType: 'ID' };
            var col3 = { name: '_fill', label: '.', fixed: false };

            $.fn._init = function (o, css) {
                var $t = this.jqGrid_init($.extend({
                    datatype: 'local', height: 'auto', sortable: false, rownumbers: false, shrinkToFit: true, headervisible: false, data: [<%=rowdata%>],
                    onSelectRow: function (rowid, status, e) { $($(this).getInd(rowid, true)).removeClass('ui-state-highlight'); },
                    UpdateCommand: function (postData) { return { AdminUpdate: postData } },
                    RowResponse: function (res, rowid, row) { sendMessage('AdminRowData', { ID: rowid, Balance: row.Balance }); },
                    cmTemplate: { sortable: false, fixed: true }, editParams: { url: 'api', },                    
                }, o));
                var c = $t.gridContainer();
                c.removeClass('ui-corner-all');
                if (css == 1)
                    c.css({ 'border-left': 0, 'border-bottom': 0, 'border-right': 0 });
                else
                    c.css({ 'border': 0 });
                $('.ui-th-column', c).removeClass('ui-state-default').addClass('ui-widget-content');
                return $t;
            }

            var $table1 = $('#table1')._init({ colModel: [col1, col2, { name: 'Password    ', width: 100, editable: true }, col3] });
            <%
        }
            %>
            iframe_auto_height();
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <div class="ui-jqgrid">
        <table cellpadding="0" cellspacing="0" class="ui-jqgrid-view ui-widget-content" style="width:800px; border-width:0; border-left-width:1px; border-bottom-width: 2px; border-right-width: 2px;">
            <tr>
                <td style="width: 220px;">
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
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-bottom: 2px;">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
