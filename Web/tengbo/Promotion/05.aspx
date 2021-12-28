<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro5.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <table class="table2 font1" cellspacing="5">
        <tr>
            <td>
                <h3 class="font2">什么是复活礼金？</h3>
                复活礼金是【腾博会】发给您的闯关红利奖金，在负盈利的情况下又可以复活重新进行游戏。
            </td>
            <td>
                <h3 class="font2">如何获得复活礼金？</h3>
                按照会员每周的盈利情况而定，会员每周负盈利800元及以上即可申请，单个用户每周最高1000元。
            </td>
        </tr>
    </table>
    <div class="b"></div>
    <center>
        <img src="<%=GetImage("~/image/pro5_img.jpg")%>" width="946" height="97" /></center>
    <div class="b"></div>
    <div class="a">
        <h1>复活礼金计算方式</h1>
        <table class="table1" style="width: 90%;">
            <tr>
                <th>周盈利</td>
                <th>复活礼金比例</td>
                <th>提款要求</td>
            </tr>
            <tr>
                <td>一周负盈利800及以上</td>
                <td>所有会员3%，最高1000</td>
                <td>8倍流水</td>
            </tr>

        </table>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>活动细则</h1>
        <ol class="rules">
              <li>【腾博会】所有会员均可享受欢乐复活礼金。</li>
              <li>复活礼金结算周期：为每周一中午12点到下周一中午11点59分59秒。</li>
              <li>复活礼金按照会员每周的盈利情况进行计算。会员一周负盈利800及以上即可获得欢乐复活礼金，每周返还负盈利的3%，最高可达1000元。</li> 
              <li>复活礼金8倍流水后即可提现。</li>
              <li>每位玩家﹑每户﹑每一住址、每一电子邮箱地址﹑每一电话号码﹑相同支付方式(相同借记卡/信用卡/银行账户) 及同一 IP地址（同一IP段将视为同一人）每周只能享受一次优惠。</li>              
              <li>该活动若存在文字理解的差异，最终解释权归【腾博会】所有。 在文字理解的差异，最终解释权归【腾博会】所有。</li>               
              <li>活动时间：截止至2014年8月28日。</li>   
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
          <div class="pro1_banner"><img src="image/pro5.jpg" width="995" height="235" /></div>
          <div class="pro1_box nome_m">
           <div class="pro3_tab">
              <div class="p_tab t_left">
              <p class="text1">什么是复活礼金？</p>
              <p>复活礼金是【腾博会】发给您的闯关红利奖金，在负盈利的情况下又可以复活重新进行游戏。</p>
              </div>
              <div class="p_tab t_right">
              <p class="text1">如何获得复活礼金？</p>
              <p>按照会员每周的盈利情况而定，会员每周负盈利800元以上即可申请，单个用户每周最高1000元。</p>
              </div>
              <div class="B"></div>
              
            </div>
            
            <div class="pro5_img"><img src="image/pro5_img.jpg" width="946" height="97" /></div>
            <p class="tit">&nbsp;&nbsp; 复活礼金计算方式</p>
            <div style="margin:20px 0 20px 30px;">
                <table width="90%" border="1" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height:40px; border-collapse:collapse;">
                  <tr>
                    <td class="protext1">周盈利</td>
                    <td class="protext1">复活礼金比例</td>
                    <td class="protext1">提款要求</td>
                  </tr>
                  <tr>
                    <td class="protext2">一周负盈利800及以上</td>
                    <td class="protext2">所有会员3%，最高1000</td>
                    <td class="protext2">8倍流水</td>
                  </tr>
                  
                </table>
              </div>
            <div class="pro4_text">
              <p class="tit">活动细则</p>
              <ol class="promo_prompt">
              <li>【腾博会】所有会员均可享受欢乐复活礼金。</li>
              <li>复活礼金按照会员每周的盈利情况进行计算。周负盈利 =周派彩+周红利 会员一周负盈利800及以上即可获得欢乐复活礼金，每周返还负盈利的3%，最高可达1000元。</li> 
              <li>复活礼金8倍流水后即可提现。</li>
              <li>每位玩家﹑每户﹑每一住址、每一电子邮箱地址﹑每一电话号码﹑相同支付方式(相同借记卡/信用卡/银行账户) 及同一 IP地址（同一IP段将视为同一人）每周只能享受一次优惠。</li>              
              <li>该活动若存在文字理解的差异，最终解释权归【腾博会】所有。 在文字理解的差异，最终解释权归【腾博会】所有。</li>               
              <li>活动时间：截止至2014年2月28日。</li>   
                            </ol>           
            </div>
          </div>
          
          
        </div>
      </div>
    </div>
  </div>--%>
</asp:Content>
