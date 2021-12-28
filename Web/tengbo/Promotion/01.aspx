<%@ Page Title="" Language="C#" MasterPageFile="Promotion.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .table1 {
            width: 100%;
        }

        .reg {
            width: 120px;
            text-align: center;
        }

            .reg img {
                border: 0;
            }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="banner" runat="server">
    <img src="<%=GetImage("~/image/pro1.jpg")%>" width="995" height="235" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="a">
        <h1>首存优惠</h1>
        <table border="0" style="width: 100%">
            <tr>
                <td colspan="2">
                    <table class="table1">
                        <tr>
                            <th>例  子</th>
                            <th>首次存款金额（RMB)</th>
                            <th>赠送红利</th>
                            <th>最高红利 </th>
                            <th>提款要求</th>
                        </tr>
                        <tr>
                            <td>套餐 A</td>
                            <td>50-500</td>
                            <td>58%</td>
                            <td>288</td>
                            <td>有效投注额=（存款+红利）*18</td>
                        </tr>
                        <tr>
                            <td>套餐 B</td>
                            <td>601-2000</td>
                            <td>48%</td>
                            <td>888</td>
                            <td>有效投注额=（存款+红利）*18</td>
                        </tr>
                        <tr>
                            <td>套餐 C</td>
                            <td>2000以上</td>
                            <td>38%</td>
                            <td>58888</td>
                            <td>有效投注额=（存款+红利）*18</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="table1">
                        <tr>
                            <th>例  子</th>
                            <th>首次存款(RMB)</th>
                            <th>获赠红利</th>
                            <th>提款要求 </th>
                        </tr>
                        <tr>
                            <td>套餐 A</td>
                            <td>300</td>
                            <td>300 X 58% = 174</td>
                            <td>(300+174) X 18 = 8,532</td>
                        </tr>
                        <tr>
                            <td>套餐 B</td>
                            <td>1,000</td>
                            <td>1000 X 48% = 480</td>
                            <td>(1000+480) X 18 = 26,640</td>
                        </tr>
                        <tr>
                            <td>套餐 C</td>
                            <td>10,000</td>
                            <td>10000 X 38% = 3800</td>
                            <td>(10000+3800) X 18 = 248,400</td>
                        </tr>
                    </table>
                </td>
                <td class="reg"><a href="<%=ResolveClientUrl("~/Register.aspx")%>">
                    <img src="<%=GetImage("~/image/pro1_bt.png")%>" width="90" height="34" /></a></td>
            </tr>
        </table>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>如何申请</h1>
        <center><img src="<%=GetImage("~/image/pro1_img.jpg")%>" width="960" height="255" /></center>
    </div>
    <div class="b"></div>
    <div class="a">
        <h1>活动细则</h1>
        红利申请要求：手机号码 + IP地址 + 银行卡归属地。三个必须为统一归属地，方可申请红利。新会员在申请开户红利时，必须填写真实的资料才具备申请条件，感谢您的支持！<br /><br />
        &nbsp;&nbsp;&nbsp;有效投注额：有效投注额是指所有产生输赢的投注。无效投注是指在游戏中下注庄闲、游戏结果为和局；同一局游戏中下注正、反两种结果，如百家乐游戏同一局下注庄闲；<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;或因特殊原因退还本金的投注。
        <ol class="rules">
            <li>首存金额指第一笔投注之前的合计存款金额,玩家只有在本娱乐城的首次存款才有资格获得此优惠。</li>
            <li>申请优惠的有效时间：在首存后7天内且第一次取款前。首存红利在会员申请并审核通过后24小时内由系统自动发放到会员账号中。</li>
            <li>提款时您的有效投注额需达到（首存+红利）X 倍数，即可随时提款。<br />例如：您首次存款1000元，则红利为480元。有效投注额需达到（1000+480）X 18=26640，才可以申请提款。</li>
            <li>所有客户只能拥有一个账号：同一个IP、同一个存/提款卡、同一个手机号码都视为同一客户，如果发现同一个人拥有两个或以上的账户，重复的账户将会被冻结，【腾博会】保留索回重复账户的红利 及盈利的权利。</li>
            <li>凡是参与本次活动的客户，在没有完成活动要求的有效投注额的情况下不能以任何理由申请退出本活动。</li>
            <li>享受首存优惠后，达到首存优惠提款要求前产生的有效投注额将无法再享受洗码优惠。若会员不申请首存优惠，游戏有效投注额达到存款金额一倍流水即可提现。</li>
            <li>领取首存优惠的流水計算只計算HG平台、AG平台、太阳城平台、沙龙国际以及波音真人视讯、波音体育、彩票，不包括波音电子游戏。</li>
            <li>该活动若存在文字理解的差异，则最终解释权归【腾博会】所有。</li>
        </ol>
    </div>
