using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Threading;

namespace collectWf
{
    public partial class Searcher : Form
    {
        IList<string> a = new List<string>();
        public Searcher()
        {
            InitializeComponent();
            //this.wb_Godaddy.DocumentCompleted += wb_Godaddy_DocumentCompleted;
        }

        /// <summary>
        /// DataGridView 导出至 Excel
        /// </summary>
        /// <param name="dGV"></param>
        /// <param name="filename"></param>
        private void ToCsV(DataGridView dGV, string filename)
        {
            string stOutput = "";
            string sHeaders = "";

            for (int j = 0; j < dGV.Columns.Count; j++)
            {
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            }

            stOutput += sHeaders + "\r\n";

            for (int i = 0; i < dGV.RowCount; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                {
                    stLine += Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                }
                stOutput += stLine + "\r\n";
            }

            Encoding encoding = Encoding.GetEncoding("gb2312");
            byte[] output = encoding.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length);
            bw.Flush();
            bw.Close();
            fs.Close();
            fs.Dispose();
            MessageBox.Show("导出成功");
        }


        /// <summary>
        /// 采集网页源代码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HtmlAgilityPack.HtmlDocument HtmlDocument(string url)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                Uri uri = new Uri(url);
                string pageSource = string.Empty;
                using (WebClient webClient = new WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    //webClient.Encoding = Encoding.GetEncoding("gb2312"); 
                    pageSource = webClient.DownloadString(uri);
                }

