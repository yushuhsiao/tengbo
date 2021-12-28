using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BankCardSelect : jgrid.GridRequest<BankCardSelect>
{
    protected override string init_defaultkey() { return "BankName"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"CorpID", "CorpID"},
            {"GroupID", "CorpID {0}, GroupID"},
            {"LogType", "LogType"},
            {"CardID", "CardID"},
            {"BankName", "BankName"},
            {"AccName", "AccName"},
            {"Loc1", "Loc1"},
            {"Loc2", "Loc2"},
            {"Loc3", "Loc3"},
            {"Locked", "Locked"},
            {"ExpireTime", "ExpireTime"},
            {"CreateTime", "CreateTime"},
            {"CreateUser", "CreateUser"},
            {"ModifyUser", "ModifyUser"},
            {"ModifyTime", "ModifyTime"},
        };
    }

    [JsonProperty]
    public string BankName;

    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<BankCardRow> execute(BankCardSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<BankCardRow> data = new jgrid.GridResponse<BankCardRow>();

            StringBuilder sql = new StringBuilder(@"from BankCard with(nolock)");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            //sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (command.AgentACNT * text.ValidAsACNT).Remove("%"));
            //sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            //sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (command.Name * text.ValidAsName).Remove("%"));
            //sql_where(sql, ref cnt, "a.Locked={0}", (byte?)(command.Locked * text.ValidAsLocked));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            //data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * {0} order by {1}", sql, command.GetOrderBy()))
                data.rows.Add(r.ToObject<BankCardRow>());
            return data;
        }
        //jgrid.GridResponse<BankCardRow> data = new jgrid.GridResponse<BankCardRow>();
        ////Dictionary<int, string> banks = new Dictionary<int, string>();
        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        //{
        //    //string sql;
        //    //if (command.BankID.HasValue)
        //    //    sql = string.Format("select * from BankCard with(nolock) where BankID={0}  order by CreateTime desc", command.BankID);
        //    //else
        //    //    sql = "select a.* from BankCard a with(nolock) inner join Bank b with(nolock) on a.BankID=b.ID where b.Locked=0 order by a.CreateTime desc";
        //    //foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select a.* from BankCard a with(nolock) inner join Bank b with(nolock) on a.BankID=b.ID where b.Locked=0 order by a.CreateTime desc"))
        //    //foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql))
        //    //    data.rows.Add(r.ToObject<BankCardRow>());
        //    //foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from BankCard nolock where Locked=0"))
        //        //banks[r.GetInt32("ID")] = r.GetStringN("Name");
        //    //data["banklist"] = banks;
        //    //data.Enums("BankID", banks);
        //    command.BankName *= text.ValidAsString;
        //    string sql;
        //    if (string.IsNullOrEmpty(command.BankName))
        //        sql = "select a.* from BankCard a with(nolock) inner join Bank b with(nolock) on a.BankName=b.Name where b.Locked=0 order by a.CreateTime desc";
        //    else
        //        sql = string.Format("select * from BankCard with(nolock) where BankName=N'{0}' order by CreateTime desc", command.BankName);
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql))
        //        data.rows.Add(r.ToObject<BankCardRow>());
        //    return data;
        //}
    }
}

namespace web
{
    class BankCardRowCommand2
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty("GroupID")]
        public virtual long? _in_GroupID { get; set; }
        public byte? GroupID
        {
            get { return text.GroupRowID_GroupID(this._in_GroupID); }
        }
        [JsonProperty]
        public LogType? LogType;
        [JsonProperty]
        public virtual string CardID { get; set; }
        [JsonProperty]
        public virtual string BankName { get; set; }
        [JsonProperty]
        public virtual string AccName { get; set; }
        [JsonProperty]
        public virtual string Loc1 { get; set; }
        [JsonProperty]
        public virtual string Loc2 { get; set; }
        [JsonProperty]
        public virtual string Loc3 { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual string Password { get; set; }
        [JsonProperty]
        public virtual DateTime? ExpireTime { get; set; }

        public BankCardRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                BankCardRow row = sqlcmd.GetRowEx<BankCardRow>(RowErrorCode.NotFound, "select * from BankCard nolock where ID={0}", this.ID);
                sqltool s = new sqltool();
                s[" ", "CorpID", "    ", row.CorpID, "    "] = this.CorpID;
                s[" ", "GroupID", "   ", row.GroupID, "   "] = this.GroupID;
                s[" ", "LogType", "   ", row.LogType, "   "] = this.LogType;
                s[" ", "CardID", "    ", row.CardID, "    "] = text.ValidAsString * this.CardID;
                s["N", "BankName", "  ", row.BankName, "  "] = text.ValidAsName * this.BankName;
                s["N", "AccName", "   ", row.AccName, "   "] = text.ValidAsName * this.AccName;
                s["N", "Loc1", "      ", row.Loc1, "      "] = text.ValidAsString * this.Loc1;
                s["N", "Loc2", "      ", row.Loc2, "      "] = text.ValidAsString * this.Loc2;
                s["N", "Loc3", "      ", row.Loc3, "      "] = text.ValidAsString * this.Loc3;
                s[" ", "Locked", "    ", row.Locked, "    "] = this.Locked;
                s[" ", "pwd", "       ", row.Password, "  "] = this.Password;
                s[" ", "ExpireTime", "", row.ExpireTime, ""] = this.ExpireTime;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.BuildEx("update BankCard set ", sqltool._FieldValue, " where ID={ID} select * from BankCard nolock where ID={ID}");
                return sqlcmd.ExecuteEx<BankCardRow>(sqlstr);
            }
        }

        public BankCardRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["*", "CorpID", "    "] = this.CorpID;
            s["*", "GroupID", "   "] = this.GroupID;
            s["*", "LogType", "   "] = this.LogType;
            s["*", "CardID", "    "] = text.ValidAsString * this.CardID;
            s["N", "BankName", "  "] = text.ValidAsName * this.BankName ?? "";
            s["N", "AccName", "   "] = text.ValidAsName * this.AccName ?? "";
            s["N", "Loc1", "      "] = text.ValidAsString * this.Loc1 ?? "";
            s["N", "Loc2", "      "] = text.ValidAsString * this.Loc2 ?? "";
            s["N", "Loc3", "      "] = text.ValidAsString * this.Loc3 ?? "";
            s[" ", "Locked", "    "] = this.Locked ?? BU.Locked.Active;
            s[" ", "pwd", "       "] = this.Password ?? "";
            s[" ", "ExpireTime", ""] = this.ExpireTime;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            s.fields.Remove("BankID");
            string sqlstr = s.BuildEx("insert into BankCard (", sqltool._Fields, ") values (", sqltool._Values, @")
if @@rowcount>0 select * from BankCard nolock where ID=@@IDENTITY");
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<BankCardRow>(sqlstr);
        }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BankCardUpdate : BankCardRowCommand2, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Write)]
    static object execute(BankCardUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BankCardInsert : BankCardRowCommand2, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Write)]
    static object execute(BankCardInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
}
