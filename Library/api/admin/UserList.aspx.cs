using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Web;
using web;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
using Tools;

namespace web
{
    using Tools;
    public abstract class UserRow
    {
        public abstract UserType UserType { get; }
        public abstract string TableName { get; }
        public abstract StringEx.sql_str TableName2 { get; }
        public abstract StringEx.sql_str GroupTableName { get; }
        public abstract int? ParentID { get; set; }
        public abstract string ParentACNT { get; set; }

        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public string ACNT;
        [DbImport]
        public byte? GroupID;
        [JsonProperty("GroupID")]
        long? _out_GroupID
        {
            get { return text.GroupRowID(this.CorpID, this.GroupID); }
        }
        [DbImport, JsonProperty]
        public string Name;
        [DbImport("pwd")]
        public string Password;
        [DbImport, JsonProperty]
        public Locked? Locked;
        [DbImport]
        public byte? TranFlag;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;

        [DbImport, JsonProperty]
        public CurrencyCode? Currency;
        [DbImport, JsonProperty]
        public decimal? Balance;

        protected static T GetUser<T>(SqlCmd sqlcmd, string tableName, int? id, int? corpID, string acnt, params string[] fields) where T : UserRow, new()
        {
            if ((id.HasValue) || (corpID.HasValue && (string.IsNullOrEmpty(acnt) == false)))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select ");
                if (fields.GetValue<string>(0) == "*")
                    sql.Append("*");
                else
                {
                    sql.Append("ID, CorpID, ACNT");
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (fields[i] == "ID") continue;
                        if (fields[i] == "CorpID") continue;
                        if (fields[i] == "ACNT") continue;
                        sql.Append(",");
                        sql.Append(fields[i]);
                    }
                }
                sql.Append(" from ");
                sql.Append(tableName);
                sql.Append(" nolock where");
                if (id.HasValue)
                    sql.AppendFormat(" ID={0}", id.Value);
                else
                    sql.AppendFormat(" CorpID={0} and ACNT='{1}'", corpID.Value, acnt * text.ValidAsACNT);
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                    return sqlcmd.ToObject<T>(sql.ToString());
            }
            return null;
        }
    }

    #region 管理帳戶

    [DebuggerStepThrough]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdminRow : UserRow
    {
        public override UserType UserType { get { return BU.UserType.Admin; } }
        static StringEx.sql_str s_TableName = "Admin";
        public override string TableName { get { return s_TableName.value; } }
        public override StringEx.sql_str TableName2 { get { return s_TableName; } }
        static StringEx.sql_str s_GroupTableName = (StringEx.sql_str)"AdminGroup";
        public override StringEx.sql_str GroupTableName { get { return s_GroupTableName; } }
        public override int? ParentID
        {
            get { return null; }
            set { }
        }
        public override string ParentACNT
        {
            get { return null; }
            set { }
        }
        //[DbImport, JsonProperty]
        //public int? ParentID;
        //[DbImport, JsonProperty]
        //public string ParentACNT;
        public static AdminRow GetAdmin(SqlCmd sqlcmd, int? adminID, int? corpID, string acnt, params string[] fields)
        {
            return GetUser<AdminRow>(sqlcmd, "Admin", adminID, corpID, acnt, fields);
        }
        public static AdminRow GetAdminEx(SqlCmd sqlcmd, int? adminID, int? corpID, string acnt, params string[] fields)
        {
            AdminRow row = GetAdmin(sqlcmd, adminID, corpID, acnt, fields);
            if (row == null) throw new RowException(RowErrorCode.AdminNotFound);
            return row;
        }
        //public static AdminRow GetAdmin(SqlCmd sqlcmd, int? adminID, params string[] fields) /*              */ { return GetAdmin(sqlcmd, adminID, null, null, fields); }
        //public static AdminRow GetAdminEx(SqlCmd sqlcmd, int? adminID, params string[] fields) /*            */ { return GetAdminEx(sqlcmd, adminID, null, null, fields); }
        //public static AdminRow GetAdmin(SqlCmd sqlcmd, int? corpID, string acnt, params string[] fields) /*  */ { return GetAdmin(sqlcmd, null, corpID, acnt, fields); }
        //public static AdminRow GetAdminEx(SqlCmd sqlcmd, int? corpID, string acnt, params string[] fields) /**/ { return GetAdminEx(sqlcmd, null, corpID, acnt, fields); }
    }

    public class AdminRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual string ACNT { get; set; }
        [JsonProperty("GroupID")]
        public virtual long? _in_GroupID { get; set; }
        public byte? GroupID
        {
            get { return text.GroupRowID_GroupID(this._in_GroupID); }
        }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual string Password { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        //[JsonProperty]
        //public virtual int? ParentID { get; set; }
        //[JsonProperty]
        //public virtual string ParentACNT { get; set; }

        public bool Password_verify;
        public string Password_old;

        public AdminRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                AdminRow row = sqlcmd.GetRowEx<AdminRow>(RowErrorCode.AdminNotFound, "select * from [Admin] nolock where ID={0}", this.ID);
                if (this.Password_verify)
                {
                    string p1 = text.EncodePassword(row.ACNT, this.Password_old);
                    if (p1 != row.Password)
                        throw new RowException(RowErrorCode.PasswordError);
                }
                sqltool s = new sqltool();
                s[" ", "GroupID", "   ", row.GroupID, " "] = this.GroupID;
                s["N", "Name", "      ", row.Name, "    "] = text.ValidAsName * this.Name;
                s[" ", "GroupID", "   ", row.GroupID, " "] = this.GroupID;
                s[" ", "pwd", "       ", row.Password, ""] = text.EncodePassword(row.ACNT, this.Password);
                s[" ", "Locked", "    ", row.Locked, "  "] = this.Locked;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.BuildEx("update Admin set ", sqltool._FieldValue, " where ID={ID} select * from [Admin] nolock where ID={ID}");
                return sqlcmd.ExecuteEx<AdminRow>(sqlstr);
            }
        }

        public AdminRow insert(SqlCmd sqlcmd, CorpRow corp_row, string json_s, params object[] args)
        {
            string acnt = text.ValidAsACNT * this.ACNT;
            sqltool s = new sqltool();
            s["*", "CorpID", "    "] = this.CorpID;
            s["*", "ACNT", "      "] = acnt;
            s["*", "GroupID", "   "] = 1;
            s["N", "Name", "      "] = (text.ValidAsName * this.Name) ?? acnt;
            s[" ", "pwd", "       "] = text.EncodePassword(acnt, this.Password ?? _Global.DefaultPassword);
            s[" ", "Locked", "    "] = this.Locked ?? BU.Locked.Active;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            s.Values["CorpID"] = (StringEx.sql_str)"ID";
            s.Values["CorpID_"] = this.CorpID;
            if (corp_row != null)
            {
                s["", "pwd", ""] = "Disabled";
                if (corp_row.ID == 0)
                {
                    s.Values["ID"] = _Global.RootAdminID;
                    s["", "pwd", ""] = text.EncodePassword(acnt, acnt);
                }
            }
            //            string sqlstr = s.BuildEx("declare @ADMINID int ", s.Values.ContainsKey("ID") ? "set @ADMINID={ID}" : "exec alloc_UserID @ADMINID output, @type='Admin',@corpid={CorpID_},@acnt={ACNT}", @"
            //insert into [Admin] (ID, GroupID,", sqltool._Fields, @")
            //select @ADMINID, b.ID,", sqltool._Values, @"
            //from Corp a with(nolock)
            //left join AdminGroup b with(nolock) on a.ID=b.CorpID
            //where b.Class=isnull({GroupID},1) and a.ID={CorpID_}
            //", AdminRowCommand.select_from_admin, " where a.ID=@ADMINID");
            string sqlstr = s.BuildEx("declare @ADMINID int ", s.Values.ContainsKey("ID") ? "set @ADMINID={ID}" : "exec alloc_UserID @ADMINID output, @type='Admin',@corpid={CorpID_},@acnt={ACNT}", @"
insert into [Admin] ([ID],", sqltool._Fields, @")
select @ADMINID,", sqltool._Values, @"
from Corp nolock where ID={CorpID_}
select * from [Admin] nolock where ID=@ADMINID");
            //string sqlstr = s.BuildEx("exec admin_insert ", sqltool._AtFieldValue);
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                if (corp_row != null)
                    return sqlcmd.ToObject<AdminRow>(sqlstr);
                else
                    return sqlcmd.ExecuteEx<AdminRow>(sqlstr);
            }
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            //    return sqlcmd.ExecuteEx<AdminRow>(sqlstr);
        }
    }

    #endregion

    #region 代理帳戶

    [_DebuggerStepThrough, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [DebuggerDisplay("ID:{ID}, Parent:{ParentID}")]
    public class AgentRow : UserRow
    {
        public override UserType UserType { get { return BU.UserType.Agent; } }
        static StringEx.sql_str s_TableName = "Agent";
        public override string TableName { get { return s_TableName.value; } }
        public override StringEx.sql_str TableName2 { get { return s_TableName; } }
        static StringEx.sql_str s_GroupTableName = (StringEx.sql_str)"AgentGroup";
        public override StringEx.sql_str GroupTableName { get { return s_GroupTableName; } }
        [DbImport("sec_pwd")]
        public string SecurityPassword;
        [DbImport, JsonProperty]
        public override int? ParentID
        {
            get;
            set;
        }
        [DbImport, JsonProperty]
        public override string ParentACNT
        {
            get;
            set;
        }

        [DbImport, JsonProperty]
        public decimal? A_BonusW;
        [DbImport, JsonProperty]
        public decimal? A_BonusL;
        [DbImport, JsonProperty]
        public decimal? A_ShareW;
        [DbImport, JsonProperty]
        public decimal? A_ShareL;


        [DbImport, JsonProperty]
        public decimal? M_BonusW;
        [DbImport, JsonProperty]
        public decimal? M_BonusL;
        [DbImport, JsonProperty]
        public decimal? M_ShareW;
        [DbImport, JsonProperty]
        public decimal? M_ShareL;

        [DbImport("PCT")]
        decimal? m_PCT;
        [JsonProperty]
        public decimal? PCT { get { return this.ParentID == 0 ? 1 : this.m_PCT; } }
        [DbImport, JsonProperty]
        public int? MaxMember;
        [DbImport, JsonProperty]
        public int? MaxAgent;
        [DbImport, JsonProperty]
        public int? MaxDepth;
        [DbImport]
        public TranFlag? TranFlag;
        [JsonProperty]
        public bool? A_TranFlag1 { get { return getFlag(BU.TranFlag.Agent1); } }
        [JsonProperty]
        public bool? A_TranFlag2 { get { return getFlag(BU.TranFlag.Agent2); } }
        [JsonProperty]
        public bool? M_TranFlag1 { get { return getFlag(BU.TranFlag.Member1); } }
        [JsonProperty]
        public bool? M_TranFlag2 { get { return getFlag(BU.TranFlag.Member2); } }

        bool? getFlag(BU.TranFlag n1)
        {
            if (this.TranFlag.HasValue)
                return (this.TranFlag.Value & n1) == n1;
            return null;
        }

        //internal static AgentRow GetRow(SqlCmd sqlcmd, int? agentID, int? corpID, string agentACNT)
        //{
        //    using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
        //    {
        //        User user = HttpContext.Current.User as User;
        //        if (agentID.HasValue)
        //        {
        //            return sqlcmd.ToObject<AgentRow>("select * from Agent nolock where ID={0} {2}and CorpID={1}", agentID, user.CorpID, user.CorpID == 0 ? "--" : "");
        //        }
        //        else
        //        {
        //            int? _corpID = (user.CorpID == 0) ? corpID : user.CorpID;
        //            if (!_corpID.HasValue)
        //                throw new RowException(RowErrorCode.InvalidCorpID);
        //            if (string.IsNullOrEmpty(agentACNT))
        //                return sqlcmd.ToObject<AgentRow>("select b.* from Corp a with(nolock) left join Agent b with(nolock) on a.AgentACNT=b.ACNT where a.ID={0}", _corpID);
        //            else
        //                return sqlcmd.ToObject<AgentRow>("select * from Agent nolock where CorpID={0} and ACNT='{1}'", _corpID, agentACNT);
        //        }
        //    }
        //}



        public static AgentRow GetAgent(SqlCmd sqlcmd, int? agentID, int? corpID, string acnt, params string[] fields)
        {
            if ((agentID.HasValue) || (corpID.HasValue && (string.IsNullOrEmpty(acnt) == false)))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select ");
                bool b = false;
                if (fields.GetValue<string>(0) == "*")
                {
                    sql.Append("a.*, b.ACNT as ParentACNT");
                    b = true;
                }
                else
                {
                    sql.Append("a.ID, a.CorpID, a.ACNT");
                    for (int i = 0; i < fields.Length; i++)
                    {
                        switch (fields[i])
                        {
                            case "ID":
                            case "CorpID":
                            case "ACNT": continue;
                            case "ParentACNT": fields[i] = "b.ACNT as ParentACNT"; break;
                        }
                        sql.Append(" ,");
                        if (fields[i].StartsWith("b."))
                        {
                            b = true;
                            sql.Append(fields[i]);
                        }
                        else
                        {
                            sql.Append("a.");
                            sql.Append(fields[i]);
                        }
                    }
                }
                sql.Append(" from Agent a with(nolock)");
                if (b) sql.Append(" left join Agent b with(nolock) on a.ParentID=b.ID");
                sql.Append(" where");
                if (agentID.HasValue)
                    sql.AppendFormat(" a.ID={0}", agentID.Value);
                else
                    sql.AppendFormat(" a.CorpID={0} and a.ACNT='{1}'", corpID.Value, acnt * text.ValidAsACNT);
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                    return sqlcmd.ToObject<AgentRow>(sql.ToString());
            }
            return null;
        }
        public static AgentRow GetAgentEx(SqlCmd sqlcmd, int? agentID, int? corpID, string acnt, params string[] fields)
        {
            return GetAgentEx(sqlcmd, RowErrorCode.AgentNotFound, agentID, corpID, acnt, fields);
        }
        public static AgentRow GetAgentEx(SqlCmd sqlcmd, RowErrorCode err, int? agentID, int? corpID, string acnt, params string[] fields)
        {
            AgentRow row = GetAgent(sqlcmd, agentID, corpID, acnt, fields);
            if (row == null) throw new RowException(err);
            return row;
        }
    }

    public class AgentRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual string ACNT { get; set; }
        [JsonProperty]
        public virtual int? ParentID { get; set; }
        [JsonProperty]
        public virtual string ParentACNT { get; set; }
        [JsonProperty("GroupID")]
        public virtual long? _in_GroupID { get; set; }
        public byte? GroupID
        {
            get { return text.GroupRowID_GroupID(this._in_GroupID); }
        }
        [JsonProperty]
        public virtual CurrencyCode? Currency { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual string Password { get; set; }
        [JsonProperty]
        public virtual string SecPassword { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }

        [JsonProperty]
        public virtual decimal? PCT { get; set; }

        [JsonProperty]
        public virtual decimal? A_BonusW { get; set; }
        [JsonProperty]
        public virtual decimal? A_BonusL { get; set; }
        [JsonProperty]
        public virtual decimal? A_ShareW { get; set; }
        [JsonProperty]
        public virtual decimal? A_ShareL { get; set; }

        [JsonProperty]
        public virtual decimal? M_ShareW { get; set; }
        [JsonProperty]
        public virtual decimal? M_ShareL { get; set; }
        [JsonProperty]
        public virtual decimal? M_BonusW { get; set; }
        [JsonProperty]
        public virtual decimal? M_BonusL { get; set; }

        [JsonProperty]
        public virtual int? MaxMember { get; set; }
        [JsonProperty]
        public virtual int? MaxAgent { get; set; }
        [JsonProperty]
        public virtual int? MaxDepth { get; set; }

        [JsonProperty]
        public bool? A_TranFlag1 { get; set; }
        [JsonProperty]
        public bool? A_TranFlag2 { get; set; }
        [JsonProperty]
        public bool? M_TranFlag1 { get; set; }
        [JsonProperty]
        public bool? M_TranFlag2 { get; set; }
        //public TranFlag? TranFlag
        //{
        //    get
        //    {
        //        TranFlag tranFlag = 0;
        //        if (this.A_TranFlag1.HasValue && this.A_TranFlag2.HasValue && this.M_TranFlag1.HasValue && this.M_TranFlag2.HasValue)
        //        {
        //            if (this.A_TranFlag1 == true) tranFlag |= BU.TranFlag.Agent1;
        //            if (this.A_TranFlag2 == true) tranFlag |= BU.TranFlag.Agent2;
        //            if (this.M_TranFlag1 == true) tranFlag |= BU.TranFlag.Member1;
        //            if (this.M_TranFlag2 == true) tranFlag |= BU.TranFlag.Member2;
        //            return tranFlag;
        //        }
        //        return null;
        //    }
        //}

        public const string select_from_agent = "select a.*, c.ACNT as ParentACNT from [Agent] a with(nolock) left join Agent c on a.ParentID=c.ID";


        static TranFlag setFlag(TranFlag prev, TranFlag n1, bool? n2)
        {
            if (n2.HasValue)
            {
                if (n2.Value)
                    return prev | n1;
                else
                    return prev & ~n1;
            }
            return prev;
        }

        TranFlag GetTranFlag(TranFlag? init)
        {
            TranFlag tranFlag = init ?? 0;
            tranFlag = setFlag(tranFlag, BU.TranFlag.Agent1, this.A_TranFlag1);
            tranFlag = setFlag(tranFlag, BU.TranFlag.Agent2, this.A_TranFlag2);
            tranFlag = setFlag(tranFlag, BU.TranFlag.Member1, this.M_TranFlag1);
            tranFlag = setFlag(tranFlag, BU.TranFlag.Member2, this.M_TranFlag2);
            return tranFlag;
        }

        static object _limit(int? n)
        {
            if (n.HasValue)
            {
                if (n.Value >= 0)
                    return n.Value;
                else
                    return StringEx.sql_str.Null;
            }
            return null;
        }

        public AgentRow insert(SqlCmd sqlcmd, CorpRow corp_row, string json_s, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                this.ParentACNT *= text.ValidAsACNT;
                this.ACNT *= text.ValidAsACNT;
                if (string.IsNullOrEmpty(this.ACNT))
                    throw new RowException(RowErrorCode.FieldNeeds, "", "ACNT");
                sqltool s = new sqltool();
                if (corp_row == null)
                {
                    AgentRow parent = AgentRow.GetAgentEx(sqlcmd, RowErrorCode.ParentAgentNotFound, this.ParentID, this.CorpID, this.ParentACNT, "ParentID", "MaxAgent", "MaxDepth", "Currency");
                    int d = 0;
                    for (AgentRow agent = parent; agent != null; agent = AgentRow.GetAgent(sqlcmd, agent.ParentID, null, null, "ParentID", "MaxDepth"), d++)
                        if (d >= (agent.MaxDepth ?? int.MaxValue))
                            throw new RowException(RowErrorCode.MaxDepth);
                    if (parent.MaxAgent.HasValue)
                        if (sqlcmd.ExecuteScalar<int>("select count(*) from Agent nolock where ParentID={0}", parent.ID) >= parent.MaxAgent.Value)
                            throw new RowException(RowErrorCode.MaxAgent);
                    s["*", "CorpID", "    "] = parent.CorpID;
                    s["*", "ParentID", "  "] = parent.ID;
                    s["*", "Currency", "  "] = this.Currency ?? parent.Currency;
                }
                else
                {
                    s["*", "CorpID", "    "] = corp_row.ID;
                    s["*", "ParentID", "  "] = 0;
                    s["*", "Currency", "  "] = this.Currency ?? corp_row.Currency;
                }
                s["*", "ACNT", "      "] = this.ACNT;
                s["*", "GroupID", "   "] = 1;
                s["N", "Name", "      "] = (text.ValidAsName * this.Name) ?? this.ACNT;
                s[" ", "pwd", "       "] = text.EncodePassword(this.ACNT, this.Password ?? _Global.DefaultPassword);
                s[" ", "sec_pwd", "   "] = text.EncodePassword(this.ACNT, this.SecPassword);
                s[" ", "Locked", "    "] = this.Locked ?? BU.Locked.Active;
                s[" ", "TranFlag", "  "] = (int)GetTranFlag(null);
                s[" ", "PCT", "       "] = this.PCT;
                s[" ", "A_BonusW", "  "] = this.A_BonusW;
                s[" ", "A_BonusL", "  "] = this.A_BonusL;
                s[" ", "A_ShareW", "  "] = this.A_ShareW;
                s[" ", "A_ShareL", "  "] = this.A_ShareL;
                s[" ", "M_BonusW", "  "] = this.M_BonusW;
                s[" ", "M_BonusL", "  "] = this.M_BonusL;
                s[" ", "M_ShareW", "  "] = this.M_ShareW;
                s[" ", "M_ShareL", "  "] = this.M_ShareL;
                s[" ", "MaxMember", " "] = _limit(this.MaxMember);
                s[" ", "MaxAgent", "  "] = _limit(this.MaxAgent);
                s[" ", "MaxDepth", "  "] = _limit(this.MaxDepth);
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                string sqlstr = s.BuildEx("declare @AGENTID int exec alloc_UserID @AGENTID output, @type='Agent',@corpid={CorpID},@acnt={ACNT}", @"
insert into [Agent] (ID,", sqltool._Fields, @") values (@AGENTID,", sqltool._Values, ")", @"
select * from Agent nolock where ID=@AGENTID");
                if (corp_row == null)
                    return sqlcmd.ExecuteEx<AgentRow>(sqlstr);
                else
                    return sqlcmd.ToObject<AgentRow>(sqlstr);
            }
        }

        public AgentRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                AgentRow row = sqlcmd.GetRowEx<AgentRow>(RowErrorCode.AgentNotFound, "{0} where a.ID={1}", AgentRowCommand.select_from_agent, this.ID);
                sqltool s = new sqltool();
                s[" ", "GroupID", "     ", row.GroupID, "         "] = this.GroupID;
                s["N", "Name", "        ", row.Name, "            "] = text.ValidAsName * this.Name;
                s[" ", "pwd", "         ", row.Password, "        "] = text.EncodePassword(row.ACNT, this.Password);
                s[" ", "sec_pwd", "     ", row.SecurityPassword, ""] = text.EncodePassword(row.ACNT, this.SecPassword);
                s[" ", "Locked", "      ", row.Locked, "          "] = this.Locked;
                s[" ", "TranFlag", "    ", row.TranFlag, "        "] = (int)GetTranFlag(row.TranFlag);
                s[" ", "PCT", "         ", row.PCT, "             "] = this.PCT;
                s[" ", "A_BonusW", "    ", row.A_BonusW, "        "] = this.A_BonusW;
                s[" ", "A_BonusL", "    ", row.A_BonusL, "        "] = this.A_BonusL;
                s[" ", "A_ShareW", "    ", row.A_ShareW, "        "] = this.A_ShareW;
                s[" ", "A_ShareL", "    ", row.A_ShareL, "        "] = this.A_ShareL;
                s[" ", "M_BonusW", "    ", row.M_BonusW, "        "] = this.M_BonusW;
                s[" ", "M_BonusL", "    ", row.M_BonusL, "        "] = this.M_BonusL;
                s[" ", "M_ShareW", "    ", row.M_ShareW, "        "] = this.M_ShareW;
                s[" ", "M_ShareL", "    ", row.M_ShareL, "        "] = this.M_ShareL;
                s[" ", "MaxMember", "   ", row.MaxMember, "       "] = _limit(this.MaxMember);
                s[" ", "MaxAgent", "    ", row.MaxAgent, "        "] = _limit(this.MaxAgent);
                s[" ", "MaxDepth", "    ", row.MaxDepth, "        "] = _limit(this.MaxDepth);
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                //string sqlstr = s.BuildEx("update Agent set ", sqltool._FieldValue, " where ID={ID} exec agent_select_single @ID={ID}");
                string sqlstr = s.BuildEx("update Agent set ", sqltool._FieldValue, " where ID={ID} ", AgentRowCommand.select_from_agent, " where a.ID={ID}");
                return sqlcmd.ExecuteEx<AgentRow>(sqlstr);
            }
        }
    }

    #endregion

    #region 會員帳戶

    [_DebuggerStepThrough]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [DebuggerDisplay("ID:{ID}, Parent:{ParentID}")]
    public class MemberRow : UserRow
    {
        public override UserType UserType { get { return BU.UserType.Member; } }
        static StringEx.sql_str s_TableName = "Member";
        public override string TableName { get { return s_TableName.value; } }
        public override StringEx.sql_str TableName2 { get { return s_TableName; } }
        static StringEx.sql_str s_GroupTableName = (StringEx.sql_str)"MemberGroup";
        public override StringEx.sql_str GroupTableName { get { return s_GroupTableName; } }
        [DbImport, JsonProperty]
        public int? AgentID;
        public override int? ParentID
        {
            get { return this.AgentID; }
            set { this.AgentID = value; }
        }
        [DbImport, JsonProperty]
        public string AgentACNT;
        public override string ParentACNT
        {
            get { return this.AgentACNT; }
            set { this.AgentACNT = value; }
        }
        [DbImport, JsonProperty]
        public DateTime? Birthday;
        [DbImport, JsonProperty]
        public string Tel;
        [DbImport, JsonProperty]
        public string Mail;
        [DbImport, JsonProperty]
        public string QQ;
        [DbImport, JsonProperty]
        public string RegisterIP;
        [DbImport("sec_pwd")]
        public string SecurityPassword;

        [DbImport, JsonProperty]
        public string Introducer;
        [DbImport, JsonProperty]
        public UserSex? Sex;
        [DbImport, JsonProperty]
        public string Addr;
        [DbImport, JsonProperty]
        public string UserMemo;

        [DbImport, JsonProperty]
        public DateTime? LoginTime;
        [DbImport, JsonProperty]
        public string LoginIP;
        [DbImport, JsonProperty]
        public int? LoginCount;

        [JsonProperty]
        public int? DepositCount;
        [JsonProperty]
        public string Bank;
        [DbImport, JsonProperty]
        public string Memo;

        //[DbImport]
        //public RowErrorCode? err;
        //[DbImport]
        //public string msg;

        //public MemberSubAccRow2 GetSubAcc(SqlCmd sqlcmd, GameID gameID)
        //{
        //    switch (gameID)
        //    {
        //        case GameID.HG: return new MemberGameRow_HG2();
        //        case GameID.EA: return new MemberGameRow_EA2();
        //        case GameID.WFT: return new MemberGameRow_WFT2();
        //        case GameID.KG: return new MemberGameRow_KENO2();
        //        case GameID.SUNBET: return new MemberGameRow_SUNBET2();
        //        default: throw new RowException(RowErrorCode.GameNotFound);
        //    }
        //}
        //public void AddDeposit() { }
        //public void AddWithDrawal() { }
        //public void AddPromp() { }
        //public void AddBankCard() { }

        //public static MemberRow GetRow(SqlCmd sqlcmd, int? memberID)
        //{
        //    return sqlcmd.GetRow<MemberRow>("select * from Member nolock where ID={0}", memberID);
        //}
        //public static MemberRow GetRowEx(SqlCmd sqlcmd, int? memberID)
        //{
        //    return sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", memberID);
        //}

        public static MemberRow GetMember(SqlCmd sqlcmd, int? memberID, int? corpID, string acnt, params string[] fields)
        {
            if ((memberID.HasValue) || (corpID.HasValue && (string.IsNullOrEmpty(acnt) == false)))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select ");
                bool b = false;
                if (fields.GetValue<string>(0) == "*")
                {
                    sql.Append("a.*, b.ACNT as AgentACNT");
                    b = true;
                }
                else
                {
                    sql.Append("a.ID, a.CorpID, a.ACNT");
                    for (int i = 0; i < fields.Length; i++)
                    {
                        switch (fields[i])
                        {
                            case "ID":
                            case "CorpID":
                            case "ACNT": continue;
                            case "ParentID": fields[i] = "AgentID"; break;
                            case "ParentACNT": fields[i] = "b.ACNT as AgentACNT"; break;
                        }
                        sql.Append(" ,");
                        if (fields[i].StartsWith("b."))
                        {
                            b = true;
                            sql.Append(fields[i]);
                        }
                        else
                        {
                            sql.Append("a.");
                            sql.Append(fields[i]);
                        }
                    }
                }
                sql.Append(" from Member a with(nolock)");
                if (b) sql.Append(" left join Agent b with(nolock) on a.AgentID=b.ID");
                sql.Append(" where");
                if (memberID.HasValue)
                    sql.AppendFormat(" a.ID={0}", memberID.Value);
                else
                    sql.AppendFormat(" a.CorpID={0} and a.ACNT='{1}'", corpID.Value, acnt * text.ValidAsACNT);
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                    return sqlcmd.ToObject<MemberRow>(sql.ToString());
            }
            return null;
            return GetUser<MemberRow>(sqlcmd, "Member", memberID, corpID, acnt, fields);
        }
        public static MemberRow GetMemberEx(SqlCmd sqlcmd, int? memberID, int? corpID, string acnt, params string[] fields)
        {
            MemberRow row = GetMember(sqlcmd, memberID, corpID, acnt, fields);
            if (row == null) throw new RowException(RowErrorCode.MemberNotFound);
            return row;
        }
        //public static MemberRow GetMember(SqlCmd sqlcmd, int? memberID, params string[] fields) /*              */ { return GetMember(sqlcmd, memberID, null, null, fields); }
        //public static MemberRow GetMemberEx(SqlCmd sqlcmd, int? memberID, params string[] fields) /*            */ { return GetMemberEx(sqlcmd, memberID, null, null, fields); }
        //public static MemberRow GetMember(SqlCmd sqlcmd, int? corpID, string acnt, params string[] fields) /*   */ { return GetMember(sqlcmd, null, corpID, acnt, fields); }
        //public static MemberRow GetMemberEx(SqlCmd sqlcmd, int? corpID, string acnt, params string[] fields) /* */ { return GetMemberEx(sqlcmd, null, corpID, acnt, fields); }
    }

    public class MemberRowCommand
    {
        [JsonProperty]
        public virtual string ACNT { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual int? AgentID { get; set; }
        [JsonProperty]
        public virtual string AgentACNT { get; set; }
        [JsonProperty("GroupID")]
        public virtual long? _in_GroupID { get; set; }
        public byte? GroupID
        {
            get { return text.GroupRowID_GroupID(this._in_GroupID); }
        }
        [JsonProperty]
        public virtual CurrencyCode? Currency { get; set; }
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual string Password { get; set; }
        [JsonProperty]
        public virtual string SecPassword { get; set; }
        [JsonProperty]
        public virtual DateTime? Birthday { get; set; }
        [JsonProperty]
        public virtual string Tel { get; set; }
        [JsonProperty]
        public virtual string Mail { get; set; }
        [JsonProperty]
        public virtual string QQ { get; set; }
        [JsonProperty]
        public virtual string Introducer { get; set; }
        [JsonProperty]
        public virtual string Memo { get; set; }
        [JsonProperty]
        public virtual string Sex { get; set; }
        [JsonProperty]
        public virtual string Addr { get; set; }
        [JsonProperty]
        public virtual string UserMemo { get; set; }

        public const string select_from_member = "select a.*, c.ACNT as AgentACNT from Member a with(nolock) left join Agent c with(nolock) on a.AgentID=c.ID";

        protected virtual void OnGetRow(SqlCmd sqlcmd, MemberRow row) { }

        public MemberRow insert(SqlCmd sqlcmd, CorpRow corp_row, string json_s, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                bool create_corp = corp_row != null;
                this.AgentACNT *= text.ValidAsACNT;
                this.ACNT *= text.ValidAsACNT;
                if (string.IsNullOrEmpty(this.ACNT))
                    throw new RowException(RowErrorCode.FieldNeeds, "", "ACNT");
                if (corp_row != null)
                {
                    this.CorpID = corp_row.ID;
                    this.AgentACNT = corp_row.AgentACNT;
                    this.AgentID = null;
                }
                AgentRow parent = AgentRow.GetAgentEx(sqlcmd, RowErrorCode.ParentAgentNotFound, this.AgentID, this.CorpID, this.AgentACNT, "MaxMember", "Currency");
                if (parent.MaxMember.HasValue)
                    if (sqlcmd.ExecuteScalar<int>("select count(*) from Member nolock where AgentID={0}", parent.ID) >= parent.MaxMember.Value)
                        throw new RowException(RowErrorCode.MaxMember);
                sqltool s = new sqltool();
                s["*", "CorpID", "    "] = parent.CorpID;
                s["*", "AgentID", "   "] = parent.ID;
                s["*", "ACNT", "      "] = this.ACNT;
                s["*", "GroupID", "   "] = 1;
                s["N", "Name", "      "] = (text.ValidAsName * this.Name) ?? this.ACNT;
                s[" ", "pwd", "       "] = text.EncodePassword(this.ACNT, this.Password ?? _Global.DefaultPassword);
                s[" ", "sec_pwd", "   "] = text.EncodePassword(this.ACNT, this.SecPassword);
                s[" ", "Locked", "    "] = this.Locked ?? BU.Locked.Active;
                s[" ", "Currency", "  "] = this.Currency ?? parent.Currency;
                s["D", "Birthday", "  "] = this.Birthday;
                s[" ", "Tel", "       "] = text.ValidAsString * this.Tel;
                s[" ", "Mail", "      "] = text.ValidAsString * this.Mail;
                s[" ", "QQ", "        "] = text.ValidAsString * this.QQ;
                s[" ", "Introducer", ""] = text.ValidAsString * this.Introducer;
                s["N", "Memo", "      "] = text.ValidAsString * this.Memo;
                s[" ", "Sex", "       "] = this.Sex.ToByte();
                s["N", "Addr", "      "] = text.ValidAsString * this.Addr;
                s["N", "UserMemo", "  "] = text.ValidAsString * this.UserMemo;
                s[" ", "RegisterIP", ""] = HttpContext.Current.RequestIP();
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                string sqlstr = s.BuildEx("declare @MEMBERID int exec alloc_UserID @MEMBERID output, @type='Member',@corpid={CorpID},@acnt={ACNT}", @"
insert into [Member] (ID,", sqltool._Fields, ") values (@MEMBERID,", sqltool._Values, ")", @"
", select_from_member, " where a.ID=@MEMBERID");
                if (create_corp)
                    return sqlcmd.ToObject<MemberRow>(sqlstr);
                else
                    return sqlcmd.ExecuteEx<MemberRow>(sqlstr);
            }
        }

        public MemberRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                MemberRow row = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "{0} where a.ID={1}", MemberRowCommand.select_from_member, this.ID);
                this.OnGetRow(sqlcmd, row);
                //MemberRow row = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", this.ID);
                sqltool s = new sqltool();
                s[" ", "GroupID", "   ", row.GroupID, "         "] = this.GroupID;
                s["N", "Name", "      ", row.Name, "            "] = text.ValidAsName * this.Name;
                s[" ", "pwd", "       ", row.Password, "        "] = text.EncodePassword(row.ACNT, this.Password);
                s[" ", "sec_pwd", "   ", row.SecurityPassword, ""] = text.EncodePassword(row.ACNT, this.SecPassword);
                s[" ", "Locked", "    ", row.Locked, "          "] = this.Locked;
                s["D", "Birthday", "  ", row.Birthday, "        "] = this.Birthday;
                s[" ", "Tel", "       ", row.Tel, "             "] = text.ValidAsString * this.Tel;
                s[" ", "Mail", "      ", row.Mail, "            "] = text.ValidAsString * this.Mail;
                s[" ", "QQ", "        ", row.QQ, "              "] = text.ValidAsString * this.QQ;
                s["N", "Memo", "      ", row.Memo, "            "] = text.ValidAsString * this.Memo;
                s[" ", "Introducer", "", row.Introducer, "      "] = text.ValidAsString * this.Introducer;
                s[" ", "Sex", "       ", row.Sex, "             "] = this.Sex.ToByte();
                s["N", "Addr", "      ", row.Addr, "            "] = text.ValidAsString * this.Addr;
                s["N", "UserMemo", "  ", row.UserMemo, "        "] = text.ValidAsString * this.UserMemo;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                //string sqlstr = s.BuildEx("update Member set ", sqltool._FieldValue, " where ID={ID} exec member_select_single @ID={ID}");
                string sqlstr = s.BuildEx("update Member set ", sqltool._FieldValue, " where ID={ID} ", MemberRowCommand.select_from_member, " where a.ID={ID}");
                return sqlcmd.ExecuteEx<MemberRow>(sqlstr);
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberBankCardRow
    {
        [DbImport, JsonProperty]
        public int? MemberID;
        [DbImport, JsonProperty]
        public int? Index;
        [DbImport, JsonProperty]
        public string CardID;
        [DbImport, JsonProperty]
        public string BankName;
        [DbImport, JsonProperty]
        public string AccountName;
        [DbImport, JsonProperty]
        public string Loc1;
        [DbImport, JsonProperty]
        public string Loc2;
        [DbImport, JsonProperty]
        public string Loc3;
    }

    public class MemberBankCardRowCommand
    {
        [JsonProperty]
        public virtual int? MemberID { get; set; }
        [JsonProperty]
        public virtual int? Index { get; set; }
        [JsonProperty]
        public virtual int? newIndex { get; set; }
        [JsonProperty]
        public virtual string CardID { get; set; }
        [JsonProperty]
        public virtual string BankName { get; set; }
        [JsonProperty]
        public virtual string AccountName { get; set; }
        [JsonProperty]
        public virtual string Loc1 { get; set; }
        [JsonProperty]
        public virtual string Loc2 { get; set; }
        [JsonProperty]
        public virtual string Loc3 { get; set; }

        public MemberBankCardRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["* ", "MemberID", "   "] = this.MemberID;
            s["* ", "Index", "      "] = this.newIndex;
            s["* ", "CardID", "     "] = text.ValidAsString * this.CardID;
            s["*N", "BankName", "   "] = text.ValidAsString * this.BankName;
            s["*N", "AccountName", ""] = text.ValidAsString * this.AccountName;
            s[" N", "Loc1", "       "] = text.ValidAsString * this.Loc1;
            s[" N", "Loc2", "       "] = text.ValidAsString * this.Loc2;
            s[" N", "Loc3", "       "] = text.ValidAsString * this.Loc3;
            s.TestFieldNeeds();
            s.Values["MemberID_"] = s.Values["MemberID"];
            s.Values["MemberID"] = (StringEx.sql_str)"ID";
            string sqlstr = s.BuildEx("insert into MemberBank (", sqltool._Fields, ") select ", sqltool._Values, " from Member nolock where ID={MemberID_} select * from MemberBank nolock where MemberID={MemberID_} and [Index]={Index}");
            SqlCmd sqlcmd;
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
                return sqlcmd.ExecuteEx<MemberBankCardRow>(sqlstr);
        }

        public bool getRowOnly = false;
        public bool throwNotFound = true;

        public MemberBankCardRow update(string json_s, params object[] args)
        {
            SqlCmd sqlcmd;
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
            {
                MemberBankCardRow row = sqlcmd.GetRow<MemberBankCardRow>("select * from MemberBank nolock where MemberID={0} and [Index]={1}", this.MemberID, this.Index);
                if (getRowOnly)
                    return row;
                if (row == null)
                {
                    if (throwNotFound) throw new RowException(RowErrorCode.NotFound);
                    return row;
                }
                sqltool s = new sqltool();
                s["* ", "Index", "      ", row.Index, "      "] = this.newIndex;
                s["* ", "CardID", "     ", row.CardID, "     "] = text.ValidAsString * this.CardID;
                s["*N", "BankName", "   ", row.BankName, "   "] = text.ValidAsString * this.BankName;
                s["*N", "AccountName", "", row.AccountName, ""] = text.ValidAsString * this.AccountName;
                s[" N", "Loc1", "       ", row.Loc1, "       "] = text.ValidAsString * this.Loc1;
                s[" N", "Loc2", "       ", row.Loc2, "       "] = text.ValidAsString * this.Loc2;
                s[" N", "Loc3", "       ", row.Loc3, "       "] = text.ValidAsString * this.Loc3;
                if (s.fields.Count == 0) return row;
                //s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["MemberID"] = row.MemberID;
                s.Values["Index"] = this.newIndex;
                s.Values["oldIndex"] = row.Index;
                s.TestFieldNeeds();
                string sqlstr = s.BuildEx("update MemberBank set ", sqltool._FieldValue, " where MemberID={MemberID} and [Index]={oldIndex} select * from MemberBank nolock where MemberID={MemberID} and [Index]={Index}");
                return sqlcmd.ExecuteEx<MemberBankCardRow>(sqlstr);
            }
        }

        //public static MemberBankCardRow GetRow(SqlCmd sqlcmd, int memberID, int? index)
        //{
        //    using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd))
        //        return sqlcmd.ToObject<MemberBankCardRow>("select * from MemberBank nolock where MemberID={0} and [Index]={1}", memberID, index ?? 1);
        //}
    }

    #endregion
}

