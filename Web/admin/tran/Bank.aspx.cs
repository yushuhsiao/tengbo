using BU;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BankSelect : jgrid.GridRequest
{
    [ObjectInvoke, Permissions(Permissions.Code.tran_banklist, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<BankRow> execute(BankSelect command, string json_s, params object[] args)
    {
        jgrid.GridResponse<BankRow> data = new jgrid.GridResponse<BankRow>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Bank nolock order by CreateTime desc"))
                data.rows.Add(r.ToObject<BankRow>());
            return data;
        }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BankUpdate : BankRowCommand, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.tran_banklist, Permissions.Flag.Write)]
    static object execute(BankUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BankInsert : BankRowCommand, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.tran_banklist, Permissions.Flag.Write)]
    static object execute(BankInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
}
