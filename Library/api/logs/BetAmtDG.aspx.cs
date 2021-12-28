using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;


[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class GameLog_BetAmtDG_Row
{
    [DbImport, JsonProperty]
    public int? sn;
    [DbImport, JsonProperty]
    public DateTime? ACTime;
    [DbImport, JsonProperty]
    public GameID? GameID;
    [DbImport, JsonProperty]
    public string GameType;
    [DbImport, JsonProperty]
    public int? CorpID;
    [DbImport, JsonProperty]
    public int? MemberID;
    [DbImport, JsonProperty]
    public string ACNT;
    [DbImport, JsonProperty]
    public string Name;
    [DbImport, JsonProperty]
    public int? AgentID;
    [DbImport, JsonProperty]
    public string AgentACNT;
    [DbImport, JsonProperty]
    public decimal? BetAmount;
    [DbImport, JsonProperty]
    public decimal? BetAmountAct;
    [DbImport, JsonProperty]
    public decimal? Payout;
    [DbImport, JsonProperty]
    public DateTime CreateTime;
    [DbImport, JsonProperty]
    public _SystemUser CreateUser;
}

public abstract class GameLog_BetAmtDG_RowCommand
{
    [JsonProperty]
    public int? sn { get; set; }
    [JsonProperty]
    public DateTime? ACTime { get; set; }
    [JsonProperty]
    public int? CorpID { get; set; }
    //[JsonProperty]
    //public string AgentACNT;
    [JsonProperty]
    public string ACNT { get; set; }
    [JsonProperty]
    public GameID? GameID { get; set; }
    [JsonProperty]
    public string GameType { get; set; }
    [JsonProperty]
    public decimal? BetAmount { get; set; }
    [JsonProperty]
    public decimal? BetAmountAct { get; set; }
    [JsonProperty]
    public decimal? Payout { get; set; }
}