//#region 會員子帳戶

//namespace web
//{
//    //public abstract class MemberGameRowCommand
//    //{
//    //    public MemberGameRow Update(string json_s, params object[] args)
//    //    {
//    //    }


//    //    public readonly string TableName;
//    //    public MemberGameRowCommand(GameID gameID, string tableName)
//    //    {
//    //        this.GameID = gameID;
//    //        this.TableName = tableName;
//    //    }
//    //}

//    //public abstract class MemberGameRowCommand<TRow, TRowCommand> : MemberGameRowCommand
//    //    where TRow : MemberGameRow, new()
//    //    where TRowCommand : MemberGameRowCommand<TRow, TRowCommand>, new()
//    //{
//    //    public MemberGameRowCommand(GameID gameID, string tableName) : base(gameID, tableName) { }
//    //}

//    //public abstract partial class MemberGame
//    //{
//    //    public readonly GameID GameID;
//    //    public readonly string TableName;
//    //    internal MemberGame(GameID gameID, string tableName)
//    //    {
//    //        this.GameID = gameID;
//    //        this.TableName = tableName;
//    //    }

//    //    public static MemberGame GetInstance(GameID? gameID)
//    //    {
//    //        switch (gameID ?? 0)
//    //        {
//    //            case GameID.HG: return MemberGame_HG.Instance;
//    //            case GameID.EA: return MemberGame_EA.Instance;
//    //            case GameID.WFT: return MemberGame_WFT.Instance;
//    //            case GameID.WFT_SPORTS: return MemberGame_WFT_SPORTS.Instance;
//    //            case GameID.KENO: return MemberGame_KENO.Instance;
//    //            case GameID.KENO_SSC: return MemberGame_KENO_SSC.Instance;
//    //            case GameID.SUNBET: return MemberGame_SUNBET.Instance;
//    //            case GameID.AG: return MemberGame_AG.Instance;
//    //            case GameID.BBIN: return MemberGame_BBIN.Instance;
//    //        }
//    //        return null;
//    //    }

