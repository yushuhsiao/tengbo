
var source = {
    name: '',
    type: '', //'string', 'date', 'number', 'int', 'float', 'bool'
    map: '',
    format: '',
    values: null,
}
var col = {
	datafield: null,
	displayfield: null,
	text: '',
	sortable: true,
	hideable: true,
	hidden: false,
	groupable: true,
	columngroup: null,
	menu: true,
	exportable: true,
	resizable: true,
	width: 'auto',
	minwidth: 25,
	maxwidth: 'auto',
	pinned: false,
	enabletooltips: true,
	nullable: true,
	columntype: null,   // number, checkbox, numberinput, dropdownlist, combobox, datetimeinput, textbox, template, custom 
	classname: '',
	cellclassname: '',
	renderer: function (text, align, height) { },
	cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) { },
	rendered: function (columnHeaderElement) { },
	cellsformat: '',
	align: 'left',      // 'left', 'center', 'right'
	cellsalign: 'left', // 'left', 'center', 'right'
	editable: true,
	initeditor: function (row, cellvalue, editor, celltext, pressedChar) { },
	createeditor: function (row, cellvalue, editor, celltext, cellwidth, cellheight) { },
	destroyeditor: null,
	geteditorvalue: function (row, cellvalue, editor) { return null; },
	validation: function (cell, value) { return { result: false, message: '' }; },
	cellbeginedit: function (row, datafield, columntype) { return true; },
	cellendedit: function (row, datafield, columntype, oldvalue, newvalue) { return true; },
	cellvaluechanging: function (row, datafield, columntype, oldvalue, newvalue) { return null; },
	aggregates: ['avg', 'count', 'min', 'max', 'sum', 'product', 'stdev', 'stdevp', 'varp', 'var', function (aggregatedValue, currentValue, column, record) { return null; }],
	aggregatesrenderer: function (aggregates) { },
	createfilterwidget: function (column, columnElement, widget) { },
	filterable: true,
	filter: null,   // $.jqx.filter
	filteritems: [],
	filterdelay: 800,
	filtertype: null,   // textbox, input, checkedlist, list, number, checkbox, date, range
	filtercondition: null,  // filtertype = textbox : 'EMPTY', 'NOT_EMPTY', 'CONTAINS', 'CONTAINS_CASE_SENSITIVE','DOES_NOT_CONTAIN', 'DOES_NOT_CONTAIN_CASE_SENSITIVE', 'STARTS_WITH', 'STARTS_WITH_CASE_SENSITIVE', 'ENDS_WITH', 'ENDS_WITH_CASE_SENSITIVE', 'EQUAL', 'EQUAL_CASE_SENSITIVE', 'NULL', 'NOT_NULL'
							// filtertype = number  : 'EQUAL', 'NOT_EQUAL', 'LESS_THAN', 'LESS_THAN_OR_EQUAL', 'GREATER_THAN', 'GREATER_THAN_OR_EQUAL', 'NULL', 'NOT_NULL'
}

