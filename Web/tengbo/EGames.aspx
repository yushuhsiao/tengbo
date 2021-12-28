<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.master" AutoEventWireup="true" Inherits="root_aspx" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.RootMasterPage.NavIndex = 2;
        this.RootMasterPage.ShowFooter = false;
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript">
        $(document).ready(function () {
            $(".game_menu li").click(function () {
                var num = $(".game_menu li").index(this);
                $(this).addClass("mtab").siblings().removeClass("mtab");
                $(".content-area").eq(num).fadeIn().siblings().hide();
            })
        });
    </script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#game_menu li').click(function () {
                $('#game_menu li').removeClass('active');
                $(this).addClass('active');
                var index = $(this).attr('index');
                $('.items li').hide();
                $('.items li.' + index).fadeIn();
            });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="egames">
        <div class="banner"></div>
        <div class="menu">
            <div class="menu-title">游戏选单：</div>
            <ul id="game_menu">
                <li index="game1" class="s active">老虎机</li>
                <li index="game2" class="s">电动扑克</li>
                <li index="game3" class="s">大型电玩</li>
                <li index="game4" class="s">桌上游戏</li>
                <li index="game5">经典老虎机</li>
            </ul>
            <div class="clear"></div>
        </div>
        <div id="test1"></div>
        <div class="items">
            <ul>
                <li class="game1"><a>规则说明</a><img src="<%=GetImage("~/image/1.jpg")%>"    width="220" height="116" /><span>金瓶梅2</span></li>
                <li class="game1"><a>规则说明</a><img src="<%=GetImage("~/image/2.jpg")%>"    width="220" height="116" /><span>月光宝盒</span></li>
                <li class="game1"><a>规则说明</a><img src="<%=GetImage("~/image/3.jpg")%>"    width="220" height="116" /><span>爱你一万年</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/4.jpg")%>"    width="220" height="116" /><span>战火佳人</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/5.jpg")%>"    width="220" height="116" /><span>尸乐园</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/6.jpg")%>"    width="220" height="116" /><span>超级7</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/7.jpg")%>"    width="220" height="116" /><span>特务危机</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/8.jpg")%>"    width="220" height="116" /><span>玉蒲团</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/9.jpg")%>"    width="220" height="116" /><span>幸运财神</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/10.jpg")%>"   width="220" height="116" /><span>圣诞派对</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/11.jpg")%>"   width="220" height="116" /><span>中秋月光派对</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/12.jpg")%>"   width="220" height="116" /><span>功夫龙</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/13.jpg")%>"   width="220" height="116" /><span>2012伦敦奥运</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/14.jpg")%>"   width="220" height="116" /><span>法海斗白蛇</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/15.jpg")%>"   width="220" height="116" /><span>2012欧锦赛</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/16.jpg")%>"   width="220" height="116" /><span>金瓶梅</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/17.jpg")%>"   width="220" height="116" /><span>热带风情</span></li>
                <li class="game1 game4 game5"><a>规则说明</a><img src="<%=GetImage("~/image/18.jpg")%>"   width="220" height="116" /><span>封神榜</span></li>
                <li style="display:none;" class="game2"><a>规则说明</a><img src="<%=GetImage("~/image/2-01.png")%>" width="220" height="116" /><span>王牌5PK</span></li>
                <li style="display:none;" class="game2"><a>规则说明</a><img src="<%=GetImage("~/image/2-02.png")%>" width="220" height="116" /><span>7靶射击</span></li>
                <li style="display:none;" class="game2"><a>规则说明</a><img src="<%=GetImage("~/image/2-03.png")%>" width="220" height="116" /><span>7PK</span></li>
                <li style="display:none;" class="game3"><a>规则说明</a><img src="<%=GetImage("~/image/3-01.png")%>" width="220" height="116" /><span>象棋大转轮</span></li>
                <li style="display:none;" class="game3"><a>规则说明</a><img src="<%=GetImage("~/image/3-02.png")%>" width="220" height="116" /><span>3D数字大转轮</span></li>
                <li style="display:none;" class="game3"><a>规则说明</a><img src="<%=GetImage("~/image/3-03.png")%>" width="220" height="116" /><span>水果大转轮</span></li>
                <li style="display:none;" class="game3"><a>规则说明</a><img src="<%=GetImage("~/image/3-04.png")%>" width="220" height="116" /><span>数字大转轮</span></li>
                <li style="display:none;" class="game3"><a>规则说明</a><img src="<%=GetImage("~/image/3-05.png")%>" width="220" height="116" /><span>钻石列车</span></li>
                <li style="display:none;" class="game3"><a>规则说明</a><img src="<%=GetImage("~/image/3-06.png")%>" width="220" height="116" /><span>圣兽传说</span></li>
                <li style="display:none;" class="game4"><a>规则说明</a><img src="<%=GetImage("~/image/4-01.png")%>" width="220" height="116" /><span>皇家德州扑克</span></li>
                <li style="display:none;" class="game4"><a>规则说明</a><img src="<%=GetImage("~/image/4-02.png")%>" width="220" height="116" /><span>经典21点</span></li>
                <li style="display:none;" class="game4"><a>规则说明</a><img src="<%=GetImage("~/image/4-03.png")%>" width="220" height="116" /><span>西班牙21点</span></li>
                <li style="display:none;" class="game5"><a>规则说明</a><img src="<%=GetImage("~/image/5-01.png")%>" width="220" height="116" /><span>异国之夜</span></li>
                <li style="display:none;" class="game5"><a>规则说明</a><img src="<%=GetImage("~/image/5-02.png")%>" width="220" height="116" /><span>喜福牛年</span></li>
                <li style="display:none;" class="game5"><a>规则说明</a><img src="<%=GetImage("~/image/5-03.png")%>" width="220" height="116" /><span>龙卷风</span></li>
            </ul>
        </div>
        <div class="clear"></div>
        <div class="game-footer">Copyright © 腾博会娱乐公司 Reserved</div>
    </div>
  <%--<div class="main_games">
    <div class="main_index">
      <div class="main_lights">
        <div class="lights_left"></div>
        <div class="lights_right"></div>
      </div>
      <div class="video_main">
          <div class="video_con">
            <div class="video_rules">
            <a></a>
            </div>
            <div id="page_content">
              <ul class="game_menu">
                <div style=" float:left; width:60px; color:#FFF; text-align:center;">游戏选单：</div>
                <li class="mtab">老虎机<span style="margin:0 5px; color:#FFF">|</span></li>
                <li>电动扑克<span style="margin:0 5px; color:#FFF">|</span></li>
                <li>大型电玩<span style="margin:0 5px; color:#FFF">| </span></li>
                <li>桌上游戏<span style="margin:0 5px; color:#FFF">|</span></li>
                <li>经典老虎机<span style="margin:0 5px; color:#FFF">|</span></li>
                <div class="B"></div>
              </ul>
             <div>
              <div class="content-area" id="casino-1">
                <div id="game-layout"><a class="gameimg"><img src="image/1.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">金瓶梅2</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/2.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">月光宝盒</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/3.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">爱你一万年</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/4.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">战火佳人</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/5.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">尸乐园</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/6.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">超级7</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/7.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">特务危机</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/8.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">玉蒲团</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/9.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">幸运财神</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/10.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">圣诞派对</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/11.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">中秋月光派对</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/12.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">功夫龙</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/13.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">2012伦敦奥运</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/14.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">法海斗白蛇</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/15.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">2012欧锦赛</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/16.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">金瓶梅</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/17.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">热带风情</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/18.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">封神榜</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
              </div>
              
              
              
              <div class="content-area" id="casino-2" style=" display:none;">
                <div id="game-layout"><a class="gameimg"><img src="image/2-01.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">王牌5PK</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/2-02.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">7靶射击</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/2-03.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">7PK</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                
              </div>
              
              
              
              <div class="content-area" id="casino-3" style=" display:none;">
                <div id="game-layout"><a class="gameimg"><img src="image/3-01.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">象棋大转轮</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/3-02.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">3D数字大转轮</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/3-03.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">水果大转轮</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/3-04.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">数字大转轮</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/3-05.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">钻石列车</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/3-06.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">圣兽传说</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                
                
              </div>
              
              
              
              <div class="content-area" id="casino-4" style=" display:none;">
                <div id="game-layout"><a class="gameimg"><img src="image/4-01.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">皇家德州扑克</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/4-02.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">经典21点</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/4-03.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">西班牙21点</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/4.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">战火佳人</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/5.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">尸乐园</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/6.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">超级7</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/7.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">特务危机</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/8.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">玉蒲团</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/9.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">幸运财神</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/10.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">圣诞派对</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/11.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">中秋月光派对</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/12.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">功夫龙</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/13.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">2012伦敦奥运</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/14.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">法海斗白蛇</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/15.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">2012欧锦赛</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/16.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">金瓶梅</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/17.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">热带风情</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/18.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">封神榜</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
              </div>
              
              
              
              <div class="content-area" id="casino-5" style=" display:none;">
                <div id="game-layout"><a class="gameimg"><img src="image/5-01.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">异国之夜</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/5-02.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">喜福牛年</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/5-03.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">龙卷风</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/4.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">战火佳人</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/5.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">尸乐园</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/6.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">超级7</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/7.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">特务危机</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/8.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">玉蒲团</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/9.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">幸运财神</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/10.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">圣诞派对</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/11.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">中秋月光派对</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/12.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">功夫龙</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/13.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">2012伦敦奥运</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/14.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">法海斗白蛇</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/15.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">2012欧锦赛</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/16.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">金瓶梅</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/17.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">热带风情</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
                <div id="game-layout"><a class="gameimg"><img src="image/18.png" width="220" height="116" /></a>
                  <div class="game_text">
                    <div class="game_text_tit">封神榜</div>
                    <div class="game_text_rules"><a>规则说明</a></div>
                  </div>
                </div>
              </div>
              </div>
            </div>
          </div>
          
      </div>
    </div>
  </div>--%>
  <%--<div class="B"></div>--%>
    <%--<div id="page-foot">Copyright © 腾博会娱乐公司 Reserved</div>--%>
</asp:Content>
