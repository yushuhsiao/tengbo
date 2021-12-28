using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Tools;

namespace collecReportLog
{
    public partial class collectReportLog : Form
    {
        public DataList list2 = new DataList();
        //public List<string> listOfJson = new List<string>();
        //public List<BetAmtDG_Command> list = new List<BetAmtDG_Command>();

        public collectReportLog()
        {
            InitializeComponent();
            btnConfigReload_Click(null, EventArgs.Empty);
            dgvBetAmtDG.AutoGenerateColumns = false;
            Tick.OnTick += UpdateProc;
            ConsoleLogWriter.Enabled = true;

            //Properties.Settings.Default.sec_name = "123";
            //Properties.Settings.Default.Save();
            for (int i = 0; i < 1; i++)
            {
                this.list2.AddData(new BetAmtDG_Command()
                {
                    GameID = BU.GameID.AG_AG,
                    GameType = RandomString.GetRandomString(RandomString.LowerLetter, 10),
                    ACTime = DateTime.Now.Date,
                    ACNT = "tb296896",
                    BetAmount = RandomValue.GetInt32(),
                    BetAmountAct = RandomValue.GetInt32(),
                    Payout = RandomValue.GetInt32()
                });
            }
        }

        static string[] config_keys = new string[] { 
            "sec_name", 
            "sec_id", 
            "sec_key",
            "url_Admin",
            "url_BBIN",
            "url_AG",
            "url_HG",
            "url_SunCity",
            "url_Salon",
        };

