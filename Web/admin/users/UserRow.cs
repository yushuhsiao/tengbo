using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Tools;

namespace web
{
    public abstract class UserRow
    {
        public abstract UserType UserType { get; }
        public abstract string TableName { get; }
        public abstract StringEx.sql_str TableName2 { get; }
        public abstract StringEx.sql_str GroupTableName { get; }
        public abstract int? ParentID { get; set; }
        public abstract string ParentACNT { get; set; }
        public abstract int UserLevel { get; set; }

        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public string ACNT;
        [DbImport, JsonProperty]
        public Guid? GroupID;
        //[JsonProperty("GroupID")]
        //long? _out_GroupID
        //{
        //    get { return text.GroupRowID(this.CorpID, this.GroupID); }
        //}
        [DbImport, JsonProperty]
        public string Name;
        [DbImport("pwd")]
        public string Password;
        [DbImport, JsonProperty]
        public Locked? Locked;
        [DbImport]
        public byte? TranFlag;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;

        [DbImport, JsonProperty]
        public CurrencyCode? Currency;
        [DbImport, JsonProperty]
        public decimal? Balance;

        protected static T GetUser<T>(SqlCmd sqlcmd, string tableName, int? id, int? corpID, string acnt, params string[] fields) where T : UserRow, new()
        {
            if ((id.HasValue) || (corpID.HasValue && (string.IsNullOrEmpty(acnt) == false)))
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select ");
                if (fields.GetValue<string>(0) == "*")
                    sql.Append("*");
                else
                {
                    sql.Append("ID, CorpID, ACNT");
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (fields[i] == "ID") continue;
                        if (fields[i] == "CorpID") continue;
                        if (fields[i] == "ACNT") continue;
                        sql.Append(",");
                        sql.Append(fields[i]);
                    }
                }
                sql.Append(" from ");
                sql.Append(tableName);
                sql.Append(" nolock where");
                if (id.HasValue)
                    sql.AppendFormat(" ID={0}", id.Value);
                else
                    sql.AppendFormat(" CorpID={0} and ACNT='{1}'", corpID.Value, acnt * text.ValidAsACNT);
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                    return sqlcmd.ToObject<T>(sql.ToString());
            }
            return null;
        }
    }

    public abstract class UserRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual string ACNT { get; set; }
        [JsonProperty]
        public virtual int? UserLevel { get; set; }
        [JsonProperty]
        public virtual Guid? GroupID { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual string Password { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }

        public void apply_GroupID(SqlCmd sqlcmd)
        {
            if (this.GroupID.HasValue) return;
            foreach (SqlDataReader r1 in sqlcmd.ExecuteReader2("select ID, Sort from {0} nolock where CorpID={1} order by Sort", _null<AdminRow>.value.GroupTableName, this.CorpID))
            {
                int sort = r1.GetInt32("Sort");
                if (sort < 1)
                    this.GroupID = r1.GetGuidN("ID");
            }
        }
    }

    public abstract class UserDetails_page : web.page
    {
        public abstract UserType UserType { get; }
    }

    public abstract class UserList2_aspx : web.page
    {
        public List<game.UserGameRow> rows1;
        public List<game.UserGameRow> rows2;
        public abstract UserType UserType { get; }

        public int userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.userID = Request.QueryString["id"].ToInt32() ?? 0;
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                foreach (GameRow g in GameRow.Cache.GetInstance(sqlcmd, null).Rows)
                {
                    game.IUserGameRowCommand cmd = game.GetUserGameRowCommand(this.UserType, g.ID, null, true);
                    if (cmd != null)
                    {
                        game.UserGameRow row = cmd.SelectUserRow(sqlcmd, userID, null, true);
                        if (cmd.HasAPI)
                        {
                            if (this.rows1 == null)
                                this.rows1 = new List<game.UserGameRow>();
                            this.rows1.Add(row);
                        }
                        else
                        {
                            if (this.rows2 == null)
                                this.rows2 = new List<game.UserGameRow>();
                            this.rows2.Add(row);
                        }
                    }
                }
            }
        }
    }

    abstract class UserGameCommand : IRowCommand
    {
        [JsonProperty]
        protected UserType? UserType;

        [JsonProperty]
        protected GameID? GameID;
    }
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    sealed class UserGameUpdate : UserGameCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.agent2, Permissions.Flag.Write)]
        static game.UserGameRow execute(UserGameUpdate command, string json_s, params object[] args)
        {
            return game.GetUserGameRowCommand(command.UserType, command.GameID, json_s, false).UpdateUserRow(null, json_s, args);
        }
    }
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    sealed class UserGameBalance : UserGameCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.agent2, Permissions.Flag.Write)]
        static game.UserGameRow execute(UserGameBalance command, string json_s, params object[] args)
        {
            return game.GetUserGameRowCommand(command.UserType, command.GameID, json_s, false).GetBalance(null, json_s, args);
        }
    }
}

