using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using BU;
using System.Reflection;

namespace web
{
    class sqltool
    {
        static readonly List<string> _null_list = new List<string>();
        static readonly Dictionary<string, string> _null_dict = new Dictionary<string, string>();
        public Dictionary<string, object> Values = new Dictionary<string, object>();
        public List<string> fields = new List<string>();
        public List<string> needs;
        Dictionary<string, string> flags;

        // flags :
        //  * - 必要欄位
        //  N - nvarchar
        //  d - 日期
        //  t - 時間

        public object this[string flag, string field, string fill]
        {
            set
            {
                flag = flag.Replace(" ", "");
                if (!string.IsNullOrEmpty(flag))
                {
                    if (flag.Contains('*'))
                    {
                        if (value == null)
                        {
                            if (this.needs == null)
                                this.needs = new List<string>();
                            if (!this.needs.Contains(field))
                                this.needs.Add(field);
                        }
                    }
                    if (this.flags == null)
                        this.flags = new Dictionary<string, string>();
                    this.flags[field] = flag;
                }
                //foreach (char f in flags)
                //{
                //    if (f == '*')
                //    {
                //        if (value == null)
                //        {
                //            if (this.needs == null)
                //                this.needs = new List<string>();
                //            if (!this.needs.Contains(field))
                //                this.needs.Add(field);
                //        }
                //    }
                //    else if (f == 'N')
                //    {
                //        if (this.nvarchar == null)
                //            this.nvarchar = new List<string>();
                //        if (!this.nvarchar.Contains(field))
                //            this.nvarchar.Add(field);
                //    }
                //}

                if (value != null)
                {
                    if (!this.fields.Contains(field))
                        this.fields.Add(field);
                    if ((value is CurrencyCode) || (value is CurrencyCode?))
                        this.Values[field] = Convert.ToString(value);
                    else
                        this.Values[field] = value;
                }
            }
        }

        public object this[string flag, string field, string fill1, object oldValue, string fill2]
        {
            set
            {
                if (object.Equals(value, oldValue)) return;
                //if (value == null)
                //{ if (oldValue == null) return; }
                //else
                //{ if (value.Equals(oldValue)) return; }
                this[flag, field, fill1] = value;
            }
        }


        /// <summary>
        /// [Field1],[Field2],[Field3]
        /// </summary>
        public const char _Fields = '1';
        /// <summary>
        /// {Field1},{Field2},{Field3}
        /// </summary>
        public const char _Values = '2';
        /// <summary>
        /// @Field1={Field1},@Field2={Field2},@Field3={Field3}
        /// </summary>
        public const char _AtFieldValue = '3';
        /// <summary>
        /// [Field1]={Field1},[Field2]={Field2},[Field3]={Field3}
        /// </summary>
        public const char _FieldValue = '4';

