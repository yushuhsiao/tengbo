//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Reflection;
//using System.Security.Cryptography;
//using System.Web;
//using Cashino;
//using Newtonsoft.Json;

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class AdminLogin
//{
//    [JsonProperty("t1")]
//    public string UserName;

//    [JsonProperty("t2")]
//    public string Password;

//    [ObjectInvoke]
//    static LoginResult login(object sender, AdminLogin login_s, object result, string json)
//    {
//        UserInfo user = UserInfo.Current;
//        if (user.IsLogin)
//            return LoginResult.Success;
//        string corpName = "";
//        string userName = (login_s.UserName ?? "").Replace("'", "''").Trim();
//        int n = userName.IndexOf("@");
//        if (n >= 0)
//        {
//            corpName = userName.Substring(n + 1);
//            userName = userName.Substring(0, n);
//        }

//        SqlCmd sql;
//        try { sql = new SqlCmd("GameDB"); }
//        catch (Exception ex) { return new LoginResult(LoginStatus.SystemError, ex.Message); }

//        LoginResult login_r = null;
//        using (sql)
//        {
//            try
//            {
//                UserData data = sql.ToObject<UserData>(string.Format("exec sp_get_admin '{0}','{1}'", corpName, userName));

//                if (data == null)
//                    return login_r = new LoginResult(LoginStatus.UserNotExist, null);

//                if (!data.ActiveTime.HasValue)
//                    return login_r = new LoginResult(LoginStatus.NotActive, null);

//                if (data.ExpireTime.HasValue)
//                    if (data.ExpireTime >= data.CurrentTime)
//                        return login_r = new LoginResult(LoginStatus.Expired, null);

//                string password = Convert.ToBase64String(login_s.Password.TripleDESEncrypt(data.ACNT, null).MD5());
//                if (password != data.Password)
//                    return login_r = new LoginResult(LoginStatus.PasswordError, null);
//                // write login success log
//                lock (user)
//                {
//                    user.CorpID = data.CorpID.Value;
//                    user.ACNT = data.ACNT;
//                    user.Name = data.Name;
//                }
//                return login_r = LoginResult.Success;
//            }
//            catch (Exception ex)
//            {
//                // write login error log
//                return login_r = new LoginResult(LoginStatus.SystemError, ex.Message);
//            }
//            finally
//            {
//                if (login_r != null)
//                {
//                    switch (login_r.Status)
//                    {
//                        case LoginStatus.Success:
//                        case LoginStatus.SystemError:
//                            break;
//                        default:
//                            // write login failed log
//                            //Response.Write(s);
//                            //Response.Write("\r\n");
//                            //Response.Write(JsonConvert.SerializeObject(login));
//                            //result = LoginResult.Failed;
//                            break;
//                    }
//                }
//            }
//        }
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class AdminLogout
//{
//    [ObjectInvoke]
//    static LoginResult logout(object sender, AdminLogout login_s, object result, string json)
//    {
//        UserInfo user = UserInfo.Current;
//        lock (user)
//        {
//            user.CorpID = 0;
//            user.ACNT = user.Name = null;
//        }
//        return LoginResult.Success;
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class LoginResult
//{
//    [JsonProperty("t1")]
//    public LoginStatus Status;

//    [JsonProperty("t2")]
//    public string Message;

//    public LoginResult(LoginStatus status, string message)
//    {
//        this.Status = status;
//        this.Message = message ?? status.ToString();
//    }

//    public static LoginResult Success = new LoginResult(LoginStatus.Success, "");
//}

//class UserData
//{
//    [DbImport]
//    public int? CorpID;

//    [DbImport]
//    public string ACNT;

//    [DbImport]
//    public string Parent;

//    [DbImport]
//    public string Name;

//    [DbImport]
//    public string Password;

//    [DbImport]
//    public DateTime? ActiveTime;

//    [DbImport]
//    public DateTime? ExpireTime;

//    [DbImport]
//    public DateTime? CurrentTime;
//}