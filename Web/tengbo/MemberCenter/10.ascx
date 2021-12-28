<%@ Control Language="C#" AutoEventWireup="true" Inherits="SiteLogControl" %>
<%@Import Namespace="BU" %>
<%@Import Namespace="System.Data" %>
<%@Import Namespace="System.Data.SqlClient" %>
<script runat="server">
    protected override string nav_index { get { return "10"; } }
</script>

<div class="user_center_content" index="<%=this.content_index_s%>" reload="false" style="display: <%=this.css_display%>;">
<div class="item_title">优惠记录</div>
<div class="log_title">请点击选择您需要的账单月份：</div>
<ul class="log_nav"><% foreach (int i in this.log_nav) { %>
    <li onclick="$.nav('<%=this.nav_index%>').navshow('<%=this.items[i]%>')"><div <% if (i == this.content_index) { %> class="cur"<% } %>><%=this.titles[i]%></div></li> <% } %>
</ul>
<div class="log_title"><%=string.Format("{0:yyyy}年{0:MM}", this.times[this.content_index])%>月份优惠记录明细如下：</div>
    <div style="overflow-x:auto;height:345px;">
<table class="log_detail" style="width:90%; margin-left: 15px" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <th>日期</th>
        <th>类型</th>
        <th>金额</th>
        <th>备注</th>
    </tr> <% 
        foreach (SqlDataReader r in this.GetPromotion())
        {
            if (r == null) { %><tr><td colspan="4">无记录</td></tr><% } else { %>
    <tr>
        <td><%=string.Format("{0:yyyy/MM/dd HH:mm}", r.GetValue("CreateTime"))%></td>
        <td><%=(BU.LogType)r.GetInt32("LogType") %></td>
        <td><%=string.Format("{0:0.00}", Math.Abs(r.GetDecimal("Amount")))%></td>
        <td></td>
    </tr><% }
        } %>
</table>
        </div>
<div class="prompt" style="margin-top: 5px;">
    使用说明：1、请点击账单月份查询账单明细&nbsp; 2、账单记录结算时间为北京时间中午12点
</div>
<%--<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="line-height: 35px;">
    <tr>
        <td align="center">
            <table width="100%" border="0" align="left" cellpadding="2" cellspacing="0">
                <tr>
                    <td align="left">
                        <p>请点击选择您需要的账单月份：</p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="month">
                            <ul>
                                <li class="cur">2013/10</li>
                                <li>2013/09</li>
                                <li>2013/08</li>
                                <li>2013/07</li>
                                <li>2013/06</li>
                                <li>2013/05</li>
                            </ul>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left">

                        <div class="month_tab">
                            <p>2013年10月份优惠记录明细如下：</p>
                            <div style="margin: 10px 0 10px 10px; overflow: hidden;">
                                <table width="100%" border="1" align="left" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="tab_2" style="line-height: 40px; border-collapse: collapse;">
                                    <tr>
                                        <td width="25%" class="protext1">日期</td>
                                        <td width="25%" class="protext1">类型</td>
                                        <td width="25%" class="protext1">金额</td>
                                        <td width="25%" class="protext1">备注</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年10月</td>
                                        <td class="protext2">xxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年10月</td>
                                        <td class="protext2">xxxxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">xxxx</td>
                                    </tr>

                                </table>
                            </div>
                        </div>

                        <div class="month_tab" style="display: none;">
                            <p>2013年09月份优惠记录明细如下：</p>
                            <div style="margin: 10px 0 10px 10px; overflow: hidden;">
                                <table width="100%" border="1" align="left" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="Table1" style="line-height: 40px; border-collapse: collapse;">
                                    <tr>
                                        <td width="25%" class="protext1">日期</td>
                                        <td width="25%" class="protext1">类型</td>
                                        <td width="25%" class="protext1">金额</td>
                                        <td width="25%" class="protext1">备注</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年09月</td>
                                        <td class="protext2">xxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年09月</td>
                                        <td class="protext2">xxxxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">xxxx</td>
                                    </tr>

                                </table>
                            </div>
                        </div>

                        <div class="month_tab" style="display: none;">
                            <p>2013年08月份优惠记录明细如下：</p>
                            <div style="margin: 10px 0 10px 10px; overflow: hidden;">
                                <table width="100%" border="1" align="left" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="Table2" style="line-height: 40px; border-collapse: collapse;">
                                    <tr>
                                        <td width="25%" class="protext1">日期</td>
                                        <td width="25%" class="protext1">类型</td>
                                        <td width="25%" class="protext1">金额</td>
                                        <td width="25%" class="protext1">备注</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年08月</td>
                                        <td class="protext2">xxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年08月</td>
                                        <td class="protext2">xxxxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">xxxx</td>
                                    </tr>

                                </table>
                            </div>
                        </div>

                        <div class="month_tab" style="display: none;">
                            <p>2013年07月份优惠记录明细如下：</p>
                            <div style="margin: 10px 0 10px 10px; overflow: hidden;">
                                <table width="100%" border="1" align="left" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="Table3" style="line-height: 40px; border-collapse: collapse;">
                                    <tr>
                                        <td width="25%" class="protext1">日期</td>
                                        <td width="25%" class="protext1">类型</td>
                                        <td width="25%" class="protext1">金额</td>
                                        <td width="25%" class="protext1">备注</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年07月</td>
                                        <td class="protext2">xxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年07月</td>
                                        <td class="protext2">xxxxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">xxxx</td>
                                    </tr>

                                </table>
                            </div>
                        </div>

                        <div class="month_tab" style="display: none;">
                            <p>2013年06月份优惠记录明细如下：</p>
                            <div style="margin: 10px 0 10px 10px; overflow: hidden;">
                                <table width="100%" border="1" align="left" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="Table4" style="line-height: 40px; border-collapse: collapse;">
                                    <tr>
                                        <td width="25%" class="protext1">日期</td>
                                        <td width="25%" class="protext1">类型</td>
                                        <td width="25%" class="protext1">金额</td>
                                        <td width="25%" class="protext1">备注</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年06月</td>
                                        <td class="protext2">xxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年06月</td>
                                        <td class="protext2">xxxxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">xxxx</td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                        <div class="month_tab" style="display: none;">
                            <p>2013年05月份优惠记录明细如下：</p>
                            <div style="margin: 10px 0 10px 10px; overflow: hidden;">
                                <table width="100%" border="1" align="left" cellpadding="0" cellspacing="0" bordercolor="#bca8aa " id="Table5" style="line-height: 40px; border-collapse: collapse;">
                                    <tr>
                                        <td width="25%" class="protext1">日期</td>
                                        <td width="25%" class="protext1">类型</td>
                                        <td width="25%" class="protext1">金额</td>
                                        <td width="25%" class="protext1">备注</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年05月</td>
                                        <td class="protext2">xxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="protext2">2013年05月</td>
                                        <td class="protext2">xxxxxxxxx</td>
                                        <td class="protext2">xxxxxx</td>
                                        <td class="protext2">xxxx</td>
                                    </tr>

                                </table>
                            </div>
                        </div>



                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <p>使用说明： </p>
                        <p>1、请点击账单月份查询账单明细</p>
                        <p>2、账单记录结算时间为北京时间中午12点</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>--%>
</div>