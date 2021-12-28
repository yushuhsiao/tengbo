using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BU;
using Newtonsoft.Json;

namespace web.data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class IPSSelect : jgrid.GridRequest
    {
        [ObjectInvoke, api.Async]
        static object execute(IPSSelect command, string json_s, params object[] args)
        {
            jgrid.GridResponse<IPSPayRow> data = new jgrid.GridResponse<IPSPayRow>();
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from IPSPay"))
                    data.rows.Add(r.ToObject<IPSPayRow>());
                return data;
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class IPSInsert : jgrid.RowRequest
    {
        [ObjectInvoke, api.Async]
        static object execute(MemberInsert command, string json_s, params object[] args)
        {
            HttpContext context = HttpContext.Current;
            User user = context.User as User;

            sql_tool d = new sql_tool(json_s);
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                d.isUpdate = false;
                d.String("IPSName", null, null);
                d.Locked("Locked", null, null);
                d.String("MerCode", null, null);
                d.String("SubmitUrl", null, null);
                d.String("MerchantKey", null, null);
                d.dst["ModifyUser"] = user.ID;
                string sql = d.sql_build("exec ipspay_insert ", sql_tool._AtFieldValue);
                return d.sql_execute<IPSPayRow>(sqlcmd, command.rowid, sql.SqlExport(null, d.dst));
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class IPSUpdate : jgrid.RowRequest
    {
        [ObjectInvoke, api.Async]
        static object execute(MemberUpdate command, string json_s, params object[] args)
        {
            HttpContext context = HttpContext.Current;
            User user = context.User as User;

            sql_tool d = new sql_tool(json_s);
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                IPSPayRow row = d.sql_GetRow<IPSPayRow>(sqlcmd, "exec ips_select_single @ID={0}", "ID");
                d.isUpdate = true;
                d.String("IPSName", null, row.IPSName);
                d.Locked("Locked", null, row.Locked);
                d.String("MerCode", null, row.MerCode);
                d.String("SubmitUrl", null, row.SubmitUrl);
                d.String("MerchantKey", null, row.MerchantKey);
                if (d.dst.Count == 0)
                    return jgrid.RowResponse.Ignore(command.rowid, row);
                else
                {
                    d.dst["ModifyTime"] = (StringEx.sql_str)"getdate()";
                    d.dst["ModifyUser"] = user.ID;
                    string sql = d.sql_build("update IPSPay set ", sql_tool._FieldValue, " where ID={ID} exec ips_select_single @ID={ID}");
                    d.dst["ID"] = row.ID;
                    return d.sql_execute<MemberRow>(sqlcmd, command.rowid, sql.SqlExport(null, d.dst));
                }
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class IPSPayRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public string IPSName;
        [DbImport, JsonProperty]
        public byte? Locked;
        [DbImport, JsonProperty]
        public string MerCode;
        [DbImport, JsonProperty]
        public string SubmitUrl;
        [DbImport, JsonProperty]
        public string MerchantKey;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public int? CreateUser;
       
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public int? ModifyUser;
    }
}