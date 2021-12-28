/*
jQWidgets v3.6.0 (2014-Nov-25)
Copyright (c) 2011-2014 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a) {
    a.jqx.jqxWidget("jqxMenu", "", {});
    a.extend(a.jqx._jqxMenu.prototype, {
        defineInstance: function() {
            var b = {
                items: new Array(),
                mode: "horizontal",
                width: null,
                height: null,
                minimizeWidth: "auto",
                easing: "easeInOutSine",
                animationShowDuration: 200,
                animationHideDuration: 200,
                autoCloseInterval: 0,
                animationHideDelay: 100,
                animationShowDelay: 100,
                menuElements: new Array(),
                autoSizeMainItems: false,
                autoCloseOnClick: true,
                autoCloseOnMouseLeave: true,
                enableRoundedCorners: true,
                disabled: false,
                autoOpenPopup: true,
                enableHover: true,
                autoOpen: true,
                autoGenerate: true,
                clickToOpen: false,
                showTopLevelArrows: false,
                touchMode: "auto",
                source: null,
                popupZIndex: 17000,
                rtl: false,
                keyboardNavigation: false,
                lockFocus: false,
                title: "",
                events: ["shown", "closed", "itemclick", "initialized"]
            };
            a.extend(true, this, b);
            return b
        },
        createInstance: function(c) {
            var b = this;
            this.host.attr("role", "menubar");
            a.jqx.utilities.resize(this.host, function() {
                b.refresh()
            }, false, this.mode != "popup");
            if (this.minimizeWidth != "auto" && this.minimizeWidth != null && this.width && this.width.toString().indexOf("%") == -1) {
                a(window).resize(function() {
                    b.refresh()
                })
            }
            this.host.css("outline", "none");
            if (this.source) {
                if (this.source != null) {
                    var d = this.loadItems(this.source);
                    this.element.innerHTML = d
                }
            }
            this._tmpHTML = this.element.innerHTML;
            if (this.element.innerHTML.indexOf("UL")) {
                var e = this.host.find("ul:first");
                if (e.length > 0) {
                    this._createMenu(e[0])
                }
            }
            this.host.data("autoclose", {});
            this._render();
            this.setSize();
            if (a.jqx.browser.msie && a.jqx.browser.version < 8) {
                this.host.attr("hideFocus", true)
            }
        },
        focus: function() {
            try {
                this.host.focus();
                if (this.keyboardNavigation) {
                    if (!this.activeItem) {
                        a(this.items[0].element).addClass(this.toThemeProperty("jqx-fill-state-focus"));
                        this.activeItem = this.items[0]
                    }
                }
            } catch (b) {}
        },
        loadItems: function(c, e) {
            if (c == null) {
                return
            }
            if (c.length == 0) {
                return ""
            }
            var b = this;
            this.items = new Array();
            var d = "<ul>";
            if (e) {
                d = '<ul style="width:' + e + ';">'
            }
            a.map(c, function(f) {
                if (f == undefined) {
                    return null
                }
                d += b._parseItem(f)
            });
            d += "</ul>";
            return d
        },
        _parseItem: function(f) {
            var c = "";
            if (f == undefined) {
                return null
            }
            var b = f.label;
            if (!f.label && f.html) {
                b = f.html
            }
            if (!b) {
                b = "Item"
            }
            if (typeof f === "string") {
                b = f
            }
            var e = false;
            if (f.selected != undefined && f.selected) {
                e = true
            }
            var d = false;
            if (f.disabled != undefined && f.disabled) {
                d = true
            }
            c += "<li";
            if (d) {
                c += ' item-disabled="true" '
            }
            if (f.label && !f.html) {
                c += ' item-label="' + b + '" '
            }
            if (f.value != null) {
                c += ' item-value="' + f.value + '" '
            }
            if (f.id != undefined) {
                c += ' id="' + f.id + '" '
            }
            c += ">" + b;
            if (f.items) {
                if (f.subMenuWidth) {
                    c += this.loadItems(f.items, f.subMenuWidth)
                } else {
                    c += this.loadItems(f.items)
                }
            }
            c += "</li>";
            return c
        },
        setSize: function() {
            if (this.width != null && this.width.toString().indexOf("%") != -1) {
                this.host.width(this.width)
            } else {
                if (this.width != null && this.width.toString().indexOf("px") != -1) {
                    this.host.width(this.width)
                } else {
                    if (this.width != undefined && !isNaN(this.width)) {
                        this.host.width(this.width)
                    }
                }
            }
            if (this.height != null && this.height.toString().indexOf("%") != -1) {
                this.host.height(this.height)
            } else {
                if (this.height != null && this.height.toString().indexOf("px") != -1) {
                    this.host.height(this.height)
                } else {
                    if (this.height != undefined && !isNaN(this.height)) {
                        this.host.height(this.height)
                    }
                }
            }
            if (this.height === null) {
                this.host.height("auto")
            }
            var g = this;
            if (this.minimizeWidth != null && this.mode != "popup") {
                var f = a(window).width();
                if (!a.jqx.response) {
                    var e = false;
                    if (navigator.userAgent.match(/Windows|Linux|MacOS/)) {
                        var b = navigator.userAgent.indexOf("Windows Phone") >= 0 || navigator.userAgent.indexOf("WPDesktop") >= 0 || navigator.userAgent.indexOf("IEMobile") >= 0 || navigator.userAgent.indexOf("ZuneWP7") >= 0;
                        if (!b) {
                            e = true
                        }
                    }
                    var c = this.minimizeWidth;
                    if (e && this.minimizeWidth == "auto") {
                        return
                    }
                }
                if (this.minimizeWidth == "auto" && a.jqx.response) {
                    var d = new a.jqx.response();
                    if (d.device.type == "Phone" || d.device.type == "Tablet") {
                        if (!this.minimized) {
                            this.minimize()
                        }
                    }
                } else {
                    if ((f < c) && !this.minimized) {
                        this.minimize()
                    } else {
                        if (this.minimized && f >= c) {
                            this.restore()
                        }
                    }
                }
            }
        },
        minimize: function() {
            if (this.minimized) {
                return
            }
            var e = this;
            this.host.addClass(this.toThemeProperty("jqx-menu-minimized"));
            this.minimized = true;
            this._tmpMode = this.mode;
            this.mode = "simple";
            var h = this.host.closest("div.jqx-menu-wrapper");
            h.remove();
            a("#menuWrapper" + this.element.id).remove();
            a.each(this.items, function() {
                var k = this;
                var j = a(k.element);
                var i = a(k.subMenuElement);
                var l = i.closest("div.jqx-menu-popup");
                l.remove()
            });
            if (this.source) {
                var d = this.loadItems(this.source);
                this.element.innerHTML = d;
                this._tmpHTML = this.element.innerHTML
            }
            this.element.innerHTML = this._tmpHTML;
            if (this.element.innerHTML.indexOf("UL")) {
                var g = this.host.find("ul:first");
                if (g.length > 0) {
                    this._createMenu(g[0])
                }
            }
            this._render();
            var c = this.host.find("ul:first");
            c.wrap('<div class="jqx-menu-wrapper" style="z-index:' + this.popupZIndex + '; padding: 0px; display: none; margin: 0px; height: auto; width: auto; position: absolute; top: 0; left: 0; display: block; visibility: visible;"></div>');
            var h = c.closest("div.jqx-menu-wrapper");
            h[0].id = "menuWrapper" + this.element.id;
            h.detach();
            h.appendTo(a(document.body));
            h.addClass(this.toThemeProperty("jqx-widget"));
            h.addClass(this.toThemeProperty("jqx-menu"));
            h.addClass(this.toThemeProperty("jqx-menu-minimized"));
            h.addClass(this.toThemeProperty("jqx-widget-header"));
            c.children().hide();
            h.hide();
            h.find("ul").addClass(this.toThemeProperty("jqx-menu-ul-minimized"));
            this.minimizedItem = a("<div></div>");
            this.minimizedItem.addClass(this.toThemeProperty("jqx-item"));
            this.minimizedItem.addClass(this.toThemeProperty("jqx-menu-item-top"));
            this.addHandler(h, "keydown", function(i) {
                return e.host.trigger(i)
            });
            this.minimizedItem.addClass(this.toThemeProperty("jqx-menu-minimized-button"));
            this.minimizedItem.prependTo(this.host);
            this.titleElement = a("<div>" + this.title + "</div>");
            this.titleElement.addClass(this.toThemeProperty("jqx-item"));
            this.titleElement.addClass(this.toThemeProperty("jqx-menu-title"));
            this.titleElement.prependTo(this.host);
            a("<div style='clear:both;'></div>").insertAfter(this.minimizedItem);
            e.minimizedHidden = true;
            var b = function(j) {
                e.minimizedHidden = true;
                e.minimizedItem.show();
                var i = false;
                if (e.minimizedItem.css("float") == "right") {
                    i = true
                }
                h.animate({
                    left: !i ? -h.outerWidth() : e.host.coord().left + e.host.width() + h.width(),
                    opacity: 0
                }, e.animationHideDuration, function() {
                    h.find("ul:first").children().hide();
                    h.hide()
                })
            };
            var f = function(k) {
                if (e.minimizedHidden) {
                    h.find("ul:first").children().show();
                    e.minimizedHidden = false;
                    h.show();
                    h.css("opacity", 0);
                    h.css("left", -h.outerWidth());
                    var j = false;
                    var i = h.width();
                    if (e.minimizedItem.css("float") == "right") {
                        h.css("left", e.host.coord().left + e.host.width() + i);
                        j = true
                    }
                    h.css("top", e.host.coord().top + e.host.height());
                    h.animate({
                        left: !j ? e.host.coord().left : e.host.coord().left + e.host.width() - i,
                        opacity: 0.95
                    }, e.animationShowDuration, function() {})
                } else {
                    b(k)
                }
                e._raiseEvent("2", {
                    item: e.minimizedItem[0],
                    event: k
                });
                e.setSize()
            };
            this.addHandler(a(window), "orientationchange.jqxmenu" + this.element.id, function(i) {
                setTimeout(function() {
                    if (!e.minimizedHidden) {
                        var j = h.width();
                        var k = false;
                        var j = h.width();
                        if (e.minimizedItem.css("float") == "right") {
                            k = true
                        }
                        h.css("top", e.host.coord().top + e.host.height());
                        h.css({
                            left: !k ? e.host.coord().left : e.host.coord().left + e.host.width() - j
                        })
                    }
                }, 25)
            });
            this.addHandler(this.minimizedItem, "click", function(i) {
                f(i)
            })
        },
        restore: function() {
            if (!this.minimized) {
                return
            }
            this.host.find("ul").removeClass(this.toThemeProperty("jqx-menu-ul-minimized"));
            this.host.removeClass(this.toThemeProperty("jqx-menu-minimized"));
            this.minimized = false;
            this.mode = this._tmpMode;
            if (this.minimizedItem) {
                this.minimizedItem.remove()
            }
            var d = a("#menuWrapper" + this.element.id);
            d.remove();
            if (this.source) {
                var b = this.loadItems(this.source);
                this.element.innerHTML = b;
                this._tmpHTML = b
            }
            this.element.innerHTML = this._tmpHTML;
            if (this.element.innerHTML.indexOf("UL")) {
                var c = this.host.find("ul:first");
                if (c.length > 0) {
                    this._createMenu(c[0])
                }
            }
            this.setSize();
            this._render()
        },
        isTouchDevice: function() {
            if (this._isTouchDevice != undefined) {
                return this._isTouchDevice
            }
            var b = a.jqx.mobile.isTouchDevice();
            if (this.touchMode == true) {
                b = true
            } else {
                if (this.touchMode == false) {
                    b = false
                }
            }
            if (b) {
                this.host.addClass(this.toThemeProperty("jqx-touch"));
                a(".jqx-menu-item").addClass(this.toThemeProperty("jqx-touch"))
            }
            this._isTouchDevice = b;
            return b
        },
        refresh: function(b) {
            if (!b) {
                this.setSize()
            }
        },
        resize: function(c, b) {
            this.width = c;
            this.height = b;
            this.refresh()
        },
        _closeAll: function(f) {
            var d = f != null ? f.data : this;
            var b = d.items;
            a.each(b, function() {
                var e = this;
                if (e.hasItems == true) {
                    if (e.isOpen) {
                        d._closeItem(d, e)
                    }
                }
            });
            if (d.mode == "popup") {
                if (f != null) {
                    var c = d._isRightClick(f);
                    if (!c) {
                        d.close()
                    }
                }
            }
        },
        closeItem: function(e) {
            if (e == null) {
                return false
            }
            var b = e;
            var c = document.getElementById(b);
            var d = this;
            a.each(d.items, function() {
                var f = this;
                if (f.isOpen == true && f.element == c) {
                    d._closeItem(d, f);
                    if (f.parentId) {
                        d.closeItem(f.parentId)
                    }
                }
            });
            return true
        },
        openItem: function(e) {
            if (e == null) {
                return false
            }
            var b = e;
            var c = document.getElementById(b);
            var d = this;
            a.each(d.items, function() {
                var f = this;
                if (f.isOpen == false && f.element == c) {
                    d._openItem(d, f);
                    if (f.parentId) {
                        d.openItem(f.parentId)
                    }
                }
            });
            return true
        },
        _getClosedSubMenuOffset: function(c) {
            var b = a(c.subMenuElement);
            var f = -b.outerHeight();
            var e = -b.outerWidth();
            var d = c.level == 0 && this.mode == "horizontal";
            if (d) {
                e = 0
            } else {
                f = 0
            }
            switch (c.openVerticalDirection) {
                case "up":
                case "center":
                    f = b.outerHeight();
                    break
            }
            switch (c.openHorizontalDirection) {
                case this._getDir("left"):
                    if (d) {
                        e = 0
                    } else {
                        e = b.outerWidth()
                    }
                    break;
                case "center":
                    if (d) {
                        e = 0
                    } else {
                        e = b.outerWidth()
                    }
                    break
            }
            return {
                left: e,
                top: f
            }
        },
        _closeItem: function(l, o, g, c) {
            if (l == null || o == null) {
                return false
            }
            var j = a(o.subMenuElement);
            var b = o.level == 0 && this.mode == "horizontal";
            var f = this._getClosedSubMenuOffset(o);
            var m = f.top;
            var e = f.left;
            var i = a(o.element);
            var k = j.closest("div.jqx-menu-popup");
            if (k != null) {
                var h = l.animationHideDelay;
                if (c == true) {
                    h = 0
                }
                if (j.data("timer") && j.data("timer").show != null) {
                    clearTimeout(j.data("timer").show);
                    j.data("timer").show = null
                }
                var n = function() {
                    o.isOpen = false;
                    if (b) {
                        j.stop().animate({
                            top: m
                        }, l.animationHideDuration, function() {
                            a(o.element).removeClass(l.toThemeProperty("jqx-fill-state-pressed"));
                            a(o.element).removeClass(l.toThemeProperty("jqx-menu-item-top-selected"));
                            a(o.element).removeClass(l.toThemeProperty("jqx-rc-b-expanded"));
                            k.removeClass(l.toThemeProperty("jqx-rc-t-expanded"));
                            var p = a(o.arrow);
                            if (p.length > 0 && l.showTopLevelArrows) {
                                p.removeClass();
                                if (o.openVerticalDirection == "down") {
                                    p.addClass(l.toThemeProperty("jqx-menu-item-arrow-down"));
                                    p.addClass(l.toThemeProperty("jqx-icon-arrow-down"))
                                } else {
                                    p.addClass(l.toThemeProperty("jqx-menu-item-arrow-up"));
                                    p.addClass(l.toThemeProperty("jqx-icon-arrow-up"))
                                }
                            }
                            a.jqx.aria(a(o.element), "aria-expanded", false);
                            k.css({
                                display: "none"
                            });
                            if (l.animationHideDuration == 0) {
                                j.css({
                                    top: m
                                })
                            }
                            l._raiseEvent("1", o)
                        })
                    } else {
                        if (!a.jqx.browser.msie) {}
                        j.stop().animate({
                            left: e
                        }, l.animationHideDuration, function() {
                            if (l.animationHideDuration == 0) {
                                j.css({
                                    left: e
                                })
                            }
                            if (o.level > 0) {
                                a(o.element).removeClass(l.toThemeProperty("jqx-fill-state-pressed"));
                                a(o.element).removeClass(l.toThemeProperty("jqx-menu-item-selected"));
                                var p = a(o.arrow);
                                if (p.length > 0) {
                                    p.removeClass();
                                    if (o.openHorizontalDirection != "left") {
                                        p.addClass(l.toThemeProperty("jqx-menu-item-arrow-" + l._getDir("right")));
                                        p.addClass(l.toThemeProperty("jqx-icon-arrow-" + l._getDir("right")))
                                    } else {
                                        p.addClass(l.toThemeProperty("jqx-menu-item-arrow-" + l._getDir("left")));
                                        p.addClass(l.toThemeProperty("jqx-icon-arrow-" + l._getDir("left")))
                                    }
                                }
                            } else {
                                a(o.element).removeClass(l.toThemeProperty("jqx-fill-state-pressed"));
                                a(o.element).removeClass(l.toThemeProperty("jqx-menu-item-top-selected"));
                                var p = a(o.arrow);
                                if (p.length > 0) {
                                    p.removeClass();
                                    if (o.openHorizontalDirection != "left") {
                                        p.addClass(l.toThemeProperty("jqx-menu-item-arrow-top-" + l._getDir("right")));
                                        p.addClass(l.toThemeProperty("jqx-icon-arrow-" + l._getDir("right")))
                                    } else {
                                        p.addClass(l.toThemeProperty("jqx-menu-item-arrow-top-" + l._getDir("left")));
                                        p.addClass(l.toThemeProperty("jqx-icon-arrow-" + l._getDir("left")))
                                    }
                                }
                            }
                            a.jqx.aria(a(o.element), "aria-expanded", false);
                            k.css({
                                display: "none"
                            });
                            l._raiseEvent("1", o)
                        })
                    }
                };
                if (h > 0) {
                    if (j.data("timer")) {
                        j.data("timer").hide = setTimeout(function() {
                            n()
                        }, h)
                    }
                } else {
                    n()
                }
                if (g != undefined && g) {
                    var d = j.children();
                    a.each(d, function() {
                        if (l.menuElements[this.id] && l.menuElements[this.id].isOpen) {
                            var p = a(l.menuElements[this.id].subMenuElement);
                            l._closeItem(l, l.menuElements[this.id], true, true)
                        }
                    })
                }
            }
        },
        getSubItems: function(i, h) {
            if (i == null) {
                return false
            }
            var g = this;
            var c = new Array();
            if (h != null) {
                a.extend(c, h)
            }
            var d = i;
            var f = this.menuElements[d];
            var b = a(f.subMenuElement);
            var e = b.find(".jqx-menu-item");
            a.each(e, function() {
                c[this.id] = g.menuElements[this.id];
                var j = g.getSubItems(this.id, c);
                a.extend(c, j)
            });
            return c
        },
        disable: function(g, d) {
            if (g == null) {
                return
            }
            var c = g;
            var f = this;
            if (this.menuElements[c]) {
                var e = this.menuElements[c];
                e.disabled = d;
                var b = a(e.element);
                e.element.disabled = d;
                a.each(b.children(), function() {
                    this.disabled = d
                });
                if (d) {
                    b.addClass(f.toThemeProperty("jqx-menu-item-disabled"));
                    b.addClass(f.toThemeProperty("jqx-fill-state-disabled"))
                } else {
                    b.removeClass(f.toThemeProperty("jqx-menu-item-disabled"));
                    b.removeClass(f.toThemeProperty("jqx-fill-state-disabled"))
                }
            }
        },
        _setItemProperty: function(g, c, f) {
            if (g == null) {
                return
            }
            var b = g;
            var e = this;
            if (this.menuElements[b]) {
                var d = this.menuElements[b];
                if (d[c]) {
                    d[c] = f
                }
            }
        },
        setItemOpenDirection: function(d, c, e) {
            if (d == null) {
                return
            }
            var j = d;
            var g = this;
            var f = a.jqx.browser.msie && a.jqx.browser.version < 8;
            if (this.menuElements[j]) {
                var i = this.menuElements[j];
                if (c != null) {
                    i.openHorizontalDirection = c;
                    if (i.hasItems && i.level > 0) {
                        var h = a(i.element);
                        if (h != undefined) {
                            var b = a(i.arrow);
                            if (i.arrow == null) {
                                b = a('<span id="arrow' + h[0].id + '"></span>');
                                if (!f) {
                                    b.prependTo(h)
                                } else {
                                    b.appendTo(h)
                                }
                                i.arrow = b[0]
                            }
                            b.removeClass();
                            if (i.openHorizontalDirection == "left") {
                                b.addClass(g.toThemeProperty("jqx-menu-item-arrow-" + g._getDir("left")));
                                b.addClass(g.toThemeProperty("jqx-icon-arrow-" + g._getDir("left")))
                            } else {
                                b.addClass(g.toThemeProperty("jqx-menu-item-arrow-" + g._getDir("right")));
                                b.addClass(g.toThemeProperty("jqx-icon-arrow-" + g._getDir("right")))
                            }
                            b.css("visibility", "visible");
                            if (!f) {
                                b.css("display", "block");
                                b.css("float", "right")
                            } else {
                                b.css("display", "inline-block");
                                b.css("float", "none")
                            }
                        }
                    }
                }
                if (e != null) {
                    i.openVerticalDirection = e;
                    var b = a(i.arrow);
                    var h = a(i.element);
                    if (!g.showTopLevelArrows) {
                        return
                    }
                    if (h != undefined) {
                        if (i.arrow == null) {
                            b = a('<span id="arrow' + h[0].id + '"></span>');
                            if (!f) {
                                b.prependTo(h)
                            } else {
                                b.appendTo(h)
                            }
                            i.arrow = b[0]
                        }
                        b.removeClass();
                        if (i.openVerticalDirection == "down") {
                            b.addClass(g.toThemeProperty("jqx-menu-item-arrow-down"));
                            b.addClass(g.toThemeProperty("jqx-icon-arrow-down"))
                        } else {
                            b.addClass(g.toThemeProperty("jqx-menu-item-arrow-up"));
                            b.addClass(g.toThemeProperty("jqx-icon-arrow-up"))
                        }
                        b.css("visibility", "visible");
                        if (!f) {
                            b.css("display", "block");
                            b.css("float", "right")
                        } else {
                            b.css("display", "inline-block");
                            b.css("float", "none")
                        }
                    }
                }
            }
        },
        _getSiblings: function(d) {
            var e = new Array();
            var b = 0;
            for (var c = 0; c < this.items.length; c++) {
                if (this.items[c] == d) {
                    continue
                }
                if (this.items[c].parentId == d.parentId && this.items[c].hasItems) {
                    e[b++] = this.items[c]
                }
            }
            return e
        },
        _openItem: function(s, r, q) {
            if (s == null || r == null) {
                return false
            }
            if (r.isOpen) {
                return false
            }
            if (r.disabled) {
                return false
            }
            if (s.disabled) {
                return false
            }
            var l = s.popupZIndex;
            if (q != undefined) {
                l = q
            }
            var e = s.animationHideDuration;
            s.animationHideDuration = 0;
            s._closeItem(s, r, true, true);
            s.animationHideDuration = e;
            this.host.focus();
            var f = [5, 5];
            var t = a(r.subMenuElement);
            if (t != null) {
                t.stop()
            }
            if (t.data("timer") && t.data("timer").hide != null) {
                clearTimeout(t.data("timer").hide)
            }
            var o = t.closest("div.jqx-menu-popup");
            var h = a(r.element);
            var i = r.level == 0 ? this._getOffset(r.element) : h.position();
            if (r.level > 0 && this.hasTransform) {
                var p = parseInt(h.coord().top) - parseInt(this._getOffset(r.element).top);
                i.top += p
            }
            if (r.level == 0 && this.mode == "popup") {
                i = h.coord()
            }
            var j = r.level == 0 && this.mode == "horizontal";
            var b = j ? i.left : this.menuElements[r.parentId] != null && this.menuElements[r.parentId].subMenuElement != null ? parseInt(a(a(this.menuElements[r.parentId].subMenuElement).closest("div.jqx-menu-popup")).outerWidth()) - f[0] : parseInt(t.outerWidth());
            o.css({
                visibility: "visible",
                display: "block",
                left: b,
                top: j ? i.top + h.outerHeight() : i.top,
                zIndex: l
            });
            t.css("display", "block");
            if (this.mode != "horizontal" && r.level == 0) {
                var d = this._getOffset(this.element);
                o.css("left", -1 + d.left + this.host.outerWidth());
                t.css("left", -t.outerWidth())
            } else {
                var c = this._getClosedSubMenuOffset(r);
                t.css("left", c.left);
                t.css("top", c.top)
            }
            o.css({
                height: parseInt(t.outerHeight()) + parseInt(f[1]) + "px"
            });
            var n = 0;
            var g = 0;
            switch (r.openVerticalDirection) {
                case "up":
                    if (j) {
                        t.css("top", t.outerHeight());
                        n = f[1];
                        var k = parseInt(t.parent().css("padding-bottom"));
                        if (isNaN(k)) {
                            k = 0
                        }
                        if (k > 0) {
                            o.addClass(this.toThemeProperty("jqx-menu-popup-clear"))
                        }
                        t.css("top", t.outerHeight() - k);
                        o.css({
                            display: "block",
                            top: i.top - o.outerHeight(),
                            zIndex: l
                        })
                    } else {
                        n = f[1];
                        t.css("top", t.outerHeight());
                        o.css({
                            display: "block",
                            top: i.top - o.outerHeight() + f[1] + h.outerHeight(),
                            zIndex: l
                        })
                    }
                    break;
                case "center":
                    if (j) {
                        t.css("top", 0);
                        o.css({
                            display: "block",
                            top: i.top - o.outerHeight() / 2 + f[1],
                            zIndex: l
                        })
                    } else {
                        t.css("top", 0);
                        o.css({
                            display: "block",
                            top: i.top + h.outerHeight() / 2 - o.outerHeight() / 2 + f[1],
                            zIndex: l
                        })
                    }
                    break
            }
            switch (r.openHorizontalDirection) {
                case this._getDir("left"):
                    if (j) {
                        o.css({
                            left: i.left - (o.outerWidth() - h.outerWidth() - f[0])
                        })
                    } else {
                        g = 0;
                        t.css("left", o.outerWidth());
                        o.css({
                            left: i.left - (o.outerWidth()) + 2 * r.level
                        })
                    }
                    break;
                case "center":
                    if (j) {
                        o.css({
                            left: i.left - (o.outerWidth() / 2 - h.outerWidth() / 2 - f[0] / 2)
                        })
                    } else {
                        o.css({
                            left: i.left - (o.outerWidth() / 2 - h.outerWidth() / 2 - f[0] / 2)
                        });
                        t.css("left", o.outerWidth())
                    }
                    break
            }
            if (j) {
                if (parseInt(t.css("top")) == n) {
                    r.isOpen = true;
                    return
                }
            } else {
                if (parseInt(t.css("left")) == g) {
                    r.isOpen == true;
                    return
                }
            }
            a.each(s._getSiblings(r), function() {
                s._closeItem(s, this, true, true)
            });
            var m = a.data(s.element, "animationHideDelay");
            s.animationHideDelay = m;
            if (this.autoCloseInterval > 0) {
                if (this.host.data("autoclose") != null && this.host.data("autoclose").close != null) {
                    clearTimeout(this.host.data("autoclose").close)
                }
                if (this.host.data("autoclose") != null) {
                    this.host.data("autoclose").close = setTimeout(function() {
                        s._closeAll()
                    }, this.autoCloseInterval)
                }
            }
            if (t.data("timer")) {
                t.data("timer").show = setTimeout(function() {
                    if (o != null) {
                        if (j) {
                            t.stop();
                            t.css("left", g);
                            if (!a.jqx.browser.msie) {}
                            h.addClass(s.toThemeProperty("jqx-fill-state-pressed"));
                            h.addClass(s.toThemeProperty("jqx-menu-item-top-selected"));
                            if (r.openVerticalDirection == "down") {
                                a(r.element).addClass(s.toThemeProperty("jqx-rc-b-expanded"));
                                o.addClass(s.toThemeProperty("jqx-rc-t-expanded"))
                            } else {
                                a(r.element).addClass(s.toThemeProperty("jqx-rc-t-expanded"));
                                o.addClass(s.toThemeProperty("jqx-rc-b-expanded"))
                            }
                            var u = a(r.arrow);
                            if (u.length > 0 && s.showTopLevelArrows) {
                                u.removeClass();
                                if (r.openVerticalDirection == "down") {
                                    u.addClass(s.toThemeProperty("jqx-menu-item-arrow-down-selected"));
                                    u.addClass(s.toThemeProperty("jqx-icon-arrow-down"))
                                } else {
                                    u.addClass(s.toThemeProperty("jqx-menu-item-arrow-up-selected"));
                                    u.addClass(s.toThemeProperty("jqx-icon-arrow-up"))
                                }
                            }
                            if (s.animationShowDuration == 0) {
                                t.css({
                                    top: n
                                });
                                r.isOpen = true;
                                s._raiseEvent("0", r);
                                a.jqx.aria(a(r.element), "aria-expanded", true)
                            } else {
                                t.animate({
                                    top: n
                                }, s.animationShowDuration, s.easing, function() {
                                    r.isOpen = true;
                                    a.jqx.aria(a(r.element), "aria-expanded", true);
                                    s._raiseEvent("0", r)
                                })
                            }
                        } else {
                            t.stop();
                            t.css("top", n);
                            if (!a.jqx.browser.msie) {}
                            if (r.level > 0) {
                                h.addClass(s.toThemeProperty("jqx-fill-state-pressed"));
                                h.addClass(s.toThemeProperty("jqx-menu-item-selected"));
                                var u = a(r.arrow);
                                if (u.length > 0) {
                                    u.removeClass();
                                    if (r.openHorizontalDirection != "left") {
                                        u.addClass(s.toThemeProperty("jqx-menu-item-arrow-" + s._getDir("right") + "-selected"));
                                        u.addClass(s.toThemeProperty("jqx-icon-arrow-" + s._getDir("right")))
                                    } else {
                                        u.addClass(s.toThemeProperty("jqx-menu-item-arrow-" + s._getDir("left") + "-selected"));
                                        u.addClass(s.toThemeProperty("jqx-icon-arrow-" + s._getDir("left")))
                                    }
                                }
                            } else {
                                h.addClass(s.toThemeProperty("jqx-fill-state-pressed"));
                                h.addClass(s.toThemeProperty("jqx-menu-item-top-selected"));
                                var u = a(r.arrow);
                                if (u.length > 0) {
                                    u.removeClass();
                                    if (r.openHorizontalDirection != "left") {
                                        u.addClass(s.toThemeProperty("jqx-menu-item-arrow-" + s._getDir("right") + "-selected"));
                                        u.addClass(s.toThemeProperty("jqx-icon-arrow-" + s._getDir("right")))
                                    } else {
                                        u.addClass(s.toThemeProperty("jqx-menu-item-arrow-" + s._getDir("left") + "-selected"));
                                        u.addClass(s.toThemeProperty("jqx-icon-arrow-" + s._getDir("left")))
                                    }
                                }
                            }
                            if (!a.jqx.browser.msie) {}
                            if (s.animationShowDuration == 0) {
                                t.css({
                                    left: g
                                });
                                s._raiseEvent("0", r);
                                r.isOpen = true;
                                a.jqx.aria(a(r.element), "aria-expanded", true)
                            } else {
                                t.animate({
                                    left: g
                                }, s.animationShowDuration, s.easing, function() {
                                    s._raiseEvent("0", r);
                                    r.isOpen = true;
                                    a.jqx.aria(a(r.element), "aria-expanded", true)
                                })
                            }
                        }
                    }
                }, this.animationShowDelay)
            }
        },
        _getDir: function(b) {
            switch (b) {
                case "left":
                    return !this.rtl ? "left" : "right";
                case "right":
                    return this.rtl ? "left" : "right"
            }
            return "left"
        },
        _applyOrientation: function(i, d) {
            var g = this;
            var f = 0;
            this.host.removeClass(g.toThemeProperty("jqx-menu-horizontal"));
            this.host.removeClass(g.toThemeProperty("jqx-menu-vertical"));
            this.host.removeClass(g.toThemeProperty("jqx-menu"));
            this.host.removeClass(g.toThemeProperty("jqx-widget"));
            this.host.addClass(g.toThemeProperty("jqx-widget"));
            this.host.addClass(g.toThemeProperty("jqx-menu"));
            if (i != undefined && d != undefined && d == "popup") {
                if (this.host.parent().length > 0 && this.host.parent().parent().length > 0 && this.host.parent().parent()[0] == document.body) {
                    var h = a.data(document.body, "jqxMenuOldHost" + this.element.id);
                    if (h != null) {
                        var e = this.host.closest("div.jqx-menu-wrapper");
                        e.remove();
                        e.appendTo(h);
                        this.host.css("display", "block");
                        this.host.css("visibility", "visible");
                        e.css("display", "block");
                        e.css("visibility", "visible")
                    }
                }
            } else {
                if (i == undefined && d == undefined) {
                    a.data(document.body, "jqxMenuOldHost" + this.element.id, this.host.parent()[0])
                }
            }
            if (this.autoOpenPopup) {
                if (this.mode == "popup") {
                    this.addHandler(a(document), "contextmenu." + this.element.id, function(j) {
                        return false
                    });
                    this.addHandler(a(document), "mousedown.menu" + this.element.id, function(j) {
                        g._openContextMenu(j)
                    })
                } else {
                    this.removeHandler(a(document), "contextmenu." + this.element.id);
                    this.removeHandler(a(document), "mousedown.menu" + this.element.id)
                }
            } else {
                this.removeHandler(a(document), "contextmenu." + this.element.id);
                this.removeHandler(a(document), "mousedown.menu" + this.element.id)
            }
            if (this.rtl) {
                this.host.addClass(this.toThemeProperty("jqx-rtl"))
            }
            switch (this.mode) {
                case "horizontal":
                    this.host.addClass(g.toThemeProperty("jqx-widget-header"));
                    this.host.addClass(g.toThemeProperty("jqx-menu-horizontal"));
                    a.each(this.items, function() {
                        var l = this;
                        $element = a(l.element);
                        var k = a(l.arrow);
                        k.removeClass();
                        if (l.hasItems && l.level > 0) {
                            var k = a('<span style="border: none; background-color: transparent;" id="arrow' + $element[0].id + '"></span>');
                            k.prependTo($element);
                            k.css("float", g._getDir("right"));
                            k.addClass(g.toThemeProperty("jqx-menu-item-arrow-" + g._getDir("right")));
                            k.addClass(g.toThemeProperty("jqx-icon-arrow-" + g._getDir("right")));
                            l.arrow = k[0]
                        }
                        if (l.level == 0) {
                            a(l.element).css("float", g._getDir("left"));
                            if (!l.ignoretheme && l.hasItems && g.showTopLevelArrows) {
                                var k = a('<span style="border: none; background-color: transparent;" id="arrow' + $element[0].id + '"></span>');
                                var j = a.jqx.browser.msie && a.jqx.browser.version < 8;
                                if (l.arrow == null) {
                                    if (!j) {
                                        k.prependTo($element)
                                    } else {
                                        k.appendTo($element)
                                    }
                                } else {
                                    k = a(l.arrow)
                                }
                                if (l.openVerticalDirection == "down") {
                                    k.addClass(g.toThemeProperty("jqx-menu-item-arrow-down"));
                                    k.addClass(g.toThemeProperty("jqx-icon-arrow-down"))
                                } else {
                                    k.addClass(g.toThemeProperty("jqx-menu-item-arrow-up"));
                                    k.addClass(g.toThemeProperty("jqx-icon-arrow-up"))
                                }
                                k.css("visibility", "visible");
                                if (!j) {
                                    k.css("display", "block");
                                    k.css("float", "right")
                                } else {
                                    k.css("display", "inline-block")
                                }
                                l.arrow = k[0]
                            } else {
                                if (!l.ignoretheme && l.hasItems && !g.showTopLevelArrows) {
                                    if (l.arrow != null) {
                                        var k = a(l.arrow);
                                        k.remove();
                                        l.arrow = null
                                    }
                                }
                            }
                            f = Math.max(f, $element.height())
                        }
                    });
                    break;
                case "vertical":
                case "popup":
                case "simple":
                    this.host.addClass(g.toThemeProperty("jqx-menu-vertical"));
                    a.each(this.items, function() {
                        var k = this;
                        $element = a(k.element);
                        if (k.hasItems && !k.ignoretheme) {
                            if (k.arrow) {
                                a(k.arrow).remove()
                            }
                            if (g.mode == "simple") {
                                return true
                            }
                            var j = a('<span style="border: none; background-color: transparent;" id="arrow' + $element[0].id + '"></span>');
                            j.prependTo($element);
                            j.css("float", "right");
                            if (k.level == 0) {
                                j.addClass(g.toThemeProperty("jqx-menu-item-arrow-top-" + g._getDir("right")));
                                j.addClass(g.toThemeProperty("jqx-icon-arrow-" + g._getDir("right")))
                            } else {
                                j.addClass(g.toThemeProperty("jqx-menu-item-arrow-" + g._getDir("right")));
                                j.addClass(g.toThemeProperty("jqx-icon-arrow-" + g._getDir("right")))
                            }
                            k.arrow = j[0]
                        }
                        $element.css("float", "none")
                    });
                    if (this.mode == "popup") {
                        this.host.addClass(g.toThemeProperty("jqx-widget-content"));
                        this.host.wrap('<div class="jqx-menu-wrapper" style="z-index:' + this.popupZIndex + '; border: none; background-color: transparent; padding: 0px; margin: 0px; position: absolute; top: 0; left: 0; display: block; visibility: visible;"></div>');
                        var e = this.host.closest("div.jqx-menu-wrapper");
                        this.host.addClass(g.toThemeProperty("jqx-popup"));
                        e[0].id = "menuWrapper" + this.element.id;
                        e.appendTo(a(document.body))
                    } else {
                        this.host.addClass(g.toThemeProperty("jqx-widget-header"))
                    }
                    if (this.mode == "popup") {
                        var b = this.host.height();
                        this.host.css("position", "absolute");
                        this.host.css("top", "0");
                        this.host.css("left", "0");
                        if (this.mode != "simple") {
                            this.host.height(b);
                            this.host.css("display", "none")
                        }
                    }
                    break
            }
            var c = this.isTouchDevice();
            if (this.autoCloseOnClick) {
                this.removeHandler(a(document), "mousedown.menu" + this.element.id, g._closeAfterClick);
                this.addHandler(a(document), "mousedown.menu" + this.element.id, g._closeAfterClick, g);
                if (c) {
                    this.removeHandler(a(document), a.jqx.mobile.getTouchEventName("touchstart") + ".menu" + this.element.id, g._closeAfterClick, g);
                    this.addHandler(a(document), a.jqx.mobile.getTouchEventName("touchstart") + ".menu" + this.element.id, g._closeAfterClick, g)
                }
            }
        },
        _getBodyOffset: function() {
            var c = 0;
            var b = 0;
            if (a("body").css("border-top-width") != "0px") {
                c = parseInt(a("body").css("border-top-width"));
                if (isNaN(c)) {
                    c = 0
                }
            }
            if (a("body").css("border-left-width") != "0px") {
                b = parseInt(a("body").css("border-left-width"));
                if (isNaN(b)) {
                    b = 0
                }
            }
            return {
                left: b,
                top: c
            }
        },
        _getOffset: function(c) {
            var e = a.jqx.mobile.isSafariMobileBrowser();
            var h = a(c).coord(true);
            var g = h.top;
            var f = h.left;
            if (a("body").css("border-top-width") != "0px") {
                g = parseInt(g) + this._getBodyOffset().top
            }
            if (a("body").css("border-left-width") != "0px") {
                f = parseInt(f) + this._getBodyOffset().left
            }
            var d = a.jqx.mobile.isWindowsPhone();
            if (this.hasTransform || (e != null && e) || d) {
                var b = {
                    left: a.jqx.mobile.getLeftPos(c),
                    top: a.jqx.mobile.getTopPos(c)
                };
                return b
            } else {
                return {
                    left: f,
                    top: g
                }
            }
        },
        _isRightClick: function(c) {
            var b;
            if (!c) {
                var c = window.event
            }
            if (c.which) {
                b = (c.which == 3)
            } else {
                if (c.button) {
                    b = (c.button == 2)
                }
            }
            return b
        },
        _openContextMenu: function(d) {
            var c = this;
            var b = c._isRightClick(d);
            if (b) {
                c.open(parseInt(d.clientX) + 5, parseInt(d.clientY) + 5)
            }
        },
        close: function() {
            var c = this;
            var d = a.data(this.element, "contextMenuOpened" + this.element.id);
            if (d) {
                var b = this.host;
                a.each(c.items, function() {
                    var e = this;
                    if (e.hasItems) {
                        c._closeItem(c, e)
                    }
                });
                a.each(c.items, function() {
                    var e = this;
                    if (e.isOpen == true) {
                        $submenu = a(e.subMenuElement);
                        var f = $submenu.closest("div.jqx-menu-popup");
                        f.hide(this.animationHideDuration)
                    }
                });
                this.host.hide(this.animationHideDuration);
                a.data(c.element, "contextMenuOpened" + this.element.id, false);
                c._raiseEvent("1", c)
            }
        },
        open: function(e, d) {
            if (this.mode == "popup") {
                var c = 0;
                if (this.host.css("display") == "block") {
                    this.close();
                    c = this.animationHideDuration
                }
                var b = this;
                if (e == undefined || e == null) {
                    e = 0
                }
                if (d == undefined || d == null) {
                    d = 0
                }
                setTimeout(function() {
                    b.host.show(b.animationShowDuration);
                    b.host.css("visibility", "visible");
                    a.data(b.element, "contextMenuOpened" + b.element.id, true);
                    b._raiseEvent("0", b);
                    b.host.css("z-index", 9999);
                    if (e != undefined && d != undefined) {
                        b.host.css({
                            left: e,
                            top: d
                        })
                    }
                }, c)
            }
        },
        _renderHover: function(c, e, b) {
            var d = this;
            if (!e.ignoretheme) {
                this.addHandler(c, "mouseenter", function() {
                    d.hoveredItem = e;
                    if (!e.disabled && !e.separator && d.enableHover && !d.disabled) {
                        if (e.level > 0) {
                            c.addClass(d.toThemeProperty("jqx-fill-state-hover"));
                            c.addClass(d.toThemeProperty("jqx-menu-item-hover"))
                        } else {
                            c.addClass(d.toThemeProperty("jqx-fill-state-hover"));
                            c.addClass(d.toThemeProperty("jqx-menu-item-top-hover"))
                        }
                    }
                });
                this.addHandler(c, "mouseleave", function() {
                    if (!e.disabled && !e.separator && d.enableHover && !d.disabled) {
                        if (e.level > 0) {
                            c.removeClass(d.toThemeProperty("jqx-fill-state-hover"));
                            c.removeClass(d.toThemeProperty("jqx-menu-item-hover"))
                        } else {
                            c.removeClass(d.toThemeProperty("jqx-fill-state-hover"));
                            c.removeClass(d.toThemeProperty("jqx-menu-item-top-hover"))
                        }
                    }
                })
            }
        },
        _closeAfterClick: function(c) {
            var b = c != null ? c.data : this;
            var d = false;
            if (b.autoCloseOnClick) {
                a.each(a(c.target).parents(), function() {
                    if (this.className.indexOf) {
                        if (this.className.indexOf("jqx-menu") != -1) {
                            d = true;
                            return false
                        }
                    }
                });
                if (!d) {
                    c.data = b;
                    b._closeAll(c)
                }
            }
        },
        _autoSizeHorizontalMenuItems: function() {
            var c = this;
            if (c.autoSizeMainItems && this.mode == "horizontal") {
                var b = this.maxHeight;
                if (parseInt(b) > parseInt(this.host.height())) {
                    b = parseInt(this.host.height())
                }
                b = parseInt(this.host.height());
                a.each(this.items, function() {
                    var l = this;
                    $element = a(l.element);
                    if (l.level == 0 && b > 0) {
                        var d = $element.children().length > 0 ? parseInt($element.children().height()) : $element.height();
                        var g = c.host.find("ul:first");
                        var h = parseInt(g.css("padding-top"));
                        var m = parseInt(g.css("margin-top"));
                        var j = b - 2 * (m + h);
                        var i = parseInt(j) / 2 - d / 2;
                        var e = parseInt(i);
                        var k = parseInt(i);
                        $element.css("padding-top", e);
                        $element.css("padding-bottom", k);
                        if (parseInt($element.outerHeight()) > j) {
                            var f = 1;
                            $element.css("padding-top", e - f);
                            e = e - f
                        }
                    }
                })
            }
            a.each(this.items, function() {
                var f = this;
                $element = a(f.element);
                if (f.hasItems && f.level > 0) {
                    if (f.arrow) {
                        var e = a(f.arrow);
                        var d = a(f.element).height();
                        if (d > 15) {
                            e.css("margin-top", (d - 15) / 2)
                        }
                    }
                }
            })
        },
        _nextVisibleItem: function(c, d) {
            if (c == null || c == undefined) {
                return null
            }
            var b = c;
            while (b != null) {
                b = b.nextItem;
                if (this._isVisible(b) && !b.disabled && b.type !== "separator") {
                    if (this.minimized) {
                        return b
                    }
                    if (d != undefined) {
                        if (b && b.level != d) {
                            continue
                        }
                    }
                    return b
                }
            }
            return null
        },
        _prevVisibleItem: function(c, d) {
            if (c == null || c == undefined) {
                return null
            }
            var b = c;
            while (b != null) {
                b = b.prevItem;
                if (this._isVisible(b) && !b.disabled && b.type !== "separator") {
                    if (this.minimized) {
                        return b
                    }
                    if (d != undefined) {
                        if (b && b.level != d) {
                            continue
                        }
                    }
                    return b
                }
            }
            return null
        },
        _parentItem: function(d) {
            if (d == null || d == undefined) {
                return null
            }
            var c = d.parentElement;
            if (!c) {
                return null
            }
            var b = null;
            a.each(this.items, function() {
                if (this.element == c) {
                    b = this;
                    return false
                }
            });
            return b
        },
        _isElementVisible: function(b) {
            if (b == null) {
                return false
            }
            if (a(b).css("display") != "none" && a(b).css("visibility") != "hidden") {
                return true
            }
            return false
        },
        _isVisible: function(c) {
            if (c == null || c == undefined) {
                return false
            }
            if (!this._isElementVisible(c.element)) {
                return false
            }
            var b = this._parentItem(c);
            if (b == null) {
                return true
            }
            if (this.minimized) {
                return true
            }
            if (b != null) {
                if (!this._isElementVisible(b.element)) {
                    return false
                }
                if (b.isOpen || this.minimized) {
                    while (b != null) {
                        b = this._parentItem(b);
                        if (b != null && !this._isElementVisible(b.element)) {
                            return false
                        }
                        if (b != null && !b.isOpen) {
                            return false
                        }
                    }
                } else {
                    return false
                }
            }
            return true
        },
        _render: function(f, g) {
            if (this.disabled) {
                this.host.addClass(this.toThemeProperty("jqx-fill-state-disabled"));
                this.host.addClass(this.toThemeProperty("jqx-menu-disabled"))
            }
            if (this.host.attr("tabindex") == undefined) {
                this.host.attr("tabindex", 0)
            }
            var i = this.popupZIndex;
            var d = [5, 5];
            var h = this;
            a.data(h.element, "animationHideDelay", h.animationHideDelay);
            var e = this.isTouchDevice();
            var c = e && (a.jqx.mobile.isWindowsPhone() || navigator.userAgent.indexOf("Touch") >= 0);
            var j = false;
            if (navigator.platform.toLowerCase().indexOf("win") != -1) {
                if (navigator.userAgent.indexOf("Windows Phone") >= 0 || navigator.userAgent.indexOf("WPDesktop") >= 0 || navigator.userAgent.indexOf("IEMobile") >= 0 || navigator.userAgent.indexOf("ZuneWP7") >= 0) {
                    this.touchDevice = true
                } else {
                    if (navigator.userAgent.indexOf("Touch") >= 0) {
                        var b = ("MSPointerDown" in window);
                        if (b || a.jqx.mobile.isWindowsPhone() || navigator.userAgent.indexOf("ARM") >= 0) {
                            j = true;
                            c = true;
                            h.clickToOpen = true;
                            h.autoCloseOnClick = false;
                            h.enableHover = false
                        }
                    }
                }
            }
            a.data(document.body, "menuel", this);
            this.hasTransform = a.jqx.utilities.hasTransform(this.host);
            this._applyOrientation(f, g);
            this.removeHandler(this.host, "blur");
            this.removeHandler(this.host, "focus");
            this.addHandler(this.host, "blur", function(k) {
                if (h.keyboardNavigation) {
                    if (h.activeItem) {
                        a(h.activeItem.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                        h.activeItem = null
                    }
                }
            });
            this.addHandler(this.host, "focus", function(k) {
                if (h.keyboardNavigation) {
                    if (!h.activeItem) {
                        if (h.hoveredItem) {
                            a(h.hoveredItem.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                            h.activeItem = h.hoveredItem
                        } else {
                            a(h.items[0].element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                            h.activeItem = h.items[0]
                        }
                    }
                }
            });
            this.removeHandler(this.host, "keydown");
            this.addHandler(this.host, "keydown", function(k) {
                if (h.keyboardNavigation) {
                    if (k.target.nodeName.toLowerCase() === "input") {
                        return true
                    }
                    var q = null;
                    var o = null;
                    a.each(h.items, function() {
                        var A = this;
                        if (this.disabled) {
                            return true
                        }
                        if (this.element.className.indexOf("pressed") >= 0) {
                            o = this
                        }
                        if (this.element.className.indexOf("focus") >= 0) {
                            q = this;
                            return false
                        }
                    });
                    if (!q && o) {
                        q = o
                    }
                    if (!q) {
                        a(h.items[0].element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                        h.activeItem = h.items[0]
                    }
                    var t = false;
                    if (k.keyCode == 27) {
                        k.data = h;
                        h._closeAll(k);
                        if (q) {
                            var z = q;
                            while (z != null) {
                                if (z.parentItem) {
                                    z = z.parentItem
                                } else {
                                    a(h.activeItem.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    h.activeItem = z;
                                    a(h.activeItem.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    z = z.parentItem
                                }
                            }
                        }
                        t = true
                    }
                    if (k.keyCode == 13) {
                        if (q) {
                            t = true;
                            h._raiseEvent("2", {
                                item: q.element,
                                event: k
                            });
                            var r = q.anchor != null ? a(q.anchor) : null;
                            if (r != null && r.length > 0) {
                                var l = r.attr("href");
                                var u = r.attr("target");
                                if (l != null) {
                                    if (u != null) {
                                        window.open(l, u)
                                    } else {
                                        window.location = l
                                    }
                                }
                            }
                            k.preventDefault();
                            k.stopPropagation();
                            a(q.element).focus()
                        }
                    }
                    var n = function(D) {
                        if (D == null) {
                            return new Array()
                        }
                        var C = new Array();
                        var A = 0;
                        for (var B = 0; B < h.items.length; B++) {
                            if (h.items[B].parentId == D.parentId) {
                                C[A++] = h.items[B]
                            }
                        }
                        return C
                    };
                    var v = "";
                    switch (k.keyCode) {
                        case 40:
                            v = "down";
                            break;
                        case 38:
                            v = "up";
                            break;
                        case 39:
                            v = "right";
                            break;
                        case 37:
                            v = "left";
                            break
                    }
                    if (q && q.openHorizontalDirection === "left" && v === "left") {
                        v = "right"
                    }
                    if (q && q.openHorizontalDirection === "left" && v === "right") {
                        v = "left"
                    }
                    if (q && q.openVerticalDirection === "top" && v === "top") {
                        v = "bottom"
                    }
                    if (q && q.openVerticalDirection === "top" && v === "bottom") {
                        v = "top"
                    }
                    if (h.rtl) {
                        if (v === "right") {
                            v = "left"
                        } else {
                            if (v === "left") {
                                v = "right"
                            }
                        }
                    }
                    if (v === "right" && !h.minimized) {
                        if (k.altKey && (q.level != 0 && q.hasItems || h.mode != "horizontal")) {
                            h._openItem(h, q)
                        } else {
                            var x = h._nextVisibleItem(q, 0);
                            var m = h._nextVisibleItem(q);
                            var w = n(m);
                            if (!x) {
                                x = m
                            }
                            if (x && ((x.parentId === q.parentId && x.level == 0 && h.mode == "horizontal") || (m.id == w[0].id && m.level != 0))) {
                                if (m.id == w[0].id && ((q.level != 0) || (q.level == 0 && h.mode != "horizontal"))) {
                                    x = m
                                }
                                a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                h.activeItem = x
                            }
                        }
                        k.preventDefault();
                        k.stopPropagation()
                    }
                    if (v === "left" && !h.minimized) {
                        if (k.altKey && ((q.level != 0 && h.mode !== "horizontal") || (q.level > 1 && h.mode === "horizontal") || (q.level == 1 && q.hasItems && h.mode === "horizontal"))) {
                            if (q.hasItems) {
                                h._closeItem(h, q)
                            } else {
                                if (q.parentItem) {
                                    h._closeItem(h, q.parentItem);
                                    a(q.parentItem.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    h.activeItem = q.parentItem
                                }
                            }
                        } else {
                            var x = h._prevVisibleItem(q, 0);
                            var y = q.parentItem;
                            if (x && (x.parentId === q.parentId && x.level == 0 && h.mode == "horizontal")) {
                                a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                h.activeItem = x
                            } else {
                                if (!(y && y.level == 0 && h.mode == "horizontal") && y && y.level == q.level - 1) {
                                    a(y.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    h.activeItem = y
                                }
                            }
                        }
                        k.preventDefault();
                        k.stopPropagation()
                    }
                    if (v === "down") {
                        if (k.altKey) {
                            if (q.level == 0 && q.hasItems) {
                                h._openItem(h, q)
                            }
                            if (h.minimized) {
                                if (h.minimizedHidden) {
                                    h.minimizedItem.trigger("click")
                                }
                            }
                        } else {
                            var x = h._nextVisibleItem(q, q.level);
                            var w = n(x);
                            if (h.minimized && x) {
                                a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                h.activeItem = x
                            } else {
                                if (x && (x.parentId === q.parentId || (x.id == w[0].id && h.mode == "horizontal"))) {
                                    if (!(x.level == 0 && h.mode == "horizontal")) {
                                        a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                        a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                        h.activeItem = x
                                    }
                                }
                                if (h.mode === "horizontal" && q.level === 0 && q.isOpen && q.hasItems) {
                                    var x = h._nextVisibleItem(q);
                                    a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    h.activeItem = x
                                }
                            }
                        }
                        k.preventDefault();
                        k.stopPropagation()
                    } else {
                        if (v === "up") {
                            if (k.altKey) {
                                if (q.parentItem && q.parentItem.level == 0) {
                                    h._closeItem(h, q.parentItem);
                                    a(q.parentItem.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    h.activeItem = q.parentItem
                                } else {
                                    if (q.parentItem === null && q.level === 0 && h.mode === "horizontal") {
                                        h._closeItem(h, q)
                                    }
                                }
                                if (h.minimized) {
                                    if (!h.minimizedHidden) {
                                        h.minimizedItem.trigger("click")
                                    }
                                }
                            } else {
                                var x = h._prevVisibleItem(q, q.level);
                                var w = n(q);
                                if (h.minimized && x) {
                                    a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    h.activeItem = x
                                } else {
                                    if (x && (x.parentId === q.parentId || (x.id == q.parentId && x.level == 0 && h.mode == "horizontal"))) {
                                        if (!(x.level == 0 && h.mode === "horizontal" && q.level === 0)) {
                                            a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                            a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                            h.activeItem = x
                                        }
                                    } else {
                                        if (q && q.id == w[0].id && q.parentItem && q.parentItem.level === 0 && h.mode === "horizontal") {
                                            var x = q.parentItem;
                                            a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                            a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                            h.activeItem = x
                                        }
                                    }
                                }
                            }
                            k.preventDefault();
                            k.stopPropagation()
                        }
                    }
                    if (k.keyCode == 9) {
                        var x = k.shiftKey ? h._prevVisibleItem(q) : h._nextVisibleItem(q);
                        if (x) {
                            a(x.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                            a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                            h.activeItem = x;
                            k.preventDefault();
                            k.stopPropagation()
                        } else {
                            if (h.lockFocus) {
                                var w = new Array();
                                var s = 0;
                                for (var p = 0; p < h.items.length; p++) {
                                    if (h.items[p] == q) {
                                        continue
                                    }
                                    if (h.items[p].parentId == q.parentId) {
                                        w[s++] = h.items[p]
                                    }
                                }
                                if (w.length > 0) {
                                    if (k.shiftKey) {
                                        a(w[w.length - 1].element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                        h.activeItem = w[w.length - 1]
                                    } else {
                                        a(w[0].element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                                        h.activeItem = w[0]
                                    }
                                    a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"))
                                }
                                k.preventDefault();
                                k.stopPropagation()
                            } else {
                                if (q) {
                                    a(q.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"));
                                    h.activeItem = null
                                }
                            }
                        }
                    }
                }
            });
            if (h.enableRoundedCorners) {
                this.host.addClass(h.toThemeProperty("jqx-rc-all"))
            }
            a.each(this.items, function() {
                var r = this;
                var n = a(r.element);
                n.attr("role", "menuitem");
                if (h.enableRoundedCorners) {
                    n.addClass(h.toThemeProperty("jqx-rc-all"))
                }
                h.removeHandler(n, "click");
                h.addHandler(n, "click", function(w) {
                    if (r.disabled) {
                        return
                    }
                    if (h.disabled) {
                        return
                    }
                    if (h.keyboardNavigation) {
                        if (h.activeItem) {
                            a(h.activeItem.element).removeClass(h.toThemeProperty("jqx-fill-state-focus"))
                        }
                        h.activeItem = r;
                        a(r.element).addClass(h.toThemeProperty("jqx-fill-state-focus"));
                        if (h.minimized) {
                            w.stopPropagation()
                        }
                    }
                    h._raiseEvent("2", {
                        item: r.element,
                        event: w
                    });
                    if (!h.autoOpen) {
                        if (r.level > 0) {
                            if (h.autoCloseOnClick && !e && !h.clickToOpen) {
                                w.data = h;
                                h._closeAll(w)
                            }
                        }
                    } else {
                        if (h.autoCloseOnClick && !e && !h.clickToOpen) {
                            if (r.closeOnClick) {
                                w.data = h;
                                h._closeAll(w)
                            }
                        }
                    }
                    if (e && h.autoCloseOnClick) {
                        w.data = h;
                        if (!r.hasItems) {
                            h._closeAll(w)
                        }
                    }
                    if (w.target.tagName != "A" && w.target.tagName != "a") {
                        var u = r.anchor != null ? a(r.anchor) : null;
                        if (u != null && u.length > 0) {
                            var t = u.attr("href");
                            var v = u.attr("target");
                            if (t != null) {
                                if (v != null) {
                                    window.open(t, v)
                                } else {
                                    window.location = t
                                }
                            }
                        }
                    }
                });
                h.removeHandler(n, "mouseenter");
                h.removeHandler(n, "mouseleave");
                if (!c && h.mode != "simple") {
                    h._renderHover(n, r, e)
                }
                if (r.subMenuElement != null) {
                    var o = a(r.subMenuElement);
                    if (h.mode == "simple") {
                        o.show();
                        return true
                    }
                    o.wrap('<div class="jqx-menu-popup ' + h.toThemeProperty("jqx-menu-popup") + '" style="border: none; background-color: transparent; z-index:' + i + '; padding: 0px; margin: 0px; position: absolute; top: 0; left: 0; display: block; visibility: hidden;"><div style="background-color: transparent; border: none; position:absolute; overflow:hidden; left: 0; top: 0; right: 0; width: 100%; height: 100%;"></div></div>');
                    o.css({
                        overflow: "hidden",
                        position: "absolute",
                        left: 0,
                        display: "inherit",
                        top: -o.outerHeight()
                    });
                    o.data("timer", {});
                    if (r.level > 0) {
                        o.css("left", -o.outerWidth())
                    } else {
                        if (h.mode == "horizontal") {
                            o.css("left", 0)
                        }
                    }
                    i++;
                    var q = a(r.subMenuElement).closest("div.jqx-menu-popup").css({
                        width: parseInt(a(r.subMenuElement).outerWidth()) + parseInt(d[0]) + "px",
                        height: parseInt(a(r.subMenuElement).outerHeight()) + parseInt(d[1]) + "px"
                    });
                    var s = n.closest("div.jqx-menu-popup");
                    if (s.length > 0) {
                        var k = o.css("margin-left");
                        var m = o.css("margin-right");
                        var l = o.css("padding-left");
                        var p = o.css("padding-right");
                        q.appendTo(s);
                        o.css("margin-left", k);
                        o.css("margin-right", m);
                        o.css("padding-left", l);
                        o.css("padding-right", p)
                    } else {
                        var k = o.css("margin-left");
                        var m = o.css("margin-right");
                        var l = o.css("padding-left");
                        var p = o.css("padding-right");
                        q.appendTo(a(document.body));
                        o.css("margin-left", k);
                        o.css("margin-right", m);
                        o.css("padding-left", l);
                        o.css("padding-right", p)
                    }
                    if (!h.clickToOpen) {
                        if (e || c) {
                            h.removeHandler(n, a.jqx.mobile.getTouchEventName("touchstart"));
                            h.addHandler(n, a.jqx.mobile.getTouchEventName("touchstart"), function(t) {
                                clearTimeout(o.data("timer").hide);
                                if (o != null) {
                                    o.stop()
                                }
                                if (r.level == 0 && !r.isOpen && h.mode != "popup") {
                                    t.data = h;
                                    h._closeAll(t)
                                }
                                if (!r.isOpen) {
                                    h._openItem(h, r)
                                } else {
                                    h._closeItem(h, r, true)
                                }
                                return false
                            })
                        }
                        if (!c) {
                            h.addHandler(n, "mouseenter", function() {
                                if (h.autoOpen || (r.level > 0 && !h.autoOpen)) {
                                    clearTimeout(o.data("timer").hide)
                                }
                                if (r.parentId && r.parentId != 0) {
                                    if (h.menuElements[r.parentId]) {
                                        var t = h.menuElements[r.parentId].isOpen;
                                        if (!t) {
                                            return
                                        }
                                    }
                                }
                                if (h.autoOpen || (r.level > 0 && !h.autoOpen)) {
                                    h._openItem(h, r)
                                }
                                return false
                            });
                            h.addHandler(n, "mousedown", function() {
                                if (!h.autoOpen && r.level == 0) {
                                    clearTimeout(o.data("timer").hide);
                                    if (o != null) {
                                        o.stop()
                                    }
                                    if (!r.isOpen) {
                                        h._openItem(h, r)
                                    } else {
                                        h._closeItem(h, r, true)
                                    }
                                }
                            });
                            h.addHandler(n, "mouseleave", function(u) {
                                if (h.autoCloseOnMouseLeave) {
                                    clearTimeout(o.data("timer").hide);
                                    var x = a(r.subMenuElement);
                                    var t = {
                                        left: parseInt(u.pageX),
                                        top: parseInt(u.pageY)
                                    };
                                    var w = {
                                        left: parseInt(x.coord().left),
                                        top: parseInt(x.coord().top),
                                        width: parseInt(x.outerWidth()),
                                        height: parseInt(x.outerHeight())
                                    };
                                    var v = true;
                                    if (w.left - 5 <= t.left && t.left <= w.left + w.width + 5) {
                                        if (w.top <= t.top && t.top <= w.top + w.height) {
                                            v = false
                                        }
                                    }
                                    if (v) {
                                        h._closeItem(h, r, true)
                                    }
                                }
                            });
                            h.removeHandler(q, "mouseenter");
                            h.addHandler(q, "mouseenter", function() {
                                clearTimeout(o.data("timer").hide)
                            });
                            h.removeHandler(q, "mouseleave");
                            h.addHandler(q, "mouseleave", function(t) {
                                if (h.autoCloseOnMouseLeave) {
                                    clearTimeout(o.data("timer").hide);
                                    clearTimeout(o.data("timer").show);
                                    if (o != null) {
                                        o.stop()
                                    }
                                    h._closeItem(h, r, true)
                                }
                            })
                        }
                    } else {
                        h.removeHandler(n, "mousedown");
                        h.addHandler(n, "mousedown", function(t) {
                            clearTimeout(o.data("timer").hide);
                            if (o != null) {
                                o.stop()
                            }
                            if (r.level == 0 && !r.isOpen) {
                                t.data = h;
                                h._closeAll(t)
                            }
                            if (!r.isOpen) {
                                h._openItem(h, r)
                            } else {
                                h._closeItem(h, r, true)
                            }
                        })
                    }
                }
            });
            if (this.mode == "simple") {
                this._renderSimpleMode()
            }
            this._autoSizeHorizontalMenuItems();
            this._raiseEvent("3", this)
        },
        _renderSimpleMode: function() {
            this.host.show()
        },
        createID: function() {
            var b = Math.random() + "";
            b = b.replace(".", "");
            b = "99" + b;
            b = b / 1;
            while (this.items[b]) {
                b = Math.random() + "";
                b = b.replace(".", "");
                b = b / 1
            }
            return "menuItem" + b
        },
        _createMenu: function(c, f) {
            if (c == null) {
                return
            }
            if (f == undefined) {
                f = true
            }
            if (f == null) {
                f = true
            }
            var o = this;
            var u = a(c).find("li");
            var q = 0;
            this.itemMapping = new Array();
            for (var j = 0; j < u.length; j++) {
                var m = u[j];
                var s = a(m);
                if (m.className.indexOf("jqx-menu") == -1 && this.autoGenerate == false) {
                    continue
                }
                var p = m.id;
                if (!p) {
                    p = this.createID()
                }
                if (f) {
                    m.id = p;
                    this.items[q] = new a.jqx._jqxMenu.jqxMenuItem();
                    this.menuElements[p] = this.items[q]
                }
                q += 1;
                var t = 0;
                var x = this;
                var h = s.children();
                h.each(function() {
                    if (!f) {
                        this.className = "";
                        if (x.autoGenerate) {
                            a(x.items[q - 1].subMenuElement)[0].className = "";
                            if (!x.minimized) {
                                a(x.items[q - 1].subMenuElement).addClass(x.toThemeProperty("jqx-widget-content"))
                            }
                            a(x.items[q - 1].subMenuElement).addClass(x.toThemeProperty("jqx-menu-dropdown"));
                            a(x.items[q - 1].subMenuElement).addClass(x.toThemeProperty("jqx-popup"))
                        }
                    }
                    if (this.className.indexOf("jqx-menu-dropdown") != -1) {
                        if (f) {
                            x.items[q - 1].subMenuElement = this
                        }
                        return false
                    } else {
                        if (x.autoGenerate && (this.tagName == "ul" || this.tagName == "UL")) {
                            if (f) {
                                x.items[q - 1].subMenuElement = this
                            }
                            this.className = "";
                            if (!x.minimized) {
                                a(this).addClass(x.toThemeProperty("jqx-widget-content"))
                            }
                            a(this).addClass(x.toThemeProperty("jqx-menu-dropdown"));
                            a(this).addClass(x.toThemeProperty("jqx-popup"));
                            a(this).attr("role", "menu");
                            if (x.rtl) {
                                a(this).addClass(x.toThemeProperty("jqx-rc-l"))
                            } else {
                                a(this).addClass(x.toThemeProperty("jqx-rc-r"))
                            }
                            a(this).addClass(x.toThemeProperty("jqx-rc-b"));
                            return false
                        }
                    }
                });
                var w = s.parents();
                w.each(function() {
                    if (this.className.indexOf("jqx-menu-item") != -1) {
                        t = this.id;
                        return false
                    } else {
                        if (x.autoGenerate && (this.tagName == "li" || this.tagName == "LI")) {
                            t = this.id;
                            return false
                        }
                    }
                });
                var e = false;
                var d = m.getAttribute("type");
                var b = m.getAttribute("ignoretheme") || m.getAttribute("data-ignoretheme");
                if (b) {
                    if (b == "true" || b == true) {
                        b = true
                    }
                } else {
                    b = false
                }
                if (!d) {
                    d = m.type
                } else {
                    if (d == "separator") {
                        var e = true
                    }
                }
                if (!e) {
                    if (t) {
                        d = "sub"
                    } else {
                        d = "top"
                    }
                }
                var g = this.items[q - 1];
                if (f) {
                    g.id = p;
                    g.parentId = t;
                    g.type = d;
                    g.separator = e;
                    g.element = u[j];
                    var l = s.children("a");
                    g.disabled = m.getAttribute("item-disabled") == "true" ? true : false;
                    g.level = s.parents("li").length;
                    g.anchor = l.length > 0 ? l : null;
                    if (g.anchor) {
                        a(g.anchor).attr("tabindex", -1)
                    }
                }
                g.ignoretheme = b;
                var n = this.menuElements[t];
                if (n != null) {
                    if (n.ignoretheme) {
                        g.ignoretheme = n.ignoretheme;
                        b = n.ignoretheme
                    }
                    g.parentItem = n;
                    g.parentElement = n.element
                }
                if (this.autoGenerate) {
                    if (d == "separator") {
                        s.removeClass();
                        s.addClass(this.toThemeProperty("jqx-menu-item-separator"));
                        s.attr("role", "separator")
                    } else {
                        if (!b) {
                            s[0].className = "";
                            if (this.rtl) {
                                s.addClass(this.toThemeProperty("jqx-rtl"))
                            }
                            if (g.level > 0 && !x.minimized) {
                                s.addClass(this.toThemeProperty("jqx-item"));
                                s.addClass(this.toThemeProperty("jqx-menu-item"))
                            } else {
                                s.addClass(this.toThemeProperty("jqx-item"));
                                s.addClass(this.toThemeProperty("jqx-menu-item-top"))
                            }
                        }
                    }
                }
                if (g.disabled) {
                    s.addClass(x.toThemeProperty("jqx-menu-item-disabled"));
                    s.addClass(x.toThemeProperty("jqx-fill-state-disabled"))
                }
                this.itemMapping[j] = {
                    element: u[j],
                    item: g
                };
                this.itemMapping["id" + u[j].id] = this.itemMapping[j];
                if (f && !b) {
                    g.hasItems = s.find("li").length > 0;
                    if (g.hasItems) {
                        if (g.element) {
                            a.jqx.aria(a(g.element), "aria-haspopup", true);
                            if (!g.subMenuElement.id) {
                                g.subMenuElement.id = a.jqx.utilities.createId()
                            }
                            a.jqx.aria(a(g.element), "aria-owns", g.subMenuElement.id)
                        }
                    }
                }
            }
            for (var r = 0; r < u.length; r++) {
                var v = u[r];
                if (this.itemMapping["id" + v.id]) {
                    var g = this.itemMapping["id" + v.id].item;
                    if (!g) {
                        continue
                    }
                    g.prevItem = null;
                    g.nextItem = null;
                    if (r > 0) {
                        if (this.itemMapping["id" + u[r - 1].id]) {
                            g.prevItem = this.itemMapping["id" + u[r - 1].id].item
                        }
                    }
                    if (r < u.length - 1) {
                        if (this.itemMapping["id" + u[r + 1].id]) {
                            g.nextItem = this.itemMapping["id" + u[r + 1].id].item
                        }
                    }
                }
            }
        },
        destroy: function() {
            a.jqx.utilities.resize(this.host, null, true);
            var d = this.host.closest("div.jqx-menu-wrapper");
            d.remove();
            a("#menuWrapper" + this.element.id).remove();
            var b = this;
            this.removeHandler(this.host, "keydown");
            this.removeHandler(this.host, "focus");
            this.removeHandler(this.host, "blur");
            this.removeHandler(a(document), "mousedown.menu" + this.element.id, b._closeAfterClick);
            this.removeHandler(a(document), "mouseup.menu" + this.element.id, b._closeAfterClick);
            a.data(document.body, "jqxMenuOldHost" + this.element.id, null);
            if (this.isTouchDevice()) {
                this.removeHandler(a(document), a.jqx.mobile.getTouchEventName("touchstart") + ".menu" + this.element.id, this._closeAfterClick, this)
            }
            if (a(window).off) {
                a(window).off("resize.menu" + b.element.id)
            }
            a.each(this.items, function() {
                var g = this;
                var f = a(g.element);
                b.removeHandler(f, "click");
                b.removeHandler(f, "selectstart");
                b.removeHandler(f, "mouseenter");
                b.removeHandler(f, "mouseleave");
                b.removeHandler(f, "mousedown");
                b.removeHandler(f, "mouseleave");
                var e = a(g.subMenuElement);
                var h = e.closest("div.jqx-menu-popup");
                h.remove();
                delete this.subMenuElement;
                delete this.element
            });
            a.data(document.body, "menuel", null);
            delete this.menuElements;
            this.items = new Array();
            delete this.items;
            var c = a.data(this.element, "jqxMenu");
            if (c) {
                delete c.instance
            }
            this.host.removeClass();
            this.host.remove();
            delete this.host;
            delete this.element
        },
        _raiseEvent: function(f, c) {
            if (c == undefined) {
                c = {
                    owner: null
                }
            }
            var d = this.events[f];
            args = c;
            args.owner = this;
            var e = new a.Event(d);
            if (f == "2") {
                args = c.item;
                args.owner = this;
                a.extend(e, c.event);
                e.type = "itemclick"
            }
            e.owner = this;
            e.args = args;
            var b = this.host.trigger(e);
            return b
        },
        propertyChangedHandler: function(b, d, g, f) {
            if (this.isInitialized == undefined || this.isInitialized == false) {
                return
            }
            if (d == "disabled") {
                if (b.disabled) {
                    b.host.addClass(b.toThemeProperty("jqx-fill-state-disabled"));
                    b.host.addClass(b.toThemeProperty("jqx-menu-disabled"))
                } else {
                    b.host.removeClass(b.toThemeProperty("jqx-fill-state-disabled"));
                    b.host.removeClass(b.toThemeProperty("jqx-menu-disabled"))
                }
            }
            if (f == g) {
                return
            }
            if (d == "touchMode") {
                this._isTouchDevice = null;
                b._render(f, g)
            }
            if (d == "source") {
                if (b.source != null) {
                    var c = b.loadItems(b.source);
                    b.element.innerHTML = c;
                    var e = b.host.find("ul:first");
                    if (e.length > 0) {
                        b.refresh();
                        b._createMenu(e[0]);
                        b._render()
                    }
                }
            }
            if (d == "autoCloseOnClick") {
                if (f == false) {
                    b.removeHandler(a(document), "mousedown.menu" + this.element.id, b._closeAll)
                } else {
                    b.addHandler(a(document), "mousedown.menu" + this.element.id, b, b._closeAll)
                }
            } else {
                if (d == "mode" || d == "width" || d == "height" || d == "showTopLevelArrows") {
                    b.refresh();
                    if (d == "mode") {
                        b._render(f, g)
                    } else {
                        b._applyOrientation()
                    }
                } else {
                    if (d == "theme") {
                        a.jqx.utilities.setTheme(g, f, b.host)
                    }
                }
            }
        }
    })
})(jqxBaseFramework);
(function(a) {
    a.jqx._jqxMenu.jqxMenuItem = function(e, d, c) {
        var b = {
            id: e,
            parentId: d,
            parentItem: null,
            anchor: null,
            type: c,
            disabled: false,
            level: 0,
            isOpen: false,
            hasItems: false,
            element: null,
            subMenuElement: null,
            arrow: null,
            openHorizontalDirection: "right",
            openVerticalDirection: "down",
            closeOnClick: true
        };
        return b
    }
})(jqxBaseFramework);