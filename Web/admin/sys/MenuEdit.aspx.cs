using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Linq;
using web;
using Tools.Protocol;
using System.Threading;
using System.Diagnostics;
using System.Linq;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [System.Diagnostics.DebuggerDisplay("{Code},ID:{ID},Parent:{Parent}")]
    public class MenuRow
    {
        [JsonProperty, DbImport]
        public int ID;
        [JsonProperty, DbImport]
        public int Parent;
        [JsonProperty, DbImport("Flag1")]
        public bool IsMenu;
        [JsonProperty, DbImport]
        public bool Flag2;
        [JsonProperty, DbImport]
        public bool Flag3;
        [JsonProperty, DbImport]
        public bool Flag4;
        [JsonProperty, DbImport]
        public int Sort;
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
        //public const string menu_root = "menu_root";
        //public const string admin_root = "admin_root";
        //public const string agent_root = "agent_root";
        //public const string member_root = "member_root";
        //public const string other_root = "other_root";
        //public static string[] _root = new string[] { MenuRow.menu_root, MenuRow.admin_root, MenuRow.agent_root, MenuRow.member_root, MenuRow.other_root };

        public string GetLabel(int lcid = 0)
        {
            return Lang.GetLang("menu", this.Code, lcid) ?? this.Name;
        }

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
                    Dictionary<string, MenuRow> rows1 = new Dictionary<string, MenuRow>();
                    Dictionary<int, MenuRow> rows2 = new Dictionary<int, MenuRow>();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Permission1 nolock order by Sort asc"))
                    {
                        MenuRow row = r.ToObject<MenuRow>();
                        rows1[row.Code] = rows2[row.ID] = row;
                    }
                    MenuRow root = new MenuRow()
                    {
                        ID = 0,
                        Parent = 0,
                        Sort = 0,
                        Code = "",
                        Name = "Root Menu",
                        Childs = new List<MenuRow>(),
                    };
                    rows2[0] = root;
                    MenuRow missing = null;
                    foreach (MenuRow row in rows1.Values)
                    {
                        if (row.ID == 0) continue;
                        MenuRow parent;
                        if (rows2.TryGetValue(row.Parent, out parent))
                        {
                            if (parent.Childs == null)
                                parent.Childs = new List<MenuRow>();
                        }
                        else
                        {
                            if (missing == null)
                            {
                                missing = new MenuRow()
                                {
                                    ID = -1,
                                    Parent = 0,
                                    Sort = 0,
                                    Code = "",
                                    Name = "Missing",
                                    Childs = new List<MenuRow>(),
                                };
                            }
                            parent = missing;
                        }
                        //row.Hidden = false;
                        parent.Childs.Add(row);
                    }
                    if (missing != null)
                        root.Childs.Add(missing);
                    //foreach (var r1 in from row in rows1.Values
                    //                   where row.ID != 0
                    //                   orderby row.Sort
                    //                   group row by row.Parent into grp
                    //                   select grp)
                    //{
                    //    foreach (MenuRow r2 in from q2 in rows1.Values
                    //                           where q2.ID == r1.Key
                    //                           select q2)
                    //    {
                    //        r2.Childs = r1.ToList();
                    //        break;
                    //    }
                    //}
                    Interlocked.Exchange(ref _root, root);
                    base.Value = rows1;
                }
            }

            MenuRow _root;
            public MenuRow Root
            {
                [DebuggerStepThrough]
                get { return Interlocked.CompareExchange(ref _root, null, null); }
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

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MenuRowCommand : IRowCommand
    {
        [JsonProperty]
        string oper;
        [JsonProperty]
        public int? ID;
        [JsonProperty]
        public int? Parent;
        [JsonProperty]
        public bool? IsMenu;
        [JsonProperty]
        public int? Sort;
        [JsonProperty]
        public string Name;
        [JsonProperty, JsonProtocol.String(Empty = false, Trim = true)]
        public string Code;
        [JsonProperty, JsonProtocol.String(Empty = false, Trim = true)]
        public string Url;

        [ObjectInvoke, Permissions(Permissions.Code.menu_edit, Permissions.Flag.Write)]
        static MenuRow execute(MenuRowCommand command, string json_s, params object[] args)
        {
            if (command.oper == jgrid.oper.insert)
            #region ...
            {
                string code = (text.ValidAsString * command.Code) ?? Guid.NewGuid().ToString();
                string sqlstr;
                if (code.StartsWith("=") && command.Parent.HasValue)
                {
                    code = code.Substring(1).Trim();
                    sqlstr = string.Format("update Permission1 set Parent={0} where Code='{1}' select * from Permission1 nolock where Code='{1}'", command.Parent, code);
                }
                else
                {
                    sqltool s = new sqltool();
                    s[" ", "Parent", ""] = command.Parent ?? 0;
                    s[" ", "Flag1", ""] = text.Sql_Bool(command.IsMenu);
                    s[" ", "Sort", "  "] = command.Sort ?? 0;
                    s[" ", "Code", "  "] = code;
                    s[" ", "Name", "  "] = text.ValidAsString * command.Name;
                    s[" ", "Url", "   "] = text.ValidAsString * command.Url;
                    s.TestFieldNeeds();
                    sqlstr = s.BuildEx("insert into Permission1 (", sqltool._Fields, ") values (", sqltool._Values, ") select * from Permission1 nolock where ID=@@Identity");
                }
                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                    return sqlcmd.ExecuteEx<MenuRow>(sqlstr);
            }
            #endregion
            if (command.oper == jgrid.oper.update)
            #region ...
            {
                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                {
                    MenuRow row = sqlcmd.GetRowEx<MenuRow>(RowErrorCode.NotFound, "select * from Permission1 nolock where ID={0}", command.ID);
                    sqltool s = new sqltool();
                    int isMenu = text.Sql_Bool(row.IsMenu);
                    object url = text.ValidAsString * command.Url;
                    s[" ", "Parent", "", row.Parent, ""] = command.Parent;
                    s[" ", "Flag1 ", "", isMenu, "    "] = text.Sql_Bool(command.IsMenu);
                    s[" ", "Sort", "  ", row.Sort, "  "] = command.Sort ?? 99;
                    s[" ", "Code", "  ", row.Code, "  "] = text.ValidAsString * command.Code;
                    s[" ", "Name", "  ", row.Name, "  "] = text.ValidAsString * command.Name;
                    s[" ", "Url", "   ", row.Url, "   "] = url ?? StringEx.sql_str.Null;
                    if (s.fields.Count == 0) return row;
                    s.Values["ID"] = command.ID;
                    string sqlstr = s.BuildEx2("update Permission1 set ", sqltool._FieldValue, " where ID={ID} select * from Permission1 nolock where ID={ID}");
                    return sqlcmd.ExecuteEx<MenuRow>(sqlstr);
                }
            }
            #endregion
            if (command.oper == jgrid.oper.delete)
            #region ...
            {
                if (command.ID.HasValue)
                    using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                        return sqlcmd.ExecuteEx<MenuRow>("select * from Permission1 nolock where ID={0} delete Permission1 where ID={0}", command.ID);
                return null;
            }
            #endregion
            throw new ArgumentException("oper");
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MenuSelect : jgrid.GridRequest<AgentTreeSelect>
    {
        protected override string init_defaultkey() { return "Sort"; }
        protected override Dictionary<string, string> init_sortkeys()
        {
            return new Dictionary<string, string>() { { "Sort", "Sort" }, };
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class MenuTreeRow : MenuRow
        {
            [DbImport, JsonProperty]
            public int? level = 0;
            [JsonProperty]
            public bool? isLeaf;
            [JsonProperty]
            public bool? expanded;
            [JsonProperty]
            public bool? loaded;
        }

        [JsonProperty]
        public int? nodeid;
        [JsonProperty]
        public int? n_level;

        //public static MenuTreeRow getAgent(SqlCmd sqlcmd, int agent_ID, int parentID)
        //{
        //    return sqlcmd.ToObject<MenuTreeRow>("select ID,CorpID,ACNT,ParentID,Name,dbo.getAgentLevel(ID,{1}) level from Agent nolock where ID={0}", agent_ID, parentID);
        //}
        //public static void getAgents(SqlCmd sqlcmd, List<MenuTreeRow> list, int parentID, int corpID, int setLevel)
        //{
        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendFormat("select ID,CorpID,ACNT,ParentID,Name,{1} level from Agent nolock where ParentID={0}", parentID, setLevel);
        //    if (corpID != 0)
        //        sql.AppendFormat(" and CorpID={0}", corpID);
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.ToString()))
        //        list.Add(r.ToObject<MenuTreeRow>());
        //}

        [ObjectInvoke, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write | Permissions.Flag.Read)]
        static jgrid.GridResponse<MenuTreeRow> execute(MenuSelect command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<MenuTreeRow> data = new jgrid.GridResponse<MenuTreeRow>();

                int level = (command.n_level ?? 0) + 1;
                command.nodeid = command.nodeid ?? 0;
                if (command.nodeid == -1)
                {
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select 2 as [level], a.*
from Permission1 a with(nolock) left join Permission1 b with(nolock)
on a.Parent=b.ID
where a.Parent <> 0 and b.Code is null order by a.ID"))
                    {
                        MenuTreeRow row = r.ToObject<MenuTreeRow>();
                        row.Parent = -1;
                        data.rows.Add(row);
                    }
                }
                else
                {
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select dbo.getMenuLevel(ID) as [level], * from Permission1 nolock where Parent={0} order by Sort", command.nodeid))
                    {
                        MenuTreeRow row = r.ToObject<MenuTreeRow>();
                        data.rows.Add(row);
                    }
                    if (command.nodeid == 0)
                    {
                        data.rows.Add(new MenuTreeRow()
                        {
                            ID = -1,
                            level = 1,
                            Sort = -1,
                            Name = "[Other]",
                        });
                    }
                }

//                StringBuilder sql = new StringBuilder();
//                sql.Append(@"select dbo.getMenuLevel(a.ID) as [level], a.*, b.Code as ParentCode
//from Permission1 a with(nolock) left join Permission1 b with(nolock)
//on a.Parent=b.ID where ");
//                if (command.nodeid.HasValue)
//                    sql.AppendFormat(" b.ID={0}", command.nodeid);
//                else
//                    sql.AppendFormat(" b.Code is null");
//                sql.Append(" order by a.Sort");

//                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.ToString()))
//                {
//                    MenuTreeRow row = r.ToObject<MenuTreeRow>();
//                    //row.level = level;
//                    //row.loaded = false;
//                    //row.expanded = false;
//                    data.rows.Add(row);
//                }
                return data;
            }
        }
    }


    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class MenuSelect1 : jgrid.GridRequest
    //{
    //    [JsonProperty]
    //    public int? Parent;

    //    [ObjectInvoke, Permissions(Permissions.Code.menu_edit, Permissions.Flag.Write | Permissions.Flag.Read)]
    //    public static jgrid.GridResponse<MenuRow> select(MenuSelect1 command, string json_s, params object[] args)
    //    {
    //        jgrid.GridResponse<MenuRow> data = new jgrid.GridResponse<MenuRow>();
    //        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
    //        {
    //            int parent = command.Parent ?? 0;
    //            string code = sqlcmd.ExecuteScalar("select Code from Permission1 nolock where ID={0}", parent) as string;
    //            if (code == MenuRow.other_root)
    //            {
    //                Dictionary<int, MenuRow> rows = new Dictionary<int, MenuRow>();
    //                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Permission1 nolock order by Sort"))
    //                    rows[r.GetInt32("ID")] = r.ToObject<MenuRow>();
    //                foreach (MenuRow row in rows.Values)
    //                {
    //                    if (MenuRow._root.Conatins(row.Code)) continue;
    //                    MenuRow r1 = row, r2;
    //                    while (rows.TryGetValue(r1.Parent, out r2))
    //                        r1 = r2;
    //                    if (r1.Code == MenuRow.admin_root) continue;
    //                    if (r1.Code == MenuRow.agent_root) continue;
    //                    if (r1.Code == MenuRow.member_root) continue;
    //                    if (r1.Code == MenuRow.menu_root) continue;
    //                    data.rows.Add(row);
    //                }
    //            }
    //            else
    //            {
    //                StringBuilder sql = new StringBuilder("select * from Permission1 nolock where ");
    //                if (parent == 0)
    //                {
    //                    sql.Append("Code in (''");
    //                    foreach (string s in MenuRow._root)
    //                        sql.AppendFormat(",'{0}'", s);
    //                    sql.Append(")");
    //                }
    //                else
    //                {
    //                    sql.AppendFormat("Parent={0}", command.Parent);
    //                }
    //                sql.Append(" order by Sort asc");
    //                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.ToString()))
    //                    data.rows.Add(r.ToObject<MenuRow>());
    //            }
    //            return data;
    //        }
    //    }
    //}
}

