using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;
using Tools;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace web
{
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
        public override int? ParentID
        {
            get;
            set;
        }
        //public int? ParentID;
        //public override int? ParentID
        //{
        //    get { return this.ParentID; }
        //    set { this.ParentID = value; }
        //}
        [DbImport, JsonProperty]
        public override string ParentACNT
        {
            get;
            set;
        }
        //public string AgentACNT;
        //public override string ParentACNT
        //{
        //    get { return this.AgentACNT; }
        //    set { this.AgentACNT = value; }
        //}
        public override int UserLevel
        {
            get { return 0; }
            set { }
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
                            //case "ParentID": fields[i] = "ParentID"; break;
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
                sql.Append(" from Member a with(nolock)");
                if (b) sql.Append(" left join Agent b with(nolock) on a.ParentID=b.ID");
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

    public class MemberRowCommand : UserRowCommand
    {
        [JsonProperty]
        public virtual int? ParentID { get; set; }
        [JsonProperty]
        public virtual string ParentACNT { get; set; }
        [JsonProperty]
        public virtual CurrencyCode? Currency { get; set; }
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

        public const string select_from_member = "select a.*, c.ACNT as ParentACNT from Member a with(nolock) left join Agent c with(nolock) on a.ParentID=c.ID";

        protected virtual void OnGetRow(SqlCmd sqlcmd, MemberRow row) { }

        public MemberRow insert(SqlCmd sqlcmd, CorpRow corp_row, string json_s, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                this.apply_GroupID(sqlcmd);
                bool create_corp = corp_row != null;
                this.ParentACNT *= text.ValidAsACNT;
                this.ACNT *= text.ValidAsACNT;
                if (string.IsNullOrEmpty(this.ACNT))
                    throw new RowException(RowErrorCode.FieldNeeds, "", "ACNT");
                if (corp_row != null)
                {
                    this.CorpID = corp_row.ID;
                    this.ParentACNT = corp_row.AgentACNT;
                    this.ParentID = null;
                }
                AgentRow parent = AgentRow.GetAgentEx(sqlcmd, RowErrorCode.ParentAgentNotFound, this.ParentID, this.CorpID, this.ParentACNT, "MaxMember", "Currency");
                if (parent.MaxMember.HasValue)
                    if (sqlcmd.ExecuteScalar<int>("select count(*) from Member nolock where ParentID={0}", parent.ID) >= parent.MaxMember.Value)
                        throw new RowException(RowErrorCode.MaxMember);
                sqltool s = new sqltool();
                s["*", "CorpID", "    "] = parent.CorpID;
                s["*", "ParentID", "  "] = parent.ID;
                s["*", "ACNT", "      "] = this.ACNT;
                s["*", "GroupID", "   "] = this.GroupID;
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

    //[jqx._sort(sortcolumn = "ParentACNT", sqlsortcolumn = "b.ACNT")]
    [jqx.Column(field = "ParentACNT", sort_sql = "b.ACNT {0}             ")]
    [jqx.Column(field = "LoginTime ", sort_sql = "c.LoginTime {0}        ")]
    [jqx.Column(field = "LoginIP   ", sort_sql = "c.IP {0}               ")]
    [jqx.Column(field = "ID        ", sort_sql = "a.ID {0}               ")]
    [jqx.Column(field = "CorpID    ", sort_sql = "a.CorpID {0}           ")]
    [jqx.Column(field = "ACNT      ", sort_sql = "a.ACNT {0}             ")]
    [jqx.Column(field = "Name      ", sort_sql = "a.Name {0}             ")]
    [jqx.Column(field = "GroupID   ", sort_sql = "a.CorpID {0}, a.GroupID")]
    [jqx.Column(field = "Locked    ", sort_sql = "a.Locked {0}           ")]
    [jqx.Column(field = "Balance   ", sort_sql = "a.Balance {0}          ")]
    [jqx.Column(field = "Currency  ", sort_sql = "a.Currency {0}         ")]
    [jqx.Column(field = "Memo      ", sort_sql = "a.Memo {0}             ")]
    [jqx.Column(field = "RegisterIP", sort_sql = "a.RegisterIP {0}       ")]
    [jqx.Column(field = "CreateTime", sort_sql = "a.CreateTime {0}       ", sortdefault = true, sortorder = SortOrder.Descending)]
    [jqx.Column(field = "CreateUser", sort_sql = "a.CreateUser {0}       ")]
    [jqx.Column(field = "ModifyTime", sort_sql = "a.ModifyTime {0}       ")]
    [jqx.Column(field = "ModifyUser", sort_sql = "a.ModifyUser {0}       ")]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberSelect2 : jqx.GridRequest<MemberSelect2, MemberRow>
    {
        [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Read)]
        protected override jqx.GridResponse<MemberRow> execute(MemberSelect2 command, string json_s, params object[] args)
        {

            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jqx.GridResponse<MemberRow> data = new jqx.GridResponse<MemberRow>();
                StringBuilder sql = new StringBuilder();
                int cnt = 0;
                foreach (jqx._filter f in base.filters)
                {
                    switch (f.datafield)
                    {
                        case "CorpID": jqx._filter.add_sql(f, sql, ref cnt, "a.CorpID = {0}", jqx._filter.parseInt32); break;
                        default: continue;
                    }
                }
                //int cnt = 0;
                //sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", this.CorpID);
                //sql_where(sql, ref cnt, "a.ParentID='{0}'", this.ParentID);
                //sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (this.AgentACNT * text.ValidAsACNT).Remove("%"));
                //sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (this.ACNT * text.ValidAsACNT).Remove("%"));
                //sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (this.Name * text.ValidAsName).Remove("%"));
                //sql_where(sql, ref cnt, "a.Locked={0}", (byte?)this.Locked);
                //this.sort = this.sort ?? sort_default;

                string sql1 = string.Format("select count(*) from Member a with(nolock) left join Agent b {0} on a.ParentID=b.ID", sql);
                string sql2 = string.Format(@"select * from (
select row_number() over (order by {2}) as rowid, b.ACNT ParentACNT, a.*, c.IP as LoginIP, c.LoginTime
from Member a with(nolock) left join Agent b with(nolock) on a.ParentID=b.ID left join LoginState c with(nolock) on a.ID=c.ID {3}) a
where rowid>{0} and rowid<={1} order by rowid", this.rows_start, this.rows_end, this.GetSort(), sql);
                data.totalrecords = sqlcmd.ExecuteScalar<int>(sql1);
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql2))
                    data.rows.Add(r.ToObject<MemberRow>());
                return data;
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MemberSelect : jgrid.GridRequest<MemberSelect>
    {
        protected override string init_defaultkey() { return "a.CreateTime"; }
        protected override Dictionary<string, string> init_sortkeys()
        {
            return new Dictionary<string, string>()
        {
            {"ParentACNT", "b.ACNT"},
            {"LoginTime", "c.LoginTime"},
            {"LoginIP", "c.IP"},
            {"ID", "a.ID"},
            {"CorpID", "a.CorpID"},
            {"ACNT", "a.ACNT"},
            {"Name", "a.Name"},
            {"GroupID", "a.CorpID {0}, a.GroupID"},
            {"Locked", "a.Locked"},
            {"Balance", "a.Balance"},
            {"Currency", "a.Currency"},
            {"Memo", "a.Memo"},
            {"RegisterIP", "a.RegisterIP"},
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

        [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Read)]
        jgrid.GridResponse<MemberRow> OnExecute(MemberSelect command1, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<MemberRow> data = new jgrid.GridResponse<MemberRow>();
                StringBuilder sql = new StringBuilder(@"from Member a with(nolock) left join Agent b with(nolock) on a.ParentID=b.ID left join LoginState c with(nolock) on a.ID=c.ID");
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
select row_number() over (order by {2}) as rowid, b.ACNT ParentACNT, a.*, c.IP as LoginIP, c.LoginTime
{3}) a where rowid>{0} and rowid<={1} order by rowid", this.rows_start, this.rows_end, this.GetOrderBy(), sql))
                    data.rows.Add(r.ToObject<MemberRow>());
                return data;
            }
        }

        [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
        static object execute(MemberInsert command, string json_s, params object[] args) { return command.insert(null, null, json_s, args); }

        [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
        static object execute(MemberUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MemberInsert : MemberRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MemberUpdate : MemberRowCommand, IRowCommand { }

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

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MemberBankCardUpdate : MemberBankCardRowCommand, IRowCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
        static object execute(MemberBankCardUpdate command, string json_s, params object[] args)
        {
            command.throwNotFound = false;
            return command.update(json_s, args);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MemberBankCardInsert : MemberBankCardRowCommand, IRowCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
        static object execute(MemberBankCardInsert command, string json_s, params object[] args)
        {
            command.throwNotFound = false;
            return command.insert(json_s, args);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MemberBankCardDelete : MemberBankCardRowCommand, IRowCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
        static object execute(MemberBankCardDelete command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.GetRowEx<MemberBankCardRow>(RowErrorCode.NotFound, "select * from MemberBank nolock where MemberID={0} and [Index]={1} delete MemberBank where MemberID={0} and [Index]={1}", command.MemberID, command.Index);
        }
    }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class MemberGameSelect : IRowCommand
    //{
    //    [JsonProperty]
    //    GameID? GameID;
    //    [JsonProperty]
    //    int? MemberID;

    //    [ObjectInvoke, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Read)]
    //    static MemberGameRow execute(MemberGameSelect command, string json_s, params object[] args)
    //    {
    //        return MemberGame.GetInstance(command.GameID).SelectRow(null, command.MemberID, true);
    //    }
    //}

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class MemberGameUpdate : IRowCommand
    //{
    //    [JsonProperty]
    //    GameID? GameID;

    //    [ObjectInvoke, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
    //    static object execute(MemberGameUpdate command, string json_s, params object[] args)
    //    {
    //        return MemberGame.GetInstance(command.GameID).DeserializeObject(json_s).Update(json_s, args);
    //    }
    //}
}