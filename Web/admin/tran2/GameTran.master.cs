using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

public partial class GameTran_master : tran_master2<GameTran_page>
{
}
public partial class GameTran_page : tran_page2
{
}


[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameTranSelect : jgrid.GridRequest<GameTranSelect>
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


    [ObjectInvoke]
    [Permissions(Permissions.Code.tran_game_d, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_game_d, Permissions.Flag.Read | Permissions.Flag.Write)]
    [Permissions(Permissions.Code.tran_game_w, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_game_w, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<GameTranRow> execute(GameTranSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<GameTranRow> data = new jgrid.GridResponse<GameTranRow>();

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("from {0} with(nolock)", command.IsHist.Value ? "GameTran2" : "GameTran1");

            int cnt = 0;
            sql.Append(" where LogType in (");
            foreach (LogType l in command.IsDeposit.Value == true ? text.MemberGameDepositLogTypes : text.MemberGameWithdrawalLogTypes)
            {
                if (cnt++ > 0) sql.Append(',');
                sql.Append((int)l);
            }
            sql.Append(")");

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

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(command.IsHist.Value ? "FinishTime" : "CreateTime"), sql))
                data.rows.Add(r.ToObject<GameTranRow>());
            return data;
            //        jgrid.GridResponse<GameTranRow> data = new jgrid.GridResponse<GameTranRow>();
            //        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            //        {
            //            int logTypes = (int)(command.IsDeposit == true ? LogType.GameDeposit : LogType.GameWithdrawal);
            //            string sqlstr;
            //            if (command.IsHist == true)
            //            {
            //                data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from GameTran2 nolock where LogType={0}", logTypes), command.page_size);
            //                sqlstr = string.Format(@"select * from (select row_number() over (order by FinishTime desc) as rowid, * from GameTran2 nolock where LogType={0})
            //a where a.rowid>{1} and a.rowid<={2} order by FinishTime desc", logTypes, command.rows_start, command.rows_end);
            //            }
            //            else
            //                sqlstr = string.Format("select * from GameTran1 nolock where LogType={0} order by CreateTime desc", logTypes);
            //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
            //                data.rows.Add(r.ToObject<GameTranRow>());
            //            return data;
            //        }
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.tran_game_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_game_w, Permissions.Flag.Write)]
    static object execute(GameTranUpdate command, string json_s, params object[] args) { return command.Update(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.tran_game_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_game_w, Permissions.Flag.Write)]
    static object execute(GameTranInsert command, string json_s, params object[] args) { return command.Insert(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.tran_game_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_game_w, Permissions.Flag.Write)]
    static object execute(GameTranDelete command, string json_s, params object[] args) { return command.Delete(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameTranUpdate : GameTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameTranInsert : GameTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameTranDelete : GameTranRowCommand, IRowCommand { }