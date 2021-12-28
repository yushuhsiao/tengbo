using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using Tools;

namespace web
{
    public class tran_page : page { }

    public class tranhist_page : tran_page { }

    public class tran_ascx : usercontrol
    {
        public string PermissionsCode1 { get; set; }
        public string PermissionsCode2 { get; set; }
    }

    public interface tran_master
    {
        bool IsHist { get; }
        int? UserID { get; }
        LogType[] LogTypes1 { get; }
        LogType[] LogTypes2 { get; }
        UserType UserType { get; }
        Type SelectCommandType { get; }
        Type RowCommandType { get; }
    }

    partial class tran
    {
        partial class RowCommand<TRowData, TRowCommand>
        {
            public abstract class tran_master<TUser, _TSelectCommand, _TRowCommand> : masterpage, tran_master where TUser : UserRow, new()
            {
                public int? UserID { get; private set; }
                public string url_id { get { if (this.UserID.HasValue) return string.Format("?id={0}", this.UserID); else return null; } }
                public bool IsHist { get { return this.Page is tranhist_page; } }
                public UserType UserType { get { return _null<TUser>.value.UserType; } }
                public LogType[] LogTypes1 { get { return _null<TRowCommand>.value.AcceptLogTypes; } }
                public LogType[] LogTypes2 { get { return _null<TRowCommand>.value.AcceptLogTypes; } }

                static MenuRow s_link1;
                static MenuRow s_link2;
                public MenuRow link1 { get { lock (typeof(TRowCommand)) return s_link1 = s_link1 ?? MenuRow.Cache.Instance.GetItem(this.link_key1); } }
                public MenuRow link2 { get { lock (typeof(TRowCommand)) return s_link2 = s_link2 ?? MenuRow.Cache.Instance.GetItem(this.link_key2); } }
                protected abstract string link_key1 { get; }
                protected abstract string link_key2 { get; }

                public Type SelectCommandType { get { return typeof(_TSelectCommand); } }
                public Type RowCommandType { get { return typeof(_TRowCommand); } }

                protected override void OnLoad(EventArgs e)
                {
                    this.UserID = Request.QueryString["id"].ToInt32();
                    base.OnLoad(e);
                }

                [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
                public class SelectCommand : jgrid.GridRequest<SelectCommand>
                {
                    [JsonProperty]
                    public bool? IsHist;
                    [JsonProperty]
                    public LogType? LogType;

                    protected override string init_defaultkey() { throw new NotImplementedException(); }
                    protected override Dictionary<string, string> init_sortkeys() { lock (typeof(TRowCommand)) return _sortkeys; }
                    static Dictionary<string, string> _sortkeys;

                    protected virtual void filter(StringBuilder sql, ref int cnt, string name, string value)
                    {
                        switch (name)
                        {
                            case "SerialNumber": sql_where(sql, ref cnt, "lower(SerialNumber) like lower('%{0}%')", (value * text.ValidAsString).Remove("%")); break;
                            case "UserID": sql_where(sql, ref cnt, "UserID = {0}", value.ToInt32()); break;
                            case "State": sql_where(sql, ref cnt, "State = {0}", (int?)value.ToEnum<BU.TranState>()); break;
                            case "GameID": sql_where(sql, ref cnt, "GameID = {0}", (int?)value.ToEnum<BU.GameID>()); break;
                            case "ParentACNT": sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (value * text.ValidAsACNT).Remove("%")); break;
                            case "UserACNT": sql_where(sql, ref cnt, "UserACNT like '%{0}%'", (value * text.ValidAsACNT).Remove("%")); break;
                            case "Amount1": sql_where(sql, ref cnt, "Amount1 = {0}", value.ToDecimal()); break;
                            case "RequestIP": sql_where(sql, ref cnt, "RequestIP like '%{0}%'", (value * text.ValidAsString).Remove("%")); break;
                            case "CreateUser": sql_where(sql, ref cnt, "CreateUser = {0}", value.ToInt32()); break;
                            case "ModifyUser": sql_where(sql, ref cnt, "ModifyUser = {0}", value.ToInt32()); break;
                        }
                    }

                    public virtual object select(string json_s, params object[] args)
                    {
                        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
                        {
                            SqlSchemaTable schema = SqlSchemaTable.GetSchema(sqlcmd, (string)_null<TRowCommand>.value.TranTable1);
                            lock (typeof(TRowCommand))
                            {
                                if (_sortkeys == null)
                                {
                                    _sortkeys = new Dictionary<string, string>();
                                    foreach (string s in schema.Keys)
                                        _sortkeys[s] = s;
                                }
                            }

                            StringBuilder sql = new StringBuilder();
                            this.IsHist = this.IsHist ?? false;
                            StringEx.sql_str tableName = this.IsHist == false ? _null<TRowCommand>.value.TranTable1 : _null<TRowCommand>.value.TranTable2;
                            sql.AppendFormat("from {0} with(nolock) where LogType", tableName);

                            LogType[] logTypes = _null<TRowCommand>.value.AcceptLogTypes;
                            if (logTypes.Length == 0) throw new Exception("程式結構錯誤, AcceptLogTypes 至少要指定一個");
                            if (logTypes.Length == 1) this.LogType = this.LogType ?? logTypes[0];
                            if (this.LogType.HasValue && Enum<BU.LogType>.IsDefined(this.LogType.Value))
                                sql.AppendFormat(" = {0}", (int)this.LogType.Value);
                            else
                            {
                                sql.Append(" in (");
                                for (int i = 0; i < logTypes.Length; i++)
                                {
                                    if (i > 0) sql.Append(',');
                                    sql.Append((int)logTypes[i]);
                                }
                                sql.Append(")");
                            }

                            int cnt = 1;
                            sql_where_CorpID(sql, ref cnt, "CorpID = {0}", this.CorpID);

                            if (_null<TUser>.value.UserType.In(BU.UserType.Agent, BU.UserType.Member))
                                sql_where(sql, ref cnt, "UserType = {0}", (int)_null<TUser>.value.UserType);

                            using (StringReader r1 = new StringReader(json_s))
                            using (api.json_reader r2 = new api.json_reader(r1))
                            {
                                while (r2.Read())
                                {
                                    if ((r2.Depth == 2) && (r2.TokenType == JsonToken.PropertyName))
                                    {
                                        string name = r2.Value as string;
                                        if (string.IsNullOrEmpty(name)) continue;
                                        if ((name == "LogType") || (name == "UserType") || (name == "CorpID")) continue;
                                        if (!schema.ContainsKey(name)) continue;
                                        string value = r2.ReadAsString();
                                        if (string.IsNullOrEmpty(value)) continue;
                                        this.filter(sql, ref cnt, name, value);
                                    }
                                }
                            }

                            jgrid.GridResponse<TRowData> data = new jgrid.GridResponse<TRowData>();
                            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
                            data.page_size = this.page_size;
                            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", this.rows_start, this.rows_end, this.GetOrderBy(this.IsHist.Value ? "FinishTime" : "CreateTime"), sql))
                                data.rows.Add(r.ToObject<TRowData>());
                            return data;
                        }
                    }
                }
            }
        }
    }
}
