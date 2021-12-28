using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using web;

#region 會員點數明細

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_PointsSelect : jgrid.GridRequest<GameLog_PointsSelect>
{
    protected override string init_defaultkey() { return "CreateTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime", null},
                {"CreateTime", null},
                {"GameID", null},
                {"LogType", null},
                {"CorpID", null},
                {"ParentACNT", null},
                {"ACNT", null},
                {"PrevBalance", null},
                {"Amount", null},
                {"Balance", null},
                {"CurrencyA", null},
                {"CurrencyB", null},
                {"CurrencyX", null},
                {"SerialNumber", null},
                {"RequestIP", null},
                {"RequestTime", null},
                {"FinishTime", null},
            };
    }

    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;
    [JsonProperty]
    public string SerialNumber;

    [ObjectInvoke, Permissions("log_000", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_PointsSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"from GameLog_000 nolock");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "lower(SerialNumber) like lower('%{0}%')", (command.SerialNumber * text.ValidAsString).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from ( select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class TranLogSelect : jgrid.GridRequest<TranLogSelect>
{
    protected override string init_defaultkey() { return "CreateTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime", null},
                {"CreateTime", null},
                {"LogType", null},
                {"GameID", null},
                {"CorpID", null},
                {"UserType", null},
                {"UserID", null},
                {"UserACNT", null},
                {"ParentID", null},
                {"ParentACNT", null},
                {"PrevBalance", null},
                {"Amount", null},
                {"Balance", null},
                {"Profit", null},
                {"CurrencyA", null},
                {"CurrencyB", null},
                {"CurrencyX", null},
                {"SerialNumber", null},
                {"RequestIP", null},
                {"RequestTime", null},
                {"FinishTime", null},
            };
    }

    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public byte? IsProvider;
    [JsonProperty]
    public string UserType;
    [JsonProperty]
    public int? UserID;
    [JsonProperty]
    public string UserACNT;
    [JsonProperty]
    public int? ParentID;
    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string SerialNumber;

    [ObjectInvoke, Permissions("log_000", Permissions.Flag.Read)]
    static jgrid.GridResponse<TranLogRow> execute(TranLogSelect command, string json_s, params object[] args)
    {
        if ((command.IsProvider != 0) && (command.IsProvider != 1)) command.IsProvider = null;
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<TranLogRow> data = new jgrid.GridResponse<TranLogRow>();
            StringBuilder sql = new StringBuilder(@"from TranLog nolock");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "IsProvider={0}", command.IsProvider);
            sql_where(sql, ref cnt, "UserType={0}", (int?)command.UserType.ToEnum<UserType>());
            sql_where(sql, ref cnt, "UserACNT like '%{0}%'", (command.UserACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "UserID={0}", command.UserID);
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ParentID={0}", command.ParentID);
            sql_where(sql, ref cnt, "lower(SerialNumber) like lower('%{0}%')", (command.SerialNumber * text.ValidAsString).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from ( select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<TranLogRow>());
            return data;
        }
    }
}

#endregion

