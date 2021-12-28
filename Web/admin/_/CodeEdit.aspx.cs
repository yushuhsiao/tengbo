using BU;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;
using web.data;
using webAPI;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class CodeSelect : jgrid.GridRequest
{
    [ObjectInvoke, api.Async, api.Permission(AllowGuest = false)]
    static jgrid.GridResponse<CodeRow> execute(CodeSelect command, string json_s, params object[] args)
    {
        jgrid.GridResponse<CodeRow> data = new jgrid.GridResponse<CodeRow>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from p_Code nolock"))
                data.rows.Add(r.ToObject<CodeRow>());
            return data;
        }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class CodeRowCommand
{
    [JsonProperty]
    public virtual int? ID { get; set; }
    [JsonProperty]
    public virtual string Code { get; set; }
    [JsonProperty]
    public virtual string Description { get; set; }

    public CodeRow update(string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            CodeRow row = sqlcmd.GetRowEx<CodeRow>(RowErrorCode.NotFound, "select * from p_Code nolock where ID={0}", this.ID);
            sqltool s = new sqltool();
            s[" ", "Code", "        ", row.Code, "          "] = text.ValidAsString * this.Code;
            s[" ", "Description", " ", row.Description, "   "] = text.ValidAsString * this.Description;
            if (s.fields.Count == 0) return row;
            s.Values["ID"] = this.ID;
            return sqlcmd.ExecuteEx<CodeRow>(s.BuildEx("update p_Code set ", sqltool._FieldValue, "where ID={ID} select * from p_Code nolock where ID={ID}"));
        }
    }

    public CodeRow insert(string json_s, params object[] args)
    {
        sqltool s = new sqltool();
        s[" ", "Code", "        "] = text.ValidAsString * this.Code;
        s[" ", "Description", " "] = text.ValidAsString * (this.Description ?? this.Code);
        s.TestFieldNeeds();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            return sqlcmd.ExecuteEx<CodeRow>(s.BuildEx("insert into p_Code (", sqltool._Fields, ") values (", sqltool._Values, ") select * from p_Code nolock where ID=@@IDENTITY"));
    }

    public CodeRow delete(string json_s, params object[] args)
    {
        if (this.ID.HasValue)
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<CodeRow>("select * from p_Code nolock where ID={0} delete p_Code where ID={0}", this.ID);
        return null;
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class CodeUpdate : CodeRowCommand
{
    [ObjectInvoke, api.Async, api.Permission(AllowGuest = false)]
    static object execute(CodeUpdate command, string json_s, params object[] args)
    {
        try { return text.RowUpdate(command.update(json_s, args)); }
        catch (RowException ex) { return ex; }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class CodeInsert : CodeRowCommand
{
    [ObjectInvoke, api.Async, api.Permission(AllowGuest = false)]
    static object execute(CodeInsert command, string json_s, params object[] args)
    {
        try { return text.RowUpdate(command.insert(json_s, args)); }
        catch (RowException ex) { return ex; }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class CodeDelete : CodeRowCommand
{
    [ObjectInvoke, api.Async, api.Permission(AllowGuest = false)]
    static object execute(CodeDelete command, string json_s, params object[] args)
    {
        try { return text.RowUpdate(command.delete(json_s, args)); }
        catch (RowException ex) { return ex; }
    }
}