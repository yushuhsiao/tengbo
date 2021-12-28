using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using Tools;

namespace web
{
    public static partial class game
    {
        public static Dictionary<GameID, string> Names_Active { [DebuggerStepThrough] get { return GameRow.Cache.Instance.Rows2; } }
        public static Dictionary<GameID, string> Names { [DebuggerStepThrough] get { return GameRow.Cache.Instance.All2; } }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        class gameAttribute : Attribute
        {
            public readonly StringEx.sql_str TableName;
            public readonly GameID GameID;
            public readonly bool HasAPI;
            public readonly StringEx.sql_str Field_ID;
            public gameAttribute(string tableName, string field_id, GameID gameID) : this(tableName, field_id, gameID, false) { }
            public gameAttribute(string tableName, string field_id, GameID gameID, bool hasAPI)
            {
                this.TableName = tableName;
                this.Field_ID = field_id;
                this.GameID = gameID;
                this.HasAPI = hasAPI;
            }
        }

        public abstract class UserGameRow
        {
            public abstract int UserID { get; set; }
            public abstract UserType UserType { get; }

            [DbImport, JsonProperty]
            public virtual GameID GameID { get; set; }
            [DbImport, JsonProperty]
            public virtual Locked Locked { get; set; }
            [DbImport, JsonProperty]
            public virtual decimal Balance { get; set; }
            [DbImport, JsonProperty]
            public virtual string ACNT { get; set; }
            [DbImport("pwd"), JsonProperty]
            public virtual string Password { get; set; }
            [DbImport, JsonProperty]
            public virtual string Currency { get; set; }
            [DbImport, JsonProperty]
            public virtual DateTime? GetBalanceTime { get; set; }
            [DbImport, JsonProperty]
            public virtual DateTime CreateTime { get; set; }
            [DbImport, JsonProperty]
            public virtual int CreateUser { get; set; }
            [DbImport, JsonProperty]
            public virtual DateTime ModifyTime { get; set; }
            [DbImport, JsonProperty]
            public virtual int ModifyUser { get; set; }

            internal bool? Register_Success;
        }

        //public abstract class UserGameRowCommand
        //{
        //    //[JsonProperty]
        //    //public virtual int? UserID { get; set; }
        //    //[JsonProperty]
        //    //public virtual GameID? GameID { get; set; }
        //    //[JsonProperty]
        //    //public virtual Locked? Locked { get; set; }
        //    //[JsonProperty]
        //    //public virtual decimal? Balance { get; set; }
        //    //[JsonProperty]
        //    //public virtual string ACNT { get; set; }
        //    //[JsonProperty]
        //    //public virtual string Password { get; set; }
        //    //[JsonProperty]
        //    //public virtual string Currency { get; set; }

        //    //[JsonProperty]
        //    //public bool? op_GetBalance;

        //    static List<UserGameRowCommand> instances = new List<UserGameRowCommand>();
        //    public static UserGameRowCommand GetInstance(UserType? userType, GameID? gameID)
        //    {
        //        lock (instances)
        //        {
        //            if (instances.Count == 0)
        //            {
        //                foreach (Type t in typeof(game).GetNestedTypes())
        //                {
        //                    if (t.IsAbstract) continue;
        //                    if (!typeof(UserGameRowCommand).IsAssignableFrom(t)) continue;
        //                    instances.Add((UserGameRowCommand)Activator.CreateInstance(t));
        //                }
        //            }
        //            if (userType.HasValue && gameID.HasValue)
        //                foreach (UserGameRowCommand cmd in instances)
        //                    if ((cmd.AcceptUserType == userType.Value) && (cmd.AcceptGameID == gameID.Value))
        //                        return cmd;
        //        }
        //        return null;
        //    }

