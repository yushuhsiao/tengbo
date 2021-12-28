; (function ($) {
    langs = {
        en: {
            CreateTime: 'CreateTime', CreateUser: 'CreateUser', 
            CreateTime: 'ModifyTime', CreateUser: 'ModifyUser', 
        },
        tw: {
        },
        cn: {
        }
    }
    $.lang = {};
    (function setlang(name) { $.extend($.lang, langs[name]); })('en')
})(jQuery);
