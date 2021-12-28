using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;
using Tools;

namespace web
{
    [DebuggerStepThrough]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdminRow : UserRow
    {
        public override UserType UserType { get { return BU.UserType.Admin; } }
        static StringEx.sql_str s_TableName = "Admin";
        public override string TableName { get { return s_TableName.value; } }
        public override StringEx.sql_str TableName2 { get { return s_TableName; } }
        static StringEx.sql_str s_GroupTableName = (StringEx.sql_str)"AdminGroup";
        public override StringEx.sql_str GroupTableName { get { return s_GroupTableName; } }
        public override int? ParentID
        {
            get { return null; }
            set { }
        }
        public override string ParentACNT
        {
            get { return null; }
            set { }
        }

        [DbImport, JsonProperty]
        public override int UserLevel
        {
            get;
            set;
        }
        public static AdminRow GetAdmin(SqlCmd sqlcmd, int? adminID, int? corpID, string acnt, params string[] fields)
        {
            return GetUser<AdminRow>(sqlcmd, "Admin", adminID, corpID, acnt, fields);
        }
        public static AdminRow GetAdminEx(SqlCmd sqlcmd, int? adminID, int? corpID, string acnt, params string[] fields)
        {
            AdminRow row = GetAdmin(sqlcmd, adminID, corpID, acnt, fields);
            if (row == null) throw new RowException(RowErrorCode.AdminNotFound);
            return row;
        }
    }

    public class AdminRowCommand : UserRowCommand
    {
        public bool Password_verify;
        public string Password_old;

