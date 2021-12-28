<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="web.page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var $setpassword = $('#setpassword').jqGrid_init({
                datatype: 'local', height: 'auto', sortable: false, rownumbers: false, autowidth: false, headervisible: false, editParams: { url: 'api', }, data: [{}],
                UpdateCommand: function (postData) { return { AdminSetPassword: postData } },
                beforeSelectRow: function () { return false; },
                colModel: [
                    { name: 'Action   ', label: '.', colType: 'Buttons' },
                    { width: 080, editable: false, formatter: function () { return '<%=lang["Password1"]%>' } }, { name: 'Password1', width: 080, editable: true, edittype: 'password' },
                    { width: 080, editable: false, formatter: function () { return '<%=lang["Password2"]%>' } }, { name: 'Password2', width: 080, editable: true, edittype: 'password' },
                    { width: 080, editable: false, formatter: function () { return '<%=lang["Password3"]%>' } }, { name: 'Password3', width: 080, editable: true, edittype: 'password' }
                ]
            });
            $setpassword.gridContainer().removeClass('ui-corner-all');
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <table id="setpassword">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow"    icon="ui-icon-key"   ><%=lang["Password0"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
    </table>
    <%--<table id="setpwd">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow" icon="ui-icon-pencil"><%=lang["Password0"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow" icon="ui-icon-disk"><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
    </table>--%>
</asp:Content>
