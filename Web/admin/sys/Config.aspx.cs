using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Tools;

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

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class ConfigSelect : jgrid.GridRequest
    {
        [ObjectInvoke, Permissions(Permissions.Code.config_edit, Permissions.Flag.Read | Permissions.Flag.Write)]
        public static jgrid.GridResponse<ConfigRow> execute(ConfigSelect command, string json_s, params object[] args)
        {
            jgrid.GridResponse<ConfigRow> data = new jgrid.GridResponse<ConfigRow>();
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Config nolock order by Category, [Key], CorpID"))
                    data.rows.Add(r.ToObject<ConfigRow>());
                return data;
            }
        }

        [ObjectInvoke, Permissions(Permissions.Code.config_edit, Permissions.Flag.Write)]
        static ConfigRow insert(ConfigInsert command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                sqltool s = new sqltool();
                s["*", "CorpID", "     "] = command.CorpID ?? 0;
                s["*", "Category", "   "] = text.ValidAsString * command.Category ?? "";
                s["*", "Key", "        "] = text.ValidAsString * command.Key ?? "";
                s[" ", "Value", "      "] = text.ValidAsString * command.Value ?? "";
                s["N", "Description", ""] = text.ValidAsString * command.Description;
                s.TestFieldNeeds();
                string sqlstr = s.BuildEx("insert into Config (", sqltool._Fields, ") values (", sqltool._Values, @") select * from Config nolock where Category={Category} and [Key]={Key}");
                return sqlcmd.ExecuteEx<ConfigRow>(sqlstr);
            }
        }

        [ObjectInvoke, Permissions(Permissions.Code.config_edit, Permissions.Flag.Write)]
        static ConfigRow update(ConfigUpdate command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                ConfigRow row = sqlcmd.GetRowEx<ConfigRow>(RowErrorCode.NotFound, "select * from Config nolock where ID={0}", command.ID);
                sqltool s = new sqltool();
                s[" ", "CorpID", "     ", row.CorpID ?? 0, "   "] = command.CorpID ?? 0;
                s[" ", "Category", "   ", row.Category ?? "", ""] = text.ValidAsString * command.Category ?? "";
                s[" ", "Key", "        ", row.Key ?? "", "     "] = text.ValidAsString * command.Key ?? "";
                s[" ", "Value", "      ", row.Value ?? "", "   "] = text.ValidAsString * command.Value ?? "";
                s["N", "Description", "", row.Description, "   "] = text.ValidAsString * command.Description;
                if (s.fields.Count == 0) return row;
                s.Values["ID"] = row.ID;
                string sqlstr = s.BuildEx("update Config set ", sqltool._FieldValue, " where ID={ID} select * from Config nolock where ID={ID}");
                return sqlcmd.ExecuteEx<ConfigRow>(sqlstr);
            }
        }

        [ObjectInvoke, Permissions(Permissions.Code.config_edit, Permissions.Flag.Write)]
        static ConfigRow delete(ConfigDelete command, string json_s, params object[] args)
        {
            if (command.ID.HasValue)
                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                    return sqlcmd.ExecuteEx<ConfigRow>("select * from Config nolock where ID={0} delete Config where ID={0}", command.ID);
            return null;
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class ConfigUpdate : ConfigRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class ConfigInsert : ConfigRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class ConfigDelete : ConfigRowCommand, IRowCommand { }

    class ConfigRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual string Category { get; set; }
        [JsonProperty]
        public virtual string Key { get; set; }
        [JsonProperty]
        public virtual string Value { get; set; }
        [JsonProperty]
        public virtual string Description { get; set; }
    }
}