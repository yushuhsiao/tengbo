using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.Security;
using BU;
using Newtonsoft.Json;
using web.data;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class notify : api.Comet
    {
    }

    public class Admin : AuthedUser
    {
        public override UserType UserType
        {
            get { return BU.UserType.Admin; }
        }
        //public override string TableName
        //{
        //    get { return "Admin"; }
        //}
    }

    public class Agent : AuthedUser
    {
        public override UserType UserType
        {
            get { return BU.UserType.Agent; }
        }
        //public override string TableName
        //{
        //    get { return "Agent"; }
        //}
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminLogin
    {
        [JsonProperty("t1")]
        public string loginName;

        [JsonProperty("t2")]
        public string loginPassword;

        //#region db

        //[DbImport("ct")]
        //public DateTime CurrentTime;

        //[DbImport]
        //public int? ID;

        //[DbImport]
        //public int CorpID;

        //[DbImport]
        //public string ACNT;

        //[DbImport]
        //public int ParentID;

        //[DbImport]
        //public string Name;

        //[DbImport("pwd")]
        //public string Password;

        //[DbImport]
        //public byte? Locked;

        //#endregion

        [ObjectInvoke, api.Async]
        static object login(AdminLogin command, string json_s, params object[] args)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                Guest guest = context.User as Guest;
                if (guest == null)
                    return LoginResult.Failed(null, json_s, BU.LoginStatus.AlreadyLogin, null);

                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                {
                    int? corpID;
                    string acnt;
                    if (!text.SplitMailAddress(sqlcmd, command.loginName, out corpID, out acnt))
                        return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.InvaildChar, null);
                    if (!corpID.HasValue)
                        return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.CorpNotExist, null);
                    AdminRow row = sqlcmd.ToObject<AdminRow>("select getdate() as ct,ID,CorpID,GroupID,ACNT,[Name],pwd,Locked from [Admin] nolock where CorpID={0} and ACNT='{1}'", corpID, acnt);
                    if ((row == null) || (!row.ID.HasValue))
                        return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.UserNotExist, null);
                    if (row.ID == 0)
                        row.Locked = (byte)Locked.Active;
                    else
                        row.Locked = row.Locked ?? (byte)Locked.Active;
                    if ((Locked)row.Locked.Value != Locked.Active)
                        return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.Locked, null);
                    if (text.EncodePassword(row.ACNT, command.loginPassword) != row.Password)
                        return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.PasswordError, null);

                    // update status
                    // write login success log

                    Admin admin = new Admin()
                    {
                        ID = row.ID.Value,
                        CorpID = row.CorpID.Value,
                        GroupID = row.GroupID.Value,
                        ACNT = row.ACNT,
                        Name = row.Name,
                    };
                    //admin.LoadPermissions(sqlcmd);
                    admin.SignIn(sqlcmd, context, guest);
                    //User.Manager.SignIn(sqlcmd, context, guest, admin);
                    return LoginResult.Success(sqlcmd, json_s, admin, context);
                }
            }
            catch (Exception ex)
            {
                // write login error log
                return LoginResult.Failed(null, json_s, ex, null);
            }
        }

    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminLogout
    {
        [ObjectInvoke]
        LoginResult logout(AdminLogout cmd, string json_s, params object[] args)
        {
            AuthedUser user = HttpContext.Current.User as AuthedUser;
            if (user != null)
                user.SignOut(null);
            return LoginResult.Success(null, json_s, null, null);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AdminSetInfo
    {
        //[ObjectInvoke, api.Async]
        //static object setinfo(AdminSetInfo command, string json_s, params object[] args)
        //{
        //    try
        //    {
        //        HttpContext context = HttpContext.Current;
        //        Admin admin = context.User as Admin;
        //        if (admin == null)
        //            return LoginResult.Failed(null, json_s, BU.LoginStatus.AccessDenied, null);

        //        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //        {
        //            //int? corpID;
        //            //string acnt;
        //            //if (!text.SplitMailAddress(sqlcmd, command.loginName, out corpID, out acnt))
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.InvaildChar, null);
        //            //if (!corpID.HasValue)
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.CorpNotExist, null);
        //            //AdminRow row = sqlcmd.ToObject<AdminRow>("select getdate() as ct,ID,CorpID,ACNT,[Name],pwd,Locked from [Admin] nolock where CorpID={0} and ACNT='{1}'", corpID, acnt);
        //            //if ((row == null) || (!row.ID.HasValue))
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.UserNotExist, null);
        //            //if (row.ID == 0)
        //            //    row.Locked = (byte)Locked.Active;
        //            //else
        //            //    row.Locked = row.Locked ?? (byte)Locked.Active;
        //            //if ((Locked)row.Locked.Value != Locked.Active)
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.Locked, null);
        //            //if (text.EncodePassword(row.ACNT, command.loginPassword) != row.Password)
        //            //    return LoginResult.Failed(sqlcmd, json_s, BU.LoginStatus.PasswordError, null);

        //            //// update status
        //            //// write login success log

        //            //Admin admin = new Admin()
        //            //{
        //            //    ID = row.ID.Value,
        //            //    CorpID = row.CorpID.Value,
        //            //    ACNT = row.ACNT,
        //            //    Name = row.Name,
        //            //};
        //            ////admin.LoadPermissions(sqlcmd);
        //            //admin.SignIn(sqlcmd, context, admin);
        //            ////User.Manager.SignIn(sqlcmd, context, guest, admin);
        //            //return LoginResult.Success(sqlcmd, json_s, admin, context);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // write login error log
        //        return LoginResult.Failed(null, json_s, ex, null);
        //    }
        //}
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentLogin
    {
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentLogout
    {
    }
}