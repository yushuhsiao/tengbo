using BU;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Tools;

namespace web
{
    public abstract class User : IPrincipal, IIdentity
    {
        public abstract UserType UserType { get; }
        public Permissions Permissions { get; internal set; }
        public api.CommandQueue CommandQueue { get; internal set; }
        [DbImport]
        public int ID { get; set; }
        [DbImport]
        public Guid? LoginID { get; set; }
        [DbImport]
        public virtual int CorpID { get; set; }
        [DbImport]
        public virtual string ACNT { get; set; }
        [DbImport]
        public virtual byte GroupID { get; set; }
        [DbImport]
        public virtual string Name { get; set; }
        [DbImport]
        public virtual DateTime UpdateTime { get; set; }
        [DbImport]
        public virtual DateTime ExpireTime { get; set; }
        [DbImport]
        public virtual DateTime LoginTime { get; set; }
        [DbImport]
        public virtual DateTime KickTime { get; set; }
        public virtual string SessionID { get; set; }
        [DbImport("IP")]
        public virtual string LoginIP { get; set; }
        public User()
        {
            this.Permissions = _null<Permissions>.value;
            this.CommandQueue = api.CommandQueue.Global;
        }

        public bool IsExipredOrKicked
        {
            get
            {
                if (this.KickTime > this.LoginTime)
                    return true;
                if (this.ExpireTime < DateTime.Now)
                    return true;
                return false;
            }
        }

        #region IPrincipal 成員

        IIdentity IPrincipal.Identity
        {
            get { return this; }
        }

        bool IPrincipal.IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IIdentity 成員

        string IIdentity.AuthenticationType
        {
            get { return "Forms"; }
        }

        bool IIdentity.IsAuthenticated
        {
            get { return !(this.UserType == BU.UserType.Guest); }
        }

        #endregion

        public static void UserLogout()
        {
            User user = HttpContext.Current.User as User;
            if (user != null)
                user.Logout(null);
        }
        public virtual void Logout(SqlCmd sqlcmd) { }
        internal protected virtual void OnUserInit(SqlCmd sqlcmd) { }

        public static int CurrentUserID
        {
            get
            {
                User user = HttpContext.Current.User as User;
                if (user != null)
                    return user.ID;
                else
                    return 0;
            }
        }
    }

