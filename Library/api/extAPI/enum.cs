using System;
using System.Collections.Generic;
using System.Web;

namespace extAPI.kg
{
    public enum KGStatusCode
    {
        VENDOR_NOT_AUTHORIZED,
        VENDOR_ACCOUNT_SUSPENDED,
        NOT_ACCEPT_NEGATIVE_AMOUNT_TRANSFER,
        GAME_SERVER_MAINTENANCE,
        GAME_SERVER_NOT_APPROPRIATE,
        PLAYER_ACCOUNT_SUSPENDED,
        PLAYER_CURRENCY_NOT_SUPPORT,
        PLAYER_CURRENCY_NOT_MATCH_BEFORE,
        PLAYER_STAKE_NOT_BELONG_TO_VENDOR,
        TRIAL_ACCOUNT_CONFLICT
    }
    public enum IsTrial : int
    {
        是 = 0, 否 = 1,
    }
    public enum KGHourType : int
    { 
        十二点到12点=1,
        零点到23点=2
    }
    public struct KGGameType
    { 
     public static string KENO ="KENO";
     public static string SSC ="SSC";
    }
}