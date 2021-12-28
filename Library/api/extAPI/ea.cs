using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace extAPI.ea
{
    public class eaApi
    {
        #region Configeration
        /// <summary>
        /// ea存款url
        /// </summary>
        [SqlSetting("ea", "eadeposit_url"), DefaultValue("https://mis.ea3-mission.com/configs/external/deposit/winclubs/server.php")]
        private static string eadeposit_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        /// <summary>
        /// ea取款url
        /// </summary>
        [SqlSetting("ea","eawithdraw_url"), DefaultValue("https://mis.ea3-mission.com/configs/external/withdrawal/winclubs/server.php")]
        private static string eawithdraw_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        /// <summary>
        /// ea用户余额url
        /// </summary>
        [SqlSetting("ea", "eabalance_url"), DefaultValue("https://mis.ea3-mission.com/configs/external/checkclient/winclubs/server.php")]
        private static string eabalance_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        /// <summary>
        /// ea商户信息url
        /// </summary>
        [SqlSetting("ea", "eaaffiliate_url"), DefaultValue("https://mis.ea3-mission.com/configs/external/checkaffiliate/winclubs/server.php")]
        private static string eaaffiliate_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        /// <summary>
        /// eavendorid
        /// </summary>
       [SqlSetting("ea", "testeavendorid"), DefaultValue("2")]
        public static string testvendorid
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        /// <summary>
        /// eavendorid
        /// </summary>
        [SqlSetting("ea", "eavendorid"), DefaultValue("3")]
        public static string eavendorid
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        /// <summary>
        /// ea代理编号
        /// </summary>
        [SqlSetting("ea", "eaacode"), DefaultValue("WCSMainPlayerks")]
        public static string eaacode
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        #endregion

        public static string LoginResponse(EAUserInfo eauser)
        {
            using (eaRequest request=new eaRequest("clogin",eauser.LoginID))
            {
                request["userid"] = eauser.UserID;
                request["username"] = eauser.UserName;
                request["acode"] = eaacode;
                request["vendorid"] = eauser.Mode== LoginMode.试玩?testvendorid: eavendorid;
                request["currencyid"] = eauser.CurrencyID;
                request["status"] = eauser.Status;
                request["errdesc"] = eauser.Errdesc;
                 
                return request.GetString();
            }
        }

        public static EAFanishInfo DepositFirst(EAFanishInfo deposit)
        {
            using (eaRequest request=new eaRequest("cdeposit",deposit.FuncID))
            {
                request["userid"] = deposit.UserID;
                request["acode"] = eaacode;
                request["vendorid"] = deposit.Mode == LoginMode.试玩 ? testvendorid : eavendorid;
                request["currencyid"] = deposit.CurrencyID;
                request["amount"] = deposit.Amount;
                request["refno"] = deposit.Refno;
                 
                string response = request.GetResponse(eadeposit_url);
               // return MakeAndParsXML.parsEADepositXML(response);
                #region parse response
                using (StringReader sr = new StringReader(response.Trim()))
                {
                    using (XmlTextReader xr = new XmlTextReader(sr))
                    {
                        foreach (var n0 in xr.ReadElement(0, "request"))
                        {
                            foreach (var n1 in xr.ReadElement(1, "element"))
                            {
                                foreach (var n2 in xr.ReadElement(2, "properties"))
                                {
                                    //while (xr.Read(2))
                                    //{
                                        if (xr.GetAttribute("name") == "acode")
                                        {
                                            while (xr.Read(3))
                                                if (xr.IsTextNode(3))
                                                    deposit.Acode = xr.Value;
                                        }
                                        else if (xr.GetAttribute("name") == "status")
                                        {
                                            while (xr.Read(3))
                                                if (xr.IsTextNode(3))
                                                    deposit.Status = xr.Value;
                                        }
                                        else if (xr.GetAttribute("name") == "refno")
                                        {
                                            while (xr.Read(3))
                                                if (xr.IsTextNode(3))
                                                    deposit.Refno = xr.Value;
                                        }
                                        else if (xr.GetAttribute("name") == "paymentid")
                                        {
                                            while (xr.Read(3))
                                                if (xr.IsTextNode(3))
                                                    deposit.PaymentID = xr.Value;
                                        }
                                        else if (xr.GetAttribute("name") == "errdesc")
                                        {
                                            while (xr.Read(3))
                                                if (xr.IsTextNode(3))
                                                    deposit.Errdesc = xr.Value;
                                            return deposit;
                                        }
                                    //}
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            return deposit;
        }
        public static string DepositConfirm(EAFanishInfo deposit)
        {
            using (eaRequest request = new eaRequest("cdeposit-confirm", deposit.FuncID))
            {
                request["acode"] = eaacode;
                request["status"] = deposit.Status;
                request["paymentid"] = deposit.PaymentID;
                request["errdesc"] = deposit.Errdesc;
                 
                string response = request.GetResponse(eadeposit_url);
                return response;
            }
        }

        public static EAFanishInfo Withdraw(EAFanishInfo withdraw)
        {
            using (eaRequest request = new eaRequest("cwithdrawal", withdraw.FuncID))
            {
                request["userid"] = withdraw.UserID;
                request["vendorid"] = withdraw.Mode== LoginMode.试玩 ? testvendorid : eavendorid;
                request["currencyid"] = withdraw.CurrencyID;
                request["amount"] = withdraw.Amount;
                request["refno"] = withdraw.Refno;
                 
                string response = request.GetResponse(eawithdraw_url);
                #region parse response
                using (StringReader sr = new StringReader(response.Trim()))
                {
                    using (XmlTextReader xr = new XmlTextReader(sr))
                    {
                        foreach (var n0 in xr.ReadElement(0, "request"))
                        {
                            foreach (var n1 in xr.ReadElement(1, "element"))
                            {
                                foreach (var n2 in xr.ReadElement(2, "properties"))
                                {
                                   if (xr.GetAttribute("name") == "status")
                                    {
                                        while (xr.Read(3))
                                            if (xr.IsTextNode(3))
                                                withdraw.Status = xr.Value;
                                    }
                                    else if (xr.GetAttribute("name") == "refno")
                                    {
                                        while (xr.Read(3))
                                            if (xr.IsTextNode(3))
                                                withdraw.Refno = xr.Value;
                                    }
                                    else if (xr.GetAttribute("name") == "paymentid")
                                    {
                                        while (xr.Read(3))
                                            if (xr.IsTextNode(3))
                                                withdraw.PaymentID = xr.Value;
                                    }
                                    else if (xr.GetAttribute("name") == "errdesc")
                                    {
                                        while (xr.Read(3))
                                            if (xr.IsTextNode(3))
                                                withdraw.Errdesc = xr.Value;
                                        return withdraw;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                return withdraw;
            }
        }
        /// <summary>
        /// 获取用户账户信息
        /// </summary>
        /// <param name="fanish"></param>
        /// <returns></returns>
        public static EAFanishInfo GetUserBalance(EAFanishInfo fanish)
        {
            using (eaRequest request = new eaRequest("ccheckclient", fanish.FuncID))
            {
                request["userid"] = fanish.UserID;
                request["vendorid"] = fanish.Mode== LoginMode.试玩 ? testvendorid : eavendorid;
                request["currencyid"] = fanish.CurrencyID;
                 
                string response = request.GetResponse(eabalance_url);
                #region parse response
                using (StringReader sr = new StringReader(response.Trim()))
                {
                    using (XmlTextReader xr = new XmlTextReader(sr))
                    {
                        foreach (var n0 in xr.ReadElement(0, "request"))
                        {
                            foreach (var n1 in xr.ReadElement(1, "element"))
                            {
                                foreach (var n2 in xr.ReadElement(2, "properties"))
                                {
                                    if (xr.GetAttribute("name") == "userid")
                                    {
                                        while (xr.Read(3))
                                            if (xr.IsTextNode(3))
                                                fanish.UserID = xr.Value;
                                    }
                                    else if (xr.GetAttribute("name") == "balance")
                                    {
                                        while (xr.Read(3))
                                            if (xr.IsTextNode(3))
                                                fanish.Balance = xr.Value.ToSingle();
                                    }
                                    else if (xr.GetAttribute("name") == "currencyid")
                                    {
                                        while (xr.Read(3))
                                            if (xr.IsTextNode(3))
                                                fanish.CurrencyID = xr.Value;
                                    }
                                    else if (xr.GetAttribute("name") == "location")
                                    {
                                        while (xr.Read(3))
                                            if (xr.IsTextNode(3))
                                                fanish.Location = xr.Value;
                                        return fanish;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            return fanish;
        }
        /// <summary>
        /// 获取玩家输赢记录和公司输赢记录
        /// </summary>
        /// <param name="eaplayer"></param>
        /// <returns></returns>
        public static List<EAaffiliate> GetAgentPlayerInfo(EAPlayerInfo eaplayer)
        {
            using (eaRequest request = new eaRequest("ccheckaffiliate", eaplayer.CheckID))
            {
                request["vendorid"] = eaplayer.Mode== LoginMode.试玩 ? testvendorid : eavendorid;
                request["acode"] = "<acode>" + eaacode + "</acode>";
                request["begindate", "yyyy-MM-dd"] = eaplayer.BeginDate;
                request["enddate"] = eaplayer.EndDate;
                 
                string response = request.GetResponse(eaaffiliate_url);
                #region parse response
                List<EAaffiliate> LstAffiliate = new List<EAaffiliate>();
                using (StringReader sr = new StringReader(response.Trim()))
                {
                    using (XmlTextReader xr = new XmlTextReader(sr))
                    {
                        foreach (var n0 in xr.ReadElement(0, "request"))
                        {
                            foreach (var n1 in xr.ReadElement(1, "element"))
                            {
                                foreach (var n2 in xr.ReadElement(2, "properties"))
                                {
                                    if (xr.GetAttribute("acode") == eaacode)
                                    {

                                        foreach (var n3 in xr.ReadElement(3,"date"))
                                        {
                                            EAaffiliate affiliate = new EAaffiliate();
                                            affiliate.Date = xr.GetAttribute("value");
                                            while (xr.Read(4))
                                            {
                                                if (xr.IsStartElement(4, "lostwin"))
                                                {
                                                    while (xr.Read(5))
                                                        if (xr.IsTextNode(5))
                                                            affiliate.LostWin = xr.Value;
                                                }
                                                else if (xr.IsStartElement(4, "turnover"))
                                                {
                                                    while (xr.Read(5))
                                                        if (xr.IsTextNode(5))
                                                            affiliate.TurnOver = xr.Value;
                                                }
                                                else if (xr.IsStartElement(4, "firstlogin"))
                                                {
                                                    while (xr.Read(5))
                                                        if (xr.IsTextNode(5))
                                                            affiliate.FirstLogin = xr.Value;
                                                }
                                                else if (xr.IsStartElement(4, "newfunded"))
                                                {
                                                    while (xr.Read(5))
                                                        if (xr.IsTextNode(5))
                                                            affiliate.NewFunded = xr.Value;
                                                }
                                                else if (xr.IsStartElement(4, "totalbet"))
                                                {
                                                    while (xr.Read(5))
                                                        if (xr.IsTextNode(5))
                                                            affiliate.Totalbet = xr.Value;
                                                }
                                                else if (xr.IsStartElement(4, "firstbets"))
                                                {
                                                    while (xr.Read(5))
                                                        if (xr.IsTextNode(5))
                                                            affiliate.Firstbets = xr.Value;
                                                }
                                                else if (xr.IsStartElement(4, "player"))
                                                {
                                                    EAFanPlayer playerfirst = new EAFanPlayer();
                                                    playerfirst.UserID = xr.GetAttribute("id");
                                                    while (xr.Read(5))
                                                    {
                                                        if (xr.IsStartElement(5, "lostwin"))
                                                        {
                                                            while (xr.Read(6))
                                                                if (xr.IsTextNode(6))
                                                                    playerfirst.LostWin = xr.Value;
                                                        }
                                                        else if (xr.IsStartElement(5, "turnover"))
                                                        {
                                                            while (xr.Read(6))
                                                                if (xr.IsTextNode(6))
                                                                    playerfirst.TurnOver = xr.Value;
                                                            affiliate.PlayerList.Add(playerfirst);
                                                            break;
                                                        }
                                                    }
                                                    foreach (var n4 in xr.ReadElement(4,"player"))
                                                    {
                                                        EAFanPlayer player = new EAFanPlayer();
                                                        player.UserID = xr.GetAttribute("id");
                                                        while (xr.Read(5))
                                                        {
                                                            if (xr.IsStartElement(5, "lostwin"))
                                                            {
                                                                while (xr.Read(6))
                                                                    if (xr.IsTextNode(6))
                                                                        player.LostWin = xr.Value;
                                                            }
                                                            else if (xr.IsStartElement(5, "turnover"))
                                                            {
                                                                while (xr.Read(6))
                                                                    if (xr.IsTextNode(6))
                                                                        player.TurnOver = xr.Value;
                                                                affiliate.PlayerList.Add(player);
                                                                break;
                                                            }
                                                        }
                                                        
                                                    }
                                                    break;
                                                }
                                            }
                                            LstAffiliate.Add(affiliate);
                                        }
                                        return LstAffiliate;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                return LstAffiliate;
            }
            
        }

        /// <summary>
        /// 获取玩家输赢记录和公司输赢记录
        /// </summary>
        /// <param name="eaplayer"></param>
        /// <returns></returns>
        public static string GetAgentPlayerInfo(EAPlayerInfo eaplayer,bool ok)
        {
            using (eaRequest request = new eaRequest("ccheckaffiliate", eaplayer.CheckID))
            {
                request["vendorid"] = eaplayer.Mode== LoginMode.试玩 ? testvendorid : eavendorid;
                request["acode"] = "<acode>" + eaacode + "</acode>";
                request["begindate", "yyyy-MM-dd"] = eaplayer.BeginDate;
                request["enddate"] = eaplayer.EndDate;

                string response = request.GetResponse(eaaffiliate_url);
                return response;
                //#region parse response
                //List<EAaffiliate> LstAffiliate = new List<EAaffiliate>();
                //using (StringReader sr = new StringReader(response.Trim()))
                //{
                //    using (XmlTextReader xr = new XmlTextReader(sr))
                //    {
                //        foreach (var n0 in xr.ReadElement(0, "request"))
                //        {
                //            foreach (var n1 in xr.ReadElement(1, "element"))
                //            {
                //                foreach (var n2 in xr.ReadElement(2, "properties"))
                //                {
                //                    if (xr.GetAttribute("acode") == eaacode)
                //                    {

                //                        foreach (var n3 in xr.ReadElement(3, "date"))
                //                        {
                //                            EAaffiliate affiliate = new EAaffiliate();
                //                            affiliate.Date = xr.GetAttribute("value");
                //                            while (xr.Read(4))
                //                            {
                //                                if (xr.IsStartElement(4, "lostwin"))
                //                                {
                //                                    while (xr.Read(5))
                //                                        if (xr.IsTextNode(5))
                //                                            affiliate.LostWin = xr.Value;
                //                                }
                //                                else if (xr.IsStartElement(4, "turnover"))
                //                                {
                //                                    while (xr.Read(5))
                //                                        if (xr.IsTextNode(5))
                //                                            affiliate.TurnOver = xr.Value;
                //                                }
                //                                else if (xr.IsStartElement(4, "firstlogin"))
                //                                {
                //                                    while (xr.Read(5))
                //                                        if (xr.IsTextNode(5))
                //                                            affiliate.FirstLogin = xr.Value;
                //                                }
                //                                else if (xr.IsStartElement(4, "newfunded"))
                //                                {
                //                                    while (xr.Read(5))
                //                                        if (xr.IsTextNode(5))
                //                                            affiliate.NewFunded = xr.Value;
                //                                }
                //                                else if (xr.IsStartElement(4, "totalbet"))
                //                                {
                //                                    while (xr.Read(5))
                //                                        if (xr.IsTextNode(5))
                //                                            affiliate.Totalbet = xr.Value;
                //                                }
                //                                else if (xr.IsStartElement(4, "firstbets"))
                //                                {
                //                                    while (xr.Read(5))
                //                                        if (xr.IsTextNode(5))
                //                                            affiliate.Firstbets = xr.Value;
                //                                }
                //                                else if (xr.IsStartElement(4, "player"))
                //                                {
                //                                    EAFanPlayer playerfirst = new EAFanPlayer();
                //                                    playerfirst.UserID = xr.GetAttribute("id");
                //                                    while (xr.Read(5))
                //                                    {
                //                                        if (xr.IsStartElement(5, "lostwin"))
                //                                        {
                //                                            while (xr.Read(6))
                //                                                if (xr.IsTextNode(6))
                //                                                    playerfirst.LostWin = xr.Value;
                //                                        }
                //                                        else if (xr.IsStartElement(5, "turnover"))
                //                                        {
                //                                            while (xr.Read(6))
                //                                                if (xr.IsTextNode(6))
                //                                                    playerfirst.TurnOver = xr.Value;
                //                                            affiliate.PlayerList.Add(playerfirst);
                //                                            break;
                //                                        }
                //                                    }
                //                                    foreach (var n4 in xr.ReadElement(4, "player"))
                //                                    {
                //                                        EAFanPlayer player = new EAFanPlayer();
                //                                        player.UserID = xr.GetAttribute("id");
                //                                        while (xr.Read(5))
                //                                        {
                //                                            if (xr.IsStartElement(5, "lostwin"))
                //                                            {
                //                                                while (xr.Read(6))
                //                                                    if (xr.IsTextNode(6))
                //                                                        player.LostWin = xr.Value;
                //                                            }
                //                                            else if (xr.IsStartElement(5, "turnover"))
                //                                            {
                //                                                while (xr.Read(6))
                //                                                    if (xr.IsTextNode(6))
                //                                                        player.TurnOver = xr.Value;
                //                                                affiliate.PlayerList.Add(player);
                //                                                break;
                //                                            }
                //                                        }

                //                                    }
                //                                    break;
                //                                }
                //                            }
                //                            LstAffiliate.Add(affiliate);
                //                        }
                //                        return LstAffiliate;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //#endregion
                //return LstAffiliate;
            }

        }
    }

    class eaRequest : apiRequest
    {
        public string Action { get; private set; }

        public eaRequest(string action,string id)
        {
            xml.WriteStartDocument();
            xml.WriteStartElement("request");
            xml.WriteAttributeString("action", this.Action = action);
            xml.WriteStartElement("element");
            xml.WriteAttributeString("id", id);
        }

        public object this[string name]
        {
            set
            {
                if (value == null) return;
                xml.WriteStartElement("properties");
                xml.WriteAttributeString("name", name);
                xml.WriteValue(util.EnumToValue(value));
                xml.WriteEndElement();
            }
        }
        public object this[string name,string format]
        {
            set
            {
                if (value == null) return;
                xml.WriteStartElement("properties");
                xml.WriteAttributeString("name", name);
                xml.WriteValue(DateTime.Parse(value.ToString()).ToString(format));
                xml.WriteEndElement();
            }
        }
    }

    public class EAFanishInfo
    {
        private LoginMode _mode = LoginMode.真正;
        public string FuncID { get; set; }
        public string UserID { get; set; }
        public string Acode { get; set; }
        public string VendorID { get; set; }
        public string CurrencyID { get; set; }
        public float? Amount { get; set; }
        public string Refno { get; set; }
        public string Status { get; set; }
        public string PaymentID { get; set; }
        public string Errdesc { get; set; }
        public float? Balance { get; set; }
        public string Location { get; set; }
        public LoginMode Mode
        {
            get { return this._mode; }
            set { this._mode = value; }
        }

        public override string ToString()
        {
            return string.Format("用户名：{0};金额：{1};location:{2}", this.UserID, this.Balance, this.Location);
        }

    }

    public class EAPlayerInfo
    {
        private LoginMode _mode = LoginMode.真正;
        public string CheckID { get; set; }
        public string VendorID { get; set; }
        public string Acode { get; set; }
        public LoginMode Mode
        {
            get { return this._mode; }
            set { this._mode = value; }
        }

        /// <summary>
        /// yyyy-mm-dd
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// yyyy-mm-dd
        /// </summary>
        public string EndDate { get; set; }
    }

    public class EAUserInfo
    {
        private LoginMode _mode = LoginMode.真正;
        public string LoginID { get; set; }
        /// <summary>
        /// 玩家登录名
        /// </summary>
        /// <value>The user ID.</value>
        public string UserID { get; set; }
        /// <summary>
        /// 玩家昵称
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
        public LoginMode Mode
        {
            get { return this._mode; }
            set { this._mode = value; }
        }

        /// <summary>
        /// 合营商代理
        /// 只有使用合营系统以及需要发送合营商代码
        ///以便标志在玩家账号的商家才需要提供此代码,不
        ///需要此代码的商家只需要回复空值即可
        /// </summary>
        /// <value>The acode.</value>
        public string Acode { get; set; }
        /// <summary>
        /// 商家ID
        /// </summary>
        /// <value>The vendor ID.</value>
        public string VendorID { get; set; }

        /// <summary>
        /// 货币ID（根据国际标准化组织的数字标准） ，例如： “ 156 “ ， “ 840 “等，
        /// </summary>
        /// <value>The currency ID.</value>
        public string CurrencyID { get; set; }
        /// <summary>
        /// 0: SUCCESS (成功)
        ///101: ERROR_INVALID_ACCOUNT_ID
        ///(错误：无效账户名称)
        ///102: ERROR_ALREADY_LOGIN
        ///(错误：已登陆)
        ///103: ERROR_DATABASE_ERROR
        ///(错误：数据库错误)
        ///104: ERROR_ACCOUNT_SUSPENDED
        ///(错误：帐户暂停）
        ///105: ERROR_INVALID_CURRENCY
        ///(错误：无效货币)
        ///601: ERROR_INVALID_LOGIN_URL
        ///(错误：登入连接无效)
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }
        /// <summary>
        /// 错误信息为非零答覆
        /// </summary>
        /// <value>The errdesc.</value>
        public string Errdesc { get; set; }

    }

    public class EAaffiliate
    {
        public string Date { get; set; }
        public string LostWin { get; set; }
        public string TurnOver { get; set; }
        public string FirstLogin { get; set; }
        public string NewFunded { get; set; }
        public string Totalbet { get; set; }
        public string Firstbets { get; set; }
        public List<EAFanPlayer> PlayerList = new List<EAFanPlayer>();

        public override string ToString()
        {
            string palyerStrings = "";
            foreach (EAFanPlayer item in PlayerList)
            {
                palyerStrings += item.ToString();
            }
            return Date + ";" + LostWin + ";" + FirstLogin+palyerStrings;
        }
    }
    public class EAFanPlayer
    {
        public string UserID { get; set; }
        public string LostWin { get; set; }
        public string TurnOver { get; set; }
        public override string ToString()
        {
            return "UserID:"+UserID+"LostWin:"+LostWin+"TurnOver:"+TurnOver;
        }
    }

    public enum LoginMode : int
    {
        试玩 = 0, 真正 = 1,
    }
}
