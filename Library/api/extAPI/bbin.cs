using BU;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using web;
using Tools.Protocol;


namespace extAPI
{
    public class bbin
    {
        static readonly api_instances<bbin> instances = new api_instances<bbin>();
        public static bbin GetInstance(int? corpID) { return instances[corpID]; }
        readonly int CorpID;
        [DebuggerStepThrough]
        public bbin(int corpID) { this.CorpID = corpID; }

        public enum StatusCode : int
        {
            _44900_IP_is_not_accepted = 44900,
            Add_account_successful = 21100,
            //{"result":false,"data":{"Code":"44900","Message":"IP is not accepted."}}
            //{"result":true,"data":{"Code":"21100","Message":"Add account successful."}}
            //{"result":false,"data":{"Code":"21001","Message":"The account is repeated."}}
            //{"result":false,"data":{"Code":"22002","Message":"The user is not exist."}}
            //{"result":false,"data":{"Code":"44000","Message":"Key error"}}
            //{"result":false,"data":{"Code":"44900","Message":"IP is not accepted."}}
            //{"result":false,"data":{"Code":"44001","Message":"The parameters are not complete."}}
            //{"result":true,"data":{"Code":"22001","Message":"User Logout"}}
            //{"result":false,"data":{"Code":"22000","Message":"User hasn't Login."}}
            //{"result":false,"data":{"Code":"44444","Message":"System is in maintenance"}}
            //{"result":true,"data":[{"LoginName":"tbtest001","Currency":"RMB","Balance":0,"TotalBalance":0}],"pagination":{"Page":1,"PageLimit":500,"TotalNumber":1,"TotalPage":1}}
        }

        public static string log_prefix = "bbin";

