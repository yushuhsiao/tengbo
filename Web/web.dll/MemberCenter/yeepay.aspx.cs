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
using com.yeepay.icc;
using System.Data.SqlClient;
using BU;
using Tools;
using web;
using System.Text;
using System.Collections.Generic;

public partial class yeepay_aspx : System.Web.UI.Page
{
    public static readonly Dictionary<string, string> banklist;
    static yeepay_aspx()
    {
        banklist = new Dictionary<string, string>();
        banklist["ICBC-NET-B2C"] = "工商银行";
        banklist["CCB-NET-B2C"] = "建设银行";
        banklist["ABC-NET-B2C"] = "中国农业银行";
        banklist["CMBCHINA-NET-B2C"] = "招商银行";
        banklist["BOC-NET-B2C"] = "中国银行";
        banklist["BOCO-NET-B2C"] = "交通银行";
        banklist["POST-NET-B2C"] = "中国邮政储蓄";
        banklist["CEB-NET-B2C"] = "光大银行";
        banklist["GDB-NET-B2C"] = "广发银行";
        banklist["CIB-NET-B2C"] = "兴业银行";
        banklist["SPDB-NET-B2C"] = "上海浦东发展银行";
        banklist["CMBC-NET-B2C"] = "中国民生银行";
        banklist["ECITIC-NET-B2C"] = "中信银行";
        banklist["PINGANBANK-NET-B2C"] = "平安银行";
        banklist["SDB-NET-B2C"] = "深圳发展银行";
        banklist["BCCB-NET-B2C"] = "北京银行";
        banklist["BJRCB-NET-B2C"] = "北京农村商业银行";
        banklist["SHB-NET-B2C"] = "上海银行";
        banklist["NBCB-NET-B2C"] = "宁波银行";
        banklist["HXB-NET-B2C"] = "华夏银行";
        banklist["NJCB-NET-B2C"] = "南京银行";
        banklist["HKBEA-NET-B2C"] = "东亚银行";
        banklist["HZBANK-NET-B2C"] = "杭州银行";
        banklist["SRCB-NET-B2C"] = "上海农商银行";
        banklist["CZ-NET-B2C"] = "浙商银行";
    }
  // 1

    /// <summary>
    /// 商家號
    /// </summary>
    protected string p1_MerId;
    /// <summary>
    /// 商户平台订单号
    /// 若不为""，提交的订单号必须在自身账户交易中唯一;为""时，易宝支付会自动生成随机的商户订单号.
    /// </summary>
    protected string p2_Order;
    /// <summary>
    /// 交易金额  精确两位小数，最小值为0.01,为持卡人实际要支付的金额.
    /// </summary>
    protected string p3_Amt;
    /// <summary>
    /// 交易币种,固定值"CNY".
    /// </summary>
    protected string p4_Cur;
    /// <summary>
    /// 商品名称
    /// 用于支付时显示在易宝支付网关左侧的订单产品信息.
    /// </summary>
    protected string p5_Pid;
    /// <summary>
    /// 商品种类
    /// </summary>
    protected string p6_Pcat;

    // 2

    /// <summary>
    /// 商品描述
    /// </summary>
    protected string p7_Pdesc;
    /// <summary>
    /// 商户接收支付成功数据的地址,支付成功后易宝支付会向该地址发送两次成功通知.
    /// </summary>
    protected string p8_Url;
    /// <summary>
    /// 送货地址
    /// 为“1”: 需要用户将送货地址留在易宝支付系统;为“0”: 不需要，默认为 ”0”.
    /// </summary>
    protected string p9_SAF;
    /// <summary>
    /// 商户扩展信息
    /// 商户可以任意填写1K 的字符串,支付成功时将原样返回.
    /// </summary>
    protected string pa_MP;
    /// <summary>
    /// 银行编码
    /// 默认为""，到易宝支付网关.若不需显示易宝支付的页面，直接跳转到各银行、神州行支付、骏网一卡通等支付页面，该字段可依照附录:银行列表设置参数值.
    /// </summary>
    protected string pd_FrpId;

    // 3

    /// <summary>
    /// 应答机制
    /// 默认为"1": 需要应答机制;
    /// </summary>
    protected string pr_NeedResponse; 
    protected string hmac;

    protected string reqURL_onLine;
    protected string authorizationURL;
    //{
    //    get { return chn.authorizationURL; }
    //}
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
                YeepayRowData chn = _null<YeepayRowCommand>.value.GetRow(sqlcmd, Request.Form["n0"].ToGuid());
                tran.Cash.ThirdPaymentRowData tran_row = new web.payment.yeepay()
                {
                    op_Insert = true,
                    LogType = chn.LogType,
                    UserType = UserType.Member,
                    UserID = member.ID,
                    Amount = amount,
                    CashChannelID = chn.ID,
                }.Execute(sqlcmd, null);
                //MemberTranRow tran_row = new web.payment.yeepay() { LogType = BU.LogType.YeePay, MemberID = member.ID, Amount1 = amount, }.Insert(null, sqlcmd);
                reqURL_onLine = string.Format("http://{0}/MemberCenter/yeepay.aspx", chn.alias_domain);
                authorizationURL = chn.authorizationURL;
                p1_MerId = chn.merhantId;
                p2_Order = tran_row.SerialNumber;// Request.Form["p2_Order"];
                p3_Amt = string.Format("{0:0.00}", amount);// Request.Form["p3_Amt"];
                p4_Cur = "CNY";
                p5_Pid = chn.p5_Pid;  // Request.Form["p5_Pid"];
                p6_Pcat = chn.p6_Pcat; // Request.Form["p6_Pcat"];
                p7_Pdesc = chn.p7_Pdesc; //Request.Form["p7_Pdesc"];
                p8_Url = string.Format("http://{0}/MemberCenter/yeepay.ashx", chn.alias_domain);//Request.Form["p8_Url"];
                p9_SAF = "0";
                pa_MP = Newtonsoft.Json.JsonConvert.SerializeObject(new YeepayRowData.MP()
                {
                    ChannelID = chn.ID,
                    TransactionID = tran_row.ID,
                    SourceSite = string.Format("{0}://{1}/", Request.Url.Scheme, Request.Url.Host)
                });// Request.Form["pa_MP"];
                pa_MP = Convert.ToBase64String(Encoding.ASCII.GetBytes(pa_MP));
                pd_FrpId = Request.Form["n3"] ?? "";
                pr_NeedResponse = "1";
                hmac = Buy.CreateBuyHmac(p1_MerId, p2_Order, p3_Amt, p4_Cur, p5_Pid, p6_Pcat, p7_Pdesc, p8_Url, p9_SAF
                    , pa_MP, pd_FrpId, pr_NeedResponse, authorizationURL, chn.keyValue);
            }
        }
    }
}
