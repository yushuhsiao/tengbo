using BU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Security;
using Tools;
using web;

namespace web
{
    /// <summary>
    /// 智付網 callback 處理函式
    /// </summary>
    public class dinpay : web.tran.Cash.ThirdPaymentRowCommand, IHttpHandler
    {
        //public static config CurrentConfig { [DebuggerStepThrough] get { return dinpay.config.GetInstance(_Global.DefaultCorpID); } }
        //[DebuggerStepThrough]
        //public static config GetConfig(int? corpID) { return config.GetInstance(corpID); }
        //public class config
        //{
        //    static Dictionary<int, config> instances = new Dictionary<int, config>();
        //    public static config GetInstance(int? corpID)
        //    {
        //        corpID = corpID ?? 0;
        //        config instance;
        //        lock (instances)
        //            if (!instances.TryGetValue(corpID.Value, out instance))
        //                instances[corpID.Value] = instance = new config(corpID.Value);
        //        return instance;
        //    }
        //    readonly int CorpID;
        //    private config(int corpID) { this.CorpID = corpID; }

        //    [SqlSetting("dinpay", "Enabled"), DefaultValue(false)]
        //    public bool Enabled
        //    {
        //        get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod(), this.CorpID); }
        //    }

        //    [SqlSetting("dinpay", "key"), DefaultValue("")] //<--支付密钥--> 注:此处密钥必须与商家后台里的密钥一致
        //    public string Key
        //    {
        //        get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        //    }

        //    [SqlSetting("dinpay", "action"), DefaultValue("https://payment.dinpay.com/VirReceiveMerchantAction.do")]
        //    public string action
        //    {
        //        get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        //    }

        //    [SqlSetting("dinpay", "M_ID"), DefaultValue("2680603")]
        //    public string M_ID
        //    {
        //        get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        //    }

        //    [SqlSetting("dinpay", "M_URL"), DefaultValue("http://www.tengfa8.com/dinpay.ashx")]
        //    public string M_URL
        //    {
        //        get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        //    }
        //}

        string m_id;
        string m_orderid;
        string m_oamount;
        string m_ocurrency;
        string m_url;
        string m_language;
        string s_name;
        string s_addr;
        string s_postcode;
        string s_tel;
        string s_eml;
        string r_name;
        string r_addr;
        string r_postcode;
        string r_eml;
        string r_tel;
        string m_ocomment;
        string m_status;
        string modate;
        string newmd5info;

        public void ProcessRequest(HttpContext context)
        {
            try { log.message("dinpay", "{0}", context.Request.Form.ToString()); }
            catch { }

            try
            {
                this.m_id = context.Request.Form["m_id"] ?? "";
                //if (string.IsNullOrEmpty(this.m_id))
                //{
                //    context.Response.Redirect("~/");
                //    return;
                //}
                this.m_orderid = context.Request.Form["m_orderid"] ?? "";
                this.m_oamount = context.Request.Form["m_oamount"] ?? "";
                //this.m_ocurrency = context.Request.Form["m_ocurrency"] ?? "";
                //this.m_url = context.Request.Form["m_url"] ?? "";
                //this.m_language = context.Request.Form["m_language"] ?? "";
                //this.s_name = context.Request.Form["s_name"] ?? "";
                this.s_addr = context.Request.Form["s_addr"] ?? "";
                //this.s_postcode = context.Request.Form["s_postcode"] ?? "";
                //this.s_tel = context.Request.Form["s_tel"] ?? "";
                //this.s_eml = context.Request.Form["s_eml"] ?? "";
                //this.r_name = context.Request.Form["r_name"] ?? "";
                this.r_addr = context.Request.Form["r_addr"] ?? "";
                //this.r_postcode = context.Request.Form["r_postcode"] ?? "";
                //this.r_eml = context.Request.Form["r_eml"] ?? "";
                //this.r_tel = context.Request.Form["r_tel"] ?? "";
                this.m_ocomment = context.Request.Form["m_ocomment"] ?? "";
                this.m_status = context.Request.Form["m_status"] ?? "";
                //this.modate = context.Request.Form["modate"] ?? "";
                this.newmd5info = context.Request.Form["newmd5info"] ?? "";
                if (string.IsNullOrEmpty(this.newmd5info))
                    throw new ArgumentNullException("newmd5info is null");
                //string newOrderMessage = m_id + m_orderid + m_oamount + GetConfig(null).Key + m_status;
                //string newMD5text = FormsAuthentication.HashPasswordForStoringInConfigFile(newOrderMessage, "md5").Trim().ToUpper();
                //string Upnewmd5info = newmd5info.ToUpper();
                //if (newMD5text != Upnewmd5info)
                //    throw new Exception("newMD5text != Upnewmd5info");

                base.ID = new Guid(Convert.FromBase64String(r_addr)); // new Guid(Convert.FromBase64String(m_orderid + "=="));
                base.op_Finish = m_status == "2";// ? 1 : 0;
                base.op_Delete = true;
                tran.Cash.ThirdPaymentRowData tranRow = base.Execute(sqlcmd, null, null);
                //MemberTranRow tranRow = base.Update(null, null);
                log.message("dinpay", "TranID:{0}\tStatus:{2}\t{3}\r\n{1}", api.SerializeObject(this), api.SerializeObject(tranRow), m_status, context.Request.Form.ToString());
                //跳转至支付结果页面
                //context.Response.Redirect(mp.SourceSite + "MemberCenter/pay_Finish.aspx?status=1&amt=" + result.R3_Amt);
            }
            catch (Exception ex)
            {
                log.message("dinpay", "{0}", ex);
            }
        }

        protected override tran.Cash.ThirdPaymentRowData GetTranRow(RowErrorCode? err, bool and_LogType)
        {
            tran.Cash.ThirdPaymentRowData row = base.GetTranRow(err, and_LogType);
            DinpayRowData chn = _null<DinpayRowCommand>.value.GetRow(sqlcmd, row.CashChannelID);
            if (chn == null) throw new Exception("Unable found CashChannel");
            string newOrderMessage = this.m_id + this.m_orderid + this.m_oamount + chn.sec_key + this.m_status;
            string newMD5text = FormsAuthentication.HashPasswordForStoringInConfigFile(newOrderMessage, "md5").Trim().ToUpper();
            string Upnewmd5info = this.newmd5info.ToUpper();
            if (newMD5text == Upnewmd5info)
                return row;
            else
                throw new Exception("newMD5text != Upnewmd5info");
        }

        //protected override void OnGetRow(SqlCmd sqlcmd, MemberTranRow row)
        //{
        //    bool success = false;
        //    foreach (CashChannelRow _chn in CashChannelRow.Cache.GetInstance(sqlcmd, null).Rows)
        //    {
        //        DinpayRowData chn = _chn as DinpayRowData;
        //        if (chn == null) continue;
        //        string newOrderMessage = this.m_id + this.m_orderid + this.m_oamount + chn.sec_key + this.m_status;
        //        string newMD5text = FormsAuthentication.HashPasswordForStoringInConfigFile(newOrderMessage, "md5").Trim().ToUpper();
        //        string Upnewmd5info = this.newmd5info.ToUpper();
        //        if (success = (newMD5text == Upnewmd5info)) break;
        //    }
        //    if (success == false)
        //        throw new Exception("newMD5text != Upnewmd5info");
        //    base.OnGetRow(sqlcmd, row);
        //}

        public bool IsReusable
        {
            get { return false; }
        }
    }
}