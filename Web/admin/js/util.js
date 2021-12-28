// json2.js
var JSON; JSON || (JSON = {}), (function () { "use strict"; function i(n) { return n < 10 ? "0" + n : n } function f(n) { return o.lastIndex = 0, o.test(n) ? '"' + n.replace(o, function (n) { var t = s[n]; return typeof t == "string" ? t : "\\u" + ("0000" + n.charCodeAt(0).toString(16)).slice(-4) }) + '"' : '"' + n + '"' } function r(i, e) { var h, l, c, a, v = n, s, o = e[i]; o && typeof o == "object" && typeof o.toJSON == "function" && (o = o.toJSON(i)), typeof t == "function" && (o = t.call(e, i, o)); switch (typeof o) { case "string": return f(o); case "number": return isFinite(o) ? String(o) : "null"; case "boolean": case "null": return String(o); case "object": if (!o) return "null"; n += u, s = []; if (Object.prototype.toString.apply(o) === "[object Array]") { for (a = o.length, h = 0; h < a; h += 1) s[h] = r(h, o) || "null"; return c = s.length === 0 ? "[]" : n ? "[\n" + n + s.join(",\n" + n) + "\n" + v + "]" : "[" + s.join(",") + "]", n = v, c } if (t && typeof t == "object") for (a = t.length, h = 0; h < a; h += 1) typeof t[h] == "string" && (l = t[h], c = r(l, o), c && s.push(f(l) + (n ? ": " : ":") + c)); else for (l in o) Object.prototype.hasOwnProperty.call(o, l) && (c = r(l, o), c && s.push(f(l) + (n ? ": " : ":") + c)); return c = s.length === 0 ? "{}" : n ? "{\n" + n + s.join(",\n" + n) + "\n" + v + "}" : "{" + s.join(",") + "}", n = v, c } } typeof Date.prototype.toJSON != "function" && (Date.prototype.toJSON = function () { return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + i(this.getUTCMonth() + 1) + "-" + i(this.getUTCDate()) + "T" + i(this.getUTCHours()) + ":" + i(this.getUTCMinutes()) + ":" + i(this.getUTCSeconds()) + "Z" : null }, String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function () { return this.valueOf() }); var e = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, o = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, n, u, s = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, t; typeof JSON.stringify != "function" && (JSON.stringify = function (i, f, e) { var o; n = "", u = ""; if (typeof e == "number") for (o = 0; o < e; o += 1) u += " "; else typeof e == "string" && (u = e); t = f; if (f && typeof f != "function" && (typeof f != "object" || typeof f.length != "number")) throw new Error("JSON.stringify"); return r("", { "": i }) }), typeof JSON.parse != "function" && (JSON.parse = function (n, t) { function r(n, i) { var f, e, u = n[i]; if (u && typeof u == "object") for (f in u) Object.prototype.hasOwnProperty.call(u, f) && (e = r(u, f), e !== undefined ? u[f] = e : delete u[f]); return t.call(n, i, u) } var i; n = String(n), e.lastIndex = 0, e.test(n) && (n = n.replace(e, function (n) { return "\\u" + ("0000" + n.charCodeAt(0).toString(16)).slice(-4) })); if (/^[\],:{}\s]*$/.test(n.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) return i = eval("(" + n + ")"), typeof t == "function" ? r({ "": i }, "") : i; throw new SyntaxError("JSON.parse"); }) })();

// invoke_api
(function ($) {
    jQuery.extend({ // { success: function (data, textStatus, jqXHR) { }, error: function (xhr, ajaxOptions, thrownError) { } }
        invoke_api: function (command, options) {
            return $.ajax($.extend(true, {
                type: 'post',
                url: 'api',
                dataType: 'json',
                cache: false,
                async: true,
                data: { str: JSON.stringify(command) }
            }, options));
        }
    });
})(jQuery);

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
(function () {
    //可在Javascript中使用如同C#中的string.format
    //使用方式 : var fullName = String.format('Hello. My name is {0} {1}.', 'FirstName', 'LastName');
    String.format = function () {
        var s = arguments[0];
        if (s == null) return "";
        for (var i = 0; i < arguments.length - 1; i++) {
            var reg = getStringFormatPlaceHolderRegEx(i);
            s = s.replace(reg, (arguments[i + 1] == null ? "" : arguments[i + 1]));
        }
        return cleanStringFormatResult(s);
    }
    //可在Javascript中使用如同C#中的string.format (對jQuery String的擴充方法)
    //使用方式 : var fullName = 'Hello. My name is {0} {1}.'.format('FirstName', 'LastName');
    String.prototype.format = function () {
        var txt = this.toString();
        for (var i = 0; i < arguments.length; i++) {
            var exp = getStringFormatPlaceHolderRegEx(i);
            txt = txt.replace(exp, (arguments[i] == null ? "" : arguments[i]));
        }
        return cleanStringFormatResult(txt);
    }
    //讓輸入的字串可以包含{}
    function getStringFormatPlaceHolderRegEx(placeHolderIndex) {
        return new RegExp('({)?\\{' + placeHolderIndex + '\\}(?!})', 'gm')
    }
    //當format格式有多餘的position時，就不會將多餘的position輸出
    //ex:
    // var fullName = 'Hello. My name is {0} {1} {2}.'.format('firstName', 'lastName');
    // 輸出的 fullName 為 'firstName lastName', 而不會是 'firstName lastName {2}'
    function cleanStringFormatResult(txt) {
        if (txt == null) return "";
        return txt.replace(getStringFormatPlaceHolderRegEx("\\d+"), "");
    }

    String.prototype.padL = function (width, pad) {
        if (!width || width < 1)
            return this;

        if (!pad) pad = " ";

        var length = width - this.length

        if (length < 1)
            return this.substr(0, width);

        return (String.repeat(pad, length) + this).substr(0, width);
    }
    String.prototype.padR = function (width, pad) {
        if (!width || width < 1)
            return this;

        if (!pad) pad = " ";

        var length = width - this.length

        if (length < 1) this.substr(0, width);
        return (this + String.repeat(pad, length)).substr(0, width);
    }
    String.repeat = function (chr, count) {
        var str = "";
        for (var x = 0; x < count; x++) {
            str += chr
        };
        return str;
    }
})();

(function () {
})();
