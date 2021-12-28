<%@ Control Language="C#" AutoEventWireup="true" Inherits="login_ascx" %>
<% if (this.Page == null) { %>
<link href="css/jquery-ui/jquery-ui.css" rel="stylesheet" />
<link href="css/style.min.css" rel="stylesheet" />
<script type="text/javascript" src="js/jquery.js"></script>
<script type="text/javascript" src="js/jquery-ui.js"></script>
<script type="text/javascript" src="js/script.js"></script>
<% } %>
<% if (this.Member==null) { %>
<table class="user-state-guest">
    <tr>
        <td><a href="<%=ResolveUrl("~/Register.aspx")%>" class="register_link"></a></td>
        <td>
            <div class="login1"><label>账号：</label><input class="input1" name="n1" type="text" /></div>
            <div class="login2"><label>密码：</label><input class="input1" name="n2" type="password" /></div>
        </td>
        <td>
            <button class="login" onclick="btnLogin_click('<%=ResolveUrl("~/login.aspx")%>');"></button>
            <%--<a class="recover tubiao" onclick="live800_chat();">忘记密码？</a>--%>
            <a class="recover tubiao" onclick="$(this).loadBox(443, 273, '<%=ResolveUrl("~/Recovery.aspx")%>', '<%=ResolveUrl("~/")%>');">忘记密码？</a>
        </td>
    </tr>
</table>
<div class="login_err ui-corner-all" style="display: none;" onclick="login_error.hide()" ></div>
<% if (this.errmsg != null) { %><script type="text/javascript" > $(document).ready(function () { login_error.show('<%=errmsg%>'); }); </script><% } %>
<div style="display:none;">
    <form id="playgame01" target="_blank" method="post" action="http://777.ddt518.com/cl/?module=System&method=Live"     ></form>
    <form id="playgame02" target="_blank" method="post" action="http://777.ddt518.com/cl/?module=System&method=Game"     ></form>
    <form id="playgame03" target="_blank" method="post" action="http://777.ddt518.com/cl/?module=System&method=Ltlottery"></form>
    <form id="playgame04" target="_blank" method="post" action="http://777.ddt518.com/cl/?module=System&method=ball"     ></form>
</div>
<% } else { %>
<table class="user-state-login" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <div class="user_account_div">欢迎您！<span class="user_account"><%=this.Member.ACNTString()%></span></div>
            <div class="user_balance_div">会员额度：<span class="user_balance"><%=this.Member.BalanceString()%></span></div>
        </td>
        <td class="user_panel">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td><a class="button1" href="<%=ResolveUrl("~/MemberCenter")                %>">会员中心</a></td>
                    <td><a class="button1" href="<%=ResolveUrl("~/MemberCenter/Deposit.aspx")   %>">我要存款</a></td>
                    <td><a class="button1" href="<%=ResolveUrl("~/MemberCenter/Withdrawal.aspx")%>">我要取款</a></td>
                </tr>
                <tr>
                    <td><a class="button1" href="<%=ResolveUrl("~/MemberCenter/GameTran.aspx")  %>">户内转账</a></td>
                    <td><a class="button1" <%--href="<%=ResolveUrl("~/MemberCenter/UserMsg.aspx")%>"--%>>消&nbsp;息</a></td>
                    <td><a class="button1" onclick="btnLogout_click('<%=ResolveUrl("~/login.aspx")%>','<%=ResolveUrl("~/")%>');">退出</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div style="display: none;">
    <form id="playgame01" target="_blank" method="post" action="<%=ResolveUrl("~/Game/bbin.ashx")%>"><input name="page_site" type="text" value="<%=extAPI.bbin.page_site.live%>"   /></form>
    <form id="playgame02" target="_blank" method="post" action="<%=ResolveUrl("~/Game/bbin.ashx")%>"><input name="page_site" type="text" value="<%=extAPI.bbin.page_site.game%>"   /></form>
    <form id="playgame03" target="_blank" method="post" action="<%=ResolveUrl("~/Game/bbin.ashx")%>"><input name="page_site" type="text" value="<%=extAPI.bbin.page_site.Lottery%>"/></form>
    <form id="playgame04" target="_blank" method="post" action="<%=ResolveUrl("~/Game/bbin.ashx")%>"><input name="page_site" type="text" value="<%=extAPI.bbin.page_site.ball%>"   /></form>
</div>
<% } %>
<div style="display: none;">
    <form id="playgame05" target="_blank" method="post" action="<%=ResolveUrl("~/Game/HG.aspx" )%>"><input type="text" name="trial" /></form>
    <form id="playgame11" target="_blank" method="post" action="<%=ResolveUrl("~/Game/ag1.aspx")%>"><input type="text" name="trial" /></form>
    <form id="playgame12" target="_blank" method="post" action="<%=ResolveUrl("~/Game/ag2.aspx")%>"><input type="text" name="trial" /></form>
    <form id="playgame13" target="_blank" method="post" action="<%=ResolveUrl("~/Game/ag3.aspx")%>"><input type="text" name="trial" /></form>
</div>