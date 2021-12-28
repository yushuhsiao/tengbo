<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataGrid.ascx.cs" Inherits="web.DataGrid" %>
<asp:PlaceHolder ID="ColumnsPlaceHolder" runat="server"></asp:PlaceHolder>
<div id="<%=this.ClientID%>">1</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#<%=this.ClientID%>').datagrid({
            a: 1,
            columns: [
                <% foreach (var col in this.GetColumns()) { %>
                <%=web.api.SerializeObject(col)%>,
                <% } %> ]
        });
        var grid = $('#<%=this.ClientID%>').datagrid('getInstance');
        //console.log(grid);
        <%
    %>
    });
</script>