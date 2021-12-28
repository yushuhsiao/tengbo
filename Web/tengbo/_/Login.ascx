<%@ Control Language="C#" AutoEventWireup="true" Inherits="Login_ascx" %>
<% if (this.Page == null) { %>
<link href="css/jquery-ui/jquery-ui.css" rel="stylesheet" />
<link href="css/style.min.css" rel="stylesheet" />
<script type="text/javascript" src="js/jquery.js"></script>
<script type="text/javascript" src="js/jquery-ui.js"></script>
<script type="text/javascript" src="js/jquery.blockUI.js"></script>
<script type="text/javascript" src="js/script.js"></script>
<% } %>
<% if (this.Member==null) { %>
<table class="user-state-guest">
    <tr>
        <td><a href="<%=ResolveClientUrl("~/Register.aspx")%>" class="register_link"></a></td>
        <td>
            <div class="login1"><label>账号：</label><input class="input1" name="n1" type="text" /></div>
            <div class="login2"><label>密码：</label><input class="input1" name="n2" type="password" /></div>
        </td>
        <td>
            <button class="login" onclick="btnLogin_click('<%=ResolveClientUrl("~/login.aspx")%>');"></button>
            <a class="recover tubiao">忘记密码？</a>
        </td>
    </tr>
</table>
<div class="login_err ui-corner-all" style="display: none;" onclick="login_error.hide()" ></div>
<% if (this.errmsg != null) { %><script type="text/javascript" > $(document).ready(function () { login_error.show('<%=errmsg%>'); }); </script><% } %>
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
                    <td><a class="button1" href="<%=ResolveClientUrl("~/MemberCenter")                %>">会员中心</a></td>
                    <td><a class="button1" href="<%=ResolveClientUrl("~/MemberCenter/Deposit.aspx")   %>">我要存款</a></td>
                    <td><a class="button1" href="<%=ResolveClientUrl("~/MemberCenter/Withdrawal.aspx")%>">我要取款</a></td>
                </tr>
                <tr>
                    <td><a class="button1" href="<%=ResolveClientUrl("~/MemberCenter/GameTran.aspx")  %>">户内转账</a></td>
                    <td><a class="button1" href="<%=ResolveClientUrl("~/MemberCenterUserMsg.aspx")    %>">消&nbsp;息</a></td>
                    <td><a class="button1" onclick="btnLogout_click('<%=ResolveClientUrl("~/login.aspx")%>','<%=ResolveClientUrl("~/")%>');">退出</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div style="display: none;">
    <form id="playgame_<%=BU.GameID.HG%>" target="_blank" method="post" action="<%=ResolveClientUrl("~/Game/HG.aspx")%>"></form>
    <form id="playgame_<%=BU.GameID.BBIN%>" target="_blank" method="post" action="<%=ResolveClientUrl("~/Game/bbin.ashx")%>"></form>
</div>
<% } %>