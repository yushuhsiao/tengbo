using BU;
using extAPI;
using extAPI.hg;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Tools;
using web;

namespace LogService
{
    partial class frmMain : Form
    {
        //hg_proc proc = new hg_proc();

        const double _default_Interval = 5000;
        public frmMain()
        {
            DateTime t1 = new DateTime(2014, 1, 1);
            InitializeComponent();
            this.notifyIcon1.Icon = this.Icon;
            //this.propertyGrid1.SelectedObject = this.proc;
            this.Interval = new config_Item(this, 0, "Interval", null) { Time = DateTime.Now, MinLen = _default_Interval, Reserved = 0, };
            this.hg_GameLog = new config_GameLog(this, GameID.HG, "GameLog", "HG_GameLog") { Time = t1 };
            this.ag_GameLog = new config_GameLog(this, GameID.AG_AG, "GameLog", "AG_GameLog") { Time = t1 };
            this.bb_GameLog = new config_GameLog(this, GameID.BBIN, "GameLog", "BBIN_GameLog") { Time = t1 };
            this.hg_BetAmtDG = new config_BetAmtDG(this, GameID.HG, "BetAmtDG", "HG_BetAmtDG") { Time = t1, TimeFormat = "yyyy/MM/dd" };
            this.ag_BetAmtDG = new config_BetAmtDG(this, GameID.AG_AG, "BetAmtDG", "AG_BetAmtDG") { Time = t1, TimeFormat = "yyyy/MM/dd" };
            this.bb_BetAmtDG = new config_BetAmtDG(this, GameID.BBIN, "BetAmtDG", "BBIN_BetAmtDG") { Time = t1, TimeFormat = "yyyy/MM/dd" };
            foreach (config n in this.items) n.Update();
            this.hg_Betinfo1 = new config_Item(this, BU.GameID.HG, "Betinfo1", "hg_Betinfo1") { Time = new DateTime(2013, 10, 1), MinLen = 300, MaxLen = 1800, Reserved = 300, };
            this.hg_Betinfo2 = new config_Item(this, BU.GameID.HG, "Betinfo2", "hg_Betinfo2") { Time = new DateTime(2013, 10, 1), MinLen = 7200, MaxLen = 0, Reserved = 0, };
            this.hg_GameResult = new config_Item(this, BU.GameID.HG, "GameResult", "hg_GameResult") { Time = new DateTime(2013, 10, 1), MinLen = 1800, Reserved = 180, };
            this.hg_Transfer = new config_Item(this, BU.GameID.HG, "Transfer", "hg_Transfer") { Time = new DateTime(2013, 10, 1), MinLen = 300, MaxLen = 1800, Reserved = 300, };
            this.ag_getdata = new config_Item(this, BU.GameID.AG_AG, "getdata", "ag_ftp") { Time = new DateTime(2013, 10, 1), MinLen = 300, Reserved = 180, };
            this.ag_parsedata = new config_GameLog(this, GameID.AG_AG, "parsedata", "ag_parse") { Time = t1 };
            config_bbin bbin_ = new config_bbin(this, bbin.gamekind.ball, null);
            new config_bbin(this, bbin.gamekind.Lottery, null);
            new config_bbin(this, bbin.gamekind.live, null);
            new config_bbin(this, bbin.gamekind.game, null);
            new config_bbin(this, bbin.gamekind.ThreeD, null);
            foreach (bbin.gametype_彩票 t in Enum.GetValues(typeof(bbin.gametype_彩票)))
                new config_bbin(this, bbin.gamekind.ltlottery, (bbin.gametype)t);
            ThreadPool.QueueUserWorkItem(init);
        }

        List<config> items = new List<config>();
        List<config_bbin> items_bbin = new List<config_bbin>();

        readonly config hg_GameLog;
        readonly config ag_GameLog;
        readonly config bb_GameLog;
        readonly config hg_BetAmtDG;
        readonly config ag_BetAmtDG;
        readonly config bb_BetAmtDG;

        readonly config Interval;
        readonly config ag_getdata;
        readonly config ag_parsedata;
        readonly config hg_Betinfo1;
        readonly config hg_Betinfo2;
        readonly config hg_GameResult;
        readonly config hg_Transfer;

        DateTime SqlReadTime
        {
            get { return (DateTime)Interlocked.CompareExchange(ref this.m_SqlReadTime, null, null); }
            set { Interlocked.Exchange(ref this.m_SqlReadTime, value); }
        }
        object m_SqlReadTime = DateTime.MinValue;

        #region SqlCmd

        Dictionary<Thread, Dictionary<DB.Name, SqlCmd>> _sqlcmd = new Dictionary<Thread, Dictionary<DB.Name, SqlCmd>>();
        [DebuggerStepThrough]
        SqlCmd getSqlCmd(DB.Name name)
        {
            lock (this._sqlcmd)
            {
                Dictionary<DB.Name, SqlCmd> dict;
                if (!this._sqlcmd.TryGetValue(Thread.CurrentThread, out dict))
                    dict = this._sqlcmd[Thread.CurrentThread] = new Dictionary<DB.Name, SqlCmd>();
                SqlCmd sqlcmd;
                if (!dict.TryGetValue(name, out sqlcmd))
                    sqlcmd = dict[name] = DB.Open(name, DB.Access.ReadWrite);
                return sqlcmd;
            }
        }
        void sqlcmd_release()
        {
            lock (this._sqlcmd)
            {
                Dictionary<DB.Name, SqlCmd> dict;
                if (this._sqlcmd.TryGetValue(Thread.CurrentThread, out dict))
                {
                    int cnt = 0;
                    foreach (SqlCmd sqlcmd in dict.Values)
                        using (sqlcmd)
                            cnt++;
                    dict.Clear();
                }
            }
        }
        SqlCmd GameDB { [DebuggerStepThrough] get { return getSqlCmd(DB.Name.Main); } }
        SqlCmd LogDB { [DebuggerStepThrough] get { return getSqlCmd(DB.Name.Log); } }

        #endregion

