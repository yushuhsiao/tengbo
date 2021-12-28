using BU;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;
using Tools.Protocol;


namespace web 
{
    public static class jgrid
    {
        public static class oper
        {
            public const string insert = "insert";
            public const string update = "update";
            public const string delete = "delete";
        }

        public class colModel
        {
            public colModel() { }
            public colModel(string name) { this.name = name; }

            public string name;
            public string label;
            public int? width;
            public bool? editable;
            [JsonProperty("fixed")]
            public bool? fixed_;
            public bool? hidden;
            public bool? key;

            public override string ToString()
            {
                return api.SerializeObject(this);
            }

            public class ID : colModel
            {
                public ID() : this(null) { }
                public ID(string name)
                {
                    this.name = name;
                    this.width = 50;
                    this.editable = false;
                    this.fixed_ = true;
                    this.hidden = true;
                    this.key = true;
                }
            }
        }

        // use for jqGrid.beforeProcessing
        // generate from jqGrid.serializeGridData
        public class GridRequest
        {
            [JsonProperty]
            public int? CorpID;

            [JsonProperty]
            public bool? _search;
            [JsonProperty]
            public long? nd;
            [JsonProperty("rows")]
            int? _rows;
            public int? page_size
            {
                get { return this._rows ?? 50; }
                set { this._rows = value; }
            }
            [JsonProperty]
            public int? page;

            public bool hasPager
            {
                get { return this.page.HasValue && this.page_size.HasValue; }
            }
            public int? rows_start
            {
                get { return (this.page - 1) * this.page_size; }
            }
            public int? rows_end
            {
                get { return this.page * this.page_size; }
            }

            [JsonProperty("sidx")]
            public string sidx;
            [JsonProperty("sord")]
            public string sord;

            //public string order_by
            //{
            //    get
            //    {
            //        string s = this.sord;
            //        if (s != null)
            //        {
            //            s = s.ToLower();
            //            if ((s != "asc") && (s != "desc"))
            //                s = null;
            //        }
            //        return string.Format("{0} {1}", this.sidx, this.sord ?? "desc");
            //    }
            //}

            //public static string valid_sort(string sord)
            //{
            //    if (sord != null)
            //    {
            //        sord = sord.ToLower();
            //        switch (sord)
            //        {
            //            case "asc":
            //            case "desc": return sord;
            //        }
            //    }
            //    return "desc";
            //}

            public static void sql_where_CorpID(StringBuilder s, ref int count, string format, int? corpID)
            {
                User user = HttpContext.Current.User as User;
                if (user.CorpID == 0)
                    corpID = CorpRow.Cache.Instance.GetCorp(corpID).ID;
                else
                    corpID = user.CorpID;
                sql_where(s, ref count, format, corpID);
            }

            public static void sql_where(StringBuilder s, ref int count, string format, object arg1)
            {
                if (arg1 == null) return;
                if (count == 0)
                    s.Append(" where ");
                else
                    s.Append(" and ");
                s.AppendFormat(format, arg1);
                count++;
            }
        }

        public abstract class GridRequest<T> : GridRequest where T : GridRequest<T>, new()
        {
            //static T instance = new T();
            static string _defaultkey;
            static Dictionary<string, string> _sortkeys;
            protected abstract string init_defaultkey();
            protected abstract Dictionary<string, string> init_sortkeys();

            public string GetOrderBy(Dictionary<string, string> keys, string defaultKey)
            {
                string sidx;
                if (keys.ContainsKey(this.sidx))
                    sidx = keys[this.sidx] ?? this.sidx;
                else
                    sidx = defaultKey;
                if (!string.IsNullOrEmpty(sidx))
                {
                    string sord = this.sord;
                    if (sord != null)
                    {
                        sord = sord.ToLower();
                        if ((sord != "asc") && (sord != "desc"))
                            sord = null;
                    }
                    sord = sord ?? "desc";
                    sidx = string.Format(sidx, sord);
                    return string.Format("{0} {1}", sidx, sord);
                }
                return null;
            }
            
