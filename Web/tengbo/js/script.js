//Float.js
$.fn.Float = function (obj) { var that = this; var lock = { topSide: 150, floatRight: 1, side: 5, init: function () { var el = that, ua = navigator.userAgent; el.css({ "position": "absolute", "z-index": "1000", "top": this.topSide }); if (ua.toLowerCase().indexOf("360se") > -1) { this.isBlock = true } else { if (ua.toLowerCase().indexOf("theworld") > 0) { this.isBlock = true } else { if (ua.toLowerCase().indexOf("msie 7") > 0) { this.side = -1 } } } this.floatRight == 1 ? el.css("right", this.side) : el.css("left", this.side); var locker = this; setInterval(function () { locker.lock.call(locker) }, 20); if (this.close != undefined) { this.closeFloat() } if (this.floatRight == 1) { $(window).resize(function () { $(this).scrollLeft(0); el.css("right", 5) }) } }, lockTop: function (el, position, page, scroll, icon) { var top = el.css("top"); var y = scroll.top + icon.topSide, offsetTop = (y - parseInt(top)) / 20; if (this.isBlock) { offsetTop = (y - parseInt(top)) } var topSide = parseInt(top) + offsetTop; if ((topSide + position.height) < page.height) { el.css("top", topSide) } }, lockLeft: function (el, scroll, icon) { var left = el.css("left"); var x = scroll.left + icon.side, offsetLeft = (x - parseInt(left)) / 20; el.css("left", parseInt(left) + offsetLeft) }, lockRight: function (el, scroll, icon) { var right = el.css("right"); var d = document; if (scroll.left == 0) { var x = icon.side, offsetRight = (Math.abs(x) - Math.abs(parseInt(right))) / 20; el.css("right", Math.abs(parseInt(right)) + offsetRight) } else { var x = scroll.left - icon.side, offsetRight = (Math.abs(x) - Math.abs(parseInt(right))) / 20; right = -(Math.abs(parseInt(right)) + offsetRight) + "px"; el.css("right", right) } }, lock: function () { var el = that, position = this.currentPosition(el), win = this.windowDimension(), scroll = this.scrollPosition(), page = this.pageDimension(), icon = this; this.lockTop(el, position, page, scroll, icon); if (this.floatRight == 1) { this.lockRight(el, scroll, icon) } else { this.lockLeft(el, scroll, icon) } if (this.isBlock) { if (this.lastTop != el.css("top")) { el.css("visibility", "hidden"); this.lastTop = el.css("top") } else { el.css("visibility", "visible") } } }, currentPosition: function (el) { var offset = el.offset(); return { top: offset.top, left: offset.left, width: el.outerWidth(), height: el.outerHeight() } }, windowDimension: function () { if ((typeof innerWidth != "undefined" && innerWidth != 0) && (typeof innerHeight != "undefined" && innerHeight != 0)) { return { width: innerWidth, height: innerHeight } } var d = document; return { width: Math.min(d.body.clientWidth, d.documentElement.clientWidth), height: Math.min(d.body.clientHeight, d.documentElement.clientHeight) } }, scrollPosition: function () { var d = document; return { top: Math.max(d.body.scrollTop, d.documentElement.scrollTop), left: Math.max(d.body.scrollLeft, d.documentElement.scrollLeft) } }, pageDimension: function () { var d = document; return { width: Math.max(d.body.scrollWidth, d.documentElement.scrollWidth), height: Math.max(d.body.scrollHeight, d.documentElement.scrollHeight) } }, closeFloat: function () { that.find("#" + this.close).click(function () { that.hide() }).css("cursor", "pointer") } }; if (obj) { $.extend(lock, obj) } lock.init() };
// json2.js
var JSON; JSON || (JSON = {}), (function () { "use strict"; function i(n) { return n < 10 ? "0" + n : n } function f(n) { return o.lastIndex = 0, o.test(n) ? '"' + n.replace(o, function (n) { var t = s[n]; return typeof t == "string" ? t : "\\u" + ("0000" + n.charCodeAt(0).toString(16)).slice(-4) }) + '"' : '"' + n + '"' } function r(i, e) { var h, l, c, a, v = n, s, o = e[i]; o && typeof o == "object" && typeof o.toJSON == "function" && (o = o.toJSON(i)), typeof t == "function" && (o = t.call(e, i, o)); switch (typeof o) { case "string": return f(o); case "number": return isFinite(o) ? String(o) : "null"; case "boolean": case "null": return String(o); case "object": if (!o) return "null"; n += u, s = []; if (Object.prototype.toString.apply(o) === "[object Array]") { for (a = o.length, h = 0; h < a; h += 1) s[h] = r(h, o) || "null"; return c = s.length === 0 ? "[]" : n ? "[\n" + n + s.join(",\n" + n) + "\n" + v + "]" : "[" + s.join(",") + "]", n = v, c } if (t && typeof t == "object") for (a = t.length, h = 0; h < a; h += 1) typeof t[h] == "string" && (l = t[h], c = r(l, o), c && s.push(f(l) + (n ? ": " : ":") + c)); else for (l in o) Object.prototype.hasOwnProperty.call(o, l) && (c = r(l, o), c && s.push(f(l) + (n ? ": " : ":") + c)); return c = s.length === 0 ? "{}" : n ? "{\n" + n + s.join(",\n" + n) + "\n" + v + "}" : "{" + s.join(",") + "}", n = v, c } } typeof Date.prototype.toJSON != "function" && (Date.prototype.toJSON = function () { return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + i(this.getUTCMonth() + 1) + "-" + i(this.getUTCDate()) + "T" + i(this.getUTCHours()) + ":" + i(this.getUTCMinutes()) + ":" + i(this.getUTCSeconds()) + "Z" : null }, String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function () { return this.valueOf() }); var e = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, o = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, n, u, s = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, t; typeof JSON.stringify != "function" && (JSON.stringify = function (i, f, e) { var o; n = "", u = ""; if (typeof e == "number") for (o = 0; o < e; o += 1) u += " "; else typeof e == "string" && (u = e); t = f; if (f && typeof f != "function" && (typeof f != "object" || typeof f.length != "number")) throw new Error("JSON.stringify"); return r("", { "": i }) }), typeof JSON.parse != "function" && (JSON.parse = function (n, t) { function r(n, i) { var f, e, u = n[i]; if (u && typeof u == "object") for (f in u) Object.prototype.hasOwnProperty.call(u, f) && (e = r(u, f), e !== undefined ? u[f] = e : delete u[f]); return t.call(n, i, u) } var i; n = String(n), e.lastIndex = 0, e.test(n) && (n = n.replace(e, function (n) { return "\\u" + ("0000" + n.charCodeAt(0).toString(16)).slice(-4) })); if (/^[\],:{}\s]*$/.test(n.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) return i = eval("(" + n + ")"), typeof t == "function" ? r({ "": i }, "") : i; throw new SyntaxError("JSON.parse"); }) })();
// LoadBox.js
$.fn.centerY = function () { var y = (($(window).innerHeight() - this.outerHeight()) / 2); if (y < 0) { y = 0 } this.css({ top: y }); return this };
$.fn.loadBox = function (width, height, fUrl, path) { var initialHtml = $('<div class="iframe_container" style="text-align: center; display: none; position: relative; top: 275px; width: 100px; height: 100px; margin: 0 auto;"><div style="position: absolute; top: -15px; right: -15px; width: 30px; height: 30px; background-image:url(\'' + path + './image/idialog_s.png\');background-position:0 -60px;" onclick="$.unblockUI()" onmouseover="$(this).css({\'background-position\':\'0 -94px\', \'cursor\': \'pointer\'});" onmouseout="$(this).css({\'background-position\':\'0 -60px\'});"></div><iframe style="border: none; width: 100%; height: 100%; margin: 0; padding: 0; overflow: hidden;" scrolling="no"></iframe></div>'); if ($("body").find(".iframe_container").length == 0) { $("body").append(initialHtml) } $.blockUI({ message: '<div class="div_LoadBox" style="width:100px;height:100px;margin:0 auto;position:relative;border:3px solid yellow;background:url(\'' + path + './image/loading.gif\') no-repeat center center white;"></div>', overlayCSS: { backgroundColor: "#000", opacity: 0.4 } }); $(".blockUI.blockMsg.blockPage").css({ left: 0, top: 0, width: "100%", height: "100%" }); var p = $(".div_LoadBox").parent(); var n = $(".iframe_container").clone(true, true); n.appendTo(p); $(".div_LoadBox").centerY(); p.find("iframe").load(function () { p.find(".div_LoadBox").hide(); var y = (($(window).innerHeight() - height) / 2); if (y < 0) { y = 0 } p.find(".iframe_container").show().centerY().animate({ width: width, height: height, top: y }, "slow", null, function () { $(window).resize(function () { p.find(".iframe_container").centerY() }).trigger("resize") }) }); p.find("iframe").attr("src", fUrl); p.click(function () { $.unblockUI() }) };
(function () { "use strict"; function n(n) { function s(s, h) { var rt, ut, p = s == window, l = h && h.message !== undefined ? h.message : undefined, g, k, d, tt, nt, w, b, it, ft, et, at; if (h = n.extend({}, n.blockUI.defaults, h || {}), !h.ignoreIfBlocked || !n(s).data("blockUI.isBlocked")) { if (h.overlayCSS = n.extend({}, n.blockUI.defaults.overlayCSS, h.overlayCSS || {}), rt = n.extend({}, n.blockUI.defaults.css, h.css || {}), h.onOverlayClick && (h.overlayCSS.cursor = "pointer"), ut = n.extend({}, n.blockUI.defaults.themedCSS, h.themedCSS || {}), l = l === undefined ? h.message : l, p && t && e(window, { fadeOut: 0 }), l && typeof l != "string" && (l.parentNode || l.jquery) && (g = l.jquery ? l[0] : l, k = {}, n(s).data("blockUI.history", k), k.el = g, k.parent = g.parentNode, k.display = g.style.display, k.position = g.style.position, k.parent && k.parent.removeChild(g)), n(s).data("blockUI.onUnblock", h.onUnblock), d = h.baseZ, tt = f || h.forceIframe ? n('<iframe class="blockUI" style="z-index:' + d++ + ';display:none;border:none;margin:0;padding:0;position:absolute;width:100%;height:100%;top:0;left:0" src="' + h.iframeSrc + '"><\/iframe>') : n('<div class="blockUI" style="display:none"><\/div>'), nt = h.theme ? n('<div class="blockUI blockOverlay ui-widget-overlay" style="z-index:' + d++ + ';display:none"><\/div>') : n('<div class="blockUI blockOverlay" style="z-index:' + d++ + ';display:none;border:none;margin:0;padding:0;width:100%;height:100%;top:0;left:0"><\/div>'), h.theme && p ? (b = '<div class="blockUI ' + h.blockMsgClass + ' blockPage ui-dialog ui-widget ui-corner-all" style="z-index:' + (d + 10) + ';display:none;position:fixed">', h.title && (b += '<div class="ui-widget-header ui-dialog-titlebar ui-corner-all blockTitle">' + (h.title || "&nbsp;") + "<\/div>"), b += '<div class="ui-widget-content ui-dialog-content"><\/div>', b += "<\/div>") : h.theme ? (b = '<div class="blockUI ' + h.blockMsgClass + ' blockElement ui-dialog ui-widget ui-corner-all" style="z-index:' + (d + 10) + ';display:none;position:absolute">', h.title && (b += '<div class="ui-widget-header ui-dialog-titlebar ui-corner-all blockTitle">' + (h.title || "&nbsp;") + "<\/div>"), b += '<div class="ui-widget-content ui-dialog-content"><\/div>', b += "<\/div>") : b = p ? '<div class="blockUI ' + h.blockMsgClass + ' blockPage" style="z-index:' + (d + 10) + ';display:none;position:fixed"><\/div>' : '<div class="blockUI ' + h.blockMsgClass + ' blockElement" style="z-index:' + (d + 10) + ';display:none;position:absolute"><\/div>', w = n(b), l && (h.theme ? (w.css(ut), w.addClass("ui-widget-content")) : w.css(rt)), h.theme || nt.css(h.overlayCSS), nt.css("position", p ? "fixed" : "absolute"), (f || h.forceIframe) && tt.css("opacity", 0), it = [tt, nt, w], ft = p ? n("body") : n(s), n.each(it, function () { this.appendTo(ft) }), h.theme && h.draggable && n.fn.draggable && w.draggable({ handle: ".ui-dialog-titlebar", cancel: "li" }), et = v && (!n.support.boxModel || n("object,embed", p ? null : s).length > 0), o || et) { if (p && h.allowBodyStretch && n.support.boxModel && n("html,body").css("height", "100%"), (o || !n.support.boxModel) && !p) var ot = r(s, "borderTopWidth"), st = r(s, "borderLeftWidth"), ht = ot ? "(0 - " + ot + ")" : 0, ct = st ? "(0 - " + st + ")" : 0; n.each(it, function (n, t) { var i = t[0].style, r, u; i.position = "absolute", n < 2 ? (p ? i.setExpression("height", "Math.max(document.body.scrollHeight, document.body.offsetHeight) - (jQuery.support.boxModel?0:" + h.quirksmodeOffsetHack + ') + "px"') : i.setExpression("height", 'this.parentNode.offsetHeight + "px"'), p ? i.setExpression("width", 'jQuery.support.boxModel && document.documentElement.clientWidth || document.body.clientWidth + "px"') : i.setExpression("width", 'this.parentNode.offsetWidth + "px"'), ct && i.setExpression("left", ct), ht && i.setExpression("top", ht)) : h.centerY ? (p && i.setExpression("top", '(document.documentElement.clientHeight || document.body.clientHeight) / 2 - (this.offsetHeight / 2) + (blah = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop) + "px"'), i.marginTop = 0) : !h.centerY && p && (r = h.css && h.css.top ? parseInt(h.css.top, 10) : 0, u = "((document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop) + " + r + ') + "px"', i.setExpression("top", u)) }) } if (l && (h.theme ? w.find(".ui-widget-content").append(l) : w.append(l), (l.jquery || l.nodeType) && n(l).show()), (f || h.forceIframe) && h.showOverlay && tt.show(), h.fadeIn) { var lt = h.onBlock ? h.onBlock : u, vt = h.showOverlay && !l ? lt : u, yt = l ? lt : u; h.showOverlay && nt._fadeIn(h.fadeIn, vt), l && w._fadeIn(h.fadeIn, yt) } else h.showOverlay && nt.show(), l && w.show(), h.onBlock && h.onBlock(); c(1, s, h), p ? (t = w[0], i = n(h.focusableElements, t), h.focusInput && setTimeout(a, 20)) : y(w[0], h.centerX, h.centerY), h.timeout && (at = setTimeout(function () { p ? n.unblockUI(h) : n(s).unblock(h) }, h.timeout), n(s).data("blockUI.timeout", at)) } } function e(r, u) { var o, s = r == window, e = n(r), l = e.data("blockUI.history"), a = e.data("blockUI.timeout"), f; a && (clearTimeout(a), e.removeData("blockUI.timeout")), u = n.extend({}, n.blockUI.defaults, u || {}), c(0, r, u), u.onUnblock === null && (u.onUnblock = e.data("blockUI.onUnblock"), e.removeData("blockUI.onUnblock")), f = s ? n("body").children().filter(".blockUI").add("body > .blockUI") : e.find(">.blockUI"), u.cursorReset && (f.length > 1 && (f[1].style.cursor = u.cursorReset), f.length > 2 && (f[2].style.cursor = u.cursorReset)), s && (t = i = null), u.fadeOut ? (o = f.length, f.stop().fadeOut(u.fadeOut, function () { --o == 0 && h(f, l, u, r) })) : h(f, l, u, r) } function h(t, i, r, u) { var f = n(u); if (!f.data("blockUI.isBlocked")) { if (t.each(function () { this.parentNode && this.parentNode.removeChild(this) }), i && i.el && (i.el.style.display = i.display, i.el.style.position = i.position, i.parent && i.parent.appendChild(i.el), f.removeData("blockUI.history")), f.data("blockUI.static") && f.css("position", "static"), typeof r.onUnblock == "function") r.onUnblock(u, r); var e = n(document.body), o = e.width(), s = e[0].style.width; e.width(o - 1).width(o), e[0].style.width = s } } function c(i, r, u) { var f = r == window, o = n(r), e; (i || (!f || t) && (f || o.data("blockUI.isBlocked"))) && (o.data("blockUI.isBlocked", i), f && u.bindEvents && (!i || u.showOverlay)) && (e = "mousedown mouseup keydown keypress keyup touchstart touchend touchmove", i ? n(document).bind(e, u, l) : n(document).unbind(e, l)) } function l(r) { var u, f; if (r.type === "keydown" && r.keyCode && r.keyCode == 9 && t && r.data.constrainTabKey) { var e = i, s = !r.shiftKey && r.target === e[e.length - 1], o = r.shiftKey && r.target === e[0]; if (s || o) return setTimeout(function () { a(o) }, 10), !1 } if (u = r.data, f = n(r.target), f.hasClass("blockOverlay") && u.onOverlayClick) u.onOverlayClick(r); return f.parents("div." + u.blockMsgClass).length > 0 ? !0 : f.parents().children().filter("div.blockUI").length === 0 } function a(n) { if (i) { var t = i[n === !0 ? i.length - 1 : 0]; t && t.focus() } } function y(n, t, i) { var u = n.parentNode, f = n.style, e = (u.offsetWidth - n.offsetWidth) / 2 - r(u, "borderLeftWidth"), o = (u.offsetHeight - n.offsetHeight) / 2 - r(u, "borderTopWidth"); t && (f.left = e > 0 ? e + "px" : "0"), i && (f.top = o > 0 ? o + "px" : "0") } function r(t, i) { return parseInt(n.css(t, i), 10) || 0 } var t, i; n.fn._fadeIn = n.fn.fadeIn; var u = n.noop || function () { }, f = /MSIE/.test(navigator.userAgent), o = /MSIE 6.0/.test(navigator.userAgent) && !/MSIE 8.0/.test(navigator.userAgent), p = document.documentMode || 0, v = n.isFunction(document.createElement("div").style.setExpression); n.blockUI = function (n) { s(window, n) }, n.unblockUI = function (n) { e(window, n) }, n.growlUI = function (t, i, r, u) { var f = n('<div class="growlUI"><\/div>'), e, o; t && f.append("<h1>" + t + "<\/h1>"), i && f.append("<h2>" + i + "<\/h2>"), r === undefined && (r = 3e3), e = function (t) { t = t || {}, n.blockUI({ message: f, fadeIn: typeof t.fadeIn != "undefined" ? t.fadeIn : 700, fadeOut: typeof t.fadeOut != "undefined" ? t.fadeOut : 1e3, timeout: typeof t.timeout != "undefined" ? t.timeout : r, centerY: !1, showOverlay: !1, onUnblock: u, css: n.blockUI.defaults.growlCSS }) }, e(), o = f.css("opacity"), f.mouseover(function () { e({ fadeIn: 0, timeout: 3e4 }); var t = n(".blockMsg"); t.stop(), t.fadeTo(300, 1) }).mouseout(function () { n(".blockMsg").fadeOut(1e3) }) }, n.fn.block = function (t) { if (this[0] === window) return n.blockUI(t), this; var i = n.extend({}, n.blockUI.defaults, t || {}); return this.each(function () { var t = n(this); i.ignoreIfBlocked && t.data("blockUI.isBlocked") || t.unblock({ fadeOut: 0 }) }), this.each(function () { n.css(this, "position") == "static" && (this.style.position = "relative", n(this).data("blockUI.static", !0)), this.style.zoom = 1, s(this, t) }) }, n.fn.unblock = function (t) { return this[0] === window ? (n.unblockUI(t), this) : this.each(function () { e(this, t) }) }, n.blockUI.version = 2.66, n.blockUI.defaults = { message: "<h1>Please wait...<\/h1>", title: null, draggable: !0, theme: !1, css: { padding: 0, margin: 0, width: "30%", top: "40%", left: "35%", textAlign: "center", color: "#000", border: "3px solid #aaa", backgroundColor: "#fff", cursor: "wait" }, themedCSS: { width: "30%", top: "40%", left: "35%" }, overlayCSS: { backgroundColor: "#000", opacity: .6, cursor: "wait" }, cursorReset: "default", growlCSS: { width: "350px", top: "10px", left: "", right: "10px", border: "none", padding: "5px", opacity: .6, cursor: "default", color: "#fff", backgroundColor: "#000", "-webkit-border-radius": "10px", "-moz-border-radius": "10px", "border-radius": "10px" }, iframeSrc: /^https/i.test(window.location.href || "") ? "javascript:false" : "about:blank", forceIframe: !1, baseZ: 1e3, centerX: !0, centerY: !0, allowBodyStretch: !0, bindEvents: !0, constrainTabKey: !0, fadeIn: 200, fadeOut: 400, timeout: 0, showOverlay: !0, focusInput: !0, focusableElements: ":input:enabled:visible", onBlock: null, onUnblock: null, onOverlayClick: null, quirksmodeOffsetHack: 4, blockMsgClass: "blockMsg", ignoreIfBlocked: !1 }, t = null, i = [] } typeof define == "function" && define.amd && define.amd.jQuery ? define(["jquery"], n) : n(jQuery) })();

