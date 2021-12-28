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
        public static partial class Promo
        {
            public abstract class _RowData : RowData
            {
            }

            public abstract class _RowCommand<TRowData, TRowCommand> : RowCommand<TRowData, TRowCommand>
                where TRowData : _RowData, new()
                where TRowCommand : _RowCommand<TRowData, TRowCommand>, new()
            {
                protected override void execute(string json_s, params object[] args)
                {
                    if (op_Insert == true) this.insert();
                    if (op_Update == true) this.update();
                    if (op_Finish == true) this.finish();
                    if (op_Delete == true) this.delete();
                }

                protected AgentRow _provider;

                protected virtual void insert_fill(sqltool s) { }
                protected void insert()
                {
                    if (this.prefix == null) throw new RowException(RowErrorCode.InvaildLogType);
                    _user = this.GetUserEx(this.UserType, this.UserID, this.CorpID, this.UserACNT, "*");
                    _provider = _user.GetProvider(sqlcmd);
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
                    this.insert_fill(s);
                    s.TestFieldNeeds();
                    s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
                    s.values["table_tran"] = this.TranTable1;
                    s.values["prefix"] = this.prefix;
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
                    if (s.fields.Count == 0) return;
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.values["table_tran"] = this.TranTable1;
                    s.values["ID"] = _tranrow.ID;
                    TRowData tmp = sqlcmd.ToObject<TRowData>(true, s.BuildEx2("update {table_tran} set ", sqltool._FieldValue, @" where ID={ID} and FinishTime is null
select * from {table_tran} nolock where ID={ID}"));
                    if (tmp == null) throw new RowException(RowErrorCode.NoResult);
                    _tranrow = tmp;
                }

                protected void finish()
                {
                    _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        if (setTransferred_in())
                        {
                            //sqlcmd.ExecuteNonQuery("".SqlExport(this));
                            //sqlcmd.ExecuteNonQuery("".SqlExport(this));
                            UpdateUserBalance(true);
                            UpdateProviderBalance(false);
                            TRowData tmp = GetTranRow(RowErrorCode.NoResult, false);
                            finish_writelog(tmp);
                            sqlcmd.Commit();
                            _tranrow = tmp;
                            this.op_Delete = true;
                        }
                        else sqlcmd.Rollback();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }
                protected virtual void finish_writelog(TRowData tmp)
                {
                    TranLogRow log1 = this.WriteTranLog(tmp, false);
                    TranLogRow log2 = this.WriteTranLog(tmp, true);
                }

                protected void delete()
                {
                    _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        setRejected();
                        TRowData tmp = GetTranRow(RowErrorCode.NoResult, false);
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
            }

            [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
            public class BetAmtRowData : _RowData
            {
                [DbImport, JsonProperty]
                public DateTime ACTime1;        // 起始時間
                [DbImport, JsonProperty]
                public DateTime ACTime2;        // 結束時間
                [DbImport, JsonProperty]
                public decimal BetAmount1;      // 總投注額
                [DbImport, JsonProperty]
                public decimal BetAmount2;      // 有效投注額
                [DbImport, JsonProperty]
                public decimal BetPayout;       // 輸贏
                [DbImport, JsonProperty]
                public decimal Rate;            // 洗碼比例/佣金比例
            }

            [tran("tranBetAmt1", "tranBetAmt2", BU.LogType.BetAmt)]
            public class BetAmtRowCommand : _RowCommand<BetAmtRowData, BetAmtRowCommand>
            {
                [JsonProperty]
                public DateTime? ACTime1;
                [JsonProperty]
                public DateTime? ACTime2;
                [JsonProperty]
                public decimal? BetAmount1;
                [JsonProperty]
                public decimal? BetAmount2;
                [JsonProperty]
                public decimal? BetPayout;
                [JsonProperty]
                public decimal? Rate;

                decimal? GetRate()
                {
                    if (_user is MemberRow)
                    {
                        for (AgentRow agent = AgentRow.GetAgentEx(sqlcmd, _user.ParentID, null, null, "*"); agent != null; agent = AgentRow.GetAgentEx(sqlcmd, agent.ParentID, null, null, "*"))
                        {
                            if (agent.ParentID == 0)
                            {
                                MemberGroupRow grp = sqlcmd.ToObject<MemberGroupRow>("select BonusW,BonusL from {GroupTableName} nolock where CorpID={CorpID} and GroupID={GroupID}".SqlExport(_user));
                                return this.BetPayout >= 0 ? grp.BonusW : grp.BonusL;
                            }
                            else
                            {
                                decimal r = (this.BetPayout >= 0 ? agent.M_BonusW : agent.M_BonusL) ?? 0;
                                if (r != 0) return r;
                            }
                        }
                    }
                    else if (_user is AgentRow)
                    {
                        for (AgentRow agent = AgentRow.GetAgentEx(sqlcmd, _user.ParentID, null, null, "*"); agent != null; agent = AgentRow.GetAgentEx(sqlcmd, agent.ParentID, null, null, "*"))
                        {
                            decimal r = (this.BetPayout >= 0 ? agent.A_BonusW : agent.A_BonusL) ?? 0;
                            if (r != 0) return r;
                        }
                    }
                    return null;
                }

                protected override void insert_fill(sqltool s)
                {
                    this.BetAmount2 = this.BetAmount2.amount_valid();
                    this.BetPayout = this.BetPayout ?? 0;
                    this.Rate = this.Rate.amount_valid() ?? this.GetRate();
                    decimal? amount = null;
                    if (this.BetAmount2.HasValue && this.Rate.HasValue)
                        amount = Math.Round(this.BetAmount2.Value * this.Rate.Value, 2, MidpointRounding.AwayFromZero);
                    s["*", "ACTime1", "   "] = this.ACTime1;
                    s["*", "ACTime2", "   "] = this.ACTime2;
                    s[" ", "BetAmount1", ""] = this.BetAmount1 ?? 0;
                    s["*", "BetAmount2", ""] = this.BetAmount2;
                    s[" ", "BetPayout", " "] = this.BetPayout;
                    s["*", "Rate", "      "] = this.Rate;
                    s["*", "Amount", "    "] = amount;// calc_amount(this.BetAmount2, 0, this.Rate, 0);
                }
                protected override void update_fill(sqltool s)
                {
                    this.Rate = this.Rate.amount_valid() ?? _tranrow.Rate;
                    s[" ", "ACTime1", "   ", _tranrow.ACTime1, "   "] = this.ACTime1;
                    s[" ", "ACTime2", "   ", _tranrow.ACTime2, "   "] = this.ACTime2;
                    s[" ", "BetAmount1", "", _tranrow.BetAmount1, ""] = this.BetAmount1;
                    s[" ", "BetAmount2", "", _tranrow.BetAmount2, ""] = this.BetAmount2;
                    s[" ", "BetPayout", " ", _tranrow.BetPayout, " "] = this.BetPayout;
                    s[" ", "Rate", "      ", _tranrow.Rate, "      "] = this.Rate;
                    s[" ", "Amount", "    ", _tranrow.Amount, "    "] = Math.Round((this.BetAmount2 ?? _tranrow.BetAmount2) * (this.Rate ?? _tranrow.Rate), 2, MidpointRounding.AwayFromZero); // calc_amount(this.BetAmount2, _tranrow.BetAmount2, this.Rate, _tranrow.Rate);
                }
                protected override void finish_writelog(BetAmtRowData tmp)
                {
                    TranLogRow log1 = this.WriteTranLog(tmp, false, log_BetAmount = tmp.BetAmount2, log_BetBonus = tmp.Rate);
                    TranLogRow log2 = this.WriteTranLog(tmp, true, log_BetAmount = tmp.BetAmount2, log_BetBonus = tmp.Rate);
                }
            }

            [tran("tranBetAmt1", "tranBetAmt2", BU.LogType.AgentShare, insert_acceptMember = false)]
            public class AgentShareRowCommand : _RowCommand<BetAmtRowData, AgentShareRowCommand>
            {
                [JsonProperty]
                public DateTime? ACTime1;
                [JsonProperty]
                public DateTime? ACTime2;
                [JsonProperty]
                public decimal? BetAmount1;
                [JsonProperty]
                public decimal? BetAmount2;
                [JsonProperty]
                public decimal? BetPayout;
                [JsonProperty]
                public decimal? Rate;

                decimal? GetRate()
                {
                    if (_user is AgentRow)
                    {
                        for (AgentRow agent = AgentRow.GetAgentEx(sqlcmd, _user.ParentID, null, null, "*"); agent != null; agent = AgentRow.GetAgent(sqlcmd, agent.ParentID, null, null, "*"))
                        {
                            decimal r = (this.BetPayout >= 0 ? agent.A_ShareW : agent.A_ShareL) ?? 0;
                            if (r != 0) return r;
                        }
                    }
                    return null;
                }

                protected override void insert_fill(sqltool s)
                {
                    this.Rate = this.Rate.amount_valid() ?? this.GetRate();
                    decimal? amount = null;
                    if (this.BetPayout.HasValue && this.Rate.HasValue)
                        amount = Math.Round(this.BetPayout.Value * this.Rate.Value, 2, MidpointRounding.AwayFromZero);
                    s["*", "ACTime1", "   "] = this.ACTime1;
                    s["*", "ACTime2", "   "] = this.ACTime2;
                    s[" ", "BetAmount1", ""] = this.BetAmount1 ?? 0;
                    s[" ", "BetAmount2", ""] = this.BetAmount2 ?? 0;
                    s["*", "BetPayout", " "] = this.BetPayout;
                    s["*", "Rate", "      "] = this.Rate;
                    s[" ", "Amount", "    "] = amount;
                }
                protected override void update_fill(sqltool s)
                {
                    this.Rate = this.Rate.amount_valid() ?? _tranrow.Rate;
                    s[" ", "ACTime1", "   ", _tranrow.ACTime1, "   "] = this.ACTime1;
                    s[" ", "ACTime2", "   ", _tranrow.ACTime2, "   "] = this.ACTime2;
                    s[" ", "BetAmount1", "", _tranrow.BetAmount1, ""] = this.BetAmount1;
                    s[" ", "BetAmount2", "", _tranrow.BetAmount2, ""] = this.BetAmount2;
                    s[" ", "BetPayout", " ", _tranrow.BetPayout, " "] = this.BetPayout;
                    s[" ", "Rate", "      ", _tranrow.Rate, "      "] = this.Rate;
                    s[" ", "Amount", "    ", _tranrow.Amount, "    "] = Math.Round((this.BetPayout ?? _tranrow.BetPayout) * (this.Rate ?? _tranrow.Rate), 2, MidpointRounding.AwayFromZero); //calc_amount(this.BetPayout, _tranrow.BetPayout, this.Rate, _tranrow.Rate);
                }
                protected override void finish_writelog(BetAmtRowData tmp)
                {
                    TranLogRow log1 = this.WriteTranLog(tmp, false, log_BetPayout = tmp.BetPayout, log_BetShare = tmp.Rate);
                    TranLogRow log2 = this.WriteTranLog(tmp, true, log_BetPayout = tmp.BetPayout, log_BetShare = tmp.Rate);
                }
            }

            [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
            public class FirstDepositRowData : _RowData
            {
                [DbImport, JsonProperty]
                public decimal DepositAmount;   // 存款金額
                [DbImport, JsonProperty]
                public decimal Rate;            // 倍數要求
                [DbImport, JsonProperty]
                public decimal BetAmt;          // 流水要求
            }

            [tran("tranFirstDeposit1", "tranFirstDeposit2", BU.LogType.FirstDeposit, BU.LogType.SecondDeposit, insert_acceptAgent = false)]
            public class FirstDepositRowCommand : _RowCommand<FirstDepositRowData, FirstDepositRowCommand>
            {
                [JsonProperty]
                public decimal? DepositAmount;   // 存款金額
                [JsonProperty]
                public decimal? Rate;            // 倍數要求
                [JsonProperty]
                public decimal? BetAmt;          // 流水要求

                protected override void insert_fill(sqltool s)
                {
                    s["*", "Amount", "       "] = this.Amount.amount_valid();
                    s["*", "DepositAmount", ""] = this.DepositAmount ?? 0;
                    s[" ", "BetAmt", "       "] = this.BetAmt ?? 0;
                    s[" ", "Rate", "         "] = this.Rate ?? 0;
                }
                protected override void update_fill(sqltool s)
                {
                    s[" ", "DepositAmount", "", _tranrow.DepositAmount, ""] = this.DepositAmount;
                    s[" ", "BetAmt", "       ", _tranrow.BetAmt, "       "] = this.BetAmt;
                    s[" ", "Rate", "         ", _tranrow.Rate, "         "] = this.Rate;
                }
            }

            [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
            public class PromoRowData : _RowData { }

            [tran("tranPromo1", "tranPromo2", BU.LogType.Promos)]
            public class PromoRowCommand : _RowCommand<PromoRowData, PromoRowCommand>
            {
                protected override void insert_fill(sqltool s)
                {
                    s["*", "Amount", "       "] = this.Amount.amount_valid();
                }
            }
        }
    }
}