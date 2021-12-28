using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using BU;

namespace web
{
    public sealed class config
    {
        /// <summary>
        /// 站台ID
        /// </summary>

        [DebuggerStepThrough]
        public static class Defaults
        {
            [SqlSetting, DefaultValue("0000")]
            public static string Password
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            }

            //[SqlSetting("MemberType")]
            //static string _MemberType
            //{
            //    get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            //}

            //public static MemberType MemberType
            //{
            //    get { return _MemberType.ToEnum<MemberType>() ?? MemberType.Normal; }
            //}

            [SqlSetting("Currency")]
            static string _Currency
            {
                get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            }

            public static CurrencyCode Currency
            {
                get { return _Currency.ToEnum<CurrencyCode>() ?? CurrencyCode.USD; }
            }
        }
    }
}