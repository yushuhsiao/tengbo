using BU;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;
using System.Linq;
using System.Text;

abstract class LoginLogSelect<T> : jgrid.GridRequest<T> where T : LoginLogSelect<T>, new()
{
    protected override string init_defaultkey() { return "LoginTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"CorpID", "CorpID"},
            {"ACNT", "ACNT"},
            {"Name", "Name"},
            {"Result", "Result"},
            {"Message", "Message"},
            {"LoginIP", "LoginIP"},
        };
    }

    protected abstract UserType UserType { get; }
    [JsonProperty]
    string ACNT;
    [JsonProperty]
    string LoginIP;
    [JsonProperty]
    int? Result;
    [JsonProperty]
    string Message;


    protected static jgrid.GridResponse<Dictionary<string, object>> _execute(T command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"from LoginLog nolock");
            int cnt = 0;
            sql_where(sql, ref cnt, "UserType={0}", (byte)command.UserType);
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "LoginIP like '%{0}%'", (command.LoginIP * text.ValidAsString).Remove("%"));
            sql_where(sql, ref cnt, "Result = {0}", command.Result);
            sql_where(sql, ref cnt, "Message like '%{0}%'", (command.Message * text.ValidAsString).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }

        //        UserType? userType = command.UserType.ToEnum<UserType>();
        //        jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
        //        if (userType.HasValue)
        //        {
        //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        //            {
        //                if (command.hasPager)
        //                {
        //                    data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from LoginLog nolock where UserType={0}", (byte)userType), command.page_size);
        //                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from 
        //(select row_number() over (order by LoginTime desc) as rowid, * from LoginLog with(nolock) where UserType={0}) a
        //where a.rowid>{1} and a.rowid<={2}
        //order by LoginTime desc", (byte)userType, command.rows_start, command.rows_end))
        //                        data.rows.Add(r.ToObject<Dictionary<string, object>>());
        //                }
        //                else
        //                {
        //                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from LoginLog nolock where UserType={0}", (byte)userType))
        //                        data.rows.Add(r.ToObject<Dictionary<string, object>>());
        //                }
        //            }
        //        }
        //        return data;
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AdminLoginLog : LoginLogSelect<AdminLoginLog>
{
    protected override UserType UserType { get { return UserType.Admin; } }
    [ObjectInvoke, Permissions(Permissions.Code.admin_loginhist, Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(AdminLoginLog command, string json_s, params object[] args) { return _execute(command, json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AgentLoginLog : LoginLogSelect<AgentLoginLog>
{
    protected override UserType UserType { get { return UserType.Agent; } }
    [ObjectInvoke, Permissions(Permissions.Code.agent_loginhist, Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(AgentLoginLog command, string json_s, params object[] args) { return _execute(command, json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberLoginLog : LoginLogSelect<MemberLoginLog>
{
    protected override UserType UserType { get { return UserType.Member; } }
    [ObjectInvoke, Permissions(Permissions.Code.member_loginhist, Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(MemberLoginLog command, string json_s, params object[] args) { return _execute(command, json_s, args); }
}