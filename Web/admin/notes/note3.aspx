<%@ Page Title="" Language="C#" MasterPageFile="notes.master" AutoEventWireup="true" Inherits="web.page" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        ((notes_master)this.Master).type = BU.NoteTypes.SystemUpdate;
    }
</script>


