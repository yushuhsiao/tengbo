                case "dropdownlist":
                    if (this.host.jqxDropDownList) {
                        n.innerHTML = "";
                        var D = a.trim(G.datafield).split(" ").join("");
                        var A = a.trim(G.displayfield).split(" ").join("");
                        if (D.indexOf(".") != -1) {
                            D = D.replace(".", "")
                        }
                        if (A.indexOf(".") != -1) {
                            A = A.replace(".", "")
                        }
                        var k = this.editors["dropdownlist_" + D];
                        d = k == undefined ? a("<div style='border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='dropdownlisteditor'></div>") : k;
                        d.css("top", a(n).parent().position().top);
                        if (this.oldhscroll) {
                            d.css("left", -i + parseInt(a(n).position().left))
                        } else {
                            d.css("left", parseInt(a(n).position().left))
                        } if (G.pinned) {
                            d.css("left", i + parseInt(a(n).position().left))
                        }
                        if (k == undefined) {
                            d.prependTo(this.table);
                            d[0].id = "dropdownlisteditor" + this.element.id + D;
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
                            d.jqxDropDownList({
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
                            this.editors["dropdownlist_" + D] = d;
                            if (G.createeditor) {
                                G.createeditor(q, I, d)
                            }
                        }
                        if (G._requirewidthupdate) {
                            d.jqxDropDownList({
                                width: g.width() - 2
                            })
                        }
                        var c = d.jqxDropDownList("listBox").visibleItems;
                        if (!G.createeditor) {
                            if (c.length < 8) {
                                d.jqxDropDownList("autoDropDownHeight", true)
                            } else {
                                d.jqxDropDownList("autoDropDownHeight", false)
                            }
                        }
                        var I = this.getcellvalue(q, A);
                        var z = this.findRecordIndex(I, A, c);
                        if (K) {
                            if (I != "") {
                                d.jqxDropDownList("selectIndex", z, true)
                            } else {
                                d.jqxDropDownList("selectIndex", -1)
                            }
                        }
                        if (!this.editcell) {
                            return
                        }
                        if (this.editcell.defaultvalue != undefined) {
                            d.jqxDropDownList("selectIndex", this.editcell.defaultvalue, true)
                        }
                        if (w) {
                            d.jqxDropDownList("focus")
                        }
                    }
                    break;