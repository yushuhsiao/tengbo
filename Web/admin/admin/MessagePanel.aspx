<%@ Page Title="" Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" Inherits="web.page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var recvMessage = {
            dbgmsg: function (data) {
                var n = $msg[0].rows.length;
                $msg.addRowData(n, { id: n, time: Date.now(), msg: data.msg, args: data.args, code: data.code });
                $msg.setSelection(n);
                var ind = $msg.getInd(n, true);
                $(ind).addClass('ui-state-' + data.state);
            }
        }

        var $msg;
        $(document).ready(function () {
            $msg = $('#msg').jqGrid({
                datatype: 'local', autowidth: true, shrinkToFit: true, hoverrows: false, scrollrows: true,
                colModel: [
                    { name: 'id', label: '.', width: 35, fixed: true, align: 'center' },
                    { name: 'time', label: 'time', width: 120, fixed: true, align: 'center', formatter: 'datejs', formatoptions: { format: 'yyyy-MM-dd HH:mm:ss', formatNaN: 'N/A' } },
                    { name: 'msg', label: 'msg', width: "auto" },
                    { name: 'args', label: 'args', width: "auto" },
                    { name: 'code', label: 'code', width: "auto" }
                ]
            });
            try { $($msg[0].grid.hDiv).hide(); } catch (e) { }
            $(window).resize(function () { $msg.gridSize(window); }).trigger('resize');
        });
    </script>
    <style type="text/css">
        #msg td { font-family: Arial, sans-serif; font-size: xx-small; height: auto; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <table id="msg"></table>
</asp:Content>
