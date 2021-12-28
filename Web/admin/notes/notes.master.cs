using BU;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using web;
using Tools.Protocol;

public partial class notes_master : masterpage
{
    public BU.NoteTypes? type;

    public bool NoteState = false;
}

namespace BU
{
    [JsonProtocol.UnderlyingValueInDictionaryKey]
    public enum NoteTypes : byte
    {
        Events = 0, Notifys = 1, Reports = 2, SystemUpdate = 3, Questions = 4
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
    //[JsonProperty("Note")]
    //string _Note
    //{
    //    get { return this.Note.Replace("\r\n", "<br>"); }
    //}

    [DbImport, JsonProperty]
    public DateTime? CreateTime;
    [DbImport, JsonProperty]
    public _SystemUser CreateUser;
    [DbImport, JsonProperty]
    public DateTime? ModifyTime;
    [DbImport, JsonProperty]
    public _SystemUser ModifyUser;
}

abstract class NoteSelect<TSelect, TUpdate, TInsert> : jgrid.GridRequest
    where TSelect : NoteSelect<TSelect, TUpdate, TInsert>, new()
    where TUpdate : NoteSelect<TSelect, TUpdate, TInsert>.RowCommand<TUpdate>
    where TInsert : NoteSelect<TSelect, TUpdate, TInsert>.RowCommand<TInsert>
{
    static readonly TSelect SelectCommand = new TSelect();
    public abstract NoteTypes NoteType { get; }

    public abstract class RowCommand<T> : IRowCommand
    {
        [JsonProperty]
        public int? ID;
        public NoteTypes NoteType { get { return SelectCommand.NoteType; } }
        [JsonProperty]
        public NoteStates? NoteState;
        [JsonProperty]
        public string Note;

        public virtual NoteRow update(TUpdate command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                NoteRow row = sqlcmd.GetRowEx<NoteRow>(RowErrorCode.NotFound, "select * from WorkingNote nolock where ID={0}", command.ID);
                sqltool s = new sqltool();
                s[" ", "NoteType", " ", row.NoteType, " "] = command.NoteType;
                s[" ", "NoteState", "", row.NoteState, ""] = command.NoteState;
                s["N", "Note", "     ", row.Note, "     "] = text.ValidAsString * command.Note;
                if (s.fields.Count == 0) return row;
                s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
                s.Values["ID"] = row.ID;
                string sql = s.BuildEx("update WorkingNote set ", sqltool._FieldValue, @" where ID={ID} select * from WorkingNote nolock where ID={ID}");
                return sqlcmd.ExecuteEx<NoteRow>(sql);
            }
        }

        public virtual NoteRow insert(TInsert command, string json_s, params object[] args)
        {
            sqltool s = new sqltool();
            s["  ", "NoteType", " "] = command.NoteType;
            s["  ", "NoteState", ""] = command.NoteState ?? BU.NoteStates.None;
            s["*N", "Note", "     "] = text.ValidAsString * command.Note;
            s.SetUser(sqltool.CreateUser, sqltool.ModifyUser);
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx("insert into WorkingNote (", sqltool._Fields, ") values (", sqltool._Values, @")
if @@rowcount>0 select * from WorkingNote nolock where ID=@@IDENTITY");
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                return sqlcmd.ExecuteEx<NoteRow>(sqlstr);
        }
    }

    public virtual jgrid.GridResponse<NoteRow> select(TSelect command, string json_s, params object[] args)
    {
        jgrid.GridResponse<NoteRow> data = new jgrid.GridResponse<NoteRow>();
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from WorkingNote nolock where NoteType={0} order by CreateTime desc", (int)command.NoteType))
                data.rows.Add(r.ToObject<NoteRow>());
            //data["corps"] = CorpRow.Cache.GetInstance(null, sqlcmd).names;
            return data;
        }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note0Select : NoteSelect<Note0Select, Note0Update, Note0Insert>
{
    public override NoteTypes NoteType { get { return NoteTypes.Events; } }

    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote0, Permissions.Flag.Read)]
    public override jgrid.GridResponse<NoteRow> select(Note0Select command, string json_s, params object[] args) { return base.select(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note0Update : Note0Select.RowCommand<Note0Update>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote0, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow update(Note0Update command, string json_s, params object[] args) { return base.update(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note0Insert : Note0Select.RowCommand<Note0Insert>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote0, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow insert(Note0Insert command, string json_s, params object[] args) { return base.insert(command, json_s, args); }
}



[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note1Select : NoteSelect<Note1Select, Note1Update, Note1Insert>
{
    public override NoteTypes NoteType { get { return NoteTypes.Notifys; } }

    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote1, Permissions.Flag.Read)]
    public override jgrid.GridResponse<NoteRow> select(Note1Select command, string json_s, params object[] args) { return base.select(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note1Update : Note1Select.RowCommand<Note1Update>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote1, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow update(Note1Update command, string json_s, params object[] args) { return base.update(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note1Insert : Note1Select.RowCommand<Note1Insert>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote1, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow insert(Note1Insert command, string json_s, params object[] args) { return base.insert(command, json_s, args); }
}



[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note2Select : NoteSelect<Note2Select, Note2Update, Note2Insert>
{
    public override NoteTypes NoteType { get { return NoteTypes.Reports; } }

    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote2, Permissions.Flag.Read)]
    public override jgrid.GridResponse<NoteRow> select(Note2Select command, string json_s, params object[] args) { return base.select(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note2Update : Note2Select.RowCommand<Note2Update>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote2, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow update(Note2Update command, string json_s, params object[] args) { return base.update(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note2Insert : Note2Select.RowCommand<Note2Insert>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote2, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow insert(Note2Insert command, string json_s, params object[] args) { return base.insert(command, json_s, args); }
}



[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note3Select : NoteSelect<Note3Select, Note3Update, Note3Insert>
{
    public override NoteTypes NoteType { get { return NoteTypes.SystemUpdate; } }

    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote3, Permissions.Flag.Read)]
    public override jgrid.GridResponse<NoteRow> select(Note3Select command, string json_s, params object[] args) { return base.select(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note3Update : Note3Select.RowCommand<Note3Update>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote3, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow update(Note3Update command, string json_s, params object[] args) { return base.update(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note3Insert : Note3Select.RowCommand<Note3Insert>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote3, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow insert(Note3Insert command, string json_s, params object[] args) { return base.insert(command, json_s, args); }
}



[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note4Select : NoteSelect<Note4Select, Note4Update, Note4Insert>
{
    public override NoteTypes NoteType { get { return NoteTypes.Questions; } }

    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote4, Permissions.Flag.Read)]
    public override jgrid.GridResponse<NoteRow> select(Note4Select command, string json_s, params object[] args) { return base.select(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note4Update : Note4Select.RowCommand<Note4Update>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote4, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow update(Note4Update command, string json_s, params object[] args) { return base.update(command, json_s, args); }
}
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class Note4Insert : Note4Select.RowCommand<Note4Insert>
{
    [ObjectInvoke, Permissions(Permissions.Code.m_workingnote4, Permissions.Flag.Read | Permissions.Flag.Write)]
    public override NoteRow insert(Note4Insert command, string json_s, params object[] args) { return base.insert(command, json_s, args); }
}
