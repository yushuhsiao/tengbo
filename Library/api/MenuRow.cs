using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using web;


[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
[System.Diagnostics.DebuggerDisplay("{Code},ID:{ID},Parent:{Parent}")]
public class MenuRow
{
    [JsonProperty, DbImport]
    public int? ID;
    [JsonProperty, DbImport]
    public int? Parent;
    [JsonProperty, DbImport]
    public int? Sort;
    [JsonProperty, DbImport]
    public string Code;
    [JsonProperty, DbImport]
    public string Name;
    [JsonProperty, DbImport]
    public string Url;
    [DbImport, JsonProperty]
    public Permissions.Flag Flag;

    [JsonProperty]
    bool[] Flags
    {
        get
        {
            int n = (int)this.Flag;
            bool[] ret = new bool[32];
            for (int i = 0; i < ret.Length; i++)
            {
                if ((n % 2) == 1)
                    ret[i] = true;
                n >>= 1;
            }
            return ret;
        }
    }

    public List<MenuRow> Childs;

    public const int DisplayCount = 32;
    public const string menu_root = "menu_root";
    public const string admin_root = "admin_root";
    public const string agent_root = "agent_root";
    public const string member_root = "member_root";
    public const string other_root = "other_root";
    public static string[] _root = new string[] { MenuRow.menu_root, MenuRow.admin_root, MenuRow.agent_root, MenuRow.member_root, MenuRow.other_root };

    public class Cache : WebTools.ObjectCache<Cache, Dictionary<string, MenuRow>>
    {
        [SqlSetting("Cache", "MenuRow"), DefaultValue(30000)]
        public override double LifeTime
        {
            get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
            set { }
        }

        public override void Update(SqlCmd sqlcmd, string key, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd ?? args.GetValue<SqlCmd>(0)))
            {
                Dictionary<int, MenuRow> rows1 = new Dictionary<int, MenuRow>();
                Dictionary<string, MenuRow> rows2 = new Dictionary<string, MenuRow>();
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Permission1 nolock order by Sort"))
                {
                    MenuRow row = r.ToObject<MenuRow>();
                    rows1[row.ID.Value] = row;
                    rows2[row.Code] = row;
                }
                foreach (MenuRow row in rows1.Values)
                {
                    MenuRow parentRow;
                    if (rows1.TryGetValue(row.Parent.Value, out parentRow))
                    {
                        if (parentRow.Childs == null)
                            parentRow.Childs = new List<MenuRow>();
                        parentRow.Childs.Add(row);
                    }
                }
                base.Value = rows2;
            }
        }

        public MenuRow GetItem(User user, string key)
        {
            if (user.Permissions[key])
                return this.GetItem(key);
            return null;
        }
        public MenuRow GetItem(string key)
        {
            MenuRow row;
            base.Value.TryGetValue(key, out row);
            return row;
        }
    }
}
