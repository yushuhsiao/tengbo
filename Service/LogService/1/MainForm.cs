using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using extAPI.hg;
using Tools;
using System.Data.SqlClient;
using BU;
using System.Diagnostics;
using System.Reflection;
using web;
using Newtonsoft.Json;
using System.Data.Common;

namespace service
{
    public partial class MainForm : Form
    {
        //hg_proc proc = new hg_proc();

        public MainForm()
        {
            Tick.OnTick += Tick_OnTick;
            InitializeComponent();
            //this.propertyGrid1.SelectedObject = this.proc;
            log.message(null, "init complete");

            this.Interval = this.dataGridView1.AddRow("Interval");
            this.dataGridView1.AddRow("Interval1");
            Tick.OnTick += init;
        }

        gridRow Interval;

        bool init()
        {
            //using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.Read))
            //{
            //}
            return false;
        }

        bool Tick_OnTick()
        {
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.propertyGrid1.Refresh();
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
    }

    #region grid view

    class gridView : DataGridView
    {
        public gridView()
        {
            this.DataError += DataGridView2_DataError;
        }

        void DataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        public gridRow AddRow(string key)
        {
            if (this.ColumnCount == 0)
            {
                #region init columns
                this.Columns.Add(new gridTextBoxColumn()
                {
                    Name = "Key",
                    HeaderText = "Key",
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    ReadOnly = true,
                });
                this.Columns.Add(new gridCheckBoxColumn()
                {
                    Name = "_Active",
                    HeaderText = "Active",
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                });
                this.Columns.Add(new gridTextBoxColumn()
                {
                    Name = "Time",
                    HeaderText = "Time",
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    ReadOnly = true,
                });
                this.Columns.Add(new gridTextBoxColumn()
                {
                    Name = "Len",
                    HeaderText = "Len",
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    ReadOnly = true,
                });
                this.Columns.Add(new gridTextBoxColumn()
                {
                    Name = "Reserved",
                    HeaderText = "Reserved",
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    ReadOnly = true,
                });
                #endregion
                this.RowTemplate = _null<gridRow>.value;
            }
            gridRow row = new gridRow() { Key = key };
            base.Rows.Add(row);
            return row;
        }
    }

    static class Templates<T> where T : new() { public static T Template = new T(); }
    class gridRow : DataGridViewRow
    {
        [DbImport("Key")]
        public string Key { get; set; }
        [DbImport("Active")]
        public Locked Active { get; set; }
        protected bool _Active
        {
            get { return this.Active == Locked.Active; }
            set { this.Active = value ? Locked.Active : Locked.Locked; }
        }
        [DbImport("Time"), TypeConverter(typeof(TimeTypeConverter))]
        public DateTime Time { get; set; }
        [DbImport("Len")]
        public double Len { get; set; }
        [DbImport("Reserved")]
        public double Reserved { get; set; }

        public object cellGetValue(DataGridViewCell cell, cellGetValue getValue, int rowIndex)
        {
            PropertyInfo p = typeof(gridRow).GetProperty(cell.OwningColumn.Name);
            if (p != null)
                return p.GetValue(this, null);
            return getValue(rowIndex);
        }

        public bool cellSetValue(DataGridViewCell cell, cellSetValue setValue, int rowIndex, object value)
        {
            PropertyInfo p = typeof(gridRow).GetProperty(cell.OwningColumn.Name);
            if (p != null)
                p.SetValue(this, value, null);
            return setValue(rowIndex, value);
        }
    }
    delegate object cellGetValue(int rowIndex);
    delegate bool cellSetValue(int rowIndex, object value);
    class gridCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        public override DataGridViewCell CellTemplate
        {
            get { return _null<gridCheckBoxCell>.value; }
            set { base.CellTemplate = _null<gridCheckBoxCell>.value; }
        }
    }
    class gridCheckBoxCell : DataGridViewCheckBoxCell
    {
        protected override object GetValue(int rowIndex) { return ((gridRow)this.OwningRow).cellGetValue(this, base.GetValue, rowIndex); }
        protected override bool SetValue(int rowIndex, object value) { return ((gridRow)this.OwningRow).cellSetValue(this, base.SetValue, rowIndex, value); }
    }
    class gridTextBoxColumn : DataGridViewTextBoxColumn
    {
        public override DataGridViewCell CellTemplate
        {
            get { return _null<gridTextBoxCell>.value; }
            set { base.CellTemplate = _null<gridTextBoxCell>.value; }
        }
    }
    class gridTextBoxCell : DataGridViewTextBoxCell
    {
        protected override object GetValue(int rowIndex) { return ((gridRow)this.OwningRow).cellGetValue(this, base.GetValue, rowIndex); }
        protected override bool SetValue(int rowIndex, object value) { return ((gridRow)this.OwningRow).cellSetValue(this, base.SetValue, rowIndex, value); }
    }

