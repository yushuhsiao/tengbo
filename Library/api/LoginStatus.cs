using System;
using System.Collections.Generic;
using System.Text;

namespace BU
{
    public enum LoginStatus
    {
        Failed = -1,
        Unknown = 0,
        Success = 1,
        AccessDenied = 2,
        SystemError = 3,
        InvaildChar = 4,
        CorpNotExist = 5,
        AlreadyLogin = 6,
        VerifyCodeError = 7,
        UserNotExist = 8,                   // 帳號不存在
        NotActive = 9,                      // 帳號尚未啟用
        Expired = 10,                       // 帳號已過期(停用)
        Locked = 11,                        // 帳號已停用
        PasswordError = 12,                 // 密碼不正確
        PasswordExpired = 13,               // 密碼過期
        FieldNeeds = 14,                    // for register
        UserAlreadyExist = 15,              // 帳號已存在 (for register)

        RecoveryAccept = 101,
        RecoverySuccessed = 102,
        RecoveryTokenError = 103,
    }
}