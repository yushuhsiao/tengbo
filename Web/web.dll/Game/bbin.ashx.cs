using extAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using web;

public class bbin_ashx : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        Member member = context.User as Member;
        if (member != null)
        {
            bbin.page_site? page_site = context.Request.Form["page_site"].ToEnum<bbin.page_site>();
            MemberGame_BBIN.Row row = MemberGame_BBIN.Instance.Login(null, member.ID, member.CorpID, page_site);
            bbin.Request res = row.login_result;
            //http://888.ddt518.com/app/WebService/JSON/display.php
            //http://888.ddt518.com/app/WebService/JSON/display.php/Login?website=LWIN999&username=tbtest001&uppername=dtf88&lang=zh-cn&page_site=live&key=jwpnsxsda3fa0dd26e4c40b3f9d484466cad48d9x
            // <?xml version="1.0"?><request action="Login"><element><website>LWIN999</website><username>tbtest001</username><uppername>dtf88</uppername><lang>zh-cn</lang><page_site>live</page_site><key>jwpnsxsda3fa0dd26e4c40b3f9d484466cad48d9x</key></element></request>
            if (res != null)
            {
                if (res.result.HasValue)
                {
                    if (res.MessageCode.HasValue)
                    {
                        if (res.MessageCode.Value == bbin.StatusCode._44900_IP_is_not_accepted)
                        {
                            StringBuilder s1 = new StringBuilder();
                            foreach (KeyValuePair<string, string> p in res.parameters)
                            {
                                if (s1.Length > 0)
                                    s1.Append("&");
                                s1.AppendFormat("{0}={1}", HttpUtility.UrlEncode(p.Key), HttpUtility.UrlEncode(p.Value));
                            }
                            string url = string.Format("{0}/{1}?{2}", res.RequestUrl, res.Action, s1);
                            context.Response.Redirect(url);
                            return;
                        }
                    }
                }
                else
                {
                    context.Response.Write(res.ResponseText);
                    return;
                }
            }
        }
        context.Response.Redirect("~/");
    }

    public bool IsReusable
    {
        get { return false; }
    }
}