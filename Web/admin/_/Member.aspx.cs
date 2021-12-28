using BU;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;
using System.Linq;
using System.Text;
using System.Web;
using System;

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberSelect : jgrid.GridRequest
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.members_list, Permissions.Flag.Write | Permissions.Flag.Read)]
//    static jgrid.GridResponse<MemberRow> execute(MemberSelect command, string json_s, params object[] args)
//    {
//        User user = HttpContext.Current.User as User;
//        jgrid.GridResponse<MemberRow> data = new jgrid.GridResponse<MemberRow>();
//        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
//        {
//            StringBuilder s1 = new StringBuilder(@"select row_number() over (order by a.CreateTime desc) as rowid,
//b.ACNT AgentACNT, a.*, c.IP as LoginIP, c.LoginTime
//from Member a with(nolock)
//left join Agent b with(nolock) on a.AgentID=b.ID
//left join LoginState c with(nolock) on a.ID=c.ID");

//            StringBuilder s2 = new StringBuilder("select count(*) from Member nolock");
//            if (user.CorpID != 0)
//            {
//                s1.AppendFormat(" where a.CorpID={0}", user.CorpID);
//                s2.AppendFormat(" where CorpID={0}", user.CorpID);
//            }

//            data.setPager(sqlcmd.ExecuteScalar<int>(s2.ToString()), command.rows);
//            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from ({0}) a where rowid>{1} and rowid<={2} order by rowid desc", s1, command.rows_start, command.rows_end))
//            {
//                data.rows.Add(r.ToObject<MemberRow>());
//            }



////            if (command.hasPager)
////            {
////                data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from Member nolock"), command.rows);
////                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by a.CreateTime desc) as rowid,
////b.ACNT AgentACNT, a.*, c.IP as LoginIP, c.LoginTime
////from Member a with(nolock)
////left join Agent b with(nolock) on a.AgentID=b.ID
////left join LoginState c with(nolock) on a.ID=c.ID
////) a where a.rowid>{0} and a.rowid<={1}
////order by CreateTime desc", command.rows_start, command.rows_end))
////                    data.rows.Add(r.ToObject<MemberRow>());
////            }
////            else
////            {
////                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select b.ACNT AgentACNT, a.*, c.IP as LoginIP
////from Member a with(nolock)
////left join Agent b with(nolock)
////left join LoginState c with(nolock) on a.ID=c.ID
////on a.AgentID=b.ID order by CreateTime desc"))
////                    data.rows.Add(r.ToObject<MemberRow>());
////            }

//            //foreach (jgrid.GridUserList users in data.GridUserList(sqlcmd))
//            //{
//            //    foreach (MemberRow row in data.rows)
//            //        users.AddUser(row.CreateUser, row.ModifyUser);
//            //}


//            //var n1 = from users in data.rows
//            //        group users by users.ModifyUser into user_id
//            //        select user_id.Key;
//            //var n2 = from users in data.rows
//            //         group users by users.CreateUser into user_id
//            //         select user_id.Key;

//            return data;
//        }
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberSelect : jgrid.GridRequest<MemberSelect>
//{
//    protected override string init_defaultkey() { return "a.CreateTime"; }
//    protected override Dictionary<string, string> init_sortkeys()
//    {
//        return new Dictionary<string, string>()
//        {
//            {"AgentACNT", "b.ACNT"},
//            {"LoginTime", "c.LoginTime"},
//            {"LoginIP", "c.IP"},
//            {"ID", "a.ID"},
//            {"CorpID", "a.CorpID"},
//            {"ACNT", "a.ACNT"},
//            {"Name", "a.Name"},
//            {"GroupID", "a.CorpID {0}, a.GroupID"},
//            {"Locked", "a.Locked"},
//            {"Balance", "a.Balance"},
//            {"Currency", "a.Currency"},
//            {"Memo", "a.Memo"},
//            {"RegisterIP", "a.RegisterIP"},
//            {"ModifyUser", "a.ModifyUser"},
//            {"ModifyTime", "a.ModifyTime"},
//            {"CreateUser", "a.CreateUser"},
//        };
//    }

//    [JsonProperty]
//    string AgentACNT;
//    [JsonProperty]
//    string ACNT;
//    [JsonProperty]
//    string Name;
//    [JsonProperty]
//    string Locked;


//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.members_list, Permissions.Flag.Read)]
//    static jgrid.GridResponse<MemberRow> OnExecute(MemberSelect command, string json_s, params object[] args)
//    {
//        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
//        {
//            jgrid.GridResponse<MemberRow> data = new jgrid.GridResponse<MemberRow>();

//            StringBuilder sql = new StringBuilder(@"from Member a with(nolock) left join Agent b with(nolock) on a.AgentID=b.ID left join LoginState c with(nolock) on a.ID=c.ID");

//            int cnt = 0;
//            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
//            sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (command.AgentACNT * text.ValidAsACNT).Remove("%"));
//            sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
//            sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (command.Name * text.ValidAsName).Remove("%"));
//            sql_where(sql, ref cnt, "a.Locked={0}", (byte?)(command.Locked * text.ValidAsLocked));

//            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
//            data.page_size = command.page_size;
//            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
//select row_number() over (order by {2}) as rowid, b.ACNT AgentACNT, a.*, c.IP as LoginIP, c.LoginTime
//{3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
//                data.rows.Add(r.ToObject<MemberRow>());
//            return data;
//        }
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberUpdate : MemberRowCommand, IRowCommand
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
//    static object execute(MemberUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberInsert : MemberRowCommand, IRowCommand
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
//    static object execute(MemberInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
//}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberGameSelect : IRowCommand
{
    [JsonProperty]
    GameID? GameID;
    [JsonProperty]
    int? MemberID;

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Read)]
    static MemberGameRow execute(MemberGameSelect command, string json_s, params object[] args)
    {
        return MemberGame.GetInstance(command.GameID).SelectRow(null, command.MemberID, true);
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberGameUpdate : IRowCommand
{
    [JsonProperty]
    GameID? GameID;

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
    static object execute(MemberGameUpdate command, string json_s, params object[] args)
    {
        return MemberGame.GetInstance(command.GameID).DeserializeObject(json_s).Update(json_s, args);
    }
}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberUpdate_HG : MemberGame_HG.RowCommand, IRowCommand
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
//    static object execute(MemberUpdate_HG command, string json_s, params object[] args) { return command.Update(json_s, args); }
//}
//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberUpdate_EA : MemberGame_EA.RowCommand, IRowCommand
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
//    static object execute(MemberUpdate_EA command, string json_s, params object[] args) { return command.Update(json_s, args); }
//}
//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberUpdate_WFT : MemberGame_WFT.RowCommand, IRowCommand
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
//    static object execute(MemberUpdate_WFT command, string json_s, params object[] args) { return command.Update(json_s, args); }
//}
//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberUpdate_KG : MemberGame_KENO.RowCommand, IRowCommand
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
//    static object execute(MemberUpdate_KG command, string json_s, params object[] args) { return command.Update(json_s, args); }
//}
//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberUpdate_SUNBET : MemberGame_SUNBET.RowCommand, IRowCommand
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
//    static object execute(MemberUpdate_SUNBET command, string json_s, params object[] args) { return command.Update(json_s, args); }
//}
//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberUpdate_AG : MemberGame_AG.RowCommand, IRowCommand
//{
//    [ObjectInvoke, api.Async, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
//    static object execute(MemberUpdate_AG command, string json_s, params object[] args) { return command.Update(json_s, args); }
//}
