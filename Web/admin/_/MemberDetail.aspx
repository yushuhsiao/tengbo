<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/admin.master" AutoEventWireup="true" Inherits="page" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="BU" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="web" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var $info = [], $subacc = {}, $bankcard;

        $(document).ready(function () {
            var $nav_root = $('#nav');
            var $content_root = $('#content');
            var items = [];
            var colAction = { name: 'Action     ', label: '<%=lang["colAction       "]%>', colType: 'Buttons' };
            var colGameID = { name: 'GameID     ', label: '<%=lang["colGameID       "]%>', colType: 'ID', hidden: false, editable: false, formatter: 'select', edittype: 'select', editoptions: {<%=enumlist<GameID>("value")%> } };
            var colLocked = { name: 'Locked     ', label: '<%=lang["colLocked       "]%>', colType: 'Locked', editoptions: {<%=enumlist<Locked>("value")%> } };

            function item(elem) {
                var $nav = $('.nav', elem);
                var $content = $('.content', elem);

                this.set_active = function () {
                    $('.nav').removeClass('ui-state-highlight');
                    $('.content').hide();
                    $nav.addClass('ui-state-highlight');
                    $content.show();
                    iframe_auto_height();
                }

                $nav.appendTo($nav_root);
                $content.appendTo($content_root).hide();
                $nav.button({ icons: { primary: $nav.attr('icon') } }).removeClass('ui-corner-all').removeClass('ui-state-default').addClass('ui-widget-content');
                $nav.click(this.set_active);
                if ($(elem).hasClass('ui-state-active')) {
                    this.set_active();
                    $(elem).remove();
                }
            }
            $.fn.jqGrid_init2 = function (pin) {
                this.jqGrid_init($.extend(true, {
                    cmTemplate: { sortable: false },
                    cellEdit: true,
                    cellEditOnClick: false,
                    datatype: 'local',
                    height: 'auto',
                    sortable: false,
                    rownumbers: false,
                    autowidth: false,
                    editParams: { url: '.api', },
                    //onSelectCell: function (rowid, celname, value, iRow, iCol) { console.log("onSelectCell", arguments); return false; },
                    //beforeEditCell: function (rowid, cellname, value, iRow, iCol) { console.log("beforeEditCell", arguments); },
                    //afterEditCell: function (rowid, cellname, value, iRow, iCol) { console.log("afterEditCell", arguments); },
                    //beforeSaveCell: function (rowid, cellname, value, iRow, iCol) { console.log("beforeSaveCell", arguments); },
                    //afterSaveCell: function (rowid, cellname, value, iRow, iCol) { console.log("afterSaveCell", arguments); },
                    //beforeSubmitCell: function (rowid, cellname, value, iRow, iCol) { console.log("beforeSubmitCell", arguments); },
                    //afterSubmitCell: function (serverresponse, rowid, cellname, value, iRow, iCol) { console.log("afterSubmitCell", arguments); },
                    //afterRestoreCell: function (rowid, value, iRow, iCol) { console.log("afterRestoreCell", arguments); },
                    //errorCell: function (serverresponse, status) { console.log("errorCell", arguments); },
                    //formatCell: function (rowid, cellname, value, iRow, iCol) { console.log("formatCell", arguments); },
                    //serializeCellData: function (postdata) { console.log("serializeCellData", arguments); },
                }, pin));
                this.closest('tr').removeClass('hide');
                this.editCell(1, 0, false);
                return this;
            }
            <% 
        User user = HttpContext.Current.User as User;
        int? memberID = Request.QueryString["id"].ToInt32();
        if (!memberID.HasValue) return;
        using (SqlCmd sqlcmd = DB.Open(DB.Name.Main, DB.Access.ReadWrite))
        {
            MemberRow row = sqlcmd.ToObject<MemberRow>("select * from Member nolock where ID={0}", memberID);
            if (row == null) return;
            string rowdata = api.SerializeObject(row);
             %>
            var rowdata = [<%=rowdata%>];
            <%
            List<MemberBankCardRow> cards = new List<MemberBankCardRow>();
            foreach (SqlDataReader r in sqlcmd.ExecuteReader2("select * from MemberBank nolock where MemberID={0}", memberID))
                cards.Add(r.ToObject<MemberBankCardRow>());
            if (cards.Count == 0)
                cards.Add(new MemberBankCardRow());
                %>
            $bankcard = $(bankcard).jqGrid_init2({
                data: <%=api.SerializeObject(cards)%>,
                colModel: [
                    { name: 'Action     ', label: 'Banks', colType: 'Buttons' },
                    { name: 'BankName   ', label: '<%=lang["colBankName   "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'AccountName', label: '<%=lang["colAccountName"]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'CardID     ', label: '<%=lang["colCardID     "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Loc1       ', label: '<%=lang["colLoc1       "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Loc2       ', label: '<%=lang["colLoc2       "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Loc3       ', label: '<%=lang["colLoc3       "]%>', width: 080, sorttype: 'text', editable: true },
                ]
            });<%
            foreach (GameRow game in GameRow.Cache.GetInstance(null, sqlcmd).Rows)
            {
                switch (game.ID)
                {
                    case GameID.HG: {
                        MemberGameRow_HG subrow = MemberGameRowCommand_HG.Instance.SelectRow(sqlcmd, memberID.Value, null); %>
            $subacc.HG = $(s_HG).jqGrid_init2({
                data: [<%=api.SerializeObject(subrow ?? Tools._null<object>.value)%>],
                UpdateCommand: function (postData) { postData.MemberID = rowdata[0].ID; return { MemberUpdate_HG: postData } },
                colModel: [
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text', editable: false },
                    colLocked,
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: false, editonce: false , editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'mode               ', label: '<%=lang["colHGmode           "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'firstname          ', label: '<%=lang["colHGfirstname      "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'lastname           ', label: '<%=lang["colHGlastname       "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'agentid            ', label: '<%=lang["colHGagentid        "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'affiliateid        ', label: '<%=lang["colHGaffiliateid    "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'playerlevel        ', label: '<%=lang["colHGplayerlevel    "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'testusr            ', label: '<%=lang["colHGtestusr        "]%>', width: 080, sorttype: 'text', editable: false }
                ],
                onSelectCell: function (rowid, celname, value, iRow, iCol) {
                    if (celname=='Balance') {
                        //var $t = this;
                        //var opers = $t.p.prmNames;
                        //var idname = opers.id;
                        //var oper = opers.oper;
                        //var postdata={};
                        //postdata[idname] = $.jgrid.stripPref($t.p.idPrefix, $t.rows[iRow].id);
                        //postdata[oper] = opers.editoper;
                        //$.ajax({
                        //    url: $t.p.cellurl,
                        //    data :$.isFunction($t.p.serializeCellData) ? $t.p.serializeCellData.call($t, postdata) : postdata,
                        //    type: "POST",
                        //});
                        //console.log("onSelectCell", { a: this.p.UpdateCommand({}), b: this.p.cellurl });
                        //$(this).editCell(iRow, iCol, true);
                        //$(this).saveCell(iRow, iCol);
                        //$(this).setCell(rowid, celname, '<img src="../images/loading2_000000.gif" />');
                        //$(this).editRow(rowid);
                        //$(this).saveRow(rowid);
                    }
                    return true; 
                }
            });<% } break;
                  
                  
                    case GameID.EA: {
                        MemberGameRow_EA subrow = MemberGameRowCommand_EA.Instance.SelectRow(sqlcmd, memberID.Value, null); %>
            $subacc.EA = $(s_EA).jqGrid_init2({
                data: [<%=api.SerializeObject(subrow ?? Tools._null<object>.value)%>],
                UpdateCommand: function (postData) { postData.MemberID = rowdata[0].ID; return { MemberUpdate_EA: postData } },
                colModel: [
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text', editable: false },
                    colLocked,                  
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: false, editonce: false , editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text', editable: true },
                    { name: 'mode               ', label: '<%=lang["colGameMode         "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'agentid            ', label: '<%=lang["colAgentACNT        "]%>', width: 080, sorttype: 'text', editable: false },
                    { name: 'username           ', label: '<%=lang["colNickName         "]%>', width: 080, sorttype: 'text', editable: false },
                    
                ]
            });<% } break;
                  
                  
                    case GameID.WFT: {
                        MemberGameRow_WFT row_wft = MemberGameRowCommand_WFT.Instance.SelectRow(sqlcmd, memberID.Value, null); %>
            $subacc.WFT = $(s_WFT).jqGrid_init2({
                data: [<%=api.SerializeObject(row_wft ?? Tools._null<object>.value)%>],
                UpdateCommand: function (postData) { postData.MemberID = rowdata[0].ID; return { MemberUpdate_WFT: postData } },
                colModel: [
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    colLocked,                  
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'currencyid         ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: true, editonce: false , editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'gametype           ', label: '<%=lang["colGameType         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'username           ', label: '<%=lang["colNickName         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'gametype           ', label: '<%=lang["colGameType         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'language           ', label: '<%=lang["colLanguage         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'stake              ', label: '<%=lang["colStake            "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            });<% } break;
                  
                  
                    case GameID.WFT_SPORTS:
                        MemberGameRow_WFT row_wft2 = MemberGameRowCommand_WFT_SPORTS.Instance.SelectRow(sqlcmd, memberID.Value, null); %>
            $subacc.WFT = $(s_WFT).jqGrid_init2({
                data: [<%=api.SerializeObject(row_wft2 ?? Tools._null<object>.value)%>],
                UpdateCommand: function (postData) { postData.MemberID = rowdata[0].ID; return { MemberUpdate_WFT: postData } },
                colModel: [
                    { name: 'Action             ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT               ', label: '<%=lang["colACNT             "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password           ', label: '<%=lang["colPassword         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    colLocked,                  
                    { name: 'Balance            ', label: '<%=lang["colBalance          "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Currency           ', label: '<%=lang["colCurrency         "]%>', colType: 'Currency', editable: true, editonce: false , editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount      ', label: '<%=lang["colDeposit          "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount   ', label: '<%=lang["colWithdrawal       "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'mode               ', label: '<%=lang["colGameMode         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'username           ', label: '<%=lang["colNickName         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'gametype           ', label: '<%=lang["colGameType         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'language           ', label: '<%=lang["colLanguage         "]%>', width: 080, sorttype: 'text    ', editable: false },
                    { name: 'stake              ', label: '<%=lang["colStake            "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            });<% break;
                    case GameID.KENO:
                  MemberGameRow_KENO row_kg = MemberGameRowCommand_KENO.Instance.SelectRow(sqlcmd, memberID.Value, null);
                      // (MemberGameRow_KENO)MemberGameRowCommand.GetInstance(GameID.KENO).GetRow(sqlcmd, memberID.Value); %>
            $subacc.KENO = $(s_KG).jqGrid_init2({
                data: [<%=api.SerializeObject(row_kg ?? Tools._null<object>.value)%>],
                UpdateCommand: function (postData) { postData.MemberID = rowdata[0].ID; return { MemberUpdate_KG: postData } },
                colModel: [
                    { name: 'Action          ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT            ', label: '<%=lang["colACNT         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password        ', label: '<%=lang["colPassword     "]%>', width: 080, sorttype: 'text    ', editable: true },
                    colLocked,               
                    { name: 'Balance         ', label: '<%=lang["colBalance      "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Currency        ', label: '<%=lang["colCurrency    "]%>', colType: 'Currency', editable: true, editonce: false , editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount   ', label: '<%=lang["colDeposit      "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount', label: '<%=lang["colWithdrawal   "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            });<% break;
                    case GameID.SUNBET:
                  MemberGameRow_SUNBET row_sunbet = MemberGameRowCommand_SUNBET.Instance.SelectRow(sqlcmd, memberID.Value, null); %>
            $subacc.SUNBET = $(s_SUNBET).jqGrid_init2({
                data: [<%=api.SerializeObject(row_sunbet ?? Tools._null<object>.value)%>],
                UpdateCommand: function (postData) { postData.MemberID = rowdata[0].ID; return { MemberUpdate_SUNBET: postData } },
                colModel: [
                    { name: 'Action          ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT            ', label: '<%=lang["colACNT         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password        ', label: '<%=lang["colPassword     "]%>', width: 080, sorttype: 'text    ', editable: true },
                    colLocked,               
                    { name: 'Balance         ', label: '<%=lang["colBalance      "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Currency        ', label: '<%=lang["colCurrency    "]%>', colType: 'Currency', editable: true, editonce: false , editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount   ', label: '<%=lang["colDeposit      "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount', label: '<%=lang["colWithdrawal   "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            }); <% break;
                    case GameID.AG:
                   MemberGameRow_AG row_ag = MemberGameRowCommand_AG.Instance.SelectRow(sqlcmd, memberID.Value, null); %>
            $subacc.AG = $(s_AG).jqGrid_init2({
                data: [<%=api.SerializeObject(row_ag ?? Tools._null<object>.value)%>],
                UpdateCommand: function (postData) { postData.MemberID = rowdata[0].ID; return { MemberUpdate_AG : postData } },
                colModel: [
                    { name: 'Action          ', label: '<%=game.Name%>', colType: 'Buttons' },
                    { name: 'ACNT            ', label: '<%=lang["colACNT         "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Password        ', label: '<%=lang["colPassword     "]%>', width: 080, sorttype: 'text    ', editable: true },
                    colLocked,               
                    { name: 'Balance         ', label: '<%=lang["colBalance      "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'Currency        ', label: '<%=lang["colCurrency    "]%>', colType: 'Currency', editable: true, editonce: false , editoptions: { <%=enumlist<BU.CurrencyCode>("value")%> } },
                    { name: 'DepositAmount   ', label: '<%=lang["colDeposit      "]%>', width: 080, sorttype: 'text    ', editable: true },
                    { name: 'WithdrawalAmount', label: '<%=lang["colWithdrawal   "]%>', width: 080, sorttype: 'text    ', editable: true },
                ]
            }); <% break;
                }
            }
        } %>


            $('.item').each(function () { items.push(new item(this)); });

            $('.ui-jqgrid').removeClass('ui-corner-all');//.css({ 'border-top-width': 0, 'border-bottom-width': 1, 'border-right-width': 0 });
            $('.ui-th-column').removeClass('ui-state-default').addClass('ui-widget-content');

            $('.button').each(function () {
                $(this).button({ icons: { primary: $(this).attr('icon') } })/*.removeClass('ui-state-default')*/.click(function () {
                    var action = $(this).attr('action');
                    var td = $(this).closest('td[role="gridcell"]');
                    if (td.length != 1) return;
                    var $t = $(this).closest('div.ui-jqgrid').find('table.ui-jqgrid-btable');
                    if ($t.length != 1) return;
                    var t = $t[0];
                    if (!t.grid) return;
                    for (var i = 0; i < t.p.colModel.length; i++) {
                        if ($("tr.footrow td:eq(" + i + ")", t.grid.sDiv)[0] == td[0]) {
                            switch (action) {
                                case 'editCell': $t.editCell(1, i, true); break;
                                case 'saveCell': $t.saveCell(1, i); break;
                                case 'restoreCell': $t.restoreCell(1, i); break;
                            }
                        }
                    }
                    //console.log({ "td": td, "table.grid": t.grid, "colModel": t.p.colModel, xx: $("tr.footrow td:eq("+0+")",t.grid.sDiv) });
                });
            });

            iframe_auto_height();
        });

        function iframe_auto_height() {
            $(window.frameElement).css('height', document.body.offsetHeight);
        }
    </script>
    <style type="text/css">
        #nav, #content {
            vertical-align: top;
        }

        #nav {
            width: 100px;
            padding-left: 1px;
            padding-right: 1px;
        }

        .nav {
            width: 100%;
        }

        .content {
        }

        .ui-jqgrid tr.footrow td {
            border-top-width: 0;
        }
        /*.nav0, .nav1 , .nav2 { width:100%; text-align:left; }
        .fill_x { width: 90%; }
        .op_form {
            border-width:0;
            border-right-width: 0;
            border-bottom-width: 0;
        }
        .border-left {
            border-left-width: 1px;
            border-left-color: inherit;
            border-left-style: solid;
        }
        .op_form td,
        .border-right {
            border-right-width: 1px;
            border-right-color: inherit;
            border-right-style: solid;
        }
        .border-top {
            border-top-width: 1px;
            border-top-color: inherit;
            border-top-style: solid;
        }
        .op_form td,
        .border-bottom {
            border-bottom-width: 1px;
            border-bottom-color: inherit;
            border-bottom-style: solid;
        }*/
        /*table.subacc .button .ui-state-disabled:hover,
        table.subacc .button .ui-button-text { padding-right: .1em; }*/
        /*table.subacc .title { text-align: center; }*/
        /*table.subacc .buttons { text-align: center; }*/
        .hide {
            display: none;
        }

        .nav,
        .ui-jqgrid tr.jqgrow td {
            white-space: nowrap;
        }

        .button.editshow {
            display: none;
        }

        .button.edithide {
            display: inline-block;
        }

        td.celledit .button.editshow {
            display: inline-block;
        }

        td.celledit .button.edithide {
            display: none;
        }

        .ui-button-text-icon-primary .ui-button-icon-primary, .ui-button-text-icons .ui-button-icon-primary, .ui-button-icons-only .ui-button-icon-primary {
            left: 0;
        }

        .ui-button-text-icon-primary .ui-button-text, .ui-button-text-icons .ui-button-text {
            padding-left: 1.3em;
            padding-right: .5em;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <table class="ui-jqgrid" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="ui-jqgrid-view" id="nav"></td>
            <td class="ui-jqgrid-view" id="content"></td>
        </tr>
    </table>
    <div class="item ui-state-active">
        <div class="nav" icon="ui-icon-comment">詳細資料</div>
        <table class="content" cellpadding="0" cellspacing="0">
            <tr class="hide">
                <td>
                    <table id="info0"></table>
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table id="info1"></table>
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table id="info2"></table>
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table id="info3"></table>
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table id="bankcard">
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
                </td>
            </tr>
        </table>
    </div>
    <div class="item">
        <div class="nav" icon="ui-icon-comment">子帳戶</div>
        <table class="content" cellpadding="0" cellspacing="0">
            <tr class="hide">
                <td>
                    <table class="subacc" id="s_HG">
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
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table class="subacc" id="s_EA">
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
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table class="subacc" id="s_WFT">
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
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table class="subacc" id="s_KG">
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
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table class="subacc" id="s_SUNBET">
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
                </td>
            </tr>
            <tr class="hide">
                <td>
                    <table class="subacc" id="s_AG">
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
                </td>
            </tr>
        </table>
    </div>
    <div class="item">
        <div class="nav ui-corner-bottom" icon="ui-icon-comment">金流</div>
        <div id="tmp1" class="hide">
            <label class="button edithide" icon="ui-icon-pencil" action="editCell">編輯</label>
            <label class="button editshow" icon="ui-icon-disk" action="saveCell">儲存</label>
            <label class="button editshow" icon="ui-icon-cancel" action="restoreCell">取消</label>
        </div>
        <div id="tmp2" class="hide">
            <label class="button edithide" icon="ui-icon-transfer-e-w" action="editCell">轉帳</label>
            <label class="button editshow" icon="ui-icon-disk" action="saveCell">確定</label>
            <label class="button editshow" icon="ui-icon-cancel" action="restoreCell">取消</label>
        </div>
    </div>
</asp:Content>
