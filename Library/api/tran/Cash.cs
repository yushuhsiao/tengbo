using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using Tools;

namespace web
{
    partial class tran
    {
        public static partial class Cash
        {
            public abstract class _RowData : RowData
            {
                [DbImport, JsonProperty]
                public decimal CashAmount;
                [DbImport, JsonProperty]
                public decimal Fees1;
                [DbImport, JsonProperty]
                public decimal Fees2;
                [DbImport, JsonProperty]
                internal decimal? Fees1x;
                [DbImport, JsonProperty]
                internal decimal? Fees2x;
                [DbImport, JsonProperty]
                public decimal PCT;
                [DbImport, JsonProperty]
                public Guid? CashChannelID;
            }

            public abstract class _RowCommand<TRowData, TRowCommand> : tran.RowCommand<TRowData, TRowCommand>
                where TRowData : _RowData, new()
                where TRowCommand : _RowCommand<TRowData, TRowCommand>, new()
            {
                [JsonProperty]
                public decimal? CashAmount;
                [JsonProperty]
                public decimal? Fees1;
                [JsonProperty]
                public decimal? Fees2;
                [JsonProperty]
                public decimal? PCT;
                [JsonProperty]
                public Guid? CashChannelID;

                protected AgentRow _provider;

                protected virtual void insert_fill(sqltool s) { }
                protected void insert()
                {
                    if (this.prefix == null) throw new RowException(RowErrorCode.InvaildLogType);
                    _user = this.GetUserEx(this.UserType, this.UserID, this.CorpID, this.UserACNT, "*");
                    _provider = _user.GetProvider(sqlcmd);
                    #region 佔成
                    if (_user is AgentRow)
                    {
                        AgentRow agent = (AgentRow)_user;
                        this.PCT = this.PCT ?? agent.PCT ?? 0;
                    }
                    else
                    {
                        this.PCT = this.PCT ?? 0;
                        this.CashAmount = null;
                    }

                    if ((this.PCT.Value >= 1) || (this.PCT < 0)) this.PCT = null;
                    this.Amount = this.Amount.amount_valid();
                    this.CashAmount = this.CashAmount.amount_valid();
                    object amount, cash_amount;
                    if (this.Amount.HasValue)
                    {
                        amount = (StringEx.sql_str)"{_Amount}";
                        cash_amount = (StringEx.sql_str)"{_Amount} * (1-{PCT})";
                    }
                    else if (this.CashAmount.HasValue)
                    {
                        cash_amount = (StringEx.sql_str)"{_CashAmount}";
                        amount = (StringEx.sql_str)"{_CashAmount} / (1-{PCT})";
                    }
                    else amount = cash_amount = null;
                    #endregion
                    sqltool s = new sqltool();
                    s["* ", "LogType", "            "] = this.LogType;
                    s["* ", "CorpID", "             "] = _user.CorpID;
                    s["* ", "ProviderParentID", "   "] = _provider.ParentID ?? 0;
                    s["* ", "ProviderParentACNT", " "] = _provider.ParentACNT ?? "";
                    s["* ", "ProviderID", "         "] = _provider.ID ?? 0;
                    s["* ", "ProviderACNT", "       "] = _provider.ACNT ?? "";
                    s["* ", "ParentID", "           "] = _user.ParentID;
                    s["* ", "ParentACNT", "         "] = _user.ParentACNT;
                    s["* ", "UserType", "           "] = _user.UserType;
                    s["* ", "UserID", "             "] = _user.ID;
                    s["* ", "UserACNT", "           "] = _user.ACNT;
                    s["*N", "UserName", "           "] = _user.Name;
                    s["  ", "State", "              "] = TranState.Initial;
                    s["* ", "CurrencyA", "          "] = _user.Currency;
                    s["* ", "CurrencyB", "          "] = _provider.Currency;
                    s["* ", "CurrencyX", "          "] = 1;
                    s["  ", "RequestIP", "          "] = HttpContext.Current.RequestIP();
                    s[" N", "Memo1", "              "] = this.Memo1 * text.ValidAsString;
                    s[" N", "Memo2", "              "] = this.Memo2 * text.ValidAsString;
                    s["* ", "Amount", "             "] = amount;
                    s["* ", "CashAmount", "         "] = cash_amount;
                    s["* ", "Fees1", "              "] = this.Fees1.amount_valid() ?? 0;
                    s["* ", "Fees2", "              "] = this.Fees2.amount_valid() ?? 0;
                    s["* ", "PCT", "                "] = this.PCT;
                    s["  ", "CashChannelID", "      "] = this.CashChannelID;
                    this.insert_fill(s);
                    s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
                    s.TestFieldNeeds();
                    s.values["table_tran"] = this.TranTable1;
                    s.values["prefix"] = this.prefix;
                    s.values["_Amount"] = this.Amount;
                    s.values["_CashAmount"] = this.CashAmount;
                    TRowData tmp = sqlcmd.ToObject<TRowData>(true, s.BuildEx2(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
insert into {table_tran} (ID,SerialNumber,", sqltool._Fields, @")
values (@ID,@SerialNumber,", sqltool._Values, @")
select * from {table_tran} nolock where ID=@ID"));
                    if (tmp == null) throw new RowException(RowErrorCode.NoResult);
                    _tranrow = tmp;
                }

                protected virtual void update_fill(sqltool s) { }
                protected void update()
                {
                    _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    sqltool s = new sqltool();
                    s[" N", "Memo1", "   ", _tranrow.Memo1, "  "] = this.Memo1 * text.ValidAsString;
                    s[" N", "Memo2", "   ", _tranrow.Memo2, "  "] = this.Memo2 * text.ValidAsString;
                    this.update_fill(s);
                    if (this.Fees1.HasValue && (this.Fees1.Value != _tranrow.Fees1))
                    {
                        s["  ", "Fees1", "   "] = (StringEx.sql_str)"isnull(Fees1x,{Fees1_})";
                        s.values["Fees1_"] = this.Fees1;
                    }
                    if (this.Fees2.HasValue && (this.Fees2.Value != _tranrow.Fees2))
                    {
                        s["  ", "Fees2", "   "] = (StringEx.sql_str)"isnull(Fees2x,{Fees2_})";
                        s.values["Fees2_"] = this.Fees2;
                    }
                    if (s.fields.Count == 0) return;
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.values["table_tran"] = this.TranTable1;
                    s.values["ID"] = _tranrow.ID;
                    TRowData tmp = sqlcmd.ToObject<TRowData>(true, s.BuildEx2("update {table_tran} set ", sqltool._FieldValue, @" where ID={ID}
select * from {table_tran} nolock where ID={ID}"));
                    if (tmp == null) throw new RowException(RowErrorCode.NoResult);
                    _tranrow = tmp;
                }
            }

            public abstract class _DepositRowCommand<TRowData, TRowCommand> : _RowCommand<TRowData, TRowCommand>
                where TRowData : _RowData, new()
                where TRowCommand : _DepositRowCommand<TRowData, TRowCommand>, new()
            {
                protected override void execute(string json_s, params object[] args)
                {
                    if (op_Insert == true) this.insert();
                    if (op_Update == true) this.update();
                    if (op_Finish == true) this.finish();
                    if (op_Delete == true) this.delete();
                }

                protected void finish()
                {
                    TranLogRow log1, log2; _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        bool _state = setTransferred_in(), f1 = fees1(), f2 = fees2();
                        if (_state || f1 || f2)
                        {
                            TRowData tmp;
                            if (_state)
                            {
                                f1 = false;
                                UpdateUserBalance(true);
                                UpdateProviderBalance(false);
                                tmp = GetTranRow(RowErrorCode.NoResult, false);
                                log2 = this.WriteTranLog(tmp, false
                                    , log_CashPCT = tmp.PCT
                                    , log_CashAmount = -tmp.CashAmount
                                    , log_CashFees = null
                                    , log_CashChannelID = tmp.CashChannelID);
                                log1 = this.WriteTranLog(tmp, true
                                    , log_CashPCT = 1 - tmp.PCT
                                    , log_CashFees = -tmp.Fees1
                                    , log_CashAmount = tmp.CashAmount
                                    , log_CashChannelID = tmp.CashChannelID);
                            }
                            else tmp = GetTranRow(RowErrorCode.NoResult, false);
                            if (f1)
                                log1 = this.WriteTranLog(tmp, true, log_Values = 0, log_CashFees = -tmp.Fees1, log_CashChannelID = tmp.CashChannelID);
                            sqlcmd.Commit();
                            _tranrow = tmp;
                        } else sqlcmd.Rollback();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }

                protected void delete()
                {
                    TranLogRow log1, log2; _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        setRejected();
                        bool f1 = fees1(), f2 = fees2();
                        TRowData tmp = GetTranRow(RowErrorCode.NoResult, false);
                        if (f1)
                            log1 = this.WriteTranLog(tmp, true, log_Values = 0, log_CashFees = -tmp.Fees1);
                        DeleteTranRow();
                        sqlcmd.Commit();
                        _tranrow = tmp;
                        this.IsDeleted = true;
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }
//                protected override void execute(string json_s, params object[] args)
//                {
//                    if (op_Insert == true) this.insert();
//                    if (op_Update == true) this.update();
//                    if (op_Finish == true) this.finish();
//                    if (op_Delete == true) this.delete();
//                }

//                const string sql_finish1 = "update {TranTable1} set FinishTime=getdate(), [State]={State_Transferred}" + sql_finish_fees + ", ModifyTime=getdate(), ModifyUser={ModifyUser} where ID={ID} and FinishTime is null";
//                const string sql_finish2 = sql_deposit_finish2;
//                protected void finish()
//                {
//                    _tranrow = _tranrow ?? GetTranRow(true);
//                    try
//                    {
//                        sqlcmd.BeginTransaction();
//                        if (sqlcmd.ExecuteNonQuery(sql_finish1.SqlExport(this)) == 1)
//                        {
//                            TRowData tmp = UpdateBalance(sql_finish2, true, true, false);
//                            log_CashPCT = _tranrow.PCT;
//                            log_CashAmount = -_tranrow.CashAmount;
//                            log_CashFees = null;
//                            TranLogRow log1 = this.WriteTranLog(tmp, false);
//                            log_CashPCT = 1 - _tranrow.PCT;
//                            log_CashFees = -_tranrow.Fees1;
//                            log_CashAmount = _tranrow.CashAmount;
//                            TranLogRow log2 = this.WriteTranLog(tmp, true);
//                            sqlcmd.Commit();
//                            _tranrow = tmp;
//                        }
//                        else sqlcmd.Rollback();
//                    }
//                    catch
//                    {
//                        sqlcmd.Rollback();
//                        throw;
//                    }
//                }

//                const string sql_delete = @"declare @id uniqueidentifier set @id={ID}
//select 1 as n, * from {TranTable1} nolock where ID=@id
//update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}" + sql_finish_fees + @" where ID=@id and FinishTime is not null
//update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, FinishTime=getdate(),[State]={State_Rejected} where ID=@id and FinishTime is null
//select * from {TranTable1} nolock where ID=@id";
//                protected void delete()
//                {
//                    try
//                    {
//                        sqlcmd.BeginTransaction();
//                        TRowData row1 = null, row2 = null;
//                        foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql_delete.SqlExport(this)))
//                            if (r.HasValue("n"))
//                                row1 = r.ToObject<TRowData>();
//                            else
//                                row2 = r.ToObject<TRowData>();
//                        if ((row1 == null) || (row2 == null)) throw new RowException(RowErrorCode.NoResult);
//                        if (row1.Fees1x != row2.Fees1x)
//                        {
//                            log_Values = 0;
//                            log_CashFees = -row2.Fees1;
//                            TranLogRow log1 = this.WriteTranLog(row2, true);
//                        }
//                        DeleteTranRow();
//                        sqlcmd.Commit();
//                        _tranrow = row2;
//                        this.IsDeleted = true;
//                    }
//                    catch
//                    {
//                        sqlcmd.Rollback();
//                        throw;
//                    }
//                }
            }

            public abstract class _WithdrawalRowCommand<TRowData, TRowCommand> : _RowCommand<TRowData, TRowCommand>
                where TRowData : _RowData, new()
                where TRowCommand : _WithdrawalRowCommand<TRowData, TRowCommand>, new()
            {
                protected override void execute(string json_s, params object[] args)
                {
                    if (op_Insert == true) this.insert();
                    if (op_Update == true) this.update();
                    if (op_Accept == true) this.accept();
                    if (op_Finish == true) this.finish();
                    if (op_Delete == true) this.delete();
                }

                protected void accept()
                {
                    TranLogRow log1, log2; _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        if (setAccepted())
                        {
                            UpdateUserBalance(false);
                            TRowData tmp = GetTranRow(RowErrorCode.NoResult, false);
                            log1 = this.WriteTranLog(tmp, false, log_LogType = BU.LogType.WithdrawalWithholding, log_CashPCT = tmp.PCT);
                            sqlcmd.Commit();
                            _tranrow = tmp;
                        }
                        else sqlcmd.Rollback();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }

                protected void finish()
                {
                    TranLogRow log1, log2; _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        bool _state = setTransferred_out(), f1 = fees1(), f2 = fees2();
                        if (_state || f1 || f2)
                        {
                            TRowData tmp;
                            if (_state)
                            {
                                f1 = false;
                                log_Values = 0;
                                UpdateProviderBalance(true);
                                tmp = GetTranRow(RowErrorCode.NoResult, false);
                                log2 = this.WriteTranLog(tmp, false, log_LogType = null
                                    , log_CashPCT = tmp.PCT
                                    , log_CashAmount = tmp.CashAmount
                                    , log_CashFees = -tmp.Fees2);
                                log1 = this.WriteTranLog(tmp, true
                                    , log_CashPCT = 1 - tmp.PCT
                                    , log_CashAmount = -tmp.CashAmount
                                    , log_CashFees = -tmp.Fees1);
                            }
                            else tmp = GetTranRow(RowErrorCode.NoResult, false);
                            log_Values = 0;
                            if (f2)
                            {
                                if (!_state)
                                    log2 = this.WriteTranLog(tmp, false, log_CashFees = -tmp.Fees2);
                                log1 = this.WriteTranLog(tmp, true, log_CashFees = tmp.Fees2);
                            }
                            if (f1)
                                log1 = this.WriteTranLog(tmp, true, log_CashFees = -tmp.Fees1);
                            sqlcmd.Commit();
                            _tranrow = tmp;
                        }
                        else sqlcmd.Rollback();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }

                protected void delete()
                {
                    TranLogRow log1, log2; _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        bool _state1 = setRejected(), _state2 = setFailed();
                        TRowData tmp;
                        if (_state2)
                        {
                            UpdateUserBalance(true);
                            tmp = GetTranRow(RowErrorCode.NoResult, false);
                            log1 = this.WriteTranLog(tmp, false, log_LogType = BU.LogType.WithdrawalRollback);
                        }
                        else tmp = GetTranRow(RowErrorCode.NoResult, false);
                        if (!_state1 && !_state2)
                        {
                            bool f1 = fees1(),
                                f2 = fees2();
                            if (f1 || f2)
                            {
                                tmp = GetTranRow(RowErrorCode.NoResult, false);
                                log_Values = 0;
                                if (f2)
                                {
                                    log2 = this.WriteTranLog(tmp, false, log_CashFees = -tmp.Fees2);
                                    log1 = this.WriteTranLog(tmp, true, log_CashFees = tmp.Fees2);
                                }
                                if (f1)
                                    log1 = this.WriteTranLog(tmp, true, log_CashFees = -tmp.Fees1);
                            }
                        }
                        DeleteTranRow();
                        sqlcmd.Commit();
                        _tranrow = tmp;
                        this.IsDeleted = true;
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }
//                protected override void execute(string json_s, params object[] args)
//                {
//                    if (op_Insert == true) this.insert();
//                    if (op_Update == true) this.update();
//                    if (op_Accept == true) this.accept();
//                    if (op_Finish == true) this.finish();
//                    if (op_Delete == true) this.delete();
//                }

//                const string sql_accept1 = "update {TranTable1} set AcceptTime=getdate(), [State]={State_Accepted}, ModifyTime=getdate(), ModifyUser={ModifyUser} where ID={ID} and AcceptTime is null and FinishTime is null";
//                const string sql_accept2 = @"declare @id uniqueidentifier set @id={ID}
//select a.Balance as log_PrevBalance1 from {TranTable1} t with(nolock) left join {UserTable} a with(nolock) on a.ID=t.UserID where t.ID=@id
//update {UserTable} set Balance=Balance-Amount from {UserTable} a left join {TranTable1} t on a.ID=t.UserID where Balance>=Amount and t.ID=@id
//select a.Balance as log_Balance1, -Amount as log_Amount1 from {TranTable1} t with(nolock) left join {UserTable} a with(nolock) on a.ID=t.UserID where t.ID=@id";
//                protected void accept()
//                {
//                    _tranrow = _tranrow ?? GetTranRow(true);
//                    try
//                    {
//                        sqlcmd.BeginTransaction();
//                        if (sqlcmd.ExecuteNonQuery(sql_accept1.SqlExport(this)) == 1)
//                        {
//                            TRowData tmp = UpdateBalance(sql_accept2, true, false, true);
//                            log_CashPCT = tmp.PCT;
//                            log_LogType = BU.LogType.WithdrawalWithholding;
//                            TranLogRow log1 = this.WriteTranLog(tmp, false);
//                            sqlcmd.Commit();
//                            _tranrow = tmp;
//                        }
//                        else sqlcmd.Rollback();
//                    }
//                    catch
//                    {
//                        sqlcmd.Rollback();
//                        throw;
//                    }
//                }

//                const string sql_finish1 = "update {TranTable1} set FinishTime=getdate(), [State]={State_Transferred}" + sql_finish_fees + ", ModifyTime=getdate(), ModifyUser={ModifyUser} where ID={ID} and AcceptTime is not null and FinishTime is null";
//                const string sql_finish2 = @"declare @id uniqueidentifier set @id={ID}
//select b.Balance as log_PrevBalance2 from {TranTable1} t with(nolock) left join Agent b with(nolock) on b.ID=t.ProviderID where t.ID=@id
//update Agent set Balance=Balance+Amount from Agent b left join {TranTable1} t on b.ID=t.ProviderID where t.ID=@id
//select b.Balance as log_Balance2, Amount as log_Amount2 from {TranTable1} t with(nolock) left join Agent b with(nolock) on b.ID=t.ProviderID where t.ID=@id";
//                protected void finish()
//                {
//                    _tranrow = _tranrow ?? GetTranRow(true);
//                    try
//                    {
//                        sqlcmd.BeginTransaction();
//                        if (sqlcmd.ExecuteNonQuery(sql_finish1.SqlExport(this)) == 1)
//                        {
//                            log_LogType = null;
//                            TRowData tmp = UpdateBalance(sql_finish2, false, true, false);
//                            log_Balance1 = log_PrevBalance1 = log_Amount1 = 0;
//                            log_CashPCT = _tranrow.PCT;
//                            log_CashAmount = _tranrow.CashAmount;
//                            log_CashFees = -_tranrow.Fees2;
//                            TranLogRow log1 = this.WriteTranLog(tmp, false);
//                            log_CashPCT = 1 - _tranrow.PCT;
//                            log_CashFees = -_tranrow.Fees1;
//                            log_CashAmount = -_tranrow.CashAmount;
//                            TranLogRow log2 = this.WriteTranLog(tmp, true);
//                            sqlcmd.Commit();
//                            _tranrow = tmp;
//                        }
//                        else sqlcmd.Rollback();
//                    }
//                    catch
//                    {
//                        sqlcmd.Rollback();
//                        throw;
//                    }
//                }

//                const string sql_delete1 = @"declare @id uniqueidentifier set @id={ID}
//select 1 as n, * from {TranTable1} nolock where ID=@id
//update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}" + sql_finish_fees + @" where ID=@id and AcceptTime is not null and FinishTime is not null
//update {TranTable1} set ModifyTime=getdate(), ModifyUser={ModifyUser}, AcceptTime=null, FinishTime=getdate(),[State]=(case when AcceptTime is null then {State_Rejected} else {State_Failed} end) where ID=@id and FinishTime is null
//select * from {TranTable1} nolock where ID=@id";
//                const string sql_delete2 = @"declare @id uniqueidentifier set @id={ID}
//select a.Balance as log_PrevBalance1 from {TranTable1} t with(nolock) left join {UserTable} a with(nolock) on a.ID=t.UserID where t.ID=@id
//update {UserTable} set Balance=Balance+Amount from {UserTable} a left join {TranTable1} t on a.ID=t.UserID where t.ID=@id
//select a.Balance as log_Balance1, Amount as log_Amount1 from {TranTable1} t with(nolock) left join {UserTable} a with(nolock) on a.ID=t.UserID where t.ID=@id";
//                protected void delete()
//                {
//                    try
//                    {
//                        sqlcmd.BeginTransaction();
//                        TRowData row1 = null, row2 = null;
//                        foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql_delete1.SqlExport(this)))
//                            if (r.HasValue("n"))
//                                row1 = r.ToObject<TRowData>();
//                            else
//                                row2 = r.ToObject<TRowData>();
//                        if ((row1 == null) || (row2 == null)) throw new RowException(RowErrorCode.NoResult);
//                        if (row1.AcceptTime.HasValue && !row2.AcceptTime.HasValue)
//                        {   // 退還預扣
//                            this.UserType = row2.UserType;
//                            UpdateBalance(sql_delete2, true, false, false);
//                            log_LogType = BU.LogType.WithdrawalRollback;
//                            TranLogRow log1 = this.WriteTranLog(row2, false);
//                        }
//                        else
//                        {
//                            if (row1.Fees1x != row2.Fees1x)
//                            {
//                                log_Values = 0;
//                                log_CashFees = -row2.Fees1;
//                                TranLogRow log1 = this.WriteTranLog(row2, true);
//                            }
//                            if (row1.Fees2x != row2.Fees2x)
//                            {
//                                log_Values = 0;
//                                log_CashFees = row2.Fees2;
//                                TranLogRow log2 = this.WriteTranLog(row2, false);
//                            }
//                        }
//                        DeleteTranRow();
//                        sqlcmd.Commit();
//                        _tranrow = row2;
//                        this.IsDeleted = true;
//                    }
//                    catch
//                    {
//                        sqlcmd.Rollback();
//                        throw;
//                    }
//                }
            }



            [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
            public class CashRowData : _RowData
            {
                #region Fields

                [DbImport, JsonProperty]
                public string a_BankName;
                [DbImport, JsonProperty]
                public string a_CardID;
                [DbImport, JsonProperty]
                public string a_Name;
                [DbImport, JsonProperty]
                public DateTime? a_TranTime;
                [DbImport, JsonProperty]
                public string a_TranSerial;
                [DbImport, JsonProperty]
                public string a_TranMemo;
                [DbImport, JsonProperty]
                public string b_BankName;
                [DbImport, JsonProperty]
                public string b_CardID;
                [DbImport, JsonProperty]
                public string b_Name;
                [DbImport, JsonProperty]
                public DateTime? b_TranTime;
                [DbImport, JsonProperty]
                public string b_TranSerial;
                [DbImport, JsonProperty]
                public string b_TranMemo;

                #endregion
            }

            #region interface IBankCard
            interface IBankCard
            {
                string a_BankName { get; set; }
                string a_CardID { get; set; }
                string a_Name { get; set; }
                DateTime? a_TranTime { get; set; }
                string a_TranSerial { get; set; }
                string a_TranMemo { get; set; }
                string b_BankName { get; set; }
                string b_CardID { get; set; }
                string b_Name { get; set; }
                DateTime? b_TranTime { get; set; }
                string b_TranSerial { get; set; }
                string b_TranMemo { get; set; }
            }
            #endregion

            static void insert_fill_bankcard(IBankCard src, sqltool s)
            {
                s[" N", "a_BankName", "  "] = src.a_BankName * text.ValidAsString;
                s["  ", "a_CardID", "    "] = src.a_CardID * text.ValidAsString;
                s[" N", "a_Name", "      "] = src.a_Name * text.ValidAsString;
                s["  ", "a_TranTime", "  "] = src.a_TranTime;
                s["  ", "a_TranSerial", ""] = src.a_TranSerial * text.ValidAsString;
                s[" N", "a_TranMemo", "  "] = src.a_TranMemo * text.ValidAsString;
                s[" N", "b_BankName", "  "] = src.b_BankName * text.ValidAsString;
                s["  ", "b_CardID", "    "] = src.b_CardID * text.ValidAsString;
                s[" N", "b_Name", "      "] = src.b_Name * text.ValidAsString;
                s["  ", "b_TranTime", "  "] = src.b_TranTime;
                s["  ", "b_TranSerial", ""] = src.b_TranSerial * text.ValidAsString;
                s[" N", "b_TranMemo", "  "] = src.b_TranMemo * text.ValidAsString;
            }
            static void update_fill_bankcard(IBankCard src, sqltool s, CashRowData tranrow)
            {
                s[" N", "a_BankName", "  ", tranrow.a_BankName, "  "] = src.a_BankName * text.ValidAsString;
                s["  ", "a_CardID", "    ", tranrow.a_CardID, "    "] = src.a_CardID * text.ValidAsString;
                s[" N", "a_Name", "      ", tranrow.a_Name, "      "] = src.a_Name * text.ValidAsString;
                s["  ", "a_TranTime", "  ", tranrow.a_TranTime, "  "] = src.a_TranTime;
                s["  ", "a_TranSerial", "", tranrow.a_TranSerial, ""] = src.a_TranSerial * text.ValidAsString;
                s[" N", "a_TranMemo", "  ", tranrow.a_TranMemo, "  "] = src.a_TranMemo * text.ValidAsString;
                s[" N", "b_BankName", "  ", tranrow.b_BankName, "  "] = src.b_BankName * text.ValidAsString;
                s["  ", "b_CardID", "    ", tranrow.b_CardID, "    "] = src.b_CardID * text.ValidAsString;
                s[" N", "b_Name", "      ", tranrow.b_Name, "      "] = src.b_Name * text.ValidAsString;
                s["  ", "b_TranTime", "  ", tranrow.b_TranTime, "  "] = src.b_TranTime;
                s["  ", "b_TranSerial", "", tranrow.b_TranSerial, ""] = src.b_TranSerial * text.ValidAsString;
                s[" N", "b_TranMemo", "  ", tranrow.b_TranMemo, "  "] = src.b_TranMemo * text.ValidAsString;
            }

            public abstract class CashRowCommand<TRowCommand> : tran.Cash._RowCommand<CashRowData, TRowCommand> where TRowCommand : CashRowCommand<TRowCommand>, new()
            {
                #region Fields

                [DbImport, JsonProperty]
                public string a_BankName;
                [DbImport, JsonProperty]
                public string a_CardID;
                [DbImport, JsonProperty]
                public string a_Name;
                [DbImport, JsonProperty]
                public DateTime? a_TranTime;
                [DbImport, JsonProperty]
                public string a_TranSerial;
                [DbImport, JsonProperty]
                public string a_TranMemo;
                [DbImport, JsonProperty]
                public string b_BankName;
                [DbImport, JsonProperty]
                public string b_CardID;
                [DbImport, JsonProperty]
                public string b_Name;
                [DbImport, JsonProperty]
                public DateTime? b_TranTime;
                [DbImport, JsonProperty]
                public string b_TranSerial;
                [DbImport, JsonProperty]
                public string b_TranMemo;

                #endregion
            }

            [tran("tranCash1", "tranCash2", BU.LogType.Deposit, BU.LogType.Alipay, BU.LogType.TenPay)]
            public class DepositRowCommand : _DepositRowCommand<CashRowData, DepositRowCommand>, IBankCard
            {
                #region IBankCard

                [JsonProperty]
                public string a_BankName { get; set; }
                [JsonProperty]
                public string a_CardID { get; set; }
                [JsonProperty]
                public string a_Name { get; set; }
                [JsonProperty]
                public DateTime? a_TranTime { get; set; }
                [JsonProperty]
                public string a_TranSerial { get; set; }
                [JsonProperty]
                public string a_TranMemo { get; set; }
                [JsonProperty]
                public string b_BankName { get; set; }
                [JsonProperty]
                public string b_CardID { get; set; }
                [JsonProperty]
                public string b_Name { get; set; }
                [JsonProperty]
                public DateTime? b_TranTime { get; set; }
                [JsonProperty]
                public string b_TranSerial { get; set; }
                [JsonProperty]
                public string b_TranMemo { get; set; }

                #endregion

                protected override void insert_fill(sqltool s)
                {
                    insert_fill_bankcard(this, s);
                }
                protected override void update_fill(sqltool s)
                {
                    update_fill_bankcard(this, s, _tranrow);
                }
            }

            [tran("tranCash1", "tranCash2", BU.LogType.Withdrawal)]
            public class WithdrawalRowCommand : _WithdrawalRowCommand<CashRowData, WithdrawalRowCommand>, IBankCard
            {
                #region IBankCard

                [JsonProperty]
                public string a_BankName { get; set; }
                [JsonProperty]
                public string a_CardID { get; set; }
                [JsonProperty]
                public string a_Name { get; set; }
                [JsonProperty]
                public DateTime? a_TranTime { get; set; }
                [JsonProperty]
                public string a_TranSerial { get; set; }
                [JsonProperty]
                public string a_TranMemo { get; set; }
                [JsonProperty]
                public string b_BankName { get; set; }
                [JsonProperty]
                public string b_CardID { get; set; }
                [JsonProperty]
                public string b_Name { get; set; }
                [JsonProperty]
                public DateTime? b_TranTime { get; set; }
                [JsonProperty]
                public string b_TranSerial { get; set; }
                [JsonProperty]
                public string b_TranMemo { get; set; }

                #endregion

                protected override void insert_fill(sqltool s)
                {
                    insert_fill_bankcard(this, s);
                }
                protected override void update_fill(sqltool s)
                {
                    update_fill_bankcard(this, s, _tranrow);
                }
            }



            [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
            public class ThirdPaymentRowData : _RowData
            {
                [DbImport, JsonProperty]
                public string MerhantID;
            }

            [tran("tranCashThird1", "tranCashThird2", BU.LogType.Dinpay, BU.LogType.YeePay, BU.LogType.Ecpss)]
            public class ThirdPaymentRowCommand : _DepositRowCommand<ThirdPaymentRowData, ThirdPaymentRowCommand>
            {
                [JsonProperty]
                public string MerhantID;

                protected override void insert_fill(sqltool s)
                {
                    s[" ", "MerhantID", ""] = this.MerhantID * text.ValidAsString;
                }
                protected override void update_fill(sqltool s)
                {
                    s["*", "MerhantID", "", _tranrow.MerhantID, ""] = this.MerhantID * text.ValidAsString;
                }
            }
        }
    }
}