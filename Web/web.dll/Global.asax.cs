using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using web;

public class Global_asax : _Global
{
    protected void Application_AcquireRequestState(object sender, EventArgs e) // session
    {
        Member.UserList.AcquireRequestState(HttpContext.Current);
    }

    protected void Application_Start(object sender, EventArgs e) { }
    protected void Session_Start(object sender, EventArgs e) { }
    protected void Application_BeginRequest(object sender, EventArgs e) { }
    protected void Application_AuthenticateRequest(object sender, EventArgs e) { }
    protected void Application_Error(object sender, EventArgs e) { }
    protected void Session_End(object sender, EventArgs e) { }
    protected void Application_End(object sender, EventArgs e) { }
}

public class Member : AbstractMember<Member>
{
    public string ACNTString()
    {
        return prefix_add(this.ACNT);
    }

    public string BalanceString()
    {
        return BalanceString(this.Balance);
    }

    public static string BalanceString(decimal? balance)
    {
        if (balance.HasValue)
            return string.Format("{0:0.00}", balance);
        else
            return null;
    }

    public string GroupIDString()
    {
        MemberGroupRow row = MemberGroupRow.Cache.Instance.GetRow(this.CorpID, this.GroupID);
        if (row == null) return null;
        return row.Name;
    }

    public override int CorpID
    {
        get { return _Global.DefaultCorpID; }
        set { }
    }

    MemberRow row;
    DateTime row_update;
    [SqlSetting("Cache", "MemberRow"), DefaultValue(10000)]
    public double RowReload
    {
        get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
    }

    [AppSetting("ImageUrl")]
    public static string ImageUrl
    {
        get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), _Global.DefaultCorpID); }
    }

    public MemberRow GetMemberRow(SqlCmd sqlcmd, bool force, params string[] fields)
    {
        lock (this)
        {
            if (force)
                this.row = null;
            TimeSpan t = DateTime.Now - row_update;
            if (t.TotalMilliseconds > RowReload)
                this.row = null;
            if (this.row == null)
            {
                StringBuilder s = new StringBuilder();
                s.Append("select Balance, GroupID, ACNT");
                for (int i = 0; i < fields.Length; i++)
                {
                    switch (fields[i])
                    {
                        case "ACNT":
                        case "Balance":
                        case "GroupID": break;
                        default: s.AppendFormat(",{0}", fields[i]); break;
                    }
                }
                s.Append(" from Member nolock where ID=");
                s.Append(this.ID);
                using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd))
                    this.row = sqlcmd.ToObject<MemberRow>(s.ToString());
                this.row_update = DateTime.Now;
            }
            return this.row;
        }
    }

    public string UserPassword { get; set; }


    /// <summary>
    /// 未处理存款次数
    /// </summary>
    /// <param name="sqlcmd"></param>
    /// <param name="memberID"></param>
    /// <param name="corpID"></param>
    /// <param name="acnt"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    public int UndisposedWithdrawalsCount(SqlCmd sqlcmd)
    {
        using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            return Convert.ToInt32(sqlcmd.ExecuteScalar(string.Format("select count(*) from tranCash1 nolock where UserType=3 and UserID = {0} and LogType=2 and FinishTime is null", this.ID)));
    }


    public decimal Balance
    {
        get { return this.GetMemberRow(null, false).Balance ?? 0; }
    }

    public static string prefix_add(string acnt)
    {
        return CorpRow.Cache.Instance.GetCorp(_Global.DefaultCorpID).prefix + acnt;
    }
    public static string prefix_remove(string acnt)
    {
        CorpRow corp = CorpRow.Cache.Instance.GetCorp(_Global.DefaultCorpID);
        if (!string.IsNullOrEmpty(corp.prefix))
            if (acnt.StartsWith(corp.prefix))
                return acnt.Substring(corp.prefix.Length);
        return acnt;
    }

    [AppSetting]
    public static bool DebugMode2
    {
        get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
    }

    //public static bool RequireLogin(HttpContext context)
    //{
    //    context = context ?? HttpContext.Current;
    //    Member member = context.User as Member;
    //    if (member == null)
    //    {
    //        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}
}

