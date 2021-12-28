using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Tools.Protocol;

public partial class Management_Corps : System.Web.UI.Page
{
    public web.jqGridOptions grid = new web.jqGridOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        string[] s = new string[] { "aa", "bb", "cc", "dd" };
        string ss = JsonProtocol.SerializeObject(s);
    }

    class Corp
    {
        [DbImport]
        public int CorpID;
        [DbImport]
        public string Domain;
        [DbImport]
        public string Name;
        [DbImport]
        public DateTime? ActiveTime;
        [DbImport]
        public DateTime CreateTime;
        [DbImport]
        public DateTime ModifyTime;
        [DbImport]
        public string ModifyUser;
    }
}