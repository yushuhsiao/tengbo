using BU;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Web;
using web;

public partial class Login_ascx : SiteControl
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!string.IsNullOrEmpty(this.PostData))
        {
            bool reload = false;
            if (this.action == "login")
            {
                if (this.Member == null)
                {
                    int corpID = User.Manager.DefaultCorpID;
                    string acnt = null;
                    string password = null;
                    try
                    {
                        acnt = Member.prefix_remove(this.loginName * text.ValidAsACNT);
                        password = this.loginPassword * text.ValidAsString;
                        string verifycode = this.verifyCode * text.ValidAsString;
                        if (string.IsNullOrEmpty(acnt) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(verifycode))
                            throw new RowException(RowErrorCode.InvaildChar, "输入不合法");
                        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                        {
                            MemberRow row;
                            Member member = Member.Login(sqlcmd, this.PostData, corpID, acnt, password, out row);
                        }
                        reload = true;
                    }
                    catch (RowException ex)
                    {
                        Member.WriteLoginLog(null, null, corpID, acnt ?? this.loginName, null, null, ex.Status, ex.Message, this.PostData);
                        switch (ex.Status)
                        {
                            case RowErrorCode.PasswordError: this.errmsg = "登入失敗"; break;
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