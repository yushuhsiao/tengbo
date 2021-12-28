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
using BU;
using System.Reflection;

namespace web
{
    public interface sqltool_values
    {
        object this[string field] { get; set; }
        bool ContainsKey(string field);
    }
    public class sqltool : sqltool_values
    {
        static readonly List<string> _null_list = new List<string>();
        static readonly Dictionary<string, string> _null_dict = new Dictionary<string, string>();
        public Dictionary<string, object> values = new Dictionary<string, object>();
        public List<string> fields = new List<string>();
        List<string> needs;
        Dictionary<string, string> flags;

        // flags :
        //  * - 必要欄位
        //  N - nvarchar
        //  d - 日期
        //  t - 時間

        //public object this[string flag, string field]
        //{
        //    set { if (field == null) return; this[flag, field.Trim(), string.Empty] = value; }
        //}
        public object this[string flag, string field, string fill]
        {
            set { this[null, flag, field, fill] = value; }
        }
        public object this[SqlSchemaTable schema, string flag, string field, string fill]
        {
            set
            {
                field = field.Trim();
                if ((schema != null) && (!schema.ContainsKey(field))) return;
                flag = flag.Replace(" ", "");
                if (!string.IsNullOrEmpty(flag))
                {
                    if (flag.Contains('*'))
                    {
                        if (value == null)
                        {
                            if (this.needs == null)
                                this.needs = new List<string>();
                            if (!this.needs.Contains(field))
                                this.needs.Add(field);
                        }
                    }
                    if (this.flags == null)
                        this.flags = new Dictionary<string, string>();
                    this.flags[field] = flag;
                }

                if (value != null)
                {
                    if (!this.fields.Contains(field))
                        this.fields.Add(field);
                    if ((value is CurrencyCode) || (value is CurrencyCode?))
                        this.values[field] = Convert.ToString(value);
                    else
                        this.values[field] = value;
                }
            }
        }

        public object this[string flag, string field, string fill1, object oldValue, string fill2]
        {
            set
            {
                if (object.Equals(value, oldValue)) return;
                //if (value == null)
                //{ if (oldValue == null) return; }
                //else
                //{ if (value.Equals(oldValue)) return; }
                this[flag, field, fill1] = value;
            }
        }

        public sqltool_values Values { get { return this; } }
        object sqltool_values.this[string field]
        {
            get { if (values.ContainsKey(field)) return values[field]; return null; }
            set { this.values[field] = value; }
        }
        bool sqltool_values.ContainsKey(string field)
        {
            return this.values.ContainsKey(field);
        }

        /// <summary>
        /// [Field1],[Field2],[Field3]
        /// </summary>
        public const char _Fields = '1';
        /// <summary>
        /// {Field1},{Field2},{Field3}
        /// </summary>
        public const char _Values = '2';
        /// <summary>
        /// @Field1={Field1},@Field2={Field2},@Field3={Field3}
        /// </summary>
        public const char _AtFieldValue = '3';
        /// <summary>
        /// [Field1]={Field1},[Field2]={Field2},[Field3]={Field3}
        /// </summary>
        public const char _FieldValue = '4';
        /// <summary>
        /// [Field1]={Field1} and [Field2]={Field2} and [Field3]={Field3}
        /// </summary>
        public const char _AndFieldValue = '5';

        public void TestFieldNeeds()
        {
            //if (this.flags == null) return;
            //List<string> err = null;
            //foreach (KeyValuePair<string, string> p in this.flags)
            //{
            //    if (!p.Value.Contains('*')) continue;
            //    if (this.values.ContainsKey(p.Key)) continue;
            //    (err = err ?? new List<string>()).Add(p.Key);
            //}
            //if (err != null)
            //    throw new RowException(RowErrorCode.FieldNeeds, null, this.needs);
            if (this.needs != null)
                throw new RowException(RowErrorCode.FieldNeeds, null, this.needs);
        }

