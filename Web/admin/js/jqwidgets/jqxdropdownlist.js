/*
jQWidgets v3.6.0 (2014-Nov-25)
Copyright (c) 2011-2014 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a) {
    a.jqx.jqxWidget("jqxDropDownList", "", {});
    a.extend(a.jqx._jqxDropDownList.prototype, {
        defineInstance: function() {
            var b = {
                disabled: false,
                width: null,
                height: null,
                items: new Array(),
                selectedIndex: -1,
                source: null,
                scrollBarSize: 15,
                arrowSize: 19,
                enableHover: true,
                enableSelection: true,
                visualItems: new Array(),
                groups: new Array(),
                equalItemsWidth: true,
                itemHeight: -1,
                visibleItems: new Array(),
                emptyGroupText: "Group",
                checkboxes: false,
                openDelay: 250,
                closeDelay: 300,
                animationType: "default",
                autoOpen: false,
                dropDownWidth: "auto",
                dropDownHeight: "200px",
                autoDropDownHeight: false,
                keyboardSelection: true,
                enableBrowserBoundsDetection: false,
                dropDownHorizontalAlignment: "left",
                displayMember: "",
                valueMember: "",
                searchMode: "startswithignorecase",
                incrementalSearch: true,
                incrementalSearchDelay: 700,
                renderer: null,
                placeHolder: "Please Choose:",
                promptText: "Please Choose:",
                emptyString: "",
                rtl: false,
                selectionRenderer: null,
                listBox: null,
                popupZIndex: 9999999999999,
                renderMode: "default",
                touchMode: "auto",
                _checkForHiddenParent: true,
                autoBind: true,
                focusable: true,
                filterable: false,
                filterHeight: 27,
                filterPlaceHolder: "Looking for",
                filterDelay: 100,
                aria: {
                    "aria-disabled": {
                        name: "disabled",
                        type: "boolean"
                    }
                },
                events: ["open", "close", "select", "unselect", "change", "checkChange", "bindingComplete"]
            };
            a.extend(true, this, b);
            return b
        },
        createInstance: function(b) {
            this.render()
        },
        render: function() {
            var p = this;
            if (!p.width) {
                p.width = 200
            }
            if (!p.height) {
                p.height = 25
            }
            var n = p.element.nodeName.toLowerCase();
            if (n == "select" || n == "ul" || n == "ol") {
                p.field = p.element;
                if (p.field.className) {
                    p._className = p.field.className
                }
                var j = {
                    title: p.field.title
                };
                if (p.field.id.length) {
                    j.id = p.field.id.replace(/[^\w]/g, "_") + "_jqxDropDownList"
                } else {
                    j.id = a.jqx.utilities.createId() + "_jqxDropDownList"
                }
                var c = a("<div></div>", j);
                if (!p.width) {
                    p.width = a(p.field).width()
                }
                if (!p.height) {
                    p.height = a(p.field).outerHeight()
                }
                a(p.field).hide().after(c);
                p.host = c;
                p.element = c[0];
                p.element.id = p.field.id;
                p.field.id = j.id;
                if (p.field.tabIndex) {
                    var f = p.field.tabIndex;
                    p.field.tabIndex = -1;
                    p.element.tabIndex = f
                }
                var q = a.jqx.parseSourceTag(p.field);
                p.source = q.items;
                if (p.selectedIndex == -1) {
                    p.selectedIndex = q.index
                }
            }
            p.element.innerHTML = "";
            p.isanimating = false;
            p.id = p.element.id || a.jqx.utilities.createId();
            p.host.attr("role", "combobox");
            a.jqx.aria(p, "aria-autocomplete", "both");
            a.jqx.aria(p, "aria-readonly", false);
            var g = a("<div style='background-color: transparent; -webkit-appearance: none; outline: none; width:100%; height: 100%; padding: 0px; margin: 0px; border: 0px; position: relative;'><div id='dropdownlistWrapper' style='overflow: hidden; outline: none; background-color: transparent; border: none; float: left; width:100%; height: 100%; position: relative;'><div id='dropdownlistContent' unselectable='on' style='outline: none; background-color: transparent; border: none; float: left; position: relative;'/><div id='dropdownlistArrow' unselectable='on' style='background-color: transparent; border: none; float: right; position: relative;'><div unselectable='on'></div></div></div></div>");
            p._addInput();
            if (a.jqx._jqxListBox == null || a.jqx._jqxListBox == undefined) {
                throw new Error("jqxDropDownList: Missing reference to jqxlistbox.js.")
            }
            if (p.host.attr("tabindex")) {
                g.attr("tabindex", p.host.attr("tabindex"));
                p.host.removeAttr("tabindex")
            } else {
                g.attr("tabindex", 0)
            }
            var k = p;
            p.touch = a.jqx.mobile.isTouchDevice();
            p.comboStructure = g;
            p.host.append(g);
            p.dropdownlistWrapper = p.host.find("#dropdownlistWrapper");
            p.dropdownlistArrow = p.host.find("#dropdownlistArrow");
            p.arrow = a(p.dropdownlistArrow.children()[0]);
            p.dropdownlistContent = p.host.find("#dropdownlistContent");
            p.dropdownlistContent.addClass(p.toThemeProperty("jqx-dropdownlist-content"));
            p.dropdownlistWrapper.addClass(p.toThemeProperty("jqx-disableselect"));
            if (p.rtl) {
                p.dropdownlistContent.addClass(p.toThemeProperty("jqx-rtl"));
                p.dropdownlistContent.addClass(p.toThemeProperty("jqx-dropdownlist-content-rtl"))
            }
            p.addHandler(p.dropdownlistWrapper, "selectstart", function() {
                return false
            });
            p.dropdownlistWrapper[0].id = "dropdownlistWrapper" + p.element.id;
            p.dropdownlistArrow[0].id = "dropdownlistArrow" + p.element.id;
            p.dropdownlistContent[0].id = "dropdownlistContent" + p.element.id;
            if (p.promptText != "Please Choose:") {
                p.placeHolder = p.promptText
            }
            var m = p.toThemeProperty("jqx-widget") + " " + p.toThemeProperty("jqx-dropdownlist-state-normal") + " " + p.toThemeProperty("jqx-rc-all") + " " + p.toThemeProperty("jqx-fill-state-normal");
            p.element.className += " " + m;
            p._firstDiv = p.host.find("div:first");
            try {
                var o = "listBox" + p.id;
                var h = a(a.find("#" + o));
                if (h.length > 0) {
                    h.remove()
                }
                a.jqx.aria(p, "aria-owns", o);
                a.jqx.aria(p, "aria-haspopup", true);
                var b = a("<div style='overflow: hidden; background-color: transparent; border: none; position: absolute;' id='listBox" + p.id + "'><div id='innerListBox" + p.id + "'></div></div>");
                b.hide();
                b.appendTo(document.body);
                p.container = b;
                p.listBoxContainer = a(a.find("#innerListBox" + p.id));
                var d = p.width;
                if (p.dropDownWidth != "auto") {
                    d = p.dropDownWidth
                }
                if (d == null) {
                    d = p.host.width();
                    if (d == 0) {
                        d = p.dropDownWidth
                    }
                }
                if (p.dropDownHeight == null) {
                    p.dropDownHeight = 200
                }
                var k = p;
                p.container.width(parseInt(d) + 25);
                p.container.height(parseInt(p.dropDownHeight) + 25);
                p.addHandler(p.listBoxContainer, "bindingComplete", function(e) {
                    p._raiseEvent("6")
                });
                p.listBoxContainer.jqxListBox({
                    filterHeight: p.filterHeight,
                    filterPlaceHolder: p.filterPlaceHolder,
                    filterDelay: p.filterDelay,
                    filterable: p.filterable,
                    allowDrop: false,
                    allowDrag: false,
                    autoBind: p.autoBind,
                    _checkForHiddenParent: false,
                    focusable: p.focusable,
                    touchMode: p.touchMode,
                    checkboxes: p.checkboxes,
                    rtl: p.rtl,
                    emptyString: p.emptyString,
                    itemHeight: p.itemHeight,
                    width: d,
                    searchMode: p.searchMode,
                    incrementalSearch: p.incrementalSearch,
                    incrementalSearchDelay: p.incrementalSearchDelay,
                    displayMember: p.displayMember,
                    valueMember: p.valueMember,
                    height: p.dropDownHeight,
                    autoHeight: p.autoDropDownHeight,
                    scrollBarSize: p.scrollBarSize,
                    selectedIndex: p.selectedIndex,
                    source: p.source,
                    theme: p.theme,
                    rendered: function() {
                        if (p.selectedIndex != p.listBoxContainer.jqxListBox("selectedIndex")) {
                            p.listBox = a.data(p.listBoxContainer[0], "jqxListBox").instance;
                            p.listBoxContainer.jqxListBox({
                                selectedIndex: p.selectedIndex
                            });
                            p.renderSelection("mouse")
                        } else {
                            p.renderSelection("mouse")
                        }
                    },
                    renderer: p.renderer
                });
                p.listBoxContainer.css({
                    position: "absolute",
                    zIndex: p.popupZIndex,
                    top: 0,
                    left: 0
                });
                p.listBox = a.data(p.listBoxContainer[0], "jqxListBox").instance;
                p.listBox.enableSelection = p.enableSelection;
                p.listBox.enableHover = p.enableHover;
                p.listBox.equalItemsWidth = p.equalItemsWidth;
                p.listBox.selectIndex(p.selectedIndex);
                p.listBox._arrange();
                p.listBoxContainer.addClass(p.toThemeProperty("jqx-popup"));
                if (a.jqx.browser.msie) {
                    p.listBoxContainer.addClass(p.toThemeProperty("jqx-noshadow"))
                }
                p.addHandler(p.listBoxContainer, "unselect", function(e) {
                    p._raiseEvent("3", {
                        index: e.args.index,
                        type: e.args.type,
                        item: e.args.item
                    })
                });
                p.addHandler(p.listBoxContainer, "change", function(e) {
                    if (e.args) {
                        if (e.args.type != "keyboard") {
                            p._raiseEvent("4", {
                                index: e.args.index,
                                type: e.args.type,
                                item: e.args.item
                            })
                        } else {
                            if (e.args.type == "keyboard") {
                                if (!p.isOpened()) {
                                    p._raiseEvent("4", {
                                        index: p.selectedIndex,
                                        type: "keyboard",
                                        item: p.getItem(p.selectedIndex)
                                    })
                                }
                            }
                        }
                    }
                });
                if (p.animationType == "none") {
                    p.container.css("display", "none")
                } else {
                    p.container.hide()
                }
            } catch (i) {}
            var p = p;
            p.propertyChangeMap.disabled = function(e, s, r, t) {
                if (t) {
                    e.host.addClass(p.toThemeProperty("jqx-dropdownlist-state-disabled"));
                    e.host.addClass(p.toThemeProperty("jqx-fill-state-disabled"));
                    e.dropdownlistContent.addClass(p.toThemeProperty("jqx-dropdownlist-content-disabled"))
                } else {
                    e.host.removeClass(p.toThemeProperty("jqx-dropdownlist-state-disabled"));
                    e.host.removeClass(p.toThemeProperty("jqx-fill-state-disabled"));
                    e.dropdownlistContent.removeClass(p.toThemeProperty("jqx-dropdownlist-content-disabled"))
                }
                a.jqx.aria(e, "aria-disabled", e.disabled)
            };
            if (p.disabled) {
                p.host.addClass(p.toThemeProperty("jqx-dropdownlist-state-disabled"));
                p.host.addClass(p.toThemeProperty("jqx-fill-state-disabled"));
                p.dropdownlistContent.addClass(p.toThemeProperty("jqx-dropdownlist-content-disabled"))
            }
            p.arrow.addClass(p.toThemeProperty("jqx-icon-arrow-down"));
            p.arrow.addClass(p.toThemeProperty("jqx-icon"));
            if (p.renderMode === "simple") {
                p.arrow.remove();
                p.host.removeClass(p.toThemeProperty("jqx-fill-state-normal"));
                p.host.removeClass(p.toThemeProperty("jqx-rc-all"))
            }
            p._updateHandlers();
            p._setSize();
            p._arrange();
            if (p.listBox) {
                p.renderSelection()
            }
            if (a.jqx.browser.msie && a.jqx.browser.version < 8) {
                if (p.host.parents(".jqx-window").length > 0) {
                    var l = p.host.parents(".jqx-window").css("z-index");
                    b.css("z-index", l + 10);
                    p.listBoxContainer.css("z-index", l + 10)
                }
            }
        },
        resize: function(c, b) {
            this.width = c;
            this.height = b;
            this._setSize();
            this._arrange()
        },
        val: function(c) {
            if (!this.dropdownlistContent) {
                return ""
            }
            var d = function(f) {
                for (var e in f) {
                    if (f.hasOwnProperty(e)) {
                        return false
                    }
                }
                if (typeof c == "number") {
                    return false
                }
                if (typeof c == "date") {
                    return false
                }
                if (typeof c == "boolean") {
                    return false
                }
                if (typeof c == "string") {
                    return false
                }
                return true
            };
            if (this.input && (d(c) || arguments.length == 0)) {
                return this.input.val()
            }
            var b = this.getItemByValue(c);
            if (b != null) {
                this.selectItem(b)
            }
            if (this.input) {
                return this.input.val()
            }
        },
        focus: function() {
            try {
                var d = this;
                var c = function() {
                    d.host.focus();
                    if (d._firstDiv) {
                        d._firstDiv.focus()
                    }
                };
                c();
                setTimeout(function() {
                    c()
                }, 10)
            } catch (b) {}
        },
        _addInput: function() {
            var b = this.host.attr("name");
            this.input = a("<input type='hidden'/>");
            this.host.append(this.input);
            if (b) {
                this.input.attr("name", b)
            }
        },
        getItems: function() {
            if (!this.listBox) {
                return new Array()
            }
            return this.listBox.items
        },
        getVisibleItems: function() {
            return this.listBox.getVisibleItems()
        },
        _setSize: function() {
            if (this.width != null && this.width.toString().indexOf("px") != -1) {
                this.host.width(this.width)
            } else {
                if (this.width != undefined && !isNaN(this.width)) {
                    this.host.width(this.width)
                }
            }
            if (this.height != null && this.height.toString().indexOf("px") != -1) {
                this.host.height(this.height)
            } else {
                if (this.height != undefined && !isNaN(this.height)) {
                    this.host.height(this.height)
                }
            }
            var e = false;
            if (this.width != null && this.width.toString().indexOf("%") != -1) {
                e = true;
                this.host.width(this.width)
            }
            if (this.height != null && this.height.toString().indexOf("%") != -1) {
                e = true;
                this.host.height(this.height)
            }
            var c = this;
            var d = function() {
                c._arrange();
                if (c.dropDownWidth == "auto") {
                    var f = c.host.width();
                    c.listBoxContainer.jqxListBox({
                        width: f
                    });
                    c.container.width(parseInt(f) + 25)
                }
            };
            if (e) {
                var b = this.host.width();
                if (this.dropDownWidth != "auto") {
                    b = this.dropDownWidth
                }
                this.listBoxContainer.jqxListBox({
                    width: b
                });
                this.container.width(parseInt(b) + 25)
            }
            a.jqx.utilities.resize(this.host, function() {
                d()
            }, false, this._checkForHiddenParent)
        },
        isOpened: function() {
            var c = this;
            var b = a.data(document.body, "openedJQXListBox" + this.id);
            if (b != null && b == c.listBoxContainer) {
                return true
            }
            return false
        },
        _updateHandlers: function() {
            var c = this;
            var d = false;
            this.removeHandlers();
            if (!this.touch) {
                this.addHandler(this.host, "mouseenter", function() {
                    if (!c.disabled && c.enableHover && c.renderMode !== "simple") {
                        d = true;
                        c.host.addClass(c.toThemeProperty("jqx-dropdownlist-state-hover"));
                        c.arrow.addClass(c.toThemeProperty("jqx-icon-arrow-down-hover"));
                        c.host.addClass(c.toThemeProperty("jqx-fill-state-hover"))
                    }
                });
                this.addHandler(this.host, "mouseleave", function() {
                    if (!c.disabled && c.enableHover && c.renderMode !== "simple") {
                        c.host.removeClass(c.toThemeProperty("jqx-dropdownlist-state-hover"));
                        c.host.removeClass(c.toThemeProperty("jqx-fill-state-hover"));
                        c.arrow.removeClass(c.toThemeProperty("jqx-icon-arrow-down-hover"));
                        d = false
                    }
                })
            }
            if (this.host.parents()) {
                this.addHandler(this.host.parents(), "scroll.dropdownlist" + this.element.id, function(e) {
                    var f = c.isOpened();
                    if (f) {
                        c.close()
                    }
                })
            }
            var b = "mousedown";
            if (this.touch) {
                b = a.jqx.mobile.getTouchEventName("touchstart")
            }
            this.addHandler(this.dropdownlistWrapper, b, function(f) {
                if (!c.disabled) {
                    var e = c.container.css("display") == "block";
                    if (!c.isanimating) {
                        if (e) {
                            c.hideListBox();
                            return false
                        } else {
                            c.showListBox();
                            if (!c.focusable) {
                                if (f.preventDefault) {
                                    f.preventDefault()
                                }
                            } else {
                                c.focus()
                            }
                        }
                    }
                }
            });
            if (c.autoOpen) {
                this.addHandler(this.host, "mouseenter", function() {
                    var e = c.isOpened();
                    if (!e && c.autoOpen) {
                        c.open();
                        c.host.focus()
                    }
                });
                a(document).on("mousemove." + c.id, function(h) {
                    var g = c.isOpened();
                    if (g && c.autoOpen) {
                        var l = c.host.coord();
                        var k = l.top;
                        var j = l.left;
                        var i = c.container.coord();
                        var e = i.left;
                        var f = i.top;
                        canClose = true;
                        if (h.pageY >= k && h.pageY <= k + c.host.height()) {
                            if (h.pageX >= j && h.pageX < j + c.host.width()) {
                                canClose = false
                            }
                        }
                        if (h.pageY >= f && h.pageY <= f + c.container.height()) {
                            if (h.pageX >= e && h.pageX < e + c.container.width()) {
                                canClose = false
                            }
                        }
                        if (canClose) {
                            c.close()
                        }
                    }
                })
            }
            if (this.touch) {
                this.addHandler(a(document), a.jqx.mobile.getTouchEventName("touchstart") + "." + this.id, c.closeOpenedListBox, {
                    me: this,
                    listbox: this.listBox,
                    id: this.id
                })
            } else {
                this.addHandler(a(document), "mousedown." + this.id, c.closeOpenedListBox, {
                    me: this,
                    listbox: this.listBox,
                    id: this.id
                })
            }
            this.addHandler(this.host, "keydown", function(f) {
                var e = c.container.css("display") == "block";
                if (c.host.css("display") == "none") {
                    return true
                }
                if (f.keyCode == "13" || f.keyCode == "9") {
                    if (!c.isanimating) {
                        if (e) {
                            c.renderSelection();
                            if (f.keyCode == "13" && c.focusable) {
                                c._firstDiv.focus()
                            }
                            c.hideListBox();
                            if (!c.keyboardSelection) {
                                c._raiseEvent("2", {
                                    index: c.selectedIndex,
                                    type: "keyboard",
                                    item: c.getItem(c.selectedIndex)
                                })
                            }
                            if (f.keyCode == "13") {
                                c._raiseEvent("4", {
                                    index: c.selectedIndex,
                                    type: "keyboard",
                                    item: c.getItem(c.selectedIndex)
                                })
                            }
                        }
                        if (e && f.keyCode != "9") {
                            return false
                        }
                        return true
                    }
                }
                if (f.keyCode == 115) {
                    if (!c.isanimating) {
                        if (!c.isOpened()) {
                            c.showListBox()
                        } else {
                            if (c.isOpened()) {
                                c.hideListBox()
                            }
                        }
                    }
                    return false
                }
                if (f.altKey) {
                    if (c.host.css("display") == "block") {
                        if (f.keyCode == 38) {
                            if (c.isOpened()) {
                                c.hideListBox();
                                return true
                            }
                        } else {
                            if (f.keyCode == 40) {
                                if (!c.isOpened()) {
                                    c.showListBox();
                                    return true
                                }
                            }
                        }
                    }
                }
                if (f.keyCode == "27") {
                    if (!c.ishiding) {
                        if (c.isOpened()) {
                            c.hideListBox();
                            if (c.tempSelectedIndex != undefined) {
                                c.selectIndex(c.tempSelectedIndex)
                            }
                        }
                        return true
                    }
                }
                if (!c.disabled) {
                    c._kbnavigated = c.listBox._handleKeyDown(f);
                    return c._kbnavigated
                }
            });
            this.addHandler(this.listBoxContainer, "checkChange", function(e) {
                c.renderSelection();
                c._updateInputSelection();
                c._raiseEvent(5, {
                    label: e.args.label,
                    value: e.args.value,
                    checked: e.args.checked,
                    item: e.args.item
                })
            });
            this.addHandler(this.listBoxContainer, "select", function(e) {
                if (!c.disabled) {
                    if (!e.args) {
                        return
                    }
                    if (e.args.type == "keyboard" && !c.isOpened()) {
                        c.renderSelection()
                    }
                    if (e.args.type != "keyboard" || c.keyboardSelection) {
                        c.renderSelection();
                        c._raiseEvent("2", {
                            index: e.args.index,
                            type: e.args.type,
                            item: e.args.item,
                            originalEvent: e.args.originalEvent
                        });
                        if (e.args.type == "mouse") {
                            if (!c.checkboxes) {
                                c.hideListBox();
                                if (c._firstDiv && c.focusable) {
                                    c._firstDiv.focus()
                                }
                            }
                        }
                    }
                }
            });
            if (this.listBox) {
                if (this.listBox.content) {
                    this.addHandler(this.listBox.content, "click", function(e) {
                        if (!c.disabled) {
                            if (c.listBox.itemswrapper && e.target === c.listBox.itemswrapper[0]) {
                                return true
                            }
                            c.renderSelection("mouse");
                            if (!c.touch) {
                                if (!c.ishiding) {
                                    if (!c.checkboxes) {
                                        c.hideListBox();
                                        if (c._firstDiv && c.focusable) {
                                            c._firstDiv.focus()
                                        }
                                    }
                                }
                            }
                            if (!c.keyboardSelection) {
                                if (c._kbnavigated === false) {
                                    if (c.tempSelectedIndex != c.selectedIndex) {
                                        c._raiseEvent("4", {
                                            index: c.selectedIndex,
                                            type: "mouse",
                                            item: c.getItem(c.selectedIndex)
                                        })
                                    }
                                    c._kbnavigated = true
                                }
                                if (c._oldSelectedInd == undefined) {
                                    c._oldSelectedIndx = c.selectedIndex
                                }
                                if (c.selectedIndex != c._oldSelectedIndx) {
                                    c._raiseEvent("2", {
                                        index: c.selectedIndex,
                                        type: "keyboard",
                                        item: c.getItem(c.selectedIndex)
                                    });
                                    c._oldSelectedIndx = c.selectedIndex
                                }
                            }
                        }
                    })
                }
            }
            this.addHandler(this.host, "focus", function(e) {
                if (c.renderMode !== "simple") {
                    c.host.addClass(c.toThemeProperty("jqx-dropdownlist-state-focus"));
                    c.host.addClass(c.toThemeProperty("jqx-fill-state-focus"))
                }
            });
            this.addHandler(this.host, "blur", function() {
                if (c.renderMode !== "simple") {
                    c.host.removeClass(c.toThemeProperty("jqx-dropdownlist-state-focus"));
                    c.host.removeClass(c.toThemeProperty("jqx-fill-state-focus"))
                }
            });
            this.addHandler(this._firstDiv, "focus", function(e) {
                if (c.renderMode !== "simple") {
                    c.host.addClass(c.toThemeProperty("jqx-dropdownlist-state-focus"));
                    c.host.addClass(c.toThemeProperty("jqx-fill-state-focus"))
                }
            });
            this.addHandler(this._firstDiv, "blur", function() {
                if (c.renderMode !== "simple") {
                    c.host.removeClass(c.toThemeProperty("jqx-dropdownlist-state-focus"));
                    c.host.removeClass(c.toThemeProperty("jqx-fill-state-focus"))
                }
            })
        },
        removeHandlers: function() {
            var c = this;
            var b = "mousedown";
            if (this.touch) {
                b = a.jqx.mobile.getTouchEventName("touchstart")
            }
            this.removeHandler(this.dropdownlistWrapper, b);
            if (this.listBox) {
                if (this.listBox.content) {
                    this.removeHandler(this.listBox.content, "click")
                }
            }
            this.removeHandler(this.host, "loadContent");
            this.removeHandler(this.listBoxContainer, "checkChange");
            this.removeHandler(this.host, "keydown");
            this.removeHandler(this.host, "focus");
            this.removeHandler(this.host, "blur");
            this.removeHandler(this._firstDiv, "focus");
            this.removeHandler(this._firstDiv, "blur");
            this.removeHandler(this.host, "mouseenter");
            this.removeHandler(this.host, "mouseleave");
            this.removeHandler(a(document), "mousemove." + c.id)
        },
        getItem: function(b) {
            var c = this.listBox.getItem(b);
            return c
        },
        getItemByValue: function(c) {
            var b = this.listBox.getItemByValue(c);
            return b
        },
        selectItem: function(b) {
            if (this.listBox != undefined) {
                this.listBox.selectItem(b);
                this.selectedIndex = this.listBox.selectedIndex;
                this.renderSelection("mouse")
            }
        },
        unselectItem: function(b) {
            if (this.listBox != undefined) {
                this.listBox.unselectItem(b);
                this.renderSelection("mouse")
            }
        },
        checkItem: function(b) {
            if (this.listBox != undefined) {
                this.listBox.checkItem(b)
            }
        },
        uncheckItem: function(b) {
            if (this.listBox != undefined) {
                this.listBox.uncheckItem(b)
            }
        },
        indeterminateItem: function(b) {
            if (this.listBox != undefined) {
                this.listBox.indeterminateItem(b)
            }
        },
        renderSelection: function() {
            if (this.listBox == null) {
                return
            }
            if (this.height && this.height.toString().indexOf("%") != -1) {
                this._arrange()
            }
            var q = this.listBox.visibleItems[this.listBox.selectedIndex];
            if (this.filterable) {
                if (this.listBox.selectedIndex == -1) {
                    for (var d in this.listBox.selectedValues) {
                        var k = this.listBox.selectedValues[d];
                        var b = this.listBox.getItemByValue(k);
                        if (b) {
                            q = b
                        }
                    }
                }
            }
            var s = this;
            if (this.checkboxes) {
                var t = this.getCheckedItems();
                if (t != null && t.length > 0) {
                    q = t[0]
                } else {
                    q = null
                }
            }
            if (q == null) {
                var h = a('<span unselectable="on" style="color: inherit; border: none; background-color: transparent;"></span>');
                h.appendTo(a(document.body));
                h.addClass(this.toThemeProperty("jqx-widget"));
                h.addClass(this.toThemeProperty("jqx-listitem-state-normal"));
                h.addClass(this.toThemeProperty("jqx-item"));
                a.jqx.utilities.html(h, this.placeHolder);
                var r = this.dropdownlistContent.css("padding-top");
                var u = this.dropdownlistContent.css("padding-bottom");
                h.css("padding-top", r);
                h.css("padding-bottom", u);
                var p = h.outerHeight();
                h.remove();
                h.removeClass();
                a.jqx.utilities.html(this.dropdownlistContent, h);
                var f = this.host.height();
                if (this.height != null && this.height != undefined) {
                    if (this.height.toString().indexOf("%") === -1) {
                        f = parseInt(this.height)
                    }
                }
                var e = parseInt((parseInt(f) - parseInt(p)) / 2);
                if (e > 0) {
                    this.dropdownlistContent.css("margin-top", e + "px");
                    this.dropdownlistContent.css("margin-bottom", e + "px")
                }
                if (this.selectionRenderer) {
                    a.jqx.utilities.html(this.dropdownlistContent, this.selectionRenderer(h, -1, "", ""));
                    this.dropdownlistContent.css("margin-top", "0px");
                    this.dropdownlistContent.css("margin-bottom", "0px");
                    this._updateInputSelection()
                } else {
                    this._updateInputSelection()
                }
                this.selectedIndex = this.listBox.selectedIndex;
                if (this.width === "auto") {
                    this._arrange()
                }
                if (this.focusable && this.isOpened()) {
                    this.focus()
                }
                return
            }
            this.selectedIndex = this.listBox.selectedIndex;
            var h = a('<span unselectable="on" style="color: inherit; border: none; background-color: transparent;"></span>');
            h.appendTo(a(document.body));
            h.addClass(this.toThemeProperty("jqx-widget"));
            h.addClass(this.toThemeProperty("jqx-listitem-state-normal"));
            h.addClass(this.toThemeProperty("jqx-item"));
            var o = false;
            try {
                if (q.html != undefined && q.html != null && q.html.toString().length > 0) {
                    a.jqx.utilities.html(h, q.html)
                } else {
                    if (q.label != undefined && q.label != null && q.label.toString().length > 0) {
                        a.jqx.utilities.html(h, q.label)
                    } else {
                        if (q.label === null || q.label === "") {
                            o = true;
                            a.jqx.utilities.html(h, "")
                        } else {
                            if (q.value != undefined && q.value != null && q.value.toString().length > 0) {
                                a.jqx.utilities.html(h, q.value)
                            } else {
                                if (q.title != undefined && q.title != null && q.title.toString().length > 0) {
                                    a.jqx.utilities.html(h, q.title)
                                } else {
                                    if (q.label == "" || q.label == null) {
                                        o = true;
                                        a.jqx.utilities.html(h, "")
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (m) {
                var l = m
            }
            var r = this.dropdownlistContent.css("padding-top");
            var u = this.dropdownlistContent.css("padding-bottom");
            h.css("padding-top", r);
            h.css("padding-bottom", u);
            var p = h.outerHeight();
            if (p === 0) {
                p = 16
            }
            if ((q.label == "" || q.label == null) && o) {
                a.jqx.utilities.html(h, "")
            }
            var c = this.width && this.width.toString().indexOf("%") <= 0;
            h.remove();
            h.removeClass();
            if (this.selectionRenderer) {
                a.jqx.utilities.html(this.dropdownlistContent, this.selectionRenderer(h, q.index, q.label, q.value));
                if (this.focusable && this.isOpened()) {
                    this.focus()
                }
            } else {
                if (this.checkboxes) {
                    var g = this.getCheckedItems();
                    var j = "";
                    for (var n = 0; n < g.length; n++) {
                        if (n == g.length - 1) {
                            j += g[n].label
                        } else {
                            j += g[n].label + ","
                        }
                    }
                    h.text(j);
                    if (c) {
                        h.css("max-width", this.host.width() - 30)
                    }
                    h.css("overflow", "hidden");
                    h.css("display", "block");
                    if (!this.rtl) {
                        if (c) {
                            h.css("width", this.host.width() - 30)
                        }
                    }
                    h.css("text-overflow", "ellipsis");
                    h.css("padding-bottom", 1 + parseInt(u));
                    this.dropdownlistContent.html(h);
                    if (this.focusable && this.isOpened()) {
                        this.focus()
                    }
                } else {
                    if (this.width && this.width !== "auto") {
                        if (c) {
                            if (!this.rtl) {
                                h.css("max-width", this.host.width() - this.arrowSize - 3)
                            }
                        }
                        h.css("overflow", "hidden");
                        h.css("display", "block");
                        h.css("padding-bottom", 1 + parseInt(u));
                        if (!this.rtl) {
                            if (c) {
                                h.css("width", this.host.width() - this.arrowSize - 3)
                            }
                        }
                        h.css("text-overflow", "ellipsis")
                    }
                    this.dropdownlistContent.html(h);
                    if (this.focusable && this.isOpened()) {
                        this.focus()
                    }
                }
            }
            var f = this.host.height();
            if (this.height != null && this.height != undefined) {
                if (this.height.toString().indexOf("%") === -1) {
                    f = parseInt(this.height)
                }
            }
            var e = parseInt((parseInt(f) - parseInt(p)) / 2);
            if (e > 0) {
                this.dropdownlistContent.css("margin-top", e + "px");
                this.dropdownlistContent.css("margin-bottom", e + "px")
            }
            if (this.selectionRenderer) {
                this.dropdownlistContent.css("margin-top", "0px");
                this.dropdownlistContent.css("margin-bottom", "0px")
            }
            if (this.dropdownlistContent && this.input) {
                this._updateInputSelection()
            }
            if (this.listBox && this.listBox._activeElement) {
                a.jqx.aria(this, "aria-activedescendant", this.listBox._activeElement.id)
            }
            if (this.width === "auto") {
                this._arrange()
            }
        },
        _updateInputSelection: function() {
            if (this.input) {
                var c = new Array();
                if (this.selectedIndex == -1) {
                    this.input.val("")
                } else {
                    var f = this.getSelectedItem();
                    if (f != null) {
                        this.input.val(f.value);
                        c.push(f.value)
                    } else {
                        this.input.val(this.dropdownlistContent.text())
                    }
                }
                if (this.checkboxes) {
                    var b = this.getCheckedItems();
                    var g = "";
                    if (b != null) {
                        for (var d = 0; d < b.length; d++) {
                            var e = b[d].value;
                            if (e == undefined) {
                                continue
                            }
                            if (d == b.length - 1) {
                                g += e
                            } else {
                                g += e + ","
                            }
                            c.push(e)
                        }
                    }
                    this.input.val(g)
                }
            }
            if (this.field && this.input) {
                if (this.field.nodeName.toLowerCase() == "select") {
                    a.each(this.field, function(h, i) {
                        a(this).removeAttr("selected");
                        this.selected = c.indexOf(this.value) >= 0;
                        if (this.selected) {
                            a(this).attr("selected", true)
                        }
                    })
                } else {
                    a.each(this.items, function(h, i) {
                        a(this.originalItem.originalItem).removeAttr("data-selected");
                        this.selected = c.indexOf(this.value) >= 0;
                        if (this.selected) {
                            a(this.originalItem.originalItem).attr("data-selected", true)
                        }
                    })
                }
            }
        },
        setContent: function(b) {
            a.jqx.utilities.html(this.dropdownlistContent, b);
            this._updateInputSelection()
        },
        dataBind: function() {
            this.listBoxContainer.jqxListBox({
                source: this.source
            });
            this.renderSelection("mouse");
            if (this.source == null) {
                this.clearSelection()
            }
        },
        clear: function() {
            this.listBoxContainer.jqxListBox({
                source: null
            });
            this.clearSelection()
        },
        clearSelection: function(b) {
            this.selectedIndex = -1;
            this._updateInputSelection();
            this.listBox.clearSelection();
            this.renderSelection();
            if (!this.selectionRenderer) {
                a.jqx.utilities.html(this.dropdownlistContent, this.placeHolder)
            }
        },
        unselectIndex: function(b, c) {
            if (isNaN(b)) {
                return
            }
            this.listBox.unselectIndex(b, c);
            this.renderSelection()
        },
        selectIndex: function(b, d, e, c) {
            this.listBox.selectIndex(b, d, e, c, "api")
        },
        getSelectedIndex: function() {
            return this.selectedIndex
        },
        getSelectedItem: function() {
            return this.listBox.getVisibleItem(this.selectedIndex)
        },
        getCheckedItems: function() {
            return this.listBox.getCheckedItems()
        },
        checkIndex: function(b) {
            this.listBox.checkIndex(b)
        },
        uncheckIndex: function(b) {
            this.listBox.uncheckIndex(b)
        },
        indeterminateIndex: function(b) {
            this.listBox.indeterminateIndex(b)
        },
        checkAll: function() {
            this.listBox.checkAll();
            this.renderSelection("mouse")
        },
        uncheckAll: function() {
            this.listBox.uncheckAll();
            this.renderSelection("mouse")
        },
        addItem: function(b) {
            return this.listBox.addItem(b)
        },
        insertAt: function(c, b) {
            if (c == null) {
                return false
            }
            return this.listBox.insertAt(c, b)
        },
        removeAt: function(c) {
            var b = this.listBox.removeAt(c);
            this.renderSelection("mouse");
            return b
        },
        removeItem: function(c) {
            var b = this.listBox.removeItem(c);
            this.renderSelection("mouse");
            return b
        },
        updateItem: function(c, d) {
            var b = this.listBox.updateItem(c, d);
            this.renderSelection("mouse");
            return b
        },
        updateAt: function(d, c) {
            var b = this.listBox.updateAt(d, c);
            this.renderSelection("mouse");
            return b
        },
        ensureVisible: function(b) {
            return this.listBox.ensureVisible(b)
        },
        disableAt: function(b) {
            return this.listBox.disableAt(b)
        },
        enableAt: function(b) {
            return this.listBox.enableAt(b)
        },
        disableItem: function(b) {
            return this.listBox.disableItem(b)
        },
        enableItem: function(b) {
            return this.listBox.enableItem(b)
        },
        _findPos: function(c) {
            while (c && (c.type == "hidden" || c.nodeType != 1 || a.expr.filters.hidden(c))) {
                c = c.nextSibling
            }
            var b = a(c).coord(true);
            return [b.left, b.top]
        },
        testOffset: function(h, f, c) {
            var g = h.outerWidth();
            var j = h.outerHeight();
            var i = a(window).width() + a(window).scrollLeft();
            var e = a(window).height() + a(window).scrollTop();
            if (f.left + g > i) {
                if (g > this.host.width()) {
                    var d = this.host.coord().left;
                    var b = g - this.host.width();
                    f.left = d - b + 2
                }
            }
            if (f.left < 0) {
                f.left = parseInt(this.host.coord().left) + "px"
            }
            f.top -= Math.min(f.top, (f.top + j > e && e > j) ? Math.abs(j + c + 22) : 0);
            return f
        },
        open: function() {
            this.showListBox()
        },
        close: function() {
            this.hideListBox()
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
        showListBox: function() {
            a.jqx.aria(this, "aria-expanded", true);
            if (this.dropDownWidth == "auto" && this.width != null && this.width.indexOf && (this.width.indexOf("%") != -1 || this.width.indexOf("auto") != -1)) {
                if (this.listBox.host.width() != this.host.width()) {
                    var c = this.host.width();
                    this.listBoxContainer.jqxListBox({
                        width: c
                    });
                    this.container.width(parseInt(c) + 25)
                }
            }
            var q = this;
            var e = this.listBoxContainer;
            var k = this.listBox;
            var n = a(window).scrollTop();
            var i = a(window).scrollLeft();
            var l = parseInt(this._findPos(this.host[0])[1]) + parseInt(this.host.outerHeight()) - 1 + "px";
            var g, h = parseInt(Math.round(this.host.coord(true).left));
            g = h + "px";
            var p = a.jqx.mobile.isSafariMobileBrowser() || a.jqx.mobile.isWindowsPhone();
            if (this.listBox == null) {
                return
            }
            var d = a.jqx.utilities.hasTransform(this.host);
            this.ishiding = false;
            if (!this.keyboardSelection) {
                this.listBox.selectIndex(this.selectedIndex);
                this.listBox.ensureVisible(this.selectedIndex)
            }
            this.tempSelectedIndex = this.selectedIndex;
            if (this.autoDropDownHeight) {
                this.container.height(this.listBoxContainer.height() + 25)
            }
            if (d || (p != null && p)) {
                g = a.jqx.mobile.getLeftPos(this.element);
                l = a.jqx.mobile.getTopPos(this.element) + parseInt(this.host.outerHeight());
                if (a("body").css("border-top-width") != "0px") {
                    l = parseInt(l) - this._getBodyOffset().top + "px"
                }
                if (a("body").css("border-left-width") != "0px") {
                    g = parseInt(g) - this._getBodyOffset().left + "px"
                }
            }
            e.stop();
            if (this.renderMode !== "simple") {
                this.host.addClass(this.toThemeProperty("jqx-dropdownlist-state-selected"));
                this.host.addClass(this.toThemeProperty("jqx-fill-state-pressed"));
                this.arrow.addClass(this.toThemeProperty("jqx-icon-arrow-down-selected"))
            }
            this.container.css("left", g);
            this.container.css("top", l);
            k._arrange();
            var f = true;
            var r = false;
            if (this.dropDownHorizontalAlignment == "right" || this.rtl) {
                var m = this.container.outerWidth();
                var b = Math.abs(m - this.host.width());
                if (m > this.host.width()) {
                    this.container.css("left", 25 + parseInt(Math.round(h)) - b + "px")
                } else {
                    this.container.css("left", 25 + parseInt(Math.round(h)) + b + "px")
                }
            }
            if (this.enableBrowserBoundsDetection) {
                var j = this.testOffset(e, {
                    left: parseInt(this.container.css("left")),
                    top: parseInt(l)
                }, parseInt(this.host.outerHeight()));
                if (parseInt(this.container.css("top")) != j.top) {
                    r = true;
                    e.css("top", 23);
                    e.addClass(this.toThemeProperty("jqx-popup-up"))
                } else {
                    e.css("top", 0)
                }
                this.container.css("top", j.top);
                if (parseInt(this.container.css("left")) != j.left) {
                    this.container.css("left", j.left)
                }
            }
            if (this.animationType == "none") {
                this.container.css("display", "block");
                a.data(document.body, "openedJQXListBoxParent", q);
                a.data(document.body, "openedJQXListBox" + this.id, e);
                e.css("margin-top", 0);
                e.css("opacity", 1);
                k._renderItems();
                q._raiseEvent("0", k)
            } else {
                this.container.css("display", "block");
                q.isanimating = true;
                if (this.animationType == "fade") {
                    e.css("margin-top", 0);
                    e.css("opacity", 0);
                    e.animate({
                        opacity: 1
                    }, this.openDelay, function() {
                        a.data(document.body, "openedJQXListBoxParent", q);
                        a.data(document.body, "openedJQXListBox" + q.id, e);
                        q.ishiding = false;
                        q.isanimating = false;
                        k._renderItems();
                        q._raiseEvent("0", k)
                    })
                } else {
                    e.css("opacity", 1);
                    var o = e.outerHeight();
                    if (r) {
                        e.css("margin-top", o)
                    } else {
                        e.css("margin-top", -o)
                    }
                    e.animate({
                        "margin-top": 0
                    }, this.openDelay, function() {
                        a.data(document.body, "openedJQXListBoxParent", q);
                        a.data(document.body, "openedJQXListBox" + q.id, e);
                        q.ishiding = false;
                        q.isanimating = false;
                        k._renderItems();
                        q._raiseEvent("0", k)
                    })
                }
            }
            if (!r) {
                this.host.addClass(this.toThemeProperty("jqx-rc-b-expanded"));
                e.addClass(this.toThemeProperty("jqx-rc-t-expanded"))
            } else {
                this.host.addClass(this.toThemeProperty("jqx-rc-t-expanded"));
                e.addClass(this.toThemeProperty("jqx-rc-b-expanded"))
            }
            if (this.renderMode !== "simple") {
                e.addClass(this.toThemeProperty("jqx-fill-state-focus"));
                this.host.addClass(this.toThemeProperty("jqx-dropdownlist-state-focus"));
                this.host.addClass(this.toThemeProperty("jqx-fill-state-focus"))
            }
        },
        hideListBox: function() {
            a.jqx.aria(this, "aria-expanded", false);
            var f = this.listBoxContainer;
            var g = this.listBox;
            var c = this.container;
            var d = this;
            a.data(document.body, "openedJQXListBox" + this.id, null);
            if (this.animationType == "none") {
                this.container.css("display", "none")
            } else {
                if (!d.ishiding) {
                    f.stop();
                    var b = f.outerHeight();
                    f.css("margin-top", 0);
                    d.isanimating = true;
                    var e = -b;
                    if (parseInt(this.container.coord().top) < parseInt(this.host.coord().top)) {
                        e = b
                    }
                    if (this.animationType == "fade") {
                        f.css({
                            opacity: 1
                        });
                        f.animate({
                            opacity: 0
                        }, this.closeDelay, function() {
                            c.css("display", "none");
                            d.isanimating = false;
                            d.ishiding = false
                        })
                    } else {
                        f.animate({
                            "margin-top": e
                        }, this.closeDelay, function() {
                            c.css("display", "none");
                            d.isanimating = false;
                            d.ishiding = false
                        })
                    }
                }
            }
            this.ishiding = true;
            this.host.removeClass(this.toThemeProperty("jqx-dropdownlist-state-selected"));
            this.host.removeClass(this.toThemeProperty("jqx-fill-state-pressed"));
            this.arrow.removeClass(this.toThemeProperty("jqx-icon-arrow-down-selected"));
            this.host.removeClass(this.toThemeProperty("jqx-rc-b-expanded"));
            f.removeClass(this.toThemeProperty("jqx-rc-t-expanded"));
            this.host.removeClass(this.toThemeProperty("jqx-rc-t-expanded"));
            f.removeClass(this.toThemeProperty("jqx-rc-b-expanded"));
            f.removeClass(this.toThemeProperty("jqx-fill-state-focus"));
            this.host.removeClass(this.toThemeProperty("jqx-dropdownlist-state-focus"));
            this.host.removeClass(this.toThemeProperty("jqx-fill-state-focus"));
            this._raiseEvent("1", g)
        },
        closeOpenedListBox: function(e) {
            var d = e.data.me;
            var b = a(e.target);
            var c = e.data.listbox;
            if (c == null) {
                return true
            }
            if (a(e.target).ischildof(e.data.me.host)) {
                return true
            }
            if (!d.isOpened()) {
                return true
            }
            var f = d;
            var g = false;
            a.each(b.parents(), function() {
                if (this.className != "undefined") {
                    if (this.className.indexOf) {
                        if (this.className.indexOf("jqx-listbox") != -1) {
                            g = true;
                            return false
                        }
                        if (this.className.indexOf("jqx-dropdownlist") != -1) {
                            if (d.element.id == this.id) {
                                g = true
                            }
                            return false
                        }
                    }
                }
            });
            if (c != null && !g && d.isOpened()) {
                d.hideListBox()
            }
            return true
        },
        loadFromSelect: function(b) {
            this.listBox.loadFromSelect(b)
        },
        refresh: function(b) {
            if (b !== true) {
                this._setSize();
                this._arrange();
                if (this.listBox) {
                    this.renderSelection()
                }
            }
        },
        _arrange: function() {
            var h = parseInt(this.host.width());
            var b = parseInt(this.host.height());
            var e = this.arrowSize;
            var d = this.arrowSize;
            var i = 3;
            var c = h - d - 2 * i;
            if (c > 0 && this.width !== "auto") {
                this.dropdownlistContent.width(c + "px")
            } else {
                if (c <= 0) {
                    this.dropdownlistContent.width(0)
                }
            }
            if (this.width === "auto") {
                this.dropdownlistContent.css("width", "auto");
                h = this.dropdownlistContent.width() + d + 2 * i;
                this.host.width(h)
            }
            this.dropdownlistContent.height(b);
            this.dropdownlistContent.css("left", 0);
            this.dropdownlistContent.css("top", 0);
            this.dropdownlistArrow.width(d);
            if (this.width && this.width.toString().indexOf("%") >= 0) {
                var g = (d * 100) / h;
                var f = (c * 100) / h;
                this.dropdownlistArrow.css("width", g + "%");
                this.dropdownlistContent.css("width", f + "%")
            }
            this.dropdownlistArrow.height(b);
            if (this.rtl) {
                this.dropdownlistArrow.css("float", "left");
                this.dropdownlistContent.css("float", "right")
            }
        },
        destroy: function() {
            a.jqx.utilities.resize(this.host, null, true);
            this.removeHandler(this.listBoxContainer, "select");
            this.removeHandler(this.listBoxContainer, "unselect");
            this.removeHandler(this.listBoxContainer, "change");
            this.removeHandler(this.dropdownlistWrapper, "selectstart");
            this.removeHandler(this.dropdownlistWrapper, "mousedown");
            this.removeHandler(this.host, "keydown");
            this.removeHandler(this.listBoxContainer, "select");
            this.removeHandler(this.listBox.content, "click");
            this.removeHandler(this.listBoxContainer, "bindingComplete");
            if (this.host.parents()) {
                this.removeHandler(this.host.parents(), "scroll.dropdownlist" + this.element.id)
            }
            this.removeHandlers();
            this.listBoxContainer.jqxListBox("destroy");
            this.listBoxContainer.remove();
            this.host.removeClass();
            this.removeHandler(a(document), "mousedown." + this.id, this.closeOpenedListBox);
            if (this.touch) {
                this.removeHandler(a(document), a.jqx.mobile.getTouchEventName("touchstart") + "." + this.id)
            }
            this.dropdownlistArrow.remove();
            delete this.dropdownlistArrow;
            delete this.dropdownlistWrapper;
            delete this.listBoxContainer;
            delete this.input;
            delete this.arrow;
            delete this.dropdownlistContent;
            delete this.listBox;
            delete this._firstDiv;
            this.container.remove();
            delete this.container;
            var b = a.data(this.element, "jqxDropDownList");
            if (b) {
                delete b.instance
            }
            this.host.removeData();
            this.host.remove();
            delete this.comboStructure;
            delete this.host;
            delete this.set;
            delete this.get;
            delete this.call;
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
            e.owner = this;
            if (f == 2 || f == 3 || f == 4 || f == 5) {
                e.args = c
            }
            var b = this.host.trigger(e);
            return b
        },
        propertyChangedHandler: function(b, c, f, e) {
            if (b.isInitialized == undefined || b.isInitialized == false) {
                return
            }
            if (c == "filterable") {
                b.listBoxContainer.jqxListBox({
                    filterable: e
                })
            }
            if (c == "filterHeight") {
                b.listBoxContainer.jqxListBox({
                    filterHeight: e
                })
            }
            if (c == "filterPlaceHolder") {
                b.listBoxContainer.jqxListBox({
                    filterPlaceHolder: e
                })
            }
            if (c == "filterDelay") {
                b.listBoxContainer.jqxListBox({
                    filterDelay: e
                })
            }
            if (c == "enableSelection") {
                b.listBoxContainer.jqxListBox({
                    enableSelection: e
                })
            }
            if (c == "enableHover") {
                b.listBoxContainer.jqxListBox({
                    enableHover: e
                })
            }
            if (c == "autoOpen") {
                b._updateHandlers()
            }
            if (c == "emptyString") {
                b.listBox.emptyString = b.emptyString
            }
            if (c == "itemHeight") {
                b.listBoxContainer.jqxListBox({
                    itemHeight: e
                })
            }
            if (c == "renderer") {
                b.listBoxContainer.jqxListBox({
                    renderer: e
                })
            }
            if (c == "rtl") {
                if (e) {
                    b.dropdownlistArrow.css("float", "left");
                    b.dropdownlistContent.css("float", "right")
                } else {
                    b.dropdownlistArrow.css("float", "right");
                    b.dropdownlistContent.css("float", "left")
                }
                b.listBoxContainer.jqxListBox({
                    rtl: b.rtl
                })
            }
            if (c == "source") {
                b.listBoxContainer.jqxListBox({
                    source: b.source
                });
                b.listBox.selectedIndex = -1;
                b.listBox.selectIndex(this.selectedIndex);
                b.renderSelection();
                if (e == null) {
                    b.clear()
                }
            }
            if (c == "displayMember" || c == "valueMember") {
                b.listBoxContainer.jqxListBox({
                    displayMember: b.displayMember,
                    valueMember: b.valueMember
                });
                b.renderSelection()
            }
            if (c == "placeHolder") {
                b.renderSelection()
            }
            if (c == "theme" && e != null) {
                b.listBoxContainer.jqxListBox({
                    theme: e
                });
                b.listBoxContainer.addClass(b.toThemeProperty("jqx-popup"));
                a.jqx.utilities.setTheme(f, e, b.host)
            }
            if (c == "autoDropDownHeight") {
                b.listBoxContainer.jqxListBox({
                    autoHeight: b.autoDropDownHeight
                });
                if (b.autoDropDownHeight) {
                    b.container.height(b.listBoxContainer.height() + 25)
                } else {
                    b.listBoxContainer.jqxListBox({
                        height: b.dropDownHeight
                    });
                    b.container.height(parseInt(b.dropDownHeight) + 25)
                }
                b.listBox._arrange();
                b.listBox._updatescrollbars()
            }
            if (c == "searchMode") {
                b.listBoxContainer.jqxListBox({
                    searchMode: b.searchMode
                })
            }
            if (c == "incrementalSearch") {
                b.listBoxContainer.jqxListBox({
                    incrementalSearch: b.incrementalSearch
                })
            }
            if (c == "incrementalSearchDelay") {
                b.listBoxContainer.jqxListBox({
                    incrementalSearchDelay: b.incrementalSearchDelay
                })
            }
            if (c == "dropDownHeight") {
                if (!b.autoDropDownHeight) {
                    b.listBoxContainer.jqxListBox({
                        height: b.dropDownHeight
                    });
                    b.container.height(parseInt(b.dropDownHeight) + 25)
                }
            }
            if (c == "dropDownWidth" || c == "scrollBarSize") {
                var d = b.width;
                if (b.dropDownWidth != "auto") {
                    d = b.dropDownWidth
                }
                b.listBoxContainer.jqxListBox({
                    width: d,
                    scrollBarSize: b.scrollBarSize
                });
                b.container.width(parseInt(d) + 25)
            }
            if (c == "width" || c == "height") {
                if (e != f) {
                    this.refresh();
                    if (c == "width") {
                        if (b.dropDownWidth == "auto") {
                            var d = b.host.width();
                            b.listBoxContainer.jqxListBox({
                                width: d
                            });
                            b.container.width(parseInt(d) + 25)
                        }
                    }
                }
            }
            if (c == "checkboxes") {
                b.listBoxContainer.jqxListBox({
                    checkboxes: b.checkboxes
                })
            }
            if (c == "selectedIndex") {
                if (b.listBox != null) {
                    b.listBox.selectIndex(parseInt(e));
                    b.renderSelection()
                }
            }
        }
    })
})(jqxBaseFramework);