using BU;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using web;

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