    public abstract class User<TUser, TUserRow> : User
        where TUser : User<TUser, TUserRow>, new()
        where TUserRow : UserRow, new()
    {
        static TUser _instance = new TUser();
        protected abstract string _sql_table { get; }
        public static UserType _UserType { get { return _instance.UserType; } }

        public static class UserList
        {
            static string _key = typeof(User).FullName;

            public static TUser UserLogin(HttpContext context, SqlCmd sqlcmd, TUserRow row, DateTime? sqltime, string json)
            {
                TUser user = new TUser()
                {
                    ID = row.ID.Value,
                    LoginIP = context.RequestIP(),
                    CorpID = row.CorpID.Value,
                    GroupID = row.GroupID.Value,
                    ACNT = row.ACNT,
                    SessionID = context.Session.SessionID,
                    //UserPassword = JObject.Parse(json).Value<string>("n2") ?? JObject.Parse(json)["MemberRegister"].Value<string>("pwd2"),    //登录密码或者注册密码
                };
                try
                {
                    StringBuilder sql = new StringBuilder("declare @ct datetime, @cte datetime ");
                    if (sqltime.HasValue)
                        sql.AppendFormat(@"set @ct='{0}'", sqltime.Value.ToString(sqltool.DateTimeFormat));
                    else
                        sql.AppendFormat(@"set @ct=getdate()");
                    sql.AppendFormat(" set @cte=dateadd(ms, {0}, @ct)", _Global.sessionStateSection.Timeout.TotalMilliseconds);
                    sql.Append(@"
update LoginState set SessionID=null where (SessionID={SessionID}) or (ID={ID})
if exists (select ID from LoginState nolock where ID={ID})
update LoginState set UserType={UserType},LoginID=newid(),IP={LoginIP},SessionID={SessionID},CorpID={CorpID},GroupID={GroupID},ACNT={ACNT},ExpireTime=@cte,LoginTime=@ct,KickTime=@ct, [Count]=isnull([Count],1)+1 where ID={ID}
else
insert into LoginState (ID, UserType,LoginID,IP,SessionID,CorpID,GroupID,ACNT,ExpireTime,LoginTime,KickTime,[Count])
values ({ID},{UserType},newid(),{LoginIP},{SessionID},{CorpID},{GroupID},{ACNT},@cte,@ct,@ct, 1)
select * from LoginState nolock where ID={ID}");
                    string sql2 = sql.ToString().SqlExport(null, user);
                    sqlcmd.BeginTransaction();
                    sqlcmd.FillObject(user, sql.ToString().SqlExport(null, user));
                    sqlcmd.Commit();
                }
                catch (Exception ex)
                {
                    sqlcmd.Rollback();
                    throw new RowException(ex, RowErrorCode.Unknown, null);
                }
                //user.UserInit(sqlcmd);
                SetCurrentUser(context, sqlcmd, user);
                WriteLoginLog(sqlcmd, user.ID, user.UserType, user.CorpID, user.ACNT, sqltime, user.LoginIP, RowErrorCode.Successed, null, json);
                return user;
            }

            public static void UserLogout(HttpContext context, SqlCmd sqlcmd, User user)
            {
                if (user == null) return;
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    SetCurrentUser(context, sqlcmd, null);
                    RemoveUser(_Global.GetUserList(context), user);
                    try
                    {
                        sqlcmd.BeginTransaction();
                        sqlcmd.ExecuteNonQuery("update LoginState set ExpireTime=LoginTime, SessionID=null where ID={0}", user.ID);
                        sqlcmd.Commit();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                        throw;
                    }
                }
            }

            static void RemoveUser(List<User> list, User user )
            {
                lock (list)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                        if ((list[i].SessionID == user.SessionID) || (list[i].ID == user.ID))
                            list.RemoveAt(i);
                    while (list.Remove(user)) { }
                }
            }

            static User SetCurrentUser(HttpContext context, SqlCmd sqlcmd, User user)
            {
                user = user ?? _null<Guest>.value;
                if (context.Session != null)
                {
                    context.Session[typeof(User).FullName] = user;
                    if (user is Guest) return user;
                    user.OnUserInit(sqlcmd);
                    user.CommandQueue = new api.CommandQueue();
                    user.Permissions = new Permissions();
                    user.Permissions.Load(sqlcmd, user.UserType, user.CorpID, user.GroupID, user.ID);

                    List<User> list = _Global.GetUserList(context);
                    lock (list)
                    {
                        RemoveUser(list, user);
                        list.Add(user);
                    }
                }
                return user;
            }

            static User GetUser(HttpContext context)
            {
                if (context.Session != null)
                {
                    User user1 = context.Session[typeof(User).FullName] as User;
                    if (user1 == null)
                    {
                        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                        {
                            TUser user2 = null;
                            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select getdate() as CurrentTime, * from LoginState nolock where UserType={0} and SessionID='{1}'", (byte)_instance.UserType, context.Session.SessionID))
                                if (user2 == null)
                                    user2 = r.ToObject<TUser>();
                                else
                                    return SetCurrentUser(context, sqlcmd, null);
                            if (user2 != null)
                                if (!user2.IsExipredOrKicked)
                                    return SetCurrentUser(context, sqlcmd, user2);
                        }
                    }
                    else
                    {
                        if (user1 is Guest)
                            return user1;
                        if (user1.IsExipredOrKicked == false)
                        {
                            List<User> list = _Global.GetUserList(context);
                            lock (list)
                            {
                                foreach (User user2 in list)
                                    if ((user2.SessionID == user1.SessionID) && (user2.ID == user1.ID))
                                        return user2;
                            }
                        }
                    }
                }
                return SetCurrentUser(context, null, null);
            }

            [DebuggerStepThrough]
            public static void AcquireRequestState(HttpContext context)
            {
                #region 語系切換
                if (context.Request.UserLanguages != null)
                {
                    try
                    {
                        foreach (string s in context.Request.UserLanguages)
                        {
                            try
                            {
                                CultureInfo c = CultureInfo.GetCultureInfoByIetfLanguageTag(s);
                                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = c;
                                break;
                            }
                            catch { }
                        }
                        //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture
                    }
                    catch { }
                }
                #endregion
                context.User = GetUser(context);
            }

            public static void WriteLoginLog(SqlCmd sqlcmd, int? id, UserType? userType, int? corpID, string acnt, DateTime? time, string ip, RowErrorCode? result, string message, string json)
            {
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    try
                    {
                        sqlcmd.BeginTransaction();
                        sqltool s = new sqltool();
                        object _time;
                        if (time.HasValue) _time = time;
                        else _time = StringEx.sql_str.getdate;
                        s["", "ID", "       "] = id;
                        s["", "UserType", " "] = (byte?)userType;
                        s["", "CorpID", "   "] = corpID;
                        s["", "ACNT", "     "] = text.ValidAsACNT * acnt;
                        s["", "LoginTime", ""] = _time;
                        s["", "LoginIP", "  "] = ip ?? HttpContext.Current.RequestIP();
                        s["", "Result", "   "] = result ?? RowErrorCode.Unknown;
                        s["", "Message", "  "] = message;
                        s["", "json", "     "] = text.ValidAsString * json;
                        sqlcmd.ExecuteNonQuery(s.BuildEx(@"insert into LoginLog (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                        sqlcmd.Commit();
                    }
                    catch
                    {
                        sqlcmd.Rollback();
                    }
                }
            }
        }

        public static TUser Login(SqlCmd sqlcmd, string json, int? corpID, string acnt, string password, out TUserRow row)
        {
            HttpContext context = HttpContext.Current;
            row = null;
            DateTime sqltime = DateTime.Now;
            using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
            {
                try
                {
                    corpID = corpID ?? _Global.DefaultCorpID;
                    acnt *= text.ValidAsACNT2;
                    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select getdate() as ct,ID,CorpID,ACNT,GroupID,Name,pwd,Locked from [{0}] nolock where CorpID={1} and ACNT='{2}'"
                        , _instance._sql_table, corpID, acnt))
                    {
                        sqltime = r.GetDateTime("ct");
                        row = r.ToObject<TUserRow>();
                    }
                    if ((row == null) || !row.ID.HasValue)
                        throw new RowException(RowErrorCode.AccountNotExist);
                    if (row.ID == _Global.RootAdminID) row.Locked = Locked.Active; else row.Locked = row.Locked ?? Locked.Active;
                    if (row.Locked.Value == Locked.Locked)
                        throw new RowException(RowErrorCode.AccountLocked);
                    if (text.EncodePassword(row.ACNT, password) != row.Password)
                        throw new RowException(RowErrorCode.PasswordError);
                    return UserList.UserLogin(context, sqlcmd, row, sqltime, json);
                }
                catch (RowException ex)
                {
                    if (row == null)
                        UserList.WriteLoginLog(sqlcmd, null, _instance.UserType, corpID, acnt, null, context.RequestIP(), ex.Status, ex.Message, json);
                    else
                        UserList.WriteLoginLog(sqlcmd, row.ID, _instance.UserType, row.CorpID, row.ACNT, sqltime, context.RequestIP(), ex.Status, ex.Message, json);
                    throw ex;
                }
            }
        }

        public override void Logout(SqlCmd sqlcmd)
        {
            UserList.UserLogout(HttpContext.Current, sqlcmd, this);
        }

        #region //

