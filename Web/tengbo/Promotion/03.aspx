<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro3.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <table class="table2 font1" cellspacing="5">
        <tr>
            <td>
                <h3 class="font2">什么是 VIP直通车？？</h3>
                VIP直通车是[腾博会]为回馈广大会员而新推出的活动。
            </td>
            <td>
                <h3 class="font2">如何获得VIP直通车资格？</h3>
                活动期间注册并存款（100元以上）的会员将立即晋升为VIP并享有VIP会员的专属特权。
            </td>
        </tr>
    </table>
    <div class="a">
        <h1>活动细则</h1>
        <ol class="rules">
            <li>【腾博会】所有新注册会员均可享受VIP直通车资格。</li>
            <li>存款成功后会员将直接晋升为VIP会员并享受VIP权利。</li>
            <li>活动时间截止至2014年8月28日。</li>
            <li>该活动若存在文字理解的差异，最终解释权归【腾博会】所有。</li>
        </ol>
    </div>

    <%--<div class="main_games">
    <div class="main_index">
      <div class="main_lights">
        <div class="lights_left"></div>
        <div class="lights_right"></div>
      </div>
      <div class="pro1_content">
        <div class="pro1">
          <div class="pro1_banner"><img src="image/pro3.jpg" width="995" height="235" /></div>
          <div class="pro1_box nome_m">
           <div class="pro3_tab">
              <div class="p_tab t_left">
              <p class="text1">什么是 VIP直通车？？</p>
              <p>VIP直通车是[腾博会]为回馈广大会员而新推出的活动。</p>
              </div>
              <div class="p_tab t_right">
              <p class="text1">如何获得VIP直通车资格？</p>
              <p>活动期间注册并存款（100元以上）的会员将立即晋升为VIP并享有VIP会员的专属特权。</p>
              </div>
              <div class="B"></div>
              
            </div>
            <div class="pro4_text">
              <p class="tit">活动细则</p>
              <ol class="promo_prompt">
              <li>【腾博会】所有新注册会员均可享受VIP直通车资格。</li>
              <li>存款成功后会员将直接晋升为VIP会员并享受VIP权利。</li>
              <li>活动时间截止至2014年2月28日。</li>
              <li>该活动若存在文字理解的差异，最终解释权归【腾博会】所有。</li>
                            </ol>
            </div>
          </div>
          
          
        </div>
      </div>
    </div>
  </div>--%>
</asp:Content>
