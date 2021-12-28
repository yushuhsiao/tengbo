﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;

namespace TopGame.WebSockets
{
    public delegate void SocketHandler(WebSocketServer server, Socket socket);
    public delegate void WebSocketHandler(WebSocketServer server, WebSocket_ ws);
    public delegate void EventHandler(WebSocket_ ws);
    public delegate void HandshakeHandler(WebSocket_ ws, WebSocketRequest_ request, WebSocketResponse_ response);
    public delegate void FrameHandler(WebSocket_ ws, Frame frame);
    public delegate void FrameHandler<T>(WebSocket_ ws, Frame frame, T arg);
    
    public sealed class WebSocketServer
    {
        public Uri Url { get; private set; }
        public double HandshakeTimeoutValue { get; set; }
        Socket m_Listener;
        public WebSocketServer(string url)
        {
            Url = new Uri(url);
            HandshakeTimeoutValue = 3000000;
            m_Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_Listener.Bind(new IPEndPoint(IPAddress.Any, Url.Port));
            m_Listener.Listen(100);
            m_Listener.BeginAccept(accept, null);
            log.message(null, "WebSocket url : {0}", url);
        }

        void accept(IAsyncResult ar)
        {
            DateTime time = DateTime.Now;
            try
            {
                Socket socket = m_Listener.EndAccept(ar);
                m_Listener.BeginAccept(accept, null);
                log.message("ws", "Accept socket from {0}", socket.RemoteEndPoint);
                AcceptSocketEvent(this, socket);
                WebSocketRequest.StartHandshake(this, new SocketStream(socket));
                //WebSocket ws = new WebSocket(this, socket);
                //AcceptWebSocketEvent(this, ws);
            }
            catch (Exception ex) { log.message("ex", ex.ToString()); }
        }

        private SocketHandler e_AcceptSocket;
        public SocketHandler AcceptSocketEvent
        {
            get { return Interlocked.CompareExchange(ref e_AcceptSocket, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_AcceptSocket, value); }
        }
        public event SocketHandler AcceptSocket
        {
            add { AcceptSocketEvent = value; }
            remove { AcceptSocketEvent = null; }
        }

        private WebSocketHandler e_AcceptWebSocket;
        public WebSocketHandler AcceptWebSocketEvent
        {
            get { return Interlocked.CompareExchange(ref e_AcceptWebSocket, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_AcceptWebSocket, value); }
        }
        public event WebSocketHandler AcceptWebSocket
        {
            add { AcceptWebSocketEvent = value; }
            remove { AcceptWebSocketEvent = null; }
        }


        private SocketHandler e_HandshakeTimeout;
        public SocketHandler HandshakeTimeoutEvent
        {
            get { return Interlocked.CompareExchange(ref e_HandshakeTimeout, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_HandshakeTimeout, value); }
        }
        public event SocketHandler HandshakeTimeout
        {
            add { HandshakeTimeoutEvent = value; }
            remove { HandshakeTimeoutEvent = null; }
        }
    }

    class SocketStream : Stream
    {
        internal readonly Socket socket;
        public readonly IPEndPoint LocalEndPoint;
        public readonly IPEndPoint RemoteEndPoint;

        [DebuggerStepThrough]
        internal SocketStream(Socket socket)
        {
            this.socket = socket;
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            this.LocalEndPoint = (IPEndPoint)socket.LocalEndPoint;
            this.RemoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
        }

        Action<SocketStream> close_callback;
        DateTime close_time;

        public override void Close()
        {
            try
            {
                Monitor.Enter(this);
                using (socket)
                {
                    try { socket.Shutdown(SocketShutdown.Both); }
                    catch { }
                    try
                    {
                        socket.Close();
                        if (close_callback != null)
                            close_callback(this);
                    }
                    catch { }
                }
                base.Close();
            }
            finally
            {
                close_callback = null;
                Monitor.Exit(this);
            }
        }

        public void Close(double? timeout, Action<SocketStream> callback)
        {
            if (timeout.HasValue)
            {
                Interlocked.Exchange(ref close_callback, callback);
                lock (this)
                    close_time = DateTime.Now.AddMilliseconds(timeout.Value);
                Tick.OnTick += close_tick;
            }
            else
            {
                Interlocked.Exchange(ref close_callback, null);
            }
        }

        bool close_tick()
        {
            if (Interlocked.CompareExchange(ref close_callback, null, null) == null)
                return false;
            if (!Monitor.TryEnter(this))
                return true;
            try
            {
                if (close_time >= DateTime.Now)
                    return true;
                using (this)
                    return false;
            }
            finally { Monitor.Exit(this); }
        }



