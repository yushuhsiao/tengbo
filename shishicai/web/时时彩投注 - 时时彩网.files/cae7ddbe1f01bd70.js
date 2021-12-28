if (!this.JSON) {
    JSON = {}
}
(function () {
    function f(n) {
        return n < 10 ? "0" + n : n
    }
    if (typeof Date.prototype.toJSON !== "function") {
        Date.prototype.toJSON = function (key) {
            return this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z"
        };
        String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (key) {
            return this.valueOf()
        }

    }
    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = {
        "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\"
    }
    , rep;
    function quote(string) {
        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            if (typeof c === "string") {
                return c
            }
            return "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4)
        }) + '"' : '"' + string + '"'
    }
    function str(key, holder) {
        var i, k, v, length, mind = gap, partial, value = holder[key];
        if (value && typeof value === "object" && typeof value.toJSON === "function") {
            value = value.toJSON(key)
        }
        if (typeof rep === "function") {
            value = rep.call(holder, key, value)
        }
        switch (typeof value) {
            case "string": return quote(value);
            case "number": return isFinite(value) ? String(value) : "null";
            case "boolean": case "null": return String(value);
            case "object": if (!value) {
                return "null"
            }
                gap += indent;
                partial = [];
                if (typeof value.length === "number" && !value.propertyIsEnumerable("length")) {
                    length = value.length;
                    for (i = 0;
                    i < length;
                    i += 1) {
                        partial[i] = str(i, value) || "null"
                    }
                    v = partial.length === 0 ? "[]" : gap ? "[\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "]" : "[" + partial.join(",") + "]";
                    gap = mind;
                    return v
                }
                if (rep && typeof rep === "object") {
                    length = rep.length;
                    for (i = 0;
                    i < length;
                    i += 1) {
                        k = rep[i];
                        if (typeof k === "string") {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ": " : ":") + v)
                            }

                        }

                    }

                }
                else {
                    for (k in value) {
                        if (Object.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ": " : ":") + v)
                            }

                        }

                    }

                }
                v = partial.length === 0 ? "{}" : gap ? "{\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "}" : "{" + partial.join(",") + "}";
                gap = mind;
                return v
        }

    }
    if (typeof JSON.stringify !== "function") {
        JSON.stringify = function (value, replacer, space) {
            var i;
            gap = "";
            indent = "";
            if (typeof space === "number") {
                for (i = 0;
                i < space;
                i += 1) {
                    indent += " "
                }

            }
            else {
                if (typeof space === "string") {
                    indent = space
                }

            }
            rep = replacer;
            if (replacer && typeof replacer !== "function" && (typeof replacer !== "object" || typeof replacer.length !== "number")) {
                throw new Error("JSON.stringify")
            }
            return str("", {
                "": value
            })
        }

    }
    if (typeof JSON.parse !== "function") {
        JSON.parse = function (text, reviver) {
            var j;
            function walk(holder, key) {
                var k, v, value = holder[key];
                if (value && typeof value === "object") {
                    for (k in value) {
                        if (Object.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v
                            }
                            else {
                                delete value[k]
                            }

                        }

                    }

                }
                return reviver.call(holder, key, value)
            }
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4)
                })
            }
            if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) {
                j = eval("(" + text + ")");
                return typeof reviver === "function" ? walk({
                    "": j
                }
                , "") : j
            }
            throw new SyntaxError("JSON.parse")
        }

    }

})();
Function.prototype.bind = function () {
    var a = this;
    var b = Array.prototype.slice.call(arguments);
    var c = b.shift();
    return function () {
        return a.apply(c, b.concat(Array.prototype.slice.call(arguments)))
    }

};
Number.prototype.toFormatString = function (a) {
    var d = this.toString();
    var c = [];
    for (var b = 0;
    b < a.length - d.length;
    b++) {
        c.push(a.toString().substr(b, 1))
    }
    return c.join("") + d
};
Number.prototype.toLeftTime = function () {
    var a = {
        Day: 0, Hour: 0, TotalHours: 0, Minute: 0, TotalMinutes: 0, Seond: 0, TotalSeconds: 0, MilliSeond: 0
    };
    a.Day = parseInt(this / (1000 * 60 * 60 * 24), 10);
    a.Hour = parseInt((this / (1000 * 60 * 60)) % 24, 10);
    a.TotalHours = parseInt((this / (1000 * 60 * 60)) / 24, 10);
    a.Minute = parseInt((this / (1000 * 60)) % 60, 10);
    a.TotalMinutes = parseInt((this / (1000 * 60)) / 60, 10);
    a.Seond = parseInt((this / 1000) % 60, 10);
    a.TotalSeconds = parseInt((this / 1000), 10);
    a.MilliSeond = this % 1000;
    a.toString = function (b) {
        b = b || "[{#d#}天 ]{#HH#}:{#mm#}:{#ss#}";
        return b.replace(/\[([^\]]*){#tHH#}([^\]]*)\]/g, this.TotalHours <= 0 ? "" : "$1" + this.TotalHours.toFormatString("00") + "$2").replace(/\[([^\]]*){#tmm#}([^\]]*)\]/g, this.TotalMinutes <= 0 ? "" : "$1" + this.TotalMinutes.toFormatString("00") + "$2").replace(/\[([^\]]*){#tss#}([^\]]*)\]/g, this.TotalSeconds <= 0 ? "" : "$1" + this.TotalSeconds.toFormatString("00") + "$2").replace(/\[([^\]]*){#tH#}([^\]]*)\]/g, this.TotalHours <= 0 ? "" : "$1" + this.TotalHours + "$2").replace(/\[([^\]]*){#tm#}([^\]]*)\]/g, this.TotalMinutes <= 0 ? "" : "$1" + this.TotalMinutes + "$2").replace(/\[([^\]]*){#ts#}([^\]]*)\]/g, this.TotalSeconds <= 0 ? "" : "$1" + this.TotalSeconds + "$2").replace(/{#tHH#}/g, this.TotalHours.toFormatString("00")).replace(/{#tmm#}/g, this.TotalMinutes.toFormatString("00")).replace(/{#tss#}/g, this.TotalSeconds.toFormatString("00")).replace(/{#tH#}/g, this.TotalHours).replace(/{#tm#}/g, this.TotalMinutes).replace(/{#ts#}/g, this.TotalSeconds).replace(/\[([^\]]*){#dd#}([^\]]*)\]/g, this.Day <= 0 ? "" : "$1" + this.Day.toFormatString("00") + "$2").replace(/\[([^\]]*){#HH#}([^\]]*)\]/g, this.Hour <= 0 ? "" : "$1" + this.Hour.toFormatString("00") + "$2").replace(/\[([^\]]*){#mm#}([^\]]*)\]/g, this.Minute <= 0 ? "" : "$1" + this.Minute.toFormatString("00") + "$2").replace(/\[([^\]]*){#ss#}([^\]]*)\]/g, this.Seond <= 0 ? "" : "$1" + this.Seond.toFormatString("00") + "$2").replace(/\[([^\]]*){#d#}([^\]]*)\]/g, this.Day <= 0 ? "" : "$1" + this.Day + "$2").replace(/\[([^\]]*){#H#}([^\]]*)\]/g, this.Hour <= 0 ? "" : "$1" + this.Hour + "$2").replace(/\[([^\]]*){#m#}([^\]]*)\]/g, this.Minute <= 0 ? "" : "$1" + this.Minute + "$2").replace(/\[([^\]]*){#s#}([^\]]*)\]/g, this.Seond <= 0 ? "" : "$1" + this.Seond + "$2").replace(/{#dd#}/g, this.Day.toFormatString("00")).replace(/{#HH#}/g, this.Hour.toFormatString("00")).replace(/{#mm#}/g, this.Minute.toFormatString("00")).replace(/{#ss#}/g, this.Seond.toFormatString("00")).replace(/{#d#}/g, this.Day).replace(/{#H#}/g, this.Hour).replace(/{#m#}/g, this.Minute).replace(/{#s#}/g, this.Seond)
    };
    a.toSimple = function () {
        var b = this.toString();
        if (this.Hour == 0 && this.Minute == 0) {
            b = this.Seond + "秒前"
        }
        else {
            if (this.Hour == 0) {
                b = this.Minute + "分钟前"
            }
            else {
                b = this.Hour + "小时前"
            }

        }
        return b
    };
    return a
};
Number.prototype.toMoney = function (g, d, a) {
    g = g || "";
    var c = this >= 0 ? "" : "-";
    var e = Math.abs(this).toString();
    var i = e.split(".");
    var h = i[0];
    while (h.length % 3 != 0) {
        h = "0" + h
    }
    h = g + c + h.replace(/(\d{3})/g, "$1,").replace(/,\./, ".").replace(/(^0*)|(,$)/g, "");
    h = h == g || h.replace(g, "").indexOf(".") == 0 ? g + "0" + (h.replace(g, "").indexOf(".") == 0 ? h.replace(g, "") : "") : h;
    if (typeof (d) != "undefined" && !isNaN(d) && d > 0) {
        var b = i.length == 1 ? 0 : parseFloat("0." + i[1]);
        b = b * Math.pow(10, d);
        switch (a) {
            case "floor": b = Math.floor(b);
                break;
            case "ceil": b = Math.ceil(b);
                break;
            case "round": default: b = Math.round(b);
                break
        }
        b = b / Math.pow(10, d);
        h = h + "." + b.toFixed(d).split(".")[1]
    }
    return h == g ? "0" : h
};
Number.prototype.checkPrime = function () {
    if (this == 0) {
        return false
    }
    for (var a = 2;
    a <= Math.sqrt(this) ;
    a++) {
        if (this % a == 0) {
            return false
        }

    }
    return true
};
String.format = function () {
    var b = arguments[0].toString();
    for (var a = 1;
    a < arguments.length;
    a++) {
        b = b.replace(new RegExp("\\{" + (a - 1).toString() + "\\}", "g"), arguments[a])
    }
    return b
};
String.prototype.toInt = function (a) {
    return parseInt(this, a | 10)
};
String.prototype.toFloat = function () {
    return parseFloat(this.replace(/\,/ig, ""))
};
String.prototype.toRGBA = function () {
    var a = this.replace("#", "");
    var b = a.length == 3 ? 1 : 2;
    var c = [a.substr(0, b), a.substr(b, b), a.substr(2 * b, b)];
    for (var b = 0;
    b < c.length;
    b++) {
        c[b] = parseInt(c[b], 16).toString(10);
        c[b] = a.length == 6 ? c[b] : parseInt(c[b], 10) * parseInt(c[b], 10)
    }
    return c.toString()
};
String.prototype.contains = function (a) {
    return (this.indexOf(a) > -1)
};
String.prototype.IsChinese = function () {
    var a = /^[\u4E00-\u9FA5]*$/;
    return a.test(this)
};
String.prototype.toHorn = function (c) {
    var d = new Array();
    for (var b = 0;
    b < this.length;
    b++) {
        var a = this.charCodeAt(b);
        if (a < 125 && c == true) {
            d.push(String.fromCharCode(a + 65248))
        }
        else {
            if (a > 125 && c != true) {
                d.push(String.fromCharCode(a - 65248))
            }
            else {
                d.push(this.substr(b, 1))
            }

        }

    }
    return d.join("")
};
String.prototype.toDate = function () {
    var a = new Date();
    a.setTime(Date.parse(this.replace(/\-/ig, "/")));
    return a
};
String.prototype.toUrl = function () {
    var b = document.createElement("a");
    b.href = this;
    return {
        source: this, protocol: b.protocol.replace(":", ""), host: b.hostname, port: parseInt(b.port, 10) == 0 ? 80 : parseInt(b.port, 10), query: b.search, params: (function () {
            var d = {}, f = b.search.replace(/^\?/, "").split("&"), c = f.length, a = 0, e;
            for (;
            a < c;
            a++) {
                if (!f[a]) {
                    continue
                }
                e = f[a].split("=");
                d[e[0]] = e[1]
            }
            return d
        })(), file: (b.pathname.match(/\/([^\/?#]+)$/i) || [, ""])[1], hash: b.hash.replace("#", ""), path: b.pathname.replace(/^([^\/])/, "/$1"), relative: (b.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ""])[1], segments: b.pathname.replace(/^\//, "").split("/")
    }

};
Array.prototype.toJSONAry = function () {
    var c = [];
    for (var a = 0;
    a < this.length;
    a++) {
        var b = this[a];
        if (typeof b === "object" && b.propertyIsEnumerable("toJSONInstance") && typeof b.toJSONInstance === "function") {
            c.push(b.toJSONInstance())
        }
        else {
            c.push(b)
        }

    }
    return c
};
Array.prototype.indexOf = function (d, b) {
    var a = this.length;
    if (!b) {
        b = 0
    }
    else {
        if (b < 0) {
            b = Math.max(0, a + b)
        }

    }
    for (var c = b;
    c < a;
    c++) {
        if (this[c] == d) {
            return c
        }

    }
    return -1
};
Array.prototype.remove = function (b, d) {
    var c = 0;
    if (!d) {
        var a = this.indexOf(b);
        if (a > -1) {
            this.splice(a, 1);
            c = 1
        }

    }
    else {
        while ((a = this.indexOf(b)) > -1) {
            this.splice(a, 1);
            c++
        }

    }
    return c
};
Array.prototype.removeAt = function (a) {
    return this.splice(a, 1)
};
Array.prototype.removeReItem = function (c) {
    var e = {};
    var a = this.length;
    for (var b = 0;
    b < a;
    b++) {
        if (typeof e[this[b]] == "undefined") {
            if (this[b] != c) {
                e[this[b]] = 1
            }

        }

    }
    this.length = 0;
    for (var d in e) {
        this[this.length] = d
    }
    return this
};
Array.prototype.clearRepeat = function () {
    var d = {};
    var a = this.length;
    for (var b = 0;
    b < a;
    b++) {
        if (typeof d[this[b]] == "undefined") {
            d[this[b]] = this[b]
        }

    }
    this.length = 0;
    for (var c in d) {
        this[this.length] = c
    }
    return this
};
Array.prototype.hasRepeat = function () {
    var c = {};
    var a = this.length;
    for (var b = 0;
    b < a;
    b++) {
        if (typeof c[this[b]] == "undefined") {
            c[this[b]] = this[b]
        }
        else {
            return true
        }

    }
    return false
};
Array.prototype.exists = function (a) {
    return (this.indexOf(a) != -1)
};
Array.prototype.getMax = function () {
    var a = this.length;
    for (var b = 1, c = this[0];
    b < a;
    b++) {
        if (c < this[b]) {
            c = this[b]
        }

    }
    return c
};
Array.prototype.getMin = function () {
    var a = this.length;
    for (var b = 1, c = this[0];
    b < a;
    b++) {
        if (c > this[b]) {
            c = this[b]
        }

    }
    return c
};
Array.prototype.clear = function () {
    this.length = 0
};
Array.prototype.addArray = function (a) {
    var c = a.length;
    for (var b = 0;
    b < c;
    b++) {
        this.push(a[b])
    }

};
Array.prototype.insertAt = function (a, b) {
    this.splice(a, 0, b)
};
Array.prototype.insertBefore = function (a, c) {
    var b = this.indexOf(a);
    if (b == -1) {
        this.push(c)
    }
    else {
        this.splice(b, 0, c)
    }

};
Array.prototype.getMostItems = function () {
    var f = {}, c = 0, d = new Array();
    var a = this.length;
    for (var b = 0;
    b < a;
    b++) {
        f[this[b]] ? ++f[this[b]] : f[this[b]] = 1
    }
    for (var e in f) {
        c = Math.max(c, f[e]);
        if (c == f[e]) {
            d[d.length] = e
        }

    }
    return {
        value: d, times: c
    }

};
Array.prototype.circle = function (b) {
    var c = null;
    if (b) {
        c = this[0];
        this.shift();
        this.push(c)
    }
    else {
        var a = this.length;
        c = this[a - 1];
        this.length = a - 1;
        this.unshift(c)
    }

};
Array.prototype.math = function (b, a) {
    var f = parseInt("a", 10), d = b[0];
    for (var c = 0;
    c < this.length;
    c++) {
        if (typeof (this[c]) == "string" && a != true) {
            this[c] = parseFloat(this[c], 10)
        }
        switch (d.toString().toLowerCase()) {
            case "-": if (b.length == 1) {
                f = c == 0 ? this[c] : f - this[c]
            }
            else {
                var e = [].concat(b.slice(1, b.length));
                f = c == 0 ? this[c].math(e) : f - this[c].math(e)
            }
                break;
            case "+": if (b.length == 1) {
                f = c == 0 ? this[c] : f + this[c]
            }
            else {
                var e = [].concat(b.slice(1, b.length));
                f = c == 0 ? this[c].math(e) : f + this[c].math(e)
            }
                break;
            case "x": if (b.length == 1) {
                f = c == 0 ? this[c] : f * this[c]
            }
            else {
                var e = [].concat(b.slice(1, b.length));
                f = c == 0 ? this[c].math(e) : f * this[c].math(e)
            }
                break
        }

    }
    return f
};
Date.prototype.toFormatString = function (a) {
    var h = this.getFullYear();
    var e = parseInt(this.getMonth() + 1, 10);
    var b = parseInt(this.getDate(), 10);
    var c = parseInt(this.getHours(), 10);
    var d = parseInt(this.getMinutes(), 10);
    var g = parseInt(this.getSeconds(), 10);
    var f = parseInt(this.getMilliseconds(), 10);
    a = a || "{#yyyy#}-{#MM#}-{#dd#} {#HH#}:{#mm#}:{#ss#}";
    return a.replace("{#yyyy#}", h).replace("{#MM#}", e.toFormatString("00")).replace("{#dd#}", b.toFormatString("00")).replace("{#HH#}", c.toFormatString("00")).replace("{#mm#}", d.toFormatString("00")).replace("{#ss#}", g.toFormatString("00")).replace("{#fff#}", f.toFormatString("000")).replace("{#M#}", e).replace("{#d#}", b).replace("{#H#}", c).replace("{#m#}", d).replace("{#s#}", g).replace("{#yy#}", h.toString().substr(2, 2)).replace("{#f#}", f.toString().substr(0, 1)).replace("{#week#}", this.getWeekDay()).replace("{#day#}", this.getDay())
};
Date.prototype.addMonthes = function (b) {
    var f = parseInt(b / 12, 10);
    var e = this.getFullYear() + f;
    var a = this.getMonth() + b % 12;
    if (a > 11) {
        e += 1;
        a -= 12
    }
    else {
        if (a < 0) {
            e -= 1;
            a += 12
        }

    }
    var c = new Date(e, a, this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds(), this.getMilliseconds());
    if (c.getMonth() > a) {
        var d = new Date(c.getFullYear(), c.getMonth()).addDays(-1);
        c = new Date(d.getFullYear(), d.getMonth(), d.getDate(), c.getHours(), c.getMinutes(), c.getSeconds(), c.getMilliseconds())
    }
    return c
};
Date.prototype.addDays = function (b) {
    var c = this.getTime() + (b * 24 * 60 * 60 * 1000);
    var a = new Date();
    a.setTime(c);
    return a
};
Date.prototype.addHours = function (b) {
    var c = this.getTime() + (b * 60 * 60 * 1000);
    var a = new Date();
    a.setTime(c);
    return a
};
Date.prototype.addMinutes = function (b) {
    var c = this.getTime() + (b * 60 * 1000);
    var a = new Date();
    a.setTime(c);
    return a
};
Date.prototype.addSeconds = function (b) {
    var c = this.getTime() + (b * 1000);
    var a = new Date();
    a.setTime(c);
    return a
};
Date.prototype.addMilliseconds = function (b) {
    var c = this.getTime() + b;
    var a = new Date();
    a.setTime(c);
    return a
};
Date.prototype.getWeekDay = function (a) {
    a = a || ["日", "一", "二", "三", "四", "五", "六"];
    return a[this.getDay()]
};
(function (br, bq) {
    var A = br.document, ad = br.navigator, ac = br.location;
    var Z = (function () {
        var bF = function (b4, b3) {
            return new bF.fn.init(b4, b3, bP)
        }
        , bx = br.jQuery, e = br.$, bP, bH = /^(?:[^#<]*(<[\w\W]+>)[^>]*$|#([\w\-]*)$)/, bO = /\S/, b0 = /^\s+/, b1 = /\s+$/, bJ = /\d/, bR = /^<(\w+)\s*\/?>(?:<\/\1>)?$/, bT = /^[\],:{}\s]*$/, bU = /\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, bV = /"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, bS = /(?:^|:|,)(?:\s*\[)+/g, bW = /(webkit)[ \/]([\w.]+)/, bQ = /(opera)(?:.*version)?[ \/]([\w.]+)/, bM = /(msie) ([\w.]+)/, bL = /(mozilla)(?:.*? rv:([\w.]+))?/, bI = /-([a-z]|[0-9])/ig, bN = /^-ms-/, bC = function (b3, b4) {
            return (b4 + "").toUpperCase()
        }
        , b2 = ad.userAgent, by, bK, bA, bY = Object.prototype.toString, bD = Object.prototype.hasOwnProperty, bG = Array.prototype.push, bX = Array.prototype.slice, bZ = String.prototype.trim, bE = Array.prototype.indexOf, bz = {};
        bF.fn = bF.prototype = {
            constructor: bF, init: function (b9, b3, b8) {
                var b6, b5, b7, b4;
                if (!b9) {
                    return this
                }
                if (b9.nodeType) {
                    this.context = this[0] = b9;
                    this.length = 1;
                    return this
                }
                if (b9 === "body" && !b3 && A.body) {
                    this.context = A;
                    this[0] = A.body;
                    this.selector = b9;
                    this.length = 1;
                    return this
                }
                if (typeof b9 === "string") {
                    if (b9.charAt(0) === "<" && b9.charAt(b9.length - 1) === ">" && b9.length >= 3) {
                        b6 = [null, b9, null]
                    }
                    else {
                        b6 = bH.exec(b9)
                    }
                    if (b6 && (b6[1] || !b3)) {
                        if (b6[1]) {
                            b3 = b3 instanceof bF ? b3[0] : b3;
                            b4 = (b3 ? b3.ownerDocument || b3 : A);
                            b7 = bR.exec(b9);
                            if (b7) {
                                if (bF.isPlainObject(b3)) {
                                    b9 = [A.createElement(b7[1])];
                                    bF.fn.attr.call(b9, b3, true)
                                }
                                else {
                                    b9 = [b4.createElement(b7[1])]
                                }

                            }
                            else {
                                b7 = bF.buildFragment([b6[1]], [b4]);
                                b9 = (b7.cacheable ? bF.clone(b7.fragment) : b7.fragment).childNodes
                            }
                            return bF.merge(this, b9)
                        }
                        else {
                            b5 = A.getElementById(b6[2]);
                            if (b5 && b5.parentNode) {
                                if (b5.id !== b6[2]) {
                                    return b8.find(b9)
                                }
                                this.length = 1;
                                this[0] = b5
                            }
                            this.context = A;
                            this.selector = b9;
                            return this
                        }

                    }
                    else {
                        if (!b3 || b3.jquery) {
                            return (b3 || b8).find(b9)
                        }
                        else {
                            return this.constructor(b3).find(b9)
                        }

                    }

                }
                else {
                    if (bF.isFunction(b9)) {
                        return b8.ready(b9)
                    }

                }
                if (b9.selector !== bq) {
                    this.selector = b9.selector;
                    this.context = b9.context
                }
                return bF.makeArray(b9, this)
            }
            , selector: "", jquery: "1.7", length: 0, size: function () {
                return this.length
            }
            , toArray: function () {
                return bX.call(this, 0)
            }
            , get: function (b3) {
                return b3 == null ? this.toArray() : (b3 < 0 ? this[this.length + b3] : this[b3])
            }
            , pushStack: function (b3, b4, b6) {
                var b5 = this.constructor();
                if (bF.isArray(b3)) {
                    bG.apply(b5, b3)
                }
                else {
                    bF.merge(b5, b3)
                }
                b5.prevObject = this;
                b5.context = this.context;
                if (b4 === "find") {
                    b5.selector = this.selector + (this.selector ? " " : "") + b6
                }
                else {
                    if (b4) {
                        b5.selector = this.selector + "." + b4 + "(" + b6 + ")"
                    }

                }
                return b5
            }
            , each: function (b4, b3) {
                return bF.each(this, b4, b3)
            }
            , ready: function (b3) {
                bF.bindReady();
                bK.add(b3);
                return this
            }
            , eq: function (b3) {
                return b3 === -1 ? this.slice(b3) : this.slice(b3, +b3 + 1)
            }
            , first: function () {
                return this.eq(0)
            }
            , last: function () {
                return this.eq(-1)
            }
            , slice: function () {
                return this.pushStack(bX.apply(this, arguments), "slice", bX.call(arguments).join(","))
            }
            , map: function (b3) {
                return this.pushStack(bF.map(this, function (b4, b5) {
                    return b3.call(b4, b5, b4)
                }))
            }
            , end: function () {
                return this.prevObject || this.constructor(null)
            }
            , push: bG, sort: [].sort, splice: [].splice
        };
        bF.fn.init.prototype = bF.fn;
        bF.extend = bF.fn.extend = function () {
            var ca, b9, cb, b4, b5, b3, cc = arguments[0] || {}, b7 = 1, b8 = arguments.length, b6 = false;
            if (typeof cc === "boolean") {
                b6 = cc;
                cc = arguments[1] || {};
                b7 = 2
            }
            if (typeof cc !== "object" && !bF.isFunction(cc)) {
                cc = {}
            }
            if (b8 === b7) {
                cc = this;
                --b7
            }
            for (;
            b7 < b8;
            b7++) {
                if ((ca = arguments[b7]) != null) {
                    for (b9 in ca) {
                        cb = cc[b9];
                        b4 = ca[b9];
                        if (cc === b4) {
                            continue
                        }
                        if (b6 && b4 && (bF.isPlainObject(b4) || (b5 = bF.isArray(b4)))) {
                            if (b5) {
                                b5 = false;
                                b3 = cb && bF.isArray(cb) ? cb : []
                            }
                            else {
                                b3 = cb && bF.isPlainObject(cb) ? cb : {}
                            }
                            cc[b9] = bF.extend(b6, b3, b4)
                        }
                        else {
                            if (b4 !== bq) {
                                cc[b9] = b4
                            }

                        }

                    }

                }

            }
            return cc
        };
        bF.extend({
            noConflict: function (b3) {
                if (br.$ === bF) {
                    br.$ = e
                }
                if (b3 && br.jQuery === bF) {
                    br.jQuery = bx
                }
                return bF
            }
            , isReady: false, readyWait: 1, holdReady: function (b3) {
                if (b3) {
                    bF.readyWait++
                }
                else {
                    bF.ready(true)
                }

            }
            , ready: function (b3) {
                if ((b3 === true && !--bF.readyWait) || (b3 !== true && !bF.isReady)) {
                    if (!A.body) {
                        return setTimeout(bF.ready, 1)
                    }
                    bF.isReady = true;
                    if (b3 !== true && --bF.readyWait > 0) {
                        return
                    }
                    bK.fireWith(A, [bF]);
                    if (bF.fn.trigger) {
                        bF(A).trigger("ready").unbind("ready")
                    }

                }

            }
            , bindReady: function () {
                if (bK) {
                    return
                }
                bK = bF.Callbacks("once memory");
                if (A.readyState === "complete") {
                    return setTimeout(bF.ready, 1)
                }
                if (A.addEventListener) {
                    A.addEventListener("DOMContentLoaded", bA, false);
                    br.addEventListener("load", bF.ready, false)
                }
                else {
                    if (A.attachEvent) {
                        A.attachEvent("onreadystatechange", bA);
                        br.attachEvent("onload", bF.ready);
                        var b4 = false;
                        try {
                            b4 = br.frameElement == null
                        }
                        catch (b3) { } if (A.documentElement.doScroll && b4) {
                            bB()
                        }

                    }

                }

            }
            , isFunction: function (b3) {
                return bF.type(b3) === "function"
            }
            , isArray: Array.isArray || function (b3) {
                return bF.type(b3) === "array"
            }
            , isWindow: function (b3) {
                return b3 && typeof b3 === "object" && "setInterval" in b3
            }
            , isNumeric: function (b3) {
                return b3 != null && bJ.test(b3) && !isNaN(b3)
            }
            , type: function (b3) {
                return b3 == null ? String(b3) : bz[bY.call(b3)] || "object"
            }
            , isPlainObject: function (b5) {
                if (!b5 || bF.type(b5) !== "object" || b5.nodeType || bF.isWindow(b5)) {
                    return false
                }
                try {
                    if (b5.constructor && !bD.call(b5, "constructor") && !bD.call(b5.constructor.prototype, "isPrototypeOf")) {
                        return false
                    }

                }
                catch (b3) {
                    return false
                }
                var b4;
                for (b4 in b5) { } return b4 === bq || bD.call(b5, b4)
            }
            , isEmptyObject: function (b4) {
                for (var b3 in b4) {
                    return false
                }
                return true
            }
            , error: function (b3) {
                throw b3
            }
            , parseJSON: function (b3) {
                if (typeof b3 !== "string" || !b3) {
                    return null
                }
                b3 = bF.trim(b3);
                if (br.JSON && br.JSON.parse) {
                    return br.JSON.parse(b3)
                }
                if (bT.test(b3.replace(bU, "@").replace(bV, "]").replace(bS, ""))) {
                    return (new Function("return " + b3))()
                }
                bF.error("Invalid JSON: " + b3)
            }
            , parseXML: function (b3) {
                var b6, b5;
                try {
                    if (br.DOMParser) {
                        b5 = new DOMParser();
                        b6 = b5.parseFromString(b3, "text/xml")
                    }
                    else {
                        b6 = new ActiveXObject("Microsoft.XMLDOM");
                        b6.async = "false";
                        b6.loadXML(b3)
                    }

                }
                catch (b4) {
                    b6 = bq
                }
                if (!b6 || !b6.documentElement || b6.getElementsByTagName("parsererror").length) {
                    bF.error("Invalid XML: " + b3)
                }
                return b6
            }
            , noop: function () { }, globalEval: function (b3) {
                if (b3 && bO.test(b3)) {
                    (br.execScript || function (b4) {
                        br["eval"].call(br, b4)
                    })(b3)
                }

            }
            , camelCase: function (b3) {
                return b3.replace(bN, "ms-").replace(bI, bC)
            }
            , nodeName: function (b3, b4) {
                return b3.nodeName && b3.nodeName.toUpperCase() === b4.toUpperCase()
            }
            , each: function (b9, b4, b3) {
                var b8, b5 = 0, b7 = b9.length, b6 = b7 === bq || bF.isFunction(b9);
                if (b3) {
                    if (b6) {
                        for (b8 in b9) {
                            if (b4.apply(b9[b8], b3) === false) {
                                break
                            }

                        }

                    }
                    else {
                        for (;
                        b5 < b7;
                        ) {
                            if (b4.apply(b9[b5++], b3) === false) {
                                break
                            }

                        }

                    }

                }
                else {
                    if (b6) {
                        for (b8 in b9) {
                            if (b4.call(b9[b8], b8, b9[b8]) === false) {
                                break
                            }

                        }

                    }
                    else {
                        for (;
                        b5 < b7;
                        ) {
                            if (b4.call(b9[b5], b5, b9[b5++]) === false) {
                                break
                            }

                        }

                    }

                }
                return b9
            }
            , trim: bZ ? function (b3) {
                return b3 == null ? "" : bZ.call(b3)
            }
            : function (b3) {
                return b3 == null ? "" : b3.toString().replace(b0, "").replace(b1, "")
            }
            , makeArray: function (b3, b4) {
                var b5 = b4 || [];
                if (b3 != null) {
                    var b6 = bF.type(b3);
                    if (b3.length == null || b6 === "string" || b6 === "function" || b6 === "regexp" || bF.isWindow(b3)) {
                        bG.call(b5, b3)
                    }
                    else {
                        bF.merge(b5, b3)
                    }

                }
                return b5
            }
            , inArray: function (b4, b3, b5) {
                var b6;
                if (b3) {
                    if (bE) {
                        return bE.call(b3, b4, b5)
                    }
                    b6 = b3.length;
                    b5 = b5 ? b5 < 0 ? Math.max(0, b6 + b5) : b5 : 0;
                    for (;
                    b5 < b6;
                    b5++) {
                        if (b5 in b3 && b3[b5] === b4) {
                            return b5
                        }

                    }

                }
                return -1
            }
            , merge: function (b3, b7) {
                var b4 = b3.length, b5 = 0;
                if (typeof b7.length === "number") {
                    for (var b6 = b7.length;
                    b5 < b6;
                    b5++) {
                        b3[b4++] = b7[b5]
                    }

                }
                else {
                    while (b7[b5] !== bq) {
                        b3[b4++] = b7[b5++]
                    }

                }
                b3.length = b4;
                return b3
            }
            , grep: function (b4, b3, b6) {
                var b8 = [], b9;
                b6 = !!b6;
                for (var b5 = 0, b7 = b4.length;
                b5 < b7;
                b5++) {
                    b9 = !!b3(b4[b5], b5);
                    if (b6 !== b9) {
                        b8.push(b4[b5])
                    }

                }
                return b8
            }
            , map: function (b5, b4, b3) {
                var cb, b8, ca = [], b6 = 0, b9 = b5.length, b7 = b5 instanceof bF || b9 !== bq && typeof b9 === "number" && ((b9 > 0 && b5[0] && b5[b9 - 1]) || b9 === 0 || bF.isArray(b5));
                if (b7) {
                    for (;
                    b6 < b9;
                    b6++) {
                        cb = b4(b5[b6], b6, b3);
                        if (cb != null) {
                            ca[ca.length] = cb
                        }

                    }

                }
                else {
                    for (b8 in b5) {
                        cb = b4(b5[b8], b8, b3);
                        if (cb != null) {
                            ca[ca.length] = cb
                        }

                    }

                }
                return ca.concat.apply([], ca)
            }
            , guid: 1, proxy: function (b5, b4) {
                if (typeof b4 === "string") {
                    var b7 = b5[b4];
                    b4 = b5;
                    b5 = b7
                }
                if (!bF.isFunction(b5)) {
                    return bq
                }
                var b3 = bX.call(arguments, 2), b6 = function () {
                    return b5.apply(b4, b3.concat(bX.call(arguments)))
                };
                b6.guid = b5.guid = b5.guid || b6.guid || bF.guid++;
                return b6
            }
            , access: function (b3, b8, cb, b4, b5, ca) {
                var b9 = b3.length;
                if (typeof b8 === "object") {
                    for (var b7 in b8) {
                        bF.access(b3, b7, b8[b7], b4, b5, cb)
                    }
                    return b3
                }
                if (cb !== bq) {
                    b4 = !ca && b4 && bF.isFunction(cb);
                    for (var b6 = 0;
                    b6 < b9;
                    b6++) {
                        b5(b3[b6], b8, b4 ? cb.call(b3[b6], b6, b5(b3[b6], b8)) : cb, ca)
                    }
                    return b3
                }
                return b9 ? b5(b3[0], b8) : bq
            }
            , now: function () {
                return (new Date()).getTime()
            }
            , uaMatch: function (b4) {
                b4 = b4.toLowerCase();
                var b3 = bW.exec(b4) || bQ.exec(b4) || bM.exec(b4) || b4.indexOf("compatible") < 0 && bL.exec(b4) || [];
                return {
                    browser: b3[1] || "", version: b3[2] || "0"
                }

            }
            , sub: function () {
                function b4(b7, b6) {
                    return new b4.fn.init(b7, b6)
                }
                bF.extend(true, b4, this);
                b4.superclass = this;
                b4.fn = b4.prototype = this();
                b4.fn.constructor = b4;
                b4.sub = this.sub;
                b4.fn.init = function b3(b7, b6) {
                    if (b6 && b6 instanceof bF && !(b6 instanceof b4)) {
                        b6 = b4(b6)
                    }
                    return bF.fn.init.call(this, b7, b6, b5)
                };
                b4.fn.init.prototype = b4.fn;
                var b5 = b4(A);
                return b4
            }
            , browser: {}
        });
        bF.each("Boolean Number String Function Array Date RegExp Object".split(" "), function (b3, b4) {
            bz["[object " + b4 + "]"] = b4.toLowerCase()
        });
        by = bF.uaMatch(b2);
        if (by.browser) {
            bF.browser[by.browser] = true;
            bF.browser.version = by.version
        }
        if (bF.browser.webkit) {
            bF.browser.safari = true
        }
        if (bO.test("\xA0")) {
            b0 = /^[\s\xA0]+/;
            b1 = /[\s\xA0]+$/
        }
        bP = bF(A);
        if (A.addEventListener) {
            bA = function () {
                A.removeEventListener("DOMContentLoaded", bA, false);
                bF.ready()
            }

        }
        else {
            if (A.attachEvent) {
                bA = function () {
                    if (A.readyState === "complete") {
                        A.detachEvent("onreadystatechange", bA);
                        bF.ready()
                    }

                }

            }

        }
        function bB() {
            if (bF.isReady) {
                return
            }
            try {
                A.documentElement.doScroll("left")
            }
            catch (b3) {
                setTimeout(bB, 1);
                return
            }
            bF.ready()
        }
        if (typeof define === "function" && define.amd && define.amd.jQuery) {
            define("jquery", [], function () {
                return bF
            })
        }
        return bF
    })();
    var H = {};
    function p(e) {
        var bz = H[e] = {}, bx, by;
        e = e.split(/\s+/);
        for (bx = 0, by = e.length;
        bx < by;
        bx++) {
            bz[e[bx]] = true
        }
        return bz
    }
    Z.Callbacks = function (bC) {
        bC = bC ? (H[bC] || p(bC)) : {};
        var bD = [], bG = [], bE, by, bB, bA, bz, e = function (bI) {
            var bK, bL, bJ, bM, bH;
            for (bK = 0, bL = bI.length;
            bK < bL;
            bK++) {
                bJ = bI[bK];
                bM = Z.type(bJ);
                if (bM === "array") {
                    e(bJ)
                }
                else {
                    if (bM === "function") {
                        if (!bC.unique || !bF.has(bJ)) {
                            bD.push(bJ)
                        }

                    }

                }

            }

        }
        , bx = function (bI, bH) {
            bH = bH || [];
            bE = !bC.memory || [bI, bH];
            by = true;
            bz = bB || 0;
            bB = 0;
            bA = bD.length;
            for (;
            bD && bz < bA;
            bz++) {
                if (bD[bz].apply(bI, bH) === false && bC.stopOnFalse) {
                    bE = true;
                    break
                }

            }
            by = false;
            if (bD) {
                if (!bC.once) {
                    if (bG && bG.length) {
                        bE = bG.shift();
                        bF.fireWith(bE[0], bE[1])
                    }

                }
                else {
                    if (bE === true) {
                        bF.disable()
                    }
                    else {
                        bD = []
                    }

                }

            }

        }
        , bF = {
            add: function () {
                if (bD) {
                    var bH = bD.length;
                    e(arguments);
                    if (by) {
                        bA = bD.length
                    }
                    else {
                        if (bE && bE !== true) {
                            bB = bH;
                            bx(bE[0], bE[1])
                        }

                    }

                }
                return this
            }
            , remove: function () {
                if (bD) {
                    var bJ = arguments, bH = 0, bI = bJ.length;
                    for (;
                    bH < bI;
                    bH++) {
                        for (var bK = 0;
                        bK < bD.length;
                        bK++) {
                            if (bJ[bH] === bD[bK]) {
                                if (by) {
                                    if (bK <= bA) {
                                        bA--;
                                        if (bK <= bz) {
                                            bz--
                                        }

                                    }

                                }
                                bD.splice(bK--, 1);
                                if (bC.unique) {
                                    break
                                }

                            }

                        }

                    }

                }
                return this
            }
            , has: function (bH) {
                if (bD) {
                    var bI = 0, bJ = bD.length;
                    for (;
                    bI < bJ;
                    bI++) {
                        if (bH === bD[bI]) {
                            return true
                        }

                    }

                }
                return false
            }
            , empty: function () {
                bD = [];
                return this
            }
            , disable: function () {
                bD = bG = bE = bq;
                return this
            }
            , disabled: function () {
                return !bD
            }
            , lock: function () {
                bG = bq;
                if (!bE || bE === true) {
                    bF.disable()
                }
                return this
            }
            , locked: function () {
                return !bG
            }
            , fireWith: function (bI, bH) {
                if (bG) {
                    if (by) {
                        if (!bC.once) {
                            bG.push([bI, bH])
                        }

                    }
                    else {
                        if (!(bC.once && bE)) {
                            bx(bI, bH)
                        }

                    }

                }
                return this
            }
            , fire: function () {
                bF.fireWith(this, arguments);
                return this
            }
            , fired: function () {
                return !!bE
            }

        };
        return bF
    };
    var bn = [].slice;
    Z.extend({
        Deferred: function (bz) {
            var bx = Z.Callbacks("once memory"), by = Z.Callbacks("once memory"), bC = Z.Callbacks("memory"), bE = "pending", bB = {
                resolve: bx, reject: by, notify: bC
            }
            , bD = {
                done: bx.add, fail: by.add, progress: bC.add, state: function () {
                    return bE
                }
                , isResolved: bx.fired, isRejected: by.fired, then: function (bF, bG, bH) {
                    e.done(bF).fail(bG).progress(bH);
                    return this
                }
                , always: function () {
                    return e.done.apply(e, arguments).fail.apply(e, arguments)
                }
                , pipe: function (bF, bG, bH) {
                    return Z.Deferred(function (bI) {
                        Z.each({
                            done: [bF, "resolve"], fail: [bG, "reject"], progress: [bH, "notify"]
                        }
                        , function (bM, bK) {
                            var bL = bK[0], bJ = bK[1], bN;
                            if (Z.isFunction(bL)) {
                                e[bM](function () {
                                    bN = bL.apply(this, arguments);
                                    if (bN && Z.isFunction(bN.promise)) {
                                        bN.promise().then(bI.resolve, bI.reject, bI.notify)
                                    }
                                    else {
                                        bI[bJ + "With"](this === e ? bI : this, [bN])
                                    }

                                })
                            }
                            else {
                                e[bM](bI[bJ])
                            }

                        })
                    }).promise()
                }
                , promise: function (bG) {
                    if (bG == null) {
                        bG = bD
                    }
                    else {
                        for (var bF in bD) {
                            bG[bF] = bD[bF]
                        }

                    }
                    return bG
                }

            }
            , e = bD.promise({}), bA;
            for (bA in bB) {
                e[bA] = bB[bA].fire;
                e[bA + "With"] = bB[bA].fireWith
            }
            e.done(function () {
                bE = "resolved"
            }
            , by.disable, bC.lock).fail(function () {
                bE = "rejected"
            }
            , bx.disable, bC.lock);
            if (bz) {
                bz.call(e, e)
            }
            return e
        }
        , when: function (bz) {
            var e = bn.call(arguments, 0), bA = 0, bB = e.length, bF = new Array(bB), bx = bB, bC = bB, by = bB <= 1 && bz && Z.isFunction(bz.promise) ? bz : Z.Deferred(), bE = by.promise();
            function bG(bH) {
                return function (bI) {
                    e[bH] = arguments.length > 1 ? bn.call(arguments, 0) : bI;
                    if (!(--bx)) {
                        by.resolveWith(by, e)
                    }

                }

            }
            function bD(bH) {
                return function (bI) {
                    bF[bH] = arguments.length > 1 ? bn.call(arguments, 0) : bI;
                    by.notifyWith(bE, bF)
                }

            }
            if (bB > 1) {
                for (;
                bA < bB;
                bA++) {
                    if (e[bA] && e[bA].promise && Z.isFunction(e[bA].promise)) {
                        e[bA].promise().then(bG(bA), by.reject, bD(bA))
                    }
                    else {
                        --bx
                    }

                }
                if (!bx) {
                    by.resolveWith(by, e)
                }

            }
            else {
                if (by !== bz) {
                    by.resolveWith(by, bB ? [bz] : [])
                }

            }
            return bE
        }

    });
    Z.support = (function () {
        var bA = A.createElement("div"), bB = A.documentElement, by, bx, bL, bK, bH, bJ, bM, bF, bz, bP, bO, bQ, bN, bE, bD, bG, bI;
        bA.setAttribute("className", "t");
        bA.innerHTML = "   <link/><table></table><a href='/a' style='top:1px;float:left;opacity:.55;'>a</a><input type='checkbox'/><nav></nav>";
        by = bA.getElementsByTagName("*");
        bx = bA.getElementsByTagName("a")[0];
        if (!by || !by.length || !bx) {
            return {}
        }
        bL = A.createElement("select");
        bK = bL.appendChild(A.createElement("option"));
        bH = bA.getElementsByTagName("input")[0];
        bM = {
            leadingWhitespace: (bA.firstChild.nodeType === 3), tbody: !bA.getElementsByTagName("tbody").length, htmlSerialize: !!bA.getElementsByTagName("link").length, style: /top/.test(bx.getAttribute("style")), hrefNormalized: (bx.getAttribute("href") === "/a"), opacity: /^0.55/.test(bx.style.opacity), cssFloat: !!bx.style.cssFloat, unknownElems: !!bA.getElementsByTagName("nav").length, checkOn: (bH.value === "on"), optSelected: bK.selected, getSetAttribute: bA.className !== "t", enctype: !!A.createElement("form").enctype, submitBubbles: true, changeBubbles: true, focusinBubbles: false, deleteExpando: true, noCloneEvent: true, inlineBlockNeedsLayout: false, shrinkWrapBlocks: false, reliableMarginRight: true
        };
        bH.checked = true;
        bM.noCloneChecked = bH.cloneNode(true).checked;
        bL.disabled = true;
        bM.optDisabled = !bK.disabled;
        try {
            delete bA.test
        }
        catch (bC) {
            bM.deleteExpando = false
        }
        if (!bA.addEventListener && bA.attachEvent && bA.fireEvent) {
            bA.attachEvent("onclick", function () {
                bM.noCloneEvent = false
            });
            bA.cloneNode(true).fireEvent("onclick")
        }
        bH = A.createElement("input");
        bH.value = "t";
        bH.setAttribute("type", "radio");
        bM.radioValue = bH.value === "t";
        bH.setAttribute("checked", "checked");
        bA.appendChild(bH);
        bF = A.createDocumentFragment();
        bF.appendChild(bA.lastChild);
        bM.checkClone = bF.cloneNode(true).cloneNode(true).lastChild.checked;
        bA.innerHTML = "";
        bA.style.width = bA.style.paddingLeft = "1px";
        bz = A.getElementsByTagName("body")[0];
        bO = A.createElement(bz ? "div" : "body");
        bQ = {
            visibility: "hidden", width: 0, height: 0, border: 0, margin: 0, background: "none"
        };
        if (bz) {
            Z.extend(bQ, {
                position: "absolute", left: "-999px", top: "-999px"
            })
        }
        for (bG in bQ) {
            bO.style[bG] = bQ[bG]
        }
        bO.appendChild(bA);
        bP = bz || bB;
        bP.insertBefore(bO, bP.firstChild);
        bM.appendChecked = bH.checked;
        bM.boxModel = bA.offsetWidth === 2;
        if ("zoom" in bA.style) {
            bA.style.display = "inline";
            bA.style.zoom = 1;
            bM.inlineBlockNeedsLayout = (bA.offsetWidth === 2);
            bA.style.display = "";
            bA.innerHTML = "<div style='width:4px;'></div>";
            bM.shrinkWrapBlocks = (bA.offsetWidth !== 2)
        }
        bA.innerHTML = "<table><tr><td style='padding:0;border:0;display:none'></td><td>t</td></tr></table>";
        bN = bA.getElementsByTagName("td");
        bI = (bN[0].offsetHeight === 0);
        bN[0].style.display = "";
        bN[1].style.display = "none";
        bM.reliableHiddenOffsets = bI && (bN[0].offsetHeight === 0);
        bA.innerHTML = "";
        if (A.defaultView && A.defaultView.getComputedStyle) {
            bJ = A.createElement("div");
            bJ.style.width = "0";
            bJ.style.marginRight = "0";
            bA.appendChild(bJ);
            bM.reliableMarginRight = (parseInt((A.defaultView.getComputedStyle(bJ, null) || {
                marginRight: 0
            }).marginRight, 10) || 0) === 0
        }
        if (bA.attachEvent) {
            for (bG in {
                submit: 1, change: 1, focusin: 1
            }) {
                bD = "on" + bG;
                bI = (bD in bA);
                if (!bI) {
                    bA.setAttribute(bD, "return;");
                    bI = (typeof bA[bD] === "function")
                }
                bM[bG + "Bubbles"] = bI
            }

        }
        Z(function () {
            var bR, bV, bT, bY, bZ, bU, e = 1, bW = "position:absolute;top:0;left:0;width:1px;height:1px;margin:0;", b0 = "visibility:hidden;border:0;", bX = "style='" + bW + "border:5px solid #000;padding:0;'", bS = "<div " + bX + "><div></div></div><table " + bX + " cellpadding='0' cellspacing='0'><tr><td></td></tr></table>";
            bz = A.getElementsByTagName("body")[0];
            if (!bz) {
                return
            }
            bR = A.createElement("div");
            bR.style.cssText = b0 + "width:0;height:0;position:static;top:0;margin-top:" + e + "px";
            bz.insertBefore(bR, bz.firstChild);
            bO = A.createElement("div");
            bO.style.cssText = bW + b0;
            bO.innerHTML = bS;
            bR.appendChild(bO);
            bV = bO.firstChild;
            bT = bV.firstChild;
            bZ = bV.nextSibling.firstChild.firstChild;
            bU = {
                doesNotAddBorder: (bT.offsetTop !== 5), doesAddBorderForTableAndCells: (bZ.offsetTop === 5)
            };
            bT.style.position = "fixed";
            bT.style.top = "20px";
            bU.fixedPosition = (bT.offsetTop === 20 || bT.offsetTop === 15);
            bT.style.position = bT.style.top = "";
            bV.style.overflow = "hidden";
            bV.style.position = "relative";
            bU.subtractsBorderForOverflowNotVisible = (bT.offsetTop === -5);
            bU.doesNotIncludeMarginInBodyOffset = (bz.offsetTop !== e);
            bz.removeChild(bR);
            bO = bR = null;
            Z.extend(bM, bU)
        });
        bO.innerHTML = "";
        bP.removeChild(bO);
        bO = bF = bL = bK = bz = bJ = bA = bH = null;
        return bM
    })();
    Z.boxModel = Z.support.boxModel;
    var an = /^(?:\{.*\}|\[.*\])$/, aM = /([A-Z])/g;
    Z.extend({
        cache: {}, uuid: 0, expando: "jQuery" + (Z.fn.jquery + Math.random()).replace(/\D/g, ""), noData: {
            embed: true, object: "clsid:D27CDB6E-AE6D-11cf-96B8-444553540000", applet: true
        }
        , hasData: function (e) {
            e = e.nodeType ? Z.cache[e[Z.expando]] : e[Z.expando];
            return !!e && !X(e)
        }
        , data: function (by, bE, bx, bG) {
            if (!Z.acceptData(by)) {
                return
            }
            var bF, bI, bH, bB = Z.expando, bz = typeof bE === "string", bD = by.nodeType, e = bD ? Z.cache : by, bA = bD ? by[Z.expando] : by[Z.expando] && Z.expando, bC = bE === "events";
            if ((!bA || !e[bA] || (!bC && !bG && !e[bA].data)) && bz && bx === bq) {
                return
            }
            if (!bA) {
                if (bD) {
                    by[Z.expando] = bA = ++Z.uuid
                }
                else {
                    bA = Z.expando
                }

            }
            if (!e[bA]) {
                e[bA] = {};
                if (!bD) {
                    e[bA].toJSON = Z.noop
                }

            }
            if (typeof bE === "object" || typeof bE === "function") {
                if (bG) {
                    e[bA] = Z.extend(e[bA], bE)
                }
                else {
                    e[bA].data = Z.extend(e[bA].data, bE)
                }

            }
            bF = bI = e[bA];
            if (!bG) {
                if (!bI.data) {
                    bI.data = {}
                }
                bI = bI.data
            }
            if (bx !== bq) {
                bI[Z.camelCase(bE)] = bx
            }
            if (bC && !bI[bE]) {
                return bF.events
            }
            if (bz) {
                bH = bI[bE];
                if (bH == null) {
                    bH = bI[Z.camelCase(bE)]
                }

            }
            else {
                bH = bI
            }
            return bH
        }
        , removeData: function (bx, bD, bE) {
            if (!Z.acceptData(bx)) {
                return
            }
            var bF, by, bC, bA = Z.expando, bB = bx.nodeType, e = bB ? Z.cache : bx, bz = bB ? bx[Z.expando] : Z.expando;
            if (!e[bz]) {
                return
            }
            if (bD) {
                bF = bE ? e[bz] : e[bz].data;
                if (bF) {
                    if (Z.isArray(bD)) {
                        bD = bD
                    }
                    else {
                        if (bD in bF) {
                            bD = [bD]
                        }
                        else {
                            bD = Z.camelCase(bD);
                            if (bD in bF) {
                                bD = [bD]
                            }
                            else {
                                bD = bD.split(" ")
                            }

                        }

                    }
                    for (by = 0, bC = bD.length;
                    by < bC;
                    by++) {
                        delete bF[bD[by]]
                    }
                    if (!(bE ? X : Z.isEmptyObject)(bF)) {
                        return
                    }

                }

            }
            if (!bE) {
                delete e[bz].data;
                if (!X(e[bz])) {
                    return
                }

            }
            if (Z.support.deleteExpando || !e.setInterval) {
                delete e[bz]
            }
            else {
                e[bz] = null
            }
            if (bB) {
                if (Z.support.deleteExpando) {
                    delete bx[Z.expando]
                }
                else {
                    if (bx.removeAttribute) {
                        bx.removeAttribute(Z.expando)
                    }
                    else {
                        bx[Z.expando] = null
                    }

                }

            }

        }
        , _data: function (bx, by, e) {
            return Z.data(bx, by, e, true)
        }
        , acceptData: function (e) {
            if (e.nodeName) {
                var bx = Z.noData[e.nodeName.toLowerCase()];
                if (bx) {
                    return !(bx === true || e.getAttribute("classid") !== bx)
                }

            }
            return true
        }

    });
    Z.fn.extend({
        data: function (bz, bD) {
            var bC, e, bB, bx = null;
            if (typeof bz === "undefined") {
                if (this.length) {
                    bx = Z.data(this[0]);
                    if (this[0].nodeType === 1 && !Z._data(this[0], "parsedAttrs")) {
                        e = this[0].attributes;
                        for (var by = 0, bA = e.length;
                        by < bA;
                        by++) {
                            bB = e[by].name;
                            if (bB.indexOf("data-") === 0) {
                                bB = Z.camelCase(bB.substring(5));
                                y(this[0], bB, bx[bB])
                            }

                        }
                        Z._data(this[0], "parsedAttrs", true)
                    }

                }
                return bx
            }
            else {
                if (typeof bz === "object") {
                    return this.each(function () {
                        Z.data(this, bz)
                    })
                }

            }
            bC = bz.split(".");
            bC[1] = bC[1] ? "." + bC[1] : "";
            if (bD === bq) {
                bx = this.triggerHandler("getData" + bC[1] + "!", [bC[0]]);
                if (bx === bq && this.length) {
                    bx = Z.data(this[0], bz);
                    bx = y(this[0], bz, bx)
                }
                return bx === bq && bC[1] ? this.data(bC[0]) : bx
            }
            else {
                return this.each(function () {
                    var bE = Z(this), bF = [bC[0], bD];
                    bE.triggerHandler("setData" + bC[1] + "!", bF);
                    Z.data(this, bz, bD);
                    bE.triggerHandler("changeData" + bC[1] + "!", bF)
                })
            }

        }
        , removeData: function (e) {
            return this.each(function () {
                Z.removeData(this, e)
            })
        }

    });
    function y(bz, bA, bx) {
        if (bx === bq && bz.nodeType === 1) {
            var bB = "data-" + bA.replace(aM, "-$1").toLowerCase();
            bx = bz.getAttribute(bB);
            if (typeof bx === "string") {
                try {
                    bx = bx === "true" ? true : bx === "false" ? false : bx === "null" ? null : Z.isNumeric(bx) ? parseFloat(bx) : an.test(bx) ? Z.parseJSON(bx) : bx
                }
                catch (by) { } Z.data(bz, bA, bx)
            }
            else {
                bx = bq
            }

        }
        return bx
    }
    function X(bx) {
        for (var e in bx) {
            if (e === "data" && Z.isEmptyObject(bx[e])) {
                continue
            }
            if (e !== "toJSON") {
                return false
            }

        }
        return true
    }
    function R(by, bC, bB) {
        var bx = bC + "defer", bA = bC + "queue", bz = bC + "mark", e = Z._data(by, bx);
        if (e && (bB === "queue" || !Z._data(by, bA)) && (bB === "mark" || !Z._data(by, bz))) {
            setTimeout(function () {
                if (!Z._data(by, bA) && !Z._data(by, bz)) {
                    Z.removeData(by, bx, true);
                    e.fire()
                }

            }
            , 0)
        }

    }
    Z.extend({
        _mark: function (e, bx) {
            if (e) {
                bx = (bx || "fx") + "mark";
                Z._data(e, bx, (Z._data(e, bx) || 0) + 1)
            }

        }
        , _unmark: function (by, bx, bA) {
            if (by !== true) {
                bA = bx;
                bx = by;
                by = false
            }
            if (bx) {
                bA = bA || "fx";
                var bz = bA + "mark", e = by ? 0 : ((Z._data(bx, bz) || 1) - 1);
                if (e) {
                    Z._data(bx, bz, e)
                }
                else {
                    Z.removeData(bx, bz, true);
                    R(bx, bA, "mark")
                }

            }

        }
        , queue: function (bx, bz, e) {
            var by;
            if (bx) {
                bz = (bz || "fx") + "queue";
                by = Z._data(bx, bz);
                if (e) {
                    if (!by || Z.isArray(e)) {
                        by = Z._data(bx, bz, Z.makeArray(e))
                    }
                    else {
                        by.push(e)
                    }

                }
                return by || []
            }

        }
        , dequeue: function (e, bA) {
            bA = bA || "fx";
            var bz = Z.queue(e, bA), bx = bz.shift(), by = {};
            if (bx === "inprogress") {
                bx = bz.shift()
            }
            if (bx) {
                if (bA === "fx") {
                    bz.unshift("inprogress")
                }
                Z._data(e, bA + ".run", by);
                bx.call(e, function () {
                    Z.dequeue(e, bA)
                }
                , by)
            }
            if (!bz.length) {
                Z.removeData(e, bA + "queue " + bA + ".run", true);
                R(e, bA, "queue")
            }

        }

    });
    Z.fn.extend({
        queue: function (bx, e) {
            if (typeof bx !== "string") {
                e = bx;
                bx = "fx"
            }
            if (e === bq) {
                return Z.queue(this[0], bx)
            }
            return this.each(function () {
                var by = Z.queue(this, bx, e);
                if (bx === "fx" && by[0] !== "inprogress") {
                    Z.dequeue(this, bx)
                }

            })
        }
        , dequeue: function (e) {
            return this.each(function () {
                Z.dequeue(this, e)
            })
        }
        , delay: function (e, bx) {
            e = Z.fx ? Z.fx.speeds[e] || e : e;
            bx = bx || "fx";
            return this.queue(bx, function (bz, by) {
                var bA = setTimeout(bz, e);
                by.stop = function () {
                    clearTimeout(bA)
                }

            })
        }
        , clearQueue: function (e) {
            return this.queue(e || "fx", [])
        }
        , promise: function (bG, bC) {
            if (typeof bG !== "string") {
                bC = bG;
                bG = bq
            }
            bG = bG || "fx";
            var bx = Z.Deferred(), bz = this, bA = bz.length, e = 1, by = bG + "defer", bD = bG + "queue", bB = bG + "mark", bF;
            function bE() {
                if (!(--e)) {
                    bx.resolveWith(bz, [bz])
                }

            }
            while (bA--) {
                if ((bF = Z.data(bz[bA], by, bq, true) || (Z.data(bz[bA], bD, bq, true) || Z.data(bz[bA], bB, bq, true)) && Z.data(bz[bA], by, Z.Callbacks("once memory"), true))) {
                    e++;
                    bF.add(bE)
                }

            }
            bE();
            return bx.promise()
        }

    });
    var aq = /[\n\t\r]/g, a8 = /\s+/, a3 = /\r/g, bf = /^(?:button|input)$/i, ay = /^(?:button|input|object|select|textarea)$/i, at = /^a(?:rea)?$/i, am = /^(?:autofocus|autoplay|async|checked|controls|defer|disabled|hidden|loop|multiple|open|readonly|required|scoped|selected)$/i, N = Z.support.getSetAttribute, ae, j, G;
    Z.fn.extend({
        attr: function (e, bx) {
            return Z.access(this, e, bx, true, Z.attr)
        }
        , removeAttr: function (e) {
            return this.each(function () {
                Z.removeAttr(this, e)
            })
        }
        , prop: function (e, bx) {
            return Z.access(this, e, bx, true, Z.prop)
        }
        , removeProp: function (e) {
            e = Z.propFix[e] || e;
            return this.each(function () {
                try {
                    this[e] = bq;
                    delete this[e]
                }
                catch (bx) { }
            })
        }
        , addClass: function (bD) {
            var by, bA, bB, bz, bC, e, bx;
            if (Z.isFunction(bD)) {
                return this.each(function (bE) {
                    Z(this).addClass(bD.call(this, bE, this.className))
                })
            }
            if (bD && typeof bD === "string") {
                by = bD.split(a8);
                for (bA = 0, bB = this.length;
                bA < bB;
                bA++) {
                    bz = this[bA];
                    if (bz.nodeType === 1) {
                        if (!bz.className && by.length === 1) {
                            bz.className = bD
                        }
                        else {
                            bC = " " + bz.className + " ";
                            for (e = 0, bx = by.length;
                            e < bx;
                            e++) {
                                if (!~bC.indexOf(" " + by[e] + " ")) {
                                    bC += by[e] + " "
                                }

                            }
                            bz.className = Z.trim(bC)
                        }

                    }

                }

            }
            return this
        }
        , removeClass: function (bD) {
            var bz, bB, bC, bA, by, e, bx;
            if (Z.isFunction(bD)) {
                return this.each(function (bE) {
                    Z(this).removeClass(bD.call(this, bE, this.className))
                })
            }
            if ((bD && typeof bD === "string") || bD === bq) {
                bz = (bD || "").split(a8);
                for (bB = 0, bC = this.length;
                bB < bC;
                bB++) {
                    bA = this[bB];
                    if (bA.nodeType === 1 && bA.className) {
                        if (bD) {
                            by = (" " + bA.className + " ").replace(aq, " ");
                            for (e = 0, bx = bz.length;
                            e < bx;
                            e++) {
                                by = by.replace(" " + bz[e] + " ", " ")
                            }
                            bA.className = Z.trim(by)
                        }
                        else {
                            bA.className = ""
                        }

                    }

                }

            }
            return this
        }
        , toggleClass: function (bz, bx) {
            var by = typeof bz, e = typeof bx === "boolean";
            if (Z.isFunction(bz)) {
                return this.each(function (bA) {
                    Z(this).toggleClass(bz.call(this, bA, this.className, bx), bx)
                })
            }
            return this.each(function () {
                if (by === "string") {
                    var bA, bC = 0, bD = Z(this), bE = bx, bB = bz.split(a8);
                    while ((bA = bB[bC++])) {
                        bE = e ? bE : !bD.hasClass(bA);
                        bD[bE ? "addClass" : "removeClass"](bA)
                    }

                }
                else {
                    if (by === "undefined" || by === "boolean") {
                        if (this.className) {
                            Z._data(this, "__className__", this.className)
                        }
                        this.className = this.className || bz === false ? "" : Z._data(this, "__className__") || ""
                    }

                }

            })
        }
        , hasClass: function (bz) {
            var e = " " + bz + " ", bx = 0, by = this.length;
            for (;
            bx < by;
            bx++) {
                if (this[bx].nodeType === 1 && (" " + this[bx].className + " ").replace(aq, " ").indexOf(e) > -1) {
                    return true
                }

            }
            return false
        }
        , val: function (bA) {
            var bx, bz, by, e = this[0];
            if (!arguments.length) {
                if (e) {
                    bx = Z.valHooks[e.nodeName.toLowerCase()] || Z.valHooks[e.type];
                    if (bx && "get" in bx && (bz = bx.get(e, "value")) !== bq) {
                        return bz
                    }
                    bz = e.value;
                    return typeof bz === "string" ? bz.replace(a3, "") : bz == null ? "" : bz
                }
                return bq
            }
            by = Z.isFunction(bA);
            return this.each(function (bB) {
                var bC = Z(this), bD;
                if (this.nodeType !== 1) {
                    return
                }
                if (by) {
                    bD = bA.call(this, bB, bC.val())
                }
                else {
                    bD = bA
                }
                if (bD == null) {
                    bD = ""
                }
                else {
                    if (typeof bD === "number") {
                        bD += ""
                    }
                    else {
                        if (Z.isArray(bD)) {
                            bD = Z.map(bD, function (bE) {
                                return bE == null ? "" : bE + ""
                            })
                        }

                    }

                }
                bx = Z.valHooks[this.nodeName.toLowerCase()] || Z.valHooks[this.type];
                if (!bx || !("set" in bx) || bx.set(this, bD, "value") === bq) {
                    this.value = bD
                }

            })
        }

    });
    Z.extend({
        valHooks: {
            option: {
                get: function (e) {
                    var bx = e.attributes.value;
                    return !bx || bx.specified ? e.value : e.text
                }

            }
            , select: {
                get: function (e) {
                    var bD, bx, bz, bB, by = e.selectedIndex, bE = [], bC = e.options, bA = e.type === "select-one";
                    if (by < 0) {
                        return null
                    }
                    bx = bA ? by : 0;
                    bz = bA ? by + 1 : bC.length;
                    for (;
                    bx < bz;
                    bx++) {
                        bB = bC[bx];
                        if (bB.selected && (Z.support.optDisabled ? !bB.disabled : bB.getAttribute("disabled") === null) && (!bB.parentNode.disabled || !Z.nodeName(bB.parentNode, "optgroup"))) {
                            bD = Z(bB).val();
                            if (bA) {
                                return bD
                            }
                            bE.push(bD)
                        }

                    }
                    if (bA && !bE.length && bC.length) {
                        return Z(bC[by]).val()
                    }
                    return bE
                }
                , set: function (e, bx) {
                    var by = Z.makeArray(bx);
                    Z(e).find("option").each(function () {
                        this.selected = Z.inArray(Z(this).val(), by) >= 0
                    });
                    if (!by.length) {
                        e.selectedIndex = -1
                    }
                    return by
                }

            }

        }
        , attrFn: {
            val: true, css: true, html: true, text: true, data: true, width: true, height: true, offset: true
        }
        , attr: function (e, by, bD, bB) {
            var bC, bx, bz, bA = e.nodeType;
            if (!e || bA === 3 || bA === 8 || bA === 2) {
                return bq
            }
            if (bB && by in Z.attrFn) {
                return Z(e)[by](bD)
            }
            if (!("getAttribute" in e)) {
                return Z.prop(e, by, bD)
            }
            bz = bA !== 1 || !Z.isXMLDoc(e);
            if (bz) {
                by = by.toLowerCase();
                bx = Z.attrHooks[by] || (am.test(by) ? j : ae)
            }
            if (bD !== bq) {
                if (bD === null) {
                    Z.removeAttr(e, by);
                    return bq
                }
                else {
                    if (bx && "set" in bx && bz && (bC = bx.set(e, bD, by)) !== bq) {
                        return bC
                    }
                    else {
                        e.setAttribute(by, "" + bD);
                        return bD
                    }

                }

            }
            else {
                if (bx && "get" in bx && bz && (bC = bx.get(e, by)) !== null) {
                    return bC
                }
                else {
                    bC = e.getAttribute(by);
                    return bC === null ? bq : bC
                }

            }

        }
        , removeAttr: function (bx, bC) {
            var bB, e, bA, bz, by = 0;
            if (bx.nodeType === 1) {
                e = (bC || "").split(a8);
                bz = e.length;
                for (;
                by < bz;
                by++) {
                    bA = e[by].toLowerCase();
                    bB = Z.propFix[bA] || bA;
                    Z.attr(bx, bA, "");
                    bx.removeAttribute(N ? bA : bB);
                    if (am.test(bA) && bB in bx) {
                        bx[bB] = false
                    }

                }

            }

        }
        , attrHooks: {
            type: {
                set: function (e, by) {
                    if (bf.test(e.nodeName) && e.parentNode) {
                        Z.error("type property can't be changed")
                    }
                    else {
                        if (!Z.support.radioValue && by === "radio" && Z.nodeName(e, "input")) {
                            var bx = e.value;
                            e.setAttribute("type", by);
                            if (bx) {
                                e.value = bx
                            }
                            return by
                        }

                    }

                }

            }
            , value: {
                get: function (e, bx) {
                    if (ae && Z.nodeName(e, "button")) {
                        return ae.get(e, bx)
                    }
                    return bx in e ? e.value : null
                }
                , set: function (e, by, bx) {
                    if (ae && Z.nodeName(e, "button")) {
                        return ae.set(e, by, bx)
                    }
                    e.value = by
                }

            }

        }
        , propFix: {
            tabindex: "tabIndex", readonly: "readOnly", "for": "htmlFor", "class": "className", maxlength: "maxLength", cellspacing: "cellSpacing", cellpadding: "cellPadding", rowspan: "rowSpan", colspan: "colSpan", usemap: "useMap", frameborder: "frameBorder", contenteditable: "contentEditable"
        }
        , prop: function (e, by, bC) {
            var bB, bx, bz, bA = e.nodeType;
            if (!e || bA === 3 || bA === 8 || bA === 2) {
                return bq
            }
            bz = bA !== 1 || !Z.isXMLDoc(e);
            if (bz) {
                by = Z.propFix[by] || by;
                bx = Z.propHooks[by]
            }
            if (bC !== bq) {
                if (bx && "set" in bx && (bB = bx.set(e, bC, by)) !== bq) {
                    return bB
                }
                else {
                    return (e[by] = bC)
                }

            }
            else {
                if (bx && "get" in bx && (bB = bx.get(e, by)) !== null) {
                    return bB
                }
                else {
                    return e[by]
                }

            }

        }
        , propHooks: {
            tabIndex: {
                get: function (bx) {
                    var e = bx.getAttributeNode("tabindex");
                    return e && e.specified ? parseInt(e.value, 10) : ay.test(bx.nodeName) || at.test(bx.nodeName) && bx.href ? 0 : bq
                }

            }

        }

    });
    Z.attrHooks.tabindex = Z.propHooks.tabIndex;
    j = {
        get: function (bx, by) {
            var e, bz = Z.prop(bx, by);
            return bz === true || typeof bz !== "boolean" && (e = bx.getAttributeNode(by)) && e.nodeValue !== false ? by.toLowerCase() : bq
        }
        , set: function (e, bz, bx) {
            var by;
            if (bz === false) {
                Z.removeAttr(e, bx)
            }
            else {
                by = Z.propFix[bx] || bx;
                if (by in e) {
                    e[by] = true
                }
                e.setAttribute(bx, bx.toLowerCase())
            }
            return bx
        }

    };
    if (!N) {
        G = {
            name: true, id: true
        };
        ae = Z.valHooks.button = {
            get: function (e, bx) {
                var by;
                by = e.getAttributeNode(bx);
                return by && (G[bx] ? by.nodeValue !== "" : by.specified) ? by.nodeValue : bq
            }
            , set: function (e, bz, bx) {
                var by = e.getAttributeNode(bx);
                if (!by) {
                    by = A.createAttribute(bx);
                    e.setAttributeNode(by)
                }
                return (by.nodeValue = bz + "")
            }

        };
        Z.attrHooks.tabindex.set = ae.set;
        Z.each(["width", "height"], function (e, bx) {
            Z.attrHooks[bx] = Z.extend(Z.attrHooks[bx], {
                set: function (by, bz) {
                    if (bz === "") {
                        by.setAttribute(bx, "auto");
                        return bz
                    }

                }

            })
        });
        Z.attrHooks.contenteditable = {
            get: ae.get, set: function (e, by, bx) {
                if (by === "") {
                    by = "false"
                }
                ae.set(e, by, bx)
            }

        }

    }
    if (!Z.support.hrefNormalized) {
        Z.each(["href", "src", "width", "height"], function (e, bx) {
            Z.attrHooks[bx] = Z.extend(Z.attrHooks[bx], {
                get: function (by) {
                    var bz = by.getAttribute(bx, 2);
                    return bz === null ? bq : bz
                }

            })
        })
    }
    if (!Z.support.style) {
        Z.attrHooks.style = {
            get: function (e) {
                return e.style.cssText.toLowerCase() || bq
            }
            , set: function (e, bx) {
                return (e.style.cssText = "" + bx)
            }

        }

    }
    if (!Z.support.optSelected) {
        Z.propHooks.selected = Z.extend(Z.propHooks.selected, {
            get: function (e) {
                var bx = e.parentNode;
                if (bx) {
                    bx.selectedIndex;
                    if (bx.parentNode) {
                        bx.parentNode.selectedIndex
                    }

                }
                return null
            }

        })
    }
    if (!Z.support.enctype) {
        Z.propFix.enctype = "encoding"
    }
    if (!Z.support.checkOn) {
        Z.each(["radio", "checkbox"], function () {
            Z.valHooks[this] = {
                get: function (e) {
                    return e.getAttribute("value") === null ? "on" : e.value
                }

            }

        })
    }
    Z.each(["radio", "checkbox"], function () {
        Z.valHooks[this] = Z.extend(Z.valHooks[this], {
            set: function (e, bx) {
                if (Z.isArray(bx)) {
                    return (e.checked = Z.inArray(Z(e).val(), bx) >= 0)
                }

            }

        })
    });
    var aO = /\.(.*)$/, az = /^(?:textarea|input|select)$/i, aY = /\./g, a9 = / /g, av = /[^\w\s.|`]/g, bg = /^([^\.]*)?(?:\.(.+))?$/, aE = /\bhover(\.\S+)?/, aI = /^key/, aL = /^(?:mouse|contextmenu)|click/, a1 = /^(\w*)(?:#([\w\-]+))?(?:\.([\w\-]+))?$/, aj = function (bx) {
        var e = a1.exec(bx);
        if (e) {
            e[1] = (e[1] || "").toLowerCase();
            e[3] = e[3] && new RegExp("(?:^|\\s)" + e[3] + "(?:\\s|$)")
        }
        return e
    }
    , ai = function (e, bx) {
        return ((!bx[1] || e.nodeName.toLowerCase() === bx[1]) && (!bx[2] || e.id === bx[2]) && (!bx[3] || bx[3].test(e.className)))
    }
    , S = function (e) {
        return Z.event.special.hover ? e : e.replace(aE, "mouseenter$1 mouseleave$1")
    };
    Z.event = {
        add: function (bx, bM, bD, e, bH) {
            var by, bz, bA, bJ, bK, bL, bF, bB, bC, bG, bE, bI;
            if (bx.nodeType === 3 || bx.nodeType === 8 || !bM || !bD || !(by = Z._data(bx))) {
                return
            }
            if (bD.handler) {
                bC = bD;
                bD = bC.handler
            }
            if (!bD.guid) {
                bD.guid = Z.guid++
            }
            bA = by.events;
            if (!bA) {
                by.events = bA = {}
            }
            bz = by.handle;
            if (!bz) {
                by.handle = bz = function (bN) {
                    return typeof Z !== "undefined" && (!bN || Z.event.triggered !== bN.type) ? Z.event.dispatch.apply(bz.elem, arguments) : bq
                };
                bz.elem = bx
            }
            bM = S(bM).split(" ");
            for (bJ = 0;
            bJ < bM.length;
            bJ++) {
                bK = bg.exec(bM[bJ]) || [];
                bL = bK[1];
                bF = (bK[2] || "").split(".").sort();
                bI = Z.event.special[bL] || {};
                bL = (bH ? bI.delegateType : bI.bindType) || bL;
                bI = Z.event.special[bL] || {};
                bB = Z.extend({
                    type: bL, origType: bK[1], data: e, handler: bD, guid: bD.guid, selector: bH, namespace: bF.join(".")
                }
                , bC);
                if (bH) {
                    bB.quick = aj(bH);
                    if (!bB.quick && Z.expr.match.POS.test(bH)) {
                        bB.isPositional = true
                    }

                }
                bE = bA[bL];
                if (!bE) {
                    bE = bA[bL] = [];
                    bE.delegateCount = 0;
                    if (!bI.setup || bI.setup.call(bx, e, bF, bz) === false) {
                        if (bx.addEventListener) {
                            bx.addEventListener(bL, bz, false)
                        }
                        else {
                            if (bx.attachEvent) {
                                bx.attachEvent("on" + bL, bz)
                            }

                        }

                    }

                }
                if (bI.add) {
                    bI.add.call(bx, bB);
                    if (!bB.handler.guid) {
                        bB.handler.guid = bD.guid
                    }

                }
                if (bH) {
                    bE.splice(bE.delegateCount++, 0, bB)
                }
                else {
                    bE.push(bB)
                }
                Z.event.global[bL] = true
            }
            bx = null
        }
        , global: {}, remove: function (e, bL, bC, bG) {
            var bx = Z.hasData(e) && Z._data(e), bI, bJ, bK, bE, bF, bD, by, bH, bA, bz, bB;
            if (!bx || !(by = bx.events)) {
                return
            }
            bL = S(bL || "").split(" ");
            for (bI = 0;
            bI < bL.length;
            bI++) {
                bJ = bg.exec(bL[bI]) || [];
                bK = bJ[1];
                bE = bJ[2];
                if (!bK) {
                    bE = bE ? "." + bE : "";
                    for (bD in by) {
                        Z.event.remove(e, bD + bE, bC, bG)
                    }
                    return
                }
                bH = Z.event.special[bK] || {};
                bK = (bG ? bH.delegateType : bH.bindType) || bK;
                bz = by[bK] || [];
                bF = bz.length;
                bE = bE ? new RegExp("(^|\\.)" + bE.split(".").sort().join("\\.(?:.*\\.)?") + "(\\.|$)") : null;
                if (bC || bE || bG || bH.remove) {
                    for (bD = 0;
                    bD < bz.length;
                    bD++) {
                        bB = bz[bD];
                        if (!bC || bC.guid === bB.guid) {
                            if (!bE || bE.test(bB.namespace)) {
                                if (!bG || bG === bB.selector || bG === "**" && bB.selector) {
                                    bz.splice(bD--, 1);
                                    if (bB.selector) {
                                        bz.delegateCount--
                                    }
                                    if (bH.remove) {
                                        bH.remove.call(e, bB)
                                    }

                                }

                            }

                        }

                    }

                }
                else {
                    bz.length = 0
                }
                if (bz.length === 0 && bF !== bz.length) {
                    if (!bH.teardown || bH.teardown.call(e, bE) === false) {
                        Z.removeEvent(e, bK, bx.handle)
                    }
                    delete by[bK]
                }

            }
            if (Z.isEmptyObject(by)) {
                bA = bx.handle;
                if (bA) {
                    bA.elem = null
                }
                Z.removeData(e, ["events", "handle"], true)
            }

        }
        , customEvent: {
            getData: true, setData: true, changeData: true
        }
        , trigger: function (bB, bz, bA, bI) {
            if (bA && (bA.nodeType === 3 || bA.nodeType === 8)) {
                return
            }
            var bL = bB.type || bB, bG = [], bx, bD, bF, by, bH, bJ, bK, bE, bC, e;
            if (bL.indexOf("!") >= 0) {
                bL = bL.slice(0, -1);
                bD = true
            }
            if (bL.indexOf(".") >= 0) {
                bG = bL.split(".");
                bL = bG.shift();
                bG.sort()
            }
            if ((!bA || Z.event.customEvent[bL]) && !Z.event.global[bL]) {
                return
            }
            bB = typeof bB === "object" ? bB[Z.expando] ? bB : new Z.Event(bL, bB) : new Z.Event(bL);
            bB.type = bL;
            bB.isTrigger = true;
            bB.exclusive = bD;
            bB.namespace = bG.join(".");
            bB.namespace_re = bB.namespace ? new RegExp("(^|\\.)" + bG.join("\\.(?:.*\\.)?") + "(\\.|$)") : null;
            bJ = bL.indexOf(":") < 0 ? "on" + bL : "";
            if (bI || !bA) {
                bB.preventDefault()
            }
            if (!bA) {
                bx = Z.cache;
                for (bF in bx) {
                    if (bx[bF].events && bx[bF].events[bL]) {
                        Z.event.trigger(bB, bz, bx[bF].handle.elem, true)
                    }

                }
                return
            }
            bB.result = bq;
            if (!bB.target) {
                bB.target = bA
            }
            bz = bz != null ? Z.makeArray(bz) : [];
            bz.unshift(bB);
            bK = Z.event.special[bL] || {};
            if (bK.trigger && bK.trigger.apply(bA, bz) === false) {
                return
            }
            bC = [[bA, bK.bindType || bL]];
            if (!bI && !bK.noBubble && !Z.isWindow(bA)) {
                e = bK.delegateType || bL;
                bH = null;
                for (by = bA.parentNode;
                by;
                by = by.parentNode) {
                    bC.push([by, e]);
                    bH = by
                }
                if (bH && bH === bA.ownerDocument) {
                    bC.push([bH.defaultView || bH.parentWindow || br, e])
                }

            }
            for (bF = 0;
            bF < bC.length;
            bF++) {
                by = bC[bF][0];
                bB.type = bC[bF][1];
                bE = (Z._data(by, "events") || {})[bB.type] && Z._data(by, "handle");
                if (bE) {
                    bE.apply(by, bz)
                }
                bE = bJ && by[bJ];
                if (bE && Z.acceptData(by)) {
                    bE.apply(by, bz)
                }
                if (bB.isPropagationStopped()) {
                    break
                }

            }
            bB.type = bL;
            if (!bB.isDefaultPrevented()) {
                if ((!bK._default || bK._default.apply(bA.ownerDocument, bz) === false) && !(bL === "click" && Z.nodeName(bA, "a")) && Z.acceptData(bA)) {
                    if (bJ && bA[bL] && ((bL !== "focus" && bL !== "blur") || bB.target.offsetWidth !== 0) && !Z.isWindow(bA)) {
                        bH = bA[bJ];
                        if (bH) {
                            bA[bJ] = null
                        }
                        Z.event.triggered = bL;
                        bA[bL]();
                        Z.event.triggered = bq;
                        if (bH) {
                            bA[bJ] = bH
                        }

                    }

                }

            }
            return bB.result
        }
        , dispatch: function (bz) {
            bz = Z.event.fix(bz || br.event);
            var bC = ((Z._data(this, "events") || {})[bz.type] || []), by = bC.delegateCount, e = [].slice.call(arguments, 0), bK = !bz.exclusive && !bz.namespace, bN = (Z.event.special[bz.type] || {}).handle, bB = [], bE, bF, bx, bJ, bM, bG, bH, bA, bL, bD, bI;
            e[0] = bz;
            bz.delegateTarget = this;
            if (by && !bz.target.disabled && !(bz.button && bz.type === "click")) {
                for (bx = bz.target;
                bx != this;
                bx = bx.parentNode || this) {
                    bM = {};
                    bH = [];
                    for (bE = 0;
                    bE < by;
                    bE++) {
                        bA = bC[bE];
                        bL = bA.selector;
                        bD = bM[bL];
                        if (bA.isPositional) {
                            bD = (bD || (bM[bL] = Z(bL))).index(bx) >= 0
                        }
                        else {
                            if (bD === bq) {
                                bD = bM[bL] = (bA.quick ? ai(bx, bA.quick) : Z(bx).is(bL))
                            }

                        }
                        if (bD) {
                            bH.push(bA)
                        }

                    }
                    if (bH.length) {
                        bB.push({
                            elem: bx, matches: bH
                        })
                    }

                }

            }
            if (bC.length > by) {
                bB.push({
                    elem: this, matches: bC.slice(by)
                })
            }
            for (bE = 0;
            bE < bB.length && !bz.isPropagationStopped() ;
            bE++) {
                bG = bB[bE];
                bz.currentTarget = bG.elem;
                for (bF = 0;
                bF < bG.matches.length && !bz.isImmediatePropagationStopped() ;
                bF++) {
                    bA = bG.matches[bF];
                    if (bK || (!bz.namespace && !bA.namespace) || bz.namespace_re && bz.namespace_re.test(bA.namespace)) {
                        bz.data = bA.data;
                        bz.handleObj = bA;
                        bJ = (bN || bA.handler).apply(bG.elem, e);
                        if (bJ !== bq) {
                            bz.result = bJ;
                            if (bJ === false) {
                                bz.preventDefault();
                                bz.stopPropagation()
                            }

                        }

                    }

                }

            }
            return bz.result
        }
        , props: "attrChange attrName relatedNode srcElement altKey bubbles cancelable ctrlKey currentTarget eventPhase metaKey relatedTarget shiftKey target timeStamp view which".split(" "), fixHooks: {}, keyHooks: {
            props: "char charCode key keyCode".split(" "), filter: function (e, bx) {
                if (e.which == null) {
                    e.which = bx.charCode != null ? bx.charCode : bx.keyCode
                }
                return e
            }

        }
        , mouseHooks: {
            props: "button buttons clientX clientY fromElement offsetX offsetY pageX pageY screenX screenY toElement wheelDelta".split(" "), filter: function (bz, bC) {
                var bA, by, e, bx = bC.button, bB = bC.fromElement;
                if (bz.pageX == null && bC.clientX != null) {
                    bA = bz.target.ownerDocument || A;
                    by = bA.documentElement;
                    e = bA.body;
                    bz.pageX = bC.clientX + (by && by.scrollLeft || e && e.scrollLeft || 0) - (by && by.clientLeft || e && e.clientLeft || 0);
                    bz.pageY = bC.clientY + (by && by.scrollTop || e && e.scrollTop || 0) - (by && by.clientTop || e && e.clientTop || 0)
                }
                if (!bz.relatedTarget && bB) {
                    bz.relatedTarget = bB === bz.target ? bC.toElement : bB
                }
                if (!bz.which && bx !== bq) {
                    bz.which = (bx & 1 ? 1 : (bx & 2 ? 3 : (bx & 4 ? 2 : 0)))
                }
                return bz
            }

        }
        , fix: function (bx) {
            if (bx[Z.expando]) {
                return bx
            }
            var bz, bB, bA = bx, by = Z.event.fixHooks[bx.type] || {}, e = by.props ? this.props.concat(by.props) : this.props;
            bx = Z.Event(bA);
            for (bz = e.length;
            bz;
            ) {
                bB = e[--bz];
                bx[bB] = bA[bB]
            }
            if (!bx.target) {
                bx.target = bA.srcElement || A
            }
            if (bx.target.nodeType === 3) {
                bx.target = bx.target.parentNode
            }
            if (bx.metaKey === bq) {
                bx.metaKey = bx.ctrlKey
            }
            return by.filter ? by.filter(bx, bA) : bx
        }
        , special: {
            ready: {
                setup: Z.bindReady
            }
            , focus: {
                delegateType: "focusin", noBubble: true
            }
            , blur: {
                delegateType: "focusout", noBubble: true
            }
            , beforeunload: {
                setup: function (e, by, bx) {
                    if (Z.isWindow(this)) {
                        this.onbeforeunload = bx
                    }

                }
                , teardown: function (bx, e) {
                    if (this.onbeforeunload === e) {
                        this.onbeforeunload = null
                    }

                }

            }

        }
        , simulate: function (bB, bz, bA, bx) {
            var by = Z.extend(new Z.Event(), bA, {
                type: bB, isSimulated: true, originalEvent: {}
            });
            if (bx) {
                Z.event.trigger(by, null, bz)
            }
            else {
                Z.event.dispatch.call(bz, by)
            }
            if (by.isDefaultPrevented()) {
                bA.preventDefault()
            }

        }

    };
    Z.event.handle = Z.event.dispatch;
    Z.removeEvent = A.removeEventListener ? function (e, by, bx) {
        if (e.removeEventListener) {
            e.removeEventListener(by, bx, false)
        }

    }
    : function (e, by, bx) {
        if (e.detachEvent) {
            e.detachEvent("on" + by, bx)
        }

    };
    Z.Event = function (bx, e) {
        if (!(this instanceof Z.Event)) {
            return new Z.Event(bx, e)
        }
        if (bx && bx.type) {
            this.originalEvent = bx;
            this.type = bx.type;
            this.isDefaultPrevented = (bx.defaultPrevented || bx.returnValue === false || bx.getPreventDefault && bx.getPreventDefault()) ? ax : aw
        }
        else {
            this.type = bx
        }
        if (e) {
            Z.extend(this, e)
        }
        this.timeStamp = bx && bx.timeStamp || Z.now();
        this[Z.expando] = true
    };
    function aw() {
        return false
    }
    function ax() {
        return true
    }
    Z.Event.prototype = {
        preventDefault: function () {
            this.isDefaultPrevented = ax;
            var bx = this.originalEvent;
            if (!bx) {
                return
            }
            if (bx.preventDefault) {
                bx.preventDefault()
            }
            else {
                bx.returnValue = false
            }

        }
        , stopPropagation: function () {
            this.isPropagationStopped = ax;
            var bx = this.originalEvent;
            if (!bx) {
                return
            }
            if (bx.stopPropagation) {
                bx.stopPropagation()
            }
            bx.cancelBubble = true
        }
        , stopImmediatePropagation: function () {
            this.isImmediatePropagationStopped = ax;
            this.stopPropagation()
        }
        , isDefaultPrevented: aw, isPropagationStopped: aw, isImmediatePropagationStopped: aw
    };
    Z.each({
        mouseenter: "mouseover", mouseleave: "mouseout"
    }
    , function (bx, e) {
        Z.event.special[bx] = Z.event.special[e] = {
            delegateType: e, bindType: e, handle: function (by) {
                var bE = this, bB = by.relatedTarget, bz = by.handleObj, bD = bz.selector, bA, bC;
                if (!bB || bz.origType === by.type || (bB !== bE && !Z.contains(bE, bB))) {
                    bA = by.type;
                    by.type = bz.origType;
                    bC = bz.handler.apply(this, arguments);
                    by.type = bA
                }
                return bC
            }

        }

    });
    if (!Z.support.submitBubbles) {
        Z.event.special.submit = {
            setup: function () {
                if (Z.nodeName(this, "form")) {
                    return false
                }
                Z.event.add(this, "click._submit keypress._submit", function (bx) {
                    var by = bx.target, bz = Z.nodeName(by, "input") || Z.nodeName(by, "button") ? by.form : bq;
                    if (bz && !bz._submit_attached) {
                        Z.event.add(bz, "submit._submit", function (e) {
                            if (this.parentNode) {
                                Z.event.simulate("submit", this.parentNode, e, true)
                            }

                        });
                        bz._submit_attached = true
                    }

                })
            }
            , teardown: function () {
                if (Z.nodeName(this, "form")) {
                    return false
                }
                Z.event.remove(this, "._submit")
            }

        }

    }
    if (!Z.support.changeBubbles) {
        Z.event.special.change = {
            setup: function () {
                if (az.test(this.nodeName)) {
                    if (this.type === "checkbox" || this.type === "radio") {
                        Z.event.add(this, "propertychange._change", function (e) {
                            if (e.originalEvent.propertyName === "checked") {
                                this._just_changed = true
                            }

                        });
                        Z.event.add(this, "click._change", function (e) {
                            if (this._just_changed) {
                                this._just_changed = false;
                                Z.event.simulate("change", this, e, true)
                            }

                        })
                    }
                    return false
                }
                Z.event.add(this, "beforeactivate._change", function (bx) {
                    var by = bx.target;
                    if (az.test(by.nodeName) && !by._change_attached) {
                        Z.event.add(by, "change._change", function (e) {
                            if (this.parentNode && !e.isSimulated) {
                                Z.event.simulate("change", this.parentNode, e, true)
                            }

                        });
                        by._change_attached = true
                    }

                })
            }
            , handle: function (bx) {
                var e = bx.target;
                if (this !== e || bx.isSimulated || bx.isTrigger || (e.type !== "radio" && e.type !== "checkbox")) {
                    return bx.handleObj.handler.apply(this, arguments)
                }

            }
            , teardown: function () {
                Z.event.remove(this, "._change");
                return az.test(this.nodeName)
            }

        }

    }
    if (!Z.support.focusinBubbles) {
        Z.each({
            focus: "focusin", blur: "focusout"
        }
        , function (bz, bx) {
            var e = 0, by = function (bA) {
                Z.event.simulate(bx, bA.target, Z.event.fix(bA), true)
            };
            Z.event.special[bx] = {
                setup: function () {
                    if (e++ === 0) {
                        A.addEventListener(bz, by, true)
                    }

                }
                , teardown: function () {
                    if (--e === 0) {
                        A.removeEventListener(bz, by, true)
                    }

                }

            }

        })
    }
    Z.fn.extend({
        on: function (bC, bA, e, bx, by) {
            var bz, bB;
            if (typeof bC === "object") {
                if (typeof bA !== "string") {
                    e = bA;
                    bA = bq
                }
                for (bB in bC) {
                    this.on(bB, bA, e, bC[bB], by)
                }
                return this
            }
            if (e == null && bx == null) {
                bx = bA;
                e = bA = bq
            }
            else {
                if (bx == null) {
                    if (typeof bA === "string") {
                        bx = e;
                        e = bq
                    }
                    else {
                        bx = e;
                        e = bA;
                        bA = bq
                    }

                }

            }
            if (bx === false) {
                bx = aw
            }
            else {
                if (!bx) {
                    return this
                }

            }
            if (by === 1) {
                bz = bx;
                bx = function (bD) {
                    Z().off(bD);
                    return bz.apply(this, arguments)
                };
                bx.guid = bz.guid || (bz.guid = Z.guid++)
            }
            return this.each(function () {
                Z.event.add(this, bC, bx, e, bA)
            })
        }
        , one: function (bz, by, e, bx) {
            return this.on.call(this, bz, by, e, bx, 1)
        }
        , off: function (bA, by, e) {
            if (bA && bA.preventDefault && bA.handleObj) {
                var bx = bA.handleObj;
                Z(bA.delegateTarget).off(bx.namespace ? bx.type + "." + bx.namespace : bx.type, bx.selector, bx.handler);
                return this
            }
            if (typeof bA === "object") {
                for (var bz in bA) {
                    this.off(bz, by, bA[bz])
                }
                return this
            }
            if (by === false || typeof by === "function") {
                e = by;
                by = bq
            }
            if (e === false) {
                e = aw
            }
            return this.each(function () {
                Z.event.remove(this, bA, e, by)
            })
        }
        , bind: function (by, e, bx) {
            return this.on(by, null, e, bx)
        }
        , unbind: function (bx, e) {
            return this.off(bx, null, e)
        }
        , live: function (by, e, bx) {
            Z(this.context).on(by, this.selector, e, bx);
            return this
        }
        , die: function (bx, e) {
            Z(this.context).off(bx, this.selector || "**", e);
            return this
        }
        , delegate: function (by, bz, e, bx) {
            return this.on(bz, by, e, bx)
        }
        , undelegate: function (bx, by, e) {
            return arguments.length == 1 ? this.off(bx, "**") : this.off(by, bx, e)
        }
        , trigger: function (bx, e) {
            return this.each(function () {
                Z.event.trigger(bx, e, this)
            })
        }
        , triggerHandler: function (bx, e) {
            if (this[0]) {
                return Z.event.trigger(bx, e, this[0], true)
            }

        }
        , toggle: function (bx) {
            var e = arguments, by = bx.guid || Z.guid++, bz = 0, bA = function (bB) {
                var bC = (Z._data(this, "lastToggle" + bx.guid) || 0) % bz;
                Z._data(this, "lastToggle" + bx.guid, bC + 1);
                bB.preventDefault();
                return e[bC].apply(this, arguments) || false
            };
            bA.guid = by;
            while (bz < e.length) {
                e[bz++].guid = by
            }
            return this.click(bA)
        }
        , hover: function (bx, e) {
            return this.mouseenter(bx).mouseleave(e || bx)
        }

    });
    Z.each(("blur focus focusin focusout load resize scroll unload click dblclick mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave change select submit keydown keypress keyup error contextmenu").split(" "), function (e, bx) {
        Z.fn[bx] = function (by, bz) {
            if (bz == null) {
                bz = by;
                by = null
            }
            return arguments.length > 0 ? this.bind(bx, by, bz) : this.trigger(bx)
        };
        if (Z.attrFn) {
            Z.attrFn[bx] = true
        }
        if (aI.test(bx)) {
            Z.event.fixHooks[bx] = Z.event.keyHooks
        }
        if (aL.test(bx)) {
            Z.event.fixHooks[bx] = Z.event.mouseHooks
        }

    });
    /*
        * Sizzle CSS Selector Engine
        *  Copyright 2011, The Dojo Foundation
        *  Released under the MIT, BSD, and GPL Licenses.
        *  More information: http://sizzlejs.com/
        */
    (function () {
        var by = /((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^\[\]]*\]|['"][^'"]*['"]|[^\[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g, bD = "sizcache" + (Math.random() + "").replace(".", ""), bB = 0, bR = Object.prototype.toString, bH = false, bx = true, bL = /\\/g, bN = /\r\n/g, bM = /\W/;
        [0, 0].sort(function () {
            bx = false;
            return 0
        });
        var bP = function (b6, bT, b3, b5) {
            b3 = b3 || [];
            bT = bT || A;
            var bZ = bT;
            if (bT.nodeType !== 1 && bT.nodeType !== 9) {
                return []
            }
            if (!b6 || typeof b6 !== "string") {
                return b3
            }
            var bY, b7, e, bW, b4, bV, b1, bX, b2 = true, bU = bP.isXML(bT), b0 = [], b8 = b6;
            do {
                by.exec("");
                bY = by.exec(b8);
                if (bY) {
                    b8 = bY[3];
                    b0.push(bY[1]);
                    if (bY[2]) {
                        bW = bY[3];
                        break
                    }

                }

            }
            while (bY);
            if (b0.length > 1 && bJ.exec(b6)) {
                if (b0.length === 2 && bE.relative[b0[0]]) {
                    b7 = bK(b0[0] + b0[1], bT, b5)
                }
                else {
                    b7 = bE.relative[b0[0]] ? [bT] : bP(b0.shift(), bT);
                    while (b0.length) {
                        b6 = b0.shift();
                        if (bE.relative[b6]) {
                            b6 += b0.shift()
                        }
                        b7 = bK(b6, b7, b5)
                    }

                }

            }
            else {
                if (!b5 && b0.length > 1 && bT.nodeType === 9 && !bU && bE.match.ID.test(b0[0]) && !bE.match.ID.test(b0[b0.length - 1])) {
                    b4 = bP.find(b0.shift(), bT, bU);
                    bT = b4.expr ? bP.filter(b4.expr, b4.set)[0] : b4.set[0]
                }
                if (bT) {
                    b4 = b5 ? {
                        expr: b0.pop(), set: bI(b5)
                    }
                    : bP.find(b0.pop(), b0.length === 1 && (b0[0] === "~" || b0[0] === "+") && bT.parentNode ? bT.parentNode : bT, bU);
                    b7 = b4.expr ? bP.filter(b4.expr, b4.set) : b4.set;
                    if (b0.length > 0) {
                        e = bI(b7)
                    }
                    else {
                        b2 = false
                    }
                    while (b0.length) {
                        bV = b0.pop();
                        b1 = bV;
                        if (!bE.relative[bV]) {
                            bV = ""
                        }
                        else {
                            b1 = b0.pop()
                        }
                        if (b1 == null) {
                            b1 = bT
                        }
                        bE.relative[bV](e, b1, bU)
                    }

                }
                else {
                    e = b0 = []
                }

            }
            if (!e) {
                e = b7
            }
            if (!e) {
                bP.error(bV || b6)
            }
            if (bR.call(e) === "[object Array]") {
                if (!b2) {
                    b3.push.apply(b3, e)
                }
                else {
                    if (bT && bT.nodeType === 1) {
                        for (bX = 0;
                        e[bX] != null;
                        bX++) {
                            if (e[bX] && (e[bX] === true || e[bX].nodeType === 1 && bP.contains(bT, e[bX]))) {
                                b3.push(b7[bX])
                            }

                        }

                    }
                    else {
                        for (bX = 0;
                        e[bX] != null;
                        bX++) {
                            if (e[bX] && e[bX].nodeType === 1) {
                                b3.push(b7[bX])
                            }

                        }

                    }

                }

            }
            else {
                bI(e, b3)
            }
            if (bW) {
                bP(bW, bZ, b3, b5);
                bP.uniqueSort(b3)
            }
            return b3
        };
        bP.uniqueSort = function (bT) {
            if (bQ) {
                bH = bx;
                bT.sort(bQ);
                if (bH) {
                    for (var e = 1;
                    e < bT.length;
                    e++) {
                        if (bT[e] === bT[e - 1]) {
                            bT.splice(e--, 1)
                        }

                    }

                }

            }
            return bT
        };
        bP.matches = function (e, bT) {
            return bP(e, null, null, bT)
        };
        bP.matchesSelector = function (bT, e) {
            return bP(e, null, null, [bT]).length > 0
        };
        bP.find = function (bT, e, bV) {
            var bZ, bU, bX, bY, b0, bW;
            if (!bT) {
                return []
            }
            for (bU = 0, bX = bE.order.length;
            bU < bX;
            bU++) {
                b0 = bE.order[bU];
                if ((bY = bE.leftMatch[b0].exec(bT))) {
                    bW = bY[1];
                    bY.splice(1, 1);
                    if (bW.substr(bW.length - 1) !== "\\") {
                        bY[1] = (bY[1] || "").replace(bL, "");
                        bZ = bE.find[b0](bY, e, bV);
                        if (bZ != null) {
                            bT = bT.replace(bE.match[b0], "");
                            break
                        }

                    }

                }

            }
            if (!bZ) {
                bZ = typeof e.getElementsByTagName !== "undefined" ? e.getElementsByTagName("*") : []
            }
            return {
                set: bZ, expr: bT
            }

        };
        bP.filter = function (bU, b7, bY, b3) {
            var b2, e, b8, bW, b0, bV, b1, bX, b5, b4 = bU, b6 = [], bT = b7, bZ = b7 && b7[0] && bP.isXML(b7[0]);
            while (bU && b7.length) {
                for (b8 in bE.filter) {
                    if ((b2 = bE.leftMatch[b8].exec(bU)) != null && b2[2]) {
                        bV = bE.filter[b8];
                        b1 = b2[1];
                        e = false;
                        b2.splice(1, 1);
                        if (b1.substr(b1.length - 1) === "\\") {
                            continue
                        }
                        if (bT === b6) {
                            b6 = []
                        }
                        if (bE.preFilter[b8]) {
                            b2 = bE.preFilter[b8](b2, bT, bY, b6, b3, bZ);
                            if (!b2) {
                                e = bW = true
                            }
                            else {
                                if (b2 === true) {
                                    continue
                                }

                            }

                        }
                        if (b2) {
                            for (bX = 0;
                            (b0 = bT[bX]) != null;
                            bX++) {
                                if (b0) {
                                    bW = bV(b0, b2, bX, bT);
                                    b5 = b3 ^ bW;
                                    if (bY && bW != null) {
                                        if (b5) {
                                            e = true
                                        }
                                        else {
                                            bT[bX] = false
                                        }

                                    }
                                    else {
                                        if (b5) {
                                            b6.push(b0);
                                            e = true
                                        }

                                    }

                                }

                            }

                        }
                        if (bW !== bq) {
                            if (!bY) {
                                bT = b6
                            }
                            bU = bU.replace(bE.match[b8], "");
                            if (!e) {
                                return []
                            }
                            break
                        }

                    }

                }
                if (bU === b4) {
                    if (e == null) {
                        bP.error(bU)
                    }
                    else {
                        break
                    }

                }
                b4 = bU
            }
            return bT
        };
        bP.error = function (e) {
            throw "Syntax error, unrecognized expression: " + e
        };
        var bG = bP.getText = function (e) {
            var bT, bU, bV = e.nodeType, bW = "";
            if (bV) {
                if (bV === 1) {
                    if (typeof e.textContent === "string") {
                        return e.textContent
                    }
                    else {
                        if (typeof e.innerText === "string") {
                            return e.innerText.replace(bN, "")
                        }
                        else {
                            for (e = e.firstChild;
                            e;
                            e = e.nextSibling) {
                                bW += bG(e)
                            }

                        }

                    }

                }
                else {
                    if (bV === 3 || bV === 4) {
                        return e.nodeValue
                    }

                }

            }
            else {
                for (bT = 0;
                (bU = e[bT]) ;
                bT++) {
                    if (bU.nodeType !== 8) {
                        bW += bG(bU)
                    }

                }

            }
            return bW
        };
        var bE = bP.selectors = {
            order: ["ID", "NAME", "TAG"], match: {
                ID: /#((?:[\w\u00c0-\uFFFF\-]|\\.)+)/, CLASS: /\.((?:[\w\u00c0-\uFFFF\-]|\\.)+)/, NAME: /\[name=['"]*((?:[\w\u00c0-\uFFFF\-]|\\.)+)['"]*\]/, ATTR: /\[\s*((?:[\w\u00c0-\uFFFF\-]|\\.)+)\s*(?:(\S?=)\s*(?:(['"])(.*?)\3|(#?(?:[\w\u00c0-\uFFFF\-]|\\.)*)|)|)\s*\]/, TAG: /^((?:[\w\u00c0-\uFFFF\*\-]|\\.)+)/, CHILD: /:(only|nth|last|first)-child(?:\(\s*(even|odd|(?:[+\-]?\d+|(?:[+\-]?\d*)?n\s*(?:[+\-]\s*\d+)?))\s*\))?/, POS: /:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^\-]|$)/, PSEUDO: /:((?:[\w\u00c0-\uFFFF\-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?/
            }
            , leftMatch: {}, attrMap: {
                "class": "className", "for": "htmlFor"
            }
            , attrHandle: {
                href: function (e) {
                    return e.getAttribute("href")
                }
                , type: function (e) {
                    return e.getAttribute("type")
                }

            }
            , relative: {
                "+": function (e, bZ) {
                    var bV = typeof bZ === "string", bX = bV && !bM.test(bZ), bW = bV && !bX;
                    if (bX) {
                        bZ = bZ.toLowerCase()
                    }
                    for (var bU = 0, bY = e.length, bT;
                    bU < bY;
                    bU++) {
                        if ((bT = e[bU])) {
                            while ((bT = bT.previousSibling) && bT.nodeType !== 1) { } e[bU] = bW || bT && bT.nodeName.toLowerCase() === bZ ? bT || false : bT === bZ
                        }

                    }
                    if (bW) {
                        bP.filter(bZ, e, true)
                    }

                }
                , ">": function (e, bY) {
                    var bT, bV = typeof bY === "string", bU = 0, bW = e.length;
                    if (bV && !bM.test(bY)) {
                        bY = bY.toLowerCase();
                        for (;
                        bU < bW;
                        bU++) {
                            bT = e[bU];
                            if (bT) {
                                var bX = bT.parentNode;
                                e[bU] = bX.nodeName.toLowerCase() === bY ? bX : false
                            }

                        }

                    }
                    else {
                        for (;
                        bU < bW;
                        bU++) {
                            bT = e[bU];
                            if (bT) {
                                e[bU] = bV ? bT.parentNode : bT.parentNode === bY
                            }

                        }
                        if (bV) {
                            bP.filter(bY, e, true)
                        }

                    }

                }
                , "": function (bT, bX, bV) {
                    var bW, bU = bB++, e = bz;
                    if (typeof bX === "string" && !bM.test(bX)) {
                        bX = bX.toLowerCase();
                        bW = bX;
                        e = bA
                    }
                    e("parentNode", bX, bU, bT, bW, bV)
                }
                , "~": function (bT, bX, bV) {
                    var bW, bU = bB++, e = bz;
                    if (typeof bX === "string" && !bM.test(bX)) {
                        bX = bX.toLowerCase();
                        bW = bX;
                        e = bA
                    }
                    e("previousSibling", bX, bU, bT, bW, bV)
                }

            }
            , find: {
                ID: function (bV, e, bT) {
                    if (typeof e.getElementById !== "undefined" && !bT) {
                        var bU = e.getElementById(bV[1]);
                        return bU && bU.parentNode ? [bU] : []
                    }

                }
                , NAME: function (bV, e) {
                    if (typeof e.getElementsByName !== "undefined") {
                        var bX = [], bW = e.getElementsByName(bV[1]);
                        for (var bT = 0, bU = bW.length;
                        bT < bU;
                        bT++) {
                            if (bW[bT].getAttribute("name") === bV[1]) {
                                bX.push(bW[bT])
                            }

                        }
                        return bX.length === 0 ? null : bX
                    }

                }
                , TAG: function (bT, e) {
                    if (typeof e.getElementsByTagName !== "undefined") {
                        return e.getElementsByTagName(bT[1])
                    }

                }

            }
            , preFilter: {
                CLASS: function (bX, e, bV, bZ, bY, bW) {
                    bX = " " + bX[1].replace(bL, "") + " ";
                    if (bW) {
                        return bX
                    }
                    for (var bU = 0, bT;
                    (bT = e[bU]) != null;
                    bU++) {
                        if (bT) {
                            if (bY ^ (bT.className && (" " + bT.className + " ").replace(/[\t\n\r]/g, " ").indexOf(bX) >= 0)) {
                                if (!bV) {
                                    bZ.push(bT)
                                }

                            }
                            else {
                                if (bV) {
                                    e[bU] = false
                                }

                            }

                        }

                    }
                    return false
                }
                , ID: function (e) {
                    return e[1].replace(bL, "")
                }
                , TAG: function (bT, e) {
                    return bT[1].replace(bL, "").toLowerCase()
                }
                , CHILD: function (e) {
                    if (e[1] === "nth") {
                        if (!e[2]) {
                            bP.error(e[0])
                        }
                        e[2] = e[2].replace(/^\+|\s*/g, "");
                        var bT = /(-?)(\d*)(?:n([+\-]?\d*))?/.exec(e[2] === "even" && "2n" || e[2] === "odd" && "2n+1" || !/\D/.test(e[2]) && "0n+" + e[2] || e[2]);
                        e[2] = (bT[1] + (bT[2] || 1)) - 0;
                        e[3] = bT[3] - 0
                    }
                    else {
                        if (e[2]) {
                            bP.error(e[0])
                        }

                    }
                    e[0] = bB++;
                    return e
                }
                , ATTR: function (bV, e, bT, bY, bX, bU) {
                    var bW = bV[1] = bV[1].replace(bL, "");
                    if (!bU && bE.attrMap[bW]) {
                        bV[1] = bE.attrMap[bW]
                    }
                    bV[4] = (bV[4] || bV[5] || "").replace(bL, "");
                    if (bV[2] === "~=") {
                        bV[4] = " " + bV[4] + " "
                    }
                    return bV
                }
                , PSEUDO: function (bU, e, bT, bW, bV) {
                    if (bU[1] === "not") {
                        if ((by.exec(bU[3]) || "").length > 1 || /^\w/.test(bU[3])) {
                            bU[3] = bP(bU[3], null, null, e)
                        }
                        else {
                            var bX = bP.filter(bU[3], e, bT, true ^ bV);
                            if (!bT) {
                                bW.push.apply(bW, bX)
                            }
                            return false
                        }

                    }
                    else {
                        if (bE.match.POS.test(bU[0]) || bE.match.CHILD.test(bU[0])) {
                            return true
                        }

                    }
                    return bU
                }
                , POS: function (e) {
                    e.unshift(true);
                    return e
                }

            }
            , filters: {
                enabled: function (e) {
                    return e.disabled === false && e.type !== "hidden"
                }
                , disabled: function (e) {
                    return e.disabled === true
                }
                , checked: function (e) {
                    return e.checked === true
                }
                , selected: function (e) {
                    if (e.parentNode) {
                        e.parentNode.selectedIndex
                    }
                    return e.selected === true
                }
                , parent: function (e) {
                    return !!e.firstChild
                }
                , empty: function (e) {
                    return !e.firstChild
                }
                , has: function (e, bT, bU) {
                    return !!bP(bU[3], e).length
                }
                , header: function (e) {
                    return (/h\d/i).test(e.nodeName)
                }
                , text: function (bT) {
                    var e = bT.getAttribute("type"), bU = bT.type;
                    return bT.nodeName.toLowerCase() === "input" && "text" === bU && (e === bU || e === null)
                }
                , radio: function (e) {
                    return e.nodeName.toLowerCase() === "input" && "radio" === e.type
                }
                , checkbox: function (e) {
                    return e.nodeName.toLowerCase() === "input" && "checkbox" === e.type
                }
                , file: function (e) {
                    return e.nodeName.toLowerCase() === "input" && "file" === e.type
                }
                , password: function (e) {
                    return e.nodeName.toLowerCase() === "input" && "password" === e.type
                }
                , submit: function (e) {
                    var bT = e.nodeName.toLowerCase();
                    return (bT === "input" || bT === "button") && "submit" === e.type
                }
                , image: function (e) {
                    return e.nodeName.toLowerCase() === "input" && "image" === e.type
                }
                , reset: function (e) {
                    var bT = e.nodeName.toLowerCase();
                    return (bT === "input" || bT === "button") && "reset" === e.type
                }
                , button: function (e) {
                    var bT = e.nodeName.toLowerCase();
                    return bT === "input" && "button" === e.type || bT === "button"
                }
                , input: function (e) {
                    return (/input|select|textarea|button/i).test(e.nodeName)
                }
                , focus: function (e) {
                    return e === e.ownerDocument.activeElement
                }

            }
            , setFilters: {
                first: function (e, bT) {
                    return bT === 0
                }
                , last: function (bT, bU, bV, e) {
                    return bU === e.length - 1
                }
                , even: function (e, bT) {
                    return bT % 2 === 0
                }
                , odd: function (e, bT) {
                    return bT % 2 === 1
                }
                , lt: function (e, bT, bU) {
                    return bT < bU[3] - 0
                }
                , gt: function (e, bT, bU) {
                    return bT > bU[3] - 0
                }
                , nth: function (e, bT, bU) {
                    return bU[3] - 0 === bT
                }
                , eq: function (e, bT, bU) {
                    return bU[3] - 0 === bT
                }

            }
            , filter: {
                PSEUDO: function (bT, bY, bV, e) {
                    var bZ = bY[1], bU = bE.filters[bZ];
                    if (bU) {
                        return bU(bT, bV, bY, e)
                    }
                    else {
                        if (bZ === "contains") {
                            return (bT.textContent || bT.innerText || bG([bT]) || "").indexOf(bY[3]) >= 0
                        }
                        else {
                            if (bZ === "not") {
                                var b0 = bY[3];
                                for (var bW = 0, bX = b0.length;
                                bW < bX;
                                bW++) {
                                    if (b0[bW] === bT) {
                                        return false
                                    }

                                }
                                return true
                            }
                            else {
                                bP.error(bZ)
                            }

                        }

                    }

                }
                , CHILD: function (bW, bZ) {
                    var bX, bY, bV, b1, e, bT, bU, b2 = bZ[1], b0 = bW;
                    switch (b2) {
                        case "only": case "first": while ((b0 = b0.previousSibling)) {
                            if (b0.nodeType === 1) {
                                return false
                            }

                        }
                            if (b2 === "first") {
                                return true
                            }
                            b0 = bW;
                        case "last": while ((b0 = b0.nextSibling)) {
                            if (b0.nodeType === 1) {
                                return false
                            }

                        }
                            return true;
                        case "nth": bX = bZ[2];
                            bY = bZ[3];
                            if (bX === 1 && bY === 0) {
                                return true
                            }
                            bV = bZ[0];
                            b1 = bW.parentNode;
                            if (b1 && (b1[bD] !== bV || !bW.nodeIndex)) {
                                bT = 0;
                                for (b0 = b1.firstChild;
                                b0;
                                b0 = b0.nextSibling) {
                                    if (b0.nodeType === 1) {
                                        b0.nodeIndex = ++bT
                                    }

                                }
                                b1[bD] = bV
                            }
                            bU = bW.nodeIndex - bY;
                            if (bX === 0) {
                                return bU === 0
                            }
                            else {
                                return (bU % bX === 0 && bU / bX >= 0)
                            }

                    }

                }
                , ID: function (e, bT) {
                    return e.nodeType === 1 && e.getAttribute("id") === bT
                }
                , TAG: function (e, bT) {
                    return (bT === "*" && e.nodeType === 1) || !!e.nodeName && e.nodeName.toLowerCase() === bT
                }
                , CLASS: function (e, bT) {
                    return (" " + (e.className || e.getAttribute("class")) + " ").indexOf(bT) > -1
                }
                , ATTR: function (bT, bU) {
                    var bV = bU[1], bW = bP.attr ? bP.attr(bT, bV) : bE.attrHandle[bV] ? bE.attrHandle[bV](bT) : bT[bV] != null ? bT[bV] : bT.getAttribute(bV), bY = bW + "", bX = bU[2], e = bU[4];
                    return bW == null ? bX === "!=" : !bX && bP.attr ? bW != null : bX === "=" ? bY === e : bX === "*=" ? bY.indexOf(e) >= 0 : bX === "~=" ? (" " + bY + " ").indexOf(e) >= 0 : !e ? bY && bW !== false : bX === "!=" ? bY !== e : bX === "^=" ? bY.indexOf(e) === 0 : bX === "$=" ? bY.substr(bY.length - e.length) === e : bX === "|=" ? bY === e || bY.substr(0, e.length + 1) === e + "-" : false
                }
                , POS: function (bT, bW, bV, e) {
                    var bX = bW[2], bU = bE.setFilters[bX];
                    if (bU) {
                        return bU(bT, bV, bW, e)
                    }

                }

            }

        };
        var bJ = bE.match.POS, bF = function (e, bT) {
            return "\\" + (bT - 0 + 1)
        };
        for (var bS in bE.match) {
            bE.match[bS] = new RegExp(bE.match[bS].source + (/(?![^\[]*\])(?![^\(]*\))/.source));
            bE.leftMatch[bS] = new RegExp(/(^(?:.|\r|\n)*?)/.source + bE.match[bS].source.replace(/\\(\d+)/g, bF))
        }
        var bI = function (e, bT) {
            e = Array.prototype.slice.call(e, 0);
            if (bT) {
                bT.push.apply(bT, e);
                return bT
            }
            return e
        };
        try {
            Array.prototype.slice.call(A.documentElement.childNodes, 0)[0].nodeType
        }
        catch (bC) {
            bI = function (e, bV) {
                var bT = 0, bW = bV || [];
                if (bR.call(e) === "[object Array]") {
                    Array.prototype.push.apply(bW, e)
                }
                else {
                    if (typeof e.length === "number") {
                        for (var bU = e.length;
                        bT < bU;
                        bT++) {
                            bW.push(e[bT])
                        }

                    }
                    else {
                        for (;
                        e[bT];
                        bT++) {
                            bW.push(e[bT])
                        }

                    }

                }
                return bW
            }

        }
        var bQ, bO;
        if (A.documentElement.compareDocumentPosition) {
            bQ = function (e, bT) {
                if (e === bT) {
                    bH = true;
                    return 0
                }
                if (!e.compareDocumentPosition || !bT.compareDocumentPosition) {
                    return e.compareDocumentPosition ? -1 : 1
                }
                return e.compareDocumentPosition(bT) & 4 ? -1 : 1
            }

        }
        else {
            bQ = function (e, bW) {
                if (e === bW) {
                    bH = true;
                    return 0
                }
                else {
                    if (e.sourceIndex && bW.sourceIndex) {
                        return e.sourceIndex - bW.sourceIndex
                    }

                }
                var bT, bX, bU = [], bY = [], bV = e.parentNode, bZ = bW.parentNode, b0 = bV;
                if (bV === bZ) {
                    return bO(e, bW)
                }
                else {
                    if (!bV) {
                        return -1
                    }
                    else {
                        if (!bZ) {
                            return 1
                        }

                    }

                }
                while (b0) {
                    bU.unshift(b0);
                    b0 = b0.parentNode
                }
                b0 = bZ;
                while (b0) {
                    bY.unshift(b0);
                    b0 = b0.parentNode
                }
                bT = bU.length;
                bX = bY.length;
                for (var b1 = 0;
                b1 < bT && b1 < bX;
                b1++) {
                    if (bU[b1] !== bY[b1]) {
                        return bO(bU[b1], bY[b1])
                    }

                }
                return b1 === bT ? bO(e, bY[b1], -1) : bO(bU[b1], bW, 1)
            };
            bO = function (e, bT, bV) {
                if (e === bT) {
                    return bV
                }
                var bU = e.nextSibling;
                while (bU) {
                    if (bU === bT) {
                        return -1
                    }
                    bU = bU.nextSibling
                }
                return 1
            }

        }
        (function () {
            var e = A.createElement("div"), bT = "script" + (new Date()).getTime(), bU = A.documentElement;
            e.innerHTML = "<a name='" + bT + "'/>";
            bU.insertBefore(e, bU.firstChild);
            if (A.getElementById(bT)) {
                bE.find.ID = function (bY, bV, bW) {
                    if (typeof bV.getElementById !== "undefined" && !bW) {
                        var bX = bV.getElementById(bY[1]);
                        return bX ? bX.id === bY[1] || typeof bX.getAttributeNode !== "undefined" && bX.getAttributeNode("id").nodeValue === bY[1] ? [bX] : bq : []
                    }

                };
                bE.filter.ID = function (bV, bW) {
                    var bX = typeof bV.getAttributeNode !== "undefined" && bV.getAttributeNode("id");
                    return bV.nodeType === 1 && bX && bX.nodeValue === bW
                }

            }
            bU.removeChild(e);
            bU = e = null
        })();
        (function () {
            var e = A.createElement("div");
            e.appendChild(A.createComment(""));
            if (e.getElementsByTagName("*").length > 0) {
                bE.find.TAG = function (bV, bT) {
                    var bW = bT.getElementsByTagName(bV[1]);
                    if (bV[1] === "*") {
                        var bX = [];
                        for (var bU = 0;
                        bW[bU];
                        bU++) {
                            if (bW[bU].nodeType === 1) {
                                bX.push(bW[bU])
                            }

                        }
                        bW = bX
                    }
                    return bW
                }

            }
            e.innerHTML = "<a href='#'></a>";
            if (e.firstChild && typeof e.firstChild.getAttribute !== "undefined" && e.firstChild.getAttribute("href") !== "#") {
                bE.attrHandle.href = function (bT) {
                    return bT.getAttribute("href", 2)
                }

            }
            e = null
        })();
        if (A.querySelectorAll) {
            (function () {
                var bU = bP, e = A.createElement("div"), bT = "__sizzle__";
                e.innerHTML = "<p class='TEST'></p>";
                if (e.querySelectorAll && e.querySelectorAll(".TEST").length === 0) {
                    return
                }
                bP = function (b6, bW, bY, b8) {
                    bW = bW || A;
                    if (!b8 && !bP.isXML(bW)) {
                        var b0 = /^(\w+$)|^\.([\w\-]+$)|^#([\w\-]+$)/.exec(b6);
                        if (b0 && (bW.nodeType === 1 || bW.nodeType === 9)) {
                            if (b0[1]) {
                                return bI(bW.getElementsByTagName(b6), bY)
                            }
                            else {
                                if (b0[2] && bE.find.CLASS && bW.getElementsByClassName) {
                                    return bI(bW.getElementsByClassName(b0[2]), bY)
                                }

                            }

                        }
                        if (bW.nodeType === 9) {
                            if (b6 === "body" && bW.body) {
                                return bI([bW.body], bY)
                            }
                            else {
                                if (b0 && b0[3]) {
                                    var bX = bW.getElementById(b0[3]);
                                    if (bX && bX.parentNode) {
                                        if (bX.id === b0[3]) {
                                            return bI([bX], bY)
                                        }

                                    }
                                    else {
                                        return bI([], bY)
                                    }

                                }

                            }
                            try {
                                return bI(bW.querySelectorAll(b6), bY)
                            }
                            catch (b5) { }
                        }
                        else {
                            if (bW.nodeType === 1 && bW.nodeName.toLowerCase() !== "object") {
                                var b3 = bW, b2 = bW.getAttribute("id"), b1 = b2 || bT, bZ = bW.parentNode, b7 = /^\s*[+~]/.test(b6);
                                if (!b2) {
                                    bW.setAttribute("id", b1)
                                }
                                else {
                                    b1 = b1.replace(/'/g, "\\$&")
                                }
                                if (b7 && bZ) {
                                    bW = bW.parentNode
                                }
                                try {
                                    if (!b7 || bZ) {
                                        return bI(bW.querySelectorAll("[id='" + b1 + "'] " + b6), bY)
                                    }

                                }
                                catch (b4) { } finally {
                                    if (!b2) {
                                        b3.removeAttribute("id")
                                    }

                                }

                            }

                        }

                    }
                    return bU(b6, bW, bY, b8)
                };
                for (var bV in bU) {
                    bP[bV] = bU[bV]
                }
                e = null
            })()
        }
        (function () {
            var bT = A.documentElement, bU = bT.matchesSelector || bT.mozMatchesSelector || bT.webkitMatchesSelector || bT.msMatchesSelector;
            if (bU) {
                var e = !bU.call(A.createElement("div"), "div"), bW = false;
                try {
                    bU.call(A.documentElement, "[test!='']:sizzle")
                }
                catch (bV) {
                    bW = true
                }
                bP.matchesSelector = function (bZ, bY) {
                    bY = bY.replace(/\=\s*([^'"\]]*)\s*\]/g, "='$1']");
                    if (!bP.isXML(bZ)) {
                        try {
                            if (bW || !bE.match.PSEUDO.test(bY) && !/!=/.test(bY)) {
                                var b0 = bU.call(bZ, bY);
                                if (b0 || !e || bZ.document && bZ.document.nodeType !== 11) {
                                    return b0
                                }

                            }

                        }
                        catch (bX) { }
                    }
                    return bP(bY, null, null, [bZ]).length > 0
                }

            }

        })();
        (function () {
            var e = A.createElement("div");
            e.innerHTML = "<div class='test e'></div><div class='test'></div>";
            if (!e.getElementsByClassName || e.getElementsByClassName("e").length === 0) {
                return
            }
            e.lastChild.className = "e";
            if (e.getElementsByClassName("e").length === 1) {
                return
            }
            bE.order.splice(1, 0, "CLASS");
            bE.find.CLASS = function (bV, bT, bU) {
                if (typeof bT.getElementsByClassName !== "undefined" && !bU) {
                    return bT.getElementsByClassName(bV[1])
                }

            };
            e = null
        })();
        function bA(bU, bT, bV, e, b1, bY) {
            for (var bX = 0, bZ = e.length;
            bX < bZ;
            bX++) {
                var bW = e[bX];
                if (bW) {
                    var b0 = false;
                    bW = bW[bU];
                    while (bW) {
                        if (bW[bD] === bV) {
                            b0 = e[bW.sizset];
                            break
                        }
                        if (bW.nodeType === 1 && !bY) {
                            bW[bD] = bV;
                            bW.sizset = bX
                        }
                        if (bW.nodeName.toLowerCase() === bT) {
                            b0 = bW;
                            break
                        }
                        bW = bW[bU]
                    }
                    e[bX] = b0
                }

            }

        }
        function bz(bU, bT, bV, e, b1, bY) {
            for (var bX = 0, bZ = e.length;
            bX < bZ;
            bX++) {
                var bW = e[bX];
                if (bW) {
                    var b0 = false;
                    bW = bW[bU];
                    while (bW) {
                        if (bW[bD] === bV) {
                            b0 = e[bW.sizset];
                            break
                        }
                        if (bW.nodeType === 1) {
                            if (!bY) {
                                bW[bD] = bV;
                                bW.sizset = bX
                            }
                            if (typeof bT !== "string") {
                                if (bW === bT) {
                                    b0 = true;
                                    break
                                }

                            }
                            else {
                                if (bP.filter(bT, [bW]).length > 0) {
                                    b0 = bW;
                                    break
                                }

                            }

                        }
                        bW = bW[bU]
                    }
                    e[bX] = b0
                }

            }

        }
        if (A.documentElement.contains) {
            bP.contains = function (e, bT) {
                return e !== bT && (e.contains ? e.contains(bT) : true)
            }

        }
        else {
            if (A.documentElement.compareDocumentPosition) {
                bP.contains = function (e, bT) {
                    return !!(e.compareDocumentPosition(bT) & 16)
                }

            }
            else {
                bP.contains = function () {
                    return false
                }

            }

        }
        bP.isXML = function (bT) {
            var e = (bT ? bT.ownerDocument || bT : 0).documentElement;
            return e ? e.nodeName !== "HTML" : false
        };
        var bK = function (bZ, e, bY) {
            var bW, b0 = [], bV = "", bX = e.nodeType ? [e] : e;
            while ((bW = bE.match.PSEUDO.exec(bZ))) {
                bV += bW[0];
                bZ = bZ.replace(bE.match.PSEUDO, "")
            }
            bZ = bE.relative[bZ] ? bZ + "*" : bZ;
            for (var bT = 0, bU = bX.length;
            bT < bU;
            bT++) {
                bP(bZ, bX[bT], b0, bY)
            }
            return bP.filter(bV, b0)
        };
        bP.attr = Z.attr;
        bP.selectors.attrMap = {};
        Z.find = bP;
        Z.expr = bP.selectors;
        Z.expr[":"] = Z.expr.filters;
        Z.unique = bP.uniqueSort;
        Z.text = bP.getText;
        Z.isXMLDoc = bP.isXML;
        Z.contains = bP.contains
    })();
    var bh = /Until$/, aX = /^(?:parents|prevUntil|prevAll)/, aN = /,/, Y = /^.[^:#\[\.,]*$/, bm = Array.prototype.slice, ag = Z.expr.match.POS, Q = {
        children: true, contents: true, next: true, prev: true
    };
    Z.fn.extend({
        find: function (bC) {
            var bD = this, e, bx;
            if (typeof bC !== "string") {
                return Z(bC).filter(function () {
                    for (e = 0, bx = bD.length;
                    e < bx;
                    e++) {
                        if (Z.contains(bD[e], this)) {
                            return true
                        }

                    }

                })
            }
            var bB = this.pushStack("", "find", bC), by, bz, bA;
            for (e = 0, bx = this.length;
            e < bx;
            e++) {
                by = bB.length;
                Z.find(bC, this[e], bB);
                if (e > 0) {
                    for (bz = by;
                    bz < bB.length;
                    bz++) {
                        for (bA = 0;
                        bA < by;
                        bA++) {
                            if (bB[bA] === bB[bz]) {
                                bB.splice(bz--, 1);
                                break
                            }

                        }

                    }

                }

            }
            return bB
        }
        , has: function (e) {
            var bx = Z(e);
            return this.filter(function () {
                for (var by = 0, bz = bx.length;
                by < bz;
                by++) {
                    if (Z.contains(this, bx[by])) {
                        return true
                    }

                }

            })
        }
        , not: function (e) {
            return this.pushStack(bs(this, e, false), "not", e)
        }
        , filter: function (e) {
            return this.pushStack(bs(this, e, true), "filter", e)
        }
        , is: function (e) {
            return !!e && (typeof e === "string" ? ag.test(e) ? Z(e, this.context).index(this[0]) >= 0 : Z.filter(e, this).length > 0 : this.filter(e).length > 0)
        }
        , closest: function (bD, e) {
            var bC = [], by, bz, bx = this[0];
            if (Z.isArray(bD)) {
                var bA = 1;
                while (bx && bx.ownerDocument && bx !== e) {
                    for (by = 0;
                    by < bD.length;
                    by++) {
                        if (Z(bx).is(bD[by])) {
                            bC.push({
                                selector: bD[by], elem: bx, level: bA
                            })
                        }

                    }
                    bx = bx.parentNode;
                    bA++
                }
                return bC
            }
            var bB = ag.test(bD) || typeof bD !== "string" ? Z(bD, e || this.context) : 0;
            for (by = 0, bz = this.length;
            by < bz;
            by++) {
                bx = this[by];
                while (bx) {
                    if (bB ? bB.index(bx) > -1 : Z.find.matchesSelector(bx, bD)) {
                        bC.push(bx);
                        break
                    }
                    else {
                        bx = bx.parentNode;
                        if (!bx || !bx.ownerDocument || bx === e || bx.nodeType === 11) {
                            break
                        }

                    }

                }

            }
            bC = bC.length > 1 ? Z.unique(bC) : bC;
            return this.pushStack(bC, "closest", bD)
        }
        , index: function (e) {
            if (!e) {
                return (this[0] && this[0].parentNode) ? this.prevAll().length : -1
            }
            if (typeof e === "string") {
                return Z.inArray(this[0], Z(e))
            }
            return Z.inArray(e.jquery ? e[0] : e, this)
        }
        , add: function (by, bx) {
            var bz = typeof by === "string" ? Z(by, bx) : Z.makeArray(by && by.nodeType ? [by] : by), e = Z.merge(this.get(), bz);
            return this.pushStack(W(bz[0]) || W(e[0]) ? e : Z.unique(e))
        }
        , andSelf: function () {
            return this.add(this.prevObject)
        }

    });
    function W(e) {
        return !e || !e.parentNode || e.parentNode.nodeType === 11
    }
    Z.each({
        parent: function (e) {
            var bx = e.parentNode;
            return bx && bx.nodeType !== 11 ? bx : null
        }
        , parents: function (e) {
            return Z.dir(e, "parentNode")
        }
        , parentsUntil: function (e, bx, by) {
            return Z.dir(e, "parentNode", by)
        }
        , next: function (e) {
            return Z.nth(e, 2, "nextSibling")
        }
        , prev: function (e) {
            return Z.nth(e, 2, "previousSibling")
        }
        , nextAll: function (e) {
            return Z.dir(e, "nextSibling")
        }
        , prevAll: function (e) {
            return Z.dir(e, "previousSibling")
        }
        , nextUntil: function (e, bx, by) {
            return Z.dir(e, "nextSibling", by)
        }
        , prevUntil: function (e, bx, by) {
            return Z.dir(e, "previousSibling", by)
        }
        , siblings: function (e) {
            return Z.sibling(e.parentNode.firstChild, e)
        }
        , children: function (e) {
            return Z.sibling(e.firstChild)
        }
        , contents: function (e) {
            return Z.nodeName(e, "iframe") ? e.contentDocument || e.contentWindow.document : Z.makeArray(e.childNodes)
        }

    }
    , function (bx, e) {
        Z.fn[bx] = function (bB, bA) {
            var bz = Z.map(this, e, bB), by = bm.call(arguments);
            if (!bh.test(bx)) {
                bA = bB
            }
            if (bA && typeof bA === "string") {
                bz = Z.filter(bA, bz)
            }
            bz = this.length > 1 && !Q[bx] ? Z.unique(bz) : bz;
            if ((this.length > 1 || aN.test(bA)) && aX.test(bx)) {
                bz = bz.reverse()
            }
            return this.pushStack(bz, bx, by.join(","))
        }

    });
    Z.extend({
        filter: function (bx, e, by) {
            if (by) {
                bx = ":not(" + bx + ")"
            }
            return e.length === 1 ? Z.find.matchesSelector(e[0], bx) ? [e[0]] : [] : Z.find.matches(bx, e)
        }
        , dir: function (by, bx, bA) {
            var bz = [], e = by[bx];
            while (e && e.nodeType !== 9 && (bA === bq || e.nodeType !== 1 || !Z(e).is(bA))) {
                if (e.nodeType === 1) {
                    bz.push(e)
                }
                e = e[bx]
            }
            return bz
        }
        , nth: function (e, bA, bx, by) {
            bA = bA || 1;
            var bz = 0;
            for (;
            e;
            e = e[bx]) {
                if (e.nodeType === 1 && ++bz === bA) {
                    break
                }

            }
            return e
        }
        , sibling: function (bx, e) {
            var by = [];
            for (;
            bx;
            bx = bx.nextSibling) {
                if (bx.nodeType === 1 && bx !== e) {
                    by.push(bx)
                }

            }
            return by
        }

    });
    function bs(e, bz, by) {
        bz = bz || 0;
        if (Z.isFunction(bz)) {
            return Z.grep(e, function (bA, bB) {
                var bC = !!bz.call(bA, bB, bA);
                return bC === by
            })
        }
        else {
            if (bz.nodeType) {
                return Z.grep(e, function (bA, bB) {
                    return (bA === bz) === by
                })
            }
            else {
                if (typeof bz === "string") {
                    var bx = Z.grep(e, function (bA) {
                        return bA.nodeType === 1
                    });
                    if (Y.test(bz)) {
                        return Z.filter(bz, bx, !by)
                    }
                    else {
                        bz = Z.filter(bz, bx)
                    }

                }

            }

        }
        return Z.grep(e, function (bA, bB) {
            return (Z.inArray(bA, bz) >= 0) === by
        })
    }
    function r(e) {
        var bx = af.split(" "), by = e.createDocumentFragment();
        if (by.createElement) {
            while (bx.length) {
                by.createElement(bx.pop())
            }

        }
        return by
    }
    var af = "abbr article aside audio canvas datalist details figcaption figure footer header hgroup mark meter nav output progress section summary time video", aG = / jQuery\d+="(?:\d+|null)"/g, aJ = /^\s+/, bk = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/ig, bc = /<([\w:]+)/, bd = /<tbody/i, aF = /<|&#?\w+;/, aR = /<(?:script|style)/i, aP = /<(?:script|object|embed|option|style)/i, aS = new RegExp("<(?:" + af.replace(" ", "|") + ")", "i"), ap = /checked\s*(?:[^=]|=\s*.checked.)/i, a6 = /\/(java|ecma)script/i, ar = /^\s*<!(?:\[CDATA\[|\-\-)/, bt = {
        option: [1, "<select multiple='multiple'>", "</select>"], legend: [1, "<fieldset>", "</fieldset>"], thead: [1, "<table>", "</table>"], tr: [2, "<table><tbody>", "</tbody></table>"], td: [3, "<table><tbody><tr>", "</tr></tbody></table>"], col: [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"], area: [1, "<map>", "</map>"], _default: [0, "", ""]
    }
    , bl = r(A);
    bt.optgroup = bt.option;
    bt.tbody = bt.tfoot = bt.colgroup = bt.caption = bt.thead;
    bt.th = bt.td;
    if (!Z.support.htmlSerialize) {
        bt._default = [1, "div<div>", "</div>"]
    }
    Z.fn.extend({
        text: function (e) {
            if (Z.isFunction(e)) {
                return this.each(function (bx) {
                    var by = Z(this);
                    by.text(e.call(this, bx, by.text()))
                })
            }
            if (typeof e !== "object" && e !== bq) {
                return this.empty().append((this[0] && this[0].ownerDocument || A).createTextNode(e))
            }
            return Z.text(this)
        }
        , wrapAll: function (e) {
            if (Z.isFunction(e)) {
                return this.each(function (by) {
                    Z(this).wrapAll(e.call(this, by))
                })
            }
            if (this[0]) {
                var bx = Z(e, this[0].ownerDocument).eq(0).clone(true);
                if (this[0].parentNode) {
                    bx.insertBefore(this[0])
                }
                bx.map(function () {
                    var by = this;
                    while (by.firstChild && by.firstChild.nodeType === 1) {
                        by = by.firstChild
                    }
                    return by
                }).append(this)
            }
            return this
        }
        , wrapInner: function (e) {
            if (Z.isFunction(e)) {
                return this.each(function (bx) {
                    Z(this).wrapInner(e.call(this, bx))
                })
            }
            return this.each(function () {
                var by = Z(this), bx = by.contents();
                if (bx.length) {
                    bx.wrapAll(e)
                }
                else {
                    by.append(e)
                }

            })
        }
        , wrap: function (e) {
            return this.each(function () {
                Z(this).wrapAll(e)
            })
        }
        , unwrap: function () {
            return this.parent().each(function () {
                if (!Z.nodeName(this, "body")) {
                    Z(this).replaceWith(this.childNodes)
                }

            }).end()
        }
        , append: function () {
            return this.domManip(arguments, true, function (e) {
                if (this.nodeType === 1) {
                    this.appendChild(e)
                }

            })
        }
        , prepend: function () {
            return this.domManip(arguments, true, function (e) {
                if (this.nodeType === 1) {
                    this.insertBefore(e, this.firstChild)
                }

            })
        }
        , before: function () {
            if (this[0] && this[0].parentNode) {
                return this.domManip(arguments, false, function (bx) {
                    this.parentNode.insertBefore(bx, this)
                })
            }
            else {
                if (arguments.length) {
                    var e = Z(arguments[0]);
                    e.push.apply(e, this.toArray());
                    return this.pushStack(e, "before", arguments)
                }

            }

        }
        , after: function () {
            if (this[0] && this[0].parentNode) {
                return this.domManip(arguments, false, function (bx) {
                    this.parentNode.insertBefore(bx, this.nextSibling)
                })
            }
            else {
                if (arguments.length) {
                    var e = this.pushStack(this, "after", arguments);
                    e.push.apply(e, Z(arguments[0]).toArray());
                    return e
                }

            }

        }
        , remove: function (bz, by) {
            for (var bx = 0, e;
            (e = this[bx]) != null;
            bx++) {
                if (!bz || Z.filter(bz, [e]).length) {
                    if (!by && e.nodeType === 1) {
                        Z.cleanData(e.getElementsByTagName("*"));
                        Z.cleanData([e])
                    }
                    if (e.parentNode) {
                        e.parentNode.removeChild(e)
                    }

                }

            }
            return this
        }
        , empty: function () {
            for (var bx = 0, e;
            (e = this[bx]) != null;
            bx++) {
                if (e.nodeType === 1) {
                    Z.cleanData(e.getElementsByTagName("*"))
                }
                while (e.firstChild) {
                    e.removeChild(e.firstChild)
                }

            }
            return this
        }
        , clone: function (e, bx) {
            e = e == null ? false : e;
            bx = bx == null ? e : bx;
            return this.map(function () {
                return Z.clone(this, e, bx)
            })
        }
        , html: function (bA) {
            if (bA === bq) {
                return this[0] && this[0].nodeType === 1 ? this[0].innerHTML.replace(aG, "") : null
            }
            else {
                if (typeof bA === "string" && !aR.test(bA) && (Z.support.leadingWhitespace || !aJ.test(bA)) && !bt[(bc.exec(bA) || ["", ""])[1].toLowerCase()]) {
                    bA = bA.replace(bk, "<$1></$2>");
                    try {
                        for (var by = 0, bz = this.length;
                        by < bz;
                        by++) {
                            if (this[by].nodeType === 1) {
                                Z.cleanData(this[by].getElementsByTagName("*"));
                                this[by].innerHTML = bA
                            }

                        }

                    }
                    catch (bx) {
                        this.empty().append(bA)
                    }

                }
                else {
                    if (Z.isFunction(bA)) {
                        this.each(function (e) {
                            var bB = Z(this);
                            bB.html(bA.call(this, e, bB.html()))
                        })
                    }
                    else {
                        this.empty().append(bA)
                    }

                }

            }
            return this
        }
        , replaceWith: function (e) {
            if (this[0] && this[0].parentNode) {
                if (Z.isFunction(e)) {
                    return this.each(function (bx) {
                        var bz = Z(this), by = bz.html();
                        bz.replaceWith(e.call(this, bx, by))
                    })
                }
                if (typeof e !== "string") {
                    e = Z(e).detach()
                }
                return this.each(function () {
                    var bx = this.nextSibling, by = this.parentNode;
                    Z(this).remove();
                    if (bx) {
                        Z(bx).before(e)
                    }
                    else {
                        Z(by).append(e)
                    }

                })
            }
            else {
                return this.length ? this.pushStack(Z(Z.isFunction(e) ? e() : e), "replaceWith", e) : this
            }

        }
        , detach: function (e) {
            return this.remove(e, true)
        }
        , domManip: function (e, bG, bx) {
            var bE, by, bz, bD, bH = e[0], bF = [];
            if (!Z.support.checkClone && arguments.length === 3 && typeof bH === "string" && ap.test(bH)) {
                return this.each(function () {
                    Z(this).domManip(e, bG, bx, true)
                })
            }
            if (Z.isFunction(bH)) {
                return this.each(function (bI) {
                    var bJ = Z(this);
                    e[0] = bH.call(this, bI, bG ? bJ.html() : bq);
                    bJ.domManip(e, bG, bx)
                })
            }
            if (this[0]) {
                bD = bH && bH.parentNode;
                if (Z.support.parentNode && bD && bD.nodeType === 11 && bD.childNodes.length === this.length) {
                    bE = {
                        fragment: bD
                    }

                }
                else {
                    bE = Z.buildFragment(e, this, bF)
                }
                bz = bE.fragment;
                if (bz.childNodes.length === 1) {
                    by = bz = bz.firstChild
                }
                else {
                    by = bz.firstChild
                }
                if (by) {
                    bG = bG && Z.nodeName(by, "tr");
                    for (var bA = 0, bB = this.length, bC = bB - 1;
                    bA < bB;
                    bA++) {
                        bx.call(bG ? aV(this[bA], by) : this[bA], bE.cacheable || (bB > 1 && bA < bC) ? Z.clone(bz, true, true) : bz)
                    }

                }
                if (bF.length) {
                    Z.each(bF, D)
                }

            }
            return this
        }

    });
    function aV(bx, e) {
        return Z.nodeName(bx, "table") ? (bx.getElementsByTagName("tbody")[0] || bx.appendChild(bx.ownerDocument.createElement("tbody"))) : bx
    }
    function m(bC, bx) {
        if (bx.nodeType !== 1 || !Z.hasData(bC)) {
            return
        }
        var bD, bz, bA, bB = Z._data(bC), e = Z._data(bx, bB), by = bB.events;
        if (by) {
            delete e.handle;
            e.events = {};
            for (bD in by) {
                for (bz = 0, bA = by[bD].length;
                bz < bA;
                bz++) {
                    Z.event.add(bx, bD + (by[bD][bz].namespace ? "." : "") + by[bD][bz].namespace, by[bD][bz], by[bD][bz].data)
                }

            }

        }
        if (e.data) {
            e.data = Z.extend({}, e.data)
        }

    }
    function n(by, e) {
        var bx;
        if (e.nodeType !== 1) {
            return
        }
        if (e.clearAttributes) {
            e.clearAttributes()
        }
        if (e.mergeAttributes) {
            e.mergeAttributes(by)
        }
        bx = e.nodeName.toLowerCase();
        if (bx === "object") {
            e.outerHTML = by.outerHTML
        }
        else {
            if (bx === "input" && (by.type === "checkbox" || by.type === "radio")) {
                if (by.checked) {
                    e.defaultChecked = e.checked = by.checked
                }
                if (e.value !== by.value) {
                    e.value = by.value
                }

            }
            else {
                if (bx === "option") {
                    e.selected = by.defaultSelected
                }
                else {
                    if (bx === "input" || bx === "textarea") {
                        e.defaultValue = by.defaultValue
                    }

                }

            }

        }
        e.removeAttribute(Z.expando)
    }
    Z.buildFragment = function (e, bC, bD) {
        var bB, bx, by, bz, bA = e[0];
        if (bC && bC[0]) {
            bz = bC[0].ownerDocument || bC[0]
        }
        if (!bz.createDocumentFragment) {
            bz = A
        }
        if (e.length === 1 && typeof bA === "string" && bA.length < 512 && bz === A && bA.charAt(0) === "<" && !aP.test(bA) && (Z.support.checkClone || !ap.test(bA)) && (!Z.support.unknownElems && aS.test(bA))) {
            bx = true;
            by = Z.fragments[bA];
            if (by && by !== 1) {
                bB = by
            }

        }
        if (!bB) {
            bB = bz.createDocumentFragment();
            Z.clean(e, bz, bB, bD)
        }
        if (bx) {
            Z.fragments[bA] = by ? bB : 1
        }
        return {
            fragment: bB, cacheable: bx
        }

    };
    Z.fragments = {};
    Z.each({
        appendTo: "append", prependTo: "prepend", insertBefore: "before", insertAfter: "after", replaceAll: "replaceWith"
    }
    , function (e, bx) {
        Z.fn[e] = function (bE) {
            var bD = [], bA = Z(bE), bC = this.length === 1 && this[0].parentNode;
            if (bC && bC.nodeType === 11 && bC.childNodes.length === 1 && bA.length === 1) {
                bA[bx](this[0]);
                return this
            }
            else {
                for (var bz = 0, bB = bA.length;
                bz < bB;
                bz++) {
                    var by = (bz > 0 ? this.clone(true) : this).get();
                    Z(bA[bz])[bx](by);
                    bD = bD.concat(by)
                }
                return this.pushStack(bD, e, bA.selector)
            }

        }

    });
    function L(e) {
        if (typeof e.getElementsByTagName !== "undefined") {
            return e.getElementsByTagName("*")
        }
        else {
            if (typeof e.querySelectorAll !== "undefined") {
                return e.querySelectorAll("*")
            }
            else {
                return []
            }

        }

    }
    function F(e) {
        if (e.type === "checkbox" || e.type === "radio") {
            e.defaultChecked = e.checked
        }

    }
    function E(e) {
        var bx = (e.nodeName || "").toLowerCase();
        if (bx === "input") {
            F(e)
        }
        else {
            if (bx !== "script" && typeof e.getElementsByTagName !== "undefined") {
                Z.grep(e.getElementsByTagName("input"), F)
            }

        }

    }
    Z.extend({
        clone: function (bA, bx, by) {
            var e = bA.cloneNode(true), bC, bz, bB;
            if ((!Z.support.noCloneEvent || !Z.support.noCloneChecked) && (bA.nodeType === 1 || bA.nodeType === 11) && !Z.isXMLDoc(bA)) {
                n(bA, e);
                bC = L(bA);
                bz = L(e);
                for (bB = 0;
                bC[bB];
                ++bB) {
                    if (bz[bB]) {
                        n(bC[bB], bz[bB])
                    }

                }

            }
            if (bx) {
                m(bA, e);
                if (by) {
                    bC = L(bA);
                    bz = L(e);
                    for (bB = 0;
                    bC[bB];
                    ++bB) {
                        m(bC[bB], bz[bB])
                    }

                }

            }
            bC = bz = null;
            return e
        }
        , clean: function (bB, bx, bC, bJ) {
            var e;
            bx = bx || A;
            if (typeof bx.createElement === "undefined") {
                bx = bx.ownerDocument || bx[0] && bx[0].ownerDocument || A
            }
            var bI = [], bF;
            for (var bE = 0, bA;
            (bA = bB[bE]) != null;
            bE++) {
                if (typeof bA === "number") {
                    bA += ""
                }
                if (!bA) {
                    continue
                }
                if (typeof bA === "string") {
                    if (!aF.test(bA)) {
                        bA = bx.createTextNode(bA)
                    }
                    else {
                        bA = bA.replace(bk, "<$1></$2>");
                        var bK = (bc.exec(bA) || ["", ""])[1].toLowerCase(), bM = bt[bK] || bt._default, by = bM[0], bz = bx.createElement("div");
                        if (bx === A) {
                            bl.appendChild(bz)
                        }
                        else {
                            r(bx).appendChild(bz)
                        }
                        bz.innerHTML = bM[1] + bA + bM[2];
                        while (by--) {
                            bz = bz.lastChild
                        }
                        if (!Z.support.tbody) {
                            var bD = bd.test(bA), bL = bK === "table" && !bD ? bz.firstChild && bz.firstChild.childNodes : bM[1] === "<table>" && !bD ? bz.childNodes : [];
                            for (bF = bL.length - 1;
                            bF >= 0;
                            --bF) {
                                if (Z.nodeName(bL[bF], "tbody") && !bL[bF].childNodes.length) {
                                    bL[bF].parentNode.removeChild(bL[bF])
                                }

                            }

                        }
                        if (!Z.support.leadingWhitespace && aJ.test(bA)) {
                            bz.insertBefore(bx.createTextNode(aJ.exec(bA)[0]), bz.firstChild)
                        }
                        bA = bz.childNodes
                    }

                }
                var bH;
                if (!Z.support.appendChecked) {
                    if (bA[0] && typeof (bH = bA.length) === "number") {
                        for (bF = 0;
                        bF < bH;
                        bF++) {
                            E(bA[bF])
                        }

                    }
                    else {
                        E(bA)
                    }

                }
                if (bA.nodeType) {
                    bI.push(bA)
                }
                else {
                    bI = Z.merge(bI, bA)
                }

            }
            if (bC) {
                e = function (bN) {
                    return !bN.type || a6.test(bN.type)
                };
                for (bE = 0;
                bI[bE];
                bE++) {
                    if (bJ && Z.nodeName(bI[bE], "script") && (!bI[bE].type || bI[bE].type.toLowerCase() === "text/javascript")) {
                        bJ.push(bI[bE].parentNode ? bI[bE].parentNode.removeChild(bI[bE]) : bI[bE])
                    }
                    else {
                        if (bI[bE].nodeType === 1) {
                            var bG = Z.grep(bI[bE].getElementsByTagName("script"), e);
                            bI.splice.apply(bI, [bE + 1, 0].concat(bG))
                        }
                        bC.appendChild(bI[bE])
                    }

                }

            }
            return bI
        }
        , cleanData: function (bA) {
            var bx, bC, e = Z.cache, bD = Z.event.special, by = Z.support.deleteExpando;
            for (var bB = 0, bz;
            (bz = bA[bB]) != null;
            bB++) {
                if (bz.nodeName && Z.noData[bz.nodeName.toLowerCase()]) {
                    continue
                }
                bC = bz[Z.expando];
                if (bC) {
                    bx = e[bC];
                    if (bx && bx.events) {
                        for (var bE in bx.events) {
                            if (bD[bE]) {
                                Z.event.remove(bz, bE)
                            }
                            else {
                                Z.removeEvent(bz, bE, bx.handle)
                            }

                        }
                        if (bx.handle) {
                            bx.handle.elem = null
                        }

                    }
                    if (by) {
                        delete bz[Z.expando]
                    }
                    else {
                        if (bz.removeAttribute) {
                            bz.removeAttribute(Z.expando)
                        }

                    }
                    delete e[bC]
                }

            }

        }

    });
    function D(bx, e) {
        if (e.src) {
            Z.ajax({
                url: e.src, async: false, dataType: "script"
            })
        }
        else {
            Z.globalEval((e.text || e.textContent || e.innerHTML || "").replace(ar, "/*$0*/"))
        }
        if (e.parentNode) {
            e.parentNode.removeChild(e)
        }

    }
    var al = /alpha\([^)]*\)/i, aW = /opacity=([^)]*)/, bi = /([A-Z]|^ms)/g, aU = /^-?\d+(?:px)?$/i, aT = /^-?\d/, a2 = /^([\-+])=([\-+.\de]+)/, u = {
        position: "absolute", visibility: "hidden", display: "block"
    }
    , v = ["Left", "Right"], t = ["Top", "Bottom"], w, M, x;
    Z.fn.css = function (e, bx) {
        if (arguments.length === 2 && bx === bq) {
            return this
        }
        return Z.access(this, e, bx, true, function (by, bz, bA) {
            return bA !== bq ? Z.style(by, bz, bA) : Z.css(by, bz)
        })
    };
    Z.extend({
        cssHooks: {
            opacity: {
                get: function (bx, e) {
                    if (e) {
                        var by = w(bx, "opacity", "opacity");
                        return by === "" ? "1" : by
                    }
                    else {
                        return bx.style.opacity
                    }

                }

            }

        }
        , cssNumber: {
            fillOpacity: true, fontWeight: true, lineHeight: true, opacity: true, orphans: true, widows: true, zIndex: true, zoom: true
        }
        , cssProps: {
            "float": Z.support.cssFloat ? "cssFloat" : "styleFloat"
        }
        , style: function (by, bB, bG, bz) {
            if (!by || by.nodeType === 3 || by.nodeType === 8 || !by.style) {
                return
            }
            var bD, bF, bC = Z.camelCase(bB), bE = by.style, bA = Z.cssHooks[bC];
            bB = Z.cssProps[bC] || bC;
            if (bG !== bq) {
                bF = typeof bG;
                if (bF === "string" && (bD = a2.exec(bG))) {
                    bG = (+(bD[1] + 1) * +bD[2]) + parseFloat(Z.css(by, bB));
                    bF = "number"
                }
                if (bG == null || bF === "number" && isNaN(bG)) {
                    return
                }
                if (bF === "number" && !Z.cssNumber[bC]) {
                    bG += "px"
                }
                if (!bA || !("set" in bA) || (bG = bA.set(by, bG)) !== bq) {
                    try {
                        bE[bB] = bG
                    }
                    catch (bx) { }
                }

            }
            else {
                if (bA && "get" in bA && (bD = bA.get(by, false, bz)) !== bq) {
                    return bD
                }
                return bE[bB]
            }

        }
        , css: function (e, bz, bx) {
            var bA, by;
            bz = Z.camelCase(bz);
            by = Z.cssHooks[bz];
            bz = Z.cssProps[bz] || bz;
            if (bz === "cssFloat") {
                bz = "float"
            }
            if (by && "get" in by && (bA = by.get(e, true, bx)) !== bq) {
                return bA
            }
            else {
                if (w) {
                    return w(e, bz)
                }

            }

        }
        , swap: function (bx, bA, e) {
            var bz = {};
            for (var by in bA) {
                bz[by] = bx.style[by];
                bx.style[by] = bA[by]
            }
            e.call(bx);
            for (by in bA) {
                bx.style[by] = bz[by]
            }

        }

    });
    Z.curCSS = Z.css;
    Z.each(["height", "width"], function (e, bx) {
        Z.cssHooks[bx] = {
            get: function (bz, by, bA) {
                var bB;
                if (by) {
                    if (bz.offsetWidth !== 0) {
                        return O(bz, bx, bA)
                    }
                    else {
                        Z.swap(bz, u, function () {
                            bB = O(bz, bx, bA)
                        })
                    }
                    return bB
                }

            }
            , set: function (by, bz) {
                if (aU.test(bz)) {
                    bz = parseFloat(bz);
                    if (bz >= 0) {
                        return bz + "px"
                    }

                }
                else {
                    return bz
                }

            }

        }

    });
    if (!Z.support.opacity) {
        Z.cssHooks.opacity = {
            get: function (bx, e) {
                return aW.test((e && bx.currentStyle ? bx.currentStyle.filter : bx.style.filter) || "") ? (parseFloat(RegExp.$1) / 100) + "" : e ? "1" : ""
            }
            , set: function (bx, bB) {
                var bA = bx.style, e = bx.currentStyle, bz = Z.isNumeric(bB) ? "alpha(opacity=" + bB * 100 + ")" : "", by = e && e.filter || bA.filter || "";
                bA.zoom = 1;
                if (bB >= 1 && Z.trim(by.replace(al, "")) === "") {
                    bA.removeAttribute("filter");
                    if (e && !e.filter) {
                        return
                    }

                }
                bA.filter = al.test(by) ? by.replace(al, bz) : by + " " + bz
            }

        }

    }
    Z(function () {
        if (!Z.support.reliableMarginRight) {
            Z.cssHooks.marginRight = {
                get: function (bx, e) {
                    var by;
                    Z.swap(bx, {
                        display: "inline-block"
                    }
                    , function () {
                        if (e) {
                            by = w(bx, "margin-right", "marginRight")
                        }
                        else {
                            by = bx.style.marginRight
                        }

                    });
                    return by
                }

            }

        }

    });
    if (A.defaultView && A.defaultView.getComputedStyle) {
        M = function (by, bz) {
            var bA, bx, e;
            bz = bz.replace(bi, "-$1").toLowerCase();
            if (!(bx = by.ownerDocument.defaultView)) {
                return bq
            }
            if ((e = bx.getComputedStyle(by, null))) {
                bA = e.getPropertyValue(bz);
                if (bA === "" && !Z.contains(by.ownerDocument.documentElement, by)) {
                    bA = Z.style(by, bz)
                }

            }
            return bA
        }

    }
    if (A.documentElement.currentStyle) {
        x = function (e, by) {
            var bx, bA, bC, bz = e.currentStyle && e.currentStyle[by], bB = e.style;
            if (bz === null && bB && (bC = bB[by])) {
                bz = bC
            }
            if (!aU.test(bz) && aT.test(bz)) {
                bx = bB.left;
                bA = e.runtimeStyle && e.runtimeStyle.left;
                if (bA) {
                    e.runtimeStyle.left = e.currentStyle.left
                }
                bB.left = by === "fontSize" ? "1em" : (bz || 0);
                bz = bB.pixelLeft + "px";
                bB.left = bx;
                if (bA) {
                    e.runtimeStyle.left = bA
                }

            }
            return bz === "" ? "auto" : bz
        }

    }
    w = M || x;
    function O(e, by, bx) {
        var bz = by === "width" ? e.offsetWidth : e.offsetHeight, bA = by === "width" ? v : t;
        if (bz > 0) {
            if (bx !== "border") {
                Z.each(bA, function () {
                    if (!bx) {
                        bz -= parseFloat(Z.css(e, "padding" + this)) || 0
                    }
                    if (bx === "margin") {
                        bz += parseFloat(Z.css(e, bx + this)) || 0
                    }
                    else {
                        bz -= parseFloat(Z.css(e, "border" + this + "Width")) || 0
                    }

                })
            }
            return bz + "px"
        }
        bz = w(e, by, by);
        if (bz < 0 || bz == null) {
            bz = e.style[by] || 0
        }
        bz = parseFloat(bz) || 0;
        if (bx) {
            Z.each(bA, function () {
                bz += parseFloat(Z.css(e, "padding" + this)) || 0;
                if (bx !== "padding") {
                    bz += parseFloat(Z.css(e, "border" + this + "Width")) || 0
                }
                if (bx === "margin") {
                    bz += parseFloat(Z.css(e, bx + this)) || 0
                }

            })
        }
        return bz + "px"
    }
    if (Z.expr && Z.expr.filters) {
        Z.expr.filters.hidden = function (e) {
            var by = e.offsetWidth, bx = e.offsetHeight;
            return (by === 0 && bx === 0) || (!Z.support.reliableHiddenOffsets && ((e.style && e.style.display) || Z.css(e, "display")) === "none")
        };
        Z.expr.filters.visible = function (e) {
            return !Z.expr.filters.hidden(e)
        }

    }
    var ak = /%20/g, ao = /\[\]$/, au = /\r?\n/g, aC = /#.*$/, aD = /^(.*?):[ \t]*([^\r\n]*)\r?$/mg, aH = /^(?:color|date|datetime|datetime-local|email|hidden|month|number|password|range|search|tel|text|time|url|week)$/i, aK = /^(?:about|app|app\-storage|.+\-extension|file|res|widget):$/, aQ = /^(?:GET|HEAD)$/, aZ = /^\/\//, a0 = /\?/, a5 = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi, a7 = /^(?:select|textarea)/i, ba = /\s+/, be = /([?&])_=[^&]*/, bj = /^([\w\+\.\-]+:)(?:\/\/([^\/?#:]*)(?::(\d+))?)?/, a = Z.fn.load, ah = {}, bp = {}, g, h, i = ["*/"] + ["*"];
    try {
        g = ac.href
    }
    catch (B) {
        g = A.createElement("a");
        g.href = "";
        g = g.href
    }
    h = bj.exec(g.toLowerCase()) || [];
    function b(e) {
        return function (by, bA) {
            if (typeof by !== "string") {
                bA = by;
                by = "*"
            }
            if (Z.isFunction(bA)) {
                var bz = by.toLowerCase().split(ba), bB = 0, bC = bz.length, bx, bD, bE;
                for (;
                bB < bC;
                bB++) {
                    bx = bz[bB];
                    bE = /^\+/.test(bx);
                    if (bE) {
                        bx = bx.substr(1) || "*"
                    }
                    bD = e[bx] = e[bx] || [];
                    bD[bE ? "unshift" : "push"](bA)
                }

            }

        }

    }
    function V(bG, bD, bE, bA, e, bz) {
        e = e || bD.dataTypes[0];
        bz = bz || {};
        bz[e] = true;
        var bC = bG[e], by = 0, bB = bC ? bC.length : 0, bx = (bG === ah), bF;
        for (;
        by < bB && (bx || !bF) ;
        by++) {
            bF = bC[by](bD, bE, bA);
            if (typeof bF === "string") {
                if (!bx || bz[bF]) {
                    bF = bq
                }
                else {
                    bD.dataTypes.unshift(bF);
                    bF = V(bG, bD, bE, bA, bF, bz)
                }

            }

        }
        if ((bx || !bF) && !bz["*"]) {
            bF = V(bG, bD, bE, bA, "*", bz)
        }
        return bF
    }
    function d(bA, bz) {
        var by, e, bx = Z.ajaxSettings.flatOptions || {};
        for (by in bz) {
            if (bz[by] !== bq) {
                (bx[by] ? bA : (e || (e = {})))[by] = bz[by]
            }

        }
        if (e) {
            Z.extend(true, bA, e)
        }

    }
    Z.fn.extend({
        load: function (bC, by, e) {
            if (typeof bC !== "string" && a) {
                return a.apply(this, arguments)
            }
            else {
                if (!this.length) {
                    return this
                }

            }
            var bx = bC.indexOf(" ");
            if (bx >= 0) {
                var bz = bC.slice(bx, bC.length);
                bC = bC.slice(0, bx)
            }
            var bB = "GET";
            if (by) {
                if (Z.isFunction(by)) {
                    e = by;
                    by = bq
                }
                else {
                    if (typeof by === "object") {
                        by = Z.param(by, Z.ajaxSettings.traditional);
                        bB = "POST"
                    }

                }

            }
            var bA = this;
            Z.ajax({
                url: bC, type: bB, dataType: "html", data: by, complete: function (bD, bF, bE) {
                    bE = bD.responseText;
                    if (bD.isResolved()) {
                        bD.done(function (bG) {
                            bE = bG
                        });
                        bA.html(bz ? Z("<div>").append(bE.replace(a5, "")).find(bz) : bE)
                    }
                    if (e) {
                        bA.each(e, [bE, bF, bD])
                    }

                }

            });
            return this
        }
        , serialize: function () {
            return Z.param(this.serializeArray())
        }
        , serializeArray: function () {
            return this.map(function () {
                return this.elements ? Z.makeArray(this.elements) : this
            }).filter(function () {
                return this.name && !this.disabled && (this.checked || a7.test(this.nodeName) || aH.test(this.type))
            }).map(function (bx, e) {
                var by = Z(this).val();
                return by == null ? null : Z.isArray(by) ? Z.map(by, function (bA, bz) {
                    return {
                        name: e.name, value: bA.replace(au, "\r\n")
                    }

                }) : {
                    name: e.name, value: by.replace(au, "\r\n")
                }

            }).get()
        }

    });
    Z.each("ajaxStart ajaxStop ajaxComplete ajaxError ajaxSuccess ajaxSend".split(" "), function (e, bx) {
        Z.fn[bx] = function (by) {
            return this.bind(bx, by)
        }

    });
    Z.each(["get", "post"], function (e, bx) {
        Z[bx] = function (bB, bz, by, bA) {
            if (Z.isFunction(bz)) {
                bA = bA || by;
                by = bz;
                bz = bq
            }
            return Z.ajax({
                type: bx, url: bB, data: bz, success: by, dataType: bA
            })
        }

    });
    Z.extend({
        getScript: function (bx, e) {
            return Z.get(bx, bq, e, "script")
        }
        , getJSON: function (by, bx, e) {
            return Z.get(by, bx, e, "json")
        }
        , ajaxSetup: function (bx, e) {
            if (e) {
                d(bx, Z.ajaxSettings)
            }
            else {
                e = bx;
                bx = Z.ajaxSettings
            }
            d(bx, e);
            return bx
        }
        , ajaxSettings: {
            url: g, isLocal: aK.test(h[1]), global: true, type: "GET", contentType: "application/x-www-form-urlencoded", processData: true, async: true, accepts: {
                xml: "application/xml, text/xml", html: "text/html", text: "text/plain", json: "application/json, text/javascript", "*": i
            }
            , contents: {
                xml: /xml/, html: /html/, json: /json/
            }
            , responseFields: {
                xml: "responseXML", text: "responseText"
            }
            , converters: {
                "* text": br.String, "text html": true, "text json": Z.parseJSON, "text xml": Z.parseXML
            }
            , flatOptions: {
                context: true, url: true
            }

        }
        , ajaxPrefilter: b(ah), ajaxTransport: b(bp), ajax: function (bU, bH) {
            if (typeof bU === "object") {
                bH = bU;
                bU = bq
            }
            bH = bH || {};
            var bO = Z.ajaxSetup({}, bH), bx = bO.context || bO, bD = bx !== bO && (bx.nodeType || bx instanceof Z) ? Z(bx) : Z.event, bz = Z.Deferred(), by = Z.Callbacks("once memory"), bQ = bO.statusCode || {}, bF, bJ = {}, bK = {}, bM, bL, bS, bR, bI, bP = 0, bC, bE, bG = {
                readyState: 0, setRequestHeader: function (bV, bW) {
                    if (!bP) {
                        var e = bV.toLowerCase();
                        bV = bK[e] = bK[e] || bV;
                        bJ[bV] = bW
                    }
                    return this
                }
                , getAllResponseHeaders: function () {
                    return bP === 2 ? bM : null
                }
                , getResponseHeader: function (e) {
                    var bV;
                    if (bP === 2) {
                        if (!bL) {
                            bL = {};
                            while ((bV = aD.exec(bM))) {
                                bL[bV[1].toLowerCase()] = bV[2]
                            }

                        }
                        bV = bL[e.toLowerCase()]
                    }
                    return bV === bq ? null : bV
                }
                , overrideMimeType: function (e) {
                    if (!bP) {
                        bO.mimeType = e
                    }
                    return this
                }
                , abort: function (e) {
                    e = e || "abort";
                    if (bS) {
                        bS.abort(e)
                    }
                    bA(0, e);
                    return this
                }

            };
            function bA(b4, b1, b3, bY) {
                if (bP === 2) {
                    return
                }
                bP = 2;
                if (bR) {
                    clearTimeout(bR)
                }
                bS = bq;
                bM = bY || "";
                bG.readyState = b4 > 0 ? 4 : 0;
                var bZ, b6, bW, b5 = b1, b2 = b3 ? f(bO, bG, b3) : bq, b0, bX;
                if (b4 >= 200 && b4 < 300 || b4 === 304) {
                    if (bO.ifModified) {
                        if ((b0 = bG.getResponseHeader("Last-Modified"))) {
                            Z.lastModified[bF] = b0
                        }
                        if ((bX = bG.getResponseHeader("Etag"))) {
                            Z.etag[bF] = bX
                        }

                    }
                    if (b4 === 304) {
                        b5 = "notmodified";
                        bZ = true
                    }
                    else {
                        try {
                            b6 = c(bO, b2);
                            b5 = "success";
                            bZ = true
                        }
                        catch (bV) {
                            b5 = "parsererror";
                            bW = bV
                        }

                    }

                }
                else {
                    bW = b5;
                    if (!b5 || b4) {
                        b5 = "error";
                        if (b4 < 0) {
                            b4 = 0
                        }

                    }

                }
                bG.status = b4;
                bG.statusText = "" + (b1 || b5);
                if (bZ) {
                    bz.resolveWith(bx, [b6, b5, bG])
                }
                else {
                    bz.rejectWith(bx, [bG, b5, bW])
                }
                bG.statusCode(bQ);
                bQ = bq;
                if (bC) {
                    bD.trigger("ajax" + (bZ ? "Success" : "Error"), [bG, bO, bZ ? b6 : bW])
                }
                by.fireWith(bx, [bG, b5]);
                if (bC) {
                    bD.trigger("ajaxComplete", [bG, bO]);
                    if (!(--Z.active)) {
                        Z.event.trigger("ajaxStop")
                    }

                }

            }
            bz.promise(bG);
            bG.success = bG.done;
            bG.error = bG.fail;
            bG.complete = by.add;
            bG.statusCode = function (e) {
                if (e) {
                    var bV;
                    if (bP < 2) {
                        for (bV in e) {
                            bQ[bV] = [bQ[bV], e[bV]]
                        }

                    }
                    else {
                        bV = e[bG.status];
                        bG.then(bV, bV)
                    }

                }
                return this
            };
            bO.url = ((bU || bO.url) + "").replace(aC, "").replace(aZ, h[1] + "//");
            bO.dataTypes = Z.trim(bO.dataType || "*").toLowerCase().split(ba);
            if (bO.crossDomain == null) {
                bI = bj.exec(bO.url.toLowerCase());
                bO.crossDomain = !!(bI && (bI[1] != h[1] || bI[2] != h[2] || (bI[3] || (bI[1] === "http:" ? 80 : 443)) != (h[3] || (h[1] === "http:" ? 80 : 443))))
            }
            if (bO.data && bO.processData && typeof bO.data !== "string") {
                bO.data = Z.param(bO.data, bO.traditional)
            }
            V(ah, bO, bH, bG);
            if (bP === 2) {
                return false
            }
            bC = bO.global;
            bO.type = bO.type.toUpperCase();
            bO.hasContent = !aQ.test(bO.type);
            if (bC && Z.active++ === 0) {
                Z.event.trigger("ajaxStart")
            }
            if (!bO.hasContent) {
                if (bO.data) {
                    bO.url += (a0.test(bO.url) ? "&" : "?") + bO.data;
                    delete bO.data
                }
                bF = bO.url;
                if (bO.cache === false) {
                    var bT = Z.now(), bN = bO.url.replace(be, "$1_=" + bT);
                    bO.url = bN + ((bN === bO.url) ? (a0.test(bO.url) ? "&" : "?") + "_=" + bT : "")
                }

            }
            if (bO.data && bO.hasContent && bO.contentType !== false || bH.contentType) {
                bG.setRequestHeader("Content-Type", bO.contentType)
            }
            if (bO.ifModified) {
                bF = bF || bO.url;
                if (Z.lastModified[bF]) {
                    bG.setRequestHeader("If-Modified-Since", Z.lastModified[bF])
                }
                if (Z.etag[bF]) {
                    bG.setRequestHeader("If-None-Match", Z.etag[bF])
                }

            }
            bG.setRequestHeader("Accept", bO.dataTypes[0] && bO.accepts[bO.dataTypes[0]] ? bO.accepts[bO.dataTypes[0]] + (bO.dataTypes[0] !== "*" ? ", " + i + "; q=0.01" : "") : bO.accepts["*"]);
            for (bE in bO.headers) {
                bG.setRequestHeader(bE, bO.headers[bE])
            }
            if (bO.beforeSend && (bO.beforeSend.call(bx, bG, bO) === false || bP === 2)) {
                bG.abort();
                return false
            }
            for (bE in {
                success: 1, error: 1, complete: 1
            }) {
                bG[bE](bO[bE])
            }
            bS = V(bp, bO, bH, bG);
            if (!bS) {
                bA(-1, "No Transport")
            }
            else {
                bG.readyState = 1;
                if (bC) {
                    bD.trigger("ajaxSend", [bG, bO])
                }
                if (bO.async && bO.timeout > 0) {
                    bR = setTimeout(function () {
                        bG.abort("timeout")
                    }
                    , bO.timeout)
                }
                try {
                    bP = 1;
                    bS.send(bJ, bA)
                }
                catch (bB) {
                    if (bP < 2) {
                        bA(-1, bB)
                    }
                    else {
                        Z.error(bB)
                    }

                }

            }
            return bG
        }
        , param: function (e, bA) {
            var bz = [], bx = function (bB, bC) {
                bC = Z.isFunction(bC) ? bC() : bC;
                bz[bz.length] = encodeURIComponent(bB) + "=" + encodeURIComponent(bC)
            };
            if (bA === bq) {
                bA = Z.ajaxSettings.traditional
            }
            if (Z.isArray(e) || (e.jquery && !Z.isPlainObject(e))) {
                Z.each(e, function () {
                    bx(this.name, this.value)
                })
            }
            else {
                for (var by in e) {
                    k(by, e[by], bA, bx)
                }

            }
            return bz.join("&").replace(ak, "+")
        }

    });
    function k(bz, by, bA, e) {
        if (Z.isArray(by)) {
            Z.each(by, function (bB, bC) {
                if (bA || ao.test(bz)) {
                    e(bz, bC)
                }
                else {
                    k(bz + "[" + (typeof bC === "object" || Z.isArray(bC) ? bB : "") + "]", bC, bA, e)
                }

            })
        }
        else {
            if (!bA && by != null && typeof by === "object") {
                for (var bx in by) {
                    k(bz + "[" + bx + "]", by[bx], bA, e)
                }

            }
            else {
                e(bz, by)
            }

        }

    }
    Z.extend({
        active: 0, lastModified: {}, etag: {}
    });
    function f(bE, bB, bD) {
        var e = bE.contents, by = bE.dataTypes, bC = bE.responseFields, bx, bF, bz, bA;
        for (bF in bC) {
            if (bF in bD) {
                bB[bC[bF]] = bD[bF]
            }

        }
        while (by[0] === "*") {
            by.shift();
            if (bx === bq) {
                bx = bE.mimeType || bB.getResponseHeader("content-type")
            }

        }
        if (bx) {
            for (bF in e) {
                if (e[bF] && e[bF].test(bx)) {
                    by.unshift(bF);
                    break
                }

            }

        }
        if (by[0] in bD) {
            bz = by[0]
        }
        else {
            for (bF in bD) {
                if (!by[0] || bE.converters[bF + " " + by[0]]) {
                    bz = bF;
                    break
                }
                if (!bA) {
                    bA = bF
                }

            }
            bz = bz || bA
        }
        if (bz) {
            if (bz !== by[0]) {
                by.unshift(bz)
            }
            return bD[bz]
        }

    }
    function c(bI, bH) {
        if (bI.dataFilter) {
            bH = bI.dataFilter(bH, bI.dataType)
        }
        var bC = bI.dataTypes, bA = {}, bD, bE, bF = bC.length, bJ, bB = bC[0], bG, bz, e, bx, by;
        for (bD = 1;
        bD < bF;
        bD++) {
            if (bD === 1) {
                for (bE in bI.converters) {
                    if (typeof bE === "string") {
                        bA[bE.toLowerCase()] = bI.converters[bE]
                    }

                }

            }
            bG = bB;
            bB = bC[bD];
            if (bB === "*") {
                bB = bG
            }
            else {
                if (bG !== "*" && bG !== bB) {
                    bz = bG + " " + bB;
                    e = bA[bz] || bA["* " + bB];
                    if (!e) {
                        by = bq;
                        for (bx in bA) {
                            bJ = bx.split(" ");
                            if (bJ[0] === bG || bJ[0] === "*") {
                                by = bA[bJ[1] + " " + bB];
                                if (by) {
                                    bx = bA[bx];
                                    if (bx === true) {
                                        e = by
                                    }
                                    else {
                                        if (by === true) {
                                            e = bx
                                        }

                                    }
                                    break
                                }

                            }

                        }

                    }
                    if (!(e || by)) {
                        Z.error("No conversion from " + bz.replace(" ", " to "))
                    }
                    if (e !== true) {
                        bH = e ? e(bH) : by(bx(bH))
                    }

                }

            }

        }
        return bH
    }
    var aa = Z.now(), ab = /(\=)\?(&|$)|\?\?/i;
    Z.ajaxSetup({
        jsonp: "callback", jsonpCallback: function () {
            return Z.expando + "_" + (aa++)
        }

    });
    Z.ajaxPrefilter("json jsonp", function (bE, bA, by) {
        var bx = bE.contentType === "application/x-www-form-urlencoded" && (typeof bE.data === "string");
        if (bE.dataTypes[0] === "jsonp" || bE.jsonp !== false && (ab.test(bE.url) || bx && ab.test(bE.data))) {
            var bD, bz = bE.jsonpCallback = Z.isFunction(bE.jsonpCallback) ? bE.jsonpCallback() : bE.jsonpCallback, bB = br[bz], bF = bE.url, e = bE.data, bC = "$1" + bz + "$2";
            if (bE.jsonp !== false) {
                bF = bF.replace(ab, bC);
                if (bE.url === bF) {
                    if (bx) {
                        e = e.replace(ab, bC)
                    }
                    if (bE.data === e) {
                        bF += (/\?/.test(bF) ? "&" : "?") + bE.jsonp + "=" + bz
                    }

                }

            }
            bE.url = bF;
            bE.data = e;
            br[bz] = function (bG) {
                bD = [bG]
            };
            by.always(function () {
                br[bz] = bB;
                if (bD && Z.isFunction(bB)) {
                    br[bz](bD[0])
                }

            });
            bE.converters["script json"] = function () {
                if (!bD) {
                    Z.error(bz + " was not called")
                }
                return bD[0]
            };
            bE.dataTypes[0] = "json";
            return "script"
        }

    });
    Z.ajaxSetup({
        accepts: {
            script: "text/javascript, application/javascript, application/ecmascript, application/x-ecmascript"
        }
        , contents: {
            script: /javascript|ecmascript/
        }
        , converters: {
            "text script": function (e) {
                Z.globalEval(e);
                return e
            }

        }

    });
    Z.ajaxPrefilter("script", function (e) {
        if (e.cache === bq) {
            e.cache = false
        }
        if (e.crossDomain) {
            e.type = "GET";
            e.global = false
        }

    });
    Z.ajaxTransport("script", function (bx) {
        if (bx.crossDomain) {
            var by, e = A.head || A.getElementsByTagName("head")[0] || A.documentElement;
            return {
                send: function (bz, bA) {
                    by = A.createElement("script");
                    by.async = "async";
                    if (bx.scriptCharset) {
                        by.charset = bx.scriptCharset
                    }
                    by.src = bx.url;
                    by.onload = by.onreadystatechange = function (bB, bC) {
                        if (bC || !by.readyState || /loaded|complete/.test(by.readyState)) {
                            by.onload = by.onreadystatechange = null;
                            if (e && by.parentNode) {
                                e.removeChild(by)
                            }
                            by = bq;
                            if (!bC) {
                                bA(200, "success")
                            }

                        }

                    };
                    e.insertBefore(by, e.firstChild)
                }
                , abort: function () {
                    if (by) {
                        by.onload(0, 1)
                    }

                }

            }

        }

    });
    var bw = br.ActiveXObject ? function () {
        for (var e in bu) {
            bu[e](0, 1)
        }

    }
    : false, bv = 0, bu;
    function s() {
        try {
            return new br.XMLHttpRequest()
        }
        catch (bx) { }
    }
    function o() {
        try {
            return new br.ActiveXObject("Microsoft.XMLHTTP")
        }
        catch (bx) { }
    }
    Z.ajaxSettings.xhr = br.ActiveXObject ? function () {
        return !this.isLocal && s() || o()
    }
    : s;
    (function (e) {
        Z.extend(Z.support, {
            ajax: !!e, cors: !!e && ("withCredentials" in e)
        })
    })(Z.ajaxSettings.xhr());
    if (Z.support.ajax) {
        Z.ajaxTransport(function (bx) {
            if (!bx.crossDomain || Z.support.cors) {
                var e;
                return {
                    send: function (bB, bz) {
                        var bD = bx.xhr(), bA, bC;
                        if (bx.username) {
                            bD.open(bx.type, bx.url, bx.async, bx.username, bx.password)
                        }
                        else {
                            bD.open(bx.type, bx.url, bx.async)
                        }
                        if (bx.xhrFields) {
                            for (bC in bx.xhrFields) {
                                bD[bC] = bx.xhrFields[bC]
                            }

                        }
                        if (bx.mimeType && bD.overrideMimeType) {
                            bD.overrideMimeType(bx.mimeType)
                        }
                        if (!bx.crossDomain && !bB["X-Requested-With"]) {
                            bB["X-Requested-With"] = "XMLHttpRequest"
                        }
                        try {
                            for (bC in bB) {
                                bD.setRequestHeader(bC, bB[bC])
                            }

                        }
                        catch (by) { } bD.send((bx.hasContent && bx.data) || null);
                        e = function (bE, bH) {
                            var bK, bL, bI, bJ, bM;
                            try {
                                if (e && (bH || bD.readyState === 4)) {
                                    e = bq;
                                    if (bA) {
                                        bD.onreadystatechange = Z.noop;
                                        if (bw) {
                                            delete bu[bA]
                                        }

                                    }
                                    if (bH) {
                                        if (bD.readyState !== 4) {
                                            bD.abort()
                                        }

                                    }
                                    else {
                                        bK = bD.status;
                                        bI = bD.getAllResponseHeaders();
                                        bJ = {};
                                        bM = bD.responseXML;
                                        if (bM && bM.documentElement) {
                                            bJ.xml = bM
                                        }
                                        bJ.text = bD.responseText;
                                        try {
                                            bL = bD.statusText
                                        }
                                        catch (bF) {
                                            bL = ""
                                        }
                                        if (!bK && bx.isLocal && !bx.crossDomain) {
                                            bK = bJ.text ? 200 : 404
                                        }
                                        else {
                                            if (bK === 1223) {
                                                bK = 204
                                            }

                                        }

                                    }

                                }

                            }
                            catch (bG) {
                                if (!bH) {
                                    bz(-1, bG)
                                }

                            }
                            if (bJ) {
                                bz(bK, bL, bJ, bI)
                            }

                        };
                        if (!bx.async || bD.readyState === 4) {
                            e()
                        }
                        else {
                            bA = ++bv;
                            if (bw) {
                                if (!bu) {
                                    bu = {};
                                    Z(br).unload(bw)
                                }
                                bu[bA] = e
                            }
                            bD.onreadystatechange = e
                        }

                    }
                    , abort: function () {
                        if (e) {
                            e(0, 1)
                        }

                    }

                }

            }

        })
    }
    var C = {}, T, U, aB = /^(?:toggle|show|hide)$/, aA = /^([+\-]=)?([\d+.\-]+)([a-z%]*)$/i, bo, I = [["height", "marginTop", "marginBottom", "paddingTop", "paddingBottom"], ["width", "marginLeft", "marginRight", "paddingLeft", "paddingRight"], ["opacity"]], J;
    Z.fn.extend({
        show: function (bC, by, e) {
            var bz, bx;
            if (bC || bC === 0) {
                return this.animate(K("show", 3), bC, by, e)
            }
            else {
                for (var bA = 0, bB = this.length;
                bA < bB;
                bA++) {
                    bz = this[bA];
                    if (bz.style) {
                        bx = bz.style.display;
                        if (!Z._data(bz, "olddisplay") && bx === "none") {
                            bx = bz.style.display = ""
                        }
                        if (bx === "" && Z.css(bz, "display") === "none") {
                            Z._data(bz, "olddisplay", z(bz.nodeName))
                        }

                    }

                }
                for (bA = 0;
                bA < bB;
                bA++) {
                    bz = this[bA];
                    if (bz.style) {
                        bx = bz.style.display;
                        if (bx === "" || bx === "none") {
                            bz.style.display = Z._data(bz, "olddisplay") || ""
                        }

                    }

                }
                return this
            }

        }
        , hide: function (bC, by, e) {
            if (bC || bC === 0) {
                return this.animate(K("hide", 3), bC, by, e)
            }
            else {
                var bz, bx, bA = 0, bB = this.length;
                for (;
                bA < bB;
                bA++) {
                    bz = this[bA];
                    if (bz.style) {
                        bx = Z.css(bz, "display");
                        if (bx !== "none" && !Z._data(bz, "olddisplay")) {
                            Z._data(bz, "olddisplay", bx)
                        }

                    }

                }
                for (bA = 0;
                bA < bB;
                bA++) {
                    if (this[bA].style) {
                        this[bA].style.display = "none"
                    }

                }
                return this
            }

        }
        , _toggle: Z.fn.toggle, toggle: function (by, bz, bx) {
            var e = typeof by === "boolean";
            if (Z.isFunction(by) && Z.isFunction(bz)) {
                this._toggle.apply(this, arguments)
            }
            else {
                if (by == null || e) {
                    this.each(function () {
                        var bA = e ? by : Z(this).is(":hidden");
                        Z(this)[bA ? "show" : "hide"]()
                    })
                }
                else {
                    this.animate(K("toggle", 3), by, bz, bx)
                }

            }
            return this
        }
        , fadeTo: function (by, bz, bx, e) {
            return this.filter(":hidden").css("opacity", 0).show().end().animate({
                opacity: bz
            }
            , by, bx, e)
        }
        , animate: function (bA, bB, by, e) {
            var bz = Z.speed(bB, by, e);
            if (Z.isEmptyObject(bA)) {
                return this.each(bz.complete, [false])
            }
            bA = Z.extend({}, bA);
            function bx() {
                if (bz.queue === false) {
                    Z._mark(this)
                }
                var bI = Z.extend({}, bz), bF = this.nodeType === 1, bE = bF && Z(this).is(":hidden"), bH, bN, bJ, bC, bK, bL, bD, bM, bG;
                bI.animatedProperties = {};
                for (bJ in bA) {
                    bH = Z.camelCase(bJ);
                    if (bJ !== bH) {
                        bA[bH] = bA[bJ];
                        delete bA[bJ]
                    }
                    bN = bA[bH];
                    if (Z.isArray(bN)) {
                        bI.animatedProperties[bH] = bN[1];
                        bN = bA[bH] = bN[0]
                    }
                    else {
                        bI.animatedProperties[bH] = bI.specialEasing && bI.specialEasing[bH] || bI.easing || "swing"
                    }
                    if (bN === "hide" && bE || bN === "show" && !bE) {
                        return bI.complete.call(this)
                    }
                    if (bF && (bH === "height" || bH === "width")) {
                        bI.overflow = [this.style.overflow, this.style.overflowX, this.style.overflowY];
                        if (Z.css(this, "display") === "inline" && Z.css(this, "float") === "none") {
                            if (!Z.support.inlineBlockNeedsLayout || z(this.nodeName) === "inline") {
                                this.style.display = "inline-block"
                            }
                            else {
                                this.style.zoom = 1
                            }

                        }

                    }

                }
                if (bI.overflow != null) {
                    this.style.overflow = "hidden"
                }
                for (bJ in bA) {
                    bC = new Z.fx(this, bI, bJ);
                    bN = bA[bJ];
                    if (aB.test(bN)) {
                        bG = Z._data(this, "toggle" + bJ) || (bN === "toggle" ? bE ? "show" : "hide" : 0);
                        if (bG) {
                            Z._data(this, "toggle" + bJ, bG === "show" ? "hide" : "show");
                            bC[bG]()
                        }
                        else {
                            bC[bN]()
                        }

                    }
                    else {
                        bK = aA.exec(bN);
                        bL = bC.cur();
                        if (bK) {
                            bD = parseFloat(bK[2]);
                            bM = bK[3] || (Z.cssNumber[bJ] ? "" : "px");
                            if (bM !== "px") {
                                Z.style(this, bJ, (bD || 1) + bM);
                                bL = ((bD || 1) / bC.cur()) * bL;
                                Z.style(this, bJ, bL + bM)
                            }
                            if (bK[1]) {
                                bD = ((bK[1] === "-=" ? -1 : 1) * bD) + bL
                            }
                            bC.custom(bL, bD, bM)
                        }
                        else {
                            bC.custom(bL, bN, "")
                        }

                    }

                }
                return true
            }
            return bz.queue === false ? this.each(bx) : this.queue(bz.queue, bx)
        }
        , stop: function (by, e, bx) {
            if (typeof by !== "string") {
                bx = e;
                e = by;
                by = bq
            }
            if (e && by !== false) {
                this.queue(by || "fx", [])
            }
            return this.each(function () {
                var bB, bA = false, bD = Z.timers, bz = Z._data(this);
                if (!bx) {
                    Z._unmark(true, this)
                }
                function bC(bF, bE, bH) {
                    var bG = bE[bH];
                    Z.removeData(bF, bH, true);
                    bG.stop(bx)
                }
                if (by == null) {
                    for (bB in bz) {
                        if (bz[bB].stop && bB.indexOf(".run") === bB.length - 4) {
                            bC(this, bz, bB)
                        }

                    }

                }
                else {
                    if (bz[bB = by + ".run"] && bz[bB].stop) {
                        bC(this, bz, bB)
                    }

                }
                for (bB = bD.length;
                bB--;
                ) {
                    if (bD[bB].elem === this && (by == null || bD[bB].queue === by)) {
                        if (bx) {
                            bD[bB](true)
                        }
                        else {
                            bD[bB].saveState()
                        }
                        bA = true;
                        bD.splice(bB, 1)
                    }

                }
                if (!(bx && bA)) {
                    Z.dequeue(this, by)
                }

            })
        }

    });
    function q() {
        setTimeout(l, 0);
        return (J = Z.now())
    }
    function l() {
        J = bq
    }
    function K(by, e) {
        var bx = {};
        Z.each(I.concat.apply([], I.slice(0, e)), function () {
            bx[this] = by
        });
        return bx
    }
    Z.each({
        slideDown: K("show", 1), slideUp: K("hide", 1), slideToggle: K("toggle", 1), fadeIn: {
            opacity: "show"
        }
        , fadeOut: {
            opacity: "hide"
        }
        , fadeToggle: {
            opacity: "toggle"
        }

    }
    , function (e, bx) {
        Z.fn[e] = function (bA, bz, by) {
            return this.animate(bx, bA, bz, by)
        }

    });
    Z.extend({
        speed: function (bz, e, bx) {
            var by = bz && typeof bz === "object" ? Z.extend({}, bz) : {
                complete: bx || !bx && e || Z.isFunction(bz) && bz, duration: bz, easing: bx && e || e && !Z.isFunction(e) && e
            };
            by.duration = Z.fx.off ? 0 : typeof by.duration === "number" ? by.duration : by.duration in Z.fx.speeds ? Z.fx.speeds[by.duration] : Z.fx.speeds._default;
            if (by.queue == null || by.queue === true) {
                by.queue = "fx"
            }
            by.old = by.complete;
            by.complete = function (bA) {
                if (Z.isFunction(by.old)) {
                    by.old.call(this)
                }
                if (by.queue) {
                    Z.dequeue(this, by.queue)
                }
                else {
                    if (bA !== false) {
                        Z._unmark(this)
                    }

                }

            };
            return by
        }
        , easing: {
            linear: function (bz, by, bx, e) {
                return bx + e * bz
            }
            , swing: function (bz, by, bx, e) {
                return ((-Math.cos(bz * Math.PI) / 2) + 0.5) * e + bx
            }

        }
        , timers: [], fx: function (e, bx, by) {
            this.options = bx;
            this.elem = e;
            this.prop = by;
            bx.orig = bx.orig || {}
        }

    });
    Z.fx.prototype = {
        update: function () {
            if (this.options.step) {
                this.options.step.call(this.elem, this.now, this)
            }
            (Z.fx.step[this.prop] || Z.fx.step._default)(this)
        }
        , cur: function () {
            if (this.elem[this.prop] != null && (!this.elem.style || this.elem.style[this.prop] == null)) {
                return this.elem[this.prop]
            }
            var e, bx = Z.css(this.elem, this.prop);
            return isNaN(e = parseFloat(bx)) ? !bx || bx === "auto" ? 0 : bx : e
        }
        , custom: function (e, bA, bB) {
            var by = this, bx = Z.fx;
            this.startTime = J || q();
            this.end = bA;
            this.now = this.start = e;
            this.pos = this.state = 0;
            this.unit = bB || this.unit || (Z.cssNumber[this.prop] ? "" : "px");
            function bz(bC) {
                return by.step(bC)
            }
            bz.queue = this.options.queue;
            bz.elem = this.elem;
            bz.saveState = function () {
                if (by.options.hide && Z._data(by.elem, "fxshow" + by.prop) === bq) {
                    Z._data(by.elem, "fxshow" + by.prop, by.start)
                }

            };
            if (bz() && Z.timers.push(bz) && !bo) {
                bo = setInterval(bx.tick, bx.interval)
            }

        }
        , show: function () {
            var e = Z._data(this.elem, "fxshow" + this.prop);
            this.options.orig[this.prop] = e || Z.style(this.elem, this.prop);
            this.options.show = true;
            if (e !== bq) {
                this.custom(this.cur(), e)
            }
            else {
                this.custom(this.prop === "width" || this.prop === "height" ? 1 : 0, this.cur())
            }
            Z(this.elem).show()
        }
        , hide: function () {
            this.options.orig[this.prop] = Z._data(this.elem, "fxshow" + this.prop) || Z.style(this.elem, this.prop);
            this.options.hide = true;
            this.custom(this.cur(), 0)
        }
        , step: function (bz) {
            var bC, bA, e, bD = J || q(), bx = true, by = this.elem, bB = this.options;
            if (bz || bD >= bB.duration + this.startTime) {
                this.now = this.end;
                this.pos = this.state = 1;
                this.update();
                bB.animatedProperties[this.prop] = true;
                for (bC in bB.animatedProperties) {
                    if (bB.animatedProperties[bC] !== true) {
                        bx = false
                    }

                }
                if (bx) {
                    if (bB.overflow != null && !Z.support.shrinkWrapBlocks) {
                        Z.each(["", "X", "Y"], function (bE, bF) {
                            by.style["overflow" + bF] = bB.overflow[bE]
                        })
                    }
                    if (bB.hide) {
                        Z(by).hide()
                    }
                    if (bB.hide || bB.show) {
                        for (bC in bB.animatedProperties) {
                            Z.style(by, bC, bB.orig[bC]);
                            Z.removeData(by, "fxshow" + bC, true);
                            Z.removeData(by, "toggle" + bC, true)
                        }

                    }
                    e = bB.complete;
                    if (e) {
                        bB.complete = false;
                        e.call(by)
                    }

                }
                return false
            }
            else {
                if (bB.duration == Infinity) {
                    this.now = bD
                }
                else {
                    bA = bD - this.startTime;
                    this.state = bA / bB.duration;
                    this.pos = Z.easing[bB.animatedProperties[this.prop]](this.state, bA, 0, 1, bB.duration);
                    this.now = this.start + ((this.end - this.start) * this.pos)
                }
                this.update()
            }
            return true
        }

    };
    Z.extend(Z.fx, {
        tick: function () {
            var bx, by = Z.timers, e = 0;
            for (;
            e < by.length;
            e++) {
                bx = by[e];
                if (!bx() && by[e] === bx) {
                    by.splice(e--, 1)
                }

            }
            if (!by.length) {
                Z.fx.stop()
            }

        }
        , interval: 13, stop: function () {
            clearInterval(bo);
            bo = null
        }
        , speeds: {
            slow: 600, fast: 200, _default: 400
        }
        , step: {
            opacity: function (e) {
                Z.style(e.elem, "opacity", e.now)
            }
            , _default: function (e) {
                if (e.elem.style && e.elem.style[e.prop] != null) {
                    e.elem.style[e.prop] = e.now + e.unit
                }
                else {
                    e.elem[e.prop] = e.now
                }

            }

        }

    });
    Z.each(["width", "height"], function (e, bx) {
        Z.fx.step[bx] = function (by) {
            Z.style(by.elem, bx, Math.max(0, by.now))
        }

    });
    if (Z.expr && Z.expr.filters) {
        Z.expr.filters.animated = function (e) {
            return Z.grep(Z.timers, function (bx) {
                return e === bx.elem
            }).length
        }

    }
    function z(bz) {
        if (!C[bz]) {
            var e = A.body, by = Z("<" + bz + ">").appendTo(e), bx = by.css("display");
            by.remove();
            if (bx === "none" || bx === "") {
                if (!T) {
                    T = A.createElement("iframe");
                    T.frameBorder = T.width = T.height = 0
                }
                e.appendChild(T);
                if (!U || !T.createElement) {
                    U = (T.contentWindow || T.contentDocument).document;
                    U.write((A.compatMode === "CSS1Compat" ? "<!doctype html>" : "") + "<html><body>");
                    U.close()
                }
                by = U.createElement(bz);
                U.body.appendChild(by);
                bx = Z.css(by, "display");
                e.removeChild(T)
            }
            C[bz] = bx
        }
        return C[bz]
    }
    var bb = /^t(?:able|d|h)$/i, a4 = /^(?:body|html)$/i;
    if ("getBoundingClientRect" in A.documentElement) {
        Z.fn.offset = function (bG) {
            var bE = this[0], by;
            if (bG) {
                return this.each(function (e) {
                    Z.offset.setOffset(this, bG, e)
                })
            }
            if (!bE || !bE.ownerDocument) {
                return null
            }
            if (bE === bE.ownerDocument.body) {
                return Z.offset.bodyOffset(bE)
            }
            try {
                by = bE.getBoundingClientRect()
            }
            catch (bD) { } var bB = bE.ownerDocument, bC = bB.documentElement;
            if (!by || !Z.contains(bC, bE)) {
                return by ? {
                    top: by.top, left: by.left
                }
                : {
                    top: 0, left: 0
                }

            }
            var bx = bB.body, bK = P(bB), bA = bC.clientTop || bx.clientTop || 0, bz = bC.clientLeft || bx.clientLeft || 0, bI = bK.pageYOffset || Z.support.boxModel && bC.scrollTop || bx.scrollTop, bH = bK.pageXOffset || Z.support.boxModel && bC.scrollLeft || bx.scrollLeft, bJ = by.top + bI - bA, bF = by.left + bH - bz;
            return {
                top: bJ, left: bF
            }

        }

    }
    else {
        Z.fn.offset = function (bE) {
            var bB = this[0];
            if (bE) {
                return this.each(function (bI) {
                    Z.offset.setOffset(this, bE, bI)
                })
            }
            if (!bB || !bB.ownerDocument) {
                return null
            }
            if (bB === bB.ownerDocument.body) {
                return Z.offset.bodyOffset(bB)
            }
            var bx, bD = bB.offsetParent, bG = bB, bz = bB.ownerDocument, bA = bz.documentElement, e = bz.body, by = bz.defaultView, bF = by ? by.getComputedStyle(bB, null) : bB.currentStyle, bH = bB.offsetTop, bC = bB.offsetLeft;
            while ((bB = bB.parentNode) && bB !== e && bB !== bA) {
                if (Z.support.fixedPosition && bF.position === "fixed") {
                    break
                }
                bx = by ? by.getComputedStyle(bB, null) : bB.currentStyle;
                bH -= bB.scrollTop;
                bC -= bB.scrollLeft;
                if (bB === bD) {
                    bH += bB.offsetTop;
                    bC += bB.offsetLeft;
                    if (Z.support.doesNotAddBorder && !(Z.support.doesAddBorderForTableAndCells && bb.test(bB.nodeName))) {
                        bH += parseFloat(bx.borderTopWidth) || 0;
                        bC += parseFloat(bx.borderLeftWidth) || 0
                    }
                    bG = bD;
                    bD = bB.offsetParent
                }
                if (Z.support.subtractsBorderForOverflowNotVisible && bx.overflow !== "visible") {
                    bH += parseFloat(bx.borderTopWidth) || 0;
                    bC += parseFloat(bx.borderLeftWidth) || 0
                }
                bF = bx
            }
            if (bF.position === "relative" || bF.position === "static") {
                bH += e.offsetTop;
                bC += e.offsetLeft
            }
            if (Z.support.fixedPosition && bF.position === "fixed") {
                bH += Math.max(bA.scrollTop, e.scrollTop);
                bC += Math.max(bA.scrollLeft, e.scrollLeft)
            }
            return {
                top: bH, left: bC
            }

        }

    }
    Z.offset = {
        bodyOffset: function (e) {
            var by = e.offsetTop, bx = e.offsetLeft;
            if (Z.support.doesNotIncludeMarginInBodyOffset) {
                by += parseFloat(Z.css(e, "marginTop")) || 0;
                bx += parseFloat(Z.css(e, "marginLeft")) || 0
            }
            return {
                top: by, left: bx
            }

        }
        , setOffset: function (bE, bG, bF) {
            var bH = Z.css(bE, "position");
            if (bH === "static") {
                bE.style.position = "relative"
            }
            var bz = Z(bE), bB = bz.offset(), by = Z.css(bE, "top"), bx = Z.css(bE, "left"), e = (bH === "absolute" || bH === "fixed") && Z.inArray("auto", [by, bx]) > -1, bI = {}, bC = {}, bD, bA;
            if (e) {
                bC = bz.position();
                bD = bC.top;
                bA = bC.left
            }
            else {
                bD = parseFloat(by) || 0;
                bA = parseFloat(bx) || 0
            }
            if (Z.isFunction(bG)) {
                bG = bG.call(bE, bF, bB)
            }
            if (bG.top != null) {
                bI.top = (bG.top - bB.top) + bD
            }
            if (bG.left != null) {
                bI.left = (bG.left - bB.left) + bA
            }
            if ("using" in bG) {
                bG.using.call(bE, bI)
            }
            else {
                bz.css(bI)
            }

        }

    };
    Z.fn.extend({
        position: function () {
            if (!this[0]) {
                return null
            }
            var e = this[0], by = this.offsetParent(), bx = this.offset(), bz = a4.test(by[0].nodeName) ? {
                top: 0, left: 0
            }
            : by.offset();
            bx.top -= parseFloat(Z.css(e, "marginTop")) || 0;
            bx.left -= parseFloat(Z.css(e, "marginLeft")) || 0;
            bz.top += parseFloat(Z.css(by[0], "borderTopWidth")) || 0;
            bz.left += parseFloat(Z.css(by[0], "borderLeftWidth")) || 0;
            return {
                top: bx.top - bz.top, left: bx.left - bz.left
            }

        }
        , offsetParent: function () {
            return this.map(function () {
                var e = this.offsetParent || A.body;
                while (e && (!a4.test(e.nodeName) && Z.css(e, "position") === "static")) {
                    e = e.offsetParent
                }
                return e
            })
        }

    });
    Z.each(["Left", "Top"], function (e, by) {
        var bx = "scroll" + by;
        Z.fn[bx] = function (bA) {
            var bz, bB;
            if (bA === bq) {
                bz = this[0];
                if (!bz) {
                    return null
                }
                bB = P(bz);
                return bB ? ("pageXOffset" in bB) ? bB[e ? "pageYOffset" : "pageXOffset"] : Z.support.boxModel && bB.document.documentElement[bx] || bB.document.body[bx] : bz[bx]
            }
            return this.each(function () {
                bB = P(this);
                if (bB) {
                    bB.scrollTo(!e ? bA : Z(bB).scrollLeft(), e ? bA : Z(bB).scrollTop())
                }
                else {
                    this[bx] = bA
                }

            })
        }

    });
    function P(e) {
        return Z.isWindow(e) ? e : e.nodeType === 9 ? e.defaultView || e.parentWindow : false
    }
    Z.each(["Height", "Width"], function (e, bx) {
        var by = bx.toLowerCase();
        Z.fn["inner" + bx] = function () {
            var bz = this[0];
            return bz ? bz.style ? parseFloat(Z.css(bz, by, "padding")) : this[by]() : null
        };
        Z.fn["outer" + bx] = function (bA) {
            var bz = this[0];
            return bz ? bz.style ? parseFloat(Z.css(bz, by, bA ? "margin" : "border")) : this[by]() : null
        };
        Z.fn[by] = function (bE) {
            var bB = this[0];
            if (!bB) {
                return bE == null ? null : this
            }
            if (Z.isFunction(bE)) {
                return this.each(function (bF) {
                    var bG = Z(this);
                    bG[by](bE.call(this, bF, bG[by]()))
                })
            }
            if (Z.isWindow(bB)) {
                var bA = bB.document.documentElement["client" + bx], bz = bB.document.body;
                return bB.document.compatMode === "CSS1Compat" && bA || bz && bz["client" + bx] || bA
            }
            else {
                if (bB.nodeType === 9) {
                    return Math.max(bB.documentElement["client" + bx], bB.body["scroll" + bx], bB.documentElement["scroll" + bx], bB.body["offset" + bx], bB.documentElement["offset" + bx])
                }
                else {
                    if (bE === bq) {
                        var bC = Z.css(bB, by), bD = parseFloat(bC);
                        return Z.isNumeric(bD) ? bD : bC
                    }
                    else {
                        return this.css(by, typeof bE === "string" ? bE : bE + "px")
                    }

                }

            }

        }

    });
    br.jQuery = br.$ = Z
})(window);
(function (c, b) {
    var a = (function () {
        var d = function () { };
        d.JsonCommand = function () {
            this.Command = "";
            this.ListParameter = new Array();
            this.toJSONInstance = function () {
                return {
                    c: this.Command, p: this.ListParameter
                }

            };
            this.toJSONString = function () {
                return JSON.stringify(this.toJSONInstance())
            };
            this.parse = function (e) {
                return $.extend(this, {
                    Command: e.c, ListParameter: e.p, Remark: e.r
                })
            }

        };
        d.Date = new function () {
            var e = new Date();
            var f = new Date();
            this.setServerTime = function (g) {
                e = typeof (g) == "string" ? g.toDate() : g;
                f = new Date()
            };
            this.getServerTime = function () {
                var g = new Date();
                g.setTime(e.getTime() + new Date().getTime() - f.getTime());
                return g
            };
            this.update = function () {
                $.ajax({
                    type: "post", url: "/handler/timer.ashx", success: this.setServerTime.bind(this)
                })
            };
            this.getCompareDateDay = function (g, h, i) {
                g = typeof g == "string" ? g.toDate() : g;
                h = h || "日";
                i = i || "周";
                var k = {
                    days: 0, rencent: "", week: "", date: new Date().getDate()
                };
                var j = (g.toFormatString("{#yyyy#}-{#MM#}-{#dd#}").toDate() - this.getServerTime().toFormatString("{#yyyy#}-{#MM#}-{#dd#}").toDate()).toLeftTime();
                k.days = j.Day;
                switch (k.days) {
                    case 0: k.rencent = "今" + h;
                        break;
                    case 1: k.rencent = "明" + h;
                        break;
                    case -1: k.rencent = "昨" + h;
                        break;
                    default: k.rencent = g.toFormatString("{#MM#}月{#dd#}日");
                        break
                }
                k.rencent = k.rencent != "" ? k.rencent : k.rencent;
                k.week = i + g.getWeekDay();
                k.date = g.getDate();
                return k
            }

        };
        d.Enum = new function () {
            this.parse = function (f, e) {
                for (var g in e) {
                    if (e[g] == f) {
                        return g
                    }

                }

            }

        };
        d.dispose = function () {
            for (var e in arguments) {
                arguments[e] = null
            }

        };
        d.Math = new function () {
            this.c = function (k, j) {
                if (k < 0 || j <= 0 || j < k) {
                    return 0
                }
                if (k == 0) {
                    return 1
                }
                var h = 1, f = 1;
                for (var e = 0;
                e < k;
                e++) {
                    h *= j - e;
                    f *= e + 1
                }
                var g = h / f;
                return g;
                return Math.round(g)
            };
            this.listC = function (e, h, i) {
                var j = [];
                (function g(s, f, p, q) {
                    if (p == 0) {
                        return j.push(s)
                    }
                    for (var k = 0, o = f.length;
                    k <= o - p;
                    k++) {
                        if (f[k].constructor === s.constructor) {
                            if (q > 0) {
                                var r = $S.Math.listC(f[k], q);
                                for (var m = 0;
                                m < r.length;
                                m++) {
                                    g(s.concat(r[m]), f.slice(k + 1), p - 1, q)
                                }

                            }
                            else {
                                g(s.concat([f[k]]), f.slice(k + 1), p - 1)
                            }

                        }
                        else {
                            g(s.concat(f[k]), f.slice(k + 1), p - 1)
                        }

                    }

                })([], e, h, i);
                return j
            };
            this.RandomAry = function (e, f, g) {
                var h = e.concat([]);
                var k = [];
                while (k.length < f) {
                    var i = Math.floor(Math.random() * h.length);
                    var j = h[i];
                    k.push(j);
                    if (g != true) {
                        h.remove(j)
                    }

                }
                return k
            }

        }
        ();
        d.Query = new function () {
            this.get = function (e) {
                var g = new RegExp("(^|&)" + e + "=([^&]*)(&|$)", "i");
                var f = c.location.search.substr(1).match(g);
                if (f != null) {
                    return unescape(f[2])
                }
                return null
            }

        }
        ();
        d.Cookie = new function () {
            this.set = function (f, h, g) {
                g = g || 365 * 60 * 60;
                var e = new Date().addMinutes(g);
                document.cookie = f + "=" + escape(h) + ";path=/;expires=" + e.toUTCString()
            };
            this.get = function (e) {
                var g = new RegExp("(^|; )" + e + "=([^;]*)(; |$)", "i");
                var f = document.cookie.match(g);
                if (f != null) {
                    return decodeURIComponent(f[2])
                }
                return null
            };
            this.del = function (f) {
                var e = new Date().addMinutes(-1);
                if (this.get(f) != null) {
                    this.set(f, "", e)
                }

            }

        }
        ();
        d.Favorite = new function () {
            this.add = function (j, h) {
                j = j || location.href;
                h = h || document.title;
                var i = navigator.userAgent.toLowerCase();
                var e = i.indexOf("mac") != -1 ? "Command/Cmd" : "Ctrl";
                try {
                    if (i.indexOf("msie 8") > -1) {
                        c.external.addToFavoritesBar(j, h, "时时彩网")
                    }
                    else {
                        try {
                            c.external.addFavorite(j, h)
                        }
                        catch (g) {
                            c.sidebar.addPanel(h, j, "时时彩网")
                        }

                    }

                }
                catch (f) {
                    alert("您的浏览器不支持该操作!\n您可以尝试通过快捷键" + e + " + D 将本页面加入到收藏夹!")
                }

            }

        }
        ();
        d.Debug = new function () {
            this.Enable = d.Query.get("__debug") || false;
            this.Console = d.Query.get("__console") || false;
            this.Box = null;
            this.Button = null;
            this.showHide = function () {
                if (this.Box.style.display == "none") {
                    this.Box.style.display = "";
                    this.Button.value = "close"
                }
                else {
                    this.Box.style.display = "none";
                    this.Button.value = "show"
                }

            };
            function e(h, f, g) {
                return {
                    key: h + "【" + (typeof (g) == "string" ? g : g.toFormatString("{#HH#}:{#mm#}:{#ss#}")) + "】", value: f
                }

            }
            this.console = function (h, f, g) {
                g = g || e(h, f, new Date());
                c.console && console.log(g.key, g.value)
            };
            this.log = function (k, h, f, g, j) {
                var i = null;
                g = g || this.Console;
                j = j || this.Enable;
                j = location.hostname == "lc.shishicai.cn" ? true : j;
                if (j || g) {
                    if (this.Box == null) {
                        if (document.body) {
                            var l = " top:0px; z-index:99999;";
                            this.Button = document.createElement("input");
                            this.Button.style.cssText = l + "position:fixed; display:block; right:0px; float:right; z-index:999999;";
                            this.Button.type = "button";
                            this.Button.value = "close";
                            this.Button.onclick = this.showHide.bind(this);
                            document.body.appendChild(this.Button);
                            this.Box = document.createElement("div");
                            this.Box.style.cssText = l + "position:absolute; color:#fff; left:0px; background-color:#000; width:100%;";
                            document.body.appendChild(this.Box)
                        }
                        else {
                            var m = function () {
                                d.Debug.log(k, h, f, g, j)
                            };
                            if (c.attachEvent) {
                                c.attachEvent("onload", m)
                            }
                            else {
                                if (c.addEventListener) {
                                    c.addEventListener("load", m, false)
                                }

                            }
                            return
                        }

                    }
                    if (f || d.Query.get("__clear")) {
                        this.Box.innerHTML = ""
                    }
                    i = e(k, h, new Date());
                    if (j) {
                        this.Box.innerHTML = [this.Box.innerHTML, "<p>" + i.key + "&nbsp;&gt;&gt;&nbsp;" + i.value + "</p>"].join("");
                        if (!f && this.Box.offsetHeight > c.screen.availHeight) {
                            c.scrollTo(0, this.Box.offsetHeight)
                        }

                    }
                    if (g) {
                        this.console(k, h, i)
                    }

                }

            }

        };
        d.PlaceHolder = {
            _support: (function () {
                return "placeholder" in document.createElement("input")
            })(), className: "grey", _setValue: function (e) {
                if (!this._support) {
                    e.value = e.attributes.placeholder.nodeValue
                }
                e.className = this.className
            }
            , create: function (k) {
                var j, g = this;
                if (!k.length) {
                    k = [k]
                }
                function f(i) {
                    if (!this._support && this.value === this.getAttribute("placeholder")) {
                        this.value = ""
                    }
                    this.className = ""
                }
                function e(i) {
                    if (this.value === "") {
                        g._setValue(this)
                    }

                }
                for (var h = 0, l = k.length;
                h < l;
                h++) {
                    j = k[h];
                    if (j.getAttribute("placeholder")) {
                        this._setValue(j);
                        if (typeof (j.addEventListener) != "undefined") {
                            j.addEventListener("focus", f.bind(j), false);
                            j.addEventListener("blur", e.bind(j), false)
                        }
                        else {
                            j.attachEvent("onfocus", f.bind(j));
                            j.attachEvent("onblur", e.bind(j))
                        }

                    }

                }

            }
            , init: function () {
                var e = document.getElementsByTagName("input");
                var f = document.getElementsByTagName("textarea");
                this.create(e);
                this.create(f)
            }

        };
        d.Flash = new function () {
            this.create = function (e) {
                var f = $.extend({
                    Id: "", Name: "", Width: 0, Height: 0, Movie: "", Wmode: "transparent", AllowScriptAccess: "always", AllowFullScreen: false, Quality: "high", Bgcolor: "#ffffff"
                }
                , e);
                f.Name = f.Name == "" ? f.Id : f.Name;
                var g = new Array();
                g.push('<object id="{0}" width="{2}" height="{3}" name="{1}" type="application/x-shockwave-flash" data="{4}" wmode="{5}">');
                g.push('<param name="allowScriptAccess" value="{6}">');
                g.push('<param name="allowFullScreen" value="{7}">');
                g.push('<param name="movie" value="{4}">');
                g.push('<param name="wmode" value="{5}">');
                g.push('<param name="quality" value="{8}">');
                g.push('<param name="bgcolor" value="{9}">');
                g.push('<embed id="{0}" src="{4}" wmode="{5}" quality="{8}" bgcolor="{9}" width="{2}" height="{3}" name="{1}" align="middle" allowscriptaccess="{6}" allowfullscreen="{7}" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer">');
                g.push("</object>");
                return String.format(g.join(""), f.Id, f.Name, f.Width, f.Height, f.Movie, f.Wmode, f.AllowScriptAccess, f.AllowFullScreen, f.Quality, f.Bgcolor)
            }

        }
        ();
        return d
    })();
    c.System = c.$S = a
})(window);
(function (b, a) {
    if (typeof TK === "undefined") {
        TK = new (function () { })()
    }
    TK.Url = {
        site_image: "", site_css: "", site_js: "", pay: "", login_interface_alipay: ""
    }

})(window);
