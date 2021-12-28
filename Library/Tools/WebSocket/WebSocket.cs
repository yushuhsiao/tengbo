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
using System.Net.Security;
using System.Security.Authentication;

namespace System.Net.WebSockets2
{
    #region delegates
    public delegate void WebSocketHandler(WebSocketServer server, WebSocket ws);
    public delegate void SocketHandler(WebSocketServer server, Socket socket);
    public delegate void EventHandler(WebSocket ws);
    public delegate void HandshakeHandler(Socket socket, WebSocketRequest request, WebSocketResponse response);
    public delegate void FrameHandler(WebSocket ws, Frame frame);
    public delegate void FrameHandler<T>(WebSocket ws, Frame frame, T arg);
    #endregion

    #region class WebSocketServer
    public sealed class WebSocketServer
    {
        public const string log_prefix = "WebSocket";

        #region events
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
        #endregion

        public Uri Url { get; private set; }
        public string InitUrl { get; set; }
        public int InitPortRange { get; set; }
        public double HandshakeTimeoutValue { get; set; }
        Socket m_Listener;
        public WebSocketServer()
        {
            HandshakeTimeoutValue = 30000;
        }

        public void Start(string url, int port_range)
        {
            this.InitUrl = url;
            this.InitPortRange = port_range;
            this.Start();
        }
        public void Start()
        {
            try
            {
                UriBuilder uri = new UriBuilder(this.InitUrl);
                uri.Host = Dns.GetHostName();
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                for (int port = uri.Port, cnt = this.InitPortRange; ; )
                {
                    try
                    {
                        listener.Bind(new IPEndPoint(IPAddress.Any, port));
                        listener.Listen(100);
                        listener.BeginAccept(accept, null);
                        m_Listener = listener;
                        uri.Port = port;
                        log.message(null, "WebSocket url : {0}", this.Url = uri.Uri);
                        break;
                    }
                    catch
                    {
                        if (--cnt <= 0) throw;
                        port += cnt > 0 ? 1 : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.message(null, "WebSocket : {0}", ex.Message);
            }
        }

        void accept(IAsyncResult ar)
        {
            DateTime time = DateTime.Now;
            try
            {
                Socket socket = m_Listener.EndAccept(ar);
                m_Listener.BeginAccept(accept, null);
                log.message(log_prefix, "Accept socket from {0}", socket.RemoteEndPoint);
                AcceptSocketEvent(this, socket);
                BeginHandshake(new SocketStream(socket));
            }
            catch (Exception ex) { log.message(log_prefix, ex.ToString()); }
        }

        void BeginHandshake(SocketStream socket)
        {
            try
            {
                byte[] buffer = socket.handshake_buf;
                int offset = socket.handshake_index;
                if (buffer == null)
                {
                    socket.Close(HandshakeTimeoutValue, this.OnHandshakeTimeout);
                    buffer = new byte[tools.BLOCK_SIZE];
                    offset = 0;
                }
                if (buffer.Length <= offset)
                    Array.Resize(ref buffer, buffer.Length + tools.BLOCK_SIZE);
                socket.handshake_buf = buffer;
                socket.handshake_index = offset;
                socket.BeginRead(buffer, offset, buffer.Length - offset, EndHandshake, socket);
            }
            catch { using (socket) return; }
        }

        void EndHandshake(IAsyncResult ar)
        {
            SocketStream socket = (SocketStream)ar.AsyncState;
            try
            {
                int count = socket.EndRead(ar);
                if (count == 0) using (socket) return;
                socket.handshake_index += count;
                if (socket.Available == 0)
                {
                    WebSocketRequest request = WebSocketRequest.Parse(socket.handshake_buf, socket.handshake_index);
                    if (request != null)
                    {
                        Handshake(socket, request, new WebSocketResponse());
                        return;
                    }
                }
                BeginHandshake(socket);
            }
            catch { using (socket) return; }
        }

        void Handshake(SocketStream socket, WebSocketRequest request, WebSocketResponse response)
        {
            socket.Close(null, null);
            WebSocket ws = null;
            try
            {
                response.Status = HttpStatusCode.SwitchingProtocols;
                this.HandshakeRequestEvent(socket.socket, request, response);
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
                            if (Hybi13Handshake(request, response))
                                ws = new Hybi13WebSocket();
                            else if (Draft76Handshake(request, response))
                                ws = new Draft76WebSocket();
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
            log.message(log_prefix, "{0} [{1} {2} {3}] [{4}]", socket.RemoteEndPoint.Address, request.Method, request.Path, request.Version, response.ResponseHeader);

            HandshakeHandler handshake_event;
            AsyncCallback callback;
            try
            {
                if (ws == null)
                {
                    handshake_event = this.HandshakeFailedEvent;
                    callback = this.OnHandshakeFailed;
                }
                else
                {
                    try { this.AcceptWebSocketEvent(this, ws); }
                    catch { }
                    handshake_event = HandshakeSuccessEvent;
                    callback = ws.Start;
                }
                try { handshake_event(socket.socket, request, response); }
                catch { }
                byte[] data = response.GetBytes();
                socket.BeginWrite(data, 0, data.Length, callback, socket);
            }
            catch { }
        }

        void OnHandshakeTimeout(SocketStream socket)
        {
            try
            {
                using (socket)
                {
                    log.message(log_prefix, "{0} handshake timeout!", socket.RemoteEndPoint.Address);
                    this.HandshakeTimeoutEvent(this, socket.socket);
                }
            }
            catch { }
        }

        void OnHandshakeFailed(IAsyncResult ar)
        {
            try
            {
                SocketStream socket = (SocketStream)ar.AsyncState;
                socket.Close(500, tools._null);
            }
            catch { }
        }

        static SHA1 sha = SHA1.Create();
        const string magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        bool Hybi13Handshake(WebSocketRequest request, WebSocketResponse response)
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

        static MD5 md5 = MD5.Create();
        static string nums = "0123456789";
        bool Draft76Handshake(WebSocketRequest request, WebSocketResponse response)
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
            if (!Draft76_ParseKey(seckey1, out space1, out key1, out s1))
                return false;
            if (!Draft76_ParseKey(seckey2, out space2, out key2, out s2))
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
        static bool Draft76_ParseKey(string seckey, out uint space, out ulong key, out uint s)
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
    #endregion

    #region class WebSocketRequest
    public sealed class WebSocketRequest : NameValueCollection
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

        static int id;
        readonly int ID = Interlocked.Increment(ref id);
        static Queue<WebSocketRequest> pooling = new Queue<WebSocketRequest>();

        static byte[] http_end = Encoding.UTF8.GetBytes("\r\n\r\n");
        internal static WebSocketRequest Parse(byte[] buffer, int length)
        {
            int n = buffer.IndexOf(0, http_end);
            if (n < 0) return null;
            n += 4;
            string http = Encoding.UTF8.GetString(buffer, 0, n);
            int pos = 0;
            WebSocketRequest request = new WebSocketRequest()
            {
                HttpRequest = http,
                Body = buffer.GetBytes(n, length),
                Method = http.ReadTo(ref pos, ' '),
                Path = http.ReadTo(ref pos, ' '),
                Version = http.ReadLine(ref pos),
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
    #endregion

    #region class WebSocketResponse
    public sealed class WebSocketResponse : NameValueCollection
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
    #endregion

    #region class WebSocket
    public abstract class WebSocket : IDisposable
    {
        public const string log_prefix = WebSocketServer.log_prefix;
        public static bool TransferLog = false;

        internal SocketStream socket;
        void IDisposable.Dispose()
        {
            using (SocketStream socket = Interlocked.Exchange(ref this.socket, null))
            {
                if (socket == null)
                    return;
                socket.Close();
                log.message(log_prefix, "{0} disconnected.", socket.RemoteEndPoint.Address);
            }
        }

        internal SocketStream Socket
        {
            get { return Interlocked.CompareExchange(ref socket, null, null); }
        }
        public bool Disposed
        {
            get { return Socket == null; }
        }
        public bool Alive
        {
            get { return Socket != null; }
        }

        public void Close(double? timeout)
        {
            SocketStream socket = this.Socket;
            if (socket != null) socket.Close(timeout, Close);
        }
        public void Close()
        {
            using (this) return;
        }
        void Close(SocketStream socket)
        {
            Close();
        }


        internal object OnError(Exception ex, string message)
        {
            return null;
        }

        internal object OnError(string message)
        {
            if (TransferLog) log.message(log_prefix, "<0>\tError\t{1}", socket.RemoteEndPoint.Address, message);
            return null;
        }

        internal abstract void Start(IAsyncResult ar);

        public virtual void SendFrame(Frame frame, FrameTypes? type, byte[] data)
        {
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
    }

    abstract class WebSocket<TFrame> : WebSocket where TFrame : Frame, new()
    {
        internal override void Start(IAsyncResult ar)
        {
            Interlocked.Exchange(ref this.socket, ar.AsyncState as SocketStream);
            this.send(null);
            this.recv(null);
        }

        Queue<TFrame> recv_queue = new Queue<TFrame>();
        protected void recv(IAsyncResult ar)
        {
            try
            {
                DateTime time = DateTime.Now;
                SocketStream socket;
                object state;
                if ((socket = base.Socket) == null) return;
                if (ar != null)
                {
                    state = ar.AsyncState;
                    int count = socket.EndRead(ar);
                    if (count == 0) using (this) return;
                    this.EndReceive(time, state, count, socket.Available);
                }
                else
                    state = null;
                if ((socket = base.Socket) == null) return;
                this.BeginReceive(socket, state);

                TFrame frame;
                if (!Monitor.TryEnter(recv_queue))
                    return;
                try
                {
                    if (recv_queue.Count == 0)
                        return;
                    frame = recv_queue.Dequeue();
                }
                finally { Monitor.Exit(recv_queue); }
                try
                {
                    this.ReceiveFrameEvent(this, frame);
                    string text;
                    byte[] binary;
                    switch (frame.FrameType)
                    {
                        case FrameTypes.Text:
                            this.ReceiveTextEvent(this, frame, text = frame.Text);
                            if (TransferLog) log.message(log_prefix, "ReceiveText, {0}", text);
                            break;
                        case FrameTypes.Binary:
                            this.ReceiveBinaryEvent(this, frame, binary = frame.Binary);
                            if (TransferLog) log.message(log_prefix, "ReceiveBinary, len={0}", binary.Length);
                            break;
                        case FrameTypes.Ping:
                            this.ReceivePingEvent(this, frame, binary = frame.Binary);
                            if (TransferLog) log.message(log_prefix, "ReceivePing, len={0}", binary.Length);
                            break;
                        case FrameTypes.Pong:
                            this.ReceivePingEvent(this, frame, binary = frame.Binary);
                            if (TransferLog) log.message(log_prefix, "ReceivePing, len={0}", binary.Length);
                            break;
                        case FrameTypes.Close:
                            this.ReceiveCloseEvent(this, frame, binary = frame.Binary);
                            if (TransferLog) log.message(log_prefix, "ReceiveClose, len={0}", binary.Length);
                            this.Close(500);
                            break;
                        default: if (TransferLog) log.message(log_prefix, "OnReceiveFrame"); break;
                    }
                }
                catch { }
            }
            catch (Exception ex) { OnError(ex, null); using (this) return; }
        }

        protected abstract void BeginReceive(SocketStream socket, object state);
        protected abstract void EndReceive(DateTime time, object state, int count, int available);

        protected void OnFrameReady(TFrame frame)
        {
            lock (recv_queue)
                if (!recv_queue.Contains(frame))
                    recv_queue.Enqueue(frame);
            try { this.FrameReadyEvent(this, frame); }
            catch { }
        }

        Queue<TFrame> send_queue = new Queue<TFrame>();
        TFrame send_frame;
        int send_mode;
        protected void send(IAsyncResult ar)
        {
            try
            {
                SocketStream socket = base.Socket;
                if (socket == null) return;
                object state = null;
                if (ar != null)
                {
                    state = ar.AsyncState;
                    socket.EndWrite(ar);
                    this.EndSend(state);
                }
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
                Interlocked.CompareExchange(ref send_frame, this.BeginSend(socket, frame, frame_start, state), frame);
            }
            catch (Exception ex) { OnError(ex, null); using (this) return; }
        }
        protected virtual TFrame BeginSend(SocketStream socket, TFrame frame, bool frame_start, object state)
        {
            if (frame_start)
                send_mode = 0;
            while (send_mode < frame.datas.Length)
            {
                while (frame.datas[send_mode].Length > 0)
                {
                    socket.BeginWrite(frame.datas[send_mode], 0, frame.datas[send_mode].Length, send, null);
                    return frame;
                }
                send_mode++;
            }
            return null;
        }
        protected virtual void EndSend(object state)
        {
            send_mode++;
        }

        public override void SendFrame(Frame frame, FrameTypes? type, byte[] data)
        {
            TFrame item = frame as TFrame;
            if (item == null)
                if (type.HasValue && (data != null))
                    item = new TFrame() { FrameType = type.Value };
            if (item == null)
                return;
            lock (send_queue)
                send_queue.Enqueue(item);
            this.send(null);
        }
    }
    #endregion

    #region class Frame
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

    public enum FrameTypes : byte
    {
        Continuation = 0x00,
        Text = 0x01,
        Binary = 0x02,
        Close = 0x08,
        Ping = 0x09,
        Pong = 0x0A,
    }
    #endregion

    #region class Hybi13WebSocket
    class Hybi13WebSocket : WebSocket<Hybi13Frame>
    {
        // http://tools.ietf.org/html/rfc6455
        Hybi13Frame recv_part;
        DateTime recv_header_time;
        byte[][] recv_datas;
        int recv_mode;
        int recv_index;
        protected override void BeginReceive(SocketStream socket, object state)
        {
            if (recv_datas == null)
            {
                recv_datas = Hybi13Frame.datas_default();
                recv_mode = 0;
                recv_index = 0;
            }
            socket.BeginRead(recv_datas[recv_mode], recv_index, recv_datas[recv_mode].Length - recv_index, base.recv, null);
        }
        protected override void EndReceive(DateTime time, object state, int count, int available)
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
                        using (this) { this.OnError("未知的訊息類型"); break; }

                    bool fin = Hybi13Frame._FIN(recv_datas[0]);

                    if (fin) recv_index = 0;
                    else if ((frameType == FrameTypes.Ping) || (frameType == FrameTypes.Pong) || (frameType == FrameTypes.Close))
                        using (this) { this.OnError("控制訊息不能分段"); break; }
                    else if (frameType == FrameTypes.Continuation)
                    {
                        if (recv_part == null) using (this) { this.OnError("第一個分段訊息不存在"); break; }
                    }
                    else
                    {
                        if (recv_part != null) using (this) { this.OnError("已經有一個分段訊息了"); break; }
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
                        if (recv_part == null)
                            this.OnFrameReady(frame);
                        else
                        {
                            recv_part.AddNext(frame);
                            this.OnFrameReady(recv_part);
                            recv_part = null;
                        }
                    }
                    else
                    {
                        if (recv_part != null) using (this) { this.OnError("已經有一個分段訊息了"); break; }
                        recv_part = frame;
                    }
                    break;
                }
                recv_mode++;
            }
        }
    }
    #endregion

    #region class Hybi13Frame
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
                m_MaskingKey_old = RandomValue.GetBytes(4);
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
    #endregion

    #region class Draft76WebSocket
    class Draft76WebSocket : WebSocket<Draft76Frame>
    {
        // http://tools.ietf.org/html/draft-ietf-hybi-thewebsocketprotocol-17
        DateTime? recv_start;
        byte[] recv_buf;
        List<byte[]> recv_datas = new List<byte[]>();
        int recv_index;
        
        protected override void BeginReceive(SocketStream socket, object state)
        {
            if (recv_buf == null)
            {
                recv_buf = new byte[tools.BLOCK_SIZE];
                recv_index = 0;
            }
            socket.BeginRead(recv_buf, recv_index, recv_buf.Length - recv_index, recv, state);
        }
        protected override void EndReceive(DateTime time, object state, int count, int available)
        {
            int start = recv_index;
            recv_index += count;
            for (int n = start; n < recv_index; n++)
            {
                Draft76Frame frame = null;
                for (; n < recv_index; n++)
                {
                    byte b = recv_buf[n];
                    if (b == tools.StartByte)
                    {
                        start = n;
                        recv_start = time;
                    }
                    else if (b == tools.EndByte)
                    {
                        if (recv_start.HasValue)
                        {
                            n++;
                            frame = new Draft76Frame() { HeaderReady = recv_start.Value, PacketReady = time, FrameType = FrameTypes.Text, };
                            break;
                        }
                    }
                }
                if (recv_start.HasValue)
                {
                    int len = n - start;
                    byte[] data;
                    if (len == recv_buf.Length)
                    {
                        data = recv_buf;
                        recv_buf = null;
                    }
                    else
                    {
                        data = new byte[len];
                        Array.Copy(recv_buf, start, data, 0, len);
                    }
                    recv_datas.Add(data);
                }
                if (frame != null)
                {
                    frame.datas = recv_datas.ToArray();
                    recv_datas.Clear();
                    recv_start = null;
                    this.OnFrameReady(frame);
                }
            }
            if (recv_buf != null)
                if (recv_buf.Length <= recv_index)
                    recv_index = 0;
        }
    }
    #endregion

    #region class Draft76Frame
    class Draft76Frame : Frame
    {
        public Draft76Frame() : base(null) { }
        public Draft76Frame(byte[][] datas) : base(datas) { }

        public override byte[] Binary
        {
            get { return tools.null_bytes; }
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
                    int n2 = 0;
                    int end = n2 + datas[n].Length;
                    if (n == 0) n2++;
                    if (n == (datas.Length - 1)) end--;
                    for (; n2 < end; n2++)
                        if (tools.UTF8Decode(datas[n][n2], ref c, ref l))
                            sb.Append(c);
                }
                return sb.ToString();
            }
            set { this.Binary = Encoding.UTF8.GetBytes(value); }
        }
    }
    #endregion

    #region static class tools
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
        public static unsafe bool UTF8Decode(byte b, ref char c, ref int c_index)
        {
            if (b < 0x80)
            {
                c = (char)b;
                c_index = 0;
                return true;
            }
            byte b1;
            byte m1 = 0x80;
            byte m2 = 0x7f;
            for (int i = -1; i < 4; m1 >>= 1, m2 >>= 1, i++)
            {
                b1 = b;
                b1 &= m1;
                if (b1 == 0)
                {
                    b1 = b;
                    b1 &= m2;
                    if (i == 0)
                    {
                        if (c_index > 0)
                        {
                            c <<= 6;
                            c |= (char)b1;
                            (c_index)--;
                            return c_index == 0;
                        }
                    }
                    //else if (i == -1) { c = (char)b1; c_index = 0; return true; }
                    else if (i < 4) { c = (char)b1; c_index = i; }
                    break;
                }
            }
            return false;
        }

        public static int IndexOf(this byte[] bytes, int startIndex, byte[] value)
        {
            int n1 = bytes.Length;
            int n2 = value.Length;
            for (int i = startIndex; i < n1; i++)
            {
                if ((i + n2) > n1) break;
                bool success = true;
                for (int j = 0; success & (j < n2); j++)
                    success &= (bytes[i + j] == value[j]);
                if (success)
                    return i;
            }
            return -1;
        }

        public static byte[] GetBytes(this byte[] bytes, int startIndex, int endIndex)
        {
            int len = endIndex - startIndex;
            if (len < 0) len = 0;
            byte[] ret = new byte[len];
            for (int n1 = 0, n2 = startIndex; n2 < endIndex; n1++, n2++)
                ret[n1] = bytes[n2];
            return ret;
        }

        internal static byte[] null_bytes = new byte[0];

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
    #endregion
}

namespace System.Net.WebSockets
{
    public sealed class WebSocketServer
    {
        Dictionary<int, Socket> listener = new Dictionary<int, Socket>();
        public static void Test()
        {
            ConsoleLogWriter.Enabled = true;
            WebSocketServer srv = new WebSocketServer();
            srv.AddListener("ws://localhost:8080");
            Console.ReadKey();
        }

