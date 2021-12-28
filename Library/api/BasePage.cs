using BU;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Tools.Protocol;
using web;

namespace web
{
    public interface IPageLang
    {
        string this[string resourceKey] { get; }
        string this[string classKey, string resourceKey] { get; }
    }

    public class BasePage : System.Web.UI.Page, IPageLang
    {
        public new User User
        {
            [DebuggerStepThrough]
            get { return base.User as User; }
        }

        public IPageLang lang
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        string IPageLang.this[string resourceKey]
        {
            get
            {
                string s = Lang.GetLocalResourceObject(this.AppRelativeVirtualPath, resourceKey);
                for (MasterPage m = this.Master; (s == null) && (m != null); m = m.Master)
                    s = Lang.GetLocalResourceObject(m.AppRelativeVirtualPath, resourceKey);
                s = s ?? Lang.GetGlobalResourceObject("res", resourceKey);
                return s;
                //return Lang.GetLocalResourceObject(this.AppRelativeVirtualPath, resourceKey) ?? GetMasterLang(this.Master, resourceKey);
            }
        }

        string IPageLang.this[string classKey, string resourceKey]
        {
            get { return Lang.GetGlobalResourceObject(classKey, resourceKey); }
        }

        internal static string GetMasterLang(MasterPage master, string key)
        {
            if (master != null)
                return Lang.GetLocalResourceObject(master.AppRelativeVirtualPath, key) ?? GetMasterLang(master.Master, key);
            return Lang.GetGlobalResourceObject("res", key) as string;
        }
    }

    public class BaseMasterPage : System.Web.UI.MasterPage, IPageLang
    {
        public new User User
        {
            [DebuggerStepThrough]
            get { return base.Context.User as User; }
        }

        public IPageLang lang
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        string IPageLang.this[string resourceKey]
        {
            get { return (this.Page as BasePage).lang[resourceKey]; } //get { return BasePage.GetMasterLang(this, resourceKey); }
        }

        string IPageLang.this[string classKey, string resourceKey]
        {
            get { return Lang.GetGlobalResourceObject(classKey, resourceKey); }
        }
    }

    public class BaseUserControl : System.Web.UI.UserControl, IPageLang
    {
        public IPageLang lang
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        string IPageLang.this[string resourceKey]
        {
            get { return ((BasePage)this.Page).lang[resourceKey]; } //get { return BasePage.GetMasterLang(this.Page.Master, resourceKey); }
        }

        string IPageLang.this[string classKey, string resourceKey]
        {
            get { return Lang.GetGlobalResourceObject(classKey, resourceKey); }
        }
    }

    public class Lang
    {
        static string fixkey(string resourceKey)
        {
            return (resourceKey ?? "").Trim().Replace(" ", "_").Replace("-", "_");
        }
        public static string GetLocalResourceObject(string virtualPath, string resourceKey)
        {
            resourceKey = (resourceKey ?? "").Trim();
            return GetLang(virtualPath, resourceKey) ?? HttpContext.GetLocalResourceObject(virtualPath, fixkey(resourceKey)) as string;
        }
        public static string GetGlobalResourceObject(string classKey, string resourceKey)
        {
            resourceKey = (resourceKey ?? "").Trim();
            return GetLang(classKey, resourceKey) ?? HttpContext.GetGlobalResourceObject(classKey, fixkey(resourceKey)) as string;
        }

        public static string GetLang(string classKey, string resourceKey)
        {
            Dictionary<int, Dictionary<string, string>> d1;
            if (cache.Instance.Value.TryGetValue(classKey ?? "", out d1))
            {
                for (CultureInfo c = CultureInfo.CurrentUICulture; c.Parent != c; c = c.Parent)
                {
                    Dictionary<string, string> d2;
                    if (d1.TryGetValue(c.LCID, out d2))
                    {
                        string txt;
                        if (d2.TryGetValue(resourceKey, out txt))
                            return txt;
                    }
                }
            }
            return null;
        }

        class cache : WebTools.ObjectCache<cache, Dictionary<string, Dictionary<int, Dictionary<string, string>>>>
        {
            [SqlSetting("Cache", "Lang"), DefaultValue(30 * 60 * 1000)]
            public override double LifeTime
            {
                get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
                set { }
            }
            public override void Update(SqlCmd sqlcmd, string key, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd))
                {
                    Dictionary<string, Dictionary<int, Dictionary<string, string>>> d0 = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Lang nolock"))
                    {
                        string cls = r.GetString("cls");
                        int lcid = r.GetInt32("lcid");
                        string txt1 = r.GetString("txt1");
                        string txt2 = r.GetString("txt2");
                        Dictionary<int, Dictionary<string, string>> d1;
                        if (!d0.TryGetValue(cls, out d1))
                            d1 = d0[cls] = new Dictionary<int, Dictionary<string, string>>();
                        Dictionary<string, string> d2;
                        if (!d1.TryGetValue(lcid, out d2))
                            d2 = d1[lcid] = new Dictionary<string, string>();
                        d2[txt1] = txt2;
                    }
                    base.Value = d0;
                }
            }
        }
    }
}