#region 會員單日點數

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_PointsDSelect : jgrid.GridRequest<GameLog_PointsDSelect>
{
    protected override string init_defaultkey() { return "ACTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime", null},
                {"GameID", null},
                {"LogType", null},
                {"CorpID", null},
                {"ParentACNT", null},
                {"ACNT", null},
                {"Amount", null},
                {"CurrencyA", null},
                {"CurrencyB", null},
                {"CurrencyX", null},
            };
    }

    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;

    [ObjectInvoke, Permissions("log_000d", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_PointsDSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"ACTime ,LogType ,GameID ,CorpID ,MemberID ,ACNT ,ParentID ,ParentACNT, CurrencyA ,CurrencyB ,CurrencyX from GameLog_000 nolock");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));

            sql.Append(" group by ACTime ,LogType ,GameID ,CorpID ,MemberID ,ACNT ,ParentID ,ParentACNT ,CurrencyA ,CurrencyB ,CurrencyX");

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) from (select {0}) a", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by {2}) as rowid, * from (select sum(Amount) Amount, {3}) a) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 會員單週點數

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_PointsWSelect : jgrid.GridRequest<GameLog_PointsWSelect>
{
    protected override string init_defaultkey() { return "ACTime1"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime1", null},
                {"ACTime2", null},
                {"GameID", null},
                {"LogType", null},
                {"CorpID", null},
                {"ParentACNT", null},
                {"ACNT", null},
                {"Amount", null},
                {"CurrencyA", null},
                {"CurrencyB", null},
                {"CurrencyX", null},
            };
    }

    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;

    [ObjectInvoke, Permissions("log_000w", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_PointsWSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"dateadd(dd,(datediff(dd,0,ACTime)/7)*7,0) ACTime1, dateadd(dd,((datediff(dd,0,ACTime)/7)*7)+6,0) ACTime2,LogType ,GameID ,CorpID ,MemberID ,ACNT ,ParentID ,ParentACNT, CurrencyA ,CurrencyB ,CurrencyX from GameLog_000 nolock");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));

            sql.Append(" group by dateadd(dd,(datediff(dd,0,ACTime)/7)*7,0), dateadd(dd,((datediff(dd,0,ACTime)/7)*7)+6,0) ,LogType ,GameID ,CorpID ,MemberID ,ACNT ,ParentID ,ParentACNT ,CurrencyA ,CurrencyB ,CurrencyX");

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) from (select {0}) a", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by {2}) as rowid, * from (select sum(Amount) Amount, {3}) a) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 會員單月點數

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_PointsMSelect : jgrid.GridRequest<GameLog_PointsMSelect>
{
    protected override string init_defaultkey() { return "ACMonth"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACMonth", null},
                {"GameID", null},
                {"LogType", null},
                {"CorpID", null},
                {"ParentACNT", null},
                {"ACNT", null},
                {"Amount", null},
                {"CurrencyA", null},
                {"CurrencyB", null},
                {"CurrencyX", null},
            };
    }

    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;

    [ObjectInvoke, Permissions("log_000m", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_PointsMSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"dateadd(mm,datediff(mm,0,ACTime),0) ACMonth ,LogType ,GameID ,CorpID ,MemberID ,ACNT ,ParentID ,ParentACNT, CurrencyA ,CurrencyB ,CurrencyX from GameLog_000 nolock");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));

            sql.Append(" group by dateadd(mm,datediff(mm,0,ACTime),0) ,LogType ,GameID ,CorpID ,MemberID ,ACNT ,ParentID ,ParentACNT ,CurrencyA ,CurrencyB ,CurrencyX");

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) from (select {0}) a", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by {2}) as rowid, * from (select sum(Amount) Amount, {3}) a) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 代理單日點數

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_PointsADSelect : jgrid.GridRequest<GameLog_PointsADSelect>
{
    protected override string init_defaultkey() { return "ACTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime", null},
                {"GameID", null},
                {"LogType", null},
                {"CorpID", null},
                {"ParentACNT", null},
                {"ACNT", null},
                {"Amount", null},
                {"CurrencyA", null},
                {"CurrencyB", null},
                {"CurrencyX", null},
            };
    }

    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;

    [ObjectInvoke, Permissions("log_000ad", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_PointsADSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"ACTime ,LogType ,GameID ,CorpID ,ParentID ,ParentACNT, CurrencyA ,CurrencyB ,CurrencyX from GameLog_000 nolock");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));

            sql.Append(" group by ACTime ,LogType ,GameID ,CorpID ,ParentID ,ParentACNT ,CurrencyA ,CurrencyB ,CurrencyX");

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) from (select {0}) a", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by {2}) as rowid, * from (select sum(Amount) Amount, {3}) a) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 代理單週點數

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_PointsAWSelect : jgrid.GridRequest<GameLog_PointsAWSelect>
{
    protected override string init_defaultkey() { return "ACTime1"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime1", null},
                {"ACTime2", null},
                {"GameID", null},
                {"LogType", null},
                {"CorpID", null},
                {"ParentACNT", null},
                {"ACNT", null},
                {"Amount", null},
                {"CurrencyA", null},
                {"CurrencyB", null},
                {"CurrencyX", null},
            };
    }

    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;

    [ObjectInvoke, Permissions("log_000aw", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_PointsAWSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"dateadd(dd,(datediff(dd,0,ACTime)/7)*7,0) ACTime1, dateadd(dd,((datediff(dd,0,ACTime)/7)*7)+6,0) ACTime2 ,LogType ,GameID ,CorpID ,ParentID ,ParentACNT, CurrencyA ,CurrencyB ,CurrencyX from GameLog_000 nolock");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));

            sql.Append(" group by dateadd(dd,(datediff(dd,0,ACTime)/7)*7,0), dateadd(dd,((datediff(dd,0,ACTime)/7)*7)+6,0) ,LogType ,GameID ,CorpID ,ParentID ,ParentACNT ,CurrencyA ,CurrencyB ,CurrencyX");

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) from (select {0}) a", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by {2}) as rowid, * from (select sum(Amount) Amount, {3}) a) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 代理單月點數

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_PointsAMSelect : jgrid.GridRequest<GameLog_PointsAMSelect>
{
    protected override string init_defaultkey() { return "ACMonth"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACMonth", null},
                {"GameID", null},
                {"LogType", null},
                {"CorpID", null},
                {"ParentACNT", null},
                {"ACNT", null},
                {"Amount", null},
                {"CurrencyA", null},
                {"CurrencyB", null},
                {"CurrencyX", null},
            };
    }

    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string LogType;
    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;

    [ObjectInvoke, Permissions("log_000am", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_PointsAMSelect command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"dateadd(mm,datediff(mm,0,ACTime),0) ACMonth ,LogType ,GameID ,CorpID ,ParentID ,ParentACNT, CurrencyA ,CurrencyB ,CurrencyX from GameLog_000 nolock");
            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "LogType={0}", (int?)command.LogType.ToEnum<LogType>());
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));

            sql.Append(" group by dateadd(mm,datediff(mm,0,ACTime),0) ,LogType ,GameID ,CorpID ,ParentID ,ParentACNT ,CurrencyA ,CurrencyB ,CurrencyX");

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) from (select {0}) a", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by {2}) as rowid, * from (select sum(Amount) Amount, {3}) a) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion



