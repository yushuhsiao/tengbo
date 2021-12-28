<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro4.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="a">
        <h2>感谢新老会员长久以来对【腾博会】的支持和厚爱，现推出“推荐新会员，礼金大派送”活动，期待您的加入。</h2>
        <h1>活动细则：</h1>
        <ol class="rules">
            <li>推荐人必须是VIP或以上的会员，符合条件的会员可推荐好朋友来【腾博会】。只要被推荐的朋友在【腾博会】开户成功并通过客服的审核后，推荐人即可获得被推荐人首存金额10%的推荐礼金( 最高可达1800元 )。</li>
            <li>当被推荐人等级晋级时，推荐人将可获得相应的推荐晋级礼金（详情如下表所示），推荐礼金5倍流水后即可提现。
                <table class="table1" style="width:90%;">
                    <tr>
                        <th>被推荐人</th>
                        <th>推荐人福利</th>
                        <th>获赠红利</th>
                        <th>提款要求 </th>
                    </tr>
                    <tr>
                        <td>注册成功并存款</td>
                        <td>推荐礼金</td>
                        <td>被推荐人首存10%（最高1800）</td>
                        <td>10%好友推荐金 5倍流水</td>
                    </tr>
                    <tr>
                        <td>晋升为白银VIP</td>
                        <td rowspan="4">推荐晋级礼金</td>
                        <td>288</td>
                        <td rowspan="4">直接提款</td>
                    </tr>
                    <tr>
                        <td>晋升为黄金VIP</td>
                        <td>588</td>
                    </tr>
                    <tr>
                        <td>晋升为钻石VIP</td>
                        <td>888</td>
                    </tr>
                    <tr>
                        <td>晋升为至尊VIP</td>
                        <td>1288</td>
                    </tr>
                </table>
            </li>
            <li>被推荐人的注册时间必须迟于推荐人，审核时间最长为3天，被推荐人只允许有一个推荐人（推荐人是VIP或以上的会员）。被推荐人必须使用独立的账户、银行账户和IP，即可通过审核。</li>
            <li>被推荐人同样可以享受网站本身的优惠。</li>
            <li>被推荐用户在注册的时候可以在注册表中填入推荐人账号或联系客服填入推荐人账号。【腾博会】的风控部门人员在收到推荐请求后，将确认推荐关系，确认无误后即可通过审核。</li>
            <li>10%好友推荐金在被推荐人首次存款和通过审核后由系统自动发放到推荐人的游戏账户中。</li>
        </ol>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>注意事项：</h1>
        <ol class="rules" style="list-style-type:square;">
            <li>此优惠活动的推荐人和被推荐的朋友，不能来自于同一位客户、同一台电脑、同一个住址、同一个电子邮箱、同一个电话号码和同一个IP位址。</li>
            <li>任何人若以不诚实的手法获取奖金，【腾博会】将会取消其好友计划的资格，严重则冻结其账户及没收所有结余。</li>
            <li>【腾博会】保留对活动的最终解释权，有权修改活动细节及规则。</li>
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
          <div class="pro1_banner"><img src="image/pro4.jpg" width="995" height="235" /></div>
          <div class="pro1_box nome_m">
            <div class="pro4_text">
              <p class="tit1">感谢新老会员长久以来对【腾博会】的支持和厚爱，现推出“推荐新会员，礼金大派送”活动，期待您的加入。
              </p>
              <p  class="tit">活动细则：</p>
              <p> 1、推荐人必须是VIP或以上的会员，符合条件的会员可推荐好朋友来【腾博会】。只要被推荐的朋友在【腾博会】开户成功并通过客服的审核后，推荐人即可获得</p>
                                        <p> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;被推荐人首存金额10%的推荐礼金( 最高可达1800元 )。&nbsp; </p>
              <p> 2、当被推荐人等级晋级时，推荐人将可获得相应的推荐晋级礼金（详情如下表所示），推荐晋级礼金5倍流水后即可提现。</p>
              <div style="margin:20px 0;">
                <table width="90%" border="1" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height:40px; border-collapse:collapse;">
                  <tr>
                    <td class="protext1">被推荐人</td>
                    <td class="protext1">推荐人福利</td>
                    <td class="protext1">获赠红利</td>
                    <td class="protext1">提款要求 </td>
                  </tr>
                  <tr>
                    <td class="protext2">注册成功并存款</td>
                    <td class="protext2">推荐礼金</td>
                    <td class="protext2">被推荐人首存10%（最高1800）</td>
                    <td class="protext2">10%好友推荐金 5倍流水</td>
                  </tr>
                  <tr>
                    <td class="protext2">晋升为白银VIP</td>
                    <td rowspan="4" class="protext2">推荐晋级礼金</td>
                    <td class="protext2">288</td>
                    <td rowspan="4" class="protext2">直接提款</td>
                  </tr>
                  <tr>
                    <td class="protext2">晋升为黄金VIP</td>
                    <td class="protext2">588</td>
                  </tr>
                   <tr>
                    <td class="protext2">晋升为钻石VIP</td>
                    <td class="protext2">888</td>
                  </tr>
                   <tr>
                    <td class="protext2">晋升为至尊VIP</td>
                    <td class="protext2">1288</td>
                  </tr>
                </table>
              <p>3、被推荐人的注册时间必须迟于推荐人，审核时间最长为3天，被推荐人只允许有一个推荐人（推荐人是VIP或以上的会员）。被推荐人必须使用独立的账户、银行</p>
                                        <p> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 账户和IP，即可通过审核。</p>
              <p>4、被推荐人同样可以享受网站本身的优惠。</p>
              <p>5、被推荐用户在注册的时候可以在注册表中填入推荐人账号或在用户中心后台输入推荐人账号。【腾博会】的风控部门人员在收到推荐请求后，将确认推荐关系，</p>
                                            <p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;确认无误后即可通过审核。</p>
              <p> 6、10%好友推荐金在被推荐人首次存款和通过审核后由系统自动发放到推荐人的游戏账户中。</p>
              <p class="tit">注意事项：</p>
              <p>■&nbsp; 此优惠活动的推荐人和被推荐的朋友，不能来自于同一位客户、同一台电脑、同一个住址、同一个电子邮箱、同一个电话号码和同一个IP位址。              </p>
              <p> ■&nbsp; 任何人若以不诚实的手法获取奖金，【腾博会】将会取消其好友计划的资格，严重则冻结其账户及没收所有结余。              </p>
              <p>■ 【腾博会】保留对活动的最终解释权，有权修改活动细节及规则。
                </p>
                </p>
            </div>
          </div>
          
          
        </div>
      </div>
    </div>
  </div>
    --%>
</asp:Content>
