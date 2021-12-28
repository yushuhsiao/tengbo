using BU;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using web;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class UserGameBalance : IRowCommand
{
    [JsonProperty]
    protected string GameID;

    protected game.IUserGameRowCommand GetProc(string json_s)
    {
        Member member = HttpContext.Current.User as Member;
        if (member == null) return null;
        GameID? gameID = this.GameID.ToEnum<GameID>();
        game.IUserGameRowCommand proc = game.GetUserGameRowCommand(BU.UserType.Member, gameID, json_s, false);
        proc.UserID = member.ID;
        return proc;
    }

    [ObjectInvoke]
    object execute(UserGameBalance command, string json_s, params object[] args)
    {
        game.IUserGameRowCommand proc = this.GetProc(json_s);
        decimal? result = null;
        if (proc != null)
        {
            try
            {
                game.UserGameRow _usergame = proc.GetBalance(null, null);
                if (_usergame != null)
                    result = _usergame.Balance;
            }
            catch { result = null; }
        }
        if (result.HasValue)
            return string.Format("{0:0.00}", result);
        else
            return "N/A";
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class UserGameTran : IRowCommand
{
    // parameters

    [JsonProperty("gameid_1")]
    string GameID_1;
    [JsonProperty("gameid_0")]
    string GameID_0;
    [JsonProperty("amount")]
    decimal? amount;



    // results

    [JsonProperty]
    public string Balance;
    [JsonProperty]
    public string GameBalance;

    [ObjectInvoke]
    static object execute(UserGameTran command, string json_s, params object[] args)
    {
        Member member = HttpContext.Current.User as Member;
        if (command.amount <= 0) command.amount = null;
        GameID? gameID_1 = command.GameID_1.ToEnum<GameID>();
        GameID? gameID_0 = command.GameID_0.ToEnum<GameID>();
        if (command.amount.HasValue)
        {
            tran.Game.GameRowData result = null;
            if (gameID_0.HasValue)
            {
                tran.Game.GameDepositRowCommand cmd = new tran.Game.GameDepositRowCommand()
                {
                    op_Insert = true,
                    GameID = gameID_0,
                    UserType = UserType.Member,
                    UserID = member.ID,
                    Amount = command.amount,
                    LogType = LogType.GameDeposit,
                };
                using (cmd) result = cmd.Execute(null, json_s, args);
            }
            else if (gameID_1.HasValue)
            {
                tran.Game.GameWithdrawalRowCommand cmd = new tran.Game.GameWithdrawalRowCommand()
                {
                    op_Insert = true,
                    GameID = gameID_1,
                    UserID = member.ID,
                    UserType = UserType.Member,
                    Amount = command.amount,
                    LogType = LogType.GameWithdrawal,
                };
                using (cmd) result = cmd.Execute(null, json_s, args);
            }
            if (result != null)
            {
                command.Balance = Member.BalanceString(result.UserBalance);
                if (result.UserGameRow != null)
                    command.GameBalance = Member.BalanceString(result.UserGameRow.Balance);
                return command;
            }
        }
        return "";
    }
}


//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//public class GameBalance
//{
//    [JsonProperty]
//    string GameID;

//    [ObjectInvoke]
//    static object execute(GameBalance command, string json_s, params object[] args)
//    {
//        Member member = HttpContext.Current.User as Member;
//        if (member == null) return null;
//        GameID? gameID = command.GameID.ToEnum<GameID>();
//        decimal? result = null;
//        try
//        {
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                MemberGameRow row = MemberGame.GetInstance(gameID).SelectRow(sqlcmd, member.ID, true);
//                if (row != null)
//                    result = row.Balance;
//                //result = g.GetBalance(sqlcmd, row);
//            }
//        }
//        catch { result = null; }
//        if (result.HasValue)
//            return string.Format("{0:0.00}", result);
//        else
//            return "N/A";
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//public class GameTran : IRowCommand
//{
//    [JsonProperty("gameid_1")]
//    string GameID_1;
//    [JsonProperty("gameid_0")]
//    string GameID_0;
//    [JsonProperty("amount")]
//    decimal? amount;

//    [ObjectInvoke]
//    static object execute(GameTran command, string json_s, params object[] args)
//    {
//        Member member = HttpContext.Current.User as Member;
//        if (command.amount <= 0) command.amount = null;
//        GameID? gameID_1 = command.GameID_1.ToEnum<GameID>();
//        GameID? gameID_0 = command.GameID_0.ToEnum<GameID>();
//        if (command.amount.HasValue && (gameID_0.HasValue || gameID_1.HasValue))
//        {
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                GameTranRow tran = new GameTranRowCommand()
//                {
//                    GameID = gameID_0 ?? gameID_1,
//                    MemberID = member.ID,
//                    LogType = gameID_0.HasValue ? LogType.GameDeposit : LogType.GameWithdrawal,
//                    Amount1 = command.amount,
//                }.Insert(json_s, sqlcmd);
//                command.Balance = Member.BalanceString(tran.Balance);
//                //tran.MemberGameRow = MemberGame.GetInstance(tran.GameID).SelectRow(sqlcmd, tran.MemberID, true);
//                if (tran.MemberGameRow != null)
//                    command.GameBalance = Member.BalanceString(tran.MemberGameRow.Balance);
//                return command;
//            }
//        }
//        return "";
//    }

//    [JsonProperty]
//    public string Balance;
//    [JsonProperty]
//    public string GameBalance;
//}
