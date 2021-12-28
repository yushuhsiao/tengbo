using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Tools;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CorpRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public string ACNT;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public Locked? Locked;
        [DbImport, JsonProperty]
        public CurrencyCode? Currency;
        [DbImport, JsonProperty]
        public string AdminACNT;
        [DbImport, JsonProperty]
        public string AgentACNT;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;

        [SqlSetting]
        public string prefix
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.ID.Value); }
        }

        public Dictionary<Guid, MemberGroupRow> MemberGroups
        {
            get
            {
                Dictionary<Guid, MemberGroupRow> list = null;
                if (this.ID.HasValue)
                    MemberGroupRow.Cache.Instance.Value0.TryGetValue(this.ID.Value, out list);
                return list;
            }
        }

        public class Cache : WebTools.ListCache<Cache, CorpRow>
        {
            //Dictionary<int, string> _names;
            //public Dictionary<int, string> names
            //{
            //    get { return Interlocked.CompareExchange(ref this._names, null, null); }
            //    set { Interlocked.Exchange(ref this._names, value); }
            //}

            [SqlSetting("Cache", "Corps"), DefaultValue(30000.0)]
            public override double LifeTime
            {
                get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
                set { }
            }

            //protected override bool CheckUpdate(string key, params object[] args)
            //{
            //    return this.names == null;
            //}

            public override void Update(SqlCmd sqlcmd, string key, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd ?? args.GetValue<SqlCmd>(0)))
                {
                    //Dictionary<int, string> names = new Dictionary<int, string>();
                    List<CorpRow> rows = new List<CorpRow>();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Corp nolock"))
                    {
                        CorpRow row = r.ToObject<CorpRow>();
                        //names[row.ID.Value] = row.ACNT;
                        rows.Add(row);
                    }
                    base.Rows = rows;
                    //this.names = names;
                }
            }

            public CorpRow GetCorp(int? corpID)
            {
                foreach (CorpRow row in this.Rows)
                    if (row.ID == corpID)
                        return row;
                return _null<CorpRow>.value;
            }

            public CorpRow GetCorp(string acnt)
            {
                foreach (CorpRow row in this.Rows)
                    if (string.Compare(row.ACNT, acnt, true) == 0)
                        return row;
                return _null<CorpRow>.value;
            }
        }
    }

    public class CorpRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        //[JsonProperty]
        //public int? newID;
        [JsonProperty]
        public virtual string ACNT { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual CurrencyCode? Currency { get; set; }
        //[JsonProperty]
        //public virtual decimal? BonusW { get; set; }
        //[JsonProperty]
        //public virtual decimal? BonusL { get; set; }
        [JsonProperty]
        public virtual string AdminACNT { get; set; }
        [JsonProperty]
        public virtual string AgentACNT { get; set; }

        public CorpRow update(string json_s, params object[] args)
        {
            SqlCmd sqlcmd;
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
            {
                CorpRow row = sqlcmd.GetRowEx<CorpRow>(RowErrorCode.CorpNotFound, "select * from Corp nolock where ID={0}", this.ID);
                sqltool s = new sqltool();
                s["N", "Name", "  ", row.Name, "  "] = text.ValidAsString * this.Name;
                s[" ", "Locked", "", row.Locked, ""] = this.Locked;
                //s[" ", "BonusW", "", row.BonusW, ""] = this.BonusW;
                //s[" ", "BonusL", "", row.BonusL, ""] = this.BonusL;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.BuildEx("update Corp set ", sqltool._FieldValue, " where ID={ID} select * from Corp nolock where ID={ID}");
                return sqlcmd.ExecuteEx<CorpRow>(sqlstr);
            }
        }

        public CorpRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            if (this.ID == 0)
            {
                this.ACNT = "root";
                this.Name = null;
                this.Locked = BU.Locked.Active;
                this.Currency = null;
                this.AdminACNT = this.AgentACNT = null;
            }
            string acnt = text.ValidAsACNT * this.ACNT;
            s["*", "ID", "             "] = this.ID;
            s["*", "ACNT", "           "] = acnt;
            s["N", "Name", "           "] = (text.ValidAsString * this.Name) ?? acnt;
            s[" ", "Locked", "         "] = this.Locked;
            s[" ", "Currency", "       "] = (this.Currency ?? CurrencyCode.USD).ToString();
            s["*", "AdminACNT", "      "] = (text.ValidAsACNT * this.AdminACNT) ?? acnt;
            s["*", "AgentACNT", "      "] = (text.ValidAsACNT * this.AgentACNT) ?? acnt;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx("insert into Corp (", sqltool._Fields, ") values (", sqltool._Values, ") select * from Corp nolock where ID={ID}");
            s.Values["RootAdminID"] = _Global.RootAdminID;
            s.Values["RootAgentID"] = _Global.RootAgentID;
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                try
                {
                    // system init
                    CorpRow corp;
                    if (this.ID == 0)
                    {
                        foreach (GameID gameid in System.Enum.GetValues(typeof(GameID)))
                            new GameRowCommand() { ID = gameid }.auto_create(sqlcmd);
                        corp = sqlcmd.ToObject<CorpRow>("select * from Corp nolock where ID={0}", this.ID);
                        if (corp != null) return corp;
                    }
                    sqlcmd.BeginTransaction();
                    corp = sqlcmd.ToObject<CorpRow>(sqlstr);
                    if (corp == null) throw new RowException(RowErrorCode.NoResult);

                    AdminGroupRow admin_grp =
                        sqlcmd.ToObject<AdminGroupRow>("select top(1) * from [AdminGroup] nolock where CorpID={0}", corp.ID) ??
                        new AdminGroupRowCommand() { op_Insert = true, CorpID = corp.ID, Sort = 1, Name = "default" }.insert(sqlcmd, corp, null);
                    if (admin_grp == null) throw new RowException(RowErrorCode.NoResult);

                    AgentGroupRow agent_grp =
                        sqlcmd.ToObject<AgentGroupRow>("select top(1) * from [AgentGroup] nolock where CorpID={0}", corp.ID) ??
                         new AgentGroupRowCommand() { op_Insert = true, CorpID = corp.ID, Sort = 1, Name = "default" }.insert(sqlcmd, corp, null);
                    if (agent_grp == null) throw new RowException(RowErrorCode.NoResult);

                    MemberGroupRow member_grp =
                        sqlcmd.ToObject<MemberGroupRow>("select top(1) * from [MemberGroup] nolock where CorpID={0}", corp.ID) ??
                        new MemberGroupRowCommand() { op_Insert = true, CorpID = corp.ID, Sort = 1, Name = "default" }.insert(sqlcmd, corp, null);
                    if (member_grp == null) throw new RowException(RowErrorCode.NoResult);

                    AdminRow admin = sqlcmd.ToObject<AdminRow>(s.BuildEx("select * from [Admin] nolock where ", corp.ID == 0 ? "ID={RootAdminID}" : "CorpID={ID} and ACNT={AdminACNT}")) ?? new AdminRowCommand() { CorpID = corp.ID, ACNT = corp.AdminACNT }.insert(sqlcmd, corp, json_s, args);
                    if (admin == null) throw new RowException(RowErrorCode.NoResult);
                    AgentRow agent = sqlcmd.ToObject<AgentRow>(s.BuildEx("select * from [Agent] nolock where ", corp.ID == 0 ? "ID={RootAgentID}" : "CorpID={ID} and ACNT={AgentACNT}")) ?? new AgentRowCommand() { CorpID = corp.ID, ACNT = corp.AgentACNT }.insert(sqlcmd, corp, json_s, args);
                    if (agent == null) throw new RowException(RowErrorCode.NoResult);
                    //MemberRow member = sqlcmd.ToObject<MemberRow>(s.BuildEx("select * from [Member] nolock where ", corp.ID == 0 ? "ID={RootMemberID}" : "CorpID={ID} and ACNT={AgentACNT}")) ?? new AgentRowCommand() { CorpID = corp.ID, ACNT = corp.AgentACNT }.insert(sqlcmd, corp, json_s, args);
                    //if (member == null) throw new RowException(RowErrorCode.NoResult);
                    sqlcmd.Commit();
                    return corp;
                }
                catch
                {
                    sqlcmd.Rollback();
                    throw;
                }
            }
        }
    }
}


[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class CorpSelect : jgrid.GridRequest
{
    [ObjectInvoke, Permissions(Permissions.Code.corp_list, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<CorpRow> execute(CorpSelect command, string json_s, params object[] args)
    {
        jgrid.GridResponse<CorpRow> data = new jgrid.GridResponse<CorpRow>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Corp nolock"))
                data.rows.Add(r.ToObject<CorpRow>());
            return data;
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.corp_list, Permissions.Flag.Write)]
    static object execute(CorpUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.corp_list, Permissions.Flag.Write)]
    static object execute(CorpInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class CorpUpdate : CorpRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class CorpInsert : CorpRowCommand, IRowCommand { }