        [SqlSetting("bbin", "prefix")]
        public string prefix { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "api_url")]
        public string api_url { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "website")]
        public string website { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "uppername")]
        public string uppername { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "KeyB_CheckUsrBalance")]
        public string KeyB_CheckUsrBalance { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "KeyB_CreateMember")]
        public string KeyB_CreateMember { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "KeyB_Login")]
        public string KeyB_Login { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "KeyB_Logout")]
        public string KeyB_Logout { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "KeyB_Transfer")]
        public string KeyB_Transfer { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }
        [SqlSetting("bbin", "KeyB_getbet")]
        public string KeyB_getbet { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.CorpID); } }

        public enum page_site
        {
            /// <summary>
            /// 球類
            /// </summary>
            ball,

            /// <summary>
            /// 彩票
            /// </summary>
            ltlottery,

            /// <summary>
            /// bb彩票
            /// </summary>
            Lottery,

            /// <summary>
            /// 視訊
            /// </summary>
            live,

            /// <summary>
            /// 機率
            /// </summary>
            game,
        }

        public enum lang { zh_cn, zh_tw, en_us, euc_jp, ko, th, es, vi, khm, lao }

        public enum tran_action { IN, OUT }

        //球類  3D廳 視訊 機率 BB彩票 彩票
        [JsonProtocol.UnderlyingValueInDictionaryKey]
        public enum gamekind
        {
            //球類 = ball,
            //3D廳 = ThreeD,
            //視訊 = live,
            //機率 = game,
            //BB彩票 = Lottery,
            //彩票 = ltlottery,
            /// <summary>
            /// 球類
            /// </summary>
            ball = 1,
            /// <summary>
            /// 彩票
            /// </summary>
            ltlottery = 12,
            /// <summary>
            /// BB彩票
            /// </summary>
            Lottery = 2,
            /// <summary>
            /// 視訊
            /// </summary>
            live = 3,
            /// <summary>
            /// 機率
            /// </summary>
            game = 5,
            /// <summary>
            /// 3D廳
            /// </summary>
            ThreeD = 15,
        }

        
        public enum gametype : int
        {
            // BB體育遊戲
            BK = 0x424B0000,//籃球
            BS = 0x42530000,//棒球
            F1 = 0x46310000,//其他
            FB = 0x46420000,//美足
            FT = 0x46540000,//足球
            FU = 0x46550000,//指數
            IH = 0x49480000,//冰球
            SP = 0x53500000,//冠軍
            TN = 0x544E0000,//網球

            //彩票遊戲
            LT = 0x4C540000,//六合彩
            D3 = 0x44330000,//3D彩
            P3 = 0x50330000,//排列三
            BT = 0x42540000,//BB3D時時彩
            T3 = 0x54330000,//重慶時時彩
            CQ = 0x43510000,//上海時時彩
            JX = 0x4A580000,//江西時時彩
            TJ = 0x544A0000,//天津時時彩
            GXSF = 0x47585346,//廣西十分彩
            GDSF = 0x47445346,//廣東十分彩
            TJSF = 0x544A5346,//天津十分彩
            BJKN = 0x424A4B4E,//北京快樂 8
            CAKN = 0x43414B4E,//加拿大卑斯
            AUKN = 0x41554B4E,//澳洲首都商業區
            BBKN = 0x42424B4E,//BB快樂彩
            BJPK = 0x424A504B,//北京PK拾


            //3D厅
            _15006 = 15006, _3D玉蒲團 = 15006,
            _15016 = 15016, 廚王争霸 = 15016,
            _15017 = 15017, 連環奪寶 = 15017,
            _15018 = 15018, 激情243 = 15018,
            _15019 = 15019, 倩女幽魂 = 15019,
            _15020 = 15020, 欲望射手 = 15020,
            _15021 = 15021, 全民狗仔 = 15021,

            ////BB彩票遊戲
            //FU3D,//福彩3D
            //SP3D,//體彩排列3
            //TI3D,//時時樂3D
            //BJK8,//北京快樂8
            //BCK8,//加拿大卑斯
            //HKMS,//香港六合彩
            //CQ3D,//重慶時時彩
            //HL3D,//黑龍江時時彩
            //ACK8,//澳洲首都商業區
            //BB3D,//BB3D時時彩
            //BBK8,//BB快樂彩

            //視訊遊戲
            _3001 = 3001, 百家樂 = 3001,
            _3002 = 3002, 二八槓 = 3002,
            _3003 = 3003, 龍虎鬥 = 3003,
            _3005 = 3005, 三公 = 3005,
            _3006 = 3006, 溫州牌九 = 3006,
            _3007 = 3007, 輪盤_ = 3007,
            _3008 = 3008, 輪盤 = 3008,
            _3010 = 3010, 德州撲克 = 3010,
            _3011 = 3011, 色碟 = 3011,
            _3012 = 3012, 牛牛 = 3012,

            //機率遊戲
            _5001 = 5001, 水果拉霸 = 5001,
            _5002 = 5002, 撲克拉霸 = 5002,
            _5003 = 5003, 筒子拉霸 = 5003,
            _5004 = 5004, 足球拉霸 = 5004,
            _5011 = 5011, 西遊記 = 5011,
            _5012 = 5012, 外星爭霸 = 5012,
            _5013 = 5013, 傳統 = 5013,
            _5014 = 5014, 叢林 = 5014,
            _5015 = 5015, FIFA2010 = 5015,
            _5016 = 5016, 史前叢林冒險 = 5016,
            _5017 = 5017, 星際大戰 = 5017,
            _5018 = 5018, 齊天大聖 = 5018,
            _5019 = 5019, 水果樂園 = 5019,
            _5020 = 5020, 熱帶風情 = 5020,
            _5021 = 5021, _7PK = 5021,
            _5023 = 5023, 七靶射擊 = 5023,
            _5024 = 5024, _2012歐錦賽 = 5024,
            _5025 = 5025, 法海鬥白蛇 = 5025,
            _5026 = 5026, _2012倫敦奧運 = 5026,
            _5027 = 5027, 功夫龍 = 5027,
            _5028 = 5028, 中秋月光派對 = 5028,
            _5029 = 5029, 聖誕派對 = 5029,
            _5030 = 5030, 幸運財神 = 5030,
            _5034 = 5034, 王牌5PK = 5034,
            _5035 = 5035, 加勒比撲克 = 5035,
            _5047 = 5047, 屍樂園 = 5047,
            _5048 = 5048, 特務危機 = 5048,
            _5049 = 5049, 玉蒲團 = 5049,
            _5050 = 5050, 戰火佳人 = 5050,
            _5057 = 5057, 明星97 = 5057,
            _5058 = 5058, 瘋狂水果盤 = 5058,
            _5059 = 5059, 馬戲團 = 5059,
            _5060 = 5060, 動物奇觀五代 = 5060,
            _5061 = 5061, 超級7 = 5061,
            _5062 = 5062, 龍在囧途 = 5062,
            _5074 = 5074, 鑽石列車 = 5074,
            _5075 = 5075, 聖獸傳說 = 5075,
            _5076 = 5076, 數字大轉輪 = 5076,
            _5077 = 5077, 水果大轉輪 = 5077,
            _5078 = 5078, 象棋大轉輪 = 5078,
            _5079 = 5079, _3D數字大轉輪 = 5079,
            _5080 = 5080, 樂透轉輪 = 5080,
            _5081 = 5081, 鬥大 = 5081,
            _5082 = 5082, 紅狗 = 5082,
            _5091 = 5091, 三國拉霸 = 5091,
            _5092 = 5092, 封神榜 = 5092,
            _5093 = 5093, 金瓶梅 = 5093,
            _5094 = 5094, 金瓶梅2 = 5094,
            _5101 = 5101, 歐式輪盤 = 5101,
            _5102 = 5102, 美式輪盤 = 5102,
            _5103 = 5103, 彩金輪盤 = 5103,
            _5104 = 5104, 法式輪盤 = 5104,
            _5115 = 5115, 經典21點 = 5115,
            _5116 = 5116, 西班牙21點 = 5116,
            _5117 = 5117, 維加斯21點 = 5117,
            _5118 = 5118, 獎金21點 = 5118,
            _5131 = 5131, 皇家德州撲克 = 5131,
            _5201 = 5201, 火燄山 = 5201,
            _5202 = 5202, 月光寶盒 = 5202,
            _5203 = 5203, 愛你一萬年 = 5203,
            _5401 = 5401, 天山俠侶傳 = 5401,
            _5402 = 5402, 夜市人生 = 5402,
            _5403 = 5403, 七劍傳說 = 5403,
            _5801 = 5801, 海豚世界 = 5801,
            _5802 = 5802, 阿基里斯 = 5802,
            _5803 = 5803, 阿兹特克寶藏 = 5803,
            _5804 = 5804, 大明星 = 5804,
            _5805 = 5805, 凱薩帝國 = 5805,
            _5806 = 5806, 奇幻花園 = 5806,
            _5807 = 5807, 東方魅力 = 5807,
            _5808 = 5808, 浪人武士 = 5808,
            _5809 = 5809, 空戰英豪 = 5809,
            _5810 = 5810, 航海時代 = 5810,
            _5811 = 5811, 狂歡夜 = 5811,
            _5821 = 5821, 國際足球 = 5821,
            _5822 = 5822, 兔女郎 = 5822,
            _5823 = 5823, 發大財 = 5823,
            _5824 = 5824, 惡龍傳說 = 5824,
            _5825 = 5825, 金蓮 = 5825,
            _5826 = 5826, 金礦工 = 5826,
            _5827 = 5827, 老船長 = 5827,
            _5828 = 5828, 霸王龍 = 5828,
            _5831 = 5831, 高球之旅 = 5831,
            _5832 = 5832, 高速卡車 = 5832,
            _5833 = 5833, 沉默武士 = 5833,
            _5834 = 5834, 異國之夜 = 5834,
            _5835 = 5835, 喜福牛年 = 5835,
            _5836 = 5836, 龍捲風 = 5836,
            _5888 = 5888, JackPot = 5888,

        }

        public enum gametype_彩票 : int
        {
            LT = gametype.LT,    //六合彩
            D3 = gametype.D3,    //3D彩
            P3 = gametype.P3,    //排列三
            BT = gametype.BT,    //BB3D時時彩
            T3 = gametype.T3,    //重慶時時彩
            CQ = gametype.CQ,    //上海時時彩
            JX = gametype.JX,    //江西時時彩
            TJ = gametype.TJ,    //天津時時彩
            GXSF = gametype.GXSF,//廣西十分彩
            GDSF = gametype.GDSF,//廣東十分彩
            TJSF = gametype.TJSF,//天津十分彩
            BJKN = gametype.BJKN,//北京快樂 8
            CAKN = gametype.CAKN,//加拿大卑斯
            AUKN = gametype.AUKN,//澳洲首都商業區
            BBKN = gametype.BBKN,//BB快樂彩
            BJPK = gametype.BJPK,//北京PK拾
        }

        /// <param name="website">*網站名稱</param>
        /// <param name="username">*會員帳號(英文字母以及數字的組合)</param>
        /// <param name="uppername">*上層帳號</param>
        /// <param name="password">密碼(6~12碼英文或數字且符合0~9及a~z字元)</param>
        public Request CreateMember(string username, string uppername, string password)
        {
            Request request = new Request("CreateMember");
            request["website"] = this.website;
            request["username"] = username;
            request["uppername"] = uppername;
            request["password"] = password;
            //string b = string.Format("{0}{1}{2}{3:yyyyMMdd}", website, username, this.KeyB_CreateMember, DateTime.Now.ToACTime());
            request.setkey(this.website, 5, 2, username, null, this.KeyB_CreateMember, null);
            request.GetResponse(this.api_url);
            return request;
        }

        /// <param name="website">*網站名稱</param>
        /// <param name="username">*會員帳號</param>
        /// <param name="uppername">*上層帳號</param>
        /// <param name="password">密碼</param>
        /// <param name="lang">語系: zh-cn, zh-tw, en-us, euc-jp, ko, th, es, vi, khm, lao</param>
        /// <param name="page_site">球類:ball, 彩票:ltlottery, bb彩票:Lottery, 視訊:live, 機率:game</param>
        public Request Login(string username, string uppername, string password, lang? lang, page_site? page_site)
        {
            Request request = new Request("Login");
            request["website"] = this.website;
            request["username"] = username;
            request["uppername"] = uppername ?? this.uppername;
            request["password"] = password;
            if (lang.HasValue)
                request["lang"] = lang.ToString().Replace('_', '-');
            request["page_site"] = page_site.ToString();
            //string b = string.Format("{0}{1}{2}{3:yyyyMMdd}", website, username, this.KeyB_Login, DateTime.Now.ToACTime());
            request.setkey(this.website, 8, 1, username, null, this.KeyB_Login, null);
            request.GetResponse(this.api_url);
            //<html>
            //    <head><title></title><meta http-equiv="Content-Type" content="text/html; charset=UTF-8"></head>
            //    <body onload='document.post_form.submit();'>
            //        <form id='post_form' name='post_form' method="post" action='http://777.ddt518.com'>
            //            <input name="uid" type='hidden' value='ib73dd8az6g48a35z120ln23z3cknrcz142'><input name="langx" type='hidden' value='zh-cn'>
            //        </form>
            //    </body>
            //</html>
            //if (request.response.Contains(
            return request;
        }

        /// <param name="website">*網站名稱</param>
        /// <param name="username">*會員帳號</param>
        public Request Logout(string username)
        {
            Request request = new Request("Logout");
            request["website"] = this.website;
            request["username"] = username;
            //string b = string.Format("{0}{1}{2}{3:yyyyMMdd}", website, username, this.KeyB_Logout, DateTime.Now.ToACTime());
            request.setkey(this.website, 4, 6, username, null, this.KeyB_Logout, null);
            request.GetResponse(this.api_url);
            return request;
        }

        /// <param name="website">*網站名稱</param>
        /// <param name="username">會員帳號(可以使用sql的%語句)</param>
        /// <param name="uppername">*上層帳號</param>
        public Request CheckUsrBalance(string username, string uppername)
        {
            Request request = new Request("CheckUsrBalance");
            request["website"] = this.website;
            request["username"] = username;
            request["uppername"] = uppername;
            request.setkey(this.website, 9, 6, username, null, this.KeyB_CheckUsrBalance, null);
            request.GetResponse(this.api_url);
            return request;
        }

        //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        //class Response
        //{
        //    [JsonProperty]
        //    public bool result;
        //}
        //class Response<T1, T> : Response
        //    where T1 : Response<T1, T>
        //    where T : Response
        //{
        //    [JsonProperty]
        //    public T data;
        //}
        //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        //class CheckUsrBalanceResponse : Response<CheckUsrBalanceResponse, CheckUsrBalanceResponse>
        //{
        //}

        /// <param name="website">*網站名稱</param>
        /// <param name="username">*會員帳號</param>
        /// <param name="uppername">*上層帳號</param>
        /// <param name="remitno_str">*存提序號 (long)</param>
        /// <param name="action">*IN(存入視訊額度),OUT(提出視訊額度)</param>
        /// <param name="remit">*存提額度</param>
        /// <param name="newcredit">存提後A公司額度(驗算用)</param>
        /// <param name="credit">存提前A公司額度(驗算用)</param>
        public Request Transfer(string username, string uppername, string remitno, tran_action action, decimal remit, decimal? newcredit, decimal? credit)
        {
            Request request = new Request("Transfer");
            request["website"] = this.website;
            request["username"] = username;
            request["uppername"] = uppername;
            request["remitno"] = remitno;
            request["action"] = action.ToString().ToLower();
            request["remit"] = (int)remit;
            request["newcredit"] = newcredit;
            request["credit"] = credit;
            request.setkey(this.website, 2, 7, username, remitno, this.KeyB_Transfer, null);
            request.GetResponse(this.api_url);
            return request;
        }

        /// <param name="website">*網站名稱</param>
        /// <param name="username">會員帳號</param>
        /// <param name="uppername">*上層帳號</param>
        /// <param name="rounddate">*日期 ex:2012/03/21, 2012-03-21</param>
        /// <param name="starttime">開始時間 ex:00:00:00 (BB體育遊戲無效)</param>
        /// <param name="endtime">結束時間 ex:23:59:59 (BB體育遊戲無效)</param>
        /// <param name="gamekind">*遊戲種類(1:球類,2:BB彩票,3:視訊,5:機率,12:彩票</param>
        /// <param name="gametype">參閱gametype, gamekine=12時, 此欄位不可省略</param>
        /// <param name="page">查詢頁數</param>
        /// <param name="pagelimit">每頁數量</param>
        /// <returns></returns>
        public Request BetRecord(string username, string uppername, DateTime rounddate, DateTime? starttime, DateTime? endtime, gamekind gamekind, gametype? gametype, int? page, int? pagelimit)
        {
            Request request = new Request("BetRecord");
            request["website"] = this.website;
            request["username"] = username;
            request["uppername"] = uppername ?? this.uppername;
            request["rounddate"] = rounddate.ToACTime().ToString("yyyy/MM/dd");
            if (starttime.HasValue) request["starttime"] = starttime.Value.AddHours(-12).ToString("HH:mm:ss");
            if (endtime.HasValue) request["endtime"] = endtime.Value.AddHours(-12).ToString("HH:mm:ss");
            request["gamekind"] = (int)gamekind;
            if (gametype.HasValue) request["gametype"] = gametype.Value.ToString();
            request["page"] = page;
            request["pagelimit"] = pagelimit;
            request.setkey(this.website, 1, 7, username, null, this.KeyB_getbet, null);
            request.GetResponse(this.api_url);
            return request;
        }
        public Request BetRecord2(string username, string uppername, DateTime start_date, DateTime end_date, DateTime? starttime, DateTime? endtime, gamekind gamekind, gametype? gametype, int? page, int? pagelimit)
        {
            Request request = new Request("BetRecord2");
            request["website"] = this.website;
            request["username"] = username;
            request["uppername"] = uppername ?? this.uppername;
            request["start_date"] = start_date.ToACTime().ToString("yyyy/MM/dd");
            request["end_date"] = end_date.ToACTime().ToString("yyyy/MM/dd");
            if (starttime.HasValue) request["starttime"] = starttime.Value.AddHours(-12).ToString("HH:mm:ss");
            if (endtime.HasValue) request["endtime"] = endtime.Value.AddHours(-12).ToString("HH:mm:ss");
            request["gamekind"] = (int)gamekind;
            if (gametype.HasValue) request["gametype"] = gametype.Value.ToString();
            request["page"] = page;
            request["pagelimit"] = pagelimit;
            request.setkey(this.website, 1, 7, username, null, this.KeyB_getbet, null);
            request.GetResponse(this.api_url);
            return request;
        }

        //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        //public class response
        //{
        //    [JsonProperty]
        //    public bool? result;

        //    [JsonProperty]
        //    public data data;
        //}

        //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        //public class data
        //{
        //    [JsonProperty]
        //    public StatusCode? Code;
        //    [JsonProperty]
        //    public string Message;
        //}

        public unsafe void test()
        {
            JObject obj = JObject.Parse(@"{""result"":true,""data"":[{""LoginName"":""tbtest001"",""Currency"":""RMB"",""Balance"":0,""TotalBalance"":0}],""pagination"":{""Page"":1,""PageLimit"":500,""TotalNumber"":1,""TotalPage"":1}}");

            //System.Numeric.
            //while (true)
            //{
            //    Guid guid = Guid.NewGuid();
            //    byte[] b1 = guid.ToByteArray();
            //    BitConverter.
            //    fixed (byte* p = b1)
            //    {
            //        *p &= 0x7f;
            //        *(p + 8) *= 0x7f;
            //        long* l1 = (long*)p;
            //        long* l2 = l1 + 1;
            //        if (*l1 < 0) Debugger.Break();
            //        if (*l2 < 0) Debugger.Break();
            //    }
                
            //    Array.Resize(ref b1, b1.Length + 1);
            //    System.Numerics.BigInteger bigint = new System.Numerics.BigInteger(b1);
            //    string s1 = bigint.ToString();

            //    string s4 = s1.Substring(Math.Max(s1.Length - 40, 0));
            //    string s2 = bigint.ToString("X");
            //    string s3 = guid.ToString();
            //    if (bigint.Sign == -1)
            //        Debugger.Break();
            //    Console.Write(s1.Length);
            //    Console.Write('\t');
            //    Console.WriteLine(s1);
            //}

            //CreateMember("tbtest001", null, null);
            Request r1;
            r1 = CheckUsrBalance("tbtest001", null);
            Debugger.Break();
            r1 = Login("tbtest001", null, null, null, null);
            Debugger.Break();
            r1 = Transfer("tbtest001", null, "0", tran_action.IN, 1, null, null);
            Debugger.Break();
            r1 = CheckUsrBalance("tbtest001", null);
            Debugger.Break();
            r1 = Transfer("tbtest001", null, "0", tran_action.OUT, 1, null, null);
            Debugger.Break();
            r1 = CheckUsrBalance("tbtest001", null);
            Debugger.Break();
            //CheckUsrBalance("tbtest%", null);
            //CheckUsrBalance(null, null);
            r1 = Logout("tbtest001");
            Debugger.Break();
            //response xxx = JsonConvert.DeserializeObject<response>(s);
        }


        public class Request
        {
            //StringWriter sw;
            //XmlTextWriter xml;

            public string Action { get; private set; }
            public Dictionary<string, string> parameters = new Dictionary<string, string>();
            public string get_parameter(string key)
            {
                if (this.parameters.ContainsKey(key))
                    return this.parameters[key];
                return null;
            }

            public Request(string action)
            {
                this.Action = action;
                //sw = new EncodingStringWriter();
                //xml = new XmlTextWriter(sw);
                //xml.WriteStartDocument();
                //xml.WriteStartElement("request");
                //xml.WriteAttributeString("action", this.Action = action);
                //xml.WriteStartElement("element");
            }

            public object this[string name]
            {
                set
                {
                    if (string.IsNullOrEmpty(this.RequestText))
                    {
                        if (value == null) return;
                        this.parameters[name] = Convert.ToString(value);
                        //xml.WriteStartElement(name);
                        //xml.WriteValue(Convert.ToString(value));
                        //xml.WriteEndElement();
                        //this[name, false] = value;
                    }
                }
            }

            public string _A { get; private set; }
            public string _B { get; private set; }
            public string _B_md5 { get; private set; }
            public string _C { get; private set; }

            public void setkey(string website, int a, int c, string username, string remitno, string keyb, DateTime? time)
            {
                this["key"] = string.Format("{0}{1}{2}",
                    this._A = RandomString.GetRandomString(RandomString.LowerLetter, a),
                    this._B_md5 = Crypto.MD5Hex(this._B = string.Format("{0}{1}{2}{3}{4:yyyyMMdd}", website, username, remitno, keyb, (time ?? DateTime.Now).ToACTime())),
                    this._C = RandomString.GetRandomString(RandomString.LowerLetter, c));
            }

            public string RequestText { get; private set; }
            public string RequestUrl { get; private set; }
            public string ResponseText { get; private set; }
            public JObject Response;

            public bool? result
            {
                get { try { return this.Response.Value<bool?>("result"); } catch { return null; } }
            }

            public StatusCode? MessageCode
            {
                get { try { return (StatusCode?)(this.Response["data"]).Value<int?>("Code"); } catch { return null; } }
            }

            public string Message
            {
                get { try { return this.Response["data"].Value<string>("Message"); } catch { return null; } }
            }

            public UserBalance[] balance_data
            {
                get { try { return this.Response["data"].ToObject<UserBalance[]>(); } catch { return null; } }
            }

            public IEnumerable<UserBalance> balance_data_each()
            {
                UserBalance[] data = this.balance_data;
                if (this.balance_data != null)
                {
                    foreach (UserBalance n in data)
                    {
                        n.LoginName *= text.ValidAsString;
                        if (string.IsNullOrEmpty(n.LoginName)) continue;
                        if (!n.Balance.HasValue) continue;
                        if (!n.TotalBalance.HasValue) continue;
                        yield return n;
                    }
                }
            }

            public Pagination pagination
            {
                get { try { return this.Response["pagination"].ToObject<Pagination>(); } catch { return null; } }
            }

            string getxml()
            {
                StringWriter sw;
                using (sw = new EncodingStringWriter())
                using (XmlTextWriter xml = new XmlTextWriter(sw))
                {
                    xml.WriteStartDocument();
                    xml.WriteStartElement("request");
                    xml.WriteAttributeString("action", this.Action);
                    xml.WriteStartElement("element");
                    foreach (KeyValuePair<string, string> p in this.parameters)
                    {
                        xml.WriteStartElement(p.Key);
                        xml.WriteValue(p.Value);
                        xml.WriteEndElement();
                    }
                }
                return sw.ToString();
            }

            public void GetResponse(string api_url)
            {
                this.ResponseText = util.GetResponse(this.RequestUrl = api_url, this.RequestText = this.getxml(), bbin.log_prefix);
                try { this.Response = JObject.Parse(this.ResponseText); }
                catch { this.Response = Tools._null<JObject>.value; }
                //var a = this.result;
                //JToken token = this.Response.GetValue("result", StringComparison.OrdinalIgnoreCase);
                //bool? result = Newtonsoft.Json.Linq.Extensions.Convert<JToken, bool?>(token);
            }
        }
        public class Pagination
        {
            public int? Page;
            public int? PageLimit;
            public int? TotalNumber;
            public int? TotalPage;
        }
        public class UserBalance
        {
            public string LoginName;
            public string Currency;
            public decimal? Balance;
            public decimal? TotalBalance;
        }
    }
}