#region HG投注明細

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLogSelect_001 : jgrid.GridRequest<GameLogSelect_001>
{
    protected override string init_defaultkey() { return "BetStartTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime", null},
                {"CorpID", null},
                {"ParentID", null},
                {"ParentACNT", null},
                {"MemberID", null},
                {"ACNT", null},
                {"AccountId", null},
                {"UserID", null},
                {"GameType", null},
                {"TableId", null},
                {"TableName", null},
                {"GameId", null},
                {"BetId", null},
                {"BetStartTime", null},
                {"BetEndTime", null},
                {"BetAmount", null},
                {"BetAmountAct", null},
                {"Payout", null},
                {"BetSpot", null},
                {"BetNo", null},
                {"Currency", null},
            };
    }

    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;
    [JsonProperty]
    public string AccountId;

    [ObjectInvoke, Permissions("log_001", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLogSelect_001 command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"from GameLog_001 nolock");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "AccountId like '%{0}%'", (command.AccountId * text.ValidAsString2).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
        //jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        //{
        //    data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from GameLog_001 nolock"), command.page_size);
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by BetStartTime desc) as rowid, * from GameLog_001 nolock) a where a.rowid>{0} and a.rowid<={1} order by BetStartTime desc", command.rows_start, command.rows_end))
        //        data.rows.Add(r.ToObject<Dictionary<string, object>>());
        //    return data;
        //}
    }
}

#endregion

