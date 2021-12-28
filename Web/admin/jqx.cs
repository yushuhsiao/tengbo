using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;
using Tools;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace web
{
    public static class jqx
    {
        public class grid_opt
        {
            public bool disabled = false;
            public int width = 600;
            public int height = 400;
            public override string ToString()
            {
                return api.SerializeObject(this);
            }
        }

        public class column
        {
            public string datafield;
            public string text;
            public string _text;
            public string ToString(IPageLang src)
            {
                trim(ref this.datafield);
                this.text = this.text ?? src[this._text];
                this._text = null;
                return api.SerializeObject(this) + ",";
            }

            static void trim(ref string n)
            {
                if (n == null) return;
                n = n.Trim();
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class _sort
        {
            [JsonProperty]
            public string sortcolumn;
            public string sort_sql;
            [JsonProperty]
            public _sortdirection sortdirection;

            public override string ToString()
            {
                return string.Format(this.sort_sql, this.sortdirection);
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class _sortdirection
        {
            [JsonProperty]
            public bool? ascending;
            [JsonProperty]
            public bool? descending;

            public override string ToString()
            {
                if (this.ascending == true)
                    return " asc";
                if (this.descending == true)
                    return " desc";
                return null;
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class _filter
        {
            [JsonProperty("operator")]
            public string op;
            [JsonProperty]
            public string datafield;
            [JsonProperty("filters")]
            public List<_filter_item> items;

            public static object parseInt32(string src)
            {
                int n;
                if (src.ToInt32(out n))
                    return n;
                else
                    return null;
            }

            public static void add_sql(_filter f, StringBuilder sql, ref int count, string format, Func<string, object> parse)
            {
                int _cnt1 = count;
                string sql_a = "(", sql_b = ")";
                for (int i = 0; i < f.items.Count; i++)
                {
                    object value = parse(f.items[i].value);
                    if (value == null) continue;
                    if (count == _cnt1)
                    {
                        sql.Append(count == 0 ? " where " : " and ");
                        count++;
                    }
                    if (sql_a == null)
                    {
                        if (f.op == "and") sql_a = " and ";
                        else break;
                    }
                    sql.Append(sql_a); sql_a = null;
                    sql.AppendFormat(format, value);
                }
                if (sql_a == null) sql.Append(sql_b);
            }
        }

        public enum _filter_type
        {
            numericfilter,
            stringfilter,
            datefilter,
            booleanfilter,
            custom,
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class _filter_item
        {
            [JsonProperty]
            public string value;
            [JsonProperty]
            public string condition;
            [JsonProperty("operator")]
            public object op;
            [JsonProperty]
            public _filter_type type;
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class ColumnAttribute : Attribute
        {
            /// <summary>
            /// field (UI)
            /// </summary>
            public string field;
            public bool sortdefault = false;
            /// <summary>
            /// field (Sql)
            /// </summary>
            public string sort_sql;
            public SortOrder sortorder = SortOrder.Ascending;
            public bool IsFilterDefault = false;
        }

        public abstract class GridRequest
        {
            [JsonProperty]
            public int? filterscount;
            [JsonProperty]
            public int? groupscount;
            [JsonProperty]
            public int? pagenum;
            [JsonProperty]
            public int? pagesize;
            [JsonProperty]
            public int? recordstartindex;
            [JsonProperty]
            public int? recordendindex;
            public int? rows_start { get { return this.pagenum * this.pagesize; } }
            public int? rows_end { get { return (this.pagenum + 1) * this.pagesize; } }

            //[JsonProperty("_pager")]
            //public _pager pager;

            //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
            //public class _pager
            //{
            //    [JsonProperty]
            //    public int? pagenum;
            //    [JsonProperty]
            //    public int? pagesize;
            //    [JsonProperty]
            //    public int? pagescount;
            //}

            [JsonProperty("_filter")]
            _filter[] __filters
            {
                set
                {
                    if (value == null) return;
                    if (value.Length == 0) return;
                    this.filters = new List<_filter>();
                    foreach (_filter f1 in value)
                    {
                        if (f1.op != null) f1.op = f1.op.Trim().ToLower();
                        _filter f2;
                        if (this.getFilter(f1.datafield, out f2))
                        {
                            f2.op = f2.op ?? f1.op;
                            f2.items.AddRange(f1.items);
                        }
                        else if (f1.items.Count > 0)
                            this.filters.Add(f1);
                    }
                    User user = HttpContext.Current.User as User;
                    if (user.CorpID != 0)
                    {
                        _filter f_CorpID;
                        if (!this.getFilter("CorpID", out f_CorpID))
                            this.filters.Add(f_CorpID = new _filter());
                        while (f_CorpID.items.Count > 1)
                            f_CorpID.items.RemoveAt(1);
                        f_CorpID.items[0].value = user.CorpID.ToString();
                    }
                }
            }

            protected List<_filter> filters = _null<List<_filter>>.value;
            protected _filter getFilter(string name)
            {
                List<_filter> filters = this.filters;
                for (int i = 0; i < filters.Count; i++)
                    if (filters[i].datafield == name)
                        return filters[i];
                return null;
            }
            protected bool getFilter(string name, out _filter value)
            {
                return (value = this.getFilter(name)) != null;
            }

            [JsonProperty("_sort")]
            protected _sort sort;
            public abstract _sort GetSort();
        }
        public abstract class GridRequest<T1, T2> : GridRequest where T1 : GridRequest<T1, T2>
        {
            static List<ColumnAttribute> columns;
            static _sort _sortkeys_default;
            static List<_sort> _sortkeys;
            static void init_columns()
            {
                lock (typeof(T1))
                {
                    if (columns == null)
                    {
                        columns = new List<ColumnAttribute>();
                        _sortkeys = new List<_sort>();
                        foreach (ColumnAttribute c in typeof(T1).GetCustomAttributes(typeof(ColumnAttribute), true))
                        {
                            if (c.field == null) continue;
                            c.field = c.field.Trim();
                            c.sort_sql = (c.sort_sql ?? c.field).Trim();
                            _sort s = new _sort()
                            {
                                sortcolumn = c.field,
                                sort_sql = c.sort_sql,
                                sortdirection = new _sortdirection()
                                {
                                    ascending = c.sortorder == SortOrder.Ascending ? true : (bool?)null,
                                    descending = c.sortorder == SortOrder.Descending ? true : (bool?)null,
                                }
                            };
                            _sortkeys.Add(s);
                            if (c.sortdefault)
                                _sortkeys_default = _sortkeys_default ?? s;

                        }
                    }
                }
            }
            static List<_sort> get_sortkeys()
            {
                lock (typeof(T1))
                {
                    init_columns();
                    return _sortkeys;
                }
            }

            protected abstract GridResponse<T2> execute(T1 command, string json_s, params object[] args);

            static GridRequest()
            {
            }

            public override _sort GetSort()
            {
                List<_sort> src;
                _sort d;
                lock (typeof(T1))
                {
                    init_columns();
                    src = _sortkeys;
                    d = _sortkeys_default;
                }
                _sort input = base.sort;
                foreach (_sort value in src)
                {
                    if (input.sortcolumn == value.sortcolumn)
                    {
                        input.sort_sql = value.sort_sql;
                        return input;
                    }
                }
                return d;
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class GridResponse<T>
        {
            [JsonProperty]
            public List<T> rows = new List<T>();
            [JsonProperty]
            public int? totalrecords;
            [JsonProperty]
            public bool? loadallrecords;
        }
    }
}