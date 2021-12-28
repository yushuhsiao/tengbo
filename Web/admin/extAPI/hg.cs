using BU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;
using web;

namespace extAPI
{
    [DebuggerStepThrough]
    public sealed class api_instances<T> where T : class
    {
        static readonly Dictionary<int, T> dict = new Dictionary<int, T>();
        public T this[int? corpID]
        {
            get
            {
                if (!corpID.HasValue)
                    return null;
                lock (dict)
                    if (dict.ContainsKey(corpID.Value))
                        return dict[corpID.Value];
                    else
                        return dict[corpID.Value] = (T)Activator.CreateInstance(typeof(T), corpID.Value);
            }
        }
    }
}

namespace extAPI.hg
{
    //[DebuggerStepThrough]
    //class test : IHttpHandler
    //{
    //    public void ProcessRequest(HttpContext context)
    //    {
    //        //hg.api.test(context);
    //    }
    //    public bool IsReusable { get { return false; } }
    //}


    //class PlayGame : IHttpHandler
    //{
    //    public bool IsReusable { get { return false; } }
    //    public void ProcessRequest(HttpContext context)
    //    {
    //        string httpMethod = context.Request.HttpMethod;
    //        string xml1;
    //        using (Stream s1 = context.Request.InputStream)
    //        using (StreamReader s2 = new StreamReader(s1))
    //            xml1 = s2.ReadToEnd();

    //        hgResponse1 response = hgResponse1.Parse(xml1);

    //        //string action = doc1.SelectSingleNode("/request/@action").Value;

    //        //XmlDocument doc2 = new XmlDocument();
    //        //XmlElement p;
    //        //doc2.AppendChild(doc2.CreateElement("response"));
    //        //doc2.DocumentElement.AppendChild(p = doc2.CreateElement("element"));
    //    }
    //}


    public class api
    {
        #region Configuration

        static readonly api_instances<api> instances = new api_instances<api>();
        public static api GetInstance(int? corpID) { return instances[corpID]; }
        readonly int CorpID;
        [DebuggerStepThrough]
        public api(int corpID) { this.CorpID = corpID; }

