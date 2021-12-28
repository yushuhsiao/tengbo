using BU;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using System.Web;
using web;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberExist : IRowCommand
{
    [JsonProperty("id")]
    public string ACNT;

    [ObjectInvoke, api.Async]
    static object check(MemberExist command, string json_s, params object[] args)
    {
        command.ACNT *= text.ValidAsACNT;
        if (!string.IsNullOrEmpty(command.ACNT))
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                Thread.Sleep(1000);
                int cnt = sqlcmd.ExecuteScalar<int>("select count(*) from Member nolock where CorpID={0} and ACNT='{1}'", User.Manager.DefaultCorpID, command.ACNT) ?? 0;
                if (cnt == 0)
                    return "ok";
            }
        }
        throw new RowException(RowErrorCode.AlreadyExist);
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberRegister : MemberRowCommand, IRowCommand
{
    public override int? CorpID
    {
        get { return User.Manager.DefaultCorpID; }
        set { }
    }

    [JsonProperty("id")]
    public override string ACNT
    {
        get { return base.ACNT; }
        set { base.ACNT = value; }
    }

    [JsonProperty("pwd1")]
    public override string Password
    {
        get { return base.Password; }
        set { base.Password = value; }
    }

    [JsonProperty("name")]
    public override string Name
    {
        get { return base.Name; }
        set { base.Name = value; }
    }

    [JsonProperty("tel")]
    public override string Tel
    {
        get { return base.Tel; }
        set { base.Tel = value; }
    }

    [JsonProperty("qq")]
    public override string QQ
    {
        get { return base.QQ; }
        set { base.QQ = value; }
    }

    [JsonProperty("mail")]
    public override string Mail
    {
        get { return base.Mail; }
        set { base.Mail = value; }
    }

    [JsonProperty("intro")]
    public override string Introducer
    {
        get { return base.Introducer; }
        set { base.Introducer = value; }
    }


    public override string SecPassword
    {
        get { return base.SecPassword; }
        set { base.SecPassword = value; }
    }
    public override string Locked
    {
        get { return base.Locked; }
        set { base.Locked = value; }
    }
    public override CurrencyCode? Currency
    {
        get { return CurrencyCode.CNY; }
        set { }
    }
    public override DateTime? Birthday
    {
        get { return base.Birthday; }
        set { base.Birthday = value; }
    }
    public override string Memo
    {
        get { return base.Memo; }
        set { base.Memo = value; }
    }

    [ObjectInvoke, api.Async]
    static object register(MemberRegister command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            command.ACNT *= text.ValidAsACNT2;
            if (string.IsNullOrEmpty(command.ACNT))
                throw new RowException(RowErrorCode.reg_ACNT_Null);
            MemberRow row = sqlcmd.GetRow<MemberRow>("select * from Member nolock where CorpID={0} and ACNT='{1}'", command.CorpID, command.ACNT);
            if (row != null)
                throw new RowException(RowErrorCode.MemberAlreadyExist);
            row = command.insert(json_s, sqlcmd);
            return Member.Login(HttpContext.Current, sqlcmd, row, null, json_s);
        }
    }
}
