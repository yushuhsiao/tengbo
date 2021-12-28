using BU;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameTranSelect : jgrid.GridRequest
{
    [JsonProperty]
    public bool? IsDeposit;

    [JsonProperty]
    public bool? IsHist;

    [ObjectInvoke, api.Async]
    [Permissions(Permissions.Code.tran_game_d, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_game_d, Permissions.Flag.Read | Permissions.Flag.Write)]
    [Permissions(Permissions.Code.tran_game_w, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_game_w, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<GameTranRow> execute(GameTranSelect command, string json_s, params object[] args)
    {
        jgrid.GridResponse<GameTranRow> data = new jgrid.GridResponse<GameTranRow>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            int logTypes = (int)(command.IsDeposit == true ? LogType.GameDeposit : LogType.GameWithdrawal);
            string sqlstr;
            if (command.IsHist == true)
            {
                data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from GameTran2 nolock where LogType={0}", logTypes), command.page_size);
                sqlstr = string.Format(@"select * from (select row_number() over (order by FinishTime desc) as rowid, * from GameTran2 nolock where LogType={0})
a where a.rowid>{1} and a.rowid<={2} order by FinishTime desc", logTypes, command.rows_start, command.rows_end);
            }
            else
                sqlstr = string.Format("select * from GameTran1 nolock where LogType={0} order by CreateTime desc", logTypes);
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
                data.rows.Add(r.ToObject<GameTranRow>());
            return data;
        }
    }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_game_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_game_w, Permissions.Flag.Write)]
    static object execute(GameTranUpdate command, string json_s, params object[] args) { return command.Update(json_s, args); }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_game_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_game_w, Permissions.Flag.Write)]
    static object execute(GameTranInsert command, string json_s, params object[] args) { return command.Insert(json_s, args); }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_game_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_game_w, Permissions.Flag.Write)]
    static object execute(GameTranDelete command, string json_s, params object[] args) { return command.Delete(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameTranUpdate : GameTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameTranInsert : GameTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameTranDelete : GameTranRowCommand, IRowCommand { }