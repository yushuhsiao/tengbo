<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" culture="auto" uiculture="auto" CodeBehind="Code2.aspx.cs" Inherits="page" %>

<script runat="server">

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //}
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">

        var $table;

        //function _codes(rows) {
        //    var codes = { /*undefined: '*'*/ };
        //    codes[undefined] = 'N/A';
        //    for (var i in rows)
        //        codes[rows[i].Code] = rows[i].Code + ' - ' + rows[i].resid;
        //    return codes;
        //}

        $(document).ready(function () {

            var codes = { undefined: 'N/A' };

            function update_codes(rows) {
                if (!$.isArray(rows))
                    rows = $table.getRowData();
                if (!$.isArray(rows))
                    return;
                for (var i = 0; i < rows.length; i++) {
                    codes[rows[i].Code] = rows[i].Code + ' - ' + rows[i].resid;
                }
            }

            $table = $('#table01').jqGrid_init({
                pager: true,
                rowNum: 100,
                cmTemplate: { align: 'left' },
                loadonce: true,
                treeGrid: true,
                treeGridModel: 'adjacency',
                ExpandColumn: 'Code',
                nav1: '#nav1', nav2: '#nav2',
                treeReader: {
                    level_field: "level",
                    parent_id_field: "Parent",
                    leaf_field: "isLeaf",
                    expanded_field: "expanded",
                    loaded: "loaded",
                    icon_field: "icon"
                },
                nav1: '#nav1', nav2: '#nav2', msglist: '.msglist',
                SelectCommand: function (postData) { return { Code2Select: postData } },
                UpdateCommand: function (postData) { return { Code2Update: postData } },
                InsertCommand: function (postData) { return { Code2Insert: postData } },

                colModel: [
                    { name: 'Action ', label: '<%=lang["colAction"]%>', colType: 'Buttons' },
                    { name: 'newCode', label: '<%=lang["colCode  "]%>', width: 080, sorttype: 'int     ', editable: true, formatter: 'alias', formatoptions: 'Code' },
                    { name: 'Code   ', label: '<%=lang["colCode  "]%>', colType: 'ID', editable: true, hidden: true },
                    { name: 'resid  ', label: '<%=lang["colresid "]%>', width: 120, sorttype: 'text    ', editable: true },
                    { name: 'Parent ', label: '<%=lang["colParent"]%>', width: 150, sorttype: 'int     ', editable: true, formatter: 'select', edittype: 'select', editoptions: { value_func: function () { return codes; } } },
                    { name: 'Flag   ', label: '<%=lang["colFlag  "]%>', width: 050, sorttype: 'int     ', editable: true },
                    { name: 'Sort   ', label: '<%=lang["colSort  "]%>', width: 050, sorttype: 'int     ', editable: true },
                    { name: 'Path   ', label: '<%=lang["colPath  "]%>', width: 200, sorttype: 'text    ', editable: true },
                ],
                editParams: {
                    aftersavefunc: function (rowid, res) {
                        var id1 = $table.getDataIDs();
                        var tmp = [];
                        for (var id2 in id1) {
                            var rowid = id1[id2];
                            tmp[rowid] = $table.getCell(rowid, 'Parent');
                        }
                        update_codes();
                        //console.log(codes);
                        for (var id2 in id1) {
                            var rowid = id1[id2];
                            $table.setCell(rowid, 'Parent', tmp[rowid]);
                        }
                    }
                },
                beforeProcessing: function (data, status, xhr) {
                    update_codes(data.rows);
                    if (this.p.treeGrid == true) {
                        $table.resizeColumn("newCode", 200)
                        $table.hideCol('Action');
                        $table.hideCol('Parent');
                        $table.treeGridConvertRows(data);
                    }
                },
                gridComplete: update_codes,
            });
            $table.colModel('Parent').editoptions.value = codes;
            //$table.resizeColumn(4, 100);
            

            // 工具列
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);

            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div id="nav1">
        <button id="btnAdd"><%=lang["btnAdd"]%></button>
    </div>
    <table id="table01">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
    </table>
    <div id="nav2" class="ui-widget-content" style=""><table class="msglist"></table></div>
</asp:Content>