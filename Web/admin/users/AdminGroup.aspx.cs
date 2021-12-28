using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading;
using web;
using System.Linq;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdminGroupRow : Groups<AdminRow, AdminGroupRow, AdminGroupRowCommand>.RowData
    {
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdminGroupRowCommand : Groups<AdminRow, AdminGroupRow, AdminGroupRowCommand>.RowCommand, IRowCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.admingroup_list, Permissions.Flag.Write)]
        protected override AdminGroupRow execute(AdminGroupRowCommand command, string json_s, params object[] args)
        {
            return base.execute(command, json_s, args);
        }
//        static AdminGroupRow execute(AdminGroupRowCommand command, string json_s, params object[] args)
//        {
//            if (command.op_Insert == true)
//                return command.insert(null, null, json_s, args);
//            else
//                return command.update(json_s, args);
//        }

//        AdminGroupRow update(string json_s, params object[] args)
//        {
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                AdminGroupRow row = sqlcmd.GetRowEx<AdminGroupRow>(RowErrorCode.NotFound, "select * from AdminGroup nolock where ID='{0}'", this.ID);
//                sqltool s = new sqltool();
//                s[" ", "CorpID", " ", row.CorpID, " "] = this.CorpID;
//                s[" ", "Sort", "   ", row.Sort, "   "] = this.Sort;
//                s["N", "Name", "   ", row.Name, "   "] = this.Name * text.ValidAsString;
//                s[" ", "Locked", " ", row.Locked, " "] = this.Locked ?? BU.Locked.Active;
//                if (s.fields.Count == 0) return row;
//                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                s.values["ID"] = this.ID;
//                string sqlstr = s.BuildEx2("update AdminGroup set ", sqltool._FieldValue, " where ID={ID} select * from AdminGroup nolock where ID={ID}");
//                return sqlcmd.ExecuteEx<AdminGroupRow>(sqlstr);
//            }
//        }

//        internal AdminGroupRow insert(SqlCmd sqlcmd, CorpRow corp, string json_s, params object[] args)
//        {
//            sqltool s = new sqltool();
//            s["* ", "CorpID", "      "] = this.CorpID;
//            s["* ", "Sort", "        "] = this.Sort;
//            s["*N", "Name", "        "] = this.Name * text.ValidAsString;
//            s["  ", "Locked", "      "] = this.Locked ?? BU.Locked.Active;
//            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
//            s.TestFieldNeeds();
//            string sqlstr = s.BuildEx(@"declare @id uniqueidentifier set @id=newid()
//insert into AdminGroup (ID,", sqltool._Fields, @")
//select @id,", sqltool._Values, @" from Corp nolock where ID={CorpID}
//select * from AdminGroup nolock where ID=@id");
//            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
//            {
//                if (corp != null)
//                    return sqlcmd.ToObject<AdminGroupRow>(sqlstr);
//                else
//                    return sqlcmd.ExecuteEx<AdminGroupRow>(sqlstr);
//            }
//        }
    }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class AdminGroupUpdate : AdminGroupRowCommand, IRowCommand { }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class AdminGroupInsert : AdminGroupRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdminGroupSelect : jgrid.GridRequest<AdminGroupSelect>
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
            {"Sort", "Sort"},
            {"ID", "CorpID"},
            {"CorpID", "CorpID"},
        };
        }

        //internal const string sql_query = @"select a.*,b.ACNT as ModifyUserACNT,c.ACNT as CreateUserACNT from AdminGroup a with(nolock) left join [Admin] b with(nolock) on a.ModifyUser=b.ID left join [Admin] c with(nolock) on a.CreateUser=c.ID";
        [ObjectInvoke, Permissions(Permissions.Code.admingroup_list, Permissions.Flag.Write | Permissions.Flag.Read)]
        public static jgrid.GridResponse<AdminGroupRow> execute(AdminGroupSelect command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<AdminGroupRow> data = new jgrid.GridResponse<AdminGroupRow>();
                StringBuilder sql = new StringBuilder(@"select * from AdminGroup nolock");
                int cnt = 0;
                sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
                sql.AppendFormat(" order by {0}", command.GetOrderBy());

                //data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
                //data.page_size = command.page_size;
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.ToString()))
                    data.rows.Add(r.ToObject<AdminGroupRow>());
                return data;
            }
            //jgrid.GridResponse<AdminGroupRow> data = new jgrid.GridResponse<AdminGroupRow>();
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            //{
            //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from AdminGroup nolock order by CorpID asc, GroupID asc"))
            //        data.rows.Add(r.ToObject<AdminGroupRow>());
            //    return data;
            //}
        }

        //[ObjectInvoke, Permissions(Permissions.Code.admingroup_list, Permissions.Flag.Write)]
        //static object update(AdminGroupUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

        //[ObjectInvoke, Permissions(Permissions.Code.admingroup_list, Permissions.Flag.Write)]
        //static object insert(AdminGroupInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AdminGroupPermissionSelect : PermissionGroupSelect
{
    protected override UserType UserType { get { return BU.UserType.Admin; } }

    [ObjectInvoke, Permissions(Permissions.Code.admingroup_permission, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<MenuRow> select(AdminGroupPermissionSelect command, string json_s, params object[] args) { return command.execute(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.admingroup_permission, Permissions.Flag.Write)]
    static object update(AdminGroupPermissionUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AdminGroupPermissionUpdate : PermissionGroupUpdate, IRowCommand { protected override UserType UserType { get { return BU.UserType.Admin; } } }
