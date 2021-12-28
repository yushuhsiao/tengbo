using BU;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

public partial class PrompTran_master : tran_master2<PrompTran_page>
{
}
public partial class PrompTran_page : tran_page2
{
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class PromoTranSelect : jgrid.GridRequest<PromoTranSelect>
{
    protected override string init_defaultkey() { throw new NotImplementedException(); }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"SerialNumber", "SerialNumber"},
            {"LogType", "LogType"},
            {"State", "State"},
            {"GameID", "GameID"},
            {"FinishTime", "FinishTime"},
            {"CreateTime", "CreateTime"},
            {"MemberID", "MemberID"},
            {"CorpID", "CorpID"},
            {"AgentACNT", "AgentACNT"},
            {"MemberACNT", "MemberACNT"},
            {"Amount1", "Amount1"},
            {"Amount2", "Amount2"},
            {"Currency", "Currency"},
            {"Memo1", "Memo1"},
            {"Memo2", "Memo2"},
            {"RequestIP", "RequestIP"},
            {"CreateUser", "CreateUser"},
            {"ModifyTime", "ModifyTime"},
            {"ModifyUser", "ModifyUser"},
        };
    }

    [JsonProperty]
    public bool? IsHist;
    [JsonProperty]
    public string SerialNumber;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public string State;
    [JsonProperty]
    public string AgentACNT;
    [JsonProperty]
    public string MemberACNT;
    [JsonProperty]
    public string GameID;
    [JsonProperty]
    public decimal? Amount1;
    [JsonProperty]
    public decimal? Amount2;
    [JsonProperty]
    public string RequestIP;
    [JsonProperty]
    public int? CreateUser;
    [JsonProperty]
    public int? ModifyUser;
    [JsonProperty]
    public string Memo1;
    [JsonProperty]
    public string Memo2;


    [ObjectInvoke, Permissions(Permissions.Code.tran_promo, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_promo, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<PromoTranRow> execute(PromoTranSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<PromoTranRow> data = new jgrid.GridResponse<PromoTranRow>();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("from {0} with(nolock)", command.IsHist.Value ? "PromoTran2" : "PromoTran1");

            int cnt = 0;
            //sql.Append(" where LogType in (");
            //foreach (LogType l in command.IsDeposit.Value == true ? text.MemberGameDepositLogTypes : text.MemberGameWithdrawalLogTypes)
            //{
            //    if (cnt++ > 0) sql.Append(',');
            //    sql.Append((int)l);
            //}
            //sql.Append(")");

            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "lower(SerialNumber) like lower('%{0}%')", (command.SerialNumber * text.ValidAsString).Remove("%"));
            sql_where(sql, ref cnt, "State = {0}", (int?)command.State.ToEnum<TranState>());
            sql_where(sql, ref cnt, "LogType = {0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "GameID = {0}", (int?)command.GameID.ToEnum<GameID>());
            sql_where(sql, ref cnt, "AgentACNT like '%{0}%'", (command.AgentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "MemberACNT like '%{0}%'", (command.MemberACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "Amount1 = {0}", command.Amount1);
            sql_where(sql, ref cnt, "Amount2 = {0}", command.Amount2);
            sql_where(sql, ref cnt, "RequestIP like '%{0}%'", (command.RequestIP * text.ValidAsString).Remove("%"));
            sql_where(sql, ref cnt, "CreateUser = {0}", command.CreateUser);
            sql_where(sql, ref cnt, "ModifyUser = {0}", command.ModifyUser);
            sql_where(sql, ref cnt, "Memo1 like '%{0}%'", (command.Memo1 * text.ValidAsString).Remove("%"));
            sql_where(sql, ref cnt, "Memo2 like '%{0}%'", (command.Memo2 * text.ValidAsString).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(command.IsHist.Value ? "FinishTime" : "CreateTime"), sql))
                data.rows.Add(r.ToObject<PromoTranRow>());
            return data;
            //jgrid.GridResponse<PromoTranRow> data = new jgrid.GridResponse<PromoTranRow>();
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            //{
            //    string sqlstr;
            //    if (command.IsHist == true)
            //    {
            //        data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from PromoTran2 nolock"), command.page_size);
            //        sqlstr = string.Format(@"select * from (select row_number() over (order by FinishTime desc) as rowid, * from PromoTran2) a where a.rowid>{0} and a.rowid<={1} order by FinishTime desc", command.rows_start, command.rows_end);
            //    }
            //    else
            //        sqlstr = "select * from PromoTran1 nolock order by CreateTime desc";
            //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
            //        data.rows.Add(r.ToObject<PromoTranRow>());
            //    return data;
            //}
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.tran_promo, Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_promo, Permissions.Flag.Write)]
    static object execute(PromoTranUpdate command, string json_s, params object[] args) { return command.Update(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.tran_promo, Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_promo, Permissions.Flag.Write)]
    static object execute(PromoTranInsert command, string json_s, params object[] args)
    {
        string _acnt = command.MemberACNT ?? "";
        DateTime? actime;
        if (command.CorpID.HasValue &&
            (command.LogType == BU.LogType.BetAmt) &&
            _acnt.StartsWith("*") &&
            (actime = _acnt.Substring(1).ToDateTime()).HasValue)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read), sqlcmd2 = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                List<BetAmt_tmp> tmp = new List<BetAmt_tmp>();
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select *,BetAmountAct * BonusRate as BetBonus from (
select a.*, b.CorpID, b.ACNT, case when a.Payout>0 then c.BonusW else c.BonusL end as BonusRate
from (select ACTime, MemberID, sum(BetAmount) BetAmount, sum(BetAmountAct) BetAmountAct, sum(Payout) Payout
from GameLog_BetAmtDG a with(nolock)
where ACTime='{0:yyyy-MM-dd}' and CorpID={1}
group by ACTime, MemberID) a
left join Member b with(nolock) on a.MemberID=b.ID
left join MemberGroup c with(nolock) on b.CorpID=c.CorpID and b.GroupID=c.GroupID) x", actime, command.CorpID))
                {
                    int memberID = r.GetInt32("MemberID");
                    int corpID = r.GetInt32("CorpID");
                    string acnt = r.GetString("ACNT");
                    decimal betamt1 = r.GetDecimal("BetAmount");
                    decimal betamt2 = r.GetDecimal("BetAmountAct");
                    decimal payout = r.GetDecimal("Payout");
                    decimal rate = r.GetDecimal("BonusRate");
                    decimal bonus = r.GetDecimal("BetBonus");
                    command.MemberACNT = acnt;
                    command.Amount1 = Math.Round(bonus, 2, MidpointRounding.AwayFromZero);
                    command.Memo1 = string.Format("{0:0.00} * {1:0.0}%", betamt2, rate * 100);
                    if (command.Amount1 <= 0) continue;
                    try { command.Insert(sqlcmd2, json_s, args); }
                    catch { }
                }
                throw new RowException(RowErrorCode.RefreshRequired);
            }
        }
        return command.Insert(null, json_s, args);
    }
    class BetAmt_tmp
    {
        public int MemberID;
        public decimal BetAmount;
        public decimal BetAmountAct;
        public decimal Payout;
        public decimal BonusRate;
        public decimal BetBonus;
    }

    [ObjectInvoke, Permissions(Permissions.Code.tran_promo, Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_promo, Permissions.Flag.Write)]
    static object execute(PromoTranDelete command, string json_s, params object[] args) { return command.Delete(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class PromoTranUpdate : PromoTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class PromoTranInsert : PromoTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class PromoTranDelete : PromoTranRowCommand, IRowCommand { }