    #endregion

    [TypeConverter(typeof(ExpandableObjectConverter_))]
    class configRow : DataGridViewRow
    {
        [DbImport("Key")]
        public string key;
        [DbImport("Active")]
        public Locked Active { get; set; }
        [DbImport("Time"), TypeConverter(typeof(TimeTypeConverter))]
        public DateTime Time { get; set; }
        [DbImport("Len")]
        public double Len { get; set; }
        [DbImport("Reserved")]
        public double Reserved { get; set; }
    }

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
if @MemberID is not null select a.CorpID,a.ID,a.ACNT,b.ID as AgentID,b.ACNT as AgentACNT from Member a with(nolock) left join Agent b with(nolock) on a.AgentID=b.ID where a.ID=@MemberID", acnt);
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
                                s.Values["MemberID"] = member.ID;
                                s.Values["AgentID"] = member.AgentID;
                                s.Values["AgentACNT"] = member.AgentACNT;
                                s.Values["ACTime"] = r.GetDateTime("BetStartDate").AddHours(8).ToACTime().ToString("yyyy-MM-dd");
                                mainDB.BeginTransaction();
                                long? id = mainDB.ExecuteScalar<long>(s.BuildEx("select ID from GameLog_001 nolock where AccountId={AccountId} and TableId={TableId} and GameId={GameId} and BetId={BetId} and BetNo={BetNo}"));
                                if (id.HasValue)
                                {
                                    s.Values["ID"] = id;
                                    mainDB.ExecuteNonQuery(s.BuildEx("update GameLog_001 set ", src == 1 ? "BetAmount={BetAmount},TableName={TableName}" : "BetAmountAct={BetAmountAct},UserID={UserID}", " where ID={ID}"));
                                }
                                else
                                {
                                    mainDB.ExecuteNonQuery(s.BuildEx(@"insert into GameLog_001 (MemberID,CorpID,ACNT,ACTime,AgentID,AgentACNT,", sqltool._Fields, ") select ID,CorpID,ACNT,{ACTime},{AgentID},{AgentACNT},", sqltool._Values, @" from Member nolock where ID={MemberID}"));
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
insert into GameLog_BetAmtDG (ACTime,GameID,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser)
select ACTime,{0},GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(BetAmount),sum(BetAmountAct),sum(Payout), getdate(),0
from GameLog_001 where ACTime='{1:yyyy-MM-dd}' group by ACTime,GameType,CorpID,AgentID,AgentACNT,MemberID,ACNT", (int)GameID.HG, actime1);
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
        //                            s.Values["AgentID"] = member.AgentID;
        //                            s.Values["AgentACNT"] = member.AgentACNT;
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
        //                                mainDB.ExecuteNonQuery(s.BuildEx(@"insert into GameLog_001 (MemberID,CorpID,ACNT,ACTime,AgentID,AgentACNT,", sqltool._Fields, ") select ID,CorpID,ACNT,{ACTime},{AgentID},{AgentACNT},", sqltool._Values, @" from Member nolock where ID={MemberID}"));
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
        //insert into GameLog_BetAmtDG (ACTime,GameID,GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,BetAmount,BetAmountAct,Payout,CreateTime,CreateUser)
        //select ACTime,{0},GameType,CorpID,MemberID,ACNT,AgentID,AgentACNT,sum(BetAmount),sum(BetAmountAct),sum(Payout), getdate(),0
        //from GameLog_001 where ACTime='{1:yyyy-MM-dd}' group by ACTime,GameType,CorpID,AgentID,AgentACNT,MemberID,ACNT", (int)GameID.HG, actime1);
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