//        public static void setCurrentUser(HttpContext context, User value)
//        {
//            context.User = value ?? Tools._null<Guest>.value;
//            context.Session[typeof(TUser).FullName] = value;
//        }

//        public static void AcquireRequestState(HttpContext context)
//        {
//            #region 語系切換
//            if (context.Request.UserLanguages != null)
//            {
//                try
//                {
//                    foreach (string s in context.Request.UserLanguages)
//                    {
//                        try
//                        {
//                            CultureInfo c = CultureInfo.GetCultureInfo(s);
//                            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = c;
//                            break;
//                        }
//                        catch { }
//                    }
//                    //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture
//                }
//                catch { }
//            }
//            #endregion
//            if (context.Session == null)
//                context.User = Tools._null<Guest>.value;
//            else
//            {
//                User user1 = context.Session[typeof(TUser).FullName] as User;
//                TUser user2 = user1 as TUser;
//                if (user2 == null)
//                {
//                    #region resume user
//                    using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//                    {
//                        foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select getdate() as CurrentTime, * from LoginState nolock where UserType={0} and SessionID='{1}'", (byte)_instance.UserType, context.Session.SessionID))
//                        {
//                            if (user2 == null)
//                            {
//                                user2 = r.ToObject<TUser>();
//                                if (user2.IsExipredOrKicked)
//                                    user2 = null;
//                            }
//                            else
//                            {
//                                user2 = null;
//                                break;
//                            }
//                        }
//                        if (user2 != null)
//                        {
//                            user1 = user2;
//                            user2.UserInit(sqlcmd);
//                        }
//                    }
//                    #endregion
//                }
//                else
//                {
//                    if (user2.IsExipredOrKicked)
//                        user2 = null;
//                }
//                setCurrentUser(context, user1 ?? new Guest());
//            }
//        }

//        public class UserList : List<TUser>
//        {
//            private UserList() { }
//            static UserList GetInstance(HttpContext context)
//            {
//                try
//                {
//                    context.Application.Lock();
//                    lock (context.Application)
//                    {
//                        string name = typeof(UserList).FullName;
//                        UserList list = context.Application.Get(name) as UserList;
//                        if (list == null) context.Application.Set(name, list = new UserList());
//                        return list;
//                    }
//                }
//                finally
//                {
//                    context.Application.UnLock();
//                }
//            }

//            public void GetUser(int memberID)
//            {
//            }
//            public void GetUser(Guid loginID)
//            {
//            }
//            public void GetUser(HttpContext context)
//            {
//            }


