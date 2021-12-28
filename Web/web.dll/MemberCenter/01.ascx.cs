using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web;

public class MemberCenter01_ascx : SiteControl
{
    public MemberRow row;

    public string errmsg;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!string.IsNullOrEmpty(this.PostData))
        {
            this.row = new web.MemberRowCommand()
            {
                ID = this.Member.ID,
                Sex = this.sex,
                Birthday = this.Birthday,
                Tel = this.tel,
                Addr = this.addr,
                Mail = this.mail,
                QQ = this.qq,
                UserMemo = this.memo,
            }.update(this.PostData);
            this.errmsg = "基本信息已经更新!!";
        }
        this.row = this.row ?? this.Member.GetMemberRow(null, true, "Birthday", "Tel", "Mail", "QQ", "Sex", "Addr", "UserMemo");
        row.Sex = row.Sex ?? 0;
        if (row.Sex.HasValue)
            if (!Enum.IsDefined(typeof(BU.UserSex), row.Sex.Value))
                row.Sex = 0;
    }

    [JsonProperty("n1")]
    string sex;
    [JsonProperty("n2")]
    int? yy;
    [JsonProperty("n3")]
    int? mm;
    [JsonProperty("n4")]
    int? dd;
    DateTime? Birthday { get { try { return new DateTime(this.yy ?? 0, this.mm ?? 0, this.dd ?? 0); } catch { } return null; } }
    [JsonProperty("n5")]
    string tel;
    [JsonProperty("n6")]
    string addr;
    [JsonProperty("n7")]
    string mail;
    [JsonProperty("n8")]
    string qq;
    [JsonProperty("n9")]
    string memo;
}