#region AG投注明细
[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLogSelect_011 : jgrid.GridRequest<GameLogSelect_011>
{
    protected override string init_defaultkey() { return "betTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime", null},
                {"GameID", null},
                {"CorpID", null},
                {"MemberID", null},
                {"ACNT", null},
                {"ParentID", null},
                {"ParentACNT", null},
                {"billNo", null},
                {"playerName", null},
                {"agentCode", null},
                {"gameCode", null},
                {"netAmount", null},
                {"betTime", null},
                {"gameType", null},
                {"betAmount", null},
                {"validBetAmount", null},
                {"flag", null},
                {"playType", null},
                {"currency", null},
                {"tableCode", null},
                {"loginIP", null},
                {"recalcuTime", null},
                {"platformId", null},
                {"platformType", null},
                {"stringex", null},
                {"remark", null},
                {"round", null},
            };
    }

    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;
    //[JsonProperty]
    //public string UserName;

    [ObjectInvoke, Permissions("log_011", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLogSelect_011 command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"from GameLog_011 nolock");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            //sql_where(sql, ref cnt, "UserName like '%{0}%'", (command.UserName * text.ValidAsString2).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
        //jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        //{
        //    data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from GameLog_001 nolock"), command.page_size);
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by BetStartTime desc) as rowid, * from GameLog_001 nolock) a where a.rowid>{0} and a.rowid<={1} order by BetStartTime desc", command.rows_start, command.rows_end))
        //        data.rows.Add(r.ToObject<Dictionary<string, object>>());
        //    return data;
        //}
    }
}

#endregion

#region BBIN投注明細

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLogSelect_009 : jgrid.GridRequest<GameLogSelect_009>
{
    protected override string init_defaultkey() { return "WagersDate"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime", null},
                {"CorpID", null},
                {"ParentID", null},
                {"ParentACNT", null},
                {"MemberID", null},
                {"ACNT", null},
                {"bbgametype", null},
                {"gamekind", null},
                {"UserName", null},
                {"WagersID", null},
                {"WagersDate", null},
                {"GameType", null},
                {"Result", null},
                {"BetAmount", null},
                {"Payoff", null},
                {"Currency", null},
                {"Commissionable", null},
                {"ExchangeRate", null},
                {"SerialID", null},
                {"RoundNo", null},
                {"GameCode", null},
                {"ResultType", null},
                {"Card", null},
                {"Commission", null},
                {"IsPaid", null},
            };
    }

    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;
    [JsonProperty]
    public string UserName;

    [ObjectInvoke, Permissions("log_009", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLogSelect_009 command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@"from GameLog_009 nolock");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "UserName like '%{0}%'", (command.UserName * text.ValidAsString2).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, * {3}) a where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
        //jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        //{
        //    data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from GameLog_001 nolock"), command.page_size);
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by BetStartTime desc) as rowid, * from GameLog_001 nolock) a where a.rowid>{0} and a.rowid<={1} order by BetStartTime desc", command.rows_start, command.rows_end))
        //        data.rows.Add(r.ToObject<Dictionary<string, object>>());
        //    return data;
        //}
    }
}

#endregion

