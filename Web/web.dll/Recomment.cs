using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace web
{
    public class Recomment : IHttpHandler
    {
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            try
            {
                string p = context.Request.AppRelativeCurrentExecutionFilePath;
                int n2 = p.IndexOf(".reg");
                int n1 = p.LastIndexOf("/", n2);
                string s = p.Substring(n1 + 1, n2 - n1 - 1);
                string url = string.Format("~/Register.aspx?reg={0}", s);
                context.Response.Redirect(url);
            }
            catch
            {
                context.Response.Redirect("~/Register.aspx");
            }
        }
    }
}