using BU;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using web;

namespace extAPI.hg
{
    public enum LoginMode : int
    {
        娛樂 = 0, 真正 = 1,
    }

    public enum SessionStatus : int
    {
        會話不可用 = 0, 會話可用 = 1,
    }

    public enum ForceReset : int
    {
        正常請求 = 0, 強制性重置 = 1,
    }

    public enum TestUser
    {
        真正 = 0, 測試 = 1,
    }

    public enum StatusCode : int
    {
        SUCCESS = 0,                                        // 成功
        ERR_DEP_LOAD_REQ = 101,                             // 存款請求報錯
        ERR_DEP_INVALID_REQ = 102,                          // 存款請求報錯– 模式不合法
        ERR_DEP_INVALID_ACTION = 103,                       // 存款請求報錯– 操作不合法
        AGENTID_INSUFFICIENT_BALANCE = 104,                 // 存款請求報錯–經紀人餘額不足
        AGENTID_REQUIRED = 105,                             // 請求報錯 –必輸字段經紀人 ID 缺失
        AMS_DISABLED = 106,                                 // 請求報錯–EGame AMS 未啓用。請聯繫管理員啓用基於 AMS 的轉賬。
        ERROR107 = 107,                                     // 幣別和玩家級彆不合法。請聯繫管理員。
        ERROR108 = 108,                                     // 玩家級彆不合法。請聯繫管理員。
        ERR_DEP_PEND_PARA = 111,                            // 存款待審核請求參數報錯
        ERR_DEP_PEND_EMPTY_PARA = 112,                      // 存款待審核請求參數報錯– 空值
        ERR_SL_CODE = 113,                                  // 存款金額錯誤
        ERR_DEP_AMOUNT = 114,                               // 存款報錯 –用戶不合法
        ERR_DEP_PEND_NUM_PARA = 115,                        // 存款報錯– 服務器錯誤
        ERR_DEP_USER = 116,                                 // 錯誤,非通用接口用戶
        ERR_DEP_BLACK_LIST = 117,                           // 存款確認狀態碼錯誤
        ERR_DEP_PEND_OTHER = 118,                           // 存款確認報錯 –已取消
        ERR_DEP_PEND_UNKNOWN = 119,                         // 錯誤,字段不合法
        ERR_NOT_GENRIC_INTERFACE_USER = 120,                // 數據庫錯誤
        ERR_DEP_CONFIRM_PARA = 121,                         // 帳戶被屏蔽
        ERR_DEP_CONFIRM_STATUS = 122,                       // 錯誤 –帳戶 ID 不合法
        ERR_DEP_CONFIRM_CANCEL = 123,                       // 錯誤 –金額不合法
        ERR_DEP_CONFIRM_TRAN = 124,                         // 錯誤 –資金不足
        ERROR_INVALID_FIELD = 201,                          // 錯誤 –幣彆不合法
        ERROR_DUPLICATE_ACCOUNT_ID = 202,                   // 錯誤 –支付 ID 不合法
        ERROR_DATABASE_ERROR = 203,                         // 錯誤 –支付 ID 非待審核
        ERROR_ACCOUNT_SUSPENDED = 204,                      // 錯誤 – 重複登錄
        ERROR_INVALID_ACCOUNT_ID = 205,                     // 錯誤 – 重複編號
        ERROR_INVALID_AMOUNT = 206,                         // 錯誤 – 密碼不符
        ERROR_INSUFFICIENT_FUNDS = 207,                     // 促銷 ID 不合法
        ERROR_INVALID_CURRENCY = 208,                       // 該用戶已經分配了促銷 ID
        ERROR_INVALID_PAYMENT_ID = 209,                     // 錯誤 –取款確認狀態碼 ID
        ERROR_PAYMENT_NOT_PENDING = 210,                    // 錯誤 –取款確認狀態碼 ID
        ERROR_MULTIPLE_LOGIN = 211,                         // 存款請求報錯
        DUPLICATE_REF_ID = 212,                             // 錯誤,取款模式請求不合法
        ERROR_INVALID_PASSWORD = 213,                       // 錯誤 – 取款請求中未發送要求的參數
        ERR_DEP_CONFIRM_INIT = 300,                         // 錯誤 - 取款請求中參數值為空
        ERR_DEP_CONFIRM_UNKNOWN = 301,                      // 取款請求中金額錯誤
        ERR_DEP_CONFIRM_OTHER = 302,                        // 未知錯誤 –服務器側
        ERR_DEP_CONFIRM_DUPLICATE = 303,                    // 取款和獲取餘額請求中用戶錯誤
        ERR_DEP_CONFIRM_DEPOSIT = 310,                      // 取款請求中用戶餘額錯誤
        ERR_DEP_CONFIRM_USER = 311,
        ERR_DEP_CONFIRM_CASHFLOW = 321,
        ERR_DEP_CONFIRM_ACCOUNT = 322,
        INVALID_PROMO_ID = 323,
        PROMO_ID_ALREADY_GIVEN = 324,
        ERR_WITHDRAW_CONFIRM_STATUS = 422,
        ERR_WITHDRAW_CONFIRM_CANCEL = 423,
        ERR_WITH_LOAD_REQ = 501,
        ERR_WITH_INVALID_REQ = 502,
        ERR_WITH_INVALID_ACTION = 503,
        ERR_WITH_PARA = 511,
        ERR_WITH_EMPTY_PARA = 512,
        ERR_WITH_NUM_PARA = 513,
        ERR_SL_CODE_514 = 514,
        ERR_WITH_AMOUNT = 515,
        ERR_WITH_INIT = 600,
        ERR_WITH_UNKNOWN = 601,
        ERR_WITH_OTHER = 602,
        ERR_WITH_USER = 608,
        ERR_BETTING = 610,
        ERR_WITH_USER_BAL = 611,
        ERR_WITH_ACCOUNT = 622,
        ERR_WITH_INSERT = 623,
        EXCEDDED_MAX_NO_TRANSACTION = 700,
        LESS_THAN_MIN_TRANS_AMOUNT = 701,
        MORE_THAN_MAX_TRANS_AMOUNT = 702,
        EXCEDDED_MAX_AMOUNT_TRANSACTION = 703,
        ERR704 = 704,                                       // 錯誤 – 重複編號 (Invalid agent ID.Account is not created)
        ERR_EMAIL_NULL = 705,
        ERR_EMAIL_NOTVALID = 706,
        ERR_WINLIMIT_REACHED = 801,
    }

    public class hgRequest1
    {
        public static string log_prefix = "hg";
        StringBuilder sb;
        StringWriter sw;
        XmlTextWriter xml;

        public string Action { get; private set; }

        public hgRequest1(string action)
        {
            sb = new StringBuilder();
            sw = new EncodingStringWriter(sb);
            xml = new XmlTextWriter(sw);
            xml.WriteStartDocument();
            xml.WriteStartElement("request");
            xml.WriteAttributeString("action", this.Action = action);
            xml.WriteStartElement("element");
        }

        public object this[string name]
        {
            set
            {
                if (value == null) return;
                xml.WriteStartElement("properties");
                xml.WriteAttributeString("name", name);
                xml.WriteValue(Convert.ToString(value));
                xml.WriteEndElement();
                //this[name, false] = value;
            }
        }

        //public object this[string name, bool _addTag]
        //{
        //    set
        //    {
        //        if (value == null) return;
        //        xml.WriteStartElement("properties");
        //        xml.WriteAttributeString("name", name);
        //        xml.WriteValue(Convert.ToString(value));
        //        xml.WriteEndElement();
        //    }
        //}

        //void setValue(string name, object value)
        //{
        //    if (value == null) return;
        //    xml.WriteStartElement("properties");
        //    xml.WriteAttributeString("name", name);
        //    xml.WriteValue(Convert.ToString(value));
        //    xml.WriteEndElement();
        //}

