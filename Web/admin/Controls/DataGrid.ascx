<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataGrid.cs" Inherits="web.Controls._DataGrid" %>
<div id="<%=this.ClientID%>"></div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#<%=this.ClientID%>').datagrid({
            <%=prop("toolbar: '#{0}', ", this.DataGrid._Toolbar.ID)
            %><%=prop("width : '{0}',", this.DataGrid.Width)%><%=prop("height : '{0}',", this.DataGrid.Height)
            %><%=prop("shrinkToFit : {0}, ", this.DataGrid.ShrinkToFit)%>
            columns: [
                <% foreach (var col in this.GetColumns())
                   { %>
                <%=web.api.SerializeObject(col)%>,
                <% } %>]
        });
        var grid = $('#<%=this.ClientID%>').datagrid('getInstance');
        //console.log(grid);
        <%
    %>
    });
</script>