/// <reference path="scripts/jquery.d.ts" />

class auto_size2 {
    logo: JQuery;
    menu: JQuery;
    main: JQuery;
    constructor(logo: string, menu: string, main: string) {
        this.logo = $(logo);
        this.menu = $(menu);
        this.main = $(main);
        $(document).resize(this.resize)
        $(window).resize(this.resize)
    }

    private resize(eventObject: JQueryEventObject): JQuery {
        return null;
    }
}

$(document).ready(function () {
    var xx = new auto_size('#logo', '#menu', '#main');
});


function auto_size(logo, menu, main) {
    var obj = {
        logo: $(logo),
        menu: $(menu),
        main: $(main),
        root: $(document)
    }
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
