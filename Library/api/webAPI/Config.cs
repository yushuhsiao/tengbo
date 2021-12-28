using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Tools;
using web;

namespace web
{
    public class ConfigRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public string Category;
        [DbImport, JsonProperty]
        public string Key;
        [DbImport, JsonProperty]
        public string Value;
        [DbImport, JsonProperty]
        public string Description;

        public class Cache : WebTools.ObjectCache<Cache, Dictionary<int, Dictionary<string, Dictionary<string, string>>>>
        {
            const double _LifeTime = 60 * 60 * 1000;
            [DefaultValue(_LifeTime)]
            public override double LifeTime
            {
                get { return base.LifeTime; }
                set { base.LifeTime = value; }
            }

            public override void Update(SqlCmd sqlcmd, string key, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd ?? args.GetValue<SqlCmd>(0)))
                {
                    Dictionary<int, Dictionary<string, Dictionary<string, string>>> data = new Dictionary<int, Dictionary<string, Dictionary<string, string>>>();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Config nolock"))
                    {
                        ConfigRow row = r.ToObject<ConfigRow>();
                        row.CorpID = row.CorpID ?? 0;
                        row.Category = row.Category ?? "";
                        Dictionary<string, Dictionary<string, string>> d1;
                        if (!data.TryGetValue(row.CorpID.Value, out d1))
                            d1 = data[row.CorpID.Value] = new Dictionary<string, Dictionary<string, string>>();
                        Dictionary<string, string> d2;
                        if (!d1.TryGetValue(row.Category, out d2))
                            d2 = d1[row.Category] = new Dictionary<string, string>();
                        d2[row.Key ?? ""] = row.Value;
                        if ((row.CorpID == 0) && (row.Category == "Cache") && (row.Key == "ConfigReload"))
                            base.LifeTime = row.Value.ToDouble() ?? _LifeTime;
                    }
                    base.Value = data;
                }
            }

            public Dictionary<string, Dictionary<string, string>> CorpConfig(int corpID)
            {
                Dictionary<string, Dictionary<string, string>> result;
                if (this.Value.TryGetValue(corpID, out result))
                    return result;
                return _null<Dictionary<string, Dictionary<string, string>>>.value;
            }
        }
    }
}

namespace System.Configuration
{
    public class SqlSettingAttribute : DataBaseSettingAttribute
    {
        //static Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
        //public static void Load(SqlCmd _sqlcmd)
        //{
        //    Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
        //    SqlCmd sqlcmd;
        //    using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, _sqlcmd))
        //    {
        //        List<ConfigRow> rows = new List<ConfigRow>();
        //        foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select Category,[Key],Value from Config nolock"))
        //            rows.Add(r.ToObject<ConfigRow>());
        //        foreach (ConfigRow row in rows)
        //        {
        //            row.Category = (row.Category ?? "").Trim();
        //            row.Key = (row.Key ?? "").Trim();
        //            row.Value = (row.Value ?? "").Trim();
        //            Dictionary<string, string> d;
        //            if (!dict.TryGetValue(row.Category, out d))
        //                dict[row.Category] = d = new Dictionary<string, string>();
        //            d[row.Key] = row.Value;
        //        }
        //    }
        //    Interlocked.Exchange(ref SqlSettingAttribute.dict, dict);
        //}

        public string Category { get; set; }
        public string Key { get; set; }
        public SqlSettingAttribute() { }
        public SqlSettingAttribute(string key) : this(null, key) { }
        public SqlSettingAttribute(string category, string key) { this.Category = category; this.Key = key; }

        protected override bool GetValue(MemberInfo m, int index, out string result)
        {
            Dictionary<string, Dictionary<string, string>> d1;
            Dictionary<string, string> d2;
            if (ConfigRow.Cache.Instance.Value.TryGetValue(index, out d1))
                if (d1.TryGetValue(this.Category ?? "", out d2))
                    return d2.TryGetValue(this.Key ?? m.Name, out result);
            //Dictionary<string, Dictionary<string, string>> dict = Interlocked.CompareExchange(ref SqlSettingAttribute.dict, null, null);
            //Dictionary<string, string> d;
            //if (dict.TryGetValue(this.Category ?? "", out d))
            //    return d.TryGetValue(this.Key ?? m.Name, out result);
            //result = null;
            result = null;
            return false;
        }
    }
}
