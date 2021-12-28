$.fn.sliderShow = function () {
    var $top = this;
    var $ul = this.find('ul');

    var $nav = $('<ul></ul>').appendTo($top).addClass('slider_nav');
    $ul.find('li').each(function () {
        $('img', this).hide();
        $('<li></li>').appendTo($nav);
    });

    var t1 = null;

    function slide_img(index1, index2, direction1, direction2) {
        $ul.find('li').eq(index1).find('img').hide({ effect: "slide", direction: direction1, duration: 250 });
        $ul.find('li').eq(index2).find('img').show({ effect: "slide", direction: direction2, duration: 250 });
    }
    function fade_img(index1, index2) {
        $ul.find('li').eq(index1).find('img').hide({ effect: "fade", duration: 250 });
        $ul.find('li').eq(index2).find('img').show({ effect: "fade", duration: 250 });
    }
    function clip_explode(index1, index2) {
        $ul.find('li').eq(index1).find('img').hide({ effect: "explode", duration: 800, pieces: 36 });
        $ul.find('li').eq(index2).find('img').show({ effect: "fade", duration: 400 });
        //$('#s_show li').eq(index2).find('img').show({ effect: "explode", duration: 1000, pieces: 24 });
    }
    function clip_img(index1, index2, direction) {
        $ul.find('li').eq(index1).find('img').hide({
            effect: "clip", direction: direction, duration: 250,
            complete: function () {
                $ul.find('li').eq(index2).find('img').show({ effect: "clip", direction: direction, duration: 250 });
            }
        });
    }

    function show_img(index2) {
        clearTimeout(t1);
        t1 = null;
        var index1 = $nav.find('li.active').index();
        if (index1 == index2)
            return;
        var n = $ul.find('li > img');
        if (n.length == 0)
            return;
        if (index2 == undefined)
            index2 = (index1 + 1) % n.length;
        n.stop(true, true);
        $nav.find('li').eq(index1).removeClass('active');
        $nav.find('li').eq(index2).addClass('active');
        switch (parseInt(Math.random() * 10)) {
            case 1: slide_img(index1, index2, "up", "down"); break;
            case 2: slide_img(index1, index2, "down", "up"); break;
            case 3: slide_img(index1, index2, "left", "right"); break;
            case 4: slide_img(index1, index2, "right", "left"); break;
            case 5: clip_img(index1, index2, "up"); break;
            case 6: clip_img(index1, index2, "down"); break;
            case 7: clip_img(index1, index2, "left"); break;
            case 8: clip_img(index1, index2, "right"); break;
            case 9: clip_explode(index1, index2, "right"); break;
            default: fade_img(index1, index2); break;
        }
        t1 = setTimeout(show_img, 3000);
    }

    $nav.find('li').click(function () { show_img($(this).index()); });

    show_img(0);
}

$.fn.hoverSilder = function (s1, s2, speed) {
    this.hover(
        function () {
            $(s1, this).stop(true, true).show({ effect: "slide", direction: 'right', duration: speed });
            $(s2, this).stop(true, true).show({ effect: "slide", direction: 'down', duration: speed });
        },
        function () {
            $(s1, this).stop(true, true).hide({ effect: "slide", direction: 'right', duration: speed });
            $(s2, this).stop(true, true).hide({ effect: "slide", direction: 'down', duration: speed });
        });
}

function addBookmark(url, title) {
    if (document.all) {
        try { window.external.addFavorite(window.location.href, document.title); }
        catch (e) { alert("加入收藏失败，请使用Ctrl+D进行添加"); }
    } else if (window.sidebar) {
        window.sidebar.addPanel(document.title, window.location.href, "");
    } else {
        alert("加入收藏失败，请使用Ctrl+D进行添加");
    }
}

// String.format.js
(function () {
    //可在Javascript中使用如同C#中的string.format
    //使用方式 : var fullName = String.format('Hello. My name is {0} {1}.', 'FirstName', 'LastName');
    String.format = function () {
        var s = arguments[0];
        if (s == null) return "";
        for (var i = 0; i < arguments.length - 1; i++) {
            var reg = getStringFormatPlaceHolderRegEx(i);
            s = s.replace(reg, (arguments[i + 1] == null ? "" : arguments[i + 1]));
        }
        return cleanStringFormatResult(s);
    }
    //可在Javascript中使用如同C#中的string.format (對jQuery String的擴充方法)
    //使用方式 : var fullName = 'Hello. My name is {0} {1}.'.format('FirstName', 'LastName');
    String.prototype.format = function () {
        var txt = this.toString();
        for (var i = 0; i < arguments.length; i++) {
            var exp = getStringFormatPlaceHolderRegEx(i);
            txt = txt.replace(exp, (arguments[i] == null ? "" : arguments[i]));
        }
        return cleanStringFormatResult(txt);
    }
    //讓輸入的字串可以包含{}
    function getStringFormatPlaceHolderRegEx(placeHolderIndex) {
        return new RegExp('({)?\\{' + placeHolderIndex + '\\}(?!})', 'gm')
    }
    //當format格式有多餘的position時，就不會將多餘的position輸出
    //ex:
    // var fullName = 'Hello. My name is {0} {1} {2}.'.format('firstName', 'lastName');
    // 輸出的 fullName 為 'firstName lastName', 而不會是 'firstName lastName {2}'
    function cleanStringFormatResult(txt) {
        if (txt == null) return "";
        return txt.replace(getStringFormatPlaceHolderRegEx("\\d+"), "");
    }

    String.prototype.padL = function (width, pad) {
        if (!width || width < 1)
            return this;

        if (!pad) pad = " ";

        var length = width - this.length

        if (length < 1)
            return this.substr(0, width);

        return (String.repeat(pad, length) + this).substr(0, width);
    }
    String.prototype.padR = function (width, pad) {
        if (!width || width < 1)
            return this;

        if (!pad) pad = " ";

        var length = width - this.length

        if (length < 1) this.substr(0, width);
        return (this + String.repeat(pad, length)).substr(0, width);
    }
    String.repeat = function (chr, count) {
        var str = "";
        for (var x = 0; x < count; x++) {
            str += chr
        };
        return str;
    }
})();


$.fn.getPostData = function (reset) {
    var postData = {};
    $('input[type="text"], input[type="password"], input[type="radio"]:checked, select, textarea', this).each(function () {
        var name = $(this).prop('name') || '';
        if (name.length > 0) {
            postData[name] = $.trim($(this).val()) || '';
            if (reset == true)
                $(this).val('');
        }
    });
    return postData
}

$.fn.load2 = function (url, params, data, callback) {
    if (data) {
        if (params) {
            params.data = JSON.stringify(data);
        }
    }
    return this.load(url, params, callback);
}
