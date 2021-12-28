using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using Tools;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace web
{
    static partial class game
    {

    }
}

namespace web
{
    //public static partial class tran__
    //{
    //    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //    public class GameRowData : tran__.RowData
    //    {
    //        #region Fields
    //        #endregion

    //        [JsonProperty]
    //        public TranLogRow TranLog;
    //        public StringEx.sql_str TableUser
    //        {
    //            get
    //            {
    //                if (this.UserType == BU.UserType.Agent) return s_TableA;
    //                if (this.UserType == BU.UserType.Member) return s_TableM;
    //                return null;
    //            }
    //        }
    //        static StringEx.sql_str s_TableA = _null<AgentRow>.value.TableName;
    //        static StringEx.sql_str s_TableM = _null<MemberRow>.value.TableName;
    //        static StringEx.sql_str s_Table1 = "tranGame1";
    //        static StringEx.sql_str s_Table2 = "tranGame2";
    //        public override StringEx.sql_str Table1 { get { return s_Table1; } }
    //        public override StringEx.sql_str Table2 { get { return s_Table2; } }
    //    }

    //    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //    //public abstract class GameRowCommand<TUser, TRowCommand> : tran.RowCommand<TUser, tran.GameRowData, TRowCommand>
    //    //    where TUser : UserRow, new()
    //    //    where TRowCommand : GameRowCommand<TUser, TRowCommand>, new()
    //    //{
    //    //}

    //    static __game.proc game_proc(SqlCmd sqlcmd, string id, bool? op_Insert, out tran__.GameRowData tranrow, UserType? userType, GameID? gameID)
    //    {
    //        if (op_Insert == true)
    //            tranrow = null;
    //        else
    //        {
    //            tranrow = sqlcmd.GetRowEx<tran__.GameRowData>(RowErrorCode.GameTranNotFound, "select * from {0} nolock where ID='{1}'", _null<tran__.GameRowData>.value.Table1, id * text.ValidAsString);
    //            if (tranrow != null)
    //            {
    //                userType = tranrow.UserType;
    //                gameID = tranrow.GameID;
    //            }
    //        }
    //        return __game.GetProc(userType, gameID);
    //    }

    //    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //    public abstract class GameDepositRowCommand<TUser, TRowCommand> : tran__.RowCommand<TUser, tran__.GameRowData, TRowCommand>
    //        where TUser : UserRow, new()
    //        where TRowCommand : GameDepositRowCommand<TUser, TRowCommand>, new()
    //    {
    //        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return this.LogType.Value; }
    //        public override string prefix { get { return "C"; } }
    //        public override BU.LogType? LogType { get { return BU.LogType.GameDeposit; } set { base.LogType = BU.LogType.GameDeposit; } }

    //        protected override GameRowData proc_tran(string json_s, params object[] args)
    //        {
    //            SqlCmd sqlcmd;
    //            tran__.GameRowData tranrow;
    //            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
    //                return game_proc(sqlcmd, this.ID, this.op_Insert, out tranrow, this.UserType, this.GameID).Deposit(sqlcmd, tranrow, this, json_s, args);
    //        }
    //    }

    //    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //    public abstract class WithdrawalRowCommand<TUser, TRowCommand> : tran__.RowCommand<TUser, tran__.GameRowData, TRowCommand>
    //        where TUser : UserRow, new()
    //        where TRowCommand : WithdrawalRowCommand<TUser, TRowCommand>, new()
    //    {
    //        protected override IEnumerable<BU.LogType> initAcceptLogTypes() { yield return this.LogType.Value; }
    //        public override string prefix { get { return "D"; } }
    //        public override BU.LogType? LogType { get { return BU.LogType.GameWithdrawal; } set { base.LogType = BU.LogType.GameWithdrawal; } }

