<%@ Page Title="" Language="C#" MasterPageFile="~/master/Cdefault.master" AutoEventWireup="true" Inherits="CLogin_aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="window">
      <div class="chuna_box">
        <div class="chuna_com" >
          <div class="chuna_dl">
            <table width="100%" border="0" align="center" cellpadding="2" cellspacing="0" class="chuna login_panel">
              <tr>
                <td width="100" class="chuna_tit">登录账号：</td>
                <td><input class="input_chuna" name="n1" type="text" /></td>
              </tr>
              <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td width="100" class="chuna_tit">账号密码：</td>
                <td><input class="input_chuna" name="n2" type="password" /></td>
              </tr>
              <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td>&nbsp;</td>
                <td><input name="button" type="button" class="bnt_dl" id="button" value="登  录" onclick="btnCLogin_click('<%=ResolveUrl("~/CLoginHandler.aspx")%>', '<%=ResolveUrl("~/MemberCenter/Cdefault.aspx")%>');" />&nbsp;&nbsp;<label id="lab_ErrMsg" style="color:#f5ff00; font-weight: bold;"></label></td>
              </tr>
            </table>
          </div>
        </div>
      </div>
    </div>
</asp:Content>
