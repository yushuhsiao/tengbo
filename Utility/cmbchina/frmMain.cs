using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Configuration;
using System.Web;

namespace cmbchina
{
    // 修改清單:

    // * 組態設定視窗

    // * 歷史紀錄查詢, 自動輸入條件

    // * api 格式:
    // http://www.xxx.com/import.aspx?src=cmb&a=1&b=2&c=3
    // 讀取回應, http body, 1=>success

    partial class frmMain : Form
    {
        [STAThread]
        static void Main()
        {
            DateTime t = new DateTime(2013, 6, 2).AddDays(59);
            log.AddWriter(new TextLogWriter());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        config config = new config();

        public frmMain()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            InitializeComponent();
            this.wb.form = this;
            this.pages.Add(this.page_login = new _page_login(this));
            this.pages.Add(this.page_genIndex = new _page_genIndex(this));
            this.pages.Add(this.page_today = new _page_today(this));
            this.pages.Add(this.page_hist = new _page_hist(this));

            this.propertyGrid1.SelectedObject = this.config.min;

            //timer.Start();
            //while (true)
            //{
            //    Thread.Sleep(1000);
            //    Debugger.Break();
            //    timer.Reset();
            //    timer.Start();
            //    Debugger.Break();
            //}
            //foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(config.instance))
            //{
            //    AppSettingAttribute a = (AppSettingAttribute)p.Attributes[typeof(AppSettingAttribute)];
            //    if (a != null)
            //    {
            //        CategoryAttribute c = (CategoryAttribute)p.Attributes[typeof(CategoryAttribute)];
            //        if (c != null)
            //        {
            //            FieldInfo f = c.GetType().GetField("categoryValue", BindingFlags.Instance | BindingFlags.NonPublic);
            //            if (f != null)
            //            {
            //                f.SetValue(c, a.SectionName ?? c.Category);
            //            }
            //        }
            //    }
            //}


            Tick.OnTick += new Tick.Handler(OnTick);
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Visible = true;
            this.MessagePanel = false;
            this.SetProgress(null, null);

            log.message(null, "Open url {0}...", this.config.cmbchina_url);
            this.wb.Navigate(this.config.cmbchina_url);

            //using (WebClient w = new WebClient())
            //{
            //    string query = string.Format("{0}?{1}", config.API_Url, new row() { _time = DateTime.Now, _out = 100, _in = 100, _bal = 100, _type = "123", _memo = "456", _name = "789" }.GetQueryString());
            //    string s = w.UploadString(query, "");
            //    log.message("post", "{0}\r\n{1}{2}", query, w.ResponseHeaders.ToString(), s);
            //}
        }


        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.Visible ^= true)
                this.BringToFront();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            //if (this.WindowState == FormWindowState.Minimized)
            //    this.Hide();
        }

        protected override void OnClosed(EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            base.OnClosed(e);
        }


        class WebBrowser2 : WebBrowser
        {
            public frmMain form;

            protected override void OnProgressChanged(WebBrowserProgressChangedEventArgs e)
            {
                base.OnProgressChanged(e);
                form.SetProgress((int)e.CurrentProgress, (int)e.MaximumProgress);
            }

            protected override void OnStatusTextChanged(EventArgs e)
            {
                base.OnStatusTextChanged(e);
                form.txtStatusText.Text = this.StatusText;
            }

            //protected override void OnFileDownload(EventArgs e)
            //{
            //    base.OnFileDownload(e);
            //    log.message("event", "OnFileDownload");
            //}

            protected override void OnNewWindow(CancelEventArgs e)
            {
                base.OnNewWindow(e);
                log.message("event", "OnNewWindow");
            }

            protected override void OnEncryptionLevelChanged(EventArgs e)
            {
                base.OnEncryptionLevelChanged(e);
                form.txtEncryptionLevel.Text = this.EncryptionLevel.ToString();
                //log.message("event", "OnEncryptionLevelChanged\t{0}", this.EncryptionLevel);
            }

            protected override void OnDocumentTitleChanged(EventArgs e)
            {
                base.OnDocumentTitleChanged(e);
                log.message("event", "OnDocumentTitleChanged\t{0}", this.DocumentTitle);
            }

            protected override void OnCanGoBackChanged(EventArgs e)
            {
                base.OnCanGoBackChanged(e);
                log.message("event", "OnDocumentTitleChanged");
            }

            protected override void OnCanGoForwardChanged(EventArgs e)
            {
                base.OnCanGoForwardChanged(e);
                log.message("event", "OnDocumentTitleChanged");
            }

            public IEnumerable<HtmlDocument> Documents
            {
                get
                {
                    if (this.Document != null)
                    {
                        yield return this.Document;
                        foreach (HtmlWindow frame in this.Document.Window.Frames)
                            if (frame.Document != null)
                                yield return frame.Document;
                    }
                }
            }

            protected override void OnNavigating(WebBrowserNavigatingEventArgs e)
            {
                base.OnNavigating(e);
                log.message("event", "OnNavigating\t{0}", e.Url);
                try
                {
                    foreach (var page in form.pages)
                        if (page.IsMatch(e.Url))
                            page.OnNavigating();
                }
                catch (Exception ex) { log.message(null, ex.ToString()); }
            }

            protected override void OnNavigated(WebBrowserNavigatedEventArgs e)
            {
                base.OnNavigated(e);
                log.message("event", "OnNavigated\t{0}", e.Url);
                form.SetProgress(null, null);
                try
                {
                    foreach (var page in form.pages)
                        if (page.IsMatch(e.Url))
                            page.OnNavigated();
                }
                catch (Exception ex) { log.message(null, ex.ToString()); }
            }

            protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
            {
                base.OnDocumentCompleted(e);
                log.message("event", "OnDocumentCompleted\t{0}", e.Url);
                try
                {
                    foreach (HtmlDocument doc in this.Documents)
                    {
                        if (doc.Url.AbsolutePath == e.Url.AbsolutePath)
                        {
                            foreach (var page in form.pages)
                                if (page.IsMatch(doc))
                                    page.OnDocumentCompleted(doc);
                        }
                    }
                }
                catch (Exception ex) { log.message(null, ex.ToString()); }
                //try
                //{
                //    lock (form.parse_docs)
                //    {
                //        if (!form.parse_docs.Contains(this.Document))
                //            form.parse_docs.Enqueue(this.Document);
                //        foreach (HtmlWindow frame in this.Document.Window.Frames)
                //            form.parse_docs.Enqueue(frame.Document);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    log.message(null, ex.ToString());
                //}
            }
        }

        void SetProgress(int? current, int? max)
        {
            try
            {
                if (current.HasValue && max.HasValue)
                {
                    if (max.Value < 0) return;
                    if (current.Value < 0) return;
                    this.loading1.Minimum = 0;
                    this.loading1.Maximum = max.Value;
                    this.loading1.Value = current.Value;
                    this.loading1.Visible = this.loading2.Visible = false;
                    this.loading2.Text = string.Format("{0}/{1}", current, max);
                }
                else
                {
                    this.loading1.Visible = this.loading2.Visible = false;
                }
                Application.DoEvents();
            }
            catch { }
        }

        private void cmdMsg_Click(object sender, EventArgs e)
        {
            this.MessagePanel ^= true;
        }

        public bool MessagePanel
        {
            get { return this.cmdMsg.Checked; }
            set
            {
                this.cmdMsg.Checked = value;
                this.splitContainer1.Panel2Collapsed = !value;
            }
        }

        readonly DateTime time_start = DateTime.Now;

        readonly row.list rows = new row.list();

        //bool isLogin;

        //HtmlElement btnOK;
        //DateTime? btnOK_time;

        //Dictionary<forms, HtmlElement> forms1 = new Dictionary<forms, HtmlElement>();
        //Dictionary<forms, HtmlElement> forms2 = new Dictionary<forms, HtmlElement>();

        //private void OnTick2(object sender, EventArgs eventArgs)
        //{
        //    if (timer1.Tag != null) return;
        //    try
        //    {
        //        timer1.Tag = timer1;

        //        #region parseTransRecSet

        //        HtmlDocument parse_doc = null;
        //        if (Monitor.TryEnter(this.parse_docs))
        //        {
        //            try
        //            {
        //                if (this.parse_docs.Count > 0)
        //                    parse_doc = this.parse_docs.Dequeue();
        //            }
        //            finally { Monitor.Exit(this.parse_docs); }
        //        }
        //        if (this.config.today_parse)
        //            rows.parse(this, parse_doc, this.config.today_TransRecSet);
        //        if (this.config.hist_parse)
        //            rows.parse(this, parse_doc, this.config.hist_TransRecSet);

        //        #endregion

        //        txtState.Text = wb.ReadyState.ToString();
        //        if (wb.ReadyState != WebBrowserReadyState.Complete)
        //            return;
        //        if (wb.Document == null)
        //            return;
        //        forms2.Clear();

        //        foreach (HtmlElement e in wb.Document.Forms)
        //        {
        //            if (e.Id == this.config.forms_LoginForm)
        //                forms2[forms.LoginForm] = e;
        //            else if (e.Id == this.config.forms_TheForm)
        //                forms2[forms.TheForm] = e;
        //            else if (e.Id == this.config.forms_GenForm)
        //                forms2[forms.GenForm] = e;
        //            else if (e.Id == this.config.forms_am_QueryHistoryTrans)
        //                forms2[forms.am_QueryHistoryTrans] = e;
        //            else if (e.Id == this.config.forms_am_QueryTodayTrans)
        //                forms2[forms.am_QueryTodayTrans] = e;
        //        }
        //        forms f1 = default(forms);
        //        foreach (forms f2 in forms2.Keys)
        //            f1 |= f2;
        //        txtState.Text = f1.ToString();
                
        //        bool isLogin;
        //        if (forms2.ContainsKey(forms.LoginForm))
        //            isLogin = false;
        //        else
        //            isLogin = forms2.ContainsKey(forms.TheForm) && forms2.ContainsKey(forms.GenForm);
        //        if (loginTime.HasValue != isLogin)
        //        {
        //            if (isLogin)
        //                loginTime = DateTime.Now;
        //            else
        //                loginTime = null;
        //            log.message("login", "{0}", isLogin);
        //        }
        //        if (forms2.ContainsKey(forms.TheForm) && forms2.ContainsKey(forms.GenForm))
        //        {
        //            if (forms2.ContainsKey(forms.am_QueryTodayTrans))
        //            {
        //            }
        //            if (forms2.ContainsKey(forms.am_QueryHistoryTrans))
        //            {
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.message(null, ex.ToString());
        //    }
        //    finally { timer1.Tag = null; }
        //}


        Stopwatch html_click_time = new Stopwatch();
        bool html_click(HtmlElement e)
        {
            try
            {
                if ((html_click_time.ElapsedMilliseconds > 3000) || (!html_click_time.IsRunning))
                {
                    html_click_time.Reset();
                    html_click_time.Start();
                    e.InvokeMember("click");
                    log.message("click", e.OuterHtml);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.message("click", ex.ToString());
            }
            return false;
        }


        //void update_today()
        //{
        //    if (!this.config.today_Update)
        //        return;
        //    if (time_today.HasValue)
        //        if ((DateTime.Now - time_today.Value).TotalMilliseconds < this.config.today_Interval)
        //            return;
        //    if (this.config.hist_Update)
        //    {
        //        if (time_history.HasValue)
        //            if ((DateTime.Now - time_history.Value).TotalMilliseconds < this.config.hist_Interval)
        //                return;
        //    }
        //}
        //void update_hist()
        //{
        //}

        //DateTime? time_today;
        //DateTime? time_history;
        //DateTime? click_today;
        //DateTime? click_history;
        //HtmlWindow frame_today = null;
        //HtmlWindow frame_history = null;

        //IEnumerable<HtmlDocument> EnumFrames()
        //{
        //    if (wb.Document != null)
        //    {
        //        yield return wb.Document;
        //        foreach (HtmlWindow frame in wb.Document.Window.Frames)
        //            if (frame.Document != null)
        //                yield return frame.Document;
        //    }
        //}

        List<_page_base> pages = new List<_page_base>();
        _page_login page_login;
        _page_genIndex page_genIndex;
        _page_today page_today;
        _page_hist page_hist;

        abstract class _page_base
        {
            protected readonly frmMain form;
            public _page_base(frmMain form)
            {
                this.form = form;
            }
            public abstract string Url { get; }

            public virtual void Tick(EventArgs event_args) { }

            public bool IsMatch(HtmlDocument doc)
            {
                if (doc == null) return false;
                return this.IsMatch(doc.Url);
            }
            public bool IsMatch(Uri uri)
            {
                if (uri == null) return false;
                return uri.AbsolutePath.Contains(this.Url);
            }

            public IEnumerable<HtmlDocument> GetDocument()
            {
                foreach (HtmlDocument doc in form.wb.Documents)
                    if (this.IsMatch(doc))
                        yield return doc;
            }

            public Stopwatch Timer = new Stopwatch();

            public virtual void OnNavigating()
            {
                this.Timer.Stop();
            }
            public virtual void OnNavigated()
            {
                this.Timer.Stop();
            }
            public virtual void OnDocumentCompleted(HtmlDocument doc)
            {
                this.Timer.Reset();
                this.Timer.Start();
            }
        }
        class _page_login : _page_base
        {
            #region html
            //<TD vAlign=top width="50%" align=middle><INPUT style="CURSOR: pointer" id=LoginBtn src="../doc/images/login_btn1.gif" width=87 height=23 type=image> </TD>
            #endregion
            public _page_login(frmMain form) : base(form) { }
            public override string Url
            {
                get { return "Login/Login.aspx"; }
            }
            public void GetElement(HtmlDocument doc, ref HtmlElement LoginBtn)
            {
                LoginBtn = doc.GetElementById(form.config.tags_LoginBtn, "tr", "td");
            }

            //public override void OnNavigated()
            //{
            //    //form.page_genIndex.open_first += null;
            //    form.state = TickState.Login_Navigating;
            //}
            //public override void OnNavigating()
            //{
            //    //form.page_genIndex.open_first += null;
            //    form.state = TickState.Login_Navigated;
            //}
            //public override void OnDocumentCompleted(HtmlDocument doc)
            //{
            //    //form.page_genIndex.open_first += null;
            //    form.state = TickState.Login_DocumentCompleted;
            //}
        }
        class _page_genIndex : _page_base
        {
            #region html
            //<UL style="Z-INDEX: 100; LEFT: -9999em" id=ID0.31691080162377644>
            //<LI id=LI_0.693930637664254 isRoot="false" isLeaf="true"><A style="COLOR: #0783df" onclick="CallFuncEx2('A','CBANK_PB','DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM',null);" href="https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/GenShellPC/Login/GenIndex.aspx#">当天交易查询</A> </LI>
            //<LI id=LI_0.1890585453817077 isRoot="false" isLeaf="true"><A onclick="CallFuncEx2('A','CBANK_PB','DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM',null);" href="https://pbsz.ebank.cmbchina.com/CmbBank_GenShell/UI/GenShellPC/Login/GenIndex.aspx#">历史交易查询</A> </LI></UL>
            #endregion
            public _page_genIndex(frmMain form) : base(form) { }
            public override string Url
            {
                get { return "Login/GenIndex.aspx"; }
            }

            public bool GetElement(HtmlDocument doc, ref HtmlElement am_QueryTodayTrans, ref HtmlElement am_QueryHistoryTrans)
            {
                foreach (HtmlElement a in doc.GetElementsByTagName("a"))
                {
                    if (a.ParentsIs("ul", "li"))
                    {
                        string onclick = a.OuterHtml ?? ""; // a.GetAttribute("onclick") ?? "";
                        if (onclick.Contains(form.config.tags_GenIndex_QueryTodayTrans))
                            am_QueryTodayTrans = a;
                        else if (onclick.Contains(form.config.tags_GenIndex_QueryHistoryTrans))
                            am_QueryHistoryTrans = a;
                    }
                    if ((am_QueryTodayTrans != null) && (am_QueryHistoryTrans != null))
                        return true;
                }
                return false;
            }

        }
        class _page_today : _page_base
        {
            #region html
            //<TBODY>
            //<TR>
            //<TD class=tdPanelHead></TD>
            //<TD class=tdPanelSel><A class=lkPanelSel onclick="triggerFunc('../DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM','_self')" href="#">当天交易查询</A></TD>
            //<TD class=tdPanelNoSel><A class=lkPanelNoSel onclick="triggerFunc('../DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM','_self')" href="#">历史交易查询</A></TD></TR></TBODY>

            //<TR>
            //<TD class=tdLeftH30>
            //子账户：<SELECT style="WIDTH: 200px" id=ddlSubAccountList name=ddlSubAccountList> <OPTION selected value=125222485000002>活期结算户 人民币 00000</OPTION></SELECT>
            //交易类型：<SELECT style="WIDTH: 100px" id=ddlTransTypeList name=ddlTransTypeList> <OPTION selected value=->[ 全部 ]</OPTION> <OPTION value=ICRR>客户转账</OPTION></SELECT>
            //<INPUT id=BtnOK class=btn value="查 询" type=submit name=BtnOK> </TD></TR></TBODY></TABLE></TD></TR>
            #endregion
            public _page_today(frmMain form) : base(form) { }
            public virtual bool Enabled
            {
                get { return form.config.today_Update; }
            }
            public override string Url
            {
                get { return "DebitCard_AccountManager/am_QueryTodayTrans.aspx"; }
            }
            public virtual long Interval
            {
                get { return form.config.today_Interval; }
            }
            protected virtual bool ParseDoc
            {
                get { return form.config.today_parse; }
            }
            protected virtual string TransRecSet
            {
                get { return form.config.today_TransRecSet; }
            }
            protected virtual string BtnOK
            {
                get { return form.config.today_BtnOK; }
            }
            protected virtual string QueryTodayTrans
            {
                get { return form.config.today_QueryTodayTrans; }
            }
            protected virtual string QueryHistoryTrans
            {
                get { return form.config.today_QueryHistoryTrans; }
            }

            public bool GetElement(HtmlDocument doc, ref HtmlElement am_QueryTodayTrans, ref HtmlElement am_QueryHistoryTrans, ref HtmlElement BtnOK)
            {
                BtnOK = doc.GetElementById(this.BtnOK, "tr", "td");
                foreach (HtmlElement a in doc.GetElementsByTagName("a"))
                {
                    if (a.ParentsIs("tr", "td"))
                    {
                        string onclick = a.OuterHtml ?? ""; // a.GetAttribute("onclick") ?? "";
                        if (onclick.Contains(this.QueryTodayTrans ))
                            am_QueryTodayTrans = a;
                        else if (onclick.Contains(this.QueryHistoryTrans))
                            am_QueryHistoryTrans = a;
                    }
                    if ((am_QueryTodayTrans != null) && (am_QueryHistoryTrans != null))
                        return BtnOK != null;
                }
                return false;
            }


            public override void OnDocumentCompleted(HtmlDocument doc)
            {
                if (this.ParseDoc)
                    form.rows.parse(form, doc, this.TransRecSet);
                base.OnDocumentCompleted(doc);
            }

        }
        class _page_hist : _page_today
        {
            #region html
            //<TBODY>
            //<TR>
            //<TD class=tdPanelHead></TD>
            //<TD class=tdPanelNoSel><A class=lkPanelNoSel onclick="triggerFunc('../DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM','_self')" href="#">当天交易查询</A></TD>
            //<TD class=tdPanelSel><A class=lkPanelSel onclick="triggerFunc('../DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM','_self')" href="#">历史交易查询</A></TD></TR></TBODY>

            //<TR>
            //<TD class=tdLeftH30>
            //起始日期：<INPUT style="WIDTH: 80px" id=BeginDate value=20130401 maxLength=8 name=BeginDate>
            //<IMG style="CURSOR: pointer" id=BtnBeginDate onclick=openWinBeginDateSel() src="../doc/Images/bt_selDate.gif">
            //终止日期：<INPUT style="WIDTH: 80px" id=EndDate value=20130425 maxLength=8 name=EndDate>
            //<IMG style="CURSOR: pointer" id=BtnEndDate onclick=openWinEndDateSel() src="../doc/Images/bt_selDate.gif">
            //<INPUT id=BtnOK class=btn onclick="return CheckValid();" value="查 询" type=submit name=BtnOK> </TD></TR></TBODY></TABLE></TD></TR>
            #endregion
            public _page_hist(frmMain form) : base(form) { }
            public override bool Enabled
            {
                get { return form.config.hist_Update; }
            }
            public override string Url
            {
                get { return "DebitCard_AccountManager/am_QueryHistoryTrans.aspx"; }
            }
            public override long Interval
            {
                get { return form.config.hist_Interval; }
            }
            protected override bool ParseDoc
            {
                get { return form.config.hist_parse; }
            }
            protected override string TransRecSet
            {
                get { return form.config.hist_TransRecSet; }
            }
            protected override string BtnOK
            {
                get { return form.config.hist_BtnOK; }
            }
            protected override string QueryTodayTrans
            {
                get { return form.config.hist_QueryTodayTrans; }
            }
            protected override string QueryHistoryTrans
            {
                get { return form.config.hist_QueryHistoryTrans; }
            }
            public int Range
            {
                get { return form.config.hist_Range; }
            }

            public bool GetElement(HtmlDocument doc, ref HtmlElement am_QueryTodayTrans, ref HtmlElement am_QueryHistoryTrans, ref HtmlElement BtnOK, ref HtmlElement BeginDate, ref HtmlElement EndDate)
            {
                if (GetElement(doc, ref am_QueryTodayTrans, ref am_QueryHistoryTrans, ref BtnOK))
                {
                    BeginDate = doc.GetElementById(form.config.hist_BeginDate, "tr", "td");
                    EndDate = doc.GetElementById(form.config.hist_EndDate, "tr", "td");
                    return (BeginDate != null) && (EndDate != null);
                }
                return false;
            }
            public void SetDate(HtmlElement BeginDate, HtmlElement EndDate)
            {
                DateTime t = DateTime.Now;
                BeginDate.SetAttribute("value", t.AddDays(-Range).ToString("yyyyMMdd"));
                EndDate.SetAttribute("value", t.ToString("yyyyMMdd"));
            }

            public int step = 0;
        }
        //enum NavigateStates
        //{
        //    Navigating, Navigated, DocumentCompleted, DocumentCompleted_1
        //}
        //enum NavigateType
        //{
        //    Today, History,
        //}
        //enum TickState
        //{
        //    Unknown,
        //    Login_Navigating, Login_Navigated, Login_DocumentCompleted,
        //    GenIndex_Navigating, GenIndex_Navigated, GenIndex_DocumentCompleted_, GenIndex_DocumentCompleted, GenIndex_Idle,
        //    Today_Navigating, Today_Navigated, Today_DocumentCompleted_, Today_DocumentCompleted, Today_Idle,
        //    History_Navigating, History_Navigated, History_DocumentCompleted_, History_DocumentCompleted, History_Idle,
        //}
        //TickState current_state = TickState.Unknown;
        //TickState state = TickState.Unknown;
        //Stopwatch timer = new Stopwatch();

        private void OnTick(out TimeSpan? t)
        {
            t = null;
            bool doc0 = false, doc1 = false, doc2 = false;
            HtmlElement
                btnLogin = null,
                today0 = null, hist0 = null,
                today1 = null, hist1 = null, btnOK1 = null,
                today2 = null, hist2 = null, btnOK2 = null, beginDate = null, endDate = null;
            foreach (HtmlDocument doc in wb.Documents)
            {
                string url = doc.Url.AbsolutePath ?? "";
                if (url.Contains(page_login.Url))
                {
                    page_login.GetElement(doc, ref btnLogin);
                    return;
                }
                else if (url.Contains(page_genIndex.Url))
                    doc0 = page_genIndex.GetElement(doc, ref today0, ref hist0);
                else if (url.Contains(page_today.Url))
                    doc1 = page_today.GetElement(doc, ref today1, ref hist1, ref btnOK1);
                else if (url.Contains(page_hist.Url))
                    doc2 = page_hist.GetElement(doc, ref today2, ref hist2, ref btnOK2, ref beginDate, ref endDate);
            }

            if (doc0)
            {
                if (page_genIndex.Timer.IsRunning)
                {
                    t = page_genIndex.Timer.Elapsed;
                    if (page_genIndex.Timer.ElapsedMilliseconds > 3000)
                    {
                        HtmlElement click = null;
                        if (page_today.Enabled)
                            click = today0;
                        else if (page_hist.Enabled)
                            click = hist0;
                        if (click != null)
                        {
                            if (this.html_click(click))
                            {
                                page_genIndex.Timer.Stop();
                                page_hist.step = 0;
                                return;
                            }
                        }
                    }
                    return;
                }
                if (doc1)
                {
                    if (page_today.Timer.IsRunning)
                    {
                        t = page_today.Timer.Elapsed;
                        if (page_today.Timer.ElapsedMilliseconds > page_today.Interval)
                        {
                            if (page_hist.Enabled)
                            {
                                if (html_click(hist1 ?? hist0 ?? hist2 ?? btnOK2))
                                {
                                    page_today.Timer.Stop();
                                    page_hist.step = 0;
                                }
                            }
                            else if (page_today.Enabled)
                            {
                                if (html_click(btnOK1 ?? today1 ?? today0 ?? today2))
                                {
                                    page_today.Timer.Stop();
                                }
                            }
                        }
                    }
                }
                else if (doc2)
                {
                    if (page_hist.Timer.IsRunning)
                    {
                        t = page_hist.Timer.Elapsed;
                        if (page_hist.step == 0)
                        {
                            page_hist.SetDate(beginDate, endDate);
                            if (html_click(btnOK2))
                            {
                                page_hist.step = 1;
                                page_hist.Timer.Stop();
                            }
                        }
                        else
                        {
                            if (page_hist.Timer.ElapsedMilliseconds > page_hist.Interval)
                            {
                                if (page_today.Enabled)
                                {
                                    if (html_click(today2 ?? today0 ?? today1 ?? btnOK1))
                                    {
                                        page_hist.Timer.Stop();
                                    }
                                }
                                else if (page_hist.Enabled)
                                {
                                    page_hist.SetDate(beginDate, endDate);
                                    if (html_click(btnOK2))
                                    {
                                        page_hist.Timer.Stop();
                                    }
                                }
                            }
                        }
                    }
                }
            }


            //if (doc0)
            //{
            //    if (doc1)
            //    {
            //        page_hist.Timer.Stop();
            //        if (page_today.Timer.IsRunning)
            //        {
            //            if (page_today.Timer.ElapsedMilliseconds > page_today.Interval)
            //            {
            //                if (html_click(hist1 ?? hist0 ?? hist2 ?? btnOK2))
            //                    page_today.Timer.Stop();
            //            }
            //        }
            //    }
            //    else if (doc2)
            //    {
            //    }
            //    else
            //    {
            //        if (page_genIndex.Timer.IsRunning)
            //        {
            //            if (page_genIndex.Timer.ElapsedMilliseconds > 5000)
            //            {
            //                HtmlElement click = null;
            //                if (page_today.Enabled)
            //                    click = today0;
            //                else if (page_hist.Enabled)
            //                    click = hist0;
            //                if (click != null)
            //                {
            //                    if (this.html_click(click))
            //                    {
            //                        page_genIndex.Timer.Stop();
            //                        return;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //return;



            //switch (current_state)
            //{
            //    case TickState.Unknown: break;
            //    case TickState.Login_Navigating: break;
            //    case TickState.Login_Navigated: break;
            //    case TickState.Login_DocumentCompleted: break;
            //    case TickState.GenIndex_Navigating: break;
            //    case TickState.GenIndex_Navigated: break;
            //    case TickState.GenIndex_DocumentCompleted_: state = TickState.GenIndex_DocumentCompleted; break;
            //    case TickState.GenIndex_DocumentCompleted:
            //        if (timer.ElapsedMilliseconds > 5000)
            //        {
            //            if (page_today.Enabled)
            //            {
            //                if (this.html_click(today0 ?? today1 ?? today2 ?? btnOK1))
            //                    this.state = TickState.GenIndex_Idle;
            //            }
            //            else if (page_hist.Enabled)
            //            {
            //                if (this.html_click(hist0 ?? hist1 ?? hist2 ?? btnOK2))
            //                    this.state = TickState.GenIndex_Idle;
            //            }
            //            else this.state = TickState.GenIndex_Idle;
            //        }
            //        break;
            //    case TickState.GenIndex_Idle: break;
            //    case TickState.Today_Navigating: break;
            //    case TickState.Today_Navigated: break;
            //    case TickState.Today_DocumentCompleted_: state = TickState.Today_DocumentCompleted; break;
            //    case TickState.Today_DocumentCompleted:
            //        if (timer.ElapsedMilliseconds > page_today.Interval)
            //        {
            //            if (page_hist.Enabled)
            //            {
            //                if ((btnOK2 != null) && (beginDate != null) && (endDate != null))
            //                {
            //                    page_hist.SetDate(beginDate, endDate);
            //                }
            //                else
            //                {
            //                    if (this.html_click(hist1 ?? hist2 ?? hist0 ?? btnOK2))
            //                        state = TickState.Today_Idle;
            //                }
            //            }
            //            else if (page_today.Enabled)
            //            {
            //                if (this.html_click(btnOK1 ?? today1 ?? today2 ?? today0))
            //                    state = TickState.Today_Idle;
            //            }
            //            else
            //                state = TickState.Today_Idle;
            //        }
            //        break;
            //    case TickState.Today_Idle: break;
            //    case TickState.History_Navigating: break;
            //    case TickState.History_Navigated: break;
            //    case TickState.History_DocumentCompleted_: state = TickState.History_DocumentCompleted; break;
            //    case TickState.History_DocumentCompleted:
            //        if (timer.ElapsedMilliseconds > page_hist.Interval)
            //        {
            //        }
            //        break;
            //    case TickState.History_Idle: break;
            //}
        }

        private void Timer_Tick(object sender, EventArgs event_args)
        {
            if (timer1.Tag == null) timer1.Tag = timer1; else return;
            try
            {
                TimeSpan? t = null;
                try
                {
                    OnTick(out t);
                }
                finally
                {
                    //if (this.current_state != this.state)
                    //{
                    //    this.current_state = this.state;
                    //    timer.Reset();
                    //    timer.Start();
                    //}
                    //StringBuilder s = new StringBuilder();
                    //s.AppendFormat("[ {0:0.00} ]", page_genIndex.Timer.Elapsed.TotalSeconds);
                    //s.AppendFormat("[ {0:0.00} ]", page_today.Timer.Elapsed.TotalSeconds);
                    //s.AppendFormat("[ {1} {0:0.00} ]", page_hist.Timer.Elapsed.TotalSeconds, page_hist.step);
                    //txtTickStatus.Text = s.ToString();
                    string txt;
                    if (t.HasValue)
                        txt = string.Format("{0:00}:{1:00}:{2:00}", t.Value.Hours, t.Value.Minutes, t.Value.Seconds);
                    else
                        txt = "--:--:--";
                    if (txtUpdateTime.Text != txt)
                        txtUpdateTime.Text = txt;
                }
                //foreach (var page in this.pages)
                //    page.Tick_Document = null;
                //foreach (HtmlDocument doc in wb.GetDocuments())
                //    foreach (var page in this.pages)
                //        if (page.IsMatch(doc))
                //            page.Tick_Document = doc;

                //foreach (var page in this.pages)
                //    page.Tick(event_args);
            }
            catch (Exception ex)
            {
                log.message(null, ex.ToString());
            }
            finally { timer1.Tag = null; }
        }
        //private void OnTick11111(object sender, EventArgs eventArgs)
        //{
        //    if (timer1.Tag != null) return;
        //    try
        //    {
        //        #region parseTransRecSet

        //        HtmlDocument parse_doc = null;
        //        if (Monitor.TryEnter(this.parse_docs))
        //        {
        //            try
        //            {
        //                if (this.parse_docs.Count > 0)
        //                    parse_doc = this.parse_docs.Dequeue();
        //            }
        //            finally { Monitor.Exit(this.parse_docs); }
        //        }
        //        if (this.config.today_parse)
        //            rows.parse(this, parse_doc, this.config.today_TransRecSet);
        //        if (this.config.hist_parse)
        //            rows.parse(this, parse_doc, this.config.hist_TransRecSet);

        //        #endregion

        //        string txt = "";
        //        try
        //        {
        //            HtmlElement login = null;
        //            HtmlElement today0 = null, hist0 = null;
        //            HtmlElement today1 = null, hist1 = null, btnOK1 = null;
        //            HtmlElement today2 = null, hist2 = null, btnOK2 = null, BeginDate = null, EndDate = null;

        //            #region GetElement
        //            foreach (HtmlDocument doc in EnumFrames())
        //            {
        //                string url = doc.Url.AbsolutePath;
        //                if (url == null)
        //                    continue;
        //                else if (url.Contains(page_login.Url))
        //                {
        //                    page_login.GetElement(doc, ref login);
        //                    if (login != null) return;
        //                }
        //                else if (url.Contains(page_genIndex.Url))
        //                    page_genIndex.GetElement(doc, ref today0, ref hist0);
        //                else if (url.Contains(page_today.Url))
        //                    page_today.GetElement(doc, ref today1, ref hist1, ref btnOK1);
        //                else if (url.Contains(page_hist.Url))
        //                {
        //                    page_hist.GetElement(doc, ref today2, ref hist2, ref btnOK2);
        //                    page_hist.GetElement(doc, ref BeginDate, ref EndDate);
        //                }
        //            }
        //            #endregion


        //            //if (page_today.Enabled)
        //            //{
        //            //    if (page_hist.IsIdleTimeout)
        //            //    {
        //            //        if (page_today.IsIdleTimeout)
        //            //        {
        //            //            if (page_today.Opening == null)
        //            //            {
        //            //                HtmlElement click = today2 ?? today1 ?? today0;
        //            //                if (html_click(click))
        //            //                    page_today.Opening = click;
        //            //                return;
        //            //            }
        //            //        }
        //            //    }
        //            //}

        //            //if (wb.Document == null) return;
        //            //HtmlElement login_button = wb.Document.GetElementById_(config.instance.LoginBtn, null, false);
        //            //if (login_button != null)
        //            //{
        //            //    if (loginTime.HasValue)
        //            //    {
        //            //        loginTime = null;
        //            //        log.message("login", "false");
        //            //    }
        //            //    return;
        //            //}

        //            return;

        //            //HtmlElement btn_today1 = null;
        //            //HtmlElement btn_today2 = null;

        //            //HtmlElement am_QueryTodayTrans1 = null;         // menu
        //            //HtmlElement am_QueryHistoryTrans1 = null;       // menu
        //            //HtmlElement am_QueryTodayTrans2 = null;
        //            //HtmlElement am_QueryHistoryTrans2 = null;

        //            //// am_QueryTodayTrans.aspx      <INPUT id=BtnOK class=btn value="查 询" type=submit name=BtnOK>
        //            //// am_QueryHistoryTrans.aspx    <INPUT style="WIDTH: 80px" id=BeginDate value=20130401 maxLength=8 name=BeginDate>
        //            //// am_QueryHistoryTrans.aspx    <INPUT style="WIDTH: 80px" id=EndDate value=20130425 maxLength=8 name=EndDate>
        //            //// am_QueryHistoryTrans.aspx    <INPUT id=BtnOK class=btn onclick="return CheckValid();" value="查 询" type=submit name=BtnOK>

        //            //foreach (HtmlElement e in wb.Document.All)
        //            //{
        //            //    if (e.TagNameIs("a"))
        //            //    {
        //            //        if (e.Parent.TagNameIs("li"))
        //            //        {
        //            //            if (e.Parent.Parent.TagNameIs("ul"))
        //            //            {
        //            //                string onclick = e.GetAttribute("onclick");
        //            //                if (onclick == "CallFuncEx2('A','CBANK_PB','DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM',null);")
        //            //                    am_QueryTodayTrans1 = e;
        //            //                else if (onclick == "CallFuncEx2('A','CBANK_PB','DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM',null);")
        //            //                    am_QueryHistoryTrans1 = e;
        //            //                if ((am_QueryTodayTrans1 != null) && (am_QueryHistoryTrans1 != null))
        //            //                    break;
        //            //            }
        //            //        }
        //            //    }
        //            //}
        //            //foreach (HtmlWindow frame in wb.Document.Window.Frames)
        //            //{
        //            //    foreach (HtmlElement e in wb.Document.All)
        //            //    {
        //            //        if (e.TagNameIs("a"))
        //            //        {
        //            //            if (e.Parent.TagNameIs("td"))
        //            //            {
        //            //                if (e.Parent.TagNameIs("tr"))
        //            //                {
        //            //                    string onclick = e.GetAttribute("onclick");
        //            //                    if (onclick == "triggerFunc('../DebitCard_AccountManager/am_QueryTodayTrans.aspx','FORM','_self')")
        //            //                        am_QueryTodayTrans2 = e;
        //            //                    if (onclick == "triggerFunc('../DebitCard_AccountManager/am_QueryHistoryTrans.aspx','FORM','_self')")
        //            //                        am_QueryHistoryTrans2 = e;
        //            //                }
        //            //            }
        //            //        }
        //            //        else if (e.TagNameIs("input"))
        //            //        {
        //            //        }
        //            //    }
        //            //}

        //            //if (config.instance.today_Update)
        //            //{
        //            //    if (!click_today.HasValue)
        //            //    {
        //            //        if (html_click(btn_today2 ?? btn_today1))
        //            //        {
        //            //            click_today = DateTime.Now;
        //            //            frame_today = null;
        //            //        }
        //            //        return;
        //            //    }
        //            //    // verify load complete
        //            //    if (!time_today.HasValue)
        //            //    {
        //            //        // click today;
        //            //        return;
        //            //    }
        //            //}


        //            foreach (HtmlWindow frame in wb.Document.Window.Frames)
        //            {
        //            }

        //            if (this.config.today_Update)
        //            {
        //            }
        //            if (this.config.hist_Update)
        //            {
        //            }
        //        }
        //        finally
        //        {
        //            if (this.txtUpdateTime.Text != txt)
        //                this.txtUpdateTime.Text = txt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.message(null, ex.ToString());
        //    }
        //    finally { timer1.Tag = null; }
        //}
        //private void OnTick1(object sender, EventArgs e)
        //{
        //    if (timer1.Tag != null) return;
        //    try
        //    {

        //        #region parseTransRecSet

        //        HtmlDocument parse_doc = null;
        //        if (Monitor.TryEnter(this.parse_docs))
        //        {
        //            try
        //            {
        //                if (this.parse_docs.Count > 0)
        //                    parse_doc = this.parse_docs.Dequeue();
        //            }
        //            finally { Monitor.Exit(this.parse_docs); }
        //        }
        //        if (this.config.today_parse)
        //            rows.parse(this, parse_doc, this.config.today_TransRecSet);
        //        if (this.config.hist_parse)
        //            rows.parse(this, parse_doc, this.config.hist_TransRecSet);

        //        #endregion

        //        string txt = "";
        //        try
        //        {
        //            if (this.wb.Document.GetElementById_(this.config.LoginBtn, null, false) != null)
        //            {
        //                if (isLogin)
        //                {
        //                    isLogin = false;
        //                    log.message("login", "false");
        //                }
        //                return;
        //            }

        //            HtmlElement am_QueryHistoryTrans = get_GenIndex(this.config.today_authName, this.config.today_funcName);
        //            if (am_QueryHistoryTrans != null)
        //            {
        //                txt = "--:--:--";
        //                if (!isLogin)
        //                {
        //                    am_QueryHistoryTrans.InvokeMember("click");
        //                    isLogin = true;
        //                    log.message("login", "true");
        //                    return;
        //                }
        //            }

        //            if (isLogin)
        //            {
        //                HtmlElement btnOK = wb.Document.GetElementById_(this.config.BtnOK, this.config.today_url, true);
        //                if (this.btnOK != btnOK)
        //                {
        //                    if (btnOK == null)
        //                        this.btnOK_time = null;
        //                    else
        //                        this.btnOK_time = DateTime.Now;
        //                    this.btnOK = btnOK;
        //                }
        //                if (this.btnOK_time.HasValue)
        //                {
        //                    TimeSpan t2 = DateTime.Now - this.btnOK_time.Value;
        //                    txt = string.Format("{0:00}:{1:00}:{2:00}", t2.Hours, t2.Minutes, t2.Seconds);
        //                    if (t2.TotalMilliseconds > this.config.Interval)
        //                    {
        //                        this.btnOK_time = null;
        //                        btnOK.InvokeMember("click");
        //                    }
        //                }
        //            }



        //            //if (am_QueryHistoryTrans == null)
        //            //{
        //            //}
        //            //else
        //            //{
        //            //    HtmlElement aa = null;
        //            //    if (isLogin)
        //            //    {
        //            //        if (update_1.HasValue)
        //            //        {
        //            //            TimeSpan t2 = DateTime.Now - update_1.Value;
        //            //            txt = string.Format("{0:00}:{1:00}:{2:00}", t2.Hours, t2.Minutes, t2.Seconds);
        //            //            if (t2.TotalMilliseconds > config.Interval)
        //            //                aa = am_QueryHistoryTrans;
        //            //        }
        //            //    }
        //            //    else
        //            //    {   // OnLogin
        //            //        isLogin = true;
        //            //        aa = am_QueryHistoryTrans;
        //            //    }
        //            //    if (aa != null)
        //            //    {
        //            //        aa.RaiseEvent("click");
        //            //        //aa.click();
        //            //        update_1 = null;
        //            //    }
        //            //}
        //        }
        //        finally
        //        {
        //            if (this.txtUpdateTime.Text != txt)
        //                this.txtUpdateTime.Text = txt;
        //        }
        //        //if (this.a_update == null)
        //        //{
        //        //    if (update_1.HasValue)
        //        //    {
        //        //    }
        //        //}

        //        //TimeSpan? t2 = null;
        //        //if (this.reload_1.HasValue)
        //        //{
        //        //    t2 = DateTime.Now - this.reload_1.Value;
        //        //    this.a_update = getQueryTodayTransButton();
        //        //    this.reload_1 = null;
        //        //}
        //        //if (this.a_update != null)
        //        //{
        //        //    this.a_update.click();
        //        //    this.update_1 = null;
        //        //    this.a_update = null;
        //        //}

        //        //if (config.Update)
        //        //{
        //        //    DateTime next = this.last_update.AddMilliseconds(Math.Max(10000.0, config.Interval));
        //        //    TimeSpan t2 = next - DateTime.Now;
        //        //    if (t2 < TimeSpan.Zero)
        //        //        t2 = TimeSpan.Zero;
        //        //    if (t2.TotalMilliseconds <= 0)
        //        //    {
        //        //        this.a_update = getQueryTodayTransButton();
        //        //    }
        //        //    this.txtUpdateTime.Text = ((int)t2.TotalSeconds).ToString();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        log.message(null, ex.ToString());
        //    }
        //    finally { timer1.Tag = null; }
        //}


        Queue<row> post_data = new Queue<row>();
        bool OnTick()
        {
            row row;
            if (!Monitor.TryEnter(this.post_data))
                return true;
            try
            {
                if (this.post_data.Count == 0) return true;
                row = this.post_data.Dequeue();
            }
            finally { Monitor.Exit(this.post_data); }
            row.upload(this);
            return true;
        }

        //Queue<HtmlDocument> parse_docs = new Queue<HtmlDocument>();

        class row
        {
            public class list : List<row>
            {
                int row_count;
                public void parse(frmMain form, HtmlDocument doc, string id)
                {
                    try
                    {
                        row row1 = null;
                        if (doc == null) return;
                        HtmlElement e2 = doc.GetElementById(id);
                        if (e2 == null) return;
                        string src_url = doc.Url.ToString();
                        log.message(null, "Parse {0}...", doc.Url.ToString());
                        foreach (HtmlElement e21 in e2.Children)
                        {
                            foreach (HtmlElement _tr in e21.Children)
                            {
                                if (!_tr.TagNameIs("tr")) continue;
                                //HTMLTableRowClass _tr = e22.DomElement as HTMLTableRowClass;
                                //if (_tr == null) continue;
                                if (row1 == null)
                                    row1 = new row();
                                else
                                    row1.Reset();
                                foreach (HtmlElement _td in _tr.All)
                                {
                                    if (!_td.TagNameIs("td")) continue;
                                    //HTMLTableCellClass _td = e23.DomElement as HTMLTableCellClass;
                                    //if (_td == null) continue;
                                    string s = (_td.InnerText ?? "").Trim();
                                    int? cellIndex = _td.GetAttribute("cellIndex").ToInt32();
                                    switch (cellIndex)
                                    {
                                        case 0: row1._time = s.ToDateTime(); break;
                                        case 1: row1._out = s.ToDecimal(); break;
                                        case 2: row1._in = s.ToDecimal(); break;
                                        case 3: row1._bal = s.ToDecimal(); break;
                                        case 4: row1._type = s; break;
                                        case 5: row1.set_memo(s); break;
                                    }
                                }
                                if (row1.isVaild)
                                {
                                    foreach (row row2 in this)
                                    {
                                        if (row2.compare(row1))
                                        {
                                            row1 = null;
                                            break;
                                        }
                                    }
                                    if (row1 == null) continue;
                                    if (row1._in.HasValue)
                                    {
                                        this.Add(row1);
                                        lock (form.post_data)
                                            form.post_data.Enqueue(row1);
                                        row1._url = src_url;
                                        row1._index = ++row_count;
                                        row1.CreateView(form.view);
                                        form.tabPage2.Text = string.Format("Data ({0})", form.view.Rows.Count);
                                        row1 = null;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.message("error", ex.ToString());
                    }
                }
            }

            /*
            2013-03-05 04:08:49.46	data	0	0	交易日期	交易日期
            2013-03-05 04:08:49.46	data	0	1	支出	支出
            2013-03-05 04:08:49.46	data	0	2	存入	存入
            2013-03-05 04:08:49.46	data	0	3	余额	余额
            2013-03-05 04:08:49.46	data	0	4	交易类型	交易类型
            2013-03-05 04:08:49.46	data	0	5	交易备注	交易备注
            
            2013-03-05 04:08:49.46	data	5	0	2013-03-02	2013-03-02
            2013-03-05 04:08:49.46	data	5	1	&nbsp;	 
            2013-03-05 04:08:49.46	data	5	2	4,995.96	4,995.96
            2013-03-05 04:08:49.46	data	5	3	28,072.74	28,072.74
            2013-03-05 04:08:49.46	data	5	4	客户转账	客户转账
            2013-03-05 04:08:49.46	data	5	5	网银贷记接收收款 赖秋坛	网银贷记接收收款 赖秋坛
            */
            public string _url;
            public int _index;
            public DateTime? _time;
            public decimal? _out;
            public decimal? _in;
            public decimal? _bal;
            public string _type;
            public string _memo;
            public string _name;
            public DataGridViewRow view;

            string encode(string prefix, string key, object value)
            {
                return string.Format("{0}{1}={2}", prefix, HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(Convert.ToString(value)));
            }

            public void upload(frmMain form)
            {
                using (WebClient w = new WebClient())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(form.config.Post_Url);
                    //StringExportAttribute.Export("?src=cmbchina&_time={time}&_out={out}&_in={in}&_bal={bal}&_type={type}&_memo={memo}&_name={name}", null, this, false);
                    query.Append(encode("?", form.config.post_id_key, form.config.post_id_value));
                    query.Append(encode("&", form.config.post_time, this._time));
                    query.Append(encode("&", form.config.post_out, this._out));
                    query.Append(encode("&", form.config.post_in, this._in));
                    query.Append(encode("&", form.config.post_bal, this._bal));
                    query.Append(encode("&", form.config.post_type, this._type));
                    query.Append(encode("&", form.config.post_memo, this._memo));
                    query.Append(encode("&", form.config.post_name, this._name));
                    //query.Append("?src=cmbchina");
                    //query.AppendFormat("&{0}={1}", "_time", HttpUtility.UrlEncode(Convert.ToString(this._time)));
                    //query.AppendFormat("&{0}={1}", "_out", HttpUtility.UrlEncode(Convert.ToString(this._out)));
                    //query.AppendFormat("&{0}={1}", "_in", HttpUtility.UrlEncode(Convert.ToString(this._in)));
                    //query.AppendFormat("&{0}={1}", "_bal", HttpUtility.UrlEncode(Convert.ToString(this._bal)));
                    //query.AppendFormat("&{0}={1}", "_type", HttpUtility.UrlEncode(Convert.ToString(this._type)));
                    //query.AppendFormat("&{0}={1}", "_memo", HttpUtility.UrlEncode(Convert.ToString(this._memo)));
                    //query.AppendFormat("&{0}={1}", "_name", HttpUtility.UrlEncode(Convert.ToString(this._name)));
                    log.message("post", "{0}", query);
                    try
                    {
                        string result = w.UploadString(query.ToString(), "");
                        log.message("post", "\r\n{0}{1}", w.ResponseHeaders.ToString(), result);
                        set_status(result);
                    }
                    catch (Exception ex)
                    {
                        set_status(ex.Message);
                        log.message("post", ex.ToString());
                    }
                }
            }

            void set_status(string status)
            {
                if (this.view != null)
                {
                    if (this.view.DataGridView.InvokeRequired)
                        this.view.DataGridView.Invoke((Action<string>)set_status, status);
                    else
                        this.view.Cells[util.col_Status].Value = status;
                }
            }

            public string GetQueryString()
            {
                StringBuilder s = new StringBuilder();
                s.AppendFormat("{0}={1}", "src", HttpUtility.UrlEncode(Convert.ToString("cmbchina")));
                s.AppendFormat("&{0}={1}", "_time", HttpUtility.UrlEncode(Convert.ToString(this._time)));
                s.AppendFormat("&{0}={1}", "_out", HttpUtility.UrlEncode(Convert.ToString(this._out)));
                s.AppendFormat("&{0}={1}", "_in", HttpUtility.UrlEncode(Convert.ToString(this._in)));
                s.AppendFormat("&{0}={1}", "_bal", HttpUtility.UrlEncode(Convert.ToString(this._bal)));
                s.AppendFormat("&{0}={1}", "_type", HttpUtility.UrlEncode(Convert.ToString(this._type)));
                s.AppendFormat("&{0}={1}", "_memo", HttpUtility.UrlEncode(Convert.ToString(this._memo)));
                s.AppendFormat("&{0}={1}", "_name", HttpUtility.UrlEncode(Convert.ToString(this._name)));
                return s.ToString();
            }

            public void CreateView(DataGridView view)
            {
                this.view = view.Rows[view.Rows.Add()];
                this.view.Cells[util.col_index].Value = this._index;
                this.view.Cells[util.col_Time].Value = this._time;
                this.view.Cells[util.col_out].Value = this._out;
                this.view.Cells[util.col_in].Value = this._in;
                this.view.Cells[util.col_bal].Value = this._bal;
                this.view.Cells[util.col_type].Value = this._type;
                this.view.Cells[util.col_memo].Value = this._memo;
                this.view.Cells[util.col_name].Value = this._name;
                this.view.Cells[util.col_srcUrl].Value = this._url;
            }

            public void set_memo(string value)
            {
                if (value != null)
                {
                    int n = value.IndexOf(' ');
                    if (n >= 0)
                    {
                        this._memo = value.Substring(0, n).Trim();
                        this._name = value.Substring(n).Trim();
                    }
                    else
                    {
                        this._memo = value;
                        this._name = "";
                    }
                }
            }

            public void Reset()
            {
                this._time = null;
                this._out = this._in = this._bal = null;
                this._type = this._memo = this._name = null;
            }

            public bool isVaild
            {
                get { return (this._time.HasValue && (this._in.HasValue || this._out.HasValue) && this._bal.HasValue); }
            }

            public bool compare(row row)
            {
                return
                    (this._time == row._time) &&
                    (this._out == row._out) &&
                    (this._in == row._in) &&
                    (this._bal == row._bal) &&
                    (this._type == row._type) &&
                    (this._memo == row._memo) &&
                    (this._name == row._name);
            }
        }

        //HtmlElement get_GenIndex(string authName, string funcName)
        //{
        //    try
        //    {
        //        if (this.wb.Document != null)
        //        {
        //            string _authName = string.Format("'{0}'", authName);
        //            string _funcName = string.Format("'{0}'", funcName);
        //            foreach (HtmlElement e in this.wb.Document.All)
        //            {
        //                if (!e.TagNameIs("a")) continue;
        //                if (e.Parent == null) continue;
        //                if (!e.Parent.TagNameIs("li")) continue;
        //                if (e.Parent.Parent == null) continue;
        //                if (!e.Parent.Parent.TagNameIs("ul")) continue;
        //                if (e.OuterHtml.IndexOf(_authName) < 0) continue;
        //                if (e.OuterHtml.IndexOf(_funcName) < 0) continue;
        //                return e;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.message("error", ex.ToString());
        //    }
        //    return null;
        //}

        private void btnConfigReload_Click(object sender, EventArgs e)
        {
            app.config.Load(this.config);
            this.propertyGrid1.Refresh();
        }

        private void btnConfigSave_Click(object sender, EventArgs e)
        {
            //foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(config.instance))
            //{
            //    p.AddValueChanged(config.instance, (sender2, e2) =>
            //    {
            //        Debugger.Break();
            //    });
            ////    FieldInfo f;
            ////    BrowsableAttribute a = (BrowsableAttribute)p.Attributes[typeof(BrowsableAttribute)];
            ////    f = a.GetType().GetField("browsable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            ////    f.SetValue(a, true);
            ////    DisplayNameAttribute d = (DisplayNameAttribute)p.Attributes[typeof(DisplayNameAttribute)];
            ////    f = d.GetType().GetField("_displayName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            ////    f.SetValue(d, "");
            //}
            app.config.Save(this.config);
            this.propertyGrid1.Refresh();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = checkBox1.Checked ? this.config : this.config.min;
        }
    }

}