(function () { // date.js
    Date.CultureInfo = { name: "en-US", englishName: "English (United States)", nativeName: "English (United States)", dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], abbreviatedDayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], shortestDayNames: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"], firstLetterDayNames: ["S", "M", "T", "W", "T", "F", "S"], monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], abbreviatedMonthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], amDesignator: "AM", pmDesignator: "PM", firstDayOfWeek: 0, twoDigitYearMax: 2029, dateElementOrder: "mdy", formatPatterns: { shortDate: "M/d/yyyy", longDate: "dddd, MMMM dd, yyyy", shortTime: "h:mm tt", longTime: "h:mm:ss tt", fullDateTime: "dddd, MMMM dd, yyyy h:mm:ss tt", sortableDateTime: "yyyy-MM-ddTHH:mm:ss", universalSortableDateTime: "yyyy-MM-dd HH:mm:ssZ", rfc1123: "ddd, dd MMM yyyy HH:mm:ss GMT", monthDay: "MMMM dd", yearMonth: "MMMM, yyyy" }, regexPatterns: { jan: /^jan(uary)?/i, feb: /^feb(ruary)?/i, mar: /^mar(ch)?/i, apr: /^apr(il)?/i, may: /^may/i, jun: /^jun(e)?/i, jul: /^jul(y)?/i, aug: /^aug(ust)?/i, sep: /^sep(t(ember)?)?/i, oct: /^oct(ober)?/i, nov: /^nov(ember)?/i, dec: /^dec(ember)?/i, sun: /^su(n(day)?)?/i, mon: /^mo(n(day)?)?/i, tue: /^tu(e(s(day)?)?)?/i, wed: /^we(d(nesday)?)?/i, thu: /^th(u(r(s(day)?)?)?)?/i, fri: /^fr(i(day)?)?/i, sat: /^sa(t(urday)?)?/i, future: /^next/i, past: /^last|past|prev(ious)?/i, add: /^(\+|after|from)/i, subtract: /^(\-|before|ago)/i, yesterday: /^yesterday/i, today: /^t(oday)?/i, tomorrow: /^tomorrow/i, now: /^n(ow)?/i, millisecond: /^ms|milli(second)?s?/i, second: /^sec(ond)?s?/i, minute: /^min(ute)?s?/i, hour: /^h(ou)?rs?/i, week: /^w(ee)?k/i, month: /^m(o(nth)?s?)?/i, day: /^d(ays?)?/i, year: /^y((ea)?rs?)?/i, shortMeridian: /^(a|p)/i, longMeridian: /^(a\.?m?\.?|p\.?m?\.?)/i, timezone: /^((e(s|d)t|c(s|d)t|m(s|d)t|p(s|d)t)|((gmt)?\s*(\+|\-)\s*\d\d\d\d?)|gmt)/i, ordinalSuffix: /^\s*(st|nd|rd|th)/i, timeContext: /^\s*(\:|a|p)/i }, abbreviatedTimeZoneStandard: { GMT: "-000", EST: "-0400", CST: "-0500", MST: "-0600", PST: "-0700" }, abbreviatedTimeZoneDST: { GMT: "-000", EDT: "-0500", CDT: "-0600", MDT: "-0700", PDT: "-0800" } }
    Date.getMonthNumberFromName = function (n) { for (var i = Date.CultureInfo.monthNames, u = Date.CultureInfo.abbreviatedMonthNames, r = n.toLowerCase(), t = 0; t < i.length; t++) if (i[t].toLowerCase() == r || u[t].toLowerCase() == r) return t; return -1 }, Date.getDayNumberFromName = function (n) { for (var i = Date.CultureInfo.dayNames, u = Date.CultureInfo.abbreviatedDayNames, f = Date.CultureInfo.shortestDayNames, r = n.toLowerCase(), t = 0; t < i.length; t++) if (i[t].toLowerCase() == r || u[t].toLowerCase() == r) return t; return -1 }, Date.isLeapYear = function (n) { return n % 4 == 0 && n % 100 != 0 || n % 400 == 0 }, Date.getDaysInMonth = function (n, t) { return [31, Date.isLeapYear(n) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][t] }, Date.getTimezoneOffset = function (n, t) { return t || !1 ? Date.CultureInfo.abbreviatedTimeZoneDST[n.toUpperCase()] : Date.CultureInfo.abbreviatedTimeZoneStandard[n.toUpperCase()] }, Date.getTimezoneAbbreviation = function (n, t) { var r = t || !1 ? Date.CultureInfo.abbreviatedTimeZoneDST : Date.CultureInfo.abbreviatedTimeZoneStandard, i; for (i in r) if (r[i] === n) return i; return null }, Date.prototype.clone = function () { return new Date(this.getTime()) }, Date.prototype.compareTo = function (n) { if (isNaN(this)) throw new Error(this); if (n instanceof Date && !isNaN(n)) return this > n ? 1 : this < n ? -1 : 0; throw new TypeError(n); }, Date.prototype.equals = function (n) { return this.compareTo(n) === 0 }, Date.prototype.between = function (n, t) { var i = this.getTime(); return i >= n.getTime() && i <= t.getTime() }, Date.prototype.addMilliseconds = function (n) { return this.setMilliseconds(this.getMilliseconds() + n), this }, Date.prototype.addSeconds = function (n) { return this.addMilliseconds(n * 1e3) }, Date.prototype.addMinutes = function (n) { return this.addMilliseconds(n * 6e4) }, Date.prototype.addHours = function (n) { return this.addMilliseconds(n * 36e5) }, Date.prototype.addDays = function (n) { return this.addMilliseconds(n * 864e5) }, Date.prototype.addWeeks = function (n) { return this.addMilliseconds(n * 6048e5) }, Date.prototype.addMonths = function (n) { var t = this.getDate(); return this.setDate(1), this.setMonth(this.getMonth() + n), this.setDate(Math.min(t, this.getDaysInMonth())), this }, Date.prototype.addYears = function (n) { return this.addMonths(n * 12) }, Date.prototype.add = function (n) { if (typeof n == "number") return this._orient = n, this; var t = n; return (t.millisecond || t.milliseconds) && this.addMilliseconds(t.millisecond || t.milliseconds), (t.second || t.seconds) && this.addSeconds(t.second || t.seconds), (t.minute || t.minutes) && this.addMinutes(t.minute || t.minutes), (t.hour || t.hours) && this.addHours(t.hour || t.hours), (t.month || t.months) && this.addMonths(t.month || t.months), (t.year || t.years) && this.addYears(t.year || t.years), (t.day || t.days) && this.addDays(t.day || t.days), this }, Date._validate = function (n, t, i, r) { if (typeof n != "number") throw new TypeError(n + " is not a Number."); else if (n < t || n > i) throw new RangeError(n + " is not a valid value for " + r + "."); return !0 }, Date.validateMillisecond = function (n) { return Date._validate(n, 0, 999, "milliseconds") }, Date.validateSecond = function (n) { return Date._validate(n, 0, 59, "seconds") }, Date.validateMinute = function (n) { return Date._validate(n, 0, 59, "minutes") }, Date.validateHour = function (n) { return Date._validate(n, 0, 23, "hours") }, Date.validateDay = function (n, t, i) { return Date._validate(n, 1, Date.getDaysInMonth(t, i), "days") }, Date.validateMonth = function (n) { return Date._validate(n, 0, 11, "months") }, Date.validateYear = function (n) { return Date._validate(n, 1, 9999, "seconds") }, Date.prototype.set = function (n) { var t = n; return t.millisecond || t.millisecond === 0 || (t.millisecond = -1), t.second || t.second === 0 || (t.second = -1), t.minute || t.minute === 0 || (t.minute = -1), t.hour || t.hour === 0 || (t.hour = -1), t.day || t.day === 0 || (t.day = -1), t.month || t.month === 0 || (t.month = -1), t.year || t.year === 0 || (t.year = -1), t.millisecond != -1 && Date.validateMillisecond(t.millisecond) && this.addMilliseconds(t.millisecond - this.getMilliseconds()), t.second != -1 && Date.validateSecond(t.second) && this.addSeconds(t.second - this.getSeconds()), t.minute != -1 && Date.validateMinute(t.minute) && this.addMinutes(t.minute - this.getMinutes()), t.hour != -1 && Date.validateHour(t.hour) && this.addHours(t.hour - this.getHours()), t.month !== -1 && Date.validateMonth(t.month) && this.addMonths(t.month - this.getMonth()), t.year != -1 && Date.validateYear(t.year) && this.addYears(t.year - this.getFullYear()), t.day != -1 && Date.validateDay(t.day, this.getFullYear(), this.getMonth()) && this.addDays(t.day - this.getDate()), t.timezone && this.setTimezone(t.timezone), t.timezoneOffset && this.setTimezoneOffset(t.timezoneOffset), this }, Date.prototype.clearTime = function () { return this.setHours(0), this.setMinutes(0), this.setSeconds(0), this.setMilliseconds(0), this }, Date.prototype.isLeapYear = function () { var n = this.getFullYear(); return n % 4 == 0 && n % 100 != 0 || n % 400 == 0 }, Date.prototype.isWeekday = function () { return !(this.is().sat() || this.is().sun()) }, Date.prototype.getDaysInMonth = function () { return Date.getDaysInMonth(this.getFullYear(), this.getMonth()) }, Date.prototype.moveToFirstDayOfMonth = function () { return this.set({ day: 1 }) }, Date.prototype.moveToLastDayOfMonth = function () { return this.set({ day: this.getDaysInMonth() }) }, Date.prototype.moveToDayOfWeek = function (n, t) { var i = (n - this.getDay() + 7 * (t || 1)) % 7; return this.addDays(i === 0 ? i += 7 * (t || 1) : i) }, Date.prototype.moveToMonth = function (n, t) { var i = (n - this.getMonth() + 12 * (t || 1)) % 12; return this.addMonths(i === 0 ? i += 12 * (t || 1) : i) }, Date.prototype.getDayOfYear = function () { return Math.floor((this - new Date(this.getFullYear(), 0, 1)) / 864e5) }, Date.prototype.getWeekOfYear = function (n) { var t = this.getFullYear(), e = this.getMonth(), o = this.getDate(), s = n || Date.CultureInfo.firstDayOfWeek, r = 8 - new Date(t, 0, 1).getDay(), f, i, u; return r == 8 && (r = 1), f = (Date.UTC(t, e, o, 0, 0, 0) - Date.UTC(t, 0, 1, 0, 0, 0)) / 864e5 + 1, i = Math.floor((f - r + 7) / 7), i === s && (t--, u = 8 - new Date(t, 0, 1).getDay(), i = u == 2 || u == 8 ? 53 : 52), i }, Date.prototype.isDST = function () { return console.log("isDST"), this.toString().match(/(E|C|M|P)(S|D)T/)[2] == "D" }, Date.prototype.getTimezone = function () { return Date.getTimezoneAbbreviation(this.getUTCOffset, this.isDST()) }, Date.prototype.setTimezoneOffset = function (n) { var t = this.getTimezoneOffset(), i = Number(n) * -6 / 10; return this.addMinutes(i - t), this }, Date.prototype.setTimezone = function (n) { return this.setTimezoneOffset(Date.getTimezoneOffset(n)) }, Date.prototype.getUTCOffset = function () { var t = this.getTimezoneOffset() * -10 / 6, n; return t < 0 ? (n = (t - 1e4).toString(), n[0] + n.substr(2)) : (n = (t + 1e4).toString(), "+" + n.substr(1)) }, Date.prototype.getDayName = function (n) { return n ? Date.CultureInfo.abbreviatedDayNames[this.getDay()] : Date.CultureInfo.dayNames[this.getDay()] }, Date.prototype.getMonthName = function (n) { return n ? Date.CultureInfo.abbreviatedMonthNames[this.getMonth()] : Date.CultureInfo.monthNames[this.getMonth()] }, Date.prototype._toString = Date.prototype.toString, Date.prototype.toString = function (n) { var t = this, i = function (n) { return n.toString().length == 1 ? "0" + n : n }; return n ? n.replace(/dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?/g, function (n) { switch (n) { case "hh": return i(t.getHours() < 13 ? t.getHours() : t.getHours() - 12); case "h": return t.getHours() < 13 ? t.getHours() : t.getHours() - 12; case "HH": return i(t.getHours()); case "H": return t.getHours(); case "mm": return i(t.getMinutes()); case "m": return t.getMinutes(); case "ss": return i(t.getSeconds()); case "s": return t.getSeconds(); case "yyyy": return t.getFullYear(); case "yy": return t.getFullYear().toString().substring(2, 4); case "dddd": return t.getDayName(); case "ddd": return t.getDayName(!0); case "dd": return i(t.getDate()); case "d": return t.getDate().toString(); case "MMMM": return t.getMonthName(); case "MMM": return t.getMonthName(!0); case "MM": return i(t.getMonth() + 1); case "M": return t.getMonth() + 1; case "t": return t.getHours() < 12 ? Date.CultureInfo.amDesignator.substring(0, 1) : Date.CultureInfo.pmDesignator.substring(0, 1); case "tt": return t.getHours() < 12 ? Date.CultureInfo.amDesignator : Date.CultureInfo.pmDesignator; case "zzz": case "zz": case "z": return "" } }) : this._toString() }, Date.now = function () { return new Date }, Date.today = function () { return Date.now().clearTime() }, Date.prototype._orient = 1, Date.prototype.next = function () { return this._orient = 1, this }, Date.prototype.last = Date.prototype.prev = Date.prototype.previous = function () { return this._orient = -1, this }, Date.prototype._is = !1, Date.prototype.is = function () { return this._is = !0, this }, Number.prototype._dateElement = "day", Number.prototype.fromNow = function () { var n = {}; return n[this._dateElement] = this, Date.now().add(n) }, Number.prototype.ago = function () { var n = {}; return n[this._dateElement] = this * -1, Date.now().add(n) }, function () { for (var n = Date.prototype, s = Number.prototype, f = "sunday monday tuesday wednesday thursday friday saturday".split(/\s/), e = "january february march april may june july august september october november december".split(/\s/), o = "Millisecond Second Minute Hour Day Week Month Year".split(/\s/), t, a = function (n) { return function () { return this._is ? (this._is = !1, this.getDay() == n) : this.moveToDayOfWeek(n, this._orient) } }, h, i, c, l, r, u = 0; u < f.length; u++) n[f[u]] = n[f[u].substring(0, 3)] = a(u); for (h = function (n) { return function () { return this._is ? (this._is = !1, this.getMonth() === n) : this.moveToMonth(n, this._orient) } }, i = 0; i < e.length; i++) n[e[i]] = n[e[i].substring(0, 3)] = h(i); for (c = function (n) { return function () { return n.substring(n.length - 1) != "s" && (n += "s"), this["add" + n](this._orient) } }, l = function (n) { return function () { return this._dateElement = n, this } }, r = 0; r < o.length; r++) t = o[r].toLowerCase(), n[t] = n[t + "s"] = c(o[r]), s[t] = s[t + "s"] = l(t) }(), Date.prototype.toJSONString = function () { return this.toString("yyyy-MM-ddThh:mm:ssZ") }, Date.prototype.toShortDateString = function () { return this.toString(Date.CultureInfo.formatPatterns.shortDatePattern) }, Date.prototype.toLongDateString = function () { return this.toString(Date.CultureInfo.formatPatterns.longDatePattern) }, Date.prototype.toShortTimeString = function () { return this.toString(Date.CultureInfo.formatPatterns.shortTimePattern) }, Date.prototype.toLongTimeString = function () { return this.toString(Date.CultureInfo.formatPatterns.longTimePattern) }, Date.prototype.getOrdinal = function () { switch (this.getDate()) { case 1: case 21: case 31: return "st"; case 2: case 22: return "nd"; case 3: case 23: return "rd"; default: return "th" } }, function () { var i, o, f, u; Date.Parsing = { Exception: function (n) { this.message = "Parse error at '" + n.substring(0, 10) + " ...'" } }; var t = Date.Parsing, n = t.Operators = { rtoken: function (n) { return function (i) { var r = i.match(n); if (r) return [r[0], i.substring(r[0].length)]; throw new t.Exception(i); } }, token: function () { return function (t) { return n.rtoken(new RegExp("^s*" + t + "s*"))(t) } }, stoken: function (t) { return n.rtoken(new RegExp("^" + t)) }, until: function (n) { return function (t) { for (var r = [], i = null; t.length;) { try { i = n.call(this, t) } catch (u) { r.push(i[0]), t = i[1]; continue } break } return [r, t] } }, many: function (n) { return function (t) { for (var i = [], r = null; t.length;) { try { r = n.call(this, t) } catch (u) { return [i, t] } i.push(r[0]), t = r[1] } return [i, t] } }, optional: function (n) { return function (t) { var i = null; try { i = n.call(this, t) } catch (r) { return [null, t] } return [i[0], i[1]] } }, not: function (n) { return function (i) { try { n.call(this, i) } catch (r) { return [null, i] } throw new t.Exception(i); } }, ignore: function (n) { return n ? function (t) { var i = null; return i = n.call(this, t), [null, i[1]] } : null }, product: function () { for (var i = arguments[0], u = Array.prototype.slice.call(arguments, 1), r = [], t = 0; t < i.length; t++) r.push(n.each(i[t], u)); return r }, cache: function (n) { var r = {}, i = null; return function (u) { try { i = r[u] = r[u] || n.call(this, u) } catch (f) { i = r[u] = f } if (i instanceof t.Exception) throw i; else return i } }, any: function () { var n = arguments; return function (i) { for (var r = null, u = 0; u < n.length; u++) if (n[u] != null) { try { r = n[u].call(this, i) } catch (f) { r = null } if (r) return r } throw new t.Exception(i); } }, each: function () { var n = arguments; return function (i) { for (var f = [], u = null, r = 0; r < n.length; r++) if (n[r] != null) { try { u = n[r].call(this, i) } catch (e) { throw new t.Exception(i); } f.push(u[0]), i = u[1] } return [f, i] } }, all: function () { var t = arguments, n = n; return n.each(n.optional(t)) }, sequence: function (i, r, u) { return (r = r || n.rtoken(/^\s*/), u = u || null, i.length == 1) ? i[0] : function (n) { for (var f = null, e = null, s = [], o = 0; o < i.length; o++) { try { f = i[o].call(this, n) } catch (h) { break } s.push(f[0]); try { e = r.call(this, f[1]) } catch (c) { e = null; break } n = e[1] } if (!f) throw new t.Exception(n); if (e) throw new t.Exception(e[1]); if (u) try { f = u.call(this, f[1]) } catch (l) { throw new t.Exception(f[1]); } return [s, f ? f[1] : n] } }, between: function (t, i, u) { u = u || t; var f = n.each(n.ignore(t), i, n.ignore(u)); return function (n) { var t = f.call(this, n); return [[t[0][0], r[0][2]], t[1]] } }, list: function (t, i, r) { return i = i || n.rtoken(/^\s*/), r = r || null, t instanceof Array ? n.each(n.product(t.slice(0, -1), n.ignore(i)), t.slice(-1), n.ignore(r)) : n.each(n.many(n.each(t, n.ignore(i))), px, n.ignore(r)) }, set: function (i, r, u) { return r = r || n.rtoken(/^\s*/), u = u || null, function (f) { for (var s = null, l = null, h = null, c = null, e = [[], f], o = !1, y, a, v = 0; v < i.length; v++) { h = null, l = null, s = null, o = i.length == 1; try { s = i[v].call(this, f) } catch (p) { continue } if (c = [[s[0]], s[1]], s[1].length > 0 && !o) try { h = r.call(this, s[1]) } catch (w) { o = !0 } else o = !0; if (o || h[1].length !== 0 || (o = !0), !o) { for (y = [], a = 0; a < i.length; a++) v != a && y.push(i[a]); l = n.set(y, r).call(this, h[1]), l[0].length > 0 && (c[0] = c[0].concat(l[0]), c[1] = l[1]) } if (c[1].length < e[1].length && (e = c), e[1].length === 0) break } if (e[0].length === 0) return e; if (u) { try { h = u.call(this, e[1]) } catch (b) { throw new t.Exception(e[1]); } e[1] = h[1] } return e } }, forward: function (n, t) { return function (i) { return n[t].call(this, i) } }, replace: function (n, t) { return function (i) { var r = n.call(this, i); return [t, r[1]] } }, process: function (n, t) { return function (i) { var r = n.call(this, i); return [t.call(this, r[0]), r[1]] } }, min: function (n, i) { return function (r) { var u = i.call(this, r); if (u[0].length < n) throw new t.Exception(r); return u } } }, s = function (n) { return function () { var t = null, u = [], i, r; if (arguments.length > 1 ? t = Array.prototype.slice.call(arguments) : arguments[0] instanceof Array && (t = arguments[0]), t) for (i = 0, r = t.shift() ; i < r.length; i++) return t.unshift(r[i]), u.push(n.apply(null, t)), t.shift(), u; else return n.apply(null, arguments) } }, e = "optional not ignore cache".split(/\s/); for (i = 0; i < e.length; i++) n[e[i]] = s(n[e[i]]); for (o = function (n) { return function () { return arguments[0] instanceof Array ? n.apply(null, arguments[0]) : n.apply(null, arguments) } }, f = "each any all".split(/\s/), u = 0; u < f.length; u++) n[f[u]] = o(n[f[u]]) }(), function () { var o = function (n) { for (var i = [], t = 0; t < n.length; t++) n[t] instanceof Array ? i = i.concat(o(n[t])) : n[t] && i.push(n[t]); return i }, u, f, e; Date.Grammar = {}, Date.Translator = { hour: function (n) { return function () { this.hour = Number(n) } }, minute: function (n) { return function () { this.minute = Number(n) } }, second: function (n) { return function () { this.second = Number(n) } }, meridian: function (n) { return function () { this.meridian = n.slice(0, 1).toLowerCase() } }, timezone: function (n) { return function () { var t = n.replace(/[^\d\+\-]/g, ""); t.length ? this.timezoneOffset = Number(t) : this.timezone = n.toLowerCase() } }, day: function (n) { var t = n[0]; return function () { this.day = Number(t.match(/\d+/)[0]) } }, month: function (n) { return function () { this.month = n.length == 3 ? Date.getMonthNumberFromName(n) : Number(n) - 1 } }, year: function (n) { return function () { var t = Number(n); this.year = n.length > 2 ? t : t + (t + 2e3 < Date.CultureInfo.twoDigitYearMax ? 2e3 : 1900) } }, rday: function (n) { return function () { switch (n) { case "yesterday": this.days = -1; break; case "tomorrow": this.days = 1; break; case "today": this.days = 0; break; case "now": this.days = 0, this.now = !0 } } }, finishExact: function (n) { var r, t, i; for (n = n instanceof Array ? n : [n], r = new Date, this.year = r.getFullYear(), this.month = r.getMonth(), this.day = 1, this.hour = 0, this.minute = 0, this.second = 0, t = 0; t < n.length; t++) n[t] && n[t].call(this); if (this.hour = this.meridian == "p" && this.hour < 13 ? this.hour + 12 : this.hour, this.day > Date.getDaysInMonth(this.year, this.month)) throw new RangeError(this.day + " is not a valid value for days."); return i = new Date(this.year, this.month, this.day, this.hour, this.minute, this.second), this.timezone ? i.set({ timezone: this.timezone }) : this.timezoneOffset && i.set({ timezoneOffset: this.timezoneOffset }), i }, finish: function (n) { var f, r, t, u, i, e; if (n = n instanceof Array ? o(n) : [n], n.length === 0) return null; for (f = 0; f < n.length; f++) typeof n[f] == "function" && n[f].call(this); return this.now ? new Date : (i = Date.today(), e = !!(this.days != null || this.orient || this.operator), e ? (u = this.orient == "past" || this.operator == "subtract" ? -1 : 1, this.weekday && (this.unit = "day", r = Date.getDayNumberFromName(this.weekday) - i.getDay(), t = 7, this.days = r ? (r + u * t) % t : u * t), this.month && (this.unit = "month", r = this.month - i.getMonth(), t = 12, this.months = r ? (r + u * t) % t : u * t, this.month = null), this.unit || (this.unit = "day"), (this[this.unit + "s"] == null || this.operator != null) && (this.value || (this.value = 1), this.unit == "week" && (this.unit = "day", this.value = this.value * 7), this[this.unit + "s"] = this.value * u), i.add(this)) : (this.meridian && this.hour && (this.hour = this.hour < 13 && this.meridian == "p" ? this.hour + 12 : this.hour), this.weekday && !this.day && (this.day = i.addDays(Date.getDayNumberFromName(this.weekday) - i.getDay()).getDate()), this.month && !this.day && (this.day = 1), i.set(this))) } }; var t = Date.Parsing.Operators, n = Date.Grammar, i = Date.Translator, r; n.datePartDelimiter = t.rtoken(/^([\s\-\.\,\/\x27]+)/), n.timePartDelimiter = t.stoken(":"), n.whiteSpace = t.rtoken(/^\s*/), n.generalDelimiter = t.rtoken(/^(([\s\,]|at|on)+)/), u = {}, n.ctoken = function (n) { var r = u[n], i; if (!r) { var o = Date.CultureInfo.regexPatterns, f = n.split(/\s+/), e = []; for (i = 0; i < f.length; i++) e.push(t.replace(t.rtoken(o[f[i]]), f[i])); r = u[n] = t.any.apply(null, e) } return r }, n.ctoken2 = function (n) { return t.rtoken(Date.CultureInfo.regexPatterns[n]) }, n.h = t.cache(t.process(t.rtoken(/^(0[0-9]|1[0-2]|[1-9])/), i.hour)), n.hh = t.cache(t.process(t.rtoken(/^(0[0-9]|1[0-2])/), i.hour)), n.H = t.cache(t.process(t.rtoken(/^([0-1][0-9]|2[0-3]|[0-9])/), i.hour)), n.HH = t.cache(t.process(t.rtoken(/^([0-1][0-9]|2[0-3])/), i.hour)), n.m = t.cache(t.process(t.rtoken(/^([0-5][0-9]|[0-9])/), i.minute)), n.mm = t.cache(t.process(t.rtoken(/^[0-5][0-9]/), i.minute)), n.s = t.cache(t.process(t.rtoken(/^([0-5][0-9]|[0-9])/), i.second)), n.ss = t.cache(t.process(t.rtoken(/^[0-5][0-9]/), i.second)), n.hms = t.cache(t.sequence([n.H, n.mm, n.ss], n.timePartDelimiter)), n.t = t.cache(t.process(n.ctoken2("shortMeridian"), i.meridian)), n.tt = t.cache(t.process(n.ctoken2("longMeridian"), i.meridian)), n.z = t.cache(t.process(t.rtoken(/^(\+|\-)?\s*\d\d\d\d?/), i.timezone)), n.zz = t.cache(t.process(t.rtoken(/^(\+|\-)\s*\d\d\d\d/), i.timezone)), n.zzz = t.cache(t.process(n.ctoken2("timezone"), i.timezone)), n.timeSuffix = t.each(t.ignore(n.whiteSpace), t.set([n.tt, n.zzz])), n.time = t.each(t.optional(t.ignore(t.stoken("T"))), n.hms, n.timeSuffix), n.d = t.cache(t.process(t.each(t.rtoken(/^([0-2]\d|3[0-1]|\d)/), t.optional(n.ctoken2("ordinalSuffix"))), i.day)), n.dd = t.cache(t.process(t.each(t.rtoken(/^([0-2]\d|3[0-1])/), t.optional(n.ctoken2("ordinalSuffix"))), i.day)), n.ddd = n.dddd = t.cache(t.process(n.ctoken("sun mon tue wed thu fri sat"), function (n) { return function () { this.weekday = n } })), n.M = t.cache(t.process(t.rtoken(/^(1[0-2]|0\d|\d)/), i.month)), n.MM = t.cache(t.process(t.rtoken(/^(1[0-2]|0\d)/), i.month)), n.MMM = n.MMMM = t.cache(t.process(n.ctoken("jan feb mar apr may jun jul aug sep oct nov dec"), i.month)), n.y = t.cache(t.process(t.rtoken(/^(\d\d?)/), i.year)), n.yy = t.cache(t.process(t.rtoken(/^(\d\d)/), i.year)), n.yyy = t.cache(t.process(t.rtoken(/^(\d\d?\d?\d?)/), i.year)), n.yyyy = t.cache(t.process(t.rtoken(/^(\d\d\d\d)/), i.year)), r = function () { return t.each(t.any.apply(null, arguments), t.not(n.ctoken2("timeContext"))) }, n.day = r(n.d, n.dd), n.month = r(n.M, n.MMM), n.year = r(n.yyyy, n.yy), n.orientation = t.process(n.ctoken("past future"), function (n) { return function () { this.orient = n } }), n.operator = t.process(n.ctoken("add subtract"), function (n) { return function () { this.operator = n } }), n.rday = t.process(n.ctoken("yesterday tomorrow today now"), i.rday), n.unit = t.process(n.ctoken("minute hour day week month year"), function (n) { return function () { this.unit = n } }), n.value = t.process(t.rtoken(/^\d\d?(st|nd|rd|th)?/), function (n) { return function () { this.value = n.replace(/\D/g, "") } }), n.expression = t.set([n.rday, n.operator, n.value, n.unit, n.orientation, n.ddd, n.MMM]), r = function () { return t.set(arguments, n.datePartDelimiter) }, n.mdy = r(n.ddd, n.month, n.day, n.year), n.ymd = r(n.ddd, n.year, n.month, n.day), n.dmy = r(n.ddd, n.day, n.month, n.year), n.date = function (t) { return (n[Date.CultureInfo.dateElementOrder] || n.mdy).call(this, t) }, n.format = t.process(t.many(t.any(t.process(t.rtoken(/^(dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?)/), function (t) { if (n[t]) return n[t]; throw Date.Parsing.Exception(t); }), t.process(t.rtoken(/^[^dMyhHmstz]+/), function (n) { return t.ignore(t.stoken(n)) }))), function (n) { return t.process(t.each.apply(null, n), i.finishExact) }), f = {}, e = function (t) { return f[t] = f[t] || n.format(t)[0] }, n.formats = function (n) { var r, i; if (n instanceof Array) { for (r = [], i = 0; i < n.length; i++) r.push(e(n[i])); return t.any.apply(null, r) } return e(n) }, n._formats = n.formats(["yyyy-MM-ddTHH:mm:ss", "ddd, MMM dd, yyyy H:mm:ss tt", "ddd MMM d yyyy HH:mm:ss zzz", "d"]), n._start = t.process(t.set([n.date, n.time, n.expression], n.generalDelimiter, n.whiteSpace), i.finish), n.start = function (t) { try { var i = n._formats.call({}, t); if (i[1].length === 0) return i } catch (r) { } return n._start.call({}, t) } }(), Date._parse = Date.parse, Date.parse = function (n) { var t = null; if (!n) return null; try { t = Date.Grammar.start.call({}, n) } catch (i) { return null } return t[1].length === 0 ? t[0] : null }, Date.getParseFunction = function (n) { var t = Date.Grammar.formats(n); return function (n) { var i = null; try { i = t.call({}, n) } catch (r) { return null } return i[1].length === 0 ? i[0] : null } }, Date.parseExact = function (n, t) { return Date.getParseFunction(t)(n) }
})();

