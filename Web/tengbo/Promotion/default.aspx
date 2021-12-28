<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.master" AutoEventWireup="true" Inherits="SitePage" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.RootMasterPage.NavIndex = 5;
    }
</script>

<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="promotion">
        <div class="body">
            <div class="title">优惠活动</div>
            <ul>
                <li class="item ui-corner-all"><div class="item-title">首存优惠    </div><div class="item-cont"><a href="01.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_1.jpg")%>" width="305" height="146" /><p>新会员开户成功后首次存款即可申请首存红利，最高可获58%。                    </p><div class="button2">点击查看详情</div></a></div></li>
                <li class="item ui-corner-all"><div class="item-title">洗码优惠    </div><div class="item-cont"><a href="02.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_2.jpg")%>" width="305" height="146" /><p>洗码退水按会员级别分为不同等级，最高洗码退水1.2%。                         </p><div class="button2">点击查看详情</div></a></div></li>
                <li class="item ui-corner-all"><div class="item-title">VIP直通车   </div><div class="item-cont"><a href="03.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_3.jpg")%>" width="305" height="146" /><p>专为新会员准备的贺礼，立即晋升为VIP会员。                                  </p><div class="button2">点击查看详情</div></a></div></li>
                <li class="item ui-corner-all"><div class="item-title">好友推荐    </div><div class="item-cont"><a href="04.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_4.jpg")%>" width="305" height="146" /><p>推荐新朋友，即可获得推荐礼金和推荐晋级礼金。                               </p><div class="button2">点击查看详情</div></a></div></li>
                <li class="item ui-corner-all"><div class="item-title">复活礼金    </div><div class="item-cont"><a href="05.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_5.jpg")%>" width="305" height="146" /><p>在负盈利情况下复活的返利方式，免费赠送给所有会员，8倍流水即可提款。        </p><div class="button2">点击查看详情</div></a></div></li>
                <li class="item ui-corner-all"><div class="item-title">生日礼金    </div><div class="item-cont"><a href="06.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_6.jpg")%>" width="305" height="146" /><p>生日礼金是【腾博会】为所有会员准备的生日大礼。                             </p><div class="button2">点击查看详情</div></a></div></li>
                <li class="item ui-corner-all"><div class="item-title">晋级奖金    </div><div class="item-cont"><a href="07.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_7.jpg")%>" width="305" height="146" /><p>为【腾博会】所有级别会员在晋级中准备的鼓励奖金。                           </p><div class="button2">点击查看详情</div></a></div></li>
                <li class="item ui-corner-all"><div class="item-title">周周红利    </div><div class="item-cont"><a href="08.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_8.jpg")%>" width="305" height="146" /><p>根据会员投注额产生的优惠红利，投注额越高，红利越多                         </p><div class="button2">点击查看详情</div></a></div></li>
                <li class="item ui-corner-all"><div class="item-title">绿色通道入款</div><div class="item-cont"><a href="09.aspx" target="_promotion_frame"><img src="<%=GetImage("~/image/pro_9.jpg")%>" width="305" height="146" /><p>针对绿色通道入款方式的返利优惠。</p><div class="button2">点击查看详情</div></a></div></li>
            </ul>
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