        [DebuggerStepThrough]
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffer, offset, count, SocketFlags.None, callback, state);
        }
        [DebuggerStepThrough]
        public override int EndRead(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }
        [DebuggerStepThrough]
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffer, offset, count, SocketFlags.None, callback, state);
        }
        [DebuggerStepThrough]
        public override void EndWrite(IAsyncResult asyncResult)
        {
            socket.EndSend(asyncResult);
        }
        public override bool CanRead
        {
            [DebuggerStepThrough]
            get { return true; }
        }
        public override bool CanSeek
        {
            [DebuggerStepThrough]
            get { return false; }
        }
        public override bool CanWrite
        {
            [DebuggerStepThrough]
            get { return true; }
        }
        [DebuggerStepThrough]
        public override void Flush()
        {
        }
        public override long Length
        {
            [DebuggerStepThrough]
            get { return 0; }
        }
        public override long Position
        {
            [DebuggerStepThrough]
            get { return 0; }
            [DebuggerStepThrough]
            set { }
        }
        [DebuggerStepThrough]
        public override int Read(byte[] buffer, int offset, int count)
        {
            return socket.Receive(buffer, offset, count, SocketFlags.None);
        }
        [DebuggerStepThrough]
        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }
        [DebuggerStepThrough]
        public override void SetLength(long value)
        {
        }
        [DebuggerStepThrough]
        public override void Write(byte[] buffer, int offset, int count)
        {
            socket.Send(buffer, offset, count, SocketFlags.None);
        }
    }

    public sealed class WebSocketRequest : NameValueCollection, IDisposable
    {
        public string this[HttpRequestHeader name]
        {
            get { return base[name.ToString()]; }
            set { base[name.ToString()] = value; }
        }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Upgrade
        {
            get { return this[HttpRequestHeader.Upgrade]; }
            set { this[HttpRequestHeader.Upgrade] = value; }
        }
        public string Connection
        {
            get { return this[HttpRequestHeader.Connection]; }
            set { this[HttpRequestHeader.Connection] = value; }
        }
        public string Host
        {
            get { return this[HttpRequestHeader.Host]; }
            set { this[HttpRequestHeader.Host] = value; }
        }
        public string Origin
        {
            get { return this["Origin"]; }
            set { this["Origin"] = value; }
        }
        public string SecWebSocketKey
        {
            get { return this[tools.SecWebSocketKey]; }
            set { this[tools.SecWebSocketKey] = value; }
        }
        public string SecWebSocketVersion
        {
            get { return this[tools.SecWebSocketVersion]; }
            set { this[tools.SecWebSocketVersion] = value; }
        }
        public string SecWebSocketExtensions
        {
            get { return this[tools.SecWebSocketExtensions]; }
            set { this[tools.SecWebSocketExtensions] = value; }
        }
        public string SecWebSocketDraft
        {
            get { return this["Sec-WebSocket-Draft"]; }
            set { this["Sec-WebSocket-Draft"] = value; }
        }
        public string SecWebSocketKey1
        {
            get { return this[tools.SecWebSocketKey1]; }
            set { this[tools.SecWebSocketKey1] = value; }
        }
        public string SecWebSocketKey2
        {
            get { return this[tools.SecWebSocketKey2]; }
            set { this[tools.SecWebSocketKey2] = value; }
        }
        public string SecWebSocketKey3
        {
            get { return this[tools.SecWebSocketKey3]; }
            set { this[tools.SecWebSocketKey3] = value; }
        }
        public string WebSocketProtocol
        {
            get { return this[tools.WebSocketProtocol]; }
            set { this[tools.WebSocketProtocol] = value; }
        }

        public byte[] Body { get; set; }
        public byte[] Bytes { get { return bytes; } }
        public string HttpRequest { get; set; }


        static int id;
        readonly int ID = Interlocked.Increment(ref id);
        static Queue<WebSocketRequest> pooling = new Queue<WebSocketRequest>();

        readonly WebSocketResponse response;
        WebSocketServer server;
        SocketStream socket;
        byte[] bytes = new byte[tools.BLOCK_SIZE];
        int offset;

        internal static WebSocketRequest StartHandshake(WebSocketServer server, SocketStream socket)
        {
            WebSocketRequest obj;
            lock (pooling)
                if (pooling.Count > 0)
                    obj = pooling.Dequeue();
                else
                    obj = new WebSocketRequest();
            lock (obj)
            {
                obj.server = server;
                obj.socket = socket;
                obj.recv(null);
                socket.Close(server.HandshakeTimeoutValue, obj.timeout);
            }
            return obj;
        }

        void IDisposable.Dispose()
        {
            try
            {
                lock (this)
                {
                    using (socket)
                    {
                        socket = null;
                        server = null;
                        offset = 0;
                    }
                }
            }
            catch { }
            lock (pooling)
                if (!pooling.Contains(this))
                    pooling.Enqueue(this);
        }

        void timeout(SocketStream socket)
        {
            try
            {
                lock (this)
                {
                    if (server == null) using (this) return;
                    log.message("ws", "{0}\thandshake timeout!", socket.RemoteEndPoint.Address);
                    (server.HandshakeTimeoutEvent ?? tools._null)(server, socket.socket);
                }
            }
            catch { using (this) return; }
        }

        unsafe void recv(IAsyncResult ar)
        {
            try
            {
                lock (this)
                {
                    if (socket == null) using (this) return;
                    if (ar != null)
                    {
                        int count = socket.EndRead(ar);
                        if (count == 0) using (this) return;
                        offset += count;
                        if (socket.socket.Available == 0)
                        {
                            if (this.Parse())
                            {
                                //GetResponse();
                                byte[] data = response.GetBytes();
                                //ws.send(data, 0, data.Length, null);
                                Debugger.Break();
                            }
                        }
                    }
                    if (bytes.Length <= offset)
                        Array.Resize(ref bytes, bytes.Length + tools.BLOCK_SIZE);
                    socket.BeginRead(bytes, offset, bytes.Length - offset, recv, null);
                }
            }
            catch { using (this) return; }
        }

        static byte[] http_end = Encoding.UTF8.GetBytes("\r\n\r\n");
        bool Parse()
        {
            int n;
            for (int i = 0, end = offset - http_end.Length + 1; i < end; i++)
            {
                bool success = true;
                for (n = 0; success & (n < http_end.Length); n++)
                    success &= (bytes[i + n] == http_end[n]);
                if (success)
                {
                    i += 4;
                    string http = this.HttpRequest = Encoding.UTF8.GetString(bytes, 0, i);
                    Body = new byte[offset - i];
                    for (n = 0, i++; i < offset; n++, i++)
                        Body[n] = bytes[i];
                    n = 0;
                    HttpRequest = http.ToString();
                    Method = http.ReadTo(ref n, ' ');
                    Path = http.ReadTo(ref n, ' ');
                    Version = http.ReadLine(ref n);
                    string key, value;
                    for (; ; )
                    {
                        if ((key = http.ReadTo(ref n, ':')) == null) break;
                        if ((value = http.ReadLine(ref n)) == null) break;
                        this[key] = value;
                    }
                    return true;
                }
            }
            return false;
        }

        static WebSocketResponse[] responses = new WebSocketResponse[] { new Hybi13WebSocketResponse(), new Draft76WebSocketResponse() };
        public static WebSocketResponse_ GetResponse(WebSocket_ ws, WebSocketRequest_ request)
        {
            WebSocketResponse_ response = new WebSocketResponse_();
            try
            {
                log.message("ws", "{0}\t{1} {2} {3}", ws.RemoteEndPoint.Address, request.Method, request.Path, request.Version);
                response.Status = HttpStatusCode.SwitchingProtocols;
                ws.HandshakeRequestEvent(ws, request, response);
                if (response.Status == HttpStatusCode.SwitchingProtocols)
                {
                    response.Clear();
                    if (string.IsNullOrEmpty(request.Method) || string.IsNullOrEmpty(request.Path) || string.IsNullOrEmpty(request.Version))
                        response.Status = HttpStatusCode.BadRequest;
                    else if (request.Method.compares("GET") == false)
                        response.Status = HttpStatusCode.MethodNotAllowed;
                    else if (request.Version.compares(tools.HTTP11) == false)
                        response.Status = HttpStatusCode.HttpVersionNotSupported;
                    else if (request.Connection.contains("Upgrade") == false)
                        response.Status = HttpStatusCode.BadRequest;
                    else if (request.Upgrade.compares("WebSocket") == false)
                        response.Status = HttpStatusCode.BadRequest;
                    else
                    {
                        try
                        {
                            if (Hybi13Protocol.Handshake(ws, request, response))
                                ws.Protocol = new Hybi13Protocol();
                            else if (Draft76Protocol.Handshake(ws, request, response))
                                ws.Protocol = new Draft76Protocol();
                            else
                                response.Status = HttpStatusCode.NotImplemented;
                        }
                        catch
                        {
                            response.Status = HttpStatusCode.InternalServerError;
                        }
                    }
                }
            }
            catch
            {
                response.Clear();
                response.Status = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }

    public class WebSocketResponse : NameValueCollection
    {
        public string this[HttpResponseHeader name]
        {
            get { return base[name.ToString()]; }
            set { base[name.ToString()] = value; }
        }
        public string Version { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public string Upgrade
        {
            get { return this[HttpResponseHeader.Upgrade]; }
            set { this[HttpResponseHeader.Upgrade] = value; }
        }
        public string Connection
        {
            get { return this[HttpResponseHeader.Connection]; }
            set { this[HttpResponseHeader.Connection] = value; }
        }
        public string SecWebSocketAccept
        {
            get { return this[tools.SecWebSocketAccept]; }
            set { this[tools.SecWebSocketAccept] = value; }
        }
        public string SecWebSocketOrigin
        {
            get { return this[tools.SecWebSocketOrigin]; }
            set { this[tools.SecWebSocketOrigin] = value; }
        }
        public string SecWebSocketLocation
        {
            get { return this[tools.SecWebSocketLocation]; }
            set { this[tools.SecWebSocketLocation] = value; }
        }
        public string SecWebSocketProtocol
        {
            get { return this[tools.SecWebSocketProtocol]; }
            set { this[tools.SecWebSocketProtocol] = value; }
        }

        public byte[] Body { get; set; }
        public byte[] Bytes { get; set; }
        public string HttpResponse { get; set; }

        public string ResponseHeader
        {
            get
            {
                if (string.IsNullOrEmpty(Message))
                    return string.Format("{0} {1} {2}", Version, (int)Status, Status);
                else
                    return string.Format("{0} {1} {2}", Version, (int)Status, Message);
            }
        }

        public WebSocketResponse() { this.Clear(); }
        public override void Clear()
        {
            Status = (HttpStatusCode)0;
            Version = tools.HTTP11;
            Message = null;
            Body = null;
            base.Clear();
        }

        internal byte[] GetBytes()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ResponseHeader);
            foreach (string key in this.AllKeys)
            {
                string value = this[key];
                if (string.IsNullOrEmpty(value)) continue;
                sb.Append(key);
                sb.Append(": ");
                sb.Append(value);
                sb.AppendLine();
            }
            sb.AppendLine();
            HttpResponse = sb.ToString();
            Body = Body ?? new byte[0];
            this.Bytes = new byte[Encoding.UTF8.GetByteCount(HttpResponse) + Body.Length];
            Array.Copy(Body, 0, Bytes, Encoding.UTF8.GetBytes(HttpResponse, 0, HttpResponse.Length, Bytes, 0), Body.Length);
            return this.Bytes;
        }
    }
    sealed class Hybi13WebSocketResponse : WebSocketResponse
    {
        static SHA1 sha = SHA1.Create();
        const string magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        public static bool Handshake(WebSocket_ ws, WebSocketRequest_ request, WebSocketResponse_ response)
        {
            if ((request.SecWebSocketVersion ?? request.SecWebSocketDraft ?? request.SecWebSocketKey1).compares("13") == false)
                return false;
            string key = request.SecWebSocketKey;
            if (string.IsNullOrEmpty(key))
            {
                response.Status = HttpStatusCode.BadRequest;
                return false;
            }
            byte[] key1 = Encoding.ASCII.GetBytes(key + magic);
            byte[] key2 = sha.ComputeHash(key1);
            key = Convert.ToBase64String(key2);
            response.Status = HttpStatusCode.SwitchingProtocols;
            response.Message = "Switching Protocols";
            response.Upgrade = "WebSocket";
            response.Connection = "Upgrade";
            response.SecWebSocketAccept = key;
            return true;
        }
    }
    sealed class Draft76WebSocketResponse : WebSocketResponse
    {
        static MD5 md5 = MD5.Create();
        static string nums = "0123456789";
        public static bool Handshake(WebSocket_ ws, WebSocketRequest_ request, WebSocketResponse_ response)
        {
            string seckey1, seckey2;
            uint space1, space2;
            ulong key1, key2;
            uint s1, s2;
            if (string.IsNullOrEmpty(seckey1 = request.SecWebSocketKey1))
                return false;
            if (string.IsNullOrEmpty(seckey2 = request.SecWebSocketKey2))
                return false;
            if ((request.Body ?? tools.null_bytes).Length != 8)
                return false;
            if (!parse_key(seckey1, out space1, out key1, out s1))
                return false;
            if (!parse_key(seckey2, out space2, out key2, out s2))
                return false;
            byte[] key3 = new byte[16];
            for (int i = 3; i >= 0; i--, s1 >>= 8)
                key3[i + 0] = (byte)s1;
            for (int i = 3; i >= 0; i--, s2 >>= 8)
                key3[i + 4] = (byte)s2;
            for (int i = 0; i < 8; i++)
                key3[i + 8] = request.Body[i];
            byte[] responseKey = md5.ComputeHash(key3);

            response.Status = HttpStatusCode.SwitchingProtocols;
            response.Message = "WebSocket Protocol Handshake";
            response.Upgrade = "WebSocket";
            response.Connection = "Upgrade";
            response.SecWebSocketOrigin = request.Origin;
            response.SecWebSocketLocation = string.Format("ws://{0}{1}", request.Host, request.Path);
            response.SecWebSocketProtocol = request.WebSocketProtocol;
            response.Body = responseKey;
            return true;
        }
        static bool parse_key(string seckey, out uint space, out ulong key, out uint s)
        {
            space = 0;
            key = 0;
            s = 0;
            for (int i = 0; i < seckey.Length; i++)
            {
                char c = seckey[i];
                int n = nums.IndexOf(c);
                if (n >= 0)
                {
                    key *= 10;
                    key += (ulong)n;
                }
                else if (c == ' ')
                    space++;
            }
            if (space != 0)
            {
                if ((key % space) == 0)
                {
                    s = (uint)(key / (ulong)space);
                    return true;
                }
            }
            return false;
        }
    }

    public abstract class WebSocket
    {
    }
    abstract class WebSocket<TWebSocket, TFrame> : WebSocket
        where TWebSocket : WebSocket
        where TFrame : Frame
    {
    }
    class Hybi13WebSocket : WebSocket<Hybi13WebSocket, Hybi13Frame>
    {
    }
    class Draft76WebSocket : WebSocket<Draft76WebSocket, Draft76Frame>
    {
    }


    public sealed class WebSocket_ : IDisposable
    {
        internal readonly WebSocketServer server;
        readonly Socket socket;
        public IPEndPoint LocalEndPoint { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
        internal DateTime begin_time;

        Protocol m_Protocol;
        internal Protocol Protocol
        {
            [DebuggerStepThrough]
            get { return Interlocked.CompareExchange(ref m_Protocol, null, null); }
            [DebuggerStepThrough]
            set { Interlocked.Exchange(ref m_Protocol, value); }
        }

        internal WebSocket_(WebSocketServer server, Socket socket)
        {
            this.begin_time = DateTime.Now;
            this.server = server;
            this.socket = socket;
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            this.LocalEndPoint = (IPEndPoint)socket.LocalEndPoint;
            this.RemoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            this.isAlive = this;
            this.m_Protocol = HandshakeProtocol.Instance;
            HandshakeProtocol.Instance.BeginReceive(this, new byte[1]);
            Tick.OnTick += tick;
        }
		
        internal int handshake_offset;

        #region Close

        object isAlive;
        public bool Disposed
        {
            [DebuggerStepThrough]
            get { return Interlocked.CompareExchange(ref isAlive, null, null) == null; }
        }
        public bool Alive
        {
            [DebuggerStepThrough]
            get { return Interlocked.CompareExchange(ref isAlive, null, null) != null; }
        }

        void IDisposable.Dispose()
        {
            if (Interlocked.Exchange(ref isAlive, null) == null)
                return;
            Tick.OnTick -= tick;
            using (socket)
            {
                try { socket.Shutdown(SocketShutdown.Both); }
                catch { }
                try
                {
                    socket.Close();
                    log.message("ws", "{0} disconnected", this.RemoteEndPoint.Address);
                    DisconnectedEvent(this);
                }
                catch { }
            }
        }

        public void Close()
        {
            using (this) return;
        }
        public void Close(double? timeout)
        {
            if (timeout.HasValue)
                Interlocked.Exchange(ref disconnect_time, DateTime.Now.AddMilliseconds(timeout.Value).ToBinary());
            else
                Interlocked.Exchange(ref disconnect_time, 0);
        }

        #endregion

        long disconnect_time;
        //DateTime ping = DateTime.Now;
        bool tick()
        {
            if (Disposed) return false;
            long disconnect = Interlocked.CompareExchange(ref disconnect_time, 0, 0);
            if (disconnect != 0)
            {
                DateTime time = DateTime.FromBinary(disconnect);
                if (time > DateTime.Now)
                    using (this)
                        return false;
            }
            Protocol.Tick(this);
            return true;
            //if (Protocol is HandshakeProtocol)
            //{
            //    //handshake(null);
            //}
            //else
            //{
            //    if (DateTime.Now > ping)
            //    {
            //        ping = DateTime.Now.AddMilliseconds(500);
            //        this.SendPing(1, 2, 3);
            //    }
            //}
        }

        internal void recv(byte[] buffer, int offset, int size, object state)
        {
            try { if (Alive) socket.BeginReceive(buffer, offset, size, SocketFlags.None, recv, state); }
            catch (Exception ex) { OnError(ex, null); }
        }
        void recv(IAsyncResult ar)
        {
            DateTime time = DateTime.Now;
            try
            {
                if (Disposed) return;
                int count = socket.EndReceive(ar);
                if (count == 0) using (this) return;
                object state = ar.AsyncState;
                Protocol.EndReceive(this, time, ref state, count, socket.Available);
                if (Disposed) return;
                Protocol.BeginReceive(this, state);
            }
            catch (Exception ex) { OnError(ex, null); using (this) return; }
        }

        object send_busy;
        internal void send(byte[] buffer, int offset, int size, object state)
        {
            try
            {
                if (Alive && (Interlocked.CompareExchange(ref send_busy, buffer, null) == null))
                    socket.BeginSend(buffer, offset, size, SocketFlags.None, send, state);
            }
            catch (Exception ex) { OnError(ex, null); }
        }
        void send(IAsyncResult ar)
        {
            try
            {
                if (Disposed) return;
                int count = socket.EndSend(ar);
                if (count == 0) using (this) return;
                Protocol.EndSend(this, ar.AsyncState, count);
                Interlocked.Exchange(ref send_busy, null);
                if (Disposed) return;
                Protocol.BeginSend(this, ar.AsyncState);
            }
            catch (Exception ex) { OnError(ex, null); using (this) return; }
        }



        private EventHandler e_Disconnected;
        public EventHandler DisconnectedEvent
        {
            get { return Interlocked.CompareExchange(ref e_Disconnected, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_Disconnected, value); }
        }
        public event EventHandler Disconnected
        {
            add { DisconnectedEvent = value; }
            remove { DisconnectedEvent = null; }
        }

        #region Handshake Events

        private HandshakeHandler e_HandshakeRequest;
        public HandshakeHandler HandshakeRequestEvent
        {
            get { return Interlocked.CompareExchange(ref e_HandshakeRequest, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_HandshakeRequest, value); }
        }
        public event HandshakeHandler HandshakeRequest
        {
            add { HandshakeRequestEvent = value; }
            remove { HandshakeRequestEvent = null; }
        }

        private HandshakeHandler e_HandshakeSuccess;
        public HandshakeHandler HandshakeSuccessEvent
        {
            get { return Interlocked.CompareExchange(ref e_HandshakeSuccess, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_HandshakeSuccess, value); }
        }
        public event HandshakeHandler HandshakeSuccess
        {
            add { HandshakeSuccessEvent = value; }
            remove { HandshakeSuccessEvent = null; }
        }

        private HandshakeHandler e_HandshakeFailed;
        public HandshakeHandler HandshakeFailedEvent
        {
            get { return Interlocked.CompareExchange(ref e_HandshakeFailed, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_HandshakeFailed, value); }
        }
        public event HandshakeHandler HandshakeFailed
        {
            add { HandshakeFailedEvent = value; }
            remove { HandshakeFailedEvent = null; }
        }

        private EventHandler e_HandshakeTimeout;
        public EventHandler HandshakeTimeoutEvent
        {
            get { return Interlocked.CompareExchange(ref e_HandshakeTimeout, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_HandshakeTimeout, value); }
        }
        public event EventHandler HandshakeTimeout
        {
            add { HandshakeTimeoutEvent = value; }
            remove { HandshakeTimeoutEvent = null; }
        }

        #endregion

        internal object OnError(Exception ex, string message)
        {
            return null;
        }

        internal object OnError(string message)
        {
            log.message("ws", "Error\t{0}", message);
            return null;
        }

        #region Receive Frame

        private FrameHandler e_FrameReady;
        public FrameHandler FrameReadyEvent
        {
            get { return Interlocked.CompareExchange(ref e_FrameReady, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_FrameReady, value); }
        }
        public event FrameHandler FrameReady
        {
            add { FrameReadyEvent = value; }
            remove { FrameReadyEvent = null; }
        }

        private FrameHandler e_ReceiveFrame;
        public FrameHandler ReceiveFrameEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceiveFrame, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_ReceiveFrame, value); }
        }
        public event FrameHandler ReceiveFrame
        {
            add { ReceiveFrameEvent = value; }
            remove { ReceiveFrameEvent = null; }
        }

        private FrameHandler<string> e_ReceiveText;
        public FrameHandler<string> ReceiveTextEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceiveText, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_ReceiveText, value); }
        }
        public event FrameHandler<string> ReceiveText
        {
            add { ReceiveTextEvent = value; }
            remove { ReceiveTextEvent = null; }
        }

        private FrameHandler<byte[]> e_ReceiveBinary;
        public FrameHandler<byte[]> ReceiveBinaryEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceiveBinary, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_ReceiveBinary, value); }
        }
        public event FrameHandler<byte[]> ReceiveBinary
        {
            add { ReceiveBinaryEvent = value; }
            remove { ReceiveBinaryEvent = null; }
        }

        private FrameHandler<byte[]> e_ReceivePing;
        public FrameHandler<byte[]> ReceivePingEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceivePing, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_ReceivePing, value); }
        }
        public event FrameHandler<byte[]> ReceivePing
        {
            add { ReceivePingEvent = value; }
            remove { ReceivePingEvent = null; }
        }

        private FrameHandler<byte[]> e_ReceivePong;
        public FrameHandler<byte[]> ReceivePongEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceivePong, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_ReceivePong, value); }
        }
        public event FrameHandler<byte[]> ReceivePong
        {
            add { ReceivePongEvent = value; }
            remove { ReceivePongEvent = null; }
        }

        private FrameHandler<byte[]> e_ReceiveClose;
        public FrameHandler<byte[]> ReceiveCloseEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceiveClose, null, null) ?? tools._null; }
            set { Interlocked.Exchange(ref e_ReceiveClose, value); }
        }
        public event FrameHandler<byte[]> ReceiveClose
        {
            add { ReceiveCloseEvent = value; }
            remove { ReceiveCloseEvent = null; }
        }

        #endregion

        public void SendText(string text)
        {
            if (Alive) Protocol.SendFrame(this, null, FrameTypes.Text, Encoding.UTF8.GetBytes(text));
        }
        
        public void SendBinary(params byte[] data)
        {
            if (Alive) Protocol.SendFrame(this, null, FrameTypes.Binary, data);
        }
        
        public void SendPing(params byte[] data)
        {
            if (Alive) Protocol.SendFrame(this, null, FrameTypes.Ping, data);
        }
        
        public void SendPong(params byte[] data)
        {
            if (Alive) Protocol.SendFrame(this, null, FrameTypes.Pong, data);
        }

        public void SendClose(params byte[] data)
        {
            if (Alive) Protocol.SendFrame(this, null, FrameTypes.Close, data);
        }
    }

    [DebuggerStepThrough]
    abstract class Protocol
    {
        public virtual void Tick(WebSocket_ ws) { }
        public abstract void OnHandshakeCompleted(WebSocket_ ws, WebSocketRequest_ request, WebSocketResponse_ response);
        public abstract void BeginReceive(WebSocket_ ws, object state);
        public abstract void EndReceive(WebSocket_ ws, DateTime time, ref object state, int count, int available);
        public virtual void BeginSend(WebSocket_ ws, object state) { }
        public virtual void EndSend(WebSocket_ ws, object state, int count) { }
        public virtual void SendFrame(WebSocket_ ws, Frame frame, FrameTypes? type, byte[] data) { }
    }

    public enum FrameTypes : byte
    {
        Continuation = 0x00,
        Text = 0x01,
        Binary = 0x02,
        Close = 0x08,
        Ping = 0x09,
        Pong = 0x0A,
    }

    [DebuggerStepThrough]
    public abstract class Frame
    {
        internal byte[][] datas;
        public Frame(byte[][] datas) { this.datas = datas; }

        public virtual DateTime HeaderReady { get; set; }
        public virtual DateTime PacketReady { get; set; }
        public virtual FrameTypes FrameType { get; set; }
        internal virtual void PreProcess() { }

        public abstract byte[] Binary { get; set; }
        public virtual string Text
        {
            get { return Encoding.UTF8.GetString(this.Binary); }
            set { this.Binary = Encoding.UTF8.GetBytes(value); }
        }
    }

    sealed partial class HandshakeProtocol : Protocol
    {
        HandshakeProtocol() { }
        public static HandshakeProtocol Instance = new HandshakeProtocol();

        public override void Tick(WebSocket_ ws)
        {
            TimeSpan t = DateTime.Now - ws.begin_time;
            if (t.TotalMilliseconds > ws.server.HandshakeTimeoutValue)
                using (ws)
                {
                    try
                    {
                        log.message("ws", "{0}\thandshake timeout!", ws.RemoteEndPoint.Address);
                        ws.HandshakeTimeoutEvent(ws);
                    }
                    catch { }
                }
        }

        public override void BeginReceive(WebSocket_ ws, object state)
        {
            byte[] buffer = state as byte[];
            if (buffer == null) using (ws) return;
            if (buffer.Length <= ws.handshake_offset)
                Array.Resize(ref buffer, buffer.Length + tools.BLOCK_SIZE);
            ws.recv(buffer, ws.handshake_offset, buffer.Length - ws.handshake_offset, buffer);
        }

        public override void EndReceive(WebSocket_ ws, DateTime time, ref object state, int count, int available)
        {
            byte[] buffer = (byte[])state;
            ws.handshake_offset += count;
            WebSocketRequest_ request = WebSocketRequest_.Parse(buffer, ws.handshake_offset, available);
            if (request != null)
            {
                state = null;
                WebSocketResponse_ response = HandshakeProtocol.GetResponse(ws, request);
                byte[] data = response.GetBytes();
                ws.send(data, 0, data.Length, null);
                ws.Protocol.OnHandshakeCompleted(ws, request, response);
            }
        }

        public override void OnHandshakeCompleted(WebSocket_ ws, WebSocketRequest_ request, WebSocketResponse_ response)
        {
            try
            {
                log.message("ws", "{0}\t{1}", ws.RemoteEndPoint.Address, response.ResponseHeader);
                ws.Close(500);
                ws.HandshakeFailedEvent(ws, request, response);
            }
            catch { }
        }
    }

    abstract class Protocol<TFrame> : Protocol where TFrame : Frame, new()
    {
        Queue<TFrame> recv_queue = new Queue<TFrame>();
        Queue<TFrame> send_queue = new Queue<TFrame>();

        public override void OnHandshakeCompleted(WebSocket_ ws, WebSocketRequest_ request, WebSocketResponse_ response)
        {
            try
            {
                log.message("ws", "{0}\t{1}", ws.RemoteEndPoint.Address, response.ResponseHeader);
                ws.HandshakeSuccessEvent(ws, request, response);
            }
            catch { }
        }

        public override void BeginReceive(WebSocket_ ws, object state)
        {
            this.OnBeginReceive(ws, state);
            #region process receive queue
            if (Monitor.TryEnter(recv_queue))
            {
                TFrame frame;
                try
                {
                    if (recv_queue.Count == 0)
                        return;
                    frame = recv_queue.Dequeue();
                }
                finally { Monitor.Exit(recv_queue); }
                //frame.PreProcess();
                try
                {
                    ws.ReceiveFrameEvent(ws, frame);
                    string text;
                    byte[] binary;
                    switch (frame.FrameType)
                    {
                        case FrameTypes.Text:
                            ws.ReceiveTextEvent(ws, frame, text = frame.Text);
                            this.SendFrame(ws, frame, null, null);
                            log.message("ws", "ReceiveText, {0}", text);
                            break;
                        case FrameTypes.Binary:
                            ws.ReceiveBinaryEvent(ws, frame, binary = frame.Binary);
                            log.message("ws", "ReceiveBinary, len={0}", binary.Length);
                            break;
                        case FrameTypes.Ping:
                            ws.ReceivePingEvent(ws, frame, binary = frame.Binary);
                            log.message("ws", "ReceivePing, len={0}", binary.Length);
                            break;
                        case FrameTypes.Pong:
                            ws.ReceivePingEvent(ws, frame, binary = frame.Binary);
                            log.message("ws", "ReceivePing, len={0}", binary.Length);
                            break;
                        case FrameTypes.Close:
                            ws.ReceiveCloseEvent(ws, frame, binary = frame.Binary);
                            log.message("ws", "ReceiveClose, len={0}", binary.Length);
                            ws.Close(500);
                            break;
                        default: log.message("ws", "OnReceiveFrame"); break;
                    }
                }
                catch { }
            }
            #endregion
        }
        protected abstract void OnBeginReceive(WebSocket_ ws, object state);

        public override void EndReceive(WebSocket_ ws, DateTime time, ref object state, int count, int available)
        {
            int remain = count;
            while (remain > 0)
            {
                TFrame frame = this.OnEndReceive(ws, time, state, ref remain);
                if (frame != null)
                {
                    lock (recv_queue)
                        recv_queue.Enqueue(frame);
                    try { ws.FrameReadyEvent(ws, frame); }
                    catch { }
                }
            }
        }
        protected abstract TFrame OnEndReceive(WebSocket_ ws, DateTime time, object state, ref int count);

        public override void SendFrame(WebSocket_ ws, Frame frame, FrameTypes? type, byte[] data)
        {
            TFrame item = frame as TFrame;
            if (item == null)
                if (type.HasValue && (data != null))
                    item = new TFrame() { FrameType = type.Value };
            if (item == null)
                return;
            lock (send_queue)
                send_queue.Enqueue(item);
            this.BeginSend(ws, null);
        }

        protected abstract TFrame CreateFrame(FrameTypes type, byte[] data);

        TFrame send_frame;
        int send_index;
        int send_mode;
        
        public override void BeginSend(WebSocket_ ws, object state)
        {
            bool frame_start = false;
            TFrame frame = Interlocked.CompareExchange(ref send_frame, null, null);
            if (frame == null)
            {
                if (!Monitor.TryEnter(send_queue))
                    return;
                try
                {
                    if (send_queue.Count == 0)
                        return;
                    if (Interlocked.CompareExchange(ref send_frame, send_queue.Peek(), null) != null)
                        return;
                    frame = send_queue.Dequeue();
                    frame_start = true;
                }
                finally { Monitor.Exit(send_queue); }
            }
            Interlocked.CompareExchange(ref send_frame, this.OnBeginSend(ws, state, frame, frame_start), frame);
        }
        protected virtual TFrame OnBeginSend(WebSocket_ ws, object state, TFrame frame, bool frame_start)
        {
            if (frame_start)
                send_mode = 0;
            while (send_mode < frame.datas.Length)
            {
                while (frame.datas[send_mode].Length > 0)
                {
                    ws.send(frame.datas[send_mode], 0, frame.datas[send_mode].Length, null);
                    return frame;
                }
            }
            return null;
        }

        public override void EndSend(WebSocket_ ws, object state, int count)
        {
            TFrame frame = Interlocked.CompareExchange(ref send_frame, null, null);
            if (frame == null)
                return;
            Interlocked.CompareExchange(ref send_frame, this.OnEndSend(ws, state, frame, count), frame);
        }
        protected virtual TFrame OnEndSend(WebSocket_ ws, object state, TFrame frame, int count)
        {
            send_index += count;
            while (send_mode < frame.datas.Length)
            {
                while (send_index < frame.datas[send_mode].Length)
                    return frame;
                send_index = 0;
                send_mode++;
            }
            return null;
        }
    }

    // http://tools.ietf.org/html/rfc6455
    partial class Hybi13Protocol : Protocol<Hybi13Frame>
    {
        Hybi13Frame recv_part;
        DateTime recv_header_time;
        byte[][] recv_datas;
        int recv_mode;
        int recv_index;

        protected override void OnBeginReceive(WebSocket_ ws, object state)
        {
            if (recv_datas == null)
            {
                recv_datas = Hybi13Frame.datas_default();
                recv_mode = 0;
                recv_index = 0;
            }
            ws.recv(recv_datas[recv_mode], recv_index, recv_datas[recv_mode].Length - recv_index, null);
        }

        protected override Hybi13Frame OnEndReceive(WebSocket_ ws, DateTime time, object state, ref int count)
        {
            recv_index += count;
            count = 0;
            while (recv_index >= recv_datas[recv_mode].Length)
            {
                recv_index = 0;
                if (recv_mode == 0)
                {
                    recv_header_time = time;
                    byte opcode = Hybi13Frame._opcode(recv_datas[0]);
                    FrameTypes frameType = (FrameTypes)opcode;
                    if (!Enum.IsDefined(typeof(FrameTypes), opcode))
                        using (ws) { ws.OnError("未知的訊息類型"); break; }

                    bool fin = Hybi13Frame._FIN(recv_datas[0]);

                    if (fin) recv_index = 0;
                    else if ((frameType == FrameTypes.Ping) || (frameType == FrameTypes.Pong) || (frameType == FrameTypes.Close))
                        using (ws) { ws.OnError("控制訊息不能分段"); break; }
                    else if (frameType == FrameTypes.Continuation)
                    {
                        if (recv_part == null) using (ws) { ws.OnError("第一個分段訊息不存在"); break; }
                    }
                    else
                    {
                        if (recv_part != null) using (ws) { ws.OnError("已經有一個分段訊息了"); break; }
                    }

                    int PAYLOAD_LEN = Hybi13Frame._PAYLOAD_LEN(recv_datas[0]);
                    if (PAYLOAD_LEN == 127)
                        recv_datas[1] = new byte[8];
                    else if (PAYLOAD_LEN == 126)
                        recv_datas[1] = new byte[2];
                    else
                        recv_datas[3] = new byte[PAYLOAD_LEN];

                    bool mask = Hybi13Frame._MASK(recv_datas[0]);
                    if (mask) recv_datas[2] = new byte[4];
                }
                else if (recv_mode == 1)
                {
                    int PAYLOAD_LEN = Hybi13Frame._PAYLOAD_LEN(recv_datas[0]);
                    if (PAYLOAD_LEN >= 126)
                        recv_datas[3] = new byte[Hybi13Frame._EXT_PAYLOAD_LEN(recv_datas[1])];
                }
                else if (recv_mode == 2)
                {
                }
                else
                {
                    bool fin = Hybi13Frame._FIN(recv_datas[0]);
                    Hybi13Frame frame = new Hybi13Frame(recv_datas)
                    {
                        HeaderReady = recv_header_time,
                        PacketReady = time,
                    };
                    recv_datas = null;

                    if (frame.FIN)
                    {
                        if (recv_part != null)
                        {
                            recv_part.AddNext(frame);
                            frame = recv_part;
                            recv_part = null;
                        }
                        return frame;
                    }
                    else
                    {
                        if (recv_part != null) using (ws) { ws.OnError("已經有一個分段訊息了"); break; }
                        recv_part = frame;
                    }
                    break;
                }
                recv_mode++;
            }
            return null;
        }

        protected override Hybi13Frame CreateFrame(FrameTypes type, byte[] data)
        {
            return new Hybi13Frame()
            {
                FIN = true,
                FrameType = type,
                MASK = false,
                EXT_PAYLOAD_LEN = data.Length,
                Binary = data,
            };
        }
    }

    class Hybi13Frame : Frame
    {
        #region FIN
        public bool FIN
        {
            [DebuggerStepThrough]
            get { return Hybi13Frame._FIN(datas[0]); }
            [DebuggerStepThrough]
            set { Hybi13Frame._FIN(datas[0], value); }
        }
        [DebuggerStepThrough]
        public static bool _FIN(byte[] data)
        {
            return (data[0] & 0x80) != 0;
        }
        [DebuggerStepThrough]
        public static void _FIN(byte[] data, bool value)
        {
            if (value) data[0] |= 0x80; else data[0] &= 0x7f;
        }
        #endregion
        #region RSV1
        public bool RSV1
        {
            [DebuggerStepThrough]
            get { return Hybi13Frame._RSV1(datas[0]); }
            [DebuggerStepThrough]
            set { Hybi13Frame._RSV1(datas[0], value); }
        }
        [DebuggerStepThrough]
        public static bool _RSV1(byte[] data)
        {
            return (data[0] & 0x40) != 0;
        }
        [DebuggerStepThrough]
        public static void _RSV1(byte[] data, bool value)
        {
            if (value) data[0] |= 0x40; else data[0] &= 0xbf;
        }
        #endregion
        #region RSV2
        public bool RSV2
        {
            [DebuggerStepThrough]
            get { return Hybi13Frame._RSV2(datas[0]); }
            [DebuggerStepThrough]
            set { Hybi13Frame._RSV2(datas[0], value); }
        }
        [DebuggerStepThrough]
        public static bool _RSV2(byte[] data)
        {
            return (data[0] & 0x20) != 0;
        }
        [DebuggerStepThrough]
        public static void _RSV2(byte[] data, bool value)
        {
            if (value) data[0] |= 0x20; else data[0] &= 0xdf;
        }
        #endregion
        #region RSV3
        public bool RSV3
        {
            [DebuggerStepThrough]
            get { return Hybi13Frame._RSV3(datas[0]); }
            [DebuggerStepThrough]
            set { Hybi13Frame._RSV3(datas[0], value); }
        }
        [DebuggerStepThrough]
        public static bool _RSV3(byte[] data)
        {
            return (data[0] & 0x10) != 0;
        }
        [DebuggerStepThrough]
        public static void _RSV3(byte[] data, bool value)
        {
            if (value) data[0] |= 0x10; else data[0] &= 0xef;
        }
        #endregion
        #region FrameType (opcode)
        public override FrameTypes FrameType
        {
            [DebuggerStepThrough]
            get { return (FrameTypes)this.opcode; }
            [DebuggerStepThrough]
            set { this.opcode = (byte)value; }
        }
        public byte opcode
        {
            [DebuggerStepThrough]
            get { return Hybi13Frame._opcode(datas[0]); }
            [DebuggerStepThrough]
            set { Hybi13Frame._opcode(datas[0], value); }
        }
        [DebuggerStepThrough]
        public static byte _opcode(byte[] data)
        {
            byte n = data[0]; n &= 0x0f; return n;
        }
        [DebuggerStepThrough]
        public static void _opcode(byte[] data, byte value)
        {
            data[0] &= 0xf0; data[0] |= value;
        }
        #endregion
        #region MASK
        public bool MASK
        {
            [DebuggerStepThrough]
            get { return Hybi13Frame._MASK(datas[0]); }
            [DebuggerStepThrough]
            set { Hybi13Frame._MASK(datas[0], value); }
        }
        [DebuggerStepThrough]
        public static bool _MASK(byte[] data)
        {
            return (data[1] & 0x80) != 0;
        }
        [DebuggerStepThrough]
        public static void _MASK(byte[] data, bool value)
        {
            if (value) data[1] |= 0x80; else data[1] &= 0x7f;
        }
        #endregion
        #region PAYLOAD_LEN
        public int PAYLOAD_LEN
        {
            [DebuggerStepThrough]
            get { return Hybi13Frame._PAYLOAD_LEN(datas[0]); }
            [DebuggerStepThrough]
            set { Hybi13Frame._PAYLOAD_LEN(datas[0], value); }
        }
        [DebuggerStepThrough]
        public static int _PAYLOAD_LEN(byte[] data)
        {
            return (int)(data[1] & 0x7f);
        }
        [DebuggerStepThrough]
        public static void _PAYLOAD_LEN(byte[] data, int value)
        {
            byte mask = data[1];
            mask &= 0x80; data[1] = (byte)value;
            data[1] &= 0x7f;
            data[1] |= mask;
        }
        #endregion
        #region EXT_PAYLOAD_LEN
        public int EXT_PAYLOAD_LEN
        {
            [DebuggerStepThrough]
            get { return Hybi13Frame._EXT_PAYLOAD_LEN(datas[1]); }
            [DebuggerStepThrough]
            set { Hybi13Frame._EXT_PAYLOAD_LEN(datas[0], datas[1], value); }
        }
        [DebuggerStepThrough]
        public static int _EXT_PAYLOAD_LEN(byte[] payload)
        {
            uint len = 0;
            for (int i = 0; i < payload.Length; i++)
            {
                len <<= 8;
                len |= payload[i];
            }
            return (int)len;
        }
        [DebuggerStepThrough]
        public static void _EXT_PAYLOAD_LEN(byte[] header, byte[] payload, int value)
        {
            int size;
            if (value >= 32768)
            {
                size = 8;
                _PAYLOAD_LEN(header, 127);
            }
            else if (value >= 126)
            {
                size = 2;
                _PAYLOAD_LEN(header, 126);
            }
            else
            {
                size = 0;
                _PAYLOAD_LEN(header, value);
            }
            if (payload.Length != size)
                payload = new byte[size];
            for (int i = size - 1, n = value; i >= 0; i--)
            {
                payload[i] = (byte)n;
                n <<= 8;
            }
        }
        #endregion

        public static byte[][] datas_default()
        {
            return new byte[][] { new byte[2], tools.null_bytes, tools.null_bytes, tools.null_bytes };
        }

        public Hybi13Frame(byte[][] datas) : base(datas) { }
        public Hybi13Frame() : base(datas_default()) { }

        byte[] m_MaskingKey_old;

        public byte[] MaskingFrame(bool value)
        {
            if (m_MaskingKey_old == null)
                m_MaskingKey_old = datas[2];

            if (MASK == value)
                return datas[3];

            if ((m_MaskingKey_old ?? tools.null_bytes).Length != 4)
                m_MaskingKey_old = tools.RandomBytes(4);
            byte[] key;
            MASK = value;
            if (value)
            {
                key = datas[2] = m_MaskingKey_old;
            }
            else
            {
                key = m_MaskingKey_old;
                datas[2] = tools.null_bytes;
            }

            int len1 = key.Length;
            int len2 = datas[3].Length;
            for (int i = 0; i < len1; i++)
            {
                byte mask = key[i];
                for (int j = i; j < len2; j += len1)
                    datas[3][j] ^= mask;
            }
            return datas[3];
        }

        Hybi13Frame next;

        public void AddNext(Hybi13Frame frame)
        {
            if (next == null)
                next = frame;
            else
                next.AddNext(frame);
        }

        public override byte[] Binary
        {
            get { return MaskingFrame(false); }
            set
            {
                this.MASK = false;
                this.datas[3] = value ?? tools.null_bytes;
            }
        }
    }

    // http://tools.ietf.org/html/draft-ietf-hybi-thewebsocketprotocol-17
    partial class Draft76Protocol : Protocol<Draft76Frame>
    {
        DateTime? recv_start;
        byte[] recv_buf;
        List<byte[]> recv_datas = new List<byte[]>();
        int recv_index;

        protected override void OnBeginReceive(WebSocket_ ws, object state)
        {
            if (recv_buf == null)
            {
                recv_buf = new byte[tools.BLOCK_SIZE];
                recv_index = 0;
            }
            ws.recv(recv_buf, recv_index, recv_buf.Length - recv_index, state);
        }

        protected override Draft76Frame OnEndReceive(WebSocket_ ws, DateTime time, object state, ref int count)
        {
            int start = recv_index;
            Draft76Frame frame = null;
            for (int end = recv_index + count; recv_index < end; )
            {
                byte b = recv_buf[recv_index];
                recv_index++;
                count--;
                if (b == tools.StartByte)
                {
                    start = recv_index - 1;
                    recv_start = time;
                }
                else if (b == tools.EndByte)
                {
                    if (recv_start.HasValue)
                    {
                        frame = new Draft76Frame() { HeaderReady = recv_start.Value, PacketReady = time, FrameType = FrameTypes.Text, };
                        recv_start = null;
                        break;
                    }
                }
            }
            int len = recv_index - start;
            if (len == recv_buf.Length)
            {
                recv_datas.Add(recv_buf);
                recv_buf = null;
            }
            else
            {
                byte[] tmp = new byte[len];
                Array.Copy(recv_buf, start, tmp, 0, len);
                if (recv_index >= recv_buf.Length)
                    recv_index = 0;
                recv_datas.Add(tmp);
            }
            if (frame != null)
            {
                frame.datas = recv_datas.ToArray();
                recv_datas.Clear();
            }
            return frame;
        }

        protected override Draft76Frame CreateFrame(FrameTypes type, byte[] data)
        {
            return new Draft76Frame() { FrameType = type, Binary = data };
        }
    }

    class Draft76Frame : Frame
    {
        public Draft76Frame() : base(null) { }
        public Draft76Frame(byte[][] datas) : base(datas) { }

        public override byte[] Binary
        {
            get
            {
                return tools.null_bytes;
            }
            set { datas = new byte[][] { tools.StartBytes, value, tools.EndBytes, }; }
        }

        public unsafe override string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int l = default(int);
                char c = default(char);
                for (int n = 0; n < datas.Length; n++)
                {
                    int start = 0;
                    int size = datas[n].Length;
                    if (n == 0) start++;
                    if (n == (datas.Length - 1)) size--;
                    sb.UTF8Decode(datas[n], start, size, ref c, ref l);
                }
                return sb.ToString();
            }
            set { this.Binary = Encoding.UTF8.GetBytes(value); }
        }
    }

    partial class HandshakeProtocol
    {
        public static WebSocketResponse_ GetResponse(WebSocket_ ws, WebSocketRequest_ request)
        {
            WebSocketResponse_ response = new WebSocketResponse_();
            try
            {
                log.message("ws", "{0}\t{1} {2} {3}", ws.RemoteEndPoint.Address, request.Method, request.Path, request.Version);
                response.Status = HttpStatusCode.SwitchingProtocols;
                ws.HandshakeRequestEvent(ws, request, response);
                if (response.Status == HttpStatusCode.SwitchingProtocols)
                {
                    response.Clear();
                    if (string.IsNullOrEmpty(request.Method) || string.IsNullOrEmpty(request.Path) || string.IsNullOrEmpty(request.Version))
                        response.Status = HttpStatusCode.BadRequest;
                    else if (request.Method.compares("GET") == false)
                        response.Status = HttpStatusCode.MethodNotAllowed;
                    else if (request.Version.compares(tools.HTTP11) == false)
                        response.Status = HttpStatusCode.HttpVersionNotSupported;
                    else if (request.Connection.contains("Upgrade") == false)
                        response.Status = HttpStatusCode.BadRequest;
                    else if (request.Upgrade.compares("WebSocket") == false)
                        response.Status = HttpStatusCode.BadRequest;
                    else
                    {
                        try
                        {
                            if (Hybi13Protocol.Handshake(ws, request, response))
                                ws.Protocol = new Hybi13Protocol();
                            else if (Draft76Protocol.Handshake(ws, request, response))
                                ws.Protocol = new Draft76Protocol();
                            else
                                response.Status = HttpStatusCode.NotImplemented;
                        }
                        catch
                        {
                            response.Status = HttpStatusCode.InternalServerError;
                        }
                    }
                }
            }
            catch
            {
                response.Clear();
                response.Status = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
    partial class Hybi13Protocol
    {
        static SHA1 sha = SHA1.Create();
        const string magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        public static bool Handshake(WebSocket_ ws, WebSocketRequest_ request, WebSocketResponse_ response)
        {
            if ((request.SecWebSocketVersion ?? request.SecWebSocketDraft ?? request.SecWebSocketKey1).compares("13") == false)
                return false;
            string key = request.SecWebSocketKey;
            if (string.IsNullOrEmpty(key))
            {
                response.Status = HttpStatusCode.BadRequest;
                return false;
            }
            byte[] key1 = Encoding.ASCII.GetBytes(key + magic);
            byte[] key2 = sha.ComputeHash(key1);
            key = Convert.ToBase64String(key2);
            response.Status = HttpStatusCode.SwitchingProtocols;
            response.Message = "Switching Protocols";
            response.Upgrade = "WebSocket";
            response.Connection = "Upgrade";
            response.SecWebSocketAccept = key;
            return true;
        }
    }
    partial class Draft76Protocol
    {
        static MD5 md5 = MD5.Create();
        static string nums = "0123456789";
        public static bool Handshake(WebSocket_ ws, WebSocketRequest_ request, WebSocketResponse_ response)
        {
            string seckey1, seckey2;
            uint space1, space2;
            ulong key1, key2;
            uint s1, s2;
            if (string.IsNullOrEmpty(seckey1 = request.SecWebSocketKey1))
                return false;
            if (string.IsNullOrEmpty(seckey2 = request.SecWebSocketKey2))
                return false;
            if ((request.Body ?? tools.null_bytes).Length != 8)
                return false;
            if (!parse_key(seckey1, out space1, out key1, out s1))
                return false;
            if (!parse_key(seckey2, out space2, out key2, out s2))
                return false;
            byte[] key3 = new byte[16];
            for (int i = 3; i >= 0; i--, s1 >>= 8)
                key3[i + 0] = (byte)s1;
            for (int i = 3; i >= 0; i--, s2 >>= 8)
                key3[i + 4] = (byte)s2;
            for (int i = 0; i < 8; i++)
                key3[i + 8] = request.Body[i];
            byte[] responseKey = md5.ComputeHash(key3);

            response.Status = HttpStatusCode.SwitchingProtocols;
            response.Message = "WebSocket Protocol Handshake";
            response.Upgrade = "WebSocket";
            response.Connection = "Upgrade";
            response.SecWebSocketOrigin = request.Origin;
            response.SecWebSocketLocation = string.Format("ws://{0}{1}", request.Host, request.Path);
            response.SecWebSocketProtocol = request.WebSocketProtocol;
            response.Body = responseKey;
            return true;
        }
        static bool parse_key(string seckey, out uint space, out ulong key, out uint s)
        {
            space = 0;
            key = 0;
            s = 0;
            for (int i = 0; i < seckey.Length; i++)
            {
                char c = seckey[i];
                int n = nums.IndexOf(c);
                if (n >= 0)
                {
                    key *= 10;
                    key += (ulong)n;
                }
                else if (c == ' ')
                    space++;
            }
            if (space != 0)
            {
                if ((key % space) == 0)
                {
                    s = (uint)(key / (ulong)space);
                    return true;
                }
            }
            return false;
        }
    }

    public sealed class WebSocketRequest_ : NameValueCollection
    {
        public string this[HttpRequestHeader name]
        {
            get { return base[name.ToString()]; }
            set { base[name.ToString()] = value; }
        }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Upgrade
        {
            get { return this[HttpRequestHeader.Upgrade]; }
            set { this[HttpRequestHeader.Upgrade] = value; }
        }
        public string Connection
        {
            get { return this[HttpRequestHeader.Connection]; }
            set { this[HttpRequestHeader.Connection] = value; }
        }
        public string Host
        {
            get { return this[HttpRequestHeader.Host]; }
            set { this[HttpRequestHeader.Host] = value; }
        }
        public string Origin
        {
            get { return this["Origin"]; }
            set { this["Origin"] = value; }
        }
        public string SecWebSocketKey
        {
            get { return this[tools.SecWebSocketKey]; }
            set { this[tools.SecWebSocketKey] = value; }
        }
        public string SecWebSocketVersion
        {
            get { return this[tools.SecWebSocketVersion]; }
            set { this[tools.SecWebSocketVersion] = value; }
        }
        public string SecWebSocketExtensions
        {
            get { return this[tools.SecWebSocketExtensions]; }
            set { this[tools.SecWebSocketExtensions] = value; }
        }
        public string SecWebSocketDraft
        {
            get { return this["Sec-WebSocket-Draft"]; }
            set { this["Sec-WebSocket-Draft"] = value; }
        }
        public string SecWebSocketKey1
        {
            get { return this[tools.SecWebSocketKey1]; }
            set { this[tools.SecWebSocketKey1] = value; }
        }
        public string SecWebSocketKey2
        {
            get { return this[tools.SecWebSocketKey2]; }
            set { this[tools.SecWebSocketKey2] = value; }
        }
        public string SecWebSocketKey3
        {
            get { return this[tools.SecWebSocketKey3]; }
            set { this[tools.SecWebSocketKey3] = value; }
        }
        public string WebSocketProtocol
        {
            get { return this[tools.WebSocketProtocol]; }
            set { this[tools.WebSocketProtocol] = value; }
        }

        public byte[] Body { get; set; }
        public byte[] Bytes { get; set; }
        public string HttpRequest { get; set; }

        internal static WebSocketRequest_ Parse(byte[] buffer, int length, int available)
        {
            if (length < 4)
                return null;
            for (int n1 = length - 4; n1 >= 0; n1--)
            {
                if (buffer[n1 + 0] != '\r') continue;
                if (buffer[n1 + 1] != '\n') continue;
                if (buffer[n1 + 2] != '\r') continue;
                if (buffer[n1 + 3] != '\n') continue;
                if (available == 0)
                {
                    n1 += 4;
                    int http_len = n1;
                    string http = Encoding.UTF8.GetString(buffer, 0, http_len);
                    int body_len = length - n1;
                    if (body_len < 0) body_len = 0;
                    byte[] body = new byte[body_len];
                    for (int n2 = 0; n1 < length; n1++, n2++)
                        body[n2] = buffer[n1];
                    Array.Resize(ref buffer, http_len);
                    int pos = 0;
                    WebSocketRequest_ request = new WebSocketRequest_()
                    {
                        Method = http.ReadTo(ref pos, ' '),
                        Path = http.ReadTo(ref pos, ' '),
                        Version = http.ReadLine(ref pos),
                        HttpRequest = http,
                        Body = body,
                        Bytes = buffer,
                    };
                    string key, value;
                    for (; ; )
                    {
                        if ((key = http.ReadTo(ref pos, ':')) == null) break;
                        if ((value = http.ReadLine(ref pos)) == null) break;
                        request[key] = value;
                    }
                    return request;
                }
            }
            return null;
        }
    }

    public sealed class WebSocketResponse_ : NameValueCollection
    {
        public string this[HttpResponseHeader name]
        {
            get { return base[name.ToString()]; }
            set { base[name.ToString()] = value; }
        }
        public string Version { get; set; }
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public string Upgrade
        {
            get { return this[HttpResponseHeader.Upgrade]; }
            set { this[HttpResponseHeader.Upgrade] = value; }
        }
        public string Connection
        {
            get { return this[HttpResponseHeader.Connection]; }
            set { this[HttpResponseHeader.Connection] = value; }
        }
        public string SecWebSocketAccept
        {
            get { return this[tools.SecWebSocketAccept]; }
            set { this[tools.SecWebSocketAccept] = value; }
        }
        public string SecWebSocketOrigin
        {
            get { return this[tools.SecWebSocketOrigin]; }
            set { this[tools.SecWebSocketOrigin] = value; }
        }
        public string SecWebSocketLocation
        {
            get { return this[tools.SecWebSocketLocation]; }
            set { this[tools.SecWebSocketLocation] = value; }
        }
        public string SecWebSocketProtocol
        {
            get { return this[tools.SecWebSocketProtocol]; }
            set { this[tools.SecWebSocketProtocol] = value; }
        }

        public byte[] Body { get; set; }
        public byte[] Bytes { get; set; }
        public string HttpResponse { get; set; }

        public string ResponseHeader
        {
            get
            {
                if (string.IsNullOrEmpty(Message))
                    return string.Format("{0} {1} {2}", Version, (int)Status, Status);
                else
                    return string.Format("{0} {1} {2}", Version, (int)Status, Message);
            }
        }

        public WebSocketResponse_() { this.Clear(); }
        public override void Clear()
        {
            Status = (HttpStatusCode)0;
            Version = tools.HTTP11;
            Message = null;
            Body = null;
            base.Clear();
        }

        internal byte[] GetBytes()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ResponseHeader);
            foreach (string key in this.AllKeys)
            {
                string value = this[key];
                if (string.IsNullOrEmpty(value)) continue;
                sb.Append(key);
                sb.Append(": ");
                sb.Append(value);
                sb.AppendLine();
            }
            sb.AppendLine();
            HttpResponse = sb.ToString();
            Body = Body ?? new byte[0];
            this.Bytes = new byte[Encoding.UTF8.GetByteCount(HttpResponse) + Body.Length];
            Array.Copy(Body, 0, Bytes, Encoding.UTF8.GetBytes(HttpResponse, 0, HttpResponse.Length, Bytes, 0), Body.Length);
            return this.Bytes;
        }

        internal static WebSocketResponse_ GetResponse(WebSocket_ ws, WebSocketRequest_ request)
        {
            return null;
        }
    }


    static class tools
    {
        public static void _null() { }
        public static void _null<T>(T t) { }
        public static void _null<T1, T2>(T1 t, T2 t2) { }
        public static void _null<T1, T2, T3>(T1 t1, T2 t2, T3 t3) { }

        internal const int BLOCK_SIZE = 32;

        // Char. number range  |        UTF-8 octet sequence
        //    (hexadecimal)    |              (binary)
        // --------------------+---------------------------------------------
        // 00000000-0000007F | 0xxxxxxx
        // 00000080-000007FF | 110xxxxx 10xxxxxx
        // 00000800-0000FFFF | 1110xxxx 10xxxxxx 10xxxxxx
        // 00010000-0010FFFF | 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx

        static byte[] mask1 = new byte[] { 0x80, 0xc0, 0xe0, 0xf0, 0xf8 };
        static byte[] mask2 = new byte[] { 0x7f, 0x3f, 0x1f, 0x0f, 0x07 };

        public static unsafe bool UTF8Decode(byte b, ref char c, ref int c_index)
        {
            if (b < 0x80)
            {
                c = (char)b;
                c_index = 0;
                return true;
            }
            for (int i = 0; i < mask1.Length; i++)
            {
                if ((b & mask1[i]) == 0)
                {
                    if (i == 0)
                    {
                        c = (char)b;
                        c_index = 0;
                        return true;
                    }
                    if (i == 1)
                    {
                        if (c_index > 0)
                        {
                            c <<= 6;
                            c |= (char)(b & mask2[i]);
                            return --c_index == 0;
                        }
                    }
                    else
                    {
                        c = (char)(b & mask2[i]);
                        c_index = i;
                    }
                    break;
                }
            }
            return false;
        }

        public static unsafe void UTF8Decode(this StringBuilder sb, byte[] bytes, int start, int length, ref char c, ref int c_index)
        {
            fixed (byte* ptr = bytes)
            {
                byte b;
                byte m1;
                byte m2;
                int i;
                for (byte* p0 = ptr, p1 = ptr + length; p0 < p1; p0++)
                {
                    if (*p0 < 0x80)
                    {
                        sb.Append((char)*p0);
                        c_index = 0;
                        continue;
                    }
                    m1 = 0x80;
                    m2 = 0x7f;
                    for (i = -1; i < 4; m1 >>= 1, m2 >>= 1, i++)
                    {
                        b = *p0;
                        b &= m1;
                        if (b == 0)
                            break;
                    }
                    b = *p0;
                    b &= m2;
                    // 0xxx xxxx -> i=0, ascii
                    // 10xx xxxx -> i=1, part
                    // 110x xxxx -> i=2, 2
                    // 1110 xxxx -> i=3, 3
                    // 1111 0xxx -> i=4. 4
                    if (i == 0)
                    {
                        if (c_index > 0)
                        {
                            c <<= 6;
                            c |= (char)b;
                            (c_index)--;
                            if (c_index == 0)
                                sb.Append(c);
                        }
                    }
                    //else if (i == -1)
                    //{
                    //    sb.Append((char)_byte);
                    //    c_index = 0;
                    //}
                    else if (i < 4)
                    {
                        c = (char)b;
                        c_index = i;
                    }
                }
            }
        }

        internal static RandomNumberGenerator rng = RNGCryptoServiceProvider.Create();


        internal static byte[] null_bytes = new byte[0];

        internal static byte[] RandomBytes(int count)
        {
            byte[] bytes = new byte[count];
            rng.GetBytes(bytes);
            return bytes;
        }

        public static string ReadTo(this string str, ref int pos, char c)
        {
            int start, n;
            for (start = pos, n = str.Length; pos < n; pos++)
                if (str[pos] == c)
                    return str.Substring(start, pos++ - start);
            return null;
        }
        public static string ReadLine(this string str, ref int pos)
        {
            int start = pos;
            int n = str.Length - 1;
            char c;
            for (; pos < n; pos++)
            {
                c = str[pos];
                if ((pos == start) && (c == ' '))
                    start++;
                if ((c == '\r') && (str[pos + 1] == '\n'))
                {
                    string s = str.Substring(start, pos++ - start);
                    pos++;
                    return s;
                }
            }
            return null;
        }

        public static string ReadTo(this StringBuilder sb, ref int pos, char c)
        {
            int start, n;
            for (start = pos, n = sb.Length; pos < n; pos++)
                if (sb[pos] == c)
                    return sb.ToString(start, pos++ - start);
            return null;
        }
        public static string ReadLine(this StringBuilder sb, ref int pos)
        {
            int start = pos;
            int n = sb.Length - 1;
            char c;
            for (; pos < n; pos++)
            {
                c = sb[pos];
                if ((pos == start) && (c == ' '))
                    start++;
                if ((c == '\r') && (sb[pos + 1] == '\n'))
                {
                    string s = sb.ToString(start, pos++ - start);
                    pos++;
                    return s;
                }
            }
            return null;
        }

        public static bool compares(this string a, string b)
        {
            return string.Compare(a, b, true) == 0;
        }
        public static bool contains(this string a, string b)
        {
            if (a == null) return false;
            if (b == null) return false;
            return a.ToLower().Contains(b.ToLower());
        }

        public const string Host = "Host";
        public const string Connection = "Connection";
        public const string SecWebSocketAccept = "Sec-WebSocket-Accept";
        public const string SecWebSocketKey1 = "Sec-WebSocket-Key1";
        public const string SecWebSocketKey2 = "Sec-WebSocket-Key2";
        public const string SecWebSocketKey3 = "Sec-WebSocket-Key3";
        public const string SecWebSocketKey = "Sec-WebSocket-Key";
        public const string SecWebSocketOrigin = "Sec-WebSocket-Origin";
        public const string SecWebSocketLocation = "Sec-WebSocket-Location";
        public const string SecWebSocketVersion = "Sec-WebSocket-Version";
        public const string SecWebSocketProtocol = "Sec-WebSocket-Protocol";
        public const string SecWebSocketExtensions = "Sec-WebSocket-Extensions";
        public const string WebSocketProtocol = "WebSocket-Protocol";
        public const string Cookie = "Cookie";
        public const string Upgrade = "Upgrade";
        public const string Origin = "Origin";
        public const string ResponseHeadLine00 = "HTTP/1.1 101 WebSocket Protocol Handshake";
        public const string ResponseHeadLine10 = "HTTP/1.1 101 Switching Protocols";
        public const string ResponseUpgradeLine = Upgrade + ": WebSocket";
        public const string ResponseConnectionLine = Connection + ": Upgrade";
        public const string ResponseOriginLine = "Sec-WebSocket-Origin: {0}";
        public const string ResponseLocationLine = "Sec-WebSocket-Location: {0}://{1}{2}";
        public const string ResponseProtocolLine = SecWebSocketProtocol + ": {0}";
        public const string ResponseAcceptLine = "Sec-WebSocket-Accept: {0}";
        public const string HTTP11 = "HTTP/1.1";
        public const byte StartByte = 0x00;
        public const byte EndByte = 0xFF;
        public static byte[] StartBytes = new byte[] { StartByte };
        public static byte[] EndBytes = new byte[] { EndByte };
        public static byte[] ClosingHandshake = new byte[] { 0xFF, 0x00 };
    }
}
