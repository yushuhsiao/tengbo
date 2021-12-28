using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;

public class WebIMAsyncHandler : IHttpAsyncHandler
{
    #region IHttpAsyncHandler 成员

    public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
    {
        string _UID = context.Request.Params["uid"];

        WebIMClientAsyncResult _AsyncResult = new WebIMClientAsyncResult(context, cb, extraData);

        string _Content = context.Request.Params["content"];
        string _Action = context.Request.Params["action"];
        if (_Action == "login")
        {
            _UID = context.Request.Params["uid"];
            _AsyncResult.LoginID = _UID;
            WebIMMessageHandler.Instance().Login(_UID, _AsyncResult);
        }
        else if (_Action == "logout")
        {
            _AsyncResult.LoginID = _UID;
            WebIMMessageHandler.Instance().Logout(_UID, _AsyncResult);
        }
        else if (_Action == "connect")
        {
            _AsyncResult.LoginID = _UID;
            WebIMMessageHandler.Instance().Connect(_AsyncResult);
        }
        else if (_Action == "getuserlist")
        {
            _AsyncResult.LoginID = _UID;
            WebIMMessageHandler.Instance().GetUserList(_AsyncResult);
        }


        //调用
        WebIMMessageHandler.Instance().AddMessage(_Content, _AsyncResult);
        return _AsyncResult;
    }

    public void EndProcessRequest(IAsyncResult result)
    {

    }

    #endregion

    #region IHttpHandler 成员

    public bool IsReusable
    {
        get { return false; ; }
    }

    public void ProcessRequest(HttpContext context)
    {
        throw new NotImplementedException();
    }

    #endregion
}

public class WebIMClientAsyncResult : IAsyncResult
{

    bool m_IsCompleted = false;
    private HttpContext m_Context;
    private AsyncCallback m_Callback;
    private object m_ExtraData;
    private string m_Content;
    private string m_LoginID = string.Empty;


    public WebIMClientAsyncResult(HttpContext p_Context, AsyncCallback p_Callback, object p_ExtraData)
    {
        this.m_Context = p_Context;
        this.m_Callback = p_Callback;
        this.m_ExtraData = p_ExtraData;
    }
    /// <summary>
    /// 用户编号
    /// </summary>
    public string LoginID
    {
        get
        {
            return m_LoginID;
        }
        set
        {
            m_LoginID = value;
        }
    }
    /// <summary>
    /// 发送消息的内容，暂时未使用到
    /// </summary>
    public string Content
    {
        get
        {
            return m_Content;
        }
        set
        {
            m_Content = value;
        }
    }
    #region IAsyncResult 成员

    public object AsyncState
    {
        get { return null; }
    }

    public WaitHandle AsyncWaitHandle
    {
        get { return null; }
    }

    public bool CompletedSynchronously
    {
        get { return false; }
    }

    public bool IsCompleted
    {
        get { return m_IsCompleted; }
    }

    #endregion

    /// <summary>
    /// 向客户端响应消息
    /// </summary>
    /// <param name="data"></param>
    public void Send(object data)
    {
        try
        {
            m_Context.Response.Write(this.Content);
            if (m_Callback != null)
            {
                m_Callback(this);
            }

        }
        catch { }
        finally
        {
            m_IsCompleted = true;
        }
    }
}

public class WebIMMessageHandler
{
    private static readonly WebIMMessageHandler m_Instance = new WebIMMessageHandler();
    //记录所有请求的客户端
    List<WebIMClientAsyncResult> m_Clients = new List<WebIMClientAsyncResult>();
    string m_Users = string.Empty;
    public WebIMMessageHandler()
    {

    }

    public static WebIMMessageHandler Instance()
    {
        return m_Instance;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="p_LoginID"></param>
    /// <param name="p_ClientAsyncResult"></param>
    public void Login(string p_LoginID, WebIMClientAsyncResult p_ClientAsyncResult)
    {
        bool _Logined = false;
        foreach (WebIMClientAsyncResult _Item in m_Clients)
        {
            if (_Item.LoginID == p_LoginID)
            {
                p_ClientAsyncResult.Content = "你已登录";
                _Logined = true;
                break;
            }
        }
        if (!_Logined)
        {
            //m_Clients.Add(p_ClientAsyncResult);
            p_ClientAsyncResult.Content = "OK";
        }
        p_ClientAsyncResult.Send(null);


    }

    private string GetUsers()
    {
        string _Users = string.Empty;
        foreach (WebIMClientAsyncResult _Item in m_Clients)
        {
            _Users += _Item.LoginID + ",";
        }
        return _Users;
    }

    public void Logout(string p_LoginID, WebIMClientAsyncResult p_ClientAsyncResult)
    {
        foreach (WebIMClientAsyncResult _Item in m_Clients)
        {
            if (_Item.LoginID == p_LoginID)
            {
                m_Clients.Remove(_Item);
                break;
            }
        }
        p_ClientAsyncResult.Content = "退出成功";
        p_ClientAsyncResult.Send(null);
        //UpdateUserList();

        string _Users = GetUsers();
        foreach (WebIMClientAsyncResult _Item in m_Clients)
        {
            _Item.Content = _Users;
            _Item.Send(null);
        }
        m_Clients.Clear();
    }

    public void GetUserList(WebIMClientAsyncResult p_ClientAsyncResult)
    {
        Connect(p_ClientAsyncResult);
        string _Users = GetUsers();
        foreach (WebIMClientAsyncResult _Item in m_Clients)
        {
            _Item.Content = _Users;
            _Item.Send(null);
        }
        m_Clients.Clear();
    }

    public void Connect(WebIMClientAsyncResult p_Client)
    {
        bool _Exists = false;
        foreach (WebIMClientAsyncResult _Item in m_Clients)
        {
            if (_Item.LoginID == p_Client.LoginID)
            {
                _Exists = true;
                break;
            }
        }
        if (!_Exists)
        {
            m_Clients.Add(p_Client);
        }
    }

    public void UpdateUserList()
    {
        string _Users = GetUsers();
        foreach (WebIMClientAsyncResult result in m_Clients)
        {
            result.Content = m_Users;
            result.Send(null);
        }
        m_Clients.Clear();

    }

    /// <summary>
    /// 广播消息
    /// </summary>
    /// <param name="p_Message"></param>
    /// <param name="p_AsyncResult"></param>
    public void AddMessage(string p_Message, WebIMClientAsyncResult p_ClientAsyncResult)
    {
        ////保持联连
        //if (p_Message == "-1")
        //{
        //    m_Clients.Add(p_ClientAsyncResult);

        //}
        //else
        //{
        //    //将当前请求的内容输出到客户端
        //    p_ClientAsyncResult.Content = p_Message;
        //    p_ClientAsyncResult.Send(null);

        //    //否则将遍历所有已缓存的client,并将当前内容输出到客户端
        //    foreach (WebIMClientAsyncResult result in m_Clients)
        //    {
        //        result.Content = p_Message;
        //        result.Send(null);
        //    }

        //    //清空所有缓存
        //    m_Clients.Clear();
        //}
    }

}
