<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/admin/page.master" CodeBehind="IPS.aspx.cs"  Culture="auto" UICulture="auto" Inherits="page" meta:resourcekey="PageResource1"%>
<%@ Import Namespace="BU" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var $table;

        $(document).ready(function () {
           
            $table = $('#table01').jqGrid_init({
                pager: true,
                caption: "环迅支付列表",
                nav1: $('#nav1'), nav2: $('#nav2'),
                multiselect: true,
                SelectCommand: function (postData) { return { IPSSelect: postData } },
                UpdateCommand: function (postData) { return { IPSRowRequest: postData } },
                InsertCommand: function (postData) { return { IPSRowRequest: postData } },
                colModel: [
                    //{ name: 'Action        ', label: '编辑', colType: { 'Buttons   ': { src: $('#inline-buttons > *') } } },
                    { name: 'Action        ', label: '编辑', colType: 'Buttons' },
                    { name: 'ID            ', label: '编号', colType: 'ID',<%=showID%>},
                    { name: 'IPSName        ', label: '支付名称', width: 100, editable: true,sorttype:'text' },
                    { name: 'Locked        ', label: '启用状态', colType: 'Locked', editoptions: {<%=enumlist<Locked>("value")%> } },
                    { name: 'MerCode       ', label: '商户编号', width: 080, editable: true },
                    { name: 'SubmitUrl     ', label: '提交网址', width: 280, editable: true },
                    { name: 'MerchantKey    ', label: '商户证书', width: 280, editable: true },
                    { name: 'CreateTime    ', label: '创建时间', colType: 'DateTime', formatter: { 'datejs': { format: 'yyyy-MM-dd\r\nHH:mm:ss', formatNaN: 'N/A' }, } },
                    { name: 'CreateUser    ', label: '创建人', colType: 'ACNT', editable: false },
                    { name: 'ModifyTime    ', label: '修改时间', colType: 'DateTime', formatter: { 'datejs': { format: 'yyyy-MM-dd\r\nHH:mm:ss', formatNaN: 'N/A' }, } },
                    { name: 'ModifyUser    ', label: '修改人', colType: 'ACNT', editable: false, }
                ],

                beforeProcessing: function (data, status, xhr) {
                    //writelog("beforeProcessing", arguments);
                    ////if (data.banks) {
                    ////    banks = {}, urls = {};
                    ////    for (var n in data.banks) {
                    ////        var d = data.banks[n];
                    ////        if (d.ID) {
                    ////            banks[d.ID] = d.Name;
                    ////            urls[d.ID] = d.WebATM;
                    ////        }
                    ////    }
                    ////    $.colModel.update($table, 'BankID', banks);
                    ////}
                    ////return data.rows != null;
                },
            });

            //grid01.navGrid(grid01.topPager, { search: false, edit: false, add: false, del: false, refresh: false });

            


            // 工具列
            //grid01.$toolbar1.css('height', 'auto');
            //$('#toolbar1').appendTo(grid01.$toolbar1);
            $('#btnToggleSearch').button({ icons: { primary: 'ui-icon-pin-s' } }).change(function () {
                //if ($(this).prop('checked'))
                //    $('#search').show('fast', null, resize);
                //else
                //    $('#search').hide('fast', null, resize);
                $('#search').toggle('fast');
            }).css('border', 0);
            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $('#subview_opt').buttonset({ icons: { primary: 'ui-icon-comment' } }).css('border', 0);
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
            $('#btnDetail').click(
                function () {
                    var s;
                    s = jQuery("#table01").jqGrid('getGridParam', 'selarrrow');

                    alert(s);
                }
                )
        });

        var resize = function () {
            var w = $(window).innerWidth(), h = $(window).innerHeight() - 5;

            $table[0].g.width(w);

            var h1 = $('#nav1').height();
            var h2 = $('#nav2').height();
            h2 = 0;
            $table[0].g.height(h - h1 - h2);
        };

       
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
      <div id="nav1">
        <div>
            <input type="checkbox" id="btnToggleSearch" /><label for="btnToggleSearch" style="border:0;">查询</label>
            <button id="btnAdd">新增</button>
            <div id="subview_opt" style="display:inline-block;">
                <input type="radio" name="detail" id="btnDetail" checked="checked" /><label for="btnDetail">显示ID</label>
                <input type="radio" name="detail" id="btnSubAcc" /><label for="btnSubAcc">XXXX</label>
                <input type="radio" name="detail" id="btnAddPoint" /><label for="btnAddPoint">XXXX</label>
                <input type="radio" name="detail" id="btnReport" /><label for="btnReport">XXXX</label>
                <input type="radio" name="detail" id="btnBonus" /><label for="btnBonus">XXXX</label>
                <input type="radio" name="detail" id="btnLog" /><label for="btnLog">XXXX</label>
            </div>
        </div>
    </div>

    <table id="table01">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="inline-button edithide" action="editRow"><span class="ui-icon ui-icon-pencil"></span><%=lang["actionEdit"]%></div>
                    <div class="inline-button editshow" action="restoreRow"><span class="ui-icon ui-icon-cancel"></span><%=lang["actionCancel"]%></div>
                    <div class="inline-button editshow" action="saveRow"><span class="ui-icon ui-icon-disk"></span><%=lang["actionSave"]%></div>
                </span>
            </td>
        </tr>
    </table>

    <div id="nav2">
       
    </div>
    </asp:Content>