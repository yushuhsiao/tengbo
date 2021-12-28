using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Configuration;
using System.Web;

namespace cmbchina
{
    static class categorys
    {
        public const string Login = "1.登入";
        public const string Post = "2.上傳資料";
        public const string Today = "3.當天交易查询";
        public const string History = "4.歷史交易查询";
        public const string Tags = "5.Tags";
    }
    static class sections
    {
        public const string Login = "Login";
        public const string Post = "Post";
        public const string Today = "Today";
        public const string History = "History";
        public const string Tags = "Tags";
    }
    partial class config_min
    {
        [Category(categorys.Login), DisplayName("Url")]
        [AppSetting(sections.Login, "Url")]
        [DefaultValue("https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/GenShellPC/Login/Login.aspx")]
        public string cmbchina_url
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        //[AppSetting(Key = "Update")]
        //public bool Update
        //{
        //    get { return (bool)app.config.GetValue(MethodBase.GetCurrentMethod()); }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}

        //[AppSetting(Key = "Interval")]
        //public double Interval
        //{
        //    get { return Math.Max(3000.0, (double)app.config.GetValue(MethodBase.GetCurrentMethod())); }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), Math.Max(3000.0, value)); }
        //}

    }
    partial class config
    {
        public readonly config_min min = new config_min();
    }

