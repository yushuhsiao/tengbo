using BU;
using extAPI;
using extAPI.hg;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using web;

public partial class ag_master : SiteMasterPage
{
    public string url;
}

public abstract class ag_aspx : SitePage
{
}
public abstract class ag_aspx<TAPI, T, TRow, TRowCommand> : ag_aspx
    where TAPI : ag.api<TAPI>
    where T : MemberGame_AG<TAPI, T, TRow, TRowCommand>, new()
    where TRow : MemberGameRow_AG, new()
    where TRowCommand : MemberGameRowCommand_AG, new()
{
    static int trial_index;
    protected void Page_Load(object sender, EventArgs e)
    {
        bool? isTrial = Request.Form["trial"].ToBoolean();
        try
        {
            ag_master master = (ag_master)this.Master;
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                if ((isTrial == true) || (this.Member == null))
                {
                    TAPI agapi = extAPI.ag.api<TAPI>.GetInstance(_Global.DefaultCorpID);
                    int n = Interlocked.Increment(ref trial_index);
                    string loginname = string.Format("{0}test{1:000}", agapi.prefix, n % 10);
                    //string loginname = string.Format("{0}test{1}", agapi.prefix, RandomString.LowerNumber.GetRandomString(4));

                    ag.Response res1 = agapi.GetBalance(loginname, ag.actype.trial, agapi.password);
                    bool geturl;
                    if ((geturl = res1.result) == true)
                    {
                        geturl = res1.info.ToInt32().HasValue;
                        if (geturl == false)
                        {
                            ag.Response res2 = agapi.CheckOrCreateGameAccout(loginname, ag.actype.trial, agapi.password, ag.oddtype.A);
                            geturl = res2.result;
                        }
                    }
                    if (geturl)
                        master.url = agapi.forwardGame(loginname, agapi.password, null, null, extAPI.ag.actype.trial, null, null);
                }
                else
                    master.url = MemberGame_AG<TAPI, T, TRow, TRowCommand>.Instance.Login(sqlcmd, this.Member.ID, this.Member.CorpID/*, trial*/).forwardGame_url;
                if (string.IsNullOrEmpty(master.url))
                    Response.Redirect("~/");
                //master.gameframe.Attributes["src"] = url;
                return;
            }
        }
        catch (Exception ex)
        {
            log.error(ex);
        }
        Response.Redirect("~/");
    }
}
public class ag1_aspx : ag_aspx<ag.AG, MemberGame_AG_AG, MemberGame_AG_AG.Row, MemberGame_AG_AG.RowCommand> { }
public class ag2_aspx : ag_aspx<ag.AGIN, MemberGame_AG_AGIN, MemberGame_AG_AGIN.Row, MemberGame_AG_AGIN.RowCommand> { }
public class ag3_aspx : ag_aspx<ag.DSP, MemberGame_AG_DSP, MemberGame_AG_DSP.Row, MemberGame_AG_DSP.RowCommand> { }