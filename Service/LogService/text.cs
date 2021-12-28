using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace web
{
    public class sql_tool
    {
        public Dictionary<string, string> src;
        public Dictionary<string, object> dst;
        public sql_tool(string jsonString)
        {
            this.src = Tools.Protocol.JsonProtocol.DeserializeObject<Dictionary<string, string>>(jsonString);
            this.dst = new Dictionary<string, object>();
        }

        public string fields(char? l, char? r)
        {
            int i = 0;
            StringBuilder s = new StringBuilder();
            foreach (string f in this.dst.Keys)
            {
                if (i++ > 0) s.Append(',');
                s.Append(l);
                s.Append(f);
                s.Append(r);
            }
            return s.ToString();
        }

        public string sql_str(string s1, string s2)
        {
            return sql_str(s1, s2, "");
        }
        public string sql_str(string s1, string s2, string field_prefix)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(s1);
            int i = 0;
            foreach (string f in this.dst.Keys)
            {
                if (i++ > 0) sql.Append(",");
                sql.AppendFormat("{1}{0}={{{0}}}", f, field_prefix);
            }
            sql.Append(s2);
            return sql.ToString();
        }

        public T sql_GetRow<T>(SqlCmd sqlcmd, string key_id, string sqltext) where T : new()
        {
            int? id = this.Int32(key_id, null, null);
            if (id.HasValue)
                return sqlcmd.ToObject<T>(string.Format(sqltext, id.Value));
            return default(T);
        }

        public bool? isUpdate;

        public T? _Value<T>(string srckey, string dstkey, T? value2, StringEx.GetValueHandler<T> getValue) where T : struct
        {
            dstkey = dstkey ?? srckey;
            string s;
            src.TryGetValue(srckey, out s);
            T? value;
            bool setnull = s == "null" || s == "undefined";
            if (setnull)
                value = null;
            else
                value = getValue(s);
            if (isUpdate.HasValue)
            {
                if (isUpdate.Value)
                {
                    if ((value.HasValue && (!EqualityComparer<T?>.Default.Equals(value, value2))) || (value2.HasValue && setnull))
                        dst[dstkey] = value;
                }
                else
                {
                    value = value ?? value2;
                    if (value.HasValue)
                        dst[dstkey] = value;
                }
            }
            return value;
        }

        public void NullField(params string[] fields)
        {
            foreach (string f in fields)
                if (!dst.ContainsKey(f))
                    dst[f] = null;
        }

        public int? Int32(string srckey, string dstkey, int? value2)
        {
            return _Value<Int32>(srckey, dstkey, value2, StringEx.ToInt32);
        }

        public DateTime? DateTime(string srckey, string dstkey, DateTime? value2)
        {
            return _Value<DateTime>(srckey, dstkey, value2, StringEx.ToDateTime);
        }

        public byte? Locked(string srckey, string dstkey, byte? value2)
        {
            return _Value<byte>(srckey, dstkey, value2, text.ToLockedValue);
        }

        public decimal? Decimal(string srckey, string dstkey, decimal? value2)
        {
            return _Value<Decimal>(srckey, dstkey, value2, StringEx.ToDecimal);
        }

        public T? Enum<T>(string srckey, string dstkey, T? value2) where T : struct
        {
            return _Value<T>(srckey, dstkey, value2, StringEx.ToEnum<T>);
        }

        public string EnumString<T>(string srckey, string dstkey, T? value2) where T : struct
        {
            T? value = _Value<T>(srckey, dstkey, value2, StringEx.ToEnum<T>);
            dstkey = dstkey ?? srckey;
            string str;
            if (value.HasValue)
            {
                str = value.Value.ToString();
                if (dst.ContainsKey(dstkey))
                    dst[dstkey] = str;
            }
            else
            {
                str = null;
                if (dst.ContainsKey(dstkey))
                    dst.Remove(dstkey);
            }
            return str;
        }

        public string String(string srckey, string dstkey, string value2)
        {
            dstkey = dstkey ?? srckey;
            string s;
            string value;
            if (src.TryGetValue(srckey, out s))
            {
                value = s.Trim().ReplaceQuote();
                if (string.IsNullOrEmpty(value))
                    value = null;
            }
            else
                value = null;
            if (isUpdate.HasValue)
            {
                if (isUpdate.Value)
                {
                    if ((value != null) && (value != value2))
                        dst[dstkey] = value;
                }
                else
                {
                    value = value ?? value2;
                    if (value != null)
                        dst[dstkey] = value;
                }
            }
            return value;
        }

        public string Name(string srckey, string dstkey, string value2)
        {
            return String(srckey, dstkey, value2);
        }

        public string ACNT(string srckey, string dstkey, string value2)
        {
            return text.ValidACNT(String(srckey, dstkey, value2));
        }

        public string Password(string srckey, string dstkey, string acnt, string value2)
        {
            dstkey = dstkey ?? srckey;
            string s;
            if (src.TryGetValue(srckey, out s))
            {
                s = s.Trim();
                if (string.IsNullOrEmpty(s))
                    s = null;
            }
            else
                s = null;
            if (isUpdate.HasValue)
            {
                if (isUpdate.Value)
                {
                    if (s != null)
                    {
                        string p = BU.text.EncodePassword(acnt, s);
                        if (p != value2)
                            dst[dstkey] = p;
                    }
                }
                else
                {
                    s = s ?? value2;
                    if (s != null)
                        dst[dstkey] = BU.text.EncodePassword(acnt, s);
                }
            }
            return s;
        }


    }

    public static class fields
    {
        public const string ID = "ID";
        public const string ID_ = "ID_";
        public const string CorpID = "CorpID";
        public const string ACNT = "ACNT";
        public const string ParentACNT = "ParentACNT";
        public const string Name = "Name";
        public const string Password = "Password";
        public const string pwd = "pwd";
        public const string Locked = "Locked";
        public const string Currency = "Currency";
        public const string BonusW = "BonusW";
        public const string BonusL = "BonusL";
        public const string AdminACNT = "AdminACNT";
        public const string AgentACNT = "AgentACNT";
        public const string MaxUser = "MaxUser";
        public const string MaxAgent = "MaxAgent";
        public const string MaxDepth = "MaxDepth";
        public const string ModifyTime = "ModifyTime";
        public const string ModifyUser = "ModifyUser";
        public const string ModifyUserType = "ModifyUserType";
        public const string MemberType = "MemberType";
        public const string Birthday = "Birthday";
        public const string Tel = "Tel";
        public const string Mail = "Mail";
        public const string QQ = "QQ";
        public const string RegisterIP = "RegisterIP";
        public const string SecPassword = "SecPassword";
        public const string sec_pwd = "sec_pwd";
        public const string WebATM = "WebATM";
        public const string Location = "Location";
        public const string CardID = "CardID";
        public const string BankID = "BankID";
        public const string GroupID = "GroupID";
        public const string Sort = "Sort";
        public const string Text = "Text";
        public const string Place = "Place";
        public const string MemberID = "MemberID";
        public const string MemberACNT = "MemberACNT";
        public const string Amount = "Amount";
        public const string Disabled = "Disabled";
        public const string RecordType = "RecordType";
        public const string ID1 = "ID1";
        public const string ID2 = "ID2";
    }

    static class text
    {
        public static bool AddValue(this StringBuilder sql, string prefix, string field, string s1, string s2)
        {
            if (s1 == s2)
                return true;
            return sql.AddValue(prefix, field, s2);
        }

        public static bool AddValue<T>(this StringBuilder sql, string prefix, string field, T s1, T s2)
        {
            if (EqualityComparer<T>.Default.Equals(s1, s2))
                return true;
            return sql.AddValue(prefix, field, s2);
        }

        public static bool AddValue(this StringBuilder sql, string prefix, string field, object value)
        {
            if (value == null)
                return false;
            if (value is string)
            {
                if (value.Equals(string.Empty))
                    return false;
                sql.AppendFormat(",{0}{1}='{2}'", prefix, field, value);
            }
            else if ((value is DateTime) || (value is DateTime?))
                sql.AppendFormat(",{0}{1}='{2:yyyy-MM-dd HH:mm:ss}'", prefix, field, value);
            else if (value.GetType().IsEnum)
                sql.AppendFormat(",{0}{1}='{2}'", prefix, field, value);
            else
                sql.AppendFormat(",{0}{1}={2}", prefix, field, value);
            return true;
        }

        public static string ReplaceQuote(this string s)
        {
            if (s != null)
                return s.Replace("'", "''");
            return s;
        }


        public static string ValidACNT(string acnt)
        {
            if (acnt == null) return null;
            acnt = acnt.Trim().ReplaceQuote();
            if (acnt == string.Empty)
                return null;
            return acnt.ToLower();
        }

        //public static string EncodePassword(string acnt, string password)
        //{
        //    if (password == null)
        //        return null;
        //    password = password.Trim();
        //    if (password == string.Empty)
        //        return null;
        //    return Convert.ToBase64String(password.TripleDESEncrypt(acnt, null).MD5());
        //}

        public static byte? ToLockedValue(this string s)
        {
            if (s != null)
            {
                if (Regex.IsMatch(s, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
                    return 0;
                if (Regex.IsMatch(s, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
                    return 1;
            }
            return null;
        }

        //public static bool SplitMailAddress(this jstring s, out jstring acnt, out jstring corp)
        //{
        //    if ((s != null) && (s.Value != null))
        //    {
        //        int n = s.Value.IndexOf('@');
        //        if (n >= 0)
        //        {
        //            acnt = jstring.Create(s.Value.Substring(0, n));
        //            corp = jstring.Create(s.Value.Substring(n + 1));
        //        }
        //        else
        //        {
        //            acnt = s;
        //            corp = jstring.Null;
        //        }
        //        return true;
        //    }
        //    acnt = corp = null;
        //    return false;
        //}



        //public static int? ToID(this jstring s)
        //{
        //    if (s == null) return null;
        //    return s.Value.ToInt32();
        //}


        //public static string ToACNT(this jstring s)
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return null;
        //    return s.Value.ToLower().replace_quote();
        //}

        //public static string ToName(this jstring s)
        //{
        //    return s.ToStr();
        //}

        //public static string GetValue(this jstring s)
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return null;
        //    return s.Value;
        //}

        //public static string ToPassword(this jstring s, string acnt, string _default)
        //{
        //    string n;
        //    if (s == null)
        //        n = _default;
        //    else if (string.IsNullOrEmpty(s.Value))
        //        n = _default;
        //    else
        //        n = s.Value;
        //    if (n == null) return null;
        //    return Convert.ToBase64String(n.TripleDESEncrypt(acnt, null).MD5());
        //}

        //public static byte? ToLocked(this jstring s)
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return null;
        //    if (Regex.IsMatch(s.Value, "(false|f|0|no|n|off|undefined)", RegexOptions.IgnoreCase))
        //        return 0;
        //    if (Regex.IsMatch(s.Value, "(true|t|1|yes|y|on)", RegexOptions.IgnoreCase))
        //        return 1;
        //    return null;
        //}

        //public static T? ToEnum<T>(this jstring s, T? _default) where T : struct 
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return _default;
        //    return s.Value.ToEnum<T>() ?? _default;
        //}
        //public static string ToEnumString<T>(this jstring s, T? _default) where T : struct 
        //{
        //    T? n = s.ToEnum<T>(_default);
        //    if (n.HasValue)
        //        return n.Value.ToString();
        //    return null;
        //}

        /// <summary>
        /// 如果 user 的 CorpID=0, 則解析 CorpID, 否則傳回 User.CorpID
        /// </summary>
        /// <param name="s"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        //public static int? ToCorpID(this jstring s, SqlCmd sqlcmd, User user)
        //{
        //    if (user.CorpID == 0)
        //        return s.ToCorpID(sqlcmd);
        //    else
        //        return user.CorpID;
        //}
        //public static int? ToCorpID(this jstring s, SqlCmd sqlcmd)
        //{
        //    if (s == null) return null;
        //    return CorpList.GetInstance(null, sqlcmd).GetCorpID(s.Value);
        //}

        //public static string ToStr(this jstring s)
        //{
        //    if (s == null) return null;
        //    if (string.IsNullOrEmpty(s.Value)) return null;
        //    return s.Value.replace_quote();
        //}

        //public static string ValueOrDefault(string value, string _default)
        //{
        //    if (value == null)
        //        return _default;
        //    if (value.Trim() == string.Empty)
        //        return _default;
        //    return value;
        //}



        //public const string exp_ACNT = @"^([a-z]|[A-Z])[\w_]{3,20}$";

        //public static StringBuilder AppendFormatN(this StringBuilder sb, string format, object arg0)
        //{
        //    return sb.AppendFormat(format, arg0);
        //}

        //log.message(null, "{0}", Regex.IsMatch(this.acnt, exp_acnt));
        //string exp_acnt = @"^^{3,20}[a-zA-Z0-9]*$";
        //bool x = Regex.IsMatch(command.mail, ext_mail);
        //public static string operator *(string s1, Member_Update s2)
        //{
        //    s1 = (s1 ?? "").Trim();
        //    return string.IsNullOrEmpty(s1) ? null : s1;
        //}
        //public static bool ValidACNT()
        //{
        //    string ext_mail = @"^([a-zA-Z0-9._-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+";
        //    string exp_111 = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        //}
    }

    //class CorpIDJsonConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        serializer.Serialize(writer, CorpList.Instance.GetCorpACNT(value as int?));
    //    }
    //}
}