        public string Build(params object[] args)
        {
            StringBuilder sql = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string)
                    sql.Append(args[i]);
                else if (args[i] is char)
                {
                    Dictionary<string, string> flags = this.flags ?? _null_dict;
                    char op = (char)args[i];
                    bool f, at, eq, v;

                    if (op == _Fields)
                    { f = true; eq = false; v = false; at = false; }
                    else if (op == _Values)
                    { f = false; eq = false; v = true; at = false; }
                    else if (op == _AtFieldValue)
                    { f = v = eq = true; at = true; }
                    else if (op == _FieldValue)
                    { f = v = eq = true; at = false; }
                    else continue;

                    for (int index = 0; index < this.fields.Count; index++)
                    {
                        string field = this.fields[index];
                        string flag;
                        if (!flags.TryGetValue(field, out flag))
                            flag = string.Empty;
                        if (index > 0) sql.Append(',');
                        if (f)
                        {
                            if (at)
                            {
                                sql.Append('@');
                                sql.Append(field);
                            }
                            else
                            {
                                sql.Append('[');
                                sql.Append(field);
                                sql.Append(']');
                            }
                        }
                        if (eq) sql.Append('=');
                        if (v)
                        {
                            if (flag.Contains('N'))
                                sql.Append('N');
                            sql.Append('{');
                            sql.Append(field);
                            object value;
                            if (Values.TryGetValue(field, out value))
                            {
                                if (value is DateTime)
                                {
                                    sql.Append(':');
                                    if (flag.Contains('d'))
                                        sql.Append(DateFormat);
                                    else if (flag.Contains('t'))
                                        sql.Append(TimeFormat);
                                    else
                                        sql.Append(DateTimeFormat);
                                }
                            }
                            sql.Append('}');
                        }
                    }
                }
            }
            return sql.ToString();
        }

        public const string CreateTime = "CreateTime";
        public const string ModifyTime = "ModifyTime";
        public const string CreateUser = "CreateUser";
        public const string ModifyUser = "ModifyUser";
        public void SetUser(params string[] fields)
        {
            User user = HttpContext.Current.User as User;
            for (int i = 0; i < fields.Length; i++)
            {
                switch (fields[i])
                {
                    case sqltool.CreateTime:
                    case sqltool.ModifyTime: this["", fields[i], ""] = (StringEx.sql_str)"getdate()"; break;
                    case sqltool.CreateUser:
                    case sqltool.ModifyUser: this["", fields[i], ""] = user.ID; break;
                }
            }
        }

        public static string DateFormat = "yyyy-MM-dd";
        public static string TimeFormat = "HH:mm:ss";
        public static string DateTimeFormat = DateFormat + " " + TimeFormat;

        public string SqlExport(string pattern)
        {
            return pattern.SqlExport(null, this.Values);
        }

        //public static jgrid.RowResponse sql_execute<TRow>(SqlCmd sqlcmd, string sqlstr) where TRow : new()
        //{
        //    bool commit = true;
        ////    return sql_tool2.sql_execute2<TRow>(sqlcmd, sqlstr, true);
        ////}
        ////public static jgrid.RowResponse sql_execute2<TRow>(SqlCmd sqlcmd, string sqlstr, bool commit) where TRow : new()
        ////{
        //    try
        //    {
        //        TRow row = default(TRow);
        //        sqlcmd.BeginTransaction();
        //        foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
        //        {
        //            int? err = r.GetInt32N("err");
        //            if (err.HasValue)
        //                return new jgrid.RowResponse() { sp_errCode = err, sp_msg = r.GetStringN("msg") };
        //            row = r.ToObject<TRow>();
        //            break;
        //            //log.message("debug", r.Dump());
        //        }
        //        if (row == null)
        //            return new jgrid.RowResponse().Error(null);
        //        (commit ? (ThreadStart)sqlcmd.Commit : (ThreadStart)sqlcmd.Rollback)();
        //        return new jgrid.RowResponse().Success(row);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.message("error", ex.Message);
        //        sqlcmd.Rollback();
        //        return new jgrid.RowResponse().Error(ex);
        //    }
        //}

        //public static T sql_getrow<T>(SqlCmd sqlcmd, string format, params object[] key) where T : new()
        //{
        //    for (int i = 0; i < key.Length; i++)
        //        if (key[i] == null)
        //            return default(T);
        //    return sqlcmd.ToObject<T>(string.Format(format, key));
        //}

        public static int? sql_getrows_count(SqlCmd sqlcmd, string format, params object[] key)
        {
            for (int i = 0; i < key.Length; i++)
                if (key[i] == null)
                    return null;
            return sqlcmd.ExecuteScalar<int>(format, key);
        }
    }

    public class jgrid
    {
        // use for jqGrid.beforeProcessing
        // generate from jqGrid.serializeGridData
        public class GridRequest
        {
            [JsonProperty]
            public bool? _search;
            [JsonProperty]
            public long? nd;
            [JsonProperty]
            public int? rows;
            [JsonProperty]
            public int? page;
            [JsonProperty]
            public string sidx;
            [JsonProperty]
            public string sord;
        }

        public class GridResponse<T> : Dictionary<string, object>
        {
            public GridResponse() { this.rows = new List<T>(); }

            public void Enums(string name, object value)
            {
                this.getField<Dictionary<string, object>>("Enums")[name] = value;
            }

            public void ColumnVisible(string colName, bool show)
            {
                this.getField<Dictionary<string, bool>>("colVisible")[colName] = show;
            }

            T getField<T>(string key) where T : class,new()
            {
                if (this.ContainsKey(key))
                    if (this[key] is T)
                        return (T)this[key];
                T value = new T();
                this[key] = value;
                return value;
            }

            /// <summary>
            /// total pages for the query
            /// </summary>
            public int? total
            {
                get { object o; this.TryGetValue("total", out o); return o as int?; }
                set { this["total"] = value; }
            }

            /// <summary>
            /// current page of the query
            /// </summary>
            public int? page
            {
                get { object o; this.TryGetValue("page", out o); return o as int?; }
                set { this["total"] = value; }
            }

            /// <summary>
            /// total number of records for the query
            /// </summary>
            public int? records
            {
                get { object o; this.TryGetValue("records", out o); return o as int?; }
                set { this["records"] = value; }
            }

            /// <summary>
            /// an array that contains the actual data
            /// </summary>
            public List<T> rows
            {
                get { object o; this.TryGetValue("rows", out o); return o as List<T>; }
                set { this["rows"] = value; }
            }

            /// <summary>
            /// the unique id of the row
            /// </summary>
            public int? id
            {
                get { object o; this.TryGetValue("id", out o); return o as int?; }
                set { this["id"] = value; }
            }

            /// <summary>
            /// an array that contains the data for a row
            /// </summary>
            public object cell
            {
                get { object o; this.TryGetValue("cell", out o); return o; }
                set { this["cell"] = value; }
            }

            //public object AdjustData()
            //{
            //    if (rows != null)
            //    {
            //        foreach (object row in rows)
            //            if (row is jqgridResponse)
            //                ((jqgridResponse)row).AdjustData();
            //    }
            //    return this;
            //}
        }

        public class RowRequest<TRow> where TRow : new()
        {
            [JsonProperty]
            string oper;

            [ObjectInvoke, api.Async]
            static object execute(RowRequest<TRow> command, string json_s, params object[] args)
            {
                return ObjectInvoke.CallByName(command.oper, command, command, json_s, args);
            }

            public static TRow GetRow(SqlCmd sqlcmd, string format, params object[] key)
            {
                for (int i = 0; i < key.Length; i++)
                    if (key[i] == null)
                        return default(TRow);
                return sqlcmd.ToObject<TRow>(string.Format(format, key));
            }

            public static RowResponse sql_execute(SqlCmd sqlcmd, string sqlstr)
            {
                bool commit = true;
                try
                {
                    TRow row = default(TRow);
                    sqlcmd.BeginTransaction();
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
                    {
                        row = r.ToObject<TRow>();
                        break;
                        //log.message("debug", r.Dump());
                    }
                    if (row == null)
                        return new RowResponse().Error(null);
                    (commit ? (ThreadStart)sqlcmd.Commit : (ThreadStart)sqlcmd.Rollback)();
                    return new RowResponse(row);
                }
                catch (Exception ex)
                {
                    log.message("error", ex.Message);
                    sqlcmd.Rollback();
                    return new RowResponse().Error(ex);
                }
            }

            [JsonConverter(typeof(RowResponseJsonConverter))]
            public class RowResponse : RowResponseBase
            {
                // success = true  : [ true , row]
                // success = false : [ false, msg, args ]

                public bool IsSuccess;
                public TRow row;
                public string msg;
                public object[] args;

                public RowResponse()
                {
                    this.IsSuccess = false;
                }
                public RowResponse(TRow row)
                {
                    this.IsSuccess = true;
                    this.row = row;
                }
                public RowResponse(string msg, params object[] args)
                {
                    this.IsSuccess = false;
                    this.msg = msg;
                    this.args = args;
                }

                public RowResponse SetMsg(string msg, params object[] args)
                {
                    this.IsSuccess = false;
                    this.row = default(TRow);
                    this.msg = msg;
                    this.args = args;
                    return this;
                }

                public RowResponse Success(TRow row)
                {
                    this.IsSuccess = true;
                    this.row = row;
                    return this;
                }

                public RowResponse Error(Exception ex)
                {
                    string msg = "Unknown Error";
                    if (ex != null)
                        msg = ex.Message;
                    return this.SetMsg("UpdateError", msg);
                }

                /// <summary>
                /// 缺少必要的欄位
                /// </summary>
                public RowResponse FieldNeeds(List<string> fields)
                {
                    return this.SetMsg("FieldNeeds", fields.ToArray());
                }

                /// <summary>
                /// 更新失敗, 找不到資料列
                /// </summary>
                public RowResponse UpdateMissing()
                {
                    return this.SetMsg("UpdateMissing");
                }

                /// <summary>
                /// 資料沒有變化, 不執行 update
                /// </summary>
                public RowResponse UpdateIgnore(TRow row)
                {
                    return this.Success(row);
                    //return this.SetMsg("UpdateIgnore");
                }

                /// <summary>
                /// 無法插入重複的索引鍵
                /// </summary>
                public RowResponse AlreadyExist(params object[] args)
                {
                    return this.SetMsg("AlreadyExist", args);
                }

                /// <summary>
                /// 
                /// </summary>
                public RowResponse NotExist(params object[] args)
                {
                    return this.SetMsg("NotExist", args);
                }

                /// <summary>
                /// 輸入參數錯誤
                /// </summary>
                public RowResponse InvaildParam(params object[] args)
                {
                    return this.SetMsg("InvaildParam", args);
                }

                public override object[] SerializeData()
                {
                    return new object[] { this.IsSuccess, this.IsSuccess ? (object)row : msg, this.args };
                }
            }
        }

        public abstract class RowResponseBase
        {
            public abstract object[] SerializeData();
        }

        class RowResponseJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                RowResponseBase obj = value as RowResponseBase;
                if (obj != null)
                    value = obj.SerializeData();
                serializer.Serialize(writer, value);
            }
        }

        //public class RowResponse : List<object>
        //{
        //    public bool IsSuccess;
        //    public object row;
        //    public string msg;
        //    public object args;
        //    public int? sp_errCode;
        //    public string sp_msg;

        //    public RowResponse SetMsg(params object[] args)
        //    {
        //        this.AddRange(args);
        //        return this;
        //    }

        //    public RowResponse Success(object row)
        //    {
        //        return this.SetMsg(true, row);
        //    }

        //    public RowResponse Error(Exception ex)
        //    {
        //        string msg = "Unknown Error";
        //        if (ex != null)
        //            msg = ex.Message;
        //        return this.SetMsg(false, "UpdateError", msg);
        //    }

        //    /// <summary>
        //    /// 缺少必要的欄位
        //    /// </summary>
        //    public RowResponse FieldNeeds(List<string> fields)
        //    {
        //        return this.SetMsg(false, "FieldNeeds", fields);
        //    }

        //    /// <summary>
        //    /// 更新失敗, 找不到資料列
        //    /// </summary>
        //    public RowResponse UpdateMissing()
        //    {
        //        return this.SetMsg(false, "UpdateMissing");
        //    }

        //    /// <summary>
        //    /// 資料沒有變化, 不執行 update
        //    /// </summary>
        //    public RowResponse UpdateIgnore(object row)
        //    {
        //        return this.SetMsg(false, "UpdateIgnore");
        //    }

        //    /// <summary>
        //    /// 無法插入重複的索引鍵
        //    /// </summary>
        //    public RowResponse AlreadyExist(params object[] args)
        //    {
        //        return this.SetMsg(false, "AlreadyExist", args);
        //    }

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public RowResponse NotExist(params object[] args)
        //    {
        //        return this.SetMsg(false, "NotExist", args);
        //    }

        //    /// <summary>
        //    /// 輸入參數錯誤
        //    /// </summary>
        //    public RowResponse InvaildParam(params object[] args)
        //    {
        //        return this.SetMsg(false, "InvaildParam", args);
        //    }
        //}
    }

    //static class text
    //{
        //public static string GetChange(string s1, string s2)
        //{
        //    //EqualityComparer<string>.
        //    if (s1 == s2)
        //        return null;
        //    return s2;
        //}

        //public static T GetChange<T>(T s1, T s2)
        //{
        //    if (EqualityComparer<T>.Default.Equals(s1, s2))
        //        return default(T);
        //    return s2;
        //}

        //public static bool AddValue(this StringBuilder sql, string prefix, string field, string s1, string s2)
        //{
        //    if (s1 == s2)
        //        return true;
        //    return sql.AddValue(prefix, field, s2);
        //}

        //public static bool AddValue<T>(this StringBuilder sql, string prefix, string field, T s1, T s2)
        //{
        //    if (EqualityComparer<T>.Default.Equals(s1, s2))
        //        return true;
        //    return sql.AddValue(prefix, field, s2);
        //}

        //public static bool AddValue(this StringBuilder sql, string prefix, string field, object value)
        //{
        //    if (value == null)
        //        return false;
        //    if (value is string)
        //    {
        //        if (value.Equals(string.Empty))
        //            return false;
        //        sql.AppendFormat(",{0}{1}='{2}'", prefix, field, value);
        //    }
        //    else if ((value is DateTime) || (value is DateTime?))
        //        sql.AppendFormat(",{0}{1}='{2:yyyy-MM-dd HH:mm:ss}'", prefix, field, value);
        //    else if (value.GetType().IsEnum)
        //        sql.AppendFormat(",{0}{1}='{2}'", prefix, field, value);
        //    else
        //        sql.AppendFormat(",{0}{1}={2}", prefix, field, value);
        //    return true;
        //}

        //public static string ReplaceQuote(this string s)
        //{
        //    if (s != null)
        //        return s.Replace("'", "''");
        //    return s;
        //}


        //public static string ValidACNT(string acnt)
        //{
        //    if (acnt == null) return null;
        //    acnt = acnt.Trim().ReplaceQuote();
        //    if (acnt == string.Empty)
        //        return null;
        //    return acnt.ToLower();
        //}


        //public static byte? ToLockedValue(this string s)
        //{
        //    if (s != null)
        //    {
        //        if (Regex.IsMatch(s, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
        //            return 0;
        //        if (Regex.IsMatch(s, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
        //            return 1;
        //    }
        //    return null;
        //}




        //public static bool SplitMailAddress(this jstring s, out jstring acnt, out jstring corp)
        //{
        //    if ((s != null) && (s.Value != null))
        //    {
        //        int n = s.Value.IndexOf('@');
        //        if (n >= 0)
        //        {
        //            acnt = jstring.Create(s.Value.Substring(0, n));
        //            corp = jstring.Create(s.Value.Substring(n + 1));
        //        }
        //        else
        //        {
        //            acnt = s;
        //            corp = jstring.Null;
        //        }
        //        return true;
        //    }
        //    acnt = corp = null;
        //    return false;
        //}



        //public static int? ToID(this jstring s)
        //{
        //    if (s == null) return null;
        //    return s.Value.ToInt32();
        //}


        //public static string ToACNT(this jstring s)
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return null;
        //    return s.Value.ToLower().replace_quote();
        //}

        //public static string ToName(this jstring s)
        //{
        //    return s.ToStr();
        //}

        //public static string GetValue(this jstring s)
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return null;
        //    return s.Value;
        //}

        //public static string ToPassword(this jstring s, string acnt, string _default)
        //{
        //    string n;
        //    if (s == null)
        //        n = _default;
        //    else if (string.IsNullOrEmpty(s.Value))
        //        n = _default;
        //    else
        //        n = s.Value;
        //    if (n == null) return null;
        //    return Convert.ToBase64String(n.TripleDESEncrypt(acnt, null).MD5());
        //}

        //public static byte? ToLocked(this jstring s)
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return null;
        //    if (Regex.IsMatch(s.Value, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
        //        return 0;
        //    if (Regex.IsMatch(s.Value, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
        //        return 1;
        //    return null;
        //}

        //public static T? ToEnum<T>(this jstring s, T? _default) where T : struct 
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return _default;
        //    return s.Value.ToEnum<T>() ?? _default;
        //}
        //public static string ToEnumString<T>(this jstring s, T? _default) where T : struct 
        //{
        //    T? n = s.ToEnum<T>(_default);
        //    if (n.HasValue)
        //        return n.Value.ToString();
        //    return null;
        //}

        /// <summary>
        /// 如果 user 的 CorpID=0, 則解析 CorpID, 否則傳回 User.CorpID
        /// </summary>
        /// <param name="s"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        //public static int? ToCorpID(this jstring s, SqlCmd sqlcmd, User user)
        //{
        //    if (user.CorpID == 0)
        //        return s.ToCorpID(sqlcmd);
        //    else
        //        return user.CorpID;
        //}
        //public static int? ToCorpID(this jstring s, SqlCmd sqlcmd)
        //{
        //    if (s == null) return null;
        //    return CorpList.GetInstance(null, sqlcmd).GetCorpID(s.Value);
        //}

        //public static string ToStr(this jstring s)
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return null;
        //    return s.Value.replace_quote();
        //}

        //public static string ValueOrDefault(string value, string _default)
        //{
        //    if (value == null)
        //        return _default;
        //    if (value.Trim() == string.Empty)
        //        return _default;
        //    return value;
        //}



        //public const string exp_ACNT = @"^([a-z]|[A-Z])[\w_]{3,20}$";

        //public static StringBuilder AppendFormatN(this StringBuilder sb, string format, object arg0)
        //{
        //    return sb.AppendFormat(format, arg0);
        //}

        //log.message(null, "{0}", Regex.IsMatch(this.acnt, exp_acnt));
        //string exp_acnt = @"^^{3,20}[a-zA-Z0-9]*$";
        //bool x = Regex.IsMatch(command.mail, ext_mail);
        //public static string operator *(string s1, Member_Update s2)
        //{
        //    s1 = (s1 ?? "").Trim();
        //    return string.IsNullOrEmpty(s1) ? null : s1;
        //}
        //public static bool ValidACNT()
        //{
        //    string ext_mail = @"^([a-zA-Z0-9._-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+";
        //    string exp_111 = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        //}
    //}


    //class CorpIDJsonConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        serializer.Serialize(writer, CorpList.Instance.GetCorpACNT(value as int?));
    //    }
    //}
    //[JsonConverter(typeof(jstringConverter))]
    //[DebuggerDisplay("{Value}")]
    //public class jstring
    //{
    //    public string OriginalString { get; private set; }
    //    public string Value { get; private set; }
    //    public bool IsNull
    //    {
    //        get { return this.Value == null; }
    //    }

    //    public static jstring Create(object obj)
    //    {
    //        string str1 = obj as string;
    //        if (str1 == null)
    //            return jstring.Null;
    //        string str2 = str1.Trim();
    //        //if (str2 == string.Empty)
    //        //    str2 = null;
    //        return new jstring() { OriginalString = str1, Value = str2, };
    //    }

    //    public override string ToString()
    //    {
    //        if (Value == null) return null;
    //        return Value.ToString();
    //    }

    //    public static jstring Null = new jstring();
    //}

    //class jstringConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType) { throw new NotImplementedException(); }
    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { return jstring.Create(reader.Value); }
    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { throw new NotImplementedException(); }
    //}
    //public class sql_tool
    //{
    //    public Dictionary<string, string> src;
    //    public Dictionary<string, object> dst;
    //    public sql_tool(string jsonString)
    //    {
    //        this.src = Tools.Protocol.JsonProtocol.DeserializeObject<Dictionary<string, string>>(jsonString);
    //        this.dst = new Dictionary<string, object>();
    //    }

    //    public IEnumerable<object> check_args(string rowid, params string[] dstkeys)
    //    {
    //        List<string> fields = null;
    //        foreach (string dstkey in dstkeys)
    //        {
    //            if (!dst.ContainsKey(dstkey))
    //            {
    //                if (fields == null)
    //                    fields = new List<string>();
    //                fields.Add(dstkey);
    //            }
    //        }
    //        if (fields != null)
    //            yield return jgrid.RowResponse.FieldNeeds(rowid, fields);
    //    }

    //    public string[] fields_ignore = new string[0];

    //    public void field(StringBuilder s, string prefix, string l, string f, string r)
    //    {
    //        s.Append(prefix);
    //        s.Append(l);
    //        s.Append(f);
    //        s.Append(r);
    //    }

    //    public void fields(StringBuilder s, string l, string r)
    //    {
    //        string comma = "";
    //        foreach (string f in this.dst.Keys)
    //        {
    //            if (fields_ignore.Conatins(f)) continue;
    //            s.Append(comma);
    //            field(s, null, l, f, r);
    //            comma = ",";
    //        }
    //    }

    //    public void keyvalues(StringBuilder s, string prefix, string l, string r)
    //    {
    //        string comma = "";
    //        foreach (string f in this.dst.Keys)
    //        {
    //            if (fields_ignore.Conatins(f)) continue;
    //            s.Append(comma);
    //            field(s, prefix, l, f, r);
    //            field(s, "=", "{", f, "}");
    //            comma = ",";
    //        }
    //        //sql.AppendFormat("{1}{0}={{{0}}}", n1.Current, field_prefix);
    //    }

    //    /// <summary>
    //    /// [Field1],[Field2],[Field3]
    //    /// </summary>
    //    public static object _Fields = sql_tool2._Fields;
    //    /// <summary>
    //    /// {Field1},{Field2},{Field3}
    //    /// </summary>
    //    public static object _Values = sql_tool2._Values;
    //    /// <summary>
    //    /// @Field1={Field1},@Field2={Field2},@Field3={Field3}
    //    /// </summary>
    //    public static object _AtFieldValue = sql_tool2._AtFieldValue;
    //    /// <summary>
    //    /// [Field1]={Field1},[Field2]={Field2},[Field3]={Field3}
    //    /// </summary>
    //    public static object _FieldValue = sql_tool2._FieldValue;

    //    public string sql_build(params object[] args)
    //    {
    //        StringBuilder sql = new StringBuilder();
    //        for (int i = 0; i < args.Length; i++)
    //        {
    //            object arg = args[i];
    //            if (arg is string)
    //                sql.Append(arg);
    //            else if (arg.Equals(_Fields))
    //                fields(sql, "[", "]");
    //            else if (arg.Equals(_Values))
    //                fields(sql, "{", "}");
    //            else if (arg.Equals(_AtFieldValue))
    //                keyvalues(sql, "@", null, null);
    //            else if (arg.Equals(_FieldValue))
    //                keyvalues(sql, null, "[", "]");
    //        }
    //        return sql.ToString();
    //    }

    //    //public string sql_str(string s1, string s2)
    //    //{
    //    //    return sql_str(s1, s2, "");
    //    //}
    //    //public string sql_str(string s1, string s2, string field_prefix)
    //    //{
    //    //    StringBuilder sql = new StringBuilder();
    //    //    sql.Append(s1);
    //    //    IEnumerator n1 = this.dst.Keys.GetEnumerator();
    //    //    for (bool n2 = n1.MoveNext(); n2; )
    //    //    {
    //    //        sql.Append(field_prefix);
    //    //        sql.AppendFormat("{0}={{{0}}}", n1.Current);
    //    //        if (n1.MoveNext())
    //    //            sql.Append(',');
    //    //    }
    //    //    sql.Append(s2);
    //    //    return sql.ToString();
    //    //}

    //    public object sql_execute<TRow>(SqlCmd sqlcmd, string rowid, string sqlstr) where TRow : new()
    //    {
    //        try
    //        {
    //            TRow row = default(TRow);
    //            sqlcmd.BeginTransaction();
    //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
    //            {
    //                //int? err = r.GetInt32N("err");
    //                row = r.ToObject<TRow>();
    //                //log.message("debug", r.Dump());
    //            }
    //            if (row == null)
    //                return jgrid.RowResponse.Error(rowid, null);
    //            sqlcmd.Commit();
    //            return jgrid.RowResponse.Success(rowid, row);
    //        }
    //        catch (Exception ex)
    //        {
    //            log.message("error", ex.Message);
    //            sqlcmd.Rollback();
    //            return jgrid.RowResponse.Error(rowid, ex);
    //        }
    //    }

    //    public T sql_GetRow<T>(SqlCmd sqlcmd, string sqltext, string key_id) where T : new()
    //    {
    //        int? id = this.Int32(key_id, null, null);
    //        if (id.HasValue)
    //            return sqlcmd.ToObject<T>(string.Format(sqltext, id.Value));
    //        return default(T);
    //    }

    //    public bool? isUpdate;

    //    public T? _Value<T>(string srckey, string dstkey, T? value2, StringEx.GetValueHandler<T> getValue) where T : struct
    //    {
    //        dstkey = dstkey ?? srckey;
    //        string s;
    //        src.TryGetValue(srckey, out s);
    //        T? value;
    //        bool setnull = s == "null" || s == "undefined";
    //        if (setnull)
    //            value = null;
    //        else
    //            value = getValue(s);
    //        if (isUpdate.HasValue)
    //        {
    //            if (isUpdate.Value)
    //            {
    //                if ((value.HasValue && (!EqualityComparer<T?>.Default.Equals(value, value2))) || (value2.HasValue && setnull))
    //                    dst[dstkey] = value;
    //            }
    //            else
    //            {
    //                value = value ?? value2;
    //                if (value.HasValue)
    //                    dst[dstkey] = value;
    //            }
    //        }
    //        return value;
    //    }

    //    public void NullField(params string[] fields)
    //    {
    //        foreach (string f in fields)
    //            if (!dst.ContainsKey(f))
    //                dst[f] = null;
    //    }

    //    public int? Int32(string srckey, string dstkey, int? value2)
    //    {
    //        return _Value<Int32>(srckey, dstkey, value2, StringEx.ToInt32);
    //    }

    //    public DateTime? DateTime(string srckey, string dstkey, DateTime? value2)
    //    {
    //        return _Value<DateTime>(srckey, dstkey, value2, StringEx.ToDateTime);
    //    }

    //    public byte? Locked(string srckey, string dstkey, byte? value2)
    //    {
    //        return _Value<byte>(srckey, dstkey, value2, text.ToLockedValue);
    //    }

    //    public decimal? Decimal(string srckey, string dstkey, decimal? value2)
    //    {
    //        return _Value<Decimal>(srckey, dstkey, value2, StringEx.ToDecimal);
    //    }

    //    public T? Enum<T>(string srckey, string dstkey, T? value2) where T : struct
    //    {
    //        return _Value<T>(srckey, dstkey, value2, StringEx.ToEnum<T>);
    //    }

    //    public string EnumString<T>(string srckey, string dstkey, T? value2) where T : struct
    //    {
    //        T? value = _Value<T>(srckey, dstkey, value2, StringEx.ToEnum<T>);
    //        dstkey = dstkey ?? srckey;
    //        string str;
    //        if (value.HasValue)
    //        {
    //            str = value.Value.ToString();
    //            if (dst.ContainsKey(dstkey))
    //                dst[dstkey] = str;
    //        }
    //        else
    //        {
    //            str = null;
    //            if (dst.ContainsKey(dstkey))
    //                dst.Remove(dstkey);
    //        }
    //        return str;
    //    }

    //    public string String(string srckey, string dstkey, string value2)
    //    {
    //        dstkey = dstkey ?? srckey;
    //        string s;
    //        string value;
    //        if (src.TryGetValue(srckey, out s))
    //        {
    //            value = s.Trim().ReplaceQuote();
    //            if (string.IsNullOrEmpty(value))
    //                value = null;
    //        }
    //        else
    //            value = null;
    //        if (isUpdate.HasValue)
    //        {
    //            if (isUpdate.Value)
    //            {
    //                if ((value != null) && (value != value2))
    //                    dst[dstkey] = value;
    //            }
    //            else
    //            {
    //                value = value ?? value2;
    //                if (value != null)
    //                    dst[dstkey] = value;
    //            }
    //        }
    //        return value;
    //    }

    //    public string Name(string srckey, string dstkey, string value2)
    //    {
    //        return String(srckey, dstkey, value2);
    //    }

    //    public string ACNT(string srckey, string dstkey, string value2)
    //    {
    //        return text.ValidACNT(String(srckey, dstkey, value2));
    //    }

    //    public string Password(string srckey, string dstkey, string acnt, string value2)
    //    {
    //        if (acnt == null) return null;
    //        dstkey = dstkey ?? srckey;
    //        string s;
    //        if (src.TryGetValue(srckey, out s))
    //        {
    //            s = s.Trim();
    //            if (string.IsNullOrEmpty(s))
    //                s = null;
    //        }
    //        else
    //            s = null;
    //        if (isUpdate.HasValue)
    //        {
    //            if (isUpdate.Value)
    //            {
    //                if (s != null)
    //                {
    //                    string p = text.EncodePassword(acnt, s);
    //                    if (p != value2)
    //                        dst[dstkey] = p;
    //                }
    //            }
    //            else
    //            {
    //                s = s ?? value2;
    //                if (s != null)
    //                    dst[dstkey] = text.EncodePassword(acnt, s);
    //            }
    //        }
    //        return s;
    //    }


    //    //public string _String(string srckey, string dstkey, string _defaultValue, string _oldValue)
    //    //{
    //    //    dstkey = dstkey ?? srckey;
    //    //    string s;
    //    //    string value;
    //    //    if (src.TryGetValue(srckey, out s))
    //    //    {
    //    //        s = s.Trim().ReplaceQuote();
    //    //        if (string.IsNullOrEmpty(s))
    //    //            s = null;
    //    //        value = s ?? _defaultValue;
    //    //    }
    //    //    else
    //    //        value = _defaultValue;
    //    //    if (value != null)
    //    //    {
    //    //        if (value != _oldValue)
    //    //            dst[dstkey] = value;
    //    //    }
    //    //    return value;
    //    //}

    //    //public string _Name(string srckey, string dstkey, string _default, string _oldValue)
    //    //{
    //    //    return _String(srckey, dstkey, _default, _oldValue);
    //    //}

    //    //public byte? _Locked(string srckey, string dstkey, byte? _defaultValue, byte? _oldValue)
    //    //{
    //    //    dstkey = dstkey ?? srckey;
    //    //    string s;
    //    //    byte? value;
    //    //    if (src.TryGetValue(srckey, out s))
    //    //    {
    //    //        if (Regex.IsMatch(s, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
    //    //            value = 0;
    //    //        else if (Regex.IsMatch(s, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
    //    //            value = 1;
    //    //        else
    //    //            value = _defaultValue;
    //    //    }
    //    //    else
    //    //        value = _defaultValue;
    //    //    if (value.HasValue)
    //    //    {
    //    //        if (value != _oldValue)
    //    //            dst[dstkey] = value;
    //    //    }
    //    //    return value;
    //    //}
    //}
    //public static class fields
    //{

    //    //public static fields fromjson(string json)
    //    //{
    //    //    return Tools.Protocol.JsonProtocol.Deserialize<fields>(json);
    //    //}

    //    //public bool getACNT(string key, out string acnt)
    //    //{
    //    //    if (this.TryGetValue(key, out acnt))
    //    //        acnt = text.ValidACNT(acnt);
    //    //    else
    //    //        acnt = null;
    //    //    return acnt != null;
    //    //}
    //    //public string getACNT(string key)
    //    //{
    //    //    string acnt;
    //    //    this.TryGetValue(key, out acnt);
    //    //    return text.ValidACNT(acnt);
    //    //}

    //    //public bool getString(string key, out string str)
    //    //{
    //    //    string s;
    //    //    if (this.TryGetValue(key, out s))
    //    //    {
    //    //        str = s.Trim().ReplaceQuote();
    //    //        if (!string.IsNullOrEmpty(str))
    //    //            return true;
    //    //    }
    //    //    str = null;
    //    //    return false;
    //    //}
    //    //public string str(string key)
    //    //{
    //    //    string s;
    //    //    this.getString(key, out s);
    //    //    return s;
    //    //}
    //    //public string getName(string key)
    //    //{
    //    //    return this.str(key);
    //    //}

    //    //public string getPassword(string key, string acnt, string _default)
    //    //{
    //    //    string s;
    //    //    if (this.TryGetValue(key, out s))
    //    //        s = text.EncodePassword(acnt, s);
    //    //    if (string.IsNullOrEmpty(s))
    //    //        return text.EncodePassword(acnt, _default);
    //    //    return s;
    //    //}

    //    //public byte? getLocked(string key)
    //    //{
    //    //    string s;
    //    //    if (this.TryGetValue(key, out s))
    //    //    {
    //    //        if (Regex.IsMatch(s, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
    //    //            return 0;
    //    //        if (Regex.IsMatch(s, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
    //    //            return 1;
    //    //    }
    //    //    return null;
    //    //}

    //    public const string ID_ = "ID_";
    //    public const string CorpID = "CorpID";
    //    public const string ACNT = "ACNT";
    //    public const string ParentACNT = "ParentACNT";
    //    public const string Name = "Name";
    //    public const string Password = "Password";
    //    public const string pwd = "pwd";
    //    public const string Locked = "Locked";
    //    public const string Currency = "Currency";
    //    public const string BonusW = "BonusW";
    //    public const string BonusL = "BonusL";
    //    public const string AdminACNT = "AdminACNT";
    //    public const string AgentACNT = "AgentACNT";
    //    public const string MaxUser = "MaxUser";
    //    public const string MaxAgent = "MaxAgent";
    //    public const string MaxDepth = "MaxDepth";
    //    public const string ModifyTime = "ModifyTime";
    //    public const string CreateUser = "CreateUser";
    //    public const string ModifyUser = "ModifyUser";
    //    public const string ModifyUserType = "ModifyUserType";
    //    public const string MemberType = "MemberType";
    //    public const string Birthday = "Birthday";
    //    public const string Tel = "Tel";
    //    public const string Mail = "Mail";
    //    public const string QQ = "QQ";
    //    public const string RegisterIP = "RegisterIP";
    //    public const string SecPassword = "SecPassword";
    //    public const string sec_pwd = "sec_pwd";
    //    public const string WebATM = "WebATM";
    //    public const string Location = "Location";
    //    public const string CardID = "CardID";
    //    public const string BankID = "BankID";
    //    public const string GroupID = "GroupID";
    //    public const string Sort = "Sort";
    //    public const string Text = "Text";
    //    public const string Place = "Place";
    //    public const string MemberID = "MemberID";
    //    public const string MemberACNT = "MemberACNT";
    //    public const string Amount = "Amount";
    //    public const string Disabled = "Disabled";
    //    public const string RecordType = "RecordType";
    //    public const string ID1 = "ID1";
    //    public const string ID2 = "ID2";
    //}
}