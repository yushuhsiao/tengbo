using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using web;

public class HelpMasterPage : SiteMasterPage
{
}

public class HelpPage : SitePage
{
    public int? HelpIndex;
}
