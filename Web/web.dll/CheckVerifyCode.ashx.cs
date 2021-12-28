using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace web
{
    /// <summary>
    /// CheckVerifyCode 的摘要说明
    /// </summary>
    public class CheckVerifyCode : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Session[context.Request.QueryString["type"].ToString() + "verify_code"].ToString().ToLower() != context.Request.QueryString["value"].ToLower())
                {
                    context.Response.Write("验证码不正确,请重新输入");
                }   
                //if (context.Session[context.Request.QueryString["type"].ToString() + "verify_code"].ToString().ToLower() != context.Request.QueryString["value"].ToLower())
                //{
                //    context.Response.Write("验证码不正确,请重新输入<script type='text/javascript'>$('#img_CheckCode').attr('src', 'VerifyImage.ashx?width=80&height=30&type=tryplay');$('#txt_CheckCode').val('');</script>");
                //}
                //else
                //{
                //    context.Response.Write(@"<script type='text/javascript'>top.$.unblockUI();
                //    $('#playgame" + context.Request.QueryString["gameType"].ToString() + "').click();</script>");
                //}
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}