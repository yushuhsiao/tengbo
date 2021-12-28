<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.Master" AutoEventWireup="true" Inherits="root_aspx" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.slider').sliderShow();
            $('.gamea').hoverSilder('.bg1', '.bg2', '', 200);
            $('.gameb').hoverSilder('', '.bg3', '', 200);
            $('.gamea').hoverSilder('.bg4', '.bg6', '.bg5', 200);
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body1" runat="server">
    <div class="slider">
        <ul>
            <li class="slider_img"><img src="<%=GetImage("~/image/banner1.jpg")%>" style="width: 1103px; height: 300px;" /></li>
            <li class="slider_img"><img src="<%=GetImage("~/image/banner2.jpg")%>" style="width: 1103px; height: 300px;" /></li>
            <li class="slider_img"><img src="<%=GetImage("~/image/banner3.jpg")%>" style="width: 1103px; height: 300px;" /></li>
            <li class="slider_img"><img src="<%=GetImage("~/image/banner4.jpg")%>" style="width: 1103px; height: 300px;" /></li>
            <li class="slider_img"><img src="<%=GetImage("~/image/banner5.jpg")%>" style="width: 1103px; height: 300px;" /></li>
            <li class="slider_img"><img src="<%=GetImage("~/image/banner6.jpg")%>" style="width: 1103px; height: 300px;" /></li>
        </ul>
    </div>
    <div class="mid_links">
        <ul>
            <%--$('#playgame05').playgame(true);--%>
            <li><a onclick="live800_chat()" class="mid_link1"></a></li>
            <li><a onclick="$(this).loadBox(443, 273, './float.aspx', './');" id="trial_link" class="mid_link2"></a></li>
            <li><a href="VIP.aspx" class="mid_link3"></a></li>
            <li><a href="Downloads" class="mid_link4"></a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="main_games">
        <ul>
            <li class="gamea game1">
                <div class="bg1" style="display: none;">
                    <div class="txt1">亚洲最受欢迎的娱乐场，拥有最大最先进的真人娱乐棚</div>
                </div>
                <div class="bg2" style="display: none;"><a class="button2" onclick="$('#playgame01').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a></div>
            </li>
            <li class="gamea game2">
                <div class="bg4 float1" style="display: none; margin-top: 45px;">
                    <img src="<%=GetImage("~/image/f1.png")%>" width="114" height="25" />
                    <a onclick="$('#playgame12').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a>
                </div>
                <div class="bg5 float1" style="display: none;">
                    <img src="<%=GetImage("~/image/f2.png")%>" width="115" height="24" />
                    <a onclick="$('#playgame11').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a>
                </div>
                <div class="bg6 float1 n_b" style="display: none;">
                    <img src="<%=GetImage("~/image/f3.png")%>" width="115" height="26" />
                    <a onclick="$('#playgame13').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a>
                </div>
            </li>
            <li class="gamea game3">
                <div class="bg1" style="display: none;">
                    <div class="txt1">澳门何氏集团所有，网络真人娱乐场先锋，以美女荷官和高清画面闻名亚洲</div>
                </div>
                <div class="bg2" style="display: none;"><a class="button2" onclick="$('#playgame05').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a></div>
            </li>
            <li class="game456">
                <ul>
                    <li class="gameb game4">
                        <div class="bg3" style="display: none;">
                            <div class="txt4"><a class="button2" onclick="$('#playgame02').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a><%--老虎机，电玩，电动扑克等<br />--%></div>
                        </div>
                    </li>
                    <li class="gameb game5">
                        <div class="bg3" style="display: none;">
                            <div class="txt5"><a class="button2" onclick="$('#playgame03').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a><%--<a class="button2">BB彩票</a><a class="button2">彩 票</a>--%></div>
                        </div>
                    </li>
                    <li class="gameb game6">
                        <div class="bg3" style="display: none;">
                            <div class="txt6"><a class="button2" onclick="$('#playgame04').playgame()"><span class="p1">开始游戏</span><span class="p2">试玩体验</span></a><%--体育赛事<br/>--%></div>
                        </div>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
</asp:Content>
