<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table1 { width: 90%; margin: 0 auto; }
        .txt1 { line-height: 25px; font-size: 14px; letter-spacing: 1px; padding-top: 10px; padding-bottom: 10px; }
        .cont2{ width:938px; margin:0 auto; overflow: hidden; padding-bottom:5px;}
        .cont2 li{ width:421px; height:auto; background:#5a0101; float:left; display: block; margin:0px 14px 14px 14px; padding:5px 10px; color:#fefafa; font-size:14px; font-family:"微软雅黑"; line-height:25px; letter-spacing:1px;}
        .cont2 li span{ color:#eeb826;}
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro2.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="a">
        <h1>会员洗码返点比例</h1>
        <table class="table1">
            <tr>
                <th>会员级别</th>
                <th>周有效投注额</th>
                <th>洗码比例</th>
                <th>洗码结算时间 </th>
            </tr>
            <tr>
                <td>普通会员</td>
                <td>≥100</td>
                <td>0.6%</td>
                <td>每周一下午18点前</td>
            </tr>
            <tr>
                <td>VIP会员</td>
                <td>≥50万</td>
                <td>0.8%</td>
                <td>每天下午18点前</td>
            </tr>
            <tr>
                <td>白银VIP</td>
                <td>≥300万</td>
                <td>0.9%</td>
                <td>每天下午18点前</td>
            </tr>
            <tr>
                <td>黄金VIP</td>
                <td>≥500万</td>
                <td>1.0%</td>
                <td>每天下午18点前</td>
            </tr>
            <tr>
                <td>钻石VIP</td>
                <td>≥800万</td>
                <td>1.1%</td>
                <td>每天下午18点前</td>
            </tr>
            <tr>
                <td>至尊VIP</td>
                <td>≥1200万</td>
                <td>1.2%</td>
                <td>每天下午18点前</td>
            </tr>
        </table>
        <div class="font1 txt1">例子：会员A为VIP会员，平时享受0.8% 洗码退水。在某一周的有效投注额达到了800万，成为了钻石VIP会员，根据钻石VIP的洗码比例，他将永久享受1.1%的洗码退水比例。</div>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>[腾博会] 洗码特色</h1>
        <ul class="cont2">
            <li><span>适用游戏种类：</span>适用于AG平台、HG平台、太阳城、沙龙国际和波音平台下电子游艺、体育以及彩票类等所有游戏。</li>
            <li><span>亮点特色：</span>对首次存款的用户超高奖励（首存优惠58%）后自动升级为VIP会员并享有天天0.8%洗码退水</li>
            <li><span>天天结算洗码：</span>所有VIP会员可天天结算洗码</li>
            <li><span>返水高额：</span>高达1.2% 高额洗码</li>
            <li><span>洗码提现要求：</span>金额上限不封顶。洗码金额可直接提款。</li>
            <li><span>其他奖励：</span>海外游和VIP赌厅显身手</li>
        </ul>
        <div class="clear"></div>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>活动细则</h1>
        <ol class="rules">
            <li>洗码结算时间：周洗码时间为每周一中午12点到下周一中午11点59分59秒。日洗码时间为当日中午12点到次日中午11点59分59秒。</li>
            <li>VIP会员（钻石、至尊）可享受每天洗码结算。洗码无需申请，系统将于每天下午18点之前自动发放。</li>
            <li>洗码优惠金额无上限，多投多得。您可以选择继续投注游戏也可以申请提款。</li>
            <li>洗码优惠与首存优惠不能同时享受。</li>
            <li>所有无风险投注不计有效投注额；无风险投注包含以下状况：
                <ol style="list-style-type: disc">
                    <li>所有拒绝投注、无效投注、打平或是赔率低于0.5，将不包括及计算于投注金额内。</li>
                    <li>任何「取消注单」或「和局」的投注额将不予计算有效投注。 </li>
                </ol>
            </li>
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
          <div class="pro1_banner"><img src="image/pro2.jpg" width="995" height="235" /></div>
          <div class="pro1_box">
            <div class="pro2_tit">会员洗码返点比例</div>
            <table width="860" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td height="60">&nbsp;</td>
              </tr>
              <tr>
                <td><table width="100%" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height:40px; border-collapse:collapse;">
                  <tr>
                    <td class="protext1">会员级别</td>
                    <td class="protext1">周有效投注额</td>
                    <td class="protext1">洗码比例</td>
                    <td class="protext1">洗码结算时间 </td>
                    </tr>
                  <tr>
                    <td class="protext2">普通会员</td>
                    <td class="protext2">≥100</td>
                    <td class="protext2">0.6%</td>
                    <td class="protext2">每周一下午16点前</td>
                    </tr>
                  <tr>
                    <td class="protext2">VIP会员</td>
                    <td class="protext2">≥50万</td>
                    <td class="protext2">0.8%</td>
                    <td class="protext2">每天下午16点前</td>
                    </tr>
                  <tr>
                    <td class="protext2">白银VIP</td>
                    <td class="protext2">≥100万</td>
                    <td class="protext2">0.9%</td>
                    <td class="protext2">每天下午16点前</td>
                    </tr>
                  <tr>
                    <td class="protext2">黄金VIP</td>
                    <td class="protext2">≥300万</td>
                    <td class="protext2">1.0%</td>
                    <td class="protext2">每天下午16点前</td>
                  </tr>
                  <tr>
                    <td class="protext2">钻石VIP</td>
                    <td class="protext2">≥800万</td>
                    <td class="protext2">1.1%</td>
                    <td class="protext2">每天下午16点前</td>
                  </tr>
                  <tr>
                    <td class="protext2">至尊VIP</td>
                    <td class="protext2">≥1200万</td>
                    <td class="protext2">1.2%</td>
                    <td class="protext2">每天下午16点前</td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td height="10"></td>
              </tr>
              <tr>
                <td class="pro2_text">例子：会员A为VIP会员，平时享受0.8% 洗码退水。在某一周的有效投注额达到了800万，成为了钻石VIP会员，根据钻石VIP的洗码比例，他将永久享受1.1%的洗码退水比例。</td>
              </tr>
              <tr>
                <td height="20">&nbsp;</td>
              </tr>
            </table>
          </div>
          
          <div class="pro1_box">
            <div class="pro2_tit">[腾博会] 洗码特色</div>
            <div >
              <ul class="pro2_con">
                <li><span>适用游戏种类：</span>真人视讯、电子游戏、彩票、体育投注等所有游戏种类</li>
                <li><span>亮点特色：</span>对首次存款的用户超高奖励（首存优惠68%）后自动升级为VIP会员并享有天天0.8%洗码退水</li>
                <li><span>天天结算洗码：</span>所有VIP会员可天天结算洗码</li>
                <li><span>返水高额：</span>高达1.2% 高额洗码</li>
                <li><span>洗码提现要求：</span>一倍流水（有效投注额），金额上限不封顶。天洗码返点金额达到200元或以上可选择直接提款，无需一倍流水要求。</li>
                <li><span>其他奖励：海外游和VIP赌厅显身手</li>
              </ul>
            </div>
          </div>
          
          
          <div class="pro1_box nome_m">
            <div class="pro1_tit">活动细则</div>
            <div class="pro1_text">
              <p>1、洗码结算时间：周洗码时间为每周一中午12点到下周一中午11点59分59秒。日洗码时间为当日中午12点到次日中午11点59分59秒。</p>
              <p> 2、VIP会员（钻石、至尊）可享受每天洗码结算。洗码无需申请，系统将于每天下午16点之前自动发放。                </p>
              <p>3、洗码优惠金额无上限，多投多得。您可以选择继续投注游戏也可以在在达到一倍流水（有效投注额）后申请提款。                </p>
              <p>4、洗码优惠与首存优惠不能同时享受。</p>
              <p> 5、所有无风险投注不计有效投注额；无风险投注包含以下状况：</p>
              <p> &nbsp;&nbsp;&nbsp; * 所有拒绝投注、无效投注、打平或是赔率低于0.5，将不包括及计算于投注金额内。                </p>
              <p>&nbsp;&nbsp;&nbsp; * 任何「取消注单」或「和局」的投注额将不予计算有效投注。 </p>
            </div>
          </div>
          
          
        </div>
      </div>
    </div>
  </div>--%>
</asp:Content>
