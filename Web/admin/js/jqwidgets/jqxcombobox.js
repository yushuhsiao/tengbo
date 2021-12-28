/*
jQWidgets v3.6.0 (2014-Nov-25)
Copyright (c) 2011-2014 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a) {
    a.jqx.jqxWidget("jqxComboBox", "", {});
    a.extend(a.jqx._jqxComboBox.prototype, {
        defineInstance: function() {
            var b = {
                disabled: false,
                width: 200,
                height: 25,
                items: new Array(),
                selectedIndex: -1,
                selectedItems: new Array(),
                _selectedItems: new Array(),
                source: null,
                scrollBarSize: a.jqx.utilities.scrollBarSize,
                arrowSize: 18,
                enableHover: true,
                enableSelection: true,
                visualItems: new Array(),
                groups: new Array(),
                equalItemsWidth: true,
                itemHeight: -1,
                visibleItems: new Array(),
                emptyGroupText: "Group",
                emptyString: "",
                openDelay: 250,
                closeDelay: 300,
                animationType: "default",
                dropDownWidth: "auto",
                dropDownHeight: "200px",
                autoDropDownHeight: false,
                enableBrowserBoundsDetection: false,
                dropDownHorizontalAlignment: "left",
                searchMode: "startswithignorecase",
                autoComplete: false,
                remoteAutoComplete: false,
                remoteAutoCompleteDelay: 500,
                selectionMode: "default",
                minLength: 2,
                displayMember: "",
                valueMember: "",
                keyboardSelection: true,
                renderer: null,
                autoOpen: false,
                checkboxes: false,
                promptText: "",
                placeHolder: "",
                rtl: false,
                listBox: null,
                validateSelection: null,
                showCloseButtons: true,
                renderSelectedItem: null,
                search: null,
                popupZIndex: 100000,
                searchString: null,
                multiSelect: false,
                showArrow: true,
                _disabledItems: new Array(),
                touchMode: "auto",
                autoBind: true,
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
            var c = this;
            this.host.attr("role", "combobox");
            a.jqx.aria(this, "aria-autocomplete", "both");
            if (a.jqx._jqxListBox == null || a.jqx._jqxListBox == undefined) {
                throw new Error("jqxComboBox: Missing reference to jqxlistbox.js.")
            }
            a.jqx.aria(this);
            if (this.promptText != "") {
                this.placeHolder = this.promptText
            }
            this.render()
        },
        render: function() {
            var m = this;
            var o = m.element.nodeName.toLowerCase();
            if (o == "select" || o == "ul" || o == "ol") {
                m.field = m.element;
                if (m.field.className) {
                    m._className = m.field.className
                }
                var l = {
                    title: m.field.title
                };
                if (m.field.id.length) {
                    l.id = m.field.id.replace(/[^\w]/g, "_") + "_jqxComboBox"
                } else {
                    l.id = a.jqx.utilities.createId() + "_jqxComboBox"
                }
                var c = a("<div></div>", l);
                if (!m.width) {
                    m.width = a(m.field).width()
                }
                if (!m.height) {
                    m.height = a(m.field).outerHeight()
                }
                a(m.field).hide().after(c);
                m.host = c;
                m.element = c[0];
                m.element.id = m.field.id;
                m.field.id = l.id;
                if (m.field.tabIndex) {
                    var f = m.field.tabIndex;
                    m.field.tabIndex = -1;
                    m.element.tabIndex = f
                }
                var r = a.jqx.parseSourceTag(m.field);
                m.source = r.items;
                if (m.selectedIndex == -1) {
                    m.selectedIndex = r.index
                }
            }
            m.removeHandlers();
            m.isanimating = false;
            m.id = a.jqx.utilities.createId();
            m.element.innerHTML = "";
            var g = a("<div style='background-color: transparent; -webkit-appearance: none; outline: none; width:100%; height: 100%; padding: 0px; margin: 0px; border: 0px; position: relative;'><div id='dropdownlistWrapper' style='padding: 0; margin: 0; border: none; background-color: transparent; float: left; width:100%; height: 100%; position: relative;'><div id='dropdownlistContent' style='padding: 0; margin: 0; border-top: none; border-bottom: none; float: left; position: absolute;'/><div id='dropdownlistArrow' style='padding: 0; margin: 0; border-left-width: 1px; border-bottom-width: 0px; border-top-width: 0px; border-right-width: 0px; float: right; position: absolute;'/></div></div>");
            m.comboStructure = g;
            if (a.jqx._jqxListBox == null || a.jqx._jqxListBox == undefined) {
                throw "jqxComboBox: Missing reference to jqxlistbox.js."
            }
            m.touch = a.jqx.mobile.isTouchDevice();
            if (m.touchMode === true) {
                m.touch = true
            }
            m.host.append(g);
            m.dropdownlistWrapper = m.host.find("#dropdownlistWrapper");
            m.dropdownlistArrow = m.host.find("#dropdownlistArrow");
            m.dropdownlistContent = m.host.find("#dropdownlistContent");
            m.dropdownlistContent.addClass(m.toThemeProperty("jqx-combobox-content"));
            m.dropdownlistContent.addClass(m.toThemeProperty("jqx-widget-content"));
            m.dropdownlistWrapper[0].id = "dropdownlistWrapper" + m.element.id;
            m.dropdownlistArrow[0].id = "dropdownlistArrow" + m.element.id;
            m.dropdownlistContent[0].id = "dropdownlistContent" + m.element.id;
            m.dropdownlistContent.append(a('<input autocomplete="off" style="margin: 0; padding: 0; border: 0;" type="textarea"/>'));
            m.input = m.dropdownlistContent.find("input");
            m.input.addClass(m.toThemeProperty("jqx-combobox-input"));
            m.input.addClass(m.toThemeProperty("jqx-widget-content"));
            if (m.host.attr("tabindex")) {
                m.input.attr("tabindex", m.host.attr("tabindex"));
                m.host.removeAttr("tabindex")
            }
            m._addInput();
            if (m.rtl) {
                m.input.css({
                    direction: "rtl"
                });
                m.dropdownlistContent.addClass(m.toThemeProperty("jqx-combobox-content-rtl"))
            }
            try {
                var p = "listBox" + m.id;
                var i = a(a.find("#" + p));
                if (i.length > 0) {
                    i.remove()
                }
                a.jqx.aria(this, "aria-owns", p);
                a.jqx.aria(this, "aria-haspopup", true);
                a.jqx.aria(this, "aria-multiline", false);
                if (m.listBoxContainer) {
                    m.listBoxContainer.jqxListBox("destroy")
                }
                if (m.container) {
                    m.container.remove()
                }
                var b = a("<div style='overflow: hidden; border: none; background-color: transparent; position: absolute;' id='listBox" + m.id + "'><div id='innerListBox" + m.id + "'></div></div>");
                b.hide();
                b.appendTo(document.body);
                m.container = b;
                m.listBoxContainer = a(a.find("#innerListBox" + m.id));
                var d = m.width;
                if (m.dropDownWidth != "auto") {
                    d = m.dropDownWidth
                }
                if (m.dropDownHeight == null) {
                    m.dropDownHeight = 200
                }
                m.container.width(parseInt(d) + 25);
                m.container.height(parseInt(m.dropDownHeight) + 25);
                m.addHandler(m.listBoxContainer, "bindingComplete", function(e) {
                    m._raiseEvent("6")
                });
                var j = true;
                m.listBoxContainer.jqxListBox({
                    _checkForHiddenParent: false,
                    allowDrop: false,
                    allowDrag: false,
                    checkboxes: m.checkboxes,
                    emptyString: m.emptyString,
                    autoBind: !m.remoteAutoComplete && m.autoBind,
                    renderer: m.renderer,
                    rtl: m.rtl,
                    itemHeight: m.itemHeight,
                    selectedIndex: m.selectedIndex,
                    incrementalSearch: false,
                    width: d,
                    scrollBarSize: m.scrollBarSize,
                    autoHeight: m.autoDropDownHeight,
                    height: m.dropDownHeight,
                    displayMember: m.displayMember,
                    valueMember: m.valueMember,
                    source: m.source,
                    theme: m.theme,
                    rendered: function() {
                        m.listBox = a.data(m.listBoxContainer[0], "jqxListBox").instance;
                        if (m.remoteAutoComplete) {
                            if (m.autoDropDownHeight) {
                                m.container.height(m.listBox.virtualSize.height + 25);
                                m.listBoxContainer.height(m.listBox.virtualSize.height);
                                m.listBox._arrange()
                            } else {
                                m.listBox._arrange();
                                m.listBox.ensureVisible(0);
                                m.listBox._renderItems();
                                m.container.height(m.listBoxContainer.height() + 25)
                            }
                            if (m.searchString != undefined && m.searchString.length >= m.minLength) {
                                var e = m.listBoxContainer.jqxListBox("items");
                                if (e) {
                                    if (e.length > 0) {
                                        if (!m.isOpened()) {
                                            m.open()
                                        }
                                    } else {
                                        m.close()
                                    }
                                } else {
                                    m.close()
                                }
                            } else {
                                m.close()
                            }
                        } else {
                            m.renderSelection("mouse");
                            if (m.multiSelect) {
                                m.doMultiSelect(false)
                            }
                        }
                        if (m.rendered) {
                            m.rendered()
                        }
                    }
                });
                m.listBoxContainer.css({
                    position: "absolute",
                    zIndex: m.popupZIndex,
                    top: 0,
                    left: 0
                });
                m.listBoxContainer.css("border-top-width", "1px");
                m.listBoxContainer.addClass(m.toThemeProperty("jqx-popup"));
                if (a.jqx.browser.msie) {
                    m.listBoxContainer.addClass(m.toThemeProperty("jqx-noshadow"))
                }
                m.listBox = a.data(m.listBoxContainer[0], "jqxListBox").instance;
                m.listBox.enableSelection = m.enableSelection;
                m.listBox.enableHover = m.enableHover;
                m.listBox.equalItemsWidth = m.equalItemsWidth;
                m.listBox._arrange();
                m.addHandler(m.listBoxContainer, "unselect", function(e) {
                    if (!m.multiSelect) {
                        m._raiseEvent("3", {
                            index: e.args.index,
                            type: e.args.type,
                            item: e.args.item
                        })
                    }
                });
                m.addHandler(m.listBoxContainer, "change", function(e) {
                    if (!m.multiSelect) {
                        m.selectedIndex = m.listBox.selectedIndex;
                        m._raiseEvent("4", {
                            index: e.args.index,
                            type: e.args.type,
                            item: e.args.item
                        })
                    }
                });
                if (m.animationType == "none") {
                    m.container.css("display", "none")
                } else {
                    m.container.hide()
                }
                j = false
            } catch (k) {
                throw k
            }
            var q = this;
            q.input.attr("disabled", q.disabled);
            var h = a.jqx.browser.msie && a.jqx.browser.version < 8;
            if (!h) {
                q.input.attr("placeholder", q.placeHolder)
            }
            m.propertyChangeMap.disabled = function(e, t, s, u) {
                if (u) {
                    e.host.addClass(q.toThemeProperty("jqx-combobox-state-disabled"));
                    e.host.addClass(q.toThemeProperty("jqx-fill-state-disabled"));
                    e.dropdownlistContent.addClass(q.toThemeProperty("jqx-combobox-content-disabled"))
                } else {
                    e.host.removeClass(q.toThemeProperty("jqx-combobox-state-disabled"));
                    e.host.removeClass(q.toThemeProperty("jqx-fill-state-disabled"));
                    e.dropdownlistContent.removeClass(q.toThemeProperty("jqx-combobox-content-disabled"))
                }
                e.input.attr("disabled", e.disabled);
                a.jqx.aria(e, "aria-disabled", e.disabled);
                e.input.attr("disabled", e.disabled)
            };
            if (m.disabled) {
                m.host.addClass(m.toThemeProperty("jqx-combobox-state-disabled"));
                m.host.addClass(m.toThemeProperty("jqx-fill-state-disabled"));
                m.dropdownlistContent.addClass(m.toThemeProperty("jqx-combobox-content-disabled"))
            }
            m.host.addClass(m.toThemeProperty("jqx-combobox-state-normal"));
            m.host.addClass(m.toThemeProperty("jqx-combobox"));
            m.host.addClass(m.toThemeProperty("jqx-rc-all"));
            m.host.addClass(m.toThemeProperty("jqx-widget"));
            m.host.addClass(m.toThemeProperty("jqx-widget-content"));
            m.dropdownlistArrowIcon = a("<div></div>");
            m.dropdownlistArrowIcon.addClass(m.toThemeProperty("jqx-icon-arrow-down"));
            m.dropdownlistArrowIcon.addClass(m.toThemeProperty("jqx-icon"));
            m.dropdownlistArrow.append(m.dropdownlistArrowIcon);
            m.dropdownlistArrow.addClass(m.toThemeProperty("jqx-combobox-arrow-normal"));
            m.dropdownlistArrow.addClass(m.toThemeProperty("jqx-fill-state-normal"));
            if (!m.rtl) {
                m.dropdownlistArrow.addClass(m.toThemeProperty("jqx-rc-r"))
            } else {
                m.dropdownlistArrow.addClass(m.toThemeProperty("jqx-rc-l"))
            }
            m._setSize();
            m._updateHandlers();
            m.addHandler(m.input, "keyup.textchange", function(e) {
                var s = m._search(e);
                if (m.cinput && m.input) {
                    if (!m.displayMember) {
                        m.cinput[0].value = m.input[0].value
                    } else {
                        m._updateInputSelection()
                    }
                }
            });
            if (a.jqx.browser.msie && a.jqx.browser.version < 8) {
                if (m.host.parents(".jqx-window").length > 0) {
                    var n = m.host.parents(".jqx-window").css("z-index");
                    b.css("z-index", n + 10);
                    m.listBoxContainer.css("z-index", n + 10)
                }
            }
            if (m.checkboxes) {
                m.input.attr("readonly", true);
                a.jqx.aria(this, "aria-readonly", true)
            } else {
                a.jqx.aria(this, "aria-readonly", false)
            }
            if (!m.remoteAutoComplete) {
                m.searchString = ""
            }
        },
        _addInput: function() {
            var b = this.host.attr("name");
            this.cinput = a("<input type='hidden'/>");
            this.host.append(this.cinput);
            if (b) {
                this.cinput.attr("name", b)
            }
        },
        _updateInputSelection: function() {
            if (this.cinput) {
                var c = new Array();
                if (this.selectedIndex == -1) {
                    this.cinput.val("")
                } else {
                    var e = this.getSelectedItem();
                    if (e != null) {
                        this.cinput.val(e.value);
                        c.push(e.value)
                    } else {
                        this.cinput.val(this.dropdownlistContent.text())
                    }
                }
                if (this.checkboxes || this.multiSelect) {
                    if (!this.multiSelect) {
                        var b = this.getCheckedItems()
                    } else {
                        var b = this.getSelectedItems()
                    }
                    var f = "";
                    if (b != null) {
                        for (var d = 0; d < b.length; d++) {
                            if (d == b.length - 1) {
                                f += b[d].value
                            } else {
                                f += b[d].value + ","
                            }
                            c.push(b[d].value)
                        }
                    }
                    this.cinput.val(f)
                }
                if (this.field && this.cinput) {
                    if (this.field.nodeName.toLowerCase() == "select") {
                        a.each(this.field, function(g, h) {
                            a(this).removeAttr("selected");
                            this.selected = c.indexOf(this.value) >= 0;
                            if (this.selected) {
                                a(this).attr("selected", true)
                            }
                        })
                    } else {
                        a.each(this.items, function(g, h) {
                            a(this.originalItem.originalItem).removeAttr("data-selected");
                            this.selected = c.indexOf(this.value) >= 0;
                            if (this.selected) {
                                a(this.originalItem.originalItem).attr("data-selected", true)
                            }
                        })
                    }
                }
            }
        },
        _search: function(n) {
            var e = this;
            if (n.keyCode == 9) {
                return
            }
            if (e.searchMode == "none" || e.searchMode == null || e.searchMode == "undefined") {
                return
            }
            if (n.keyCode == 16 || n.keyCode == 17 || n.keyCode == 20) {
                return
            }
            if (e.checkboxes) {
                return
            }
            if (e.multiSelect) {
                var o = a("<span style='visibility: hidden; white-space: nowrap;'>" + e.input.val() + "</span>");
                o.addClass(e.toThemeProperty("jqx-widget"));
                a(document.body).append(o);
                var k = o.width() + 15;
                o.remove();
                if (k > e.host.width()) {
                    k = e.host.width()
                }
                if (k < 25) {
                    k = 25
                }
                e.input.css("width", k + "px");
                if (e.selectedItems.length == 0) {
                    e.input.css("width", "100%");
                    e.input.attr("placeholder", e.placeHolder)
                } else {
                    e.input.attr("placeholder", "")
                }
                var g = parseInt(this._findPos(e.host[0])[1]) + parseInt(e.host.outerHeight()) - 1 + "px";
                var p = a.jqx.mobile.isSafariMobileBrowser() || a.jqx.mobile.isWindowsPhone();
                var c = a.jqx.utilities.hasTransform(e.host);
                if (c || (p != null && p)) {
                    g = a.jqx.mobile.getTopPos(this.element) + parseInt(e.host.outerHeight());
                    if (a("body").css("border-top-width") != "0px") {
                        g = parseInt(g) - this._getBodyOffset().top + "px"
                    }
                }
                e.container.css("top", g);
                var j = parseInt(e.host.height());
                e.dropdownlistArrow.height(j)
            }
            if (!e.isanimating) {
                if (n.altKey && n.keyCode == 38) {
                    e.hideListBox("altKey");
                    return false
                }
                if (n.altKey && n.keyCode == 40) {
                    if (!e.isOpened()) {
                        e.showListBox("altKey")
                    }
                    return false
                }
            }
            if (n.keyCode == 37 || n.keyCode == 39) {
                return false
            }
            if (n.altKey || n.keyCode == 18) {
                return
            }
            if (n.keyCode >= 33 && n.keyCode <= 40) {
                return
            }
            if (n.ctrlKey || e.ctrlKey) {
                if (n.keyCode != 88 && n.keyCode != 86) {
                    return
                }
            }
            var m = e.input.val();
            if (m.length == 0 && !e.autoComplete) {
                e.listBox.searchString = e.input.val();
                e.listBox.clearSelection();
                e.hideListBox("search");
                e.searchString = e.input.val();
                return
            }
            if (e.remoteAutoComplete) {
                var t = this;
                var q = function() {
                    t.listBox.vScrollInstance.value = 0
                };
                if (m.length >= t.minLength) {
                    if (!n.ctrlKey && !n.altKey) {
                        if (t.searchString != m) {
                            var l = t.listBoxContainer.jqxListBox("source");
                            if (l == null) {
                                t.listBoxContainer.jqxListBox({
                                    source: t.source
                                })
                            }
                            if (e._searchTimer) {
                                clearTimeout(e._searchTimer)
                            }
                            if (n.keyCode != 13 && n.keyCode != 27) {
                                e._searchTimer = setTimeout(function() {
                                    q();
                                    if (t.autoDropDownHeight) {
                                        t.listBox.autoHeight = true
                                    }
                                    t.searchString = t.input.val();
                                    if (t.search != null) {
                                        t.search(t.input.val())
                                    } else {
                                        throw "'search' function is not defined"
                                    }
                                }, e.remoteAutoCompleteDelay)
                            }
                        }
                        t.searchString = m
                    }
                } else {
                    if (e._searchTimer) {
                        clearTimeout(e._searchTimer)
                    }
                    q();
                    t.searchString = "";
                    t.listBoxContainer.jqxListBox({
                        source: null
                    })
                }
                return
            }
            var t = this;
            if (m === t.searchString) {
                return
            }
            if (!(n.keyCode == "27" || n.keyCode == "13")) {
                var b = e._updateItemsVisibility(m);
                var f = b.matchItems;
                var d = b.index;
                if (!e.autoComplete && !e.removeAutoComplete) {
                    if (!e.multiSelect || (e.multiSelect && d >= 0)) {
                        e.listBox.selectIndex(d);
                        var s = e.listBox.isIndexInView(d);
                        if (!s) {
                            e.listBox.ensureVisible(d)
                        } else {
                            e.listBox._renderItems()
                        }
                    }
                }
                if (e.autoComplete && f.length === 0) {
                    e.hideListBox("search")
                }
            }
            if (n.keyCode == "13") {
                var h = e.container.css("display") == "block";
                if (h && !e.isanimating) {
                    e.hideListBox("keyboard");
                    e._oldvalue = e.listBox.selectedValue;
                    return
                }
            } else {
                if (n.keyCode == "27") {
                    var h = e.container.css("display") == "block";
                    if (h && !e.isanimating) {
                        if (!i.multiSelect) {
                            var r = e.listBox.getVisibleItem(e._oldvalue);
                            if (r) {
                                var i = this;
                                setTimeout(function() {
                                    if (i.autoComplete) {
                                        i._updateItemsVisibility("")
                                    }
                                    i.listBox.selectIndex(r.index);
                                    i.renderSelection("api")
                                }, i.closeDelay)
                            } else {
                                e.clearSelection()
                            }
                        } else {
                            i.input.val("");
                            i.listBox.selectedValue = null
                        }
                        e.hideListBox("keyboard");
                        e.renderSelection("api");
                        n.preventDefault();
                        return false
                    }
                } else {
                    if (!e.isOpened() && !e.opening && !n.ctrlKey) {
                        if (e.listBox.visibleItems && e.listBox.visibleItems.length > 0) {
                            if (e.input.val() != e.searchString && e.searchString != undefined && d != -1) {
                                e.showListBox("search")
                            }
                        }
                    }
                    e.searchString = e.input.val();
                    if (e.searchString == "") {
                        if (!e.listBox.itemsByValue[""]) {
                            d = -1;
                            e.clearSelection()
                        }
                    }
                    var r = e.listBox.getVisibleItem(d);
                    if (r != undefined) {
                        e._updateInputSelection()
                    }
                }
            }
        },
        val: function(c) {
            if (!this.input) {
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
            if (d(c) || arguments.length == 0) {
                var b = this.getSelectedItem();
                if (b) {
                    return b.value
                }
                return this.input.val()
            } else {
                var b = this.getItemByValue(c);
                if (b != null) {
                    this.selectItem(b)
                } else {
                    this.input.val(c)
                }
                return this.input.val()
            }
        },
        focus: function() {
            var c = this;
            var b = function() {
                c.input.focus();
                var d = c.input.val();
                c._setSelection(0, d.length)
            };
            b();
            setTimeout(function() {
                b()
            }, 10)
        },
        _setSelection: function(e, b) {
            try {
                if ("selectionStart" in this.input[0]) {
                    this.input[0].focus();
                    this.input[0].setSelectionRange(e, b)
                } else {
                    var c = this.input[0].createTextRange();
                    c.collapse(true);
                    c.moveEnd("character", b);
                    c.moveStart("character", e);
                    c.select()
                }
            } catch (d) {}
        },
        setContent: function(b) {
            this.input.val(b)
        },
        _updateItemsVisibility: function(l) {
            var i = this.getItems();
            if (i == undefined) {
                return {
                    index: -1,
                    matchItem: new Array()
                }
            }
            var j = this;
            var g = -1;
            var m = new Array();
            var k = 0;
            a.each(i, function(p) {
                var r = "";
                if (!this.isGroup) {
                    if (this.label) {
                        r = this.label
                    } else {
                        if (this.value) {
                            r = this.value
                        } else {
                            if (this.title) {
                                r = this.title
                            } else {
                                r = "jqxItem"
                            }
                        }
                    }
                    r = r.toString();
                    var q = false;
                    switch (j.searchMode) {
                        case "containsignorecase":
                            q = a.jqx.string.containsIgnoreCase(r, l);
                            break;
                        case "contains":
                            q = a.jqx.string.contains(r, l);
                            break;
                        case "equals":
                            q = a.jqx.string.equals(r, l);
                            break;
                        case "equalsignorecase":
                            q = a.jqx.string.equalsIgnoreCase(r, l);
                            break;
                        case "startswith":
                            q = a.jqx.string.startsWith(r, l);
                            break;
                        case "startswithignorecase":
                            q = a.jqx.string.startsWithIgnoreCase(r, l);
                            break;
                        case "endswith":
                            q = a.jqx.string.endsWith(r, l);
                            break;
                        case "endswithignorecase":
                            q = a.jqx.string.endsWithIgnoreCase(r, l);
                            break
                    }
                    if (j.autoComplete && !q) {
                        this.visible = false
                    }
                    if (q && j.autoComplete) {
                        m[k++] = this;
                        this.visible = true;
                        g = this.visibleIndex
                    }
                    if (l == "" && j.autoComplete) {
                        this.visible = true;
                        q = false
                    }
                    if (j.multiSelect) {
                        this.disabled = false;
                        if (j.selectedItems.indexOf(this.value) >= 0 || j._disabledItems.indexOf(this.value) >= 0) {
                            this.disabled = true;
                            q = false
                        }
                    }
                    if (!j.multiSelect) {
                        if (q && !j.autoComplete) {
                            g = this.visibleIndex;
                            return false
                        }
                    } else {
                        if (q && !j.autoComplete) {
                            if (g === -1) {
                                g = this.visibleIndex
                            }
                            return true
                        }
                    }
                }
            });
            this.listBox.searchString = l;
            var f = this;
            var h = function() {
                if (!f.multiSelect) {
                    return
                }
                var p = 0;
                var s = false;
                var r = null;
                for (var q = 0; q < f.listBox.items.length; q++) {
                    f.listBox.selectedIndexes[q] = -1;
                    if (!f.listBox.items[q].disabled) {
                        if (s == false) {
                            r = f.listBox.items[q];
                            p = r.visibleIndex;
                            s = true
                        }
                    }
                }
                f.listBox.selectedIndex = -1;
                f.listBox.selectedIndex = p;
                f.listBox.selectedIndexes[p] = p;
                if (f.listBox.visibleItems.length > 0) {
                    if (r) {
                        f.listBox.selectedValue = r.value
                    } else {
                        f.listBox.selectedValue = null
                    }
                } else {
                    f.listBox.selectedValue = null
                }
                f.listBox.ensureVisible(0)
            };
            if (!this.autoComplete) {
                h();
                return {
                    index: g,
                    matchItems: m
                }
            }
            this.listBox.renderedVisibleItems = new Array();
            var b = this.listBox.vScrollInstance.value;
            this.listBox.vScrollInstance.value = 0;
            this.listBox.visibleItems = new Array();
            this.listBox._renderItems();
            var e = this.listBox.selectedValue;
            var o = this.listBox.getItemByValue(e);
            if (!this.multiSelect) {
                if (o) {
                    if (o.visible) {
                        this.listBox.selectedIndex = o.visibleIndex;
                        for (var d = 0; d < this.listBox.items.length; d++) {
                            this.listBox.selectedIndexes[d] = -1
                        }
                        this.listBox.selectedIndexes[o.visibleIndex] = o.visibleIndex
                    } else {
                        for (var d = 0; d < this.listBox.items.length; d++) {
                            this.listBox.selectedIndexes[d] = -1
                        }
                        this.listBox.selectedIndex = -1
                    }
                }
            } else {
                h()
            }
            this.listBox._renderItems();
            var n = this.listBox._calculateVirtualSize().height;
            if (n < b) {
                b = 0;
                this.listBox.vScrollInstance.refresh()
            }
            if (this.autoDropDownHeight) {
                this._disableSelection = true;
                if (this.listBox.autoHeight != this.autoDropDownHeight) {
                    this.listBoxContainer.jqxListBox({
                        autoHeight: this.autoDropDownHeight
                    })
                }
                this.container.height(n + 25);
                this.listBox.invalidate();
                this._disableSelection = false
            } else {
                if (n < parseInt(this.dropDownHeight)) {
                    var c = this.listBox.hScrollBar[0].style.visibility == "hidden" ? 0 : 20;
                    this.listBox.height = c + n;
                    this.container.height(n + 25 + c);
                    this.listBox.invalidate()
                } else {
                    this.listBox.height = parseInt(this.dropDownHeight);
                    this.container.height(parseInt(this.dropDownHeight) + 25);
                    this.listBox.invalidate()
                }
            }
            this.listBox.vScrollInstance.setPosition(b);
            return {
                index: g,
                matchItems: m
            }
        },
        findItems: function(e) {
            var b = this.getItems();
            var d = this;
            var c = 0;
            var f = new Array();
            a.each(b, function(g) {
                var j = "";
                if (!this.isGroup) {
                    if (this.label) {
                        j = this.label
                    } else {
                        if (this.value) {
                            j = this.value
                        } else {
                            if (this.title) {
                                j = this.title
                            } else {
                                j = "jqxItem"
                            }
                        }
                    }
                    var h = false;
                    switch (d.searchMode) {
                        case "containsignorecase":
                            h = a.jqx.string.containsIgnoreCase(j, e);
                            break;
                        case "contains":
                            h = a.jqx.string.contains(j, e);
                            break;
                        case "equals":
                            h = a.jqx.string.equals(j, e);
                            break;
                        case "equalsignorecase":
                            h = a.jqx.string.equalsIgnoreCase(j, e);
                            break;
                        case "startswith":
                            h = a.jqx.string.startsWith(j, e);
                            break;
                        case "startswithignorecase":
                            h = a.jqx.string.startsWithIgnoreCase(j, e);
                            break;
                        case "endswith":
                            h = a.jqx.string.endsWith(j, e);
                            break;
                        case "endswithignorecase":
                            h = a.jqx.string.endsWithIgnoreCase(j, e);
                            break
                    }
                    if (h) {
                        f[c++] = this
                    }
                }
            });
            return f
        },
        _resetautocomplete: function() {
            a.each(this.listBox.items, function(b) {
                this.visible = true
            });
            this.listBox.vScrollInstance.value = 0;
            this.listBox._addItems();
            this.listBox.autoHeight = false;
            this.listBox.height = this.dropDownHeight;
            this.container.height(parseInt(this.dropDownHeight) + 25);
            this.listBoxContainer.height(parseInt(this.dropDownHeight));
            this.listBox._arrange();
            this.listBox._addItems();
            this.listBox._renderItems()
        },
        getItems: function() {
            var b = this.listBox.items;
            return b
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
            if (e) {
                var c = this;
                var b = this.host.width();
                if (this.dropDownWidth != "auto") {
                    b = this.dropDownWidth
                }
                this.listBoxContainer.jqxListBox({
                    width: b
                });
                this.container.width(parseInt(b) + 25);
                this._arrange()
            }
            var c = this;
            var d = function() {
                if (c.multiSelect) {
                    c.host.height(c.height)
                }
                c._arrange();
                if (c.multiSelect) {
                    c.host.height("auto")
                }
            };
            a.jqx.utilities.resize(this.host, function() {
                d();
                c.hideListBox("api")
            })
        },
        isOpened: function() {
            var c = this;
            var b = a.data(document.body, "openedComboJQXListBox" + this.element.id);
            if (this.container.css("display") != "block") {
                return false
            }
            if (b != null && b == c.listBoxContainer) {
                return true
            }
            return false
        },
        _updateHandlers: function() {
            var d = this;
            var e = false;
            this.removeHandlers();
            if (this.multiSelect) {
                this.addHandler(this.dropdownlistContent, "click", function(f) {
                    if (f.target.href) {
                        return false
                    }
                    d.input.focus();
                    setTimeout(function() {
                        d.input.focus()
                    }, 10)
                });
                this.addHandler(this.dropdownlistContent, "focus", function(f) {
                    if (f.target.href) {
                        return false
                    }
                    d.input.focus();
                    setTimeout(function() {
                        d.input.focus()
                    }, 10)
                })
            }
            if (!this.touch) {
                if (this.host.parents()) {
                    this.addHandler(this.host.parents(), "scroll.combobox" + this.element.id, function(f) {
                        var g = d.isOpened();
                        if (g) {
                            d.close()
                        }
                    })
                }
                this.addHandler(this.host, "mouseenter", function() {
                    if (!d.disabled && d.enableHover) {
                        e = true;
                        d.host.addClass(d.toThemeProperty("jqx-combobox-state-hover"));
                        d.dropdownlistArrowIcon.addClass(d.toThemeProperty("jqx-icon-arrow-down-hover"));
                        d.dropdownlistArrow.addClass(d.toThemeProperty("jqx-combobox-arrow-hover"));
                        d.dropdownlistArrow.addClass(d.toThemeProperty("jqx-fill-state-hover"))
                    }
                });
                this.addHandler(this.host, "mouseleave", function() {
                    if (!d.disabled && d.enableHover) {
                        d.host.removeClass(d.toThemeProperty("jqx-combobox-state-hover"));
                        d.dropdownlistArrowIcon.removeClass(d.toThemeProperty("jqx-icon-arrow-down-hover"));
                        d.dropdownlistArrow.removeClass(d.toThemeProperty("jqx-combobox-arrow-hover"));
                        d.dropdownlistArrow.removeClass(d.toThemeProperty("jqx-fill-state-hover"));
                        e = false
                    }
                })
            }
            if (d.autoOpen) {
                this.addHandler(this.host, "mouseenter", function() {
                    var f = d.isOpened();
                    if (!f && d.autoOpen) {
                        d.open();
                        d.host.focus()
                    }
                });
                this.addHandler(a(document), "mousemove." + d.id, function(i) {
                    var h = d.isOpened();
                    if (h && d.autoOpen) {
                        var m = d.host.coord();
                        var l = m.top;
                        var k = m.left;
                        var j = d.container.coord();
                        var f = j.left;
                        var g = j.top;
                        canClose = true;
                        if (i.pageY >= l && i.pageY <= l + d.host.height() + 2) {
                            if (i.pageX >= k && i.pageX < k + d.host.width()) {
                                canClose = false
                            }
                        }
                        if (i.pageY >= g && i.pageY <= g + d.container.height() - 20) {
                            if (i.pageX >= f && i.pageX < f + d.container.width()) {
                                canClose = false
                            }
                        }
                        if (canClose) {
                            d.close()
                        }
                    }
                })
            }
            var c = "mousedown";
            if (this.touch) {
                c = a.jqx.mobile.getTouchEventName("touchstart")
            }
            var b = function(h) {
                if (!d.disabled) {
                    var f = d.container.css("display") == "block";
                    if (!d.isanimating) {
                        if (f) {
                            d.hideListBox("api");
                            if (!a.jqx.mobile.isTouchDevice()) {
                                d.input.focus();
                                setTimeout(function() {
                                    d.input.focus()
                                }, 10)
                            }
                            return true
                        } else {
                            if (d.autoDropDownHeight) {
                                d.container.height(d.listBoxContainer.height() + 25);
                                var g = d.listBoxContainer.jqxListBox("autoHeight");
                                if (!g) {
                                    d.listBoxContainer.jqxListBox({
                                        autoHeight: d.autoDropDownHeight
                                    });
                                    d.listBox._arrange();
                                    d.listBox.ensureVisible(0);
                                    d.listBox._renderItems();
                                    d.container.height(d.listBoxContainer.height() + 25)
                                }
                            }
                            d.showListBox("api");
                            if (!a.jqx.mobile.isTouchDevice()) {
                                setTimeout(function() {
                                    d.input.focus()
                                }, 10)
                            } else {
                                return true
                            }
                        }
                    }
                }
            };
            this.addHandler(this.dropdownlistArrow, c, function(f) {
                b(f)
            });
            this.addHandler(this.dropdownlistArrowIcon, c, function(f) {});
            this.addHandler(this.host, "focus", function() {
                d.focus()
            });
            this.addHandler(this.input, "focus", function(f) {
                d.focused = true;
                d.host.addClass(d.toThemeProperty("jqx-combobox-state-focus"));
                d.host.addClass(d.toThemeProperty("jqx-fill-state-focus"));
                d.dropdownlistContent.addClass(d.toThemeProperty("jqx-combobox-content-focus"));
                if (f.stopPropagation) {
                    f.stopPropagation()
                }
                if (f.preventDefault) {
                    f.preventDefault()
                }
                return false
            });
            this.addHandler(this.input, "blur", function() {
                d.focused = false;
                if (!d.isOpened() && !d.opening) {
                    if (d.selectionMode == "dropDownList") {
                        d._selectOldValue()
                    }
                    d.host.removeClass(d.toThemeProperty("jqx-combobox-state-focus"));
                    d.host.removeClass(d.toThemeProperty("jqx-fill-state-focus"));
                    d.dropdownlistContent.removeClass(d.toThemeProperty("jqx-combobox-content-focus"))
                }
                if (d._searchTimer) {
                    clearTimeout(d._searchTimer)
                }
            });
            this.addHandler(a(document), "mousedown." + this.id, d.closeOpenedListBox, {
                me: this,
                listbox: this.listBox,
                id: this.id
            });
            if (this.touch) {
                this.addHandler(a(document), a.jqx.mobile.getTouchEventName("touchstart") + "." + this.id, d.closeOpenedListBox, {
                    me: this,
                    listbox: this.listBox,
                    id: this.id
                })
            }
            this.addHandler(this.host, "keydown", function(k) {
                var h = d.container.css("display") == "block";
                d.ctrlKey = k.ctrlKey;
                if (d.host.css("display") == "none") {
                    return true
                }
                if (k.keyCode == "13" || k.keyCode == "9") {
                    if (h && !d.isanimating) {
                        if (d.listBox.selectedIndex != -1) {
                            d.renderSelection("mouse");
                            var f = d.listBox.selectedIndex;
                            var j = d.listBox.getVisibleItem(f);
                            if (j) {
                                d.listBox.selectedValue = j.value
                            }
                            d._setSelection(d.input.val().length, d.input.val().length);
                            d.hideListBox("keyboard")
                        }
                        if (k.keyCode == "13") {
                            d._oldvalue = d.listBox.selectedValue
                        }
                        if (!d.keyboardSelection) {
                            d._raiseEvent("2", {
                                index: d.selectedIndex,
                                type: "keyboard",
                                item: d.getItem(d.selectedIndex)
                            })
                        }
                        if (k.keyCode == "9") {
                            return true
                        }
                        return false
                    }
                }
                if (k.keyCode == 115) {
                    if (!d.isanimating) {
                        if (!d.isOpened()) {
                            d.showListBox("keyboard")
                        } else {
                            if (d.isOpened()) {
                                d.hideListBox("keyboard")
                            }
                        }
                    }
                    return false
                }
                if (k.altKey) {
                    if (d.host.css("display") == "block") {
                        if (!d.isanimating) {
                            if (k.keyCode == 38) {
                                if (d.isOpened()) {
                                    d.hideListBox("altKey")
                                }
                            } else {
                                if (k.keyCode == 40) {
                                    if (!d.isOpened()) {
                                        d.showListBox("altKey")
                                    }
                                }
                            }
                        }
                    }
                }
                if (k.keyCode == "27" || k.keyCode == "9") {
                    if (d.isOpened() && !d.isanimating) {
                        if (k.keyCode == "27") {
                            if (!d.multiSelect) {
                                var j = d.listBox.getItemByValue(d._oldvalue);
                                if (j) {
                                    setTimeout(function() {
                                        if (d.autoComplete) {
                                            d._updateItemsVisibility("")
                                        }
                                        d.listBox.selectIndex(j.index);
                                        d.renderSelection("api")
                                    }, d.closeDelay)
                                } else {
                                    d.clearSelection()
                                }
                            } else {
                                d.listBox.selectedValue = null;
                                d.input.val("")
                            }
                        }
                        d.hideListBox("keyboard");
                        if (k.keyCode == "9") {
                            return true
                        }
                        d.renderSelection("api");
                        k.preventDefault();
                        return false
                    }
                }
                var g = k.keyCode;
                if (h && !d.disabled && g != 8) {
                    return d.listBox._handleKeyDown(k)
                } else {
                    if (!d.disabled && !h) {
                        var g = k.keyCode;
                        if (g == 33 || g == 34 || g == 35 || g == 36 || g == 38 || g == 40) {
                            return d.listBox._handleKeyDown(k)
                        }
                    }
                }
                if (g === 8 && d.multiSelect) {
                    if (d.input.val().length === 0) {
                        var i = d.selectedItems[d.selectedItems.length - 1];
                        d.selectedItems.pop();
                        d._selectedItems.pop();
                        if (i) {
                            d._raiseEvent("3", {
                                index: i.index,
                                type: "keyboard",
                                item: i
                            });
                            d._raiseEvent("4", {
                                index: i.index,
                                type: "keyboard",
                                item: i
                            })
                        }
                        d.listBox.selectedValue = null;
                        d.doMultiSelect();
                        return false
                    }
                }
            });
            this.addHandler(this.listBoxContainer, "checkChange", function(f) {
                d.renderSelection("mouse");
                d._updateInputSelection();
                d._raiseEvent(5, {
                    label: f.args.label,
                    value: f.args.value,
                    checked: f.args.checked,
                    item: f.args.item
                })
            });
            this.addHandler(this.listBoxContainer, "select", function(f) {
                if (!d.disabled) {
                    if (f.args.type != "keyboard" || d.keyboardSelection) {
                        d.renderSelection(f.args.type);
                        if (!d.multiSelect) {
                            d._raiseEvent("2", {
                                index: f.args.index,
                                type: f.args.type,
                                item: f.args.item
                            })
                        }
                        if (f.args.type == "mouse") {
                            d._oldvalue = d.listBox.selectedValue;
                            if (!d.checkboxes) {
                                d.hideListBox("mouse");
                                if (!d.touch) {
                                    d.input.focus()
                                } else {
                                    return false
                                }
                            }
                        }
                    }
                }
            });
            if (this.listBox != null && this.listBox.content != null) {
                this.addHandler(this.listBox.content, "click", function(f) {
                    if (!d.disabled) {
                        if (d.listBox.itemswrapper) {
                            if (f.target === d.listBox.itemswrapper[0]) {
                                return true
                            }
                        }
                        if (f.target && f.target.className) {
                            if (f.target.className.indexOf("jqx-fill-state-disabled") >= 0) {
                                return true
                            }
                        }
                        d.renderSelection("mouse");
                        d._oldvalue = d.listBox.selectedValue;
                        if (!d.touch && !d.ishiding) {
                            if (!d.checkboxes) {
                                d.hideListBox("mouse");
                                d.input.focus()
                            }
                        }
                        if (d.touch === true) {
                            if (!d.checkboxes) {
                                d.hideListBox("mouse")
                            }
                        }
                    }
                })
            }
        },
        _selectOldValue: function() {
            var b = this;
            if (b.listBox.selectedIndex == -1) {
                if (!b.multiSelect) {
                    var c = b.listBox.getItemByValue(b._oldvalue);
                    if (c) {
                        setTimeout(function() {
                            if (b.autoComplete) {
                                b._updateItemsVisibility("")
                            }
                            b.listBox.selectIndex(c.index);
                            b.renderSelection("api")
                        }, b.closeDelay)
                    } else {
                        b.clearSelection();
                        b.listBox.selectIndex(0);
                        b.renderSelection("api")
                    }
                } else {
                    b.listBox.selectedValue = null;
                    b.input.val("")
                }
            } else {
                b.renderSelection("api")
            }
        },
        removeHandlers: function() {
            var c = this;
            if (this.dropdownlistWrapper != null) {
                this.removeHandler(this.dropdownlistWrapper, "mousedown")
            }
            if (this.dropdownlistContent) {
                this.removeHandler(this.dropdownlistContent, "click");
                this.removeHandler(this.dropdownlistContent, "focus")
            }
            this.removeHandler(this.host, "keydown");
            this.removeHandler(this.host, "focus");
            if (this.input != null) {
                this.removeHandler(this.input, "focus");
                this.removeHandler(this.input, "blur")
            }
            this.removeHandler(this.host, "mouseenter");
            this.removeHandler(this.host, "mouseleave");
            this.removeHandler(a(document), "mousemove." + c.id);
            if (this.listBoxContainer) {
                this.removeHandler(this.listBoxContainer, "checkChange");
                this.removeHandler(this.listBoxContainer, "select")
            }
            if (this.host.parents()) {
                this.removeHandler(this.host.parents(), "scroll.combobox" + this.element.id)
            }
            if (this.dropdownlistArrowIcon && this.dropdownlistArrow) {
                var b = "mousedown";
                if (this.touch) {
                    b = a.jqx.mobile.getTouchEventName("touchstart")
                }
                this.removeHandler(this.dropdownlistArrowIcon, b);
                this.removeHandler(this.dropdownlistArrow, b)
            }
        },
        getItem: function(b) {
            var c = this.listBox.getItem(b);
            return c
        },
        getItemByValue: function(c) {
            var b = this.listBox.getItemByValue(c);
            return b
        },
        getVisibleItem: function(b) {
            var c = this.listBox.getVisibleItem(b);
            return c
        },
        renderSelection: function(j) {
            if (j == undefined || j == "none") {
                return
            }
            if (this._disableSelection === true) {
                return
            }
            if (this.listBox == null) {
                return
            }
            if (this.multiSelect) {
                return
            }
            var k = this.listBox.visibleItems[this.listBox.selectedIndex];
            if (this.autoComplete && !this.checkboxes) {
                if (this.listBox.selectedValue !== undefined) {
                    var k = this.getItemByValue(this.listBox.selectedValue)
                }
            }
            if (this.checkboxes) {
                var f = this.getCheckedItems();
                if (f != null && f.length > 0) {
                    k = f[0]
                } else {
                    k = null
                }
            }
            if (k == null) {
                var d = a.jqx.browser.msie && a.jqx.browser.version < 8;
                this.input.val("");
                if (!d) {
                    this.input.attr("placeholder", this.placeHolder)
                }
                this._updateInputSelection();
                return
            }
            this.selectedIndex = this.listBox.selectedIndex;
            var c = a("<span></span>");
            if (k.label != undefined && k.label != null && k.label.toString().length > 0) {
                a.jqx.utilities.html(c, k.label)
            } else {
                if (k.value != undefined && k.value != null && k.value.toString().length > 0) {
                    a.jqx.utilities.html(c, k.value)
                } else {
                    if (k.title != undefined && k.title != null && k.title.toString().length > 0) {
                        a.jqx.utilities.html(c, k.title)
                    } else {
                        a.jqx.utilities.html(c, this.emptyString)
                    }
                }
            }
            var b = c.outerHeight();
            if (this.checkboxes) {
                var g = this.getCheckedItems();
                var h = "";
                for (var e = 0; e < g.length; e++) {
                    if (e == g.length - 1) {
                        h += g[e].label
                    } else {
                        h += g[e].label + ", "
                    }
                }
                this.input.val(h)
            } else {
                this.input.val(c.text())
            }
            c.remove();
            this._updateInputSelection();
            if (this.renderSelectedItem) {
                var l = this.renderSelectedItem(this.listBox.selectedIndex, k);
                if (l != undefined) {
                    this.input[0].value = l
                }
            }
            if (this.listBox && this.listBox._activeElement) {
                a.jqx.aria(this, "aria-activedescendant", this.listBox._activeElement.id)
            }
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
            this.listBox.clearSelection();
            this.input.val("");
            if (this.multiSelect) {
                this.listBox.selectedValue = "";
                this.selectedItems = new Array();
                this._selectedItems = new Array();
                this.doMultiSelect(false)
            }
        },
        unselectIndex: function(c, d) {
            if (isNaN(c)) {
                return
            }
            if (this.autoComplete) {
                this._updateItemsVisibility("")
            }
            this.listBox.unselectIndex(c, d);
            this.renderSelection("mouse");
            if (this.multiSelect) {
                if (c >= 0) {
                    var b = this.getItem(c);
                    var e = this.selectedItems.indexOf(b.value);
                    if (e >= 0) {
                        if (b.value === this.listBox.selectedValue) {
                            this.listBox.selectedValue = null
                        }
                        this.selectedItems.splice(e, 1);
                        this._selectedItems.splice(e, 1)
                    }
                }
                this.doMultiSelect(false)
            }
        },
        selectIndex: function(b, d, e, c) {
            if (this.autoComplete) {
                this._updateItemsVisibility("")
            }
            this.listBox.selectIndex(b, d, e, c);
            this.renderSelection("mouse");
            this.selectedIndex = b;
            if (this.multiSelect) {
                this.doMultiSelect()
            }
        },
        selectItem: function(b) {
            if (this.autoComplete) {
                this._updateItemsVisibility("")
            }
            if (this.listBox != undefined) {
                this.listBox.selectedIndex = -1;
                this.listBox.selectItem(b);
                this.selectedIndex = this.listBox.selectedIndex;
                this.renderSelection("mouse");
                if (this.multiSelect) {
                    this.doMultiSelect(false)
                }
            }
        },
        unselectItem: function(d) {
            if (this.autoComplete) {
                this._updateItemsVisibility("")
            }
            if (this.listBox != undefined) {
                this.listBox.unselectItem(d);
                this.renderSelection("mouse");
                if (this.multiSelect) {
                    var b = this.getItemByValue(d);
                    if (b) {
                        var c = this.selectedItems.indexOf(b.value);
                        if (c >= 0) {
                            if (b.value === this.listBox.selectedValue) {
                                this.listBox.selectedValue = null
                            }
                            this.selectedItems.splice(c, 1);
                            this._selectedItems.splice(c, 1)
                        }
                    }
                    this.doMultiSelect(false)
                }
            }
        },
        checkItem: function(b) {
            if (this.autoComplete) {
                this._updateItemsVisibility("")
            }
            if (this.listBox != undefined) {
                this.listBox.checkItem(b)
            }
        },
        uncheckItem: function(b) {
            if (this.autoComplete) {
                this._updateItemsVisibility("")
            }
            if (this.listBox != undefined) {
                this.listBox.uncheckItem(b)
            }
        },
        indeterminateItem: function(b) {
            if (this.autoComplete) {
                this._updateItemsVisibility("")
            }
            if (this.listBox != undefined) {
                this.listBox.indeterminateItem(b)
            }
        },
        getSelectedValue: function() {
            return this.listBox.selectedValue
        },
        getSelectedIndex: function() {
            return this.listBox.selectedIndex
        },
        getSelectedItem: function() {
            return this.getVisibleItem(this.listBox.selectedIndex)
        },
        getSelectedItems: function() {
            if (this.remoteAutoComplete && this.multiSelect) {
                return this._selectedItems
            }
            var c = new Array();
            var b = this;
            a.each(this.selectedItems, function() {
                var d = b.getItemByValue(this);
                if (d) {
                    c.push(d)
                } else {
                    var d = b._selectedItems[this];
                    if (d) {
                        c.push(d)
                    }
                }
            });
            return c
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
        insertAt: function(c, b) {
            if (c == null) {
                return false
            }
            return this.listBox.insertAt(c, b)
        },
        addItem: function(b) {
            return this.listBox.addItem(b)
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
            var c = this.getVisibleItem(b);
            if (c) {
                this._disabledItems.push(c.value)
            }
            return this.listBox.disableAt(b)
        },
        enableAt: function(b) {
            var c = this.getVisibleItem(b);
            if (c) {
                this._disabledItems.splice(this._disabledItems.indexOf(c.value), 1)
            }
            return this.listBox.enableAt(b)
        },
        disableItem: function(b) {
            var b = this.getVisibleItem(b);
            if (b) {
                this._disabledItems.push(b.value)
            }
            return this.listBox.disableItem(b)
        },
        enableItem: function(b) {
            var b = this.getVisibleItem(b);
            if (b) {
                this._disabledItems.splice(this._disabledItems.indexOf(b.value), 1)
            }
            return this.listBox.enableItem(b)
        },
        _findPos: function(c) {
            while (c && (c.type == "hidden" || c.nodeType != 1 || a.expr.filters.hidden(c))) {
                c = c.nextSibling
            }
            if (c) {
                var b = a(c).coord(true);
                return [b.left, b.top]
            }
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
            f.top -= Math.min(f.top, (f.top + j > e && e > j) ? Math.abs(j + c + 23) : 0);
            return f
        },
        open: function() {
            if (!this.isOpened() && !this.opening) {
                this.showListBox("api")
            }
        },
        close: function() {
            if (this.isOpened()) {
                this.hideListBox("api")
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
        showListBox: function(l) {
            if (this.listBox.items && this.listBox.items.length == 0) {
                return
            }
            if (l == "search" && !this.autoComplete && !this.remoteAutoComplete) {
                if (this.autoDropDownHeight) {
                    this.container.height(this.listBoxContainer.height() + 25)
                }
            }
            if (this.autoComplete || this.multiSelect && !this.remoteAutoComplete) {
                if (l != "search") {
                    this._updateItemsVisibility("");
                    if (this.multiSelect) {
                        var p = this.getVisibleItems();
                        for (var t = 0; t < p.length; t++) {
                            if (!p[t].disabled) {
                                this.ensureVisible(t);
                                break
                            }
                        }
                    }
                }
            }
            if (this.remoteAutoComplete) {
                this.listBox.clearSelection()
            }
            if (l != "search") {
                this._oldvalue = this.listBox.selectedValue
            }
            a.jqx.aria(this, "aria-expanded", true);
            if (this.dropDownWidth == "auto" && this.width != null && this.width.indexOf && this.width.indexOf("%") != -1) {
                if (this.listBox.host.width() != this.host.width()) {
                    var r = this.host.width();
                    this.listBoxContainer.jqxListBox({
                        width: r
                    });
                    this.container.width(parseInt(r) + 25)
                }
            }
            var o = this;
            var h = this.listBoxContainer;
            var v = this.listBox;
            var e = a(window).scrollTop();
            var f = a(window).scrollLeft();
            var m = parseInt(this._findPos(this.host[0])[1]) + parseInt(this.host.outerHeight()) - 1 + "px";
            var d, q = parseInt(Math.round(this.host.coord(true).left));
            d = q + "px";
            var u = a.jqx.mobile.isSafariMobileBrowser() || a.jqx.mobile.isWindowsPhone();
            this.ishiding = false;
            var g = a.jqx.utilities.hasTransform(this.host);
            if (g || (u != null && u)) {
                d = a.jqx.mobile.getLeftPos(this.element);
                m = a.jqx.mobile.getTopPos(this.element) + parseInt(this.host.outerHeight());
                if (a("body").css("border-top-width") != "0px") {
                    m = parseInt(m) - this._getBodyOffset().top + "px"
                }
                if (a("body").css("border-left-width") != "0px") {
                    d = parseInt(d) - this._getBodyOffset().left + "px"
                }
            }
            this.host.addClass(this.toThemeProperty("jqx-combobox-state-selected"));
            this.dropdownlistArrowIcon.addClass(this.toThemeProperty("jqx-icon-arrow-down-selected"));
            this.dropdownlistArrow.addClass(this.toThemeProperty("jqx-combobox-arrow-selected"));
            this.dropdownlistArrow.addClass(this.toThemeProperty("jqx-fill-state-pressed"));
            this.host.addClass(this.toThemeProperty("jqx-combobox-state-focus"));
            this.host.addClass(this.toThemeProperty("jqx-fill-state-focus"));
            this.dropdownlistContent.addClass(this.toThemeProperty("jqx-combobox-content-focus"));
            this.container.css("left", d);
            this.container.css("top", m);
            v._arrange();
            var c = true;
            var b = false;
            if (this.dropDownHorizontalAlignment == "right" || this.rtl) {
                var k = this.container.outerWidth();
                var s = Math.abs(k - this.host.width());
                if (k > this.host.width()) {
                    this.container.css("left", 25 + parseInt(Math.round(q)) - s + "px")
                } else {
                    this.container.css("left", 25 + parseInt(Math.round(q)) + s + "px")
                }
            }
            if (this.enableBrowserBoundsDetection) {
                var j = this.testOffset(h, {
                    left: parseInt(this.container.css("left")),
                    top: parseInt(m)
                }, parseInt(this.host.outerHeight()));
                if (parseInt(this.container.css("top")) != j.top) {
                    b = true;
                    h.css("top", 23);
                    h.addClass(this.toThemeProperty("jqx-popup-up"))
                } else {
                    h.css("top", 0)
                }
                this.container.css("top", j.top);
                this.container.css("top", j.top);
                if (parseInt(this.container.css("left")) != j.left) {
                    this.container.css("left", j.left)
                }
            }
            if (this.animationType == "none") {
                this.container.css("display", "block");
                a.data(document.body, "openedComboJQXListBoxParent", o);
                a.data(document.body, "openedComboJQXListBox" + o.element.id, h);
                h.css("margin-top", 0);
                h.css("opacity", 1)
            } else {
                this.container.css("display", "block");
                var n = h.outerHeight();
                h.stop();
                if (this.animationType == "fade") {
                    h.css("margin-top", 0);
                    h.css("opacity", 0);
                    h.animate({
                        opacity: 1
                    }, this.openDelay, function() {
                        o.isanimating = false;
                        o.opening = false;
                        a.data(document.body, "openedComboJQXListBoxParent", o);
                        a.data(document.body, "openedComboJQXListBox" + o.element.id, h)
                    })
                } else {
                    h.css("opacity", 1);
                    if (b) {
                        h.css("margin-top", n)
                    } else {
                        h.css("margin-top", -n)
                    }
                    this.isanimating = true;
                    this.opening = true;
                    h.animate({
                        "margin-top": 0
                    }, this.openDelay, function() {
                        o.isanimating = false;
                        o.opening = false;
                        a.data(document.body, "openedComboJQXListBoxParent", o);
                        a.data(document.body, "openedComboJQXListBox" + o.element.id, h)
                    })
                }
            }
            v._renderItems();
            if (!b) {
                this.host.addClass(this.toThemeProperty("jqx-rc-b-expanded"));
                h.addClass(this.toThemeProperty("jqx-rc-t-expanded"));
                this.dropdownlistArrow.addClass(this.toThemeProperty("jqx-rc-b-expanded"))
            } else {
                this.host.addClass(this.toThemeProperty("jqx-rc-t-expanded"));
                h.addClass(this.toThemeProperty("jqx-rc-b-expanded"));
                this.dropdownlistArrow.addClass(this.toThemeProperty("jqx-rc-t-expanded"))
            }
            h.addClass(this.toThemeProperty("jqx-fill-state-focus"));
            this._raiseEvent("0", v)
        },
        doMultiSelect: function(c) {
            if (this.checkboxes) {
                this.multiSelect = false
            }
            var e = this;
            if (!this.multiSelect) {
                var g = e.dropdownlistContent.find(".jqx-button");
                var d = "mousedown";
                if (this.touch) {
                    d = a.jqx.mobile.getTouchEventName("touchstart")
                }
                this.removeHandler(g, d);
                this.removeHandler(g.find(".jqx-icon-close"), d);
                g.remove();
                this.selectedItems = new Array();
                this._selectedItems = new Array();
                return
            }
            if (this.validateSelection) {
                var k = this.validateSelection(this.listBox.selectedValue);
                if (!k) {
                    return
                }
            }
            var h = this.selectedItems;
            if (this.listBox.selectedValue) {
                if (this.selectedItems.indexOf(this.listBox.selectedValue) === -1) {
                    var j = this.getItemByValue(this.listBox.selectedValue);
                    if (j && j.visible) {
                        this.selectedItems.push(this.listBox.selectedValue);
                        this._selectedItems.push(j);
                        this._raiseEvent("2", {
                            index: j.index,
                            item: j
                        });
                        this._raiseEvent("4", {
                            index: j.index,
                            item: j
                        })
                    }
                }
                this.listBox.selectedIndex = 0
            }
            var f = this.listBox.items;
            if (!f) {
                return
            }
            for (var b = 0; b < f.length; b++) {
                f[b].disabled = false;
                if (this.selectedItems.indexOf(f[b].value) >= 0 || this._disabledItems.indexOf(this.value) >= 0) {
                    f[b].disabled = true
                }
            }
            this.listBox._renderItems();
            this.searchString = "";
            this.input.val("");
            var f = "";
            var d = "mousedown";
            var g = e.dropdownlistContent.find(".jqx-button");
            if (this.touch) {
                d = a.jqx.mobile.getTouchEventName("touchstart")
            }
            this.removeHandler(g, d);
            this.removeHandler(g.find(".jqx-icon-close"), d);
            g.remove();
            e.input.detach();
            if (this.selectedItems.length > 0) {
                e.input.css("width", "25px");
                e.input.attr("placeholder", "")
            } else {
                e.input.css("width", "100%");
                e.input.attr("placeholder", this.placeHolder)
            }
            a.each(this.selectedItems, function(i) {
                var m = e.getItemByValue(this);
                if (!m || e.remoteAutoComplete) {
                    m = e._selectedItems[i]
                }
                var o = a('<div style="overflow: hidden; float: left;"></div>');
                o.addClass(e.toThemeProperty("jqx-button"));
                o.addClass(e.toThemeProperty("jqx-combobox-multi-item"));
                o.addClass(e.toThemeProperty("jqx-fill-state-normal"));
                o.addClass(e.toThemeProperty("jqx-rc-all"));
                if (m) {
                    var p = m.label;
                    if (o[0].innerHTML == "") {
                        o[0].innerHTML = '<a data-value="' + m.value + '" style="float: left;" href="#">' + p + "</a>"
                    }
                    if (e.rtl) {
                        o[0].innerHTML = '<a data-value="' + m.value + '" style="float: right;" href="#">' + p + "</a>"
                    }
                    var n = !e.rtl ? "right" : "left";
                    if (e.showCloseButtons) {
                        var l = '<div style="position: relative; overflow: hidden; float: ' + n + '; min-height: 16px; min-width: 18px;"><div style="position: absolute; left: 100%; top: 50%; margin-left: -18px; margin-top: -7px; float: none; width: 16px; height: 16px;" class="' + e.toThemeProperty("jqx-icon-close") + '"></div></div>';
                        if (a.jqx.browser.msie && a.jqx.browser.version < 8) {
                            l = '<div style="position: relative; overflow: hidden; float: left; min-height: 16px; min-width: 18px;"><div style="position: absolute; left: 100%; top: 50%; margin-left: -18px; margin-top: -7px; float: none; width: 16px; height: 16px;" class="' + e.toThemeProperty("jqx-icon-close") + '"></div></div>'
                        }
                        if (e.rtl) {
                            var l = '<div style="position: relative; overflow: hidden; float: ' + n + '; min-height: 16px; min-width: 18px;"><div style="position: absolute; left: 0px; top: 50%; margin-top: -7px; float: none; width: 16px; height: 16px;" class="' + e.toThemeProperty("jqx-icon-close") + '"></div></div>';
                            if (a.jqx.browser.msie && a.jqx.browser.version < 8) {
                                l = '<div style="position: relative; overflow: hidden; float: left; min-height: 16px; min-width: 18px;"><div style="position: absolute; left: 0px; top: 50%; margin-top: -7px; float: none; width: 16px; height: 16px;" class="' + e.toThemeProperty("jqx-icon-close") + '"></div></div>'
                            }
                        }
                        o[0].innerHTML += l
                    }
                } else {
                    if (o[0].innerHTML == "") {
                        o[0].innerHTML = '<a href="#"></a>'
                    }
                }
                e.dropdownlistContent.append(o)
            });
            e.dropdownlistContent.append(e.input);
            e.input.val("");
            if (c !== false) {
                e.input.focus();
                setTimeout(function() {
                    e.input.focus()
                }, 10)
            }
            var g = e.dropdownlistContent.find(".jqx-button");
            if (this.touchMode === true) {
                d = "mousedown"
            }
            this.addHandler(g, d, function(l) {
                if (l.target.className.indexOf("jqx-icon-close") >= 0) {
                    return true
                }
                if (e.disabled) {
                    return true
                }
                var m = a(l.target).attr("data-value");
                var i = e.getItemByValue(m);
                if (i) {
                    e.listBox.selectedValue = null;
                    e.listBox.clearSelection()
                }
                e.listBox.scrollTo(0, 0);
                e.open();
                if (l.preventDefault) {
                    l.preventDefault()
                }
                if (l.stopPropagation) {
                    l.stopPropagation()
                }
                return false
            });
            this.addHandler(g.find(".jqx-icon-close"), d, function(p) {
                if (e.disabled) {
                    return
                }
                var r = a(p.target).parent().parent().find("a").attr("data-value");
                var o = e.getItemByValue(r);
                if (o || (e.remoteAutoComplete && !o && e.selectedItems.indexOf(r) >= 0)) {
                    e.listBox.selectedValue = null;
                    var l = e.selectedItems.indexOf(r);
                    var n = o && o.index >= 0 ? o.index : l;
                    if (l >= 0) {
                        e.selectedItems.splice(l, 1);
                        var q = e._selectedItems[l];
                        if (!q) {
                            q = o
                        }
                        e._selectedItems.splice(l, 1);
                        e._raiseEvent("3", {
                            index: n,
                            type: "mouse",
                            item: q
                        });
                        e._raiseEvent("4", {
                            index: n,
                            type: "mouse",
                            item: q
                        });
                        e.doMultiSelect()
                    } else {
                        for (var m = 0; m < e.selectedItems.length; m++) {
                            var q = e.selectedItems[m];
                            if (q == r) {
                                e.selectedItems.splice(m, 1);
                                e._selectedItems.splice(m, 1);
                                e._raiseEvent("3", {
                                    index: n,
                                    type: "mouse",
                                    item: o
                                });
                                e._raiseEvent("4", {
                                    index: n,
                                    type: "mouse",
                                    item: o
                                });
                                e.doMultiSelect();
                                break
                            }
                        }
                    }
                }
            });
            e.dropdownlistArrow.height(this.host.height());
            e._updateInputSelection()
        },
        hideListBox: function(h) {
            var f = this.listBoxContainer;
            var g = this.listBox;
            var c = this.container;
            if (this.container[0].style.display == "none") {
                return
            }
            a.jqx.aria(this, "aria-expanded", false);
            if (h == "keyboard" || h == "mouse") {
                this.listBox.searchString = ""
            }
            if (h == "keyboard" || h == "mouse" && this.multiSelect) {
                this.doMultiSelect()
            }
            var d = this;
            a.data(document.body, "openedComboJQXListBox" + this.element.id, null);
            if (this.animationType == "none") {
                this.opening = false;
                this.container.css("display", "none")
            } else {
                if (!this.ishiding) {
                    var b = f.outerHeight();
                    f.css("margin-top", 0);
                    f.stop();
                    this.opening = false;
                    this.isanimating = true;
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
                            d.isanimating = false;
                            c.css("display", "none");
                            d.ishiding = false
                        })
                    } else {
                        f.animate({
                            "margin-top": e
                        }, this.closeDelay, function() {
                            d.isanimating = false;
                            c.css("display", "none");
                            d.ishiding = false
                        })
                    }
                }
            }
            this.ishiding = true;
            this.host.removeClass(this.toThemeProperty("jqx-combobox-state-selected"));
            this.dropdownlistArrowIcon.removeClass(this.toThemeProperty("jqx-icon-arrow-down-selected"));
            this.dropdownlistArrow.removeClass(this.toThemeProperty("jqx-combobox-arrow-selected"));
            this.dropdownlistArrow.removeClass(this.toThemeProperty("jqx-fill-state-pressed"));
            if (!this.focused) {
                this.host.removeClass(this.toThemeProperty("jqx-combobox-state-focus"));
                this.host.removeClass(this.toThemeProperty("jqx-fill-state-focus"));
                this.dropdownlistContent.removeClass(this.toThemeProperty("jqx-combobox-content-focus"))
            }
            this.host.removeClass(this.toThemeProperty("jqx-rc-b-expanded"));
            f.removeClass(this.toThemeProperty("jqx-rc-t-expanded"));
            this.host.removeClass(this.toThemeProperty("jqx-rc-t-expanded"));
            f.removeClass(this.toThemeProperty("jqx-rc-b-expanded"));
            f.removeClass(this.toThemeProperty("jqx-fill-state-focus"));
            this.dropdownlistArrow.removeClass(this.toThemeProperty("jqx-rc-t-expanded"));
            this.dropdownlistArrow.removeClass(this.toThemeProperty("jqx-rc-b-expanded"));
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
                return
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
                        if (this.className.indexOf("jqx-combobox") != -1) {
                            if (d.element.id == this.id) {
                                g = true
                            }
                            return false
                        }
                    }
                }
            });
            if (c != null && !g) {
                if (d.isOpened()) {
                    d.hideListBox("api");
                    d.input.blur()
                }
            }
            return true
        },
        loadFromSelect: function(b) {
            this.listBox.loadFromSelect(b)
        },
        refresh: function(b) {
            this._setSize();
            this._arrange();
            if (this.listBox) {
                this.renderSelection()
            }
        },
        resize: function() {
            this._setSize();
            this._arrange()
        },
        _arrange: function() {
            var d = parseInt(this.host.width());
            var j = parseInt(this.host.height());
            var e = this.arrowSize;
            var f = this.arrowSize;
            var h = 1;
            if (!this.showArrow) {
                f = 0;
                e = 0;
                this.dropdownlistArrow.hide();
                h = 0;
                this.host.css("cursor", "arrow")
            }
            var b = d - f - 1 * h;
            if (b > 0) {
                this.dropdownlistContent[0].style.width = b + "px"
            }
            if (this.rtl) {
                this.dropdownlistContent[0].style.width = (-1 + b + "px")
            }
            this.dropdownlistContent[0].style.height = j + "px";
            this.dropdownlistContent[0].style.left = "0px";
            this.dropdownlistContent[0].style.top = "0px";
            this.dropdownlistArrow[0].style.width = f + 1 + "px";
            this.dropdownlistArrow[0].style.height = j + "px";
            this.dropdownlistArrow[0].style.left = 1 + b + "px";
            this.input[0].style.width = "100%";
            if (!this.multiSelect) {
                this.input.height(j)
            }
            var c = this.input.height();
            if (c == 0) {
                c = parseInt(this.input.css("font-size")) + 3
            }
            if (this.input[0].className.indexOf("jqx-rc-all") == -1) {
                this.input.addClass(this.toThemeProperty("jqx-rc-all"))
            }
            var i = parseInt(j) / 2 - parseInt(c) / 2;
            if (i > 0) {}
            if (this.rtl) {
                this.dropdownlistArrow.css("left", "0px");
                this.dropdownlistContent.css("left", this.dropdownlistArrow.width());
                if (a.jqx.browser.msie && a.jqx.browser.version <= 8) {
                    this.dropdownlistContent.css("left", 1 + this.dropdownlistArrow.width())
                }
            }
            if (this.multiSelect) {
                this.input.css("float", "left");
                this.dropdownlistWrapper.parent().css("height", "auto");
                this.dropdownlistContent.css("height", "auto");
                this.dropdownlistWrapper.css("height", "auto");
                this.dropdownlistContent.css("position", "relative");
                this.dropdownlistContent.css("cursor", "text");
                this.host.css("height", "auto");
                this.host.css("min-height", this.height);
                this.dropdownlistContent.css("min-height", this.height);
                var j = parseInt(this.host.height());
                this.dropdownlistArrow.height(j);
                var g = parseInt(this.host.css("min-height"));
                var i = parseInt(g) / 2 - parseInt(c) / 2;
                if (i > 0) {
                    this.input.css("margin-top", i)
                }
            }
        },
        destroy: function() {
            if (this.source && this.source.unbindBindingUpdate) {
                this.source.unbindBindingUpdate(this.element.id);
                this.source.unbindBindingUpdate(this.listBoxContainer[0].id);
                this.source.unbindDownloadComplete(this.element.id);
                this.source.unbindDownloadComplete(this.listBoxContainer[0].id)
            }
            a.jqx.utilities.resize(this.host, null, true);
            this.removeHandler(this.listBoxContainer, "select");
            this.removeHandler(this.listBoxContainer, "unselect");
            this.removeHandler(this.listBoxContainer, "change");
            this.removeHandler(this.listBoxContainer, "bindingComplete");
            this.removeHandler(this.dropdownlistWrapper, "selectstart");
            this.removeHandler(this.dropdownlistWrapper, "mousedown");
            this.removeHandler(this.host, "keydown");
            this.removeHandler(this.listBoxContainer, "select");
            this.removeHandler(this.listBox.content, "click");
            this.removeHandlers();
            this.removeHandler(this.input, "keyup.textchange");
            this.listBoxContainer.jqxListBox("destroy");
            this.listBoxContainer.remove();
            this.host.removeClass();
            this.removeHandler(a(document), "mousedown." + this.id, this.closeOpenedListBox);
            if (this.touch) {
                this.removeHandler(a(document), a.jqx.mobile.getTouchEventName("touchstart") + "." + this.id)
            }
            this.cinput.remove();
            delete this.cinput;
            this.dropdownlistArrow.remove();
            delete this.dropdownlistArrow;
            this.dropdownlistArrowIcon.remove();
            delete this.dropdownlistArrowIcon;
            delete this.dropdownlistWrapper;
            delete this.listBoxContainer;
            delete this.input;
            delete this.dropdownlistContent;
            delete this.comboStructure;
            this.container.remove();
            delete this.listBox;
            delete this.container;
            var b = a.data(this.element, "jqxComboBox");
            if (b) {
                delete b.instance
            }
            this.host.removeData();
            this.host.remove();
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
            if (c == "itemHeight") {
                b.listBoxContainer.jqxListBox({
                    itemHeight: e
                })
            }
            if (c == "renderSelectedItem") {
                b.renderSelection("mouse")
            }
            if (c == "renderer") {
                b.listBoxContainer.jqxListBox({
                    renderer: e
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
            if (c === "touchMode") {
                b.listBoxContainer.jqxListBox({
                    touchMode: e
                });
                b.touch = a.jqx.mobile.isTouchDevice();
                if (b.touchMode === true) {
                    b.touch = true
                }
                b._updateHandlers()
            }
            if (c == "multiSelect") {
                if (e) {
                    b.doMultiSelect(false)
                } else {
                    b.doMultiSelect(false);
                    b.dropdownlistWrapper.parent().css("height", "100%");
                    b.dropdownlistContent.css("height", "100");
                    b.dropdownlistWrapper.css("height", "100");
                    b.dropdownlistContent.css("position", "relative");
                    b.host.css("min-height", null);
                    b._setSize();
                    b._arrange()
                }
            }
            if (c == "showArrow") {
                b._arrange();
                if (b.multiSelect) {
                    b.doMultiSelect(false)
                }
            }
            if (c == "popupZIndex") {
                b.listBoxContainer.css({
                    zIndex: b.popupZIndex
                })
            }
            if (c == "promptText") {
                b.placeHolder = e
            }
            if (c == "autoOpen") {
                b._updateHandlers()
            }
            if (c == "renderer") {
                b.listBox.renderer = b.renderer
            }
            if (c == "itemHeight") {
                b.listBox.itemHeight = e
            }
            if (c == "source") {
                b.input.val("");
                b.listBoxContainer.jqxListBox({
                    source: b.source
                });
                b.renderSelection("mouse");
                if (b.source == null) {
                    b.clearSelection()
                }
                if (b.multiSelect) {
                    b.selectedItems = new Array();
                    b._selectedItems = new Array();
                    b.doMultiSelect(false)
                }
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
            if (c == "displayMember" || c == "valueMember") {
                b.listBoxContainer.jqxListBox({
                    displayMember: b.displayMember,
                    valueMember: b.valueMember
                });
                b.renderSelection("mouse")
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
            if (c == "autoComplete") {
                b._resetautocomplete()
            }
            if (c == "checkboxes") {
                b.listBoxContainer.jqxListBox({
                    checkboxes: b.checkboxes
                });
                if (b.checkboxes) {
                    b.input.attr("readonly", true);
                    a.jqx.aria(b, "aria-readonly", true)
                } else {
                    a.jqx.aria(b, "aria-readonly", false)
                }
            }
            if (c == "theme" && e != null) {
                b.listBoxContainer.jqxListBox({
                    theme: e
                });
                b.listBoxContainer.addClass(b.toThemeProperty("jqx-popup"));
                if (a.jqx.browser.msie) {
                    b.listBoxContainer.addClass(b.toThemeProperty("jqx-noshadow"))
                }
                a.jqx.utilities.setTheme(f, e, b.host)
            }
            if (c == "rtl") {
                b.render();
                b.refresh()
            }
            if (c == "width" || c == "height") {
                b._setSize();
                if (c == "width") {
                    if (b.dropDownWidth == "auto") {
                        var d = b.host.width();
                        b.listBoxContainer.jqxListBox({
                            width: d
                        });
                        b.container.width(parseInt(d) + 25)
                    }
                }
                b._arrange()
            }
            if (c == "selectedIndex") {
                b.listBox.selectIndex(e);
                b.renderSelection("mouse")
            }
        }
    })
})(jqxBaseFramework);