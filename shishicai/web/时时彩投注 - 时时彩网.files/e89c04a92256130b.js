(function (b, c, d) {
    if ("BackCompat" === document.compatMode) {
        throw Error("artDialog: Document types require more than xhtml1.0")
    }
    var f, j = 0, i = "artDialog" + +new Date, r = c.VBArray && !c.XMLHttpRequest, o = "createTouch" in document && !("onmousemove" in document) || /(iPhone|iPad|iPod)/i.test(navigator.userAgent), g = !r && !o, a = function (e, h, k) {
        e = e || {};
        if ("string" === typeof e || 1 === e.nodeType) {
            e = {
                content: e, fixed: !o
            }

        }
        var l;
        l = a.defaults;
        var m = e.follow = 1 === this.nodeType && this || e.follow, n;
        for (n in l) {
            e[n] === d && (e[n] = l[n])
        }
        e.id = m && m[i + "follow"] || e.id || i + j;
        if (l = a.list[e.id]) {
            return m && l.follow(m), l.zIndex().focus(), l
        }
        if (!g) {
            e.fixed = !1
        }
        if (!e.button || !e.button.push) {
            e.button = []
        }
        if (h !== d) {
            e.ok = h
        }
        e.ok && e.button.push({
            id: "ok", value: e.okValue, callback: e.ok, focus: !0
        });
        if (k !== d) {
            e.cancel = k
        }
        e.cancel && e.button.push({
            id: "cancel", value: e.cancelValue, callback: e.cancel
        });
        a.defaults.zIndex = e.zIndex;
        j++;
        return a.list[e.id] = f ? f.constructor(e) : new a.fn.constructor(e)
    };
    a.version = "5.0";
    a.fn = a.prototype = {
        constructor: function (e) {
            var h;
            this.closed = !1;
            this.config = e;
            this.dom = h = this.dom || this._getDom();
            e.skin && h.wrap.addClass(e.skin);
            h.wrap.css("position", e.fixed ? "fixed" : "absolute");
            h.close[!1 === e.cancel ? "hide" : "show"]();
            h.content.css("padding", e.padding);
            this.button.apply(this, e.button);
            this.title(e.title).content(e.content).size(e.width, e.height).time(e.time);
            e.follow ? this.follow(e.follow) : this.position();
            this.zIndex();
            e.lock && this.lock();
            this._addEvent();
            this[e.visible ? "visible" : "hidden"]().focus();
            f = null;
            e.initialize && e.initialize.call(this);
            return this
        }
        , content: function (h) {
            var k, l, m, p, q = this, n = this.dom.content, s = n[0];
            this._elemBack && (this._elemBack(), delete this._elemBack);
            if ("string" === typeof h) {
                n.html(h)
            }
            else {
                if (h && 1 === h.nodeType) {
                    p = h.style.display, k = h.previousSibling, l = h.nextSibling, m = h.parentNode, this._elemBack = function () {
                        k && k.parentNode ? k.parentNode.insertBefore(h, k.nextSibling) : l && l.parentNode ? l.parentNode.insertBefore(h, l) : m && m.appendChild(h);
                        h.style.display = p;
                        q._elemBack = null
                    }
                    , n.html(""), s.appendChild(h), b(h).show()
                }

            }
            return this.position()
        }
        , title: function (e) {
            var h = this.dom, k = h.outer, h = h.title;
            !1 === e ? (h.hide().html(""), k.addClass("d-state-noTitle")) : (h.show().html(e), k.removeClass("d-state-noTitle"));
            return this
        }
        , position: function () {
            var k = this.dom, l = k.wrap[0], m = k.window, n = k.document, q = this.config.fixed, k = q ? 0 : n.scrollLeft(), n = q ? 0 : n.scrollTop(), q = m.width(), p = m.height(), s = l.offsetHeight, m = (q - l.offsetWidth) / 2 + k, q = q = (s < 4 * p / 7 ? 0.382 * p - s / 2 : (p - s) / 2) + n, l = l.style;
            l.left = Math.max(m, k) + "px";
            l.top = Math.max(q, n) + "px";
            return this
        }
        , size: function (e, h) {
            var k = this.dom.main[0].style;
            "number" === typeof e && (e += "px");
            "number" === typeof h && (h += "px");
            k.width = e;
            k.height = h;
            return this
        }
        , follow: function (h) {
            var p = b(h), t = this.config;
            if (!h || !h.offsetWidth && !h.offsetHeight) {
                return this.position(this._left, this._top)
            }
            var u = t.fixed, v = i + "follow", w = this.dom, G = w.window, y = w.document, w = G.width(), G = G.height(), F = y.scrollLeft(), y = y.scrollTop(), x = p.offset(), p = h.offsetWidth, z = u ? x.left - F : x.left, x = u ? x.top - y : x.top, D = this.dom.wrap[0], B = D.style, A = D.offsetWidth, D = D.offsetHeight, C = z - (A - p) / 2, E = x + h.offsetHeight, F = u ? 0 : F, u = u ? 0 : y;
            B.left = (C < F ? z : C + A > w && z - A > F ? z - A + p : C) + "px";
            B.top = (E + D > G + u && x - D > u ? x - D : E) + "px";
            this._follow && this._follow.removeAttribute(v);
            this._follow = h;
            h[v] = t.id;
            return this
        }
        , button: function () {
            for (var h = this.dom.buttons, m = h[0], n = this._listeners = this._listeners || {}, p = [].slice.call(arguments), q = 0, s, v, u, w, t;
            q < p.length;
            q++) {
                s = p[q];
                v = s.value;
                u = s.id || v;
                w = !n[u];
                t = !w ? n[u].elem : document.createElement("input");
                t.type = "button";
                t.className = "d-button";
                n[u] || (n[u] = {});
                if (v) {
                    t.value = v
                }
                if (s.width) {
                    t.style.width = s.width
                }
                if (s.callback) {
                    n[u].callback = s.callback
                }
                if (s.focus) {
                    this._focus && this._focus.removeClass("d-state-highlight"), this._focus = b(t).addClass("d-state-highlight"), this.focus()
                }
                t[i + "callback"] = u;
                t.disabled = !!s.disabled;
                if (w) {
                    n[u].elem = t, m.appendChild(t)
                }

            }
            h[0].style.display = p.length ? "" : "none";
            return this
        }
        , visible: function () {
            this.dom.wrap.css("visibility", "visible");
            this.dom.outer.addClass("d-state-visible");
            this._isLock && this._lockMask.show();
            return this
        }
        , hidden: function () {
            this.dom.wrap.css("visibility", "hidden");
            this.dom.outer.removeClass("d-state-visible");
            this._isLock && this._lockMask.hide();
            return this
        }
        , close: function () {
            if (this.closed) {
                return this
            }
            var e = this.dom, h = e.wrap, k = a.list, l = this.config.beforeunload, m = this.config.follow;
            if (l && !1 === l.call(this)) {
                return this
            }
            if (a.focus === this) {
                a.focus = null
            }
            m && m.removeAttribute(i + "follow");
            this._elemBack && this._elemBack();
            this.time();
            this.unlock();
            this._removeEvent();
            delete k[this.config.id];
            if (f) {
                h.remove()
            }
            else {
                f = this;
                e.title.html("");
                e.content.html("");
                e.buttons.html("");
                h[0].className = h[0].style.cssText = "";
                e.outer[0].className = "d-outer";
                h.css({
                    left: 0, top: 0, position: g ? "fixed" : "absolute"
                });
                for (var n in this) {
                    this.hasOwnProperty(n) && "dom" !== n && delete this[n]
                }
                this.hidden()
            }
            this.closed = !0;
            return this
        }
        , time: function (e) {
            var h = this, k = this._timer;
            k && clearTimeout(k);
            if (e) {
                this._timer = setTimeout(function () {
                    h._click("cancel")
                }
                , e)
            }
            return this
        }
        , focus: function () {
            if (this.config.focus) {
                try {
                    var e = this._focus && this._focus[0] || this.dom.close[0];
                    e && e.focus()
                }
                catch (h) { }
            }
            return this
        }
        , zIndex: function () {
            var e = this.dom, h = a.focus, k = a.defaults.zIndex++;
            e.wrap.css("zIndex", k);
            this._lockMask && this._lockMask.css("zIndex", k - 1);
            h && h.dom.outer.removeClass("d-state-focus");
            a.focus = this;
            e.outer.addClass("d-state-focus");
            return this
        }
        , lock: function () {
            if (this._isLock) {
                return this
            }
            var e = this, h = this.dom, k = document.createElement("div"), l = b(k), m = a.defaults.zIndex - 1;
            this.zIndex();
            h.outer.addClass("d-state-lock");
            l.css({
                zIndex: m, position: "fixed", left: 0, top: 0, width: "100%", height: "100%", overflow: "hidden"
            }).addClass("d-mask");
            g || l.css({
                position: "absolute", width: b(c).width() + "px", height: b(document).height() + "px"
            });
            l.bind("click", function () {
                e._reset()
            }).bind("dblclick", function () {
                e._click("cancel")
            });
            document.body.appendChild(k);
            this._lockMask = l;
            this._isLock = !0;
            return this
        }
        , unlock: function () {
            if (!this._isLock) {
                return this
            }
            this._lockMask.unbind();
            this._lockMask.hide();
            this._lockMask.remove();
            this.dom.outer.removeClass("d-state-lock");
            this._isLock = !1;
            return this
        }
        , _getDom: function () {
            var e = document.body;
            if (!e) {
                throw Error('artDialog: "documents.body" not ready')
            }
            var h = document.createElement("div");
            h.style.cssText = "position:absolute;left:0;top:0";
            h.innerHTML = a._templates;
            e.insertBefore(h, e.firstChild);
            for (var k = 0, l = {}, m = h.getElementsByTagName("*"), n = m.length;
            k < n;
            k++) {
                (e = m[k].className.split("d-")[1]) && (l[e] = b(m[k]))
            }
            l.window = b(c);
            l.document = b(document);
            l.wrap = b(h);
            return l
        }
        , _click: function (e) {
            e = this._listeners[e] && this._listeners[e].callback;
            return "function" !== typeof e || !1 !== e.call(this) ? this.close() : this
        }
        , _reset: function () {
            var e = this.config.follow;
            e ? this.follow(e) : this.position()
        }
        , _addEvent: function () {
            var e = this, h = this.dom;
            h.wrap.bind("click", function (k) {
                k = k.target;
                if (k.disabled) {
                    return !1
                }
                if (k === h.close[0]) {
                    return e._click("cancel"), !1
                }
                (k = k[i + "callback"]) && e._click(k)
            }).bind("mousedown", function () {
                e.zIndex()
            })
        }
        , _removeEvent: function () {
            this.dom.wrap.unbind()
        }

    };
    a.fn.constructor.prototype = a.fn;
    b.fn.dialog = b.fn.artDialog = function () {
        var e = arguments;
        this[this.live ? "live" : "bind"]("click", function () {
            a.apply(this, e);
            return !1
        });
        return this
    };
    a.focus = null;
    a.get = function (e) {
        return e === d ? a.list : a.list[e]
    };
    a.list = {};
    b(document).bind("keydown", function (e) {
        var h = e.target, k = h.nodeName, l = /^input|textarea$/i, m = a.focus, e = e.keyCode;
        m && m.config.esc && !(l.test(k) && "button" !== h.type) && 27 === e && m._click("cancel")
    });
    b(c).bind("resize", function () {
        var e = a.list, h;
        for (h in e) {
            e[h]._reset()
        }

    });
    a._templates = '<div class="d-outer"><table class="d-dialog"><tbody><tr><td class="d-header"><div class="d-titleBar"><div class="d-title"></div><a class="d-close" href="javascript:/*artDialog*/;">\u00d7</a></div></td></tr><tr><td class="d-main"><div class="d-content"></div></td></tr><tr><td class="d-footer"><div class="d-buttons"></div></td></tr></tbody></table></div>';
    a.defaults = {
        content: '<div class="d-loading"><span>loading..</span></div>', title: "message", button: null, ok: null, cancel: null, initialize: null, beforeunload: null, okValue: "ok", cancelValue: "cancel", width: "auto", height: "auto", padding: "15px 15px", skin: null, time: null, esc: !0, focus: !0, visible: !0, follow: null, lock: !1, fixed: !1, zIndex: 1987
    };
    this.artDialog = b.dialog = b.artDialog = a
})(this.art || this.jQuery, this);
(function (a) {
    a.alert = a.dialog.alert = function (i, c) {
        return a.dialog({
            id: "Alert", fixed: !0, lock: !0, content: i, ok: !0, beforeunload: c
        })
    };
    a.confirm = a.dialog.confirm = function (i, c, j) {
        return a.dialog({
            id: "Confirm", fixed: !0, lock: !0, content: i, ok: c, cancel: j
        })
    };
    a.prompt = a.dialog.prompt = function (i, c, k) {
        var j;
        return a.dialog({
            id: "Prompt", fixed: !0, lock: !0, content: ['<div style="margin-bottom:5px;font-size:12px">', i, '</div><div><input type="text" class="d-input-text" value="', k || "", '" style="width:18em;padding:6px 4px" /></div>'].join(""), initialize: function () {
                j = this.dom.content.find(".d-input-text")[0];
                j.select();
                j.focus()
            }
            , ok: function () {
                return c && c.call(this, j.value)
            }
            , cancel: function () { }
        })
    };
    a.dialog.prototype.shake = function () {
        var i = function (j, k, n) {
            var p = +new Date, o = setInterval(function () {
                var q = (+new Date - p) / n;
                1 <= q ? (clearInterval(o), k(q)) : j(q)
            }
            , 13)
        }
        , c = function (j, k, p, q) {
            var n = q;
            void 0 === n && (n = 6, p /= n);
            var o = parseInt(j.style.marginLeft) || 0;
            i(function (r) {
                j.style.marginLeft = o + (k - o) * r + "px"
            }
            , function () {
                0 !== n && c(j, 1 === n ? 0 : 1.3 * (k / n - k), p, --n)
            }
            , p)
        };
        return function () {
            c(this.dom.wrap[0], 40, 600);
            return this
        }

    }
    ();
    var g = function () {
        var i = this, c = function (j) {
            var k = i[j];
            i[j] = function () {
                return k.apply(i, arguments)
            }

        };
        c("start");
        c("over");
        c("end")
    };
    g.prototype = {
        start: function (c) {
            a(document).bind("mousemove", this.over).bind("mouseup", this.end);
            this._sClientX = c.clientX;
            this._sClientY = c.clientY;
            this.onstart(c.clientX, c.clientY);
            return !1
        }
        , over: function (c) {
            this._mClientX = c.clientX;
            this._mClientY = c.clientY;
            this.onover(c.clientX - this._sClientX, c.clientY - this._sClientY);
            return !1
        }
        , end: function (c) {
            a(document).unbind("mousemove", this.over).unbind("mouseup", this.end);
            this.onend(c.clientX, c.clientY);
            return !1
        }

    };
    var d = a(window), e = a(document), b = document.documentElement, h = !!("minWidth" in b.style) && "onlosecapture" in b, l = "setCapture" in b, m = function () {
        return !1
    }
    , f = function (o) {
        var j = new g, p = artDialog.focus, q = p.dom, C = q.wrap, D = q.title, r = C[0], B = D[0], E = q.main[0], F = r.style, H = E.style, I = o.target === q.se[0] ? !0 : !1, J = (q = "fixed" === r.style.position) ? 0 : e.scrollLeft(), K = q ? 0 : e.scrollTop(), G = d.width() - r.offsetWidth + J, k = d.height() - r.offsetHeight + K, L, M, N, O;
        j.onstart = function () {
            I ? (L = E.offsetWidth, M = E.offsetHeight) : (N = r.offsetLeft, O = r.offsetTop);
            e.bind("dblclick", j.end).bind("dragstart", m);
            h ? D.bind("losecapture", j.end) : d.bind("blur", j.end);
            l && B.setCapture();
            C.addClass("d-state-drag");
            p.focus()
        };
        j.onover = function (i, n) {
            if (I) {
                var s = i + L, t = n + M;
                F.width = "auto";
                H.width = Math.max(0, s) + "px";
                F.width = r.offsetWidth + "px";
                H.height = Math.max(0, t) + "px"
            }
            else {
                s = Math.max(J, Math.min(G, i + N)), t = Math.max(K, Math.min(k, n + O)), F.left = s + "px", F.top = t + "px"
            }

        };
        j.onend = function () {
            e.unbind("dblclick", j.end).unbind("dragstart", m);
            h ? D.unbind("losecapture", j.end) : d.unbind("blur", j.end);
            l && B.releaseCapture();
            C.removeClass("d-state-drag")
        };
        j.start(o)
    };
    a(document).bind("mousedown", function (j) {
        var i = artDialog.focus;
        if (i) {
            var k = j.target, n = i.config, i = i.dom;
            if (!1 !== n.drag && k === i.title[0] || !1 !== n.resize && typeof i.se !== "undefined" && k === i.se[0]) {
                return f(j), !1
            }

        }

    })
})(this.art || this.jQuery);
$.artDialog.defaults.lock = true;
$.artDialog.defaults.fixed = true;
$.artDialog.defaults.zIndex = 99999;
$.artDialog.defaults.title = "&nbsp;";
$.artDialog.defaults.okValue = "确定";
$.artDialog.defaults.cancelValue = "取消";
$.artDialog.tkAlert = function (g, f, h, a, b, c) {
    try {
        $.artDialog.get(c).close()
    }
    catch (n) { } var p = "artDialog_tkAlert";
    var m = $.artDialog.get(p);
    var l;
    function k() {
        $.artDialog.get(p).close()
    }
    switch (typeof (b)) {
        case "string": b = [b];
            break;
        case "undefined": b = [$('<a class="Dbutt"><span>确 定</span></a>').click(k)];
            break
    }
    if (!b || (typeof (b.length) == "number" && b.length == 0)) {
        b = [$('<a class="Dbutt"><span>确 定</span></a>').click(k)]
    }
    if (f) {
        if (typeof (g) == "string") {
            g = [g]
        }
        l = $(String.format('<div class="dia_c_main"><div class="teLeft"><p class="tipok">{0}</p></div><div class="cetBtn"></div></div>', g.join("</p><p>")))
    }
    else {
        l = $(String.format('<div class="dia_c_main"><div class="cet"><b class="plaint"></b>{0}</div><div class="cetBtn"></div></div>', (typeof (g) == "string" ? "<span>" + g + "</span>" : "<p>" + g.join("</p><p>") + "</p>")))
    }
    var j = l.find("div.cetBtn");
    for (var o = 0;
    o < b.length;
    o++) {
        j.append(b[o])
    }
    j.find("a.cancel").click(k);
    $.artDialog({
        id: p, title: h || "提示", content: l.get(0), beforeunload: a || function () { }
    })
};
$.artDialog.tkConfirm = function (b, d, a, c) {
    var g = "artDialog_tkAlert";
    var f;
    if (typeof (a) == "function" || typeof (a) == "undefined") {
        a = a || function () { };
        f = $('<a class="Dbutt"><span>确 定</span></a>').click(function () {
            a();
            if (c != true) {
                $.artDialog.get(g).close()
            }

        })
    }
    else {
        if (typeof (a) == "object") {
            f = a
        }

    }
    var e = $('<a class="cancel">取　消</a>');
    $.artDialog.tkAlert(b, null, d, null, [f, e], g)
};
$.artDialog.tkLoading = new function () {
    this.Html = '<div style="position:absolute;"><div style="position:absolute;background-color:#fff;filter:alpha(opacity=30);-moz-opacity:0.3;opacity:0.3;width:100%;"></div><img src="' + TK.Url.site_image + '/media/img/loading.gif" /></div>';
    this.Wrap = $(this.Html);
    this.Opacity = $(this.Wrap).children("div");
    this.Img = $(this.Wrap).children("img");
    this.hide = function () {
        $(this.Wrap).remove()
    };
    this.show = function (b, a) {
        var c = $(b).css("position");
        switch (c) {
            case "relative": case "absolute": break;
            default: $(b).css("position", "relative");
                break
        }
        var f = $(b).height(), g = $(b).width();
        f = f < 100 ? 100 : f;
        $(this.Wrap).css("left", "0").css("top", "0").width($(b).width()).height(f).appendTo(b);
        var d = $(this.Img).height(), e = $(this.Img).width();
        $(this.Img).css("position", "relative").css("top", (f - d) / 2).css("left", (g - e) / 2);
        $(this.Opacity).height(f);
        setTimeout(a || function () { }, 200)
    };
    return this
};
(function () {
    function a() {
        this.enter_event = function () { };
        this.keyup_event = function () { };
        this.change_event = function () { };
        this.change_max = null;
        this.change_min = null;
        this.autoselect = false;
        this.horn_status = 0;
        this.limit_inputKeyCode = false;
        this.limit_inputRegexp = null;
        this.limit_outputRegexp = null;
        this.limit_changeRegexp = null;
        this.limit_length = null
    }
    jQuery.dom = new a();
    $.fn.dom = function (g) {
        var g = $.extend(new a(), g);
        function c(h) {
            var i = h;
            switch (g.horn_status) {
                case -1: i = h.toHorn(false);
                    break;
                case 1: i = h.toHorn(true);
                    break;
                default: i = h;
                    break
            }
            return i
        }
        function d(i) {
            i = i || window.event;
            var l = i.srcElement || i.target;
            var k = i.keyCode || i.which;
            var m = $(l).val().toString();
            if (k == 13) {
                $(l).triggerHandler("enter_event", [l])
            }
            var h = [8, 36, 35, 37, 39, 46];
            if (!h.exists(k)) {
                if (g.limit_inputKeyCode) {
                    if (!((k >= 48 && k <= 57) || (k >= 96 && k <= 105) || [110, 190].exists(k))) {
                        return false
                    }

                }
                else {
                    switch (k) {
                        case 110: k = 190;
                            break
                    }
                    if (k >= 96 && k <= 105) {
                        k = k - 48
                    }
                    else {
                        if (k >= 188 && k <= 191) {
                            k = k - 144
                        }

                    }
                    var j = String.fromCharCode(k);
                    if (g.limit_inputRegexp != null && !g.limit_inputRegexp.test(j)) {
                        return false
                    }

                }
                $(l).attr("lastValue", m)
            }
            return true
        }
        function e(h) {
            h = h || window.event;
            var k = h.srcElement || h.target;
            var l = $(k).val().toString();
            var i = c(l);
            var j = $(k).attr("lastValue");
            if (g.limit_outputRegexp != null && !g.limit_outputRegexp.test(i)) {
                $(k).val(j);
                return
            }
            if (l != i) {
                $(k).val(i)
            }
            $(k).triggerHandler("keyup_event", [k])
        }
        function b(h) {
            h = h || window.event;
            var i = h.srcElement || h.target;
            if (g.change_min != null) {
                var j = parseFloat($(i).val());
                if (isNaN(j) || j < g.change_min) {
                    $(i).val(g.change_min)
                }
                else {
                    $(i).val(j)
                }

            }
            if (g.change_max != null) {
                var j = parseFloat($(i).val());
                if (isNaN(j) || j > g.change_max) {
                    $(i).val(g.change_max)
                }
                else {
                    $(i).val(j)
                }

            }
            $(i).triggerHandler("change_event", [i])
        }
        function f(h) {
            h = window.event || h;
            var i = h.target || h.srcElement;
            if (g.autoselect) {
                $(i).select()
            }

        }
        this.each(function () {
            $(this).bind("keydown", function (h) {
                return d(h)
            }).bind("keyup", function (h) {
                return e(h)
            }).bind("change", function (h) {
                return b(h)
            }).bind("blur", function (h) {
                return b(h)
            }).bind("mouseup", function (h) {
                return f(h)
            }).bind("enter_event", g.enter_event).bind("keyup_event", g.keyup_event).bind("change_event", g.change_event);
            if (g.limit_length != null) {
                $(this).attr("maxLength", g.limit_length)
            }

        });
        return this
    }

})();
(function (window, undefined) {
    function User() { } User.DomHeader = function () { };
    User.DomHeader.prototype = {
        LoginedWrap: null, UnLoginWrap: null, BtnLogin: null, MyAccountWrap: null, MyAccountPop: null, LoginedAccountBox: null, UserName: null, Balance: null, LastUpdateBalanceDate: new Date().addDays(-1), initialize: function () {
            this.LoginedWrap = $("div.login_on");
            this.UserName = $(this.LoginedWrap).children("b");
            this.UnLoginWrap = $("div.login_bg");
            this.BtnLogin = $(this.UnLoginWrap).children("a:first");
            this.MyAccountWrap = $("div.myAccount");
            this.MyAccountPop = $(this.MyAccountWrap).find("div.Pop div.mybetPop");
            this.LoginedAccountBox = $(this.MyAccountPop).children("div.L");
            this.Balance = $(this.LoginedAccountBox).find("ul:first li:first span")
        }

    };
    User.DomLogin = function () { };
    User.DomLogin.prototype = {
        EnableLoginAspx: false, EnableSessionCode: false, Wrap: null, UserName: null, Password: null, Session: null, Image: null, BtnChangeCode: null, BtnLogin: null, CodeWrap: null, showError: function (argMsg, argClosedEvent) {
            if (this.EnableLoginAspx) {
                if ($.trim(argMsg).length == 0) {
                    return
                }
                var dialogError = $('<div class="dia_c_main"><div class="cet"><b class="plaint"></b><span>' + argMsg + '</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>');
                var loginErrorDialog = $.artDialog({
                    content: dialogError.get(0), beforeunload: argClosedEvent || function () { }
                });
                $(dialogError).find("a[class*=Dbutt]").click(function () {
                    loginErrorDialog.close()
                })
            }
            else {
                if ($.trim(argMsg).length > 0) {
                    $(this.Wrap).find(".dia_c_error").addClass("show").html(argMsg)
                }
                else {
                    $(this.Wrap).find(".dia_c_error").removeClass("show")
                }

            }

        }
        , responseSubmit: function (argData) {
            var data = new $S.JsonCommand().parse(JSON.parse(argData));
            switch (data.Command) {
                case 0: this.EnableSessionCode = false;
                    $(this).triggerHandler("login", [data.ListParameter]);
                    return;
                    break;
                case 101: case 102: this.showError("登录账户或密码错误", function () {
                    $(this.Password).focus().val("")
                }
                .bind(this));
                    break;
                case 104: this.showError("验证码错误", function () {
                    $(this.Session).focus().val("")
                }
                .bind(this));
                    this.EnableSessionCode = true;
                    break;
                case 105: this.showError("登录账户或密码错误", function () {
                    $(this.Password).focus().val("")
                }
                .bind(this));
                    this.EnableSessionCode = true;
                    break;
                default: this.showError(String.format("登录失败，代码[{0}]", data.Command));
                    break
            }
            this.refreshSessionCode();
            $(this.BtnLogin).removeClass("disabled")
        }
        , responseError: function (e) {
            if ($S.Debug.Enable) {
                $.artDialog.tkAlert(e.responseText)
            }
            $(this.BtnLogin).removeClass("disabled")
        }
        , submit: function () {
            if ($(this.BtnLogin).hasClass("disabled")) {
                return
            }
            if ($.trim($(this.UserName).val()).length == 0) {
                this.showError("请填写登录账号", function () {
                    $(this.UserName).focus().select()
                }
                .bind(this));
                return
            }
            if ($(this.Password).val().length == 0) {
                this.showError("请输入登录密码", function () {
                    $(this.Password).focus()
                }
                .bind(this));
                return
            }
            if (this.EnableSessionCode && $.trim($(this.Session).val()).length == 0) {
                this.showError("请填写验证码", function () {
                    $(this.Session).focus().select()
                }
                .bind(this));
                return
            }
            this.showError("");
            $(this.BtnLogin).addClass("disabled");
            $.ajax({
                url: "/handler/login.ashx", type: "post", data: {
                    type: "doLogin", username: $.trim($(this.UserName).val()), password: $(this.Password).val(), code: $.trim($(this.Session).val())
                }
                , success: this.responseSubmit.bind(this), error: this.responseError.bind(this)
            })
        }
        , autoFocus: function () {
            var userInputName = $.trim($(this.UserName).val());
            var lastLoginName = userInputName == "" ? $S.Cookie.get("TKshishicai_user_LastUserName") : userInputName;
            var autoInputName = lastLoginName || "";
            $(this.UserName).val(autoInputName);
            if (autoInputName == "") {
                $(this.UserName).focus()
            }
            else {
                $(this.Password).focus().select()
            }

        }
        , refreshSessionCode: function (argFocus) {
            if (!this.EnableSessionCode) {
                $(this.CodeWrap).hide();
                return
            }
            $(this.CodeWrap).show();
            $(this.Image).attr("src", "/handler/sessioncode.ashx?" + Math.random());
            if (argFocus == true) {
                $(this.Session).focus().select()
            }

        }
        , initialize: function (argIsAspx, argSessionCode) {
            this.EnableLoginAspx = argIsAspx || false;
            this.EnableSessionCode = argSessionCode || false;
            if (!this.EnableLoginAspx) {
                this.Wrap = $(String.format('<div class="dialog_content"><div class="dia_c_main"><div class="dia_c_error">&nbsp;</div><div class="L"><div><label>登录账号</label><input type="text" class="input" tabindex="1" /><a class="forget" href="http://{0}/register.aspx">免费注册</a></div><div><label>登录密码</label><input type="password" class="input" tabindex="1" /><a class="forget" href="http://{0}/password/">忘记密码？</a></div><div class="Login_CodeTag"><label>验证码</label><input type="text" class="code" tabindex="1" /><span class="codePic"><img id="imgLoginCode" src="" width="80" height="26" /></span><a class="changeCode">换一张？</a></div><div class="submitDiv"><label>&nbsp;</label><a class="Dbutt" href="javascript:void(0)"><span>登 录</span></a></div></div><div class="R"><p class="tit">第三方登录</p><p><a class="but_alipay" href="{1}">支付宝登录</a></p></div></div></div>', TK.Url.login_interface_alipay.toUrl().host, TK.Url.login_interface_alipay));
                this.BtnLogin = $(this.Wrap).find("a.Dbutt").click(this.submit.bind(this))
            }
            else {
                this.Wrap = $("div.main div.regLeft");
                this.BtnLogin = $(this.Wrap).find("a.butt").click(this.submit.bind(this))
            }
            this.UserName = $(this.Wrap).find("input[type=text]:first").dom({
                enter_event: this.submit.bind(this)
            });
            this.Password = $(this.Wrap).find("input[type=password]:first").dom({
                enter_event: this.submit.bind(this)
            });
            this.Session = $(this.Wrap).find("input[type=text]:last").dom({
                enter_event: this.submit.bind(this)
            });
            this.Image = $(this.Wrap).find("img").click(this.refreshSessionCode.bind(this, true));
            this.BtnChangeCode = $(this.Wrap).find("a.changeCode").click(this.refreshSessionCode.bind(this, true));
            this.CodeWrap = $(this.Wrap).find("div.Login_CodeTag");
            if (this.EnableLoginAspx) {
                this.refreshSessionCode();
                this.autoFocus()
            }

        }

    };
    User.prototype = {
        DomHeader: new User.DomHeader(), DomLogin: new User.DomLogin(), DomArtDialog: null, EnableLoginAspx: false, EnableSessionCode: false, Handler_LoginSuccess: "Handler_LoginSuccess", UserName: "", TrueName: "", UserLevel: 0, IsBindPhone: false, IsLogined: false, UrlLoginSuccess: "/", handler_BindPhoneChanged: function () { }, showLoginForm: function () {
            if (this.EnableLoginAspx) { } else {
                this.DomArtDialog = $.artDialog({
                    title: "登录", content: $(this.DomLogin.Wrap).get(0)
                })
            }
            this.DomLogin.refreshSessionCode();
            this.DomLogin.autoFocus()
        }
        , event_logined: function (e, argListParameter) {
            this.UserName = argListParameter[0];
            this.UserLevel = parseInt(argListParameter.length >= 2 ? argListParameter[1] : this.UserLevel);
            this.IsBindPhone = argListParameter.length >= 3 ? eval(argListParameter[2]) : this.IsBindPhone;
            this.IsLogined = true;
            this.initStatus();
            if (this.EnableLoginAspx) {
                window.location.href = this.UrlLoginSuccess
            }
            else {
                this.DomArtDialog.close()
            }
            $(this).triggerHandler(this.Handler_LoginSuccess)
        }
        , responseAccountInfo: function (argData) {
            $(this.DomHeader.Balance).html(parseFloat(argData).toMoney("", 2, "floor") + "元");
            this.DomHeader.LastUpdateBalanceDate = new Date()
        }
        , requestAccountInfo: function () {
            if (this.IsLogined != true) {
                return
            }
            if ((new Date() - this.DomHeader.LastUpdateBalanceDate) < 5000) {
                return
            }
            $(this.DomHeader.Balance).html('<i class="d-loading">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i>');
            $.ajax({
                type: "post", url: "/handler/login.ashx", data: {
                    type: "balance"
                }
                , success: this.responseAccountInfo.bind(this)
            })
        }
        , showHeaderMenuBox: function () {
            if (this.IsLogined) {
                this.requestAccountInfo();
                $(this.DomHeader.LoginedAccountBox).show()
            }
            else {
                $(this.DomHeader.LoginedAccountBox).hide()
            }
            $(this.DomHeader.MyAccountPop).show();
            $(this.DomHeader.MyAccountWrap).addClass("active")
        }
        , hideHeaderMenuBox: function () {
            if (this.IsLogined) {
                this.DomHeader.LastUpdateBalanceDate = new Date()
            }
            $(this.DomHeader.MyAccountPop).hide();
            $(this.DomHeader.MyAccountWrap).removeClass("active")
        }
        , initStatus: function () {
            if (this.IsLogined) {
                $(this.DomHeader.LoginedWrap).show();
                $(this.DomHeader.UnLoginWrap).hide();
                $(this.DomHeader.UserName).html(this.UserName);
                if (this.UserLevel == 20) {
                    $(this.DomHeader.UserName).addClass("vip")
                }

            }
            else {
                $(this.DomHeader.LoginedWrap).hide();
                $(this.DomHeader.UnLoginWrap).show()
            }

        }
        , initialize: function (argUserName, argIsLogined, argIsBindPhone) {
            if (typeof TK_User_EnableLoginAspx !== "undefined") {
                this.EnableLoginAspx = TK_User_EnableLoginAspx
            }
            if (typeof TK_User_EnableSessionCode !== "undefined") {
                this.EnableSessionCode = TK_User_EnableSessionCode
            }
            if (typeof TK_User_UrlLoginSuccess !== "undefined") {
                this.UrlLoginSuccess = TK_User_UrlLoginSuccess
            }
            this.UserName = argUserName || "";
            this.IsLogined = argIsLogined || false;
            this.IsBindPhone = argIsBindPhone || false;
            this.DomHeader.initialize();
            $(this.DomHeader.BtnLogin).bind("click", this.showLoginForm.bind(this));
            this.DomLogin.initialize(this.EnableLoginAspx, this.EnableSessionCode);
            $(this.DomLogin).bind("login", this.event_logined.bind(this));
            this.initStatus();
            this.DomHeader.MyAccountWrap.hover(this.showHeaderMenuBox.bind(this), this.hideHeaderMenuBox.bind(this))
        }

    };
    if (typeof TK.User === "undefined") {
        TK.User = new User()
    }

})(window);
