TK.Miss = function () { };
TK.Miss.prototype = {
    IcyHotDate: [], SetIcyHot: function () { }, Init: function () {
        this.SetIcyHot()
    }

};
TK.CurrentLotteryType = kkBet.BetProject.LotteryType = TK.LotteryType.cqssc重庆时时彩;
MenuBet_ComputeBonusMoney = true;
TK.Bet.PlayType = {
    dxds大小单双: 99, yxds一星单式: 101, yxfs一星复式: 102, exds二星单式: 201, exfs二星复式: 202, exhz二星和值: 203, exzxds二星组选单式: 204, exzxfs二星组选复式: 205, exzxfwds二星组选分位单式: 206, exzxfwfs二星组选分位复式: 207, exzxhz二星组选和值: 208, exzxbd二星组选包胆: 209, sxds三星单式: 301, sxfs三星复式: 302, sxhz三星和值: 303, sxzxhz三星组选和值: 304, sxzxbd三组包胆: 305, sxzxsds三星组选三单式: 306, sxzxsfs三星组选三复式: 307, sxzxsdt三星组选三胆拖: 308, sxzxshz三星组选三和值: 309, sxzxlds三星组选六单式: 310, sxzxlfs三星组选六复式: 311, sxzxldt三星组选六胆拖: 312, sxzxlhz三星组选六和值: 313, sxzxzh三星直选组合: 314, wxds五星单式: 501, wxfs五星复式: 502, wxtxds五星通选单式: 503, wxtxfs五星通选复式: 504
};
TK.Bet.PlayType.parse = function (b) {
    var c = b;
    var e = c.toString();
    switch (c) {
        case TK.Bet.PlayType.sxzxhz三星组选和值: e = "三组包点";
            break;
        case TK.Bet.PlayType.exzxhz二星组选和值: e = "二组包点";
            break;
        default: for (var a in TK.Bet.PlayType) {
            if (TK.Bet.PlayType[a] == c) {
                var d = new RegExp("([^\u4E00-\u9FA5]*)(.*)");
                e = a.replace(d, "$2").replace(/单式|复式/, "");
                break
            }

        }
            break
    }
    return e
};
TK.Bet.Config = [{
    Menu: "二星和值", Name: "二星和值", Desc: '至少选择一个和值，竞猜开奖号码<i class="orange">后两位</i>数字之和，奖金<i class="orange">100</i>元。', Demo: [{
        th: "投注", td: "1"
    }
    , {
        th: "开奖", td: '***<i class="orange">01</i>、***<i class="orange">10</i>'
    }
    , {
        th: "奖金", td: "100元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.exhz二星和值]
}
, {
    Menu: "三星和值", Name: "三星和值", Desc: '至少选择一个和值，竞猜开奖号码<i class="orange">后三位</i>数字之和，奖金<i class="orange">1000</i>元。', Demo: [{
        th: "投注", td: "1"
    }
    , {
        th: "开奖", td: '**<i class="orange">001</i>、**<i class="orange">010</i>、**<i class="orange">100</i>'
    }
    , {
        th: "奖金", td: "1000元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.sxhz三星和值]
}
, {
    Menu: "五星通选", Name: "五星通选", Desc: '竞猜开奖号码的<i class="orange">全部五位</i>，猜中五位奖金<i class="orange">20000</i>元；猜中前三或后三奖金<i class="orange">200</i>元；猜中前二或后二奖金<i class="orange">20</i>元。', Demo: [{
        th: "投注", td: "45678"
    }
    , {
        th: "开奖", td: '<i class="orange">45678</i>'
    }
    , {
        th: "奖金", td: "20440元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    , {
        Value: TK.Bet.BetMethod.文件选号
    }
    ], PlayType: [TK.Bet.PlayType.wxtxds五星通选单式, TK.Bet.PlayType.wxtxfs五星通选复式]
}
, {
    Menu: "组选三", Name: "组选三", Desc: '至少选择<i class="orange">两个</i>号码投注，竞猜开奖号码<i class="orange">后三位</i>，开奖号码为<a href="/help/2/9/" target="_blank" style="margin-left:0px;">组三形态</a>，且号码都选中即中奖，奖金<i class="orange">320</i>元。', Demo: [{
        th: "投注", td: "67"
    }
    , {
        th: "开奖", td: '**<i class="orange">677</i>'
    }
    , {
        th: "奖金", td: "320元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号, SelectPlayType: [{
            Name: "单式", PlayType: [TK.Bet.PlayType.sxzxsds三星组选三单式]
        }
        , {
            Name: "复式", PlayType: [TK.Bet.PlayType.sxzxsfs三星组选三复式]
        }
        ]
    }
    , {
        Value: TK.Bet.BetMethod.文件选号
    }
    ], PlayType: [TK.Bet.PlayType.sxzxsds三星组选三单式, TK.Bet.PlayType.sxzxsfs三星组选三复式]
}
, {
    Menu: "组选六", Name: "组选六", Desc: '至少选择<i class="orange">三个</i>号码投注，竞猜开奖号码<i class="orange">后三位</i>，开奖号码为<a href="/help/2/9/" target="_blank" style="margin-left:0px;">组六形态</a>，且号码都选中即中奖，奖金<i class="orange">160</i>元。', Demo: [{
        th: "投注", td: "678"
    }
    , {
        th: "开奖", td: '**<i class="orange">678</i>'
    }
    , {
        th: "奖金", td: "160元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    , {
        Value: TK.Bet.BetMethod.文件选号
    }
    ], PlayType: [TK.Bet.PlayType.sxzxlds三星组选六单式, TK.Bet.PlayType.sxzxlfs三星组选六复式]
}
, {
    Menu: "三星直选组合", Name: "三星直选组合", Desc: '至少选择<i class="orange">三个</i>号码投注，竞猜开奖号码<i class="orange">后三位</i>，且开奖号码为<i class="blue">组六形态</i>即中奖，奖金<i class="orange">1000</i>元。', Demo: [{
        th: "投注", td: "678"
    }
    , {
        th: "开奖", td: '**<i class="orange">678</i>'
    }
    , {
        th: "奖金", td: "1000元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.sxzxzh三星直选组合]
}
, {
    Menu: "三组包点", Name: "三组包点", Desc: '至少选择一个和值，竞猜开奖号码<i class="orange">后三位</i>数字之和，后三位开奖号码为<i class="blue">豹子形态</i>奖金<i class="orange">1000</i>元；<i class="blue">组三形态</i>奖金<i class="orange">320</i>元；<i class="blue">组六形态</i>奖金<i class="orange">160</i>元。', Demo: [{
        th: "投注", td: "3"
    }
    , {
        th: "开奖", td: '**<i class="orange">012</i>'
    }
    , {
        th: "奖金", td: "160元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.sxzxhz三星组选和值]
}
, {
    Menu: "三组包胆", Name: "三组包胆", Desc: '竞猜开奖号码<i class="orange">后三位</i>的胆码，所有胆码开出即中奖，后三位开奖号码为<i class="blue">豹子形态</i>奖金<i class="orange">1000</i>元；<i class="blue">组三形态</i>奖金<i class="orange">320</i>元；<i class="blue">组六形态</i>奖金<i class="orange">160</i>元。', Demo: [{
        th: "投注", td: "222"
    }
    , {
        th: "开奖", td: '**<i class="orange">222</i>'
    }
    , {
        th: "奖金", td: "1000元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号, SelectPlayType: [{
            Name: "包一胆", PlayType: [TK.Bet.PlayType.sxzxbd三组包胆]
        }
        , {
            Name: "包二胆", PlayType: [TK.Bet.PlayType.sxzxbd三组包胆]
        }
        ]
    }
    ], PlayType: [TK.Bet.PlayType.sxzxbd三组包胆]
}
, {
    Menu: "一星", Name: "一星", Desc: '至少选择一个号码，竞猜开奖号码<i class="orange">最后一位</i>，奖金<i class="orange">10</i>元。', Demo: [{
        th: "投注", td: "8"
    }
    , {
        th: "开奖", td: '****<i class="orange">8</i>'
    }
    , {
        th: "奖金", td: "10元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.yxds一星单式, TK.Bet.PlayType.yxfs一星复式]
}
, {
    Menu: "二星", Name: "二星", Desc: '<i class="orange">每位</i>至少选择一个号码，竞猜开奖号码的<i class="orange">最后两位</i>，号码和位置都对应即中奖，奖金<i class="orange">100</i>元。', Demo: [{
        th: "投注", td: "78"
    }
    , {
        th: "开奖", td: '***<i class="orange">78</i>'
    }
    , {
        th: "奖金", td: "100元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    , {
        Value: TK.Bet.BetMethod.文件选号
    }
    ], PlayType: [TK.Bet.PlayType.exds二星单式, TK.Bet.PlayType.exfs二星复式]
}
, {
    Menu: "三星", Name: "三星", Desc: '<i class="orange">每位</i>至少选择一个号码，竞猜开奖号码的<i class="orange">最后三位</i>，号码和位置都对应即中奖，奖金<i class="orange">1000</i>元。', Demo: [{
        th: "投注", td: "678"
    }
    , {
        th: "开奖", td: '**<i class="orange">678</i>'
    }
    , {
        th: "奖金", td: "1000元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    , {
        Value: TK.Bet.BetMethod.文件选号
    }
    ], PlayType: [TK.Bet.PlayType.sxds三星单式, TK.Bet.PlayType.sxfs三星复式]
}
, {
    Menu: "五星", Name: "五星", Desc: '<i class="orange">每位</i>至少选择一个号码，竞猜开奖号码的<i class="orange">全部五位</i>，号码和位置都对应即中奖，奖金<i class="orange">100000</i>元。', Demo: [{
        th: "投注", td: "45678"
    }
    , {
        th: "开奖", td: '<i class="orange">45678</i>'
    }
    , {
        th: "奖金", td: "100000元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    , {
        Value: TK.Bet.BetMethod.文件选号
    }
    ], PlayType: [TK.Bet.PlayType.wxds五星单式, TK.Bet.PlayType.wxfs五星复式]
}
, {
    Menu: "大小单双", Name: "大小单双", Desc: '<i class="orange">同时</i>竞猜十位和个位的大小单双，<i class="orange">全部</i>猜中即中奖<i class="orange">4</i>元。', Demo: [{
        th: "投注", td: "大大"
    }
    , {
        th: "开奖", td: '***<i class="orange">78</i>'
    }
    , {
        th: "奖金", td: "4元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.dxds大小单双]
}
, {
    Menu: "二星组选", Name: "二星组选", Desc: '至少选择<i class="orange">两个</i>号码，竞猜开奖号码的<i class="orange">最后两位</i>，奖金<i class="orange">50</i>元（<i class="orange">开出对子不算中奖</i>）。', Demo: [{
        th: "投注", td: "78"
    }
    , {
        th: "开奖", td: '***<i class="orange">78</i>'
    }
    , {
        th: "奖金", td: "50元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    , {
        Value: TK.Bet.BetMethod.文件选号
    }
    ], PlayType: [TK.Bet.PlayType.exzxds二星组选单式, TK.Bet.PlayType.exzxfs二星组选复式]
}
, {
    Menu: "二组分位", Name: "二组分位", Desc: '<i class="orange">每位</i>至少选择一个号码，竞猜开奖号码的<i class="orange">最后两位</i>，非对子奖金<i class="orange">50</i>元，对子奖金<i class="orange">100</i>元。', Demo: [{
        th: "投注", td: "77"
    }
    , {
        th: "开奖", td: '***<i class="orange">77</i>'
    }
    , {
        th: "奖金", td: "100元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.exzxfwds二星组选分位单式, TK.Bet.PlayType.exzxfwfs二星组选分位复式]
}
, {
    Menu: "二组包点", Name: "二组包点", Desc: '至少选择一个和值，竞猜开奖号码<i class="orange">后两位</i>数字之和，非对子奖金<i class="orange">50</i>元，对子奖金<i class="orange">100</i>元。', Demo: [{
        th: "投注", td: "2"
    }
    , {
        th: "开奖", td: '***<i class="orange">11</i>'
    }
    , {
        th: "奖金", td: "100元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.exzxhz二星组选和值]
}
, {
    Menu: "二组包胆", Name: "二组包胆", Desc: '竞猜开奖号码<i class="orange">后两位</i>的胆码，<i class="orange">任意一位</i>开出即中奖，非对子奖金<i class="orange">50</i>元，对子奖金<i class="orange">100</i>元。', Demo: [{
        th: "投注", td: "1"
    }
    , {
        th: "开奖", td: '***<i class="orange">11</i>'
    }
    , {
        th: "奖金", td: "100元"
    }
    ], BetMethod: [{
        Value: TK.Bet.BetMethod.常规选号
    }
    ], PlayType: [TK.Bet.PlayType.exzxbd二星组选包胆]
}
];
TK.Bet.Service.MenuBet = function () { };
TK.Bet.Service.MenuBet.prototype = {
    ObjMissAbout: null, DomListMenu: new Array(), DomPlayType: null, DomBetMethod: null, DomDesc: null, DomExample: null, DomNumberBox: null, CurrentMenu: null, CurrentConfig: null, CurrentBetMethod: null, CurrentNumberBox: null, getBonusMoney: function (a) {
        if ($S.Debug.IntelliSense) {
            a = new TK.Bet.Util.BetItem()
        }
        var b = [];
        var f = {}, c = a.PlayTypeName, e = 0, d = 0, g = 0, h = 0;
        switch (a.PlayType) {
            case TK.Bet.PlayType.dxds大小单双: c = "大小单双";
                b = [{
                    Text: "中十位和个位", BonusMoney: 4
                }
                ];
                e = 4;
                d = e;
                break;
            case TK.Bet.PlayType.yxds一星单式: case TK.Bet.PlayType.yxfs一星复式: c = "一星";
                b = [{
                    Text: "中个位", BonusMoney: 10
                }
                ];
                e = 10;
                d = e;
                break;
            case TK.Bet.PlayType.exzxds二星组选单式: case TK.Bet.PlayType.exzxfs二星组选复式: c = "二星组选";
                b = [{
                    Text: "中非对子", BonusMoney: 50
                }
                ];
                e = 50;
                d = e;
                break;
            case TK.Bet.PlayType.exzxfwds二星组选分位单式: case TK.Bet.PlayType.exzxfwfs二星组选分位复式: c = "二星组选分位";
                b = [{
                    Text: "中对子", BonusMoney: 100
                }
                , {
                    Text: "非对子", BonusMoney: 50
                }
                ];
                e = 50;
                d = 100;
                break;
            case TK.Bet.PlayType.exds二星单式: case TK.Bet.PlayType.exfs二星复式: c = "二星直选";
                b = [{
                    Text: "中十位和个位", BonusMoney: 100
                }
                ];
                e = 100;
                d = e;
                break;
            case TK.Bet.PlayType.exhz二星和值: c = "二星和值";
                b = [{
                    Text: "中十位和个位之和", BonusMoney: 100
                }
                ];
                e = 100;
                d = e;
                break;
            case TK.Bet.PlayType.exzxhz二星组选和值: c = "二星组选和值";
                b = [{
                    Text: "中对子", BonusMoney: 100
                }
                , {
                    Text: "非对子", BonusMoney: 50
                }
                ];
                e = 50;
                d = 100;
                break;
            case TK.Bet.PlayType.exzxbd二星组选包胆: c = "二星组选包胆";
                b = [{
                    Text: "中对子", BonusMoney: 100
                }
                , {
                    Text: "非对子", BonusMoney: 50
                }
                ];
                e = 50;
                d = 100;
                break;
            case TK.Bet.PlayType.sxds三星单式: case TK.Bet.PlayType.sxfs三星复式: c = "三星直选";
                b = [{
                    Text: "中后三位", BonusMoney: 1000
                }
                ];
                e = 1000;
                d = e;
                break;
            case TK.Bet.PlayType.sxhz三星和值: c = "三星和值";
                b = [{
                    Text: "中后三位之和", BonusMoney: 1000
                }
                ];
                e = 1000;
                d = e;
                break;
            case TK.Bet.PlayType.sxzxhz三星组选和值: case TK.Bet.PlayType.sxzxbd三组包胆: b = [{
                Text: "豹子形态", BonusMoney: 1000
            }
            , {
                Text: "组三形态", BonusMoney: 320
            }
            , {
                Text: "组六形态", BonusMoney: 160
            }
            ];
                e = 160;
                d = 1000;
                break;
            case TK.Bet.PlayType.sxzxsds三星组选三单式: case TK.Bet.PlayType.sxzxsfs三星组选三复式: case TK.Bet.PlayType.sxzxsdt三星组选三胆拖: case TK.Bet.PlayType.sxzxshz三星组选三和值: b = [{
                Text: "组三形态", BonusMoney: 320
            }
            ];
                e = 320;
                d = e;
                break;
            case TK.Bet.PlayType.sxzxlds三星组选六单式: case TK.Bet.PlayType.sxzxlfs三星组选六复式: case TK.Bet.PlayType.sxzxldt三星组选六胆拖: case TK.Bet.PlayType.sxzxlhz三星组选六和值: b = [{
                Text: "组六形态", BonusMoney: 160
            }
            ];
                e = 160;
                d = e;
                break;
            case TK.Bet.PlayType.sxzxzh三星直选组合: c = "三星直选组合";
                b = [{
                    Text: "中后三位", BonusMoney: 1000
                }
                ];
                e = 1000;
                d = e;
                break;
            case TK.Bet.PlayType.wxds五星单式: case TK.Bet.PlayType.wxfs五星复式: c = "五星直选(税后)";
                b = [{
                    Text: "全中", BonusMoney: 80000
                }
                ];
                e = 80000;
                d = e;
                break;
            case TK.Bet.PlayType.wxtxds五星通选单式: case TK.Bet.PlayType.wxtxfs五星通选复式: c = "五星通选";
                b = [{
                    Text: "中五位(税后)", BonusMoney: 16440
                }
                , {
                    Text: "中前三或后三", BonusMoney: 220
                }
                , {
                    Text: "中前二和后二", BonusMoney: 40
                }
                , {
                    Text: "中前二或者后二", BonusMoney: 20
                }
                ];
                e = 20;
                d = 16440;
                break
        }
        b.sort(function (i, j) {
            return j.BonusMoney - i.BonusMoney
        });
        f[c] = b;
        f.minBonusMoney = e;
        f.maxBonusMoney = d;
        return f
    }
    , getDefaultPlayType: function () {
        var a = $S.Cookie.get(String.format("bet_kuaikai_{0}_dPlayType", TK.CurrentLotteryType));
        if (a == null) {
            a = 3
        }
        if (parseInt(a, 10) >= this.DomListMenu.length) {
            a = 0
        }
        return a
    }
    , setDefaultPlayType: function (a) {
        $S.Cookie.set(String.format("bet_kuaikai_{0}_dPlayType", TK.CurrentLotteryType), a)
    }
    , showNumberBox: function (a, b) {
        $.extend({
            Value: -1, Box: null
        }
        , a);
        var f = b.target;
        if (this.ObjMissAbout == null) {
            this.ObjMissAbout = new TK.Miss.CreateBox();
            $(TK.Video).bind(TK.Video.Handler_GetBonus, {
                missObj: this.ObjMissAbout
            }
            , function (h, j, g) {
                h.data.missObj.Box_AddIssue(j, g)
            })
        }
        $(this.CurrentBetMethod).removeClass("active");
        this.CurrentBetMethod = f;
        $(this.CurrentBetMethod).addClass("active");
        $(this.CurrentNumberBox).hide();
        if (a.Box == null) {
            var c = new TK.Bet.Service.MenuBet.Box(this.CurrentConfig, a);
            a.Box = c.DomWrap.appendTo(this.DomNumberBox);
            var d = this;
            $(c).bind("box_addItem", function (h, g) {
                $(d).triggerHandler("menuBet_addItem", g)
            });
            this.randomCode(d.CurrentConfig.PlayType[0], a)
        }
        try {
            $(this.ObjMissAbout).triggerHandler("box_showMiss", [this.CurrentConfig.PlayType[0], a, aryIssue])
        }
        catch (b) {
            $S.Debug.log("handler[box_showMiss] error", b.message)
        }
        this.CurrentNumberBox = a.Box;
        $(this.CurrentNumberBox).show()
    }
    , randomCode: function (b, a) {
        var c = 1, e = true, d = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        switch (b) {
            case TK.Bet.PlayType.dxds大小单双: c = 2;
                d = [0, 1, 2, 3];
                break;
            case TK.Bet.PlayType.yxds一星单式: break;
            case TK.Bet.PlayType.wxtxds五星通选单式: case TK.Bet.PlayType.wxds五星单式: c = 5;
                break;
            case TK.Bet.PlayType.sxzxsds三星组选三单式: case TK.Bet.PlayType.exzxds二星组选单式: c = 2;
                e = false;
                break;
            case TK.Bet.PlayType.exds二星单式: case TK.Bet.PlayType.exzxfwds二星组选分位单式: c = 2;
                break;
            case TK.Bet.PlayType.sxzxlds三星组选六单式: case TK.Bet.PlayType.sxzxzh三星直选组合: c = 3;
                e = false;
                break;
            case TK.Bet.PlayType.sxds三星单式: c = 3;
                break;
            default: return;
                break
        }
        $("li.fr a.random", a.Box).click(function () {
            $("span[n].active,span[n].num_on", a.Box).removeClass("active num_on");
            var f = $S.Math.RandomAry(d, c, e);
            var j = $("div.numArea ul.btn_numlist", a.Box);
            if (f.length == j.length) {
                for (var g = 0;
                g < j.length;
                g++) {
                    var h = $("span[n]", j.eq(g));
                    g == j.length - 1 ? h.eq(f[g]).click() : h.eq(f[g]).addClass("active")
                }

            }
            else {
                if (j.length == 1) {
                    var h = $("span[n]", j.eq(0));
                    for (var g = 1;
                    g <= f.length;
                    g++) {
                        g == f.length ? h.eq(f[g - 1]).click() : h.eq(f[g - 1]).addClass("active")
                    }

                }
                else {
                    if ($("input:radio:eq(0)", a.Box).prop("checked")) {
                        j.each(function (k) {
                            if (k >= 3) {
                                return true
                            }
                            var l = $("span[n]", this);
                            k == 2 ? l.eq(f[1]).click() : l.eq(f[0]).addClass("active")
                        })
                    }
                    else {
                        var h = $("span[n]", j.eq(3));
                        for (var g = 1;
                        g <= f.length;
                        g++) {
                            g == f.length ? h.eq(f[g - 1]).click() : h.eq(f[g - 1]).addClass("active")
                        }

                    }

                }

            }

        })
    }
    , initDomText: function () {
        var a = this.CurrentConfig;
        $(this.DomPlayType).html(a.Name);
        $(this.DomDesc).html(a.Desc);
        var b = [];
        for (var c = 0;
        c < a.Demo.length;
        c++) {
            b.push(String.format("<tr><th>{0}</th><td>{1}</td></tr>", a.Demo[c].th, a.Demo[c].td))
        }
        $(this.DomExample).html(b.join(""));
        b.clear();
        for (var c = 0;
        c < a.BetMethod.length;
        c++) {
            b.push(String.format('<a href="javascript:void(0)" v="{1}">{0}</a>', $S.Enum.parse(a.BetMethod[c].Value, TK.Bet.BetMethod), a.BetMethod[c].Value))
        }
        var d = this;
        $(b.join("")).appendTo($(this.DomBetMethod).html("")).each(function (e) {
            $(this).click(d.showNumberBox.bind(d, a.BetMethod[e]))
        }).eq(0).click()
    }
    , selectMenu: function (a) {
        if (typeof (a) != "object") {
            a = $(this.DomListMenu).eq(a)
        }
        var b = $(TK.Bet.Config).filter(function () {
            return this.Menu == $(a).find("span").text()
        }).get(0);
        if (typeof (b) == "undefined") {
            $.artDialog({
                content: '<b style="color:#fff;">此玩法选号功能生产中……</b>'
            });
            return
        }
        if (this.CurrentMenu == a) {
            return
        }
        $(this.CurrentMenu).removeClass("active");
        this.CurrentMenu = a;
        $(this.CurrentMenu).addClass("active");
        this.CurrentConfig = b;
        this.initDomText();
        this.setDefaultPlayType($(this.DomListMenu).index(a))
    }
    , initialize: function () {
        var a = this;
        this.DomListMenu = $("#Bet_Menu li a").each(function () {
            $(this).attr("href", "javascript:void(0)").click(a.selectMenu.bind(a, this))
        });
        this.DomPlayType = $("#Bet_Menu_PlayType");
        this.DomBetMethod = $("#Bet_Menu_Method");
        this.DomDesc = $("#Bet_Menu_Desc");
        this.DomExample = $("#Bet_Menu_Example");
        $(this.DomExample).parent().parent().hide().parent().hover(function () {
            $(this).children("div.pop").show()
        }
        , function () {
            $(this).children("div.pop").hide()
        });
        this.DomBetMethod = $("#Bet_Menu_Method");
        this.DomNumberBox = $("#Bet_Number_Box");
        this.selectMenu(this.getDefaultPlayType());
        return this
    }

};
TK.Bet.Service.MenuBet.Box = function (b, a) {
    this.initialize.bind(this);
    this.initialize.apply(this, arguments)
};
TK.Bet.Service.MenuBet.Box.prototype = {
    Config: {
        Menu: "", Name: "", Desc: "", Demo: [], BetMethod: [], PlayType: []
    }
    , BetMethod: {
        Value: -1, SelectPlayType: []
    }
    , SelectPlayTypeIndex: 0, DomWrap: null, DomPanelLeft: null, DomPanelRight: null, DomPickType: null, DomPickBox: null, DomBtnAddItem: null, DomDataBetCount: null, DomDataBetMoney: null, DomDataBonusInfo: null, ListPickBalls: [], ListData: [], addItem: function () {
        if (this.compute(this.SelectPlayTypeIndex, true) == 0) {
            var b = $('<div class="dia_c_main"><div class="cet"><b class="plaint">&nbsp;</b><span>选择的投注号码不正确!</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>').find("a[class*=Dbutt]").click(function () {
                $.artDialog.get("artDialog_PlayTypeSelected_Error").close()
            }).end();
            $.artDialog({
                id: "artDialog_PlayTypeSelected_Error", content: b.get(0)
            });
            return
        }
        for (var a = 0;
        a < this.ListData.length;
        a++) {
            if (this.ListData[a].BetCount > 0) {
                $(this).triggerHandler("box_addItem", this.ListData[a].init())
            }

        }
        for (var a = 0;
        a < this.BetMethod.SelectPlayType[this.SelectPlayTypeIndex].ListPickBalls.length;
        a++) {
            $(this.BetMethod.SelectPlayType[this.SelectPlayTypeIndex].ListPickBalls[a]).triggerHandler("clear")
        }
        this.compute(this.SelectPlayTypeIndex, true)
    }
    , addInputItem: function () {
        if (this.ListData.length == 0) {
            var b = $('<div class="dia_c_main"><div class="cet"><b class="plaint">&nbsp;</b><span>导入的号码格式不正确!</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>').find("a[class*=Dbutt]").click(function () {
                $.artDialog.get("artDialog_PlayTypeSelected_Error").close()
            }).end();
            $.artDialog({
                id: "artDialog_PlayTypeSelected_Error", content: b.get(0)
            });
            return
        }
        for (var a = 0;
        a < this.ListData.length;
        a++) {
            if (this.ListData[a].BetCount > 0) {
                $(this).triggerHandler("box_addItem", this.ListData[a].init())
            }

        }
        $(this.DomPanelLeft).find("textarea").val("").change()
    }
    , compute: function (e, d) {
        d = true;
        if (d == true) {
            var r = this;
            this.ListData.clear()
        }
        var h = 0;
        var q = this.BetMethod.SelectPlayType[e];
        this.SelectPlayTypeIndex = e;
        var m = q.ListPickBalls;
        switch (q.PlayType[0]) {
            case TK.Bet.PlayType.sxhz三星和值: case TK.Bet.PlayType.exhz二星和值: case TK.Bet.PlayType.sxzxhz三星组选和值: case TK.Bet.PlayType.exzxhz二星组选和值: $(m[0].ListBalls).filter(":has(span.num_on)").each(function () {
                var i = new TK.Bet.Util.BetItem();
                i.BetCount = parseInt($(this).children("span").attr("bc"), 10);
                h += i.BetCount;
                if (d == true) {
                    i.BetNumber = $(this).children("span").attr("n");
                    i.BetMoney = i.BetCount * TK.PerMoney;
                    i.BetMethod = r.BetMethod.Value;
                    i.PlayType = q.PlayType[0];
                    r.ListData.push(i)
                }

            });
                break;
            case TK.Bet.PlayType.sxzxsds三星组选三单式: h = 1;
                var c = [];
                for (var k = 0;
                k < m.length;
                k++) {
                    var b = $(m[k].ListBalls).filter(":has(span.active)");
                    c.push($(b).children("span").attr("n"));
                    h *= $S.Math.c(1, b.length)
                }
                h = c.getMostItems().times == 2 ? h : 0;
                if (d == true && h > 0) {
                    var l = new TK.Bet.Util.BetItem();
                    l.BetNumber = c.join(",");
                    l.BetCount = h;
                    l.BetMoney = l.BetCount * TK.PerMoney;
                    l.BetMethod = r.BetMethod.Value;
                    l.PlayType = q.PlayType[0];
                    r.ListData.push(l)
                }
                break;
            case TK.Bet.PlayType.sxzxsfs三星组选三复式: var c = $(m[0].ListBalls).filter(":has(span[class=active][n])");
                var a = c.length;
                h = $S.Math.c(1, a) * (a - 1);
                if (d == true && h > 0) {
                    var g = [];
                    c.each(function () {
                        g.push($(this).children("span").attr("n"))
                    });
                    var l = new TK.Bet.Util.BetItem();
                    l.BetNumber = g.join(",");
                    l.BetCount = h;
                    l.BetMoney = l.BetCount * TK.PerMoney;
                    l.BetMethod = r.BetMethod.Value;
                    l.PlayType = q.PlayType[0];
                    r.ListData.push(l)
                }
                break;
            case TK.Bet.PlayType.sxzxlds三星组选六单式: var c = $(m[0].ListBalls).filter(":has(span[class=active][n])");
                h = $S.Math.c(3, c.length);
                if (d == true && h > 0) {
                    var g = [];
                    c.each(function () {
                        g.push($(this).children("span").attr("n"))
                    });
                    var l = new TK.Bet.Util.BetItem();
                    l.BetNumber = g.join(",");
                    l.BetCount = h;
                    l.BetMoney = l.BetCount * TK.PerMoney;
                    l.BetMethod = r.BetMethod.Value;
                    l.PlayType = q.PlayType[h > 1 ? 1 : 0];
                    r.ListData.push(l)
                }
                break;
            case TK.Bet.PlayType.sxzxzh三星直选组合: var c = $(m[0].ListBalls).filter(":has(span[class=active][n])");
                h = $S.Math.c(3, c.length) * 6;
                if (d == true && h > 0) {
                    var g = [];
                    c.each(function () {
                        g.push($(this).children("span").attr("n"))
                    });
                    var l = new TK.Bet.Util.BetItem();
                    l.BetNumber = g.join(",");
                    l.BetCount = h;
                    l.BetMoney = l.BetCount * TK.PerMoney;
                    l.BetMethod = r.BetMethod.Value;
                    l.PlayType = q.PlayType[0];
                    r.ListData.push(l)
                }
                break;
            case TK.Bet.PlayType.sxzxbd三组包胆: if (m.length == 2 && $(m[0].ListBalls).filter(":has(span[class=active][n])").length == 1 && $(m[1].ListBalls).filter(":has(span.active)").length == 1) {
                h = 10
            }
            else {
                if (m.length == 1 && $(m[0].ListBalls).filter(":has(span[class=active][n])").length == 1) {
                    h = 55
                }

            }
                if (d == true && h > 0) {
                    var g = [];
                    $(m).each(function () {
                        g.push($(this.ListBalls).children("span[class=active][n]").attr("n"))
                    });
                    var l = new TK.Bet.Util.BetItem();
                    l.BetNumber = g.join(",");
                    l.BetCount = h;
                    l.BetMoney = l.BetCount * TK.PerMoney;
                    l.BetMethod = r.BetMethod.Value;
                    l.PlayType = q.PlayType[0];
                    r.ListData.push(l)
                }
                break;
            case TK.Bet.PlayType.dxds大小单双: case TK.Bet.PlayType.yxds一星单式: case TK.Bet.PlayType.exds二星单式: case TK.Bet.PlayType.sxds三星单式: case TK.Bet.PlayType.wxds五星单式: case TK.Bet.PlayType.wxtxds五星通选单式: case TK.Bet.PlayType.exzxfwds二星组选分位单式: h = 1;
                for (var k = 0;
                k < m.length;
                k++) {
                    h *= $S.Math.c(1, $(m[k].ListBalls).filter(":has(span[class=active][n])").length)
                }
                if (d == true && h > 0) {
                    var f = [];
                    for (var k = 0;
                    k < m.length;
                    k++) {
                        var g = [];
                        var c = $(m[k].ListBalls).filter(":has(span[class=active][n])");
                        c.each(function () {
                            g.push($(this).children("span").attr("n"))
                        });
                        f.push(g.join(","))
                    }
                    var l = new TK.Bet.Util.BetItem();
                    l.BetNumber = f.join("|");
                    l.BetCount = h;
                    l.BetMoney = l.BetCount * TK.PerMoney;
                    l.BetMethod = r.BetMethod.Value;
                    l.PlayType = q.PlayType[h > 1 ? 1 : 0];
                    r.ListData.push(l)
                }
                break;
            case TK.Bet.PlayType.exzxds二星组选单式: var c = $(m[0].ListBalls).filter(":has(span.active)");
                h = $S.Math.c(2, c.length);
                if (d == true && h > 0) {
                    var g = [];
                    c.each(function () {
                        g.push($(this).children("span").attr("n"))
                    });
                    var l = new TK.Bet.Util.BetItem();
                    l.BetNumber = g.join(",");
                    l.BetCount = h;
                    l.BetMoney = l.BetCount * TK.PerMoney;
                    l.BetMethod = r.BetMethod.Value;
                    l.PlayType = q.PlayType[h > 1 ? 1 : 0];
                    r.ListData.push(l)
                }
                break;
            case TK.Bet.PlayType.exzxbd二星组选包胆: var c = $(m[0].ListBalls).filter(":has(span.active)");
                h = c.length * 10;
                if (d == true && h > 0) {
                    c.each(function () {
                        var i = new TK.Bet.Util.BetItem();
                        i.BetNumber = $(this).children("span").attr("n");
                        i.BetCount = 10;
                        i.BetMoney = i.BetCount * TK.PerMoney;
                        i.BetMethod = r.BetMethod.Value;
                        i.PlayType = q.PlayType[0];
                        r.ListData.push(i)
                    })
                }
                break
        }
        $(this.DomDataBetCount).html(h.toMoney());
        $(this.DomDataBetMoney).html((h * TK.PerMoney).toMoney());
        if (h > 0) {
            var j = new TK.Bet.Util.BetItem();
            $.extend(j, this.ListData[0].init());
            var p = new TK.Bet.Service.MenuBet().getBonusMoney(j);
            var o = parseFloat(p.minBonusMoney), n = parseFloat(p.maxBonusMoney);
            if (o == n) {
                $(this.DomDataBonusInfo).html(String.format("如果中奖，奖金<em>{0}</em>元，盈利<em>{1}</em>元。", o, (o - h * TK.PerMoney))).show()
            }
            else {
                $(this.DomDataBonusInfo).html(String.format("如果中奖，奖金<em>{0}</em>元至<em>{1}</em>元，盈利<em>{2}</em>元至<em>{3}</em>元。", o, n, (o - h * TK.PerMoney), (n - h * TK.PerMoney))).show()
            }

        }
        else {
            $(this.DomDataBonusInfo).hide()
        }
        return h
    }
    , createPickBallTable: function () {
        for (var c = 0;
        c < this.BetMethod.SelectPlayType.length;
        c++) {
            var g = false;
            var k = this.BetMethod.SelectPlayType[c];
            k.Table = $("<div></div>").appendTo(this.DomPickBox).hide();
            var a = [], b = false;
            var h = this.BetMethod.SelectPlayType[c].PlayType[0];
            switch (h) {
                case TK.Bet.PlayType.sxzxsds三星组选三单式: g = true;
                    a.push({
                        title: "号码", quickList: [], showBottom: false
                    });
                    a.push({
                        title: "号码", quickList: [], showBottom: false
                    });
                    a.push({
                        title: "号码", quickList: []
                    });
                    b = true;
                    break;
                case TK.Bet.PlayType.sxzxsfs三星组选三复式: a.push({
                    title: "号码"
                });
                    b = true;
                    break;
                case TK.Bet.PlayType.sxzxlds三星组选六单式: a.push({
                    title: "号码"
                });
                    break;
                case TK.Bet.PlayType.sxzxzh三星直选组合: a.push({
                    title: "号码"
                });
                    break;
                case TK.Bet.PlayType.sxzxbd三组包胆: g = true;
                    if (c == 1) {
                        a.push({
                            title: "号码", quickList: [], showBottom: false
                        })
                    }
                    a.push({
                        title: "号码", quickList: []
                    });
                    break;
                case TK.Bet.PlayType.wxtxds五星通选单式: a.push({
                    title: "万位"
                });
                    a.push({
                        title: "千位"
                    });
                    a.push({
                        title: "百位"
                    });
                    a.push({
                        title: "十位"
                    });
                    a.push({
                        title: "个位"
                    });
                    break;
                case TK.Bet.PlayType.yxds一星单式: a.push({
                    title: "个位"
                });
                    break;
                case TK.Bet.PlayType.exds二星单式: a.push({
                    title: "十位"
                });
                    a.push({
                        title: "个位"
                    });
                    break;
                case TK.Bet.PlayType.sxds三星单式: a.push({
                    title: "百位"
                });
                    a.push({
                        title: "十位"
                    });
                    a.push({
                        title: "个位"
                    });
                    break;
                case TK.Bet.PlayType.wxds五星单式: a.push({
                    title: "万位"
                });
                    a.push({
                        title: "千位"
                    });
                    a.push({
                        title: "百位"
                    });
                    a.push({
                        title: "十位"
                    });
                    a.push({
                        title: "个位"
                    });
                    break;
                case TK.Bet.PlayType.dxds大小单双: $(this.DomPanelRight).addClass("numCheckDS");
                    var e = ["大", "小", "单", "双"];
                    g = true;
                    a.push({
                        title: "十位", listBalls: e, quickList: []
                    });
                    a.push({
                        title: "个位", listBalls: e, quickList: []
                    });
                    break;
                case TK.Bet.PlayType.exzxds二星组选单式: a.push({
                    title: "二组"
                });
                    break;
                case TK.Bet.PlayType.exzxfwds二星组选分位单式: a.push({
                    title: "十位"
                });
                    a.push({
                        title: "个位"
                    });
                    break;
                case TK.Bet.PlayType.exzxbd二星组选包胆: a.push({
                    title: "包胆"
                });
                    break
            }
            for (var d = 0;
            d < a.length;
            d++) {
                var f = k.ListPickBalls;
                f.push(new TK.Bet.Service.MenuBet.PickBall(g));
                a[d].index = d + 1;
                k.Table.append(f[f.length - 1].createRow(a[d], b));
                $(f[f.length - 1]).bind("compute", this.compute.bind(this, c))
            }
            if (c == this.BetMethod.SelectPlayType.length - 1) {
                k.Table.show()
            }

        }

    }
    , inputTextChanged: function (d) {
        var k = d.target || d.srcElement || d;
        var h = k.value.replace(/\s+/g, "\n");
        var a = h.split("\n");
        this.ListData.clear();
        var j, l = false, b = 0, c = 0;
        for (var f = 0;
        f < a.length;
        f++) {
            switch (this.Config.PlayType[0]) {
                case TK.Bet.PlayType.exds二星单式: a[f] = a[f].replace(/\|/igm, "").split("").join("|");
                    j = new RegExp(/^([0-9][\|]){1}[0-9]$/igm);
                    l = j.test(a[f]);
                    if (l) {
                        c = 1
                    }
                    break;
                case TK.Bet.PlayType.sxds三星单式: a[f] = a[f].replace(/\|/igm, "").split("").join("|");
                    j = new RegExp(/^([0-9][\|]){2}[0-9]$/igm);
                    l = j.test(a[f]);
                    if (l) {
                        c = 1
                    }
                    break;
                case TK.Bet.PlayType.wxds五星单式: case TK.Bet.PlayType.wxtxds五星通选单式: a[f] = a[f].replace(/\|/igm, "").split("").join("|");
                    j = new RegExp(/^([0-9][\|]){4}[0-9]$/igm);
                    l = j.test(a[f]);
                    if (l) {
                        c = 1
                    }
                    break;
                case TK.Bet.PlayType.exzxds二星组选单式: a[f] = a[f].replace(/\,/ig, "").split("").join();
                    j = new RegExp(/^(?!.*?([0-9]).*?\1)(([0-9],){1}[0-9])$/igm);
                    l = j.test(a[f]);
                    if (l) {
                        c = $S.Math.c(2, a[f].split(",").length)
                    }
                    break;
                case TK.Bet.PlayType.sxzxsds三星组选三单式: a[f] = a[f].replace(/\,/ig, "").split("").join();
                    j = new RegExp(/^(([0-9],){2}[0-9])$/igm);
                    l = j.test(a[f]);
                    if (l) {
                        l = a[f].split(",").clearRepeat().length == 2;
                        c = 1
                    }
                    break;
                case TK.Bet.PlayType.sxzxlds三星组选六单式: a[f] = a[f].replace(/\,/ig, "").split("").join();
                    j = new RegExp(/^(([0-9],){2}[0-9])$/igm);
                    l = j.test(a[f]);
                    if (l) {
                        l = a[f].split(",").clearRepeat().length == 3
                    }
                    c = 1;
                    break
            }
            if (l) {
                b += c;
                var g = new TK.Bet.Util.BetItem();
                g.BetCount = c;
                g.BetNumber = a[f];
                g.BetMoney = g.BetCount * TK.PerMoney;
                g.BetMethod = this.BetMethod.Value;
                g.PlayType = this.BetMethod.SelectPlayType[0].PlayType[c > 1 ? 1 : 0];
                this.ListData.push(g)
            }

        }
        $(this.DomDataBetCount).html(b.toMoney());
        $(this.DomDataBetMoney).html((b * TK.PerMoney).toMoney());
        return b
    }
    , initDom: function () {
        if (this.BetMethod.Value == TK.Bet.BetMethod.常规选号) {
            var b = $('<div class="chartCheck"><span class="sum">共<em>0</em>注，共<em class="f_highlight">0</em>元</span><span class="suml"><a class="btn btnChartCheck">添加号码</a><a class="btnSecretary">选号助手</a></span></div>');
            switch (this.Config.PlayType[0]) {
                case TK.Bet.PlayType.exhz二星和值: case TK.Bet.PlayType.sxhz三星和值: case TK.Bet.PlayType.sxzxhz三星组选和值: case TK.Bet.PlayType.exzxhz二星组选和值: this.DomPanelRight = $('<div class="drawTrend"></div>').appendTo(this.DomWrap);
                    $(this.DomWrap).append(b).addClass("panelLeft panelTrend");
                    var a = {};
                    if (this.Config.PlayType[0] == TK.Bet.PlayType.sxhz三星和值) {
                        a = {
                            title: this.Config.Name, listBalls: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], listBottom: [1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 75, 73, 69, 63, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1]
                        }

                    }
                    else {
                        if (this.Config.PlayType[0] == TK.Bet.PlayType.sxzxhz三星组选和值) {
                            a = {
                                title: this.Config.Name, listBalls: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], listBottom: [1, 1, 2, 3, 4, 5, 7, 8, 10, 12, 13, 14, 15, 15, 15, 15, 14, 13, 12, 10, 8, 7, 5, 4, 3, 2, 1, 1]
                            }

                        }
                        else {
                            if (this.Config.PlayType[0] == TK.Bet.PlayType.exhz二星和值) {
                                a = {
                                    title: this.Config.Name, listBalls: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18], listBottom: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1]
                                }

                            }
                            else {
                                if (this.Config.PlayType[0] == TK.Bet.PlayType.exzxhz二星组选和值) {
                                    a = {
                                        title: this.Config.Name, listBalls: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18], listBottom: [1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 5, 4, 4, 3, 3, 2, 2, 1, 1]
                                    }

                                }

                            }

                        }

                    }
                    var k = this.BetMethod.SelectPlayType[0].ListPickBalls;
                    k.push(new TK.Bet.Service.MenuBet.PickTable());
                    $(k[k.length - 1]).bind("compute", this.compute.bind(this, k.length - 1));
                    this.BetMethod.SelectPlayType[0].Table = k[k.length - 1].createTable(a);
                    $(this.DomPanelRight).append(this.BetMethod.SelectPlayType[0].Table);
                    break;
                default: this.DomPanelLeft = $('<div class="panelLeft"></div>').appendTo(this.DomWrap);
                    this.DomPanelRight = $('<div class="numCheck"></div>').appendTo(this.DomWrap);
                    var g = [];
                    if (this.BetMethod.SelectPlayType.length > 1) {
                        for (var h = 0;
                        h < this.BetMethod.SelectPlayType.length;
                        h++) {
                            g.push(String.format('<li><label><input name="{1}" type="radio" />{0}</label></li>', this.BetMethod.SelectPlayType[h].Name, this.Config.PlayType.join("_")))
                        }

                    }
                    g.push('<li><label><input type="checkbox" />冷热</label></li>');
                    var m = "";
                    switch (this.Config.PlayType[0]) {
                        case TK.Bet.PlayType.sxzxsds三星组选三单式: case TK.Bet.PlayType.sxzxzh三星直选组合: m = "机选";
                            break;
                        case TK.Bet.PlayType.sxzxbd三组包胆: case TK.Bet.PlayType.exzxbd二星组选包胆: break;
                        default: m = "机选一注";
                            break
                    }
                    if (m.length > 0) {
                        g.push('<li class="fr"><a class="random" href="javascript:void(0)">' + m + "</a></li>")
                    }
                    var l = $(String.format('<ul class="numAreaTab">{0}</ul>', g.join(""))).appendTo(this.DomPanelRight);
                    l.find("input:checkbox").prop("checked", true);
                    if (this.BetMethod.SelectPlayType.length > 1) {
                        var n = this;
                        this.DomPickType = l.find("label").each(function (e) {
                            $(this).click(function () {
                                n.compute(e);
                                $(n.BetMethod.SelectPlayType).each(function (i) {
                                    if (e == i) {
                                        $(this).prop("Table").show()
                                    }
                                    else {
                                        $(this).prop("Table").hide()
                                    }

                                })
                            });
                            if (e == 1) {
                                $(this).children("input:radio").prop("checked", true)
                            }

                        })
                    }
                    this.DomPickBox = $('<div class="numArea"></div>').appendTo(this.DomPanelRight);
                    b.appendTo(this.DomPanelRight);
                    this.createPickBallTable();
                    break
            }
            this.DomDataBetCount = b.find("em").eq(0);
            this.DomDataBetMoney = b.find("em").eq(1);
            this.DomBtnAddItem = b.find("a[class*=btnChartCheck]").click(this.addItem.bind(this));
            b.find("a[class*=btnSecretary]").click(kkSecretary.show.bind(kkSecretary))
        }
        else {
            if (this.BetMethod.Value == TK.Bet.BetMethod.文件选号) {
                var d = [], f = "|";
                switch (this.Config.PlayType[0]) {
                    case TK.Bet.PlayType.exds二星单式: d = ["0|1", "2|3"];
                        break;
                    case TK.Bet.PlayType.sxds三星单式: d = ["0|1|2", "2|3|4"];
                        break;
                    case TK.Bet.PlayType.wxds五星单式: case TK.Bet.PlayType.wxtxds五星通选单式: d = ["0|1|2|3|4", "2|3|4|5|6"];
                        break;
                    case TK.Bet.PlayType.exzxds二星组选单式: d = ["0,2", "2,3"];
                        f = ",";
                        break;
                    case TK.Bet.PlayType.sxzxsds三星组选三单式: d = ["0,0,1", "2,3,3"];
                        f = ",";
                        break;
                    case TK.Bet.PlayType.sxzxlds三星组选六单式: d = ["0,1,2", "2,3,4"];
                        f = ",";
                        break
                }
                this.DomPanelLeft = $('<div class="panelLeft pick_input"><div><label>大底号码</label><textarea class="pick_input_n"></textarea></div><div><label>文件路径：</label><iframe name="iFile" frameborder="0" scrolling="no" allowtransparency="true" style="height:25px;" src="/handler/kuaikai/file.aspx"></iframe><p class="ins">支持记事本.txt文档</p><p class="ins">导入文本内容将覆盖大底号码框中现有的内容!</p></div></div>').appendTo(this.DomWrap);
                this.DomPanelRight = $(String.format('<div class="numCheck pick_input_ins">← 请把您已有的大底号码复制或导入到左边文本框中。<p class="indt" style="margin-top:5px">每注号码之间可用 空格　换行符　隔开。</p><p class="indt">每注号码每位之间可用 "' + f + '" 分开。</p><p class="indt">单式号码之间可不使用符号分隔。</p><p class="indt">仅支持单式。</p><p class="indt indtt">例如：</p><p class="indt">{0}</p><div class="chartCheck"><span class="sum">共<em>0</em>注，共<em class="f_highlight">0</em>元</span><span class="suml"><a class="btn btnChartCheck">添加号码</a></span></div></div>', d.join('</p><p class="indt">'))).appendTo(this.DomWrap);
                $(this.DomPanelLeft).find("textarea").change(this.inputTextChanged.bind(this));
                var n = this;
                var j = $(this.DomPanelLeft).find("iframe").bind("load", function () {
                    var e = $(n.DomPanelLeft).find("textarea");
                    e.val(this.contentWindow.areaValue);
                    n.inputTextChanged(e.get(0))
                });
                try {
                    setTimeout(function () {
                        j.get(0).contentWindow.location.href = $(j).attr("src")
                    }
                    , 500)
                }
                catch (c) { } this.DomDataBetCount = $(this.DomPanelRight).find("em").eq(0);
                this.DomDataBetMoney = $(this.DomPanelRight).find("em").eq(1);
                this.DomBtnAddItem = $(this.DomPanelRight).find("a").click(this.addInputItem.bind(this))
            }

        }
        this.DomDataBonusInfo = $("<p></p>").appendTo($(this.DomDataBetCount).parent())
    }
    , initialize: function (b, a) {
        this.Config = {
            Menu: "", Name: "", Desc: "", Demo: [], BetMethod: [], PlayType: []
        };
        this.Config = $.extend(this.Config, b);
        this.BetMethod = {
            Value: -1, SelectPlayType: [{}]
        };
        this.BetMethod = $.extend(this.BetMethod, a);
        for (var c = 0;
        c < this.BetMethod.SelectPlayType.length;
        c++) {
            this.BetMethod.SelectPlayType[c] = $.extend({
                Name: "", PlayType: this.Config.PlayType, Table: null, ListPickBalls: []
            }
            , this.BetMethod.SelectPlayType[c])
        }
        this.DomWrap = $("<div></div>");
        this.initDom()
    }

};
TK.Bet.Service.MenuBet.PickTable = function () {
    $(this).bind("clear", this.clear.bind(this))
};
TK.Bet.Service.MenuBet.PickTable.prototype = {
    ListBalls: [], clear: function () {
        $(this.ListBalls).find("span[class*=num_on]").removeClass("num_on")
    }
    , event_clickBall: function (b, a) {
        var c = b.target;
        $(c).toggleClass("num_on");
        $(this).triggerHandler("compute")
    }
    , createTable: function (a) {
        a = $.extend({
            title: "", listBalls: [], listBottom: []
        }
        , a);
        var c = [];
        c.push('<table class="chart">');
        c.push("<tbody>");
        var e = ["<th>欲出几率：</th>"], f = [String.format("<th>{0}：</th>", a.title)], g = ["<th>包含注数：</th>"];
        for (var d = 0;
        d < a.listBalls.length;
        d++) {
            e.push('<td>{0}<i style="height:12px;"><em></em></i></td>');
            f.push(String.format('<td><span unselectable="on" class="num" bc="{1}" n="{0}">{0}</span></td>', a.listBalls[d], a.listBottom[d]));
            g.push(String.format("<td>{0}</td>", a.listBottom[d]))
        }
        c.push('<tr class="c1">');
        c.push(e.join(""));
        c.push("</tr>");
        c.push('<tr class="c2">');
        c.push(f.join(""));
        c.push("</tr>");
        c.push('<tr class="c3">');
        c.push(g.join(""));
        c.push("</tr>");
        c.push("</tbody>");
        c.push("</table>");
        var b = $(c.join(""));
        this.ListBalls = b.find("tr.c2 td");
        this.ListBalls.children("span").click(this.event_clickBall.bind(this));
        return b
    }

};
TK.Bet.Service.MenuBet.PickBall = function (a) {
    if (a == true) {
        this.OnlyOne = a
    }
    $(this).bind("clear", this.clear.bind(this))
};
TK.Bet.Service.MenuBet.PickBall.prototype = {
    OnlyOne: false, ListBalls: [], clear: function () {
        $(this.ListBalls).children("span").removeClass("active")
    }
    , event_clickBall: function (b, a) {
        if (this.OnlyOne == true) {
            $(this.ListBalls).find("span").removeClass("active")
        }
        var c = b.target;
        $(c).toggleClass("active");
        if (a != true) {
            $(this).triggerHandler("compute")
        }

    }
    , event_clickCommand: function (c) {
        var d = c.target;
        var b = $(this.ListBalls).children("span");
        var a = [];
        switch ($(d).text()) {
            case "全": a = b.removeClass("active");
                break;
            case "0": case "1": case "2": a = b.removeClass("active").filter(function () {
                return parseInt($(this).text(), 10) % 3 == parseInt($(d).text(), 10)
            });
                break;
            case "大": a = b.removeClass("active").filter(function () {
                return parseInt($(this).text(), 10) >= 5
            });
                break;
            case "小": a = b.removeClass("active").filter(function () {
                return parseInt($(this).text(), 10) < 5
            });
                break;
            case "奇": a = b.removeClass("active").filter(function () {
                return parseInt($(this).text(), 10) % 2 == 1
            });
                break;
            case "偶": a = b.removeClass("active").filter(function () {
                return parseInt($(this).text(), 10) % 2 == 0
            });
                break;
            case "质": a = b.removeClass("active").filter(function () {
                return parseInt($(this).text(), 10).checkPrime()
            });
                break;
            case "合": a = b.removeClass("active").filter(function () {
                return !parseInt($(this).text(), 10).checkPrime()
            });
                break;
            case "反": a = b;
                break;
            case "清": a = b.filter(".active");
                break
        }
        a.each(function (e) {
            $(this).trigger("click", e != a.length - 1)
        })
    }
    , createRow: function (b, a) {
        b = $.extend({
            title: "", listBalls: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], quickList: ["全", "0", "1", "2", "大", "小", "奇", "偶", "质", "合", "反", "清"], index: 0, showBottom: true
        }
        , b);
        var d = [];
        d.push('<ul class="btn_numlist">');
        d.push(String.format('<li class="nums"><em>{0}</em>{1}</li>', b.title, (b.showBottom ? String.format("<p>遗漏</p>{0}<p>冷热</p>", a ? "<p>对子</p>" : "") : "")));
        for (var e = 0;
        e < b.listBalls.length;
        e++) {
            d.push(String.format('<li><span unselectable="on" n="{0}">{0}</span>{1}</li>', b.listBalls[e], b.showBottom ? String.format("<p>-</p>{0}<p>-</p>", a ? "<p>-</p>" : "") : ""))
        }
        if (b.quickList.length > 0) {
            d.push(String.format('<li class="selectAll z{0}">', b.index));
            d.push('<ul class="s_all">');
            for (var e = 0;
            e < b.quickList.length;
            e++) {
                d.push(String.format("<li>{0}</li>", b.quickList[e]))
            }
            d.push("</ul>");
            d.push("</li>")
        }
        d.push("</ul>");
        var c = $(d.join(""));
        this.ListBalls = c.children("li:not([class])");
        this.ListBalls.children("span").click(this.event_clickBall.bind(this));
        c.find("li[class*=selectAll]").hover(function () {
            $(this).children("ul").show()
        }
        , function () {
            $(this).children("ul").hide()
        }).find("li").click(this.event_clickCommand.bind(this));
        return c
    }

};
function todayBonus(b) {
    this.param = $.extend({
        startIndex: 1, indexFormat: "00", todayCount: 84, rowCount: 14, todayIssue: [], tbAttr: ""
    }
    , b);
    this.PanelBonus = null;
    this.CheckXT = function (k, j) {
        var m = new Array();
        m.push('<td class="num">' + k + "</td>");
        if (!j) {
            return m.join("")
        }
        var l = new Array();
        for (var i = 2;
        i < 5;
        i++) {
            l.push(k.charAt(i))
        }
        m.push("<td");
        m.push(l[1] == l[2] ? ' class="y1">对子' : ">&nbsp;");
        m.push("</td>");
        switch (l.clearRepeat().length) {
            case 3: m.push('<td class="y3">组六</td>');
                break;
            case 2: m.push('<td class="y2">组三</td>');
                break;
            default: m.push('<td class="y1">豹子</td>');
                break
        }
        return m.join("")
    };
    var c = new Array(), d = "<td>--</td>", f = false, a = this;
    switch (TK.CurrentLotteryType) {
        case TK.LotteryType.cqssc重庆时时彩: case TK.LotteryType.jxssc江西时时彩: case TK.LotteryType.tjssc天津时时彩: d += "<td>--</td><td>--</td>";
            f = true;
            break
    }
    c.push('<div class="section sectionChart update"><h3 class="stn">今日开奖<span class="str"></span></h3>');
    c.push('<div class="stnmain"><div class="updateElement">');
    for (var e = this.param.startIndex;
    e <= this.param.todayCount;
    e++) {
        if ((e - 1) % this.param.rowCount == 0) {
            if (e != this.param.startIndex) {
                c.push('<tr class="last"><td colspan="4"><i class="shadowBottom"><i class="shadowBottomL"></i></i></td></tr></table>')
            }
            c.push("<table " + this.param.tbAttr + "><tr><th>期号</th><th>开奖号</th>");
            if (f) {
                c.push("<th>后2</th><th>后3</th>")
            }
            c.push("</tr>")
        }
        c.push("<tr>");
        var g = e.toFormatString(this.param.indexFormat);
        c.push('<td i="' + g + '">' + g + "</td>");
        var h = $(this.param.todayIssue).filter(function () {
            var i = parseInt(this.i.split("-")[1], 10).toFormatString(a.param.indexFormat);
            return i == g && parseInt(this.i.split("-")[0], 10) == todayDate
        });
        if (h.length == 0) {
            c.push(d)
        }
        else {
            c.push(this.CheckXT(h[0].b, f))
        }
        c.push("</tr>")
    }
    c.push('<tr class="last"><td colspan="' + (f ? 4 : 2) + '"><i class="shadowBottom"><i class="shadowBottomL"></i></i></td></tr></table>');
    c.push("</div></div></div>");
    this.PanelBonus = $(c.join("")).appendTo("div.main");
    this.ChangeBonus = function (i) {
        var j = parseInt(i.i.split("-")[1], 10).toFormatString(this.param.indexFormat);
        var k = "<td>" + j + "</td>";
        k += this.CheckXT(i.b, f);
        $("td[i=" + j + "]", this.PanelBonus).parent().html(k)
    }

}
TK.Miss.prototype.SetIcyHot = function () {
    aryIssue.sort(function (i, l) {
        return i.i.localeCompare(l.i)
    });
    var a = [[0, 0, 0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]], k = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0], g = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0], f = [[0, 0, 0, 0], [0, 0, 0, 0]];
    for (var h = aryIssue.length - 20 < 0 ? 0 : aryIssue.length - 20;
    h < aryIssue.length;
    h++) {
        var d = aryIssue[h].b;
        for (var c = 0;
        c < d.length;
        c++) {
            var e = parseInt(d.charAt(c), 10);
            a[c][e]++;
            if (c > 1) {
                k[e]++;
                if (c > 2) {
                    g[e]++;
                    var j = [e > 4 ? 0 : 1, e % 2 == 1 ? 2 : 3];
                    f[c - 3][j[0]]++;
                    f[c - 3][j[1]]++
                }

            }

        }

    }
    this.IcyHotDate = [a, k, g, f]
};
TK.Miss.MissType = {
    一星: 1, 十位: 2, 百位: 3, 千位: 4, 万位: 5, 一星二码: 560, 一星三码: 565, 一星大小奇偶遗漏: 310, 一星奇偶质合遗漏: 311, 一星大小质合遗漏: 312, 个位大小遗漏: 10, 个位单双遗漏: 11, 十位大小遗漏: 20, 十位单双遗漏: 21, 大小单双组合遗漏: 336, 二星号码分布: 61, 二星直选遗漏: 60, 二星直选复式: 65, 二星组选遗漏: 70, 二星组选三码: 71, 二星组选四码: 72, 二星组选五码: 73, 二星对子遗漏: 90, 二星胆码遗漏: 91, 二星连号遗漏: 94, 二星间隔遗漏: 95, 二星组选单胆: 96, 二星和值遗漏: 62, 二星和值段遗漏: 63, 二星和值尾遗漏: 64, 二星大小奇偶遗漏: 330, 二星奇偶质合遗漏: 331, 二星大小质合遗漏: 332, 二星012路遗漏: 86, 三星直选遗漏: 170, 三星号码分布: 171, 三星豹子遗漏: 200, 三星单胆遗漏: 201, 三星双胆遗漏: 202, 三星组选遗漏: 180, 三星组三遗漏: 181, 三星组六遗漏: 182, 三星组六四码: 183, 三星组六五码: 184, 三星组选单胆: 203, 三星组选双胆: 204, 三星大小遗漏: 190, 三星奇偶遗漏: 192, 三星质合遗漏: 194, 三星012路遗漏: 196, 四码遗漏: 210, 五码遗漏: 211, 六码遗漏: 212, 七码遗漏: 213, 四码组三遗漏: 440, 五码组三遗漏: 441, 六码组三遗漏: 442, 七码组三遗漏: 443, 四码组六遗漏: 455, 五码组六遗漏: 456, 六码组六遗漏: 457, 七码组六遗漏: 458, 三星和值遗漏: 172, 三星和值段遗漏: 173, 三星和值尾遗漏: 174, 三星组三对子: 550, 前二直选遗漏: 110, 前三直选遗漏: 230, 中三直选遗漏: 340
};
TK.Miss.LeftMiss = function () { };
TK.Miss.LeftMiss.prototype = {
    MissObject: null, BetMethod: -1, PlayType: -1, BoxContainerLeft: null, HeShuAry: ["0", "4", "6", "8", "9"], MissTab: {
        cyyl: "常用遗漏", bzyl: "标准遗漏", sqzs: "10期走势", fwyl: "分位遗漏", fwyly: "分位遗漏(1)", fwylt: "分位遗漏(2)", wnyl: "万能复式遗漏", xtyl: "形态遗漏", xtyl_dxds: "形态遗漏dxds", hz: "和值"
    }
    , SqzsType: {
        daxiao: "大小", quandaIs: "全大", quanxiaoIs: "全小", dxbi: "大小比", jiou: "奇偶", quandanIs: "全单", quanshuangIs: "全双", jioubi: "奇偶比", dxds: "大小单双", zhihe: "质合", zhihebi: "质合比", sanlu: "012路", sanlubi: "012路比", chs: "传号数", xingtai: "形态", zusan: "组三", zuliu: "组六", xiemaIs: "斜码", tiaomaIs: "跳码", jiangeIs: "间隔", duiziIs: "对子", lianhaoIs: "连号", chongma: "重码", chongmaIs: "重号"
    }
    , Trigger_Handler: "Click_Miss", CreateConfig: function () {
        var a = null;
        switch (this.PlayType) {
            case TK.Bet.PlayType.yxds一星单式: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.一星, t: "一星单式"
                    }
                    , {
                        m: TK.Miss.MissType.一星二码, t: "一星二码"
                    }
                    , {
                        m: TK.Miss.MissType.一星三码, t: "一星三码"
                    }
                    , {
                        m: TK.Miss.MissType.一星大小奇偶遗漏, t: "一星大小奇偶"
                    }
                    , {
                        m: TK.Miss.MissType.一星奇偶质合遗漏, t: "一星奇偶质合"
                    }
                    , {
                        m: TK.Miss.MissType.一星大小质合遗漏, t: "一星大小质合"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.chongmaIs, t: "重码"
                    }
                    , {
                        m: this.SqzsType.xiemaIs, t: ""
                    }
                    , {
                        m: this.SqzsType.tiaomaIs, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.exds二星单式: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.二星直选遗漏, t: "(定位)单式遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星直选复式, t: "(定位)复式遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星对子遗漏, t: "对子遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星胆码遗漏, t: "胆码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星连号遗漏, t: "连号遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星间隔遗漏, t: "间隔遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.xtyl, type: [{
                        m: TK.Miss.MissType.二星大小奇偶遗漏, t: "大小奇偶遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星奇偶质合遗漏, t: "奇偶质合遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星大小质合遗漏, t: "大小质合遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星012路遗漏, t: "012路遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.duiziIs, t: ""
                    }
                    , {
                        m: this.SqzsType.lianhaoIs, t: ""
                    }
                    , {
                        m: this.SqzsType.jiangeIs, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.sxds三星单式: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.三星直选遗漏, t: "(定位)遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星豹子遗漏, t: "豹子遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星组三遗漏, t: "组三遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星单胆遗漏, t: "单胆遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星双胆遗漏, t: "双胆遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.xtyl, type: [{
                        m: TK.Miss.MissType.三星大小遗漏, t: "大小遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星奇偶遗漏, t: "奇偶遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星质合遗漏, t: "质合遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星012路遗漏, t: "012路遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.wnyl, type: [{
                        m: TK.Miss.MissType.四码遗漏, t: "万能四码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.五码遗漏, t: "万能五码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.六码遗漏, t: "万能六码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.七码遗漏, t: "万能七码遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.xingtai, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.wxds五星单式: case TK.Bet.PlayType.wxtxds五星通选单式: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.fwyl, type: [{
                        m: TK.Miss.MissType.前二直选遗漏, t: "前两位遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星直选遗漏, t: "后两位遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.前三直选遗漏, t: "前三位遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.中三直选遗漏, t: "中三位遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星直选遗漏, t: "后三位遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.dxbi, t: ""
                    }
                    , {
                        m: this.SqzsType.jioubi, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihebi, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlubi, t: ""
                    }
                    , {
                        m: this.SqzsType.chs, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.exhz二星和值: case TK.Bet.PlayType.exzxhz二星组选和值: a = {
                cardAry: [{
                    name: this.MissTab.hz, type: [{
                        m: TK.Miss.MissType.二星和值遗漏, t: "二星和值遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星和值尾遗漏, t: "二星和值尾遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星和值段遗漏, t: "二星和值段遗漏"
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.sxhz三星和值: case TK.Bet.PlayType.sxzxhz三星组选和值: a = {
                cardAry: [{
                    name: this.MissTab.hz, type: [{
                        m: TK.Miss.MissType.三星和值遗漏, t: "三星和值遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星和值尾遗漏, t: "三星和值尾遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星和值段遗漏, t: "三星和值段遗漏"
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.sxzxsds三星组选三单式: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.三星组选遗漏, t: "三组遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.三星组三遗漏, t: "组三遗漏(不定位)"
                    }
                    ]
                }
                , {
                    name: this.MissTab.wnyl, type: [{
                        m: TK.Miss.MissType.四码组三遗漏, t: "万能四码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.五码组三遗漏, t: "万能五码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.六码组三遗漏, t: "万能六码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.七码组三遗漏, t: "万能七码遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.zusan, t: ""
                    }
                    , {
                        m: this.SqzsType.zuliu, t: ""
                    }
                    , {
                        m: this.SqzsType.chongma, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.sxzxlds三星组选六单式: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.三星直选遗漏, t: "三星直选(定位)"
                    }
                    , {
                        m: TK.Miss.MissType.三星组六遗漏, t: "组六三码(不定位)"
                    }
                    , {
                        m: TK.Miss.MissType.三星组六四码, t: "组六四码(不定位)"
                    }
                    , {
                        m: TK.Miss.MissType.三星组六五码, t: "组六五码(不定位)"
                    }
                    ]
                }
                , {
                    name: this.MissTab.wnyl, type: [{
                        m: TK.Miss.MissType.四码组六遗漏, t: "万能四码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.五码组六遗漏, t: "万能五码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.六码组六遗漏, t: "万能六码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.七码组六遗漏, t: "万能七码遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.zusan, t: ""
                    }
                    , {
                        m: this.SqzsType.zuliu, t: ""
                    }
                    , {
                        m: this.SqzsType.chongma, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.sxzxzh三星直选组合: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.三星直选遗漏, t: "三星直选(定位)"
                    }
                    , {
                        m: TK.Miss.MissType.三星组六遗漏, t: "组六三码(不定位)"
                    }
                    , {
                        m: TK.Miss.MissType.三星组六四码, t: "组六四码(不定位)"
                    }
                    , {
                        m: TK.Miss.MissType.三星组六五码, t: "组六五码(不定位)"
                    }
                    ]
                }
                , {
                    name: this.MissTab.wnyl, type: [{
                        m: TK.Miss.MissType.四码遗漏, t: "万能四码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.五码遗漏, t: "万能五码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.六码遗漏, t: "万能六码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.七码遗漏, t: "万能七码遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.zusan, t: ""
                    }
                    , {
                        m: this.SqzsType.zuliu, t: ""
                    }
                    , {
                        m: this.SqzsType.chongma, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.sxzxbd三组包胆: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.三星组选单胆, t: "三星组选单胆"
                    }
                    , {
                        m: TK.Miss.MissType.三星组选双胆, t: "三星组选双胆"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.zusan, t: ""
                    }
                    , {
                        m: this.SqzsType.zuliu, t: ""
                    }
                    , {
                        m: this.SqzsType.chongma, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.dxds大小单双: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.xtyl_dxds, type: [{
                        m: TK.Miss.MissType.大小单双组合遗漏, t: ""
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.dxds, t: "十位"
                    }
                    , {
                        m: this.SqzsType.dxds, t: "个位"
                    }
                    , {
                        m: this.SqzsType.quandaIs, t: ""
                    }
                    , {
                        m: this.SqzsType.quanxiaoIs, t: ""
                    }
                    , {
                        m: this.SqzsType.quandanIs, t: ""
                    }
                    , {
                        m: this.SqzsType.quanshuangIs, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.exzxds二星组选单式: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.二星组选遗漏, t: "二码（不定位）遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星组选三码, t: "三码（不定位）遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星组选四码, t: "四码（不定位）遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星组选五码, t: "五码（不定位）遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星连号遗漏, t: "二组连号遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星间隔遗漏, t: "二组间隔遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.duiziIs, t: ""
                    }
                    , {
                        m: this.SqzsType.lianhaoIs, t: ""
                    }
                    , {
                        m: this.SqzsType.chongma, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.exzxbd二星组选包胆: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.二星组选遗漏, t: "二组遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星组选单胆, t: "二组胆码遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星连号遗漏, t: "二组连号遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星间隔遗漏, t: "二组间隔遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.duiziIs, t: ""
                    }
                    , {
                        m: this.SqzsType.lianhaoIs, t: ""
                    }
                    , {
                        m: this.SqzsType.chongma, t: ""
                    }
                    ]
                }
                ]
            };
                break;
            case TK.Bet.PlayType.exzxfwds二星组选分位单式: a = {
                cardAry: [{
                    name: this.MissTab.cyyl, type: []
                }
                , {
                    name: this.MissTab.bzyl, type: [{
                        m: TK.Miss.MissType.二星组选遗漏, t: "二组遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星对子遗漏, t: "二组对子遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星连号遗漏, t: "二组连号遗漏"
                    }
                    , {
                        m: TK.Miss.MissType.二星间隔遗漏, t: "二组间隔遗漏"
                    }
                    ]
                }
                , {
                    name: this.MissTab.sqzs, type: [{
                        m: this.SqzsType.daxiao, t: ""
                    }
                    , {
                        m: this.SqzsType.jiou, t: ""
                    }
                    , {
                        m: this.SqzsType.zhihe, t: ""
                    }
                    , {
                        m: this.SqzsType.sanlu, t: ""
                    }
                    , {
                        m: this.SqzsType.duiziIs, t: ""
                    }
                    , {
                        m: this.SqzsType.lianhaoIs, t: ""
                    }
                    , {
                        m: this.SqzsType.chongma, t: ""
                    }
                    ]
                }
                ]
            };
                break
        }
        return a
    }
    , CreateTemplate: function (a) {
        if (typeof ($(this.BoxContainerLeft).data("missDivObj")) != "undefined") {
            return
        }
        var e = new Object();
        var h = [];
        for (var d = 0;
        d < a.cardAry.length;
        d++) {
            var b = a.cardAry[d], g;
            switch (b.name) {
                case this.MissTab.cyyl: h.push("<li><span>常用遗漏</span></li>");
                    var c = '<div class="panelList"><div class="panelTble panelTble2"></div><i class="shadowBottom"><i class="shadowBottomL"></i></i></div>';
                    g = $(c).appendTo(this.BoxContainerLeft).find("div.panelTble");
                    break;
                case this.MissTab.sqzs: h.push("<li><span>10期走势</span></li>");
                    var c = '<div class="panelList" style="display: none;"><div class="panelTble"></div><i class="shadowBottom"><i class="shadowBottomL"></i></i></div>';
                    g = $(c).appendTo(this.BoxContainerLeft).find("div.panelTble");
                    break;
                case this.MissTab.hz: var c = '<div class="panelList"><i class="shadowBottom"><i class="shadowBottomL"></i></i></div>';
                    g = $(c).prependTo(this.BoxContainerLeft);
                    break;
                case this.MissTab.xtyl_dxds: h.push("<li><span>形态遗漏</span></li>");
                    var c = '<div class="panelList panelListYilou" style="display: none;"><i class="shadowBottom"><i class="shadowBottomL"></i></i></div>';
                    g = $(c).appendTo(this.BoxContainerLeft);
                    break;
                default: h.push(String.format("<li><span>{0}</span></li>", b.name));
                    var c = '<div class="panelList" style="display: none;"><i class="shadowBottom"><i class="shadowBottomL"></i></i></div>';
                    g = $(c).appendTo(this.BoxContainerLeft);
                    break
            }
            e[b.name] = g
        }
        $(this.BoxContainerLeft).data("missDivObj", e);
        if (h.length > 0) {
            var f = [];
            f.push('<ul class="tab_panel">');
            f.push(h.join(""));
            f.push("</ul>");
            $(f.join("")).prependTo(this.BoxContainerLeft).find("li").each(function (j) {
                $(this).bind("click", function () {
                    $(this).addClass("active").siblings(".active").removeClass("active");
                    $(this).parent().siblings("div.panelList:eq(" + j + ")").show().siblings(".panelList:visible").hide()
                });
                j == 0 && $(this).click()
            })
        }

    }
    , CreateMissTab: function () {
        var c = this.CreateConfig();
        if (c == null) {
            return
        }
        this.CreateTemplate(c);
        var f = $(this.BoxContainerLeft).data("missDivObj");
        for (var d = 0;
        d < c.cardAry.length;
        d++) {
            var a = c.cardAry[d], h = [];
            var b = f[a.name];
            switch (a.name) {
                case this.MissTab.cyyl: $(b).empty();
                    h.push(this.CreateCYYL());
                    $(b).append(h.join(""));
                    break;
                case this.MissTab.sqzs: $(b).empty();
                    h.push(this.CreateSQZS(a.type));
                    $(h.join("")).appendTo(b).find("th:first").click(function () {
                        $(this).html($(this).text() == "期号↓" ? "期号↑" : "期号↓");
                        var i = $(this).parent();
                        $(i).siblings().remove().each(function (j) {
                            i.after(this)
                        })
                    }).css("cursor", "pointer");
                    break;
                case this.MissTab.hz: $(b).children(":not(i)").remove();
                    h.push(this.CreateHeZhi(a.type));
                    $(h.join("")).prependTo(b).find("span.plitem").hover(function () {
                        $(this).addClass("active")
                    }
                    , function () {
                        $(this).removeClass("active")
                    }).bind("click", {
                        self: this
                    }
                    , function (i) {
                        $(i.data.self).triggerHandler(i.data.self.Trigger_Handler, [parseInt($(this).attr("t"), 10), $(this).text()])
                    });
                    break;
                case this.MissTab.xtyl_dxds: $(b).children(":not(i)").remove();
                    h.push(this.CreateDXDS(a.type));
                    $(h.join("")).prependTo(b).find("span.ybtn").hover(function () {
                        $(this).addClass("ybtn_active")
                    }
                    , function () {
                        $(this).removeClass("ybtn_active")
                    }).bind("click", {
                        self: this
                    }
                    , function (i) {
                        $(i.data.self).triggerHandler(i.data.self.Trigger_Handler, [parseInt($(this).attr("t"), 10), $(this).text()])
                    });
                    break;
                default: $(b).children(":not(i)").remove();
                    for (var e = 0;
                    e < a.type.length;
                    e++) {
                        var g = this.MissObject[a.type[e].m];
                        h.push(this.CreateUlMiss(g, a.type[e].t))
                    }
                    $(h.join("")).prependTo(b).find("span.plitem").hover(function () {
                        $(this).addClass("active")
                    }
                    , function () {
                        $(this).removeClass("active")
                    }).bind("click", {
                        self: this
                    }
                    , function (i) {
                        $(i.data.self).triggerHandler(i.data.self.Trigger_Handler, [parseInt($(this).attr("t"), 10), $(this).text()])
                    });
                    break
            }

        }

    }
    , CreateCYYL: function () {
        var b = [{
            t: "大小单双", m: TK.Miss.MissType.大小单双组合遗漏
        }
        , {
            t: "一星", m: TK.Miss.MissType.一星
        }
        , {
            t: "二星和值", m: TK.Miss.MissType.二星和值遗漏
        }
        , {
            t: "三星和值", m: TK.Miss.MissType.三星和值遗漏
        }
        , {
            t: "二星(定位)", m: TK.Miss.MissType.二星直选遗漏
        }
        , {
            t: "二星(不定位)", m: TK.Miss.MissType.二星组选遗漏
        }
        , {
            t: "三星(定位)", m: TK.Miss.MissType.三星直选遗漏
        }
        , {
            t: "三星(不定位)", m: TK.Miss.MissType.三星组选遗漏
        }
        , {
            t: "万能四码", m: TK.Miss.MissType.四码遗漏
        }
        , {
            t: "万能五码", m: TK.Miss.MissType.五码遗漏
        }
        , {
            t: "万能六码", m: TK.Miss.MissType.六码遗漏
        }
        , {
            t: "万能七码", m: TK.Miss.MissType.七码遗漏
        }
        ];
        var a = [];
        for (var c = 0;
        c < b.length;
        c++) {
            var f = this.MissObject[b[c].m.toString()];
            if (typeof (f) == "undefined") {
                continue
            }
            if (typeof (f.yltable) == "undefined" || f.yltable.length == 0) {
                if (f.p.length < 3) {
                    continue
                }
                var e = f.p.length > 3 ? this.SortMiss(f.p.concat([]), 3) : f.p;
                var g = [];
                g.push(String.format('<table><tr><th colspan="2">{0}</th></tr>', b[c].t));
                for (var d = 0;
                d < e.length;
                d++) {
                    g.push(String.format('<tr><td style="width:58%;" align="center">{0}期</td>', e[d].c));
                    g.push(String.format('<td align="left">{0}</td></tr>', e[d].n.replace(/\|/g, "")))
                }
                g.push("</table>");
                f.yltable = g.join("")
            }
            a.push(f.yltable)
        }
        return a.join("")
    }
    , CreateDXDS: function (a) {
        var c = this.MissObject[a[0].m];
        if (typeof (c) == "undefined" || c.p.length == 0) {
            return ""
        }
        var d = ['<table class="panelYilou">'], e = [];
        for (var b = 0;
        b < c.p.length;
        b++) {
            if (b % 8 == 0) {
                if (b != 0) {
                    d.push("</tr>" + e.join("") + "<tr>");
                    e.clear();
                    d.push('</table><table class="panelYilou panelYilou2">')
                }
                d.push("<tr><td> </td>");
                e.push("<tr><td>遗漏</td>")
            }
            d.push(String.format('<td><span class="ybtn" t="{0}">{1}</span></td>', c.t, c.p[b].n));
            e.push("<td><span>" + c.p[b].c + "</span></td>")
        }
        d.push("</tr>" + e.join("") + "</tr></table>");
        return d.join("")
    }
    , CreateHeZhi: function (a) {
        var d = [];
        for (var o = 0;
        o < a.length;
        o++) {
            var r = this.MissObject[a[o].m.toString()];
            d.push(this.CreateUlMiss(r, a[o].t))
        }
        var k = [], f = [], m = [], g = [], n = [];
        var s = 2, h = [{
            s: 0, b: 8
        }
        , {
            s: 9, b: 11
        }
        , {
            s: 12, b: 13
        }
        , {
            s: 14, b: 15
        }
        , {
            s: 16, b: 18
        }
        , {
            s: 19, b: 27
        }
        ];
        switch (this.PlayType) {
            case TK.Bet.PlayType.exzxhzds二星和值单式: s = 3;
                h = [{
                    s: 0, b: 6
                }
                , {
                    s: 7, b: 9
                }
                , {
                    s: 9, b: 11
                }
                , {
                    s: 12, b: 18
                }
                ];
                break
        }
        for (var p = aryIssue.length - 10 < 1 ? 1 : aryIssue.length - 10;
        p < aryIssue.length;
        p++) {
            k.push(aryIssue[p].i.split("-")[1]);
            var c = [], e = 0;
            for (var q = s;
            q < aryIssue[p].b.length;
            q++) {
                var b = aryIssue[p].b.charAt(q);
                c.push(b);
                e += parseInt(b, 10)
            }
            f.push(c.join(","));
            m.push(e);
            for (var q = 0;
            q < h.length;
            q++) {
                if (e >= h[q].s && e <= h[q].b) {
                    g.push(String.format("{0}-{1}", h[q].s, h[q].b));
                    break
                }

            }
            n.push(e % 10)
        }
        d.push('<div class="trendShow"><table>');
        d.push("<tr><td>期号</td><td>" + k.join("</td><td>") + "</td></tr>");
        d.push("<tr><td>开奖号码</td><td>" + f.join("</td><td>") + "</td></tr>");
        d.push("<tr><td>和值</td><td>" + m.join("</td><td>") + "</td></tr>");
        d.push("<tr><td>和值段</td><td>" + g.join("</td><td>") + "</td></tr>");
        d.push("<tr><td>和值尾数</td><td>" + n.join("</td><td>") + "</td></tr>");
        d.push("</table></div>");
        return d.join("")
    }
    , SetTrSQZS: function (e, b, c, d) {
        var h = [], v = [];
        for (var o = 0;
        o < b.length;
        o++) {
            h.push(parseInt(b.charAt(o), 10))
        }
        for (var o = 0;
        o < e.length;
        o++) {
            var l = e[o], w = "";
            switch (l.m) {
                case this.SqzsType.daxiao: var j = [];
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        j.push(h[p] >= 5 ? "大" : "小")
                    }
                    w = j.join("");
                    break;
                case this.SqzsType.quandaIs: w = h[0] >= 5 && h[1] >= 5 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.quanxiaoIs: w = h[0] <= 4 && h[1] <= 4 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.dxbi: var g = 0;
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        h[p] >= 5 && g++
                    }
                    w = String.format("{0}:{1}", g, h.length - g);
                    break;
                case this.SqzsType.dxds: w = (h[o] >= 5 ? "大" : "小") + (h[o] % 2 == 1 ? "单" : "双");
                    break;
                case this.SqzsType.jiou: var j = [];
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        j.push(h[p] % 2 == 1 ? "奇" : "偶")
                    }
                    w = j.join("");
                    break;
                case this.SqzsType.quandanIs: w = h[0] % 2 == 1 && h[1] % 2 == 1 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.quanshuangIs: w = h[0] % 2 == 0 && h[1] % 2 == 0 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.jioubi: var m = 0;
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        h[p] % 2 == 1 && m++
                    }
                    w = String.format("{0}:{1}", m, h.length - m);
                    break;
                case this.SqzsType.zhihe: var j = [];
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        j.push(this.HeShuAry.exists(h[p]) ? "合" : "质")
                    }
                    w = j.join("");
                    break;
                case this.SqzsType.zhihebi: var n = 0;
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        this.HeShuAry.exists(h[p]) && n++
                    }
                    w = String.format("{0}:{1}", h.length - n, n);
                    break;
                case this.SqzsType.sanlu: var j = [];
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        j.push(h[p] % 3)
                    }
                    w = j.join("");
                    break;
                case this.SqzsType.sanlubi: var u = [0, 0, 0];
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        u[h[p] % 3]++
                    }
                    w = u.join(":");
                    break;
                case this.SqzsType.chs: var a = 0;
                    for (var p = 0;
                    p < c.length;
                    ) {
                        var q = c.charAt(p), t = new RegExp(q, "gm");
                        a += b.length - b.replace(t, "").length;
                        if (p + 1 <= c.length) {
                            c = c.substring(p + 1).replace(t, "")
                        }

                    }
                    w = a.toString();
                    break;
                case this.SqzsType.xingtai: var r = h.slice(0), s = r.clearRepeat().length;
                    w = s == 1 ? "豹子" : (s == 3 ? "组六" : "组三");
                    break;
                case this.SqzsType.zusan: var r = h.slice(0);
                    w = r.clearRepeat().length == 2 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.zuliu: var r = h.slice(0);
                    w = r.clearRepeat().length == 3 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.xiemaIs: w = h[0] == parseInt(c.charAt(0), 10) + 1 || h[0] == parseInt(c.charAt(0), 10) - 1 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.tiaomaIs: w = h[0].toString() != c.charAt(0) && h[0].toString() == d.charAt(0) ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.jiangeIs: w = Math.abs(h[0] - h[1]) == 2 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.duiziIs: w = h[0] == h[1] ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.lianhaoIs: w = Math.abs(h[0] - h[1]) == 1 ? "√" : "&nbsp;";
                    break;
                case this.SqzsType.chongmaIs: for (var p = 0;
                p < h.length;
                p++) {
                    if (c.indexOf(h[p].toString()) >= 0) {
                        w = "√";
                        break
                    }
                    if (p == h.length - 1) {
                        w = "&nbsp;"
                    }

                }
                    break;
                case this.SqzsType.chongma: var f = [];
                    for (var p = 0;
                    p < h.length;
                    p++) {
                        if (c.indexOf(h[p].toString()) >= 0 && !f.exists(h[p])) {
                            f.push(h[p])
                        }

                    }
                    w = f.join(",");
                    break
            }
            v.push("<td>" + w + "</td>")
        }
        return v.join("")
    }
    , CreateSQZS: function (a) {
        var e = 0;
        switch (this.PlayType) {
            case TK.Bet.PlayType.yxds一星单式: e = 4;
                break;
            case TK.Bet.PlayType.exds二星单式: case TK.Bet.PlayType.exzxds二星组选单式: case TK.Bet.PlayType.dxds大小单双: e = 3;
                break;
            case TK.Bet.PlayType.sxds三星单式: case TK.Bet.PlayType.sxzxsds三星组选三单式: case TK.Bet.PlayType.sxzxlds三星组选六单式: e = 2;
                break;
            case TK.Bet.PlayType.sxds四星单式: e = 1;
                break
        }
        var g = [];
        for (var c = aryIssue.length - 10 < 1 ? 1 : aryIssue.length - 10;
        c < aryIssue.length;
        c++) {
            g.push("<tr><td>" + aryIssue[c].i.split("-")[1] + "</td>");
            var d = aryIssue[c].b.substring(e);
            g.push("<td>" + aryIssue[c].b.substring(0, e) + "<strong>" + d + "</strong></td>");
            g.push(this.SetTrSQZS(a, d, aryIssue[c - 1].b.substring(e), aryIssue[c - 2].b.substring(e)));
            g.push("</tr>")
        }
        var f = ["<table><tr>"];
        f.push("<th>期号↓</th>");
        f.push("<th>开奖号码</th>");
        for (var b = 0;
        b < a.length;
        b++) {
            f.push("<th>" + (a[b].t.length > 0 ? a[b].t : a[b].m) + "</th>")
        }
        f.push("</tr>");
        f.push(g.join(""));
        f.push("</table>");
        return f.join("")
    }
    , CreateUlMiss: function (a, b) {
        if (typeof (a) == "undefined") {
            return ""
        }
        var c = "ylul" + this.PlayType.toString();
        if (typeof (a[c]) == "undefined" || a[c].length == 0) {
            if (a.p.length < 3) {
                return ""
            }
            var e = a.p.length > 3 ? this.SortMiss(a.p, 3) : a.p;
            var f = ['<ul class="pItem">'];
            for (var d = 0;
            d < e.length;
            d++) {
                f.push(String.format('<li><span class="plnum">{0}</span><span class="plitem" t="{1}"><span><em>{2}</em></span></span></li>', e[d].c, a.t, e[d].n))
            }
            f.push(String.format('<li class="item">{0}</li></ul>', b));
            a[c] = f.join("")
        }
        return a[c]
    }
    , SortMiss: function (a, b) {
        a.sort(function (c, d) {
            return d.c - c.c
        });
        return a.length > b ? a.slice(0, b) : a
    }
    , Initialize: function (c, a, d, b) {
        this.MissObject = c;
        this.BetMethod = a;
        this.PlayType = d;
        this.BoxContainerLeft = b;
        this.CreateMissTab()
    }

};
TK.Miss.RightMiss = function () { };
TK.Miss.RightMiss.prototype = {
    MissObject: null, BetMethod: -1, PlayType: -1, BoxContainerRight: null, HeShuAry: ["0", "4", "6", "8", "9"], IssueAllCount: -1, IcyHot: null, GetCodeMissConfig: function () {
        var a = [];
        switch (this.PlayType) {
            case TK.Bet.PlayType.wxds五星单式: case TK.Bet.PlayType.wxtxds五星通选单式: a.push(TK.Miss.MissType.万位);
                a.push(TK.Miss.MissType.千位);
            case TK.Bet.PlayType.sxds三星单式: a.push(TK.Miss.MissType.百位);
            case TK.Bet.PlayType.exds二星单式: case TK.Bet.PlayType.exzxfwds二星组选分位单式: a.push(TK.Miss.MissType.十位);
            case TK.Bet.PlayType.yxds一星单式: a.push(TK.Miss.MissType.一星);
                break;
            case TK.Bet.PlayType.sxzxsds三星组选三单式: a = [TK.Miss.MissType.三星号码分布, TK.Miss.MissType.三星组三对子];
                break;
            case TK.Bet.PlayType.sxzxbd三组包胆: case TK.Bet.PlayType.sxzxlds三星组选六单式: case TK.Bet.PlayType.sxzxzh三星直选组合: a = [TK.Miss.MissType.三星号码分布, TK.Miss.MissType.三星号码分布];
                break;
            case TK.Bet.PlayType.exhz二星和值: case TK.Bet.PlayType.exzxhz二星组选和值: a = [TK.Miss.MissType.二星和值遗漏];
                break;
            case TK.Bet.PlayType.sxhz三星和值: case TK.Bet.PlayType.sxzxhz三星组选和值: a = [TK.Miss.MissType.三星和值遗漏];
                break;
            case TK.Bet.PlayType.exzxds二星组选单式: case TK.Bet.PlayType.exzxbd二星组选包胆: a = [TK.Miss.MissType.二星号码分布];
                break;
            case TK.Bet.PlayType.dxds大小单双: a = [TK.Miss.MissType.十位大小遗漏, TK.Miss.MissType.十位单双遗漏, TK.Miss.MissType.个位大小遗漏, TK.Miss.MissType.个位单双遗漏];
                break
        }
        return a
    }
    , DisplayMiss: function () {
        var b = this.GetCodeMissConfig(), g = 0;
        switch (this.PlayType) {
            case TK.Bet.PlayType.exhz二星和值: case TK.Bet.PlayType.exzxhz二星组选和值: g = TK.Miss.MissType.二星和值遗漏;
            case TK.Bet.PlayType.sxhz三星和值: case TK.Bet.PlayType.sxzxhz三星组选和值: if (g == 0) {
                g = TK.Miss.MissType.三星和值遗漏
            }
                var f = this.MissObject[g];
                if (typeof (f) == "undefined") {
                    return
                }
                var c = new Array(), e = 0, a = this;
                $("tr.c2 span[n]", this.BoxContainerRight).each(function () {
                    var k = $(this).attr("n");
                    var j = $(f.p).filter(function () {
                        return this.n == k
                    }).get(0);
                    var i = j.a > 0 ? j.c / ((a.IssueAllCount - j.c - j.a) / j.a) : 0;
                    i = parseFloat(i.toFixed(2));
                    if (i > e) {
                        e = i
                    }
                    c.push(i)
                });
                var h = ["<th>欲出几率：</th>"];
                for (var d = 0;
                d < c.length;
                d++) {
                    h.push(String.format('<td{0}>{1}<i style="height:{2}px;"><em></em></i></td>', c[d] == e ? ' class="lger"' : "", c[d], (c[d] / e) * 50))
                }
                $("tr.c1", this.BoxContainerRight).html(h.join(""));
                break;
            default: var a = this;
                $("ul:has(span[n]+p)", this.BoxContainerRight).each(function (j) {
                    var p = [], o = [];
                    switch (a.PlayType) {
                        case TK.Bet.PlayType.dxds大小单双: for (var i = j * 2;
                        i < j * 2 + 2;
                        i++) {
                            var n = a.MissObject[b[i]];
                            if (typeof (n) == "undefined") {
                                continue
                            }
                            p = p.concat(n.p)
                        }
                            break;
                        case TK.Bet.PlayType.sxzxsds三星组选三单式: for (var i = 0;
                        i < 2;
                        i++) {
                            var n = a.MissObject[b[i]];
                            if (typeof (n) == "undefined") {
                                continue
                            }
                            if (i % 2 == 1) {
                                o = n.p
                            }
                            else {
                                p = n.p
                            }

                        }
                            break;
                        default: var n = a.MissObject[b[j]];
                            if (typeof (n) == "undefined" || n.p.length == 0) {
                                return true
                            }
                            p = n.p;
                            break
                    }
                    if (typeof (p) == "undefined" || p.length == 0) {
                        return true
                    }
                    var m = 0, l = 0;
                    for (var i = 0;
                    i < p.length;
                    i++) {
                        if (p[i].c > m) {
                            m = p[i].c
                        }

                    }
                    for (var i = 0;
                    i < o.length;
                    i++) {
                        if (o[i].c > l) {
                            l = o[i].c
                        }

                    }
                    $("span[n]", this).each(function () {
                        var r = $(this).attr("n");
                        if (a.PlayType == TK.Bet.PlayType.dxds大小单双) {
                            r = r.replace("单", "奇").replace("双", "偶")
                        }
                        var q = $(p).filter(function () {
                            return this.n == r
                        }).get(0);
                        var k = m == q.c ? String.format('<b style="color:#EBAB00;font-weight:bold;">{0}</b>', q.c) : q.c;
                        $(this).next().html(k);
                        if (o.length > 0) {
                            r += r;
                            q = $(o).filter(function () {
                                return this.n == r
                            }).get(0);
                            k = l == q.c ? String.format('<b style="color:#EBAB00;font-weight:bold;">{0}</b>', q.c) : q.c;
                            $(this).next().next().html(k)
                        }

                    })
                });
                break
        }

    }
    , HotChange: function () {
        this.IcyHot.Init();
        this.HotDisplay()
    }
    , ClickMiss: function (c, p) {
        var j = [], b = "active", f = [], g = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], a = this;
        switch (this.PlayType) {
            case TK.Bet.PlayType.wxds五星单式: case TK.Bet.PlayType.wxtxds五星通选单式: f = p.split("|");
                switch (c) {
                    case TK.Miss.MissType.前三直选遗漏: f = f.concat(["_", "_"]);
                        break;
                    case TK.Miss.MissType.前二直选遗漏: f = f.concat(["_", "_", "_"]);
                        break;
                    case TK.Miss.MissType.中三直选遗漏: f.push("_");
                        f = ["_"].concat(f);
                        break;
                    case TK.Miss.MissType.二星直选遗漏: f = ["_", "_", "_"].concat(f);
                        break;
                    case TK.Miss.MissType.三星直选遗漏: f = ["_", "_"].concat(f);
                        break
                }
                $("ul:has(span[n])", this.BoxContainerRight).each(function (k) {
                    $("span[n]", this).each(function () {
                        if (f[k] == "_" || $(this).attr("n") == f[k]) {
                            j.push(this)
                        }

                    })
                });
                break;
            case TK.Bet.PlayType.sxds三星单式: p = p.replace(new RegExp("[|]|[,]", "gm"), "");
                for (var l = 0;
                l < p.length;
                l++) {
                    f.push(p.charAt(l))
                }
                $("ul:has(span[n])", this.BoxContainerRight).each(function (q) {
                    switch (c) {
                        case TK.Miss.MissType.四码遗漏: case TK.Miss.MissType.五码遗漏: case TK.Miss.MissType.六码遗漏: case TK.Miss.MissType.七码遗漏: $("span[n]", this).each(function () {
                            if (f.exists($(this).attr("n"))) {
                                j.push(this)
                            }

                        });
                            break;
                        case TK.Miss.MissType.三星直选遗漏: case TK.Miss.MissType.三星单胆遗漏: case TK.Miss.MissType.三星双胆遗漏: case TK.Miss.MissType.三星豹子遗漏: $("span[n]", this).each(function () {
                            (f[q] == "_" || f[q] == $(this).attr("n")) && j.push(this)
                        });
                            break;
                        case TK.Miss.MissType.三星组三遗漏: $("span[n]", this).each(function () {
                            f.exists($(this).attr("n")) && j.push(this)
                        });
                            break;
                        case TK.Miss.MissType.三星012路遗漏: case TK.Miss.MissType.三星大小遗漏: case TK.Miss.MissType.三星奇偶遗漏: case TK.Miss.MissType.三星质合遗漏: var k = a.CheckNumber(g, [f[q]]);
                            $("li span[n]", this).each(function () {
                                k.exists($(this).attr("n")) && j.push(this)
                            });
                            break
                    }

                });
                break;
            case TK.Bet.PlayType.sxzxsds三星组选三单式: p = p.replace(new RegExp("[,]", "gm"), "");
                for (var l = 0;
                l < p.length;
                l++) {
                    f.push(p.charAt(l))
                }
                if ($("ul input:radio:eq(0)", this.BoxContainerRight).is(":checked")) {
                    if (f.length == 3) {
                        $("ul.btn_numlist:visible", this.BoxContainerRight).each(function (k) {
                            $("span[n]", this).each(function () {
                                if ($(this).attr("n") == f[k]) {
                                    j.push(this);
                                    return false
                                }

                            })
                        })
                    }
                    else {
                        alert("此遗漏类型和当前玩法的选号方式不匹配")
                    }

                }
                else {
                    $("ul:visible span[n]", this.BoxContainerRight).each(function () {
                        f.exists($(this).attr("n")) && j.push(this)
                    })
                }
                break;
            case TK.Bet.PlayType.sxzxlds三星组选六单式: case TK.Bet.PlayType.sxzxzh三星直选组合: p = p.replace(new RegExp("[,]|[|]", "gm"), "");
                for (var l = 0;
                l < p.length;
                l++) {
                    f.push(p.charAt(l))
                }
                $("ul.btn_numlist", this.BoxContainerRight).each(function (k) {
                    $("span[n]", this).each(function () {
                        f.exists($(this).attr("n")) && j.push(this)
                    })
                });
                break;
            case TK.Bet.PlayType.sxzxbd三组包胆: p = p.replace(new RegExp("[,]", "gm"), "");
                for (var l = 0;
                l < p.length;
                l++) {
                    f.push(p.charAt(l))
                }
                var h = $("ul input:radio:eq(0)", this.BoxContainerRight).prop("checked") ? 1 : 2;
                if (h != f.length) {
                    alert("此遗漏类型和当前玩法的选号方式不匹配");
                    return
                }
                $("ul.btn_numlist:visible", this.BoxContainerRight).each(function (k) {
                    $("span[n]", this).each(function () {
                        f[k] == $(this).attr("n") && j.push(this)
                    })
                });
                break;
            case TK.Bet.PlayType.exds二星单式: case TK.Bet.PlayType.exzxfwds二星组选分位单式: switch (c) {
                case TK.Miss.MissType.二星大小奇偶遗漏: case TK.Miss.MissType.二星大小质合遗漏: case TK.Miss.MissType.二星奇偶质合遗漏: case TK.Miss.MissType.二星012路遗漏: f = p.split("|");
                    for (var l = 0;
                    l < f.length;
                    l++) {
                        var e = [];
                        for (var m = 0;
                        m < f[l].length;
                        m++) {
                            e.push(f[l].charAt(m))
                        }
                        var d = this.CheckNumber(g, e);
                        $("ul.btn_numlist:eq(" + l + ") span[n]", this.BoxContainerRight).each(function () {
                            d.exists($(this).attr("n")) && j.push(this)
                        })
                    }
                    break;
                case TK.Miss.MissType.二星直选复式: f = p.split("|");
                    for (var l = 0;
                    l < f.length;
                    l++) {
                        var o = f[l].split(",");
                        $("ul.btn_numlist:eq(" + l + ") span[n]", this.BoxContainerRight).each(function () {
                            (o.exists($(this).attr("n"))) && j.push(this)
                        })
                    }
                    break;
                default: p = p.replace(new RegExp("[|]|[,]", "gm"), "");
                    for (var l = 0;
                    l < p.length;
                    l++) {
                        var n = p.charAt(l);
                        $("ul.btn_numlist:eq(" + l + ") span[n]", this.BoxContainerRight).each(function () {
                            (n == "_" || n == $(this).attr("n")) && j.push(this)
                        })
                    }

            }
                break;
            case TK.Bet.PlayType.exzxds二星组选单式: case TK.Bet.PlayType.exzxbd二星组选包胆: f = p.split(",");
                $("ul.btn_numlist span[n]", this.BoxContainerRight).each(function (k) {
                    f.exists($(this).attr("n")) && j.push(this)
                });
                break;
            case TK.Bet.PlayType.exhz二星和值: case TK.Bet.PlayType.exzxhz二星组选和值: case TK.Bet.PlayType.sxhz三星和值: case TK.Bet.PlayType.sxzxhz三星组选和值: b = "num_on";
                $("table span[n]", this.BoxContainerRight).each(function () {
                    var i = $(this).attr("n");
                    switch (c) {
                        case TK.Miss.MissType.二星和值遗漏: case TK.Miss.MissType.三星和值遗漏: if (p == i) {
                            j.push(this)
                        }
                            break;
                        case TK.Miss.MissType.二星和值尾遗漏: case TK.Miss.MissType.三星和值尾遗漏: if (p == (parseInt(i, 10) % 10).toString()) {
                            j.push(this)
                        }
                            break;
                        case TK.Miss.MissType.二星和值段遗漏: case TK.Miss.MissType.三星和值段遗漏: f = p.split("-");
                            if (parseInt(f[0], 10) <= parseInt(i, 10) && parseInt(f[1], 10) >= parseInt(i, 10)) {
                                j.push(this)
                            }
                            break
                    }

                });
                break;
            case TK.Bet.PlayType.yxds一星单式: var d = [];
                switch (c) {
                    case TK.Miss.MissType.一星: d = [p];
                        break;
                    case TK.Miss.MissType.一星二码: case TK.Miss.MissType.一星三码: for (var l = 0;
                    l < p.length;
                    l++) {
                        d.push(p.charAt(l))
                    }
                        break;
                    default: var e = [];
                        for (var l = 0;
                        l < p.length;
                        l++) {
                            e.push(p.charAt(l))
                        }
                        d = this.CheckNumber(g, e);
                        break
                }
                $("ul.btn_numlist span[n]", this.BoxContainerRight).each(function () {
                    d.exists($(this).attr("n")) && j.push(this)
                });
                break;
            case TK.Bet.PlayType.dxds大小单双: $("ul:has(span[n])", this.BoxContainerRight).each(function (k) {
                var q = p.charAt(k);
                $("span[n]", this).each(function () {
                    if ($(this).attr("n") == q) {
                        j.push(this);
                        return false
                    }

                })
            });
                break
        }
        for (var l = 1;
        l <= j.length;
        l++) {
            if (l == 1) {
                $("ul span[n].active,table span[n].num_on", this.BoxContainerRight).removeClass("active num_on")
            }
            l == j.length ? $(j[l - 1]).click() : $(j[l - 1]).addClass(b)
        }

    }
    , HotDisplay: function () {
        var c = -1, b = 0, a = this;
        switch (this.PlayType) {
            case TK.Bet.PlayType.wxtxds五星通选单式: case TK.Bet.PlayType.wxds五星单式: c = 0;
            case TK.Bet.PlayType.sxds三星单式: if (c < 0) {
                c = 0;
                b = 2
            }
            case TK.Bet.PlayType.exds二星单式: case TK.Bet.PlayType.exzxfwds二星组选分位单式: if (c < 0) {
                c = 0;
                b = 3
            }
            case TK.Bet.PlayType.yxds一星单式: if (c < 0) {
                c = 0;
                b = 4
            }
            case TK.Bet.PlayType.exzxds二星组选单式: case TK.Bet.PlayType.exzxbd二星组选包胆: if (c < 0) {
                c = 2;
                b = -1
            }
            case TK.Bet.PlayType.dxds大小单双: if (c < 0) {
                c = 3
            }
            case TK.Bet.PlayType.sxzxsds三星组选三单式: case TK.Bet.PlayType.sxzxlds三星组选六单式: case TK.Bet.PlayType.sxzxbd三组包胆: case TK.Bet.PlayType.sxzxzh三星直选组合: if (c < 0) {
                c = 1;
                b = -1
            }
                $("ul.btn_numlist:has(span[n])", this.BoxContainerRight).each(function (e) {
                    var d = [];
                    if (b < 0) {
                        d = a.IcyHot.IcyHotDate[c]
                    }
                    else {
                        d = a.IcyHot.IcyHotDate[c][b + e]
                    }
                    var f = d.getMax();
                    $("span[n]", this).each(function (g) {
                        $(this).siblings("p:last").html(d[g] == f ? String.format('<b style="color:#EBAB00;font-weight:bold;">{0}</b>', d[g]) : d[g])
                    })
                });
                break
        }
        if ($("input:checkbox", this.BoxContainerRight).data("status") != true) {
            $("input:checkbox", this.BoxContainerRight).data("status", true).bind("click", function () {
                if (typeof ($(this).data("liAry")) == "undefined" || $(this).data("liAry").length == 0) {
                    $(this).data("liAry", $(this).parents("ul").siblings("div.numArea").find("li>p:last-child"))
                }
                $(this).data("liAry").toggle($(this).prop("checked"))
            })
        }

    }
    , CheckNumber: function (b, a) {
        for (var d = 0;
        d < a.length;
        d++) {
            var e = new Array();
            switch (a[d]) {
                case "0": case "1": case "2": var f = parseInt(a[d], 10);
                    e = $(b).filter(function () {
                        return this % 3 == f
                    });
                    break;
                case "奇": case "单": e = $(b).filter(function () {
                    return this % 2 == 1
                });
                    break;
                case "偶": case "双": e = $(b).filter(function () {
                    return this % 2 == 0
                });
                    break;
                case "质": var c = this.HeShuAry;
                    e = $(b).filter(function () {
                        return !c.exists(this.toString())
                    });
                    break;
                case "合": var c = this.HeShuAry;
                    e = $(b).filter(function () {
                        return c.exists(this.toString())
                    });
                    break;
                case "大": e = $(b).filter(function () {
                    return this >= 5
                });
                    break;
                case "小": e = $(b).filter(function () {
                    return this <= 4
                });
                    break;
                default: break
            }
            b = e
        }
        return b.toArray()
    }
    , Initialize: function (d, a, e, b, c) {
        this.MissObject = d;
        this.BetMethod = a;
        this.PlayType = e;
        this.BoxContainerRight = b;
        this.IssueAllCount = c;
        if (this.IcyHot == null) {
            this.IcyHot = new TK.Miss();
            this.IcyHot.Init()
        }
        this.DisplayMiss()
    }

};
TK.Miss.CreateBox = function () {
    this.Initialize.bind(this);
    this.Initialize.apply(this, arguments)
};
TK.Miss.CreateBox.prototype = {
    MissObj: null, IssueAllCount: -1, IcyHot: null, BoxContainer: null, BoxLeftContainer: null, BoxRightContainer: null, BetMethodValue: TK.Bet.BetMethod.常规选号, PlayType: -1, RecentBonus: null, LeftMiss: new TK.Miss.LeftMiss(), RightMiss: null, InitStatus: true, MissObjParse: function (a) {
        var l = [], m = [];
        for (var c = 0;
        c < a.length;
        c++) {
            var g = a[c].split("#");
            if (g.length < 2) {
                continue
            }
            var h = g[0].split("!");
            var f = {
                p: [], t: parseInt(h[0], 10)
            };
            for (var b = 1;
            b < g.length;
            b++) {
                var n = new Object();
                var o = g[b].split("!");
                for (var d = 0;
                d < o.length;
                d++) {
                    var e = "";
                    switch (d) {
                        case 0: e = "n";
                            break;
                        case 1: e = "c";
                            break;
                        case 2: e = "a";
                            break;
                        case 3: e = "m";
                            break;
                        case 4: default: e = "l";
                            break
                    }
                    n[e] = d == 0 ? o[d] : parseInt(o[d], 10)
                }
                f.p.push(n)
            }
            this.MissObj[h[0]] = f;
            h.length > 1 && h[1] == "1" ? m.push(h[0]) : l.push(h[0])
        }
        return m.length > 0 ? [true, l] : [false]
    }
    , Box_ShowMiss: function (a) {
        this.InitDom();
        this.RightMiss.Initialize(this.MissObj, this.BetMethodValue, this.PlayType, this.BoxRightContainer, this.IssueAllCount);
        a && this.RightMiss.HotDisplay();
        this.InitStatus = false;
        this.LeftMiss.Initialize(this.MissObj, this.BetMethodValue, this.PlayType, this.BoxLeftContainer)
    }
    , Box_AddIssue: function (c, b) {
        var d = {
            i: c, b: b
        }
        , a = this;
        aryIssue.push(d);
        setTimeout(function () {
            a.PostMiss("")
        }
        , $S.Math.RandomAry([18, 19, 20, 21], 1, false) * 1000);
        this.RightMiss.HotChange();
        this.RecentBonus.ChangeBonus(d)
    }
    , InitDom: function () {
        switch (this.PlayType) {
            case TK.Bet.PlayType.exhz二星和值: case TK.Bet.PlayType.exzxhz二星组选和值: case TK.Bet.PlayType.sxhz三星和值: case TK.Bet.PlayType.sxzxhz三星组选和值: this.BoxLeftContainer = this.BoxContainer;
                this.BoxRightContainer = $(this.BoxContainer).find("div.drawTrend");
                break;
            default: this.BoxLeftContainer = $(this.BoxContainer).find("div.panelLeft");
                this.BoxRightContainer = $(this.BoxContainer).find("div.numCheck");
                break
        }

    }
    , PostMiss: function (a) {
        var b = new $S.JsonCommand();
        b.Command = TK.Command.Enum.RecentBetMiss;
        b.ListParameter.push(TK.CurrentLotteryType);
        b.ListParameter.push(a);
        TK.Command.sendAjax(b)
    }
    , Initialize: function () {
        this.LeftMiss = new TK.Miss.LeftMiss();
        this.RightMiss = new TK.Miss.RightMiss();
        $(this.LeftMiss).bind(this.LeftMiss.Trigger_Handler, {
            _self: this
        }
        , function (c, b, a) {
            c.data._self.RightMiss.ClickMiss(b, a)
        });
        $(TK.Command).bind("RetBetMiss", {
            self: this
        }
        , function (b, d) {
            var c = JSON.parse(d.ListParameter[0]), a = [];
            if (c.c == "0") {
                a = b.data.self.MissObjParse(c.p);
                if (a[0]) {
                    if (b.data.self.Status != true) {
                        setTimeout(function () {
                            b.data.self.PostMiss(a[1].join("|"))
                        }
                        , 15000);
                        b.data.self.Status = true
                    }
                    else {
                        b.data.self.Status = false
                    }

                }
                else {
                    b.data.self.Status = false
                }
                b.data.self.IssueAllCount = parseInt(c.r, 10);
                b.data.self.Box_ShowMiss(b.data.self.InitStatus)
            }

        });
        $(this).bind("box_showMiss", function (c, b, a) {
            if (this.RecentBonus == null) {
                this.RecentBonus = new todayBonus({
                    indexFormat: "000", todayCount: 120, rowCount: 20, todayIssue: aryIssue
                })
            }
            if (a.Value != TK.Bet.BetMethod.常规选号) {
                return
            }
            this.BetMethodValue = a.Value;
            this.BoxContainer = a.Box;
            this.PlayType = b;
            if (this.MissObj == null) {
                this.MissObj = new Object();
                this.PostMiss("")
            }
            else {
                this.Box_ShowMiss(true)
            }

        })
    }

};
if (typeof (TK.Bet.Secretary) == "undefined") {
    TK.Bet.Secretary = function () { }
}
function _SecretaryData(b, a) {
    if (typeof (b) == "string") {
        b = JSON.parse(b)
    }
    b = $.extend({
        DataType: b.DataType || b.d || 0, IssueNumber: b.IssueNumber || b.i || "", ListDataInfo: b.ListDataInfo || JSON.parse(b.l) || [], CurShowMaxMissArea: b.CurShowMaxMissArea || 0
    }
    , b);
    for (var c = 0;
    c < b.ListDataInfo.length;
    c++) {
        b.ListDataInfo[c] = new _SecretaryData.MissData(b.ListDataInfo[c], a)
    }
    return b
}
_SecretaryData.MissData = function (b, a) {
    b = $.extend({
        Number: b.n || b.Number || "", CurMiss: b.c || b.CurMiss || 0, LastMiss: b.l || b.LastMiss || 0, ListMaxMiss: b.lm || b.ListMaxMiss || [], AppearCount: b.AppearCount || 0
    }
    , b);
    if (typeof (a) != "undefined" && typeof (a.lengre) == "function") {
        b.AppearCount = a.lengre(b.Number)
    }
    return b
};
function __SecretaryConfig() { } __SecretaryConfig.prototype = {
    PlayTypeName: "", PlayTypeValue: 0, Wrap: $(""), DomListMenu: [], ListChildType: [], SelectedChildTypeRadio: null
};
function ___SecretaryType() { } ___SecretaryType.prototype = {
    TypeName: "", TypeValue: 0, PlayTypeValue: 0, Wrap: $(""), DomBox: $(""), Class: "", Kind: 0, ListData: [], BetCount: 1, filter: function (a, b) { }
};
TK.Bet.Secretary.prototype = {
    DomID: "TK_Bet_Secretary", DomWrap: $(""), DomListMenu: [], DomSecCon: $(""), DomMyRecordWrap: $(""), ObjectData: {}, ListMenuData: [], NeedUpdateMyBetData: true, CurrentPlayTypeMenu: null, CurrentChildTypeRadio: null, ListAreaMaxMiss: [0, 200, 500, 1000], SubmitUrl: "/handler/kuaikai/secretary.ashx", show: function () {
        $.artDialog({
            id: this.DomID, content: this.DomWrap.get(0)
        });
        if (this.CurrentPlayTypeMenu == null) {
            this.menuChanged(this.DomListMenu[0])
        }

    }
    , hide: function () {
        $.artDialog.get(this.DomID).close()
    }
    , myBetAddNumber: function (a) {
        $(a).find("tbody input[type=checkbox]:checked").each(function () {
            var b = $(this).prop("betitem");
            kkBet.BetList.addItem(null, b.init())
        });
        this.hide()
    }
    , myBetCompute: function (a) {
        var b = 0;
        $(a).find("tbody input[type=checkbox]:checked").each(function () {
            b += parseInt($(this).val(), 10)
        });
        $(a).find(".chartCheck span.sum").html(String.format('共{0}注，共<em class="f_highlight">{1}</em>元', b.toMoney(""), (b * TK.PerMoney).toMoney("")))
    }
    , responseMyBetData: function (c, b) {
        var n = JSON.parse(b);
        if (n.length == 0) {
            $(c).html('<p class="Snobet">您近期没有投注记录</p>');
            return
        }
        var f = [], l = [];
        f.push('<div class="Smytit">查询到<b>' + n.length + "</b>个方案</div>");
        f.push('<div class="Smybet">');
        for (var h = 0;
        h < n.length;
        h++) {
            var m = JSON.parse(n[h].c);
            var g = [], p = 0, q = [];
            for (var k = 0;
            k < m.length;
            k++) {
                var e = m[k];
                var d = new TK.Bet.Util.BetItem();
                d = $.extend(d, {
                    PlayType: e.p, BetNumber: e.c, BetMoney: e.m, BetCount: e.m / TK.PerMoney, BetMethod: TK.Bet.BetMethod.我的选号
                }).init();
                g.push(String.format('<tr><td>{0}</td><td>{1}</td><td>{2}</td><td width="40"><input type="checkbox" value="{3}" /></td></tr>', d.PlayTypeName, d.BetNumber, d.BetMoney.toMoney(""), d.BetCount));
                l.push(d);
                p += d.BetMoney;
                if (!q.exists(d.PlayTypeName)) {
                    switch (q.length) {
                        case 0: q.push(d.PlayTypeName);
                            break;
                        case 1: q.push("等");
                            break
                    }

                }

            }
            f.push('<div class="Smybet_box">');
            f.push(String.format('<div class="SecN_tit"><span>{0} {1}<em>金额<i>{2}</i>元</em></span><a class="open on" title="展开"></a></div>', (h + 1), q.join(" "), p.toMoney("")));
            f.push('<div class="SecN_td"><table class="Sectable">');
            f.push('<thead><tr><th>玩法</th><th>号码 </th><th>单倍金额</th><th width="40"><input type="checkbox" /></th></tr></thead>');
            f.push("<tbody>");
            f.push(g.join(""));
            f.push("</tbody>");
            f.push("</table></div>");
            f.push("</div>")
        }
        f.push("</div>");
        f.push('<div class="chartCheck"><span class="sum">共0注，共<em class="f_highlight">0</em>元</span><span class="suml"><a class="btnChartCheck">添加号码</a></span></div>');
        var o = $(f.join("")).appendTo($(c).html(""));
        var a = this;
        o.find("div.SecN_tit").each(function () {
            $(this).click(function () {
                $(this).find("a").toggleClass("on");
                $(this).nextAll("div.SecN_td").toggle();
                o.filter(".Smybet").jscroll({
                    Height: 330, EnableMaxHeight: false, Bar: {
                        Pos: "auto"
                    }

                })
            })
        });
        o.find("thead input[type=checkbox]").each(function () {
            $(this).click(function () {
                var i = this;
                $(this).parent().parent().parent().nextAll("tbody").find("input[type=checkbox]").each(function () {
                    this.checked = i.checked
                });
                a.myBetCompute(c)
            })
        });
        o.find("tbody input[type=checkbox]").each(function (i) {
            $(this).prop("betitem", l[i]);
            $(this).click(function () {
                a.myBetCompute(c)
            })
        });
        o.find("a.btnChartCheck").click(this.myBetAddNumber.bind(this, c));
        o.filter(".Smybet").jscroll({
            Height: 330, EnableMaxHeight: false, Bar: {
                Pos: "auto"
            }

        })
    }
    , requestMyBetData: function (a) {
        if (this.NeedUpdateMyBetData == true) {
            this.NeedUpdateMyBetData == false;
            $(a).html('<p class="loading"></p>');
            var b = new $S.JsonCommand();
            b.Command = "record";
            b.ListParameter.push(TK.CurrentLotteryType);
            $.ajax({
                type: "post", url: this.SubmitUrl, data: {
                    d: b.toJSONString()
                }
                , success: this.responseMyBetData.bind(this, a), error: function (c) {
                    _self.StatusBetting = false;
                    var d = $('<div class="dia_c_main"><div class="cet"><b class="plaint">&nbsp;</b><span>' + c.statusText + '</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>').find("a.Dbutt").click(function () {
                        $.artDialog.get(this.DomID).close()
                    }).end();
                    $.artDialog.get(this.DomID).title("系统异常").content(d.get(0))
                }
                .bind(this)
            })
        }

    }
    , searchAddNumber: function (a) {
        if ($S.Debug.IntelliSense) {
            a = new ___SecretaryType()
        }
        $(a.DomBox).find("tbody input[type=checkbox]:checked").each(function () {
            var b = new TK.Bet.Util.BetItem();
            b.BetNumber = $(this).val();
            if (typeof (a.createNumber) == "function") {
                b.BetNumber = a.createNumber(b.BetNumber)
            }
            b.BetCount = a.BetCount;
            b.BetMoney = b.BetCount * TK.PerMoney;
            b.BetMethod = TK.Bet.BetMethod.秘书选号;
            b.PlayType = a.PlayTypeValue;
            kkBet.BetList.addItem(null, b.init())
        });
        this.hide()
    }
    , searchCompute: function (a) {
        if ($S.Debug.IntelliSense) {
            a = new ___SecretaryType()
        }
        var b = a.BetCount * $(a.DomBox).find("tbody input[type=checkbox]:checked").length;
        var c = b * TK.PerMoney;
        $(a.Wrap).find("div.chartCheck span.sum i").html(b.toMoney(""));
        $(a.Wrap).find("div.chartCheck em.f_highlight").html(c.toMoney(""))
    }
    , createMaxMissMenu: function (a) {
        var b = [];
        b.push("<select>");
        for (var c = 0;
        c < this.ListAreaMaxMiss.length;
        c++) {
            b.push(String.format('<option{0} value="{2}">{1}最大遗漏</li>', this.ListAreaMaxMiss[c] == a.CurShowMaxMissArea ? ' selected="true"' : "", this.ListAreaMaxMiss[c] == 0 ? "历史" : "近" + this.ListAreaMaxMiss[c] + "期", this.ListAreaMaxMiss[c]))
        }
        b.push("</select>");
        return b.join("")
    }
    , searchDeal: function (b, c, d) {
        if ($S.Debug.IntelliSense) {
            b = new ___SecretaryType()
        }
        d = d || 0;
        var l = [], s = this.ObjectData[b.TypeValue], k = typeof (b.lengre) == "function";
        if ($S.Debug.IntelliSense) {
            s = new _SecretaryData()
        }
        var a = this;
        switch (b.Kind) {
            case 2: var m = [], n = [], o = [], f = "一二三四五六七八九十";
                var g = [].concat(s.ListDataInfo);
                switch (c) {
                    case "按号码显示": break;
                    case "按遗漏显示": g.sort(function (i, v) {
                        if ($S.Debug.IntelliSense) {
                            i = new _SecretaryData.MissData();
                            v = new _SecretaryData.MissData()
                        }
                        return i.CurMiss - v.CurMiss
                    });
                        break
                }
                var h = Math.ceil(g.length / 10);
                for (var p = 0;
                p < 10;
                p++) {
                    var e = [];
                    m.push(e);
                    if (p % 2 == 0) {
                        n.push(e)
                    }
                    else {
                        o.push(e)
                    }

                }
                for (var p = 0;
                p < g.length;
                p++) {
                    var e = m[parseInt(p / h, 10)];
                    var q = g[p];
                    if ($S.Debug.IntelliSense) {
                        q = new _SecretaryData.MissData()
                    }
                    e.push(String.format('<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td>{4}<td width="40"><input type="checkbox" checked="checked" value="{0}" /></td></tr>', q.Number, q.CurMiss, q.LastMiss, q.ListMaxMiss[this.ListAreaMaxMiss.indexOf(s.CurShowMaxMissArea)], (k ? "<td>" + q.AppearCount + "</td>" : "")))
                }
                l.push('<div class="hmd_L">');
                for (var p = 0;
                p < n.length;
                p++) {
                    var e = n[p];
                    l.push('<div class="hmd_box">');
                    l.push(String.format('<div class="SecN_tit"><span>第{0}段（共<i>{1}</i>注）</span><a class="open" title="展开"></a></div>', f.charAt(2 * p), e.length * b.BetCount));
                    l.push('<div class="SecN_td">');
                    l.push('<table class="Sectable">');
                    l.push(String.format('<thead><tr><th>号码<b class="sub sup"></b></th><th>当前遗漏</th><th>上次遗漏</th><th>最大遗漏</th>{0}<th width="40"><input type="checkbox" checked /></th></tr></thead>', (k ? "<th>20期冷热</th>" : "")));
                    l.push("<tbody>");
                    l.push(e.join(""));
                    l.push("</tbody>");
                    l.push("</table>");
                    l.push("</div>");
                    l.push("</div>")
                }
                l.push("</div>");
                l.push('<div class="hmd_R">');
                for (var p = 0;
                p < o.length;
                p++) {
                    var e = o[p];
                    l.push('<div class="hmd_box">');
                    l.push(String.format('<div class="SecN_tit"><span>第{0}段（共<i>{1}</i>注）</span><a class="open" title="展开"></a></div>', f.charAt(2 * p + 1), e.length * b.BetCount));
                    l.push('<div class="SecN_td">');
                    l.push('<table class="Sectable">');
                    l.push(String.format('<thead><tr><th>号码<b class="sub sup"></b></th><th>当前遗漏</th><th>上次遗漏</th><th>最大遗漏</th>{0}<th width="40"><input type="checkbox" checked /></th></tr></thead>', (k ? "<th>20期冷热</th>" : "")));
                    l.push("<tbody>");
                    l.push(e.join(""));
                    l.push("</tbody>");
                    l.push("</table>");
                    l.push("</div>");
                    l.push("</div>")
                }
                l.push("</div>");
                l.push('<div style="clear:left;"></div>');
                var t = $(l.join("")).appendTo($(b.DomBox).html(""));
                $(t).find("div.SecN_tit").each(function () {
                    $(this).click(function () {
                        $(this).find("a").toggleClass("on");
                        $(this).nextAll("div.SecN_td").toggle();
                        $(b.DomBox).jscroll({
                            Height: 280, EnableMaxHeight: true, Bar: {
                                Pos: "auto"
                            }

                        })
                    })
                });
                var r = $(t).find("tbody input[type=checkbox]").each(function () {
                    $(this).click(a.searchCompute.bind(a, b))
                });
                $(t).find("thead input[type=checkbox]").click(function () {
                    var i = this;
                    $(i).parent().parent().parent().parent().find("tbody input[type=checkbox]").each(function () {
                        this.checked = i.checked
                    });
                    a.searchCompute(b)
                });
                $(b.DomBox).jscroll({
                    Height: 280, EnableMaxHeight: true
                });
                break;
            default: var m = [], u = 0, j = typeof (b.convertType) == "function";
                s.ListDataInfo.sort(function (i, w) {
                    if ($S.Debug.IntelliSense) {
                        i = new _SecretaryData.MissData();
                        w = new _SecretaryData.MissData()
                    }
                    var v = parseFloat(i.Number.toString().replace(/[|,]/ig, ""));
                    var x = parseFloat(w.Number.toString().replace(/[|,]/ig, ""));
                    var y, z = (b.Sort && b.Sort[d]) || 1;
                    if (j) {
                        switch (d) {
                            case 0: y = (b.convertType(i.Number) - b.convertType(w.Number)) * z;
                                if (y == 0) {
                                    z = (b.Sort && b.Sort[d + 1]) || 1;
                                    y = (v - x) * z
                                }
                                break;
                            case 1: y = (v - x) * z;
                                break;
                            case 2: y = (i.CurMiss - w.CurMiss) * z;
                                break;
                            case 3: y = (i.LastMiss - w.LastMiss) * z;
                                break;
                            case 4: y = (i.ListMaxMiss[a.ListAreaMaxMiss.indexOf(s.CurShowMaxMissArea)] - w.ListMaxMiss[a.ListAreaMaxMiss.indexOf(s.CurShowMaxMissArea)]) * z;
                                break;
                            case 5: y = (i.AppearCount - w.AppearCount) * z;
                                break
                        }

                    }
                    else {
                        switch (d) {
                            case 0: y = (v - x) * z;
                                break;
                            case 1: y = (i.CurMiss - w.CurMiss) * z;
                                break;
                            case 2: y = (i.LastMiss - w.LastMiss) * z;
                                break;
                            case 3: y = (i.ListMaxMiss[a.ListAreaMaxMiss.indexOf(s.CurShowMaxMissArea)] - w.ListMaxMiss[a.ListAreaMaxMiss.indexOf(s.CurShowMaxMissArea)]) * z;
                                break;
                            case 4: y = (i.AppearCount - w.AppearCount) * z;
                                break
                        }

                    }
                    return y
                });
                for (var p = 0;
                p < s.ListDataInfo.length;
                p++) {
                    var q = s.ListDataInfo[p];
                    if ($S.Debug.IntelliSense) {
                        q = new _SecretaryData.MissData()
                    }
                    if (c.length > 0 && !b.filter(c, q.Number)) {
                        continue
                    }
                    u += b.BetCount;
                    if (j) {
                        m.push(String.format('<tr><td class="sw1">{0}</td><td class="sw2">{1}</td><td class="sw3">{2}</td><td class="sw4">{3}</td><td style="width:150px;">{4}</td>{5}<td width="35"><input type="checkbox" checked="checked" value="{1}" /></td></tr>', b.convertType(q.Number), q.Number, q.CurMiss, q.LastMiss, q.ListMaxMiss[this.ListAreaMaxMiss.indexOf(s.CurShowMaxMissArea)], (k ? "<td>" + q.AppearCount + "</td>" : "")))
                    }
                    else {
                        m.push(String.format('<tr><td class="sw1">{0}</td><td class="sw2">{1}</td><td class="sw3">{2}</td><td style="width:150px;">{3}</td>{4}<td width="35"><input type="checkbox" checked="checked" value="{0}" /></td></tr>', q.Number, q.CurMiss, q.LastMiss, q.ListMaxMiss[this.ListAreaMaxMiss.indexOf(s.CurShowMaxMissArea)], (k ? '<td class="sw4">' + q.AppearCount + "</td>" : "")))
                    }

                }
                l.push(String.format('<div class="SecN_tit"><span>查询结果<em class="white">{0}：</em><span></span><em>共<b>{2}</b>注</em></span>数据截止到【<i>{1}</i>】期</div>', b.TypeName, s.IssueNumber, u));
                if (j) {
                    l.push(String.format('<div class="SecN_th"><table class="Sectable"><thead><tr><th class="sw1"><em>' + b.TypeName + '</em></th><th class="sw2"><em>号码</em></th><th class="sw3"><em>当前遗漏</em></th><th class="sw4"><em>上次遗漏</em></th><th style="width:150px;">' + this.createMaxMissMenu(s) + '<em>&nbsp;</em></th>{0}<th width="35"><input type="checkbox" checked="checked" /></th></tr></thead></table></div>', (k ? "<th><em>20期冷热</em></th>" : "")))
                }
                else {
                    l.push(String.format('<div class="SecN_th"><table class="Sectable"><thead><tr><th class="sw1"><em>号码</em></th><th class="sw2"><em>当前遗漏</em></th><th class="sw3"><em>上次遗漏</em></th><th style="width:150px;">' + this.createMaxMissMenu(s) + '<em>&nbsp;</em></th>{0}<th width="35"><input type="checkbox" checked="checked" /></th></tr></thead></table></div>', (k ? '<th class="sw4"><em>20期冷热</em></th>' : "")))
                }
                l.push('<div class="SecN_td"><table class="Sectable"><tbody>');
                l.push(m.join(""));
                l.push("</tbody></table></div>");
                var t = $(l.join("")).appendTo($(b.DomBox).html(""));
                var r = $(t).find("tbody input[type=checkbox]").each(function () {
                    $(this).click(a.searchCompute.bind(a, b))
                });
                $(t).find("thead input[type=checkbox]").click(function () {
                    var i = this;
                    $(r).each(function () {
                        this.checked = i.checked
                    });
                    a.searchCompute(b)
                });
                $(t).find("thead em").each(function (i) {
                    if (typeof (b.Sort) == "undefined") {
                        b.Sort = {}
                    }
                    var v = (b.Sort[i] || 1);
                    if (v > 0) {
                        $(this).addClass("sup")
                    }
                    else {
                        $(this).addClass("sub")
                    }
                    $(this).click(function () {
                        b.Sort[i] = -1 * v;
                        a.searchDeal(b, c, i)
                    });
                    if ($(this).prevAll("select").length > 0) {
                        $(this).prevAll("select").change(function () {
                            s.CurShowMaxMissArea = parseInt($(this).val(), 10);
                            a.ObjectData[b.TypeValue] = s;
                            a.searchDeal(b, c, i)
                        })
                    }

                });
                $(t).filter("div.SecN_td").css("height", 220).css("overflow", "auto");
                break
        }
        this.searchCompute(b)
    }
    , searchResponse: function (a, c, b) {
        var d = new _SecretaryData(b, a);
        this.ObjectData[d.DataType] = d;
        this.searchDeal(a, c)
    }
    , searchRequest: function (a, b) {
        if ($S.Debug.IntelliSense) {
            a = new ___SecretaryType()
        }
        if (typeof (this.ObjectData[a.TypeValue]) != "undefined") {
            this.searchDeal(a, b);
            return
        }
        $(a.DomBox).html('<div class="loading"></div>');
        var c = new $S.JsonCommand();
        c.Command = "search";
        c.ListParameter.push(TK.CurrentLotteryType);
        c.ListParameter.push(a.TypeValue);
        $.ajax({
            type: "post", url: this.SubmitUrl, data: {
                d: c.toJSONString()
            }
            , success: this.searchResponse.bind(this, a, b), error: function (d) {
                var f = $('<div class="dia_c_main"><div class="cet"><b class="plaint">&nbsp;</b><span>' + d.statusText + '</span></div><div class="cetBtn"><a class="Dbutt"><span>确　定</span></a></div></div>').find("a.Dbutt").click(function () {
                    $.artDialog.get(this.DomID).close()
                }).end();
                $.artDialog.get(this.DomID).title("系统异常").content(f.get(0))
            }
            .bind(this)
        })
    }
    , createChildTypeBox: function (b) {
        if ($S.Debug.IntelliSense) {
            b = new ___SecretaryType()
        }
        var c = [];
        c.push("<div>");
        c.push('<div class="SecNumCon">');
        c.push('  <div class="SecNbox">');
        switch (b.Kind) {
            case 2: c.push('      <div class="SecNtop hmd">');
                c.push("<ul>");
                for (var d = 0;
                d < b.ListData.length;
                d++) {
                    c.push(String.format("<li>{0}</li>", b.ListData[d]))
                }
                c.push(String.format(""));
                c.push("</ul>");
                c.push("      </div>");
                c.push('      <div class="SecNbtm btmhmd"></div>');
                break;
            default: c.push('      <div class="SecNtop ' + (b.Class || "") + '">');
                c.push(String.format("<em>{0}</em><span>", b.TypeName));
                for (var d = 0;
                d < b.ListData.length;
                d++) {
                    c.push(String.format("<i>{0}</i>", b.ListData[d]))
                }
                c.push('      </span><a href="javascript:void(0)" class="btn_sel">查询</a>');
                c.push("      </div>");
                c.push('      <div class="SecNbtm"></div>');
                break
        }
        c.push("  </div>");
        c.push("</div>");
        c.push('<div class="chartCheck"><span class="sum">共<i>0</i>注，共<em class="f_highlight">0</em>元</span><span class="suml"><a href="javascript:void(0)" class="btnChartCheck">添加号码</a></span></div>');
        c.push("</div>");
        var a = this;
        var f = $(c.join("")).appendTo($(this.CurrentPlayTypeMenu).prop("config").Wrap);
        b.DomBox = $(f).find("div[class*=SecNbtm]");
        $(f).find("a.btnChartCheck").click(this.searchAddNumber.bind(this, b));
        switch (b.Kind) {
            case 2: f.find("div.SecNtop li").each(function () {
                $(this).click(function () {
                    $(this).addClass("on").siblings().removeClass("on");
                    a.searchRequest(b, $(this).text())
                })
            });
                if ($(b.DomBox).children().length == 0) {
                    setTimeout(function () {
                        f.find("div.SecNtop li").eq(0).click()
                    }
                    , 10)
                }
                break;
            default: var e = f.find("div.SecNtop i").each(function () {
                $(this).click(function () {
                    $(this).toggleClass("on")
                })
            });
                $(f).find("a.btn_sel").click(function () {
                    var g = [];
                    $(e).filter(".on").each(function () {
                        g.push($(this).text())
                    });
                    a.searchRequest(b, g)
                });
                if ($(b.DomBox).children().length == 0) {
                    setTimeout(function () {
                        $(f).find("a.btn_sel").click()
                    }
                    , 10)
                }
                break
        }
        return f
    }
    , childTypeChanged: function (a) {
        if (this.CurrentChildTypeRadio != null) {
            var b = $(this.CurrentChildTypeRadio).prop("box");
            if ($S.Debug.IntelliSense) {
                b = new ___SecretaryType()
            }
            $(b.Wrap).hide()
        }
        this.CurrentChildTypeRadio = a;
        var b = $(this.CurrentChildTypeRadio).prop("box");
        if ($S.Debug.IntelliSense) {
            b = new ___SecretaryType()
        }
        if (typeof (b.Wrap) == "undefined") {
            b.Wrap = this.createChildTypeBox(b)
        }
        else {
            switch (b.Kind) {
                case 2: $(b.Wrap).find("div.SecNtop li").eq(0).click();
                    break;
                default: $(b.Wrap).find("a.btn_sel").click();
                    break
            }

        }
        $(b.Wrap).show();
        var c = $(this.CurrentPlayTypeMenu).prop("config");
        if ($S.Debug.IntelliSense) {
            c = new __SecretaryConfig()
        }
        c.SelectedChildTypeRadio = $(this.CurrentChildTypeRadio);
        $(this.CurrentPlayTypeMenu).prop("config", c);
        $(this.CurrentChildTypeRadio).prop("box", b)
    }
    , createSecTabBox: function (b) {
        if ($S.Debug.IntelliSense) {
            b = new __SecretaryConfig()
        }
        var c = [];
        if (b.PlayTypeValue == 0) {
            c.push('<div class="SecTabBox"></div>');
            var f = $(c.join("")).appendTo(this.DomSecCon)
        }
        else {
            c.push('<div class="SecTabBox">');
            c.push('<div class="SecNumStyle">');
            for (var d = 0;
            d < b.ListChildType.length;
            d++) {
                var e = b.ListChildType[d];
                if ($S.Debug.IntelliSense) {
                    e = new ___SecretaryType()
                }
                c.push(String.format('<span><label><input type="radio" name="secre_{1}" />{0}</label></span>', e.TypeName, b.PlayTypeValue))
            }
            c.push("</div>");
            c.push("</div>");
            var f = $(c.join("")).appendTo(this.DomSecCon), a = this;
            b.DomListMenu = f.find("input[type=radio]").each(function (g) {
                $(this).prop("box", b.ListChildType[g]);
                $(this).click(a.childTypeChanged.bind(a, this))
            })
        }
        return f
    }
    , menuChanged: function (a) {
        if (this.CurrentPlayTypeMenu != null) {
            $(this.CurrentPlayTypeMenu).removeClass("active");
            var b = $(this.CurrentPlayTypeMenu).prop("config");
            if ($S.Debug.IntelliSense) {
                b = new __SecretaryConfig()
            }
            $(b.Wrap).hide()
        }
        this.CurrentPlayTypeMenu = a;
        $(this.CurrentPlayTypeMenu).addClass("active");
        var b = $(this.CurrentPlayTypeMenu).prop("config");
        if ($S.Debug.IntelliSense) {
            b = new __SecretaryConfig()
        }
        if (typeof (b.Wrap) == "undefined") {
            b.Wrap = this.createSecTabBox(b)
        }
        $(b.Wrap).show();
        if (b.PlayTypeValue == 0) {
            this.requestMyBetData(b.Wrap)
        }
        else {
            if (b.SelectedChildTypeRadio == null) {
                b.SelectedChildTypeRadio = $(b.DomListMenu[0])
            }
            this.childTypeChanged($(b.SelectedChildTypeRadio).attr("checked", true))
        }
        $(this.CurrentPlayTypeMenu).prop("config", b)
    }
    , initDom: function () {
        if (this.DomWrap != null) {
            return
        }
        var b = [], a = this;
        b.push('<div class="SecretaryWrap">');
        b.push('<div class="SecTab"><ul>');
        for (var c = 0;
        c < this.ListMenuData.length;
        c++) {
            var d = this.ListMenuData[c];
            if ($S.Debug.IntelliSense) {
                d = new __SecretaryConfig()
            }
            b.push(String.format("<li><i>{0}</i></li>", d.PlayTypeName))
        }
        b.push("</ul></div>");
        b.push('<div class="SecCon"></div>');
        b.push("</div>");
        this.DomWrap = $(b.join(""));
        this.DomSecCon = $(this.DomWrap).find("div.SecCon");
        this.DomListMenu = this.DomWrap.find("div.SecTab li").each(function (e) {
            $(this).prop("config", a.ListMenuData[e]);
            $(this).click(a.menuChanged.bind(a, this))
        })
    }
    , initialize: function () {
        this.DomWrap = null;
        this.initDom();
        var a = this;
        $(TK.Command).bind("RetBetMiss", function () {
            a.ObjectData = {}
        });
        return this
    }

};
var kkSecretary = new TK.Bet.Secretary();
kkSecretary.ListMenuData = [{
    PlayTypeName: "一星", PlayTypeValue: 102, ListChildType: [{
        TypeName: "二码遗漏", TypeValue: 560, PlayTypeValue: 102, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 2, filter: function (a, b) {
            switch (a.length) {
                case 1: return b.contains(a[0]);
                    break;
                case 2: return b == a.join("");
                    break;
                default: var c = $S.Math.listC(a, 2);
                    for (var d = 0;
                    d < c.length;
                    d++) {
                        if (b == c[d].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }
        , createNumber: function (a) {
            return a.split("").join()
        }

    }
    ]
}
, {
    PlayTypeName: "二星直选", PlayTypeValue: 201, ListChildType: [{
        TypeName: "跨度", TypeValue: 60, PlayTypeValue: 201, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, convertType: function (a) {
            return Math.abs(a.split("|").math(["-"]))
        }
        , filter: function (a, b) {
            var d = Math.abs(b.split("|").math(["-"])).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "和值", Class: "hz", TypeValue: 60, PlayTypeValue: 201, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18], BetCount: 1, convertType: function (a) {
            return Math.abs(a.split("|").math(["+"]))
        }
        , filter: function (a, b) {
            var d = Math.abs(b.split("|").math(["+"])).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "号码段", TypeValue: 60, PlayTypeValue: 201, Kind: 2, ListData: ["按遗漏显示", "按号码显示"], BetCount: 1
    }
    , {
        TypeName: "单式遗漏", TypeValue: 60, PlayTypeValue: 201, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, filter: function (a, b) {
            a.sort();
            var d = b.split("|");
            d.sort();
            switch (a.length) {
                case 1: return d.exists(a[0]);
                    break;
                case 2: return d.join("") == a.join("");
                    break;
                default: var c = $S.Math.listC(a, 2);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    ]
}
, {
    PlayTypeName: "三星直选", PlayTypeValue: 301, ListChildType: [{
        TypeName: "跨度", TypeValue: 170, PlayTypeValue: 301, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, convertType: function (a) {
            var b = a.split("|");
            return (parseInt(b.getMax(), 10) - parseInt(b.getMin(), 10))
        }
        , filter: function (a, b) {
            var e = b.split("|");
            var d = (parseInt(e.getMax(), 10) - parseInt(e.getMin(), 10)).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "和值", Class: "hz", PlayTypeValue: 301, TypeValue: 170, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27], BetCount: 1, convertType: function (a) {
            return Math.abs(a.split("|").math(["+"]))
        }
        , filter: function (a, b) {
            var d = Math.abs(b.split("|").math(["+"])).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "号码段", TypeValue: 170, PlayTypeValue: 301, Kind: 2, ListData: ["按遗漏显示", "按号码显示"], BetCount: 1
    }
    , {
        TypeName: "单式遗漏", TypeValue: 170, PlayTypeValue: 301, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, filter: function (a, b) {
            a.sort();
            var d = b.split("|");
            d.sort();
            switch (a.length) {
                case 1: case 2: for (var e = 0;
                e < a.length;
                e++) {
                    if (!d.exists(a[e])) {
                        return false
                    }

                }
                    return true;
                    break;
                default: var c = $S.Math.listC(a, 3);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    ]
}
, {
    PlayTypeName: "二星组选", PlayTypeValue: 204, ListChildType: [{
        TypeName: "跨度", TypeValue: 70, PlayTypeValue: 204, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, convertType: function (a) {
            var b = a.split(",");
            return (parseInt(b.getMax(), 10) - parseInt(b.getMin(), 10))
        }
        , filter: function (a, b) {
            var e = b.split(",");
            var d = (parseInt(e.getMax(), 10) - parseInt(e.getMin(), 10)).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "和值", Class: "hz", TypeValue: 70, PlayTypeValue: 204, ListData: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17], BetCount: 1, convertType: function (a) {
            return Math.abs(a.split(",").math(["+"]))
        }
        , filter: function (a, b) {
            var d = Math.abs(b.split(",").math(["+"])).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "号码段", TypeValue: 70, PlayTypeValue: 204, Kind: 2, ListData: ["按遗漏显示", "按号码显示"], BetCount: 1
    }
    , {
        TypeName: "单式遗漏", TypeValue: 70, PlayTypeValue: 204, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, filter: function (a, b) {
            a.sort();
            var d = b.split(",");
            d.sort();
            switch (a.length) {
                case 1: return d.exists(a[0]);
                    break;
                case 2: return d.join("") == a.join("");
                    break;
                default: var c = $S.Math.listC(a, 2);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    ]
}
, {
    PlayTypeName: "组选三", PlayTypeValue: 306, ListChildType: [{
        TypeName: "跨度", TypeValue: 181, PlayTypeValue: 306, ListData: [1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, convertType: function (a) {
            var b = a.split(",");
            return (parseInt(b.getMax(), 10) - parseInt(b.getMin(), 10))
        }
        , filter: function (a, b) {
            var e = b.split(",");
            var d = (parseInt(e.getMax(), 10) - parseInt(e.getMin(), 10)).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "和值", Class: "hz", TypeValue: 181, PlayTypeValue: 306, ListData: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26], BetCount: 1, convertType: function (a) {
            return Math.abs(a.split(",").math(["+"]))
        }
        , filter: function (a, b) {
            var d = Math.abs(b.split(",").math(["+"])).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "号码段", TypeValue: 181, PlayTypeValue: 306, Kind: 2, ListData: ["按遗漏显示", "按号码显示"], BetCount: 1
    }
    , {
        TypeName: "二码遗漏", PlayTypeValue: 307, TypeValue: 520, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 2, filter: function (a, b) {
            a.sort();
            var d = b.split(",");
            d.sort();
            switch (a.length) {
                case 1: for (var e = 0;
                e < a.length;
                e++) {
                    if (!d.exists(a[e])) {
                        return false
                    }

                }
                    return true;
                    break;
                default: var c = $S.Math.listC(a, 2);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    , {
        TypeName: "三码遗漏", TypeValue: 521, PlayTypeValue: 307, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 6, filter: function (a, b) {
            a.sort();
            var d = b.split(",");
            d.sort();
            switch (a.length) {
                case 1: case 2: for (var e = 0;
                e < a.length;
                e++) {
                    if (!d.exists(a[e])) {
                        return false
                    }

                }
                    return true;
                    break;
                default: var c = $S.Math.listC(a, 3);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    , {
        TypeName: "四码遗漏", TypeValue: 522, PlayTypeValue: 307, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 12, filter: function (a, b) {
            a.sort();
            var d = b.split(",");
            d.sort();
            switch (a.length) {
                case 1: case 2: case 3: for (var e = 0;
                e < a.length;
                e++) {
                    if (!d.exists(a[e])) {
                        return false
                    }

                }
                    return true;
                    break;
                default: var c = $S.Math.listC(a, 4);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    ]
}
, {
    PlayTypeName: "组选六", PlayTypeValue: 310, ListChildType: [{
        TypeName: "跨度", TypeValue: 182, PlayTypeValue: 310, ListData: [2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, convertType: function (a) {
            var b = a.split(",");
            return (parseInt(b.getMax(), 10) - parseInt(b.getMin(), 10))
        }
        , filter: function (a, b) {
            var e = b.split(",");
            var d = (parseInt(e.getMax(), 10) - parseInt(e.getMin(), 10)).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "和值", Class: "hz", TypeValue: 182, PlayTypeValue: 310, ListData: [3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24], BetCount: 1, convertType: function (a) {
            return Math.abs(a.split(",").math(["+"]))
        }
        , filter: function (a, b) {
            var d = Math.abs(b.split(",").math(["+"])).toString();
            for (var c = 0;
            c < a.length;
            c++) {
                if (a.exists(d)) {
                    return true
                }

            }
            return false
        }

    }
    , {
        TypeName: "号码段", TypeValue: 182, PlayTypeValue: 310, Kind: 2, ListData: ["按遗漏显示", "按号码显示"], BetCount: 1
    }
    , {
        TypeName: "单式遗漏", TypeValue: 182, PlayTypeValue: 310, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 1, filter: function (a, b) {
            a.sort();
            var d = b.split(",");
            d.sort();
            switch (a.length) {
                case 1: case 2: for (var e = 0;
                e < a.length;
                e++) {
                    if (!d.exists(a[e])) {
                        return false
                    }

                }
                    return true;
                    break;
                default: var c = $S.Math.listC(a, 3);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    , {
        TypeName: "四码遗漏", TypeValue: 183, PlayTypeValue: 311, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 4, filter: function (a, b) {
            a.sort();
            var d = b.split(",");
            d.sort();
            switch (a.length) {
                case 1: case 2: case 3: for (var e = 0;
                e < a.length;
                e++) {
                    if (!d.exists(a[e])) {
                        return false
                    }

                }
                    return true;
                    break;
                default: var c = $S.Math.listC(a, 4);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    , {
        TypeName: "五码遗漏", TypeValue: 184, PlayTypeValue: 311, ListData: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], BetCount: 10, filter: function (a, b) {
            a.sort();
            var d = b.split(",");
            d.sort();
            switch (a.length) {
                case 1: case 2: case 3: case 4: for (var e = 0;
                e < a.length;
                e++) {
                    if (!d.exists(a[e])) {
                        return false
                    }

                }
                    return true;
                    break;
                default: var c = $S.Math.listC(a, 5);
                    for (var e = 0;
                    e < c.length;
                    e++) {
                        if (d.join("") == c[e].join("")) {
                            return true
                        }

                    }
                    break
            }
            return false
        }

    }
    ]
}
, {
    PlayTypeName: "我投注的", PlayTypeValue: 0
}
];
kkSecretary.initialize();
