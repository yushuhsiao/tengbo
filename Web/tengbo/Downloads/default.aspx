<%@ Page Title="" Language="C#" MasterPageFile="~/master/default.master" AutoEventWireup="true" Inherits="SitePage" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body2" runat="server">
    <div class="downloads">
        <div class="body">
            <div class="title">下载专区</div>
            <center class="items font2">
                <ul>
                    <%--<li style="background-image: url(<%=GetImage("~/Downloads/tengbo.jpg")%>);"><a href="http://www.55msc.com/packer.aspx" target="_blank"><span class="item-title">下载腾博会客户端</span></a></li>--%>
                    <li style="background-image: url('./tengbo.jpg')"><a href="./tengbo.rar" target="_blank"><span class="item-title">下载腾博会客户端</span></a></li>
                    <li style="background-image: url(<%=GetImage("~/Downloads/flash.jpg")%>);"><a href=" http://get.adobe.com/cn/flashplayer/" target="_blank"><span class="item-title">下载flash player播放器          </span></a></li>
                    <li style="background-image: url(<%=GetImage("~/Downloads/bbs.jpg")%>);"><a href="<%=GetImage("~/Downloads/BBBrowser-Launcher.exe")%>"><span class="item-title">波音专用浏览器下载（波音厅游戏首选）</span></a></li>
                    <li style="background-image: url(<%=GetImage("~/Downloads/tyc.jpg")%>);"><a href="http://www.55msc.com/packer.aspx" target="_blank"><span class="item-title">下载太阳城客户端</span></a></li>
                    <li style="background-image: url(<%=GetImage("~/Downloads/360.jpg")%>);"><a href="http://se.360.cn/" target="_blank"><span class="item-title">下载360浏览器                   </span></a></li>
                    <li style="background-image: url(<%=GetImage("~/Downloads/cc.jpg")%>);"><a href="https://www.google.com/intl/zh-CN/chrome/browser/" target="_blank"><span class="item-title">下载谷歌浏览器                   </span></a></li>
                </ul>
                <div class="clear"></div>
            </center>
        </div>
    </div>

    <%--<div class="main_games">
    <div class="main_index">
      <div class="main_lights">
        <div class="lights_left"></div>
        <div class="lights_right"></div>
      </div>
      <div class="down_content">
        <div class="down">
          <div class="down_tit">下载专区</div>
          <div class="down_con">
            <div class="td_AG">
            <a class="link_td1"></a>
            <a class="link_td2">下载AG客户端</a>
            </div>
            <div class="td_flash">
            <a class="link_td1"></a>
            <a class="link_td2">下载flash player播放器</a>
            </div>
            <div class="td_bbs">
            <a class="link_td1"></a>
            <a class="link_td2 m">波音专用浏览器下载（波音厅游戏首选）</a>
            </div>
            <div class="td_ie">
            <a class="link_td1"></a>
            <a class="link_td2">下载IE客户端</a>
            </div>
            <div class="td_ff">
            <a class="link_td1"></a>
            <a class="link_td2">下载火狐浏览器</a>
            </div>
            <div class="td_cc">
            <a class="link_td1"></a>
            <a class="link_td2">下载谷歌浏览器</a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>--%>
</asp:Content>
