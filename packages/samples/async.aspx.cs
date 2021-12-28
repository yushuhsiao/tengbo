//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//public partial class async : System.Web.UI.Page
//{
//    protected void Page_Load(object sender, EventArgs e)
//    {

//    }
//}


#region
//using System;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Net;
//using System.IO;
//using System.Text;
//using System.Text.RegularExpressions;
//public partial class async : System.Web.UI.Page
//{
//    private WebRequest _request;
//    void Page_Load(object sender, EventArgs e)
//    {
//        AddOnPreRenderCompleteAsync(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation));
//    }
    
//    IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
//    {
//        _request = WebRequest.Create("http://msdn.microsoft.com");
//        return _request.BeginGetResponse(cb, state);
//    }
    
//    void EndAsyncOperation(IAsyncResult ar)
//    {
//        string text;
//        using (WebResponse response = _request.EndGetResponse(ar))
//        {
//            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
//            {
//                text = reader.ReadToEnd();
//            }
//        }
//        Regex regex = new Regex("href\\s*=\\s*\"([^\"]*)\"", RegexOptions.IgnoreCase);
//        MatchCollection matches = regex.Matches(text);
//        StringBuilder builder = new StringBuilder(1024);
//        foreach (Match match in matches)
//        {
//            builder.Append(match.Groups[1]);
//            builder.Append("<br/>");
//        }
//        Output.Text = builder.ToString();
//    }
//}
#endregion

#region AsyncDataBind
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
public partial class async : System.Web.UI.Page
{
    private SqlConnection _connection;
    private SqlCommand _command;
    private SqlDataReader _reader;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { // Hook PreRenderComplete event for data binding
            this.PreRenderComplete += new EventHandler(Page_PreRenderComplete); // Register async methods 
            AddOnPreRenderCompleteAsync(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation));
        }
    }
    IAsyncResult BeginAsyncOperation(object sender, EventArgs e, AsyncCallback cb, object state)
    {
        string connect = WebConfigurationManager.ConnectionStrings["PubsConnectionString"].ConnectionString;
        _connection = new SqlConnection(connect);
        _connection.Open();
        _command = new SqlCommand("SELECT title_id, title, price FROM titles", _connection);
        return _command.BeginExecuteReader(cb, state);
    }
    void EndAsyncOperation(IAsyncResult ar)
    {
        _reader = _command.EndExecuteReader(ar);
    }
    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        //Output.DataSource = _reader; Output.DataBind();
    }
    public override void Dispose()
    {
        if (_connection != null) _connection.Close(); base.Dispose();
    }
}
#endregion