using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using Tools;

namespace web
{
    partial class tran
    {
        public static partial class Game
        {
            [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
            public class GameRowData : RowData
            {
                [DbImport, JsonProperty]
                public GameID GameID;
                [DbImport, JsonProperty]
                public string GameACNT;
                [JsonProperty]
                public game.UserGameRow UserGameRow;
            }

            public abstract class GameRowCommand<TRowCommand> : tran.RowCommand<GameRowData, TRowCommand> where TRowCommand : GameRowCommand<TRowCommand>, new()
            {
                [DbImport, JsonProperty]
                public GameID? GameID;

                protected internal override GameRowData _tranrow
                {
                    get { return base._tranrow; }
                    set
                    {
                        if (value != null)
                            this.GameID = value.GameID;
                        base._tranrow = value;
                    }
                }

                internal game.UserGameRow _usergame;

                protected game.IUserGameRowCommand get_proc()
                {
                    game.IUserGameRowCommand proc;
                    proc = game.GetUserGameRowCommand(this.UserType, this.GameID, null, false);
                    if (proc != null) return proc;
                    if ((this.op_Insert != true) && this.ID.HasValue)
                    {
                        _tranrow = GetTranRow(RowErrorCode.TranNotFound, true);
                        proc = game.GetUserGameRowCommand(_tranrow.UserType, _tranrow.GameID, null, false);
                        if (proc != null) return proc;
                    }
                    throw new RowException(RowErrorCode.InvalidGameID);
                }

                protected internal void insert()
                {
                    if (_usergame == null)
                        throw new RowException(RowErrorCode.GameAccountNotFound);
                    if ((_usergame.Locked == Locked.Locked) && (this.LogType != BU.LogType.GameWithdrawal))
                        throw new RowException(RowErrorCode.GameAccountLocked);
                    sqltool s = new sqltool();
                    s["* ", "LogType", "            "] = this.LogType;
                    s["* ", "GameID", "             "] = this.GameID;
                    s["* ", "CorpID", "             "] = _user.CorpID;
                    s["* ", "ParentID", "           "] = _user.ParentID;
                    s["* ", "ParentACNT", "         "] = _user.ParentACNT;
                    s["* ", "UserType", "           "] = _user.UserType;
                    s["* ", "UserID", "             "] = _user.ID;
                    s["* ", "UserACNT", "           "] = _user.ACNT;
                    s["  ", "GameACNT", "           "] = _usergame.ACNT;
                    s["*N", "UserName", "           "] = _user.Name;
                    s["  ", "State", "              "] = TranState.Initial;
                    s["* ", "Amount", "             "] = this.Amount.amount_valid();
                    s["* ", "CurrencyA", "          "] = _user.Currency;
                    s["* ", "CurrencyB", "          "] = _usergame.Currency;
                    s["* ", "CurrencyX", "          "] = 1;
                    s["  ", "RequestIP", "          "] = HttpContext.Current.RequestIP();
                    s[" N", "Memo1", "              "] = this.Memo1 * text.ValidAsString;
                    s[" N", "Memo2", "              "] = this.Memo2 * text.ValidAsString;
                    s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
                    s.TestFieldNeeds();
                    s.values["table_tran"] = this.TranTable1;
                    s.values["prefix"] = this.prefix;
                    GameRowData tmp = sqlcmd.ToObject<GameRowData>(true, s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
insert into {table_tran} (ID,SerialNumber,", sqltool._Fields, @")
values (@ID,@SerialNumber,", sqltool._Values, @")
select * from {table_tran} nolock where ID=@ID"));
                    if (tmp == null) throw new RowException(RowErrorCode.NoResult);
                    _tranrow = tmp;
                }

                protected internal void update(string memo1, string memo2)
                {
                    _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    sqltool s = new sqltool();
                    s[" N", "Memo1", "   ", _tranrow.Memo1, "  "] = memo1 * text.ValidAsString;
                    s[" N", "Memo2", "   ", _tranrow.Memo2, "  "] = memo2 * text.ValidAsString;
                    if (s.fields.Count == 0) return;
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.values["ID"] = _tranrow.ID;
                    s.values["table_tran"] = this.TranTable1;
                    GameRowData tmp = sqlcmd.ToObject<GameRowData>(true, s.BuildEx2("update {table_tran} set ", sqltool._FieldValue, @" where ID={ID} --and FinishTime is null
select * from {table_tran} nolock where ID={ID}"));
                    if (tmp == null) throw new RowException(RowErrorCode.NoResult);
                    _tranrow = tmp;
                }

                protected internal void update()
                {
                    update(this.Memo1, this.Memo2);
                }
            }

            [tran("tranGame1", "tranGame2", BU.LogType.GameDeposit)]
            public class GameDepositRowCommand : GameRowCommand<GameDepositRowCommand>
            {
                protected override void execute(string json_s, params object[] args)
                {
                    base.get_proc().execute(this);
                    if (_tranrow != null)
                        _tranrow.UserGameRow = this._usergame;
                }

                protected internal void accept()
                {
                    TranLogRow log1; _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        if (setAccepted())
                        {
                            UpdateUserBalance(false);
                            GameRowData tmp = GetTranRow(RowErrorCode.NoResult, false);
                            log1 = this.WriteTranLog(tmp, false, log_GameID = tmp.GameID);
                            sqlcmd.Commit();
                            _tranrow = tmp;
                        }
                        else sqlcmd.Rollback();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }

                protected internal void finish()
                {
                    _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        if (setTransferred_out())
                        {
                            GameRowData tmp = GetTranRow(RowErrorCode.NoResult, false);
                            sqlcmd.Commit();
                            _tranrow = tmp;
                            this.op_Delete = true;
                        }
                        else sqlcmd.Rollback();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }

                protected internal void delete()
                {
                    TranLogRow log1; _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        bool _state1 = setRejected(), _state2 = setFailed();
                        GameRowData tmp;
                        if (_state2)
                        {
                            UpdateUserBalance(true);
                            tmp = GetTranRow(RowErrorCode.NoResult, false);
                            log1 = this.WriteTranLog(tmp, false, log_GameID = tmp.GameID, log_LogType = BU.LogType.GameDepositRollback);
                        }
                        else tmp = GetTranRow(RowErrorCode.NoResult, false);
                        DeleteTranRow();
                        sqlcmd.Commit();
                        _tranrow = tmp;
                        this.IsDeleted = true;
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }
            }

            [tran("tranGame1", "tranGame2", BU.LogType.GameWithdrawal)]
            public class GameWithdrawalRowCommand : GameRowCommand<GameWithdrawalRowCommand>
            {
                protected override void execute(string json_s, params object[] args)
                {
                    base.get_proc().execute(this);
                    if (_tranrow != null)
                        _tranrow.UserGameRow = this._usergame;
                }

                protected internal void finish()
                {
                    TranLogRow log1; _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        if (setTransferred_in())
                        {
                            UpdateUserBalance(true);
                            GameRowData tmp = GetTranRow(RowErrorCode.NoResult, false);
                            log1 = this.WriteTranLog(tmp, false, log_GameID = tmp.GameID);
                            sqlcmd.Commit();
                            _tranrow = tmp;
                            this.op_Delete = true;
                        }
                        else sqlcmd.Rollback();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }

                protected internal void delete()
                {
                    _tranrow = _tranrow ?? GetTranRow(RowErrorCode.TranNotFound, true);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        setRejected();
                        GameRowData tmp = GetTranRow(RowErrorCode.NoResult, false);
                        DeleteTranRow();
                        sqlcmd.Commit();
                        _tranrow = tmp;
                        this.IsDeleted = true;
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}