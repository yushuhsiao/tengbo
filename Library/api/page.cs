using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using web;

namespace web
{
    public class page : web.BasePage
    {
        public string enumlist<T>(string propertyName) where T : struct
        {
            return page.enumlist<T>(this, propertyName);
        }
        public string enumlist<T>(string propertyName, bool ignoreResourceMissing) where T : struct
        {
            return page.enumlist<T>(this, propertyName, ignoreResourceMissing);
        }
        public string enumlist<T>(string propertyName, bool ignoreResourceMissing, params T[] items) where T : struct
        {
            return page.enumlist<T>(this, propertyName, ignoreResourceMissing, items);
        }
        public string serializeEnum<TKey, TValue>(string propertyName, Dictionary<TKey, TValue> dict)
        {
            return page.serializeEnum<TKey, TValue>(this, propertyName, dict);
        }

        internal static string enumlist<T>(IPageLang lang, string propertyName) where T : struct
        {
            return page.enumlist<T>(lang, propertyName, true);
        }

        internal static string enumlist<T>(IPageLang lang, string propertyName, bool ignoreResourceMissing) where T : struct
        {
            if (typeof(T).IsEnum)
            {
                Array a = Enum.GetValues(typeof(T));
                T[] b = new T[a.Length];
                a.CopyTo(b, 0);
                return enumlist(lang, propertyName, ignoreResourceMissing, b);
            }
            return null;
        }

        internal static string enumlist<T>(IPageLang lang, string propertyName, bool ignoreResourceMissing, params T[] items) where T : struct
        {
            Dictionary<T, string> dict = new Dictionary<T, string>();
            foreach (T t in items)
            {
                string key = typeof(T).Name + "_" + t.ToString();
                string txt = lang[key];
                if (string.IsNullOrEmpty(txt))
                    if (ignoreResourceMissing)
                        txt = t.ToString();
                    else
                        continue;
                dict[t] = txt;
            }
            return serializeEnum(lang, propertyName, dict);
        }

        internal static string serializeEnum<TKey, TValue>(IPageLang lang, string propertyName, Dictionary<TKey, TValue> dict)
        {
            string json = api.SerializeObject(dict);
            if (string.IsNullOrEmpty(propertyName))
                return json;
            return string.Format("{0}:{1}", propertyName, json);
        }

        public bool showID
        {
            get { return User.Permissions[BU.Permissions.Code.develover]; }
        }

        public bool showCorpID
        {
            get { return this.User.CorpID == 0; }
        }
    }

    public class masterpage : web.BaseMasterPage
    {
        public string enumlist<T>(string propertyName) where T : struct
        {
            return page.enumlist<T>(this, propertyName);
        }
        public string enumlist<T>(string propertyName, bool ignoreResourceMissing) where T : struct
        {
            return page.enumlist<T>(this, propertyName, ignoreResourceMissing);
        }
        public string enumlist<T>(string propertyName, bool ignoreResourceMissing, params T[] items) where T : struct
        {
            return page.enumlist<T>(this, propertyName, ignoreResourceMissing, items);
        }
        public string serializeEnum<TKey, TValue>(string propertyName, Dictionary<TKey, TValue> dict)
        {
            return page.serializeEnum<TKey, TValue>(this, propertyName, dict);
        }

        public bool showID
        {
            get { return User.Permissions[BU.Permissions.Code.develover]; }
        }

        public bool showCorpID
        {
            get { return this.User.CorpID == 0; }
        }
    }

    public class usercontrol : web.BaseUserControl
    {
        public string enumlist<T>(string propertyName) where T : struct
        {
            return page.enumlist<T>(this, propertyName);
        }
        public string enumlist<T>(string propertyName, bool ignoreResourceMissing) where T : struct
        {
            return page.enumlist<T>(this, propertyName, ignoreResourceMissing);
        }
        public string enumlist<T>(string propertyName, bool ignoreResourceMissing, params T[] items) where T : struct
        {
            return page.enumlist<T>(this, propertyName, ignoreResourceMissing, items);
        }
        public string serializeEnum<TKey, TValue>(string propertyName, Dictionary<TKey, TValue> dict)
        {
            return page.serializeEnum<TKey, TValue>(this, propertyName, dict);
        }

        public bool showID
        {
            get { return ((BasePage)this.Page).User.Permissions[BU.Permissions.Code.develover]; }
        }

        public bool showCorpID
        {
            get { return ((BasePage)this.Page).User.CorpID == 0; }
        }
    }
}
