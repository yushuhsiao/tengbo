/*
jQWidgets v3.6.0 (2014-Nov-25)
Copyright (c) 2011-2014 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a) {
    a.jqx.jqxWidget("jqxDragDrop", "", {});
    a.extend(a.jqx._jqxDragDrop.prototype, {
        defineInstance: function() {
            var b = {
                restricter: "document",
                handle: false,
                feedback: "clone",
                opacity: 0.6,
                revert: false,
                revertDuration: 400,
                distance: 5,
                disabled: false,
                tolerance: "intersect",
                data: null,
                dropAction: "default",
                dragZIndex: 999999,
                appendTo: "parent",
                cursor: "move",
                onDragEnd: null,
                onDrag: null,
                onDragStart: null,
                onTargetDrop: null,
                onDropTargetEnter: null,
                onDropTargetLeave: null,
                initFeedback: null,
                dropTarget: null,
                isDestroyed: false,
                triggerEvents: true,
                _touchEvents: {
                    mousedown: a.jqx.mobile.getTouchEventName("touchstart"),
                    click: a.jqx.mobile.getTouchEventName("touchstart"),
                    mouseup: a.jqx.mobile.getTouchEventName("touchend"),
                    mousemove: a.jqx.mobile.getTouchEventName("touchmove"),
                    mouseenter: "mouseenter",
                    mouseleave: "mouseleave"
                },
                _restricter: null,
                _zIndexBackup: 0,
                _targetEnterFired: false,
                _oldOpacity: 1,
                _feedbackType: undefined,
                _isTouchDevice: false,
                _events: ["dragStart", "dragEnd", "dragging", "dropTargetEnter", "dropTargetLeave"]
            };
            a.extend(true, this, b);
            return b
        },
        createInstance: function() {
            this._createDragDrop()
        },
        _createDragDrop: function() {
            var c = a.data(document.body, "jqx-draggables") || 1;
            this.appendTo = this._getParent();
            this._isTouchDevice = a.jqx.mobile.isTouchDevice();
            if ((/(static|relative)/).test(this.host.css("position"))) {
                if (!this.feedback || this.feedback === "original") {
                    var d = this._getRelativeOffset(this.host);
                    var b = this.appendTo.offset();
                    if (this.appendTo.css("position") != "static") {
                        b = {
                            left: 0,
                            top: 0
                        }
                    }
                    this.element.style.position = "absolute";
                    this.element.style.left = b.left + d.left + "px";
                    this.element.style.top = b.top + d.top + "px"
                }
            }
            this._validateProperties();
            this._idHandler(c);
            if (this.disabled) {
                this.disable()
            }
            if (typeof this.dropTarget === "string") {
                this.dropTarget = a(this.dropTarget)
            }
            this._refresh();
            c += 1;
            a.data(document.body, "jqx-draggables", c);
            this.host.addClass("jqx-draggable");
            if (!this.disabled) {
                this.host.css("cursor", this.cursor)
            }
        },
        _getParent: function() {
            var b = this.appendTo;
            if (typeof this.appendTo === "string") {
                switch (this.appendTo) {
                    case "parent":
                        b = this.host.parent();
                        break;
                    case "document":
                        b = a(document);
                        break;
                    case "body":
                        b = a(document.body);
                        break;
                    default:
                        b = a(this.appendTo);
                        break
                }
            }
            return b
        },
        _idHandler: function(b) {
            if (!this.element.id) {
                var c = "jqx-draggable-" + b;
                this.element.id = c
            }
        },
        _refresh: function() {
            this._removeEventHandlers();
            this._addEventHandlers()
        },
        _getEvent: function(b) {
            if (this._isTouchDevice) {
                return this._touchEvents[b]
            } else {
                return b
            }
        },
        _validateProperties: function() {
            if (this.feedback === "clone") {
                this._feedbackType = "clone"
            } else {
                this._feedbackType = "original"
            }
            if (this.dropAction !== "default") {
                this.dropAction = "nothing"
            }
        },
        _removeEventHandlers: function() {
            this.removeHandler(this.host, "dragstart");
            this.removeHandler(this.host, this._getEvent("mousedown") + ".draggable." + this.element.id, this._mouseDown);
            this.removeHandler(a(document), this._getEvent("mousemove") + ".draggable." + this.element.id, this._mouseMove);
            this.removeHandler(a(document), this._getEvent("mouseup") + ".draggable." + this.element.id, this._mouseUp)
        },
        _addEventHandlers: function() {
            var b = this;
            this.addHandler(this.host, "dragstart", function(g) {
                if (b.disabled) {
                    return true
                }
                var f = a.jqx.mobile.isTouchDevice();
                if (!f) {
                    g.preventDefault();
                    return false
                }
            });
            this.addHandler(this.host, this._getEvent("mousedown") + ".draggable." + this.element.id, this._mouseDown, {
                self: this
            });
            this.addHandler(a(document), this._getEvent("mousemove") + ".draggable." + this.element.id, this._mouseMove, {
                self: this
            });
            this.addHandler(a(document), this._getEvent("mouseup") + ".draggable." + this.element.id, this._mouseUp, {
                self: this
            });
            try {
                if (document.referrer != "" || window.frameElement) {
                    if (window.top != null && window.top != window.self) {
                        var e = "";
                        if (window.parent && document.referrer) {
                            e = document.referrer
                        }
                        if (e.indexOf(document.location.host) != -1) {
                            var d = function(f) {
                                b._mouseUp(b)
                            };
                            if (window.top.document.addEventListener) {
                                window.top.document.addEventListener("mouseup", d, false)
                            } else {
                                if (window.top.document.attachEvent) {
                                    window.top.document.attachEvent("onmouseup", d)
                                }
                            }
                        }
                    }
                }
            } catch (c) {}
        },
        _mouseDown: function(f) {
            var b = f.data.self,
                d = b._getMouseCoordinates(f),
                c = b._mouseCapture(f);
            b._originalPageX = d.left;
            b._originalPageY = d.top;
            if (b.disabled) {
                return true
            }
            var e = false;
            if (!b._mouseStarted) {
                b._mouseUp(f);
                e = true
            }
            if (c) {
                b._mouseDownEvent = f
            }
            if (b._isTouchDevice) {
                return true
            }
            if (f.which !== 1 || !c) {
                return true
            }
            f.preventDefault();
            if (e == true) {}
        },
        _mouseMove: function(c) {
            var b = c.data.self;
            if (b.disabled) {
                return true
            }
            if (b._mouseStarted) {
                b._mouseDrag(c);
                if (c.preventDefault) {
                    c.preventDefault()
                }
                return false
            }
            if (b._mouseDownEvent && b._isMovedDistance(c)) {
                if (b._mouseStart(b._mouseDownEvent, c)) {
                    b._mouseStarted = true
                } else {
                    b._mouseStarted = false
                }
                if (b._mouseStarted) {
                    b._mouseDrag(c)
                } else {
                    b._mouseUp(c)
                }
            }
            return !b._mouseStarted
        },
        _mouseUp: function(c) {
            var b;
            if (c.data && c.data.self) {
                b = c.data.self
            } else {
                b = this
            }
            if (b.disabled) {
                return true
            }
            b._mouseDownEvent = false;
            b._movedDistance = false;
            if (b._mouseStarted) {
                b._mouseStarted = false;
                b._mouseStop(c)
            }
            if (b.feedback && b.feedback[0] && b._feedbackType !== "original" && typeof b.feedback.remove === "function" && !b.revert) {
                b.feedback.remove()
            }
            if (!b._isTouchDevice) {
                return false
            }
        },
        cancelDrag: function() {
            var b = this.revertDuration;
            this.revertDuration = 0;
            this._mouseDownEvent = false;
            this._movedDistance = false;
            this._mouseStarted = false;
            this._mouseStop();
            this.feedback.remove();
            this.revertDuration = b
        },
        _isMovedDistance: function(b) {
            var c = this._getMouseCoordinates(b);
            if (this._movedDistance) {
                return true
            }
            if (c.left >= this._originalPageX + this.distance || c.left <= this._originalPageX - this.distance || c.top >= this._originalPageY + this.distance || c.top <= this._originalPageY - this.distance) {
                this._movedDistance = true;
                return true
            }
            return false
        },
        _getMouseCoordinates: function(b) {
            if (this._isTouchDevice) {
                var c = a.jqx.position(b);
                return {
                    left: c.left,
                    top: c.top
                }
            } else {
                return {
                    left: b.pageX,
                    top: b.pageY
                }
            }
        },
        destroy: function() {
            this._enableSelection(this.host);
            this.host.removeData("draggable").off(".draggable").removeClass("jqx-draggable jqx-draggable-dragging jqx-draggable-disabled");
            this._removeEventHandlers();
            this.isDestroyed = true;
            return this
        },
        _disableSelection: function(b) {
            b.each(function() {
                a(this).attr("unselectable", "on").css({
                    "-ms-user-select": "none",
                    "-moz-user-select": "none",
                    "-webkit-user-select": "none",
                    "user-select": "none"
                }).each(function() {
                    this.onselectstart = function() {
                        return false
                    }
                })
            })
        },
        _enableSelection: function(b) {
            b.each(function() {
                a(this).attr("unselectable", "off").css({
                    "-ms-user-select": "text",
                    "-moz-user-select": "text",
                    "-webkit-user-select": "text",
                    "user-select": "text"
                }).each(function() {
                    this.onselectstart = null
                })
            })
        },
        _mouseCapture: function(b) {
            if (this.disabled) {
                return false
            }
            if (!this._getHandle(b)) {
                return false
            }
            this._disableSelection(this.host);
            return true
        },
        _getScrollParent: function(b) {
            var c;
            if ((a.jqx.browser.msie && (/(static|relative)/).test(b.css("position"))) || (/absolute/).test(b.css("position"))) {
                c = b.parents().filter(function() {
                    return (/(relative|absolute|fixed)/).test(a.css(this, "position", 1)) && (/(auto|scroll)/).test(a.css(this, "overflow", 1) + a.css(this, "overflow-y", 1) + a.css(this, "overflow-x", 1))
                }).eq(0)
            } else {
                c = b.parents().filter(function() {
                    return (/(auto|scroll)/).test(a.css(this, "overflow", 1) + a.css(this, "overflow-y", 1) + a.css(this, "overflow-x", 1))
                }).eq(0)
            }
            return (/fixed/).test(b.css("position")) || !c.length ? a(document) : c
        },
        _mouseStart: function(e) {
            var d = this._getMouseCoordinates(e),
                c = this._getParentOffset(this.host);
            this.feedback = this._createFeedback(e);
            this._zIndexBackup = this.feedback.css("z-index");
            this.feedback[0].style.zIndex = this.dragZIndex;
            this._backupFeedbackProportions();
            this._backupeMargins();
            this._positionType = this.feedback.css("position");
            this._scrollParent = this._getScrollParent(this.feedback);
            this._offset = this.positionAbs = this.host.offset();
            this._offset = {
                top: this._offset.top - this.margins.top,
                left: this._offset.left - this.margins.left
            };
            a.extend(this._offset, {
                click: {
                    left: d.left - this._offset.left,
                    top: d.top - this._offset.top
                },
                parent: this._getParentOffset(),
                relative: this._getRelativeOffset(),
                hostRelative: this._getRelativeOffset(this.host)
            });
            this.position = this._generatePosition(e);
            this.originalPosition = this._fixPosition();
            if (this.restricter) {
                this._setRestricter()
            }
            this.feedback.addClass(this.toThemeProperty("jqx-draggable-dragging"));
            var b = this._raiseEvent(0, e);
            if (this.onDragStart && typeof this.onDragStart === "function") {
                this.onDragStart(this.position)
            }
            this._mouseDrag(e, true);
            return true
        },
        _fixPosition: function() {
            var c = this._getRelativeOffset(this.host),
                b = this.position;
            b = {
                left: this.position.left + c.left,
                top: this.position.top + c.top
            };
            return b
        },
        _mouseDrag: function(b, c) {
            this.position = this._generatePosition(b);
            this.positionAbs = this._convertPositionTo("absolute");
            this.feedback[0].style.left = this.position.left + "px";
            this.feedback[0].style.top = this.position.top + "px";
            this._raiseEvent(2, b);
            if (this.onDrag && typeof this.onDrag === "function") {
                this.onDrag(this.data, this.position)
            }
            this._handleTarget();
            return false
        },
        _over: function(b, d, e) {
            if (this.dropTarget) {
                var f = false,
                    c = this;
                a.each(this.dropTarget, function(g, h) {
                    f = c._overItem(h, b, d, e);
                    if (f.over) {
                        return false
                    }
                })
            }
            return f
        },
        _overItem: function(i, c, e, g) {
            i = a(i);
            var b = i.offset(),
                f = i.outerHeight(),
                d = i.outerWidth(),
                h;
            if (!i || i[0] === this.element) {
                return
            }
            var h = false;
            switch (this.tolerance) {
                case "intersect":
                    if (c.left + e > b.left && c.left < b.left + d && c.top + g > b.top && c.top < b.top + f) {
                        h = true
                    }
                    break;
                case "fit":
                    if (e + c.left <= b.left + d && c.left >= b.left && g + c.top <= b.top + f && c.top >= b.top) {
                        h = true
                    }
                    break
            }
            return {
                over: h,
                target: i
            }
        },
        _handleTarget: function() {
            if (this.dropTarget) {
                var b = this.feedback.offset(),
                    c = this.feedback.outerWidth(),
                    d = this.feedback.outerHeight(),
                    e = this._over(b, c, d);
                if (e.over) {
                    if (this._targetEnterFired && e.target.length > 0 && this._oldtarget && this._oldtarget.length > 0 && e.target[0] != this._oldtarget[0]) {
                        this._raiseEvent(4, {
                            target: this._oldtarget
                        });
                        if (this.onDropTargetLeave && typeof this.onDropTargetLeave === "function") {
                            this.onDropTargetLeave(this._oldtarget)
                        }
                    }
                    if (!this._targetEnterFired || (e.target.length > 0 && this._oldtarget && this._oldtarget.length > 0 && e.target[0] != this._oldtarget[0])) {
                        this._targetEnterFired = true;
                        this._raiseEvent(3, {
                            target: e.target
                        });
                        if (this.onDropTargetEnter && typeof this.onDropTargetEnter === "function") {
                            this.onDropTargetEnter(e.target)
                        }
                    }
                    this._oldtarget = e.target
                } else {
                    if (this._targetEnterFired) {
                        this._targetEnterFired = false;
                        this._raiseEvent(4, {
                            target: this._oldtarget || e.target
                        });
                        if (this.onDropTargetLeave && typeof this.onDropTargetLeave === "function") {
                            this.onDropTargetLeave(this._oldtarget || e.target)
                        }
                    }
                }
            }
        },
        _mouseStop: function(d) {
            var e = false,
                b = this._fixPosition(),
                c = {
                    width: this.host.outerWidth(),
                    height: this.host.outerHeight()
                };
            this.feedback[0].style.opacity = this._oldOpacity;
            if (!this.revert) {
                this.feedback[0].style.zIndex = this._zIndexBackup
            }
            this._enableSelection(this.host);
            if (this.dropped) {
                e = this.dropped;
                this.dropped = false
            }
            if ((!this.element || !this.element.parentNode) && this.feedback === "original") {
                return false
            }
            this._dropElement(b);
            this.feedback.removeClass(this.toThemeProperty("jqx-draggable-dragging"));
            this._raiseEvent(1, d);
            if (this.onDragEnd && typeof this.onDragEnd === "function") {
                this.onDragEnd(this.data)
            }
            if (this.onTargetDrop && typeof this.onTargetDrop === "function" && this._over(b, c.width, c.height).over) {
                this.onTargetDrop(this._over(b, c.width, c.height).target)
            }
            this._revertHandler();
            return false
        },
        _dropElement: function(b) {
            if (this.dropAction === "default" && this.feedback && this.feedback[0] !== this.element && this.feedback !== "original") {
                if (!this.revert) {
                    if (!(/(fixed|absolute)/).test(this.host.css("position"))) {
                        this.host.css("position", "relative");
                        var c = this._getRelativeOffset(this.host);
                        b = this.position;
                        b.left -= c.left;
                        b.top -= c.top;
                        this.element.style.left = b.left + "px";
                        this.element.style.top = b.top + "px"
                    }
                }
            }
        },
        _revertHandler: function() {
            if (this.revert || (a.isFunction(this.revert) && this.revert())) {
                var b = this;
                if (this._feedbackType != "original") {
                    if (this.feedback != null) {
                        if (this.dropAction != "none") {
                            a(this.feedback).animate({
                                left: b.originalPosition.left - b._offset.hostRelative.left,
                                top: b.originalPosition.top - b._offset.hostRelative.top
                            }, parseInt(this.revertDuration, 10), function() {
                                if (b.feedback && b.feedback[0] && b._feedbackType !== "original" && typeof b.feedback.remove === "function") {
                                    b.feedback.remove()
                                }
                            })
                        } else {
                            if (b.feedback && b.feedback[0] && b._feedbackType !== "original" && typeof b.feedback.remove === "function") {
                                b.feedback.remove()
                            }
                        }
                    }
                } else {
                    this.element.style.zIndex = this.dragZIndex;
                    a(this.host).animate({
                        left: b.originalPosition.left - b._offset.hostRelative.left,
                        top: b.originalPosition.top - b._offset.hostRelative.top
                    }, parseInt(this.revertDuration, 10), function() {
                        b.element.style.zIndex = b._zIndexBackup
                    })
                }
            }
        },
        _getHandle: function(b) {
            var c;
            if (!this.handle) {
                c = true
            } else {
                a(this.handle, this.host).find("*").andSelf().each(function() {
                    if (this == b.target) {
                        c = true
                    }
                })
            }
            return c
        },
        _createFeedback: function(c) {
            var b;
            if (typeof this._feedbackType === "function") {
                b = this._feedbackType()
            } else {
                if (this._feedbackType === "clone") {
                    b = this.host.clone().removeAttr("id")
                } else {
                    b = this.host
                }
            }
            if (!(/(absolute|fixed)/).test(b.css("position"))) {
                b.css("position", "absolute")
            }
            if (this.appendTo[0] !== this.host.parent()[0] || b[0] !== this.element) {
                var d = {};
                b.css({
                    left: this.host.offset().left - this._getParentOffset(this.host).left + this._getParentOffset(b).left,
                    top: this.host.offset().top - this._getParentOffset(this.host).top + this._getParentOffset(b).top
                });
                b.appendTo(this.appendTo)
            }
            if (typeof this.initFeedback === "function") {
                this.initFeedback(b)
            }
            return b
        },
        _getParentOffset: function(c) {
            var c = c || this.feedback;
            this._offsetParent = c.offsetParent();
            var b = this._offsetParent.offset();
            if (this._positionType == "absolute" && this._scrollParent[0] !== document && a.contains(this._scrollParent[0], this._offsetParent[0])) {
                b.left += this._scrollParent.scrollLeft();
                b.top += this._scrollParent.scrollTop()
            }
            if ((this._offsetParent[0] == document.body) || (this._offsetParent[0].tagName && this._offsetParent[0].tagName.toLowerCase() == "html" && a.jqx.browser.msie)) {
                b = {
                    top: 0,
                    left: 0
                }
            }
            return {
                top: b.top + (parseInt(this._offsetParent.css("border-top-width"), 10) || 0),
                left: b.left + (parseInt(this._offsetParent.css("border-left-width"), 10) || 0)
            }
        },
        _getRelativeOffset: function(c) {
            var d = this._scrollParent || c.parent();
            c = c || this.feedback;
            if (c.css("position") === "relative") {
                var b = this.host.position();
                return {
                    top: b.top - (parseInt(c.css("top"), 10) || 0),
                    left: b.left - (parseInt(c.css("left"), 10) || 0)
                }
            } else {
                return {
                    top: 0,
                    left: 0
                }
            }
        },
        _backupeMargins: function() {
            this.margins = {
                left: (parseInt(this.host.css("margin-left"), 10) || 0),
                top: (parseInt(this.host.css("margin-top"), 10) || 0),
                right: (parseInt(this.host.css("margin-right"), 10) || 0),
                bottom: (parseInt(this.host.css("margin-bottom"), 10) || 0)
            }
        },
        _backupFeedbackProportions: function() {
            this.feedback[0].style.opacity = this.opacity;
            this._feedbackProportions = {
                width: this.feedback.outerWidth(),
                height: this.feedback.outerHeight()
            }
        },
        _setRestricter: function() {
            if (this.restricter == "parent") {
                this.restricter = this.feedback[0].parentNode
            }
            if (this.restricter == "document" || this.restricter == "window") {
                this._handleNativeRestricter()
            }
            if (typeof this.restricter.left !== "undefined" && typeof this.restricter.top !== "undefined" && typeof this.restricter.height !== "undefined" && typeof this.restricter.width !== "undefined") {
                this._restricter = [this.restricter.left, this.restricter.top, this.restricter.width, this.restricter.height]
            } else {
                if (!(/^(document|window|parent)$/).test(this.restricter) && this.restricter.constructor != Array) {
                    this._handleDOMParentRestricter()
                } else {
                    if (this.restricter.constructor == Array) {
                        this._restricter = this.restricter
                    }
                }
            }
        },
        _handleNativeRestricter: function() {
            this._restricter = [this.restricter === "document" ? 0 : a(window).scrollLeft() - this._offset.relative.left - this._offset.parent.left, this.restricter === "document" ? 0 : a(window).scrollTop() - this._offset.relative.top - this._offset.parent.top, (this.restricter === "document" ? 0 : a(window).scrollLeft()) + a(this.restricter === "document" ? document : window).width() - this._feedbackProportions.width - this.margins.left, (this.restricter === "document" ? 0 : a(window).scrollTop()) + (a(this.restricter === "document" ? document : window).height() || document.body.parentNode.scrollHeight) - this._feedbackProportions.height - this.margins.top]
        },
        _handleDOMParentRestricter: function() {
            var d = a(this.restricter),
                b = d[0];
            if (!b) {
                return
            }
            var c = (a(b).css("overflow") !== "hidden");
            this._restricter = [(parseInt(a(b).css("borderLeftWidth"), 10) || 0) + (parseInt(a(b).css("paddingLeft"), 10) || 0), (parseInt(a(b).css("borderTopWidth"), 10) || 0) + (parseInt(a(b).css("paddingTop"), 10) || 0), (c ? Math.max(b.scrollWidth, b.offsetWidth) : b.offsetWidth) - (parseInt(a(b).css("borderLeftWidth"), 10) || 0) - (parseInt(a(b).css("paddingRight"), 10) || 0) - this._feedbackProportions.width - this.margins.left - this.margins.right, (c ? Math.max(b.scrollHeight, b.offsetHeight) : b.offsetHeight) - (parseInt(a(b).css("borderTopWidth"), 10) || 0) - (parseInt(a(b).css("paddingBottom"), 10) || 0) - this._feedbackProportions.height - this.margins.top - this.margins.bottom];
            this._restrictiveContainer = d
        },
        _convertPositionTo: function(f, c) {
            if (!c) {
                c = this.position
            }
            var e, b, g;
            if (f === "absolute") {
                e = 1
            } else {
                e = -1
            }
            if (this._positionType === "absolute" && !(this._scrollParent[0] != document && a.contains(this._scrollParent[0], this._offsetParent[0]))) {
                b = this._offsetParent
            } else {
                b = this._scrollParent
            }
            g = (/(html|body)/i).test(b[0].tagName);
            return this._getPosition(c, e, g, b)
        },
        _getPosition: function(c, d, e, b) {
            return {
                top: (c.top + this._offset.relative.top * d + this._offset.parent.top * d - (a.jqx.browser.safari && a.jqx.browser.version < 526 && this._positionType == "fixed" ? 0 : (this._positionType == "fixed" ? -this._scrollParent.scrollTop() : (e ? 0 : b.scrollTop())) * d)),
                left: (c.left + this._offset.relative.left * d + this._offset.parent.left * d - (a.jqx.browser.safari && a.jqx.browser.version < 526 && this._positionType == "fixed" ? 0 : (this._positionType == "fixed" ? -this._scrollParent.scrollLeft() : e ? 0 : b.scrollLeft()) * d))
            }
        },
        _generatePosition: function(f) {
            var b = this._positionType == "absolute" && !(this._scrollParent[0] != document && a.contains(this._scrollParent[0], this._offsetParent[0])) ? this._offsetParent : this._scrollParent,
                i = (/(html|body)/i).test(b[0].tagName);
            var e = this._getMouseCoordinates(f),
                d = e.left,
                c = e.top;
            if (this.originalPosition) {
                var h;
                if (this.restricter) {
                    if (this._restrictiveContainer) {
                        var g = this._restrictiveContainer.offset();
                        h = [this._restricter[0] + g.left, this._restricter[1] + g.top, this._restricter[2] + g.left, this._restricter[3] + g.top]
                    } else {
                        h = this._restricter
                    }
                    if (e.left - this._offset.click.left < h[0]) {
                        d = h[0] + this._offset.click.left
                    }
                    if (e.top - this._offset.click.top < h[1]) {
                        c = h[1] + this._offset.click.top
                    }
                    if (e.left - this._offset.click.left > h[2]) {
                        d = h[2] + this._offset.click.left
                    }
                    if (e.top - this._offset.click.top > h[3]) {
                        c = h[3] + this._offset.click.top
                    }
                }
            }
            return {
                top: (c - this._offset.click.top - this._offset.relative.top - this._offset.parent.top + (a.jqx.browser.safari && a.jqx.browser.version < 526 && this._positionType == "fixed" ? 0 : (this._positionType == "fixed" ? -this._scrollParent.scrollTop() : (i ? 0 : b.scrollTop())))),
                left: (d - this._offset.click.left - this._offset.relative.left - this._offset.parent.left + (a.jqx.browser.safari && a.jqx.browser.version < 526 && this._positionType == "fixed" ? 0 : (this._positionType == "fixed" ? -this._scrollParent.scrollLeft() : i ? 0 : b.scrollLeft())))
            }
        },
        _raiseEvent: function(c, e) {
            if (this.triggerEvents != undefined && this.triggerEvents == false) {
                return
            }
            var b = this._events[c],
                d = a.Event(b),
                e = e || {};
            e.position = this.position;
            e.element = this.element;
            a.extend(e, this.data);
            e.feedback = this.feedback;
            d.args = e;
            return this.host.trigger(d)
        },
        disable: function() {
            this.disabled = true;
            this.host.addClass(this.toThemeProperty("jqx-draggable-disabled"));
            this._enableSelection(this.host)
        },
        enable: function() {
            this.disabled = false;
            this.host.removeClass(this.toThemeProperty("jqx-draggable-disabled"))
        },
        propertyChangedHandler: function(b, c, e, d) {
            if (c === "dropTarget") {
                if (typeof d === "string") {
                    b.dropTarget = a(d)
                }
            } else {
                if (c == "disabled") {
                    if (d) {
                        b._enableSelection(b.host)
                    }
                } else {
                    if (c == "cursor") {
                        b.host.css("cursor", b.cursor)
                    }
                }
            }
        }
    })
})(jqxBaseFramework);
(function(a) {
    jqxListBoxDragDrop = function() {
        a.extend(a.jqx._jqxListBox.prototype, {
            _hitTestBounds: function(b, c, e) {
                var f = b.host.offset();
                var g = e - parseInt(f.top);
                var i = c - parseInt(f.left);
                var k = b._hitTest(i, g);
                if (g < 0) {
                    return null
                }
                if (k != null) {
                    var d = parseInt(f.left);
                    var j = d + b.host.width();
                    if (d <= c + k.width / 2 && c <= j) {
                        return k
                    }
                    return null
                }
                if (b.items && b.items.length > 0) {
                    var h = b.items[b.items.length - 1];
                    if (b.groups.length < 2) {
                        if (h.top + h.height + 15 >= g) {
                            return h
                        }
                    }
                }
                return null
            },
            _handleDragStart: function(d, c) {
                var b = a.jqx.mobile.isTouchDevice();
                if (b) {
                    if (c.allowDrag) {
                        d.on(a.jqx.mobile.getTouchEventName("touchstart"), function() {
                            a.jqx.mobile.setTouchScroll(false, c.element.id)
                        })
                    }
                }
                d.off("dragStart");
                d.on("dragStart", function(h) {
                    if (c.allowDrag && !c.disabled) {
                        c.feedbackElement = a("<div style='z-index: 99999; position: absolute;'></div>");
                        c.feedbackElement.addClass(c.toThemeProperty("jqx-listbox-feedback"));
                        c.feedbackElement.appendTo(a(document.body));
                        c.feedbackElement.hide();
                        c.isDragging = true;
                        c._dragCancel = false;
                        var j = c._getMouseCoordinates(h);
                        var g = c._hitTestBounds(c, j.left, j.top);
                        var i = a.find(".jqx-listbox");
                        c._listBoxes = i;
                        a.each(c._listBoxes, function() {
                            var k = a.data(this, "jqxListBox").instance;
                            k._enableHover = k.enableHover;
                            k.enableHover = false;
                            a.jqx.mobile.setTouchScroll(false, c.element.id)
                        });
                        var f = function() {
                            c._dragCancel = true;
                            a(h.args.element).jqxDragDrop({
                                triggerEvents: false
                            });
                            a(h.args.element).jqxDragDrop("cancelDrag");
                            clearInterval(c._autoScrollTimer);
                            a(h.args.element).jqxDragDrop({
                                triggerEvents: true
                            });
                            a.each(c._listBoxes, function() {
                                var k = a.data(this, "jqxListBox").instance;
                                if (k._enableHover != undefined) {
                                    k.enableHover = k._enableHover;
                                    a.jqx.mobile.setTouchScroll(true, c.element.id)
                                }
                            })
                        };
                        if (g != null && !g.isGroup) {
                            c._dragItem = g;
                            if (c.dragStart) {
                                var e = c.dragStart(g);
                                if (e == false) {
                                    f();
                                    return false
                                }
                            }
                            if (g.disabled) {
                                f()
                            }
                            c._raiseEvent(4, {
                                label: g.label,
                                value: g.value,
                                originalEvent: h.args
                            })
                        } else {
                            if (g == null) {
                                f()
                            }
                        }
                    }
                    return false
                })
            },
            _handleDragging: function(c, b) {
                c.off("dragging");
                c.on("dragging", function(f) {
                    var e = f.args;
                    if (b._dragCancel) {
                        return
                    }
                    var g = b._getMouseCoordinates(f);
                    var d = g;
                    b._lastDraggingPosition = g;
                    b._dragOverItem = null;
                    b.feedbackElement.hide();
                    a.each(b._listBoxes, function() {
                        if (a.jqx.isHidden(a(this))) {
                            return true
                        }
                        var l = a(this).offset();
                        var n = l.top + 20;
                        var h = a(this).height() + n - 40;
                        var j = l.left;
                        var i = a(this).width();
                        var o = j + i;
                        var m = a.data(this, "jqxListBox").instance;
                        var p = m._hitTestBounds(m, g.left, g.top);
                        var k = m.vScrollInstance;
                        if (p != null) {
                            if (m.allowDrop && !m.disabled) {
                                b._dragOverItem = p;
                                if (p.element) {
                                    b.feedbackElement.show();
                                    var q = a(p.element).offset().top + 1;
                                    if (d.top > q + p.height / 2) {
                                        q = q + p.height
                                    }
                                    b.feedbackElement.css("top", q);
                                    b.feedbackElement.css("left", j);
                                    if (m.vScrollBar.css("visibility") != "visible") {
                                        b.feedbackElement.width(a(this).width())
                                    } else {
                                        b.feedbackElement.width(a(this).width() - 20)
                                    }
                                }
                            }
                        }
                        if (g.left >= j && g.left < o) {
                            if (e.position.top < n && e.position.top >= n - 30) {
                                clearInterval(m._autoScrollTimer);
                                if (k.value != 0) {
                                    b.feedbackElement.hide()
                                }
                                m._autoScrollTimer = setInterval(function() {
                                    var r = m.scrollUp();
                                    if (!r) {
                                        clearInterval(m._autoScrollTimer)
                                    }
                                }, 100)
                            } else {
                                if (e.position.top > h && e.position.top < h + 30) {
                                    clearInterval(m._autoScrollTimer);
                                    if ((m.vScrollBar.css("visibility") != "hidden") && k.value != k.max) {
                                        b.feedbackElement.hide()
                                    }
                                    m._autoScrollTimer = setInterval(function() {
                                        var r = m.scrollDown();
                                        if (!r) {
                                            clearInterval(m._autoScrollTimer)
                                        }
                                    }, 100)
                                } else {
                                    clearInterval(m._autoScrollTimer)
                                }
                            }
                        } else {
                            if (b._dragOverItem == null) {
                                b.feedbackElement.hide()
                            }
                            clearInterval(m._autoScrollTimer)
                        }
                    })
                })
            },
            _handleDragEnd: function(c, b) {
                var d = a.find(".jqx-listbox");
                c.off("dragEnd");
                c.on("dragEnd", function(f) {
                    clearInterval(b._autoScrollTimer);
                    var o = a.jqx.mobile.isTouchDevice();
                    var k = o ? b._lastDraggingPosition : b._getMouseCoordinates(f);
                    var g = a.find(".jqx-listbox");
                    var h = null;
                    b.feedbackElement.remove();
                    if (b._dragCancel) {
                        f.stopPropagation();
                        return
                    }
                    a.each(g, function() {
                        if (a.jqx.isHidden(a(this))) {
                            return true
                        }
                        var w = parseInt(a(this).offset().left);
                        var u = w + a(this).width();
                        var x = a.data(this, "jqxListBox").instance;
                        clearInterval(x._autoScrollTimer);
                        if (x._enableHover != undefined) {
                            x.enableHover = x._enableHover;
                            a.jqx.mobile.setTouchScroll(true, b.element.id)
                        }
                        if (b._dragItem != null) {
                            if (k.left + b._dragItem.width / 2 >= w && k.left < u) {
                                var v = parseInt(a(this).offset().top);
                                var t = v + a(this).height();
                                if (k.top >= v && k.top <= t) {
                                    h = a(this)
                                }
                            }
                        }
                    });
                    var s = b._dragItem;
                    if (h != null && h.length > 0) {
                        var n = a.data(h[0], "jqxListBox").instance;
                        var l = n.allowDrop;
                        if (l && !n.disabled) {
                            var n = a.data(h[0], "jqxListBox").instance;
                            var p = n._hitTestBounds(n, k.left, k.top);
                            p = b._dragOverItem;
                            if (p != null && !p.isGroup) {
                                var r = true;
                                if (b.dragEnd) {
                                    r = b.dragEnd(s, p, f.args);
                                    if (r == false) {
                                        a(f.args.element).jqxDragDrop({
                                            triggerEvents: false
                                        });
                                        a(f.args.element).jqxDragDrop("cancelDrag");
                                        clearInterval(b._autoScrollTimer);
                                        a(f.args.element).jqxDragDrop({
                                            triggerEvents: true
                                        });
                                        if (f.preventDefault) {
                                            f.preventDefault()
                                        }
                                        if (f.stopPropagation) {
                                            f.stopPropagation()
                                        }
                                        return false
                                    }
                                    if (r == undefined) {
                                        r = true
                                    }
                                }
                                if (r) {
                                    var e = p.visibleIndex;
                                    var j = function() {
                                        var u = p.visibleIndex;
                                        for (var t = u - 2; t <= u + 2; t++) {
                                            if (n.items && n.items.length > t) {
                                                var v = n.items[t];
                                                if (v != null) {
                                                    if (v.value == s.value) {
                                                        return v.visibleIndex
                                                    }
                                                }
                                            }
                                        }
                                        return u
                                    };
                                    if (n.dropAction != "none") {
                                        if (p.element) {
                                            var q = a(p.element).offset().top + 1
                                        } else {
                                            var q = a(n.element).offset().top + 1
                                        }
                                        if (n.content.find(".draggable").length > 0) {
                                            n.content.find(".draggable").jqxDragDrop("destroy")
                                        }
                                        if (k.top > q + p.height / 2) {
                                            n.insertAt(b._dragItem, p.index + 1)
                                        } else {
                                            n.insertAt(b._dragItem, p.index)
                                        }
                                        if (b.dropAction == "default") {
                                            if (s.visibleIndex > 0) {
                                                b.selectIndex(s.visibleIndex - 1)
                                            }
                                            b.removeItem(s, true)
                                        }
                                        var m = j();
                                        n.clearSelection();
                                        n.selectIndex(m)
                                    }
                                }
                            } else {
                                if (n.dropAction != "none") {
                                    if (n.content.find(".draggable").length > 0) {
                                        n.content.find(".draggable").jqxDragDrop("destroy")
                                    }
                                    if (b.dragEnd) {
                                        var r = b.dragEnd(b._dragItem, null, f.args);
                                        if (r == false) {
                                            a(f.args.element).jqxDragDrop({
                                                triggerEvents: false
                                            });
                                            a(f.args.element).jqxDragDrop("cancelDrag");
                                            clearInterval(b._autoScrollTimer);
                                            a(f.args.element).jqxDragDrop({
                                                triggerEvents: true
                                            });
                                            if (f.preventDefault) {
                                                f.preventDefault()
                                            }
                                            if (f.stopPropagation) {
                                                f.stopPropagation()
                                            }
                                            return false
                                        }
                                        if (r == undefined) {
                                            r = true
                                        }
                                    }
                                    n.addItem(b._dragItem);
                                    if (n.dropAction == "default") {
                                        if (s.visibleIndex > 0) {
                                            b.selectIndex(s.visibleIndex - 1)
                                        }
                                        b.removeItem(s, true)
                                    }
                                    n.clearSelection();
                                    n.selectIndex(n.items.length - 1)
                                }
                            }
                        }
                    } else {
                        if (b.dragEnd) {
                            var i = b.dragEnd(s, f.args);
                            if (false == i) {
                                if (f.preventDefault) {
                                    f.preventDefault()
                                }
                                if (f.stopPropagation) {
                                    f.stopPropagation()
                                }
                                return false
                            }
                        }
                    }
                    if (s != null) {
                        b._raiseEvent(5, {
                            label: s.label,
                            value: s.value,
                            originalEvent: f.args
                        })
                    }
                    return false
                })
            },
            _enableDragDrop: function() {
                if (this.allowDrag && this.host.jqxDragDrop) {
                    var c = this.content.find(".draggable");
                    if (c.length > 0) {
                        var b = this;
                        c.jqxDragDrop({
                            cursor: "arrow",
                            revertDuration: 0,
                            appendTo: "body",
                            dragZIndex: 99999,
                            revert: true,
                            initFeedback: function(d) {
                                var f = a('<span style="white-space: nowrap;" class="' + b.toThemeProperty("jqx-fill-state-normal") + '">' + d.text() + "</span>");
                                a(document.body).append(f);
                                var e = f.width();
                                f.remove();
                                d.width(e + 5);
                                d.addClass(b.toThemeProperty("jqx-fill-state-pressed"))
                            }
                        });
                        this._autoScrollTimer = null;
                        b._dragItem = null;
                        b._handleDragStart(c, b);
                        b._handleDragging(c, b);
                        b._handleDragEnd(c, b)
                    }
                }
            },
            _getMouseCoordinates: function(b) {
                this._isTouchDevice = a.jqx.mobile.isTouchDevice();
                if (this._isTouchDevice) {
                    var c = a.jqx.position(b.args);
                    return {
                        left: c.left,
                        top: c.top
                    }
                } else {
                    return {
                        left: b.args.pageX,
                        top: b.args.pageY
                    }
                }
            }
        })
    };
    jqxTreeDragDrop = function() {
        a.extend(a.jqx._jqxTree.prototype, {
            _hitTestBounds: function(b, g, f) {
                var d = this;
                var e = null;
                if (b._visibleItems) {
                    var c = parseInt(b.host.offset().left);
                    var h = b.host.outerWidth();
                    a.each(b._visibleItems, function(j) {
                        if (g >= c && g < c + h) {
                            if (this.top + 5 < f && f < this.top + this.height) {
                                var i = a(this.element).parents("li:first");
                                if (i.length > 0) {
                                    e = b.getItem(i[0]);
                                    if (e != null) {
                                        e.height = this.height;
                                        e.top = this.top;
                                        return false
                                    }
                                }
                            }
                        }
                    })
                }
                return e
            },
            _handleDragStart: function(d, c) {
                if (c._dragOverItem) {
                    c._dragOverItem.titleElement.removeClass(c.toThemeProperty("jqx-fill-state-hover"))
                }
                var b = a.jqx.mobile.isTouchDevice();
                if (b) {
                    if (c.allowDrag) {
                        d.on(a.jqx.mobile.getTouchEventName("touchstart"), function() {
                            a.jqx.mobile.setTouchScroll(false, "panel" + c.element.id)
                        })
                    }
                }
                d.off("dragStart");
                d.on("dragStart", function(g) {
                    c.feedbackElement = a("<div style='z-index: 99999; position: absolute;'></div>");
                    c.feedbackElement.addClass(c.toThemeProperty("jqx-listbox-feedback"));
                    c.feedbackElement.appendTo(a(document.body));
                    c.feedbackElement.hide();
                    c._dragCancel = false;
                    var e = g.args.position;
                    var f = a.find(".jqx-tree");
                    c._trees = f;
                    a.each(f, function() {
                        var j = a.data(this, "jqxTree").instance;
                        var l = j.host.find(".draggable");
                        j._syncItems(l);
                        if (j.allowDrag && !j.disabled) {
                            var i = a(g.target).parents("li:first");
                            if (i.length > 0) {
                                var k = j.getItem(i[0]);
                                if (k) {
                                    c._dragItem = k;
                                    if (j.dragStart) {
                                        var h = j.dragStart(k);
                                        if (h == false) {
                                            c._dragCancel = true;
                                            a(g.args.element).jqxDragDrop({
                                                triggerEvents: false
                                            });
                                            a(g.args.element).jqxDragDrop("cancelDrag");
                                            clearInterval(c._autoScrollTimer);
                                            a(g.args.element).jqxDragDrop({
                                                triggerEvents: j
                                            });
                                            return false
                                        }
                                    }
                                    j._raiseEvent(8, {
                                        label: k.label,
                                        value: k.value,
                                        originalEvent: g.args
                                    })
                                }
                            }
                        }
                    });
                    return false
                })
            },
            _getMouseCoordinates: function(b) {
                this._isTouchDevice = a.jqx.mobile.isTouchDevice();
                if (this._isTouchDevice) {
                    var c = a.jqx.position(b.args);
                    return {
                        left: c.left,
                        top: c.top
                    }
                } else {
                    return {
                        left: b.args.pageX,
                        top: b.args.pageY
                    }
                }
            },
            _handleDragging: function(c, b) {
                var c = this.host.find(".draggable");
                c.off("dragging");
                c.on("dragging", function(h) {
                    var f = h.args;
                    var d = f.position;
                    var e = b._trees;
                    if (b._dragCancel) {
                        return
                    }
                    if (b._dragOverItem) {
                        b._dragOverItem.titleElement.removeClass(b.toThemeProperty("jqx-fill-state-hover"))
                    }
                    var i = true;
                    var g = b._getMouseCoordinates(h);
                    b._lastDraggingPosition = g;
                    a.each(e, function() {
                        if (a.jqx.isHidden(a(this))) {
                            return true
                        }
                        var m = a(this).offset();
                        var q = m.top + 20;
                        var j = a(this).height() + q - 40;
                        var l = m.left;
                        var k = a(this).width();
                        var r = l + k;
                        var p = a.data(this, "jqxTree").instance;
                        if (p.disabled || !p.allowDrop) {
                            return
                        }
                        var n = p.vScrollInstance;
                        var s = p._hitTestBounds(p, g.left, g.top);
                        if (s != null) {
                            if (b._dragOverItem) {
                                b._dragOverItem.titleElement.removeClass(p.toThemeProperty("jqx-fill-state-hover"))
                            }
                            b._dragOverItem = s;
                            if (s.element) {
                                b.feedbackElement.show();
                                var t = s.top;
                                var o = g.top;
                                b._dropPosition = "before";
                                if (o > t + s.height / 3) {
                                    t = s.top + s.height / 2;
                                    b._dragOverItem.titleElement.addClass(b.toThemeProperty("jqx-fill-state-hover"));
                                    b.feedbackElement.hide();
                                    b._dropPosition = "inside"
                                }
                                if (o > (s.top + s.height) - s.height / 3) {
                                    t = s.top + s.height;
                                    b._dragOverItem.titleElement.removeClass(b.toThemeProperty("jqx-fill-state-hover"));
                                    b.feedbackElement.show();
                                    b._dropPosition = "after"
                                }
                                b.feedbackElement.css("top", t);
                                var l = -2 + parseInt(s.titleElement.offset().left);
                                b.feedbackElement.css("left", l);
                                b.feedbackElement.width(a(s.titleElement).width() + 12)
                            }
                        }
                        if (g.left >= l && g.left < r) {
                            if (g.top + 20 >= q && g.top <= q + p.host.height()) {
                                i = false
                            }
                            if (g.top < q && g.top >= q - 30) {
                                clearInterval(p._autoScrollTimer);
                                if (n.value != 0) {
                                    b.feedbackElement.hide()
                                }
                                p._autoScrollTimer = setInterval(function() {
                                    var v = p.panelInstance.scrollUp();
                                    var u = p.host.find(".draggable");
                                    p._syncItems(u);
                                    if (!v) {
                                        clearInterval(p._autoScrollTimer)
                                    }
                                }, 100)
                            } else {
                                if (g.top > j && g.top < j + 30) {
                                    clearInterval(p._autoScrollTimer);
                                    if (n.value != n.max) {
                                        b.feedbackElement.hide()
                                    }
                                    p._autoScrollTimer = setInterval(function() {
                                        var v = p.panelInstance.scrollDown();
                                        var u = p.host.find(".draggable");
                                        p._syncItems(u);
                                        if (!v) {
                                            clearInterval(p._autoScrollTimer)
                                        }
                                    }, 100)
                                } else {
                                    clearInterval(p._autoScrollTimer)
                                }
                            }
                        } else {
                            clearInterval(p._autoScrollTimer)
                        }
                    });
                    if (i) {
                        if (b.feedbackElement) {
                            b.feedbackElement.hide()
                        }
                    }
                })
            },
            _handleDragEnd: function(c, b) {
                c.off("dragEnd");
                c.on("dragEnd", function(f) {
                    var d = b.host.find(".draggable");
                    clearInterval(b._autoScrollTimer);
                    var k = f.args.position;
                    var s = b._trees;
                    var t = null;
                    var n = a.jqx.mobile.isTouchDevice();
                    var g = n ? b._lastDraggingPosition : b._getMouseCoordinates(f);
                    b.feedbackElement.remove();
                    if (b._dragCancel) {
                        return false
                    }
                    if (b._dragOverItem) {
                        b._dragOverItem.titleElement.removeClass(b.toThemeProperty("jqx-fill-state-hover"))
                    }
                    a.each(s, function() {
                        if (a.jqx.isHidden(a(this))) {
                            return true
                        }
                        var y = parseInt(a(this).offset().left);
                        var w = y + a(this).width();
                        var v = a.data(this, "jqxTree").instance;
                        clearInterval(v._autoScrollTimer);
                        if (b._dragItem != null) {
                            if (g.left >= y && g.left < w) {
                                var x = parseInt(a(this).offset().top);
                                var u = x + a(this).height();
                                if (g.top >= x && g.top <= u) {
                                    t = a(this)
                                }
                            }
                        }
                    });
                    var r = b._dragItem;
                    if (t != null && t.length > 0) {
                        var l = t.jqxTree("allowDrop");
                        if (l) {
                            var m = a.data(t[0], "jqxTree").instance;
                            var o = b._dragOverItem;
                            if (o != null && b._dragOverItem.treeInstance.element.id == m.element.id) {
                                var q = true;
                                if (b.dragEnd) {
                                    q = b.dragEnd(r, o, f.args, b._dropPosition, t);
                                    if (q == false) {
                                        a(f.args.element).jqxDragDrop({
                                            triggerEvents: false
                                        });
                                        a(f.args.element).jqxDragDrop("cancelDrag");
                                        clearInterval(b._autoScrollTimer);
                                        a(f.args.element).jqxDragDrop({
                                            triggerEvents: true
                                        })
                                    }
                                    if (undefined == q) {
                                        q = true
                                    }
                                }
                                if (q) {
                                    var e = function() {
                                        var u = b._dragItem.treeInstance;
                                        u._refreshMapping();
                                        u._updateItemsNavigation();
                                        u._render(true, false);
                                        if (u.checkboxes) {
                                            u._updateCheckStates()
                                        }
                                        b._dragItem.treeInstance = m;
                                        b._syncItems(b._dragItem.treeInstance.host.find(".draggable"))
                                    };
                                    if (m.dropAction != "none") {
                                        if (b._dragItem.id != b._dragOverItem.id) {
                                            if (b._dropPosition == "inside") {
                                                m._drop(b._dragItem.element, b._dragOverItem.element, -1, m);
                                                e()
                                            } else {
                                                var i = 0;
                                                if (b._dropPosition == "after") {
                                                    i++
                                                }
                                                m._drop(b._dragItem.element, b._dragOverItem.parentElement, i + a(b._dragOverItem.element).index(), m);
                                                e()
                                            }
                                        }
                                    }
                                    m._render(true, false);
                                    var p = m.host.find(".draggable");
                                    b._syncItems(p);
                                    b._dragOverItem = null;
                                    b._dragItem = null;
                                    m._refreshMapping();
                                    m._updateItemsNavigation();
                                    m.selectedItem = null;
                                    m.selectItem(r.element);
                                    if (m.checkboxes) {
                                        m._updateCheckStates()
                                    }
                                    m._render(true, false)
                                }
                            } else {
                                if (m.dropAction != "none") {
                                    if (m.allowDrop) {
                                        var q = true;
                                        if (b.dragEnd) {
                                            q = b.dragEnd(r, o, f.args, b._dropPosition, t);
                                            if (q == false) {
                                                a(f.args.element).jqxDragDrop({
                                                    triggerEvents: false
                                                });
                                                a(f.args.element).jqxDragDrop("cancelDrag");
                                                clearInterval(b._autoScrollTimer);
                                                a(f.args.element).jqxDragDrop({
                                                    triggerEvents: true
                                                })
                                            }
                                            if (undefined == q) {
                                                q = true
                                            }
                                        }
                                        if (q) {
                                            b._dragItem.parentElement = null;
                                            m._drop(b._dragItem.element, null, -1, m);
                                            var h = b._dragItem.treeInstance;
                                            h._refreshMapping();
                                            h._updateItemsNavigation();
                                            if (h.checkboxes) {
                                                h._updateCheckStates()
                                            }
                                            var p = h.host.find(".draggable");
                                            b._syncItems(p);
                                            b._dragItem.treeInstance = m;
                                            m.items[m.items.length] = b._dragItem;
                                            m._render(true, false);
                                            m.selectItem(r.element);
                                            m._refreshMapping();
                                            m._updateItemsNavigation();
                                            var p = m.host.find(".draggable");
                                            m._syncItems(p);
                                            if (m.checkboxes) {
                                                m._updateCheckStates()
                                            }
                                            b._dragOverItem = null;
                                            b._dragItem = null
                                        }
                                    }
                                }
                            }
                        }
                    } else {
                        if (b.dragEnd) {
                            var j = b.dragEnd(r, f.args);
                            if (false == j) {
                                return false
                            }
                        }
                    }
                    if (r != null) {
                        b._raiseEvent(7, {
                            label: r.label,
                            value: r.value,
                            originalEvent: f.args
                        })
                    }
                    return false
                })
            },
            _drop: function(f, b, e, c) {
                if (a(b).parents("#" + f.id).length > 0) {
                    return
                }
                if (b != null) {
                    if (b.id == f.id) {
                        return
                    }
                }
                var h = this;
                if (c.element.innerHTML.indexOf("UL")) {
                    var i = c.host.find("ul:first")
                }
                if (b == undefined && b == null) {
                    if (e == undefined || e == -1) {
                        i.append(f)
                    } else {
                        if (i.children("li").eq(e).length == 0) {
                            i.children("li").eq(e - 1).after(f)
                        } else {
                            if (i.children("li").eq(e)[0].id != f.id) {
                                i.children("li").eq(e).before(f)
                            }
                        }
                    }
                } else {
                    if (e == undefined || e == -1) {
                        b = a(b);
                        var d = b.find("ul:first");
                        if (d.length == 0) {
                            ulElement = a("<ul></ul>");
                            a(b).append(ulElement);
                            d = b.find("ul:first");
                            var g = c.itemMapping["id" + b[0].id].item;
                            g.subtreeElement = d[0];
                            g.hasItems = true;
                            d.addClass(c.toThemeProperty("jqx-tree-dropdown"));
                            d.append(f);
                            f = d.find("li:first");
                            g.parentElement = f
                        } else {
                            d.append(f)
                        }
                    } else {
                        b = a(b);
                        var d = b.find("ul:first");
                        if (d.length == 0) {
                            ulElement = a("<ul></ul>");
                            a(b).append(ulElement);
                            d = b.find("ul:first");
                            if (b) {
                                var g = c.itemMapping["id" + b[0].id].item;
                                g.subtreeElement = d[0];
                                g.hasItems = true
                            }
                            d.addClass(c.toThemeProperty("jqx-tree-dropdown"));
                            d.append(f);
                            f = d.find("li:first");
                            g.parentElement = f
                        } else {
                            if (d.children("li").eq(e).length == 0) {
                                d.children("li").eq(e - 1).after(f)
                            } else {
                                if (d.children("li").eq(e)[0].id != f.id) {
                                    d.children("li").eq(e).before(f)
                                }
                            }
                        }
                    }
                }
            },
            _enableDragDrop: function() {
                if (this.allowDrag && this.host.jqxDragDrop) {
                    var d = this.host.find(".draggable");
                    var c = this;
                    if (d.length > 0) {
                        d.jqxDragDrop({
                            cursor: "arrow",
                            revertDuration: 0,
                            appendTo: "body",
                            dragZIndex: 99999,
                            revert: true,
                            initFeedback: function(e) {
                                var g = a('<span style="white-space: nowrap;" class="' + c.toThemeProperty("jqx-fill-state-normal") + '">' + e.text() + "</span>");
                                a(document.body).append(g);
                                var f = g.width();
                                g.remove();
                                e.width(f + 5);
                                e.addClass(c.toThemeProperty("jqx-fill-state-pressed"))
                            }
                        });
                        var b = d.jqxDragDrop("isDestroyed");
                        if (b) {
                            d.jqxDragDrop("_createDragDrop")
                        }
                        this._autoScrollTimer = null;
                        c._dragItem = null;
                        c._handleDragStart(d, c);
                        c._handleDragging(d, c);
                        c._handleDragEnd(d, c)
                    }
                }
            }
        })
    }
})(jqxBaseFramework);