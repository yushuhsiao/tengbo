using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using BU;
using Newtonsoft.Json;

namespace web.data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class BillboardSelect : jgrid.GridRequest
    {
        [ObjectInvoke, api.Async]
        static object execute(BillboardSelect command, string json_s, params object[] args)
        {
            jgrid.GridResponse<BillboardRow> data = new jgrid.GridResponse<BillboardRow>();
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Billboard nolock order by CorpID,Place"))
                    data.rows.Add(r.ToObject<BillboardRow>());
                data["corps"] = CorpList.GetInstance(null, sqlcmd).names;
                return data;
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class BillboardInsert : jgrid.RowRequest
    {
        [ObjectInvoke, api.Async]
        static object execute(BillboardInsert command, string json_s, params object[] args)
        {
            HttpContext context = HttpContext.Current;
            User user = context.User as User;

            sql_tool d = new sql_tool(json_s);
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                d.isUpdate = false;
                d.Int32(fields.Place, null, 0);
                d.Int32("CorpID", null, 0);
                d.Int32(fields.MemberID, null, null);
                d.ACNT(fields.MemberACNT, null, null);
                d.Decimal(fields.Amount, null, 0);
                d.Locked(fields.Disabled, null, 0);
                d.Int32(fields.RecordType, null, 0);
                d.dst[fields.CreateUser] = d.dst["ModifyUser"] = user.ID;
                //d.NullField(fields.MemberID, fields.MemberACNT);
                foreach (object failed in d.check_args(command.rowid))
                    return failed;
                string sql = d.sql_build("insert into Billboard (", sql_tool._Fields, ") values (", sql_tool.Values, @")
select * from Billboard nolock where ID=IDENT_CURRENT('dbo.Billboard')");
                return d.sql_execute<BillboardRow>(sqlcmd, command.rowid, sql.SqlExport(null, d.dst));
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class BillboardUpdate : jgrid.RowRequest
    {
        [ObjectInvoke, api.Async]
        static object execute(BillboardUpdate command, string json_s, params object[] args)
        {
            HttpContext context = HttpContext.Current;
            User user = context.User as User;

            sql_tool d = new sql_tool(json_s);
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                BillboardRow row = d.sql_GetRow<BillboardRow>(sqlcmd, "select * from Billboard nolock where ID={0}", "ID");

                d.isUpdate = true;
                d.Int32(fields.Place, null, row.Place);
                d.Int32("CorpID", null, row.CorpID);
                d.Int32(fields.MemberID, null, row.MemberID);
                d.ACNT(fields.MemberACNT, null, row.MemberACNT);
                d.Decimal(fields.Amount, null, row.Amount);
                d.Locked(fields.Disabled, null, row.Disabled);
                d.Int32(fields.RecordType, null, row.RecordType);
                d.dst["ModifyUser"] = user.ID;
                if (d.dst.Count == 0)
                    return jgrid.RowResponse.Ignore(command.rowid, row);
                else
                {
                    d.dst["ModifyTime"] = (StringEx.sql_str)"getdate()";
                    d.dst["ModifyUser"] = user.ID;
                    string sql = d.sql_build("update Billboard set ", sql_tool._FieldValue, @" where ID={ID} select * from Billboard nolock where ID={ID}");
                    d.dst["ID"] = row.ID;
                    return d.sql_execute<BillboardRow>(sqlcmd, command.rowid, sql.SqlExport(null, d.dst));
                }
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class BillboardRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public int? Place;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public int? MemberID;
        [DbImport, JsonProperty]
        public string MemberACNT;
        [DbImport, JsonProperty]
        public decimal? Amount;
        [DbImport, JsonProperty]
        public byte? Disabled;
        [DbImport, JsonProperty]
        public int? RecordType;
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