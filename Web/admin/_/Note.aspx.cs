using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;
using Tools.Protocol;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class NoteSelect : jgrid.GridRequest
{
    [JsonProperty]
    public NoteTypes? NoteType;
    
    [ObjectInvoke, api.Async]
    [Permissions(Permissions.Code.m_workingnote1, Permissions.Flag.Read | Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote2, Permissions.Flag.Read | Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote3, Permissions.Flag.Read | Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote4, Permissions.Flag.Read | Permissions.Flag.Write)]
    static jgrid.GridResponse<NoteRow> execute(NoteSelect command, string json_s, params object[] args)
    {
        command.NoteType = text.ValidEnum(command.NoteType) ?? NoteTypes.Events;
        jgrid.GridResponse<NoteRow> data = new jgrid.GridResponse<NoteRow>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from WorkingNote nolock where NoteType={0} order by CreateTime desc", (int)command.NoteType))
                data.rows.Add(r.ToObject<NoteRow>());
            //data["corps"] = CorpRow.Cache.GetInstance(null, sqlcmd).names;
            return data;
        }
    }

    [ObjectInvoke, api.Async]
    [Permissions(Permissions.Code.m_workingnote1, Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote2, Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote3, Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote4, Permissions.Flag.Write)]
    static object execute(NoteUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

    [ObjectInvoke, api.Async]
    [Permissions(Permissions.Code.m_workingnote1, Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote2, Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote3, Permissions.Flag.Write)]
    [Permissions(Permissions.Code.m_workingnote4, Permissions.Flag.Write)]
    static object execute(NoteInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
}

namespace BU
{
    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum NoteTypes : byte
    {
        Events = 0, Notifys = 1, Reports = 2, SystemUpdate = 3
    }
    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum NoteStates : byte
    {
        None = 0, Closed = 1,
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class NoteRow
{
    [DbImport, JsonProperty]
    public int? ID;
    [DbImport, JsonProperty]
    public NoteTypes NoteType;
    [DbImport, JsonProperty]
    public NoteStates NoteState;
    [DbImport, JsonProperty]
    public string Note;
    [DbImport, JsonProperty]
    public DateTime? CreateTime;
    [DbImport, JsonProperty]
    public int? CreateUser;
    [DbImport, JsonProperty]
    public DateTime? ModifyTime;
    [DbImport, JsonProperty]
    public int? ModifyUser;
}

abstract class NoteRowCommand
{
    [JsonProperty]
    public int? ID;
    [JsonProperty]
    public NoteTypes? NoteType;
    [JsonProperty]
    public NoteStates? NoteState;
    [JsonProperty]
    public string Note;

    public NoteRow update(string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            NoteRow row = sqlcmd.GetRowEx<NoteRow>(RowErrorCode.NotFound, "select * from WorkingNote nolock where ID={0}", this.ID);
            sqltool s = new sqltool();
            s[" ", "NoteType", " ", row.NoteType, " "] = this.NoteType;
            s[" ", "NoteState", "", row.NoteState, ""] = this.NoteState;
            s["N", "Note", "     ", row.Note, "     "] = text.ValidAsString * this.Note;
            if (s.fields.Count == 0) return row;
            s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
            s.Values["ID"] = row.ID;
            string sql = s.BuildEx("update WorkingNote set ", sqltool._FieldValue, @" where ID={ID} select * from WorkingNote nolock where ID={ID}");
            return sqlcmd.ExecuteEx<NoteRow>(sql);
        }
    }

    public NoteRow insert(string json_s, params object[] args)
    {
        sqltool s = new sqltool();
        s["  ", "NoteType", " "] = this.NoteType ?? BU.NoteTypes.Events;
        s["  ", "NoteState", ""] = this.NoteState ?? BU.NoteStates.None;
        s["*N", "Note", "     "] = text.ValidAsString * this.Note;
        s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
        s.TestFieldNeeds();
        string sqlstr = s.BuildEx("insert into WorkingNote (", sqltool._Fields, ") values (", sqltool._Values, @")
if @@rowcount>0 select * from WorkingNote nolock where ID=@@IDENTITY");
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            return sqlcmd.ExecuteEx<NoteRow>(sqlstr);
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class NoteUpdate : NoteRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class NoteInsert : NoteRowCommand, IRowCommand { }