//    //    public abstract MemberGameRow Select(SqlCmd sqlcmd, int memberID);
//    //    public abstract MemberGameRow InsertRow(SqlCmd sqlcmd, int memberID);
//    //    public abstract MemberGameRow UpdateRow(string json_s, params object[] args);
//    //}

//    //public abstract partial class MemberGame<T, TRow, TRowCommand> : MemberGame
//    //    where T : MemberGame<T, TRow, TRowCommand>, new()
//    //    where TRow : MemberGameRow, new()
//    //    where TRowCommand : MemberGameRowCommand, new()
//    //{
//    //    public static readonly T Instance = new T();
//    //    internal MemberGame(GameID gameID, string tableName) : base(gameID, tableName) { }

//    //    public override MemberGameRow Select(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return this.Select2(sqlcmd, memberID);
//    //    }
//    //    public TRow Select2(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return sqlcmd.ToObject<TRow>("select * from {0} nolock where GameID={1} and MemberID={2}", this.TableName, (int?)this.GameID, memberID);
//    //    }

//    //    public override MemberGameRow InsertRow(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return this.InsertRow(sqlcmd, memberID);
//    //    }
//    //    public TRow InsertRow2(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return null;
//    //    }

//    //    public override MemberGameRow UpdateRow(string json_s, params object[] args)
//    //    {
//    //        return this.UpdateRow2(json_s, args);
//    //    }
//    //    public TRow UpdateRow2(string json_s, params object[] args)
//    //    {
//    //        return null;
//    //    }
//    //}