        Configuration config;
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = configGrid.Rows[e.RowIndex];
                string key = row.Cells[colConfigKey.Index].Value as string;
                string value = row.Cells[colConfigValue.Index].Value as string;
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value) ) return;
                KeyValueConfigurationElement item = config.AppSettings.Settings[key];
                if (item == null)
                {
                    item = new KeyValueConfigurationElement(key, value);
                    this.config.AppSettings.Settings.Add(item);
                }
                else
                {
                    item.Value = value;
                }
            }
            catch { }
        }

        private void btnConfigReload_Click(object sender, EventArgs e)
        {
            try
            {
                this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configGrid.Rows.Clear();
                foreach (string key in config_keys)
                {
                    KeyValueConfigurationElement item = this.config.AppSettings.Settings[key];
                    string value;
                    if (item == null) value = null; else value = item.Value;
                    configGrid.Rows.Add(key, value);
                }
            }
            catch { }
        }

        private void btnConfigSave_Click(object sender, EventArgs e)
        {
            try { this.config.Save(); }
            catch { }
        }

        //public void AddJson(BetAmtDG_Command g)
        //{
        //    string gJson = web.api.SerializeObject(g);
        //    if (listOfJson.Contains(gJson)) return;
        //    listOfJson.Add(gJson);
        //    list.Add(g);
        //}

        /// <summary>
        /// 浏览器进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wbReportPage_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            tsp_WebProcress.Visible = true;
            if ((e.CurrentProgress > 0) && (e.MaximumProgress > 0))
            {
                tsp_WebProcress.Maximum = Convert.ToInt32(e.MaximumProgress);
                tsp_WebProcress.Step = Convert.ToInt32(e.CurrentProgress);
                tsp_WebProcress.PerformStep();
            }
            else if (GetCurrentPage().ReadyState == WebBrowserReadyState.Complete)
            {
                tsp_WebProcress.Value = 0;
                tsp_WebProcress.Visible = false;
            }
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Collect_Click(object sender, EventArgs e)
        {
            try
            {
                collectWebBrowser w = this.GetCurrentPage();
                if (w == null)
                {
                    MessageBox.Show("请选择要采集的平台");
                }
                else
                {
                    w.Collect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabReportPage_Selected(object sender, TabControlEventArgs e)
        {
            try { this.GetCurrentPage().StartBrowse(); return; }
            catch { }
            if (this.tabReportPage.SelectedTab == tabPage_Upload)
            {
                if (list2.Version1 != list2.Version2)
                {
                    dgvBetAmtDG.DataSource = null;
                    dgvBetAmtDG.DataSource = new List<BetAmtDG_Command>(list2.Values);

                    int rowNumber = 1;
                    foreach (DataGridViewRow row in dgvBetAmtDG.Rows)
                    {
                        row.HeaderCell.Value = rowNumber.ToString();
                        rowNumber++;
                    }
                    dgvBetAmtDG.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                    list2.Version2 = list2.Version1;
                }
            }
        }

        collectWebBrowser GetCurrentPage()
        {
            try
            {
                TabPage p = tabReportPage.SelectedTab;
                if (p != null)
                    foreach (Control c in p.Controls)
                        if (c is collectWebBrowser)
                            return (collectWebBrowser)c;
            }
            catch { }
            return null;
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            try { GetCurrentPage().Refresh(); }
            catch { }
        }

        private void collectReportLog_Load(object sender, EventArgs e)
        {
            //进度条默认隐藏
            tsp_WebProcress.Visible = false;
            this.tabReportPage.SelectedTab = this.tabPage_Upload;

            //collect_BBIN.Url = new Uri(collect_BBIN.SiteUrl);
        }

        private void tsbDataToServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tabReportPage.SelectedTab == this.tabPage_Upload)
                {
                    if (list2.Count == 0)
                    {
                        MessageBox.Show("暂无要上传的数据");
                    }
                    else
                    {
                        this.list2.PrepareUpload();
                    }
                }
                else
                {
                    tabReportPage.SelectTab(this.tabPage_Upload);
                }
            }
            catch { }
        }

        private void dgvBetAmtDG_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("你确定要退出吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes) this.Close();
        }

        bool UpdateProc()
        {
            BetAmtDG_Command item = this.list2.GetUploadItem();
            if (item == null) return true;
            try
            {
                list2.SetState(false, "Uploading...");
                //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(DataList.AdminUrl);
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[DataList.Robot_Name] = DataList.Robot_ID;
                    string json_s = web.api.SerializeObject("GameLog_BetAmtDG_Insert", item);
                    string json_e = Crypto.RSAEncrypt(json_s, DataList.RSA_Key);
                    NameValueCollection formdata = new NameValueCollection();
                    formdata["str"] = json_s;
                    formdata["strenc"] = json_e;
                    byte[] ret = wc.UploadValues(DataList.AdminUrl, "POST", formdata);
                    string json_r = Encoding.UTF8.GetString(ret);
                    web.api.result<GameLog_BetAmtDG_Row> result = web.api.CreateResult<GameLog_BetAmtDG_Row>(json_r);
                    list2.SetState(result.Status == BU.RowErrorCode.Successed, result.Status == BU.RowErrorCode.Successed ? "OK" : result.Message);
                }
            }
            catch (Exception ex)
            {
                list2.SetState(false, ex.Message);
            }
            finally
            {
                list2.UploadComplete();
            }
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (timer1.Tag != null) return;
                timer1.Tag = timer1;
                DataGridViewCell c = dgvBetAmtDG.FirstDisplayedCell;
                for (int i = c.RowIndex; i < dgvBetAmtDG.Rows.Count; i++)
                {
                    if (dgvBetAmtDG.Rows[i].Displayed)
                    {
                        dgvBetAmtDG.UpdateCellValue(isUpdateLoad.Index, i);
                        dgvBetAmtDG.UpdateCellValue(colStatus.Index, i);
                    }
                }
            }
            catch { }
            finally
            {
                timer1.Tag = null;
            }
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (list2.Count > 0)
                {
                    DialogResult dr = MessageBox.Show("您确定要清除所有数据吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        dgvBetAmtDG.DataSource = null;
                        list2.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("暂无要清理的数据");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class DataList : Dictionary<string, BetAmtDG_Command>
    {
        public int Version1 = 0;
        public int Version2 = 0;
        public void AddData(BetAmtDG_Command g)
        {
            this[g.json] = g;
            unchecked { this.Version1++; }
        }

        [AppSetting("url_Admin")]
        public static string AdminUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [AppSetting("sec_key")]
        public static string RSA_Key
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [AppSetting("sec_id")]
        public static string Robot_ID
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [AppSetting("sec_name")]
        public static string Robot_Name
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        Queue<BetAmtDG_Command> upload_data = new Queue<BetAmtDG_Command>();
        BetAmtDG_Command upload_busy;

        public void PrepareUpload()
        {
            lock (upload_data)
            {
                foreach (BetAmtDG_Command n in this.Values)
                {
                    if (upload_data.Contains(n)) continue;
                    if (n.IsUploaded) continue;
                    upload_data.Enqueue(n);
                }
            }
        }

        public void SetState(bool success, string status)
        {
            lock (upload_data)
            {
                if (upload_busy == null) return;
                upload_busy.IsUploaded = success;
                upload_busy.Status = status;
            }
        }

        public void UploadComplete()
        {
            lock (upload_data)
                upload_busy = null;
        }

        public BetAmtDG_Command GetUploadItem()
        {
            lock (upload_data)
            {
                if (upload_data.Count == 0) return null;
                if (upload_busy != null) return null;
                upload_busy = upload_data.Dequeue();
                return upload_busy;
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BetAmtDG_Command : GameLog_BetAmtDG_RowCommand
    {
        public bool IsUploaded { get; set; }
        public string Status { get; set; }
        public string json
        {
            get { return web.api.SerializeObject(this); }
        }

        public DateTime? _ACTime
        {
            get
            {
                if (base.ACTime.HasValue)
                    return base.ACTime.Value.AddHours(8);
                return base.ACTime;
            }
        }
    }

    public abstract class collectWebBrowser : System.Windows.Forms.WebBrowser
    {
        bool isStartBrowse = false;
        public void StartBrowse()
        {
            if (this.isStartBrowse) return;
            this.isStartBrowse = true;
            base.Url = new Uri(this.SiteUrl);
        }

        public abstract void Collect();

        public abstract string SiteUrl { get; }

        public collectReportLog OwnerWindow
        {
            get
            {
                for (Control c = this.Parent; c != null; c = c.Parent)
                    if (c is collectReportLog)
                        return (collectReportLog)c;
                return null;
            }
        }

        protected override void OnProgressChanged(WebBrowserProgressChangedEventArgs e)
        {
            base.OnProgressChanged(e);
            collectReportLog form = this.OwnerWindow;
            form.tsp_WebProcress.Visible = true;
            if ((e.CurrentProgress > 0) && (e.MaximumProgress > 0))
            {
                form.tsp_WebProcress.Maximum = Convert.ToInt32(e.MaximumProgress);
                form.tsp_WebProcress.Step = Convert.ToInt32(e.CurrentProgress);
                form.tsp_WebProcress.PerformStep();
            }
            else if (this.ReadyState == WebBrowserReadyState.Complete)
            {
                form.tsp_WebProcress.Value = 0;
                form.tsp_WebProcress.Visible = false;
            }
        }
    }

    public class collect_BBIN : collectWebBrowser
    {
        public override void Collect()
        {
            try
            {
                HtmlDocument doc = this.Document;

                if (doc.Window.Frames.Count == 0)
                {
                    throw new Exception("请进入到报表页面再进行采集");
                }
                foreach (HtmlWindow h in doc.Window.Frames)
                {
                    if (h.Name == "net_topFrame")
                    {
                        HtmlElementCollection trs = null;
                        try
                        {
                            trs = h.Frames[1].Document.GetElementById("MainTbody_ID_All").GetElementsByTagName("tr");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("请进入到报表页面再进行采集");
                        }

                        if (trs.Count <= 0)
                        {
                            throw new Exception("暂无数据，请稍候再试！");
                        }

                        //账务日
                        DateTime actime = Convert.ToDateTime(h.Frames[1].Document.GetElementById("myTableAll").GetElementsByTagName("span")[0].InnerText.Substring(6, 10));
                        actime = actime.AddHours(8);

                        foreach (HtmlElement tr in trs)
                        {
                            HtmlElementCollection tds = tr.GetElementsByTagName("td");
                            this.OwnerWindow.list2.AddData(new BetAmtDG_Command()
                            {
                                GameID = BU.GameID.BBIN,
                                GameType = "无",
                                ACTime = actime,
                                ACNT = tds[0].InnerText,
                                BetAmount = tds[2].InnerText.ToDecimal(),
                                BetAmountAct = tds[4].InnerText.ToDecimal(),
                                Payout = -tds[3].InnerText.ToDecimal()
                            });
                        }
                        MessageBox.Show("该页采集完毕！");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [AppSetting("url_BBIN")]
        public override string SiteUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
    }

    public class collect_AG : collectWebBrowser
    {
        public override void Collect()
        {
            try
            {
                if (this.Url.AbsoluteUri != "http://gdc.asia-gaming.com:8083/order/groupByLoginNameOrderAction.htm")
                {
                    throw new Exception("请进入到报表页面再进行采集");
                }

                HtmlDocument doc = this.Document;

                //账务日
                DateTime actime = Convert.ToDateTime(doc.GetElementById("begin_time").GetAttribute("value").Substring(0, 10));
                actime = actime.AddHours(8);

                //厅类型
                string strPlat = doc.GetElementById("platformType").GetAttribute("value");
                BU.GameID gamePlat = BU.GameID.AG;
                if (string.IsNullOrEmpty(strPlat))
                {
                    throw new Exception("请选择平台类型！");
                }
                else if (strPlat == "AG")
                {
                    gamePlat = BU.GameID.AG_AG;
                }
                else if (strPlat == "DSP")
                {
                    gamePlat = BU.GameID.AG_DSP;
                }
                else if (strPlat == "AGIN")
                {
                    gamePlat = BU.GameID.AG_AGIN;
                }

                HtmlElementCollection trs = doc.GetElementById("tableWork").GetElementsByTagName("tr");

                if (trs.Count <= 0)
                {
                    throw new Exception("暂无数据，请稍候再试！");
                }
                foreach (HtmlElement tr in trs)
                {
                    //到小计部分截止采集
                    if (tr.GetAttribute("id") != "pro")
                    {
                        continue;
                    }

                    HtmlElementCollection tds = tr.GetElementsByTagName("td");

                    this.OwnerWindow.list2.AddData(new BetAmtDG_Command()
                    {
                        GameID = gamePlat,
                        GameType = "无",
                        ACTime = actime,
                        ACNT = tds[1].InnerText,
                        BetAmount = tds[10].InnerText.ToDecimal(),
                        BetAmountAct = tds[11].InnerText.ToDecimal(),
                        Payout = tds[12].InnerText.ToDecimal()
                    });
                }
                MessageBox.Show("该页采集完毕！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [AppSetting("url_AG")]
        public override string SiteUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
    }

    public class collect_HG : collectWebBrowser
    {
        public override void Collect()
        {
            try
            {
                if (!this.Url.AbsoluteUri.Contains("https://reports.virtuagaming.com/PlayerReport.aspx"))
                {
                    throw new Exception("请进入到玩家报告页面再进行采集");
                }

                HtmlDocument doc = this.Document;

                //三个checkbox都勾选获取的是有效投注额
                bool allChecked = doc.GetElementById("chkExcludTieBet").GetAttribute("checked") == "True" && doc.GetElementById("chkExcludEvenBet").GetAttribute("checked") == "True" && doc.GetElementById("chkBonusBet").GetAttribute("checked") == "True";
                //三个checkbox都未勾选获取的是总投注额和输赢额度
                bool allNoChecked = doc.GetElementById("chkExcludTieBet").GetAttribute("checked") == "False" && doc.GetElementById("chkExcludEvenBet").GetAttribute("checked") == "False" && doc.GetElementById("chkBonusBet").GetAttribute("checked") == "False";

                if (!allChecked && !allNoChecked)
                {
                    throw new Exception("请勾选全部复选框，或者全不勾选在进行采集");
                }

                //账务日
                DateTime actime = Convert.ToDateTime(doc.GetElementById("txt_stdate").GetAttribute("value").Substring(0, 10));
                actime = actime.AddHours(8);

                HtmlElementCollection trs = doc.GetElementById("UpdatePanel2").GetElementsByTagName("table")[1].GetElementsByTagName("table")[0].GetElementsByTagName("tr");

                if (trs.Count == 2)
                {
                    throw new Exception("暂无数据，请稍候再试！");
                }

                int flag = 0;
                foreach (HtmlElement tr in trs)
                {
                    flag++;
                    if (flag == 1) continue;
                    if (flag == trs.Count) break;

                    HtmlElementCollection tds = tr.GetElementsByTagName("td");
                    this.AddData(actime, tds, allNoChecked, allChecked);

                    //BetAmtDG_Command oldGb = null;

                    ////检查当前该玩家基本资料是否已经插入
                    //foreach (BetAmtDG_Command gb in this.OwnerWindow.list)
                    //{
                    //    if (gb.GameID == BU.GameID.HG && gb.ACTime == actime && gb.ACNT == tds[1].InnerText)
                    //    {
                    //        oldGb = gb;
                    //        break;
                    //    }
                    //}

                    ////如果存在
                    //if (oldGb != null)
                    //{
                    //    this.OwnerWindow.list.Remove(oldGb);
                    //    if (allNoChecked)
                    //    {
                    //        oldGb.BetAmount = tds[6].InnerText.ToDecimal();
                    //        oldGb.Payout = tds[7].InnerText.ToDecimal();

                    //    }
                    //    if (allChecked)
                    //    {
                    //        oldGb.BetAmountAct = tds[6].InnerText.ToDecimal();
                    //    }
                    //    this.OwnerWindow.list.Add(oldGb);

                    //    if (oldGb.BetAmount != 0 && oldGb.BetAmountAct != 0 && oldGb.Payout != 0)
                    //    {
                    //        string gJson = web.api.SerializeObject(oldGb);
                    //        if (!this.OwnerWindow.listOfJson.Contains(gJson))
                    //        {
                    //            this.OwnerWindow.listOfJson.Add(gJson);
                    //        }
                    //    }
                    //}
                    ////不存在直接插入数据
                    //else
                    //{
                    //    BetAmtDG_Command g = new BetAmtDG_Command()
                    //    {
                    //        GameID = BU.GameID.HG,
                    //        GameType = "无",
                    //        ACTime = actime,
                    //        ACNT = tds[1].InnerText,
                    //        BetAmount = allNoChecked ? tds[6].InnerText.ToDecimal() : 0,
                    //        BetAmountAct = allChecked ? tds[6].InnerText.ToDecimal() : 0,
                    //        Payout = allNoChecked ? tds[7].InnerText.ToDecimal() : 0
                    //    };
                    //    this.OwnerWindow.list.Add(g);
                    //}
                }
                MessageBox.Show("该页采集完毕！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [AppSetting("url_HG")]
        public override string SiteUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        void AddData(DateTime actime, HtmlElementCollection tds, bool allNoChecked, bool allChecked)
        {
            foreach (KeyValuePair<string, BetAmtDG_Command> p in this.OwnerWindow.list2)
            {
                if (p.Value.GameID == BU.GameID.HG && p.Value.ACTime == actime && p.Value.ACNT == tds[1].InnerText)
                {
                    if (allNoChecked)
                    {
                        p.Value.BetAmount = tds[6].InnerText.ToDecimal();
                        p.Value.Payout = tds[7].InnerText.ToDecimal();

                    }
                    if (allChecked)
                    {
                        p.Value.BetAmountAct = tds[6].InnerText.ToDecimal();
                    }

                    this.OwnerWindow.list2.Remove(p.Key);
                    this.OwnerWindow.list2.AddData(p.Value);
                    return;
                }
            }
            this.OwnerWindow.list2.AddData(new BetAmtDG_Command()
            {
                GameID = BU.GameID.HG,
                GameType = "无",
                ACTime = actime,
                ACNT = tds[1].InnerText,
                BetAmount = allNoChecked ? tds[6].InnerText.ToDecimal() : 0,
                BetAmountAct = allChecked ? tds[6].InnerText.ToDecimal() : 0,
                Payout = allNoChecked ? tds[7].InnerText.ToDecimal() : 0
            });
        }
    }

    public class collect_SunCity : collectWebBrowser
    {
        public override void Collect()
        {
            try
            {
                HtmlDocument doc = this.Document;

                if (doc.Window.Frames.Count == 0)
                {
                    throw new Exception("请进入到报表页面再进行采集");
                }

                foreach (HtmlWindow h in doc.Window.Frames)
                {
                    if (h.Name == "middle")
                    {
                        try
                        {
                            if (h.Frames[0].Document.GetElementById("Label3").InnerText != "輸贏報表" && h.Frames[0].Document.GetElementById("Label3").InnerText != "输赢报表")
                            {
                                throw new Exception("请进入到报表页面再进行采集");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("请进入到报表页面再进行采集");
                        }

                        //账务日      
                        DateTime actime = Convert.ToDateTime(h.Frames[0].Document.GetElementById("wFromDate").GetAttribute("value"));
                        actime = actime.AddHours(8);

                        HtmlElementCollection trs = null;

                        try
                        {
                            trs = h.Frames[0].Document.GetElementById("form1").GetElementsByTagName("div")[2].GetElementsByTagName("table")[2].GetElementsByTagName("div")[1].GetElementsByTagName("tr");
                        }
                        catch (Exception ex) { throw new Exception("请点击查询"); }

                        int flag = 0;
                        foreach (HtmlElement tr in trs)
                        {
                            flag++;
                            if (flag == 1) continue;
                            if (flag == trs.Count) break;

                            HtmlElementCollection tds = tr.GetElementsByTagName("td");

                            if (tds[2].InnerText == "tengfa8") continue;

                            this.OwnerWindow.list2.AddData(new BetAmtDG_Command()
                            {
                                GameID = BU.GameID.SUNBET,
                                GameType = tds[6].InnerText,
                                ACTime = actime,
                                ACNT = tds[2].InnerText,
                                BetAmount = tds[3].InnerText.ToDecimal(),
                                BetAmountAct = tds[5].InnerText.ToDecimal(),
                                Payout = tds[4].InnerText.ToDecimal(),
                            });
                        }
                        MessageBox.Show("该页采集完毕！");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [AppSetting("url_SunCity")]
        public override string SiteUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
    }
}