//#region 會員子帳戶

//namespace web
//{
//    //public abstract class MemberGameRowCommand
//    //{
//    //    public MemberGameRow Update(string json_s, params object[] args)
//    //    {
//    //    }


//    //    public readonly string TableName;
//    //    public MemberGameRowCommand(GameID gameID, string tableName)
//    //    {
//    //        this.GameID = gameID;
//    //        this.TableName = tableName;
//    //    }
//    //}

//    //public abstract class MemberGameRowCommand<TRow, TRowCommand> : MemberGameRowCommand
//    //    where TRow : MemberGameRow, new()
//    //    where TRowCommand : MemberGameRowCommand<TRow, TRowCommand>, new()
//    //{
//    //    public MemberGameRowCommand(GameID gameID, string tableName) : base(gameID, tableName) { }
//    //}

//    //public abstract partial class MemberGame
//    //{
//    //    public readonly GameID GameID;
//    //    public readonly string TableName;
//    //    internal MemberGame(GameID gameID, string tableName)
//    //    {
//    //        this.GameID = gameID;
//    //        this.TableName = tableName;
//    //    }

//    //    public static MemberGame GetInstance(GameID? gameID)
//    //    {
//    //        switch (gameID ?? 0)
//    //        {
//    //            case GameID.HG: return MemberGame_HG.Instance;
//    //            case GameID.EA: return MemberGame_EA.Instance;
//    //            case GameID.WFT: return MemberGame_WFT.Instance;
//    //            case GameID.WFT_SPORTS: return MemberGame_WFT_SPORTS.Instance;
//    //            case GameID.KENO: return MemberGame_KENO.Instance;
//    //            case GameID.KENO_SSC: return MemberGame_KENO_SSC.Instance;
//    //            case GameID.SUNBET: return MemberGame_SUNBET.Instance;
//    //            case GameID.AG: return MemberGame_AG.Instance;
//    //            case GameID.BBIN: return MemberGame_BBIN.Instance;
//    //        }
//    //        return null;
//    //    }

//    //    public abstract MemberGameRow Select(SqlCmd sqlcmd, int memberID);
//    //    public abstract MemberGameRow InsertRow(SqlCmd sqlcmd, int memberID);
//    //    public abstract MemberGameRow UpdateRow(string json_s, params object[] args);
//    //}

//    //public abstract partial class MemberGame<T, TRow, TRowCommand> : MemberGame
//    //    where T : MemberGame<T, TRow, TRowCommand>, new()
//    //    where TRow : MemberGameRow, new()
//    //    where TRowCommand : MemberGameRowCommand, new()
//    //{
//    //    public static readonly T Instance = new T();
//    //    internal MemberGame(GameID gameID, string tableName) : base(gameID, tableName) { }