        //    //protected UserRow GetUserEx(SqlCmd sqlcmd, UserType? userType, int? userID, int? corpID, string acnt, params string[] fields)
        //    //{
        //    //    if (userType == BU.UserType.Agent)
        //    //        return AgentRow.GetAgentEx(sqlcmd, userID, corpID, acnt, fields);
        //    //    if (userType == BU.UserType.Member)
        //    //        return MemberRow.GetMemberEx(sqlcmd, userID, corpID, acnt, fields);
        //    //    throw new RowException(BU.RowErrorCode.InvaildUserType);
        //    //}

        //    //internal bool IsStatic { get { lock (instances) return instances.Contains(this); } }
        //    internal abstract UserType AcceptUserType { get; }
        //    internal abstract GameID AcceptGameID { get; }
        //    public abstract bool HasAPI { get; }
        //    public abstract StringEx.sql_str TableName { get; }

        //    //int? corpID;
        //    //public int? GetCorpID(SqlCmd sqlcmd)
        //    //{
        //    //    if (corpID.HasValue)
        //    //        return corpID;
        //    //    using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
        //    //        return this.corpID = GetUserEx(sqlcmd, this.AcceptUserType, this.UserID, null, null).CorpID;
        //    //}

        //    public abstract UserGameRowCommand DeserializeObject(string json);
        //    public abstract UserGameRow _InsertUserRow(SqlCmd sqlcmd, string json_s, params object[] args);
        //    public abstract UserGameRow _UpdateUserRow(SqlCmd sqlcmd, string json_s, params object[] args);
        //    public abstract UserGameRow _SelectUserRow(SqlCmd sqlcmd, int? userID, string gameacnt, bool nullrow);
        //    public abstract void execute(tran.Game.GameDepositRowCommand command);
        //    public abstract void execute(tran.Game.GameWithdrawalRowCommand command);
        //}

        const int reg_id_max = 10;
        static List<IUserGameRowCommand> userGameRowCommand = new List<IUserGameRowCommand>();
        public static IUserGameRowCommand GetUserGameRowCommand(UserType? userType, GameID? gameID, string json, bool use_static)
        {
            lock (userGameRowCommand)
            {
                if (userGameRowCommand.Count == 0)
                {
                    foreach (Type t in typeof(game).GetNestedTypes())
                        if ((t.IsAbstract == false) && t.HasInterface<IUserGameRowCommand>())
                            userGameRowCommand.Add((IUserGameRowCommand)Activator.CreateInstance(t));
                }
                if (userType.HasValue && gameID.HasValue)
                {
                    foreach (IUserGameRowCommand cmd in userGameRowCommand)
                    {
                        if ((cmd.AcceptUserType == userType.Value) && (cmd.AcceptGameID == gameID.Value))
                        {
                            if (use_static) return cmd;
                            if (string.IsNullOrEmpty(json))
                                return (IUserGameRowCommand)Activator.CreateInstance(cmd.GetType());
                            else
                                return (IUserGameRowCommand)api.DeserializeObject(cmd.GetType(), json);
                        }
                    }
                }
            }
            return null;
        }
        
        public interface IUserGameRowCommand
        {
            UserType AcceptUserType { get; }
            GameID AcceptGameID { get; }
            bool HasAPI { get; }
            StringEx.sql_str TableName { get; }

            int? UserID { get; set; }

            //IUserGameRowCommand DeserializeObject(string json);
            //UserGameRow InsertUserRow(SqlCmd sqlcmd, string json_s, params object[] args);
            UserGameRow UpdateUserRow(SqlCmd sqlcmd, string json_s, params object[] args);
            UserGameRow SelectUserRow(SqlCmd sqlcmd, int? userID, string gameacnt, bool nullrow);
            UserGameRow GetBalance(SqlCmd sqlcmd, string json_s, params object[] args);
            void execute(tran.Game.GameDepositRowCommand command);
            void execute(tran.Game.GameWithdrawalRowCommand command);
        }