    partial class config_min
    {
        [Category(categorys.Post), DisplayName("Url")]
        [AppSetting(sections.Post, "Url")]
        [DefaultValue("http://localhost/postdata.aspx")]
        public string Post_Url
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }
    }
    partial class config
    {
        [Category(categorys.Post), DisplayName("ID_Key")]
        [AppSetting(sections.Post, "ID_Key")]
        [DefaultValue("src_id")]
        public string post_id_key
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Post), DisplayName("ID")]
        [AppSetting(sections.Post, "ID")]
        [DefaultValue("cmbchina")]
        public string post_id_value
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Post), DisplayName("time")]
        [AppSetting(sections.Post, "time")]
        [DefaultValue("time")]
        public string post_time
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Post), DisplayName("out")]
        [AppSetting(sections.Post, "out")]
        [DefaultValue("out")]
        public string post_out
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Post), DisplayName("in")]
        [AppSetting(sections.Post, "in")]
        [DefaultValue("in")]
        public string post_in
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Post), DisplayName("bal")]
        [AppSetting(sections.Post, "bal")]
        [DefaultValue("bal")]
        public string post_bal
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Post), DisplayName("type")]
        [AppSetting(sections.Post, "type")]
        [DefaultValue("type")]
        public string post_type
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Post), DisplayName("memo")]
        [AppSetting(sections.Post, "memo")]
        [DefaultValue("memo")]
        public string post_memo
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Post), DisplayName("name")]
        [AppSetting(sections.Post, "name")]
        [DefaultValue("name")]
        public string post_name
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }
    }

    partial class config_min
    {
        [Category(categorys.Today), DisplayName("自動更新")]
        [AppSetting(sections.Today, "Update")]
        [DefaultValue(true)]
        public bool today_Update
        {
            get { return (bool)app.config.GetValue(MethodBase.GetCurrentMethod()); }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("自動更新")]
        [AppSetting(sections.History, "Update")]
        [DefaultValue(true)]
        public bool hist_Update
        {
            get { return (bool)app.config.GetValue(MethodBase.GetCurrentMethod()); }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Today), DisplayName("自動更新(ms)")]
        [AppSetting(sections.Today, "Interval")]
        [DefaultValue(60000L)]
        public long today_Interval
        {
            get { return Math.Max(3000L, (long)app.config.GetValue(MethodBase.GetCurrentMethod())); }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), Math.Max(3000L, value)); }
        }

        [Category(categorys.History), DisplayName("自動更新(ms)")]
        [AppSetting(sections.History, "Interval")]
        [DefaultValue(60000L)]
        public long hist_Interval
        {
            get { return Math.Max(3000L, (long)app.config.GetValue(MethodBase.GetCurrentMethod())); }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), Math.Max(3000L, value)); }
        }

        [Category(categorys.History), DisplayName("日期區間(days)")]
        [AppSetting(sections.History, "Range")]
        [DefaultValue(2)]
        public int hist_Range
        {
            get { return Math.Max(1, (int)app.config.GetValue(MethodBase.GetCurrentMethod())); }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), Math.Max(1, value)); }
        }

        [Category(categorys.Today), DisplayName("匯入資料")]
        [AppSetting(sections.Today, "Parse")]
        [DefaultValue(true)]
        public bool today_parse
        {
            get { return (bool)app.config.GetValue(MethodBase.GetCurrentMethod()); }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("匯入資料")]
        [AppSetting(sections.History, "Parse")]
        [DefaultValue(true)]
        public bool hist_parse
        {
            get { return (bool)app.config.GetValue(MethodBase.GetCurrentMethod()); }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }
    }
    partial class config
    {
        //[Category(categorys.Today), DisplayName("Url")]
        //[AppSetting(sections.Today, "Url")]
        //[DefaultValue("https://pbsz.ebank.cmbchina.com/CmbBank_PB/UI/PBPC/DebitCard_AccountManager/am_QueryTodayTrans.aspx")]
        //public string today_url
        //{
        //    get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}

        //[Category(categorys.History), DisplayName("Url")]
        //[AppSetting(sections.History, "Url")]
        //[DefaultValue("https://pbsz.ebank.cmbchina.com/CmbBank_PB/UI/PBPC/DebitCard_AccountManager/am_QueryHistoryTrans.aspx")]
        //public string hist_url
        //{
        //    get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}

        [Category(categorys.Today), DisplayName("Table")]
        [AppSetting(sections.Today, "Table")]
        [DefaultValue("Table7")]
        public string today_table
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("Table")]
        [AppSetting(sections.History, "Table")]
        [DefaultValue("Table8")]
        public string hist_table
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Today), DisplayName("TransRecSet")]
        [AppSetting(sections.Today, "TransRecSet")]
        [DefaultValue("dgTodayTransRecSet")]
        public string today_TransRecSet
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("TransRecSet")]
        [AppSetting(sections.History, "TransRecSet")]
        [DefaultValue("dgHistoryTransRecSet")]
        public string hist_TransRecSet
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        //[Category(categorys.Today), DisplayName("authName")]
        //[AppSetting(sections.Today, "authName")]
        //[DefaultValue("CBANK_PB")]
        //public string today_authName
        //{
        //    get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}

        //[Category(categorys.History), DisplayName("authName")]
        //[AppSetting(sections.History, "authName")]
        //[DefaultValue("CBANK_PB")]
        //public string hist_authName
        //{
        //    get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}

        //[Category(categorys.Today), DisplayName("funcName")]
        //[AppSetting(sections.Today, "funcName")]
        //[DefaultValue("DebitCard_AccountManager/am_QueryTodayTrans.aspx")]
        //public string today_funcName
        //{
        //    get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}

        //[Category(categorys.History), DisplayName("funcName")]
        //[AppSetting(sections.History, "funcName")]
        //[DefaultValue("DebitCard_AccountManager/am_QueryHistoryTrans.aspx")]
        //public string hist_funcName
        //{
        //    get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}

        [Category(categorys.Today), DisplayName("BtnOK")]
        [AppSetting(sections.Today, "BtnOK")]
        [DefaultValue("BtnOK")]
        public string today_BtnOK
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("BtnOK")]
        [AppSetting(sections.History, "BtnOK")]
        [DefaultValue("BtnOK")]
        public string hist_BtnOK
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Today), DisplayName("QueryTodayTrans")]
        [AppSetting(sections.Today, "QueryTodayTrans")]
        [DefaultValue("triggerFunc('../DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM','_self')")]
        public string today_QueryTodayTrans
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("QueryTodayTrans")]
        [AppSetting(sections.History, "QueryTodayTrans")]
        [DefaultValue("triggerFunc('../DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM','_self')")]
        public string hist_QueryTodayTrans
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.Today), DisplayName("QueryHistoryTrans")]
        [AppSetting(sections.Today, "QueryTodayTrans")]
        [DefaultValue("triggerFunc('../DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM','_self')")]
        public string today_QueryHistoryTrans
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("QueryHistoryTrans")]
        [AppSetting(sections.History, "QueryHistoryTrans")]
        [DefaultValue("triggerFunc('../DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM','_self')")]
        public string hist_QueryHistoryTrans
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("BeginDate")]
        [AppSetting(sections.History, "BeginDate")]
        [DefaultValue("BeginDate")]
        public string hist_BeginDate
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [Category(categorys.History), DisplayName("EndDate")]
        [AppSetting(sections.History, "EndDate")]
        [DefaultValue("EndDate")]
        public string hist_EndDate
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }
    }

    partial class config : config_min
    {
        //[AppSetting(Key = "LoginBtn")]
        //[DefaultValue("LoginBtn")]
        //public string LoginBtn
        //{
        //    get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}

        //[AppSetting(Key = "BtnOK")]
        //[DefaultValue("BtnOK")]
        //public string BtnOK
        //{
        //    get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
        //    set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        //}


        [AppSetting(SectionName = sections.Tags, Key = "LoginBtn")]
        [Category(categorys.Tags)]
        [DefaultValue("LoginBtn")]
        public string tags_LoginBtn
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [AppSetting(SectionName = sections.Tags, Key = "GenIndex_QueryTodayTrans")]
        [Category(categorys.Tags)]
        [DefaultValue("CallFuncEx2('A','CBANK_PB','DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM',null)")]
        public string tags_GenIndex_QueryTodayTrans
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }

        [AppSetting(SectionName = sections.Tags, Key = "GenIndex_QueryHistoryTrans")]
        [Category(categorys.Tags)]
        [DefaultValue("CallFuncEx2('A','CBANK_PB','DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM',null)")]
        public string tags_GenIndex_QueryHistoryTrans
        {
            get { return app.config.GetValue(MethodBase.GetCurrentMethod()) as string; }
            set { app.config.SetValue(MethodBase.GetCurrentMethod(), value); }
        }
    }

    //[Flags]
    //enum forms { LoginForm = 0x01, TheForm = 0x02, GenForm = 0x04, am_QueryHistoryTrans = 0x08, am_QueryTodayTrans = 0x10 }

    partial class config
    {
        //public string forms_LoginForm = "LoginForm";
        //public string forms_TheForm = "TheForm";
        //public string forms_GenForm = "GenForm";
        //public string forms_am_QueryHistoryTrans = "am_QueryHistoryTrans";
        //public string forms_am_QueryTodayTrans = "am_QueryTodayTrans";
    }

    static class util
    {
        //public static bool IsTimeout(this DateTime? t, double interval)
        //{
        //    if (t.HasValue)
        //        return (DateTime.Now - t.Value).TotalMilliseconds > interval;
        //    return true;
        //}

        public const string col_index = "col_index";
        public const string col_Time = "col_Time";
        public const string col_out = "col_out";
        public const string col_in = "col_in";
        public const string col_bal = "col_bal";
        public const string col_type = "col_type";
        public const string col_memo = "col_memo";
        public const string col_name = "col_name";
        public const string col_srcUrl = "col_srcUrl";
        public const string col_Status = "col_Status";

        //public static void AppendX(this StringBuilder sb, object value)
        //{
        //    sb.Append(HttpUtility.UrlEncode(Convert.ToString(value)));
        //}

        public static bool TagNameIs(this HtmlElement e, string name)
        {
            if (e == null) return false;
            return (e.TagName ?? "").ToLower() == name.ToLower();
        }

        public static HtmlElement GetElementById(this HtmlDocument doc, string id, params string[] parents)
        {
            if (doc == null)
                return null;
            HtmlElement e = doc.GetElementById(id);
            return e.ParentsIs(parents) ? e : null;
        }
        public static bool ParentsIs(this HtmlElement e, params string[] parents)
        {
            if (e == null) return false;
            HtmlElement node = e;
            for (int i = parents.Length - 1; i >= 0; i--)
            {
                node = node.Parent;
                if (node == null) return false;
                if ((node.TagName ?? "").ToLower() != (parents[i] ?? "").ToLower())
                    return false;
            }
            return true;
        }

        public static HtmlElement GetElementById_(this HtmlDocument doc, string id, string match_url, bool r)
        {
            if (doc != null)
            {
                if ((match_url == null) || (match_url == doc.Url.ToString()))
                {
                    HtmlElement e = doc.GetElementById(id);
                    if (e != null) return e;
                }
                if (r)
                {
                    foreach (HtmlWindow frame in doc.Window.Frames)
                    {
                        HtmlElement e = GetElementById_(frame.Document, id, match_url, false);
                        if (e != null) return e;
                    }
                }
            }
            return null;
        }

        //public static IEnumerable<HtmlElement> GetAllForms(this HtmlWindow window)
        //{
        //    if (window == null)
        //        yield break;
        //    foreach (HtmlElement form in window.Document.Forms)
        //        yield return form;
        //    foreach (HtmlWindow frame in window.Frames)
        //        foreach (HtmlElement form2 in GetAllForms(frame))
        //            yield return form2;
        //}
    }

}
