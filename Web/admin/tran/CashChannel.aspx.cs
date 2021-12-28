using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Tools;
using web;

public class CashChannel_aspx : web.page
{
    protected static readonly CashChannelRowCommand[] RowCommands;
    protected static readonly LogType[] LogTypes;
    protected static readonly SqlSchemaTable[] Schemas;
    protected static readonly List<string> AllFields;
    protected static readonly List<string> ShareFields;
    static readonly string sql_select;
    static CashChannel_aspx()
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            RowCommands = new CashChannelRowCommand[] { _null<BankCardRowCommand>.value, _null<DinpayRowCommand>.value, _null<YeepayRowCommand>.value, _null<EcpssRowCommand>.value };
            LogTypes = new LogType[RowCommands.Length];
            Schemas = new SqlSchemaTable[RowCommands.Length];
            Dictionary<string, int> tmp = new Dictionary<string, int>();
            for (int i = 0; i < RowCommands.Length; i++)
            {
                LogTypes[i] = RowCommands[i].AcceptLogType;
                Schemas[i] = SqlSchemaTable.GetSchema(sqlcmd, RowCommands[i].TableName.value);
                foreach (string field in Schemas[i].Keys)
                {
                    if (tmp.ContainsKey(field))
                        tmp[field]++;
                    else
                        tmp[field] = 1;
                }
            }
            AllFields = new List<string>(tmp.Keys);
            ShareFields = new List<string>();
            foreach (KeyValuePair<string, int> p in tmp)
                if (p.Value == RowCommands.Length)
                    ShareFields.Add(p.Key);
            ShareFields.Add("FeesRate");
            for (int i = 0; i < RowCommands.Length; i++)
                ShareFields.Add(RowCommands[i].MerhantID_Name.value);

            StringBuilder sql = new StringBuilder("from (");
            for (int i = 0; i < RowCommands.Length; i++)
            {
                if (i != 0)
                    sql.Append(" union");
                sql.AppendLine();
                sql.Append("select ID");
                SqlSchemaTable schema = SqlSchemaTable.GetSchema(sqlcmd, RowCommands[i].TableName.value);
                foreach (string field in AllFields)
                {
                    if (field == "ID") continue;
                    if (schema.ContainsKey(field))
                        sql.AppendFormat(",[{0}]", field);
                    else
                        sql.AppendFormat(",null as [{0}]", field);
                }
                sql.AppendFormat(" from {0} nolock", RowCommands[i].TableName);
            }
            sql.Append(") a");
            sql_select = sql.ToString();
        }
    }

    protected static bool GetRowCommand(LogType? logType, out CashChannelRowCommand command)
    {
        command = null;
        if (logType.HasValue)
        {
            for (int i = 0; i < RowCommands.Length; i++)
            {
                if (RowCommands[i].AcceptLogType == logType.Value)
                {
                    command = RowCommands[i];
                    break;
                }
            }
        }
        return command != null;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class CashChannelSelect : jgrid.GridRequest<CashChannelSelect>
    {
        protected override string init_defaultkey() { return "BankName"; }
        protected override Dictionary<string, string> init_sortkeys()
        {
            return new Dictionary<string, string>()
        {
            {"ID", "ID"},
            {"CorpID", "CorpID"},
            {"LogType", "LogType"},
            {"Locked", "Locked"},
            {"CreateTime", "CreateTime"},
            {"CreateUser", "CreateUser"},
            {"ModifyUser", "ModifyUser"},
            {"ModifyTime", "ModifyTime"},
            {"FeesRate", "FeesRate"},
            {"MerhantId", "MerhantId"},
        };
        }

        [JsonProperty]
        public string LogType;
        [JsonProperty]
        public string Locked;

        [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Read | Permissions.Flag.Write)]
        static jgrid.GridResponse<CashChannelRow> execute(CashChannelSelect command, string json_s, params object[] args)
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
            {
                jgrid.GridResponse<CashChannelRow> data = new jgrid.GridResponse<CashChannelRow>();

                StringBuilder sql = new StringBuilder(sql_select);

                int cnt = 0;
                sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
                sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<BU.LogType>());
                sql_where(sql, ref cnt, "a.Locked={0}", (byte?)command.Locked.ToEnum<BU.Locked>());

                data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
                //data.page_size = command.page_size;
                foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * {0} order by {1}", sql, command.GetOrderBy()))
                {
                    CashChannelRowCommand cmd;
                    if (GetRowCommand((LogType)r.GetInt32("LogType"), out cmd))
                        data.rows.Add((CashChannelRow)r.ToObject(cmd.RowType));
                }
                return data;
            }
        }
    }

    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Read | Permissions.Flag.Write)]
    static CashChannelRow execute(CashChannelGetRow command, string json_s, params object[] args)
    {
        return null;
    }

    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Read | Permissions.Flag.Write)]
    static CashChannelRow execute(CashChannelUpdate command, string json_s, params object[] args)
    {
        CashChannelRowCommand cmd;
        if (GetRowCommand(command.LogType, out cmd))
            return ((CashChannelRowCommand)api.DeserializeObject(cmd.GetType(), json_s)).update(json_s, args);
        return null;
    }

    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Read | Permissions.Flag.Write)]
    static CashChannelRow execute(CashChannelInsert command, string json_s, params object[] args)
    {
        CashChannelRowCommand cmd;
        if (GetRowCommand(command.LogType, out cmd))
            return ((CashChannelRowCommand)api.DeserializeObject(cmd.GetType(), json_s)).insert(json_s, args);
        return null;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class CashChannelGetRow : IRowCommand
    {
        [JsonProperty]
        public Guid? ID;
        [JsonProperty]
        public LogType? LogType;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class CashChannelUpdate : IRowCommand
    {
        [JsonProperty]
        public Guid? ID;
        [JsonProperty]
        public LogType? LogType;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class CashChannelInsert : IRowCommand
    {
        [JsonProperty]
        public LogType? LogType;
    }
}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class CashChannelUpdate : CashChannelRowCommand2, IRowCommand
//{
//    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Write)]
//    static object execute(CashChannelUpdate command, string json_s, params object[] args) { return command.update(json_s, args); }
//}

//[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//class CashChannelInsert : CashChannelRowCommand2, IRowCommand
//{
//    [ObjectInvoke, Permissions(Permissions.Code.tran_bankcard, Permissions.Flag.Write)]
//    static object execute(CashChannelInsert command, string json_s, params object[] args) { return command.insert(json_s, args); }
//}