#region 會員單日投注(依遊戲)

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtDG_Select : jgrid.GridRequest<GameLog_BetAmtDG_Select>
{
    protected override string init_defaultkey() { return "ACTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
        {
            {"ACTime",null},
            {"CorpID",null},
            {"ParentID",null},
            {"ParentACNT",null},
            {"MemberID",null},
            {"ACNT",null},
            {"GameID",null},
            {"GameType",null},
            {"BetAmount",null},
            {"BetAmountAct",null},
            {"Payout",null},
            {"CreateTime",null},
            {"CreateUser",null},
        };
    }

    [JsonProperty]
    public string ParentACNT;
    [JsonProperty]
    public string ACNT;
    [JsonProperty]
    public int? GameID;
    [JsonProperty]
    public string GameType;

    [ObjectInvoke, Permissions(Permissions.Code.log_betamtdg, Permissions.Flag.Read)]
    static jgrid.GridResponse<GameLog_BetAmtDG_Row> execute(GameLog_BetAmtDG_Select command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<GameLog_BetAmtDG_Row> data = new jgrid.GridResponse<GameLog_BetAmtDG_Row>();
            StringBuilder sql = new StringBuilder(@"from GameLog_BetAmtDG nolock");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "CorpID={0}", command.CorpID);
            if (command.GameID.HasValue)
                if ((command.GameID.Value == 0) || Enum.IsDefined(typeof(GameID), command.GameID))
                    sql_where(sql, ref cnt, "GameID={0}", command.GameID);
            sql_where(sql, ref cnt, "ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "GameType like '%{0}%'", (command.GameType * text.ValidAsString).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select a.*, b.Name from (select row_number() over (order by {2}) as rowid, * {3}) a left join Member b with(nolock) on a.MemberID=b.ID where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<GameLog_BetAmtDG_Row>());
            return data;
        }
        //jgrid.GridResponse<GameLog_BetAmtDG_Row> data = new jgrid.GridResponse<GameLog_BetAmtDG_Row>();
        //using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        //{
        //    data.setPager(sqlcmd.ExecuteScalar<int>("select count(*) from GameLog_BetAmtDG nolock"), command.page_size);
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (select row_number() over (order by ACTime desc, CorpID asc, ACNT asc) as rowid, * from GameLog_BetAmtDG nolock) a where a.rowid>{0} and a.rowid<={1} order by ACTime desc, ACNT asc", command.rows_start, command.rows_end))
        //        data.rows.Add(r.ToObject<GameLog_BetAmtDG_Row>());
        //    return data;
        //}
    }
}

#endregion

