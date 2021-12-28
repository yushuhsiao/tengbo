<%@ Page Language="C#" AutoEventWireup="true" Inherits="SitePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    <link href="css/com.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript"> (function ($) { jQuery.extend({ invoke_api: function (command, options) { return $.ajax($.extend(true, { type: 'post', url: '<%=ResolveClientUrl("~/api.ashx")%>', dataType: 'json', cache: false, async: true, data: { str: JSON.stringify(command) } }, options)); } }); })(jQuery); </script>
    <script type="text/javascript">
        $.fn.getPostData = function (n) { var t = {}; return $('input[type="text"], input[type="password"], input[type="radio"]:checked, select, textarea', this).each(function () { var i = $(this).prop("name") || ""; i.length > 0 && (t[i] = $.trim($(this).val()) || "", n == !0 && $(this).val("")) }), t }
        $(function () {
            var loginInfoTag = $(".login_Info .td2").css({ "font-weight": "normal" });

            $(".t0").focus(function () {
                loginInfoTag.text("");
            });

            $('#btn_ok').click(function () {
                var postData = $('.table1').getPostData();

                if (postData.t7 == '') {
                    if (postData.t1 == '') { loginInfoTag.text("请输入帐号"); return }
                    if (postData.t2 == '') { loginInfoTag.text("请输入验证身份密码"); return; }
                    if (postData.t6 == '') { loginInfoTag.text("请输入验证码"); return; }
                }
                else {
                    if (postData.t3 == '') { loginInfoTag.text("请输入新密码"); return }
                    if (postData.t4 != postData.t3) { loginInfoTag.text("两次密码输入不一致"); return; }
                    if (postData.t5 == '') { loginInfoTag.text("请输入验证身份密码"); return; }
                }
                loginInfoTag.text("");

                $.invoke_api({ RecoveryPassword: postData }, {
                    success: function (data, textStatus, jqXHR) {

                        switch (data.status) {
                            case 'accept':
                                $("#t7").val(data.t7);
                                $(".login_hide").hide();
                                $(".login_show").show();
                                break;
                            case 'ok':
                                $(".login_hide").hide();
                                $(".login_show").hide();
                                $(".mima_tj").hide();
                                $(".login_Success").show();
                                break;
                            default:
                                $('.t0').val('');
                                $("#t7").val("");
                                $(".login_hide").show();
                                $(".login_show").hide();
                                loginInfoTag.text(data.status == 'verify' ? '验证码错误' : '用户名或密码错误' + '，请重新填写!');
                                $('#img_CheckCode').attr('src', 'VerifyImage.ashx?width=80&height=30&type=recovery'); $('#t6').val('');
                                break;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        loginInfoTag.text(xhr.responseText);
                        postData.t7 = '';
                        $('#img_CheckCode').attr('src', 'VerifyImage.ashx?width=80&height=30&type=recovery'); $('#t6').val('');
                    }
                });
            });

        });
    </script>
    <style type="text/css">
        .table1 {
            color: #fff;
            font-size: 14px;
            margin-top: 10px;
            font-weight: bold;
            border-collapse: separate;
            border-spacing: 10px;
            width: 415px;
        }

        .td1 {
            width: 92px;
            text-align: right;
        }

        input {
            width: 212px;
            height: 22px;
        }

        .td3 {
            width: 125px;
        }

            .td3 input {
                width: 120px;
            }
    </style>
</head>
<body>
    <div class="float" id="main">
        <div class="cqk_ck">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center">
                        <table class="table1" border="0" align="center">
                            <tr class="login_hide">
                                <td class="td1">会员账号：</td>
                                <td class="td2" colspan="2">
                                    <input class="t0" id="t1" name="t1" type="text" /></td>
                            </tr>
                            <tr class="login_hide">
                                <td class="td1">安全密码：</td>
                                <td class="td2" colspan="2">
                                    <input class="t0" id="t2" name="t2" type="password" /></td>
                            </tr>
                            <tr class="login_show" style="display: none;">
                                <td class="td1">新密码：</td>
                                <td class="td2" colspan="2">
                                    <input class="t0" id="t3" name="t3" type="password" /></td>
                            </tr>
                            <tr class="login_show" style="display: none;">
                                <td class="td1">确认密码：</td>
                                <td class="td2" colspan="2">
                                    <input class="t0" id="t4" name="t4" type="password" /></td>
                            </tr>
                            <tr class="login_show" style="display: none;">
                                <td class="td1">安全密码：</td>
                                <td class="td2" colspan="2">
                                    <input class="t0" id="t5" name="t5" type="password" /></td>
                            </tr>
                            <tr class="login_hide">
                                <td class="td1">验证码：</td>
                                <td class="td3">
                                    <input class="t0" id="t6" name="t6" type="text" maxlength="4" /></td>
                                <td class="td4">
                                    <img id="img_CheckCode" src="<%=ResolveClientUrl("~/VerifyImage.ashx?width=80&height=30&type=recovery")%>" width="80" height="30" style="cursor: pointer;" title="点击刷新" onclick="$(this).attr('src', '<%=ResolveClientUrl("~/VerifyImage.ashx?width=80&height=30&type=tryplay")%>');" />
                                </td>
                            </tr>
                            <tr class="login_Info" height="18">
                                <td class="td1">
                                    <input type="text" id="t7" name="t7" style="display: none;" /></td>
                                <td class="td2" colspan="2" width="290"></td>
                            </tr>
                            <tr class="login_Success" style="display: none;">
                                <td colspan="3" style="padding-left: 65px;">您的密码修改成功！</td>
                            </tr>
                            <tr class="login_Success" style="display: none;">
                                <td colspan="3" style="padding-left: 65px;">如有疑问您可以点击下面按钮联系在线客服</td>
                            </tr>
                            <tr class="login_Success" style="display: none;">
                                <td colspan="3" style="padding-left: 65px;">
                                    <input type="button" value="在线客服" style="width: 80px;" onclick="window.parent.live800_chat();window.parent.$.unblockUI();" /><input type="button" value="关闭" style="width: 80px; margin-left: 30px;" onclick="window.parent.$.unblockUI();" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="mima_tj" style="padding-left: 112px;">
            <span id="btn_ok" style="padding-right: 20px; cursor: pointer;">
                <img src="image/queren2.gif" width="80" height="34" /></span>
            <span onclick="window.parent.$.unblockUI();" style="cursor: pointer;"><a>
                <img src="image/quxiao2.gif" width="80" height="34" /></a></span>
        </div>
    </div>
</body>
</html>