        [SqlSetting("hg", "game")]
        public string hg_game
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        [SqlSetting("hg", "callback")]
        public string hg_callback
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        [SqlSetting("hg", "api"), DefaultValue("https://live.winclubs.com/cgibin/EGameIntegration")]
        public string api_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        [SqlSetting("hg", "betapi"), DefaultValue("http://webapi-asia.hointeractive.com/betapi.asmx")]
        public static string betapi_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("hg", "username"), DefaultValue("winclubs")]
        public static string username
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("hg", "password"), DefaultValue("win@clubs@")]
        public static string password
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("hg", "casinoid"), DefaultValue("wins18club5g63yu")]
        public static string casinoid
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        /// <summary>
        /// 使用的前綴
        /// </summary>
        [SqlSetting("hg", "prefix")]
        public string prefix
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        /// <summary>
        /// 是否對帳號名稱進行編碼
        /// </summary>
        [SqlSetting("hg", "encode_acnt")]
        public bool EncodeACNT
        {
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        #endregion

        //internal static void test(HttpContext context)
        //{
        //    string httpMethod = context.Request.HttpMethod;
        //    string xml1;
        //    using (Stream s1 = context.Request.InputStream)
        //    using (StreamReader s2 = new StreamReader(s1))
        //        xml1 = s2.ReadToEnd();
        //    XmlDocument doc1 = new XmlDocument();
        //    doc1.LoadXml(xml1);

        //    string action = doc1.SelectSingleNode("/request/@action").Value;

        //    XmlDocument doc2 = new XmlDocument();
        //    XmlElement p;
        //    doc2.AppendChild(doc2.CreateElement("response"));
        //    doc2.DocumentElement.AppendChild(p = doc2.CreateElement("element"));

        //    ObjectInvoke2.CallByName(action, typeof(api), doc1, p);
        //    //MethodInfo m = typeof(api).GetMethod(action.Replace("-", "_"), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
        //    //if (m != null)
        //    //    m.Invoke(null, new object[] { doc1, p });

        //    string xml2 = doc2.OuterXml;
        //    using (Stream s3 = context.Response.OutputStream)
        //        doc2.Save(s3);

        //    //hgResponse1 a = hgResponse1.Parse(context.Request.InputStream);

        //    //using (hgRequest1 b = new hgRequest1(context.Response.OutputStream, "response", a.Action))
        //    //{
        //    //    MethodInfo m = typeof(api).GetMethod(a.Action.Replace("-", "_"), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
        //    //    if (m != null)
        //    //        m.Invoke(null, new object[] { a, b });
        //    //}
        //}
        //static void test(XmlElement e, string name, object value)
        //{
        //    XmlElement n = e.OwnerDocument.CreateElement("properties");
        //    n.SetAttribute("name", name);
        //    n.InnerText = util.EnumToValue(value).ToString();
        //    e.AppendChild(n);
        //}

        #region registration

        /// <summary>
        /// 登錄驗證請求消息
        /// </summary>
        /// <param name="username">用戶名,不定長字符串(50)</param>
        /// <param name="password">密碼,最少 6 位(可選)</param>
        /// <param name="mode">登錄模式。必須是 0 或 1。0 指代娛樂,1 指代真正</param>
        /// <param name="firstname">用戶的名,不定長字符串(50)</param>
        /// <param name="lastname">用戶的姓,不定長字符串(50)</param>
        /// <param name="currencyid">幣別代碼,不定長字符串(5) ISO 由三個字母組成的代碼 例如 USD</param>
        /// <param name="agentid">經紀人 ID 是指代 AMS 系統中某位經紀人的唯一號碼。 整數(4) (可選)</param>
        /// <param name="affiliateid">用戶的分支機構 ID。 不定長字符串(16) (可選)</param>
        /// <param name="testusr">用於創建測試帳戶。true 代表真正(0),false 代表測試帳戶(1) (可選)</param>
        /// <param name="playerlevel">玩家級。整數 4(可選) 如果傳遞了參數,則將對照該第三方的允許玩家級別進行驗 證。如果沒有傳,將採用默認級別。</param>
        public hgResponse1 registration(string username, string password, LoginMode mode, string firstname, string lastname, string currencyid, int? agentid, string affiliateid, TestUser? testusr, int? playerlevel)
        {
            hgRequest1 request = new hgRequest1("registration");
            request["username"] = username;
            request["password"] = password;
            request["mode"] = (int)mode;
            request["firstname"] = firstname;
            request["lastname"] = lastname;
            request["currencyid"] = currencyid;
            request["agentid"] = agentid;
            request["affiliateid"] = affiliateid;
            request["testusr"] = (int?)testusr;
            request["playerlevel"] = playerlevel;
            return request.GetResponse(this.api_url);
        }

        #endregion

        #region deposit

        /// <summary>
        /// 存款待審核請求消息
        /// </summary>
        /// <param name="username">第三方帳戶用戶名</param>
        /// <param name="mode">登錄模式。必須是 0 或 1。0 指代娛樂,1 指代真正</param>
        /// <param name="currencyid">第三方帳戶幣別 ID (USD)</param>
        /// <param name="amount">存款金額</param>
        /// <param name="refno">第三方的交易編號,不定長字符串(16)</param>
        /// <param name="promoid">促銷 ID,不定長字符串(50)(可選)</param>
        /// <param name="agentid">經紀人 ID 是指代 AMS 系統中某位經紀人的唯一號碼。 整數(4) (可選)</param>
        public hgResponse1 deposit(string username, LoginMode mode, string currencyid, decimal amount, string refno, string promoid, int? agentid)
        {
            hgRequest1 request = new hgRequest1("deposit");
            request["username"] = username;
            request["mode"] = (int)mode;
            request["currencyid"] = currencyid;
            request["amount"] = amount;
            request["refno"] = refno;
            request["promoid"] = promoid;
            request["agentid"] = agentid;
            return request.GetResponse(this.api_url);
        }
        public hgResponse1 deposit(string username, LoginMode mode, CurrencyCode currencyid, decimal amount, string refno, string promoid, int? agentid)
        {
            return this.deposit(username, mode, currencyid.ToString(), amount, refno, promoid, agentid);
        }
        public hgResponse1 deposit(string username, LoginMode mode, string currencyid, decimal amount, string refno)
        {
            return this.deposit(username, mode, currencyid, amount, refno, null, null);
        }

        #endregion

        #region deposit-confirm

        /// <summary>
        /// 存款確認請求消息
        /// </summary>
        /// <param name="status"></param>
        /// <param name="paymentid">遊戲軟件中的支付 ID</param>
        /// <param name="errdesc">針對非零回覆的錯誤消息</param>
        public hgResponse1 deposit_confirm(StatusCode status, string paymentid, string errdesc)
        {
            hgRequest1 request = new hgRequest1("deposit-confirm");
            request["status"] = (int)status;
            request["paymentid"] = paymentid;
            request["errdesc"] = errdesc;
            return request.GetResponse(this.api_url);
        }

        #endregion

        #region withdrawal

        /// <summary>
        /// 取款請求消息
        /// </summary>
        /// <param name="username">第三方帳戶用戶名</param>
        /// <param name="mode">登錄模式。必須是 0 或 1。0 指代娛樂,1 指代真正</param>
        /// <param name="currencyid">第三方帳戶幣別 ID (USD)</param>
        /// <param name="amount">存款金額</param>
        /// <param name="refno">第三方的交易編號,不定長字符串(16)</param>
        /// <param name="agentid">經紀人 ID 是指代 AMS 系統中某位經紀人的唯一號碼。 整數(4)(可選)</param>
        public hgResponse1 withdrawal(string username, LoginMode mode, string currencyid, decimal amount, string refno, int? agentid)
        {
            hgRequest1 request = new hgRequest1("withdrawal");
            request["username"] = username;
            request["mode"] = (int)mode;
            request["currencyid"] = currencyid;
            request["amount"] = amount;
            request["refno"] = refno;
            request["agentid"] = agentid;
            return request.GetResponse(this.api_url);
        }
        //public hgResponse1 withdrawal(string username, LoginMode mode, CurrencyCode currencyid, decimal amount, string refno, int? agentid)
        //{
        //    return this.withdrawal(username, mode, currencyid.ToString(), amount, refno, agentid);
        //}
        //public hgResponse1 withdrawal(string username, LoginMode mode, string currencyid, decimal amount, string refno)
        //{
        //    return this.withdrawal(username, mode, currencyid.ToString(), amount, refno, null);
        //}

        #endregion

        #region withdrawal-confirm

        /// <summary>
        /// 取款確認請求消息
        /// </summary>
        /// <param name="status">0——成功。非零回覆視為金額退還</param>
        /// <param name="paymentid">遊戲軟件中的支付 ID</param>
        /// <param name="errdesc">針對非零回覆的錯誤消息</param>
        public hgResponse1 withdrawal_confirm(StatusCode status, string paymentid, string errdesc)
        {
            hgRequest1 request = new hgRequest1("withdrawal-confirm");
            request["status"] = (int)status;
            request["paymentid"] = paymentid;
            request["errdesc"] = errdesc;
            return request.GetResponse(this.api_url);
        }

        #endregion

        #region logout

        /// <summary>
        /// 客戶註銷消息 (callback)
        /// </summary>
        /// <param name="username">用戶名</param>
        /// <param name="balance">金額(現金)例如 123.33</param>
        /// <param name="currencyid">ISO 3 個字母的代碼,例如 USD</param>
        public hgResponse1 logout(string username, decimal balance, CurrencyCode currencyid)
        {
            hgRequest1 request = new hgRequest1("logout");
            request["username"] = username;
            request["balance"] = balance;
            request["currencyid"] = currencyid;
            return request.GetResponse(this.api_url);
        }

        [ObjectInvoke("logout")]
        static void logout(XmlDocument a, XmlElement b)
        {
        }

        #endregion

        #region accountbalance

        /// <summary>
        /// 帳戶餘額請求消息
        /// </summary>
        /// <param name="username">用戶名</param>
        /// <param name="mode">登錄模式。必須是 0 或 1。0 指代娛樂,1 指代真正</param>
        public hgResponse1 accountbalance(string username, LoginMode mode)
        {
            hgRequest1 request = new hgRequest1("accountbalance");
            request["username"] = username;
            request["mode"] = (int)mode;
            return request.GetResponse(this.api_url);
        }

        #endregion

        #region login-deposit

        /// <summary>
        /// 登錄存款待審核請求消息
        /// </summary>
        /// <param name="username">第三方帳戶用戶名</param>
        /// <param name="firstname">第三方帳戶名</param>
        /// <param name="lastname">第三方帳戶姓</param>
        /// <param name="mode">登錄模式。必須是 0 或 1。0 指代娛樂,1 指代真正</param>
        /// <param name="currencyid">第三方帳戶幣別 ID (USD)</param>
        /// <param name="amount">存款金額</param>
        /// <param name="refno">第三方的交易編號,不定長字符串 (16)</param>
        /// <param name="promoid">促銷 ID ,不定長字符串(50)(可選)</param>
        /// <param name="agentid">經紀人 ID 是指代 AMS 系統中某位經紀人的唯一號碼。 整數(4)(可選)</param>
        /// <param name="affiliateid">用戶的分支機構 ID 不定長字符串(16)(可選)</param>
        /// <param name="testusr">用於創建測試帳戶。必須是 0 或 1。0 代表真正, 1 代表測試 帳戶(可選)</param>
        /// <param name="playerlevel">玩家級。整數 4(可選) 如果傳遞了參數,則將對照該第三方的允許玩家級別進行驗 證。如果沒有傳,將採用默認級別。</param>
        public hgResponse1 login_deposit(string username, string firstname, string lastname, LoginMode mode, string currencyid, decimal amount, string refno, string promoid, int? agentid, string affiliateid, TestUser? testusr, int? playerlevel)
        {
            hgRequest1 request = new hgRequest1("login-deposit");
            request["username"] = username;
            request["firstname"] = firstname;
            request["lastname"] = lastname;
            request["mode"] = (int)mode;
            request["currencyid"] = currencyid;
            request["amount"] = amount;
            request["refno"] = refno;
            request["promoid"] = promoid;
            request["agentid"] = agentid;
            request["affiliateid"] = affiliateid;
            request["testusr"] = (int?)testusr;
            request["playerlevel"] = playerlevel;
            return request.GetResponse(this.api_url);
        }

        #endregion

        #region session

        /// <summary>
        /// 會話請求消息
        /// </summary>
        /// <param name="username">第三方帳戶用戶名</param>
        /// <param name="mode">登錄模式。必須是 0 或 1。0 指代娛樂,1 指代真正</param>
        public hgResponse1 session(string username, LoginMode mode)
        {
            hgRequest1 request = new hgRequest1("session");
            request["username"] = username;
            request["mode"] = mode;
            return request.GetResponse(this.api_url);
        }

        #endregion

        #region blockuser

        /// <summary>
        /// 用戶屏蔽請求消息
        /// </summary>
        /// <param name="username">第三方帳戶用戶名</param>
        public hgResponse1 blockuser(string username)
        {
            hgRequest1 request = new hgRequest1("blockuser");
            request["username"] = username;
            return request.GetResponse(this.api_url);
        }

        #endregion

        #region winlimit

        /// <summary>
        /// 用戶贏錢限額設置請求消息
        /// </summary>
        /// <param name="username">第三方帳戶用戶名</param>
        /// <param name="amount">贏錢限額金額</param>
        /// <param name="forcereset">強制性重置該用戶的贏錢限額設置 “1”強制性重置,“0”正常請求</param>
        public hgResponse1 winlimit(string username, decimal amount, ForceReset forcereset)
        {
            hgRequest1 request = new hgRequest1("winlimit");
            request["username"] = username;
            request["amount"] = amount;
            request["forcereset"] = forcereset;
            return request.GetResponse(this.api_url);
        }

        #endregion

        #region GetAllbetdetails()

        /// <summary>
        /// This Webservice will fetch bet details for different casinos of current date. The frequency of the call should be made to this API after every 5 minutes otherwise it will throw an error.
        /// </summary>
        /// <param name="UserId">This is an optional parameter.
        /// Email address or Account Id</param>
        /// <param name="Dateval">This is an optional parameter.
        /// If user passes this Date, it will return bet details of the given date or Current date. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011or 2011/01/12</param>
        /// <param name="TimeRange">This is an optional parameter.
        /// If user passes time from 0 to 24. Time difference should be minimum of 1 hour and maximum of 4 hours</param>
        /// <param name="Status">This is an optional parameter. 
        /// If the status is not used in the request it will return bet details of the successful games and if a status like “Cancel” is used as an input parameter this will return only cancelled game bet details.</param>
        public static hgResponse2 GetAllbetdetails(string UserId, DateTime? Dateval, int? TimeRange, string Status)
        {
            hgRequest2 request = new hgRequest2("GetBetdetails");
            request["UserId"] = UserId;
            request["Dateval", "yyyy/MM/dd"] = Dateval;
            request["TimeRange"] = TimeRange;
            request["Status"] = Status;
            hgResponse2 res = request.GetResponse("GetAllbetdetails");
            res.DocumentElement.SetAttribute("src", MethodBase.GetCurrentMethod().Name);
            return res;
        }

        #endregion

        #region GetAllBetDetailsfor30seconds()
        /// <summary>
        /// This webservice will fetch bet details of different casinos for last 30 seconds. The frequency of the call should be made to this API after every 30 seconds otherwise it will throw an error.
        /// </summary>

        /// <summary>
        /// The following xml must be posted back to the webservice once the user receives the above response from the webservice.
        /// </summary>
        /// <param name="BatchId">Batch ID which was received in the response XML structure</param>
        public static hgResponse2 GetAllBetDetailsfor30seconds(int? BatchId)
        {
            hgRequest2 request = new hgRequest2("GetBetdetails");
            request["BatchId"] = BatchId;
            hgResponse2 res = request.GetResponse("GetAllBetDetailsfor30seconds");
            res.DocumentElement.SetAttribute("src", MethodBase.GetCurrentMethod().Name);
            return res;
        }
        #endregion

        #region GetBetDetailsByAffiliate()

        /// <summary>
        /// This Webservice will fetch the bet details of different casinos either for all or specific affiliates and this API can be called at any time without having any restrictions.
        /// </summary>
        /// <param name="CasinoId">ID</param>
        /// <param name="Dateval">This is optional parameter.
        /// If user passes this Date, it will return bet details of the given date or Current date.</param>
        /// <param name="AffiliateId">This is optional parameter.
        /// If user passes this value, it will return the bet details for a particular affiliates</param>
        public static hgResponse2 GetBetDetailsByAffiliate(DateTime? Dateval, string AffiliateId)
        {
            hgRequest2 request = new hgRequest2("GetBetdetails");
            request["Dateval", "yyyy/MM/dd"] = Dateval;
            request["AffiliateId"] = AffiliateId;
            return request.GetResponse("GetBetDetailsByAffiliate");
        }

        #endregion

        #region GetTableList()
        /// <summary>
        /// This webservice will fetch the available table details of different casinos and this API can be called at any time without having any restrictions.
        /// </summary>
        public static hgResponse2 GetTableList()
        {
            hgRequest2 request = new hgRequest2("Gettabledetails");
            return request.GetResponse("GetTableList");
        }

        #endregion

        #region GetPlayerDetails()
        /// <summary>
        /// This webservice will fetch Player details of different casinos and this API can be called at any time without having any restrictions.日期间隔不超过7天
        /// </summary>
        /// <param name="AccountID">Provide the Player AccountID (username)</param>
        /// <param name="StartDate">start date. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011 or 2011/01/12</param>
        /// <param name="Enddate">End Date. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011 or 2011/01/12</param>
        public static hgResponse2 GetPlayerDetails(string AccountID, DateTime StartDate, DateTime Enddate)
        {
            hgRequest2 request = new hgRequest2("GetPlayerdetails");
            request["AccountID"] = AccountID;
            request["StartDate", "yyyy/MM/dd"] = StartDate;
            request["Enddate", "yyyy/MM/dd"] = Enddate;
            hgResponse2 res = request.GetResponse("GetPlayerDetails");
            if (res != null)
            {
                res.DocumentElement.SetAttribute("StartDate", StartDate.ToString("yyyy/MM/dd"));
                res.DocumentElement.SetAttribute("Enddate", StartDate.ToString("yyyy/MM/dd"));
            }
            return res;
        }
        #endregion

        #region GetAgentPlayerDetails()
        /// <summary>
        /// This webservice will fetch Player details of different Agents and this API can be called at any time without having any restrictions. This API must be used by AMS (Agent Management System) users.
        /// </summary>
        /// <param name="DateVal">Single Date. , it will return bet details of the given date.</param>
        public static hgResponse2 GetAgentPlayerDetails(DateTime DateVal)
        {
            hgRequest2 request = new hgRequest2("GetAgentPlayerdetails");
            request["DateVal", "yyyy/MM/dd"] = DateVal;
            return request.GetResponse("GetAgentPlayerDetails");
        }
        #endregion

        #region GetEventbetsAgentPlayerDetails()
        /// <summary>
        /// This webservice will fetch Player details of particular Agents (single shop) and this API can be called at any time without having any restrictions. This API must be used by the AMS (Agent Management System) users.
        /// </summary>
        /// <param name="DateVal">Single Date. , it will return bet details of the given date. (MM/dd/yyyy or yyyy/mm/dd)</param>
        /// <param name="AgentName">This is an optional parameter. If you pass the Agent name, it will return the bet details for a particular Agents player otherwise it will return all player details from all the agents.</param>
        public static hgResponse2 GetEventbetsAgentPlayerDetails(DateTime DateVal, string AgentName)
        {
            hgRequest2 request = new hgRequest2("GetAgentPlayerdetails");
            request["DateVal", "yyyy/MM/dd"] = DateVal;
            request["AgentName"] = AgentName;
            return request.GetResponse("GetEventbetsAgentPlayerDetails");
        }
        #endregion

        #region GetAgentsEvenbetsDetails()
        /// <summary>
        /// This webservice will fetch the all agent even bet details for particular casino and this API can be called at any time without having any restrictions. This API must be used by the AMS (Agent Management System) users.
        /// </summary>
        /// <param name="DateVal">Single Date. , it will return bet details of the given date. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011 or 2011/01/12</param>
        public static hgResponse2 GetAgentsEvenbetsDetails(DateTime DateVal)
        {
            hgRequest2 request = new hgRequest2("GetAgentdetails");
            request["DateVal", "yyyy/MM/dd"] = DateVal;
            return request.GetResponse("GetAgentsEvenbetsDetails");
        }
        #endregion

        #region GetAllBetDetailsPerTimeInterval()
        /// <summary>
        /// This web service will fetch the bet details of given dates & time for casinos and This API can be called at any time without having any restrictions.
        /// </summary>
        /// <param name="UserId">This is an optional parameter. Email address or Account Id</param>
        /// <param name="startTime">It used to passes this start Date, Time to fetch the bet details. Format MM/dd/yyyy hh:mm:ss or yyyy/ mm/dd hh:mm:ss Ex: 01/12/2011 03:59:59 or 2011/01/12 03:59:59</param>
        /// <param name="EndTime">It used to passes this End Date, Time to fetch the bet details. Format MM/dd/yyyy hh:mm:ss or yyyy/ mm/dd hh:mm:ss Ex: 01/12/2011 03:59:59 or 2011/01/12 03:59:59</param>
        /// <param name="PageSize">This is an optional parameter. It specifies the Page size will return. Ex :20</param>
        /// <param name="PageNumber">This is an optional parameter. It Specifies the Page Number which will return the records from the given page Number. Ex : 5</param>
        /// <param name="Status">This is an optional parameter.
        /// If the status is not used in the request it will return bet details of the successful games and if a status like “Cancel” is used as an input parameter this will return only cancelled game bet details.</param>
        public static hgResponse2 GetAllBetDetailsPerTimeInterval(string UserId, DateTime startTime, DateTime EndTime, int? PageSize, int? PageNumber)
        {
            hgRequest2 request = new hgRequest2("GetAllBetDetailsPerTimeInterval");
            request["UserId"] = UserId;
            request["StartTime", "yyyy/MM/dd HH:mm:ss"] = startTime;
            request["EndTime", "yyyy/MM/dd HH:mm:ss"] = EndTime;
            request["PageSize"] = PageSize;
            request["PageNumber"] = PageNumber;
            hgResponse2 res = request.GetResponse("GetAllBetDetailsPerTimeInterval");
            res.DocumentElement.SetAttribute("tablename", "hg_Betinfo1");
            return res;
        }
        #endregion

        #region GetAllFundTransferDetailsTimeInterval()
        /// <summary>
        /// This web service will fetch the Fund Transfer details of given date & time for casino and this API can be called at any time without having any restrictions.
        /// </summary>
        /// <param name="UserId">This is an optional parameter. Email address or Account Id</param>
        /// <param name="startTime">It used to passes this start Date, Time to fetch the bet details. Format MM/dd/yyyy hh:mm:ss or yyyy/ mm/dd hh:mm:ss Ex: 01/12/2011 03:59:59 or 2011/01/12 03:59:59</param>
        /// <param name="EndTime">It used to passes this End Date, Time to fetch the bet details. Format MM/dd/yyyy hh:mm:ss or yyyy/ mm/dd hh:mm:ss Ex: 01/12/2011 03:59:59 or 2011/01/12 03:59:59</param>
        /// <param name="PageSize">This is an optional parameter. It specifies the Page size will return. Ex :20</param>
        /// <param name="PageNumber">This is an optional parameter. It Specifies the Page Number which will return the records from the given page Number. Ex : 5</param>
        public static hgResponse2 GetAllFundTransferDetailsTimeInterval(string UserId, DateTime startTime, DateTime EndTime, int? PageSize, int? PageNumber)
        {
            hgRequest2 request = new hgRequest2("GetAllFundTransferDetailsPerTimeInterval");
            request["UserId"] = UserId;
            request["StartTime", "yyyy/MM/dd HH:mm:ss"] = startTime;
            request["EndTime", "yyyy/MM/dd HH:mm:ss"] = EndTime;
            request["PageSize"] = PageSize;
            request["PageNumber"] = PageNumber;
            return request.GetResponse("GetAllFundTransferDetailsTimeInterval");
        }
        #endregion

        #region GetGameResultInfo()
        /// <summary>
        /// This web service will fetch the Game Result details of given date & time for casino and this API can be called at any time without having any restrictions.
        /// </summary>
        /// <param name="UserId">This is an optional parameter. Email address or Account Id</param>
        /// <param name="startTime">It used to passes this start Date, Time to fetch the bet details. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011 or or 2011/01/12</param>
        /// <param name="EndTime">It used to passes this End Date, Time to fetch the bet details. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011 or or 2011/01/12</param>
        /// <param name="PageSize">This is an optional parameter. It specifies the Page size will return. Ex :20</param>
        /// <param name="PageNumber">This is an optional parameter. It Specifies the Page Number which will return the records from the given page Number. Ex : 5</param>
        public static hgResponse2 GetGameResultInfo(string UserId, DateTime startTime, DateTime EndTime, int? PageSize, int? PageNumber)
        {
            hgRequest2 request = new hgRequest2("GameResultInfo");
            request["UserId"] = UserId;
            request["startTime", "yyyy/MM/dd HH:mm:ss"] = startTime;
            request["EndTime", "yyyy/MM/dd HH:mm:ss"] = EndTime;
            request["PageSize"] = PageSize;
            request["PageNumber"] = PageNumber;
            return request.GetResponse("GetGameResultInfo");
        }
        #endregion

        #region GetPlayerBetAmount()
        /// <summary>
        /// This web service will fetch the live game stake amount and live game exclude even and tie bet amount for casino and this API can be called at any time without having any restrictions.
        /// </summary>
        /// <param name="UserId">This is an optional parameter. Email address or Account Id</param>
        /// <param name="DateVal">This is an optional parameter. If user passes this Date, it will return bet details of the given date or Current date. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011 or or 2011/01/12</param>
        /// <param name="TimeRange">This is an optional parameter. If user passes time from 0 to 24.</param>
        public static hgResponse2 GetPlayerBetAmount(string UserId, DateTime? DateVal, int? TimeRange)
        {
            hgRequest2 request = new hgRequest2("PlayerBetInfo");
            request["UserId"] = UserId;
            request["DateVal", "yyyy/MM/dd"] = DateVal;
            //request["DateVal"] = DateVal;
            request["TimeRange"] = TimeRange;
            hgResponse2 res = request.GetResponse("GetPlayerBetAmount");
            if (res != null)
            {
                if (DateVal.HasValue) res.DocumentElement.SetAttribute("dateval", DateVal.Value.ToString("yyyy/MM/dd"));
                if (TimeRange.HasValue) res.DocumentElement.SetAttribute("timerange", TimeRange.Value.ToString());
            }
            return res;
        }
        #endregion

        #region GetBonusInfo()
        /// <summary>
        /// This web service will get the player bonus details from the bonus accepted date. This API can be called at any time without having any restrictions.
        /// </summary>
        /// <param name="UserId">This is an optional parameter. Email address or Account Id</param>
        /// <param name="StartTime">If user passes this Date.(Player Bonus accept date). Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011 or or 2011/01/12</param>
        /// <param name="EndTime">If user passes this Date.(Player Bonus accept date). Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011 or or 2011/01/12</param>
        /// <param name="BonusName">This is an optional parameter. Name of the Bonus</param>
        public static hgResponse2 GetBonusInfo(string UserId, DateTime StartTime, DateTime EndTime, string BonusName)
        {
            hgRequest2 request = new hgRequest2("BonusPT");
            request["UserId"] = UserId;
            request["StartTime", "yyyy/MM/dd"] = StartTime;
            request["EndTime", "yyyy/MM/dd"] = EndTime;
            request["BonusName"] = BonusName;//List<BonusInfo>
            return request.GetResponse("GetBonusInfo");
        }
        #endregion

        #region Getbetdetails_ExcludeTieandEven()
        /// <summary>
        /// This Webservice will fetch bet details for different casinos for a date. Bet details of even bet spot and tie result bets will not be fetched.
        /// </summary>
        /// <param name="UserId">This is an optional parameter. Email address or Account Id</param>
        /// <param name="Dateval">This is an optional parameter. If user passes this Date, it will return bet details of the given date or Current date. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011or 2011/01/12</param>
        /// <param name="TimeRange">This is an optional parameter. If user passes time from 0 to 24. Time difference should be minimum of 1 hour and maximum of 4 hours</param>
        public static hgResponse2 Getbetdetails_ExcludeTieAndEven(string UserId, DateTime? Dateval, int? TimeRange)
        {
            hgRequest2 request = new hgRequest2("GetBetdetails");
            request["UserId"] = UserId;
            request["Dateval", "yyyy/MM/dd"] = Dateval;
            //request["Dateval"] = Dateval;
            if (TimeRange.HasValue)
                request["TimeRange"] = (int)Math.Min(Math.Max(TimeRange.Value, 1), 4);
            hgResponse2 res = request.GetResponse("Getbetdetails_ExcludeTieAndEven");
            res.DocumentElement.SetAttribute("tablename", "hg_Betinfo2");
            return res;
        }
        #endregion


        //static hgResponse invoke(string url, hgRequest request)
        //{
        //    request.Http = (HttpWebRequest)WebRequest.Create(url);
        //    request.Http.Method = "POST";
        //    request.Http.ContentType = "text/xml";
        //    request.Write(request.Http.GetRequestStream());

        //    hgResponse response = new hgResponse();
        //    response.Http = (HttpWebResponse)request.Http.GetResponse();
        //    response.Parse(response.Http.GetResponseStream());
        //    return response;
        //}
    }
}
 