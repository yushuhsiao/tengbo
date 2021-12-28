<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.master" AutoEventWireup="true" Inherits="root_aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="body1" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="vip_page">
        <div class="body">
            <center class="banner">
                <img src="<%=GetImage("~/image/vip_banner.jpg")%>" width="1031" height="271" /></center>
            <table class="tab1 font1" cellspacing="5">
                <tr>
                    <td>
                        <h3 class="font2">什么是【腾博会】VIP？</h3>
                        【腾博会】VIP级别共分五个等级，分别为普通VIP、白银VIP、黄金VIP、钻石VIP和至尊VIP，每位VIP都有自己专属的等级特权。
                    </td>
                    <td>
                        <h3 class="font2">如何成为【腾博会】VIP？</h3>
                        回馈老客户活动期间自动升级为普通VIP会员，享有0.8%洗码优惠并每日下午18点以前结算。
                    </td>
                </tr>
            </table>
            <div class="tab2 font1">
                <div class="title1">VIP晋升计划</div>
                <table class="table1">
                    <tr>
                        <th>VIP晋升计划</th>
                        <th>周有效投注额</th>
                        <th>洗码比例</th>
                        <th>晋级礼金</th>
                        <th>周年礼金</th>
                        <th>VIP海外旅游</th>
                    </tr>
                    <tr>
                        <td>普通会员</td>
                        <td>≥100</td>
                        <td>0.6%</td>
                        <td>无</td>
                        <td>无</td>
                        <td>不参加</td>
                    </tr>
                    <tr>
                        <td>普通VIP</td>
                        <td>≥50万</td>
                        <td>0.8%</td>
                        <td>188</td>
                        <td>88</td>
                        <td>不参加</td>
                    </tr>
                    <tr>
                        <td>白银VIP</td>
                        <td>≥300万</td>
                        <td>0.9%</td>
                        <td>388</td>
                        <td>188</td>
                        <td>不参加</td>
                    </tr>
                    <tr>
                        <td>黄金VIP</td>
                        <td>≥500万</td>
                        <td>1.0%</td>
                        <td>888</td>
                        <td>288</td>
                        <td>不参加</td>
                    </tr>
                    <tr>
                        <td>钻石VIP</td>
                        <td>≥800万</td>
                        <td>1.1%</td>
                        <td>1288</td>
                        <td>588</td>
                        <td>参加</td>
                    </tr>
                    <tr>
                        <td>至尊VIP</td>
                        <td>≥1200万</td>
                        <td>1.2%</td>
                        <td>1888</td>
                        <td>1888</td>
                        <td>参加</td>
                    </tr>
                </table>
            </div>
            <div class="tab2 font1">
                <div class="title1">服务特权</div>
                <ul>
                    <li class="text3">一对一的VIP客服</li>
                    <li>1.  VIP热线服务，随时为您遇到的问题排忧解难。</li>
                    <li>2.  可随时联系客服经理专人服务。</li>
                    <li>3.  开辟VIP专属的存取款通道，保证VIP用户能畅通无阻地享受游戏。</li>
                </ul>
            </div>
            <div class="tab2 font1">
                <div class="title1">VIP专属</div>
                <ul class="title2">
                    <li>【腾博会】自2009年5月成立以来，坚持“以诚为本，服务至上”的信念为广大会员服务。为回馈广大VIP会员对【腾博会】的支持，特推出"休闲之旅"活动。</li>
                    <li>申请步骤：成为至尊VIP会员 -- 提交报名申请 -- VIP客服协助，确认行程 -- 办理护照和签证 -- 抵达马尼拉 -- 机场绿色通道接待 -- 自选行程安排 -- 返程专车送到机场。</li>
                </ul>
                <ul>
                    <li class="text3">自选行程包括有：</li>
                    <li>A.  马尼拉当地一日游</li>
                    <li>B.  赌场VIP包厅小试身手（自选）</li>
                    <li>C.  长滩岛旅游观光3天2晚</li>
                    <li>D.  观看马尼拉本地演出</li>
                    <li class="note">备注：总共时间为5天4晚，期间除会员本身购买礼品外，其他所有费用由【腾博会】负责。</li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>

