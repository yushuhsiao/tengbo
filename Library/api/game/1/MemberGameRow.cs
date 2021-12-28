using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using web;

namespace web
{
    // public abstract class MemberGame
    // {
    //     public static MemberGame GetInstance(GameID? gameID)
    //     {
    //         switch (gameID ?? 0)
    //         {
    //             //case GameID.HG: return MemberGameRowCommand_HG.Instance;
    //             //case GameID.EA: return MemberGameRowCommand_EA.Instance;
    //             //case GameID.WFT: return MemberGameRowCommand_WFT.Instance;
    //             //case GameID.WFT_SPORTS: return MemberGameRowCommand_WFT_SPORTS.Instance;
    //             //case GameID.KENO: return MemberGameRowCommand_KENO.Instance;
    //             //case GameID.KENO_SSC: return MemberGameRowCommand_KENO_SSC.Instance;
    //             //case GameID.SUNBET: return MemberGameRowCommand_SUNBET.Instance;
    //             //case GameID.AG: return MemberGameRowCommand_AG.Instance;
    //             //case GameID.BBIN: return MemberGameRowCommand_BBIN.Instance;
    //             //case GameID.CROWN_SPORTS: return MemberGameRowCommand_CROWN.Instance;
    //             default: return null;
    //         }
    //     }
    // }

    // public class memberGameRow
    // {
    //     [DbImport, JsonProperty]
    //     public GameID? GameID;
    //     [DbImport, JsonProperty]
    //     public int? MemberID;

    //     [DbImport]
    //     public int? CorpID;
    //     [DbImport]
    //     public string MemberACNT;

    //     [DbImport, JsonProperty]
    //     public virtual MemberGameLocked? Locked { get; set; }              // dbo.Member_???.Locked
    //     [DbImport, JsonProperty]
    //     public virtual decimal? Balance { get; set; }
    //     [DbImport, JsonProperty]
    //     public virtual string ACNT { get; set; }
    //     [DbImport("pwd"), JsonProperty]
    //     public virtual string Password { get; set; }
    //     [DbImport, JsonProperty]
    //     public virtual CurrencyCode? Currency { get; set; }
    //     [DbImport, JsonProperty]
    //     public virtual DateTime? CreateTime { get; set; }
    //     [DbImport, JsonProperty]
    //     public virtual int? CreateUser { get; set; }
    //     [DbImport, JsonProperty]
    //     public virtual DateTime? ModifyTime { get; set; }
    //     [DbImport, JsonProperty]
    //     public virtual int? ModifyUser { get; set; }
    // }
    // public class memberGameRowCommand
    // {
    //     [JsonProperty]
    //     internal GameID GameID;
    //     [JsonProperty]
    //     public virtual int? MemberID { get; set; }
    //     [JsonProperty]
    //     public virtual string Locked { get; set; }
    //     [JsonProperty]
    //     public virtual string Balance { get; set; }
    //     [JsonProperty]
    //     public virtual string Password { get; set; }
    //     [JsonProperty]
    //     public virtual string ACNT { get; set; }
    //     [JsonProperty]
    //     public virtual CurrencyCode? Currency { get; set; }
    //     [JsonProperty]
    //     public virtual string DepositAmount { get; set; }
    //     public GameTranRow DepositResult;
    //     [JsonProperty]
    //     public virtual string WithdrawalAmount { get; set; }
    //     public GameTranRow WithdrawalResult;
    //}

    // public class MemberGame<T, TRow, TRowCommand>
    //     where T : MemberGame<T, TRow, TRowCommand>, new()
    //     where TRow : MemberGame<T, TRow, TRowCommand>.Row, new()
    //     where TRowCommand : MemberGame<T, TRow, TRowCommand>.RowCommand, new()
    // {
    //     static readonly T instance = new T();

    //     public class Row : memberGameRow
    //     {
    //     }
    //     public class RowCommand : memberGameRowCommand
    //     {
    //     }
    // }

