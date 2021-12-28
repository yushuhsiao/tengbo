using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;

namespace extAPI
{
    public abstract class ag
    {
        const string log_prefix = "ag";
        static int log_msgid;
        public enum actype : byte
        {
            real = 1,   // 真錢账号
            trial = 0,  // 试玩账号
        }
        public enum oddtype : byte
        {
            /// <summary>
            /// 20~10000
            /// </summary>
            A = 0,
            /// <summary>
            /// 50~5000
            /// </summary>
            B = 1,
            /// <summary>
            /// 100~10000
            /// </summary>
            C = 2,
            /// <summary>
            /// 200~20000
            /// </summary>
            D = 3,
            /// <summary>
            /// 300~30000
            /// </summary>
            E = 4,
            /// <summary>
            /// 400~40000
            /// </summary>
            F = 5,
            /// <summary>
            /// 500~50000
            /// </summary>
            G = 6,
            /// <summary>
            /// 1000~100000
            /// </summary>
            H = 7,
            /// <summary>
            /// 2000~200000
            /// </summary>
            I = 8,
            /// <summary>
            /// 10000~500000
            /// </summary>
            N = 9,
        }
        public enum trantype
        {
            IN, OUT,
        }

        // 游戏类型
        public enum gameType
        {
            BAC,    // 百家乐
            LINK,   // 连环百家乐
            DT,     // 龙虎
            SHB,    // 骰宝
        }
        // 结算状态
        public enum flag
        {
            _1 = 1,         // 已结算
            _0 = 0,         // 未结算
            _p1 = -1,       // 重置试玩额度
            _p2 = -2,       // 注单被篡改
            _p8 = -8,       // 取消指定局注单
            _p9 = -9,       // 取消注单
        }
        // 游戏玩法
        public enum playType
        {
            // 百家乐
            Banker = 1,           // 庄
            Player = 2,           // 闲
            Tie = 3,              // 和
            BankerPair = 4,       // 庄对
            PlayerPair = 5,       // 闲对
            Big = 6,              // 大
            Small = 7,            // 小
            _8 = 8,               // 散客区庄
            _9 = 9,               // 散客区闲
            _11 = 11,             // 庄(免佣)
            // 龙虎
            _21 = 21,             // 龙
            _22 = 22,             // 虎
            _23 = 23,             // 和(龙虎)
            // 骰宝
            _41 = 41,             // 大 (big)
            _42 = 42,             // 小 (small)
            _43 = 43,             // 单 (single)
            _44 = 44,             // 双 (double)
            _45 = 45,             // 全围 (all wei)
            _46 = 46,             // 围 1 (wei 1)
            _47 = 47,             // 围 2 (wei 2)
            _48 = 48,             // 围 3 (wei 3)
            _49 = 49,             // 围 4 (wei 4)
            _50 = 50,             // 围 5 (wei 5)
            _51 = 51,             // 围 6 (wei 6)
            _52 = 52,             // 单点 1 (single 1)
            _53 = 53,             // 单点 2 (single 2)
            _54 = 54,             // 单点 3 (single 3)
            _55 = 55,             // 单点 4 (single 4)
            _56 = 56,             // 单点 5 (single 5)
            _57 = 57,             // 单点 6 (single 6)
            _58 = 58,             // 对子 1 (double 1)
            _59 = 59,             // 对子 2 (double 2)
            _60 = 60,             // 对子 3 (double 3)
            _61 = 61,             // 对子 4 (double 4)
            _62 = 62,             // 对子 5 (double 5)
            _63 = 63,             // 对子 6 (double 6)
            _64 = 64,             // 组合 12 (combine 12)
            _65 = 65,             // 组合 13 (combine 13)
            _66 = 66,             // 组合 14 (combine 14)
            _67 = 67,             // 组合 15 (combine 15)
            _68 = 68,             // 组合 16 (combine 16)
            _69 = 69,             // 组合 23 (combine 23)
            _70 = 70,             // 组合 24 (combine 24)
            _71 = 71,             // 组合 25 (combine 25)
            _72 = 72,             // 组合 26 (combine 26)
            _73 = 73,             // 组合 34 (combine 34)
            _74 = 74,             // 组合 35 (combine 35)
            _75 = 75,             // 组合 36 (combine 36)
            _76 = 76,             // 组合 45 (combine 45)
            _77 = 77,             // 组合 46 (combine 46)
            _78 = 78,             // 组合 56 (combine 56)
            _79 = 79,             // 和值 4 (sum 4)
            _80 = 80,             // 和值 5 (sum 5)
            _81 = 81,             // 和值 6 (sum 6)
            _82 = 82,             // 和值 7 (sum 7)
            _83 = 83,             // 和值 8 (sum 8)
            _84 = 84,             // 和值 9 (sum 9)
            _85 = 85,             // 和值 10 (sum 10)
            _86 = 86,             // 和值 11 (sum 11)
            _87 = 87,             // 和值 12 (sum 12)
            _88 = 88,             // 和值 13 (sum 13)
            _89 = 89,             // 和值 14 (sum 14)
            _90 = 90,             // 和值 15 (sum 15)
            _91 = 91,             // 和值 16 (sum 16)
            _92 = 92,             // 和值 17 (sum 17)
        }
        // 转账类别
        public enum transferType
        {
            OUT,                    // 转出额度
            IN,                     // 转入额度
            RECALC,                 // 重新派彩
            GBED,                   // 代理修改額度
            RECKON,                 // 派彩
            BET,                    // 下注
            RECALC_ERR,             // 重新派彩时扣款失败
            CHANGE_CUS_BALANCE,     // 修改玩家账户额度
            CHANGE_CUS_CREDIT,      // 修改玩家信用额度
            RESET_CUS_CREDIT,       // 重置玩家信用额度
            DONATEFEE,              // 玩家小费
            CANCEL_BET,             // 系统取消下注
        }

