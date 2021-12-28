using BU;
using Newtonsoft.Json;
using System;
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
    public int? ParentID;
    [DbImport, JsonProperty]
    public string ParentACNT;
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
    //public string ParentACNT;
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

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtDG_Insert : GameLog_BetAmtDG_RowCommand, IRowCommand, IDisposable
{
    SqlCmd m_sqlcmd;
    SqlCmd sqlcmd
    {
        get
        {
            SqlCmd ret = m_sqlcmd;
            if (ret == null)
                ret = m_sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite);
            return ret;
        }
    }

    void IDisposable.Dispose()
    {
        using (sqlcmd) m_sqlcmd = null;
    }

    [ObjectInvoke, Permissions(Permissions.Code.log_betamtdg, Permissions.Flag.Write)]
    GameLog_BetAmtDG_Row insert(GameLog_BetAmtDG_Insert command, string json_s, params object[] args)
    {
        if ((this.BetAmount == 0) && (this.BetAmountAct == 0) && (this.Payout == 0))
            this.BetAmount = this.BetAmountAct = this.Payout = null;
        this.GameID = this.GameID ?? 0;
        MemberRow member;
        if (this.GameID == 0)
        {
            this.ACNT *= text.ValidAsACNT2;
            member = MemberRow.GetMemberEx(sqlcmd, null, this.CorpID, this.ACNT, "ParentID", "ParentACNT");
        }
        else
        {
            this.ACNT *= text.ValidAsGameACNT;
            game.IUserGameRowCommand g_cmd = game.GetUserGameRowCommand(BU.UserType.Member, this.GameID, null, true);
            if (g_cmd == null) throw new RowException(RowErrorCode.InvalidGameID);
            game.UserGameRow usergame = g_cmd.SelectUserRow(sqlcmd, null, this.ACNT, false);
            if (usergame == null) throw new RowException(RowErrorCode.GameAccountNotFound);
            member = MemberRow.GetMemberEx(sqlcmd, usergame.UserID, null, null, "ParentID", "ParentACNT");
        }

        sqltool s = new sqltool();
        s["* ", "ACTime", "              "] = this.ACTime;
        s["* ", "GameID", "              "] = this.GameID;
        s["*N", "GameType", "            "] = this.GameType * text.ValidAsString2;
        s["* ", "CorpID", "              "] = member.CorpID;
        s["* ", "MemberID", "            "] = member.ID;
        s["* ", "ACNT", "                "] = member.ACNT;
        s["* ", "ParentID", "            "] = member.ParentID;
        s["* ", "ParentACNT", "          "] = member.ParentACNT;
        s["* ", "BetAmount", "           "] = this.BetAmount;
        s["* ", "BetAmountAct", "        "] = this.BetAmountAct;
        s["* ", "Payout", "              "] = this.Payout;
        s.TestFieldNeeds();
        s.SetUser(sqltool.CreateUser);
        s.Values["ACTime"] = this.ACTime.Value.Date;
        return sqlcmd.ExecuteEx<GameLog_BetAmtDG_Row>(s.BuildEx2("insert into GameLog_BetAmtDG (", sqltool._Fields, @")
values (", sqltool._Values, @")
select * from GameLog_BetAmtDG nolock where sn=@@Identity"));
        //string sql1;
        //string sql2;
//        MemberGame membergame = MemberGame.GetInstance(this.GameID);
//        if (membergame == null)
//        {
//            //sql = s.BuildEx("insert into GameLog_BetAmtDG (MemberID,ParentID,ParentACNT", sqltool._Fields, @") select a.ID,c.ID,c.ACNT,0,", sqltool._Values, @" from Member a with(nolock) left join Agent c with(nolock) on a.ParentID=c.ID where a.ACNT={ACNT}");
//            sql1 = null;
//            sql2 = "a";
//        }
//        else
//        {
//            s.Values["TableName"] = (StringEx.sql_str)membergame.TableName;
//            //sql = s.BuildEx("insert into GameLog_BetAmtDG (MemberID,ParentID,ParentACNT", sqltool._Fields, @") select a.ID,c.ID,c.ACNT,0,", sqltool._Values, @" from Member a with(nolock) left join {TableName} b with(nolock) on a.ID=b.MemberID left join Agent c with(nolock) on a.ParentID=c.ID where b.ACNT={ACNT}");
//            sql1 = "left join {TableName} b with(nolock) on a.ID=b.MemberID";
//            sql2 = "b";
//        }
//        string sqlstr = s.BuildEx("insert into GameLog_BetAmtDG (MemberID,ACNT,ParentID,ParentACNT,", sqltool._Fields, @")
//select a.ID,a.ACNT,c.ID,c.ACNT,", sqltool._Values, @" from Member a with(nolock) ", sql1, " left join Agent c with(nolock) on a.ParentID=c.ID where ", sql2, @".ACNT={ACNT} and a.CorpID={CorpID}
//select * from GameLog_BetAmtDG nolock where sn=@@Identity");
//        return sqlcmd.ExecuteEx<GameLog_BetAmtDG_Row>(sqlstr);
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtDG_Update : GameLog_BetAmtDG_RowCommand, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.log_betamtdg, Permissions.Flag.Write)]
    static GameLog_BetAmtDG_Row update(GameLog_BetAmtDG_Update command, string json_s, params object[] args)
    {
        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //{
        //    MemberTranRow row = sqlcmd.GetRowEx<MemberTranRow>(RowErrorCode.MemberTranNotFound, "select * from MemberTran1 nolock where ID='{0}'", this.ID);
        //    sqltool s = new sqltool();
        //    object a_TranTime = this.a_TranTime; if (this.a_TranTime_GetDate) a_TranTime = (StringEx.sql_str)"getdate()";
        //    object b_TranTime = this.b_TranTime; if (this.b_TranTime_GetDate) b_TranTime = (StringEx.sql_str)"getdate()";
        //    if (VerifyMemberID)
        //        if (row.MemberID != (HttpContext.Current.User as User).ID)
        //            return row;

        //    s[" ", "Amount2", "     ", row.Amount2, "     "] = this.Amount2;
        //    s["N", "a_BankName", "  ", row.a_BankName, "  "] = this.a_BankName;
        //    s[" ", "a_CardID", "    ", row.a_CardID, "    "] = this.a_CardID;
        //    s["N", "a_Name", "      ", row.a_Name, "      "] = this.a_Name;
        //    s[" ", "a_TranTime", "  ", row.a_TranTime, "  "] = a_TranTime;
        //    s[" ", "a_TranSerial", "", row.a_TranSerial, ""] = this.a_TranSerial;
        //    s["N", "a_TranMemo", "  ", row.a_TranMemo, "  "] = this.a_TranMemo;
        //    s["N", "b_BankName", "  ", row.b_BankName, "  "] = this.b_BankName;
        //    s[" ", "b_CardID", "    ", row.b_CardID, "    "] = this.b_CardID;
        //    s["N", "b_Name", "      ", row.b_Name, "      "] = this.b_Name;
        //    s[" ", "b_TranTime", "  ", row.b_TranTime, "  "] = b_TranTime;
        //    s[" ", "b_TranSerial", "", row.b_TranSerial, ""] = this.b_TranSerial;
        //    s["N", "b_TranMemo", "  ", row.b_TranMemo, "  "] = this.b_TranMemo;
        //    s["N", "Memo1", "       ", row.Memo1, "       "] = this.Memo1;
        //    s["N", "Memo2", "       ", row.Memo2, "       "] = this.Memo2;
        //    if (s.fields.Count > 0)
        //    {
        //        s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
        //        s.Values["ID"] = row.ID;
        //        string sqlstr = s.BuildEx("update MemberTran1 set ", sqltool._FieldValue, " where ID={ID} select * from MemberTran1 nolock where ID={ID}");
        //        row = sqlcmd.ExecuteEx<MemberTranRow>(sqlstr);
        //    }
        //    return op_exec(sqlcmd, row, json_s, args);
        //}
        return null;
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtDG_Delete : GameLog_BetAmtDG_RowCommand, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.log_betamtdg, Permissions.Flag.Write)]
    static GameLog_BetAmtDG_Row delete(GameLog_BetAmtDG_Delete command, string json_s, params object[] args)
    {
        if (command.sn.HasValue)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                int? createUser = sqlcmd.ExecuteScalar<int>("select CreateUser from GameLog_BetAmtDG nolock where sn={0}", command.sn ?? 0);
                if (createUser == 0)
                    throw new RowException(RowErrorCode.UnableToDeleteRow);
                return sqlcmd.ExecuteEx<GameLog_BetAmtDG_Row>("select * from GameLog_BetAmtDG nolock where sn={0} delete GameLog_BetAmtDG where sn={0}", command.sn.Value);
            }
        }
        return null;
    }
}