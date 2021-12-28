using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web;

public partial class MemberCenter02_ascx : SiteControl
{
    public string errmsg;
    public const string err0 = "请输入旧密码";
    public const string err1 = "请输入新密码";
    public const string err2 = "两次输入必须相同";
    public const string err3 = "原始密码不正确";
    public bool updateState = false;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!string.IsNullOrEmpty(this.PostData))
        {
            MemberRow row = this.Member.GetMemberRow(null, true, "pwd");
            if (string.IsNullOrEmpty(this.pwd0))
                errmsg = err0;
            else if (string.IsNullOrEmpty(this.pwd1))
                errmsg = err1;
            else if (this.pwd1 != this.pwd2)
                errmsg = err2;
            else if (text.EncodePassword(row.ACNT, this.pwd0) != row.Password)
                errmsg = err3;
            else
            {
                new web.MemberRowCommand() { ID = this.Member.ID, Password = this.pwd1 }.update(this.PostData);
                this.Member.UserPassword = this.pwd1;
                updateState = true;

                //此处为同步密码至PT
                errmsg = "<label style='color: red;'>恭喜您！</label><br /><br /><label style='color: #f4d408;'>您的登录密码已经修改成功！<br />请在下次登录时使用新密码！</label>";
            }
        }
    }

    [JsonProperty]
    string pwd0;
    [JsonProperty]
    string pwd1;
    [JsonProperty]
    string pwd2;

}