/*

Possible Number strings: 
"d" - decimal numbers. 
"f" - floating-point numbers. 
"n" - integer numbers. 
"c" - currency numbers. 
"p" - percentage numbers. 

For adding decimal places to the numbers, add a number after the formatting string. 
For example: "c3" displays a number in this format $25.256 
Possible built-in Date formats: 

// short date pattern d: "M/d/yyyy", 
// long date pattern D: "dddd, MMMM dd, yyyy", 
// short time pattern t: "h:mm tt", 
// long time pattern T: "h:mm:ss tt", 
// long date, short time pattern f: "dddd, MMMM dd, yyyy h:mm tt", 
// long date, long time pattern F: "dddd, MMMM dd, yyyy h:mm:ss tt", 
// month/day pattern M: "MMMM dd", 
// month/year pattern Y: "yyyy MMMM", 
// S is a sortable format that does not vary by culture S: "yyyy\u0027-\u0027MM\u0027-\u0027dd\u0027T\u0027HH\u0027:\u0027mm\u0027:\u0027ss" 

Possible Date format strings: 

"d"-the day of the month;
"dd"-the day of the month; 
"ddd"-the abbreviated name of the day of the week;
"dddd"- the full name of the day of the week;
"h"-the hour, using a 12-hour clock from 1 to 12; 
"hh"-the hour, using a 12-hour clock from 01 to 12; 
"H"-the hour, using a 24-hour clock from 0 to 23;
"HH"- the hour, using a 24-hour clock from 00 to 23; 
"m"-the minute, from 0 through 59;
"mm"-the minutes,from 00 though59;
"M"- the month, from 1 through 12;
"MM"- the month, from 01 through 12;
"MMM"-the abbreviated name of the month;
"MMMM"-the full name of the month;
"s"-the second, from 0 through 59; 
"ss"-the second, from 00 through 59; 
"t"- the first character of the AM/PM designator;
"tt"-the AM/PM designator; 
"y"- the year, from 0 to 99; 
"yy"- the year, from 00 to 99; 
"yyy"-the year, with a minimum of three digits; 
"yyyy"-the year as a four-digit number; 
"yyyyy"-the year as a four-digit number. 
/*


$.columns.Action2 = function (n, opts) {
    return $.columns($.extend(true, {
        datafield: '_act' + n, text: ' ', width: 60,
        cellsalign: 'center',
        pinned: true, sortable: false, hideable: false, groupable: false, exportable: false, resizable: false,
        enabletooltips: false, editable: false, filterable: false, menu: false,
        cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
            //console.log(this.action);
            console.log(this, arguments);
            return 'Edit';
        },
        columntype: 'button', buttonclick: function (row, event) {
            if (this.owner.actions)
                if ($.isFunction(this.owner.actions.buttonclick))
                    this.owner.actions.buttonclick.call(this, n, row, event);
        },
    }, opts));
}

$.columns.Action3 = function (opts) {
    return $.columns($.extend({
        width: 100, datafield: '_Actions', text: ' ',
        button_text: { edit: { text: 'Edit', confirm: 'Ok', cancel: 'Cancel' }, remove: { Text: 'Remove', confirm: 'Ok', cancel: 'Cancel' }, },
        cellsalign: 'center',
        pinned: true, sortable: false, hideable: false, groupable: false, exportable: false, resizable: false,
        enabletooltips: false, editable: false, filterable: false, menu: false,
        //cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) { return ''; },
        cellsrendered: function (datagrid, column, bound, cellvalue, cellelement, q) {
            var $dom = bound.$dom;
            var $div = $(cellelement);
            function click() {
                var act1 = $(this).attr('act1'),
                    act2 = $(this).attr('act2');
                var f = datagrid.actions[act1];
                if (f) f = f[act2];
                if ($.isFunction(f)) {
                    $dom.removeClass('grid-action-edit grid-action-remove');
                    if (act2 == 'active') {
                        $dom.addClass('grid-action-' + act1);
                    }
                    f.apply(datagrid, arguments);
                }
            }
            //$div.empty();
            if ($dom == null) {
                console.log(bound);
                $dom = bound.$dom = $('<div class="grid-action"></div>').appendTo($div);
                $('<button act1="edit"   act2="active" ></button>').appendTo($dom).jqxButton().on('click', click).text('Edit');
                $('<button act1="edit"   act2="confirm"></button>').appendTo($dom).jqxButton().on('click', click).text('e.Ok');
                $('<button act1="edit"   act2="cancel" ></button>').appendTo($dom).jqxButton().on('click', click).text('e.Cancel');
                $('<button act1="remove" act2="active" ></button>').appendTo($dom).jqxButton().on('click', click).text('Delete');
                $('<button act1="remove" act2="confirm"></button>').appendTo($dom).jqxButton().on('click', click).text('r.Ok');
                $('<button act1="remove" act2="cancel" ></button>').appendTo($dom).jqxButton().on('click', click).text('r.Cancel');
                //function () {
                //  bound.bounddata._editing = true;
                //datagrid.editingrows.push(bound.bounddata);
                //console.log(bound.bounddata);
                //console.log([datagrid, column, bound, cellvalue, cellelement, q]);
                //datagrid.beginrowedit(bound.boundindex);
                //$grid.jqxGrid('beginrowedit', row);
                //})
            }
            else {
                $dom.appendTo($div);
            }
            //console.log('cellsrendered', arguments);
        }
    }, opts));
}


//datagrid.actions = {
//    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
//    },
//    buttonclick: function (n, row, event) {
//        console.log(this, arguments);
//    },
//}

//datagrid.action = {
//    cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) {
//    }
//}

//datagrid.actions = {
//    //cellbeginedit: function (row, datafield, columntype, value) {
//    //    var xxx = datagrid.getcell(row, datafield);
//    //    var column = this.owner.getcolumn(datafield);
//    //    var rowdata = this.owner.getrowdata(row);
//    //    //console.log(xxx);
//    //    console.log(column, rowdata);
//    //    console.log('cellbeginedit', arguments);
//    //    return true;
//    //},
//    //cellendedit: function (row, datafield, columntype, oldvalue, newvalue) {
//    //    console.log('cellendedit', arguments);
//    //    return true;
//    //},
//    edit: {
//        active: function () {
//            console.log('edit active', arguments);
//        },
//        confirm: function () {
//            console.log('edit confirm', arguments);
//        },
//        cancel: function () {
//            console.log('edit cancel', arguments);
//        }
//    },
//    remove: {
//        active: function () {
//            console.log('remove active', arguments);
//        },
//        confirm: function () {
//            console.log('remove confirm', arguments);
//        },
//        cancel: function () {
//            console.log('remove cancel', arguments);
//        }
//    }
//};


//var _raiseEvent = datagrid._raiseEvent;
//datagrid._raiseEvent = function () {
//    console.log('raiseevent', arguments);
//    return _raiseEvent.apply(this, arguments);
//}



//var _rendercell = datagrid._rendercell;
//datagrid._rendercell = function (datagrid, column, bound, cellvalue, cellelement, q) {
//    var ret = _rendercell.apply(this, arguments);
//    for (var i = 0; i < opts.options.columns.length; i++) {
//        if (opts.options.columns[i].datafield == column.datafield) {
//            var cb = opts.options.columns[i].cellsrendered
//            if ($.isFunction(cb))
//                cb.apply(this, arguments);
//        }
//    }
//    return ret;
//}



$.columns = function (opts) {
    if (opts.datafield)
        opts.datafield = $.trim(opts.datafield);
    if (opts.text && (opts.text != ' '))
        opts.text = $.trim(opts.text);
    opts = $.extend({
        // cellbeginedit: function (row, datafield, columntype, value) {/*return this.owner.actions.cellbeginedit.apply(this, arguments);*/return true; }, cellendedit: function (row, datafield, columntype, oldvalue, newvalue) {/*return this.owner.actions.cellendedit.apply(this, arguments);*/return true; },
        CreateEditor: function (row, editor, column, bound, cellvalue, cellelement) {
            var elem = $('<input autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" type="textbox" style="width: 99%; height: 100%;"/>');
            elem.jqxInput();
            return elem;
        },
        ShowEditor: function (row, editor, elem, column, bound, cellvalue, cellelement) {
            if (editor.isEditing) {
                $(cellelement).empty();
                elem.appendTo(cellelement).val(cellvalue);
            }
        },
        //cellsrendered: function (row, editor, datagrid, column, bound, cellvalue, cellelement, q) {
        //    if (editor.isEditing) {
        //        if (column.editable != true) return;
        //        $(cellelement).empty();
        //        console.log('cellsrendered', arguments);
        //        var elem = editor.elem[column.datafield];
        //        if (elem == null) {
        //            elem = editor.elem[column.datafield] = $('<input autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" type="textbox" style="width: 99%; height: 100%;"/>');
        //            elem.jqxInput();
        //        }
        //        elem.appendTo(cellelement).val(cellvalue);
        //        //$(cellelement).
        //    }
        //},
    }, opts);
    return opts;
}