        public string Build(params object[] args)
        {
            StringBuilder sql = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string)
                    sql.Append(args[i]);
                else if (args[i] is char)
                {
                    Dictionary<string, string> flags = this.flags ?? _null_dict;
                    char op = (char)args[i];
                    bool f, at, eq, v;
                    string sep = ",";

                    if (op == _Fields)
                    { f = true; eq = false; v = false; at = false; }
                    else if (op == _Values)
                    { f = false; eq = false; v = true; at = false; }
                    else if (op == _AtFieldValue)
                    { f = v = eq = true; at = true; }
                    else if (op == _FieldValue)
                    { f = v = eq = true; at = false; }
                    else if (op == _AndFieldValue)
                    { f = v = eq = true; at = false; sep = " and "; }
                    else continue;

                    for (int index = 0; index < this.fields.Count; index++)
                    {
                        string field = this.fields[index];
                        string flag;
                        if (!flags.TryGetValue(field, out flag))
                            flag = string.Empty;
                        if (index > 0) sql.Append(sep);
                        if (f)
                        {
                            if (at)
                            {
                                sql.Append('@');
                                sql.Append(field);
                            }
                            else
                            {
                                sql.Append('[');
                                sql.Append(field);
                                sql.Append(']');
                            }
                        }
                        if (eq) sql.Append('=');
                        if (v)
                        {
                            if (flag.Contains('N'))
                                sql.Append('N');
                            sql.Append('{');
                            sql.Append(field);
                            object value;
                            if (values.TryGetValue(field, out value))
                            {
                                if (value is DateTime)
                                {
                                    sql.Append(':');
                                    if (flag.Contains('d'))
                                        sql.Append(DateFormat);
                                    else if (flag.Contains('t'))
                                        sql.Append(TimeFormat);
                                    else
                                        sql.Append(DateTimeFormat);
                                }
                            }
                            sql.Append('}');
                        }
                    }
                }
            }
            return sql.ToString();
        }

        public string Build2(params object[] args)
        {
            StringBuilder sql = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string)
                    sql.Append(args[i]);
                else if (args[i] is char)
                {
                    Dictionary<string, string> flags = this.flags ?? _null_dict;
                    char op = (char)args[i];
                    bool f, at, eq, v;
                    string sep = ",";

                    if (op == _Fields)
                    { f = true; eq = false; v = false; at = false; }
                    else if (op == _Values)
                    { f = false; eq = false; v = true; at = false; }
                    else if (op == _AtFieldValue)
                    { f = v = eq = true; at = true; }
                    else if (op == _FieldValue)
                    { f = v = eq = true; at = false; }
                    else if (op == _AndFieldValue)
                    { f = v = eq = true; at = false; sep = " and "; }
                    else continue;

                    for (int index = 0; index < this.fields.Count; index++)
                    {
                        string field = this.fields[index];
                        string flag;
                        if (!flags.TryGetValue(field, out flag))
                            flag = string.Empty;
                        if (index > 0) sql.Append(sep);
                        if (f)
                        {
                            if (at)
                            {
                                sql.Append('@');
                                sql.Append(field);
                            }
                            else
                            {
                                sql.Append('[');
                                sql.Append(field);
                                sql.Append(']');
                            }
                        }
                        if (eq) sql.Append('=');
                        if (v)
                        {
                            object value;
                            bool hasValue = values.TryGetValue(field, out value);
                            if (value is StringEx.sql_str)
                            {
                                sql.Append(value);
                            }
                            else
                            {
                                if (flag.Contains('N'))
                                    sql.Append('N');
                                sql.Append('{');
                                sql.Append(field);
                                if (hasValue)
                                {
                                    if (value is DateTime)
                                    {
                                        sql.Append(':');
                                        if (flag.Contains('d'))
                                            sql.Append(DateFormat);
                                        else if (flag.Contains('t'))
                                            sql.Append(TimeFormat);
                                        else
                                            sql.Append(DateTimeFormat);
                                    }
                                }
                                sql.Append('}');
                            }
                        }
                    }
                }
            }
            return sql.ToString();
        }

        public string BuildEx(params object[] args)
        {
            return this.SqlExport(this.Build(args));
        }
        public string BuildEx2(params object[] args)
        {
            return this.SqlExport(this.Build2(args));
        }

        public const string CreateTime = "CreateTime";
        public const string ModifyTime = "ModifyTime";
        public const string CreateUser = "CreateUser";
        public const string ModifyUser = "ModifyUser";
        public void SetUser(params string[] fields)
        {
            int? userID = User.CurrentUserID;
            for (int i = 0; i < fields.Length; i++)
            {
                switch (fields[i])
                {
                    case sqltool.CreateTime:
                    case sqltool.ModifyTime: this["", fields[i], ""] = StringEx.sql_str.getdate; break;
                    case sqltool.CreateUser:
                    case sqltool.ModifyUser: this["", fields[i], ""] = userID; break;
                }
            }
        }

        public static string DateFormat = "yyyy-MM-dd";
        public static string TimeFormat = "HH:mm:ss";
        public static string DateTimeFormat = DateFormat + " " + TimeFormat;

        public string SqlExport(string pattern)
        {
            return pattern.SqlExport(null, this.values);
        }
    }

    //public static class _sqlcmd
    //{
    //    // args 任一值都不可以為 null
    //    public static bool GetSingleRow<TRow>(this SqlCmd sqlcmd, out TRow row, string format, params object[] keys) where TRow : new()
    //    {
    //        for (int i = 0; i < keys.Length; i++)
    //        {
    //            if (keys[i] == null)
    //            {
    //                row = default(TRow);
    //                return false;
    //            }
    //        }
    //        row = sqlcmd.ToObject<TRow>(format, keys);
    //        return row != null;
    //    }

    //    public static jgrid.RowResponse Execute<TRow>(this SqlCmd sqlcmd, string format, params object[] args) where TRow : class, new()
    //    {
    //        jgrid.RowResponse res;
    //        TRow row;
    //        if (sqlcmd.Execute(out row, out res, format, args))
    //            return jgrid.RowResponse.Success(row);
    //        else
    //            return res;
    //    }
    //    public static bool Execute<TRow>(this SqlCmd sqlcmd, out TRow row, out jgrid.RowResponse res, string format, params object[] args) where TRow : class, new()
    //    {
    //        row = null;
    //        try
    //        {
    //            sqlcmd.BeginTransaction();
    //            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(format, args))
    //            {
    //                RowException.Test(r, "err", "msg");
    //                if (row == null)
    //                    row = r.ToObject<TRow>();
    //                else
    //                    r.FillObject(row);
    //            }
    //            RowException.TestNull(row);
    //            sqlcmd.Commit();
    //            //sqlcmd.Rollback();
    //            res = jgrid.RowResponse.Success(row);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            sqlcmd.Rollback();
    //            log.error_msg(ex);
    //            res = jgrid.RowResponse.Error(ex);
    //            return false;
    //        }
    //    }
    //}
}