//polyfill to use Date.fromISO which doesn't supports by old IE, FF
(function () {
    var D = new Date('2011-06-02T09:34:29+02:00');
    if (!D || +D !== 1307000069000) {
        Date.fromISO = function (s) {
            var day, tz,
            rx = /^(\d{4}\-\d\d\-\d\d([tT ][\d:\.]*)?)([zZ]|([+\-])(\d\d):(\d\d))?$/,
            p = rx.exec(s) || [];
            if (p[1]) {
                day = p[1].split(/\D/);
                for (var i = 0, L = day.length; i < L; i++) {
                    day[i] = parseInt(day[i], 10) || 0;
                };
                day[1] -= 1;
                day = new Date(Date.UTC.apply(Date, day));
                if (!day.getDate()) return NaN;
                if (p[5]) {
                    tz = (parseInt(p[5], 10) * 60);
                    if (p[6]) tz += parseInt(p[6], 10);
                    if (p[4] == '+') tz *= -1;
                    if (tz) day.setUTCMinutes(day.getUTCMinutes() + tz);
                }
                return day;
            }
            return NaN;
        }
    }
    else {
        Date.fromISO = function (s) {
            return new Date(s);
        }
    }
})();

// String.format.js
(function(){function n(n){return new RegExp("({)?\\{"+n+"\\}(?!})","gm")}function t(t){return t==null?"":t.replace(n("\\d+"),"")}String.format=function(){var r=arguments[0],i,u;if(r==null)return"";for(i=0;i<arguments.length-1;i++)u=n(i),r=r.replace(u,arguments[i+1]==null?"":arguments[i+1]);return t(r)},String.prototype.format=function(){for(var r=this.toString(),u,i=0;i<arguments.length;i++)u=n(i),r=r.replace(u,arguments[i]==null?"":arguments[i]);return t(r)},String.prototype.padL=function(n,t){if(!n||n<1)return this;t||(t=" ");var i=n-this.length;return i<1?this.substr(0,n):(String.repeat(t,i)+this).substr(0,n)},String.prototype.padR=function(n,t){if(!n||n<1)return this;t||(t=" ");var i=n-this.length;return i<1&&this.substr(0,n),(this+String.repeat(t,i)).substr(0,n)},String.repeat=function(n,t){for(var r="",i=0;i<t;i++)r+=n;return r}})()

