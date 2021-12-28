using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Tools;
using System.Data.SqlClient;
using BU;
using System.Text;

namespace web.payment
{
    /// <summary>
    /// Dinpay V3.0 callback 的摘要说明
    /// </summary>
    public class dinpay_v3 : web.tran.Cash.ThirdPaymentRowCommand, IHttpHandler
    {
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            try
            {
                log.message("dinpay", context.Request.Url.Query);

                //获取智付GET过来反馈信息
                //商号号
                string merchant_code = context.Request.Form["merchant_code"].ToString().Trim();
                //通知类型
                string notify_type = context.Request.Form["notify_type"].ToString().Trim();
                //通知校验ID
                string notify_id = context.Request.Form["notify_id"].ToString().Trim();
                //接口版本
                string interface_version = context.Request.Form["interface_version"].ToString().Trim();
                //签名方式
                string sign_type = context.Request.Form["sign_type"].ToString().Trim();
                //签名
                string dinpaySign = context.Request.Form["sign"].ToString().Trim();
                //商家订单号
                string order_no = context.Request.Form["order_no"].ToString().Trim();
                //商家订单时间
                string order_time = context.Request.Form["order_time"].ToString().Trim();
                //商家订单金额
                string order_amount = context.Request.Form["order_amount"].ToString().Trim();
                //回传参数
                string extra_return_param = context.Request.Form["extra_return_param"].ToString().Trim();
                //智付交易定单号
                string trade_no = context.Request.Form["trade_no"].ToString().Trim();
                //智付交易时间
                string trade_time = context.Request.Form["trade_time"].ToString().Trim();
                //交易状态 SUCCESS 成功  FAILED 失败
                string trade_status = context.Request.Form["trade_status"].ToString().Trim();
                //银行交易流水号
                string bank_seq_no = context.Request.Form["bank_seq_no"].ToString().Trim();
                /**
                 *签名顺序按照参数名a到z的顺序排序，若遇到相同首字母，则看第二个字母，以此类推，
                *同时将商家支付密钥key放在最后参与签名，组成规则如下：
                *参数名1=参数值1&参数名2=参数值2&……&参数名n=参数值n&key=key值
                **/

                //组织订单信息
                string signStr = "";

                if (bank_seq_no != "")
                    signStr = signStr + "bank_seq_no=" + bank_seq_no + "&";
                if (extra_return_param != "")
                    signStr = signStr + "extra_return_param=" + extra_return_param + "&";

                signStr = signStr + "interface_version=V3.0" + "&";
                signStr = signStr + "merchant_code=" + merchant_code + "&";

                if (notify_id != "")
                    signStr = signStr + "notify_id=" + notify_id + "&notify_type=page_notify&";

                signStr = signStr + "order_amount=" + order_amount + "&";
                signStr = signStr + "order_no=" + order_no + "&";
                signStr = signStr + "order_time=" + order_time + "&";
                signStr = signStr + "trade_no=" + trade_no + "&";
                signStr = signStr + "trade_status=" + trade_status + "&";

                if (trade_time != "")
                    signStr = signStr + "trade_time=" + trade_time + "&";


                extra_return_param = Encoding.ASCII.GetString(Convert.FromBase64String(extra_return_param));
                DinpayRowData.MP mp = Newtonsoft.Json.JsonConvert.DeserializeObject<DinpayRowData.MP>(extra_return_param);
                if (mp == null) return;

                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                {
                    DinpayRowData chn = _null<DinpayRowCommand>.value.GetRow(sqlcmd, mp.ChannelID);
                    signStr = signStr + "key=" + chn.sec_key;

                    //将组装好的信息MD5签名
                    signStr = FormsAuthentication.HashPasswordForStoringInConfigFile(signStr, "md5").ToLower(); //注意与支付签名不同  此处对String进行加密

                    //比较智付返回的签名串与商家这边组装的签名串是否一致
                    if (dinpaySign.ToLower() == signStr.ToLower() && trade_status.ToLower() == "SUCCESS".ToLower())
                    {
                        base.ID = mp.TransactionID;
                        base.op_Finish = true;
                        base.op_Delete = true;
                        tran.Cash.ThirdPaymentRowData tranRow = base.Execute(sqlcmd, null, null);
                        context.Response.Redirect(mp.SourceSite + "MemberCenter/pay_Finish.aspx?status=1&amt=" + order_amount);
                    }
                    else
                    {
                        context.Response.Redirect(mp.SourceSite + "MemberCenter/pay_Finish.aspx?status=0&amt=" + order_amount);
                    }
                }
            }
            catch (Exception ex)
            {
                log.message("dinpay", ex.ToString());
            }
        }

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }
    }
}