//            public static TUser UserLogin(HttpContext context, SqlCmd sqlcmd, TUserRow row, DateTime? sqltime, string json)
//            {
//                TUser user = new TUser()
//                {
//                    ID = row.ID.Value,
//                    LoginIP = context.RequestIP(),
//                    CorpID = row.CorpID.Value,
//                    GroupID = row.GroupID.Value,
//                    ACNT = row.ACNT,
//                    SessionID = context.Session.SessionID,
//                };
//                try
//                {
//                    StringBuilder sql = new StringBuilder("declare @ct datetime, @cte datetime ");
//                    if (sqltime.HasValue)
//                        sql.AppendFormat(@"set @ct='{0}'", sqltime.Value.ToString(sqltool.DateTimeFormat));
//                    else
//                        sql.AppendFormat(@"set @ct=getdate()");
//                    sql.AppendFormat(" set @cte=dateadd(ms, {0}, @ct)", _Global.sessionStateSection.Timeout.TotalMilliseconds);
//                    sql.Append(@"
//        update LoginState set SessionID=null where (SessionID={SessionID}) or (ID={ID})
//        if exists (select ID from LoginState nolock where ID={ID})
//        update LoginState set UserType={UserType},LoginID=newid(),IP={LoginIP},SessionID={SessionID},CorpID={CorpID},GroupID={GroupID},ACNT={ACNT},ExpireTime=@cte,LoginTime=@ct,KickTime=@ct, [Count]=isnull([Count],1)+1 where ID={ID}
//        else
//        insert into LoginState (ID, UserType,LoginID,IP,SessionID,CorpID,GroupID,ACNT,ExpireTime,LoginTime,KickTime,[Count])
//        values ({ID},{UserType},newid(),{LoginIP},{SessionID},{CorpID},{GroupID},{ACNT},@cte,@ct,@ct, 1)
//        select * from LoginState nolock where ID={ID}");
//                    string sql2 = sql.ToString().SqlExport(null, user);
//                    sqlcmd.BeginTransaction();
//                    sqlcmd.FillObject(user, sql.ToString().SqlExport(null, user));
//                    sqlcmd.Commit();
//                }
//                catch (Exception ex)
//                {
//                    sqlcmd.Rollback();
//                    throw new RowException(ex, RowErrorCode.Unknown, null);
//                }
//                UserList list = UserList.GetInstance(context);
//                lock (list)
//                {
//                    list.RemoveUser(user);
//                    list.Add(user);
//                }
//                user.UserInit(sqlcmd);
//                WriteLoginLog(sqlcmd, user.ID, user.CorpID, user.ACNT, sqltime, user.LoginIP, RowErrorCode.Successed, null, json);
//                return user;
//            }

//            public static TUser UserLogout(HttpContext context, SqlCmd sqlcmd, TUser user)
//            {
//                if (user == null) return user;
//                UserList.GetInstance(context).RemoveUser(user);
//                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
//                {
//                    try
//                    {
//                        sqlcmd.BeginTransaction();
//                        sqlcmd.ExecuteNonQuery("update LoginState set ExpireTime=LoginTime, SessionID=null where ID={0}", user.ID);
//                        sqlcmd.Commit();
//                    }
//                    catch
//                    {
//                        sqlcmd.Rollback();
//                        throw;
//                    }
//                }
//                return user;
//            }

//            TUser RemoveUser(TUser user)
//            {
//                if (user == null) return user;
//                lock (this)
//                {
//                    for (int i = base.Count - 1; i >= 0; i--)
//                        if ((base[i].SessionID == user.SessionID) || (base[i].ID == user.ID))
//                            base.RemoveAt(i);
//                    while (base.Remove(user)) { }
//                    return user;
//                }
//            }
//        }

//        static User _CurrentUser
//        {
//            get { return HttpContext.Current.Session[typeof(TUser).FullName] as User; }
//            set { HttpContext.Current.Session[typeof(TUser).FullName] = value; }
//        }

//        public static TUser Login(HttpContext context, SqlCmd sqlcmd, TUserRow row, DateTime? sqltime, string json)
//        {
//            TUser user; // = new TUser() { ID = row.ID.Value, CorpID = row.CorpID.Value, GroupID = row.GroupID.Value, ACNT = row.ACNT, Name = row.Name, };
//            string ip = context.RequestIP();
//            try
//            {
//                sqltool s = new sqltool();
//                s["", "ID", "        "] = row.ID.Value;
//                s["", "UserType", "  "] = _instance.UserType;
//                s["", "LoginID", "   "] = (StringEx.sql_str)"newid()";
//                s["", "IP", "        "] = ip;
//                s["", "SessionID", " "] = context.Session.SessionID;
//                s["", "CorpID", "    "] = row.CorpID.Value;
//                s["", "GroupID", "   "] = row.GroupID.Value;
//                s["", "ACNT", "      "] = row.ACNT;
//                s["", "ClassName", " "] = typeof(TUser).AssemblyQualifiedName;
//                s["", "ExpireTime", ""] = (StringEx.sql_str)string.Format("dateadd(ms, {0}, @ct)", _Global.sessionStateSection.Timeout.TotalMilliseconds);
//                s["", "LoginTime", " "] = (StringEx.sql_str)"@ct";
//                s["", "KickTime", "  "] = (StringEx.sql_str)"@ct";
//                if (sqltime.HasValue)
//                    s.Values["ct"] = sqltime.Value.ToString(sqltool.DateTimeFormat);
//                else
//                    s.Values["ct"] = (StringEx.sql_str)"getdate()";
//                sqlcmd.BeginTransaction();
//                user = sqlcmd.ToObject<TUser>(s.BuildEx(@"declare @ct datetime set @ct={ct} delete LoginState where ID={ID}
//update LoginState set SessionID='' where (SessionID={SessionID}) or (ID={ID})
//insert into LoginState (", sqltool._Fields, ") values (", sqltool._Values, @")
//select * from LoginState nolock where ID={ID}"));
//                sqlcmd.Commit();
//            }
//            catch (Exception ex)
//            {
//                sqlcmd.Rollback();
//                throw new RowException(ex, RowErrorCode.Unknown, null);
//            }
//            //UpdateLoginStatus(sqlcmd, context, user, ip, sqltime);
//            user.UserInit(sqlcmd);
//            WriteLoginLog(sqlcmd, row.ID, row.CorpID, row.ACNT, sqltime, ip, RowErrorCode.Successed, null, json);
//            UserList.setCurrentUser(context, user);
//            return user;
//        }

