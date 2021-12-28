<%@ Control Language="C#" AutoEventWireup="true" Inherits="SiteLogControl" %>
<%@Import Namespace="BU" %>
<%@Import Namespace="System.Data" %>
<%@Import Namespace="System.Data.SqlClient" %>
<script runat="server">
    protected override string nav_index { get { return "08"; } }
</script>

<div class="user_center_content" index="<%=this.content_index_s%>" <% if (!this.reload) { %> reload="false"<% } %> style="display: <%=this.css_display%>;">
<div class="item_title">存款记录</div>
<div class="log_title">请点击选择您需要的账单月份：</div>
<ul class="log_nav"><% foreach (int i in this.log_nav) { %>
    <li onclick="$.nav('<%=this.nav_index%>').navshow('<%=this.items[i]%>')"><div <% if (i == this.content_index) { %> class="cur"<% } %>><%=this.titles[i]%></div></li> <% } %>
</ul>
<div class="log_title"><%=string.Format("{0:yyyy}年{0:MM}", this.times[this.content_index])%>月份存款记录明细如下：</div>
<div style="overflow-x:auto;height:345px;">
    <table class="log_detail" style="width:75%; margin-left: 15px" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <th>存款日期</th>
        <th>存款金额</th>
    </tr> <% 
        foreach (SqlDataReader r in this.GetDeposit())
        {
            if (r == null) { %><tr><td colspan="2">无记录</td></tr><% } else { %>
    <tr>
        <td><%=string.Format("{0:yyyy/MM/dd HH:mm}", r.GetValue("CreateTime"))%></td>
        <td><%=string.Format("{0:0.00}", Math.Abs(r.GetDecimal("Amount")))%></td>
    </tr><% }
        } %>
</table>

</div>
<div class="prompt" style="margin-top: 5px;">
    使用说明：1、请点击账单月份查询账单明细&nbsp; 2、账单记录结算时间为北京时间中午12点
</div>
</div>