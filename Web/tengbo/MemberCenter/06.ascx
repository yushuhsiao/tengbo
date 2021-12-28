<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="06.ascx.cs" Inherits="MemberCenter06_ascx" %>

<%--<script src="js/jquery.js"></script>--%>
<script type="text/javascript" >
    function <%=randID("f1")%>_() {
        $('#<%=randID("table_id")%> .user_errmsg').text('');
        var postData = $('#<%=randID("table_id")%>').getPostData();
        var post = true;
        function check(name) {
            if (postData.hasOwnProperty(name)) {
                if (postData[name] == '') {
                    console.log($('#<%=randID("table_id")%> input[name="' + name + '"]').closest('td.value').find('span.needs'));
                    $('#<%=randID("table_id")%> input[name="' + name + '"]').closest('td.value').find('span.needs').show();
                    post = false;
                }
            }
        }
        //console.log(post, postData);
        <% if (this.cardrow==null) { %>
        check('n1');
        check('n2');
        check('n3');
        <% } %>
        check('n7');
        check('n8');
        if (post) $.nav('06').navshow('06', postData);
        //$('#<%=randID("table_id")%> .user_errmsg').text('xxxxxxx');
        //console.log(post, postData);
    }
</script>

<div class="user_center_content" index="06" style="display: <%=this.css_display%>;">
<div class="item_title">会员提款</div>
        <% if (this.tran_result != null) { %>
        <% } %>
    <table id="<%=randID("table_id")%>" class="user_table" border="0">
        <tr><td class="title" style="color: yellow;">注意：     </td><td class="value">第一次提款的用户请先设定您的安全密码</td></tr>
        <% if (this.cardrow==null) { %>
        <tr><td class="title">开户姓名：  </td><td class="value"><input name="n1" type="text" /><span class="needs" style="display: none;">此栏位为必填</span></td></tr>
        <tr><td class="title">收款银行：  </td><td class="value"><input name="n2" type="text" /><span class="needs" style="display: none;">此栏位为必填</span></td></tr>
        <tr><td class="title">银行账号：  </td><td class="value"><input name="n3" type="text" /><span class="needs" style="display: none;">此栏位为必填</span></td></tr>
        <tr><td class="title">开户省份：  </td><td class="value"><input name="n4" type="text" /></td></tr>
        <tr><td class="title">开户城市：  </td><td class="value"><input name="n5" type="text" /></td></tr>
        <tr><td class="title">开户行网点：</td><td class="value"><input name="n6" type="text" /></td></tr>
        <% } else { %>
        <tr><td class="title">开户姓名：  </td><td class="value"><%= cardrow.AccountName %></td></tr>
        <tr><td class="title">收款银行：  </td><td class="value"><%= cardrow.BankName    %></td></tr>
        <tr><td class="title">银行账号：  </td><td class="value"><%= cardrow.CardID      %></td></tr>
        <tr><td class="title">开户省份：  </td><td class="value"><%= cardrow.Loc1        %></td></tr>
        <tr><td class="title">开户城市：  </td><td class="value"><%= cardrow.Loc2        %></td></tr>
        <tr><td class="title">开户行网点：</td><td class="value"><%= cardrow.Loc3        %></td></tr>
        <% } %>
        <% if (this.tran_result == null) { %>
        <tr><td class="title">提款金额：  </td><td class="value"><input name="n7" type="text" /><span class="needs" style="display: none;">请填写提款金额</span></td></tr>
        <tr><td class="title">安全密码：  </td><td class="value"><input name="n8" type="password" /><span class="needs" style="display: none;">请填写安全密码</span></td></tr>
        <tr><td class="title">备注填写：  </td><td class="value"><textarea name="n9" rows="2" cols="30"></textarea> </td></tr>
        <tr>
            <td class="submit" colspan="2">
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()" />
            </td>
        </tr>
        <tr>
            <td class="title"><%=errtitle %></td>
            <td><span class="user_errmsg" style="color: red;"><%=errmsg%></span></td>
        </tr>
        <tr>
            <td class="title" style="color: yellow;">备注：</td>
            <td class="value" style="color: yellow; line-height: 14px;">单次提款不能少于100元！<br />每天提款次数不限，当天提款次数超过三次的按单笔30元收取手续费<br />(当天00:00:00-次日23:59:59) 敬请谅解！</td>
        </tr>
        <% } else { %>
        <tr><td class="title">提款金额：  </td><td class="value"><%=this.tran_result.Amount%></td></tr>
        <tr><td class="title">备注填写：  </td><td class="value"><%=this.tran_result.b_BankName%></td></tr>
        <tr><td class="title item_title" colspan="2" style="text-align: center">您的提款要求已送出，我们会尽快为您处理</td></tr>
        <% } %>
    </table>
</div>