using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using Tools;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace web
{

//    public static partial class tran__
//    {
//        #region public abstract class RowData

//        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//        public abstract class RowData
//        {
//            #region Fields

//            [DbImport, JsonProperty]
//            public Guid ID;
//            [DbImport, JsonProperty]
//            public LogType LogType;
//            [DbImport, JsonProperty]
//            public GameID GameID;
//            [DbImport, JsonProperty]
//            public int CorpID;
//            [DbImport, JsonProperty]
//            public UserType UserType;
//            [DbImport, JsonProperty]
//            public int UserID;
//            [DbImport, JsonProperty]
//            public string UserACNT;
//            [DbImport, JsonProperty]
//            public string UserName;
//            [DbImport, JsonProperty]
//            public int ParentID;
//            [DbImport, JsonProperty]
//            public string ParentACNT;
//            [DbImport, JsonProperty]
//            public decimal Amount1;
//            [DbImport, JsonProperty]
//            public DateTime? AcceptTime;
//            [JsonProperty]
//            bool IsAccepted { get { return this.AcceptTime.HasValue; } }
//            [DbImport, JsonProperty]
//            public TranState State;
//            public readonly TranState State_Initial = BU.TranState.Initial;
//            public readonly TranState State_Accepted = BU.TranState.Accepted;
//            public readonly TranState State_Rejected = BU.TranState.Rejected;
//            public readonly TranState State_Transferred = BU.TranState.Transferred;
//            public readonly TranState State_Failed = BU.TranState.Failed;

//            [DbImport, JsonProperty]
//            public CurrencyCode CurrencyA;
//            [DbImport, JsonProperty]
//            public CurrencyCode CurrencyB;
//            [DbImport, JsonProperty]
//            public decimal CurrencyX;
//            [DbImport, JsonProperty]
//            public string SerialNumber;
//            [DbImport, JsonProperty]
//            public string RequestIP;
//            [DbImport, JsonProperty]
//            public DateTime? FinishTime;
//            [DbImport, JsonProperty]
//            public DateTime CreateTime;
//            [DbImport, JsonProperty]
//            public int CreateUser;
//            [DbImport, JsonProperty]
//            public DateTime ModifyTime;
//            [DbImport, JsonProperty]
//            public int ModifyUser;

//            #endregion

//            public abstract StringEx.sql_str Table1 { get; }
//            public abstract StringEx.sql_str Table2 { get; }

//            static Dictionary<string, string> delete_sql = new Dictionary<string, string>();
//            public void delete_row(SqlCmd sqlcmd)
//            {
//                string sqlstr;
//                lock (delete_sql)
//                {
//                    string table1 = (string)this.Table1;
//                    string table2 = (string)this.Table2;
//                    if (!delete_sql.TryGetValue(table2, out sqlstr))
//                    {
//                        using (SqlDataReader r = sqlcmd.ExecuteReader("select top(0) * from {0} nolock", table2))
//                        {
//                            string[] fields = new string[r.FieldCount];
//                            for (int i = 0; i < r.FieldCount; i++)
//                                fields[i] = string.Format("[{0}]", r.GetName(i));
//                            delete_sql[table2] = sqlstr = string.Format(@"insert into {1} ({2})
//select {2}
//from {0} nolock where ID='{{0}}'
//delete {0} where ID='{{0}}'", table1, table2, string.Join(",", fields));
//                        }
//                    }
//                }
//                sqlcmd.ExecuteNonQuery(sqlstr, this.ID);
//                this._row_delete = 1;
//            }

//            [JsonProperty]
//            public int? _row_delete;
//        }

//        #endregion

//        static AgentRow get_provider(this AgentRow parent, SqlCmd sqlcmd)
//        {
//            AgentRow provider = parent;
//            while ((provider.ParentID != 0) && (provider.PCT == 0))
//                provider = AgentRow.GetAgentEx(sqlcmd, parent.ParentID, null, null, "*");
//            return provider;
//        }

//        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//        public abstract class RowCommand<TUser, TRowData, TRowCommand>
//            where TUser : UserRow, new()
//            where TRowData : RowData, new()
//            where TRowCommand : RowCommand<TUser, TRowData, TRowCommand>, new()
//        {
//            #region Fields

//            [JsonProperty]
//            public string ID;
//            [JsonProperty]
//            public virtual LogType? LogType { get; set; }
//            [JsonProperty]
//            public GameID? GameID;
//            [JsonProperty]
//            public int? CorpID;
//            [JsonProperty]
//            public int? UserID;
//            [JsonProperty]
//            public string UserACNT;
//            [JsonProperty]
//            public string UserName;
//            [JsonProperty]
//            public int? ParentID;
//            [JsonProperty]
//            public string ParentACNT;
//            [JsonProperty]
//            public decimal? Amount1;
//            [JsonProperty]
//            public DateTime? AcceptTime;
//            [JsonProperty]
//            public string State;
//            [JsonProperty]
//            public CurrencyCode? CurrencyA;
//            [JsonProperty]
//            public CurrencyCode? CurrencyB;
//            [JsonProperty]
//            public decimal? CurrencyX;
//            [JsonProperty]
//            public string SerialNumber;
//            [JsonProperty]
//            public string RequestIP;
//            [JsonProperty]
//            public DateTime? FinishTime;
//            [JsonProperty]
//            public DateTime? CreateTime;
//            [JsonProperty]
//            public int? CreateUser;
//            [JsonProperty]
//            public DateTime? ModifyTime;
//            [JsonProperty]
//            public int? ModifyUser;

//            #endregion

//            [JsonProperty("Accept")]
//            public bool? op_Accept;
//            [JsonProperty("Finish")]
//            public bool? op_Finish;

//            public bool? op_Insert;
//            public bool? op_Update;
//            public bool? op_Delete;

//            public UserType UserType { get { return _null<TUser>.value.UserType; } }
//            public LogType[] AcceptLogTypes
//            {
//                get
//                {
//                    lock (typeof(TRowData))
//                    {
//                        if (s_AcceptLogTypes == null)
//                            s_AcceptLogTypes = new List<BU.LogType>(this.initAcceptLogTypes()).ToArray();
//                        return s_AcceptLogTypes;
//                    }
//                }
//            }
//            protected abstract IEnumerable<LogType> initAcceptLogTypes();
//            static LogType[] s_AcceptLogTypes;
//            public abstract string prefix { get; }

//            public TRowData insert(string json_s, params object[] args) { this.op_Insert = true; return this.proc_tran(json_s, args); }
//            public TRowData update(string json_s, params object[] args) { this.op_Update = true; return this.proc_tran(json_s, args); }
//            public TRowData delete(string json_s, params object[] args) { this.op_Delete = true; return this.proc_tran(json_s, args); }

//            protected abstract TRowData proc_tran(string json_s, params object[] args);

//            internal protected void insert_fill(sqltool s, UserRow user, AgentRow provider, AgentRow parent)
//            {
//                s["* ", "LogType", "     "] = this.LogType;
//                s["* ", "GameID", "      "] = 0;
//                s["* ", "UserType", "    "] = this.UserType;
//                s["* ", "CorpID", "      "] = user.CorpID;
//                s["* ", "UserID", "      "] = user.ID;
//                s["* ", "UserACNT", "    "] = user.ACNT;
//                s["*N", "UserName", "    "] = user.Name;
//                s["* ", "ParentID", "    "] = parent.ID;
//                s["* ", "ParentACNT", "  "] = parent.ACNT;
//                s["* ", "ProviderID", "  "] = provider.ID;
//                s["* ", "ProviderACNT", ""] = provider.ACNT;
//                s["  ", "State", "       "] = TranState.Initial;
//                s["* ", "CurrencyA", "   "] = user.Currency;
//                s["* ", "CurrencyB", "   "] = user.Currency;
//                s["* ", "CurrencyX", "   "] = 1;
//                s["  ", "RequestIP", "   "] = HttpContext.Current.RequestIP();
//            }
//        }
     
//        #region public class BalanceRowData

//        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//        public class BalanceRowData : tran__.RowData
//        {
//            #region Fields

//            [DbImport, JsonProperty]
//            public int ProviderID;
//            [DbImport, JsonProperty]
//            public string ProviderACNT;

//            [DbImport, JsonProperty]
//            public decimal? Amount3;
//            [DbImport, JsonProperty]
//            public decimal? PCT;
//            [DbImport, JsonProperty]
//            public decimal Fees1;
//            [DbImport, JsonProperty]
//            public decimal Fees2;
//            [DbImport, JsonProperty]
//            public string a_BankName;
//            [DbImport, JsonProperty]
//            public string a_CardID;
//            [DbImport, JsonProperty]
//            public string a_Name;
//            [DbImport, JsonProperty]
//            public DateTime? a_TranTime;
//            [DbImport, JsonProperty]
//            public string a_TranSerial;
//            [DbImport, JsonProperty]
//            public string a_TranMemo;
//            [DbImport, JsonProperty]
//            public string b_BankName;
//            [DbImport, JsonProperty]
//            public string b_CardID;
//            [DbImport, JsonProperty]
//            public string b_Name;
//            [DbImport, JsonProperty]
//            public DateTime? b_TranTime;
//            [DbImport, JsonProperty]
//            public string b_TranSerial;
//            [DbImport, JsonProperty]
//            public string b_TranMemo;
//            [DbImport, JsonProperty]
//            public string Memo1;
//            [DbImport, JsonProperty]
//            public string Memo2;

//            #endregion

//            public override StringEx.sql_str Table1 { get { return "tranBalance1"; } }
//            public override StringEx.sql_str Table2 { get { return "tranBalance2"; } }
//        }

//        #endregion

//        #region public class PromoRowData

//        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//        public class PromoRowData : tran__.RowData
//        {
//            #region Fields

//            [DbImport, JsonProperty]
//            public int ProviderID;
//            [DbImport, JsonProperty]
//            public string ProviderACNT;

//            [DbImport, JsonProperty]
//            public string Memo1;
//            [DbImport, JsonProperty]
//            public string Memo2;

//            #endregion

//            public override StringEx.sql_str Table1 { get { return "tranPromo1"; } }
//            public override StringEx.sql_str Table2 { get { return "tranPromo2"; } }
//        }

//        #endregion

//        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//        public abstract class BalanceRowCommand<TUser, TRowData, TRowCommand> : RowCommand<TUser, TRowData, TRowCommand>
//            where TUser : UserRow, new()
//            where TRowData : BalanceRowData, new()
//            where TRowCommand : BalanceRowCommand<TUser, TRowData, TRowCommand>, new()
//        {
//            #region Fields

//            [JsonProperty]
//            public decimal? Amount3;
//            [JsonProperty]
//            public decimal? PCT;
//            [JsonProperty]
//            public decimal? Fees1;
//            [JsonProperty]
//            public decimal? Fees2;
//            [JsonProperty]
//            public string a_BankName;
//            [JsonProperty]
//            public string a_CardID;
//            [JsonProperty]
//            public string a_Name;
//            [JsonProperty]
//            public DateTime? a_TranTime;
//            [JsonProperty]
//            public string a_TranSerial;
//            [JsonProperty]
//            public string a_TranMemo;
//            [JsonProperty]
//            public string b_BankName;
//            [JsonProperty]
//            public string b_CardID;
//            [JsonProperty]
//            public string b_Name;
//            [JsonProperty]
//            public DateTime? b_TranTime;
//            [JsonProperty]
//            public string b_TranSerial;
//            [JsonProperty]
//            public string b_TranMemo;
//            [JsonProperty]
//            public string Memo1;
//            [JsonProperty]
//            public string Memo2;

//            #endregion

//            //protected abstract bool IsDeposit { get; }

//            public override string prefix
//            {
//                get
//                {
//                    if (this.AcceptLogTypes.Conatins(this.LogType))
//                    {
//                        switch (this.LogType.Value)
//                        {
//                            case BU.LogType.Deposit: return "A";
//                            case BU.LogType.Withdrawal: return "B";
//                            case BU.LogType.Dinpay: return "I";
//                            case BU.LogType.Alipay: return "J";
//                        }
//                    }
//                    return null;
//                }
//            }

//            TRowData exec_insert(string json_s, params object[] args)
//            {
//                SqlCmd sqlcmd; UserRow user; AgentRow parent; AgentRow agent; MemberRow member;
//                if (this.Amount1 <= 0) this.Amount1 = null;
//                if (this.Amount3 <= 0) this.Amount3 = null;
//                if (this.Fees1 <= 0) this.Fees1 = null;
//                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
//                {
//                    sqltool s = new sqltool();
//                    s.Values["table_tran"] = _null<TRowData>.value.Table1;
//                    if ((s.Values["prefix"] = this.prefix) == null)
//                        this.LogType = null;
                    
//                    if (this.UserType == BU.UserType.Agent)
//                    {
//                        user = agent = AgentRow.GetAgentEx(sqlcmd, this.UserID, this.CorpID, this.UserACNT, "*");
//                        parent = AgentRow.GetAgentEx(sqlcmd, user.ParentID, null, null, "*");
//                        this.PCT = this.PCT ?? agent.PCT ?? 0;
//                    }
//                    else if (this.UserType == BU.UserType.Member)
//                    {
//                        user = member = MemberRow.GetMemberEx(sqlcmd, this.UserID, this.CorpID, this.UserACNT, "*");
//                        parent = AgentRow.GetAgentEx(sqlcmd, user.ParentID, null, null, "*");
//                        this.PCT = this.PCT ?? 0;
//                        this.Amount3 = null;
//                    }
//                    else throw new RowException(BU.RowErrorCode.InvaildUserType);

//                    AgentRow provider = parent.get_provider(sqlcmd);
//                    base.insert_fill(s, user, provider, parent);
//                    string amount1, amount3;
//                    if ((this.PCT.Value >= 1) || (this.PCT < 0)) this.PCT = null;
//                    if (this.Amount1.HasValue)
//                    {
//                        amount1 = "{Amount1}";
//                        amount3 = "{Amount1} * (1-{PCT})";
//                    }
//                    else if (this.Amount3.HasValue)
//                    {
//                        amount3 = "{Amount3}";
//                        amount1 = "{Amount3} / (1-{PCT})";
//                    }
//                    else amount1 = amount3 = null;
//                    s.values["Amount1"] = this.Amount1;
//                    s.values["Amount3"] = this.Amount3;
//                    s["* ", "Fees1", "       "] = this.Fees1 ?? 0;
//                    s["* ", "Fees2", "       "] = 0;
//                    s["* ", "PCT", "         "] = this.PCT;
//                    s[" N", "a_BankName", "  "] = this.a_BankName * text.ValidAsString;
//                    s["  ", "a_CardID", "    "] = this.a_CardID * text.ValidAsString;
//                    s[" N", "a_Name", "      "] = this.a_Name * text.ValidAsString;
//                    s["  ", "a_TranTime", "  "] = this.a_TranTime;
//                    s["  ", "a_TranSerial", ""] = this.a_TranSerial * text.ValidAsString;
//                    s[" N", "a_TranMemo", "  "] = this.a_TranMemo * text.ValidAsString;
//                    s[" N", "b_BankName", "  "] = this.b_BankName * text.ValidAsString;
//                    s["  ", "b_CardID", "    "] = this.b_CardID * text.ValidAsString;
//                    s[" N", "b_Name", "      "] = this.b_Name * text.ValidAsString;
//                    s["  ", "b_TranTime", "  "] = this.b_TranTime;
//                    s["  ", "b_TranSerial", ""] = this.b_TranSerial * text.ValidAsString;
//                    s[" N", "b_TranMemo", "  "] = this.b_TranMemo * text.ValidAsString;
//                    s[" N", "Memo1", "       "] = this.Memo1 * text.ValidAsString;
//                    s[" N", "Memo2", "       "] = this.Memo2 * text.ValidAsString;
//                    s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
//                    s.TestFieldNeeds();
//                    TRowData row = sqlcmd.ToObject<TRowData>(true, s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
//insert into {table_tran} (ID,SerialNumber,Amount1,Amount3,", sqltool._Fields, @")
//values (@ID,@SerialNumber,", amount1, ",", amount3, ",", sqltool._Values, @")
//select * from {table_tran} nolock where ID=@ID"));
//                    if (row == null) throw new RowException(RowErrorCode.NoResult);
//                    return row;
//                }
//            }

//            TRowData exec_update(string json_s, params object[] args)
//            {
//                SqlCmd sqlcmd;
//                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
//                {
//                    TRowData row = sqlcmd.GetRowEx<TRowData>(RowErrorCode.MemberTranNotFound, "select * from {0} nolock where ID='{1}'", _null<TRowData>.value.Table1, this.ID);
//                    sqltool s = new sqltool();
//                    s[" ", "Fees1", "       ", row.Fees1, "       "] = this.Fees1;
//                    s[" ", "Fees2", "       ", row.Fees2, "       "] = this.Fees2;
//                    s["N", "a_BankName", "  ", row.a_BankName, "  "] = this.a_BankName * text.ValidAsString;
//                    s[" ", "a_CardID", "    ", row.a_CardID, "    "] = this.a_CardID * text.ValidAsString;
//                    s["N", "a_Name", "      ", row.a_Name, "      "] = this.a_Name * text.ValidAsString;
//                    s[" ", "a_TranTime", "  ", row.a_TranTime, "  "] = this.a_TranTime;
//                    s[" ", "a_TranSerial", "", row.a_TranSerial, ""] = this.a_TranSerial * text.ValidAsString;
//                    s["N", "a_TranMemo", "  ", row.a_TranMemo, "  "] = this.a_TranMemo * text.ValidAsString;
//                    s["N", "b_BankName", "  ", row.b_BankName, "  "] = this.b_BankName * text.ValidAsString;
//                    s[" ", "b_CardID", "    ", row.b_CardID, "    "] = this.b_CardID * text.ValidAsString;
//                    s["N", "b_Name", "      ", row.b_Name, "      "] = this.b_Name * text.ValidAsString;
//                    s[" ", "b_TranTime", "  ", row.b_TranTime, "  "] = this.b_TranTime;
//                    s[" ", "b_TranSerial", "", row.b_TranSerial, ""] = this.b_TranSerial * text.ValidAsString;
//                    s["N", "b_TranMemo", "  ", row.b_TranMemo, "  "] = this.b_TranMemo * text.ValidAsString;
//                    s["N", "Memo1", "       ", row.Memo1, "       "] = this.Memo1 * text.ValidAsString;
//                    s["N", "Memo2", "       ", row.Memo2, "       "] = this.Memo2 * text.ValidAsString;
//                    if (s.fields.Count > 0)
//                    {
//                        s.Values["table_tran"] = _null<TRowData>.value.Table1;
//                        s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                        s.Values["ID"] = row.ID;
//                        string sqlstr = s.BuildEx("update {table_tran} set ", sqltool._FieldValue, " where ID={ID} select * from {table_tran} nolock where ID={ID}");
//                        row = sqlcmd.ExecuteEx<TRowData>(sqlstr);
//                    }
//                    return row;
//                }
//            }

//            protected TRowData deposit(string json_s, params object[] args)
//            {
//                throw new NotImplementedException();
//            }

//            protected TRowData withdrawal(string json_s, params object[] args)
//            {
//                throw new NotImplementedException();
//            }
//        }

//        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//        public abstract class PromoRowCommand<TUser, TRowData, TRowCommand> : RowCommand<TUser, TRowData, TRowCommand>
//            where TUser : UserRow, new()
//            where TRowData : PromoRowData, new()
//            where TRowCommand : PromoRowCommand<TUser, TRowData, TRowCommand>, new()
//        {
//            #region Fields

//            [JsonProperty]
//            public string Memo1;
//            [JsonProperty]
//            public string Memo2;

//            #endregion

//            protected override IEnumerable<BU.LogType> initAcceptLogTypes()
//            {
//                foreach (LogType l in Enum.GetValues(typeof(LogType)))
//                {
//                    if (l <= BU.LogType.Promos) continue;
//                    if (l >= BU.LogType.PromosMAX) continue;
//                    if (l == BU.LogType.BetAmt) continue;
//                    if (l == BU.LogType.FirstDeposit) continue;
//                    if (l == BU.LogType.AgentShare) continue;
//                    yield return l;
//                }
//            }

//            public override string prefix
//            {
//                get
//                {
//                    if (this.AcceptLogTypes.Conatins(this.LogType))
//                    {
//                        switch (this.LogType.Value)
//                        {
//                            case BU.LogType.FirstDeposit: return "E";
//                            case BU.LogType.BetAmt: return "G";
//                            case BU.LogType.AgentShare: return "F";
//                            default: return "P";
//                        }
//                    }
//                    return null;
//                }
//            }

//            TRowData _insert(string json_s, params object[] args)
//            {
//                if (this.Amount1 <= 0) this.Amount1 = null;
//                SqlCmd sqlcmd; UserRow user; AgentRow parent; AgentRow agent; MemberRow member;
//                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
//                {
//                    sqltool s = new sqltool();
//                    s.Values["table_tran"] = _null<TRowData>.value.Table1;
//                    if ((s.Values["prefix"] = this.prefix) == null)
//                        this.LogType = null;

//                    if (this.UserType == BU.UserType.Agent)
//                        user = agent = AgentRow.GetAgentEx(sqlcmd, this.UserID, this.CorpID, this.UserACNT, "*");
//                    else if (this.UserType == BU.UserType.Member)
//                        user = member = MemberRow.GetMemberEx(sqlcmd, this.UserID, this.CorpID, this.UserACNT, "*");
//                    else throw new RowException(BU.RowErrorCode.InvaildUserType);
//                    parent = AgentRow.GetAgentEx(sqlcmd, user.ParentID, null, null, "*");
                    
//                    AgentRow provider = parent.get_provider(sqlcmd);
//                    base.insert_fill(s, user, provider, parent);
//                    s[" *", "Amount1", "     "] = this.Amount1;
//                    s[" N", "Memo1", "       "] = this.Memo1 * text.ValidAsString;
//                    s[" N", "Memo2", "       "] = this.Memo2 * text.ValidAsString;
//                    s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
//                    s.TestFieldNeeds();
//                    TRowData row = sqlcmd.ToObject<TRowData>(true, s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
//insert into {table_tran} (ID,SerialNumber,", sqltool._Fields, @")
//values (@ID,@SerialNumber,", sqltool._Values, @")
//select * from {table_tran} nolock where ID=@ID"));
//                    if (row == null) throw new RowException(RowErrorCode.NoResult);
//                    return row;
//                }
//            }

//            TRowData _update(string json_s, params object[] args)
//            {
//                SqlCmd sqlcmd;
//                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
//                {
//                    TRowData row = sqlcmd.GetRowEx<TRowData>(RowErrorCode.MemberTranNotFound, "select * from {0} nolock where ID='{1}'", _null<TRowData>.value.Table1, this.ID);
//                    sqltool s = new sqltool();
//                    s["N", "Memo1", "       ", row.Memo1, "       "] = this.Memo1 * text.ValidAsString;
//                    s["N", "Memo2", "       ", row.Memo2, "       "] = this.Memo2 * text.ValidAsString;
//                    if (s.fields.Count > 0)
//                    {
//                        s.Values["table_tran"] = _null<TRowData>.value.Table1;
//                        s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                        s.Values["ID"] = row.ID;
//                        string sqlstr = s.BuildEx("update {table_tran} set ", sqltool._FieldValue, " where ID={ID} select * from {table_tran} nolock where ID={ID}");
//                        row = sqlcmd.ExecuteEx<TRowData>(sqlstr);
//                    }
//                    return row;
//                }
//            }

//            protected override TRowData proc_tran(string json_s, params object[] args)
//            {
//                throw new NotImplementedException();
//            }
//        }
//    }

//    #region  會員存款
//    public class MemberDepositRowCommand : tran__.BalanceRowCommand<MemberRow, tran__.BalanceRowData, MemberDepositRowCommand>
//    {
//        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return BU.LogType.Deposit; yield return BU.LogType.Dinpay; yield return BU.LogType.Alipay; }
//        //protected override bool IsDeposit { get { return true; } }

//        protected override tran__.BalanceRowData proc_tran(string json_s, params object[] args) { return base.deposit(json_s, args); }
//    }
//    #endregion
//    #region 會員提款

//    public class MemberWithdrawalRowCommand : tran__.BalanceRowCommand<MemberRow, tran__.BalanceRowData, MemberWithdrawalRowCommand>
//    {
//        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return BU.LogType.Withdrawal; }
//        //protected override bool IsDeposit { get { return false; } }

//        protected override tran__.BalanceRowData proc_tran(string json_s, params object[] args) { return base.withdrawal(json_s, args); }
//    }

//    #endregion
//    #region 代理存款

//    public class AgentDepositRowCommand : tran__.BalanceRowCommand<AgentRow, tran__.BalanceRowData, AgentDepositRowCommand>
//    {
//        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return BU.LogType.Deposit; yield return BU.LogType.Dinpay; yield return BU.LogType.Alipay; }
//        //protected override bool IsDeposit { get { return true; } }

//        protected override tran__.BalanceRowData proc_tran(string json_s, params object[] args) { return base.deposit(json_s, args); }
//    }
    
//    #endregion
//    #region 代理提款
    
//    public class AgentWithdrawalRowCommand : tran__.BalanceRowCommand<AgentRow, tran__.BalanceRowData, AgentWithdrawalRowCommand>
//    {
//        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return BU.LogType.Withdrawal; }
//        //protected override bool IsDeposit { get { return false; } }

//        protected override tran__.BalanceRowData proc_tran(string json_s, params object[] args) { return base.withdrawal(json_s, args); }
//    }
    
//    #endregion
//    #region 會員轉帳到遊戲

//    public class MemberGameDepositRowCommand : tran__.GameDepositRowCommand<MemberRow, MemberGameDepositRowCommand>
//    {
//    }

//    #endregion
//    #region 會員轉回主帳戶

//    public class MemberGameWithdrawalRowCommand : tran__.WithdrawalRowCommand<MemberRow, MemberGameWithdrawalRowCommand>
//    {
//    }

//    #endregion
//    #region 代理轉帳到遊戲

//    public class AgentGameDepositRowCommand : tran__.GameDepositRowCommand<AgentRow, AgentGameDepositRowCommand>
//    {
//    }
    
//    #endregion
//    #region 代理轉回主帳戶

//    public class AgentGameWithdrawalRowCommand : tran__.WithdrawalRowCommand<AgentRow, AgentGameWithdrawalRowCommand>
//    {
//    }

//    #endregion
//    #region 會員優惠

//    public class MemberPromoRowCommand : tran__.PromoRowCommand<MemberRow, tran__.PromoRowData, MemberPromoRowCommand>
//    {
//    }

//    #endregion
//    #region 代理優惠

//    public class AgentPromoRowCommand : tran__.PromoRowCommand<AgentRow, tran__.PromoRowData, AgentPromoRowCommand>
//    {
//    }

//    #endregion
//    #region 會員洗碼

//    public class MemberPromo_BetAmtRowCommand : tran__.PromoRowCommand<MemberRow, tran__.PromoRowData, MemberPromo_BetAmtRowCommand>
//    {
//        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return BU.LogType.BetAmt; }
//    }
    
//    #endregion
//    #region 代理洗碼

//    public class AgentPromo_BetAmtRowCommand : tran__.PromoRowCommand<AgentRow, tran__.PromoRowData, AgentPromo_BetAmtRowCommand>
//    {
//        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return BU.LogType.BetAmt; }
//    }
    
//    #endregion
//    #region 會員首存

//    public class MemberPromo_FirstDepositRowCommand : tran__.PromoRowCommand<MemberRow, tran__.PromoRowData, MemberPromo_FirstDepositRowCommand>
//    {
//        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return BU.LogType.FirstDeposit; }
//    }

//    #endregion
//    #region 代理佣金

//    public class AgentPromo_ShareRowCommand : tran__.PromoRowCommand<AgentRow, tran__.PromoRowData, AgentPromo_ShareRowCommand>
//    {
//        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return BU.LogType.AgentShare; }
//    }
    
//    #endregion
}