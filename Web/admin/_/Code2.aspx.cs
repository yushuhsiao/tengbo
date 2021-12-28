using BU;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;
using web.data;
using webAPI;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Code2Select : jgrid.GridRequest
{
    [ObjectInvoke, api.Async, api.Permission(AllowGuest = false)]
    static jgrid.GridResponse<Code2Row> execute(Code2Select command, string json_s, params object[] args)
    {
        jgrid.GridResponse<Code2Row> data = new jgrid.GridResponse<Code2Row>();
        //jgrid.GridResponse<object> data2 = new jgrid.GridResponse<object>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Code1 nolock"))
            {
                //data2.rows.Add(new
                //{
                //    Code = r.GetInt32N("Code"),
                //    Parent = r.GetInt32N("Parent"),
                //    resid = r.GetStringN("resid"),
                //    Flag = r.GetInt32N("Flag"),
                //    Sort = r.GetInt32N("Sort"),
                //    Path = r.GetStringN("Path"),
                //    level = 0,
                //    isLeaf = false,
                //    expanded = true,
                //    loaded = true,
                //});
                data.rows.Add(r.ToObject<Code2Row>());
            }
            return data;
        }
    }
}

class Code2RowCommand
{
    [JsonProperty]
    public virtual int? Code { get; set; }
    [JsonProperty]
    public virtual int? newCode { get; set; }
    [JsonProperty]
    public virtual int? Parent { get; set; }
    [JsonProperty]
    public virtual string resid { get; set; }
    [JsonProperty]
    public virtual int? Flag { get; set; }
    [JsonProperty]
    public virtual int? Sort { get; set; }
    [JsonProperty]
    public virtual string Path { get; set; }

    public Code2Row update(string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            Code2Row row = sqlcmd.GetRowEx<Code2Row>(RowErrorCode.NotFound, "select * from Code1 nolock where Code={0}", this.Code);
            sqltool s = new sqltool();
            s[" ", "Code", "  ", row.Code, "  "] = this.newCode;
            s[" ", "Parent", "", row.Parent, ""] = this.Parent;
            s[" ", "resid", " ", row.resid, " "] = text.ValidAsString * this.resid;
            s[" ", "Flag", "  ", row.Flag, "  "] = this.Flag;
            s[" ", "Sort", "  ", row.Sort, "  "] = this.Sort;
            s[" ", "Path", "  ", row.Path, "  "] = text.ValidAsString * this.Path;
            if (s.fields.Count == 0) return row;
            string sqlstr;
            if (s.Values.ContainsKey("Code"))
            {
                sqlstr = s.Build(@"update Code1 set Parent={Code} where Parent={oldCode}
delete Code2 where Code={Code} update Code2 set Code={Code} where Code={oldCode}
update Code1 set ", sqltool._FieldValue, @" where Code={oldCode}
select * from Code1 nolock where Code={Code}");
                s.Values["oldCode"] = row.Code;
            }
            else
            {
                sqlstr = s.Build("update Code1 set ", sqltool._FieldValue, " where Code={Code} select * from Code1 nolock where Code={Code}");
                s.Values["Code"] = row.Code;
            }
            sqlstr = s.SqlExport(sqlstr);
            return sqlcmd.ExecuteEx<Code2Row>(sqlstr);
        }
    }

    public Code2Row insert(string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            Code2Row row = sqlcmd.GetRow<Code2Row>("select Code from Code1 nolock where Code={0}", this.newCode);
            if (row != null) throw new RowException(RowErrorCode.AlreadyExist);
            //if (this.newCode.HasValue && (sqlcmd.ExecuteScalar<int>("select count(Code) from Code1 nolock where Code={0}", this.newCode) > 0))
            //    return jgrid.RowResponse.AlreadyExist(this.newCode);
            sqltool s = new sqltool();
            s["*", "Code", "  "] = this.newCode;
            s[" ", "Parent", ""] = this.Parent;
            s[" ", "resid", " "] = text.ValidAsString * this.resid ?? "";
            s[" ", "Flag", "  "] = this.Flag ?? 0;
            s[" ", "Sort", "  "] = this.Sort;
            s[" ", "Path", "  "] = text.ValidAsString * this.Path;
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx("insert into Code1 (", sqltool._Fields, ") values (", sqltool._Values, @")
select * from Code1 nolock where Code={Code}");
            return sqlcmd.ExecuteEx<Code2Row>(sqlstr);
        }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Code2Update : Code2RowCommand
{
    [ObjectInvoke, api.Async, api.Permission(AllowGuest = false)]
    static object execute(Code2Update command, string json_s, params object[] args)
    {
        try { return text.RowUpdate(command.update(json_s, args)); }
        catch (RowException ex) { return ex; }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Code2Insert : Code2RowCommand
{
    [ObjectInvoke, api.Async, api.Permission(AllowGuest = false)]
    static object execute(Code2Insert command, string json_s, params object[] args)
    {
        try { return text.RowUpdate(command.insert(json_s, args)); }
        catch (RowException ex) { return ex; }
    }
}