//    //public abstract partial class MemberGameRowCommand
//    //{
//    //    [JsonProperty]
//    //    internal readonly GameID GameID;
//    //    [JsonProperty]
//    //    public virtual int? MemberID { get; set; }
//    //    [JsonProperty]
//    //    public virtual string Locked { get; set; }
//    //    [JsonProperty]
//    //    public virtual string Balance { get; set; }
//    //    [JsonProperty]
//    //    public virtual string Password { get; set; }
//    //    [JsonProperty]
//    //    public virtual string ACNT { get; set; }
//    //    [JsonProperty]
//    //    public virtual CurrencyCode? Currency { get; set; }
//    //    [JsonProperty]
//    //    public virtual string DepositAmount { get; set; }
//    //    public GameTranRow DepositResult;
//    //    [JsonProperty]
//    //    public virtual string WithdrawalAmount { get; set; }
//    //    public GameTranRow WithdrawalResult;

//    //public static MemberGameRowCommand GetInstance(GameID gameID)
//    //{
//    //    switch (gameID)
//    //    {
//    //        case GameID.HG: return MemberGameRowCommand_HG.Instance;
//    //        case GameID.EA: return MemberGameRowCommand_EA.Instance;
//    //        case GameID.WFT: return MemberGameRowCommand_WFT.Instance;
//    //        case GameID.WFT_SPORTS: return MemberGameRowCommand_WFT_SPORTS.Instance;
//    //        case GameID.KENO: return MemberGameRowCommand_KENO.Instance;
//    //        case GameID.KENO_SSC: return MemberGameRowCommand_KENO_SSC.Instance;
//    //        case GameID.SUNBET: return MemberGameRowCommand_SUNBET.Instance;
//    //        case GameID.AG: return MemberGameRowCommand_AG.Instance;
//    //        case GameID.BBIN: return MemberGameRowCommand_BBIN.Instance;
//    //    }
//    //    return null;
//    //}
        
