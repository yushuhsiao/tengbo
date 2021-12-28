// 參考用...
(function ($) {
    var reference = {
        colModel: {
            align: 'left',  // left, center, right
            cellattr: function (rowId, tv, rawObject, cm, rdata) { },
            classes: '',
            datefmt: 'Y-m-d',
            defval: null,

            formatter: { // 'integer', 'number', 'currency', 'date', 'email', 'link', 'showlink', 'checkbox', 'select', 'actions'
                func: function (cellval, opts, rwdat, _act) {
                },
                integer: {
                    thousandsSeparator: null,       // thousandsSeparator determines the separator for the thousands
                    defaulValue: null               // defaultValue set the default value if nothing in the data
                },
                number: {
                    decimalSeparator: null,         // thousandsSeparator determines the separator for the thousands
                    thousandsSeparator: null,       // decimalSeparator determines the separator for the decimals
                    decimalPlaces: null,            // decimalPlaces determine how many decimal places we should have for the number
                    defaulValue: null               // defaultValue set the default value if nothing in the data
                },
                currency: {
                    decimalSeparator: null,         // thousandsSeparator determines the separator for the thousands
                    thousandsSeparator: null,       // decimalSeparator determines the separator for the decimals
                    decimalPlaces: null,            // decimalPlaces determine how many decimal places we should have for the number
                    defaulValue: null,              // defaultValue set the default value if nothing in the data
                    prefix: null,                   // text that is inserted before the number
                    suffix: null                    // text that is added after the number
                },
                date: {
                    srcformat: null,                // srcformat is the source format - i.e. the format of the date that should be converted
                    newformat: null,                // newformat is the new output format. The definition of the date format uses the PHP conversions. Also you can use a set of predefined date format - see the mask options in the default date formatting set
                    parseRe: null                   // parseRe is a expression for parsing date strings
                },
                email: {                            // When a mail type is used we directly add a href with mailto: before the e-mail
                },
                link: {
                    target: null                    // The default value of the target options is null. When this options is set, we construct a link with the target property set and the cell value put in the href tag.
                },
                showlink: {
                    baseLinkUrl: null,              // baseLinkUrl is the link. 
                    showAction: null,               // showAction is an additional value which is added after the baseLinkUrl. 
                    addParam: null,                 // addParam is an additional parameter that can be added after the idName property. 
                    target: null,                   // target, if set, is added as an additional attribute. 
                    idName: null                    // idName is the first parameter that is added after the showAction. By default, this is id,
                },
                checkbox: {
                    disabled: null                  // The default value for the disabled is true. This option determines if the checkbox can be changed. If set to false, the values in checkbox can be changed
                },
                select: {                           // this is not a real select but a special case. See note below
                },
                actions: {                          // This type of formatter is a easy way to add a buttons at certain column for inline editing. We add a two types of actions edit and delete. When the editformbutton parameter is set to true the form editing dialogue appear instead of in-line edit. The option editOptions will be used only for the form editing.
                    keys: false,
                    editbutton: true,
                    delbutton: true,
                    editformbutton: false,
                    onEdit: null,
                    onSuccess: null,
                    afterSave: null,
                    onError: null,
                    afterRestore: null,
                    extraparam: { oper: 'edit' },
                    url: null,
                    delOptions: {},
                    editOptions: {}
                }
            },
            formatoptions: null, // { thousandsSeparator:null, defaulValue:null}, {decimalSeparator:null,thousandsSeparator:null,decimalPlaces:null,defaulValue:null}

            editable: false,
            edittype: {
                text: {}, // <input type="text"/>
                password: {}, // <input type="password" />
                button: {}, // <input type="button" />
                image: {}, // <input type="image" />
                file: {}, // <input type="file" />
                checkbox: {}, //<input type="checkbox" value="Yes" offval="No" />
                textarea: { rows: null, cols: null }, // <textarea></textarea>
                select: {},
                custom: {},
            },   // 'text', 'textarea', 'select', 'checkbox', 'password', 'button', 'image', 'file', 'custom'
            editoptions: {
                value: 'mixed',
                dataUrl: null,
                buildSelect: function () { },
                dataInit: function () { },
                dataEvents: new Array(),
                defaultValue: null,
                NullIfEmpty: false,
                custom_element: function () { },
                custom_value: function () { },
            },
            editrules: {
                edithidden: null,   // This option is valid only in form editing module. By default the hidden fields are not editable. If the field is hidden in the grid and edithidden is set to true, the field can be edited when add or edit methods are called.
                required: null,     // (true or false) if set to true, the value will be checked and if empty, an error message will be displayed.
                number: null,       // (true or false) if set to true, the value will be checked and if this is not a number, an error message will be displayed.
                integer: null,      // (true or false) if set to true, the value will be checked and if this is not a integer, an error message will be displayed.
                minValue: null,     // if set, the value will be checked and if the value is less than this, an error message will be displayed.
                maxValue: null,     // if set, the value will be checked and if the value is more than this, an error message will be displayed.
                email: null,        // if set to true, the value will be checked and if this is not valid e-mail, an error message will be displayed
                url: null,          // if set to true, the value will be checked and if this is not valid url, an error message will be displayed
                date: null,         // if set to true a value from datefmt option is get (if not set ISO date is used) and the value will be checked and if this is not valid date, an error message will be displayed
                time: null,         // if set to true, the value will be checked and if this is not valid time, an error message will be displayed. Currently we support only hh:mm format and optional am/pm at the end
                custom: null,        // if set to true allow definition of the custom checking rules via a custom function. See below
                custom_func: function () { }
            },

            formoptions: null,

            search: true,
            stype: '',
            searchoptions: null,

            firstsortorder: null,
            fixed: false,
            frozen: false,
            hidedlg: false,
            hidden: false,
            index: null,
            jsonmap: null,
            key: false,
            label: null,
            name: 'Required',
            resizable: true,
            sortable: true,
            sorttype: '',   // 'int', 'integer', 'float', 'number', 'currency', 'date', 'text', function()
            surl: null,
            template: null,
            title: true,
            width: 150,
            xmlmap: null,
            unformat: function () { },
            viewable: true,
        },
        gridOptions: {
            lastpage: 0,
            pager: "",
            pagerpos: 'center',     // 'left', 'center', 'right'
            pgbuttons: true,
            pginput: true,
            pgtext: null,
            reccount: 0,
            recordpos: 'right',
            records: 0,
            recordtext: null,
            rowList: [],
            rowNum: 20,
            viewrecords: false,


            url: "",
            height: 150,
            page: 1,
            rowTotal: null,
            colModel: [],
            colNames: [],
            sortable: false,
            sortorder: "asc",
            sortname: "",
            datatype: "xml",
            mtype: "GET",
            altRows: false,
            selarrrow: [],
            savedRow: [],
            shrinkToFit: true,
            xmlReader: {},
            jsonReader: {},
            subGrid: false,
            subGridOptions: null,
            subGridType: null,
            subGridUrl: null,
            subGridModel: [],
            lastsort: 0,
            selrow: null,
            beforeSelectRow: null,
            onSelectRow: null,
            onSortCol: null,
            ondblClickRow: null,
            onRightClickRow: null,
            onPaging: null,
            onSelectAll: null,
            onInitGrid: null,
            loadComplete: null,
            gridComplete: null,
            loadError: null,
            loadBeforeSend: null,
            afterInsertRow: null,
            beforeRequest: null,
            beforeProcessing: null,
            onHeaderClick: null,
            loadonce: false,
            multiselect: false,
            multikey: false,
            editurl: null,
            search: false,
            caption: "",
            hidegrid: true,
            hiddengrid: false,
            postData: {},
            userData: {},
            treeGrid: false,
            treeGridModel: 'nested',
            treeReader: {},
            treeANode: -1,
            treeIcons: null,
            treedatatype: null,
            ExpandColumn: null,
            tree_root_level: 0,
            prmNames: { page: "page", rows: "rows", sort: "sidx", order: "sord", search: "_search", nd: "nd", id: "id", oper: "oper", editoper: "edit", addoper: "add", deloper: "del", subgridid: "id", npage: null, totalrows: "totalrows" },
            forceFit: false,
            gridstate: "visible",
            cellEdit: false,
            cellurl: null,
            cellsubmit: "remote",
            nv: 0,
            loadui: "enable",
            loadtext: null,
            toolbar: [false, "" /* 'top', 'bottom', 'both' */],
            scroll: false,
            multiboxonly: false,
            deselectAfterSort: true,
            scrollrows: false,
            autowidth: false,
            scrollOffset: 18,
            cellLayout: 5,
            subGridWidth: 20,
            multiselectWidth: 20,
            gridview: false,
            rownumWidth: 25,
            rownumbers: false,
            footerrow: false,
            userDataOnFooter: false,
            hoverrows: true,
            altclass: 'ui-priority-secondary',
            viewsortcols: [false, 'vertical', true],
            resizeclass: '',
            autoencode: false,
            remapColumns: [],
            ajaxGridOptions: {},
            direction: "ltr",
            toppager: false,
            headertitles: false,
            scrollTimeout: 40,
            data: [],
            datastr: null,
            _index: {},
            grouping: false,
            groupingView: { groupField: [], groupOrder: [], groupText: [], groupColumnShow: [], groupSummary: [], showSummaryOnHide: false, sortitems: [], sortnames: [], summary: [], summaryval: [], plusicon: 'ui-icon-circlesmall-plus', minusicon: 'ui-icon-circlesmall-minus', displayField: [] },
            ignoreCase: false,
            cmTemplate: {},
            idPrefix: "",
            multiSort: false,
            deepempty: false,
            emptyrecords: null,
            ExpandColClick: true,
            ExpandColumn: null,
            inlineData: null,
            totaltime: null,
        }
    }

    var colModel = function (a, b, c) {
        return new colModel.prototype.init(a, b, c);
    };
    colModel.prototype = {
        constructor: colModel,
        init: function (a, b, c) {
            this.a = a;
            this.b = b;
            this.c = c;
            return this;
        },
        c: 1,
    };
    colModel.prototype.init.prototype = colModel.prototype;
});