//    //    public override MemberGameRow Select(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return this.Select2(sqlcmd, memberID);
//    //    }
//    //    public TRow Select2(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return sqlcmd.ToObject<TRow>("select * from {0} nolock where GameID={1} and MemberID={2}", this.TableName, (int?)this.GameID, memberID);
//    //    }

//    //    public override MemberGameRow InsertRow(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return this.InsertRow(sqlcmd, memberID);
//    //    }
//    //    public TRow InsertRow2(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return null;
//    //    }

//    //    public override MemberGameRow UpdateRow(string json_s, params object[] args)
//    //    {
//    //        return this.UpdateRow2(json_s, args);
//    //    }
//    //    public TRow UpdateRow2(string json_s, params object[] args)
//    //    {
//    //        return null;
//    //    }
//    //}

//    //public abstract partial class MemberGameRowCommand
//    //{
//    //    [JsonProperty]
//    //    internal readonly GameID GameID;
//    //    [JsonProperty]
//    //    public virtual int? MemberID { get; set; }
//    //    [JsonProperty]
//    //    public virtual string Locked { get; set; }
//    //    [JsonProperty]
//    //    public virtual string Balance { get; set; }
//    //    [JsonProperty]
//    //    public virtual string Password { get; set; }
//    //    [JsonProperty]
//    //    public virtual string ACNT { get; set; }
//    //    [JsonProperty]
//    //    public virtual CurrencyCode? Currency { get; set; }
//    //    [JsonProperty]
//    //    public virtual string DepositAmount { get; set; }
//    //    public GameTranRow DepositResult;
//    //    [JsonProperty]
//    //    public virtual string WithdrawalAmount { get; set; }
//    //    public GameTranRow WithdrawalResult;

//    //public static MemberGameRowCommand GetInstance(GameID gameID)
//    //{
//    //    switch (gameID)
//    //    {
//    //        case GameID.HG: return MemberGameRowCommand_HG.Instance;
//    //        case GameID.EA: return MemberGameRowCommand_EA.Instance;
//    //        case GameID.WFT: return MemberGameRowCommand_WFT.Instance;
//    //        case GameID.WFT_SPORTS: return MemberGameRowCommand_WFT_SPORTS.Instance;
//    //        case GameID.KENO: return MemberGameRowCommand_KENO.Instance;
//    //        case GameID.KENO_SSC: return MemberGameRowCommand_KENO_SSC.Instance;
//    //        case GameID.SUNBET: return MemberGameRowCommand_SUNBET.Instance;
//    //        case GameID.AG: return MemberGameRowCommand_AG.Instance;
//    //        case GameID.BBIN: return MemberGameRowCommand_BBIN.Instance;
//    //    }
//    //    return null;
//    //}
        
//    //    public readonly string TableName;
//    //    internal MemberGameRowCommand(GameID gameID, string tableName) { this.GameID = gameID; this.TableName = tableName; }
//    //    public abstract MemberGameRow GetRow(SqlCmd sqlcmd, int memberID);
//    //    public abstract MemberGameRow Update(string json_s, params object[] args);
//    //    internal abstract MemberGameRow OnUpdate(SqlCmd _sqlcmd, string json_s, params object[] args);
//    //    internal virtual bool OnGameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran) { return false; }
//    //    internal virtual bool OnGameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran) { return false; }
//    //}

//    //public abstract partial class MemberGameRowCommand<TRowCommand, TRow> : MemberGameRowCommand
//    //    where TRowCommand : MemberGameRowCommand<TRowCommand, TRow>, new()
//    //    where TRow : MemberGameRow, new()
//    //{
//    //    internal MemberGameRowCommand(GameID gameID, string tableName) : base(gameID, tableName) { }
//    //    public static readonly TRowCommand Instance = new TRowCommand();

