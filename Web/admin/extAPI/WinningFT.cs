using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using extAPI.hg;

namespace extAPI.wft
{
    public static class wftapi
    {
        #region Configuration
        [SqlSetting("wft","wft_url"), DefaultValue("http://hapi.bm.1sgames.com/api.aspx")]
        private static string wft_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [SqlSetting("wft", "wftagent"), DefaultValue("p@wc")]
        private static string wftagent
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [SqlSetting("wft", "wftsecret"), DefaultValue("wc15")]
        private static string wftsecret
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [SqlSetting("wft", "wftegamehost"), DefaultValue("member.eg.1sgames.com")]
        private static string wftegamehost
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("wft", "wftsportsagent"), DefaultValue("p@wd")]
        private static string wftsportsagent
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [SqlSetting("wft", "wftsportssecret"), DefaultValue("wd16")]
        private static string wftsportssecret
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [SqlSetting("wft", "wftsportshost"), DefaultValue("sport.eg.1sgames.com")]
        private static string wftsportshost
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        #endregion

        public static Dictionary<string, string> Regist(string username,string currency,WFTGameType gametype)
        {
            using (wftRequest request=new wftRequest("create",gametype))
            {
                request["username"] = username;
                request["currency"] = currency;
                return PubRequest(request.GetString());
            }
        }

