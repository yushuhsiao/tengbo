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
using Tools;
using web;

public abstract partial class MemberCenter05_2_ascx : SiteControl
{
    public string errmsg;

    //public MemberTranRow row3;
    public tran.Cash.CashRowData row;
    public bool tran_end;

    protected abstract string[] AllBankList { get; }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (string.IsNullOrEmpty(this.PostData))
            return;
        this.tran_end = false;
        try
        {
            if (this.tranid.HasValue)
            {
                #region //
                //this.row3 = new MemberTranRowCommand()
                //{
                //    ID = this.tranid,
                //    b_TranSerial = this.n04,
                //    b_Name = this.n05,
                //    b_CardID = this.n06,
                //    b_TranMemo = this.n07,
                //}.Update(this.PostData);
                //row3.ID = null;
                #endregion
                this.row = new tran.Cash.DepositRowCommand()
                {
                    op_Update = true,
                    ID = this.tranid,
                    b_TranSerial = this.n04,
                    b_Name = this.n05,
                    b_CardID = this.n06,
                    b_TranMemo = this.n07,
                }.Execute(null, this.PostData);
                tran_end = true;
            }
            else
            {
                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        BankCardRowData card = CashChannelRow.Cache.GetInstance(sqlcmd, null).RandomGetRow() as BankCardRowData;
                        if (IsUsableCard(card))
                        {
                            if (card.BankName == AllBankList[this.bank_index.Value])
                            {
                                #region //
                                //this.row3 = new MemberTranRowCommand()
                                //{
                                //    LogType = BU.LogType.Deposit,
                                //    MemberID = this.Member.ID,
                                //    Amount1 = this.amount,
                                //    a_BankName = card.BankName,
                                //    a_CardID = card.CardID,
                                //    a_Name = card.AccName,
                                //}.Insert(this.PostData, sqlcmd);
                                #endregion
                                this.row = new tran.Cash.DepositRowCommand()
                                {
                                    op_Insert = true,
                                    LogType = BU.LogType.Deposit,
                                    UserType = BU.UserType.Member,
                                    UserID = this.Member.ID,
                                    Amount = this.amount,
                                    CashChannelID = card.ID,
                                    a_BankName = card.BankName,
                                    a_CardID = card.CardID,
                                    a_Name = card.AccName,
                                }.Execute(sqlcmd, this.PostData);
                                break;
                            }
                        }
                    }
                    #region //
//                    BankRow bank = sqlcmd.GetRow<BankRow>("select * from Bank nolock where [Name]=N'{0}'", AllBankList[this.bank_index.Value]);
//                    BankCardRow card = sqlcmd.ToObject<BankCardRow>(@"declare @cnt int
//select @cnt = count(*) from BankCard nolock where BankName=N'{0}' and Locked=0 and CorpID={1}
//select * from (
//select Row_Number() over (order by ID) as rowid, * from BankCard nolock
//where BankName=N'{0}' and Locked=0 and CorpID={1}) a
//where rowid = convert(int, rand() * @cnt) + 1", bank.Name, _Global.DefaultCorpID);
//                    this.row = new MemberTranRowCommand()
//                    {
//                        LogType = BU.LogType.Deposit,
//                        MemberID = this.Member.ID,
//                        Amount1 = this.amount,
//                        a_BankName = card.BankName,
//                        a_CardID = card.CardID,
//                        a_Name = card.AccName,
//                    }.Insert(this.PostData, sqlcmd);
                    #endregion
                }
            }
        }
        catch (Exception ex)
        {
            log.error_msg(ex);
        }
    }

    protected bool IsUsableCard(BankCardRowData row)
    {
        if (row == null) return false;
        if (row.LogType != LogType.Deposit) return false;
        if (row.CorpID != _Global.DefaultCorpID) return false;
        if (row.Locked != Locked.Active) return false;
        return true;
    }

    Dictionary<string, int> banks_count;
    protected int GetCount(string bankname)
    {
        if (this.banks_count == null)
        {
            this.banks_count = new Dictionary<string, int>();
            foreach (CashChannelRow r1 in CashChannelRow.Cache.Instance.Rows)
            {
                BankCardRowData r2 = r1 as BankCardRowData;
                if (!IsUsableCard(r2)) continue;
                if (banks_count.ContainsKey(r2.BankName))
                    banks_count[r2.BankName]++;
                else
                    banks_count[r2.BankName] = 1;
            }
        }
        int ret = 0;
        banks_count.TryGetValue(bankname, out ret);
        return ret;
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


    //public static string[] AllBankList = new string[] { "中国工商银行", "中国农业银行", "中国建设银行", "中国招商银行" };
    //public string banklist_name;
    //public int banklist_index;

    //public IEnumerable<int> banklist()
    //{
    //    for (int i = 0; i < AllBankList.Length; i++)
    //    {
    //        this.banklist_index = i;
    //        this.banklist_name = AllBankList[i];
    //        int count = (from bank in web.cache.Bank.Instance.Rows
    //                     where (bank.BankName == AllBankList[i]) && (bank.CorpID == this.Member.CorpID) && (bank.GroupID == this.Member.GroupID) && (bank.LogType == BU.LogType.Deposit)
    //                     select bank.BankName).Count();

    //        yield return count;
    //    }
    //}
}
