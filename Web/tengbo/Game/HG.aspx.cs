using BU;
using extAPI.hg;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class hg_aspx : SitePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool? isTrial = Request.Form["trial"].ToBoolean();
        if ((isTrial == true) || (this.Member == null))
        {
            try
            {
                extAPI.hg.api hgapi = extAPI.hg.api.GetInstance(web._Global.DefaultCorpID);
                Guid guid = Guid.NewGuid();
                string name = guid.ToString().Replace("-", "");
                extAPI.hg.hgResponse1 trial = hgapi.registration(hgapi.prefix + name, null, 0, name, hgapi.prefix, "cny", null, null, null, null);
                if (trial.status == extAPI.hg.StatusCode.SUCCESS)
                {
                    this.gameframe.Attributes["src"] = string.Format("{0}?ticketId={1}&lang={2}", hgapi.hg_game, trial.ticket, "ch");
                    return;
                }
            }
            catch (Exception ex)
            {
                log.error(ex);
            }
        }
        else
        {
            using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
            {
                try
                {
                    web.MemberGame_HG.Row row = web.MemberGame_HG.Instance.Login(sqlcmd, this.Member.ID, this.Member.CorpID);
                    if (string.IsNullOrEmpty(row.ticket))
                        Response.Redirect("~/");
                    this.gameframe.Attributes["src"] = string.Format("{0}?ticketId={1}&lang={2}", row.get_api(null).hg_game, row.ticket, "ch");
                    return;
                }
                catch (Exception ex)
                {
                    log.error(ex);
                }
            }
        }
        Response.Redirect("~/");
    }
}