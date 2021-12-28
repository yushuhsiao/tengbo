// 2014.1.6 - 所有api一律為async模式

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using BU;
using Newtonsoft.Json;
using Tools;
using Tools.Protocol;
using System.Data.SqlClient;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;
using System.Security.Cryptography;

namespace web
{
    [_DebuggerStepThrough]
    public abstract class api : IHttpAsyncHandler, IAsyncResult, IDisposable
    {
        static int _msgid;
        internal readonly int msgid = Interlocked.Increment(ref _msgid);

        public void writelog(HttpContext context, string msg)
        {
            log.message(prefix, "{0}\t{1}", context.RequestIP(), msg);
        }
        public void writelog(HttpContext context, string format, params object[] args)
        {
            writelog(context, string.Format(format, args));
        }

        HttpContext _context;
        protected HttpContext context
        {
            get { return Interlocked.CompareExchange(ref this._context, null, null); }
            set { Interlocked.Exchange(ref this._context, value); }
        }

        AsyncCallback _cb;
        protected AsyncCallback cb
        {
            get { return Interlocked.CompareExchange(ref this._cb, null, null); }
            set { Interlocked.Exchange(ref this._cb, value); }
        }

        string _json_s;
        protected string json_s
        {
            get { return Interlocked.CompareExchange(ref this._json_s, null, null); }
            set { Interlocked.Exchange(ref this._json_s, value); }
        }

        object _command;
        protected object command
        {
            get { return Interlocked.CompareExchange(ref this._command, null, null); }
            set { Interlocked.Exchange(ref this._command, value); }
        }

        void IDisposable.Dispose()
        {
            (this.cb ?? _null.noop)(this);
            this.context = null;
            this.cb = null;
            this.json_s = null;
            this.command = null;
            this.OnDispose();
        }
        protected abstract void OnDispose();

        #region IAsyncResult

        object IAsyncResult.AsyncState { get { return null; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        bool IAsyncResult.CompletedSynchronously { get { return this.cb == null; } }
        bool IAsyncResult.IsCompleted { get { return this.cb == null; } }

        #endregion

        #region Serialize / Deserialize

        [_DebuggerStepThrough]
        public class json_reader : JsonProtocol.JsonReader
        {
            public json_reader(TextReader reader) : base(reader) { }

            public override byte[] ReadAsBytes()
            {
                try { return base.ReadAsBytes(); }
                catch { SetToken(JsonToken.Null); return null; }
            }

            public override DateTime? ReadAsDateTime()
            {
                try { return base.ReadAsDateTime(); }
                catch { SetToken(JsonToken.Null); return null; }
            }

            public override decimal? ReadAsDecimal()
            {
                try { return base.ReadAsDecimal(); }
                catch { SetToken(JsonToken.Null); return null; }
            }

            public override int? ReadAsInt32()
            {
                try { return base.ReadAsInt32(); }
                catch { SetToken(JsonToken.Null); return null; }
            }

            //public override string ReadAsString()
            //{
            //    string s = base.ReadAsString();
            //    if (s != null)
            //    {
            //        s = s.Trim();
            //        if (s == string.Empty)
            //        {
            //            SetToken(JsonToken.Null);
            //            return null;
            //        }
            //    }
            //    return s;
            //}
        }

        [_DebuggerStepThrough]
        public class json_writer : JsonProtocol.JsonWriter
        {
            public json_writer(TextWriter textWriter)
                : base(textWriter)
            {
                base.QuoteChar = '\"';
                base.QuoteName = true;
                base.Formatting = Formatting.None;
            }

            public override void WriteValue(DateTime value)
            {   // protocol 的日期一律轉換成 utc
                base.WriteValue(value.ToUniversalTime());
            }

            SqlCmd m_sqlcmd;
            public SqlCmd sqlcmd
            {
                get { lock (this) return this.m_sqlcmd = this.m_sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read); }
            }

            public override void Close()
            {
                base.Close();
                using (this.m_sqlcmd)
                    this.m_sqlcmd = null;
            }
        }

        public static string SerializeObject(object value)
        {
            return JsonProtocol.SerializeObject<json_writer>(value);
        }

        public static string SerializeObject(string propertyName, object value)
        {
            return JsonProtocol.SerializeObject<json_writer>(propertyName, value);
        }

