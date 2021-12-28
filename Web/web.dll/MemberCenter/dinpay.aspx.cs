using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BU;
using web;
using System.Data.SqlClient;
using System.Web.Security;
using Tools;

public partial class dinpay_aspx : System.Web.UI.Page
{
    public string M_ID;
    public string MOrderID;
    public string MOAmount;
    public string MOCurrency;
    public string M_URL;
    public string M_Language;
    public string S_Name;
    public string S_Address;
    public string S_PostCode;
    public string S_Telephone;
    public string S_Email;
    public string R_Name;
    public string R_Address;
    public string R_PostCode;
    public string R_Telephone;
    public string R_Email;
    public string MOComment;
    public string MODate;
    public string State;
    public string digestinfo;

    public string action_url;
    public string reqURL_onLine;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!web.dinpay.CurrentConfig.Enabled) return;
        //dinpay.config dinpay_config = dinpay.GetConfig(_Global.DefaultCorpID);
        Member member = HttpContext.Current.User as Member;
        if (member == null)
        {
            if (Request.Params["action_url"] == null)
            {
                Response.Redirect("~/");
            }
        }
        else
        {
            decimal? amount = Request.Form["n1"].ToDecimal();
            if (!amount.HasValue)
            {
                Response.Redirect("~/");
                //Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return;
            }

            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                DinpayRowData chn = _null<DinpayRowCommand>.value.GetRow(sqlcmd, Request.Form["n0"].ToGuid());
                tran.Cash.ThirdPaymentRowData tran_row = new tran.Cash.ThirdPaymentRowCommand()
                {
                    op_Insert = true,
                    LogType = chn.LogType,
                    UserType = UserType.Member,
                    UserID = member.ID,
                    Amount = amount,
                    CashChannelID = chn.ID,
                }.Execute(sqlcmd, null);
                //MemberTranRow tran_row = new web.dinpay() { LogType = BU.LogType.Dinpay, MemberID = member.ID, Amount1 = amount, }.Insert(null, sqlcmd);
                reqURL_onLine = string.Format("http://{0}/MemberCenter/dinpay.aspx", chn.alias_domain);

                this.action_url = chn.action_Url;// dinpay_config.action;
                M_ID = chn.M_ID;// dinpay_config.M_ID;                                           //<----商家号-------->
                MODate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");        //<---不可以为空的--->
                string id_str = Convert.ToBase64String(tran_row.ID.ToByteArray());
                MOrderID = tran_row.SerialNumber; // id_str.Substring(0, id_str.Length - 2);
                //<----定单号-------->
                MOAmount = tran_row.Amount.ToString();                           //<----定单金额------> 
                MOCurrency = "1";                                             //<----币种-默认为1---->
                M_URL = string.Format("http://{0}/MemberCenter/dinpay.ashx", chn.alias_domain);
                //M_URL = string.Format("{0}://{1}/MemberCenter/dinpay.ashx", Request.Url.Scheme, Request.Url.Host); //chn.M_URL;// dinpay_config.M_URL;                                         //<--返回地址-此项默认为空不起作用-->
                M_Language = "1";
                S_Name = "test1";
                //S_Address = "dinpay";
                S_PostCode = "518000";
                S_Telephone = "0755-88833166";
                S_Email = "service@dinpay.com";
                R_Name = "test2";
                //R_Address = "dinpay";
                R_PostCode = "518000";
                R_Telephone = "0755-82384511";
                R_Email = "service@dinpay.com";
                MOComment = "dinpay";
                State = "0";

                S_Address = R_Address = id_str;

                string OrderMessage = M_ID + MOrderID + MOAmount + MOCurrency + M_URL + M_Language + S_PostCode + S_Telephone + S_Email + R_PostCode + R_Telephone + R_Email + MODate + chn.sec_key;// dinpay_config.Key;
                //Response.Write("串起来的订单信息为：" + OrderMessage + "<br>");
                string digest = FormsAuthentication.HashPasswordForStoringInConfigFile(OrderMessage, "md5").Trim().ToUpper();
                //Response.Write("加密认证为：" + digest);
                digestinfo = digest;
            }
        }
    }
}