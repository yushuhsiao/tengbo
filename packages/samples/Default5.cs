using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using Tools.Protocol;
using web;

public partial class _Default : System.Web.UI.Page
{
    public string MenuString;
    protected void Page_Load(object sender, EventArgs e)
    {
        User user = Context.User as User;
        Codes.Node menu0 = Codes.Instance.Menu;
        UrlList urllist = UrlList.GetInstance(this.Context);
        Menu menu1 = new Menu(menu0, user, urllist, CultureInfo.CurrentUICulture.LCID);
        this.MenuString = JsonProtocol.Serialize(menu1, null, true, false, Newtonsoft.Json.Formatting.Indented);
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class Menu
    {
        //[JsonProperty("op")]
        int Code;
        [JsonProperty("txt")]
        string Text;
        [JsonProperty("url")]
        string Url;
        [JsonProperty("sub")]
        List<Menu> Childs;

        public Menu(Codes.Node node, User user, UrlList urllist, int lcid)
        {
            this.Code = node.Code;
            this.Text = node.GetText(lcid);
            if (urllist.ContainsKey(this.Code))
            {
                this.Url = urllist[this.Code] ?? "";
                if (this.Url.StartsWith("~/"))
                    this.Url = this.Url.Substring(2);
            }
            for (int i = 0; i < node.Childs.Count; i++)
            {
                if (user.Permissions[node.Childs[i].Code])
                {
                    if (this.Childs == null)
                        this.Childs = new List<Menu>();
                    this.Childs.Add(new Menu(node.Childs[i], user, urllist, lcid));
                }
            }
        }
    }



    //public string Menu0;
    //public string Menu1;
    //public string UserName;
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    web.AbstractUser user = Context.GetUser();
    //    web.Menu.Node root = web.Menu.Instance.Root;
    //    StringBuilder sb0 = new StringBuilder();
    //    StringBuilder sb1 = new StringBuilder();
    //    int menu_id = 0;
    //    for (int i = 0; i < root.Count; i++)
    //    {
    //        web.Menu.Node node = root[i];
    //        if (user.Permissions[node.ID])
    //        {
    //            sb0.AppendFormat(@"<label class=""navbar"" for=""menu{0}"">{1}</label>", ++menu_id, node.Texts.Value);
    //            sb1.AppendFormat(@"<table id=""menu{0}"" class=""navbar""><tr><td class=""fill""></td><td>", menu_id);
    //            RenderMenu(user, node, sb1, 0);
    //            sb1.Append("</td></tr></table>");
    //        }
    //    }
    //    Menu0 = sb0.ToString();
    //    Menu1 = sb1.ToString();
    //}

    //void RenderMenu(web.AbstractUser user, web.Menu.Node root, StringBuilder sb1, int depth)
    //{
    //    int count = 0;
    //    foreach (web.Menu.Node n in root)
    //        if (user.Permissions[n.ID])
    //            count++;
    //    if (count == 0) return;
    //    count = 0;
    //    for (int i = 0; i < root.Count; i++)
    //    {
    //        web.Menu.Node node = root[i];
    //        if (!user.Permissions[node.ID])
    //            continue;
    //        if (count == 0)
    //            sb1.AppendFormat("<ul{0}>", depth == 0 ? @" class=""menu""" : "");
    //        sb1.Append("<li><a");
    //        string url = node.Urls.Value;
    //        if (!string.IsNullOrEmpty(url))
    //            sb1.AppendFormat(@" href=""{0}"" target=""mainframe""", url);
    //        sb1.AppendFormat(">{0}</a>", node.Texts.Value);
    //        RenderMenu(user, node, sb1, depth + 1);
    //        sb1.Append("</li>");
    //        count++;
    //    }
    //    if (count > 0)
    //        sb1.Append("</ul>");

    //    //int count = 0;
    //    //foreach (sysMenu.Node n in root)
    //    //    if (user.Permissions[n.ID])
    //    //        count++;
    //    //if (count == 0) return;
    //    //count = 0;
    //    //for (int i = 0; i < root.Count; i++)
    //    //{
    //    //    sysMenu.Node node = root[i];
    //    //    if (!user.Permissions[node.ID])
    //    //        continue;
    //    //    if (depth == 0)
    //    //    {
    //    //        string menu_id = string.Format("menu{0}", count + 1);
    //    //        sb0.AppendFormat(@"<label class=""navbar"" for=""menu{0}"">{1}</label>", count + 1, node.Texts.Value);
    //    //        sb1.AppendFormat(@"<table id=""menu{0}"" class=""navbar""><tr><td class=""fill""></td><td>", count + 1);
    //    //        RenderMenu(user, node, sb0, sb1, depth + 1);
    //    //        sb1.Append("</td></tr></table>");
    //    //    }
    //    //    else
    //    //    {
    //    //        if (count == 0)
    //    //            sb1.AppendFormat("<ul{0}>", depth == 1 ? @" class=""menu""" : "");
    //    //        sb1.Append("<li><a");
    //    //        string url = node.Urls.Value;
    //    //        if (!string.IsNullOrEmpty(url))
    //    //            sb1.AppendFormat(@" href=""{0}"" target=""mainframe""", url);
    //    //        sb1.AppendFormat(">{0}</a>", node.Texts.Value);
    //    //        RenderMenu(user, node, sb0, sb1, depth + 1);
    //    //        sb1.Append("</li>");
    //    //    }
    //    //    count++;
    //    //}
    //    //if ((depth > 0) && (count > 0))
    //    //    sb1.Append("</ul>");
    //}

    //public void RenderMenuC()
    //{
    //    sysMenu.Node root = sysMenu.Instance.Root;
    //    StringBuilder sb1 = new StringBuilder();
    //    StringBuilder sb2 = new StringBuilder();
    //    sb1.Append(@"<table id=""menu0""><tr><td class=""fill""></td><td>");
    //    RenderMenuC(sb1, sb2, root, 0);
    //    //int index = 0;
    //    //for (int i = 0; i < root.childs.Count; i++)
    //    //{
    //    //    appMenu.Node node = root.childs[i];
    //    //    // check permission
    //    //    sb1.AppendFormat(@"<label class=""navbar"" for=""menu{0}"">玩家管理</label>", index + 1, node.Text);
    //    //    sb2.AppendFormat(@"<table id=""menu{0}""><tr><td class=""fill""></td><td>", index + 1);
    //    //    RenderMenu3(sb1, sb2, node, 1);
    //    //    sb2.Append("</td></tr></table>");
    //    //    index++;
    //    //}
    //    sb1.Append("</td></tr></table>");
    //    #region
    //    //using (StringWriter sw = new StringWriter(sb))
    //    //using (HtmlTextWriter html = new HtmlTextWriter(sw))
    //    //{
    //    //    html.AddAttribute(HtmlTextWriterAttribute.Id, "menu0");
    //    //    html.RenderBeginTag(HtmlTextWriterTag.Table);
    //    //    html.RenderBeginTag(HtmlTextWriterTag.Tr);
    //    //    html.AddAttribute(HtmlTextWriterAttribute.Class, "fill");
    //    //    html.RenderBeginTag(HtmlTextWriterTag.Td);
    //    //    html.RenderEndTag();
    //    //    html.RenderBeginTag(HtmlTextWriterTag.Td);
    //    //    for (int i = 0; i < root.childs.Count; i++)
    //    //    {
    //    //        html.AddAttribute(HtmlTextWriterAttribute.Class, "navbar");
    //    //        html.AddAttribute(HtmlTextWriterAttribute.For, string.Format("menu{0}", i + 1));
    //    //        html.RenderBeginTag(HtmlTextWriterTag.Label);
    //    //        html.Write(root.childs[i].Text);
    //    //        html.RenderEndTag();
    //    //    }
    //    //    html.RenderEndTag();
    //    //    html.RenderEndTag();
    //    //    html.RenderEndTag();

    //    //    for (int i = 0; i < root.childs.Count; i++)
    //    //    {
    //    //        html.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("menu{0}", i + 1));
    //    //        html.RenderBeginTag(HtmlTextWriterTag.Table);
    //    //        html.RenderBeginTag(HtmlTextWriterTag.Tr);
    //    //        html.AddAttribute(HtmlTextWriterAttribute.Class, "fill");
    //    //        html.RenderBeginTag(HtmlTextWriterTag.Td);
    //    //        html.RenderEndTag();
    //    //        html.RenderBeginTag(HtmlTextWriterTag.Td);
    //    //        RenderMenu3(html, root.childs[i], 0);
    //    //        html.RenderEndTag();
    //    //        html.RenderEndTag();
    //    //        html.RenderEndTag();
    //    //    }
    //    //}
    //    #endregion
    //    Menu0 = sb1.ToString();
    //    Menu1 = sb2.ToString();
    //}
    //void RenderMenuC(StringBuilder sb1, StringBuilder sb2, sysMenu.Node root, int depth)
    //{
    //    int index = 0;
    //    for (int i = 0; i < root.Count; i++)
    //    {
    //        sysMenu.Node node = root[i];
    //        // check permission
    //        if (depth == 0)
    //        {
    //            sb1.AppendFormat(@"<label class=""navbar"" for=""menu{0}"">{1}</label>", index + 1, node.Texts.Value);
    //            sb2.AppendFormat(@"<table id=""menu{0}"" class=""navbar""><tr><td class=""fill""></td><td>", index + 1);
    //        }
    //        else
    //        {
    //            if (index == 0)
    //                sb2.AppendFormat("<ul{0}>", depth == 1 ? @" class=""menu""" : "");
    //            sb2.Append("<li><a");
    //            string url = node.Urls.Value;
    //            if (!string.IsNullOrEmpty(url))
    //                sb2.AppendFormat(@" href=""{0}"" target=""mainframe""", url);
    //            sb2.AppendFormat(">{0}</a>", node.Texts.Value);
    //        }
    //        RenderMenuC(sb1, sb2, node, depth + 1);
    //        sb2.Append(depth == 0 ? "</td></tr></table>" : "</li>");
    //        index++;
    //    }
    //    if (depth > 0)
    //        if (index > 0)
    //            sb2.Append("</ul>");
    //}
    ////public void RenderMenu3(HtmlTextWriter html, appMenu.Node node, int depth)
    ////{
    ////    if (node.childs.Count > 0)
    ////    {
    ////        if (depth == 0)
    ////            html.AddAttribute(HtmlTextWriterAttribute.Class, "menu");
    ////        html.RenderBeginTag(HtmlTextWriterTag.Ul);
    ////        for (int i = 0; i < node.childs.Count; i++)
    ////        {
    ////            html.RenderBeginTag(HtmlTextWriterTag.Li);
    ////            html.RenderBeginTag(HtmlTextWriterTag.A);
    ////            string url = node.childs[i].Url;
    ////            if (!string.IsNullOrEmpty(url))
    ////            {
    ////                html.AddAttribute(HtmlTextWriterAttribute.Href, url);
    ////                html.AddAttribute(HtmlTextWriterAttribute.Target, "mainframe");
    ////            }
    ////            html.Write(node.childs[i].Text);
    ////            html.RenderEndTag();
    ////            RenderMenu3(html, node.childs[i], depth + 1);
    ////            html.RenderEndTag();
    ////        }
    ////        html.RenderEndTag();
    ////    }
    ////}

    //public string RenderMenuA()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
    //    using (HtmlTextWriter html = new HtmlTextWriter(sw))
    //        RenderMenuA(html, sysMenu.Instance.Root, 0);
    //    return sb.ToString();
    //}
    //void RenderMenuA(HtmlTextWriter html, sysMenu.Node root, int depth)
    //{
    //    for (int i = 0; i < root.Count; i++)
    //    {
    //        sysMenu.Node item = root[i];
    //        if (depth == 0)
    //            html.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("root-{0}", i));
    //        html.RenderBeginTag(HtmlTextWriterTag.Li);
    //        string url = item.Urls.Value;
    //        if (!string.IsNullOrEmpty(url))
    //        {
    //            html.AddAttribute(HtmlTextWriterAttribute.Href, url);
    //            html.AddAttribute(HtmlTextWriterAttribute.Target, "mainframe");
    //        }
    //        html.RenderBeginTag(HtmlTextWriterTag.A);
    //        html.Write(item.Texts.Value);
    //        html.RenderEndTag();
    //        if (item.Count > 0)
    //        {
    //            html.RenderBeginTag(HtmlTextWriterTag.Ul);
    //            RenderMenuA(html, item, depth + 1);
    //            html.RenderEndTag();
    //        }
    //        html.RenderEndTag();
    //    }
    //}


    //public string RenderMenuB()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
    //    using (HtmlTextWriter html = new HtmlTextWriter(sw))
    //        RenderMenuB(html, 0);
    //    return sb.ToString();
    //}
    //void RenderMenuB(HtmlTextWriter html, int depth)
    //{
    //    for (int i = 0; i < 5; i++)
    //    {
    //        if (depth == 0)
    //            html.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("root-{0}", i));
    //        html.RenderBeginTag(HtmlTextWriterTag.Li);
    //        html.RenderBeginTag(HtmlTextWriterTag.A);
    //        html.Write(string.Format("[{0}-{1}]", i + 1, RandomString.GetRandomString(RandomString.LowerNumber, 5)));
    //        html.RenderEndTag();
    //        if (depth < 5)
    //        {
    //            html.RenderBeginTag(HtmlTextWriterTag.Ul);
    //            RenderMenuB(html, depth + 1);
    //            html.RenderEndTag();
    //        }
    //        html.RenderEndTag();
    //    }
    //}
}


