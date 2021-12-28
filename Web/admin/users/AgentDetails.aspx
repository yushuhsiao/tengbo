<%@ Page Title="" Language="C#" MasterPageFile="UserDetail.master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="web.UserDetails_page" %>

<script runat="server">
    public override BU.UserType UserType { get { return BU.UserType.Agent; } }
</script>
