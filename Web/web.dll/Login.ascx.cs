using BU;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Web;
using web;
using System.Web.SessionState;

public partial class CLogin_aspx : root_aspx 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Member != null)
            Response.Redirect("~/MemberCenter/Cdefault.aspx");
    }
}

public class CLoginHandler_aspx : SitePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.PostData))
        {
            bool reload = false;
            string password = null;
            if (this.action == "login")
            {
                if (this.Member == null)
                {
                    int corpID = _Global.DefaultCorpID;
                    string acnt = null;
                    bool errlog = true;
                    try
                    {
                        acnt = Member.prefix_remove(this.loginName * text.ValidAsACNT);
                        password = this.loginPassword * text.ValidAsString;
                        string verifycode = this.verifyCode * text.ValidAsString;
                        if (string.IsNullOrEmpty(acnt) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(verifycode))
                            throw new RowException(RowErrorCode.InvaildChar, "输入不合法!");
                        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                        {
                            MemberRow row;
                            errlog = false;
                            Member member = Member.Login(sqlcmd, this.PostData, corpID, acnt, password, out row);
                            member.UserPassword = this.loginPassword;
                        }
                        reload = true;
                    }
                    catch (RowException ex)
                    {
                        if (errlog) Member.UserList.WriteLoginLog(null, null, Member._UserType, corpID, acnt ?? this.loginName, null, null, ex.Status, ex.Message, this.PostData);
                        switch (ex.Status)
                        {
                            case RowErrorCode.PasswordError: this.errmsg = "密码错误!"; break;
                            case RowErrorCode.AccountNotExist: this.errmsg = "帐号不存在!"; break;
                            case RowErrorCode.AccountLocked: this.errmsg = "帐号被锁定!"; break;
                            default: this.errmsg = ex.Message; break;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.error_msg(ex);
                        this.errmsg = ex.Message;
                    }
                    Response.Write(errmsg);
                }
            }
            else if (this.action == "logout")
            {
                Member.UserLogout();
                reload = true;
            }
            if (reload)
            {
                if ((Request.AppRelativeCurrentExecutionFilePath ?? "").ToLower() == "~/login.aspx")
                    Response.Redirect(Request.AppRelativeCurrentExecutionFilePath);
            }
        }
    }

    [JsonProperty("action")]
    string action;

    public string errmsg;

    [JsonProperty("n1")]
    string loginName;

    [JsonProperty("n2")]
    string loginPassword;

    [JsonProperty("n3")]
    string verifyCode = "*";
}

public partial class login_aspx : SitePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = -1441;
        Response.Cache.SetExpires(DateTime.MinValue);
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetNoServerCaching();
    }
}

public partial class login_ascx : SiteControl
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!string.IsNullOrEmpty(this.PostData))
        {
            bool reload = false;
            string password = null;
            if (this.action == "login")
            {
                if (this.Member == null)
                {
                    int corpID = _Global.DefaultCorpID;
                    string acnt = null;
                    bool errlog = true;
                    try
                    {
                        acnt = Member.prefix_remove(this.loginName * text.ValidAsACNT);
                        password = this.loginPassword * text.ValidAsString;
                        string verifycode = this.verifyCode * text.ValidAsString;
                        if (string.IsNullOrEmpty(acnt) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(verifycode))
                            throw new RowException(RowErrorCode.InvaildChar, "输入不合法!");
                        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                        {
                            MemberRow row;
                            errlog = false;
                            Member member = Member.Login(sqlcmd, this.PostData, corpID, acnt, password, out row);
                            member.UserPassword = this.loginPassword;
                        }
                        reload = true;
                    }
                    catch (RowException ex)
                    {
                        if (errlog) Member.UserList.WriteLoginLog(null, null, Member._UserType, corpID, acnt ?? this.loginName, null, null, ex.Status, ex.Message, this.PostData);
                        switch (ex.Status)
                        {
                            case RowErrorCode.PasswordError: this.errmsg = "密码错误!"; break;
                            case RowErrorCode.AccountNotExist: this.errmsg = "帐号不存在!"; break;
                            case RowErrorCode.AccountLocked: this.errmsg = "帐号被锁定!"; break;
                            default: this.errmsg = ex.Message; break;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.error_msg(ex);
                        this.errmsg = ex.Message;
                    }
                }
            }
            else if (this.action == "logout")
            {
                Member.UserLogout();
                reload = true;
            }
            if (reload)
            {
                if ((Request.AppRelativeCurrentExecutionFilePath ?? "").ToLower() == "~/login.aspx")
                    Response.Redirect(Request.AppRelativeCurrentExecutionFilePath);
            }
        }
    }

    [JsonProperty("action")]
    string action;

    public string errmsg;

    [JsonProperty("n1")]
    string loginName;

    [JsonProperty("n2")]
    string loginPassword;

    [JsonProperty("n3")]
    string verifyCode = "*";
}

