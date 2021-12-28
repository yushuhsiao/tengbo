<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.master" AutoEventWireup="true" Inherits="root_aspx" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        this.RootMasterPage.NavIndex = 1;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //$('.box1, .box2, .box3').hoverSilder('', '.txt3, .txt4, .txt5', 200);
            $('.box1, .box2, .box3').hoverSilder('', '.div_AGBtn, .txt3', 200);
        });
    </script>
    <style type="text/css">
        .livecasino {
            background-color: #480102;
            padding: 10px;
            margin: 18px 16px;
        }

        .body {
            padding: 15px 22px;
            background-color: #000;
        }

        .box1, .box2, .box3 {
            float: left;
            position: relative;
        }

        .box1 {
            background: url(image/live_img1a.jpg) no-repeat;
            width: 1004px;
            height: 289px;
            margin-bottom: 18px;
        }

        .box2 {
            background: url(image/live_img2a.jpg) no-repeat;
            width: 492px;
            height: 257px;
            margin-right: 10px;
        }

        .box3 {
            background: url(image/live_img3a.jpg) no-repeat;
            width: 492px;
            height: 257px;
            margin-left: 10px;
        }

        .txt1 {
            position: absolute;
            font-size: 13px;
            line-height: 25px;
        }

        .txt2 {
            position: absolute;
            font-size: 13px;
            opacity: .5;
        }

        .txt3, .txt4, .txt5 {
            position: absolute;
            display: inline-block;
            text-align: center;
            padding-top: 10px;
            height: 37px;
        }

        .box1 .txt1 {
            left: 430px;
            top: 100px;
        }

        .box2 .txt1 {
            left: 30px;
            top: 80px;
            width: 250px;
        }

        .box3 .txt1 {
            left: 30px;
            top: 80px;
            width: 280px;
        }

        .box1 .txt2 {
            left: 18px;
            bottom: 8px;
            line-height: 26px;
            height: 26px; /*top: 255px;*/
        }

        .box2 .txt2 {
            left: 18px;
            bottom: 5px;
            line-height: 27px;
            height: 27px; /*top: 225px;*/
        }

        .box3 .txt2 {
            left: 18px;
            bottom: 5px;
            line-height: 27px;
            height: 27px; /*top: 225px;*/
        }

        .box1 .txt4 {
            left: 570px;
            top: 229px;
            width: 110px;
        }
        /*727px*/
        .box1 .txt3 {
            left: 683px;
            top: 229px;
            width: 110px;
        }

        .box1 .txt5 {
            left: 796px;
            top: 229px;
            width: 110px;
        }

        .box2 .txt3 {
            left: 033px;
            top: 166px;
            width: 206px;
        }

        .box3 .txt3 {
            left: 033px;
            top: 166px;
            width: 206px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="livecasino">
        <div class="body font2">
            <ul>
                <script type="text/javascript">
                    //$(function () {
                    //    $(".block").hover(function () {
                    //        $(this).find(".tt").animate({ top: "404px" }, 200);
                    //    }, function () {
                    //        $(this).find(".tt").animate({ top: "280px" }, 200);
                    //    });
                    //});
                </script>
                <li class="box1">
                    <%--<div class="txt1">现场360度视角，实时显示输赢排行榜，推荐最好牌路。<br />独创6张牌先发，多种创新玩法。<br />实地赌场，高清大视频，操作简洁明了。<br /></div>
                    <div class="txt2">百家乐 连环百家乐 极速百家乐 VIP包桌 龙虎 骰宝 轮盘</div>
                    <div class="txt3" style="display: none;"><a class="button2" onclick="$('#playgame11').playgame()"><span class="p1">AG旗舰厅</span><span class="p2">旗舰厅试玩</span></a></div>
                    <div class="txt4" style="display: none;"><a class="button2" onclick="$('#playgame12').playgame()"><span class="p1">AG国际厅</span><span class="p2">国际厅试玩</span></a></div>
                    <div class="txt5" style="display: none;"><a class="button2" onclick="$('#playgame13').playgame()"><span class="p1">AG实地厅</span><span class="p2">实地厅试玩</span></a></div>--%>
                    <div class="txt2">百家乐 连环百家乐 极速百家乐 VIP包桌 龙虎 骰宝 轮盘</div>
                    <div class="block">
                        <ul>
                            <li class="boder">
                                <img src="<%=GetImage("~/image/AG_1.png")%>" width="123" height="28" />
                                <p>现场360度视角，实时显示输赢排行榜，推荐最好牌路，多款游戏同台下注，百家乐，龙虎，骰宝等</p>
                                <div class="div_AGBtn" style="display: none; height: 28px; width: 175px;"><a onclick="$('#playgame12').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a></div>
                            </li>
                            <li class="boder">
                                <img src="<%=GetImage("~/image/AG_2.png")%>" width="124" height="26" />
                                <p>独创6张牌先发，全球首创，多种创新玩法，免佣百家乐，极速百家乐，连环百家乐，龙虎，轮盘等</p>
                                <div class="div_AGBtn" style="display: none; height: 28px; width: 175px;"><a onclick="$('#playgame11').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a></div>
                            </li>
                            <li>
                                <img src="<%=GetImage("~/image/AG_3.png")%>" width="125" height="26" />
                                <p>完全实体赌场，高清大视频，操作简洁明了，连环百家乐，经典百家乐，龙虎，骰宝等</p>
                                <div class="div_AGBtn" style="display: none; height: 28px; width: 175px;"><a onclick="$('#playgame13').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a></div>
                            </li>
                        </ul>
                    </div>
                </li>
                <li class="box2">
                    <div class="txt1">BBIN（波音厅）拥有亚洲最大最先进的摄影棚，系统稳定细致，界面简单明了，体验多桌同时投注，让您乐在其中。</div>
                    <div class="txt2">百家乐 龙虎 骰宝 轮盘 二八杠 温州牌九 德州扑克</div>
                    <div class="txt3" style="display: none;"><a class="button2" onclick="$('#playgame01').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a></div>
                </li>
                <li class="box3">
                    <div class="txt1">
                        HG（HoGaming）平台汇集各类经典游戏。<br />
                        美女荷官现场助阵让您彷佛置身欧洲真实赌场，体验不同的博彩感受。
                    </div>
                    <div class="txt2">高级百家乐 二十一点 专业轮盘 专业骰宝 龙虎</div>
                    <div class="txt3" style="display: none;"><a class="button2" onclick="$('#playgame05').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a></div>
                </li>
            </ul>
            <div class="clear"></div>
        </div>
    </div>
    <%--<div class="main_games">
        <div class="main_index">
            <div class="main_lights">
                <div class="lights_left"></div>
                <div class="lights_right"></div>
            </div>
            <div class="content">
                <div class="live">
                    <div class="live_box1">
                        <a></a>
                        <a></a>
                    </div>
                    <div class="live_box2">
                        <a></a>
                        <a></a>
                    </div>
                    <div class="live_box3">
                        <a></a>
                        <a></a>
                    </div>
                    <div class="B"></div>
                </div>
            </div>
        </div>
    </div>--%>
</asp:Content>
