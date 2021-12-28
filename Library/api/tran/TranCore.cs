using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using Tools;
using Tools.Protocol;

namespace web
{
    public static partial class tran { }

    partial class tran
    {
        static decimal? amount_valid(this decimal? value)
        {
            if (value.HasValue)
                if (value.Value <= 0)
                    return null;
            return value;
        }

        static AgentRow GetProvider(this UserRow user, SqlCmd sqlcmd)
        {
            int? parentID = user.ParentID;
            for (int i = 0; i < 100; i++)
            {
                AgentRow _provider = AgentRow.GetAgentEx(sqlcmd, RowErrorCode.ProviderNotFound, parentID, null, null, "*");
                if ((_provider.ParentID == 0) || (_provider.PCT != 0))
                    return _provider;
                parentID = _provider.ParentID;
            }
            throw new RowException(RowErrorCode.ProviderNotFound);
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public abstract class RowData
        {
            [DbImport, JsonProperty]
            public Guid ID;                 // 
            [DbImport, JsonProperty]
            public LogType LogType;         //
            //public virtual GameID GameID { get; set; }
            [DbImport, JsonProperty]
            public int CorpID;              //
            [DbImport, JsonProperty]
            public int ProviderParentID;          // 撥分上級
            [DbImport, JsonProperty]
            public string ProviderParentACNT;     //
            [DbImport, JsonProperty]
            public int ProviderID;          // 撥分上級
            [DbImport, JsonProperty]
            public string ProviderACNT;     //
            [DbImport, JsonProperty]
            public int ParentID;            // 所屬上級
            [DbImport, JsonProperty]
            public string ParentACNT;       //
            [DbImport, JsonProperty]
            public UserType UserType;       //
            [DbImport, JsonProperty]
            public int UserID;              //
            [DbImport, JsonProperty]
            public string UserACNT;         //
            [DbImport, JsonProperty]
            public string UserName;         //
            [DbImport, JsonProperty]
            public decimal Amount;          //
            [DbImport, JsonProperty]
            public TranState State;         //
            [DbImport, JsonProperty]
            public CurrencyCode CurrencyA;  //
            [DbImport, JsonProperty]
            public CurrencyCode CurrencyB;  //
            [DbImport, JsonProperty]
            public decimal CurrencyX;       //
            [DbImport, JsonProperty]
            public string SerialNumber;     //
            [DbImport, JsonProperty]
            public string RequestIP;        //
            [DbImport, JsonProperty]
            public DateTime? AcceptTime;    //
            [DbImport, JsonProperty]
            public DateTime? FinishTime;    //
            [DbImport, JsonProperty]
            public DateTime CreateTime;     //
            [DbImport, JsonProperty]
            public _SystemUser CreateUser;          //
            [DbImport, JsonProperty]
            public DateTime ModifyTime;     //
            [DbImport, JsonProperty]
            public _SystemUser ModifyUser;          //
            [DbImport, JsonProperty]
            public string Memo1;            //
            [DbImport, JsonProperty]
            public string Memo2;            //

            [JsonProperty]
            public int? _row_delete;
            [JsonProperty]
            public decimal? ProviderBalance;
            [JsonProperty]
            public decimal? UserBalance;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public abstract class RowCommand
        {
            [JsonProperty]
            public Guid? ID;
            [JsonProperty]
            public int? CorpID;
            [JsonProperty]
            public UserType? UserType;
            [JsonProperty]
            public int? UserID;
            [JsonProperty]
            public string UserACNT;
            [JsonProperty]
            public decimal? Amount;
            [JsonProperty, JsonProtocol.String(Empty = true)]
            public string Memo1;
            [JsonProperty, JsonProtocol.String(Empty = true)]
            public string Memo2;

            [JsonProperty]
            public LogType? LogType;
            public virtual string prefix
            {
                get { return tranAttribute.GetPrefix(this.LogType, this.AcceptLogTypes); }
            }

            public abstract StringEx.sql_str TranTable1 { get; }
            public abstract StringEx.sql_str TranTable2 { get; }
            public abstract LogType[] AcceptLogTypes { get; }
            internal StringEx.sql_str UserTable
            {
                get
                {
                    if (this.UserType == BU.UserType.Agent) return _null<AgentRow>.value.TableName2;
                    if (this.UserType == BU.UserType.Member) return _null<MemberRow>.value.TableName2;
                    return null;
                }
            }
            static readonly TranState State_Initial = BU.TranState.Initial;
            static readonly TranState State_Accepted = BU.TranState.Accepted;
            static readonly TranState State_Rejected = BU.TranState.Rejected;
            static readonly TranState State_Transferred = BU.TranState.Transferred;
            static readonly TranState State_Failed = BU.TranState.Failed;

            public int ModifyUser
            {
                get
                {
                    User user = HttpContext.Current.User as User;
                    if (user != null)
                        return user.ID;
                    else
                        return 0;
                }
            }

            [JsonProperty]
            public bool? op_Insert;     // 新增
            [JsonProperty]
            public bool? op_Update;     // 更新
            [JsonProperty]
            public bool? op_Delete;     // 刪除
            [JsonProperty("Accept")]
            public bool? op_Accept;     // 預扣
            [JsonProperty("Finish")]
            public bool? op_Finish;     // 完成

            protected bool IsDeleted;
            protected decimal? ProviderBalance;
            protected decimal? UserBalance;
        }

        internal class tranAttribute : Attribute
        {
            public StringEx.sql_str Table1;
            public StringEx.sql_str Table2;
            public LogType[] AcceptLogTypes;
            public bool insert_acceptAgent = true;
            public bool insert_acceptMember = true;

            public tranAttribute(string table1, string table2, params LogType[] acceptLogTypes)
            {
                this.Table1 = table1;
                this.Table2 = table2;
                if ((acceptLogTypes.Length == 1) && (acceptLogTypes[0] == LogType.Promos))
                {
                    List<LogType> list = new List<LogType>();
                    foreach (LogType l1 in Enum.GetValues(typeof(LogType)))
                    {
                        if (l1 <= BU.LogType.Promos) continue;
                        if (l1 >= BU.LogType.PromosMAX) continue;
                        if (l1 == BU.LogType.BetAmt) continue;
                        if (l1 == BU.LogType.FirstDeposit) continue;
                        if (l1 == BU.LogType.SecondDeposit) continue;
                        if (l1 == BU.LogType.AgentShare) continue;
                        list.Add(l1);
                    }
                    this.AcceptLogTypes = list.ToArray();
                }
                else
                    this.AcceptLogTypes = acceptLogTypes;
            }

            public static string GetPrefix(LogType? logType, LogType[] acceptLogTypes)
            {
                if (!logType.HasValue) return null;
                foreach (LogType lt in acceptLogTypes)
                {
                    if (lt != logType.Value) continue;
                    if (lt == BU.LogType.Deposit) return "A";
                    if (lt == BU.LogType.Withdrawal) return "B";
                    if (lt == BU.LogType.GameDeposit) return "C";
                    if (lt == BU.LogType.GameWithdrawal) return "D";
                    if (lt == BU.LogType.Dinpay) return "IA";
                    if (lt == BU.LogType.YeePay) return "IB";
                    if (lt == BU.LogType.Ecpss) return "IC";
                    if (lt == BU.LogType.Alipay) return "J";
                    if (lt == BU.LogType.TenPay) return "K";
                    if (lt == BU.LogType.BetAmt) return "G";
                    if (lt == BU.LogType.AgentShare) return "G";
                    if (lt == BU.LogType.FirstDeposit) return "E";
                    if (lt == BU.LogType.SecondDeposit) return "E";
                    if (lt == BU.LogType.LoadingBalance) return "PD";
                    if (lt == BU.LogType.UnloadingBalance) return "PW";
                    if ((lt > BU.LogType.Promos) && (lt < BU.LogType.PromosMAX)) return "K";
                    break;
                }
                return null;
            }
        }

        public abstract partial class RowCommand<TRowData, TRowCommand> : RowCommand, IDisposable
            where TRowData : RowData, new()
            where TRowCommand : RowCommand<TRowData, TRowCommand>, new()
        {
            internal static tranAttribute tran_attr;
            public override StringEx.sql_str TranTable1
            {
                get { return tran_attr.Table1; }
            }
            public override StringEx.sql_str TranTable2
            {
                get { return tran_attr.Table2; }
            }
            public override LogType[] AcceptLogTypes
            {
                get { return tran_attr.AcceptLogTypes; }
            }
            static RowCommand()
            {
                foreach (tranAttribute a in typeof(TRowCommand).GetCustomAttributes(typeof(tranAttribute), true))
                    tran_attr = a;
            }

            static StringEx.sql_str s_TranTable_Fields;
            protected StringEx.sql_str TranTable_Fields
            {
                get
                {
                    lock (typeof(TRowCommand))
                    {
                        if (s_TranTable_Fields.value == null)
                        {
                            SqlSchemaTable schema = SqlSchemaTable.GetSchema(sqlcmd, (string)this.TranTable1);
                            StringBuilder f = new StringBuilder("[ID]");
                            foreach (string field in schema.Keys)
                            {
                                if (field == "ID") continue;
                                f.AppendFormat(",[{0}]", field);
                            }
                            s_TranTable_Fields = (StringEx.sql_str)f.ToString();
                        }
                        return s_TranTable_Fields;
                    }
                }
            }

            protected virtual TRowData GetTranRow(BU.RowErrorCode? err, bool and_LogType)
            {
                TRowData tranrow;
                if (and_LogType)
                {
                    tranrow = null;
                    if (this.ID.HasValue)
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.AppendFormat("select * from {0} nolock where ID='{1}' and LogType", this.TranTable1, this.ID);
                        if (this.AcceptLogTypes.Length == 1)
                            sql.AppendFormat("={0}", (int)this.AcceptLogTypes[0]);
                        else
                        {
                            sql.Append(" in (");
                            for (int i = 0; i < this.AcceptLogTypes.Length; i++)
                            {
                                if (i > 0) sql.Append(",");
                                sql.Append((int)this.AcceptLogTypes[i]);
                            }
                            sql.Append(")");
                        }
                        tranrow = sqlcmd.ToObject<TRowData>(sql.ToString());
                    }
                }
                else
                {
                    tranrow = sqlcmd.ToObject<TRowData>("select * from {TranTable1} with(nolock) where ID={ID}".SqlExport(this));
                    if (tranrow == null)
                        throw new RowException(RowErrorCode.NoResult);
                }
                if ((tranrow == null) && (err.HasValue))
                    throw new RowException(err.Value);
                return tranrow;
            }

            protected internal UserRow GetUserEx(UserType? userType, int? userID, int? corpID, string acnt, params string[] fields)
            {
                if (tran_attr.insert_acceptAgent && (userType == BU.UserType.Agent))
                    return AgentRow.GetAgentEx(sqlcmd, userID, corpID, acnt, fields);
                if (tran_attr.insert_acceptMember && (userType == BU.UserType.Member))
                    return MemberRow.GetMemberEx(sqlcmd, userID, corpID, acnt, fields);
                throw new RowException(BU.RowErrorCode.InvaildUserType);
            }

            SqlCmd m_sqlcmd1;
            SqlCmd m_sqlcmd2;
            protected internal SqlCmd sqlcmd
            {
                [DebuggerStepThrough]
                get
                {
                    SqlCmd ret = this.m_sqlcmd2 ?? this.m_sqlcmd1;
                    if (ret == null)
                        ret = this.m_sqlcmd1 = DB.Open(DB.Name.Main, DB.Access.ReadWrite);
                    return ret;
                }
            }

            TRowData m_tranrow;
            protected internal virtual TRowData _tranrow
            {
                get { return m_tranrow; }
                set
                {
                    if (value != null)
                    {
                        this.ID = value.ID;
                        this.UserType = value.UserType;
                    }
                    this.m_tranrow = value;
                }
            }
            protected internal UserRow _user;

            public TRowData Execute(SqlCmd sqlcmd, string json_s, params object[] args)
            {
                try
                {
                    this.m_sqlcmd2 = sqlcmd;
                    this.execute(json_s, args);
                    if (_tranrow != null)
                    {
                        if (this.IsDeleted) _tranrow._row_delete = 1;
                        _tranrow.ProviderBalance = this.ProviderBalance;
                        _tranrow.UserBalance = this.UserBalance;
                    }
                    return _tranrow;
                }
                finally
                {
                    this.m_sqlcmd2 = null;
                }
            }

            void IDisposable.Dispose()
            {
                using (SqlCmd sqlcmd = this.m_sqlcmd1)
                    this.m_sqlcmd1 = null;
            }
            protected abstract void execute(string json_s, params object[] args);

            const string sql_user_add = @"declare @id uniqueidentifier set @id={ID}
select a.Balance as log_PrevBalance1 from {TranTable1} t with(nolock) left join {UserTable} a with(nolock) on a.ID=t.UserID where t.ID=@id
update {UserTable} set Balance=Balance+Amount from {UserTable} a left join {TranTable1} t on a.ID=t.UserID where t.ID=@id
select a.Balance as log_Balance1,  Amount as log_Amount1 from {TranTable1} t with(nolock) left join {UserTable} a with(nolock) on a.ID=t.UserID where t.ID=@id";
            const string sql_user_sub = @"declare @id uniqueidentifier set @id={ID}
select a.Balance as log_PrevBalance1 from {TranTable1} t with(nolock) left join {UserTable} a with(nolock) on a.ID=t.UserID where t.ID=@id
update {UserTable} set Balance=Balance-Amount from {UserTable} a left join {TranTable1} t on a.ID=t.UserID where Balance>=Amount and t.ID=@id
select a.Balance as log_Balance1, -Amount as log_Amount1 from {TranTable1} t with(nolock) left join {UserTable} a with(nolock) on a.ID=t.UserID where t.ID=@id";
            protected void UpdateUserBalance(bool add)
            {
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2((add ? sql_user_add : sql_user_sub).SqlExport(this)))
                    r.FillObject(this);
                CheckUserExist();
                if ((add == false) && (log_Balance1.Value == log_PrevBalance1.Value) && (this.log_Amount1 != 0))
                    throw new RowException(RowErrorCode.BalanceNotEnough);
            }
            protected void CheckUserExist()
            {
                if (log_Balance1.HasValue && log_PrevBalance1.HasValue)
                    this.UserBalance = log_Balance1;
                else
                {
                    if (_tranrow.UserType == BU.UserType.Agent)
                        throw new RowException(RowErrorCode.AgentNotFound);
                    if (_tranrow.UserType == BU.UserType.Member)
                        throw new RowException(RowErrorCode.MemberNotFound);
                    throw new RowException(RowErrorCode.InvaildUserType);
                }
            }
            const string sql_provider_add = @"declare @id uniqueidentifier set @id={ID}
select b.Balance as log_PrevBalance2 from {TranTable1} t with(nolock) left join Agent b with(nolock) on b.ID=t.ProviderID where t.ID=@id
update Agent set Balance=Balance+Amount from Agent b left join {TranTable1} t on b.ID=t.ProviderID where t.ID=@id
select b.Balance as log_Balance2,  Amount as log_Amount2 from {TranTable1} t with(nolock) left join Agent b with(nolock) on b.ID=t.ProviderID where t.ID=@id";
            const string sql_provider_sub = @"declare @id uniqueidentifier set @id={ID}
select b.Balance as log_PrevBalance2 from {TranTable1} t with(nolock) left join Agent b with(nolock) on b.ID=t.ProviderID where t.ID=@id
update Agent set Balance=Balance-Amount from Agent b left join {TranTable1} t on b.ID=t.ProviderID where t.ID=@id
select b.Balance as log_Balance2, -Amount as log_Amount2 from {TranTable1} t with(nolock) left join Agent b with(nolock) on b.ID=t.ProviderID where t.ID=@id";
            protected void UpdateProviderBalance(bool add)
            {
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2((add ? sql_provider_add : sql_provider_sub).SqlExport(this)))
                    r.FillObject(this);
                CheckProviderExist();
            }
            protected void CheckProviderExist()
            {
                if (log_Balance2.HasValue && log_PrevBalance2.HasValue)
                    this.ProviderBalance = log_Balance2;
                else
                    throw new RowException(RowErrorCode.ProviderNotFound);
            }
            bool execsql(string sqlstr)
            {
                int r = sqlcmd.ExecuteNonQuery(sqlstr.SqlExport(this));
                if (r > 1) throw new RowException(RowErrorCode.SystemError, "result > 1");
                return r == 1;
            }
            protected bool fees1() /*             */ { return execsql(@"update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, Fees1x=Fees1 where ID={ID} and Fees1x is null and Fees1<>0"); }
            protected bool fees2() /*             */ { return execsql(@"update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, Fees2x=Fees2 where ID={ID} and Fees2x is null and Fees2<>0"); }
            protected bool setTransferred_in() /* */ { return execsql(@"update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, FinishTime=getdate(), [State]={State_Transferred} where ID={ID} and AcceptTime is null and FinishTime is null"); }
            protected bool setTransferred_out() /**/ { return execsql(@"update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, FinishTime=getdate(), [State]={State_Transferred} where ID={ID} and AcceptTime is not null and FinishTime is null"); }
            protected bool setAccepted() /*       */ { return execsql(@"update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, AcceptTime=getdate(), [State]={State_Accepted} where ID={ID} and AcceptTime is null and FinishTime is null"); }
            protected bool setRejected() /*       */ { return execsql(@"update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, FinishTime=getdate(), [State]={State_Rejected} where ID={ID} and AcceptTime is null and FinishTime is null"); }
            protected bool setFailed() /*         */ { return execsql(@"update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, FinishTime=getdate(), AcceptTime=null, [State]={State_Failed} where ID={ID} and AcceptTime is not null and FinishTime is null"); }

