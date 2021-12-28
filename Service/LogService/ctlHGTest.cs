using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using BU;

namespace service
{
    public partial class ctlHGTest : UserControl
    {
        public ctlHGTest()
        {
            InitializeComponent();
        }
        private void button01_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetAllbetdetails(null, null, null, null); } catch (Exception ex) { log.error(ex); } }); }
        private void button02_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetAllBetDetailsfor30seconds(null); } catch (Exception ex) { log.error(ex); } }); }
        private void button03_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetBetDetailsByAffiliate(dateTimePicker2.Value, null); } catch (Exception ex) { log.error(ex); } }); }
        private void button04_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetTableList(); } catch (Exception ex) { log.error(ex); } }); }
        private void button05_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((object state) =>
            {
                try
                {
                    using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite))
                        extAPI.hg.api.GetPlayerDetails(null, dateTimePicker3.Value.Date, dateTimePicker3.Value.Date).parse_data(sqlcmd);
                }
                catch (Exception ex) { log.error(ex); }
            });
        }
        private void button06_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetAgentPlayerDetails(DateTime.Now); } catch (Exception ex) { log.error(ex); } }); }
        private void button07_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetEventbetsAgentPlayerDetails(DateTime.Now, null); } catch (Exception ex) { log.error(ex); } }); }
        private void button08_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetAgentsEvenbetsDetails(DateTime.Now); } catch (Exception ex) { log.error(ex); } }); }
        private void button09_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetAllBetDetailsPerTimeInterval(null, DateTime.Now, DateTime.Now, null, null); } catch (Exception ex) { log.error(ex); } }); }
        private void button10_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetAllFundTransferDetailsTimeInterval(null, DateTime.Now, DateTime.Now, null, null); } catch (Exception ex) { log.error(ex); } }); }
        private void button11_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { DateTime t = dateTimePicker1.Value.Date; extAPI.hg.api.GetGameResultInfo(null, t, t.AddDays(1).AddSeconds(-1), null, null); } catch (Exception ex) { log.error(ex); } }); }
        private void button12_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((object state) =>
            {
                try
                {
                    //extAPI.hg.hgResponse2 res = extAPI.hg.api.GetPlayerBetAmount(null, dateTimePicker5.Value.Date, numericUpDown1.Value >= 0 ? (int?)numericUpDown1.Value : null);
                    extAPI.hg.hgResponse2 res = extAPI.hg.api.GetPlayerBetAmount(null, dateTimePicker5.Value.Date.AddHours((double)numericUpDown1.Value), null);
                    if (res != null)
                        using (SqlCmd sqlcmd = DB.Open(DB.Name.Log, DB.Access.ReadWrite))
                            res.parse_data(sqlcmd);
                }
                catch (Exception ex) { log.error(ex); }
            });
        }
        private void button13_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.GetBonusInfo(null, DateTime.Now, DateTime.Now, null); } catch (Exception ex) { log.error(ex); } }); }
        private void button14_Click(object sender, EventArgs e) { ThreadPool.QueueUserWorkItem((object state) => { try { extAPI.hg.api.Getbetdetails_ExcludeTieAndEven(null, null, null); } catch (Exception ex) { log.error(ex); } }); }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