//        public override void Logout(SqlCmd sqlcmd)
//        {
//            using (this.CommandQueue)
//            {
//                UserList.setCurrentUser(HttpContext.Current, null);
//                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
//                {
//                    try
//                    {
//                        sqlcmd.BeginTransaction();
//                        sqlcmd.ExecuteNonQuery("update LoginState set ExpireTime=LoginTime where ID={0}", this.ID);
//                        sqlcmd.Commit();
//                    }
//                    catch
//                    {
//                        sqlcmd.Rollback();
//                        throw;
//                    }
//                }
//            }
//        }

//        static void UpdateLoginStatus(SqlCmd sqlcmd, HttpContext context, TUser user, string ip, DateTime? sqltime)
//        {
//            try
//            {
//                sqltool s = new sqltool();
//                s["", "ID", "        "] = user.ID;
//                s["", "UserType", " "] = _instance.UserType;
//                s["", "LoginID", "   "] = (StringEx.sql_str)"newid()";
//                s["", "IP", "        "] = ip;
//                s["", "SessionID", " "] = context.Session.SessionID;
//                s["", "CorpID", "    "] = user.CorpID;
//                s["", "GroupID", "   "] = user.GroupID;
//                s["", "ACNT", "      "] = user.ACNT;
//                s["", "ClassName", " "] = typeof(TUser).AssemblyQualifiedName;
//                s["", "ExpireTime", ""] = (StringEx.sql_str)string.Format("dateadd(ms, {0}, @ct)", _Global.sessionStateSection.Timeout.TotalMilliseconds);
//                s["", "LoginTime", " "] = (StringEx.sql_str)"@ct";
//                s["", "KickTime", "  "] = (StringEx.sql_str)"@ct";
//                if (sqltime.HasValue)
//                    s.Values["ct"] = sqltime.Value.ToString(sqltool.DateTimeFormat);
//                else
//                    s.Values["ct"] = (StringEx.sql_str)"getdate()";
//                sqlcmd.BeginTransaction();
//                sqlcmd.FillObject(user, s.BuildEx(@"declare @ct datetime set @ct={ct} delete LoginState where ID={ID}
//update LoginState set SessionID='' where SessionID={SessionID}
//insert into LoginState (", sqltool._Fields, ") values (", sqltool._Values, @")
//select * from LoginState nolock where ID={ID}"));
//                sqlcmd.Commit();
//            }
//            catch (Exception ex)
//            {
//                sqlcmd.Rollback();
//                throw new RowException(ex, RowErrorCode.Unknown, null);
//            }
//        }

        #endregion
    }

    partial class _Global
    {
        internal static List<User> GetUserList(HttpContext context)
        {
            try
            {
                context.Application.Lock();
                lock (context.Application)
                {
                    string name = typeof(List<User>).FullName;
                    List<User> list = context.Application.Get(name) as List<User>;
                    if (list == null) context.Application.Set(name, list = new List<User>());
                    return list;
                }
            }
            finally
            {
                context.Application.UnLock();
            }
        }
    }

    #region AbstractAdmin
    public abstract class AbstractAdmin<T> : User<T, AdminRow> where T : AbstractAdmin<T>, new()
    {
        public override UserType UserType
        {
            get { return BU.UserType.Admin; }
        }
        protected override string _sql_table
        {
            get { return "Admin"; }
        }

        static object sync_LoadPermission = new object();
        public void LoadPermission()
        {
            lock (sync_LoadPermission)
            {
                if (this.Permissions.IsNull)
                    (this.Permissions = new Permissions()).Load(null, this.UserType, this.CorpID, this.GroupID, this.ID);
            }
        }
    }
    #endregion

    #region AbstractAgent
    public abstract class AbstractAgent<T> : User<T, AgentRow> where T : AbstractAgent<T>, new()
    {
        public override UserType UserType
        {
            get { return BU.UserType.Agent; }
        }
        protected override string _sql_table
        {
            get { return "Agent"; }
        }
    }
    #endregion

    #region AbstractMember
    public abstract class AbstractMember<T> : User<T, MemberRow> where T : AbstractMember<T>, new()
    {
        public override UserType UserType
        {
            get { return BU.UserType.Member; }
        }
        protected override string _sql_table
        {
            get { return "Member"; }
        }
    }
    #endregion

    #region Guest
    public class Guest : User
    {
        public override UserType UserType
        {
            get { return BU.UserType.Guest; }
        }
    }
    #endregion