public static class urls
{
    public const string help1 = "~/help/1.aspx";
    public const string help2 = "~/help/2.aspx";
    public const string help3 = "~/help/3.aspx";
    public const string help4 = "~/help/4.aspx";
    public const string help5 = "~/help/5.aspx";
    public const string help6 = "~/help/6.aspx";
    public const string help7 = "~/help/7.aspx";
    public const string help8 = "~/help/8.aspx";
    public const string help9 = "~/help/9.aspx";
    public const string MemberCenter = "~/membercenter/default.aspx";
    public const string Deposit = "~/membercenter/deposit.aspx";
    public const string Withdrawal = "~/membercenter/withdrawal.aspx";
    public const string GameTran = "~/membercenter/gametran.aspx";
    public const string YeePayFinish = "~/membercenter/yeepay_finish.aspx";

    public static string randID(ref Dictionary<string, string> dict, string id)
    {
        if (dict == null)
            dict = new Dictionary<string, string>();
        string res;
        if (!dict.TryGetValue(id, out res))
        {
            //string id_str = "_" + Guid.NewGuid().ToString().Replace("-", ""); //Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace('/', '_');
            string id_str = RandomString.GetRandomString(RandomString.LowerLetter, 1) + RandomString.GetRandomString(RandomString.LowerNumber, 29);
            res = dict[id] = id_str;
        }
        return res;
    }
}

public class SiteMasterPage : web.BaseMasterPage
{
    public string PageUrl { get; set; }
    public int? NavIndex;
    public bool ShowHeader = true;
    public bool ShowNav = true;
    public bool ShowNotice = true;
    public bool ShowFooter = true;

    public static SiteMasterPage GetRoot(MasterPage master)
    {
        MasterPage m = master;
        while (m.Master != null)
            m = m.Master;
        return m as SiteMasterPage;
    }

    public SiteMasterPage RootMasterPage
    {
        [DebuggerStepThrough]
        get { return SiteMasterPage.GetRoot(this); }
    }

    public Member Member { get; private set; }

    protected override void OnLoad(EventArgs e)
    {
        this.Member = HttpContext.Current.User as Member;
        this.PageUrl = this.Page.AppRelativeVirtualPath.ToLower();
        base.OnLoad(e);
    }

    public string GetImage(string path)
    {
        string imageUrl = Member.ImageUrl;
        if (string.IsNullOrEmpty(imageUrl))
            return ResolveUrl(path);
        else
            return path.Replace("~/", imageUrl);
    }
}

public class SitePage : web.BasePage
{
    public SiteMasterPage RootMasterPage
    {
        [DebuggerStepThrough]
        get { return SiteMasterPage.GetRoot(this.Master); }
    }

    public Member Member { get; private set; }

    public string PostData;

    protected override void OnInit(EventArgs e)
    {
        this.Member = HttpContext.Current.User as Member;
        this.PostData = Request.Form["data"];
        if (!string.IsNullOrEmpty(this.PostData))
            log.message("http", "{1}\t{0}\t{2}", Context.Request.AppRelativeCurrentExecutionFilePath, Context.RequestIP(), this.PostData);
        if (!string.IsNullOrEmpty(this.PostData))
            web.api.PopulateObject(this.PostData, this);
        base.OnInit(e);
    }

    [AppSetting, DefaultValue("")]
    static string site_index
    {
        get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
    }

    public static string GetRes(string classname, string key)
    {
        classname = string.Format("{0}{1}", classname, SitePage.site_index);
        return (HttpContext.GetGlobalResourceObject(classname, key) as string) ?? (HttpContext.GetGlobalResourceObject(classname, "_") as string);
    }

    public static string GetRes(string classname, System.Web.UI.Page page)
    {
        string key = page.AppRelativeVirtualPath.ToLower();
        int n1 = 0, n2 = 0;
        if (key.StartsWith("~/"))
            n1 = 2;
        if (key.EndsWith(".aspx"))
            n2 = key.Length - 5;
        if ((n1 > 0) || (n2 > 0))
            key = key.Substring(n1, n2 - n1);
        key = key.Replace(".", "_").Replace("/", "_");
        return SitePage.GetRes(classname, key);
    }

    Dictionary<string, string> randID_dict;
    public string randID(string id) { return urls.randID(ref this.randID_dict, id); }

    public string GetImage(string path)
    {
        string imageUrl = Member.ImageUrl;
        if (string.IsNullOrEmpty(imageUrl))
            return ResolveUrl(path);
        else
            return path.Replace("~/", imageUrl);
    }
}

