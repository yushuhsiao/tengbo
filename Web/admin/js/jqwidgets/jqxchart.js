/*
jQWidgets v3.6.0 (2014-Nov-25)
Copyright (c) 2011-2014 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a) {
    a.jqx.jqxWidget("jqxChart", "", {});
    a.extend(a.jqx._jqxChart.prototype, {
        defineInstance: function() {
            var c = {
                title: "Title",
                description: "Description",
                source: [],
                seriesGroups: [],
                categoryAxis: null,
                xAxis: {},
                renderEngine: "",
                enableAnimations: true,
                enableAxisTextAnimation: false,
                backgroundImage: "",
                background: "#FFFFFF",
                padding: {
                    left: 5,
                    top: 5,
                    right: 5,
                    bottom: 5
                },
                backgroundColor: "#FFFFFF",
                showBorderLine: true,
                borderLineWidth: 1,
                borderLineColor: null,
                borderColor: null,
                titlePadding: {
                    left: 5,
                    top: 5,
                    right: 5,
                    bottom: 10
                },
                showLegend: true,
                legendLayout: null,
                enabled: true,
                colorScheme: "scheme01",
                animationDuration: 500,
                showToolTips: true,
                toolTipShowDelay: 500,
                toolTipDelay: 500,
                toolTipHideDelay: 4000,
                toolTipFormatFunction: null,
                columnSeriesOverlap: false,
                rtl: false,
                legendPosition: null,
                greyScale: false,
                axisPadding: 5,
                enableCrosshairs: false,
                crosshairsColor: "#BCBCBC",
                crosshairsDashStyle: "2,2",
                crosshairsLineWidth: 1,
                enableEvents: true,
                _itemsToggleState: [],
                _isToggleRefresh: false,
                drawBefore: null,
                draw: null
            };
            a.extend(true, this, c);
            this._createColorsCache()
        },
        _defaultLineColor: "#BCBCBC",
        _touchEvents: {
            mousedown: a.jqx.mobile.getTouchEventName("touchstart"),
            click: a.jqx.mobile.getTouchEventName("touchstart"),
            mouseup: a.jqx.mobile.getTouchEventName("touchend"),
            mousemove: a.jqx.mobile.getTouchEventName("touchmove"),
            mouseenter: "mouseenter",
            mouseleave: "mouseleave"
        },
        _getEvent: function(c) {
            if (this._isTouchDevice) {
                return this._touchEvents[c]
            } else {
                return c
            }
        },
        createInstance: function(e) {
            if (!a.jqx.dataAdapter) {
                throw "jqxdata.js is not loaded"
            }
            var d = this;
            d._refreshOnDownloadComlete();
            d._isTouchDevice = a.jqx.mobile.isTouchDevice();
            d.addHandler(d.host, d._getEvent("mousemove"), function(i) {
                if (d.enabled == false) {
                    return
                }
                var h = i.pageX || i.clientX || i.screenX;
                var l = i.pageY || i.clientY || i.screenY;
                var k = d.host.offset();
                if (d._isTouchDevice) {
                    var j = a.jqx.position(i);
                    h = j.left;
                    l = j.top
                } else {
                    i.preventDefault()
                }
                h -= k.left;
                l -= k.top;
                d.onmousemove(h, l)
            });
            d.addHandler(d.host, d._getEvent("mouseleave"), function(j) {
                if (d.enabled == false) {
                    return
                }
                var h = d._mouseX;
                var k = d._mouseY;
                var i = d._plotRect;
                if (i && h >= i.x && h <= i.x + i.width && k >= i.y && k <= i.y + i.height) {
                    return
                }
                d._cancelTooltipTimer();
                d._hideToolTip(0);
                d._unselect()
            });
            d.addHandler(d.host, "click", function(i) {
                if (d.enabled == false) {
                    return
                }
                var h = i.pageX || i.clientX || i.screenX;
                var l = i.pageY || i.clientY || i.screenY;
                var k = d.host.offset();
                if (d._isTouchDevice) {
                    var j = a.jqx.position(i);
                    h = j.left;
                    l = j.top
                } else {
                    i.preventDefault()
                }
                h -= k.left;
                l -= k.top;
                d._mouseX = h;
                d._mouseY = l;
                if (!isNaN(d._lastClickTs)) {
                    if ((new Date()).valueOf() - d._lastClickTs < 100) {
                        return
                    }
                }
                this._hostClickTimer = setTimeout(function() {
                    if (!d._isTouchDevice) {
                        d._cancelTooltipTimer();
                        d._hideToolTip();
                        d._unselect()
                    }
                    if (d._pointMarker && d._pointMarker.element) {
                        var n = d.seriesGroups[d._pointMarker.gidx];
                        var m = n.series[d._pointMarker.sidx];
                        d._raiseItemEvent("click", n, m, d._pointMarker.iidx)
                    }
                }, 100)
            });
            var f = d.element.style;
            if (f) {
                var c = false;
                if (f.width != null) {
                    c |= f.width.toString().indexOf("%") != -1
                }
                if (f.height != null) {
                    c |= f.height.toString().indexOf("%") != -1
                }
                if (c) {
                    a.jqx.utilities.resize(this.host, function() {
                        if (d.timer) {
                            clearTimeout(d.timer)
                        }
                        var h = 1;
                        d.timer = setTimeout(function() {
                            var i = d.enableAnimations;
                            d.enableAnimations = false;
                            d.refresh();
                            d.enableAnimations = i
                        }, h)
                    }, false, true)
                }
            }
        },
        _refreshOnDownloadComlete: function() {
            var e = this;
            var f = this.source;
            if (f instanceof a.jqx.dataAdapter) {
                var h = f._options;
                if (h == undefined || (h != undefined && !h.autoBind)) {
                    f.autoSync = false;
                    f.dataBind()
                }
                var d = this.element.id;
                if (f.records.length == 0) {
                    var c = function() {
                        if (e.ready) {
                            e.ready()
                        }
                        e.refresh()
                    };
                    f.unbindDownloadComplete(d);
                    f.bindDownloadComplete(d, c)
                } else {
                    if (e.ready) {
                        e.ready()
                    }
                }
                f.unbindBindingUpdate(d);
                f.bindBindingUpdate(d, function() {
                    e.refresh()
                })
            }
        },
        propertyChangedHandler: function(c, d, f, e) {
            if (this.isInitialized == undefined || this.isInitialized == false) {
                return
            }
            if (d == "source") {
                this._refreshOnDownloadComlete()
            }
            this.refresh()
        },
        _initRenderer: function(c) {
            if (!a.jqx.createRenderer) {
                throw "Please include a reference to jqxdraw.js"
            }
            return a.jqx.createRenderer(this, c)
        },
        _internalRefresh: function() {
            var c = this;
            if (a.jqx.isHidden(c.host)) {
                return
            }
            c._stopAnimations();
            if (!c.renderer || (!c._isToggleRefresh && !c._isUpdate)) {
                c._isVML = false;
                c.host.empty();
                c._ttEl = undefined;
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
            c._isUpdate = false
        },
        saveAsPNG: function(e, c, d) {
            return this._saveAsImage("png", e, c, d)
        },
        saveAsJPEG: function(e, c, d) {
            return this._saveAsImage("jpeg", e, c, d)
        },
        _saveAsImage: function(e, f, c, d) {
            return a.jqx._widgetToImage(this, e, f, c, d)
        },
        refresh: function() {
            this._internalRefresh()
        },
        update: function() {
            this._isUpdate = true;
            this._internalRefresh()
        },
        _seriesTypes: ["line", "stackedline", "stackedline100", "spline", "stackedspline", "stackedspline100", "stepline", "stackedstepline", "stackedstepline100", "area", "stackedarea", "stackedarea100", "splinearea", "stackedsplinearea", "stackedsplinearea100", "steparea", "stackedsteparea", "stackedsteparea100", "rangearea", "splinerangearea", "steprangearea", "column", "stackedcolumn", "stackedcolumn100", "rangecolumn", "scatter", "stackedscatter", "stackedscatter100", "bubble", "stackedbubble", "stackedbubble100", "pie", "donut", "candlestick", "ohlc"],
        _render: function(H) {
            var n = this;
            var L = n.renderer;
            n._colorsCache.clear();
            if (!n._isToggleRefresh && n._isUpdate && n._renderData) {
                n._renderDataClone()
            }
            n._renderData = [];
            L.clear();
            n._unselect();
            n._hideToolTip(0);
            var o = n.backgroundImage;
            if (o == undefined || o == "") {
                n.host.css({
                    "background-image": ""
                })
            } else {
                n.host.css({
                    "background-image": (o.indexOf("(") != -1 ? o : "url('" + o + "')")
                })
            }
            n._rect = H;
            var aa = n.padding || {
                left: 5,
                top: 5,
                right: 5,
                bottom: 5
            };
            var s = L.createClipRect(H);
            var N = L.beginGroup();
            L.setClip(N, s);
            var ai = L.rect(H.x, H.y, H.width - 2, H.height - 2);
            if (o == undefined || o == "") {
                L.attr(ai, {
                    fill: n.backgroundColor || n.background || "white"
                })
            } else {
                L.attr(ai, {
                    fill: "transparent"
                })
            }
            if (n.showBorderLine != false) {
                var J = n.borderLineColor == undefined ? n.borderColor : n.borderLineColor;
                if (J == undefined) {
                    J = n._defaultLineColor
                }
                var p = this.borderLineWidth;
                if (isNaN(p) || p < 0 || p > 10) {
                    p = 1
                }
                L.attr(ai, {
                    "stroke-width": p,
                    stroke: J
                })
            }
            if (a.isFunction(n.drawBefore)) {
                n.drawBefore(L, H)
            }
            var W = {
                x: aa.left,
                y: aa.top,
                width: H.width - aa.left - aa.right,
                height: H.height - aa.top - aa.bottom
            };
            n._paddedRect = W;
            var k = n.titlePadding || {
                left: 2,
                top: 2,
                right: 2,
                bottom: 2
            };
            var m;
            if (n.title && n.title.length > 0) {
                var T = n.toThemeProperty("jqx-chart-title-text", null);
                m = L.measureText(n.title, 0, {
                    "class": T
                });
                L.text(n.title, W.x + k.left, W.y + k.top, W.width - (k.left + k.right), m.height, 0, {
                    "class": T
                }, true, "center", "center");
                W.y += m.height;
                W.height -= m.height
            }
            if (n.description && n.description.length > 0) {
                var U = n.toThemeProperty("jqx-chart-title-description", null);
                m = L.measureText(n.description, 0, {
                    "class": U
                });
                L.text(n.description, W.x + k.left, W.y + k.top, W.width - (k.left + k.right), m.height, 0, {
                    "class": U
                }, true, "center", "center");
                W.y += m.height;
                W.height -= m.height
            }
            if (n.title || n.description) {
                W.y += (k.bottom + k.top);
                W.height -= (k.bottom + k.top)
            }
            var c = {
                x: W.x,
                y: W.y,
                width: W.width,
                height: W.height
            };
            n._buildStats(c);
            var K = n._isPieOnlySeries();
            var v = n.seriesGroups;
            var I;
            var D = {};
            for (var ab = 0; ab < v.length && !K; ab++) {
                if (v[ab].type == "pie" || v[ab].type == "donut") {
                    continue
                }
                I = v[ab].orientation == "horizontal";
                var G = v[ab].valueAxis;
                if (!G) {
                    G = v[ab].valueAxis = {}
                }
                var e = n._getCategoryAxis(ab);
                if (!e) {
                    throw "seriesGroup[" + ab + "] is missing " + (!I ? "xAxis" : "valueAxis") + " definition"
                }
                var A = e == n._getCategoryAxis() ? -1 : ab;
                D[A] = 0
            }
            var V = n.axisPadding;
            if (isNaN(V)) {
                V = 5
            }
            var u = {
                left: 0,
                right: 0,
                leftCount: 0,
                rightCount: 0
            };
            var q = [];
            for (ab = 0; ab < v.length; ab++) {
                var af = v[ab];
                if (af.type == "pie" || af.type == "donut" || af.spider == true || af.polar == true) {
                    q.push({
                        width: 0,
                        position: 0,
                        xRel: 0
                    });
                    continue
                }
                I = af.orientation == "horizontal";
                var Z = n._getCategoryAxis(ab);
                var A = Z == n._getCategoryAxis() ? -1 : ab;
                var G = af.valueAxis;
                if (!G) {
                    G = af.valueAxis = {}
                }
                var S = !I ? G.axisSize : Z.axisSize;
                var l = {
                    x: 0,
                    y: c.y,
                    width: c.width,
                    height: c.height
                };
                var R;
                if (!S || S == "auto") {
                    if (I) {
                        S = this._renderCategoryAxis(ab, l, true, c).width;
                        if ((D[A] & 1) == 1) {
                            S = 0
                        } else {
                            if (S > 0) {
                                D[A] |= 1
                            }
                        }
                        R = n._getCategoryAxis(ab).position
                    } else {
                        S = n._renderValueAxis(ab, l, true, c).width;
                        if (af.valueAxis) {
                            R = af.valueAxis.position
                        }
                    }
                }
                if (R != "left" && n.rtl == true) {
                    R = "right"
                }
                if (R != "right") {
                    R = "left"
                }
                if (u[R + "Count"] > 0 && u[R] > 0 && S > 0) {
                    u[R] += V
                }
                q.push({
                    width: S,
                    position: R,
                    xRel: u[R]
                });
                u[R] += S;
                u[R + "Count"] ++
            }
            var B = Math.max(1, Math.max(H.width, H.height));
            var ae = {
                top: 0,
                bottom: 0,
                topCount: 0,
                bottomCount: 0
            };
            var X = [];
            for (ab = 0; ab < v.length; ab++) {
                var af = v[ab];
                if (af.type == "pie" || af.type == "donut" || af.spider == true || af.polar == true) {
                    X.push({
                        height: 0,
                        position: 0,
                        yRel: 0
                    });
                    continue
                }
                I = af.orientation == "horizontal";
                var G = af.valueAxis;
                if (!G) {
                    G = af.valueAxis = {}
                }
                var e = n._getCategoryAxis(ab);
                var A = e == n._getCategoryAxis() ? -1 : ab;
                var R;
                var ad = !I ? e.axisSize : G.axisSize;
                if (!ad || ad == "auto") {
                    if (I) {
                        ad = n._renderValueAxis(ab, {
                            x: 0,
                            y: 0,
                            width: B,
                            height: 0
                        }, true, c).height;
                        if (n.seriesGroups[ab].valueAxis) {
                            R = af.valueAxis.position
                        }
                    } else {
                        ad = n._renderCategoryAxis(ab, {
                            x: 0,
                            y: 0,
                            width: B,
                            height: 0
                        }, true).height;
                        if ((D[A] & 2) == 2) {
                            ad = 0
                        } else {
                            if (ad > 0) {
                                D[A] |= 2
                            }
                        }
                        R = n._getCategoryAxis(ab).position
                    }
                }
                if (R != "top") {
                    R = "bottom"
                }
                if (ae[R + "Count"] > 0 && ae[R] > 0 && ad > 0) {
                    ae[R] += V
                }
                X.push({
                    height: ad,
                    position: R,
                    yRel: ae[R]
                });
                ae[R] += ad;
                ae[R + "Count"] ++
            }
            n._createAnimationGroup("series");
            n._plotRect = c;
            var z = (n.showLegend != false);
            var F = !z ? {
                width: 0,
                height: 0
            } : n._renderLegend(n.legendLayout ? n._rect : W, true);
            if (this.legendLayout && (!isNaN(this.legendLayout.left) || !isNaN(this.legendLayout.top))) {
                F = {
                    width: 0,
                    height: 0
                }
            }
            if (W.height < ae.top + ae.bottom + F.height || W.width < u.left + u.right) {
                L.endGroup();
                return
            }
            c.height -= ae.top + ae.bottom + F.height;
            c.x += u.left;
            c.width -= u.left + u.right;
            c.y += ae.top;
            var t = [];
            if (!K) {
                var ag = n._getCategoryAxis().tickMarksColor || n._defaultLineColor;
                for (ab = 0; ab < v.length; ab++) {
                    var af = v[ab];
                    if (af.polar == true || af.spider == true) {
                        continue
                    }
                    I = af.orientation == "horizontal";
                    var A = n._getCategoryAxis(ab) == n._getCategoryAxis() ? -1 : ab;
                    var l = {
                        x: c.x,
                        y: 0,
                        width: c.width,
                        height: X[ab].height
                    };
                    if (X[ab].position != "top") {
                        l.y = c.y + c.height + X[ab].yRel
                    } else {
                        l.y = c.y - X[ab].yRel - X[ab].height
                    }
                    if (I) {
                        n._renderValueAxis(ab, l, false, c)
                    } else {
                        t.push(l);
                        if ((D[A] & 4) == 4) {
                            continue
                        }
                        if (!n._isGroupVisible(ab)) {
                            continue
                        }
                        n._renderCategoryAxis(ab, l, false, c);
                        D[A] |= 4
                    }
                }
            }
            if (z) {
                var E = n.legendLayout ? n._rect : W;
                var Q = W.x + a.jqx._ptrnd((W.width - F.width) / 2);
                var P = c.y + c.height + ae.bottom;
                var S = W.width;
                var ad = F.height;
                if (n.legendLayout) {
                    if (!isNaN(n.legendLayout.left)) {
                        Q = n.legendLayout.left
                    }
                    if (!isNaN(n.legendLayout.top)) {
                        P = n.legendLayout.top
                    }
                    if (!isNaN(n.legendLayout.width)) {
                        S = n.legendLayout.width
                    }
                    if (!isNaN(n.legendLayout.height)) {
                        ad = n.legendLayout.height
                    }
                }
                if (Q + S > E.x + E.width) {
                    S = E.x + E.width - Q
                }
                if (P + ad > E.y + E.height) {
                    ad = E.y + E.height - P
                }
                n._renderLegend({
                    x: Q,
                    y: P,
                    width: S,
                    height: ad
                })
            }
            n._hasHorizontalLines = false;
            if (!K) {
                for (ab = 0; ab < v.length; ab++) {
                    var af = v[ab];
                    if (af.polar == true || af.spider == true) {
                        continue
                    }
                    I = v[ab].orientation == "horizontal";
                    var l = {
                        x: c.x - q[ab].xRel - q[ab].width,
                        y: c.y,
                        width: q[ab].width,
                        height: c.height
                    };
                    if (q[ab].position != "left") {
                        l.x = c.x + c.width + q[ab].xRel
                    }
                    if (I) {
                        t.push(l);
                        if ((D[n._getCategoryAxis(ab)] & 8) == 8) {
                            continue
                        }
                        if (!n._isGroupVisible(ab)) {
                            continue
                        }
                        n._renderCategoryAxis(ab, l, false, c);
                        D[n._getCategoryAxis(ab)] |= 8
                    } else {
                        n._renderValueAxis(ab, l, false, c)
                    }
                }
            }
            if (c.width <= 0 || c.height <= 0) {
                return
            }
            n._plotRect = {
                x: c.x,
                y: c.y,
                width: c.width,
                height: c.height
            };
            for (ab = 0; ab < v.length; ab++) {
                this._drawPlotAreaLines(ab, true, {
                    gridLines: false,
                    tickMarks: false,
                    alternatingBackground: true
                });
                this._drawPlotAreaLines(ab, false, {
                    gridLines: false,
                    tickMarks: false,
                    alternatingBackground: true
                })
            }
            for (ab = 0; ab < v.length; ab++) {
                this._drawPlotAreaLines(ab, true, {
                    gridLines: true,
                    tickMarks: true,
                    alternatingBackground: false
                });
                this._drawPlotAreaLines(ab, false, {
                    gridLines: true,
                    tickMarks: true,
                    alternatingBackground: false
                })
            }
            var M = L.createClipRect({
                x: c.x - 2,
                y: c.y,
                width: c.width + 4,
                height: c.height
            });
            var O = L.beginGroup();
            L.setClip(O, M);
            for (ab = 0; ab < v.length; ab++) {
                var af = v[ab];
                var d = false;
                for (var ah in n._seriesTypes) {
                    if (n._seriesTypes[ah] == af.type) {
                        d = true;
                        break
                    }
                }
                if (!d) {
                    throw 'jqxChart: invalid series type "' + af.type + '"'
                }
                if (a.isFunction(af.drawBefore)) {
                    af.drawBefore(L, H, ab)
                }
                if (af.polar == true || af.spider == true) {
                    if (af.type.indexOf("pie") == -1 && af.type.indexOf("donut") == -1) {
                        n._renderSpiderAxis(ab, c)
                    }
                }
                if (af.bands) {
                    for (var Y = 0; Y < af.bands.length; Y++) {
                        n._renderBand(ab, Y, c)
                    }
                }
                if (af.type.indexOf("column") != -1) {
                    n._renderColumnSeries(ab, c)
                } else {
                    if (af.type.indexOf("pie") != -1 || af.type.indexOf("donut") != -1) {
                        n._renderPieSeries(ab, c)
                    } else {
                        if (af.type.indexOf("line") != -1 || af.type.indexOf("area") != -1) {
                            n._renderLineSeries(ab, c)
                        } else {
                            if (af.type.indexOf("scatter") != -1 || af.type.indexOf("bubble") != -1) {
                                n._renderScatterSeries(ab, c)
                            } else {
                                if (af.type.indexOf("candlestick") != -1) {
                                    n._renderCandleStickSeries(ab, c, false)
                                } else {
                                    if (af.type.indexOf("ohlc") != -1) {
                                        n._renderCandleStickSeries(ab, c, true)
                                    }
                                }
                            }
                        }
                    }
                }
                if (a.isFunction(af.draw)) {
                    n.draw(L, H, ab)
                }
            }
            L.endGroup();
            if (n.enabled == false) {
                var ac = L.rect(H.x, H.y, H.width, H.height);
                L.attr(ac, {
                    fill: "#777777",
                    opacity: 0.5,
                    stroke: "#00FFFFFF"
                })
            }
            if (a.isFunction(n.draw)) {
                n.draw(L, H)
            }
            L.endGroup();
            n._startAnimation("series");
            if (this._renderCategoryAxisRangeSelector) {
                var f = [];
                if (!this._isSelectorRefresh) {
                    n.removeHandler(a(document), n._getEvent("mousemove"), n._onSliderMouseMove);
                    n.removeHandler(a(document), n._getEvent("mousedown"), n._onSliderMouseDown);
                    n.removeHandler(a(document), n._getEvent("mouseup"), n._onSliderMouseUp)
                }
                for (ab = 0; ab < n.seriesGroups.length; ab++) {
                    var C = this._getCategoryAxis(ab);
                    if (f.indexOf(C) == -1) {
                        if (this._renderCategoryAxisRangeSelector(ab, t[ab])) {
                            f.push(C)
                        }
                    }
                }
            }
        },
        _isPieOnlySeries: function() {
            var d = this.seriesGroups;
            if (d.length == 0) {
                return false
            }
            for (var c = 0; c < d.length; c++) {
                if (d[c].type != "pie" && d[c].type != "donut") {
                    return false
                }
            }
            return true
        },
        _renderChartLegend: function(T, D, R, v) {
            var l = this;
            var E = l.renderer;
            var J = {
                x: D.x + 3,
                y: D.y + 3,
                width: D.width - 6,
                height: D.height - 6
            };
            var F = {
                width: J.width,
                height: 0
            };
            var H = 0,
                G = 0;
            var q = 20;
            var m = 0;
            var h = 10;
            var P = 10;
            var w = 0;
            for (var O = 0; O < T.length; O++) {
                var K = T[O].css;
                if (!K) {
                    K = l.toThemeProperty("jqx-chart-legend-text", null)
                }
                q = 20;
                var B = T[O].text;
                var k = E.measureText(B, 0, {
                    "class": K
                });
                if (k.height > q) {
                    q = k.height
                }
                if (k.width > w) {
                    w = k.width
                }
                if (v) {
                    if (O != 0) {
                        G += q
                    }
                    if (G > J.height) {
                        G = 0;
                        H += w + 2 * P + h;
                        w = k.width;
                        F.width = H + w
                    }
                } else {
                    if (H != 0) {
                        H += P
                    }
                    if (H + 2 * h + k.width > J.width && k.width < J.width) {
                        H = 0;
                        G += q;
                        q = 20;
                        m = J.width;
                        F.height = G + q
                    }
                }
                var L = false;
                if (k.width > D.width) {
                    L = true;
                    var s = D.width;
                    var S = B;
                    var V = S.split(/\s+/).reverse();
                    var n = [];
                    var u = "";
                    var p = [];
                    while (undefined != (word = V.pop())) {
                        n.push(word);
                        u = n.join(" ");
                        var C = l.renderer.measureText(u, 0, {
                            "class": K
                        });
                        if (C.width > s && p.length > 0) {
                            n.pop();
                            n = [word];
                            u = n.join(" ")
                        }
                        p.push({
                            text: u
                        })
                    }
                    k.width = 0;
                    var c = 0;
                    for (var I = 0; I < p.length; I++) {
                        var U = p[I].text;
                        var C = l.renderer.measureText(U, 0, {
                            "class": K
                        });
                        k.width = Math.max(k.width, C.width);
                        c += k.height
                    }
                    k.height = c
                }
                var z = J.x + H + k.width < D.x + D.width && J.y + G + k.height < D.y + D.height;
                if (l.legendLayout) {
                    var z = J.x + H + k.width < l._rect.x + l._rect.width && J.y + G + k.height < l._rect.y + l._rect.height
                }
                if (!R && z) {
                    var j = T[O].seriesIndex;
                    var o = T[O].groupIndex;
                    var d = T[O].itemIndex;
                    var A = T[O].color;
                    var f = l._isSerieVisible(o, j, d);
                    var Q = E.beginGroup();
                    var N = f ? T[O].opacity : 0.1;
                    if (L) {
                        var S = B;
                        var s = D.width;
                        var V = S.split(/\s+/).reverse();
                        var n = [];
                        var u = "";
                        var e = 0;
                        var p = [];
                        while (undefined != (word = V.pop())) {
                            n.push(word);
                            u = n.join(" ");
                            var C = l.renderer.measureText(u, 0, {
                                "class": K
                            });
                            if (C.width > s && p.length > 0) {
                                n.pop();
                                e += C.height;
                                n = [word];
                                u = n.join(" ")
                            }
                            p.push({
                                text: u,
                                dy: e
                            })
                        }
                        for (var I = 0; I < p.length; I++) {
                            var U = p[I].text;
                            e = p[I].dy;
                            var C = l.renderer.measureText(U, 0, {
                                "class": K
                            });
                            if (v) {
                                l.renderer.text(U, J.x + H + 1.5 * h, J.y + G + e, k.width, q, 0, {
                                    "class": K
                                }, false, "left", "center")
                            } else {
                                l.renderer.text(U, J.x + H + 1.5 * h, J.y + G + e, k.width, q, 0, {
                                    "class": K
                                }, false, "center", "center")
                            }
                        }
                        var M = E.rect(J.x + H, J.y + G + h / 2 + e / 2, h, h);
                        if (v) {
                            G += e
                        }
                        l.renderer.attr(M, {
                            fill: A,
                            "fill-opacity": N,
                            stroke: A,
                            "stroke-width": 1,
                            "stroke-opacity": T[O].opacity
                        })
                    } else {
                        var M = E.rect(J.x + H, J.y + G + h / 2, h, h);
                        l.renderer.attr(M, {
                            fill: A,
                            "fill-opacity": N,
                            stroke: A,
                            "stroke-width": 1,
                            "stroke-opacity": T[O].opacity
                        });
                        if (v) {
                            l.renderer.text(B, J.x + H + 1.5 * h, J.y + G, k.width, k.height + h / 2, 0, {
                                "class": K
                            }, false, "left", "center")
                        } else {
                            l.renderer.text(B, J.x + H + 1.5 * h, J.y + G, k.width, q, 0, {
                                "class": K
                            }, false, "center", "center")
                        }
                    }
                    l.renderer.endGroup();
                    l._setLegendToggleHandler(o, j, d, Q)
                }
                if (v) {} else {
                    H += k.width + 2 * h;
                    if (m < H) {
                        m = H
                    }
                }
            }
            if (R) {
                F.height = a.jqx._ptrnd(G + q + 5);
                F.width = a.jqx._ptrnd(m);
                return F
            }
        },
        _isSerieVisible: function(h, c, e) {
            while (this._itemsToggleState.length < h + 1) {
                this._itemsToggleState.push([])
            }
            var f = this._itemsToggleState[h];
            while (f.length < c + 1) {
                f.push(isNaN(e) ? true : [])
            }
            var d = f[c];
            if (isNaN(e)) {
                return d
            }
            if (!a.isArray(d)) {
                f[c] = d = []
            }
            while (d.length < e + 1) {
                d.push(true)
            }
            return d[e]
        },
        _isGroupVisible: function(f) {
            var e = false;
            var d = this.seriesGroups[f].series;
            if (!d) {
                return e
            }
            for (var c = 0; c < d.length; c++) {
                if (this._isSerieVisible(f, c)) {
                    e = true;
                    break
                }
            }
            return e
        },
        _toggleSerie: function(j, c, f, d) {
            var i = !this._isSerieVisible(j, c, f);
            if (d != undefined) {
                i = d
            }
            var k = this.seriesGroups[j];
            var h = k.series[c];
            this._raiseEvent("toggle", {
                state: i,
                seriesGroup: k,
                serie: h,
                elementIndex: f
            });
            if (isNaN(f)) {
                this._itemsToggleState[j][c] = i
            } else {
                var e = this._itemsToggleState[j][c];
                if (!a.isArray(e)) {
                    e = []
                }
                while (e.length < f) {
                    e.push(true)
                }
                e[f] = i
            }
            this._isToggleRefresh = true;
            this.update();
            this._isToggleRefresh = false
        },
        showSerie: function(e, c, d) {
            this._toggleSerie(e, c, d, true)
        },
        hideSerie: function(e, c, d) {
            this._toggleSerie(e, c, d, false)
        },
        _setLegendToggleHandler: function(k, d, i, f) {
            var j = this.seriesGroups[k];
            var h = j.series[d];
            var c = h.enableSeriesToggle;
            if (c == undefined) {
                c = j.enableSeriesToggle != false
            }
            if (c) {
                var e = this;
                this.renderer.addHandler(f, "click", function(l) {
                    l.preventDefault();
                    e._toggleSerie(k, d, i)
                })
            }
        },
        _renderLegend: function(p, o) {
            var u = this;
            var e = [];
            for (var t = 0; t < u.seriesGroups.length; t++) {
                var m = u.seriesGroups[t];
                if (m.showLegend == false) {
                    continue
                }
                for (var q = 0; q < m.series.length; q++) {
                    var v = m.series[q];
                    if (v.showLegend == false) {
                        continue
                    }
                    var j = u._getSerieSettings(t, q);
                    var n;
                    if (m.type == "pie" || m.type == "donut") {
                        var h = u._getCategoryAxis(t);
                        var l = v.legendFormatSettings || m.legendFormatSettings || h.formatSettings || v.formatSettings || m.formatSettings;
                        var d = v.legendFormatFunction || m.legendFormatFunction || h.formatFunction || v.formatFunction || m.formatFunction;
                        var f = u._getDataLen(t);
                        for (var k = 0; k < f; k++) {
                            n = u._getDataValue(k, v.displayText, t);
                            n = u._formatValue(n, l, d, t, q, k);
                            var c = u._getColors(t, q, k);
                            e.push({
                                groupIndex: t,
                                seriesIndex: q,
                                itemIndex: k,
                                text: n,
                                css: v.displayTextClass,
                                color: c.fillColor,
                                opacity: j.opacity
                            })
                        }
                        continue
                    }
                    var l = v.legendFormatSettings || m.legendFormatSettings;
                    var d = v.legendFormatFunction || m.legendFormatFunction;
                    n = u._formatValue(v.displayText || v.dataField || "", l, d, t, q, NaN);
                    var c = u._getSeriesColors(t, q);
                    e.push({
                        groupIndex: t,
                        seriesIndex: q,
                        text: n,
                        css: v.displayTextClass,
                        color: c.fillColor,
                        opacity: j.opacity
                    })
                }
            }
            return u._renderChartLegend(e, p, o, (u.legendLayout && u.legendLayout.flow == "vertical"))
        },
        _renderCategoryAxis: function(f, E, V, e) {
            var k = this;
            var v = k._getCategoryAxis(f);
            var U = k.seriesGroups[f];
            var ac = U.orientation == "horizontal";
            var M = {
                width: 0,
                height: 0
            };
            if (!v || v.visible == false || U.type == "spider") {
                return M
            }
            if (!k._isGroupVisible(f)) {
                return M
            }
            var ab = k._alignValuesWithTicks(f);
            if (k.rtl) {
                v.flip = true
            }
            var H = ac ? E.height : E.width;
            var C = v.text;
            var z = k._calculateXOffsets(f, H);
            var Y = z.axisStats;
            var m = v.rangeSelector;
            var K = 0;
            if (m) {
                if (!this._selectorGetSize) {
                    throw new Error("jqxChart: Missing reference to jqxchart.rangeselector.js")
                }
                K = this._selectorGetSize(v)
            }
            var L = Y.interval;
            if (isNaN(L)) {
                return
            }
            var h = {
                visible: (v.showGridLines != false),
                color: (v.gridLinesColor || k._defaultLineColor),
                unitInterval: (v.gridLinesInterval || L),
                dashStyle: v.gridLinesDashStyle,
                offsets: [],
                alternatingBackgroundColor: v.alternatingBackgroundColor,
                alternatingBackgroundColor2: v.alternatingBackgroundColor2,
                alternatingBackgroundOpacity: v.alternatingBackgroundOpacity
            };
            var F = {
                visible: (v.showTickMarks != false),
                color: (v.tickMarksColor || k._defaultLineColor),
                unitInterval: (v.tickMarksInterval || L),
                dashStyle: v.tickMarksDashStyle,
                offsets: []
            };
            var t = v.textRotationAngle || 0;
            var P;
            var aa = Y.min;
            var w = Y.max;
            var S = z.padding;
            var X = v.flip == true || k.rtl;
            if (v.type == "date") {
                h.offsets = this._generateDTOffsets(aa, w, H, S, h.unitInterval, L, Y.dateTimeUnit, ab, NaN, false, X);
                F.offsets = this._generateDTOffsets(aa, w, H, S, F.unitInterval, L, Y.dateTimeUnit, ab, NaN, false, X);
                P = this._generateDTOffsets(aa, w, H, S, L, L, Y.dateTimeUnit, ab, NaN, true, X)
            } else {
                h.offsets = this._generateOffsets(aa, w, H, S, h.unitInterval, L, ab, NaN, false, X);
                F.offsets = this._generateOffsets(aa, w, H, S, F.unitInterval, L, ab, NaN, false, X);
                P = this._generateOffsets(aa, w, H, S, L, L, ab, NaN, true, X)
            }
            if (z.length == 0) {
                P = []
            }
            var d = v.horizontalTextAlignment;
            var p = k.renderer.getRect();
            var o = p.width - E.x - E.width;
            var s = k._getDataLen(f);
            var q;
            if (k._elementRenderInfo && k._elementRenderInfo.length > f) {
                q = k._elementRenderInfo[f].xAxis
            }
            var u = [];
            var O = v.formatFunction;
            var B = v.formatSettings;
            if (v.type == "date" && !B && !O) {
                O = this._getDefaultDTFormatFn(v.baseUnit || "day")
            }
            for (var T = 0; T < P.length; T++) {
                var R = P[T].value;
                var N = P[T].offset;
                if (v.type != "date" && Y.useIndeces && v.dataField) {
                    var Z = Math.round(R);
                    R = k._getDataValue(Z, v.dataField);
                    if (R == undefined) {
                        R = ""
                    }
                }
                var C = k._formatValue(R, B, O, f, undefined, T);
                if (C == undefined || C.toString() == "") {
                    C = Y.useIndeces ? (Y.min + T).toString() : (R == undefined ? "" : R.toString())
                }
                var c = {
                    key: R,
                    text: C,
                    targetX: N,
                    x: N
                };
                if (q && q.itemOffsets[R]) {
                    c.x = q.itemOffsets[R].x;
                    c.y = q.itemOffsets[R].y
                }
                u.push(c)
            }
            var Q = v.descriptionClass;
            if (!Q) {
                Q = k.toThemeProperty("jqx-chart-axis-description", null)
            }
            var D = v["class"];
            if (!D) {
                D = k.toThemeProperty("jqx-chart-axis-text", null)
            }
            if (ac) {
                t -= 90
            }
            var W = {
                text: v.description,
                style: Q,
                halign: v.horizontalDescriptionAlignment || "center",
                valign: v.verticalDescriptionAlignment || "center",
                textRotationAngle: ac ? -90 : 0
            };
            var l = {
                textRotationAngle: t,
                style: D,
                halign: d,
                valign: v.verticalTextAlignment || "center",
                textRotationPoint: v.textRotationPoint || "auto",
                textOffset: v.textOffset
            };
            var J = (ac && v.position == "right") || (!ac && v.position == "top");
            var n = {
                rangeLength: z.rangeLength,
                itemWidth: z.itemWidth,
                intervalWidth: z.intervalWidth,
                data: z,
                rect: E
            };
            var G = {
                items: u,
                renderData: n
            };
            while (k._renderData.length < f + 1) {
                k._renderData.push({})
            }
            k._renderData[f].xAxis = n;
            var I = k._getAnimProps(f);
            var A = I.enabled && u.length < 500 ? I.duration : 0;
            if (k.enableAxisTextAnimation == false) {
                A = 0
            }
            if (!V && m) {
                if (ac) {
                    E.width -= K;
                    if (v.position != "right") {
                        E.x += K
                    }
                } else {
                    E.height -= K;
                    if (v.position == "top") {
                        E.y += K
                    }
                }
            }
            n.gridLinesSettings = h;
            n.tickMarksSettings = F;
            n.isMirror = J;
            n.rect = E;
            var j = k._renderAxis(ac, J, W, l, {
                x: E.x,
                y: E.y,
                width: E.width,
                height: E.height
            }, e, L, false, true, G, h, F, V, A);
            if (ac) {
                j.width += K
            } else {
                j.height += K
            }
            return j
        },
        _animateAxisText: function(h, k) {
            var d = h.items;
            var e = h.textSettings;
            for (var f = 0; f < d.length; f++) {
                var j = d[f];
                if (!j.visible) {
                    continue
                }
                var c = j.targetX;
                var l = j.targetY;
                if (!isNaN(j.x) && !isNaN(j.y)) {
                    c = j.x + (c - j.x) * k;
                    l = j.y + (l - j.y) * k
                }
                if (j.element) {
                    this.renderer.removeElement(j.element);
                    j.element = undefined
                }
                j.element = this.renderer.text(j.text, c, l, j.width, j.height, e.textRotationAngle, {
                    "class": e.style
                }, false, e.halign, e.valign, e.textRotationPoint)
            }
        },
        _getPolarAxisCoords: function(l, j) {
            var k = this.seriesGroups[l];
            var d = this._calcGroupOffsets(l, j).xoffsets;
            if (!d) {
                return
            }
            var f = j.x + a.jqx.getNum([k.offsetX, j.width / 2]);
            var e = j.y + a.jqx.getNum([k.offsetY, j.height / 2]);
            var h = k.radius;
            if (isNaN(h)) {
                h = Math.min(j.width, j.height) / 2 * 0.6
            }
            var c = this._alignValuesWithTicks(l);
            var i = k.startAngle;
            if (isNaN(i)) {
                i = 0
            } else {
                i = (i < 0 ? -1 : 1) * (Math.abs(i) % 360);
                i = 2 * Math.PI * i / 360
            }
            return {
                x: f,
                y: e,
                r: h,
                itemWidth: d.itemWidth,
                rangeLength: d.rangeLength,
                valuesOnTicks: c,
                startAngle: i
            }
        },
        _toPolarCoord: function(e, i, d, k) {
            var j = ((d - i.x) * 2 * Math.PI) / Math.max(1, i.width) + e.startAngle;
            var c = ((i.height + i.y) - k) * e.r / Math.max(1, i.height);
            var h = e.x + c * Math.cos(j);
            var f = e.y + c * Math.sin(j);
            return {
                x: a.jqx._ptrnd(h),
                y: a.jqx._ptrnd(f)
            }
        },
        _renderSpiderAxis: function(f, L) {
            var n = this;
            var G = n._getCategoryAxis(f);
            if (!G || G.visible == false) {
                return
            }
            var B = n.seriesGroups[f];
            var M = n._getPolarAxisCoords(f, L);
            if (!M) {
                return
            }
            var W = a.jqx._ptrnd(M.x);
            var T = a.jqx._ptrnd(M.y);
            var aa = M.r;
            var m = M.startAngle;
            if (aa < 1) {
                return
            }
            aa = a.jqx._ptrnd(aa);
            var P = Math.PI * 2 * aa;
            var I = n._calculateXOffsets(f, P);
            if (!I.rangeLength) {
                return
            }
            var S = G.unitInterval;
            if (isNaN(S) || S < 1) {
                S = 1
            }
            var h = {
                visible: (G.showGridLines != false),
                color: (G.gridLinesColor || n._defaultLineColor),
                unitInterval: (G.gridLinesInterval || G.unitInterval || S),
                dashStyle: G.gridLinesDashStyle,
                offsets: []
            };
            var N = {
                visible: (G.showTickMarks != false),
                color: (G.tickMarksColor || n._defaultLineColor),
                unitInterval: (G.tickMarksInterval || G.unitInterval || S),
                dashStyle: G.tickMarksDashStyle,
                offsets: []
            };
            var e = G.horizontalTextAlignment;
            var al = n._alignValuesWithTicks(f);
            var R = n.renderer;
            var X;
            var ai = I.axisStats;
            var ak = ai.min;
            var H = ai.max;
            var ac = this._getPaddingSize(I.axisStats, G, al, P, true, false);
            var ag = G.flip == true || n.rtl;
            if (G.type == "date") {
                h.offsets = this._generateDTOffsets(ak, H, P, ac, h.unitInterval, S, G.baseUnit, false, 0, false, ag);
                N.offsets = this._generateDTOffsets(ak, H, P, ac, N.unitInterval, S, G.baseUnit, false, 0, false, ag);
                X = this._generateDTOffsets(ak, H, P, ac, S, S, G.baseUnit, false, 0, true, ag)
            } else {
                h.offsets = this._generateOffsets(ak, H, P, ac, h.unitInterval, S, true, 0, false, ag);
                N.offsets = this._generateOffsets(ak, H, P, ac, N.unitInterval, S, true, 0, false, ag);
                X = this._generateOffsets(ak, H, P, ac, S, S, true, 0, false, ag)
            }
            var e = G.horizontalTextAlignment;
            var u = n.renderer.getRect();
            var t = u.width - L.x - L.width;
            var A = n._getDataLen(f);
            var w;
            if (n._elementRenderInfo && n._elementRenderInfo.length > f) {
                w = n._elementRenderInfo[f].xAxis
            }
            var D = [];
            for (var ad = 0; ad < X.length; ad++) {
                var U = X[ad].offset;
                var Z = X[ad].value;
                if (G.type != "date" && ai.useIndeces && G.dataField) {
                    var aj = Math.round(Z);
                    Z = n._getDataValue(aj, G.dataField);
                    if (Z == undefined) {
                        Z = ""
                    }
                }
                var J = n._formatValue(Z, G.formatSettings, G.formatFunction, f, undefined, ad);
                if (J == undefined || J.toString() == "") {
                    J = ai.useIndeces ? (ai.min + ad).toString() : (Z == undefined ? "" : Z.toString())
                }
                var d = {
                    key: Z,
                    text: J,
                    targetX: U,
                    x: U
                };
                if (w && w.itemOffsets[Z]) {
                    d.x = w.itemOffsets[Z].x;
                    d.y = w.itemOffsets[Z].y
                }
                D.push(d)
            }
            var Y = G.descriptionClass;
            if (!Y) {
                Y = n.toThemeProperty("jqx-chart-axis-description", null)
            }
            var K = G["class"];
            if (!K) {
                K = n.toThemeProperty("jqx-chart-axis-text", null)
            }
            var J = G.text;
            var C = G.textRotationAngle || 0;
            var am = n.seriesGroups[f].orientation == "horizontal";
            if (am) {
                C -= 90
            }
            var af = {
                text: G.description,
                style: Y,
                halign: G.horizontalDescriptionAlignment || "center",
                valign: G.verticalDescriptionAlignment || "center",
                textRotationAngle: am ? -90 : 0
            };
            var p = {
                textRotationAngle: C,
                style: K,
                halign: e,
                valign: G.verticalTextAlignment || "center",
                textRotationPoint: G.textRotationPoint || "auto",
                textOffset: G.textOffset
            };
            var Q = (am && G.position == "right") || (!am && G.position == "top");
            var s = {
                rangeLength: I.rangeLength,
                itemWidth: I.itemWidth
            };
            var O = {
                items: D,
                renderData: s
            };
            while (n._renderData.length < f + 1) {
                n._renderData.push({})
            }
            n._renderData[f].xAxis = s;
            var q = {
                stroke: h.color,
                fill: "none",
                "stroke-width": 1,
                "stroke-dasharray": h.dashStyle || ""
            };
            var ab = R.circle(W, T, aa, q);
            var E = D.length;
            var o = 2 * Math.PI / (E);
            var c = m;
            for (var ad = 0; ad < D.length; ad++) {
                var V = D[ad].x;
                var z = c + (V * 2 * Math.PI) / Math.max(1, P);
                z = (360 - z / (2 * Math.PI) * 360) % 360;
                if (z < 0) {
                    z = 360 + z
                }
                var l = R.measureText(D[ad].text, 0, {
                    "class": K
                });
                var F = this._adjustTextBoxPosition(W, T, l, aa + (N.visible ? 7 : 2), z, false, false, true);
                R.text(D[ad].text, F.x, F.y, l.width, l.height, 0, {
                    "class": K
                }, false, "center", "center")
            }
            if (h.visible) {
                for (var ad = 0; ad < h.offsets.length; ad++) {
                    var V = h.offsets[ad].offset;
                    if (!al) {
                        V -= ac.right / 2
                    }
                    var z = c + (V * 2 * Math.PI) / Math.max(1, P);
                    var k = W + aa * Math.cos(z);
                    var j = T + aa * Math.sin(z);
                    R.line(W, T, a.jqx._ptrnd(k), a.jqx._ptrnd(j), q)
                }
            }
            if (N.visible) {
                var v = 5;
                var q = {
                    stroke: N.color,
                    fill: "none",
                    "stroke-width": 1,
                    "stroke-dasharray": N.dashStyle || ""
                };
                for (var ad = 0; ad < N.offsets.length; ad++) {
                    var V = N.offsets[ad].offset;
                    if (!al) {
                        V -= ac.right / 2
                    }
                    var z = c + (V * 2 * Math.PI) / Math.max(1, P);
                    var ah = {
                        x: W + aa * Math.cos(z),
                        y: T + aa * Math.sin(z)
                    };
                    var ae = {
                        x: W + (aa + v) * Math.cos(z),
                        y: T + (aa + v) * Math.sin(z)
                    };
                    R.line(a.jqx._ptrnd(ah.x), a.jqx._ptrnd(ah.y), a.jqx._ptrnd(ae.x), a.jqx._ptrnd(ae.y), q)
                }
            }
            n._renderSpiderValueAxis(f, L)
        },
        _renderSpiderValueAxis: function(f, d) {
            var w = this;
            var l = this.seriesGroups[f];
            var z = this._getPolarAxisCoords(f, d);
            if (!z) {
                return
            }
            var K = a.jqx._ptrnd(z.x);
            var J = a.jqx._ptrnd(z.y);
            var j = z.r;
            var D = z.startAngle;
            if (j < 1) {
                return
            }
            j = a.jqx._ptrnd(j);
            var H = this.seriesGroups[f].valueAxis;
            if (!H || false == H.displayValueAxis || false == H.visible) {
                return
            }
            var p = H["class"];
            if (!p) {
                p = this.toThemeProperty("jqx-chart-axis-text", null)
            }
            var o = H.formatSettings;
            var e = l.type.indexOf("stacked") != -1 && l.type.indexOf("100") != -1;
            if (e && !o) {
                o = {
                    sufix: "%"
                }
            }
            this._calcValueAxisItems(f, j);
            var k = this._stats.seriesGroups[f].mu;
            var h = {
                visible: (H.showGridLines != false),
                color: (H.gridLinesColor || w._defaultLineColor),
                unitInterval: (H.gridLinesInterval || k || 1),
                dashStyle: H.gridLinesDashStyle
            };
            var c = {
                stroke: h.color,
                fill: "none",
                "stroke-width": 1,
                "stroke-dasharray": h.dashStyle || ""
            };
            var s = this._renderData[f].valueAxis;
            var v = s.items;
            if (v.length) {
                this.renderer.line(K, J, K, a.jqx._ptrnd(J - j), c)
            }
            v = v.reverse();
            var B = this.renderer;
            for (var E = 0; E < v.length - 1; E++) {
                var A = v[E];
                var q = (H.formatFunction) ? H.formatFunction(A) : this._formatNumber(A, o);
                var t = B.measureText(q, 0, {
                    "class": p
                });
                var n = K + (H.showTickMarks != false ? 3 : 2);
                var m = J - s.itemWidth * E - t.height;
                B.text(q, n, m, t.width, t.height, 0, {
                    "class": p
                }, false, "center", "center")
            }
            var u = H.logarithmicScale == true;
            var G = u ? v.length : s.rangeLength;
            aIncrement = 2 * Math.PI / G;
            if (h.visible) {
                var c = {
                    stroke: h.color,
                    fill: "none",
                    "stroke-width": 1,
                    "stroke-dasharray": h.dashStyle || ""
                };
                for (var E = 0; E < G; E += h.unitInterval) {
                    var m = a.jqx._ptrnd(j * E / G);
                    B.circle(K, J, m, c)
                }
            }
            var C = {
                visible: (H.showTickMarks != false),
                color: (H.tickMarksColor || w._defaultLineColor),
                unitInterval: (H.tickMarksInterval || k),
                dashStyle: H.tickMarksDashStyle
            };
            if (C.visible) {
                tickMarkSize = 5;
                var c = {
                    stroke: C.color,
                    fill: "none",
                    "stroke-width": 1,
                    "stroke-dasharray": C.dashStyle || ""
                };
                var I = K - Math.round(tickMarkSize / 2);
                var F = I + tickMarkSize;
                for (var E = 0; E < G; E += C.unitInterval) {
                    if (h.visible && (E % h.unitInterval) == 0) {
                        continue
                    }
                    var m = a.jqx._ptrnd(J - j * E / G);
                    B.line(a.jqx._ptrnd(I), m, a.jqx._ptrnd(F), m, c)
                }
            }
        },
        _renderAxis: function(K, G, V, q, C, d, I, p, W, F, f, D, U, e) {
            var s = D.visible ? 4 : 0;
            var R = 2;
            var J = {
                width: 0,
                height: 0
            };
            var t = {
                width: 0,
                height: 0
            };
            if (K) {
                J.height = t.height = C.height
            } else {
                J.width = t.width = C.width
            }
            if (!U && G) {
                if (K) {
                    C.x -= C.width
                }
            }
            var o = F.renderData;
            var c = o.itemWidth;
            if (V.text != undefined && V != "") {
                var u = V.textRotationAngle;
                var j = this.renderer.measureText(V.text, u, {
                    "class": V.style
                });
                t.width = j.width;
                t.height = j.height;
                if (!U) {
                    this.renderer.text(V.text, C.x + (K ? (!G ? R : -R + 2 * C.width - t.width) : 0), C.y + (!K ? (!G ? C.height - R - t.height : R) : 0), K ? t.width : C.width, !K ? t.height : C.height, u, {
                        "class": V.style
                    }, true, V.halign, V.valign)
                }
            }
            var O = 0;
            var A = W ? -c / 2 : 0;
            if (W && !K) {
                q.halign = "center"
            }
            var Q = C.x;
            var P = C.y;
            var H = q.textOffset;
            if (H) {
                if (!isNaN(H.x)) {
                    Q += H.x
                }
                if (!isNaN(H.y)) {
                    P += H.y
                }
            }
            if (!K) {
                Q += A;
                if (G) {
                    P += t.height > 0 ? t.height + 3 * R : 2 * R;
                    P += s - (W ? s : s / 4)
                } else {
                    P += W ? s : s / 4
                }
            } else {
                Q += R + (t.width > 0 ? t.width + R : 0) + (G ? C.width - t.width : 0);
                P += A
            }
            var T = 0;
            var N = 0;
            var v = F.items;
            o.itemOffsets = {};
            if (this._isToggleRefresh || !this._isUpdate) {
                e = 0
            }
            var n = false;
            var l = 0;
            for (var S = 0; S < v.length; S++, O += c) {
                var B = v[S].text;
                if (!isNaN(v[S].targetX)) {
                    O = v[S].targetX
                }
                var j = this.renderer.measureText(B, q.textRotationAngle, {
                    "class": q.style
                });
                if (j.width > N) {
                    N = j.width
                }
                if (j.height > T) {
                    T = j.height
                }
                l += K ? T : N;
                if (!U) {
                    if ((K && O > C.height + 2) || (!K && O > C.width + 2)) {
                        break
                    }
                    var M = K ? Q + (G ? (t.width == 0 ? s : s - R) : 0) : Q + O;
                    var L = K ? P + O : P;
                    o.itemOffsets[v[S].key] = {
                        x: M,
                        y: L
                    };
                    if (!n) {
                        if (!isNaN(v[S].x) || !isNaN(v[S].y) && e) {
                            n = true
                        }
                    }
                    v[S].targetX = M;
                    v[S].targetY = L;
                    v[S].width = !K ? c : C.width - 2 * R - s - ((t.width > 0) ? t.width + R : 0);
                    v[S].height = K ? c : C.height - 2 * R - s - ((t.height > 0) ? t.height + R : 0);
                    v[S].visible = !p || (p && (S % I) == 0)
                }
            }
            o.avgWidth = v.length == 0 ? 0 : l / v.length;
            if (!U) {
                var z = {
                    items: v,
                    textSettings: q
                };
                if (isNaN(e) || !n) {
                    e = 0
                }
                this._animateAxisText(z, e == 0 ? 1 : 0);
                if (e != 0) {
                    var k = this;
                    this._enqueueAnimation("series", undefined, undefined, e, function(i, h, w) {
                        k._animateAxisText(h, w)
                    }, z)
                }
            }
            J.width += 2 * R + s + t.width + N + (K && t.width > 0 ? R : 0);
            J.height += 2 * R + s + t.height + T + (!K && t.height > 0 ? R : 0);
            var E = {};
            var m = {
                stroke: f.color,
                "stroke-width": 1,
                "stroke-dasharray": f.dashStyle || ""
            };
            if (!U) {
                var L = a.jqx._ptrnd(C.y + (G ? C.height : 0));
                if (K) {
                    this.renderer.line(a.jqx._ptrnd(C.x + C.width), C.y, a.jqx._ptrnd(C.x + C.width), C.y + C.height, m)
                } else {
                    this.renderer.line(a.jqx._ptrnd(C.x), L, a.jqx._ptrnd(C.x + C.width + 1), L, m)
                }
            }
            J.width = a.jqx._rup(J.width);
            J.height = a.jqx._rup(J.height);
            return J
        },
        _drawPlotAreaLines: function(k, e, j) {
            var B = this.seriesGroups[k];
            var c = B.orientation != "horizontal";
            if (!this._renderData || this._renderData.length <= k) {
                return
            }
            var F = e ? "valueAxis" : "xAxis";
            var u = this._renderData[k][F];
            if (!u) {
                return
            }
            var n = this._renderData.axisDrawState;
            if (!n) {
                n = this._renderData.axisDrawState = {}
            }
            var w = "";
            if (e) {
                w = "valueAxis_" + k
            } else {
                w = "xAxis_" + ((B.xAxis || B.categoryAxis) ? k : "")
            }
            if (n[w]) {
                n = n[w]
            } else {
                n = n[w] = {}
            }
            if (!e) {
                c = !c
            }
            var D = u.gridLinesSettings;
            var q = u.tickMarksSettings;
            var h = u.rect;
            var l = this._plotRect;
            if (!D || !q) {
                return
            }
            var p = 0.5;
            var f = [];
            var d = {
                stroke: D.color,
                "stroke-width": 1,
                "stroke-dasharray": D.dashStyle || ""
            };
            var A = e ? h.y : h.x;
            var o = D.offsets;
            if (!o || o.length == 0) {
                return
            }
            for (var z = 0; z < o.length; z++) {
                if (c) {
                    C = a.jqx._ptrnd(h.y + o[z].offset);
                    if (C < h.y - p) {
                        break
                    }
                } else {
                    C = a.jqx._ptrnd(h.x + o[z].offset);
                    if (C > h.x + h.width + p) {
                        break
                    }
                }
                if (j.gridLines && D.visible != false && n.gridLines != true) {
                    if (c) {
                        this.renderer.line(a.jqx._ptrnd(l.x), C, a.jqx._ptrnd(l.x + l.width), C, d)
                    } else {
                        this.renderer.line(C, a.jqx._ptrnd(l.y), C, a.jqx._ptrnd(l.y + l.height), d)
                    }
                }
                f[C] = true;
                if (j.alternatingBackground && (D.alternatingBackgroundColor || D.alternatingBackgroundColor2) && n.alternatingBackground != true) {
                    var m = ((z % 2) == 0) ? D.alternatingBackgroundColor2 : D.alternatingBackgroundColor;
                    if (m) {
                        var E;
                        if (c) {
                            E = this.renderer.rect(a.jqx._ptrnd(l.x), A, a.jqx._ptrnd(l.width - 1), C - A, d)
                        } else {
                            E = this.renderer.rect(A, a.jqx._ptrnd(l.y), C - A, a.jqx._ptrnd(l.height), d)
                        }
                        this.renderer.attr(E, {
                            "stroke-width": 0,
                            fill: m,
                            opacity: D.alternatingBackgroundOpacity || 1
                        })
                    }
                    A = C
                }
            }
            var d = {
                stroke: q.color,
                "stroke-width": 1,
                "stroke-dasharray": q.dashStyle || ""
            };
            if (j.tickMarks && q.visible && n.tickMarks != true) {
                var t = 4;
                var o = q.offsets;
                for (var z = 0; z < o.length; z++) {
                    var C = a.jqx._ptrnd((c ? h.y + o[z].offset : h.x + o[z].offset));
                    if (f[C - 1]) {
                        C--
                    } else {
                        if (f[C + 1]) {
                            C++
                        }
                    }
                    if (c) {
                        if (C > h.y + h.height + p) {
                            break
                        }
                    } else {
                        if (C > h.x + h.width + p) {
                            break
                        }
                    }
                    var v = !u.isMirror ? -t : t;
                    if (c) {
                        this.renderer.line(h.x + h.width, C, h.x + h.width + v, C, d)
                    } else {
                        var s = a.jqx._ptrnd(h.y + (u.isMirror ? h.height : 0));
                        this.renderer.line(C, s, C, s - v, d)
                    }
                }
            }
            n.tickMarks = n.tickMarks || j.tickMarks;
            n.gridLines = n.gridLines || j.gridLines;
            n.alternatingBackground = n.alternatingBackground || j.alternatingBackground
        },
        _calcValueAxisItems: function(k, e) {
            var n = this._stats.seriesGroups[k];
            if (!n || !n.isValid) {
                return false
            }
            var z = this.seriesGroups[k];
            var c = z.orientation == "horizontal";
            var h = z.valueAxis;
            var m = h.valuesOnTicks != false;
            var f = h.dataField;
            var o = n.intervals;
            var t = e / o;
            var v = n.min;
            var s = n.mu;
            var d = h.logarithmicScale == true;
            var l = h.logarithmicScaleBase || 10;
            var j = z.type.indexOf("stacked") != -1 && z.type.indexOf("100") != -1;
            if (d) {
                s = !isNaN(h.unitInterval) ? h.unitInterval : 1
            }
            if (!m) {
                o = Math.max(o - 1, 1)
            }
            while (this._renderData.length < k + 1) {
                this._renderData.push({})
            }
            this._renderData[k].valueAxis = {};
            var p = this._renderData[k].valueAxis;
            p.itemWidth = p.intervalWidth = t;
            p.items = [];
            var q = p.items;
            for (var w = 0; w <= o; w++) {
                var u = 0;
                if (d) {
                    if (j) {
                        u = n.max / Math.pow(l, o - w)
                    } else {
                        u = v * Math.pow(l, w)
                    }
                } else {
                    u = m ? v + w * s : v + (w + 0.5) * s
                }
                q.push(u)
            }
            p.rangeLength = d && !j ? n.intervals : (n.intervals) * s;
            if (z.valueAxis.flip != true) {
                q = q.reverse()
            }
            return true
        },
        _renderValueAxis: function(f, w, N, e) {
            var M = this.seriesGroups[f];
            var S = M.orientation == "horizontal";
            var p = M.valueAxis;
            if (!p) {
                throw "SeriesGroup " + f + " is missing valueAxis definition"
            }
            var G = {
                width: 0,
                height: 0
            };
            if (!this._isGroupVisible(f) || this._isPieOnlySeries() || M.type == "spider") {
                return G
            }
            if (!this._calcValueAxisItems(f, (S ? w.width : w.height)) || false == p.displayValueAxis || false == p.visible) {
                return G
            }
            var K = p.descriptionClass;
            if (!K) {
                K = this.toThemeProperty("jqx-chart-axis-description", null)
            }
            var O = {
                text: p.description,
                style: K,
                halign: p.horizontalDescriptionAlignment || "center",
                valign: p.verticalDescriptionAlignment || "center",
                textRotationAngle: S ? 0 : (!this.rtl ? -90 : 90)
            };
            var u = p.itemsClass;
            if (!u) {
                u = this.toThemeProperty("jqx-chart-axis-text", null)
            }
            var k = {
                style: u,
                halign: p.horizontalTextAlignment || "center",
                valign: p.verticalTextAlignment || "center",
                textRotationAngle: p.textRotationAngle || 0,
                textRotationPoint: p.textRotationPoint || "auto",
                textOffset: p.textOffset
            };
            var R = p.valuesOnTicks != false;
            var H = this._stats.seriesGroups[f];
            var j = H.mu;
            var v = p.formatSettings;
            var d = M.type.indexOf("stacked") != -1 && M.type.indexOf("100") != -1;
            if (d && !v) {
                v = {
                    sufix: "%"
                }
            }
            var F = p.logarithmicScale == true;
            var C = p.logarithmicScaleBase || 10;
            if (F) {
                j = !isNaN(p.unitInterval) ? p.unitInterval : 1
            }
            var o = [];
            var l = this._renderData[f].valueAxis;
            var n;
            if (this._elementRenderInfo && this._elementRenderInfo.length > f) {
                n = this._elementRenderInfo[f].valueAxis
            }
            for (var L = 0; L < l.items.length; L++) {
                var J = l.items[L];
                var t = (p.formatFunction) ? p.formatFunction(J) : this._formatNumber(J, v);
                var c = {
                    key: J,
                    text: t
                };
                if (n && n.itemOffsets[J]) {
                    c.x = n.itemOffsets[J].x;
                    c.y = n.itemOffsets[J].y
                }
                o.push(c)
            }
            var m = p.gridLinesInterval || p.unitInterval;
            if (isNaN(m) || (F && m < j)) {
                m = j
            }
            var B = S ? w.width : w.height;
            var h = {
                visible: (p.showGridLines != false),
                color: (p.gridLinesColor || this._defaultLineColor),
                unitInterval: m,
                dashStyle: p.gridLinesDashStyle,
                alternatingBackgroundColor: p.alternatingBackgroundColor,
                alternatingBackgroundColor2: p.alternatingBackgroundColor2,
                alternatingBackgroundOpacity: p.alternatingBackgroundOpacity
            };
            var Q = H.logarithmic ? H.minPow : H.min;
            var q = H.logarithmic ? H.maxPow : H.max;
            var P = false;
            if (h.visible || p.alternatingBackgroundColor || p.alternatingBackgroundColor2) {
                h.offsets = this._generateOffsets(Q, q, B, {
                    left: 0,
                    right: 0
                }, h.unitInterval, j, true, 0, false, P)
            }
            var I = p.tickMarksInterval || p.unitInterval;
            if (isNaN(I) || (F && I < j)) {
                I = j
            }
            var z = {
                visible: (p.showTickMarks != false),
                color: (p.tickMarksColor || this._defaultLineColor),
                unitInterval: I,
                dashStyle: p.tickMarksDashStyle
            };
            if (z.visible) {
                z.offsets = this._generateOffsets(Q, q, B, {
                    left: 0,
                    right: 0
                }, z.unitInterval, j, true, 0, false, P)
            }
            var E = (S && p.position == "top") || (!S && p.position == "right") || (!S && this.rtl && p.position != "left");
            var A = {
                items: o,
                renderData: l
            };
            var D = this._getAnimProps(f);
            var s = D.enabled && o.length < 500 ? D.duration : 0;
            if (this.enableAxisTextAnimation == false) {
                s = 0
            }
            l.gridLinesSettings = h;
            l.tickMarksSettings = z;
            l.isMirror = E;
            l.rect = w;
            return this._renderAxis(!S, E, O, k, w, e, j, F, R, A, h, z, N, s)
        },
        _generateOffsets: function(o, q, v, n, w, e, c, t, u, j) {
            var h = [];
            var k = q - o;
            var m = v - n.left - n.right;
            if (k == 0) {
                if (u || c) {
                    h.push({
                        offset: n.left + m / 2,
                        value: o
                    })
                } else {
                    h.push({
                        offset: 0,
                        value: o
                    })
                }
                return h
            }
            var z = m / k;
            var d = z * e;
            var f = n.left;
            if (!c) {
                if (!u) {
                    q += e
                }
            }
            for (var s = o; s <= q; s += e, f += d) {
                h.push({
                    offset: f,
                    value: s
                })
            }
            if (!c && h.length > 1) {
                if (isNaN(t)) {
                    t = u ? 0 : d / 2
                }
                for (var s = 0; s < h.length; s++) {
                    h[s].offset -= t;
                    if (h[s].offset <= 2) {
                        h[s].offset = 0
                    }
                    if (h[s].offset >= v - 2) {
                        h[s].offset = v
                    }
                }
            }
            if (w > e) {
                var p = [];
                var l = Math.round(w / e);
                for (var s = 0; s < h.length; s++) {
                    if ((s % l) == 0) {
                        p.push({
                            offset: h[s].offset,
                            value: h[s].value
                        })
                    }
                }
                h = p
            }
            if (j) {
                for (var s = 0; s < h.length; s++) {
                    h[s].offset = v - h[s].offset
                }
            }
            return h
        },
        _generateDTOffsets: function(s, v, C, p, D, d, q, c, z, A, j) {
            if (!q) {
                q = "day"
            }
            var h = [];
            if (s > v) {
                return h
            }
            if (s == v) {
                if (A) {
                    h.push({
                        offset: c ? C / 2 : p.left,
                        value: s
                    })
                } else {
                    if (c) {
                        h.push({
                            offset: C / 2,
                            value: s
                        })
                    }
                }
                return h
            }
            var l = C - p.left - p.right;
            var B = s;
            var m = p.left;
            var f = m;
            d = Math.max(d, 1);
            var o = d;
            var e = Math.min(1, d);
            if (d > 1 && q != "millisecond") {
                d = 1
            }
            while (a.jqx._ptrnd(f) <= a.jqx._ptrnd(p.left + l + (c ? 0 : p.right))) {
                h.push({
                    offset: f,
                    value: B
                });
                var E = new Date(B.valueOf());
                if (q == "millisecond") {
                    E.setMilliseconds(B.getMilliseconds() + d)
                } else {
                    if (q == "second") {
                        E.setSeconds(B.getSeconds() + d)
                    } else {
                        if (q == "minute") {
                            E.setMinutes(B.getMinutes() + d)
                        } else {
                            if (q == "hour") {
                                var n = E.valueOf();
                                E.setHours(B.getHours() + d);
                                if (n == E.valueOf()) {
                                    E.setHours(B.getHours() + d + 1)
                                }
                            } else {
                                if (q == "day") {
                                    E.setDate(B.getDate() + d)
                                } else {
                                    if (q == "month") {
                                        E.setMonth(B.getMonth() + d)
                                    } else {
                                        if (q == "year") {
                                            E.setFullYear(B.getFullYear() + d)
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                B = E;
                f = m + (B.valueOf() - s.valueOf()) * e / (v.valueOf() - s.valueOf()) * l
            }
            if (j) {
                for (var u = 0; u < h.length; u++) {
                    h[u].offset = C - h[u].offset
                }
            }
            if (o > 1 && q != "millisecond") {
                var t = [];
                for (var u = 0; u < h.length; u += o) {
                    t.push({
                        offset: h[u].offset,
                        value: h[u].value
                    })
                }
                h = t
            }
            if (!c && !A && h.length > 1) {
                var t = [];
                t.push({
                    offset: 0,
                    value: undefined
                });
                for (var u = 1; u < h.length; u++) {
                    t.push({
                        offset: h[u - 1].offset + (h[u].offset - h[u - 1].offset) / 2,
                        value: undefined
                    })
                }
                var w = t.length;
                if (w > 1) {
                    t.push({
                        offset: t[w - 1].offset + (t[w - 1].offset - t[w - 2].offset)
                    })
                } else {
                    t.push({
                        offset: C,
                        value: undefined
                    })
                }
                h = t
            }
            if (D > d) {
                var t = [];
                var k = Math.round(D / o);
                for (var u = 0; u < h.length; u++) {
                    if ((u % k) == 0) {
                        t.push({
                            offset: h[u].offset,
                            value: h[u].value
                        })
                    }
                }
                h = t
            }
            return h
        },
        _buildStats: function(N) {
            var X = {
                seriesGroups: []
            };
            this._stats = X;
            for (var v = 0; v < this.seriesGroups.length; v++) {
                var G = this.seriesGroups[v];
                X.seriesGroups[v] = {};
                var J = this._getCategoryAxis(v);
                var s = this._getCategoryAxisStats(v, J, (G.orientation == "vertical") ? N.width : N.height);
                var D = X.seriesGroups[v];
                D.isValid = true;
                var T = G.valueAxis != undefined;
                var O = (G.orientation == "horizontal") ? N.width : N.height;
                var Q = false;
                var P = 10;
                if (T) {
                    Q = G.valueAxis.logarithmicScale == true;
                    P = G.valueAxis.logarithmicScaleBase;
                    if (isNaN(P)) {
                        P = 10
                    }
                }
                var K = -1 != G.type.indexOf("stacked");
                var f = K && -1 != G.type.indexOf("100");
                var M = -1 != G.type.indexOf("range");
                if (f) {
                    D.psums = [];
                    D.nsums = []
                }
                var w = NaN,
                    R = NaN;
                var e = NaN,
                    h = NaN;
                var t = G.baselineValue;
                if (isNaN(t)) {
                    t = Q && !f ? 1 : 0
                }
                var F = this._getDataLen(v);
                var d = 0;
                var Z = NaN;
                for (var W = 0; W < F && D.isValid; W++) {
                    if (J.rangeSelector) {
                        var k = J.dataField ? this._getDataValue(W, J.dataField, v) : W;
                        if (k && s.isDateTime) {
                            k = this._castAsDate(k)
                        }
                        if (k && (k.valueOf() < s.min.valueOf() || k.valueOf() > s.max.valueOf())) {
                            continue
                        }
                    }
                    var aa = T ? G.valueAxis.minValue : Infinity;
                    var I = T ? G.valueAxis.maxValue : -Infinity;
                    var z = 0,
                        B = 0;
                    for (var l = 0; l < G.series.length; l++) {
                        if (!this._isSerieVisible(v, l)) {
                            continue
                        }
                        var L = NaN,
                            U = NaN,
                            E = NaN;
                        if (G.type.indexOf("candle") != -1 || G.type.indexOf("ohlc") != -1) {
                            var c = ["Open", "Low", "Close", "High"];
                            for (var V in c) {
                                var n = this._getDataValueAsNumber(W, G.series[l]["dataField" + c[V]], v);
                                if (isNaN(n)) {
                                    continue
                                }
                                E = isNaN(U) ? n : Math.min(E, n);
                                U = isNaN(U) ? n : Math.max(U, n)
                            }
                        } else {
                            if (M) {
                                var ab = this._getDataValueAsNumber(W, G.series[l].dataFieldFrom, v);
                                var H = this._getDataValueAsNumber(W, G.series[l].dataFieldTo, v);
                                U = Math.max(ab, H);
                                E = Math.min(ab, H)
                            } else {
                                L = this._getDataValueAsNumber(W, G.series[l].dataField, v);
                                if (isNaN(L) || (Q && L <= 0)) {
                                    continue
                                }
                                E = U = L
                            }
                        }
                        if ((isNaN(I) || U > I) && ((!T || isNaN(G.valueAxis.maxValue)) ? true : U <= G.valueAxis.maxValue)) {
                            I = U
                        }
                        if ((isNaN(aa) || E < aa) && ((!T || isNaN(G.valueAxis.minValue)) ? true : E >= G.valueAxis.minValue)) {
                            aa = E
                        }
                        if (!isNaN(L)) {
                            if (L > t) {
                                z += L
                            } else {
                                if (L < t) {
                                    B += L
                                }
                            }
                        }
                    }
                    if (!f && T) {
                        if (!isNaN(G.valueAxis.maxValue)) {
                            z = Math.min(G.valueAxis.maxValue, z)
                        }
                        if (!isNaN(G.valueAxis.minValue)) {
                            B = Math.max(G.valueAxis.minValue, B)
                        }
                    }
                    if (Q && f) {
                        for (var l = 0; l < G.series.length; l++) {
                            if (!this._isSerieVisible(v, l)) {
                                Z = 0.01;
                                continue
                            }
                            var L = this._getDataValueAsNumber(W, G.series[l].dataField, v);
                            if (isNaN(L) || L <= 0) {
                                Z = 0.01;
                                continue
                            }
                            var S = z == 0 ? 0 : L / z;
                            if (isNaN(Z) || S < Z) {
                                Z = S
                            }
                        }
                    }
                    var q = z - B;
                    if (d < q) {
                        d = q
                    }
                    if (f) {
                        D.psums[W] = z;
                        D.nsums[W] = B
                    }
                    if (I > R || isNaN(R)) {
                        R = I
                    }
                    if (aa < w || isNaN(w)) {
                        w = aa
                    }
                    if (z > e || isNaN(e)) {
                        e = z
                    }
                    if (B < h || isNaN(h)) {
                        h = B
                    }
                }
                if (f) {
                    e = e == 0 ? 0 : Math.max(e, -h);
                    h = h == 0 ? 0 : Math.min(h, -e)
                }
                var o = T ? G.valueAxis.unitInterval : 0;
                if (!o) {
                    if (T) {
                        o = this._calcInterval(K ? h : w, K ? e : R, Math.max(O / 80, 2))
                    } else {
                        o = K ? (e - h) / 10 : (R - w) / 10
                    }
                }
                var C = NaN;
                var Y = 0;
                var u = 0;
                if (Q) {
                    if (f) {
                        C = 0;
                        var S = 1;
                        Y = u = a.jqx.log(100, P);
                        while (S > Z) {
                            S /= P;
                            Y--;
                            C++
                        }
                        w = Math.pow(P, Y)
                    } else {
                        if (K) {
                            R = Math.max(R, e)
                        }
                        u = a.jqx._rnd(a.jqx.log(R, P), 1, true);
                        R = Math.pow(P, u);
                        Y = a.jqx._rnd(a.jqx.log(w, P), 1, false);
                        w = Math.pow(P, Y)
                    }
                    o = P
                }
                if (w < h) {
                    h = w
                }
                if (R > e) {
                    e = R
                }
                var A = Q ? w : a.jqx._rnd(K ? h : w, o, false);
                var m = Q ? R : a.jqx._rnd(K ? e : R, o, true);
                if (f && m > 100) {
                    m = 100
                }
                if (f && !Q) {
                    m = (m > 0) ? 100 : 0;
                    A = (A < 0) ? -100 : 0;
                    o = T ? G.valueAxis.unitInterval : 10;
                    if (isNaN(o) || o <= 0 || o >= 100) {
                        o = 10
                    }
                }
                if (isNaN(m) || isNaN(A) || isNaN(o)) {
                    continue
                }
                if (isNaN(C)) {
                    C = parseInt(((m - A) / (o == 0 ? 1 : o)).toFixed())
                }
                if (Q && !f) {
                    C = u - Y;
                    d = Math.pow(P, C)
                }
                if (C < 1) {
                    continue
                }
                D.min = A;
                D.max = m;
                D.logarithmic = Q;
                D.logBase = P;
                D.base = t;
                D.minPow = Y;
                D.maxPow = u;
                D.mu = o;
                D.maxRange = d;
                D.intervals = C
            }
        },
        _getDataLen: function(d) {
            var c = this.source;
            if (d != undefined && d != -1 && this.seriesGroups[d].source) {
                c = this.seriesGroups[d].source
            }
            if (c instanceof a.jqx.dataAdapter) {
                c = c.records
            }
            if (c) {
                return c.length
            }
            return 0
        },
        _getDataValue: function(c, f, e) {
            var d = this.source;
            if (e != undefined && e != -1) {
                d = this.seriesGroups[e].source || d
            }
            if (d instanceof a.jqx.dataAdapter) {
                d = d.records
            }
            if (!d || c < 0 || c > d.length - 1) {
                return undefined
            }
            if (a.isFunction(f)) {
                return f(c, d)
            }
            return (f && f != "") ? d[c][f] : d[c]
        },
        _getDataValueAsNumber: function(c, f, d) {
            var e = this._getDataValue(c, f, d);
            if (this._isDate(e)) {
                return e.valueOf()
            }
            if (typeof(e) != "number") {
                e = parseFloat(e)
            }
            if (typeof(e) != "number") {
                e = undefined
            }
            return e
        },
        _renderPieSeries: function(f, d) {
            var h = this._getDataLen(f);
            var j = this.seriesGroups[f];
            var o = this._calcGroupOffsets(f, d).offsets;
            for (var t = 0; t < j.series.length; t++) {
                var m = j.series[t];
                var z = this._getSerieSettings(f, t);
                var k = m.colorScheme || j.colorScheme || this.colorScheme;
                var v = this._getAnimProps(f, t);
                var c = v.enabled && h < 5000 && !this._isToggleRefresh && this._isVML != true ? v.duration : 0;
                if (a.jqx.mobile.isMobileBrowser() && (this.renderer instanceof a.jqx.HTML5Renderer)) {
                    c = 0
                }
                var q = {
                    rect: d,
                    groupIndex: f,
                    serieIndex: t,
                    settings: z,
                    items: []
                };
                for (var w = 0; w < h; w++) {
                    var p = o[t][w];
                    if (!p.visible) {
                        continue
                    }
                    var u = p.fromAngle;
                    var e = p.toAngle;
                    var A = this.renderer.pieslice(p.x, p.y, p.innerRadius, p.outerRadius, u, c == 0 ? e : u, p.centerOffset);
                    var l = {
                        element: A,
                        displayValue: p.displayValue,
                        itemIndex: w,
                        visible: p.visible,
                        x: p.x,
                        y: p.y,
                        innerRadius: p.innerRadius,
                        outerRadius: p.outerRadius,
                        fromAngle: u,
                        toAngle: e,
                        centerOffset: p.centerOffset
                    };
                    q.items.push(l)
                }
                this._animatePieSlices(q, 0);
                var n = this;
                this._enqueueAnimation("series", undefined, undefined, c, function(s, i, B) {
                    n._animatePieSlices(i, B)
                }, q)
            }
        },
        _sliceSortFunction: function(d, c) {
            return d.fromAngle - c.fromAngle
        },
        _animatePieSlices: function(A, d) {
            var p;
            if (this._elementRenderInfo && this._elementRenderInfo.length > A.groupIndex && this._elementRenderInfo[A.groupIndex].series && this._elementRenderInfo[A.groupIndex].series.length > A.serieIndex) {
                p = this._elementRenderInfo[A.groupIndex].series[A.serieIndex]
            }
            var l = 360 * d;
            var c = [];
            for (var F = 0; F < A.items.length; F++) {
                var K = A.items[F];
                if (!K.visible) {
                    continue
                }
                var C = K.fromAngle;
                var k = K.fromAngle + d * (K.toAngle - K.fromAngle);
                if (p && p[K.displayValue]) {
                    var v = p[K.displayValue].fromAngle;
                    var e = p[K.displayValue].toAngle;
                    C = v + (C - v) * d;
                    k = e + (k - e) * d
                }
                c.push({
                    index: F,
                    from: C,
                    to: k
                })
            }
            if (p) {
                c.sort(this._sliceSortFunction)
            }
            var L = NaN;
            for (var F = 0; F < c.length; F++) {
                var K = A.items[c[F].index];
                if (K.labelElement) {
                    this.renderer.removeElement(K.labelElement)
                }
                var C = c[F].from;
                var k = c[F].to;
                if (p) {
                    if (!isNaN(L) && C > L) {
                        C = L
                    }
                    L = k;
                    if (F == c.length - 1 && k != c[0].from) {
                        k = 360 + c[0].from
                    }
                }
                var D = this.renderer.pieSlicePath(K.x, K.y, K.innerRadius, K.outerRadius, C, k, K.centerOffset);
                this.renderer.attr(K.element, {
                    d: D
                });
                var n = this._getColors(A.groupIndex, A.serieIndex, K.itemIndex, "radialGradient", K.outerRadius);
                var J = A.settings;
                this.renderer.attr(K.element, {
                    fill: n.fillColor,
                    stroke: n.lineColor,
                    "stroke-width": J.stroke,
                    "fill-opacity": J.opacity,
                    "stroke-opacity": J.opacity,
                    "stroke-dasharray": "none" || J.dashStyle
                });
                var G = this.seriesGroups[A.groupIndex];
                var u = G.series[A.serieIndex];
                if (u.showLabels == true || (!u.showLabels && G.showLabels == true)) {
                    var N = C,
                        O = k;
                    var q = Math.abs(N - O);
                    var z = q > 180 ? 1 : 0;
                    if (q > 360) {
                        N = 0;
                        O = 360
                    }
                    var w = N * Math.PI * 2 / 360;
                    var m = O * Math.PI * 2 / 360;
                    var o = q / 2 + N;
                    o = o % 360;
                    var M = o * Math.PI * 2 / 360;
                    var B;
                    if (u.labelsAutoRotate == true) {
                        B = o < 90 || o > 270 ? 360 - o : 180 - o
                    }
                    var t = this._showLabel(A.groupIndex, A.serieIndex, K.itemIndex, {
                        x: 0,
                        y: 0,
                        width: 0,
                        height: 0
                    }, "center", "center", true, false, false, B);
                    var j = u.labelRadius || K.outerRadius + Math.max(t.width, t.height);
                    j += K.centerOffset;
                    var I = a.jqx.getNum([u.offsetX, G.offsetX, A.rect.width / 2]);
                    var H = a.jqx.getNum([u.offsetY, G.offsetY, A.rect.height / 2]);
                    var h = A.rect.x + I;
                    var f = A.rect.y + H;
                    var E = this._adjustTextBoxPosition(h, f, t, j, o, K.outerRadius > j, u.labelLinesAngles != false, u.labelsAutoRotate == true);
                    K.labelElement = this._showLabel(A.groupIndex, A.serieIndex, K.itemIndex, {
                        x: E.x,
                        y: E.y,
                        width: t.width,
                        height: t.height
                    }, "left", "top", false, false, false, B);
                    if (j > K.outerRadius + 5 && u.labelLinesEnabled != false) {
                        K.labelArrowPath = this._updateLebelArrowPath(K.labelArrowPath, h, f, j, K.outerRadius, M, u.labelLinesAngles != false, n, J)
                    }
                }
                if (d == 1) {
                    this._installHandlers(K.element, "pieslice", A.groupIndex, A.serieIndex, K.itemIndex)
                }
            }
        },
        _updateLebelArrowPath: function(f, l, i, k, m, j, p, c, h) {
            var e = a.jqx._ptrnd(l + (k - 0) * Math.cos(j));
            var o = a.jqx._ptrnd(i - (k - 0) * Math.sin(j));
            var d = a.jqx._ptrnd(l + (m + 2) * Math.cos(j));
            var n = a.jqx._ptrnd(i - (m + 2) * Math.sin(j));
            var q = "M " + e + "," + o + " L" + d + "," + n;
            if (p) {
                q = "M " + e + "," + o + " L" + d + "," + o + " L" + d + "," + n
            }
            if (f) {
                this.renderer.attr(f, {
                    d: q
                })
            } else {
                f = this.renderer.path(q, {})
            }
            this.renderer.attr(f, {
                fill: "none",
                stroke: c.lineColor,
                "stroke-width": h.stroke,
                "stroke-opacity": h.opacity,
                "stroke-dasharray": "none" || h.dashStyle
            });
            return f
        },
        _adjustTextBoxPosition: function(f, e, o, i, u, c, j, p) {
            var d = u * Math.PI * 2 / 360;
            var l = a.jqx._ptrnd(f + i * Math.cos(d));
            var k = a.jqx._ptrnd(e - i * Math.sin(d));
            if (p) {
                var m = o.width;
                var q = o.height;
                var v = Math.atan(q / m) % (Math.PI * 2);
                var z = d % (Math.PI * 2);
                var t = 0,
                    s = 0;
                var n = 0;
                if (z <= v) {
                    n = m / 2 * Math.cos(d);
                    s = n * Math.sin(d);
                    t = -m / 2
                } else {
                    if (z >= v && z < Math.PI - v) {
                        n = (q / 2) * Math.sin(d);
                        s = q / 2;
                        t = -Math.cos(d) * n
                    } else {
                        if (z >= Math.PI - v && z < Math.PI + v) {
                            n = m / 2 * Math.cos(d);
                            s = -n * Math.sin(d);
                            t = m / 2
                        } else {
                            if (z >= Math.PI + v && z < 2 * Math.PI - v) {
                                n = q / 2 * Math.sin(d);
                                s = -q / 2;
                                t = Math.cos(d) * n
                            } else {
                                if (z >= 2 * Math.PI - v && z < 2 * Math.PI) {
                                    n = m / 2 * Math.cos(d);
                                    s = n * Math.sin(d);
                                    t = -m / 2
                                }
                            }
                        }
                    }
                }
                i += Math.abs(n) + 3;
                var l = a.jqx._ptrnd(f + i * Math.cos(d));
                var k = a.jqx._ptrnd(e - i * Math.sin(d));
                l -= o.width / 2;
                k -= o.height / 2;
                return {
                    x: l,
                    y: k
                }
            }
            if (!c) {
                if (!j) {
                    if (u >= 0 && u < 45 || u >= 315 && u < 360) {
                        k -= o.height / 2
                    } else {
                        if (u >= 45 && u < 135) {
                            k -= o.height;
                            l -= o.width / 2
                        } else {
                            if (u >= 135 && u < 225) {
                                k -= o.height / 2;
                                l -= o.width
                            } else {
                                if (u >= 225 && u < 315) {
                                    l -= o.width / 2
                                }
                            }
                        }
                    }
                } else {
                    if (u >= 90 && u < 270) {
                        k -= o.height / 2;
                        l -= o.width
                    } else {
                        k -= o.height / 2
                    }
                }
            } else {
                l -= o.width / 2;
                k -= o.height / 2
            }
            return {
                x: l,
                y: k
            }
        },
        _getColumnGroupsCount: function(d) {
            var f = 0;
            d = d || "vertical";
            var h = this.seriesGroups;
            for (var e = 0; e < h.length; e++) {
                var c = h[e].orientation || "vertical";
                if (h[e].type.indexOf("column") != -1 && c == d) {
                    f++
                }
            }
            return f
        },
        _getColumnGroupIndex: function(j) {
            var c = 0;
            var d = this.seriesGroups[j].orientation || "vertical";
            for (var f = 0; f < j; f++) {
                var h = this.seriesGroups[f];
                var e = h.orientation || "vertical";
                if (h.type.indexOf("column") != -1 && e == d) {
                    c++
                }
            }
            return c
        },
        _renderBand: function(s, n, l) {
            var q = this.seriesGroups[s];
            if (!q.bands || q.bands.length <= n) {
                return
            }
            var d = l;
            if (q.orientation == "horizontal") {
                d = {
                    x: l.y,
                    y: l.x,
                    width: l.height,
                    height: l.width
                }
            }
            var t = this._calcGroupOffsets(s, d);
            if (!t || t.length <= s) {
                return
            }
            var u = q.bands[n];
            var j = t.bands[n];
            var p = j.from;
            var o = j.to;
            var f = Math.abs(p - o);
            var k = {
                x: d.x,
                y: Math.min(p, o),
                width: d.width,
                height: f
            };
            if (q.orientation == "horizontal") {
                var e = k.x;
                k.x = k.y;
                k.y = e;
                e = k.width;
                k.width = k.height;
                k.height = e
            }
            var m = this.renderer.rect(k.x, k.y, k.width, k.height);
            var c = u.color || "#AAAAAA";
            var i = u.opacity;
            if (isNaN(i) || i < 0 || i > 1) {
                i = 0.5
            }
            this.renderer.attr(m, {
                fill: c,
                "fill-opacity": i,
                stroke: c,
                "stroke-opacity": i,
                "stroke-width": 0
            })
        },
        _calcColumnWidth: function(l, h, n) {
            var f = this.seriesGroups[l];
            var c = f.minColumnsWidth || 5;
            var m = f.maxColumnsWidth || n / 5;
            for (var k in f.series) {
                if (!isNaN(f.series[k].minColumnsWidth)) {
                    c = Math.max(c, f.series[k].minColumnsWidth)
                }
                if (!isNaN(f.series[k].maxColumnsWidth)) {
                    m = Math.min(m, f.series[k].maxColumnsWidth)
                }
            }
            var d = f.columnsGapPercent;
            if (isNaN(d) || d < 0 || d > 100) {
                d = 25
            }
            d /= 100;
            var j = m;
            for (var e = h.first; e < h.last; e++) {
                if (Math.abs(h.data[e + 1] - h.data[e]) >= j * (1 + d)) {
                    continue
                } else {
                    j = Math.abs(h.data[e + 1] - h.data[e]) / (1 + d)
                }
            }
            if (j < c) {
                j = c
            }
            return {
                width: j,
                isMin: j == c,
                columnGap: d
            }
        },
        _renderColumnSeries: function(m, L) {
            var A = this.seriesGroups[m];
            if (!A.series || A.series.length == 0) {
                return
            }
            var E = A.type.indexOf("stacked") != -1;
            var f = E && A.type.indexOf("100") != -1;
            var J = A.type.indexOf("range") != -1;
            var u = this._getDataLen(m);
            var R = A.seriesGapPercent;
            if (isNaN(R) || R < 0 || R > 100) {
                R = 10
            }
            var B = A.orientation == "horizontal";
            var q = L;
            if (B) {
                q = {
                    x: L.y,
                    y: L.x,
                    width: L.height,
                    height: L.width
                }
            }
            var v = this._calcGroupOffsets(m, q);
            if (!v || v.xoffsets.length == 0) {
                return
            }
            var k = this._getColumnGroupsCount(A.orientation);
            var c = this._getColumnGroupIndex(m);
            if (this.columnSeriesOverlap == true) {
                k = 1;
                c = 0
            }
            var U = true;
            var d;
            if (A.polar == true || A.spider == true) {
                d = this._getPolarAxisCoords(m, q);
                R = 0
            }
            var C = {
                groupIndex: m,
                rect: L,
                vertical: !B,
                seriesCtx: [],
                renderData: v,
                polarAxisCoords: d
            };
            C.columnInfo = this._calcColumnWidth(m, v.xoffsets, B ? q.height : q.width);
            var z = this._getGroupGradientType(m);
            for (var n = 0; n < A.series.length; n++) {
                var Q = A.series[n];
                var M = Q.columnsMaxWidth || A.columnsMaxWidth;
                var G = Q.columnsMinWidth || A.columnsMinWidth || 1;
                var D = Q.dataField;
                var O = this._getAnimProps(m, n);
                var H = O.enabled && !this._isToggleRefresh && v.xoffsets.length < 100 ? O.duration : 0;
                var P = 0;
                var j = C.columnInfo.width;
                if (U) {
                    P -= j / 2
                }
                P += j * (c / k);
                j /= k;
                var w = P + j;
                var K = (w - P);
                var t = (!E && A.series.length > 1) ? (K * R / 100) / (A.series.length - 1) : 0;
                var I = (K - t * (A.series.length - 1));
                I = Math.max(I, G);
                if (K < 1) {
                    K = 1
                }
                var o = 0;
                if (!E && A.series.length > 1) {
                    I /= A.series.length;
                    o = n
                }
                var V = P + o * (t + I);
                if (o == A.series.length) {
                    I = K - P + K - x
                }
                if (!isNaN(M)) {
                    var N = Math.min(I, M);
                    V = V + (I - N) / 2;
                    I = N
                }
                var l = this._isSerieVisible(m, n);
                var h = this._getSerieSettings(m, n);
                var F = this._getColors(m, n, NaN, this._getGroupGradientType(m), 4);
                var e = [];
                if (a.isFunction(Q.colorFunction) && !d) {
                    for (var S = v.xoffsets.first; S <= v.xoffsets.last; S++) {
                        e.push(this._getColors(m, n, S, z, 4))
                    }
                }
                var T = {
                    seriesIndex: n,
                    serieColors: F,
                    itemsColors: e,
                    settings: h,
                    columnWidth: I,
                    xAdjust: V,
                    isVisible: l
                };
                C.seriesCtx.push(T)
            }
            this._animColumns(C, H == 0 ? 1 : 0);
            var p = this;
            this._enqueueAnimation("series", undefined, undefined, H, function(s, i, W) {
                p._animColumns(i, W)
            }, C)
        },
        _getColumnVOffsets: function(t, f, w, E, m, c) {
            var l = this.seriesGroups[f];
            var j = [];
            for (var D = 0; D < w.length; D++) {
                var B = w[D];
                var z = B.seriesIndex;
                var p = l.series[z];
                var A = t.offsets[z][E].from;
                var d = t.offsets[z][E].to;
                var F = t.xoffsets.data[E];
                var e;
                var v = B.isVisible;
                if (!v) {
                    d = A
                }
                if (v && this._elementRenderInfo && this._elementRenderInfo.length > f) {
                    var n = t.xoffsets.xvalues[E];
                    e = this._elementRenderInfo[f].series[z][n];
                    if (e && !isNaN(e.from) && !isNaN(e.to)) {
                        A = e.from + (A - e.from) * c;
                        d = e.to + (d - e.to) * c;
                        F = e.xoffset + (F - e.xoffset) * c
                    }
                }
                if (!e) {
                    d = A + (d - A) * (m ? 1 : c)
                }
                if (isNaN(A)) {
                    A = 0
                }
                if (isNaN(d)) {
                    d = isNaN(A) ? 0 : A
                }
                j.push({
                    from: A,
                    to: d,
                    xOffset: F
                })
            }
            if (m && j.length > 1 && !(this._elementRenderInfo && this._elementRenderInfo.length > f)) {
                var h = 0,
                    k = 0;
                for (var C = 0; C < j.length; C++) {
                    if (j[C].to >= j[C].from) {
                        k += j[C].to - j[C].from
                    } else {
                        h += j[C].from - j[C].to
                    }
                }
                h *= c;
                k *= c;
                var q = 0,
                    u = 0;
                for (var C = 0; C < j.length; C++) {
                    if (j[C].to >= j[C].from) {
                        var o = j[C].to - j[C].from;
                        if (o + u > k) {
                            o = Math.max(0, k - u);
                            j[C].to = j[C].from + o
                        }
                        u += o
                    } else {
                        var o = j[C].from - j[C].to;
                        if (o + q > h) {
                            o = Math.max(0, h - q);
                            j[C].to = j[C].from - o
                        }
                        q += o
                    }
                }
            }
            return j
        },
        _columnAsPieSlice: function(c, h, n, p, q) {
            var f = this._toPolarCoord(p, n, q.x, q.y);
            var i = this._toPolarCoord(p, n, q.x, q.y + q.height);
            var s = this._toPolarCoord(p, n, q.x + q.width, q.y);
            var o = a.jqx._ptdist(p.x, p.y, i.x, i.y);
            var l = a.jqx._ptdist(p.x, p.y, f.x, f.y);
            var e = n.width;
            var d = -((q.x - n.x) * 360) / e;
            var k = -((q.x + q.width - n.x) * 360) / e;
            var m = p.startAngle;
            m = 360 * m / (Math.PI * 2);
            d -= m;
            k -= m;
            if (c[h] != undefined) {
                var j = this.renderer.pieSlicePath(p.x, p.y, o, l, k, d, 0);
                this.renderer.attr(c[h], {
                    d: j
                })
            } else {
                c[h] = this.renderer.pieslice(p.x, p.y, o, l, k, d, 0)
            }
            return {
                fromAngle: k,
                toAngle: d,
                innerRadius: o,
                outerRadius: l
            }
        },
        _animColumns: function(f, c) {
            var L = f.groupIndex;
            var m = this.seriesGroups[L];
            var z = f.renderData;
            var n = m.type.indexOf("stacked") != -1;
            var s = f.polarAxisCoords;
            var h = this._getGroupGradientType(L);
            var j = f.columnInfo.width;
            var u = j;
            if (f.columnInfo.isMin) {
                u = j * (1 + f.columnInfo.columnGap)
            }
            var E = f.renderData.xoffsets;
            var K = -1;
            for (var I = E.first; I <= E.last; I++) {
                var p = E.data[I];
                if (isNaN(p)) {
                    continue
                }
                if (K != -1 && Math.abs(p - K) < u && m.skipOverlappingPoints != false) {
                    continue
                } else {
                    K = p
                }
                var l = this._getColumnVOffsets(z, L, f.seriesCtx, I, n, c);
                for (var H = 0; H < f.seriesCtx.length; H++) {
                    var F = f.seriesCtx[H];
                    var C = F.seriesIndex;
                    var A = m.series[C];
                    var D = l[H].from;
                    var e = l[H].to;
                    var M = l[H].xOffset;
                    if (!F.elements) {
                        F.elements = {}
                    }
                    if (!F.labelElements) {
                        F.labelElements = {}
                    }
                    var t = F.elements;
                    var w = F.labelElements;
                    var G = (f.vertical ? f.rect.x : f.rect.y) + F.xAdjust;
                    var J = F.settings;
                    var o = F.itemsColors.length != 0 ? F.itemsColors[I - z.xoffsets.first] : F.serieColors;
                    var B = this._isSerieVisible(L, C);
                    if (!B && !n) {
                        continue
                    }
                    var p = a.jqx._ptrnd(G + M);
                    var d = {
                        x: p,
                        width: F.columnWidth
                    };
                    var q = true;
                    if (f.vertical) {
                        d.y = D;
                        d.height = e - D;
                        if (d.height < 0) {
                            d.y += d.height;
                            d.height = -d.height;
                            q = false
                        }
                    } else {
                        d.x = D < e ? D : e;
                        d.width = Math.abs(D - e);
                        d.y = p;
                        d.height = F.columnWidth
                    }
                    var v = D - e;
                    if (isNaN(v)) {
                        continue
                    }
                    v = Math.abs(v);
                    if (t[I] == undefined) {
                        if (!s) {
                            t[I] = this.renderer.rect(d.x, d.y, f.vertical ? d.width : 0, f.vertical ? 0 : d.height)
                        } else {
                            this._columnAsPieSlice(t, I, f.rect, s, d)
                        }
                        this.renderer.attr(t[I], {
                            fill: o.fillColor,
                            "fill-opacity": J.opacity,
                            "stroke-opacity": J.opacity,
                            stroke: o.lineColor,
                            "stroke-width": J.stroke,
                            "stroke-dasharray": J.dashStyle
                        })
                    }
                    if (v < 1 && c != 1) {
                        this.renderer.attr(t[I], {
                            display: "none"
                        })
                    } else {
                        this.renderer.attr(t[I], {
                            display: "block"
                        })
                    }
                    if (s) {
                        var k = this._columnAsPieSlice(t, I, f.rect, s, d);
                        var o = this._getColors(L, C, undefined, "radialGradient", k.outerRadius);
                        this.renderer.attr(t[I], {
                            fill: o.fillColor,
                            "fill-opacity": J.opacity,
                            "stroke-opacity": J.opacity,
                            stroke: o.lineColor,
                            "stroke-width": J.stroke,
                            "stroke-dasharray": J.dashStyle
                        })
                    } else {
                        if (f.vertical == true) {
                            this.renderer.attr(t[I], {
                                x: d.x,
                                y: d.y,
                                height: v
                            })
                        } else {
                            this.renderer.attr(t[I], {
                                x: d.x,
                                y: d.y,
                                width: v
                            })
                        }
                    }
                    this.renderer.removeElement(w[I]);
                    if (!B || (v == 0 && c < 1)) {
                        continue
                    }
                    w[I] = this._showLabel(L, C, I, d, undefined, undefined, false, false, q);
                    if (c == 1) {
                        this._installHandlers(t[I], "column", L, C, I)
                    }
                }
            }
        },
        _renderCandleStickSeries: function(f, d, v) {
            var n = this;
            var j = n.seriesGroups[f];
            if (!j.series || j.series.length == 0) {
                return
            }
            var e = j.orientation == "horizontal";
            var z = d;
            if (e) {
                z = {
                    x: d.y,
                    y: d.x,
                    width: d.height,
                    height: d.width
                }
            }
            var o = n._calcGroupOffsets(f, z);
            if (!o || o.xoffsets.length == 0) {
                return
            }
            var A = z.width;
            var l;
            if (j.polar || j.spider) {
                l = n._getPolarAxisCoords(f, z);
                A = 2 * l.r
            }
            var i = n._alignValuesWithTicks(f);
            var h = n._getGroupGradientType(f);
            for (var q = 0; q < j.series.length; q++) {
                if (!this._isSerieVisible(f, q)) {
                    continue
                }
                var w = n._getSerieSettings(f, q);
                var m = j.series[q];
                var k = a.isFunction(m.colorFunction) ? undefined : n._getColors(f, q, NaN, h);
                var p = {
                    rect: d,
                    inverse: e,
                    groupIndex: f,
                    seriesIndex: q,
                    symbolType: m.symbolType,
                    symbolSize: m.symbolSize,
                    "fill-opacity": w.opacity,
                    "stroke-opacity": w.opacity,
                    "stroke-width": w.stroke,
                    "stroke-dasharray": w.dashStyle,
                    gradientType: h,
                    colors: k,
                    renderData: o,
                    polarAxisCoords: l,
                    columnInfo: n._calcColumnWidth(f, o.xoffsets, e ? z.height : z.width),
                    isOHLC: v,
                    items: [],
                    self: n
                };
                var t = n._getAnimProps(f, q);
                var c = t.enabled && !n._isToggleRefresh && o.xoffsets.length < 5000 ? t.duration : 0;
                n._animCandleStick(p, 0);
                var u;
                n._enqueueAnimation("series", undefined, undefined, c, function(C, s, B) {
                    n._animCandleStick(s, B)
                }, p)
            }
        },
        _animCandleStick: function(t, c) {
            var s = ["Open", "Low", "Close", "High"];
            var e = t.columnInfo.width;
            var n = e;
            if (t.columnInfo.isMin) {
                n = e * (1 + t.columnInfo.columnGap)
            }
            var h = t.self.seriesGroups[t.groupIndex];
            var w = t.renderData.xoffsets;
            var D = -1;
            var o = Math.abs(w.data[w.last] - w.data[w.first]);
            o *= c;
            for (var A = w.first; A <= w.last; A++) {
                var m = w.data[A];
                if (isNaN(m)) {
                    continue
                }
                if (D != -1 && Math.abs(m - D) < n && h.skipOverlappingPoints != false) {
                    continue
                }
                var B = Math.abs(w.data[A] - w.data[w.first]);
                if (B > o) {
                    break
                }
                D = m;
                var C = t.items[A] = t.items[A] || {};
                for (var z in s) {
                    var E = t.self._getDataValueAsNumber(A, h.series[t.seriesIndex]["dataField" + s[z]], t.groupIndex);
                    if (isNaN(E)) {
                        break
                    }
                    var l = t.renderData.offsets[t.seriesIndex][A][s[z]];
                    if (isNaN(l)) {
                        break
                    }
                    C[s[z]] = l
                }
                m += t.inverse ? t.rect.y : t.rect.x;
                if (t.polarAxisCoords) {
                    var u = this._toPolarCoord(t.polarAxisCoords, this._plotRect, m, l);
                    m = u.x;
                    l = u.y
                }
                m = a.jqx._ptrnd(m);
                for (var f in s) {
                    C[f] = a.jqx._ptrnd(C[f])
                }
                var k = t.colors;
                if (!k) {
                    k = t.self._getColors(t.groupIndex, t.seriesIndex, A, t.gradientType)
                }
                if (!t.isOHLC) {
                    var v = C.lineElement;
                    if (!v) {
                        v = t.inverse ? this.renderer.line(C.Low, m, C.High, m) : this.renderer.line(m, C.Low, m, C.High);
                        this.renderer.attr(v, {
                            fill: k.fillColor,
                            "fill-opacity": t["fill-opacity"],
                            "stroke-opacity": t["fill-opacity"],
                            stroke: k.lineColor,
                            "stroke-width": t["stroke-width"],
                            "stroke-dasharray": t["stroke-dasharray"]
                        });
                        C.lineElement = v
                    }
                    var q = C.stickElement;
                    m -= e / 2;
                    if (!q) {
                        var d = k.fillColor;
                        if (C.Close <= C.Open && k.fillColorAlt) {
                            d = k.fillColorAlt
                        }
                        q = t.inverse ? this.renderer.rect(Math.min(C.Open, C.Close), m, Math.abs(C.Close - C.Open), e) : this.renderer.rect(m, Math.min(C.Open, C.Close), e, Math.abs(C.Close - C.Open));
                        this.renderer.attr(q, {
                            fill: d,
                            "fill-opacity": t["fill-opacity"],
                            "stroke-opacity": t["fill-opacity"],
                            stroke: k.lineColor,
                            "stroke-width": t["stroke-width"],
                            "stroke-dasharray": t["stroke-dasharray"]
                        });
                        C.stickElement = q
                    }
                    if (c == 1) {
                        this._installHandlers(q, "column", t.groupIndex, t.seriesIndex, A)
                    }
                } else {
                    var p = "M" + m + "," + C.Low + " L" + m + "," + C.High + " M" + (m - e / 2) + "," + C.Open + " L" + m + "," + C.Open + " M" + (m + e / 2) + "," + C.Close + " L" + m + "," + C.Close;
                    if (t.inverse) {
                        p = "M" + C.Low + "," + m + " L" + C.High + "," + m + " M" + C.Open + "," + (m - e / 2) + " L" + C.Open + "," + m + " M" + C.Close + "," + m + " L" + C.Close + "," + (m + e / 2)
                    }
                    var v = C.lineElement;
                    if (!v) {
                        v = this.renderer.path(p, {});
                        this.renderer.attr(v, {
                            fill: k.fillColor,
                            "fill-opacity": t["fill-opacity"],
                            "stroke-opacity": t["fill-opacity"],
                            stroke: k.lineColor,
                            "stroke-width": t["stroke-width"],
                            "stroke-dasharray": t["stroke-dasharray"]
                        });
                        C.lineElement = v
                    }
                    if (c == 1) {
                        this._installHandlers(v, "column", t.groupIndex, t.seriesIndex, A)
                    }
                }
            }
        },
        _renderScatterSeries: function(e, D, F) {
            var u = this.seriesGroups[e];
            if (!u.series || u.series.length == 0) {
                return
            }
            var f = u.type.indexOf("bubble") != -1;
            var v = u.orientation == "horizontal";
            var n = D;
            if (v) {
                n = {
                    x: D.y,
                    y: D.x,
                    width: D.height,
                    height: D.width
                }
            }
            var o = this._calcGroupOffsets(e, n);
            if (!o || o.xoffsets.length == 0) {
                return
            }
            var N = n.width;
            var c;
            if (u.polar || u.spider) {
                c = this._getPolarAxisCoords(e, n);
                N = 2 * c.r
            }
            var V = this._alignValuesWithTicks(e);
            var t = this._getGroupGradientType(e);
            if (!F) {
                F = "to"
            }
            for (var h = 0; h < u.series.length; h++) {
                var T = this._getSerieSettings(e, h);
                var K = u.series[h];
                var A = K.dataField;
                var m = a.isFunction(K.colorFunction);
                var L = this._getColors(e, h, NaN, t);
                var U = NaN,
                    z = NaN;
                if (f) {
                    for (var S = o.xoffsets.first; S <= o.xoffsets.last; S++) {
                        var C = this._getDataValueAsNumber(S, (K.radiusDataField || K.sizeDataField), e);
                        if (typeof(C) != "number") {
                            throw "Invalid radiusDataField value at [" + S + "]"
                        }
                        if (!isNaN(C)) {
                            if (isNaN(U) || C < U) {
                                U = C
                            }
                            if (isNaN(z) || C > z) {
                                z = C
                            }
                        }
                    }
                }
                var k = K.minRadius || K.minSymbolSize;
                if (isNaN(k)) {
                    k = N / 50
                }
                var E = K.maxRadius || K.maxSymbolSize;
                if (isNaN(E)) {
                    E = N / 25
                }
                if (k > E) {
                    E = k
                }
                var M = K.radius;
                if (isNaN(M) && !isNaN(K.symbolSize)) {
                    M = (K.symbolType == "circle") ? K.symbolSize / 2 : K.symbolSize
                } else {
                    M = 5
                }
                var G = this._getAnimProps(e, h);
                var B = G.enabled && !this._isToggleRefresh && o.xoffsets.length < 5000 ? G.duration : 0;
                var w = {
                    groupIndex: e,
                    seriesIndex: h,
                    symbolType: K.symbolType,
                    symbolSize: K.symbolSize,
                    "fill-opacity": T.opacity,
                    "stroke-opacity": T.opacity,
                    "stroke-width": T.stroke,
                    "stroke-dasharray": T.dashStyle,
                    items: [],
                    polarAxisCoords: c
                };
                for (var S = o.xoffsets.first; S <= o.xoffsets.last; S++) {
                    var C = this._getDataValueAsNumber(S, A, e);
                    if (typeof(C) != "number") {
                        continue
                    }
                    var J = o.xoffsets.data[S];
                    var H = o.xoffsets.xvalues[S];
                    var I = o.offsets[h][S][F];
                    if (isNaN(J) || isNaN(I)) {
                        continue
                    }
                    if (v) {
                        var Q = J;
                        J = I;
                        I = Q + D.y
                    } else {
                        J += D.x
                    }
                    var O = M;
                    if (f) {
                        var p = this._getDataValueAsNumber(S, (K.radiusDataField || K.sizeDataField), e);
                        if (typeof(p) != "number") {
                            continue
                        }
                        O = k + (E - k) * (p - U) / Math.max(1, z - U);
                        if (isNaN(O)) {
                            O = k
                        }
                    }
                    var l = NaN,
                        P = NaN;
                    var q = 0;
                    if (H != undefined && this._elementRenderInfo && this._elementRenderInfo.length > e) {
                        var d = this._elementRenderInfo[e].series[h][H];
                        if (d && !isNaN(d.to)) {
                            l = d.to;
                            P = d.xoffset;
                            q = M;
                            if (v) {
                                var Q = P;
                                P = l;
                                l = Q + D.y
                            } else {
                                P += D.x
                            }
                            if (f) {
                                q = k + (E - k) * (d.valueRadius - U) / Math.max(1, z - U);
                                if (isNaN(q)) {
                                    q = k
                                }
                            }
                        }
                    }
                    if (m) {
                        L = this._getColors(e, h, S, t)
                    }
                    w.items.push({
                        from: q,
                        to: O,
                        itemIndex: S,
                        fill: L.fillColor,
                        stroke: L.lineColor,
                        x: J,
                        y: I,
                        xFrom: P,
                        yFrom: l
                    })
                }
                this._animR(w, 0);
                var j = this;
                var R;
                this._enqueueAnimation("series", undefined, undefined, B, function(W, i, s) {
                    j._animR(i, s)
                }, w)
            }
        },
        _animR: function(o, h) {
            var j = o.items;
            var p = o.symbolType || "circle";
            var d = o.symbolSize;
            for (var f = 0; f < j.length; f++) {
                var n = j[f];
                var l = n.x;
                var k = n.y;
                var c = Math.round((n.to - n.from) * h + n.from);
                if (!isNaN(n.yFrom)) {
                    k = n.yFrom + (k - n.yFrom) * h
                }
                if (!isNaN(n.xFrom)) {
                    l = n.xFrom + (l - n.xFrom) * h
                }
                if (o.polarAxisCoords) {
                    var m = this._toPolarCoord(o.polarAxisCoords, this._plotRect, l, k);
                    l = m.x;
                    k = m.y
                }
                l = a.jqx._ptrnd(l);
                k = a.jqx._ptrnd(k);
                c = a.jqx._ptrnd(c);
                var e = n.element;
                if (p == "circle") {
                    if (!e) {
                        e = this.renderer.circle(l, k, c);
                        this.renderer.attr(e, {
                            fill: n.fill,
                            "fill-opacity": o["fill-opacity"],
                            "stroke-opacity": o["fill-opacity"],
                            stroke: n.stroke,
                            "stroke-width": o["stroke-width"],
                            "stroke-dasharray": o["stroke-dasharray"]
                        });
                        n.element = e
                    }
                    if (this._isVML) {
                        this.renderer.updateCircle(e, undefined, undefined, c)
                    } else {
                        this.renderer.attr(e, {
                            r: c,
                            cy: k,
                            cx: l
                        })
                    }
                } else {
                    if (e) {
                        this.renderer.removeElement(e)
                    }
                    n.element = e = this._drawSymbol(p, l, k, n.fill, o["fill-opacity"], n.stroke, o["stroke-opacity"] || o["fill-opacity"], o["stroke-width"], o["stroke-dasharray"], d || c)
                }
                if (n.labelElement) {
                    this.renderer.removeElement(n.labelElement)
                }
                n.labelElement = this._showLabel(o.groupIndex, o.seriesIndex, n.itemIndex, {
                    x: l - c,
                    y: k - c,
                    width: 2 * c,
                    height: 2 * c
                });
                if (h >= 1) {
                    this._installHandlers(e, "circle", o.groupIndex, o.seriesIndex, n.itemIndex)
                }
            }
        },
        _showToolTip: function(G, E, l, e, d) {
            var i = this;
            var s = i._getCategoryAxis(l);
            if (i._ttEl && l == i._ttEl.gidx && e == i._ttEl.sidx && d == i._ttEl.iidx) {
                return
            }
            var o = i.seriesGroups[l];
            var f = o.series[e];
            var C = i.enableCrosshairs && !(o.polar || o.spider);
            if (i._pointMarker) {
                G = parseInt(i._pointMarker.x + 5);
                E = parseInt(i._pointMarker.y - 5)
            } else {
                C = false
            }
            var Q = C && i.showToolTips == false;
            G = a.jqx._ptrnd(G);
            E = a.jqx._ptrnd(E);
            var k = i._ttEl == undefined;
            if (o.showToolTips == false || f.showToolTips == false) {
                return
            }
            var u = f.toolTipFormatSettings || o.toolTipFormatSettings;
            var B = f.toolTipFormatFunction || o.toolTipFormatFunction || i.toolTipFormatFunction;
            var K = i._getColors(l, e, d);
            var A = i._getDataValue(d, s.dataField, l);
            if (s.dataField == undefined || s.dataField == "") {
                A = d
            }
            if (s.type == "date") {
                A = i._castAsDate(A)
            }
            var w = "";
            if (a.isFunction(B)) {
                var I = {};
                var q = 0;
                for (var j in f) {
                    if (j.indexOf("dataField") == 0) {
                        I[j.substring(9, j.length).toLowerCase()] = i._getDataValue(d, f[j], l);
                        q++
                    }
                }
                if (q == 0) {
                    I = i._getDataValue(d, undefined, l)
                } else {
                    if (q == 1) {
                        I = I[""]
                    }
                }
                w = B(I, d, f, o, A, s)
            } else {
                w = i._getFormattedValue(l, e, d, u, B);
                var L = s.toolTipFormatSettings || s.formatSettings;
                var P = s.toolTipFormatFunction || s.formatFunction;
                if (!P && !L && s.type == "date") {
                    P = this._getDefaultDTFormatFn(s.baseUnit || "day")
                }
                var m = i._formatValue(A, L, P);
                if (o.type != "pie" && o.type != "donut") {
                    var J = (s.displayText || s.dataField || "");
                    if (J.length > 0) {
                        w = J + ": " + m + "<br>" + w
                    } else {
                        w = m + "<br>" + w
                    }
                } else {
                    A = i._getDataValue(d, f.displayText || f.dataField, l);
                    m = i._formatValue(A, L, P);
                    w = m + ": " + w
                }
            }
            var N = f.toolTipClass || o.toolTipClass || this.toThemeProperty("jqx-chart-tooltip-text", null);
            var p = f.toolTipBackground || o.toolTipBackground || "#FFFFFF";
            var n = f.toolTipLineColor || o.toolTipLineColor || K.lineColor;
            if (!i._ttEl) {
                i._ttEl = {}
            }
            i._ttEl.sidx = e;
            i._ttEl.gidx = l;
            i._ttEl.iidx = d;
            rect = i.renderer.getRect();
            if (C) {
                var F = a.jqx._ptrnd(i._pointMarker.x);
                var D = a.jqx._ptrnd(i._pointMarker.y);
                if (i._ttEl.vLine && i._ttEl.hLine) {
                    i.renderer.attr(i._ttEl.vLine, {
                        x1: F,
                        x2: F
                    });
                    i.renderer.attr(i._ttEl.hLine, {
                        y1: D,
                        y2: D
                    })
                } else {
                    var v = i.crosshairsColor || i._defaultLineColor;
                    i._ttEl.vLine = i.renderer.line(F, i._plotRect.y, F, i._plotRect.y + i._plotRect.height, {
                        stroke: v,
                        "stroke-width": i.crosshairsLineWidth || 1,
                        "stroke-dasharray": i.crosshairsDashStyle || ""
                    });
                    i._ttEl.hLine = i.renderer.line(i._plotRect.x, D, i._plotRect.x + i._plotRect.width, D, {
                        stroke: v,
                        "stroke-width": i.crosshairsLineWidth || 1,
                        "stroke-dasharray": i.crosshairsDashStyle || ""
                    })
                }
            }
            if (!Q && i.showToolTips != false) {
                var O = !k ? i._ttEl.box : document.createElement("div");
                var H = {
                    left: 0,
                    top: 0
                };
                if (k) {
                    O.style.position = "absolute";
                    O.style.cursor = "default";
                    O.style.overflow = "hidden";
                    a(O).addClass("jqx-rc-all jqx-button");
                    a(O).css("z-index", 9999999);
                    a(document.body).append(O)
                }
                O.style.backgroundColor = p;
                O.style.borderColor = n;
                i._ttEl.box = O;
                i._ttEl.txt = w;
                var z = "<span class='" + N + "'>" + w + "</span>";
                var M = i._ttEl.tmp;
                if (k) {
                    i._ttEl.tmp = M = document.createElement("div");
                    M.style.position = "absolute";
                    M.style.cursor = "default";
                    M.style.overflow = "hidden";
                    M.style.display = "none";
                    M.style.zIndex = 999999;
                    M.style.backgroundColor = p;
                    M.style.borderColor = n;
                    a(M).addClass("jqx-rc-all jqx-button");
                    i.host.append(M)
                }
                a(M).html(z);
                if (!w || w.length == 0) {
                    a(O).fadeTo(0, 0);
                    return
                }
                var h = {
                    width: a(M).width(),
                    height: a(M).height()
                };
                h.width = h.width + 5;
                h.height = h.height + 6;
                G = Math.max(G, rect.x);
                E = Math.max(E - h.height, rect.y);
                if (h.width > rect.width || h.height > rect.height) {
                    return
                }
                if (G + H.left + h.width > rect.x + rect.width - 5) {
                    G = rect.x + rect.width - h.width - H.left - 5
                }
                if (E + H.top + h.height > rect.y + rect.height - 5) {
                    E = rect.y + rect.height - h.height - 5
                }
                var c = i.host.coord();
                if (k) {
                    a(O).fadeOut(0, 0);
                    O.style.left = H.left + G + c.left + "px";
                    O.style.top = H.top + E + c.top + "px"
                }
                a(O).html(z);
                a(O).clearQueue();
                a(O).animate({
                    left: H.left + G + c.left,
                    top: H.top + E + c.top,
                    opacity: 1
                }, 300, "easeInOutCirc");
                a(O).fadeTo(400, 1)
            }
        },
        _hideToolTip: function(c) {
            if (!this._ttEl) {
                return
            }
            if (this._ttEl.box) {
                if (c == 0) {
                    a(this._ttEl.box).hide()
                } else {
                    a(this._ttEl.box).fadeOut()
                }
            }
            this._hideCrosshairs();
            this._ttEl.gidx = undefined
        },
        _hideCrosshairs: function() {
            if (!this._ttEl) {
                return
            }
            if (this._ttEl.vLine) {
                this.renderer.removeElement(this._ttEl.vLine);
                this._ttEl.vLine = undefined
            }
            if (this._ttEl.hLine) {
                this.renderer.removeElement(this._ttEl.hLine);
                this._ttEl.hLine = undefined
            }
        },
        _showLabel: function(G, C, f, c, u, j, e, l, d, D) {
            var n = this.seriesGroups[G];
            var s = n.series[C];
            var A = {
                width: 0,
                height: 0
            };
            if (s.showLabels == false || (!s.showLabels && !n.showLabels)) {
                return e ? A : undefined
            }
            if (c.width < 0 || c.height < 0) {
                return e ? A : undefined
            }
            var B = s.labelClass || n.labelClass || this.toThemeProperty("jqx-chart-label-text", null);
            var i = s.labelAngle || s.labelsAngle || n.labelAngle || n.labelsAngle || 0;
            if (!isNaN(D)) {
                i = D
            }
            var k = s.labelOffset || s.labelsOffset || n.labelOffset || n.labelsOffset || {};
            var E = {
                x: k.x,
                y: k.y
            };
            if (isNaN(E.x)) {
                E.x = 0
            }
            if (isNaN(E.y)) {
                E.y = 0
            }
            u = u || s.labelsHorizontalAlignment || n.labelsHorizontalAlignment || "center";
            j = j || s.labelsVerticalAlignment || n.labelsVerticalAlignment || "center";
            var z = this._getFormattedValue(G, C, f, undefined, undefined, true);
            var t = c.width;
            var F = c.height;
            if (l == true && u != "center") {
                u = u == "right" ? "left" : "right"
            }
            if (d == true && j != "center" && j != "middle") {
                j = j == "top" ? "bottom" : "top";
                E.y *= -1
            }
            A = this.renderer.measureText(z, i, {
                "class": B
            });
            if (e) {
                return A
            }
            var q = 0;
            if (t > 0) {
                if (u == "" || u == "center") {
                    q += (t - A.width) / 2
                } else {
                    if (u == "right") {
                        q += (t - A.width)
                    }
                }
            }
            var o = 0;
            if (F > 0) {
                if (j == "" || j == "center") {
                    o += (F - A.height) / 2
                } else {
                    if (j == "bottom") {
                        o += (F - A.height)
                    }
                }
            }
            q += c.x + E.x;
            o += c.y + E.y;
            var p = this._plotRect;
            if (q <= p.x) {
                q = p.x + 2
            }
            if (o <= p.y) {
                o = p.y + 2
            }
            var m = {
                width: Math.max(A.width, 1),
                height: Math.max(A.height, 1)
            };
            if (o + m.height >= p.y + p.height) {
                o = p.y + p.height - m.height - 2
            }
            if (q + m.width >= p.x + p.width) {
                q = p.x + p.width - m.width - 2
            }
            var v = this.renderer.text(z, q, o, A.width, A.height, i, {
                "class": B
            }, false, "center", "center");
            this.renderer.attr(v, {
                "class": B
            });
            if (this._isVML) {
                this.renderer.removeElement(v);
                this.renderer.getContainer()[0].appendChild(v)
            }
            return v
        },
        _getAnimProps: function(k, h) {
            var f = this.seriesGroups[k];
            var d = !isNaN(h) ? f.series[h] : undefined;
            var c = this.enableAnimations == true;
            if (f.enableAnimations) {
                c = f.enableAnimations == true
            }
            if (d && d.enableAnimations) {
                c = d.enableAnimations == true
            }
            var j = this.animationDuration;
            if (isNaN(j)) {
                j = 1000
            }
            var e = f.animationDuration;
            if (!isNaN(e)) {
                j = e
            }
            if (d) {
                var i = d.animationDuration;
                if (!isNaN(i)) {
                    j = i
                }
            }
            if (j > 5000) {
                j = 1000
            }
            return {
                enabled: c,
                duration: j
            }
        },
        _isColorTransition: function(h, e, f, i) {
            if (i - 1 < f.xoffsets.first) {
                return false
            }
            var c = this._getColors(h, e, i, this._getGroupGradientType(h));
            var d = this._getColors(h, e, i - 1, this._getGroupGradientType(h));
            return (c.fillColor != d.fillColor)
        },
        _renderLineSeries: function(l, S) {
            var J = this.seriesGroups[l];
            if (!J.series || J.series.length == 0) {
                return
            }
            var u = J.type.indexOf("area") != -1;
            var M = J.type.indexOf("stacked") != -1;
            var e = M && J.type.indexOf("100") != -1;
            var ag = J.type.indexOf("spline") != -1;
            var v = J.type.indexOf("step") != -1;
            var Q = J.type.indexOf("range") != -1;
            var ah = J.polar == true || J.spider == true;
            if (ah) {
                v = false
            }
            if (v && ag) {
                return
            }
            var C = this._getDataLen(l);
            var ae = S.width / C;
            var al = J.orientation == "horizontal";
            var E = this._getCategoryAxis(l).flip == true;
            var B = S;
            if (al) {
                B = {
                    x: S.y,
                    y: S.x,
                    width: S.height,
                    height: S.width
                }
            }
            var F = this._calcGroupOffsets(l, B);
            if (!F || F.xoffsets.length == 0) {
                return
            }
            if (!this._linesRenderInfo) {
                this._linesRenderInfo = {}
            }
            this._linesRenderInfo[l] = {};
            for (var o = J.series.length - 1; o >= 0; o--) {
                var h = this._getSerieSettings(l, o);
                var aj = {
                    groupIndex: l,
                    serieIndex: o,
                    swapXY: al,
                    isArea: u,
                    isSpline: ag,
                    isRange: Q,
                    isPolar: ah,
                    settings: h,
                    segments: [],
                    pointsLength: 0
                };
                var k = this._isSerieVisible(l, o);
                if (!k) {
                    this._linesRenderInfo[l][o] = aj;
                    continue
                }
                var L = J.series[o];
                var A = a.isFunction(L.colorFunction);
                var W = F.xoffsets.first;
                var I = W;
                var P = this._getColors(l, o, NaN, this._getGroupGradientType(l));
                var ad = false;
                var w;
                do {
                    var Y = [];
                    var V = [];
                    var t = [];
                    var R = -1;
                    var q = 0,
                        p = 0;
                    var T = NaN;
                    var G = NaN;
                    var ak = NaN;
                    if (F.xoffsets.length < 1) {
                        continue
                    }
                    var U = this._getAnimProps(l, o);
                    var N = U.enabled && !this._isToggleRefresh && F.xoffsets.length < 10000 && this._isVML != true ? U.duration : 0;
                    var z = W;
                    w = false;
                    var d = this._getColors(l, o, W, this._getGroupGradientType(l));
                    for (var af = W; af <= F.xoffsets.last; af++) {
                        W = af;
                        var Z = F.xoffsets.data[af];
                        var X = F.xoffsets.xvalues[af];
                        if (isNaN(Z)) {
                            continue
                        }
                        Z = Math.max(Z, 1);
                        q = Z;
                        p = F.offsets[o][af].to;
                        var ac = F.offsets[o][af].from;
                        if (isNaN(p) || isNaN(ac)) {
                            if (L.emptyPointsDisplay == "connect") {
                                continue
                            } else {
                                if (L.emptyPointsDisplay == "zero") {
                                    if (isNaN(p)) {
                                        p = F.baseOffset
                                    }
                                    if (isNaN(ac)) {
                                        ac = F.baseOffset
                                    }
                                } else {
                                    w = true;
                                    break
                                }
                            }
                        }
                        if (A && this._isColorTransition(l, o, F, W)) {
                            if (Y.length > 1) {
                                W--;
                                break
                            }
                        }
                        if (this._elementRenderInfo && this._elementRenderInfo.length > l && this._elementRenderInfo[l].series.length > o) {
                            var f = this._elementRenderInfo[l].series[o][X];
                            var ak = a.jqx._ptrnd(f ? f.to : undefined);
                            var K = a.jqx._ptrnd(B.x + (f ? f.xoffset : undefined));
                            t.push(al ? {
                                y: K,
                                x: ak,
                                index: af
                            } : {
                                x: K,
                                y: ak,
                                index: af
                            })
                        }
                        I = af;
                        if (h.stroke < 2) {
                            if (p - B.y <= 1) {
                                p = B.y + 1
                            }
                            if (ac - B.y <= 1) {
                                ac = B.y + 1
                            }
                            if (B.y + B.height - p <= 1) {
                                p = B.y + B.height - 1
                            }
                            if (B.y + B.height - p <= 1) {
                                ac = B.y + B.height - 1
                            }
                        }
                        if (!u && e) {
                            if (p <= B.y) {
                                p = B.y + 1
                            }
                            if (p >= B.y + B.height) {
                                p = B.y + B.height - 1
                            }
                            if (ac <= B.y) {
                                ac = B.y + 1
                            }
                            if (ac >= B.y + B.height) {
                                ac = B.y + B.height - 1
                            }
                        }
                        Z = Math.max(Z, 1);
                        q = Z + B.x;
                        if (v && !isNaN(T) && !isNaN(G)) {
                            if (G != p) {
                                Y.push(al ? {
                                    y: q,
                                    x: a.jqx._ptrnd(G)
                                } : {
                                    x: q,
                                    y: a.jqx._ptrnd(G)
                                })
                            }
                        }
                        Y.push(al ? {
                            y: q,
                            x: a.jqx._ptrnd(p),
                            index: af
                        } : {
                            x: q,
                            y: a.jqx._ptrnd(p),
                            index: af
                        });
                        V.push(al ? {
                            y: q,
                            x: a.jqx._ptrnd(ac),
                            index: af
                        } : {
                            x: q,
                            y: a.jqx._ptrnd(ac),
                            index: af
                        });
                        T = q;
                        G = p;
                        if (isNaN(ak)) {
                            ak = p
                        }
                    }
                    if (Y.length == 0) {
                        W++;
                        continue
                    }
                    var H = Y[Y.length - 1].index;
                    if (A) {
                        P = this._getColors(l, o, H, this._getGroupGradientType(l))
                    }
                    var m = B.x + F.xoffsets.data[z];
                    var ab = B.x + F.xoffsets.data[I];
                    if (u && J.alignEndPointsWithIntervals == true) {
                        var D = E ? -1 : 1;
                        if (m > B.x) {
                            m = B.x
                        }
                        if (ab < B.x + B.width) {
                            ab = B.x + B.width
                        }
                        if (E) {
                            var aa = m;
                            m = ab;
                            ab = aa
                        }
                    }
                    ab = a.jqx._ptrnd(ab);
                    m = a.jqx._ptrnd(m);
                    var n = F.baseOffset;
                    ak = a.jqx._ptrnd(ak);
                    var j = a.jqx._ptrnd(p) || n;
                    if (Q) {
                        Y = Y.concat(V.reverse())
                    }
                    aj.pointsLength += Y.length;
                    var c = {
                        lastItemIndex: H,
                        colorSettings: P,
                        pointsArray: Y,
                        pointsStart: t,
                        left: m,
                        right: ab,
                        pyStart: ak,
                        pyEnd: j,
                        yBase: n,
                        labelElements: [],
                        symbolElements: []
                    };
                    aj.segments.push(c)
                } while (W < F.xoffsets.length - 1 || w);
                this._linesRenderInfo[l][o] = aj
            }
            var O = this._linesRenderInfo[l];
            var ai = [];
            for (var af in O) {
                ai.push(O[af])
            }
            ai = ai.sort(function(am, i) {
                return am.serieIndex - i.serieIndex
            });
            if (u && M) {
                ai.reverse()
            }
            for (var af = 0; af < ai.length; af++) {
                var aj = ai[af];
                this._animateLine(aj, N == 0 ? 1 : 0);
                var s = this;
                this._enqueueAnimation("series", undefined, undefined, N, function(am, i, an) {
                    s._animateLine(i, an)
                }, aj)
            }
        },
        _animateLine: function(A, c) {
            var E = A.settings;
            var h = A.groupIndex;
            var j = A.serieIndex;
            var l = this.seriesGroups[h];
            var u = l.series[j];
            var z = this._getSymbol(h, j);
            var q = u.showLabels == true || (l.showLabels && u.showLabels != false);
            var s = 0;
            for (var e = 0; e < A.segments.length; e++) {
                var w = A.segments[e];
                var B = this._calculateLine(h, A.pointsLength, s, w.pointsArray, w.pointsStart, w.yBase, c, A.isArea, A.swapXY);
                s += w.pointsArray.length;
                if (B == "") {
                    continue
                }
                var t = B.split(" ");
                var C = t.length;
                var k = B;
                if (k != "") {
                    k = this._buildLineCmd(B, A.isRange, w.left, w.right, w.pyStart, w.pyEnd, w.yBase, A.isArea, A.isPolar, A.isSpline, A.swapXY)
                } else {
                    k = "M 0 0"
                }
                var n = w.colorSettings;
                if (!w.pathElement) {
                    w.pathElement = this.renderer.path(k, {
                        "stroke-width": E.stroke,
                        stroke: n.lineColor,
                        "stroke-opacity": E.opacity,
                        "fill-opacity": E.opacity,
                        "stroke-dasharray": E.dashStyle,
                        fill: A.isArea ? n.fillColor : "none"
                    });
                    this._installHandlers(w.pathElement, "path", h, j, w.lastItemIndex)
                } else {
                    this.renderer.attr(w.pathElement, {
                        d: k
                    })
                }
                if (w.labelElements) {
                    for (var D = 0; D < w.labelElements.length; D++) {
                        this.renderer.removeElement(w.labelElements[D])
                    }
                    w.labelElements = []
                }
                if (w.symbolElements) {
                    for (var D = 0; D < w.symbolElements.length; D++) {
                        this.renderer.removeElement(w.symbolElements[D])
                    }
                    w.symbolElements = []
                }
                if (w.pointsArray.length == t.length) {
                    if (z != "none" || q) {
                        var F = u.symbolSize;
                        for (var D = 0; D < t.length; D++) {
                            var v = t[D].split(",");
                            v = {
                                x: parseFloat(v[0]),
                                y: parseFloat(v[1])
                            };
                            if (z != "none") {
                                var p = this._getColors(h, j, w.pointsArray[D].index, this._getGroupGradientType(h));
                                var f = this._drawSymbol(z, v.x, v.y, p.fillColorSymbol, E.opacity, p.lineColorSymbol, E.opacity, 1, undefined, F);
                                w.symbolElements.push(f)
                            }
                            if (q) {
                                var m = (D > 0 ? t[D - 1] : t[D]).split(",");
                                m = {
                                    x: parseFloat(m[0]),
                                    y: parseFloat(m[1])
                                };
                                var o = (D < t.length - 1 ? t[D + 1] : t[D]).split(",");
                                o = {
                                    x: parseFloat(o[0]),
                                    y: parseFloat(o[1])
                                };
                                v = this._adjustLineLabelPosition(h, j, w.pointsArray[D].index, v, m, o);
                                var d = this._showLabel(h, j, w.pointsArray[D].index, {
                                    x: v.x,
                                    y: v.y,
                                    width: 0,
                                    height: 0
                                });
                                w.labelElements.push(d)
                            }
                        }
                    }
                }
                if (c == 1 && z != "none") {
                    for (var D = 0; D < w.symbolElements.length; D++) {
                        if (!w.pointsArray[D].index) {
                            continue
                        }
                        this._installHandlers(w.symbolElements[D], "symbol", h, j, w.pointsArray[D].index)
                    }
                }
            }
        },
        _adjustLineLabelPosition: function(k, i, e, j, h, f) {
            var c = this._showLabel(k, i, e, {
                width: 0,
                height: 0
            }, "", "", true);
            var d = {
                x: 0,
                y: 0
            };
            if (j.y == h.y && j.x == h.x) {
                if (f.y < j.y) {
                    d = {
                        x: j.x,
                        y: j.y + c.height
                    }
                } else {
                    d = {
                        x: j.x,
                        y: j.y - c.height
                    }
                }
            } else {
                if (j.y == f.y && j.x == f.x) {
                    if (h.y < j.y) {
                        d = {
                            x: j.x,
                            y: j.y + c.height
                        }
                    } else {
                        d = {
                            x: j.x,
                            y: j.y - c.height
                        }
                    }
                }
            }
            if (j.y > h.y && j.y > f.y) {
                d = {
                    x: j.x,
                    y: j.y + c.height
                }
            } else {
                d = {
                    x: j.x,
                    y: j.y - c.height
                }
            }
            return d
        },
        _calculateLine: function(j, z, q, p, o, h, f, B, d) {
            var A = this.seriesGroups[j];
            var n;
            if (A.polar == true || A.spider == true) {
                n = this._getPolarAxisCoords(j, this._plotRect)
            }
            var u = "";
            var v = p.length;
            if (!B && o.length == 0) {
                var t = z * f;
                v = t - q
            }
            var k = NaN;
            for (var w = 0; w < v + 1 && w < p.length; w++) {
                if (w > 0) {
                    u += " "
                }
                var l = p[w].y;
                var m = p[w].x;
                var c = !B ? l : h;
                var e = m;
                if (o && o.length > w) {
                    c = o[w].y;
                    e = o[w].x;
                    if (isNaN(c) || isNaN(e)) {
                        c = l;
                        e = m
                    }
                }
                k = e;
                if (v <= p.length && w > 0 && w == v) {
                    e = p[w - 1].x;
                    c = p[w - 1].y
                }
                if (d) {
                    m = a.jqx._ptrnd((m - c) * (B ? f : 1) + c);
                    l = a.jqx._ptrnd(l)
                } else {
                    m = a.jqx._ptrnd((m - e) * f + e);
                    l = a.jqx._ptrnd((l - c) * f + c)
                }
                if (n) {
                    var s = this._toPolarCoord(n, this._plotRect, m, l);
                    m = s.x;
                    l = s.y
                }
                u += m + "," + l;
                if (p.length == 1 && !B) {
                    u += " " + (m + 2) + "," + (l + 2)
                }
            }
            return u
        },
        _buildLineCmd: function(m, k, h, q, p, c, s, o, t, e, l) {
            var f = m;
            var d = l ? s + "," + h : h + "," + s;
            var j = l ? s + "," + q : q + "," + s;
            if (o && !t && !k) {
                f = d + " " + m + " " + j
            }
            if (e) {
                f = this._getBezierPoints(f)
            }
            var n = f.split(" ");
            var i = n[0].replace("M", "");
            if (o && !t) {
                if (!k) {
                    f = "M " + d + " L " + i + " " + f
                } else {
                    f = "M " + i + " L " + i + (e ? "" : (" L " + i + " ")) + f
                }
            } else {
                if (!e) {
                    f = "M " + i + " L " + i + " " + f
                }
            }
            if (t) {
                f += " Z"
            }
            return f
        },
        _getSerieSettings: function(j, c) {
            var i = this.seriesGroups[j];
            var h = i.type.indexOf("area") != -1;
            var f = i.type.indexOf("line") != -1;
            var d = i.series[c];
            var l = d.dashStyle || i.dashStyle || "";
            var e = d.opacity || i.opacity;
            if (isNaN(e) || e < 0 || e > 1) {
                e = 1
            }
            var k = d.lineWidth;
            if (isNaN(k) && k != "auto") {
                k = i.lineWidth
            }
            if (k == "auto" || isNaN(k) || k < 0 || k > 15) {
                if (h) {
                    k = 2
                } else {
                    if (f) {
                        k = 3
                    } else {
                        k = 1
                    }
                }
            }
            return {
                stroke: k,
                opacity: e,
                dashStyle: l
            }
        },
        _getColors: function(w, s, e, f, c) {
            var m = this.seriesGroups[w];
            var q = m.series[s];
            var d = q.useGradient;
            if (d == undefined) {
                d = q.useGradientColors
            }
            if (d == undefined) {
                d = m.useGradient
            }
            if (d == undefined) {
                d = m.useGradientColors
            }
            if (d == undefined) {
                d = true
            }
            var n = this._getSeriesColors(w, s, e);
            if (!n.fillColor) {
                n.fillColor = color;
                n.fillColorSelected = a.jqx.adjustColor(color, 1.1);
                n.fillColorAlt = a.jqx.adjustColor(color, 4);
                n.fillColorAltSelected = a.jqx.adjustColor(color, 3);
                n.lineColor = n.symbolColor = a.jqx.adjustColor(color, 0.9);
                n.lineColorSelected = n.symbolColorSelected = a.jqx.adjustColor(color, 0.9)
            }
            var k = [
                [0, 1.4],
                [100, 1]
            ];
            var h = [
                [0, 1],
                [25, 1.1],
                [50, 1.4],
                [100, 1]
            ];
            var p = [
                [0, 1.3],
                [90, 1.2],
                [100, 1]
            ];
            var l = NaN;
            if (!isNaN(c)) {
                l = c == 2 ? k : h
            }
            if (d) {
                var t = {};
                for (var u in n) {
                    t[u] = n[u]
                }
                n = t;
                if (f == "verticalLinearGradient" || f == "horizontalLinearGradient") {
                    var j = f == "verticalLinearGradient" ? l || k : l || h;
                    var o = ["fillColor", "fillColorSelected", "fillColorAlt", "fillColorAltSelected"];
                    for (var u in o) {
                        n[o[u]] = this.renderer._toLinearGradient(n[o[u]], f == "verticalLinearGradient", j)
                    }
                } else {
                    if (f == "radialGradient") {
                        var v;
                        var l = k;
                        if ((m.type == "pie" || m.type == "donut" || m.polar) && e != undefined && this._renderData[w] && this._renderData[w].offsets[s]) {
                            v = this._renderData[w].offsets[s][e];
                            l = p
                        }
                        n.fillColor = this.renderer._toRadialGradient(n.fillColor, l, v);
                        n.fillColorSelected = this.renderer._toRadialGradient(n.fillColorSelected, l, v)
                    }
                }
            }
            return n
        },
        _installHandlers: function(d, h, j, i, e) {
            if (!this.enableEvents) {
                return false
            }
            var k = this;
            var f = this.seriesGroups[j];
            var l = this.seriesGroups[j].series[i];
            var c = f.type.indexOf("line") != -1 || f.type.indexOf("area") != -1;
            if (!c) {
                this.renderer.addHandler(d, "mousemove", function(o) {
                    var n = k._selected;
                    if (n && n.isLineType && n.linesUnselectMode == "click" && !(n.group == j && n.series == i)) {
                        return
                    }
                    o.preventDefault();
                    var m = o.pageX || o.clientX || o.screenX;
                    var q = o.pageY || o.clientY || o.screenY;
                    var p = k.host.offset();
                    m -= p.left;
                    q -= p.top;
                    if (k._mouseX == m && k._mouseY == q) {
                        return
                    }
                    if (k._ttEl) {
                        if (k._ttEl.gidx == j && k._ttEl.sidx == i && k._ttEl.iidx == e) {
                            return
                        }
                    }
                    k._startTooltipTimer(j, i, e)
                });
                this.renderer.addHandler(d, "mouseout", function(n) {
                    if (!isNaN(k._lastClickTs) && (new Date()).valueOf() - k._lastClickTs < 100) {
                        return
                    }
                    n.preventDefault();
                    if (e != undefined) {
                        k._cancelTooltipTimer()
                    }
                    if (c) {
                        return
                    }
                    var m = k._selected;
                    if (m && m.isLineType && m.linesUnselectMode == "click" && !(m.group == j && m.series == i)) {
                        return
                    }
                    k._unselect()
                })
            }
            this.renderer.addHandler(d, "mouseover", function(n) {
                n.preventDefault();
                var m = k._selected;
                if (m && m.isLineType && m.linesUnselectMode == "click" && !(m.group == j && m.series == i)) {
                    return
                }
                k._select(d, h, j, i, e, e)
            });
            this.renderer.addHandler(d, "click", function(m) {
                clearTimeout(k._hostClickTimer);
                k._lastClickTs = (new Date()).valueOf();
                if (c && (h != "symbol" && h != "pointMarker")) {
                    return
                }
                if (f.type.indexOf("column") != -1) {
                    k._unselect()
                }
                if (isNaN(e)) {
                    return
                }
                k._raiseItemEvent("click", f, l, e)
            })
        },
        _getHorizontalOffset: function(C, u, l, k) {
            var d = this._plotRect;
            var j = this._getDataLen(C);
            if (j == 0) {
                return {
                    index: undefined,
                    value: l
                }
            }
            var q = this._calcGroupOffsets(C, this._plotRect);
            if (q.xoffsets.length == 0) {
                return {
                    index: undefined,
                    value: undefined
                }
            }
            var o = l;
            var n = k;
            var A = this.seriesGroups[C];
            var m;
            if (A.polar || A.spider) {
                m = this._getPolarAxisCoords(C, d)
            }
            if (A.orientation == "horizontal" && !m) {
                var B = o;
                o = n;
                n = B
            }
            var f = this._getCategoryAxis(C).flip == true;
            var c, p, z, h;
            for (var v = q.xoffsets.first; v <= q.xoffsets.last; v++) {
                var w = q.xoffsets.data[v];
                var e = q.offsets[u][v].to;
                var s = 0;
                if (m) {
                    var t = this._toPolarCoord(m, d, w + d.x, e);
                    w = t.x;
                    e = t.y;
                    s = a.jqx._ptdist(o, n, w, e)
                } else {
                    w += d.x;
                    e += d.y;
                    s = Math.abs(o - w)
                }
                if (isNaN(c) || c > s) {
                    c = s;
                    p = v;
                    z = w;
                    h = e
                }
            }
            return {
                index: p,
                value: q.xoffsets.data[p],
                polarAxisCoords: m,
                x: z,
                y: h
            }
        },
        onmousemove: function(m, l) {
            if (this._mouseX == m && this._mouseY == l) {
                return
            }
            this._mouseX = m;
            this._mouseY = l;
            if (!this._selected) {
                return
            }
            var D = this._selected.group;
            var u = this._selected.series;
            var A = this.seriesGroups[D];
            var p = A.series[u];
            var c = this._plotRect;
            if (this.renderer) {
                c = this.renderer.getRect();
                c.x += 5;
                c.y += 5;
                c.width -= 10;
                c.height -= 10
            }
            if (m < c.x || m > c.x + c.width || l < c.y || l > c.y + c.height) {
                this._hideToolTip();
                this._unselect();
                return
            }
            var f = A.orientation == "horizontal";
            var c = this._plotRect;
            if (A.type.indexOf("line") != -1 || A.type.indexOf("area") != -1) {
                var j = this._getHorizontalOffset(D, this._selected.series, m, l);
                var z = j.index;
                if (z == undefined) {
                    return
                }
                if (this._selected.item != z) {
                    var t = this._linesRenderInfo[D][u].segments;
                    var v = 0;
                    while (z > t[v].lastItemIndex) {
                        v++;
                        if (v >= t.length) {
                            return
                        }
                    }
                    var d = t[v].pathElement;
                    var E = t[v].lastItemIndex;
                    this._unselect(false);
                    this._select(d, "path", D, u, z, E)
                } else {
                    return
                }
                var o = this._getSymbol(this._selected.group, this._selected.series);
                if (o == "none") {
                    o = "circle"
                }
                var q = this._calcGroupOffsets(D, c);
                var e = q.offsets[this._selected.series][z].to;
                var w = e;
                if (A.type.indexOf("range") != -1) {
                    w = q.offsets[this._selected.series][z].from
                }
                var n = f ? m : l;
                if (!isNaN(w) && Math.abs(n - w) < Math.abs(n - e)) {
                    l = w
                } else {
                    l = e
                }
                if (isNaN(l)) {
                    return
                }
                m = j.value;
                if (f) {
                    var B = m;
                    m = l;
                    l = B + c.y
                } else {
                    m += c.x
                }
                if (j.polarAxisCoords) {
                    m = j.x;
                    l = j.y
                }
                l = a.jqx._ptrnd(l);
                m = a.jqx._ptrnd(m);
                if (this._pointMarker && this._pointMarker.element) {
                    this.renderer.removeElement(this._pointMarker.element);
                    this._pointMarker.element = undefined
                }
                if (isNaN(m) || isNaN(l)) {
                    return
                }
                var k = this._getSeriesColors(D, u, z);
                var h = p.opacity;
                if (isNaN(h) || h < 0 || h > 1) {
                    h = A.opacity
                }
                if (isNaN(h) || h < 0 || h > 1) {
                    h = 1
                }
                var C = p.symbolSizeSelected;
                if (isNaN(C)) {
                    C = p.symbolSize
                }
                if (isNaN(C) || C > 50 || C < 0) {
                    C = A.symbolSize
                }
                if (isNaN(C) || C > 50 || C < 0) {
                    C = 6
                }
                this._pointMarker = {
                    type: o,
                    x: m,
                    y: l,
                    gidx: D,
                    sidx: u,
                    iidx: z
                };
                this._pointMarker.element = this._drawSymbol(o, m, l, k.fillColorSymbolSelected, h, k.lineColorSymbolSelected, h, 1, undefined, C);
                this._installHandlers(this._pointMarker.element, "pointMarker", D, u, z);
                this._startTooltipTimer(D, this._selected.series, z)
            }
        },
        _drawSymbol: function(k, n, l, d, o, m, h, i, c, q) {
            var f;
            var j = q || 6;
            var e = j / 2;
            switch (k) {
                case "none":
                    return undefined;
                case "circle":
                    f = this.renderer.circle(n, l, j / 2);
                    break;
                case "square":
                    j = j - 1;
                    e = j / 2;
                    f = this.renderer.rect(n - e, l - e, j, j);
                    break;
                case "diamond":
                    var p = "M " + (n - e) + "," + (l) + " L" + (n) + "," + (l - e) + " L" + (n + e) + "," + (l) + " L" + (n) + "," + (l + e) + " Z";
                    f = this.renderer.path(p);
                    break;
                case "triangle_up":
                case "triangle":
                    var p = "M " + (n - e) + "," + (l + e) + " L " + (n + e) + "," + (l + e) + " L " + (n) + "," + (l - e) + " Z";
                    f = this.renderer.path(p);
                    break;
                case "triangle_down":
                    var p = "M " + (n - e) + "," + (l - e) + " L " + (n) + "," + (l + e) + " L " + (n + e) + "," + (l - e) + " Z";
                    f = this.renderer.path(p);
                    break;
                case "triangle_left":
                    var p = "M " + (n - e) + "," + (l) + " L " + (n + e) + "," + (l + e) + " L " + (n + e) + "," + (l - e) + " Z";
                    f = this.renderer.path(p);
                    break;
                case "triangle_right":
                    var p = "M " + (n - e) + "," + (l - e) + " L " + (n - e) + "," + (l + e) + " L " + (n + e) + "," + (l) + " Z";
                    f = this.renderer.path(p);
                    break;
                default:
                    f = this.renderer.circle(n, l, j)
            }
            this.renderer.attr(f, {
                fill: d,
                "fill-opacity": o,
                stroke: m,
                "stroke-width": i,
                "stroke-opacity": h,
                "stroke-dasharray": c || ""
            });
            return f
        },
        _getSymbol: function(h, c) {
            var d = ["circle", "square", "diamond", "triangle_up", "triangle_down", "triangle_left", "triangle_right"];
            var f = this.seriesGroups[h];
            var e = f.series[c];
            var i;
            if (e.symbolType != undefined) {
                i = e.symbolType
            }
            if (i == undefined) {
                i = f.symbolType
            }
            if (i == "default") {
                return d[c % d.length]
            } else {
                if (i != undefined) {
                    return i
                }
            }
            return "none"
        },
        _startTooltipTimer: function(i, h, e) {
            this._cancelTooltipTimer();
            var c = this;
            var f = c.seriesGroups[i];
            var d = this.toolTipShowDelay || this.toolTipDelay;
            if (isNaN(d) || d > 10000 || d < 0) {
                d = 500
            }
            if (this._ttEl || (true == this.enableCrosshairs && false == this.showToolTips)) {
                d = 0
            }
            clearTimeout(this._tttimerHide);
            if (d == 0) {
                c._showToolTip(c._mouseX, c._mouseY - 3, i, h, e)
            }
            this._tttimer = setTimeout(function() {
                if (d != 0) {
                    c._showToolTip(c._mouseX, c._mouseY - 3, i, h, e)
                }
                var j = c.toolTipHideDelay;
                if (isNaN(j)) {
                    j = 4000
                }
                c._tttimerHide = setTimeout(function() {
                    c._hideToolTip();
                    c._unselect()
                }, j)
            }, d)
        },
        _cancelTooltipTimer: function() {
            clearTimeout(this._tttimer)
        },
        _getGroupGradientType: function(d) {
            var c = this.seriesGroups[d];
            if (c.type.indexOf("area") != -1) {
                return c.orientation == "horizontal" ? "horizontalLinearGradient" : "verticalLinearGradient"
            } else {
                if (c.type.indexOf("column") != -1 || c.type.indexOf("candle") != -1) {
                    if (c.polar) {
                        return "radialGradient"
                    }
                    return c.orientation == "horizontal" ? "verticalLinearGradient" : "horizontalLinearGradient"
                } else {
                    if (c.type.indexOf("scatter") != -1 || c.type.indexOf("bubble") != -1 || c.type.indexOf("pie") != -1 || c.type.indexOf("donut") != -1) {
                        return "radialGradient"
                    }
                }
            }
            return undefined
        },
        _select: function(i, m, p, o, j, n) {
            if (this._selected) {
                if ((this._selected.item != j || this._selected.series != o || this._selected.group != p)) {
                    this._unselect()
                } else {
                    return
                }
            }
            var l = this.seriesGroups[p];
            var q = l.series[o];
            var h = l.type.indexOf("line") != -1 && l.type.indexOf("area") == -1;
            this._selected = {
                element: i,
                type: m,
                group: p,
                series: o,
                item: j,
                iidxBase: n,
                isLineType: h,
                linesUnselectMode: q.linesUnselectMode || l.linesUnselectMode
            };
            var c = this._getColors(p, o, n || j, this._getGroupGradientType(p));
            var d = c.fillColorSelected;
            if (h) {
                d = "none"
            }
            var f = this._getSerieSettings(p, o);
            var e = m == "symbol" ? c.lineColorSymbolSelected : c.lineColorSelected;
            d = m == "symbol" ? c.fillColorSymbolSelected : d;
            var k = f.stroke;
            if (this.renderer.getAttr(i, "fill") == c.fillColorAlt) {
                d = c.fillColorAltSelected
            }
            this.renderer.attr(i, {
                stroke: e,
                fill: d,
                "stroke-width": k
            });
            this._raiseItemEvent("mouseover", l, q, j)
        },
        _unselect: function() {
            var p = this;
            if (p._selected) {
                var o = p._selected.group;
                var n = p._selected.series;
                var h = p._selected.item;
                var l = p._selected.iidxBase;
                var k = p._selected.type;
                var j = p.seriesGroups[o];
                var q = j.series[n];
                var f = j.type.indexOf("line") != -1 && j.type.indexOf("area") == -1;
                var c = p._getColors(o, n, l || h, p._getGroupGradientType(o));
                var d = c.fillColor;
                if (f) {
                    d = "none"
                }
                var e = p._getSerieSettings(o, n);
                var m = k == "symbol" ? c.lineColorSymbol : c.lineColor;
                d = k == "symbol" ? c.fillColorSymbol : d;
                if (this.renderer.getAttr(p._selected.element, "fill") == c.fillColorAltSelected) {
                    d = c.fillColorAlt
                }
                var i = e.stroke;
                this.renderer.attr(p._selected.element, {
                    stroke: m,
                    fill: d,
                    "stroke-width": i
                });
                p._selected = undefined;
                if (!isNaN(h)) {
                    p._raiseItemEvent("mouseout", j, q, h)
                }
            }
            if (p._pointMarker) {
                if (p._pointMarker.element) {
                    p.renderer.removeElement(p._pointMarker.element);
                    p._pointMarker.element = undefined
                }
                p._pointMarker = undefined;
                p._hideCrosshairs()
            }
        },
        _raiseItemEvent: function(h, i, f, d) {
            var e = f[h] || i[h];
            var j = 0;
            for (; j < this.seriesGroups.length; j++) {
                if (this.seriesGroups[j] == i) {
                    break
                }
            }
            if (j == this.seriesGroups.length) {
                return
            }
            var c = {
                event: h,
                seriesGroup: i,
                serie: f,
                elementIndex: d,
                elementValue: this._getDataValue(d, f.dataField, j)
            };
            if (e && a.isFunction(e)) {
                e(c)
            }
            this._raiseEvent(h, c)
        },
        _raiseEvent: function(e, d) {
            var f = new a.Event(e);
            f.owner = this;
            f.args = d;
            var c = this.host.trigger(f);
            return c
        },
        _calcInterval: function(e, l, k) {
            var o = Math.abs(l - e);
            var m = o / k;
            var h = [1, 2, 3, 4, 5, 10, 15, 20, 25, 50, 100];
            var c = [0.5, 0.25, 0.125, 0.1];
            var d = 0.1;
            var j = h;
            if (m < 1) {
                j = c;
                d = 10
            }
            var n = 0;
            do {
                n = 0;
                if (m >= 1) {
                    d *= 10
                } else {
                    d /= 10
                }
                for (var f = 1; f < j.length; f++) {
                    if (Math.abs(j[n] * d - m) > Math.abs(j[f] * d - m)) {
                        n = f
                    } else {
                        break
                    }
                }
            } while (n == j.length - 1);
            return j[n] * d
        },
        _renderDataClone: function() {
            if (!this._renderData || this._isToggleRefresh) {
                return
            }
            var e = this._elementRenderInfo = [];
            if (this._isSelectorRefresh) {
                return
            }
            for (var k = 0; k < this._renderData.length; k++) {
                var d = this._getCategoryAxis(k).dataField;
                while (e.length <= k) {
                    e.push({})
                }
                var c = e[k];
                var h = this._renderData[k];
                if (!h.offsets) {
                    continue
                }
                if (h.valueAxis) {
                    c.valueAxis = {
                        itemOffsets: {}
                    };
                    for (var m in h.valueAxis.itemOffsets) {
                        c.valueAxis.itemOffsets[m] = h.valueAxis.itemOffsets[m]
                    }
                }
                if (h.xAxis) {
                    c.xAxis = {
                        itemOffsets: {}
                    };
                    for (var m in h.xAxis.itemOffsets) {
                        c.xAxis.itemOffsets[m] = h.xAxis.itemOffsets[m]
                    }
                }
                c.series = [];
                var j = c.series;
                var l = this.seriesGroups[k].type;
                var o = l.indexOf("pie") != -1 || l.indexOf("donut") != -1;
                for (var p = 0; p < h.offsets.length; p++) {
                    j.push({});
                    for (var f = 0; f < h.offsets[p].length; f++) {
                        if (!o) {
                            j[p][h.xoffsets.xvalues[f]] = {
                                value: h.offsets[p][f].value,
                                valueFrom: h.offsets[p][f].valueFrom,
                                valueRadius: h.offsets[p][f].valueRadius,
                                xoffset: h.xoffsets.data[f],
                                from: h.offsets[p][f].from,
                                to: h.offsets[p][f].to
                            }
                        } else {
                            var n = h.offsets[p][f];
                            j[p][n.displayValue] = {
                                value: n.value,
                                x: n.x,
                                y: n.y,
                                fromAngle: n.fromAngle,
                                toAngle: n.toAngle
                            }
                        }
                    }
                }
            }
        },
        _getDataPointOffset: function(h, d, e, j, f, c) {
            var i;
            if (isNaN(h)) {
                h = d
            }
            if (!isNaN(e)) {
                i = (a.jqx.log(h, e) - a.jqx.log(d, e)) * j
            } else {
                i = (h - d) * j
            }
            if (this._isVML) {
                i = Math.round(i)
            }
            if (c) {
                i = f + i
            } else {
                i = f - i
            }
            return i
        },
        _calcGroupOffsets: function(n, R) {
            var D = this.seriesGroups[n];
            while (this._renderData.length < n + 1) {
                this._renderData.push({})
            }
            if (this._renderData[n] != null && this._renderData[n].offsets != undefined) {
                return this._renderData[n]
            }
            if (D.type.indexOf("pie") != -1 || D.type.indexOf("donut") != -1) {
                return this._calcPieSeriesGroupOffsets(n, R)
            }
            if (!D.valueAxis || !D.series || D.series.length == 0) {
                return this._renderData[n]
            }
            var E = D.valueAxis.flip == true;
            var U = D.valueAxis.logarithmicScale == true;
            var T = D.valueAxis.logarithmicScaleBase || 10;
            var Z = [];
            var K = D.type.indexOf("stacked") != -1;
            var e = K && D.type.indexOf("100") != -1;
            var Q = D.type.indexOf("range") != -1;
            var aa = D.type.indexOf("column") != -1;
            var u = this._getDataLen(n);
            var t = D.baselineValue || D.valueAxis.baselineValue || 0;
            if (e) {
                t = 0
            }
            var al = this._stats.seriesGroups[n];
            if (!al || !al.isValid) {
                return
            }
            if (t > al.max) {
                t = al.max
            }
            if (t < al.min) {
                t = al.min
            }
            var s = (e || U) ? al.maxRange : al.max - al.min;
            var aq = al.min;
            var H = al.max;
            var S = R.height / (U ? al.intervals : s);
            var an = 0;
            if (e) {
                if (aq * H < 0) {
                    s /= 2;
                    an = -(s + t) * S
                } else {
                    an = -t * S
                }
            } else {
                an = -(t - aq) * S
            }
            if (E) {
                an = R.y - an
            } else {
                an += R.y + R.height
            }
            var am = [];
            var ai = [];
            var Y = [];
            var ap, M;
            if (U) {
                ap = a.jqx.log(H, T) - a.jqx.log(t, T);
                if (K) {
                    ap = al.intervals;
                    t = e ? 0 : aq
                }
                M = al.intervals - ap;
                if (!E) {
                    an = R.y + ap / al.intervals * R.height
                }
            }
            an = a.jqx._ptrnd(an);
            var d = (aq * H < 0) ? R.height / 2 : R.height;
            var o = [];
            var G = [];
            if (D.bands) {
                for (var ag = 0; ag < D.bands.length; ag++) {
                    var z = D.bands[ag].minValue;
                    var av = D.bands[ag].maxValue;
                    var A = this._getDataPointOffset(z, t, U ? T : NaN, S, an, E);
                    var P = this._getDataPointOffset(av, t, U ? T : NaN, S, an, E);
                    G.push({
                        from: A,
                        to: P
                    })
                }
            }
            var ac = [];
            var ar = aa || (!aa && !K) || e || U;
            for (var ag = 0; ag < D.series.length; ag++) {
                if (!K && U) {
                    o = []
                }
                var I = D.series[ag];
                var J = I.dataField;
                var au = I.dataFieldFrom;
                var V = I.dataFieldTo;
                var ae = I.radiusDataField || I.sizeDataField;
                Z.push([]);
                var m = this._isSerieVisible(n, ag);
                for (var ah = 0; ah < u; ah++) {
                    if (D.type.indexOf("candle") == -1 && D.type.indexOf("ohlc") == -1) {
                        while (ac.length <= ah) {
                            ac.push(0)
                        }
                        var at = NaN;
                        if (Q) {
                            at = this._getDataValueAsNumber(ah, au, n);
                            if (isNaN(at)) {
                                at = t
                            }
                        }
                        var O = NaN;
                        if (Q) {
                            O = this._getDataValueAsNumber(ah, V, n)
                        } else {
                            O = this._getDataValueAsNumber(ah, J, n)
                        }
                        var l = this._getDataValueAsNumber(ah, ae, n);
                        if (!m) {
                            O = NaN
                        }
                        if (isNaN(O) || (U && O <= 0)) {
                            Z[ag].push({
                                from: undefined,
                                to: undefined
                            });
                            continue
                        }
                        var N;
                        if (ar) {
                            N = (O >= t) ? am : ai
                        } else {
                            ac[ah] = O = ac[ah] + O
                        }
                        var ak = S * (O - t);
                        if (Q) {
                            ak = S * (O - at)
                        }
                        if (U) {
                            while (o.length <= ah) {
                                o.push({
                                    p: {
                                        value: 0,
                                        height: 0
                                    },
                                    n: {
                                        value: 0,
                                        height: 0
                                    }
                                })
                            }
                            var F = Q ? at : t;
                            var af = O > F ? o[ah].p : o[ah].n;
                            af.value += O;
                            if (e) {
                                O = af.value / (al.psums[ah] + al.nsums[ah]) * 100;
                                ak = (a.jqx.log(O, T) - al.minPow) * S
                            } else {
                                ak = a.jqx.log(af.value, T) - a.jqx.log(F, T);
                                ak *= S
                            }
                            ak -= af.height;
                            af.height += ak
                        }
                        var X = an;
                        if (Q) {
                            var v = 0;
                            if (U) {
                                v = (a.jqx.log(at, T) - a.jqx.log(t, T)) * S
                            } else {
                                v = (at - t) * S
                            }
                            X += E ? v : -v
                        }
                        if (K) {
                            if (e && !U) {
                                var C = (al.psums[ah] - al.nsums[ah]);
                                if (O > t) {
                                    ak = (al.psums[ah] / C) * d;
                                    if (al.psums[ah] != 0) {
                                        ak *= O / al.psums[ah]
                                    }
                                } else {
                                    ak = (al.nsums[ah] / C) * d;
                                    if (al.nsums[ah] != 0) {
                                        ak *= O / al.nsums[ah]
                                    }
                                }
                            }
                            if (ar) {
                                if (isNaN(N[ah])) {
                                    N[ah] = X
                                }
                                X = N[ah]
                            }
                        }
                        if (isNaN(Y[ah])) {
                            Y[ah] = 0
                        }
                        var aj = Y[ah];
                        ak = Math.abs(ak);
                        var ab = ak;
                        h_new = this._isVML ? Math.round(ak) : a.jqx._ptrnd(ak) - 1;
                        if (Math.abs(ak - h_new) > 0.5) {
                            ak = Math.round(ak)
                        } else {
                            ak = h_new
                        }
                        aj += ak - ab;
                        if (!K) {
                            aj = 0
                        }
                        if (Math.abs(aj) > 0.5) {
                            if (aj > 0) {
                                ak -= 1;
                                aj -= 1
                            } else {
                                ak += 1;
                                aj += 1
                            }
                        }
                        Y[ah] = aj;
                        if (ag == D.series.length - 1 && e) {
                            var B = 0;
                            for (var ad = 0; ad < ag; ad++) {
                                B += Math.abs(Z[ad][ah].to - Z[ad][ah].from)
                            }
                            B += ak;
                            if (B < d) {
                                if (ak > 0.5) {
                                    ak = a.jqx._ptrnd(ak + d - B)
                                } else {
                                    var ad = ag - 1;
                                    while (ad >= 0) {
                                        var L = Math.abs(Z[ad][ah].to - Z[ad][ah].from);
                                        if (L > 1) {
                                            if (Z[ad][ah].from > Z[ad][ah].to) {
                                                Z[ad][ah].from += d - B
                                            }
                                            break
                                        }
                                        ad--
                                    }
                                }
                            }
                        }
                        if (E) {
                            ak *= -1
                        }
                        var W = O < t;
                        if (Q) {
                            W = at > O
                        }
                        var p = isNaN(at) ? O : {
                            from: at,
                            to: O
                        };
                        if (W) {
                            if (ar) {
                                N[ah] += ak
                            }
                            Z[ag].push({
                                from: X,
                                to: X + ak,
                                value: p,
                                valueFrom: at,
                                valueRadius: l
                            })
                        } else {
                            if (ar) {
                                N[ah] -= ak
                            }
                            Z[ag].push({
                                from: X,
                                to: X - ak,
                                value: p,
                                valueFrom: at,
                                valueRadius: l
                            })
                        }
                    } else {
                        Z[ag].push({});
                        var c = ["Open", "Close", "High", "Low"];
                        for (var ao in c) {
                            var q = "dataField" + c[ao];
                            if (I[q]) {
                                Z[ag][Z[ag].length - 1][c[ao]] = this._getDataPointOffset(this._getDataValueAsNumber(ah, I[q], n), t, U ? T : NaN, S, an, E)
                            }
                        }
                    }
                }
            }
            var w = this._renderData[n];
            w.baseOffset = an;
            w.offsets = Z;
            w.bands = G;
            w.xoffsets = this._calculateXOffsets(n, R.width);
            return this._renderData[n]
        },
        _calcPieSeriesGroupOffsets: function(e, c) {
            var n = this._getDataLen(e);
            var o = this.seriesGroups[e];
            var A = this._renderData[e] = {};
            var G = A.offsets = [];
            for (var C = 0; C < o.series.length; C++) {
                var v = o.series[C];
                var E = v.minAngle;
                if (isNaN(E) || E < 0 || E > 360) {
                    E = 0
                }
                var M = v.maxAngle;
                if (isNaN(M) || M < 0 || M > 360) {
                    M = 360
                }
                var f = M - E;
                var p = v.initialAngle || 0;
                if (p < E) {
                    p = E
                }
                if (p > M) {
                    p = M
                }
                var z = p;
                var h = v.radius || Math.min(c.width, c.height) * 0.4;
                if (isNaN(h)) {
                    h = 1
                }
                var m = v.innerRadius || 0;
                if (isNaN(m) || m >= h) {
                    m = 0
                }
                var d = v.centerOffset || 0;
                var K = a.jqx.getNum([v.offsetX, o.offsetX, c.width / 2]);
                var J = a.jqx.getNum([v.offsetY, o.offsetY, c.height / 2]);
                G.push([]);
                var j = 0;
                var k = 0;
                for (var F = 0; F < n; F++) {
                    var L = this._getDataValueAsNumber(F, v.dataField, e);
                    if (isNaN(L)) {
                        continue
                    }
                    if (!this._isSerieVisible(e, C, F) && v.hiddenPointsDisplay != true) {
                        continue
                    }
                    if (L > 0) {
                        j += L
                    } else {
                        k += L
                    }
                }
                var u = j - k;
                if (u == 0) {
                    u = 1
                }
                for (var F = 0; F < n; F++) {
                    var L = this._getDataValueAsNumber(F, v.dataField, e);
                    if (isNaN(L)) {
                        G[C].push({});
                        continue
                    }
                    var D = v.displayText || v.displayField;
                    var l = this._getDataValue(F, D, e);
                    if (l == undefined) {
                        l = F
                    }
                    var I = 0;
                    var B = this._isSerieVisible(e, C, F);
                    if (B || v.hiddenPointsDisplay == true) {
                        I = Math.abs(L) / u * f
                    }
                    var t = c.x + K;
                    var q = c.y + J;
                    var H = d;
                    if (a.isFunction(d)) {
                        H = d({
                            seriesIndex: C,
                            seriesGroupIndex: e,
                            itemIndex: F
                        })
                    }
                    if (isNaN(H)) {
                        H = 0
                    }
                    var w = {
                        key: e + "_" + C + "_" + F,
                        value: L,
                        displayValue: l,
                        x: t,
                        y: q,
                        fromAngle: z,
                        toAngle: z + I,
                        centerOffset: H,
                        innerRadius: m,
                        outerRadius: h,
                        visible: B
                    };
                    G[C].push(w);
                    z += I
                }
            }
            return A
        },
        _isPointSeriesOnly: function() {
            for (var c = 0; c < this.seriesGroups.length; c++) {
                var d = this.seriesGroups[c];
                if (d.type.indexOf("line") == -1 && d.type.indexOf("area") == -1 && d.type.indexOf("scatter") == -1 && d.type.indexOf("bubble") == -1) {
                    return false
                }
            }
            return true
        },
        _hasColumnSeries: function() {
            var e = ["column", "ohlc", "candlestick"];
            for (var d = 0; d < this.seriesGroups.length; d++) {
                var f = this.seriesGroups[d];
                for (var c in e) {
                    if (f.type.indexOf(e[c]) != -1) {
                        return true
                    }
                }
            }
            return false
        },
        _alignValuesWithTicks: function(h) {
            var c = this._isPointSeriesOnly();
            var d = this.seriesGroups[h];
            var f = this._getCategoryAxis(h);
            var e = f.valuesOnTicks == undefined ? c : f.valuesOnTicks != false;
            if (h == undefined) {
                return e
            }
            if (d.valuesOnTicks == undefined) {
                return e
            }
            return d.valuesOnTicks
        },
        _getYearsDiff: function(d, c) {
            return c.getFullYear() - d.getFullYear()
        },
        _getMonthsDiff: function(d, c) {
            return 12 * (c.getFullYear() - d.getFullYear()) + c.getMonth() - d.getMonth()
        },
        _getDateDiff: function(h, f, e, c) {
            var d = 0;
            if (e != "year" && e != "month") {
                d = f.valueOf() - h.valueOf()
            }
            switch (e) {
                case "year":
                    d = this._getYearsDiff(h, f);
                    break;
                case "month":
                    d = this._getMonthsDiff(h, f);
                    break;
                case "day":
                    d /= (24 * 3600 * 1000);
                    break;
                case "hour":
                    d /= (3600 * 1000);
                    break;
                case "minute":
                    d /= (60 * 1000);
                    break;
                case "second":
                    d /= (1000);
                    break;
                case "millisecond":
                    break
            }
            if (e != "year" && e != "month" && c != false) {
                d = a.jqx._rnd(d, 1, true)
            }
            return d
        },
        _getAsDate: function(c, d) {
            c = this._castAsDate(c);
            if (d == "month") {
                return new Date(c.getFullYear(), c.getMonth(), 1)
            }
            if (d == "year") {
                return new Date(c.getFullYear(), 0, 1)
            }
            if (d == "day") {
                return new Date(c.getFullYear(), c.getMonth(), c.getDate())
            }
            return c
        },
        _getBestDTUnit: function(m, s, t, e, k) {
            var h = "day";
            var o = s.valueOf() - m.valueOf();
            if (o < 1000) {
                h = "second"
            } else {
                if (o < 3600000) {
                    h = "minute"
                } else {
                    if (o < 86400000) {
                        h = "hour"
                    } else {
                        if (o < 2592000000) {
                            h = "day"
                        } else {
                            if (o < 31104000000) {
                                h = "month"
                            } else {
                                h = "year"
                            }
                        }
                    }
                }
            }
            var q = [{
                key: "year",
                cnt: o / (1000 * 60 * 60 * 24 * 365)
            }, {
                key: "month",
                cnt: o / (1000 * 60 * 60 * 24 * 30)
            }, {
                key: "day",
                cnt: o / (1000 * 60 * 60 * 24)
            }, {
                key: "hour",
                cnt: o / (1000 * 60 * 60)
            }, {
                key: "minute",
                cnt: o / (1000 * 60)
            }, {
                key: "second",
                cnt: o / 1000
            }, {
                key: "millisecond",
                cnt: o
            }];
            var n = -1;
            for (var l = 0; l < q.length; l++) {
                if (q[l].key == h) {
                    n = l;
                    break
                }
            }
            var c = -1,
                p = -1;
            for (; n < q.length; n++) {
                if (q[n].cnt / 100 > e) {
                    break
                }
                var d = this._estAxisInterval(m, s, t, e, q[n].key, k);
                var f = this._getDTIntCnt(m, s, d, q[n].key);
                if (c == -1 || c < f) {
                    c = f;
                    p = n
                }
            }
            h = q[p].key;
            return h
        },
        _getCategoryAxisStats: function(h, C, A) {
            var k = this._getDataLen(h);
            var c = C.type == "date" || C.type == "time";
            var l = c ? this._castAsDate(C.minValue) : this._castAsNumber(C.minValue);
            var n = c ? this._castAsDate(C.maxValue) : this._castAsNumber(C.maxValue);
            var u = l,
                w = n;
            var f, m;
            var d = C.type == undefined || C.type == "auto";
            var j = (d || C.type == "basic");
            var v = 0,
                e = 0;
            for (var z = 0; z < k && C.dataField; z++) {
                var t = this._getDataValue(z, C.dataField, h);
                t = c ? this._castAsDate(t) : this._castAsNumber(t);
                if (isNaN(t)) {
                    continue
                }
                if (c) {
                    v++
                } else {
                    e++
                }
                if (isNaN(f) || t < f) {
                    f = t
                }
                if (isNaN(m) || t >= m) {
                    m = t
                }
            }
            if (d && ((!c && e == k) || (c && v == k))) {
                j = false
            }
            if (j) {
                f = 0;
                m = k - 1
            }
            if (isNaN(u)) {
                u = f
            }
            if (isNaN(w)) {
                w = m
            }
            if (c) {
                if (!this._isDate(u)) {
                    u = this._isDate(w) ? w : new Date()
                }
                if (!this._isDate(w)) {
                    w = this._isDate(u) ? u : new Date()
                }
            } else {
                if (isNaN(u)) {
                    u = 0
                }
                if (isNaN(w)) {
                    w = j ? k - 1 : u
                }
            }
            if (f == undefined) {
                f = u
            }
            if (m == undefined) {
                m = w
            }
            var o = C.rangeSelector;
            if (o) {
                var p = o.minValue || u;
                if (p && c) {
                    p = this._castAsDate(p)
                }
                var s = o.maxValue || w;
                if (s && c) {
                    s = this._castAsDate(s)
                }
                if (u < p) {
                    u = p
                }
                if (w < p) {
                    w = s
                }
                if (u > s) {
                    u = p
                }
                if (w > s) {
                    w = s
                }
            }
            var B = C.unitInterval;
            var q, D;
            if (c) {
                q = C.baseUnit;
                if (!q) {
                    q = this._getBestDTUnit(u, w, h, A)
                }
                D = q == "hour" || q == "minute" || q == "second" || q == "millisecond"
            }
            var B = C.unitInterval;
            if (isNaN(B) || B <= 0) {
                B = this._estAxisInterval(u, w, h, A, q)
            }
            return {
                min: u,
                max: w,
                dsRange: {
                    min: f,
                    max: m
                },
                useIndeces: j,
                isDateTime: c,
                isTimeUnit: D,
                dateTimeUnit: q,
                interval: B
            }
        },
        _getDefaultDTFormatFn: function(e) {
            var c = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            var d;
            if (e == "year" || e == "month" || e == "day") {
                d = function(f) {
                    return f.getDate() + "-" + c[f.getMonth()] + "-" + f.getFullYear()
                }
            } else {
                d = function(f) {
                    return f.getHours() + ":" + f.getMinutes() + ":" + f.getSeconds()
                }
            }
            return d
        },
        _getDTIntCnt: function(h, d, e, k) {
            var f = 0;
            var i = new Date(h);
            var j = new Date(d);
            if (e <= 0) {
                return 1
            }
            while (i.valueOf() < j.valueOf()) {
                if (k == "millisecond") {
                    i.setMilliseconds(i.getMilliseconds() + e)
                } else {
                    if (k == "second") {
                        i.setSeconds(i.getSeconds() + e)
                    } else {
                        if (k == "minute") {
                            i.setMinutes(i.getMinutes() + e)
                        } else {
                            if (k == "hour") {
                                var c = i.valueOf();
                                i.setHours(i.getHours() + e);
                                if (c === i.valueOf()) {
                                    i.setHours(i.getHours() + e + 1)
                                }
                            } else {
                                if (k == "day") {
                                    i.setDate(i.getDate() + e)
                                } else {
                                    if (k == "month") {
                                        i.setMonth(i.getMonth() + e)
                                    } else {
                                        if (k == "year") {
                                            i.setFullYear(i.getFullYear() + e)
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                f++
            }
            return f
        },
        _estAxisInterval: function(f, k, n, c, l, d) {
            var e = [1, 2, 5, 10, 15, 20, 50, 100, 200, 500];
            var j = 0;
            var h = c / ((!isNaN(d) && d > 0) ? d : 50);
            if (this._renderData && this._renderData.length > n && this._renderData[n].xAxis && !isNaN(this._renderData[n].xAxis.avgWidth)) {
                var p = Math.max(1, this._renderData[n].xAxis.avgWidth);
                if (p != 0 && isNaN(d)) {
                    h = 0.9 * c / p
                }
            }
            if (h <= 1) {
                return 1
            }
            var o = 0;
            while (true) {
                var m = j >= e.length ? Math.pow(10, 3 + j - e.length) : e[j];
                if (this._isDate(f) && this._isDate(k)) {
                    o = this._getDTIntCnt(f, k, m, l)
                } else {
                    o = (k - f) / m
                }
                if (o <= h) {
                    return m
                }
                j++
            }
        },
        _getPaddingSize: function(l, f, h, d, n, o) {
            var i = l.min;
            var k = l.max;
            var c = l.interval;
            var e = l.dateTimeUnit;
            if (n) {
                return {
                    left: 0,
                    right: (d / Math.max(1, k - i + 1)) * c
                }
            }
            if (h && !o) {
                return {
                    left: 0,
                    right: 0
                }
            }
            if (this._isDate(i) && this._isDate(k)) {
                var m = this._getDTIntCnt(i, k, Math.min(c, k - i), e);
                var j = d / Math.max(2, m);
                return {
                    left: j / 2,
                    right: j / 2
                }
            }
            var m = Math.max(1, k - i);
            if (m == 1) {
                sz = d / 4;
                return {
                    left: sz,
                    right: sz
                }
            }
            var j = d / (m + 1);
            return {
                left: j / 2,
                right: j / 2
            }
        },
        _calculateXOffsets: function(f, C) {
            var B = this.seriesGroups[f];
            var o = this._getCategoryAxis(f);
            var v = [];
            var m = [];
            var n = this._getDataLen(f);
            var e = this._getCategoryAxisStats(f, o, C);
            var u = e.min;
            var z = e.max;
            var c = e.isDateTime;
            var D = e.isTimeUnit;
            var A = this._hasColumnSeries();
            var d = B.polar || B.spider;
            var l = this._alignValuesWithTicks(f);
            var s = this._getPaddingSize(e, o, l, C, d, A);
            var F = z - u;
            if (F == 0) {
                F = 1
            }
            var E = C - s.left - s.right;
            if (d && l) {
                s.left = s.right = 0
            }
            var j = -1,
                p = -1;
            for (var w = 0; w < n; w++) {
                var t = (o.dataField === undefined) ? w : this._getDataValue(w, o.dataField, f);
                if (e.useIndeces) {
                    if (w < u || w > z) {
                        v.push(NaN);
                        m.push(undefined);
                        continue
                    }
                    v.push(a.jqx._ptrnd(s.left + (w - u) / F * E));
                    m.push(t);
                    if (j == -1) {
                        j = w
                    }
                    if (p == -1 || p < w) {
                        p = w
                    }
                    continue
                }
                t = c ? this._castAsDate(t) : this._castAsNumber(t);
                if (isNaN(t) || t < u || t > z) {
                    v.push(NaN);
                    m.push(undefined);
                    continue
                }
                var q = 0;
                if (!c || (c && D)) {
                    diffFromMin = t - u;
                    q = (t - u) * E / F
                } else {
                    q = (t.valueOf() - u.valueOf()) / (z.valueOf() - u.valueOf()) * E
                }
                q = a.jqx._ptrnd(s.left + q);
                v.push(q);
                m.push(t);
                if (j == -1) {
                    j = w
                }
                if (p == -1 || p < w) {
                    p = w
                }
            }
            if (o.flip == true) {
                for (var w = 0; w < v.length; w++) {
                    if (!isNaN(v[w])) {
                        v[w] = C - v[w]
                    }
                }
            }
            if (D || c) {
                F = this._getDateDiff(u, z, o.baseUnit);
                F = a.jqx._rnd(F, 1, false)
            }
            var k = Math.max(1, F);
            var h = E / k;
            if (j == p) {
                v[j] = s.left + E / 2
            }
            return {
                axisStats: e,
                data: v,
                xvalues: m,
                first: j,
                last: p,
                length: p == -1 ? 0 : p - j + 1,
                itemWidth: h,
                intervalWidth: h * e.interval,
                rangeLength: F,
                useIndeces: e.useIndeces,
                padding: s
            }
        },
        _getCategoryAxis: function(c) {
            if (c == undefined || this.seriesGroups.length <= c) {
                return this.categoryAxis || this.xAxis
            }
            return this.seriesGroups[c].categoryAxis || this.seriesGroups[c].xAxis || this.categoryAxis || this.xAxis
        },
        _isGreyScale: function(f, c) {
            var e = this.seriesGroups[f];
            var d = e.series[c];
            if (d.greyScale == true) {
                return true
            } else {
                if (d.greyScale == false) {
                    return false
                }
            }
            if (e.greyScale == true) {
                return true
            } else {
                if (e.greyScale == false) {
                    return false
                }
            }
            return this.greyScale == true
        },
        _getSeriesColors: function(h, d, f) {
            var c = this._getSeriesColorsInternal(h, d, f);
            if (this._isGreyScale(h, d)) {
                for (var e in c) {
                    c[e] = a.jqx.toGreyScale(c[e])
                }
            }
            return c
        },
        _getColorFromScheme: function(q, n, c) {
            var e = "#000000";
            var p = this.seriesGroups[q];
            var k = p.series[n];
            if (p.type == "pie" || p.type == "donut") {
                var d = this._getDataLen(q);
                e = this._getItemColorFromScheme(k.colorScheme || p.colorScheme || this.colorScheme, n * d + c, q, n)
            } else {
                var o = 0;
                for (var h = 0; h <= q; h++) {
                    for (var f in this.seriesGroups[h].series) {
                        if (h == q && f == n) {
                            break
                        } else {
                            o++
                        }
                    }
                }
                var m = this.colorScheme;
                if (p.colorScheme) {
                    m = p.colorScheme;
                    sidex = seriesIndex
                }
                if (m == undefined || m == "") {
                    m = this.colorSchemes[0].name
                }
                if (!m) {
                    return e
                }
                for (var h = 0; h < this.colorSchemes.length; h++) {
                    var l = this.colorSchemes[h];
                    if (l.name == m) {
                        while (o > l.colors.length) {
                            o -= l.colors.length;
                            if (++h >= this.colorSchemes.length) {
                                h = 0
                            }
                            l = this.colorSchemes[h]
                        }
                        e = l.colors[o % l.colors.length]
                    }
                }
            }
            return e
        },
        _createColorsCache: function() {
            this._colorsCache = {
                get: function(c) {
                    if (this._store[c]) {
                        return this._store[c]
                    }
                },
                set: function(d, c) {
                    if (this._size < 10000) {
                        this._store[d] = c;
                        this._size++
                    }
                },
                clear: function() {
                    this._store = {};
                    this._size = 0
                },
                _size: 0,
                _store: {}
            }
        },
        _getSeriesColorsInternal: function(n, e, c) {
            var i = n + "_" + e + "_" + (isNaN(c) ? "NaN" : c);
            if (this._colorsCache.get(i)) {
                return this._colorsCache.get(i)
            }
            var h = this.seriesGroups[n];
            var p = h.series[e];
            var d = {
                lineColor: "#222222",
                lineColorSelected: "#151515",
                lineColorSymbol: "#222222",
                lineColorSymbolSelected: "#151515",
                fillColor: "#222222",
                fillColorSelected: "#333333",
                fillColorSymbol: "#222222",
                fillColorSymbolSelected: "#333333",
                fillColorAlt: "#222222",
                fillColorAltSelected: "#333333"
            };
            var j;
            if (a.isFunction(p.colorFunction)) {
                var k = !isNaN(c) ? this._getDataValue(c, p.dataField, n) : NaN;
                if (h.type.indexOf("range") != -1) {
                    var f = this._getDataValue(c, p.dataFieldFrom, n);
                    var m = this._getDataValue(c, p.dataFieldTo, n);
                    k = {
                        from: f,
                        to: m
                    }
                }
                j = p.colorFunction(k, c, p, h);
                if (typeof(j) == "object") {
                    for (var l in j) {
                        d[l] = j[l]
                    }
                } else {
                    d.fillColor = j
                }
            } else {
                for (var l in d) {
                    if (p.key) {
                        d[l] = p[l]
                    }
                }
                if (!p.fillColor && !p.color) {
                    d.fillColor = this._getColorFromScheme(n, e, c)
                } else {
                    p.fillColor = p.fillColor || p.color
                }
            }
            var o = {
                fillColor: {
                    baseColor: "fillColor",
                    adjust: 1
                },
                fillColorSelected: {
                    baseColor: "fillColor",
                    adjust: 1.1
                },
                fillColorSymbol: {
                    baseColor: "fillColor",
                    adjust: 1
                },
                fillColorSymbolSelected: {
                    baseColor: "fillColorSymbol",
                    adjust: 2
                },
                fillColorAlt: {
                    baseColor: "fillColor",
                    adjust: 4
                },
                fillColorAltSelected: {
                    baseColor: "fillColor",
                    adjust: 3
                },
                lineColor: {
                    baseColor: "fillColor",
                    adjust: 0.9
                },
                lineColorSelected: {
                    baseColor: "lineColor",
                    adjust: 0.8
                },
                lineColorSymbol: {
                    baseColor: "lineColor",
                    adjust: 1
                },
                lineColorSymbolSelected: {
                    baseColor: "lineColorSelected",
                    adjust: 1
                }
            };
            for (var l in d) {
                if (typeof(j) != "object" || !j[l]) {
                    if (p[l]) {
                        d[l] = p[l]
                    } else {
                        d[l] = a.jqx.adjustColor(d[o[l].baseColor], o[l].adjust)
                    }
                }
            }
            this._colorsCache.set(i, d);
            return d
        },
        _getItemColorFromScheme: function(e, h, m, l) {
            if (e == undefined || e == "") {
                e = this.colorSchemes[0].name
            }
            for (var k = 0; k < this.colorSchemes.length; k++) {
                if (e == this.colorSchemes[k].name) {
                    break
                }
            }
            var f = 0;
            while (f <= h) {
                if (k == this.colorSchemes.length) {
                    k = 0
                }
                var c = this.colorSchemes[k].colors.length;
                if (f + c <= h) {
                    f += c;
                    k++
                } else {
                    var d = this.colorSchemes[k].colors[h - f];
                    if (this._isGreyScale(m, l) && d.indexOf("#") == 0) {
                        d = a.jqx.toGreyScale(d)
                    }
                    return d
                }
            }
        },
        getColorScheme: function(c) {
            for (var d in this.colorSchemes) {
                if (this.colorSchemes[d].name == c) {
                    return this.colorSchemes[d].colors
                }
            }
            return undefined
        },
        addColorScheme: function(d, c) {
            for (var e in this.colorSchemes) {
                if (this.colorSchemes[e].name == d) {
                    this.colorSchemes[e].colors = c;
                    return
                }
            }
            this.colorSchemes.push({
                name: d,
                colors: c
            })
        },
        removeColorScheme: function(c) {
            for (var d in this.colorSchemes) {
                if (this.colorSchemes[d].name == c) {
                    this.colorSchemes.splice(d, 1);
                    break
                }
            }
        },
        colorSchemes: [{
            name: "scheme01",
            colors: ["#307DD7", "#AA4643", "#89A54E", "#71588F", "#4198AF"]
        }, {
            name: "scheme02",
            colors: ["#7FD13B", "#EA157A", "#FEB80A", "#00ADDC", "#738AC8"]
        }, {
            name: "scheme03",
            colors: ["#E8601A", "#FF9639", "#F5BD6A", "#599994", "#115D6E"]
        }, {
            name: "scheme04",
            colors: ["#D02841", "#FF7C41", "#FFC051", "#5B5F4D", "#364651"]
        }, {
            name: "scheme05",
            colors: ["#25A0DA", "#309B46", "#8EBC00", "#FF7515", "#FFAE00"]
        }, {
            name: "scheme06",
            colors: ["#0A3A4A", "#196674", "#33A6B2", "#9AC836", "#D0E64B"]
        }, {
            name: "scheme07",
            colors: ["#CC6B32", "#FFAB48", "#FFE7AD", "#A7C9AE", "#888A63"]
        }, {
            name: "scheme08",
            colors: ["#3F3943", "#01A2A6", "#29D9C2", "#BDF271", "#FFFFA6"]
        }, {
            name: "scheme09",
            colors: ["#1B2B32", "#37646F", "#A3ABAF", "#E1E7E8", "#B22E2F"]
        }, {
            name: "scheme10",
            colors: ["#5A4B53", "#9C3C58", "#DE2B5B", "#D86A41", "#D2A825"]
        }, {
            name: "scheme11",
            colors: ["#993144", "#FFA257", "#CCA56A", "#ADA072", "#949681"]
        }, {
            name: "scheme12",
            colors: ["#105B63", "#EEEAC5", "#FFD34E", "#DB9E36", "#BD4932"]
        }, {
            name: "scheme13",
            colors: ["#BBEBBC", "#F0EE94", "#F5C465", "#FA7642", "#FF1E54"]
        }, {
            name: "scheme14",
            colors: ["#60573E", "#F2EEAC", "#BFA575", "#A63841", "#BFB8A3"]
        }, {
            name: "scheme15",
            colors: ["#444546", "#FFBB6E", "#F28D00", "#D94F00", "#7F203B"]
        }, {
            name: "scheme16",
            colors: ["#583C39", "#674E49", "#948658", "#F0E99A", "#564E49"]
        }, {
            name: "scheme17",
            colors: ["#142D58", "#447F6E", "#E1B65B", "#C8782A", "#9E3E17"]
        }, {
            name: "scheme18",
            colors: ["#4D2B1F", "#635D61", "#7992A2", "#97BFD5", "#BFDCF5"]
        }, {
            name: "scheme19",
            colors: ["#844341", "#D5CC92", "#BBA146", "#897B26", "#55591C"]
        }, {
            name: "scheme20",
            colors: ["#56626B", "#6C9380", "#C0CA55", "#F07C6C", "#AD5472"]
        }, {
            name: "scheme21",
            colors: ["#96003A", "#FF7347", "#FFBC7B", "#FF4154", "#642223"]
        }, {
            name: "scheme22",
            colors: ["#5D7359", "#E0D697", "#D6AA5C", "#8C5430", "#661C0E"]
        }, {
            name: "scheme23",
            colors: ["#16193B", "#35478C", "#4E7AC7", "#7FB2F0", "#ADD5F7"]
        }, {
            name: "scheme24",
            colors: ["#7B1A25", "#BF5322", "#9DA860", "#CEA457", "#B67818"]
        }, {
            name: "scheme25",
            colors: ["#0081DA", "#3AAFFF", "#99C900", "#FFEB3D", "#309B46"]
        }, {
            name: "scheme26",
            colors: ["#0069A5", "#0098EE", "#7BD2F6", "#FFB800", "#FF6800"]
        }, {
            name: "scheme27",
            colors: ["#FF6800", "#A0A700", "#FF8D00", "#678900", "#0069A5"]
        }],
        _formatValue: function(i, k, d, h, c, f) {
            if (i == undefined) {
                return ""
            }
            if (this._isObject(i) && !d) {
                return ""
            }
            if (d) {
                if (!a.isFunction(d)) {
                    return i.toString()
                }
                try {
                    return d(i, f, c, h)
                } catch (j) {
                    return j.message
                }
            }
            if (this._isNumber(i)) {
                return this._formatNumber(i, k)
            }
            if (this._isDate(i)) {
                return this._formatDate(i, k)
            }
            if (k) {
                return (k.prefix || "") + i.toString() + (k.sufix || "")
            }
            return i.toString()
        },
        _getFormattedValue: function(h, j, C, q, f, m) {
            var A = this.seriesGroups[h];
            var o = A.series[j];
            var n = "";
            var k = q,
                l = f;
            if (!l) {
                l = o.formatFunction || A.formatFunction
            }
            if (!k) {
                k = o.formatSettings || A.formatSettings
            }
            if (!o.formatFunction && o.formatSettings) {
                l = undefined
            }
            var p = {},
                v = 0;
            for (var c in o) {
                if (c.indexOf("dataField") == 0) {
                    p[c.substring(9).toLowerCase()] = this._getDataValue(C, o[c], h);
                    v++
                }
            }
            if (v == 0) {
                p = this._getDataValue(C, undefined, h)
            }
            if (l && a.isFunction(l)) {
                try {
                    return l(v == 1 ? p[""] : p, C, o, A)
                } catch (B) {
                    return B.message
                }
            }
            if (v == 1 && (A.type.indexOf("pie") != -1 || A.type.indexOf("donut") != -1)) {
                return this._formatValue(p[""], k, l, h, j, C)
            }
            if (v > 0) {
                var w = 0;
                for (var c in p) {
                    if (w > 0 && n != "") {
                        n += "<br>"
                    }
                    var u = "dataField" + (c.length > 0 ? c.substring(0, 1).toUpperCase() + c.substring(1) : "");
                    var t = "displayText" + (c.length > 0 ? c.substring(0, 1).toUpperCase() + c.substring(1) : "");
                    var z = o[t] || o[u];
                    var d = p[c];
                    if (undefined != d) {
                        d = this._formatValue(d, k, l, h, j, C)
                    }
                    if (m === true) {
                        n += d
                    } else {
                        n += z + ": " + d
                    }
                    w++
                }
            } else {
                if (undefined != p) {
                    n = this._formatValue(p, k, l, h, j, C)
                }
            }
            return n || ""
        },
        _isNumberAsString: function(e) {
            if (typeof(e) != "string") {
                return false
            }
            e = a.trim(e);
            for (var c = 0; c < e.length; c++) {
                var d = e.charAt(c);
                if ((d >= "0" && d <= "9") || d == "," || d == ".") {
                    continue
                }
                if (d == "-" && c == 0) {
                    continue
                }
                if ((d == "(" && c == 0) || (d == ")" && c == e.length - 1)) {
                    continue
                }
                return false
            }
            return true
        },
        _castAsDate: function(e) {
            if (e instanceof Date && !isNaN(e)) {
                return e
            }
            if (typeof(e) == "string") {
                var d = new Date(e);
                if (!isNaN(d)) {
                    if (e.indexOf(":") == -1) {
                        d.setHours(0, 0, 0, 0)
                    }
                } else {
                    if (a.jqx.dataFormat) {
                        var c = a.jqx.dataFormat.tryparsedate(e);
                        if (c) {
                            d = c
                        } else {
                            d = this._parseISO8601Date(e)
                        }
                    } else {
                        d = this._parseISO8601Date(e)
                    }
                }
                if (d != undefined && !isNaN(d)) {
                    return d
                }
            }
            return undefined
        },
        _parseISO8601Date: function(i) {
            var m = i.split(" ");
            if (m.length < 0) {
                return NaN
            }
            var c = m[0].split("-");
            var d = m.length == 2 ? m[1].split(":") : "";
            var h = c[0];
            var j = c.length > 1 ? c[1] - 1 : 0;
            var k = c.length > 2 ? c[2] : 1;
            var e = d[1];
            var f = d.length > 1 ? d[1] : 0;
            var e = d.length > 2 ? d[2] : 0;
            var l = d.length > 3 ? d[3] : 0;
            return new Date(h, j, k, e, f, l)
        },
        _castAsNumber: function(d) {
            if (d instanceof Date && !isNaN(d)) {
                return d.valueOf()
            }
            if (typeof(d) == "string") {
                if (this._isNumber(d)) {
                    d = parseFloat(d)
                } else {
                    if (!/[a-zA-Z]/.test(d)) {
                        var c = new Date(d);
                        if (c != undefined) {
                            d = c.valueOf()
                        }
                    }
                }
            }
            return d
        },
        _isNumber: function(c) {
            if (typeof(c) == "string") {
                if (this._isNumberAsString(c)) {
                    c = parseFloat(c)
                }
            }
            return typeof c === "number" && isFinite(c)
        },
        _isDate: function(c) {
            return c instanceof Date && !isNaN(c.getDate())
        },
        _isBoolean: function(c) {
            return typeof c === "boolean"
        },
        _isObject: function(c) {
            return (c && (typeof c === "object" || a.isFunction(c))) || false
        },
        _formatDate: function(d, c) {
            return d.toString()
        },
        _formatNumber: function(p, f) {
            if (!this._isNumber(p)) {
                return p
            }
            f = f || {};
            var t = f.decimalSeparator || ".";
            var q = f.thousandsSeparator || "";
            var o = f.prefix || "";
            var s = f.sufix || "";
            var k = f.decimalPlaces;
            if (isNaN(k)) {
                k = ((p * 100 != parseInt(p) * 100) ? 2 : 0)
            }
            var n = f.negativeWithBrackets || false;
            var j = (p < 0);
            if (j && n) {
                p *= -1
            }
            var e = p.toString();
            var c;
            var m = Math.pow(10, k);
            e = (Math.round(p * m) / m).toString();
            if (isNaN(e)) {
                e = ""
            }
            c = e.lastIndexOf(".");
            if (k > 0) {
                if (c < 0) {
                    e += t;
                    c = e.length - 1
                } else {
                    if (t !== ".") {
                        e = e.replace(".", t)
                    }
                }
                while ((e.length - 1 - c) < k) {
                    e += "0"
                }
            }
            c = e.lastIndexOf(t);
            c = (c > -1) ? c : e.length;
            var h = e.substring(c);
            var d = 0;
            for (var l = c; l > 0; l--, d++) {
                if ((d % 3 === 0) && (l !== c) && (!j || (l > 1) || (j && n))) {
                    h = q + h
                }
                h = e.charAt(l - 1) + h
            }
            e = h;
            if (j && n) {
                e = "(" + e + ")"
            }
            return o + e + s
        },
        _defaultNumberFormat: {
            prefix: "",
            sufix: "",
            decimalSeparator: ".",
            thousandsSeparator: ",",
            decimalPlaces: 2,
            negativeWithBrackets: false
        },
        _calculateControlPoints: function(i, h) {
            var f = i[h],
                o = i[h + 1],
                e = i[h + 2],
                l = i[h + 3],
                d = i[h + 4],
                k = i[h + 5];
            var n = 0.4;
            var q = Math.sqrt(Math.pow(e - f, 2) + Math.pow(l - o, 2));
            var c = Math.sqrt(Math.pow(d - e, 2) + Math.pow(k - l, 2));
            var j = (q + c);
            if (j == 0) {
                j = 1
            }
            var p = n * q / j;
            var m = n - p;
            return [e + p * (f - d), l + p * (o - k), e - m * (f - d), l - m * (o - k)]
        },
        _getBezierPoints: function(e) {
            var d = "";
            var k = [],
                f = [];
            var j = e.split(" ");
            for (var h = 0; h < j.length; h++) {
                var l = j[h].split(",");
                k.push(parseFloat(l[0]));
                k.push(parseFloat(l[1]))
            }
            var c = k.length;
            for (var h = 0; h < c - 4; h += 2) {
                f = f.concat(this._calculateControlPoints(k, h))
            }
            for (var h = 2; h < c - 5; h += 2) {
                d += " C" + a.jqx._ptrnd(f[2 * h - 2]) + "," + a.jqx._ptrnd(f[2 * h - 1]) + " " + a.jqx._ptrnd(f[2 * h]) + "," + a.jqx._ptrnd(f[2 * h + 1]) + " " + a.jqx._ptrnd(k[h + 2]) + "," + a.jqx._ptrnd(k[h + 3]) + " "
            }
            if (Math.abs(k[0] - k[2]) < 3 || Math.abs(k[1] - k[3]) < 3) {
                d = "M" + a.jqx._ptrnd(k[0]) + "," + a.jqx._ptrnd(k[1]) + " L" + a.jqx._ptrnd(k[2]) + "," + a.jqx._ptrnd(k[3]) + " " + d
            } else {
                d = "M" + a.jqx._ptrnd(k[0]) + "," + a.jqx._ptrnd(k[1]) + " Q" + a.jqx._ptrnd(f[0]) + "," + a.jqx._ptrnd(f[1]) + " " + a.jqx._ptrnd(k[2]) + "," + a.jqx._ptrnd(k[3]) + " " + d
            }
            if (Math.abs(k[c - 2] - k[c - 4]) < 3 || Math.abs(k[c - 1] - k[c - 3]) < 3) {
                d += " L" + a.jqx._ptrnd(k[c - 2]) + "," + a.jqx._ptrnd(k[c - 1]) + " "
            } else {
                d += " Q" + a.jqx._ptrnd(f[c * 2 - 10]) + "," + a.jqx._ptrnd(f[c * 2 - 9]) + " " + a.jqx._ptrnd(k[c - 2]) + "," + a.jqx._ptrnd(k[c - 1]) + " "
            }
            return d
        },
        _animTickInt: 50,
        _createAnimationGroup: function(c) {
            if (!this._animGroups) {
                this._animGroups = {}
            }
            this._animGroups[c] = {
                animations: [],
                startTick: NaN
            }
        },
        _startAnimation: function(e) {
            var f = new Date();
            var c = f.getTime();
            this._animGroups[e].startTick = c;
            this._runAnimation();
            this._enableAnimTimer()
        },
        _enqueueAnimation: function(f, e, d, i, h, c, j) {
            if (i < 0) {
                i = 0
            }
            if (j == undefined) {
                j = "easeInOutSine"
            }
            this._animGroups[f].animations.push({
                key: e,
                properties: d,
                duration: i,
                fn: h,
                context: c,
                easing: j
            })
        },
        _stopAnimations: function() {
            clearTimeout(this._animtimer);
            this._animtimer = undefined;
            this._animGroups = undefined
        },
        _enableAnimTimer: function() {
            if (!this._animtimer) {
                var c = this;
                this._animtimer = setTimeout(function() {
                    c._runAnimation()
                }, this._animTickInt)
            }
        },
        _runAnimation: function(t) {
            if (this._animGroups) {
                var w = new Date();
                var l = w.getTime();
                var s = {};
                for (var n in this._animGroups) {
                    var v = this._animGroups[n].animations;
                    var o = this._animGroups[n].startTick;
                    var k = 0;
                    for (var q = 0; q < v.length; q++) {
                        var z = v[q];
                        var c = (l - o);
                        if (z.duration > k) {
                            k = z.duration
                        }
                        var u = z.duration > 0 ? c / z.duration : 1;
                        var m = u;
                        if (z.easing && z.duration != 0) {
                            m = a.easing[z.easing](u, c, 0, 1, z.duration)
                        }
                        if (u > 1) {
                            u = 1;
                            m = 1
                        }
                        if (z.fn) {
                            z.fn(z.key, z.context, m);
                            continue
                        }
                        var h = {};
                        for (var n = 0; n < z.properties.length; n++) {
                            var e = z.properties[n];
                            var f = 0;
                            if (u == 1) {
                                f = e.to
                            } else {
                                f = easeParecent * (e.to - e.from) + e.from
                            }
                            h[e.key] = f
                        }
                        this.renderer.attr(z.key, h)
                    }
                    if (o + k > l) {
                        s[n] = ({
                            startTick: o,
                            animations: v
                        })
                    }
                }
                this._animGroups = s;
                if (this.renderer instanceof a.jqx.HTML5Renderer) {
                    this.renderer.refresh()
                }
            }
            this._animtimer = null;
            for (var n in this._animGroups) {
                this._enableAnimTimer();
                break
            }
        }
    })
})(jqxBaseFramework);
(function(a) {
    a.extend(a.jqx._jqxChart.prototype, {
        _renderCategoryAxisRangeSelector: function(p, n) {
            var s = this;
            s._isTouchDevice = a.jqx.mobile.isTouchDevice();
            var i = s.seriesGroups[p];
            var e = s._getCategoryAxis(p);
            var l = e ? e.rangeSelector : undefined;
            if (!s._isSelectorRefresh) {
                var q = (l && l.renderTo) ? l.renderTo : s.host;
                q.find(".rangeSelector").remove()
            }
            if (!e || e.visible == false || i.type == "spider") {
                return false
            }
            if (!s._isGroupVisible(p)) {
                return false
            }
            if (!l) {
                return false
            }
            var h = i.orientation == "horizontal";
            if (l.renderTo) {
                h = false
            }
            if (s.rtl) {
                e.flip = true
            }
            var d = h ? this.host.height() : this.host.width();
            d -= 4;
            var o = this._getCategoryAxisStats(p, e, d);
            var k = e.position;
            if (l.renderTo && l.position) {
                k = l.position
            }
            if (!this._isSelectorRefresh) {
                var m = l.renderTo;
                var c = "<div class='rangeSelector jqx-disableselect' style='position: absolute; background-color: transparent;' onselectstart='return false;'></div>";
                var f = a(c).appendTo(m ? m : this.host.find(".chartContainer"));
                if (!m) {
                    var j = this.host.coord();
                    selectorSize = this._selectorGetSize(e);
                    if (!h) {
                        f.css("left", j.left + 1);
                        f.css("top", j.top + n.y + (k != "top" ? n.height : -selectorSize));
                        f.css("height", selectorSize);
                        f.css("width", d)
                    } else {
                        f.css("left", j.left + 1 + n.x + (k != "right" ? -selectorSize : n.width));
                        f.css("top", j.top);
                        f.css("height", d);
                        f.css("width", selectorSize);
                        n.height = selectorSize
                    }
                } else {
                    f.css({
                        width: m.width(),
                        height: m.height()
                    });
                    n.width = m.width();
                    n.height = m.height()
                }
                this._refreshSelector(p, e, o, f, n, h)
            }
            this._isSelectorRefresh = false;
            return true
        },
        _refreshSelector: function(h, f, e, E, d, c) {
            var G = {};
            var w = f.rangeSelector;
            var m = this.seriesGroups[h];
            for (var A in w) {
                G[A] = w[A]
            }
            var t = G.minValue;
            var z = G.maxValue;
            if (undefined == t) {
                t = Math.min(e.min.valueOf(), e.dsRange.min.valueOf())
            }
            if (undefined == z) {
                z = Math.max(e.max.valueOf(), e.dsRange.max.valueOf())
            }
            if (this._isDate(e.min)) {
                t = new Date(t)
            }
            if (this._isDate(e.max)) {
                z = new Date(z)
            }
            var n = f.position;
            if (w.renderTo && w.position) {
                n = w.position
            }
            G.dataField = f.dataField;
            G.rangeSelector = undefined;
            G.type = f.type;
            G.baseUnit = w.baseUnit || f.baseUnit;
            G.minValue = t;
            G.maxValue = z;
            G.flip = f.flip;
            G.position = n;
            var l = 5;
            var s = 2,
                D = 2,
                C = 2,
                H = 2;
            if (!w.renderTo) {
                s = c ? 0 : d.x;
                D = c ? 0 : this._rect.width - d.x - d.width;
                C = c ? d.y : l;
                H = c ? this._paddedRect.height - this._plotRect.height : l
            }
            var p = w.padding;
            if (p == undefined && !w.renderTo) {
                p = {
                    left: s,
                    top: C,
                    right: D,
                    bottom: H
                }
            } else {
                p = {
                    left: ((p && p.left) ? p.left : s),
                    top: ((p && p.top) ? p.top : C),
                    right: ((p && p.right) ? p.right : D),
                    bottom: ((p && p.bottom) ? p.bottom : H)
                }
            }
            var v = f.rangeSelector.dataField;
            for (var A = 0; undefined == v && A < this.seriesGroups.length; A++) {
                for (var u = 0; undefined == v && u < this.seriesGroups[A].series.length; u++) {
                    v = this.seriesGroups[A].series[u].dataField
                }
            }
            var o = {
                padding: p,
                title: w.title || "",
                description: w.description || "",
                titlePadding: w.titlePadding,
                colorScheme: w.colorScheme || this.colorScheme,
                backgroundColor: w.backgroundColor || this.backgroundColor || "transparent",
                backgroundImage: w.backgroundImage || "",
                showBorderLine: w.showBorderLine || (w.renderTo ? true : false),
                borderLineWidth: w.borderLineWidth || this.borderLineWidth,
                borderLineColor: w.borderLineColor || this.borderLineColor,
                rtl: w.rtl || this.rtl,
                greyScale: w.greyScale || this.greyScale,
                showLegend: false,
                enableAnimations: false,
                enableEvents: false,
                showToolTips: false,
                source: this.source,
                xAxis: G,
                seriesGroups: [{
                    orientation: c ? "horizontal" : "vertical",
                    valueAxis: {
                        visible: false
                    },
                    type: f.rangeSelector.serieType || "area",
                    series: [{
                        dataField: v,
                        opacity: 0.8,
                        lineWidth: 1
                    }]
                }]
            };
            E.empty();
            E.jqxChart(o);
            var q = this;
            E.on(q._getEvent("mousemove"), function() {
                q._unselect();
                q._hideToolTip()
            });
            var B = E.jqxChart("getInstance");
            if (!B._plotRect) {
                return
            }
            var F = B._paddedRect;
            F.height = B._plotRect.height;
            if (!c && n == "top") {
                F.y += B._renderData[0].xAxis.rect.height
            } else {
                if (c) {
                    var k = B._renderData[0].xAxis.rect.width;
                    F.width -= k;
                    if (n != "right") {
                        F.x += k
                    }
                }
            }
            q._createSliderElements(h, E, F, w);
            q.addHandler(a(document), q._getEvent("mousemove"), q._onSliderMouseMove, {
                self: this,
                groupIndex: h,
                renderTo: E,
                swapXY: c
            });
            q.addHandler(a(document), q._getEvent("mousedown"), q._onSliderMouseDown, {
                self: this,
                groupIndex: h,
                renderTo: E,
                swapXY: c
            });
            q.addHandler(a(document), q._getEvent("mouseup"), q._onSliderMouseUp, {
                self: this,
                groupIndex: h,
                renderTo: E,
                swapXY: c
            })
        },
        _createSliderElements: function(u, n, o, p) {
            n.find(".slider").remove();
            var t = p.colorSelectedRange || "blue";
            var i = p.colorUnselectedRange || "white";
            var c = a("<div class='slider' style='position: absolute;'></div>");
            c.css({
                background: t,
                opacity: 0.1,
                left: o.x,
                top: o.y,
                width: o.width,
                height: o.height
            });
            c.appendTo(n);
            if (!this._sliders) {
                this._sliders = []
            }
            while (this._sliders.length < u + 1) {
                this._sliders.push({})
            }
            var l = "<div class='slider' style='position: absolute;  background: " + i + "; opacity: 0.5;'></div>";
            var d = "<div class='slider' style='position: absolute; background: grey; opacity: 0.5;'></div>";
            var j = "<div class='slider jqx-rc-all' style='position: absolute; background: white; border-style: solid; border-width: 1px; border-color: grey;'></div>";
            this._sliders[u] = {
                element: c,
                host: n,
                fullRect: {
                    x: c.coord().left,
                    y: c.coord().top,
                    width: o.width,
                    height: o.height
                },
                rect: o,
                left: a(l),
                right: a(l),
                leftTop: a(d),
                rightTop: a(d),
                leftBorder: a(d),
                leftBar: a(j),
                rightBorder: a(d),
                rightBar: a(j)
            };
            this._sliders[u].left.appendTo(n);
            this._sliders[u].right.appendTo(n);
            this._sliders[u].leftTop.appendTo(n);
            this._sliders[u].rightTop.appendTo(n);
            this._sliders[u].leftBorder.appendTo(n);
            this._sliders[u].rightBorder.appendTo(n);
            this._sliders[u].leftBar.appendTo(n);
            this._sliders[u].rightBar.appendTo(n);
            var s = this._renderData[u].xAxis;
            var f = s.data.axisStats;
            var q = f.min.valueOf();
            var h = f.max.valueOf();
            var k = this._valueToOffset(u, q);
            var m = this._valueToOffset(u, h);
            if (k > m) {
                var e = m;
                m = k;
                k = e
            }
            if (this.seriesGroups[u].orientation != "horizontal") {
                c.css({
                    left: Math.round(o.x + k),
                    top: o.y,
                    width: Math.round(m - k),
                    height: o.height
                })
            } else {
                c.css({
                    top: Math.round(o.y + k),
                    left: o.x,
                    height: Math.round(m - k),
                    width: o.width
                })
            }
            this._setSliderPositions(u, k, m)
        },
        _setSliderPositions: function(f, t, i) {
            var v = this.seriesGroups[f];
            var e = this._getCategoryAxis(f);
            var p = e.rangeSelector;
            var c = v.orientation == "horizontal";
            if (e.rangeSelector.renderTo) {
                c = false
            }
            var k = e.position;
            if (p.renderTo && p.position) {
                k = p.position
            }
            var m = (c && k == "right") || (!c && k == "top");
            var o = this._sliders[f];
            var s = c ? "top" : "left";
            var h = c ? "left" : "top";
            var j = c ? "height" : "width";
            var q = c ? "width" : "height";
            var l = c ? "y" : "x";
            var n = c ? "x" : "y";
            var d = o.rect;
            o.left.css(s, d[l]);
            o.left.css(h, d[n]);
            o.left.css(j, t);
            o.left.css(q, d[q]);
            o.right.css(s, d[l] + i);
            o.right.css(h, d[n]);
            o.right.css(j, d[j] - i + 1);
            o.right.css(q, d[q]);
            o.leftTop.css(s, d[l]);
            o.leftTop.css(h, d[n] + (((c && k == "right") || (!c && k != "top")) ? 0 : d[q]));
            o.leftTop.css(j, t);
            o.leftTop.css(q, 1);
            o.rightTop.css(s, d[l] + i);
            o.rightTop.css(h, d[n] + (((c && k == "right") || (!c && k != "top")) ? 0 : d[q]));
            o.rightTop.css(j, d[j] - i + 1);
            o.rightTop.css(q, 1);
            o.leftBorder.css(s, d[l] + t);
            o.leftBorder.css(h, d[n]);
            o.leftBorder.css(j, 1);
            o.leftBorder.css(q, d[q]);
            var u = d[q] / 4;
            if (u > 20) {
                u = 20
            }
            if (u < 3) {
                u = 3
            }
            o.leftBar.css(s, d[l] + t - 3);
            o.leftBar.css(h, d[n] + d[q] / 2 - u / 2);
            o.leftBar.css(j, 5);
            o.leftBar.css(q, u);
            o.rightBorder.css(s, d[l] + i);
            o.rightBorder.css(h, d[n]);
            o.rightBorder.css(j, 1);
            o.rightBorder.css(q, d[q]);
            o.rightBar.css(s, d[l] + i - 3);
            o.rightBar.css(h, d[n] + d[q] / 2 - u / 2);
            o.rightBar.css(j, 5);
            o.rightBar.css(q, u)
        },
        _resizeState: {},
        _onSliderMouseDown: function(e) {
            var c = e.data.self;
            var d = c._sliders[e.data.groupIndex];
            if (!d) {
                return
            }
            if (c._resizeState.state == undefined) {
                c._testAndSetReadyResize(e)
            }
            if (c._resizeState.state != "ready") {
                return
            }
            c._resizeState.state = "resizing"
        },
        _valueToOffset: function(o, m) {
            var n = this.seriesGroups[o];
            var e = this._sliders[o];
            var d = e.host.jqxChart("getInstance");
            var p = d._renderData[0].xAxis;
            var i = p.data.axisStats;
            var l = i.min.valueOf();
            var c = i.max.valueOf();
            var j = c - l;
            if (j == 0) {
                j = 1
            }
            var f = this._getCategoryAxis(o);
            var h = n.orientation == "horizontal" ? "height" : "width";
            var k = (m.valueOf() - l) / j;
            return e.fullRect[h] * (f.flip ? (1 - k) : k)
        },
        _offsetToValue: function(q, h) {
            var e = this._sliders[q];
            var p = this.seriesGroups[q];
            var f = this._getCategoryAxis(q);
            var i = p.orientation == "horizontal" ? "height" : "width";
            var k = e.fullRect[i];
            if (k == 0) {
                k = 1
            }
            var l = h / k;
            var d = e.host.jqxChart("getInstance");
            var o = d._renderData[0].xAxis;
            var j = o.data.axisStats;
            var m = j.min.valueOf();
            var c = j.max.valueOf();
            var n = h / k * (c - m) + m;
            if (f.flip == true) {
                n = c - h / k * (c - m)
            }
            if (this._isDate(j.min) || this._isDate(j.max)) {
                n = new Date(n)
            } else {
                if (f.dataField == undefined) {
                    n = Math.round(n)
                }
                if (n < j.min) {
                    n = j.min
                }
                if (n > j.max) {
                    n = j.max
                }
            }
            return n
        },
        _onSliderMouseUp: function(q) {
            var l = q.data.self;
            var h = q.data.groupIndex;
            var c = q.data.swapXY;
            var n = l._sliders[h];
            if (!n) {
                return
            }
            if (l._resizeState.state != "resizing") {
                return
            }
            l._resizeState = {};
            l.host.css("cursor", "default");
            var i = !c ? "left" : "top";
            var d = !c ? "width" : "height";
            var p = !c ? "x" : "y";
            var o = n.element.coord()[i];
            var e = o + (!c ? n.element.width() : n.element.height());
            var j = l._offsetToValue(h, o - n.fullRect[p]);
            var s = l._offsetToValue(h, e - n.fullRect[p]);
            var k = n.host.jqxChart("getInstance");
            var m = k._renderData[0].xAxis;
            var u = m.data.axisStats;
            if (!u.isTimeUnit && (s.valueOf() - j.valueOf()) > 86400000) {
                j.setHours(0, 0, 0, 0);
                s.setDate(s.getDate() + 1);
                s.setHours(0, 0, 0, 0)
            }
            var f = l._getCategoryAxis(h);
            if (f.flip) {
                var t = j;
                j = s;
                s = t
            }
            f.minValue = j;
            f.maxValue = s;
            l._isSelectorRefresh = true;
            var v = l.enableAnimations;
            l.enableAnimations = false;
            l.update();
            l.enableAnimations = v
        },
        _onSliderMouseMove: function(w) {
            var q = w.data.self;
            var A = w.data.renderTo;
            var k = w.data.groupIndex;
            var t = q._sliders[k];
            var e = w.data.swapXY;
            if (!t) {
                return
            }
            var h = t.fullRect;
            var j = t.element;
            var B = a.jqx.position(w);
            var u = j.coord();
            var s = e ? "left" : "top";
            var o = !e ? "left" : "top";
            var i = e ? "width" : "height";
            var f = !e ? "width" : "height";
            var v = !e ? "x" : "y";
            if (q._resizeState.state == "resizing") {
                if (q._resizeState.side == "left") {
                    var p = Math.round(B[o] - u[o]);
                    var n = h[v];
                    if (u[o] + p >= n && u[o] + p <= n + h[f]) {
                        var l = parseInt(j.css(o));
                        var d = Math.max(2, (e ? j.height() : j.width()) - p);
                        j.css(f, d);
                        j.css(o, l + p)
                    }
                } else {
                    if (q._resizeState.side == "right") {
                        var c = e ? j.height() : j.width();
                        var p = Math.round(B[o] - u[o] - c);
                        var n = h[v];
                        if (u[o] + c + p >= n && u[o] + p + c <= n + h[f]) {
                            var d = Math.max(2, c + p);
                            j.css(f, d)
                        }
                    } else {
                        if (q._resizeState.side == "move") {
                            var c = e ? j.height() : j.width();
                            var l = parseInt(j.css(o));
                            var p = Math.round(B[o] - q._resizeState.startPos);
                            if (u[o] + p >= h[v] && u[o] + p + c <= h[v] + h[f]) {
                                q._resizeState.startPos = B[o];
                                j.css(o, l + p)
                            }
                        }
                    }
                }
                var z = parseInt(j.css(o)) - t.rect[v];
                var m = z + (e ? j.height() : j.width());
                q._setSliderPositions(k, z, m)
            } else {
                q._testAndSetReadyResize(w)
            }
        },
        _testAndSetReadyResize: function(c) {
            var t = c.data.self;
            var m = c.data.renderTo;
            var q = c.data.groupIndex;
            var d = t._sliders[q];
            var i = c.data.swapXY;
            var o = d.fullRect;
            var f = d.element;
            var h = a.jqx.position(c);
            var j = f.coord();
            var l = i ? "left" : "top";
            var s = !i ? "left" : "top";
            var k = i ? "width" : "height";
            var n = !i ? "width" : "height";
            var e = !i ? "x" : "y";
            var p = t._isTouchDevice ? 30 : 5;
            if (h[l] >= j[l] && h[l] <= j[l] + o[k]) {
                if (Math.abs(h[s] - j[s]) <= p) {
                    m.css("cursor", i ? "row-resize" : "col-resize");
                    t._resizeState = {
                        state: "ready",
                        side: "left"
                    }
                } else {
                    if (Math.abs(h[s] - j[s] - (!i ? f.width() : f.height())) <= p) {
                        m.css("cursor", i ? "row-resize" : "col-resize");
                        t._resizeState = {
                            state: "ready",
                            side: "right"
                        }
                    } else {
                        if (h[s] + p > j[s] && h[s] - p < j[s] + (!i ? f.width() : f.height())) {
                            m.css("cursor", "hand");
                            t._resizeState = {
                                state: "ready",
                                side: "move",
                                startPos: h[s]
                            }
                        } else {
                            m.css("cursor", "default");
                            t._resizeState = {}
                        }
                    }
                }
            } else {
                m.css("cursor", "default");
                t._resizeState = {}
            }
        },
        _selectorGetSize: function(c) {
            if (c.rangeSelector.renderTo) {
                return 0
            }
            return c.rangeSelector.size || this._paddedRect.height / 3
        }
    })
})(jqxBaseFramework);
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