$.columns.Action = function (opts) {
    opts = $.columns($.extend(true, {
        width: 100, datafield: '_Actions', text: ' ', cellsalign: 'center',
        editable: true, columntype: 'custom',
        buttons: {
            edit: { visible: true, active: 'Edit', confirm: 'Ok.1', cancel: 'Cancel.1' },
            remove: { visible: true, active: 'Remove', confirm: 'Ok.2', cancel: 'Cancel.2' },
        },
        pinned: true, sortable: false, hideable: false, groupable: false, exportable: false, resizable: false, enabletooltips: false, filterable: false, menu: false,
        cellsrenderer: function (row, columnfield, value, defaulthtml, columnproperties) { return ''; },
        CreateEditor: function (row, editor, column, bound, cellvalue, cellelement) {
            var _this = this;
            var elem = {};
            elem.all = $(
                    '<button class="grid-actions">' + opts.buttons.edit.active + '</button>' +
                    '<button class="grid-actions">' + opts.buttons.edit.cancel + '</button>' +
                    '<button class="grid-actions">' + opts.buttons.edit.confirm + '</button>' +
                    '<button class="grid-actions">' + opts.buttons.remove.active + '</button>' +
                    '<button class="grid-actions">' + opts.buttons.remove.cancel + '</button>' +
                    '<button class="grid-actions">' + opts.buttons.remove.confirm + '</button>');
            (elem.e1 = $(elem.all[0])).jqxButton().on('click', function () { _this.EditRow(row); });
            (elem.e2 = $(elem.all[1])).jqxButton().on('click', function () { _this.EditRow(row, false); });
            (elem.e3 = $(elem.all[2])).jqxButton().on('click', function () { _this.EditRow(row, true); });
            (elem.r1 = $(elem.all[3])).jqxButton().on('click', function () { _this.RemoveRow(row); });
            (elem.r2 = $(elem.all[4])).jqxButton().on('click', function () { _this.RemoveRow(row, false); });
            (elem.r3 = $(elem.all[5])).jqxButton().on('click', function () { _this.RemoveRow(row, true); });
            return elem;
        },
        ShowEditor: function (row, editor, elem, column, bound, cellvalue, cellelement) {
            $(cellelement).empty();
            if (editor.isEditing) {
                elem.e2.appendTo(cellelement);
                elem.e3.appendTo(cellelement);
            }
            else if (editor.isRemoving) {
                elem.r2.appendTo(cellelement);
                elem.r3.appendTo(cellelement);
            }
            else {
                if (opts.buttons.edit.visible) elem.e1.appendTo(cellelement);
                if (opts.buttons.remove.visible) elem.r1.appendTo(cellelement);
            }
        },
        //createeditor: function (row, cellvalue, editor, celltext, cellwidth, cellheight) { },
        //initeditor: function (row, cellvalue, editor, celltext, pressedChar) { },
        //destroyeditor: function () { },
        //cellsrendered: function (row, editor, datagrid, column, bound, cellvalue, cellelement, q) {
        //    var elem = editor.elem[column.datafield]
        //    if (elem == null) {
        //        elem = editor.elem[column.datafield] = {};
        //        elem.all = $(
        //            '<button class="grid-actions">' + opts.buttons.edit.active + '</button>' +
        //            '<button class="grid-actions">' + opts.buttons.edit.cancel + '</button>' +
        //            '<button class="grid-actions">' + opts.buttons.edit.confirm + '</button>' +
        //            '<button class="grid-actions">' + opts.buttons.remove.active + '</button>' +
        //            '<button class="grid-actions">' + opts.buttons.remove.cancel + '</button>' +
        //            '<button class="grid-actions">' + opts.buttons.remove.confirm + '</button>');
        //        (elem.e1 = $(elem.all[0])).jqxButton().on('click', function () { datagrid.EditRow(row); });
        //        (elem.e2 = $(elem.all[1])).jqxButton().on('click', function () { datagrid.EditRow(row, false); });
        //        (elem.e3 = $(elem.all[2])).jqxButton().on('click', function () { datagrid.EditRow(row, true); });
        //        (elem.r1 = $(elem.all[3])).jqxButton().on('click', function () { datagrid.RemoveRow(row); });
        //        (elem.r2 = $(elem.all[4])).jqxButton().on('click', function () { datagrid.RemoveRow(row, false); });
        //        (elem.r3 = $(elem.all[5])).jqxButton().on('click', function () { datagrid.RemoveRow(row, true); });
        //    }
        //    if (editor.isEditing) {
        //        elem.e2.appendTo(cellelement);
        //        elem.e3.appendTo(cellelement);
        //    }
        //    else if (editor.isRemoving) {
        //        elem.r2.appendTo(cellelement);
        //        elem.r3.appendTo(cellelement);
        //    }
        //    else {
        //        if (opts.buttons.edit.visible) elem.e1.appendTo(cellelement);
        //        if (opts.buttons.remove.visible) elem.r1.appendTo(cellelement);
        //    }
        //    //if (datagrid.action_panel[row] == null)
        //    //    datagrid.action_panel[row] = new $.columns.Action.panel(row, datagrid, opts);
        //    //return datagrid.action_panel[row].render.call(datagrid, column, bound, cellvalue, cellelement, q);
        //}
    }, opts));
    return opts;
}