public class root_aspx : SitePage 
{
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class SiteControl : web.BaseUserControl
{
    public Member Member { get; private set; }
    public string PostData;
    public string css_display = "none";

    protected override void OnInit(EventArgs e)
    {
        this.Member = HttpContext.Current.User as Member;
        this.PostData = Request.Form["data"];
        if (!string.IsNullOrEmpty(this.PostData))
            log.message("http", "{1}\t{0}\t{2}", Context.Request.AppRelativeCurrentExecutionFilePath, Context.RequestIP(), this.PostData);
        if (!string.IsNullOrEmpty(this.PostData))
            web.api.PopulateObject(this.PostData, this);
        base.OnInit(e);
    }

    Dictionary<string, string> randID_dict;
    public string randID(string id) { return urls.randID(ref this.randID_dict, id); }

    public string GetImage(string path)
    {
        string imageUrl = Member.ImageUrl;
        if (string.IsNullOrEmpty(imageUrl))
            return ResolveUrl(path);
        else
            return path.Replace("~/", imageUrl);
    }

    /// <summary>
    /// 默认第三方支付方式
    /// </summary>
    [SqlSetting("ThirdPay", "ThirdpaytDefault")]
    public string ThirdpaytDefault
    {
        get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), _Global.DefaultCorpID); }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class RecoveryPassword
{
    [JsonProperty("t1")]
    public string ACNT;
    [JsonProperty("t2")]
    public string SecPassword1;
    [JsonProperty("t3")]
    public string Password1;
    [JsonProperty("t4")]
    public string Password2;
    [JsonProperty("t5")]
    public string SecPassword2;
    [JsonProperty("t6")]
    public string VerifyCode;
    [JsonProperty("t7")]
    public string Token;

    [ObjectInvoke]
    static object recovery_password(RecoveryPassword command, string json_s, params object[] args)
    {
        try
        {
            command.ACNT = Member.prefix_remove(command.ACNT * text.ValidAsACNT);
            HttpContext context = HttpContext.Current;
            User user = context.User as User;
            if (string.Compare(command.VerifyCode ?? "", context.Session[VerifyImage.GetKey(VerifyImage.Type_Recovery)] as string, true) != 0)
                return new { status = "verify" };
            bool createToken = string.IsNullOrEmpty(command.Token);
            string secPassword = createToken ? command.SecPassword1 : command.SecPassword2;
            if (string.IsNullOrEmpty(secPassword))
                return new { status = "user" }; // 
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                MemberRow row = sqlcmd.ToObject<MemberRow>("select ID, ACNT, sec_pwd from Member nolock where CorpID={0} and ACNT='{1}'", _Global.DefaultCorpID, command.ACNT);//User.Manager.DefaultCorpID
                if (row == null)
                    return new { status = "user" }; // user not exist
                string sec_pwd = text.EncodePassword(row.ACNT, secPassword);
                if (sec_pwd != row.SecurityPassword)
                    return new { status = "user" }; // password error
                Guid guid;
                if (createToken)
                {
                    guid = Guid.NewGuid();
                    context.Session["recovery-token"] = guid.ToString();
                }
                else
                {
                    guid = new Guid(context.Session["recovery-token"] as string);
                    context.Session.Remove("recovery-token");
                }
                byte[] data1 = guid.ToByteArray();
                byte[] data2 = Crypto.TripleDES.Encrypt(data1, secPassword, Encoding.UTF8.GetBytes("recovery password"));
                string token = Convert.ToBase64String(data2);
                if (createToken)
                {
                    return new { status = "accept", t7 = token };
                }
                else
                {
                    if (token != command.Token)
                        return new { status = "token", t7 = token }; // token not match
                    if (string.IsNullOrEmpty(command.Password2) || (command.Password1 != command.Password2))
                        return new { status = "user" }; // password error
                    row = new MemberRowCommand()
                    {
                        ID = row.ID.Value,
                        Password = command.Password1,
                    }.update(json_s, args);
                    return new { status = "ok" };
                }
            }
        }
        catch (Exception ex)
        {
            return new { status = "error", msg = ex.Message };
        }
    }
}


namespace web.cache
{
    public class Anno : WebTools.ListCache<Anno, string>
    {
        [SqlSetting("Cache", "Anno"), DefaultValue(30000.0)]
        public override double LifeTime
        {
            get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
            set { }
        }

        public override void Update(SqlCmd sqlcmd, string key, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd ?? args.GetValue<SqlCmd>(0)))
            {
                List<string> anno = new List<string>();
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select top(5) Text from Anno nolock where (CorpID={0} or CorpID=0) and Locked=0 order by CorpID, Sort asc", _Global.DefaultCorpID))
                    anno.Add(r.GetString(r.GetOrdinal("Text")));
                base.Rows = anno;
            }
        }
    }
}