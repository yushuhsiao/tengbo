(function (d, c) {
    var a = d.$S;
    var b = function () {
        var g = "/media/core.swf";
        var f = "$S.Flash.Core.swfLoaded";
        var e = this;
        this.SWF = null;
        this.Handler_loaded = "Handler_loaded";
        this.Loaded = false;
        this.setPath = function (h) {
            g = h;
            return this
        };
        this.init = function (h) {
            if (typeof (h) == "function") {
                $(this).bind(this.Handler_loaded, h)
            }
            if (e.SWF == null) {
                if (document.body == null) {
                    $(document).ready(e.init.bind(e));
                    return
                }
                e.SWF = $(a.Flash.create({
                    Id: "SWFCore", Width: 1, Height: 1, Movie: g + "?loaded=" + f + "&r=" + new Date().getTime()
                })).appendTo(document.body).get(0)
            }

        };
        this.swfLoaded = function () {
            this.Loaded = true;
            setTimeout(function () {
                try {
                    $(e).triggerHandler(e.Handler_loaded).unbind(e.Handler_loaded)
                }
                catch (h) { }
            }
            , 0)
        };
        this.Timer = {
            create: function (h) {
                return e.SWF.Timer_Create(h)
            }
            , run: function (j, i, k, h) {
                if (!e.Loaded) {
                    e.init(this.run.bind(this, j, i, k, h));
                    return
                }
                var l = this.create(j);
                h.push(this.runById(l, i, k))
            }
            , runById: function (j, h, i) {
                e.SWF.Timer_Start(j, h, i);
                return j
            }
            , stop: function (h) {
                e.SWF.Timer_Stop(h);
                return h
            }

        };
        this.Sound = {
            create: function (h) {
                return e.SWF.Sound_Create(h)
            }
            , play: function (i, j, h) {
                if (!e.Loaded) {
                    e.init(this.play.bind(this, i, j, h));
                    return
                }
                var k = this.create(i);
                h.push(this.playById(k, j))
            }
            , playById: function (i, h) {
                e.SWF.Sound_Play(i, (h || 1));
                return i
            }
            , stopById: function (h) {
                e.SWF.Sound_Stop(h);
                return h
            }

        };
        this.Socket = {
            create: function (h) {
                a.Debug.log("socket", "创建一个socket客户端对象");
                var i = e.SWF.Socket_Create();
                if (typeof (h) != "undefined") {
                    i.Javascript_connected = h.instance + ".event_connected";
                    i.Javascript_closed = h.instance + ".event_closed";
                    i.Javascript_getMsg = h.instance + ".event_receive";
                    i.Javascript_getMsgError = h.instance + ".event_receiveError";
                    i.Javascript_ioError = h.instance + ".event_ioError";
                    i.Javascript_securityError = h.instance + ".event_securityError"
                }
                return i
            }
            , connect: function (i, j, h) {
                if (!e.Loaded) {
                    e.init(this.connect.bind(this, i, j, h));
                    return
                }
                var k = this.create(h);
                k.Connect_IP = i;
                k.Connect_Port = j;
                h.swf = this.connectByIns(k)
            }
            , connectByIns: function (j, h, i) {
                j.Connect_IP = h || j.Connect_IP;
                j.Connect_Port = i || j.Connect_Port;
                a.Debug.log("socket(" + j.Connect_IP + ":" + j.Connect_Port + ")", "正在连接到服务器……");
                e.SWF.Socket_Connect(j);
                return j
            }
            , close: function (h) {
                a.Debug.log("socket(" + h.socket.Connect_IP + ":" + h.Connect_Port + ")", "从服务器关闭连接.");
                e.SWF.Socket_Close(h);
                return h
            }
            , send: function (i, h) {
                e.SWF.Socket_Send(i, h);
                return i
            }
            , getConnectedStatus: function (h) {
                return e.SWF.Socket_GetConnectedStatus(h)
            }

        }

    };
    a.Flash.Core = new b();
    a.SWFTimer = a.Flash.Core.Timer;
    a.SWFSound = a.Flash.Core.Sound;
    a.SWFSocket = a.Flash.Core.Socket;
    a.SocketHandler = function () {
        this.Connected = "Connected";
        this.Closed = "Closed";
        this.Receive = "Receive";
        this.ReceiveError = "ReceiveError";
        this.IoError = "IoError";
        this.SecurityError = "SecurityError"
    };
    a.Socket = function (f, g, h) {
        var e = "socket(" + g + ":" + h + ")";
        this.instance = f;
        this.swf = null;
        this.AutoConnected = true;
        this.Connectting = false;
        this.HeartSeonds = 5000;
        this.Handler = new a.SocketHandler();
        this.setConnectInfo = function (i, j) { };
        this.status = function () {
            return this.swf != null && a.SWFSocket.getConnectedStatus(this.swf)
        };
        this.close = function () {
            if (this.swf == null) {
                return
            }
            a.Debug.log(e, "从客户端断开服务器连接");
            a.SWFSocket.close(this.swf)
        };
        this.send = function (i) {
            a.Debug.log(e, "向服务器发送数据" + i);
            a.SWFSocket.send(this.swf, i + ";")
        };
        this.connect = function () {
            if (this.Connectting) {
                return
            }
            this.Connectting = true;
            if (this.swf == null) {
                a.SWFSocket.connect(g, h, this)
            }
            else {
                a.SWFSocket.connectByIns(this.swf)
            }

        };
        this.heart = function () {
            var i = this.status();
            setTimeout(this.heart.bind(this), this.HeartSeonds);
            if (i || !this.AutoConnected) {
                return
            }
            this.connect()
        };
        this.heartlog = function () {
            var i = this.status();
            a.Debug.log(e, "心跳检测状态：" + i + "，自动连接：" + this.AutoConnected)
        }
        , this.init = function () {
            this.heart();
            if (!this.AutoConnected) {
                this.connect()
            }

        };
        this.event_connected = function (i) {
            this.Connectting = false;
            a.Debug.log(e, "建立连接返回：" + i.toString());
            $(this).trigger(this.Handler.Connected, i)
        };
        this.event_closed = function () {
            a.Debug.log(e, "服务器意外关闭连接");
            this.close();
            $(this).trigger(this.Handler.Closed)
        };
        this.event_receive = function (i) {
            a.Debug.log(e, "接收服务器数据：" + i);
            $(this).trigger(this.Handler.Receive, i)
        };
        this.event_receiveError = function (i) {
            a.Debug.log(e, "接收服务器数据出错：" + JSON.stringify(i));
            $(this).trigger(this.Handler.ReceiveError, i)
        };
        this.event_ioError = function (i) {
            a.Debug.log(e, "IO错误：" + JSON.stringify(i));
            $(this).trigger(this.Handler.IoError, i)
        };
        this.event_securityError = function (i) {
            this.Connectting = false;
            a.Debug.log(e, "Security错误：" + JSON.stringify(i));
            $(this).trigger(this.Handler.IoError, i)
        }

    }

})(window);
$.fn.extend({
    mousewheel: function (a) {
        return this.each(function () {
            var b = this;
            b.D = 0;
            if ($.browser.msie || $.browser.safari) {
                b.onmousewheel = function () {
                    b.D = event.wheelDelta;
                    event.returnValue = false;
                    a && a.call(b)
                }

            }
            else {
                b.addEventListener("DOMMouseScroll", function (c) {
                    b.D = c.detail > 0 ? -1 : 1;
                    c.preventDefault();
                    a && a.call(b)
                }
                , false)
            }

        })
    }

});
$.fn.extend({
    jscroll: function (b) {
        b = b || {};
        b.Fun = b.Fun || function () { };
        b.Top = b.Top || 0;
        b.Height = b.Height || 200;
        b.EnableMaxHeight = b.EnableMaxHeight || false;
        b.Animate = b.Animate || false;
        b.Bar = b.Bar || {};
        b.Btn = b.Btn || {};
        b.Bar.Bg = b.Bar.Bg || {};
        b.Bar.Bd = b.Bar.Bd || {};
        b.Btn.uBg = b.Btn.uBg || {};
        b.Btn.dBg = b.Btn.dBg || {};
        var c = {
            W: 11, BgUrl: "", Bg: "", Bar: {
                Pos: "up", Border: "", Bd: {
                    Out: "", Hover: ""
                }
                , Bg: {
                    Out: "", Hover: "", Focus: ""
                }

            }
            , Btn: {
                btn: false, uBg: {
                    Out: "#2a2a2a", Hover: "orange", Focus: "orange"
                }
                , dBg: {
                    Out: "#2a2a2a", Hover: "orange", Focus: "orange"
                }

            }
            , Fn: function () { }
        };
        b.W = b.W || c.W;
        b.BgUrl = b.BgUrl || c.BgUrl;
        b.Bg = b.Bg || c.Bg;
        b.Bar.Pos = b.Bar.Pos || c.Bar.Pos;
        b.Bar.Border = b.Bar.Border || c.Bar.Border;
        b.Bar.Bd.Out = b.Bar.Bd.Out || c.Bar.Bd.Out;
        b.Bar.Bd.Hover = b.Bar.Bd.Hover || c.Bar.Bd.Hover;
        b.Bar.Bg.Out = b.Bar.Bg.Out || c.Bar.Bg.Out;
        b.Bar.Bg.Hover = b.Bar.Bg.Hover || c.Bar.Bg.Hover;
        b.Bar.Bg.Focus = b.Bar.Bg.Focus || c.Bar.Bg.Focus;
        b.Btn.btn = b.Btn.btn != undefined ? b.Btn.btn : c.Btn.btn;
        b.Btn.uBg.Out = b.Btn.uBg.Out || c.Btn.uBg.Out;
        b.Btn.uBg.Hover = b.Btn.uBg.Hover || c.Btn.uBg.Hover;
        b.Btn.uBg.Focus = b.Btn.uBg.Focus || c.Btn.uBg.Focus;
        b.Btn.dBg.Out = b.Btn.dBg.Out || c.Btn.dBg.Out;
        b.Btn.dBg.Hover = b.Btn.dBg.Hover || c.Btn.dBg.Hover;
        b.Btn.dBg.Focus = b.Btn.dBg.Focus || c.Btn.dBg.Focus;
        b.Fn = b.Fn || c.Fn;
        var a = this;
        if (b.EnableMaxHeight) {
            $(this).css({
                height: "auto"
            });
            if ($(this).height() > b.Height) {
                $(this).css({
                    height: b.Height
                })
            }

        }
        else {
            $(this).css({
                height: b.Height
            })
        }
        return this.each(function () {
            var A, z = 0, l = 0;
            $(a).css({
                overflow: "hidden", position: "relative", padding: "0px"
            });
            var j = $(a).width(), i = $(a).height();
            var B = b.W ? parseInt(b.W, 10) : 21;
            var y = j - B;
            var f = b.Btn.btn == true ? B : 0;
            var k = $(a).children(".jscroll-c").height() == null;
            if (k) {
                $(a).wrapInner("<div class='jscroll-c' style='top:0px;z-index:9;zoom:1;position:relative'></div>");
                $(a).children(".jscroll-c").prepend("<div style='height:0px;overflow:hidden'></div>");
                $(a).append(String.format("<div class='jscroll-e' unselectable='on' style=' height:100%;top:0px;right:0;-moz-user-select:none;position:absolute;overflow:hidden;z-index:10;'><div class='jscroll-u' style='top:0px;'></div><div class='jscroll-h' unselectable='on'></div><div class='jscroll-d' style='position:absolute;bottom:0px;width:100%;left:0;{0}overflow:hidden'></div></div>", b.Btn.dBg.Out != "" ? "background:" + b.Btn.dBg.Out + ";" : ""))
            }
            var m = $(a).children(".jscroll-c");
            var o = $(a).children(".jscroll-e");
            var p = o.children(".jscroll-h");
            var q = o.children(".jscroll-u");
            var n = o.children(".jscroll-d");
            if ($.browser.msie) {
                document.execCommand("BackgroundImageCache", false, true)
            }
            var v = m.height();
            var x = (i - 2 * f) * i / v;
            if (x < 10) {
                x = 10
            }
            var C = x / 6;
            var h = 0, d = false;
            p.height(x);
            if (v <= b.Height) {
                o.css({
                    display: "none"
                })
            }
            else {
                d = true;
                var s = {
                    display: "block"
                };
                o.css(s);
                var t = {};
                var g = parseFloat(p.css("top"));
                if (k || isNaN(g)) {
                    t.top = f
                }
                else {
                    h = g
                }
                p.css(t);
                var u = {
                    height: f
                };
                q.css(u);
                var r = {
                    height: f
                };
                n.css(r);
                p.hover(function () { }, function () { });
                q.hover(function () { }, function () { });
                n.hover(function () { }, function () { })
            }
            b.Fun(v <= b.Height);
            switch (b.Bar.Pos) {
                case "up": h = 0;
                    w();
                    break;
                case "down": h = i - x - f;
                    w();
                    break;
                default: break
            }
            p.bind("mousedown", function (D) {
                b.Fn && b.Fn.call(a);
                l = 1;
                var E = D.pageY, F = parseInt($(this).css("top"));
                $(document).mousemove(function (G) {
                    h = F + G.pageY - E;
                    w()
                });
                $(document).mouseup(function () {
                    l = 0;
                    $(document).unbind()
                });
                return false
            });
            q.bind("mousedown", function (D) {
                b.Fn && b.Fn.call(a);
                l = 1;
                a.timeSetT("u");
                $(document).mouseup(function () {
                    l = 0;
                    $(document).unbind();
                    clearTimeout(A);
                    z = 0
                });
                return false
            });
            n.bind("mousedown", function (D) {
                b.Fn && b.Fn.call(a);
                l = 1;
                a.timeSetT("d");
                $(document).mouseup(function () {
                    l = 0;
                    $(document).unbind();
                    clearTimeout(A);
                    z = 0
                });
                return false
            });
            a.timeSetT = function (D) {
                var E = this;
                if (D == "u") {
                    h -= C
                }
                else {
                    h += C
                }
                w();
                z += 2;
                var F = 500 - z * 50;
                if (F <= 0) {
                    F = 0
                }
                A = setTimeout(function () {
                    E.timeSetT(D)
                }
                , F)
            };
            o.bind("mousedown", function (D) {
                b.Fn && b.Fn.call(a);
                h = h + D.pageY - p.offset().top - x / 2;
                if (b.Animate) {
                    e()
                }
                else {
                    w()
                }
                return false
            });
            function e() {
                if (h < f) {
                    h = f
                }
                if (h > i - x - f && i - x - f >= 0) {
                    h = i - x - f
                }
                p.stop().animate({
                    top: h
                }
                , 100);
                var D = -((h - f) * (v - i) / (i - 2 * f - x));
                m.stop().animate({
                    top: D
                }
                , 1000)
            }
            function w() {
                if (h < f) {
                    h = f
                }
                if (h > i - x - f && i - x - f >= 0) {
                    h = i - x - f
                }
                p.css({
                    top: h
                });
                var D = -((h - f) * (v - i) / (i - 2 * f - x));
                m.css({
                    top: D
                })
            }
            $(a).mousewheel(function () {
                if (d != true) {
                    return
                }
                b.Fn && b.Fn.call(a);
                if (this.D > 0) {
                    h -= C
                }
                else {
                    h += C
                }
                w()
            });
            if (b.Top > 0) {
                h = (b.Top * (i - 2 * f - x) / (v - i) + f);
                w()
            }

        })
    }

});
TK.PerMoney = 2;
(function (b, a) {
    if (typeof TK.Bet === "undefined") {
        TK.Bet = new (function () { })()
    }

})(window);
TK.Bet.BetMethod = {
    常规选号: 1, 文件选号: 2, 胆拖选号: 3, 走势选号: 4, 和值选号: 5, 随机选号: 6, 随心所欲: 7, 首页机选: 8, 专家推荐: 9, 频道机选: 10, 命中注定: 11, 我的选号: 12, 活动选号: 13, 秘书选号: 14
};
TK.Bet.TicketStatus = {
    待出票: 1, 正在发送: 2, 发送成功: 3, 购买成功: 4, 出票失败: 5, 用户撤单: 6, 系统撤单: 7, 中奖撤单: 8, 出号撤单: 9, 限号撤单: 10, 部分成功: 11
};
TK.LotteryType = {
    ssq双色球: 101, qlc七乐彩: 102, dlt大乐透: 103, jczq竞彩足球: 201, jclq竞彩篮球: 202, bjdc北京单场: 203, fc3d福彩3d: 151, qxc七星彩: 152, pl3排列三: 153, pl5排列五: 154, cqssc重庆时时彩: 4, jxssc江西时时彩: 5, xjssc新疆时时彩: 17, hljssc黑龙江时时彩: 21, tjssc天津时时彩: 27, hnssc湖南时时彩: 28, fjssc福建时时彩: 30, sd11x5山东11选5: 16, jx11x5江西11选5: 23, gd11x5广东11选5: 24, cq11x5重庆11选5: 36, tjkl10天津快乐十分: 15, gdkl10广东快乐十分: 22, xync重庆幸运农场: 33, gxkl10广西快乐十分: 34, shssl上海时时乐: 6, sdqyh山东群英会: 25, hnjlc湖南即乐彩: 26, bjkl8北京快乐8: 35, jsk3江苏快3: 3, weizhi未知: 255
};
TK.CurrentLotteryType = TK.LotteryType.weizhi未知;
TK.BetStatus = {};
TK.BetStatus.Enum = {
    销售: 1, 停售: 2
};
TK.BetStatus.parse = function (a) {
    a = a || {};
    return {
        LotteryType: a.l || TK.LotteryType.weizhi未知, Status: a.s || TK.BetStatus.Enum.销售, Remark: a.r || "暂停销售"
    }

};
if ($S.Debug.IntelliSense) {
    for (var lt in TK.LotteryType) {
        TK.BetStatus[TK.LotteryType[lt]] = TK.BetStatus.parse({
            l: TK.LotteryType[lt]
        })
    }

}
Number.prototype.toLotteryName = function () {
    var b = this;
    var d = b.toString();
    switch (b) {
        default: for (var a in TK.LotteryType) {
            if (TK.LotteryType[a] == b) {
                switch (parseInt(b, 10)) {
                    case 16: d = "十一运夺金";
                        break;
                    case 4: d = "时时彩";
                        break;
                    case 3: d = "快3";
                        break;
                    case 33: d = "快乐十分";
                        break;
                    default: var c = new RegExp("([^\u4E00-\u9FA5]*)(.*)");
                        d = a.replace(c, "$2");
                        break
                }
                break
            }

        }
            break
    }
    return d
};
TK.Bet.PlayType = {};
TK.Bet.PlayType.parse = function (a) { };
TK.Bet.createBetMethod = function (a, c, b) {
    var d = "";
    for (var e in TK.Bet.BetMethod) {
        if (TK.Bet.BetMethod[e] != TK.Bet.BetMethod.常规选号 && TK.Bet.BetMethod[e] == a) {
            d = e;
            break
        }

    }
    if (d == "") {
        for (var e in TK.Bet.PlayType) {
            if (TK.Bet.PlayType[e] == c) {
                d = e + (typeof b != "undefined" ? b : "选号");
                break
            }

        }

    }
    return d
};
TK.Bet.ConvertResultStatus = function (d) {
    var e = $.extend({
        ObjStatus: {}, DefaultText: null, OutputButton: []
    }
    , d);
    var b = e.ObjStatus, a = e.DefaultText, c = e.OutputButton;
    var f = a || "投注失败";
    if ($S.Debug.IntelliSense) {
        b = new $S.JsonCommand()
    }
    switch (b.Command) {
        case 102: f = "密码错误";
            break;
        case 200: f = "账户余额不足";
            c.push('<a class="Dbutt" href="' + TK.Url.pay + '" target="_blank"><span>充 值</span></a>');
            c.push('<a class="cancel"><span>关 闭</span></a>');
            break;
        case 300: f = "未登录或登录超时";
            break;
        case 301: case 302: f = "投注金额有误";
            break;
        case 303: f = "投注号码有误";
            break;
        case 304: f = "投注奖期已截止";
            break;
        case 305: f = "投注奖期重复";
            break;
        case 307: f = "暂停销售";
            break;
        default: f = f + "(" + b.Command + ")";
            break
    }
    if (typeof (d) == "object" && typeof (d.OutputButton) == "object") {
        d.OutputButton = c
    }
    return f
};
var MenuBet_ComputeBonusMoney = false;
TK.Bet.Service = function () { };
TK.Bet.Util = {};
TK.Bet.Util.BetItem = function () {
    this.PlayType = -1;
    this.PlayTypeName = "";
    this.BetNumber = "";
    this.BetCount = -1;
    this.BetMoney = -1;
    this.BetMethod = -1;
    this.Id = -1;
    this.Multiple = -1;
    this.TicketStatus = -1;
    this.BonusStatus = -1;
    this.BonusMoney = -1;
    this.ChildTicketCount = -1;
    this.SuccessCount = -1;
    this.FailCount = -1;
    this.parse = function (a) {
        this.Id = a.i;
        this.PlayType = a.p;
        try {
            this.PlayTypeName = TK.Bet.PlayType.parse(this.PlayType)
        }
        catch (b) { } this.BetNumber = a.c;
        this.Multiple = a.mt;
        this.BetMoney = a.m;
        this.TicketStatus = a.t;
        this.BonusStatus = a.b;
        this.BonusMoney = a.bm;
        this.ChildTicketCount = a.tc;
        this.SuccessCount = a.sc;
        this.FailCount = a.fc;
        return this
    };
    this.toJSONInstance = function () {
        return {
            p: this.PlayType, c: this.BetNumber, m: this.BetMoney, b: this.BetMethod
        }

    };
    this.init = function () {
        try {
            this.PlayTypeName = TK.Bet.PlayType.parse(this.PlayType)
        }
        catch (a) { } return this
    }

};
TK.Bet.Util.BetIssue = function () {
    this.IssueNumber = "";
    this.Multiple = 0;
    this.toJSONInstance = function () {
        return {
            i: this.IssueNumber, m: this.Multiple
        }

    }

};
TK.Bet.Util.Project = function () {
    this.initialize.bind(this);
    this.initialize.apply(this, arguments)
};
TK.Bet.Util.Project.prototype = {
    LotteryType: TK.LotteryType.weizhi未知, ListBetItem: [], ListBetIssue: [], TotalBetMoney: 0, DropBonusLevel: "", DropEarlyLevel: "", DropBonusMoney: 0, DropEarlyMoney: 0, toJSONInstance: function () {
        return {
            l: this.LotteryType, c: JSON.stringify(this.ListBetItem.toJSONAry()), i: JSON.stringify(this.ListBetIssue.toJSONAry()), m: this.TotalBetMoney, bl: this.DropBonusLevel, el: this.DropEarlyLevel
        }

    }
    , toJSONString: function () {
        return JSON.stringify(this.toJSONInstance())
    }
    , initialize: function () {
        this.ListBetIssue = [];
        this.ListBetItem = []
    }

};
TK.Bet.Util.WaitProject = function () {
    this.Id = "";
    this.ProjectNo = "";
    this.Money = 0;
    this.BetIssueCount = 0;
    this.WaitIssueCount = 0;
    this.WaitMoney = 0;
    this.prase = function (a) {
        this.Id = a.id;
        this.ProjectNo = a.n;
        this.Money = a.m;
        this.BetIssueCount = a.ic;
        this.WaitIssueCount = a.wc;
        this.ProjectDate = a.t;
        this.WaitMoney = a.wm;
        return this
    }

};
TK.Bet.PlayType = new Object();
TK.Bet.Service.BetRecord = function () { };
TK.Bet.Service.BetRecord.prototype = {
    DomListMenu: [], DomListWrap: [], CurrentContext: null, LastIssueNumber: "", CurrentIssueNumber: "", LastData: [], CurrentData: [], WaitData: [], HashTicket: {}, UpdatingIdList: [], RecordImg: "", DomRecordImg: null, menuChanged: function (a) {
        if (this.CurrentContext != null) {
            $(this.CurrentContext).removeClass("active").prop("wrap").hide()
        }
        this.CurrentContext = a;
        $(this.CurrentContext).addClass("active").prop("wrap").show();
        this.checkBlankImgShow()
    }
    , translateStatus: function (b) {
        if ($S.Debug.IntelliSense) {
            b = new TK.Bet.Util.BetItem()
        }
        var c = "";
        function a(e, d) {
            return e == 1 ? (d > 0 ? String.format("奖金：￥{0}", d.toMoney()) : "待兑奖") : "未中奖"
        }
        b.rc = false;
        b.rb = false;
        switch (b.TicketStatus) {
            case 1: if (b.ChildTicketCount == b.SuccessCount && b.ChildTicketCount > 0) {
                c = "购买成功";
                b.rb = !(b.BonusStatus > 0);
                if (!b.rb) {
                    c = a(b.BonusStatus, b.BonusMoney)
                }

            }
            else {
                if (b.ChildTicketCount == b.FailCount && b.ChildTicketCount > 0) {
                    c = "购买失败"
                }
                else {
                    if (b.SuccessCount > 0) {
                        c = "部分成功";
                        b.rc = b.SuccessCount + b.FailCount < b.ChildTicketCount;
                        b.rb = !(b.BonusStatus > 0);
                        if (!b.rb) {
                            c = a(b.BonusStatus, b.BonusMoney)
                        }

                    }
                    else {
                        c = "购买中";
                        b.rc = true
                    }

                }

            }
                break;
            case 4: c = "购买成功";
                b.rb = !(b.BonusStatus > 0);
                if (b.BonusStatus > 0) {
                    c = a(b.BonusStatus, b.BonusMoney)
                }
                break;
            case 6: c = "用户撤单";
                break;
            case 7: case 10: case 5: c = "购买失败";
                break;
            case 8: c = "中奖撤单";
                break;
            case 9: c = "出号撤单";
                break
        }
        this.HashTicket[b.Id] = b;
        return c
    }
    , autoScroll: function (a) {
        var b = $(a);
        while (true) {
            b = $(b).parent();
            if (b.get(0).tagName.toLowerCase() == "div" && $(b).hasClass("sListTD")) {
                break
            }

        }
        $(b).jscroll({
            Height: 120, EnableMaxHeight: false, Fun: function (c) {
                $(b).parents("div.sListc").find("div.sListTH").css("padding-right", c ? "0px" : "11px")
            }

        })
    }
    , setLastData: function (b, a) {
        this.LastIssueNumber = b;
        var g = this.LastIssueNumber.split("-").length > 1 ? this.LastIssueNumber.split("-")[1] : this.LastIssueNumber;
        var d = $(this.DomListMenu[0]).html(String.format("上一期({0})", g)).prop("headIssue");
        $(d).html(String.format('【<span style="color:#57d;">{0}期</span>】', this.LastIssueNumber));
        if (a.length == 0) {
            return
        }
        var e = [];
        this.LastData.clear();
        var h = JSON.parse(a);
        for (var f = 0;
        f < h.length;
        f++) {
            var c = new TK.Bet.Util.BetItem().parse(h[f]);
            e.push(String.format('<tr tid="{0}"><td class="i1">{1}</td><td class="i2">{2}</td><td class="i3">{3}</td><td class="i4">{4}</td><td class="i5">{5}</td></tr>', c.Id, c.PlayTypeName, c.BetNumber, c.Multiple, c.BetMoney.toMoney(), this.translateStatus(c)));
            this.LastData.push(c.Id)
        }
        $(this.DomListMenu[0]).prop("body").html(e.join(""));
        this.autoScroll($(this.DomListMenu[0]).prop("body"))
    }
    , setCurrentData: function (b, a) {
        this.CurrentIssueNumber = b;
        var g = this.CurrentIssueNumber.split("-").length > 1 ? this.CurrentIssueNumber.split("-")[1] : this.CurrentIssueNumber;
        var d = $(this.DomListMenu[1]).html(String.format("当前期({0})", g)).prop("headIssue");
        $(d).html(String.format('【<span style="color:#d20;">{0}期</span>】', this.CurrentIssueNumber));
        if (a.length == 0) {
            return
        }
        var e = [];
        this.CurrentData.clear();
        var h = JSON.parse(a);
        for (var f = 0;
        f < h.length;
        f++) {
            var c = new TK.Bet.Util.BetItem().parse(h[f]);
            e.push(String.format('<tr tid="{0}"><td class="i1">{1}</td><td class="i2">{2}</td><td class="i3">{3}</td><td class="i4">{4}</td><td class="i5">{5}</td></tr>', c.Id, c.PlayTypeName, c.BetNumber, c.Multiple, c.BetMoney.toMoney(), this.translateStatus(c)));
            this.CurrentData.push(c.Id)
        }
        $(this.DomListMenu[1]).prop("body").html(e.join(""));
        this.autoScroll($(this.DomListMenu[1]).prop("body"))
    }
    , setWaitData: function (a) {
        if (a.length == 0) {
            return
        }
        var b = [];
        var d = JSON.parse(a);
        for (var c = 0;
        c < d.length;
        c++) {
            var e = new TK.Bet.Util.WaitProject().prase(d[c]);
            b.push(String.format('<tr><td class="i1">{0}</td><td class="i2">{1}</td><td class="i3">{2}</td><td class="i4">{3}</td><td class="i5"><a href="{4}" target="_blank">查看</a></td></tr>', e.ProjectNo, e.Money.toMoney(), e.WaitMoney.toMoney(), e.BetIssueCount - e.WaitIssueCount + "/" + (e.BetIssueCount), String.format($("#linkDetailDemo").attr("href"), TK.CurrentLotteryType, e.ProjectDate, e.Id)))
        }
        $(this.DomListMenu[2]).prop("headIssue").html();
        $(this.DomListMenu[2]).prop("body").html(b.join(""));
        this.autoScroll($(this.DomListMenu[2]).prop("body"))
    }
    , handlerBonus: function (b, a) {
        setTimeout(this.requestDataItem.bind(this, true), 5000)
    }
    , handlerBetEnd: function (a) {
        this.menuChanged(this.DomListMenu[0])
    }
    , handlerResponseDataBlock: function (b, a) {
        this.responseDataBlock(a)
    }
    , responseDataBlock: function (a) {
        this.setLastData(a[0], a[2]);
        this.setCurrentData(a[1], a[3]);
        this.setWaitData(a[4]);
        this.requestDataItem();
        this.checkBlankImgShow()
    }
    , handlerResponseDataItem: function (e, argData) {
        this.responseDataItem(JSON.parse(argData[0]), eval(argData[1].toString().toLowerCase()))
    }
    , responseDataItem: function (a, b) {
        for (var d = 0;
        d < a.length;
        d++) {
            var c = new TK.Bet.Util.BetItem().parse(a[d]);
            this.UpdatingIdList.remove(c.Id);
            var e = this.translateStatus(c);
            $(this.DomListWrap).find(String.format("tr[tid={0}] td.i5", c.Id)).html(e)
        }
        this.requestDataItem(b)
    }
    , requestDataItem: function (a) {
        var d = [];
        if (this.LastIssueNumber == TK.Video.LastIssueData.IssueNumber && TK.Video.LastIssueData.BonusNumber != "") {
            for (var c = 0;
            c < this.LastData.length;
            c++) {
                var b = this.HashTicket[this.LastData[c]];
                if (b.rb && !this.UpdatingIdList.exists(b.Id)) {
                    d.push(b.Id)
                }

            }
            if (this.CurrentContext != this.DomListMenu[1] && d.length == 0 && a == true) {
                this.menuChanged(this.DomListMenu[1])
            }

        }
        for (var c = 0;
        c < this.LastData.length;
        c++) {
            var b = this.HashTicket[this.LastData[c]];
            if (b.rc && !this.UpdatingIdList.exists(b.Id) && !d.exists(b.Id)) {
                d.push(b.Id)
            }

        }
        for (var c = 0;
        c < this.CurrentData.length;
        c++) {
            var b = this.HashTicket[this.CurrentData[c]];
            if (b.rc && !this.UpdatingIdList.exists(b.Id)) {
                d.push(b.Id)
            }

        }
        if (d.length > 0) {
            this.UpdatingIdList = this.UpdatingIdList.concat(d);
            var e = new $S.JsonCommand();
            e.Command = TK.Command.Enum.RecentTicketRecord;
            e.ListParameter.push(JSON.stringify(d));
            e.ListParameter.push(a == true);
            var f = this;
            setTimeout(function () {
                TK.Command.sendAjax(e)
            }
            , 5000)
        }

    }
    , handlerRequestDataBlock: function (a) {
        this.requestDataBlock({
            Last: true, Current: true, Wait: true, RecallTimeout: 0
        })
    }
    , requestDataBlock: function (a) {
        a = $.extend({
            Last: false, Current: false, Wait: false, RecallTimeout: 3000
        }
        , a);
        var b = new $S.JsonCommand();
        b.Command = TK.Command.Enum.RecentBetRecord;
        b.ListParameter = [a.Last ? 1 : 0, a.Current ? 1 : 0, a.Wait ? 1 : 0];
        TK.Command.sendAjax(b)
    }
    , handlerBetSubmitOK: function () {
        this.requestDataBlock({
            Last: true, Current: true, Wait: true, RecallTimeout: 0
        });
        this.menuChanged(this.DomListMenu[1])
    }
    , checkBlankImgShow: function () {
        if (this.DomRecordImg == null) {
            return
        }
        var a = $(this.CurrentContext).prop("wrap");
        if ($(a).find("tbody:last tr").length > 0) {
            $(a).show();
            this.DomRecordImg.hide()
        }
        else {
            $(a).hide();
            this.DomRecordImg.show()
        }

    }
    , setBlankImg: function () {
        if (this.RecordImg == "" || this.DomRecordImg != null) {
            return
        }
        var a = String.format('<div class="sListc">{0}</div>', this.RecordImg);
        this.DomRecordImg = $(a).insertBefore(this.DomListWrap[0])
    }
    , initialize: function () {
        var b = this;
        var c = $("div.sumInfo");
        var a = this.DomListWrap = $(c).children("div.sListc");
        this.DomListMenu = $(c).find("div.sListt a").each(function (e) {
            var d = a[e];
            $(this).click(b.menuChanged.bind(b, this)).prop("wrap", $(d)).prop("headIssue", $(d).children("p.cpt").children("i")).prop("headCount", $(d).children("p.cpt").children("span")).prop("body", $(d).find("tbody:last"))
        });
        if (this.RecordImg != "") {
            this.setBlankImg()
        }
        this.menuChanged(this.DomListMenu[1]);
        $(TK.Command).bind("Response_" + TK.Command.Enum.RecentBetRecord, this.handlerResponseDataBlock.bind(this));
        $(TK.Command).bind("Response_" + TK.Command.Enum.RecentTicketRecord, this.handlerResponseDataItem.bind(this));
        $(TK.Video).bind(TK.Video.Handler_BetIssueEnd, this.handlerBetEnd.bind(this));
        $(TK.Video).bind(TK.Video.Handler_GetRecentBetRecord, this.handlerResponseDataBlock.bind(this));
        $(TK.Video).bind(TK.Video.Handler_GetBonus, this.handlerBonus.bind(this));
        $(TK.User).bind(TK.User.Handler_LoginSuccess, this.handlerRequestDataBlock.bind(this));
        return this
    }

};
TK.Bet.Service.MenuBet = function () {
    this.getBonusMoney = function (a) {
        return []
    };
    this.initialize = function () { }
};
TK.Bet.Service.initMenuBetPrototype = function (a) {
    if (typeof (a.getBonusMoney) != "function") {
        a.getBonusMoney = function () {
            return []
        }

    }

};
TK.Bet.Service.BetList = function () { };
TK.Bet.Service.BetList.prototype = {
    ListBetItem: [], DomWrap: null, DomEmpty: null, DomItemCount: null, DomClear: null, Handler_itemChanged: "Handler_itemChanged", doCompute: function () {
        $(this.DomItemCount).html(String.format("投注项：{0}", this.ListBetItem.length));
        $(this).triggerHandler(this.Handler_itemChanged, [this.ListBetItem])
    }
    , delItem: function (a) {
        $(a).remove();
        if ($(this.DomWrap).children("li:not([class])").length == 0) {
            $(this.DomEmpty).appendTo(this.DomWrap)
        }
        this.ListBetItem.remove($(a).prop("data"));
        this.doCompute()
    }
    , addItem: function (c, b) {
        var a = new TK.Bet.Util.BetItem();
        $.extend(a, b);
        var d = [];
        d.push("<li>");
        d.push(String.format('<span class="c1">{0}</span>', a.PlayTypeName));
        d.push(String.format('<span class="c2">{0}</span>', a.BetNumber));
        d.push(String.format('<span class="c3">{0}注</span>', a.BetCount.toMoney()));
        d.push(String.format('<span class="c4">{0}</span>', a.BetMoney.toMoney("￥")));
        d.push('<span class="c5">×</span>');
        d.push("</li>");
        var f = $(d.join("")).appendTo(this.DomWrap).prop("data", a);
        f.children("span.c5").click(this.delItem.bind(this, f));
        $(this.DomEmpty).remove();
        this.ListBetItem.push(a);
        this.doCompute()
    }
    , clear: function () {
        $(this.DomWrap).children("li:not([class])").remove();
        $(this.DomEmpty).appendTo(this.DomWrap);
        this.ListBetItem = [];
        this.doCompute()
    }
    , initialize: function () {
        this.DomWrap = $("#Bet_ItemList");
        this.DomEmpty = $('<li class="empty">暂无投注项</li>').appendTo(this.DomWrap);
        this.DomItemCount = $(this.DomWrap).find("li.thc span.l");
        this.DomClear = $(this.DomWrap).find("li.thc span.r").click(this.clear.bind(this));
        this.ListBetItem = [];
        return this
    }

};
TK.Bet.Service.IssueList = function () { };
TK.Bet.Service.IssueList.Current = function () { };
TK.Bet.Service.IssueList.Current.prototype = {
    BetIssue: new TK.Bet.Util.BetIssue(), PerBetCount: 0, PerBetMoney: 0, DomWrap: null, DomBetCount: null, DomBetMultiple: null, DomBetMoney: null, DomCurrentIssue: null, Handler_PerBetInfoChanged: "Handler_PerBetInfoChanged", Handler_ComputeChanged: "Handler_ComputeChanged", compute: function () {
        $(this.DomBetCount).html(this.PerBetCount.toMoney());
        this.BetIssue.Multiple = parseInt($(this.DomBetMultiple).val(), 10);
        $(this.DomBetMoney).html((this.BetIssue.Multiple * this.PerBetMoney).toMoney("￥"));
        $(this).triggerHandler(this.Handler_ComputeChanged)
    }
    , updatePerBetInfo: function (c, a, b) {
        this.PerBetCount = a;
        this.PerBetMoney = b;
        this.compute()
    }
    , updateBetIssue: function () {
        $(this.DomCurrentIssue).html(this.BetIssue.IssueNumber)
    }
    , show: function (a, b) {
        if (b == true) {
            $(this.DomWrap).show()
        }
        else {
            $(this.DomWrap).hide()
        }

    }
    , initialize: function () {
        var a = this;
        this.BetIssue = new TK.Bet.Util.BetIssue();
        this.BetIssue.Multiple = 1;
        this.DomWrap = $("#Bet_IssueList_Current");
        this.DomBetCount = $(this.DomWrap).find("em.flt:first");
        this.DomBetMultiple = $(this.DomWrap).find("input:text.bs").val(this.BetIssue.Multiple).dom({
            limit_inputRegexp: new RegExp(/^\d$/), limit_outputRegexp: new RegExp(/^$|\d*$/), limit_length: 6, autoselect: true, horn_status: -1, keyup_event: this.compute.bind(this), change_min: 1, change_event: this.compute.bind(this)
        });
        this.DomBetMoney = $(this.DomWrap).find("em.flt:last");
        this.DomCurrentIssue = $(this.DomWrap).find("span.vra span");
        $(this).bind(this.Handler_PerBetInfoChanged, this.updatePerBetInfo.bind(this));
        $(this).bind("IssueList_UpdateIssueInfoForBet", this.updateBetIssue.bind(this));
        $(this).bind("show", this.show.bind(this));
        return this
    }

};
TK.Bet.Service.IssueList.ZhuiHao = function () { };
TK.Bet.Service.IssueList.ZhuiHao.prototype = {
    ListBetIssueInfo: [], ListIssueListData: [], PerBetCount: 0, PerBetMoney: 0, PerBonusMoney: [], DomWrap: null, DomInputIssueCount: null, DomLabelMaxIssueCount: null, DomCheckBoxAll: null, DomIssueListWrap: null, DomBonusStop: null, DomEarMoneyStop: null, DomEarlyStop: null, DomIssueListContainer: null, Handler_PerBetInfoChanged: "Handler_PerBetInfoChanged", Handler_ComputeChanged: "Handler_ComputeChanged", Handler_ToggleBetTool: "Handler_ToggleBetTool", compute: function (a) {
        this.ListIssueListData.clear();
        var k = $(this.DomIssueListWrap).children("li").children("span.i1");
        var j = 0, n = 0;
        for (var f = 0;
        f < k.length;
        f++) {
            var h = k[f];
            var d = $(h).children("input[type=checkbox]");
            var m = d.prop("checked");
            var g = $(h).nextAll("span.i3").children("input[type=text]");
            var l = parseInt(g.val(), 10);
            if (g.get(0) == a) {
                j = l
            }
            if (j > 0) {
                l = j;
                $(g).val(j)
            }
            var e = this.PerBetMoney * l;
            if (m) {
                n += e;
                var b = new TK.Bet.Util.BetIssue();
                b.IssueNumber = $(d).val();
                b.Multiple = l;
                this.ListIssueListData.push(b)
            }
            $(h).nextAll("span.i4").html(e.toMoney("￥"));
            $(h).nextAll("span.i5").html(n.toMoney("￥"));
            if (this.PerBonusMoney > 0) {
                $(h).nextAll("span.i6").html((this.PerBonusMoney * l).toMoney("￥"));
                $(h).nextAll("span.i7").html(((this.PerBonusMoney * l) - n).toMoney("￥"))
            }
            else {
                $(h).nextAll("span.i6").html("-");
                $(h).nextAll("span.i7").html("-")
            }

        }
        var c = true;
        if (this.ListIssueListData.length > 1) {
            $(this.DomBonusStop).parent().parent().show()
        }
        else {
            $(this.DomBonusStop).parent().parent().hide();
            c = false
        }
        if (this.ListIssueListData.length > 0 && this.ListIssueListData[this.ListIssueListData.length - 1].IssueNumber > this.ListBetIssueInfo[0].IssueNumber) {
            $(this.DomEarlyStop).parent().parent().show();
            c = true
        }
        else {
            $(this.DomEarlyStop).parent().parent().hide()
        }
        $(this.DomBonusStop).parent().parent().parent().toggle(c);
        $(this).triggerHandler(this.Handler_ComputeChanged)
    }
    , updatePerBetInfo: function (d, a, b, c) {
        this.PerBetCount = a;
        this.PerBetMoney = b;
        this.PerBonusMoney = MenuBet_ComputeBonusMoney ? c.MinBonusMoney : c.length > 0 ? c[0] : 0;
        this.compute()
    }
    , itemSelectChanged: function (b, a) {
        var c = $(b).parent().parent();
        if ($(b).prop("checked")) {
            $(c).addClass("selected").find("span").filter(function () {
                return !($(this).hasClass("i1") || $(this).hasClass("i2"))
            }).show()
        }
        else {
            $(c).removeClass("selected").find("span").filter(function () {
                return !($(this).hasClass("i1") || $(this).hasClass("i2"))
            }).hide()
        }
        if (a) {
            this.compute()
        }

    }
    , allItemSelectChanged: function () {
        var a = this;
        if ($(this.DomCheckBoxAll).prop("checked")) {
            $(this.DomIssueListWrap).find("input[type=checkbox]:not(checked)").each(function () {
                $(this).prop("checked", true);
                a.itemSelectChanged(this)
            })
        }
        else {
            $(this.DomIssueListWrap).find("input[type=checkbox]:checked").each(function () {
                $(this).prop("checked", false);
                a.itemSelectChanged(this)
            })
        }
        this.compute()
    }
    , initIssueItems: function (a) {
        var b = [], f = 0;
        for (var c = 0;
        c < a;
        c++) {
            f += this.PerBetMoney;
            b.push('<li class="selected">');
            b.push(String.format('<span class="i1"><input type="checkbox" checked="checked" value="{0}" /></span>', this.ListBetIssueInfo[c].IssueNumber));
            b.push(String.format('<span class="i2">{0}</span>', this.ListBetIssueInfo[c].IssueNumber));
            b.push('<span class="i3"><input type="text" value="1" /></span>');
            b.push(String.format('<span class="i4">{0}</span>', this.PerBetMoney.toMoney("￥")));
            b.push(String.format('<span class="i5">{0}</span>', f.toMoney("￥")));
            b.push(String.format('<span class="i6">{0}</span>', (this.PerBonusMoney > 0 ? parseFloat(this.PerBonusMoney).toMoney("￥") : "-")));
            b.push(String.format('<span class="i7">{0}</span>', (this.PerBonusMoney > 0 ? (parseFloat(this.PerBonusMoney) - f).toMoney("￥") : "-")));
            b.push("</li>")
        }
        var e = this;
        $(b.join("")).appendTo($(this.DomIssueListWrap).html("")).find("input[type=checkbox]").each(function () {
            $(this).click(e.itemSelectChanged.bind(e, this, true))
        }).end().find("input[type=text]").each(function () {
            $(this).dom({
                autoselect: true, limit_length: 6, keyup_event: e.compute.bind(e, this), change_event: e.compute.bind(e, this), change_min: 1
            })
        });
        var d = $(this.DomIssueListWrap).children("li:first");
        if (this.DomIssueListContainer == null) {
            this.DomIssueListContainer = $("<div></div>").appendTo($(this.DomIssueListWrap).parent());
            $(this.DomIssueListWrap).appendTo(this.DomIssueListContainer)
        }
        $(this.DomIssueListContainer).jscroll({
            Height: (d.height() + 2) * 10, EnableMaxHeight: true
        });
        setTimeout(this.compute.bind(this), 1)
    }
    , issueCountChanged: function () {
        this.initIssueItems($(this.DomInputIssueCount).val())
    }
    , updateBetIssue: function () {
        var b = this;
        var a = $(this.DomIssueListWrap).find("input[type=checkbox]");
        if (parseInt($(this.DomInputIssueCount).val(), 10) > this.ListBetIssueInfo.length) {
            $(this.DomInputIssueCount).val(this.ListBetIssueInfo.length)
        }
        if (a.length != parseInt($(this.DomInputIssueCount).val(), 10)) {
            this.issueCountChanged()
        }
        else {
            a.each(function (c) {
                $(this).val(b.ListBetIssueInfo[c].IssueNumber);
                if ($(this).prop("checked")) {
                    $(this).parent().next().html(b.ListBetIssueInfo[c].IssueNumber)
                }

            })
        }
        $(this.DomLabelMaxIssueCount).html(this.ListBetIssueInfo.length);
        this.compute()
    }
    , show: function (a, b) {
        if (b == true) {
            $(this.DomWrap).show();
            this.updateBetIssue()
        }
        else {
            $(this.DomWrap).hide()
        }

    }
    , toggleBetTool: function () {
        $(this).triggerHandler(this.Handler_ToggleBetTool, [this])
    }
    , initialize: function () {
        var c = this;
        this.DomWrap = $("#Bet_IssueList_ZhuiHao");
        $(this.DomWrap).find("ul.tab_multiple input:radio:eq(1)").click(function () {
            this.checked = false;
            c.toggleBetTool()
        });
        var d = $(this.DomWrap).find("ul[class*=itemList][class*=condtn]");
        $(d).find("a").attr("href", "javascript:void(0)").click(function () {
            $(c.DomInputIssueCount).val($(this).text().replace(/^追(\d+)期$/ig, "$1"));
            c.issueCountChanged()
        });
        function a() {
            if (parseInt($(this).val(), 10) > c.ListBetIssueInfo.length) {
                $(this).val(c.ListBetIssueInfo.length)
            }
            c.issueCountChanged()
        }
        this.DomInputIssueCount = d.find("input[type=text]").dom({
            autoselect: true, keyup_event: a, change_event: a, change_min: 1
        });
        this.DomLabelMaxIssueCount = d.find("i");
        this.DomCheckBoxAll = $(this.DomWrap).find("div.itemBox ul[class*=thc] span.i1 input[type=checkbox]").attr("checked", "true").click(this.allItemSelectChanged.bind(this));
        this.DomIssueListWrap = $(this.DomWrap).find("div.itemBox ul[class*=itemListC]");
        this.DomBonusStop = $(this.DomWrap).find("ul[class*=chase] p input[type=checkbox]:first").prop("checked", true).attr("value", "1");
        var b = "display:none;";
        switch (TK.CurrentLotteryType) {
            default: b = "";
                break
        }
        this.DomEarMoneyStop = $('<p style="' + b + '"><label><input type="checkbox" value="2" /><strong style="color:#f00;">盈利停止</strong>&nbsp;&nbsp;投注多期时，当奖金达到方案整体盈利，自动放弃后面的投注操作。</label></p>').insertAfter($(this.DomBonusStop).parent().parent()).find("input");
        $(this.DomWrap).find("ul[class*=chase] p input[type=checkbox][value]").each(function () {
            $(this).click(function () {
                if ($(this).get(0).checked) {
                    $(this).parent().parent().siblings().find("input[value]").get(0).checked = false
                }

            })
        });
        this.DomEarlyStop = $(this.DomWrap).find("ul[class*=chase] p input[type=checkbox]:last").prop("checked", true);
        $(this).bind(this.Handler_PerBetInfoChanged, this.updatePerBetInfo.bind(this));
        $(this).bind("IssueList_UpdateIssueInfoForBet", this.updateBetIssue.bind(this));
        $(this).bind("show", this.show.bind(this));
        return this
    }

};
TK.Bet.Service.IssueList.BetTool = function () { };
TK.Bet.Service.IssueList.BetTool.prototype = {
    ListBetIssueInfo: [], ListIssueListData: [], PerBetCount: 0, PerBetMoney: 0, PerBonusMoney: [], DomWrap: null, DomRadioes: null, DomFirstMultiple: null, DomInputIssueCount: null, DomLabelMaxIssueCount: null, DomAllProfitsRate: null, DomForepartProfitsRate: null, DomForepartSplitIssueRate: null, DomAfterpartProfitsRate: null, DomAllProfitsMoney: null, DomForepartProfitsMoney: null, DomForepartSplitIssueMoney: null, DomAfterpartProfitsMoney: null, DomBtnCreatePlan: null, DomIssueListWrap: null, DomBonusStop: null, DomEarMoneyStop: null, DomEarlyStop: null, DomCaluMoneyParentWrap: null, DomCaluMoneyChildWrap: null, Handler_ComputeChanged: "Handler_ComputeChanged", Handler_PerBetInfoChanged: "Handler_PerBetInfoChanged", Handler_ToggleZhuiHao: "Handler_ToggleZhuiHao", clear: function () {
        $(this.DomIssueListWrap).html("");
        this.compute()
    }
    , getComputeBonusMoney: function () {
        var a = 0;
        if (MenuBet_ComputeBonusMoney) {
            var c = $(this.DomCaluMoneyParentWrap).find("input[type=radio]:checked");
            if ($(c).val() == "0") {
                var b = $(this.DomCaluMoneyChildWrap).find("input[type=radio]:checked");
                a = parseFloat($(b).val())
            }
            else {
                if (!isNaN($(c).val())) {
                    a = parseFloat($(c).val())
                }
                else {
                    a = parseFloat($(this.DomCaluMoneyParentWrap).find("input[type=text]").val()) || this.PerBonusMoney.MinBonusMoney
                }

            }

        }
        else {
            if (this.PerBonusMoney.length > 0) {
                a = this.PerBonusMoney[0]
            }

        }
        return a
    }
    , compute: function (a) {
        this.ListIssueListData.clear();
        var l = $(this.DomIssueListWrap).children("li").children("span.i1");
        var k = 0, n = 0;
        for (var e = 0;
        e < l.length;
        e++) {
            var j = l[e];
            var g = $(j).nextAll("span.i2").children("input[type=text]");
            var m = parseInt(g.val(), 10);
            var d = this.PerBetMoney * m;
            n += d;
            var b = new TK.Bet.Util.BetIssue();
            b.IssueNumber = $(j).text();
            b.Multiple = m;
            this.ListIssueListData.push(b);
            $(j).nextAll("span.i3").html(d.toMoney("￥"));
            $(j).nextAll("span.i4").html(n.toMoney("￥"));
            var c = this.getComputeBonusMoney();
            if (c > 0) {
                var f = c * m;
                var h = f - n;
                $(j).nextAll("span.i5").html(f.toMoney("￥"));
                $(j).nextAll("span.i6").html(h.toMoney("￥"));
                $(j).nextAll("span.i7").html(new $S.PlanItem().getProfitsPercentRate(h / n) + "%")
            }
            else {
                $(j).nextAll("span.i5").html("-");
                $(j).nextAll("span.i6").html("-");
                $(j).nextAll("span.i7").html("-")
            }

        }
        if (this.ListIssueListData.length > 1) {
            $(this.DomBonusStop).parent().parent().show();
            $(this.DomEarMoneyStop).parent().parent().show()
        }
        else {
            $(this.DomBonusStop).parent().parent().hide();
            $(this.DomEarMoneyStop).parent().parent().hide()
        }
        if (this.ListIssueListData.length > 0 && this.ListIssueListData[this.ListIssueListData.length - 1].IssueNumber > this.ListBetIssueInfo[0].IssueNumber) {
            $(this.DomEarlyStop).parent().parent().show()
        }
        else {
            $(this.DomEarlyStop).parent().parent().hide()
        }
        $(this).triggerHandler(this.Handler_ComputeChanged)
    }
    , updatePerBetInfo: function (d, a, b, c) {
        this.PerBetCount = a;
        this.PerBetMoney = b;
        this.PerBonusMoney = c;
        if (MenuBet_ComputeBonusMoney) {
            this.createParentUL()
        }
        this.compute()
    }
    , createPlan: function (b) {
        var j = $(this.DomRadioes).filter(":checked").val();
        var f = new $S.PlanList();
        f.PerBetMoney = this.PerBetMoney;
        f.PerMinBonusMoney = this.getComputeBonusMoney();
        f.PerMaxBonusMoney = f.PerMinBonusMoney;
        f.FirstMultiple = parseInt($(this.DomFirstMultiple).val(), 10);
        f.IssueCount = parseInt($(this.DomInputIssueCount).val(), 10);
        var h = [];
        switch (parseInt(j, 10)) {
            case $S.PlanItem.enumType.全程利润率: h = f.computeByWholeRate(parseFloat($(this.DomAllProfitsRate).val()) / 100);
                break;
            case $S.PlanItem.enumType.两步利润率: h = f.computeBySplitRate(parseInt($(this.DomForepartSplitIssueRate).val(), 10), parseFloat($(this.DomForepartProfitsRate).val()) / 100, parseFloat($(this.DomAfterpartProfitsRate).val()) / 100);
                break;
            case $S.PlanItem.enumType.全程利润金额: h = f.computeByWholeMoney(parseFloat($(this.DomAllProfitsMoney).val()));
                break;
            case $S.PlanItem.enumType.两步利润金额: h = f.computeBySplitMoney(parseInt($(this.DomForepartSplitIssueMoney).val(), 10), parseFloat($(this.DomForepartProfitsMoney).val()), parseFloat($(this.DomAfterpartProfitsMoney).val()));
                break
        }
        var c = [];
        for (var d = 0;
        d < h.length;
        d++) {
            var g = h[d];
            if ($S.Debug.IntelliSense) {
                g = new $S.PlanItem()
            }
            c.push(String.format('<li><span class="i1">{0}</span><span class="i2"><input type="text" value="{1}" /></span><span class="i3">{2}</span><span class="i4">{3}</span><span class="i5">{4}</span><span class="i6">{5}</span><span class="i7">{6}</span></li>', this.ListBetIssueInfo[d].IssueNumber, g.Multiple, g.BetMoney.toMoney("￥"), g.TotalBetMoney.toMoney("￥"), g.BonusMinMoney.toMoney("￥"), g.ProfitsMinMoney.toMoney("￥"), g.getProfitsPercentRate(g.ProfitsMinRate) + "%"))
        }
        if (h.length < f.IssueCount) {
            c.push('<li tip="1" style="text-align:center;">之后奖期的投注不能满足当前设置的条件或者投入金额太大……</li>')
        }
        var a = this;
        $(c.join("")).appendTo($(this.DomIssueListWrap).html("")).find("input[type=text]").each(function () {
            $(this).dom({
                autoselect: true, keyup_event: a.compute.bind(a, this), change_event: a.compute.bind(a, this), change_min: 1
            })
        });
        if (b == true) {
            return
        }
        function e(i) {
            if (i >= 8) {
                return
            }
            i++;
            $(a.DomIssueListWrap).find("li[tip=1]").css("color", i % 2 == 0 ? "#ff6c16" : "#ccc");
            setTimeout(e.bind(this, i), 100)
        }
        e(0);
        setTimeout(this.compute.bind(this), 1)
    }
    , updateBetIssue: function () {
        var a = this;
        if (parseInt($(this.DomInputIssueCount).val(), 10) > this.ListBetIssueInfo.length) {
            $(this.DomInputIssueCount).val(this.ListBetIssueInfo.length)
        }
        this.createPlan(true);
        $(this.DomLabelMaxIssueCount).html(this.ListBetIssueInfo.length);
        this.compute()
    }
    , show: function (a, b) {
        if (b == true) {
            $(this.DomWrap).show();
            this.updateBetIssue()
        }
        else {
            $(this.DomWrap).hide()
        }

    }
    , toggleZhuiHao: function () {
        $(this).triggerHandler(this.Handler_ToggleZhuiHao, [this])
    }
    , createChildUL: function (b) {
        if (typeof (b) == "undefined" || typeof (this.PerBonusMoney[b]) == "undefined" || this.PerBonusMoney[b].length == 0) {
            $(this.DomCaluMoneyChildWrap).hide();
            return
        }
        var c = [];
        c.push("<p>选择用于计算的单倍奖金：</p>");
        for (var d = 0;
        d < this.PerBonusMoney[b].length;
        d++) {
            var e = this.PerBonusMoney[b][d], f = this.PerBonusMoney[b].length - 1;
            if ($S.Debug.IntelliSense) {
                e = {
                    Text: "", BonusMoney: 0, BonusCount: 0
                }

            }
            if (d % 3 == 0) {
                c.push("<li>")
            }
            c.push(String.format('<label{3}><input type="radio" value="{1}" name="child"{2} />{0}（{1}元）</label>', e.Text, e.BonusMoney, (d == f ? ' checked="checked"' : ""), (d == f ? ' class="active"' : "")));
            if (d % 3 == 2 || d == f) {
                c.push("</li>")
            }

        }
        $(this.DomCaluMoneyChildWrap).html(c.join("")).show();
        var g = $(this.DomCaluMoneyChildWrap).find("input[type=radio]");
        var a = this;
        g.each(function () {
            $(this).click(function () {
                $(g).each(function () {
                    $(this).parent().removeClass("active")
                });
                $(this).parent().addClass("active")
            })
        });
        if (this.PerBonusMoney[b].length == 1) {
            $(this.DomCaluMoneyChildWrap).hide()
        }

    }
    , createParentUL: function () {
        var e = [], f = 0, b = "";
        e.push("<p>选择用于计算的玩法奖金：</p>");
        e.push(String.format('<li><label class="active"><input type="radio" value="{0}" checked="checked" name="parent" />最低奖金（{0}元）</label><label><a class="more">更多玩法奖金数据</a></label></li>', this.PerBonusMoney.MinBonusMoney));
        e.push('<ul class="d" style="display:none;">');
        for (var g in this.PerBonusMoney) {
            if (g.toString().toLowerCase() == "minbonusmoney" || g.toString().toLowerCase() == "maxbonusmoney") {
                continue
            }
            b = g;
            var h = this.PerBonusMoney[g][0].BonusMoney, i = this.PerBonusMoney[g][this.PerBonusMoney[g].length - 1].BonusMoney;
            if (f % 3 == 0) {
                e.push("<li>")
            }
            e.push(String.format('<label><input type="radio" value="0" name="parent" key="{0}" />{0}（{1}元）</label>', g, (i == h ? i : i + "~" + h)));
            if (f % 3 == 2) {
                e.push("</li>")
            }
            f++
        }
        if ((f - 1) % 3 != 2) {
            e.push("</li>")
        }
        else {
            e.push("<li></li>")
        }
        e.push("</ul>");
        var d = $(e.join("")).appendTo($(this.DomCaluMoneyParentWrap).html(""));
        $('<label><input type="radio" name="parent" key="selfdefine" />自定义 <input type="text" class="cdn_ipt" value="' + i + '" /> 元</label>').appendTo($(this.DomCaluMoneyParentWrap).find("ul.d li:last"));
        var k = $(this.DomCaluMoneyParentWrap).find("input[type=radio]");
        var a = this;
        k.each(function () {
            $(this).click(function () {
                $(k).each(function () {
                    $(this).parent().removeClass("active")
                });
                $(this).parent().addClass("active");
                a.createChildUL($(this).attr("key"));
                if ($(this).attr("key") == "selfdefine") {
                    $(this).nextAll("input[type=text]").focus().select()
                }

            })
        });
        var c = $(this.DomCaluMoneyParentWrap).find("ul.d");
        var j = $(this.DomCaluMoneyParentWrap).find("a.more").click(function () {
            if ($(c).is(":hidden")) {
                $(c).slideDown(100);
                $(this).text("返回最低奖金")
            }
            else {
                $(c).slideUp(100);
                $(this).text("更多玩法奖金数据");
                $(k.eq(0)).click()
            }

        });
        if (f > 1 || (f == 1 && this.PerBonusMoney[b].length > 1)) {
            $(this.DomCaluMoneyParentWrap).show()
        }
        else {
            $(this.DomCaluMoneyChildWrap).hide();
            $(this.DomCaluMoneyParentWrap).hide()
        }

    }
    , initialize: function () {
        var d = this;
        this.DomWrap = $("#Bet_IssueList_BeiTou");
        var f = $(this.DomWrap).find("ul[class*=itemList][class*=condtn]");
        $(this.DomWrap).find("ul.tab_multiple input:radio:eq(0)").click(function () {
            this.checked = false;
            d.toggleZhuiHao()
        });
        function a() {
            if (parseInt($(this).val(), 10) > d.ListBetIssueInfo.length) {
                $(this).val(d.ListBetIssueInfo.length)
            }

        }
        this.DomFirstMultiple = f.find("input[type=text]:eq(0)").dom({
            autoselect: true, change_min: 1
        });
        this.DomInputIssueCount = f.find("input[type=text]:eq(1)").dom({
            autoselect: true, keyup_event: a, change_event: a, change_min: 2
        });
        this.DomLabelMaxIssueCount = f.find("i");
        this.DomRadioes = f.find("ul input[type=radio]").click(function () {
            $(this).parent().parent().addClass("active").siblings().removeClass("active")
        });
        var b = f.find("ul input[type=text]").dom({
            autoselect: true, change_min: 1
        });
        this.DomAllProfitsRate = b[0];
        this.DomForepartSplitIssueRate = b[1];
        this.DomForepartProfitsRate = b[2];
        this.DomAfterpartProfitsRate = b[3];
        this.DomAllProfitsMoney = b[4];
        this.DomForepartSplitIssueMoney = b[5];
        this.DomForepartProfitsMoney = b[6];
        this.DomAfterpartProfitsMoney = b[7];
        this.DomBtnCreatePlan = f.find(".cdn_go input:button").click(this.createPlan.bind(this));
        this.DomIssueListWrap = $(this.DomWrap).find("div.itemBox ul[class*=itemListC]");
        this.DomBonusStop = $(this.DomWrap).find("ul[class*=chase] p input[type=checkbox]:first").prop("checked", true).attr("value", "1");
        var c = "display: none;";
        this.DomEarMoneyStop = $('<p style="' + c + '"><label><input type="checkbox" value="2" /><strong style="color:#f00;">盈利停止</strong>&nbsp;&nbsp;投注多期时，当奖金达到方案整体盈利，自动放弃后面的投注操作。</label></p>').insertAfter($(this.DomBonusStop).parent().parent()).find("input");
        $(this.DomWrap).find("ul[class*=chase] p input[type=checkbox][value]").each(function () {
            $(this).click(function () {
                if ($(this).get(0).checked) {
                    $(this).parent().parent().siblings().find("input[value]").get(0).checked = false
                }

            })
        });
        this.DomEarlyStop = $(this.DomWrap).find("ul[class*=chase] p input[type=checkbox]:last").prop("checked", true);
        if (this.DomRadioes.length > 0) {
            this.DomRadioes[0].click()
        }
        $(this.DomWrap).find("ul[class*=chase] p input[type=checkbox][value]").each(function () {
            $(this).click(function () {
                if ($(this).get(0).checked) {
                    $(this).parent().parent().siblings().find("input[value]").get(0).checked = false
                }

            })
        });
        $(this).bind(this.Handler_PerBetInfoChanged, this.updatePerBetInfo.bind(this));
        $(this).bind("IssueList_UpdateIssueInfoForBet", this.updateBetIssue.bind(this));
        $(this).bind("show", this.show.bind(this));
        if (MenuBet_ComputeBonusMoney) {
            var e = f.find("ul");
            this.DomCaluMoneyParentWrap = $('<ul class="c"></ul>').insertBefore(e);
            this.DomCaluMoneyChildWrap = $('<ul class="c"></ul>').insertBefore(e).hide()
        }
        return this
    }

};
TK.Bet.Service.IssueList.prototype = {
    ObjCurrent: new TK.Bet.Service.IssueList.Current(), ObjZhuiHao: new TK.Bet.Service.IssueList.ZhuiHao(), ObjBetTool: new TK.Bet.Service.IssueList.BetTool(), CBoxDuoQi: null, ListIssueInfoForBet: [], PerBetCount: 0, PerBetMoney: 0, PerBonusMoney: MenuBet_ComputeBonusMoney ? {} : [], ShowObject: null, Handler_itemChanged: "Handler_itemChanged", getListBetIssue: function () {
        var a = [];
        switch (this.ShowObject) {
            case this.ObjCurrent: a.push($.extend(new TK.Bet.Util.BetIssue(), this.ObjCurrent.BetIssue));
                break;
            case this.ObjZhuiHao: case this.ObjBetTool: a = this.ShowObject.ListIssueListData;
                break
        }
        return a
    }
    , getStopConfig: function () {
        var a = {
            DropBonusLevel: 0, DropEarlyLevel: 0
        };
        switch (this.ShowObject) {
            case this.ObjCurrent: break;
            case this.ObjZhuiHao: case this.ObjBetTool: a.DropBonusLevel = $(this.ShowObject.DomBonusStop).parent().parent().is(":hidden") ? "0" : ($(this.ShowObject.DomBonusStop).prop("checked") ? "1" : ($(this.ShowObject.DomEarMoneyStop).prop("checked") ? "2" : "0"));
                a.DropEarlyLevel = $(this.ShowObject.DomEarlyStop).parent().parent().is(":hidden") ? "0" : ($(this.ShowObject.DomEarlyStop).prop("checked") ? "1" : "0");
                break
        }
        return a
    }
    , issueList_compute: function () {
        $(this).triggerHandler(this.Handler_itemChanged, [this.getListBetIssue(), this.getStopConfig()])
    }
    , showChange: function (a) {
        if (this.ShowObject != null) {
            $(this.ShowObject).triggerHandler("show", [false])
        }
        this.ShowObject = a;
        $(this.ShowObject).triggerHandler("show", [true]);
        this.perBetInfoChanged()
    }
    , perBetInfoChanged: function (a, b, c) {
        if (typeof (a) != "undefined") {
            this.PerBetCount = a
        }
        if (typeof (b) != "undefined") {
            this.PerBetMoney = b
        }
        if (typeof (c) != "undefined") {
            this.PerBonusMoney = c
        }
        if (this.ShowObject == this.ObjBetTool && (MenuBet_ComputeBonusMoney ? this.PerBonusMoney.MinBonusMoney == 0 : this.PerBonusMoney.length == 0)) {
            this.toggleDuoQi(null, this.ObjBetTool)
        }
        $(this.ShowObject).triggerHandler(this.ShowObject.Handler_PerBetInfoChanged, [this.PerBetCount, this.PerBetMoney, this.PerBonusMoney])
    }
    , refreshListIssueInfoForBet: function (a) {
        if (a) {
            this.ListIssueInfoForBet.clear();
            for (var b = 0;
            b < a.length;
            b++) {
                this.ListIssueInfoForBet.push(a[b])
            }

        }
        this.ObjCurrent.BetIssue.IssueNumber = this.ListIssueInfoForBet[0].IssueNumber;
        this.ObjZhuiHao.ListBetIssueInfo = this.ListIssueInfoForBet;
        this.ObjBetTool.ListBetIssueInfo = this.ListIssueInfoForBet;
        $(this.ShowObject).triggerHandler("IssueList_UpdateIssueInfoForBet")
    }
    , event__duoqiChanged: function () {
        this.showChange($(this.CBoxDuoQi).prop("checked") == true ? this.ObjZhuiHao : this.ObjCurrent)
    }
    , toggleDuoQi: function (b, a) {
        if (a == this.ObjZhuiHao) {
            if (MenuBet_ComputeBonusMoney ? this.PerBonusMoney.MinBonusMoney > 0 : this.PerBonusMoney.length > 0) {
                this.showChange(this.ObjBetTool)
            }
            else {
                var c = $('<div class="dia_c_main"><div class="cet"><b class="plaint">&nbsp;</b><span>当前投注单内容不能使用倍投工具！</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>').find("a[class*=Dbutt]").click(function () {
                    $.artDialog.get("artDialog_FailBetTool").close()
                }).end();
                $.artDialog({
                    id: "artDialog_FailBetTool", title: "提示", content: $(c).get(0)
                })
            }

        }
        else {
            this.showChange(this.ObjZhuiHao)
        }

    }
    , initialize: function () {
        this.CBoxDuoQi = $("#cnn").click(this.event__duoqiChanged.bind(this));
        this.ObjCurrent = this.ObjCurrent.initialize();
        this.ObjZhuiHao = this.ObjZhuiHao.initialize();
        this.ObjBetTool = this.ObjBetTool.initialize();
        $(this.ObjCurrent).bind(this.ObjCurrent.Handler_ComputeChanged, this.issueList_compute.bind(this));
        $(this.ObjZhuiHao).bind(this.ObjZhuiHao.Handler_ComputeChanged, this.issueList_compute.bind(this));
        $(this.ObjZhuiHao).bind(this.ObjZhuiHao.Handler_ToggleBetTool, this.toggleDuoQi.bind(this));
        $(this.ObjBetTool).bind(this.ObjBetTool.Handler_ComputeChanged, this.issueList_compute.bind(this));
        $(this.ObjBetTool).bind(this.ObjBetTool.Handler_ToggleZhuiHao, this.toggleDuoQi.bind(this));
        this.event__duoqiChanged();
        return this
    }

};
TK.Bet.Service.prototype = {
    BetProject: new TK.Bet.Util.Project(), PerBetCount: 0, PerBetMoney: 0, ListIssueInfoForBet: [], LotteryType: TK.LotteryType.weizhi未知, BetRecord: new TK.Bet.Service.BetRecord(), MenuBet: new TK.Bet.Service.MenuBet(), BetList: new TK.Bet.Service.BetList(), IssueList: new TK.Bet.Service.IssueList(), DomBetInfo: null, DomBtnSubmit: null, AutoClosedStatus: false, StatusBetting: false, clear: function () {
        this.BetList.clear()
    }
    , refresh: function () {
        this.BetProject.TotalBetMoney = 0;
        for (var a = 0;
        a < this.BetProject.ListBetIssue.length;
        a++) {
            var b = $.extend(new TK.Bet.Util.BetIssue(), this.BetProject.ListBetIssue[a]);
            this.BetProject.TotalBetMoney += this.PerBetMoney * b.Multiple
        }
        $(this.DomBetInfo).html(String.format("共{0}注，购买{1}期，合计：￥{2}", this.PerBetCount.toMoney(), this.BetProject.ListBetIssue.length, this.BetProject.TotalBetMoney.toMoney()))
    }
    , event_BetList_ItemChanged: function (d, b) {
        this.PerBetCount = this.PerBetMoney = 0;
        var a = MenuBet_ComputeBonusMoney ? {
            MinBonusMoney: 0
        }
        : [];
        this.BetProject.ListBetItem.clear();
        if (!MenuBet_ComputeBonusMoney) {
            var c = {
                pt: null, computeOk: true
            }

        }
        for (var f = 0;
        f < b.length;
        f++) {
            var g = b[f];
            if ($S.Debug.IntelliSense) {
                g = new TK.Bet.Util.BetItem()
            }
            this.PerBetCount += g.BetCount;
            this.PerBetMoney += g.BetMoney;
            switch (TK.CurrentLotteryType) {
                case TK.LotteryType.jxssc江西时时彩: if (g.BetMethod == TK.Bet.BetMethod.和值选号) {
                    g.BetMethod = TK.Bet.BetMethod.常规选号
                }
                    break;
                case TK.LotteryType.jx11x5江西11选5: case TK.LotteryType.sd11x5山东11选5: case TK.LotteryType.gd11x5广东11选5: case TK.LotteryType.cq11x5重庆11选5: if (g.BetMethod == TK.Bet.BetMethod.胆拖选号) {
                    g.BetMethod = TK.Bet.BetMethod.常规选号
                }
                    break
            }
            this.BetProject.ListBetItem.push(g);
            if (MenuBet_ComputeBonusMoney) {
                a = $.extend(a, this.MenuBet.getBonusMoney(g));
                a.MinBonusMoney = a.MinBonusMoney == 0 || a.minBonusMoney < a.MinBonusMoney ? a.minBonusMoney : a.MinBonusMoney
            }
            else {
                if (c.computeOk == true) {
                    if (c.pt == null) {
                        c.pt = g.PlayType
                    }
                    else {
                        if (c.pt != g.PlayType) {
                            c.computeOk = false;
                            continue
                        }

                    }
                    var m = this.MenuBet.getBonusMoney(g);
                    if (c["ptno_" + g.BetNumber] == null) {
                        c["ptno_" + g.BetNumber] = m
                    }
                    else {
                        for (var h = 0;
                        h < c["ptno_" + g.BetNumber].length;
                        h++) {
                            c["ptno_" + g.BetNumber][h] += m[h]
                        }

                    }

                }

            }

        }
        if (!MenuBet_ComputeBonusMoney && c.computeOk) {
            for (var l in c) {
                if (l != "pt" && l != "computeOk") {
                    a = a.concat(c[l])
                }

            }
            a.clearRepeat().sort(function (e, i) {
                return e - i
            })
        }
        this.IssueList.perBetInfoChanged(this.PerBetCount, this.PerBetMoney, a)
    }
    , event_IssueList_ItemChanged: function (c, b, a) {
        this.BetProject.ListBetIssue = b;
        this.BetProject.DropBonusLevel = a.DropBonusLevel;
        this.BetProject.DropEarlyLevel = a.DropEarlyLevel;
        this.refresh()
    }
    , responseSubmit: function (b) {
        this.StatusBetting = false;
        var e = new $S.JsonCommand().parse(JSON.parse(b));
        var c, f = this;
        if (e.Command == 0) {
            c = $(String.format('<div class="dia_c_main"><div class="teLeft"><p class="tipok">方案已提交</p><p>方案编号：{0}</p><p>起始期数：{1}</p><p>总金额：￥{2}</p></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>', e.ListParameter[1], e.ListParameter[3], this.BetProject.TotalBetMoney.toMoney())).find("a.Dbutt").click(function () {
                $.artDialog.get("artDialog_Submit").close()
            }).end();
            this.BetRecord.handlerBetSubmitOK();
            this.clear();
            $.artDialog.get("artDialog_Submit").content(c.get(0));
            $(this.IssueList.ObjZhuiHao.DomInputIssueCount).val(10);
            this.IssueList.ObjZhuiHao.issueCountChanged();
            $(this.IssueList.CBoxDuoQi).get(0).checked = false;
            this.IssueList.event__duoqiChanged()
        }
        else {
            var a = [], d = TK.Bet.ConvertResultStatus({
                ObjStatus: e, OutputButton: a
            });
            $.artDialog.tkAlert(d, false, null, null, a, "artDialog_Submit")
        }

    }
    , confirmSubmit: function (b, d) {
        if (this.StatusBetting) {
            return
        }
        var a = this;
        this.StatusBetting = true;
        $(d).parent().html("").append('<span class="d-loading">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>正在提交数据，请稍后……').end().remove();
        var c = new $S.JsonCommand();
        c.Command = "bet";
        c.ListParameter.push(this.BetProject.toJSONString());
        $.ajax({
            type: "post", url: "/handler/kuaikai/bet.ashx", data: {
                d: c.toJSONString()
            }
            , success: this.responseSubmit.bind(this), error: function (f) {
                a.StatusBetting = false;
                var g = $('<div class="dia_c_main"><div class="cet"><b class="plaint">&nbsp;</b><span>' + f.statusText + '</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>').find("a.Dbutt").click(function () {
                    $.artDialog.get("artDialog_Submit").close()
                }).end();
                $.artDialog.get("artDialog_Submit").title("系统异常").content(g.get(0))
            }

        })
    }
    , submit: function () {
        var a = this.checkBetStatus();
        if (a.Status != TK.BetStatus.Enum.销售) {
            $.artDialog.tkAlert(a.Remark);
            return
        }
        if (TK.User.IsLogined != true) {
            $(TK.User).bind(TK.User.Handler_LoginSuccess, this.submit.bind(this));
            TK.User.showLoginForm();
            return
        }
        this.event_IssueList_ItemChanged(null, this.IssueList.getListBetIssue(), this.IssueList.getStopConfig());
        var b = null, c = this;
        if (this.BetProject.ListBetItem.length == 0) {
            b = $('<div class="dia_c_main"><div class="cet"><b class="plaint">&nbsp;</b><span>投注单为空</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>').find("a[class*=Dbutt]").click(function () {
                $.artDialog.get("artDialog_BetError").close()
            }).end();
            $.artDialog({
                id: "artDialog_BetError", title: "提示", content: $(b).get(0)
            });
            return
        }
        if (this.BetProject.ListBetIssue.length == 0) {
            b = $('<div class="dia_c_main"><div class="cet"><b class="plaint">&nbsp;</b><span>至少选择一期购买</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>').find("a[class*=Dbutt]").click(function () {
                $.artDialog.get("artDialog_BetError").close()
            }).end();
            $.artDialog({
                id: "artDialog_BetError", title: "提示", content: $(b).get(0)
            });
            return
        }
        b = $(String.format('<div class="dia_c_main"><div class="teLeft"><p class="plaint">请确认以下信息，确认无误后请点击确认按钮。</p><p>注数：{0}</p><p>起始期次：{1}</p><p>期数：{2}</p><p>总金额：￥{3}</p></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a><a class="cancel">取　消</a></div></div>', this.PerBetCount.toMoney(), this.BetProject.ListBetIssue[0].IssueNumber, this.BetProject.ListBetIssue.length, this.BetProject.TotalBetMoney.toMoney())).find("a.Dbutt").click(function () {
            $(c).triggerHandler("submit", [this])
        }).end();
        b.find("a.cancel").click(function () {
            $.artDialog.get("artDialog_Submit").close()
        });
        $.artDialog({
            id: "artDialog_Submit", title: "提示", content: $(b).get(0)
        })
    }
    , autoClose: function (a, b) {
        if (this.AutoClosedStatus) {
            return
        }
        var f = this;
        var c = $.artDialog.get("artDialog_BetIssueChanged");
        if (b <= 0) {
            if (typeof c != "undefined") {
                c.close()
            }
            return
        }
        var e = $(String.format(a, b));
        var d = e.find("a.Dbutt").click(function () {
            c.close();
            f.AutoClosedStatus = true
        });
        if (typeof c == "undefined") {
            $.artDialog({
                id: "artDialog_BetIssueChanged", title: "提示", content: e.get(0), beforeunload: function () {
                    f.AutoClosedStatus = true
                }

            })
        }
        else {
            c.content(e.get(0))
        }
        setTimeout(this.autoClose.bind(this, a, b - 1), 1000)
    }
    , event_BetIssueChanged: function (c, a, b) {
        for (var d = 0;
        d < this.ListIssueInfoForBet.length;
        d++) {
            if (this.ListIssueInfoForBet[0].IssueNumber > b) {
                return
            }
            if (this.ListIssueInfoForBet[0].IssueNumber < b) {
                this.ListIssueInfoForBet.removeAt(0);
                continue
            }
            if (this.ListIssueInfoForBet[0].IssueNumber == b) {
                break
            }

        }
        this.IssueList.refreshListIssueInfoForBet(this.ListIssueInfoForBet);
        this.AutoClosedStatus = false;
        this.autoClose(String.format('<div class="dia_c_main"><div class="teLeft"><p class="plaint">第{0}期投注已截止！</p><p>当前实时投注期次：{1}</p><p><i>{2}</i>秒后自动关闭……</p></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>', a, b, "{0}"), 5)
    }
    , checkBetStatus: function () {
        var a = TK.BetStatus[TK.CurrentLotteryType];
        if ($S.Debug.IntelliSense) {
            a = TK.BetStatus.parse()
        }
        if (a.Status == TK.BetStatus.Enum.销售) {
            $(this.DomBtnSubmit).removeClass("stopSale")
        }
        else {
            $(this.DomBtnSubmit).addClass("stopSale")
        }
        return a
    }
    , initialize: function () {
        $(this).bind("submit", this.confirmSubmit.bind(this));
        this.BetRecord = this.BetRecord.initialize();
        this.MenuBet = new TK.Bet.Service.MenuBet().initialize();
        TK.Bet.Service.initMenuBetPrototype(this.MenuBet);
        this.BetList = this.BetList.initialize();
        this.IssueList = this.IssueList.initialize();
        $(this.MenuBet).bind("menuBet_addItem", this.BetList.addItem.bind(this.BetList));
        $(this.BetList).bind(this.BetList.Handler_itemChanged, this.event_BetList_ItemChanged.bind(this));
        $(this.IssueList).bind(this.IssueList.Handler_itemChanged, this.event_IssueList_ItemChanged.bind(this));
        this.DomBtnSubmit = $("#Bet_BtnSubmit").click(this.submit.bind(this));
        this.checkBetStatus();
        this.DomBetInfo = $(this.DomBtnSubmit).parent().prev("span.sum");
        for (var a = 0;
        a < this.ListIssueInfoForBet.length;
        a++) {
            this.ListIssueInfoForBet[a] = {
                IssueNumber: this.ListIssueInfoForBet[a].i
            }

        }
        this.IssueList.refreshListIssueInfoForBet(this.ListIssueInfoForBet);
        $(TK.Video).bind(TK.Video.Handler_BetIssueEnd, this.event_BetIssueChanged.bind(this));
        return this
    }

};
var kkBet = new TK.Bet.Service();
(function (d, c) {
    var b = function () { };
    b.prototype = {
        PInstance: null, ObjSocket: new $S.Socket(), Disabled: false, getStatus: function () {
            return this.Disabled ? false : this.ObjSocket.status()
        }
        , receive: function (f, g) {
            this.PInstance.response(g)
        }
        , initialize: function (e, f, g) {
            this.PInstance = e;
            f = f || "";
            g = g || parseInt(TK.CurrentLotteryType.toFormatString("28000"), 10);
            this.ObjSocket = new $S.Socket("TK.Command.Socket.ObjSocket", f, g);
            $(this.ObjSocket).bind(this.ObjSocket.Handler.Receive, this.receive.bind(this));
            this.ObjSocket.init()
        }

    };
    var a = function () { };
    a.prototype = {
        Enum: {
            Video: 0, RecentBetRecord: 1, RecentTicketRecord: 2, RecentBetMiss: 3
        }
        , GetMiss: true, Socket: new b(), response: function (f) {
            var h = null;
            try {
                h = JSON.parse(f)
            }
            catch (g) {
                $S.Debug.log("获取数据解析错误", f);
                return
            }
            f = new $S.JsonCommand().parse(h);
            switch (parseInt(f.Command, 10)) {
                case this.Enum.Video: TK.Video.response(f.ListParameter);
                    break;
                case this.Enum.RecentBetRecord: $(this).triggerHandler("Response_" + f.Command, [f.ListParameter]);
                    break;
                case this.Enum.RecentTicketRecord: $(this).triggerHandler("Response_" + f.Command, [f.ListParameter]);
                    break;
                case this.Enum.RecentBetMiss: this.GetMiss = false;
                    $(this).triggerHandler("RetBetMiss", [f]);
                    break
            }

        }
        , sendSocket: function (e) {
            e = $.extend(new $S.JsonCommand(), e);
            this.Socket.ObjSocket.send(e.toJSONString())
        }
        , sendAjax: function (e) {
            e = $.extend(new $S.JsonCommand(), e);
            switch (e.Command) {
                case this.Enum.Video: case this.Enum.RecentBetRecord: case this.Enum.RecentTicketRecord: e.ListParameter.insertAt(0, TK.CurrentLotteryType);
                    break;
                case this.Enum.RecentBetMiss: if (!this.GetMiss && !TK.User.IsLogined) {
                    return
                }
                    break
            }
            $.ajax({
                type: "post", url: "/handler/kuaikai/command.ashx", data: {
                    d: e.toJSONString()
                }
                , success: this.response.bind(this)
            })
        }
        , send: function (e) {
            var f = {
                CommandData: null, RecallTimeout: 0
            };
            f.RecallFunction = this.send.bind(this, e.CommandData);
            e = $.extend(f, e);
            if (e.CommandData == null) {
                return
            }
            if (this.Socket.getStatus()) {
                if (e.RecallTimeout > 0) {
                    setTimeout(e.RecallFunction, e.RecallTimeout)
                }
                else {
                    this.sendSocket(e.CommandData)
                }

            }
            else {
                this.sendAjax(e.CommandData)
            }

        }

    };
    if (typeof TK.Command === "undefined") {
        TK.Command = new a()
    }

})(window);
(function (g, e) {
    var d = function () { };
    d.prototype = {
        IssueNumber: "", EndTime: new Date(), parse: function (h) {
            return {
                IssueNumber: h.n || "", EndTime: h.t.toDate() || new Date()
            }

        }

    };
    var c = function () { };
    c.prototype = {
        IssueNumber: "", BonusNumber: "", parse: function (h) {
            return {
                IssueNumber: h.i || "", BonusNumber: h.b || ""
            }

        }

    };
    var b = function () { };
    b.prototype = {
        Object: null, FlashDataModel: function () {
            return {
                BetIssue: "", BetLeaveTime: 0, TrueIssue: "", DrawLeaveTime: 0, BonusIssue: "", BonusNumber: ""
            }

        }
        , chargeGroup: function () {
            var h = 1;
            switch (TK.CurrentLotteryType) {
                case TK.LotteryType.cqssc重庆时时彩: case TK.LotteryType.jxssc江西时时彩: case TK.LotteryType.tjssc天津时时彩: h = 1;
                    break;
                case TK.LotteryType.sd11x5山东11选5: case TK.LotteryType.jx11x5江西11选5: case TK.LotteryType.gd11x5广东11选5: case TK.LotteryType.cq11x5重庆11选5: case TK.LotteryType.gdkl10广东快乐十分: case TK.LotteryType.xync重庆幸运农场: h = 2;
                    break;
                case TK.LotteryType.jsk3江苏快3: h = 3;
                    break;
                case TK.LotteryType.sdqyh山东群英会: break
            }
            return h
        }
        , chargeGroupColor: function () {
            var h = "";
            switch (TK.CurrentLotteryType) {
                case TK.LotteryType.cqssc重庆时时彩: break;
                case TK.LotteryType.jxssc江西时时彩: h = "&color=005500";
                    break;
                case TK.LotteryType.tjssc天津时时彩: h = "&color=c50159";
                    break
            }
            return h
        }
        , chargeGroupDic: function () {
            var h = "BetFlash";
            switch (TK.CurrentLotteryType) {
                case TK.LotteryType.gdkl10广东快乐十分: case TK.LotteryType.xync重庆幸运农场: h += "_ball8";
                    break;
                case TK.LotteryType.sdqyh山东群英会: h += "_qyh";
                    break
            }
            return h
        }
        , getActString: function (n, p) {
            var o = "";
            var m = new Array();
            switch (p) {
                case "三星形态": o = n.charAt(0) == n.charAt(1) && n.charAt(0) == n.charAt(2) ? "豹子" : (n.charAt(0) == n.charAt(1) || n.charAt(0) == n.charAt(2) || n.charAt(1) == n.charAt(2) ? "组3" : "组6");
                    break;
                case "二星和值": o = parseInt(n.charAt(0), 10) + parseInt(n.charAt(1), 10);
                    break;
                case "三星和值": case "和值": o = parseInt(n.charAt(0), 10) + parseInt(n.charAt(1), 10) + parseInt(n.charAt(2), 10);
                    break;
                case "大小单双": m.push([parseInt(n.charAt(0), 10) > 4 ? "大" : "小", parseInt(n.charAt(0), 10) % 2 == 1 ? "单" : "双"]);
                    m.push([parseInt(n.charAt(1), 10) > 4 ? "大" : "小", parseInt(n.charAt(1), 10) % 2 == 1 ? "单" : "双"]);
                    var h = "";
                    for (var k = 0;
                    k < m[0].length;
                    k++) {
                        o += k == 0 ? "" : " ";
                        h = m[0][k];
                        for (var l = 0;
                        l < m[1].length;
                        l++) {
                            o += l == 0 ? "" : " ";
                            o += h + m[1][l]
                        }

                    }
                    break;
                case "小区": for (var k = 0;
                k < n.length;
                k++) {
                    if (parseInt(n[k], 10) < 11) {
                        m.push(n[k])
                    }

                }
                    o = m.toString();
                    break;
                case "中区": for (var k = 0;
                k < n.length;
                k++) {
                    if (parseInt(n[k], 10) > 10 && parseInt(n[k], 10) < 21) {
                        m.push(n[k])
                    }

                }
                    o = m.toString();
                    break;
                case "大区": for (var k = 0;
                k < n.length;
                k++) {
                    if (parseInt(n[k], 10) > 20) {
                        m.push(n[k])
                    }

                }
                    o = m.toString();
                    break;
                case "任选一": break
            }
            return o.toString() == "" ? "-" : o.toString()
        }
        , chargeBonusInfo: function (h) {
            var j = [];
            switch (TK.CurrentLotteryType) {
                case TK.LotteryType.cqssc重庆时时彩: case TK.LotteryType.jxssc江西时时彩: case TK.LotteryType.tjssc天津时时彩: j.push({
                    Name: "三星形态", Value: h === "" ? "-" : this.getActString(h.substr(2), "三星形态")
                });
                    j.push({
                        Name: "二星和值", Value: h === "" ? "-" : this.getActString(h.substr(3), "二星和值")
                    });
                    j.push({
                        Name: "三星和值", Value: h === "" ? "-" : this.getActString(h.substr(2), "三星和值")
                    });
                    j.push({
                        Name: "大小单双", Value: h === "" ? "-" : this.getActString(h.substr(3), "大小单双")
                    });
                    break;
                case TK.LotteryType.sd11x5山东11选5: case TK.LotteryType.jx11x5江西11选5: case TK.LotteryType.gd11x5广东11选5: case TK.LotteryType.cq11x5重庆11选5: j.push({
                    Name: "任选一", Value: h === "" ? "-" : h.split(",")[0]
                });
                    j.push({
                        Name: "前二位", Value: h === "" ? "-" : h.substr(0, 5)
                    });
                    j.push({
                        Name: "前三位", Value: h === "" ? "-" : h.substr(0, 8)
                    });
                    break;
                case TK.LotteryType.tjkl10天津快乐十分: case TK.LotteryType.gdkl10广东快乐十分: case TK.LotteryType.xync重庆幸运农场: var k = h.split(",");
                    k.sort(function (i, n) {
                        return parseInt(i, 10) - parseInt(n, 10)
                    });
                    j.push({
                        Name: "开奖号码排序", Value: h === "" ? "-" : k.join(",")
                    });
                    break;
                case TK.LotteryType.jsk3江苏快3: j.push({
                    Name: "和值", Value: h === "" ? "-" : this.getActString(h, "和值")
                });
                    var k = [], m = "-";
                    for (var l = 0;
                    l < h.length;
                    l++) {
                        k.push(h.charAt(l))
                    }
                    switch (k.clearRepeat().length) {
                        case 1: m = "三同号";
                            break;
                        case 2: m = "二同号";
                            break;
                        case 3: if (k.length == 3 && parseInt(k[2], 10) - parseInt(k[1], 10) == parseInt(k[1], 10) - parseInt(k[0], 10)) {
                            m = "三连号"
                        }
                        else {
                            m = "三不同"
                        }
                            break
                    }
                    j.push({
                        Name: "形态", Value: m
                    });
                    break
            }
            return j
        }
        , setDataToFlash: function (h) {
            if (typeof this.Object !== "object") {
                setTimeout(this.setDataToFlash.bind(this, h), 10);
                return
            }
            if (typeof this.Object.BetFlash !== "function") {
                setTimeout(this.setDataToFlash.bind(this, h), 10);
                return
            }
            var i = {
                DataModel: $.extend(new this.FlashDataModel(), h)
            };
            i.Status = i.DataModel.BetIssue == i.DataModel.TrueIssue ? 0 : 1;
            i.BonusInfo = this.chargeBonusInfo(i.DataModel.BonusNumber);
            this.Object.BetFlash(i)
        }
        , chargeImcome: function () {
            return "&ad=1"
        }
        , initialize: function (h) {
            this.Object = $($S.Flash.create({
                Id: "FlashSWF", Width: 470, Height: 183, Movie: String.format("/media/{0}.swf?lt={1}&group={2}{3}{4}", this.chargeGroupDic(), TK.CurrentLotteryType.toLotteryName(), this.chargeGroup(), this.chargeGroupColor(), this.chargeImcome())
            })).appendTo(h).get(0)
        }

    };
    var a = function () { };
    a.prototype = {
        Flash: new b(), setData: function (h, i, j) {
            h = $.extend(new d(), h);
            i = $.extend(new d(), i);
            j = $.extend(new c(), j);
            var l = $S.Date.getServerTime();
            var k = new this.Flash.FlashDataModel();
            k.BetIssue = h.IssueNumber;
            k.BetLeaveTime = (h.EndTime - l) / 1000;
            k.TrueIssue = i.IssueNumber;
            k.DrawLeaveTime = (i.EndTime - l) / 1000;
            k.BonusIssue = j.IssueNumber;
            k.BonusNumber = j.BonusNumber;
            this.Flash.setDataToFlash(k)
        }
        , initialize: function (h) {
            this.Flash.initialize(h || "div.lotNow")
        }

    };
    var f = function () { };
    f.prototype = {
        BetIssueData: new d(), BetNextIssueData: new d(), CurrentIssueData: new d(), CurrentBakIssueData: new d(), CurrentNextIssueData: new d(), LastIssueData: new c(), Movie: new a(), ObjTimeout: null, MaxWaitBonusTime: 30000, Handler_BetIssueEnd: "Handler_BetIssueEnd", Handler_GetBonus: "Handler_GetBonus", Handler_GetRecentBetRecord: "Handler_GetRecentBetRecord", LastRequestTime: $S.Date.getServerTime().addMinutes(-11), HasBetDataInfo: true, clock: function () {
            var h = $S.Date.getServerTime();
            if ((this.BetIssueData.EndTime - h) <= 0) {
                this.event_betIssueTimeOver()
            }
            if ((this.CurrentIssueData.EndTime - h) <= 0) {
                this.event_currentIsueTimeOver()
            }

        }
        , setBetIssueData: function (h, i) {
            this.BetIssueData = $.extend(this.BetIssueData, h);
            if (typeof i !== "undefined") {
                this.BetNextIssueData = $.extend(this.BetNextIssueData, i)
            }

        }
        , setCurrentIssueData: function (h, i) {
            this.CurrentIssueData = $.extend(this.CurrentIssueData, h);
            if (typeof i !== "undefined") {
                this.CurrentNextIssueData = $.extend(this.CurrentNextIssueData, i)
            }

        }
        , response: function (i) {
            if (i == null) {
                this.Movie.setData(this.BetIssueData, this.CurrentIssueData, this.LastIssueData);
                return
            }
            if (i.length == 2) {
                this.response(JSON.parse(i[0]));
                $(this).triggerHandler(this.Handler_GetRecentBetRecord, [JSON.parse(i[1])]);
                return
            }
            var h = this;
            if (i[2] != "" && i[2] != "[]") {
                var n = this.LastIssueData.IssueNumber.length > 0 && this.LastIssueData.BonusNumber == "";
                var s = new c(), j = this.LastIssueData;
                try {
                    s = new c().parse(JSON.parse(i[2]))
                }
                catch (r) {
                    $S.Debug.log("video[上一期次信息转换出错]", r.message + ":" + i[2])
                }
                if (s.IssueNumber >= this.LastIssueData.IssueNumber) {
                    this.LastIssueData = s
                }
                if (!this.LastIssueData.BonusNumber || this.LastIssueData.BonusNumber.length == 0) {
                    this.ObjTimeout = setTimeout(this.request.bind(this, {
                        last: true
                    }), this.MaxWaitBonusTime);
                    this.MaxWaitBonusTime = 5 * 1000
                }
                else {
                    if (n) {
                        clearTimeout(this.ObjTimeout);
                        this.MaxWaitBonusTime = 30000;
                        if (($S.Date.getServerTime().getTime() - this.CurrentBakIssueData.EndTime.getTime()) < 2000) {
                            setTimeout(function () {
                                h.LastIssueData = j;
                                h.response(i)
                            }
                            , 2000);
                            return
                        }
                        $(this).triggerHandler(this.Handler_GetBonus, [this.LastIssueData.IssueNumber, this.LastIssueData.BonusNumber])
                    }

                }

            }
            if (i[0] != "" && i[0] != "[]") {
                var k = JSON.parse(i[0]);
                var l = new d().parse(k[0]);
                var m = new d().parse(k[1]);
                if (l.IssueNumber < this.BetIssueData.IssueNumber) {
                    $S.Date.update();
                    if (m.IssueNumber == this.BetIssueData.IssueNumber) {
                        this.setBetIssueData(m)
                    }
                    setTimeout(this.request.bind(this, {
                        bet: true
                    }), 2 * 60 * 1000)
                }
                else {
                    this.setBetIssueData(l, m)
                }

            }
            if (i[1] != "" && i[1] != "[]") {
                var o = JSON.parse(i[1]);
                var p = new d().parse(o[0]);
                var q = new d().parse(o[1]);
                if (p.IssueNumber < this.CurrentIssueData.IssueNumber) {
                    $S.Date.update();
                    if (q.IssueNumber == this.CurrentIssueData.IssueNumber) {
                        this.setCurrentIssueData(q)
                    }
                    setTimeout(this.request.bind(this, {
                        current: true
                    }), 2 * 60 * 1000)
                }
                else {
                    this.setCurrentIssueData(p, q)
                }

            }
            this.Movie.setData(this.BetIssueData, this.CurrentIssueData, this.LastIssueData)
        }
        , request: function (i, h, j) {
            if (($S.Date.getServerTime() - this.LastRequestTime) <= 10000) {
                setTimeout(this.request.bind(this, i, h), 10000);
                return
            }
            this.LastRequestTime = $S.Date.getServerTime();
            i = $.extend({
                bet: false, current: false, last: false
            }
            , i);
            var k = new $S.JsonCommand();
            k.Command = TK.Command.Enum.Video;
            k.ListParameter = [i.bet ? 1 : 0, i.current ? 1 : 0, i.last ? 1 : 0];
            h = h ? 0 : 2000;
            if (j == true) {
                setTimeout(TK.Command.sendAjax.bind(TK.Command, k), h)
            }
            else {
                TK.Command.send({
                    CommandData: k, RecallTimeout: h, RecallFunction: this.request.bind(this, i, true)
                })
            }

        }
        , event_betIssueTimeOver: function () {
            if (this.BetIssueData.IssueNumber == "") {
                return
            }
            $(this).triggerHandler(this.Handler_BetIssueEnd, [this.BetIssueData.IssueNumber, this.BetNextIssueData.IssueNumber]);
            this.BetIssueData = this.BetNextIssueData;
            this.BetNextIssueData = new d();
            this.Movie.setData(this.BetIssueData, this.CurrentIssueData, this.LastIssueData);
            this.request({
                bet: true
            }
            , null, this.HasBetDataInfo && TK.User.IsLogined)
        }
        , event_currentIsueTimeOver: function () {
            if (this.CurrentIssueData.IssueNumber == "") {
                return
            }
            this.LastIssueData.IssueNumber = this.CurrentIssueData.IssueNumber;
            this.LastIssueData.BonusNumber = "";
            this.CurrentBakIssueData = this.CurrentIssueData;
            this.CurrentIssueData = this.CurrentNextIssueData;
            this.CurrentNextIssueData = new d();
            this.Movie.setData(this.BetIssueData, this.CurrentIssueData, this.LastIssueData);
            this.request({
                current: true, last: true
            })
        }
        , initialize: function (k, h, i, j) {
            this.Movie.initialize(k);
            if (typeof (j) != "undefined") {
                this.response(j)
            }
            setInterval(this.clock.bind(this), 1000);
            TK.Command.Socket.initialize(TK.Command, h, i);
            if (this.BetIssueData.IssueNumber == "" || this.BetNextIssueData.IssueNumber == "" || this.CurrentIssueData.IssueNumber == "" || this.CurrentNextIssueData.IssueNumber == "" || this.LastIssueData.IssueNumber == "") {
                this.request({
                    current: true, last: true, bet: true
                })
            }
            return this
        }

    };
    if (typeof TK.Video === "undefined") {
        TK.Video = new f()
    }

})(window);
(function (d, c) {
    function b() { } b.prototype = {
        IssueIndex: 0, Multiple: 0, BetMoney: 0, TotalBetMoney: 0, BonusMinMoney: 0, BonusMaxMoney: 0, ProfitsMinMoney: 0, ProfitsMaxMoney: 0, ProfitsMinRate: 0, ProfitsMaxRate: 0, getProfitsPercentRate: function (e) {
            return Math.floor(e * 10000) / 100
        }

    };
    b.enumType = {
        全程利润率: 1, 两步利润率: 2, 全程利润金额: 3, 两步利润金额: 4
    };
    function a() { } a.prototype = {
        LimitBetMoney: 1000000, FirstMultiple: 0, IssueCount: 0, PerBetMoney: 0, PerMinBonusMoney: 0, PerMaxBonusMoney: 0, ListPlan: [], AllProfitsRate: 0, AllProfitsMoney: 0, SplitIssueCount: 0, ForepartProfitsRate: 0, ForepartProfitsMoney: 0, AfterpartProfitsRate: 0, AfterpartProfitsMoney: 0, createSerial: function (f, e) {
            var g = new b();
            g.Multiple = f;
            g.BetMoney = g.Multiple * this.PerBetMoney;
            g.TotalBetMoney = (e || 0) + g.BetMoney;
            g.BonusMinMoney = g.Multiple * this.PerMinBonusMoney;
            g.BonusMaxMoney = g.Multiple * this.PerMaxBonusMoney;
            g.ProfitsMinMoney = g.BonusMinMoney - g.TotalBetMoney;
            g.ProfitsMaxMoney = g.BonusMaxMoney - g.TotalBetMoney;
            g.ProfitsMinRate = g.ProfitsMinMoney / g.TotalBetMoney;
            g.ProfitsMaxRate = g.ProfitsMaxMoney / g.TotalBetMoney;
            return g
        }
        , checkProfits: function (g, f, e) {
            var h = true;
            if ($S.Debug.IntelliSense) {
                f = new b()
            }
            switch (g) {
                case b.enumType.全程利润率: h = f.ProfitsMinRate >= this.AllProfitsRate;
                    break;
                case b.enumType.全程利润金额: h = f.ProfitsMinMoney >= this.AllProfitsMoney;
                    break;
                case b.enumType.两步利润率: if (e < this.SplitIssueCount) {
                    h = f.ProfitsMinRate >= this.ForepartProfitsRate
                }
                else {
                    h = f.ProfitsMinRate >= this.AfterpartProfitsRate
                }
                    break;
                case b.enumType.两步利润金额: if (e < this.SplitIssueCount) {
                    h = f.ProfitsMinMoney >= this.ForepartProfitsMoney
                }
                else {
                    h = f.ProfitsMinMoney >= this.AfterpartProfitsMoney
                }
                    break
            }
            return h
        }
        , computeWithoutParameter: function (e) {
            this.ListPlan.clear();
            var f = typeof (e) == "number" ? e : this.FirstMultiple;
            var j = typeof (e) == "object" ? e : [];
            var h = 0;
            for (var g = 0;
            g < this.IssueCount;
            g++) {
                var k = j.length > g ? j[g] : f;
                var l = this.createSerial(k, h);
                f = l.Multiple;
                h = l.TotalBetMoney;
                this.ListPlan.push(l)
            }
            return this.ListPlan
        }
        , compute: function (e) {
            this.ListPlan.clear();
            if (this.PerBetMoney == 0 || this.PerMinBonusMoney == 0) {
                return this.ListPlan
            }
            var g = this.FirstMultiple, h = 0;
            for (var f = 0;
            f < this.IssueCount;
            f++) {
                var j = this.createSerial(g, h), k = j;
                if (f == 0 && !this.checkProfits(e, j, f)) {
                    return this.ListPlan
                }
                while (true) {
                    if (this.checkProfits(e, k, f)) {
                        j = k;
                        k = null;
                        break
                    }
                    k = this.createSerial(k.Multiple + 1, h)
                }
                g = j.Multiple;
                h = j.TotalBetMoney;
                if (j.TotalBetMoney > this.LimitBetMoney) {
                    break
                }
                this.ListPlan.push(j)
            }
            return this.ListPlan
        }
        , computeByWholeRate: function (e) {
            this.AllProfitsRate = e;
            return this.compute(b.enumType.全程利润率)
        }
        , computeByWholeMoney: function (e) {
            this.AllProfitsMoney = e;
            return this.compute(b.enumType.全程利润金额)
        }
        , computeBySplitRate: function (g, f, e) {
            this.SplitIssueCount = g;
            this.ForepartProfitsRate = f;
            this.AfterpartProfitsRate = e;
            return this.compute(b.enumType.两步利润率)
        }
        , computeBySplitMoney: function (g, f, e) {
            this.SplitIssueCount = g;
            this.ForepartProfitsMoney = f;
            this.AfterpartProfitsMoney = e;
            return this.compute(b.enumType.两步利润金额)
        }

    };
    $S.PlanItem = b;
    $S.PlanList = a
})(window);
