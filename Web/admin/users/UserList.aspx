<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="web.page" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    }
</script>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#frame_tree').load(function () {
                $('#frame_user').prop('src', 'Member.aspx');
                $('#btn_tree').show();
            }).prop('src', 'AgentTree.aspx');
            var show_tree = true;

            function resize() {
                var ww = $(window).innerWidth();
                var hh = $(window).innerHeight() - 2;
                var w;
                if (show_tree) {
                    w = $('#div_tree').outerWidth();
                    $('#div_tree').show();
                }
                else {
                    w = 0;
                    $('#div_tree').hide();
                }
                $('#div_tree').css({ left: 0, top: 0, height: hh });
                $('#div_user').css({ left: w, top: 0, height: hh, width: ww - w });
            };

            $('#btn_tree').button({
                icons: { primary: 'ui-icon-transferthick-e-w' },
                text: false
            }).click(function () {
                show_tree = !show_tree;
                resize();
            }).hide();

            $('#div_tree').resizable({
                handles: 'e', resize: resize,
                start: function () { $('.mask').show(); },
                stop: function () { $('.mask').hide(); }
            });

            $(window).resize(resize).trigger('resize');
        });
    </script>
    <style type="text/css">
        .mask { position: absolute; display: none; left: 0; top: 0; width: 110%; height: 110%; opacity: 0.1; }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <div id="div_user" style="position: absolute; ">
        <iframe id="frame_user" style="border: 0; display: block; width: 100%; height: 100%;"></iframe>
        <div class="mask ui-state-highlight"></div>
    </div>
    <div id="div_tree" style="position: absolute; width: 200px; ">
        <iframe id="frame_tree" style="border: 0; display: block; width: 100%; height: 100%;"></iframe>
        <div class="mask ui-state-highlight"></div>
    </div>
    <button id="btn_tree" style="position: absolute; top:1px; left: 1px; width: 1em; margin: 0; border: 0;"></button>
</asp:Content>
