using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;

namespace web
{
    public class get_file : IHttpHandler
    {
        bool IHttpHandler.IsReusable
        {
            get { return true; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            switch (context.Request.AppRelativeCurrentExecutionFilePath)
            {
                case "~/Scripts/jqgrid/js/i18n/grid.locale-en.js":
                    for (CultureInfo c = Thread.CurrentThread.CurrentUICulture; c.Parent != c; c = c.Parent)
                    {
                        switch (c.Name)
                        {
                            case "zh-CHT": context.Response.TransmitFile("~/Scripts/jqgrid/js/i18n/grid.locale-tw.js"); return;
                            case "zh-CHS": context.Response.TransmitFile("~/Scripts/jqgrid/js/i18n/grid.locale-cn.js"); return;
                        }
                    }
                    break;
            }
            context.Response.TransmitFile(context.Request.AppRelativeCurrentExecutionFilePath);
        }
    }
}