    //        protected override GameRowData proc_tran(string json_s, params object[] args)
    //        {
    //            SqlCmd sqlcmd;
    //            tran__.GameRowData tranrow;
    //            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
    //                return game_proc(sqlcmd, this.ID, this.op_Insert, out tranrow, this.UserType, this.GameID).Withdrawal(sqlcmd, tranrow, this, json_s, args);
    //        }
    //    }
    //}
//    public static partial class __game
//    {
//        public class UserGameRowData
//        {
//            #region Fields

//            [DbImport, JsonProperty]
//            public virtual int? MemberID { get; set; }
//            [DbImport, JsonProperty]
//            public virtual int? AgentID { get; set; }
//            [DbImport, JsonProperty]
//            public virtual GameID? GameID { get; set; }
//            [DbImport, JsonProperty]
//            public virtual Locked? Locked { get; set; }
//            [DbImport, JsonProperty]
//            public virtual decimal? Balance { get; set; }
//            [DbImport, JsonProperty]
//            public virtual string ACNT { get; set; }
//            [DbImport("pwd"), JsonProperty]
//            public virtual string Password { get; set; }
//            [DbImport, JsonProperty]
//            public virtual string Currency { get; set; }
//            [DbImport, JsonProperty]
//            public virtual DateTime? CreateTime { get; set; }
//            [DbImport, JsonProperty]
//            public virtual int? CreateUser { get; set; }
//            [DbImport, JsonProperty]
//            public virtual DateTime? ModifyTime { get; set; }
//            [DbImport, JsonProperty]
//            public virtual int? ModifyUser { get; set; }

//            #endregion
//        }
//        public class UserGameRowCommand
//        {
//            #region Fields

//            [JsonProperty]
//            public virtual int? MemberID { get; set; }
//            [JsonProperty]
//            public virtual int? AgentID { get; set; }
//            [JsonProperty]
//            public virtual GameID? GameID { get; set; }
//            [JsonProperty]
//            public virtual Locked? Locked { get; set; }
//            [JsonProperty]
//            public virtual decimal? Balance { get; set; }
//            [JsonProperty]
//            public virtual string ACNT { get; set; }
//            [JsonProperty]
//            public virtual string Password { get; set; }
//            [JsonProperty]
//            public virtual string Currency { get; set; }

//            #endregion
//        }

//        public static List<UserGameRowData>[] SelectAgentGameRows(int agentID)
//        {
//            if (agentID == 0) return null;
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                AgentRow agent = AgentRow.GetAgent(sqlcmd, agentID, null, null);
//                if (agent == null) return null;
//                List<UserGameRowData>[] rows = new List<UserGameRowData>[] { new List<UserGameRowData>(), new List<UserGameRowData>(), };
//                foreach (GameRow game in GameRow.Cache.GetInstance(null, sqlcmd).Rows)
//                {
//                    web.__game.proc proc = web.__game.GetProc(UserType.Agent, game.ID);
//                    if (proc.IsSupported)
//                        rows[proc.HasAPI ? 0 : 1].Add(proc.SelectUserGameRow(sqlcmd, agent));
//                }
//                return rows;
//            }
//        }
//        public static List<UserGameRowData>[] SelectMemberGameRows(int memberID)
//        {
//            if (memberID == 0) return null;
//            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//            {
//                MemberRow member = MemberRow.GetMember(sqlcmd, memberID, null, null);
//                if (member == null) return null;
//                List<UserGameRowData>[] rows = new List<UserGameRowData>[] { new List<UserGameRowData>(), new List<UserGameRowData>(), };
//                foreach (GameRow game in GameRow.Cache.GetInstance(null, sqlcmd).Rows)
//                {
//                    web.__game.proc proc = web.__game.GetProc(UserType.Member, game.ID);
//                    if (proc.IsSupported)
//                        rows[proc.HasAPI ? 0 : 1].Add(proc.SelectUserGameRow(sqlcmd, member));
//                }
//                return rows;
//            }
//        }

//        static proc[] procs;
//        [DebuggerStepThrough]
//        public static proc GetProc(UserType? userType, GameID? gameID)
//        {
//            lock (typeof(proc))
//            {
//                if (procs == null)
//                {
//                    List<proc> tmp = new List<proc>();
//                    foreach (Type t in typeof(__game).GetNestedTypes(BindingFlags.Public | BindingFlags.Instance))
//                        if ((t.IsAbstract == false) && t.HasInterface<proc>())
//                            tmp.Add((proc)Activator.CreateInstance(t));
//                    procs = tmp.ToArray();
//                }
//                if (userType.HasValue && gameID.HasValue)
//                    for (int i = 0; i < procs.Length; i++)
//                        if ((procs[i].UserType == userType.Value) && (procs[i].GameID == gameID.Value))
//                            return procs[i];
//            }
//            return _null<proc_null>.value;
//        }

//        #region proc

//        public interface proc
//        {
//            bool HasAPI { get; }
//            bool IsSupported { get; }
//            UserType UserType { get; }
//            GameID GameID { get; }
//            UserGameRowData UpdateUserGameRow(string json_s, params object[] args);
//            UserGameRowData SelectUserGameRow(SqlCmd sqlcmd, UserRow user);
//            tran__.GameRowData Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, object command, string json_s, params object[] args);
//            tran__.GameRowData Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, object command, string json_s, params object[] args);
//        }

//        class proc_null : proc
//        {
//            bool proc.HasAPI { get { return false; } }
//            bool proc.IsSupported { get { return false; } }
//            UserType proc.UserType { get { throw err; } }
//            GameID proc.GameID { get { throw err; } }
//            UserGameRowData proc.UpdateUserGameRow(string json_s, params object[] args) { throw err; }
//            UserGameRowData proc.SelectUserGameRow(SqlCmd sqlcmd, UserRow user) { throw err; }
//            tran__.GameRowData proc.Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, object command, string json_s, params object[] args) { throw err; }
//            tran__.GameRowData proc.Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, object command, string json_s, params object[] args) { throw err; }
            
//            static RowException err = new RowException(BU.RowErrorCode.InvalidGameID);
//        }

//        class procAttribute : Attribute
//        {
//            public GameID GameID;
//            public string TableName;
//            public bool HasAPI;
//        }

//        #endregion

//        public abstract class proc<T, TUser, TUserGameRowData, TUserGameRowCommand, TDepositCommand, TWithdrawalCommand> : proc
//            where T : proc<T, TUser, TUserGameRowData, TUserGameRowCommand, TDepositCommand, TWithdrawalCommand>, new()
//            where TUser : UserRow, new()
//            where TUserGameRowData : UserGameRowData, new()
//            where TUserGameRowCommand : UserGameRowCommand, new()
//            where TDepositCommand : tran__.GameDepositRowCommand<TUser, TDepositCommand>, new()
//            where TWithdrawalCommand : tran__.WithdrawalRowCommand<TUser, TWithdrawalCommand>, new()
//        {
//            public proc()
//            {
//                foreach (procAttribute a in this.GetType().GetCustomAttributes(typeof(procAttribute), false))
//                {
//                    this.GameID = a.GameID;
//                    this.m_table_UserGame = a.TableName;
//                    this.HasAPI = a.HasAPI;
//                }
//                if (this.GameID == 0) throw new Exception("參數設定有錯誤!");
//                this.n_RowData = new TUserGameRowData() { GameID = this.GameID };
//            }
//            public bool HasAPI { get; private set; }
//            public virtual bool IsSupported { get { return !string.IsNullOrEmpty(this.m_table_UserGame); } }
//            void _IsSupported() { if (string.IsNullOrEmpty(this.m_table_UserGame)) throw new NotSupportedException(); }

//            public UserType UserType { get { return _null<TUser>.value.UserType; } }
//            public GameID GameID { get; private set; }
//            readonly string m_table_UserGame;
//            public string table_UserGame { get { this._IsSupported(); return this.m_table_UserGame; } }


//            protected abstract TUser GetUser(SqlCmd sqlcmd, int? userID, int? corpID, string acnt, params string[] fields);
//            protected abstract TUser GetUserEx(SqlCmd sqlcmd, int? userID, int? corpID, string acnt, params string[] fields);

//            TUserGameRowData n_RowData;
//            UserGameRowData proc.SelectUserGameRow(SqlCmd sqlcmd, UserRow user) { if (this.IsSupported) return this.SelectUserGameRow(sqlcmd, user as TUser) ?? n_RowData; return null; }
//            protected TUserGameRowData SelectUserGameRow(SqlCmd sqlcmd, TUser user) { return this.SelectUserGameRow(sqlcmd, user.ID); }
//            protected abstract TUserGameRowData SelectUserGameRow(SqlCmd sqlcmd, int? userID);

//            #region 編輯子帳戶
//            UserGameRowData proc.UpdateUserGameRow(string json_s, params object[] args)
//            {
//                this._IsSupported();
//                bool isAgent = this.UserType == BU.UserType.Agent;
//                int? userID;
//                string userID_f;

//                TUserGameRowCommand command = api.DeserializeObject<TUserGameRowCommand>(json_s);
//                if (isAgent)
//                {
//                    userID = command.AgentID;
//                    userID_f = "AgentID";
//                }
//                else
//                {
//                    userID = command.MemberID;
//                    userID_f = "MemberID";
//                }
//                if (!userID.HasValue)
//                    throw new RowException(BU.RowErrorCode.FieldNeeds, userID_f);
//                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//                {
//                    string sqlstr;
//                    sqltool s = new sqltool();
//                    s.Values["userID"] = userID;
//                    s.Values["userID_f"] = (StringEx.sql_str)userID_f;
//                    s.Values["table_usergame"] = (StringEx.sql_str)this.table_UserGame;
//                    TUserGameRowData row = this.SelectUserGameRow(sqlcmd, userID);
//                    if (row == null)
//                    {
//                        if (this.HasAPI) throw new NotSupportedException(); // 有api的平台的子帳戶會自動建立
//                        TUser user = this.GetUserEx(sqlcmd, userID, null, null);
//                        s["*", userID_f, ""] = user.ID;
//                        s["*", "GameID    "] = this.GameID;
//                        s[" ", "Locked    "] = command.Locked;
//                        s[" ", "Balance   "] = command.Balance;
//                        s["*", "ACNT      "] = command.ACNT * text.ValidAsACNT;
//                        s[" ", "pwd       "] = command.Password;
//                        s[" ", "Currency  "] = (text.ValidAsString * command.Currency) ?? user.Currency.ToString();
//                        s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
//                        s.TestFieldNeeds();
//                        sqlstr = s.BuildEx("insert into {table_usergame} (", sqltool._Fields, ") values (", sqltool._Values, @")
//select * from {table_usergame} nolock where GameID={GameID} and {userID_f}={userID}");
//                    }
//                    else
//                    {
//                        s[" ", "Locked", "  ", row.Locked, "  "] = command.Locked;
//                        if (this.HasAPI == false) // 有api的平台不允許修改其他資料
//                        {
//                            s[" ", "Balance", " ", row.Balance, " "] = command.Balance;
//                            s[" ", "ACNT", "    ", row.ACNT, "    "] = text.ValidAsString * command.ACNT;
//                            s[" ", "pwd", "     ", row.Password, ""] = text.ValidAsString * command.Password;
//                            s[" ", "Currency", "", row.Currency, ""] = command.Currency;
//                        }
//                        if (s.fields.Count == 0) return row;
//                        s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
//                        s.Values["GameID"] = this.GameID;
//                        sqlstr = s.BuildEx("update {table_usergame} set ", sqltool._FieldValue, @" where GameID={GameID} and {userID_f}={userID}
//select * from {table_usergame} nolock where GameID={GameID} and {userID_f}={userID}");
//                    }
//                    return sqlcmd.ExecuteEx<TUserGameRowData>(sqlstr);
//                }
//                //return this.UpdateUserGameRow(api.DeserializeObject<TRowCommand>(json_s), json_s, args);
//            }
//            //protected abstract TRowData UpdateUserGameRow(TRowCommand command, string json_s, params object[] args);
//            #endregion

//            [DebuggerStepThrough]
//            tran__.GameRowData proc.Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, object command, string json_s, params object[] args)
//            {
//                this._IsSupported(); return this.Deposit(sqlcmd, tranrow, (TDepositCommand)command, json_s, args);
//            }
//            protected virtual tran__.GameRowData Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, TDepositCommand command, string json_s, params object[] args)
//            {
//                if (command.op_Insert == true) { tranrow = exec_insert<TDepositCommand>(sqlcmd, command, true, true); }
//                if (command.op_Accept == true) { tranrow = exec_Accept(sqlcmd, command, tranrow); }
//                if (command.op_Finish == true) { tranrow = exec_Finish(sqlcmd, command, tranrow); command.op_Delete = true; }
//                if (command.op_Delete == true) { tranrow = exec_Delete(sqlcmd, command, tranrow); }
//                return tranrow;
//            }

//            [DebuggerStepThrough]
//            tran__.GameRowData proc.Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, object command, string json_s, params object[] args)
//            {
//                this._IsSupported(); return this.Withdrawal(sqlcmd, tranrow, (TWithdrawalCommand)command, json_s, args);
//            }
//            protected virtual tran__.GameRowData Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, TWithdrawalCommand command, string json_s, params object[] args)
//            {
//                if (command.op_Insert == true) { tranrow = exec_insert<TWithdrawalCommand>(sqlcmd, command, false, true); }
//                if (command.op_Finish == true) { tranrow = exec_Finish(sqlcmd, command, tranrow); command.op_Delete = true; }
//                if (command.op_Delete == true) { tranrow = exec_Delete(sqlcmd, command, tranrow); }
//                return tranrow;
//            }

//            tran__.GameRowData exec_insert<TRowCommand>(SqlCmd sqlcmd, tran__.RowCommand<TUser, tran__.GameRowData, TRowCommand> command, bool check_locked, bool check_usergame) where TRowCommand : tran__.RowCommand<TUser, tran__.GameRowData, TRowCommand>, new()
//            {
//                TUser user = GetUserEx(sqlcmd, command.UserID, command.CorpID, command.UserACNT, "Name", "ParentID", "Currency", "Locked");
//                if (check_locked && (user.Locked == Locked.Locked)) throw new RowException(user is AgentRow ? RowErrorCode.AgentLocked : RowErrorCode.MemberLocked);

//                AgentRow parent = AgentRow.GetAgentEx(sqlcmd, user.ParentID, null, null, "Name");
//                TUserGameRowData usergame = this.SelectUserGameRow(sqlcmd, user);
//                string currencyB;
//                if (usergame == null)
//                {
//                    if (check_usergame) throw new RowException(RowErrorCode.GameAccountNotFound);
//                    currencyB = user.Currency.ToString();
//                }
//                else
//                {
//                    if (check_locked && (usergame.Locked == Locked.Locked)) throw new RowException(RowErrorCode.GameAccountLocked);
//                    currencyB = usergame.Currency;
//                }
//                if (command.Amount1 <= 0) command.Amount1 = null;
//                sqltool s = new sqltool();
//                s.values["table_tran"] = _null<tran__.GameRowData>.value.Table1;
//                s.values["table_usergame"] = (StringEx.sql_str)this.table_UserGame;
//                s.values["prefix"] = command.prefix;
//                s.values["user_id"] = user.ID;
//                s["*", "LogType   "] = command.LogType;
//                s["*", "GameID    "] = this.GameID;
//                s["*", "UserType  "] = this.UserType;
//                s["*", "CorpID    "] = user.CorpID; // (StringEx.sql_str)"a.CorpID";
//                s["*", "UserID    "] = user.ID; // (StringEx.sql_str)"a.ID";
//                s["*", "UserACNT  "] = user.ACNT; // (StringEx.sql_str)"a.ACNT";
//                s["*", "UserName  "] = user.Name; // (StringEx.sql_str)"a.Name";
//                s["*", "ParentID  "] = parent.ID; // (StringEx.sql_str)"b.ID";
//                s["*", "ParentACNT"] = parent.ACNT; // (StringEx.sql_str)"b.ACNT";
//              //s["*", "ParentName"] = parent.Name;// (StringEx.sql_str)"b.Name";
//                s["*", "Amount1   "] = command.Amount1;
//                s["*", "State     "] = TranState.Initial;
//                s["*", "CurrencyA "] = user.Currency;// (StringEx.sql_str)"a.Currency";
//                s["*", "CurrencyB "] = currencyB;// (StringEx.sql_str)"c.Currency";
//                s["*", "CurrencyX "] = 1;
//                s["*", "RequestIP "] = HttpContext.Current.RequestIP();
//                s.TestFieldNeeds();
//                s.SetUser(sqltool.ModifyUser, sqltool.CreateUser);
//                //" from Agent  a with(nolock) left join Agent b with(nolock) on a.ParentID=b.ID left join {table_usergame} c with(nolock) on a.ID=c.AgentID  where a.ID={user_id}" :
//                //" from Member a with(nolock) left join Agent b with(nolock) on a.AgentID =b.ID left join {table_usergame} c with(nolock) on a.ID=c.MemberID where a.ID={user_id}"
//                string sqlstr = s.BuildEx(@"declare @ID uniqueidentifier, @SerialNumber varchar(16) exec alloc_TranID @ID output, @SerialNumber output, @prefix={prefix}
//insert into {table_tran} (ID,SerialNumber,", sqltool._Fields, @")
//select @ID,@SerialNumber,", sqltool._Values, user is AgentRow ?
//                          " from Agent  a with(nolock) left join Agent b with(nolock) on a.ParentID=b.ID" :
//                          " from Member a with(nolock) left join Agent b with(nolock) on a.AgentID =b.ID",
//                          @" where a.ID={UserID}
//select * from {table_tran} nolock where ID=@ID");
//                return sqlcmd.GetRowEx<tran__.GameRowData>(RowErrorCode.NoResult, sqlstr);
//            }

//            static tran__.GameRowData exec_err(tran__.GameRowData tranrow, string format, params object[] args)
//            {
//                return exec_err(tranrow, string.Format(format, args));
//            }
//            static tran__.GameRowData exec_err(tran__.GameRowData tranrow, string message)
//            {
//                log.message("GameTran", "{0} : {1}\r\n{2}", tranrow.ID, message, api.SerializeObject(tranrow));
//                return tranrow;
//            }

//            protected tran__.GameRowData exec_Accept(SqlCmd sqlcmd, TDepositCommand command, tran__.GameRowData tranrow)
//            {
//                if (tranrow.AcceptTime.HasValue)
//                    return exec_err(tranrow, "Already Accepted");
//                if (tranrow.FinishTime.HasValue)
//                    return exec_err(tranrow, "Already Finished");
//                if (tranrow.LogType != BU.LogType.GameDeposit)
//                    return exec_err(tranrow, "Invaild LogType : {0}", tranrow.LogType);
//                //decimal? prevBalance = sqlcmd.ExecuteScalar<decimal>("select Balance from {TableUser} nolock where ID={UserID}".SqlExport(tranrow));
//                //if (!prevBalance.HasValue)
//                //    throw new RowException(tranrow.UserType == BU.UserType.Agent ? RowErrorCode.AgentNotFound : RowErrorCode.MemberNotFound);
//                //if (prevBalance.Value < tranrow.Amount1) throw new RowException(RowErrorCode.BalanceNotEnough);
//                TranLogRow logrow = null;
//                try
//                {
//                    sqlcmd.BeginTransaction();
//                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"declare @id uniqueidentifier, @b1 decimal(19,6) set @id={ID}
//select @b1=Balance from {Table1} b with(nolock) left join {TableUser} a with(nolock) on a.ID=b.UserID where b.ID=@id
//if @b1 is null return
//update {TableUser} set Balance=Balance-Amount1 from {TableUser} a left join {Table1} b on a.ID=b.UserID where b.ID=@id and FinishTime is null and AcceptTime is null
//update {Table1} set AcceptTime=getdate(),State={State_Accepted} where ID=@id and FinishTime is null and AcceptTime is null
//select b.*, @b1 as PrevBalance, a.Balance from {Table1} b with(nolock) left join {TableUser} a with(nolock) on b.UserID=a.ID where b.ID=@id".SqlExport(tranrow)))
//                    {
//                        tranrow = r.ToObject<tran__.GameRowData>();
//                        logrow = r.ToObject<TranLogRow>();
//                        logrow.TranID = tranrow.ID;
//                        logrow.RequestTime = tranrow.CreateTime;
//                        if (logrow.Balance.Value < 0) throw new RowException(RowErrorCode.BalanceNotEnough);
//                    }
//                    if (logrow == null)
//                        throw new RowException(tranrow.UserType == BU.UserType.Agent ? RowErrorCode.AgentNotFound : RowErrorCode.MemberNotFound);
//                    tranrow.TranLog = TranLogRow.Write(sqlcmd, logrow);
//                    sqlcmd.Commit();
//                    return tranrow;
//                }
//                catch
//                {
//                    sqlcmd.Rollback();
//                    throw;
//                }
//            }

//            protected tran__.GameRowData exec_Finish(SqlCmd sqlcmd, TDepositCommand command, tran__.GameRowData tranrow)
//            {
//                tran__.GameRowData tranrow2 = sqlcmd.ToObject<tran__.GameRowData>(true, @"declare @id uniqueidentifier set @id={ID}
//update {Table1} set FinishTime=getdate(), State={State_Transferred} where ID=@id and FinishTime is null and AcceptTime is not null
//update {Table1} set FinishTime=getdate(), State={State_Rejected} where ID=@id and FinishTime is null and AcceptTime is null
//select * from {Table1} nolock where ID={ID}".SqlExport(tranrow));
//                tranrow2.TranLog = tranrow.TranLog;
//                return tranrow2;
//            }

//            protected tran__.GameRowData exec_Delete(SqlCmd sqlcmd, TDepositCommand command, tran__.GameRowData tranrow)
//            {
//                try
//                {
//                    TranLogRow logrow = null;
//                    sqlcmd.BeginTransaction();
//                    // rollback when needs
//                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"declare @id uniqueidentifier, @b1 decimal(19,6) set @id={ID}
//if (select FinishTime from {Table1} nolock where ID=@id) is not null return
//select @b1=Balance from {Table1} b with(nolock) left join {TableUser} a with(nolock) on a.ID=b.UserID where b.ID=@id
//update {TableUser} set Balance=Balance+Amount1 from {TableUser} a left join {Table1} b on a.ID=b.UserID where b.ID=@id and FinishTime is null and AcceptTime is not null
//update {Table1} set FinishTime=getdate(),State={State_Rejected} where ID=@id and FinishTime is null and AcceptTime is null
//update {Table1} set FinishTime=getdate(),State={State_Failed} where ID=@id and FinishTime is null and AcceptTime is not null
//select b.*, @b1 as PrevBalance, a.Balance from {Table1} b with(nolock) left join {TableUser} a with(nolock) on b.UserID=a.ID where b.ID=@id".SqlExport(tranrow)))
//                    {
//                        tranrow = r.ToObject<tran__.GameRowData>();
//                        logrow = r.ToObject<TranLogRow>();
//                        logrow.TranID = tranrow.ID;
//                        logrow.RequestTime = tranrow.CreateTime;
//                        logrow.LogType = BU.LogType.GameDepositRollback;
//                    }
//                    if (logrow != null)
//                        tranrow.TranLog = TranLogRow.Write(sqlcmd, logrow);
//                    tranrow.delete_row(sqlcmd);
//                    sqlcmd.Commit();
//                    return tranrow;
//                }
//                catch
//                {
//                    sqlcmd.Rollback();
//                    throw;
//                }
//            }

//            protected tran__.GameRowData exec_Finish(SqlCmd sqlcmd, TWithdrawalCommand command, tran__.GameRowData tranrow)
//            {
//                if (tranrow.AcceptTime.HasValue)
//                    return exec_err(tranrow, "Already Accepted");
//                if (tranrow.LogType != BU.LogType.GameWithdrawal)
//                    return exec_err(tranrow, "Invaild LogType : {0}", tranrow.LogType);
//                TranLogRow logrow = null;
//                try
//                {
//                    sqlcmd.BeginTransaction();
//                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"declare @id uniqueidentifier, @b1 decimal(19,6) set @id={ID}
//select @b1=Balance from {Table1} b with(nolock) left join {TableUser} a with(nolock) on a.ID=b.UserID where b.ID=@id
//if @b1 is null return
//update {TableUser} set Balance=Balance+Amount1 from {TableUser} a left join {Table1} b on a.ID=b.UserID where b.ID=@id and FinishTime is null
//update {Table1} set FinishTime=getdate(),State={State_Transferred} where ID=@id and FinishTime is null
//select b.*, @b1 as PrevBalance, a.Balance from {Table1} b with(nolock) left join {TableUser} a with(nolock) on b.UserID=a.ID where b.ID=@id".SqlExport(tranrow)))
//                    {
//                        tranrow = r.ToObject<tran__.GameRowData>();
//                        logrow = r.ToObject<TranLogRow>();
//                        logrow.TranID = tranrow.ID;
//                        logrow.RequestTime = tranrow.CreateTime;
//                    }
//                    if (logrow == null)
//                        throw new RowException(tranrow.UserType == BU.UserType.Agent ? RowErrorCode.AgentNotFound : RowErrorCode.MemberNotFound);
//                    tranrow.TranLog = TranLogRow.Write(sqlcmd, logrow);
//                    sqlcmd.Commit();
//                    return tranrow;
//                }
//                catch
//                {
//                    sqlcmd.Rollback();
//                    throw;
//                }
//            }

//            protected tran__.GameRowData exec_Delete(SqlCmd sqlcmd, TWithdrawalCommand command, tran__.GameRowData tranrow)
//            {
//                try
//                {
//                    sqlcmd.BeginTransaction();
//                    tranrow = sqlcmd.ToObject<tran__.GameRowData>(@"declare @id uniqueidentifier set @id={ID}
//update {Table1} set FinishTime=getdate(),State={State_Rejected} where ID=@id and FinishTime is null
//select * from {Table1} nolock where ID=@id".SqlExport(tranrow));
//                    tranrow.delete_row(sqlcmd);
//                    sqlcmd.Commit();
//                    return tranrow;
//                }
//                catch
//                {
//                    sqlcmd.Rollback();
//                    throw;
//                }
//            }
//        }

//        public abstract class agent<T, TUserRowData, TUserRowCommand, TDepositCommand, TWithdrawalCommand> : proc<T, AgentRow, TUserRowData, TUserRowCommand, TDepositCommand, TWithdrawalCommand>
//            where T : agent<T, TUserRowData, TUserRowCommand, TDepositCommand, TWithdrawalCommand>, new()
//            where TUserRowData : UserGameRowData, new()
//            where TUserRowCommand : UserGameRowCommand, new()
//            where TDepositCommand : tran__.GameDepositRowCommand<AgentRow, TDepositCommand>, new()
//            where TWithdrawalCommand : tran__.WithdrawalRowCommand<AgentRow, TWithdrawalCommand>, new()
//        {
//            protected override AgentRow GetUser(SqlCmd sqlcmd, int? userID, int? corpID, string acnt, params string[] fields)
//            {
//                return AgentRow.GetAgent(sqlcmd, userID, corpID, acnt, fields);
//            }
//            protected override AgentRow GetUserEx(SqlCmd sqlcmd, int? userID, int? corpID, string acnt, params string[] fields)
//            {
//                return AgentRow.GetAgentEx(sqlcmd, userID, corpID, acnt, fields);
//            }
//            protected override TUserRowData SelectUserGameRow(System.Data.SqlClient.SqlCmd sqlcmd, int? userID)
//            {
//                return sqlcmd.ToObject<TUserRowData>("select * from {0} nolock where AgentID={1} and GameID={2}", this.table_UserGame, userID ?? 0, (int)this.GameID);
//            }
//        }

//        public abstract class member<T, TUserRowData, TUserRowCommand, TDepositCommand, TWithdrawalCommand> : proc<T, MemberRow, TUserRowData, TUserRowCommand, TDepositCommand, TWithdrawalCommand>
//            where T : member<T, TUserRowData, TUserRowCommand, TDepositCommand, TWithdrawalCommand>, new()
//            where TUserRowData : UserGameRowData, new()
//            where TUserRowCommand : UserGameRowCommand, new()
//            where TDepositCommand : tran__.GameDepositRowCommand<MemberRow, TDepositCommand>, new()
//            where TWithdrawalCommand : tran__.WithdrawalRowCommand<MemberRow, TWithdrawalCommand>, new()
//        {
//            protected override MemberRow GetUser(SqlCmd sqlcmd, int? userID, int? corpID, string acnt, params string[] fields)
//            {
//                return MemberRow.GetMember(sqlcmd, userID, corpID, acnt, fields);
//            }
//            protected override MemberRow GetUserEx(SqlCmd sqlcmd, int? userID, int? corpID, string acnt, params string[] fields)
//            {
//                return MemberRow.GetMemberEx(sqlcmd, userID, corpID, acnt, fields);
//            }
//            protected override TUserRowData SelectUserGameRow(System.Data.SqlClient.SqlCmd sqlcmd, int? userID)
//            {
//                return sqlcmd.ToObject<TUserRowData>("select * from {0} nolock where MemberID={1} and GameID={2}", this.table_UserGame, userID ?? 0, (int)this.GameID);
//            }
//        }

//        #region AgentGame_HG

//        [proc(TableName = null, HasAPI = false, GameID = GameID.HG)]
//        public class AgentGame_HG : agent<AgentGame_HG, AgentGame_HG.RowData, AgentGame_HG.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }
        
//        #endregion
//        #region AgentGame_EA

//        [proc(TableName = null, HasAPI = false, GameID = GameID.EA)]
//        public class AgentGame_EA : agent<AgentGame_EA, AgentGame_EA.RowData, AgentGame_EA.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_WFT

//        [proc(TableName = null, HasAPI = false, GameID = GameID.WFT)]
//        public class AgentGame_WFT : agent<AgentGame_WFT, AgentGame_WFT.RowData, AgentGame_WFT.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_WFT_SPORTS

//        [proc(TableName = null, HasAPI = false, GameID = GameID.WFT_SPORTS)]
//        public class AgentGame_WFT_SPORTS : agent<AgentGame_WFT_SPORTS, AgentGame_WFT_SPORTS.RowData, AgentGame_WFT_SPORTS.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_KENO

//        [proc(TableName = null, HasAPI = false, GameID = GameID.KENO)]
//        public class AgentGame_KENO : agent<AgentGame_KENO, AgentGame_KENO.RowData, AgentGame_KENO.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_KENO_SSC

//        [proc(TableName = null, HasAPI = false, GameID = GameID.KENO_SSC)]
//        public class AgentGame_KENO_SSC : agent<AgentGame_KENO_SSC, AgentGame_KENO_SSC.RowData, AgentGame_KENO_SSC.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_SUNBET

//        [proc(TableName = "Agent_000", HasAPI = false, GameID = GameID.SUNBET)]
//        public class AgentGame_SUNBET : agent<AgentGame_SUNBET, AgentGame_SUNBET.RowData, AgentGame_SUNBET.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_AG

//        [proc(TableName = "Agent_000", HasAPI = false, GameID = GameID.AG)]
//        public class AgentGame_AG : agent<AgentGame_AG, AgentGame_AG.RowData, AgentGame_AG.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }
        
//        #endregion
//        #region AgentGame_BBIN

//        [proc(TableName = null, HasAPI = false, GameID = GameID.BBIN)]
//        public class AgentGame_BBIN : agent<AgentGame_BBIN, AgentGame_BBIN.RowData, AgentGame_BBIN.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }
        
//        #endregion
//        #region AgentGame_CROWN_SPORTS

//        [proc(TableName = "Agent_000", HasAPI = false, GameID = GameID.CROWN_SPORTS)]
//        public class AgentGame_CROWN_SPORTS : agent<AgentGame_CROWN_SPORTS, AgentGame_CROWN_SPORTS.RowData, AgentGame_CROWN_SPORTS.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }
        
//        #endregion
//        #region AgentGame_AG_AG

//        [proc(TableName = null, HasAPI = false, GameID = GameID.AG_AG)]
//        public class AgentGame_AG_AG : agent<AgentGame_AG_AG, AgentGame_AG_AG.RowData, AgentGame_AG_AG.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_AG_AGIN

//        [proc(TableName = null, HasAPI = false, GameID = GameID.AG_AGIN)]
//        public class AgentGame_AG_AGIN : agent<AgentGame_AG_AGIN, AgentGame_AG_AGIN.RowData, AgentGame_AG_AGIN.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_AG_DSP

//        [proc(TableName = null, HasAPI = false, GameID = GameID.AG_DSP)]
//        public class AgentGame_AG_DSP : agent<AgentGame_AG_DSP, AgentGame_AG_DSP.RowData, AgentGame_AG_DSP.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_SALON

//        [proc(TableName = "Agent_000", HasAPI = false, GameID = GameID.SALON)]
//        public class AgentGame_SALON : agent<AgentGame_SALON, AgentGame_SALON.RowData, AgentGame_SALON.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region AgentGame_EXTRA

//        [proc(TableName = "Agent_000", HasAPI = false, GameID = GameID.EXTRA)]
//        public class AgentGame_EXTRA : agent<AgentGame_EXTRA, AgentGame_EXTRA.RowData, AgentGame_EXTRA.RowCommand, AgentGameDepositRowCommand, AgentGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion

//        #region MemberGame_HG

//        [proc(TableName = "Member_001", HasAPI = true, GameID = GameID.HG)]
//        public class MemberGame_HG : member<MemberGame_HG, MemberGame_HG.RowData, MemberGame_HG.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//            protected override tran__.GameRowData Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameDepositRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//            protected override tran__.GameRowData Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameWithdrawalRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//        }

//        #endregion
//        #region MemberGame_EA

//        [proc(TableName = "Member_002", HasAPI = true, GameID = GameID.EA)]
//        public class MemberGame_EA : member<MemberGame_EA, MemberGame_EA.RowData, MemberGame_EA.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//            protected override tran__.GameRowData Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameDepositRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//            protected override tran__.GameRowData Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameWithdrawalRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//        }

//        #endregion
//        #region MemberGame_WFT

//        public abstract class MemberGame_WFT<T> : member<T, MemberGame_WFT<T>.RowData, MemberGame_WFT<T>.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand> where T : MemberGame_WFT<T>, new()
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//            protected override tran__.GameRowData Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameDepositRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//            protected override tran__.GameRowData Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameWithdrawalRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//        [proc(TableName = "Member_003", HasAPI = true, GameID = GameID.WFT)]
//        public class MemberGame_WFT : MemberGame_WFT<MemberGame_WFT> { }
//        [proc(TableName = "Member_008", HasAPI = true, GameID = GameID.WFT_SPORTS)]
//        public class MemberGame_WFT_SPORTS : MemberGame_WFT<MemberGame_WFT_SPORTS> { }

//        #endregion
//        #region MemberGame_KENO

//        public abstract class MemberGame_KENO<T> : member<T, MemberGame_KENO<T>.RowData, MemberGame_KENO<T>.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand> where T : MemberGame_KENO<T>, new()
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//            protected override tran__.GameRowData Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameDepositRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//            protected override tran__.GameRowData Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameWithdrawalRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//        [proc(TableName = "Member_004", HasAPI = true, GameID = GameID.KENO)]
//        public class MemberGame_KENO : MemberGame_KENO<MemberGame_KENO> { }
//        [proc(TableName = "Member_007", HasAPI = true, GameID = GameID.KENO_SSC)]
//        public class MemberGame_KENO_SSC : MemberGame_KENO<MemberGame_KENO_SSC> { }

//        #endregion
//        #region MemberGame_SUNBET

//        [proc(TableName = "Member_005", HasAPI = false, GameID = GameID.SUNBET)]
//        public class MemberGame_SUNBET : member<MemberGame_SUNBET, MemberGame_SUNBET.RowData, MemberGame_SUNBET.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region MemberGame_AG

//        [proc(TableName = "Member_006", HasAPI = false, GameID = GameID.AG)]
//        public class MemberGame_AG : member<MemberGame_AG, MemberGame_AG.RowData, MemberGame_AG.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region MemberGame_BBIN

//        [proc(TableName = "Member_009", HasAPI = true, GameID = GameID.BBIN)]
//        public class MemberGame_BBIN : member<MemberGame_BBIN, MemberGame_BBIN.RowData, MemberGame_BBIN.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//            protected override tran__.GameRowData Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameDepositRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//            protected override tran__.GameRowData Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameWithdrawalRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//        }

//        #endregion
//        #region MemberGame_CROWN_SPORTS

//        [proc(TableName = "Member_010", HasAPI = false, GameID = GameID.CROWN_SPORTS)]
//        public class MemberGame_CROWN_SPORTS : member<MemberGame_CROWN_SPORTS, MemberGame_CROWN_SPORTS.RowData, MemberGame_CROWN_SPORTS.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region MemberGame_AG

//        public abstract class MemberGame_AG<T> : member<T, MemberGame_AG<T>.RowData, MemberGame_AG<T>.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand> where T : MemberGame_AG<T>, new()
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//            protected override tran__.GameRowData Deposit(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameDepositRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//            protected override tran__.GameRowData Withdrawal(SqlCmd sqlcmd, tran__.GameRowData tranrow, MemberGameWithdrawalRowCommand command, string json_s, params object[] args)
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//        [proc(TableName = "Member_011", HasAPI = true, GameID = GameID.AG_AG)]
//        public class MemberGame_AG_AG : MemberGame_AG<MemberGame_AG_AG> { }
//        [proc(TableName = "Member_012", HasAPI = true, GameID = GameID.AG_AGIN)]
//        public class MemberGame_AG_AGIN : MemberGame_AG<MemberGame_AG_AGIN> { }
//        [proc(TableName = "Member_013", HasAPI = true, GameID = GameID.AG_DSP)]
//        public class MemberGame_AG_DSP : MemberGame_AG<MemberGame_AG_DSP> { }

//        #endregion
//        #region MemberGame_SALON

//        [proc(TableName = "Member_014", HasAPI = false, GameID = GameID.SALON)]
//        public class MemberGame_SALON : member<MemberGame_SALON, MemberGame_SALON.RowData, MemberGame_SALON.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//        #region MemberGame_EXTRA

//        [proc(TableName = "Member_254", HasAPI = false, GameID = GameID.EXTRA)]
//        public class MemberGame_EXTRA : member<MemberGame_EXTRA, MemberGame_EXTRA.RowData, MemberGame_EXTRA.RowCommand, MemberGameDepositRowCommand, MemberGameWithdrawalRowCommand>
//        {
//            public class RowData : UserGameRowData { }
//            public class RowCommand : UserGameRowCommand { }
//        }

//        #endregion
//    }
}
