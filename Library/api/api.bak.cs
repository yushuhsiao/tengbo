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

namespace web
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public class api : IHttpAsyncHandler, IRequiresSessionState
    {
        const string prefix = "api";

        public api() { }
        bool IHttpHandler.IsReusable { get { return true; } }
        void IHttpHandler.ProcessRequest(HttpContext context) { }

        #region Serialize / Deserialize

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

            public override string ReadAsString()
            {
                string s = base.ReadAsString();
                if (s != null)
                {
                    s = s.Trim();
                    if (s == string.Empty)
                    {
                        SetToken(JsonToken.Null);
                        return null;
                    }
                }
                return s;
            }
        }

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

        public static bool PopulateObject(string json, object obj)
        {
            return JsonProtocol.Populate<json_reader>(json, obj);
        }

        #endregion

        public void ProcessRequest(HttpContext context) { ((IHttpAsyncHandler)this).BeginProcessRequest(context, null, null); }

        IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            User user = context.User as User;
            if (user == null)
                return this.ResponseError(context, HttpStatusCode.Forbidden);

            string json_s = context.Request.Form["str"] ?? "";
            if (User.Manager.DebugMode)
                json_s = (extraData as string) ?? json_s;

            object command;
            if (!JsonProtocol.Deserialize<json_reader>(json_s, out command))
                return this.ResponseError(context, HttpStatusCode.NotFound);

            #region //
            //Comet comet = command as Comet;
            ////comet = null; // 尚未找到同時發送多個 http reques 的方法, 所以 comet 部分暫時關閉
            //if (comet != null)
            //{
            //    //if (user is Guest)
            //    //    return api.ResponseError(context, HttpStatusCode.Forbidden);
            //    if (!api.PermissionAttribute.CheckPermission(comet, user))
            //        return api.ResponseError(context, HttpStatusCode.Forbidden);
            //    comet.context = context;
            //    comet.command = command;
            //    comet.json_s = json_s;
            //    if (user.CommandQueue.Add(comet, cb))
            //    {
            //        log.message(prefix, "{0}\tcomet request\t{1}\t{2}", context.Request.UserHostAddress, comet.msgid, json_s);
            //        return comet;
            //    }
            //    else
            //        using (comet) return api.ResponseError(context, HttpStatusCode.Forbidden);
            //}
            #endregion

            ObjectInvokeAttribute objectInvoke;
            if (!System.Reflection.ObjectInvoke.GetDefine(out objectInvoke, command, null, command, json_s, _null.objects))
                return this.ResponseError(context, HttpStatusCode.NotFound);

            if (!user.Permissions.Check(objectInvoke.Method))
                return this.ResponseError(context, HttpStatusCode.Forbidden);

            if ((cb != null) && api.AsyncAttribute.IsAsync(objectInvoke.Method))
            {
                Comet comet = command as Comet;
                if (comet != null)
                {
                    if (user.CommandQueue.Add(comet, this, context, cb, json_s))
                    {
                        log.message(prefix, "{0}\tcomet request\t{1}\t{2}", context.Request.UserHostAddress, comet.msgid, json_s);
                        return comet;
                    }
                }

                ObjectCommand async = new ObjectCommand() { api = this, context = context, command = command, json_s = json_s, objectInvoke = objectInvoke };
                if (user.CommandQueue.Add(async, cb))
                {
                    log.message(prefix, "{0}\tasync request\t{1}\t{2}", context.Request.UserHostAddress, async.msgid, json_s);
                    return async;
                }
                else using (async) { }
            }
            this.ProcessRequest(null, context, objectInvoke, command, json_s);
            return Sync.Instance;
        }
        void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result) { }

        #region 發生錯誤的時候, 是否要顯示完整的錯誤訊息內容

        [AppSetting]
        static bool api_fullmsg
        {
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
        }

        #endregion

        void ProcessRequest(int? msgid, HttpContext context, ObjectInvokeAttribute objectInvoke, object command, string json_s, params object[] args)
        {
            if (msgid.HasValue)
                log.message(prefix, "{0}\tprocess async\t{1}", context.Request.UserHostAddress, msgid);
            else
                log.message(prefix, "{0}\trequest\t{1}", context.Request.UserHostAddress, json_s);
            try
            {
                object result = null;
                if (command is IRowCommand)
                {
                    try
                    {
                        object row = objectInvoke.Invoke(command, command, json_s, args);
                        if (row != null)
                            this.WriteResponse(context, RowException.Success(row));
                    }
                    catch (RowException ex) { this.WriteResponse(context, ex); }
                    catch (Exception ex) { this.WriteResponse(context, new RowException(ex, RowErrorCode.Unknown, api_fullmsg ? ex.Message : null)); throw ex; }
                }
                else
                {
                    result = objectInvoke.Invoke(command, command, json_s, args);
                    if (result != null)
                        this.WriteResponse(context, result);
                }
            }
            catch (Exception ex)
            {
                log.message(prefix, "error\t{0}", ex.Message);
                throw ex;
            }
        }

        public bool EnableWriteResponse = true;

        void WriteResponse(HttpContext context, object result)
        {
            string json_r;
            if (!JsonProtocol.Serialize<json_writer>(result, out json_r))
                json_r = JsonProtocol.SerializeObject<json_writer>(result);
            if (this.EnableWriteResponse)
                context.Response.Write(json_r);
            log.message(prefix, "{0}\tresponse\t{1}", context.Request.UserHostAddress, json_r);
        }

        IAsyncResult ResponseError(HttpContext context, HttpStatusCode statusCode)
        {
            if (this.EnableWriteResponse)
                context.Response.StatusCode = (int)statusCode;
            return Sync.Instance;
        }

        #region sealed class Sync

        [_DebuggerStepThrough]
        sealed class Sync : IAsyncResult
        {
            private Sync() { }

            internal static readonly Sync Instance = new Sync();

            object IAsyncResult.AsyncState { get { return null; } }

            WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }

            bool IAsyncResult.CompletedSynchronously { get { return true; } }

            bool IAsyncResult.IsCompleted { get { return true; } }
        }

        #endregion

        #region abstract class Async

        [_DebuggerStepThrough]
        public abstract class Async : IAsyncResult, IDisposable
        {
            static int _msgid;
            internal readonly int msgid = Interlocked.Increment(ref _msgid);

            internal api api;
            internal HttpContext context;
            internal AsyncCallback cb;
            internal string json_s;

            internal Async() { }

            void IDisposable.Dispose()
            {
                if (this.cb != null)
                    this.cb(this);
                this.context = null;
                this.cb = null;
                this.json_s = null;
                this.OnDispose();
            }
            protected abstract void OnDispose();

            object IAsyncResult.AsyncState { get { return null; } }

            WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }

            bool IAsyncResult.CompletedSynchronously { get { return false; } }

            bool IAsyncResult.IsCompleted { get { return false; } }
        }

        #endregion

        [_DebuggerStepThrough]
        public sealed class ObjectCommand : Async
        {
            internal ObjectCommand() { }

            internal object command;
            internal ObjectInvokeAttribute objectInvoke;

            protected override void OnDispose()
            {
                this.command = null;
                this.objectInvoke = null;
            }
        }

        [_DebuggerStepThrough]
        public abstract class Comet : Async
        {
            [SqlSetting("comet", "port"), DefaultValue(10080)]
            public static int Port
            {
                get { return app.config.GetValue<int>(MethodBase.GetCurrentMethod()); }
            }

            [SqlSetting("comet", "timeout"), DefaultValue(30000)]
            public static double Timeout
            {
                get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
            }

            public double CometTimeout;

            internal DateTime time { get; set; }

            protected override void OnDispose() { }
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

            Queue<ObjectCommand> _async = new Queue<ObjectCommand>();

            public bool Add(ObjectCommand async, AsyncCallback cb)
            {
                if (this.IsDisposed)
                    return false;
                lock (this._async)
                {
                    if (this._async.Contains(async))
                        return false;
                    async.cb = cb;
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
                            using (ObjectCommand async = this._async.Dequeue())
                            {
                                async.context.Response.StatusCode = (int)HttpStatusCode.Moved;
                            }
                        }
                    }
                }
                else if (Monitor.TryEnter(this._async))
                {
                    ObjectCommand async;
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
                            api api = async.api;
                            async.api = null;
                            api.ProcessRequest(async.msgid, HttpContext.Current = async.context, async.objectInvoke, async.command, async.json_s);
                        }
                        catch { }
                    }
                }
            }

            #endregion

            #region Comet

            List<Comet> _comet = new List<Comet>();

            internal bool Add(Comet comet, api api, HttpContext context, AsyncCallback cb, string json_s)
            {
                if (this.IsDisposed)
                    return false;
                lock (this._comet)
                {
                    if (this._comet.Contains(comet))
                        return false;
                    comet.api = api;
                    comet.context = context;
                    comet.cb = cb;
                    comet.json_s = json_s;
                    comet.CometTimeout = Comet.Timeout;
                    comet.time = DateTime.Now;
                    this._comet.Add(comet);
                    context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
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
                            using (Comet comet = this._comet[0])
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
                        for (int i = 0; i < this._comet.Count; )
                        {
                            Comet c = this._comet[i];
                            double t = (DateTime.Now.Ticks - c.time.Ticks) / 10000;
                            if (t >= c.CometTimeout)
                                log.message(prefix, "comet timeout!");
                            else if (!c.context.Response.IsClientConnected)
                                log.message(prefix, "comet disconnected!");
                            else
                            {
                                i++;
                                continue;
                            }
                            using (c) this._comet.RemoveAt(i);
                        }
                    }
                    finally { Monitor.Exit(this._comet); }
                }
            }

            #endregion

            /// <summary>
            /// Post Message to Comet
            /// </summary>
            public static void Post<T>(User user, int lifetime, object value) where T : Comet
            {
            }
        }

        [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class AsyncAttribute : Attribute
        {
            static Dictionary<MethodInfo, bool> cache = new Dictionary<MethodInfo, bool>();

            internal static bool IsAsync(MethodInfo method)
            {
                return true;
                //lock (cache)
                //{
                //    bool res;
                //    if (cache.TryGetValue(method, out res))
                //        return res;
                //    foreach (object a in method.GetCustomAttributes(false))
                //        if (a is AsyncAttribute)
                //            return cache[method] = true;
                //    return cache[method] = false;
                //}
            }
        }
    }
}