//    public abstract class User : IPrincipal, IIdentity//, IDisposable
//    {
//        public static class Manager
//        {
//            #region config

//            [AppSetting("CorpID"), DefaultValue(0)]
//            public static int DefaultCorpID
//            {
//                get { return app.config.GetValue<int>(MethodBase.GetCurrentMethod()); }
//            }

//            [SqlSetting("Password"), DefaultValue("0000")]
//            public static string DefaultPassword
//            {
//                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
//            }

//            //[SqlSetting("MemberType")]
//            //static string _DefaultMemberType
//            //{
//            //    get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
//            //}

//            //public static MemberType DefaultMemberType
//            //{
//            //    get { return _DefaultMemberType.ToEnum<MemberType>() ?? BU.MemberType.Normal; }
//            //}

//            [SqlSetting(Key = "DebugMode"), DefaultValue(false)]
//            public static bool DebugMode
//            {
//                get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
//            }

//            [AppSetting(Key = "DebugMode"), DefaultValue(false)]
//            public static bool DebugMode2
//            {
//                get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
//            }

//            public static T WebConfigSection<T>(string sectionName) where T : ConfigurationSection
//            {
//                return WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath).GetSection(sectionName) as T;
//            }

//            public static SessionStateSection sessionStateSection
//            {
//                get { return _Global.WebConfigSection<SessionStateSection>("system.web/sessionState"); }
//            }

//            #endregion

//            public const int RootAdminID = 1;
//            public const int RootAgentID = 2;

//            internal static readonly List<User> UserList = new List<User>();
//            static readonly Dictionary<Guid, HttpContext> UserResume = new Dictionary<Guid, HttpContext>();

//            static Manager()
//            {
//                TraceLogWriter.Enabled = TextLogWriter.Enabled = JsonTextLogWriter.Enabled = true;
//                try
//                {
//                    using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
//                        ConfigList.GetInstance(null, sqlcmd);
//                    log.message(null, "init completed.");
//                }
//                catch (Exception ex)
//                {
//                    log.error_msg(ex);
//                }
//            }

//            #region LoginID

//            static byte[] null_guid = Guid.Empty.ToByteArray();

//            static bool GetLoginID(HttpContext context, out Guid loginID)
//            {
//                loginID = Guid.Empty;
//                if (context != null)
//                {
//                    HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
//                    if (cookie != null)
//                    {
//                        try
//                        {
//                            string value = string.Format("{0}==", cookie.Value);
//                            byte[] data = Convert.FromBase64String(value);
//                            if (data.Length > null_guid.Length)
//                                Array.Resize(ref data, null_guid.Length);
//                            return !Guid.Empty.Equals(loginID = new Guid(data));
//                        }
//                        catch { }
//                    }
//                }
//                loginID = Guid.Empty;
//                return false;
//            }

//            internal static Guid SetLoginID(HttpContext context, Guid loginID)
//            {
//                byte[] data = loginID.ToByteArray();
//                string value = Convert.ToBase64String(data);
//                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value.Substring(0, value.Length - 2));
//                context.Response.Cookies.Add(cookie);
//                return loginID;
//            }

//            #endregion

//            static User GetUser(HttpContext context)
//            {
//                Guid loginID;
//                if (GetLoginID(context, out loginID))
//                {
//                    for (int i = 0, sleep = 0; ; i++, sleep = 10)
//                    {
//                        Thread.Sleep(sleep);
//                        lock (UserList)
//                            foreach (User user in UserList)
//                                if (user.LoginID.Equals(loginID))
//                                    return user.AuthenticateUser();
//                        lock (UserResume)
//                        {
//                            if (UserResume.ContainsKey(loginID)) continue;
//                            UserResume[loginID] = context;
//                        }
//                        try
//                        {
//                            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
//                            {
//                                AuthedUser user = null;
//                                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select getdate() as CurrentTime, * from LoginState nolock where LoginID='{0}'", loginID))
//                                {
//                                    Type userType = Type.GetType(r.GetString("ClassName"));
//                                    if (userType != null)
//                                        user = r.ToObject(userType) as AuthedUser;
//                                    break;
//                                }
//                                if (user == null)
//                                    return new Guest(loginID, "resume", "not found");
//                                else
//                                    return user.ResumeUser(sqlcmd);
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            return new Guest(loginID, "resume", ex.Message);
//                        }
//                        finally
//                        {
//                            lock (UserResume) UserResume.Remove(loginID);
//                        }
//                    }
//                }
//                Guest guest = new Guest(Guid.NewGuid(), "new guest", "");
//                SetLoginID(context, guest.LoginID);
//                return guest;
//            }

