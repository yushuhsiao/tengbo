<%@ Page Title="" Language="C#" MasterPageFile="Help.master" AutoEventWireup="true" Inherits="HelpPage" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.HelpIndex = 4;
    }
</script>

<asp:Content ContentPlaceHolderID="help2" runat="server">
    <div class="help-title"><div class="title0">联系我们</div></div>
</asp:Content>
