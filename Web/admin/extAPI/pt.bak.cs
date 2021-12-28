using BU;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using web;
using Tools.Protocol;
using System.Security.Cryptography.X509Certificates;
using System.Web;


namespace extAPI
{

    #region
    public class pt2
    {
        static readonly api_instances<pt> instances = new api_instances<pt>();
        public static pt GetInstance(int? corpID) { return instances[corpID]; }
        readonly int CorpID;
        [DebuggerStepThrough]
        public pt2(int corpID) { this.CorpID = corpID; }

        public static string log_prefix = "pt";

        [SqlSetting("pt", "prefix")]
        public string prefix { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("pt", "api_url")]
        public string api_url { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("pt", "kioskadminname")]
        public string kioskadminname { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("pt", "kioskname")]
        public string kioskname { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("pt", "EntityKEY")]
        public string EntityKEY { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("pt", "pp_pw")]
        public string pp_pw { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("pt", "keyFile")]
        public string keyFile { get { return HttpContext.Current.Server.MapPath(app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID)); } }

        public enum lang { zh_cn, zh_tw, en_us, euc_jp, ko, th, es, vi, khm, lao }

        public enum tran_action { deposit, withdraw }

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="ACNT">游戏帐号</param>
        /// <param name="kioskadminname"></param>
        /// <param name="kioskname"></param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public Request CreateMember(string ACNT, string kioskadminname, string kioskname, string pwd)
        {
            return PrepareRequest(this.api_url + "createPlayer" + "/" + ACNT + "/" + this.kioskadminname + "/" + this.kioskname + "/" + "password" + "/" + "123456"); //pwd
        }

        /// <summary>
        /// 登出玩家
        /// </summary>
        /// <param name="ACNT">游戏帐号</param>
        /// <returns></returns>
        public Request Logout(string ACNT)
        {
            return PrepareRequest(this.api_url + "logoutPlayer" + "/" + ACNT);
        }

        /// <summary>
        /// 检查余额
        /// </summary>
        /// <param name="ACNT">游戏帐号</param>
        /// <returns></returns>
        public Request CheckUsrBalance(string ACNT)
        {
            return PrepareRequest(this.api_url + "getPlayerInfo" + "/" + ACNT);
        }

        /// <summary>
        /// 户内转账
        /// </summary>
        /// <param name="action">存款/提款</param>
        /// <param name="ACNT">游戏帐号</param>
        /// <param name="amount">操作金额</param>
        /// <param name="externaltranid">订单号</param>
        /// <returns></returns>
        public Request Transfer(tran_action action, string ACNT, decimal? amount, string externaltranid)
        {
            return PrepareRequest(this.api_url + action.ToString() + "/" + ACNT + "/" + amount + "/" + this.kioskadminname + "/" + "externaltranid" + "/" + externaltranid);
        }

        /// <summary>
        /// 检查存取款交易的状态
        /// </summary>
        public Request CheckTransfer(string externaltranid)
        {
            return PrepareRequest(this.api_url + "externaltranid" + "/" + externaltranid);
        }

        /// <summary>
        /// 开始请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Request PrepareRequest(string url)
        {
            Request request = new Request();
            request.GetResponse(url, this.EntityKEY, this.keyFile, this.pp_pw);
            return request;
        }

        public class Request
        {
            public string RequestUrl { get; private set; }
            public string ResponseText { get; private set; }
            public JObject Response;

            /// <summary>
            /// 成功注册玩家名字
            /// </summary>
            public string playername
            {
                get { try { return this.Response.Value<string>("playername"); } catch { return null; } }
            }

            /// <summary>
            /// 错误信息
            /// </summary>
            public string error
            {
                get { try { return this.Response.Value<string>("error"); } catch { return null; } }
            }

            /// <summary>
            /// 玩家余额
            /// </summary>
            public decimal? BALANCE
            {
                get { try { return this.Response.Value<decimal>("BALANCE"); } catch { return null; } }
            }

            /// <summary>
            /// 查询余额玩家名称
            /// </summary>
            public string USERNAME
            {
                get { try { return this.Response.Value<string>("USERNAME"); } catch { return null; } }
            }