            public string GetOrderBy(string defaultKey)
            {
                Dictionary<string, string> sortkeys;
                lock (typeof(T))
                {
                    _sortkeys = _sortkeys ?? init_sortkeys();
                    sortkeys = _sortkeys;
                }
                return GetOrderBy(sortkeys, defaultKey);
            }

            public string GetOrderBy()
            {
                string defaultKey;
                lock (typeof(T))
                {
                    _defaultkey = _defaultkey ?? init_defaultkey();
                    defaultKey = _defaultkey;
                }
                return GetOrderBy(defaultKey);
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class GridResponse<T>
        {
            public GridResponse() { this.rows = new List<T>(); }

            //public void Enums(string name, object value)
            //{
            //    this.getField<Dictionary<string, object>>("Enums")[name] = value;
            //}

            //public void ColumnVisible(string colName, bool show)
            //{
            //    this.getField<Dictionary<string, bool>>("colVisible")[colName] = show;
            //}

            //T getField<T>(string key) where T : class,new()
            //{
            //    if (this.ContainsKey(key))
            //        if (this[key] is T)
            //            return (T)this[key];
            //    T value = new T();
            //    this[key] = value;
            //    return value;
            //}

            /// <summary>
            /// total pages for the query
            /// </summary>
            [JsonProperty("total")]
            public int? total_pages
            {
                get
                {
                    int? n1 = this.total_rows / this.page_size;
                    int? n2 = this.page_size * n1;
                    if (n1 != n2)
                        n1++;
                    return n1;
                }
            }
            //{
            //    get { object o; this.TryGetValue("total", out o); return o as int?; }
            //    set { this["total"] = value; }
            //}

            public int? page_size;

            public void setPager(int? total_rows, int? rows)
            {
                this.total_rows = total_rows;
                this.page_size = rows;
                //int? total = total_rows / rows;
                //int? remainder = total_rows % rows;
                //if (remainder > 0)
                //    total++;
                //this.total_pages = total;
            }

            /// <summary>
            /// current page of the query
            /// </summary>
            [JsonProperty]
            public int? page;
            //{
            //    get { object o; this.TryGetValue("page", out o); return o as int?; }
            //    set { this["total"] = value; }
            //}

            /// <summary>
            /// total number of records for the query
            /// </summary>
            [JsonProperty("records")]
            public int? total_rows;
            //{
            //    get { object o; this.TryGetValue("records", out o); return o as int?; }
            //    set { this["records"] = value; }
            //}

            /// <summary>
            /// an array that contains the actual data
            /// </summary>
            [JsonProperty]
            public List<T> rows;
            //{
            //    get { object o; this.TryGetValue("rows", out o); return o as List<T>; }
            //    set { this["rows"] = value; }
            //}

            /// <summary>
            /// the unique id of the row
            /// </summary>
            [JsonProperty]
            public int? id;
            //{
            //    get { object o; this.TryGetValue("id", out o); return o as int?; }
            //    set { this["id"] = value; }
            //}

            /// <summary>
            /// an array that contains the data for a row
            /// </summary>
            [JsonProperty]
            public object cell;
            //{
            //    get { object o; this.TryGetValue("cell", out o); return o; }
            //    set { this["cell"] = value; }
            //}

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

            //public IEnumerable<GridUserList> GridUserList(SqlCmd sqlcmd)
            //{
            //    GridUserList userlist = new GridUserList();
            //    yield return userlist;
            //    userlist.GetUserAcnt(sqlcmd);
            //    this["UserList"] = userlist;
            //}
        }

        //public class GridUserList : Dictionary<int, string>
        //{
        //    public void AddUser(params int?[] users)
        //    {
        //        foreach (int? n in users)
        //            if (n.HasValue)
        //                base[n.Value] = null;
        //    }
        //    public void GetUserAcnt(SqlCmd sqlcmd)
        //    {
        //        if (base.Count > 0)
        //        {
        //            StringBuilder sql = new StringBuilder();
        //            sql.Append("select ID,ACNT from [Admin] nolock where ID in (");
        //            int n1 = 0;
        //            foreach (int n2 in base.Keys)
        //            {
        //                if (n1 > 0) sql.Append(",");
        //                sql.Append(n2);
        //                n1++;
        //            }
        //            base.Clear();
        //            sql.Append(")");
        //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.ToString()))
        //                base[r.GetInt32("ID")] = r.GetString("ACNT");
        //        }
        //    }
        //}

        //public class RowRequest<TRow> where TRow : new()
        //{
        //    [JsonProperty]
        //    string oper;

        //    [ObjectInvoke2, api.Async]
        //    static object execute(RowRequest<TRow> command, string json_s, params object[] args)
        //    {
        //        return ObjectInvoke.CallByName(command.oper, command, command, json_s, args);
        //    }

        //    //public static TRow GetRow(SqlCmd sqlcmd, string format, params object[] key)
        //    //{
        //    //    for (int i = 0; i < key.Length; i++)
        //    //        if (key[i] == null)
        //    //            return default(TRow);
        //    //    return sqlcmd.ToObject<TRow>(string.Format(format, key));
        //    //}

        //    public static RowResponse sql_execute(SqlCmd sqlcmd, string sqlstr)
        //    {
        //        try
        //        {
        //            sqlcmd.BeginTransaction();
        //            TRow row = sqlcmd.ToObject<TRow>(sqlstr);
        //            if (row == null)
        //                return new RowResponse().Error(null);
        //            sqlcmd.Commit();
        //            return new RowResponse(row);
        //        }
        //        catch (Exception ex)
        //        {
        //            log.message("error", ex.Message);
        //            sqlcmd.Rollback();
        //            return new RowResponse().Error(ex);
        //        }
        //    }

        //    [JsonConverter(typeof(RowResponseJsonConverter))]
        //    public class RowResponse : RowResponseJsonConverter.IRowResponse
        //    {
        //        // success = true  : [ true , row]
        //        // success = false : [ false, msg, args ]

        //        public bool IsSuccess;
        //        public TRow row;
        //        public string msg;
        //        public object[] args;

        //        public RowResponse()
        //        {
        //            this.IsSuccess = false;
        //        }
        //        public RowResponse(TRow row)
        //        {
        //            this.IsSuccess = true;
        //            this.row = row;
        //        }
        //        public RowResponse(string msg, params object[] args)
        //        {
        //            this.IsSuccess = false;
        //            this.msg = msg;
        //            this.args = args;
        //        }

        //        public RowResponse SetMsg(string msg, params object[] args)
        //        {
        //            this.IsSuccess = false;
        //            this.row = default(TRow);
        //            this.msg = msg;
        //            this.args = args;
        //            return this;
        //        }

        //        public RowResponse Success(TRow row)
        //        {
        //            this.IsSuccess = true;
        //            this.row = row;
        //            return this;
        //        }

        //        public RowResponse Error(Exception ex)
        //        {
        //            string msg = "Unknown Error";
        //            if (ex != null)
        //                msg = ex.Message;
        //            return this.SetMsg("UpdateError", msg);
        //        }

        //        /// <summary>
        //        /// 缺少必要的欄位
        //        /// </summary>
        //        public RowResponse FieldNeeds(List<string> fields)
        //        {
        //            return this.SetMsg("FieldNeeds", fields.ToArray());
        //        }

        //        /// <summary>
        //        /// 更新失敗, 找不到資料列
        //        /// </summary>
        //        public RowResponse UpdateMissing()
        //        {
        //            return this.SetMsg("UpdateMissing");
        //        }

        //        /// <summary>
        //        /// 資料沒有變化, 不執行 update
        //        /// </summary>
        //        public RowResponse UpdateIgnore(TRow row)
        //        {
        //            return this.Success(row);
        //            //return this.SetMsg("UpdateIgnore");
        //        }

        //        /// <summary>
        //        /// 無法插入重複的索引鍵
        //        /// </summary>
        //        public RowResponse AlreadyExist(params object[] args)
        //        {
        //            return this.SetMsg("AlreadyExist", args);
        //        }

        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        public RowResponse NotExist(params object[] args)
        //        {
        //            return this.SetMsg("NotExist", args);
        //        }

        //        /// <summary>
        //        /// 輸入參數錯誤
        //        /// </summary>
        //        public RowResponse InvaildParam(params object[] args)
        //        {
        //            return this.SetMsg("InvaildParam", args);
        //        }

        //        object[] RowResponseJsonConverter.IRowResponse.SerializeData()
        //        {
        //            return new object[] { this.IsSuccess, this.IsSuccess ? (object)row : msg, this.args };
        //        }
        //    }
        //}

        //[JsonConverter(typeof(RowResponse._JsonConverter))]
        //public class RowResponse
        //{
        //    // success = true  : [ true , row]
        //    // success = false : [ false, msg, args ]

        //    class _JsonConverter : JsonConverter
        //    {
        //        public override bool CanConvert(Type objectType)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //        {
        //            throw new NotImplementedException();
        //        }

        //        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //        {
        //            RowResponse obj = value as RowResponse;
        //            if (obj != null)
        //                value = new object[] { obj.IsSuccess, obj.IsSuccess ? (object)obj.row : obj.retmsg, obj.args };
        //            serializer.Serialize(writer, value);
        //        }
        //    }

        //    public bool IsSuccess;
        //    public object row;
        //    public string retmsg;
        //    public object[] args;

        //    public RowResponse()
        //    {
        //        this.IsSuccess = false;
        //    }
        //    public RowResponse(object row)
        //    {
        //        this.IsSuccess = true;
        //        this.row = row;
        //    }
        //    public RowResponse(string retmsg, params object[] args)
        //    {
        //        this.IsSuccess = false;
        //        this.row = null;
        //        this.retmsg = retmsg;
        //        this.args = args;
        //    }
        //    public RowResponse(RowErrorCode err, params object[] args) : this(err.ToString(), args) { }

        //    //public RowResponse SetMsg(string retmsg, params object[] args)
        //    //{
        //    //    this.IsSuccess = false;
        //    //    this.row = default(object);
        //    //    this.retmsg = retmsg;
        //    //    this.args = args;
        //    //    return this;
        //    //}

        //    public static RowResponse Success(object row)
        //    {
        //        return new RowResponse(row);
        //    }

        //    public static RowResponse Error(Exception ex)
        //    {
        //        string msg = "Unknown Error";
        //        if (ex != null)
        //            msg = ex.Message;
        //        if (ex is RowException)
        //            return new RowResponse(((RowException)ex).ErrorCode);
        //        return new RowResponse("UpdateError", msg);
        //    }

        //    public static RowResponse Other(string retmsg, params object[] args)
        //    {
        //        return new RowResponse(retmsg, args);
        //    }

        //    /// <summary>
        //    /// 缺少必要的欄位
        //    /// </summary>
        //    public static RowResponse FieldNeeds(List<string> fields)
        //    {
        //        return new RowResponse(RowErrorCode.FieldNeeds, fields.ToArray());
        //    }

        //    /// <summary>
        //    /// 更新失敗, 找不到資料列
        //    /// </summary>
        //    public static RowResponse UpdateMissing()
        //    {
        //        return new RowResponse("UpdateMissing");
        //    }

        //    /// <summary>
        //    /// 資料沒有變化, 不執行 update
        //    /// </summary>
        //    public static RowResponse UpdateIgnore(object row)
        //    {
        //        return RowResponse.Success(row);
        //        //return new RowResponse("UpdateIgnore", row);
        //    }

        //    /// <summary>
        //    /// 無法插入重複的索引鍵
        //    /// </summary>
        //    public static RowResponse AlreadyExist(params object[] args)
        //    {
        //        return new RowResponse("AlreadyExist", args);
        //    }

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public static RowResponse NotExist(params object[] args)
        //    {
        //        return new RowResponse("NotExist", args);
        //    }

        //    /// <summary>
        //    /// 輸入參數錯誤
        //    /// </summary>
        //    public static RowResponse InvaildParam(params object[] args)
        //    {
        //        return new RowResponse("InvaildParam", args);
        //    }

        //}



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
}