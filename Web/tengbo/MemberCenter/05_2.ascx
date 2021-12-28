<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="05_2.ascx.cs" Inherits="MemberCenter05_2_ascx" %>

<script runat="server">
    static string[] s_AllBankList = new string[] { "中国工商银行", "中国农业银行", "中国建设银行", "中国招商银行", "中国民生银行" };
    protected override string[] AllBankList { get { return s_AllBankList; } }
</script>

<div class="user_center_content" index="05_2" reload="true" style="display: <%=this.css_display%>;">
    <% if (this.row == null) { %>
    <script type="text/javascript">
        function <%=randID("f1")%>_() {
            var postData = $('#<%=randID("table1_id")%>').getPostData();
            if (postData.n01 == null) {
                $('#<%=randID("table1_id")%> .user_errmsg').text('请选择存款银行');
            }
            else if (!$.isNumeric(postData.n02)) {
                $('#<%=randID("table1_id")%> .user_errmsg').text('请输入存款金额');
            }
            else {
                $('#<%=randID("table1_id")%> input[name="n01"]:checked').prop('checked', false);
                $('#<%=randID("table1_id")%> input[name="n02"]').val('');
                $.nav('05').navshow('05_2', postData);
            }
        }
    </script>
    <div class="item_title">绿色通道入款</div>
    <table id="<%=randID("table1_id")%>" class="user_table" border="0" style="width: 100%;">
        <tr>
            <td class="title" style="text-align: right; width: 102px;">存款银行：</td>
            <td class="value" style="line-height: 30px;text-align: left;"> <% for (int i = 0; i < this.AllBankList.Length; i++) { string bk_name = this.AllBankList[i]; %>
                <input type="radio" name="n01" id="bank_<%=i%>" value="<%=i%>" <%if (this.GetCount(bk_name) == 0) { %> disabled="disabled" <% } %> />
                <label for="bank_<%=i%>"><%=bk_name%></label><br /><% } %>
            </td>
        </tr>
        <tr><td class="title" style="text-align: right; width: 102px;">存款金额：</td><td class="value" style="text-align: left;"><input name="n02" type="text" /></td></tr>
        <tr><td class="title"></td><td class="value" style="color: yellow;">建议存款时请添加零头 例如：存500元的可以存501或508，以便我们优先为您处理！</td></tr>
        <tr>
            <td></td>
            <td class="submit" style="text-align: left;padding-left: 7px;">
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()" />
                <span class="user_errmsg"></span>
        </td></tr>
        <tr><td colspan="2">
            
        </td></tr>
    </table>
    <div class="prompt">银行转账提示：
                <ol>
                    <%--<li>绿色通道入款包括有网银转账、ATM、银行柜台、支付宝等，金额不限。</li>--%>
                    <li>每次存款前，请注意查看我们最新的存款银行账户信息，如因转账错误导致的损失，公司不承<br />担任何责任！如果您有什么问题请联系在线客服！</li>
                    <li>绿色通道入款的用户我们将自动为您添加1%存款优惠。</li>
                    <li>建议汇款时增添一个零头，例如您要汇款1000元的话可以汇款1002方便我们尽快为您添加额<br />度。</li>
                </ol>
            </div>
    <% } else { %>
    <script type="text/javascript" >
        function <%=randID("f2")%>_() {
            var postData = $('#<%=randID("table2_id")%>').getPostData();
            postData.n03 = '<%=row.ID%>';
            $.nav('05').navshow('05_2', postData);
        }
    </script>
    <%
        string bankName = row.a_BankName;
        string bankUrl = string.Empty;
        if (bankName == "中国工商银行")
        {
            bankUrl = "http://www.icbc.com.cn/icbc/";
        }
        else if (bankName == "中国农业银行")
        {
            bankUrl = "http://www.abchina.com/cn/wydl/grwydl/default.htm";
        }
        else if (bankName == "中国建设银行")
        {
            bankUrl = "http://www.ccb.com/cn/home/index.html";
        }
        else if (bankName == "中国招商银行")
        {
            bankUrl = "http://www.cmbchina.com/";
        }
        else if (bankName == "中国民生银行")
        {
            bankUrl = "https://per.cmbc.com.cn/pweb/static/login.html";
        }
    %>
    <div class="item_title">绿色通道入款</div>
    <table id="<%=randID("table2_id")%>" class="user_table" border="0" style="<% if (this.tran_end == true) { %>line-height: 28px; <% } %>">
        <tr><td class="title">收款银行：  </td><td class="value"><%= row.a_BankName %></td></tr>
        <tr><td class="title">账（卡）号：</td><td class="value"><%= row.a_CardID %></td></tr>
        <tr><td class="title">户名：      </td><td class="value"><%= row.a_Name %></td></tr>
        <tr><td class="title">存款金额：  </td><td class="value"><%= string.Format("{0:0.00}", row.Amount) %> RMB</td></tr>
        <% if (this.tran_end == false) { %>
        <tr style="display: none"><td class="title">交易流水号：</td><td class="value"><input name="n04" type="text" /></td></tr>
        <tr><td class="title">付款人姓名：</td><td class="value"><input name="n05" type="text" /></td></tr>
        <tr style="display: none"><td class="title">账（卡）号：</td><td class="value"><input name="n06" type="text" /></td></tr>
        <tr><td class="title">备注：      </td><td class="value"><textarea rows="2" cols="30" name="n07"></textarea><label style="color: yellow;">(可不填)</label> </td></tr>
        <tr>
            <td class="submit" colspan="2">
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f2")%>_()" />
                <span class="user_errmsg"><%=errmsg%></span>
            </td>
        </tr>
        <tr><td class="title">说明：</td><td>完成支付后，请填写您的姓名。备注可不用填写或写上您的需求。<br />例如：请收到款后直接帮转到AG国际厅。</td></tr>
        <% } else { %>
        <tr style="display: none"><td class="title">交易流水号：</td><td class="value"><%= row.b_TranSerial %></td></tr>
        <tr><td class="title">付款人姓名：</td><td class="value"><%= row.b_Name %></td></tr>
        <tr style="display: none"><td class="title">账（卡）号：</td><td class="value"><%= row.b_CardID %></td></tr>
        <tr><td class="title">备注：      </td><td class="value"><%= row.b_TranMemo %></td></tr>
        <tr><td></td><td><a href="<%= bankUrl %>" class="info_button" target="_blank" style="padding: 10px;">立即转账</a></td></tr>
        <tr height="40"><td></td><td></td></tr>
        <tr><td class="title">注意：      </td><td class="value" width="500px"> 点击“立即转账”后会进入您的银行网银页面，请复制以上银行账号和收款人信息！ 通过绿色通道支付的会员将获得额外1%存款优惠，单笔封顶50元！</td></tr>
        <% } %>
    </table>
    <% } %>
</div>