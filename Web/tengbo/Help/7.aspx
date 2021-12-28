<%@ Page Title="" Language="C#" MasterPageFile="Help.master" AutoEventWireup="true" Inherits="HelpPage" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.HelpIndex = 7;
    }
</script>

<asp:Content ContentPlaceHolderID="help2" runat="server">
    <div class="help-title">
        <div class="title0">提款流程</div>
    </div>
    <script type="text/javascript">
        $(function () {
            $(".money_tabs a").click(function () {
                $(this).parent().siblings().removeClass("cur");
                $(this).parent().addClass("cur");
                $(".tab_h2").hide();
                $("." + $(this).attr("rel") + "").show();
            })
        });
    </script>
    <div class="quest_con">
        <div class="money_con">
            <div class="money_tabs">
                <ul>
                    <li class="cur"><a href="javascript:void(0)" rel="tab-1">第一步</a></li>
                    <li><a href="javascript:void(0)" rel="tab-2">第二步</a></li>
                    <li><a href="javascript:void(0)" rel="tab-3">第三步</a></li>
                </ul>
                <div>
                    <div class="tab_h2 tab-1">
                        <div class="tab_txt"><span class="tab_txt_b">第一步：</span>登陆账号，点击“我要提款”或者点击“会员中心”再选择“会员提款”</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/tk_1.png")%>" width="501" height="84" />
                        </div>
                    </div>
                    <div class="tab_h2 tab-2" style="display: none">
                        <div class="tab_txt"><span class="tab_txt_b">第二步：</span>完善您的提款银行资料 （如果之前您已经完善，该步会自动省略掉）</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/tk_2.png")%>" width="633" height="463" />
                        </div>
                    </div>

                    <div class="tab_h2 tab-3" style="display: none">
                        <div class="tab_txt">
                            <span class="tab_txt_b">第三步：</span>核对您的收款银行资料，并填写您要提款的金额和安全密码，提交后即可<br />
                            （如绑定的银行资料有误， 请联系客服更改）
                        </div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/tk_3.png")%>" width="633" height="432" />
                        </div>
                    </div>
                    <%--<div class="tab_h2" id="tab-4" style="display: none">
                        <div class="tab_txt"><span class="tab_txt_b">第四步：</span>提交后选择要支付的银行，并按照第三方支付的要求步骤操作</div>
                        <div class="tab_pic">
                            <img src="image/zxzf_4.png" width="633" height="251" />
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
