// jqxwidgets extension
(function ($) {
    console.log_call = function () {
        return console.log.apply(this, arguments)
    };

    var _uniqueid = 0;

    $.fn.addClass_jqx = function (e) { return this.addClass(e + ' ' + e + '-' + $.jqx.theme); }

    $.fn.removeClass_jqx = function (e) { return this.removeClass(e + ' ' + e + '-' + $.jqx.theme); }

    if (!$.jqx) return;

    $.fn.jqxButton2 = function () {
        var icon = this.attr('icon');
        var $ret = this.jqxButton(arguments);
        if (icon) {
            this.addClass('button-icon-text');
            this.wrapInner('<span class="button-text"></span>');
            var $icon = $('<span class="button-icon" style="' + this.attr('icon-style') + '"></span>').addClass_jqx('jqx-icon-' + icon);
            $icon.insertBefore(this.children().first());
        }
        this.removeAttr('icon icon-style');
        return $ret;
    };

    $.fn.jqxMenu2 = function (o) {
        this.jqxMenu(o);
        this.removeClass_jqx('jqx-widget-header');//.addClass_jqx('jqx-widget-content');
        return this;
    };

    $.replace_function($.jqx, 'jqxWidget', function (b, d, j) {
        arguments.callee._original.apply(this, arguments);
        a.jqx["_" + g].prototype.getInstance = function () { return this; };
    });

})(jqxBaseFramework);
