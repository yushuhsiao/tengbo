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
    public class MemberGroupRow : Groups<MemberRow, MemberGroupRow, MemberGroupRowCommand>.RowData
    {
        [DbImport, JsonProperty]
        public decimal? BonusW;
        [DbImport, JsonProperty]
        public decimal? BonusL;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberGroupRowCommand : Groups<MemberRow, MemberGroupRow, MemberGroupRowCommand>.RowCommand, IRowCommand
    {
        [JsonProperty]
        public virtual decimal? BonusW { get; set; }
        [JsonProperty]
        public virtual decimal? BonusL { get; set; }

        [ObjectInvoke, Permissions(Permissions.Code.membergroup_list, Permissions.Flag.Write)]
        protected override MemberGroupRow execute(MemberGroupRowCommand command, string json_s, params object[] args)
        {
            return base.execute(command, json_s, args);
        }

        protected override void update_fill(MemberGroupRow row, sqltool s)
        {
            s[" ", "BonusW", " ", row.BonusW, " "] = this.BonusW;
            s[" ", "BonusL", " ", row.BonusL, " "] = this.BonusL;
        }

        protected override void insert_fill(sqltool s)
        {
            s[" ", "BonusW", " "] = this.BonusW ?? 0;
            s[" ", "BonusL", " "] = this.BonusL ?? 0;
        }

//        MemberGroupRow update(string json_s, params object[] args)
//        {
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                MemberGroupRow row = sqlcmd.GetRowEx<MemberGroupRow>(RowErrorCode.NotFound, "select * from MemberGroup nolock where ID='{0}'", this.ID);
//                sqltool s = new sqltool();
//                s[" ", "CorpID", " ", row.CorpID, " "] = this.CorpID;
//                s[" ", "Sort", "   ", row.Sort, "   "] = this.Sort;
//                s["N", "Name", "   ", row.Name, "   "] = this.Name * text.ValidAsString;
//                s[" ", "Locked", " ", row.Locked, " "] = this.Locked ?? BU.Locked.Active;
//                s[" ", "BonusW", " ", row.BonusW, " "] = this.BonusW;
//                s[" ", "BonusL", " ", row.BonusL, " "] = this.BonusL;
//                if (s.fields.Count == 0) return row;
//                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                s.values["ID"] = this.ID;
//                string sqlstr = s.BuildEx("update MemberGroup set ", sqltool._FieldValue, " where ID={ID} select * from MemberGroup nolock where ID={ID}");
//                return sqlcmd.ExecuteEx<MemberGroupRow>(sqlstr);
//            }
//        }

//        internal MemberGroupRow insert(SqlCmd sqlcmd, CorpRow corp, string json_s, params object[] args)
//        {
//            sqltool s = new sqltool();
//            s["* ", "CorpID", "      "] = this.CorpID;
//            s["* ", "GroupID", "     "] = this.Sort;
//            s["*N", "Name", "        "] = this.Name * text.ValidAsString;
//            s["  ", "Locked", ""] = this.Locked ?? BU.Locked.Active;
//            s["  ", "BonusW", ""] = this.BonusW ?? 0;
//            s["  ", "BonusL", ""] = this.BonusL ?? 0;
//            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
//            s.TestFieldNeeds();
//            s.Values["CorpID"] = (StringEx.sql_str)"ID";
//            s.values["CorpID_"] = this.CorpID;
//            string sqlstr = s.BuildEx(@"insert into MemberGroup (", sqltool._Fields, @")
//select ", sqltool._Values, @" from Corp nolock where ID={CorpID_}
//select * from MemberGroup nolock where CorpID={CorpID_} and GroupID={GroupID}");
//            //            string sqlstr = s.BuildEx(@"declare @ID int exec alloc_UserID @ID output, @type='MemberGroup',@corpid={CorpID},@acnt={ACNT}
//            //insert into MemberGroup (ID,", sqltool._Fields, @")
//            //select @ID,", sqltool._Values, @" from Corp nolock where ID={CorpID}
//            //select * from MemberGroup nolock where ID=@ID");
//            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
//            {
//                if (corp != null)
//                    return sqlcmd.ToObject<MemberGroupRow>(sqlstr);
//                else
//                    return sqlcmd.ExecuteEx<MemberGroupRow>(sqlstr);
//            }
//        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class MemberGroupSelect : jgrid.GridRequest<MemberGroupSelect>
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
            {"BonusW", "BonusW"},
            {"BonusL", "BonusL"},
        };
        }

        [ObjectInvoke, Permissions(Permissions.Code.membergroup_list, Permissions.Flag.Write | Permissions.Flag.Read)]
        public static jgrid.GridResponse<MemberGroupRow> execute(MemberGroupSelect command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<MemberGroupRow> data = new jgrid.GridResponse<MemberGroupRow>();
                StringBuilder sql = new StringBuilder(@"select * from MemberGroup nolock");

                int cnt = 0;
                sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
                sql.AppendFormat(" order by {0}", command.GetOrderBy());

                //data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
                //data.page_size = command.page_size;
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.ToString()))
                    data.rows.Add(r.ToObject<MemberGroupRow>());
                return data;
            }
            //jgrid.GridResponse<MemberGroupRow> data = new jgrid.GridResponse<MemberGroupRow>();
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            //{
            //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from MemberGroup nolock order by CorpID asc, GroupID asc"))
            //        data.rows.Add(r.ToObject<MemberGroupRow>());
            //    return data;
            //}
        }

        //static object execute(MemberGroupUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

        //static object execute(MemberGroupInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
    }
}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberGroupUpdate : MemberGroupRowCommand, IRowCommand { }

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberGroupInsert : MemberGroupRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberGroupPermissionSelect : PermissionGroupSelect
{
    protected override UserType UserType { get { return BU.UserType.Member; } }

    [ObjectInvoke, Permissions(Permissions.Code.membergroup_permission, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<MenuRow> execute(MemberGroupPermissionSelect command, string json_s, params object[] args)
    {
        return command.execute(json_s, args);
    }

    [ObjectInvoke, Permissions(Permissions.Code.membergroup_permission, Permissions.Flag.Write)]
    static object execute(MemberGroupPermissionUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberGroupPermissionUpdate : PermissionGroupUpdate, IRowCommand { protected override UserType UserType { get { return BU.UserType.Member; } } }
