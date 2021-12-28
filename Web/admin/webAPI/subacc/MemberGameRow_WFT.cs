using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using extAPI.wft;
using web;
using System.Diagnostics;

namespace web
{
    public abstract class MemberGameRow_WFT : MemberGameRow
    {
        [DbImport, JsonProperty]
        public WFTGameType gametype;

        [DbImport, JsonProperty]
        public string agentid;
        [DbImport, JsonProperty]
        public string language;

        [DbImport("Currency"), JsonProperty]
        public string currencyid;
    }
    public abstract class MemberGameRowCommand_WFT : MemberGameRowCommand
    {
        [JsonProperty]
        public string agentid;
        [JsonProperty]
        public WFTGameType gametype;
        [JsonProperty]
        public string language;

        [JsonProperty]
        public string currencyid;
    }

    public abstract class MemberGame_WFT<T, TRow, TRowCommand> : MemberGame<T, TRow, TRowCommand>
        where T : MemberGame_WFT<T, TRow, TRowCommand>, new()
        where TRow : MemberGameRow_WFT, new()
        where TRowCommand : MemberGameRowCommand_WFT, new()
    {
        internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        protected override TRow UpdateRow(SqlCmd sqlcmd, TRowCommand command, string json_s, params object[] args)
        {
            throw new NotImplementedException();
        }

        //protected override decimal? OnGetBalance(SqlCmd sqlcmd, TRow row)
        //{
        //    throw new NotImplementedException();
        //}

        protected override TRow GetBalance(SqlCmd sqlcmd, TRow row)
        {
            throw new NotImplementedException();
        }

        public override GameID GameID
        {
            get { throw new NotImplementedException(); }
        }

        public override string TableName
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class MemberGame_WFT : MemberGame_WFT<MemberGame_WFT, MemberGame_WFT.Row, MemberGame_WFT.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow_WFT
        {
        }
        public class RowCommand : MemberGameRowCommand_WFT
        {
            protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_WFT.Instance; } }
        }

        public override GameID GameID { get { return BU.GameID.WFT; } }
        public override string TableName { get { return "Member_003"; } }
    }

    public class MemberGame_WFT_SPORTS : MemberGame_WFT<MemberGame_WFT_SPORTS, MemberGame_WFT_SPORTS.Row, MemberGame_WFT_SPORTS.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow_WFT
        {
        }
        public class RowCommand : MemberGameRowCommand_WFT
        {
            protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_WFT_SPORTS.Instance; } }
        }

        public override GameID GameID { get { return BU.GameID.WFT_SPORTS; } }
        public override string TableName { get { return "Member_008"; } }
    }

    //internal class MemberGame_WFT : MemberGame<MemberGame_WFT, MemberGameRow_WFT, MemberGameRowCommand_WFT>
    //{
    //    public override GameID GameID { get { return BU.GameID.WFT; } }
    //    public override string TableName { get { return "Member_003"; } }

    //    internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //internal class MemberGame_WFT_SPORTS : MemberGame<MemberGame_WFT_SPORTS, MemberGameRow_WFT, MemberGameRowCommand_WFT>
    //{
    //    public override GameID GameID { get { return BU.GameID.WFT_SPORTS; } }
    //    public override string TableName { get { return "Member_008"; } }

    //    internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

        //internal MemberGameRowCommand_WFT(GameID gameid, string tableName) : base(gameid, tableName) { }

        //internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

        //internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override MemberGameRow_WFT UpdateRow(SqlCmd sqlcmd, string json_s, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

    //[DebuggerStepThrough]
    //public class MemberGameRowCommand_WFT : MemberGameRowCommand_WFT<MemberGameRowCommand_WFT>
    //{
    //    public MemberGameRowCommand_WFT() : base(GameID.WFT, "Member_003") { }
    //}
    //[DebuggerStepThrough]
    //public class MemberGameRowCommand_WFT_SPORTS : MemberGameRowCommand_WFT<MemberGameRowCommand_WFT_SPORTS>
    //{
    //    public MemberGameRowCommand_WFT_SPORTS() : base(GameID.WFT_SPORTS, "Member_008") { }
    //}



//    public abstract class _MemberGameRowCommand_WFT<TMemberGameRowCommand_WFT> : MemberGameRowCommand<TMemberGameRowCommand_WFT, MemberGameRow_WFT>, IGameAPI
//        where TMemberGameRowCommand_WFT : _MemberGameRowCommand_WFT<TMemberGameRowCommand_WFT>, new()
//    {
//        internal _MemberGameRowCommand_WFT(GameID gameid, string tableName) : base(gameid, tableName) { }


