using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Tools;
using Tools.Protocol;
using web;

namespace web
{
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
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;
        [DbImport, JsonProperty]
        public decimal FeesRate;
        [DbImport, JsonProperty]
        public string DisplayName;

        public class Cache : WebTools.ListCache<Cache, CashChannelRow>
        {
            [SqlSetting("Cache", "Banklist"), DefaultValue(5000.0)]
            public override double LifeTime
            {
                get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
                set { }
            }

            public override void Update(SqlCmd sqlcmd, string key, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    List<CashChannelRow> rows = new List<CashChannelRow>();
                    foreach (var n in _null<BankCardRowCommand>.value.GetRows(sqlcmd))
                        rows.Add(n);
                    foreach (var n in _null<DinpayRowCommand>.value.GetRows(sqlcmd))
                        rows.Add(n);
                    foreach (var n in _null<YeepayRowCommand>.value.GetRows(sqlcmd))
                        rows.Add(n);
                    foreach (var n in _null<EcpssRowCommand>.value.GetRows(sqlcmd))
                        rows.Add(n);
                    base.Rows = rows;
                }
            }

            public CashChannelRow RandomGetRow()
            {
                int n = this.Rows.Count;
                if (n == 0) return null;
                return this.Rows[RandomValue.GetInt32() % n];
            }
        }
    }

    public abstract class CashChannelRowCommand
    {
        public abstract StringEx.sql_str TableName { get; }
        public abstract LogType AcceptLogType { get; }
        public abstract StringEx.sql_str MerhantID_Name { get; }
        public abstract Type RowType { get; }

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
        [JsonProperty]
        public decimal? FeesRate;
        [JsonProperty]
        public string DisplayName;

        public abstract CashChannelRow insert(string json_s, params object[] args);
        public abstract CashChannelRow update(string json_s, params object[] args);
    }

    public abstract class CashChannelRowCommand<TRow> : CashChannelRowCommand where TRow : CashChannelRow, new()
    {
        public override Type RowType
        {
            get { return typeof(TRow); }
        }

        public TRow GetRow(SqlCmd sqlcmd, Guid? id)
        {
            if (id.HasValue)
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                    return sqlcmd.ToObject<TRow>("select * from {0} nolock where ID='{1}'", this.TableName, id);
            return null;
        }

        public IEnumerable<TRow> GetRows(SqlCmd sqlcmd)
        {
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from {0} nolock where Locked=0", this.TableName))
                yield return r.ToObject<TRow>();
        }

        protected virtual void insert_fill(sqltool s) { }
        public override CashChannelRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["*", "CorpID", "     "] = this.CorpID;
            s["*", "LogType", "    "] = this.LogType;
            s[" ", "Locked", "     "] = this.Locked ?? BU.Locked.Active;
            s["N", "DisplayName", ""] = this.DisplayName * text.ValidAsString;
            this.insert_fill(s);
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            s.values["TableName"] = this.TableName;
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<TRow>(s.BuildEx(@"declare @id uniqueidentifier set @id=newid()
insert into {TableName} (ID,", sqltool._Fields, ") values (@id,", sqltool._Values, @")
select * from {TableName} nolock where ID=@id"));
        }

        protected abstract void update_fill(sqltool s, TRow row);
        public override CashChannelRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                TRow row = this.GetRow(sqlcmd, this.ID);
                if (row == null) throw new RowException(RowErrorCode.NotFound);
                sqltool s = new sqltool();
                s[" ", "CorpID", "     ", row.CorpID, "     "] = this.CorpID;
                s[" ", "Locked", "     ", row.Locked, "     "] = this.Locked;
                s["N", "DisplayName", "", row.DisplayName, ""] = this.DisplayName * text.ValidAsString;
                s[" ", "FeesRate", "   ", row.FeesRate, "   "] = this.FeesRate;
                this.update_fill(s, row);
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.values["TableName"] = this.TableName;
                s.Values["ID"] = row.ID;
                return sqlcmd.ExecuteEx<TRow>(s.BuildEx2("update {TableName} set ", sqltool._FieldValue, @" where ID={ID}
select * from {TableName} nolock where ID={ID}"));
            }
        }
    }



    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BankCardRowData : CashChannelRow
    {
        [DbImport, JsonProperty("MerhantID")]
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
        public override LogType AcceptLogType { get { return BU.LogType.Deposit; } }
        public override StringEx.sql_str TableName { get { return s_TableName; } } static StringEx.sql_str s_TableName = "cashBankCard";
        public override StringEx.sql_str MerhantID_Name { get { return s_MerhantID; } } static StringEx.sql_str s_MerhantID = "CardID";

        [JsonProperty("MerhantID")]
        public string CardID;
        [JsonProperty]
        public string BankName;
        [JsonProperty]
        public string AccName;
        [JsonProperty, JsonProtocol.String(Empty = true)]
        public string Loc1;
        [JsonProperty, JsonProtocol.String(Empty = true)]
        public string Loc2;
        [JsonProperty, JsonProtocol.String(Empty = true)]
        public string Loc3;

        protected override void insert_fill(sqltool s)
        {
            s["N", "CardID", "  "] = text.ValidAsString * this.CardID ?? "";
            s["N", "BankName", ""] = text.ValidAsString * this.BankName ?? "";
            s["N", "AccName", " "] = text.ValidAsString * this.AccName ?? "";
            s["N", "Loc1", "    "] = text.ValidAsString * this.Loc1 ?? "";
            s["N", "Loc2", "    "] = text.ValidAsString * this.Loc2 ?? "";
            s["N", "Loc3", "    "] = text.ValidAsString * this.Loc3 ?? "";
        }
        protected override void update_fill(sqltool s, BankCardRowData row)
        {
            s["N", "CardID", "  ", row.CardID, "  "] = text.ValidAsString * this.CardID;
            s["N", "BankName", "", row.BankName, ""] = text.ValidAsString * this.BankName;
            s["N", "AccName", " ", row.AccName, " "] = text.ValidAsString * this.AccName;
            s["N", "Loc1", "    ", row.Loc1, "    "] = text.ValidAsString * this.Loc1;
            s["N", "Loc2", "    ", row.Loc2, "    "] = text.ValidAsString * this.Loc2;
            s["N", "Loc3", "    ", row.Loc3, "    "] = text.ValidAsString * this.Loc3;
        }
    }



    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class DinpayRowData : CashChannelRow
    {
        [DbImport, JsonProperty]
        public string alias_domain;
        [DbImport, JsonProperty("MerhantID")]
        public string M_ID;         // 商家號
        [DbImport, JsonProperty]
        public string M_URL;        // 反饋網址
        [DbImport, JsonProperty]
        public string action_Url;   // 提交網址
        [DbImport, JsonProperty]
        public string sec_key;      // 密鑰

        public class MP
        {
            public Guid? ChannelID;
            public Guid? TransactionID;
            public string SourceSite;
        }
    }

    public class DinpayRowCommand : CashChannelRowCommand<DinpayRowData>
    {
        public override LogType AcceptLogType { get { return BU.LogType.Dinpay; } }
        public override StringEx.sql_str TableName { get { return s_TableName; } } static StringEx.sql_str s_TableName = "cashDinapy";
        public override StringEx.sql_str MerhantID_Name { get { return s_MerhantID; } } static StringEx.sql_str s_MerhantID = "M_ID";

        [JsonProperty]
        public string alias_domain;
        [JsonProperty("MerhantID")]
        public string M_ID;             // 商家號
        [JsonProperty]
        public string M_URL;            // 反饋網址
        [JsonProperty]
        public string action_Url;       // 提交網址
        [JsonProperty]
        public string sec_key;          // 密鑰

        protected override void insert_fill(sqltool s)
        {
            s[" ", "FeesRate", "        "] = this.FeesRate ?? 0;
            s[" ", "alias_domain", "    "] = text.ValidAsString * this.alias_domain;
            s[" ", "M_ID", "            "] = text.ValidAsString * this.M_ID;
            s[" ", "M_URL", "           "] = text.ValidAsString * this.M_URL ?? "";
            s[" ", "action_Url", "      "] = text.ValidAsString * this.action_Url ?? "";
            s[" ", "sec_key", "         "] = text.ValidAsString * this.sec_key ?? "";
        }
        protected override void update_fill(sqltool s, DinpayRowData row)
        {
            s[" ", "FeesRate", "        ", row.FeesRate, "      "] = this.FeesRate ?? 0;
            s[" ", "alias_domain", "    ", row.alias_domain, "  "] = text.ValidAsString * this.alias_domain;
            s[" ", "M_ID", "            ", row.M_ID, "          "] = text.ValidAsString * this.M_ID;
            s[" ", "M_URL", "           ", row.M_URL, "         "] = text.ValidAsString * this.M_URL;
            s[" ", "action_Url", "      ", row.action_Url, "    "] = text.ValidAsString * this.action_Url;
            s[" ", "sec_key", "         ", row.sec_key, "       "] = text.ValidAsString * this.sec_key;
        }
    }



    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class YeepayRowData : CashChannelRow
    {
        [DbImport, JsonProperty]
        public string alias_domain;
        [DbImport, JsonProperty]
        public string authorizationURL;
        [DbImport, JsonProperty("MerhantID")]
        public string merhantId;
        [DbImport, JsonProperty]
        public string p5_Pid;
        [DbImport, JsonProperty]
        public string p6_Pcat;
        [DbImport, JsonProperty]
        public string p7_Pdesc;
        [DbImport, JsonProperty]
        public string pa_MP;
        [DbImport, JsonProperty]
        public string pd_FrpId;
        [DbImport, JsonProperty]
        public string keyValue;

        public class MP
        {
            public Guid? ChannelID;
            public Guid? TransactionID;
            public string SourceSite;
        }
    }

    public class YeepayRowCommand : CashChannelRowCommand<YeepayRowData>
    {
        public override LogType AcceptLogType { get { return BU.LogType.YeePay; } }
        public override StringEx.sql_str TableName { get { return s_TableName; } } static StringEx.sql_str s_TableName = "cashYeepay";
        public override StringEx.sql_str MerhantID_Name { get { return s_MerhantID; } } static StringEx.sql_str s_MerhantID = "merhantId";

        [JsonProperty]
        public string alias_domain;
        [JsonProperty]
        public string authorizationURL;
        [JsonProperty("MerhantID")]
        public string merhantId;
        [JsonProperty]
        public string p5_Pid;
        [JsonProperty]
        public string p6_Pcat;
        [JsonProperty]
        public string p7_Pdesc;
        [JsonProperty]
        public string pa_MP;
        [JsonProperty]
        public string pd_FrpId;
        [JsonProperty]
        public string keyValue;

        protected override void insert_fill(sqltool s)
        {
            s[" ", "FeesRate", "        "] = this.FeesRate ?? 0;
            s[" ", "alias_domain", "    "] = text.ValidAsString * this.alias_domain;
            s[" ", "authorizationURL", ""] = text.ValidAsString * this.authorizationURL;
            s[" ", "merhantId", "       "] = text.ValidAsString * this.merhantId;
            s[" ", "p5_Pid", "          "] = text.ValidAsString * this.p5_Pid;
            s[" ", "p6_Pcat", "         "] = text.ValidAsString * this.p6_Pcat;
            s[" ", "p7_Pdesc", "        "] = text.ValidAsString * this.p7_Pdesc;
            s[" ", "pa_MP", "           "] = text.ValidAsString * this.pa_MP;
            s[" ", "pd_FrpId", "        "] = text.ValidAsString * this.pd_FrpId;
            s[" ", "keyValue", "        "] = text.ValidAsString * this.keyValue;
        }
        protected override void update_fill(sqltool s, YeepayRowData row)
        {
            s[" ", "FeesRate", "        ", row.FeesRate, "        "] = this.FeesRate ?? 0;
            s[" ", "alias_domain", "    ", row.alias_domain, "    "] = text.ValidAsString * this.alias_domain;
            s[" ", "authorizationURL", "", row.authorizationURL, ""] = text.ValidAsString * this.authorizationURL;
            s[" ", "merhantId", "       ", row.merhantId, "       "] = text.ValidAsString * this.merhantId;
            s[" ", "p5_Pid", "          ", row.p5_Pid, "          "] = text.ValidAsString * this.p5_Pid;
            s[" ", "p6_Pcat", "         ", row.p6_Pcat, "         "] = text.ValidAsString * this.p6_Pcat;
            s[" ", "p7_Pdesc", "        ", row.p7_Pdesc, "        "] = text.ValidAsString * this.p7_Pdesc;
            s[" ", "pa_MP", "           ", row.pa_MP, "           "] = text.ValidAsString * this.pa_MP;
            s[" ", "pd_FrpId", "        ", row.pd_FrpId, "        "] = text.ValidAsString * this.pd_FrpId;
            s[" ", "keyValue", "        ", row.keyValue, "        "] = text.ValidAsString * this.keyValue;
        }
    }


    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EcpssRowData : CashChannelRow
    {
        [DbImport, JsonProperty]
        public string alias_domain;
        [DbImport, JsonProperty]
        public string authorizationURL;
        [DbImport, JsonProperty("MerhantID")]
        public string merhantId;
        [DbImport, JsonProperty]
        public string MD5key;
        [DbImport, JsonProperty]
        public string OrderDesc;
        [DbImport, JsonProperty]
        public string Remark;
        [DbImport, JsonProperty]
        public string Products;

        public class MP
        {
            public Guid? ChannelID;
            public Guid? TransactionID;
            public string SourceSite;
        }
    }

    public class EcpssRowCommand : CashChannelRowCommand<EcpssRowData>
    {
        public override LogType AcceptLogType { get { return BU.LogType.Ecpss; } }
        public override StringEx.sql_str TableName { get { return s_TableName; } } static StringEx.sql_str s_TableName = "cashEcpss";
        public override StringEx.sql_str MerhantID_Name { get { return s_MerhantID; } } static StringEx.sql_str s_MerhantID = "merhantId";

        [JsonProperty]
        public string alias_domain;
        [JsonProperty]
        public string authorizationURL;
        [JsonProperty("MerhantID")]
        public string merhantId;
        [JsonProperty]
        public string MD5key;
        [JsonProperty]
        public string OrderDesc;
        [JsonProperty]
        public string Remark;
        [JsonProperty]
        public string Products;

        protected override void insert_fill(sqltool s)
        {
            s[" ", "FeesRate", "        "] = this.FeesRate ?? 0;
            s[" ", "alias_domain", "    "] = text.ValidAsString * this.alias_domain;
            s[" ", "authorizationURL", ""] = text.ValidAsString * this.authorizationURL;
            s[" ", "merhantId", "       "] = text.ValidAsString * this.merhantId;
            s[" ", "MD5key", "       "] = text.ValidAsString * this.MD5key;
            s[" ", "OrderDesc", "       "] = text.ValidAsString * this.OrderDesc;
            s[" ", "Remark", "       "] = text.ValidAsString * this.Remark;
            s[" ", "Products", "       "] = text.ValidAsString * this.Products;
        }
        protected override void update_fill(sqltool s, EcpssRowData row)
        {
            s[" ", "FeesRate", "        ", row.FeesRate, "        "] = this.FeesRate ?? 0;
            s[" ", "alias_domain", "    ", row.alias_domain, "    "] = text.ValidAsString * this.alias_domain;
            s[" ", "authorizationURL", "", row.authorizationURL, ""] = text.ValidAsString * this.authorizationURL;
            s[" ", "merhantId", "       ", row.merhantId, "       "] = text.ValidAsString * this.merhantId;
            s[" ", "MD5key", "          ", row.MD5key, "          "] = text.ValidAsString * this.MD5key;
            s[" ", "OrderDesc", "       ", row.OrderDesc, "       "] = text.ValidAsString * this.OrderDesc;
            s[" ", "Remark", "          ", row.Remark, "          "] = text.ValidAsString * this.Remark;
            s[" ", "Products", "        ", row.Products, "        "] = text.ValidAsString * this.Products;
        }
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