                doc.LoadHtml(pageSource);
            }
            catch (Exception ex)
            {
                //无法解析此远程名称
                doc.LoadHtml("<html><body>Unable to resolve</body></html>");
                //throw new WebException();
            }
            return doc;
        }

        //private void wb_Godaddy_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    HtmlAgilityPack.HtmlDocument rdoc = new HtmlAgilityPack.HtmlDocument();
        //    rdoc.LoadHtml(wb_Godaddy.DocumentText);
        //
        //    try
        //    {
        //        string b = rdoc.DocumentNode.SelectSingleNode("//div[@class='headerResult']").Attributes["display"].Value;
        //        
        //        MessageBox.Show("不可用");
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            string c = rdoc.DocumentNode.SelectSingleNode("//div[@class='unavailableCopy'][1]").Attributes["display"].Value;
        //            MessageBox.Show("可用");
        //        }
        //        catch (Exception exc)
        //        {
        //            MessageBox.Show("不可用");
        //        }
        //    }
        //    wb_Godaddy_begin();
        //}

        //public void wb_Godaddy_begin()
        //{
        //    if (wb_Godaddy_queue.Count == 0)
        //    {
        //        return;
        //    }
        //    wb_Godaddy.Document.GetElementById("domain_search_input").SetAttribute("value", wb_Godaddy_queue.Dequeue().DMain);
        //    wb_Godaddy.Document.GetElementById("search_form_btn").InvokeMember("click");
        //}

        //Queue<DoMain> wb_Godaddy_queue = new Queue<DoMain>();


        private void btn_Search_Click(object sender, EventArgs e)
        {
            //wb_Godaddy_queue.Enqueue(new DoMain { Content = "1", DMain = "tengbo.com", Href = "", KeyWords = "", PageIndex = "", Rule = "", SearchName = "" });
            //wb_Godaddy_queue.Enqueue(new DoMain { Content = "1", DMain = "baidu.com", Href = "", KeyWords = "", PageIndex = "", Rule = "", SearchName = "" });
            //wb_Godaddy_begin();
            //return;
            //List<int> name = new List<int>();
            //if (name == null && name.Count == 0)
            //{
            //    MessageBox.Show("null 0");
            //}
            //else if (name == null)
            //{
            //    MessageBox.Show("null");
            //}
            //else if (name.Count == 0) {
            //    MessageBox.Show("0");
            //}
            //return;
            string keyWords = txt_KeyWords.Text;
            int pageIndex = Convert.ToInt32(cbo_PageIndex.Text);
            IList<DoMain> list_DoMain = new List<DoMain>();
            IList<DoMain> list_DoMain_GoDaddy = new List<DoMain>();
            IList<DoMain> list_DoMain_CantResolve = new List<DoMain>();

            if (keyWords.Trim() == "")
            {
                MessageBox.Show("请输入关键字");
                return;
            }

            string[] keywords = txt_KeyWords.Lines;

            //采集域名规则
            string xPath = string.Empty;
            string searchUrl = cbo_SearchUrl.SelectedValue.ToString();
            int searchUrlIndex = cbo_SearchUrl.SelectedIndex;
            string searchName = string.Empty;
            if (searchUrlIndex == 0)
            {
                xPath = "//li[@class='res-list']";
                searchName = "360";
            }
            else if (searchUrlIndex == 1)
            {
                xPath = "//h3[@class='pt'] | //h3[@class='vrt']";
                searchName = "搜狗";
            }

            string currentKey = string.Empty;
            try
            {
                foreach (string key in keywords)
                {
                    if (key == "" || key == null || key == string.Empty)
                    {
                        continue;
                    }
                    for (int i = 0; i < pageIndex; i++)
                    {
                        currentKey = key;

                        HtmlNodeCollection list = HtmlDocument(string.Format(searchUrl, key, (i + 1))).DocumentNode.SelectNodes(xPath);

                        if (list == null || list.Count == 0)
                        {
                            continue;
                        }

                        int count = 0;
                        foreach (HtmlNode node in list)
                        {
                            count++;
                            HtmlNode n = null;
                            string href = string.Empty;
                            string doMain = string.Empty;
                            doMain = string.Empty;

                            try
                            {
                                n = node.SelectSingleNode(".//a[1]");
                                href = n.Attributes["href"].Value;
                                string protocol = "";
                                if (href.ToLower().Contains("http://"))
                                {
                                    protocol = "http://";
                                }
                                else
                                {
                                    protocol = "https://";
                                }
                                doMain = href.Replace(protocol, "");
                                doMain = protocol + doMain.Substring(0, doMain.IndexOf("/"));
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }

                            list_DoMain.Add(
                                new DoMain
                                {
                                    Content = n.InnerText,
                                    Href = href,
                                    DMain = doMain,
                                    KeyWords = key,
                                    PageIndex = (i + 1).ToString(),
                                    Ranking = "第" + count.ToString() + "位",
                                    SearchName = searchName
                                });
                        }
                    }
                }

                //多线程
                //ThreadPool.QueueUserWorkItem(delegate(object state)
                //{
                //});

                if (list_DoMain.Count > 0)
                {
                    dgv_List.DataSource = list_DoMain;

                    int rowNumber = 1;
                    foreach (DataGridViewRow row in dgv_List.Rows)
                    {
                        row.HeaderCell.Value = rowNumber.ToString();
                        rowNumber++;
                    }
                    dgv_List.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                }
                else
                {
                    dgv_List.DataSource = null;
                    MessageBox.Show("未采集到任何数据");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("域名采集过程中发生异常:" + ex.Message + " (当前搜索关键字:" + currentKey + ")");
                return;
            }

            //筛选可用域名
            try
            {
                string usableFileName = "Log/可用域名" + DateTime.Now.ToString("yyyy-MM-dd HH点mm分ss秒") + ".txt";
                foreach (DoMain domain in list_DoMain)
                {
                    if (!domain.DMain.ToLower().Contains("www") && domain.DMain.ToLower().Split('.').Length > 2)
                    {
                        continue;
                    }

                    HtmlAgilityPack.HtmlNode node = null;
                    try
                    {
                        node = HtmlDocument(domain.DMain).DocumentNode;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    //CollectPageSourceLog(domain, node);

                    //GoDaddy
                    if (node.InnerHtml.ToString().Trim() == "<html><body></body></html>")
                    {
                        domain.Rule = "域名商:GoDaddy";
                        list_DoMain_GoDaddy.Add(domain);

                        WriteCanUseLog(usableFileName, domain);
                    }
                    //Cannot be resolved
                    else if (node.InnerText.ToString().Trim() == "Unable to resolve")
                    {
                        domain.Rule = "无法解析此远程名称";
                        list_DoMain_CantResolve.Add(domain);
                    }
                }

                //第二次排查无法解析域名
                if (list_DoMain_CantResolve != null && list_DoMain_CantResolve.Count != 0)
                {
                    foreach (DoMain domain in list_DoMain_CantResolve)
                    {
                        HtmlAgilityPack.HtmlNode node = null;
                        try
                        {
                            node = HtmlDocument(domain.DMain).DocumentNode;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        //GoDaddy
                        if (node.InnerHtml.ToString().Trim() == "<html><body></body></html>")
                        {
                            domain.Rule = "域名商:GoDaddy";
                            list_DoMain_GoDaddy.Add(domain);
                            list_DoMain_CantResolve.Remove(domain);

                            WriteCanUseLog(usableFileName, domain);
                        }
                    }
                }

                //第三次排查无法解析域名
                if (list_DoMain_CantResolve != null && list_DoMain_CantResolve.Count != 0)
                {
                    foreach (DoMain domain in list_DoMain_CantResolve)
                    {
                        HtmlAgilityPack.HtmlNode node = null;
                        try
                        {
                            node = HtmlDocument(domain.DMain).DocumentNode;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        //GoDaddy
                        if (node.InnerHtml.ToString().Trim() == "<html><body></body></html>")
                        {
                            domain.Rule = "域名商:GoDaddy";
                            list_DoMain_GoDaddy.Add(domain);
                            list_DoMain_CantResolve.Remove(domain);

                            WriteCanUseLog(usableFileName, domain);
                        }
                        //Cannot be resolved
                        else if (node.InnerText.ToString().Trim() == "Unable to resolve")
                        {
                            WriteCanUseLog(usableFileName, domain);
                        }
                    }
                }

                IList<DoMain> list_YDoMain = list_DoMain_GoDaddy.Concat(list_DoMain_CantResolve).ToList<DoMain>();

                if (list_YDoMain.Count > 0)
                {
                    dgv_YList.DataSource = list_YDoMain;

                    int rowNumber = 1;
                    foreach (DataGridViewRow row in dgv_YList.Rows)
                    {
                        row.HeaderCell.Value = rowNumber.ToString();
                        rowNumber++;
                    }
                    dgv_YList.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

                    MessageBox.Show("分析完毕：有可用域名");
                }
                else
                {
                    dgv_YList.DataSource = null;
                    MessageBox.Show("分析完毕：没有可用域名");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("域名分析过程中发生异常:" + ex.Message);
            }
        }

        /// <summary>
        /// 写入可用日志
        /// </summary>
        /// <param name="usableFileName"></param>
        /// <param name="domain"></param>
        public void WriteCanUseLog(string usableFileName, DoMain domain)
        {
            StreamWriter sw2 = new StreamWriter(usableFileName, true, Encoding.UTF8);
            sw2.WriteLine("标题:" + domain.Content + "  链接:" + domain.Href + "  域名:" + domain.DMain + "  关键字:" + domain.KeyWords + "  第几页:" + domain.PageIndex + "  搜索引擎:" + domain.SearchName + "  //" + domain.Rule);
            sw2.Close();
            sw2.Dispose();
        }

        /// <summary>
        /// 采集源代码日志
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="node"></param>
        public void CollectPageSourceLog(DoMain domain, HtmlAgilityPack.HtmlNode node)
        {
            //string xPath_CharSet = "//meta[@http-equiv='Content-Type'][1]";
            //MessageBox.Show(node.SelectSingleNode(xPath_CharSet).Attributes["content"].Value);
            string filename = "Log/" + domain.Content.Replace("*", "").Replace("/", "").Replace("\\", "").Replace("|", "").Replace("<", "").Replace(">", "").Replace("?", "").Replace(":", "").Replace("\"", "") + ".txt";
            StreamWriter sw1 = new StreamWriter(filename, false, Encoding.UTF8);
            sw1.WriteLine(node.InnerHtml);
            sw1.Close();
            sw1.Dispose();
        }

        private void Searcher_Load(object sender, EventArgs e)
        {
            //搜索前几页
            cbo_PageIndex.Text = "1";
            //绑定搜索引擎列表
            IList<SearchURL> searchURLList = new List<SearchURL>() { 
                new SearchURL() { 
                URlText = "360搜索引擎",
                URLValue = "http://www.so.com/s?ie=utf-8&src=360sou_home&q={0}&pn={1}"
            }, new SearchURL() {
                URlText = "搜狗搜索引擎",
                URLValue = "http://www.sogou.com/web?query={0}&page={1}"
            }};
            cbo_SearchUrl.DisplayMember = "URlText";
            cbo_SearchUrl.ValueMember = "URLValue";
            cbo_SearchUrl.DataSource = searchURLList;
        }

        private void btn_ToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string rc0 = dgv_YList.Rows[0].Cells[0].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("暂无要导出的数据");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = DateTime.Now.ToString("yyyy-MM-dd HH点mm分ss秒") + ".xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ToCsV(dgv_YList, sfd.FileName);
            }
        }
    }

    public class DoMain
    {
        public string Content { get; set; }
        public string Href { get; set; }
        public string DMain { get; set; }
        public string KeyWords { get; set; }
        public string PageIndex { get; set; }
        public string Ranking { get; set; }
        public string SearchName { get; set; }
        public string Rule { get; set; }
    }

    public class SearchURL
    {
        public string URlText { get; set; }
        public string URLValue { get; set; }
    }
}
