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
using System.Data;

namespace extAPI
{
    #region
    public class PtApi
    {
        static readonly api_instances<PtApi> instances = new api_instances<PtApi>();
        [DebuggerStepThrough]
        public static PtApi GetInstance(int? corpID) 
        {
            return instances[corpID]; 
        }
        readonly int CorpID;
        [DebuggerStepThrough]
        public PtApi(int corpID)
        { 
            this.CorpID = corpID; 
        }
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
        [SqlSetting("pt", "key_pwd")]
        public string pp_pw
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }
        [SqlSetting("pt", "keyPath")]
        public string keyPath
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }
        public class CreatePlayer
        {
            public string playername;
            public string password;
            public string error;
            public string errorcode;
        }
        public class UpdatePlayer
        {
            public string result;
            public string error;
            public string errorcode;
        }
        public class GetPlayerInfo
        {
            public string casinoname;
            public string uaername;
            public string address;
            public string birthday;
            public string city;
            public string countrycode;
            public string currency;
            public string customer01;
            public string customer02;
            public string email;
            public string fax;
            public string firstname;
            public string lastname;
            public string nobonus;
            public string occupation;
            public string phone;
            public string sex;
            public string state;
            public string viplevel;
            public string wantmail;
            public string zip;
            public string advertiser;
            public string bannerid;
            public string clienttype;
            public string creferer;
            public string languagecode;
            public string serial;
            public string signupdate;
            public string signupclientversion;
            public string password;
            public string frozen;
            public string clientskin;
            public string comments;
            public string signupclientplatform;
            public string isinternalplayer;
            public string suspended;
            public string fullfirstname;
            public string fullfirstsurname;
            public string fullsecondsurname;
            public string taxresidenceregion;
            public string code;
            public string balance;
            public string ntries;
            public string maxbalance;
            public string reservedbalance;
            public string currentbet;
            public string currentbonusbet;
            public string pendingbonusbalance;
            public string bonusbalance;
            public string comppoints;
            public string totalcomppoints;
            public string kioscode;
            public string kiosadmincode;
            public string casinocode;
            public string lockminutes;
            public string currencycode;
            public string casino_nickname;
            public string network_nicknames;
            public string isingame;
            public string kiosname;
            public string kiosadminname;
            public string error;
            public string errorcode;
        }
        public class IsPlayerOnline
        {
            public string result;
            public string error;
            public string errorcode;
        }
        public class Deposit
        {
            public string result;
            public string error;
            public string errorcode;
        }
        public class Withdraw
        {
            public string result;
            public string error;
            public string errorcode;
        }
        public class CheckTransAction
        {
            public string status;
            public string error;
            public string errorcode;
        }

        public enum COMMANDID
        {
            /// <summary>
            /// 请求创建账户
            /// </summary>
            CREATE_PLAYER_REQUEST = 0x01,
            /// <summary>
            /// 创建账户回应
            /// </summary>
            CREATE_PLAYER_RESPONSE,
            /// <summary>
            /// 请求更新账户信息
            /// </summary>
            UPDATE_PLAYER_REQUEST,
            /// <summary>
            /// 更新账户信息回应
            /// </summary>
            UPDATE_PLAYER_RESPONSE,
            /// <summary>
            /// 请求读取账户信息
            /// </summary>
            GET_PLAYERINFO_REQUEST,
            /// <summary>
            /// 读取账户信息回应
            /// </summary>
            GET_PLAYERINFO_RESPONSE,
            /// <summary>
            /// 请求查询账户是否在线
            /// </summary>
            IS_PLAYERONLINE_REQUEST,
            /// <summary>
            /// 查询账户是否在线回应
            /// </summary>
            IS_PLAYERONLINE_RESPONSE,
            /// <summary>
            /// get balance
            /// </summary>
            GET_PLAYERBALANCE_REQUEST,
            /// <summary>
            /// 存款请求
            /// </summary>
            DEPOSIT_REQUEST,
            /// <summary>
            /// 存款回应
            /// </summary>
            DEPOSIT_RESPONSE,
            /// <summary>
            /// 提款请求
            /// </summary>
            WITHDRAW_REQUEST,
            /// <summary>
            /// 提款回应
            /// </summary>
            WITHDRAW_RESPONSE,
            /// <summary>
            /// 请求查询转账状态
            /// </summary>
            CHECKTRANSACTION_REQUEST,
            /// <summary>
            /// 查询转账状态回应
            /// </summary>
            CHECKTRANSACTION_RESPONSE,
            /// <summary>
            /// 请求账户状态
            /// </summary>
            PLAYERSTATS_REQUEST,
            /// <summary>
            /// 账户状态回应
            /// </summary>
            PLAYERSTATS_RESPONSE,
            /// <summary>
            /// 游戏状态请求
            /// </summary>
            GAMESTATS_REQUEST,
            /// <summary>
            /// 游戏状态回应
            /// </summary>
            GAMESTATE_RESPONSE,
            MASSCREATEPLAYERS_REQUEST,
            MASSCREATEPLAYERS_RESPONSE,
            MASSFREEZEPLAYERS_REQUEST,
            MASSFREEZEPLAYERS_RESPONSE,
            MASSDEPOSITPLAYERS_REQUEST,
            MASSDEPOSITPLAYERS_RESPONSE,
        }

        //api
        /// <summary>
        /// get player info
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Request GetPlayInfo(params object[] args)
        {
            return new Request("info").GetResponse(this, args);
        }
        /// <summary>
        /// get player balance
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Request GetPlayerBalance(params object[] args)
        {
            return new Request("balance").GetResponse(this, args);
        }
        /// <summary>
        /// withdraw 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        //public Request Withdraw(params object[] args)
        //{
        //    return new Request("withdraw").GetResponse(this, args);
        //}
        /// <summary>
        /// create new player
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        //public Request CreatePlayer(params object[] args)
        //{
        //    return new Request("create").GetResponse(this, args);
        //}
        /// <summary>
        /// trans to game--deposit
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        //public Request Deposit(params object[] args)
        //{
        //    return new Request("deposit").GetResponse(this, args);
        //}
        //connect request params
        private string CHECKTRANSACTION_REQUEST(params object[] args)
        {
            string externaltranid = args[0].ToString().Trim();
            StringBuilder Sb = new StringBuilder();
            Sb.AppendFormat(@"externaltranid={0}", externaltranid);
            return Sb.ToString();
        }
       
        private string WITHDRAW_REQUEST(params object[] args)
        {
            string playername = args[0].ToString().Trim();
            string amount = args[1].ToString().Trim();

            decimal amount_dec = Decimal.Parse(amount);
            int amount_int = 0;
            amount_dec = Math.Truncate(amount_dec);
            amount_int = (int)amount_dec;

            string externaltranid = args[3].ToString().Trim();
            StringBuilder Sb = new StringBuilder();
            Sb.AppendFormat(@"/player/withdraw/playername/{0}/amount/{1}/isForce/1/adminname/{2}/externaltranid/{3}", playername.ToUpper(), amount_int, this.kioskadminname, externaltranid);
            return Sb.ToString();
        }

        private string DEPOSIT_REQUEST(params object[] args)
        {
            string playername = args[0].ToString().Trim();
            string amount = args[1].ToString().Trim();

            decimal amount_dec = Decimal.Parse(amount);
            int amount_int = 0;
            amount_dec = Math.Truncate(amount_dec);
            amount_int = (int)amount_dec;

            string externaltranid = args[3].ToString().Trim();
            StringBuilder Sb = new StringBuilder();
            Sb.AppendFormat(@"/player/deposit/playername/{0}/amount/{1}/adminname/{2}/externaltranid/{3}", playername.ToUpper(), amount_int, this.kioskadminname, externaltranid);
            return Sb.ToString();
        }

        private string IS_PLAYERONLINE_REQUEST(params object[] args)
        {
            string playername = args[0].ToString().Trim();
            StringBuilder Sb = new StringBuilder();
            Sb.AppendFormat(@"/player/online/playername/{0}", playername.ToUpper());
            return Sb.ToString();
        }

        public string GET_PLAYERINFO_REQUEST(params object[] args)
        {
            string playername = args[0].ToString().Trim();
            StringBuilder Sb = new StringBuilder();
            Sb.AppendFormat(@"/player/info/playername/{0}/", playername.ToUpper());
            return Sb.ToString();
        }
        public string GET_PLAYER_BALANCE_REQUEST(params object[] args)
        {
            string playername = args[0].ToString().Trim();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"/player/balance/{0}/", playername.ToUpper());
            return sb.ToString();
        }

        private string UPDATE_PLAYER_REQUEST(params object[] args)
        {
            string playername = args[0].ToString().Trim();
            string password = args[1].ToString().Trim();
            StringBuilder Sb = new StringBuilder();
            Sb.AppendFormat(@"/player/update/playername/{0}/password/{1}/", playername.ToUpper(), password);
            return Sb.ToString();
        }

        private string CREATE_PLAYER_REQUEST(params object[] args)
        {
            string playername = args[0].ToString().Trim();
            string password = args[1].ToString().Trim();
            StringBuilder Sb = new StringBuilder();
            Sb.AppendFormat(@"/player/create/playername/{0}/password/{1}/adminname/{2}/kioskname/{3}/", playername.ToUpper(), password, this.kioskadminname, this.kioskname);
            return Sb.ToString();
        }
       

        public class Request
        {
            private string action { set; get; }
            public Request(string action) { this.action = action; }
            //def response
            public JObject response;
            public string request_url { set; get; }
            public string response_text { set; get; }

            /// <summary>
            /// 成功注册玩家名字
            /// </summary>
            public string playername
            {
                get { try { return this.response.Value<string>("playername"); } catch { return null; } }
            }

            /// <summary>
            /// 错误信息
            /// </summary>
            public string error
            {
                get { try { return this.response.Value<string>("error"); } catch { return null; } }
            }

            /// <summary>
            /// 玩家余额
            /// </summary>
            public decimal? BALANCE
            {
                get { try { return this.response.Value<decimal>("BALANCE"); } catch { return null; } }
            }

            /// <summary>
            /// 查询余额玩家名称
            /// </summary>
            public string USERNAME
            {
                get { try { return this.response.Value<string>("USERNAME"); } catch { return null; } }
            }

            /// <summary>
            /// 转账结果
            /// </summary>
            public string result
            {
                get { try { return this.response.Value<string>("result"); } catch { return null; } }
            }
            //get url param
            
            /// <summary>
            /// call request function
            /// </summary>
            /// <param name="pt"></param>
            /// <param name="args"></param>
            /// <returns></returns>
            internal Request GetResponse(PtApi pt, params object[] args)
            {
                StringBuilder s = new StringBuilder();
                s.Append(pt.api_url);
                switch(this.action)
                {
                    case "balance": { s.Append(pt.GET_PLAYER_BALANCE_REQUEST(extAPI.PtApi.COMMANDID.GET_PLAYERBALANCE_REQUEST, args)); } break;
                    case "info": { s.Append(pt.GET_PLAYERINFO_REQUEST(extAPI.PtApi.COMMANDID.GET_PLAYERINFO_REQUEST,args)); } break;
                    default: break;
                }
                this.request_url = s.ToString();

                DateTime t1 = DateTime.Now;
                string log_prefix = "pt";
                string prefix = extAPI.util.extAPI + "." + log_prefix;
                try
                {
                    HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(this.request_url);
                    httpRequest.Headers.Add("X_ENTITY_KEY", pt.EntityKEY);
                    httpRequest.Method = "GET";
                    X509Certificate2 x509 = new X509Certificate2(System.IO.File.ReadAllBytes(pt.keyPath), pt.pp_pw, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                    httpRequest.ClientCertificates.Add(x509);
                    log.message(prefix, "Request:\t{0}", this.request_url);
                    using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                log.message(prefix, "Response ({1}ms):\t{0}", this.response_text = sr.ReadToEnd(), (int)(DateTime.Now - t1).TotalMilliseconds);
                                try { this.response = JObject.Parse(this.response_text); }
                                catch { this.response = Tools._null<JObject>.value; }
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

    #endregion 
}