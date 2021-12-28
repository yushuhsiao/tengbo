using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace extAPI.kg
{
    public static class kgapi
    {
        #region Configeration
        /// <summary>
        /// kg注册、登录url
        /// </summary>
        [SqlSetting("kg","kgreg_url"), DefaultValue("http://test.integrate.v88kgg.com/player_enter_keno.php")]
        private static string kgreg_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        /// <summary>
        /// kg转账url
        /// </summary>
       [SqlSetting("kg", "kgtransfer_url"), DefaultValue("http://test.integrate.v88kgg.com/player_fund_in_out_first.php")]
        private static string kgtransfer_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        /// <summary>
        /// kg转账确认url
        /// </summary>
        [SqlSetting("kg", "kgtransferconfirm"), DefaultValue("http://test.integrate.v88kgg.com/player_fund_in_out_confirm.php")]
        private static string kgtransferconfirm_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("kg", "kgcredit_url"), DefaultValue("http://test.integrate.v88kgg.com/player_get_credit.php")]
        private static string kgcredit_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

       [SqlSetting("kg", "kgpnl_url"), DefaultValue("http://integrate.v88kgg.com/player_pnl.php")]
        private static string kgpnl_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [SqlSetting("kg", "kgpnlssc_url"), DefaultValue("http://integrate.v88kgg.com/player_ssc_pnl.php")]
        private static string kgpnlssc_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [SqlSetting("kg", "kgVendorID"), DefaultValue("68")]
        private static string kgVendorID
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        #endregion

        public static kgResponse RegistAndLogin(KGUserInfo kguser)
        {
            string responses = "";
            using (kgRequest request=new kgRequest("PlayerLanding"))
            {
                request["VendorId"] = kgVendorID;
                request["PlayerId"] = kguser.PlayerId;
                request["PlayerRealName"] = kguser.PlayerRealName;
                request["PlayerCurrency"] = kguser.PlayerCurrency;
                request["PlayerAllowStake"] = kguser.PlayerAllowStake;
                request["Trial"] = kguser.Trial;
                request["GameType"] = kguser.GameType;
                request["Language"] = kguser.Language;
                request["PlayerIP"] = kguser.PlayerIP;
                //request.EndMake();
                responses = request.GetResponse(kgreg_url);
            }
            return kgResponse.Parse(responses);
        }

        public static kgResponse KGAccountFirstTransfer(KGAccountTransfer trans)
        {
            string responses = "";
            using (kgRequest request=new kgRequest("FundInOutFirst"))
            {
                request["VendorId"] = kgVendorID;
                request["PlayerId"] = trans.PlayerId;
                request["Amount"] = trans.Amount;
                //request.EndMake();
                responses = request.GetResponse(kgtransfer_url);
            }
            return kgResponse.Parse(responses);
        }

        public static kgResponse KGAccountConfirmTransfer(KGAccountTransfer trans)
        {
            string responses = "";
            using (kgRequest request = new kgRequest("FundInOutConfirm"))
            {
                request["VendorId"] = kgVendorID;
                request["PlayerId"] = trans.PlayerId;
                request["Amount"] = trans.Amount;
                request["PlayerIP"] = trans.PlayerIP;
                request["FundIntegrationId"] = trans.FundIntegrationId;
                request["VendorRef"] = trans.VendorRef;
                //request.EndMake();
                responses = request.GetResponse(kgtransferconfirm_url);
            }
            return kgResponse.Parse(responses);
        }

        public static kgResponse KGUserDetailSearch(string username)
        {
            string responses = "";
            using (kgRequest request = new kgRequest("GetCredit"))
            {
                request["VendorId"] = kgVendorID;
                request["PlayerId"] = username;
                //request.EndMake();
                responses = request.GetResponse(kgcredit_url);
            }
            return kgResponse.Parse(responses);
        }

        /// <summary>
        /// 获取盈亏记录
        /// </summary>
        /// <param name="pnl">The PNL.</param>
        public static KGPnl GetProfitAndLoss(KGPnl pnl)
        {
            string postUrl = kgpnl_url;
            if (!string.IsNullOrEmpty(pnl.GameType) && pnl.GameType=="SSC")
            {
                postUrl = kgpnlssc_url;
            }
            string responses = "";
            using (kgRequest request = new kgRequest("GetPNL"))
            {
                request["VendorId"] = kgVendorID;
                request["Date"] = pnl.Date;
                request["HourType"] = pnl.HourType;
                //request.EndMake();
                responses = request.GetResponse(postUrl);
            }
            return parsKGPnlXML(responses);
        }
        /// <summary>
        /// 解析KG游戏盈亏返回信息
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private static KGPnl parsKGPnlXML(string xml)
        {
            KGPnl detail = new KGPnl();
            try
            {
                XmlDocument doc = new XmlDocument();
                if (!string.IsNullOrEmpty(xml))
                {
                    xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
                    doc.LoadXml(xml);

                    XmlNodeList root = doc.GetElementsByTagName("member");
                    XmlNodeList properties = null;
                    foreach (XmlNode node in root)
                    {
                        properties = node.ChildNodes;
                        detail.PlayerId = string.IsNullOrEmpty(detail.PlayerId) ? XMLPropertiesHepler.getKGDetailNodeValues(properties, "PlayerId") : detail.PlayerId;
                        detail.Total_Stake_Accurate = string.IsNullOrEmpty(detail.Total_Stake_Accurate) ? XMLPropertiesHepler.getKGDetailNodeValues(properties, "Total_Stake_Accurate") : detail.Total_Stake_Accurate;
                        detail.Total_PNL = string.IsNullOrEmpty(detail.Total_PNL) ? XMLPropertiesHepler.getKGDetailNodeValues(properties, "Total_PNL") : detail.Total_PNL;
                    }
                }
            }
            catch (Exception e)
            {
                //logger.error("解析XML异常!", e);
                log.message("解析kg盈亏返回信息XML异常!", e.Message);
            }
            return detail;
        }
    }
    class kgRequest : apiRequest
    {
        public string Action { get; private set; }

        public kgRequest(string methodName)
        {
            xml.WriteStartDocument();
            xml.WriteStartElement("methodCall");
            xml.WriteStartElement("methodName");
            xml.WriteValue(util.EnumToValue(methodName));
            xml.WriteEndElement();
            xml.WriteStartElement("params");
            xml.WriteStartElement("param");
            xml.WriteStartElement("value");
            xml.WriteStartElement("struct");
        }

        public object this[string name]
        {
            set
            {
                if (value == null) return;
                xml.WriteStartElement("member");

                xml.WriteStartElement("name");
                xml.WriteValue(util.EnumToValue(name));
                xml.WriteEndElement();

                xml.WriteStartElement("value");
                    xml.WriteStartElement("string");
                    xml.WriteValue(util.EnumToValue(value));
                    xml.WriteEndElement();
                xml.WriteEndElement();

                xml.WriteEndElement();
            }
        }
       
        public object this[string name, string format]
        {
            set
            {
                if (value == null) return;
                xml.WriteStartElement(name);
                xml.WriteValue(string.Format(format, util.EnumToValue(value)));
                xml.WriteEndElement();
            }
        }
    }

    public class kgResponse: Dictionary<string, string>
    {
        public string Root;
        public string Action;

        public static kgResponse Parse(string xml)
        {
            kgResponse ret = null;
            xml = System.Text.RegularExpressions.Regex.Replace(xml, "^[^<]", "");
            using (StringReader sr = new StringReader(xml))
            using (XmlTextReader r = new XmlTextReader(sr))
            {
                while (r.Read())
                {
                    if (r.NodeType == XmlNodeType.Element)
                    {
                        if (r.Depth == 1)
                        {
                            ret = new kgResponse();
                        }
                        else if ((r.Depth == 4) && r.Name.Equals("struct", StringComparison.OrdinalIgnoreCase))
                        {
                            if (ret == null) return null;
                            string mykey="customError";
                            foreach (var n5 in r.ReadElement(5,"member"))
                            {
                                while (r.Read(6))
                                {
                                    if (r.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                                    {
                                        mykey = r.ReadString();
                                    }
                                    else if (r.Name.Equals("value", StringComparison.OrdinalIgnoreCase))
                                    {
                                       
                                        while (r.Read(7))
                                        {
                                            if (r.IsStartElement())
                                            {
                                                ret[mykey] = r.ReadString();
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                }
                                
                            }
                            return ret;
                        }
                    }
                }
            }
            return ret;
        }
    }
    
    public class KGUserInfo
    {
        /// <summary>
        /// 网主 客户端前端网址
        /// </summary>
        /// <value>The vendor site.</value>
        public string VendorSite { get; set; }
        /// <summary>
        /// 网主给予玩家fund in/out 的网址
        /// </summary>
        /// <value>The vendor site.</value>
        public string FundLink { get; set; }
        /// <summary>
        /// 网主分配给玩家ID (独特的ID)
        /// </summary>
        /// <value>The vendor site.</value>
        public string PlayerId { get; set; }
        /// <summary>
        /// 显示玩家名称
        /// </summary>
        /// <value>The vendor site.</value>
        public string PlayerRealName { get; set; }
        /// <summary>
        /// 玩家信用货币(KenoGroup会审核玩家之前的信用货币，不匹配将被驳回)
        /// </summary>
        /// <value>The vendor site.</value>
        public string PlayerCurrency { get; set; }
        /// <summary>
        /// 默认为  0
        /// </summary>
        /// <value>The vendor site.</value>
        public decimal PlayerCredit { get; set; }
        /// <summary>
        /// 玩家选择赌注上限组别
        /// </summary>
        /// <value>The vendor site.</value>
        public string PlayerAllowStake { get; set; }
        /// <summary>
        /// 是否试玩用户
        /// </summary>
        /// <value>The vendor site.</value>
        public IsTrial Trial { get; set; }
        /// <summary>
        /// 游戏类别
        /// </summary>
        /// <value>The vendor site.</value>
        public string GameType { get; set; }
        /// <summary>
        /// 玩家选择赌注上限组别
        /// </summary>
        /// <value>The vendor site.</value>
        public string Language { get; set; }
        /// <summary>
        /// 玩家选择赌注上限组别
        /// </summary>
        /// <value>The vendor site.</value>
        public string PlayerIP { get; set; }
    }

    public class KGAccountTransfer
    {
        public int VendorId { get; set; }
        public string PlayerId { get; set; }
        public decimal? Amount { get; set; }
        public string PlayerIP { get; set; }
        public int? FundIntegrationId { get; set; }
        public string VendorRef { get; set; }
    }

    public class KGUserDetails
    {
        public int VendorId { get; set; }
        public string PlayerId { get; set; }
        public string Credit { get; set; }
        public string Jackpot { get; set; }
        public string Status { get; set; }
        public string LastLoginTime { get; set; }

        public override string ToString()
        {
            return "Credit=" + this.Credit + ";Jackpot=" + Jackpot + ";Status=" + Status + "LastLoginTime=" + LastLoginTime;
        }
    }

    /// <summary>
    /// KG游戏盈亏
    /// </summary>
    public class KGPnl
    {
        public string GameType { get; set; }
        public string PlayerId { get; set; }
        public string Total_Stake_Accurate { get; set; }
        public string Total_PNL { get; set; }
        public int VendorId { get; set; }
        public string Date { get; set; }
        /// <summary>
        /// 1 is 12:00:00-12:00:00, 2 is 00:00:00 - 23:59:59
        /// </summary>
        /// <value>The type of the hour.</value>
        public KGHourType HourType { get; set; }
    }

}
