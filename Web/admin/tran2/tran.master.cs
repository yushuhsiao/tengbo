using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tran_master2 : web.masterpage
{
}

public partial class tran_master2<tpage> : tran_master2 where tpage : tran_page2
{
    public tpage _page
    {
        get { return this.Page as tpage; }
    }
}

public class tran_page2 : web.page
{
    public bool IsHist;
    public bool IsDeposit;
}