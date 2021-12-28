using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web;

public partial class MemberCenter03_ascx : SiteControl
{
    public MemberBankCardRow cardrow;

    public string errmsg;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            web.MemberBankCardRowCommand cmd = new web.MemberBankCardRowCommand()
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
            this.cardrow = cmd.update(this.PostData);
            if (!string.IsNullOrEmpty(this.PostData))
            {
                cmd.getRowOnly = false;
                cmd.throwNotFound = true;
                try
                {
                    if (this.cardrow == null)
                        this.cardrow = cmd.insert(this.PostData, sqlcmd);
                    else
                        this.cardrow = cmd.update(this.PostData, sqlcmd);
                    this.errmsg = "银行资料已经更新!!";
                }
                catch (Exception ex)
                {
                    log.error_msg(ex);
                    this.errmsg = ex.Message;
                }
            }
            //this.cardrow = this.cardrow;
            //web.MemberBankCardRowCommand cmd = new web.MemberBankCardRowCommand()
            //{
            //    throwNotFound = false,
            //    MemberID = this.Member.ID,
            //    Index = 1,
            //    AccountName = this.AccountName,
            //    BankName = this.BankName,
            //    CardID = this.CardID,
            //    Loc1 = this.Loc1,
            //    Loc2 = this.Loc2,
            //    Loc3 = this.Loc3,
            //};
            //if (string.IsNullOrEmpty(this.PostData))
            //{
            //    cmd.getRowOnly = true;
            //    this.row = cmd.update(this.PostData, sqlcmd);
            //}
            //else
            //{
            //    cmd.getRowOnly = false;
            //    this.row = cmd.update(this.PostData, sqlcmd);
            //    if (this.row == null)
            //        this.row = cmd.insert(this.PostData, sqlcmd);
            //    this.errmsg = "银行资料已经更新!!";
            //}            
            //this.row = this.row ?? new MemberBankCardRow();
        }
    }

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
}