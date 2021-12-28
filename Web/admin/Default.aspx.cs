using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using web;

public partial class default_aspx : page
{
    public static string[] themes = new string[] {
        "base", 
        "black-tie", 
        "blitzer", 
        "cupertino", 
        "dark-hive", 
        "dot-luv", 
        "eggplant", 
        "excite-bike", 
        "flick", 
        "hot-sneaks", 
        "humanity", 
        "le-frog", 
        "mint-choc", 
        "overcast", 
        "pepper-grinder", 
        "redmond", 
        "smoothness", 
        "south-street", 
        "start", 
        "sunny", 
        "swanky-purse", 
        "trontastic", 
        "ui-darkness", 
        "ui-lightness",
    };
    public static string[] themes_jqx = new string[] {
        "web",
        "orange",
        "summer",
        "metro",
        "metrodark",
        "classic",
        "energyblue",
        "fresh",
        "office",
        "shinyblack",
        "bootstrap",
        "darkblue",
        "black",
        "arctic",
        "highcontrast",

        //"mobile",
        //"android",
        //"blackberry",
        //"windowsphone",

        "ui-darkness",
        "ui-le-frog",
        "ui-lightness",
        "ui-overcast",
        "ui-redmond",
        "ui-smoothness",
        "ui-start",
        "ui-sunny",
    };
    public static string[] themes_jqui = new string[]{
    };

    protected class menu_node
    {
        public int id;
        public string href;
        public string label;
        //public string text { get { return label; } }
        public List<menu_node> items;
    }

    protected menu_node menu;

    void build_menu(MenuRow a, menu_node b)
    {
        if (a.Childs == null) return;
        if (a.Childs.Count == 0) return;
        foreach (MenuRow a1 in a.Childs)
        {
            if ((a1.IsMenu == true) && (this.User.Permissions[a1.Code, BU.Permissions.Flag.Read] == true))
            {
                string url = null;
                if (!string.IsNullOrEmpty(a1.Url))
                    url = ResolveClientUrl(a1.Url);
                menu_node b1 = new menu_node()
                {
                    id = a1.ID,
                    href = url,
                    label = Lang.GetLang("menu", a1.Code) ?? a1.Name
                };
                //using (System.Data.SqlClient.SqlCmd sqlcmd = BU.DB.Open(BU.DB.Name.Main, BU.DB.Access.ReadWrite))
                //{
                //    sqlcmd.ExecuteNonQuery(true, "update Lang set txt1='{0}' where cls='menu' and txt1='{1}' select * from Lang nolock where cls = 'menu'", a1.Code, a1.Name);
                //}
                //b1.label += b1.label;
                //b1.label += b1.label;
                b.items = b.items ?? new List<menu_node>();
                b.items.Add(b1);
                build_menu(a1, b1);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        build_menu(MenuRow.Cache.Instance.Root, this.menu = new menu_node());
    }

    public string GetMenuText(object menuItem)
    {
        MenuRow item = menuItem as MenuRow;
        if (item == null) return string.Empty;
        return Lang.GetLang("menu", item.Name) ?? item.Name;
    }
}

public partial class default_ascx : usercontrol
{
    public object DataItem { get; set; }

    default_aspx _Page
    {
        get { return (default_aspx)base.Page; }
    }

    protected override void RenderChildren(HtmlTextWriter writer)
    {
        this.RenderMenu(writer, this.DataItem as MenuRow);
        base.RenderChildren(writer);
    }

    void RenderMenu(HtmlTextWriter writer, MenuRow menu)
    {
        web.User user = HttpContext.Current.User as web.User;
        if (menu.Childs == null) return;
        writer.WriteLine();
        writer.WriteBeginTag("ul");
        if (menu == this.DataItem)
        {
            writer.WriteAttribute("class", "ui-menu");
        }
        writer.Write(">");
        foreach (MenuRow row in menu.Childs)
        {
            if (row.Sort < 0) continue;
            if (!user.Permissions[row.Code, BU.Permissions.Flag.Read])
                continue;
            writer.WriteLine();
            writer.WriteBeginTag("li");
            //writer.WriteAttribute("class", "ui-menu-item ui-widget-content");
            writer.Write(">");
            writer.WriteBeginTag("a");
            if (!string.IsNullOrEmpty(row.Url))
            {
                writer.WriteAttribute("target", "mainframe");
                writer.WriteAttribute("href", ResolveClientUrl(row.Url));
            }
            writer.Write(">");
            writer.Write(_Page.GetMenuText(row));
            writer.WriteEndTag("a");
            RenderMenu(writer, row);
            writer.WriteEndTag("li");
        }
        writer.WriteLine();
        writer.WriteEndTag("ul");
    }
}
