using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BankRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public byte? Locked;
        [DbImport, JsonProperty]
        public string WebATM;
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
    public class BankRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual string WebATM { get; set; }

        public BankRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                BankRow row = sqlcmd.GetRowEx<BankRow>(RowErrorCode.NotFound, "select * from Bank nolock where ID={0}", this.ID);
                sqltool s = new sqltool();
                s["N", "Name", "  ", row.Name, "  "] = text.ValidAsName * this.Name;
                s[" ", "Locked", "", row.Locked, ""] = this.Locked;
                s["N", "WebATM", "", row.WebATM, ""] = this.WebATM;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sqlstr = s.BuildEx("update Bank set ", sqltool._FieldValue, " where ID={ID} select * from Bank nolock where ID={ID}");
                return sqlcmd.ExecuteEx<BankRow>(sqlstr);
            }
        }

        public BankRow insert(string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["*N", "Name", "  "] = text.ValidAsName * this.Name;
            s["  ", "Locked", ""] = this.Locked ?? BU.Locked.Active;
            s[" N", "WebATM", ""] = text.ValidAsString * this.WebATM ?? "";
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx("insert into Bank (", sqltool._Fields, ") values (", sqltool._Values, @")
if @@rowcount>0 select * from Bank nolock where ID=@@IDENTITY");
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<BankRow>(sqlstr);
        }
    }
}
