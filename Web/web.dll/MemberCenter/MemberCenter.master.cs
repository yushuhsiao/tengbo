using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tools;

public class SiteLogControl : SiteControl
{
    protected virtual string nav_index { get { return ""; } }

    public string content_index_s;
    public int content_index;
    const int count = 6;
    public DateTime Time;
    public string[] items = new string[count];
    public DateTime[] times = new DateTime[count];
    public string[] titles = new string[count];

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        this.content_index_s = Request.Form["content_index"] ?? this.nav_index;
        this.Time = DateTime.Now;
        for (int i = 0; i < count; i++)
        {
            DateTime t2 = times[i] = this.Time.AddMonths(-i);
            items[i] = string.Format((this.Time.Month == t2.Month) ? "{0}" : "{0}_{1:yyyyMM}", this.nav_index, t2);
            titles[i] = t2.ToString("yyyy/MM");
            if (this.items[i] == this.content_index_s)
                this.content_index = i;
        }
    }

    public bool reload
    {
        get { return this.Time.Month == this.times[this.content_index].Month; }
    }

    public IEnumerable<int> log_nav
    {
        get { for (int i = 0; i < count; i++) yield return i; }
    }

    static int add_logtype(StringBuilder s, LogType l, int cnt)
    {
        if (cnt == 0)
            s.Length = 0;
        else
            s.Append(",");
        s.Append((int)l);
        cnt++;
        return cnt;
    }

    static SiteLogControl()
    {
        int i, cnt;
        StringBuilder s;
        s = new StringBuilder();
        cnt = 0;
        foreach (LogType l in _null<web.tran.Cash.DepositRowCommand>.value.AcceptLogTypes)
            cnt = add_logtype(s, l, cnt);
        foreach (LogType l in _null<web.tran.Cash.ThirdPaymentRowCommand>.value.AcceptLogTypes)
            cnt = add_logtype(s, l, cnt);
        logType_deposit = s.ToString();

        cnt = 0;
        foreach (LogType l in _null<web.tran.Cash.WithdrawalRowCommand>.value.AcceptLogTypes)
            cnt = add_logtype(s, l, cnt);
        cnt = add_logtype(s, LogType.WithdrawalRollback, cnt);
        cnt = add_logtype(s, LogType.WithdrawalWithholding, cnt);
        logType_withdrawal = s.ToString();

        cnt = 0;
        foreach (LogType l in _null<web.tran.Game.GameDepositRowCommand>.value.AcceptLogTypes)
            cnt = add_logtype(s, l, cnt);
        foreach (LogType l in _null<web.tran.Game.GameWithdrawalRowCommand>.value.AcceptLogTypes)
            cnt = add_logtype(s, l, cnt);
        cnt = add_logtype(s, LogType.GameDepositRollback, cnt);
        logType_GameTran = s.ToString();
    }

    static string logType_deposit;
    static string logType_withdrawal;
    static string logType_GameTran;

    public IEnumerable<SqlDataReader> GetGameTran()
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            int count = 0;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select dateadd(mm,datediff(mm,0,CreateTime),0),CreateTime,Amount,Balance,LogType,GameID from TranLog nolock where TranID in 
(select TranID from TranLog nolock 
where UserID={0} and dateadd(mm,datediff(mm,0,CreateTime),0)='{1:yyyy-MM-01}' and LogType in ({2}) 
group by TranID having count(1)=1) order by CreateTime desc", this.Member.ID, this.times[this.content_index], logType_GameTran))
            {   
                count++;
                yield return r;
            }
            if (count == 0)
                yield return null;
        }
    }

    public IEnumerable<SqlDataReader> GetDeposit()
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            int count = 0;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select dateadd(mm,datediff(mm,0,CreateTime),0),CreateTime,Amount from TranLog nolock 
where UserID={0} and dateadd(mm,datediff(mm,0,CreateTime),0)='{1:yyyy-MM-01}' and LogType in ({2}) order by CreateTime desc", this.Member.ID, this.times[this.content_index], logType_deposit))
            {
                count++;
                yield return r;
            }
            if (count == 0)
                yield return null;
        }
    }

    public IEnumerable<SqlDataReader> GetWithdrawal()
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            int count = 0;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select * from TranLog
where UserID={0} and dateadd(mm,datediff(mm,0,CreateTime),0)='{1:yyyy-MM-01}' and LogType in ({2}) and Amount<>0
order by CreateTime desc", this.Member.ID, this.times[this.content_index], logType_withdrawal))
            {
                count++;
                yield return r;
            }
            if (count == 0)
                yield return null;
        }
    }

    public IEnumerable<SqlDataReader> GetBetBonus()
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            int count = 0;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select dateadd(mm,datediff(mm,0,CreateTime),0),CreateTime,Amount,BetAmount,BetBonus,BetPayout from TranLog nolock 
where UserID={0} and dateadd(mm,datediff(mm,0,CreateTime),0)='{1:yyyy-MM-01}' and LogType = {2}  order by CreateTime desc", this.Member.ID, this.times[this.content_index], (int)LogType.洗碼優惠))
            {
                count++;
                yield return r;
            }
            if (count == 0)
                yield return null;
        }
    }

    public IEnumerable<SqlDataReader> GetPromotion()
    {
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            int count = 0;
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2(@"select dateadd(mm,datediff(mm,0,CreateTime),0),CreateTime,Amount,LogType from TranLog nolock 
where UserID={0} and dateadd(mm,datediff(mm,0,CreateTime),0)='{1:yyyy-MM-01}' and LogType <> {2} and LogType > {3} and LogType <= {4} order by CreateTime desc", this.Member.ID, this.times[this.content_index], (int)LogType.洗碼優惠, (int)LogType.Promos, (int)LogType.PromosMAX))
            {
                count++;
                yield return r;
            }
            if (count == 0)
                yield return null;
        }
    }
}