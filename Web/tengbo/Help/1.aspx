<%@ Page Title="" Language="C#" MasterPageFile="Help.master" AutoEventWireup="true" Inherits="HelpPage" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        this.HelpIndex = 1;
    }
</script>

<asp:Content ContentPlaceHolderID="help2" runat="server">
    <div class="help-title">
        <div class="title0">关于我们</div>
    </div>
    <p>腾博会成立于2009年，持有菲律宾政府颁发的网络博彩运营执照，同时也是网络博彩委员会制定有合法博彩牌照的网络博彩网站，是正规合法对外营业的专业赌场。公司与亚游（Asia Gaming）,波音（bbin）, 何博士（HoGaming）进行技术合作，共同打造高品质的游戏平台，具有绝对的合法性和公平性。除此之外，公司旗下还有腾发会，主要运营太阳城、AG亚游、皇冠体育、云顶彩票等平台，力求以最多样的游戏，为广大客户提供丰富多彩的博彩活动，并提供丰厚的优惠回馈。</p>
    <%--<div style="overflow: hidden; margin-bottom: 8px;">
            <div class="about_game_one">
                <div class="game_one_tit">腾博会</div>
                <div class="game_one_img"></div>
                <div class="game_one_text"><a>立即体验 &gt;&gt;</a></div>
                <div class="B"></div>
            </div>
            <div class="about_game_two">
                <div class="game_one_tit">腾发会</div>
                <div class="game_two_img"></div>
                <div class="game_one_text"><a>点击注册腾发会专用账号体验游戏 &gt;&gt;</a></div>
                <div class="B"></div>
            </div>
            <div class="B"></div>
        </div>--%>
    <h3>诚信经营</h3>
    <p>公司严格遵守菲律宾政府规定博彩守则，以“诚”为本。100%信誉保证游戏公平、公正。您的每笔存取款业务在经过审核后都将安全、快捷的完成。</p>
    <h3>公平公正</h3>
    <p>真人娱乐中的荷官均接受严格的国际赌场专业训练与认证，所有赌局都依据荷官动作做出反应，而不是无趣的电脑几率预设结果。我们运用高科技的网络直播技术，带给您亲历赌场的刺激体验。电子游艺使用最公平的随机数产生机率，让您安心享受多元，炫丽的娱乐性游戏。同时我们在硬体上投入了大量的人力及资源，由顶级的盘房进行专业操盘，提供完整赛事，搭配丰富的玩法组合提供给热爱体育的玩家。</p>
    <h3>安全与隐私</h3>
    <p>为了客户的网络安全，公司成立网络安全维护中心，并获得GEOTRUST国际认证，确保网站公平公正。而在游戏投注中，我们采用128位SSL加密传输系统，MD5加密保护为游戏平台和客户信息提供进一步保护。我们承诺，将继续不遗余力地严格实行保密和隐私制度，确保个人资料的安全，并保证不将您的资料泄露给任意一方。</p>
    <h3>友善和专业的客户服务团队</h3>
    <p>我们亲切友善的客服专员训练有素，在帮助解决客户疑难的同时也能给与您最优越和最贴身的服务。我们也有提供线上聊天的功能，让客户们能即时联络我们。</p>
    <h3>现场赌厅</h3>
    <p>腾博会在菲律宾马尼拉最豪华的两家赌场云顶赌场（Resort World）和Solaire赌场中均设有VIP赌厅，我们可以为您提供最优质的现场VIP赌厅服务，欢迎新老客户莅临现场参观! </p>
</asp:Content>