//            public static void Authenticate(FormsAuthenticationEventArgs e)
//            {
//                e.User = e.Context.User = GetUser(e.Context);
//            }
//        }

//        public void UpdateActivity(HttpContext context)
//        {
//        }

//        [DbImport]
//        public int ID { get; set; }
//        [DbImport]
//        internal Guid LoginID;
//        [DbImport]
//        public string IP { get; set; }

//        public virtual string Name { get; set; }
//        [DbImport]
//        string SessionID;
//        [DbImport]
//        public virtual int CorpID { get; set; }
//        [DbImport]
//        public virtual string ACNT { get; set; }
//        [DbImport]
//        public virtual byte GroupID { get; set; }
//        public abstract UserType UserType { get; }

//        Dictionary<string, string> m_data = new Dictionary<string, string>();
//        public string this[string key]
//        {
//            get
//            {
//                lock (this.m_data)
//                {
//                    string value;
//                    if (this.m_data.TryGetValue(key, out value))
//                        return value;
//                    return null;
//                }
//            }
//            set { lock (this.m_data) this.m_data[key] = value; }
//        }

//        public Permissions Permissions { get; internal set; }
//        public api.CommandQueue CommandQueue { get; internal set; }

//        public User()
//        {
//            this.Permissions = _null<Permissions>.value;
//            //this.Permissions2 = Permissions2.Null;
//            this.CommandQueue = api.CommandQueue.Global;
//        }

//        internal abstract User AuthenticateUser();

//        #region IPrincipal 成員

//        IIdentity IPrincipal.Identity
//        {
//            get { return this; }
//        }

//        bool IPrincipal.IsInRole(string role)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        #region IIdentity 成員

//        string IIdentity.AuthenticationType
//        {
//            get { return "Forms"; }
//        }

//        bool IIdentity.IsAuthenticated
//        {
//            get { return !(this is Guest); }
//        }

//        #endregion

//        //public interface Row
//        //{
//        //    int? ID { get; set; }
//        //    int? CorpID { get; set; }
//        //    byte? GroupID { get; set; }
//        //    string ACNT { get; set; }
//        //    string Name { get; set; }
//        //    Locked? Locked { get; set; }
//        //    string Password { get; set; }
//        //}
//    }

//    public abstract class AuthedUser : User
//    {
//        [DbImport]
//        internal DateTime UpdateTime;
//        [DbImport]
//        internal DateTime ExpireTime;
//        [DbImport]
//        internal DateTime LoginTime;
//        [DbImport]
//        internal DateTime KickTime;

//        public User SignIn(SqlCmd sqlcmd, HttpContext context, User existUser)
//        {
//            lock (_Global.UserList)
//            {
//                while (_Global.UserList.Remove(existUser)) { }
//                if (_Global.UserList.Contains(this))
//                    return this;
//                for (int i = _Global.UserList.Count - 1; i >= 0; i--)
//                    if (_Global.UserList[i].ID == this.ID)
//                        _Global.UserList.RemoveAt(i);
//            }
//            try
//            {
//                sqlcmd.BeginTransaction();
//                sqltool s = new sqltool();
//                s["", "ID", "        "] = this.ID;
//                s["", "LoginID", "   "] = (StringEx.sql_str)"newid()";
//                s["", "IP", "        "] = context.RequestIP();
//                s["", "SessionID", " "] = "";
//                s["", "CorpID", "    "] = this.CorpID;
//                s["", "GroupID", "   "] = this.GroupID;
//                s["", "ACNT", "      "] = this.ACNT;
//                s["", "ClassName", " "] = this.GetType().AssemblyQualifiedName;
//                s["", "ExpireTime", ""] = (StringEx.sql_str)string.Format("dateadd(ms, {0}, @ct)", _Global.sessionStateSection.Timeout.TotalMilliseconds);
//                s["", "LoginTime", " "] = (StringEx.sql_str)"@ct";
//                s["", "KickTime", "  "] = (StringEx.sql_str)"@ct";
//                string sqlstr = s.BuildEx(@"declare @ct datetime set @ct=getdate()
//if exists (select ID from LoginState nolock where ID={ID})
//update LoginState set ", sqltool._FieldValue, @" where ID={ID}
//else
//insert LoginState (", sqltool._Fields, ") values (", sqltool._Values, @")
//select * from LoginState nolock where ID={ID}");
//                sqlcmd.FillObject(this, sqlstr);
//                sqlcmd.Commit();
//            }
//            catch
//            {
//                sqlcmd.Rollback();
//                throw;
//            }
//            _Global.SetLoginID(context, this.LoginID);
//            this.UserInit(sqlcmd);
//            return this;
//        }