//    //    [DebuggerStepThrough]
//    //    public override MemberGameRow GetRow(SqlCmd sqlcmd, int memberID) { return GetRow2(sqlcmd, memberID); }
//    //    public TRow GetRow2(SqlCmd sqlcmd, int memberID)
//    //    {
//    //        return sqlcmd.ToObject<TRow>("select * from {0} nolock where GameID={1} and MemberID={2}", this.TableName, (int?)this.GameID, memberID);
//    //    }

//    //    [DebuggerStepThrough]
//    //    public override MemberGameRow Update(string json_s, params object[] args) { return this.Update2(json_s, args); }
//    //    public TRow Update2(string json_s, params object[] args)
//    //    {
//    //        SqlCmd sqlcmd = null;
//    //        decimal deposit = this.DepositAmount.ToDecimal() ?? 0;
//    //        if (deposit > 0)
//    //        {
//    //            DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd);
//    //            try
//    //            {
//    //                this.DepositResult = new GameTranRowCommand()
//    //                {
//    //                    GameID = this.GameID,
//    //                    MemberID = this.MemberID,
//    //                    LogType = LogType.GameDeposit,
//    //                    Amount1 = deposit
//    //                }.Insert(null, sqlcmd);
//    //                this.Balance = "*";
//    //            }
//    //            catch { }
//    //        }
//    //        decimal withdrawal = this.WithdrawalAmount.ToDecimal() ?? 0;
//    //        if (withdrawal > 0)
//    //        {
//    //            DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd);
//    //            try
//    //            {
//    //                this.WithdrawalResult = new GameTranRowCommand()
//    //                {
//    //                    GameID = this.GameID,
//    //                    MemberID = this.MemberID,
//    //                    LogType = LogType.GameWithdrawal,
//    //                    Amount1 = withdrawal
//    //                }.Insert(null, sqlcmd);
//    //                this.Balance = "*";
//    //            }
//    //            catch { }
//    //        }
//    //        return this.OnUpdate2(sqlcmd, json_s, args);
//    //    }

//    //    [DebuggerStepThrough]
//    //    internal override MemberGameRow OnUpdate(SqlCmd sqlcmd, string json_s, params object[] args) { return this.OnUpdate2(sqlcmd, json_s, args); }
//    //    internal abstract TRow OnUpdate2(SqlCmd sqlcmd, string json_s, params object[] args);
//    //}
//}

//#endregion

//#region 會員子帳戶 v2
//namespace web
//{
//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public abstract class MemberGameRow
//    {
//        [DbImport, JsonProperty]
//        public GameID? GameID;
//        [DbImport, JsonProperty]
//        public int? MemberID;

//        [DbImport, JsonProperty]
//        public virtual Locked? Locked { get; set; }              // dbo.Member_???.Locked
//        [DbImport, JsonProperty]
//        public virtual decimal? Balance { get; set; }
//        [DbImport, JsonProperty]
//        public virtual string ACNT { get; set; }
//        [DbImport("pwd"), JsonProperty]
//        public virtual string Password { get; set; }
//        [DbImport, JsonProperty]
//        public virtual CurrencyCode? Currency { get; set; }
//        [DbImport, JsonProperty]
//        public virtual DateTime? CreateTime { get; set; }
//        [DbImport, JsonProperty]
//        public virtual int? CreateUser { get; set; }
//        [DbImport, JsonProperty]
//        public virtual DateTime? ModifyTime { get; set; }
//        [DbImport, JsonProperty]
//        public virtual int? ModifyUser { get; set; }
//    }

//    public abstract class MemberGameRowCommand
//    {
//        public static MemberGameRowCommand GetInstance(GameID gameID)
//        {
//            switch (gameID)
//            {
//                case GameID.HG: return MemberGameRowCommand_HG.Instance;
//                case GameID.EA: return MemberGameRowCommand_EA.Instance;
//                case GameID.WFT: return MemberGameRowCommand_WFT.Instance;
//                case GameID.WFT_SPORTS: return MemberGameRowCommand_WFT_SPORTS.Instance;
//                case GameID.KENO: return MemberGameRowCommand_KENO.Instance;
//                case GameID.KENO_SSC: return MemberGameRowCommand_KENO_SSC.Instance;
//                case GameID.SUNBET: return MemberGameRowCommand_SUNBET.Instance;
//                case GameID.AG: return MemberGameRowCommand_AG.Instance;
//                case GameID.BBIN: return MemberGameRowCommand_BBIN.Instance;
//            }
//            return null;
//        }

