using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class jqxGrid : web.usercontrol
    {
        public jqxGrid()
        {
            for (Type t = this.GetType(); ; )
            {
                foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    foreach (DefaultValueAttribute d in p.GetCustomAttributes(typeof(DefaultValueAttribute), true))
                    {
                        p.SetValue(this, d.Value, null);
                        break;
                    }
                }
                if (t == typeof(jqxGrid)) break;
                t = t.BaseType;
            }
        }

        protected bool prop(string name)
        {
            PropertyInfo p = this.GetType().GetProperty(name);
            if (p != null)
            {
                object value = p.GetValue(this, null);
                foreach (DefaultValueAttribute d in p.GetCustomAttributes(typeof(DefaultValueAttribute), true))
                    return !object.Equals(d.Value, value);
            }
            return false;
        }

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //}
    }
}