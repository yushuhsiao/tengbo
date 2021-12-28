using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace web
{
//    public class TranLogWriter
//    {
//        public DateTime? ACTime;
//        public LogType? LogType;
//        public string LogTypeText;
//        public GameID? GameID;
//        public int? CorpID;
//        public UserType? srcUserType;
//        public int? srcUserID;
//        public string srcUserACNT;
//        public int? srcParentID;
//        public string srcParentACNT;
//        public UserType? UserType;
//        public int? UserID;
//        public string UserACNT;
//        public int? ParentID;
//        public string ParentACNT;
//        public decimal? PrevBalance1;
//        public decimal? PrevBalance2;
//        public decimal? Balance1;
//        public decimal? Balance2;
//        public decimal? CashAmount;
//        public decimal? CashPCT;
//        public decimal? CashFees;
//        public decimal? BetAmount;
//        public decimal? BetBonus;
//        public decimal? BetPayout;
//        public decimal? BetShare;
//        public CurrencyCode? CurrencyA; string _CurrencyA { get { if (this.CurrencyA.HasValue) return this.CurrencyA.Value.ToString(); return null; } }
//        public CurrencyCode? CurrencyB; string _CurrencyB { get { if (this.CurrencyB.HasValue) return this.CurrencyB.Value.ToString(); return null; } }
//        public decimal? CurrencyX;
//        public string SerialNumber;
//        public Guid? TranID;
//        public string RequestIP;
//        public DateTime? RequestTime;
//        public DateTime? FinishTime;

//        public TranLogWriter(tran.RowData tranrow2, UserRow srcUser, UserRow user)
//        {
//            this.ACTime = tranrow2.CreateTime.ToACTime();
//            this.LogType = tranrow2.LogType;
//            this.GameID = 0;
//            this.CorpID = tranrow2.CorpID;

//            if (srcUser == null)
//            {
//                this.srcParentID = 0;
//                this.srcParentACNT = "";
//                this.srcUserType = 0;
//                this.srcUserID = 0;
//                this.srcUserACNT = "";
//                this.PrevBalance2 = 0;
//                this.Balance2 = 0;
//            }
//            else
//            {
//                this.srcParentID = srcUser.ParentID ?? 0;
//                this.srcParentACNT = srcUser.ParentACNT ?? "";
//                this.srcUserType = srcUser.UserType;
//                this.srcUserID = srcUser.ID;
//                this.srcUserACNT = srcUser.ACNT;
//                this.PrevBalance2 = tranrow2.PrevBalance2;
//                this.Balance2 = srcUser.Balance;
//            }
//            this.ParentID = user.ParentID ?? 0;
//            this.ParentACNT = user.ParentACNT ?? "";
//            this.UserType = user.UserType;
//            this.UserID = user.ID;
//            this.UserACNT = user.ACNT;
//            this.PrevBalance1 = tranrow2.PrevBalance1;
//            this.Balance1 = user.Balance;
//            this.CurrencyA = tranrow2.CurrencyA;
//            this.CurrencyB = tranrow2.CurrencyB;
//            this.CurrencyX = tranrow2.CurrencyX;
//            this.SerialNumber = tranrow2.SerialNumber;
//            this.TranID = tranrow2.ID;
//            this.RequestIP = tranrow2.RequestIP;
//            this.RequestTime = tranrow2.CreateTime;
//            this.FinishTime = tranrow2.FinishTime;
//        }

//        static string sql1;
//        static string sql2;
//        static void init_sql(SqlCmd sqlcmd, bool swap_user)
//        {
//            if ((sql1 != null) && (sql2 != null)) return;
//            StringBuilder s0 = new StringBuilder();
//            StringBuilder s1 = new StringBuilder();
//            StringBuilder s2 = new StringBuilder();
//            string sql0 = @"insert into TranLog ([ACTime]{0})
//values ({{ACTime:{2}}}{1})
//select * from TranLog nolock where sn=@@Identity";
//            foreach (string field in SqlSchemaTable.GetSchema(sqlcmd, "TranLog").Keys)
//            {
//                string v1, v2;
//                v1 = v2 = string.Format(", {{{0}}}", field);
//                switch (field)
//                {
//                    case "sn":
//                    case "ACTime":
//                        continue;
//                    case "RequestTime":
//                    case "FinishTime": v1 = v2 = string.Format(", {{{0}:{1}}}", field, sqltool.DateTimeFormat); break;
//                    case "CreateTime": v1 = v2 = ", getdate()"; break;
//                    case "srcUserType": v2 = ", {UserType}"; break;
//                    case "srcUserID": v2 = ", {UserID}"; break;
//                    case "srcUserACNT": v2 = ", {UserACNT}"; break;
//                    case "srcParentID": v2 = ", {ParentID}"; break;
//                    case "srcParentACNT": v2 = ", {ParentACNT}"; break;
//                    case "UserType": v2 = ", {srcUserType}"; break;
//                    case "UserID": v2 = ", {srcUserID}"; break;
//                    case "UserACNT": v2 = ", {srcUserACNT}"; break;
//                    case "ParentID": v2 = ", {srcParentID}"; break;
//                    case "ParentACNT": v2 = ", {srcParentACNT}"; break;
//                    case "Amount": 
//                        v1 = ", ({Balance1})-({PrevBalance1})"; 
//                        v2 = ", ({Balance2})-({PrevBalance2})"; break;
//                    case "PrevBalance":
//                        v1 = ", {PrevBalance1}";
//                        v2 = ", {PrevBalance2}"; break;
//                    case "Balance":
//                        v1 = ", {Balance1}";
//                        v2 = ", {Balance2}"; break;
//                    case "CurrencyA":
//                        v1 = ", {_CurrencyA}";
//                        v2 = ", {_CurrencyB}"; break;
//                    case "CurrencyB":
//                        v1 = ", {_CurrencyB}";
//                        v2 = ", {_CurrencyA}"; break;
//                    case "CurrencyX":
//                        v1 = ", {CurrencyX}";
//                        v2 = ", 1/{CurrencyX}"; break;
//                }
//                s0.AppendFormat(", [{0}]", field);
//                s1.Append(v1);
//                s2.Append(v2);
//            }
//            sql1 = string.Format(sql0, s0, s1, sqltool.DateFormat);
//            sql2 = string.Format(sql0, s0, s2, sqltool.DateFormat);
//        }
//        public TranLogRow Write(SqlCmd sqlcmd, bool swap_user)
//        {
//            this.CashAmount = this.CashAmount ?? 0;
//            this.CashPCT = this.CashPCT ?? 0;
//            this.CashFees = this.CashFees ?? 0;
//            this.BetAmount = this.BetAmount ?? 0;
//            this.BetBonus = this.BetBonus ?? 0;
//            this.BetPayout = this.BetPayout ?? 0;
//            this.BetShare = this.BetShare ?? 0;
//            string sqlstr;
//            decimal? _prevBalance, _balance;
//            lock (typeof(TranLogRow))
//            {
//                init_sql(sqlcmd, swap_user);
//                if (swap_user)
//                { sqlstr = sql2; _prevBalance = this.PrevBalance2; _balance = this.Balance2; }
//                else
//                { sqlstr = sql1; _prevBalance = this.PrevBalance1; _balance = this.Balance1; }
//            }
//            if ((_prevBalance == _balance) && (this.CashFees == 0)) return null;
//            sqlstr = sqlstr.SqlExport(this);
//            return sqlcmd.ToObject<TranLogRow>(sqlstr);
//        }
//    }
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TranLogRow
    {
        [DbImport, JsonProperty]
        public long? sn;
        [DbImport, JsonProperty]
        public DateTime? ACTime;
        [DbImport, JsonProperty]
        public LogType? LogType;
        [DbImport, JsonProperty]
        public string LogTypeText;
        [DbImport, JsonProperty]
        public GameID? GameID;
        [DbImport, JsonProperty]
        public int? CorpID;
        //[DbImport, JsonProperty]
        //public UserType? srcUserType;
        //[DbImport, JsonProperty]
        //public int? srcUserID;
        //[DbImport, JsonProperty]
        //public string srcUserACNT;
        //[DbImport, JsonProperty]
        //public int? srcParentID;
        //[DbImport, JsonProperty]
        //public string srcParentACNT;
        [DbImport, JsonProperty]
        public int? IsProvider;
        [DbImport, JsonProperty]
        public UserType? UserType;
        [DbImport, JsonProperty]
        public int? UserID;
        [DbImport, JsonProperty]
        public string UserACNT;
        [DbImport, JsonProperty]
        public int? ParentID;
        [DbImport, JsonProperty]
        public string ParentACNT;
        [DbImport, JsonProperty]
        public decimal? PrevBalance;
        [DbImport, JsonProperty]
        public decimal? Amount;
        [DbImport, JsonProperty]
        public decimal? Balance;

        decimal? json_out(decimal? n, decimal? a, decimal? b)
        {
            if (a.HasValue && b.HasValue)
            {
                if (a.Value != 0) return n;
                if (b.Value != 0) return n;
            }
            return null;
        }

        /// <summary>
        /// 存提款:現金額度
        /// </summary>
        [DbImport]
        public decimal? CashAmount;
        [JsonProperty("CashAmount")]
        decimal? _CashAmount { get { return json_out(CashAmount, CashAmount, CashPCT); } }

        /// <summary>
        /// 存提款:佔成比
        /// </summary>
        [DbImport]
        public decimal? CashPCT;
        [JsonProperty("CashPCT")]
        decimal? _CashPCT { get { return json_out(CashPCT, CashAmount, CashPCT); } }

        /// <summary>
        /// 存提款:佔成比
        /// </summary>
        [DbImport]
        public decimal? CashFees;
        [JsonProperty("CashFees")]
        decimal? _CashFees { get { return CashFees != 0 ? CashFees : null; } }

        /// <summary>
        /// 洗碼:有效投注額
        /// </summary>
        [DbImport]
        public decimal? BetAmount;
        [JsonProperty("BetAmount")]
        decimal? _BetAmount { get { return json_out(BetAmount, BetAmount, BetBonus); } }
        /// <summary>
        /// 洗碼:洗碼比
        /// </summary>
        [DbImport]
        public decimal? BetBonus;
        [JsonProperty("BetBonus")]
        decimal? _BetBonus { get { return json_out(BetBonus, BetAmount, BetBonus); } }

        /// <summary>
        /// 佣金:輸贏值
        /// </summary>
        [DbImport]
        public decimal? BetPayout;
        [JsonProperty("BetPayout")]
        decimal? _BetPayout { get { return json_out(BetPayout, BetPayout, BetShare); } }
        /// <summary>
        /// 佣金:佣金比例
        /// </summary>
        [DbImport]
        public decimal? BetShare;
        [JsonProperty("BetShare")]
        decimal? _BetShare { get { return json_out(BetShare, BetPayout, BetShare); } }


        [DbImport, JsonProperty]
        public CurrencyCode? CurrencyA;
        [DbImport, JsonProperty]
        public CurrencyCode? CurrencyB;
        [DbImport, JsonProperty]
        public decimal? CurrencyX;
        [DbImport, JsonProperty]
        public string SerialNumber;
        [DbImport, JsonProperty]
        public Guid? TranID;
        [DbImport, JsonProperty]
        public string RequestIP;
        [DbImport, JsonProperty]
        public DateTime? RequestTime;
        [DbImport, JsonProperty]
        public DateTime? FinishTime;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public Guid? CashChannelID;

//        static string sql1;
//        static string sql2;
//        public static TranLogRow Write(SqlCmd sqlcmd, TranLogRow logrow, bool currency_inv)
//        {
//            string sqlstr;
//            lock (typeof(TranLogRow))
//            {
//                if (sql1 == null)
//                {
//                    StringBuilder sb1 = new StringBuilder();
//                    StringBuilder sb2 = new StringBuilder();
//                    StringBuilder sb3 = new StringBuilder();
//                    string sql0 = @"insert into TranLog ([ACTime]{0})
//values ({{ACTime:{2}}}{1})
//select * from TranLog nolock where sn=@@Identity";
//                    foreach (string field in SqlSchemaTable.GetSchema(sqlcmd, "TranLog").Keys)
//                    {
//                        switch (field)
//                        {
//                            case "sn":
//                            case "ACTime":
//                                continue;
//                            case "RequestTime":
//                            case "FinishTime":
//                                sb2.AppendFormat(", {{{0}:{1}}}", field, sqltool.DateTimeFormat);
//                                sb3.AppendFormat(", {{{0}:{1}}}", field, sqltool.DateTimeFormat);
//                                break;
//                            default:
//                                sb2.AppendFormat(", {{{0}}}", field);
//                                sb3.AppendFormat(", {{{0}}}", field);
//                                break;
//                        }
//                        sb1.AppendFormat(", [{0}]", field);
//                    }
//                    sql1 = string.Format(sql0, sb1, sb2, sqltool.DateFormat);
//                    sql2 = string.Format(sql0, sb1, sb3, sqltool.DateFormat);

////                    sqltool s1 = new sqltool();
////                    foreach (string field in SqlSchemaTable.GetSchema(sqlcmd, "TranLog").Keys)
////                    {
////                        switch (field)
////                        {
////                            case "sn": continue;
////                            case "RequestTime":
////                            case "FinishTime": s1["", field, ""] = DateTime.Now; break;
////                            default: s1["", field, ""] = field; break;
////                        }
////                    }
////                    StringBuilder s2 = new StringBuilder(s1.Build(@"insert into TranLog (", sqltool._Fields, @")
////values (", sqltool._Values, @")
////select * from TranLog nolock where sn=@@Identity"));
////                    sql1 = s2
////                        .Replace("{ACTime}", string.Format("{{ACTime:{0}}}", sqltool.DateFormat))
////                        .Replace("{Amount}", "({Balance})-({PrevBalance})")
////                        .Replace("{CreateTime}", "getdate()")
////                        .Replace("{CurrencyA}", "{_CurrencyA}")
////                        .Replace("{CurrencyB}", "{_CurrencyB}")
////                        .ToString();
////                    sql2 = s2
////                        .Replace("{CurrencyX}", "1/{CurrencyX}")
////                        .ToString();
//                    //    .Replace("{CreateTime}", "getdate()")
//                    //    .Replace("{CurrencyX}", "1/{CurrencyX}")
//                    //    .ToString();
//                }
//                sqlstr = currency_inv ? sql2 : sql1;
//            }
//            if (logrow == null) return null;
//            if (logrow.PrevBalance == logrow.Balance) return null;
//            logrow.ACTime = logrow.RequestTime.Value.ToACTime();
//            logrow.GameID = logrow.GameID ?? 0;
//            logrow.ParentACNT = logrow.ParentACNT ?? "";
//            logrow.srcParentACNT = logrow.srcParentACNT ?? "";
//            logrow.CashAmount = logrow.CashAmount ?? 0;
//            logrow.CashPCT = logrow.CashPCT ?? 0;
//            logrow.BetAmount = logrow.BetAmount ?? 0;
//            logrow.BetBonus = logrow.BetBonus ?? 0;
//            logrow.BetPayout = logrow.BetPayout ?? 0;
//            logrow.BetShare = logrow.BetShare ?? 0;
//            sqlstr = sqlstr.SqlExport(logrow);
//            return sqlcmd.ToObject<TranLogRow>(sqlstr);
////            sqltool s = new sqltool();
////            s["", "ACTime", "         "] = logrow.RequestTime.Value.ToACTime();// (StringEx.sql_str)"dateadd(dd,datediff(dd,0,dateadd(hh,-12,@rt)),0)";
////            s["", "LogType", "        "] = logrow.LogType;
////            s["", "LogTypeText", "    "] = logrow.LogTypeText;
////            s["", "GameID", "         "] = logrow.GameID ?? 0;
////            s["", "CorpID", "         "] = logrow.CorpID;
////            s["", "srcUserType", "    "] = logrow.srcUserType;
////            s["", "srcUserID", "      "] = logrow.srcUserID;
////            s["", "srcUserACNT", "    "] = logrow.srcUserACNT;
////            s["", "srcParentID", "    "] = logrow.srcParentID;
////            s["", "srcParentACNT", "  "] = logrow.srcParentACNT;
////            s["", "UserType", "       "] = logrow.UserType;
////            s["", "UserID", "         "] = logrow.UserID;
////            s["", "UserACNT", "       "] = logrow.UserACNT;
////            s["", "ParentID", "       "] = logrow.ParentID;
////            s["", "ParentACNT", "     "] = logrow.ParentACNT;
////            s["", "PrevBalance", "    "] = logrow.PrevBalance;
////          //s["", "Amount", "         "] = (StringEx.sql_str)"({Balance})-({PrevBalance})";
////            s["", "Balance", "        "] = logrow.Balance;
////            s["", "CashAmount", "     "] = logrow.CashAmount ?? 0;
////            s["", "CashPCT", "        "] = logrow.CashPCT ?? 0;
////            s["", "BetAmount", "      "] = logrow.BetAmount ?? 0;
////            s["", "BetBonus", "       "] = logrow.BetBonus ?? 0;
////            s["", "BetPayout", "      "] = logrow.BetPayout ?? 0;
////            s["", "BetShare", "       "] = logrow.BetShare ?? 0;
////            s["", "CurrencyA", "      "] = logrow.CurrencyA;
////            s["", "CurrencyB", "      "] = logrow.CurrencyB;
////            s["", "CurrencyX", "      "] = logrow.CurrencyX;
////            s["", "SerialNumber", "   "] = logrow.SerialNumber;
////            s["", "TranID", "         "] = logrow.TranID;
////            s["", "RequestIP", "      "] = logrow.RequestIP;
////            s["", "RequestTime", "    "] = logrow.RequestTime;
////            s["", "FinishTime", "     "] = logrow.FinishTime;
////            s["", "CreateTime", "     "] = (StringEx.sql_str)"getdate()";
////            return sqlcmd.ToObject<TranLogRow>(s.BuildEx(@"insert into TranLog (", sqltool._Fields, ",Amount) values (", sqltool._Values, @",({Balance})-({PrevBalance}))
////select * from TranLog nolock where sn=@@Identity"));
//        }
    }
}
