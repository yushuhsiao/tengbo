<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="web.page" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        var $table;
        $(document).ready(function () {
            var _addChild = null;

            $table = $('#table01').jqGrid_init({
                pager: false, rownumbers: false, viewrecords: false, cmTemplate: { sortable: false }, sortname: 'Sort',
                navButton_action:{
                    addRoowNode: function () {
                        $(this).addRow({ initdata: { isLeaf: false } })
                    },
                },
                colModel: [
                    { name: 'ID         ', label: '<%=lang["colID           "]%>', colType: 'ID', hidden: false },
                    {
                        name: 'Action     ', label: '<%=lang["colAction       "]%>', width: 180, colType: 'Buttons', action: {
                            addChildNode: function (ind, rowid) {
                                var treeReader = $table[0].p.treeReader;
                                var rc = $table.getLocalRow(rowid);
                                var loaded = rc[treeReader.loaded];
                                var __addchild = { parentNode: rowid, initdata: { isLeaf: false } };
                                $table.expandNode(rc);
                                if (loaded)
                                    $table.addRow(__addchild);
                                else
                                    _addChild = __addchild;
                            }
                        }
                    },
                    { name: 'Name       ', label: '<%=lang["colName         "]%>', sorttype: 'text', editable: true },
                    { name: 'IsMenu     ', label: '<%=lang["colIsMenu       "]%>', width: 060, fixed: true, editable: true, formatter: 'checkbox', edittype: 'checkbox' },
                    { name: 'Sort       ', label: '<%=lang["colSort         "]%>', width: 100, fixed: true, editable: true },
                    { name: 'Code       ', label: '<%=lang["colCode         "]%>', width: 150, fixed: true, editable: true },
                    { name: 'Url        ', label: '<%=lang["colUrl          "]%>', width: 400, fixed: true, editable: true, align: 'left' },
                ],
                treeGrid: true, treeGridModel: 'adjacency', tree_root_level: 1, ExpandColumn: 'Name',
                treeReader: {
                    level_field: "level",
                    parent_id_field: "Parent",
                    leaf_field: "isLeaf",
                    expanded_field: "expanded",
                    loaded: "loaded",
                    icon_field: "icon"
                },
                SelectCommand: function (postData) { return { MenuSelect: postData } },
                InsertCommand: function (postData) { return { MenuRowCommand: postData } },
                UpdateCommand: function (postData) { return { MenuRowCommand: postData } },
                DeleteCommand: function (postData) { return { MenuRowCommand: postData } },
                gridComplete: function () {
                    var __addchild = _addChild;
                    _addChild = null;
                    if (__addchild != null) {
                        setTimeout(function () {
                            $table.addRow(__addchild);
                        }, 1000)
                        //console.log(__addchild);
                    }
                },
                jqGridAfterLoadComplete: function () {
                    console.log(arguments);
                }
                //onSelectRow: onSelectRow,
                //navButton_action: {
                //    resetSelection: function () {
                //        $table.resetSelection();
                //        onSelectRow();
                //    }
                //}
            });

            //$table = $('#table01').menuedit_init(0);
            // 工具列
            //$('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <table id="table01">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide"   action="editRow"      icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="edithide"   action="delRow"       icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="saveRow"      icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="restoreRow"   icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="restoreRow"   icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="saveRow"      icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                    <div class="edithide"   action="addChildNode" icon="ui-icon-plus"  ><%=lang["Actions_AddChild"]%></div>
                </span>
            </td>
        </tr>
        <tr class="grid-option">
            <td>
                <div name="nav1"><button action="addRoowNode"       icon="ui-icon-plus"   ><%=lang["Actions_AddRoot"]%></button></div>
                <div name="nav2" class="ui-widget-content" style=""></div>
            </td>
        </tr>
    </table>
</asp:Content>