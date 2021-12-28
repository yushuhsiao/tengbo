using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System
{
    [_DebuggerStepThrough]
    public static class DateTimeEx
    {
        public static string DateTimeText(this DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static readonly DateTime UnixBaseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        static long unix_time_base_tick = UnixBaseTime.Ticks;

        public static long ToUnixTime(this DateTime t)
        {
            return (t.Ticks - unix_time_base_tick) / 10000;
        }
        public static DateTime FromUnixTime(long unixtime)
        {
            return new DateTime((unixtime + unix_time_base_tick) * 10000);
        }

        public static DateTime ToACTime(this DateTime datetime)
        {
            return datetime.AddHours(-12).Date;
        }
    }

    [_DebuggerStepThrough]
    public static partial class StringEx
    {

        public delegate bool TryParseHandler<T>(string value, out T result);
        public delegate T? GetValueHandler<T>(string s) where T : struct;
        

        //public static byte[] GetBytes(this string s, Encoding encoding)
        //{
        //    return encoding.GetBytes(s);
        //}
        //public static string ToBase64String(this byte[] b)
        //{
        //    return Convert.ToBase64String(b);
        //}
        //public static byte[] GetBytesFromBase64String(this string s)
        //{
        //    return Convert.FromBase64String(s);
        //}

        public static bool In<T>(this T? n, params T[] args) where T : struct
        {
            if (n.HasValue)
                return args.Conatins(n.Value);
            return false;
        }
        public static bool In<T>(this T n, params T[] args) where T : struct
        {
            return args.Conatins(n);
        }

        public static bool Conatins<T>(this T[] src, T? value) where T : struct
        {
            if (value.HasValue)
                return src.Conatins(value.Value);
            return false;
        }
        public static bool Conatins<T>(this T[] src, T value)
        {
            if (src != null)
                foreach (T s in src)
                    if (EqualityComparer<T>.Default.Equals(s, value))
                        return true;
            return false;
        }
        public static bool Conatins(this Array src, object value)
        {
            if (src != null)
                foreach (object s in src)
                    if (s == value)
                        return true;
            return false;
        }


        public static bool TryGetValue<T>(this String a, out T value, TryParseHandler<T> tryParse)
        {
            try
            {
                if (!string.IsNullOrEmpty(a))
                    if (tryParse != null)
                        return tryParse(a, out value);
            }
            catch
            {
            }
            value = default(T);
            return false;
        }

        //public static bool ToValue(this String s, Type type, out object value)
        //{
        //    if (s != null)
        //    {
        //        if (type == typeof(string))
        //        {
        //            value = s;
        //            return true;
        //        }
        //        else if ((type == typeof(Boolean)) || (type == typeof(Boolean?)))
        //        {
        //            Boolean n; if (s.ToBoolean(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(SByte)) || (type == typeof(SByte?)))
        //        {
        //            SByte n; if (s.ToSByte(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Byte)) || (type == typeof(Byte?)))
        //        {
        //            Byte n; if (s.ToByte(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Int16)) || (type == typeof(Int16?)))
        //        {
        //            Int16 n; if (s.ToInt16(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(UInt16)) || (type == typeof(UInt16?)))
        //        {
        //            UInt16 n; if (s.ToUInt16(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Int32)) || (type == typeof(Int32?)))
        //        {
        //            Int32 n; if (s.ToInt32(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(UInt32)) || (type == typeof(UInt32?)))
        //        {
        //            UInt32 n; if (s.ToUInt32(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Int64)) || (type == typeof(Int64?)))
        //        {
        //            Int64 n; if (s.ToInt64(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(UInt64)) || (type == typeof(UInt64?)))
        //        {
        //            UInt64 n; if (s.ToUInt64(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Single)) || (type == typeof(Single?)))
        //        {
        //            Single n; if (s.ToSingle(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Double)) || (type == typeof(Double?)))
        //        {
        //            Double n; if (s.ToDouble(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(Decimal)) || (type == typeof(Decimal?)))
        //        {
        //            Decimal n; if (s.ToDecimal(out n)) { value = n; return true; }
        //        }
        //        else if ((type == typeof(DateTime)) || (type == typeof(DateTime?)))
        //        {
        //            DateTime n; if (s.ToDateTime(out n)) { value = n; return true; }
        //        }
        //        else if (type == typeof(IPAddress))
        //        {
        //            IPAddress n; if (s.ToIPAddress(out n)) { value = n; return true; }
        //        }
        //    }
        //    value = null;
        //    return false;
        //}

        //public static string ToSqlCommandText<T>(this Nullable<T> value) where T : struct
        //{
        //    if (value.HasValue)
        //        return value.ToString();
        //    else
        //        return "null";
        //}

        public static Boolean? ToBoolean(this String s) /*********************/ { Boolean v; if (s.TryGetValue(out v, Boolean.TryParse)) return v; return null; }
        public static bool ToBoolean(this String s, out Boolean result) /*****/ { return s.TryGetValue(out result, Boolean.TryParse); }

        public static Byte? ToByte(this String s) /***************************/ { Byte v; if (s.TryGetValue(out v, Byte.TryParse)) return v; return null; }
        public static bool ToByte(this String s, out Byte result) /***********/ { return s.TryGetValue(out result, Byte.TryParse); }

        public static SByte? ToSByte(this String s) /*************************/ { SByte v; if (s.TryGetValue(out v, SByte.TryParse)) return v; return null; }
        public static bool ToSByte(this String s, out SByte result) /*********/ { return s.TryGetValue(out result, SByte.TryParse); }

        public static Int16? ToInt16(this String s) /*************************/ { Int16 v; if (s.TryGetValue(out v, Int16.TryParse)) return v; return null; }
        public static bool ToInt16(this String s, out Int16 result) /*********/ { return s.TryGetValue(out result, Int16.TryParse); }

        public static UInt16? ToUInt16(this String s) /***********************/ { UInt16 v; if (s.TryGetValue(out v, UInt16.TryParse)) return v; return null; }
        public static bool ToUInt16(this String s, out UInt16 result) /*******/ { return s.TryGetValue(out result, UInt16.TryParse); }

        public static Int32? ToInt32(this String s) /*************************/ { Int32 v; if (s.TryGetValue(out v, Int32.TryParse)) return v; return null; }
        public static bool ToInt32(this String s, out Int32 result) /*********/ { return s.TryGetValue(out result, Int32.TryParse); }

        public static UInt32? ToUInt32(this String s) /***********************/ { UInt32 v; if (s.TryGetValue(out v, UInt32.TryParse)) return v; return null; }
        public static bool ToUInt32(this String s, out UInt32 result) /*******/ { return s.TryGetValue(out result, UInt32.TryParse); }

        public static Int64? ToInt64(this String s) /*************************/ { Int64 v; if (s.TryGetValue(out v, Int64.TryParse)) return v; return null; }
        public static bool ToInt64(this String s, out Int64 result) /*********/ { return s.TryGetValue(out result, Int64.TryParse); }

        public static UInt64? ToUInt64(this String s) /***********************/ { UInt64 v; if (s.TryGetValue(out v, UInt64.TryParse)) return v; return null; }
        public static bool ToUInt64(this String s, out UInt64 result) /*******/ { return s.TryGetValue(out result, UInt64.TryParse); }

        public static Single? ToSingle(this String s) /***********************/ { Single v; if (s.TryGetValue(out v, Single.TryParse)) return v; return null; }
        public static bool ToSingle(this String s, out Single result) /*******/ { return s.TryGetValue(out result, Single.TryParse); }

        public static Double? ToDouble(this String s) /***********************/ { Double v; if (s.TryGetValue(out v, Double.TryParse)) return v; return null; }
        public static bool ToDouble(this String s, out Double result) /*******/ { return s.TryGetValue(out result, Double.TryParse); }

        public static Decimal? ToDecimal(this String s) /*********************/ { Decimal v; if (s.TryGetValue(out v, Decimal.TryParse)) return v; return null; }
        public static bool ToDecimal(this String s, out Decimal result) /*****/ { return s.TryGetValue(out result, Decimal.TryParse); }

        public static DateTime? ToDateTime(this String s) /*******************/ { DateTime v; if (s.TryGetValue(out v, DateTime.TryParse)) return v; return null; }
        public static bool ToDateTime(this String s, out DateTime result) /***/ { return s.TryGetValue(out result, DateTime.TryParse); }

        public static Guid? ToGuid(this String s) /***************************/ { try { return new Guid(s); } catch { return null; } }
        public static bool ToGuid(this String s, out Guid result) /***********/ { try { result = new Guid(s); return true; } catch { result = default(Guid); return false; } }

        public static T? ToEnum<T>(this String s) where T : struct
        {
            return s.ToEnum<T>(true);
        }
        public static T? ToEnum<T>(this String s, bool ignoreCase) where T : struct
        {
            if ((s != null) && typeof(T).IsEnum)
            {
                try
                {
                    T n = (T)Enum.Parse(typeof(T), s.Trim(), ignoreCase);
                    if (Enum.IsDefined(typeof(T), n))
                        return n;
                }
                catch { }
            }
            return null;
        }

        public static IPAddress ToIPAddress(this String s) /******************/ { IPAddress n; if (!string.IsNullOrEmpty(s) && IPAddress.TryParse(s, out n)) return n; return null; }
        public static bool ToIPAddress(this String s, out IPAddress result)
        {
            if (!string.IsNullOrEmpty(s) && IPAddress.TryParse(s, out result))
                return true;
            result = default(IPAddress);
            return false;
        }

        public static IPEndPoint ToIPEndPoint(this String s) { IPEndPoint ip; if (s.ToIPEndPoint(out ip)) return ip; else return null; }
        public static bool ToIPEndPoint(this String s, out IPEndPoint result)
        {
            if (!string.IsNullOrEmpty(s))
            {
                string[] ss = s.Split(':');
                if (ss.Length >= 2)
                {
                    IPAddress ip;
                    int port;
                    if (IPAddress.TryParse(ss[0], out ip) && int.TryParse(ss[1], out port))
                    {
                        result = new IPEndPoint(ip, port);
                        return true;
                    }
                }
            }
            result = null;
            return false;
        }



        public static byte? Max(this byte? val1, byte? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static decimal? Max(this decimal? val1, decimal? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static double? Max(this double? val1, double? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static float? Max(this float? val1, float? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static int? Max(this int? val1, int? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static long? Max(this long? val1, long? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static sbyte? Max(this sbyte? val1, sbyte? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static short? Max(this short? val1, short? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static uint? Max(this uint? val1, uint? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static ulong? Max(this ulong? val1, ulong? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static ushort? Max(this ushort? val1, ushort? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Max(val1.Value, val2.Value);
        }
        public static byte? Min(this byte? val1, byte? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static decimal? Min(this decimal? val1, decimal? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static double? Min(this double? val1, double? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static float? Min(this float? val1, float? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static int? Min(this int? val1, int? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static long? Min(this long? val1, long? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static sbyte? Min(this sbyte? val1, sbyte? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static short? Min(this short? val1, short? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static uint? Min(this uint? val1, uint? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static ulong? Min(this ulong? val1, ulong? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }
        public static ushort? Min(this ushort? val1, ushort? val2)
        {
            if (val1 == null) return val2;
            else if (val2 == null) return val1;
            else return Math.Min(val1.Value, val2.Value);
        }



    }
}