        public abstract class UserGameRowCommand<TUser, TUserGameRowData, TUserGameRowCommand> : IUserGameRowCommand
            where TUser : web.UserRow, new()
            where TUserGameRowData : game.UserGameRow, new()
            where TUserGameRowCommand : UserGameRowCommand<TUser, TUserGameRowData, TUserGameRowCommand>, new()
        {
            [JsonProperty]
            public virtual int? UserID { get; set; }
            [JsonProperty]
            public virtual GameID? GameID { get; set; }
            [JsonProperty]
            public virtual Locked? Locked { get; set; }
            [JsonProperty]
            public virtual decimal? Balance { get; set; }
            [JsonProperty]
            public virtual string ACNT { get; set; }
            [JsonProperty]
            public virtual string Password { get; set; }
            [JsonProperty]
            public virtual string Currency { get; set; }

            protected UserRow _user;

            //[JsonProperty]
            //public bool? op_GetBalance { get; set; }
            //protected virtual decimal? GetBalance(SqlCmd sqlcmd, TUserGameRowData row, sqltool s, bool throw_error) { return this.Balance; }



            internal bool IsStatic { get { lock (userGameRowCommand) return userGameRowCommand.Contains(this); } }

            static UserGameRowCommand()
            {
                attr = (gameAttribute)typeof(TUserGameRowCommand).GetCustomAttributes(typeof(gameAttribute), false)[0];
            }

            static readonly gameAttribute attr;
            public UserType AcceptUserType
            {
                get { return _null<TUser>.value.UserType; }
            }
            public StringEx.sql_str TableName
            {
                get { return attr.TableName; }
            }
            public GameID AcceptGameID
            {
                get { return attr.GameID; }
            }
            public bool HasAPI
            {
                get { return attr.HasAPI; }
            }
            protected StringEx.sql_str Field_ID
            {
                get { return attr.Field_ID; }
            }

            //IUserGameRowCommand IUserGameRowCommand.DeserializeObject(string json)
            //{
            //    return api.DeserializeObject<TGameRowCommand>(json);
            //}

            //UserGameRow IUserGameRowCommand.InsertUserRow(SqlCmd sqlcmd, string json_s, params object[] args)
            //{
            //    return InsertUserRow(sqlcmd, json_s, args);
            //}
            private TUserGameRowData InsertUserRow(SqlCmd sqlcmd, string json_s, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    web.UserRow user;
                    if (this.AcceptUserType == UserType.Agent)
                        user = AgentRow.GetAgentEx(sqlcmd, this.UserID, null, null, "*");
                    else if (this.AcceptUserType == UserType.Member)
                        user = MemberRow.GetMemberEx(sqlcmd, this.UserID, null, null, "*");
                    else throw new RowException(RowErrorCode.InvaildUserType);
                    sqltool s = new sqltool();
                    s["*", (string)Field_ID, "  "] = user.ID;
                    s["*", "GameID", "          "] = this.GameID;
                    s[" ", "Locked", "          "] = this.Locked ?? BU.Locked.Active;
                    s[" ", "Balance", "         "] = this.Balance;
                    s["*", "ACNT", "            "] = this.ACNT * text.ValidAsGameACNT;
                    s[" ", "pwd", "             "] = text.ValidAsString * this.Password;
                    s[" ", "Currency", "        "] = this.Currency ?? user.Currency.ToString();
                    s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                    s.TestFieldNeeds();
                    s.values["TableName"] = this.TableName;
                    s.values["Field_ID"] = this.Field_ID;
                    s.values["UserID"] = user.ID;
                    return sqlcmd.ExecuteEx<TUserGameRowData>(s.BuildEx2("insert into {TableName} (", sqltool._Fields, ") values (", sqltool._Values, @")
select * from {TableName} nolock where {Field_ID}={UserID} and GameID={GameID}"));
                }
            }

            [DebuggerStepThrough]
            UserGameRow IUserGameRowCommand.UpdateUserRow(SqlCmd sqlcmd, string json_s, params object[] args)
            {
                return UpdateUserRow(sqlcmd, json_s, args);
            }
            public virtual TUserGameRowData UpdateUserRow(SqlCmd sqlcmd, string json_s, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    SqlSchemaTable schema = SqlSchemaTable.GetSchema(sqlcmd, this.TableName.value);
                    TUserGameRowData row = this.SelectUserRow(sqlcmd, this.UserID, null, false);
                    if (row == null)
                    {
                        if (this.HasAPI)
                            throw new RowException(RowErrorCode.GameAccountNotFound);
                        return InsertUserRow(sqlcmd, json_s, args);
                    }
                    sqltool s = new sqltool();
                    //if (op_GetBalance == true)
                    //    this.Balance = this.GetBalance(sqlcmd, row, s, true);
                    s[" ", "Balance", " ", row.Balance, " "] = this.Balance;
                    s[" ", "Locked", "  ", row.Locked, "  "] = this.Locked;
                    s[" ", "ACNT", "    ", row.ACNT, "    "] = text.ValidAsString * this.ACNT;
                    s[" ", "pwd", "     ", row.Password, ""] = text.ValidAsString * this.Password;
                    s[" ", "Currency", "", row.Currency, ""] = this.Currency;
                    if (s.fields.Count == 0) return row;
                    //if (s.fields.Contains("Balance"))
                    //    s[schema, "", "GetBalanceTime", ""] = StringEx.sql_str.getdate;
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.values["TableName"] = this.TableName;
                    s.values["Field_ID"] = this.Field_ID;
                    s.values["UserID"] = row.UserID;
                    s.values["GameID"] = this.GameID;
                    return sqlcmd.ExecuteEx<TUserGameRowData>(s.BuildEx2("update {TableName} set ", sqltool._FieldValue, @" where {Field_ID}={UserID} and GameID={GameID}
select * from {TableName} nolock where {Field_ID}={UserID} and GameID={GameID}"));
                }
            }

            [DebuggerStepThrough]
            UserGameRow IUserGameRowCommand.SelectUserRow(SqlCmd sqlcmd, int? userID, string gameacnt, bool nullrow)
            {
                return SelectUserRow(sqlcmd, userID, gameacnt, nullrow);
            }
            public TUserGameRowData SelectUserRow(SqlCmd sqlcmd, int? userID, string gameacnt, bool nullrow)
            {
                TUserGameRowData row = null;
                if (userID.HasValue)
                    row = sqlcmd.ToObject<TUserGameRowData>(string.Format("select * from {{TableName}} nolock where {{Field_ID}}={0} and GameID={{AcceptGameID}} and Locked in (0,1)", userID).SqlExport(this));
                else if (gameacnt != null)
                {
                    string acnt = gameacnt * text.ValidAsGameACNT;
                    if (!string.IsNullOrEmpty(acnt))
                        row = sqlcmd.ToObject<TUserGameRowData>(string.Format("select * from {{TableName}} nolock where ACNT='{0}' and GameID={{AcceptGameID}} and Locked in (0,1)", acnt).SqlExport(this));
                }
                if ((row == null) && nullrow)
                    row = new TUserGameRowData() { GameID = this.AcceptGameID, UserID = userID ?? 0 };
                return row;
            }

            //[DebuggerStepThrough]
            //protected virtual TUserGameRowData Register<TRowCommand>(tran.Game.GameRowCommand<TRowCommand> command) where TRowCommand : tran.Game.GameRowCommand<TRowCommand>, new()
            //{
            //    throw new NotImplementedException();
            //}

            [DebuggerStepThrough]
            protected virtual TUserGameRowData Register(SqlCmd sqlcmd, UserRow _user, GameID gameID)
            {
                throw new NotImplementedException();
            }

            protected IEnumerable<TUserGameRowData> Register(SqlCmd sqlcmd, sqltool s, string prefix, string acnt)
            {
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                s.values["TableName"] = this.TableName;
                s.values["Field_ID"] = this.Field_ID;
                s.values["UserID"] = s.values[this.Field_ID.value];
                for (int id = 0; id < reg_id_max; id++)
                {
                    TUserGameRowData row;
                    try
                    {
                        s["", "ACNT", ""] = string.Format("{0}{1}{2}", prefix, acnt, id == 0 ? "" : id.ToString());
                        sqlcmd.BeginTransaction();
                        row = sqlcmd.ToObject<TUserGameRowData>(s.BuildEx2(
                            "delete {TableName} where GameID={GameID} and {Field_ID}={UserID} and Locked=2 ",
                            "select * from {TableName} nolock where GameID={GameID} and {Field_ID}={UserID}"));
                        if (row == null)
                        {
                            int cnt = sqlcmd.ExecuteScalar<int>(s.BuildEx2("select count(*) from {TableName} nolock where GameID={GameID} and ACNT={ACNT}")) ?? 0;
                            if (cnt == 0)
                                row = sqlcmd.ToObject<TUserGameRowData>(s.BuildEx2(
                                    "insert into {TableName} (", sqltool._Fields, ") values (", sqltool._Values, ") ",
                                    "select * from {TableName} nolock where GameID={GameID} and {Field_ID}={UserID}"));
                        }
                        sqlcmd.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlcmd.Rollback();
                        log.error(ex);
                        row = null;
                    }
                    if (row == null) continue;
                    yield return row;
                    if (row.Register_Success == true)
                    {
                        row = sqlcmd.ToObject<TUserGameRowData>(true, s.BuildEx2(
                            "update {TableName} set Locked=0 where GameID={GameID} and {Field_ID}={UserID} ",
                            "select * from {TableName} nolock where GameID={GameID} and {Field_ID}={UserID}"));
                        row.Register_Success = true;
                        yield return row;
                    }
                }
            }

            [DebuggerStepThrough]
            UserGameRow IUserGameRowCommand.GetBalance(SqlCmd sqlcmd, string json_s, params object[] args)
            {
                return this.GetBalance(sqlcmd, null, true);
            }
            protected virtual decimal? OnGetBalance(SqlCmd sqlcmd, TUserGameRowData row, bool throw_error) { return this.Balance; }
            protected virtual TUserGameRowData GetBalance(SqlCmd sqlcmd, TUserGameRowData row, bool throw_error)
            {
                if (this.IsStatic)
                    throw new RowException(RowErrorCode.SystemError, "IsStatic");
                if (this.HasAPI == false)
                    throw new NotSupportedException();
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    row = row ?? this.SelectUserRow(sqlcmd, this.UserID, null, false);
                    if (row == null)
                        throw new RowException(RowErrorCode.GameAccountNotFound);
                    this.Balance = this.OnGetBalance(sqlcmd, row, throw_error) ?? row.Balance;
                    if (row.Balance == this.Balance) return row;
                    sqlcmd.ExecuteNonQuery("update {TableName} set Balance={Balance}, GetBalanceTime=getdate() where {Field_ID}={UserID} and GameID={AcceptGameID}".SqlExport(this));
                    return this.SelectUserRow(sqlcmd, this.UserID, null, false);
                }
            }



            protected IEnumerable<TUserGameRowData> tran_insert<TRowCommand>(tran.Game.GameRowCommand<TRowCommand> command, bool register) where TRowCommand : tran.Game.GameRowCommand<TRowCommand>, new()
            {
                if (command.prefix == null) throw new RowException(RowErrorCode.InvaildLogType);
                command._user = command.GetUserEx(command.UserType, command.UserID, command.CorpID, command.UserACNT, "*");
                TUserGameRowData _usergame = this.SelectUserRow(command.sqlcmd, this.UserID = command.UserID = command._user.ID, null, false);
                if ((_usergame == null) && register)
                    _usergame = this.Register(command.sqlcmd, command._user, command.GameID.Value);
                command._usergame = _usergame;
                this._user = command._user;
                if (_usergame != null)
                    yield return _usergame;
            }

            public virtual void execute(tran.Game.GameDepositRowCommand command)
            {
                if (command.op_Insert == true) foreach (TUserGameRowData _usergame in tran_insert(command, false)) command.insert();
                if (command.op_Update == true) command.update();
                if (command.op_Accept == true) command.accept();
                if (command.op_Finish == true) command.finish();
                if (command.op_Delete == true) command.delete();
            }

            public virtual void execute(tran.Game.GameWithdrawalRowCommand command)
            {
                if (command.op_Insert == true) foreach (TUserGameRowData _usergame in tran_insert(command, false)) command.insert();
                if (command.op_Update == true) command.update();
                if (command.op_Finish == true) command.finish();
                if (command.op_Delete == true) command.delete();
            }
        }

        const int tranid_len = 18;
        static string GetTranID(tran.Game.GameRowData row, string prefix, int cnt)
        {
            StringBuilder s = new StringBuilder();
            if (cnt > 99) cnt = 99;
            if (cnt < 0) cnt = 0;
            s.AppendFormat("{0:yyyyMMdd}{1}{2:00}", row.CreateTime, row.SerialNumber.Substring(prefix.Length), cnt);
            while (s.Length > tranid_len) s.Remove(8, 1);
            while (s.Length < tranid_len) s.Insert(8, '0');
            return s.ToString();
        }
    }