        public WebSocketServer()
        {
        }

        double m_HandshakeTimeoutValue = 3000;
        public double HandshakeTimeoutValue
        {
            get { return Interlocked.CompareExchange(ref m_HandshakeTimeoutValue, 0, 0); }
            set { Interlocked.Exchange(ref m_HandshakeTimeoutValue, value); }
        }

        public void AddListener(string url)
        {
            new Listener(this, url);
        }

        class Listener : Socket
        {
            internal readonly WebSocketServer server;
            internal readonly Uri Uri;

            internal Listener(WebSocketServer server, string url)
                : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                this.Uri = new Uri(url);
                this.Bind(new IPEndPoint(IPAddress.Any, this.Uri.Port));
                this.Listen(100);
                this.server = server;
                this.server.writelog("WebSocket url : {0}", url);
                this.BeginAccept(accept, null);
            }

            void accept(IAsyncResult ar)
            {
                DateTime time = DateTime.Now;
                try
                {
                    Socket socket = this.EndAccept(ar);
                    this.BeginAccept(accept, null);
                    this.server.writelog("Accept socket from {0}", socket.RemoteEndPoint);
                    Handshake.Start(this, socket);
                }
                catch (Exception ex) { this.server.OnError(ex); }
            }
        }

        class Handshake : IDisposable
        {
            byte[] buffer;
            int offset;

