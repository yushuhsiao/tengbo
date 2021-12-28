using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Tools;

namespace web
{
    public static class Groups<TUser, TRowData, TRowCommand>
        where TUser : UserRow, new()
        where TRowData : Groups<TUser, TRowData, TRowCommand>.RowData, new()
        where TRowCommand : Groups<TUser, TRowData, TRowCommand>.RowCommand, new()
    {
        public abstract class RowData
        {
            [DbImport, JsonProperty]
            public Guid? ID;
            [DbImport, JsonProperty]
            public int? CorpID;
            [DbImport, JsonProperty]
            public string Name;
            [DbImport, JsonProperty]
            public Locked? Locked;
            [DbImport, JsonProperty]
            public int? Sort;
            [DbImport, JsonProperty]
            public DateTime? CreateTime;
            [DbImport, JsonProperty]
            public _SystemUser CreateUser;
            [DbImport, JsonProperty]
            public DateTime? ModifyTime;
            [DbImport, JsonProperty]
            public _SystemUser ModifyUser;

            public class Cache : WebTools.ListCache<Cache, TRowData>
            {
                [SqlSetting("Cache", "GroupList"), DefaultValue(30000)]
                public override double LifeTime
                {
                    get { return base.LifeTime; }
                    set { base.LifeTime = value; }
                }

                public override void Update(SqlCmd sqlcmd, string key, params object[] args)
                {
                    List<TRowData> rows = new List<TRowData>();
                    Dictionary<int, Dictionary<Guid, TRowData>> value0 = new Dictionary<int, Dictionary<Guid, TRowData>>();
                    Dictionary<int, Dictionary<Guid, string>> value1 = new Dictionary<int, Dictionary<Guid, string>>();
                    Dictionary<Guid, string> value2 = new Dictionary<Guid, string>();
                    using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                    {
                        foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from {0} nolock order by ID asc", _null<TUser>.value.GroupTableName))
                        {
                            TRowData row = r.ToObject<TRowData>();
                            rows.Add(row);
                            if (!value0.ContainsKey(row.CorpID.Value))
                            {
                                value0[row.CorpID.Value] = new Dictionary<Guid, TRowData>();
                                value1[row.CorpID.Value] = new Dictionary<Guid, string>();
                            }
                            value0[row.CorpID.Value][row.ID.Value] = row;
                            value1[row.CorpID.Value][row.ID.Value] = row.Name;
                            value2[row.ID.Value] = row.Name;
                        }
                    }
                    base.Value = rows;
                    this.Value0 = value0;
                    this.Value1 = value1;
                    this.Value2 = value2;
                }

                Dictionary<int, Dictionary<Guid, TRowData>> value0 = new Dictionary<int, Dictionary<Guid, TRowData>>();
                public Dictionary<int, Dictionary<Guid, TRowData>> Value0
                {
                    get { return Interlocked.CompareExchange(ref this.value0, null, null); }
                    set { Interlocked.Exchange(ref this.value0, value); }
                }

                Dictionary<int, Dictionary<Guid, string>> value1 = new Dictionary<int, Dictionary<Guid, string>>();
                public Dictionary<int, Dictionary<Guid, string>> Value1
                {
                    get { return Interlocked.CompareExchange(ref this.value1, null, null); }
                    set { Interlocked.Exchange(ref this.value1, value); }
                }

                Dictionary<Guid, string> value2 = new Dictionary<Guid, string>();
                public Dictionary<Guid, string> Value2
                {
                    get { return Interlocked.CompareExchange(ref this.value2, null, null); }
                    set { Interlocked.Exchange(ref this.value2, value); }
                }

                public TRowData GetRow(int? corpID, Guid? groupID)
                {
                    foreach (TRowData row in this.Value)
                        if ((row.CorpID == corpID) && (row.ID == groupID))
                            return row;
                    return null;
                }
            }
        }

        public class RowCommand
        {
            [JsonProperty]
            public bool? op_Insert { get; set; }
            [JsonProperty]
            public Guid? ID { get; set; }
            [JsonProperty]
            public int? CorpID { get; set; }
            [JsonProperty]
            public string Name { get; set; }
            [JsonProperty]
            public Locked? Locked { get; set; }
            [JsonProperty]
            public int? Sort { get; set; }

            StringEx.sql_str TableName
            {
                get { return _null<TUser>.value.GroupTableName; }
            }

            protected virtual TRowData execute(TRowCommand command, string json_s, params object[] args)
            {
                if (command.op_Insert == true)
                    return command.insert(null, null, json_s, args);
                else
                    return command.update(json_s, args);
            }

            protected virtual void update_fill(TRowData row, sqltool s) { }
            TRowData update(string json_s, params object[] args)
            {
                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                {
                    TRowData row = sqlcmd.GetRowEx<TRowData>(RowErrorCode.NotFound, "select * from {TableName} nolock where ID={ID}".SqlExport(this));
                    sqltool s = new sqltool();
                    s[" ", "CorpID", " ", row.CorpID, " "] = this.CorpID;
                    s[" ", "Sort", "   ", row.Sort, "   "] = this.Sort;
                    s["N", "Name", "   ", row.Name, "   "] = this.Name * text.ValidAsString;
                    s[" ", "Locked", " ", row.Locked, " "] = this.Locked ?? BU.Locked.Active;
                    this.update_fill(row, s);
                    if (s.fields.Count == 0) return row;
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    s.values["ID"] = this.ID;
                    s.values["TableName"] = this.TableName;
                    string sqlstr = s.BuildEx2("update {TableName} set ", sqltool._FieldValue, " where ID={ID} select * from {TableName} nolock where ID={ID}");
                    return sqlcmd.ExecuteEx<TRowData>(sqlstr);
                }
            }

            protected virtual void insert_fill(sqltool s) { }
            internal TRowData insert(SqlCmd sqlcmd, CorpRow corp, string json_s, params object[] args)
            {
                sqltool s = new sqltool();
                s["* ", "CorpID", "      "] = this.CorpID;
                s["* ", "Sort", "        "] = this.Sort;
                s["*N", "Name", "        "] = this.Name * text.ValidAsString;
                s["  ", "Locked", "      "] = this.Locked ?? BU.Locked.Active;
                this.insert_fill(s);
                s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                s.TestFieldNeeds();
                s.values["TableName"] = this.TableName;
                string sqlstr = s.BuildEx2(@"declare @id uniqueidentifier set @id=newid()
insert into {TableName} (ID,", sqltool._Fields, @")
select @id,", sqltool._Values, @" from Corp nolock where ID={CorpID}
select * from {TableName} nolock where ID=@id");
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    if (corp != null)
                        return sqlcmd.ToObject<TRowData>(sqlstr);
                    else
                        return sqlcmd.ExecuteEx<TRowData>(sqlstr);
                }
            }
        }
    }
}