$.fn.jqxGrid2 = function (o) {
    var $grid = this;
    var datagrid = null;
    var opts = o;

    var source = $.extend(true, {
        //id: 'ID',
        //root: "rows",
        //record: "",
        url: 'api',
        datatype: "json",
        type: 'post',
        datafields: [],
        mapChar: '.',
        //localdata: [],
        //data: { a: 1, b: 2 },
        loadallrecords: false,
    }, o.source);
    var settings = $.extend(true, {
        //async: true,
        //autoBind: false,
        //contentType: "application/x-www-form-urlencoded",
        //processData: function () { },
        //beforeSend: function () { },
        //loadError: function (xhr, status, error) { },
        //beforeLoadComplete: function () { },
        downloadComplete: function (data, status, xhr) {
            //console.log(arguments);
        },
        //loadComplete: function (data) { },
        //loadServerData: function () { },
    }, o.settings);
    if (o.settings.SelectCommand) {
        delete settings.SelectCommand;
        settings.formatData = function (data) {
            if (datagrid) {
                if (datagrid.dataview.filters.length > 0) {
                    data._filter = new Array();
                    for (var i = 0; i < datagrid.dataview.filters.length; i++) {
                        var f1 = datagrid.dataview.filters[i];
                        var f2 = f1.filter.getfilters();
                        if (f2.length > 0) {
                            var o = {
                                operator: f1.operator,
                                datafield: f1.datafield,
                                filters: f2,
                            }
                            data._filter.push(o);
                        }
                    }
                }
                data._sort = datagrid.getsortinformation();
                //data._pager = datagrid.getpaginginformation();
            }
            var command = {
            };
            command[opts.settings.SelectCommand] = data;
            return {
                str: JSON.stringify(command)
            };
        }
    }

    var options = $.extend(true, {
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
        showfiltercolumnbackground: true,
        showpinnedcolumnbackground: false, //true,
        //showsortcolumnbackground: true,
        //altrows: false,
        //altstart: 1,
        //altstep: 1,
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
        //showfiltermenuitems: true,
        //showgroupmenuitems: true,
        //enablebrowserselection: false,
        //enablekeyboarddelete: true,
        //clipboard: true,
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
        rendergridrows: function (params) { return params.data; },
        //sort: null,
        //columnsmenu: true,
        //columnsresize: false,
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
        //rtl: false,
        //menuitemsarray: [],
        //events: ["initialized", "rowClick", "rowSelect", "rowUnselect", "groupExpand", "groupCollapse", "sort", "columnClick", "cellClick", "pageChanged", "pageSizeChanged", "bindingComplete", "groupsChanged", "filter", "columnResized", "cellSelect", "cellUnselect", "cellBeginEdit", "cellEndEdit", "cellValueChanged", "rowExpand", "rowCollapse", "rowDoubleClick", "cellDoubleClick", "columnReordered", "pageChanging"],
    }, o.options);

    for (var i = 0; i < options.columns.length; i++) {
        var col = options.columns[i];
        if (col.cellsrendered) {
            delete col.cellsrendered;
        }

        if (col.source) {
            if (col.source.type != 'none') {
                col.source.name = col.datafield;
                source.datafields.push(col.source);
            }
            delete col.source;
        }
    }
    for (var i = 0; i < source.datafields.length; i++) {
        var col = source.datafields[i];
        col['name'] = $.trim(col['name']);
        col['type'] = $.trim(col['type']);
    }

    options.source = new $.jqx.dataAdapter(source, settings);

    // console.log(o);
    // console.log(this[0].attributes);

    this.jqxGrid(options); $grid.grid = datagrid = this.jqxGrid("getInstance"); // init jqxGrid
    datagrid._Init(opts);

    console.log('jqxGrid init.');
    if (o.events)
        for (var e in o.events)
            $grid.on(e, o.events[e]);

    //new $.injection(datagrid.dataview, 'refresh', datagrid._ResetEditor);

    //new $.injection(datagrid, '_render', function () {
    //    console.log('_render');
    //    datagrid.action_panel.dom_remove();
    //});
    return this;
};



    //$.fn.jqxGrid_formatData = function (data) {
    //    var inst = this.jqxGrid("getInstance");
    //    if (inst.dataview.filters.length > 0) {
    //        data._filter = new Array();
    //        for (var i = 0; i < inst.dataview.filters.length; i++) {
    //            var f1 = inst.dataview.filters[i];
    //            var f2 = f1.filter.getfilters();
    //            if (f2.length > 0) {
    //                var o = {
    //                    operator: f1.operator,
    //                    datafield: f1.datafield,
    //                    filters: f2,
    //                    //str: new Array(),
    //                }
    //                //for (var j = 0; j < f2.length; j++) o.str.push(JSON.stringify(f2[j]));
    //                data._filter.push(o);
    //            }
    //        }
    //    }
    //    data._sort = inst.getsortinformation();
    //    data._pager = inst.getpaginginformation();
    //    return data;
    //};





