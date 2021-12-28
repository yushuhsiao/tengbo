using BU;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;
using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using Tools.Protocol;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GameRow
    {
        [DbImport, JsonProperty]
        public GameID? ID;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public GameLocked? Locked;
        [DbImport, JsonProperty]
        public decimal? BonusW;
        [DbImport, JsonProperty]
        public decimal? BonusL;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;

        //[JsonProperty]
        //public int level = 0;

        //[JsonProperty]
        //public bool isLeaf = false;

        //[JsonProperty]
        //public bool expanded = false;
        public class Cache : WebTools.ListCache<Cache, GameRow>
        {
            [SqlSetting("Cache", "Games"), DefaultValue(30000.0)]
            public override double LifeTime
            {
                get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
                set { }
            }

            public override void Update(SqlCmd sqlcmd, string key, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd ?? args.GetValue<SqlCmd>(0)))
                {
                    List<GameRow> all = new List<GameRow>();
                    Dictionary<GameID, string> allName = new Dictionary<GameID, string>();
                    List<GameRow> rows1 = new List<GameRow>();
                    Dictionary<GameID, string> rows2 = new Dictionary<GameID, string>();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Game nolock"))
                    {
                        GameRow row = r.ToObject<GameRow>();
                        if (row.ID.HasValue)
                        {
                            all.Add(row);
                            allName[row.ID.Value] = row.Name;
                            if (row.Locked.In(BU.GameLocked.Active, BU.GameLocked.Pause))
                            {
                                rows1.Add(row);
                                rows2[row.ID.Value] = row.Name;
                            }
                        }
                    }
                    this.All = all;
                    this.All2 = allName;
                    base.Rows = rows1;
                    this.Rows2 = rows2;
                }
            }

            Dictionary<GameID, string> m_Names_Active = new Dictionary<GameID, string>();
            public Dictionary<GameID, string> Rows2
            {
                get { return Interlocked.CompareExchange(ref m_Names_Active, null, null); }
                set { Interlocked.Exchange(ref m_Names_Active, value); }
            }

            List<GameRow> all = new List<GameRow>();
            public List<GameRow> All
            {
                get { return Interlocked.CompareExchange(ref all, null, null); }
                set { Interlocked.Exchange(ref all, value); }
            }

            Dictionary<GameID, string> m_Names = new Dictionary<GameID, string>();
            public Dictionary<GameID, string> All2
            {
                get { return Interlocked.CompareExchange(ref m_Names, null, null); }
                set { Interlocked.Exchange(ref m_Names, value); }
            }
        }
    }

    public class GameRowCommand
    {
        [JsonProperty]
        public virtual GameID? ID { get; set; }
        //[JsonProperty]
        //public virtual GameID? newID;
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual GameLocked? Locked { get; set; }
        [JsonProperty]
        public virtual decimal? BonusW { get; set; }
        [JsonProperty]
        public virtual decimal? BonusL { get; set; }

        public GameRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                GameRow row = sqlcmd.GetRowEx<GameRow>(RowErrorCode.GameNotFound, "select * from Game nolock where ID={0}", (int?)this.ID);
                sqltool s = new sqltool();
                //s["*", "ID", "    ", row.ID, "    "] = command.newID;
                s["*N", "Name", "       ", row.Name, "       "] = text.ValidAsString * this.Name;
                s["  ", "Locked", "     ", row.Locked, "     "] = (int?)this.Locked;
                s["  ", "BonusW", "     ", row.BonusW, "     "] = this.BonusW;
                s["  ", "BonusL", "     ", row.BonusL, "     "] = this.BonusL;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.BuildEx("update Game set ", sqltool._FieldValue, " where ID={ID} select * from Game nolock where ID={ID}");
                return sqlcmd.ExecuteEx<GameRow>(sqlstr);
            }
        }

        public GameRow auto_create(SqlCmd sqlcmd)
        {
            sqltool s = new sqltool();
            s["* ", "ID", "         "] = this.ID;
            s["*N", "Name", "       "] = (text.ValidAsString * this.Name) ?? this.ID.ToString();
            s["  ", "Locked", "     "] = this.Locked ?? 0;
            s["  ", "BonusW", "     "] = this.BonusW;
            s["  ", "BonusL", "     "] = this.BonusL;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx(@"if not exists(select * from Game nolock where ID={ID})
insert into Game (", sqltool._Fields, @") values (", sqltool._Values, @")
select * from Game nolock where ID={ID}");
            return sqlcmd.ExecuteEx<GameRow>(sqlstr);
        }

        //        public GameRow insert(string json_s, params object[] args)
        //        {
        //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //            {
        //                //if (this.newID.HasValue && (sqlcmd.ExecuteScalar<int>("select count(ID) from Game nolock where ID={0}", this.newID) > 0))
        //                //    return jgrid.RowResponse.AlreadyExist(this.newID);
        //                sqltool s = new sqltool();
        //                s["* ", "ID", "         "] = this.ID;
        //                s["*N", "Name", "       "] = text.ValidAsString * this.Name;
        //                s["  ", "Locked", "     "] = this.Locked ?? 0;
        //                s["  ", "BonusW", "     "] = this.BonusW;
        //                s["  ", "BonusL", "     "] = this.BonusL;
        //                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
        //                s.TestFieldNeeds();
        //                string sqlstr = s.BuildEx(@"insert into Game (", sqltool._Fields, @") values (", sqltool._Values, @")
        //select * from Game nolock where ID={ID}");
        //                return sqlcmd.ExecuteEx<GameRow>(sqlstr);
        //            }
        //        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class GameSelect : jgrid.GridRequest
    {
        [ObjectInvoke, Permissions(Permissions.Code.game_list, Permissions.Flag.Read | Permissions.Flag.Write)]
        static jgrid.GridResponse<GameRow> execute(GameSelect command, string json_s, params object[] args)
        {
            jgrid.GridResponse<GameRow> data = new jgrid.GridResponse<GameRow>();
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Game nolock order by ID asc"))
                    data.rows.Add(r.ToObject<GameRow>());
                return data;
            }
        }

        [ObjectInvoke, Permissions(Permissions.Code.game_list, Permissions.Flag.Write)]
        static object execute(GameUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class GameUpdate : GameRowCommand, IRowCommand { }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class GameInsert : GameRowCommand
    //{
    //    [ObjectInvoke, api.Async, api.Permission(AllowGuest = false)]
    //    static object execute(GameInsert command, string json_s, params object[] args)
    //    {
    //        try { return text.RowUpdate(command.insert(json_s, args)); }
    //        catch (RowException ex) { return ex; }
    //    }
    //}

}

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GameTypeRow
    {
        [DbImport, JsonProperty]
        public GameID? GameID;
        [DbImport, JsonProperty]
        public int? TypeCode;
        [DbImport, JsonProperty]
        public string GameType;
        [DbImport, JsonProperty]
        public string Name;
    }

    public class GameTypeRowCommand
    {
        [JsonProperty]
        public virtual GameID? GameID { get; set; }
        [JsonProperty]
        public virtual int? TypeCode { get; set; }
        [JsonProperty]
        public virtual int? newTypeCode { get; set; }
        [JsonProperty]
        public virtual string GameType { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }

        public GameTypeRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["* ", "GameID", "  "] = this.GameID;
            s["* ", "TypeCode", ""] = this.newTypeCode;
            s["* ", "GameType", ""] = text.ValidAsString * this.GameType;
            s["*N", "Name", "    "] = text.ValidAsString * this.Name;
            string sqlstr = s.BuildEx("insert into GameType (", sqltool._Fields, ") values (", sqltool._Values, ") select * from GameType nolock where GameID={GameID} and TypeCode={TypeCode}");
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<GameTypeRow>(sqlstr);
        }

        public GameTypeRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                GameTypeRow row = sqlcmd.GetRowEx<GameTypeRow>(RowErrorCode.NotFound, "select * from GameType nolock where GameID={0} and TypeCode={1}", (int?)this.GameID, this.TypeCode);
                sqltool s = new sqltool();
                s[" ", "TypeCode", "", row.TypeCode, ""] = this.newTypeCode;
                s[" ", "GameType", "", row.GameType, ""] = this.GameType;
                s["N", "Name", "    ", row.Name, "    "] = this.Name;
                if (s.fields.Count == 0) return row;
                s.Values["GameID"] = this.GameID;
                s.Values["oldTypeCode"] = this.TypeCode;
                s.Values["newTypeCode"] = this.newTypeCode;
                string sqlstr = s.BuildEx("update GameType set ", sqltool._FieldValue, @" where GameID={GameID} and TypeCode={oldTypeCode}
select * from GameType nolock where GameID={GameID} and TypeCode={newTypeCode}");
                return sqlcmd.ExecuteEx<GameTypeRow>(sqlstr);
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class GameTypeSelect : jgrid.GridRequest
    {
        [JsonProperty]
        public int? GameID;

        [ObjectInvoke, Permissions(Permissions.Code.gametype_list, Permissions.Flag.Read | Permissions.Flag.Write)]
        static jgrid.GridResponse<GameTypeRow> execute(GameTypeSelect command, string json_s, params object[] args)
        {
            jgrid.GridResponse<GameTypeRow> data = new jgrid.GridResponse<GameTypeRow>();
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from GameType nolock where GameID={0} order by TypeCode asc", command.GameID ?? 0))
                    data.rows.Add(r.ToObject<GameTypeRow>());
                return data;
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class GameTypeUpdate : GameTypeRowCommand, IRowCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.game_list, Permissions.Flag.Write)]
        static object execute(GameTypeUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class GameTypeInsert : GameTypeRowCommand, IRowCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.game_list, Permissions.Flag.Write)]
        static object execute(GameTypeInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
    }
}