using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class ThirdPaymentSelect : jgrid.GridRequest<ThirdPaymentSelect>
//{
//    protected override string init_defaultkey() { return "BankName"; }
//    protected override Dictionary<string, string> init_sortkeys()
//    {
//        return new Dictionary<string, string>()
//        {
//            {"CorpID", "CorpID"},
//            {"GroupID", "CorpID {0}, GroupID"},
//            {"LogType", "LogType"},
//            {"Locked", "Locked"},
//            {"CardID", "CardID"},
//            {"BankName", "BankName"},
//            {"AccName", "AccName"},
//            {"Loc1", "Loc1"},
//            {"Loc2", "Loc2"},
//            {"Loc3", "Loc3"},
//            {"ExpireTime", "ExpireTime"},
//            {"CreateTime", "CreateTime"},
//            {"CreateUser", "CreateUser"},
//            {"ModifyUser", "ModifyUser"},
//            {"ModifyTime", "ModifyTime"},
//        };
//    }

//    [JsonProperty]
//    public string BankName;

//    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Read | Permissions.Flag.Write)]
//    static jgrid.GridResponse<ThirdPaymentRow> execute(ThirdPaymentSelect command, string json_s, params object[] args)
//    {
//        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
//        {
//            jgrid.GridResponse<ThirdPaymentRow> data = new jgrid.GridResponse<ThirdPaymentRow>();

//            StringBuilder sql = new StringBuilder(@"from BankCard with(nolock)");

//            int cnt = 0;
//            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
//            //sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (command.AgentACNT * text.ValidAsACNT).Remove("%"));
//            //sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
//            //sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (command.Name * text.ValidAsName).Remove("%"));
//            //sql_where(sql, ref cnt, "a.Locked={0}", (byte?)(command.Locked * text.ValidAsLocked));

