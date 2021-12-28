<%@ Control Language="C#" AutoEventWireup="true" Inherits="SiteControl" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        string content_index = Request.Form["content_index"];
        switch (content_index)
        {
            case "05_1":
            case "05_2":
                break;
            default: content_index = "05_0"; break;
        }
        this.Controls.Add(LoadControl(string.Format("~/MemberCenter{0}.ascx", content_index)));
    }
</script>