<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/admin.master" AutoEventWireup="true" culture="auto" uiculture="auto" CodeBehind="Member.aspx.cs" Inherits="page" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<script runat="server">
</script>
<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var $table;

        $(document).ready(function () {
            $table = $('#table01').jqGrid_init({
                pager: true, subGrid: true, nav1: '#nav1', nav2: '#nav2',                
                SelectCommand: function (postData) { return { MemberSelect: postData } },
                UpdateCommand: function (postData) { return { MemberUpdate: postData } },
                InsertCommand: function (postData) { return { MemberInsert: postData } },

                colModel: [
                    { name: 'Action      ', label: '<%=lang["colAction      "]%>', colType: 'Buttons' },
                    { name: 'ID          ', label: '<%=lang["colID          "]%>', colType: 'ID'<%if (showID) { %>, hidden: false<% }%>, search: false },
                    { name: 'CorpID      ', label: '<%=lang["colCorpID      "]%>', colType: 'CorpID'<%if (showCorpID) { %>, hidden: false<% }%> },
                    { name: 'AgentACNT   ', label: '<%=lang["colAgentACNT   "]%>', colType: 'ACNT', search: true, editable: true, editonce: true },
                    { name: 'ACNT        ', label: '<%=lang["colACNT        "]%>', colType: 'ACNT', width: 120, search: true },
                    { name: 'Name        ', label: '<%=lang["colName        "]%>', width: 080, search: true, editable: true },
                    { name: 'GroupID     ', label: '<%=lang["colGroupID     "]%>', width: 080, editable: true, editonce: false, formatter: 'select', formatoptions: {<%=serializeEnum<long,string>("value", web.MemberGroupRow.Cache.Instance.Value2)%> }, edittype: 'select', editoptions: { value_func: function (rowdata) { return <%=web.api.SerializeObject(web.MemberGroupRow.Cache.Instance.Value1)%>[rowdata.CorpID] || {}; } } },
                    { name: 'Locked      ', label: '<%=lang["colLocked      "]%>', colType: 'Locked' },
                    { name: 'Balance     ', label: '<%=lang["colBalance     "]%>', width: 080, editable: false, formatter: 'currency' },
                    { name: 'Currency    ', label: '<%=lang["colCurrency    "]%>', colType: 'Currency' },
                    { name: 'Memo        ', label: '<%=lang["colMemo        "]%>', width: 080, editable: true },
                    { name: 'LoginTime   ', label: '<%=lang["colLoginTime   "]%>', colType: 'DateTime2' },
                    { name: 'LoginIP     ', label: '<%=lang["colLoginIP     "]%>', width: 100, editable: false },
                    { name: 'LoginCount  ', label: '<%=lang["colLoginCount  "]%>', width: 060, editable: false, editonce: false, hidden: true },
                    { name: 'CreateTime  ', label: '<%=lang["colCreateTime  "]%>', colType: 'DateTime2', sortable: true },
                  //{ name: 'RegisterIP  ', label: '<%=lang["colRegisterIP  "]%>', width: 100, editable: false },
                    { name: 'CreateUser  ', label: '<%=lang["colCreateUser  "]%>', colType: 'ACNT2' },
                    { name: 'ModifyTime  ', label: '<%=lang["colModifyTime  "]%>', colType: 'DateTime2' },
                    { name: 'ModifyUser  ', label: '<%=lang["colModifyUser  "]%>', colType: 'ACNT2' },
                ],            
                subGridBeforeExpand: function (pID, id, ind) { },
                subGridRowCreated: function (pID, id, ind, tablediv) {
                    //$(tablediv.parentElement).before('<td class="ui-widget-content subgrid-cell" style="vertical-align:top;"></td>');
                    //tablediv.parentElement.colSpan--;

                    //$('<iframe frameBorder="0" src="MemberDetail.aspx?id=' + id + '" style="width:98%; height:1px;"></iframe>').appendTo(tablediv);

                    //$table.setRowData(id, {ACNT:'123'});

                    var $table_d = $('.details-root').children().clone().appendTo(tablediv);
                    $('.detail-content iframe', tablediv).load(function () {
                        $('.detail-content-loading', tablediv).hide();
                    });
                    //.each(function () {
                    //    var iframe = this;
                    //    (function run() {
                    //        try{
                    //            var h1 = $(iframe.contentDocument.body.clientHeight).height();
                    //            var h2 = $(iframe).height();
                    //            console.log(this, { h1: h1, h2: h2 });
                    //            if (h1 == h2)
                    //                setTimeout(run, 1000);
                    //            else {
                    //                $(iframe).animate({ height: h1 }, 1000, null, function () {
                    //                    setTimeout(run, 1000);
                    //                });
                    //            }
                    //        } catch (e) {
                    //            setTimeout(run, 1000);
                    //        }
                    //        //console.log(this, Date.now(),
                    //        //    {
                    //        //        h1: $(iframe.contentDocument).height(),
                    //        //        h2: $(iframe).height()
                    //        //    });
                    //        //setTimeout(run, 1000);
                    //    })();
                    //});
                    //console.log($iframe);
                    //$('.detail-content iframe', tablediv).load(function () {
                    //    $('.detail-content-loading', tablediv).hide();
                    //    var iframe = this;
                    //    $(iframe.contentDocument).ready(function () {
                    //        console.log('123');
                    //    });
                    //    console.log($(iframe.contentDocument).height(), $(iframe).height());
                    //    
                    //
                    //    //console.log($(iframe.contentDocument).height(), this);
                    //    //$(iframe).height($(iframe.contentDocument).height());
                    //    //$(this).css('height', this.contentWindow.innerHeight);
                    //    //$(iframe.contentWindow).resize(function () {
                    //    //    console.log('c-resize', $( this).size());
                    //    //
                    //    //}).trigger('resize');
                    //    //console.log('load', iframe.contentWindow);
                    //    //console.log('load', $(iframe.contentDocument).outerHeight());
                    //    //console.log($(iframe.contentDocument.body).height(), $(iframe.contentDocument).outerHeight());
                    //    //$(iframe).height(iframe.contentDocument.body.offsetHeight);
                    //}).resize(function () {
                    //    //console.log('resize', this);
                    //    //iframe1.contentWindow.outerHeight
                    //});
                    $('.detail-nav div', tablediv).each(function () {
                        $(this)
                            .button({ icons: { primary: $(this).attr('icon') } }).removeClass('ui-corner-all').removeClass('ui-state-default').addClass('ui-widget-content')
                            .click(function () {
                                $('.detail-nav div', tablediv).removeClass('ui-state-highlight');
                                $(this).addClass('ui-state-highlight');
                                $('.detail-content iframe', tablediv).prop('src', ($(this).attr('url') + "?id=" + id));
                                $('.detail-content-loading', tablediv).show();
                            });
                    });
                    $('.detail-nav div.ui-state-highlight:eq(0)', tablediv).trigger('click');
                },
                subGridRowExpanded: function (pID, id, ind, tablediv) { },
                subGridBeforeColapsed: function (pID, id, ind, tablediv) { },
                subGridRowRemoved: function (pID, id, ind, tablediv) { },
                subGridRowColapsed: function (pID, id, ind, tablediv) { }
            });

            //$table.filterToolbar({ autosearch: true, searchOnEnter: false });
            //$table[0].toggleToolbar(false);


            // 工具列
            //$('#btnToggleSearch').button({ icons: { primary: 'ui-icon-pin-s' } }).change(function () {
            //    $table.toggleToolbar();
            //    if ($(this).prop('checked')) {
            //        //$('#search').show('fast', null, resize);
            //    }
            //    else {
            //        //$('#search').hide('fast', null, resize);
            //    }
            //});
            //$('#btnSearch').button({ icons: { primary: 'ui-icon-search' } }).click(function () { $table[0].toggleToolbar(); $table.gridSize(window); }).css('border', 0);
            //$('#btnRefresh').button({ icons: { primary: 'ui-icon-refresh' } }).click(function () { $table.reloadGrid(); }).css('border', 0);
            //$('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            //$('#subview_opt').buttonset({ icons: { primary: 'ui-icon-comment' } });

            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
    <style type="text/css">
        .detail-table { width: 100%; }
        .detail-nav { width:100px; padding-left: 1px; }
        .detail-nav, .detail-content { vertical-align: top; position: relative; }
        .detail-content { padding-left: 3px; }
        .detail-content iframe { width:98%; height:1px; }
        .detail-nav div { width: 100%; }
        .detail-content-loading { position: absolute; left: 8px; top: 5px; }
        .detail-content-loading div { background: url(../images/loading3_000000.gif) #fff no-repeat center center; width: 32px; height: 32px; margin: 1px; border-width: 1px; }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
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
    <div class="details-root" style="display: none;">
        <table class="detail-table" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td class="detail-nav">
                    <div icon="ui-icon-comment" url="Member1.aspx" class="">詳細資料</div>
                    <div icon="ui-icon-comment" url="Member2.aspx" class="ui-state-highlight">子帳戶</div>
                    <div icon="ui-icon-comment" url="Member3.aspx" class="ui-corner-bottom">金流</div>
                </td>
                <td class="detail-content">
                    <div class="ui-widget-content ui-state-active detail-content-loading" style="display: none;"><div></div></div>
                    <iframe frameBorder="0"></iframe>
                </td>
            </tr>
        </table>
    </div>
    <div style="display:none;">
        <iframe id="iframe1"></iframe>
    </div>
<%--
    <div id="_top_nav" class="ui-jqgrid ui-widget ui-widget-content ui-corner-all">
        <div id="toolbar1" class="ui-state-default ui-jqgrid-view ">
            <input type="checkbox" id="btnToggleSearch" /><label for="btnToggleSearch"><%=lang["btnToggleSearch"]%></label>
            <button id="btnAdd"><%=lang["btnAdd"]%></button>
            <div id="subview_opt" style="display:inline-block;">
                <input type="radio" name="detail" id="btnDetail" checked="checked" /><label for="btnDetail"><%=lang["btnDetail"]%></label>
                <input type="radio" name="detail" id="btnSubAcc" /><label for="btnSubAcc"><%=lang["btnSubAcc"]%></label>
                <input type="radio" name="detail" id="btnAddPoint" /><label for="btnAddPoint"><%=lang["btnAddPoint"]%></label>
                <input type="radio" name="detail" id="btnReport" /><label for="btnReport"><%=lang["btnReport"]%></label>
                <input type="radio" name="detail" id="btnBonus" /><label for="btnBonus"><%=lang["btnBonus"]%></label>
                <input type="radio" name="detail" id="btnLog" /><label for="btnLog"><%=lang["btnLog"]%></label>
            </div>
        </div>
        <div id="search" class="ui-jqgrid-view" style="display: none;">
            最近一百筆資料<br />
            1<br />
            1<br />
        </div>
    </div>


    <table id="table01"></table>
    <div id="pager01"></div>
    <div id="_buttom_nav" class="ui-jqgrid ui-widget ui-widget-content ui-corner-all">
        <table id="table2"></table>
        <div id="pager2"></div>
        <br />
        <table id="table3"></table>
        <div id="pager3"></div>
        <%--<div id="toolbar_src" class=""></div>
        <div id="detail" class="ui-jqgrid-view">detail view
    <input id="Radio1" type="radio" name="a" value="123"  /><input id="Radio2" type="radio" name="a" /><input id="Radio3" type="radio" />
        </div>
    </div>--%>
</asp:Content>
