<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table8a { width: 308px; }
        .table8a td { height: 88px; vertical-align: central; }
        .table8a td div { font-size: 14px; line-height: 25px; margin-left: 130px; padding-right: 10px; }
        .table8a td span { color: #ff0000; }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro8.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <table class="table2 font1" cellspacing="5">
        <tr>
            <td>
                <h3 class="font2">什么是周周红利？</h3>
                【腾博会】会员只需要在一周时间内（每周一中午12：00-下周一中午12：00）有效投注额≥10万即可获得返奖红利，<span style="color: #F00;">最高8888元。</span>
            </td>
            <td>
                <h3 class="font2">周周红利如何领取？</h3>
                投注额越高，红利越多。无需申请，系统自动发放。
            </td>
        </tr>
    </table>
    <div class="b"></div>
    <table style="width: 100%;">
        <tr>
            <td style="vertical-align: top;">
                <div class="a">
                    <h1>周周红利邀您分享：</h1>
                    <table class="table1" style="width: 90%;">
                        <tr>
                            <th>一周有效投注额</th>
                            <th>周周红利</th>
                        </tr>
                        <tr>
                            <td>≥10万</td>
                            <td>38</td>
                        </tr>
                        <tr>
                            <td>≥50万</td>
                            <td>188</td>
                        </tr>
                        <tr>
                            <td>≥100万</td>
                            <td>388</td>
                        </tr>
                        <tr>
                            <td>≥1000万</td>
                            <td>8888</td>
                        </tr>
                    </table>
                </div>
                <div class="b"></div>
                <div class="a">
                    <h1>活动说明</h1>
                    <ol class="rules">
                        <li>【腾博会】所有会员均可享受“周周红利”奖金。</li>
                        <li>会员只需在活动期间的一周时间内有效投注额≥10万即可获得红利（投注额越多，红利越高）。</li>
                        <li>该红利礼金将在每周四下午16点前由系统自动发放到会员的游戏账户中。</li>
                        <li>该红利礼金适用于所有游戏，只需一倍流水即可提现。</li>
                        <li>“首存优惠”和“周周红利“优惠不能在同一周且不能同时享受。</li>
                        <li>活动时间：截止至2014年8月28日。</li>
                        <li>本优惠若发现恶意套利者【腾博会】有权扣除红利甚至没收本金</li>
                        <li>该活动若存在文字理解的差异，则最终解释权归【腾博会】所有。</li>
                    </ol>
                </div>
            </td>
            <td style="vertical-align: top;">
                <h1>领取流程：</h1>
                <table class="table8a" cellpadding=10>
                    <tr><td style="background: url(<%=GetImage("~/image/pro_8img1.gif")%>) no-repeat;"><div class="font2">活动期间达到有效投注额≥10万</div></td></tr>
                    <tr><td style="background: url(<%=GetImage("~/image/pro_8img2.gif")%>) no-repeat;"><div class="font2"><span>每周四下午16点</span>前由系统根据有效投注额区间自动发放红利</div></td></tr>
                    <tr><td style="background: url(<%=GetImage("~/image/pro_8img3.gif")%>) no-repeat;"><div class="font2"><span>一倍流水</span>即可提现</div></td></tr>
                </table>
            </td>
        </tr>
    </table>

    <%--<div class="main_games">
    <div class="main_index">
      <div class="main_lights">
        <div class="lights_left"></div>
        <div class="lights_right"></div>
      </div>
      <div class="pro1_content">
        <div class="pro1">
          <div class="pro1_banner"><img src="image/pro8.jpg" width="995" height="235" /></div>
          <div class="pro1_box nome_m">
           <div class="pro3_tab">
              <div class="p9_tab t_left">
              <p class="text1">什么是周周红利？</p>
              <p>【腾博会】会员只需要在一周时间内（每周一中午12：00-下周一中午12：00）有效投注额≥10万即可获得返奖红利，<span style=" color:#F00;">最高8888元。</span></p>
              </div>
              <div class="p9_tab t_right">
              <p class="text1">周周红利如何领取？</p>
              <p>投注额越高，红利越多。无需申请，系统自动发放。
</p>
              </div>
              <div class="B"></div>
              
            </div>
            
            <div class="pro8_text">
            
              <p  class="tit">周周红利邀您分享：</p>
              <div style="margin:20px 0 20px 0px;">
                <table width="90%" border="1" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height:40px; border-collapse:collapse;">
                  <tr>
                    <td class="protext1">一周有效投注额</td>
                    <td class="protext1">周周红利</td>
                  </tr>
                  <tr>
                    <td class="protext2">≥10万</td>
                    <td class="protext2">88</td>
                  </tr>
                  <tr>
                    <td class="protext2">≥50万</td>
                    <td class="protext2">388</td>
                  </tr>
                  <tr>
                    <td class="protext2">≥100万</td>
                    <td class="protext2">888</td>
                  </tr>
                  <tr>
                    <td class="protext2">≥1000万</td>
                    <td class="protext2">8888</td>
                  </tr>
                  
                  
                </table>
              </div>
              <p class="tit">活动说明</p>
             <ol class="promo_prompt">
              <li>【腾博会】所有会员均可享受“周周红利”奖金。</li>
              <li>会员只需在活动期间的一周时间内有效投注额≥10万即可获得红利（投注额越多，红利越高）。</li>
              <li>该红利礼金将在每周四下午16点前由系统自动发放到会员的游戏账户中。</li>
              <li>该红利礼金适用于所有游戏，只需一倍流水即可提现。</li>
              <li>“首存优惠”和“周周红利“优惠不能在同一周且不能同时享受。</li>
              <li>活动时间：截止至2014年2月28日。</li>
              <li>本优惠若发现恶意套利者【腾博会】有权扣除红利甚至没收本金</li>
              <li>该活动若存在文字理解的差异，则最终解释权归【腾博会】所有。</li>
                            </ol>        
            </div>
            <div class="pro8_right">
            <p  class="tit">领取流程：</p>
            <div>
              <table width="310" border="0" align="left" cellpadding="0" cellspacing="0">
              <tr>
                  <td>&nbsp;</td>
                </tr>
                <tr>
                  <td height="90" background="image/pro_8img1.gif"><table width="186" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                      <td class="pro8_text1">活动期间达到有效投注额≥10万</td>
                      </tr>
                  </table></td>
                </tr>
                <tr>
                  <td height="20">&nbsp;</td>
                </tr>
                <tr>
                  <td height="90" background="image/pro_8img2.gif"><table width="186" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                      <td class="pro8_text1"><span>每周四下午16点</span>前由系统根据有效投注额区间自动发放红利</td>
                      </tr>
                  </table></td>
                </tr>
                <tr>
                  <td height="20">&nbsp;</td>
                </tr>
                <tr>
                  <td height="90" background="image/pro_8img3.gif"><table width="186" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                      <td class="pro8_text1"><span>一倍流水</span>即可提现</td>
                      </tr>
                  </table></td>
                </tr>
                
              </table>
            </div>
<ul>
  <li></li>
</ul>
            </div>
            <div class="B"></div>
          </div>
          
          
        </div>
      </div>
    </div>
  </div>--%>
</asp:Content>
