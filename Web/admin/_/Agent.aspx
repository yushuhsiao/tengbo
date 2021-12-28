<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/admin.master" AutoEventWireup="true" culture="auto" uiculture="auto" CodeBehind="Agent.aspx.cs" Inherits="page" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var $table;
        //var currencys = { 8: 'ALL', 12: 'DZD', 32: 'ARS', 36: 'AUD', 44: 'BSD', 48: 'BHD', 50: 'BDT', 51: 'AMD', 52: 'BBD', 60: 'BMD', 64: 'BTN', 68: 'BOB', 72: 'BWP', 84: 'BZD', 90: 'SBD', 96: 'BND', 104: 'MMK', 108: 'BIF', 116: 'KHR', 124: 'CAD', 132: 'CVE', 136: 'KYD', 144: 'LKR', 152: 'CLP', 156: 'CNY', 170: 'COP', 174: 'KMF', 188: 'CRC', 191: 'HRK', 192: 'CUP', 203: 'CZK', 208: 'DKK', 214: 'DOP', 230: 'ETB', 232: 'ERN', 238: 'FKP', 242: 'FJD', 262: 'DJF', 270: 'GMD', 292: 'GIP', 320: 'GTQ', 324: 'GNF', 328: 'GYD', 332: 'HTG', 340: 'HNL', 344: 'HKD', 348: 'HUF', 352: 'ISK', 356: 'INR', 360: 'IDR', 364: 'IRR', 368: 'IQD', 376: 'ILS', 388: 'JMD', 392: 'JPY', 398: 'KZT', 400: 'JOD', 404: 'KES', 408: 'KPW', 410: 'KRW', 414: 'KWD', 417: 'KGS', 418: 'LAK', 422: 'LBP', 426: 'LSL', 428: 'LVL', 430: 'LRD', 434: 'LYD', 440: 'LTL', 446: 'MOP', 454: 'MWK', 458: 'MYR', 462: 'MVR', 478: 'MRO', 480: 'MUR', 484: 'MXN', 496: 'MNT', 498: 'MDL', 504: 'MAD', 512: 'OMR', 516: 'NAD', 524: 'NPR', 532: 'ANG', 533: 'AWG', 548: 'VUV', 554: 'NZD', 558: 'NIO', 566: 'NGN', 578: 'NOK', 586: 'PKR', 590: 'PAB', 598: 'PGK', 600: 'PYG', 604: 'PEN', 608: 'PHP', 634: 'QAR', 643: 'RUB', 646: 'RWF', 654: 'SHP', 678: 'STD', 682: 'SAR', 690: 'SCR', 694: 'SLL', 702: 'SGD', 704: 'VND', 706: 'SOS', 710: 'ZAR', 728: 'SSP', 748: 'SZL', 752: 'SEK', 756: 'CHF', 760: 'SYP', 764: 'THB', 776: 'TOP', 780: 'TTD', 784: 'AED', 788: 'TND', 800: 'UGX', 807: 'MKD', 818: 'EGP', 826: 'GBP', 834: 'TZS', 840: 'USD', 858: 'UYU', 860: 'UZS', 882: 'WST', 886: 'YER', 901: 'TWD', 931: 'CUC', 934: 'TMT', 936: 'GHS', 937: 'VEF', 938: 'SDG', 940: 'UYI', 941: 'RSD', 943: 'MZN', 944: 'AZN', 946: 'RON', 947: 'CHE', 948: 'CHW', 949: 'TRY', 950: 'XAF', 951: 'XCD', 952: 'XOF', 953: 'XPF', 955: 'XBA', 956: 'XBB', 957: 'XBC', 958: 'XBD', 959: 'XAU', 960: 'XDR', 961: 'XAG', 962: 'XPT', 963: 'XTS', 964: 'XPD', 967: 'ZMW', 968: 'SRD', 969: 'MGA', 970: 'COU', 971: 'AFN', 972: 'TJS', 973: 'AOA', 974: 'BYR', 975: 'BGN', 976: 'CDF', 977: 'BAM', 978: 'EUR', 979: 'MXV', 980: 'UAH', 981: 'GEL', 984: 'BOV', 985: 'PLN', 986: 'BRL', 990: 'CLF', 997: 'USN', 998: 'USS', 999: 'XXX'}

        $(document).ready(function () {
            $table = $('#table01').jqGrid_init({
                pager: true,  nav1: '#nav1', nav2: '#nav2',                
                SelectCommand: function (postData) { return { AgentSelect: postData } },
                UpdateCommand: function (postData) { return { AgentUpdate: postData } },
                InsertCommand: function (postData) { return { AgentInsert: postData } },

                colModel: [
                    { name: 'Action    ', label: '<%=lang["colAction    "]%>', colType: 'Buttons' },
                    { name: 'ID        ', label: '<%=lang["colID        "]%>', colType: 'ID'<%if (showID) { %>, hidden: false<% }%> },
                    { name: 'CorpID    ', label: '<%=lang["colCorpID    "]%>', width:75,colType: 'CorpID' },
                    { name: 'ParentACNT', label: '<%=lang["colParentACNT"]%>', colType: 'ACNT', search: true },
                    { name: 'ACNT      ', label: '<%=lang["colACNT      "]%>', colType: 'ACNT', search: true },
                    { name: 'GroupID   ', label: '<%=lang["colGroupID   "]%>', width: 080, sorttype: 'int', editable: true, editonce: false, formatter: 'select', formatoptions: {<%=serializeEnum<long,string>("value", web.AgentGroupRow.Cache.Instance.Value2)%> }, edittype: 'select', editoptions: { value_func: function (rowdata) { return <%=web.api.SerializeObject(web.AgentGroupRow.Cache.Instance.Value1)%>[rowdata.CorpID] || {}; } } },
                    { name: 'Name      ', label: '<%=lang["colName      "]%>', width: 080, sorttype: 'text', editable: true, search: true },
                    { name: 'Locked    ', label: '<%=lang["colLocked    "]%>', colType: 'Locked' },
                    { name: 'Currency  ', label: '<%=lang["colCurrency  "]%>', colType: 'Currency' },
                    { name: 'Balance   ', label: '<%=lang["colBalance   "]%>', width: 080, sorttype: 'currency', editable: false, formatter: 'currency' },
                    { name: 'PayShare  ', label: '<%=lang["colPayShare  "]%>', colType: 'Bonus' },
                  //{ name: 'BonusW    ', label: '<%=lang["colBonusW    "]%>', colType: 'Bonus' },
                  //{ name: 'BonusL    ', label: '<%=lang["colBonusL    "]%>', colType: 'Bonus' },
                    { name: 'MaxUser   ', label: '<%=lang["colMaxUser   "]%>', width: 080, sorttype: 'int', editable: true, hidden: false },
                    { name: 'MaxAgent  ', label: '<%=lang["colMaxAgent  "]%>', width: 080, sorttype: 'int', editable: true, hidden: false },
                    { name: 'MaxDepth  ', label: '<%=lang["colMaxDepth  "]%>', width: 080, sorttype: 'int', editable: true, hidden: false },
                    { name: 'CreateTime', label: '<%=lang["colCreateTime"]%>', colType: 'DateTime' },
                    { name: 'CreateUser', label: '<%=lang["colCreateUser"]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime', label: '<%=lang["colModifyTime"]%>', colType: 'DateTime' },
                    { name: 'ModifyUser', label: '<%=lang["colModifyUser"]%>', colType: 'ACNT2' },
                ]
                
                //onSelectRow: function (rowid) {
                //    //writelog("onSelectRow", {
                //    //    arguments: arguments,
                //    //    active: $toolbar1.groups.getActive('a'),
                //    //    rowdata: grid01.$table.getRowData(rowid)
                //    //});
                //}
            });
            //grid01.navGrid(grid01.pager, { edit: true, add: false, del: false, search: false, refresh: false, view: false});

            //$table.navToolbar({
            //    filterToolbar: { enabled: true }
            //});

            // 工具列
            //$('#btnSearch').button({ icons: { primary: 'ui-icon-search' } }).click(function () { $table[0].toggleToolbar(); $table.gridSize(window); }).css('border', 0);
            //$('#btnRefresh').button({ icons: { primary: 'ui-icon-refresh' } }).click(function () { $table.reloadGrid(); }).css('border', 0);
            //$('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div id="nav1">
        <button action="toggleSearch" icon="ui-icon-search" ><%=lang["btnSearch"]%></button>
        <button action="reloadGrid"   icon="ui-icon-refresh"><%=lang["btnRefresh"]%></button>
        <button action="addRow"       icon="ui-icon-plus"   ><%=lang["btnAdd"]%></button>
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
    <div id="nav2" class="ui-widget-content" style=""></div>
</asp:Content>