//            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
//            //data.page_size = command.page_size;
//            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * {0} order by {1}", sql, command.GetOrderBy()))
//                data.rows.Add(r.ToObject<ThirdPaymentRow>());
//            return data;
//        }
//        //jgrid.GridResponse<ThirdPaymentRow> data = new jgrid.GridResponse<ThirdPaymentRow>();
//        ////Dictionary<int, string> banks = new Dictionary<int, string>();
//        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
//        //{
//        //    //string sql;
//        //    //if (command.BankID.HasValue)
//        //    //    sql = string.Format("select * from BankCard with(nolock) where BankID={0}  order by CreateTime desc", command.BankID);
//        //    //else
//        //    //    sql = "select a.* from BankCard a with(nolock) inner join Bank b with(nolock) on a.BankID=b.ID where b.Locked=0 order by a.CreateTime desc";
//        //    //foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select a.* from BankCard a with(nolock) inner join Bank b with(nolock) on a.BankID=b.ID where b.Locked=0 order by a.CreateTime desc"))
//        //    //foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql))
//        //    //    data.rows.Add(r.ToObject<ThirdPaymentRow>());
//        //    //foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from BankCard nolock where Locked=0"))
//        //        //banks[r.GetInt32("ID")] = r.GetStringN("Name");
//        //    //data["banklist"] = banks;
//        //    //data.Enums("BankID", banks);
//        //    command.BankName *= text.ValidAsString;
//        //    string sql;
//        //    if (string.IsNullOrEmpty(command.BankName))
//        //        sql = "select a.* from BankCard a with(nolock) inner join Bank b with(nolock) on a.BankName=b.Name where b.Locked=0 order by a.CreateTime desc";
//        //    else
//        //        sql = string.Format("select * from BankCard with(nolock) where BankName=N'{0}' order by CreateTime desc", command.BankName);
//        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql))
//        //        data.rows.Add(r.ToObject<ThirdPaymentRow>());
//        //    return data;
//        //}
//    }
//}

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class CashChannelRow
    {
        [DbImport, JsonProperty]
        public Guid ID;
        [DbImport, JsonProperty]
        public int CorpID;
        [DbImport, JsonProperty]
        public int?[] Groups;
        [DbImport, JsonProperty]
        public LogType LogType;
        [DbImport, JsonProperty]
        public Locked Locked;
        [DbImport, JsonProperty]
        public DateTime CreateTime;
        [DbImport, JsonProperty]
        public int CreateUser;
        [DbImport, JsonProperty]
        public DateTime ModifyTime;
        [DbImport, JsonProperty]
        public int ModifyUser;
    }

    public abstract class CashChannelRowCommand<TRow> where TRow : CashChannelRow, new()
    {
        [JsonProperty]
        public Guid? ID;
        [JsonProperty]
        public int? CorpID;
        [JsonProperty]
        public int?[] Groups;
        [JsonProperty]
        public LogType? LogType;
        [JsonProperty]
        public Locked? Locked;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class DinpayRowData : CashChannelRow
    {
        [DbImport, JsonProperty]
        public string M_ID;         // 商家號
        [DbImport, JsonProperty]
        public string M_URL;        // 反饋路徑
        [DbImport, JsonProperty]
        public string action;       // 提交路徑
        [DbImport, JsonProperty]
        public string key;          // 密鑰
    }

    public class DinpayRowCommand : CashChannelRowCommand<DinpayRowData>
    {
        [JsonProperty]
        public string M_ID;         // 商家號
        [JsonProperty]
        public string M_URL;        // 反饋路徑
        [JsonProperty]
        public string action;       // 提交路徑
        [JsonProperty]
        public string key;          // 密鑰
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BankCardRowData : CashChannelRow
    {
        [DbImport, JsonProperty]
        public string CardID;
        [DbImport, JsonProperty]
        public string BankName;
        [DbImport, JsonProperty]
        public string AccName;
        [DbImport, JsonProperty]
        public string Loc1;
        [DbImport, JsonProperty]
        public string Loc2;
        [DbImport, JsonProperty]
        public string Loc3;
    }

    public class BankCardRowCommand : CashChannelRowCommand<BankCardRowData>
    {
        [JsonProperty]
        public string CardID;
        [JsonProperty]
        public string BankName;
        [JsonProperty]
        public string AccName;
        [JsonProperty]
        public string Loc1;
        [JsonProperty]
        public string Loc2;
        [JsonProperty]
        public string Loc3;
    }

    //public class ThirdPaymentRow
    //{
    //    [DbImport, JsonProperty]
    //    public int? ID;
    //    [DbImport, JsonProperty]
    //    public int? CorpID;
    //    [DbImport]
    //    public byte? GroupID;
    //    [JsonProperty("GroupID")]
    //    long? _out_GroupID
    //    {
    //        get { return text.GroupRowID(this.CorpID, this.GroupID); }
    //    }
    //    [DbImport, JsonProperty]
    //    public LogType? LogType;
    //    [DbImport, JsonProperty]
    //    public string CardID;
    //    [DbImport, JsonProperty]
    //    public string BankName;
    //    [DbImport, JsonProperty]
    //    public string AccName;
    //    [DbImport, JsonProperty]
    //    public string Loc1;
    //    [DbImport, JsonProperty]
    //    public string Loc2;
    //    [DbImport, JsonProperty]
    //    public string Loc3;
    //    [DbImport, JsonProperty]
    //    public Locked? Locked;
    //    [DbImport("pwd"), JsonProperty]
    //    public string Password;
    //    [DbImport, JsonProperty]
    //    public DateTime? ExpireTime;
    //    [DbImport, JsonProperty]
    //    public DateTime? CreateTime;
    //    [DbImport, JsonProperty]
    //    public int? CreateUser;
    //    [DbImport, JsonProperty]
    //    public DateTime? ModifyTime;
    //    [DbImport, JsonProperty]
    //    public int? ModifyUser;
    //}

//    class ThirdPaymentRowCommand
//    {
//        [JsonProperty]
//        public virtual int? ID { get; set; }
//        [JsonProperty]
//        public virtual int? CorpID { get; set; }
//        [JsonProperty("GroupID")]
//        public virtual long? _in_GroupID { get; set; }
//        public byte? GroupID
//        {
//            get { return text.GroupRowID_GroupID(this._in_GroupID); }
//        }
//        [JsonProperty]
//        public LogType? LogType;
//        [JsonProperty]
//        public virtual string CardID { get; set; }
//        [JsonProperty]
//        public virtual string BankName { get; set; }
//        [JsonProperty]
//        public virtual string AccName { get; set; }
//        [JsonProperty]
//        public virtual string Loc1 { get; set; }
//        [JsonProperty]
//        public virtual string Loc2 { get; set; }
//        [JsonProperty]
//        public virtual string Loc3 { get; set; }
//        [JsonProperty]
//        public virtual Locked? Locked { get; set; }
//        [JsonProperty]
//        public virtual string Password { get; set; }
//        [JsonProperty]
//        public virtual DateTime? ExpireTime { get; set; }

//        public ThirdPaymentRow update(string json_s, params object[] args)
//        {
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                ThirdPaymentRow row = sqlcmd.GetRowEx<ThirdPaymentRow>(RowErrorCode.NotFound, "select * from BankCard nolock where ID={0}", this.ID);
//                sqltool s = new sqltool();
//                s[" ", "CorpID", "    ", row.CorpID, "    "] = this.CorpID;
//                s[" ", "GroupID", "   ", row.GroupID, "   "] = this.GroupID;
//                s[" ", "LogType", "   ", row.LogType, "   "] = this.LogType;
//                s[" ", "CardID", "    ", row.CardID, "    "] = text.ValidAsString * this.CardID;
//                s["N", "BankName", "  ", row.BankName, "  "] = text.ValidAsName * this.BankName;
//                s["N", "AccName", "   ", row.AccName, "   "] = text.ValidAsName * this.AccName;
//                s["N", "Loc1", "      ", row.Loc1, "      "] = text.ValidAsString * this.Loc1;
//                s["N", "Loc2", "      ", row.Loc2, "      "] = text.ValidAsString * this.Loc2;
//                s["N", "Loc3", "      ", row.Loc3, "      "] = text.ValidAsString * this.Loc3;
//                s[" ", "Locked", "    ", row.Locked, "    "] = this.Locked;
//                s[" ", "pwd", "       ", row.Password, "  "] = this.Password;
//                s[" ", "ExpireTime", "", row.ExpireTime, ""] = this.ExpireTime;
//                if (s.fields.Count == 0) return row;
//                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                s.Values["ID"] = row.ID;
//                string sqlstr = s.BuildEx("update BankCard set ", sqltool._FieldValue, " where ID={ID} select * from BankCard nolock where ID={ID}");
//                return sqlcmd.ExecuteEx<ThirdPaymentRow>(sqlstr);
//            }
//        }

//        public ThirdPaymentRow insert(string json_s, params object[] args)
//        {
//            sqltool s = new sqltool();
//            s["*", "CorpID", "    "] = this.CorpID;
//            s["*", "GroupID", "   "] = this.GroupID;
//            s["*", "LogType", "   "] = this.LogType;
//            s["*", "CardID", "    "] = text.ValidAsString * this.CardID;
//            s["N", "BankName", "  "] = text.ValidAsName * this.BankName ?? "";
//            s["N", "AccName", "   "] = text.ValidAsName * this.AccName ?? "";
//            s["N", "Loc1", "      "] = text.ValidAsString * this.Loc1 ?? "";
//            s["N", "Loc2", "      "] = text.ValidAsString * this.Loc2 ?? "";
//            s["N", "Loc3", "      "] = text.ValidAsString * this.Loc3 ?? "";
//            s[" ", "Locked", "    "] = this.Locked ?? BU.Locked.Active;
//            s[" ", "pwd", "       "] = this.Password ?? "";
//            s[" ", "ExpireTime", ""] = this.ExpireTime;
//            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
//            s.TestFieldNeeds();
//            s.fields.Remove("BankID");
//            string sqlstr = s.BuildEx("insert into BankCard (", sqltool._Fields, ") values (", sqltool._Values, @")
//if @@rowcount>0 select * from BankCard nolock where ID=@@IDENTITY");
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//                return sqlcmd.ExecuteEx<ThirdPaymentRow>(sqlstr);
//        }
//    }
}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class ThirdPaymentUpdate : ThirdPaymentRowCommand, IRowCommand
//{
//    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Write)]
//    static object execute(ThirdPaymentUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class ThirdPaymentInsert : ThirdPaymentRowCommand, IRowCommand
//{
//    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Write)]
//    static object execute(ThirdPaymentInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
//}
