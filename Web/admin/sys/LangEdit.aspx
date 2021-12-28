<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="web.page" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>


<asp:Content ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        var $table;
        $(document).ready(function () {
            $table = $('#table01').jqGrid_init({
                shrinkToFit: true, loadonce: true, rownumbers: false, rowNum: 10000, subGrid: true, sortname: 'key1A', sortorder: "asc",
                cmTemplate: { align: 'left' }, addRowParams: { position: 'last' },
                SelectCommand: function (postData) { return { LangSelect1: postData } },
                UpdateCommand: function (postData) { return { LangUpdate1: postData } },
                InsertCommand: function (postData) { return { LangInsert1: postData } },
                DeleteCommand: function (postData) { return { LangDelete1: postData } },

                colModel: [
                    { name: 'Action     ', label: '<%=lang["colAction     "]%>', colType: 'Buttons' },
                    { name: 'key1A      ', label: '<%=lang["colID         "]%>', width: 100, editable: false, key: true, hidden: true },
                    { name: 'key1B      ', label: '<%=lang["colClass      "]%>', width: 100, editable: true },
                ],
                subGridRowCreated: function (pID, id, ind, tablediv) {
                    $(tablediv).parent().css({ 'border-width': 0, });
                    var $table2 = $('.subitem_table').clone(true, true).removeClass('subitem_table').prop('id', pID).appendTo(tablediv).show();
                    var p = {
                        shrinkToFit: true, loadonce: true, height: 'auto', sortable: false, rownumbers: false, rowNum: 10000, autowidth: true, sortname: 'txt', sortorder: "asc",
                        //nav1 : '<label></label>', nav2: $('<label><%=lang["btnAdd"]%></label>'),
                        cmTemplate: { align: 'left' }, addRowParams: { position: 'last' },
                        SelectCommand: function (postData) { console.log(postData, $table.getRowData(id)); postData.key1 = $table.getRowData(id).key1A; return { LangSelect2: postData } },
                        UpdateCommand: function (postData) { console.log(postData, $table.getRowData(id)); postData.key1 = $table.getRowData(id).key1A; return { LangUpdate2: postData } },
                        InsertCommand: function (postData) { console.log(postData, $table.getRowData(id)); postData.key1 = $table.getRowData(id).key1A; return { LangInsert2: postData } },
                        DeleteCommand: function (postData) { console.log(postData, $table.getRowData(id)); postData.key1 = $table.getRowData(id).key1A; return { LangDelete2: postData } },

                        colModel: [
                            { name: 'Action     ', label: '<%=lang["colAction     "]%>', colType: 'Buttons' },
                            { name: 'txtA       ', label: '<%=lang["colTxt_       "]%>', key: true, width: 100, hidden: true },
                            { name: 'txtB       ', label: '<%=lang["colTxt        "]%>', width: 250, fixed: true, editable: true },
                            { name: 'en_us      ', label: '<%=lang["colTxt_en     "]%>', width: 200, fixed: true, editable: true },
                            { name: 'zh_cht     ', label: '<%=lang["colTxt_cht    "]%>', width: 200, fixed: true, editable: true },
                            { name: 'zh_chs     ', label: '<%=lang["colTxt_chs    "]%>', width: 200, fixed: true, editable: true },
                            { label: '.' }
                        ]
                    };

                    $table2 = $table2.jqGrid_init(p);
                    $table2[0].grid.$bottomtoolbar.removeClass('ui-state-default');
                    $('.ui-th-column', tablediv).removeClass('ui-state-default').addClass('ui-widget-content');
                    //$('th.ui-state-default', $table2[0].grid.$rowheader).removeClass('ui-state-default');
                    $table2.gridContainer().removeClass('ui-corner-all').css({ 'border-top-width': 0 });
                    $table2[0].p.nav2.button({ icons: { primary: 'ui-icon-plus' } }).click($table2[0].addRow).css('border', 0).removeClass('ui-state-default');
                },
            });
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" Runat="Server">
    <table id="table01">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide"   action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="edithide"   action="delRow"     icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="saveRow"    icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
        <tr class="grid-option">
            <td>
                <div name="nav1">
                    <button action="addRow"       icon="ui-icon-plus"   ><%=lang["btnAdd"]??"Add"%></button>
                </div>
                <div name="nav2" class="ui-widget-content" style=""></div>
            </td>
        </tr>
    </table>
    <table class="subitem_table" style="display: none;">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide"   action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="edithide"   action="delRow"     icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="saveRow"    icon="ui-icon-trash" ><%=lang["Actions_Delete"]%></div>
                    <div class="deleteshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow"   action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
        <tr class="grid-option">
            <td>
                <label name="nav1"></label>
                <label name="nav2"><%=lang["btnAdd"]%></label>
            </td>
        </tr>
    </table>
</asp:Content>