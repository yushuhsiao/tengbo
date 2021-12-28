using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web.SessionState;
using BU;

namespace web
{
    /// <summary>
    /// VerifyImage 的摘要描述
    /// </summary>
    public class VerifyImage : IHttpHandler, IRequiresSessionState
    {
        public const string Type_Login = "login";
        public const string Type_Register = "register";
        public const string Type_Recovery = "recovery";

        public static string GetKey(string type)
        {
            return string.Format("{0}{1}", type, "verify_code");
        }

        public void ProcessRequest(HttpContext context)
        {
            using (Bitmap bitmap = new Bitmap(context.Request.QueryString["width"].ToInt32() ?? 119, context.Request.QueryString["height"].ToInt32() ?? 18))
            using (Graphics g = Graphics.FromImage((Image)bitmap))
            {
                User user = context.User as User;
                g.Clear(Color.Transparent);

                Font f = new System.Drawing.Font("Arial ", 14, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(Color.FromArgb(254, 254, 254)); 
                Brush r = new System.Drawing.SolidBrush(Color.FromArgb(254, 254, 254));

                //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
                //			g.Clear(Color.AliceBlue);//背景色
                g.Clear(System.Drawing.ColorTranslator.FromHtml("#ad1317"));//背景色

                string verify_code;
                context.Session[VerifyImage.GetKey(context.Request["type"])] = verify_code = RandomString.Number.GetRandomString(4);//web.config.VerifyCodeLength
                for (int i = 0; i < verify_code.Length; i++)
                {
                    if (char.IsNumber(verify_code[i]))
                        g.DrawString(verify_code[i].ToString(), f, r, 3 + (i * f.SizeInPoints + 2), 4);
                    else
                        g.DrawString(verify_code[i].ToString(), f, b, 3 + (i * f.SizeInPoints + 2), 4);
                }
                byte[] data;
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    ms.Flush();
                    data = ms.ToArray();
                }
                context.Response.OutputStream.Write(data, 0, data.Length);
                //bitmap.Save(context.Response.OutputStream, ImageFormat.Png);
            }
            context.Response.ContentType = "image/png";
            context.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            context.Response.Expires = -1441;
            //context.Response.CacheControl = "no-cache";
            //context.Response.AddHeader("Pragma", "no-cache");
            //context.Response.AddHeader("Pragma", "no-store");
            //context.Response.AddHeader("cache-control", "no-cache");
            context.Response.Cache.SetExpires(DateTime.MinValue);
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.Cache.SetNoServerCaching();
        }

        public static bool VerifyCurrent(HttpContext context, string type, string code, RowErrorCode? errorCode, string errorMsg)
        {
            string key = context.Session[VerifyImage.GetKey(type)] as string;
            if (string.Compare(code ?? "", key, true) == 0)
                return true;
            if (errorCode.HasValue)
                throw new RowException(errorCode.Value, errorMsg);
            return false;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}