#region 會員單日投注

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtD_Select : jgrid.GridRequest<GameLog_BetAmtD_Select>
{
    protected override string init_defaultkey() { return "ACTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime"       ,"ACTime"},
                {"CorpID"       ,"CorpID"},
                {"GroupID"      ,"grp"},
                {"ParentID"     ,"ParentID"},
                {"ParentACNT"   ,"ParentACNT"},
                {"MemberID"     ,"MemberID"},
                {"ACNT"         ,"ACNT"},
                {"BetAmount"    ,"BetAmount"},
                {"BetAmountAct" ,"BetAmountAct"},
                {"Payout"       ,"Payout"},
                {"BonusRate"    ,"BonusRate"},
                {"Bonus"        ,"Bonus"},
            };
    }

    [JsonProperty]
    string ACNT;
    [JsonProperty]
    string ParentACNT;

    [ObjectInvoke, Permissions("log_betamtd", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_BetAmtD_Select command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@" from (
select ACTime, CorpID, MemberID, ACNT, ParentID, ParentACNT, sum(BetAmount) BetAmount, sum(BetAmountAct) BetAmountAct, sum(Payout) Payout
from GameLog_BetAmtDG a with(nolock)
group by ACTime, CorpID, MemberID, ACNT, ParentID, ParentACNT) a
left join Member b with(nolock) on a.MemberID=b.ID
left join MemberGroup c with(nolock) on b.CorpID=c.CorpID and b.GroupID=c.GroupID
");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "a.ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select *, CorpID*256+grp as GroupID from (
select row_number() over (order by {2}) as rowid, * from (
select a.*, b.GroupID as grp, b.Name, c.BonusW, c.BonusL, case when a.Payout>0 then c.BonusW else c.BonusL end as BonusRate, BetAmountAct * case when a.Payout>0 then c.BonusW else c.BonusL end as Bonus
{3}
) x1
) x2 where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 會員單週投注

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtW_Select : jgrid.GridRequest<GameLog_BetAmtW_Select>
{
    protected override string init_defaultkey() { return "ACTime1"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime1"      ,"ACTime1"},
                {"ACTime2"      ,"ACTime2"},
                {"CorpID"       ,"CorpID"},
                {"GroupID"      ,"grp"},
                {"ParentID"     ,"ParentID"},
                {"ParentACNT"   ,"ParentACNT"},
                {"MemberID"     ,"MemberID"},
                {"ACNT"         ,"ACNT"},
                {"BetAmount"    ,"BetAmount"},
                {"BetAmountAct" ,"BetAmountAct"},
                {"Payout"       ,"Payout"},
            };
    }

    [JsonProperty]
    string ACNT;
    [JsonProperty]
    string ParentACNT;

    [ObjectInvoke, Permissions("log_betamtw", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_BetAmtW_Select command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@" from (
select dateadd(dd,(datediff(dd,0,ACTime)/7)*7,0) ACTime1, dateadd(dd,((datediff(dd,0,ACTime)/7)*7)+6,0) ACTime2, CorpID, MemberID, ACNT, ParentID, ParentACNT, sum(BetAmount) BetAmount, sum(BetAmountAct) BetAmountAct, sum(Payout) Payout
from GameLog_BetAmtDG a with(nolock)
group by dateadd(dd,(datediff(dd,0,ACTime)/7)*7,0), dateadd(dd,((datediff(dd,0,ACTime)/7)*7)+6,0), CorpID, MemberID, ACNT, ParentID, ParentACNT) a
left join Member b with(nolock) on a.MemberID=b.ID
left join MemberGroup c with(nolock) on b.CorpID=c.CorpID and b.GroupID=c.GroupID
");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "a.ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select *, CorpID*256+grp as GroupID from (
select row_number() over (order by {2}) as rowid, * from (
select a.*, b.GroupID as grp, b.Name 
{3}
) x1
) x2 where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 會員單月投注

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtM_Select : jgrid.GridRequest<GameLog_BetAmtM_Select>
{
    protected override string init_defaultkey() { return "ACMonth"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACMonth"      ,"ACMonth"},
                {"CorpID"       ,"CorpID"},
                {"GroupID"      ,"grp"},
                {"ParentID"     ,"ParentID"},
                {"ParentACNT"   ,"ParentACNT"},
                {"MemberID"     ,"MemberID"},
                {"ACNT"         ,"ACNT"},
                {"BetAmount"    ,"BetAmount"},
                {"BetAmountAct" ,"BetAmountAct"},
                {"Payout"       ,"Payout"},
            };
    }

    [JsonProperty]
    string ACNT;
    [JsonProperty]
    string ParentACNT;

    [ObjectInvoke, Permissions("log_betamtm", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_BetAmtM_Select command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@" from (
select dateadd(mm,datediff(mm,0,ACTime),0) ACMonth, CorpID, MemberID, ACNT, ParentID, ParentACNT, sum(BetAmount) BetAmount, sum(BetAmountAct) BetAmountAct, sum(Payout) Payout
from GameLog_BetAmtDG a with(nolock)
group by dateadd(mm,datediff(mm,0,ACTime),0), CorpID, MemberID, ACNT, ParentID, ParentACNT) a
left join Member b with(nolock) on a.MemberID=b.ID
left join MemberGroup c with(nolock) on b.CorpID=c.CorpID and b.GroupID=c.GroupID
");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "a.ACNT like '%{0}%'", (command.ACNT * text.ValidAsACNT).Remove("%"));
            sql_where(sql, ref cnt, "a.ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select *, CorpID*256+grp as GroupID from (
select row_number() over (order by {2}) as rowid, * from (
select a.*, b.GroupID as grp, b.Name 
{3}
) x1
) x2 where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 代理單日投注

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtAD_Select : jgrid.GridRequest<GameLog_BetAmtAD_Select>
{
    protected override string init_defaultkey() { return "ACTime"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime"       ,"ACTime"},
                {"CorpID"       ,"CorpID"},
                {"GroupID"      ,"grp"},
                {"ParentID"     ,"ParentID"},
                {"ParentACNT"   ,"ParentACNT"},
                {"BetAmount"    ,"BetAmount"},
                {"BetAmountAct" ,"BetAmountAct"},
                {"Payout"       ,"Payout"},
            };
    }

    [JsonProperty]
    string ParentACNT;

    [ObjectInvoke, Permissions("log_betamtad", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_BetAmtAD_Select command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@" from (
select ACTime, CorpID, ParentID, ParentACNT, sum(BetAmount) BetAmount, sum(BetAmountAct) BetAmountAct, sum(Payout) Payout
from GameLog_BetAmtDG a with(nolock)
group by ACTime, CorpID, ParentID, ParentACNT) a
");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "a.ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, *
{3}
) x2 where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 代理單週投注

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtAW_Select : jgrid.GridRequest<GameLog_BetAmtAW_Select>
{
    protected override string init_defaultkey() { return "ACTime1"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACTime1"      ,"ACTime1"},
                {"ACTime2"      ,"ACTime2"},
                {"CorpID"       ,"CorpID"},
                {"ParentID"     ,"ParentID"},
                {"ParentACNT"   ,"ParentACNT"},
                {"BetAmount"    ,"BetAmount"},
                {"BetAmountAct" ,"BetAmountAct"},
                {"Payout"       ,"Payout"},
            };
    }

    [JsonProperty]
    string ParentACNT;

    [ObjectInvoke, Permissions("log_betamtaw", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_BetAmtAW_Select command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@" from (
select dateadd(dd,(datediff(dd,0,ACTime)/7)*7,0) ACTime1, dateadd(dd,((datediff(dd,0,ACTime)/7)*7)+6,0) ACTime2, CorpID, ParentID, ParentACNT, sum(BetAmount) BetAmount, sum(BetAmountAct) BetAmountAct, sum(Payout) Payout
from GameLog_BetAmtDG a with(nolock)
group by dateadd(dd,(datediff(dd,0,ACTime)/7)*7,0), dateadd(dd,((datediff(dd,0,ACTime)/7)*7)+6,0), CorpID, ParentID, ParentACNT) a
");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "a.ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, *
{3}
) x2 where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion

#region 代理單月投注

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
class GameLog_BetAmtAM_Select : jgrid.GridRequest<GameLog_BetAmtAM_Select>
{
    protected override string init_defaultkey() { return "ACMonth"; }
    protected override Dictionary<string, string> init_sortkeys()
    {
        return new Dictionary<string, string>()
            {
                {"ACMonth"      ,"ACMonth"},
                {"CorpID"       ,"CorpID"},
                {"ParentID"     ,"ParentID"},
                {"ParentACNT"   ,"ParentACNT"},
                {"BetAmount"    ,"BetAmount"},
                {"BetAmountAct" ,"BetAmountAct"},
                {"Payout"       ,"Payout"},
            };
    }

    [JsonProperty]
    string ParentACNT;

    [ObjectInvoke, Permissions("log_betamtam", Permissions.Flag.Read)]
    static jgrid.GridResponse<Dictionary<string, object>> execute(GameLog_BetAmtAM_Select command, string json_s, params object[] args)
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.Read))
        {
            jgrid.GridResponse<Dictionary<string, object>> data = new jgrid.GridResponse<Dictionary<string, object>>();
            StringBuilder sql = new StringBuilder(@" from (
select dateadd(mm,datediff(mm,0,ACTime),0) ACMonth, CorpID, ParentID, ParentACNT, sum(BetAmount) BetAmount, sum(BetAmountAct) BetAmountAct, sum(Payout) Payout
from GameLog_BetAmtDG a with(nolock)
group by dateadd(mm,datediff(mm,0,ACTime),0), CorpID, ParentID, ParentACNT) a
");

            int cnt = 0;
            sql_where_CorpID(sql, ref cnt, "a.CorpID={0}", command.CorpID);
            sql_where(sql, ref cnt, "a.ParentACNT like '%{0}%'", (command.ParentACNT * text.ValidAsACNT).Remove("%"));

            data.total_rows = sqlcmd.ExecuteScalar<int>("select count(*) {0}", sql);
            data.page_size = command.page_size;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from (
select row_number() over (order by {2}) as rowid, *
{3}
) x2 where rowid>{0} and rowid<={1} order by rowid", command.rows_start, command.rows_end, command.GetOrderBy(), sql))
                data.rows.Add(r.ToObject<Dictionary<string, object>>());
            return data;
        }
    }
}

#endregion
