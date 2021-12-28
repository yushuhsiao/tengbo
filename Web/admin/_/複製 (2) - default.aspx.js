var auto_size2 = (function () {
    function auto_size2(logo, menu, main) {
        this.logo = $(logo);
        this.menu = $(menu);
        this.main = $(main);
        $(document).resize(this.resize);
        $(window).resize(this.resize);
    }
    auto_size2.prototype.resize = function (eventObject) {
        return null;
    };
    return auto_size2;
})();
$(document).ready(function () {
    var xx = new auto_size('#logo', '#menu', '#main');
});
function auto_size(logo, menu, main) {
    var obj = {
        logo: $(logo),
        menu: $(menu),
        main: $(main),
        root: $(document)
    };
    var resize = function (eventObject) {
        obj.main.css({
            top: obj.menu.height(),
            height: (obj.root.height() - obj.menu.height() - 5) + 'px'
        });
        return null;
    };
    $(document).click(this.resize);
    $(document).click(this.resize);
}
