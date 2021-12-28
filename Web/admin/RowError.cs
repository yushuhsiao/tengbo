using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using web;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace BU
{
    public enum RowErrorCode
    {
        Successed = 1,
        Unknown = -1,
        NoResult = -2,
        FieldNeeds = -3,
        InvaildUserType = -4,
        InvaildLogType = -5,
        SystemError = -9,

        //LoginFailed = -1,
        //LoginUnknown = 0,
        //LoginSuccess = 1,

        // Login Status
        AccessDenied = 2,
        //Login_SystemError = 3,
        InvaildChar = 4,
        UserAlreadyLogin = 6,
        VerifyCodeError = 7,
        AccountNotExist = 8,            // 帳號不存在
        NotActive = 9,                  // 帳號尚未啟用
        AccountExpired = 10,            // 帳號已過期(停用)
        AccountLocked = 11,             // 帳號已停用
        PasswordError = 12,             // 密碼不正確
        PasswordExpired = 13,           // 密碼過期

        RecoveryAccept = 51,
        RecoverySuccessed = 52,
        RecoveryTokenError = 53,


        InvalidGameID = 100,
        GameDisabled = 101,
        InvalidCorpID = 104,
        AdminLocked = 105,
        AgentLocked = 106,
        MemberLocked = 107,
        GameAccountLocked = 108,
        GameAPIError = 109,
        RefreshRequired = 110,

        NotFound = 1000,
        CorpNotFound = NotFound + 1,
        GameNotFound = NotFound + 2,
        AdminNotFound = NotFound + 3,
        AgentNotFound = NotFound + 4,
        MemberNotFound = NotFound + 5,
        GameAccountNotFound = NotFound + 6,     // 子帳戶不存在

        ParentAdminNotFound = NotFound + 11,
        ParentAgentNotFound = NotFound + 12,
        ProviderNotFound = NotFound + 13,

        TranNotFound = NotFound + 20,
        MemberTranNotFound = NotFound + 21,
        GameTranNotFound = NotFound + 22,
        PromoTranNotFount = NotFound + 23,

        AlreadyExist = 2000,
        CorpAlreadyExist = AlreadyExist + 1,
        GameAlreadyExist = AlreadyExist + 2,
        AdminAlreadyExist = AlreadyExist + 3,
        AgentAlreadyExist = AlreadyExist + 4,
        MemberAlreadyExist = AlreadyExist + 5,
        BalanceNotEnough = 3000,
        TranAlreadyFinished = 3001,
        MemberBalanceNotEnough = BalanceNotEnough + 11,
        AgentBalanceNotEnough = BalanceNotEnough + 12,
        ProviderBalanceNotEnough = BalanceNotEnough + 13,
        MaxMember,
        MaxAgent,
        MaxDepth,

        RowIsReadOnly = 4001,
        UnableToDeleteRow = 4002,
        MemberGame_UnableAllocID = 4003,

        // for register user
        reg_ACNT_Null = 8001,
    }

    //[JsonConverter(typeof(RowException._JsonConverter))]
    //public class RowException : Exception
    //{
    //    class _JsonConverter : JsonConverter
    //    {
    //        public override bool CanConvert(Type objectType)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //        {
    //            RowException ex = value as RowException;
    //            if (ex != null)
    //                value = new object[] { false, ex.Message, ex.args };
    //            serializer.Serialize(writer, value);
    //        }
    //    }

    //    public readonly RowErrorCode ErrorCode;
    //    public object[] args;

    //    public RowException(RowErrorCode errorCode) : this(errorCode, null) { }
    //    public RowException(RowErrorCode errorCode, string message, params object[] args)
    //        : base(message ?? errorCode.ToString())
    //    {
    //        this.ErrorCode = errorCode;
    //        this.args = args;
    //    }

    //    //public static void Test(SqlDataReader r, string name, string msg)
    //    //{
    //    //    RowErrorCode? err = (RowErrorCode?)r.GetInt32N(name);
    //    //    if (err.HasValue)
    //    //        if (err.Value != RowErrorCode.Successed)
    //    //            throw new RowException(err.Value, r.GetStringN(msg));
    //    //}

    //    //public static void TestNull(object value)
    //    //{
    //    //    RowException.TestNull(value, RowErrorCode.NoResult);
    //    //}
    //    //public static void TestNull(object value, RowErrorCode err)
    //    //{
    //    //    if (value == null)
    //    //        throw new RowException(err);
    //    //}
    //}

    //public interface IRowCommand { }
    public interface IRowCommand { }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn, Id = "RowException")]
    [_DebuggerStepThrough]
    [JsonConverter(typeof(RowException._JsonConverter))]
    public class RowException : Exception
    {
        class _JsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) { throw new NotImplementedException(); }
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { throw new NotImplementedException(); }
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                RowException ex = value as RowException;
                if (ex != null)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Status");
                    serializer.Serialize(writer, ex.Status);
                    if (!string.IsNullOrEmpty(ex.Message))
                    {
                        writer.WritePropertyName("Message");
                        serializer.Serialize(writer, ex.Message);
                    }
                    if ((ex.args ?? Tools._null<object>.array).Length > 0)
                    {
                        writer.WritePropertyName("args");
                        serializer.Serialize(writer, ex.args);
                    }
                    if (ex.InnerException != null)
                    {
                        writer.WritePropertyName("InnerException");
                        serializer.Serialize(writer, ex.InnerException);
                    }
                    writer.WriteEndObject();
                    return;
                    //value = new object[] { false, ex.Message, ex.args };
                }
                serializer.Serialize(writer, value);
            }
        }

        public readonly RowErrorCode Status;
        public readonly object[] args;

        public static object Success(object row)
        {
            return new { Status = RowErrorCode.Successed, row = row };
        }

        public RowException(RowErrorCode errorCode) : this(null, errorCode, null) { }
        public RowException(RowErrorCode errorCode, string message) : this(null, errorCode, message) { }
        public RowException(RowErrorCode errorCode, string message, params object[] args) : this(null, errorCode, message, args) { }
        public RowException(Exception innerException, RowErrorCode errorCode, string message, params object[] args)
            : base(message ?? errorCode.ToString())
        {
            this.Status = errorCode;
            this.args = args;
        }
    }

    //public class RawRow : Dictionary<string, object>
    //{
    //}

    //public class RawRowCommand : Dictionary<string, object>
    //{
    //}

    //public interface IRowCommand<T>
    //{
    //    T update(string json_s, params object[] args);
    //    T insert(string json_s, params object[] args);
    //}

    partial class text
    {
        //public static object RowUpdate<T>(this IRowCommand<T> command, string json_s, params object[] args)
        //{
        //    try { return new object[] { true, command.update(json_s, args) }; }
        //    catch (RowException ex) { return ex; }
        //}
        //public static object RowInsert<T>(this IRowCommand<T> command, string json_s, params object[] args)
        //{
        //    try { return new object[] { true, command.insert(json_s, args) }; }
        //    catch (RowException ex) { return ex; }
        //}

        //public static object[] RowUpdate(object row)
        //{
        //    return new object[] { true, row };
        //}
        [_DebuggerStepThrough]
        public static TRow GetRow<TRow>(this SqlCmd sqlcmd, string format, params object[] keys) where TRow : new()
        {
            for (int i = 0; i < keys.Length; i++)
                if (keys[i] == null)
                    return default(TRow);
            return sqlcmd.ToObject<TRow>(format, keys);
        }
        [_DebuggerStepThrough]
        public static TRow GetRowEx<TRow>(this SqlCmd sqlcmd, RowErrorCode err, string format, params object[] keys) where TRow : new()
        {
            TRow row = sqlcmd.GetRow<TRow>(format, keys);
            if (row == null) throw new RowException(err, null, keys);
            return row;
        }
        [_DebuggerStepThrough]
        public static TRow ExecuteEx<TRow>(this SqlCmd sqlcmd, string format, params object[] args) where TRow : class, new()
        {
            try
            {
                sqlcmd.BeginTransaction();
                TRow row = null;
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(format, args))
                {
                    RowErrorCode? err = (RowErrorCode?)r.GetInt32N("err");
                    if (err.HasValue)
                        if (err.Value != RowErrorCode.Successed)
                            throw new RowException(err.Value, r.GetStringN("msg"));
                    if (row == null)
                        row = r.ToObject<TRow>();
                    else
                        r.FillObject(row);
                }
                if (row == null)
                    throw new RowException(RowErrorCode.NoResult);
                sqlcmd.Commit();
                return row;
            }
            catch
            {
                sqlcmd.Rollback();
                throw;
            }
        }
    }
}