        static Dictionary<bbin.lang, int> map_lang;
        static int? toInt(bbin.lang? value)
        {
            lock (typeof(ag))
            {
                map_lang = map_lang ?? new Dictionary<bbin.lang, int>()
                {
                    { bbin.lang.zh_cn, 1 },
                    { bbin.lang.zh_tw, 2 },
                    { bbin.lang.en_us, 3 },
                    { bbin.lang.euc_jp, 4 },
                    { bbin.lang.ko, 5 },
                    { bbin.lang.th, 6 },
                    { bbin.lang.es, 7 },
                    { bbin.lang.vi, 8 },
                    { bbin.lang.khm, 9 },
                    { bbin.lang.lao, 10 },
                };
            }
            int res;
            if (value.HasValue && map_lang.TryGetValue(value.Value, out res))
                return res;
            return null;
        }
        static Dictionary<bbin.page_site, int> map_gameType;
        static int? toInt(bbin.page_site? value)
        {
            lock (typeof(ag))
            {
                map_gameType = map_gameType ?? new Dictionary<bbin.page_site, int>()
                {
                    { bbin.page_site.live, 1 },
                    { bbin.page_site.ltlottery, 2 },
                    { bbin.page_site.Lottery, 3 },
                    { bbin.page_site.ball, 4 },
                    { bbin.page_site.game, 5 },
                };
            }
            int res;
            if (value.HasValue && map_gameType.TryGetValue(value.Value, out res))
                return res;
            return null;
        }