            Listener listener;
            SocketStream s1;
            Stream s2;

            static Queue<Handshake> pooling = new Queue<Handshake>();
            internal static void Start(Listener listener, Socket socket)
            {
                Handshake obj;
                lock (pooling)
                    if (pooling.Count > 0)
                        obj = pooling.Dequeue();
                    else
                        obj = new Handshake();
                SocketStream s = new SocketStream(socket);
                Interlocked.Exchange(ref obj.listener, listener);
                Interlocked.Exchange(ref obj.s2, s);
                Interlocked.Exchange(ref obj.s1, s);
                obj.buffer = obj.buffer ?? new byte[tools.BLOCK_SIZE];
                obj.offset = 0;
                s.Close(listener.server.HandshakeTimeoutValue, obj.Timeout);
            }

            void IDisposable.Dispose()
            {
            }

            void Timeout(SocketStream socket)
            {
                using (this)
                {
                }
            }

        }

        void writelog(string message)
        {
            log.message("wssrv", message);
        }
        void writelog(string format, params object[] args)
        {
            log.message("wssrv", format, args);
        }
        void OnError(Exception ex)
        {
            log.message("wssrv", ex.ToString());
        }
    }

    public class WebSocket
    {
        internal WebSocket(WebSocketServer server, Socket socket)
        {
        }

        public WebSocket(string url)
        {
        }
    }

    static class tools
    {
        public static void _null() { }
        public static void _null<T>(T t) { }
        public static void _null<T1, T2>(T1 t, T2 t2) { }
        public static void _null<T1, T2, T3>(T1 t1, T2 t2, T3 t3) { }

        internal const int BLOCK_SIZE = 32;
    }
}