//function row_editor(rowid, datagrid) {
//	var _this = this;
//	this.rowid = rowid;
//	this.owner = datagrid;
//	this.update_count = 0;
//	this.isEditing = false;
//	this.isRemoving = false;
//	this.isNew = false;
//	_this.cellelements = {};
//	_this.cells = {};
//	//(this.reset = function () {
//	//})();

//	this.update = function () {
//		this.update_count++;
//		_this.owner._renderrows(_this.owner.virtualsizeinfo);
//		//this.owner._updatepageviews();
//		//for (var i = 0; i < _this.owner.columns.records.length; i++) {
//		//    if (_this.owner.columns.records[i].editable != true) continue;
//		//    var boundindex = _this.owner.getrowboundindexbyid(_this.rowid);
//		//    _this.rendercell_cancel = true;
//		//    datagrid.begincelledit(boundindex, _this.owner.columns.records[i].datafield);
//		//    _this.rendercell_cancel = false;
//		//    datagrid.endcelledit(boundindex, _this.owner.columns.records[i].datafield, true);
//		//    break;
//		//}
//	};

//	this.BeginEditRow = function () {
//		if (_this.isRemoving) return;
//		_this.isEditing = true;
//		_this.update(_this.rowid);
//	};
//	this.EndEditRow = function () {
//		if (_this.isRemoving) return;
//		_this.isEditing = false;
//		if (_this.isNew)
//			_this.owner.deleterow(rowid);
//		else
//			_this.update(_this.rowid);
//	};
//	this.CommitEditRow = function () {
//		if (_this.isRemoving) return;
//		_this.isEditing = false;
//		_this.update(_this.rowid);
//	};
//	this.BeginRemoveRow = function () {
//		if (_this.isEditing) return;
//		_this.isRemoving = true;
//		_this.update(_this.rowid);
//	};
//	this.EndRemoveRow = function () {
//		if (_this.isEditing) return;
//		_this.isRemoving = false;
//		_this.update(_this.rowid);
//	}
//	this.CommitRemoveRow = function () {
//		if (_this.isEditing) return;
//		_this.isRemoving = false;
//		_this.update(_this.rowid);
//	};

