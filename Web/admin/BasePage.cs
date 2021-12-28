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
        string this[string key2] { get; }
        string this[string key1, string key2] { get; }
        string this[string key1, string key2, int lcid] { get; }
        IPageLang Parent { get; }
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

        string IPageLang.this[string key2]
        {
            get { return Lang.GetLang(this, key2, 0); }
            //get { return ((IPageLang)this)[this.AppRelativeVirtualPath, key2, 0]; }
            //string s = Lang.GetLocalResourceObject(this.AppRelativeVirtualPath, resourceKey);
            //for (MasterPage m = this.Master; (s == null) && (m != null); m = m.Master)
            //    s = Lang.GetLocalResourceObject(m.AppRelativeVirtualPath, resourceKey);
            //s = s ?? Lang.GetGlobalResourceObject("res", resourceKey);
            //return s;
            //return Lang.GetLocalResourceObject(this.AppRelativeVirtualPath, resourceKey) ?? GetMasterLang(this.Master, resourceKey);
        }

        string IPageLang.this[string key1, string key2]
        {
            get { return Lang.GetLang(key1, key2, 0); }
        }

        string IPageLang.this[string key1, string key2, int lcid]
        {
            get { return Lang.GetLang(key1, key2, lcid); }
            //get
            //{
            //    string ret = Lang.GetLang(key1, key2, lcid);
            //    if (ret != null) return ret;
            //    IPageLang master = this.Master as IPageLang;
            //    if (master != null)
            //        return master[key1, key2, lcid];
            //    return null;
            //}
        }

        //internal static string GetMasterLang(MasterPage master, string key)
        //{
        //    return null;
        //    //if (master != null)
        //    //    return Lang.GetLocalResourceObject(master.AppRelativeVirtualPath, key) ?? GetMasterLang(master.Master, key);
        //    //return Lang.GetGlobalResourceObject("res", key) as string;
        //}

        IPageLang IPageLang.Parent
        {
            get { return this.Master as IPageLang; }
        }
    }

    public class BaseMasterPage : System.Web.UI.MasterPage, IPageLang
    {
        public User User
        {
            [DebuggerStepThrough]
            get { return base.Context.User as User; }
        }

        public IPageLang lang
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        string IPageLang.this[string key2]
        {
            get { return Lang.GetLang(this, key2, 0); }
        }

        string IPageLang.this[string key1, string key2]
        {
            get { return Lang.GetLang(key1, key2, 0); }
        }

        string IPageLang.this[string key1, string key2, int lcid]
        {
            get { return Lang.GetLang(key1, key2, lcid); }
            //get
            //{
            //    string ret = Lang.GetLang(key1, key2, lcid);
            //    if (ret != null) return ret;
            //    IPageLang master = this.Master as IPageLang;
            //    if (master != null)
            //        return master[key1, key2, lcid];
            //    return null;
            //}
        }

        IPageLang IPageLang.Parent
        {
            get { return this.Master as IPageLang; }
        }
    }

    public class BaseUserControl : System.Web.UI.UserControl, IPageLang
    {
        public IPageLang lang
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        string IPageLang.this[string key2]
        {
            get { return Lang.GetLang(this, key2, 0); }
        }

        string IPageLang.this[string key1, string key2]
        {
            get { return Lang.GetLang(key1, key2, 0); }
        }

        string IPageLang.this[string key1, string key2, int lcid]
        {
            get { return Lang.GetLang(key1, key2, lcid); }
        }

        IPageLang IPageLang.Parent
        {
            get { return this.Page as IPageLang; }
        }
    }

    public abstract class BaseWebControl : System.Web.UI.Control, IPageLang
    {
        public IPageLang lang
        {
            [DebuggerStepThrough]
            get { return this; }
        }

        string IPageLang.this[string key2]
        {
            get { return Lang.GetLang(this, key2, 0); }
        }

        string IPageLang.this[string key1, string key2]
        {
            get { return Lang.GetLang(key1, key2, 0); }
        }

        string IPageLang.this[string key1, string key2, int lcid]
        {
            get { return Lang.GetLang(key1, key2, lcid); }
        }

        IPageLang IPageLang.Parent
        {
            get { return this.Page as IPageLang; }
        }

    }

    public class Lang
    {
        //static string fixkey(string key2)
        //{
        //    return (key2 ?? "").Trim().Replace(" ", "_").Replace("-", "_");
        //}
        //public static string GetLocalResourceObject(string virtualPath, string resourceKey)
        //{
        //    resourceKey = (resourceKey ?? "").Trim();
        //    return GetLang(virtualPath, resourceKey) ?? HttpContext.GetLocalResourceObject(virtualPath, fixkey(resourceKey)) as string;
        //}
        //public static string GetGlobalResourceObject(string classKey, string resourceKey)
        //{
        //    resourceKey = (resourceKey ?? "").Trim();
        //    return GetLang(classKey, resourceKey) ?? HttpContext.GetGlobalResourceObject(classKey, fixkey(resourceKey)) as string;
        //}

        static string getLang(string key1, string key2, int lcid = 0)
        {
            string _key1 = key1 ?? "";
            item1 n1 = cache.Instance.Value;
            item2 n2;
            for (; ; )
            {
                if (cache.Instance.Value.TryGetValue(_key1, out n2))
                {
                    item3 n3;
                    if (n2.TryGetValue(key2, out n3))
                    {
                        if (lcid != 0)
                            if (n3.ContainsKey(lcid))
                                return n3[lcid];
                        for (CultureInfo c = CultureInfo.CurrentUICulture; c.Parent != c; c = c.Parent)
                            if (n3.ContainsKey(c.LCID))
                                return n3[c.LCID];
                    }
                }
                if (string.IsNullOrEmpty(_key1))
                    break;
                else
                    _key1 = "";
            }
            return null;
        }

        public static string GetLang(string key1, string key2, int lcid = 0)
        {
            key2 = (key2 ?? "").Trim();
            return getLang(key1, key2, lcid);
        }
        public static string GetLang(IPageLang page, string key2, int lcid = 0)
        {
            key2 = (key2 ?? "").Trim();
            for (IPageLang p = page; p != null; p = p.Parent)
            {
                TemplateControl c = p as TemplateControl;
                if (c == null) break;
                string ret = getLang(c.AppRelativeVirtualPath, key2, 0);
                if (ret != null) return ret;
            }
            return null;
        }

        //public static string GetLang(string key1, string key2, int lcid = 0)
        //{
        //    string _key1 = key1 ?? "";
        //    item1 n1 = cache.Instance.Value;
        //    item2 n2;
        //    for (; ; )
        //    {
        //        if (cache.Instance.Value.TryGetValue(_key1, out n2))
        //        {
        //            item3 n3;
        //            if (n2.TryGetValue(key2, out n3))
        //            {
        //                if (lcid != 0)
        //                    if (n3.ContainsKey(lcid))
        //                        return n3[lcid];
        //                for (CultureInfo c = CultureInfo.CurrentUICulture; c.Parent != c; c = c.Parent)
        //                    if (n3.ContainsKey(c.LCID))
        //                        return n3[c.LCID];
        //            }
        //        }
        //        if (string.IsNullOrEmpty(_key1))
        //            break;
        //        else
        //            _key1 = "";
        //    }
        //    //Dictionary<int, Dictionary<string, string>> d1;
        //    //if (cache.Instance.Value.TryGetValue(classKey ?? "", out d1))
        //    //{
        //    //    for (CultureInfo c = CultureInfo.CurrentUICulture; c.Parent != c; c = c.Parent)
        //    //    {
        //    //        Dictionary<string, string> d2;
        //    //        if (d1.TryGetValue(c.LCID, out d2))
        //    //        {
        //    //            string txt;
        //    //            if (d2.TryGetValue(resourceKey, out txt))
        //    //                return txt;
        //    //        }
        //    //    }
        //    //}
        //    return null;
        //}

        // lcid, txt2
        class item3 : Dictionary<int, string> { }

        // txt1, item1
        class item2 : Dictionary<string, item3> { }

        // cls, item2
        class item1 : Dictionary<string, item2> { }

        class cache : WebTools.ObjectCache<cache, item1>
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
                    item1 n1 = new item1();
                    Dictionary<string, Dictionary<int, Dictionary<string, string>>> d0 = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Lang nolock"))
                    {
                        int lcid = r.GetInt32("lcid");
                        string key1 = r.GetString("key1");
                        string key2 = r.GetString("key2");
                        string text = r.GetString("text");

                        item2 n2;
                        if (!n1.TryGetValue(key1, out n2))
                            n2 = n1[key1] = new item2();

                        item3 n3;
                        if (!n2.TryGetValue(key2, out n3))
                            n3 = n2[key2] = new item3();

                        n3[lcid] = text;

                        //Dictionary<int, Dictionary<string, string>> d1;
                        //if (!d0.TryGetValue(cls, out d1))
                        //    d1 = d0[cls] = new Dictionary<int, Dictionary<string, string>>();
                        //Dictionary<string, string> d2;
                        //if (!d1.TryGetValue(lcid, out d2))
                        //    d2 = d1[lcid] = new Dictionary<string, string>();
                        //d2[txt1] = txt2;
                    }
                    base.Value = n1;
                    //base.Value = d0;
                }
            }
        }
    }
}