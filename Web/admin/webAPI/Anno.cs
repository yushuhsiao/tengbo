using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AnnoRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public Locked? Locked;
        [DbImport, JsonProperty]
        public int? Sort;
        [DbImport, JsonProperty]
        public string Text;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;
    }
}
namespace web
{
    public abstract class AnnoRowCommand
    {
        [JsonProperty]
        public virtual int? ID { get; set; }
        [JsonProperty]
        public virtual int? CorpID { get; set; }
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public virtual Locked? Locked { get; set; }
        [JsonProperty]
        public virtual int? Sort { get; set; }
        [JsonProperty]
        public virtual string Text { get; set; }

        public AnnoRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                AnnoRow row = sqlcmd.GetRowEx<AnnoRow>(RowErrorCode.NotFound, "select * from Anno nolock where ID={0}", this.ID);
                sqltool s = new sqltool();
                s["N", "Name", "  ", row.Name, "  "] = text.ValidAsName * this.Name;
                s[" ", "CorpID", "", row.CorpID, ""] = this.CorpID;
                s[" ", "Locked", "", row.Locked, ""] = this.Locked;
                s[" ", "Sort", "  ", row.Sort, "  "] = this.Sort;
                s["N", "Text", "  ", row.Text, "  "] = text.ValidAsString * this.Text;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sql = s.BuildEx("update Anno set ", sqltool._FieldValue, @" where ID={ID} select * from Anno nolock where ID={ID}");
                return sqlcmd.ExecuteEx<AnnoRow>(sql);
            }
        }

        public AnnoRow insert(string json_s, params object[] args)
        {
            //AnnoRow row = sqlcmd.GetRow<AnnoRow>("select ID from Anno nolock where ID={0}", this.ID);
            //if (row != null) throw new RowException(RowErrorCode.RowAlreadyExist);
            sqltool s = new sqltool();
            s["* ", "CorpID", ""] = this.CorpID;
            s["*N", "Name", "  "] = text.ValidAsName * this.Name;
            s["  ", "Locked", ""] = this.Locked ?? BU.Locked.Locked;
            s["  ", "Sort", "  "] = this.Sort ?? 0;
            s[" N", "Text", "  "] = (text.ValidAsString * this.Text) ?? "";
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx("insert into Anno (", sqltool._Fields, ") values (", sqltool._Values, @")
if @@rowcount>0 select * from Anno nolock where ID=@@IDENTITY");
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<AnnoRow>(sqlstr);
        }
    }
}