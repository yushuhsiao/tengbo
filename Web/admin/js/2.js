                case "combobox":
                    if (this.host.jqxComboBox) {
                        n.innerHTML = "";
                        var D = a.trim(G.datafield).split(" ").join("");
                        var A = a.trim(G.displayfield).split(" ").join("");
                        if (D.indexOf(".") != -1) {
                            D = D.replace(".", "")
                        }
                        if (A.indexOf(".") != -1) {
                            A = A.replace(".", "")
                        }
                        var r = this.editors["combobox_" + D];
                        d = r == undefined ? a("<div style='border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='comboboxeditor'></div>") : r;
                        d.css("top", a(n).parent().position().top);
                        if (this.oldhscroll) {
                            d.css("left", -i + parseInt(a(n).position().left))
                        } else {
                            d.css("left", parseInt(a(n).position().left))
                        } if (G.pinned) {
                            d.css("left", i + parseInt(a(n).position().left))
                        }
                        if (r == undefined) {
                            d.prependTo(this.table);
                            d[0].id = "comboboxeditor" + this.element.id + D;
                            var f = this.source._source ? true : false;
                            var x = null;
                            if (!f) {
                                x = new a.jqx.dataAdapter(this.source, {
                                    autoBind: false,
                                    uniqueDataFields: [A],
                                    async: false,
                                    autoSort: true,
                                    autoSortField: A
                                })
                            } else {
                                var p = {
                                    localdata: this.source.records,
                                    datatype: this.source.datatype,
                                    async: false
                                };
                                x = new a.jqx.dataAdapter(p, {
                                    autoBind: false,
                                    async: false,
                                    uniqueDataFields: [A],
                                    autoSort: true,
                                    autoSortField: A
                                })
                            }
                            var u = !G.createeditor ? true : false;
                            d.jqxComboBox({
                                enableBrowserBoundsDetection: true,
                                keyboardSelection: false,
                                source: x,
                                rtl: this.rtl,
                                autoDropDownHeight: u,
                                theme: this.theme,
                                width: g.width() - 2,
                                height: g.height() - 2,
                                displayMember: A,
                                valueMember: E
                            });
                            d.removeAttr("tabindex");
                            d.find("div").removeAttr("tabindex");
                            this.editors["combobox_" + D] = d;
                            if (G.createeditor) {
                                G.createeditor(q, I, d)
                            }
                        }
                        if (G._requirewidthupdate) {
                            d.jqxComboBox({
                                width: g.width() - 2
                            })
                        }
                        var c = d.jqxComboBox("listBox").visibleItems;
                        if (!G.createeditor) {
                            if (c.length < 8) {
                                d.jqxComboBox("autoDropDownHeight", true)
                            } else {
                                d.jqxComboBox("autoDropDownHeight", false)
                            }
                        }
                        var I = this.getcellvalue(q, A);
                        var z = this.findRecordIndex(I, A, c);
                        if (K) {
                            if (I != "") {
                                d.jqxComboBox("selectIndex", z, true);
                                d.jqxComboBox("val", I)
                            } else {
                                d.jqxComboBox("selectIndex", -1);
                                d.jqxComboBox("val", I)
                            }
                        }
                        if (!this.editcell) {
                            return
                        }
                        if (this.editcell.defaultvalue != undefined) {
                            d.jqxComboBox("selectIndex", this.editcell.defaultvalue, true)
                        }
                        if (this.editchar && this.editchar.length > 0) {
                            d.jqxComboBox("input").val(this.editchar)
                        }
                        if (w) {
                            setTimeout(function() {
                                l(d.jqxComboBox("input"));
                                d.jqxComboBox("_setSelection", 0, 0);
                                if (s.editchar) {
                                    d.jqxComboBox("_setSelection", 1, 1);
                                    s.editchar = null
                                } else {
                                    var P = d.jqxComboBox("input").val();
                                    d.jqxComboBox("_setSelection", 0, P.length)
                                }
                            }, 10)
                        }
                    }
                    break;