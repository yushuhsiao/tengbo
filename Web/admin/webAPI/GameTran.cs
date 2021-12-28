using BU;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GameTranRow : TranRow
    {
        [JsonProperty]
        public MemberGameRow MemberGameRow;
        public Exception api_Exception;
        public string api_Status;
        public string api_Message;
    }

    public partial class GameTranRowCommand : TranRowCommand<GameTranRow>
    {
        protected override string TableName { get { return "GameTran"; } }

//        public override GameTranRow Insert(string json_s, params object[] args)
//        {
//            if (this.Amount1 <= 0) this.Amount1 = null;
//            sqltool s = new sqltool();
//            if ((s.Values["prefix"] = this.LogType.GetPrefix(BU.LogType.GameDeposit, BU.LogType.GameWithdrawal)) == null)
//                this.LogType = null;
//            string _f1 = MemberID.HasValue ? " " : "*";
//            s["*", "LogType", "     "] = this.LogType;
//            s["*", "GameID", "      "] = this.GameID;
//            s[_f1, "CorpID", "      "] = this.CorpID;
//            s["*", "MemberID", "    "] = (StringEx.sql_str)"a.ID";
//            s[_f1, "MemberACNT", "  "] = this.MemberACNT *= text.ValidAsACNT;
//            s["*", "AgentID", "     "] = (StringEx.sql_str)"b.ID";
//            s["*", "AgentACNT", "   "] = (StringEx.sql_str)"b.ACNT";
//            s["*", "Amount1", "     "] = (int?)this.Amount1;
//            s[" ", "Amount2", "     "] = 0;
//            s[" ", "State", "       "] = TranState.Initial;
//            s[" ", "CurrencyA", "   "] = (StringEx.sql_str)"a.Currency";
//            s[" ", "CurrencyB", "   "] = (StringEx.sql_str)"a.Currency";
//            s[" ", "CurrencyX", "   "] = 1;
//            s[" ", "SerialNumber", ""] = (StringEx.sql_str)"@SerialNumber";
//            s[" ", "RequestIP", "   "] = HttpContext.Current.RequestIP();
//            s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
//            s.TestFieldNeeds();
//            s["", "CorpID", ""] = (StringEx.sql_str)"a.CorpID";
//            s["", "MemberACNT", ""] = (StringEx.sql_str)"a.ACNT";
//            SqlCmd sqlcmd;
//            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
//            {
//                try
//                {
//                    // 前置檢查
//                    GameRow game = sqlcmd.GetRowEx<GameRow>(RowErrorCode.GameNotFound, "select ID, Locked from Game nolock where ID={0}", (int?)this.GameID);
//                    if (game.Locked == GameLocked.Locked)
//                        throw new RowException(RowErrorCode.GameDisabled);
//                    StringBuilder m_sql = new StringBuilder("select ID, Locked from Member nolock where ");
//                    if (this.MemberID.HasValue)
//                        m_sql.AppendFormat("ID={0}", this.MemberID);
//                    else
//                        m_sql.AppendFormat("CorpID={0} and ACNT='{1}'", this.CorpID, this.MemberACNT);
//                    MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, m_sql.ToString());
//                    if (member.Locked == BU.Locked.Locked)
//                        throw new RowException(RowErrorCode.MemberLocked);
//                    //MemberGameRow member_sub = MemberGameRowCommand.GetInstance(game.ID.Value).GetRow(sqlcmd, member.ID.Value);
//                    //Locked s_locked;
//                    //if (member_sub == null)
//                    //    s_locked = Locked.Active;
//                    //else
//                    //    s_locked = member_sub.Locked ?? Locked.Active;
//                    //if (s_locked == Locked.Locked)
//                    //    throw new RowException(RowErrorCode.MemberSubAccLocked);
//                    s.Values["MemberID_"] = member.ID;

//                    // 建立需求單
//                    string sqlstr = s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
//insert into GameTran1 (ID, ", sqltool._Fields, @")
//select @ID,", sqltool._Values, @" from Member a with(nolock) left join Agent b with(nolock) on a.AgentID=b.ID where a.ID={MemberID_}
//select * from GameTran1 nolock where ID=@ID");
//                    GameTranRow row = sqlcmd.ExecuteEx<GameTranRow>(sqlstr);
//                    this.ID = row.ID;
//                    return this.processGameTran(sqlcmd, this, row, json_s, args);
//                    //if (MemberGameRowCommand.GetInstance(game.ID.Value) is IGameAPI)
//                    //    this.op_Accept = this.op_Finish = 1;
//                    //return op_exec(sqlcmd, row, json_s, args);
//                }
//                catch (Exception ex)
//                {
//                    log.error_msg(ex);
//                    throw ex;
//                }
//            }
//        }

        //public override GameTranRow Update(string json_s, params object[] args)
        //{
        //    try
        //    {
        //        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //        {
        //            GameTranRow row = sqlcmd.GetRowEx<GameTranRow>(RowErrorCode.GameTranNotFound, "select * from GameTran1 nolock where ID='{0}'", this.ID);
        //            return this.processGameTran(sqlcmd, this, row, json_s, args);
        //            //return op_exec(sqlcmd, row, json_s, args);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.error_msg(ex);
        //        throw ex;
        //    }
        //}

        //protected override LogType? LogType_Rollback
        //{
        //    get { return BU.LogType.GameDepositRollback; }
        //}

        //GameTranRow op_exec(SqlCmd sqlcmd, GameTranRow row, string json_s, params object[] args)
        //{
        //    row.ModifyUser = (HttpContext.Current.User as User).ID;
        //    MemberGameRowCommand proc = MemberGameRowCommand.GetInstance(row.GameID.Value);
        //    try
        //    {
        //        if (row.LogType == BU.LogType.GameDeposit)
        //        {
        //            if (this.op_Accept == 1) this.TranOut(sqlcmd, row);
        //            if (this.op_Finish == 1)
        //            {
        //                if (proc is IGameAPI)
        //                    this.op_Finish = proc.OnGameDeposit(sqlcmd, this, row) ? 1 : 0;
        //                if (this.op_Finish == 1)
        //                    this.TranOut_confirm(sqlcmd, row);
        //                this.op_Delete = 1;
        //            }
        //            if (this.op_Delete == 1) this.TranOut_delete(sqlcmd, row);
        //        }
        //        else if (row.LogType==BU.LogType.GameWithdrawal)
        //        {
        //            if (this.op_Finish == 1)
        //            {
        //                if (proc is IGameAPI)
        //                    this.op_Finish = proc.OnGameWithdrawal(sqlcmd, this, row) ? 1 : 0;
        //                if (this.op_Finish == 1)
        //                    this.TranIn(sqlcmd, row);
        //                this.op_Delete = 1;
        //            }
        //            if (this.op_Delete == 1) this.TranIn_delete(sqlcmd, row);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.error_msg(ex);
        //    }
        //    return row;
        //}

        GameTranRow processGameTran(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            //if (command.op_Delete != 1)
            //{
            tran.ModifyUser = _SystemUser.GetUser((HttpContext.Current.User as User).ID);
            try
            {
                if (tran.LogType == BU.LogType.GameDeposit)
                    return MemberGame.GetInstance(tran.GameID).GameDeposit(sqlcmd, command, tran, json_s, args);
                else if (tran.LogType == BU.LogType.GameWithdrawal)
                    return MemberGame.GetInstance(tran.GameID).GameWithdrawal(sqlcmd, command, tran, json_s, args);
            }
            catch (Exception ex)
            {
                log.error_msg(ex);
                tran.api_Exception = ex;
            }
            //}
            return tran;
        }
    }
    //partial class MemberGameRowCommand
    //{
    //    internal abstract GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args);

    //    internal abstract GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args);
    //}
    //partial class noapi_MemberGameRowCommand<TRow, TRowCommand> : MemberGameRowCommand<TRow, TRowCommand>
    //    where TRow : MemberGameRow, new()
    //    where TRowCommand : noapi_MemberGameRowCommand<TRow, TRowCommand>
    //{
    //    internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
    //    {
    //        if (command.op_Accept == 1) command.TranOut(sqlcmd, tran);
    //        if (command.op_Finish == 1)
    //        {
    //            command.TranOut_confirm(sqlcmd, tran);
    //            command.op_Delete = 1;
    //        }
    //        if (command.op_Delete == 1) command.TranOut_delete(sqlcmd, tran);
    //        return tran;
    //    }

    //    internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
    //    {
    //        if (command.op_Finish == 1)
    //        {
    //            command.TranIn(sqlcmd, tran);
    //            command.op_Delete = 1;
    //        }
    //        if (command.op_Delete == 1) command.TranIn_delete(sqlcmd, tran);
    //        return tran;
    //    }
    //}
    //public abstract partial class MemberGameRowCommand<TRow, TRowCommand> : MemberGameRowCommand
    //    where TRow : MemberGameRow, new()
    //    where TRowCommand : MemberGameRowCommand<TRow, TRowCommand>
    //{
    //}
    //partial class MemberGameRowCommand<TRowCommand, TRow> : MemberGameRowCommand
    //    where TRowCommand : MemberGameRowCommand<TRowCommand, TRow>, new()
    //    where TRow : MemberGameRow, new()
    //{
    //    public virtual decimal? GetBalance(SqlCmd sqlcmd, int? corpID, int? memberID)
    //    {
    //        return null;
    //    }
    //}
}
