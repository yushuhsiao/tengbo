using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AgentGroupRow : Groups<AgentRow, AgentGroupRow, AgentGroupRowCommand>.RowData
    {
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AgentGroupRowCommand : Groups<AgentRow, AgentGroupRow, AgentGroupRowCommand>.RowCommand, IRowCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.agentgroup_list, Permissions.Flag.Write)]
        protected override AgentGroupRow execute(AgentGroupRowCommand command, string json_s, params object[] args)
        {
            return base.execute(command, json_s, args);
        }
//        static AgentGroupRow execute(AgentGroupRowCommand command, string json_s, params object[] args)
//        {
//            if (command.op_Insert == true)
//                return command.insert(null, null, json_s, args);
//            else
//                return command.update(json_s, args);
//        }
        
//        AgentGroupRow update(string json_s, params object[] args)
//        {
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                AgentGroupRow row = sqlcmd.GetRowEx<AgentGroupRow>(RowErrorCode.NotFound, "select * from AgentGroup nolock where ID='{0}'", this.ID);
//                sqltool s = new sqltool();
//                s[" ", "CorpID", " ", row.CorpID, " "] = this.CorpID;
//                s[" ", "Sort", "   ", row.Sort, "   "] = this.Sort;
//                s["N", "Name", "   ", row.Name, "   "] = this.Name * text.ValidAsString;
//                s[" ", "Locked", " ", row.Locked, " "] = this.Locked ?? BU.Locked.Active;
//                if (s.fields.Count == 0) return row;
//                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                s.values["ID"] = this.ID;
//                string sqlstr = s.BuildEx2("update AgentGroup set ", sqltool._FieldValue, " where ID={ID} select * from AgentGroup nolock where ID={ID}");
//                return sqlcmd.ExecuteEx<AgentGroupRow>(sqlstr);
//            }
//        }

//        internal AgentGroupRow insert(SqlCmd sqlcmd, CorpRow corp, string json_s, params object[] args)
//        {
//            sqltool s = new sqltool();
//            s["* ", "CorpID", "      "] = this.CorpID;
//            s["* ", "GroupID", "     "] = this.Sort;
//            s["*N", "Name", "        "] = this.Name * text.ValidAsString;
//            s["  ", "Locked", "      "] = this.Locked ?? BU.Locked.Active;
//            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
//            s.TestFieldNeeds();
//            s.Values["CorpID"] = (StringEx.sql_str)"ID";
//            s.values["CorpID_"] = this.CorpID;
//            string sqlstr = s.BuildEx(@"insert into AgentGroup (", sqltool._Fields, @")
//select ", sqltool._Values, @" from Corp nolock where ID={CorpID_}
//select * from AgentGroup nolock where CorpID={CorpID_} and GroupID={GroupID}");
//            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
//            {
//                if (corp != null)
//                    return sqlcmd.ToObject<AgentGroupRow>(sqlstr);
//                else
//                    return sqlcmd.ExecuteEx<AgentGroupRow>(sqlstr);
//            }
//        }
    }
    
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentGroupSelect : jgrid.GridRequest<AgentGroupSelect>
    {
        protected override string init_defaultkey() { return "CorpID"; }
        protected override Dictionary<string, string> init_sortkeys()
        {
            return new Dictionary<string, string>()
        {
            {"Name", "Name"},
            {"Locked", "Locked"},
            {"CreateTime", "CreateTime"},
            {"CreateUser", "CreateUser"},
            {"ModifyTime", "ModifyTime"},
            {"ModifyUser", "ModifyUser"},
            {"GroupID", "CorpID {0}, GroupID"},
            {"ID", "CorpID"},
            {"CorpID", "CorpID"},
        };
        }

        [ObjectInvoke, Permissions(Permissions.Code.agentgroup_list, Permissions.Flag.Write | Permissions.Flag.Read)]
        public static jgrid.GridResponse<AgentGroupRow> execute(AgentGroupSelect command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<AgentGroupRow> data = new jgrid.GridResponse<AgentGroupRow>();
                StringBuilder sql = new StringBuilder(@"select * from AgentGroup nolock");
                int cnt = 0;
                sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
                sql.AppendFormat(" order by {0}", command.GetOrderBy());

                //data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
                //data.page_size = command.page_size;
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.ToString()))
                    data.rows.Add(r.ToObject<AgentGroupRow>());
                return data;
            }
            //jgrid.GridResponse<AgentGroupRow> data = new jgrid.GridResponse<AgentGroupRow>();
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            //{
            //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from AgentGroup nolock order by CorpID asc, GroupID asc"))
            //        data.rows.Add(r.ToObject<AgentGroupRow>());
            //    return data;
            //}
        }

        //static object execute(AgentGroupUpdate command, string json_s, params object[] args)
        //{
        //    return command.update(json_s, args);
        //}

        //[ObjectInvoke, Permissions(Permissions.Code.agentgroup_list, Permissions.Flag.Write)]
        //static object execute(AgentGroupInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
    }
}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class AgentGroupUpdate : AgentGroupRowCommand, IRowCommand { }

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class AgentGroupInsert : AgentGroupRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AgentGroupPermissionSelect : PermissionGroupSelect
{
    protected override UserType UserType { get { return BU.UserType.Agent; } }

    [ObjectInvoke, Permissions(Permissions.Code.agentgroup_permission, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<MenuRow> execute(AgentGroupPermissionSelect command, string json_s, params object[] args) { return command.execute(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.agentgroup_permission, Permissions.Flag.Write)]
    static object execute(AgentGroupPermissionUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AgentGroupPermissionUpdate : PermissionGroupUpdate, IRowCommand { protected override UserType UserType { get { return BU.UserType.Agent; } } }
