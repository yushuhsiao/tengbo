using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace System
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
    partial class StringEx
    {
        public static string ReadTo(this string str, ref int pos, char c)
        {
            int start, n;
            for (start = pos, n = str.Length; pos < n; pos++)
                if (str[pos] == c)
                    return str.Substring(start, pos++ - start);
            return null;
        }
        public static string ReadLine(this string str, ref int pos)
        {
            int start = pos;
            int n = str.Length - 1;
            char c;
            for (; pos < n; pos++)
            {
                c = str[pos];
                if ((pos == start) && (c == ' '))
                    start++;
                if ((c == '\r') && (str[pos + 1] == '\n'))
                {
                    string s = str.Substring(start, pos++ - start);
                    pos++;
                    return s;
                }
            }
            return null;
        }

        public static string SqlExport(this string str, object obj)
        {
            return StringEx.StringExport(str, null, obj, true);
        }
        public static string SqlExport(this string str, string id, object obj)
        {
            return StringEx.StringExport(str, id, obj, true);
        }
        public static void SqlExport(this StringBuilder dst, string str, string id, object obj)
        {
            StringExport(str, dst, id, obj, false);
        }

        public static string StringExport(this string str, string id, object obj)
        {
            return StringEx.StringExport(str, id, obj, false);
        }

        public static string StringExport(string str, string id, object obj, bool sql)
        {
            StringBuilder dst = new StringBuilder();
            StringExport(str, dst, id, obj, sql);
            return dst.ToString();
        }
        public static void StringExport(string str, StringBuilder dst, string id, object obj, bool sql)
        {
            Dictionary<string, object> obj_dict = obj as Dictionary<string, object>;
            StringExportContract contract = StringExportContract.GetContract(obj, id);
            int? n1 = null, n2 = null;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (n1.HasValue)
                {
                    if ((c == ':') && !n2.HasValue)
                        n2 = i;
                    else if (c == '}')
                    {
                        string field;
                        string fmt1;
                        string fmt2;
                        if (n2.HasValue)
                        {
                            field = str.Substring(n1.Value, n2.Value - n1.Value);
                            fmt1 = str.Substring(n2.Value + 1, i - n2.Value - 1);
                            fmt2 = string.Format("{{0:{0}}}", fmt1);
                        }
                        else
                        {
                            field = str.Substring(n1.Value, i - n1.Value);
                            fmt1 = null;
                            fmt2 = "{0}";
                        }
                        int nnn;
                        bool hasValue;
                        object value;
                        if (obj_dict == null)
                        {
                            hasValue = false;
                            value = null;
                            if (contract.ContainsKey(field))
                            {
                                PropertyInfo p = contract[field].p;
                                FieldInfo f = contract[field].f;
                                if ((p != null) && (p.CanRead))
                                {
                                    value = p.GetValue(p.IsStatic() ? null : obj, null);
                                    hasValue = true;
                                }
                                else if (f != null)
                                {
                                    value = f.GetValue(f.IsStatic ? null : obj);
                                    hasValue = true;
                                }
                            }
                        }
                        else if (hasValue = obj_dict.ContainsKey(field))
                            value = obj_dict[field];
                        else
                            value = null;

                        if (hasValue)
                        {
                            if (sql)
                            {
                                bool quote = (value is string) || (value is Guid) || (value is DateTime);
                                if (value == null)
                                    value = "null";
                                else
                                {
                                    Type t = value.GetType();
                                    if (t.IsEnum)
                                        value = Convert.ChangeType(value, Enum.GetUnderlyingType(t));
                                }
                                if (quote) dst.Append('\'');
                                dst.AppendFormat(fmt2, value);
                                if (quote) dst.Append('\'');
                            }
                            else
                                dst.AppendFormat(fmt2, value);
                        }
                        else if (sql && !int.TryParse(field, out nnn))
                        {
                            dst.Append('@');
                            dst.Append(field);
                        }
                        else
                        {
                            dst.Append('{');
                            dst.AppendFormat(fmt2, field);
                            dst.Append('}');
                        }
                        n1 = n2 = null;
                    }
                }
                else if (c == '{')
                {
                    n1 = i + 1;
                }
                else
                    dst.Append(c);
            }
        }

        public struct sql_str
        {
            public sql_str(string _string) { this.value = _string; }
            public string value;
            public override string ToString() { return value; }

            public static explicit operator string(sql_str value)
            {
                return value.value;
            }
            public static implicit operator sql_str(string value)
            {
                return new sql_str(value);
            }
            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public static sql_str Null = (sql_str)"null";
            public static sql_str getdate = (sql_str)"getdate()";
        }
    }

    [_DebuggerStepThrough]
    class StringExportContract : Dictionary<string, StringExportAttribute>
    {
        static readonly StringExportContract _null = new StringExportContract();
        static readonly Dictionary<Type, Group> _all = new Dictionary<Type, Group>();

        class Group : Dictionary<string, StringExportContract>
        {
            public StringExportContract _default = new StringExportContract();
            public Group(Type t)
            {
                List<MemberInfo> mm = new List<MemberInfo>();
                List<StringExportAttribute> aa = new List<StringExportAttribute>();
                for (Type tt = t; tt != null; tt = tt.BaseType)
                {
                    foreach (MemberInfo m in tt.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
                    {
                        PropertyInfo p = m as PropertyInfo;
                        FieldInfo f = m as FieldInfo;
                        if ((p == null) && (f == null)) continue;
                        mm.Add(m);
                        foreach (StringExportAttribute a in m.GetCustomAttributes(typeof(StringExportAttribute), true))
                        {
                            a.m = m;
                            a.p = p;
                            a.f = f;
                            a.Name = a.Name ?? m.Name;
                            aa.Add(a);
                        }
                    }
                }
                bool empty = aa.Count == 0;
                if (empty)
                {
                    foreach (MemberInfo m in mm)
                        aa.Add(new StringExportAttribute() { m = m, p = m as PropertyInfo, f = m as FieldInfo, Name = m.Name });
                }
                foreach (StringExportAttribute a in aa)
                {
                    StringExportContract c;
                    if (a.ID == null)
                        c = _default;
                    else if (!this.TryGetValue(a.ID, out c))
                        c = this[a.ID] = new StringExportContract();
                    if (!c.ContainsKey(a.Name))
                        c[a.Name] = a;
                }
            }
        }
        public static StringExportContract GetContract(object obj, string id)
        {
            if (obj == null)
                return _null;
            if (obj is Dictionary<string, object>)
                return _null;
            Type t = obj.GetType();

            Group g;
            lock (_all)
            {
                if (!_all.TryGetValue(t, out g))
                    g = _all[t] = new Group(t);
            }
            if (id == null)
                return g._default;
            else if (g.ContainsKey(id))
                return g[id];
            else
                return _null;
        }
    }

    //[_DebuggerStepThrough]
    //class StringExportContractGroup : Dictionary<string, StringExportContract>
    //{
    //    public readonly StringExportContract _default;
    //    public StringExportContractGroup(Type t)
    //    {

    //        foreach (MemberInfo m in t.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    //        {
    //            foreach (StringExportAttribute a in m.GetCustomAttributes(typeof(StringExportAttribute), true))
    //            {
    //                a.p = m as PropertyInfo;
    //                a.f = m as FieldInfo;
    //                if ((a.p != null) || (a.f != null))
    //                {
    //                    a.m = m;
    //                    string name = a.Name ?? m.Name;
    //                    StringExportContract c;
    //                    if (a.ID == null)
    //                        c = this._default;
    //                    else if (!this.ContainsKey(a.ID))
    //                        c = this[a.ID] = new StringExportContract();
    //                    else
    //                        c = this[a.ID];
    //                    if (!c.ContainsKey(name))
    //                        c[name] = new List<StringExportAttribute>();
    //                    c[name].Add(a);
    //                }
    //            }
    //        }
    //    }
    //}

    [_DebuggerStepThrough]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class StringExportAttribute : Attribute
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public StringExportAttribute() { }
        public StringExportAttribute(string name) { this.Name = name; }
        public StringExportAttribute(string name, string id) : this(name) { this.ID = id; }

        internal MemberInfo m;
        internal PropertyInfo p;
        internal FieldInfo f;

    }
}
namespace System.Text
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
    public static class Extensions
    {
        public static string ReadTo(this StringBuilder sb, ref int pos, char c)
        {
            int start, n;
            for (start = pos, n = sb.Length; pos < n; pos++)
                if (sb[pos] == c)
                    return sb.ToString(start, pos++ - start);
            return null;
        }
        public static string ReadLine(this StringBuilder sb, ref int pos)
        {
            int start = pos;
            int n = sb.Length - 1;
            char c;
            for (; pos < n; pos++)
            {
                c = sb[pos];
                if ((pos == start) && (c == ' '))
                    start++;
                if ((c == '\r') && (sb[pos + 1] == '\n'))
                {
                    string s = sb.ToString(start, pos++ - start);
                    pos++;
                    return s;
                }
            }
            return null;
        }
    }
}
