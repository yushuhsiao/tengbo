using System;
using System.Collections.Generic;
using System.Web;

namespace web
{
    public interface IGetLang
    {
        object this[string resourceKey] { get; }
    }
    public class BasePage : System.Web.UI.Page, IGetLang
    {
        public IGetLang lang
        {
            get { return this; }
        }
        object IGetLang.this[string resourceKey]
        {
            get { return this.GetLocalResourceObject(resourceKey) ?? this.GetGlobalResourceObject("res", resourceKey); }
        }
    }
}