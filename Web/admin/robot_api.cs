using BU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace web
{
    public class robot_api : web.async_base
    {
        protected override bool DeserializeCommand(HttpContext context, ref User user, string json_s, out object command)
        {
            if ((context.User is Guest) && (context.Session == null))
            {
                string json_e1 = context.Request.Form["strenc"] ?? "";
                foreach (AdminAuthRow auth in AdminAuthRow.Cache.Instance.Rows)
                {
                    string idstr = context.Request.Headers[auth.header] as string;
                    if (auth.idstr != idstr) continue;
                    string json_e2 = Crypto.RSADecrypt(json_e1, auth.rsakey);
                    if (json_s == json_e2)
                    {
                        auth.Admin.LoadPermission();
                        context.User = user = auth.Admin;
                        break;
                    }
                    command = null;
                    return false;
                }
            }
            return base.DeserializeCommand(context, ref user, json_s, out command);
        }
    }
}