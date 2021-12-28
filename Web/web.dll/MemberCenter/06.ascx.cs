using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BU;
using web;

public partial class MemberCenter06_ascx : SiteControl
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        try
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                MemberBankCardRowCommand cardrow_cmd = new MemberBankCardRowCommand()
                {
                    getRowOnly = true,
                    throwNotFound = false,
                    MemberID = this.Member.ID,
                    Index = 1,
                    newIndex = 1,
                    AccountName = this.AccountName,
                    BankName = this.BankName,
                    CardID = this.CardID,
                    Loc1 = this.Loc1,
                    Loc2 = this.Loc2,
                    Loc3 = this.Loc3,
                };
                this.cardrow = cardrow_cmd.update(null, sqlcmd);
                if (!string.IsNullOrEmpty(this.PostData))
                {
                    if (this.cardrow == null)
                    {
                        cardrow_cmd.getRowOnly = false;
                        cardrow_cmd.throwNotFound = true;
                        this.cardrow = cardrow_cmd.insert(this.PostData, sqlcmd);
                    }
                    MemberRow member_row = MemberRow.GetMember(sqlcmd, this.Member.ID, null, null, "sec_pwd", "Balance");
                    if (text.EncodePassword(member_row.ACNT, this.SecPassword) != member_row.SecurityPassword)
                    {
                        errtitle = "提示：";
                        errmsg = "安全密码不正确";
                    }
                    else if (this.Member.UndisposedWithdrawalsCount(sqlcmd) > 0)
                    {
                        errtitle = "提示：";
                        errmsg = "请稍等，您上次的提款还未处理！";
                    }
                    else if (member_row.Balance < this.Amount)
                    {
                        errtitle = "提示：";
                        errmsg = "您的主账户余额不足，如果您的金额在游戏厅中请转回主账户后再“提交”！";
                    }
                    else
                    {
                        this.tran_result = new tran.Cash.WithdrawalRowCommand()
                        {
                            op_Insert = true,
                            LogType = BU.LogType.Withdrawal,
                            UserType = BU.UserType.Member,
                            UserID = this.Member.ID,
                            Amount = this.Amount,
                            a_BankName = this.cardrow.BankName,
                            a_CardID = this.cardrow.CardID,
                            a_Name = this.cardrow.AccountName,
                            b_TranMemo = this.b_TranMemo,
                        }.Execute(sqlcmd, this.PostData);
                        #region //
                        //this.tran_result2 = new MemberTranRowCommand()
                        //{
                        //    LogType = BU.LogType.Withdrawal,
                        //    MemberID = this.Member.ID,
                        //    Amount1 = this.Amount,
                        //    b_BankName = this.cardrow.BankName,
                        //    b_CardID = this.cardrow.CardID,
                        //    b_Name = this.cardrow.AccountName,
                        //    b_TranMemo = this.b_TranMemo,
                        //}.Insert(this.PostData, sqlcmd);
                        #endregion
                        errmsg = "您的提款要求已送出，我们会尽快为您处理";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.error_msg(ex);
            this.errmsg = ex.Message;
        }
    }

    public string errmsg;
    public string errtitle;

    public MemberBankCardRow cardrow;
    //public MemberTranRow tran_result2;
    public tran.Cash.CashRowData tran_result;

    [JsonProperty("n1")]
    string AccountName;
    [JsonProperty("n2")]
    string BankName;
    [JsonProperty("n3")]
    string CardID;
    [JsonProperty("n4")]
    string Loc1;
    [JsonProperty("n5")]
    string Loc2;
    [JsonProperty("n6")]
    string Loc3;
    [JsonProperty("n7")]
    decimal? Amount;
    [JsonProperty("n8")]
    string SecPassword;
    [JsonProperty("n9")]
    string b_TranMemo;
}