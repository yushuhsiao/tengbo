using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
//using extAPI.ea;

namespace extAPI
{

    public class CommonMethod : IDisposable
    {
        static string BaseUrl = "http://webapi-asia.hointeractive.com/betapi.asmx/{0}";//投注API
        MemoryStream ms;
        XmlTextWriter xw;

        public CommonMethod(string command)
        {
            this.ms = new MemoryStream();
            this.xw = new XmlTextWriter(ms, Encoding.UTF8);
            this.xw.WriteStartDocument();
            this.xw.WriteStartElement(command);
        }

        public void Param(string name, string value)
        {
            try
            {
                this.xw.WriteStartElement(name);
                this.xw.WriteString(value);
            }
            finally
            {
                this.xw.WriteEndElement();
            }
        }

        public void Param(string name, object value, string format)
        {
            try
            {
                this.xw.WriteStartElement(name);
                if (format == null)
                    format = "{0}";
                else
                    format = string.Format("{{0:{0}}}", format);
                this.xw.WriteString(string.Format(format, value));
            }
            finally
            {
                this.xw.WriteEndElement();
            }
        }

        public void optParam(string name, string value)
        {
            if (value == null) return;
            this.Param(name, value);
        }

        public void optParam(string name, object value, string format)
        {
            if (value == null) return;
            this.Param(name, value, format);
        }

        public string invoke(string url)
        {
            string request;
            return this.invoke(url, out request);
        }
        public string invoke(string url, out string request)
        {
            string returnResult = "";
            using (this.ms)
            {
                using (this.xw)
                    this.xw.Flush();
                this.ms.Flush();
                request = Encoding.UTF8.GetString(ms.ToArray());
                request = System.Text.RegularExpressions.Regex.Replace(request, "^[^<]", "");
            }
            // 檢查 request 的內容
            //Debugger.Break();
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format(BaseUrl, url));
            httpRequest.Method = "Post";
            httpRequest.ContentType = "text/xml";
            //httpRequest.ContentLength = request.Length;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = CredentialCache.DefaultCredentials;
            using (StreamWriter RequestStream = new StreamWriter(httpRequest.GetRequestStream()))
                RequestStream.Write(request);
            HttpWebResponse HttpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (StreamReader Readstream = new StreamReader(HttpResponse.GetResponseStream(), Encoding.UTF8))
                if (HttpResponse.StatusCode == HttpStatusCode.OK)
                {
                    returnResult = Readstream.ReadToEnd();
                    returnResult = returnResult.Replace("&lt;", "<");
                    returnResult = returnResult.Replace("&gt;", ">");
                    //returnResult = returnResult.Replace("\r\n", "");
                    return returnResult;
                }
                else
                {
                    return string.Format("error!{0}", (int)HttpResponse.StatusCode);
                }
        }
        public static string invokeMember(string url, string requestXML)
        {
            string resultStr = "";
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求   
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "text/xml";

            //if (!string.IsNullOrEmpty(userAgent))
            //{
            //    request.UserAgent = userAgent;
            //}
            //else
            //{
            //    request.UserAgent = DefaultUserAgent;
            //}


            //if (cookies != null)
            //{
            //    request.CookieContainer = new CookieContainer();
            //    request.CookieContainer.Add(cookies);
            //}
            //如果需要POST数据   
            if (requestXML != "")
            {
                byte[] data = Encoding.UTF8.GetBytes(requestXML);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);

                }
                HttpWebResponse HttpResponse = (HttpWebResponse)request.GetResponse();
                using (StreamReader Readstream = new StreamReader(HttpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    if (HttpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        resultStr = Readstream.ReadToEnd();
                    }
                    else
                    {
                        resultStr = string.Format("error!{0}", HttpResponse.StatusCode);
                    }
                }
                HttpResponse.Close();
            }
            return resultStr;
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }

        //public static string RequestPostData(string url, string postData)
        //{
        //    string resultStr;
        //    //HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        //    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        //    request.Method = "POST";
        //    request.KeepAlive = false;
        //    request.ContentType = "text/xml";
        //    //request.CookieContainer = cookieContainer;
        //    //request.ContentLength = postData.Length;


        //    //byte[] data = Encoding.UTF8.GetBytes(postData);
        //    //using (Stream stream = request.GetRequestStream())
        //    //{
        //    //    stream.Write(data, 0, data.Length);

        //    //}
        //    HttpWebResponse HttpResponse = (HttpWebResponse)request.GetResponse();
        //    using (StreamReader Readstream = new StreamReader(HttpResponse.GetResponseStream(), Encoding.UTF8))
        //    {
        //        if (HttpResponse.StatusCode == HttpStatusCode.OK)
        //        {
        //            resultStr = Readstream.ReadToEnd();
        //        }
        //        else
        //        {
        //            resultStr = string.Format("error!{0}", HttpResponse.StatusCode);
        //        }
        //    }
        //    HttpResponse.Close();
        //    //resultStr = System.Web.HttpUtility.UrlDecode(resultStr);
        //    return resultStr;
        //}
        public static string MyData(string url)
        {
            string resultStr = "";
            Uri ourUri = new Uri(url);
            WebRequest myWebRequest = WebRequest.Create(ourUri);

            WebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();

            using (StreamReader stream = new StreamReader(myWebResponse.GetResponseStream(), Encoding.Default))
            {
                resultStr = stream.ReadToEnd();
            }
            return resultStr;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            using (this.xw)
            using (this.ms)
            {
            }
        }

        #endregion
    }

    public class XMLPropertiesHepler
    {

        public static string getPropertiesValues(XmlNodeList list, string attrName)
        {
            string reslut = "";
            foreach (XmlNode node in list)
            {
                if (node.Attributes["name"].Value == attrName)
                {
                    reslut = node.InnerText.Trim();
                    break;
                }
            }
            return reslut;
        }

        public static string getNodeValues(XmlNodeList list, string nodeName)
        {
            string reslut = "";
            foreach (XmlNode node in list)
            {
                if (node.HasChildNodes && node.ChildNodes.Count > 1)
                {
                    foreach (XmlNode subNode in node.ChildNodes)
                    {
                        if (subNode.Name == nodeName)
                        {
                            reslut = subNode.InnerText.Trim();
                            return reslut;
                        }
                    }
                }
                else if (node.Name == nodeName)
                {
                    reslut = node.InnerText.Trim();
                    break;
                }
            }
            return reslut;
        }

        public static string getKGDetailNodeValues(XmlNodeList list, string nameString)
        {
            string reslut = "";

            if (list[0].InnerText == nameString)
            {
                reslut = list[1].InnerText.Trim();
                return reslut;
            }
            return reslut;
        }
    }

}