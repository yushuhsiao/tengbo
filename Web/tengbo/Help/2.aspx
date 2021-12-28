<%@ Page Title="" Language="C#" MasterPageFile="Help.master" AutoEventWireup="true" Inherits="HelpPage" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.HelpIndex = 2;
    }
</script>

<asp:Content ContentPlaceHolderID="help2" runat="server">
    <div class="help-title"><div class="title0">营业执照</div></div>
    <p>
       <img src="<%=GetImage("~/image/ncgac.png")%>" alt="" />
    </p>
</asp:Content>
