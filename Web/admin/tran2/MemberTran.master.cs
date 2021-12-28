using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

public partial class MemberTran_master : tran_master2<MemberTran_page>
{
}
public partial class MemberTran_page : tran_page2
{
}


[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberTranSelect : jgrid.GridRequest<MemberTranSelect>
{
    protected override string init_defaultkey() { throw new NotImplementedException(); }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"SerialNumber", "a.SerialNumber"},
            {"LogType", "a.LogType"},
            {"State", "a.State"},
            {"GameID", "a.GameID"},
            {"FinishTime", "a.FinishTime"},
            {"CreateTime", "a.CreateTime"},
            {"MemberID", "a.MemberID"},
            {"CorpID", "a.CorpID"},
            {"AgentACNT", "a.AgentACNT"},
            {"MemberACNT", "a.MemberACNT"},
            {"Amount1", "a.Amount1"},
            {"Amount2", "a.Amount2"},
            {"Currency", "a.Currency"},
            {"Memo1", "a.Memo1"},
            {"Memo2", "a.Memo2"},
            {"a_BankName", "a.a_BankName"},
            {"b_BankName", "a.b_BankName"},
            {"a_CardID", "a.a_CardID"},
            {"b_CardID", "a.b_CardID"},
            {"a_Name", "a.a_Name"},
            {"b_Name", "a.b_Name"},
            {"a_TranTime", "a.a_TranTime"},
            {"b_TranTime", "a.b_TranTime"},
            {"a_TranSerial", "a.a_TranSerial"},
            {"b_TranSerial", "a.b_TranSerial"},
            {"a_TranMemo", "a.a_TranMemo"},
            {"b_TranMemo", "a.b_TranMemo"},
            {"RequestIP", "a.RequestIP"},
            {"CreateUser", "a.CreateUser"},
            {"ModifyTime", "a.ModifyTime"},
            {"ModifyUser", "a.ModifyUser"},
            {"MemberName", "b.Name"},
        };
    }

    [JsonProperty]
    public bool? IsDeposit;
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
    public string MemberName;
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


    [ObjectInvoke]
    [Permissions(Permissions.Code.tran_member_d, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_member_d, Permissions.Flag.Read | Permissions.Flag.Write)]
    [Permissions(Permissions.Code.tran_member_w, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_member_w, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<MemberTranRow> execute(MemberTranSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<MemberTranRow> data = new jgrid.GridResponse<MemberTranRow>();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("from {0} a with(nolock) left join Member b with(nolock) on a.MemberID=b.ID", command.IsHist.Value ? "MemberTran2" : "MemberTran1");

            int cnt = 0;
            sql.Append(" where a.LogType in (");
            foreach (LogType l in command.IsDeposit.Value == true ? text.MemberDepositLogTypes : text.MemberWithdrawalLogTypes)
            {
                if (cnt++ > 0)
                    sql.Append(',');
                sql.Append((int)l);
            }
            sql.Append(")");

            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "lower(a.SerialNumber) like lower('%{0}%')", (command.SerialNumber * text.ValidAsString).Remove("%"));
            sql_where(sql, ref cnt, "a.State = {0}", (int?)command.State.ToEnum<TranState>());
            sql_where(sql, ref cnt, "a.LogType = {0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "a.AgentACNT like '%{0}%'", (command.AgentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "a.MemberACNT like '%{0}%'", (command.MemberACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "b.Name like N'%{0}%'", (command.MemberName * text.ValidAsName).Remove("%"));
            sql_where(sql, ref cnt, "a.Amount1 = {0}", command.Amount1);
            sql_where(sql, ref cnt, "a.Amount2 = {0}", command.Amount2);
            sql_where(sql, ref cnt, "a.RequestIP like '%{0}%'", (command.RequestIP * text.ValidAsString).Remove("%"));
            sql_where(sql, ref cnt, "a.CreateUser = {0}", command.CreateUser);
            sql_where(sql, ref cnt, "a.ModifyUser = {0}", command.ModifyUser);
            sql_where(sql, ref cnt, "a.Memo1 like '%{0}%'", (command.Memo1 * text.ValidAsString).Remove("%"));
            sql_where(sql, ref cnt, "a.Memo2 like '%{0}%'", (command.Memo2 * text.ValidAsString).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, a.*, b.Name as MemberName {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(command.IsHist.Value ? "a.FinishTime" : "a.CreateTime"), sql))
                data.rows.Add(r.ToObject<MemberTranRow>());
            return data;

//            StringBuilder logTypes = new StringBuilder();
//            string s1 = "";
//            foreach (LogType l in command.IsDeposit == true ? text.MemberDepositLogTypes : text.MemberWithdrawalLogTypes)
//            {
//                logTypes.Append(s1);
//                logTypes.Append((int)l);
//                s1 = ",";
//            }
//            string sqlstr;
//            if (command.IsHist == true)
//            {
//                data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from MemberTran2 nolock where LogType in ({0})", logTypes), command.page_size);
//                sqlstr = string.Format(@"select * from (select row_number() over (order by FinishTime desc) as rowid, * from MemberTran2 nolock where LogType in ({0}))
//a where a.rowid>{1} and a.rowid<={2} order by FinishTime desc", logTypes, command.rows_start, command.rows_end);
//            }
//            else
//                sqlstr = string.Format("select * from MemberTran1 nolock where LogType in ({0}) order by CreateTime desc", logTypes);
//            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
//                data.rows.Add(r.ToObject<MemberTranRow>());
//            return data;
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.tran_member_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_member_w, Permissions.Flag.Write)]
    static object execute(MemberTranUpdate command, string json_s, params object[] args) { return command.Update(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.tran_member_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_member_w, Permissions.Flag.Write)]
    static object execute(MemberTranInsert command, string json_s, params object[] args) { return command.Insert(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.tran_member_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_member_w, Permissions.Flag.Write)]
    static object execute(MemberTranDelete command, string json_s, params object[] args) { return command.Delete(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberTranUpdate : MemberTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberTranInsert : MemberTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberTranDelete : MemberTranRowCommand, IRowCommand { }