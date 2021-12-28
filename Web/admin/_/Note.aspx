<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/admin.master" AutoEventWireup="true" culture="auto" uiculture="auto" CodeBehind="Note.aspx.cs" Inherits="page" meta:resourcekey="PageResource1" %>

<script runat="server">
    public BU.NoteTypes type;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.type = ((BU.NoteTypes?)Request.QueryString["t"].ToByte()) ?? BU.NoteTypes.Events;
    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        /*.ui-jqgrid .ui-userdata { height : auto;	}*/
    </style>
    <script type="text/javascript">
        var $table;

        var type = 0<%=(byte)this.type%>;

        $(document).ready(function () {

            $table = $('#table01').jqGrid_init({
                pager: true, toppager: false, loadonce: true, subGrid: false, shrinkToFit: true,
                nav1: '#nav1', nav2: '#nav2', 
                SelectCommand: function (postData) { postData.NoteType = type; return { NoteSelect: postData } },
                UpdateCommand: function (postData) { postData.NoteType = postData.NoteType || type; return { NoteUpdate: postData } },
                InsertCommand: function (postData) { postData.NoteType = postData.NoteType || type; return { NoteInsert: postData } },
                useDefValues: true,

                colModel: [
                    { name: 'Action    ', label: '<%=lang["colAction    "]%>', colType: 'Buttons' },
                    { name: 'ID        ', label: '<%=lang["colID        "]%>', colType: 'ID' },
                    { name: 'CreateTime', label: '<%=lang["colCreateTime"]%>', fixed: true, align: 'left', colType: 'DateTime2', nowrap: false },
<% if (this.type == BU.NoteTypes.Events) {%>
                    { name: 'NoteState ', label: '<%=lang["colNoteState "]%>', width: 040, colType: 'Locked', editoptions: {<%=enumlist<BU.NoteStates>("value")%> } }, <% } %>
                    { name: 'Note      ', label: '<%=lang["colNote      "]%>', width: 300, sorttype: 'text', align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3" } },
                    { name: 'NoteType  ', label: '<%=lang["colNoteType  "]%>', width: 035, sorttype: 'int', editable: true, formatter: 'select', edittype: 'select', editoptions: { defaultValue: type,<%=enumlist<BU.NoteTypes>("value")%> } },
                    { name: 'CreateUser', label: '<%=lang["colCreateUser"]%>', fixed: true, colType: 'ACNT2' },
                    { name: 'ModifyTime', label: '<%=lang["colModifyTime"]%>', fixed: true, align: 'left', colType: 'DateTime2', nowrap: false },
                    { name: 'ModifyUser', label: '<%=lang["colModifyUser"]%>', fixed: true, colType: 'ACNT2' },
                ],
            });

            $('#btnAdd').button({ icons: { primary: 'ui-icon-plus' } }).click($table[0].addRow).css('border', 0);
            $(window).resize(function () { $table.gridSize(window); }).trigger('resize');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div id="nav1">
        <button id="btnAdd"><%=lang["btnAdd"]%></button>
    </div>
    <table id="table01">
        <tr class="colModel">
            <td name="Action">
                <span property="action">
                    <div class="edithide" action="editRow"    icon="ui-icon-pencil"><%=lang["Actions_Edit"]%></div>
                    <div class="editshow" action="restoreRow" icon="ui-icon-cancel"><%=lang["Actions_Cancel"]%></div>
                    <div class="editshow" action="saveRow"    icon="ui-icon-disk"  ><%=lang["Actions_Save"]%></div>
                </span>
            </td>
        </tr>
    </table>
    <div id="nav2" class="ui-widget-content" style=""></div>
</asp:Content>

