<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="web.comet" %>

<html>
<head>
    <%--<script type="text/javascript">
        $(document).ready(function () {

            var xhr;

            $('#test').click(function () {
                //$.when(
                //    $.ajax({
                //        type: 'post',
                //        url: '.api',
                //        dataType: 'json',
                //        cache: false,
                //        async: true,
                //        data: { str: JSON.stringify({ notify: {} }) }
                //    }
                //)).then(function () {
                //    console.log(arguments);
                //});
                xhr = $.ajax({
                    type: 'post',
                    url: '.api',
                    dataType: 'json',
                    cache: false,
                    async: true,
                    timeout: 0,
                    data: { str: JSON.stringify({ notify: {} }) },
                    success: function (data, textStatus, jqXHR) {
                        console.log("success", arguments);
                        xhr = null;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log("error", arguments);
                        xhr = null;
                    }
                });
            });
            $('#cancel').click(function () {
                if (xhr != null)
                    xhr.abort();
            });
        });
    </script>--%>
</head>
<body>
    <button id="test">test</button>
    <button id="cancel">cancel</button></body>
</html>
