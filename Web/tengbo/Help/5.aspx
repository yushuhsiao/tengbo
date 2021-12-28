<%@ Page Title="" Language="C#" MasterPageFile="Help.master" AutoEventWireup="true" Inherits="HelpPage" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.HelpIndex = 5;
    }
</script>

<asp:Content ContentPlaceHolderID="help2" runat="server">
    <div class="help-title">
        <div class="title0">常见问题</div>
    </div>
    <h3>1、忘记密码怎么办？</h3>
    <p>【腾博会】共有2个密码。一个是您的注册时预留的登陆密码、另外一个是提款时需要用到的安全密码！如果是登陆密码忘记了请点击网站右上角的“忘记密码”。如果是安全密码忘记了请联系我们的客服人员进行身份核实！
    </p>
    <h3>2、进入游戏很慢怎么办？</h3>
    <p>如果您进入我们的网站或游戏过于缓慢，建议您清除下缓存！ IE和360浏览器的步骤为：一、请您先退出我们的网站。二、点击浏览器右上角“工具”选择“Internet选项”跳出菜单点击“删除”，勾选“Cookie和网站数据”并点击删除，等待删除完成后请再次尝试进入游戏！</p>
    <h3>3、游戏中的视频无法显示怎么办？</h3>
    <p>建议您可以清除您的FLASH 缓存，请您点击这个链接网址：http://www.macromedia.com/support/documentation/cn/flashplayer/help/settings_manager07.html  然后点击“删除所有WEB站点”。在弹窗中点击“确认”即可完成操作！</p>
    <h3>4、手机可以玩游戏吗？</h3>
    <p>安卓系统的手机或PAD是可以进入游戏的，但是哪怕是在WIFI的环境下目前手机处理器的运行能力还是有限，所以会出现画面卡的情况。所以建议还是用电脑来运行游戏！</p>
    <h3>5、为什么投注记录中的有效投注额和你们统计的不一致呢？</h3>
    <p>AG平台和波音平台都是采用的美东时间（美国东部时间如：纽约、华盛顿时间）所以投注记录中的时间会和实际投注时间有差别。例如实际投注时间为当天晚上20:00则记录上显示的时间为当天早上8:00。有12个小时的时差！HG平台的投注显示时间是北京时间！</p>
    <h3>6、存款和提款的到账时间？</h3>
    <p>通过在线支付是即时到账的，如果是网银汇款、支付宝或财富通之类的一般是3分钟内可以完成充值。提款只需达到一倍流水即可提款，提款一般是5分钟到账！</p>
    <h3>7、如何投注？</h3>
    <p>进入游戏后用鼠标点击筹码，选中后把筹码放在您想要投注的相应位置后点击“确认”即可完成投注！</p>
</asp:Content>