//    //    public readonly string TableName;
//    //    internal MemberGameRowCommand(GameID gameID, string tableName) { this.GameID = gameID; this.TableName = tableName; }
//    //    public abstract MemberGameRow GetRow(SqlCmd sqlcmd, int memberID);
//    //    public abstract MemberGameRow Update(string json_s, params object[] args);
//    //    internal abstract MemberGameRow OnUpdate(SqlCmd _sqlcmd, string json_s, params object[] args);
//    //    internal virtual bool OnGameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran) { return false; }
//    //    internal virtual bool OnGameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran) { return false; }
//    //}

//    //public abstract partial class MemberGameRowCommand<TRowCommand, TRow> : MemberGameRowCommand
//    //    where TRowCommand : MemberGameRowCommand<TRowCommand, TRow>, new()
//    //    where TRow : MemberGameRow, new()
//    //{
//    //    internal MemberGameRowCommand(GameID gameID, string tableName) : base(gameID, tableName) { }
//    //    public static readonly TRowCommand Instance = new TRowCommand();

//    //    [DebuggerStepThrough]
//    //    public override MemberGameRow GetRow(SqlCmd sqlcmd, int memberID) { return GetRow2(sqlcmd, memberID); }
//    //    public TRow GetRow2(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return sqlcmd.ToObject<TRow>("select * from {0} nolock where GameID={1} and MemberID={2}", this.TableName, (int?)this.GameID, memberID);
//    //    }