    partial class game
    {
        [DebuggerStepThrough]
        public abstract class Agent_XXX : game.UserGameRow
        {
            [DbImport("AgentID"), JsonProperty]
            public override int UserID
            {
                get;
                set;
            }
            public override UserType UserType
            {
                get { return BU.UserType.Agent; }
            }
        }

        public class Agent_000 : Agent_XXX { }

        public abstract class AgentRowCommand<T> : game.UserGameRowCommand<web.AgentRow, game.Agent_000, T> where T : AgentRowCommand<T>, new() { }

        [game("Agent_000", "AgentID", BU.GameID.SUNBET)]
        public class AgentRowCommand_SUNBET : game.AgentRowCommand<AgentRowCommand_SUNBET> { }

        [game("Agent_000", "AgentID", BU.GameID.AG)]
        public class AgentRowCommand_AG : game.AgentRowCommand<AgentRowCommand_AG> { }

        [game("Agent_000", "AgentID", BU.GameID.CROWN_SPORTS)]
        public class AgentRowCommand_CROWN_SPORTS : game.AgentRowCommand<AgentRowCommand_CROWN_SPORTS> { }

        [game("Agent_000", "AgentID", BU.GameID.SALON)]
        public class AgentRowCommand_SALON : game.AgentRowCommand<AgentRowCommand_SALON> { }

