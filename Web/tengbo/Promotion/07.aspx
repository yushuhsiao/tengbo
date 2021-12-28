<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro7.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <table class="table2 font1" cellspacing="5">
        <tr>
            <td>
                <h3 class="font2">什么是晋级奖金？</h3>
                晋级奖金是【腾博会】提供给会员晋升级别时的奖励。
            </td>
            <td>
                <h3 class="font2">如何领取晋级奖金？</h3>
                达到条件的会员在晋级成功后当天由财务部门审核后发放，无需申请。
            </td>
        </tr>
    </table>
    <div class="b"></div>
    <div class="a">
        <table class="table1" style="width: 90%; margin: 0 auto;">
            <tr>
                <th>会员级别</th>
                <th>晋级投注额</th>
                <th>晋级奖金</th>
            </tr>
            <%--<tr>
                <td>VIP会员</td>
                <td>≥50万</td>
                <td>188元</td>
            </tr>--%>
            <tr>
                <td>白银VIP</td>
                <td>≥300万</td>
                <td>388元</td>
            </tr>
            <tr>
                <td>黄金VIP</td>
                <td>≥500万</td>
                <td>888元</td>
            </tr>
            <tr>
                <td>钻石VIP</td>
                <td>≥800万</td>
                <td>1288元</td>
            </tr>
            <tr>
                <td>至尊VIP</td>
                <td>≥1200万</td>
                <td>1888元</td>
            </tr>
        </table>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>活动细则</h1>
        <ol class="rules">
            <li>所有会员在达到条件后均可获得此晋级奖金。</li>
            <li>结算时间：北京时间每周一下午18点以前。</li>
            <li>会员在以上结算时间内如达到晋级投注要求，系统将在每周一下午18点以前自动为会员提升级别。</li>
            <li>该晋级奖金需达到3倍有效投注额即可申请提款。</li>
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
          <div class="pro1_banner"><img src="image/pro7.jpg" width="995" height="235" /></div>
          <div class="pro1_box nome_m">
           <div class="pro3_tab">
              <div class="p_tab t_left">
              <p class="text1">什么是晋级奖金？</p>
              <p>晋级奖金是【腾博会】提供给会员晋升级别时的奖励。</p>
              </div>
              <div class="p_tab t_right">
              <p class="text1">如何领取晋级奖金？</p>
              <p>达到条件的会员在晋级成功后当天由财务部门审核后发放，无需申请。</p>
              </div>
              <div class="B"></div>
              
            </div>
            
            <div style="margin:20px 0 20px 70px;">
                <table width="90%" border="1" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height:40px; border-collapse:collapse;">
                  <tr>
                    <td class="protext1">会员级别</td>
                    <td class="protext1">晋级投注额</td>
                    <td class="protext1">晋级奖金</td>
                  </tr>
                  <tr>
                    <td class="protext2">VIP会员</td>
                    <td class="protext2">≥50万</td>
                    <td class="protext2">188元</td>
                  </tr>
                  <tr>
                    <td class="protext2">白银VIP</td>
                    <td class="protext2">≥100万</td>
                    <td class="protext2">388元</td>
                  </tr>
                  <tr>
                    <td class="protext2">黄金VIP</td>
                    <td class="protext2">≥300万</td>
                    <td class="protext2">888元</td>
                  </tr>
                  <tr>
                    <td class="protext2">钻石VIP</td>
                    <td class="protext2">≥800万</td>
                    <td class="protext2">1288元</td>
                  </tr>
                  <tr>
                    <td class="protext2">至尊VIP</td>
                    <td class="protext2">≥1200万</td>
                    <td class="protext2">1888元</td>
                  </tr>
                  
                </table>
              </div>
            <div class="pro4_text">
              <p class="tit">活动细则</p>
              <ol class="promo_prompt">
              <li>所有会员在达到条件后均可获得此晋级奖金。</li>              
              <li>结算时间：北京时间每周一中午12点至下周一中午12点。</li>
              <li>会员在以上结算时间内如达到晋级投注要求，系统将在每周二下午14点以前自动为会员提升级别。</li>              
              <li>该晋级奖金需达到3倍有效投注额即可申请提款。</li>
              <li>该活动若存在文字理解的差异，最终解释权归【腾博会】所有。</li>
                            </ol>            
            </div>
          </div>
          
          
        </div>
      </div>
    </div>
  </div>--%>
</asp:Content>
