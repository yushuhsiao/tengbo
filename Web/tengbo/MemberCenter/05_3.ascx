<%@ Control Language="C#" AutoEventWireup="true" Inherits="SiteControl" %>

<div class="user_center_content" index="05" reload="true" style="display: <%=this.css_display%>;">
    <% 
        string message = string.Empty;
        try
        {
            if (Request.QueryString["status"].ToInt32() == 1)
            {
                message = "恭喜您，您已成功支付了 <label style='color: red;font-size: 22px;'>" + Request.QueryString["amt"] + "</label> 元!<br /><br /><label style='font-size: 18px;'>本次存款额度已经添加至您的主账户，祝您游戏愉快！</label>";
            }
            else
            {
                message = "很抱歉，您本次支付失败！<br /><br />请您尝试重新支付。如有疑问，请您联系在线客服！";
            }
            
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }    
    %>
    <center style="font-size: 1.25em; color: yellow; margin-top: 100px;font-size: 20px;"><%= message %></center>
</div>