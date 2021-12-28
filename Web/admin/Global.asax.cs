using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Security;
using web;

namespace web
{
    public class Global : _Global
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            lock (this)
            {
                try { Application.Lock(); new CorpRowCommand() { ID = 0, }.insert(null, null); }
                catch (Exception ex) { log.error(ex); }
                finally { Application.UnLock(); }
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e) { }
        void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs e)
        {
            //web._Global.Authenticate(e);
            //Global.AccessControl(e.Context);
            //web.UserManager<web.Admin, web.Guest1>.OnAuthenticate(e);
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e) { }
        protected void Application_AuthorizeRequest(object sender, EventArgs e) { }
        protected void Application_PostAuthorizeRequest(object sender, EventArgs e) { }
        protected void Application_ResolveRequestCache(object sender, EventArgs e) { }
        protected void Application_PostResolveRequestCache(object sender, EventArgs e) { }
        protected void Application_MapRequestHandler(object sender, EventArgs e) { }
        protected void Application_PostMapRequestHandler(object sender, EventArgs e) { }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            Admin.UserList.AcquireRequestState(HttpContext.Current);
            Global.AccessControl(HttpContext.Current);
        }
        protected void Application_PostAcquireRequestState(object sender, EventArgs e) { }
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e) { }
        protected void Application_PostRequestHandlerExecute(object sender, EventArgs e) { }
        protected void Application_ReleaseRequestState(object sender, EventArgs e) { }
        protected void Application_PostReleaseRequestState(object sender, EventArgs e) { }
        protected void Application_UpdateRequestCache(object sender, EventArgs e) { }
        protected void Application_PostUpdateRequestCache(object sender, EventArgs e) { }
        protected void Application_LogRequest(object sender, EventArgs e) { }
        protected void Application_PostLogRequest(object sender, EventArgs e) { }
        protected void Application_EndRequest(object sender, EventArgs e) { }
        protected void Application_PreSendRequestContent(object sender, EventArgs e) { }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e) { }
        protected void Application_Error(object sender, EventArgs e) { }
        protected void Application_End(object sender, EventArgs e) { }
        protected void Session_Start(object sender, EventArgs e) { }
        protected void Session_End(object sender, EventArgs e) { }

        public static void AccessControl(HttpContext context)
        {
            if (web._Global.DebugMode2 == false)
            {
                User user = context.User as User;
                string url = context.Request.AppRelativeCurrentExecutionFilePath;
                if ((context.CurrentHandler is web.comet) ||
                    (context.CurrentHandler is web.api) ||
                    (context.CurrentHandler is web.dinpay))
                    return;
                string appPath = context.Request.ApplicationPath;
                if (!appPath.EndsWith("/")) appPath += "/";
                string loginUrl = FormsAuthentication.LoginUrl.Replace(appPath, "~/");
                string defaultUrl = FormsAuthentication.DefaultUrl.Replace(appPath, "~/");

                if (loginUrl != defaultUrl)
                {
                    string url_t = null;
                    if (string.Compare(url, loginUrl, true) == 0)
                        url_t = user.UserType == UserType.Guest ? null : defaultUrl;
                    else if (string.Compare(url, defaultUrl, true) == 0)
                        url_t = user.UserType == UserType.Guest ? loginUrl : null;
                    else if (web._Global.DebugMode == false)
                    {
                        foreach (MenuRow menu in MenuRow.Cache.Instance.Value.Values)
                        {
                            if (string.IsNullOrEmpty(menu.Url)) continue;
                            if (string.Compare(menu.Url, url, true) != 0) continue;
                            if (user.Permissions[menu.Code, BU.Permissions.Flag.Read] == false)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                context.Response.End();
                                return;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(url_t))
                    {
                        log.message(null, "Request {0}", url);
                    }
                    else
                    {
                        log.message(null, "Request {0} => {1}", url, url_t);
                        context.Server.Transfer(url_t);
                    }
                }
            }
        }
    }
 
    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class notify : api.Comet
    //{
    //    [ObjectInvoke]
    //    static void xxx(notify command, string json, params object[] args) { }
    //}

    public class Admin : AbstractAdmin<Admin>
    {
        protected internal override void OnUserInit(SqlCmd sqlcmd)
        {
            if (string.IsNullOrEmpty(this.Name))
                sqlcmd.FillObject(this, "select [Name] from [Admin] nolock where ID={0}", this.ID);
            base.OnUserInit(sqlcmd);
        }
    }

    public class Agent : AbstractAgent<Agent>
    {
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminLogin : IRowCommand
    {
        [JsonProperty("t1")]
        public string loginName;

        [JsonProperty("t2")]
        public string loginPassword;

        //static object login(AdminLogin command, string json_s, params object[] args)
        //{
        //    try
        //    {
        //        HttpContext context = HttpContext.Current;
        //        Guest guest = context.User as Guest;
        //        if (guest == null)
        //            return LoginResult.Failed(null, json_s, BU.LoginStatus.AlreadyLogin, null);

        //        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //        {
        //            int? corpID;
        //            string acnt;
        //            if (!text.SplitMailAddress(sqlcmd, command.loginName, out corpID, out acnt))
        //                return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.InvaildChar, null);
        //            if (!corpID.HasValue)
        //                return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.CorpNotExist, null);
        //            AdminRow row = sqlcmd.ToObject<AdminRow>("select getdate() as ct,ID,CorpID,GroupID,ACNT,[Name],pwd,Locked from [Admin] nolock where CorpID={0} and ACNT='{1}'", corpID, acnt);
        //            if ((row == null) || (!row.ID.HasValue))
        //                return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.UserNotExist, null);
        //            if (row.ID == 0)
        //                row.Locked = (byte)Locked.Active;
        //            else
        //                row.Locked = row.Locked ?? (byte)Locked.Active;
        //            if ((Locked)row.Locked.Value != Locked.Active)
        //                return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.Locked, null);
        //            if (text.EncodePassword(row.ACNT, command.loginPassword) != row.Password)
        //                return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.PasswordError, null);

        //            // update status
        //            // write login success log

        //            Admin admin = new Admin()
        //            {
        //                ID = row.ID.Value,
        //                CorpID = row.CorpID.Value,
        //                GroupID = row.GroupID.Value,
        //                ACNT = row.ACNT,
        //                Name = row.Name,
        //            };
        //            //admin.LoadPermissions(sqlcmd);
        //            admin.SignIn(sqlcmd, context, guest);
        //            //_Global.SignIn(sqlcmd, context, guest, admin);
        //            return LoginResult.Success(sqlcmd, json_s, admin, context);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // write login error log
        //        return LoginResult.Failed(null, json_s, ex, null);
        //    }
        //}

        [ObjectInvoke]
        static object login(AdminLogin command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                int? corpID = null;
                string acnt = null;
                try
                {
                    if (!text.SplitMailAddress(sqlcmd, command.loginName, out corpID, out acnt))
                        throw new RowException(RowErrorCode.InvaildChar);
                    if (!corpID.HasValue)
                        throw new RowException(RowErrorCode.CorpNotFound);
                }
                catch (RowException ex)
                {
                    Admin.UserList.WriteLoginLog(sqlcmd, null, Admin._UserType, corpID, acnt, null, null, ex.Status, ex.Message, json_s);
                    throw ex;
                }
                AdminRow row;
                Admin admin = Admin.Login(sqlcmd, json_s, corpID, acnt, command.loginPassword, out row);
                return row;
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminLogout
    {
        [ObjectInvoke]
        object logout(AdminLogout cmd, string json_s, params object[] args)
        {
            //User user = HttpContext.Current.User as User;
            //Admin admin = HttpContext.Current.User as Admin;
            //if (admin != null)
            //    admin.Logout(null);
            //AuthedUser user = HttpContext.Current.User as AuthedUser;
            //if (user != null)
            //    user.SignOut(null);
            //return LoginResult.Success(null, json_s, null, null);
            Admin.UserLogout();
            return RowException.Success(null);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminSetInfo
    {
        //[ObjectInvoke]
        //static object setinfo(AdminSetInfo command, string json_s, params object[] args)
        //{
        //    try
        //    {
        //        HttpContext context = HttpContext.Current;
        //        Admin admin = context.User as Admin;
        //        if (admin == null)
        //            return LoginResult.Failed(null, json_s, BU.LoginStatus.AccessDenied, null);

        //        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //        {
        //            //int? corpID;
        //            //string acnt;
        //            //if (!text.SplitMailAddress(sqlcmd, command.loginName, out corpID, out acnt))
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.InvaildChar, null);
        //            //if (!corpID.HasValue)
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.CorpNotExist, null);
        //            //AdminRow row = sqlcmd.ToObject<AdminRow>("select getdate() as ct,ID,CorpID,ACNT,[Name],pwd,Locked from [Admin] nolock where CorpID={0} and ACNT='{1}'", corpID, acnt);
        //            //if ((row == null) || (!row.ID.HasValue))
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.UserNotExist, null);
        //            //if (row.ID == 0)
        //            //    row.Locked = (byte)Locked.Active;
        //            //else
        //            //    row.Locked = row.Locked ?? (byte)Locked.Active;
        //            //if ((Locked)row.Locked.Value != Locked.Active)
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.Locked, null);
        //            //if (text.EncodePassword(row.ACNT, command.loginPassword) != row.Password)
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.PasswordError, null);

        //            //// update status
        //            //// write login success log

        //            //Admin admin = new Admin()
        //            //{
        //            //    ID = row.ID.Value,
        //            //    CorpID = row.CorpID.Value,
        //            //    ACNT = row.ACNT,
        //            //    Name = row.Name,
        //            //};
        //            ////admin.LoadPermissions(sqlcmd);
        //            //admin.SignIn(sqlcmd, context, admin);
        //            ////_Global.SignIn(sqlcmd, context, guest, admin);
        //            //return LoginResult.Success(sqlcmd, json_s, admin, context);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // write login error log
        //        return LoginResult.Failed(null, json_s, ex, null);
        //    }
        //}
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentLogin
    {
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentLogout
    {
    }
}