        public static T DeserializeObject<T>(string json)
        {
            return JsonProtocol.DeserializeObject<json_reader, T>(json);
        }

        public static object DeserializeObject(Type type, string json)
        {
            object value;
            JsonProtocol.Deserialize<json_reader>(json, out value, type, null);
            return value;
        }

        public static bool PopulateObject(string json, object obj)
        {
            return JsonProtocol.Populate<json_reader>(json, obj);
        }

        #endregion

        protected abstract IAsyncResult OnBeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData);
        IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            this.context = context;
            return this.OnBeginProcessRequest(context, cb, extraData);
        }

        protected abstract void OnEndProcessRequest(IAsyncResult result);
        void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result)
        {
            this.OnEndProcessRequest(result);
        }

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context) { }


        public abstract string prefix { get; }

        public bool EnableWriteResponse = true;

        protected IAsyncResult ResponseError(HttpContext context, HttpStatusCode statusCode)
        {
            if (this.EnableWriteResponse)
                context.Response.StatusCode = (int)statusCode;
            return this;
        }

        protected void WriteResponse(HttpContext context, object result)
        {
            using (this)
            {
                string json_r;
                if (!JsonProtocol.Serialize<json_writer>(result, out json_r))
                    json_r = JsonProtocol.SerializeObject<json_writer>(result);
                if (this.EnableWriteResponse)
                    context.Response.Write(json_r);
                this.writelog(context, "response\t{0}", json_r); //log.message(prefix, "{0}\tresponse\t{1}", context.RequestIP(), json_r);
            }
        }



        [_DebuggerStepThrough]
        public class CommandQueue : IDisposable
        {
            static readonly Queue<CommandQueue> all;
            public static readonly CommandQueue Global;
            static readonly Timer timer;

            static CommandQueue()
            {
                CommandQueue.all = new Queue<CommandQueue>();
                CommandQueue.Global = new CommandQueue();
                CommandQueue.timer = new Timer(timer_proc, null, 1, 1);
            }

            static void timer_proc(object state)
            {
                if (Monitor.TryEnter(all))
                {
                    CommandQueue item;
                    try
                    {
                        item = all.Dequeue();
                        if (item.IsDisposed)
                            return;
                        all.Enqueue(item);
                    }
                    finally { Monitor.Exit(all); }
                    if (Interlocked.CompareExchange(ref item.busy, timer, null) == null)
                    {
                        try
                        {
                            item._comet_proc();
                            item._async_proc();
                        }
                        finally { Interlocked.Exchange(ref item.busy, null); }
                    }
                }
            }
            object busy;

            object active;
            bool IsDisposed
            {
                get { return Interlocked.CompareExchange(ref this.active, null, null) == null; }
            }

            public CommandQueue()
            {
                active = typeof(CommandQueue);
                lock (all) all.Enqueue(this);
            }
            void IDisposable.Dispose()
            {
                if (this == Global) return;
                if (Interlocked.Exchange(ref this.active, null) == null) return;
                this._async_proc();
                this._comet_proc();
            }

            #region Async

            Queue<async_base> _async = new Queue<async_base>();

            public bool Add(async_base async)
            {
                if (this.IsDisposed)
                    return false;
                lock (this._async)
                {
                    if (this._async.Contains(async))
                        return false;
                    this._async.Enqueue(async);
                    return true;
                }
            }

            void _async_proc()
            {
                if (this.IsDisposed)
                {
                    lock (this._async)
                    {
                        while (this._async.Count > 0)
                        {
                            using (async_base async = this._async.Dequeue())
                            {
                                async.context.Response.StatusCode = (int)HttpStatusCode.Moved;
                            }
                        }
                    }
                }
                else if (Monitor.TryEnter(this._async))
                {
                    async_base async;
                    try
                    {
                        if (this._async.Count == 0)
                            return;
                        async = this._async.Dequeue();
                    }
                    finally { Monitor.Exit(this._async); }
                    using (async)
                    {
                        try
                        {
                            HttpContext.Current = async.context;
                            async.ProcessRequest();
                        }
                        catch { }
                    }
                }
            }

            #endregion

            #region Comet

            List<comet> _comet = new List<comet>();

            internal bool Add(comet comet)
            {
                if (this.IsDisposed)
                    return false;
                lock (this._comet)
                {
                    if (this._comet.Contains(comet))
                        return false;
                    this._comet.Add(comet);
                    return true;
                }
            }

            void _comet_proc()
            {
                if (this.IsDisposed)
                {
                    lock (this._comet)
                    {
                        while (this._comet.Count > 0)
                        {
                            using (comet comet = this._comet[0])
                            {
                                this._comet.RemoveAt(0);
                                comet.context.Response.StatusCode = (int)HttpStatusCode.Moved;
                            }
                        }
                    }
                }
                else if (Monitor.TryEnter(this._comet))
                {
                    try
                    {
                        if (this._comet.Count == 0) return;
                        for (int i = this._comet.Count - 1; i >= 0; i--)
                        {
                            if (this._comet[i].OnTick() == false)
                                this._comet.RemoveAt(i);
                            //comet c = this._comet[i];
                            //double t = (DateTime.Now.Ticks - c.BeginTime_Tick) / 10000;
                            //if (t >= c.CometTimeout)
                            //    log.message(comet.prefix, "comet timeout!");
                            //else if (!c.context.Response.IsClientConnected)
                            //    log.message(comet.prefix, "comet disconnected!");
                            //else
                            //{
                            //    i++;
                            //    continue;
                            //}
                            //using (c) this._comet.RemoveAt(i);
                        }
                    }
                    finally { Monitor.Exit(this._comet); }
                }
            }

            #endregion

            /// <summary>
            /// Post Message to Comet
            /// </summary>
            public static void Post<T>(User user, int lifetime, object value) where T : comet
            {
            }
        }

        //[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        //public class AdminWebAttribute : Attribute { }

        //[_DebuggerStepThrough, AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        //public class AsyncAttribute : Attribute
        //{
        //    static Dictionary<MethodInfo, bool> cache = new Dictionary<MethodInfo, bool>();

        //    internal static bool IsAsync(MethodInfo method)
        //    {
        //        return true;
        //        //lock (cache)
        //        //{
        //        //    bool res;
        //        //    if (cache.TryGetValue(method, out res))
        //        //        return res;
        //        //    foreach (object a in method.GetCustomAttributes(false))
        //        //        if (a is AsyncAttribute)
        //        //            return cache[method] = true;
        //        //    return cache[method] = false;
        //        //}
        //    }
        //}

        public static result<T> CreateResult<T>(string json_r)
        {
            result<T> result = new result<T>();
            api.PopulateObject(json_r, result);
            return result;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class result<T>
        {
            [JsonProperty]
            public RowErrorCode? Status;
            [JsonProperty]
            public string Message;
            [JsonProperty]
            public T row;
        }
    }

    [_DebuggerStepThrough]
    public abstract class async_base : api, IHttpAsyncHandler, IAsyncResult, IDisposable
    {
        public override string prefix
        {
            get { return "async"; }
        }

        public async_base() { }

        ObjectInvokeAttribute _objectInvoke;
        ObjectInvokeAttribute objectInvoke
        {
            get { return Interlocked.CompareExchange(ref this._objectInvoke, null, null); }
            set { Interlocked.Exchange(ref this._objectInvoke, value); }
        }

        protected override void OnDispose()
        {
            this.objectInvoke = null;
        }

        protected virtual void ext_auth()
        {
        }

        protected virtual bool DeserializeCommand(HttpContext context, ref User user, string json_s, out object command)
        {
            return JsonProtocol.Deserialize<json_reader>(json_s, out command);
        }

        protected override IAsyncResult OnBeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            User user = context.User as User;
            if (user == null)
                return this.ResponseError(context, HttpStatusCode.Forbidden);

            string json_s = context.Request.Form["str"] ?? "";
            if (_Global.DebugMode)
                json_s = (extraData as string) ?? json_s;
            this.json_s = json_s;

            object command;
            if (!this.DeserializeCommand(context, ref user, json_s, out command))
                return this.ResponseError(context, HttpStatusCode.NotFound);
            this.command = command;

            ObjectInvokeAttribute objectInvoke;
            if (!System.Reflection.ObjectInvoke.GetDefine(out objectInvoke, command, null, command, json_s, _null.objects))
                return this.ResponseError(context, HttpStatusCode.NotFound);
            this.objectInvoke = objectInvoke;

            if (user.Permissions.Check(objectInvoke.Method) == false)
                return this.ResponseError(context, HttpStatusCode.Forbidden);

            if (cb != null)
            {
                this.cb = cb;
                if (user.CommandQueue.Add(this))
                {
                    this.writelog(context, "async request\t{0}\t{1}", this.msgid, json_s); //log.message(prefix, "{0}\tasync request\t{1}\t{2}", context.RequestIP(), this.msgid, json_s);
                    return this;
                }
            }
            using (this)
            {
                this.ProcessRequest();
                return this;
            }
        }

        protected override void OnEndProcessRequest(IAsyncResult result) { }

        // 發生錯誤的時候, 是否要顯示完整的錯誤訊息內容
        [AppSetting]
        static bool api_fullmsg
        {
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
        }

        internal void ProcessRequest()
        {
            HttpContext context = this.context;
            ObjectInvokeAttribute objectInvoke = this.objectInvoke;
            object command = this.command;
            string json_s = this.json_s;
            if (this.cb == null)
                this.writelog(context, "request\t{0}", json_s); //log.message(prefix, "{0}\trequest\t{1}", context.RequestIP(), json_s);
            else
                this.writelog(context, "process async\t{0}", msgid); //log.message(prefix, "{0}\tprocess async\t{1}", context.RequestIP(), msgid);
            try
            {
                using (command as IDisposable)
                {
                    object result = null;
                    if (command is IRowCommand)
                    {
                        try
                        {
                            object row = objectInvoke.Invoke(command, command, json_s, _null<object>.array);
                            if (row != null)
                                this.WriteResponse(context, RowException.Success(row));
                        }
                        catch (RowException ex) { this.WriteResponse(context, ex); }
                        catch (Exception ex) { this.WriteResponse(context, new RowException(ex, RowErrorCode.Unknown, api_fullmsg ? ex.Message : null)); throw ex; }
                    }
                    else
                    {
                        result = objectInvoke.Invoke(command, command, json_s, _null<object>.array);
                        if (result != null)
                            this.WriteResponse(context, result);
                    }
                }
            }
            catch (Exception ex)
            {
                this.writelog(context, "error\t{0}", ex.Message); //log.message(prefix, "{0}error\t{1}", context.RequestIP(), ex.Message);
                throw ex;
            }
        }

        //[_DebuggerStepThrough]
        //public class CommandQueue2 : IDisposable
        //{
        //    static readonly Queue<CommandQueue> all;
        //    public static readonly CommandQueue Global;
        //    static readonly Timer timer;

        //    static CommandQueue2()
        //    {
        //        CommandQueue.all = new Queue<CommandQueue>();
        //        CommandQueue.Global = new CommandQueue();
        //        CommandQueue.timer = new Timer(timer_proc, null, 1, 1);
        //    }

        //    static void timer_proc(object state)
        //    {
        //        if (Monitor.TryEnter(all))
        //        {
        //            CommandQueue item;
        //            try
        //            {
        //                item = all.Dequeue();
        //                if (item.IsDisposed)
        //                    return;
        //                all.Enqueue(item);
        //            }
        //            finally { Monitor.Exit(all); }
        //            if (Interlocked.CompareExchange(ref item.busy, timer, null) == null)
        //            {
        //                try { item._api_proc(); }
        //                finally { Interlocked.Exchange(ref item.busy, null); }
        //            }
        //        }
        //    }
        //    object busy;

        //    object active;
        //    bool IsDisposed
        //    {
        //        get { return Interlocked.CompareExchange(ref this.active, null, null) == null; }
        //    }

        //    public CommandQueue2()
        //    {
        //        active = typeof(CommandQueue);
        //        lock (all) all.Enqueue(this);
        //    }
        //    void IDisposable.Dispose()
        //    {
        //        if (this == Global) return;
        //        if (Interlocked.Exchange(ref this.active, null) == null) return;
        //        this._api_proc();
        //        //this._comet_proc();
        //    }

        //    Queue<api> _api = new Queue<api>();

        //    public bool Add(api api)
        //    {
        //        if (this.IsDisposed)
        //            return false;
        //        lock (this._api)
        //        {
        //            if (this._api.Contains(api))
        //                return false;
        //            this._api.Enqueue(api);
        //            return true;
        //        }
        //    }

        //    void _api_proc()
        //    {
        //        if (this.IsDisposed)
        //        {
        //            lock (this._api)
        //            {
        //                while (this._api.Count > 0)
        //                {
        //                    using (api async = this._api.Dequeue())
        //                    {
        //                        async.context.Response.StatusCode = (int)HttpStatusCode.Moved;
        //                    }
        //                }
        //            }
        //        }
        //        else if (Monitor.TryEnter(this._api))
        //        {
        //            api api;
        //            try
        //            {
        //                if (this._api.Count == 0)
        //                    return;
        //                api = this._api.Dequeue();
        //            }
        //            finally { Monitor.Exit(this._api); }
        //            using (api)
        //            {
        //                try
        //                {
        //                    HttpContext.Current = api.context;
        //                    api.ProcessRequest();
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //}
    }

    public class async : async_base, IRequiresSessionState { }


    [_DebuggerStepThrough]
    public class comet : api, IHttpAsyncHandler, IAsyncResult
    {
        public override string prefix
        {
            get { return "comet"; }
        }

        [SqlSetting("comet", "port"), DefaultValue(10080)]
        public static int Port
        {
            get { return app.config.GetValue<int>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("comet", "timeout"), DefaultValue(30000)]
        public static long Timeout
        {
            get { return app.config.GetValue<long>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("comet", "watchdog"), DefaultValue(30000)]
        public static long WatchDog
        {
            get { return app.config.GetValue<long>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("comet", "sleep"), DefaultValue(500)]
        public static long Sleep
        {
            get { return app.config.GetValue<long>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("comet", "url"), DefaultValue("")]
        public static string Url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        public static string GetUrl(System.Web.UI.Page page)
        {
            UriBuilder uri = new UriBuilder(page.Request.Url);
            uri.Port = web.comet.Port;
            uri.Path = page.ResolveUrl(web.comet.Url);
            return uri.ToString();
        }

        long _CometTimeout;
        public long CometTimeout
        {
            get { return Interlocked.CompareExchange(ref this._CometTimeout, 0, 0); }
            set { Interlocked.Exchange(ref this._CometTimeout, value); }
        }

        long _BeginTime_Tick;
        public long BeginTime_Tick
        {
            get { return Interlocked.CompareExchange(ref this._BeginTime_Tick, 0, 0); }
            set { Interlocked.Exchange(ref this._BeginTime_Tick, value); }
        }
        DateTime BeginTime
        {
            get { return new DateTime(this.BeginTime_Tick); }
            set { this.BeginTime_Tick = value.Ticks; }
        }

        internal bool OnTick()
        {
            double t = (DateTime.Now.Ticks - this.BeginTime_Tick) / 10000;
            if (t >= this.CometTimeout)
            {
                this.writelog(context, "comet timeout!");
                context.Response.Write("{}");
            }
            else if (!this.context.Response.IsClientConnected)
                this.writelog(context, "comet disconnected!");
            else
                return true;
            using (this)
                return false;
        }

        [JsonProperty]
        public Guid? LoginID;

        protected override IAsyncResult OnBeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            string json = context.Request.Form["str"];
            api.PopulateObject(json, this);
            if (this.LoginID.HasValue)
            {
                foreach (User _user in _Global.GetUserList(context))
                {
                    if (_user.LoginID == this.LoginID.Value)
                    {
                        context.User = _user;
                        break;
                    }
                }
            }
            User user = context.User as User;
            if (user is Guest)
            {
                using (this)
                {
                    this.ResponseError(context, HttpStatusCode.Forbidden);
                    return this;
                }
            }
            this.cb = cb;
            this.BeginTime = DateTime.Now;
            this.CometTimeout = comet.Timeout;
            context.Response.AppendHeader("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
            context.Response.AppendHeader("Access-Control-Allow-Headers", "Content-Type");
            context.Response.AppendHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
            user.CommandQueue.Add(this);
            return this;
        }

        protected override void OnEndProcessRequest(IAsyncResult result) { }

        protected override void OnDispose() { }
    }
}