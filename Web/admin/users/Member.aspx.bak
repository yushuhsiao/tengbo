<%@ Page Title="" Language="C#" MasterPageFile="~/sys/root.Master" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="web.page" %>

<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="web" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

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
    <script type="text/javascript">
        var $grid;
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
            $grid.jqxGrid({
                pkey: 'ID',
                rowdetails: true,
                showfilterrow: true,
                initrowdetails: function () { },
                rowdetailstemplate: {
                    rowdetails: "<div id='grid' style='background-color: red;'></div>",
                    rowdetailsheight: 50,
                    rowdetailshidden: true
                },
                width: window.innerWidth - 2, height: window.innerHeight - 2,
                showtoolbar: true, toolbarheight: $toolbar.outerHeight(), rendertoolbar: function (toolbardiv) { $toolbar.appendTo(toolbardiv); },
                columns: [
                    $.col('Number   ', {}),
                    $.col('Action1  ', { buttons: { remove: false, text: ['<%=lang["Actions_Edit"]%>', '<%=lang["Actions_Cancel"]%>', '<%=lang["Actions_Save"]%>'] } }),
                    $.col('Action2  ', {}),
                    $.col('ID       ', {}),
                    $.col('CorpID   ', { datafield: 'CorpID       ', text: '<%=lang["colCorpID       "]%>', }),
                    $.col('         ', { datafield: 'ParentACNT   ', text: '<%=lang["colParentACNT   "]%>', }),
                    $.col('         ', { datafield: 'ACNT         ', text: '<%=lang["colACNT         "]%>', width: 120 }),
                    $.col('         ', { datafield: 'Name         ', text: '<%=lang["colName         "]%>', }),
                    $.col('GroupID  ', { datafield: 'GroupID      ', text: '<%=lang["colGroupID      "]%>', }),
                    $.col('Locked   ', { datafield: 'Locked       ', text: '<%=lang["colLocked       "]%>', }),
                    $.col('Balance  ', { datafield: 'Balance      ', text: '<%=lang["colBalance      "]%>', }),
                    $.col('         ', { datafield: 'Currency     ', text: '<%=lang["colCurrency     "]%>', }),
                    $.col('         ', { datafield: 'Memo         ', text: '<%=lang["colMemo         "]%>', }),
                    $.col('DateTime ', { datafield: 'CreateTime   ', text: '<%=lang["colCreateTime   "]%>', editable: false }),
                    $.col('         ', { datafield: 'RegisterIP   ', text: '<%=lang["colRegisterIP   "]%>', editable: false }),
                    $.col('DateTime ', { datafield: 'LoginTime    ', text: '<%=lang["colLoginTime    "]%>', editable: false }),
                    $.col('         ', { datafield: 'LoginIP      ', text: '<%=lang["colLoginIP      "]%>', editable: false }),
                    $.col('         ', { datafield: 'LoginCount   ', text: '<%=lang["colLoginCount   "]%>', editable: false }),
                    $.col('         ', { datafield: 'CreateUser   ', text: '<%=lang["colCreateUser   "]%>', editable: false }),
                    $.col('DateTime ', { datafield: 'ModifyTime   ', text: '<%=lang["colModifyTime   "]%>', editable: false }),
                    $.col('         ', { datafield: 'ModifyUser   ', text: '<%=lang["colModifyUser   "]%>', editable: false }),
                    $.col('DateTime ', { datafield: 'Birthday     ', text: '<%=lang["colBirthday     "]%>', }),
                    $.col('         ', { datafield: 'Tel          ', text: '<%=lang["colTel          "]%>', }),
                    $.col('         ', { datafield: 'Mail         ', text: '<%=lang["colMail         "]%>', }),
                    $.col('         ', { datafield: 'QQ           ', text: '<%=lang["colQQ           "]%>', }),
                    $.col('         ', { datafield: 'Introducer   ', text: '<%=lang["colIntroducer   "]%>', }),
                    $.col('         ', { datafield: 'Sex          ', text: '<%=lang["colSex          "]%>', }),
                    $.col('         ', { datafield: 'Addr         ', text: '<%=lang["colAddr         "]%>', }),
                    $.col('         ', { datafield: 'UserMemo     ', text: '<%=lang["colUserMemo     "]%>', }),
                ],
                extra: {
                    source: {
                        addrow: function (rowid, rowdata, position, commit) {
                            //console.log('addrow', arguments);
                            commit(true);
                            //$grid.grid.action_panel[rowid].EditRow();
                        },
                        deleterow: function (rowid, commit) {
                            commit(true);
                            //console.log('deleterow', arguments);
                        },
                        updaterow: function (rowid, newdata, commit) {
                            //console.log('updaterow', arguments);
                            commit(true);
                        },
                        filter: function (filters, recordsArray) {
                            //console.log('filter', arguments);
                        }
                    },
                    settings: {
                        SelectCommand: '<%=typeof(web.MemberSelect2).Name%>',
                    },
                    events: {
                        pagechanged: function (event) {
                            //console.log('pagechanged', arguments);
                            //$grid.grid.updatebounddata();
                        },
                        filter: function (event) {
                            //console.log('filter', arguments);
                            $grid.grid.updatebounddata();
                        },
                        sort: function (event) {
                            //console.log('sort', arguments);
                            $grid.grid.updatebounddata();
                        },
                    }
                }
            });
            $grid.grid = $grid.jqxGrid('getInstance');

            //var _rendercell = $grid.grid._rendercell;
            //$grid.grid._rendercell = function (u, f, j, s, d, q) {
            //    var ret = _rendercell.apply(this, arguments);
            //    if (f.cellsrenderer)
            //        f.cellsrenderer(u.getboundindex(j), f.datafield, s, d, f.getcolumnproperties(), j.bounddata, arguments)
            //    return ret;
            //}

            $(window).resize(function () {
                //$grid.grid._ResetEditor();
                $grid.jqxGrid({
                    width: window.innerWidth - 2,
                    height: window.innerHeight - 2,
                });
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
    <div id="jqxgrid"></div>
</asp:Content>
