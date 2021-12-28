<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="01.ascx.cs" Inherits="MemberCenter01_ascx" %>

<div class="user_center_content" index="01" reload="true" style="display: <%=this.css_display%>;">
    <script type="text/javascript">
        $(document).ready(function () {
            var monthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            var $d = [$('#user_dd').find('[value=29]'), $('#user_dd').find('[value=30]'), $('#user_dd').find('[value=31]')];
            $('#user_yy, #user_mm').change(function () {
                var yy = $('#user_yy').val();
                var mm = $('#user_mm').val();
                var days = monthDays[mm - 1] - 28;
                if ((mm == 2) && (yy % 4) == 0)
                    days++;
                for (var i = 0; i < $d.length; i++) {
                    if (i < days) {
                        $d[i].appendTo($('#user_dd'));
                    }
                    else {
                        $d[i].remove();
                    }
                }
            });
        });

        function <%=randID("f1")%>_() {
            $.nav('01').navshow(null, $('#<%=randID("table_id")%>').getPostData())
        }
    </script>

    <div class="item_title">修改基本信息</div>
    <table id="<%=randID("table_id")%>" class="user_table" border="0">
        <tr>
            <td class="title">性别：</td>
            <td class="value" id="user_sex">
                <input type="radio" name="n1" value="1" id="<%=randID("1")%>" <% if ((byte?)row.Sex == 1) { %> checked="checked" <% } %> /><label for="<%=randID("1")%>">男</label>
                <input type="radio" name="n1" value="2" id="<%=randID("2")%>" <% if ((byte?)row.Sex == 2) { %> checked="checked" <% } %> /><label for="<%=randID("2")%>">女</label>
                <input type="radio" name="n1" value="0" id="<%=randID("0")%>" <% if ((byte?)row.Sex == 0) { %> checked="checked" <% } %> /><label for="<%=randID("0")%>">不填寫</label>
            </td>
        </tr>
        <tr>
            <td class="title">出生日期：</td>
            <td class="value" id="user_birthday">
                <%
                    DateTime bd = row.Birthday ?? DateTime.Now.AddYears(-18);
                    DateTime y1 = DateTime.Now.AddYears(-10);
                    DateTime y2 = y1.AddYears(-60);
                    DateTime m1 = DateTimeEx.UnixBaseTime;
                    DateTime m2 = m1.AddYears(1);
                    int d1 = 1;
                    int d2 = 31;
                    if (row.Birthday.HasValue)
                    {
                        y1 = row.Birthday.Value;
                        y2 = y1.AddDays(-1);
                        m1 = row.Birthday.Value;
                        m2 = m1.AddDays(1);
                        d1 = row.Birthday.Value.Day;
                        d2 = d1;
                    } %>
                <select name="n2"><% for (; y1 > y2; y1 = y1.AddYears(-1)) { %><option <% if (y1.Year == bd.Year) { %> selected="selected" <% } %> value="<%=y1.Year%>"><%=y1.Year%>年</option> <% } %></select>
                <select name="n3"><% for (; m1 < m2; m1 = m1.AddMonths(1)) { %><option <% if (m1.Month == bd.Month) { %> selected="selected" <% } %> value="<%=m1.Month%>"><%=m1.ToString("MMM")%></option> <% } %></select>
                <select name="n4"><% for (; d1 <= d2; d1++) { %><option <% if (d1 == bd.Day) { %> selected="selected" <% } %> value="<%=d1%>"><%=d1%>日</option> <% } %></select>
            </td>
        </tr>
        <tr><td class="title">联系电话：</td><td class="value"><input name="n5" type="text" value="<%= row.Tel %>" /></td></tr>
        <tr><td class="title">联系地址：</td><td class="value"><input name="n6" type="text" value="<%= row.Addr %>" /></td></tr>
        <tr><td class="title">邮箱地址：</td><td class="value"><input name="n7" type="text" value="<%= row.Mail %>" /></td></tr>
        <tr><td class="title">联系QQ：</td><td class="value"><input name="n8" type="text" value="<%= row.QQ %>" /></td></tr>
        <tr><td class="title">相关备注：</td><td class="value"><textarea name="n9" rows="2" cols="30"><%= row.UserMemo %></textarea></td></tr>
        <tr>
            <td class="submit" colspan="2">
                <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()" />
                <span class="user_errmsg"><%=errmsg%></span>
            </td>
        </tr>
    </table>
</div>
