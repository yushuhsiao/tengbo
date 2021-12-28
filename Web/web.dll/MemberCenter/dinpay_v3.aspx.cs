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
using System.Text;

public partial class dinpay_v3_aspx : System.Web.UI.Page
{
    //签名信息
    public string signSrc;

    //参数编码字符集(必选)
    public string input_charset;
    //接口版本(必选)固定值:V3.0
    public string interface_version;
    //商家号（必填）
    public string merchant_code;
    //后台通知地址(必填)
    public string notify_url;
    //定单金额（必填）
    public string order_amount;
    //商家定单号(必填)
    public string order_no;
    //商家定单时间(必填)
    public string order_time;
    //签名方式(必填)
    public string sign_type;
    //商品编号(选填)
    public string product_code;
    //商品描述（选填）
    public string product_desc;
    //商品名称（必填）
    public string product_name;
    //端口数量(选填)
    public string product_num;
    //页面跳转同步通知地址(选填)
    public string return_url;
    //业务类型(必填)
    public string service_type;
    //商品展示地址(选填)
    public string show_url;
    //公用业务扩展参数（选填）
    public string extend_param;
    //公用业务回传参数（选填）
    public string extra_return_param;
    // 直联通道代码（选填）
    public string bank_code;
    //客户端IP（选填）
    public string client_ip;

    public string action_url;
    public string reqURL_onLine;

    protected void Page_Load(object sender, EventArgs e)
    {
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
                tran.Cash.ThirdPaymentRowData tran_row = new tran.Cash.ThirdPaymentRowCommand() { LogType = chn.LogType, UserType = UserType.Member, UserID = member.ID, Amount = amount, }.Execute(sqlcmd, null);
                //MemberTranRow tran_row = new web.dinpay() { LogType = BU.LogType.Dinpay, MemberID = member.ID, Amount1 = amount, }.Insert(null, sqlcmd);
                reqURL_onLine = string.Format("http://{0}/MemberCenter/dinpay.aspx", chn.alias_domain);

                input_charset = "UTF-8";                                   //参数编码字符集
                interface_version = "V3.0";                                //接口版本
                merchant_code = chn.M_ID;                                  //商家号
                notify_url = string.Format("http://{0}/MemberCenter/dinpay.ashx", chn.alias_domain); //后台通知地址
                order_amount = tran_row.Amount.ToString();                //定单金额
                order_no = tran_row.SerialNumber;                          //商家定单号
                action_url = chn.action_Url;                               //提交地址
                order_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //商家订单时间
                sign_type = "MD5";                                         //签名方式
                product_name = "GoShopping";                               //商品名称
                service_type = "direct_pay";                               //业务类型 
                product_code = "TB938205";                                 //商品编号(选填)
                product_desc = "YongerSt";                                 //商品描述（选填）
                product_num = "10";                                        //商品数量（选填）
                return_url = notify_url;                                   //页面跳转同步通知地址(选填)
                show_url = "https://www.google.com";                       //商品展示地址(选填)
                extend_param = "payment";                                  //公用业务扩展参数（选填）
                extra_return_param = Newtonsoft.Json.JsonConvert.SerializeObject(new YeepayRowData.MP()
                {
                    ChannelID = chn.ID,
                    TransactionID = tran_row.ID,
                    SourceSite = string.Format("{0}://{1}/", Request.Url.Scheme, Request.Url.Host)
                });                                                        //公用业务回传参数（选填）
                extra_return_param = Convert.ToBase64String(Encoding.ASCII.GetBytes(extra_return_param));
                bank_code =  Request.Form["n3"] ?? "";                     //直联通道代码（选填）
                client_ip = Context.RequestIP();                           //客户端IP（选填）

                //组织订单信息
                if (bank_code != "")
                    signSrc = signSrc + "bank_code=" + bank_code + "&";
                if (client_ip != "")
                    signSrc = signSrc + "client_ip=" + client_ip + "&";
                if (extend_param != "")
                    signSrc = signSrc + "extend_param=" + extend_param + "&";
                if (extra_return_param != "")
                    signSrc = signSrc + "extra_return_param=" + extra_return_param + "&";
                if (input_charset != "")
                    signSrc = signSrc + "input_charset=" + input_charset + "&";
                if (interface_version != "")
                    signSrc = signSrc + "interface_version=" + interface_version + "&";
                if (merchant_code != "")
                    signSrc = signSrc + "merchant_code=" + merchant_code + "&";
                if (notify_url != "")
                    signSrc = signSrc + "notify_url=" + notify_url + "&";
                if (order_amount != "")
                    signSrc = signSrc + "order_amount=" + order_amount + "&";
                if (order_no != "")
                    signSrc = signSrc + "order_no=" + order_no + "&";
                if (order_time != "")
                    signSrc = signSrc + "order_time=" + order_time + "&";
                if (product_code != "")
                    signSrc = signSrc + "product_code=" + product_code + "&";
                if (product_desc != "")
                    signSrc = signSrc + "product_desc=" + product_desc + "&";
                if (product_name != "")
                    signSrc = signSrc + "product_name=" + product_name + "&";
                if (product_num != "")
                    signSrc = signSrc + "product_num=" + product_num + "&";
                if (return_url != "")
                    signSrc = signSrc + "return_url=" + return_url + "&";
                if (service_type != "")
                    signSrc = signSrc + "service_type=" + service_type + "&";
                if (show_url != "")
                    signSrc = signSrc + "show_url=" + show_url + "&";

                //设置密钥
                signSrc = signSrc + "key=" + chn.sec_key;
                signSrc = FormsAuthentication.HashPasswordForStoringInConfigFile(signSrc, "md5").ToLower();
            }
        }
    }
}