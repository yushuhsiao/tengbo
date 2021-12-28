using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Tools.Protocol;
using web;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

// 接入的遊戲放在 iframe
// 狀態資料庫
// 登入驗證碼: admin-any, agent-yes, member-yes

// 子帳戶

// 銀行卡管理
// 金流處理啟用/停用
// flags : 有效/已失效, 顯示在前台

namespace BU
{
    [JsonProtocol.UnderlyingValueInDictionaryKey, Flags]
    public enum UserType : byte { Guest = 0, Admin = 1, Agent = 2, Member = 4 }
    //public enum AcceptOrReject
    //{
    //    /// <summary>
    //    /// 接受請求
    //    /// </summary>
    //    Accept = 1,
    //    /// <summary>
    //    /// 拒絕請求
    //    /// </summary>
    //    Reject = 2,
    //}

    //public enum CommitOrRollback
    //{
    //    /// <summary>
    //    /// 處理成功
    //    /// </summary>
    //    Commit = 1,
    //    /// <summary>
    //    /// 處理失敗
    //    /// </summary>
    //    Rollback = 2,
    //}

    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum LogType : int
    {
        /// <summary>
        /// 存款
        /// </summary>
        Deposit = 1,            // 存款 (存款,第三方)
        /// <summary>
        /// 提款
        /// </summary>
        Withdrawal = 2,
        /// <summary>
        /// 退還提款預扣
        /// </summary>
        WithdrawalRollback = 3,
        /// <summary>
        /// 戶內轉帳-存款
        /// </summary>
        GameDeposit = 4,
        /// <summary>
        /// 戶內轉帳-提款
        /// </summary>
        GameWithdrawal = 5,
        /// <summary>
        /// 戶內轉帳-退還存款預扣
        /// </summary>
        GameDepositRollback = 6,
        /// <summary>
        /// 其他
        /// </summary>
        Misc,
        /// <summary>
        /// 提款預扣
        /// </summary>
        WithdrawalWithholding = 8,
        /// <summary>
        /// 智付 (第三方支付)
        /// </summary>
        Dinpay = 11,
        /// <summary>
        /// 易寶 (第三方支付)
        /// </summary>
        YeePay = 12,
        /// <summary>
        /// 汇潮 (第三方支付)
        /// </summary>
        Ecpss = 13,
        /// <summary>
        /// 支付寶
        /// </summary>
        Alipay = 21,
        /// <summary>
        /// 財富通
        /// </summary>
        TenPay = 22,

        /// <summary>
        /// 上分
        /// </summary>
        LoadingBalance = 101,
        /// <summary>
        /// 卸分
        /// </summary>
        UnloadingBalance,
        /// <summary>
        /// 子代理/子會員 上分
        /// </summary>
        LoadingBalanceToUser,
        /// <summary>
        /// 子代理/子會員 卸分
        /// </summary>
        UnloadingBalanceFromUser,

        ///// <summary>
        ///// 子代理存款
        ///// </summary>
        //ChildAgentDeposit = 111,
        ///// <summary>
        ///// 子代理提款
        ///// </summary>
        //ChildAgentWithdrawal = 112,
        ///// <summary>
        ///// 子會員存款
        ///// </summary>
        //ChildMemberDeposit = 113,
        ///// <summary>
        ///// 子會員提款
        ///// </summary>
        //ChildMemberWithdrawal = 114,


        Promos = 30,
        BetAmt = Promos + 3,
        FirstDeposit = Promos + 1,
        SecondDeposit = Promos + 6,
        AgentShare = Promos + 5, // 代理佣金
        首存優惠 = FirstDeposit,
        次存优惠 = SecondDeposit,
        存款優惠 = Promos + 2,
        洗碼優惠 = BetAmt,
        全勤優惠 = Promos + 4,
        彩金贈送 = Promos + 9,
        //首存優惠_前置單 = 首存優惠 + 10,
        //存款優惠_前置單 = 存款優惠 + 10,
        //洗碼優惠_前置單 = 洗碼優惠 + 10,
        //全勤優惠_前置單 = 全勤優惠 + 10,


        //VIP直通车 = Promos + 21,
        好友推荐 = Promos + 22,
        复活礼金 = Promos + 23,
        生日礼金 = Promos + 24,
        晋级奖金 = Promos + 25,
        周周红利 = Promos + 26,
        绿色通道入款 = Promos + 27,
        PromosMAX = 99,
    }

