<%@ Page Language="C#" MasterPageFile="UserDetail.master" AutoEventWireup="true" Inherits="web.page" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<script runat="server">

    int adminID;
    string rowdata;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.adminID = Request.QueryString["id"].ToInt32() ?? 0;
        rowdata = api.SerializeObject(AdminAuthRow.GetRow(null, this.adminID) ?? new AdminAuthRow() { AdminID = adminID, Locked = Locked.Locked });
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function formatXml(xml) {
            var formatted = '';
            var reg = /(>)(<)(\/*)/g;
            xml = xml.replace(reg, '$1\r\n$2$3');
            var pad = 0;
            jQuery.each(xml.split('\r\n'), function (index, node) {
                var indent = 0;
                if (node.match(/.+<\/\w[^>]*>$/)) {
                    indent = 0;
                } else if (node.match(/^<\/\w/)) {
                    if (pad != 0) {
                        pad -= 1;
                    }
                } else if (node.match(/^<\w[^>]*[^\/]>.*$/)) {
                    indent = 1;
                } else {
                    indent = 0;
                }

                var padding = '';
                for (var i = 0; i < pad; i++) {
                    padding += '  ';
                }

                formatted += padding + node + '\r\n';
                pad += indent;
            });

            return formatted;
        }

        $(document).ready(function () {
            var data = [<%=rowdata%>];
            var $table1 = $('#table1').jqGrid_init({
                autowidth: false, datatype: 'local', height: 'auto', sortable: false, rownumbers: false, shrinkToFit: true, headervisible: true, headerclass: 'ui-widget-content', data: data,
                onSelectRow: function (rowid, status, e) { $($(this).getInd(rowid, true)).removeClass('ui-state-highlight'); },
                UpdateCommand: function (postData) { return { AdminAuthRowCommand: postData } },
                RowResponse: function (res, rowid, row) { if (row.rsakey) $('.rsakey').text(formatXml(row.rsakey)); },
                cmTemplate: { sortable: false, fixed: true }, editParams: { url: 'api', },
                colModel: [
                    { name: 'Action    ', width: 100, colType: 'Buttons' },
                    { name: 'AdminID   ', width: 100, colType: 'ID' },
                    { name: 'keysize   ', label: 'KeySize   ', width: 100, editable: true },
                    { name: 'Locked    ', label: 'State         ', colType: 'Locked' },
                    { name: 'header    ', label: 'Header Name   ', width: 100, editable: true },
                    { name: 'idstr     ', label: 'Header Value  ', width: 100, editable: true },
                  //{ name: 'rsakey    ', label: '.', width: 100, hidden: true },
                    { name: 'ExpireTime', label: 'ExpireTime', colType: 'DateTime2' },
                    { name: 'CreateTime', label: 'CreateTime', colType: 'DateTime2' },
                    { name: 'CreateUser', label: 'CreateUser', colType: 'ACNT2' },
                    { name: 'ModifyTime', label: 'ModifyTime', colType: 'DateTime2' },
                    { name: 'ModifyUser', label: 'ModifyUser', colType: 'ACNT2' }
                ]
            });
            $table1.gridContainer().removeClass('ui-corner-all').css({ 'border': 0 });
            $table1[0].p.RowResponse(null, null, data[0])
            iframe_auto_height();
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <div class="ui-jqgrid">
        <table cellpadding="0" cellspacing="0" class="ui-jqgrid-view ui-widget-content" style="width:100%; border-width:0px 0px 1px 1px;">
            <tr>
                <td style="width: 220px;">
                    <table id="table1">
                        <tr class="colModel">
                            <td name="Action">
                                <span property="action">
                                    <div class="edithide" action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                                    <div class="editshow" action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-bottom: 0px;">
                    <pre><span class="rsakey"></span></pre>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
