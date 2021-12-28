<%@ Page Title="" Language="C#" MasterPageFile="~/default.master" AutoEventWireup="true" Inherits="SitePage" %>
<asp:Content ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    $(document).ready(function () {
        $('#slider').sliderShow();

        $('.games_type1').hoverSilder('.hoverSilder', 44, 280, 250);
        $('.games_type2').hoverSilder('.hoverSilder', 44, 280, 250);
        $('.games_type3').hoverSilder('.hoverSilder', 44, 280, 250);
        $('.games_type4_01').hoverSilder('.bg2', 0, 83, 250);
        $('.games_type4_02').hoverSilder('.bg2', 0, 83, 250);
        $('.games_type4_03').hoverSilder('.bg2', 0, 83, 250);

        //$('#trial_link').nyroModal();


        //var popup = new $.Popup();
        $('#trial_link').click(function () {
            //$('#float').bPopup({
            //    content: 'ajax', //'ajax', 'iframe' or 'image'
            //    contentContainer: '#float',
            //    loadUrl: 'float.aspx' //Uses jQuery.load()
            //});
            //$('#trial_link').nmInit();
            TINY.box.show({ iframe: 'float.aspx', boxid: 'frameless', width: 443, height: 272, fixed: false });
            //TINY.box.show({ url: 'float.html', boxid: 'frameless', fixed: false, maskid: 'bluemask', maskopacity: 40 });
        //    event.preventDefault();
        //    popup.open('float.html');
        //    window.setTimeout(function () {
        //        popup.close();
        //    }, 10000);
        });
    });

</script>
<style type="text/css">

#slider { height: 344px; width: 1100px; overflow: hidden; position: relative; }
.slider_img { position: absolute; left: 0; top: 0; }
.slider_img > img { width: 1100px; height: 344px; }
.slider_nav { position: absolute; right: 4px; bottom: 4px; width: 120px; z-index: 2; }
.slider_nav > li { width:18px; height:19px; background: url(image/bullet.png) no-repeat left top; float: left; text-indent: -4000px;  position:relative; margin-left:7px; color:transparent; cursor: pointer; }
.slider_nav > li:hover { background-position: 0 50%; }
.slider_nav > li.active { background-position: 0 100%; }

#banner{ width:1100px; height:344px; margin:0 auto;}
.mid_buttom{ width:1100px; height:48px; margin:0 auto;}
.mid_links{ margin:0px; padding:0px; width:1100px; overflow:hidden;}
.mid_links li{ float:left; display:inline;}

.mid_link1{  width:275px; height:48px; background: url(image/mid_bts.jpg) left top no-repeat; display:block; cursor: pointer;}
.mid_link2{  width:275px; height:48px; background: url(image/mid_bts.jpg) -275px top no-repeat; display:block; cursor: pointer;}
.mid_link3{  width:275px; height:48px; background: url(image/mid_bts.jpg) -550px top no-repeat; display:block;}
.mid_link4{  width:275px; height:48px; background: url(image/mid_bts.jpg) -825px top no-repeat; display:block;}

.mid_link1:hover{  width:275px; height:48px; background: url(image/mid_bts_bover.jpg) left top no-repeat; display:block;}
.mid_link2:hover{  width:275px; height:48px; background: url(image/mid_bts_bover.jpg) -275px top no-repeat; display:block;}
.mid_link3:hover{  width:275px; height:48px; background: url(image/mid_bts_bover.jpg) -550px top no-repeat; display:block;}
.mid_link4:hover{  width:275px; height:48px; background: url(image/mid_bts_bover.jpg) -825px top no-repeat; display:block;}


/*.bg{ background-color: #000; background-repeat:inherit; position:absolute; width:260px; height:215px; left:6px; top:265px; z-index:1; background:url(image/game_bg.png) no-repeat; }*/