<%--<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
.VIP_content{ width:1069px; height: auto; margin:0 auto; margin-top:18px;overflow:hidden; background:#480102; padding:10px;}
    </style>
</asp:Content>--%>
<%--<asp:Content ContentPlaceHolderID="body2" runat="server">
  <div class="main_games">
    <div class="main_index">
      <div class="main_lights">
        <div class="lights_left"></div>
        <div class="lights_right"></div>
      </div>
      <div class="VIP_content">
        <div class="vip">
          <div class="vip_banner"><img src="image/vip_banner.jpg" width="1031" height="271" /></div>
          <div class="vip_tab1">
            <div class="tab1 t_left">
            <p class="text1">1.什么是【腾博会】VIP？</p>
            <p>【腾博会】VIP级别共分五个等级，分别为普通VIP、白银VIP、黄金VIP、钻石VIP和至尊VIP，每位VIP都有自己专属的等级特权。</p>
            </div>
            <div class="tab1 t_right">
            <p class="text1">2.如何成为【腾博会】VIP？</p>
            <p>回馈老客户活动期间自动升级为普通VIP会员，享有0.8%洗码优惠并每日下午16点以前结算。</p>
            </div>
            <div class="B"></div>
          </div>
          <div class="vip_tab2">
            <table width="1030" border="0" align="center" cellpadding="0" cellspacing="0" >
              <tr>
                <td height="40"><table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="35"><img src="image/vip_tubiao.gif" width="22" height="20" /></td>
                    <td class="text2">VIP晋升计划</td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td height="316"><table width="95%" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height:40px; border-collapse:collapse;">
                  <tr>
                    <td class="text2_1">VIP晋升计划</td>
                    <td class="text2_1">周有效投注额</td>
                    <td class="text2_1">洗码比例</td>
                    <td class="text2_1">晋级礼金</td>
                    <td class="text2_1">周年礼金</td>
                    <td class="text2_1">VIP海外旅游</td>
                  </tr>
                  <tr>
                    <td class="text2_2">普通会员</td>
                    <td class="text2_2">≥100</td>
                    <td class="text2_2">0.6%</td>
                    <td class="text2_2"> 无</td>
                    <td class="text2_2"> 无</td>
                    <td class="text2_2">不参加</td>
                  </tr>
                  <tr>
                    <td class="text2_2">普通VIP</td>
                    <td class="text2_2"> ≥50万</td>
                    <td class="text2_2">0.8%</td>
                    <td class="text2_2">188</td>
                    <td class="text2_2">88</td>
                    <td class="text2_2">不参加</td>
                  </tr>
                  <tr>
                    <td class="text2_2">白银VIP</td>
                    <td class="text2_2"> ≥100万</td>
                    <td class="text2_2">0.9%</td>
                    <td class="text2_2">388</td>
                    <td class="text2_2">188</td>
                    <td class="text2_2">不参加</td>
                  </tr>
                  <tr>
                    <td class="text2_2">黄金VIP</td>
                    <td class="text2_2"> ≥300万</td>
                    <td class="text2_2">1%</td>
                    <td class="text2_2">888</td>
                    <td class="text2_2">288</td>
                    <td class="text2_2">不参加</td>
                  </tr>
                  <tr>
                    <td class="text2_2">钻石VIP</td>
                    <td class="text2_2"> ≥800万</td>
                    <td class="text2_2">1.1%</td>
                    <td class="text2_2">1288</td>
                    <td class="text2_2">588</td>
                    <td class="text2_2">参加</td>
                  </tr>
                  <tr>
                    <td class="text2_2">至尊VIP</td>
                    <td class="text2_2"> ≥1200万</td>
                    <td class="text2_2">1.2%</td>
                    <td class="text2_2">1888</td>
                    <td class="text2_2">1888</td>
                    <td class="text2_2">参加</td>
                  </tr>
                </table></td>
              </tr>
            </table>
          </div>
          <div class="vip_tab">
            <div class="tab_tit">服务特权</div>
            <ul class="tab_con">
              <li class="text3">一对一的VIP客服</li>
              <li>1.  VIP热线服务，随时为您遇到的问题排忧解难。</li>
              <li>2.  可随时联系客服经理专人服务。</li>
              <li>3.  开辟VIP专属的存取款通道，保证VIP用户能畅通无阻地享受游戏。</li>
            </ul>
          </div>
          <div class="vip_tab none_m">
            <div class="tab_tit">VIP专属</div>
            <p>【腾博会】自2009年5月成立以来，坚持“以诚为本，服务至上”的信念为广大会员服务。为回馈广大VIP会员对【腾博会】的支持，特推出"休闲之旅"活动。</p>
            <p>申请步骤：成为至尊VIP会员--提交报名申请--VIP客服协助，确认行程--办理护照和签证--抵达马尼拉--机场绿色通道接待--自选行程安排--返程专车送到机场。</p>
            <div class="tab4">
              <div class="tab4_left">
              <ul class="tab_con">
              <li class="text3">自选行程包括有：</li>
              <li>A.  马尼拉当地一日游</li>
              <li>B.  赌场VIP包厅小试身手（自选）</li>
              <li>C.  长滩岛旅游观光3天2晚</li>
              <li>D.  观看马尼拉本地演出</li>
              <li class=" note">备注：总共时间为5天4晚，期间除会员本身购买礼品外，其他所有费用由【腾博会】负责。</li>
              </ul>
              </div>
              <div class="tab4_right">
                <div class="tab4_video"><img src="image/vip_video.gif" width="267" height="152" /></div>
              </div>
              <div class="B"></div>
            </div>
          </div>
<div class="B"></div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>--%>
