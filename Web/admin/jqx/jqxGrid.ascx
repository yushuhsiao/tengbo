<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="jqxGrid.ascx.cs" Inherits="web.jqxGrid" %>
<%@ Import Namespace="System.ComponentModel" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<script runat="server">

    [DefaultValue(false)]
    public bool rtl { get; set; }

    [DefaultValue(null)]
    public string toolbar { get; set; }

    [DefaultValue(null)]
    public string theme { get; set; }

    [DefaultValue(false)]
    public bool altrows { get; set; }

    [DefaultValue(1)]
    public int altstart { get; set; }

    [DefaultValue(1)]
    public int altstep { get; set; }

    [DefaultValue(true)]
    public bool clipboard { get; set; }
    
    [DefaultValue(true)]
    public bool enablekeyboarddelete { get; set; }

</script>

<div id="<%=this.ID%>"></div>
<script type="text/javascript">
    $.replace_function($.jqx._jqxGrid.prototype, 'defineInstance', function () {
        $.extend(true, this, arguments.callee._original.apply(this, arguments), {
            //disabled: false,
            //width: 600,
            //height: 400,
            //groupsheaderheight: 34,
            //rowsheight: 25,
            //columnsheight: 25,
            //filterrowheight: 31,
            //groupindentwidth: 30,
            //rowdetails: false,
            //enablerowdetailsindent: true,
            //enablemousewheel: true,
            //initrowdetails: null,
            //layoutrowdetails: null,
            editable: true, //false,
            editmode: ['click', 'selectedcell', 'selectedrow', 'dblclick', 'programmatic'][4],//"selectedcell",
            pageable: true, //false,
            pagermode: ['simple', 'default'][0],
            pagesize: 50, //10,
            pagesizeoptions: [10, 20, 30, 50, 100, 500],//["5", "10", "20"],
            //pagerbuttonscount: 5,
            //pagerheight: 28,
            pagerrenderer: null,
            //groupable: false,
            sortable: true, // false
            filterable: true, //false,
            //filtermode: ['default', 'excel'][0],
            //showfilterrow: false,
            //autoshowfiltericon: true,
            //showfiltercolumnbackground: true,
            //showpinnedcolumnbackground: false, //true,
            //showsortcolumnbackground: true,
            //showrowdetailscolumn: true,
            //showtoolbar: false,
            //toolbarheight: 34,
            //showstatusbar: false,
            //statusbarheight: 34,
            //enableellipsis: true,
            //groups: [],
            //groupsrenderer: null,
            //groupcolumnrenderer: null,
            //groupsexpandedbydefault: false,
            //touchmode: "auto",
            //selectedrowindex: -1,
            //selectedrowindexes: new Array(),
            //selectedcells: new Array(),
            //autobind: true,
            //selectedcell: null,
            //tableZIndex: 799,
            //headerZIndex: 199,
            //updatefilterconditions: null,
            //showaggregates: false,
            //autorowheight: false,
            //autokoupdates: true,
            //handlekeyboardnavigation: null,
            //showsortmenuitems: true,
            //showfiltermenuitems: false,
            //showgroupmenuitems: true,
            //enablebrowserselection: false,
            //ready: null,
            //updatefilterpanel: null,
            //autogeneratecolumns: false,
            //rowdetailstemplate: null,
            //scrollfeedback: null,
            //rendertoolbar: null,
            //renderstatusbar: null,
            //rendered: null,
            //multipleselectionbegins: null,
            //columngroups: null,
            //cellhover: null,
            //source: {
            //    beforeprocessing: null,
            //    beforesend: null,
            //    loaderror: null,
            //    localdata: null,
            //    data: null,
            //    datatype: "array",
            //    datafields: [],
            //    url: "",
            //    root: "",
            //    record: "",
            //    id: "",
            //    totalrecords: 0,
            //    recordstartindex: 0,
            //    recordendindex: 0,
            //    loadallrecords: true,
            //    sortcolumn: null,
            //    sortdirection: null,
            //    sort: null,
            //    filter: null,
            //    sortcomparer: null
            //},
            //dataview: null,
            //updatedelay: null,
            //autoheight: false,
            //autowidth: false,
            //showheader: true,
            //showgroupsheader: true,
            //closeablegroups: true,
            //scrollbarsize: $.jqx.utilities.scrollBarSize,
            //touchscrollbarsize: $.jqx.utilities.touchScrollBarSize,
            virtualmode: true, //false,
            rendergridrows: function (params) {
                return params.data;
            },
            //sort: null,
            //columnsmenu: true,
            columnsresize: true, //false,
            //columnsreorder: false,
            //columnsmenuwidth: 15,
            //autoshowcolumnsmenubutton: true,
            //popupwidth: "auto",
            //sorttogglestates: 2,
            //enableanimations: true,
            enabletooltips: true, //false,
            selectionmode: ['none', 'singlerow', 'multiplerows', 'multiplerowsextended', 'singlecell', 'multiplecells', 'multiplecellsextended', 'multiplecellsadvanced', 'checkbox'][7],
            enablehover: true,
            //loadingerrormessage: "The data is still loading. When the data binding is completed, the Grid raises the 'bindingcomplete' event. Call this function in the 'bindingcomplete' event handler.",
            //verticalscrollbarstep: 25,
            //verticalscrollbarlargestep: 400,
            //horizontalscrollbarstep: 10,
            //horizontalscrollbarlargestep: 50,
            //keyboardnavigation: true,
            //touchModeStyle: "auto",
            //autoshowloadelement: true,
            //showdefaultloadelement: true,
            //showemptyrow: true,
            //autosavestate: false,
            //autoloadstate: false,
            //_updating: false,
            //_pagescache: new Array(),
            //_pageviews: new Array(),
            //_cellscache: new Array(),
            //_rowdetailscache: new Array(),
            //_rowdetailselementscache: new Array(),
            //_requiresupdate: false,
            //_hasOpenedMenu: false,
            //scrollmode: "physical",
            //deferreddatafields: null,
            //localization: null,
            //menuitemsarray: [],
            //events: ["initialized", "rowClick", "rowSelect", "rowUnselect", "groupExpand", "groupCollapse", "sort", "columnClick", "cellClick", "pageChanged", "pageSizeChanged", "bindingComplete", "groupsChanged", "filter", "columnResized", "cellSelect", "cellUnselect", "cellBeginEdit", "cellEndEdit", "cellValueChanged", "rowExpand", "rowCollapse", "rowDoubleClick", "cellDoubleClick", "columnReordered", "pageChanging"],
            extra: {
                source: {}, settings: {}, events: {},
            },
            pkey: [],
        });
    });

    $(document).ready(function () {
        //<%=true%>
        $('#<%=ID%>').jqxGrid({
            <% if (prop("rtl")) { %>rtl: <%=rtl.ToString().ToLower()%>, <% } %>
            <% if (prop("altrows")) { %>altrows: true, <% } %>
            <% if (prop("altstart")) { %>altstart: <%=altstart%>, <% } %>
            <% if (prop("altstep")) { %>altstep: <%=altstep%>, <% } %>
            <% if (prop("clipboard")) { %>clipboard: false, <% } %>
            <% if (prop("enablekeyboarddelete")) { %>enablekeyboarddelete: enablekeyboarddelete, <% } %>
            pkey: 'ID',
            rowdetails: true,
            initrowdetails: function () { },
            rowdetailstemplate: {
                rowdetails: "<div id='grid' style='background-color: red;'></div>",
                rowdetailsheight: 50,
                rowdetailshidden: true
            },
            width: window.innerWidth - 2, height: window.innerHeight - 2,
            <% if (!string.IsNullOrEmpty(this.toolbar)) { %>
            showtoolbar: true, toolbarheight: $('<%=toolbar%>').outerHeight(), rendertoolbar: function (toolbardiv) { $('<%=toolbar%>').appendTo(toolbardiv); },
            <% } %>
            columns: [
                $.col('Number   ', {}),
                $.col('Action1  ', { buttons: { remove: false, text: ['<%=lang["Actions_Edit"]%>', '<%=lang["Actions_Cancel"]%>', '<%=lang["Actions_Save"]%>'] } }),
                    $.col('Action2  ', {}),
                    $.col('ID       ', {}),
                    $.col('CorpID   ', { datafield: 'CorpID       ', text: '<%=lang["colCorpID       "]%>', }),
                    $.col('         ', { datafield: 'ParentACNT   ', text: '<%=lang["colParentACNT   "]%>', }),
                    $.col('         ', { datafield: 'ACNT         ', text: '<%=lang["colACNT         "]%>', width: 120 }),
                    $.col('         ', { datafield: 'Name         ', text: '<%=lang["colName         "]%>', }),
                    $.col('GroupID  ', { datafield: 'GroupID      ', text: '<%=lang["colGroupID      "]%>', }),
                    $.col('Locked   ', { datafield: 'Locked       ', text: '<%=lang["colLocked       "]%>', }),
                    $.col('Balance  ', { datafield: 'Balance      ', text: '<%=lang["colBalance      "]%>', }),
                    $.col('         ', { datafield: 'Currency     ', text: '<%=lang["colCurrency     "]%>', }),
                    $.col('         ', { datafield: 'Memo         ', text: '<%=lang["colMemo         "]%>', }),
                    $.col('DateTime ', { datafield: 'CreateTime   ', text: '<%=lang["colCreateTime   "]%>', editable: false }),
                    $.col('         ', { datafield: 'RegisterIP   ', text: '<%=lang["colRegisterIP   "]%>', editable: false }),
                    $.col('DateTime ', { datafield: 'LoginTime    ', text: '<%=lang["colLoginTime    "]%>', editable: false }),
                    $.col('         ', { datafield: 'LoginIP      ', text: '<%=lang["colLoginIP      "]%>', editable: false }),
                    $.col('         ', { datafield: 'LoginCount   ', text: '<%=lang["colLoginCount   "]%>', editable: false }),
                    $.col('         ', { datafield: 'CreateUser   ', text: '<%=lang["colCreateUser   "]%>', editable: false }),
                    $.col('DateTime ', { datafield: 'ModifyTime   ', text: '<%=lang["colModifyTime   "]%>', editable: false }),
                    $.col('         ', { datafield: 'ModifyUser   ', text: '<%=lang["colModifyUser   "]%>', editable: false }),
                    $.col('DateTime ', { datafield: 'Birthday     ', text: '<%=lang["colBirthday     "]%>', }),
                    $.col('         ', { datafield: 'Tel          ', text: '<%=lang["colTel          "]%>', }),
                    $.col('         ', { datafield: 'Mail         ', text: '<%=lang["colMail         "]%>', }),
                    $.col('         ', { datafield: 'QQ           ', text: '<%=lang["colQQ           "]%>', }),
                    $.col('         ', { datafield: 'Introducer   ', text: '<%=lang["colIntroducer   "]%>', }),
                    $.col('         ', { datafield: 'Sex          ', text: '<%=lang["colSex          "]%>', }),
                    $.col('         ', { datafield: 'Addr         ', text: '<%=lang["colAddr         "]%>', }),
                    $.col('         ', { datafield: 'UserMemo     ', text: '<%=lang["colUserMemo     "]%>', }),
            ],
            extra: {
                source: {
                    addrow: function (rowid, rowdata, position, commit) {
                        //console.log('addrow', arguments);
                        commit(true);
                        //$grid.grid.action_panel[rowid].EditRow();
                    },
                    deleterow: function (rowid, commit) {
                        commit(true);
                        //console.log('deleterow', arguments);
                    },
                    updaterow: function (rowid, newdata, commit) {
                        //console.log('updaterow', arguments);
                        commit(true);
                    },
                    filter: function (filters, recordsArray) {
                        //console.log('filter', arguments);
                    }
                },
                settings: {
                    SelectCommand: '<%=typeof(web.MemberSelect2).Name%>',
                },
                events: {
                    pagechanged: function (event) {
                        //console.log('pagechanged', arguments);
                        //$grid.grid.updatebounddata();
                    },
                    filter: function (event) {
                        //console.log('filter', arguments);
                        $grid.grid.updatebounddata();
                    },
                    sort: function (event) {
                        //console.log('sort', arguments);
                        $grid.grid.updatebounddata();
                    },
                }
            }
        });
    });
</script>