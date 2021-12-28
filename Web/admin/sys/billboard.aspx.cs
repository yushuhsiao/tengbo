using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Tools.Protocol;
using web;

namespace BU
{
    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum BillboardRecordType
    {
        livecasino = 1, egame = 2,
    }
}

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BillboardRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        //public int? Place;
        //[DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public int? MemberID;
        [DbImport, JsonProperty]
        public string MemberACNT;
        [DbImport, JsonProperty]
        public decimal? Amount;
        [DbImport, JsonProperty]
        public Locked? Locked;
        [DbImport, JsonProperty]
        public BillboardRecordType? RecordType;
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
    public class BillboardRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        //[JsonProperty]
        //public int? Place;
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual int? MemberID { get; set; }
        [JsonProperty]
        public virtual string MemberACNT { get; set; }
        [JsonProperty]
        public virtual decimal? Amount { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual BillboardRecordType? RecordType { get; set; }
        [JsonProperty]
        public virtual DateTime? CreateTime { get; set; }
        [JsonProperty]
        public virtual int? CreateUser { get; set; }
        [JsonProperty]
        public virtual DateTime? ModifyTime { get; set; }
        [JsonProperty]
        public virtual int? ModifyUser { get; set; }

        public BillboardRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                BillboardRow row = sqlcmd.GetRowEx<BillboardRow>(RowErrorCode.NotFound, "select * from Billboard nolock where ID={0}", this.ID);
                sqltool s = new sqltool();
                s[" ", "CorpID", "    ", row.CorpID, "    "] = this.CorpID;
                //s[" ", "Place", "     ", row.Place, "     "] = command.Place;
                s[" ", "MemberID", "  ", row.MemberID, "  "] = this.MemberID;
                s[" ", "MemberACNT", "", row.MemberACNT, ""] = text.ValidAsACNT * this.MemberACNT;
                s[" ", "Amount", "    ", row.Amount, "    "] = this.Amount;
                s[" ", "Locked", "    ", row.Locked, "    "] = this.Locked;
                s[" ", "RecordType", "", row.RecordType, ""] = this.RecordType;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.BuildEx("update Billboard set ", sqltool._FieldValue, @" where ID={ID} select * from Billboard nolock where ID={ID}");
                return sqlcmd.ExecuteEx<BillboardRow>(sqlstr);
            }
        }

        public BillboardRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["*", "CorpID", "    "] = this.CorpID;
            //s["*", "Place", "     "] = command.Place;
            s[" ", "MemberID", "  "] = this.MemberID;
            s[" ", "MemberACNT", ""] = text.ValidAsACNT * this.MemberACNT;
            s[" ", "Amount", "    "] = this.Amount ?? 0;
            s[" ", "Locked", "    "] = this.Locked ?? BU.Locked.Active;
            s[" ", "RecordType", ""] = this.RecordType ?? 0;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx("insert into Billboard (", sqltool._Fields, ") values (", sqltool._Values, @")
if @@rowcount>0 select * from Billboard nolock where ID=@@IDENTITY");
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<BillboardRow>(sqlstr);
        }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BillboardSelect : jgrid.GridRequest<BillboardSelect>
{
    protected override string init_defaultkey() { return "ModifyTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"ID", "ID"},
            {"CorpID", "CorpID"},
            {"Locked", "Locked"},
            {"RecordType", "RecordType"},
            {"Place", "Place"},
            {"MemberACNT", "MemberACNT"},
            {"Amount", "Amount"},
            {"CreateTime", "CreateTime"},
            {"CreateUser", "CreateUser"},
            {"ModifyUser", "ModifyUser"},
            {"ModifyTime", "ModifyTime"},
        };
    }

    [ObjectInvoke, Permissions(Permissions.Code.m_billboard, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<BillboardRow> execute(BillboardSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<BillboardRow> data = new jgrid.GridResponse<BillboardRow>();

            StringBuilder sql = new StringBuilder(@"select * from Billboard with(nolock)");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"{0} order by {1}", sql, command.GetOrderBy()))
                data.rows.Add(r.ToObject<BillboardRow>());
            return data;
        }
        //jgrid.GridResponse<BillboardRow> data = new jgrid.GridResponse<BillboardRow>();
        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        //{
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Billboard nolock order by CorpID,Amount desc"))
        //        data.rows.Add(r.ToObject<BillboardRow>());
        //    //data["corps"] = CorpRow.Cache.GetInstance(null, sqlcmd).names;
        //    return data;
        //}
    }

    [ObjectInvoke, Permissions(Permissions.Code.m_billboard, Permissions.Flag.Write)]
    static object execute(BillboardUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.m_billboard, Permissions.Flag.Write)]
    static object execute(BillboardInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BillboardUpdate : BillboardRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class BillboardInsert : BillboardRowCommand, IRowCommand { }