//        [JsonProperty]
//        internal readonly GameID GameID;
//        [JsonProperty]
//        public virtual int? MemberID { get; set; }
//        [JsonProperty]
//        public virtual string Locked { get; set; }
//        [JsonProperty]
//        public virtual string Balance { get; set; }
//        [JsonProperty]
//        public virtual string Password { get; set; }
//        [JsonProperty]
//        public virtual string ACNT { get; set; }
//        [JsonProperty]
//        public virtual CurrencyCode? Currency { get; set; }
//        [JsonProperty]
//        public virtual string DepositAmount { get; set; }
//        public GameTranRow DepositResult;
//        [JsonProperty]
//        public virtual string WithdrawalAmount { get; set; }
//        public GameTranRow WithdrawalResult;

//        public readonly string TableName;
//        public abstract Type RowType { get; }
//        internal MemberGameRowCommand(GameID gameid, string tableName) { this.GameID = gameid; this.TableName = tableName; }

//        public MemberGameRow Update(string json_s, params object[] args)
//        {
//            SqlCmd sqlcmd = null;
//            decimal deposit = this.DepositAmount.ToDecimal() ?? 0;
//            if (deposit > 0)
//            {
//                DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd);
//                try
//                {
//                    this.DepositResult = new GameTranRowCommand()
//                    {
//                        GameID = this.GameID,
//                        MemberID = this.MemberID,
//                        LogType = LogType.GameDeposit,
//                        Amount1 = deposit
//                    }.Insert(null, sqlcmd);
//                    this.Balance = "*";
//                }
//                catch { }
//            }
//            decimal withdrawal = this.WithdrawalAmount.ToDecimal() ?? 0;
//            if (withdrawal > 0)
//            {
//                DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd);
//                try
//                {
//                    this.WithdrawalResult = new GameTranRowCommand()
//                    {
//                        GameID = this.GameID,
//                        MemberID = this.MemberID,
//                        LogType = LogType.GameWithdrawal,
//                        Amount1 = withdrawal
//                    }.Insert(null, sqlcmd);
//                    this.Balance = "*";
//                }
//                catch { }
//            }
//            return this.OnUpdate(sqlcmd, json_s, args);
//        }
//        internal abstract MemberGameRow OnUpdate(SqlCmd _sqlcmd, string json_s, params object[] args);

//        internal abstract bool OnGameDeposit(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran);

//        internal abstract bool OnGameWithdrawal(SqlCmd sqlcmd, GameTranRowCommand command, GameTranRow tran);

//    }
//    public abstract class MemberGameRowCommand<TRow, TRowCommand> : MemberGameRowCommand
//        where TRow : MemberGameRow
//        where TRowCommand : MemberGameRowCommand<TRow, TRowCommand>
//    {
//        public static TRowCommand Instance = Activator.CreateInstance<TRowCommand>();
//        public override Type RowType { [DebuggerStepThrough] get { return typeof(TRow); } }
//        internal MemberGameRowCommand(GameID gameid, string tableName) : base(gameid, tableName) { }
//    }

//    #region HG

//    public class MemberGameRow_HG : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_HG : MemberGameRowCommand<MemberGameRow_HG, MemberGameRowCommand_HG>, IGameAPI
//    {
//        public MemberGameRowCommand_HG() : base(GameID.HG, "Member_001") { }
//    }

//    #endregion

//    #region EA

