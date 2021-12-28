using BU;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using web;

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class AgentSelect : jgrid.GridRequest<AgentSelect>
//{
//    protected override string init_defaultkey() { return "a.CreateTime"; }
//    protected override Dictionary<string, string> init_sortkeys()
//    {
//        return new Dictionary<string, string>()
//        {
//            {"ParentACNT", "b.ACNT"},
//            {"ID", "a.ID"},
//            {"CorpID", "a.CorpID"},
//            {"ACNT", "a.ACNT"},
//            {"GroupID", "a.CorpID {0}, a.GroupID"},
//            {"Name", "a.Name"},
//            {"Locked", "a.Locked"},
//            {"Currency", "a.Currency"},
//            {"Balance", "a.Balance"},
//            {"PayShare", "a.PayShare"},
//            {"MaxUser", "a.MaxUser"},
//            {"MaxAgent", "a.MaxAgent"},
//            {"MaxDepth", "a.MaxDepth"},
//            {"ModifyUser", "a.ModifyUser"},
//            {"ModifyTime", "a.ModifyTime"},
//            {"CreateUser", "a.CreateUser"},
//        };
//    }

//    [JsonProperty]
//    string ParentACNT;
//    [JsonProperty]
//    string ACNT;
//    [JsonProperty]
//    string Name;
//    [JsonProperty]
//    string Locked;

//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write | Permissions.Flag.Read)]
//    static jgrid.GridResponse<AgentRow> execute(AgentSelect command, string json_s, params object[] args)
//    {
//        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
//        {
//            jgrid.GridResponse<AgentRow> data = new jgrid.GridResponse<AgentRow>();
//            StringBuilder sql = new StringBuilder(@"from [Agent] a with(nolock) left join Agent b on a.ParentID=b.ID");
//            int cnt = 0;
//            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
//            sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
//            sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
//            sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (command.Name * text.ValidAsName).Remove("%"));
//            sql_where(sql, ref cnt, "a.Locked={0}", (byte?)(command.Locked * text.ValidAsLocked));

//            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
//            data.page_size = command.page_size;
//            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
//select row_number() over (order by {2}) as rowid, a.*, b.ACNT as ParentACNT
//{3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
//                data.rows.Add(r.ToObject<AgentRow>());
//            return data;
//        }
//        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
//        //{
//        //    jgrid.GridResponse<AgentRow> data = new jgrid.GridResponse<AgentRow>();
//        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select a.*, b.ACNT as ParentACNT from [Agent] a with(nolock) left join Agent b on a.ParentID=b.ID"))
//        //        data.rows.Add(r.ToObject<AgentRow>());
//        //    return data;
//        //}
//    }

//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write)]
//    static object execute(AgentUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write)]
//    static object execute(AgentInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class AgentUpdate : AgentRowCommand, IRowCommand { }

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class AgentInsert : AgentRowCommand, IRowCommand { }
