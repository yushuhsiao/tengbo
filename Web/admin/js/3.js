                case "custom":
                        n.innerHTML = "";
                        var D = a.trim(G.datafield).split(" ").join("");
                        if (D.indexOf(".") != -1) {
                            D = D.replace(".", "")
                        }
                        var B = this.editors["customeditor_" + D + "_" + q];
                        d = B == undefined ? a("<div style='overflow: hidden; border-radius: 0px; -moz-border-radius: 0px; -webkit-border-radius: 0px; z-index: 99999; top: 0px; left: 0px; position: absolute;' id='customeditor'></div>") : B;
                        d.show();
                        d.css("top", a(n).parent().position().top);
                        if (this.oldhscroll) {
                            d.css("left", -i + parseInt(a(n).position().left))
                        } else {
                            d.css("left", parseInt(a(n).position().left))
                        } if (G.pinned) {
                            d.css("left", i + parseInt(a(n).position().left))
                        }
                        if (B == undefined) {
                            d.prependTo(this.table);
                            d[0].id = "customeditor" + this.element.id + D + "_" + q;
                            this.editors["customeditor_" + D + "_" + q] = d;
                            var b = g.width() - 1;
                            var e = g.height() - 1;
                            d.width(b);
                            d.height(e);
                            if (G.createeditor) {
                                G.createeditor(q, I, d, C, b, e, this.editchar)
                            }
                        }
                        if (G._requirewidthupdate) {
                            d.width(g.width() - 2)
                        }
                    break;