        public AdminRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                AdminRow row = sqlcmd.GetRowEx<AdminRow>(RowErrorCode.AdminNotFound, "select * from [Admin] nolock where ID={0}", this.ID);
                if (this.Password_verify)
                {
                    string p1 = text.EncodePassword(row.ACNT, this.Password_old);
                    if (p1 != row.Password)
                        throw new RowException(RowErrorCode.PasswordError);
                }
                sqltool s = new sqltool();
                s[" ", "GroupID", "   ", row.GroupID, "  "] = this.GroupID;
                s["N", "Name", "      ", row.Name, "     "] = this.Name * text.ValidAsName;
                s[" ", "UserLevel", " ", row.UserLevel, ""] = this.UserLevel;
                s[" ", "GroupID", "   ", row.GroupID, "  "] = this.GroupID;
                s[" ", "pwd", "       ", row.Password, " "] = text.EncodePassword(row.ACNT, this.Password);
                s[" ", "Locked", "    ", row.Locked, "   "] = this.Locked;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.BuildEx("update Admin set ", sqltool._FieldValue, " where ID={ID} select * from [Admin] nolock where ID={ID}");
                return sqlcmd.ExecuteEx<AdminRow>(sqlstr);
            }
        }

        public AdminRow insert(SqlCmd sqlcmd, CorpRow corp_row, string json_s, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                this.apply_GroupID(sqlcmd);
                string acnt = text.ValidAsACNT * this.ACNT;
                sqltool s = new sqltool();
                //select ID, Sort from AdminGroup nolock where CorpID=2 order by Sort
                s["*", "CorpID", "    "] = this.CorpID;
                s["*", "ACNT", "      "] = acnt;
                s["*", "UserLevel", " "] = this.UserLevel ?? 0;
                s["*", "GroupID", "   "] = this.GroupID;
                s["N", "Name", "      "] = (text.ValidAsName * this.Name) ?? acnt;
                s[" ", "pwd", "       "] = text.EncodePassword(acnt, this.Password ?? _Global.DefaultPassword);
                s[" ", "Locked", "    "] = this.Locked ?? BU.Locked.Active;
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                s.TestFieldNeeds();
                s.Values["CorpID"] = (StringEx.sql_str)"ID";
                s.Values["CorpID_"] = this.CorpID;
                if (corp_row != null)
                {
                    s["", "pwd", ""] = "Disabled";
                    if (corp_row.ID == 0)
                    {
                        s.Values["ID"] = _Global.RootAdminID;
                        s["", "pwd", ""] = text.EncodePassword(acnt, acnt);
                    }
                }
                //            string sqlstr = s.BuildEx("declare @ADMINID int ", s.Values.ContainsKey("ID") ? "set @ADMINID={ID}" : "exec alloc_UserID @ADMINID output, @type='Admin',@corpid={CorpID_},@acnt={ACNT}", @"
                //insert into [Admin] (ID, GroupID,", sqltool._Fields, @")
                //select @ADMINID, b.ID,", sqltool._Values, @"
                //from Corp a with(nolock)
                //left join AdminGroup b with(nolock) on a.ID=b.CorpID
                //where b.Class=isnull({GroupID},1) and a.ID={CorpID_}
                //", AdminRowCommand.select_from_admin, " where a.ID=@ADMINID");
                string sqlstr = s.BuildEx("declare @ADMINID int ", s.Values.ContainsKey("ID") ? "set @ADMINID={ID}" : "exec alloc_UserID @ADMINID output, @type='Admin',@corpid={CorpID_},@acnt={ACNT}", @"
insert into [Admin] ([ID],", sqltool._Fields, @")
select @ADMINID,", sqltool._Values, @"
from Corp nolock where ID={CorpID_}
select * from [Admin] nolock where ID=@ADMINID");
                //string sqlstr = s.BuildEx("exec admin_insert ", sqltool._AtFieldValue);
                if (corp_row != null)
                    return sqlcmd.ToObject<AdminRow>(sqlstr);
                else
                    return sqlcmd.ExecuteEx<AdminRow>(sqlstr);
            }
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            //    return sqlcmd.ExecuteEx<AdminRow>(sqlstr);
        }
    }


    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminSelect : jgrid.GridRequest<AdminSelect>
    {
        protected override string init_defaultkey() { return "CreateTime"; }
        protected override Dictionary<string, string> init_sortkeys()
        {
            return new Dictionary<string, string>()
        {
            {"ID", "ID"},
            {"CorpID", "CorpID"},
            {"ACNT", "ACNT"},
            {"Name", "Name"},
            {"GroupID", "CorpID {0}, GroupID"},
            {"Locked", "Locked"},
            {"ModifyUser", "ModifyUser"},
            {"ModifyTime", "ModifyTime"},
            {"CreateUser", "CreateUser"},
        };
        }

        [JsonProperty]
        string ACNT;
        [JsonProperty]
        string Name;
        [JsonProperty]
        Locked? Locked;


        [ObjectInvoke, Permissions(Permissions.Code.admins_list, Permissions.Flag.Write | Permissions.Flag.Read)]
        static jgrid.GridResponse<AdminRow> select(AdminSelect command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<AdminRow> data = new jgrid.GridResponse<AdminRow>();
                StringBuilder sql = new StringBuilder(@"from [Admin] nolock");

                int cnt = 0;
                sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
                sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
                sql_where(sql, ref cnt, "Name like N'%{0}%'", (command.Name * text.ValidAsName).Remove("%"));
                sql_where(sql, ref cnt, "Locked={0}", (byte?)command.Locked);

                data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
                data.page_size = command.page_size;
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                    data.rows.Add(r.ToObject<AdminRow>());
                return data;
            }

            //jgrid.GridResponse<AdminRow> data = new jgrid.GridResponse<AdminRow>();
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            //{
            //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from [Admin] nolock"))
            //       data.rows.Add(r.ToObject<AdminRow>());
            //    return data;
            //}
        }

        [ObjectInvoke, Permissions(Permissions.Code.admins_list, Permissions.Flag.Write)]
        static object update(AdminUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

        [ObjectInvoke, Permissions(Permissions.Code.admins_list, Permissions.Flag.Write)]
        static object insert(AdminInsert command, string json_s, params object[] args) { return command.insert(null, null, json_s, args); }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminUpdate : AdminRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminInsert : AdminRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminSetPassword : IRowCommand
    {
        [JsonProperty]
        string Password1;
        [JsonProperty]
        string Password2;
        [JsonProperty]
        string Password3;

        [ObjectInvoke]
        static object update(AdminSetPassword command, string json_s, params object[] args)
        {
            Admin admin = (Admin)HttpContext.Current.User;
            if (string.IsNullOrEmpty(command.Password2))
                throw new RowException(RowErrorCode.FieldNeeds, "new password");
            if (command.Password2 != command.Password3)
                throw new RowException(RowErrorCode.FieldNeeds, "new password");
            return new AdminUpdate()
            {
                ID = ((Admin)HttpContext.Current.User).ID,
                Password = command.Password2,
                Password_verify = true,
                Password_old = command.Password1
            }.update(json_s, args);
        }
    }
}