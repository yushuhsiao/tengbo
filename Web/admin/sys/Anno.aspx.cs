using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;


namespace web
{

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AnnoSelect : jgrid.GridRequest<AnnoSelect>
    {
        protected override string init_defaultkey() { return "ModifyTime"; }
        protected override Dictionary<string, string> init_sortkeys()
        {
            return new Dictionary<string, string>()
        {
            {"ID", "ID"},
            {"CorpID", "CorpID"},
            {"Name", "Name"},
            {"Locked", "Locked"},
            {"Sort", "Sort"},
            {"Text", "Text"},
            {"ExpireTime", "ExpireTime"},
            {"CreateTime", "CreateTime"},
            {"CreateUser", "CreateUser"},
            {"ModifyUser", "ModifyUser"},
            {"ModifyTime", "ModifyTime"},
        };
        }

        [ObjectInvoke, Permissions(Permissions.Code.m_anno, Permissions.Flag.Read | Permissions.Flag.Write)]
        static jgrid.GridResponse<AnnoRow> execute(AnnoSelect command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<AnnoRow> data = new jgrid.GridResponse<AnnoRow>();

                StringBuilder sql = new StringBuilder(@"select * from Anno with(nolock)");

                int cnt = 0;
                sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
                //sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (command.AgentACNT * text.ValidAsACNT).Remove("%"));
                //sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
                //sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (command.Name * text.ValidAsName).Remove("%"));
                //sql_where(sql, ref cnt, "a.Locked={0}", (byte?)(command.Locked * text.ValidAsLocked));

                //data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
                //data.page_size = command.page_size;
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"{0} order by {1}", sql, command.GetOrderBy()))
                    data.rows.Add(r.ToObject<AnnoRow>());
                return data;
            }
            //jgrid.GridResponse<AnnoRow> data = new jgrid.GridResponse<AnnoRow>();
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            //{
            //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Anno nolock order by ModifyTime desc"))
            //        data.rows.Add(r.ToObject<AnnoRow>());
            //    //data["corps"] = CorpRow.Cache.GetInstance(null, sqlcmd).names;
            //    return data;
            //}
        }

        [ObjectInvoke, Permissions(Permissions.Code.m_anno, Permissions.Flag.Write)]
        static object execute(AnnoUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

        [ObjectInvoke, Permissions(Permissions.Code.m_anno, Permissions.Flag.Write)]
        static object execute(AnnoInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
    }
    
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AnnoRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public Locked? Locked;
        [DbImport, JsonProperty]
        public int? Sort;
        [DbImport, JsonProperty]
        public string Text;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;
    }

    public abstract class AnnoRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual int? Sort { get; set; }
        [JsonProperty]
        public virtual string Text { get; set; }

        public AnnoRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                AnnoRow row = sqlcmd.GetRowEx<AnnoRow>(RowErrorCode.NotFound, "select * from Anno nolock where ID={0}", this.ID);
                sqltool s = new sqltool();
                s["N", "Name", "  ", row.Name, "  "] = text.ValidAsName * this.Name;
                s[" ", "CorpID", "", row.CorpID, ""] = this.CorpID;
                s[" ", "Locked", "", row.Locked, ""] = this.Locked;
                s[" ", "Sort", "  ", row.Sort, "  "] = this.Sort;
                s["N", "Text", "  ", row.Text, "  "] = text.ValidAsString * this.Text;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sql = s.BuildEx("update Anno set ", sqltool._FieldValue, @" where ID={ID} select * from Anno nolock where ID={ID}");
                return sqlcmd.ExecuteEx<AnnoRow>(sql);
            }
        }

        public AnnoRow insert(string json_s, params object[] args)
        {
            //AnnoRow row = sqlcmd.GetRow<AnnoRow>("select ID from Anno nolock where ID={0}", this.ID);
            //if (row != null) throw new RowException(RowErrorCode.RowAlreadyExist);
            sqltool s = new sqltool();
            s["* ", "CorpID", ""] = this.CorpID;
            s["*N", "Name", "  "] = text.ValidAsName * this.Name;
            s["  ", "Locked", ""] = this.Locked ?? BU.Locked.Locked;
            s["  ", "Sort", "  "] = this.Sort ?? 0;
            s[" N", "Text", "  "] = (text.ValidAsString * this.Text) ?? "";
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx("insert into Anno (", sqltool._Fields, ") values (", sqltool._Values, @")
if @@rowcount>0 select * from Anno nolock where ID=@@IDENTITY");
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<AnnoRow>(sqlstr);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AnnoUpdate : AnnoRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AnnoInsert : AnnoRowCommand, IRowCommand { }
}