var live800_url = 'http://kf1.learnsaas.com/chat/chatClient/chatbox.jsp?companyID=293933&jid=8086264846&enterurl=\"\"&pagetitle=\"\"&lan=zh_CN&tm=1375596097871';
var live800_url1 = 'http://kf1.learnsaas.com/chat/chatClient/chatbox.jsp?companyID=293933&jid=8086264846&lan=zh_CN&tm=1375596097871';

function live800_chat(pagetitle) { window.open(live800_url1 + '&pagetitle='+ pagetitle, '', 'width=570,height=430'); }

function is_login() { return $('.user-state-login').length > 0; }

$.fn.val_trim = function () { return $.trim(this.val()); }

$.fn.getPostData = function (n) { var t = {}; return $('input[type="text"], input[type="hidden"], input[type="password"], input[type="radio"]:checked, select, textarea', this).each(function () { var i = $(this).prop("name") || ""; i.length > 0 && (t[i] = $.trim($(this).val()) || "", n == !0 && $(this).val("")) }), t }

$.fn.load2=function(n,t,i,r){return i&&t&&(t.data=JSON.stringify(i)),this.load(n,t,r)}

$.fn.sliderShow = function () { function r(t, i, r, u) { n.find("li").eq(t).find("img").hide({ effect: "slide", direction: r, duration: 250 }), n.find("li").eq(i).find("img").show({ effect: "slide", direction: u, duration: 250 }) } function o(t, i) { n.find("li").eq(t).find("img").hide({ effect: "fade", duration: 250 }), n.find("li").eq(i).find("img").show({ effect: "fade", duration: 250 }) } function s(t, i) { n.find("li").eq(t).find("img").hide({ effect: "explode", duration: 800, pieces: 36 }), n.find("li").eq(i).find("img").show({ effect: "fade", duration: 400 }) } function u(t, i, r) { n.find("li").eq(t).find("img").hide({ effect: "clip", direction: r, duration: 250, complete: function () { n.find("li").eq(i).find("img").show({ effect: "clip", direction: r, duration: 250 }) } }) } function f(e) { var h, c; if ((clearTimeout(i), i = null, h = t.find("li.active").index(), h != e) && (c = n.find("li > img"), c.length != 0)) { e == undefined && (e = (h + 1) % c.length), c.stop(!0, !0), t.find("li").eq(h).removeClass("active"), t.find("li").eq(e).addClass("active"); switch (parseInt(Math.random() * 10)) { case 1: r(h, e, "up", "down"); break; case 2: r(h, e, "down", "up"); break; case 3: r(h, e, "left", "right"); break; case 4: r(h, e, "right", "left"); break; case 5: u(h, e, "up"); break; case 6: u(h, e, "down"); break; case 7: u(h, e, "left"); break; case 8: u(h, e, "right"); break; case 9: s(h, e, "right"); break; default: o(h, e) } i = setTimeout(f, 6000) } } var e = this, n = this.find("ul"), t = $("<ul><\/ul>").appendTo(e).addClass("slider_nav"), i; n.find("li").each(function () { $("img", this).hide(), $("<li><\/li>").appendTo(t) }), i = null, t.find("li").click(function () { f($(this).index()) }), f(0) }
$.fn.hoverSilder = function (s1, s2, s3, speed) {
    this.hover(
        function () {
            var isLogin = is_login();
            $(isLogin ? '.p1' : '.p2', this).show();
            $(isLogin ? '.p2' : '.p1', this).hide();
            $(s1, this).stop(true, true).show({ effect: "slide", direction: 'right', duration: speed });
            $(s2, this).stop(true, true).show({ effect: "slide", direction: 'down', duration: speed });
            $(s3, this).stop(true, true).show({ effect: "slide", direction: 'left', duration: speed });
        },
        function () {
            $(s1, this).stop(true, true).hide({ effect: "slide", direction: 'right', duration: speed });
            $(s2, this).stop(true, true).hide({ effect: "slide", direction: 'down', duration: speed });
            $(s3, this).stop(true, true).hide({ effect: "slide", direction: 'left', duration: speed });
        });
}

