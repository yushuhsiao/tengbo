using BU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Tools;

namespace web
{
    public abstract partial class _Global : System.Web.HttpApplication
    {
        static Dictionary<Guid, _Global> instances = new Dictionary<Guid, _Global>();
        public const int RootAdminID = 1;
        public const int RootAgentID = 2;

        static _Global()
        {
            TraceLogWriter.Enabled = TextLogWriter.Enabled = JsonTextLogWriter.Enabled = true;
            try
            {
                using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
                    ConfigRow.Cache.GetInstance(sqlcmd, null);
                log.message(null, "init completed.");
            }
            catch (Exception ex)
            {
                log.error_msg(ex);
            }
        }
        public _Global()
        {
            lock (instances) instances[Guid.NewGuid()] = this;
        }

        #region config

        [AppSetting("CorpID"), DefaultValue(0)]
        public static int DefaultCorpID
        {
            get { return app.config.GetValue<int>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting("Password"), DefaultValue("0000")]
        public static string DefaultPassword
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting(Key = "DebugMode"), DefaultValue(false)]
        public static bool DebugMode
        {
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
        }

        [AppSetting(Key = "DebugMode"), DefaultValue(false)]
        public static bool DebugMode2
        {
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
        }

        //[AppSetting(Key = "AdminWeb"), DefaultValue(false)]
        //public static bool AdminWeb
        //{
        //    get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
        //}

        public static T WebConfigSection<T>(string sectionName) where T : ConfigurationSection
        {
            return WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath).GetSection(sectionName) as T;
        }

        public static SessionStateSection sessionStateSection
        {
            get { return _Global.WebConfigSection<SessionStateSection>("system.web/sessionState"); }
        }

        #endregion
    }

    internal static class util
    {
    }
}