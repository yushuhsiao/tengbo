using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace web
{
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
        public override int UserLevel
        {
            get { return 0; }
            set { }
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

    public class AgentRowCommand : UserRowCommand
    {
        [JsonProperty]
        public int? ParentID { get; set; }
        [JsonProperty]
        public string ParentACNT { get; set; }
        [JsonProperty]
        public virtual CurrencyCode? Currency { get; set; }
        [JsonProperty]
        public virtual string SecPassword { get; set; }

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
                this.apply_GroupID(sqlcmd);
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
                s["*", "GroupID", "   "] = this.GroupID;
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

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentSelect : jgrid.GridRequest<AgentSelect>
    {
        protected override string init_defaultkey() { return "a.CreateTime"; }
        protected override Dictionary<string, string> init_sortkeys()
        {
            return new Dictionary<string, string>()
        {
            {"ParentACNT", "b.ACNT"},
            {"ID", "a.ID"},
            {"CorpID", "a.CorpID"},
            {"ACNT", "a.ACNT"},
            {"GroupID", "a.CorpID {0}, a.GroupID"},
            {"Name", "a.Name"},
            {"Locked", "a.Locked"},
            {"Currency", "a.Currency"},
            {"Balance", "a.Balance"},
            {"ModifyUser", "a.ModifyUser"},
            {"ModifyTime", "a.ModifyTime"},
            {"CreateUser", "a.CreateUser"},
        };
        }

        [JsonProperty]
        int? ParentID;
        [JsonProperty]
        string ParentACNT;
        [JsonProperty]
        string ACNT;
        [JsonProperty]
        string Name;
        [JsonProperty]
        Locked? Locked;

        [ObjectInvoke, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write | Permissions.Flag.Read)]
        jgrid.GridResponse<AgentRow> execute(AgentSelect command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<AgentRow> data = new jgrid.GridResponse<AgentRow>();
                StringBuilder sql = new StringBuilder(@"from [Agent] a with(nolock) left join Agent b on a.ParentID=b.ID");
                int cnt = 0;
                sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", this.CorpID);
                sql_where(sql, ref cnt, "a.ParentID='{0}'", this.ParentID);
                sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (this.ParentACNT * text.ValidAsACNT).Remove("%"));
                sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (this.ACNT * text.ValidAsACNT).Remove("%"));
                sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (this.Name * text.ValidAsName).Remove("%"));
                sql_where(sql, ref cnt, "a.Locked={0}", (byte?)this.Locked);

                data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
                data.page_size = this.page_size;
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, a.*, b.ACNT as ParentACNT
{3}) a where rowid>{0} and rowid<={1} order by rowid", this.rows_start, this.rows_end, this.GetOrderBy(), sql))
                    data.rows.Add(r.ToObject<AgentRow>());
                return data;
            }
        }

        [ObjectInvoke, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write)]
        static object execute(AgentInsert command, string json_s, params object[] args) { return command.insert(null, null, json_s, args); }

        [ObjectInvoke, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write)]
        static object execute(AgentUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentInsert : AgentRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentUpdate : AgentRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AgentBankCardRow
    {
        [DbImport, JsonProperty]
        public int? AgentID { get; set; }
        [DbImport, JsonProperty]
        public int? Index { get; set; }
        [DbImport, JsonProperty]
        public string CardID { get; set; }
        [DbImport, JsonProperty]
        public string BankName { get; set; }
        [DbImport, JsonProperty]
        public string AccountName { get; set; }
        [DbImport, JsonProperty]
        public string Loc1 { get; set; }
        [DbImport, JsonProperty]
        public string Loc2 { get; set; }
        [DbImport, JsonProperty]
        public string Loc3 { get; set; }
    }

    public class AgentBankCardRowCommand : AgentBankCardRow
    {
        [JsonProperty]
        public virtual int? newIndex { get; set; }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class AgentBankCardInsert : AgentBankCardRowCommand, IRowCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class AgentBankCardUpdate : AgentBankCardRowCommand, IRowCommand { }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class AgentBankCardDelete : AgentBankCardRowCommand, IRowCommand { }

        [ObjectInvoke, Permissions("agent3", Permissions.Flag.Write)]
        static AgentBankCardRow insert(AgentBankCardInsert command, string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["* ", "AgentID", "    "] = command.AgentID;
            s["* ", "Index", "      "] = command.newIndex;
            s["* ", "CardID", "     "] = text.ValidAsString * command.CardID;
            s["*N", "BankName", "   "] = text.ValidAsString * command.BankName;
            s["*N", "AccountName", ""] = text.ValidAsString * command.AccountName;
            s[" N", "Loc1", "       "] = text.ValidAsString * command.Loc1;
            s[" N", "Loc2", "       "] = text.ValidAsString * command.Loc2;
            s[" N", "Loc3", "       "] = text.ValidAsString * command.Loc3;
            s.TestFieldNeeds();
            s.Values["AgentID_"] = s.Values["AgentID"];
            s.Values["AgentID"] = (StringEx.sql_str)"ID";
            string sqlstr = s.BuildEx("insert into AgentBank (", sqltool._Fields, ") select ", sqltool._Values, " from Agent nolock where ID={AgentID_} select * from AgentBank nolock where AgentID={AgentID_} and [Index]={Index}");
            SqlCmd sqlcmd;
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
                return sqlcmd.ExecuteEx<AgentBankCardRow>(sqlstr);
        }

        [ObjectInvoke, Permissions("agent3", Permissions.Flag.Write)]
        static AgentBankCardRow update(AgentBankCardUpdate command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                AgentBankCardRow row = sqlcmd.GetRowEx<AgentBankCardRow>(RowErrorCode.NotFound, "select * from AgentBank nolock where AgentID={0} and [Index]={1}", command.AgentID, command.Index);
                sqltool s = new sqltool();
                s["* ", "Index", "      ", row.Index, "      "] = command.newIndex;
                s["* ", "CardID", "     ", row.CardID, "     "] = text.ValidAsString * command.CardID;
                s["*N", "BankName", "   ", row.BankName, "   "] = text.ValidAsString * command.BankName;
                s["*N", "AccountName", "", row.AccountName, ""] = text.ValidAsString * command.AccountName;
                s[" N", "Loc1", "       ", row.Loc1, "       "] = text.ValidAsString * command.Loc1;
                s[" N", "Loc2", "       ", row.Loc2, "       "] = text.ValidAsString * command.Loc2;
                s[" N", "Loc3", "       ", row.Loc3, "       "] = text.ValidAsString * command.Loc3;
                if (s.fields.Count == 0) return row;
                //s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["AgentID"] = row.AgentID;
                s.Values["Index"] = command.newIndex;
                s.Values["oldIndex"] = row.Index;
                s.TestFieldNeeds();
                string sqlstr = s.BuildEx("update AgentBank set ", sqltool._FieldValue, " where AgentID={AgentID} and [Index]={oldIndex} select * from AgentBank nolock where AgentID={AgentID} and [Index]={Index}");
                return sqlcmd.ExecuteEx<AgentBankCardRow>(sqlstr);
            }
        }

        [ObjectInvoke, Permissions("agent3", Permissions.Flag.Write)]
        static AgentBankCardRow delete(AgentBankCardDelete command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.GetRowEx<AgentBankCardRow>(RowErrorCode.NotFound, "select * from AgentBank nolock where AgentID={0} and [Index]={1} delete AgentBank where AgentID={0} and [Index]={1}", command.AgentID, command.Index);
        }
    }
}