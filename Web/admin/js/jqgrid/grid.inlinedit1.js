/*jshint eqeqeq:false, eqnull:true, devel:true */
/*global jQuery */
(function($){
/**
 * jqGrid extension for manipulating Grid Data
 * Tony Tomov tony@trirand.com
 * http://trirand.com/blog/ 
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl-2.0.html
**/ 
"use strict";
$.jgrid.inlineEdit = $.jgrid.inlineEdit || {};

function editRow($t, ind, rowid, o) {
    var editable = $(ind).attr("editable") || "0";
    if (editable == "1" || $(ind).hasClass("not-editable-row"))
        return;
    if ($.isFunction(o.beforeEditRow)) if (o.beforeEditRow.call($t, o, rowid) == false) return;

    var cm = $t.p.colModel;
    var cnt = 0, focus = null, svr = {};
    $('td[role="gridcell"]', ind).each(function (i) {
        var nm = cm[i].name, tmp;
        var treeg = $t.p.treeGrid === true && nm === $t.p.ExpandColumn;
        if (treeg) { tmp = $("span:first", this).html(); }
        else {
            try {
                tmp = $.unformat.call($t, this, { rowId: rowid, colModel: cm[i] }, i);
            } catch (_) {
                tmp = (cm[i].edittype && cm[i].edittype === 'textarea') ? $(this).text() : $(this).html();
            }
        }
        if (nm !== 'cb' && nm !== 'subgrid' && nm !== 'rn') {
            if ($t.p.autoencode) { tmp = $.jgrid.htmlDecode(tmp); }
            svr[nm] = tmp;
            if ((cm[i].editonce === true ? $(ind).hasClass('jqgrid-new-row') : cm[i].editable) === true) {
                if (focus === null) { focus = i; }
                if (treeg) { $("span:first", this).html(""); }
                else { $(this).html(""); }
                var opt = $.extend({}, cm[i].editoptions || {}, { id: rowid + "_" + nm, name: nm });
                if (!cm[i].edittype) { cm[i].edittype = "text"; }
                if (tmp === "&nbsp;" || tmp === "&#160;" || (tmp.length === 1 && tmp.charCodeAt(0) === 160)) { tmp = ''; }
                var elc = $.jgrid.createEl.call($t, cm[i].edittype, opt, tmp, true, $.extend({}, $.jgrid.ajaxOptions, $t.p.ajaxSelectOptions || {}));
                $(elc).addClass("editable");
                if (treeg) { $("span:first", this).append(elc); }
                else { $(this).append(elc); }
                $.jgrid.bindEv.call($t, elc, opt);
                //Again IE
                if (cm[i].edittype === "select" && cm[i].editoptions !== undefined && cm[i].editoptions.multiple === true && cm[i].editoptions.dataUrl === undefined && $.jgrid.msie) {
                    $(elc).width($(elc).width());
                }
                cnt++;
            }
        }
    });
    if (cnt > 0) {
        svr.id = rowid; $t.p.savedRow.push(svr);
        $(ind).attr("editable","1");
        $("td:eq("+focus+") input",ind).focus();

        ind.restoreRow = function () { restoreRow($t, ind, rowid, $.extend(true, {}, o), svr) }
        ind.saveRow = function () { return saveRow($t, ind, rowid, $.extend(true, {}, o), svr); }
        ind.endRowEdit = function () {
            delete ind.endRowEdit;
            delete ind.restoreRow;
            delete ind.saveRow;
            $(ind).attr("editable", "0");
            if ($t.p._inlinenav) try { $($t).jqGrid('showAddEditButtons'); } catch (eer1) { }
            for (var k = 0; k < $t.p.savedRow.length; k++) {
                if (String($t.p.savedRow[k].id) === String(rowid)) {
                    $t.p.savedRow.splice(k, 1);
                }
            }
        }

        if(o.keys===true) {
            $(ind).bind("keydown",function(e) {
                if (e.keyCode === 27) { ind.restoreRow(); return false; }
                if (e.keyCode === 13) {
                    if (e.target.tagName === 'TEXTAREA') { return true; }
                    ind.saveRow();
                    return false;
                }
            });
        }
        $($t).triggerHandler("jqGridInlineEditRow", [rowid, o]);
        if( $.isFunction(o.oneditfunc)) { o.oneditfunc.call($t, rowid); }
    }
};

function renameRowID($t, ind, rowid, rowid_new) {
    if (ind.id !== $t.p.idPrefix + rowid_new) {
        var oldRowId = rowid;
        var oldid = $.jgrid.stripPref($t.p.idPrefix, rowid);
        if ($t.p._index[oldid] !== undefined) {
            $t.p._index[rowid_new] = $t.p._index[oldid];
            delete $t.p._index[oldid];
        }
        rowid = $t.p.idPrefix + rowid_new;
        $(ind).attr("id", rowid);
        if ($t.p.selrow === oldRowId) {
            $t.p.selrow = rowid;
        }
        if ($.isArray($t.p.selarrrow)) {
            var i = $.inArray(oldRowId, $t.p.selarrrow);
            if (i >= 0) {
                $t.p.selarrrow[i] = rowid;
            }
        }
        if ($t.p.multiselect) {
            var newCboxId = "jqg_" + $t.p.id + "_" + rowid;
            $("input.cbox", ind)
                .attr("id", newCboxId)
                .attr("name", newCboxId);
        }
        // TODO: to test the case of frozen columns
    }
    return rowid;
}

function saveRow($t, ind, rowid, o, svr) {
    var success = false;
    if ($.isFunction(o.beforeSaveRow)) if (o.beforeSaveRow.call($t, o, rowid) == true) return success;

    var tmp1 = {}, tmp2 = {}, postData = {}, cv;
    //var tmp1 = {}, tmp2 = {}, postData = $.extend(true, {}, $t.p.postData), cv;
    $('td[role="gridcell"]', ind).each(function (i) {
        var cm = $t.p.colModel[i];
        var nm = cm.name;
        if (nm !== 'cb' && nm !== 'subgrid' && cm.editable === true && nm !== 'rn' && !$(this).hasClass('not-editable-cell')) {
            switch (cm.edittype) {
                case "checkbox":
                    var cbv = ["Yes", "No"];
                    if (cm.editoptions) {
                        cbv = cm.editoptions.value.split(":");
                    }
                    tmp1[nm] = $("input", this).is(":checked") ? cbv[0] : cbv[1];
                    break;
                case 'text':
                case 'password':
                case 'textarea':
                case "button":
                    tmp1[nm] = $("input, textarea", this).val();
                    break;
                case 'select':
                    if (!cm.editoptions.multiple) {
                        tmp1[nm] = $("select option:selected", this).val();
                        tmp2[nm] = $("select option:selected", this).text();
                    } else {
                        var sel = $("select", this), selectedText = [];
                        tmp1[nm] = $(sel).val();
                        if (tmp1[nm]) { tmp1[nm] = tmp1[nm].join(","); } else { tmp1[nm] = ""; }
                        $("select option:selected", this).each(
                            function (i, selected) {
                                selectedText[i] = $(selected).text();
                            }
                        );
                        tmp2[nm] = selectedText.join(",");
                    }
                    if (cm.formatter && cm.formatter === 'select') { delete tmp2[nm]; }
                    break;
                case 'custom':
                    try {
                        if (cm.editoptions && $.isFunction(cm.editoptions.custom_value)) {
                            tmp1[nm] = cm.editoptions.custom_value.call($t, $(".customelement", this), 'get',null, cm.editoptions);
                            if (tmp1[nm] === undefined) { throw "e2"; }
                        } else { throw "e1"; }
                    } catch (e) {
                        if (e === "e1") { $.jgrid.info_dialog($.jgrid.errors.errcap, "function 'custom_value' " + $.jgrid.edit.msg.nodefined, $.jgrid.edit.bClose); }
                        if (e === "e2") { $.jgrid.info_dialog($.jgrid.errors.errcap, "function 'custom_value' " + $.jgrid.edit.msg.novalue, $.jgrid.edit.bClose); }
                        else { $.jgrid.info_dialog($.jgrid.errors.errcap, e.message, $.jgrid.edit.bClose); }
                    }
                    break;
            }
            cv = $.jgrid.checkValues.call($t, tmp1[nm], i);
            if (cv[0] === false) {
                return false;
            }
            if ($t.p.autoencode) { tmp1[nm] = $.jgrid.htmlEncode(tmp1[nm]); }
            if (o.url !== 'clientArray' && cm.editoptions && cm.editoptions.NullIfEmpty === true) {
                if (tmp1[nm] === "") {
                    postData[nm] = 'null';
                }
            }
        }
    });
    if (cv[0] === false){
        try {
            var tr = $t.rows.namedItem(rowid), positions = $.jgrid.findPos(tr);
            $.jgrid.info_dialog($.jgrid.errors.errcap,cv[1],$.jgrid.edit.bClose,{left:positions[0],top:positions[1]+$(tr).outerHeight()});
        } catch (e) {
            alert(cv[1]);
        }
        return success;
    }
    var idname, opers = $t.p.prmNames;
    if ($t.p.keyIndex === false) {
        idname = opers.id;
    } else {
        idname = $t.p.colModel[$t.p.keyIndex +
			($t.p.rownumbers === true ? 1 : 0) +
			($t.p.multiselect === true ? 1 : 0) +
			($t.p.subGrid === true ? 1 : 0)].name;
    }
    tmp1[opers.oper] = opers.editoper;
    if (tmp1[idname] === undefined || tmp1[idname]==="") {
        tmp1[idname] = rowid;
    }
    if($t.p.inlineData === undefined) { $t.p.inlineData ={}; }
    tmp1 = $.extend({},tmp1,$t.p.inlineData,o.extraparam);
    if (o.url === 'clientArray') {
        rowid = renameRowID($t, ind, rowid, tmp1[idname]);
        tmp1 = $.extend({}, tmp1, tmp2);
        if($t.p.autoencode) {
            $.each(tmp1,function(n,v){
                tmp1[n] = $.jgrid.htmlDecode(v);
            });
        }
        var resp = $($t).jqGrid("setRowData",rowid,tmp1);
        ind.endRowEdit();
        $($t).triggerHandler("jqGridInlineAfterSaveRow", [rowid, resp, tmp1, o]);
        if( $.isFunction(o.aftersavefunc) ) { o.aftersavefunc.call($t, rowid,resp, o); }
        success = true;
        $(ind).removeClass("jqgrid-new-row").unbind("keydown");
    } else {
        $("#lui_"+$.jgrid.jqID($t.p.id)).show();
        postData = $.extend({}, tmp1, postData);
        postData[idname] = $.jgrid.stripPref($t.p.idPrefix, postData[idname]);
        $.ajax($.extend({
            url:o.url,
            data: $.isFunction($t.p.serializeRowData) ? $t.p.serializeRowData.call($t, postData) : postData,
            type: o.mtype,
            async : false, //?!?
            complete: function(res,stat){
                $("#lui_"+$.jgrid.jqID($t.p.id)).hide();
                if (stat === "success"){
                    var ret = true, sucret;
                    sucret = $($t).triggerHandler("jqGridInlineSuccessSaveRow", [res, rowid, o]);
                    if (!$.isArray(sucret)) {sucret = [true, tmp1];}
                    if (sucret[0] && $.isFunction(o.successfunc)) {sucret = o.successfunc.call($t, res, rowid, o);}
                    if($.isArray(sucret)) {
                        // expect array - status, data, rowid
                        ret = sucret[0];
                        tmp1 = sucret[1] || tmp1;
                    } else {
                        ret = sucret;
                    }
                    if (ret === true) {
                        rowid = renameRowID($t, ind, rowid, tmp1[idname]);
                        if ($t.p.autoencode) {
                            $.each(tmp1,function(n,v){
                                tmp1[n] = $.jgrid.htmlDecode(v);
                            });
                        }
                        tmp1 = $.extend({},tmp1, tmp2);
                        for (var j = 0; j < $t.p.colModel.length; j++) {
                            var cm = $t.p.colModel[j];
                            if ((cm.editable === true) && (!tmp1.hasOwnProperty(cm.name)) && (svr.hasOwnProperty(cm.name)))
                                tmp1[cm.name] = svr[cm.name];
                        }
                        $($t).jqGrid("setRowData", rowid, tmp1);
                        ind.endRowEdit();
                        $($t).triggerHandler("jqGridInlineAfterSaveRow", [rowid, res, tmp1, o]);
                        if( $.isFunction(o.aftersavefunc) ) { o.aftersavefunc.call($t, rowid,res); }
                        success = true;
                        $(ind).removeClass("jqgrid-new-row").unbind("keydown");
                    } else {
                        $($t).triggerHandler("jqGridInlineErrorSaveRow", [rowid, res, stat, null, o]);
                        if($.isFunction(o.errorfunc) ) {
                            o.errorfunc.call($t, rowid, res, stat, null);
                        }
                        if(o.restoreAfterError === true) {
                            ind.restoreRow($t, ind, rowid, o, svr);
                        }
                    }
                }
            },
            error:function(res,stat,err){
                $("#lui_"+$.jgrid.jqID($t.p.id)).hide();
                $($t).triggerHandler("jqGridInlineErrorSaveRow", [rowid, res, stat, err, o]);
                if($.isFunction(o.errorfunc) ) {
                    o.errorfunc.call($t, rowid, res, stat, err);
                } else {
                    var rT = res.responseText || res.statusText;
                    try {
                        $.jgrid.info_dialog($.jgrid.errors.errcap,'<div class="ui-state-error">'+ rT +'</div>', $.jgrid.edit.bClose,{buttonalign:'right'});
                    } catch(e) {
                        alert(rT);
                    }
                }
                if(o.restoreAfterError === true) {
                    ind.restoreRow($t, ind, rowid, o, svr);
                    //$($t).jqGrid("restoreRow", rowid, o.afterrestorefunc);
                }
            }
        }, $.jgrid.ajaxOptions, $t.p.ajaxRowOptions || {}));
    }
    return success;
};

function restoreRow($t, ind, rowid, o, svr) {
    if ($.isFunction(o.beforeCancelRow)) if (o.beforeCancelRow.call($t, o, rowid) == true) return;

    var ares = {};
    if ($.isFunction($.fn.datepicker)) {
        try {
            $("input.hasDatepicker", "#" + $.jgrid.jqID(ind.id)).datepicker('hide');
        } catch (e) {
        }
    }
    var cm = $t.p.colModel;
    for (var i = 0; i < cm.length; i++) {
        if (cm[i].editable === true && svr.hasOwnProperty(cm[i].name)) {
            ares[cm[i].name] = svr[cm[i].name];
        }
    }
    $($t).jqGrid("setRowData", rowid, ares);
    ind.endRowEdit();
    $(ind).unbind("keydown");
    if ($("#" + $.jgrid.jqID(rowid), "#" + $.jgrid.jqID($t.p.id)).hasClass("jqgrid-new-row")) {
        setTimeout(function () {
            if ($t.p.subGrid) {
                var $subgrid = $(ind.nextSibling);
                if ($subgrid.hasClass("ui-subgrid"))
                    $subgrid.remove();
            }
            $($t).jqGrid("delRowData", rowid);
            $($t).jqGrid('showAddEditButtons');
        }, 0);
    }
    $($t).triggerHandler("jqGridInlineAfterRestoreRow", [rowid]);
    if ($.isFunction(o.afterrestorefunc)) {
        o.afterrestorefunc.call($t, rowid);
    }
};

$.jgrid.extend({
//Editing
	editRow : function(rowid,keys,oneditfunc,successfunc, url, extraparam, aftersavefunc,errorfunc, afterrestorefunc) {
	    return this.each(function () {
		    var $t = this;
		    if (!$t.grid) { return; }
		    var ind = $($t).jqGrid("getInd", rowid, true);
		    if (ind === false) { return; }

		    var o = {}
		    if ($.type(keys) === "object") {
		        o = keys;
		    } else {
		        if (keys !== undefined) { o.keys = keys; }
		        if ($.isFunction(oneditfunc)) { o.oneditfunc = oneditfunc; }
		        if ($.isFunction(successfunc)) { o.successfunc = successfunc; }
		        if (url !== undefined) { o.url = url; }
		        if (extraparam !== undefined) { o.extraparam = extraparam; }
		        if ($.isFunction(aftersavefunc)) { o.aftersavefunc = aftersavefunc; }
		        if ($.isFunction(errorfunc)) { o.errorfunc = errorfunc; }
		        if ($.isFunction(afterrestorefunc)) { o.afterrestorefunc = afterrestorefunc; }
		        // last two not as param, but as object (sorry)
		        //if (restoreAfterError !== undefined) { o.restoreAfterError = restoreAfterError; }
		        //if (mtype !== undefined) { o.mtype = mtype || "POST"; }			
		    }
		    o = $.extend(true, {
		        keys: false,
		        oneditfunc: null,
		        successfunc: null,
		        url: null,
		        extraparam: {},
		        aftersavefunc: null,
		        errorfunc: null,
		        afterrestorefunc: null,
		        restoreAfterError: true,
		        mtype: "POST"
		    }, $.jgrid.inlineEdit, $t.p.editParams, o);
		    o.url = o.url || $t.p.editurl;
		    editRow($t, ind, rowid, o)
		});
	},
	saveRow: function (rowid) {
	    var $t = this[0];
		if (!$t.grid ) { return success; }
		var ind = $($t).jqGrid("getInd",rowid,true);
		if (ind === false) { return false; }
		if ($.isFunction(ind.saveRow))
		    return ind.saveRow();
		return false;
	},
	restoreRow : function(rowid) {
	    return this.each(function () {
	        var $t = this
	        if (!$t.grid) { return; }
	        var ind = $($t).jqGrid("getInd", rowid, true);
	        if (ind === false) { return; }
	        if ($.isFunction(ind.restoreRow))
	            ind.restoreRow();
	    });
	},
	deleteRow : function(rowid,keys,onedeletefunc,successfunc, url, extraparam, afterdeletefunc,errorfunc,aftercanceldeletefunc) {
	    return this.each(function () {
	        var $t = this;
	        if (!$t.grid) { return; }
	        var ind = $($t).jqGrid("getInd", rowid, true);
	        if (ind === false) { return; }

	        var o = {}
	        if ($.type(keys) === "object") {
	            o = keys;
	        } else {
	            if (keys !== undefined) { o.keys = keys; }
	            if ($.isFunction(onedeletefunc)) { o.onedeletefunc = onedeletefunc; }
	            if ($.isFunction(successfunc)) { o.successfunc = successfunc; }
	            if (url !== undefined) { o.url = url; }
	            if (extraparam !== undefined) { o.extraparam = extraparam; }
	            if ($.isFunction(afterdeletefunc)) { o.afterdeletefunc = afterdeletefunc; }
	            if ($.isFunction(errorfunc)) { o.errorfunc = errorfunc; }
	            if ($.isFunction(aftercanceldeletefunc)) { o.aftercanceldeletefunc = aftercanceldeletefunc; }
	            // last two not as param, but as object (sorry)
	            //if (restoreAfterError !== undefined) { o.restoreAfterError = restoreAfterError; }
	            //if (mtype !== undefined) { o.mtype = mtype || "POST"; }			
	        }
	        o = $.extend(true, {
	            keys: false,
	            onedeletefunc: null,
	            successfunc: null,
	            url: null,
	            extraparam: {},
	            afterdeletefunc: null,
	            errorfunc: null,
	            restoreAfterError: true,
	            aftercanceldeletefunc: null,
	            mtype: "POST"
	        }, $.jgrid.inlineEdit, $t.p.editParams, o);
	        o.url = o.url || $t.p.editurl;
	        var opers = $t.p.prmNames,
            oper = opers.oper;
	        o.extraparam[oper] = opers.deloper;
	        deleteRow($t, ind, rowid, o)
	    });
	},
	addRow : function ( p ) {
		p = $.extend(true, {
			rowID : null,
			initdata : {},
			position :"first",
			useDefValues : true,
			useFormatter : false,
			addRowParams : {extraparam:{}}
		},p  || {});
		return this.each(function(){
			if (!this.grid ) { return; }
			var $t = this;
			var bfar = $.isFunction( p.beforeAddRow ) ?	p.beforeAddRow.call($t,p.addRowParams) :  undefined;
			if( bfar === undefined ) {
				bfar = true;
			}
			if(!bfar) { return; }
			p.rowID = $.isFunction(p.rowID) ? p.rowID.call($t, p) : ( (p.rowID != null) ? p.rowID : $.jgrid.randId());
			if(p.useDefValues === true) {
				$($t.p.colModel).each(function(){
					if( this.editoptions && this.editoptions.defaultValue ) {
						var opt = this.editoptions.defaultValue,
						tmp1 = $.isFunction(opt) ? opt.call($t) : opt;
						p.initdata[this.name] = tmp1;
					}
				});
			}
			$($t).jqGrid('addRowData', p.rowID, p.initdata, p.position);
			p.rowID = $t.p.idPrefix + p.rowID;
			$("#"+$.jgrid.jqID(p.rowID), "#"+$.jgrid.jqID($t.p.id)).addClass("jqgrid-new-row");
			if(p.useFormatter) {
				$("#"+$.jgrid.jqID(p.rowID)+" .ui-inline-edit", "#"+$.jgrid.jqID($t.p.id)).click();
			} else {
				var opers = $t.p.prmNames,
				oper = opers.oper;
				p.addRowParams.extraparam[oper] = opers.addoper;
				$($t).jqGrid('editRow', p.rowID, p.addRowParams);
				$($t).jqGrid('setSelection', p.rowID);
			}
		});
	},
	inlineNav : function (elem, o) {
		o = $.extend(true,{
			edit: true,
			editicon: "ui-icon-pencil",
			add: true,
			addicon:"ui-icon-plus",
			save: true,
			saveicon:"ui-icon-disk",
			cancel: true,
			cancelicon:"ui-icon-cancel",
			addParams : {addRowParams: {extraparam: {}}},
			editParams : {},
			restoreAfterSelect : true
		}, $.jgrid.nav, o ||{});
		return this.each(function(){
			if (!this.grid ) { return; }
			var $t = this, onSelect, gID = $.jgrid.jqID($t.p.id);
			$t.p._inlinenav = true;
			// detect the formatactions column
			if(o.addParams.useFormatter === true) {
				var cm = $t.p.colModel,i;
				for (i = 0; i<cm.length; i++) {
					if(cm[i].formatter && cm[i].formatter === "actions" ) {
						if(cm[i].formatoptions) {
							var defaults =  {
								keys:false,
								onEdit : null,
								onSuccess: null,
								afterSave:null,
								onError: null,
								afterRestore: null,
								extraparam: {},
								url: null
							},
							ap = $.extend( defaults, cm[i].formatoptions );
							o.addParams.addRowParams = {
								"keys" : ap.keys,
								"oneditfunc" : ap.onEdit,
								"successfunc" : ap.onSuccess,
								"url" : ap.url,
								"extraparam" : ap.extraparam,
								"aftersavefunc" : ap.afterSave,
								"errorfunc": ap.onError,
								"afterrestorefunc" : ap.afterRestore
							};
						}
						break;
					}
				}
			}
			if(o.add) {
				$($t).jqGrid('navButtonAdd', elem,{
					caption : o.addtext,
					title : o.addtitle,
					buttonicon : o.addicon,
					id : $t.p.id+"_iladd",
					onClickButton : function () {
						$($t).jqGrid('addRow', o.addParams);
						if(!o.addParams.useFormatter) {
							$("#"+gID+"_ilsave").removeClass('ui-state-disabled');
							$("#"+gID+"_ilcancel").removeClass('ui-state-disabled');
							$("#"+gID+"_iladd").addClass('ui-state-disabled');
							$("#"+gID+"_iledit").addClass('ui-state-disabled');
						}
					}
				});
			}
			if(o.edit) {
				$($t).jqGrid('navButtonAdd', elem,{
					caption : o.edittext,
					title : o.edittitle,
					buttonicon : o.editicon,
					id : $t.p.id+"_iledit",
					onClickButton : function () {
						var sr = $($t).jqGrid('getGridParam','selrow');
						if(sr) {
							$($t).jqGrid('editRow', sr, o.editParams);
							$("#"+gID+"_ilsave").removeClass('ui-state-disabled');
							$("#"+gID+"_ilcancel").removeClass('ui-state-disabled');
							$("#"+gID+"_iladd").addClass('ui-state-disabled');
							$("#"+gID+"_iledit").addClass('ui-state-disabled');
						} else {
							$.jgrid.viewModal("#alertmod",{gbox:"#gbox_"+gID,jqm:true});$("#jqg_alrt").focus();							
						}
					}
				});
			}
			if(o.save) {
				$($t).jqGrid('navButtonAdd', elem,{
					caption : o.savetext || '',
					title : o.savetitle || 'Save row',
					buttonicon : o.saveicon,
					id : $t.p.id+"_ilsave",
					onClickButton : function () {
						var sr = $t.p.savedRow[0].id;
						if(sr) {
							var opers = $t.p.prmNames,
							oper = opers.oper, tmpParams = {};
							if($("#"+$.jgrid.jqID(sr), "#"+gID ).hasClass("jqgrid-new-row")) {
								o.addParams.addRowParams.extraparam[oper] = opers.addoper;
								tmpParams = o.addParams.addRowParams;
							} else {
								if(!o.editParams.extraparam) {
									o.editParams.extraparam = {};
								}
								o.editParams.extraparam[oper] = opers.editoper;
								tmpParams = o.editParams;
							}
							if( $($t).jqGrid('saveRow', sr, tmpParams) ) {
								$($t).jqGrid('showAddEditButtons');
							}
						} else {
							$.jgrid.viewModal("#alertmod",{gbox:"#gbox_"+gID,jqm:true});$("#jqg_alrt").focus();							
						}
					}
				});
				$("#"+gID+"_ilsave").addClass('ui-state-disabled');
			}
			if(o.cancel) {
				$($t).jqGrid('navButtonAdd', elem,{
					caption : o.canceltext || '',
					title : o.canceltitle || 'Cancel row editing',
					buttonicon : o.cancelicon,
					id : $t.p.id+"_ilcancel",
					onClickButton : function () {
						var sr = $t.p.savedRow[0].id, cancelPrm = {};
						if(sr) {
							if($("#"+$.jgrid.jqID(sr), "#"+gID ).hasClass("jqgrid-new-row")) {
								cancelPrm = o.addParams.addRowParams;
							} else {
								cancelPrm = o.editParams;
							}
							$($t).jqGrid('restoreRow', sr, cancelPrm);
							$($t).jqGrid('showAddEditButtons');
						} else {
							$.jgrid.viewModal("#alertmod",{gbox:"#gbox_"+gID,jqm:true});$("#jqg_alrt").focus();							
						}
					}
				});
				$("#"+gID+"_ilcancel").addClass('ui-state-disabled');
			}
			if(o.restoreAfterSelect === true) {
				if($.isFunction($t.p.beforeSelectRow)) {
					onSelect = $t.p.beforeSelectRow;
				} else {
					onSelect =  false;
				}
				$t.p.beforeSelectRow = function(id, stat) {
					var ret = true;
					if($t.p.savedRow.length > 0 && $t.p._inlinenav===true && ( id !== $t.p.selrow && $t.p.selrow !==null) ) {
						if($t.p.selrow === o.addParams.rowID ) {
							$($t).jqGrid('delRowData', $t.p.selrow);
						} else {
							$($t).jqGrid('restoreRow', $t.p.selrow, o.editParams);
						}
						$($t).jqGrid('showAddEditButtons');
					}
					if(onSelect) {
						ret = onSelect.call($t, id, stat);
					}
					return ret;
				};
			}

		});
	},
	showAddEditButtons : function()  {
		return this.each(function(){
			if (!this.grid ) { return; }
			var gID = $.jgrid.jqID(this.p.id);
			$("#"+gID+"_ilsave").addClass('ui-state-disabled');
			$("#"+gID+"_ilcancel").addClass('ui-state-disabled');
			$("#"+gID+"_iladd").removeClass('ui-state-disabled');
			$("#"+gID+"_iledit").removeClass('ui-state-disabled');
		});
	}
//end inline edit
});
})(jQuery);

