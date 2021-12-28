using BU;
using extAPI.hg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using Tools;
using web;

namespace LogService
{
    class TimeTypeConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if ((value is DateTime) && (destinationType == typeof(string)))
                return ((DateTime)value).ToString(log.DefaultTimeFormat);
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    class hg_proc
    {
        class MemberID_Cache : Dictionary<string, MemberRow>
        {
            public bool GetMemberID(SqlCmd sqlcmd, string acnt, out MemberRow memberRow)
            {
                if (!this.TryGetValue(acnt, out memberRow))
                {
                    this[acnt] = memberRow = sqlcmd.ToObject<MemberRow>(@"declare @MemberID int select @MemberID=MemberID from Member_001 nolock where ACNT='{0}'
if @MemberID is not null select a.CorpID,a.ID,a.ACNT,b.ID as ParentID,b.ACNT as ParentACNT from Member a with(nolock) left join Agent b with(nolock) on a.ParentID=b.ID where a.ID=@MemberID", acnt);
                }
                return (memberRow != null);
            }
        }

        //Dictionary<string, item> all = new Dictionary<string, item>();
        public hg_proc()
        {
            //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            //            {
            //                sqlcmd.BeginTransaction();
            //                sqlcmd.ExecuteNonQuery(@"
            //--update log_config set _Time='2013-10-11' where _Key='GameLog'
            //--update log_config set _Time='2013-10-01' where _Key='BetAmtD'
            //--update log_config set _Time='2013-11-08 00:00:00' where _Key='Betinfo2'
            //--update log_config set _Time='2013-11-08 03:00:00' where _Key in ('Transfer','Betinfo1')
            //");
            //                sqlcmd.Commit();
            //            }
            Tick.OnTick += Tick_OnTick;
            //ThreadPool.QueueUserWorkItem(this.init);
        }

        [TypeConverter(typeof(TimeTypeConverter))]
        public DateTime ReadTime { get; private set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class item1
        {
            [DbImport("_Key")]
            public string key { get; set; }
            [DbImport("_Active")]
            public Locked active { get; set; }
            [DbImport("_Time"), TypeConverter(typeof(TimeTypeConverter))]
            public DateTime time { get; set; }
            [DbImport("_Len")]
            public double len { get; set; }
            [DbImport("_Reserved")]
            public double reserved { get; set; }


            //public DateTime start;
            //public DateTime end;
            //public IEnumerable<item1> Execute1(DateTime sqlTime, double max)
            //{
            //    this.start = this.time;
            //    DateTime end2 = start.AddSeconds(this.len + this.reserved);
            //    DateTime max2 = start.AddSeconds(max + this.reserved);
            //    while (end2 < max2)
            //    {
            //        DateTime end3 = end2.AddSeconds(this.len);
            //        if (end3 < sqlTime)
            //            end2 = end3;
            //        else
            //            break;
            //    }
            //    this.end = end2.AddSeconds(-this.reserved);
            //    bool res = end2 < sqlTime;
            //    if (res)
            //    {
            //        this.State = "Waiting response...";
            //        yield return this;
            //    }
            //    this.State = "Idle";
            //}

            public bool Execute1(object target, SqlCmd sqlcmd, DateTime sqltime, double? max)
            {
                max = max ?? this.len;
                DateTime start = this.time;
                DateTime end2 = start.AddSeconds(this.len + this.reserved);
                DateTime max2 = start.AddSeconds(max.Value + this.reserved);
                while (end2 < max2)
                {
                    DateTime end3 = end2.AddSeconds(this.len);
                    if (end3 < sqltime)
                        end2 = end3;
                    else
                        break;
                }
                DateTime end = end2.AddSeconds(-this.reserved);
                if (end2 < sqltime)
                {
                    this.State = "Waiting response...";
                    try
                    {
                        bool ret = (bool)ObjectInvoke.Invoke(target, this.key, sqlcmd, this, start, end);
                        if (ret == true)
                        {
                            try
                            {
                                sqlcmd.BeginTransaction();
                                sqlcmd.ExecuteNonQuery("update log_config set _Time='{0}' where _Key='{1}'", end.ToString(sqltool.DateTimeFormat), this.key);
                                sqlcmd.Commit();
                            }
                            catch
                            {
                                sqlcmd.Rollback();
                                throw;
                            }
                        }
                        this.State = "Idle";
                        return ret;
                    }
                    catch (Exception ex)
                    {
                        this.State = ex.Message;
                    }
                }
                else this.State = "Idle";
                return false;
            }

            public bool Update1(SqlCmd sqlcmd, DateTime end)
            {
                sqlcmd.ExecuteNonQuery("update log_config set _Time='{0}' where _Key='{1}'", end.ToString(sqltool.DateTimeFormat), this.key);
                return true;
            }

            public void WriteData1(SqlCmd sqlcmd, hgResponse2 res, string msg)
            {
                this.State = msg ?? "Write Data...";
                if (res != null) res.parse_data(sqlcmd);
            }

            public string State;

            public override string ToString()
            {
                return this.time.ToString(sqltool.DateTimeFormat) + ", " + this.State;
            }
        }

        [TypeConverter(typeof(TimeTypeConverter)), DisplayName("SqlTime")]
        public DateTime SqlTime { get; private set; }

        item1 interval = new item1() { key = "Interval", time = DateTime.Now, len = 1000, reserved = 0, active = Locked.Active };
        public item1 Interval { get { return this.interval; } }
        public item1 Betinfo1 { get; private set; }
        public item1 GameResult { get; private set; }
        public item1 Transfer { get; private set; }
        public item1 Betinfo2 { get; private set; }
        public item1 GameLog { get; private set; }
        public item1 BetAmtD { get; private set; }

        bool Tick_OnTick()
        {
            if (Monitor.TryEnter(this))
            {
                try
                {
                    DateTime t = this.interval.time.AddMilliseconds(Math.Max(1000, this.interval.len));
                    if (DateTime.Now > t)
                    {
                        this.interval.time = DateTime.Now;
                        using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite))
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                DateTime sqltime = DateTime.Now;
                                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select getdate() as ct, * from log_config nolock"))
                                {
                                    this.SqlTime = sqltime = r.GetDateTime("ct");
                                    string key = r.GetString("_Key");
                                    PropertyInfo p = this.GetType().GetProperty(key, typeof(item1));
                                    if (p == null) continue;
                                    object o = p.GetValue(this, null);
                                    if (o == null)
                                        p.SetValue(this, r.ToObject<item1>(), null);
                                    else
                                        r.FillObject(o);
                                }
                                this.interval.time = DateTime.Now;
                                if (i > 0) break;
                                if (!this.Betinfo2.Execute1(this, sqlcmd, sqltime, null))
                                {
                                    this.Betinfo1.Execute1(this, sqlcmd, sqltime, this.Betinfo2.len);
                                    this.GameResult.Execute1(this, sqlcmd, sqltime, this.Betinfo2.len);
                                    this.Transfer.Execute1(this, sqlcmd, sqltime, this.Betinfo2.len);
                                }
                                _GameLog(sqlcmd);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.interval.time = DateTime.Now;
                    this.interval.len = 1000;
                    log.error_msg(ex);
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }
            return true;
        }

        [ObjectInvoke("Betinfo1")]
        bool _GetAllBetDetailsPerTimeInterval______(SqlCmd sqlcmd, item1 item, DateTime start, DateTime end)
        {
            item.WriteData1(sqlcmd, extAPI.hg.api.GetAllBetDetailsPerTimeInterval(null, start.AddHours(-8), end.AddHours(-8), null, null), null);
            return true;
        }
        [ObjectInvoke("GameResult")]
        bool _GetGameResultInfo____________________(SqlCmd sqlcmd, item1 item, DateTime start, DateTime end)
        {
            item.WriteData1(sqlcmd, extAPI.hg.api.GetGameResultInfo(null, start.AddHours(-8), end.AddHours(-8), null, null), null);
            return true;
        }
        [ObjectInvoke("Transfer")]
        bool _GetAllFundTransferDetailsTimeInterval(SqlCmd sqlcmd, item1 item, DateTime start, DateTime end)
        {
            item.WriteData1(sqlcmd, extAPI.hg.api.GetAllFundTransferDetailsTimeInterval(null, start.AddHours(-8), end.AddHours(-8), null, null), null);
            return true;
        }
        [ObjectInvoke("Betinfo2")]
        bool _Getbetdetails_ExcludeTieAndEven______(SqlCmd sqlcmd, item1 item, DateTime start, DateTime end)
        {
            if ((this.Betinfo1.time >= end) && (this.GameResult.time >= end) && (this.Transfer.time >= end))
            {
                DateTime start1 = start.AddSeconds(-item.reserved).AddHours(-8);
                DateTime end1 = end.AddSeconds(item.reserved).AddHours(-8);
                item.State = "Waiting Response (GetAllBetDetailsPerTimeInterval)...";
                item.WriteData1(sqlcmd, extAPI.hg.api.GetAllBetDetailsPerTimeInterval(null, start1, end1, null, null), null);
                //item.State = "Waiting Response (GetGameResultInfo)...";
                //item.WriteData1(sqlcmd, extAPI.hg.api.GetGameResultInfo(null, start1, end1, null, null), null);
                item.State = "Waiting Response (GetAllFundTransferDetailsTimeInterval)...";
                item.WriteData1(sqlcmd, extAPI.hg.api.GetAllFundTransferDetailsTimeInterval(null, start1, end1, null, null), null);
                item.State = "Waiting Response (Getbetdetails_ExcludeTieAndEven)...";
                DateTime start2 = start.AddHours(-8).Date;
                DateTime end2 = end.AddHours(-8).Date;
                item.WriteData1(sqlcmd, extAPI.hg.api.Getbetdetails_ExcludeTieAndEven(null, start2, null), null);
                if (start2 != end2)
                    item.WriteData1(sqlcmd, extAPI.hg.api.Getbetdetails_ExcludeTieAndEven(null, end2, null), null);
                return true;
            }
            return false;
        }

        void _GameLog(SqlCmd logDB)
        {
            MemberID_Cache member_cache = new MemberID_Cache();
            item1 item = this.GameLog;
            item.WriteData1(null, null, null);
            int n1 = 0, n2 = 0;
            using (SqlCmd mainDB = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                #region 過帳
                DateTime? ct = null;
                foreach (SqlDataReader r in logDB.ExecuteReader2(@"declare @t1 datetime, @t2 datetime select @t1=_Time, @t2=getdate() from log_config nolock where _Key='GameLog'
select 0 as src, @t2 as ct
select 1 as src, * from log_hg_Betinfo1 nolock where CreateTime>=@t1 and CreateTime<@t2
select 2 as src, * from log_hg_Betinfo2 nolock where CreateTime>=@t1 and CreateTime<@t2"))
                {
                    int src = r.GetInt32("src");
                    if (src == 0)
                    {
                        ct = r.GetDateTimeN("ct");
                    }
                    else
                    {
                        n1++;
                        string AccountId = r.GetString("AccountId");
                        MemberRow member;
                        if (member_cache.GetMemberID(mainDB, AccountId, out member))
                        {
                            try
                            {
                                sqltool s = new sqltool();
                                s["", "BetAmount", "    "] = 0;
                                s["", "BetAmountAct", " "] = 0;
                                #region ...
                                for (int i = 0; i < r.FieldCount; i++)
                                {
                                    string name = r.GetName(i);
                                    object value = r.GetValue(i);
                                    switch (name)
                                    {
                                        case "src":
                                        case "ID":
                                        case "CreateTime": continue;
                                        case "BetStartDate": name = "BetStartTime"; value = r.GetDateTime(i).AddHours(8).ToString(sqltool.DateTimeFormat); break;
                                        case "BetEndDate": name = "BetEndTime"; value = r.GetDateTime(i).AddHours(8).ToString(sqltool.DateTimeFormat); break;
                                        case "BetAmount": if (src == 2) name = "BetAmountAct"; break;
                                        case "user_id": name = "UserID"; break;
                                        case "table_id": name = "TableId"; break;
                                        case "AccountId":
                                        case "TableId":
                                        case "TableName":
                                        case "GameId":
                                        case "Payout":
                                        case "BetId":
                                        case "Currency":
                                        case "GameType":
                                        case "BetSpot":
                                        case "BetNo": break;
                                        default: break;
                                    }
                                    s["", name, ""] = value;
                                }
                                #endregion
                                s.values["MemberID"] = member.ID;
                                s.values["ParentID"] = member.ParentID;
                                s.values["ParentACNT"] = member.ParentACNT;
                                s.values["ACTime"] = r.GetDateTime("BetStartDate").AddHours(8).ToACTime().ToString("yyyy-MM-dd");
                                mainDB.BeginTransaction();
                                long? id = mainDB.ExecuteScalar<long>(s.BuildEx("select ID from GameLog_001 nolock where AccountId={AccountId} and TableId={TableId} and GameId={GameId} and BetId={BetId} and BetNo={BetNo}"));
                                if (id.HasValue)
                                {
                                    s.values["ID"] = id;
                                    mainDB.ExecuteNonQuery(s.BuildEx("update GameLog_001 set ", src == 1 ? "BetAmount={BetAmount},TableName={TableName}" : "BetAmountAct={BetAmountAct},UserID={UserID}", " where ID={ID}"));
                                }
                                else
                                {
                                    mainDB.ExecuteNonQuery(s.BuildEx(@"insert into GameLog_001 (MemberID,CorpID,ACNT,ACTime,ParentID,ParentACNT,", sqltool._Fields, ") select ID,CorpID,ACNT,{ACTime},{ParentID},{ParentACNT},", sqltool._Values, @" from Member nolock where ID={MemberID}"));
                                }
                                mainDB.Commit();
                                n2++;
                            }
                            catch
                            {
                                mainDB.Rollback();
                                throw;
                            }
                        }
                    }
                }
                if (ct.HasValue) item.Update1(logDB, ct.Value);
                item.State = "";
                #endregion

                DateTime actime1 = this.BetAmtD.time.Date;
                DateTime actime2 = this.Betinfo2.time.AddHours(0).ToACTime();
                if (actime1 < actime2)
                {
                    try
                    {
                        mainDB.BeginTransaction();
                        mainDB.ExecuteNonQuery(@"delete GameLog_BetAmtDG where GameID={0} and ACTime='{1:yyyy-MM-dd}' and CreateUser=0
insert into GameLog_BetAmtDG (ACTime,GameID,GameType,CorpID,MemberID,ACNT,ParentID,ParentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser)
select ACTime,{0},GameType,CorpID,MemberID,ACNT,ParentID,ParentACNT,sum(BetAmount),sum(BetAmountAct),sum(Payout), getdate(),0
from GameLog_001 where ACTime='{1:yyyy-MM-dd}' group by ACTime,GameType,CorpID,ParentID,ParentACNT,MemberID,ACNT", (int)GameID.HG, actime1);
                        mainDB.Commit();
                        this.BetAmtD.Update1(logDB, actime1.AddDays(1));
                    }
                    catch
                    {
                        mainDB.Rollback();
                        throw;
                    }
                }
            }
        }

        //        [ObjectInvoke("Reload")]
        //        void _Reload_______________________________(item item, DateTime startTime, DateTime endTime, double len)
        //        {
        //            //log.message(null, "Reload {0} {1}", startTime.ToString(TimeFormat), endTime.ToString(TimeFormat));
        //        }
        //        [ObjectInvoke("GameLog")]
        //        void _GameLog______________________________(item item, DateTime startTime, DateTime endTime, double len)
        //        {
        //            MemberRow member;
        //            using (SqlCmd logDB = DB.Open(DB.Name.Log, DB.Access.ReadWrite), mainDB = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //            {
        //                #region 過帳
        //                MemberID_Cache member_cache = new MemberID_Cache();
        //                item.WriteData(null, null, null);
        //                int n1 = 0, n2 = 0;
        //                foreach (SqlDataReader r in logDB.ExecuteReader2(@"
        //  select 1 as src, * from log_hg_Betinfo1 nolock where CreateTime>='{0}' and CreateTime<'{1}'
        //  select 2 as src, * from log_hg_Betinfo2 nolock where CreateTime>='{0}' and CreateTime<'{1}'"
        //                    , startTime.ToString(sqltool.DateTimeFormat), endTime.ToString(sqltool.DateTimeFormat)))
        //                {
        //                    n1++;
        //                    int src = r.GetInt32("src");
        //                    string AccountId = r.GetString("AccountId");
        //                    if (member_cache.GetMemberID(mainDB, AccountId, out member))
        //                    {
        //                        try
        //                        {
        //                            sqltool s = new sqltool();
        //                            s["", "BetAmount", "    "] = 0;
        //                            s["", "BetAmountAct", " "] = 0;
        //                            #region ...
        //                            for (int i = 0; i < r.FieldCount; i++)
        //                            {
        //                                string name = r.GetName(i);
        //                                object value = r.GetValue(i);
        //                                switch (name)
        //                                {
        //                                    case "src":
        //                                    case "ID":
        //                                    case "CreateTime": continue;
        //                                    case "BetStartDate": name = "BetStartTime"; value = r.GetDateTime(i).AddHours(8).ToString(sqltool.DateTimeFormat); break;
        //                                    case "BetEndDate": name = "BetEndTime"; value = r.GetDateTime(i).AddHours(8).ToString(sqltool.DateTimeFormat); break;
        //                                    case "BetAmount": if (src == 2) name = "BetAmountAct"; break;
        //                                    case "user_id": name = "UserID"; break;
        //                                    case "table_id": name = "TableId"; break;
        //                                    case "AccountId":
        //                                    case "TableId":
        //                                    case "TableName":
        //                                    case "GameId":
        //                                    case "Payout":
        //                                    case "BetId":
        //                                    case "Currency":
        //                                    case "GameType":
        //                                    case "BetSpot":
        //                                    case "BetNo": break;
        //                                    default: break;
        //                                }
        //                                s["", name, ""] = value;
        //                            }
        //                            #endregion
        //                            s.Values["MemberID"] = member.ID;
        //                            s.Values["ParentID"] = member.ParentID;
        //                            s.Values["ParentACNT"] = member.ParentACNT;
        //                            s.Values["ACTime"] = r.GetDateTime("BetStartDate").AddHours(8).ToACTime().ToString("yyyy-MM-dd");
        //                            mainDB.BeginTransaction();
        //                            long? id = mainDB.ExecuteScalar<long>(s.BuildEx("select ID from GameLog_001 nolock where BetStartTime={BetStartTime} and BetEndTime={BetEndTime} and AccountId={AccountId} and TableId={TableId} and GameId={GameId} and BetId={BetId} and BetNo={BetNo}"));
        //                            if (id.HasValue)
        //                            {
        //                                s.Values["ID"] = id;
        //                                mainDB.ExecuteNonQuery(s.BuildEx("update GameLog_001 set ", src == 1 ? "BetAmount={BetAmount},TableName={TableName}" : "BetAmountAct={BetAmountAct},UserID={UserID}", " where ID={ID}"));
        //                            }
        //                            else
        //                            {
        //                                mainDB.ExecuteNonQuery(s.BuildEx(@"insert into GameLog_001 (MemberID,CorpID,ACNT,ACTime,ParentID,ParentACNT,", sqltool._Fields, ") select ID,CorpID,ACNT,{ACTime},{ParentID},{AgentACNT},", sqltool._Values, @" from Member nolock where ID={MemberID}"));
        //                            }
        //                            mainDB.Commit();
        //                            n2++;
        //                        }
        //                        catch
        //                        {
        //                            mainDB.Rollback();
        //                            throw;
        //                        }
        //                    }
        //                }
        //                #endregion
        //                item.update_time(logDB, len);
        //            }
        //        }
        //        [ObjectInvoke("BetAmtD")]
        //        void _BetAmtD______________________________(item item, DateTime startTime, DateTime endTime, double len)
        //        {
        //            return;
        //            DateTime actime1;
        //            if (Monitor.TryEnter(this.all))
        //            {
        //                actime1 = item.Time.Date;
        //                DateTime actime2 = Betinfo2_.Time.ToACTime();
        //                //DateTime actime3 = GameLog.Time.AddHours(-12).Date;
        //                try
        //                {
        //                    if (actime1 >= actime2) return;
        //                    if (GameLog_.Time.AddMilliseconds(GameLog_.End) < GameLog_.SqlTime) return;
        //                }
        //                finally
        //                {
        //                    Monitor.Exit(this.all);
        //                }
        //                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        //                {
        //                    try
        //                    {
        //                        sqlcmd.BeginTransaction();
        //                        sqlcmd.ExecuteNonQuery(@"delete GameLog_BetAmtDG where GameID={0} and ACTime='{1:yyyy-MM-dd}' and CreateUser=0
        //insert into GameLog_BetAmtDG (ACTime,GameID,GameType,CorpID,MemberID,ACNT,ParentID,AgentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser)
        //select ACTime,{0},GameType,CorpID,MemberID,ACNT,ParentID,AgentACNT,sum(BetAmount),sum(BetAmountAct),sum(Payout), getdate(),0
        //from GameLog_001 where ACTime='{1:yyyy-MM-dd}' group by ACTime,GameType,CorpID,ParentID,AgentACNT,MemberID,ACNT", (int)GameID.HG, actime1);
        //                        sqlcmd.Commit();
        //                    }
        //                    catch
        //                    {
        //                        sqlcmd.Rollback();
        //                        throw;
        //                    }
        //                    //log.message(null, "BetAmtD\tACTime:{0}, {1}", actime1.ToString(TimeFormat), actime2.ToString(TimeFormat));
        //                    item.update_time(null, string.Format("update log_config set _Time=dateadd(dd,1,_Time) where _Key='{0}'", item.Key));
        //                }
        //            }
        //        }
        //        [ObjectInvoke("Betinfo1")]
        //        void _GetAllBetDetailsPerTimeInterval______(item item, DateTime startTime, DateTime endTime, double len)
        //        {
        //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite))
        //            {
        //                double n = (endTime - startTime).TotalSeconds;
        //                item.WriteData(sqlcmd, extAPI.hg.api.GetAllBetDetailsPerTimeInterval(null, startTime.AddHours(-8), endTime.AddHours(-8), null, null), null);
        //                item.update_time(sqlcmd, len);
        //            }
        //        }
        //        [ObjectInvoke("Betinfo2")]
        //        void _Getbetdetails_ExcludeTieAndEven______(item item, DateTime startTime, DateTime endTime, double len)
        //        {
        //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite))
        //            {
        //                item.State = "Waiting Response (GetAllBetDetailsPerTimeInterval)...";
        //                double extend = 10;
        //                while (true)
        //                {
        //                    try { item.WriteData(sqlcmd, extAPI.hg.api.GetAllBetDetailsPerTimeInterval(null, startTime.AddHours(-8).AddMinutes(-extend), endTime.AddHours(-8).AddMinutes(extend), null, null), null); break; }
        //                    catch (Exception ex) { item.error(ex); }
        //                }
        //                item.State = "Waiting Response (GetGameResultInfo)...";
        //                while (true)
        //                {
        //                    try { item.WriteData(sqlcmd, extAPI.hg.api.GetGameResultInfo(null, startTime.AddHours(-8).AddMinutes(-extend), endTime.AddHours(-8).AddMinutes(extend), null, null), null); break; }
        //                    catch (Exception ex) { item.error(ex); }
        //                }
        //                item.State = "Waiting Response (GetAllFundTransferDetailsTimeInterval)...";
        //                while (true)
        //                {
        //                    try { item.WriteData(sqlcmd, extAPI.hg.api.GetAllFundTransferDetailsTimeInterval(null, startTime.AddHours(-8).AddMinutes(-extend), endTime.AddHours(-8).AddMinutes(extend), null, null), null); break; }
        //                    catch (Exception ex) { item.error(ex); }
        //                }
        //                item.State = "Waiting Response (Getbetdetails_ExcludeTieAndEven)...";
        //                while (true)
        //                {
        //                    try { item.WriteData(sqlcmd, extAPI.hg.api.Getbetdetails_ExcludeTieAndEven(null, startTime.AddHours(-8), null), null); break; }
        //                    catch (Exception ex) { item.error(ex); }
        //                }
        //                item.update_time(sqlcmd, len);
        //            }
        //        }
        //        [ObjectInvoke("GameResult")]
        //        void _GetGameResultInfo____________________(item item, DateTime startTime, DateTime endTime, double len)
        //        {
        //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite))
        //            {
        //                item.WriteData(sqlcmd, extAPI.hg.api.GetGameResultInfo(null, startTime.AddHours(-8), endTime.AddHours(-8), null, null), null);
        //                item.update_time(sqlcmd, len);
        //            }
        //        }
        //        [ObjectInvoke("Transfer")]
        //        void _GetAllFundTransferDetailsTimeInterval(item item, DateTime startTime, DateTime endTime, double len)
        //        {
        //            using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite))
        //            {
        //                item.WriteData(sqlcmd, extAPI.hg.api.GetAllFundTransferDetailsTimeInterval(null, startTime.AddHours(-8), endTime.AddHours(-8), null, null), null);
        //                item.update_time(sqlcmd, len);
        //            }
        //        }

        //        internal item Reload_ { get; set; }
        //        internal item GameLog_ { get; set; }
        //        internal item BetAmtD_ { get; set; }

        //        internal item Betinfo1_ { get; set; }      // HG
        //        internal item Betinfo2_ { get; set; }      // HG
        //        internal item GameResult_ { get; set; }    // HG
        //        internal item Transfer_ { get; set; }      // HG

        //        void init2(object state)
        //        {
        //            try
        //            {
        //                lock (this.all)
        //                {
        //                    using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.Read))
        //                        foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from log_config"))
        //                            r.ToObject<item>().init(this);

        //                    foreach (item item in this.all.Values)
        //                    {
        //                        PropertyInfo p = this.GetType().GetProperty(item.Key, typeof(item));
        //                        if (p == null) continue;
        //                        try { p.SetValue(this, item, null); }
        //                        catch { }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                log.error(ex);
        //            }
        //            Tick.OnTick += tick;
        //        }
        //        bool tick()
        //        {
        //            foreach (item item in this.all.Values)
        //                item.tick();
        //            return true;
        //        }

        //        [TypeConverter(typeof(ExpandableObjectConverter))]
        //        public class item
        //        {
        //            public DateTime CurrentTime
        //            {
        //                get { return this.m_SqlTime.Add(this.ReadElapsed); }
        //            }

        //            [DbImport("ct")]
        //            internal DateTime SqlTime
        //            {
        //                get { return this.m_SqlTime; }
        //                set
        //                {
        //                    this.m_SqlTime = value;
        //                    this.m_ReadTime = DateTime.Now;
        //                }
        //            }
        //            DateTime m_SqlTime = DateTime.Now;

        //            DateTime m_ReadTime = DateTime.Now;
        //            public TimeSpan ReadElapsed
        //            {
        //                get { return DateTime.Now - this.m_ReadTime; }
        //            }

        //            [DbImport("_Key")]
        //            public string Key;

        //            int m_Active;
        //            [DbImport("_Active")]
        //            public Locked Active
        //            {
        //                get { return (Locked)Interlocked.CompareExchange(ref this.m_Active, 0, 0); }
        //                private set { Interlocked.Exchange(ref this.m_Active, (int)value); }
        //            }

        //            [DbImport("_Time")]
        //            public DateTime Time = DateTime.Now;
        //            [DisplayName("Time")]
        //            public string _Time
        //            {
        //                get { return this.Time.ToString(sqltool.DateTimeFormat); }
        //            }

        //            int m_Start;
        //            [DbImport("_Start")]
        //            public int Start
        //            {
        //                get { return Interlocked.CompareExchange(ref this.m_Start, 0, 0); }
        //                set { Interlocked.Exchange(ref this.m_Start, value); }
        //            }
        //            //DateTime StartTime
        //            //{
        //            //    get { return this.Time.AddMilliseconds(this.Start); }
        //            //}
        //            //[DisplayName("StartTime")]
        //            //public string _StartTime
        //            //{
        //            //    get { return this.StartTime.ToString(TimeFormat); }
        //            //}

        //            int m_End;
        //            [DbImport("_End")]
        //            public int End
        //            {
        //                get { return Interlocked.CompareExchange(ref this.m_End, 0, 0); }
        //                set { Interlocked.Exchange(ref this.m_End, value); }
        //            }
        //            //DateTime EndTime
        //            //{
        //            //    get { return this.Time.AddMilliseconds(this.End); }
        //            //}
        //            //[DisplayName("EndTime")]
        //            //public string _EndTime
        //            //{
        //            //    get { return this.EndTime.ToString(TimeFormat); }
        //            //}

        //            public string State { get; set; }

        //            hg_proc _top;

        //            public void init(hg_proc _top)
        //            {
        //                (this._top = _top).all[this.Key] = this;
        //            }

        //            public void tick()
        //            {
        //                if (!Monitor.TryEnter(this))
        //                    return;
        //                try
        //                {
        //                    bool unlock = true;
        //                    if (!Monitor.TryEnter(_top.all))
        //                        return;
        //                    try
        //                    {
        //                        this.update_time(null, null);
        //                        double start = this.Start;
        //                        double end = this.End;
        //                        DateTime currentTime = this.CurrentTime;
        //                        DateTime time = this.Time;
        //                        DateTime startTime = time.AddMilliseconds(start);
        //                        DateTime endTime = time.AddMilliseconds(end);
        //                        if (currentTime < endTime)
        //                            this.State = (endTime - currentTime).ToString();
        //                        else if (this.Active == Locked.Active)
        //                        {
        //                            this.Active = Locked.Locked;
        //                            Monitor.Exit(_top.all);
        //                            unlock = false;
        //                            for (DateTime e1 = time.AddMinutes(30); endTime < e1; )
        //                            {
        //                                DateTime e2 = endTime.AddMilliseconds(end);
        //                                if (e2 >= currentTime) break;
        //                                endTime = e2;
        //                            }
        //                            this.State = "Waiting Response...";
        //                            ObjectInvoke.Invoke(this._top, this.Key, this, startTime, endTime, (endTime - startTime).TotalMilliseconds);
        //                            this.State = "";
        //                        }
        //                        else
        //                            this.State = "Stopped!";
        //                    }
        //                    finally
        //                    {
        //                        if (unlock) Monitor.Exit(_top.all);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    this.error(ex);
        //                }
        //                finally
        //                {
        //                    Monitor.Exit(this);
        //                }
        //            }

        //            public void WriteData(SqlCmd sqlcmd, hgResponse2 res, string msg)
        //            {
        //                this.State = msg ?? "Write Data...";
        //                if (res != null) res.parse_data(sqlcmd);
        //            }

        //            public void error(Exception ex)
        //            {
        //                this.State = ex.Message;
        //                log.error(ex);
        //                //Thread.Sleep(_top.Interval * 2);
        //            }

        //            void reset_time()
        //            {
        //                this.m_ReadTime = this.SqlTime = DateTime.Now;
        //                //_top.Interval = 15000;
        //            }

        //            public void update_time(SqlCmd sqlcmd, double time)
        //            {
        //                this.update_time(sqlcmd, string.Format("update log_config set _Time=dateadd(ms,{1},_Time) where _Key='{0}'", this.Key, time));
        //            }

        //            public void update_time(SqlCmd sqlcmd, string sql)
        //            {
        //                lock (_top.all)
        //                {
        //                    if ((sql != null) || (this.ReadElapsed.TotalMilliseconds > this.End) || (this.ReadElapsed.TotalMilliseconds < 0))
        //                    {
        //                        using (DB.Open(DB.Name.Log, DB.Access.Read, out sqlcmd, sqlcmd))
        //                        {
        //                            try
        //                            {
        //                                this.m_ReadTime = this.SqlTime = DateTime.Now;
        //                                if (sql != null) sqlcmd.BeginTransaction();
        //                                foreach (SqlDataReader r in sqlcmd.ExecuteReader2("{0}select getdate() as ct, * from log_config nolock", sql))
        //                                {
        //                                    item item;
        //                                    if (_top.all.TryGetValue(r.GetString("_Key"), out item))
        //                                        r.FillObject(item);
        //                                }
        //                                if (sql != null) sqlcmd.Commit();
        //                            }
        //                            catch
        //                            {
        //                                if (sql != null) sqlcmd.Rollback();
        //                                this.m_ReadTime = this.SqlTime = DateTime.Now;
        //                                throw;
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            public override string ToString()
        //            {
        //                return this.Time.ToString(sqltool.DateTimeFormat) + ", " + this.State;
        //            }
        //        }
    }
}



//bool Init
//{
//    get { return Interlocked.CompareExchange(ref this. }
//}

//public void config_import(SqlCmd sqlcmd, string _key)
//{
//    if (string.IsNullOrEmpty(_key))
//        _key = "[Key]";
//    else
//        _key = string.Format("'{0}'", _key);
//    foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select getdate() as ct, * from config nolock where [Key]={0}", _key))
//    {
//        GameID gameID = (GameID)r.GetInt32("GameID");
//        string key = r.GetString("Key");
//        config item;
//        if (this.items2.TryGetItem(gameID, key, out item))
//        {
//        }
//        else
//        {
//            config item_n = new config(gameID, key);
//        }
//        foreach (config item_ in item.SyncLock())
//        {
//            r.FillObject(item);
//            Interlocked.Exchange(ref this.m_SqlTime, item.CurrentTime);
//            Interlocked.Exchange(ref this.m_SqlReadTime, DateTime.Now);
//        }
//    }
//}

//public void config_import(SqlCmd sqlcmd, bool create, string format, params object[] args)
//{
//    foreach (SqlDataReader r in sqlcmd.ExecuteReader2(format ?? "select getdate() as ct, * from config nolock", args))
//    {
//        GameID _gameID = (GameID)r.GetInt32("GameID");
//        string _key = r.GetString("Key");
//        config item;
//        if (!this.items.TryGetItem(_gameID, _key, out item))
//        {
//            if (create)
//            {
//                if (_gameID == GameID.BBIN)
//                {
//                    bbin.gamekind? gamekind = _key.Substring(0, 2).ToEnum<bbin.gamekind>();
//                    bbin.gametype? gametype = _key.Substring(3).ToEnum<bbin.gametype>();
//                    if (gamekind.HasValue) item = new config_bbin(this.items_bbin, gamekind.Value, gametype);
//                }
//                else
//                    item = new config(_gameID, _key, null);
//                this.items.Add(item);
//            }
//            else continue;
//        }
//        foreach (config item_ in item.SyncLock())
//            r.FillObject(item);
//        if (item.CurrentTime.HasValue)
//            this.SqlReadTime = DateTime.Now;
//    }
//}

//        bool init()
//        {
//            if (Monitor.TryEnter(this.items))
//            {
//                try
//                {
//                    using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite))
//                    {
//                        //sqlcmd.ExecuteNonQuery("truncate table config");
//                        config_import(sqlcmd, true, null);
//                        StringBuilder sql_init = new StringBuilder();
//                        foreach (config item in items)
//                        {
//                            if (item.CurrentTime.HasValue) continue;
//                            sql_init.Append(@"
//insert into config (GameID,[Key],Active,Time,MinLen,MaxLen,Reserved) values ({GameID},{Key},{Active},{Time:yyyy-MM-dd HH:mm:ss},{MinLen},{MaxLen},{Reserved})".SqlExport(null, item));
//                        }
//                        if (sql_init.Length > 0)
//                        {
//                            try
//                            {
//                                sqlcmd.BeginTransaction();
//                                sqlcmd.ExecuteNonQuery(sql_init.ToString());
//                                config_import(sqlcmd, false, null);
//                                sqlcmd.Commit();
//                            }
//                            catch
//                            {
//                                sqlcmd.Rollback();
//                                throw;
//                            }
//                        }
//                    }
//                    Thread.Sleep(3000);
//                    Tick.OnTick += tick;
//                }
//                catch (Exception ex)
//                {
//                    log.error_msg(ex);
//                }
//                finally { Monitor.Exit(this.items); }
//            }
//            return false;
//        }


//class config_BetAmtDG : config
//{
//}

//using (StreamReader sr = new StreamReader("1.txt"))
//{
//    for (string n = sr.ReadLine(); n != null; n = sr.ReadLine())
//    {
//        try
//        {
//            // 0         1         2         3         4         5         6         7
//            // 01234567890123456789012345678901234567890123456789012345678901234567890
//            // drwxr-xr-x   2 root     root        28672 Feb 28 15:58 .
//            // -rw-r--r--   1 root     root        12661 Feb 28 14:48 201402280685.xml
//            FileInfo file = new FileInfo(n.Substring(55));
//            log.message(null, "{0}\t{1}", file.Name, file.LastWriteTime);
//            DateTime time;
//            // n.Substring(42,12), "MMM dd HH mm"
//            DateTime.TryParseExact(n.Substring(42, 12), "MMM dd HH mm", null, System.Globalization.DateTimeStyles.AdjustToUniversal, out time);
//        }
//        catch { }
//    }
//}
//return true;

//using (FtpConnection ftp = new FtpConnection(api.ftp_url, api.ftp_user, api.ftp_password))
//{
//    log.message("ag", "Connecting to {0}...", ftp.Host);
//    ftp.Open();
//    log.message("ag", "login...");
//    ftp.Login();
//    for (DateTime time = item.StartTime; ; time = item.EndTime)
//    {
//        try { ftp.SetCurrentDirectory(string.Format("/{0:yyyyMMdd}", time)); }
//        catch { break; }
//        FtpFileInfo[] files = ftp.GetFiles("*.xml");
//        //Array.Sort(files, (FtpFileInfo a, FtpFileInfo b) => { return (int)(a.LastWriteTime.Value - b.LastWriteTime.Value).Ticks; });
//        //string path = ftp.GetCurrentDirectory();
//        //FtpDirectoryInfo[] dirs = ftp.GetDirectories(string.Format("/{0:yyyyMMdd}/", item.StartTime));
//        for (int i = 0; i < files.Length; i++)
//        {
//            FtpFileInfo file = files[i];
//            DateTime filetime;
//            if (file.LastWriteTime.HasValue)
//                filetime = file.LastWriteTime.Value.Add(_ag_ftp_time);
//            else
//                filetime = time.Date;
//            //log.message("ag", "{0} {1:yyyy/MM/dd HH:mm:ss}", files[i].Name, filetime);
//            int cnt = this.LogDB.ExecuteNonQuery("select count(*) from ag_ftp where path='{0:yyyyMMdd}' and [name]='{1}'", time, file.Name);
//            if (cnt == 0)
//            {
//                //ftp.GetFile(
//            }
//        }
//        if (time.Date == item.EndTime.Date)
//            break;
//    }
//}
//item.EndTime = DateTime.Now;


//_ag_getdata_DownloadFile(item, item.StartTime.Date);
//if (item.StartTime.Date != item.EndTime.Date)
//    _ag_getdata_DownloadFile(item, item.EndTime.Date);


//Debugger.Break();
/*
drwxr-xr-x  33 root     root         4096 Mar  1 14:18 .
drwxr-xr-x  33 root     root         4096 Mar  1 14:18 ..
-rw-r--r--   1 b43      b43            18 Feb 21  2013 .bash_logout
-rw-r--r--   1 b43      b43           176 Feb 21  2013 .bash_profile
-rw-r--r--   1 b43      b43           124 Feb 21  2013 .bashrc
drwxr-xr-x   2 root     root         4096 Aug 12  2013 .gnome2
drwxr-xr-x   4 root     root         4096 Aug 12  2013 .mozilla
drwxr-xr-x   2 root     root        32768 Feb  1 15:59 20140201
drwxr-xr-x   2 root     root        36864 Feb  2 15:59 20140202
drwxr-xr-x   2 root     root        36864 Feb  3 15:59 20140203
drwxr-xr-x   2 root     root        28672 Feb  4 15:59 20140204
drwxr-xr-x   2 root     root        36864 Feb  5 15:58 20140205
drwxr-xr-x   2 root     root        36864 Feb  6 15:58 20140206
drwxr-xr-x   2 root     root        28672 Feb  7 15:59 20140207
drwxr-xr-x   2 root     root        32768 Feb  8 15:59 20140208
drwxr-xr-x   2 root     root        32768 Feb  9 15:59 20140209
drwxr-xr-x   2 root     root        32768 Feb 10 15:59 20140210
drwxr-xr-x   2 root     root        28672 Feb 11 15:59 20140211
drwxr-xr-x   2 root     root        28672 Feb 12 15:57 20140212
drwxr-xr-x   2 root     root        28672 Feb 13 15:59 20140213
drwxr-xr-x   2 root     root        32768 Feb 14 15:59 20140214
drwxr-xr-x   2 root     root        32768 Feb 15 15:59 20140215
drwxr-xr-x   2 root     root        28672 Feb 16 15:59 20140216
drwxr-xr-x   2 root     root        32768 Feb 17 15:59 20140217
drwxr-xr-x   2 root     root        32768 Feb 18 15:58 20140218
drwxr-xr-x   2 root     root        32768 Feb 19 15:58 20140219
drwxr-xr-x   2 root     root        36864 Feb 20 15:58 20140220
drwxr-xr-x   2 root     root        36864 Feb 21 15:58 20140221
drwxr-xr-x   2 root     root        32768 Feb 22 15:58 20140222
drwxr-xr-x   2 root     root        32768 Feb 23 15:58 20140223
drwxr-xr-x   2 root     root        28672 Feb 24 15:58 20140224
drwxr-xr-x   2 root     root        32768 Feb 25 15:58 20140225
drwxr-xr-x   2 root     root        32768 Feb 26 15:58 20140226
drwxr-xr-x   2 root     root        36864 Feb 27 15:59 20140227
drwxr-xr-x   2 root     root        28672 Feb 28 15:58 20140228
drwxr-xr-x   2 root     root        24576 Mar  1 14:18 20140301
-rw-r--r--   1 root     root         2600 Mar  1 14:18 fileList.txt


             
             
 */
//Debugger.Break();
//item.EndTime = this.Interval.CurrentTime.Value;







//FlagFtp.FtpClient ag_ftp;
//Uri uri = new Uri(string.Format("ftp://{0}/{1}", api.ftp_url, path));
//log.message("ag", "get file list : {0}", uri);
//item.State = uri.ToString();
//List<FlagFtp.FtpFileInfo> files = new List<FlagFtp.FtpFileInfo>();
//foreach (FlagFtp.FtpFileInfo file in this.ag_ftp.GetFiles(uri))
//{
//    files.Add(file);
//    log.message("ag", "{0:yyyy/MM/dd HH:mm:ss}\t{1}", file.LastWriteTime, file.FullName);
//}
//log.message("ag", "files count : {0}", files.Count);
//foreach (SqlDataReader r in this.LogDB.ExecuteReader2("select [name] from ag_ftp nolock where path='{0}'", path))
//{
//    string name = r.GetString("name");
//    for (int i = files.Count - 1; i >= 0; i--)
//        if (string.Compare(name, files[i].Name, true) == 0)
//            files.RemoveAt(i);
//}
//foreach (FlagFtp.FtpFileInfo file in files)
//{
//    log.message("ag", "new file : {0}", file.FullName);
//    item.State = file.FullName;
//    string xml;
//    using (FlagFtp.FtpStream s1 = this.ag_ftp.OpenRead(file))
//    using (StreamReader s2 = new StreamReader(s1))
//        xml = s2.ReadToEnd();
//    ag_WriteFile(path, file.Name, xml, file.LastWriteTime);
//}
