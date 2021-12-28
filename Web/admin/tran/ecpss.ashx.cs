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
    /// ecpss callback 的处理方式
    /// </summary>
    public class ecpss : web.tran.Cash.ThirdPaymentRowCommand, IHttpHandler
    {
        string BillNo;          //订单号
        string Amount1;          //交易金额
        string Succeed;         //支付状态:该值说明见于word说明文档[商户根据该值来修改数据库中相应订单的状态]
        string Result;      	//支付结果 (是支付状态的文字说明)
        string SignMD5info;     //验证返回码(调试时使用)
        string md5src;          //对数据进行加密验证
        string md5sign;

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            try { log.message("ecpss", "{0}", context.Request.Form.ToString()); }
            catch { }

            try
            {
                this.BillNo = context.Request.Form["BillNo"];
                this.Amount1 = context.Request.Form["Amount"];
                this.Succeed = context.Request.Form["Succeed"];
                this.Result = context.Request.Form["Result"];
                this.SignMD5info = context.Request.Form["SignMD5info"];

                if (string.IsNullOrEmpty(this.SignMD5info))
                    throw new ArgumentNullException("SignMD5info is null");

                base.ID = new Guid(Convert.FromBase64String(this.BillNo.Substring(this.BillNo.IndexOf("_") + 1)));
                base.op_Finish = this.Succeed == "88";// ? 1 : 0;
                base.op_Delete = true;
                tran.Cash.ThirdPaymentRowData tranRow = base.Execute(sqlcmd, null, null);
                //MemberTranRow tranRow = base.Update(null, null);
                log.message("ecpss", "TranID:{0}\tStatus:{2}\t{3}\r\n{1}", api.SerializeObject(this), api.SerializeObject(tranRow), Succeed, context.Request.Form.ToString());
                //跳转至支付结果页面
                //context.Response.Redirect(mp.SourceSite + "MemberCenter/pay_Finish.aspx?status=1&amt=" + result.R3_Amt);
            }
            catch (Exception ex)
            {
                log.message("ecpss", "{0}", ex);
            }
        }

        protected override tran.Cash.ThirdPaymentRowData GetTranRow(RowErrorCode? err, bool and_LogType)
        {
            tran.Cash.ThirdPaymentRowData row = base.GetTranRow(err, and_LogType);
            EcpssRowData chn = _null<EcpssRowCommand>.value.GetRow(sqlcmd, row.CashChannelID);
            if (chn == null) throw new Exception("Unable found CashChannel");
            this.md5src = this.BillNo + "&" + this.Amount1 + "&" + this.Succeed + "&" + chn.MD5key;
            this.md5sign = FormsAuthentication.HashPasswordForStoringInConfigFile(md5src, "md5").Trim().ToUpper();
            this.SignMD5info = this.SignMD5info.ToUpper();
            if (md5sign == SignMD5info)
                return row;
            else
                throw new Exception("SignMD5info != UpnewSignMD5info");
        }

        //protected override void OnGetRow(SqlCmd sqlcmd, MemberTranRow row)
        //{
        //    bool success = false;
        //    foreach (CashChannelRow _chn in CashChannelRow.Cache.GetInstance(sqlcmd, null).Rows)
        //    {
        //        EcpssRowData chn = _chn as EcpssRowData;
        //        if (chn == null) continue;
        //        this.md5src = this.BillNo + "&" + this.Amount + "&" + this.Succeed + "&" + chn.MD5key;
        //        this.md5sign = FormsAuthentication.HashPasswordForStoringInConfigFile(md5src, "md5").Trim().ToUpper();
        //        this.SignMD5info = this.SignMD5info.ToUpper();
        //        if (success = (md5sign == SignMD5info)) break;
        //    }
        //    if (success == false)
        //        throw new Exception("SignMD5info != UpnewSignMD5info");
        //    base.OnGetRow(sqlcmd, row);
        //}

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }
    }
}