<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro6.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <table class="table2 font1" cellspacing="5">
        <tr>
            <td>
                <h3 class="font2">什么是生日礼金？</h3>
                生日礼金是【腾博会】为所有会员准备的生日大礼，只要是【腾博会】存款总额1000元以上会员都有资格获得生日礼金。
            </td>
            <td>
                <h3 class="font2">如何获得生日礼金？</h3>
                生日礼金无需申请，财务部门在审核成功后由系统自动发放。
            </td>
        </tr>
    </table>
    <div class="b"></div>
    <div class="a">
        <table class="table1" style="width: 90%; margin: 0 auto;">
            <tr>
                <th>会员级别</th>
                <th>生日礼金</th>
                <th>提款要求</th>
            </tr>
            <%--<tr>
                <td>VIP会员</td>
                <td>88</td>
                <td>直接提款</td>
            </tr>--%>
            <tr>
                <td>白银VIP</td>
                <td>188</td>
                <td>直接提款</td>
            </tr>
            <tr>
                <td>黄金VIP</td>
                <td>288</td>
                <td>直接提款</td>
            </tr>
            <tr>
                <td>钻石VIP</td>
                <td>888</td>
                <td>直接提款</td>
            </tr>
            <tr>
                <td>至尊VIP</td>
                <td>1288</td>
                <td>直接提款</td>
            </tr>

        </table>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>活动细则</h1>
        <ol class="rules">
            <li>为保护会员个人隐私，生日时间以会员第一次存款日期为生日时间。</li>
            <li>一年内会员只可领取一次生日礼金。</li>
            <li>每位玩家﹑每户﹑每一住址、每一电子邮箱地址﹑每一电话号码﹑相同支付方式 (相同借记卡/信用卡/银行账户) 及同一 IP地址（同一IP段将视为同一人）每年只能享受一次该优惠。</li>
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
          <div class="pro1_banner"><img src="image/pro6.jpg" width="995" height="235" /></div>
          <div class="pro1_box nome_m">
           <div class="pro3_tab">
              <div class="p_tab t_left">
              <p class="text1">什么是生日礼金？</p>
              <p>生日礼金是【腾博会】为所有会员准备的生日大礼，只要是【腾博会】存款总额1000元以上会员都有资格获得生日礼金。</p>
              </div>
              <div class="p_tab t_right">
              <p class="text1">如何获得生日礼金？</p>
              <p>生日礼金无需申请，财务部门在审核成功后由系统自动发放。</p>
              </div>
              <div class="B"></div>
              
            </div>
            
            <div style="margin:20px 0 20px 70px;">
                <table width="90%" border="1" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height:40px; border-collapse:collapse;">
                  <tr>
                    <td class="protext1">会员级别</td>
                    <td class="protext1">生日礼金</td>
                    <td class="protext1">提款要求</td>
                  </tr>
                  <tr>
                    <td class="protext2">VIP会员</td>
                    <td class="protext2">88</td>
                    <td class="protext2">直接提款</td>
                  </tr>
                  <tr>
                    <td class="protext2">白银VIP</td>
                    <td class="protext2">188</td>
                    <td class="protext2">直接提款</td>
                  </tr>
                  <tr>
                    <td class="protext2">黄金VIP</td>
                    <td class="protext2">288</td>
                    <td class="protext2">直接提款</td>
                  </tr>
                  <tr>
                    <td class="protext2">钻石VIP</td>
                    <td class="protext2">888</td>
                    <td class="protext2">直接提款</td>
                  </tr>
                  <tr>
                    <td class="protext2">至尊VIP</td>
                    <td class="protext2">1288</td>
                    <td class="protext2">直接提款</td>
                  </tr>
                  
                </table>
              </div>
            <div class="pro4_text">
              <p class="tit">活动细则</p>
              <ol class="promo_prompt">
              <li>为保护会员个人隐私，生日时间以会员第一次存款日期为生日时间。</li>              
              <li>一年内会员只可领取一次生日礼金。</li> 
              <li>每位玩家﹑每户﹑每一住址、每一电子邮箱地址﹑每一电话号码﹑相同支付方式(相同借记卡/信用卡/银行账户) 及同一 IP地址（同一IP段将视为同一人）每年只能享受
                  一次该优惠。</li>
              <li>该活动若存在文字理解的差异，最终解释权归【腾博会】所有。</li>
                            </ol> 
            </div>
          </div>
          
          
        </div>
      </div>
    </div>
  </div>--%>
</asp:Content>