    // public class MemberGame_HG : MemberGame<MemberGame_HG, memberGameRow_HG, memberGameRowCommand_HG>
    // {
    // }
    // public class memberGameRow_HG : MemberGame_HG.Row
    // {
    // }
    // public class memberGameRowCommand_HG : MemberGame_HG.RowCommand
    // {
    // }

    // public class MemberGame_EA : MemberGame<MemberGame_EA, memberGameRow_EA, memberGameRowCommand_EA>
    // {
    // }
    // public class memberGameRow_EA : MemberGame_EA.Row
    // {
    // }
    // public class memberGameRowCommand_EA : MemberGame_EA.RowCommand
    // {
    // }

    // public class MemberGame_WFT<T, TRow, TRowCommand> : MemberGame<T, TRow, TRowCommand>
    //     where T : MemberGame_WFT<T, TRow, TRowCommand>, new()
    //     where TRow : MemberGame_WFT<T, TRow, TRowCommand>.Row, new()
    //     where TRowCommand : MemberGame_WFT<T, TRow, TRowCommand>.RowCommand, new()
    // {
    // }

    // public class MemberGame_WFT : MemberGame_WFT<MemberGame_WFT, memberGameRow_WFT, memberGameRowCommand_WFT>
    // {
    // }
    // public class memberGameRow_WFT : MemberGame_WFT.Row
    // {
    // }
    // public class memberGameRowCommand_WFT : MemberGame_WFT.RowCommand
    // {
    // }

    // public class MemberGame_WFT_SPORT : MemberGame_WFT<MemberGame_WFT_SPORT, memberGameRow_WFT_SPORT, memberGameRowCommand_WFT_SPORT>
    // {
    // }
    // public class memberGameRow_WFT_SPORT : MemberGame_WFT_SPORT.Row
    // {
    // }
    // public class memberGameRowCommand_WFT_SPORT : MemberGame_WFT_SPORT.RowCommand
    // {
    // }

    // public class MemberGame_KENO<T, TRow, TRowCommand> : MemberGame<T, TRow, TRowCommand>
    //     where T : MemberGame_KENO<T, TRow, TRowCommand>, new()
    //     where TRow : MemberGame_KENO<T, TRow, TRowCommand>.Row, new()
    //     where TRowCommand : MemberGame_KENO<T, TRow, TRowCommand>.RowCommand, new()
    // {
    // }

    // public class MemberGame_KENO : MemberGame_KENO<MemberGame_KENO, memberGameRow_KENO, memberGameRowCommand_KENO>
    // {
    // }
    // public class memberGameRow_KENO : MemberGame_KENO.Row
    // {
    // }
    // public class memberGameRowCommand_KENO : MemberGame_KENO.RowCommand
    // {
    // }

    // public class MemberGame_KENO_SSC : MemberGame_KENO<MemberGame_KENO_SSC, memberGameRow_KENO_SSC, memberGameRowCommand_KENO_SSC>
    // {
    // }
    // public class memberGameRow_KENO_SSC : MemberGame_KENO_SSC.Row
    // {
    // }
    // public class memberGameRowCommand_KENO_SSC : MemberGame_KENO_SSC.RowCommand
    // {
    // }

    // public class MemberGame_AG : MemberGame<MemberGame_AG, memberGameRow_AG, memberGameRowCommand_AG>
    // {
    // }
    // public class memberGameRow_AG : MemberGame_AG.Row
    // {
    // }
    // public class memberGameRowCommand_AG : MemberGame_AG.RowCommand
    // {
    // }


    // //public class MemberGame_KENO<T, TRow, TRowCommand> : MemberGame<T, TRow, TRowCommand>
    // //    where T : MemberGame_KENO<T, TRow, TRowCommand>, new()
    // //    where TRow : MemberGame_KENO<T, TRow, TRowCommand>.Row, new()
    // //    where TRowCommand : MemberGame_KENO<T, TRow, TRowCommand>.RowCommand, new()
    // //{
    // //}


