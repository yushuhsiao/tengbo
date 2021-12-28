<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="AgentTree.aspx.cs" Inherits="web.page" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css"> .ui-resizable-e { cursor: col-resize; } </style>
    <script type="text/javascript">
        var $table;

        var recvMessage = {
            AgentTree_GetSelect: function (data) {
                onSelectRow();
            }
            //MemberRowData: function (data) {
            //    $table_m.restoreRow(data.ID);
            //    $table_m.setRowData(data.ID, data);
            //},
            //AgentRowData: function (data) {
            //    $table_a.restoreRow(data.ID);
            //    $table_a.setRowData(data.ID, data);
            //}
        }

        function onSelectRow(rowid, status, e, force) {
            var selrow = $table.getGridParam('selrow');
            $('button[action=resetSelection]').button("option", "disabled", selrow == null);
            sendMessage('AgentTree_Select', selrow)
        };

        $(document).ready(function () {

            $table = $('#table_tree').jqGrid_init({
                pager: false, rownumbers: false, viewrecords: false, cmTemplate: { sortable: false },
                colModel: [
                    { name: 'DisplayName', label: '<%=lang["colACNT      "]%>', width: 200, editable: false, editonce: false, align: 'left', formatter: function (cellval, opts, rwd, act) { if (rwd.ACNT == rwd.Name) return rwd.ACNT; else return '{0} ({1})'.format(rwd.ACNT, rwd.Name); } },
                    { name: 'CorpID     ', label: '<%=lang["colCorpID    "]%>', colType: 'CorpID'<% if (showCorpID) { %>, hidden: false <% } %> },
                    { name: 'ACNT       ', label: '<%=lang["colACNT      "]%>', width: 080, editable: false, editonce: false, align: 'left' },
                    { name: 'Name       ', label: '<%=lang["colName      "]%>', width: 080, editable: false, hidden: true },
                    { name: 'ID         ', label: '<%=lang["colID        "]%>', width: 100, colType: 'ID'<%if (showID) { %>, hidden: false<% }%> }
                ],
                treeGrid: true, treeGridModel: 'adjacency', tree_root_level: 1, ExpandColumn: 'DisplayName',
                treeReader: {
                    level_field: "level",
                    parent_id_field: "ParentID",
                    leaf_field: "isLeaf",
                    expanded_field: "expanded",
                    loaded: "loaded",
                    icon_field: "icon"
                },
                SelectCommand: function (postData) { return { AgentTreeSelect: postData } },
                onSelectRow: onSelectRow,
                navButton_action: {
                    reloadGrid: function () {
                        $table.resetSelection();
                        $table.reloadGrid();
                        onSelectRow();
                    },
                    resetSelection: function () {
                        $table.resetSelection();
                        onSelectRow();
                    }
                }
            });

            $.GetValue.AgentTree_selrow = function () {
                return $table.getGridParam('selrow');
            }

            $table.gridContainer().removeClass('ui-corner-all');

            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
    <style type="text/css">
        /*.detail-content-loading { position: absolute; left: 8px; top: 5px; }*/
        .detail-content-loading div { background: url(../images/loading3_000000.gif) #fff no-repeat center center; width: 32px; height: 32px; margin: 1px; border-width: 1px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <table id="table_tree">
        <tr class="grid-option">
            <td>
                <div name="nav1" style="padding-left: 10px;">
                    <button action="resetSelection" icon="ui-icon-comment" disabled="true"><%=lang["btnResetSelT"]%></button>
                    <button action="reloadGrid"     icon="ui-icon-refresh"><%=lang["btnRefresh"]%></button>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
