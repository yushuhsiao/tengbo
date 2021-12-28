<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" culture="auto" uiculture="auto" meta:resourcekey="PageResource1" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>

<script runat="server">
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //}
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        /*.ui-jqgrid .ui-userdata { height : auto;	}*/
    </style>
    <script type="text/javascript">
        var $table;

        $(document).ready(function () {
            var colBankID;

            $table = $('#table01').jqGrid_init({
                pager: true,
                toppager: false,
                loadonce: true,
                subGrid: true,
                toolbar2: ['#nav1', '#nav2'],
                SelectCommand: function (postData) { return { BillboardSelect: postData } },
                UpdateCommand: function (postData) { return { BillboardUpdate: postData } },
                InsertCommand: function (postData) { return { BillboardInsert: postData } },

                colModel: [
                    { name: 'rowid   ' },
                    { name: 'Action  ', label: '<%=lang.d[this, "colAction"]%>', buttons: { edittext: '<%=lang.d[this,"actionEdit"]%>', canceltext: '<%=lang.d[this,"actionCancel"]%>', savetext: '<%=lang.d[this,"actionSave"]%>' }, },
                    { name: 'ID      ', label: '<%=lang.d[this, "colID    "]%>', },
                    { name: 'Disabled', label: '<%=lang.d[this, "colLocked"]%>', colModel_src: 'Locked', _list: {<%=lang.d[this,"Locked"]%> } },
                    { name: 'CorpID  ', label: '<%=lang.d[this, "colCorpID"]%>', },
                    { name: 'Place   ', label: '<%=lang.d[this, "colPlace "]%>', width: 080, sorttype: "int", editable: true, edittype: 'select', editoptions: { value: { 1: 1, 2: 2, 3: 3, 4: 4, 5: 5, 6: 6, 7: 7, 8: 8, 9: 9, 10: 10 } }, formatter: 'select', formatoptions: { value: { 1: 1, 2: 2, 3: 3, 4: 4, 5: 5, 6: 6, 7: 7, 8: 8, 9: 9, 10: 10 } }, },
                    //{
                    //    name: 'Place     ', label: '<%=lang.d[this, "colPlace"]     %>', width: 080, sorttype: "int", editable: true,
                    //    edittype: 'select', editoptions: {
                    //        custom_element: function (vl, options) {
                    //            var n = 0;
                    //        },
                    //        custom_value: function () {
                    //            var n = 0;
                    //        }
                    //    }, formatter: 'select',
                    //},
                    { name: 'MemberID  ', label: '<%=lang.d[this, "colMemberID"]  %>', width: 100, sorttype: "text", editable: true, },
                    { name: 'CreateTime', label: '<%=lang.d[this, "colCreateTime"]%>', },
                    { name: 'CreateUser', label: '<%=lang.d[this, "colCreateUser"]%>', },
                    { name: 'ModifyTime', label: '<%=lang.d[this, "colModifyTime"]%>', },
                    { name: 'ModifyUser', label: '<%=lang.d[this, "colModifyUser"]%>', },
                ],

                beforeProcessing: function (data, status, xhr) {
                    $.colModel.update($table, 'CorpID', data.corps);
                    return data.rows != null;
                },
            });

            //grid01.navGrid(grid01.topPager, { search: false, edit: false, add: false, del: false, refresh: false });

            //grid01.navButtonAdd({ caption: '<%=lang.d[this, "btnSearch0"]%>', buttonicon: 'ui-icon-search', onClickButton: function () { grid01.$table[0].triggerToolbar(); } });


            // 工具列
            //grid01.$toolbar1.css('height', 'auto');
            //$('#toolbar1').appendTo(grid01.$toolbar1);
            $('#btnToggleSearch').button({ icons: { primary: 'ui-icon-pin-s' } }).change(function () {
                if ($(this).prop('checked'))
                    $('#search').show('fast', null, resize);
                else
                    $('#search').hide('fast', null, resize);
            }).css('border', 0);
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $('#subview_opt').buttonset({ icons: { primary: 'ui-icon-comment' } }).css('border', 0);

            $table[0].grid.dock();
        });

        var resize = function () {
            $table[0].grid.totalWidth($(window).innerWidth());
            $table[0].grid.totalHeight($(window).innerHeight());
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div id="nav1">
        <div>
            <input type="checkbox" id="btnToggleSearch" /><label for="btnToggleSearch" style="border:0;"><%=lang.d[this, "btnToggleSearch"]%></label>
            <button id="btnAdd"><%=lang.d[this, "btnAdd"]%></button>
            <div id="subview_opt" style="display:inline-block;">
                <input type="radio" name="detail" id="btnDetail" checked="checked" /><label for="btnDetail"><%= lang.d[this, "btnDetail"]  %></label>
                <input type="radio" name="detail" id="btnSubAcc" /><label for="btnSubAcc"><%= lang.d[this, "btnSubAcc"]  %></label>
                <input type="radio" name="detail" id="btnAddPoint" /><label for="btnAddPoint"><%= lang.d[this, "btnAddPoint"]  %></label>
                <input type="radio" name="detail" id="btnReport" /><label for="btnReport"><%= lang.d[this, "btnReport"]  %></label>
                <input type="radio" name="detail" id="btnBonus" /><label for="btnBonus"><%= lang.d[this, "btnBonus"]  %></label>
                <input type="radio" name="detail" id="btnLog" /><label for="btnLog"><%= lang.d[this, "btnLog"]  %></label>
            </div>
        </div>
        <div id="search" class="ui-widget-content" style="display: none;">
            最近一百筆資料<br />
            1<br />
            1<br />
        </div>
    </div>

    <table id="table01"></table>

    <div id="nav2" class="ui-widget-content" style="display:none;">
        <div>
            1<br />
            2<br />
            3<br />
        </div>
    </div>
</asp:Content>