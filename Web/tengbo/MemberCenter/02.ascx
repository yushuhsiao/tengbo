<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="02.ascx.cs" Inherits="MemberCenter02_ascx" %>

<div class="user_center_content" index="02" reload="ture" style="display: <%=this.css_display%>;">
    <script type="text/javascript">
        function <%=randID("f1")%>_() {
            var errmsg = '';
            var postData = $('#<%=randID("table_id")%>').getPostData();

            if (postData.pwd0 == '')
                errmsg = '<%=err0%>';
            else if (postData.pwd1 == '')
                errmsg = '<%=err1%>'
            else if (postData.pwd1 != postData.pwd2)
                errmsg = '<%=err2%>'

            if (errmsg != '') {
                $('#<%=randID("table_id")%> .user_errmsg').text(errmsg);
            }
            else {
                $.nav('02').navshow(null, postData);
            }
        }
    </script>

    <div class="item_title">修改密码</div>
    <%
        if (!updateState)
        {
    %>
            <table id="<%=randID("table_id")%>" class="user_table" border="0">
                <tr><td class="title">原始密码：  </td><td class="value"><input name="pwd0" type="password" /></td></tr>
                <tr><td class="title">新密码：    </td><td class="value"><input name="pwd1" type="password" onKeyUp="value=stripscript(value);" onBlur="value=stripscript(value);" maxlength="18" /><span class="needs">注意：6位以上</span></td></tr>
                <tr><td class="title">确认新密码：</td><td class="value"><input name="pwd2" type="password" onKeyUp="value=stripscript(value);" onBlur="value=stripscript(value);" maxlength="18" /></td></tr>
                <tr>
                    <td class="submit" colspan="2">
                        <input type="button" class="info_button" value="提交" onclick="<%=randID("f1")%>_()" />
                        <span class="user_errmsg"><%=errmsg%></span>
                    </td>
                </tr>
            </table>        
    <%
        }
        else
        { 
    %>
            <center style="margin-top: 50px;font-size: 18px; color: red;"><%=errmsg%></center>
    <%       
        }
    %>
    
</div>