$.widget("custom.combobox", {
    _create: function () {
        this.wrapper = $("<span>")
          .addClass("custom-combobox")
          .insertAfter(this.element);

        this.element.hide();
        this._createAutocomplete();
        this._createShowAllButton();
    },

    _createAutocomplete: function () {
        var selected = this.element.children(":selected"),
          value = selected.val() ? selected.text() : "";

        this.input = $("<input>")
          .appendTo(this.wrapper)
          .val(value)
          .attr("title", "")
          .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
          .autocomplete({
              delay: 0,
              minLength: 0,
              source: $.proxy(this, "_source")
          })
          .tooltip({
              tooltipClass: "ui-state-highlight"
          });

        this._on(this.input, {
            autocompleteselect: function (event, ui) {
                ui.item.option.selected = true;
                this._trigger("select", event, {
                    item: ui.item.option
                });
            },

            autocompletechange: "_removeIfInvalid"
        });
    },

    _createShowAllButton: function () {
        var input = this.input,
          wasOpen = false;

        $("<a>")
          .attr("tabIndex", -1)
          .attr("title", "Show All Items")
          .tooltip()
          .appendTo(this.wrapper)
          .button({
              icons: {
                  primary: "ui-icon-triangle-1-s"
              },
              text: false
          })
          .removeClass("ui-corner-all")
          .addClass("custom-combobox-toggle ui-corner-right")
          .mousedown(function () {
              wasOpen = input.autocomplete("widget").is(":visible");
          })
          .click(function () {
              input.focus();

              // Close if already visible
              if (wasOpen) {
                  return;
              }

              // Pass empty string as value to search for, displaying all results
              input.autocomplete("search", "");
          });
    },

    _source: function (request, response) {
        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
        response(this.element.children("option").map(function () {
            var text = $(this).text();
            if (this.value && (!request.term || matcher.test(text)))
                return {
                    label: text,
                    value: text,
                    option: this
                };
        }));
    },

    _removeIfInvalid: function (event, ui) {

        // Selected an item, nothing to do
        if (ui.item) {
            return;
        }

        // Search for a match (case-insensitive)
        var value = this.input.val(),
          valueLowerCase = value.toLowerCase(),
          valid = false;
        this.element.children("option").each(function () {
            if ($(this).text().toLowerCase() === valueLowerCase) {
                this.selected = valid = true;
                return false;
            }
        });

        // Found a match, nothing to do
        if (valid) {
            return;
        }

        // Remove invalid value
        this.input
          .val("")
          .attr("title", value + " didn't match any item")
          .tooltip("open");
        this.element.val("");
        this._delay(function () {
            this.input.tooltip("close").attr("title", "");
        }, 2500);
        this.input.data("ui-autocomplete").term = "";
    },

    _destroy: function () {
        this.wrapper.remove();
        this.element.show();
    }
});

