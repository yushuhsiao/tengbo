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
using BU.data;
using Newtonsoft.Json;
using web.protocol;

namespace BU.data
{
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
namespace web
{
    #region protocol
    namespace protocol
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class IPSSelect : jgrid.GridRequest
        {
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class IPSUpdate
        {
            [JsonProperty]
            public int? ID;
            [JsonProperty]
            public string IPSName;
            [JsonProperty]
            public string Locked;
            [JsonProperty]
            public string MerCode;
            [JsonProperty]
            public string SubmitUrl;
            [JsonProperty]
            public string MerchantKey;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        class IPSInsert : IPSUpdate { }
    }
    #endregion
    partial class _ExecuteCommand
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

        [ObjectInvoke, api.Async]
        static jgrid.RowResponse insert(IPSInsert command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                sqltool s = new sqltool();
                s["N", "IPSName", "   "] = text.ValidAsName * command.IPSName;
                s[" ", "Locked", "    "] = text.ValidAsLocked * command.Locked;
                s[" ", "MerCode", "  "] = text.ValidAsString * command.MerCode;
                s[" ", "SubmitUrl", ""] = text.ValidAsString * command.SubmitUrl;
                s[" ", "MerchantKey", "    "] = text.ValidAsString * command.MerchantKey;
                s.SetUser(sqltool.CreateUser);
                if (s.needs != null)
                    return jgrid.RowResponse.FieldNeeds(s.needs);
                string sqlstr = s.SqlExport(s.Build("exec ipspay_insert ", sqltool._AtFieldValue));
                //return sqltool.sql_execute2<MemberRow>(sqlcmd, s.SqlExport(sql));
                return sqlcmd.Execute<IPSPayRow>(sqlstr);
            }
        }

        [ObjectInvoke, api.Async]
        static jgrid.RowResponse update(IPSUpdate command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                IPSPayRow row;
                if (!sqlcmd.GetSingleRow(out row, "exec ips_select_single @ID={0}", command.ID))
                    return jgrid.RowResponse.UpdateMissing();
                sqltool s = new sqltool();
                s["N", "IPSName", "      ", row.IPSName, "            "] = text.ValidAsName * command.IPSName;
                s[" ", "Locked", "       ", row.Locked, "        "] = text.ValidAsLocked * command.Locked;
                s[" ", "MerCode", "   ", row.MerCode, ""] = text.ValidAsString * command.MerCode;
                s[" ", "SubmitUrl", "    ", row.SubmitUrl, "          "] = text.ValidAsString * command.SubmitUrl;
                s[" ", "MerchantKey", "", row.MerchantKey, "      "] = text.ValidAsString * command.MerchantKey;
                if (s.fields.Count == 0)
                    return jgrid.RowResponse.UpdateIgnore(row);
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.SqlExport(s.Build("update IPSPay set ", sqltool._FieldValue, " where ID={ID} exec ips_select_single @ID={ID}"));
                return sqlcmd.Execute<IPSPayRow>(sqlstr);
            }
        }
    }

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class IPSInsert : IPSUpdate
    //{
    //    [ObjectInvoke, api.Async]
    //    static object execute(IPSInsert command, string json_s, params object[] args)
    //    {
    //        HttpContext context = HttpContext.Current;
    //        User user = context.User as User;
    //        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
    //        {
    //            sqltool s = new sqltool();
    //            s["N", "IPSName", "   "] = text.ValidAsName * command.IPSName;
    //            s[" ", "Locked", "    "] = text.ValidAsLocked * command.Locked;
    //            s[" ", "MerCode", "  "] = text.ValidAsString * command.MerCode;
    //            s[" ", "SubmitUrl", ""] = text.ValidAsString * command.SubmitUrl;
    //            s[" ", "MerchantKey", "    "] = text.ValidAsString * command.MerchantKey;
    //            s.SetUser(sqltool.CreateUser);
    //            if (s.needs != null)
    //                return new jgrid.RowResponse().FieldNeeds(s.needs);
    //            string sql = s.Build("exec ipspay_insert ", sqltool._AtFieldValue);
    //            //return sqltool.sql_execute2<MemberRow>(sqlcmd, s.SqlExport(sql));
    //            jgrid.RowResponse res = sqltool.sql_execute<IPSPayRow>(sqlcmd, s.SqlExport(sql));
    //            return res;
    //        }
    //    }
    //}

    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //class IPSUpdate : jgrid.RowRequest
    //{
    //    [JsonProperty]
    //    public int? ID;
    //    [JsonProperty]
    //    public string IPSName;
    //    [JsonProperty]
    //    public string Locked;
    //    [JsonProperty]
    //    public string MerCode;
    //    [JsonProperty]
    //    public string SubmitUrl;
    //    [JsonProperty]
    //    public string MerchantKey;


    //    [ObjectInvoke, api.Async]
    //    static object execute(IPSUpdate command, string json_s, params object[] args)
    //    {
    //        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
    //        {
    //            sqltool s = new sqltool();
    //            IPSPayRow row = sqltool.sql_getrow<IPSPayRow>(sqlcmd, "exec ips_select_single @ID={0}", command.ID);
    //            if (row == null)
    //                return new jgrid.RowResponse().UpdateMissing();
    //            s["N", "IPSName", "      ", row.IPSName, "            "] = text.ValidAsName * command.IPSName;
    //            s[" ", "Locked", "       ", row.Locked, "        "] = text.ValidAsLocked * command.Locked;
    //            s[" ", "MerCode", "   ", row.MerCode, ""] = text.ValidAsString * command.MerCode;
    //            s[" ", "SubmitUrl", "    ", row.SubmitUrl, "          "] = text.ValidAsString * command.SubmitUrl;
    //            s[" ", "MerchantKey", "", row.MerchantKey, "      "] = text.ValidAsString * command.MerchantKey;


    //            if (s.fields.Count == 0)
    //                return new jgrid.RowResponse().Ignore(row);
    //            s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
    //            s.Values["ID"] = row.ID;
    //            string sql = s.Build("update IPSPay set ", sqltool._FieldValue, " where ID={ID} exec ips_select_single @ID={ID}");
    //            return sqltool.sql_execute<MemberRow>(sqlcmd, s.SqlExport(sql));
    //        }
    //    }
    //}


}