//    //    [DebuggerStepThrough]
//    //    public override MemberGameRow Update(string json_s, params object[] args) { return this.Update2(json_s, args); }
//    //    public TRow Update2(string json_s, params object[] args)
//    //    {
//    //        SqlCmd sqlcmd = null;
//    //        decimal deposit = this.DepositAmount.ToDecimal() ?? 0;
//    //        if (deposit > 0)
//    //        {
//    //            DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd);
//    //            try
//    //            {
//    //                this.DepositResult = new GameTranRowCommand()
//    //                {
//    //                    GameID = this.GameID,
//    //                    MemberID = this.MemberID,
//    //                    LogType = LogType.GameDeposit,
//    //                    Amount1 = deposit
//    //                }.Insert(null, sqlcmd);
//    //                this.Balance = "*";
//    //            }
//    //            catch { }
//    //        }
//    //        decimal withdrawal = this.WithdrawalAmount.ToDecimal() ?? 0;
//    //        if (withdrawal > 0)
//    //        {
//    //            DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd);
//    //            try
//    //            {
//    //                this.WithdrawalResult = new GameTranRowCommand()
//    //                {
//    //                    GameID = this.GameID,
//    //                    MemberID = this.MemberID,
//    //                    LogType = LogType.GameWithdrawal,
//    //                    Amount1 = withdrawal
//    //                }.Insert(null, sqlcmd);
//    //                this.Balance = "*";
//    //            }
//    //            catch { }
//    //        }
//    //        return this.OnUpdate2(sqlcmd, json_s, args);
//    //    }

