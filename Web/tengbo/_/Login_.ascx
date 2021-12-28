<%@ Control Language="C#" AutoEventWireup="true" Inherits="SiteControl" %>

<% if (this.Member==null) { %>
<table style="width:100%; height:100%;" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td style="width:110px; text-align: center; vertical-align: central;"><a href="Register.aspx"><img src="image/open.gif" width="100" height="51" /></a></td>
        <td style="width: 180px;">
            <div style="white-space: nowrap;">账号：<input tabindex="1" class="input_login" id="login_username" type="text" /></div>
            <div style="white-space: nowrap;">密码：<input tabindex="2" class="input_login" id="login_password" type="password" /></div>
            <div style="padding: 5px 9px 5px 9px; position: fixed; border: 1px solid #996600; background-color: #FFFF99; color: #996600; z-index: 1000; margin-left: 100px; opacity: .8; display: none;" class="ui-corner-all" >密碼錯誤</div>
        </td>
        <td style="width: 90px;">
            <input style="padding-bottom: 10px;" tabindex="3" onclick="btnLogin_click();" type="image" value="提交" src="image/login.gif" />
            <a class="forgot_pass">忘记密码？</a>
        </td>
    </tr>
</table>
<%--<table class="login_body" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td width="110" rowspan="2"><a href="Register.aspx"><img src="image/open.gif" width="100" height="51" /></a></td>
        <td class="white">账号：<input tabindex="1" type="text" class="input_login" id="login_username" /></td>
        <td colspan="2"><input tabindex="3" onclick="btnLogin_click();" type="image" value="提交" src="image/login.gif" /></td>
    </tr>
    <tr>
        <td class="white">密码：<input tabindex="2" type="password" class="input_login" id="login_password" /></td>
        <td><a class="forgot_pass">忘记密码？</a></td>
    </tr>
</table>--%>
<% } else { %>
<table class="login_show" border="1" cellpadding="0" cellspacing="0">
    <tr>
        <td class="login_show_a">欢迎您！<span id="userinfo1"><%=this.Member.ACNTString()%></span></td>
        <td class="login_show_c" rowspan="2">
            <a class="button01" style="float: left;" href="MemberCenter.aspx">会员中心</a>
            <a class="button01" style="float: left;" href="Deposit.aspx">我要存款</a>
            <a class="button01" style="float: left;" href="Withdrawal.aspx">我要取款</a>
            <br />
            <a class="button01" style="float: left;" href="GameTran.aspx">户内转账</a>
            <a class="button01" style="float: left;" href="UserMsg.aspx">消&nbsp; 息</a>
            <a class="button01" style="float: left;" onclick="btnLogout_click();">退出</a>
        </td>
    </tr>
    <tr>
        <td class="login_show_b">会员额度：<span id="userinfo2"><%=this.Member.BalanceString()%></span></td>
    </tr>
</table>
<% } %>