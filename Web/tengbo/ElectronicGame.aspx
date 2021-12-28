<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.master" AutoEventWireup="true" Inherits="root_aspx" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        this.RootMasterPage.NavIndex = 2;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $(".game_menu li").click(function () {
                var num = $(".game_menu li").index(this);
                $(this).addClass("mtab").siblings().removeClass("mtab");
                $(".content-area").eq(num).fadeIn().siblings().hide();
            })
            /*pt*/
            $(".leftMenu ul li").click(function () {

                $(".leftMenu ul li").removeClass("currentTab");
                var indexValue = $(".leftMenu ul li").index(this);
                var constantWidth = 800;
                var constResult = indexValue * constantWidth;

                $("ul.sbLists").animate({
                    "margin-left": "-" + constResult
                });

                //currentTab
                $(".leftMenu ul li:nth-child(" + (indexValue + 1) + ")").addClass("currentTab");
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body2" runat="server">
    <div class="insidePt">
        <div class="left_Pt">
            <div class="leftMenu">
                <h1>PT国际版</h1>
                <ul>
                    <li class="currentTab"><a>最热门游戏</a></li>
                    <li class=""><a>经典老虎机</a></li>
                    <li class=""><a>多线老虎机</a></li>
                    <li class=""><a>刮刮卡游戏</a></li>
                    <li class=""><a>大型电玩</a></li>
                    <li class=""><a>真人现场</a></li>
                    <li class=""><a>累积游戏</a></li>
                    <li class=""><a>纸牌和桌面游戏</a></li>
                </ul>
            </div>
            <div class="pt_load">
                <div class="pt_bt">PT国际版下载</div>
            </div>
        </div>

        <div class="right_Pt">
            <div class="subContainer">
                <ul class="sbLists" style="margin-left: 0px;">
                    <li>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                    </li>

                    <li>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                    </li>

                    <li>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                    </li>

                    <li>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                    </li>
                    <li>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                    </li>
                    <li>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                    </li>
                    <li>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                    </li>
                    <li>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>钢铁侠2 50线</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>金刚</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>友爱医生</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                        <div class="slotsImg">
                            <img src="image/pt_img1.jpg" width="173" height="258">
                            <p>X战警</p>
                            <img src="image/threeLines.png" width="173" height="8">
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
