using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;

namespace System.IO
{
    public class EncodingStringWriter : StringWriter
    {
        public EncodingStringWriter() { }
        public EncodingStringWriter(StringBuilder sb) : base(sb) { }
        public EncodingStringWriter(Encoding encoding, StringBuilder sb) : base(sb) { this.encoding = encoding; }

        Encoding encoding;
        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}

namespace extAPI
{
    abstract class apiRequest : IDisposable
    {
        protected StringBuilder sb;
        StringWriter sw;
        protected XmlTextWriter xml;

        public virtual string log_prefix
        {
            get { return null; }
        }
        string prefix
        {
            get
            {
                string prefix = log_prefix;
                if (string.IsNullOrEmpty(prefix))
                    return "extAPI";
                else
                    return "extAPI." + prefix;
            }
        }

        public apiRequest()
        {
            sb = new StringBuilder();
            sw = new EncodingStringWriter(sb);
            xml = new XmlTextWriter(sw);
        }

        void close()
        {
            using (sw)
            using (xml)
            {
            }
        }

        public string GetString()
        {
            this.close();
            return sb.ToString().Replace("&lt;", "<").Replace("&gt;", ">"); ;
        }

        void IDisposable.Dispose()
        {
            this.close();
        }

        public virtual string GetResponse(string url)
        {
            string xml = this.GetString();
            try
            {
                HttpWebRequest request = null;
                //如果是发送HTTPS请求   
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    // Always Accept  Validation Callback
                    ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => { return true; };
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.PreAuthenticate = true;
                request.Credentials = CredentialCache.DefaultCredentials;
                using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                    sw.Write(xml);
                log.message(prefix, "Request:\t{0}", xml);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new HttpException((int)response.StatusCode, response.StatusDescription);
                    string ret;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        ret = sr.ReadToEnd();
                    log.message(prefix, "Response:\t{0}", ret);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                log.message(prefix, "Error:\t{0}", ex);
                throw ex;
            }
        }
    }

    static class util
    {
        static util()
        {
            // Always Accept  Validation Callback
            ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => { return true; };
        }

        public static int? ToInt32(this bool? value, int? trueValue, int? falseValue)
        {
            if (value.HasValue)
                return value.Value ? trueValue : falseValue;
            return null;
        }

        public static object EnumToValue(object value)
        {
            Type t = value.GetType();
            if (t.IsEnum)
                return Convert.ChangeType(value, Enum.GetUnderlyingType(t));
            return value;
        }

        /// <summary>
        /// UTF-8 without BOM
        /// </summary>
        public static UTF8Encoding UTF8Encoding = new UTF8Encoding(false);

        public const string extAPI = "extAPI";
        static int msgid;

        public static string GetResponse(string url, string xml, string log_prefix)
        {
            return GetResponse(url, xml, log_prefix, null, null);
        }
        public static string GetResponse(string url, string xml, string log_prefix, int? log_request, int? log_response)
        {
            DateTime t1 = DateTime.Now;
            string prefix;
            if (string.IsNullOrEmpty(log_prefix))
                prefix = extAPI;
            else
                prefix = extAPI + "." + log_prefix;
            try
            {
                HttpWebRequest httpRequest = null;
                httpRequest = WebRequest.Create(url) as HttpWebRequest;
                if (httpRequest.RequestUri.Scheme.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    httpRequest.ProtocolVersion = HttpVersion.Version10;
                httpRequest.Method = "POST";
                httpRequest.ContentType = "text/xml";
                httpRequest.PreAuthenticate = true;
                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                using (StreamWriter sw = new StreamWriter(httpRequest.GetRequestStream()))
                    sw.Write(xml);
                if (xml.Length < (log_request ?? int.MaxValue))
                    log.message(prefix, "Request:\t{0}", xml);
                else
                    log.message(prefix, "Request:\t{0}", xml);
                using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new HttpException((int)response.StatusCode, response.StatusDescription);
                    string ret;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        ret = sr.ReadToEnd();
                    if (ret.Length < (log_response ?? int.MaxValue))
                        log.message(prefix, "Response ({1}ms):\t{0}", ret, (int)(DateTime.Now - t1).TotalMilliseconds);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                log.message(prefix, "Error ({1}ms):\t{0}", ex, (int)(DateTime.Now - t1).TotalMilliseconds);
                throw ex;
            }
        }

        /// <summary>
        /// PT 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="xml"></param>
        /// <param name="log_prefix"></param>
        /// <param name="log_request"></param>
        /// <param name="log_response"></param>
        /// <returns></returns>
        public static string GetResponse(string url, string EntityKEY, string keyFile, string pp_pw, string log_prefix, int? log_request, int? log_response)
        {
            DateTime t1 = DateTime.Now;
            string prefix;
            if (string.IsNullOrEmpty(log_prefix))
                prefix = extAPI;
            else
                prefix = extAPI + "." + log_prefix;
            try
            {
                HttpWebRequest httpRequest = null;
                httpRequest = WebRequest.Create(url) as HttpWebRequest;
                httpRequest.Headers.Add("X_ENTITY_KEY", EntityKEY);
                httpRequest.Method = "GET";
                X509Certificate2 x509 = new X509Certificate2(System.IO.File.ReadAllBytes(keyFile), pp_pw, 
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                httpRequest.ClientCertificates.Add(x509);
                log.message(prefix, "Request:\t{0}", url);
                using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new HttpException((int)response.StatusCode, response.StatusDescription);
                    string ret;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        ret = sr.ReadToEnd();
                    if (ret.Length < (log_response ?? int.MaxValue))
                        log.message(prefix, "Response ({1}ms):\t{0}", ret, (int)(DateTime.Now - t1).TotalMilliseconds);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                log.message(prefix, "Error ({1}ms):\t{0}", ex, (int)(DateTime.Now - t1).TotalMilliseconds);
                throw ex;
            }
        }
    }

    class SqlSchemaTable : Dictionary<string, SqlSchemaTable.Item>
    {
        public class Item
        {
            public string Name;
            public Type Type;
        }

        static Dictionary<string, SqlSchemaTable> cache = new Dictionary<string, SqlSchemaTable>();

        public static SqlSchemaTable GetSchema(SqlCmd sqlcmd, string sqlstr)
        {
            lock (cache)
            {
                SqlSchemaTable result;
                if (!cache.TryGetValue(sqlstr, out result))
                {
                    cache[sqlstr] = result = new SqlSchemaTable();
                    using (SqlDataReader r = sqlcmd.ExecuteReader(sqlstr))
                    {
                        for (int i = 0; i < r.FieldCount; i++)
                        {
                            string name = r.GetName(i);
                            result[name.ToLower()] = new Item() { Name = name, Type = r.GetFieldType(i) };
                        }
                    }
                }
                return result;
            }
        }
    }
}