using BU;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using System;
using web;

#region AgentTreeSelect

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AgentTreeSelect : jgrid.GridRequest<AgentTreeSelect>
{
    protected override string init_defaultkey() { return "a.CreateTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"ParentACNT", "b.ACNT"},
            {"ID", "a.ID"},
            {"CorpID", "a.CorpID"},
            {"ACNT", "a.ACNT"},
            {"GroupID", "a.CorpID {0}, a.GroupID"},
            {"Name", "a.Name"},
            {"Locked", "a.Locked"},
            {"Currency", "a.Currency"},
            {"Balance", "a.Balance"},
            {"ModifyUser", "a.ModifyUser"},
            {"ModifyTime", "a.ModifyTime"},
            {"CreateUser", "a.CreateUser"},
        };
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AgentTreeRow
    {
        [DbImport, JsonProperty]
        public int? ID;
        [DbImport, JsonProperty]
        public int? CorpID;
        [DbImport, JsonProperty]
        public string ACNT;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public int? ParentID;

        [DbImport, JsonProperty]
        public int? level = 0;
        [JsonProperty]
        public bool? isLeaf;
        [JsonProperty]
        public bool? expanded;
        [JsonProperty]
        public bool? loaded;
    }

    [JsonProperty]
    public int? nodeid;
    [JsonProperty]
    public int? n_level;

    public static AgentTreeRow getAgent(SqlCmd sqlcmd, int agentID, int parentID)
    {
        return sqlcmd.ToObject<AgentTreeRow>("select ID,CorpID,ACNT,ParentID,Name,dbo.getAgentLevel(ID,{1}) level from Agent nolock where ID={0}", agentID, parentID);
    }
    public static void getAgents(SqlCmd sqlcmd, List<AgentTreeRow> list, int parentID, int corpID, int setLevel)
    {
        StringBuilder sql = new StringBuilder();
        sql.AppendFormat("select ID,CorpID,ACNT,ParentID,Name,{1} level from Agent nolock where ParentID={0}", parentID, setLevel);
        if (corpID != 0)
            sql.AppendFormat(" and CorpID={0}", corpID);
        foreach (SqlDataReader r in sqlcmd.ExecuteReader2(sql.ToString()))
            list.Add(r.ToObject<AgentTreeRow>());
    }

    [ObjectInvoke, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write | Permissions.Flag.Read)]
    static jgrid.GridResponse<AgentTreeRow> execute(AgentTreeSelect command, string json_s, params object[] args)
    {
        User user = HttpContext.Current.User as User;
        //user = new Agent() { ID = 1035 };
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<AgentTreeRow> data = new jgrid.GridResponse<AgentTreeRow>();
            if (user is Agent)
            {
                if (command.nodeid.HasValue)
                {
                    AgentTreeRow row1 = getAgent(sqlcmd, user.ID, 0);
                    AgentTreeRow row2 = getAgent(sqlcmd, command.nodeid.Value, row1.ID.Value);
                    getAgents(sqlcmd, data.rows, row2.ID.Value, row2.CorpID.Value, row2.level.Value + 2);
                }
                else
                {
                    AgentTreeRow row1 = getAgent(sqlcmd, user.ID, 0);
                    row1.level = 1;
                    data.rows.Add(row1);
                    getAgents(sqlcmd, data.rows, row1.ID.Value, row1.CorpID.Value, row1.level.Value + 1);
                }
            }
            else
            {
                if (command.nodeid.HasValue)
                {
                    AgentTreeRow row = getAgent(sqlcmd, command.nodeid.Value, 0);
                    getAgents(sqlcmd, data.rows, row.ID.Value, row.CorpID.Value, row.level.Value + 1);
                }
                else
                {
                    List<AgentTreeRow> list = new List<AgentTreeRow>();
                    getAgents(sqlcmd, list, 0, user.CorpID, 1);
                    foreach (AgentTreeRow row in list)
                    {
                        if ((user.CorpID == 0) && (row.CorpID == 0)) continue;
                        row.expanded = row.loaded = true;
                        data.rows.Add(row);
                        getAgents(sqlcmd, data.rows, row.ID.Value, 0, 2);
                    }
                }
            }
            return data;
        }
    }
}

