<%@ Page Title="" Language="C#" AutoEventWireup="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <button id="test">test</button>
    <button id="cancel">cancel</button>
</asp:Content>
