using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web;

public partial class MemberCenter04_ascx : SiteControl
{
    public string errmsg;
    public const string err0 = "请输入旧密码";
    public const string err1 = "请输入新密码";
    public const string err2 = "两次输入必须相同";
    public const string err3 = "密码不正确";
    public MemberRow row;
    public bool updateState = false;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        this.row = this.Member.GetMemberRow(null, true, "sec_pwd");
        if (!string.IsNullOrEmpty(this.PostData))
        {
            if (string.IsNullOrEmpty(this.pwd1))
                errmsg = err1;
            else if (this.pwd1 != this.pwd2)
                errmsg = err2;
            else
            {
                bool chk_old = false;
                if (string.IsNullOrEmpty(row.SecurityPassword))
                    chk_old = true;
                else
                {
                    if (string.IsNullOrEmpty(this.pwd0))
                        errmsg = err0;
                    else if (text.EncodePassword(row.ACNT, this.pwd0) != row.SecurityPassword)
                        errmsg = err3;
                    else
                        chk_old = true;
                }
                if (chk_old)
                {
                    row = new web.MemberRowCommand() { ID = this.Member.ID, SecPassword = this.pwd1 }.update(this.PostData);
                    updateState = true;
                    errmsg = "<label style='color: red;'>恭喜您！</label><br /><br /><label style='color: #f4d408;'>您的安全密码已经修改成功！</label>";
                }
            }
        }
    }

    [JsonProperty]
    string pwd0;
    [JsonProperty]
    string pwd1;
    [JsonProperty]
    string pwd2;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}