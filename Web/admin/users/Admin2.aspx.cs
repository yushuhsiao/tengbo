using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml;
using web;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdminAuthRow
    {
        [DbImport, JsonProperty]
        public int AdminID;
        [DbImport, JsonProperty]
        public string header;
        [DbImport, JsonProperty]
        public string idstr;
        [DbImport]
        public string rsakey;
        [JsonProperty("rsakey")]
        string _rsakey
        {
            get
            {
                if (rsakey == null) return null;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(this.rsakey);
                for (int i = xml.DocumentElement.ChildNodes.Count - 1; i >= 0; i--)
                {
                    if (xml.DocumentElement.ChildNodes[i].Name == "Modulus") continue;
                    if (xml.DocumentElement.ChildNodes[i].Name == "Exponent") continue;
                    xml.DocumentElement.RemoveChild(xml.DocumentElement.ChildNodes[i]);
                }
                return xml.OuterXml;
                //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //rsa.FromXmlString(this.rsakey);
                //return rsa.ToXmlString(false);
            }
        }
        [DbImport, JsonProperty]
        public Locked Locked;
        [DbImport, JsonProperty]
        public DateTime? ExpireTime;
        [DbImport, JsonProperty]
        public DateTime? CreateTime;
        [DbImport, JsonProperty]
        public _SystemUser CreateUser;
        [DbImport, JsonProperty]
        public DateTime? ModifyTime;
        [DbImport, JsonProperty]
        public _SystemUser ModifyUser;

        public Admin Admin { get; private set; }

        public class Cache : WebTools.ListCache<Cache, AdminAuthRow>
        {
            [SqlSetting("Cache", "AdminAuth"), DefaultValue(30 * 60 * 1000)]
            public override double LifeTime
            {
                get { return base.LifeTime; }
                set { base.LifeTime = value; }
            }
            public override void Update(SqlCmd sqlcmd, string key, params object[] args)
            {
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                {
                    List<AdminAuthRow> rows = sqlcmd.ToObjectList<AdminAuthRow>("select * from AdminAuth nolock where Locked=0 and ExpireTime>getdate()");
                    foreach (AdminAuthRow row in rows)
                        row.Admin = sqlcmd.ToObject<Admin>("select * from Admin nolock where ID={0}", row.AdminID);
                    base.Rows = rows;
                }
            }
        }

        public static AdminAuthRow GetRow(SqlCmd sqlcmd, int? adminID)
        {
            if (adminID.HasValue)
                using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, sqlcmd))
                    return sqlcmd.ToObject<AdminAuthRow>("select * from AdminAuth nolock where AdminID={0}", adminID);
            return null;
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AdminAuthRowCommand : IRowCommand
    {
        [JsonProperty]
        public int? AdminID;
        [JsonProperty]
        public string header;
        [JsonProperty]
        public string idstr;
        [JsonProperty]
        public int? keysize;
        [JsonProperty]
        public Locked? Locked;

        public AdminAuthRow update(string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                AdminAuthRow row = AdminAuthRow.GetRow(sqlcmd, this.AdminID);
                string sqlstr;
                if (row == null)
                {
                    sqltool s = new sqltool();
                    s["*", "AdminID", "   "] = this.AdminID;
                    s["*", "header", "    "] = this.header * text.ValidAsString;
                    s["*", "idstr", "     "] = this.idstr * text.ValidAsString;
                    s[" ", "Locked", "    "] = this.Locked ?? BU.Locked.Locked;
                    s.TestFieldNeeds();
                    if (this.keysize <= 0) this.keysize = null;
                    s[" ", "rsakey", "    "] = new RSACryptoServiceProvider(this.keysize ?? 1024).ToXmlString(true);
                    s[" ", "ExpireTime", ""] = (StringEx.sql_str)"dateadd(mm, 1, getdate())";
                    s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
                    sqlstr = s.BuildEx2("insert into AdminAuth (", sqltool._Fields, ") values (", sqltool._Values, ")");
                }
                else
                {
                    sqltool s = new sqltool();
                    s["N", "header", "    ", row.header, ""] = this.header * text.ValidAsString;
                    s[" ", "idstr", "     ", row.idstr, " "] = this.idstr * text.ValidAsString;
                    s[" ", "Locked", "    ", row.Locked, ""] = this.Locked;
                    if (this.keysize.HasValue)
                    {
                        s[" ", "rsakey", "    "] = new RSACryptoServiceProvider(this.keysize.Value).ToXmlString(true);
                        s[" ", "ExpireTime", ""] = (StringEx.sql_str)"dateadd(mm, 1, getdate())";
                    }
                    if (s.fields.Count == 0) return row;
                    s.values["AdminID"] = row.AdminID;
                    s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                    sqlstr = s.BuildEx("update AdminAuth set ", sqltool._FieldValue, " where AdminID={AdminID}");
                }
                sqlcmd.ExecuteNonQuery(sqlstr);
                return AdminAuthRow.GetRow(sqlcmd, this.AdminID);
            }
        }

        [ObjectInvoke, Permissions(Permissions.Code.admin2, Permissions.Flag.Write)]
        static object update(AdminAuthRowCommand command, string json_s, params object[] args) { return command.update(json_s, args); }
    }
}