        private static string desEncryptBase64(string source, string key_des)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(key_des);
            byte[] iv = Encoding.ASCII.GetBytes(key_des);
            byte[] dataByteArray = Encoding.UTF8.GetBytes(source);
            des.Key = key;
            des.IV = iv;
            des.Mode = CipherMode.ECB;
            string encrypt = "";
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                encrypt = Convert.ToBase64String(ms.ToArray());
            }
            return encrypt;
        }

        private static string desDecryptBase64(string encrypt, string key_des)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(key_des);
            byte[] iv = Encoding.ASCII.GetBytes(key_des);
            des.Key = key;
            des.IV = iv;
            des.Mode = CipherMode.ECB;

            byte[] dataByteArray = Convert.FromBase64String(encrypt);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        protected readonly int CorpID;
        [DebuggerStepThrough]
        public ag(int corpID) { this.CorpID = corpID; }

        #region config

        [SqlSetting("ag", "prefix")]
        public string prefix { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }

        [SqlSetting("ag", "gocde")]
        public string gocde
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        [SqlSetting("ag", "pocde")]
        public string pocde
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        [SqlSetting("ag", "token_encrypt_key")]
        public string token_encrypt_key
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        [SqlSetting("ag", "forwardGame_dm")]
        public string forwardGame_dm
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        public abstract string cagent { get; }
        public abstract string des { get; }
        public abstract string md5 { get; }
        public abstract string url { get; }
        public abstract string password { get; }

        [SqlSetting("ag", "ftp_password")]
        public string ftp_password
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        [SqlSetting("ag", "ftp_url")]
        public string ftp_url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        [SqlSetting("ag", "ftp_user")]
        public string ftp_user
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
        }

        #endregion

        public void test()
        {
            string s1 = "qP1RbIK1hOfUIhlkMBeZehxmrY14t2gim13qN0xc/7aTV+OByBibpza18tHrFOh0og5IYhskMqVtwBFPUYmd4d0t1+FxdwZ0eum1KaAvdwedGKfPFn81lgtJNx0grFfec1s/nWgxpN8DNaj6KlcQZ3TJz2CAF3Ye6vXysl4LxMVnurPpeiIsEDMAOPLGEh3eHKR1tgK7FtE=";
            string s2 = desDecryptBase64(s1, this.des);
            Debugger.Break();
        }

        public class Request : Dictionary<string, object>
        {
            public string key_des;
            public string key_md5;

            public string toQueryString(string key_des, string key_md5)
            {
                string param;
                return this.toQueryString(key_des, key_md5, out param);
            }
            public string toQueryString(string key_des, string key_md5, out string param_str)
            {
                StringBuilder s = new StringBuilder();
                foreach (KeyValuePair<string, object> p in this)
                {
                    if (p.Value == null) continue;
                    if (s.Length != 0)
                        s.Append("/\\\\/");
                    s.Append(p.Key);
                    s.Append("=");
                    s.Append(p.Value);
                }
                string param = desEncryptBase64(param_str = s.ToString(), key_des);
                string key = Crypto.MD5Hex(param + key_md5);
                return string.Format("params={0}&key={1}", param, key);
            }

            public Response invokeAPI(string url, string key_des, string key_md5)
            {
                DateTime t1 = DateTime.Now;
                int msgid = Interlocked.Increment(ref log_msgid);
                //string q = this.toQueryString(key_des, key_md5, out param);
                using (WebClient client = new WebClient())
                {
                    client.Headers["User-Agent"] = string.Format("WEB_LIB_GI_{0}", this["cagent"]);
                    string param_str;
                    string api_url = string.Format("http://{0}/doBusiness.do?{1}", url, this.toQueryString(key_des, key_md5, out param_str));
                    string prefix = util.extAPI + "." + log_prefix;
                    try
                    {
                        log.message(prefix, "{0}\tRequest:\t{1}\r\n{2}", msgid, api_url, param_str);
                        string xml = client.DownloadString(api_url);
                        log.message(prefix, "{0}\tResponse ({2}ms):\t{1}", msgid, xml, (int)(DateTime.Now - t1).TotalMilliseconds);
                        return new Response(xml);
                    }
                    catch (Exception ex)
                    {
                        log.message(prefix, "{0}\tError ({2}ms):\t{1}", msgid, ex, (int)(DateTime.Now - t1).TotalMilliseconds);
                        throw;
                    }
                }
            }
        }
        public class Response : Dictionary<string, string>
        {
            public readonly string xml;
            public readonly bool result;
            internal Response(string xml)
            {
                using (StringReader sr = new StringReader(this.xml = xml))
                using (XmlTextReader r = new XmlTextReader(sr))
                {
                    while (r.Read())
                    {
                        if ((r.NodeType == XmlNodeType.Element) && (r.Name.Equals("result", StringComparison.OrdinalIgnoreCase)))
                        {
                            this.result = true;
                            for (bool n = r.MoveToFirstAttribute(); n; n = r.MoveToNextAttribute())
                                this[r.Name] = r.Value;
                        }
                        #region
                        //if (r.NodeType == XmlNodeType.Element)
                        //{
                        //    if (r.Depth == 0)
                        //    {
                        //        result = new hgResponse1();
                        //        result.xml = xml;
                        //        result.Root = r.Name;
                        //        for (bool n = r.MoveToFirstAttribute(); n; n = r.MoveToNextAttribute())
                        //        {
                        //            if (r.Name.Equals("action", StringComparison.OrdinalIgnoreCase))
                        //            {
                        //                result.Action = r.Value;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //    else if ((r.Depth == 1) && r.Name.Equals("error", StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        if (result == null) return null;
                        //        result["error"] = r.ReadElementContentAsString();
                        //    }
                        //    else if ((r.Depth == 1) && r.Name.Equals("element", StringComparison.OrdinalIgnoreCase))
                        //    {
                        //        if (result == null) return null;
                        //    }
                        //    else if ((r.Depth == 2) && r.Name.Equals("properties"))
                        //    {
                        //        if (result == null) return null;
                        //        for (bool n = r.MoveToFirstAttribute(); n; n = r.MoveToNextAttribute())
                        //        {
                        //            if (r.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                        //            {
                        //                if (string.IsNullOrEmpty(r.Value)) continue;
                        //                result[r.Value.ToLower()] = r.ReadString();
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                }
            }

            string getValue(string key)
            {
                string value;
                if (this.TryGetValue(key, out value))
                    return value;
                return null;
            }
            public string info
            {
                get { return this.getValue("info"); }
            }
        }

        public Response CheckOrCreateGameAccout(string loginname, actype actype, string password, oddtype oddtype)
        {
            return new Request()
            {
                {"cagent"     , this.cagent},
                {"loginname"  , loginname},
                {"method"     , "lg"},
                {"actype"     , (int)actype},
                {"password"   , password},
                {"oddtype"    , oddtype},
            }.invokeAPI(this.url, this.des, this.md5);
        }

        public Response GetBalance(string loginname, actype actype, string password)
        {
            return new Request()
            {
                {"cagent"     , this.cagent},
                {"loginname"  , loginname},
                {"method"     , "gb"},
                {"actype"     , (int)actype},
                {"password"   , password},
            }.invokeAPI(this.url, this.des, this.md5);
        }

        public Response PrepareTransferCredit(string loginname, string billno, trantype type, decimal credit, actype actype, string password)
        {
            return new Request()
            {
                {"cagent"     , this.cagent},
                {"loginname"  , loginname},
                {"method"     , "tc"},
                {"billno"     , this.cagent + billno},
                {"type"       , type},
                {"credit"     , credit},
                {"actype"     , (int)actype},
                {"password"   , password},
            }.invokeAPI(this.url, this.des, this.md5);
        }

        public Response TransferCreditConfirm(string loginname, string billno, trantype type, decimal credit, actype actype, bool successed, string password)
        {
            return new Request()
            {
                {"cagent"     , this.cagent},
                {"loginname"  , loginname},
                {"method"     , "tcc"},
                {"billno"     , this.cagent + billno},
                {"type"       , type},
                {"credit"     , credit},
                {"actype"     , (int)actype},
                {"flag"       , successed ? 1 : 0},
                {"password"   , password},
            }.invokeAPI(this.url, this.des, this.md5);
        }

        public string forwardGame(string loginname, string password, string dm, string sid, actype actype, bbin.lang? lang, bbin.page_site? gameType)
        {
            // "http://gi.eggstown.com:81/forwardGame.do?params=qP1RbIK1hOdUKH8jqqQDvsN6JbPj6QU1qEIJRpugJZx7MfkCie1Ud1rbEosgq7X81i0/on8Tv5G2bajwqgiwG5ObSyJDFBK52UlD0x7sCqsyYa4H2SwGkIW6hd2TFi76wfEgkpyOnwYF/ezAmW5ZmRZV4nDzsdjLH6SpwlaGX6o=&key=82f6b7b4bd00bc89caa24542716f2dc3";
            //string s = "qP1RbIK1hOdUKH8jqqQDvsN6JbPj6QU1qEIJRpugJZx7MfkCie1Ud1rbEosgq7X81i0/on8Tv5G2bajwqgiwG5ObSyJDFBK52UlD0x7sCqsyYa4H2SwGkIW6hd2TFi76wfEgkpyOnwYF/ezAmW5ZmRZV4nDzsdjLH6SpwlaGX6o=";
            //string s2 = desDecryptBase64(s, this.des);
            // qP1RbIK1hOdUKH8jqqQDvsN6JbPj6QU1qEIJRpugJZx7MfkCie1Ud1rbEosgq7X81i0/on8Tv5G2bajwqgiwG5ObSyJDFBK52UlD0x7sCqsyYa4H2SwGkIW6hd2TFi76wfEgkpyOnwYF/ezAmW5ZmRZV4nDzsdjLH6SpwlaGX6o
            // 82f6b7b4bd00bc89caa24542716f2dc3
            // 
            string queryString = new Request()
            {
                {"cagent"     , this.cagent},
                {"loginname"  , loginname},
                {"password"   , password},
                {"dm"         , dm ?? this.forwardGame_dm},
                {"sid"        , sid},
                {"billno"     , this.cagent + sid},
                {"actype"     , (int)actype},
                {"lang"       , toInt(lang)},
                {"gameType"   , toInt(gameType)},
            }.toQueryString(this.des, this.md5);
            return string.Format("http://{0}/forwardGame.do?{1}", this.url, queryString);
        }

        //public void test()
        //{
        //    //CheckOrCreateGameAccout("player", actype.real, "李四", oddtype.A);
        //    GetBalance("player", actype.real, "李四");
        //    Debugger.Break();
        //}

        public abstract class api<T> : ag where T : api<T>
        {
            public api(int corpID) : base(corpID) { }
            static readonly api_instances<T> instances = new api_instances<T>();
            public static T GetInstance(int? corpID) { return instances[corpID]; }
        }

        public class AG : api<AG>
        {
            public AG(int corpID) : base(corpID) { }
            [SqlSetting("ag_AG", "cagent")]
            public override string cagent
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_AG", "des")]
            public override string des
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_AG", "md5")]
            public override string md5
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_AG", "url")]
            public override string url
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_AG", "password")]
            public override string password
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
        }
        public class AGIN : api<AGIN>
        {
            public AGIN(int corpID) : base(corpID) { }
            [SqlSetting("ag_AGIN", "cagent")]
            public override string cagent
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_AGIN", "des")]
            public override string des
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_AGIN", "md5")]
            public override string md5
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_AGIN", "url")]
            public override string url
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_AGIN", "password")]
            public override string password
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
        }
        public class DSP : api<DSP>
        {
            public DSP(int corpID) : base(corpID) { }
            [SqlSetting("ag_DSP", "cagent")]
            public override string cagent
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_DSP", "des")]
            public override string des
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_DSP", "md5")]
            public override string md5
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_DSP", "url")]
            public override string url
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
            [SqlSetting("ag_DSP", "password")]
            public override string password
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); }
            }
        }
    }
}