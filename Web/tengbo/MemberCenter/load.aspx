<%@ Page Language="C#" AutoEventWireup="true" Inherits="SitePage" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(300);
        string nav_index = Request.Form["nav_index"];
        string content_index = Request.Form["content_index"];
        switch (nav_index)
        {
            case "05":
                if ((content_index == "05_1") || (content_index == "05_2"))
                    nav_index = content_index;
                break;
        }
        this.Controls.Add(LoadControl(string.Format("~/MemberCenter/{0}.ascx", nav_index)));
    }
</script>
<html><body></body></html>