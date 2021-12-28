using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberTranSelect : jgrid.GridRequest
{
    [JsonProperty]
    public bool? IsDeposit;

    [JsonProperty]
    public bool? IsHist;

    [ObjectInvoke, api.Async]
    [Permissions(Permissions.Code.tran_member_d, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_member_d, Permissions.Flag.Read | Permissions.Flag.Write)]
    [Permissions(Permissions.Code.tran_member_w, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_member_w, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<MemberTranRow> execute(MemberTranSelect command, string json_s, params object[] args)
    {
        jgrid.GridResponse<MemberTranRow> data = new jgrid.GridResponse<MemberTranRow>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            StringBuilder logTypes = new StringBuilder();
            string s1 = "";
            foreach (LogType l in command.IsDeposit == true ? text.MemberDepositLogTypes : text.MemberWithdrawalLogTypes)
            {
                logTypes.Append(s1);
                logTypes.Append((int)l);
                s1 = ",";
            }
            string sqlstr;
            if (command.IsHist == true)
            {
                data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from MemberTran2 nolock where LogType in ({0})", logTypes), command.page_size);
                sqlstr = string.Format(@"select * from (select row_number() over (order by FinishTime desc) as rowid, * from MemberTran2 nolock where LogType in ({0}))
a where a.rowid>{1} and a.rowid<={2} order by FinishTime desc", logTypes, command.rows_start, command.rows_end);
            }
            else
                sqlstr = string.Format("select * from MemberTran1 nolock where LogType in ({0}) order by CreateTime desc", logTypes);
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
                data.rows.Add(r.ToObject<MemberTranRow>());
        }
        return data;
    }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_member_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_member_w, Permissions.Flag.Write)]
    static object execute(MemberTranUpdate command, string json_s, params object[] args) { return command.Update(json_s, args); }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_member_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_member_w, Permissions.Flag.Write)]
    static object execute(MemberTranInsert command, string json_s, params object[] args) { return command.Insert(json_s, args); }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_member_d, Permissions.Flag.Write), Permissions(Permissions.Code.tran_member_w, Permissions.Flag.Write)]
    static object execute(MemberTranDelete command, string json_s, params object[] args) { return command.Delete(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberTranUpdate : MemberTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberTranInsert : MemberTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberTranDelete : MemberTranRowCommand, IRowCommand { }