        void init(object state)
        {
            using (SqlCmd logDB = DB.Open(DB.Name.Log, DB.Access.Read))
            {
                //logDB.ExecuteNonQueryT("update config set Time='2014-04-20' where GameID=11 and [Key]='BetAmtDG'");
                foreach (config item in this.items)
                    item.CurrentTime = null;
                //this.ag_getdata.Time = new DateTime(2014, 2, 28);
                //logDB.ExecuteNonQueryT("update config set Time={Time:yyyy/MM/dd HH:mm:ss} where GameID={GameID} and [Key]={Key}".SqlExport(null, this.ag_getdata));
                foreach (SqlDataReader r in logDB.ExecuteReader2("select getdate() as ct, * from config nolock"))
                {
                    GameID _gameID = (GameID)r.GetInt32("GameID");
                    string _key = r.GetString("Key");
                    config item = null;
                    foreach (config _item in this.items)
                    {
                        if ((_item.GameID == _gameID) && (_item.Key == _key))
                        {
                            item = _item;
                            break;
                        }
                    }
                    if (item == null)
                    {
                        if (_gameID == GameID.BBIN)
                        {
                            bbin.gamekind? gamekind = _key.Substring(0, 2).ToEnum<bbin.gamekind>();
                            bbin.gametype? gametype = _key.Substring(3).ToEnum<bbin.gametype>();
                            if (gamekind.HasValue)
                                item = new config_bbin(this, gamekind.Value, gametype);
                            else
                                continue;
                        }
                        else
                            item = new config_Item(this, _gameID, _key, null);
                    }
                    foreach (config item_ in item.SyncLock())
                        r.FillObject(item);
                }
                foreach (config item in this.items)
                {
                    if (item.CurrentTime.HasValue)
                        item.CurrentTime = null;
                    else
                    {
                        try
                        {
                            logDB.FillObject(true, item, @"insert into config (GameID,[Key],Active,[Time],MinLen,MaxLen,Reserved)
values ({GameID},{Key},{Active},{Time:yyyy/MM/dd HH:mm:ss},{MinLen},{MaxLen},{Reserved}) 
select * from config nolock where GameID={GameID} and [Key]={Key} ".SqlExport(null, item));
                        }
                        catch { }
                    }
                }
                this.SqlReadTime = DateTime.Now;
                log.message(null, "init complete.");
                if (this.Interval.Active == Locked.Active)
                    Tick.OnTick += tick;
            }
        }
        bool tick()
        {
            this.Interval.Time = DateTime.Now;
            try
            {
                foreach (config n in this.Interval.SyncLock())
                {
                    DateTime t1 = DateTime.Now;
                    double minlen = Math.Max(n.MinLen, 5000);
                    DateTime t2 = this.SqlReadTime.AddMilliseconds(Math.Max(n.MinLen, _default_Interval));
                    TimeSpan t = t2 - t1;
                    if (t < TimeSpan.Zero)
                    {
                        n.State = "*";
                        try
                        {
                            // reload config
                            this.SqlReadTime = DateTime.MaxValue;
                            n.CurrentTime = null;
                            foreach (SqlDataReader r in this.LogDB.ExecuteReader2("select getdate() ct,* from config nolock"))
                            {
                                GameID gameID = (GameID)r.GetInt32("GameID");
                                string key = r.GetString("Key");
                                foreach (config _item1 in this.items)
                                    if ((_item1.GameID == gameID) && (_item1.Key == key))
                                        foreach (config _item2 in _item1.SyncLock())
                                            r.FillObject(_item1);
                            }
                            // 過帳
                            if (n.CurrentTime.HasValue)
                            {
                                this.hg_GameLog.execute();
                                this.ag_GameLog.execute();
                                this.bb_GameLog.execute();
                                this.hg_BetAmtDG.execute();
                                this.ag_BetAmtDG.execute();
                                this.bb_BetAmtDG.execute();
                            }
                        }
                        finally
                        {
                            this.SqlReadTime = DateTime.Now;
                        }
                    }
                    else
                    {
                        n.State = t;
                    }
                }
                //return true;
                #region execute
                foreach (config hg1 in this.hg_Betinfo1.SyncLock())
                    hg1.execute();
                foreach (config hg2 in this.hg_Betinfo2.SyncLock())
                    foreach (config hg3 in this.hg_GameResult.SyncLock())
                        foreach (config hg4 in this.hg_Transfer.SyncLock())
                        {
                            hg2.execute();
                            hg3.execute();
                            hg4.execute();
                        }
                foreach (config ag1 in this.ag_getdata.SyncLock())
                    ag1.execute();
                if (Monitor.TryEnter(this.items_bbin))
                {
                    try
                    {
                        foreach (config n in this.items_bbin)
                            n.execute();
                    }
                    finally { Monitor.Exit(this.items_bbin); }
                }
                #endregion
                return true;
            }
            finally
            {
                this.sqlcmd_release();
            }
        }



        Dictionary<GameID, Dictionary<string, MemberRow>> members = new Dictionary<GameID, Dictionary<string, MemberRow>>();
        MemberRow GetMember(GameID gameID, string acnt)
        {
            MemberGame game = MemberGame.GetInstance(gameID);
            if (game == null) return null;
            lock (members)
            {
                Dictionary<string, MemberRow> list;
                if (!members.TryGetValue(game.GameID, out  list))
                    list = members[game.GameID] = new Dictionary<string, MemberRow>();
                MemberRow row;
                if (!list.TryGetValue(acnt, out row))
                {
                    row = list[acnt] = this.GameDB.ToObject<MemberRow>(@"select b.ID as ID,b.CorpID,b.ACNT,c.ID as ParentID,c.ACNT as ParentACNT
from {0} a with(nolock)
left join Member b with(nolock) on a.MemberID=b.ID
left join Agent c with(nolock) on b.ParentID=c.ID
where a.GameID={1} and a.ACNT='{2}'", game.TableName, (int)game.GameID, acnt);
                }
                return row;
            }
        }

        static string[] hg_update_fields = new string[] { "UserID", "TableName", "BetAmountAct", "BetAmount" };
        [ObjectInvoke("HG_GameLog")]
        int _hg_GameLog(config_GameLog item)
        {
            string sql = @"declare @t1 datetime, @t2 datetime select @t1={Time:yyyy-MM-dd HH:mm:ss}, @t2={CurrentTime:yyyy-MM-dd HH:mm:ss}
select 1 as _type, sn, dateadd(hh,8,BetStartDate) BetStartTime,dateadd(hh,8,BetEndDate) BetEndTime,AccountId,                          TableId,TableName,GameId,BetId,BetAmount,             Payout,Currency,GameType,BetSpot,BetNo FROM hg_Betinfo1 nolock where CreateTime>=@t1 and CreateTime<@t2
select 2 as _type, sn, dateadd(hh,8,BetStartDate) BetStartTime,dateadd(hh,8,BetEndDate) BetEndTime,AccountId,[user_id] UserID,table_id TableId,          GameId,BetId,BetAmount BetAmountAct,Payout,Currency,GameType,BetSpot,BetNo FROM hg_Betinfo2 nolock where CreateTime>=@t1 and CreateTime<@t2".SqlExport(null, item);
            //select 1 as src, * from hg_Betinfo1 nolock where CreateTime>=@t1 and CreateTime<@t2
            //select 2 as src, * from hg_Betinfo2 nolock where CreateTime>=@t1 and CreateTime<@t2
            int count = 0;
            foreach (SqlDataReader r1 in this.LogDB.ExecuteReader2(sql))
            {
                MemberRow member = GetMember(GameID.HG, r1.GetString("AccountId"));
                if (member == null) continue;
                long? rowid = this.GameDB.ExecuteScalar<long>("select ID from GameLog_001 nolock where MemberID={0} and BetId='{1}'", member.ID, r1.GetString("BetId"));
                int _type = r1.GetInt32("_type");
                sqltool s = new sqltool();
                DateTime? actime = null;
                for (int i = 0; i < r1.FieldCount; i++)
                {
                    if (r1.IsDBNull(i)) continue;
                    string name = r1.GetName(i);
                    object value = r1.GetValue(i);
                    switch (name)
                    {
                        case "BetStartTime": actime = r1.GetDateTime(i).ToACTime(); break;
                        case "_type":
                        case "sn": continue;
                        //case "src":
                        //case "xml":
                        //case "CreateTime":
                        //    continue;
                        //case "BetStartDate": name = "BetStartTime"; value = betStartDate = r1.GetDateTime(i).AddHours(8); break;
                        //case "BetEndDate": name = "BetEndTime"; value = r1.GetDateTime(i).AddHours(8); break;
                        //case "BetAmount": if (src == 2) name = "BetAmountAct"; break;
                        //case "user_id": name = "UserID"; break;
                        //case "table_id": name = "TableId"; break;
                    }
                    s["", name, ""] = value;
                }
                s.values["ID"] = rowid;
                if (rowid.HasValue)
                {
                    this.GameDB.ExecuteNonQuery(s.BuildEx("update GameLog_001 set ", _type == 1 ? "BetAmount={BetAmount},TableName={TableName}" : "BetAmountAct={BetAmountAct},UserID={UserID}", " where ID={ID}"));
                }
                else
                {
                    if (!actime.HasValue) continue;
                    s["", "MemberID", "  "] = member.ID;
                    s["", "CorpID", "    "] = member.CorpID;
                    s["", "ACNT", "      "] = member.ACNT;
                    s["", "ParentID", "  "] = member.ParentID;
                    s["", "ParentACNT", ""] = member.ParentACNT;
                    s["", "CorpID", "    "] = member.CorpID;
                    s["", "ACTime", "    "] = actime.Value.ToString("yyyy-MM-dd");
                    this.GameDB.ExecuteNonQuery(true, s.BuildEx(@"insert into GameLog_001 (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                }
                count++;
                item.State = count.ToString();
            }
            return count;
        }

        bool ag_TryGetGameID(string platformType, out GameID gameID)
        {
            if (platformType == "AG") gameID = GameID.AG_AG;
            else if (platformType == "AGIN") gameID = GameID.AG_AGIN;
            else if (platformType == "DSP") gameID = GameID.AG_DSP;
            else
            {
                gameID = default(GameID);
                return false;
            }
            return true;
        }

        [ObjectInvoke("AG_GameLog")]
        int _ag_GameLog(config_GameLog item)
        {
            int count=0;
            foreach (SqlDataReader r1 in this.LogDB.ExecuteReader2(@"select * from ag_BetDetail nolock where CreateTime>={Time:yyyy-MM-dd HH:mm:ss} and CreateTime<{CurrentTime:yyyy-MM-dd HH:mm:ss}".SqlExport(null, item)))
            {
                GameID gameID;
                if (!ag_TryGetGameID(r1.GetStringN("platformType"), out gameID)) continue;

                MemberRow member = GetMember(gameID, r1.GetString("playerName"));
                if (member == null)
                    continue;

                GameLogRow_AG gb = this.GameDB.ToObject<GameLogRow_AG>("select * from GameLog_011 nolock where MemberID = {0} and billNo = {1}", member.ID, r1.GetInt64("billNo"));

                if (gb == null)
                #region 当这笔注单不存在 (Insert)
                {
                    sqltool s = new sqltool();
                    for (int i = 0; i < r1.FieldCount; i++)
                    {
                        if (r1.IsDBNull(i))
                            continue;

                        string name = r1.GetName(i);
                        object value = r1.GetValue(i);
                        switch (name)
                        {
                            case "CreateTime":
                            case "xml":
                            case "sn":
                                continue;
                        }
                        s["", name, ""] = value;
                    }

                    s["", "GameID", "    "] = (int)gameID;
                    s["", "MemberID", "  "] = member.ID;
                    s["", "CorpID", "    "] = member.CorpID;
                    s["", "ACNT", "      "] = member.ACNT;
                    s["", "ParentID", "  "] = member.ParentID;
                    s["", "ParentACNT", ""] = member.ParentACNT;
                    s["", "CorpID", "    "] = member.CorpID;
                    //投注时间
                    s["", "ACTime", "   "] = r1.GetDateTime("betTime").ToACTime().ToString("yyyy-MM-dd");
                    this.GameDB.ExecuteNonQuery(true, s.BuildEx(@"insert into GameLog_011 (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                }
                #endregion
                else
                #region 当这笔注单已经存在 (Update)
                {
                    //当前注单
                    Decimal? curNetAmount = r1.GetDecimalN("netAmount");
                    DateTime curRecalcuTime = r1.GetDateTime("recalcuTime");
                    string curRound = r1.GetStringN("round");

                    sqltool s = new sqltool();
                    //输赢额度相同 其他列相同 取第一次派彩的注单
                    if ((curNetAmount == gb.netAmount) && (curRecalcuTime < gb.recalcuTime))
                    {
                        s["", "recalcuTime", ""] = curRecalcuTime;
                    }
                    //输赢额度不同 取派彩最后一笔 
                    else if ((curNetAmount != gb.netAmount) && (curRecalcuTime > gb.recalcuTime))
                    {
                        s["", "netAmount", ""] = curNetAmount;
                        s["", "recalcuTime", ""] = curRecalcuTime;
                    }
                    //当round不为空
                    if (gb.round == null) s["", "round", ""] = curRound;
                    if (s.fields.Count == 0) continue;
                    s.values["ID"] = gb.ID;
                    this.GameDB.ExecuteNonQuery(true, s.BuildEx("update GameLog_011 set ", sqltool._FieldValue, " where ID={ID}"));
                }
                #endregion
                count++;
                item.State = count.ToString();
            }
            return count;
            #region //
            //string sql = @"select * from ag_BetDetail nolock where CreateTime>={Time:yyyy-MM-dd HH:mm:ss} and CreateTime<{CurrentTime:yyyy-MM-dd HH:mm:ss}".SqlExport(null, item);
            //int count = 0;

            //foreach (SqlDataReader r1 in this.LogDB.ExecuteReader2(sql))
            //{
            //    GameID gameID;
            //    string platformType = r1["platformType"] as string;
            //    if (string.IsNullOrEmpty(platformType))
            //    {
            //        continue;
            //    }
            //    else if (platformType == "AG")
            //    {
            //        gameID = GameID.AG_AG;
            //    }
            //    else if (platformType == "AGIN")
            //    {
            //        gameID = GameID.AG_AGIN;
            //    }
            //    else if (platformType == "DSP")
            //    {
            //        gameID = GameID.AG_DSP;
            //    }
            //    else
            //    {
            //        continue;
            //    }

            //    MemberRow member = GetMember(gameID, r1.GetString("playerName"));
            //    if (member == null)
            //    {
            //        continue;
            //    }

            //    sqltool s = new sqltool();
            //    for (int i = 0; i < r1.FieldCount; i++)
            //    {
            //        if (r1.IsDBNull(i))
            //        {
            //            continue;
            //        }

            //        string name = r1.GetName(i);
            //        object value = r1.GetValue(i);
            //        switch (name)
            //        {
            //            case "CreateTime":
            //            case "xml":
            //            case "sn":
            //                continue;
            //        }
            //        s["", name, ""] = value;
            //    }

            //    s["", "GameID", " "] = (int)gameID;

            //    s["", "MemberID", " "] = member.ID;
            //    s["", "CorpID", "   "] = member.CorpID;
            //    s["", "ACNT", "     "] = member.ACNT;
            //    s["", "ParentID", "  "] = member.ParentID;
            //    s["", "ParentACNT", ""] = member.ParentACNT;
            //    s["", "CorpID", "   "] = member.CorpID;
            //    //投注时间
            //    s["", "ACTime", "   "] = r1.GetDateTime("betTime").ToACTime().ToString("yyyy-MM-dd");

            //    string sql_ = string.Format("select * from GameLog_011 nolock where MemberID = {0} and billNo = {1}", member.ID, Convert.ToInt64(r1["billNo"].ToString()));
            //    GameLog_011 gb = this.GameDB.ToObject<GameLog_011>(sql_);

            //    //当这笔注单已经存在
            //    if (gb != null)
            //    {
            //        int ID = Convert.ToInt32(gb.ID);

            //        Decimal? netAmount = gb.netAmount;          //输赢额度
            //        DateTime recalcuTime = gb.recalcuTime;      //派彩时间
            //        string round = gb.round;                    //无名列

            //        //当前注单
            //        Decimal? curNetAmount = Convert.ToDecimal(r1["netAmount"].ToString());
            //        DateTime curRecalcuTime = Convert.ToDateTime(r1["recalcuTime"].ToString());
            //        string curRound = r1["round"].ToString();

            //        bool exp1 = curRound != null && round == null;
            //        string rem1 = exp1 ? null : "/*", rem2 = exp1 ? null : "*/";

            //        //输赢额度相同 其他列相同 取第一次派彩的注单
            //        if (curNetAmount == netAmount && curRecalcuTime < recalcuTime)
            //        {
            //            this.GameDB.ExecuteNonQueryT("Update GameLog_011 Set recalcuTime ='{0:yyyy-MM-dd HH:mm:ss}' {1}, round = '{2}' {3} where ID = {4}", curRecalcuTime, rem1, curRound, rem2, gb.ID);
            //        }
            //        //输赢额度不同 取派彩最后一笔 
            //        else if (curNetAmount != netAmount && curRecalcuTime > recalcuTime)
            //        {
            //            this.GameDB.ExecuteNonQueryT("Update GameLog_011 Set netAmount = {0}, recalcuTime = '{1:yyyy-MM-dd HH:mm:ss}' {2}, round = '{3}' {4} where ID = {5}", curNetAmount, curRecalcuTime, rem1, curRound, rem2, gb.ID);
            //        }
            //        //当round不为空
            //        else if (exp1)
            //        {
            //            this.GameDB.ExecuteNonQueryT("Update GameLog_011 Set round = '{0}' where ID = {1}", curRound, gb.ID);
            //        }
            //    }
            //    //当这笔注单不存在
            //    else if (gb == null)
            //    {
            //        string sql_Insert = s.BuildEx(@"insert into GameLog_011 (", sqltool._Fields, ") values (", sqltool._Values, ")");
            //        this.GameDB.ExecuteNonQueryT(sql_Insert);
            //    }
            //    count++;
            //}
            //return count;
            #endregion
        }

        [ObjectInvoke("BBIN_GameLog")]
        int _bb_GameLog(config_GameLog item)
        {
            int count = 0;
            // gamekind = 12 and 
            foreach (SqlDataReader r1 in this.LogDB.ExecuteReader2(@"select * from bbin_BetRecord nolock where CreateTime>={Time:yyyy-MM-dd HH:mm:ss} and CreateTime<{CurrentTime:yyyy-MM-dd HH:mm:ss}".SqlExport(null, item)))
            {
                MemberRow member = GetMember(GameID.BBIN, r1.GetString("UserName"));
                if (member != null)
                {
                    try
                    {
                        GameLogRow_BBIN gb = this.GameDB.ToObject<GameLogRow_BBIN>("select * from GameLog_009 nolock where MemberID={0} and WagersID={1}", member.ID, r1.GetInt64("WagersID"));
                        bbin.gamekind gamekind = (bbin.gamekind)r1.GetInt32("gamekind");

                        sqltool s = new sqltool();
                        if (gb != null)
                        #region 当这笔注单存在
                        {
                            //当前注单的gamekind
                            bool replace = false;
                            string result = r1.GetString("Result");

                            //视讯游戏  当这笔注单为派彩前的注单
                            //if ((gamekind == bbin.gamekind.live) && (!string.IsNullOrEmpty(result) && (string.IsNullOrEmpty(gb.Result) || (gb.Result == ",,,"))) && (gb.Commissionable == 0))
                            //if (gamekind == bbin.gamekind.live && (gb.Result == "" || gb.Result == ",,," || (gb.Result != "" && gb.Result != ",,," && gb.Commissionable == 0)))
                            if (gamekind == bbin.gamekind.live)
                            {
                                if (!string.IsNullOrEmpty(result) && (result != ",,,"))
                                {
                                    s["", "Result", ""] = result;
                                    s["", "Card", ""] = r1.GetStringN("Card");
                                    s["", "BetAmount", ""] = r1.GetDecimalN("BetAmount");
                                    s["", "Payoff", ""] = r1.GetDecimalN("Payoff");
                                    s["", "Commissionable", ""] = r1.GetDecimalN("Commissionable");
                                }
                            }
                            //彩票游戏
                            else if (gamekind == bbin.gamekind.ltlottery)
                            {
                                if (result != "0")
                                {
                                    //更新原有注单注单Result Payoff IsPaid Commission
                                    s["", "Result", ""] = result;
                                    s["", "Payoff", ""] = r1.GetDecimalN("Payoff");
                                    s["", "IsPaid", ""] = r1.GetStringN("IsPaid");
                                    s["", "f_sum", " "] = 1;
                                }
                                s["", "Commissionable", ""] = r1.GetDecimalN("BetAmount");
                            }
                            //機率游戏
                            else if (gamekind == bbin.gamekind.game)
                            {
                                if (result != "0")
                                {
                                    //更新原有注单注单Result Payoff
                                    s["", "Result", ""] = result;
                                    s["", "Payoff", ""] = r1.GetDecimalN("Payoff");
                                }
                            }
                            //3D  Ball 暂无重复资料

                            if (replace)
                            {
                                this.GameDB.ExecuteNonQuery(true, "delete GameLog_009 where MemberID={0} and WagersID={1}", member.ID, r1.GetInt64("WagersID"));
                            }
                            else
                            {
                                if (s.fields.Count == 0) continue;
                                s.values["ID"] = gb.ID;
                                this.GameDB.ExecuteNonQuery(s.BuildEx("update GameLog_009 set ", sqltool._FieldValue, " where ID={ID}"));
                                continue;
                            }
                        }
                        #endregion
                        #region 当这笔注单不存在或是需要替換資料
                        for (int i = 0; i < r1.FieldCount; i++)
                        {
                            if (r1.IsDBNull(i))
                                continue;

                            string name = r1.GetName(i);
                            object value = r1.GetValue(i);

                            switch (name)
                            {
                                case "CreateTime":
                                case "json":
                                case "sn":
                                    continue;
                                case "Commissionable":
                                    if (gamekind == bbin.gamekind.ltlottery)
                                        value = r1.GetDecimal("BetAmount");
                                    break;
                                case "WagersDate":
                                    DateTime wagersDate = r1.GetDateTime(i).AddHours(12);
                                    value = wagersDate;
                                    s["", "ACTime", "   "] = wagersDate.ToACTime().ToString("yyyy-MM-dd");
                                    break;

                                case "GameType":
                                    string gameType_s = r1.GetString(i);
                                    s["", "GameTypei", ""] = gameType_s.ToInt32() ?? (int?)gameType_s.ToEnum<extAPI.bbin.gametype>();
                                    break;

                                case "IsPaid":
                                    if ((gamekind == bbin.gamekind.ltlottery) && (r1.GetString("Result") == "0"))
                                        s["", "f_sum", ""] = 0;
                                    break;
                            }
                            s["", name, ""] = value;
                        }

                        s["", "MemberID", " "] = member.ID;
                        s["", "CorpID", "   "] = member.CorpID;
                        s["", "ACNT", "     "] = member.ACNT;
                        s["", "ParentID", "  "] = member.ParentID;
                        s["", "ParentACNT", ""] = member.ParentACNT;
                        s["", "CorpID", "   "] = member.CorpID;
                        this.GameDB.ExecuteNonQuery(true, s.BuildEx(@"insert into GameLog_009 (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                        #endregion
                    }
                    finally
                    {
                        count++;
                        item.State = count.ToString();
                    }
                }
            }
            return count;
            #region //
            //string sql = @"select * from bbin_BetRecord nolock where CreateTime>={Time:yyyy-MM-dd HH:mm:ss} and CreateTime<{CurrentTime:yyyy-MM-dd HH:mm:ss}".SqlExport(null, item);
            //int count = 0;

            //foreach (SqlDataReader r1 in this.LogDB.ExecuteReader2(sql))
            //{
            //    MemberRow member = GetMember(GameID.BBIN, r1.GetString("UserName"));
            //    if (member == null)
            //    {
            //        continue;
            //    }

            //    sqltool s = new sqltool();
            //    DateTime? wagersDate = null;
            //    for (int i = 0; i < r1.FieldCount; i++)
            //    {
            //        if (r1.IsDBNull(i))
            //        {
            //            continue;
            //        }

            //        string name = r1.GetName(i);
            //        object value = r1.GetValue(i);
            //        switch (name)
            //        {
            //            case "CreateTime":
            //            case "json":
            //            case "sn":
            //                continue;
            //            case "WagersDate": value = wagersDate = r1.GetDateTime(i).AddHours(12); break;
            //        }
            //        s["", name, ""] = value;
            //    }

            //    try
            //    {
            //        extAPI.bbin.gametype GameTypei = (extAPI.bbin.gametype)Enum.Parse(typeof(extAPI.bbin.gametype), r1["GameType"].ToString());
            //        s["", "GameTypei", " "] = (int)GameTypei;
            //    }
            //    catch (Exception ex)
            //    {
            //        s["", "GameTypei", " "] = null;
            //    }

            //    s["", "MemberID", " "] = member.ID;
            //    s["", "CorpID", "   "] = member.CorpID;
            //    s["", "ACNT", "     "] = member.ACNT;
            //    s["", "ParentID", "  "] = member.ParentID;
            //    s["", "ParentACNT", ""] = member.ParentACNT;
            //    s["", "CorpID", "   "] = member.CorpID;
            //    s["", "ACTime", "   "] = wagersDate.Value.ToACTime().ToString("yyyy-MM-dd");

            //    string sql_ = string.Format("select * from GameLog_009 nolock where MemberID={0} and WagersID={1}", member.ID, Convert.ToInt64(r1["WagersID"].ToString()));
            //    GameLog_009 gb = this.GameDB.ToObject<GameLog_009>(sql_);

            //    //当这笔注单已经存在
            //    if (gb != null)
            //    {
            //        int ID = Convert.ToInt32(gb.ID);
            //        string Result = gb.Result.ToString();
            //        string Commissionable = gb.Commissionable.ToString();
            //        //string Commission = gb.Commission.ToString();
            //        //string IsPaid = gb.IsPaid.ToString();

            //        //当前注单的gamekind
            //        int gamekind = Convert.ToInt32(r1["gamekind"].ToString());

            //        //视讯游戏
            //        if (gamekind == 3)
            //        {
            //            //当这笔注单为派彩前的注单
            //            if (Result == "" || Result == ",,," || (Result != "" && Result != ",,," && Commissionable == "0"))
            //            {
            //                //更新原有注单注单Result Card Payoff Commissionable BetAmount
            //                string sql_Update = string.Format("update GameLog_009 set Result = '{0}', Card = '{1}', BetAmount = {2}, Payoff = {3}, Commissionable = {4} where ID = {5}",
            //                    r1["Result"].ToString(), r1["Card"].ToString(), Convert.ToDecimal(r1["BetAmount"].ToString()), Convert.ToDecimal(r1["Payoff"].ToString()), Convert.ToDecimal(r1["Commissionable"].ToString()), ID);
            //                this.GameDB.ExecuteNonQueryT(sql_Update);
            //            }
            //            //else 当这笔注单为派彩后的注单 则不做任何的操作
            //        }
            //        //彩票游戏
            //        else if (gamekind == 12)
            //        {
            //            if (Result == "0")
            //            {
            //                //更新原有注单注单Result Payoff IsPaid Commission
            //                string sql_Update = string.Format("update GameLog_009 set Result = '{0}', Payoff = {1}, IsPaid = '{2}', Commission = {3} where ID = {4}",
            //                    r1["Result"].ToString(), Convert.ToDecimal(r1["Payoff"].ToString()), r1["IsPaid"].ToString(), Convert.ToDecimal(r1["Commission"].ToString()), ID);
            //                this.GameDB.ExecuteNonQueryT(sql_Update);
            //            }
            //            //else 当这笔注单为派彩后的注单 则不做任何的操作
            //        }
            //        //機率游戏
            //        else if (gamekind == 5)
            //        {
            //            if (Result == "0")
            //            {
            //                //更新原有注单注单Result Payoff
            //                string sql_Update = string.Format("update GameLog_009 set Result = '{0}', Payoff = {1} where ID = {2}",
            //                    r1["Result"].ToString(), Convert.ToDecimal(r1["Payoff"].ToString()), ID);
            //                this.GameDB.ExecuteNonQueryT(sql_Update);
            //            }
            //            //else 当这笔注单为派彩后的注单 则不做任何的操作
            //        }
            //    }
            //    //当这笔注单不存在
            //    else if (gb == null)
            //    {
            //        string sql_Insert = s.BuildEx(@"insert into GameLog_009 (", sqltool._Fields, ") values (", sqltool._Values, ")");
            //        this.GameDB.ExecuteNonQueryT(sql_Insert);
            //    }
            //    count++;
            //}
            //return count;
            #endregion
        }



        [ObjectInvoke("HG_BetAmtDG")]
        bool _hg_BetAmtDG(config_BetAmtDG item, DateTime time)
        {
            if (item.CheckTime_BetAmt(time, this.hg_Betinfo1) &&
                item.CheckTime_BetAmt(time, this.hg_Betinfo2))
            {
                this.GameDB.ExecuteNonQuery(true, @"exec BetAmtDG_hg '{0:yyyy/MM/dd}'", time);
                return true;
            }
            return false;
        }

        [ObjectInvoke("AG_BetAmtDG")]
        bool _ag_BetAmtDG(config_BetAmtDG item, DateTime time)
        {
            if (item.CheckTime_BetAmt(time, this.ag_getdata) &&
                item.CheckTime_BetAmt(time, this.ag_parsedata))
            {
                this.GameDB.ExecuteNonQuery(true, @"exec BetAmtDG_ag '{0:yyyy/MM/dd}'", time);
                return true;
            }
            return false;
        }

        [ObjectInvoke("BBIN_BetAmtDG")]
        bool _bb_BetAmtDG(config_BetAmtDG item, DateTime time)
        {
            foreach (config n1 in this.items_bbin)
                if (!item.CheckTime_BetAmt(time, this.ag_getdata))
                    return false;
            this.GameDB.ExecuteNonQuery(true, @"exec BetAmtDG_bbin '{0:yyyy/MM/dd}'", time);
            return true;
        }

        #region ag

        int _ag_WriteRow(XmlElement e, string tableName)
        {
            sqltool s = new sqltool();
            foreach (XmlAttribute a in e.Attributes)
                s["", a.Name, ""] = a.Value * text.ValidAsString;
            s.values["xml"] = e.OuterXml;
            try
            {
                return this.LogDB.ExecuteNonQuery(true, s.BuildEx(@"declare @xml varchar(850) set @xml={xml}
if exists (select sn from ", tableName, @" nolock where xml=@xml) return insert into ", tableName, " (", sqltool._Fields, ",xml) values (", sqltool._Values, ",@xml)"));
            }
            catch { return 0; }
        }

        [ObjectInvoke("ag_parse")]
        int _ag_parsedata(config_GameLog item)
        {
            using (SqlCmd logDB = DB.Open(DB.Name.Log, DB.Access.Read))
            {
                int count = 0;
                try
                {
                    foreach (SqlDataReader r in logDB.ExecuteReader2("select * from ag_ftp nolock where CreateTime>={Time:yyyy/MM/dd HH:mm:ss} and CreateTime<{CurrentTime:yyyy/MM/dd HH:mm:ss}".SqlExport(null, item)))
                    {
                        item.State = string.Format("count:{0}, {1}\\{2}", count, r.GetString("path"), r.GetString("name"));
                        SqlXml xml1 = r.GetSqlXml("xml");
                        XmlDocument xml2 = new XmlDocument();
                        using (XmlReader xml3 = xml1.CreateReader()) xml2.Load(xml3);
                        foreach (XmlElement e in xml2.SelectNodes("//betDetail/row"))
                            count += _ag_WriteRow(e, "ag_BetDetail");
                        foreach (XmlElement e in xml2.SelectNodes("//accountTransfer/row"))
                            count += _ag_WriteRow(e, "ag_AccountTransfer");
                    }
                    return count;
                }
                finally
                {
                    Thread.Sleep(3000);
                    item.State = "";
                }
            }
        }

        void ag_WriteRow(XmlElement e, string t)
        {
            //item.State = msg ?? "Write Data...";
            sqltool s = new sqltool();
            for (int i = 0; i < e.Attributes.Count; )
            {
                XmlAttribute a = e.Attributes[i];
                string value = a.Value * text.ValidAsString;
                if (string.IsNullOrEmpty(value))
                {
                    e.Attributes.RemoveAt(i);
                }
                else
                {
                    i++;
                    s["", a.Name, ""] = a.Value = value;
                }
            }
            string xml = e.OuterXml * text.ValidAsString;
            s.values["xml"] = xml;
            try
            {
                this.LogDB.ExecuteNonQuery(true, s.BuildEx(@"declare @xml varchar(850) set @xml={xml}
if not exists (select sn from ", t, @" nolock where xml=@xml)
insert into ", t, "(", sqltool._Fields, ",xml) values (", sqltool._Values, ",@xml)"));
            }
            catch (SqlException ex)
            {
                if ((ex.Class == 14) && (ex.Number == 2627) && (ex.State == 1)) { }
                else log.message("error", "{0}\r\n{1}", ex, this.LogDB.CommandText);
            }
            catch { }
        }

        void ag_WriteFile(string filepath, string filename, string xmlstr, DateTime filetime)
        {
            //XmlDocument xmldoc = new XmlDocument();
            //xmldoc.LoadXml(xmlstr);
            //try
            //{
            //    foreach (XmlElement node in xmldoc.SelectNodes("//result/betDetail/row"))
            //        ag_WriteRow(node, "ag_BetDetail");
            //    foreach (XmlElement node in xmldoc.SelectNodes("//result/accountTransfer/row"))
            //        ag_WriteRow(node, "ag_AccountTransfer");
            //}
            //catch { }
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@"if not exists (select * from ag_ftp nolock where [path]='{0}' and [name]='{1}')
insert into ag_ftp ([path],[name],[time],[xml]) values ('{0}', '{1}', '{2:yyyy/MM/dd HH:mm:ss}','", filepath, filename, filetime);
                int n = sql.Length;
                for (int s1 = 0; s1 < xmlstr.Length; s1++)
                {
                    sql.Append(xmlstr[s1]);
                    if (xmlstr[s1] == '\'')
                        sql.Append('\'');
                }
                sql.Append("')");
                this.LogDB.ExecuteNonQuery(true, sql.ToString());
            }
            catch { }
        }

        [AppSetting, DefaultValue("ag_ftp")]
        string ag_LocalPath
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        void _ag_getdata_ImportLocalFile(config_Item item)
        {
            DirectoryInfo dir1 = null;
            try
            {
                foreach (DirectoryInfo d1 in new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory).GetDirectories())
                {
                    if (d1.Name == this.ag_LocalPath)
                    {
                        dir1 = d1;
                        break;
                    }
                }
                if (dir1 == null)
                    return;
            }
            catch (Exception ex) { log.error_msg(ex); return; }

            foreach (DirectoryInfo dir2 in dir1.GetDirectories())
            {
                DateTime date;
                try
                {
                    int? yy = null, mm = null, dd = null;
                    if (dir2.Name.Length >= 4) yy = dir2.Name.Substring(0, 4).ToInt32();
                    if (dir2.Name.Length >= 6) mm = dir2.Name.Substring(4, 2).ToInt32();
                    if (dir2.Name.Length >= 8) dd = dir2.Name.Substring(6, 2).ToInt32();
                    if (yy.HasValue && mm.HasValue && dd.HasValue)
                        date = new DateTime(yy.Value, mm.Value, dd.Value);
                    else continue;
                }
                catch { continue; }

                foreach (FileInfo file in dir2.GetFiles())
                {
                    if (string.Compare(file.Extension, ".xml", true) != 0)
                        continue;
                    item.State = string.Format("Import {0}/{1}", file.Directory.Name, file.Name);
                    try
                    {
                        string xml1;
                        using (StreamReader xml2 = file.OpenText())
                            xml1 = xml2.ReadToEnd();
                        ag_WriteFile(file.Directory.Name, file.Name, xml1, file.LastWriteTime);
                        file.Delete();
                    }
                    catch { }
                }
                try { if (dir2.GetFiles().Length == 0) dir2.Delete(); }
                catch { }
            }
        }

        struct FtpFileInfo
        {
            public string Name;
            public bool IsDirectory;
            public long Length;
        }

        NetworkCredential ag_credential;

        FtpWebResponse ag_CreateRequest(ag api, string url, string method)
        {
            this.ag_credential = this.ag_credential ?? new NetworkCredential(api.ftp_user, api.ftp_password);
            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(url);
            ftp.Method = method;
            ftp.Credentials = this.ag_credential;
            return (FtpWebResponse)ftp.GetResponse();
        }

        bool _ag_getdata_DownloadFile(config_Item item)
        {
            ag api = extAPI.ag.AG.GetInstance(2);
            for (DateTime time = item.StartTime; ; time = item.EndTime)
            {
                string path = string.Format("{0:yyyyMMdd}", time);
                string url = string.Format("ftp://{0}/{1}", api.ftp_url, path);
                log.message("ag", "ls {0}", url);
                List<FtpFileInfo> files = new List<FtpFileInfo>();
                using (FtpWebResponse res1 = this.ag_CreateRequest(api, url, WebRequestMethods.Ftp.ListDirectoryDetails))
                {
                    string data_dir;
                    using (Stream s1 = res1.GetResponseStream())
                    using (StreamReader s2 = new StreamReader(s1))
                        data_dir = s2.ReadToEnd();
                    Regex regex = new Regex(@"^(?<FileOrDirectory>[d-])(?<Attributes>[rwxts-]{3}){3}\s+\d{1,}\s+.*?(?<FileSize>\d{1,})\s+(?<Date>\w+\s+\d{1,2}\s+(?:\d{4})?)(?<YearOrTime>\d{1,2}:\d{2})?\s+(?<Name>.+?)\s?$", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    MatchCollection matches = regex.Matches(data_dir);
                    foreach (Match match in matches)
                    {
                        FtpFileInfo info = new FtpFileInfo()
                        {
                            Name = match.Groups["Name"].Value,
                            IsDirectory = match.Groups["FileOrDirectory"].Value == "d",
                            Length = long.Parse(match.Groups["FileSize"].Value),
                        };
                        if ((info.Name == ".") || (info.Name == "..")) continue;
                        files.Add(info);
                    }
                }
                int cnt1 = files.Count;
                foreach (SqlDataReader r in this.LogDB.ExecuteReader2("select [name] from ag_ftp nolock where path='{0}'", path))
                {
                    string name = r.GetString("name");
                    for (int i = files.Count - 1; i >= 0; i--)
                        if (string.Compare(name, files[i].Name, true) == 0)
                            files.RemoveAt(i);
                }
                int cnt2 = files.Count;
                log.message("ag", "total file : {0}, {1} added.", cnt1, cnt2);
                foreach (FtpFileInfo file in files)
                {
                    string fileUrl = string.Format("{0}/{1}", url, file.Name);
                    log.message("ag", "get {0}", fileUrl);
                    DateTime lastModified;
                    using (FtpWebResponse res = ag_CreateRequest(api, fileUrl, WebRequestMethods.Ftp.GetDateTimestamp))
                        lastModified = res.LastModified;
                    string xml;
                    using (FtpWebResponse res = ag_CreateRequest(api, fileUrl, WebRequestMethods.Ftp.DownloadFile))
                    using (Stream s1 = res.GetResponseStream())
                    using (StreamReader s2 = new StreamReader(s1))
                        xml = s2.ReadToEnd();
                    ag_WriteFile(path, file.Name, xml, lastModified);
                }
                if (time.Date == item.EndTime.Date)
                    break;
            }
            return true;
        }


        [ObjectInvoke("ag_ftp")]
        bool _ag_getdata(config_Item item)
        {
            foreach (config ag2 in this.ag_parsedata.SyncLock())
            {
                try
                {
                    ag2.Active = Locked.Active;
                    _ag_getdata_ImportLocalFile(item);
                    return _ag_getdata_DownloadFile(item);
                }
                finally
                {
                    item.State = "Parse Data...";
                    ag2.execute();
                    item.State = "";
                }
            }
            return false;
        }

        #endregion

        #region bbin

        [ObjectInvoke("bbin_execute")]
        bool _bbin_execute(config_bbin item)
        {
            for (int page = 1; ; page++)
            {
                bbin.Request r = extAPI.bbin.GetInstance(2).BetRecord2(null, null, item.StartTime, item.EndTime, item.StartTime, item.EndTime, item._gamekind, item._gametype, page, null);
                if (r.result == true)
                {
                    item.State = "Write Data...";
                    foreach (JToken n in r.Response["data"].Children())
                    //for (JToken n = r.Response["data"].First; n != null; n = n.Next)
                    {
                        sqltool s = new sqltool();
                        s["", "gamekind", ""] = item._gamekind;
                        foreach (JToken nn in n.Children())
                        {
                            if (nn is JProperty)
                            {
                                JProperty p = (JProperty)nn;
                                switch (p.Name)
                                {
                                    case "BetAmount":
                                    case "Payoff":
                                    case "ExchangeRate":
                                    case "Commissionable":
                                    case "Commission":
                                        s["", p.Name, ""] = p.Value.ToString().ToDecimal();
                                        break;
                                    case "WagersDate":
                                        s["", p.Name, ""] = p.Value.ToString().ToDateTime();
                                        break;
                                    default:
                                        s["", p.Name, ""] = p.Value.ToString();
                                        break;
                                }
                            }
                        }
                        s.values["json"] = n.ToString(Newtonsoft.Json.Formatting.None);
                        this.LogDB.ExecuteNonQuery(true, s.BuildEx(@"declare @json varchar(850) set @json={json}
if exists (select sn from bbin_BetRecord nolock where json=@json) return insert into bbin_BetRecord (", sqltool._Fields, ",json) values (", sqltool._Values, ",@json)"));
                    }
                    if (r.pagination != null)
                    {
                        if (r.pagination.TotalPage > 0)
                            if (page < r.pagination.TotalPage)
                                continue;
                    }
                    return true;
                }
                else
                {
                    item.State = r.ResponseText;
                    //Thread.Sleep(3000);
                    break;
                }
            }
            return false;
        }

        class config_bbin : config_Item
        {
            static string _getkey(bbin.gamekind key, bbin.gametype? gametype)
            {
                if (gametype.HasValue)
                    return string.Format("{0:00}_{1}", (int)key, gametype.Value);
                else
                    return string.Format("{0:00}", (int)key);
            }
            public readonly bbin.gamekind _gamekind;
            public readonly bbin.gametype? _gametype;
            public config_bbin(frmMain frmMain, bbin.gamekind gamekind, bbin.gametype? gametype)
                : base(frmMain, GameID.BBIN, _getkey(gamekind, gametype), "bbin_execute")
            {
                this.Time = new DateTime(2014, 2, 1);
                this.MinLen = 300;
                this.MaxLen = 1800;
                this.Reserved = 300;
                this._gamekind = gamekind;
                this._gametype = gametype;
                frmMain.items_bbin.Add(this);
            }
        }

        #endregion

        #region hg

        void _hg_WriteRow(XmlElement elem, string tableName)
        {
            SqlSchemaTable schema = SqlSchemaTable.GetSchema(this.LogDB, tableName);
            sqltool s = new sqltool();
            foreach (XmlElement attr in elem.SelectNodes("./*"))
            {
                string name = schema.GetFieldName(attr.Name);
                if (string.IsNullOrEmpty(name)) continue;
                switch (name)
                {
                    case "asgametype": name = "GameType"; break;
                }
                s["", name, ""] = attr.InnerText * text.ValidAsString;
            }
            string xml = elem.OuterXml * text.ValidAsString;
            s.values["xml"] = xml;
            this.LogDB.ExecuteNonQuery(true, s.BuildEx(@"declare @xml varchar(850) set @xml={xml}
if exists (select sn from ", tableName, @" nolock where [xml]=@xml) return insert into ", tableName, " (", sqltool._Fields, ",xml) values (", sqltool._Values, ",@xml)"));
        }
        void _hg_WriteData(config_Item item, string tableName, hgResponse2 res, string msg)
        {
            item.State = msg ?? "Write Data...";
            if (res == null) return;
            if (string.Compare(res.DocumentElement.Name, "gameinfo") == 0)
            {
                foreach (XmlElement e1 in res.SelectNodes("//gameinfo/*"))
                    _hg_WriteRow(e1, "hg_GameResult");
            }
            else if (string.Compare(res.DocumentElement.Name, "playerinfo") == 0)
            #region playerinfo
            {
                //XmlAttribute startdate = res.DocumentElement.Attributes["startdate"];
                //XmlAttribute enddate = res.DocumentElement.Attributes["enddate"];
                //foreach (XmlElement e1 in res.SelectNodes("//playerinfo/playerdetails"))
                //{
                //    if (startdate != null) e1.AppendChild(res.CreateElement(startdate.Name)).InnerText = startdate.Value;
                //    if (enddate != null) e1.AppendChild(res.CreateElement(enddate.Name)).InnerText = enddate.Value;
                //    sqltool s = new sqltool();
                //    foreach (XmlElement e2 in e1.SelectNodes("./*"))
                //        fill(s, e2.Name.ToLower(), e1, e2);
                //    this.LogDB.ExecuteNonQueryT(s.BuildEx("insert into hg_PlayerDetails (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                //}
            }
            #endregion
            else if (string.Compare(res.DocumentElement.Name, "betinfos") == 0)
            {
                foreach (XmlElement e1 in res.SelectNodes("//betinfos/betinfo"))
                    _hg_WriteRow(e1, tableName);
            }
            else if (string.Compare(res.DocumentElement.Name, "fundtransferinfos") == 0)
            {
                foreach (XmlElement e1 in res.SelectNodes("//fundtransferinfos/getallaccounttransferdetails"))
                    _hg_WriteRow(e1, "hg_TransferDetails");
            }
            else if (string.Compare(res.DocumentElement.Name, "playerbetinfo") == 0)
            #region playerbetinfo
            {
                //XmlAttribute dateval = res.DocumentElement.Attributes["dateval"];
                //XmlAttribute timerange = res.DocumentElement.Attributes["timerange"];
                //foreach (XmlElement e1 in res.SelectNodes("//playerbetinfo/playerbetdetails"))
                //{
                //    if (dateval != null) e1.AppendChild(res.CreateElement(dateval.Name)).InnerText = dateval.Value;
                //    if (timerange != null) e1.AppendChild(res.CreateElement(timerange.Name)).InnerText = timerange.Value;
                //    sqltool s = new sqltool();
                //    foreach (XmlElement e2 in e1.SelectNodes("./*"))
                //        fill(s, e2.Name.ToLower(), e1, e2);
                //    this.LogDB.ExecuteNonQueryT(s.BuildEx("insert into hg_PlayerBetDetails (", sqltool._Fields, ") values (", sqltool._Values, ")"));
                //}
            }
            #endregion
            else if (string.Compare(res.DocumentElement.Name, "playinfos") == 0)
            {
            }
            //if (res != null) res.parse_data(this.LogDB);
        }
        [ObjectInvoke("hg_Betinfo1")]
        bool _hg_Betinfo1(config_Item item)
        {
            _hg_WriteData(item, "hg_Betinfo1", extAPI.hg.api.GetAllBetDetailsPerTimeInterval(null, item.StartTime.AddHours(-8), item.EndTime.AddHours(-8), null, null), null);
            return true;
        }
        [ObjectInvoke("hg_Betinfo2")]
        bool _hg_Betinfo2(config_Item item)
        {
            item.State = "Step 1";
            _hg_WriteData(item, "hg_Betinfo1", extAPI.hg.api.GetAllBetDetailsPerTimeInterval(null, item.StartTime.AddHours(-8), item.EndTime.AddHours(-8), null, null), null);
            DateTime start2 = item.StartTime.AddHours(-8).Date;
            DateTime end2 = item.EndTime.AddHours(-8).Date;
            item.State = "Step 2";
            _hg_WriteData(item, "hg_Betinfo2", extAPI.hg.api.Getbetdetails_ExcludeTieAndEven(null, start2, null), null);
            if (start2 != end2)
                _hg_WriteData(item, "hg_Betinfo2", extAPI.hg.api.Getbetdetails_ExcludeTieAndEven(null, end2, null), null);
            return true;
        }
        [ObjectInvoke("hg_Transfer")]
        bool _hg_Transfer(config_Item item)
        {
            _hg_WriteData(item, "hg_TransferDetails", extAPI.hg.api.GetAllFundTransferDetailsTimeInterval(null, item.StartTime.AddHours(-8), item.EndTime.AddHours(-8), null, null), null);
            return true;
        }
        [ObjectInvoke("hg_GameResult")]
        bool _hg_GameResult(config_Item item)
        {
            _hg_WriteData(item, "hg_GameResult", extAPI.hg.api.GetGameResultInfo(null, item.StartTime.AddHours(-8), item.EndTime.AddHours(-8), null, null), null);
            return true;
        }

        #endregion

        #region Form

        int update_index = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.propertyGrid1.Refresh();
            try
            {
                if (this.items.Count == 0) return;
                double t0 = 10;
                for (DateTime t1 = DateTime.Now; (DateTime.Now - t1).TotalMilliseconds < t0; )
                    this.items[this.update_index = (this.update_index + 1) % this.items.Count].Update();
            }
            catch { }
        }

        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if ((e.ColumnIndex < 0) || (e.RowIndex < 0)) return;
            //    object value = grid[e.ColumnIndex, e.RowIndex].Value;
            //    GameID gameID = (GameID)grid[colGameID.Index, e.RowIndex].Value;
            //    string key = (string)grid[colKey.Index, e.RowIndex].Value;
            //    if (e.ColumnIndex == colActive.Index)
            //    {
            //        config item;
            //        if (this.items.TryGetItem(gameID, key, out item))
            //        {
            //            Locked n = ((bool)value) ? Locked.Active : Locked.Locked;
            //            if (n != item.Active)
            //                item.SetActive = n;
            //        }
            //    }
            //}
            //catch { }
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if ((e.ColumnIndex < 0) || (e.RowIndex < 0)) return;
                object value = grid[e.ColumnIndex, e.RowIndex].Value;
                GameID gameID = (GameID)grid[colGameID.Index, e.RowIndex].Value;
                string key = (string)grid[colKey.Index, e.RowIndex].Value;
                foreach (config item in this.items)
                {
                    if ((item.GameID == gameID) && (item.Key == key))
                    {
                        if (e.ColumnIndex == colActive.Index)
                        {
                            item.Active = ((bool)value) ? Locked.Active : Locked.Locked;
                            using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite)) sqlcmd.ExecuteNonQuery(true, "update config set Active={Active} where GameID={GameID} and [Key]={Key}".SqlExport(null, item));
                        }
                        else if (e.ColumnIndex == colMinLen.Index)
                        {
                            item.MinLen = Convert.ToDouble(value);
                            using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite)) sqlcmd.ExecuteNonQuery(true, "update config set MinLen={MinLen} where GameID={GameID} and [Key]={Key}".SqlExport(null, item));
                        }
                        else if (e.ColumnIndex == colMaxLen.Index)
                        {
                            item.MaxLen = Convert.ToDouble(value);
                            using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite)) sqlcmd.ExecuteNonQuery(true, "update config set MaxLen={MaxLen} where GameID={GameID} and [Key]={Key}".SqlExport(null, item));
                        }
                        else if (e.ColumnIndex == colReserved.Index)
                        {
                            item.Reserved = Convert.ToDouble(value);
                            using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite)) sqlcmd.ExecuteNonQuery(true, "update config set Reserved={Reserved} where GameID={GameID} and [Key]={Key}".SqlExport(null, item));
                        }
                    }
                }
            }
            catch { }
        }

        protected override void OnLoad(EventArgs e)
        {
            RefreshButton();
            base.OnLoad(e);
        }

        void RefreshButton()
        {
            btnInstallService.Enabled = !LogServiceInstaller.IsServiceInstalled;
            btnUninstallService.Enabled = !btnInstallService.Enabled;
        }

        void InstallService(object sender, EventArgs e)
        {
            btnInstallService.Enabled = btnUninstallService.Enabled = false;
            ThreadPool.QueueUserWorkItem((object state) =>
            {
                try
                {
                    if (sender == btnInstallService)
                    {
                        log.message(null, "Installing service...");
                        LogServiceInstaller.InstallService();
                    }
                    else
                    {
                        log.message(null, "Uninstalling service...");
                        LogServiceInstaller.UninstallService();
                    }
                    this.Invoke((ThreadStart)RefreshButton);
                    log.message(null, "Successed");
                }
                catch (Exception ex)
                {
                    log.message(null, ex.Message);
                }
            });
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
            if (this.Visible)
                this.BringToFront();
        }

        private void cmdMsg_Click(object sender, EventArgs e)
        {
            cmdMsg.Checked ^= true;
            splitContainer1.Panel2Collapsed = !cmdMsg.Checked;
        }

        #endregion

        class config_Item : config
        {
            public config_Item(frmMain frmMain, BU.GameID gameID, string key, string method) : base(frmMain, gameID, key, method) { }

            public override void execute()
            {
                foreach (config n in this.SyncLock())
                {
                    if (this.Active != Locked.Active) return;
                    if (!this.CurrentTime.HasValue) return;
                    DateTime ct = this.CurrentTime.Value.AddSeconds(-this.Reserved);
                    DateTime end1 = this.Time.AddSeconds(this.MinLen);
                    DateTime end2 = this.Time.AddSeconds(this.MaxLen);
                    DateTime start = this.Time.AddSeconds(-this.MinLen);
                    while (end1 < end2)
                    {
                        DateTime end_tmp = end1.AddSeconds(this.MinLen);
                        if (end_tmp < ct)
                            end1 = end_tmp;
                        else
                            break;
                    }
                    if (end1 < ct)
                    {
                        this.CurrentTime = null;
                        //start = start.AddSeconds(-this.Reserved );
                        try
                        {
                            this.State = string.Format("{0:yyyy-MM-dd HH:mm:ss}-{1:yyyy-MM-dd HH:mm:ss}", this.StartTime = start, this.EndTime = end1);
                            //this.StartTime = start;//.AddSeconds(-this.Reserved);
                            //this.EndTime = end1;
                            bool ret = (bool)(ObjectInvoke.Invoke(this.frmMain, this.method ?? this.Key, this) ?? false);
                            if (ret == true)
                            {
                                this.Time = this.EndTime;
                                try
                                {
                                    frmMain.LogDB.BeginTransaction();
                                    frmMain.LogDB.ExecuteNonQuery("update config set Time={EndTime:yyyy-MM-dd HH:mm:ss} where [Key]={Key}".SqlExport(null, this));
                                    frmMain.LogDB.Commit();
                                    this.CurrentTime = null;
                                }
                                catch
                                {
                                    frmMain.LogDB.Rollback();
                                    throw;
                                }
                                this.State = "Idle";
                            }
                            return;
                        }
                        catch (Exception ex)
                        {
                            log.error_msg(ex);
                            this.State = ex.Message;
                            Thread.Sleep(5000);
                        }
                    }
                }
            }
        }
        class config_GameLog : config
        {
            public config_GameLog(frmMain frmMain, BU.GameID gameID, string key, string method) : base(frmMain, gameID, key ?? "GameLog", method) { }

            public override void execute()
            {
                foreach (config n in this.SyncLock())
                {
                    //if ((this.Active == Locked.Active) && this.CurrentTime.HasValue)
                    if (this.Active == Locked.Active)
                    {
                        foreach (config interval in frmMain.Interval.SyncLock())
                            this.CurrentTime = frmMain.Interval.CurrentTime;
                        try
                        {
                            //string m = this.method ?? string.Format("{0}_{1}", this.GameID, this.Key);
                            int count = (int)ObjectInvoke.Invoke(this.frmMain, this.method, this);
                            //if (count > 0)
                            {
                                this.Reserved = count;
                                frmMain.LogDB.ExecuteNonQuery(true, @"update config set Time={CurrentTime:yyyy-MM-dd HH:mm:ss} where GameID={GameID} and [Key]={Key}".SqlExport(null, this));
                            }
                        }
                        catch (Exception ex) { log.error_msg(ex); }
                    }
                }
            }
        }
        class config_BetAmtDG : config
        {
            public config_BetAmtDG(frmMain frmMain, BU.GameID gameID, string key, string method) : base(frmMain, gameID, key ?? "BetAmtDG", method) { }

            public override void execute()
            {
                foreach (config n in this.SyncLock())
                {
                    if (this.Active == Locked.Active)
                    {
                        this.CurrentTime = frmMain.Interval.CurrentTime;
                        try
                        {
                            DateTime time1 = this.Time.Date.AddDays(1);
                            DateTime time2 = this.CurrentTime.Value.AddSeconds(-this.Reserved);
                            DateTime time3 = time2.ToACTime();
                            if (time3 > time1)
                            {
                                bool result = (bool)ObjectInvoke.Invoke(frmMain, this.method, this, time1);
                                if (result)
                                {
                                    this.Time = time1;
                                    frmMain.LogDB.ExecuteNonQuery(true, "update config set Time={Time:yyyy/MM/dd} where GameID={GameID} and [Key]={Key}".SqlExport(null, this));
                                }
                            }
                        }
                        catch (Exception ex) { log.error_msg(ex); }
                    }
                }
            }

            public bool CheckTime_BetAmt(DateTime time, config item)
            {
                foreach (config n in item.SyncLock())
                    return time < item.Time;
                return false;
            }
        }

        [DebuggerDisplay("GameID:{GameID}, Key:{Key}")]
        abstract class config
        {
            protected readonly frmMain frmMain;
            public readonly GameID GameID;
            public readonly string Key;
            public readonly string method;
            public config(frmMain frmMain, BU.GameID gameID, string key, string method)
            {
                this.frmMain = frmMain;
                this.GameID = gameID;
                this.Key = key;
                this.method = method;
                frmMain.items.Add(this);
            }

            [DbImport("Active")]
            public Locked Active { get; set; }

            [DbImport("Time")]
            public DateTime Time { get; set; }

            //public DateTime Time_ACTime { get { return this.Time.ToACTime(); } }

            [DbImport("MinLen")]
            public double MinLen { get; set; }

            [DbImport("MaxLen")]
            public double MaxLen { get; set; }

            [DbImport("Reserved")]
            public double Reserved { get; set; }

            public object State { get; set; }

            [DbImport("ct")]
            public DateTime? CurrentTime { get; set; }

            DataGridViewRow row;

            public DateTime StartTime;
            public DateTime EndTime;

            public abstract void execute();

            int sync_count;

            public IEnumerable<config> SyncLock()
            {
                if (Monitor.TryEnter(this))
                {
                    try
                    {
                        this.sync_count++;
                        yield return this;
                    }
                    finally
                    {
                        Monitor.Exit(this);
                        this.sync_count--;
                    }
                }
            }

            public string TimeFormat = log.DefaultTimeFormat;
            public void Update()
            {
                if (this.row == null)
                {
                    this.row = frmMain.grid.Rows[frmMain.grid.Rows.Add()];
                    this.row.Cells[frmMain.colTime.Name].Style.Format = this.TimeFormat;
                }
                this.row.Cells[frmMain.colGameID.Name].update(this.GameID);
                this.row.Cells[frmMain.colKey.Name].update(this.Key);
                this.row.Cells[frmMain.colActive.Name].update(this.Active == Locked.Active);
                this.row.Cells[frmMain.colLoaded.Name].update(this.CurrentTime.HasValue);
                this.row.Cells[frmMain.colCurrentTime.Name].update(this.CurrentTime);
                this.row.Cells[frmMain.colLock.Name].update(this.sync_count > 0);
                this.row.Cells[frmMain.colTime.Name].update(this.Time);
                this.row.Cells[frmMain.colMinLen.Name].update(this.MinLen);
                this.row.Cells[frmMain.colMaxLen.Name].update(this.MaxLen);
                this.row.Cells[frmMain.colReserved.Name].update(this.Reserved);
                this.row.Cells[frmMain.colState.Name].update(this.State);
            }
        }
    }

    static class _util
    {
        public static void update(this DataGridViewCell cell, object value)
        {
            if (cell.Displayed)
                if (cell.Value != value)
                    cell.Value = value;
        }
    }

    class LogService : ServiceBase
    {
        [STAThread]
        static void Main(string[] args)
        {
            TextLogWriter.Enabled
                = JsonTextLogWriter.Enabled
                //= SqlCommandTextLogWriter.Enabled
                //= ConsoleLogWriter.Enabled
                = true;

            bool debugMode = false;
            LogService svc = new LogService();
            if (args.Length > 0)
            {
                for (int ii = 0; ii < args.Length; ii++)
                {
                    switch (args[ii].ToLower())
                    {
                        case "/install":
                        case "/i":
                            LogServiceInstaller.InstallService();
                            return;
                        case "/uninstall":
                        case "/u":
                            LogServiceInstaller.UninstallService();
                            return;
                        case "/debug":
                        case "/d":
                            debugMode = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (debugMode)
                svc.Run();
            else
                System.ServiceProcess.ServiceBase.Run(svc);
            //try
            //{
            //    using (StreamReader sr = new StreamReader(@"C:\tengfa\Service\LogService\bin\Debug\Log\2013-08\2013-08-05_18.txt"))
            //    using (JsonTextReader jr = new JsonTextReader(sr))
            //    {
            //        while (jr.Read())
            //        {
            //        }
            //    }
            //}
            //catch
            //{
            //    //Debugger.Break();
            //}

            //log.message(null, "test1");
            //log.message(null, "test3");
            //log.message("msg", "test2");
            //string json_s = @"{""Code_Update"":{""Code"":"""",""Code_"":""3"",""resid"":""33"",""Parent"":""undefined"",""Flag"":"""",""Sort"":"""",""Path"":"""",""oper"":""edit"",""rowid"":""jqg3""}}";
            //sql_tool d = new sql_tool(json_s);
            //d.isUpdate = false;
            ////d.dst["Parent"] = d.dst["Path"] = null;
            //d.Int32("Code_", "Code", null);
            //d.Int32("Parent", null, null);
            //d.String("resid", null, null);
            //d.Int32("Flag", null, null);
            //d.Int32("Sort", null, null);
            //d.Int32("Path", null, null);
            //            StringBuilder s = new StringBuilder();
            //            s.AppendFormat(@"insert into Code (Code,resid) values ({Code:00},{resid})
            //select * from Code nolock where Code={{Code}}", d.fields('[', ']'), d.fields('{', '}'));



        }

        public LogService()
        {
            //set initializers here
        }

        protected override void OnStart(string[] args)
        {
            new Thread(this.Run).Start();
        }

        void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            this.Stop();
        }

        protected override void OnStop()
        {
        }

        protected override void Dispose(bool disposing)
        {

            //clean your resources if you have to
            base.Dispose(disposing);
        }
    }
}