//    //    [DebuggerStepThrough]
//    //    internal override MemberGameRow OnUpdate(SqlCmd sqlcmd, string json_s, params object[] args) { return this.OnUpdate2(sqlcmd, json_s, args); }
//    //    internal abstract TRow OnUpdate2(SqlCmd sqlcmd, string json_s, params object[] args);
//    //}
//}

//#endregion

//#region 會員子帳戶 v2
//namespace web
//{
//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public abstract class MemberGameRow
//    {
//        [DbImport, JsonProperty]
//        public GameID? GameID;
//        [DbImport, JsonProperty]
//        public int? MemberID;

//        [DbImport, JsonProperty]
//        public virtual Locked? Locked { get; set; }              // dbo.Member_???.Locked
//        [DbImport, JsonProperty]
//        public virtual decimal? Balance { get; set; }
//        [DbImport, JsonProperty]
//        public virtual string ACNT { get; set; }
//        [DbImport("pwd"), JsonProperty]
//        public virtual string Password { get; set; }
//        [DbImport, JsonProperty]
//        public virtual CurrencyCode? Currency { get; set; }
//        [DbImport, JsonProperty]
//        public virtual DateTime? CreateTime { get; set; }
//        [DbImport, JsonProperty]
//        public virtual int? CreateUser { get; set; }
//        [DbImport, JsonProperty]
//        public virtual DateTime? ModifyTime { get; set; }
//        [DbImport, JsonProperty]
//        public virtual int? ModifyUser { get; set; }
//    }

//    public abstract class MemberGameRowCommand
//    {
//        public static MemberGameRowCommand GetInstance(GameID gameID)
//        {
//            switch (gameID)
//            {
//                case GameID.HG: return MemberGameRowCommand_HG.Instance;
//                case GameID.EA: return MemberGameRowCommand_EA.Instance;
//                case GameID.WFT: return MemberGameRowCommand_WFT.Instance;
//                case GameID.WFT_SPORTS: return MemberGameRowCommand_WFT_SPORTS.Instance;
//                case GameID.KENO: return MemberGameRowCommand_KENO.Instance;
//                case GameID.KENO_SSC: return MemberGameRowCommand_KENO_SSC.Instance;
//                case GameID.SUNBET: return MemberGameRowCommand_SUNBET.Instance;
//                case GameID.AG: return MemberGameRowCommand_AG.Instance;
//                case GameID.BBIN: return MemberGameRowCommand_BBIN.Instance;
//            }
//            return null;
//        }

