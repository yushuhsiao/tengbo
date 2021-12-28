<%@ Page Title="" Language="C#" MasterPageFile="~/sys/root.Master" AutoEventWireup="true" Inherits="web.page" %>

<%@ Register Assembly="admin" Namespace="web.Controls" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="include" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <cc1:DataGrid ID="DataGrid1" runat="server">
        <Toolbar>
            <cc1:DataGridColumn ID="DataGridColumn8" runat="server" />
            <cc1:DataGridColumn ID="DataGridColumn9" runat="server" />
        </Toolbar>
        <Columns>
            <cc1:DataGridColumn runat="server" Name="ID" IsKey="true">
                <Header />
                <Filter />
                <Viewer />
                <Editor />
            </cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="CorpID" Text_id="colCorpID" Width="3%"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="ACNT"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="GroupID"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="ParentID"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Name"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Locked"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Currency"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Balance"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Birthday"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Tel"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Mail"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="QQ"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Memo"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Introducer"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Sex"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="Addr"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="UserMemo"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="RegisterIP"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="CreateTime"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="CreateUser"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="ModifyTime"></cc1:DataGridColumn>
            <cc1:DataGridColumn runat="server" Name="ModifyUser"></cc1:DataGridColumn>
        </Columns>
        <Pager a1="1" />
    </cc1:DataGrid>
</asp:Content>