abstract class PermissionGroupSelect : jgrid.GridRequest
{
    static void GetMenu<T>(List<T> m1, List<T> m2, int? parent, string fill, string prefix) where T : MenuRow
    {
        foreach (T m in (from m0 in m1 where m0.Parent == parent orderby m0.Sort select m0))
        {
            m2.Add(m);
            while (m1.Remove(m)) { }
            m.Name = string.Format("{0}{1}", fill, m.Name);
            m.Code = string.Format("{0}{1}", prefix, m.Code);
            GetMenu(m1, m2, m.ID, fill + "-- ", prefix);
        }
    }

    protected abstract UserType UserType { get; }
    [JsonProperty("GroupID")]
    public Guid? GroupID { get; set; }
    //public virtual long? _in_GroupID { get; set; }
    //public int? CorpID
    //{
    //    get { return text.GroupRowID_CorpID(this._in_GroupID); }
    //}
    //public byte? GroupID
    //{
    //    get { return text.GroupRowID_GroupID(this._in_GroupID); }
    //}

    protected jgrid.GridResponse<MenuRow> execute(string json_s, params object[] args)
    {
        sqltool s = new sqltool();
        s["*", "UserType", ""] = (byte?)this.UserType;
        s["*", "CorpID", "  "] = this.CorpID;
        s["*", "GroupID", " "] = this.GroupID;
        s.TestFieldNeeds();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<MenuRow> data = new jgrid.GridResponse<MenuRow>();
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(s.BuildEx(@"select a.*, isnull(b.Flag,0) as Flag from Permission1 a with(nolock) left join Permission2 b with(nolock) on a.ID=b.CodeID and b.UserType={UserType} and b.CorpID={CorpID} and b.GroupID={GroupID} order by a.Sort")))
                data.rows.Add(r.ToObject<MenuRow>());
            return data;
        }
    }
}

