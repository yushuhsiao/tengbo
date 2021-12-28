/*
jQWidgets v3.6.0 (2014-Nov-25)
Copyright (c) 2011-2014 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a) {
    a.jqx.jqxWidget("jqxDraw", "", {});
    a.extend(a.jqx._jqxDraw.prototype, {
        defineInstance: function() {
            var d = {
                renderEngine: ""
            };
            a.extend(true, this, d);
            var e = ["clear", "on", "off", "removeElement", "attr", "getAttr", "line", "circle", "rect", "path", "pieslice", "text", "measureText"];
            for (var c in e) {
                this._addFn(a.jqx._jqxDraw.prototype, e[c])
            }
            return d
        },
        _addFn: function(d, c) {
            if (d[c]) {
                return
            }
            d[c] = function() {
                return this.renderer[c].apply(this.renderer, arguments)
            }
        },
        createInstance: function(c) {},
        _initRenderer: function(c) {
            return a.jqx.createRenderer(this, c)
        },
        _internalRefresh: function() {
            var c = this;
            if (a.jqx.isHidden(c.host)) {
                return
            }
            if (!c.renderer) {
                c.host.empty();
                c._initRenderer(c.host)
            }
            var e = c.renderer;
            if (!e) {
                return
            }
            var d = e.getRect();
            c._render({
                x: 1,
                y: 1,
                width: d.width,
                height: d.height
            });
            if (e instanceof a.jqx.HTML5Renderer) {
                e.refresh()
            }
        },
        _saveAsImage: function(e, f, c, d) {
            return a.jqx._widgetToImage(this, e, f, c, d)
        },
        _render: function(d) {
            var c = this;
            var e = c.renderer;
            c._plotRect = d
        },
        refresh: function() {
            this._internalRefresh()
        },
        getSize: function() {
            var c = this._plotRect;
            return {
                width: c.width,
                height: c.height
            }
        },
        saveAsPNG: function(e, c, d) {
            return this._saveAsImage("png", e, c, d)
        },
        saveAsJPEG: function(e, c, d) {
            return this._saveAsImage("jpeg", e, c, d)
        }
    })
})(jqxBaseFramework);
(function(a) {
    a.jqx.toGreyScale = function(c) {
        if (c.indexOf("#") == -1) {
            return c
        }
        var d = a.jqx.cssToRgb(c);
        d[0] = d[1] = d[2] = Math.round(0.3 * d[0] + 0.59 * d[1] + 0.11 * d[2]);
        var e = a.jqx.rgbToHex(d[0], d[1], d[2]);
        return "#" + e[0] + e[1] + e[2]
    }, a.jqx.adjustColor = function(f, e) {
        if (typeof(f) != "string") {
            return "#000000"
        }
        if (f.indexOf("#") == -1) {
            return f
        }
        var h = a.jqx.cssToRgb(f);
        var d = a.jqx.rgbToHsl(h);
        d[2] = Math.min(1, d[2] * e);
        d[1] = Math.min(1, d[1] * e * 1.1);
        h = a.jqx.hslToRgb(d);
        var f = "#";
        for (var j = 0; j < 3; j++) {
            var k = Math.round(h[j]);
            k = a.jqx.decToHex(k);
            if (k.toString().length == 1) {
                f += "0"
            }
            f += k
        }
        return f.toUpperCase()
    };
    a.jqx.decToHex = function(c) {
        return c.toString(16)
    };
    a.jqx.hexToDec = function(c) {
        return parseInt(c, 16)
    };
    a.jqx.rgbToHex = function(e, d, c) {
        return [a.jqx.decToHex(e), a.jqx.decToHex(d), a.jqx.decToHex(c)]
    };
    a.jqx.hexToRgb = function(d, f, c) {
        return [a.jqx.hexToDec(d), a.jqx.hexToDec(f), a.jqx.hexToDec(c)]
    };
    a.jqx.cssToRgb = function(c) {
        if (c.indexOf("rgb") <= -1) {
            return a.jqx.hexToRgb(c.substring(1, 3), c.substring(3, 5), c.substring(5, 7))
        }
        return c.substring(4, c.length - 1).split(",")
    };
    a.jqx.hslToRgb = function(d) {
        var f = parseFloat(d[0]);
        var e = parseFloat(d[1]);
        var c = parseFloat(d[2]);
        if (e == 0) {
            r = g = b = c
        } else {
            var i = c < 0.5 ? c * (1 + e) : c + e - c * e;
            var j = 2 * c - i;
            r = a.jqx.hueToRgb(j, i, f + 1 / 3);
            g = a.jqx.hueToRgb(j, i, f);
            b = a.jqx.hueToRgb(j, i, f - 1 / 3)
        }
        return [r * 255, g * 255, b * 255]
    };
    a.jqx.hueToRgb = function(e, d, c) {
        if (c < 0) {
            c += 1
        }
        if (c > 1) {
            c -= 1
        }
        if (c < 1 / 6) {
            return e + (d - e) * 6 * c
        } else {
            if (c < 1 / 2) {
                return d
            } else {
                if (c < 2 / 3) {
                    return e + (d - e) * (2 / 3 - c) * 6
                }
            }
        }
        return e
    };
    a.jqx.rgbToHsl = function(j) {
        var c = parseFloat(j[0]) / 255;
        var i = parseFloat(j[1]) / 255;
        var k = parseFloat(j[2]) / 255;
        var m = Math.max(c, i, k),
            e = Math.min(c, i, k);
        var f, o, d = (m + e) / 2;
        if (m == e) {
            f = o = 0
        } else {
            var n = m - e;
            o = d > 0.5 ? n / (2 - m - e) : n / (m + e);
            switch (m) {
                case c:
                    f = (i - k) / n + (i < k ? 6 : 0);
                    break;
                case i:
                    f = (k - c) / n + 2;
                    break;
                case k:
                    f = (c - i) / n + 4;
                    break
            }
            f /= 6
        }
        return [f, o, d]
    };
    a.jqx.swap = function(c, e) {
        var d = c;
        c = e;
        e = d
    };
    a.jqx.getNum = function(c) {
        if (!a.isArray(c)) {
            if (isNaN(c)) {
                return 0
            }
        } else {
            for (var d = 0; d < c.length; d++) {
                if (!isNaN(c[d])) {
                    return c[d]
                }
            }
        }
        return 0
    };
    a.jqx._ptdist = function(d, f, c, e) {
        return Math.sqrt((c - d) * (c - d) + (e - f) * (e - f))
    };
    a.jqx._ptrnd = function(d) {
        if (!document.createElementNS) {
            if (Math.round(d) == d) {
                return d
            }
            return a.jqx._rnd(d, 1, false, true)
        }
        var c = a.jqx._rnd(d, 0.5, false, true);
        if (Math.abs(c - Math.round(c)) != 0.5) {
            return c > d ? c - 0.5 : c + 0.5
        }
        return c
    };
    a.jqx._rup = function(d) {
        var c = Math.round(d);
        if (d > c) {
            c++
        }
        return c
    };
    a.jqx.log = function(d, c) {
        return Math.log(d) / (c ? Math.log(c) : 1)
    };
    a.jqx._mod = function(d, c) {
        var e = Math.abs(d > c ? c : d);
        var f = 1;
        if (e != 0) {
            while (e * f < 100) {
                f *= 10
            }
        }
        d = d * f;
        c = c * f;
        return (d % c) / f
    };
    a.jqx._rnd = function(e, h, f, d) {
        if (isNaN(e)) {
            return e
        }
        var c = e - ((d == true) ? e % h : a.jqx._mod(e, h));
        if (e == c) {
            return c
        }
        if (f) {
            if (e > c) {
                c += h
            }
        } else {
            if (c > e) {
                c -= h
            }
        }
        return c
    };
    a.jqx.commonRenderer = {
        pieSlicePath: function(l, k, i, t, C, D, e) {
            if (!t) {
                t = 1
            }
            var n = Math.abs(C - D);
            var q = n > 180 ? 1 : 0;
            if (n >= 360) {
                D = C + 359.99
            }
            var s = C * Math.PI * 2 / 360;
            var j = D * Math.PI * 2 / 360;
            var A = l,
                z = l,
                h = k,
                f = k;
            var o = !isNaN(i) && i > 0;
            if (o) {
                e = 0
            }
            if (e + i > 0) {
                if (e > 0) {
                    var m = n / 2 + C;
                    var B = m * Math.PI * 2 / 360;
                    l += e * Math.cos(B);
                    k -= e * Math.sin(B)
                }
                if (o) {
                    var w = i;
                    A = l + w * Math.cos(s);
                    h = k - w * Math.sin(s);
                    z = l + w * Math.cos(j);
                    f = k - w * Math.sin(j)
                }
            }
            var v = l + t * Math.cos(s);
            var u = l + t * Math.cos(j);
            var d = k - t * Math.sin(s);
            var c = k - t * Math.sin(j);
            var p = "";
            if (o) {
                p = "M " + z + "," + f;
                p += " a" + i + "," + i;
                p += " 0 " + q + ",1 " + (A - z) + "," + (h - f);
                p += " L" + v + "," + d;
                p += " a" + t + "," + t;
                p += " 0 " + q + ",0 " + (u - v) + "," + (c - d)
            } else {
                p = "M " + u + "," + c;
                p += " a" + t + "," + t;
                p += " 0 " + q + ",1 " + (v - u) + "," + (d - c);
                p += " L" + l + "," + k + " Z"
            }
            return p
        },
        measureText: function(q, h, i, p, n) {
            var f = n._getTextParts(q, h, i);
            var k = f.width;
            var c = f.height;
            if (false == p) {
                c /= 0.6
            }
            var d = {};
            if (isNaN(h)) {
                h = 0
            }
            if (h == 0) {
                d = {
                    width: a.jqx._rup(k),
                    height: a.jqx._rup(c)
                }
            } else {
                var m = h * Math.PI * 2 / 360;
                var e = Math.abs(Math.sin(m));
                var l = Math.abs(Math.cos(m));
                var j = Math.abs(k * e + c * l);
                var o = Math.abs(k * l + c * e);
                d = {
                    width: a.jqx._rup(o),
                    height: a.jqx._rup(j)
                }
            }
            if (p) {
                d.textPartsInfo = f
            }
            return d
        },
        alignTextInRect: function(t, p, c, u, o, q, k, s, f, e) {
            var m = f * Math.PI * 2 / 360;
            var d = Math.sin(m);
            var l = Math.cos(m);
            var n = o * d;
            var j = o * l;
            if (k == "center" || k == "" || k == "undefined") {
                t = t + c / 2
            } else {
                if (k == "right") {
                    t = t + c
                }
            }
            if (s == "center" || s == "middle" || s == "" || s == "undefined") {
                p = p + u / 2
            } else {
                if (s == "bottom") {
                    p += u - q / 2
                } else {
                    if (s == "top") {
                        p += q / 2
                    }
                }
            }
            e = e || "";
            var h = "middle";
            if (e.indexOf("top") != -1) {
                h = "top"
            } else {
                if (e.indexOf("bottom") != -1) {
                    h = "bottom"
                }
            }
            var i = "center";
            if (e.indexOf("left") != -1) {
                i = "left"
            } else {
                if (e.indexOf("right") != -1) {
                    i = "right"
                }
            }
            if (i == "center") {
                t -= j / 2;
                p -= n / 2
            } else {
                if (i == "right") {
                    t -= j;
                    p -= n
                }
            }
            if (h == "top") {
                t -= q * d;
                p += q * l
            } else {
                if (h == "middle") {
                    t -= q * d / 2;
                    p += q * l / 2
                }
            }
            t = a.jqx._rup(t);
            p = a.jqx._rup(p);
            return {
                x: t,
                y: p
            }
        }
    };
    a.jqx.svgRenderer = function() {};
    a.jqx.svgRenderer.prototype = {
        _svgns: "http://www.w3.org/2000/svg",
        init: function(h) {
            var f = "<table id=tblChart cellspacing='0' cellpadding='0' border='0' align='left' valign='top'><tr><td colspan=2 id=tdTop></td></tr><tr><td id=tdLeft></td><td><div class='chartContainer' onselectstart='return false;'></div></td></tr></table>";
            h.append(f);
            this.host = h;
            var c = h.find(".chartContainer");
            c[0].style.width = h.width() + "px";
            c[0].style.height = h.height() + "px";
            var j;
            try {
                var d = document.createElementNS(this._svgns, "svg");
                d.setAttribute("id", "svgChart");
                d.setAttribute("version", "1.1");
                d.setAttribute("width", "100%");
                d.setAttribute("height", "100%");
                d.setAttribute("overflow", "hidden");
                c[0].appendChild(d);
                this.canvas = d
            } catch (i) {
                return false
            }
            this._id = new Date().getTime();
            this.clear();
            this._layout();
            this._runLayoutFix();
            return true
        },
        refresh: function() {},
        _runLayoutFix: function() {
            var c = this;
            this._fixLayout()
        },
        _fixLayout: function() {
            var i = a(this.canvas).position();
            var e = (parseFloat(i.left) == parseInt(i.left));
            var c = (parseFloat(i.top) == parseInt(i.top));
            if (a.jqx.browser.msie) {
                var e = true,
                    c = true;
                var f = this.host;
                var d = 0,
                    h = 0;
                while (f && f.position && f[0].parentNode) {
                    var j = f.position();
                    d += parseFloat(j.left) - parseInt(j.left);
                    h += parseFloat(j.top) - parseInt(j.top);
                    f = f.parent()
                }
                e = parseFloat(d) == parseInt(d);
                c = parseFloat(h) == parseInt(h)
            }
            if (!e) {
                this.host.find("#tdLeft")[0].style.width = "0.5px"
            }
            if (!c) {
                this.host.find("#tdTop")[0].style.height = "0.5px"
            }
        },
        _layout: function() {
            var d = a(this.canvas).offset();
            var c = this.host.find(".chartContainer");
            this._width = Math.max(a.jqx._rup(this.host.width()) - 1, 0);
            this._height = Math.max(a.jqx._rup(this.host.height()) - 1, 0);
            c[0].style.width = this._width;
            c[0].style.height = this._height;
            this._fixLayout()
        },
        getRect: function() {
            return {
                x: 0,
                y: 0,
                width: this._width,
                height: this._height
            }
        },
        getContainer: function() {
            var c = this.host.find(".chartContainer");
            return c
        },
        clear: function() {
            while (this.canvas.childElementCount > 0) {
                this.removeElement(this.canvas.firstElementChild)
            }
            this._defaultParent = undefined;
            this._defs = document.createElementNS(this._svgns, "defs");
            this._gradients = {};
            this.canvas.appendChild(this._defs)
        },
        removeElement: function(e) {
            if (undefined == e) {
                return
            }
            this.removeHandler(e);
            try {
                while (e.firstChild) {
                    this.removeElement(e.firstChild)
                }
                if (e.parentNode) {
                    e.parentNode.removeChild(e)
                } else {
                    this.canvas.removeChild(e)
                }
            } catch (d) {
                var c = d
            }
        },
        _openGroups: [],
        beginGroup: function() {
            var c = this._activeParent();
            var d = document.createElementNS(this._svgns, "g");
            c.appendChild(d);
            this._openGroups.push(d);
            return d
        },
        endGroup: function() {
            if (this._openGroups.length == 0) {
                return
            }
            this._openGroups.pop()
        },
        _activeParent: function() {
            return this._openGroups.length == 0 ? this.canvas : this._openGroups[this._openGroups.length - 1]
        },
        createClipRect: function(e) {
            var f = document.createElementNS(this._svgns, "clipPath");
            var d = document.createElementNS(this._svgns, "rect");
            this.attr(d, {
                x: e.x,
                y: e.y,
                width: e.width,
                height: e.height,
                fill: "none"
            });
            this._clipId = this._clipId || 0;
            f.id = "cl" + this._id + "_" + (++this._clipId).toString();
            f.appendChild(d);
            this._defs.appendChild(f);
            return f
        },
        setClip: function(d, c) {
            return this.attr(d, {
                "clip-path": "url(#" + c.id + ")"
            })
        },
        _clipId: 0,
        addHandler: function(c, e, d) {
            if (a(c).on) {
                a(c).on(e, d)
            } else {
                a(c).bind(e, d)
            }
        },
        removeHandler: function(c, e, d) {
            if (a(c).off) {
                a(c).off(e, d)
            } else {
                a(c).unbind(e, d)
            }
        },
        on: function(c, e, d) {
            this.addHandler(c, e, d)
        },
        off: function(c, e, d) {
            this.removeHandler(c, e, d)
        },
        shape: function(c, f) {
            var d = document.createElementNS(this._svgns, c);
            if (!d) {
                return undefined
            }
            for (var e in f) {
                d.setAttribute(e, f[e])
            }
            this._activeParent().appendChild(d);
            return d
        },
        _getTextParts: function(t, j, k) {
            var h = {
                width: 0,
                height: 0,
                parts: []
            };
            var o = 0.6;
            var u = t.toString().split("<br>");
            var q = this._activeParent();
            var m = document.createElementNS(this._svgns, "text");
            this.attr(m, k);
            for (var l = 0; l < u.length; l++) {
                var d = u[l];
                var f = m.ownerDocument.createTextNode(d);
                m.appendChild(f);
                q.appendChild(m);
                var s;
                try {
                    s = m.getBBox()
                } catch (p) {}
                var n = a.jqx._rup(s.width);
                var c = a.jqx._rup(s.height * o);
                m.removeChild(f);
                h.width = Math.max(h.width, n);
                h.height += c + (l > 0 ? 4 : 0);
                h.parts.push({
                    width: n,
                    height: c,
                    text: d
                })
            }
            q.removeChild(m);
            return h
        },
        _measureText: function(f, e, d, c) {
            return a.jqx.commonRenderer.measureText(f, e, d, c, this)
        },
        measureText: function(e, d, c) {
            return this._measureText(e, d, c, false)
        },
        text: function(z, t, s, E, C, K, M, L, v, m, d) {
            var B = this._measureText(z, K, M, true);
            var l = B.textPartsInfo;
            var j = l.parts;
            var D;
            if (!v) {
                v = "center"
            }
            if (!m) {
                m = "center"
            }
            if (j.length > 1 || L) {
                D = this.beginGroup()
            }
            if (L) {
                var k = this.createClipRect({
                    x: a.jqx._rup(t) - 1,
                    y: a.jqx._rup(s) - 1,
                    width: a.jqx._rup(E) + 2,
                    height: a.jqx._rup(C) + 2
                });
                this.setClip(D, k)
            }
            var q = this._activeParent();
            var O = 0,
                n = 0;
            var c = 0.6;
            O = l.width;
            n = l.height;
            if (isNaN(E) || E <= 0) {
                E = O
            }
            if (isNaN(C) || C <= 0) {
                C = n
            }
            var u = E || 0;
            var J = C || 0;
            if (!K || K == 0) {
                s += n;
                if (m == "center" || m == "middle") {
                    s += (J - n) / 2
                } else {
                    if (m == "bottom") {
                        s += J - n
                    }
                }
                if (!E) {
                    E = O
                }
                if (!C) {
                    C = n
                }
                var q = this._activeParent();
                var p = 0;
                for (var I = j.length - 1; I >= 0; I--) {
                    var A = document.createElementNS(this._svgns, "text");
                    this.attr(A, M);
                    this.attr(A, {
                        cursor: "default"
                    });
                    var H = A.ownerDocument.createTextNode(j[I].text);
                    A.appendChild(H);
                    var P = t;
                    var o = j[I].width;
                    var f = j[I].height;
                    if (v == "center") {
                        P += (u - o) / 2
                    } else {
                        if (v == "right") {
                            P += (u - o)
                        }
                    }
                    this.attr(A, {
                        x: a.jqx._rup(P),
                        y: a.jqx._rup(s + p),
                        width: a.jqx._rup(o),
                        height: a.jqx._rup(f)
                    });
                    q.appendChild(A);
                    p -= j[I].height + 4
                }
                if (D) {
                    this.endGroup();
                    return D
                }
                return A
            }
            var F = a.jqx.commonRenderer.alignTextInRect(t, s, E, C, O, n, v, m, K, d);
            t = F.x;
            s = F.y;
            var G = this.shape("g", {
                transform: "translate(" + t + "," + s + ")"
            });
            var e = this.shape("g", {
                transform: "rotate(" + K + ")"
            });
            G.appendChild(e);
            var p = 0;
            for (var I = j.length - 1; I >= 0; I--) {
                var N = document.createElementNS(this._svgns, "text");
                this.attr(N, M);
                this.attr(N, {
                    cursor: "default"
                });
                var H = N.ownerDocument.createTextNode(j[I].text);
                N.appendChild(H);
                var P = 0;
                var o = j[I].width;
                var f = j[I].height;
                if (v == "center") {
                    P += (l.width - o) / 2
                } else {
                    if (v == "right") {
                        P += (l.width - o)
                    }
                }
                this.attr(N, {
                    x: a.jqx._rup(P),
                    y: a.jqx._rup(p),
                    width: a.jqx._rup(o),
                    height: a.jqx._rup(f)
                });
                e.appendChild(N);
                p -= f + 4
            }
            q.appendChild(G);
            if (D) {
                this.endGroup()
            }
            return G
        },
        line: function(e, h, d, f, i) {
            var c = this.shape("line", {
                x1: e,
                y1: h,
                x2: d,
                y2: f
            });
            this.attr(c, i);
            return c
        },
        path: function(d, e) {
            var c = this.shape("path");
            c.setAttribute("d", d);
            if (e) {
                this.attr(c, e)
            }
            return c
        },
        rect: function(c, j, d, f, i) {
            c = a.jqx._ptrnd(c);
            j = a.jqx._ptrnd(j);
            d = a.jqx._rup(d);
            f = a.jqx._rup(f);
            var e = this.shape("rect", {
                x: c,
                y: j,
                width: d,
                height: f
            });
            if (i) {
                this.attr(e, i)
            }
            return e
        },
        circle: function(c, h, e, f) {
            var d = this.shape("circle", {
                cx: c,
                cy: h,
                r: e
            });
            if (f) {
                this.attr(d, f)
            }
            return d
        },
        pieSlicePath: function(d, j, i, f, h, e, c) {
            return a.jqx.commonRenderer.pieSlicePath(d, j, i, f, h, e, c)
        },
        pieslice: function(l, j, i, e, h, c, k, d) {
            var f = this.pieSlicePath(l, j, i, e, h, c, k);
            var m = this.shape("path");
            m.setAttribute("d", f);
            if (d) {
                this.attr(m, d)
            }
            return m
        },
        attr: function(c, e) {
            if (!c || !e) {
                return
            }
            for (var d in e) {
                if (d == "textContent") {
                    c.textContent = e[d]
                } else {
                    c.setAttribute(d, e[d])
                }
            }
        },
        getAttr: function(d, c) {
            return d.getAttribute(c)
        },
        _gradients: {},
        _toLinearGradient: function(f, i, j) {
            var d = "grd" + this._id + f.replace("#", "") + (i ? "v" : "h");
            var c = "url(#" + d + ")";
            if (this._gradients[c]) {
                return c
            }
            var e = document.createElementNS(this._svgns, "linearGradient");
            this.attr(e, {
                x1: "0%",
                y1: "0%",
                x2: i ? "0%" : "100%",
                y2: i ? "100%" : "0%",
                id: d
            });
            for (var h in j) {
                var l = document.createElementNS(this._svgns, "stop");
                var k = "stop-color:" + a.jqx.adjustColor(f, j[h][1]);
                this.attr(l, {
                    offset: j[h][0] + "%",
                    style: k
                });
                e.appendChild(l)
            }
            this._defs.appendChild(e);
            this._gradients[c] = true;
            return c
        },
        _toRadialGradient: function(f, j, i) {
            var d = "grd" + this._id + f.replace("#", "") + "r" + (i != undefined ? i.key : "");
            var c = "url(#" + d + ")";
            if (this._gradients[c]) {
                return c
            }
            var e = document.createElementNS(this._svgns, "radialGradient");
            if (i == undefined) {
                this.attr(e, {
                    cx: "50%",
                    cy: "50%",
                    r: "100%",
                    fx: "50%",
                    fy: "50%",
                    id: d
                })
            } else {
                this.attr(e, {
                    cx: i.x,
                    cy: i.y,
                    r: i.outerRadius,
                    id: d,
                    gradientUnits: "userSpaceOnUse"
                })
            }
            for (var h in j) {
                var l = document.createElementNS(this._svgns, "stop");
                var k = "stop-color:" + a.jqx.adjustColor(f, j[h][1]);
                this.attr(l, {
                    offset: j[h][0] + "%",
                    style: k
                });
                e.appendChild(l)
            }
            this._defs.appendChild(e);
            this._gradients[c] = true;
            return c
        }
    };
    a.jqx.vmlRenderer = function() {};
    a.jqx.vmlRenderer.prototype = {
        init: function(j) {
            var h = "<div class='chartContainer' style=\"position:relative;overflow:hidden;\"><div>";
            j.append(h);
            this.host = j;
            var c = j.find(".chartContainer");
            c[0].style.width = j.width() + "px";
            c[0].style.height = j.height() + "px";
            var f = true;
            try {
                for (var d = 0; d < document.namespaces.length; d++) {
                    if (document.namespaces[d].name == "v" && document.namespaces[d].urn == "urn:schemas-microsoft-com:vml") {
                        f = false;
                        break
                    }
                }
            } catch (k) {
                return false
            }
            if (a.jqx.browser.msie && parseInt(a.jqx.browser.version) < 9 && (document.childNodes && document.childNodes.length > 0 && document.childNodes[0].data && document.childNodes[0].data.indexOf("DOCTYPE") != -1)) {
                if (f) {
                    document.namespaces.add("v", "urn:schemas-microsoft-com:vml")
                }
                this._ie8mode = true
            } else {
                if (f) {
                    document.namespaces.add("v", "urn:schemas-microsoft-com:vml");
                    document.createStyleSheet().cssText = "v\\:* { behavior: url(#default#VML); display: inline-block; }"
                }
            }
            this.canvas = c[0];
            this._width = Math.max(a.jqx._rup(c.width()), 0);
            this._height = Math.max(a.jqx._rup(c.height()), 0);
            c[0].style.width = this._width + 2;
            c[0].style.height = this._height + 2;
            this._id = new Date().getTime();
            this.clear();
            return true
        },
        refresh: function() {},
        getRect: function() {
            return {
                x: 0,
                y: 0,
                width: this._width,
                height: this._height
            }
        },
        getContainer: function() {
            var c = this.host.find(".chartContainer");
            return c
        },
        clear: function() {
            while (this.canvas.childElementCount > 0) {
                this.removeHandler(this.canvas.firstElementChild);
                this.canvas.removeChild(this.canvas.firstElementChild)
            }
            this._gradients = {};
            this._defaultParent = undefined
        },
        removeElement: function(c) {
            if (c != null) {
                this.removeHandler(c);
                c.parentNode.removeChild(c)
            }
        },
        _openGroups: [],
        beginGroup: function() {
            var c = this._activeParent();
            var d = document.createElement("v:group");
            d.style.position = "absolute";
            d.coordorigin = "0,0";
            d.coordsize = this._width + "," + this._height;
            d.style.left = 0;
            d.style.top = 0;
            d.style.width = this._width;
            d.style.height = this._height;
            c.appendChild(d);
            this._openGroups.push(d);
            return d
        },
        endGroup: function() {
            if (this._openGroups.length == 0) {
                return
            }
            this._openGroups.pop()
        },
        _activeParent: function() {
            return this._openGroups.length == 0 ? this.canvas : this._openGroups[this._openGroups.length - 1]
        },
        createClipRect: function(c) {
            var d = document.createElement("div");
            d.style.height = (c.height + 1) + "px";
            d.style.width = (c.width + 1) + "px";
            d.style.position = "absolute";
            d.style.left = c.x + "px";
            d.style.top = c.y + "px";
            d.style.overflow = "hidden";
            this._clipId = this._clipId || 0;
            d.id = "cl" + this._id + "_" + (++this._clipId).toString();
            this._activeParent().appendChild(d);
            return d
        },
        setClip: function(d, c) {},
        _clipId: 0,
        addHandler: function(c, e, d) {
            if (a(c).on) {
                a(c).on(e, d)
            } else {
                a(c).bind(e, d)
            }
        },
        removeHandler: function(c, e, d) {
            if (a(c).off) {
                a(c).off(e, d)
            } else {
                a(c).unbind(e, d)
            }
        },
        on: function(c, e, d) {
            this.addHandler(c, e, d)
        },
        off: function(c, e, d) {
            this.removeHandler(c, e, d)
        },
        _getTextParts: function(q, h, j) {
            var f = {
                width: 0,
                height: 0,
                parts: []
            };
            var o = 0.6;
            var s = q.toString().split("<br>");
            var p = this._activeParent();
            var l = document.createElement("v:textbox");
            this.attr(l, j);
            p.appendChild(l);
            for (var k = 0; k < s.length; k++) {
                var d = s[k];
                var e = document.createElement("span");
                e.appendChild(document.createTextNode(d));
                l.appendChild(e);
                if (j && j["class"]) {
                    e.className = j["class"]
                }
                var n = a(l);
                var m = a.jqx._rup(n.width());
                var c = a.jqx._rup(n.height() * o);
                if (c == 0 && a.jqx.browser.msie && parseInt(a.jqx.browser.version) < 9) {
                    var t = n.css("font-size");
                    if (t) {
                        c = parseInt(t);
                        if (isNaN(c)) {
                            c = 0
                        }
                    }
                }
                l.removeChild(e);
                f.width = Math.max(f.width, m);
                f.height += c + (k > 0 ? 2 : 0);
                f.parts.push({
                    width: m,
                    height: c,
                    text: d
                })
            }
            p.removeChild(l);
            return f
        },
        _measureText: function(f, e, d, c) {
            if (Math.abs(e) > 45) {
                e = 90
            } else {
                e = 0
            }
            return a.jqx.commonRenderer.measureText(f, e, d, c, this)
        },
        measureText: function(e, d, c) {
            return this._measureText(e, d, c, false)
        },
        text: function(u, p, o, D, z, J, L, K, t, k) {
            var E;
            if (L && L.stroke) {
                E = L.stroke
            }
            if (E == undefined) {
                E = "black"
            }
            var v = this._measureText(u, J, L, true);
            var f = v.textPartsInfo;
            var c = f.parts;
            var M = v.width;
            var l = v.height;
            if (isNaN(D) || D == 0) {
                D = M
            }
            if (isNaN(z) || z == 0) {
                z = l
            }
            var B;
            if (!t) {
                t = "center"
            }
            if (!k) {
                k = "center"
            }
            if (c.length > 0 || K) {
                B = this.beginGroup()
            }
            if (K) {
                var d = this.createClipRect({
                    x: a.jqx._rup(p),
                    y: a.jqx._rup(o),
                    width: a.jqx._rup(D),
                    height: a.jqx._rup(z)
                });
                this.setClip(B, d)
            }
            var n = this._activeParent();
            var s = D || 0;
            var I = z || 0;
            if (Math.abs(J) > 45) {
                J = 90
            } else {
                J = 0
            }
            var A = 0,
                H = 0;
            if (t == "center") {
                A += (s - M) / 2
            } else {
                if (t == "right") {
                    A += (s - M)
                }
            }
            if (k == "center") {
                H = (I - l) / 2
            } else {
                if (k == "bottom") {
                    H = I - l
                }
            }
            if (J == 0) {
                o += l + H;
                p += A
            } else {
                p += M + A;
                o += H
            }
            var m = 0,
                N = 0;
            var e;
            for (var G = c.length - 1; G >= 0; G--) {
                var C = c[G];
                var q = (M - C.width) / 2;
                if (J == 0 && t == "left") {
                    q = 0
                } else {
                    if (J == 0 && t == "right") {
                        q = M - C.width
                    } else {
                        if (J == 90) {
                            q = (l - C.width) / 2
                        }
                    }
                }
                var j = m - C.height;
                H = J == 90 ? q : j;
                A = J == 90 ? j : q;
                e = document.createElement("v:textbox");
                e.style.position = "absolute";
                e.style.left = a.jqx._rup(p + A);
                e.style.top = a.jqx._rup(o + H);
                e.style.width = a.jqx._rup(C.width);
                e.style.height = a.jqx._rup(C.height);
                if (J == 90) {
                    e.style.filter = "progid:DXImageTransform.Microsoft.BasicImage(rotation=3)"
                }
                var F = document.createElement("span");
                F.appendChild(document.createTextNode(C.text));
                if (L && L["class"]) {
                    F.className = L["class"]
                }
                e.appendChild(F);
                n.appendChild(e);
                m -= C.height + (G > 0 ? 2 : 0)
            }
            if (B) {
                this.endGroup();
                return n
            }
            return e
        },
        shape: function(c, f) {
            var d = document.createElement(this._createElementMarkup(c));
            if (!d) {
                return undefined
            }
            for (var e in f) {
                d.setAttribute(e, f[e])
            }
            this._activeParent().appendChild(d);
            return d
        },
        line: function(f, i, e, h, j) {
            var c = "M " + f + "," + i + " L " + e + "," + h + " X E";
            var d = this.path(c);
            this.attr(d, j);
            return d
        },
        _createElementMarkup: function(c) {
            var d = "<v:" + c + ' style=""></v:' + c + ">";
            if (this._ie8mode) {
                d = d.replace('style=""', 'style="behavior: url(#default#VML);"')
            }
            return d
        },
        path: function(d, e) {
            var c = document.createElement(this._createElementMarkup("shape"));
            c.style.position = "absolute";
            c.coordsize = this._width + " " + this._height;
            c.coordorigin = "0 0";
            c.style.width = parseInt(this._width);
            c.style.height = parseInt(this._height);
            c.style.left = 0 + "px";
            c.style.top = 0 + "px";
            c.setAttribute("path", d);
            this._activeParent().appendChild(c);
            if (e) {
                this.attr(c, e)
            }
            return c
        },
        rect: function(c, j, d, e, i) {
            c = a.jqx._ptrnd(c);
            j = a.jqx._ptrnd(j);
            d = a.jqx._rup(d);
            e = a.jqx._rup(e);
            var f = this.shape("rect", i);
            f.style.position = "absolute";
            f.style.left = c;
            f.style.top = j;
            f.style.width = d;
            f.style.height = e;
            f.strokeweight = 0;
            if (i) {
                this.attr(f, i)
            }
            return f
        },
        circle: function(c, h, e, f) {
            var d = this.shape("oval");
            c = a.jqx._ptrnd(c - e);
            h = a.jqx._ptrnd(h - e);
            e = a.jqx._rup(e);
            d.style.position = "absolute";
            d.style.left = c;
            d.style.top = h;
            d.style.width = e * 2;
            d.style.height = e * 2;
            if (f) {
                this.attr(d, f)
            }
            return d
        },
        updateCircle: function(e, c, f, d) {
            if (c == undefined) {
                c = parseFloat(e.style.left) + parseFloat(e.style.width) / 2
            }
            if (f == undefined) {
                f = parseFloat(e.style.top) + parseFloat(e.style.height) / 2
            }
            if (d == undefined) {
                d = parseFloat(e.width) / 2
            }
            c = a.jqx._ptrnd(c - d);
            f = a.jqx._ptrnd(f - d);
            d = a.jqx._rup(d);
            e.style.left = c;
            e.style.top = f;
            e.style.width = d * 2;
            e.style.height = d * 2
        },
        pieSlicePath: function(m, l, j, u, E, F, e) {
            if (!u) {
                u = 1
            }
            var o = Math.abs(E - F);
            var s = o > 180 ? 1 : 0;
            if (o > 360) {
                E = 0;
                F = 360
            }
            var t = E * Math.PI * 2 / 360;
            var k = F * Math.PI * 2 / 360;
            var B = m,
                A = m,
                h = l,
                f = l;
            var p = !isNaN(j) && j > 0;
            if (p) {
                e = 0
            }
            if (e > 0) {
                var n = o / 2 + E;
                var D = n * Math.PI * 2 / 360;
                m += e * Math.cos(D);
                l -= e * Math.sin(D)
            }
            if (p) {
                var z = j;
                B = a.jqx._ptrnd(m + z * Math.cos(t));
                h = a.jqx._ptrnd(l - z * Math.sin(t));
                A = a.jqx._ptrnd(m + z * Math.cos(k));
                f = a.jqx._ptrnd(l - z * Math.sin(k))
            }
            var w = a.jqx._ptrnd(m + u * Math.cos(t));
            var v = a.jqx._ptrnd(m + u * Math.cos(k));
            var d = a.jqx._ptrnd(l - u * Math.sin(t));
            var c = a.jqx._ptrnd(l - u * Math.sin(k));
            u = a.jqx._ptrnd(u);
            j = a.jqx._ptrnd(j);
            m = a.jqx._ptrnd(m);
            l = a.jqx._ptrnd(l);
            var i = Math.round(E * 65535);
            var C = Math.round((F - E) * 65536);
            if (j < 0) {
                j = 1
            }
            var q = "";
            if (p) {
                q = "M" + B + " " + h;
                q += " AE " + m + " " + l + " " + j + " " + j + " " + i + " " + C;
                q += " L " + v + " " + c;
                i = Math.round((E - F) * 65535);
                C = Math.round(F * 65536);
                q += " AE " + m + " " + l + " " + u + " " + u + " " + C + " " + i;
                q += " L " + B + " " + h
            } else {
                q = "M" + m + " " + l;
                q += " AE " + m + " " + l + " " + u + " " + u + " " + i + " " + C
            }
            q += " X E";
            return q
        },
        pieslice: function(m, k, j, f, i, c, l, e) {
            var h = this.pieSlicePath(m, k, j, f, i, c, l);
            var d = this.path(h, e);
            if (e) {
                this.attr(d, e)
            }
            return d
        },
        _keymap: [{
            svg: "fill",
            vml: "fillcolor"
        }, {
            svg: "stroke",
            vml: "strokecolor"
        }, {
            svg: "stroke-width",
            vml: "strokeweight"
        }, {
            svg: "stroke-dasharray",
            vml: "dashstyle"
        }, {
            svg: "fill-opacity",
            vml: "fillopacity"
        }, {
            svg: "stroke-opacity",
            vml: "strokeopacity"
        }, {
            svg: "opacity",
            vml: "opacity"
        }, {
            svg: "cx",
            vml: "style.left"
        }, {
            svg: "cy",
            vml: "style.top"
        }, {
            svg: "height",
            vml: "style.height"
        }, {
            svg: "width",
            vml: "style.width"
        }, {
            svg: "x",
            vml: "style.left"
        }, {
            svg: "y",
            vml: "style.top"
        }, {
            svg: "d",
            vml: "v"
        }, {
            svg: "display",
            vml: "style.display"
        }],
        _translateParam: function(c) {
            for (var d in this._keymap) {
                if (this._keymap[d].svg == c) {
                    return this._keymap[d].vml
                }
            }
            return c
        },
        attr: function(d, f) {
            if (!d || !f) {
                return
            }
            for (var e in f) {
                var c = this._translateParam(e);
                if (c == "fillcolor" && f[e].indexOf("grd") != -1) {
                    d.type = f[e]
                } else {
                    if (c == "fillcolor" && f[e] == "transparent") {
                        d.style.filter = "alpha(opacity=0)";
                        d["-ms-filter"] = "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)"
                    } else {
                        if (c == "opacity" || c == "fillopacity") {
                            if (d.fill) {
                                d.fill.opacity = f[e]
                            }
                        } else {
                            if (c == "textContent") {
                                d.children[0].innerText = f[e]
                            } else {
                                if (c == "dashstyle") {
                                    d.dashstyle = f[e].replace(",", " ")
                                } else {
                                    if (c.indexOf("style.") == -1) {
                                        d[c] = f[e]
                                    } else {
                                        d.style[c.replace("style.", "")] = f[e]
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },
        getAttr: function(e, d) {
            var c = this._translateParam(d);
            if (c == "opacity" || c == "fillopacity") {
                if (e.fill) {
                    return e.fill.opacity
                } else {
                    return 1
                }
            }
            if (c.indexOf("style.") == -1) {
                return e[c]
            }
            return e.style[c.replace("style.", "")]
        },
        _gradients: {},
        _toRadialGradient: function(c, e, d) {
            return c
        },
        _toLinearGradient: function(i, k, l) {
            if (this._ie8mode) {
                return i
            }
            var e = "grd" + i.replace("#", "") + (k ? "v" : "h");
            var f = "#" + e + "";
            if (this._gradients[f]) {
                return f
            }
            var h = document.createElement(this._createElementMarkup("fill"));
            h.type = "gradient";
            h.method = "linear";
            h.angle = k ? 0 : 90;
            var d = "";
            for (var j in l) {
                if (j > 0) {
                    d += ", "
                }
                d += l[j][0] + "% " + a.jqx.adjustColor(i, l[j][1])
            }
            h.colors = d;
            var c = document.createElement(this._createElementMarkup("shapetype"));
            c.appendChild(h);
            c.id = e;
            this.canvas.appendChild(c);
            return f
        }
    };
    a.jqx.HTML5Renderer = function() {};
    a.jqx.ptrnd = function(d) {
        if (Math.abs(Math.round(d) - d) == 0.5) {
            return d
        }
        var c = Math.round(d);
        if (c < d) {
            c = c - 1
        }
        return c + 0.5
    };
    a.jqx.HTML5Renderer.prototype = {
        _elements: {},
        init: function(c) {
            try {
                this.host = c;
                this.host.append("<canvas id='__jqxCanvasWrap' style='width:100%; height: 100%;'/>");
                this.canvas = c.find("#__jqxCanvasWrap");
                this.canvas[0].width = c.width();
                this.canvas[0].height = c.height();
                this.ctx = this.canvas[0].getContext("2d")
            } catch (d) {
                return false
            }
            return true
        },
        getContainer: function() {
            if (this.canvas && this.canvas.length == 1) {
                return this.canvas
            }
            return undefined
        },
        getRect: function() {
            return {
                x: 0,
                y: 0,
                width: this.canvas[0].width - 1,
                height: this.canvas[0].height - 1
            }
        },
        beginGroup: function() {},
        endGroup: function() {},
        setClip: function() {},
        createClipRect: function(c) {},
        addHandler: function(c, e, d) {},
        removeHandler: function(c, e, d) {},
        on: function(c, e, d) {
            this.addHandler(c, e, d)
        },
        off: function(c, e, d) {
            this.removeHandler(c, e, d)
        },
        clear: function() {
            this._elements = {};
            this._maxId = 0;
            this._renderers._gradients = {};
            this._gradientId = 0
        },
        removeElement: function(c) {
            if (undefined == c) {
                return
            }
            if (this._elements[c.id]) {
                delete this._elements[c.id]
            }
        },
        _maxId: 0,
        shape: function(c, f) {
            var d = {
                type: c,
                id: this._maxId++
            };
            for (var e in f) {
                d[e] = f[e]
            }
            this._elements[d.id] = d;
            return d
        },
        attr: function(c, e) {
            for (var d in e) {
                c[d] = e[d]
            }
        },
        rect: function(c, j, d, f, i) {
            if (isNaN(c)) {
                throw 'Invalid value for "x"'
            }
            if (isNaN(j)) {
                throw 'Invalid value for "y"'
            }
            if (isNaN(d)) {
                throw 'Invalid value for "width"'
            }
            if (isNaN(f)) {
                throw 'Invalid value for "height"'
            }
            var e = this.shape("rect", {
                x: c,
                y: j,
                width: d,
                height: f
            });
            if (i) {
                this.attr(e, i)
            }
            return e
        },
        path: function(c, e) {
            var d = this.shape("path", e);
            this.attr(d, {
                d: c
            });
            return d
        },
        line: function(d, f, c, e, h) {
            return this.path("M " + d + "," + f + " L " + c + "," + e, h)
        },
        circle: function(c, h, e, f) {
            var d = this.shape("circle", {
                x: c,
                y: h,
                r: e
            });
            if (f) {
                this.attr(d, f)
            }
            return d
        },
        pieSlicePath: function(d, j, i, f, h, e, c) {
            return a.jqx.commonRenderer.pieSlicePath(d, j, i, f, h, e, c)
        },
        pieslice: function(l, j, i, f, h, c, k, d) {
            var e = this.path(this.pieSlicePath(l, j, i, f, h, c, k), d);
            this.attr(e, {
                x: l,
                y: j,
                innerRadius: i,
                outerRadius: f,
                angleFrom: h,
                angleTo: c
            });
            return e
        },
        _getCSSStyle: function(d) {
            var k = document.styleSheets;
            try {
                for (var f = 0; f < k.length; f++) {
                    for (var c = 0; k[f].cssRules && c < k[f].cssRules.length; c++) {
                        if (k[f].cssRules[c].selectorText.indexOf(d) != -1) {
                            return k[f].cssRules[c].style
                        }
                    }
                }
            } catch (h) {}
            return {}
        },
        _getTextParts: function(s, h, j) {
            var n = "Arial";
            var t = "10pt";
            var o = "";
            if (j && j["class"]) {
                var c = this._getCSSStyle(j["class"]);
                if (c.fontSize) {
                    t = c.fontSize
                }
                if (c.fontFamily) {
                    n = c.fontFamily
                }
                if (c.fontWeight) {
                    o = c.fontWeight
                }
            }
            this.ctx.font = o + " " + t + " " + n;
            var f = {
                width: 0,
                height: 0,
                parts: []
            };
            var m = 0.6;
            var q = s.toString().split("<br>");
            for (var k = 0; k < q.length; k++) {
                var e = q[k];
                var l = this.ctx.measureText(e).width;
                var p = document.createElement("span");
                p.font = this.ctx.font;
                p.textContent = e;
                document.body.appendChild(p);
                var d = p.offsetHeight * m;
                document.body.removeChild(p);
                f.width = Math.max(f.width, a.jqx._rup(l));
                f.height += d + (k > 0 ? 4 : 0);
                f.parts.push({
                    width: l,
                    height: d,
                    text: e
                })
            }
            return f
        },
        _measureText: function(f, e, d, c) {
            return a.jqx.commonRenderer.measureText(f, e, d, c, this)
        },
        measureText: function(e, d, c) {
            return this._measureText(e, d, c, false)
        },
        text: function(o, n, l, d, p, h, i, e, j, m, f) {
            var q = this.shape("text", {
                text: o,
                x: n,
                y: l,
                width: d,
                height: p,
                angle: h,
                clip: e,
                halign: j,
                valign: m,
                rotateAround: f
            });
            if (i) {
                this.attr(q, i)
            }
            q.fontFamily = "Arial";
            q.fontSize = "10pt";
            q.fontWeight = "";
            q.color = "#000000";
            if (i && i["class"]) {
                var c = this._getCSSStyle(i["class"]);
                q.fontFamily = c.fontFamily || q.fontFamily;
                q.fontSize = c.fontSize || q.fontSize;
                q.fontWeight = c.fontWeight || q.fontWeight;
                q.color = c.color || q.color
            }
            var k = this._measureText(o, 0, i, true);
            this.attr(q, {
                textPartsInfo: k.textPartsInfo,
                textWidth: k.width,
                textHeight: k.height
            });
            if (d <= 0 || isNaN(d)) {
                this.attr(q, {
                    width: k.width
                })
            }
            if (p <= 0 || isNaN(p)) {
                this.attr(q, {
                    height: k.height
                })
            }
            return q
        },
        _toLinearGradient: function(d, j, h) {
            if (this._renderers._gradients[d]) {
                return d
            }
            var c = [];
            for (var f = 0; f < h.length; f++) {
                c.push({
                    percent: h[f][0] / 100,
                    color: a.jqx.adjustColor(d, h[f][1])
                })
            }
            var e = "gr" + this._gradientId++;
            this.createGradient(e, j ? "vertical" : "horizontal", c);
            return e
        },
        _toRadialGradient: function(d, h) {
            if (this._renderers._gradients[d]) {
                return d
            }
            var c = [];
            for (var f = 0; f < h.length; f++) {
                c.push({
                    percent: h[f][0] / 100,
                    color: a.jqx.adjustColor(d, h[f][1])
                })
            }
            var e = "gr" + this._gradientId++;
            this.createGradient(e, "radial", c);
            return e
        },
        _gradientId: 0,
        createGradient: function(e, d, c) {
            this._renderers.createGradient(e, d, c)
        },
        _renderers: {
            _gradients: {},
            createGradient: function(e, d, c) {
                this._gradients[e] = {
                    orientation: d,
                    colorStops: c
                }
            },
            setStroke: function(c, d) {
                c.strokeStyle = d.stroke || "transparent";
                c.lineWidth = d["stroke-width"] || 1;
                if (d["fill-opacity"]) {
                    c.globalAlpha = d["fill-opacity"]
                } else {
                    c.globalAlpha = 1
                }
                if (c.setLineDash) {
                    if (d["stroke-dasharray"]) {
                        c.setLineDash(d["stroke-dasharray"].split(","))
                    } else {
                        c.setLineDash([])
                    }
                }
            },
            setFillStyle: function(o, f) {
                o.fillStyle = "transparent";
                if (f["fill-opacity"]) {
                    o.globalAlpha = f["fill-opacity"]
                } else {
                    o.globalAlpha = 1
                }
                if (f.fill && f.fill.indexOf("#") == -1 && this._gradients[f.fill]) {
                    var m = this._gradients[f.fill].orientation != "horizontal";
                    var j = this._gradients[f.fill].orientation == "radial";
                    var d = a.jqx.ptrnd(f.x);
                    var n = a.jqx.ptrnd(f.y);
                    var c = a.jqx.ptrnd(f.x + (m ? 0 : f.width));
                    var k = a.jqx.ptrnd(f.y + (m ? f.height : 0));
                    var l;
                    if ((f.type == "circle" || f.type == "path") && j) {
                        x = a.jqx.ptrnd(f.x);
                        y = a.jqx.ptrnd(f.y);
                        r1 = f.innerRadius || 0;
                        r2 = f.outerRadius || f.r || 0;
                        l = o.createRadialGradient(x, y, r1, x, y, r2)
                    }
                    if (!j) {
                        if (isNaN(d) || isNaN(c) || isNaN(n) || isNaN(k)) {
                            d = 0;
                            n = 0;
                            c = m ? 0 : o.canvas.width;
                            k = m ? o.canvas.height : 0
                        }
                        l = o.createLinearGradient(d, n, c, k)
                    }
                    var e = this._gradients[f.fill].colorStops;
                    for (var h = 0; h < e.length; h++) {
                        l.addColorStop(e[h].percent, e[h].color)
                    }
                    o.fillStyle = l
                } else {
                    if (f.fill) {
                        o.fillStyle = f.fill
                    }
                }
            },
            rect: function(c, d) {
                if (d.width == 0 || d.height == 0) {
                    return
                }
                c.fillRect(a.jqx.ptrnd(d.x), a.jqx.ptrnd(d.y), d.width, d.height);
                c.strokeRect(a.jqx.ptrnd(d.x), a.jqx.ptrnd(d.y), d.width, d.height)
            },
            circle: function(c, d) {
                if (d.r == 0) {
                    return
                }
                c.beginPath();
                c.arc(a.jqx.ptrnd(d.x), a.jqx.ptrnd(d.y), d.r, 0, Math.PI * 2, false);
                c.closePath();
                c.fill();
                c.stroke()
            },
            _parsePoint: function(d) {
                var c = this._parseNumber(d);
                var e = this._parseNumber(d);
                return ({
                    x: c,
                    y: e
                })
            },
            _parseNumber: function(e) {
                var f = false;
                for (var c = this._pos; c < e.length; c++) {
                    if ((e[c] >= "0" && e[c] <= "9") || e[c] == "." || (e[c] == "-" && !f)) {
                        f = true;
                        continue
                    }
                    if (!f && (e[c] == " " || e[c] == ",")) {
                        this._pos++;
                        continue
                    }
                    break
                }
                var d = parseFloat(e.substring(this._pos, c));
                if (isNaN(d)) {
                    return undefined
                }
                this._pos = c;
                return d
            },
            _pos: 0,
            _cmds: "mlcaz",
            _lastCmd: "",
            _isRelativeCmd: function(c) {
                return a.jqx.string.contains(this._cmds, c)
            },
            _parseCmd: function(c) {
                for (var d = this._pos; d < c.length; d++) {
                    if (a.jqx.string.containsIgnoreCase(this._cmds, c[d])) {
                        this._pos = d + 1;
                        this._lastCmd = c[d];
                        return this._lastCmd
                    }
                    if (c[d] == " ") {
                        this._pos++;
                        continue
                    }
                    if (c[d] >= "0" && c[d] <= "9") {
                        this._pos = d;
                        if (this._lastCmd == "") {
                            break
                        } else {
                            return this._lastCmd
                        }
                    }
                }
                return undefined
            },
            _toAbsolutePoint: function(c) {
                return {
                    x: this._currentPoint.x + c.x,
                    y: this._currentPoint.y + c.y
                }
            },
            _currentPoint: {
                x: 0,
                y: 0
            },
            path: function(E, N) {
                var B = N.d;
                this._pos = 0;
                this._lastCmd = "";
                var n = undefined;
                this._currentPoint = {
                    x: 0,
                    y: 0
                };
                E.beginPath();
                var I = 0;
                while (this._pos < B.length) {
                    var H = this._parseCmd(B);
                    if (H == undefined) {
                        break
                    }
                    if (H == "M" || H == "m") {
                        var F = this._parsePoint(B);
                        if (F == undefined) {
                            break
                        }
                        E.moveTo(F.x, F.y);
                        this._currentPoint = F;
                        if (n == undefined) {
                            n = F
                        }
                        continue
                    }
                    if (H == "L" || H == "l") {
                        var F = this._parsePoint(B);
                        if (F == undefined) {
                            break
                        }
                        E.lineTo(F.x, F.y);
                        this._currentPoint = F;
                        continue
                    }
                    if (H == "A" || H == "a") {
                        var j = this._parseNumber(B);
                        var h = this._parseNumber(B);
                        var L = this._parseNumber(B) * (Math.PI / 180);
                        var P = this._parseNumber(B);
                        var f = this._parseNumber(B);
                        var q = this._parsePoint(B);
                        if (this._isRelativeCmd(H)) {
                            q = this._toAbsolutePoint(q)
                        }
                        if (j == 0 || h == 0) {
                            continue
                        }
                        var k = this._currentPoint;
                        var K = {
                            x: Math.cos(L) * (k.x - q.x) / 2 + Math.sin(L) * (k.y - q.y) / 2,
                            y: -Math.sin(L) * (k.x - q.x) / 2 + Math.cos(L) * (k.y - q.y) / 2
                        };
                        var l = Math.pow(K.x, 2) / Math.pow(j, 2) + Math.pow(K.y, 2) / Math.pow(h, 2);
                        if (l > 1) {
                            j *= Math.sqrt(l);
                            h *= Math.sqrt(l)
                        }
                        var t = (P == f ? -1 : 1) * Math.sqrt(((Math.pow(j, 2) * Math.pow(h, 2)) - (Math.pow(j, 2) * Math.pow(K.y, 2)) - (Math.pow(h, 2) * Math.pow(K.x, 2))) / (Math.pow(j, 2) * Math.pow(K.y, 2) + Math.pow(h, 2) * Math.pow(K.x, 2)));
                        if (isNaN(t)) {
                            t = 0
                        }
                        var J = {
                            x: t * j * K.y / h,
                            y: t * -h * K.x / j
                        };
                        var D = {
                            x: (k.x + q.x) / 2 + Math.cos(L) * J.x - Math.sin(L) * J.y,
                            y: (k.y + q.y) / 2 + Math.sin(L) * J.x + Math.cos(L) * J.y
                        };
                        var C = function(i) {
                            return Math.sqrt(Math.pow(i[0], 2) + Math.pow(i[1], 2))
                        };
                        var z = function(m, i) {
                            return (m[0] * i[0] + m[1] * i[1]) / (C(m) * C(i))
                        };
                        var O = function(m, i) {
                            return (m[0] * i[1] < m[1] * i[0] ? -1 : 1) * Math.acos(z(m, i))
                        };
                        var G = O([1, 0], [(K.x - J.x) / j, (K.y - J.y) / h]);
                        var p = [(K.x - J.x) / j, (K.y - J.y) / h];
                        var o = [(-K.x - J.x) / j, (-K.y - J.y) / h];
                        var M = O(p, o);
                        if (z(p, o) <= -1) {
                            M = Math.PI
                        }
                        if (z(p, o) >= 1) {
                            M = 0
                        }
                        if (f == 0 && M > 0) {
                            M = M - 2 * Math.PI
                        }
                        if (f == 1 && M < 0) {
                            M = M + 2 * Math.PI
                        }
                        var z = (j > h) ? j : h;
                        var A = (j > h) ? 1 : j / h;
                        var w = (j > h) ? h / j : 1;
                        E.translate(D.x, D.y);
                        E.rotate(L);
                        E.scale(A, w);
                        E.arc(0, 0, z, G, G + M, 1 - f);
                        E.scale(1 / A, 1 / w);
                        E.rotate(-L);
                        E.translate(-D.x, -D.y);
                        continue
                    }
                    if ((H == "Z" || H == "z") && n != undefined) {
                        E.lineTo(n.x, n.y);
                        this._currentPoint = n;
                        continue
                    }
                    if (H == "C" || H == "c") {
                        var e = this._parsePoint(B);
                        var d = this._parsePoint(B);
                        var c = this._parsePoint(B);
                        E.bezierCurveTo(e.x, e.y, d.x, d.y, c.x, c.y);
                        this._currentPoint = c;
                        continue
                    }
                }
                E.fill();
                E.stroke();
                E.closePath()
            },
            text: function(A, G) {
                var p = a.jqx.ptrnd(G.x);
                var o = a.jqx.ptrnd(G.y);
                var v = a.jqx.ptrnd(G.width);
                var t = a.jqx.ptrnd(G.height);
                var s = G.halign;
                var k = G.valign;
                var D = G.angle;
                var c = G.rotateAround;
                var f = G.textPartsInfo;
                var e = f.parts;
                var E = G.clip;
                if (E == undefined) {
                    E = true
                }
                A.save();
                if (!s) {
                    s = "center"
                }
                if (!k) {
                    k = "center"
                }
                if (E) {
                    A.rect(p, o, v, t);
                    A.clip()
                }
                var H = G.textWidth;
                var l = G.textHeight;
                var q = v || 0;
                var C = t || 0;
                A.fillStyle = G.color;
                A.font = G.fontWeight + " " + G.fontSize + " " + G.fontFamily;
                if (!D || D == 0) {
                    o += l;
                    if (k == "center" || k == "middle") {
                        o += (C - l) / 2
                    } else {
                        if (k == "bottom") {
                            o += C - l
                        }
                    }
                    if (!v) {
                        v = H
                    }
                    if (!t) {
                        t = l
                    }
                    var n = 0;
                    for (var B = e.length - 1; B >= 0; B--) {
                        var u = e[B];
                        var I = p;
                        var m = e[B].width;
                        var d = e[B].height;
                        if (s == "center") {
                            I += (q - m) / 2
                        } else {
                            if (s == "right") {
                                I += (q - m)
                            }
                        }
                        A.fillText(u.text, I, o + n);
                        n -= u.height + (B > 0 ? 4 : 0)
                    }
                    A.restore();
                    return
                }
                var z = a.jqx.commonRenderer.alignTextInRect(p, o, v, t, H, l, s, k, D, c);
                p = z.x;
                o = z.y;
                var j = D * Math.PI * 2 / 360;
                A.translate(p, o);
                A.rotate(j);
                var n = 0;
                var F = f.width;
                for (var B = e.length - 1; B >= 0; B--) {
                    var I = 0;
                    if (s == "center") {
                        I += (F - e[B].width) / 2
                    } else {
                        if (s == "right") {
                            I += (F - e[B].width)
                        }
                    }
                    A.fillText(e[B].text, I, n);
                    n -= e[B].height + 4
                }
                A.restore()
            }
        },
        refresh: function() {
            this.ctx.clearRect(0, 0, this.canvas[0].width, this.canvas[0].height);
            for (var c in this._elements) {
                var d = this._elements[c];
                this._renderers.setFillStyle(this.ctx, d);
                this._renderers.setStroke(this.ctx, d);
                this._renderers[this._elements[c].type](this.ctx, d)
            }
        }
    };
    a.jqx.createRenderer = function(c, e) {
        var d = c;
        var f = d.renderer = null;
        if (document.createElementNS && (d.renderEngine != "HTML5" && d.renderEngine != "VML")) {
            f = new a.jqx.svgRenderer();
            if (!f.init(e)) {
                if (d.renderEngine == "SVG") {
                    throw "Your browser does not support SVG"
                }
                return null
            }
        }
        if (f == null && d.renderEngine != "HTML5") {
            f = new a.jqx.vmlRenderer();
            if (!f.init(e)) {
                if (d.renderEngine == "VML") {
                    throw "Your browser does not support VML"
                }
                return null
            }
            d._isVML = true
        }
        if (f == null && (d.renderEngine == "HTML5" || d.renderEngine == undefined)) {
            f = new a.jqx.HTML5Renderer();
            if (!f.init(e)) {
                throw "Your browser does not support HTML5 Canvas"
            }
        }
        d.renderer = f;
        return f
    }, a.jqx._widgetToImage = function(o, j, f, m, h) {
        var k = o;
        if (!k) {
            return false
        }
        if (f == undefined || f == "") {
            f = "image." + j
        }
        var l = k.renderEngine;
        var d = k.enableAnimations;
        k.enableAnimations = false;
        k.renderEngine = "HTML5";
        if (k.renderEngine != l) {
            try {
                k.refresh()
            } catch (i) {
                k.renderEngine = l;
                k.refresh();
                k.enableAnimations = d;
                return false
            }
        }
        var c = k.renderer.getContainer()[0];
        var n = a.jqx.exportImage(c, j, f, m, h);
        if (k.renderEngine != l) {
            k.renderEngine = l;
            k.refresh();
            k.enableAnimations = d
        }
        return n
    };
    a.jqx.exportImage = function(f, l, h, m, j) {
        if (!f) {
            return false
        }
        if (h == undefined || h == "") {
            h = "image." + l
        }
        if (m == undefined || m == "") {
            throw "Please specifiy export server"
        }
        var o = true;
        try {
            if (f) {
                var i = f.toDataURL("image/" + l);
                i = i.replace("data:image/" + l + ";base64,", "");
                if (j) {
                    a.ajax({
                        dataType: "string",
                        url: m,
                        type: "POST",
                        data: {
                            content: i,
                            fname: h
                        },
                        async: false,
                        success: function(p, e, q) {
                            o = true
                        },
                        error: function(p, e, q) {
                            o = false
                        }
                    })
                } else {
                    var d = document.createElement("form");
                    d.method = "POST";
                    d.action = m;
                    d.style.display = "none";
                    document.body.appendChild(d);
                    var n = document.createElement("input");
                    n.name = "fname";
                    n.value = h;
                    n.style.display = "none";
                    var c = document.createElement("input");
                    c.name = "content";
                    c.value = i;
                    c.style.display = "none";
                    d.appendChild(n);
                    d.appendChild(c);
                    d.submit();
                    document.body.removeChild(d);
                    o = true
                }
            }
        } catch (k) {
            o = false
        }
        return o
    }
})(jqxBaseFramework);