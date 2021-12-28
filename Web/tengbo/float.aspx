<%@ Page Language="C#" AutoEventWireup="true" Inherits="SitePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    <link href="css/com.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript" src="js/jquery.js"></script>
    <script type="text/javascript" language="javascript" src="js/style.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#img_CheckCode').attr('src', 'VerifyImage.ashx?width=80&height=30&type=tryplay');
            $("#btn_TryPlay").click(function () {
                var game = $(".float_game").find(".current");
                if (game.length < 1)
                {
                    $("#td_Message").text("请选择游戏厅");
                    return;
                }
                var gameType = game.eq(0).attr("rel");

                var value = $("#txt_CheckCode").val();
                if (value == "") {
                    $("#td_Message").text("请输入验证码");
                    return;
                }
               
                //$("#td_Message").load("./CheckVerifyCode.ashx?type=tryplay&value=" + value + "&gameType=" + gameType);

                $.ajax({
                    type: "get",
                    url: "./CheckVerifyCode.ashx?type=tryplay&value=" + value,
                    async: false,
                    success: function (data) {
                        var d = $.trim(data);
                        if (d) {
                            $('#img_CheckCode').attr('src', 'VerifyImage.ashx?width=80&height=30&type=tryplay');
                            $('#txt_CheckCode').val('');
                            $("#td_Message").text(d);
                            return;
                        }
                        top.$.unblockUI();
                        $("#playgame" + gameType).click();
                    }
                });
            });
        });
    </script>
</head>

<body>
    <div style="display: none;">
        <a onclick="top.$('#playgame12').playgame(true);" id="playgame12" href="javascript:void(0);" target="_blank">AG国际厅</a>
        <a onclick="top.$('#playgame01').playgame(true);" id="playgame01" href="javascript:void(0);" target="_blank">波音厅</a>
        <a onclick="top.$('#playgame05').playgame(true);" id="playgame05" href="javascript:void(0);" target="_blank">HG厅</a>
    </div>
    <div class="float">
        <table width="383" border="0" cellspacing="0" cellpadding="0" style="line-height: 30px;">
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="28" rowspan="2" valign="top">
                                <img src="<%=GetImage("~/image/f_1.png")%>" width="23" height="35" /></td>
                            <td class="float_text">请选择游戏厅</td>
                        </tr>
                        <tr>
                            <td height="55">
                                <div class="float_game">
                                    <ul>
                                        <li rel="12">AG国际厅</li>
                                        <li rel="01">波音厅</li>
                                        <li rel="05">HG厅</li>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="28" rowspan="2" valign="top">
                                <img src="<%=GetImage("~/image/f_2.png")%>" width="23" height="30" /></td>
                            <td class="float_text">请输入验证码</td>
                        </tr>
                        <tr>
                            <td height="55">
                                <form id="form1" name="form1" method="post" action="">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="120">
                                                <label>
                                                    <input name="textfield" maxlength="4" type="text" id="txt_CheckCode" class="input_yz" />
                                                </label>
                                            </td>
                                            <td width="85">
                                                <%--<img src="<%=GetImage("~/image/f_yanzhen.gif")%>" width="80" height="30" />--%>
                                                <img id="img_CheckCode" src="<%=ResolveClientUrl("~/VerifyImage.ashx?width=80&height=30&type=tryplay")%>" width="80" height="30" style="cursor: pointer;" title="点击刷新" onclick="$(this).attr('src', '<%=ResolveClientUrl("~/VerifyImage.ashx?width=80&height=30&type=tryplay")%>');" />
                                            </td>
                                            <td class="white" width="140"><%--*请输入验证码--%></td>
                                        </tr>
                                    </table>
                                </form>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="35" valign="middle">
                                <img src="<%=GetImage("~/image/f_3.png")%>" width="23" height="33" /></td>
                            <td height="50">
                                <label>
                                    <input name="button" type="image" id="btn_TryPlay" value="提交" src="<%=GetImage("~/image/f_bt1.gif")%>" />
                                </label>
                            </td>
                            <td id="td_Message" width="150" style="text-align: left;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
