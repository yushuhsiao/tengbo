(function () {
    function a() { } a.prototype = {
        DomWrap: null, DomLink: null, DomConcat: null, DomBox: null, DomContent: null, DomErrorTip: null, DomSubmit: null, getLastSendTime: function () {
            var f = $S.Cookie.get("TKshishicai_FeedBack");
            var e = new Date().addDays(-1);
            if (f != null) {
                e = f.toString().toDate()
            }
            return e
        }
        , setSendTime: function () {
            $S.Cookie.set("TKshishicai_FeedBack", $S.Date.getServerTime().toFormatString("{#yyyy#}-{#MM#}-{#dd#} {#HH#}:{#mm#}:{#ss#}"), 3600)
        }
        , errorTip: function (e) {
            $(this.DomErrorTip).html(e)
        }
        , response: function (e) {
            var f = new $S.JsonCommand().parse(JSON.parse(e));
            switch (f.Command) {
                case 0: this.setSendTime();
                    $.artDialog.tkAlert("反馈成功！", true);
                    this.errorTip("");
                    $(this.DomContent).val("");
                    $(this.DomConcat).val("");
                    this.display();
                    break;
                default: this.errorTip(f.Remark);
                    break
            }
            $(this.DomSubmit).removeClass("disabled")
        }
        , sendRequest: function () {
            if (($S.Date.getServerTime() - this.getLastSendTime()).toLeftTime().TotalSeconds <= 60) {
                this.errorTip("一分钟只能提交一次");
                return
            }
            var e = $.trim($(this.DomContent).val());
            if (e == $(this.DomContent).attr("placeholder")) {
                e = ""
            }
            if (e.length < 20) {
                this.errorTip("内容长度不得低于20");
                return
            }
            if ($(this.DomSubmit).hasClass("disabled")) {
                return
            }
            $(this.DomSubmit).addClass("disabled");
            $.ajax({
                url: "/handler/feedback.ashx", type: "post", data: {
                    type: "feedback", qq: $.trim($(this.DomConcat).val()), content: e
                }
                , success: this.response.bind(this), error: function (f) { }
            })
        }
        , display: function () {
            if (TK.User.IsLogined) {
                $(this.DomConcat).hide()
            }
            $(this.DomBox).toggle();
            $(this.DomLink).toggleClass("on")
        }
        , autoresize: function () {
            $(this.DomWrap).css("left", (parseFloat($(".main").offset().left) + parseFloat($(".main").width())))
        }
        , createDom: function () {
            if (this.DomWrap != null) {
                return
            }
            var e = [];
            e.push('<div class="feedback">');
            e.push('<a class="feed">意见反馈</a>');
            e.push('<div class="feedbkPop">');
            e.push('<textarea class="grey" placeholder="功能服务，产品建议请在此提出建议，其他查询问题请联系在线客服，谢谢。"></textarea>');
            e.push("<p>");
            e.push('<input type="text" value="" class="grey" placeholder="您的联系QQ（选填）" />');
            e.push("<button>提意见</button>");
            e.push("<span></span>");
            e.push("</p>");
            e.push("</div>");
            e.push("</div>");
            this.DomWrap = $(e.join("")).appendTo("div.content");
            this.DomLink = $(this.DomWrap).find("a.feed").click(this.display.bind(this));
            this.DomContent = $(this.DomWrap).find("textarea");
            this.DomConcat = $(this.DomWrap).find("input[type=text]");
            this.DomErrorTip = $(this.DomWrap).find("span");
            this.DomSubmit = $(this.DomWrap).find("button").click(this.sendRequest.bind(this));
            this.DomBox = $(this.DomWrap).find("div.feedbkPop");
            $S.PlaceHolder.init();
            this.autoresize();
            $(window).bind("resize", this.autoresize.bind(this))
        }
        , initialize: function () {
            this.createDom();
            return this
        }

    };
    TK.FeedBack = new a().initialize();
    function b() { } b.prototype = {
        DomWrap: null, DomLink: null, DomMask: null, display: function () {
            $(document).scrollTop(0);
            this.autoresize();
            if (this.DomWrap == null) {
                this.createDom();
                return
            }
            $(this.DomWrap).show()
        }
        , close: function () {
            $(this.DomWrap).hide()
        }
        , autoresize: function () {
            $(this.DomWrap).filter("div.newhelpWrap").css("left", (parseFloat($(".h_top_l").offset().left) + 56))
        }
        , createDom: function () {
            if (this.DomWrap != null) {
                return
            }
            var e = [];
            var f = TK.Url.pay.toUrl().host;
            e.push('<div class="newhelpWrap" style="z-index:100002;">');
            e.push('<div class="nhDialog">');
            e.push('<a class="x" title="关闭">');
            e.push('<a class="more" target="_blank" href="http://{0}/help/2/">更多帮助</a>');
            e.push('<a class="contact qq" href="tencent://message/?uin=800019790&Site=www.shishicai.cn&Menu=yes" onclick="javascript:window.open(\'http://b.qq.com/webc.htm?new=0&sid=800019790&eid=&o=www.shishicai.cn&q=7&ref=' + document.location + "', '_blank', 'height=544, width=644,toolbar=no,scrollbars=no,menubar=no,status=no');return false;\">在线客服</a>");
            e.push('<span class="text_L"><b>由此快速进入</b><br>买彩票&nbsp;&nbsp;&nbsp;看走势&nbsp;&nbsp;&nbsp;查遗漏</span>');
            e.push('<span class="text_R"><b>由此快速进入</b><br>注册&nbsp;&nbsp;&nbsp;充值&nbsp;&nbsp;&nbsp;提现</span>');
            e.push('<div class="cont_box">');
            e.push('<ul class="t_A"><li><a target="_blank" href="http://{0}/help/2/2/">如何成为时时彩会员</a></li></ul>');
            e.push('<ul class="t_B">');
            e.push('<li><a target="_blank" href="http://{0}/help/2/1/">如何充值？</a></li>');
            e.push('<li><a target="_blank" href="http://{0}/help/2/3/">如何提现？</a></li>');
            e.push("</ul>");
            e.push('<ul class="t_C">');
            e.push('<li><a target="_blank" href="http://{0}/help/2/6/">如何购买彩票</a></li>');
            e.push('<li><a target="_blank" href="http://{0}/help/3/10/">时时彩玩法（赔率）</a></li>');
            e.push('<li><a target="_blank" href="http://{0}/help/3/14/">11选5玩法介绍</a></li>');
            e.push('<li><a target="_blank" href="http://{0}/help/2/8/">如何追号？</a></li>');
            e.push("</ul>");
            e.push('<ul class="t_D">');
            e.push('<li><a target="_blank" href="http://{0}/help/2/4/">中奖如何领奖</a></li>');
            e.push("</ul>");
            e.push("</div>");
            e.push("</div>");
            e.push("</div>");
            e.push('<div style="z-index:99999; position:fixed; left:0px; top:0px; width:100%; height:100%; overflow:hidden;background:#000000;-moz-opacity:0.5;opacity:0.5;filter:alpha(opacity=50);"></div>');
            this.DomWrap = $(String.format(e.join(""), f)).appendTo(document.body);
            $(this.DomWrap).find("a.x").click(this.close.bind(this));
            $(document).bind("keydown", function (g) {
                g = g || window.event;
                var h = g.keyCode || g.which;
                if (h == 27) {
                    this.close()
                }

            }
            .bind(this));
            this.autoresize();
            $(window).bind("resize", this.autoresize.bind(this))
        }
        , initialize: function () {
            this.DomLink = $('<a class="newhelp">新手帮助</a>').insertAfter(TK.FeedBack.DomLink).click(this.display.bind(this));
            var e = $("div.footer a.qq").click(this.display.bind(this));
            e.attr("href", "javascript:void(0)");
            e.removeAttr("onclick");
            return this
        }

    };
    TK.NewFish = new b().initialize();
    var c = $("a.bet"), d = c;
    if (c.length > 0) {
        while (d.tagName !== "DIV") {
            d = $(d).parent().get(0)
        }
        $('<p class="Accept"><input onclick="return false" type="checkbox" checked="checked">我已经阅读并同意 《<a href="/help/2/22/" target="_blank">电话投注委托规则</a>》</p>').appendTo(d)
    }

})();