#endregion

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AgentTest : IRowCommand
{
    // $.invoke_api({AgentTest:{d:3, a:3, m:5}})

    [JsonProperty]
    public int? d;
    [JsonProperty]
    public int? a;
    [JsonProperty]
    public int? m;

    [ObjectInvoke, Permissions(Permissions.Code.develover, Permissions.Flag.Write)]
    static object execute(AgentTest command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            create(0, sqlcmd, null, null, command.d ?? 1, command.a ?? 1, command.m ?? 1);
            return null;
        }
    }

    static void create(int d, SqlCmd sqlcmd, string parentACNT, string prefix, int max_d, int max_a, int max_m)
    {
        if (d >= max_d) return;
        for (int i = 1; i <= max_a; i++)
        {
            string acnt_a = string.Format("{0}{1}", prefix, i);
            AgentRow agent = AgentRow.GetAgent(sqlcmd, null, 2, acnt_a);
            if (agent == null) agent = new AgentRowCommand() { CorpID = 2, ParentACNT = parentACNT ?? "tengbo", ACNT = acnt_a }.insert(sqlcmd, null, null, null);
            for (int j = 1; j <= max_m; j++)
            {
                string acnt_m = string.Format("{0}{1}", acnt_a, j);
                MemberRow member = MemberRow.GetMember(sqlcmd, null, 2, acnt_m);
                if (member == null) member = new MemberRowCommand() { CorpID = 2, AgentACNT = agent.ACNT, ACNT = acnt_m }.insert(sqlcmd, null, null, null);
            }
            create(d + 1, sqlcmd, acnt_a, acnt_a, max_d, max_a, max_m);
        }
    }
}

#region Admin Select/Insert/Update