abstract class PermissionGroupUpdate
{
    [JsonProperty]
    public int? ID;
    protected abstract UserType UserType { get; }
    [JsonProperty]
    public Guid? GroupID { get; set; }
    //[JsonProperty("GroupID")]
    //public virtual long? _in_GroupID { get; set; }
    //public int? CorpID
    //{
    //    get { return text.GroupRowID_CorpID(this._in_GroupID); }
    //}
    //public byte? GroupID
    //{
    //    get { return text.GroupRowID_GroupID(this._in_GroupID); }
    //}
    //[JsonProperty]
    //public Permissions.Flag? Flag;

    public MenuRow update(string json_s, params object[] args)
    {
        Dictionary<string, string> dict = api.DeserializeObject<Dictionary<string, string>>(json_s);
        int flags = 0;
        for (int i = 0; i < 32; i++)
        {
            string name = string.Format("Flag{0:00}", i);
            string v1;
            if (dict.TryGetValue(name, out v1))
            {
                Locked? v2 = v1 * text.ValidAsLocked2;
                if (v2 == Locked.Locked)
                {
                    int f = 1 << i;
                    flags |= f;
                }
            }
        }
        sqltool s = new sqltool();
        s["*", "ID", "          "] = this.ID;
        s["*", "UserType", "    "] = (int?)this.UserType;
        //s["*", "CorpID", "      "] = this.CorpID;
        s["*", "GroupID", "     "] = this.GroupID;
        s[" ", "Flag", "        "] = flags;
        s.TestFieldNeeds();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            return sqlcmd.ExecuteEx<MenuRow>(s.BuildEx(@"
if exists (select Flag from Permission2 nolock where UserType={UserType} and CorpID={CorpID} and GroupID={GroupID} and CodeID={ID}) 
     update Permission2 set Flag={Flag} where UserType={UserType} and CorpID={CorpID} and GroupID={GroupID} and CodeID={ID}
else insert into Permission2 (UserType,CorpID,GroupID,CodeID,Flag) values ({UserType},{CorpID},{GroupID},{ID},{Flag})
select a.*, b.Flag from Permission1 a with(nolock) left join Permission2 b with(nolock) on a.ID=b.CodeID where b.UserType={UserType} and b.CorpID={CorpID} and b.GroupID={GroupID} and b.CodeID={ID}"));
        }
    }
}