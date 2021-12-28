/*
jQWidgets v3.6.0 (2014-Nov-25)
Copyright (c) 2011-2014 jQWidgets.
License: http://jqwidgets.com/license/
*/

var jqxBaseFramework = window.minQuery || window.jQuery;
(function(a) {
    a.jqx = a.jqx || {};
    a.jqx.define = function(b, c, d) {
        b[c] = function() {
            if (this.baseType) {
                this.base = new b[this.baseType]();
                this.base.defineInstance()
            }
            this.defineInstance()
        };
        b[c].prototype.defineInstance = function() {};
        b[c].prototype.base = null;
        b[c].prototype.baseType = undefined;
        if (d && b[d]) {
            b[c].prototype.baseType = d
        }
    };
    a.jqx.invoke = function(e, d) {
        if (d.length == 0) {
            return
        }
        var f = typeof(d) == Array || d.length > 0 ? d[0] : d;
        var c = typeof(d) == Array || d.length > 1 ? Array.prototype.slice.call(d, 1) : a({}).toArray();
        while (e[f] == undefined && e.base != null) {
            if (e[f] != undefined && a.isFunction(e[f])) {
                return e[f].apply(e, c)
            }
            if (typeof f == "string") {
                var b = f.toLowerCase();
                if (e[b] != undefined && a.isFunction(e[b])) {
                    return e[b].apply(e, c)
                }
            }
            e = e.base
        }
        if (e[f] != undefined && a.isFunction(e[f])) {
            return e[f].apply(e, c)
        }
        if (typeof f == "string") {
            var b = f.toLowerCase();
            if (e[b] != undefined && a.isFunction(e[b])) {
                return e[b].apply(e, c)
            }
        }
        return
    };
    a.jqx.hasProperty = function(c, b) {
        if (typeof(b) == "object") {
            for (var e in b) {
                var d = c;
                while (d) {
                    if (d.hasOwnProperty(e)) {
                        return true
                    }
                    if (d.hasOwnProperty(e.toLowerCase())) {
                        return true
                    }
                    d = d.base
                }
                return false
            }
        } else {
            while (c) {
                if (c.hasOwnProperty(b)) {
                    return true
                }
                if (c.hasOwnProperty(b.toLowerCase())) {
                    return true
                }
                c = c.base
            }
        }
        return false
    };
    a.jqx.hasFunction = function(e, d) {
        if (d.length == 0) {
            return false
        }
        if (e == undefined) {
            return false
        }
        var f = typeof(d) == Array || d.length > 0 ? d[0] : d;
        var c = typeof(d) == Array || d.length > 1 ? Array.prototype.slice.call(d, 1) : {};
        while (e[f] == undefined && e.base != null) {
            if (e[f] && a.isFunction(e[f])) {
                return true
            }
            if (typeof f == "string") {
                var b = f.toLowerCase();
                if (e[b] && a.isFunction(e[b])) {
                    return true
                }
            }
            e = e.base
        }
        if (e[f] && a.isFunction(e[f])) {
            return true
        }
        if (typeof f == "string") {
            var b = f.toLowerCase();
            if (e[b] && a.isFunction(e[b])) {
                return true
            }
        }
        return false
    };
    a.jqx.isPropertySetter = function(c, b) {
        if (b.length == 1 && typeof(b[0]) == "object") {
            return true
        }
        if (b.length == 2 && typeof(b[0]) == "string" && !a.jqx.hasFunction(c, b)) {
            return true
        }
        return false
    };
    a.jqx.validatePropertySetter = function(f, d, b) {
        if (!a.jqx.propertySetterValidation) {
            return true
        }
        if (d.length == 1 && typeof(d[0]) == "object") {
            for (var e in d[0]) {
                var g = f;
                while (!g.hasOwnProperty(e) && g.base) {
                    g = g.base
                }
                if (!g || !g.hasOwnProperty(e)) {
                    if (!b) {
                        var c = g.hasOwnProperty(e.toString().toLowerCase());
                        if (!c) {
                            throw "Invalid property: " + e
                        } else {
                            return true
                        }
                    }
                    return false
                }
            }
            return true
        }
        if (d.length != 2) {
            if (!b) {
                throw "Invalid property: " + d.length >= 0 ? d[0] : ""
            }
            return false
        }
        while (!f.hasOwnProperty(d[0]) && f.base) {
            f = f.base
        }
        if (!f || !f.hasOwnProperty(d[0])) {
            if (!b) {
                throw "Invalid property: " + d[0]
            }
            return false
        }
        return true
    };
    a.jqx.set = function(e, d) {
        if (d.length == 1 && typeof(d[0]) == "object") {
            if (e.isInitialized && Object.keys && Object.keys(d[0]).length > 1) {
                e.batchUpdate = d[0];
                var b = {};
                var c = {};
                a.each(d[0], function(f, g) {
                    var h = e;
                    while (!h.hasOwnProperty(f) && h.base != null) {
                        h = h.base
                    }
                    if (h.hasOwnProperty(f)) {
                        b[f] = e[f];
                        c[f] = g
                    } else {
                        if (h.hasOwnProperty(f.toLowerCase())) {
                            b[f.toLowerCase()] = e[f.toLowerCase()];
                            c[f.toLowerCase()] = g
                        }
                    }
                })
            }
            a.each(d[0], function(f, g) {
                var h = e;
                while (!h.hasOwnProperty(f) && h.base != null) {
                    h = h.base
                }
                if (h.hasOwnProperty(f)) {
                    a.jqx.setvalueraiseevent(h, f, g)
                } else {
                    if (h.hasOwnProperty(f.toLowerCase())) {
                        a.jqx.setvalueraiseevent(h, f.toLowerCase(), g)
                    } else {
                        if (a.jqx.propertySetterValidation) {
                            throw "jqxCore: invalid property '" + f + "'"
                        }
                    }
                }
            });
            if (e.batchUpdate != null) {
                e.batchUpdate = null;
                if (e.propertiesChangedHandler) {
                    e.propertiesChangedHandler(e, b, c)
                }
            }
        } else {
            if (d.length == 2) {
                while (!e.hasOwnProperty(d[0]) && e.base) {
                    e = e.base
                }
                if (e.hasOwnProperty(d[0])) {
                    a.jqx.setvalueraiseevent(e, d[0], d[1])
                } else {
                    if (e.hasOwnProperty(d[0].toLowerCase())) {
                        a.jqx.setvalueraiseevent(e, d[0].toLowerCase(), d[1])
                    } else {
                        if (a.jqx.propertySetterValidation) {
                            throw "jqxCore: invalid property '" + d[0] + "'"
                        }
                    }
                }
            }
        }
    };
    a.jqx.setvalueraiseevent = function(c, d, e) {
        var b = c[d];
        c[d] = e;
        if (!c.isInitialized) {
            return
        }
        if (c.propertyChangedHandler != undefined) {
            c.propertyChangedHandler(c, d, b, e)
        }
        if (c.propertyChangeMap != undefined && c.propertyChangeMap[d] != undefined) {
            c.propertyChangeMap[d](c, d, b, e)
        }
    };
    a.jqx.get = function(e, d) {
        if (d == undefined || d == null) {
            return undefined
        }
        if (e.propertyMap) {
            var c = e.propertyMap(d);
            if (c != null) {
                return c
            }
        }
        if (e.hasOwnProperty(d)) {
            return e[d]
        }
        if (e.hasOwnProperty(d.toLowerCase())) {
            return e[d.toLowerCase()]
        }
        var b = undefined;
        if (typeof(d) == Array) {
            if (d.length != 1) {
                return undefined
            }
            b = d[0]
        } else {
            if (typeof(d) == "string") {
                b = d
            }
        }
        while (!e.hasOwnProperty(b) && e.base) {
            e = e.base
        }
        if (e) {
            return e[b]
        }
        return undefined
    };
    a.jqx.serialize = function(e) {
        var b = "";
        if (a.isArray(e)) {
            b = "[";
            for (var d = 0; d < e.length; d++) {
                if (d > 0) {
                    b += ", "
                }
                b += a.jqx.serialize(e[d])
            }
            b += "]"
        } else {
            if (typeof(e) == "object") {
                b = "{";
                var c = 0;
                for (var d in e) {
                    if (c++ > 0) {
                        b += ", "
                    }
                    b += d + ": " + a.jqx.serialize(e[d])
                }
                b += "}"
            } else {
                b = e.toString()
            }
        }
        return b
    };
    a.jqx.propertySetterValidation = true;
    a.jqx.jqxWidgetProxy = function(g, c, b) {
        var d = a(c);
        var f = a.data(c, g);
        if (f == undefined) {
            return undefined
        }
        var e = f.instance;
        if (a.jqx.hasFunction(e, b)) {
            return a.jqx.invoke(e, b)
        }
        if (a.jqx.isPropertySetter(e, b)) {
            if (a.jqx.validatePropertySetter(e, b)) {
                a.jqx.set(e, b);
                return undefined
            }
        } else {
            if (typeof(b) == "object" && b.length == 0) {
                return
            } else {
                if (typeof(b) == "object" && b.length == 1 && a.jqx.hasProperty(e, b[0])) {
                    return a.jqx.get(e, b[0])
                } else {
                    if (typeof(b) == "string" && a.jqx.hasProperty(e, b[0])) {
                        return a.jqx.get(e, b)
                    }
                }
            }
        }
        throw "jqxCore: Invalid parameter '" + a.jqx.serialize(b) + "' does not exist.";
        return undefined
    };
    a.jqx.applyWidget = function(c, d, k, l) {
        var g = false;
        try {
            g = window.MSApp != undefined
        } catch (f) {}
        var m = a(c);
        if (!l) {
            l = new a.jqx["_" + d]()
        } else {
            l.host = m;
            l.element = c
        }
        if (c.id == "") {
            c.id = a.jqx.utilities.createId()
        }
        var j = {
            host: m,
            element: c,
            instance: l
        };
        l.widgetName = d;
        a.data(c, d, j);
        a.data(c, "jqxWidget", j.instance);
        var h = new Array();
        var l = j.instance;
        while (l) {
            l.isInitialized = false;
            h.push(l);
            l = l.base
        }
        h.reverse();
        h[0].theme = a.jqx.theme || "";
        a.jqx.jqxWidgetProxy(d, c, k);
        for (var b in h) {
            l = h[b];
            if (b == 0) {
                l.host = m;
                l.element = c;
                l.WinJS = g
            }
            if (l != undefined) {
                if (l.definedInstance) {
                    l.definedInstance()
                }
                if (l.createInstance != null) {
                    if (g) {
                        MSApp.execUnsafeLocalFunction(function() {
                            l.createInstance(k)
                        })
                    } else {
                        l.createInstance(k)
                    }
                }
            }
        }
        for (var b in h) {
            if (h[b] != undefined) {
                h[b].isInitialized = true
            }
        }
        if (g) {
            MSApp.execUnsafeLocalFunction(function() {
                j.instance.refresh(true)
            })
        } else {
            j.instance.refresh(true)
        }
    };
    a.jqx.jqxWidget = function(b, c, f) {
        var j = false;
        try {
            jqxArgs = Array.prototype.slice.call(f, 0)
        } catch (h) {
            jqxArgs = ""
        }
        try {
            j = window.MSApp != undefined
        } catch (h) {}
        var g = b;
        var l = "";
        if (c) {
            l = "_" + c
        }
        a.jqx.define(a.jqx, "_" + g, l);
        var k = new Array();
        if (!window[g]) {
            var d = function(m) {
                if (m == null) {
                    return ""
                }
                var e = a.type(m);
                switch (e) {
                    case "string":
                    case "number":
                    case "date":
                    case "boolean":
                    case "bool":
                        if (m === null) {
                            return ""
                        }
                        return m.toString()
                }
                var n = "";
                a.each(m, function(p) {
                    var r = this;
                    if (p > 0) {
                        n += ", "
                    }
                    n += "[";
                    var o = 0;
                    if (a.type(r) == "object") {
                        for (var q in r) {
                            if (o > 0) {
                                n += ", "
                            }
                            n += "{" + q + ":" + r[q] + "}";
                            o++
                        }
                    } else {
                        if (o > 0) {
                            n += ", "
                        }
                        n += "{" + p + ":" + r + "}";
                        o++
                    }
                    n += "]"
                });
                return n
            };
            window[g] = function(e, q) {
                var m = [];
                if (!q) {
                    q = {}
                }
                m.push(q);
                var n = e;
                if (a.type(n) === "object" && e[0]) {
                    n = e[0].id;
                    if (n === "") {
                        n = e[0].id = a.jqx.utilities.createId()
                    }
                }
                if (window.jqxWidgets && window.jqxWidgets[n]) {
                    if (q) {
                        a.each(window.jqxWidgets[n], function(r) {
                            a(this.element)[g](q)
                        })
                    }
                    if (window.jqxWidgets[n].length == 1) {
                        return window.jqxWidgets[n][0]
                    }
                    return window.jqxWidgets[n]
                }
                var o = a(e);
                if (o.length === 0) {
                    throw new Error("Invalid Selector - " + e + "! Please, check whether the used ID or CSS Class name is correct.")
                }
                var p = [];
                a.each(o, function(u) {
                    var w = o[u];
                    var t = null;
                    if (!k[g]) {
                        var x = w.id;
                        w.id = "";
                        t = a(w).clone();
                        w.id = x
                    }
                    a.jqx.applyWidget(w, g, m, undefined);
                    if (!k[g]) {
                        var s = a.data(w, "jqxWidget");
                        var v = t[g]().data().jqxWidget.defineInstance();
                        var r = function(z) {
                            var y = a.data(z, "jqxWidget");
                            this.widgetInstance = y;
                            var A = a.extend(this, y);
                            A.on = function(B, C) {
                                A.addHandler(A.host, B, C)
                            };
                            A.off = function(B) {
                                A.removeHandler(A.host, B)
                            };
                            return A
                        };
                        k[g] = r;
                        a.each(v, function(z, y) {
                            Object.defineProperty(r.prototype, z, {
                                get: function() {
                                    if (this.widgetInstance) {
                                        return this.widgetInstance[z]
                                    }
                                    return y
                                },
                                set: function(B) {
                                    if (this.widgetInstance && this.widgetInstance[z] != B) {
                                        if (this.widgetInstance[z] != B && d(this.widgetInstance[z]) != d(B)) {
                                            var A = {};
                                            A[z] = B;
                                            this.widgetInstance.host[g](A);
                                            this.widgetInstance[z] = B
                                        }
                                    }
                                }
                            })
                        })
                    }
                    var s = new k[g](w);
                    p.push(s);
                    if (!window.jqxWidgets) {
                        window.jqxWidgets = new Array()
                    }
                    if (!window.jqxWidgets[n]) {
                        window.jqxWidgets[n] = new Array()
                    }
                    window.jqxWidgets[n].push(s)
                });
                if (p.length === 1) {
                    return p[0]
                }
                return p
            }
        }
        a.fn[g] = function() {
            var e = Array.prototype.slice.call(arguments, 0);
            if (e.length == 0 || (e.length == 1 && typeof(e[0]) == "object")) {
                if (this.length == 0) {
                    if (this.selector) {
                        throw new Error("Invalid Selector - " + this.selector + "! Please, check whether the used ID or CSS Class name is correct.")
                    } else {
                        throw new Error("Invalid Selector! Please, check whether the used ID or CSS Class name is correct.")
                    }
                }
                return this.each(function() {
                    var p = a(this);
                    var o = this;
                    var q = a.data(o, g);
                    if (q == null) {
                        a.jqx.applyWidget(o, g, e, undefined)
                    } else {
                        a.jqx.jqxWidgetProxy(g, this, e)
                    }
                })
            } else {
                if (this.length == 0) {
                    if (this.selector) {
                        throw new Error("Invalid Selector - " + this.selector + "! Please, check whether the used ID or CSS Class name is correct.")
                    } else {
                        throw new Error("Invalid Selector! Please, check whether the used ID or CSS Class name is correct.")
                    }
                }
                var n = null;
                var m = 0;
                this.each(function() {
                    var o = a.jqx.jqxWidgetProxy(g, this, e);
                    if (m == 0) {
                        n = o;
                        m++
                    } else {
                        if (m == 1) {
                            var p = [];
                            p.push(n);
                            n = p
                        }
                        n.push(o)
                    }
                })
            }
            return n
        };
        try {
            a.extend(a.jqx["_" + g].prototype, Array.prototype.slice.call(f, 0)[0])
        } catch (h) {}
        a.extend(a.jqx["_" + g].prototype, {
            toThemeProperty: function(e, m) {
                return a.jqx.toThemeProperty(this, e, m)
            }
        });
        a.jqx["_" + g].prototype.refresh = function() {
            if (this.base) {
                this.base.refresh(true)
            }
        };
        a.jqx["_" + g].prototype.createInstance = function() {};
        a.jqx["_" + g].prototype.applyTo = function(n, m) {
            if (!(m instanceof Array)) {
                var e = [];
                e.push(m);
                m = e
            }
            a.jqx.applyWidget(n, g, m, this)
        };
        a.jqx["_" + g].prototype.getInstance = function() {
            return this
        };
        a.jqx["_" + g].prototype.propertyChangeMap = {};
        a.jqx["_" + g].prototype.addHandler = function(o, e, m, n) {
            a.jqx.addHandler(o, e, m, n)
        };
        a.jqx["_" + g].prototype.removeHandler = function(n, e, m) {
            a.jqx.removeHandler(n, e, m)
        }
    };
    a.jqx.toThemeProperty = function(c, d, h) {
        if (c.theme == "") {
            return d
        }
        var g = d.split(" ");
        var b = "";
        for (var f = 0; f < g.length; f++) {
            if (f > 0) {
                b += " "
            }
            var e = g[f];
            if (h != null && h) {
                b += e + "-" + c.theme
            } else {
                b += e + " " + e + "-" + c.theme
            }
        }
        return b
    };
    a.jqx.addHandler = function(g, h, e, f) {
        var c = h.split(" ");
        for (var b = 0; b < c.length; b++) {
            var d = c[b];
            if (window.addEventListener) {
                switch (d) {
                    case "mousewheel":
                        if (a.jqx.browser.mozilla) {
                            g[0].addEventListener("DOMMouseScroll", e, false)
                        } else {
                            g[0].addEventListener("mousewheel", e, false)
                        }
                        continue;
                    case "mousemove":
                        if (!f) {
                            g[0].addEventListener("mousemove", e, false);
                            continue
                        }
                        break
                }
            }
            if (f == undefined || f == null) {
                if (g.on) {
                    g.on(d, e)
                } else {
                    g.bind(d, e)
                }
            } else {
                if (g.on) {
                    g.on(d, f, e)
                } else {
                    g.bind(d, f, e)
                }
            }
        }
    };
    a.jqx.removeHandler = function(f, g, e) {
        if (!g) {
            return
        }
        var c = g.split(" ");
        for (var b = 0; b < c.length; b++) {
            var d = c[b];
            if (window.removeEventListener) {
                switch (d) {
                    case "mousewheel":
                        if (a.jqx.browser.mozilla) {
                            f[0].removeEventListener("DOMMouseScroll", e, false)
                        } else {
                            f[0].removeEventListener("mousewheel", e, false)
                        }
                        continue;
                    case "mousemove":
                        if (e) {
                            f[0].removeEventListener("mousemove", e, false);
                            continue
                        }
                        break
                }
            }
            if (d == undefined) {
                if (f.off) {
                    f.off()
                } else {
                    f.unbind()
                }
                continue
            }
            if (e == undefined) {
                if (f.off) {
                    f.off(d)
                } else {
                    f.unbind(d)
                }
            } else {
                if (f.off) {
                    f.off(d, e)
                } else {
                    f.unbind(d, e)
                }
            }
        }
    };
    a.jqx.theme = a.jqx.theme || "";
    a.jqx.resizeDelay = a.jqx.resizeDelay || 10;
    a.jqx.ready = function() {
        a(window).trigger("jqxReady")
    };
    a.jqx.init = function() {
        a.each(arguments[0], function(b, c) {
            if (b == "theme") {
                a.jqx.theme = c
            }
            if (b == "scrollBarSize") {
                a.jqx.utilities.scrollBarSize = c
            }
            if (b == "touchScrollBarSize") {
                a.jqx.utilities.touchScrollBarSize = c
            }
            if (b == "scrollBarButtonsVisibility") {
                a.jqx.utilities.scrollBarButtonsVisibility = c
            }
        })
    };
    a.jqx.utilities = a.jqx.utilities || {};
    a.extend(a.jqx.utilities, {
        scrollBarSize: 15,
        touchScrollBarSize: 10,
        scrollBarButtonsVisibility: "visible",
        createId: function() {
            var b = function() {
                return (((1 + Math.random()) * 65536) | 0).toString(16).substring(1)
            };
            return "jqxWidget" + b() + b()
        },
        setTheme: function(f, g, e) {
            if (typeof e === "undefined") {
                return
            }
            var h = e[0].className.split(" "),
                b = [],
                j = [],
                d = e.children();
            for (var c = 0; c < h.length; c += 1) {
                if (h[c].indexOf(f) >= 0) {
                    if (f.length > 0) {
                        b.push(h[c]);
                        j.push(h[c].replace(f, g))
                    } else {
                        j.push(h[c].replace("-" + g, "") + "-" + g)
                    }
                }
            }
            this._removeOldClasses(b, e);
            this._addNewClasses(j, e);
            for (var c = 0; c < d.length; c += 1) {
                this.setTheme(f, g, a(d[c]))
            }
        },
        _removeOldClasses: function(d, c) {
            for (var b = 0; b < d.length; b += 1) {
                c.removeClass(d[b])
            }
        },
        _addNewClasses: function(d, c) {
            for (var b = 0; b < d.length; b += 1) {
                c.addClass(d[b])
            }
        },
        getOffset: function(b) {
            var d = a.jqx.mobile.getLeftPos(b[0]);
            var c = a.jqx.mobile.getTopPos(b[0]);
            return {
                top: c,
                left: d
            }
        },
        resize: function(d, m, l, k) {
            if (k === undefined) {
                k = true
            }
            var g = -1;
            var f = this;
            var c = function(o) {
                if (!f.hiddenWidgets) {
                    return -1
                }
                var p = -1;
                for (var n = 0; n < f.hiddenWidgets.length; n++) {
                    if (o.id) {
                        if (f.hiddenWidgets[n].id == o.id) {
                            p = n;
                            break
                        }
                    } else {
                        if (f.hiddenWidgets[n].id == o[0].id) {
                            p = n;
                            break
                        }
                    }
                }
                return p
            };
            if (this.resizeHandlers) {
                for (var e = 0; e < this.resizeHandlers.length; e++) {
                    if (d.id) {
                        if (this.resizeHandlers[e].id == d.id) {
                            g = e;
                            break
                        }
                    } else {
                        if (this.resizeHandlers[e].id == d[0].id) {
                            g = e;
                            break
                        }
                    }
                }
                if (l === true) {
                    if (g != -1) {
                        this.resizeHandlers.splice(g, 1)
                    }
                    if (this.resizeHandlers.length == 0) {
                        var j = a(window);
                        if (j.off) {
                            j.off("resize.jqx");
                            j.off("orientationchange.jqx");
                            j.off("orientationchanged.jqx")
                        } else {
                            j.unbind("resize.jqx");
                            j.unbind("orientationchange.jqx");
                            j.unbind("orientationchanged.jqx")
                        }
                        this.resizeHandlers = null
                    }
                    var b = c(d);
                    if (b != -1 && this.hiddenWidgets) {
                        this.hiddenWidgets.splice(b, 1)
                    }
                    return
                }
            } else {
                if (l === true) {
                    var b = c(d);
                    if (b != -1 && this.hiddenWidgets) {
                        this.hiddenWidgets.splice(b, 1)
                    }
                    return
                }
            }
            var f = this;
            var h = function(p, x) {
                if (!f.resizeHandlers) {
                    return
                }
                var y = function(C) {
                    var B = -1;
                    var D = C.parentNode;
                    while (D) {
                        B++;
                        D = D.parentNode
                    }
                    return B
                };
                var o = function(E, C) {
                    if (!E.widget || !C.widget) {
                        return 0
                    }
                    var D = y(E.widget[0]);
                    var B = y(C.widget[0]);
                    try {
                        if (D < B) {
                            return -1
                        }
                        if (D > B) {
                            return 1
                        }
                    } catch (F) {
                        var G = F
                    }
                    return 0
                };
                var q = function(C) {
                    if (f.hiddenWidgets.length > 0) {
                        f.hiddenWidgets.sort(o);
                        var B = function() {
                            var E = false;
                            var G = new Array();
                            for (var F = 0; F < f.hiddenWidgets.length; F++) {
                                var D = f.hiddenWidgets[F];
                                if (a.jqx.isHidden(D.widget)) {
                                    E = true;
                                    G.push(D)
                                } else {
                                    if (D.callback) {
                                        D.callback(x)
                                    }
                                }
                            }
                            f.hiddenWidgets = G;
                            if (!E) {
                                clearInterval(f.__resizeInterval)
                            }
                        };
                        if (C == false) {
                            B();
                            if (f.__resizeInterval) {
                                clearInterval(f.__resizeInterval)
                            }
                            return
                        }
                        if (f.__resizeInterval) {
                            clearInterval(f.__resizeInterval)
                        }
                        f.__resizeInterval = setInterval(function() {
                            B()
                        }, 100)
                    }
                };
                if (f.hiddenWidgets && f.hiddenWidgets.length > 0) {
                    q(false)
                }
                f.hiddenWidgets = new Array();
                f.resizeHandlers.sort(o);
                for (var u = 0; u < f.resizeHandlers.length; u++) {
                    var A = f.resizeHandlers[u];
                    var w = A.widget;
                    var t = A.data;
                    if (!t) {
                        continue
                    }
                    if (!t.jqxWidget) {
                        continue
                    }
                    var n = t.jqxWidget.width;
                    var z = t.jqxWidget.height;
                    if (t.jqxWidget.base) {
                        if (n == undefined) {
                            n = t.jqxWidget.base.width
                        }
                        if (z == undefined) {
                            z = t.jqxWidget.base.height
                        }
                    }
                    if (n === undefined && z === undefined) {
                        n = t.jqxWidget.element.style.width;
                        z = t.jqxWidget.element.style.height
                    }
                    var v = false;
                    if (n != null && n.toString().indexOf("%") != -1) {
                        v = true
                    }
                    if (z != null && z.toString().indexOf("%") != -1) {
                        v = true
                    }
                    if (a.jqx.isHidden(w)) {
                        if (c(w) === -1) {
                            if (v || p === true) {
                                if (A.data.nestedWidget !== true) {
                                    f.hiddenWidgets.push(A)
                                }
                            }
                        }
                    } else {
                        if (p === undefined || p !== true) {
                            if (v) {
                                A.callback(x);
                                if (f.hiddenWidgets.indexOf(A) >= 0) {
                                    f.hiddenWidgets.splice(f.hiddenWidgets.indexOf(A), 1)
                                }
                            }
                            if (t.jqxWidget.element) {
                                var r = t.jqxWidget.element.className;
                                if (r.indexOf("dropdownlist") >= 0 || r.indexOf("datetimeinput") >= 0 || r.indexOf("combobox") >= 0 || r.indexOf("menu") >= 0) {
                                    if (t.jqxWidget.isOpened) {
                                        var s = t.jqxWidget.isOpened();
                                        if (s) {
                                            t.jqxWidget.close()
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                q()
            };
            if (!this.resizeHandlers) {
                this.resizeHandlers = new Array();
                var j = a(window);
                if (j.on) {
                    this._resizeTimer = null;
                    j.on("resize.jqx", function(n) {
                        if (f._resizeTimer != undefined) {
                            clearTimeout(f._resizeTimer)
                        }
                        f._resizeTimer = setTimeout(function() {
                            h(null, "resize")
                        }, a.jqx.resizeDelay)
                    });
                    j.on("orientationchange.jqx", function(n) {
                        h(null, "orientationchange")
                    });
                    j.on("orientationchanged.jqx", function(n) {
                        h(null, "orientationchange")
                    })
                } else {
                    j.bind("resize.jqx", function(n) {
                        h(null, "orientationchange")
                    });
                    j.bind("orientationchange.jqx", function(n) {
                        h(null, "orientationchange")
                    });
                    j.bind("orientationchanged.jqx", function(n) {
                        h(null, "orientationchange")
                    })
                }
            }
            if (k) {
                if (g === -1) {
                    this.resizeHandlers.push({
                        id: d[0].id,
                        widget: d,
                        callback: m,
                        data: d.data()
                    })
                }
            }
            if (a.jqx.isHidden(d) && k === true) {
                h(true)
            }
            a.jqx.resize = function() {
                h(null, "resize")
            }
        },
        html: function(c, d) {
            if (!a(c).on) {
                return a(c).html(d)
            }
            try {
                return a.access(c, function(s) {
                    var f = c[0] || {},
                        m = 0,
                        j = c.length;
                    if (s === undefined) {
                        return f.nodeType === 1 ? f.innerHTML.replace(rinlinejQuery, "") : undefined
                    }
                    var r = /<(?:script|style|link)/i,
                        n = "abbr|article|aside|audio|bdi|canvas|data|datalist|details|figcaption|figure|footer|header|hgroup|mark|meter|nav|output|progress|section|summary|time|video",
                        h = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([\w:]+)[^>]*)\/>/gi,
                        p = /<([\w:]+)/,
                        g = /<(?:script|object|embed|option|style)/i,
                        k = new RegExp("<(?:" + n + ")[\\s/>]", "i"),
                        q = /^\s+/,
                        t = {
                            option: [1, "<select multiple='multiple'>", "</select>"],
                            legend: [1, "<fieldset>", "</fieldset>"],
                            thead: [1, "<table>", "</table>"],
                            tr: [2, "<table><tbody>", "</tbody></table>"],
                            td: [3, "<table><tbody><tr>", "</tr></tbody></table>"],
                            col: [2, "<table><tbody></tbody><colgroup>", "</colgroup></table>"],
                            area: [1, "<map>", "</map>"],
                            _default: [0, "", ""]
                        };
                    if (typeof s === "string" && !r.test(s) && (a.support.htmlSerialize || !k.test(s)) && (a.support.leadingWhitespace || !q.test(s)) && !t[(p.exec(s) || ["", ""])[1].toLowerCase()]) {
                        s = s.replace(h, "<$1></$2>");
                        try {
                            for (; m < j; m++) {
                                f = this[m] || {};
                                if (f.nodeType === 1) {
                                    a.cleanData(f.getElementsByTagName("*"));
                                    f.innerHTML = s
                                }
                            }
                            f = 0
                        } catch (o) {}
                    }
                    if (f) {
                        c.empty().append(s)
                    }
                }, null, d, arguments.length)
            } catch (b) {
                return a(c).html(d)
            }
        },
        hasTransform: function(d) {
            var c = "";
            c = d.css("transform");
            if (c == "" || c == "none") {
                c = d.parents().css("transform");
                if (c == "" || c == "none") {
                    var b = a.jqx.utilities.getBrowser();
                    if (b.browser == "msie") {
                        c = d.css("-ms-transform");
                        if (c == "" || c == "none") {
                            c = d.parents().css("-ms-transform")
                        }
                    } else {
                        if (b.browser == "chrome") {
                            c = d.css("-webkit-transform");
                            if (c == "" || c == "none") {
                                c = d.parents().css("-webkit-transform")
                            }
                        } else {
                            if (b.browser == "opera") {
                                c = d.css("-o-transform");
                                if (c == "" || c == "none") {
                                    c = d.parents().css("-o-transform")
                                }
                            } else {
                                if (b.browser == "mozilla") {
                                    c = d.css("-moz-transform");
                                    if (c == "" || c == "none") {
                                        c = d.parents().css("-moz-transform")
                                    }
                                }
                            }
                        }
                    }
                } else {
                    return c != "" && c != "none"
                }
            }
            if (c == "" || c == "none") {
                c = a(document.body).css("transform")
            }
            return c != "" && c != "none" && c != null
        },
        getBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = /(chrome)[ \/]([\w.]+)/.exec(c) || /(webkit)[ \/]([\w.]+)/.exec(c) || /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(c) || /(msie) ([\w.]+)/.exec(c) || c.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+)|)/.exec(c) || [];
            var d = {
                browser: b[1] || "",
                version: b[2] || "0"
            };
            if (c.indexOf("rv:11.0") >= 0 && c.indexOf(".net4.0c") >= 0) {
                d.browser = "msie";
                d.version = "11";
                b[1] = "msie"
            }
            d[b[1]] = b[1];
            return d
        }
    });
    a.jqx.browser = a.jqx.utilities.getBrowser();
    a.jqx.isHidden = function(d) {
        try {
            var b = d[0].offsetWidth,
                e = d[0].offsetHeight;
            if (b === 0 || e === 0) {
                return true
            } else {
                return false
            }
        } catch (c) {
            return false
        }
    };
    a.jqx.ariaEnabled = true;
    a.jqx.aria = function(c, e, d) {
        if (!a.jqx.ariaEnabled) {
            return
        }
        if (e == undefined) {
            a.each(c.aria, function(g, h) {
                var k = !c.base ? c.host.attr(g) : c.base.host.attr(g);
                if (k != undefined && !a.isFunction(k)) {
                    var j = k;
                    switch (h.type) {
                        case "number":
                            j = new Number(k);
                            if (isNaN(j)) {
                                j = k
                            }
                            break;
                        case "boolean":
                            j = k == "true" ? true : false;
                            break;
                        case "date":
                            j = new Date(k);
                            if (j == "Invalid Date" || isNaN(j)) {
                                j = k
                            }
                            break
                    }
                    c[h.name] = j
                } else {
                    var k = c[h.name];
                    if (a.isFunction(k)) {
                        k = c[h.name]()
                    }
                    if (k == undefined) {
                        k = ""
                    }
                    try {
                        !c.base ? c.host.attr(g, k.toString()) : c.base.host.attr(g, k.toString())
                    } catch (f) {}
                }
            })
        } else {
            try {
                if (c.host) {
                    if (!c.base) {
                        if (c.host) {
                            if (c.element.setAttribute) {
                                c.element.setAttribute(e, d.toString())
                            } else {
                                c.host.attr(e, d.toString())
                            }
                        } else {
                            c.attr(e, d.toString())
                        }
                    } else {
                        if (c.base.host) {
                            c.base.host.attr(e, d.toString())
                        } else {
                            c.attr(e, d.toString())
                        }
                    }
                } else {
                    if (c.setAttribute) {
                        c.setAttribute(e, d.toString())
                    }
                }
            } catch (b) {}
        }
    };
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function(c) {
            var b = this.length;
            var d = Number(arguments[1]) || 0;
            d = (d < 0) ? Math.ceil(d) : Math.floor(d);
            if (d < 0) {
                d += b
            }
            for (; d < b; d++) {
                if (d in this && this[d] === c) {
                    return d
                }
            }
            return -1
        }
    }
    a.jqx.mobile = a.jqx.mobile || {};
    a.jqx.position = function(b) {
        var e = parseInt(b.pageX);
        var d = parseInt(b.pageY);
        if (a.jqx.mobile.isTouchDevice()) {
            var c = a.jqx.mobile.getTouches(b);
            var f = c[0];
            e = parseInt(f.pageX);
            d = parseInt(f.pageY)
        }
        return {
            left: e,
            top: d
        }
    };
    a.extend(a.jqx.mobile, {
        _touchListener: function(h, f) {
            var b = function(j, l) {
                var k = document.createEvent("MouseEvents");
                k.initMouseEvent(j, l.bubbles, l.cancelable, l.view, l.detail, l.screenX, l.screenY, l.clientX, l.clientY, l.ctrlKey, l.altKey, l.shiftKey, l.metaKey, l.button, l.relatedTarget);
                k._pageX = l.pageX;
                k._pageY = l.pageY;
                return k
            };
            var g = {
                mousedown: "touchstart",
                mouseup: "touchend",
                mousemove: "touchmove"
            };
            var d = b(g[h.type], h);
            h.target.dispatchEvent(d);
            var c = h.target["on" + g[h.type]];
            if (typeof c === "function") {
                c(h)
            }
        },
        setMobileSimulator: function(c, e) {
            if (this.isTouchDevice()) {
                return
            }
            this.simulatetouches = true;
            if (e == false) {
                this.simulatetouches = false
            }
            var d = {
                mousedown: "touchstart",
                mouseup: "touchend",
                mousemove: "touchmove"
            };
            var b = this;
            if (window.addEventListener) {
                var f = function() {
                    for (var g in d) {
                        if (c.addEventListener) {
                            c.removeEventListener(g, b._touchListener);
                            c.addEventListener(g, b._touchListener, false)
                        }
                    }
                };
                if (a.jqx.browser.msie) {
                    f()
                } else {
                    f()
                }
            }
        },
        isTouchDevice: function() {
            if (this.touchDevice != undefined) {
                return this.touchDevice
            }
            var c = "Browser CodeName: " + navigator.appCodeName + "";
            c += "Browser Name: " + navigator.appName + "";
            c += "Browser Version: " + navigator.appVersion + "";
            c += "Platform: " + navigator.platform + "";
            c += "User-agent header: " + navigator.userAgent + "";
            if (c.indexOf("Android") != -1) {
                return true
            }
            if (c.indexOf("IEMobile") != -1) {
                return true
            }
            if (c.indexOf("Windows Phone") != -1) {
                return true
            }
            if (c.indexOf("WPDesktop") != -1) {
                return true
            }
            if (c.indexOf("ZuneWP7") != -1) {
                return true
            }
            if (c.indexOf("BlackBerry") != -1 && c.indexOf("Mobile Safari") != -1) {
                return true
            }
            if (c.indexOf("ipod") != -1) {
                return true
            }
            if (c.indexOf("nokia") != -1 || c.indexOf("Nokia") != -1) {
                return true
            }
            if (c.indexOf("Chrome/17") != -1) {
                return false
            }
            if (c.indexOf("CrOS") != -1) {
                return false
            }
            if (c.indexOf("Opera") != -1 && c.indexOf("Mobi") == -1 && c.indexOf("Mini") == -1 && c.indexOf("Platform: Win") != -1) {
                return false
            }
            if (c.indexOf("Opera") != -1 && c.indexOf("Mobi") != -1 && c.indexOf("Opera Mobi") != -1) {
                return true
            }
            var d = {
                ios: "i(?:Pad|Phone|Pod)(?:.*)CPU(?: iPhone)? OS ",
                android: "(Android |HTC_|Silk/)",
                blackberry: "BlackBerry(?:.*)Version/",
                rimTablet: "RIM Tablet OS ",
                webos: "(?:webOS|hpwOS)/",
                bada: "Bada/"
            };
            try {
                if (this.touchDevice != undefined) {
                    return this.touchDevice
                }
                this.touchDevice = false;
                for (i in d) {
                    if (d.hasOwnProperty(i)) {
                        prefix = d[i];
                        match = c.match(new RegExp("(?:" + prefix + ")([^\\s;]+)"));
                        if (match) {
                            if (i.toString() == "blackberry") {
                                this.touchDevice = false;
                                return false
                            }
                            this.touchDevice = true;
                            return true
                        }
                    }
                }
                var f = navigator.userAgent;
                if (navigator.platform.toLowerCase().indexOf("win") != -1) {
                    if (f.indexOf("Windows Phone") >= 0 || f.indexOf("WPDesktop") >= 0 || f.indexOf("IEMobile") >= 0 || f.indexOf("ZuneWP7") >= 0) {
                        this.touchDevice = true;
                        return true
                    } else {
                        if (f.indexOf("Touch") >= 0) {
                            var b = ("MSPointerDown" in window) || ("pointerdown" in window);
                            if (b) {
                                this.touchDevice = true;
                                return true
                            }
                            if (f.indexOf("ARM") >= 0) {
                                this.touchDevice = true;
                                return true
                            }
                            this.touchDevice = false;
                            return false
                        }
                    }
                }
                if (navigator.platform.toLowerCase().indexOf("win") != -1) {
                    this.touchDevice = false;
                    return false
                }
                if (("ontouchstart" in window) || window.DocumentTouch && document instanceof DocumentTouch) {
                    this.touchDevice = true
                }
                return this.touchDevice
            } catch (g) {
                this.touchDevice = false;
                return false
            }
        },
        getLeftPos: function(b) {
            var c = b.offsetLeft;
            while ((b = b.offsetParent) != null) {
                if (b.tagName != "HTML") {
                    c += b.offsetLeft;
                    if (document.all) {
                        c += b.clientLeft
                    }
                }
            }
            return c
        },
        getTopPos: function(c) {
            var e = c.offsetTop;
            var b = a(c).coord();
            while ((c = c.offsetParent) != null) {
                if (c.tagName != "HTML") {
                    e += (c.offsetTop - c.scrollTop);
                    if (document.all) {
                        e += c.clientTop
                    }
                }
            }
            var d = navigator.userAgent.toLowerCase();
            var f = (d.indexOf("windows phone") != -1 || d.indexOf("WPDesktop") != -1 || d.indexOf("ZuneWP7") != -1 || d.indexOf("msie 9") != -1 || d.indexOf("msie 11") != -1 || d.indexOf("msie 10") != -1) && d.indexOf("touch") != -1;
            if (f) {
                return b.top
            }
            if (this.isSafariMobileBrowser()) {
                if (this.isSafari4MobileBrowser() && this.isIPadSafariMobileBrowser()) {
                    return e
                }
                if (d.indexOf("version/7") != -1) {
                    return b.top
                }
                e = e + a(window).scrollTop()
            }
            return e
        },
        isChromeMobileBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = c.indexOf("android") != -1;
            return b
        },
        isOperaMiniMobileBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = c.indexOf("opera mini") != -1 || c.indexOf("opera mobi") != -1;
            return b
        },
        isOperaMiniBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = c.indexOf("opera mini") != -1;
            return b
        },
        isNewSafariMobileBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = c.indexOf("ipad") != -1 || c.indexOf("iphone") != -1 || c.indexOf("ipod") != -1;
            b = b && (c.indexOf("version/5") != -1);
            return b
        },
        isSafari4MobileBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = c.indexOf("ipad") != -1 || c.indexOf("iphone") != -1 || c.indexOf("ipod") != -1;
            b = b && (c.indexOf("version/4") != -1);
            return b
        },
        isWindowsPhone: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = (c.indexOf("windows phone") != -1 || c.indexOf("WPDesktop") != -1 || c.indexOf("ZuneWP7") != -1 || c.indexOf("msie 9") != -1 || c.indexOf("msie 11") != -1 || c.indexOf("msie 10") != -1);
            return b
        },
        isSafariMobileBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = c.indexOf("ipad") != -1 || c.indexOf("iphone") != -1 || c.indexOf("ipod") != -1;
            return b
        },
        isIPadSafariMobileBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = c.indexOf("ipad") != -1;
            return b
        },
        isMobileBrowser: function() {
            var c = navigator.userAgent.toLowerCase();
            var b = c.indexOf("ipad") != -1 || c.indexOf("iphone") != -1 || c.indexOf("android") != -1;
            return b
        },
        getTouches: function(b) {
            if (b.originalEvent) {
                if (b.originalEvent.touches && b.originalEvent.touches.length) {
                    return b.originalEvent.touches
                } else {
                    if (b.originalEvent.changedTouches && b.originalEvent.changedTouches.length) {
                        return b.originalEvent.changedTouches
                    }
                }
            }
            if (!b.touches) {
                b.touches = new Array();
                b.touches[0] = b.originalEvent != undefined ? b.originalEvent : b;
                if (b.originalEvent != undefined && b.pageX) {
                    b.touches[0] = b
                }
                if (b.type == "mousemove") {
                    b.touches[0] = b
                }
            }
            return b.touches
        },
        getTouchEventName: function(b) {
            if (this.isWindowsPhone()) {
                var c = navigator.userAgent.toLowerCase();
                if (c.indexOf("windows phone 8.1") != -1) {
                    if (b.toLowerCase().indexOf("start") != -1) {
                        return "pointerdown"
                    }
                    if (b.toLowerCase().indexOf("move") != -1) {
                        return "pointermove"
                    }
                    if (b.toLowerCase().indexOf("end") != -1) {
                        return "pointerup"
                    }
                }
                if (b.toLowerCase().indexOf("start") != -1) {
                    return "MSPointerDown"
                }
                if (b.toLowerCase().indexOf("move") != -1) {
                    return "MSPointerMove"
                }
                if (b.toLowerCase().indexOf("end") != -1) {
                    return "MSPointerUp"
                }
            } else {
                return b
            }
        },
        dispatchMouseEvent: function(b, f, d) {
            if (this.simulatetouches) {
                return
            }
            var c = document.createEvent("MouseEvent");
            c.initMouseEvent(b, true, true, f.view, 1, f.screenX, f.screenY, f.clientX, f.clientY, false, false, false, false, 0, null);
            if (d != null) {
                d.dispatchEvent(c)
            }
        },
        getRootNode: function(b) {
            while (b.nodeType !== 1) {
                b = b.parentNode
            }
            return b
        },
        setTouchScroll: function(b, c) {
            if (!this.enableScrolling) {
                this.enableScrolling = []
            }
            this.enableScrolling[c] = b
        },
        touchScroll: function(d, y, g, D, b, k) {
            if (d == null) {
                return
            }
            var B = this;
            var t = 0;
            var j = 0;
            var l = 0;
            var u = 0;
            var m = 0;
            var n = 0;
            if (!this.scrolling) {
                this.scrolling = []
            }
            this.scrolling[D] = false;
            var h = false;
            var q = a(d);
            var v = ["select", "input", "textarea"];
            var c = 0;
            var e = 0;
            if (!this.enableScrolling) {
                this.enableScrolling = []
            }
            this.enableScrolling[D] = true;
            var D = D;
            var C = this.getTouchEventName("touchstart") + ".touchScroll";
            var p = this.getTouchEventName("touchend") + ".touchScroll";
            var A = this.getTouchEventName("touchmove") + ".touchScroll";
            var c = function(E) {
                if (!B.enableScrolling[D]) {
                    return true
                }
                if (a.inArray(E.target.tagName.toLowerCase(), v) !== -1) {
                    return
                }
                var F = B.getTouches(E);
                var G = F[0];
                if (F.length == 1) {
                    B.dispatchMouseEvent("mousedown", G, B.getRootNode(G.target))
                }
                h = false;
                j = G.pageY;
                m = G.pageX;
                if (B.simulatetouches) {
                    if (G._pageY != undefined) {
                        j = G._pageY;
                        m = G._pageX
                    }
                }
                B.scrolling[D] = true;
                t = 0;
                u = 0;
                return true
            };
            if (q.on) {
                q.on(C, c)
            } else {
                q.bind(C, c)
            }
            var x = function(J) {
                if (!B.enableScrolling[D]) {
                    return true
                }
                if (!B.scrolling[D]) {
                    return true
                }
                var L = B.getTouches(J);
                if (L.length > 1) {
                    return true
                }
                var H = L[0].pageY;
                var I = L[0].pageX;
                if (B.simulatetouches) {
                    if (L[0]._pageY != undefined) {
                        H = L[0]._pageY;
                        I = L[0]._pageX
                    }
                }
                var E = H - j;
                var F = I - m;
                e = H;
                touchHorizontalEnd = I;
                l = E - t;
                n = F - u;
                h = true;
                t = E;
                u = F;
                var G = b != null ? b[0].style.visibility != "hidden" : true;
                var K = k != null ? k[0].style.visibility != "hidden" : true;
                if (G || K) {
                    if ((n !== 0 && G) || (l !== 0 && K)) {
                        g(-n * 1, -l * 1, F, E, J);
                        J.preventDefault();
                        J.stopPropagation();
                        if (J.preventManipulation) {
                            J.preventManipulation()
                        }
                        return false
                    }
                }
            };
            if (q.on) {
                q.on(A, x)
            } else {
                q.bind(A, x)
            }
            if (this.simulatetouches) {
                var o = a(window).on != undefined || a(window).bind;
                var z = function(E) {
                    B.scrolling[D] = false
                };
                a(window).on != undefined ? a(document).on("mouseup.touchScroll", z) : a(document).bind("mouseup.touchScroll", z);
                if (window.frameElement) {
                    if (window.top != null) {
                        var r = function(E) {
                            B.scrolling[D] = false
                        };
                        if (window.top.document) {
                            a(window.top.document).on ? a(window.top.document).on("mouseup", r) : a(window.top.document).bind("mouseup", r)
                        }
                    }
                }
                var s = a(document).on != undefined || a(document).bind;
                var w = function(E) {
                    if (!B.scrolling[D]) {
                        return true
                    }
                    B.scrolling[D] = false;
                    var G = B.getTouches(E)[0],
                        F = B.getRootNode(G.target);
                    B.dispatchMouseEvent("mouseup", G, F);
                    B.dispatchMouseEvent("click", G, F)
                };
                a(document).on != undefined ? a(document).on("touchend", w) : a(document).bind("touchend", w)
            }
            var f = function(E) {
                if (!B.enableScrolling[D]) {
                    return true
                }
                var G = B.getTouches(E)[0];
                if (!B.scrolling[D]) {
                    return true
                }
                B.scrolling[D] = false;
                if (h) {
                    B.dispatchMouseEvent("mouseup", G, F)
                } else {
                    var G = B.getTouches(E)[0],
                        F = B.getRootNode(G.target);
                    B.dispatchMouseEvent("mouseup", G, F);
                    B.dispatchMouseEvent("click", G, F);
                    return true
                }
            };
            if (q.on) {
                q.on("dragstart", function(E) {
                    E.preventDefault()
                });
                q.on("selectstart", function(E) {
                    E.preventDefault()
                })
            }
            q.on ? q.on(p + " touchcancel.touchScroll", f) : q.bind(p + " touchcancel.touchScroll", f)
        }
    });
    a.jqx.cookie = a.jqx.cookie || {};
    a.extend(a.jqx.cookie, {
        cookie: function(e, f, c) {
            if (arguments.length > 1 && String(f) !== "[object Object]") {
                c = a.extend({}, c);
                if (f === null || f === undefined) {
                    c.expires = -1
                }
                if (typeof c.expires === "number") {
                    var h = c.expires,
                        d = c.expires = new Date();
                    d.setDate(d.getDate() + h)
                }
                f = String(f);
                return (document.cookie = [encodeURIComponent(e), "=", c.raw ? f : encodeURIComponent(f), c.expires ? "; expires=" + c.expires.toUTCString() : "", c.path ? "; path=" + c.path : "", c.domain ? "; domain=" + c.domain : "", c.secure ? "; secure" : ""].join(""))
            }
            c = f || {};
            var b, g = c.raw ? function(j) {
                return j
            } : decodeURIComponent;
            return (b = new RegExp("(?:^|; )" + encodeURIComponent(e) + "=([^;]*)").exec(document.cookie)) ? g(b[1]) : null
        }
    });
    a.jqx.string = a.jqx.string || {};
    a.extend(a.jqx.string, {
        replace: function(f, d, e) {
            if (d === e) {
                return this
            }
            var b = f;
            var c = b.indexOf(d);
            while (c != -1) {
                b = b.replace(d, e);
                c = b.indexOf(d)
            }
            return b
        },
        contains: function(b, c) {
            if (b == null || c == null) {
                return false
            }
            return b.indexOf(c) != -1
        },
        containsIgnoreCase: function(b, c) {
            if (b == null || c == null) {
                return false
            }
            return b.toUpperCase().indexOf(c.toUpperCase()) != -1
        },
        equals: function(b, c) {
            if (b == null || c == null) {
                return false
            }
            b = this.normalize(b);
            if (c.length == b.length) {
                return b.slice(0, c.length) == c
            }
            return false
        },
        equalsIgnoreCase: function(b, c) {
            if (b == null || c == null) {
                return false
            }
            b = this.normalize(b);
            if (c.length == b.length) {
                return b.toUpperCase().slice(0, c.length) == c.toUpperCase()
            }
            return false
        },
        startsWith: function(b, c) {
            if (b == null || c == null) {
                return false
            }
            return b.slice(0, c.length) == c
        },
        startsWithIgnoreCase: function(b, c) {
            if (b == null || c == null) {
                return false
            }
            return b.toUpperCase().slice(0, c.length) == c.toUpperCase()
        },
        normalize: function(b) {
            if (b.charCodeAt(b.length - 1) == 65279) {
                b = b.substring(0, b.length - 1)
            }
            return b
        },
        endsWith: function(b, c) {
            if (b == null || c == null) {
                return false
            }
            b = this.normalize(b);
            return b.slice(-c.length) == c
        },
        endsWithIgnoreCase: function(b, c) {
            if (b == null || c == null) {
                return false
            }
            b = this.normalize(b);
            return b.toUpperCase().slice(-c.length) == c.toUpperCase()
        }
    });
    a.extend(a.easing, {
        easeOutBack: function(f, g, e, k, j, h) {
            if (h == undefined) {
                h = 1.70158
            }
            return k * ((g = g / j - 1) * g * ((h + 1) * g + h) + 1) + e
        },
        easeInQuad: function(f, g, e, j, h) {
            return j * (g /= h) * g + e
        },
        easeInOutCirc: function(f, g, e, j, h) {
            if ((g /= h / 2) < 1) {
                return -j / 2 * (Math.sqrt(1 - g * g) - 1) + e
            }
            return j / 2 * (Math.sqrt(1 - (g -= 2) * g) + 1) + e
        },
        easeInOutSine: function(f, g, e, j, h) {
            return -j / 2 * (Math.cos(Math.PI * g / h) - 1) + e
        },
        easeInCubic: function(f, g, e, j, h) {
            return j * (g /= h) * g * g + e
        },
        easeOutCubic: function(f, g, e, j, h) {
            return j * ((g = g / h - 1) * g * g + 1) + e
        },
        easeInOutCubic: function(f, g, e, j, h) {
            if ((g /= h / 2) < 1) {
                return j / 2 * g * g * g + e
            }
            return j / 2 * ((g -= 2) * g * g + 2) + e
        },
        easeInSine: function(f, g, e, j, h) {
            return -j * Math.cos(g / h * (Math.PI / 2)) + j + e
        },
        easeOutSine: function(f, g, e, j, h) {
            return j * Math.sin(g / h * (Math.PI / 2)) + e
        },
        easeInOutSine: function(f, g, e, j, h) {
            return -j / 2 * (Math.cos(Math.PI * g / h) - 1) + e
        }
    })
})(jqxBaseFramework);
(function(b) {
    b.extend(b.event.special, {
        close: {
            noBubble: true
        },
        open: {
            noBubble: true
        },
        cellclick: {
            noBubble: true
        },
        rowclick: {
            noBubble: true
        },
        tabclick: {
            noBubble: true
        },
        selected: {
            noBubble: true
        },
        expanded: {
            noBubble: true
        },
        collapsed: {
            noBubble: true
        },
        valuechanged: {
            noBubble: true
        },
        expandedItem: {
            noBubble: true
        },
        collapsedItem: {
            noBubble: true
        },
        expandingItem: {
            noBubble: true
        },
        collapsingItem: {
            noBubble: true
        }
    });
    b.fn.extend({
        ischildof: function(f) {
            var d = b(this).parents().get();
            for (var c = 0; c < d.length; c++) {
                if (typeof f != "string") {
                    var e = d[c];
                    if (f !== undefined) {
                        if (e == f[0]) {
                            return true
                        }
                    }
                } else {
                    if (f !== undefined) {
                        if (b(d[c]).is(f)) {
                            return true
                        }
                    }
                }
            }
            return false
        }
    });
    b.fn.jqxProxy = function() {
        var e = b(this).data().jqxWidget;
        var c = Array.prototype.slice.call(arguments, 0);
        var d = e.element;
        if (!d) {
            d = e.base.element
        }
        return b.jqx.jqxWidgetProxy(e.widgetName, d, c)
    };
    var a = this.originalVal = b.fn.val;
    b.fn.val = function(d) {
        if (typeof d == "undefined") {
            if (b(this).hasClass("jqx-widget")) {
                var c = b(this).data().jqxWidget;
                if (c && c.val) {
                    return c.val()
                }
            }
            return a.call(this)
        } else {
            if (b(this).hasClass("jqx-widget")) {
                var c = b(this).data().jqxWidget;
                if (c && c.val) {
                    if (arguments.length != 2) {
                        return c.val(d)
                    } else {
                        return c.val(d, arguments[1])
                    }
                }
            }
            return a.call(this, d)
        }
    };
    b.fn.coord = function(o) {
        var e, k, j = {
                top: 0,
                left: 0
            },
            f = this[0],
            m = f && f.ownerDocument;
        if (!m) {
            return
        }
        e = m.documentElement;
        if (!b.contains(e, f)) {
            return j
        }
        if (typeof f.getBoundingClientRect !== undefined) {
            j = f.getBoundingClientRect()
        }
        var d = function(p) {
            return b.isWindow(p) ? p : p.nodeType === 9 ? p.defaultView || p.parentWindow : false
        };
        k = d(m);
        var h = 0;
        var c = 0;
        var g = navigator.userAgent.toLowerCase();
        var n = g.indexOf("ipad") != -1 || g.indexOf("iphone") != -1;
        if (n) {
            h = 2
        }
        if (true == o) {
            if (b(document.body).css("position") != "static") {
                var l = b(document.body).coord();
                h = -l.left;
                c = -l.top
            }
        }
        return {
            top: c + j.top + (k.pageYOffset || e.scrollTop) - (e.clientTop || 0),
            left: h + j.left + (k.pageXOffset || e.scrollLeft) - (e.clientLeft || 0)
        }
    }
})(jqxBaseFramework);