            /// <summary>
            /// 转账结果
            /// </summary>
            public string result
            {
                get { try { return this.Response.Value<string>("result"); } catch { return null; } }
            }

            public void GetResponse(string api_url, string EntityKEY, string keyFile, string pp_pw)
            {
                this.ResponseText = util.GetResponse(this.RequestUrl = api_url, EntityKEY, keyFile, pp_pw, pt.log_prefix, null, null);
                try { this.Response = JObject.Parse(this.ResponseText); }
                catch { this.Response = Tools._null<JObject>.value; }
            }
        }
    }


    [JsonObject]
    public class pt_test
    {
        [ObjectInvoke]
        static object test(pt_test command, string json, params object[] args)
        {
            //pt2.GetInstance(2).GetPlayerInfo("123");
            pt2.GetInstance(2).CreatePlayer("123", null, null, null, "aa", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            return null;
        }
    }
    #endregion

    public class pt
    {
        static readonly api_instances<pt> instances = new api_instances<pt>();
        [DebuggerStepThrough]
        public static pt GetInstance(int? corpID) { return instances[corpID]; }
        readonly int CorpID;
        [DebuggerStepThrough]
        public pt(int corpID) { this.CorpID = corpID; }

        public static string log_prefix = "pt";

        [SqlSetting("pt", "prefix")]
        public string prefix
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }
        [SqlSetting("pt", "api_url")]
        public string api_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }
        [SqlSetting("pt", "kioskadminname")]
        public string kioskadminname
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }
        [SqlSetting("pt", "kioskname")]
        public string kioskname
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }
        [SqlSetting("pt", "EntityKEY")]
        public string EntityKEY
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }
        [SqlSetting("pt", "pp_pw")]
        public string pp_pw
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }
        [SqlSetting("pt", "keyFile")]
        public string keyFile
        {
            get { return HttpContext.Current.Server.MapPath(app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID)); }
        }

        public enum lang { zh_cn, zh_tw, en_us, euc_jp, ko, th, es, vi, khm, lao }

        public class PlayerInfo
        {
            public string CASINONAME;
            public string USERNAME;
            public string ADDRESS;
            public string BIRTHDATE;
            public string CITY;
            public string COUNTRYCODE;
            public string CURRENCY;
            public string EMAIL;
            public string FAX;
            public string FIRSTNAME;
            public string LASTNAME;
            public string NOBONUS;
            public string PHONE;
            public string STATE;
            public string VIPLEVEL;
            public string WANTMAIL;
            public string ZIP;
            public string ADVERTISER;
            public string BANNERID;
            public string CLIENTTYPE;
            public string CREFERER;
            public string LANGUAGECODE;
            public string SIGNUPDATE;
            public string SIGNUPCLIENTVERSION;
            public string PASSWORD;
            public string FROZEN;
            public string CLIENTSKIN;
            public string COMMENTS;
            public string SIGNUPCLIENTPLATFORM;
            public string ISINTERNALPLAYER;
            public bool? SUSPENDED;
            public string FULLFIRSTNAME;
            public string FULLFIRSTSURNAME;
            public string FULLSECONDSURNAME;
            public string TAXRESIDENCEREGION;
            public string CODE;
            public string BALANCE;
            public string NTRIES;
            public string MAXBALANCE;
            public string RESERVEDBALANCE;
            public string CURRENTBET;
            public string CURRENTBONUSBET;
            public string PENDINGBONUSBALANCE;
            public string BONUSBALANCE;
            public string COMPPOINTS;
            public string TOTALCOMPPOINTS;
            public string KIOSKCODE;
            public string KIOSKADMINCODE;
            public string KIOSKNAME;
            public string KIOSKADMINNAME;
            public string LOCKEDMINUTES;
            public string CURRENCYCODE;
            public string CASINO_NICKNAME;
            public string[] NETWORK_NICKNAMES;  // array|null
            public bool? ISINGAME;
        }

        /// <summary>
        /// Get Player Info
        /// </summary>
        /// <param name="player">Player’s username</param>
        /// <returns></returns>
        public Request GetPlayerInfo(string player)
        {
            return new Request("getPlayerInfo")
            {
                {"/player", player},
            }.GetResponse(this);
        }

        /// <summary>
        /// Create player
        /// </summary>
        /// <param name="playername"></param>
        /// <param name="kioskadminname"></param>
        /// <param name="kioskname"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="countrycode"></param>
        /// <param name="city"></param>
        /// <param name="zip"></param>
        /// <param name="address"></param>
        /// <param name="state"></param>
        /// <param name="phone"></param>
        /// <param name="fax"></param>
        /// <param name="email"></param>
        /// <param name="comments"></param>
        /// <param name="birthdate"></param>
        /// <param name="viplevel">(if allowed for kioskadmin who create player)</param>
        /// <param name="languagecode"></param>
        /// <param name="sex"></param>
        /// <param name="password"></param>
        /// <param name="customFieldsFromStructure"></param>
        /// <param name="custom01"></param>
        /// <param name="custom02"></param>
        /// <param name="custom03"></param>
        /// <param name="custom04"></param>
        /// <param name="custom05"></param>
        /// <param name="custom06"></param>
        /// <param name="custom07"></param>
        /// <param name="custom08"></param>
        /// <param name="custom09"></param>
        /// <param name="custom10"></param>
        /// <param name="custom11"></param>
        /// <param name="custom12"></param>
        /// <param name="custom13"></param>
        /// <param name="custom14"></param>
        /// <param name="custom15"></param>
        /// <param name="custom16"></param>
        /// <param name="custom17"></param>
        /// <param name="custom18"></param>
        /// <param name="custom19"></param>
        /// <param name="custom20"></param>
        /// <returns></returns>
        public Request CreatePlayer(string playername, string kioskadminname, string kioskname, string firstname, string lastname, string countrycode, string city, string zip, string address, string state, string phone, string fax, string email, string comments, string birthdate, string viplevel, string languagecode, string sex, string password, bool? customFieldsFromStructure, string custom01, string custom02, string custom03, string custom04, string custom05, string custom06, string custom07, string custom08, string custom09, string custom10, string custom11, string custom12, string custom13, string custom14, string custom15, string custom16, string custom17, string custom18, string custom19, string custom20)
        {
            string _customFieldsFromStructure = null; if (customFieldsFromStructure.HasValue) _customFieldsFromStructure = customFieldsFromStructure.Value ? "yes" : "no";
            return new Request("createPlayer")
            {
                {"/playername"               , playername},
                {"/kioskadminname"           , kioskadminname ?? this.kioskadminname},
                {"/kioskname"                , kioskname ?? this.kioskname},
                {"firstname"                 , firstname},
                {"lastname"                  , lastname},
                {"countrycode"               , countrycode},
                {"city"                      , city},
                {"zip"                       , zip},
                {"address"                   , address},
                {"state"                     , state},
                {"phone"                     , phone},
                {"fax"                       , fax},
                {"email"                     , email},
                {"comments"                  , comments},
                {"birthdate"                 , birthdate},
                {"viplevel"                  , viplevel},
                {"languagecode"              , languagecode},
                {"sex"                       , sex},
                {"password"                  , password},
                {"customFieldsFromStructure" , _customFieldsFromStructure},
                {"custom01" , custom01}, {"custom02" , custom02}, {"custom03" , custom03}, {"custom04" , custom04}, {"custom05" , custom05}, {"custom06" , custom06}, {"custom07" , custom07}, {"custom08" , custom08}, {"custom09" , custom09}, {"custom10" , custom10},
                {"custom11" , custom11}, {"custom12" , custom12}, {"custom13" , custom13}, {"custom14" , custom14}, {"custom15" , custom15}, {"custom16" , custom16}, {"custom17" , custom17}, {"custom18" , custom18}, {"custom19" , custom19}, {"custom20" , custom20},
            }.GetResponse(this);
        }

        public Request CreatePlayer(string playername, string kioskadminname, string kioskname, string firstname, string lastname, string password)
        {
            return CreatePlayer(playername, kioskadminname, kioskname, firstname, lastname, null, null, null, null, null, null, null, null, null, null, null, null, null, password, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Update player
        /// </summary>
        /// <param name="playername"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="countrycode"></param>
        /// <param name="city"></param>
        /// <param name="zip"></param>
        /// <param name="address"></param>
        /// <param name="state"></param>
        /// <param name="phone"></param>
        /// <param name="fax"></param>
        /// <param name="email"></param>
        /// <param name="comments"></param>
        /// <param name="birthdate"></param>
        /// <param name="viplevel"></param>
        /// <param name="languagecode"></param>
        /// <param name="sex"></param>
        /// <param name="password"></param>
        /// <param name="custom01"></param>
        /// <param name="custom02"></param>
        /// <param name="custom03"></param>
        /// <param name="custom04"></param>
        /// <param name="custom05"></param>
        /// <param name="custom06"></param>
        /// <param name="custom07"></param>
        /// <param name="custom08"></param>
        /// <param name="custom09"></param>
        /// <param name="custom10"></param>
        /// <param name="custom11"></param>
        /// <param name="custom12"></param>
        /// <param name="custom13"></param>
        /// <param name="custom14"></param>
        /// <param name="custom15"></param>
        /// <param name="custom16"></param>
        /// <param name="custom17"></param>
        /// <param name="custom18"></param>
        /// <param name="custom19"></param>
        /// <param name="custom20"></param>
        /// <returns></returns>
        public Request UpdatePlayer(string playername, string firstname, string lastname, string countrycode, string city, string zip, string address, string state, string phone, string fax, string email, string comments, string birthdate, string viplevel, string languagecode, string sex, string password, string custom01, string custom02, string custom03, string custom04, string custom05, string custom06, string custom07, string custom08, string custom09, string custom10, string custom11, string custom12, string custom13, string custom14, string custom15, string custom16, string custom17, string custom18, string custom19, string custom20)
        {
            return new Request("updatePlayer")
            {
                {"/playername"               , playername},
                {"firstname"                 , firstname},
                {"lastname"                  , lastname},
                {"countrycode"               , countrycode},
                {"city"                      , city},
                {"zip"                       , zip},
                {"address"                   , address},
                {"state"                     , state},
                {"phone"                     , phone},
                {"fax"                       , fax},
                {"email"                     , email},
                {"comments"                  , comments},
                {"birthdate"                 , birthdate},
                {"viplevel"                  , viplevel},
                {"languagecode"              , languagecode},
                {"sex"                       , sex},
                {"password"                  , password},
                {"custom01" , custom01}, {"custom02" , custom02}, {"custom03" , custom03}, {"custom04" , custom04}, {"custom05" , custom05}, {"custom06" , custom06}, {"custom07" , custom07}, {"custom08" , custom08}, {"custom09" , custom09}, {"custom10" , custom10},
                {"custom11" , custom11}, {"custom12" , custom12}, {"custom13" , custom13}, {"custom14" , custom14}, {"custom15" , custom15}, {"custom16" , custom16}, {"custom17" , custom17}, {"custom18" , custom18}, {"custom19" , custom19}, {"custom20" , custom20},
            }.GetResponse(this);
        }

        public Request UpdatePlayer(string playername, string password)
        {
            return UpdatePlayer(playername, password);
        }

        /// <summary>
        /// Move player
        /// </summary>
        /// <param name="playername"></param>
        /// <param name="kioskadminname"></param>
        /// <param name="kioskname"></param>
        /// <returns></returns>
        public Request MovePlayer(string playername, string kioskadminname, string kioskname)
        {
            return new Request("movePlayer")
            {
                {"/playername"      , playername},
                {"/kioskadminname"  , kioskadminname ?? this.kioskadminname},
                {"/kioskname"       , kioskname ?? this.kioskname},
            }.GetResponse(this);
        }

        /// <summary>
        /// Deposit action
        /// </summary>
        /// <param name="playername">player username</param>
        /// <param name="amount">amount to deposit</param>
        /// <param name="kioskadminname">kiosk admin name</param>
        /// <param name="externaltranid">external tracking ID (optional)</param>
        /// <returns></returns>
        public Request Deposit(string playername, decimal? amount, string kioskadminname, string externaltranid)
        {
            return new Request("deposit")
            {
                {"/playername"      , playername},
                {"/amount"          , amount},
                {"/kioskadminname"  , kioskadminname ?? this.kioskadminname},
                {"externaltranid"   , externaltranid},
            }.GetResponse(this);

        }

        /// <summary>
        /// Withdrawal action
        /// </summary>
        /// <param name="playername">player username</param>
        /// <param name="amount">amount to withdrawal</param>
        /// <param name="kioskadminname">kiosk admin name</param>
        /// <param name="externaltranid">external tracking ID (optional)</param>
        /// <returns></returns>
        public Request Withdrawal(string playername, decimal? amount, string kioskadminname, string externaltranid)
        {
            return new Request("withdraw")
            {
                {"/playername"      , playername},
                {"/amount"          , amount},
                {"/kioskadminname"  , kioskadminname ?? this.kioskadminname},
                {"externaltranid"   , externaltranid},
            }.GetResponse(this);
        }

        public class Request : Dictionary<string, object>
        {
            public string Action { get; private set; }
            public Request(string action) { this.Action = action; }

            public string RequestUrl { get; private set; }
            public string ResponseText { get; private set; }
            public JObject Response;

            /// <summary>
            /// 成功注册玩家名字
            /// </summary>
            public string playername
            {
                get { try { return this.Response.Value<string>("playername"); } catch { return null; } }
            }

            /// <summary>
            /// 错误信息
            /// </summary>
            public string error
            {
                get { try { return this.Response.Value<string>("error"); } catch { return null; } }
            }

            /// <summary>
            /// 玩家余额
            /// </summary>
            public decimal? BALANCE
            {
                get { try { return this.Response.Value<decimal>("BALANCE"); } catch { return null; } }
            }

            /// <summary>
            /// 查询余额玩家名称
            /// </summary>
            public string USERNAME
            {
                get { try { return this.Response.Value<string>("USERNAME"); } catch { return null; } }
            }

            /// <summary>
            /// 转账结果
            /// </summary>
            public string result
            {
                get { try { return this.Response.Value<string>("result"); } catch { return null; } }
            }

            /// <summary>
            /// 发起请求
            /// </summary>
            /// <param name="pt"></param>
            /// <param name="args"></param>
            /// <returns></returns>
            internal Request GetResponse(pt pt, params object[] args)
            {
                StringBuilder s = new StringBuilder();
                s.Append(pt.api_url);
                if (!pt.api_url.EndsWith("/"))
                    s.Append('/');
                s.Append(this.Action);
                foreach (KeyValuePair<string, object> p in this)
                {
                    if (string.IsNullOrEmpty(p.Key) || (p.Value == null)) continue;
                    if (!p.Key.StartsWith("/"))
                        s.AppendFormat("/{0}", p.Key);
                    s.AppendFormat("/{0}", p.Value);
                }
                this.RequestUrl = s.ToString();

                DateTime t1 = DateTime.Now;
                string prefix = extAPI.util.extAPI + "." + log_prefix;
                try
                {
                    HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(this.RequestUrl);
                    httpRequest.Headers.Add("X_ENTITY_KEY", pt.EntityKEY);
                    httpRequest.Method = "GET";
                    X509Certificate2 x509 = new X509Certificate2(System.IO.File.ReadAllBytes(pt.keyFile), pt.pp_pw, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                    httpRequest.ClientCertificates.Add(x509);
                    log.message(prefix, "Request:\t{0}", this.RequestUrl);
                    using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                log.message(prefix, "Response ({1}ms):\t{0}", this.ResponseText = sr.ReadToEnd(), (int)(DateTime.Now - t1).TotalMilliseconds);
                                try { this.Response = JObject.Parse(this.ResponseText); }
                                catch { this.Response = Tools._null<JObject>.value; }
                                return this;
                            }
                        }
                        else
                            throw new HttpException((int)response.StatusCode, response.StatusDescription);
                    }
                }
                catch (Exception ex)
                {
                    log.message(prefix, "Error ({1}ms):\t{0}", ex, (int)(DateTime.Now - t1).TotalMilliseconds);
                    throw ex;
                }
            }
        }
    }
}