//};


//$.extend($.jqx._jqxGrid.prototype, {
//	_CleanupEditor: function (rowid) {
//		var data = this.getrowdatabyid(rowid);
//		if (data != null) return;
//		console.log(rowid);
//		delete this.row_editors[rowid];
//	},
//	getRowEditor: function (rowid, boundindex, bound) {
//		if (bound != null)
//			boundindex = this.getboundindex(bound);
//		if (boundindex != null)
//			rowid = this.getrowid(boundindex);
//		if (rowid == null) return;
//		if (this.row_editors[rowid])
//			return this.row_editors[rowid];
//		else
//			return this.row_editors[rowid] = new row_editor(rowid, this);
//	},
//	AddRow: function (data) {
//		data = data || {};
//		//this.source.addrow = function (rowid, rowdata, position, commit) {
//		//    uid = rowid;
//		//    commit(true);
//		//};
//		if (this.addrow(null, data, 'first')) {
//			var editor = this.getRowEditor(data.uid);
//			editor.isNew = true;
//			editor.BeginEditRow();
//		}
//		//var editor = this.getRowEditor(row);
//	},
//	EditRow: function (rowid, commit) {
//		var editor = this.getRowEditor(rowid);
//		if (commit === true)
//			editor.CommitEditRow();
//		else if (commit === false)
//			editor.EndEditRow();
//		else
//			editor.BeginEditRow();
//	},
//	RemoveRow: function (rowid, commit) {
//		var editor = this.getRowEditor(rowid);
//		if (commit === true)
//			editor.CommitRemoveRow();
//		else if (commit === false)
//			editor.EndRemoveRow();
//		else
//			editor.BeginRemoveRow();
//	},
//});





//var grid_editor = function (owner) {
//	var _editor = this;
//	this.owner = owner;
//	//this.rows = new Array();
//	//this.columns = new Array();
//	//for (var i = 0; i < owner.columns.length; i++) {
//	//	this.columns.push(owner.columns[i]);
//	//	//owner.columns[i].grid_editor = this;
//	//}

