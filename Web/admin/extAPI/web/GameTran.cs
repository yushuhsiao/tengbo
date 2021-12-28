using BU;
using BU.data;
using extAPI.hg;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using web;
using web.protocol;

namespace web.protocol
{
    public abstract partial class GameTranProtocol : TranProtocol<GameTranRow>
    {
        protected override string _tableName { get { return "GameTran1"; } }

        public override jgrid.RowResponse update(string json_s, params object[] args)
        {
            try
            {
                if (!this.ID.HasValue)
                    return new jgrid.RowResponse(RowErrorCode.FieldNeeds, "ID");
                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                {
                    GameTranRow row = sqlcmd.ToObject<GameTranRow>("select * from GameTran1 nolock where ID='{0}'", this.ID);
                    RowException.TestNull(row);
                    return this.process_tran(sqlcmd, row, null);
                }
            }
            catch (Exception ex)
            {
                log.error_msg(ex);
                return jgrid.RowResponse.Error(ex);
            }
        }

        public override jgrid.RowResponse insert(string json_s, params object[] args)
        {
            if (this.Amount1 <= 0) this.Amount1 = null;
            sqltool s = new sqltool();
            if ((s.Values["prefix"] = this.LogType.GetPrefix(BU.LogType.GameDeposit, BU.LogType.GameWithdrawal)) == null)
                this.LogType = null;
            s["*", "LogType", "     "] = this.LogType;
            s["*", "GameID", "      "] = this.GameID;
            s["*", "MemberID", "    "] = (StringEx.sql_str)"ID";
            s["*", "CorpID", "      "] = this.CorpID;
            s["*", "MemberACNT", "  "] = this.MemberACNT *= text.ValidAsACNT;
            s["*", "Amount1", "     "] = this.Amount1;
            s[" ", "Amount2", "     "] = 0;
            s[" ", "State", "       "] = TranState.Initial;
            s[" ", "CurrencyA", "   "] = (StringEx.sql_str)"Currency";
            s[" ", "CurrencyB", "   "] = (StringEx.sql_str)"Currency";
            s[" ", "CurrencyX", "   "] = 1;
            s[" ", "SerialNumber", ""] = (StringEx.sql_str)"@SerialNumber";
            s[" ", "RequestIP", "   "] = HttpContext.Current.RequestIP();
            s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
            if (s.needs != null)
                return jgrid.RowResponse.FieldNeeds(s.needs);
            s.Values["CorpID"] = (StringEx.sql_str)"CorpID";
            s.Values["MemberACNT"] = (StringEx.sql_str)"ACNT";
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                try
                {
                    // 前置檢查
                    GameRow game = sqlcmd.ToObject<GameRow>("select ID, Locked from Game nolock where ID={0}", (int?)this.GameID);
                    RowException.TestNull(game, RowErrorCode.GameNotExist);
                    if (game.Locked == GameLocked.Locked)
                        return new jgrid.RowResponse(RowErrorCode.GameUnavailable);
                    MemberRow member = sqlcmd.ToObject<MemberRow>("select ID, Locked from Member nolock where CorpID={0} and ACNT='{1}'", this.CorpID, this.MemberACNT);
                    RowException.TestNull(game, RowErrorCode.MemberNotExist);
                    if (member.Locked == BU.Locked.Locked)
                        return new jgrid.RowResponse(RowErrorCode.GameUnavailable);
                    MemberSubAccRow subacc = MemberSubAccRow.CreateInstance(sqlcmd, game.ID.Value, member.ID.Value);
                    if (subacc == null)
                        return new jgrid.RowResponse(RowErrorCode.GameUnavailable);
                    if (subacc.Locked == BU.Locked.Locked)
                        return new jgrid.RowResponse(RowErrorCode.GameUnavailable);
                    s.Values["MemberID_"] = member.ID;

                    // 建立需求單
                    try
                    {
                        string sqlstr = s.SqlExport(s.Build(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
insert into GameTran1 (ID, ", sqltool._Fields, @")
select @ID,", sqltool._Values, @"n from Member nolock where ID={MemberID_}
select * from GameTran1 nolock where ID=@ID"));
                        GameTranRow row;
                        sqlcmd.BeginTransaction();
                        row = sqlcmd.ToObject<GameTranRow>(sqlstr);
                        RowException.TestNull(row);
                        sqlcmd.Commit();
                        if (subacc.hasAPI) this.accept = AcceptOrReject.Accept;
                        return this.process_tran(sqlcmd, row, subacc);
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    log.error_msg(ex);
                    return jgrid.RowResponse.Error(ex);
                }
            }
        }

        jgrid.RowResponse process_tran(SqlCmd sqlcmd, GameTranRow row, MemberSubAccRow subacc)
        {
            this._TranValidOp();
            try
            {
                if ((row.LogType == BU.LogType.GameDeposit) && (this.accept.HasValue || this.commit.HasValue))
                {
                    if (this.accept == AcceptOrReject.Accept)
                    {
                        this._TranOut_accept(sqlcmd, row);
                        subacc = subacc ?? MemberSubAccRow.CreateInstance(sqlcmd, row.GameID.Value, row.MemberID.Value);
                        if (subacc.hasAPI)
                            this.commit = subacc.GameDeposit(sqlcmd, this, row) ? CommitOrRollback.Commit : CommitOrRollback.Rollback;
                    }
                    this._TranOut(sqlcmd, row, BU.LogType.GameDepositRollback);
                }
                else if ((row.LogType == BU.LogType.GameWithdrawal) && this.commit.HasValue)
                {
                    if (this.commit.Value == CommitOrRollback.Commit)
                    {
                        subacc = subacc ?? MemberSubAccRow.CreateInstance(sqlcmd, row.GameID.Value, row.MemberID.Value);
                        if (subacc.hasAPI)
                            this.commit = subacc.GameWithdrawal(sqlcmd, this, row) ? CommitOrRollback.Commit : CommitOrRollback.Rollback;
                    }
                    this._TranIn(sqlcmd, row);
                }
            }
            catch (Exception ex)
            {
                log.error_msg(ex);
                jgrid.RowResponse res2 = jgrid.RowResponse.Error(ex);
            }
            return jgrid.RowResponse.Success(row);
        }

        protected override void _TranFinish(SqlCmd sqlcmd, GameTranRow row, string amount2)
        {
            sqlcmd.ExecuteNonQuery(@"
insert into GameTran2 (ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,Amount2,State,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP ,FinishTime,CreateUser, ModifyUser)
select                 ID,LogType,GameID,MemberID,CorpID,MemberACNT,Amount1,{3}    ,{1}  ,CurrencyA,CurrencyB,CurrencyX,SerialNumber,RequestIP ,getdate() ,CreateUser,{2}
from GameTran1 nolock where ID='{0}' delete GameTran1 where ID='{0}'", row.ID, (int)row.State.Value, row.ModifyUser, amount2);
        }
    }
}

namespace BU.data
{
    public abstract class MemberSubAccRow : MemberSubAccRowBase
    {
        public abstract string MemberTable { get; }
        internal abstract bool hasAPI { get; }
        public MemberSubAccRow(SqlCmd sqlcmd, GameType gameID, int memberID)
            : base(gameID, memberID)
        {
            this.ReadRow(sqlcmd, gameID, memberID);
            this.Locked = this.Locked ?? BU.Locked.Active;
        }

        public static MemberSubAccRow CreateInstance(SqlCmd sqlcmd, GameType gameID, int memberID)
        {
            MemberSubAccRow obj;
            switch (gameID)
            {
                case GameType.HG: obj = new MemberRow_HG(sqlcmd, memberID); break;
                case GameType.EA: obj = new MemberRow_EA(sqlcmd, memberID); break;
                case GameType.KG: obj = new MemberRow_KG(sqlcmd, memberID); break;
                case GameType.WFT: obj = new MemberRow_WFT(sqlcmd, memberID); break;
                case GameType.SUNBET: obj = new MemberRow_SUNBET(sqlcmd, memberID); break;
                default: throw new RowException(RowErrorCode.InvalidGameID);
            }
            return obj;
        }

        //public void ReadMemberRow(SqlCmd sqlcmd, int? memberID, int? corpID, string acnt)
        //{
        //    StringBuilder sql = new StringBuilder("select ID, CorpID, ACNT, MemberType, Locked, Currency from Member nolock where ");
        //    if (memberID.HasValue)
        //        sql.AppendFormat("ID={0}", memberID);
        //    else
        //        sql.AppendFormat("CorpID={0} and ACNT='{1}'", corpID, acnt);
        //    RowException.TestNull(this.member = sqlcmd.ToObject<MemberRow>(sql.ToString()), RowErrorCode.MemberNotExist);
        //}

        public void ReadRow(SqlCmd sqlcmd, GameType? gameID, int? memberID)
        {
            sqlcmd.FillObject(this, "select * from {0} nolock where GameID={1} and MemberID={2}", this.MemberTable, (int?)gameID, memberID);
        }

        public virtual bool CreateRow(SqlCmd sqlcmd) { return false; }

        internal abstract bool GameDeposit(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran);

        internal abstract bool GameWithdrawal(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran);
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberRow_HG : MemberSubAccRow
    {
        #region fields
        [DbImport, JsonProperty]
        public string username;
        [DbImport, JsonProperty]
        public string password;
        [DbImport, JsonProperty]
        public extAPI.hg.LoginMode? mode;
        [DbImport, JsonProperty]
        public string firstname;
        [DbImport, JsonProperty]
        public string lastname;
        [DbImport, JsonProperty]
        public string currencyid;
        [DbImport, JsonProperty]
        public int? agentid;
        [DbImport, JsonProperty]
        public string affiliateid;
        [DbImport, JsonProperty]
        public int? playerlevel;
        [DbImport, JsonProperty]
        public extAPI.hg.TestUser? testusr;
        #endregion

        public MemberRow_HG(SqlCmd sqlcmd, int memberID) : base(sqlcmd, GameType.HG, memberID) { }
        public override string MemberTable { get { return "Member_001"; } }
        internal override bool hasAPI { get { return true; } }

        //extAPI.hg.LoginMode? LoginMode
        //{
        //    get { return member.MemberType == BU.MemberType.Trial ? extAPI.hg.LoginMode.娛樂 : extAPI.hg.LoginMode.真正; }
        //}

        public override bool CreateRow(SqlCmd _sqlcmd)
        {
            SqlCmd sqlcmd;
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, _sqlcmd))
            {
                MemberRow member = sqlcmd.ToObject<MemberRow>("select * from Member nolock where ID={0}", this.MemberID);
                if (member == null)
                    throw new RowException(RowErrorCode.MemberNotExist);
                CorpRow corp = new CorpRow() { ID = member.CorpID };
                sqltool s = new sqltool();
                s[" ", "MemberID", "   "] = member.ID;
                s[" ", "GameID", "     "] = this.GameID;
                s[" ", "Locked", "     "] = 0;
                s[" ", "Balance", "    "] = 0;
                s[" ", "username", "   "] = extAPI.hg.api.prefix + member.ACNT;
                s[" ", "password", "   "] = (StringEx.sql_str)"null";
                s[" ", "mode", "       "] = (int)(member.MemberType == BU.MemberType.Trial ? extAPI.hg.LoginMode.娛樂 : extAPI.hg.LoginMode.真正);
                s[" ", "firstname", "  "] = member.ACNT;
                s[" ", "lastname", "   "] = corp.prefix;
                s[" ", "currencyid", " "] = member.Currency;
                s[" ", "agentid", "    "] = null;
                s[" ", "affiliateid", ""] = null;
                s[" ", "testusr", "    "] = null;
                s[" ", "playerlevel", ""] = null;
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                s.Values["MemberTable"] = (StringEx.sql_str)this.MemberTable;
                try
                {
                    sqlcmd.BeginTransaction();
                    string sql = s.SqlExport(s.Build("insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, @")
select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}"));
                    sqlcmd.FillObject(this, sql);
                    sqlcmd.Commit();
                    return true;
                }
                catch
                {
                    sqlcmd.Rollback();
                    return false;
                }
            }
        }

        public string ticket;

        public bool Register(SqlCmd sqlcmd)
        {
            if (!this.CreateTime.HasValue)
                if (!this.CreateRow(sqlcmd))
                    return false;
            hgResponse1 res1 = extAPI.hg.api.registration(this.username, this.password, this.mode.Value, this.firstname, this.lastname, this.currencyid, this.agentid, this.affiliateid, this.testusr, this.playerlevel);
            if (res1.status == StatusCode.SUCCESS)
            {
                this.ticket = res1.ticket;
                return true;
            }
            return false;
        }

        internal override bool GameDeposit(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            if (!this.CreateTime.HasValue)
                if (!this.CreateRow(sqlcmd))
                    return false;
            hgResponse1 res1 = extAPI.hg.api.deposit(this.username, this.mode.Value, this.currencyid, tran.Amount1.Value, tran.SerialNumber, null, null);
            if ((res1.status == StatusCode.ERR_DEP_USER) || (res1.status == StatusCode.ERR_DEP_LOAD_REQ))
            {
                if (Register(sqlcmd))
                    res1 = extAPI.hg.api.deposit(this.username, this.mode.Value, this.currencyid, tran.Amount1.Value, tran.SerialNumber, null, null);
                else
                    return false;
            }
            if (res1.status == StatusCode.SUCCESS)
            {
                hgResponse1 res2 = extAPI.hg.api.deposit_confirm(StatusCode.SUCCESS, res1.paymentid, res1.errdesc);
                return res2.status == StatusCode.SUCCESS;
            }
            return false;
        }

        internal override bool GameWithdrawal(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            if (!this.CreateTime.HasValue)
                if (!this.CreateRow(sqlcmd))
                    return false;
            hgResponse1 res1 = extAPI.hg.api.withdrawal(this.username, this.mode.Value, this.currencyid, tran.Amount1.Value, tran.SerialNumber);
            if (res1.status == StatusCode.SUCCESS)
            {
                hgResponse1 res2 = extAPI.hg.api.withdrawal_confirm(StatusCode.SUCCESS, res1.paymentid, res1.errdesc);
                return res2.status == StatusCode.SUCCESS;
            }
            return false;
        }

        //public override MemberSubAccRow sql_insert(SqlCmd sqlcmd, GameRow game, MemberRow member)
        //{
        //}
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberRow_EA : MemberSubAccRow
    {
        public MemberRow_EA(SqlCmd sqlcmd, int memberID) : base(sqlcmd, GameType.EA, memberID) { }
        public override string MemberTable { get { return "Member_002"; } }
        internal override bool hasAPI { get { return true; } }
        internal override bool GameDeposit(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            throw new NotImplementedException();
        }
        internal override bool GameWithdrawal(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            throw new NotImplementedException();
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberRow_WFT : MemberSubAccRow
    {
        public MemberRow_WFT(SqlCmd sqlcmd, int memberID) : base(sqlcmd, GameType.WFT, memberID) { }
        public override string MemberTable { get { return "Member_003"; } }
        internal override bool hasAPI { get { return true; } }
        internal override bool GameDeposit(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            throw new NotImplementedException();
        }
        internal override bool GameWithdrawal(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            throw new NotImplementedException();
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberRow_KG : MemberSubAccRow
    {
        public MemberRow_KG(SqlCmd sqlcmd, int memberID) : base(sqlcmd, GameType.KG, memberID) { }
        public override string MemberTable { get { return "Member_004"; } }
        internal override bool hasAPI { get { return true; } }
        internal override bool GameDeposit(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            return true;
        }
        internal override bool GameWithdrawal(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            return true;
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberRow_SUNBET : MemberSubAccRow
    {
        public MemberRow_SUNBET(SqlCmd sqlcmd, int memberID) : base(sqlcmd, GameType.SUNBET, memberID) { }
        public override string MemberTable { get { return "Member_005"; } }
        internal override bool hasAPI { get { return false; } }
        internal override bool GameDeposit(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            return true;
        }
        internal override bool GameWithdrawal(SqlCmd sqlcmd, GameTranProtocol command, GameTranRow tran)
        {
            return true;
        }
    }
}
