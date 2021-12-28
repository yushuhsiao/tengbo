using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace web
{
    public abstract class GameLogRow
    {
        [DbImport, JsonProperty]
        public long? ID;
        [DbImport, JsonProperty]
        public DateTime? ACTime;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public int? MemberID;
        [DbImport, JsonProperty]
        public string ACNT;
        [DbImport, JsonProperty]
        public int? ParentID;
        [DbImport, JsonProperty]
        public string ParentACNT;
    }
    public class GameLogRow_HG : GameLogRow
    {
        [DbImport, JsonProperty]
        public string AccountId;
        [DbImport, JsonProperty]
        public string UserID;
        [DbImport, JsonProperty]
        public DateTime? BetStartTime;
        [DbImport, JsonProperty]
        public DateTime? BetEndTime;
        [DbImport, JsonProperty]
        public string TableId;
        [DbImport, JsonProperty]
        public string TableName;
        [DbImport, JsonProperty]
        public string GameId;
        [DbImport, JsonProperty]
        public string BetId;
        [DbImport, JsonProperty]
        public decimal? BetAmount;
        [DbImport, JsonProperty]
        public decimal? BetAmountAct;
        [DbImport, JsonProperty]
        public decimal? Payout;
        [DbImport, JsonProperty]
        public string Currency;
        [DbImport, JsonProperty]
        public string GameType;
        [DbImport, JsonProperty]
        public string BetSpot;
        [DbImport, JsonProperty]
        public string BetNo;
    }
    public class GameLogRow_BBIN : GameLogRow
    {
        [DbImport, JsonProperty]
        public extAPI.bbin.gamekind? gamekind;
        [DbImport, JsonProperty]
        public string UserName;
        [DbImport, JsonProperty]
        public long? WagersID;
        [DbImport, JsonProperty]
        public DateTime? WagersDate;
        [DbImport, JsonProperty]
        public string SerialID;
        [DbImport, JsonProperty]
        public string RoundNo;
        [DbImport, JsonProperty]
        public string GameType;
        [DbImport, JsonProperty]
        public extAPI.bbin.gametype GameTypei;
        [DbImport, JsonProperty]
        public string GameCode;
        [DbImport, JsonProperty]
        public string Result;
        [DbImport, JsonProperty]
        public string ResultType;
        [DbImport, JsonProperty]
        public string Card;
        [DbImport, JsonProperty]
        public decimal? BetAmount;
        [DbImport, JsonProperty]
        public decimal? Payoff;
        [DbImport, JsonProperty]
        public string Currency;
        [DbImport, JsonProperty]
        public decimal? ExchangeRate;
        [DbImport, JsonProperty]
        public decimal? Commissionable;
        [DbImport, JsonProperty]
        public decimal? Commission;
        [DbImport, JsonProperty]
        public string IsPaid;
        [DbImport, JsonProperty]
        public decimal? f_sum;
    }
    public class GameLogRow_AG : GameLogRow
    {
        [DbImport, JsonProperty]
        public long? billNo;
        [DbImport, JsonProperty]
        public string playerName;
        [DbImport, JsonProperty]
        public long? agentCode;
        [DbImport, JsonProperty]
        public string gameCode;
        [DbImport, JsonProperty]
        public decimal? netAmount;
        [DbImport, JsonProperty]
        public DateTime? betTime;
        [DbImport, JsonProperty]
        public string gameType;
        [DbImport, JsonProperty]
        public decimal? betAmount;
        [DbImport, JsonProperty]
        public decimal? validBetAmount;
        [DbImport, JsonProperty]
        public byte? flag;
        [DbImport, JsonProperty]
        public int? playType;
        [DbImport, JsonProperty]
        public string currency;
        [DbImport, JsonProperty]
        public string tableCode;
        [DbImport, JsonProperty]
        public string loginIP;
        [DbImport, JsonProperty]
        public DateTime? recalcuTime;
        [DbImport, JsonProperty]
        public string platformId;
        [DbImport, JsonProperty]
        public string platformType;
        [DbImport, JsonProperty]
        public string stringex;
        [DbImport, JsonProperty]
        public string remark;
        [DbImport, JsonProperty]
        public string round;
    }
}
