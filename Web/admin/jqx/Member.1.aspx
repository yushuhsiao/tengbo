<%@ Page Title="" Language="C#" MasterPageFile="~/sys/root.Master" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="web.page" %>

<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="web" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register Src="~/jqx/jqxGrid.ascx" TagPrefix="uc1" TagName="jqxGrid" %>


<script runat="server">
    MenuRow menu_a;
    MenuRow menu_m;
    protected void Page_Load(object sender, EventArgs e)
    {
        MenuRow.Cache m = MenuRow.Cache.Instance;
        menu_a = m.GetItem(this.User, BU.Permissions.Code.agents_list);
        menu_m = m.GetItem(this.User, BU.Permissions.Code.members_list);
    }
</script>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .jqx-button.button-icon-text { padding : .3em .6em .3em .6em; }
        .jqx-button .button-icon { display: block; float: left; }
        .jqx-button .button-text { padding-left: .8em; }

        .grid-toolbar.jqx-menu { background-color: transparent; background-image: none !important; border: 0; }
        .grid-toolbar > ul { padding: 0; }
    </style>
    <script type="text/javascript">
        //var $grid;
        $.Message.themes.change = function () {
            $grid.jqxGrid({ theme: $.jqx.theme });
        };        

        <%
        List<object> corps = new List<object>();
        List<object> groups = new List<object>();
        foreach (CorpRow corp in CorpRow.Cache.Instance.Rows)
        {
            if ((User.CorpID == corp.ID) || (User.CorpID == 0))
            {
                List<object> grps = null;
                foreach (MemberGroupRow grp in (corp.MemberGroups ?? Tools._null<Dictionary<Guid, MemberGroupRow>>.value).Values)
                {
                    object grp2 = new { ID = grp.ID.Value, Name = grp.Name };
                    grps = grps ?? new List<object>();
                    grps.Add(grp2);
                    groups.Add(grp2);
                }
                corps.Add(new
                {
                    ID = corp.ID.Value,
                    Name = corp.Name,
                    Groups = grps,
                });
            }
        }
        object list = new
        {
            Corps = corps,
            Groups = groups,
        };
        %>
        $.extend(true, $.lists, $.o(<%=api.SerializeObject(list)%>));

        $(document).ready(function () {
            $grid = $("#jqxgrid");
            var $toolbar = $('#toolbar').jqxMenu().on('itemclick', function (event) {
                //var element = event.args;
                switch ($(event.args).attr('n')) {
                    case '1':
                        $grid.grid.AddRow({});
                        break;
                    case '2':
                        $grid.jqxGrid({ showfilterrow: !$grid.grid.showfilterrow });
                        break;
                    case '3':
                        $grid.grid.updatebounddata();
                        break;
                }
            });;

            $(window).resize(function () {
                //$grid.grid._ResetEditor();
                //$grid.jqxGrid({
                //    width: window.innerWidth - 2,
                //    height: window.innerHeight - 2,
                //});
            });

            //$grid.grid._bound();
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <div id="toolbar" class="grid-toolbar">
        <ul>
            <li n="1"><%=lang["btnAdd"]%></li>
            <li n="2"><%=lang["btnSearch"]%></li>
            <li n="3"><%=lang["btnRefresh"]%></li>
            <% if (menu_a != null) { %><li><a href="<%=ResolveClientUrl(menu_a.Url)%>"><%=lang["menu", menu_a.Name] ?? menu_a.Name%></a></li><% } %>
            <% if (menu_m != null) { %><li item-disabled="true"><a><%=lang["menu", menu_m.Name] ?? menu_m.Name%></a></li><% } %>
        </ul>
    </div>
    <%--<div id="jqxgrid"></div>--%>
    <uc1:jqxGrid runat="server" ID="jqxGrid1" toolbar="#toolbar" rtl="True">
    </uc1:jqxGrid>
</asp:Content>