//        public void SignOut(SqlCmd sqlcmd)
//        {
//            using (this.CommandQueue)
//            {
//                lock (_Global.UserList)
//                    if (_Global.UserList.Contains(this))
//                        new Guest(this, "SignOut", "");
//                    else
//                        return;

//                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
//                {
//                    try
//                    {
//                        sqlcmd.BeginTransaction();
//                        sqlcmd.ExecuteNonQuery("update LoginState set ExpireTime=LoginTime where ID={0}", this.ID);
//                        sqlcmd.Commit();
//                    }
//                    catch
//                    {
//                        sqlcmd.Rollback();
//                        throw;
//                    }
//                }
//            }
//        }

//        internal override User AuthenticateUser()
//        {
//            if (this.KickTime > this.LoginTime)
//                return new Guest(this, "auth", "kicked");
//            else if (this.ExpireTime < DateTime.Now)
//                return new Guest(this, "auth", "expired");
//            else
//                return this;
//        }

//        internal User ResumeUser(SqlCmd sqlcmd)
//        {
//            if (this.KickTime > this.LoginTime)
//                return new Guest(this, "resume", "kicked");
//            else if (this.ExpireTime < DateTime.Now)
//                return new Guest(this, "resume", "expired");
//            this.UserInit(sqlcmd);
//            return this;
//        }

//        void UserInit(SqlCmd sqlcmd)
//        {
//            lock (_Global.UserList)
//                if (_Global.UserList.Contains(this))
//                    return;
//                else
//                    _Global.UserList.Add(this);
//            this.CommandQueue = new api.CommandQueue();
//            this.Permissions = new Permissions();
//            this.Permissions.Load(sqlcmd, this.UserType, this.CorpID, this.GroupID, this.ID);
//            //this.Permissions2 = new Permissions2();
//            //if (this.ID == _Global.RootAdminID)
//            //    this.Permissions2.Import(sqlcmd, "select Code,Flag from Code1 nolock");
//            //else
//            //    this.Permissions2.Import(sqlcmd, "select Code,Flag from Code2 nolock where ID={0}", this.ID);
//        }
//    }

//    public class Guest : User
//    {
//        public override UserType UserType
//        {
//            get { return BU.UserType.Guest; }
//        }

//        internal Guest(Guid loginID, string op, string msg)
//        {
//            base.LoginID = loginID;
//            lock (_Global.UserList)
//                _Global.UserList.Add(this);
//        }

//        internal Guest(User user, string op, string msg)
//        {
//            base.LoginID = user.LoginID;
//            lock (_Global.UserList)
//            {
//                while (_Global.UserList.Remove(user)) { }
//                _Global.UserList.Add(this);
//            }
//        }

//        internal override User AuthenticateUser()
//        {
//            return this;
//        }
//    }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //public class LoginResult
    //{
    //    public LoginResult(LoginStatus status, string message)
    //    {
    //        this.Status = status;
    //        this.Message = message ?? status.ToString();
    //    }

    //    [JsonProperty("t1")]
    //    public LoginStatus Status;

    //    [JsonProperty("t2")]
    //    public string Message;

    //    [JsonProperty("t3")]
    //    public Dictionary<string, object> Extra;

    //    static Dictionary<LoginStatus, LoginResult> instances = new Dictionary<LoginStatus, LoginResult>();
    //    static LoginResult GetInstance(LoginStatus status, string msg)
    //    {
    //        if (msg == null)
    //            if (instances.ContainsKey(status))
    //                return instances[status];
    //        return new LoginResult(status, msg);
    //    }
    //    static LoginResult()
    //    {
    //        foreach (LoginStatus s in Enum.GetValues(typeof(LoginStatus)))
    //            instances[s] = new LoginResult(s, null);
    //    }

    //    public static LoginResult Failed(SqlCmd sqlcmd, string json, Exception ex, string msg)
    //    {
    //        return LoginResult.Failed(sqlcmd, json, LoginStatus.SystemError, msg ?? ex.Message);
    //    }
    //    public static LoginResult Failed(SqlCmd sqlcmd, string json, LoginStatus status, string msg)
    //    {
    //        return LoginResult.GetInstance(status, msg);
    //    }
    //    public static LoginResult Success(SqlCmd sqlcmd, string json, User user, HttpContext context)
    //    {
    //        if (user != null)
    //        {
    //            //user.Permissions.Import(sqlcmd, user.ID);
    //            //user.SignIn(context, null);
    //        }
    //        return LoginResult.GetInstance(LoginStatus.Success, null);
    //    }
    //}
}