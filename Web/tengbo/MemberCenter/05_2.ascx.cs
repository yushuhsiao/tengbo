using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using web;

public partial class MemberCenter05_2_ascx : SiteControl
{
    public string errmsg;

    public MemberTranRow row;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!string.IsNullOrEmpty(this.PostData))
        {
            try
            {
                if (this.tranid.HasValue)
                {
                    this.row = new MemberTranRowCommand()
                    {
                        ID = this.tranid,
                        b_TranSerial = this.n04,
                        b_Name = this.n05,
                        b_CardID = this.n06,
                        b_TranMemo = this.n07,
                    }.Update(this.PostData);
                    row.ID = null;
                }
                else
                {
                    using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                    {
                        BankRow bank = sqlcmd.GetRow<BankRow>("select * from Bank nolock where [Name]=N'{0}'", AllBankList[this.bank_index.Value]);
                        BankCardRow card = sqlcmd.ToObject<BankCardRow>(@"declare @cnt int
select @cnt = count(*) from BankCard nolock where BankName=N'{0}' and Locked=0 and CorpID={1}
select * from (
select Row_Number() over (order by ID) as rowid, * from BankCard nolock
where BankName=N'{0}' and Locked=0 and CorpID={1}) a
where rowid = convert(int, rand() * @cnt) + 1", bank.Name, _Global.DefaultCorpID);
                        this.row = new MemberTranRowCommand()
                        {
                            LogType = BU.LogType.Deposit,
                            MemberID = this.Member.ID,
                            Amount1 = this.amount,
                            a_BankName = card.BankName,
                            a_CardID = card.CardID,
                            a_Name = card.AccName,
                        }.Insert(this.PostData, sqlcmd);
                    }
                }
            }
            catch (Exception ex)
            {
                log.error_msg(ex);
            }
        }
    }

    [JsonProperty("n01")]
    int? bank_index;
    [JsonProperty("n02")]
    decimal? amount;
    [JsonProperty("n03")]
    Guid? tranid;
    [JsonProperty("n04")]
    string n04;
    [JsonProperty("n05")]
    string n05;
    [JsonProperty("n06")]
    string n06;
    [JsonProperty("n07")]
    string n07;

    public static string[] AllBankList = new string[] { "中国工商银行", "中国农业银行", "中国建设银行", "中国招商银行" };
    public string banklist_name;
    public int banklist_index;

    public IEnumerable<int> banklist()
    {
        for (int i = 0; i < AllBankList.Length; i++)
        {
            this.banklist_index = i;
            this.banklist_name = AllBankList[i];
            int count = (from bank in web.cache.Bank.Instance.Rows
                         where (bank.BankName == AllBankList[i]) && (bank.CorpID == this.Member.CorpID) && (bank.GroupID == this.Member.GroupID) && (bank.LogType == BU.LogType.Deposit)
                         select bank.BankName).Count();

            yield return count;
        }
    }
}

namespace web.cache
{
    public class Bank : WebTools.ListCache<Bank, BankCardRow>
    {

        [SqlSetting("Cache", "Banklist"), DefaultValue(5000.0)]
        public override double LifeTime
        {
            get { return app.config.GetValue<double>(MethodBase.GetCurrentMethod()); }
            set { }
        }

        public int GetCount(string name)
        {
            return (from bank in web.cache.Bank.Instance.Rows where bank.BankName == name select bank.BankName).Count();
        }

        public override void Update(SqlCmd sqlcmd, string key, params object[] args)
        {
            List<BankCardRow> rows = new List<BankCardRow>();
            using (DB.Open(DB.Name.Main, DB.Access.Read, out sqlcmd, sqlcmd ?? args.GetValue<SqlCmd>(0)))
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select b.BankName, b.ID, b.CardID, b.CorpID, b.GroupID, b.LogType
from Bank a with(nolock)
left join BankCard b with(nolock)
on a.Name=b.BankName where a.Locked = 0 and b.Locked=0 and b.CorpID={0}", _Global.DefaultCorpID
                                                                        ))
                    rows.Add(r.ToObject<BankCardRow>());
            base.Rows = rows;
        }
    }
}