//    public class MemberGameRow_EA : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_EA : MemberGameRowCommand<MemberGameRow_EA, MemberGameRowCommand_EA>, IGameAPI
//    {
//        public MemberGameRowCommand_EA() : base(GameID.EA, "Member_002") { }
//    }

//    #endregion

//    #region WFT

//    public class MemberGameRow_WFT : MemberGameRow
//    {
//    }
//    public abstract class _MemberGameRowCommand_WFT<T> : MemberGameRowCommand<MemberGameRow_WFT, T>, IGameAPI where T : _MemberGameRowCommand_WFT<T>
//    {
//        internal _MemberGameRowCommand_WFT(GameID gameid, string tableName) : base(gameid, tableName) { }
//    }

//    public class MemberGameRowCommand_WFT : _MemberGameRowCommand_WFT<MemberGameRowCommand_WFT>
//    {
//        public MemberGameRowCommand_WFT() : base(GameID.WFT, "Member_003") { }
//    }
//    public class MemberGameRowCommand_WFT_SPORTS : _MemberGameRowCommand_WFT<MemberGameRowCommand_WFT_SPORTS>
//    {
//        public MemberGameRowCommand_WFT_SPORTS() : base(GameID.WFT_SPORTS, "Member_008") { }
//    }

//    #endregion

//    #region KENO

//    public class MemberGameRow_KENO : MemberGameRow
//    {
//    }
//    public abstract class _MemberGameRowCommand_KENO<T> : MemberGameRowCommand<MemberGameRow_KENO, T>, IGameAPI where T : _MemberGameRowCommand_KENO<T>
//    {
//        internal _MemberGameRowCommand_KENO(GameID gameid, string tableName) : base(gameid, tableName) { }
//    }

//    public class MemberGameRowCommand_KENO : _MemberGameRowCommand_KENO<MemberGameRowCommand_KENO>
//    {
//        public MemberGameRowCommand_KENO() : base(GameID.KENO, "Member_004") { }
//    }
//    public class MemberGameRowCommand_KENO_SSC : _MemberGameRowCommand_KENO<MemberGameRowCommand_KENO_SSC>
//    {
//        public MemberGameRowCommand_KENO_SSC() : base(GameID.KENO_SSC, "Member_007") { }
//    }

//    #endregion

//    #region SUBNET

//    public class MemberGameRow_SUNBET : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_SUNBET : MemberGameRowCommand<MemberGameRow_SUNBET, MemberGameRowCommand_SUNBET>
//    {
//        public MemberGameRowCommand_SUNBET() : base(GameID.SUNBET, "Member_005") { }
//    }

//    #endregion

//    #region AG

//    public class MemberGameRow_AG : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_AG : MemberGameRowCommand<MemberGameRow_AG, MemberGameRowCommand_AG>
//    {
//        public MemberGameRowCommand_AG() : base(GameID.AG, "Member_006") { }
//    }

//    #endregion

//    #region BBIN

//    public class MemberGameRow_BBIN : MemberGameRow
//    {
//    }
//    public class MemberGameRowCommand_BBIN : MemberGameRowCommand<MemberGameRow_BBIN, MemberGameRowCommand_BBIN>, IGameAPI
//    {
//        public MemberGameRowCommand_BBIN() : base(GameID.BBIN, "Member_009") { }
//    }

//    #endregion

//    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    //public abstract class MemberRow2_XXX
//    //{
//    //    public MemberRow2_XXX(GameID gameID) { this.GameID = gameID; }

//    //    [DbImport, JsonProperty]
//    //    public GameID GameID;
//    //}
//    //public class MemberRow2_HG : MemberRow2_XXX
//    //{
//    //    public MemberRow2_HG() : base(GameID.HG) { }
//    //}
//    //public class MemberRow2_EA : MemberRow2_XXX
//    //{
//    //    public MemberRow2_EA() : base(GameID.EA) { }
//    //}

//    //public abstract class MemberRowCommand2_XXX
//    //{
//    //}
//}
//namespace web
//{
//}
//#endregion