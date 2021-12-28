using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using web;

namespace BU
{
    public static partial class text
    {
        public static bool Contains(this string s, char c)
        {
            return s.Contains(c, true);
        }
        public static bool Contains(this string s, char c, bool ignoreCase)
        {
            if (s == null)
                return false;
            if (ignoreCase)
                return (s.IndexOf(char.ToLower(c)) >= 0) || (s.IndexOf(char.ToUpper(c)) >= 0);
            else
                return s.IndexOf(c) >= 0;
        }

        static byte[] salt = Encoding.UTF8.GetBytes("saltValue");
        public static string EncodePassword(string acnt, string password)
        {
            if (acnt == null)
                return null;
            if (password == null)
                return null;
            password = password.Trim();
            if (password == string.Empty)
                return null;
            return Convert.ToBase64String(Crypto.TripleDES.Encrypt(password, acnt, "saltValue", Encoding.UTF8).MD5());
            //return Convert.ToBase64String(password.TripleDESEncrypt(acnt, null).MD5());
        }

        public static int? Sql_Bool(bool? value)
        {
            if (value.HasValue)
                return Sql_Bool(value.Value);
            return null;
        }
        public static int Sql_Bool(bool value)
        {
            return value ? 1 : 0;
        }

        [DebuggerStepThrough]
        public static string RequestIP(this HttpContext context)
        {
            try
            {
                if (context.Request.IsLocal)
                    return "127.0.0.1";
                else
                    return context.Request.UserHostAddress;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// use for login
        /// </summary>
        public static bool SplitMailAddress(SqlCmd sqlcmd, string s, out int? corpID, out string acnt)
        {
            s = (s ?? "").Trim();
            if (string.IsNullOrEmpty(s))
            {
                corpID = null;
                acnt = null;
            }
            else
            {
                int n = s.IndexOf('@');
                string corp;
                if (n >= 0)
                {
                    corp = s.Substring(n + 1);
                    corpID = web.CorpRow.Cache.GetInstance(sqlcmd, null).GetCorp(corp).ID;
                    acnt = s.Substring(0, n);
                }
                else
                {
                    corpID = 0;
                    acnt = s;
                }
                acnt *= text.ValidAsACNT;
            }
            return acnt != null;
        }

        static StringEx.sql_str sql_null = "null";

        public static object Valid_PCT(string value)
        {
            if (value == "*")
                return sql_null;
            decimal? d = value.ToDecimal();
            return d;
        }


        public static readonly ValidAs<string, string> ValidAsACNT = new ValidAs<string, string>(_ValidACNT);
        public static readonly ValidAs<string, string> ValidAsGameACNT = new ValidAs<string, string>(_ValidGameACNT);
        public static readonly ValidAs<string, string> ValidAsName = new ValidAs<string, string>(_ValidString);
        public static readonly ValidAs<string, string> ValidAsString = new ValidAs<string, string>(_ValidString);
        //public static readonly ValidAs<string, string> ValidAsTrimString = new ValidAs<string, string>(_ValidTrimString);
        public static readonly ValidAs<string, Locked?> ValidAsLocked2 = new ValidAs<string, Locked?>(_ValidLocked);
        //public static readonly ValidAs<decimal?, decimal?> ValidAsMoney = new ValidAs<decimal?, decimal?>(_ValidAsMoney);

        //public static decimal? ValidAsMoney(decimal? value)
        //{
        //    if (value.HasValue && (value.Value >= 0))
        //        return value;
        //    return null;
        

        public static readonly ValidAs<string, string> ValidAsACNT2 = new ValidAs<string, string>(_ValidACNT2);
        public static readonly ValidAs<string, string> ValidAsString2 = new ValidAs<string, string>(_ValidString2);
        static string _ValidACNT2(string acnt) { return (_ValidACNT(acnt) ?? "").Replace("\t", ""); }
        static string _ValidString2(string acnt) { return (_ValidString(acnt) ?? "").Replace("\t", ""); }

        public static string Remove(this string str, params string[] part)
        {
            if (str != null)
            {
                for (int i = 0; i < part.Length; i++)
                    str = str.Replace(part[i], "");
            }
            return str;
        }

        static string _ValidACNT(string acnt)
        {
            if (acnt == null) return null;
            acnt += text.ValidAsString;
            if (acnt == string.Empty)
                return null;
            return acnt.ToLower();
        }
        static string _ValidGameACNT(string acnt)
        {
            if (acnt == null) return null;
            acnt += text.ValidAsString;
            if (acnt == string.Empty)
                return null;
            return acnt;
        }

        //static string _ValidTrimString(string str)
        //{
        //    if (str != null)
        //        return str.Replace("'", "''").Trim();
        //    return str;
        //}

        static string _ValidString(string str)
        {
            if (str != null)
                return str.Replace("'", "''").Trim();
            return str;
        }

        static Locked? _ValidLocked(string input)
        {
            if (input != null)
            {
                if (Regex.IsMatch(input, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
                    return Locked.Active;
                if (Regex.IsMatch(input, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
                    return Locked.Locked;
            }
            return null;
        }

        public static T? ValidEnum<T>(T? value) where T : struct
        {
            if (value.HasValue && typeof(T).IsEnum)
            {
                if (Enum.IsDefined(typeof(T), value.Value))
                    return value;
            }
            return null;
        }

        public static object GetValue(int? n)
        {
            if (n.HasValue)
            {
                if (n.Value > 0)
                    return n.Value;
                else
                    return StringEx.sql_str.Null;
            }
            return null;
        }

        //public static void AppendWhereString(this StringBuilder s, string format, params object[] args)
        //{
        //    if (s == null) return;
        //    if (s.Length == 0)
        //        s.Append(" where ");
        //    else
        //        s.Append(" and ");
        //    s.AppendFormat(format, args);
        //}
    }

    //public abstract class ValidAs
    //{
    //    public static readonly ValidAs<string, string> ACNT = new ValidAs<string, string>(ValidAsACNT);
    //    public static readonly ValidAs<string, string> Name = new ValidAs<string, string>(ValidAsString);
    //    public static readonly ValidAs<string, string> String = new ValidAs<string, string>(ValidAsString);
    //    public static readonly ValidAs<string, byte?> Locked = new ValidAs<string, byte?>(ValidAsLocked);

    //    static string ValidAsACNT(string acnt)
    //    {
    //        if (acnt == null) return null;
    //        acnt += ValidAs.String;
    //        if (acnt == string.Empty)
    //            return null;
    //        return acnt.ToLower();
    //    }

    //    static string ValidAsString(string str)
    //    {
    //        if (str != null)
    //            return str.Replace("'", "''");
    //        return str;
    //    }

    //    static byte? ValidAsLocked(string input)
    //    {
    //        if (input != null)
    //        {
    //            if (Regex.IsMatch(input, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
    //                return 0;
    //            if (Regex.IsMatch(input, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
    //                return 1;
    //        }
    //        return null;
    //    }
    //}

    [DebuggerStepThrough]
    public class ValidAs<_in, _out>
    {
        public static _out operator +(ValidAs<_in, _out> n, _in input) { return n.Valid(input); }
        public static _out operator +(_in input, ValidAs<_in, _out> n) { return n.Valid(input); }
        public static _out operator -(ValidAs<_in, _out> n, _in input) { return n.Valid(input); }
        public static _out operator -(_in input, ValidAs<_in, _out> n) { return n.Valid(input); }
        public static _out operator *(ValidAs<_in, _out> n, _in input) { return n.Valid(input); }
        public static _out operator *(_in input, ValidAs<_in, _out> n) { return n.Valid(input); }
        public static _out operator /(ValidAs<_in, _out> n, _in input) { return n.Valid(input); }
        public static _out operator /(_in input, ValidAs<_in, _out> n) { return n.Valid(input); }

        public delegate _out ValidHandler(_in input);

        readonly ValidHandler Valid;
        public ValidAs(ValidHandler cb) { this.Valid = cb; }
    }
}