//        internal override MemberGameRow_WFT OnUpdate2(SqlCmd _sqlcmd, string json_s, params object[] args)
//        {
//            SqlCmd sqlcmd;
//            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, _sqlcmd))
//            {
//                if (!this.MemberID.HasValue)
//                    throw new RowException(RowErrorCode.FieldNeeds, "MemberID");
//                MemberGameRow_WFT row = this.GetRow2(sqlcmd, this.MemberID.Value);
//                if (row == null)
//                {
//                    MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", this.MemberID);
//                    extAPI.hg.api hgapi = extAPI.hg.api.GetInstance(member.CorpID);
//                    CorpRow corp = new CorpRow() { ID = member.CorpID };
//                    sqltool s = new sqltool();
//                    s["*", "MemberID", "   "] = member.ID;
//                    s["*", "GameID", "     "] = this.GameID;
//                    s["*", "Locked", "     "] = (text.ValidAsLocked * this.Locked) ?? 0;
//                    s["*", "Balance", "    "] = this.Balance.ToDecimal() ?? 0;
//                    s["*", "ACNT", "       "] = (text.ValidAsACNT * this.ACNT) ?? (hgapi.prefix ?? "").Trim() + member.ACNT;
//                    //s[" ", "pwd", "        "] = text.ValidAsString * this.Password;
//                    s[" ", "gametype", "       "] = (int)this.gametype;
//                    s["*", "language", "  "] = language;
//                    s["*", "Currency", " "] = this.currencyid ?? member.Currency.ToString();
//                    s[" ", "agentid", "    "] = this.agentid;
//                    s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
//                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
//                    string sql = s.BuildEx("insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, @")
//                    select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
//                    row = sqlcmd.ExecuteEx<MemberGameRow_WFT>(sql);
//                }
//                else
//                {
//                    if (this.Balance == "*")
//                        this.Balance = this.GetBalance(sqlcmd, row);
//                    sqltool s = new sqltool();
//                    s[" ", "Locked", "     ", row.Locked, "     "] = text.ValidAsLocked * this.Locked;
//                    s[" ", "Balance", "    ", row.Balance, "    "] = this.Balance.ToDecimal();
//                    //s[" ", "ACNT", "       ", row.ACNT, "       "] = text.ValidAsString * this.ACNT;
//                    //s[" ", "gametype", "   ", row.gametype, "   "] = (int)this.gametype;
//                    //s[" ", "language", "   ", row.language, "   "] = this.language;
//                    //s[" ", "Currency", " ", row.Currency, "   "] = this.Currency;
//                    //s[" ", "agentid", "    ", row.agentid, "    "] = this.agentid;
//                    if (s.fields.Count > 0)
//                    {
//                        s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                        s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
//                        s.Values["GameID"] = this.GameID;
//                        s.Values["MemberID"] = row.MemberID;
//                        string sqlstr = s.BuildEx("update {MemberTable} set ", sqltool._FieldValue, " where GameID={GameID} and MemberID={MemberID} select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
//                        row = sqlcmd.ExecuteEx<MemberGameRow_WFT>(sqlstr);
//                    }
//                }
//                return row;
//            }
//        }

//        internal string GetBalance(SqlCmd sqlcmd, MemberGameRow_WFT row)
//        {
//            if (row != null)
//            {
//                Dictionary<string, string> retDic = extAPI.wft.wftapi.CheckBalance(row.ACNT, row.gametype);

//                if (retDic != null && retDic.Count > 0)
//                {
//                    if (retDic["errcode"] != "0")
//                    {
//                        return row.Balance.ToString();
//                    }
//                    else
//                    {
//                        return retDic["result"];
//                    }
//                }
//                else
//                {
//                    return row.Balance.ToString();
//                }
//            }
//            else
//            {
//                return row.Balance.ToString();
//            }
//        }

//        public MemberGameRow_WFT Register(SqlCmd sqlcmd, MemberGameRow_WFT row)
//        {
//            if (row == null)
//                row = (MemberGameRow_WFT)this.OnUpdate(sqlcmd, "");
//            Dictionary<string, string> retDic = extAPI.wft.wftapi.Regist(row.ACNT, row.currencyid, row.gametype);
//            if (retDic != null && retDic.Count > 0)
//            {
//                if (retDic["errcode"] != "0")
//                {
//                    return null;
//                }
//                else
//                {
//                    return row;
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }
//        internal override bool OnGameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran)
//        {
//            MemberGameRow_WFT row = this.GetRow2(sqlcmd, tran.MemberID.Value);
//            if (row == null)
//                row = (MemberGameRow_WFT)(new TMemberGameRowCommand_WFT() { MemberID = tran.MemberID }).OnUpdate(sqlcmd, "");

//            WinningInfo winInfo = new WinningInfo();
//            winInfo.Serial = tran.SerialNumber;
//            winInfo.Amount = (float)(-tran.Amount.Value);
//            winInfo.UserName = row.ACNT;

//            try
//            {
//                Dictionary<string, string> retDic = wftapi.Deposit(winInfo, row.gametype);
//                if (retDic != null && retDic.Count > 0)
//                {
//                    if (retDic["errcode"] != "0")
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        return true;
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                log.message("向WFT存款出错，消息：", e.Message);
//            }
//            return false;
//        }
//        internal override bool OnGameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran)
//        {
//            MemberGameRow_WFT row = this.GetRow2(sqlcmd, tran.MemberID.Value);
//            if (row == null)
//                row = (MemberGameRow_WFT)(new TMemberGameRowCommand_WFT() { MemberID = tran.MemberID }).OnUpdate(sqlcmd, "");

//            WinningInfo winInfo = new WinningInfo();
//            winInfo.Serial = tran.SerialNumber;
//            winInfo.Amount = (float)tran.Amount1.Value;
//            winInfo.UserName = row.ACNT;

//            try
//            {
//                Dictionary<string, string> retDic = wftapi.Withdraw(winInfo, row.gametype);
//                if (retDic != null && retDic.Count > 0)
//                {
//                    if (retDic["errcode"] != "0")
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        return true;
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                log.message("从WFT取款出错，消息：", e.Message);
//            }
//            return false;
//        }
//    }
}