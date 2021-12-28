using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using web;

namespace BU
{
    //[JsonProtocol.UnderlyingValueInDictionaryKey]
    //public enum BankCardType : byte
    //{
    //    Unknown = 0,
    //    Deposit = 10,
    //    Deposit_Normal = Deposit + MemberType.Normal,
    //    Deposit_VIP = Deposit + MemberType.VIP,
    //    Deposit_VIP2 = Deposit + MemberType.VIP2,
    //    Deposit_Other1,
    //    Deposit_Other2,
    //    WithDrawal = 20,
    //    WithDrawal_Normal = WithDrawal + MemberType.Normal,
    //    WithDrawal_VIP = WithDrawal + MemberType.VIP,
    //    WithDrawal_VIP2 = WithDrawal + MemberType.VIP2,
    //    WithDrawal_Other1,
    //    WithDrawal_Other2,
    //}
}

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BankCardRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport]
        public byte? GroupID;
        [JsonProperty("GroupID")]
        long? _out_GroupID
        {
            get { return text.GroupRowID(this.CorpID, this.GroupID); }
        }
        //[DbImport, JsonProperty]
        //public MemberType? MemberType;
        [DbImport, JsonProperty]
        public LogType? LogType;
        [DbImport, JsonProperty]
        public string CardID;
        [DbImport, JsonProperty]
        public string BankName;
        [DbImport, JsonProperty]
        public string AccName;
        [DbImport, JsonProperty]
        public string Loc1;
        [DbImport, JsonProperty]
        public string Loc2;
        [DbImport, JsonProperty]
        public string Loc3;
        [DbImport, JsonProperty]
        public Locked? Locked;
        [DbImport("pwd"), JsonProperty]
        public string Password;
        [DbImport, JsonProperty]
        public DateTime? ExpireTime;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;
    }
}