        public static Dictionary<string, string> CheckBalance(string username, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("balance",gametype))
            {
                request["username"] = username;
                return PubRequest(request.GetString());
            }
        }

        public static Dictionary<string, string> Deposit(WinningInfo winning, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("deposit",gametype))
            {
                request["username"] = winning.UserName;
                request["serial"] = winning.Serial;
                request["amount"] = winning.Amount;
                return PubRequest(request.GetString());
            }
        }

        public static Dictionary<string, string> Withdraw(WinningInfo winning, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("withdraw",gametype))
            {
                request["username"] = winning.UserName;
                request["serial"] = winning.Serial;
                request["amount"] = winning.Amount;
                return PubRequest(request.GetString());
            }
        }

        public static Dictionary<string, string> CheckPayment(WinningInfo winning, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("check_payment",gametype))
            {
                request["username"] = winning.UserName;
                request["serial"] = winning.Serial;
                return PubRequest(request.GetString());
            }
        }

        public static Dictionary<string, string> Login(string username,string language,WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("login",gametype))
            {
                request["username"] = username;
                request["host"] = gametype == WFTGameType.Egame?wftegamehost:wftsportshost;
                request["lang"] = language;
                return PubRequest(request.GetString());
            }
        }

        public static Dictionary<string, string> CheckOnlineUsers(string username, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("check_online",gametype))
            {
                request["username"] = username;
                return PubRequest(request.GetString());
            }
        }

        public static Dictionary<string, string> Logout(string username, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("logout",gametype))
            {
                request["username"] = username;
                return PubRequest(request.GetString());
            }
        }
        /// <summary>
        /// SearchTicket
        /// </summary>
        /// <param name="startdatetime">开始查询时间yyyy-MM-dd HH:mm:ss</param>
        /// <param name="duration">间隔时间的秒数</param>
        /// <param name="mathchover">1：结束0:未结束</param>
        /// <returns></returns>
        public static List<WFTTicketInfo> SearchTicket(string startdatetime, string duration, string mathchover,WFTGameType gametype)
        {
            List<WFTTicketInfo> info = new List<WFTTicketInfo>();
            using (wftRequest request = new wftRequest("ticket",gametype))
            {
                string responsestr = "";
                request["start", "yyyy-MM-dd HH:mm:ss"] = startdatetime;
                request["duration"] = duration;
                request["match_over"] = mathchover;
                responsestr = CommonMethod.MyData(request.GetString()); 
                #region parse response
                using (StringReader sr = new StringReader(responsestr))
                {
                    using (XmlTextReader xr = new XmlTextReader(sr))
                    {
                        foreach (var n0 in xr.ReadElement(0, "response"))
                        {
                            foreach (var n1 in xr.ReadElement(1, "result"))
                            {
                               
                                foreach (var n2 in xr.ReadElement(2, "ticket"))
                                {
                                    WFTTicketInfo detail = new WFTTicketInfo();
                                    while (xr.Read(2))
                                    {
                                        if (xr.IsStartElement(3, "fid"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.FetchId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "id"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.TicketId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "t"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.LastModifiedDate = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "u"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Username = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "b"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.BetAmount = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "w"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.WinAmount = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "a"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.CommissionAmount = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "c"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Commission = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "ip"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.IPaddress = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "league"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.LeagueId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "home"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.HomeId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "away"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.AwayId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "status"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.DangerStatus = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "game"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Game = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "odds"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Odds = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "side"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Side = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "info"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Info = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "half"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Half = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "trandate"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.TransactionDate = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "workdate"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.WorkingDate = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "matchdate"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.MatchDate = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "runscore"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.RunningScore = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "score"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Score = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "htscore"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.HalfTimeScore = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "flg"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.FLGRes = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "res"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Result = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "edesc"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.GameDescript = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "eres"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.GameResult = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "exrate"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.ExchangeRate = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "jp"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.JP = xr.Value;
                                            break;
                                        }
                                    }
                                    info.Add(detail);
                                }
                                
                            }
                            
                        }
                        
                    }
                }
                #endregion

            }
            return info;
        }

        public static List<Parlay> SearchParlay(string ticketid, WFTGameType gametype)
        {
            List<Parlay> info = new List<Parlay>();
            using (wftRequest request = new wftRequest("parlay",gametype))
            {
                string responsestr = "";
                request["ticket_id"] = ticketid;
                responsestr = CommonMethod.MyData(request.GetString());
                #region parse response
                using (StringReader sr = new StringReader(responsestr.Trim()))
                {
                    using (XmlTextReader xr = new XmlTextReader(sr))
                    {
                        foreach (var n0 in xr.ReadElement(0, "response"))
                        {
                            foreach (var n1 in xr.ReadElement(1, "result"))
                            {

                                foreach (var n2 in xr.ReadElement(2, "ticket"))
                                {
                                    Parlay detail = new Parlay();
                                    while (xr.Read(2))
                                    {
                                        if (xr.IsStartElement(3, "parent"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.ParentTicketId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "id"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.ParlayId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "u"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Username = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "league"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.LeagueId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "home"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.HomeId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "away"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.AwayId = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "status"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.DangerStatus = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "game"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Game = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "odds"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Odds = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "side"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Side = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "info"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Info = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "half"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Half = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "matchdate"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.MatchDate = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "runscore"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.RunningScore = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "score"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Score = xr.Value;
                                        }
                                        else if (xr.IsStartElement(3, "res"))
                                        {
                                            while (xr.Read(4))
                                                if (xr.IsTextNode(4))
                                                    detail.Result = xr.Value;
                                            break;
                                        }

                                    }
                                    info.Add(detail);
                                }

                            }

                        }

                    }
                }
                #endregion
            }
            return info;
        }
        public static Dictionary<string, string> SearchTeam(string teamid, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("team",gametype))
            {
                request["team_id"] = teamid;
                return PubRequest(request.GetString());
            }
        }
        public static Dictionary<string, string> SearchLeague(string username, string leaugeid, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("league",gametype))
            {
                request["username"] = username;
                request["league_id"] = leaugeid;
                return PubRequest(request.GetString());
            }
        }
        public static Dictionary<string, string> SearchFetch(string username, WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("fetch",gametype))
            {
                request["username"] = username;
                return PubRequest(request.GetString());
            }
        }

        public static Dictionary<string, string> MarkFetched(WFTGameType gametype,params string[] fetchids)
        {
            using (wftRequest request = new wftRequest("mark_fetched",gametype))
            {
                request["fetch_ids"] = fetchids;
                return PubRequest(request.GetString());
            }
        }
        public static Dictionary<string, string> GetJackpots(WFTGameType gametype)
        {
            using (wftRequest request = new wftRequest("get_jackpots",gametype))
            {
                return PubRequest(request.GetString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="gametype">0:Egame,1:Sports</param>
        /// <returns></returns>
        private static Dictionary<string, string> PubRequest(string postData)
        {
            string response = CommonMethod.MyData(postData);
            return parseWFTResult(response);
        }

        class wftRequest : apiRequest
        {
            public string Action { get; private set; }

            public wftRequest(string action,WFTGameType gametype)
            {
                if (gametype == WFTGameType.Egame)
                {
                    sb.Append(wft_url + "?secret=" + wftsecret);
                    sb.Append("&agent=" + wftagent);
                }
                else
                {
                    sb.Append(wft_url + "?secret=" + wftsportssecret);
                    sb.Append("&agent=" + wftsportsagent);
                }
                sb.Append("&action="+action);
            }

            public object this[string name]
            {
                set
                {
                    if (value == null || value.ToString() == "") return;
                    sb.Append("&" + name + "=" + util.EnumToValue(value));
                }
            }
            public object this[string name,string format]
            {
                set
                {
                    if (value == null) return;
                    sb.Append("&" + name + "=" + DateTime.Parse(value.ToString()).ToString(format));
                }
            }
        }
        /// <summary>
        /// 解析WFT返回信息
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static Dictionary<string, string> parseWFTResult(string xml)
        {
            Dictionary<string, string> retDic = new Dictionary<string, string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                if (!string.IsNullOrEmpty(xml))
                {
                    xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
                    xml = xml.Replace('&', '_');
                    doc.LoadXml(xml);

                    _Playerdetails info = new _Playerdetails();
                    XmlNode root = doc.SelectNodes("response")[0];
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "errcode":
                                retDic.Add("errcode", XMLPropertiesHepler.getNodeValues(root.ChildNodes, "errcode"));
                                break;
                            case "errtext":
                                retDic.Add("errtext", XMLPropertiesHepler.getNodeValues(root.ChildNodes, "errtext"));
                                break;
                            case "result":
                                retDic.Add("result", XMLPropertiesHepler.getNodeValues(root.ChildNodes, "result"));
                                break;
                        }
                    }
                    retDic["result"] = retDic["result"].Replace('_', '&');
                }
            }
            catch (Exception e)
            {
                //logger.error("解析XML异常!", e);
                log.message("解析返回信息XML异常!", e.Message + "请求信息：" + xml);
            }
            return retDic;
        }

       

    }

    /// <summary>
    /// 玩家明细
    /// </summary>
    public class _Playerdetails
    {
        /// <summary>
        /// Name of the player bet for agame
        /// </summary>
        public string PlayerName;

        /// <summary>
        /// Total bet amount during theperiod for a player
        /// </summary>
        public decimal? Bet_Amount;

        /// <summary>
        /// Total payoff during the period for a player
        /// </summary>
        public decimal? Bet_Payoff;

        /// <summary>
        /// Total win or loss during the period for a player Value positive means loss to
        ///player. Value negative means
        ///win to player
        /// </summary>
        public decimal? TotalWin;

        /// <summary>
        /// Total money bet using bonus
        /// amount i.e. bet placed using
        /// bonus money during the
        /// period for a player
        /// </summary>
        public decimal? DeductAmount;

        /// <summary>
        /// Total even bet spot amount
        ///during the period for a player
        /// </summary>
        public decimal? EvenAmount;

        /// <summary>
        /// Net bet amount after
        ///deducting the even bet
        ///amount from total bet
        ///amount during the period for
        ///a player
        /// </summary>
        public decimal? TotalAmount;

        /// <summary>
        /// Agent Name
        /// </summary>
        public string AgentName;

        /// <summary>
        /// Name of the Master the agent belongs too Ex :Jang
        /// </summary>
        public string MasterName;
    }

    public class WinningInfo
    {
        public string Secret { get; set; }
        public string Agent { get; set; }
        public string UserName { get; set; }
        public string Serial { get; set; }
        public float Amount { get; set; }
        public string Lang { get; set; }
        public string Host { get; set; }
    }

    public class WFTTicketInfo
    {
        public string FetchId { get; set; }
        public string TicketId { get; set; }
        public string LastModifiedDate { get; set; }
        public string Username { get; set; }
        public string BetAmount { get; set; }
        public string WinAmount { get; set; }
        public string CommissionAmount { get; set; }
        /// <summary>
        /// value = 0.4 means 0.4%
        /// </summary>
        public string Commission { get; set; }
        public string IPaddress { get; set; }
        public string LeagueId { get; set; }
        public string HomeId { get; set; }
        public string AwayId { get; set; }
        /// <summary>
        /// N:Auto-Accept, A:Accepted, R:Reject, C:Cancel 
        /// RG:RejectGoal, RP:RejectPenalty, RR:RejectRedCard
        /// </summary>
        public string DangerStatus { get; set; }
        public string Game { get; set; }
        public string Odds { get; set; }
        public string Side { get; set; }
        public string Info { get; set; }
        /// <summary>
        /// 0:Full Time, 1:First Half
        /// </summary>
        public string Half { get; set; }
        public string TransactionDate { get; set; }
        public string WorkingDate { get; set; }
        public string MatchDate { get; set; }
        public string RunningScore { get; set; }
        public string Score { get; set; }
        public string HalfTimeScore { get; set; }
        /// <summary>
        /// First/last goal result (empty if not match over & not FLG)
        /// </summary>
        public string FLGRes { get; set; }
        /// <summary>
        /// P:NotMatchOver WA:WinAll LA:LostAll WH:WinHalf LH:LostHalf D:Draw
        /// </summary>
        public string Result { get; set; }
        public string GameDescript { get; set; }
        public string GameResult { get; set; }
        public string ExchangeRate { get; set; }
        public string JP { get; set; }
    }

    public class Parlay
    {
        public string ParentTicketId { get; set; }
        public string ParlayId { get; set; }
        public string Username { get; set; }
        public string LeagueId { get; set; }
        public string HomeId { get; set; }
        public string AwayId { get; set; }
        /// <summary>
        /// N:Auto-Accept, A:Accepted, R:Reject, C:Cancel 
        /// RG:RejectGoal, RP:RejectPenalty, RR:RejectRedCard
        /// </summary>
        public string DangerStatus { get; set; }
        public string Game { get; set; }
        public string Odds { get; set; }
        public string Side { get; set; }
        public string Info { get; set; }
        /// <summary>
        /// 0:Full Time, 1:First Half
        /// </summary>
        public string Half { get; set; }
        public string MatchDate { get; set; }
        public string RunningScore { get; set; }
        public string Score { get; set; }
        /// <summary>
        /// / P:NotMatchOver WA:WinAll LA:LostAll WH:WinHalf LH:LostHalf D:Draw
        /// </summary>
        public string Result { get; set; }
    }

    public enum WFTGameType : int
    { 
        /// <summary>
        /// 电子游戏是0
        /// </summary>
        Egame=0,
        /// <summary>
        /// 体育是1
        /// </summary>
        Sports=1
    }
}