if (!window.console)
    window.console = { log: function () { }};

var IE7_PNG_SUFFIX = ".png";
$.blockUI.defaults.css = {};
$.blockUI.defaults.overlayCSS = {};

function addBookmark(){if(document.all)try{window.external.addFavorite(window.location.href,document.title)}catch(n){alert("加入收藏失败，请使用Ctrl+D进行添加")}else window.sidebar?window.sidebar.addPanel(document.title,window.location.href,""):alert("加入收藏失败，请使用Ctrl+D进行添加")}

function btnLogin_click(login_url) {
    var $panel = $('.login_panel');
    var postData = $panel.getPostData();
    $('input[name="n2"]', $panel).val('');
    if (postData.n1 == '') { return login_error.show('请输入帐号!'); }
    if (postData.n2 == '') { return login_error.show('请输入密码!'); }
    postData.action = 'login';
    //console.log(postData);
    $panel.block({
        blockMsgClass: 'login_busy', fadeIn: 0, fadeOut: 0, centerX: false,
        message: ' ', onBlock: function () {
            $panel.load2(login_url, {}, postData, function (responseText, status, xhr) {
                $panel.unblock();
                if (xhr.status != 200) {
                    login_error.show('({0}) {1}'.format(xhr.status, xhr.statusText));
                }
                $('input[name="n1"]', $panel).val(postData.n1);
            });
        }
    });
    return true;
}

