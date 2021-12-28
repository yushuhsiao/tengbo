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
using com.yeepay.icc;
using com.yeepay.utils;
using Tools;
using System.Data.SqlClient;
using BU;
using System.Text;

namespace web.payment
{
    /// <summary>
    /// Yeepay callback 的摘要说明
    /// </summary>
    public class yeepay : web.tran.Cash.ThirdPaymentRowCommand, IHttpHandler
    {
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            try
            {
                try { log.message("yeepay", "{0}", context.Request.Url.Query); }
                catch { }
                string r8_MP = FormatQueryString.GetQueryString("r8_MP");
                r8_MP = Encoding.ASCII.GetString(Convert.FromBase64String(r8_MP));
                YeepayRowData.MP mp = Newtonsoft.Json.JsonConvert.DeserializeObject<YeepayRowData.MP>(r8_MP);
                //mp.TransactionID = new Guid("6a163a01-da49-4aec-84d0-3a55c452a32c");
                if (mp == null) return;

                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                {
                    YeepayRowData chn = _null<YeepayRowCommand>.value.GetRow(sqlcmd, mp.ChannelID);

                    // 校验返回数据包
                    //Buy.logstr(FormatQueryString.GetQueryString("r6_Order"), context.Request.Url.Query, "");
                    BuyCallbackResult result = Buy.VerifyCallback(
                        FormatQueryString.GetQueryString("p1_MerId"),
                        FormatQueryString.GetQueryString("r0_Cmd"),
                        FormatQueryString.GetQueryString("r1_Code"),
                        FormatQueryString.GetQueryString("r2_TrxId"),
                        FormatQueryString.GetQueryString("r3_Amt"),
                        FormatQueryString.GetQueryString("r4_Cur"),
                        FormatQueryString.GetQueryString("r5_Pid"),
                        FormatQueryString.GetQueryString("r6_Order"),
                        FormatQueryString.GetQueryString("r7_Uid"),
                        FormatQueryString.GetQueryString("r8_MP"),
                        FormatQueryString.GetQueryString("r9_BType"),
                        FormatQueryString.GetQueryString("rp_PayDate"),
                        FormatQueryString.GetQueryString("hmac"),
                        chn.keyValue);

                    if (string.IsNullOrEmpty(result.ErrMsg))
                    {
                        //在接收到支付结果通知后，判断是否进行过业务逻辑处理，不要重复进行业务逻辑处理
                        if (result.R1_Code == "1")
                        {
                            base.ID = mp.TransactionID;
                            base.op_Finish = true;
                            base.op_Delete = true;
                            tran.Cash.ThirdPaymentRowData tranRow = base.Execute(sqlcmd, null, null);
                            //MemberTranRow tran_row = base.Update(null, sqlcmd);
                            if (result.R9_BType == "1")
                            {
                                context.Response.Redirect(mp.SourceSite + "MemberCenter/pay_Finish.aspx?status=1&amt=" + result.R3_Amt);
                                #region callback方式:浏览器重定向
                                //context.Response.Write("支付成功!" +
                                //    "<br>接口类型:" + result.R0_Cmd +
                                //    "<br>返回码:" + result.R1_Code +
                                //    //"<br>商户号:" + result.P1_MerId +
                                //    "<br>交易流水号:" + result.R2_TrxId +
                                //    "<br>商户订单号:" + result.R6_Order +

                                //    "<br>交易金额:" + result.R3_Amt +
                                //    "<br>交易币种:" + result.R4_Cur +
                                //    "<br>订单完成时间:" + result.Rp_PayDate +
                                //    "<br>回调方式:" + result.R9_BType +
                                //    "<br>错误信息:" + result.ErrMsg + "<BR>");
                                #endregion
                            }
                            else if (result.R9_BType == "2")
                            {
                                //context.Response.Redirect(mp.SourceSite + "MemberCenter/yeepay_Finish.aspx?status=1&amt=" + result.R3_Amt);
                                #region * 如果是服务器返回则需要回应一个特定字符串'SUCCESS',且在'SUCCESS'之前不可以有任何其他字符输出,保证首先输出的是'SUCCESS'字符串
                                context.Response.Write("SUCCESS");
                                //context.Response.Write("支付成功!" +
                                //     "<br>接口类型:" + result.R0_Cmd +
                                //     "<br>返回码:" + result.R1_Code +
                                //    //"<br>商户号:" + result.P1_MerId +
                                //     "<br>交易流水号:" + result.R2_TrxId +
                                //     "<br>商户订单号:" + result.R6_Order +

                                //     "<br>交易金额:" + result.R3_Amt +
                                //     "<br>交易币种:" + result.R4_Cur +
                                //     "<br>订单完成时间:" + result.Rp_PayDate +
                                //     "<br>回调方式:" + result.R9_BType +
                                //     "<br>错误信息:" + result.ErrMsg + "<BR>");
                                #endregion
                            }
                        }
                        else
                        {
                            context.Response.Redirect(mp.SourceSite + "MemberCenter/pay_Finish.aspx?status=0&amt=" + result.R3_Amt);
                            #region 支付失败
                            //context.Response.Write("支付失败!" +
                            //         "<br>接口类型:" + result.R0_Cmd +
                            //         "<br>返回码:" + result.R1_Code +
                            //    //"<br>商户号:" + result.P1_MerId +
                            //         "<br>交易流水号:" + result.R2_TrxId +
                            //         "<br>商户订单号:" + result.R6_Order +

                            //         "<br>交易金额:" + result.R3_Amt +
                            //         "<br>交易币种:" + result.R4_Cur +
                            //         "<br>订单完成时间:" + result.Rp_PayDate +
                            //         "<br>回调方式:" + result.R9_BType +
                            //         "<br>错误信息:" + result.ErrMsg + "<BR>");
                            #endregion
                        }
                    }
                    else
                    {
                        context.Response.Redirect(mp.SourceSite + "MemberCenter/pay_Finish.aspx?status=0&amt=" + result.R3_Amt);
                        //context.Response.Write("交易签名无效!");
                    }

                }
            }
            catch (Exception ex)
            {
                log.message("yeepay", ex.ToString());
            }
        }

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }
    }
}