//function html_pin($t, pin) {
//    var types = {
//        _integer: parseInt,
//        _number: parseInt,
//        _numeric: parseInt,
//        _array: function (value) { return eval(value); },
//        _boolean: function (value) {
//            if (value == 'true')
//                return true;
//            else if (value == 'false')
//                return false;
//        },
//        _string: function (value) { return value; },
//        _string_trim: function (value) { return $.trim(value); },
//        _mixed: function (value) { return value; },
//        _boolean_or_integer: function (value) { return value; },
//        _empty_object: function (value) { return value; },
//        _object: function (value) { return eval(value); },
//        _function: function (value) { return value; },
//        jQuery: jQuery,
//        _pager: function (value) {
//            if (value == 'true') {
//                var name = $.jgrid.jqID($t.id) + "_pager"
//                $($t).after('<div id="' + name + '"></div>');
//                return name;
//            }
//            if (value == 'false')
//                return '';
//            return value;
//        }
//    }
//    if (pin.pager == true) { $($t).attr('pager', 'true') }
//    var pin_conv = {
//        nav1 /*					*/: ['nav1				', types.jQuery],
//        nav2 /*					*/: ['nav2				', types.jQuery],
//        ajaxgridoptions /*		*/: ['ajaxGridOptions	', types._object],//	empty object	Yes	This option allows to set global ajax settings for the grid when requesting data. Note that with this option it is possible to overwrite all current ajax settings in the grid including the error, complete and beforeSend events.
//        ajaxselectoptions /*	*/: ['ajaxSelectOptions	', types._object],//	empty object	Yes	This option allows to set global ajax settings for the select element when the select is obtained via dataUrl option in editoptions or searchoptionsobjects
//        altclass /*				*/: ['altclass			', types._string],//	ui-priority-secondary	Yes. Requires reload	The class that is used for applying different styles to alternate (zebra) rows in the grid. You can construct your own class and replace this value. This option is valid only if the altRows option is set to true
//        altrows /*				*/: ['altRows			', types._boolean],//	FALSE	Yes. After reload	Set a zebra-striped grid (alternate rows have different styles)
//        autoencode /*			*/: ['autoencode		', types._boolean],//	FALSE	Yes	When set to true encodes (html encode) the incoming (from server) and posted data (from editing modules). For example < will be converted to&lt;.
//        autowidth /*			*/: ['autowidth			', types._boolean],//	FALSE	No	When set to true, the grid width is recalculated automatically to the width of the parent element. This is done only initially when the grid is created. In order to resize the grid when the parent element changes width you should apply custom code and use the setGridWidth method for this purpose
//        caption /*				*/: ['caption			', types._string],//	empty string	No.Method avail.	Defines the caption for the grid. This caption appears in the caption layer, which is above the header layer (see How It Works). If the string is empty the caption does not appear.
//        celllayout /*			*/: ['cellLayout		', types._integer],//	5	No	This option determines the padding + border width of the cell. Usually this should not be changed, but if custom changes to the td element are made in the grid css file, this will need to be changed. The initial value of 5 means paddingLef(2) + paddingRight (2) + borderLeft (1) = 5
//        celledit /*				*/: ['cellEdit			', types._boolean],//	FALSE	Yes	Enables (disables) cell editing. See Cell Editing for more details
//        cellsubmit /*			*/: ['cellsubmit		', types._string],//	'remote'	Yes	Determines where the contents of the cell are saved. Possible values areremote and clientArray. See Cell Editing for more details.
//        cellurl	 /*				*/: ['cellurl			', types._string],//	null	Yes	the url where the cell is to be saved. See Cell Editing for more details
//        cmtemplate /*			*/: ['cmTemplate		', types._object],//	null	No	Defines a set of properties which override the default values in colModel. For example if you want to make all columns not sortable, then only one propery here can be specified instead of specifying it in all columns incolModel
//        colmodel /*				*/: ['colModel			', types._array],//	empty array	No	Array which describes the parameters of the columns.This is the most important part of the grid. For a full description of all valid values seecolModel API.
//        colnames /*				*/: ['colNames			', types._array],//	empty array[]	No	An array in which we place the names of the columns. This is the text that appears in the head of the grid (header layer). The names are separated with commas. Note that the number of elements in this array should be equal of the number elements in the colModel array.
//        data /*					*/: ['data				', types._array],//	empty array	Yes	An array that stores the local data passed to the grid. You can directly point to this variable in case you want to load an array data. It can replace the addRowData method which is slow on relative big data
//        datastr /*				*/: ['datastr			', types._string],//	null	Yes	The string of data when datatype parameter is set to xmlstring orjsonstring
//        datatype /*				*/: ['datatype			', types._string],//	xml	Yes	Defines in what format to expect the data that fills the grid. Valid options are xml (we expect data in xml format), xmlstring (we expect xml data as string), json (we expect data in JSON format), jsonstring (we expect JSON data as a string), local (we expect data defined at client side (array data)), javascript (we expect javascript as data), function (custom defined function for retrieving data), or clientSide to manually load data via the data array. See colModel API and Retrieving Data
//        deepempty /*			*/: ['deepempty			', types._boolean],//	FALSE	Yes	This option should be set to true if an event or a plugin is attached to the table cell. The option uses jQuery empty for the the row and all its children elements. This of course has speed overhead, but prevents memory leaks. This option should be set to true if a sortable rows and/or columns are activated.
//        deselectaftersort /*	*/: ['deselectAfterSort	', types._boolean],//	TRUE	Yes	Applicable only when we use datatype : local. Deselects currently selected row(s) when a sort is applied.
//        direction /*			*/: ['direction			', types._string],//	ltr	No	Determines the direction of text in the grid. The default is ltr (Left To Right). When set to rtl (Right To Left) the grid automatically changes the direction of the text. It is important to note that in one page we can have two (or more) grids where the one can have direction ltr while the other can have direction rtl. This option works only in Firefox 3.x versions and Internet Explorer versions >=6. Currently Safari, Google Chrome and Opera do not completely support changing the direction to rtl. The most common problem in Firefox is that the default settings of the browser do not support rtl. In order change this see this HOW TO section in this chapter .
//        editurl /*				*/: ['editurl			', types._string],//	null	Yes	Defines the url for inline and form editing. May be set to clientArray to manually post data to server, see Inline Editing.
//        emptyrecords /*			*/: ['emptyrecords		', types._string],//	see lang file	Yes	The string to display when the returned (or the current) number of records in the grid is zero. This option is valid only if viewrecords option is set totrue.
//        expandcolclick /*		*/: ['ExpandColClick	', types._boolean],//	TRUE	No	When true, the tree grid (see treeGrid) is expanded and/or collapsed when we click anywhere on the text in the expanded column. In this case it is not necessary to click exactly on the icon.
//        expandcolumn /*			*/: ['ExpandColumn		', types._string],//	null	No	Indicates which column (name from colModel) should be used to expand the tree grid. If not set the first one is used. Valid only when the treeGridoption is set to true.
//        footerrow /*			*/: ['footerrow			', types._boolean],//	FALSE	No	If set to true this will place a footer table with one row below the gird records and above the pager. The number of columns equal those specified in colModel
//        forcefit /*				*/: ['forceFit			', types._boolean],//	FALSE	No	If set to true, and a column's width is changed, the adjacent column (to the right) will resize so that the overall grid width is maintained (e.g., reducing the width of column 2 by 30px will increase the size of column 3 by 30px). In this case there is no horizontal scrollbar. Note: This option is not compatible with shrinkToFit option - i.e if shrinkToFit is set to false,forceFit is ignored.
//        gridstate /*			*/: ['gridstate			', types._string],//	visible	No	Determines the current state of the grid (i.e. when used with hiddengrid,hidegrid and caption options). Can have either of two states: visible orhidden.
//        gridview /*				*/: ['gridview			', types._boolean],//	FALSE	Yes	In the previous versions of jqGrid including 3.4.X, reading a relatively large data set (number of rows >= 100 ) caused speed problems. The reason for this was that as every cell was inserted into the grid we applied about 5 to 6 jQuery calls to it. Now this problem is resolved; we now insert the entry row at once with a jQuery append. The result is impressive - about 3 to 5 times faster. What will be the result if we insert all the data at once? Yes, this can be done with a help of gridview option (set it to true). The result is a grid that is 5 to 10 times faster. Of course, when this option is set to true we have some limitations. If set to true we can not use treeGrid, subGrid, or the afterInsertRow event. If you do not use these three options in the grid you can set this option to true and enjoy the speed.
//        grouping /*				*/: ['grouping			', types._boolean],//	FALSE	Yes	Enables grouping in grid. Please refer to the Grouping page.
//        headertitles /*			*/: ['headertitles		', types._boolean],//	FALSE	No	If the option is set to true the title attribute is added to the column headers.
//        height /*				*/: ['height			', types._mixed],//	150	No.Method avail.	The height of the grid. Can be set as number (in this case we mean pixels) or as percentage (only 100% is acceped) or value of auto is acceptable.
//        hiddengrid /*			*/: ['hiddengrid		', types._boolean],//	FALSE	No	If set to true the grid is initially is hidden. The data is not loaded (no request is sent) and only the caption layer is shown. When the show/hide button is clicked for the first time to show grid, the request is sent to the server, the data is loaded, and grid is shown. From this point we have a regular grid. This option has effect only if the caption property is not empty and the hidegrid property (see below) is set to true.
//        hidegrid /*				*/: ['hidegrid			', types._boolean],//	TRUE	No	Enables or disables the show/hide grid button, which appears on the right side of the caption layer (see How It Works). Takes effect only if thecaption property is not an empty string.
//        hoverrows /*			*/: ['hoverrows			', types._boolean],//	TRUE	Yes	When set to false the effect of mouse hovering over the grid data rows is disabled.
//        idprefix /*				*/: ['idPrefix			', types._string],//	empty	Yes	When set, this string is added as prefix to the id of the row when it is built.
//        ignorecase /*			*/: ['ignoreCase		', types._boolean],//	FALSE	Yes	By default the local searching is case-sensitive. To make the local search and sorting not case-insensitive set this options to true
//        inlinedata /*			*/: ['inlineData		', types._empty_object],//	{}	Yes	an array used to add content to the data posted to the server when we are in inline editing.
//        jsonreader /*			*/: ['jsonReader		', types._array],//		No	An array which describes the structure of the expected json data. For a full description and default setting, see Retrieving Data JSON Data
//        lastpage /*				*/: ['lastpage			', types._integer],//	0	No	Gives the total number of pages returned from the request. If you use a function as datatype, your_grid.setGridParam({lastpage: your_number}) can be used to specify the max pages in the pager.
//        lastsort /*				*/: ['lastsort			', types._integer],//	0	No	Readonly property. Gives the index of last sorted column beginning from 0.
//        loadonce /*				*/: ['loadonce			', types._boolean],//	FALSE	No	If this flag is set to true, the grid loads the data from the server only once (using the appropriate datatype). After the first request, the datatype parameter is automatically changed to local and all further manipulations are done on the client side. The functions of the pager (if present) are disabled.
//        loadtext /*				*/: ['loadtext			', types._string],//	Loading…	No	The text which appears when requesting and sorting data. This parameter is located in language file.
//        loadui /*				*/: ['loadui			', types._string],//	enable	Yes	This option controls what to do when an ajax operation is in progress.
//        /*						*/										//			disable - disables the jqGrid progress indicator. This way you can use your own indicator.
//        /*						*/										//			enable (default) - shows the text set in the loadtext property (default value is Loading…) in the center of the grid. 
//        /*						*/										//			block - displays the text set in the loadtext property and blocks all actions in the grid until the ajax request completes. Note that this disables paging, sorting and all actions on toolbar, if any.
//        mtype /*				*/: ['mtype				', types._string],//	GET	Yes	Defines the type of request to make (“POST” or “GET”)
//        multikey /*				*/: ['multikey			', types._string],//	empty string	Yes	This parameter makes sense only when the multiselect option is set totrue. Defines the key which should be pressed when we make multiselection. The possible values are: shiftKey - the user should press Shift Key, altKey - the user should press Alt Key, and ctrlKey - the user should press Ctrl Key.
//        multiboxonly /*			*/: ['multiboxonly		', types._boolean],//	FALSE	Yes	This option works only when the multiselect option is set to true. Whenmultiselect is set to true, clicking anywhere on a row selects that row; when multiboxonly is also set to true, the multiselection is done only when the checkbox is clicked (Yahoo style). Clicking in any other row (suppose the checkbox is not clicked) deselects all rows and selects the current row.
//        multiselect /*			*/: ['multiselect		', types._boolean],//	FALSE	No. see HOWTO	If this flag is set to true a multi selection of rows is enabled. A new column at left side containing checkboxes is added. Can be used with any datatype option.
//        multiselectwidth /*		*/: ['multiselectWidth	', types._integer],//	20	No	Determines the width of the checkbox column created when themultiselect option is enabled.
//        multisort /*			*/: ['multiSort			', types._boolean],//	FALSE	Yes	If set to true enables the multisorting. The options work if the datatype is local. In case when the data is obtained from the server the sidxparameter contain the order clause. It is a comma separated string in format field1 asc, field2 desc …, fieldN. Note that the last field does not not have asc or desc. It should be obtained from sord parameter 
//        /*						*/										//			When the option is true the behavior is a s follow. The first click of the header field sort the field depending on the firstsortoption parameter in colModel or sortorder grid parameter. The next click sort it in reverse order. The third click removes the sorting from this field
//        page /*					*/: ['page				', types._integer],//	1	Yes	Set the initial page number when we make the request.This parameter is passed to the url for use by the server routine retrieving the data.
//        pager /*				*/: ['pager				', types._pager],//	empty string.Currently only one pagebar is possible.	No	Defines that we want to use a pager bar to navigate through the records. This must be a valid HTML element; in our example we gave the div the id of “pager”, but any name is acceptable. Note that the navigation layer (the “pager” div) can be positioned anywhere you want, determined by your HTML; in our example we specified that the pager will appear after the body layer. The valid settings can be (in the context of our example)pager, #pager, jQuery('#pager'). I recommend to use the second one -#pager. See Pager
//        pagerpos /*				*/: ['pagerpos			', types._string],//	center	No	Determines the position of the pager in the grid. By default the pager element when created is divided in 3 parts (one part for pager, one part for navigator buttons and one part for record information).
//        pgbuttons /*			*/: ['pgbuttons			', types._boolean],//	TRUE	No	Determines if the Pager buttons should be shown if pager is available. Also valid only if pager is set correctly. The buttons are placed in the pager bar.
//        pginput /*				*/: ['pginput			', types._boolean],//	TRUE	No	Determines if the input box, where the user can change the number of the requested page, should be available. The input box appears in the pager bar.
//        pgtext /*				*/: ['pgtext			', types._string],//	See lang file	Yes	Show information about current page status. The first value is the current loaded page. The second value is the total number of pages.
//        prmnames /*				*/: ['prmNames			', types._array],//	none	Yes	The default value of this property is: 
//        /*						*/										//			{page:“page”,rows:“rows”, sort:“sidx”, order:“sord”, search:“_search”, nd:“nd”, id:“id”, oper:“oper”, editoper:“edit”, addoper:“add”, deloper:“del”, subgridid:“id”, npage:null, totalrows:“totalrows”} 
//        /*						*/										//			This customizes names of the fields sent to the server on a POST request. For example, with this setting, you can change the sort order element from sidx to mysort by setting prmNames: {sort: “mysort”}. The string that will be POST-ed to the server will then be myurl.php?page=1&rows=10&mysort=myindex&sord=asc rather than myurl.php?page=1&rows=10&sidx=myindex&sord=asc 
//        /*						*/										//			So the value of the column on which to sort upon can be obtained by looking at $POST['mysort'] in PHP. When some parameter is set to null, it will be not sent to the server. For example if we set prmNames: {nd:null}the nd parameter will not be sent to the server. For npage option see thescroll option. 
//        /*						*/										//			These options have the following meaning and default values: 
//        /*						*/										//			page: the requested page (default value page) 
//        /*						*/										//			rows: the number of rows requested (default value rows) 
//        /*						*/										//			sort: the sorting column (default value sidx) 
//        /*						*/										//			order: the sort order (default value sord) 
//        /*						*/										//			search: the search indicator (default value _search) 
//        /*						*/										//			nd: the time passed to the request (for IE browsers not to cache the request) (default value nd) 
//        /*						*/										//			id: the name of the id when POST-ing data in editing modules (default value id) 
//        /*						*/										//			oper: the operation parameter (default value oper) 
//        /*						*/										//			editoper: the name of operation when the data is POST-ed in edit mode (default value edit) 
//        /*						*/										//			addoper: the name of operation when the data is posted in add mode (default value add) 
//        /*						*/										//			deloper: the name of operation when the data is posted in delete mode (default value del) 
//        /*						*/										//			totalrows: the number of the total rows to be obtained from server - seerowTotal (default value totalrows) 
//        /*						*/										//			subgridid: the name passed when we click to load data in the subgrid (default value id)
//        postdata /*				*/: ['postData			', types._array],//	empty array	Yes	This array is appended directly to the url. This is an associative array and can be used this way: {name1:value1…}. See API methods for manipulation.
//        reccount /*				*/: ['reccount			', types._integer],//	0	No	Readonly property. Determines the exact number of rows in the grid. Do not confuse this with records parameter. Although in many cases they may be equal, there are cases where they are not. For example, if you define rowNum to be 15, but the request to the server returns 20 records, the records parameter will be 20, but the reccount parameter will be 15 (the grid you will have 15 records and not 20).
//        recordpos /*			*/: ['recordpos			', types._string],//	right	No	Determines the position of the record information in the pager. Can beleft, center, right.
//        records /*				*/: ['records			', types._integer],//	none	No	Readonly property. Gives the number of records returned as a result of a query to the server.
//        recordtext /*			*/: ['recordtext		', types._string],//	see lang file	Yes	Text that can be shown in the pager. Also this option is valid ifviewrecords option is set to true. This text appears only if the total number of records is greater then zero. In order to show or hide some information the items in {} mean the following: 
//        /*						*/										//			{0} - the start position of the records depending on page number and number of requested records 
//        /*						*/										//			{1} - the end position 
//        /*						*/										//			{2} - total records returned from the server.
//        resizeclass /*			*/: ['resizeclass		', types._string],//	empty string	No	Assigns a class to columns that are resizable so that we can show a resize handle only for ones that are resizable.
//        rowlist /*				*/: ['rowList			', types._array],//	empty arrray	No	An array to construct a select box element in the pager in which we can change the number of the visible rows. When changed during the execution, this parameter replaces the rowNum parameter that is passed to the url. If the array is empty, this element does not appear in the pager. Typically you can set this like [10,20,30]. If the rowNum parameter is set to 30 then the selected value in the select box is 30.
//        rownumbers /*			*/: ['rownumbers		', types._boolean],//	FALSE	No	If this option is set to true, a new column at left of the grid is added. The purpose of this column is to count the number of available rows, beginning from 1. In this case colModel is extended automatically with new element with the name rn. Note: Do not to use the name rn in thecolModel.
//        rownum /*				*/: ['rowNum			', types._integer],//	20	Yes	Sets how many records we want to view in the grid. This parameter is passed to the url for use by the server routine retrieving the data. Note that if you set this parameter to 10 (i.e. retrieve 10 records) and your server return 15 then only 10 records will be loaded. Set this parameter to-1 (unlimited) to disable this checking.
//        rowtotal /*				*/: ['rowTotal			', types._integer],//	null	Yes	When set this parameter can instruct the server to load the total number of rows needed to work on. Note that rowNum determines the total records displayed in the grid, while rowTotal determines the total number of rows on which we can operate. When this parameter is set, we send an additional parameter to the server named totalrows. You can check for this parameter, and if it is available you can replace the rows parameter with this one. Mostly this parameter can be combined with loadonceparameter set to true.
//        rownumwidth /*			*/: ['rownumWidth		', types._integer],//	25	No	Determines the width of the row number column if rownumbers option is set to true.
//        savedrow /*				*/: ['savedRow			', types._array],//	empty array	No	This is a readonly property and is used in inline and cell editing modules to store the data, before editing the row or cell. See Cell Editing and Inline Editing.
//        searchdata /*			*/: ['searchdata		', types._array],//	empty array{}	Yes	This property contain the searched data in pair name:value.
//        scroll /*				*/: ['scroll			', types._boolean_or_integer],//	FALSE	No	Creates dynamic scrolling grids. When enabled, the pager elements are disabled and we can use the vertical scrollbar to load data. When set totrue the grid will always hold all the items from the start through to the latest point ever visited. 
//        /*						*/										//			When scroll is set to an integer value (example 1), the grid will just hold the visible lines. This allow us to load the data in portions whitout caring about memory leaks. In addition to this we have an optional extension to the server protocol: npage (see prmNames array). If you set the npage option in prmNames, then the grid will sometimes request more than one page at a time; if not, it will just perform multiple GET requests.
//        scrolloffset /*			*/: ['scrollOffset		', types._integer],//	18	No.Method avail.	Determines the width of the vertical scrollbar. Since different browsers interpret this width differently (and it is difficult to calculate it in all browsers) this can be changed.
//        scrolltimeout /*		*/: ['scrollTimeout		', types._integer],//	200	Yes	This controls the timeout handler when scroll is set to 1.
//        scrollrows /*			*/: ['scrollrows		', types._boolean],//	FALSE	Yes	When enabled, selecting a row with setSelection scrolls the grid so that the selected row is visible. This is especially useful when we have a verticall scrolling grid and we use form editing with navigation buttons (next or previous row). On navigating to a hidden row, the grid scrolls so that the selected row becomes visible.
//        selarrrow /*			*/: ['selarrrow			', types._array],//	empty array []	No	This options is readonly. Gives the currently selected rows whenmultiselect is set to true. This is a one-dimensional array and the values in the array correspond to the selected id's in the grid.
//        selrow /*				*/: ['selrow			', types._string],//	null	No	This option is readonly. It contains the id of the last selected row. If you sort or use paging, this options is set to null.
//        shrinktofit /*			*/: ['shrinkToFit		', types._boolean_or_integer],//	TRUE	No	This option, if set, defines how the the width of the columns of the grid should be re-calculated, taking into consideration the width of the grid. If this value is true, and the width of the columns is also set, then every column is scaled in proportion to its width. For example, if we define two columns with widths 80 and 120 pixels, but want the grid to have a width of 300 pixels, then the columns will stretch to fit the entire grid, and the extra width assigned to them will depend on the width of the columns themselves and the extra width available. The re-calculation is done as follows: the first column gets the width (300(new width)/200(sum of all widths))*80(first column width) = 120 pixels, and the second column gets the width (300(new width)/200(sum of all widths))*120(second column width) = 180 pixels. Now the widths of the columns sum up to 300 pixels, which is the width of the grid. If the value is false and the value inwidth option is set, then no re-sizing happens whatsoever. So in this example, if shrinkToFit is set to false, column one will have a width of 80 pixels, column two will have a width of 120 pixels and the grid will retain the width of 300 pixels. If the value of shrinkToFit is an integer, the width is calculated according to it.  - The effect of using an integer can be elaborated.
//        sortable /*				*/: ['sortable			', types._boolean],//	FALSE	No	When set to true, this option allows reordering columns by dragging and dropping them with the mouse. Since this option uses the jQuery UI sortable widget, be sure to load this widget and its related files in theHTML head tag of the page. Also, be sure to select the jQuery UI Addons option under the jQuery UI Addon Methods section while downloading jqGrid if you want to use this facility. Note: The colModel object also has a property called sortable, which specifies if the grid data can be sorted on a particular column or not.
//        sortname /*				*/: ['sortname			', types._string],//	empty string	Yes	The column according to which the data is to be sorted when it is initially loaded from the server (note that you will have to use datatypes xml or json to load remote data). This parameter is appended to the url. If this value is set and the index (name) matches the name from colModel, then an icon indicating that the grid is sorted according to this column is added to the column header. This icon also indicates the sorting order - descending or ascending (see the parameter sortorder). Also seeprmNames.
//        sortorder /*			*/: ['sortorder			', types._string],//	asc	Yes	The initial sorting order (ascending or descending) when we fetch data from the server using datatypes xml or json. This parameter is appended to the url - see prnNames. The two allowed values are - asc or desc.
//        subgrid /*				*/: ['subGrid			', types._boolean],//	FALSE	No	If set to true this enables using a sub-grid. If the subGrid option is enabled, an additional column at left side is added to the basic grid. This column contains a 'plus' image which indicates that the user can click on it to expand the row. By default all rows are collapsed. See Subgrid
//        subgridoptions /*		*/: ['subGridOptions	', types._object],//	see Subgrid	Yes	A set of additional options for the subgrid. For more information and default values see Subgrid.
//        subgridmodel /*			*/: ['subGridModel		', types._array],//	empty array	No	This property, which describes the model of the subgrid, has an effect only if the subGrid property is set to true. It is an array in which we describe the column model for the subgrid data. For more information seeSubgrid.
//        subgridtype /*			*/: ['subGridType		', types._mixed],//	null	Yes	This option allows loading a subgrid as a service. If not set, the datatypeparameter of the parent grid is used.
//        subgridurl /*			*/: ['subGridUrl		', types._string],//	empty string	Yes	This option has effect only if the subGrid option is set to true. This option points to the url from which we get the data for the subgrid. jqGrid adds the id of the row to this url as parameter. If there is a need to pass additional parameters, use the params option in subGridModel. SeeSubgrid
//        subgridwidth /*			*/: ['subGridWidth		', types._integer],//	20	No	Defines the width of the sub-grid column if subgrid is enabled.
//        toolbar /*				*/: ['toolbar			', types._array],//	[false, '']	No	This option defines the toolbar of the grid. This is an array with two elements in which the first element's value enables the toolbar and the second defines the position relative to the body layer (table data). Possible values are top, bottom, and both. When we set it like toolbar: [true,”both”] two toolbars are created – one on the top of table data and the other at the bottom of the table data. When we have two toolbars, then we create two elements (div). The id of the top bar is constructed by concatenating the string “t_” and the id of the grid, like“t_” + id_of_the_grid and the id of the bottom toolbar is constructed by concatenating the string “tb_” and the id of the grid, like “tb_” + id_of_the grid. In the case where only one toolbar is created, we have the id as “t_” + id_of_the_grid, independent of where this toolbar is located (top or bottom)
//        toppager /*				*/: ['toppager			', types._boolean],//	FALSE	No	When enabled this option places a pager element at top of the grid, below the caption (if available). If another pager is defined, both can coexist and are kept in sync. The id of the newly created pager is the combinationgrid_id + “_toppager”.
//        totaltime /*			*/: ['totaltime			', types._integer],//	0	No	Readonly parameter. It gives the loading time of the records - currently available only when we load xml or json data. The measurement begins when the request is complete and ends when the last row is added.
//        treedatatype /*			*/: ['treedatatype		', types._mixed],//	null	No	Gives the initial datatype (see datatype option). Usually this should not be changed. During the reading process this option is equal to the datatype option.
//        treegrid /*				*/: ['treeGrid			', types._boolean],//	FALSE	No	Enables (disables) the tree grid format. For more details see Tree Grid
//        treegridmodel /*		*/: ['treeGridModel		', types._string],//	nested	No	Deteremines the method used for the treeGrid. The value can be eithernested or adjacency. See Tree Grid
//        treeicons /*			*/: ['treeIcons			', types._array],//		No	This array sets the icons used in the tree grid. The icons should be a valid names from UI theme roller images. The default values are: {plus:'ui-icon-triangle-1-e',minus:'ui-icon-triangle-1-s',leaf:'ui-icon-radio-off'}
//        treereader /*			*/: ['treeReader		', types._array],//		No	Extends the colModel defined in the basic grid. The fields described here are appended to end of the colModel array and are hidden. This means that the data returned from the server should have values for these fields. For a full description of all valid values see Tree Grid.
//        tree_root_level /*		*/: ['tree_root_level	', types._numeric],//	0	No	Defines the level where the root element begins when treeGrid is enabled.
//        url /*					*/: ['url				', types._string],//	null	Yes	The url of the file that returns the data needed to populate the grid. May be set to clientArray to manualy post data to server; see Inline Editing.
//        userdata /*				*/: ['userData			', types._array],//	empty array	No	This array contains custom information from the request. Can be used at any time.
//        userdataonfooter /*		*/: ['userDataOnFooter	', types._boolean],//	FALSE	Yes	When set to true we directly place the user data array userData in the footer. The rules are as follows: If the userData array contains a name which matches any name defined in colModel, then the value is placed in that column. If there are no such values nothing is placed. Note that if this option is used we use the current formatter options (if available) for that column.
//        viewrecords /*			*/: ['viewrecords		', types._boolean],//	FALSE	No	If true, jqGrid displays the beginning and ending record number in the grid, out of the total number of records in the query. This information is shown in the pager bar (bottom right by default)in this format: “View X to Y out of Z”. If this value is true, there are other parameters that can be adjusted, including emptyrecords and recordtext.
//        viewsortcols /*			*/: ['viewsortcols		', types._array],//	[false,'vertical',true]	No	The purpose of this parameter is to define a different look and behavior for the sorting icons (up/down arrows) that appear in the column headers. This parameter is an array with the following default options viewsortcols : [false,'vertical',true]. The first parameter determines if sorting icons should be visible on all the columns that have the sortable property set to true. Setting this value to true could be useful if you want to indicate to the user that (s)he can sort on that particular column. The default of false sets the icon to be visible only on the column on which that data has been last sorted. Setting this parameter to true causes all icons in all sortable columns to be visible.
//        /*						*/										//			The second parameter determines how icons should be placed - verticalmeans that the sorting arrows are one under the other. 'horizontal' means that the arrows should be next to one another. 
//        /*						*/										//			The third parameter determines the click functionality. If set to true the data is sorted if the user clicks anywhere in the column's header, not only the icons. If set to false the data is sorted only when the sorting icons in the headers are clicked. 
//        /*						*/										//			Important: If you are setting the third element to false, make sure that you set the first element to true; if you don't, the icons will not be visible and the user will not know where to click to be able to sort since clicking just anywhere in the header will not guarantee a sort.
//        width /*				*/: ['width				', types._number],//	none	No. Method avail.	If this option is not set, the width of the grid is the sum of the widths of the columns defined in the colModel (in pixels). If this option is set, the initial width of each column is set according to the value of theshrinkToFit option.
//        xmlreader /*			*/: ['xmlReader			', types._array]//		No	An array which describes the structure of the expected xml data. For a full description refer to Retrieving Data in XML Format.
//    }
//    for (var i = 0; i < $t.attributes.length; i++) {
//        var attr = $t.attributes[i];
//        var name = attr.name;
//        var value = $.trim(attr.nodeValue);
//        if (pin_conv.hasOwnProperty(name)) {
//            var m = pin_conv[name];
//            try { pin[$.trim(m[0])] = m[1](value); }
//            catch (e) { console.log("error", attr); }
//        }
//        //else console.log(name, attr);
//    }
//    var cm_conv = {
//        align /*				*/: ['align				', types._string],
//        cellattr /*				*/: ['cellattr			', types._function],
//        classes /*				*/: ['classes			', types._string],
//        datefmt /*				*/: ['datefmt			', types._string],
//        defval /*				*/: ['defval			', types._string],
//        editable /*				*/: ['editable			', types._boolean],
//        editoptions /*			*/: ['editoptions		', types._array],
//        editrules /*			*/: ['editrules			', types._array],
//        edittype /*				*/: ['edittype			', types._string],
//        firstsortorder /*		*/: ['firstsortorder	', types._string],
//        fixed /*				*/: ['fixed				', types._boolean],
//        formoptions /*			*/: ['formoptions		', types._array],
//        formatoptions /*		*/: ['formatoptions		', types._array],
//        formatter /*			*/: ['formatter			', types._mixed],
//        frozen /*				*/: ['frozen			', types._boolean],
//        hidedlg /*				*/: ['hidedlg			', types._boolean],
//        hidden /*				*/: ['hidden			', types._boolean],
//        index /*				*/: ['index				', types._string],
//        jsonmap /*				*/: ['jsonmap			', types._string],
//        key /*					*/: ['key				', types._boolean],
//        label /*				*/: ['label				', types._string],
//        name /*					*/: ['name				', types._string],
//        resizable /*			*/: ['resizable			', types._boolean],
//        search /*				*/: ['search			', types._boolean],
//        searchoptions /*		*/: ['searchoptions		', types._array],
//        sortable /*				*/: ['sortable			', types._boolean],
//        sorttype /*				*/: ['sorttype			', types._mixed],
//        stype /*				*/: ['stype				', types._string],
//        surl /*					*/: ['surl				', types._string],
//        template /*				*/: ['template			', types._object],
//        title /*				*/: ['title				', types._boolean],
//        width /*				*/: ['width				', types._number],
//        xmlmap /*				*/: ['xmlmap			', types._string],
//        unformat /*				*/: ['unformat			', types._function],
//        viewable /*				*/: ['viewable			', types._boolean]
//    }
//    for (var i = 0; i < pin.colModel.length; i++) {
//        var cm = pin.colModel[i];
//        cm.name = $.trim(cm.name);
//    }
//    var $tr = $($t).find('tr.colModel');
//    if ($tr.length == 1) {
//        $tr.children().each(function () {
//            $(this).attr('name', $.trim($(this).attr('name')));
//        });
//        for (var i = 0; i < pin.colModel.length; i++) {
//            var cm = pin.colModel[i];
//            var td = $tr.find('td[name="' + cm.name + '"]')[0];
//            if (td) {
//                cm.td = td;
//                for (var n = 0; n < td.attributes.length; n++) {
//                    var attr = td.attributes[n];
//                    var name = attr.name;
//                    var value = $.trim(attr.nodeValue);
//                    if (cm_conv.hasOwnProperty(name)) {
//                        var m = cm_conv[name];
//                        try { cm[$.trim(m[0])] = m[1](value); }
//                        catch (e) { console.log("error", attr); }
//                    }
//                }
//            }
//        }
//    }
//    //console.log(pin);
//}
