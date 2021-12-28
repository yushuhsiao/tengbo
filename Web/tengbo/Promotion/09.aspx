<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro9.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <table class="table2 font1" cellspacing="5">
        <tr>
            <td>
                <h3 class="font2">什么是公司绿色通道入款方式？</h3>
                会员在登陆后，通过后台的【公司绿色通道】即可获赠1%优惠红利，红利最高可达10000元。
            </td>
            <td>
                <h3 class="font2">存款红利计算方式是怎样的？</h3>
                存款红利 = 存款总额 X 1%，單筆上限50元。 例如：通过后台的【公司绿色通道】单次存款5,000元即可获赠50元的存款利息。
            </td>
        </tr>
    </table>
    <div class="b"></div>
    <div class="a">
        <h1>领取流程：</h1>
        <ol class="rules">
            <li>使用【公司绿色通道】金额不限 。</li>
            <li>由系统自动发放，无需申请。</li>
            <li>存款红利免费赠送，和存款一样达到一倍流水即可提款 。</li>
        </ol>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>利息领取：</h1>
        <table class="table1" style="width: 90%; margin: 0 auto;">
            <tr>
                <th>单次存款金额</th>
                <th>存款红利利率</th>
                <th>红利领取</th>
            </tr>
            <tr>
                <td>5,000元</td>
                <td>1%</td>
                <td>50元</td>
            </tr>
        </table>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>活动说明</h1>
        <ol class="rules">
            <li>此优惠针对所有新老会员。</li>
            <li>此优惠无需申请，只要通过【公司绿色通道】红利就会随存款金额一并自动添加到帐。</li>
            <li>此次优惠推广可以与本娱乐城的其他的优惠、奖励或推广活动同时享用。</li>
            <li>每人每日可以获得存款红利的上限为10000元。</li>
            <li>提款所需投注额是指有效投注量，所有无效、注单取消、平局投注以及无风险投注，均不会计算在有效的投注额内（所谓无风险投注的范例为：在一手牌中平均投注不同结果，使得投注没有任何输的风险或在轮盘中同时投注红色和黑色，在百家乐中同时投注庄家和闲家，以及在体育游戏中下注亚洲盘赔率低于0.6的赌盘等等）。</li>
            <li>该活动若存在文字理解的差异，则最终解释权归【腾博会】所有。</li>
        </ol>
    </div>
    <div class="b"></div>
    <%--<div class="main_games">
        <div class="main_index">
            <div class="main_lights">
                <div class="lights_left"></div>
                <div class="lights_right"></div>
            </div>
            <div class="pro1_content">
                <div class="pro1">
                    <div class="pro1_banner">
                        <img src="image/pro9.jpg" width="995" height="235" />
                    </div>
                    <div class="pro1_box nome_m">
                        <div class="pro3_tab">
                            <div class="p9_tab t_left">
                                <p class="text1">什么是公司绿色通道入款方式？</p>
                                <p>会员在登陆后，通过后台的【公司绿色通道】即可获赠1%优惠红利，红利最高可达10000元。</p>
                            </div>
                            <div class="p9_tab t_right">
                                <p class="text1">存款红利计算方式是怎样的？</p>
                                <p>
                                    存款红利 = 存款总额X 1% 例如：通过后台的【公司绿色通道】单次存款10,000元即可获赠100元的存款利息。
                                </p>
                            </div>
                            <div class="B"></div>

                        </div>

                        <div class="pro4_text">
                            <p class="tit">领取流程：</p>
                            <ol class="promo_prompt">
                                <li>使用【公司绿色通道】金额不限 。</li>
                                <li>由系统自动发放，无需申请。</li>
                                <li>存款红利免费赠送，和存款一样达到一倍流水即可提款 。</li>
                            </ol>
                            <p class="tit">利息领取：</p>
                            <div style="margin: 20px 0 20px 70px;">
                                <table width="90%" border="1" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height: 40px; border-collapse: collapse;">
                                    <tr>
                                        <td class="protext1">单次存款金额</td>
                                        <td class="protext1">存款红利利率</td>
                                        <td class="protext1">红利领取</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">10,000元</td>
                                        <td class="protext2">1%</td>
                                        <td class="protext2">100元</td>
                                    </tr>


                                </table>
                            </div>
                            <p class="tit">活动说明</p>
                            <ol class="promo_prompt">
                                <li>此优惠针对所有新老会员。</li>
                                <li>此优惠无需申请，只要通过【公司绿色通道】红利就会随存款金额一并自动添加到帐。</li>
                                <li>此次优惠推广可以与本娱乐城的其他的优惠、奖励或推广活动同时享用。</li>
                                <li>每人每日可以获得存款红利的上限为10000元。</li>
                                <li>提款所需投注额是指有效投注量，所有无效、注单取消、平局投注以及无风险投注，均不会计算在有效的投注额内。﹝所谓无风险投注的范例为：在一手牌中平均投注不同结果，使得投注没有任何输的风险或在轮盘中同时投注红色和黑色，在百家乐中同时投注庄家和闲家，以及在体育游戏中下注亚洲盘赔率低于0.6的赌盘等等。</li>
                                <li>该活动若存在文字理解的差异，则最终解释权归【腾博会】所有。</li>
                            </ol>
                        </div>
                    </div>


                </div>
            </div>
        </div>
    </div>--%>
</asp:Content>