        [game("Agent_000", "AgentID", BU.GameID.EXTRA)]
        public class AgentRowCommand_EXTRA : game.AgentRowCommand<AgentRowCommand_EXTRA> { }
    }

    partial class game
    {
        [DebuggerStepThrough]
        public abstract class Member_XXX : game.UserGameRow
        {
            [DbImport("MemberID"), JsonProperty]
            public override int UserID
            {
                get;
                set;
            }
            public override UserType UserType
            {
                get { return BU.UserType.Member; }
            }
        }

        public class Member_000 : game.Member_XXX { }

        public abstract class MemberGameRowCommand<T> : game.UserGameRowCommand<web.MemberRow, Member_000, T> where T : MemberGameRowCommand<T>, new() { }

        [game("Member_005", "MemberID", BU.GameID.SUNBET)]
        public class MemberRowCommand_SUNBET : game.MemberGameRowCommand<MemberRowCommand_SUNBET> { }

        [game("Member_006", "MemberID", BU.GameID.AG)]
        public class MemberRowCommand_AG : game.MemberGameRowCommand<MemberRowCommand_AG> { }

        [game("Member_010", "MemberID", BU.GameID.CROWN_SPORTS)]
        public class MemberRowCommand_CROWN_SPORTS : game.MemberGameRowCommand<MemberRowCommand_CROWN_SPORTS> { }

        [game("Member_014", "MemberID", BU.GameID.SALON)]
        public class MemberRowCommand_SALON : game.MemberGameRowCommand<MemberRowCommand_SALON> { }

        [game("Member_254", "MemberID", BU.GameID.EXTRA)]
        public class MemberRowCommand_EXTRA : game.MemberGameRowCommand<MemberRowCommand_EXTRA> { }
    }
}