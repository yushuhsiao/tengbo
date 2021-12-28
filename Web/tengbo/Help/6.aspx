<%@ Page Title="" Language="C#" MasterPageFile="Help.master" AutoEventWireup="true" Inherits="HelpPage" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.HelpIndex = 6;
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.dep1').click(function () {
                var pos = $(this).position();
                pos.width = $(this).width();
                pos.height = $(this).height();
                $('#test').text(JSON.stringify(pos));
            });
            $('.dep1:eq(0)').trigger('click');
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $(".money_tit a").click(function () {
                $(this).siblings().removeClass("link_h");
                $(this).addClass("link_h");
                $(".money_con").hide();
                var rel = $(this).attr("rel");
                var curBox = $("." + rel + "").show();
                curBox.find("li").removeClass("cur");
                curBox.find("li:eq(0)").addClass("cur");
                curBox.find(".tab_h2").hide();
                curBox.find(".tab_h2:eq(0)").show();
            });


            $(".money_tabs a").click(function () {
                $(this).parent().siblings().removeClass("cur");
                $(this).parent().addClass("cur");
                $(this).closest('.money_con').find('.tab_h2').hide();
                $(this).closest('.money_con').find('.' + $(this).attr("rel")).show();
            });
        });
    </script>
    <style type="text/css">
        .dep1 {
            cursor: pointer;
            float: left;
        }

        .act {
            height: 5px;
            background-color: #dddddd;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="help2" runat="server">
    <div class="help-title">
        <div class="title0">存款流程</div>
    </div>
    <div class="quest_con">
        <div class="money_tit">
            <a href="javascript:void(0)" class="link_h" rel="lxtd">绿色通道支付</a>
            <a href="javascript:void(0)" rel="zxzf">在线支付</a>
        </div>
        <div class="money_con lxtd">
            <div class="money_tabs">
                <ul>
                    <li class="cur"><a href="javascript:void(0)" rel="tab-1">第一步</a></li>
                    <li><a href="javascript:void(0)" rel="tab-2">第二步</a></li>
                    <li><a href="javascript:void(0)" rel="tab-3">第三步</a></li>
                    <li><a href="javascript:void(0)" rel="tab-4">第四步</a></li>
                </ul>
                <div>
                    <div class="tab_h2 tab-1">
                        <div class="tab_txt"><span class="tab_txt_b">第一步：</span>登陆账号，点击“我要存款”或者点击“会员中心”再选择“会员存款”</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/lstd_1.png")%>" width="528" height="84" />
                        </div>
                    </div>
                    <div class="tab_h2 tab-2" style="display: none">
                        <div class="tab_txt"><span class="tab_txt_b">第二步：</span>点击选择”绿色通道支付”</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/lstd_2.png")%>" width="637" height="349" />
                        </div>
                    </div>

                    <div class="tab_h2 tab-3" style="display: none">
                        <div class="tab_txt"><span class="tab_txt_b">第三步：</span>选择“存款银行”和“填写存款金额并提交</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/lstd_3.png")%>" width="640" height="465" />
                        </div>
                    </div>
                    <div class="tab_h2 tab-4" style="display: none">
                        <div class="tab_txt"><span class="tab_txt_b">第四步：</span>存款成功后，请填写下方信息提交即可</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/lstd_4.png")%>" width="635" height="407" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="money_con zxzf" style="display: none">
            <div class="money_tabs">
                <ul>
                    <li class="cur"><a href="javascript:void(0)" rel="tab-1">第一步</a></li>
                    <li><a href="javascript:void(0)" rel="tab-2">第二步</a></li>
                    <li><a href="javascript:void(0)" rel="tab-3">第三步</a></li>
                    <li><a href="javascript:void(0)" rel="tab-4">第四步</a></li>
                </ul>
                <div>
                    <div class="tab_h2 tab-1">
                        <div class="tab_txt"><span class="tab_txt_b">第一步：</span>登陆账号，点击“我要存款”或者点击“会员中心”再选择“会员存款”</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/lstd_1.png")%>" width="528" height="84" />
                        </div>
                    </div>
                    <div class="tab_h2 tab-2" style="display: none">
                        <div class="tab_txt"><span class="tab_txt_b">第二步：</span>点击选择“在线支付”</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/zxzf_2.png")%>" width="640" height="358" />
                        </div>
                    </div>

                    <div class="tab_h2 tab-3" style="display: none">
                        <div class="tab_txt"><span class="tab_txt_b">第三步：</span>填写要存款的金额并提交</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/zxzf_3.png")%>" width="633" height="347" />
                        </div>
                    </div>
                    <div class="tab_h2 tab-4" style="display: none">
                        <div class="tab_txt"><span class="tab_txt_b">第四步：</span>提交后选择要支付的银行，并按照第三方支付的要求步骤操作</div>
                        <div class="tab_pic">
                            <img src="<%=GetImage("~/image/zxzf_4.png")%>" width="633" height="251" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