</asp:Content>
<%--<div class="main_games">
        <div class="main_index">
            <div class="main_lights">
                <div class="lights_left"></div>
                <div class="lights_right"></div>
            </div>
            <div class="pro1_content">
                <div class="pro1">
                    <div class="pro1_banner">
                        <img src="image/pro1.jpg" width="995" height="235" /></div>
                    <div class="pro1_box">
                        <div class="pro1_tit">首存优惠</div>
                        <table width="900" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td height="60">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height: 40px; border-collapse: collapse;">
                                        <tr>
                                            <td class="protext1">例  子</td>
                                            <td class="protext1">首次存款金额（RMB)</td>
                                            <td class="protext1">赠送红利</td>
                                            <td class="protext1">最高红利 </td>
                                            <td class="protext1">提款要求</td>
                                        </tr>
                                        <tr>
                                            <td class="protext2">套餐 A</td>
                                            <td class="protext2">50-500</td>
                                            <td class="protext2">68%</td>
                                            <td class="protext2">288</td>
                                            <td rowspan="3" class="protext2">有效投注额=（存款+红利）*20</td>
                                        </tr>
                                        <tr>
                                            <td class="protext2">套餐 B</td>
                                            <td class="protext2">601-2000</td>
                                            <td class="protext2">48%</td>
                                            <td class="protext2">888</td>
                                        </tr>
                                        <tr>
                                            <td class="protext2">套餐 C</td>
                                            <td class="protext2">2000以上</td>
                                            <td class="protext2">38%</td>
                                            <td class="protext2">58888</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="20">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="780">
                                                <table width="100%" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height: 40px; border-collapse: collapse;">
                                                    <tr>
                                                        <td class="protext1">例  子</td>
                                                        <td class="protext1">首次存款(RMB)</td>
                                                        <td class="protext1">获赠红利</td>
                                                        <td class="protext1">提款要求 </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="protext2">套餐 A</td>
                                                        <td class="protext2">500</td>
                                                        <td class="protext2">500X68%=340</td>
                                                        <td class="protext2">(500+340)X20=16,800</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="protext2">套餐 B</td>
                                                        <td class="protext2">1,000</td>
                                                        <td class="protext2">1000X48%=480</td>
                                                        <td class="protext2">(1000+480)X20=29,600</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="protext2">套餐 C</td>
                                                        <td class="protext2">10,000</td>
                                                        <td class="protext2">10000X38%=3800</td>
                                                        <td class="protext2">(10000+3800)X20=276,000</td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="center"><a>
                                                <img src="image/pro1_bt.png" width="90" height="34" /></a></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="20">&nbsp;</td>
                            </tr>
                        </table>
                    </div>

                    <div class="pro1_box">
                        <div class="pro1_tit">如何申请</div>
                        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td height="80">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <img src="image/pro1_img.jpg" width="960" height="255" /></td>
                            </tr>
                            <tr>
                                <td height="20">&nbsp;</td>
                            </tr>
                        </table>
                    </div>


                    <div class="pro1_box nome_m">
                        <div class="pro1_tit">活动细则</div>
                        <div class="pro1_text">
                            <p>1、【腾博会】所有新注册的会员均可参加首存优惠活动且只能申请一次。</p>
                            <p>2、提款时您的有效投注额需达到（首存+红利）X20倍即可随时提款。</p>
                            <p>3、首存红利必须在首次存款后7天内进行申请。</p>
                            <p>4、首存红利在会员申请并审核通过后24小时内由系统自动发放到会员账号中。 </p>
                            <p>5、同一家庭、同一会员、同一姓名、同一收款账号、同一局域网只能享受一次首存礼金。</p>
                            <p>6、享受首存优惠后，达到优惠提款要求前产生的有效投注额将无法再享受洗码优惠。若会员不申请首存优惠，游戏有效投注额达到存款金额一倍流水即可提现。</p>
                            <p>7、该活动若存在文字理解的差异，则最终解释权归【腾博会】所有。            </p>
                        </div>
                    </div>


                </div>
            </div>
        </div>
    </div>--%>
