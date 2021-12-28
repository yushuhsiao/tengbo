<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/admin/page.master"  Culture="auto" UICulture="auto" meta:resourcekey="PageResource1"%>
<%@ Import Namespace="BU" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var grid01;
        var $table;

        $(document).ready(function () {

            //grid01 = $('#table01').init_grid({
            $table = $('#table01').jqGrid_init({
                pager: true,
                toolbar2: ['#nav1', '#nav2'],
                SelectCommand: function (postData) { return { IPSSelect: postData } },
                UpdateCommand: function (postData) { return { IPSUpdate: postData } },
                InsertCommand: function (postData) { return { IPSInsert: postData } },

                colModel: [
                    { name: 'rowid         ' },
                    { name: 'Action        ', label: '编辑', buttons: { edittext: '编辑', canceltext: '取消', savetext: '保存' } },
                    { name: 'ID            ', label: '编号' },
                    { name: 'IPSName        ', label: '支付名称', editable: true },
                    { name: 'Locked        ', label: '启用状态', cmPreset: 'Enum', editable: true, editoptions: {<%=lang.d.Enum<Locked>(this,"value")%> } },
                    { name: 'MerCode       ', label: '商户编号', editable: true },
                    { name: 'SubmitUrl     ', label: '提交网址',  editable: true },
                    { name: 'MerchantKey    ', label: '商户证书', width: 080, sorttype: "text", editable: true },
                    { name: 'CreateTime    ', label: '创建时间', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss' } },
                    { name: 'CreateUser    ', label: '创建人' },
                    { name: 'ModifyTime    ', label: '修改时间', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss' } },
                    { name: 'ModifyUser    ', label: '修改人', }

                ],

                onSelectRow: function (rowid) {
                    //writelog("onSelectRow", {
                    //    arguments: arguments,
                    //    //active: $toolbar1.groups.getActive('a'),
                    //    rowdata: grid01.$table.getRowData(rowid)
                    //});
                },
                //caption: "Manipulating Array Data"
                beforeProcessing: function (data, status, xhr) {
                    if (data.rows) {
                        for (var i = 0; i < data.rows.length; i++) {
                            var row = data.rows[i];
                            row.reg_t = Date.fromISO(row.reg_t);
                            row.login_t = Date.fromISO(row.login_t);
                        }
                        return true;
                    }
                    return false;
                },
            });
            $table.setFrozenColumns();

            //for (var i = 1; i <= 100; i++)
            //    grid01.$table.addRowData(i, { "ID": i, "CorpACNT": "", "xxx": "xxx", "ACNT": "ccc" + i, "AgentACNT": "root", "Name": "ccc", "Currency": "USD", "Balance": 0.000000, "MemberType": 3, "RegisterIP": "192.168.3.2", "CreateTime": "2013-07-14T10:10:28.413" });
            //grid01.reloadGrid();
            //grid01.$table.setFrozenColumns();



            //grid01.navGrid(grid01.pager, { edit: true, add: false, del: false, search: false, refresh: false, view: false, });

            //grid01.$table.navGrid('toolbar2');


            // 工具列
            $('#btnToggleSearch').button({ icons: { primary: 'ui-icon-pin-s' } }).change(function () {
                if ($(this).prop('checked'))
                    $('#search').show('fast', null, resize);
                else
                    $('#search').hide('fast', null, resize);
            });
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $('#subview_opt').buttonset({ icons: { primary: 'ui-icon-comment' } });

            $table[0].grid.dock();
        });

        //var resize = function () {
        //    var w = $(window).innerWidth(), h = $(window).innerHeight() - 5;

        //    $table[0].g.width(w);

        //    var h1 = $('#nav1').height();
        //    var h2 = $('#nav2').height();
        //    h2 = 0;
        //    $table[0].g.height(h - h1 - h2);
        //};
        //var resize = function () {
        //    var w = $(window).innerWidth(), h = $(window).innerHeight();

        //    grid01.width(w);

        //    var h1 = $('#_top_nav').height();
        //    var h2 = grid01.height(h * 0.5);
        //    //writelog("resize", { grid_width: grid.width(w), grid_height: h1 });

        //    var h3 = h - h1 - h2 - 8;

        //    $('#_buttom_nav').height(h3);
        //    grid02.height(h3 - 1);
        //    grid03.height(h3 - 1);
        //};


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
      <div id="nav1">
        <div>
            <input type="checkbox" id="btnToggleSearch" /><label for="btnToggleSearch" style="border:0;">查询</label>
            <button id="btnAdd">新增</button>
            <div id="subview_opt" style="display:inline-block;">
                <input type="radio" name="detail" id="btnDetail" checked="checked" /><label for="btnDetail">XXXX</label>
                <input type="radio" name="detail" id="btnSubAcc" /><label for="btnSubAcc">XXXX</label>
                <input type="radio" name="detail" id="btnAddPoint" /><label for="btnAddPoint">XXXX</label>
                <input type="radio" name="detail" id="btnReport" /><label for="btnReport">XXXX</label>
                <input type="radio" name="detail" id="btnBonus" /><label for="btnBonus">XXXX</label>
                <input type="radio" name="detail" id="btnLog" /><label for="btnLog">XXXX</label>
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