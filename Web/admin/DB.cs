using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace BU
{
    [System.Diagnostics.DebuggerStepThrough]
    public static class DB
    {
        public static SqlCmd Open(Name name, Access access)
        {
            SqlCmd result;
            DB.Open(name, access, out result, null);
            return result;
        }
        public static IDisposable Open(Name name, Access access, out SqlCmd result, SqlCmd existing)
        {
            if (Enum.IsDefined(typeof(Name), name) && Enum.IsDefined(typeof(Access), access))
            {
                string key = string.Format("{0}{1:00}", (Code)name, (byte)access);
                string cn = null;
                PropertyInfo p = typeof(DB).GetProperty(key);
                if (p != null)
                    cn = (string)p.GetValue(null, null);
                if (existing != null)
                {
                    if ((existing.ctorConfigKey == key) && (existing.ctorConnectionString == cn))
                    {
                        result = existing;
                        return null;
                    }
                }
                return result = new SqlCmd(key, cn);
            }
            result = null;
            return null;
        }

        const string sql_rw = "Data Source=db01;Initial Catalog=Game;Persist Security Info=True;User ID=sa;Password=sa";
        const string sql_r = "Data Source=db02;Initial Catalog=Game;Persist Security Info=True;User ID=sa;Password=sa";

        [ConnectionString, DefaultValue(sql_rw)]
        public static string DB01
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [ConnectionString, DefaultValue(sql_r)]
        public static string DB02
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [ConnectionString, DefaultValue(sql_rw)]
        public static string ST01
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [ConnectionString, DefaultValue(sql_r)]
        public static string ST02
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [ConnectionString, DefaultValue(sql_rw)]
        public static string LOG01
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
        [ConnectionString, DefaultValue(sql_r)]
        public static string LOG02
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        public enum Name : byte
        {
            Main, State, Log
        }

        public enum Code : byte
        {
            DB = Name.Main,
            ST = Name.State,
            LOG = Name.Log,
        }

        [Flags]
        public enum Access : byte
        {
            Read = 0x02, ReadWrite = 0x01
        }
    }
}
