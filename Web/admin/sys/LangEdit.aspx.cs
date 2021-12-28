using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using web;
using Tools.Protocol;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class LangSelect1 : jgrid.GridRequest
{
    [ObjectInvoke, Permissions(Permissions.Code.lang_edit, Permissions.Flag.Write | Permissions.Flag.Read)]
    public static jgrid.GridResponse<LangRow1> select(LangSelect1 command, string json_s, params object[] args)
    {
        jgrid.GridResponse<LangRow1> data = new jgrid.GridResponse<LangRow1>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select key1 from Lang nolock group by key1"))
                data.rows.Add(r.ToObject<LangRow1>());
        return data;
    }

    public static string encode(string s) { if (string.IsNullOrEmpty(s)) return ""; return Convert.ToBase64String(Encoding.ASCII.GetBytes(s)).Replace("=", "_"); }

    public static string decode(string s) { if (string.IsNullOrEmpty(s)) return ""; return Encoding.ASCII.GetString(Convert.FromBase64String(s.Replace("_", "="))); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class LangUpdate1 : LangRow1Command, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class LangInsert1 : LangRow1Command, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class LangDelete1 : LangRow1Command, IRowCommand { }

class LangRow1Command
{
    [JsonProperty("key1A")]
    string key1_;
    public string key1A { get { return LangSelect1.decode(key1_); } }

    [JsonProperty("key1B")]
    public string key1B;

    void ValidInput()
    {
        //this.cls1 *= text.ValidAsString2;
        this.key1B *= text.ValidAsString2;
    }

    [ObjectInvoke, Permissions(Permissions.Code.lang_edit, Permissions.Flag.Write)]
    static LangRow1 insert(LangInsert1 command, string json_s, params object[] args)
    {
        command.ValidInput();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            return sqlcmd.ToObject<LangRow1>(true, @"insert into Lang (lcid,key1,key2,text) values (0,{key1B},'','') select top(1) * from Lang nolock where lcid=0 and key1={key1B} and key2=''".SqlExport(null, command));
    }

    [ObjectInvoke, Permissions(Permissions.Code.lang_edit, Permissions.Flag.Write)]
    static LangRow1 update(LangUpdate1 command, string json_s, params object[] args)
    {
        command.ValidInput();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            return sqlcmd.ToObject<LangRow1>(true, @"update Lang set key1={key1B} where key1={key1A} select top(1) * from Lang nolock where key1={key1B}".SqlExport(null, command));
    }

    [ObjectInvoke, Permissions(Permissions.Code.lang_edit, Permissions.Flag.Write)]
    static LangRow1 delete(LangDelete1 command, string json_s, params object[] args)
    {
        command.ValidInput();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            return sqlcmd.ToObject<LangRow1>(true, "select top(1) * from Lang nolock where key1={key1A} delete Lang where key1={key1B}".SqlExport(null, command));
    }

}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class LangRow1
{
    [DbImport]
    string key1;

    [JsonProperty]
    string key1A { get { return LangSelect1.encode(key1); } }
    [JsonProperty]
    string key1B { get { return key1; } }
}



[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class LangSelect2 : jgrid.GridRequest
{
    [JsonProperty("key1")]
    string key1_;
    public string key1 { get { return LangSelect1.decode(key1_); } }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class LangUpdate2 : LangRow2Command, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class LangInsert2 : LangRow2Command, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class LangDelete2 : LangRow2Command, IRowCommand { }

class LangRow2Command
{
    [JsonProperty("key1")]
    string key1_;
    public string key1 { get { return LangSelect1.decode(key1_); } }
    [JsonProperty]
    public string txtA;
    [JsonProperty]
    public string txtB;
    [JsonProperty, JsonProtocol.String(Trim = true, Empty = true)]
    public string en_us;
    [JsonProperty, JsonProtocol.String(Trim = true, Empty = true)]
    public string zh_cht;
    [JsonProperty, JsonProtocol.String(Trim = true, Empty = true)]
    public string zh_chs;

    void ValidInput()
    {
        //this.cls *= text.ValidAsString;
        this.txtA *= text.ValidAsString2;
        this.txtB *= text.ValidAsString2;
        this.zh_cht *= text.ValidAsString2;
        this.zh_chs *= text.ValidAsString2;
    }

    [ObjectInvoke, Permissions(Permissions.Code.lang_edit, Permissions.Flag.Write | Permissions.Flag.Read)]
    public static jgrid.GridResponse<LangRow2> select(LangSelect2 command, string json_s, params object[] args)
    {
        jgrid.GridResponse<LangRow2> data = new jgrid.GridResponse<LangRow2>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from Lang nolock where /*lcid<>0 and*/ key1='{0}' and key2<>''", command.key1 * text.ValidAsString2))
            {
                string key2 = r.GetString("key2");
                LangRow2 row = null;
                foreach (LangRow2 row1 in data.rows)
                {
                    if (row1.txtA == key2)
                    {
                        row = row1;
                        break;
                    }
                }
                if (row == null)
                    data.rows.Add(row = r.ToObject<LangRow2>());
                row.set_text(r.GetInt32("lcid"), r.GetString("text"));
            }
        }
        return data;
    }

    [ObjectInvoke, Permissions(Permissions.Code.lang_edit, Permissions.Flag.Write)]
    static LangRow2 insert(LangInsert2 command, string json_s, params object[] args)
    {
        command.ValidInput();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            try
            {
                sqlcmd.BeginTransaction();
                LangRow2 row = null;
//                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"
//insert into Lang (cls,lcid,txt1,txt2) values ({cls},0x0000,N{txtB},N{en_us})
//insert into Lang (cls,lcid,txt1,txt2) values ({cls},0x7C04,N{txtB},N{zh_cht})
//insert into Lang (cls,lcid,txt1,txt2) values ({cls},0x0004,N{txtB},N{zh_chs})
//select * from Lang nolock where cls={cls} and txt1=N{txtB}".SqlExport(null, command)))
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"
exec lang_update 0x0000,{key1},{txtB},N{en_us}
exec lang_update 0x7C04,{key1},{txtB},N{zh_cht}
exec lang_update 0x0004,{key1},{txtB},N{zh_chs}
select * from Lang nolock where key1={key1} and key2={txtB}".SqlExport(null, command)))
                {
                    (row = row ?? r.ToObject<LangRow2>()).set_text(r.GetInt32("lcid"), r.GetString("text"));
                }
                sqlcmd.Commit();
                return row;
            }
            catch
            {
                sqlcmd.Rollback();
                throw;
            }
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.lang_edit, Permissions.Flag.Write)]
    static LangRow2 update(LangUpdate2 command, string json_s, params object[] args)
    {
        command.ValidInput();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            try
            {
                sqlcmd.BeginTransaction();
                LangRow2 row = null;
//                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"update Lang set txt1=N{txtB} where cls={cls} and txt1=N{txtA}
//update Lang set txt2=N{en_us} where cls={cls} and txt1=N{txtB} and lcid=0x7C04
//update Lang set txt2=N{zh_cht} where cls={cls} and txt1=N{txtB} and lcid=0x7C04
//update Lang set txt2=N{zh_chs} where cls={cls} and txt1=N{txtB} and lcid=0x0004
//select * from Lang nolock where cls={cls} and txt1=N{txtB}".SqlExport(null, command)))
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"update Lang set key2={txtB} where key1={key1} and key2={txtA}
exec lang_update 0x0000,{key1},{txtB},N{en_us}
exec lang_update 0x7C04,{key1},{txtB},N{zh_cht}
exec lang_update 0x0004,{key1},{txtB},N{zh_chs}
select * from Lang nolock where key1={key1} and key2={txtB}".SqlExport(null, command)))
                {
                    (row = row ?? r.ToObject<LangRow2>()).set_text(r.GetInt32("lcid"), r.GetString("text"));
                }
                sqlcmd.Commit();
                return row;
            }
            catch
            {
                sqlcmd.Rollback();
                throw;
            }
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.lang_edit, Permissions.Flag.Write)]
    static LangRow2 delete(LangDelete2 command, string json_s, params object[] args)
    {
        command.ValidInput();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            return sqlcmd.ToObject<LangRow2>(true, "select * from Lang nolock where key1={key1} and key2={txtA} delete Lang where key1={key1} and key2={txtA}".SqlExport(null, command));
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class LangRow2
{
    [JsonProperty, DbImport("key2")]
    public string txtA;
    [JsonProperty, DbImport("key2")]
    public string txtB;
    [JsonProperty, DbImport]
    public string en_us;
    [JsonProperty, DbImport]
    public string zh_cht;
    [JsonProperty, DbImport]
    public string zh_chs;

    internal string set_text(int lcid, string text)
    {
        if (lcid == 0x0004) return this.zh_chs = text;
        if (lcid == 0x7C04) return this.zh_cht = text;
        if (lcid == 0x0000) return this.en_us = text;
        return text;
    }
}
