<%@ Page Language="C#" AutoEventWireup="true" Inherits="SitePage" %>

<%@ Register Src="~/Login.ascx" TagPrefix="uc1" TagName="Login" %>

<!DOCTYPE html>
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = -1441;
        Response.Cache.SetExpires(DateTime.MinValue);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetNoServerCaching();
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml"><body><uc1:Login runat="server" /></body></html>
