$.fn._autodock = function (obj) {
    obj.logo = $(obj.logo);
    obj.menu = $(obj.menu);
    obj.root = $(document);
    obj.main = this;
    var resize = (function () {
        obj.main.css({
            top: obj.menu.height(),
            height: (obj.root.height() - obj.menu.height() - 5) + 'px'
        });
    })();
    $(window).resize(resize);
    $(document).resize(resize);
    return this;
};

$.fn.init_sub_menu = function (menu_node) {
    for (var i = 0; i < menu_node.c.length; i++) {
        var node = menu_node.c[i];
        var li = $(document.createElement('li')).appendTo(this).text(node.a).attr('op', node.b);
        if (node.c != null)
            $(document.createElement('ul')).appendTo(li).init_sub_menu(node);
    }
    return this;
};

$.fn.init_menu = function (menu) {

    if (menu.Node == null) return;
    var menu0 = this.find('.top-nav');
    var tmp1 = this.find('label.navbar').removeClass('template');
    var tmp2 = this.find('table.navbar').removeClass('template');
    for (var i = 0; i < menu.Node.c.length; i++) {
        var _class_id = 'nav' + i;
        var node = menu.Node.c[i];
        tmp1.clone().appendTo(tmp1.parent()).text(node.b).attr('for', _class_id);
        tmp2.clone().insertBefore(this.find('.userinfo')).addClass(_class_id).find('ul.menu').init_sub_menu(node);
    }
    tmp1.remove();
    tmp2.remove();
    this.find('ul.menu').superfish();
    this.find('table.navbar').hide();
    this.find('label.navbar').hover(function () {
        var _for = $(this).attr('for');
        $('#menu table.navbar').each(function () {
            var table = $(this);
            if (table.hasClass(_for))
                table.fadeIn('fast', 'linear');
            else
                table.fadeOut(0, 'linear');
        });
    }, function () {
    });
};

var doc = (function () {
    function doc() { }
    doc.logout = function logout() {
        util.execute({
            data: {
                AdminLogout: {
                }
            },
            success: function (obj) {
                if (obj.LoginResult.t1 == 1) {
                    window.location.reload();
                }
            }
        });
    };
    return doc;
})();
