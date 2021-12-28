<%@ Page Language="C#" AutoEventWireup="true" Inherits="SitePage" %>

<!DOCTYPE html>
<script runat="server">

    BU.GameID? gameid;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.gameid = Request.Form["gameid"].ToEnum<BU.GameID>();
        if (this.gameid == BU.GameID.HG)
        {
            if (this.Member == null)
            {
            }
            else
            {
                web.MemberGame_HG.Row row = web.MemberGame_HG.Instance.Login(null, this.Member.ID, this.Member.CorpID);
                if (string.IsNullOrEmpty(row.ticket))
                    Response.Redirect("~/");
                this.gameframe.Attributes["src"] = string.Format("{0}?ticketId={1}&lang={2}", row.get_api(null).hg_game, row.ticket, "ch");
                return;
            }
        }
        else if (this.gameid == BU.GameID.BBIN)
        {
            //Response.Redirect("~/Game/bbin.ashx");
            this.gameframe.Attributes["src"] = ResolveClientUrl("~/Game/bbin.ashx");
            return;
        }
        else if (this.gameid == BU.GameID.AG)
        {
        }
        Response.Redirect("~/");
    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript">
        $(document).ready(function () { $(window).resize(function () { $('#gameframe').height($(window).innerHeight() - $('#msg').outerHeight()); }).trigger('resize'); });
    </script>
    <style type="text/css">
        html, body { margin: 0; padding: 0; width: 100%; height: 100%; background-color: black; color: white; overflow: hidden; } 
        .gameframe { position: absolute; bottom: 0; left: 0; width: 100%; } 
    </style>
</head>
<body>
    <iframe id="gameframe" class="gameframe" frameborder="0" runat="server"></iframe>
</body>
</html>