    // public class MemberGame_SUNBET
    // {
    // }
    // public class MemberGame_BBIN
    // {
    // }
    // public class MemberGame_CROWN
    // {
    // }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class MemberGameRow
    {
        [DbImport, JsonProperty]
        public GameID? GameID;
        [DbImport, JsonProperty]
        public int? MemberID;

        [DbImport]
        public int? CorpID;

        public int? GetCorpID(SqlCmd sqlcmd)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                if (!this.CorpID.HasValue)
                    sqlcmd.FillObject(this, "select CorpID from Member nolock where ID={0}", this.MemberID);
                return this.CorpID;
            }
        }

        [DbImport, JsonProperty]
        public virtual Locked? Locked { get; set; }              // dbo.Member_???.Locked
        [DbImport, JsonProperty]
        public virtual decimal? Balance { get; set; }
        [DbImport, JsonProperty]
        public DateTime? GetBalanceTime;
        [DbImport, JsonProperty]
        public virtual string ACNT { get; set; }
        [DbImport("pwd"), JsonProperty]
        public virtual string Password { get; set; }
        [DbImport, JsonProperty]
        public virtual string Currency { get; set; }
        [DbImport, JsonProperty]
        public virtual DateTime? CreateTime { get; set; }
        [DbImport, JsonProperty]
        public virtual int? CreateUser { get; set; }
        [DbImport, JsonProperty]
        public virtual DateTime? ModifyTime { get; set; }
        [DbImport, JsonProperty]
        public virtual int? ModifyUser { get; set; }
    }

    public abstract partial class MemberGameRowCommand
    {
        //public static MemberGameRowCommand GetInstance(GameID? gameID)
        //{
        //    switch (gameID ?? 0)
        //    {
        //        case GameID.HG: return MemberGameRowCommand_HG.Instance;
        //        case GameID.EA: return MemberGameRowCommand_EA.Instance;
        //        case GameID.WFT: return MemberGameRowCommand_WFT.Instance;
        //        case GameID.WFT_SPORTS: return MemberGameRowCommand_WFT_SPORTS.Instance;
        //        case GameID.KENO: return MemberGameRowCommand_KENO.Instance;
        //        case GameID.KENO_SSC: return MemberGameRowCommand_KENO_SSC.Instance;
        //        case GameID.SUNBET: return MemberGameRowCommand_SUNBET.Instance;
        //        case GameID.AG: return MemberGameRowCommand_AG.Instance;
        //        case GameID.BBIN: return MemberGameRowCommand_BBIN.Instance;
        //        case GameID.CROWN_SPORTS: return MemberGameRowCommand_CROWN.Instance;
        //        default: return null;
        //    }
        //}

        //protected abstract MemberGame proc { get; }
        //protected abstract MemberGame proc { get; }

        protected abstract MemberGame proc { get; }

        [JsonProperty]
        public virtual int? MemberID { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual decimal? Balance { get; set; }
        [JsonProperty]
        public virtual string Password { get; set; }
        [JsonProperty]
        public virtual string ACNT { get; set; }
        [JsonProperty]
        public virtual CurrencyCode? Currency { get; set; }
        [JsonProperty]
        public virtual string DepositAmount { get; set; }
        //public GameTranRow DepositResult;
        [JsonProperty]
        public virtual string WithdrawalAmount { get; set; }
        //public GameTranRow WithdrawalResult;

        //public readonly string TableName;
        //public abstract Type RowType { get; }
        //internal MemberGameRowCommand(GameID gameid, string tableName) { this.GameID = gameid; this.TableName = tableName; }

        //public abstract MemberGameRow SelectRow(SqlCmd sqlcmd, int memberID);
        //[DebuggerStepThrough]
        //public MemberGameRow Update(string json_s, params object[] args) { return this.proc.Update(this, json_s, args); }
    }

    public abstract class MemberGame
    {
        public abstract GameID GameID { get; }
        public abstract string TableName { get; }
        public virtual bool HasAPI { get { return true; } }

        static MemberGame[] instances;
        public static MemberGame GetInstance(GameID? gameID)
        {
            lock (typeof(MemberGame))
            {
                instances = instances ?? new MemberGame[] {
                    MemberGame_HG.Instance,
                    //MemberGame_EA.Instance,
                    //MemberGame_WFT.Instance,
                    //MemberGame_WFT_SPORTS.Instance,
                    //MemberGame_KENO.Instance,
                    //MemberGame_KENO_SSC.Instance,
                    MemberGame_SUNBET.Instance,
                    MemberGame_AG.Instance,
                    MemberGame_BBIN.Instance,
                    MemberGame_CROWN.Instance,
                    MemberGame_AG_AG.Instance,
                    MemberGame_AG_AGIN.Instance,
                    MemberGame_AG_DSP.Instance,
                    MemberGame_SALON.Instance,
                    MemberGame_EXTRA.Instance,
                };
            }

            if (gameID.HasValue)
            {
                for (int i = 0; i < instances.Length; i++)
                    if (instances[i].GameID == gameID.Value)
                        return instances[i];
            }
            return null;
            //switch (gameID ?? 0)
            //{
            //    case GameID.HG: return MemberGame_HG.Instance;
            //    case GameID.EA: return MemberGame_EA.Instance;
            //    case GameID.WFT: return MemberGame_WFT.Instance;
            //    case GameID.WFT_SPORTS: return MemberGame_WFT_SPORTS.Instance;
            //    case GameID.KENO: return MemberGame_KENO.Instance;
            //    case GameID.KENO_SSC: return MemberGame_KENO_SSC.Instance;
            //    case GameID.SUNBET: return MemberGame_SUNBET.Instance;
            //    case GameID.AG: return MemberGame_AG.Instance;
            //    case GameID.BBIN: return MemberGame_BBIN.Instance;
            //    case GameID.CROWN_SPORTS: return MemberGame_CROWN.Instance;
            //    default: return null;
            //}
        }

        //public abstract decimal? GetBalance(SqlCmd sqlcmd, MemberGameRow row);
        public abstract MemberGameRow SelectRow(SqlCmd sqlcmd, int? memberID, bool getBalance);

        //internal abstract GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args);
        //internal abstract GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args);

        //public abstract MemberGameRowCommand DeserializeObject(string json);
        //public abstract MemberGameRow Update(MemberGameRowCommand command, string json_s, params object[] args);
    }

    public abstract class MemberGame<T, TRow, TRowCommand> : MemberGame
        where T : MemberGame<T, TRow, TRowCommand>, new()
        where TRow : MemberGameRow, new()
        where TRowCommand : MemberGameRowCommand, new()
    {
        public static readonly T Instance = new T();

        //public override decimal? GetBalance(SqlCmd sqlcmd, MemberGameRow row) { return this.OnGetBalance(sqlcmd, row as TRow); }

        [DebuggerStepThrough]
        public override MemberGameRow SelectRow(SqlCmd sqlcmd, int? memberID, bool getBalance) { return this.OnSelectRow(sqlcmd, memberID, getBalance); }
        //const string sql_sel1 = "select * from {0} nolock where GameID={1} and MemberID={2} and Locked<2";
        //const string sql_sel2 = "update {0} set Balance={3} where GameID={1} and MemberID={2} and Locked<2 " + sql_sel1;
        public TRow OnSelectRow(SqlCmd sqlcmd, int? memberID, bool getBalance)
        {
            if (memberID.HasValue)
            {
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    TRow row = sqlcmd.ToObject<TRow>("select * from {0} nolock where GameID={1} and MemberID={2} and Locked<2", this.TableName, (int?)this.GameID, memberID);
                    if (row != null)
                    {
                        if (getBalance)
                        {
                            return this.GetBalance(sqlcmd, row);
                            //decimal? balance = this.OnGetBalance(sqlcmd, row);
                            //if (balance.HasValue && (balance != row.Balance))
                            //{
                            //    return sqlcmd.ToObject<TRow>(sql_sel2, this.TableName, (int?)this.GameID, memberID, balance);
                            //}
                        }
                    }
                    return row;
                }
            }
            return null;
        }
        //protected abstract decimal? OnGetBalance(SqlCmd sqlcmd, TRow row);
        protected abstract TRow GetBalance(SqlCmd sqlcmd, TRow row);

        //public override MemberGameRowCommand DeserializeObject(string json) { return api.DeserializeObject<TRowCommand>(json); }

        //void execute_deposit(SqlCmd sqlcmd, MemberGameRowCommand command)
        //{
        //    try
        //    {
        //        decimal deposit = command.DepositAmount.ToDecimal() ?? 0;
        //        if (deposit > 0)
        //        {
        //            command.DepositResult = new GameTranRowCommand()
        //            {
        //                GameID = this.GameID,
        //                MemberID = command.MemberID,
        //                LogType = LogType.GameDeposit,
        //                Amount1 = deposit
        //            }.Insert(null, sqlcmd);
        //        }
        //    }
        //    catch { }
        //}
        //void execute_withdrawal(SqlCmd sqlcmd, MemberGameRowCommand command)
        //{
        //    try
        //    {
        //        decimal withdrawal = command.WithdrawalAmount.ToDecimal() ?? 0;
        //        if (withdrawal > 0)
        //        {
        //            command.WithdrawalResult = new GameTranRowCommand()
        //            {
        //                GameID = this.GameID,
        //                MemberID = command.MemberID,
        //                LogType = LogType.GameWithdrawal,
        //                Amount1 = withdrawal
        //            }.Insert(null, sqlcmd, this);
        //        }
        //    }
        //    catch { }
        //}

        //public override MemberGameRow Update(MemberGameRowCommand _command, string json_s, params object[] args)
        //{
        //    TRowCommand command = _command as TRowCommand;
        //    if (!command.MemberID.HasValue)
        //        throw new RowException(RowErrorCode.FieldNeeds, "MemberID");
        //    using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //    {
        //        execute_deposit(sqlcmd, command);
        //        execute_withdrawal(sqlcmd, command);
        //        return this.UpdateRow(sqlcmd, (TRowCommand)command, json_s, args);
        //    }
        //}
        protected abstract TRow UpdateRow(SqlCmd sqlcmd, TRowCommand command, string json_s, params object[] args);
    }

    public abstract class MemberGame_noapi<T, TRow, TRowCommand> : MemberGame<T, TRow, TRowCommand>
        where T : MemberGame_noapi<T, TRow, TRowCommand>, new()
        where TRow : MemberGameRow, new()
        where TRowCommand : MemberGameRowCommand, new()
    {
        public override bool HasAPI { get { return false; } }
        //internal override GameTranRow GameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    if (command.op_Accept == 1) command.TranOut(sqlcmd, tran);
        //    if (command.op_Finish == 1)
        //    {
        //        command.TranOut_confirm(sqlcmd, tran);
        //        command.op_Delete = 1;
        //    }
        //    if (command.op_Delete == 1) command.TranOut_delete(sqlcmd, tran);
        //    try { tran.MemberGameRow = this.SelectRow(sqlcmd, tran.MemberID, true); }
        //    catch { }
        //    return tran;
        //}

        //internal override GameTranRow GameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran, string json_s, params object[] args)
        //{
        //    if (command.op_Finish == 1)
        //    {
        //        command.TranIn(sqlcmd, tran);
        //        command.op_Delete = 1;
        //    }
        //    if (command.op_Delete == 1) command.TranIn_delete(sqlcmd, tran);
        //    try { tran.MemberGameRow = this.SelectRow(sqlcmd, tran.MemberID, true); }
        //    catch { }
        //    return tran;
        //}

        protected override TRow UpdateRow(SqlCmd sqlcmd, TRowCommand command, string json_s, params object[] args)
        {
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                string sqlstr;
                TRow row = this.OnSelectRow(sqlcmd, command.MemberID.Value, false);
                if (row == null)
                {
                    MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", command.MemberID);
                    sqltool s = new sqltool();
                    s["*", "MemberID", ""] = member.ID;
                    s["*", "GameID", "  "] = this.GameID;
                    s[" ", "Locked", "  "] = command.Locked ?? BU.Locked.Active;
                    s[" ", "Balance", " "] = command.Balance;
                    s["*", "ACNT", "    "] = text.ValidAsACNT * command.ACNT;
                    s[" ", "pwd", "     "] = text.ValidAsString * command.Password;
                    s[" ", "Currency", ""] = command.Currency ?? member.Currency;
                    s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                    s.TestFieldNeeds();
                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
                    sqlstr = s.BuildEx("insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, @")
select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
                }
                else
                {
                    sqltool s = new sqltool();
                    s[" ", "Locked", "  ", row.Locked, "  "] = command.Locked;
                    s[" ", "Balance", " ", row.Balance, " "] = command.Balance;
                    s[" ", "ACNT", "    ", row.ACNT, "    "] = text.ValidAsString * command.ACNT;
                    s[" ", "pwd", "     ", row.Password, ""] = text.ValidAsString * command.Password;
                    s[" ", "Currency", "", row.Currency, ""] = command.Currency;
                    if (s.fields.Count == 0) return row;
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
                    s.Values["GameID"] = this.GameID;
                    s.Values["MemberID"] = row.MemberID;
                    sqlstr = s.BuildEx("update {MemberTable} set ", sqltool._FieldValue, @" where GameID={GameID} and MemberID={MemberID}
select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
                }
                return sqlcmd.ExecuteEx<TRow>(sqlstr);
            }
        }

        //protected override decimal? OnGetBalance(SqlCmd sqlcmd, TRow row)
        //{
        //    return null;
        //}

        protected override TRow GetBalance(SqlCmd sqlcmd, TRow row) { return row; }
    }

    public class MemberGame_SUNBET : MemberGame_noapi<MemberGame_SUNBET, MemberGame_SUNBET.Row, MemberGame_SUNBET.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow { }
        public class RowCommand : MemberGameRowCommand { protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_SUNBET.Instance; } } }

        public override GameID GameID { get { return BU.GameID.SUNBET; } }
        public override string TableName { get { return "Member_005"; } }
    }
    public class MemberGame_AG : MemberGame_noapi<MemberGame_AG, MemberGame_AG.Row, MemberGame_AG.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow { }
        public class RowCommand : MemberGameRowCommand { protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_AG.Instance; } } }

        public override GameID GameID { get { return BU.GameID.AG; } }
        public override string TableName { get { return "Member_006"; } }
    }
    public class MemberGame_CROWN : MemberGame_noapi<MemberGame_CROWN, MemberGame_CROWN.Row, MemberGame_CROWN.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow { }
        public class RowCommand : MemberGameRowCommand { protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_CROWN.Instance; } } }
        public override GameID GameID { get { return BU.GameID.CROWN_SPORTS; } }
        public override string TableName { get { return "Member_010"; } }
    }
    public class MemberGame_SALON : MemberGame_noapi<MemberGame_SALON, MemberGame_SALON.Row, MemberGame_SALON.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow { }
        public class RowCommand : MemberGameRowCommand { protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_SALON.Instance; } } }
        public override GameID GameID { get { return BU.GameID.SALON; } }
        public override string TableName { get { return "Member_014"; } }
    }

    public class MemberGame_EXTRA : MemberGame_noapi<MemberGame_EXTRA, MemberGame_EXTRA.Row, MemberGame_EXTRA.RowCommand>
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Row : MemberGameRow { }
        public class RowCommand : MemberGameRowCommand { protected override MemberGame proc { [DebuggerStepThrough] get { return MemberGame_EXTRA.Instance; } } }
        public override GameID GameID { get { return BU.GameID.EXTRA; } }
        public override string TableName { get { return "Member_254"; } }
    }



    //public abstract class MemberGameRowCommand1 : MemberRowCommand
    //{
    //    //internal MemberGameRowCommand1(GameID gameid, string tableName) : base(gameid, tableName) { }
    //}

    //public abstract partial class MemberGameRowCommand<TRow, TRowCommand> : MemberGameRowCommand
    //    where TRow : MemberGameRow, new()
    //    where TRowCommand : MemberGameRowCommand<TRow, TRowCommand>
    //{
    //    public static TRowCommand Instance = Activator.CreateInstance<TRowCommand>();
    //    public override Type RowType { [DebuggerStepThrough] get { return typeof(TRow); } }
    //    internal MemberGameRowCommand(GameID gameid, string tableName) : base(gameid, tableName) { }


    //    //public override MemberGameRow SelectRow(SqlCmd sqlcmd, int memberID) { return this.SelectRow(sqlcmd, memberID, null); }
    //    //public TRow SelectRow(SqlCmd sqlcmd, int memberID, TRow row)
    //    //{
    //    //    return sqlcmd.ToObject<TRow>("select * from {0} nolock where GameID={1} and MemberID={2}", this.TableName, (int?)this.GameID, memberID);
    //    //}

    //    //public override MemberGameRow Update(string json_s, params object[] args)
    //    //{
    //    //    if (!this.MemberID.HasValue)
    //    //        throw new RowException(RowErrorCode.FieldNeeds, "MemberID");
    //    //    using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
    //    //    {
    //    //        #region deposit
    //    //        try
    //    //        {
    //    //            decimal deposit = this.DepositAmount.ToDecimal() ?? 0;
    //    //            if (deposit > 0)
    //    //            {
    //    //                this.DepositResult = new GameTranRowCommand()
    //    //                {
    //    //                    GameID = this.GameID,
    //    //                    MemberID = this.MemberID,
    //    //                    LogType = LogType.GameDeposit,
    //    //                    Amount1 = deposit
    //    //                }.Insert(null, sqlcmd, this);
    //    //                this.Balance = "*";
    //    //            }
    //    //        }
    //    //        catch { }
    //    //        #endregion
    //    //        #region withdrawal
    //    //        try
    //    //        {
    //    //            decimal withdrawal = this.WithdrawalAmount.ToDecimal() ?? 0;
    //    //            if (withdrawal > 0)
    //    //            {
    //    //                this.WithdrawalResult = new GameTranRowCommand()
    //    //                {
    //    //                    GameID = this.GameID,
    //    //                    MemberID = this.MemberID,
    //    //                    LogType = LogType.GameWithdrawal,
    //    //                    Amount1 = withdrawal
    //    //                }.Insert(null, sqlcmd, this);
    //    //                this.Balance = "*";
    //    //            }
    //    //        }
    //    //        catch { }
    //    //        #endregion
    //    //        return this.UpdateRow(sqlcmd, json_s, args);
    //    //    }
    //    //}

    //    protected abstract TRow UpdateRow(SqlCmd sqlcmd, string json_s, params object[] args);
    //}

    //    #region 沒有 API 的平台

    ////    public abstract partial class noapi_MemberGameRowCommand<TRow, TRowCommand> : MemberGameRowCommand<TRow, TRowCommand>
    ////        where TRow : MemberGameRow, new()
    ////        where TRowCommand : noapi_MemberGameRowCommand<TRow, TRowCommand>
    ////    {
    ////        internal noapi_MemberGameRowCommand(GameID gameid, string tableName) : base(gameid, tableName) { }

    ////        protected override TRow UpdateRow(SqlCmd sqlcmd, string json_s, params object[] args)
    ////        {
    ////            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
    ////            {
    ////                string sqlstr;
    ////                TRow row = this.SelectRow(sqlcmd, this.MemberID.Value, null);
    ////                if (row == null)
    ////                {
    ////                    MemberRow member = sqlcmd.GetRowEx<MemberRow>(RowErrorCode.MemberNotFound, "select * from Member nolock where ID={0}", this.MemberID);
    ////                    sqltool s = new sqltool();
    ////                    s["*", "MemberID", ""] = member.ID;
    ////                    s["*", "GameID", "  "] = this.GameID;
    ////                    s[" ", "Locked", "  "] = (text.ValidAsLocked * this.Locked) ?? 0;
    ////                    s[" ", "Balance", " "] = this.Balance.ToDecimal() ?? 0;
    ////                    s["*", "ACNT", "    "] = text.ValidAsACNT * this.ACNT;
    ////                    s[" ", "pwd", "     "] = text.ValidAsString * this.Password;
    ////                    s[" ", "Currency", ""] = this.Currency ?? member.Currency;
    ////                    s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
    ////                    s.TestFieldNeeds();
    ////                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
    ////                    sqlstr = s.BuildEx("insert into {MemberTable} (", sqltool._Fields, ") values (", sqltool._Values, @")
    ////select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
    ////                }
    ////                else
    ////                {
    ////                    sqltool s = new sqltool();
    ////                    s[" ", "Locked", "  ", row.Locked, "  "] = text.ValidAsLocked * this.Locked;
    ////                    s[" ", "Balance", " ", row.Balance, " "] = this.Balance.ToDecimal();
    ////                    s[" ", "ACNT", "    ", row.ACNT, "    "] = text.ValidAsString * this.ACNT;
    ////                    s[" ", "pwd", "     ", row.Password, ""] = text.ValidAsString * this.Password;
    ////                    s[" ", "Currency", "", row.Currency, ""] = this.Currency;
    ////                    if (s.fields.Count == 0) return row;
    ////                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
    ////                    s.Values["MemberTable"] = (StringEx.sql_str)this.TableName;
    ////                    s.Values["GameID"] = this.GameID;
    ////                    s.Values["MemberID"] = row.MemberID;
    ////                    sqlstr = s.BuildEx("update {MemberTable} set ", sqltool._FieldValue, @" where GameID={GameID} and MemberID={MemberID}
    ////select * from {MemberTable} nolock where GameID={GameID} and MemberID={MemberID}");
    ////                }
    ////                return sqlcmd.ExecuteEx<TRow>(sqlstr);
    ////            }
    ////        }
    ////    }

    //    // 太陽城
    //    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //    public class MemberGameRow_SUNBET : MemberGameRow { }
    //    [DebuggerStepThrough]
    //    public class MemberGameRowCommand_SUNBET : MemberGameRowCommand1
    //    {
    //        public MemberGameRowCommand_SUNBET() : base(GameID.SUNBET, "Member_005") { }
    //    }

    //    // AG 國際廳
    //    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //    public class MemberGameRow_AG : MemberGameRow { }
    //    [DebuggerStepThrough]
    //    public class MemberGameRowCommand_AG : MemberGameRowCommand1
    //    {
    //        public MemberGameRowCommand_AG() : base(GameID.AG, "Member_006") { }
    //    }

    //    // 皇冠體育
    //    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //    public class MemberGameRow_CROWN : MemberGameRow { }
    //    [DebuggerStepThrough]
    //    public class MemberGameRowCommand_CROWN : MemberGameRowCommand1
    //    {
    //        public MemberGameRowCommand_CROWN() : base(GameID.CROWN_SPORTS, "Member_010") { }
    //    }
    //    #endregion
}