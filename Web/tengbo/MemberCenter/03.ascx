<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="03.ascx.cs" Inherits="MemberCenter03_ascx" %>

<div class="user_center_content" index="03" reload="<%=this.cardrow==null?"true":"false"%>" style="display: <%=this.css_display%>;">
    <script type="text/javascript">
        function <%=randID("f1")%>_() {
            $('#<%=randID("table_id")%> span.needs').text('');
            var postData = $('#<%=randID("table_id")%>').getPostData();
            var post = true;
            function check(name) {
                if (postData[name] == '') {
                    $('#<%=randID("table_id")%> span.needs[name="' + name + '"]').text('此栏位为必填');
                    post = false;
                }
            }
            check('n1');
            check('n2');
            check('n3');
            if (post) $.nav('03').navshow(null, postData);
        }
    </script>

    <div class="item_title">修改银行资料</div>
    <table id="<%=randID("table_id")%>" class="user_table" border="0">
        <% if (this.cardrow==null) { %>
        <tr><td class="title">开户姓名：  </td><td class="value"><input name="n1" type="text" /><span name="n1" class="needs"></span></td></tr>
        <tr><td class="title">收款银行：  </td><td class="value"><input name="n2" type="text" /><span name="n2" class="needs"></span></td></tr>
        <tr><td class="title">银行账号：  </td><td class="value"><input name="n3" type="text" /><span name="n3" class="needs"></span></td></tr>
        <tr><td class="title">开户省份：  </td><td class="value"><input name="n4" type="text" /></td></tr>
        <tr><td class="title">开户城市：  </td><td class="value"><input name="n5" type="text" /></td></tr>
        <tr><td class="title">开户行网点：</td><td class="value"><input name="n6" type="text" /></td></tr>
        <tr>
            <td class="submit" colspan="2">
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()" />
                <span class="user_errmsg"><%=errmsg%></span>
            </td>
        </tr>
        <% } else { %>
        <tr><td class="title">开户姓名：  </td><td class="value"><%= cardrow.AccountName %></td></tr>
        <tr><td class="title">收款银行：  </td><td class="value"><%= cardrow.BankName    %></td></tr>
        <tr><td class="title">银行账号：  </td><td class="value"><%= cardrow.CardID      %></td></tr>
        <tr><td class="title">开户省份：  </td><td class="value"><%= cardrow.Loc1        %></td></tr>
        <tr><td class="title">开户城市：  </td><td class="value"><%= cardrow.Loc2        %></td></tr>
        <tr><td class="title">开户行网点：</td><td class="value"><%= cardrow.Loc3        %></td></tr>
        <% } %>
    </table>
</div>