            [DbImport]
            protected decimal? log_PrevBalance1 { get; set; }
            [DbImport]
            protected decimal? log_PrevBalance2;
            [DbImport]
            protected decimal? log_Balance1;
            [DbImport]
            protected decimal? log_Balance2;
            [DbImport]
            protected decimal? log_Amount1;
            [DbImport]
            protected decimal? log_Amount2;

            protected BU.GameID? log_GameID;
            protected BU.LogType? log_LogType;
            protected decimal? log_CashAmount;
            protected decimal? log_CashPCT;
            protected decimal? log_CashFees;
            protected decimal? log_BetAmount;
            protected decimal? log_BetBonus;
            protected decimal? log_BetPayout;
            protected decimal? log_BetShare;
            protected decimal? log_Values
            {
                set
                {
                    log_CashAmount =
                    log_CashPCT =
                    log_CashFees =
                    log_BetAmount =
                    log_BetBonus =
                    log_BetPayout =
                    log_PrevBalance1 =
                    log_PrevBalance2 =
                    log_Balance1 =
                    log_Balance2 =
                    log_Amount1 =
                    log_Amount2 = value;
                }
            }
            protected Guid? log_CashChannelID;

            static StringEx.sql_str _ct = "@ct";
            protected TranLogRow WriteTranLog(TRowData row, bool isProvider, params object[] args)
            {
                decimal? amount = isProvider ? log_Amount2 : log_Amount1;
                if ((amount == 0) && (log_CashAmount == 0) && (log_CashPCT == 0) && (log_CashFees == 0)) return null;
                SqlSchemaTable schema = SqlSchemaTable.GetSchema(sqlcmd, "TranLog");
                sqltool s = new sqltool();
                s[schema, "     ", "TranID", "      "] = row.ID;
                s[schema, "     ", "LogType", "     "] = log_LogType ?? row.LogType;
                s[schema, "     ", "GameID", "      "] = log_GameID ?? 0;
                s[schema, "     ", "CorpID", "      "] = row.CorpID;
                s[schema, "     ", "SerialNumber", ""] = row.SerialNumber;
                s[schema, "     ", "RequestIP", "   "] = row.RequestIP;
                s[schema, "     ", "CreateTime", "  "] = _ct;
                s[schema, "     ", "CashAmount", "  "] = this.log_CashAmount ?? 0;
                s[schema, "     ", "CashPCT", "     "] = this.log_CashPCT ?? 0;
                s[schema, "     ", "CashFees", "    "] = this.log_CashFees ?? 0;
                s[schema, "     ", "BetAmount", "   "] = this.log_BetAmount ?? 0;
                s[schema, "     ", "BetBonus", "    "] = this.log_BetBonus ?? 0;
                s[schema, "     ", "BetPayout", "   "] = this.log_BetPayout ?? 0;
                s[schema, "     ", "BetShare", "    "] = this.log_BetShare ?? 0;
                s[schema, "     ", "CashChannelID", "    "] = this.log_CashChannelID;
                if (this is IUserLoadBalance)
                {
                    s[schema, " ", "ACTime", "      "] = (StringEx.sql_str)"dbo.toACTime(@ct)";
                    s[schema, " ", "RequestTime", " "] = _ct;
                    s[schema, " ", "FinishTime", "  "] = _ct;
                }
                else
                {
                    s[schema, " ", "ACTime", "      "] = row.CreateTime.ToACTime().ToString(sqltool.DateFormat);
                    s[schema, " ", "RequestTime", " "] = row.CreateTime;
                    s[schema, " ", "FinishTime", "  "] = row.FinishTime;
                }
                if (isProvider)
                {
                    s[schema, " ", "ParentID", "    "] = row.ProviderParentID;
                    s[schema, " ", "ParentACNT", "  "] = row.ProviderParentACNT ?? "";
                    s[schema, " ", "UserType", "    "] = BU.UserType.Agent;
                    s[schema, " ", "UserID", "      "] = row.ProviderID;
                    s[schema, " ", "UserACNT", "    "] = row.ProviderACNT;
                    s[schema, " ", "IsProvider", "  "] = 1;
                    s[schema, " ", "PrevBalance", " "] = log_PrevBalance2;
                    s[schema, " ", "Balance", "     "] = log_Balance2;
                    s[schema, " ", "Amount", "      "] = log_Amount2;
                    s[schema, " ", "CurrencyA", "   "] = row.CurrencyB;
                    s[schema, " ", "CurrencyB", "   "] = row.CurrencyA;
                    s[schema, " ", "CurrencyX", "   "] = (StringEx.sql_str)"1/{_CurrencyX}";
                }               
                else            
                {
                    s[schema, " ", "ParentID", "    "] = row.ParentID;
                    s[schema, " ", "ParentACNT", "  "] = row.ParentACNT ?? "";
                    s[schema, " ", "UserType", "    "] = row.UserType;
                    s[schema, " ", "UserID", "      "] = row.UserID;
                    s[schema, " ", "UserACNT", "    "] = row.UserACNT;
                    s[schema, " ", "IsProvider", "  "] = 0;
                    s[schema, " ", "PrevBalance", " "] = log_PrevBalance1;
                    s[schema, " ", "Balance", "     "] = log_Balance1;
                    s[schema, " ", "Amount", "      "] = log_Amount1;
                    s[schema, " ", "CurrencyA", "   "] = row.CurrencyA;
                    s[schema, " ", "CurrencyB", "   "] = row.CurrencyB;
                    s[schema, " ", "CurrencyX", "   "] = row.CurrencyX;
                }
                s.values["_CurrencyA"] = row.CurrencyA;
                s.values["_CurrencyB"] = row.CurrencyB;
                s.values["_CurrencyX"] = row.CurrencyX;
                return sqlcmd.ToObject<TranLogRow>(s.BuildEx2("declare @ct datetime set @ct=getdate() insert into TranLog (", sqltool._Fields, @")
values (", sqltool._Values, @")
select * from TranLog nolock where sn=@@Identity"));
            }

            protected void DeleteTranRow()
            {
                sqlcmd.ExecuteNonQuery(@"insert into {TranTable2}
({TranTable_Fields}) select
 {TranTable_Fields}
  from {TranTable1} nolock where ID={ID}
delete {TranTable1}        where ID={ID}".SqlExport(this));
            }
        }

        interface IUserLoadBalance { }
        [tran(null, null, BU.LogType.LoadingBalance, BU.LogType.UnloadingBalance)]
        public class UserLoadBalance : RowCommand<UserLoadBalance._row, UserLoadBalance>, IUserLoadBalance
        {
            public int? ParentID;

            public class _row : RowData { }

            const string sql1 = @"declare @b1 decimal(19,6), @b2 decimal(19,6)
select @b1=Balance from {UserTable} nolock where ID={UserID}
select @b2=Balance from Agent nolock where ID={ParentID}";
            const string sql2 = @"
select @b1 as log_PrevBalance1, Balance as log_Balance1, Balance-@b1 as log_Amount1 from {UserTable} nolock where ID={UserID}
select @b2 as log_PrevBalance2, Balance as log_Balance2, Balance-@b2 as log_Amount2 from Agent nolock where ID={ParentID}";
            const string sql_load = sql1 + @"
update {UserTable} set Balance=Balance + {Amount} where ID={UserID}
update Agent set Balance=Balance - {Amount} where ID={ParentID} and (Balance >= {Amount} or ParentID=0)" + sql2;
            const string sql_unload = sql1 + @"
update {UserTable} set Balance=Balance - {Amount} where ID={UserID} and Balance >= {Amount}
update Agent set Balance=Balance + {Amount} where ID={ParentID}" + sql2;

            protected override void execute(string json_s, params object[] args)
            {
                this.Amount = this.Amount.amount_valid();
                string sql;
                if (this.LogType == BU.LogType.LoadingBalance)
                    sql = sql_load;
                else if (this.LogType == BU.LogType.UnloadingBalance)
                    sql = sql_unload;
                else throw new RowException(RowErrorCode.InvaildLogType);
                
                UserRow user = GetUserEx(this.UserType, this.UserID, null, null, "ParentID", "Currency");
                AgentRow provider = AgentRow.GetAgentEx(sqlcmd, RowErrorCode.ProviderNotFound, this.ParentID = user.ParentID, null, null, "*");
                _tranrow = new _row()
                {
                    UserType = this.UserType.Value,
                    LogType = this.LogType.Value,
                    CorpID = user.CorpID.Value,
                    ProviderParentID = provider.ParentID ?? 0,
                    ProviderParentACNT = provider.ParentACNT,
                    ProviderID = provider.ID.Value,
                    ProviderACNT = provider.ACNT,
                    ParentID = provider.ID.Value,
                    ParentACNT = provider.ACNT,
                    UserID = user.ID.Value,
                    UserACNT = user.ACNT,
                    CurrencyA = provider.Currency.Value,
                    CurrencyB = user.Currency.Value,
                    CurrencyX = 1,
                    RequestIP = HttpContext.Current.RequestIP(),
                };

                try
                {
                    sqlcmd.BeginTransaction();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.SqlExport(this)))
                        r.FillObject(this);
                    CheckUserExist();
                    CheckProviderExist();
                    if (this.LogType.Value == BU.LogType.LoadingBalance)
                    {
                        if (log_Balance2.Value == log_PrevBalance2.Value)
                            throw new RowException(RowErrorCode.ProviderBalanceNotEnough);
                    }
                    else
                    {
                        if (log_Balance1.Value == log_PrevBalance1.Value)
                            throw new RowException(RowErrorCode.BalanceNotEnough);
                    }
                    sqlcmd.FillObject(_tranrow, @"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix} select @ID as ID, @SerialNumber as SerialNumber".SqlExport(this));
                    TranLogRow log1 = this.WriteTranLog(_tranrow, false);
                    TranLogRow log2 = this.WriteTranLog(_tranrow, true, log_LogType = this.LogType.Value == BU.LogType.LoadingBalance ? BU.LogType.LoadingBalanceToUser : BU.LogType.UnloadingBalanceFromUser);
                    sqlcmd.Commit();
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