//	//this.GetColumn = function (datafield) {
//	//	for (var i = 0; i < this.columns.length; i++) {
//	//		if (this.columns[i].datafield == datafield) {
//	//			return this.columns[i];
//	//		}
//	//	}
//	//};

//	//this.GetRow = function (data) {
//	//	if (data.bounddata)
//	//		data = data.bounddata;
//	//	var row = null;
//	//	for (var r = 0; r < this.rows.length; r++) {
//	//		row = this.rows[r];
//	//		for (var k = 0; k < this.owner.pkey.length; k++) {
//	//			if (row == null) break;
//	//			var name = this.owner.pkey[k];
//	//			if (data[name] == null) row = null;
//	//			else if (row.keys[name] == null) row = null;
//	//			else if (data[name] != row.keys[name]) row = null;
//	//		}
//	//		if (row != null)
//	//			return row;
//	//	}
//	//	if (row == null) {
//	//		var keys = {
//	//		};
//	//		for (var k = 0; k < this.owner.pkey.length; k++) {
//	//			if (keys == null) break;
//	//			var name = this.owner.pkey[k];
//	//			if (data[name] == null) keys = null;
//	//			else keys[name] = data[name];
//	//		}
//	//		row = new grid_editor.row(this, keys);
//	//	}
//	//	return row;
//	//};

//$.jqx._jqxGrid.prototype._InitRow = function (bound) {
//	if (bound._cells == null) {
//	}
//	return bound;
//}




$.col.defines = (function () {
    function Action () {
        return $.extend(true, {
            __addfilterwidget: function (element, width, height) {
                var column = this;
                column.__init();

                var txt = column.Action1.filters.text[column.ActionIndex];
                    
                var margin = 4;
                var $div = $('<div></div>')
                    .appendTo(element)
                    .css({ margin: margin })
                    .jqxButton({ width: width - 8 - margin * 2, height: height - 8 - margin * 2 })
                    .on('click', function () { column.owner[ column.Action1.filters.action[column.ActionIndex]](); });
                $('<div class="jqx-icon"></div>').appendTo($div).addClass(column.owner.toThemeProperty('jqx-icon-' + column.Action1.filters.icon[column.ActionIndex]));

                //var m = $("<div></div>");
                //m.width(d.width());
                //m.height(this.filterrowheight);
                //d.append(m);
                //var A = d.width() - 20;
                //var s = function (I, J, f) {
                //    var H = $('<input style="float: left;" autocomplete="off" type="textarea"/>');
                //    if (G.rtl) {
                //        H.css("float", "right");
                //        H.css("direction", "rtl")
                //    }
                //    H[0].id = $.jqx.utilities.createId();
                //    H.addClass(G.toThemeProperty("jqx-widget"));
                //    H.addClass(G.toThemeProperty("jqx-input"));
                //    H.addClass(G.toThemeProperty("jqx-rc-all"));
                //    H.addClass(G.toThemeProperty("jqx-widget-content"));
                //    H.appendTo(I);
                //    H.width(J - 10);
                //    if (G.disabled) {
                //        H.attr("disabled", true)
                //    }
                //    H.attr("disabled", false);
                //    H.height(G.filterrowheight - 10);
                //    H.css("margin", "4px");
                //    H.css("margin-right", "2px");
                //    H.focus(function () {
                //        G.focusedfilter = H;
                //        H.addClass(G.toThemeProperty("jqx-fill-state-focus"))
                //    });
                //    H.blur(function () {
                //        H.removeClass(G.toThemeProperty("jqx-fill-state-focus"))
                //    });
                //    H.keydown(function (K) {
                //        if (K.keyCode == "13") {
                //            G._applyfilterfromfilterrow()
                //        }
                //        if (H[0]._writeTimer) {
                //            clearTimeout(H[0]._writeTimer)
                //        }
                //        H[0]._writeTimer = setTimeout(function () {
                //            if (!G._loading) {
                //                if (G["_oldWriteText" + H[0].id] != H.val()) {
                //                    G._applyfilterfromfilterrow();
                //                    G["_oldWriteText" + H[0].id] = H.val()
                //                }
                //            }
                //        }, C.filterdelay);
                //        G.focusedfilter = H
                //    });
                //    H.val(z);
                //    return H
                //};
                //s(m, A);
                //var B = G._getfiltersbytype(C.filtertype == "number" ? "number" : "string");
                //var t = $("<div class='filter' style='float: left;'></div>");
                //t.css("margin-top", "4px");
                //t.appendTo(m);
                //if (G.rtl) {
                //    t.css("float", "right")
                //}
                //var h = 0;
                //if (C.filtercondition != null) {
                //    var E = new $.jqx.filter();
                //    var r = E.getoperatorsbyfiltertype(C.filtertype == "number" ? "numericfilter" : "stringfilter");
                //    var e = r.indexOf(C.filtercondition.toUpperCase());
                //    if (e != -1) {
                //        h = e
                //    }
                //}
                //var D = 170;
                //if (C.filtertype == "input") {
                //    D = 240;
                //    if (h == 0) {
                //        var e = B.indexOf("contains");
                //        if (e != -1 && C.filtercondition == null) {
                //            h = e
                //        }
                //    }
                //}
                //t.jqxDropDownList({
                //    disabled: G.disabled,
                //    touchMode: G.touchmode,
                //    rtl: G.rtl,
                //    dropDownHorizontalAlignment: "right",
                //    enableBrowserBoundsDetection: true,
                //    selectedIndex: h,
                //    width: 18,
                //    height: 21,
                //    dropDownHeight: 150,
                //    dropDownWidth: D,
                //    source: B,
                //    theme: G.theme
                //});
                //t.jqxDropDownList({
                //    selectionRenderer: function (f) {
                //        return ""
                //    }
                //});
                //t.jqxDropDownList("setContent", "");
                //t.find(".jqx-dropdownlist-content").hide();
                //if (C.createfilterwidget) {
                //    C.createfilterwidget(C, d, m)
                //}
                //C._filterwidget = m;
                //var j = null;
                //this.addHandler(t, "select", function () {
                //    var f = t.jqxDropDownList("getSelectedItem").label;
                //    if (C._filterwidget.find("input").val().length > 0 && !G.refreshingfilter) {
                //        G._applyfilterfromfilterrow()
                //    }
                //    if (C.filtertype == "input" && !G.refreshingfilter) {
                //        G._applyfilterfromfilterrow()
                //    } else {
                //        if (C._filterwidget.find("input").val().length == 0 && !G.refreshingfilter) {
                //            if (j == "null" || j == "not null" || f == "null" || f == "not null") {
                //                G._applyfilterfromfilterrow()
                //            }
                //        }
                //    }
                //    j = f
                //});


                return true;
            },
        }, arguments[0]);
    }

})()  ;