function btnCLogin_click(login_url, lobby_url) {
    var $panel = $('.login_panel');
    var postData = $panel.getPostData();
    if (postData.n1 == '') { $("#lab_ErrMsg").text("请输入帐号!"); return; }
    if (postData.n2 == '') { $("#lab_ErrMsg").text("请输入密码!"); return; }
    postData.action = 'login';
    $('input[name="n2"]').val('');
    $("#lab_ErrMsg").load2(login_url, {}, postData, function (responseText, status, xhr) {
        if (responseText.trim() == "") {
            window.location = lobby_url;
        }
    });
    return true;
}

function btnLogout_click(login_url, home_url) {
    var $panel = $('.login_panel');
    $panel.block({
        fadeIn: 0, fadeOut: 0, centerX: false, css: { left: 244 },
        message: '<div class="login_busy"></div>', onBlock: function () {
            $panel.load2(login_url, {}, { action: 'logout' }, function () {
                $panel.unblock();
                if ($('.user-state-login').length == 0) {
                    $('.user-state-membercenter').each(function () {
                        window.location = home_url;
                    });
                }
                //user-state-membercenter
                //user-state-login
            });
        }
    });
}

var login_error = new function () {
    var handle = null;
    function clearHandle() {
        if (handle != null)
            window.clearTimeout(handle);
        handle = null;
    }
    function setHandle() {
        handle = window.setTimeout(hide, 3000);
    }
    function show(msg) {
        clearHandle();
        $('.login_err').hide().text(msg).show('drop', { direction: 'right' }, 200, setHandle);
    }
    function hide() {
        clearHandle();
        $('.login_err').hide('drop', { direction: 'right' }, 500);
    }

    return { show: show, hide: hide }
}

$.fn.playgame = function (trial) {
    //console.log(this, arguments);
    //$('input[name="subgame"]', this).val(subgame);
    $('input[name="trial"]', this).val(trial);
    //console.log($('input[name="trial"]', this));
    if (this.length == 1) { this[0].submit(); }
}
//function playgame(n){console.log(n);var t=$(".form_playgame");t.length==0||($('input[name="gameid"]',t).val(n),t.submit())}

$.fn.float_close = function () { event.cancelBubble = true; $(this).closest('.TplFloatSet').hide(); }
$.fn.float_ad = function (op) {
    var $this = this;

    function centerY() { $this.each(function () { $(this).animate({ top: $(document).scrollTop() + ($(window).innerHeight() - $(this).outerHeight()) / 2 }, op); }); };
    centerY();

    $(document).scroll(function () { centerY(); }).resize(function () { centerY(); });
}

function stripscript(val) {
    var pattern = new RegExp("[/\\\\#%\"'?]");
    var rs = "";
    for (var i = 0; i < val.length; i++) {
        rs = rs + val.substr(i, 1).replace(pattern, '');
    }
    return rs;
}