[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AdminSelect : jgrid.GridRequest<AdminSelect>
{
    protected override string init_defaultkey() { return "CreateTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"ID", "ID"},
            {"CorpID", "CorpID"},
            {"ACNT", "ACNT"},
            {"Name", "Name"},
            {"GroupID", "CorpID {0}, GroupID"},
            {"Locked", "Locked"},
            {"ModifyUser", "ModifyUser"},
            {"ModifyTime", "ModifyTime"},
            {"CreateUser", "CreateUser"},
        };
    }

    [JsonProperty]
    string ACNT;
    [JsonProperty]
    string Name;
    [JsonProperty]
    Locked? Locked;


    [ObjectInvoke, Permissions(Permissions.Code.admins_list, Permissions.Flag.Write | Permissions.Flag.Read)]
    static jgrid.GridResponse<AdminRow> select(AdminSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<AdminRow> data = new jgrid.GridResponse<AdminRow>();
            StringBuilder sql = new StringBuilder(@"from [Admin] nolock");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "Name like N'%{0}%'", (command.Name * text.ValidAsName).Remove("%"));
            sql_where(sql, ref cnt, "Locked={0}", (byte?)command.Locked);

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<AdminRow>());
            return data;
        }

        //jgrid.GridResponse<AdminRow> data = new jgrid.GridResponse<AdminRow>();
        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        //{
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from [Admin] nolock"))
        //       data.rows.Add(r.ToObject<AdminRow>());
        //    return data;
        //}
    }

    [ObjectInvoke, Permissions(Permissions.Code.admins_list, Permissions.Flag.Write)]
    static object update(AdminUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.admins_list, Permissions.Flag.Write)]
    static object insert(AdminInsert command, string json_s, params object[] args) { return command.insert(null, null, json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AdminUpdate : AdminRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AdminInsert : AdminRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AdminSetPassword : IRowCommand
{
    [JsonProperty]
    string Password1;
    [JsonProperty]
    string Password2;
    [JsonProperty]
    string Password3;

    [ObjectInvoke]
    static object update(AdminSetPassword command, string json_s, params object[] args)
    {
        Admin admin = (Admin)HttpContext.Current.User;
        if (string.IsNullOrEmpty(command.Password2))
            throw new RowException(RowErrorCode.FieldNeeds, "new password");
        if (command.Password2 != command.Password3)
            throw new RowException(RowErrorCode.FieldNeeds, "new password");
        return new AdminUpdate()
        {
            ID = ((Admin)HttpContext.Current.User).ID,
            Password = command.Password2,
            Password_verify = true,
            Password_old = command.Password1
        }.update(json_s, args);
    }
}

#endregion

#region Agent Select/Insert/Update

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AgentSelect : jgrid.GridRequest<AgentSelect>
{
    protected override string init_defaultkey() { return "a.CreateTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"ParentACNT", "b.ACNT"},
            {"ID", "a.ID"},
            {"CorpID", "a.CorpID"},
            {"ACNT", "a.ACNT"},
            {"GroupID", "a.CorpID {0}, a.GroupID"},
            {"Name", "a.Name"},
            {"Locked", "a.Locked"},
            {"Currency", "a.Currency"},
            {"Balance", "a.Balance"},
            {"ModifyUser", "a.ModifyUser"},
            {"ModifyTime", "a.ModifyTime"},
            {"CreateUser", "a.CreateUser"},
        };
    }

    [JsonProperty]
    int? ParentID;
    [JsonProperty]
    string ParentACNT;
    [JsonProperty]
    string ACNT;
    [JsonProperty]
    string Name;
    [JsonProperty]
    Locked? Locked;

    [ObjectInvoke, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write | Permissions.Flag.Read)]
    jgrid.GridResponse<AgentRow> execute(AgentSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<AgentRow> data = new jgrid.GridResponse<AgentRow>();
            StringBuilder sql = new StringBuilder(@"from [Agent] a with(nolock) left join Agent b on a.ParentID=b.ID");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", this.CorpID);
            sql_where(sql, ref cnt, "a.ParentID='{0}'", this.ParentID);
            sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (this.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (this.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (this.Name * text.ValidAsName).Remove("%"));
            sql_where(sql, ref cnt, "a.Locked={0}", (byte?)this.Locked);

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = this.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, a.*, b.ACNT as ParentACNT
{3}) a where rowid>{0} and rowid<={1} order by rowid", this.rows_start, this.rows_end, this.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<AgentRow>());
            return data;
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write)]
    static object execute(AgentInsert command, string json_s, params object[] args) { return command.insert(null, null, json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.agents_list, Permissions.Flag.Write)]
    static object execute(AgentUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AgentInsert : AgentRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class AgentUpdate : AgentRowCommand, IRowCommand { }

#endregion

#region Member Select/Insert/Update

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberSelect : jgrid.GridRequest<MemberSelect>
{
    protected override string init_defaultkey() { return "a.CreateTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"AgentACNT", "b.ACNT"},
            {"LoginTime", "c.LoginTime"},
            {"LoginIP", "c.IP"},
            {"ID", "a.ID"},
            {"CorpID", "a.CorpID"},
            {"ACNT", "a.ACNT"},
            {"Name", "a.Name"},
            {"GroupID", "a.CorpID {0}, a.GroupID"},
            {"Locked", "a.Locked"},
            {"Balance", "a.Balance"},
            {"Currency", "a.Currency"},
            {"Memo", "a.Memo"},
            {"RegisterIP", "a.RegisterIP"},
            {"ModifyUser", "a.ModifyUser"},
            {"ModifyTime", "a.ModifyTime"},
            {"CreateUser", "a.CreateUser"},
        };
    }

    [JsonProperty]
    int? AgentID;
    [JsonProperty]
    string AgentACNT;
    [JsonProperty]
    string ACNT;
    [JsonProperty]
    string Name;
    [JsonProperty]
    Locked? Locked;

    [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Read)]
    jgrid.GridResponse<MemberRow> OnExecute(MemberSelect command1, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<MemberRow> data = new jgrid.GridResponse<MemberRow>();
            StringBuilder sql = new StringBuilder(@"from Member a with(nolock) left join Agent b with(nolock) on a.AgentID=b.ID left join LoginState c with(nolock) on a.ID=c.ID");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", this.CorpID);
            sql_where(sql, ref cnt, "a.AgentID='{0}'", this.AgentID);
            sql_where(sql, ref cnt, "b.ACNT like '%{0}%'", (this.AgentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (this.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "a.Name like N'%{0}%'", (this.Name * text.ValidAsName).Remove("%"));
            sql_where(sql, ref cnt, "a.Locked={0}", (byte?)this.Locked);

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = this.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, b.ACNT AgentACNT, a.*, c.IP as LoginIP, c.LoginTime
{3}) a where rowid>{0} and rowid<={1} order by rowid", this.rows_start, this.rows_end, this.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<MemberRow>());
            return data;
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
    static object execute(MemberInsert command, string json_s, params object[] args) { return command.insert(null, null, json_s, args); }

    [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
    static object execute(MemberUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberInsert : MemberRowCommand, IRowCommand { }

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberUpdate : MemberRowCommand, IRowCommand { }

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberGameSelect : IRowCommand
//{
//    [JsonProperty]
//    GameID? GameID;
//    [JsonProperty]
//    int? MemberID;

//    [ObjectInvoke, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Read)]
//    static MemberGameRow execute(MemberGameSelect command, string json_s, params object[] args)
//    {
//        return MemberGame.GetInstance(command.GameID).SelectRow(null, command.MemberID, true);
//    }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class MemberGameUpdate : IRowCommand
//{
//    [JsonProperty]
//    GameID? GameID;

//    [ObjectInvoke, Permissions(Permissions.Code.member_subacc, Permissions.Flag.Write)]
//    static object execute(MemberGameUpdate command, string json_s, params object[] args)
//    {
//        return MemberGame.GetInstance(command.GameID).DeserializeObject(json_s).Update(json_s, args);
//    }
//}

#endregion

#region Agent BankCard

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class AgentBankCardRow
{
    [DbImport, JsonProperty]
    public int? AgentID { get; set; }
    [DbImport, JsonProperty]
    public int? Index { get; set; }
    [DbImport, JsonProperty]
    public string CardID { get; set; }
    [DbImport, JsonProperty]
    public string BankName { get; set; }
    [DbImport, JsonProperty]
    public string AccountName { get; set; }
    [DbImport, JsonProperty]
    public string Loc1 { get; set; }
    [DbImport, JsonProperty]
    public string Loc2 { get; set; }
    [DbImport, JsonProperty]
    public string Loc3 { get; set; }
}

public class AgentBankCardRowCommand : AgentBankCardRow
{
    [JsonProperty]
    public virtual int? newIndex { get; set; }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentBankCardInsert : AgentBankCardRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentBankCardUpdate : AgentBankCardRowCommand, IRowCommand { }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class AgentBankCardDelete : AgentBankCardRowCommand, IRowCommand { }

    [ObjectInvoke, Permissions("agent3", Permissions.Flag.Write)]
    static AgentBankCardRow insert(AgentBankCardInsert command, string json_s, params object[] args)
    {
        sqltool s = new sqltool();
        s["* ", "AgentID", "    "] = command.AgentID;
        s["* ", "Index", "      "] = command.newIndex;
        s["* ", "CardID", "     "] = text.ValidAsString * command.CardID;
        s["*N", "BankName", "   "] = text.ValidAsString * command.BankName;
        s["*N", "AccountName", ""] = text.ValidAsString * command.AccountName;
        s[" N", "Loc1", "       "] = text.ValidAsString * command.Loc1;
        s[" N", "Loc2", "       "] = text.ValidAsString * command.Loc2;
        s[" N", "Loc3", "       "] = text.ValidAsString * command.Loc3;
        s.TestFieldNeeds();
        s.Values["AgentID_"] = s.Values["AgentID"];
        s.Values["AgentID"] = (StringEx.sql_str)"ID";
        string sqlstr = s.BuildEx("insert into AgentBank (", sqltool._Fields, ") select ", sqltool._Values, " from Agent nolock where ID={AgentID_} select * from AgentBank nolock where AgentID={AgentID_} and [Index]={Index}");
        SqlCmd sqlcmd;
        using (DB.Open(DB.Name.Main, DB.Access.ReadWrite, out sqlcmd, args.GetValue<SqlCmd>(0)))
            return sqlcmd.ExecuteEx<AgentBankCardRow>(sqlstr);
    }

    [ObjectInvoke, Permissions("agent3", Permissions.Flag.Write)]
    static AgentBankCardRow update(AgentBankCardUpdate command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            AgentBankCardRow row = sqlcmd.GetRowEx<AgentBankCardRow>(RowErrorCode.NotFound, "select * from AgentBank nolock where AgentID={0} and [Index]={1}", command.AgentID, command.Index);
            sqltool s = new sqltool();
            s["* ", "Index", "      ", row.Index, "      "] = command.newIndex;
            s["* ", "CardID", "     ", row.CardID, "     "] = text.ValidAsString * command.CardID;
            s["*N", "BankName", "   ", row.BankName, "   "] = text.ValidAsString * command.BankName;
            s["*N", "AccountName", "", row.AccountName, ""] = text.ValidAsString * command.AccountName;
            s[" N", "Loc1", "       ", row.Loc1, "       "] = text.ValidAsString * command.Loc1;
            s[" N", "Loc2", "       ", row.Loc2, "       "] = text.ValidAsString * command.Loc2;
            s[" N", "Loc3", "       ", row.Loc3, "       "] = text.ValidAsString * command.Loc3;
            if (s.fields.Count == 0) return row;
            //s.SetUser(sqltool.ModifyTime, sqltool.ModifyUser);
            s.Values["AgentID"] = row.AgentID;
            s.Values["Index"] = command.newIndex;
            s.Values["oldIndex"] = row.Index;
            s.TestFieldNeeds();
            string sqlstr = s.BuildEx("update AgentBank set ", sqltool._FieldValue, " where AgentID={AgentID} and [Index]={oldIndex} select * from AgentBank nolock where AgentID={AgentID} and [Index]={Index}");
            return sqlcmd.ExecuteEx<AgentBankCardRow>(sqlstr);
        }
    }

    [ObjectInvoke, Permissions("agent3", Permissions.Flag.Write)]
    static AgentBankCardRow delete(AgentBankCardDelete command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            return sqlcmd.GetRowEx<AgentBankCardRow>(RowErrorCode.NotFound, "select * from AgentBank nolock where AgentID={0} and [Index]={1} delete AgentBank where AgentID={0} and [Index]={1}", command.AgentID, command.Index);
    }
}

#endregion

#region Member BankCard

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberBankCardUpdate : MemberBankCardRowCommand, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
    static object execute(MemberBankCardUpdate command, string json_s, params object[] args)
    {
        command.throwNotFound = false;
        return command.update(json_s, args);
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberBankCardInsert : MemberBankCardRowCommand, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
    static object execute(MemberBankCardInsert command, string json_s, params object[] args)
    {
        command.throwNotFound = false;
        return command.insert(json_s, args);
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class MemberBankCardDelete : MemberBankCardRowCommand, IRowCommand
{
    [ObjectInvoke, Permissions(Permissions.Code.members_list, Permissions.Flag.Write)]
    static object execute(MemberBankCardDelete command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            return sqlcmd.GetRowEx<MemberBankCardRow>(RowErrorCode.NotFound, "select * from MemberBank nolock where MemberID={0} and [Index]={1} delete MemberBank where MemberID={0} and [Index]={1}", command.MemberID, command.Index);
    }
}

#endregion


// 存款額 / (1-代理佔成) = 上分額度
// 存款額 / 公司佔成 = 上分額度

namespace web
{
    public abstract class UserDetails_page : web.page
    {
        public abstract UserType UserType { get; }
    }

    public abstract class UserList2_aspx : web.page
    {
        public List<game.UserGameRow> rows1;
        public List<game.UserGameRow> rows2;
        public abstract UserType UserType { get; }

        public int userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.userID = Request.QueryString["id"].ToInt32() ?? 0;
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                foreach (GameRow g in GameRow.Cache.GetInstance(sqlcmd, null).Rows)
                {
                    game.IUserGameRowCommand cmd = game.GetUserGameRowCommand(this.UserType, g.ID, null, true);
                    if (cmd != null)
                    {
                        game.UserGameRow row = cmd.SelectUserRow(sqlcmd, userID, null, true);
                        if (cmd.HasAPI)
                        {
                            if (this.rows1 == null)
                                this.rows1 = new List<game.UserGameRow>();
                            this.rows1.Add(row);
                        }
                        else
                        {
                            if (this.rows2 == null)
                                this.rows2 = new List<game.UserGameRow>();
                            this.rows2.Add(row);
                        }
                    }
                }
            }
        }
    }

    abstract class UserGameCommand : IRowCommand
    {
        [JsonProperty]
        protected UserType? UserType;

        [JsonProperty]
        protected GameID? GameID;
    }
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    sealed class UserGameUpdate : UserGameCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.agent2, Permissions.Flag.Write)]
        static game.UserGameRow execute(UserGameUpdate command, string json_s, params object[] args)
        {
            return game.GetUserGameRowCommand(command.UserType, command.GameID, json_s, false).UpdateUserRow(null, json_s, args);
        }
    }
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    sealed class UserGameBalance : UserGameCommand
    {
        [ObjectInvoke, Permissions(Permissions.Code.agent2, Permissions.Flag.Write)]
        static game.UserGameRow execute(UserGameBalance command, string json_s, params object[] args)
        {
            return game.GetUserGameRowCommand(command.UserType, command.GameID, json_s, false).GetBalance(null, json_s, args);
        }
    }
}