.bg2{ background-color: #000; background-repeat:inherit; position:absolute; width:243px; height:83px; left:0px; top:83px; z-index:1; background: url(image/game_bg2.png) no-repeat;}
#mydiv2{ padding:5px; line-height:30px; font-family:"微软雅黑"; font-size:18px; text-align:center;}
#mydiv2 a{ margin-top:8px;width:106px; height:27px; display:block; background:#000;text-align:center; line-height:27px; color:#4d1e04; font-size:15px; margin-left:65px; background:url(image/bt.gif) no-repeat; letter-spacing:2px;}
#mydiv3{ padding:5px; line-height:30px; font-family:"微软雅黑"; font-size:18px; text-align:center;}
#mydiv3 a{ margin-top:25px;width:80px; height:27px; display:block; background:#000;text-align:center; line-height:27px; color:#4d1e04; font-size:15px; margin-left:25px; background:url(image/bt2.png) no-repeat; letter-spacing:2px; float:left;}


/*.games_type_a > div.hoverSilder { position: absolute; width: 260px; height: 215px; left: 6px; top: 285px; z-index: 1; background: url(image/game_bg.png) no-repeat; text-align: center; }*/
/*.games_type_a > div.hoverSilder .descript { width:230px; margin: 30px auto; display: block; font-family: "微软雅黑"; font-size: 18px; line-height: 30px; }*/
/*.games_type_a > div.hoverSilder .button_a { position: absolute; left: 70px; bottom: 40px; cursor: pointer; }*/

.play-1 { width: auto; height: 269px; padding:18px 5px 0px 5px; margin:0px; background-color: green; }
.play-2 { width: 272px; background-color: red; padding-left: 5px; padding-right: 5px; }
.play-3 { width: 256px; background-color: yellow; }


.games_type { width:1090px; height:269px; padding:12px 5px 0px 5px; margin:0px; overflow:hidden;}
.games_type_a { position: relative; width:272px; height:269px; left:0px; right:272px; display: block; float:left; overflow: hidden; }
.games_type_a > .hoverSilder { position: absolute; width: 260px; height: 216px; left: 6px; top: 285px; z-index: 1; background: url(image/game_bg.png) no-repeat; }
.games_type_a > .hoverSilder .descript { text-align: center; vertical-align: central; height: 130px; font-family: "微软雅黑"; font-size: 18px; line-height: 30px; padding: 0px 15px 0px 15px; }
.games_type_a > .hoverSilder .playgame { text-align: center; vertical-align: top; height: 60px; }
.games_type_a > .hoverSilder .button_a { cursor: pointer; }

/*#mydiv{ padding:10px; padding-top:30px; line-height:30px; font-family:"微软雅黑"; font-size:18px; text-align:center; }*/
/*#mydiv a { position: absolute; bottom: 40px; width: 106px; height: 27px; left: 70px; display: block; text-align: center; line-height: 27px; color: #4d1e04; font-size: 15px; float: left; margin: 0 auto; background: url(image/bt.png) no-repeat #000; letter-spacing: 2px; cursor: pointer; }*/

.games_type1 { margin:0px 0px 0px 0px; background:url(image/img2.jpg) no-repeat; }
.games_type2 { margin:0px 0px 0px 5px; background:url(image/img1.jpg) no-repeat; }
.games_type3 { margin:0px 0px 0px 5px; background:url(image/img3.jpg) no-repeat; }

.games_type4_bg{ width:256px; height:269px; margin:0px 0px 0px 5px; background: url(image/k.gif); display: block; float:left;}
.games_type4{ width:242px; height:253px; margin:7px 6px 9px 7px; overflow:hidden; background:#c7bbbc;}
.games_type4_01,
.games_type4_02,
.games_type4_03{ width:243px; height:83px; display:block; float:left; position: relative; overflow:hidden; }
.games_type4_01 > div.bg2,
.games_type4_02 > div.bg2,
.games_type4_03 > div.bg2 { position: absolute; left: 0px; top: 83px; }
.games_type4_01{ background:url(image/img4.jpg) no-repeat; margin:0px; }
.games_type4_02{ background:url(image/img5.jpg) no-repeat; margin:2px 0 0 0; }
.games_type4_03{ background:url(image/img6.jpg) no-repeat; margin:2px 0 0 0; }
</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body1" runat="server">
    <div id="float" style="display:none;"></div>
    <div id="banner">
      <div id="slider">
        <ul>
            <li class="slider_img"><img src="image/banner1.jpg" /></li>
            <li class="slider_img"><img src="image/banner2.jpg" /></li>
            <li class="slider_img"><img src="image/banner3.jpg" /></li>
            <li class="slider_img"><img src="image/banner4.jpg" /></li>
            <li class="slider_img"><img src="image/banner5.jpg" /></li>
            <li class="slider_img"><img src="image/banner6.jpg" /></li>
        </ul>
   </div>
  </div>
  <div class="mid_buttom">
    <div class="mid_links">
      <ul>
        <li><a onclick="live800_chat()" class="mid_link1"></a> </li>
        <%--<li><a href="float.aspx" target="_blank" id="trial_link" class="mid_link2"></a> </li>--%>
        <li><a id="trial_link" class="mid_link2"></a> </li>
        <li><a href="VIP.aspx" class="mid_link3"></a> </li>
        <li><a href="Downloads.aspx" class="mid_link4"></a> </li>
      </ul>
    </div>
  </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="main_games">
    <div class="main_index">
      <div class="main_lights">
        <div class="lights_left"></div>
        <div class="lights_right"></div>
      </div>
        <table class="play-1" border="0" cellpadding="0" cellspacing="0" style="display: none;">
            <tr>
                <td class="play-2"></td>
                <td class="play-2"></td>
                <td class="play-2"></td>
                <td class="play-3"></td>
            </tr>
        </table>
        <div class="games_type">
            <div class="games_type1 games_type_a">
                <table class="hoverSilder" border="0">
                    <tr><td class="descript">亚洲最受欢迎的娱乐场，拥有最大最先进的真人娱乐棚</td></tr>
                    <tr><td class="playgame"><a class="button_a" onclick="playgame('<%=BU.GameID.BBIN%>')">开始游戏</a></td></tr>
                </table>
                <%--<div class="hoverSilder"><div class="descript">行业独家创新，6张牌先发，360度视角实时输赢排行榜 好路桌台 关注功能</div><a class="button_a" onclick="playgame('<%=BU.GameID.AG%>')">开始游戏</a></div>--%>
            </div>
            <div class="games_type2 games_type_a">
                <table class="hoverSilder" border="0" cellpadding="0" cellspacing="0">
                    <tr><td class="descript">行业独家创新，6张牌先发，360度视角实时输赢排行榜 好路桌台 关注功能</td></tr>
                    <tr><td class="playgame"><a class="button_a" onclick="playgame('<%=BU.GameID.AG%>')">开始游戏</a></td></tr>
                </table>
                <%--<div class="hoverSilder"><div class="descript">亚洲最受欢迎的娱乐场，拥有最大最先进的真人娱乐棚</div><div class="button_a" onclick="playgame('<%=BU.GameID.BBIN%>')">开始游戏</div></div>--%>
            </div>
            <div class="games_type3 games_type_a">
                <table class="hoverSilder" border="0" cellpadding="0" cellspacing="0">
                    <tr><td class="descript">澳门何氏集团所有，网络真人娱乐场先锋，以美女荷官和高清画面闻名亚洲</td></tr>
                    <tr><td class="playgame"><a class="button_a" onclick="playgame('<%=BU.GameID.HG%>')">开始游戏</a></td></tr>
                </table>
                <%--<div class="hoverSilder"><div class="descript">澳门何氏集团所有，网络真人娱乐场先锋，以美女荷官和高清画面闻名亚洲</div><div class="button_a" onclick="playgame('<%=BU.GameID.HG%>')">开始游戏</div></div>--%>
            </div>
            <div class="games_type4_bg">
                <div class="games_type4">
                    <div class="games_type4_01 type">
                        <div class="bg2">
                            <div id="mydiv2">
                                <p>老虎机，电玩，电动扑克等</p>
                                <a>开始游戏</a>
                            </div>
                        </div>
                    </div>
                    <div class="games_type4_02 type">
                        <div class="bg2">
                            <div id="mydiv3">
                                <a>BB彩票</a>
                                <a>彩 票</a>
                            </div>
                        </div>
                    </div>
                    <div class="games_type4_03 type">
                        <div class="bg2">
                            <div id="mydiv2">
                                <p>体育赛事</p>
                                <a>开始游戏</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
  </div>
<%--<div id="trial_float" style="display: none">
<div class="float" style="line-height:30px; margin: 0 auto;">
  <table border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width="28" rowspan="2" valign="top"><img src="image/f_1.png" width="23" height="35" /></td>
          <td class="float_text">请选择游戏厅</td>
        </tr>
        <tr>
          <td height="55">
          <div class="float_game">
          <ul>
          <li>AG国际厅</li>
          <li>波音厅</li>
          <li>HG厅</li>
          </ul>
          
          </div></td>
        </tr>
      </table></td>
    </tr>
    <tr>
      <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width="28" rowspan="2" valign="top"><img src="image/f_2.png" width="23" height="30" /></td>
          <td class="float_text">请输入验证码</td>
        </tr>
        <tr>
          <td height="55"><form id="form2" name="form1" method="post" action="">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="120"><label>
                  <input name="textfield" type="text" id="text1"  class="input_yz"/>
                </label></td>
                <td width="85"><img src="image/f_yanzhen.gif" width="80" height="30" /></td>
                <td class="white">*请输入验证码</td>
              </tr>
            </table>
                    </form>
          </td>
        </tr>
      </table></td>
    </tr>
    <tr>
      <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width="35" valign="middle"><img src="image/f_3.png" width="23" height="33" /></td>
          <td height="50"><label>
            <input name="button" type="image" id="Image1" value="提交" src="image/f_bt1.gif" />
          </label></td>
        </tr>
        
      </table></td>
    </tr>
  </table>
</div>
</div>--%>
</asp:Content>