//        [JsonProperty]
//        internal readonly GameID GameID;
//        [JsonProperty]
//        public virtual int? MemberID { get; set; }
//        [JsonProperty]
//        public virtual string Locked { get; set; }
//        [JsonProperty]
//        public virtual string Balance { get; set; }
//        [JsonProperty]
//        public virtual string Password { get; set; }
//        [JsonProperty]
//        public virtual string ACNT { get; set; }
//        [JsonProperty]
//        public virtual CurrencyCode? Currency { get; set; }
//        [JsonProperty]
//        public virtual string DepositAmount { get; set; }
//        public GameTranRow DepositResult;
//        [JsonProperty]
//        public virtual string WithdrawalAmount { get; set; }
//        public GameTranRow WithdrawalResult;

//        public readonly string TableName;
//        public abstract Type RowType { get; }
//        internal MemberGameRowCommand(GameID gameid, string tableName) { this.GameID = gameid; this.TableName = tableName; }

//        public MemberGameRow Update(string json_s, params object[] args)
//        {
//            SqlCmd sqlcmd = null;
//            decimal deposit = this.DepositAmount.ToDecimal() ?? 0;
//            if (deposit > 0)
//            {
//                DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd);
//                try
//                {
//                    this.DepositResult = new GameTranRowCommand()
//                    {
//                        GameID = this.GameID,
//                        MemberID = this.MemberID,
//                        LogType = LogType.GameDeposit,
//                        Amount1 = deposit
//                    }.Insert(null, sqlcmd);
//                    this.Balance = "*";
//                }
//                catch { }
//            }
//            decimal withdrawal = this.WithdrawalAmount.ToDecimal() ?? 0;
//            if (withdrawal > 0)
//            {
//                DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd);
//                try
//                {
//                    this.WithdrawalResult = new GameTranRowCommand()
//                    {
//                        GameID = this.GameID,
//                        MemberID = this.MemberID,
//                        LogType = LogType.GameWithdrawal,
//                        Amount1 = withdrawal
//                    }.Insert(null, sqlcmd);
//                    this.Balance = "*";
//                }
//                catch { }
//            }
//            return this.OnUpdate(sqlcmd, json_s, args);
//        }
//        internal abstract MemberGameRow OnUpdate(SqlCmd _sqlcmd, string json_s, params object[] args);

//        internal abstract bool OnGameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran);

//        internal abstract bool OnGameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran);

//    }
//    public abstract class MemberGameRowCommand<TRow, TRowCommand> : MemberGameRowCommand
//        where TRow : MemberGameRow
//        where TRowCommand : MemberGameRowCommand<TRow, TRowCommand>
//    {
//        public static TRowCommand Instance = Activator.CreateInstance<TRowCommand>();
//        public override Type RowType { [DebuggerStepThrough] get { return typeof(TRow); } }
//        internal MemberGameRowCommand(GameID gameid, string tableName) : base(gameid, tableName) { }
//    }

//    #region HG

//    public class MemberGameRow_HG : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_HG : MemberGameRowCommand<MemberGameRow_HG, MemberGameRowCommand_HG>, IGameAPI
//    {
//        public MemberGameRowCommand_HG() : base(GameID.HG, "Member_001") { }
//    }

//    #endregion

//    #region EA

//    public class MemberGameRow_EA : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_EA : MemberGameRowCommand<MemberGameRow_EA, MemberGameRowCommand_EA>, IGameAPI
//    {
//        public MemberGameRowCommand_EA() : base(GameID.EA, "Member_002") { }
//    }

//    #endregion

//    #region WFT

//    public class MemberGameRow_WFT : MemberGameRow
//    {
//    }
//    public abstract class _MemberGameRowCommand_WFT<T> : MemberGameRowCommand<MemberGameRow_WFT, T>, IGameAPI where T : _MemberGameRowCommand_WFT<T>
//    {
//        internal _MemberGameRowCommand_WFT(GameID gameid, string tableName) : base(gameid, tableName) { }
//    }

//    public class MemberGameRowCommand_WFT : _MemberGameRowCommand_WFT<MemberGameRowCommand_WFT>
//    {
//        public MemberGameRowCommand_WFT() : base(GameID.WFT, "Member_003") { }
//    }
//    public class MemberGameRowCommand_WFT_SPORTS : _MemberGameRowCommand_WFT<MemberGameRowCommand_WFT_SPORTS>
//    {
//        public MemberGameRowCommand_WFT_SPORTS() : base(GameID.WFT_SPORTS, "Member_008") { }
//    }

//    #endregion

//    #region KENO

//    public class MemberGameRow_KENO : MemberGameRow
//    {
//    }
//    public abstract class _MemberGameRowCommand_KENO<T> : MemberGameRowCommand<MemberGameRow_KENO, T>, IGameAPI where T : _MemberGameRowCommand_KENO<T>
//    {
//        internal _MemberGameRowCommand_KENO(GameID gameid, string tableName) : base(gameid, tableName) { }
//    }

//    public class MemberGameRowCommand_KENO : _MemberGameRowCommand_KENO<MemberGameRowCommand_KENO>
//    {
//        public MemberGameRowCommand_KENO() : base(GameID.KENO, "Member_004") { }
//    }
//    public class MemberGameRowCommand_KENO_SSC : _MemberGameRowCommand_KENO<MemberGameRowCommand_KENO_SSC>
//    {
//        public MemberGameRowCommand_KENO_SSC() : base(GameID.KENO_SSC, "Member_007") { }
//    }

//    #endregion

//    #region SUBNET

//    public class MemberGameRow_SUNBET : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_SUNBET : MemberGameRowCommand<MemberGameRow_SUNBET, MemberGameRowCommand_SUNBET>
//    {
//        public MemberGameRowCommand_SUNBET() : base(GameID.SUNBET, "Member_005") { }
//    }

//    #endregion

//    #region AG

//    public class MemberGameRow_AG : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_AG : MemberGameRowCommand<MemberGameRow_AG, MemberGameRowCommand_AG>
//    {
//        public MemberGameRowCommand_AG() : base(GameID.AG, "Member_006") { }
//    }

//    #endregion

//    #region BBIN

//    public class MemberGameRow_BBIN : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_BBIN : MemberGameRowCommand<MemberGameRow_BBIN, MemberGameRowCommand_BBIN>, IGameAPI
//    {
//        public MemberGameRowCommand_BBIN() : base(GameID.BBIN, "Member_009") { }
//    }

//    #endregion

//    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    //public abstract class MemberRow2_XXX
//    //{
//    //    public MemberRow2_XXX(GameID gameID) { this.GameID = gameID; }

//    //    [DbImport, JsonProperty]
//    //    public GameID GameID;
//    //}
//    //public class MemberRow2_HG : MemberRow2_XXX
//    //{
//    //    public MemberRow2_HG() : base(GameID.HG) { }
//    //}
//    //public class MemberRow2_EA : MemberRow2_XXX
//    //{
//    //    public MemberRow2_EA() : base(GameID.EA) { }
//    //}

//    //public abstract class MemberRowCommand2_XXX
//    //{
//    //}
//}
//namespace web
//{
//}
//#endregion