    //public class LogTypeDefine
    //{
    //    static readonly LogTypeDefine[] s_instance;
    //    string a;
    //    string b;
    //    static LogTypeDefine()
    //    {
    //        List<LogType> _promos_tmp = new List<LogType>();
    //        foreach (LogType l in Enum.GetValues(typeof(LogType)))
    //            if ((l > LogType.Promos) && (l < LogType.PromosMAX))
    //                _promos_tmp.Add(l);
    //        LogType[] _promos = _promos_tmp.ToArray();
    //        LogType[] _deposit = new LogType[] { LogType.Deposit, LogType.Alipay, LogType.Dinpay };
    //        List<LogTypeDefine> tmp = new List<LogTypeDefine>();
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.Deposit, b = "       ", prefix = "A", Table1 = "tranBalance1", Table2 = "tranBalance2", AllLogType = _deposit });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.Deposit, b = "       ", prefix = "A", Table1 = "tranBalance1", Table2 = "tranBalance2", AllLogType = _deposit });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.Withdrawal, b = "    ", prefix = "B", Table1 = "tranBalance1", Table2 = "tranBalance2" });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.Withdrawal, b = "    ", prefix = "B", Table1 = "tranBalance1", Table2 = "tranBalance2" });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.GameDeposit, b = "   ", prefix = "C", Table1 = "tranGame1   ", Table2 = "tranGame2   " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.GameDeposit, b = "   ", prefix = "C", Table1 = "tranGame1   ", Table2 = "tranGame2   " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.GameWithdrawal, b = "", prefix = "D", Table1 = "tranGame1   ", Table2 = "tranGame2   " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.GameWithdrawal, b = "", prefix = "D", Table1 = "tranGame1   ", Table2 = "tranGame2   " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.Dinpay, b = "        ", prefix = "I", Table1 = "tranBalance1", Table2 = "tranBalance2", AllLogType = _deposit });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.Dinpay, b = "        ", prefix = "I", Table1 = "tranBalance1", Table2 = "tranBalance2", AllLogType = _deposit });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.Alipay, b = "        ", prefix = "J", Table1 = "tranBalance1", Table2 = "tranBalance2", AllLogType = _deposit });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.Alipay, b = "        ", prefix = "J", Table1 = "tranBalance1", Table2 = "tranBalance2", AllLogType = _deposit });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.AgentShare, b = "    ", prefix = "G", Table1 = "tranPromo1  ", Table2 = "tranPromo2  " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.AgentShare, b = "    ", prefix = " ", Table1 = "tranPromo1  ", Table2 = "tranPromo2  " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.FirstDeposit, b = "  ", prefix = " ", Table1 = "tranPromo1  ", Table2 = "tranPromo2  " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.FirstDeposit, b = "  ", prefix = "F", Table1 = "tranPromo1  ", Table2 = "tranPromo2  " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Agent, a = " ", LogType = LogType.BetAmt, b = "        ", prefix = "E", Table1 = "tranPromo1  ", Table2 = "tranPromo2  " });
    //        tmp.Add(new LogTypeDefine() { UserType = UserType.Member, a = "", LogType = LogType.BetAmt, b = "        ", prefix = "E", Table1 = "tranPromo1  ", Table2 = "tranPromo2  " });

    //        foreach (UserType u in new UserType[] { UserType.Agent, UserType.Member })
    //        {
    //            foreach (LogType l in _promos)
    //            {
    //                LogTypeDefine n = null;
    //                for (int i = tmp.Count - 1; (i > 0) && (n == null); i--)
    //                    if ((tmp[i].UserType == u) && (tmp[i].LogType == l))
    //                        n = tmp[i];
    //                if (n == null)
    //                    continue;
    //                tmp.Add(new LogTypeDefine() { UserType = u, LogType = l, prefix = "P", Table1 = "tranPromo1", Table2 = "tranPromo2", AllLogType = _promos });
    //            }
    //        }
    //        foreach (LogTypeDefine n in tmp)
    //        {
    //            if (!string.IsNullOrEmpty(n.Table1)) n.Table1 = n.Table1.Trim();
    //            if (!string.IsNullOrEmpty(n.Table2)) n.Table2 = n.Table2.Trim();
    //            if (!string.IsNullOrEmpty(n.prefix)) n.prefix = n.prefix.Trim();
    //            if (n.AllLogType == null)
    //                n.AllLogType = new LogType[] { n.LogType };
    //        }
    //        s_instance = tmp.ToArray();
    //    }
    //    public static LogTypeDefine GetDefine(UserType? userType, LogType? logType)
    //    {
    //        if (userType.HasValue && logType.HasValue)
    //            foreach (LogTypeDefine n in s_instance)
    //                if ((n.UserType == userType.Value) && (n.LogType == logType.Value))
    //                    return n;
    //        return null;
    //    }

    //    LogTypeDefine() { }
    //    public UserType UserType { get; private set; }
    //    public LogType LogType { get; private set; }
    //    public LogType[] AllLogType { get; private set; }
    //    public string prefix { get; private set; }
    //    public string Table1 { get; private set; }
    //    public string Table2 { get; private set; }
    //}

    //partial class text
    //{
    //    public static bool IsPromos(this LogType? logType)
    //    {
    //        return (logType.Value >= LogType.Promos) && (logType.Value <= LogType.PromosMAX);
    //    }

    //    public static string GetPrefix(this LogType? logType, params LogType[] limit)
    //    {
    //        if (!logType.HasValue)
    //            return null;
    //        if (limit.Length > 0)
    //        {
    //            if (!limit.Conatins(logType.Value))
    //                return null;
    //        }
    //        switch (logType.Value)
    //        {
    //            case LogType.Deposit: return "A";
    //            case LogType.Withdrawal: return "B";
    //            case LogType.GameDeposit: return "C";
    //            case LogType.GameWithdrawal: return "D";
    //            case LogType.Ecpss:
    //            case LogType.YeePay:
    //            case LogType.Dinpay: return "I";
    //            case LogType.Alipay: return "J";
    //            //case LogType.首存優惠_前置單:
    //            case LogType.首存優惠: return "E";
    //            //case LogType.存款優惠_前置單:
    //            case LogType.存款優惠: return "F";
    //            //case LogType.洗碼優惠_前置單:
    //            case LogType.洗碼優惠: return "G";
    //            //case LogType.全勤優惠_前置單:
    //            case LogType.全勤優惠: return "H";
    //            //case LogType.彩金贈送: return "K";
    //            default:
    //                if (logType.IsPromos())
    //                    return "K";
    //                return null;
    //        }
    //    }

    //    //static Dictionary<UserType, Dictionary<LogType, LogType[]>> s_LogTypes = new Dictionary<UserType, Dictionary<LogType, LogType[]>>();
    //    //public static LogType[] ContainsLogTypes(this LogType logType, UserType userType)
    //    //{
    //    //    lock (s_LogTypes)
    //    //    {
    //    //        if (s_LogTypes.Count == 0)
    //    //        {
    //    //            s_LogTypes[UserType.Agent] = new Dictionary<LogType, LogType[]>();
    //    //            s_LogTypes[UserType.Member] = new Dictionary<LogType, LogType[]>();
    //    //            s_LogTypes[UserType.Agent][LogType.Deposit] = new LogType[] { LogType.Deposit, LogType.Dinpay, LogType.Alipay };
    //    //            s_LogTypes[UserType.Member][LogType.Deposit] = new LogType[] { LogType.Deposit, LogType.Dinpay, LogType.Alipay };
    //    //            List<LogType> types = new List<LogType>();
    //    //            foreach (LogType t in Enum.GetValues(typeof(LogType)))
    //    //                if ((t >= LogType.Promos) && (t < LogType.PromosMAX))
    //    //                    types.Add(t);
    //    //            s_LogTypes[UserType.Agent][LogType.Promos] = types.ToArray();
    //    //            s_LogTypes[UserType.Member][LogType.Promos] = types.ToArray();
    //    //        }
    //    //        if (s_LogTypes.ContainsKey(userType))
    //    //        {
    //    //            if (s_LogTypes[userType].ContainsKey(logType))
    //    //                return s_LogTypes[userType][logType];
    //    //            else
    //    //                return s_LogTypes[userType][logType] = new LogType[] { logType };
    //    //        }
    //    //        return Tools._null<LogType>.array;
    //    //    }
    //    //}

    //    //static text()
    //    //{
    //    //    text.MemberDepositLogTypes = new LogType[] { LogType.Deposit, LogType.Dinpay, LogType.Alipay, LogType.YeePay, LogType.Ecpss };
    //    //    text.MemberWithdrawalLogTypes = new LogType[] { LogType.Withdrawal }; //{ LogType.WithdrawalRollback, LogType.WithdrawalWithholding };
    //    //    text.MemberGameDepositLogTypes = new LogType[] { LogType.GameDeposit };
    //    //    text.MemberGameWithdrawalLogTypes = new LogType[] { LogType.GameWithdrawal };
    //    //    text.MemberGameDepositRollbackLogTypes = new LogType[] { LogType.GameDepositRollback };
    //    //}

    //    //public static LogType[] MemberDepositLogTypes { get; private set; }
    //    //public static LogType[] MemberWithdrawalLogTypes { get; private set; }
    //    //public static LogType[] MemberGameDepositLogTypes { get; private set; }
    //    //public static LogType[] MemberGameWithdrawalLogTypes { get; private set; }
    //    //public static LogType[] MemberGameDepositRollbackLogTypes { get; private set; }
    //    //public static string Join(this LogType[] types)
    //    //{
    //    //    StringBuilder s = new StringBuilder();
    //    //    for (int i = 0; i < types.Length; i++)
    //    //    {
    //    //        if (i > 0)
    //    //            s.Append(",");
    //    //        s.Append((int)types[i]);
    //    //    }
    //    //    return s.ToString();
    //    //}

    //    //public static long? GroupRowID(int? corpID, byte? groupID)
    //    //{
    //    //    if (corpID.HasValue && groupID.HasValue)
    //    //    {
    //    //        long ret = corpID.Value;
    //    //        ret <<= 8;
    //    //        ret |= groupID.Value;
    //    //        return ret;
    //    //    }
    //    //    return null;
    //    //}
    //    //public static int? GroupRowID_CorpID(long? id)
    //    //{
    //    //    if (id.HasValue)
    //    //        return (int)(id.Value >> 8);
    //    //    return null;
    //    //}
    //    //public static byte? GroupRowID_GroupID(long? id)
    //    //{
    //    //    if (id.HasValue)
    //    //        return (byte)(id.Value);
    //    //    return null;
    //    //}
    //    //public static void GroupRowID(long? id, out int? corpID, out byte? groupID)
    //    //{
    //    //    if (id.HasValue)
    //    //    {
    //    //        corpID = (int)(id.Value >> 8);
    //    //        groupID = (byte)(id.Value);
    //    //    }
    //    //    else
    //    //    {
    //    //        corpID = null;
    //    //        groupID = null;
    //    //    }
    //    //}
    //}

    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum TranState
    {
        /// <summary>
        /// 未處理
        /// </summary>
        Initial = 0,
        /// <summary>
        /// 已受理 (預扣)
        /// </summary>
        Accepted = 1,
        /// <summary>
        /// 不受理
        /// </summary>
        Rejected = 2,
        /// <summary>
        /// 轉帳完成
        /// </summary>
        Transferred = 3,
        /// <summary>
        /// 轉帳失敗
        /// </summary>
        Failed = 4,
    }

    [Flags]
    public enum TranFlag : byte
    {
        Member1 = 0x01,     // 子會員可使用存提款
        Member2 = 0x02,     // 子會員可使用撥分
        Agent1 = 0x10,     // 子代理可使用存提款
        Agent2 = 0x20,     // 子代理可使用撥分
    }

    enum LogType_ : byte
    {
        在线支付,
        支付补单,
        转账补单,
        //存款,
        //提款,
        //退还提款,
        奖品,
        新开户,
        //首存优惠,
        //洗码优惠,   // 總投注額
        转账优惠,
        额度转入,
        额度转出,
        手工增减额度,
        试玩账户添加额度,
        退还转账转入,
        促销优惠,
        合作伙伴佣金,
        加减额度,
        退还加减额度,
        生日礼金,
        //全勤礼金,
        友情推荐,
        修复转账,
        进码,
        出码,
        退还出码,
        //存款优惠,
        砸金蛋,
    }

    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum OrderType
    {
        /// <summary>
        /// 存款
        /// </summary>
        Deposit,
        /// <summary>
        /// 提款
        /// </summary>
        Withdrawal,
    }

    public enum BetStatus : int
    {
        未結算 = 0, 已結算 = 1,
    }

    [JsonProtocol.UnderlyingValueInDictionaryKey]
    [JsonConverter(typeof(LockedJsonConverter))]
    public enum Locked : byte
    {
        Active = 0, Locked = 1
    }
    [_DebuggerStepThrough]
    class LockedJsonConverter : JsonProtocol.BooleanJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object value = base.ReadJson(reader, objectType, existingValue, serializer);
            if (value is bool)
                return ((bool)value) ? Locked.Locked : Locked.Active;
            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Locked)
                serializer.Serialize(writer, (byte)value);
            else
                serializer.Serialize(writer, value);
        }
    }

    [JsonProtocol.UnderlyingValueInDictionaryKey]
    [JsonConverter(typeof(EnabledJsonConverter))]
    public enum Enabled : byte
    {
        Enabled = 1, Disabled = 0,
    }
    class EnabledJsonConverter : JsonProtocol.BooleanJsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object value = base.ReadJson(reader, objectType, existingValue, serializer);
            if (value is bool)
                return ((bool)value) ? Enabled.Enabled : Enabled.Disabled;
            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Locked)
                serializer.Serialize(writer, (byte)value);
            else
                serializer.Serialize(writer, value);
        }
    }

    // 性別
    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum UserSex : byte
    {
        Secret = 0, Male = 1, Female = 2
    }

    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum GameLocked : byte
    {
        Active = 0, Pause = 1, Locked = 2,
    }

    public enum MemberGameLocked : byte
    {
        Active = 0, Locked = 1, Inital = 2
    }

    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum GameID : int
    {
        HG = 1,
        EA = 2,
        WFT = 3,
        KENO = 4,
        SUNBET = 5,         // 太阳城
        AG = 6,             // AG包厅
        KENO_SSC = 7,
        WFT_SPORTS = 8,
        BBIN = 9,           // 波音
        CROWN_SPORTS = 10,  // 皇冠體育
        AG_AG = 11,         // AG旗舰厅
        AG_AGIN = 12,       // AG国际厅
        AG_DSP = 13,        // AG实地厅
        SALON = 14,         // 沙龍
        PT = 15,            // PT老虎机
        EXTRA = 254,        // 補點用
    }

    // BalanceMode
    // 
    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum BalanceMode : byte
    {
        Cash = 0x01,        // 可使用現金流
        Parent = 0x02,      // 上級存入/提出
    }
}