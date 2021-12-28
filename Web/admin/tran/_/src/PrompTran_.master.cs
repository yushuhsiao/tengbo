using BU;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class PromoTranSelect : jgrid.GridRequest
{
    [JsonProperty]
    public bool? IsHist;

    [ObjectInvoke, api.Async]
    [Permissions(Permissions.Code.tran_promo, Permissions.Flag.Read | Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_promo, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<PromoTranRow> execute(PromoTranSelect command, string json_s, params object[] args)
    {
        jgrid.GridResponse<PromoTranRow> data = new jgrid.GridResponse<PromoTranRow>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            string sqlstr;
            if (command.IsHist == true)
            {
                data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from PromoTran2 nolock"), command.page_size);
                sqlstr = string.Format(@"select * from (select row_number() over (order by FinishTime desc) as rowid, * from PromoTran2) a where a.rowid>{0} and a.rowid<={1} order by FinishTime desc", command.rows_start, command.rows_end);
            }
            else
                sqlstr = "select * from PromoTran1 nolock order by CreateTime desc";
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sqlstr))
                data.rows.Add(r.ToObject<PromoTranRow>());
            return data;
        }
    }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_promo, Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_promo, Permissions.Flag.Write)]
    static object execute(PromoTranUpdate command, string json_s, params object[] args) { return command.Update(json_s, args); }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_promo, Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_promo, Permissions.Flag.Write)]
    static object execute(PromoTranInsert command, string json_s, params object[] args) { return command.Insert(json_s, args); }

    [ObjectInvoke, api.Async, Permissions(Permissions.Code.tran_promo, Permissions.Flag.Write), Permissions(Permissions.Code.tranhist_promo, Permissions.Flag.Write)]
    static object execute(PromoTranDelete command, string json_s, params object[] args) { return command.Delete(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class PromoTranUpdate : PromoTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class PromoTranInsert : PromoTranRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class PromoTranDelete : PromoTranRowCommand, IRowCommand { }