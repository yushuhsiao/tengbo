<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.master" AutoEventWireup="true" Inherits="root_aspx" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.RootMasterPage.NavIndex = 6;
    }
</script>

<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="privatetable">
        <div class="body">
            <img src="<%=GetImage("~/image/bag1.jpg")%>" width="1032" height="225" />
            <img src="<%=GetImage("~/image/bag2.jpg")%>" width="1032" height="225" />
            <img src="<%=GetImage("~/image/bag3.jpg")%>" width="1032" height="225" />
            <img src="<%=GetImage("~/image/bag4.jpg")%>" width="1032" height="225" />
            <div class="txt"><a>立即体验包桌百家乐>></a></div>
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