//$.replace_function($.jqx._jqxGrid.prototype, '_addfilterwidget', function (column, element, width) {
//    if (column._addfilterwidget) if (column._addfilterwidget.call(column, element, width, column.owner.filterrowheight) === true) return;
//    return arguments.callee._original.apply(this, arguments);
//});
//$.replace_function($.jqx._jqxGrid.prototype, '_getfilterdataadapter', function (column) {
//    if ($.isFunction(column._getfilterdataadapter))
//        return column._getfilterdataadapter();
//    var ret = arguments.callee._original.apply(this, arguments);
//    console.log('_getfilterdataadapter', { column: column, result: ret });
//    return ret;
//});





//function check_cells(n) {
//    for (var i = 0; i < this.rows.records.length; i++) {
//        var bound = this.rows.records[i];
//        for (var j = 0; j < bound._cells.length; j++) {
//            var cell = bound._cells[j];
//            if (cell.editor == null) continue;
//            if (!$.contains(this.editor, cell.editor[0])) {
//                console.log(n, cell);
//            }
//        }
//    }
//}

//$.replace_function($.jqx._jqxGrid.prototype, '_renderrows_xx', function () {
//	//for (var i = 0; i < this.rows.records.length; i++) {
//	//	var bound = this.rows.records[i];
//	//	for (var j = 0; j < bound._cells.length; j++) {
//	//		var cell = bound._cells[j];
//	//		if (cell.editor == null) continue;
//	//		cell.editor.hide();
//	//		cell.editor.appendTo(this.editor);
//	//		//if (!$.contains(this.editor, cell.editor[0])) {
//	//		//	console.log(n, cell);
//	//		//}
//	//	}
//	//}
//	//check_cells.call(this, 1);
//	var ret = arguments.callee._original.apply(this, arguments);
//	//check_cells.call(this, 2);
//	return ret;
//});




//})(jqxBaseFramework);
//
//// column preset
//(function ($) {



