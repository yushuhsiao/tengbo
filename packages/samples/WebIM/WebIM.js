/// <reference path="jquery-1.3.2.min.js" >
$(document).ready(function () {
    //状态，代表是否登录
    var _logined = false;

    //登录，登录成功后，获取在线用户列表，
    function login() {
        $.post("comet_broadcast.asyn", { action: 'login', uid: $("#txtLoginID").val() },
        function (data, status) {
            if (data == "OK") {
                _logined = true;
                getuserlist();
            }
        });
    }

    //获取在线用户列表，获取列表后，进入消息等待
    function getuserlist() {
        $.post("comet_broadcast.asyn", { action: 'getuserlist', uid: $("#txtLoginID").val() },
        function (data, status) {
            //alert('getuserlist' + data);
            var result = $("#divResult");
            result.html(result.html() + "<br/>" + "用户列表：" + data);
            wait();
        });
    }

    //退出
    function logout() {
        $.post("comet_broadcast.asyn", { action: 'logout', uid: $("#txtLoginID").val() },
        function (data, status) { _logined = false; alert(data); }
         );
    }

    //消息等待，接收到消息后显示，发起下一次的消息等待
    function wait() {
        $.post("comet_broadcast.asyn", { action: 'connect', uid: $("#txtLoginID").val() },
           function (data, status) {
               // alert('wait' + data);
               var result = $("#divResult");
               result.html(result.html() + "<br/>" + "用户列表:" + data);

               //服务器返回消息,再次立连接
               if (_logined) {
                   wait();
               }

           }, "html"
           );
    }
    //附加事件
    $("#btnLogin").click(function () { if ($("#txtLoginID").val() == '') alert('空'); login(); });
    $("#btnLogout").click(function () { logout(); });

});
