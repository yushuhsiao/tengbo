<%@ Page Language="C#" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="none" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript" src="js/util.js"></script>
    <title></title>

    <script type="text/javascript">
        var k = "";
        window.onkeypress = function (e) {
            //console.log(arguments);
            if (e.charCode == 13) {
                console.log(k, e.charCode);
                k = "";
            }
            else {
                k += String.fromCharCode(e.charCode);
            }
        }

        $(document).ready(function() {
            $('.t1').attr('_tab', '.t2');
            $('.t2').attr('_tab', '.tA');
            $('.t1,.t2').focus(function () {
                this.select();
            }).keypress(function () {
                if (event.keyCode == 13) {
                    var sender = $(this);
                    if ((sender.attr('_allownull') == 1) || ($.trim(sender.val()) != '')) {
                        event.keyCode = null;
                        $(sender.attr('_tab')).focus();
                    }
                }
            });
            $('.tA').click(function () {
                var tB = $('.tB'), t1, t2;
                if (tB == null) return;
                else if ((t1 = $.trim($('.t1').val())) == '') tB.text('<%= web.Lang.GetLang("", "loginmsg_name") %>');
                else if ((t2 = $.trim($('.t2').val())) == '') tB.text('<%= web.Lang.GetLang("", "loginmsg_pwd") %>');
                else {
                    tB.text("wait...");
                    $.invoke_api({ AdminLogin: { t1: t1, t2: t2 } }, {
                        success: function (data, textStatus, jqXHR) {
                            if (data.Status) {
                                if (data.Status == 1) {
                                    window.location.reload();
                                } else {
                                    tB.text(data.Message);
                                }
                                return;
                            }
                            if (data.LoginResult) {
                                if (data.LoginResult.t1 == 1) {
                                    window.location.reload();
                                }
                                else {
                                    tB.text(data.LoginResult.t2);
                                }
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            tB.text('Error ' + xhr.status);
                        }
                    });
                }
            });
            $('.t1').focus();
        });
    </script>

    <style type="text/css">
        html, body { margin:0; padding:0; height:100%; }
        #floater { float:left; height:50%; margin-bottom:-60px;
                   width:1px; /* only for IE7 */
        }
        .form { margin:0 auto; border-style: double; empty-cells: show; background-color: #dddddd; border-radius: 10px; clear:both; height:60px; position:relative; }
        .form td { white-space: nowrap; text-align: left; }
        .t1 { width: 80px; border-radius: 10px; }
        .t2 { width: 80px; border-radius: 10px; }
        .tA { width: 80px; border-radius: 10px; }
        .tB { color: Blue; }
    </style>
</head>
<body>
    <div id="floater"></div>
    <table class="form">
        <tr>
            <td><%= web.Lang.GetLang("", "login_name") %></td>
            <td><input class="t1" type="text" /></td>
            <td><%= web.Lang.GetLang("", "login_pwd") %></td>
            <td><input class="t2" type="password" /></td>
            <td><input class="tA" type="button" value="<%= web.Lang.GetLang("", "login_btn") %>" /></td>
        </tr>
        <tr><td class="tB" colspan="5">&nbsp;</td></tr>
    </table>
</body>

