using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using BU;
using Tools;
using web;
using System.Text;
using System.Collections.Generic;

public partial class ecpss_aspx : System.Web.UI.Page
{
    public static readonly Dictionary<string, string> banklist;
    static ecpss_aspx()
    {
        banklist = new Dictionary<string, string>();
        banklist["ICBC"] = "工商银行";
        banklist["ABC"] = "农业银行";
        banklist["BOCSH"] = "中国银行";
        banklist["CCB"] = "建设银行";
        banklist["CMB"] = "招商银行";
        banklist["SPDB"] = "浦发银行";
        banklist["GDB"] = "广发银行";
        banklist["BOCOM"] = "交通银行";
        banklist["PSBC"] = "邮政储蓄银行";
        banklist["CNCB"] = "中信银行";
        banklist["CMBC"] = "民生银行";
        banklist["CEB"] = "光大银行";
        banklist["HXB"] = "华夏银行";
        banklist["CIB"] = "兴业银行";
        banklist["BOS"] = "上海银行";
        banklist["SRCB"] = "上海农商";
        banklist["PAB"] = "平安银行";
        banklist["BCCB"] = "北京银行";
        banklist["BOC"] = "中行（大额）";
        //banklist["NOCARD"] = "银联无卡支付";
        banklist["UNIONPAY"] = "银联选择银行页面";
    }

    /// <summary>
    /// 订单描述
    /// </summary>
    protected string OrderDesc;
    /// <summary>
    /// 备注
    /// </summary>
    protected string Remark;
    /// <summary>
    /// 服务器异步通知路径
    /// </summary>
    protected string AdviceURL;
    /// <summary>
    /// 页面跳转同步通知页面
    /// </summary>
    protected string ReturnURL;
    /// <summary>
    /// 订单号
    /// </summary>
    protected string BillNo;
    /// <summary>
    /// 商户号
    /// </summary>
    protected string MerNo;
    /// <summary>
    /// 金额
    /// </summary>
    protected string Amount;
    /// <summary>
    /// 签名信息
    /// </summary>
    protected string SignInfo;
    /// <summary>
    /// 银行编码
    /// </summary>
    protected string defaultBankNumber;
    /// <summary>
    /// 请求时间
    /// </summary>
    protected string orderTime;
    /// <summary>
    /// 物品信息
    /// </summary>
    protected string products;

    protected string reqURL_onLine;
    protected string authorizationURL;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        // 设置 Response编码格式为GB2312
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
        Member member = HttpContext.Current.User as Member;
        if (member == null)
        {
            if (Request.Form["authorizationURL"] == null)
            {
                Response.Redirect("~/");
            }
        }
        else
        {
            decimal? amount = Request.Form["n1"].ToDecimal();
            if (!amount.HasValue)
            { Response.Redirect("~/"); return; }

            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                EcpssRowData chn = _null<EcpssRowCommand>.value.GetRow(sqlcmd, Request.Form["n0"].ToGuid());
                tran.Cash.ThirdPaymentRowData tran_row = new web.payment.ecpss()
                {
                    op_Insert = true,
                    LogType = chn.LogType,
                    UserType = UserType.Member,
                    UserID = member.ID,
                    Amount = amount,
                    CashChannelID = chn.ID,
                }.Execute(sqlcmd, null);
                //MemberTranRow tran_row = new web.payment.ecpss() { LogType = BU.LogType.Ecpss, MemberID = member.ID, Amount1 = amount, }.Insert(null, sqlcmd);
                reqURL_onLine = string.Format("http://{0}/MemberCenter/ecpss.aspx", chn.alias_domain);
                authorizationURL = chn.authorizationURL;
                MerNo = chn.merhantId;
                BillNo = tran_row.SerialNumber + "_" + Convert.ToBase64String(tran_row.ID.ToByteArray());
                Amount = string.Format("{0:0.00}", amount);
                OrderDesc = chn.OrderDesc;
                Remark = chn.Remark;
                products = chn.Products;
                AdviceURL = ReturnURL = string.Format("http://{0}/MemberCenter/ecpss.ashx", chn.alias_domain);//Request.Form["p8_Url"];
                SignInfo = FormsAuthentication.HashPasswordForStoringInConfigFile(MerNo + "&" + BillNo + "&" + Amount + "&" + ReturnURL + "&" + chn.MD5key, "MD5");
                defaultBankNumber = Request.Form["n3"] ?? "";
                orderTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }
    }
}