        public hgResponse1 GetResponse(string api_url)
        {
            using (this.sw) using (this.xml) { }
            return hgResponse1.Parse(util.GetResponse(api_url, sb.ToString(), hgRequest1.log_prefix));
            //xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
        }
    }

    public class hgResponse1 : Dictionary<string, string>
    {
        public string Root;
        public string Action;
        public string xml;
        public string error
        {
            get { string s; this.TryGetValue("error", out s); return s; }
        }

        public static hgResponse1 Parse(string xml)
        {
            hgResponse1 result = null;
            xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
            using (StringReader sr = new StringReader(xml))
            using (XmlTextReader r = new XmlTextReader(sr))
            {
                while (r.Read())
                {
                    if (r.NodeType == XmlNodeType.Element)
                    {
                        if (r.Depth == 0)
                        {
                            result = new hgResponse1();
                            result.xml = xml;
                            result.Root = r.Name;
                            for (bool n = r.MoveToFirstAttribute(); n; n = r.MoveToNextAttribute())
                            {
                                if (r.Name.Equals("action", StringComparison.OrdinalIgnoreCase))
                                {
                                    result.Action = r.Value;
                                    break;
                                }
                            }
                        }
                        else if ((r.Depth == 1) && r.Name.Equals("error", StringComparison.OrdinalIgnoreCase))
                        {
                            if (result == null) return null;
                            result["error"] = r.ReadElementContentAsString();
                        }
                        else if ((r.Depth == 1) && r.Name.Equals("element", StringComparison.OrdinalIgnoreCase))
                        {
                            if (result == null) return null;
                        }
                        else if ((r.Depth == 2) && r.Name.Equals("properties"))
                        {
                            if (result == null) return null;
                            for (bool n = r.MoveToFirstAttribute(); n; n = r.MoveToNextAttribute())
                            {
                                if (r.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (string.IsNullOrEmpty(r.Value)) continue;
                                    result[r.Value.ToLower()] = r.ReadString();
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public string username
        {
            get { string s; this.TryGetValue("username", out s); return s; }
        }

        public string ticket
        {
            get { string s; this.TryGetValue("ticket", out s); return s; }
        }

        public StatusCode? status
        {
            get { string s; this.TryGetValue("status", out s); return s.ToEnum<StatusCode>(); }
        }

        public string errdesc
        {
            get { string s; this.TryGetValue("errdesc", out s); return s; }
        }

        public string refno
        {
            get { string s; this.TryGetValue("refno", out s); return s; }
        }

        public string paymentid
        {
            get { string s; this.TryGetValue("paymentid", out s); return s; }
        }

        public decimal? balance
        {
            get { string s; this.TryGetValue("balance", out s); return s.ToDecimal(); }
        }

        public CurrencyCode? currencyid
        {
            get { string s; this.TryGetValue("currencyid", out s); return s.ToEnum<CurrencyCode>(); }
        }

        public string session_status
        {
            get { string s; this.TryGetValue("session-status", out s); return s; }
        }

        public string session_start
        {
            get { string s; this.TryGetValue("session-start", out s); return s; }
        }
    }

    public class hgRequest2 
    {
        StringBuilder sb;
        StringWriter sw;
        XmlTextWriter xml;

        public hgRequest2(string root)
        {
            sb = new StringBuilder();
            sw = new EncodingStringWriter(sb);
            xml = new XmlTextWriter(sw);
            xml.WriteStartDocument();
            xml.WriteStartElement(root);

            this["Username"] = api.username;
            this["Password"] = api.password;
            this["CasinoId"] = api.casinoid;
        }

        public object this[string name]
        {
            set
            {
                if (value == null || value.ToString() == "") return;
                xml.WriteStartElement(name);
                //if (value is DateTime)
                //    value = ((DateTime)value).AddHours(-8);
                xml.WriteValue(util.EnumToValue(value));
                xml.WriteEndElement();
            }
        }
        public DateTime? this[string name, string format]
        {
            set
            {
                if (value.HasValue)
                    this[name] = value.Value.ToString(format);
            }
        }

        public hgResponse2 GetResponse(string soap_method)
        {
            using (this.sw) using (this.xml) { }
            hgResponse2 res = hgResponse2.Parse(util.GetResponse(string.Format("{0}/{1}", api.betapi_url, soap_method), sb.ToString(), hgRequest1.log_prefix));
            return res;
        }

        public string _GetResponse(string soap_method)
        {
            using (this.sw) using (this.xml) { }
            return util.GetResponse(string.Format("{0}/{1}", api.betapi_url, soap_method), sb.ToString(), hgRequest1.log_prefix);
        }
    }

    public class hgResponse2 : XmlDocument
    {
        public static hgResponse2 Parse(string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                using (XmlTextReader xr = new XmlTextReader(sr))
                {
                    while (xr.Read())
                    {
                        if (xr.IsTextNode(1))
                        {
                            hgResponse2 res = new hgResponse2();
                            XmlElement root = res.CreateElement("xml");
                            root.InnerXml = xr.Value;
                            //res.AppendChild(root.SelectSingleNode("/node()[not(self::totalrecords)]"));
                            XmlNode totalrecords = root.SelectSingleNode("/totalrecords");
                            if (totalrecords != null)
                                root.RemoveChild(totalrecords);
                            if (root.ChildNodes.Count > 0)
                            {
                                res.AppendChild(root.ChildNodes[0]);
                                if (totalrecords != null)
                                    res.DocumentElement.SetAttribute("totalrecords", totalrecords.InnerText);
                            }
                            //res.parse_data();
                            return res;
                        }
                    }
                }
            }
            catch
            {
            }
            //log.message(hgRequest1.log_prefix, "Response:\t{0}", xml);
            return null;
        }

        static void fill(sqltool s, SqlSchemaTable schema, string name, XmlElement e1, XmlElement e2)
        {
            if (name == "sn") return;
            if (schema.ContainsKey(name))
            {
                Type type = schema[name].Type;
                object value ;
                if (type == typeof(DateTime))
                {
                    DateTime time;
                    if (DateTime.TryParse(e2.InnerText, out time))
                    { /*time = time.AddHours(8);*/ }
                    else if (DateTime.TryParseExact(e2.InnerText, "yyyyMMdd HH:mm:ss", null, System.Globalization.DateTimeStyles.AdjustToUniversal, out time))
                    { /*time = time.AddHours(8);*/ }
                    value = time;
                }
                else
                    value = Convert.ChangeType(e2.InnerText, type);
                s["", schema[name].Name, ""] = value;
            }
            else
                log.message("Warning", "Field not defined : {0}\r\n{1}", e2.OuterXml, e1.OuterXml);
        }
        static void write(SqlCmd sqlcmd, string sql)
        {
            try
            {
                sqlcmd.BeginTransaction();
                sqlcmd.ExecuteNonQuery(sql);
                sqlcmd.Commit();
            }
            catch (Exception ex)
            {
                sqlcmd.Rollback();
                log.error(ex);
            }
        }
        public void parse_data(SqlCmd sqlcmd)
        {
            SqlSchemaTable schema = null;
            switch (this.DocumentElement.Name.ToLower())
            {
                #region gameinfo
                case "gameinfo":
                    {
                        foreach (XmlElement e1 in this.SelectNodes("//gameinfo/*"))
                        {
                            schema = schema ?? SqlSchemaTable.GetSchema(sqlcmd, "select top(1) * from hg_GameResult nolock");
                            sqltool s = new sqltool();
                            s["", "GameType", ""] = e1.Name;
                            foreach (XmlElement e2 in e1.SelectNodes("./*"))
                            {
                                string name = e2.Name.ToLower();
                                if (name == "id") continue;
                                if (name == "gametype") continue;
                                fill(s, schema, name, e1, e2);
                            }
                            write(sqlcmd, s.BuildEx("insert into hg_GameResult (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                        }
                    }
                    break;
                #endregion
                #region playerinfo
                case "playerinfo":
                    {
                        XmlAttribute startdate = this.DocumentElement.Attributes["startdate"];
                        XmlAttribute enddate = this.DocumentElement.Attributes["enddate"];
                        foreach (XmlElement e1 in this.SelectNodes("//playerinfo/playerdetails"))
                        {
                            if (startdate != null) e1.AppendChild(this.CreateElement(startdate.Name)).InnerText = startdate.Value;
                            if (enddate != null) e1.AppendChild(this.CreateElement(enddate.Name)).InnerText = enddate.Value;
                            schema = schema ?? SqlSchemaTable.GetSchema(sqlcmd, "select top(1) * from hg_PlayerDetails nolock");
                            sqltool s = new sqltool();
                            foreach (XmlElement e2 in e1.SelectNodes("./*"))
                                fill(s, schema, e2.Name.ToLower(), e1, e2);
                            write(sqlcmd, s.BuildEx("insert into hg_PlayerDetails (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                        }
                    }
                    break;
                #endregion
                #region betinfos
                case "betinfos":
                    {
                        XmlAttribute tablename = this.DocumentElement.Attributes["tablename"];
                        if (tablename == null) break;
                        foreach (XmlElement e1 in this.SelectNodes("//betinfos/betinfo"))
                        {
                            //if (tablename != null) e1.AppendChild(this.CreateElement(tablename.Name)).InnerText = tablename.Value;
                            //renameXmlElement(e1.SelectSingleNode("./table_id") as XmlElement, "TableId");
                            //renameXmlElement(e1.SelectSingleNode("./user_id") as XmlElement, "userid");
                            renameXmlElement(e1.SelectSingleNode("./asgametype") as XmlElement, "GameType");
                            schema = schema ?? SqlSchemaTable.GetSchema(sqlcmd, string.Format("select top(1) * from {0} nolock", tablename.Value));
                            if (schema.ContainsKey("ID"))
                                schema.Remove("ID");
                            sqltool s = new sqltool();
                            foreach (XmlElement e2 in e1.SelectNodes("./*"))
                                fill(s, schema, e2.Name.ToLower(), e1, e2);
                            write(sqlcmd, s.BuildEx("insert into ", tablename.Value, " (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                        }
                    }
                    break;
                #endregion
                #region fundtransferinfos
                case "fundtransferinfos":
                    {
                        foreach (XmlElement e1 in this.SelectNodes("//fundtransferinfos/getallaccounttransferdetails"))
                        {
                            schema = schema ?? SqlSchemaTable.GetSchema(sqlcmd, "select top(1) * from hg_TransferDetails nolock");
                            sqltool s = new sqltool();
                            foreach (XmlElement e2 in e1.SelectNodes("./*"))
                                fill(s, schema, e2.Name.ToLower(), e1, e2);
                            write(sqlcmd, s.BuildEx("insert into hg_TransferDetails (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                        }
                    }
                    break;
                #endregion
                #region playerbetinfo
                case "playerbetinfo":
                    {
                        XmlAttribute dateval = this.DocumentElement.Attributes["dateval"];
                        XmlAttribute timerange = this.DocumentElement.Attributes["timerange"];
                        foreach (XmlElement e1 in this.SelectNodes("//playerbetinfo/playerbetdetails"))
                        {
                            if (dateval != null) e1.AppendChild(this.CreateElement(dateval.Name)).InnerText = dateval.Value;
                            if (timerange != null) e1.AppendChild(this.CreateElement(timerange.Name)).InnerText = timerange.Value;
                            schema = schema ?? SqlSchemaTable.GetSchema(sqlcmd, "select top(1) * from hg_PlayerBetDetails nolock");
                            sqltool s = new sqltool();
                            foreach (XmlElement e2 in e1.SelectNodes("./*"))
                                fill(s, schema, e2.Name.ToLower(), e1, e2);
                            write(sqlcmd, s.BuildEx("insert into hg_PlayerBetDetails (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                        }
                    }
                    break;
                #endregion
                case "playinfos": break;
                default: log.message("Warning", "Tag not defined : {0}", this.OuterXml); break;
            }
        }

        static void renameXmlElement(XmlElement e, string name)
        {
            if (e == null) return;
            e.ParentNode.AppendChild(e.OwnerDocument.CreateElement(name)).InnerText = e.InnerText;
            e.ParentNode.RemoveChild(e);
        }

        public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
        {
            return base.CreateElement(prefix, localName.ToLower(), namespaceURI);
        }
        public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
        {
            return base.CreateAttribute(prefix, localName.ToLower(), namespaceURI);
        }

        public int? STATUS_CODE
        {
            get { return this.SelectSingleNode("/*/status_code").GetInnerText().ToInt32(); }
        }
        public string STATUS_DESC
        {
            get { return this.SelectSingleNode("/*/status_desc").GetInnerText(); }
        }
        public int? TotalRecords
        {
            get { return this.SelectSingleNode("//@totalrecords").GetValue().ToInt32(); }
            //get { return this.SelectSingleNode("/*/totalrecords").GetInnerText().ToInt32(); }
        }
    }

    //class _hgRequest2 : apiRequest
    //{
    //    public _hgRequest2(string root)
    //    {
    //        xml.WriteStartDocument();
    //        xml.WriteStartElement(root);

    //        this["Username"] = api.username;
    //        this["Password"] = api.password;
    //        this["CasinoId"] = api.casinoid;
    //    }

    //    public object this[string name]
    //    {
    //        set
    //        {
    //            if (value == null || value.ToString() == "") return;
    //            xml.WriteStartElement(name);
    //            xml.WriteValue(util.EnumToValue(value));
    //            xml.WriteEndElement();
    //        }
    //    }

    //    public object this[string name, string format]
    //    {
    //        set
    //        {
    //            if (value == null || value.ToString() == "") return;
    //            xml.WriteStartElement(name);
    //            xml.WriteValue(DateTime.Parse(value.ToString()).ToString(format));
    //            xml.WriteEndElement();
    //        }
    //    }

    //    public override string GetResponse(string url)
    //    {
    //        string s = base.GetResponse(url);
    //        using (StringReader sr = new StringReader(s))
    //        using (XmlTextReader xr = new XmlTextReader(sr))
    //        {
    //            while (xr.Read())
    //            {
    //                if ((xr.Depth == 1) && (xr.NodeType == XmlNodeType.Text))
    //                {
    //                    return xr.Value;
    //                }
    //            }
    //        }
    //        return s;
    //    }

    //    public string GetResponse(string url, string method)
    //    {
    //        return this.GetResponse(string.Format("{0}/{1}", url, method));
    //    }
    //}

    //class BetInfos : List<Betinfo>
    //{
    //    public string BatchID;
    //}
    //class Betinfo
    //{
    //    public DateTime? BetStartDate;
    //    public DateTime? BetEndDate;
    //    public string AccountId;
    //    public string TableId;
    //    public string GameId;
    //    public string BetId;
    //    public decimal? BetAmount;
    //    public decimal? Payout;
    //    public string Currency;
    //    public string GameType;
    //    public string BetSpot;
    //    public string BetNo;
    //}
    //class Affiliate
    //{
    //    public string Affiliate_id;
    //    public decimal? Total_PL;
    //    public Player Players;
    //}
    //class Player
    //{
    //    public string user_id;
    //    public string Affiliate_ID;
    //    public string AccountID;
    //    public int? TotalBet;
    //    public decimal? BetAmount;
    //    public decimal? Payout;
    //    public string Currency;
    //    public List<BetDetail> BetDetails;
    //}
    //class BetDetail
    //{
    //    public string user_id;
    //    public string Game_id;
    //    public decimal? Amount;
    //    public decimal? Payout;
    //    public DateTime? BetStartDate;
    //    public DateTime? BetEndDate;
    //    public string TableId;
    //}
    //class TableInfo : List<TableDetails>
    //{
    //}
    //class TableDetails
    //{
    //    public string Table_Id;
    //    public string Table_Name;
    //}
    //class PlayerInfo
    //{
    //}
    //class PlayerDetails
    //{
    //}

    //public class betapi
    //{
    //    static string URLString = "https://live.winclubs.com/cgibin/EGameIntegration";//注册、存取款API

    //    #region GetAllbetdetails()

    //    /// <summary>
    //    /// This Webservice will fetch bet details for different casinos of current date. The frequency of the call should be made to this API after every 5 minutes otherwise it will throw an error.
    //    /// </summary>
    //    /// <url>"http://webapi-asia.hointeractive.com/betapi.asmx/GetAllbetdetails"</url>
    //    /// <param name="Username">Username provided to access the webservice</param>
    //    /// <param name="Password">Password provided to access the webservice</param>
    //    /// <param name="CasinoId">ID</param>
    //    /// <param name="UserId">This is optional parameter. Email address or Account Id</param>
    //    /// <param name="Dateval">This is optional parameter. If user passes this Date, it will return bet details of the given date or Current date. Format mm/dd/yyyy or yyyy/mm/dd Ex: 01/12/2011or 2011/01/12</param>
    //    /// <param name="TimeRange">This is optional parameter. If user passes time from 0 to 24. Time difference should be minimum of 1 hour and maximum of 4 hours</param>
    //    /// <param name="Status">This is optional parameter.
    //    /// If user doesn’t pass this status, it will return bet details for success otherwise status passes from “Cancel” and it will return cancel game bet details.</param>
    //    /// <returns></returns>
    //    public static List<Betinfo> GetAllbetdetails(string Username, string Password, string CasinoId, string UserId, DateTime? Dateval, int? TimeRange, string Status)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetBetdetails"))
    //        {
    //            api.Param("Username", Username);
    //            api.Param("Password", Password);
    //            api.Param("CasinoId", CasinoId);

    //            api.optParam("UserId", UserId, null);
    //            api.optParam("Dateval", Dateval, "yyyy/MM/dd");
    //            api.optParam("TimeRange", TimeRange, null);
    //            api.optParam("Status", Status, null);

    //            string request;
    //            string response = api.invoke("GetAllbetdetails", out request);

    //            return parseBetInfoXML(response);
    //        }
    //    }
    //    /// <summary>
    //    /// 解析投注记录
    //    /// </summary>
    //    /// <param name="xml">The XML.</param>
    //    /// <returns></returns>
    //    public static List<Betinfo> parseBetInfoXML(string xml)
    //    {
    //        List<Betinfo> infos = new List<Betinfo>();
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrEmpty(xml))
    //            {
    //                xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
    //                doc.LoadXml(xml);

    //                Betinfo info = new Betinfo();
    //                XmlNodeList root = doc.SelectNodes("Betinfos/Betinfo");
    //                XmlNodeList properties = null;
    //                foreach (XmlNode node in root)
    //                {
    //                    properties = node.ChildNodes;
    //                    info.BetStartDate = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "BetStartDate"));
    //                    info.BetEndDate = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "BetEndDate"));
    //                    info.AccountId = XMLPropertiesHepler.getNodeValues(properties, "AccountId");
    //                    info.TableId = XMLPropertiesHepler.getNodeValues(properties, "TableId");
    //                    info.TableName = XMLPropertiesHepler.getNodeValues(properties, "TableName");
    //                    info.BetId = XMLPropertiesHepler.getNodeValues(properties, "BetId");
    //                    info.GameId = XMLPropertiesHepler.getNodeValues(properties, "GameId");
    //                    info.BetAmount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "BetAmount"));
    //                    info.Payout = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "Payout"));
    //                    info.Currency = XMLPropertiesHepler.getNodeValues(properties, "Currency");
    //                    info.GameType = XMLPropertiesHepler.getNodeValues(properties, "GameType");
    //                    info.BetSpot = XMLPropertiesHepler.getNodeValues(properties, "BetSpot");
    //                    info.BetNo = XMLPropertiesHepler.getNodeValues(properties, "BetNo");
    //                    info.IPAddress = XMLPropertiesHepler.getNodeValues(properties, "IPAddress");
    //                    infos.Add(info);
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //logger.error("解析XML异常!", e);
    //            log.message("解析投注记录返回信息XML异常!", e.Message);
    //        }
    //        return infos;
    //    }

    //    #endregion

    //    #region GetAllBetDetailsfor30seconds()

    //    public static List<Betinfo> GetAllBetDetailsfor30seconds(string Username, string Password, string CasinoId)
    //    {
    //        return betapi.GetAllBetDetailsfor30seconds(Username, Password, CasinoId, null);
    //    }

    //    /// <summary>
    //    /// This webservice will fetch bet details of different casinos for last 30 seconds. The frequency of the call should be made to this API after every 30 seconds otherwise it will throw an error.
    //    /// </summary>
    //    /// <url>"http://webapi-asia.hointeractive.com/betapi.asmx/GetAllBetDetailsfor30seconds"</url>
    //    /// <param name="Username">Username provided to access the webservice</param>
    //    /// <param name="Password">Password provided to access the webservice</param>
    //    /// <param name="CasinoId">ID</param>
    //    /// <param name="BatchId">Batch ID which was received in the response XML structure</param>
    //    /// <returns></returns>
    //    public static List<Betinfo> GetAllBetDetailsfor30seconds(string Username, string Password, string CasinoId, int? BatchId)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetBetdetails"))
    //        {
    //            api.Param("Username", Username);
    //            api.Param("Password", Password);
    //            api.Param("CasinoId", CasinoId);
    //            api.optParam("BatchId", BatchId, null);

    //            string request;
    //            string response = api.invoke("http://webapi-asia.hointeractive.com/betapi.asmx/GetAllBetDetailsfor30seconds", out request);
    //            return parseBetInfoXML(response);
    //        }
    //    }

    //    #endregion

    //    #region GetBetDetailsByAffiliate()

    //    /// <summary>
    //    /// This Webservice will fetch the bet details of different casinos either for all or specific affiliates and this API can be called at any time without having any restrictions.
    //    /// </summary>
    //    /// <url>"http://webapi-asia.hointeractive.com/betapi.asmx/GetBetDetailsByAffiliate"</url>
    //    /// <param name="Username">Username provided to access the webservice</param>
    //    /// <param name="Password">Password provided to access the webservice</param>
    //    /// <param name="CasinoId">ID</param>
    //    /// <param name="Dateval">This is optional parameter. If user passes this Date, it will return bet details of the given date or Current date.</param>
    //    /// <param name="AffiliateId">This is optional parameter. If user passes this value, it will return the bet details for a particular affiliates</param>
    //    public static List<Affiliate> GetBetDetailsByAffiliate(string Username, string Password, string CasinoId, DateTime? Dateval, string AffiliateId)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetBetdetails"))
    //        {
    //            api.Param("Username", Username);
    //            api.Param("Password", Password);
    //            api.Param("CasinoId", CasinoId);
    //            api.optParam("Dateval", Dateval, "yyyy/MM/dd");
    //            api.optParam("AffiliateId", AffiliateId, null);

    //            string request;
    //            string response = api.invoke("http://webapi-asia.hointeractive.com/betapi.asmx/GetBetDetailsByAffiliate", out request);
    //        }

    //        return null;
    //    }

    //    #endregion

    //    #region GetTableList()
    //    /// <summary>
    //    /// Gets the table list.
    //    /// </summary>
    //    /// <param name="userName">Name of the user.</param>
    //    /// <param name="password">The password.</param>
    //    /// <param name="casinoId">The casino id.</param>
    //    /// <returns></returns>
    //    public static List<TableDetails> GetTableList(string userName,string password,string casinoId)
    //    {
    //        using (CommonMethod api = new CommonMethod("Gettabledetails"))
    //        {
    //            api.Param("Username",userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            string request;
    //            string response = api.invoke("GetTableList", out request);
    //            return parseTableInfoXML(response);
    //        }
    //    }
    //    /// <summary>
    //    /// 解析表信息
    //    /// </summary>
    //    /// <param name="xml">The XML.</param>
    //    /// <returns></returns>
    //    public static List<TableDetails> parseTableInfoXML(string xml)
    //    {
    //        List<TableDetails> infos = new List<TableDetails>();
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrEmpty(xml))
    //            {
    //                xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
    //                doc.LoadXml(xml);

    //                TableDetails info = new TableDetails();
    //                XmlNodeList root = doc.GetElementsByTagName("TableDetails");
    //                XmlNodeList properties = null;
    //                foreach (XmlNode node in root)
    //                {
    //                    properties = node.ChildNodes;
    //                    info.Table_id = XMLPropertiesHepler.getNodeValues(properties, "Table_Id");
    //                    info.Table_Name = XMLPropertiesHepler.getNodeValues(properties, "Table_Name");
    //                    infos.Add(info);
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //logger.error("解析XML异常!", e);
    //            log.message("解析表格返回信息XML异常!", e.Message);
    //        }
    //        return infos;
    //    }
    //    #endregion

    //    #region GetPlayerDetails()

    //    public static List<Playerdetails> GetPlayerDetails(string userName, string password, string casinoId, string accountID, DateTime startDate, DateTime endDate)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetPlayerdetails"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("AccountID", accountID);
    //            api.Param("StartDate", startDate,"yyyy/MM/dd");
    //            api.Param("Enddate", endDate, "yyyy/MM/dd");
    //            string request;
    //            string response = api.invoke("GetPlayerDetails", out request);
    //            return parsePlayerDetailXML(response);
    //        }
    //    }
    //    /// <summary>
    //    /// 解析用户明细信息
    //    /// </summary>
    //    /// <param name="xml">The XML.</param>
    //    /// <returns></returns>
    //    public static List<Playerdetails> parsePlayerDetailXML(string xml)
    //    {
    //        List<Playerdetails> infos = new List<Playerdetails>();
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrEmpty(xml))
    //            {
    //                xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
    //                doc.LoadXml(xml);

    //                Playerdetails info = new Playerdetails();
    //                XmlNodeList root = doc.SelectNodes("PlayerInfo/PlayerDetails");
    //                XmlNodeList properties = null;
    //                foreach (XmlNode node in root)
    //                {
    //                    properties = node.ChildNodes;
    //                    info.PlayerName = XMLPropertiesHepler.getNodeValues(properties, "PlayerName");
    //                    info.Bet_Amount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "Bet_Amount"));
    //                    info.Bet_Payoff = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "Bet_Payoff"));
    //                    info.TotalWin = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "TotalWin"));
    //                    info.DeductAmount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "DeductAmount"));
    //                    info.EvenAmount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "EvenAmount"));
    //                    info.TotalAmount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "TotalAmount"));
    //                    info.MasterName = XMLPropertiesHepler.getNodeValues(properties, "MasterName");
    //                    info.AgentName = XMLPropertiesHepler.getNodeValues(properties, "AgentName");
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //logger.error("解析XML异常!", e);
    //            log.message("解析用户明细信息返回信息XML异常!", e.Message);
    //        }
    //        return infos;
    //    }
    //    #endregion

    //    #region GetAgentPlayerDetails()

    //    public static List<AgentPlayerDetails> GetAgentPlayerDetails(string userName, string password, string casinoId, DateTime dateVal)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetPlayerdetails"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("DateVal", dateVal, "yyyy/MM/dd");
    //            string request;
    //            string response = api.invoke("GetAgentPlayerDetails", out request);
    //            return parseAgentDetailXML(response);
    //        }
    //    }
    //    /// <summary>
    //    /// 解析代理详细信息
    //    /// </summary>
    //    /// <param name="xml">The XML.</param>
    //    /// <returns></returns>
    //    public static List<AgentPlayerDetails> parseAgentDetailXML(string xml)
    //    {
    //        List<AgentPlayerDetails> infos = new List<AgentPlayerDetails>();
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrEmpty(xml))
    //            {
    //                xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
    //                doc.LoadXml(xml);

    //                AgentPlayerDetails info = new AgentPlayerDetails();
    //                XmlNodeList root = doc.SelectNodes("AgentInfo/AgentDetails");
    //                XmlNodeList properties = null;
    //                foreach (XmlNode node in root)
    //                {
    //                    properties = node.ChildNodes;
    //                    info.PlayerName = XMLPropertiesHepler.getNodeValues(properties, "PlayerName");
    //                    info.Deposit = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "Deposit"));
    //                    info.Withdraw = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "Withdraw"));
    //                    info.BetAmt = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "BetAmt"));
    //                    info.PayoffAmt = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "PayoffAmt"));
    //                    info.NetLoss = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "Netloss"));
    //                    info.AgentName = XMLPropertiesHepler.getNodeValues(properties, "AgentName");
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //logger.error("解析XML异常!", e);
    //            log.message("解析代理详细信息返回信息XML异常!", e.Message);
    //        }
    //        return infos;
    //    }
    //    #endregion

    //    #region GetEventbetsAgentPlayerDetails()

    //    public static List<Playerdetails> GetEventbetsAgentPlayerDetails(string userName, string password, string casinoId, DateTime dateVal,string agentName)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetAgentPlayerdetails"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("DateVal", dateVal, "yyyy/MM/dd");
    //            api.Param("AgentName", agentName,null);
    //            string request;
    //            string response = api.invoke("GetEventbetsAgentPlayerDetails", out request);
    //            return parsePlayerDetailXML(response);
    //        }
    //    }

    //    public static List<Playerdetails> GetAgentsEvenbetsDetails(string userName, string password, string casinoId, DateTime dateVal)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetAgentdetails"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("DateVal", dateVal, "yyyy/MM/dd");
    //            string request;
    //            string response = api.invoke("GetAgentsEvenbetsDetails", out request);
    //            return parsePlayerDetailXML(response);
    //        }
    //    }
    //    #endregion

    //    #region GetAllBetDetailsPerTimeInterval()

    //    public static List<Betinfo> GetAllBetDetailsPerTimeInterval(string userName, string password, string casinoId, string userID, DateTime startTime, DateTime endTime, int? pageSize, int? pageNumber)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetAllBetDetailsPerTimeInterval"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("UserId", userID,null);
    //            api.Param("StartTime", startTime,"yyyy/MM/dd HH:mm:ss");
    //            api.Param("EndTime", endTime, "yyyy/MM/dd HH:mm:ss");
    //            api.Param("PageSize", pageSize, null);
    //            api.Param("PageNumber", pageNumber, null);
    //            string request;
    //            string response = api.invoke("GetAllBetDetailsPerTimeInterval", out request);
    //            return parseBetInfoXML(response);
    //        }
    //    }
    //    #endregion

    //    #region GetAllFundTransferDetailsTimeInterval()

    //    public static List<AccountTransferDetails> GetAllFundTransferDetailsTimeInterval(string userName, string password, string casinoId, string userID, DateTime startTime, DateTime endTime, int? pageSize, int? pageNumber)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetAllFundTransferDetailsPerTimeInterval"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("UserId", userID, null);
    //            api.Param("StartTime", startTime, "yyyy/MM/dd HH:mm:ss");
    //            api.Param("EndTime", endTime, "yyyy/MM/dd HH:mm:ss");
    //            api.Param("PageSize", pageSize, null);
    //            api.Param("PageNumber", pageNumber, null);
    //            string request;
    //            string response = api.invoke("GetAllFundTransferDetailsTimeInterval", out request);
    //            return parseAccountTransferXML(response);
    //        }
    //    }
    //    /// <summary>
    //    /// 解析转账信息
    //    /// </summary>
    //    /// <param name="xml">The XML.</param>
    //    /// <returns></returns>
    //    public static List<AccountTransferDetails> parseAccountTransferXML(string xml)
    //    {
    //        List<AccountTransferDetails> infos = new List<AccountTransferDetails>();
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrEmpty(xml))
    //            {
    //                xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
    //                doc.LoadXml(xml);

    //                AccountTransferDetails info = new AccountTransferDetails();
    //                XmlNodeList root = doc.SelectNodes("FundTransferInfos/GetAllAccountTransferDetails");
    //                XmlNodeList properties = null;
    //                foreach (XmlNode node in root)
    //                {
    //                    properties = node.ChildNodes;
    //                    info.Accountid = XMLPropertiesHepler.getNodeValues(properties, "Accountid");
    //                    info.Transact_ID = XMLPropertiesHepler.getNodeValues(properties, "Transact_ID");
    //                    info.Transacttype_Code = XMLPropertiesHepler.getNodeValues(properties, "Transacttype_Code");
    //                    info.TransactionType = XMLPropertiesHepler.getNodeValues(properties, "TransactionType");
    //                    info.Transact_Time = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "Transact_Time"));
    //                    info.Amount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "Amount"));
    //                    info.Currency_Code = XMLPropertiesHepler.getNodeValues(properties, "Currency_Code");
    //                    info.Reference_No = XMLPropertiesHepler.getNodeValues(properties, "Reference_No");
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //logger.error("解析XML异常!", e);
    //            log.message("解析转账信息返回信息XML异常!", e.Message);
    //        }
    //        return infos;
    //    }

    //    #endregion

    //    #region GetGameResultInfo()

    //    public static List<GameResultInfo> GetGameResultInfo(string userName, string password, string casinoId, string userID, DateTime startTime, DateTime endTime, int? pageSize, int? pageNumber)
    //    {
    //        using (CommonMethod api = new CommonMethod("GameResultInfo"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("UserId", userID, null);
    //            api.Param("StartTime", startTime, "yyyy/MM/dd");
    //            api.Param("EndTime", endTime, "yyyy/MM/dd");
    //            api.Param("PageSize", pageSize, null);
    //            api.Param("PageNumber", pageNumber, null);
    //            string request;
    //            string response = api.invoke("GetGameResultInfo", out request);
    //            return parseGameResultXML(response);
    //        }
    //    }
    //    /// <summary>
    //    /// 解析游戏结果信息
    //    /// </summary>
    //    /// <param name="xml">The XML.</param>
    //    /// <returns></returns>
    //    public static List<GameResultInfo> parseGameResultXML(string xml)
    //    {
    //        List<GameResultInfo> infos = new List<GameResultInfo>();
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrEmpty(xml))
    //            {
    //                xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
    //                doc.LoadXml(xml);

    //                GameResultInfo info = new GameResultInfo();
    //                XmlNodeList root = doc.SelectNodes("GameInfo")[0].ChildNodes;
    //                XmlNodeList properties = null;
    //                foreach (XmlNode node in root)
    //                {
    //                    properties = node.ChildNodes;
    //                    info.Game_Id = XMLPropertiesHepler.getNodeValues(properties, "Game_Id");
    //                    info.StartTime = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "StartTime"));
    //                    info.EndTime = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "EndTime"));
    //                    info.GameType_Id = XMLPropertiesHepler.getNodeValues(properties, "GameType_Id");
    //                    info.AccountID = XMLPropertiesHepler.getNodeValues(properties, "AccountID");
    //                    info.Dealer = XMLPropertiesHepler.getNodeValues(properties, "Dealer");
    //                    info.BankerPoint = Int32.Parse(XMLPropertiesHepler.getNodeValues(properties, "BankerPoint"));
    //                    info.PlayerPoint = Int32.Parse(XMLPropertiesHepler.getNodeValues(properties, "PlayerPoint"));
    //                    info.Tie = Int32.Parse(XMLPropertiesHepler.getNodeValues(properties, "Tie"));
    //                    info.DragonPoint = Int32.Parse(XMLPropertiesHepler.getNodeValues(properties, "DragonPoint"));
    //                    info.TigerPoint = Int32.Parse(XMLPropertiesHepler.getNodeValues(properties, "TigerPoint"));
    //                    info.Result = XMLPropertiesHepler.getNodeValues(properties, "Result");
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //logger.error("解析XML异常!", e);
    //            log.message("解析游戏结果信息返回信息XML异常!", e.Message);
    //        }
    //        return infos;
    //    }

    //    #endregion

    //    #region GetPlayerBetAmount()

    //    public static List<PlayerBetDetails> GetPlayerBetAmount(string userName, string password, string casinoId, string userID, DateTime? dateVal,int? timeRange)
    //    {
    //        using (CommonMethod api = new CommonMethod("PlayerBetInfo"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("UserId", userID, null);
    //            api.Param("DateVal", dateVal, "yyyy/MM/dd");
    //            api.Param("TimeRange", timeRange, null);

    //            string request;
    //            string response = api.invoke("GetPlayerBetAmount", out request);
    //            return parsePlayerBetDetailsXML(response);
    //        }
    //    }
    //    /// <summary>
    //    /// 解析PlayerBetDetails
    //    /// </summary>
    //    /// <param name="xml">The XML.</param>
    //    /// <returns></returns>
    //    public static List<PlayerBetDetails> parsePlayerBetDetailsXML(string xml)
    //    {
    //        List<PlayerBetDetails> infos = new List<PlayerBetDetails>();
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrEmpty(xml))
    //            {
    //                xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
    //                doc.LoadXml(xml);

    //                PlayerBetDetails info = new PlayerBetDetails();
    //                XmlNodeList root = doc.SelectNodes("PlayerBetInfo/PlayerBetDetails");
    //                XmlNodeList properties = null;
    //                foreach (XmlNode node in root)
    //                {
    //                    properties = node.ChildNodes;
    //                    info.AccountID = XMLPropertiesHepler.getNodeValues(properties, "AccountID");
    //                    info.StakedAmount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "StakedAmount"));
    //                    info.LiveGameTotalAmount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "LiveGameTotalAmount"));
    //                    info.LiveGameExcludeEvenandTieAmount = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "LiveGameExcludeEvenandTieAmount"));
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //logger.error("解析XML异常!", e);
    //            log.message("解析PlayerBetDetails返回信息XML异常!", e.Message);
    //        }
    //        return infos;
    //    }
    //    #endregion

    //    #region GetBonusInfo()

    //    public static List<BonusInfo> GetBonusInfo(string userName, string password, string casinoId, string userID,DateTime startTime,DateTime endTime,string bonusName)
    //    {
    //        using (CommonMethod api = new CommonMethod("BonusPT"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("UserId", userID, null);
    //            api.Param("StartTime", startTime, "yyyy/MM/dd");
    //            api.Param("EndTime", endTime, "yyyy/MM/dd");
    //            api.Param("BonusName", bonusName, null);

    //            string request;
    //            string response = api.invoke("GetBonusInfo", out request);
    //            return parseBonusInfoXML(response);
    //        }
    //    }
    //    /// <summary>
    //    /// 解析红利信息
    //    /// </summary>
    //    /// <param name="xml">The XML.</param>
    //    /// <returns></returns>
    //    public static List<BonusInfo> parseBonusInfoXML(string xml)
    //    {
    //        List<BonusInfo> infos = new List<BonusInfo>();
    //        try
    //        {
    //            XmlDocument doc = new XmlDocument();
    //            if (!string.IsNullOrEmpty(xml))
    //            {
    //                xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
    //                doc.LoadXml(xml);

    //                BonusInfo info = new BonusInfo();
    //                XmlNodeList root = doc.SelectNodes("BonusPTCompleteDetails/BonusPT");
    //                XmlNodeList properties = null;
    //                foreach (XmlNode node in root)
    //                {
    //                    properties = node.ChildNodes;
    //                    info.BonusName = XMLPropertiesHepler.getNodeValues(properties, "BonusName");
    //                    info.AccounID = XMLPropertiesHepler.getNodeValues(properties, "AccounID");
    //                    info.BonusStartDate = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "BonusStartDate"));
    //                    info.BonusEndDate = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "BonusEndDate"));
    //                    info.StatusDate = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "StatusDate"));
    //                    info.PTRequirement = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "PTRequirement"));
    //                    info.CurrentPT = decimal.Parse(XMLPropertiesHepler.getNodeValues(properties, "CurrentPT"));
    //                    info.PTCompleteDate = DateTime.Parse(XMLPropertiesHepler.getNodeValues(properties, "PTCompleteDate"));
    //                    info.Status = XMLPropertiesHepler.getNodeValues(properties, "Status");
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //logger.error("解析XML异常!", e);
    //            log.message("解析红利信息返回信息XML异常!", e.Message);
    //        }
    //        return infos;
    //    }
    //    #endregion

    //    #region Getbetdetails_ExcludeTieandEven()
    //    public static List<Betinfo> Getbetdetails_ExcludeTieandEven(string userName, string password, string casinoId, string userID, DateTime? dateVal,int? timeRange)
    //    {
    //        using (CommonMethod api = new CommonMethod("GetBetdetails"))
    //        {
    //            api.Param("Username", userName);
    //            api.Param("Password", password);
    //            api.Param("CasinoId", casinoId);
    //            api.Param("UserId", userID, null);
    //            api.Param("Dateval", dateVal, "yyyy/MM/dd");
    //            api.Param("TimeRange", timeRange, null);

    //            string request;
    //            string response = api.invoke("Getbetdetails_ExcludeTieAndEven", out request);
    //            return parseBetInfoXML(response);
    //        }
    //    }
    //    #endregion
    //}

    ///// <summary>
    ///// 投注记录
    ///// </summary>
    //public class _Betinfo
    //{
    //    /// <summary>
    //    /// Number of total records count
    //    /// </summary>
    //    public int? TotalRecords;

    //    /// <summary>
    //    /// Bet place time for a game by a player. Format MM/dd/yyyy hh:mm:ss Ex: 01/12/2011 03:59:59
    //    /// </summary>
    //    public DateTime BetStartDate;   // Datetime

    //    /// <summary>
    //    /// Game end time for the bet placed by a player. Format MM/dd/yyyy hh:mm:ss Ex: 01/12/2011 04:00:23
    //    /// </summary>
    //    public DateTime BetEndDate;     // Datetime

    //    /// <summary>
    //    /// Player Login name of a bet placed
    //    /// </summary>
    //    public string AccountId;        // Varchar(100)

    //    /// <summary>
    //    /// Table id for a Game type of a game where the bet was placed by a player.
    //    /// </summary>
    //    public string TableId;          // Char(16)

    //    /// <summary>
    //    /// Table Name for a Game type
    //    ///of a game where the bet was
    //    ///placed by a player.
    //    /// </summary>
    //    public string TableName;

    //    /// <summary>
    //    /// Game id for which a bet was placed by a player
    //    /// </summary>
    //    public string GameId;           // Char(24)

    //    /// <summary>
    //    /// Bet id for a game placed by a player. BetNo & BetId value will be same.
    //    /// </summary>
    //    public string BetId;            // Char(32)

    //    /// <summary>
    //    /// Bet amount placed by a player for a game Ex:10.0000
    //    /// </summary>
    //    public decimal? BetAmount;       // Decimal(16,4)

    //    /// <summary>
    //    /// Win or loss for the bet placed by a player for a game. Value positive means win. Value negative means loss Ex:10.0000, -10.0000
    //    /// </summary>
    //    public decimal? Payout;          // Decimal(16,4)

    //    /// <summary>
    //    /// Player currency code Ex: CNY, USD
    //    /// </summary>
    //    public string Currency;         // Char(3)

    //    /// <summary>
    //    /// Game type name of a game for a bet by a player
    //    /// Ex: Baccarat, Roulette
    //    /// </summary>
    //    public string GameType;         // Varchar(100)

    //    /// <summary>
    //    /// Bet placed spot for a game by a player. Ex: Banker, Split bet: 20 and 23
    //    /// </summary>
    //    public string BetSpot;          // Varchar(80)

    //    /// <summary>
    //    /// Bet id for a game placed by a player. BetNo & BetId value will be same.
    //    /// </summary>
    //    public string BetNo;            // Char(32)

    //    public string IPAddress;         // 
    //}

    //public class _Affiliate
    //{
    //    /// <summary>
    //    /// Affiliate id of a player. Ex 310
    //    /// </summary>
    //    public string Affiliate_id;     // char(16)

    //    /// <summary>
    //    /// Total win or loss during the period for a player.
    //    /// Value positive means win to player. Value negative means loss to player
    //    /// </summary>
    //    public decimal Total_PL;        // Decimal(18,2)

    //    /// <summary>
    //    /// Player User id ex : 5wn80d0rwz54e1t7
    //    /// </summary>
    //    public string user_id;          // char(16)

    //    /// <summary>
    //    /// Player Login name Ex : 88M1l1l2355
    //    /// </summary>
    //    public string AccountID;        // Varchar(100)

    //    /// <summary>
    //    /// Total No of bets placed by a player (sum of bet count) Ex :50
    //    /// </summary>
    //    public int TotalBet;            // int

    //    /// <summary>
    //    /// Total Bet amount placed by a player for a game Ex:10.00
    //    /// </summary>
    //    public decimal BetAmount;       // Decimal(18,2)

    //    /// <summary>
    //    /// Total Win or loss for the bet placed by a player for a game. Value positive means win. Value negative means loss Ex:10.00, -10.00
    //    /// </summary>
    //    public decimal Payout;          // Decimal(18,2)

    //    /// <summary>
    //    /// Player currency code Ex: CNY, USD
    //    /// </summary>
    //    public string Currency;         // Char(3)

    //    /// <summary>
    //    /// Game id for a game where bet was placed by a player
    //    /// </summary>
    //    public string Game_id;          // Char(24)

    //    /// <summary>
    //    /// Bet amount placed by a player for a game. Ex:10.0000
    //    /// </summary>
    //    public decimal amount;          // Decimal(16,4)

    //    /// <summary>
    //    /// Bet place time for a game by a player. Format MM/dd/yyyy hh:mm:ss Ex: 01/12/2011 03:59:59
    //    /// </summary>
    //    public DateTime BetStartDate;   // Datetime

    //    /// <summary>
    //    /// Game end time for the bet placed by a player. Format MM/dd/yyyy hh:mm:ss Ex: 01/12/2011 04:00:23
    //    /// </summary>
    //    public DateTime BetEndDate;     // Datetime

    //    /// <summary>
    //    /// Table id for a Game type of a game where the bet was placed by a player.
    //    /// </summary>
    //    public string TableId;          // Char(16)


    //}

    ///// <summary>
    ///// 表格信息
    ///// </summary>
    //[DebuggerDisplay("ID:{Table_id}, Name:{Table_Name}")]
    //public class _TableDetails
    //{
    //    /// <summary>
    //    /// Table id for a Game type of agame.
    //    /// </summary>
    //    public string Table_id;//Char(16)

    //    /// <summary>
    //    /// Game table name for agame. EX: Casino Black Jack2, Casino SicBo
    //    /// </summary>
    //    public string Table_Name;//varchar(240)
    //}


    ///// <summary>
    ///// 代理商明细
    ///// </summary>
    //public class _AgentPlayerDetails
    //{
    //    /// <summary>
    //    /// Name of the player bet for a game
    //    /// </summary>
    //    public string PlayerName;

    //    /// <summary>
    //    /// Agent Deposited amount during the period to a player
    //    /// </summary>
    //    public decimal? Deposit;

    //    /// <summary>
    //    /// Agent withdrawn amount during the period to a player
    //    /// </summary>
    //    public decimal? Withdraw;

    //    /// <summary>
    //    /// Total Bet amount during the  period for a player
    //    /// </summary>
    //    public decimal? BetAmt;

    //    /// <summary>
    //    /// Total Payoff for the bet
    //    ///amount during the period for
    //    ///a player
    //    /// </summary>
    //    public decimal? PayoffAmt;

    //    /// <summary>
    //    /// Total loss for the agent
    //    ///during the period from a
    //    ///player. Value positive means
    //    ///profit to agent. Value negative
    //    ///means loss to agent
    //    /// </summary>
    //    public decimal? NetLoss;

    //    /// <summary>
    //    /// Name of the agent which the
    //    /// player belongs too.
    //    /// </summary>
    //    public string AgentName;
    //}

    ///// <summary>
    ///// 账户转移明细
    ///// </summary>
    //public class _AccountTransferDetails
    //{
    //    /// <summary>
    //    /// Number of total records count
    //    /// </summary>
    //    public int TotalRecords;

    //    /// <summary>
    //    /// Player Login name of a bet placed
    //    /// </summary>
    //    public string Accountid;

    //    /// <summary>
    //    /// Identity field of player transaction
    //    /// </summary>
    //    public string Transact_ID;

    //    /// <summary>
    //    /// Player transaction mode.Ex :Cash Purchase
    //    /// </summary>
    //    public string Transacttype_Code;

    //    /// <summary>
    //    /// Type of transaction.Ex:Deposit
    //    /// </summary>
    //    public string TransactionType;

    //    /// <summary>
    //    /// Transaction time for the transaction (Cash purchase).
    //    /// </summary>
    //    public DateTime Transact_Time;

    //    /// <summary>
    //    /// Transaction amount for the transaction Ex:10.0000
    //    /// </summary>
    //    public decimal? Amount;

    //    /// <summary>
    //    /// Player currency code Ex: CNY, USD
    //    /// </summary>
    //    public string Currency_Code;


    //    /// <summary>
    //    /// Refno for transaction. If
    //    ///transaction is not available
    //    ///refno which is showing “Nil”.
    //    /// </summary>
    //    public string Reference_No;
    //}

    ///// <summary>
    ///// 游戏结果信息
    ///// </summary>
    //public class _GameResultInfo
    //{
    //    /// <summary>
    //    /// Number of total records count
    //    /// </summary>
    //    public int TotalRecords;

    //    /// <summary>
    //    /// Game id for which a bet was
    //    ///placed by a player
    //    /// </summary>
    //    public string Game_Id;

    //    /// <summary>
    //    /// Bet place time for a game by a
    //    ///player. Format MM/dd/yyyy
    //    ///hh:mm:ss Ex: 01/12/2011
    //    ///03:59:59
    //    /// </summary>
    //    public DateTime StartTime;

    //    /// <summary>
    //    /// Game end time for the bet placed by a player
    //    /// </summary>
    //    public DateTime EndTime;

    //    /// <summary>
    //    /// Game type id of a game for a bet by a player
    //    /// </summary>
    //    public string GameType_Id;

    //    /// <summary>
    //    /// Player Login name
    //    /// </summary>
    //    public string AccountID;

    //    /// <summary>
    //    /// Game dealer name
    //    /// </summary>
    //    public string Dealer;

    //    /// <summary>
    //    /// Result of banker point for baccarat game.
    //    /// </summary>
    //    public int BankerPoint;

    //    /// <summary>
    //    /// Result of player point for baccarat game.
    //    /// </summary>
    //    public int PlayerPoint;

    //    /// <summary>
    //    /// Result of tie point for
    //    ///baccarat and dragon tiger
    //    ///game. Ex : 0 means not
    //    ///game Not Tie.
    //    ///1 means Game tie.
    //    /// </summary>
    //    public int Tie;

    //    /// <summary>
    //    /// Result of Dragon point for Dragon Tiger game.
    //    /// </summary>
    //    public int DragonPoint;

    //    /// <summary>
    //    /// Result of Tiger point for Dragon Tiger game.
    //    /// </summary>
    //    public int TigerPoint;

    //    /// <summary>
    //    /// Game result for
    //    ///Blackjack,Roulette,Sicbo
    //    ///game.Ex: 24 Black
    //    /// </summary>
    //    public string Result;
    //}

    //public class _PlayerBetDetails
    //{
    //    /// <summary>
    //    /// Player Login name of a bet placed
    //    /// </summary>
    //    public string AccountID;

    //    /// <summary>
    //    /// Sum bet amount for all games
    //    /// </summary>
    //    public decimal? StakedAmount;

    //    /// <summary>
    //    /// Sum of bet amount of live games
    //    /// </summary>
    //    public decimal? LiveGameTotalAmount;

    //    /// <summary>
    //    /// Exclude even and tie amount for only live games
    //    /// </summary>
    //    public decimal? LiveGameExcludeEvenandTieAmount;
    //}

    ///// <summary>
    ///// 红利信息
    ///// </summary>
    //public class _BonusInfo
    //{
    //    /// <summary>
    //    /// Name of the bonus
    //    /// </summary>
    //    public string BonusName;

    //    /// <summary>
    //    /// Player Login name Ex :Test123
    //    /// </summary>
    //    public string AccounID;

    //    /// <summary>
    //    /// Bonus activates date. Format MM/dd/yyyy hh:mm:ss Ex: 01/12/2011 03:59:59
    //    /// </summary>
    //    public DateTime BonusStartDate;

    //    /// <summary>
    //    /// Bonus expired date.
    //    /// </summary>
    //    public DateTime BonusEndDate;

    //    /// <summary>
    //    /// Bonus accepted or declined date. Format MM/dd/yyyy hh:mm:ss
    //    /// </summary>
    //    public DateTime StatusDate;

    //    /// <summary>
    //    /// Play through (PT) money to complete the Bonus.
    //    /// </summary>
    //    public decimal? PTRequirement;

    //    /// <summary>
    //    /// Play through (PT) money currently completed by a player.
    //    /// </summary>
    //    public decimal? CurrentPT;

    //    /// <summary>
    //    /// If Player completes the
    //    ///Play through (PT) the
    //    ///complete date will be
    //    ///shown otherwise if
    //    ///bonus is declined by
    //    ///player then the declined
    //    ///date will be shown.
    //    ///Format MM/dd/yyyy
    //    ///hh:mm:ss Ex:
    //    ///01/12/2011 03:59:59
    //    /// </summary>
    //    public DateTime PTCompleteDate;

    //    /// <summary>
    //    /// Status of the Bonus like Accept, Declined,PTCompleted and Pending.
    //    /// </summary>
    //    public string Status;
    //}
}