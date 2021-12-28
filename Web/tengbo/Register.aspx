<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.Master" AutoEventWireup="true" Inherits="root_aspx" %>
<%@ Import Namespace="web" %>

<script runat="server">

    public bool test_autofill = false;
    public CorpRow corp;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.corp = CorpRow.Cache.Instance.GetCorp(web._Global.DefaultCorpID);
        this.RootMasterPage.ShowNotice = false;
        //this.RootMasterPage.ShowFooter = false;
        //this.RootMasterPage.ShowNav = false;
        //this.RootMasterPage.ShowHeader = false;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var err_8001 = '帐号不可为空白!';
        var err_2005 = '账号已经被使用！';
        var err_0004 = '账号含有不合法字元';

        $.fn.setstate = function (state, msg) {
            var $n1 = $(this).closest('tr').find('.reg3');
            var $n2 = $('.msg.' + state, $n1);
            if ($n2.length == 0) return;
            $('.msg', $n1).hide();
            if (msg) $n2.text(msg);
            $n2.show();
        }

        $(document).ready(function () {
            var op_post = 'post';

            var postData = { id: '', pwd1: '', pwd2: '', name: '', tel: '', qq: '', mail: '', intro: '' };

            function chk_id($this) {
                var _super = this;
                var busy = false;
                var last_chk = '';

                this.getvalue = function (op) {
                    postData.id = $this.val_trim();
                    if (postData.id.length < 3 || postData.id.length >10 || parseInt(postData.id) == 0) {
                        if ((postData.id.length == 0) && (op != op_post))
                            $this.setstate('s1');
                        else
                            $this.setstate('s2', '必须是3-10位数字和字母组合且首位不为0');
                        return false;
                    }
                    if (op == 'post') {
                        return true;
                    }
                    if ((busy == false) && (last_chk != postData.id)) {
                        busy = true;
                        $this.setstate('s4');
                        $.invoke_api({ MemberExist: postData }, {
                            success: function (data, textStatus, jqXHR) {
                                if (data.Status == 1)
                                    $this.setstate('s3');
                                else
                                    $this.setstate('s2', err_2005);
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                $this.setstate('s2', "HTTP {0} : {1}".format(xhr.status, xhr.statusText));
                            },
                            complete: function (jqXHR, textStatus) {
                                last_chk = postData.id;
                                busy = false;
                                window.setTimeout(function () {
                                    $this.trigger('blur');
                                }, 100);
                            }
                        });
                    }
                }

                this.reset = function () {
                    busy = false;
                    last_chk = '';
                    $this.setstate('s1');
                    $this.val('');
                }

                $this.blur(this.getvalue);
                this.reset();
            }

            function chk_pwd($pwd1, $pwd2) {
                this.getvalue = function (op) {
                    postData.pwd1 = $pwd1.val_trim();
                    postData.pwd2 = $pwd2.val_trim();
                    var result = true;
                    if (postData.pwd1.length < 6) {
                        if ((postData.pwd1.length == 0) && (op != op_post))
                            $pwd1.setstate('s1');
                        else
                            $pwd1.setstate('s2');
                        result = false;
                    }
                    else
                        $pwd1.setstate('s3');

                    if ((postData.pwd2.length == 0) && (op != op_post))
                        $pwd2.setstate('s1');
                    else if (postData.pwd1 == postData.pwd2)
                        $pwd2.setstate('s3');
                    else
                        $pwd2.setstate('s2');
                    return result;
                }
                this.reset = function () {
                    $pwd1.setstate('s1');
                    $pwd2.setstate('s1');
                    $pwd1.val('');
                    $pwd2.val('');
                }

                $pwd1.blur(this.getvalue);
                $pwd2.blur(this.getvalue);
                this.reset();
            }

            function chk_input1($this, field_name, min_len) {
                this.getvalue = function (op) {
                    var value = postData[field_name] = $this.val_trim();
                    if (value.length < min_len) {
                        if ((value.length == 0) && (op != op_post))
                            $this.setstate('s1');
                        else
                            $this.setstate('s2');
                        return false;
                    }
                    else
                        $this.setstate('s3');
                    return true;
                }
                this.reset = function () {
                    $this.setstate('s1');
                    $this.val('');
                }
                $this.blur(this.getvalue);
                this.reset();
            }

            function chk_input2($this, field_name) {
                this.getvalue = function (op) {
                    postData[field_name] = $this.val_trim();
                    return true;
                }
                this.reset = function () {
                    $this.setstate('s1');
                    $this.val('');
                }
                $this.blur(this.getvalue);
                this.reset();
            }

            function chk_input3($chk, $label) {
                this.getvalue = function (op) {
                    if ($chk.prop('checked') == true) {
                        $label.removeClass('red_border');
                        return true;
                    }
                    else {
                        if (!$label.hasClass('red_border'))
                            $label.addClass('red_border');
                    }
                }
                this.reset = function () {
                    $chk.prop('checked', true);
                    $chk.removeClass('red_border');
                }
                this.reset();
            }

            function opt_sex(selector, field_name) {
                this.getvalue = function (op) {
                    postData[field_name] = $(selector+':checked').val()
                    return true;
                }
                this.reset = function () {
                    $(selector).setstate('s1');
                    $(selector).prop('checked', false);
                }
                //$this.blur(this.getvalue);
                this.reset();
            }

            var _fields = [
                new chk_id($('#regf_00')),
                new chk_pwd($('#regf_01'), $('#regf_02')),
                new chk_input1($('#regf_03'), 'name', 1),
                new chk_input1($('#regf_04'), 'tel', 3),
                new chk_input1($('#regf_05'), 'qq', 1),
                new chk_input2($('#regf_06'), 'mail'),
                new chk_input2($('#regf_07'), 'intro'),
                new chk_input3($('#regf_a'), $('#regf_aa')),
                new chk_input3($('#regf_b'), $('#regf_bb')),
                new opt_sex('input[name=sex]', 'sex')
            ];

            $('#button3').click(function () {
                for (var i = 0; i < _fields.length; i++)
                    _fields[i].reset();
            });
            $('#button2').click(function () {
                var post = true;
                for (var i = 0; i < _fields.length; i++) {
                    if (_fields[i].getvalue(op_post) != true) {
                        post = false;
                    }
                }
                if (post == true) {
                    $('#reg_form1').block({
                        overlayCSS: { backgroundColor: '#000', opacity: 0.6 },
                        message: '<div class="register_busy"></div>', onBlock: function () {
                            $.invoke_api({ MemberRegister: postData }, {
                                success: function (data, textStatus, jqXHR) {
                                    switch (data.Status) {
                                        case 1:
                                            $('.login_panel').load('<%=ResolveClientUrl("~/login.aspx")%>');
                                            $('.reg_form').hide('blind', null, 500,
                                                function () {
                                                    $('.reg_success').show('blind', null, 300, function () {
                                                        //v((window.opera) ? (document.compatMode == "CSS1Compat" ? $('html') : $('body')) :
                                                        $('html,body').animate({ scrollTop: 0 }, 600);
                                                    });
                                                });
                                            break;
                                        case 8001: $('#regf_00').setstate('s2', err_8001); break;
                                        case 2005: $('#regf_00').setstate('s2', err_2005); break;
                                        case 0004: $('#regf_00').setstate('s2', err_0004); break;
                                        default: $('#regf_00').setstate('s2', err_2005); break;
                                    }
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    $('#reg_err_msg').text('error!');
                                },
                                complete: function (jqXHR, textStatus) {
                                    $('#reg_form1').unblock();
                                }
                            });
                        }
                        //message: '<img src="image/loading_reg.gif" />',
                        //css: { border: 0, backgroundColor: 0, cursor: 'normal' },
                        //overlayCSS: { cursor: 'normal' }
                    });
                }
            });

            <% if (this.test_autofill) { %>
            $('#regf_00').val('<%=System.Security.Cryptography.RandomString.GetRandomString(System.Security.Cryptography.RandomString.LowerLetter, 5)%>');
            $('#regf_01, #regf_02').val('111111');
            $('#regf_03').val('33333');
            $('#regf_04').val('44444');
            $('#regf_05').val('55555');
            $('#regf_06').val('66666');
            $('#regf_07').val('77777');
            <% } %>
        });
        function noSpecialChar(e) {
            var keynum;
            var keychar;
            var numcheck;
            if (window.event) {
                keynum = e.keyCode;
            }
            else if (e.which) {
                keynum = e.which;
            }
            keychar = String.fromCharCode(keynum);
            numcheck = /[\W|_]/g;
            return !numcheck.test(keychar);
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="register">
        <div class="body">
            <ul class="reg-left">
                <li><span>优惠活动</span></li>
                <li><a href="Promotion/01.aspx" target="_promotion_frame">首存优惠    </a></li>
                <li><a href="Promotion/02.aspx" target="_promotion_frame">洗码优惠    </a></li>
                <li><a href="Promotion/03.aspx" target="_promotion_frame">VIP直通车   </a></li>
                <li><a href="Promotion/04.aspx" target="_promotion_frame">好友推荐    </a></li>
                <li><a href="Promotion/05.aspx" target="_promotion_frame">复活礼金    </a></li>
                <li><a href="Promotion/06.aspx" target="_promotion_frame">生日礼金    </a></li>
                <li><a href="Promotion/07.aspx" target="_promotion_frame">晋级奖金    </a></li>
                <li><a href="Promotion/08.aspx" target="_promotion_frame">周周红利    </a></li>
                <li><a href="Promotion/09.aspx" target="_promotion_frame">绿色通道入款</a></li>
            </ul>
            <div class="reg-right" id="reg_form1">
                <div class="reg-title">
				<div class="title0">
				<div class="title1">注册会员</div>
				<div class="title2">Join us</div>
				</div>
				</div>
                <table class="reg_form" border="0" cellpadding="2" cellspacing="0">
					<tr>
						<td colspan="3" class="reg0a">标志*为必填项目</td>
				    </tr>
                    <tr>
                        <td class="reg1"><span>*</span>登录账号：</td>
						<td class="reg2">
                        <input class="inp a" type="text" value="<%=corp.prefix%>" readonly="readonly" /><input onKeyUp="value=value.replace(/^0|[\W|_]/g,'')" onBlur="value=value.replace(/^0|[\W|_]/g,'')" maxlength="10" class="inp b" id="regf_00" type="text" />
						</td>
                        <td class="reg3">
                            <div class="msg s1">账号前自动加&#8220;<span style="color:#ff3"><%=corp.prefix%></span>&#8221;，3-10位数字和字母组合且首位不为0&#8221;</div>
                            <div class="msg s2" style="display: none;">账号已经被使用！</div>
                            <div class="msg s3" style="display: none;">&nbsp;</div>
                            <div class="msg s4" style="display: none;">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="reg1"><span>*</span>登录密码：</td>
						<td class="reg2">
							<input class="inp c" id="regf_01" type="password" onKeyUp="value=stripscript(value);" onBlur="value=stripscript(value);" maxlength="18" /></td>
                        <td class="reg3">
                            <div class="msg s1">六位数字以上英文或数字组合,且不包括【/\#%"'?】这些字符</div>
                            <div class="msg s2" style="display: none;">请输入六位数以上英文或数字组合,且不包括【/\#%"'?】这些字符！</div>
                            <div class="msg s3" style="display: none;">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="reg1"><span>*</span>重复密码：</td>
						<td class="reg2">
						<input class="inp c" id="regf_02" type="password" onKeyUp="value=stripscript(value);" onBlur="value=stripscript(value);" maxlength="18" /></td>
                        <td class="reg3">
                            <div class="msg s1">必须与密码一致</div>
                            <div class="msg s2" style="display: none;">必须与登陆密码保持一致</div>
                            <div class="msg s3" style="display: none;">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="reg1"><span>*</span>真实姓名：</td>
						<td class="reg2">
						<input class="inp c" id="regf_03" type="text" /></td>
                        <td class="reg3">
                            <div class="msg s1">必须与提款姓名一致，注册后不得更改</div>
                            <div class="msg s2" style="display: none;">真实姓名不可为空白</div>
                            <div class="msg s3" style="display: none;">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="reg1"><span>*</span>联系电话：</td>
						<td class="reg2">
						<input class="inp c" id="regf_04" type="text" /></td>
                        <td class="reg3">
                            <div class="msg s1">请输入您的联系方式</div>
                            <div class="msg s2" style="display: none;">请输入您的联系号码</div>
                            <div class="msg s3" style="display: none;">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="reg1"><span>*</span>联系Q Q：</td>
						<td class="reg2">
						<input class="inp c" id="regf_05" type="text" /></td>
                        <td class="reg3">
                            <div class="msg s1">请输入您的常用QQ</div>
                            <div class="msg s2" style="display: none;">请填入您的联系QQ</div>
                            <div class="msg s3" style="display: none;">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="reg1">性&nbsp;别：</td>
                        <td class="reg2">
                            <input type="radio" name="sex" value="1" id='<%=randID("1")%>' /><label for="<%=randID("1")%>">男</label>
                            <input type="radio" name="sex" value="2" id='<%=randID("2")%>' /><label for="<%=randID("2")%>">女</label>
                        </td>
                        <td class="reg3">
                            <div class="msg s1"></div>
                            <div class="msg s2" style="display: none;">&nbsp;</div>
                            <div class="msg s3" style="display: none;">&nbsp;</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="reg1"><span></span>会员邮箱：</td>
						<td class="reg2">
						<input class="inp c" id="regf_06" type="text" /></td>
                        <td class="reg3">
                            <div class="msg s1">请输入您的常用邮箱（非必填）</div>
                            <div class="msg s2" style="display: none;"></div>
                            <div class="msg s3" style="display: none;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="reg1"><span></span>推荐人：</td>
                        <td class="reg2">
                            <input class="inp c" id="regf_07" type="text" /></td>
                        <td class="reg3">
                            <div class="msg s1">请输入推荐人游戏账号(非必填)</div>
                            <div class="msg s2" style="display: none;"></div>
                            <div class="msg s3" style="display: none;"></div>
                        </td>
                    </tr>
					<tr>
						<td colspan="3" class="reg0b">
							<input type="checkbox" id="regf_a"/><label id="regf_aa" class="reg_chk" for="regf_a">提呈申请的同时，本人已超过合法年龄以及本人在此网站的所有活动幷没有抵触本人所身在的国家所管辖的法律。</label></td>
				    </tr>
					<tr>
						<td colspan="3" class="reg0c">
							<input type="checkbox" id="regf_b"/><label id="regf_bb" class="reg_chk" for="regf_b">本人也接受在此项申请下有关的所有规则与条例以及隐私声明。</label></td>
					</tr>
					<tr>
                        <td colspan="3" class="reg0d">
                            <input name="button2" type="submit" class="button2" id="button2" value="确认提交" />
                            <input name="button3" type="submit" class="button2" id="button3" value="重新填写" />
                        </td>
                    </tr>
                </table>
                <div class="reg_success" style="display: none;">
                    <div class="buttons">
                        <a class="button2" href="<%=ResolveClientUrl("~/")%>">回首页</a>
                        <a class="button2" href="<%=ResolveClientUrl("~/MemberCenter")%>">会员中心</a>
                        <a class="button2" href="<%=ResolveClientUrl("~/MemberCenter/Deposit.aspx") %>">我要存款</a>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
